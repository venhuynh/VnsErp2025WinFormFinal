using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MasterData.ProductService.Dto
{
    /// <summary>
    /// DTO chi tiết cho biến thể sản phẩm.
    /// Bao gồm đầy đủ thông tin ProductService, UnitOfMeasure, các thuộc tính và thông tin khác của ProductVariant.
    /// </summary>
    public class ProductVariantDetailDto
    {
        #region ProductVariant Information

        /// <summary>
        /// ID của biến thể sản phẩm
        /// </summary>
        [DisplayName("ID Biến thể")]
        public Guid Id { get; set; }

        /// <summary>
        /// ID của sản phẩm/dịch vụ
        /// </summary>
        [DisplayName("ID Sản phẩm")]
        public Guid ProductId { get; set; }

        /// <summary>
        /// Mã biến thể sản phẩm
        /// </summary>
        [DisplayName("Mã biến thể")]
        [Required(ErrorMessage = "Mã biến thể không được để trống")]
        [StringLength(50, ErrorMessage = "Mã biến thể không được vượt quá 50 ký tự")]
        public string VariantCode { get; set; }

        /// <summary>
        /// ID của đơn vị tính
        /// </summary>
        [DisplayName("ID Đơn vị")]
        public Guid UnitId { get; set; }

        /// <summary>
        /// Trạng thái hoạt động
        /// </summary>
        [DisplayName("Hoạt động")]
        public bool IsActive { get; set; }

        /// <summary>
        /// Hình ảnh thumbnail
        /// </summary>
        [DisplayName("Hình ảnh")]
        public byte[] ThumbnailImage { get; set; }

        #endregion

        #region ProductService Information

        /// <summary>
        /// Mã sản phẩm/dịch vụ
        /// </summary>
        [DisplayName("Mã sản phẩm")]
        [StringLength(50, ErrorMessage = "Mã sản phẩm không được vượt quá 50 ký tự")]
        public string ProductCode { get; set; }

        /// <summary>
        /// Tên sản phẩm/dịch vụ
        /// </summary>
        [DisplayName("Tên sản phẩm")]
        [StringLength(255, ErrorMessage = "Tên sản phẩm không được vượt quá 255 ký tự")]
        public string ProductName { get; set; }

        /// <summary>
        /// ID danh mục sản phẩm
        /// </summary>
        [DisplayName("ID Danh mục")]
        public Guid? CategoryId { get; set; }

        /// <summary>
        /// Tên danh mục sản phẩm
        /// </summary>
        [DisplayName("Danh mục")]
        [StringLength(255, ErrorMessage = "Tên danh mục không được vượt quá 255 ký tự")]
        public string CategoryName { get; set; }

        /// <summary>
        /// Là dịch vụ (true) hay sản phẩm (false)
        /// </summary>
        [DisplayName("Là dịch vụ")]
        public bool IsService { get; set; }

        /// <summary>
        /// Mô tả sản phẩm/dịch vụ
        /// </summary>
        [DisplayName("Mô tả sản phẩm")]
        [StringLength(500, ErrorMessage = "Mô tả sản phẩm không được vượt quá 500 ký tự")]
        public string ProductDescription { get; set; }

        /// <summary>
        /// Trạng thái hoạt động của sản phẩm
        /// </summary>
        [DisplayName("Sản phẩm hoạt động")]
        public bool ProductIsActive { get; set; }

        /// <summary>
        /// Hình ảnh thumbnail của sản phẩm
        /// </summary>
        [DisplayName("Hình ảnh sản phẩm")]
        public byte[] ProductThumbnailImage { get; set; }

        #endregion

        #region UnitOfMeasure Information

        /// <summary>
        /// Mã đơn vị tính
        /// </summary>
        [DisplayName("Mã đơn vị")]
        [StringLength(20, ErrorMessage = "Mã đơn vị không được vượt quá 20 ký tự")]
        public string UnitCode { get; set; }

        /// <summary>
        /// Tên đơn vị tính
        /// </summary>
        [DisplayName("Tên đơn vị")]
        [StringLength(100, ErrorMessage = "Tên đơn vị không được vượt quá 100 ký tự")]
        public string UnitName { get; set; }

        /// <summary>
        /// Mô tả đơn vị tính
        /// </summary>
        [DisplayName("Mô tả đơn vị")]
        [StringLength(255, ErrorMessage = "Mô tả đơn vị không được vượt quá 255 ký tự")]
        public string UnitDescription { get; set; }

        /// <summary>
        /// Trạng thái hoạt động của đơn vị
        /// </summary>
        [DisplayName("Đơn vị hoạt động")]
        public bool UnitIsActive { get; set; }

        #endregion

        #region Variant Attributes

        /// <summary>
        /// Danh sách thuộc tính của biến thể
        /// </summary>
        [DisplayName("Thuộc tính biến thể")]
        public List<ProductVariantAttributeDto> VariantAttributes { get; set; } = new List<ProductVariantAttributeDto>();

        /// <summary>
        /// Số lượng thuộc tính
        /// </summary>
        [DisplayName("Số thuộc tính")]
        public int AttributeCount => VariantAttributes?.Count ?? 0;

        #endregion

        #region Computed Properties

        /// <summary>
        /// Tên hiển thị đầy đủ của biến thể
        /// </summary>
        [DisplayName("Tên đầy đủ")]
        public string FullDisplayName => $"{ProductName} - {VariantCode}";

        /// <summary>
        /// Thông tin đơn vị đầy đủ
        /// </summary>
        [DisplayName("Thông tin đơn vị")]
        public string UnitInfo => !string.IsNullOrEmpty(UnitName) ? $"{UnitCode} - {UnitName}" : UnitCode;

        /// <summary>
        /// Trạng thái hiển thị
        /// </summary>
        [DisplayName("Trạng thái")]
        public string StatusDisplay => IsActive ? "Hoạt động" : "Không hoạt động";

        /// <summary>
        /// Loại hiển thị
        /// </summary>
        [DisplayName("Loại")]
        public string TypeDisplay => IsService ? "Dịch vụ" : "Sản phẩm";

        /// <summary>
        /// Tóm tắt thuộc tính
        /// </summary>
        [DisplayName("Tóm tắt thuộc tính")]
        public string AttributesSummary
        {
            get
            {
                if (VariantAttributes == null || VariantAttributes.Count == 0)
                    return "Không có thuộc tính";

                var summary = new List<string>();
                foreach (var attr in VariantAttributes)
                {
                    summary.Add($"{attr.AttributeName}: {attr.AttributeValue}");
                }
                return string.Join(", ", summary);
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Tạo bản sao
        /// </summary>
        /// <returns>Bản sao mới</returns>
        public ProductVariantDetailDto Clone()
        {
            var clone = new ProductVariantDetailDto
            {
                // ProductVariant Information
                Id = this.Id,
                ProductId = this.ProductId,
                VariantCode = this.VariantCode,
                UnitId = this.UnitId,
                IsActive = this.IsActive,
                ThumbnailImage = this.ThumbnailImage,

                // ProductService Information
                ProductCode = this.ProductCode,
                ProductName = this.ProductName,
                CategoryId = this.CategoryId,
                CategoryName = this.CategoryName,
                IsService = this.IsService,
                ProductDescription = this.ProductDescription,
                ProductIsActive = this.ProductIsActive,
                ProductThumbnailImage = this.ProductThumbnailImage,

                // UnitOfMeasure Information
                UnitCode = this.UnitCode,
                UnitName = this.UnitName,
                UnitDescription = this.UnitDescription,
                UnitIsActive = this.UnitIsActive
            };

            // Clone VariantAttributes
            if (this.VariantAttributes != null)
            {
                clone.VariantAttributes = new List<ProductVariantAttributeDto>();
                foreach (var attr in this.VariantAttributes)
                {
                    clone.VariantAttributes.Add(attr.Clone());
                }
            }

            return clone;
        }

        /// <summary>
        /// Cập nhật từ DTO khác
        /// </summary>
        /// <param name="source">DTO nguồn</param>
        public void UpdateFrom(ProductVariantDetailDto source)
        {
            if (source == null) return;

            // ProductVariant Information
            Id = source.Id;
            ProductId = source.ProductId;
            VariantCode = source.VariantCode;
            UnitId = source.UnitId;
            IsActive = source.IsActive;
            ThumbnailImage = source.ThumbnailImage;

            // ProductService Information
            ProductCode = source.ProductCode;
            ProductName = source.ProductName;
            CategoryId = source.CategoryId;
            CategoryName = source.CategoryName;
            IsService = source.IsService;
            ProductDescription = source.ProductDescription;
            ProductIsActive = source.ProductIsActive;
            ProductThumbnailImage = source.ProductThumbnailImage;

            // UnitOfMeasure Information
            UnitCode = source.UnitCode;
            UnitName = source.UnitName;
            UnitDescription = source.UnitDescription;
            UnitIsActive = source.UnitIsActive;

            // Update VariantAttributes
            if (source.VariantAttributes != null)
            {
                VariantAttributes = new List<ProductVariantAttributeDto>();
                foreach (var attr in source.VariantAttributes)
                {
                    VariantAttributes.Add(attr.Clone());
                }
            }
        }

        /// <summary>
        /// Reset về giá trị mặc định
        /// </summary>
        public void Reset()
        {
            // ProductVariant Information
            Id = Guid.Empty;
            ProductId = Guid.Empty;
            VariantCode = string.Empty;
            UnitId = Guid.Empty;
            IsActive = true;
            ThumbnailImage = null;

            // ProductService Information
            ProductCode = string.Empty;
            ProductName = string.Empty;
            CategoryId = null;
            CategoryName = string.Empty;
            IsService = false;
            ProductDescription = string.Empty;
            ProductIsActive = true;
            ProductThumbnailImage = null;

            // UnitOfMeasure Information
            UnitCode = string.Empty;
            UnitName = string.Empty;
            UnitDescription = string.Empty;
            UnitIsActive = true;

            // Reset VariantAttributes
            VariantAttributes?.Clear();
        }

        /// <summary>
        /// Kiểm tra tính hợp lệ của DTO
        /// </summary>
        /// <returns>True nếu hợp lệ</returns>
        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(VariantCode) &&
                   ProductId != Guid.Empty &&
                   UnitId != Guid.Empty &&
                   !string.IsNullOrWhiteSpace(ProductName);
        }

        /// <summary>
        /// Lấy danh sách lỗi validation
        /// </summary>
        /// <returns>Danh sách lỗi</returns>
        public List<string> GetValidationErrors()
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(VariantCode))
                errors.Add("Mã biến thể không được để trống");

            if (ProductId == Guid.Empty)
                errors.Add("ID sản phẩm không hợp lệ");

            if (UnitId == Guid.Empty)
                errors.Add("ID đơn vị tính không hợp lệ");

            if (string.IsNullOrWhiteSpace(ProductName))
                errors.Add("Tên sản phẩm không được để trống");

            if (VariantCode != null && VariantCode.Length > 50)
                errors.Add("Mã biến thể không được vượt quá 50 ký tự");

            if (ProductCode != null && ProductCode.Length > 50)
                errors.Add("Mã sản phẩm không được vượt quá 50 ký tự");

            if (ProductName != null && ProductName.Length > 255)
                errors.Add("Tên sản phẩm không được vượt quá 255 ký tự");

            if (ProductDescription != null && ProductDescription.Length > 500)
                errors.Add("Mô tả sản phẩm không được vượt quá 500 ký tự");

            if (UnitCode != null && UnitCode.Length > 20)
                errors.Add("Mã đơn vị không được vượt quá 20 ký tự");

            if (UnitName != null && UnitName.Length > 100)
                errors.Add("Tên đơn vị không được vượt quá 100 ký tự");

            if (UnitDescription != null && UnitDescription.Length > 255)
                errors.Add("Mô tả đơn vị không được vượt quá 255 ký tự");

            return errors;
        }

        /// <summary>
        /// ToString override để hiển thị thông tin
        /// </summary>
        /// <returns>Chuỗi hiển thị</returns>
        public override string ToString()
        {
            return $"{ProductName} - {VariantCode} ({UnitName})";
        }

        #endregion
    }
}
