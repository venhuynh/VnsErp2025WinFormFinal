using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MasterData.ProductService.Dto
{
    /// <summary>
    /// Data Transfer Object cho ProductService.
    /// Chuyển đổi dữ liệu giữa Entity và UI layer.
    /// </summary>
    public class ProductServiceDto
    {
        #region Properties

        [DisplayName("ID")]
        public Guid Id { get; set; }

        [DisplayName("Mã sản phẩm/dịch vụ")]
        [Required(ErrorMessage = "Mã sản phẩm/dịch vụ không được để trống")]
        [StringLength(50, ErrorMessage = "Mã sản phẩm/dịch vụ không được vượt quá 50 ký tự")]
        public string Code { get; set; }

        [DisplayName("Tên sản phẩm/dịch vụ")]
        [Required(ErrorMessage = "Tên sản phẩm/dịch vụ không được để trống")]
        [StringLength(200, ErrorMessage = "Tên sản phẩm/dịch vụ không được vượt quá 200 ký tự")]
        public string Name { get; set; }

        [DisplayName("Danh mục")]
        [Description("ID của danh mục sản phẩm/dịch vụ")]
        public Guid? CategoryId { get; set; }

        [DisplayName("Tên danh mục")]
        [Description("Tên của danh mục sản phẩm/dịch vụ (để hiển thị)")]
        public string CategoryName { get; set; }

        [DisplayName("Loại")]
        [Description("Có phải là dịch vụ không (true = dịch vụ, false = sản phẩm)")]
        public bool IsService { get; set; }

        [DisplayName("Loại hiển thị")]
        [Description("Loại sản phẩm/dịch vụ (hiển thị dạng text)")]
        public string TypeDisplay => IsService ? "Dịch vụ" : "Sản phẩm";

        [DisplayName("Mô tả")]
        [StringLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự")]
        public string Description { get; set; }

        [DisplayName("Đang hoạt động")]
        [Description("Trạng thái hoạt động")]
        public bool IsActive { get; set; }

        [DisplayName("Trạng thái hiển thị")]
        [Description("Trạng thái hoạt động (hiển thị dạng text)")]
        public string StatusDisplay => IsActive ? "Hoạt động" : "Không hoạt động";

        [DisplayName("Ảnh đại diện")]
        [Description("Ảnh đại diện của sản phẩm/dịch vụ")]
        public byte[] ThumbnailImage { get; set; }

        [DisplayName("Có ảnh đại diện")]
        [Description("Kiểm tra xem có ảnh đại diện không")]
        public bool HasThumbnailImage => ThumbnailImage != null && ThumbnailImage.Length > 0;

        [DisplayName("Số biến thể")]
        [Description("Số lượng biến thể sản phẩm")]
        public int VariantCount { get; set; }

        [DisplayName("Số ảnh")]
        [Description("Số lượng ảnh sản phẩm")]
        public int ImageCount { get; set; }

        
        #endregion
    }
}