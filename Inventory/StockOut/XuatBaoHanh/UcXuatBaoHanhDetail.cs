using Bll.Inventory.StockInOut;
using Bll.MasterData.ProductServiceBll;
using Common.Common;
using Common.Helpers;
using Common.Utils;
using DevExpress.Data;
using DTO.Inventory;
using DTO.MasterData.ProductService;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DTO.Inventory.StockInOut;

namespace Inventory.StockOut.XuatBaoHanh
{

    public partial class UcXuatBaoHanhDetail : DevExpress.XtraEditors.XtraUserControl
    {
        #region ========== FIELDS & PROPERTIES ==========

        /// <summary>
        /// Business Logic Layer cho biến thể sản phẩm
        /// </summary>
        private readonly ProductVariantBll _productVariantBll = new ProductVariantBll();


        /// <summary>
        /// Business Logic Layer cho nhập xuất kho
        /// </summary>
        private readonly StockInOutBll _stockInOutBll = new StockInOutBll();

        
        /// <summary>
        /// Logger để ghi log các sự kiện
        /// </summary>
        private readonly ILogger _logger = LoggerFactory.CreateLogger(LogCategory.UI);

        /// <summary>
        /// Trạng thái đang tải dữ liệu (guard tránh gọi LoadProductVariantsAsync song song)
        /// </summary>
        private bool _isLoadingProductVariants;

        /// <summary>
        /// Flag đánh dấu ProductVariant datasource đã được load chưa
        /// </summary>
        private bool _isProductVariantDataSourceLoaded;

        /// <summary>
        /// Trạng thái đang tính toán (guard tránh tính toán lặp vô hạn)
        /// </summary>
        private bool _isCalculating;

        /// <summary>
        /// ID phiếu nhập kho master (dùng để bind vào các detail rows)
        /// </summary>
        private Guid _stockInMasterId = Guid.Empty;

        #endregion

        #region ========== CONSTRUCTOR ==========

        public UcXuatBaoHanhDetail()
        {
            InitializeComponent();
            InitializeControl();
        }

        #endregion

        #region ========== INITIALIZATION ==========

        /// <summary>
        /// Khởi tạo control
        /// </summary>
        private void InitializeControl()
        {
            try
            {
                // GridView đã được khai báo trong Designer, property public sẽ expose nó

                // Khởi tạo binding source với danh sách rỗng
                stockInOutDetailForUIDtoBindingSource.DataSource = new List<StockInOutDetailForUIDto>();

                // Setup events
                InitializeEvents();

                // Không load dữ liệu ProductVariant ở đây, sẽ được gọi từ form khi FormLoad
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khởi tạo control");
            }
        }

        /// <summary>
        /// Khởi tạo các event handlers
        /// </summary>
        private void InitializeEvents()
        {
            // Event khi thay đổi cell value (xử lý cả ProductVariant và tính toán)
            XuatBaoHanhDetailDtoGridView.CellValueChanged += XuatBaoHanhDetailDtoGridView_CellValueChanged;

            // Event khi thêm/xóa dòng để cập nhật LineNumber
            XuatBaoHanhDetailDtoGridView.InitNewRow += XuatBaoHanhDetailDtoGridView_InitNewRow;
            XuatBaoHanhDetailDtoGridView.RowDeleted += XuatBaoHanhDetailDtoGridView_RowDeleted;

            // Event khi validate cell và row
            XuatBaoHanhDetailDtoGridView.ValidateRow += XuatBaoHanhDetailDtoGridView_ValidateRow;
            XuatBaoHanhDetailDtoGridView.ValidatingEditor += XuatBaoHanhDetailDtoGridView_ValidatingEditor;

            // Event custom draw row indicator
            XuatBaoHanhDetailDtoGridView.CustomDrawRowIndicator += XuatBaoHanhDetailDtoGridView_CustomDrawRowIndicator;

            // Event xử lý phím tắt cho GridView (Insert, Delete, Enter)
            XuatBaoHanhDetailDtoGridView.KeyDown += XuatBaoHanhDetailDtoGridView_KeyDown;

            // Event Popup cho ProductVariantSearchLookUpEdit (RepositoryItem)
            ProductVariantSearchLookUpEdit.Popup += ProductVariantSearchLookUpEdit_Popup;
        }

        #endregion

        #region ========== PUBLIC METHODS ==========

