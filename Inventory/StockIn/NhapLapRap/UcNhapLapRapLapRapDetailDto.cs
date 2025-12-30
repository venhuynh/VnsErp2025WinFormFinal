using Bll.Inventory.StockInOut;
using Bll.MasterData.ProductServiceBll;
using Common.Common;
using Common.Helpers;
using Common.Utils;
using Dal.DataContext;
using DevExpress.Data;
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

namespace Inventory.StockIn.NhapLapRap
{
    public partial class UcNhapLapRapLapRapDetailDto : DevExpress.XtraEditors.XtraUserControl
    {
        #region ========== FIELDS & PROPERTIES ==========

        /// <summary>
        /// Business Logic Layer cho biến thể sản phẩm
        /// </summary>
        private readonly ProductVariantBll _productVariantBll = new();

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
        /// ID phiếu nhập lắp ráp master (dùng để bind vào các detail rows)
        /// </summary>
        private Guid _stockInMasterId = Guid.Empty;

        #endregion

        #region ========== CONSTRUCTOR ==========

        public UcNhapLapRapLapRapDetailDto()
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
            NhapLapRapDetailDtoGridView.CellValueChanged += StockInDetailDtoGridView_CellValueChanged;

            // Event khi thêm/xóa dòng để cập nhật LineNumber
            NhapLapRapDetailDtoGridView.InitNewRow += StockInDetailDtoGridView_InitNewRow;
            NhapLapRapDetailDtoGridView.RowDeleted += StockInDetailDtoGridView_RowDeleted;

            // Event khi validate cell và row
            NhapLapRapDetailDtoGridView.ValidateRow += StockInDetailDtoGridView_ValidateRow;
            NhapLapRapDetailDtoGridView.ValidatingEditor += StockInDetailDtoGridView_ValidatingEditor;

            // Event custom draw row indicator
            NhapLapRapDetailDtoGridView.CustomDrawRowIndicator += StockInDetailDtoGridView_CustomDrawRowIndicator;

            // Event xử lý phím tắt cho GridView (Insert, Delete, Enter)
            NhapLapRapDetailDtoGridView.KeyDown += StockInDetailDtoGridView_KeyDown;

            // Event Popup cho ProductVariantSearchLookUpEdit (RepositoryItem)
            ProductVariantSearchLookUpEdit.Popup += ProductVariantSearchLookUpEdit_Popup;
        }

        #endregion

        #region ========== PUBLIC METHODS ==========

        /// <summary>
        /// Lấy danh sách chi tiết từ grid
        /// </summary>
        public List<StockInOutDetail> GetDetails()
        {
            try
            {
                //Không cast trực tiếp mà lặp từng phần tử trong binding source để tránh lỗi ambiguous call
                var details = new List<StockInOutDetail>();

                foreach (var item in nhapLapRapDetailDtoBindingSource)
                {
                    if(item is not NhapLapRapDetailDto detailDto) continue;

                    details.Add(new StockInOutDetail
                    {
                        Id = default,
                        StockInOutMasterId = _stockInMasterId,
                        ProductVariantId = detailDto.ProductVariantId,
                        StockInQty = detailDto.StockInQty,
                        StockOutQty = 0,
                        UnitPrice = 0,
                        Vat = 0,
                        VatAmount = 0,
                        TotalAmount = 0,
                        TotalAmountIncludedVat = 0,
                        GhiChu = detailDto.GhiChu,
                    });
                }
            
                return details;
            }
            catch (Exception ex)
            {
                _logger.Error("GetDetails: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi lấy danh sách chi tiết: {ex.Message}");
                return [];
            }
        }

