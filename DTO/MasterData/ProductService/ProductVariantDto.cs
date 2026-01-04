using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DTO.MasterData.ProductService;

/// <summary>
/// Data Transfer Object cho ProductVariant entity
/// Chứa thông tin biến thể sản phẩm/dịch vụ
/// Tối ưu cho hiển thị trong AdvBandedGridView
/// </summary>
public class ProductVariantDto
{
    #region Properties - Từ ProductVariant Entity

    /// <summary>
    /// ID duy nhất của biến thể (từ ProductVariant.Id)
    /// </summary>
    [DisplayName("ID")]
    [Display(Order = -1)]
    public Guid Id { get; set; }

    /// <summary>
    /// ID của sản phẩm/dịch vụ gốc (từ ProductVariant.ProductId)
    /// </summary>
    [DisplayName("ID Sản phẩm")]
    [Display(Order = -1)]
    [Required(ErrorMessage = "Sản phẩm gốc không được để trống")]
    public Guid ProductId { get; set; }

    /// <summary>
    /// Mã biến thể (từ ProductVariant.VariantCode)
    /// </summary>
    [DisplayName("Mã biến thể")]
    [Display(Order = 3)]
    [Required(ErrorMessage = "Mã biến thể không được để trống")]
    [StringLength(50, ErrorMessage = "Mã biến thể không được vượt quá 50 ký tự")]
    public string VariantCode { get; set; }

    /// <summary>
    /// ID của đơn vị tính (từ ProductVariant.UnitId)
    /// </summary>
    [DisplayName("ID Đơn vị")]
    [Display(Order = -1)]
    [Required(ErrorMessage = "Đơn vị tính không được để trống")]
    public Guid UnitId { get; set; }

    /// <summary>
    /// Trạng thái hoạt động (từ ProductVariant.IsActive)
    /// </summary>
    [DisplayName("Trạng thái")]
    [Display(Order = 7)]
    [Description("Trạng thái hoạt động của biến thể")]
    public bool IsActive { get; set; }

    /// <summary>
    /// Ảnh thumbnail của biến thể (từ ProductVariant.ThumbnailImage)
    /// </summary>
    [DisplayName("Ảnh thumbnail")]
    [Description("Ảnh thumbnail của biến thể")]
    public byte[] ThumbnailImage { get; set; }

    /// <summary>
    /// Ngày tạo (từ ProductVariant.CreatedDate)
    /// </summary>
    [DisplayName("Ngày tạo")]
    [Display(Order = 8)]
    [Description("Ngày tạo biến thể")]
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Ngày sửa (từ ProductVariant.ModifiedDate)
    /// </summary>
    [DisplayName("Ngày sửa")]
    [Display(Order = 9)]
    [Description("Ngày sửa biến thể")]
    public DateTime ModifiedDate { get; set; }

    /// <summary>
    /// Tên đầy đủ của biến thể (từ ProductVariant.VariantFullName)
    /// </summary>
    [DisplayName("Tên đầy đủ")]
    [Display(Order = 4)]
    [Description("Tên đầy đủ của biến thể")]
    public string VariantFullName { get; set; }

    /// <summary>
    /// Tên biến thể cho báo cáo (từ ProductVariant.VariantNameForReport)
    /// </summary>
    [DisplayName("Tên cho báo cáo")]
    [Display(Order = 10)]
    [Description("Tên biến thể cho báo cáo")]
    public string VariantNameForReport { get; set; }

    /// <summary>
    /// Tên file thumbnail (từ ProductVariant.ThumbnailFileName)
    /// </summary>
    [DisplayName("Tên file thumbnail")]
    [Display(Order = 11)]
    [StringLength(255, ErrorMessage = "Tên file không được vượt quá 255 ký tự")]
    [Description("Tên file của ảnh thumbnail")]
    public string ThumbnailFileName { get; set; }

