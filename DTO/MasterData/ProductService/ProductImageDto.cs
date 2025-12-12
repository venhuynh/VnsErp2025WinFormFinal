using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DTO.MasterData.ProductService;

/// <summary>
/// Data Transfer Object cho hình ảnh sản phẩm/dịch vụ
/// Dùng cho Query và truyền dữ liệu giữa Service ↔ WinForms
/// Map với bảng ProductImage trong database
/// </summary>
public class ProductImageDto
{
    #region Properties - Thông tin cơ bản

    /// <summary>
    /// ID duy nhất của hình ảnh
    /// Map với: ProductImage.Id
    /// </summary>
    [DisplayName("ID")]
    [Display(Order = -1)]
    public Guid Id { get; set; }

    /// <summary>
    /// ID sản phẩm/dịch vụ (Master)
    /// Map với: ProductImage.ProductId
    /// </summary>
    [DisplayName("ID Sản phẩm")]
    [Display(Order = 1)]
    [Required(ErrorMessage = "ID sản phẩm không được để trống")]
    public Guid? ProductId { get; set; }

    #endregion

    #region Properties - Thông tin sản phẩm (từ ProductService)

    /// <summary>
    /// Mã sản phẩm/dịch vụ
    /// Map với: ProductService.Code
    /// </summary>
    [DisplayName("Mã sản phẩm")]
    [Display(Order = 1)]
    [StringLength(50, ErrorMessage = "Mã sản phẩm không được vượt quá 50 ký tự")]
    public string ProductCode { get; set; }

    /// <summary>
    /// Tên sản phẩm/dịch vụ
    /// Map với: ProductService.Name
    /// </summary>
    [DisplayName("Tên sản phẩm")]
    [Display(Order = 2)]
    [StringLength(255, ErrorMessage = "Tên sản phẩm không được vượt quá 255 ký tự")]
    public string ProductName { get; set; }

    /// <summary>
    /// Mã biến thể sản phẩm (nếu có)
    /// Map với: ProductVariant.Code
    /// </summary>
    [DisplayName("Mã biến thể")]
    [Display(Order = 3)]
    [StringLength(50, ErrorMessage = "Mã biến thể không được vượt quá 50 ký tự")]
    public string VariantCode { get; set; }

    /// <summary>
    /// Tên biến thể sản phẩm đầy đủ (nếu có)
    /// Map với: ProductVariant.FullName
    /// </summary>
    [DisplayName("Tên biến thể")]
    [Display(Order = 4)]
    [StringLength(500, ErrorMessage = "Tên biến thể không được vượt quá 500 ký tự")]
    public string VariantFullName { get; set; }

    /// <summary>
    /// Tên đơn vị tính
    /// Map với: UnitOfMeasure.Name
    /// </summary>
    [DisplayName("Đơn vị tính")]
    [Display(Order = 5)]
    [StringLength(50, ErrorMessage = "Tên đơn vị tính không được vượt quá 50 ký tự")]
    public string UnitName { get; set; }

    /// <summary>
    /// Số thứ tự hình ảnh trong sản phẩm (1, 2, 3, ...)
    /// Được tính dựa trên CreateDate của các hình ảnh cùng ProductId
    /// </summary>
    [DisplayName("Số thứ tự")]
    [Display(Order = 6)]
    public int ImageSequenceNumber { get; set; }

    /// <summary>
    /// Caption hiển thị trên UI: "Mã sản phẩm (số thứ tự)"
    /// Ví dụ: "SP001 (1)", "SP001 (2)"
    /// </summary>
    [DisplayName("Caption")]
    [Display(Order = 7)]
    public string DisplayCaption
    {
        get
        {
            if (string.IsNullOrWhiteSpace(ProductCode))
                return ImageSequenceNumber > 0 ? $"({ImageSequenceNumber})" : FileName ?? "";
            
            return ImageSequenceNumber > 0 
                ? $"{ProductCode} ({ImageSequenceNumber})" 
                : ProductCode;
        }
    }

    /// <summary>
    /// Group Caption để nhóm các hình ảnh: "ProductCode - ProductName - VariantCode"
    /// Ví dụ: "SP001 - Áo sơ mi - M"
    /// </summary>
    [DisplayName("Group Caption")]
    [Display(Order = 8)]
    public string GroupCaption
    {
        get
        {
            var parts = new List<string>();

            // ProductCode
            if (!string.IsNullOrWhiteSpace(ProductCode))
            {
                parts.Add(ProductCode);
            }

            // ProductName
            if (!string.IsNullOrWhiteSpace(ProductName))
            {
                parts.Add(ProductName);
            }

            // VariantCode
            if (!string.IsNullOrWhiteSpace(VariantCode))
            {
                parts.Add(VariantCode);
            }

            // VariantFullName
            if (!string.IsNullOrWhiteSpace(VariantFullName))
            {
                parts.Add(VariantFullName);
            }

            return parts.Count > 0 ? string.Join(" - ", parts) : "Không có thông tin";
        }
    }

