using Bll.Inventory.InventoryManagement;
using Bll.MasterData.ProductServiceBll;
using Common.Common;
using Common.Helpers;
using Common.Utils;
using Dal.DataContext;
using DevExpress.Data;
using DTO.Inventory.StockOut.XuatLapRap;
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
using Bll.Inventory.StockInOut;

namespace Inventory.StockOut.XuatLapRap
{
    public partial class UcXuatLapRapDetailDto : DevExpress.XtraEditors.XtraUserControl
    {
        #region ========== FIELDS & PROPERTIES ==========

        /// <summary>
        /// Business Logic Layer cho biến thể sản phẩm
        /// </summary>
        private readonly ProductVariantBll _productVariantBll = new();

        /// <summary>
        /// Business Logic Layer cho Device
        /// </summary>
        private readonly DeviceBll _deviceBll = new();

        /// <summary>
        /// Business Logic Layer cho Warranty
        /// </summary>
        private readonly WarrantyBll _warrantyBll = new();

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
        /// ID phiếu xuất kho master (dùng để bind vào các detail rows)
        /// </summary>
        private Guid _stockOutMasterId = Guid.Empty;

        #endregion

        #region ========== CONSTRUCTOR ==========

        public UcXuatLapRapDetailDto()
        {
            InitializeComponent();
            
            // Chỉ khởi tạo control khi không ở design mode
            if (!DesignMode)
            {
                InitializeControl();
            }
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
                // Không chạy trong design mode để tránh lỗi load type
                if (DesignMode) return;

                // GridView đã được khai báo trong Designer, property public sẽ expose nó

                // Khởi tạo binding source với danh sách rỗng
                xuatLapRapDetailDtoBindingSource.DataSource = new List<XuatLapRapDetailDto>();

                // Setup events
                InitializeEvents();

                // Setup BarCode scanning events
                SetupBarCodeEvents();

                // Không load dữ liệu ProductVariant ở đây, sẽ được gọi từ form khi FormLoad
            }
            catch (Exception ex)
            {
                // Trong design mode, không hiển thị lỗi
                if (!DesignMode)
                {
                    ShowError(ex, "Lỗi khởi tạo control");
                }
            }
        }

        /// <summary>
        /// Khởi tạo các event handlers
        /// </summary>
        private void InitializeEvents()
        {
            // Event khi thay đổi cell value (xử lý cả ProductVariant và tính toán)
            XuatLapRapDetailDtoGridView.CellValueChanged += StockOutDetailDtoGridView_CellValueChanged;

            // Event khi thêm/xóa dòng để cập nhật LineNumber
            XuatLapRapDetailDtoGridView.InitNewRow += StockOutDetailDtoGridView_InitNewRow;
            XuatLapRapDetailDtoGridView.RowDeleted += StockOutDetailDtoGridView_RowDeleted;

            // Event khi validate cell và row
            XuatLapRapDetailDtoGridView.ValidateRow += StockOutDetailDtoGridView_ValidateRow;
            XuatLapRapDetailDtoGridView.ValidatingEditor += StockOutDetailDtoGridView_ValidatingEditor;

            // Event custom draw row indicator
            XuatLapRapDetailDtoGridView.CustomDrawRowIndicator += StockOutDetailDtoGridView_CustomDrawRowIndicator;

            // Event xử lý phím tắt cho GridView (Insert, Delete, Enter)
            XuatLapRapDetailDtoGridView.KeyDown += StockOutDetailDtoGridView_KeyDown;

            // Event Popup cho ProductVariantSearchLookUpEdit (RepositoryItem)
            ProductVariantSearchLookUpEdit.Popup += ProductVariantSearchLookUpEdit_Popup;
        }

