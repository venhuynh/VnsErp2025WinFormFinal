using Bll.MasterData.ProductServiceBll;
using Common.Utils;
using Dal.DataContext;
using DTO.Inventory.StockIn;
using DTO.MasterData.ProductService;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Common.Helpers;
using DevExpress.Data;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;

namespace Inventory.StockIn
{
    public partial class UcStockInDetail : DevExpress.XtraEditors.XtraUserControl
    {
        #region ========== FIELDS & PROPERTIES ==========

        /// <summary>
        /// Business Logic Layer cho biến thể sản phẩm
        /// </summary>
        private readonly ProductVariantBll _productVariantBll = new ProductVariantBll();

        /// <summary>
        /// Logger để ghi log các sự kiện
        /// </summary>
        private readonly ILogger _logger = LoggerFactory.CreateLogger(LogCategory.UI);

        /// <summary>
        /// Trạng thái đang tải dữ liệu (guard tránh gọi LoadProductVariantsAsync song song)
        /// </summary>
        private bool _isLoadingProductVariants;

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

        public UcStockInDetail()
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
                // Khởi tạo binding source với danh sách rỗng
                stockInDetailDtoBindingSource.DataSource = new List<StockInDetailDto>();

                // Setup events
                InitializeEvents();

                // Load dữ liệu ProductVariant vào SearchLookUpEdit
                LoadProductVariantsAsync();
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
            StockInDetailDtoGridView.CellValueChanged += StockInDetailDtoGridView_CellValueChanged;

            // Event khi thêm/xóa dòng để cập nhật LineNumber
            StockInDetailDtoGridView.InitNewRow += StockInDetailDtoGridView_InitNewRow;
            StockInDetailDtoGridView.RowDeleted += StockInDetailDtoGridView_RowDeleted;

            // Event khi validate cell và row
            StockInDetailDtoGridView.ValidateRow += StockInDetailDtoGridView_ValidateRow;
            StockInDetailDtoGridView.ValidatingEditor += StockInDetailDtoGridView_ValidatingEditor;

            // Event custom draw row indicator
            StockInDetailDtoGridView.CustomDrawRowIndicator += StockInDetailDtoGridView_CustomDrawRowIndicator;
        }

        #endregion

        #region ========== GRIDVIEW EVENT HANDLERS ==========

        /// <summary>
        /// Custom draw row indicator để hiển thị số thứ tự
        /// </summary>
        private void StockInDetailDtoGridView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            GridViewHelper.CustomDrawRowIndicator(StockInDetailDtoGridView, e);
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
                
                _logger.Debug("CellValueChanged: FieldName={0}, RowHandle={1}, Value={2}", fieldName, rowHandle, e.Value);
                
                if (rowHandle < 0)
                {
                    _logger.Debug("CellValueChanged: Skipping new row");
                    return; // Bỏ qua new row
                }

                // Lấy row data từ GridView
                if (StockInDetailDtoGridView.GetRow(rowHandle) is not StockInDetailDto rowData)
                {
                    _logger.Warning("CellValueChanged: Row data is null, RowHandle={0}", rowHandle);
                    return;
                }

                // Xử lý khi chọn ProductVariant
                if (fieldName == "ProductVariantId")
                {
                    _logger.Debug("CellValueChanged: Processing ProductVariantId change");
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
                        _logger.Debug("CellValueChanged: Invalid ProductVariantId value");
                        return; // Không có giá trị hợp lệ
                    }

                    if (productVariantId == Guid.Empty)
                    {
                        _logger.Debug("CellValueChanged: ProductVariantId is Empty");
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
                    _logger.Info("CellValueChanged: Updated ProductVariant, RowHandle={0}, ProductVariantId={1}", rowHandle, productVariantId);
                }

                // Xử lý tính toán tự động khi thay đổi số lượng, đơn giá, VAT
                if (fieldName == "StockInQty" || fieldName == "UnitPrice" || fieldName == "Vat")
                {
                    if (_isCalculating)
                    {
                        _logger.Debug("CellValueChanged: Already calculating, skipping");
                        return; // Tránh tính toán lặp vô hạn
                    }

                    _logger.Debug("CellValueChanged: Triggering calculation for field={0}", fieldName);
                    _isCalculating = true;

                    try
                    {
                        // Tính toán các giá trị
                        CalculateRowAmounts(rowData);

                        // Cập nhật tổng tiền lên master (nếu có event handler)
                        OnDetailDataChanged();
                        _logger.Debug("CellValueChanged: Calculation completed, TotalAmount={0}, VatAmount={1}, TotalAmountIncludedVat={2}", 
                            rowData.TotalAmount, rowData.VatAmount, rowData.TotalAmountIncludedVat);
                    }
                    finally
                    {
                        _isCalculating = false;
                    }
                }