    /// <summary>
    /// Đường dẫn tương đối thumbnail (từ ProductVariant.ThumbnailRelativePath)
    /// </summary>
    [DisplayName("Đường dẫn tương đối")]
    [Display(Order = 12)]
    [StringLength(500, ErrorMessage = "Đường dẫn không được vượt quá 500 ký tự")]
    [Description("Đường dẫn tương đối của ảnh thumbnail")]
    public string ThumbnailRelativePath { get; set; }

    /// <summary>
    /// Đường dẫn đầy đủ thumbnail (từ ProductVariant.ThumbnailFullPath)
    /// </summary>
    [DisplayName("Đường dẫn đầy đủ")]
    [Display(Order = 13)]
    [StringLength(1000, ErrorMessage = "Đường dẫn không được vượt quá 1000 ký tự")]
    [Description("Đường dẫn đầy đủ của ảnh thumbnail")]
    public string ThumbnailFullPath { get; set; }

    /// <summary>
    /// Loại lưu trữ thumbnail (từ ProductVariant.ThumbnailStorageType)
    /// </summary>
    [DisplayName("Loại lưu trữ")]
    [Display(Order = 14)]
    [StringLength(20, ErrorMessage = "Loại lưu trữ không được vượt quá 20 ký tự")]
    [Description("Loại lưu trữ của ảnh thumbnail")]
    public string ThumbnailStorageType { get; set; }

    /// <summary>
    /// Kích thước file thumbnail (từ ProductVariant.ThumbnailFileSize)
    /// </summary>
    [DisplayName("Kích thước file")]
    [Display(Order = 15)]
    [Description("Kích thước file của ảnh thumbnail (bytes)")]
    public long? ThumbnailFileSize { get; set; }

    /// <summary>
    /// Checksum của thumbnail (từ ProductVariant.ThumbnailChecksum)
    /// </summary>
    [DisplayName("Checksum")]
    [Display(Order = 16)]
    [StringLength(64, ErrorMessage = "Checksum không được vượt quá 64 ký tự")]
    [Description("Checksum của ảnh thumbnail")]
    public string ThumbnailChecksum { get; set; }

    #endregion

    #region Properties - Từ Related Entities

    /// <summary>
    /// Mã sản phẩm/dịch vụ gốc (từ ProductService.Code)
    /// </summary>
    [DisplayName("Mã sản phẩm")]
    [Display(Order = 1)]
    [StringLength(50, ErrorMessage = "Mã sản phẩm không được vượt quá 50 ký tự")]
    public string ProductCode { get; set; }

    /// <summary>
    /// Tên sản phẩm/dịch vụ gốc (từ ProductService.Name)
    /// </summary>
    [DisplayName("Tên sản phẩm")]
    [Display(Order = 2)]
    [StringLength(255, ErrorMessage = "Tên sản phẩm không được vượt quá 255 ký tự")]
    public string ProductName { get; set; }

    /// <summary>
    /// Mã đơn vị tính (từ UnitOfMeasure.Code)
    /// </summary>
    [DisplayName("Mã đơn vị")]
    [Display(Order = 5)]
    [StringLength(20, ErrorMessage = "Mã đơn vị không được vượt quá 20 ký tự")]
    public string UnitCode { get; set; }

    /// <summary>
    /// Tên đơn vị tính (từ UnitOfMeasure.Name)
    /// </summary>
    [DisplayName("Tên đơn vị")]
    [Display(Order = 6)]
    [StringLength(100, ErrorMessage = "Tên đơn vị không được vượt quá 100 ký tự")]
    public string UnitName { get; set; }

    /// <summary>
    /// Ảnh thumbnail của sản phẩm gốc (từ ProductService.ThumbnailImage)
    /// </summary>
    [DisplayName("Ảnh sản phẩm")]
    [Description("Ảnh thumbnail của sản phẩm gốc")]
    public byte[] ProductThumbnailImage { get; set; }

    #endregion

    
}
