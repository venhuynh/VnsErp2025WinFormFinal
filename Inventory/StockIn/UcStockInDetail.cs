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
using DevExpress.Data;

namespace Inventory.StockIn
{
    public partial class UcStockInDetail : DevExpress.XtraEditors.XtraUserControl
    {
        #region ========== KHAI BÁO BIẾN ==========

        /// <summary>
        /// Business Logic Layer cho biến thể sản phẩm
        /// </summary>
        private readonly ProductVariantBll _productVariantBll = new ProductVariantBll();

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

            // Event khi validate cell
            StockInDetailDtoGridView.ValidateRow += StockInDetailDtoGridView_ValidateRow;
            StockInDetailDtoGridView.ValidatingEditor += StockInDetailDtoGridView_ValidatingEditor;
        }

        private void StockInDetailDtoGridView_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            try
            {
                var column = StockInDetailDtoGridView.FocusedColumn;
                var fieldName = column?.FieldName;
                if (string.IsNullOrEmpty(fieldName)) return;

                // Xử lý ProductVariantId: Cập nhật tên và đơn vị tính
                if (fieldName == "ProductVariantId")
                {
                    HandleProductVariantIdChange(e);
                }
                // Validate StockInQty: Phải lớn hơn 0
                else if (fieldName == "StockInQty")
                {
                    ValidateStockInQty(e);
                }
                // Validate UnitPrice: Phải >= 0
                else if (fieldName == "UnitPrice")
                {
                    ValidateUnitPrice(e);
                }
                // Validate Vat: Phải từ 0 đến 100
                else if (fieldName == "Vat")
                {
                    ValidateVat(e);
                }
            }
            catch (Exception ex)
            {
                e.ErrorText = $"Lỗi validate: {ex.Message}";
                e.Valid = false;
            }
        }

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
                return;
            }

            if (productVariantId == Guid.Empty) return;

            // Tìm ProductVariantListDto trong binding source
            var selectedVariant = productVariantListDtoBindingSource.Cast<ProductVariantListDto>()
                .FirstOrDefault(v => v.Id == productVariantId);

            if (selectedVariant == null)
            {
                e.ErrorText = "Không tìm thấy biến thể sản phẩm";
                e.Valid = false;
                return;
            }

            var rowHandle = StockInDetailDtoGridView.FocusedRowHandle;

            // Xử lý cả new row (rowHandle < 0) và existing row (rowHandle >= 0)
            if (rowHandle < 0)
            {
                // New row: Sử dụng SetFocusedRowCellValue để set giá trị vào các cell
                // Các giá trị này sẽ được commit khi row được thêm vào binding source
                StockInDetailDtoGridView.SetFocusedRowCellValue("ProductVariantCode", selectedVariant.VariantCode);
                StockInDetailDtoGridView.SetFocusedRowCellValue("ProductVariantName", $"{selectedVariant.ProductName} - {selectedVariant.VariantFullName}");
                StockInDetailDtoGridView.SetFocusedRowCellValue("UnitOfMeasureName", selectedVariant.UnitName);
            }
            else
            {
                // Existing row: Cập nhật trực tiếp vào row data
                if (StockInDetailDtoGridView.GetRow(rowHandle) is StockInDetailDto rowData)
                {
                    rowData.ProductVariantId = selectedVariant.Id;
                    rowData.ProductVariantCode = selectedVariant.VariantCode;
                    rowData.ProductVariantName = $"{selectedVariant.ProductName} - {selectedVariant.VariantFullName}";
                    rowData.UnitOfMeasureName = selectedVariant.UnitName;

                    // Refresh grid để hiển thị thay đổi
                    StockInDetailDtoGridView.RefreshRow(rowHandle);
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
                    e.ErrorText = "Số lượng nhập phải lớn hơn 0";
                    e.Valid = false;
                    return;
                }
            }
            else
            {
                e.ErrorText = "Số lượng nhập phải là số hợp lệ";
                e.Valid = false;
                return;
            }

            e.Valid = true;
        }

        /// <summary>
        /// Validate UnitPrice: Phải >= 0
        /// </summary>
        private void ValidateUnitPrice(DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            if (e.Value == null)
            {
                e.ErrorText = "Đơn giá không được để trống";
                e.Valid = false;
                return;
            }

            if (decimal.TryParse(e.Value.ToString(), out var unitPrice))
            {
                if (unitPrice < 0)
                {
                    e.ErrorText = "Đơn giá không được âm";
                    e.Valid = false;
                    return;
                }
            }
            else
            {
                e.ErrorText = "Đơn giá phải là số hợp lệ";
                e.Valid = false;
                return;
            }

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
                e.Valid = true;
                return;
            }

            if (decimal.TryParse(e.Value.ToString(), out var vat))
            {
                if (vat < 0 || vat > 100)
                {
                    e.ErrorText = "VAT phải từ 0 đến 100";
                    e.Valid = false;
                    return;
                }
            }
            else
            {
                e.ErrorText = "VAT phải là số hợp lệ";
                e.Valid = false;
                return;
            }

            e.Valid = true;
        }

        #endregion

        #region ========== QUẢN LÝ DỮ LIỆU ==========

        /// <summary>
        /// Load danh sách biến thể sản phẩm vào SearchLookUpEdit
        /// </summary>
        private async Task LoadProductVariantsAsync()
        {
            if (_isLoadingProductVariants) return;
            _isLoadingProductVariants = true;

            try
            {
                // Lấy dữ liệu Entity từ BLL với thông tin đầy đủ
                var variants = await _productVariantBll.GetAllWithDetailsAsync();

                // Convert Entity sang ProductVariantListDto
                var variantListDtos = await ConvertToVariantListDtosAsync(variants);

                // Bind dữ liệu vào BindingSource
                productVariantListDtoBindingSource.DataSource = variantListDtos;
                productVariantListDtoBindingSource.ResetBindings(false);
            }
            catch (Exception ex)
            {
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

        #region ========== SỰ KIỆN ==========

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
                
                if (rowHandle < 0) return; // Bỏ qua new row

                // Lấy row data từ GridView
                if (StockInDetailDtoGridView.GetRow(rowHandle) is not StockInDetailDto rowData) return;

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

                    if (productVariantId == Guid.Empty) return;

                    // Tìm ProductVariantListDto trong binding source
                    var selectedVariant = productVariantListDtoBindingSource.Cast<ProductVariantListDto>()
                        .FirstOrDefault(v => v.Id == productVariantId);

                    if (selectedVariant == null) return;

                    // Cập nhật các thông tin liên quan
                    rowData.ProductVariantId = selectedVariant.Id;
                    rowData.ProductVariantCode = selectedVariant.VariantCode;
                    rowData.ProductVariantName = $"{selectedVariant.ProductName} - {selectedVariant.VariantFullName}";
                    rowData.UnitOfMeasureName = selectedVariant.UnitName;
                }

                // Xử lý tính toán tự động khi thay đổi số lượng, đơn giá, VAT
                if (fieldName == "StockInQty" || fieldName == "UnitPrice" || fieldName == "Vat")
                {
                    if (_isCalculating) return; // Tránh tính toán lặp vô hạn

                    _isCalculating = true;

                    try
                    {
                        // Tính toán các giá trị
                        CalculateRowAmounts(rowData);

                        // Cập nhật tổng tiền lên master (nếu có event handler)
                        OnDetailDataChanged();
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
                var rowData = StockInDetailDtoGridView.GetRow(e.RowHandle) as StockInDetailDto;
                if (rowData == null) return;

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

                // Cập nhật LineNumber
                UpdateLineNumbers();
            }
            catch (Exception ex)
            {
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
                // Cập nhật lại LineNumber sau khi xóa
                UpdateLineNumbers();

                // Cập nhật tổng tiền lên master
                OnDetailDataChanged();
            }
            catch (Exception ex)
            {
                MsgBox.ShowError($"Lỗi xóa dòng: {ex.Message}");
            }
        }

        /// <summary>
        /// Validate row data
        /// </summary>
        private void StockInDetailDtoGridView_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            try
            {
                var rowData = e.Row as StockInDetailDto;
                if (rowData == null)
                {
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
                    e.Valid = false;
                    e.ErrorText = string.Join("\n", validationResults.Select(r => r.ErrorMessage));
                    return;
                }

                // Validate business rules
                if (rowData.ProductVariantId == Guid.Empty)
                {
                    e.Valid = false;
                    e.ErrorText = "Vui lòng chọn hàng hóa";
                    return;
                }

                if (rowData.StockInQty <= 0)
                {
                    e.Valid = false;
                    e.ErrorText = "Số lượng nhập phải lớn hơn 0";
                    return;
                }

                if (rowData.UnitPrice < 0)
                {
                    e.Valid = false;
                    e.ErrorText = "Đơn giá không được âm";
                    return;
                }

                e.Valid = true;
            }
            catch (Exception ex)
            {
                e.Valid = false;
                e.ErrorText = $"Lỗi validate: {ex.Message}";
            }
        }

        #endregion

        #region ========== TÍNH TOÁN ==========

        /// <summary>
        /// Tính toán các giá trị tiền cho một dòng
        /// Lưu ý: TotalAmount, VatAmount, TotalAmountIncludedVat giờ là computed properties
        /// nên chúng sẽ tự động tính toán khi truy cập. Method này chỉ để trigger refresh grid.
        /// </summary>
        private void CalculateRowAmounts(StockInDetailDto rowData)
        {
            try
            {
                // Các giá trị TotalAmount, VatAmount, TotalAmountIncludedVat 
                // giờ là computed properties trong StockInDetailDto, 
                // chúng sẽ tự động tính toán từ StockInQty, UnitPrice, và Vat
                // Không cần set giá trị nữa, chỉ cần refresh grid để hiển thị
            }
            catch (Exception ex)
            {
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

                var details = stockInDetailDtoBindingSource.Cast<StockInDetailDto>().ToList();
                foreach (var detail in details)
                {
                    CalculateRowAmounts(detail);
                }

                StockInDetailDtoGridView.RefreshData();

                // Cập nhật tổng tiền lên master
                OnDetailDataChanged();
            }
            catch (Exception ex)
            {
                MsgBox.ShowError($"Lỗi tính toán lại: {ex.Message}");
            }
            finally
            {
                _isCalculating = false;
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
            }
            catch (Exception ex)
            {
                MsgBox.ShowError($"Lỗi cập nhật số thứ tự: {ex.Message}");
            }
        }

        #endregion

        #region ========== QUẢN LÝ DỮ LIỆU ==========

        /// <summary>
        /// Load danh sách chi tiết từ master ID
        /// </summary>
        public void LoadDetails(Guid stockInMasterId)
        {
            try
            {
                _stockInMasterId = stockInMasterId;

                // TODO: Load dữ liệu từ database qua BLL
                // Tạm thời khởi tạo danh sách rỗng
                var details = new List<StockInDetailDto>();
                stockInDetailDtoBindingSource.DataSource = details;
                stockInDetailDtoBindingSource.ResetBindings(false);
            }
            catch (Exception ex)
            {
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
            }
            catch (Exception ex)
            {
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

                return details;
            }
            catch (Exception ex)
            {
                MsgBox.ShowError($"Lỗi lấy danh sách chi tiết: {ex.Message}");
                return new List<StockInDetailDto>();
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

                return (totalQuantity, totalAmount, totalVat, totalAmountIncludedVat);
            }
            catch (Exception ex)
            {
                MsgBox.ShowError($"Lỗi tính tổng: {ex.Message}");
                return (0, 0, 0, 0);
            }
        }

        /// <summary>
        /// Clear dữ liệu và reset về trạng thái ban đầu
        /// </summary>
        public void ClearData()
        {
            try
            {
                stockInDetailDtoBindingSource.DataSource = new List<StockInDetailDto>();
                stockInDetailDtoBindingSource.ResetBindings(false);
                _stockInMasterId = Guid.Empty;
            }
            catch (Exception ex)
            {
                MsgBox.ShowError($"Lỗi xóa dữ liệu: {ex.Message}");
            }
        }

        /// <summary>
        /// Set StockInMasterId để bind vào các detail rows
        /// </summary>
        public void SetStockInMasterId(Guid stockInMasterId)
        {
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
        }

        #endregion

        #region ========== VALIDATION ==========

        /// <summary>
        /// Validate tất cả các dòng trong grid
        /// </summary>
        public bool ValidateAll()
        {
            try
            {
                var details = stockInDetailDtoBindingSource.Cast<StockInDetailDto>().ToList();

                if (details.Count == 0)
                {
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
                        MsgBox.ShowError($"Dòng {detail.LineNumber} có lỗi:\n{errors}");
                        return false;
                    }

                    // Validate business rules
                    if (detail.ProductVariantId == Guid.Empty)
                    {
                        MsgBox.ShowError($"Dòng {detail.LineNumber}: Vui lòng chọn hàng hóa");
                        return false;
                    }

                    if (detail.StockInQty <= 0)
                    {
                        MsgBox.ShowError($"Dòng {detail.LineNumber}: Số lượng nhập phải lớn hơn 0");
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MsgBox.ShowError($"Lỗi validate: {ex.Message}");
                return false;
            }
        }

        #endregion

        #region ========== EVENTS ==========

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
            MsgBox.ShowError($"{message}: {ex.Message}");
        }

        /// <summary>
        /// Hiển thị cảnh báo
        /// </summary>
        private void ShowWarning(string message)
        {
            MsgBox.ShowWarning(message);
        }

        #endregion
    }
}