                // Refresh grid để hiển thị thay đổi
                StockInDetailDtoGridView.RefreshRow(rowHandle);
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
                _logger.Debug("InitNewRow: RowHandle={0}", e.RowHandle);
                
                var rowData = StockInDetailDtoGridView.GetRow(e.RowHandle) as StockInDetailDto;
                if (rowData == null)
                {
                    _logger.Warning("InitNewRow: Row data is null, RowHandle={0}", e.RowHandle);
                    return;
                }

                // Gán ID mới nếu chưa có
                if (rowData.Id == Guid.Empty)
                {
                    rowData.Id = Guid.NewGuid();
                    _logger.Debug("InitNewRow: Generated new Id={0}", rowData.Id);
                }

                // Gán StockInOutMasterId nếu chưa có
                if (rowData.StockInOutMasterId == Guid.Empty && _stockInMasterId != Guid.Empty)
                {
                    rowData.StockInOutMasterId = _stockInMasterId;
                    _logger.Debug("InitNewRow: Set StockInOutMasterId={0}", _stockInMasterId);
                }

                // Cập nhật LineNumber
                UpdateLineNumbers();
                _logger.Info("InitNewRow: New row initialized, Id={0}, LineNumber={1}", rowData.Id, rowData.LineNumber);
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
                _logger.Debug("RowDeleted: RowHandle={0}", rowDeletedEventArgs.RowHandle);
                
                // Cập nhật lại LineNumber sau khi xóa
                UpdateLineNumbers();

                // Cập nhật tổng tiền lên master
                OnDetailDataChanged();
                _logger.Info("RowDeleted: Row deleted successfully, LineNumbers updated");
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
                var column = StockInDetailDtoGridView.FocusedColumn;
                var fieldName = column?.FieldName;
                var rowHandle = StockInDetailDtoGridView.FocusedRowHandle;
                
                _logger.Debug("ValidatingEditor: FieldName={0}, RowHandle={1}, Value={2}", fieldName, rowHandle, e.Value);

                if (string.IsNullOrEmpty(fieldName)) return;

                // Xử lý ProductVariantId: Cập nhật tên và đơn vị tính
                if (fieldName == "ProductVariantId")
                {
                    _logger.Debug("ValidatingEditor: Processing ProductVariantId change");
                    HandleProductVariantIdChange(e);
                }
                // Validate StockInQty: Phải lớn hơn 0
                else if (fieldName == "StockInQty")
                {
                    _logger.Debug("ValidatingEditor: Validating StockInQty");
                    ValidateStockInQty(e);
                }
                // Validate UnitPrice: Phải >= 0
                else if (fieldName == "UnitPrice")
                {
                    _logger.Debug("ValidatingEditor: Validating UnitPrice");
                    ValidateUnitPrice(e);
                }
                // Validate Vat: Phải từ 0 đến 100
                else if (fieldName == "Vat")
                {
                    _logger.Debug("ValidatingEditor: Validating Vat");
                    ValidateVat(e);
                }

                _logger.Debug("ValidatingEditor: Validation result - Valid={0}, ErrorText={1}", e.Valid, e.ErrorText);
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
                _logger.Debug("ValidateRow: Starting validation");
                
                var rowData = e.Row as StockInDetailDto;
                if (rowData == null)
                {
                    _logger.Warning("ValidateRow: Row data is null");
                    e.Valid = false;
                    e.ErrorText = "Dữ liệu không hợp lệ";
                    return;
                }

                _logger.Debug("ValidateRow: RowId={0}, ProductVariantId={1}, StockInQty={2}, UnitPrice={3}, Vat={4}", 
                    rowData.Id, rowData.ProductVariantId, rowData.StockInQty, rowData.UnitPrice, rowData.Vat);

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