    /// <summary>
    /// Dữ liệu hình ảnh (byte array)
    /// Map với: ProductImage.ImageData
    /// </summary>
    [DisplayName("Dữ liệu hình ảnh")]
    [Display(Order = 2)]
    public byte[] ImageData { get; set; }

    #endregion

    #region Properties - Thông tin file

    /// <summary>
    /// Tên file hình ảnh
    /// Map với: ProductImage.FileName
    /// </summary>
    [DisplayName("Tên file")]
    [Display(Order = 3)]
    [StringLength(255, ErrorMessage = "Tên file không được vượt quá 255 ký tự")]
    public string FileName { get; set; }

    /// <summary>
    /// Đường dẫn tương đối của file
    /// Map với: ProductImage.RelativePath
    /// </summary>
    [DisplayName("Đường dẫn tương đối")]
    [Display(Order = 4)]
    [StringLength(500, ErrorMessage = "Đường dẫn tương đối không được vượt quá 500 ký tự")]
    public string RelativePath { get; set; }

    /// <summary>
    /// Đường dẫn đầy đủ của file
    /// Map với: ProductImage.FullPath
    /// </summary>
    [DisplayName("Đường dẫn đầy đủ")]
    [Display(Order = 5)]
    [StringLength(1000, ErrorMessage = "Đường dẫn đầy đủ không được vượt quá 1000 ký tự")]
    public string FullPath { get; set; }

    /// <summary>
    /// Loại lưu trữ (Database, FileSystem, NAS, Cloud, etc.)
    /// Map với: ProductImage.StorageType
    /// </summary>
    [DisplayName("Loại lưu trữ")]
    [Display(Order = 6)]
    [StringLength(20, ErrorMessage = "Loại lưu trữ không được vượt quá 20 ký tự")]
    public string StorageType { get; set; }

    /// <summary>
    /// Kích thước file (bytes)
    /// Map với: ProductImage.FileSize
    /// </summary>
    [DisplayName("Kích thước file (bytes)")]
    [Display(Order = 7)]
    public long? FileSize { get; set; }

    /// <summary>
    /// Phần mở rộng của file (.jpg, .png, .pdf, etc.)
    /// Map với: ProductImage.FileExtension
    /// </summary>
    [DisplayName("Phần mở rộng")]
    [Display(Order = 8)]
    [StringLength(10, ErrorMessage = "Phần mở rộng không được vượt quá 10 ký tự")]
    public string FileExtension { get; set; }

    /// <summary>
    /// MIME type của file (image/jpeg, image/png, application/pdf, etc.)
    /// Map với: ProductImage.MimeType
    /// </summary>
    [DisplayName("MIME Type")]
    [Display(Order = 9)]
    [StringLength(100, ErrorMessage = "MIME type không được vượt quá 100 ký tự")]
    public string MimeType { get; set; }

    /// <summary>
    /// Checksum/MD5 hash của file để kiểm tra tính toàn vẹn
    /// Map với: ProductImage.Checksum
    /// </summary>
    [DisplayName("Checksum")]
    [Display(Order = 10)]
    [StringLength(64, ErrorMessage = "Checksum không được vượt quá 64 ký tự")]
    public string Checksum { get; set; }

    /// <summary>
    /// Trạng thái file có tồn tại hay không
    /// Map với: ProductImage.FileExists
    /// </summary>
    [DisplayName("File tồn tại")]
    [Display(Order = 11)]
    public bool? FileExists { get; set; }

    /// <summary>
    /// Ngày giờ lần cuối kiểm tra file
    /// Map với: ProductImage.LastVerified
    /// </summary>
    [DisplayName("Ngày kiểm tra cuối")]
    [Display(Order = 12)]
    public DateTime? LastVerified { get; set; }

    /// <summary>
    /// Trạng thái migration (Pending, Completed, Failed, etc.)
    /// Map với: ProductImage.MigrationStatus
    /// </summary>
    [DisplayName("Trạng thái migration")]
    [Display(Order = 13)]
    [StringLength(20, ErrorMessage = "Trạng thái migration không được vượt quá 20 ký tự")]
    public string MigrationStatus { get; set; }

