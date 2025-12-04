using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DTO.Inventory.Assembly;

/// <summary>
/// Data Transfer Object cho ProductBOM (Bill of Materials)
/// Định nghĩa cấu trúc sản phẩm lắp ráp: 1 sản phẩm hoàn chỉnh gồm nhiều linh kiện
/// </summary>
public class ProductBOMDto
{
    #region Properties - Thông tin cơ bản (map với DB)

    /// <summary>
    /// ID duy nhất của BOM
    /// Map với: ProductBOM.Id
    /// </summary>
    [DisplayName("ID")]
    [Display(Order = -1)]
    public Guid Id { get; set; }

    /// <summary>
    /// ID sản phẩm hoàn chỉnh (ví dụ: Bộ máy tính PC-001)
    /// Map với: ProductBOM.ProductVariantId
    /// </summary>
    [DisplayName("ID Sản phẩm")]
    [Display(Order = 0)]
    [Required(ErrorMessage = "Sản phẩm hoàn chỉnh không được để trống")]
    public Guid ProductVariantId { get; set; }

    /// <summary>
    /// ID linh kiện (ví dụ: CPU, RAM, Ổ cứng)
    /// Map với: ProductBOM.ComponentVariantId
    /// </summary>
    [DisplayName("ID Linh kiện")]
    [Display(Order = 1)]
    [Required(ErrorMessage = "Linh kiện không được để trống")]
    public Guid ComponentVariantId { get; set; }

    /// <summary>
    /// Số lượng linh kiện cần cho 1 sản phẩm
    /// Map với: ProductBOM.Quantity
    /// </summary>
    [DisplayName("Số lượng")]
    [Display(Order = 2)]
    [Required(ErrorMessage = "Số lượng không được để trống")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// ID đơn vị tính
    /// Map với: ProductBOM.UnitId
    /// </summary>
    [DisplayName("ID Đơn vị")]
    [Display(Order = 3)]
    [Required(ErrorMessage = "Đơn vị tính không được để trống")]
    public Guid UnitId { get; set; }

    /// <summary>
    /// Ghi chú
    /// Map với: ProductBOM.Notes
    /// </summary>
    [DisplayName("Ghi chú")]
    [Display(Order = 4)]
    [StringLength(1000, ErrorMessage = "Ghi chú không được vượt quá 1000 ký tự")]
    public string Notes { get; set; }

    /// <summary>
    /// Trạng thái hoạt động
    /// Map với: ProductBOM.IsActive
    /// </summary>
    [DisplayName("Hoạt động")]
    [Display(Order = 5)]
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Ngày tạo
    /// Map với: ProductBOM.CreatedDate
    /// </summary>
    [DisplayName("Ngày tạo")]
    [Display(Order = 6)]
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Ngày sửa
    /// Map với: ProductBOM.ModifiedDate
    /// </summary>
    [DisplayName("Ngày sửa")]
    [Display(Order = 7)]
    public DateTime ModifiedDate { get; set; }

    #endregion

    #region Properties - Thông tin hiển thị (lấy từ ProductVariant)

    /// <summary>
    /// Mã sản phẩm hoàn chỉnh (để hiển thị)
    /// Lấy từ ProductVariant.VariantCode
    /// </summary>
    [DisplayName("Mã sản phẩm")]
    [Display(Order = 10)]
    public string ProductVariantCode { get; set; }

    /// <summary>
    /// Tên sản phẩm hoàn chỉnh (để hiển thị)
    /// Lấy từ ProductVariant.VariantFullName hoặc ProductService.Name
    /// </summary>
    [DisplayName("Tên sản phẩm")]
    [Display(Order = 11)]
    public string ProductVariantName { get; set; }

    /// <summary>
    /// Mã linh kiện (để hiển thị)
    /// Lấy từ ProductVariant.VariantCode
    /// </summary>
    [DisplayName("Mã linh kiện")]
    [Display(Order = 12)]
    public string ComponentVariantCode { get; set; }

    /// <summary>
    /// Tên linh kiện (để hiển thị)
    /// Lấy từ ProductVariant.VariantFullName hoặc ProductService.Name
    /// </summary>
    [DisplayName("Tên linh kiện")]
    [Display(Order = 13)]
    public string ComponentVariantName { get; set; }

    /// <summary>
    /// Mã đơn vị tính (để hiển thị)
    /// Lấy từ UnitOfMeasure.Code
    /// </summary>
    [DisplayName("Mã ĐVT")]
    [Display(Order = 14)]
    public string UnitCode { get; set; }

    /// <summary>
    /// Tên đơn vị tính (để hiển thị)
    /// Lấy từ UnitOfMeasure.Name
    /// </summary>
    [DisplayName("Đơn vị tính")]
    [Display(Order = 15)]
    public string UnitName { get; set; }

    #endregion

    #region Constructors

    public ProductBOMDto()
    {
        Id = Guid.NewGuid();
        IsActive = true;
        CreatedDate = DateTime.Now;
        ModifiedDate = DateTime.Now;
    }

    #endregion

    #region Validation

    /// <summary>
    /// Kiểm tra dữ liệu có hợp lệ không
    /// </summary>
    public bool IsValid()
    {
        return ProductVariantId != Guid.Empty
            && ComponentVariantId != Guid.Empty
            && ProductVariantId != ComponentVariantId  // Sản phẩm không thể là linh kiện của chính nó
            && Quantity > 0
            && UnitId != Guid.Empty;
    }

    #endregion
}