        /// <summary>
        /// Setup các event handlers cho BarCode scanning
        /// </summary>
        private void SetupBarCodeEvents()
        {
            try
            {
                // Event KeyDown cho BarCodeTextEdit (Enter để thêm vào grid)
                BarCodeTextEdit.KeyDown += BarCodeTextEdit_KeyDown;

                // Event Click cho AddBarCodeHyperlinkLabelControl
                AddBarCodeHyperlinkLabelControl.Click += AddBarCodeHyperlinkLabelControl_Click;
            }
            catch (Exception ex)
            {
                _logger.Error("SetupBarCodeEvents: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi setup BarCode events: {ex.Message}");
            }
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

                foreach (var item in xuatLapRapDetailDtoBindingSource)
                {
                        if (item is not XuatLapRapDetailDto detailDto) continue;

                    details.Add(new StockInOutDetail
                    {
                        Id = default,
                        StockInOutMasterId = _stockOutMasterId,
                        ProductVariantId = detailDto.ProductVariantId,
                        StockInQty = 0,
                        StockOutQty = detailDto.StockOutQty,
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
                xuatLapRapDetailDtoBindingSource.DataSource = new List<XuatLapRapDetailDto>();
                xuatLapRapDetailDtoBindingSource.ResetBindings(false);
                _stockOutMasterId = Guid.Empty;

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
        /// Load dữ liệu detail từ ID phiếu xuất kho, sử dụng trong trường hợp được gọi từ nút "Chi Tiết" <br/>
        /// từ màn hình lịch sử xuất kho
        /// </summary>
        /// <param name="stockInOutMasterId">ID phiếu xuất kho</param>
        public Task LoadDataAsyncForEdit(Guid stockInOutMasterId)
        {
            try
            {
                // Set master ID
                _stockOutMasterId = stockInOutMasterId;

                // Lấy detail entities từ BLL
                var stockInBll = new StockInOutBll();
                var detailEntities = stockInBll.GetDetailsByMasterId(stockInOutMasterId);

                // Convert detail entities sang DTOs sử dụng extension method từ XuatLapRap namespace
                // Chỉ định rõ ràng namespace để tránh ambiguous call
                var detailDtos = detailEntities
                    .Where(e => e != null)
                    .Select(entity => entity.ToXuatLapRapDetailDto()) // Extension method từ XuatLapRap namespace
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
        /// Set StockOutMasterId để bind vào các detail rows
        /// </summary>
        public void SetStockOutMasterId(Guid stockOutMasterId)
        {
            try
            {
                _stockOutMasterId = stockOutMasterId;

                // Cập nhật StockInOutMasterId cho tất cả các dòng hiện có
                var details = xuatLapRapDetailDtoBindingSource.Cast<XuatLapRapDetailDto>().ToList();
                foreach (var detail in details)
                {
                    if (detail.StockInOutMasterId == Guid.Empty)
                    {
                        detail.StockInOutMasterId = stockOutMasterId;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("SetStockOutMasterId: Exception occurred, masterId={0}", ex, stockOutMasterId);
                MsgBox.ShowError($"Lỗi set master ID: {ex.Message}");
            }
        }

        /// <summary>
        /// Validate tất cả các dòng trong grid
        /// </summary>
        public bool ValidateAll()
        {
            try
            {
                var details = xuatLapRapDetailDtoBindingSource.Cast<XuatLapRapDetailDto>().ToList();

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

                    if (detail.StockOutQty > 0) continue;

                    _logger.Warning("ValidateAll: Row {0} has StockOutQty <= 0, value={1}", detail.LineNumber,
                        detail.StockOutQty);
                    MsgBox.ShowError($"Dòng {detail.LineNumber}: Số lượng xuất phải lớn hơn 0");
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
        private void StockOutDetailDtoGridView_CustomDrawRowIndicator(object sender,
            DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            GridViewHelper.CustomDrawRowIndicator(XuatLapRapDetailDtoGridView, e);
        }

        /// <summary>
        /// Xử lý sự kiện khi giá trị cell thay đổi trong GridView
        /// Xử lý cả việc cập nhật ProductVariant và tính toán tự động
        /// </summary>
        private void StockOutDetailDtoGridView_CellValueChanged(object sender,
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
                if (XuatLapRapDetailDtoGridView.GetRow(rowHandle) is not XuatLapRapDetailDto rowData)
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
                rowData.ProductVariantName = $"{selectedVariant.VariantFullName}";
                rowData.UnitOfMeasureName = selectedVariant.UnitName;
            }

                // Xử lý tính toán tự động khi thay đổi số lượng
                if (fieldName == "StockOutQty")
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
                XuatLapRapDetailDtoGridView.RefreshRow(rowHandle);
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
        private void StockOutDetailDtoGridView_InitNewRow(object sender,
            DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs e)
        {
            try
            {
                if (XuatLapRapDetailDtoGridView.GetRow(e.RowHandle) is not XuatLapRapDetailDto rowData)
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
                if (rowData.StockInOutMasterId == Guid.Empty && _stockOutMasterId != Guid.Empty)
                {
                    rowData.StockInOutMasterId = _stockOutMasterId;
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
        private void StockOutDetailDtoGridView_RowDeleted(object sender, RowDeletedEventArgs rowDeletedEventArgs)
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
        private void StockOutDetailDtoGridView_ValidatingEditor(object sender,
            DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            try
            {
                var column = XuatLapRapDetailDtoGridView.FocusedColumn;
                var fieldName = column?.FieldName;

                if (string.IsNullOrEmpty(fieldName)) return;

                switch (fieldName)
                {
                    // Xử lý ProductVariantId: Cập nhật tên và đơn vị tính
                    case "ProductVariantId":
                        HandleProductVariantIdChange(e);
                        break;
                    // Validate StockOutQty: Phải lớn hơn 0
                    case "StockOutQty":
                        ValidateStockOutQty(e);
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
        private void StockOutDetailDtoGridView_ValidateRow(object sender,
            DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            try
            {
                if (e.Row is not XuatLapRapDetailDto rowData)
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

                if (rowData.StockOutQty <= 0)
                {
                    _logger.Warning("ValidateRow: StockOutQty <= 0, value={0}", rowData.StockOutQty);
                    e.Valid = false;
                    e.ErrorText = "Số lượng xuất phải lớn hơn 0";
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
        private void StockOutDetailDtoGridView_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                var gridView = XuatLapRapDetailDtoGridView;
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
        /// Reload ProductVariant datasource (public method để gọi từ form)
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
                MsgBox.ShowError($"Lỗi reload datasource biến thể sản phẩm: {ex.Message}");
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
                    (productVariantListDtoBindingSource.DataSource is List<ProductVariantListDto> list &&
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
        /// <param name="details">Danh sách XuatLapRapDetailDto chứa ProductVariantId</param>
        private async Task LoadProductVariantsByIdsAsync(List<XuatLapRapDetailDto> details)
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
    /// Sử dụng extension method ToListDto() có sẵn trong DTO và bổ sung các field còn thiếu
    /// </summary>
    private Task<List<ProductVariantListDto>> ConvertToVariantListDtosAsync(List<ProductVariant> variants)
    {
        try
        {
            var result = new List<ProductVariantListDto>();

            foreach (var variant in variants)
            {
                // Sử dụng extension method ToListDto() có sẵn trong DTO
                var dto = variant.ToListDto();
                if (dto == null) continue;

                result.Add(dto);
            }

            return Task.FromResult(result);
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
                    return Task.FromResult(variant.VariantCode ??
                                           string.Empty); // Nếu không có thuộc tính, trả về mã biến thể
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

                return
                    Task.FromResult(variant.VariantCode ??
                                    string.Empty); // Fallback về mã biến thể nếu không có giá trị thuộc tính
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
        private async void LoadDetails(List<XuatLapRapDetailDto> details)
        {
            try
            {
                details ??= new List<XuatLapRapDetailDto>();

                // Gán StockInOutMasterId cho các dòng chưa có
                foreach (var detail in details)
                {
                    if (detail.StockInOutMasterId == Guid.Empty && _stockOutMasterId != Guid.Empty)
                    {
                        detail.StockInOutMasterId = _stockOutMasterId;
                    }
                }

                xuatLapRapDetailDtoBindingSource.DataSource = details;
                xuatLapRapDetailDtoBindingSource.ResetBindings(false);

                // Load ProductVariant datasource chỉ cho các ProductVariantId có trong details
                await LoadProductVariantsByIdsAsync(details);

                // Refresh GridView để hiển thị dữ liệu đã load, đặc biệt là các computed properties
                XuatLapRapDetailDtoGridView.RefreshData();

                // Tính toán lại tất cả (để đảm bảo các computed properties được tính đúng)
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

            var details = xuatLapRapDetailDtoBindingSource.Cast<XuatLapRapDetailDto>().ToList();
           
            XuatLapRapDetailDtoGridView.RefreshData();

            // Cập nhật tổng số lượng lên master
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

    /// <summary>
    /// Tính tổng số lượng từ danh sách chi tiết
    /// </summary>
    public decimal CalculateTotalQuantity()
    {
        try
        {
            var details = xuatLapRapDetailDtoBindingSource.Cast<XuatLapRapDetailDto>().ToList();

            var totalQuantity = details.Sum(d => d.StockOutQty);

            return totalQuantity;
        }
        catch (Exception ex)
        {
            _logger.Error("CalculateTotalQuantity: Exception occurred", ex);
            MsgBox.ShowError($"Lỗi tính tổng: {ex.Message}");
            return 0;
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

            var rowHandle = XuatLapRapDetailDtoGridView.FocusedRowHandle;

            // Xử lý cả new row (rowHandle < 0) và existing row (rowHandle >= 0)
            if (rowHandle < 0)
            {
                // New row: Sử dụng SetFocusedRowCellValue để set giá trị vào các cell
                // Các giá trị này sẽ được commit khi row được thêm vào binding source
                // Quan trọng: Phải set ProductVariantId trước để khi validate row, giá trị này đã có
                XuatLapRapDetailDtoGridView.SetFocusedRowCellValue("ProductVariantId", selectedVariant.Id);
                XuatLapRapDetailDtoGridView.SetFocusedRowCellValue("ProductVariantCode", selectedVariant.VariantCode);
                XuatLapRapDetailDtoGridView.SetFocusedRowCellValue("ProductVariantName",
                    $"{selectedVariant.VariantFullName}");
                XuatLapRapDetailDtoGridView.SetFocusedRowCellValue("UnitOfMeasureName", selectedVariant.UnitName);
            }
            else
            {
                // Existing row: Cập nhật trực tiếp vào row data
                if (XuatLapRapDetailDtoGridView.GetRow(rowHandle) is XuatLapRapDetailDto rowData)
                {
                    rowData.ProductVariantId = selectedVariant.Id;
                    rowData.ProductVariantCode = selectedVariant.VariantCode;
                    rowData.ProductVariantName = $"{selectedVariant.VariantFullName}";
                    rowData.UnitOfMeasureName = selectedVariant.UnitName;

                    // Refresh grid để hiển thị thay đổi
                    XuatLapRapDetailDtoGridView.RefreshRow(rowHandle);
                }
            }
        }

        /// <summary>
        /// Validate StockOutQty: Phải lớn hơn 0
        /// </summary>
        private void ValidateStockOutQty(DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            if (e.Value == null)
            {
                e.ErrorText = "Số lượng xuất không được để trống";
                e.Valid = false;
                return;
            }

            if (decimal.TryParse(e.Value.ToString(), out var stockOutQty))
            {
                if (stockOutQty <= 0)
                {
                    _logger.Warning("ValidateStockOutQty: Value <= 0, value={0}", stockOutQty);
                    e.ErrorText = "Số lượng xuất phải lớn hơn 0";
                    e.Valid = false;
                    return;
                }
            }
            else
            {
                _logger.Warning("ValidateStockOutQty: Invalid number format, value={0}", e.Value);
                e.ErrorText = "Số lượng xuất phải là số hợp lệ";
                e.Valid = false;
                return;
            }

            e.Valid = true;
        }

        #endregion

        #region ========== BARCODE SCANNING ==========

        /// <summary>
        /// Event handler khi nhấn phím trong BarCodeTextEdit
        /// </summary>
        private async void BarCodeTextEdit_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                // Khi nhấn Enter, thêm vào grid
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    e.SuppressKeyPress = true; // Ngăn tiếng beep
                    await ProcessBarCodeAndAddToGrid();
                }
            }
            catch (Exception ex)
            {
                _logger.Error("BarCodeTextEdit_KeyDown: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi xử lý mã vạch: {ex.Message}");
            }
        }

        /// <summary>
        /// Event handler khi click vào nút "Thêm vào"
        /// </summary>
        private async void AddBarCodeHyperlinkLabelControl_Click(object sender, EventArgs e)
        {
            try
            {
                await ProcessBarCodeAndAddToGrid();
            }
            catch (Exception ex)
            {
                _logger.Error("AddBarCodeHyperlinkLabelControl_Click: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi thêm mã vạch: {ex.Message}");
            }
        }

        /// <summary>
        /// Xử lý mã BarCode và thêm vào grid
        /// Tìm trong bảng Device hoặc Warranty để lấy thông tin linh kiện
        /// </summary>
        private async Task ProcessBarCodeAndAddToGrid()
        {
            try
            {
                // Lấy mã BarCode từ BarCodeTextEdit
                var barCode = BarCodeTextEdit.Text?.Trim();
                if (string.IsNullOrWhiteSpace(barCode))
                {
                    MsgBox.ShowWarning("Vui lòng nhập mã vạch");
                    BarCodeTextEdit.Focus();
                    return;
                }

                _logger.Debug("ProcessBarCodeAndAddToGrid: Bắt đầu xử lý mã vạch, BarCode={0}", barCode);

                // Ghi log vào LogTextBox
                AppendLog($"Đang tìm kiếm mã vạch: {barCode}");

                // Bước 1: Tìm trong bảng Device trước
                var device = _deviceBll.FindByBarCode(barCode);
                if (device != null)
                {
                    _logger.Info("ProcessBarCodeAndAddToGrid: Tìm thấy Device, DeviceId={0}, ProductVariantId={1}", 
                        device.Id, device.ProductVariantId);
                    AppendLog($"✓ Tìm thấy thiết bị: {device.ProductVariant?.ProductService?.Name ?? "N/A"}");

                    // Thêm vào grid với ProductVariantId từ Device
                    await AddDetailFromDeviceOrWarranty(device.ProductVariantId, device);

                    // Clear BarCode text
                    BarCodeTextEdit.Text = string.Empty;
                    BarCodeTextEdit.Focus();
                    return;
                }

                // Bước 2: Nếu không tìm thấy trong Device, tìm trong Warranty
                var warranty = _warrantyBll.FindByDeviceInfo(barCode);
                if (warranty != null)
                {
                    _logger.Info("ProcessBarCodeAndAddToGrid: Tìm thấy Warranty, WarrantyId={0}, DeviceId={1}", 
                        warranty.Id, warranty.DeviceId);
                    
                    // Lấy ProductVariantId từ Device của Warranty
                    if (warranty.DeviceId.HasValue && warranty.Device?.ProductVariantId != null)
                    {
                        var productVariantId = warranty.Device.ProductVariantId;
                        AppendLog($"✓ Tìm thấy bảo hành: {warranty.Device.ProductVariant?.ProductService?.Name ?? "N/A"}");

                        // Thêm vào grid với ProductVariantId từ Warranty
                        await AddDetailFromDeviceOrWarranty(productVariantId, null, warranty);

                        // Clear BarCode text
                        BarCodeTextEdit.Text = string.Empty;
                        BarCodeTextEdit.Focus();
                        return;
                    }
                    else
                    {
                        AppendLog("⚠ Bảo hành không có thông tin sản phẩm");
                        MsgBox.ShowWarning("Bảo hành không có thông tin sản phẩm. Vui lòng kiểm tra lại.");
                        return;
                    }
                }

                // Không tìm thấy trong cả Device và Warranty
                _logger.Warning("ProcessBarCodeAndAddToGrid: Không tìm thấy thiết bị hoặc bảo hành với mã vạch, BarCode={0}", barCode);
                AppendLog($"✗ Không tìm thấy thiết bị hoặc bảo hành với mã vạch: {barCode}");
                MsgBox.ShowWarning($"Không tìm thấy thiết bị hoặc bảo hành với mã vạch: {barCode}");
            }
            catch (Exception ex)
            {
                _logger.Error("ProcessBarCodeAndAddToGrid: Exception occurred", ex);
                AppendLog($"✗ Lỗi: {ex.Message}");
                MsgBox.ShowError($"Lỗi xử lý mã vạch: {ex.Message}");
            }
        }

        /// <summary>
        /// Thêm dòng chi tiết vào grid từ Device hoặc Warranty
        /// </summary>
        /// <param name="productVariantId">ID biến thể sản phẩm</param>
        /// <param name="device">Device entity (nếu có)</param>
        /// <param name="warranty">Warranty entity (nếu có)</param>
        private async Task AddDetailFromDeviceOrWarranty(Guid productVariantId, Device device = null, Warranty warranty = null)
        {
            try
            {
                if (productVariantId == Guid.Empty)
                {
                    _logger.Warning("AddDetailFromDeviceOrWarranty: ProductVariantId is Empty");
                    MsgBox.ShowWarning("Không có thông tin sản phẩm");
                    return;
                }

                // Kiểm tra xem ProductVariant đã có trong datasource chưa
                var variant = productVariantListDtoBindingSource.Cast<ProductVariantListDto>()
                    .FirstOrDefault(v => v.Id == productVariantId);

                if (variant == null)
                {
                    // Nếu chưa có, cần load ProductVariant vào datasource
                    _logger.Debug("AddDetailFromDeviceOrWarranty: ProductVariant chưa có trong datasource, cần load");
                    
                // Load ProductVariant từ BLL
                var productVariantEntity = await _productVariantBll.GetByIdAsync(productVariantId);
                
                if (productVariantEntity == null)
                {
                    _logger.Warning("AddDetailFromDeviceOrWarranty: Không tìm thấy ProductVariant, ProductVariantId={0}", productVariantId);
                    MsgBox.ShowWarning("Không tìm thấy thông tin sản phẩm");
                    return;
                }

                // Sử dụng extension method ToListDto() có sẵn trong DTO
                variant = productVariantEntity.ToListDto();
                if (variant == null)
                {
                    _logger.Warning("AddDetailFromDeviceOrWarranty: Không thể convert ProductVariant sang DTO, ProductVariantId={0}", productVariantId);
                    MsgBox.ShowWarning("Không thể xử lý thông tin sản phẩm");
                    return;
                }

                    // Thêm vào datasource nếu chưa có
                    var existingList = productVariantListDtoBindingSource.DataSource as List<ProductVariantListDto> ?? new List<ProductVariantListDto>();
                    if (existingList.All(v => v.Id != productVariantId))
                    {
                        existingList.Add(variant);
                        productVariantListDtoBindingSource.DataSource = existingList;
                        productVariantListDtoBindingSource.ResetBindings(false);
                    }
                }

                // Tạo detail DTO mới
                var detailDto = new XuatLapRapDetailDto
                {
                    Id = Guid.NewGuid(),
                    StockInOutMasterId = _stockOutMasterId,
                    ProductVariantId = productVariantId,
                    ProductVariantCode = variant.VariantCode,
                    ProductVariantName = $"{variant.VariantFullName}",
                    UnitOfMeasureName = variant.UnitName,
                    StockOutQty = 1, // Mặc định số lượng là 1
                    LineNumber = xuatLapRapDetailDtoBindingSource.Count + 1
                };

                // Thêm ghi chú nếu có thông tin từ Device hoặc Warranty
                var ghiChuParts = new List<string>();
                if (device != null)
                {
                    if (!string.IsNullOrWhiteSpace(device.SerialNumber))
                        ghiChuParts.Add($"Serial: {device.SerialNumber}");
                    if (!string.IsNullOrWhiteSpace(device.IMEI))
                        ghiChuParts.Add($"IMEI: {device.IMEI}");
                    if (!string.IsNullOrWhiteSpace(device.MACAddress))
                        ghiChuParts.Add($"MAC: {device.MACAddress}");
                }
                // Thêm thông tin bảo hành từ DeviceInfo
                if (warranty != null && warranty.Device != null)
                {
                    var warrantyDeviceInfoParts = new List<string>();
                    if (!string.IsNullOrWhiteSpace(warranty.Device.SerialNumber))
                        warrantyDeviceInfoParts.Add($"S/N: {warranty.Device.SerialNumber}");
                    if (!string.IsNullOrWhiteSpace(warranty.Device.IMEI))
                        warrantyDeviceInfoParts.Add($"IMEI: {warranty.Device.IMEI}");
                    if (!string.IsNullOrWhiteSpace(warranty.Device.MACAddress))
                        warrantyDeviceInfoParts.Add($"MAC: {warranty.Device.MACAddress}");
                    if (!string.IsNullOrWhiteSpace(warranty.Device.AssetTag))
                        warrantyDeviceInfoParts.Add($"Asset: {warranty.Device.AssetTag}");
                    if (!string.IsNullOrWhiteSpace(warranty.Device.LicenseKey))
                        warrantyDeviceInfoParts.Add($"License: {warranty.Device.LicenseKey}");
                    
                    if (warrantyDeviceInfoParts.Any())
                    {
                        ghiChuParts.Add($"Bảo hành: {string.Join(" | ", warrantyDeviceInfoParts)}");
                    }
                }
                
                if (ghiChuParts.Any())
                {
                    detailDto.GhiChu = string.Join(", ", ghiChuParts);
                }

                // Thêm vào binding source
                var details = xuatLapRapDetailDtoBindingSource.DataSource as List<XuatLapRapDetailDto> ?? new List<XuatLapRapDetailDto>();
                details.Add(detailDto);
                xuatLapRapDetailDtoBindingSource.DataSource = details;
                xuatLapRapDetailDtoBindingSource.ResetBindings(false);

                // Cập nhật LineNumber cho tất cả các dòng
                for (int i = 0; i < details.Count; i++)
                {
                    details[i].LineNumber = i + 1;
                }

                // Refresh grid
                XuatLapRapDetailDtoGridView.RefreshData();

                // Trigger event để cập nhật tổng lên master
                OnDetailDataChanged();

                _logger.Info("AddDetailFromDeviceOrWarranty: Đã thêm dòng chi tiết, ProductVariantId={0}", productVariantId);
                AppendLog($"✓ Đã thêm vào danh sách: {detailDto.ProductVariantName}");
            }
            catch (Exception ex)
            {
                _logger.Error("AddDetailFromDeviceOrWarranty: Exception occurred", ex);
                AppendLog($"✗ Lỗi thêm vào danh sách: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Thêm log vào LogTextBox
        /// </summary>
        private void AppendLog(string message)
        {
            try
            {
                if (LogTextBox == null) return;

                var timestamp = DateTime.Now.ToString("HH:mm:ss");
                var logMessage = $"[{timestamp}] {message}\r\n";
                
                LogTextBox.AppendText(logMessage);
                
                // Auto scroll to bottom
                LogTextBox.SelectionStart = LogTextBox.Text.Length;
                LogTextBox.ScrollToCaret();
            }
            catch (Exception ex)
            {
                _logger.Error("AppendLog: Exception occurred", ex);
            }
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