                if (rowData.UnitPrice < 0)
                {
                    _logger.Warning("ValidateRow: UnitPrice < 0, value={0}", rowData.UnitPrice);
                    e.Valid = false;
                    e.ErrorText = "Đơn giá không được âm";
                    return;
                }

                _logger.Info("ValidateRow: Validation passed, RowId={0}", rowData.Id);
                e.Valid = true;
            }
            catch (Exception ex)
            {
                _logger.Error("ValidateRow: Exception occurred", ex);
                e.Valid = false;
                e.ErrorText = $"Lỗi validate: {ex.Message}";
            }
        }

        #endregion

        #region ========== PRODUCT VARIANT MANAGEMENT ==========

        /// <summary>
        /// Load danh sách biến thể sản phẩm vào SearchLookUpEdit
        /// </summary>
        private async Task LoadProductVariantsAsync()
        {
            if (_isLoadingProductVariants) return;
            _isLoadingProductVariants = true;

            try
            {
                _logger.Debug("LoadProductVariantsAsync: Starting to load product variants");
                
                // Lấy dữ liệu Entity từ BLL với thông tin đầy đủ
                var variants = await _productVariantBll.GetAllInUseWithDetailsAsync();

                // Convert Entity sang ProductVariantListDto
                var variantListDtos = await ConvertToVariantListDtosAsync(variants);

                // Bind dữ liệu vào BindingSource
                productVariantListDtoBindingSource.DataSource = variantListDtos;
                productVariantListDtoBindingSource.ResetBindings(false);
                
                _logger.Info("LoadProductVariantsAsync: Loaded {0} product variants", variantListDtos.Count);
            }
            catch (Exception ex)
            {
                _logger.Error("LoadProductVariantsAsync: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi tải danh sách biến thể sản phẩm: {ex.Message}");
            }
            finally
            {
                _isLoadingProductVariants = false;
            }
        }

        /// <summary>
        /// Convert Entity sang ProductVariantListDto (Async)
        /// </summary>
        private async Task<List<ProductVariantListDto>> ConvertToVariantListDtosAsync(List<ProductVariant> variants)
        {
            try
            {
                _logger.Debug("ConvertToVariantListDtosAsync: Converting {0} variants", variants?.Count ?? 0);
                
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

                _logger.Debug("ConvertToVariantListDtosAsync: Converted {0} DTOs", result.Count);
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
        /// Load danh sách chi tiết từ master ID
        /// </summary>
        public void LoadDetails(Guid stockInMasterId)
        {
            try
            {
                _logger.Debug("LoadDetails: Loading details for masterId={0}", stockInMasterId);
                
                _stockInMasterId = stockInMasterId;

                // TODO: Load dữ liệu từ database qua BLL
                // Tạm thời khởi tạo danh sách rỗng
                var details = new List<StockInDetailDto>();
                stockInDetailDtoBindingSource.DataSource = details;
                stockInDetailDtoBindingSource.ResetBindings(false);
                
                _logger.Info("LoadDetails: Loaded {0} details for masterId={1}", details.Count, stockInMasterId);
            }
            catch (Exception ex)
            {
                _logger.Error("LoadDetails: Exception occurred, masterId={0}", ex, stockInMasterId);
                MsgBox.ShowError($"Lỗi tải danh sách chi tiết: {ex.Message}");
            }
        }

        /// <summary>
        /// Load danh sách chi tiết từ danh sách DTO
        /// </summary>
        public void LoadDetails(List<StockInDetailDto> details)
        {
            try
            {
                _logger.Debug("LoadDetails: Loading {0} details from DTO list", details?.Count ?? 0);
                
                if (details == null)
                {
                    details = new List<StockInDetailDto>();
                }

                // Gán StockInOutMasterId cho các dòng chưa có
                foreach (var detail in details)
                {
                    if (detail.StockInOutMasterId == Guid.Empty && _stockInMasterId != Guid.Empty)
                    {
                        detail.StockInOutMasterId = _stockInMasterId;
                    }
                }

                stockInDetailDtoBindingSource.DataSource = details;
                stockInDetailDtoBindingSource.ResetBindings(false);

                // Cập nhật LineNumber
                UpdateLineNumbers();

                // Tính toán lại tất cả
                RecalculateAll();
                
                _logger.Info("LoadDetails: Loaded {0} details successfully", details.Count);
            }
            catch (Exception ex)
            {
                _logger.Error("LoadDetails: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi tải danh sách chi tiết: {ex.Message}");
            }
        }

        /// <summary>
        /// Lấy danh sách chi tiết từ grid
        /// </summary>
        public List<StockInDetailDto> GetDetails()
        {
            try
            {
                var details = stockInDetailDtoBindingSource.Cast<StockInDetailDto>().ToList();

                // Đảm bảo tất cả các dòng đều có StockInOutMasterId
                foreach (var detail in details)
                {
                    if (detail.StockInOutMasterId == Guid.Empty && _stockInMasterId != Guid.Empty)
                    {
                        detail.StockInOutMasterId = _stockInMasterId;
                    }
                }

                _logger.Debug("GetDetails: Returning {0} details", details.Count);
                return details;
            }
            catch (Exception ex)
            {
                _logger.Error("GetDetails: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi lấy danh sách chi tiết: {ex.Message}");
                return new List<StockInDetailDto>();
            }
        }

        /// <summary>
        /// Clear dữ liệu và reset về trạng thái ban đầu
        /// </summary>
        public void ClearData()
        {
            try
            {
                _logger.Debug("ClearData: Clearing all data");
                
                stockInDetailDtoBindingSource.DataSource = new List<StockInDetailDto>();
                stockInDetailDtoBindingSource.ResetBindings(false);
                _stockInMasterId = Guid.Empty;
                
                _logger.Info("ClearData: Data cleared successfully");
            }
            catch (Exception ex)
            {
                _logger.Error("ClearData: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi xóa dữ liệu: {ex.Message}");
            }
        }

        /// <summary>
        /// Set StockInMasterId để bind vào các detail rows
        /// </summary>
        public void SetStockInMasterId(Guid stockInMasterId)
        {
            try
            {
                _logger.Debug("SetStockInMasterId: Setting masterId={0}", stockInMasterId);
                
                _stockInMasterId = stockInMasterId;

                // Cập nhật StockInOutMasterId cho tất cả các dòng hiện có
                var details = stockInDetailDtoBindingSource.Cast<StockInDetailDto>().ToList();
                foreach (var detail in details)
                {
                    if (detail.StockInOutMasterId == Guid.Empty)
                    {
                        detail.StockInOutMasterId = stockInMasterId;
                    }
                }
                
                _logger.Info("SetStockInMasterId: Updated {0} details with masterId={1}", details.Count, stockInMasterId);
            }
            catch (Exception ex)
            {
                _logger.Error("SetStockInMasterId: Exception occurred, masterId={0}", ex, stockInMasterId);
                MsgBox.ShowError($"Lỗi set master ID: {ex.Message}");
            }
        }

        #endregion

        #region ========== CALCULATION ==========

        /// <summary>
        /// Tính toán các giá trị tiền cho một dòng
        /// Lưu ý: TotalAmount, VatAmount, TotalAmountIncludedVat giờ là computed properties
        /// nên chúng sẽ tự động tính toán khi truy cập. Method này chỉ để trigger refresh grid.
        /// </summary>
        private void CalculateRowAmounts(StockInDetailDto rowData)
        {
            try
            {
                _logger.Debug("CalculateRowAmounts: RowId={0}, StockInQty={1}, UnitPrice={2}, Vat={3}", 
                    rowData.Id, rowData.StockInQty, rowData.UnitPrice, rowData.Vat);

                // Các giá trị TotalAmount, VatAmount, TotalAmountIncludedVat 
                // giờ là computed properties trong StockInDetailDto, 
                // chúng sẽ tự động tính toán từ StockInQty, UnitPrice, và Vat
                // Không cần set giá trị nữa, chỉ cần refresh grid để hiển thị
                
                // Log các giá trị computed để debug
                _logger.Debug("CalculateRowAmounts: Computed values - TotalAmount={0}, VatAmount={1}, TotalAmountIncludedVat={2}", 
                    rowData.TotalAmount, rowData.VatAmount, rowData.TotalAmountIncludedVat);
            }
            catch (Exception ex)
            {
                _logger.Error("CalculateRowAmounts: Exception occurred, RowId={0}", ex, rowData?.Id);
                throw new Exception($"Lỗi tính toán: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Tính toán lại tất cả các dòng
        /// </summary>
        public void RecalculateAll()
        {
            try
            {
                if (_isCalculating) return;
                _isCalculating = true;

                _logger.Debug("RecalculateAll: Starting recalculation");
                
                var details = stockInDetailDtoBindingSource.Cast<StockInDetailDto>().ToList();
                foreach (var detail in details)
                {
                    CalculateRowAmounts(detail);
                }

                StockInDetailDtoGridView.RefreshData();

                // Cập nhật tổng tiền lên master
                OnDetailDataChanged();
                
                _logger.Info("RecalculateAll: Recalculated {0} rows", details.Count);
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
        /// Tính tổng số lượng, tổng tiền từ danh sách chi tiết
        /// </summary>
        public (decimal TotalQuantity, decimal TotalAmount, decimal TotalVat, decimal TotalAmountIncludedVat) CalculateTotals()
        {
            try
            {
                var details = stockInDetailDtoBindingSource.Cast<StockInDetailDto>().ToList();

                var totalQuantity = details.Sum(d => d.StockInQty);
                var totalAmount = details.Sum(d => d.TotalAmount);
                var totalVat = details.Sum(d => d.VatAmount);
                var totalAmountIncludedVat = details.Sum(d => d.TotalAmountIncludedVat);

                _logger.Debug("CalculateTotals: TotalQuantity={0}, TotalAmount={1}, TotalVat={2}, TotalAmountIncludedVat={3}", 
                    totalQuantity, totalAmount, totalVat, totalAmountIncludedVat);

                return (totalQuantity, totalAmount, totalVat, totalAmountIncludedVat);
            }
            catch (Exception ex)
            {
                _logger.Error("CalculateTotals: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi tính tổng: {ex.Message}");
                return (0, 0, 0, 0);
            }
        }

        /// <summary>
        /// Cập nhật số thứ tự dòng
        /// </summary>
        private void UpdateLineNumbers()
        {
            try
            {
                var details = stockInDetailDtoBindingSource.Cast<StockInDetailDto>().ToList();
                for (int i = 0; i < details.Count; i++)
                {
                    details[i].LineNumber = i + 1;
                }
                
                _logger.Debug("UpdateLineNumbers: Updated line numbers for {0} rows", details.Count);
            }
            catch (Exception ex)
            {
                _logger.Error("UpdateLineNumbers: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi cập nhật số thứ tự: {ex.Message}");
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
                // Cho phép giá trị null/empty trong quá trình nhập
                _logger.Debug("HandleProductVariantIdChange: Value is null or empty, skipping");
                return;
            }

            if (productVariantId == Guid.Empty)
            {
                _logger.Debug("HandleProductVariantIdChange: ProductVariantId is Empty, skipping");
                return;
            }

            _logger.Debug("HandleProductVariantIdChange: ProductVariantId={0}", productVariantId);

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

            var rowHandle = StockInDetailDtoGridView.FocusedRowHandle;
            _logger.Debug("HandleProductVariantIdChange: RowHandle={0}, VariantCode={1}, VariantName={2}", 
                rowHandle, selectedVariant.VariantCode, selectedVariant.FullNameHtml);

            // Xử lý cả new row (rowHandle < 0) và existing row (rowHandle >= 0)
            if (rowHandle < 0)
            {
                // New row: Sử dụng SetFocusedRowCellValue để set giá trị vào các cell
                // Các giá trị này sẽ được commit khi row được thêm vào binding source
                // Quan trọng: Phải set ProductVariantId trước để khi validate row, giá trị này đã có
                _logger.Debug("HandleProductVariantIdChange: Setting values for new row");
                StockInDetailDtoGridView.SetFocusedRowCellValue("ProductVariantId", selectedVariant.Id);
                StockInDetailDtoGridView.SetFocusedRowCellValue("ProductVariantCode", selectedVariant.VariantCode);
                StockInDetailDtoGridView.SetFocusedRowCellValue("ProductVariantName", $"{selectedVariant.FullNameHtml}");
                StockInDetailDtoGridView.SetFocusedRowCellValue("UnitOfMeasureName", selectedVariant.UnitName);
                _logger.Info("HandleProductVariantIdChange: Updated new row with ProductVariantId={0}", selectedVariant.Id);
            }
            else
            {
                // Existing row: Cập nhật trực tiếp vào row data
                if (StockInDetailDtoGridView.GetRow(rowHandle) is StockInDetailDto rowData)
                {
                    _logger.Debug("HandleProductVariantIdChange: Updating existing row");
                    rowData.ProductVariantId = selectedVariant.Id;
                    rowData.ProductVariantCode = selectedVariant.VariantCode;
                    rowData.ProductVariantName = $"{selectedVariant.FullNameHtml}";
                    rowData.UnitOfMeasureName = selectedVariant.UnitName;

                    // Refresh grid để hiển thị thay đổi
                    StockInDetailDtoGridView.RefreshRow(rowHandle);
                    _logger.Info("HandleProductVariantIdChange: Updated existing row {0} with ProductVariantId={1}", rowHandle, selectedVariant.Id);
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
                _logger.Debug("ValidateStockInQty: Value is null");
                e.ErrorText = "Số lượng nhập không được để trống";
                e.Valid = false;
                return;
            }

            if (decimal.TryParse(e.Value.ToString(), out var stockInQty))
            {
                _logger.Debug("ValidateStockInQty: Parsed value={0}", stockInQty);
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

            _logger.Debug("ValidateStockInQty: Validation passed, value={0}", stockInQty);
            e.Valid = true;
        }

        /// <summary>
        /// Validate UnitPrice: Phải >= 0
        /// </summary>
        private void ValidateUnitPrice(DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            if (e.Value == null)
            {
                _logger.Debug("ValidateUnitPrice: Value is null");
                e.ErrorText = "Đơn giá không được để trống";
                e.Valid = false;
                return;
            }

            if (decimal.TryParse(e.Value.ToString(), out var unitPrice))
            {
                _logger.Debug("ValidateUnitPrice: Parsed value={0}", unitPrice);
                if (unitPrice < 0)
                {
                    _logger.Warning("ValidateUnitPrice: Value < 0, value={0}", unitPrice);
                    e.ErrorText = "Đơn giá không được âm";
                    e.Valid = false;
                    return;
                }
            }
            else
            {
                _logger.Warning("ValidateUnitPrice: Invalid number format, value={0}", e.Value);
                e.ErrorText = "Đơn giá phải là số hợp lệ";
                e.Valid = false;
                return;
            }

            _logger.Debug("ValidateUnitPrice: Validation passed, value={0}", unitPrice);
            e.Valid = true;
        }

        /// <summary>
        /// Validate Vat: Phải từ 0 đến 100
        /// </summary>
        private void ValidateVat(DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            if (e.Value == null)
            {
                // VAT có thể để trống (mặc định = 0)
                _logger.Debug("ValidateVat: Value is null, allowing (default=0)");
                e.Valid = true;
                return;
            }

            if (decimal.TryParse(e.Value.ToString(), out var vat))
            {
                _logger.Debug("ValidateVat: Parsed value={0}", vat);
                if (vat < 0 || vat > 100)
                {
                    _logger.Warning("ValidateVat: Value out of range, value={0}", vat);
                    e.ErrorText = "VAT phải từ 0 đến 100";
                    e.Valid = false;
                    return;
                }
            }
            else
            {
                _logger.Warning("ValidateVat: Invalid number format, value={0}", e.Value);
                e.ErrorText = "VAT phải là số hợp lệ";
                e.Valid = false;
                return;
            }

            _logger.Debug("ValidateVat: Validation passed, value={0}", vat);
            e.Valid = true;
        }

        /// <summary>
        /// Validate tất cả các dòng trong grid
        /// </summary>
        public bool ValidateAll()
        {
            try
            {
                _logger.Debug("ValidateAll: Starting validation of all rows");
                
                var details = stockInDetailDtoBindingSource.Cast<StockInDetailDto>().ToList();

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

                    if (detail.StockInQty <= 0)
                    {
                        _logger.Warning("ValidateAll: Row {0} has StockInQty <= 0, value={1}", detail.LineNumber, detail.StockInQty);
                        MsgBox.ShowError($"Dòng {detail.LineNumber}: Số lượng nhập phải lớn hơn 0");
                        return false;
                    }
                }

                _logger.Info("ValidateAll: All {0} rows validated successfully", details.Count);
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