        /// <summary>
        /// Lấy danh sách chi tiết từ grid
        /// </summary>
        public List<StockInOutDetailForUIDto> GetDetails()
        {
            try
            {
                //Không cast trực tiếp mà lặp từng phần tử trong binding source để tránh lỗi ambiguous call
                var details = new List<StockInOutDetailForUIDto>();

                foreach (var item in stockInOutDetailForUIDtoBindingSource)
                {
                    if (item is not StockInOutDetailForUIDto detailDto) continue;

                    details.Add(new StockInOutDetailForUIDto
                    {
                        Id = default,
                        ProductVariantId = detailDto.ProductVariantId,
                        StockInQty = detailDto.StockInQty,
                        StockOutQty = detailDto.StockOutQty,
                        UnitPrice = detailDto.UnitPrice,
                        Vat = detailDto.Vat,

                        GhiChu = detailDto.GhiChu,
                    });
                }

                return details;
            }
            catch (Exception ex)
            {
                _logger.Error("GetDetails: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi lấy danh sách chi tiết: {ex.Message}");
                return new List<StockInOutDetailForUIDto>();
            }
        }


        /// <summary>
        /// Tính tổng số lượng, tiền từ các detail DTOs
        /// Trả về tuple (totalQuantity, totalAmount, totalVat, totalAmountIncludedVat)
        /// Lưu ý: Xuất bảo hành chỉ tính số lượng, các giá trị tiền sẽ là 0
        /// </summary>
        public (decimal totalQuantity, decimal totalAmount, decimal totalVat, decimal totalAmountIncludedVat)
            CalculateTotals()
        {
            try
            {
                var details = stockInOutDetailForUIDtoBindingSource.Cast<StockInOutDetailForUIDto>().ToList();

                // Tính tổng số lượng từ các detail DTOs
                var totalQuantity = details.Sum(d => d.StockInQty);

                // Xuất bảo hành không tính tiền, nên các giá trị tiền là 0
                var totalAmount = 0m;
                var totalVat = 0m;
                var totalAmountIncludedVat = 0m;

                return (totalQuantity, totalAmount, totalVat, totalAmountIncludedVat);
            }
            catch (Exception ex)
            {
                _logger.Error("CalculateTotals: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi tính tổng: {ex.Message}");
                return (0m, 0m, 0m, 0m);
            }
        }

        /// <summary>
        /// Clear dữ liệu và reset về trạng thái ban đầu
        /// </summary>
        public void ClearData()
        {
            try
            {
                stockInOutDetailForUIDtoBindingSource.DataSource = new List<StockInOutDetailForUIDto>();
                stockInOutDetailForUIDtoBindingSource.ResetBindings(false);
                _stockInMasterId = Guid.Empty;

                // Reset cache flag để load lại khi cần
                _isProductVariantDataSourceLoaded = false;
            }
            catch (Exception ex)
            {
                _logger.Error("ClearData: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi xóa dữ liệu: {ex.Message}");
            }
        }

