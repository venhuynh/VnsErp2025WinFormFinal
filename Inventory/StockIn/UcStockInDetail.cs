using Bll.MasterData.ProductServiceBll;
using Common.Utils;
using Dal.DataContext;
using DevExpress.XtraEditors;
using DTO.MasterData.ProductService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        #endregion

        #region ========== CONSTRUCTOR ==========

        public UcStockInDetail()
        {
            InitializeComponent();
            InitializeEvents();
        }

        #endregion

        #region ========== INITIALIZATION ==========

        /// <summary>
        /// Khởi tạo các event handlers
        /// </summary>
        private void InitializeEvents()
        {
            // Event khi chọn ProductVariant từ SearchLookUpEdit
            StockInDetailDtoGridView.CellValueChanged += StockInDetailDtoGridView_CellValueChanged;
        }

        #endregion

        #region ========== QUẢN LÝ DỮ LIỆU ==========

        /// <summary>
        /// Load danh sách biến thể sản phẩm vào SearchLookUpEdit
        /// </summary>
        public async Task LoadProductVariantsAsync()
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
        /// Cập nhật các cột liên quan khi chọn ProductVariant
        /// </summary>
        private void StockInDetailDtoGridView_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                // Chỉ xử lý khi thay đổi cột ProductVariantId
                if (e.Column?.FieldName != "ProductVariantId") return;

                var rowHandle = e.RowHandle;
                if (rowHandle < 0) return; // Bỏ qua new row

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

                // Lấy row data từ GridView
                var rowData = StockInDetailDtoGridView.GetRow(rowHandle) as DTO.Inventory.StockIn.StockInDetailDto;
                if (rowData == null) return;

                // Cập nhật các thông tin liên quan
                rowData.ProductVariantId = selectedVariant.Id;
                rowData.ProductVariantCode = selectedVariant.VariantCode;
                rowData.ProductVariantName = $"{selectedVariant.ProductName} - {selectedVariant.VariantFullName}";
                rowData.UnitOfMeasureName = selectedVariant.UnitName;

                // Refresh grid để hiển thị thay đổi
                StockInDetailDtoGridView.RefreshRow(rowHandle);
            }
            catch (Exception ex)
            {
                MsgBox.ShowError($"Lỗi cập nhật thông tin biến thể: {ex.Message}");
            }
        }

        #endregion
    }
}