        /// <summary>
        /// Clear dữ liệu và reset về trạng thái ban đầu
        /// </summary>
        public void ClearData()
        {
            try
            {
                nhapLapRapDetailDtoBindingSource.DataSource = new List<NhapLapRapDetailDto>();
                nhapLapRapDetailDtoBindingSource.ResetBindings(false);
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

                // Lấy detail entities từ BLL
                var stockInBll = new StockInOutBll();
                var detailEntities = stockInBll.GetDetailsByMasterId(stockInOutMasterId);

                // Convert detail entities sang DTOs sử dụng extension method từ NhapLapRap namespace
                // Chỉ định rõ ràng namespace để tránh ambiguous call
                var detailDtos = detailEntities
                    .Where(e => e != null)
                    .Select(entity => entity.ToNhapLapRapDetailDto()) // Extension method từ NhapLapRap namespace
                    .Where(dto => dto != null)
                    .ToList();

                // Set line numbers cho các detail DTOs
                for (int i = 0; i < detailDtos.Count; i++)
                {
                    detailDtos[i].LineNumber = i + 1;
                }

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
                var details = nhapLapRapDetailDtoBindingSource.Cast<NhapLapRapDetailDto>().ToList();
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
                var details = nhapLapRapDetailDtoBindingSource.Cast<NhapLapRapDetailDto>().ToList();

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
                        _logger.Warning("ValidateAll: Row {0} validation failed, Errors={1}", detail.LineNumber, errors);
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

                    _logger.Warning("ValidateAll: Row {0} has StockInQty <= 0, value={1}", detail.LineNumber, detail.StockInQty);
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
        private void StockInDetailDtoGridView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            GridViewHelper.CustomDrawRowIndicator(NhapLapRapDetailDtoGridView, e);
        }

        /// <summary>
        /// Xử lý sự kiện khi giá trị cell thay đổi trong GridView
        /// Xử lý cả việc cập nhật ProductVariant và tính toán tự động
        /// </summary>
        private void StockInDetailDtoGridView_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
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
                if (NhapLapRapDetailDtoGridView.GetRow(rowHandle) is not NhapLapRapDetailDto rowData)
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

                    // Tìm ProductVariantListDto trong binding source
                    var selectedVariant = productVariantListDtoBindingSource.Cast<ProductVariantListDto>()
                        .FirstOrDefault(v => v.Id == productVariantId);

                    if (selectedVariant == null)
                    {
                        _logger.Warning("CellValueChanged: ProductVariant not found, Id={0}", productVariantId);
                        return;
                    }

                    // Cập nhật các thông tin liên quan
                    rowData.ProductVariantId = selectedVariant.Id;
                    rowData.ProductVariantCode = selectedVariant.VariantCode;
                    rowData.ProductVariantName = $"{selectedVariant.ProductName} - {selectedVariant.VariantFullName}";
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
                NhapLapRapDetailDtoGridView.RefreshRow(rowHandle);
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
        private void StockInDetailDtoGridView_InitNewRow(object sender, DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs e)
        {
            try
            {
                if (NhapLapRapDetailDtoGridView.GetRow(e.RowHandle) is not NhapLapRapDetailDto rowData)
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
        private void StockInDetailDtoGridView_RowDeleted(object sender, RowDeletedEventArgs rowDeletedEventArgs)
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
        private void StockInDetailDtoGridView_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            try
            {
                var column = NhapLapRapDetailDtoGridView.FocusedColumn;
                var fieldName = column?.FieldName;
                var rowHandle = NhapLapRapDetailDtoGridView.FocusedRowHandle;

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
        private void StockInDetailDtoGridView_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            try
            {
                if (e.Row is not NhapLapRapDetailDto rowData)
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
        private void StockInDetailDtoGridView_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                var gridView = NhapLapRapDetailDtoGridView;
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
                        // Enter: Di chuyển sang cột tiếp theo hoặc xuống dòng (chỉ commit row khi ở cột cuối cùng)
                        if (!e.Control && !e.Shift && !e.Alt)
                        {
                            e.Handled = true;
                            var focusedRowHandle = gridView.FocusedRowHandle;
                            var focusedColumn = gridView.FocusedColumn;
                            var visibleColumns = gridView.VisibleColumns;

                            if (visibleColumns == null || visibleColumns.Count == 0)
                            {
                                return;
                            }

                            // Tìm vị trí cột hiện tại trong danh sách các cột hiển thị
                            var currentColumnIndex = -1;
                            for (int i = 0; i < visibleColumns.Count; i++)
                            {
                                if (visibleColumns[i] == focusedColumn)
                                {
                                    currentColumnIndex = i;
                                    break;
                                }
                            }

                            // Nếu không tìm thấy cột, không xử lý
                            if (currentColumnIndex < 0)
                            {
                                return;
                            }

                            var lastColumnIndex = visibleColumns.Count - 1;
                            var isLastColumn = currentColumnIndex >= lastColumnIndex;

                            // Nếu đang ở cột cuối cùng
                            if (isLastColumn)
                            {
                                // Post editor để lưu giá trị hiện tại
                                if (!gridView.PostEditor())
                                {
                                    return; // Validation failed, không di chuyển
                                }

                                // Nếu đang ở new row, commit row trước
                                if (focusedRowHandle == DevExpress.XtraGrid.GridControl.NewItemRowHandle)
                                {
                                    // Validate và commit row
                                    if (gridView.UpdateCurrentRow())
                                    {
                                        // Sau khi commit, di chuyển xuống dòng tiếp theo hoặc thêm dòng mới
                                        var rowCount = gridView.RowCount;
                                        if (rowCount > 0)
                                        {
                                            // Di chuyển đến dòng cuối cùng (dòng vừa commit)
                                            gridView.FocusedRowHandle = rowCount - 1;

                                            // Di chuyển xuống dòng tiếp theo hoặc thêm dòng mới
                                            var nextRowHandle = rowCount;
                                            if (nextRowHandle < gridView.RowCount)
                                            {
                                                gridView.FocusedRowHandle = nextRowHandle;
                                            }
                                            else
                                            {
                                                // Thêm dòng mới
                                                gridView.AddNewRow();
                                            }

                                            // Focus vào cột đầu tiên
                                            if (visibleColumns.Count > 0)
                                            {
                                                gridView.FocusedColumn = visibleColumns[0];
                                            }
                                        }
                                    }
                                }
                                // Nếu đang ở dòng đã có, di chuyển xuống dòng tiếp theo hoặc thêm dòng mới
                                else if (focusedRowHandle >= 0)
                                {
                                    var nextRowHandle = focusedRowHandle + 1;
                                    if (nextRowHandle < gridView.RowCount)
                                    {
                                        gridView.FocusedRowHandle = nextRowHandle;
                                        gridView.FocusedColumn = visibleColumns[0]; // Focus vào cột đầu tiên
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
                                        else if (visibleColumns.Count > 0)
                                        {
                                            gridView.FocusedColumn = visibleColumns[0];
                                        }
                                    }
                                }
                            }
                            // Nếu không phải cột cuối cùng, di chuyển sang cột tiếp theo (không commit row)
                            else
                            {
                                // Post editor để lưu giá trị hiện tại (nhưng không commit row)
                                gridView.PostEditor();

                                // Di chuyển sang cột tiếp theo
                                var nextColumnIndex = currentColumnIndex + 1;
                                if (nextColumnIndex < visibleColumns.Count)
                                {
                                    gridView.FocusedColumn = visibleColumns[nextColumnIndex];
                                }
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.Error("StockInDetailDtoGridView_KeyDown: Exception occurred", ex);
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
                    productVariantListDtoBindingSource.DataSource is List<ProductVariantListDto> existingList &&
                    existingList.Count > 0)
                {
                    return;
                }

                // Hiển thị SplashScreen để thông báo đang load dữ liệu
                SplashScreenHelper.ShowWaitingSplashScreen();

                try
                {
                    // Lấy dữ liệu Entity từ BLL với thông tin đầy đủ
                    var variants = await _productVariantBll.GetAllInUseWithDetailsAsync();

                    // Convert Entity sang ProductVariantListDto
                    var variantListDtos = await ConvertToVariantListDtosAsync(variants);

                    // Bind dữ liệu vào BindingSource
                    productVariantListDtoBindingSource.DataSource = variantListDtos;
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
                    (productVariantListDtoBindingSource.DataSource is List<ProductVariantListDto> list && list.Count == 0))
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
        /// <param name="details">Danh sách NhapLapRapDetailDto chứa ProductVariantId</param>
        private async Task LoadProductVariantsByIdsAsync(List<NhapLapRapDetailDto> details)
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

                var variants = new List<ProductVariant>();
                foreach (var productVariantId in productVariantIds)
                {
                    // Lấy dữ liệu Entity từ BLL với thông tin đầy đủ
                    variants.Add(await _productVariantBll.GetByIdAsync(productVariantId));
                }

                // Convert Entity sang ProductVariantListDto
                var variantListDtos = await ConvertToVariantListDtosAsync(variants);

                // Bind dữ liệu vào BindingSource
                productVariantListDtoBindingSource.DataSource = variantListDtos;
                productVariantListDtoBindingSource.ResetBindings(false);
            }
            catch (Exception ex)
            {
                _logger.Error("LoadProductVariantsByIdsAsync: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi tải biến thể sản phẩm: {ex.Message}");
            }
        }

        /// <summary>
        /// Convert Entity sang ProductVariantListDto (Async)
        /// </summary>
        private async Task<List<ProductVariantListDto>> ConvertToVariantListDtosAsync(List<ProductVariant> variants)
        {
            try
            {
                var result = new List<ProductVariantListDto>();

                foreach (var variant in variants)
                {
                    var dto = new ProductVariantListDto
                    {
                        Id = variant.Id,
                        ProductCode = variant.ProductService?.Code ?? string.Empty,
                        ProductName = variant.ProductService?.Name ?? string.Empty,
                        VariantCode = variant.VariantCode ?? string.Empty,
                        VariantFullName = !string.IsNullOrWhiteSpace(variant.VariantFullName)
                            ? variant.VariantFullName
                            : await BuildVariantFullNameAsync(variant), // Fallback nếu VariantFullName chưa được cập nhật
                        UnitName = variant.UnitOfMeasure?.Name ?? string.Empty,
                        IsActive = variant.IsActive
                    };

                    result.Add(dto);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.Error("ConvertToVariantListDtosAsync: Exception occurred", ex);
                throw new Exception($"Lỗi convert sang ProductVariantListDto: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Xây dựng tên đầy đủ của biến thể từ các thuộc tính (Async)
        /// Format: Attribute1: Value1, Attribute2: Value2, ...
        /// </summary>
        private Task<string> BuildVariantFullNameAsync(ProductVariant variant)
        {
            try
            {
                // Load thông tin thuộc tính từ BLL
                var attributeValues = _productVariantBll.GetAttributeValues(variant.Id);

                if (attributeValues == null || !attributeValues.Any())
                {
                    return Task.FromResult(variant.VariantCode ?? string.Empty); // Nếu không có thuộc tính, trả về mã biến thể
                }

                var attributeParts = new List<string>();

                foreach (var (_, attributeName, value) in attributeValues)
                {
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        attributeParts.Add($"{attributeName}: {value}");
                    }
                }

                if (attributeParts.Any())
                {
                    return Task.FromResult(string.Join(", ", attributeParts));
                }

                return Task.FromResult(variant.VariantCode ?? string.Empty); // Fallback về mã biến thể nếu không có giá trị thuộc tính
            }
            catch (Exception)
            {
                // Nếu có lỗi, trả về mã biến thể
                return Task.FromResult(variant.VariantCode ?? string.Empty);
            }
        }

        #endregion

        #region ========== DATA MANAGEMENT ==========

        /// <summary>
        /// Load danh sách chi tiết từ danh sách DTO
        /// </summary>
        private async void LoadDetails(List<NhapLapRapDetailDto> details)
        {
            try
            {
                details ??= [];

                // Gán StockInOutMasterId cho các dòng chưa có
                foreach (var detail in details)
                {
                    if (detail.StockInOutMasterId == Guid.Empty && _stockInMasterId != Guid.Empty)
                    {
                        detail.StockInOutMasterId = _stockInMasterId;
                    }
                }

                nhapLapRapDetailDtoBindingSource.DataSource = details;
                nhapLapRapDetailDtoBindingSource.ResetBindings(false);

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

                var details = nhapLapRapDetailDtoBindingSource.Cast<NhapLapRapDetailDto>().ToList();

                NhapLapRapDetailDtoGridView.RefreshData();

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
        private void HandleProductVariantIdChange(DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
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

            // Tìm ProductVariantListDto trong binding source
            var selectedVariant = productVariantListDtoBindingSource.Cast<ProductVariantListDto>()
                .FirstOrDefault(v => v.Id == productVariantId);

            if (selectedVariant == null)
            {
                _logger.Warning("HandleProductVariantIdChange: ProductVariant not found, Id={0}", productVariantId);
                e.ErrorText = "Không tìm thấy biến thể sản phẩm";
                e.Valid = false;
                return;
            }

            var rowHandle = NhapLapRapDetailDtoGridView.FocusedRowHandle;

            // Xử lý cả new row (rowHandle < 0) và existing row (rowHandle >= 0)
            if (rowHandle < 0)
            {
                // New row: Sử dụng SetFocusedRowCellValue để set giá trị vào các cell
                // Các giá trị này sẽ được commit khi row được thêm vào binding source
                // Quan trọng: Phải set ProductVariantId trước để khi validate row, giá trị này đã có
                NhapLapRapDetailDtoGridView.SetFocusedRowCellValue("ProductVariantId", selectedVariant.Id);
                NhapLapRapDetailDtoGridView.SetFocusedRowCellValue("ProductVariantCode", selectedVariant.VariantCode);
                NhapLapRapDetailDtoGridView.SetFocusedRowCellValue("ProductVariantName", $"{selectedVariant.VariantFullName}");
                NhapLapRapDetailDtoGridView.SetFocusedRowCellValue("UnitOfMeasureName", selectedVariant.UnitName);
            }
            else
            {
                // Existing row: Cập nhật trực tiếp vào row data
                if (NhapLapRapDetailDtoGridView.GetRow(rowHandle) is NhapLapRapDetailDto rowData)
                {
                    rowData.ProductVariantId = selectedVariant.Id;
                    rowData.ProductVariantCode = selectedVariant.VariantCode;
                    rowData.ProductVariantName = $"{selectedVariant.VariantFullName}";
                    rowData.UnitOfMeasureName = selectedVariant.UnitName;

                    // Refresh grid để hiển thị thay đổi
                    NhapLapRapDetailDtoGridView.RefreshRow(rowHandle);
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