    #endregion

    #region Properties - Thông tin hệ thống

    /// <summary>
    /// Ngày tạo
    /// Map với: ProductImage.CreateDate
    /// </summary>
    [DisplayName("Ngày tạo")]
    [Display(Order = 20)]
    public DateTime CreateDate { get; set; }

    /// <summary>
    /// ID người tạo
    /// Map với: ProductImage.CreateBy
    /// </summary>
    [DisplayName("ID Người tạo")]
    [Display(Order = 21)]
    public Guid CreateBy { get; set; }

    /// <summary>
    /// Ngày sửa đổi
    /// Map với: ProductImage.ModifiedDate
    /// </summary>
    [DisplayName("Ngày sửa")]
    [Display(Order = 22)]
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// ID người sửa đổi
    /// Map với: ProductImage.ModifiedBy
    /// </summary>
    [DisplayName("ID Người sửa")]
    [Display(Order = 23)]
    public Guid ModifiedBy { get; set; }

    #endregion

    #region Helper Properties

    /// <summary>
    /// Hiển thị kích thước file dạng đọc được (KB, MB, GB)
    /// </summary>
    [DisplayName("Kích thước")]
    [Display(Order = 7)]
    public string FileSizeDisplay
    {
        get
        {
            if (!FileSize.HasValue || FileSize.Value == 0)
                return "0 B";

            string[] sizes = ["B", "KB", "MB", "GB", "TB"];
            double len = FileSize.Value;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len /= 1024;
            }

            return $"{len:0.##} {sizes[order]}";
        }
    }

    /// <summary>
    /// Kiểm tra xem file có tồn tại hay không (trả về true nếu FileExists = true)
    /// </summary>
    [DisplayName("Có file")]
    [Display(Order = 11)]
    public bool HasFile => FileExists == true;

    /// <summary>
    /// Đường dẫn đầy đủ của hình ảnh (ưu tiên FullPath, fallback RelativePath)
    /// </summary>
    [DisplayName("Đường dẫn đầy đủ")]
    [Display(Order = 5)]
    public string FullImagePath => FullPath ?? RelativePath;

    /// <summary>
    /// Tên hiển thị cho hình ảnh (ưu tiên FileName, fallback DisplayCaption)
    /// </summary>
    [DisplayName("Hình ảnh")]
    [Display(Order = 7)]
    public string ImageDisplayName => !string.IsNullOrWhiteSpace(FileName) ? FileName : DisplayCaption;

    /// <summary>
    /// Tên hiển thị đầy đủ cho sản phẩm/biến thể
    /// </summary>
    [DisplayName("Sản phẩm")]
    [Display(Order = 1)]
    public string ProductDisplayName
    {
        get
        {
            var parts = new List<string>();
            
            if (!string.IsNullOrWhiteSpace(ProductCode))
                parts.Add(ProductCode);
            
            if (!string.IsNullOrWhiteSpace(ProductName))
                parts.Add(ProductName);
            
            if (!string.IsNullOrWhiteSpace(VariantCode))
                parts.Add($"Biến thể: {VariantCode}");
            
            if (!string.IsNullOrWhiteSpace(VariantFullName))
                parts.Add(VariantFullName);

            return parts.Count > 0 ? string.Join(" - ", parts) : "Không có thông tin";
        }
    }

    #endregion

    #region Legacy Properties (for backward compatibility)

    /// <summary>
    /// Đường dẫn hình ảnh (backward compatibility - map từ RelativePath hoặc FullPath)
    /// </summary>
    [DisplayName("Đường dẫn hình ảnh")]
    [Display(Order = 4)]
    [Obsolete("Sử dụng RelativePath hoặc FullPath thay thế")]
    public string ImagePath
    {
        get => RelativePath ?? FullPath;
        set
        {
            if (string.IsNullOrEmpty(RelativePath))
                RelativePath = value;
        }
    }

    /// <summary>
    /// Thứ tự sắp xếp (backward compatibility - map từ ImageSequenceNumber)
    /// </summary>
    [DisplayName("Thứ tự")]
    [Display(Order = 6)]
    [Obsolete("Sử dụng ImageSequenceNumber thay thế")]
    public int SortOrder
    {
        get => ImageSequenceNumber;
        set => ImageSequenceNumber = value;
    }

    /// <summary>
    /// Có phải ảnh chính không (backward compatibility - map từ ImageSequenceNumber == 1)
    /// </summary>
    [DisplayName("Ảnh chính")]
    [Display(Order = 6)]
    [Obsolete("Sử dụng ImageSequenceNumber == 1 để xác định ảnh chính")]
    public bool IsPrimary
    {
        get => ImageSequenceNumber == 1;
        set { /* No-op, computed from ImageSequenceNumber */ }
    }

    /// <summary>
    /// Loại hình ảnh (backward compatibility - map từ FileExtension hoặc MimeType)
    /// </summary>
    [DisplayName("Loại hình ảnh")]
    [Display(Order = 8)]
    [Obsolete("Sử dụng FileExtension hoặc MimeType thay thế")]
    public string ImageType
    {
        get => FileExtension ?? MimeType;
        set
        {
            if (string.IsNullOrEmpty(FileExtension))
                FileExtension = value;
        }
    }

    /// <summary>
    /// Kích thước file (backward compatibility - map từ FileSize)
    /// </summary>
    [DisplayName("Kích thước")]
    [Display(Order = 7)]
    [Obsolete("Sử dụng FileSize thay thế")]
    public long ImageSize
    {
        get => FileSize ?? 0;
        set => FileSize = value;
    }

    /// <summary>
    /// Chiều rộng hình ảnh (backward compatibility - không còn trong schema mới)
    /// </summary>
    [DisplayName("Chiều rộng")]
    [Display(Order = 9)]
    [Obsolete("Property này không còn trong schema mới")]
    public int ImageWidth { get; set; }

    /// <summary>
    /// Chiều cao hình ảnh (backward compatibility - không còn trong schema mới)
    /// </summary>
    [DisplayName("Chiều cao")]
    [Display(Order = 10)]
    [Obsolete("Property này không còn trong schema mới")]
    public int ImageHeight { get; set; }

    /// <summary>
    /// Chú thích hình ảnh (backward compatibility - map từ FileName)
    /// </summary>
    [DisplayName("Chú thích")]
    [Display(Order = 7)]
    [Obsolete("Sử dụng FileName hoặc DisplayCaption thay thế")]
    public string Caption
    {
        get => FileName ?? DisplayCaption;
        set
        {
            if (string.IsNullOrEmpty(FileName))
                FileName = value;
        }
    }

    /// <summary>
    /// Text thay thế (backward compatibility - map từ FileName)
    /// </summary>
    [DisplayName("Alt Text")]
    [Display(Order = 7)]
    [Obsolete("Sử dụng FileName thay thế")]
    public string AltText
    {
        get => FileName;
        set
        {
            if (string.IsNullOrEmpty(FileName))
                FileName = value;
        }
    }

    /// <summary>
    /// Có hoạt động không (backward compatibility - mặc định true)
    /// </summary>
    [DisplayName("Hoạt động")]
    [Display(Order = 11)]
    [Obsolete("Property này không còn trong schema mới, mặc định true")]
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Ngày tạo (backward compatibility - map từ CreateDate)
    /// </summary>
    [DisplayName("Ngày tạo")]
    [Display(Order = 20)]
    [Obsolete("Sử dụng CreateDate thay thế")]
    public DateTime CreatedDate
    {
        get => CreateDate;
        set => CreateDate = value;
    }

    /// <summary>
    /// ID biến thể sản phẩm (backward compatibility - không còn trong schema mới)
    /// </summary>
    [DisplayName("ID Biến thể")]
    [Display(Order = 3)]
    [Obsolete("Property này không còn trong schema mới, ProductImage chỉ có ProductId")]
    public Guid? VariantId { get; set; }

    /// <summary>
    /// Kích thước file dạng hiển thị (backward compatibility - map từ FileSizeDisplay)
    /// </summary>
    [DisplayName("Kích thước hiển thị")]
    [Display(Order = 7)]
    [Obsolete("Sử dụng FileSizeDisplay thay thế")]
    public string FormattedImageSize => FileSizeDisplay;

    /// <summary>
    /// Kích thước hình ảnh dạng hiển thị (backward compatibility - không còn ImageWidth/ImageHeight)
    /// </summary>
    [DisplayName("Kích thước hình")]
    [Display(Order = 9)]
    [Obsolete("Property này không còn trong schema mới")]
    public string FormattedImageDimensions
    {
        get
        {
            if (ImageWidth > 0 && ImageHeight > 0)
                return $"{ImageWidth} x {ImageHeight}";
            return "";
        }
    }

    /// <summary>
    /// Trạng thái hiển thị (backward compatibility)
    /// </summary>
    [DisplayName("Trạng thái")]
    [Display(Order = 11)]
    [Obsolete("Sử dụng HasFile thay thế")]
    public string StatusDisplay => HasFile ? "Có file" : "Không có file";

    #endregion
}