        /// <summary>
        /// Load dữ liệu detail từ ID phiếu nhập xuất kho, sử dụng trong trường hợp được gọi từ nút "Chi Tiết" <br/>
        /// từ màn hình lịch sử nhập xuất
        /// </summary>
        /// <param name="stockInOutMasterId">ID phiếu nhập xuất kho</param>
        public Task LoadDataAsyncForEdit(Guid stockInOutMasterId)
        {
            try
            {
                // Set master ID
                _stockInMasterId = stockInOutMasterId;

                // Lấy detail DTOs từ BLL (không sử dụng DataContext trực tiếp tại UI layer)
                var detailDtos = _stockInOutBll.GetStockInOutDetailsByMasterId(stockInOutMasterId);

                // Load details vào UI
                LoadDetails(detailDtos);
            }
            catch (Exception ex)
            {
                _logger.Error("LoadDataAsync: Exception occurred, masterId={0}", ex, stockInOutMasterId);
                MsgBox.ShowError($"Lỗi tải danh sách chi tiết: {ex.Message}");
                throw;
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Set StockInMasterId để bind vào các detail rows
        /// </summary>
        public void SetStockInMasterId(Guid stockInMasterId)
        {
            try
            {
                _stockInMasterId = stockInMasterId;

                // Cập nhật StockInOutMasterId cho tất cả các dòng hiện có
                var details = stockInOutDetailForUIDtoBindingSource.Cast<StockInOutDetailForUIDto>().ToList();
                foreach (var detail in details)
                {
                    if (detail.StockInOutMasterId == Guid.Empty)
                    {
                        detail.StockInOutMasterId = stockInMasterId;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("SetStockInMasterId: Exception occurred, masterId={0}", ex, stockInMasterId);
                MsgBox.ShowError($"Lỗi set master ID: {ex.Message}");
            }
        }

        /// <summary>
        /// Reload ProductVariant datasource (public method để gọi từ Form)
        /// </summary>
        public async Task ReloadProductVariantDataSourceAsync()
        {
            try
            {
                await LoadProductVariantsAsync(forceRefresh: true);
            }
            catch (Exception ex)
            {
                _logger.Error("ReloadProductVariantDataSourceAsync: Exception occurred", ex);
                throw;
            }
        }

        /// <summary>
        /// Validate tất cả các dòng trong grid
        /// </summary>
        public bool ValidateAll()
        {
            try
            {
                var details = stockInOutDetailForUIDtoBindingSource.Cast<StockInOutDetailForUIDto>().ToList();

                if (details.Count == 0)
                {
                    _logger.Warning("ValidateAll: No details found");
                    MsgBox.ShowWarning("Vui lòng thêm ít nhất một dòng chi tiết");
                    return false;
                }

                foreach (var detail in details)
                {
                    var validationResults = new List<ValidationResult>();
                    var validationContext = new ValidationContext(detail);
                    var isValid = Validator.TryValidateObject(detail, validationContext, validationResults, true);

                    if (!isValid)
                    {
                        var errors = string.Join("\n", validationResults.Select(r => r.ErrorMessage));
                        _logger.Warning("ValidateAll: Row {0} validation failed, Errors={1}", detail.LineNumber,
                            errors);
                        MsgBox.ShowError($"Dòng {detail.LineNumber} có lỗi:\n{errors}");
                        return false;
                    }

                    // Validate business rules
                    if (detail.ProductVariantId == Guid.Empty)
                    {
                        _logger.Warning("ValidateAll: Row {0} has empty ProductVariantId", detail.LineNumber);
                        MsgBox.ShowError($"Dòng {detail.LineNumber}: Vui lòng chọn hàng hóa");
                        return false;
                    }

                    if (detail.StockInQty > 0) continue;

                    _logger.Warning("ValidateAll: Row {0} has StockInQty <= 0, value={1}", detail.LineNumber,
                        detail.StockInQty);
                    MsgBox.ShowError($"Dòng {detail.LineNumber}: Số lượng nhập phải lớn hơn 0");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.Error("ValidateAll: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi validate: {ex.Message}");
                return false;
            }
        }

        #endregion

        #region ========== PUBLIC EVENTS ==========

        /// <summary>
        /// Event được trigger khi dữ liệu detail thay đổi (để cập nhật tổng tiền lên master)
        /// </summary>
        public event EventHandler DetailDataChanged;

        /// <summary>
        /// Trigger event DetailDataChanged
        /// </summary>
        protected virtual void OnDetailDataChanged()
        {
            DetailDataChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region ========== GRIDVIEW EVENT HANDLERS ==========

        /// <summary>
        /// Custom draw row indicator để hiển thị số thứ tự
        /// </summary>
        private void XuatBaoHanhDetailDtoGridView_CustomDrawRowIndicator(object sender,
            DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            GridViewHelper.CustomDrawRowIndicator(XuatBaoHanhDetailDtoGridView, e);
        }

        /// <summary>
        /// Xử lý sự kiện khi giá trị cell thay đổi trong GridView
        /// Xử lý cả việc cập nhật ProductVariant và tính toán tự động
        /// </summary>
        private void XuatBaoHanhDetailDtoGridView_CellValueChanged(object sender,
            DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                var fieldName = e.Column?.FieldName;
                var rowHandle = e.RowHandle;

                if (rowHandle < 0)
                {
                    return; // Bỏ qua new row
                }

                // Lấy row data từ GridView
                var rowData = XuatBaoHanhDetailDtoGridView.GetRow(rowHandle) as StockInOutDetailForUIDto;
                if (rowData == null)
                {
                    _logger.Warning("CellValueChanged: Row data is null, RowHandle={0}", rowHandle);
                    return;
                }

                // Xử lý khi chọn ProductVariant
                if (fieldName == "ProductVariantId")
                {
                    // Lấy ProductVariantId từ cell value
                    Guid productVariantId;
                    if (e.Value is Guid guidValue)
                    {
                        productVariantId = guidValue;
                    }
                    else if (e.Value != null && Guid.TryParse(e.Value.ToString(), out var parsedGuid))
                    {
                        productVariantId = parsedGuid;
                    }
                    else
                    {
                        return; // Không có giá trị hợp lệ
                    }

                    if (productVariantId == Guid.Empty)
                    {
                        return;
                    }

                    // Tìm ProductVariantDto trong binding source
                    var selectedVariant = productVariantListDtoBindingSource.Cast<ProductVariantDto>()
                        .FirstOrDefault(v => v.Id == productVariantId);

                    if (selectedVariant == null)
                    {
                        _logger.Warning("CellValueChanged: ProductVariant not found, Id={0}", productVariantId);
                        return;
                    }

                    // Cập nhật các thông tin liên quan
                    rowData.ProductVariantId = selectedVariant.Id;
                    rowData.ProductVariantCode = selectedVariant.VariantCode;
                    rowData.ProductVariantName = $"{selectedVariant.VariantFullName}";
                    rowData.UnitOfMeasureName = selectedVariant.UnitName;
                }

                // Xử lý tính toán tự động khi thay đổi số lượng
                if (fieldName == "StockInQty")
                {
                    if (_isCalculating)
                    {
                        return; // Tránh tính toán lặp vô hạn
                    }

                    _isCalculating = true;

                    try
                    {
                        // Cập nhật tổng số lượng lên master (nếu có event handler)
                        OnDetailDataChanged();
                    }
                    finally
                    {
                        _isCalculating = false;
                    }
                }

                // Refresh grid để hiển thị thay đổi
                XuatBaoHanhDetailDtoGridView.RefreshRow(rowHandle);
            }
            catch (Exception ex)
            {
                _logger.Error("CellValueChanged: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi xử lý thay đổi cell: {ex.Message}");
            }
        }

        /// <summary>
        /// Xử lý sự kiện khi thêm dòng mới
        /// </summary>
        private void XuatBaoHanhDetailDtoGridView_InitNewRow(object sender,
            DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs e)
        {
            try
            {
                var rowData = XuatBaoHanhDetailDtoGridView.GetRow(e.RowHandle) as StockInOutDetailForUIDto;
                if (rowData == null)
                {
                    _logger.Warning("InitNewRow: Row data is null, RowHandle={0}", e.RowHandle);
                    return;
                }

                // Gán ID mới nếu chưa có
                if (rowData.Id == Guid.Empty)
                {
                    rowData.Id = Guid.NewGuid();
                }

                // Gán StockInOutMasterId nếu chưa có
                if (rowData.StockInOutMasterId == Guid.Empty && _stockInMasterId != Guid.Empty)
                {
                    rowData.StockInOutMasterId = _stockInMasterId;
                }

                // Trigger event để cập nhật tổng lên master khi thêm dòng mới
                OnDetailDataChanged();
            }
            catch (Exception ex)
            {
                _logger.Error("InitNewRow: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi thêm dòng: {ex.Message}");
            }
        }

        /// <summary>
        /// Xử lý sự kiện khi xóa dòng
        /// </summary>
        private void XuatBaoHanhDetailDtoGridView_RowDeleted(object sender, RowDeletedEventArgs rowDeletedEventArgs)
        {
            try
            {
                // Cập nhật tổng tiền lên master
                OnDetailDataChanged();
            }
            catch (Exception ex)
            {
                _logger.Error("RowDeleted: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi xóa dòng: {ex.Message}");
            }
        }

        /// <summary>
        /// Xử lý sự kiện validate editor (trước khi commit giá trị)
        /// </summary>
        private void XuatBaoHanhDetailDtoGridView_ValidatingEditor(object sender,
            DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            try
            {
                var column = XuatBaoHanhDetailDtoGridView.FocusedColumn;
                var fieldName = column?.FieldName;
                var rowHandle = XuatBaoHanhDetailDtoGridView.FocusedRowHandle;

                if (string.IsNullOrEmpty(fieldName)) return;

                switch (fieldName)
                {
                    // Xử lý ProductVariantId: Cập nhật tên và đơn vị tính
                    case "ProductVariantId":
                        HandleProductVariantIdChange(e);
                        break;
                    // Validate StockInQty: Phải lớn hơn 0
                    case "StockInQty":
                        ValidateStockInQty(e);
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.Error("ValidatingEditor: Exception occurred", ex);
                e.ErrorText = $"Lỗi validate: {ex.Message}";
                e.Valid = false;
            }
        }

        /// <summary>
        /// Validate row data (sau khi commit giá trị)
        /// </summary>
        private void XuatBaoHanhDetailDtoGridView_ValidateRow(object sender,
            DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            try
            {
                var rowData = e.Row as StockInOutDetailForUIDto;
                if (rowData == null)
                {
                    _logger.Warning("ValidateRow: Row data is null");
                    e.Valid = false;
                    e.ErrorText = "Dữ liệu không hợp lệ";
                    return;
                }

                // Validate bằng DataAnnotations
                var validationResults = new List<ValidationResult>();
                var validationContext = new ValidationContext(rowData);
                var isValid = Validator.TryValidateObject(rowData, validationContext, validationResults, true);

                if (!isValid)
                {
                    var errors = string.Join("\n", validationResults.Select(r => r.ErrorMessage));
                    _logger.Warning("ValidateRow: DataAnnotations validation failed, Errors={0}", errors);
                    e.Valid = false;
                    e.ErrorText = errors;
                    return;
                }

                // Validate business rules
                if (rowData.ProductVariantId == Guid.Empty)
                {
                    _logger.Warning("ValidateRow: ProductVariantId is Empty");
                    e.Valid = false;
                    e.ErrorText = "Vui lòng chọn hàng hóa";
                    return;
                }

                if (rowData.StockInQty <= 0)
                {
                    _logger.Warning("ValidateRow: StockInQty <= 0, value={0}", rowData.StockInQty);
                    e.Valid = false;
                    e.ErrorText = "Số lượng nhập phải lớn hơn 0";
                    return;
                }

                e.Valid = true;

                // Trigger event để cập nhật tổng lên master sau khi row được validate thành công
                OnDetailDataChanged();
            }
            catch (Exception ex)
            {
                _logger.Error("ValidateRow: Exception occurred", ex);
                e.Valid = false;
                e.ErrorText = $"Lỗi validate: {ex.Message}";
            }
        }

        /// <summary>
        /// Event handler xử lý phím tắt trong GridView
        /// </summary>
        private void XuatBaoHanhDetailDtoGridView_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                var gridView = XuatBaoHanhDetailDtoGridView;
                if (gridView == null) return;

                switch (e.KeyCode)
                {
                    case Keys.Insert:
                        // Insert: Thêm dòng mới
                        if (!e.Control && !e.Shift && !e.Alt)
                        {
                            e.Handled = true;

                            // Thêm dòng mới
                            gridView.AddNewRow();

                            // Focus vào cột ProductVariantCode để người dùng có thể chọn ngay
                            var productVariantColumn = gridView.Columns["ProductVariantCode"];
                            if (productVariantColumn != null)
                            {
                                gridView.FocusedColumn = productVariantColumn;
                            }
                        }

                        break;

                    case Keys.Delete:
                        // Delete: Xóa dòng được chọn
                        if (!e.Control && !e.Shift && !e.Alt)
                        {
                            var focusedRowHandle = gridView.FocusedRowHandle;
                            if (focusedRowHandle >= 0)
                            {
                                e.Handled = true;

                                // Xóa dòng
                                gridView.DeleteRow(focusedRowHandle);
                            }
                        }

                        break;

                    case Keys.Enter:
                        // Enter: Hoàn thành nhập dòng (commit row)
                        if (!e.Control && !e.Shift && !e.Alt)
                        {
                            var focusedRowHandle = gridView.FocusedRowHandle;
                                
                            // Nếu đang ở new row (rowHandle < 0), commit row
                            if (focusedRowHandle == DevExpress.XtraGrid.GridControl.NewItemRowHandle)
                            {
                                e.Handled = true;
                                // Validate row trước khi commit
                                if (gridView.PostEditor())
                                {
                                    gridView.UpdateCurrentRow();
                                }
                            }
                            // Nếu đang ở dòng đã có, di chuyển xuống dòng tiếp theo hoặc thêm dòng mới
                            else if (focusedRowHandle >= 0)
                            {
                                // Nếu đang ở cột cuối cùng, di chuyển xuống dòng tiếp theo
                                var focusedColumn = gridView.FocusedColumn;
                                var lastColumn = gridView.VisibleColumns[gridView.VisibleColumns.Count - 1];
                                    
                                if (focusedColumn == lastColumn)
                                {
                                    e.Handled = true;
                                        
                                    // Di chuyển xuống dòng tiếp theo hoặc thêm dòng mới
                                    var nextRowHandle = focusedRowHandle + 1;
                                    if (nextRowHandle < gridView.RowCount)
                                    {
                                        gridView.FocusedRowHandle = nextRowHandle;
                                        gridView.FocusedColumn = gridView.Columns[0]; // Focus vào cột đầu tiên
                                    }
                                    else
                                    {
                                        // Thêm dòng mới
                                        gridView.AddNewRow();
                                        var productVariantColumn = gridView.Columns["ProductVariantCode"];
                                        if (productVariantColumn != null)
                                        {
                                            gridView.FocusedColumn = productVariantColumn;
                                        }
                                    }
                                }
                            }
                        }

                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.Error("XuatBaoHanhDetailDtoGridView_KeyDown: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi xử lý phím tắt: {ex.Message}");
            }
        }

        #endregion

        #region ========== PRODUCT VARIANT MANAGEMENT ==========

        /// <summary>
        /// Load danh sách biến thể sản phẩm vào SearchLookUpEdit
        /// Method này được gọi từ form khi FormLoad
        /// Sử dụng SplashScreen để tăng trải nghiệm người dùng và ngăn người dùng thao tác khi đang load data
        /// </summary>
        /// <param name="forceRefresh">Nếu true, sẽ load lại từ database ngay cả khi đã load trước đó</param>
        private async Task LoadProductVariantsAsync(bool forceRefresh = false)
        {
            if (_isLoadingProductVariants) return;
            _isLoadingProductVariants = true;

            try
            {
                // Nếu đã load và không force refresh, không load lại
                if (_isProductVariantDataSourceLoaded && !forceRefresh &&
                    productVariantListDtoBindingSource.DataSource != null &&
                    productVariantListDtoBindingSource.DataSource is List<ProductVariantDto> existingList &&
                    existingList.Count > 0)
                {
                    return;
                }

                // Hiển thị SplashScreen để thông báo đang load dữ liệu
                SplashScreenHelper.ShowWaitingSplashScreen();

                try
                {
                    productVariantListDtoBindingSource.DataSource =
                        await _productVariantBll.GetAllInUseWithDetailsAsync();
                    productVariantListDtoBindingSource.ResetBindings(false);

                    _isProductVariantDataSourceLoaded = true;
                }
                finally
                {
                    // Đóng SplashScreen sau khi hoàn thành hoặc có lỗi
                    SplashScreenHelper.CloseSplashScreen();
                }
            }
            catch (Exception ex)
            {
                _logger.Error("LoadProductVariantsAsync: Exception occurred", ex);
                SplashScreenHelper.CloseSplashScreen(); // Đảm bảo đóng SplashScreen khi có lỗi
                MsgBox.ShowError($"Lỗi tải danh sách biến thể sản phẩm: {ex.Message}");
            }
            finally
            {
                _isLoadingProductVariants = false;
            }
        }

        /// <summary>
        /// Event handler khi ProductVariantSearchLookUpEdit popup
        /// Chỉ load dữ liệu nếu chưa load hoặc datasource rỗng
        /// </summary>
        private async void ProductVariantSearchLookUpEdit_Popup(object sender, EventArgs e)
        {
            try
            {
                // Chỉ load nếu chưa load hoặc datasource rỗng
                if (!_isProductVariantDataSourceLoaded ||
                    productVariantListDtoBindingSource.DataSource == null ||
                    (productVariantListDtoBindingSource.DataSource is List<ProductVariantDto> list &&
                     list.Count == 0))
                {
                    await LoadProductVariantsAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.Error("ProductVariantSearchLookUpEdit_Popup: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi tải dữ liệu biến thể sản phẩm: {ex.Message}");
            }
        }

        /// <summary>
        /// Load chỉ các ProductVariant theo danh sách ID từ details
        /// Chỉ load các ProductVariant cần thiết để tối ưu performance
        /// </summary>
        /// <param name="details">Danh sách StockInOutDetailForUIDto chứa ProductVariantId</param>
        private async Task LoadProductVariantsByIdsAsync(List<StockInOutDetailForUIDto> details)
        {
            try
            {
                if (details == null || details.Count == 0)
                {
                    return;
                }

                // Lấy danh sách ProductVariantId duy nhất từ details
                var productVariantIds = details
                    .Where(d => d.ProductVariantId != Guid.Empty)
                    .Select(d => d.ProductVariantId)
                    .Distinct()
                    .ToList();

                if (productVariantIds.Count == 0)
                {
                    return;
                }

                var variantDtos = new List<ProductVariantDto>();
                foreach (var productVariantId in productVariantIds)
                {
                    // Lấy dữ liệu DTO từ BLL với thông tin đầy đủ
                    var variant = await _productVariantBll.GetByIdAsync(productVariantId);
                    if (variant != null)
                    {
                        variantDtos.Add(variant);
                    }
                }

                // Bind dữ liệu vào BindingSource
                productVariantListDtoBindingSource.DataSource = variantDtos;
                productVariantListDtoBindingSource.ResetBindings(false);
            }
            catch (Exception ex)
            {
                _logger.Error("LoadProductVariantsByIdsAsync: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi tải biến thể sản phẩm: {ex.Message}");
            }
        }


        #endregion

        #region ========== DATA MANAGEMENT ==========

        /// <summary>
        /// Load danh sách chi tiết từ danh sách DTO
        /// </summary>
        private async void LoadDetails(List<StockInOutDetailForUIDto> details)
        {
            try
            {
                details ??= new List<StockInOutDetailForUIDto>();

                // Gán StockInOutMasterId cho các dòng chưa có
                foreach (var detail in details)
                {
                    if (detail.StockInOutMasterId == Guid.Empty && _stockInMasterId != Guid.Empty)
                    {
                        detail.StockInOutMasterId = _stockInMasterId;
                    }
                }

                stockInOutDetailForUIDtoBindingSource.DataSource = details;
                stockInOutDetailForUIDtoBindingSource.ResetBindings(false);

                // Load ProductVariant datasource chỉ cho các ProductVariantId có trong details
                await LoadProductVariantsByIdsAsync(details);

                // Tính toán lại tất cả
                RecalculateAll();
            }
            catch (Exception ex)
            {
                _logger.Error("LoadDetails: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi tải danh sách chi tiết: {ex.Message}");
            }
        }

        /// <summary>
        /// Thêm hoặc merge danh sách chi tiết mới vào grid hiện tại
        /// Nếu đã có ProductVariantId thì cộng số lượng, nếu chưa có thì thêm mới
        /// </summary>
        /// <param name="newDetails">Danh sách chi tiết mới cần thêm/merge</param>
        public async Task AddOrMergeDetailsAsync(List<StockInOutDetailForUIDto> newDetails)
        {
            try
            {
                if (newDetails == null || newDetails.Count == 0)
                {
                    return;
                }

                // Lấy danh sách hiện tại từ binding source
                var currentDetails = stockInOutDetailForUIDtoBindingSource.Cast<StockInOutDetailForUIDto>().ToList();

                // Gán StockInOutMasterId cho các dòng mới chưa có
                foreach (var detail in newDetails)
                {
                    if (detail.StockInOutMasterId == Guid.Empty && _stockInMasterId != Guid.Empty)
                    {
                        detail.StockInOutMasterId = _stockInMasterId;
                    }
                }

                // Merge dữ liệu: nếu đã có ProductVariantId thì cộng số lượng, nếu chưa có thì thêm mới
                foreach (var newDetail in newDetails)
                {
                    var existingDetail = currentDetails.FirstOrDefault(d => d.ProductVariantId == newDetail.ProductVariantId);
                    
                    if (existingDetail != null)
                    {
                        // Đã tồn tại: cộng số lượng và merge ghi chú
                        existingDetail.StockOutQty += newDetail.StockOutQty;
                        
                        // Merge ghi chú nếu có
                        if (!string.IsNullOrWhiteSpace(newDetail.GhiChu))
                        {
                            if (string.IsNullOrWhiteSpace(existingDetail.GhiChu))
                            {
                                existingDetail.GhiChu = newDetail.GhiChu;
                            }
                            else
                            {
                                existingDetail.GhiChu = $"{existingDetail.GhiChu}; {newDetail.GhiChu}";
                            }
                        }
                    }
                    else
                    {
                        // Chưa tồn tại: thêm mới
                        currentDetails.Add(newDetail);
                    }
                }

                // Cập nhật lại binding source
                stockInOutDetailForUIDtoBindingSource.DataSource = currentDetails;
                stockInOutDetailForUIDtoBindingSource.ResetBindings(false);

                // Load ProductVariant datasource cho các ProductVariantId mới
                await LoadProductVariantsByIdsAsync(newDetails);

                // Tính toán lại tất cả
                RecalculateAll();
            }
            catch (Exception ex)
            {
                _logger.Error("AddOrMergeDetailsAsync: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi thêm/merge chi tiết: {ex.Message}");
            }
        }

        #endregion

        #region ========== CALCULATION ==========

        /// <summary>
        /// Tính toán lại tất cả các dòng
        /// </summary>
        private void RecalculateAll()
        {
            try
            {
                if (_isCalculating) return;
                _isCalculating = true;

                XuatBaoHanhDetailDtoGridView.RefreshData();

                // Cập nhật tổng tiền lên master
                OnDetailDataChanged();
            }
            catch (Exception ex)
            {
                _logger.Error("RecalculateAll: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi tính toán lại: {ex.Message}");
            }
            finally
            {
                _isCalculating = false;
            }
        }

        #endregion

        #region ========== VALIDATION ==========

        /// <summary>
        /// Xử lý khi thay đổi ProductVariantId: Cập nhật tên và đơn vị tính
        /// </summary>
        private void HandleProductVariantIdChange(
            DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            // Lấy ProductVariantId mới từ editor value
            Guid productVariantId;
            if (e.Value is Guid guidValue)
            {
                productVariantId = guidValue;
            }
            else if (e.Value != null && Guid.TryParse(e.Value.ToString(), out var parsedGuid))
            {
                productVariantId = parsedGuid;
            }
            else
            {
                return;
            }

            if (productVariantId == Guid.Empty)
            {
                return;
            }

            // Tìm ProductVariantDto trong binding source
            var selectedVariant = productVariantListDtoBindingSource.Cast<ProductVariantDto>()
                .FirstOrDefault(v => v.Id == productVariantId);

            if (selectedVariant == null)
            {
                _logger.Warning("HandleProductVariantIdChange: ProductVariant not found, Id={0}", productVariantId);
                e.ErrorText = "Không tìm thấy biến thể sản phẩm";
                e.Valid = false;
                return;
            }

            var rowHandle = XuatBaoHanhDetailDtoGridView.FocusedRowHandle;

            // Xử lý cả new row (rowHandle < 0) và existing row (rowHandle >= 0)
            if (rowHandle < 0)
            {
                // New row: Sử dụng SetFocusedRowCellValue để set giá trị vào các cell
                // Các giá trị này sẽ được commit khi row được thêm vào binding source
                // Quan trọng: Phải set ProductVariantId trước để khi validate row, giá trị này đã có
                XuatBaoHanhDetailDtoGridView.SetFocusedRowCellValue("ProductVariantId", selectedVariant.Id);
                XuatBaoHanhDetailDtoGridView.SetFocusedRowCellValue("ProductVariantCode", selectedVariant.VariantCode);
                XuatBaoHanhDetailDtoGridView.SetFocusedRowCellValue("ProductVariantName",
                    $"{selectedVariant.VariantFullName}");
                XuatBaoHanhDetailDtoGridView.SetFocusedRowCellValue("UnitOfMeasureName", selectedVariant.UnitName);
            }
            else
            {
                // Existing row: Cập nhật trực tiếp vào row data
                if (XuatBaoHanhDetailDtoGridView.GetRow(rowHandle) is StockInOutDetailForUIDto rowData)
                {
                    rowData.ProductVariantId = selectedVariant.Id;
                    rowData.ProductVariantCode = selectedVariant.VariantCode;
                    rowData.ProductVariantName = $"{selectedVariant.VariantFullName}";
                    rowData.UnitOfMeasureName = selectedVariant.UnitName;

                    // Refresh grid để hiển thị thay đổi
                    XuatBaoHanhDetailDtoGridView.RefreshRow(rowHandle);
                }
            }
        }

        /// <summary>
        /// Validate StockInQty: Phải lớn hơn 0
        /// </summary>
        private void ValidateStockInQty(DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            if (e.Value == null)
            {
                e.ErrorText = "Số lượng nhập không được để trống";
                e.Valid = false;
                return;
            }

            if (decimal.TryParse(e.Value.ToString(), out var stockInQty))
            {
                if (stockInQty <= 0)
                {
                    _logger.Warning("ValidateStockInQty: Value <= 0, value={0}", stockInQty);
                    e.ErrorText = "Số lượng nhập phải lớn hơn 0";
                    e.Valid = false;
                    return;
                }
            }
            else
            {
                _logger.Warning("ValidateStockInQty: Invalid number format, value={0}", e.Value);
                e.ErrorText = "Số lượng nhập phải là số hợp lệ";
                e.Valid = false;
                return;
            }

            e.Valid = true;
        }

        #endregion

        #region ========== HELPER METHODS ==========

        /// <summary>
        /// Hiển thị lỗi
        /// </summary>
        private void ShowError(Exception ex, string message)
        {
            _logger.Error("ShowError: {0}", ex, message);
            MsgBox.ShowError($"{message}: {ex.Message}");
        }

        /// <summary>
        /// Hiển thị cảnh báo
        /// </summary>
        private void ShowWarning(string message)
        {
            _logger.Warning("ShowWarning: {0}", message);
            MsgBox.ShowWarning(message);
        }

        #endregion
    }
}