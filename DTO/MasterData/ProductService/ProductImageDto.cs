using System;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace DTO.MasterData.ProductService;

/// <summary>
/// DTO cho thông tin hình ảnh sản phẩm
/// </summary>
public class ProductImageDto
{
    #region Basic Properties

    /// <summary>
    /// ID hình ảnh
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// ID sản phẩm
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// ID biến thể sản phẩm
    /// </summary>
    public Guid? VariantId { get; set; }

    /// <summary>
    /// Đường dẫn hình ảnh
    /// </summary>
    public string ImagePath { get; set; }

    /// <summary>
    /// Thứ tự sắp xếp
    /// </summary>
    public int SortOrder { get; set; }

    /// <summary>
    /// Có phải ảnh chính không
    /// </summary>
    public bool IsPrimary { get; set; }

    /// <summary>
    /// Dữ liệu hình ảnh (byte array)
    /// </summary>
    public byte[] ImageData { get; set; }

    /// <summary>
    /// Loại hình ảnh
    /// </summary>
    public string ImageType { get; set; }

    /// <summary>
    /// Kích thước file (bytes)
    /// </summary>
    public long ImageSize { get; set; }

    /// <summary>
    /// Chiều rộng hình ảnh (pixels)
    /// </summary>
    public int ImageWidth { get; set; }

    /// <summary>
    /// Chiều cao hình ảnh (pixels)
    /// </summary>
    public int ImageHeight { get; set; }

    /// <summary>
    /// Chú thích hình ảnh
    /// </summary>
    public string Caption { get; set; }

    /// <summary>
    /// Text thay thế
    /// </summary>
    public string AltText { get; set; }

    /// <summary>
    /// Có hoạt động không
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Ngày tạo
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Ngày sửa
    /// </summary>
    public DateTime? ModifiedDate { get; set; }

    #endregion

    #region Display Properties

    /// <summary>
    /// Mã sản phẩm
    /// </summary>
    [Display(Name = "Mã sản phẩm")]
    public string ProductCode { get; set; }

    /// <summary>
    /// Tên sản phẩm
    /// </summary>
    [Display(Name = "Tên sản phẩm")]
    public string ProductName { get; set; }

    /// <summary>
    /// Mã biến thể sản phẩm
    /// </summary>
    [Display(Name = "Mã biến thể")]
    public string VariantCode { get; set; }

    /// <summary>
    /// Tên biến thể sản phẩm đầy đủ
    /// </summary>
    [Display(Name = "Tên biến thể")]
    public string VariantFullName { get; set; }

    /// <summary>
    /// Tên đơn vị tính
    /// </summary>
    [Display(Name = "Đơn vị tính")]
    public string UnitName { get; set; }

    /// <summary>
    /// Tên file hình ảnh
    /// </summary>
    [Display(Name = "Tên file")]
    public string FileName { get; set; }

    /// <summary>
    /// Kích thước file dạng hiển thị (KB, MB)
    /// </summary>
    [Display(Name = "Kích thước")]
    public string FormattedImageSize { get; set; }

    /// <summary>
    /// Kích thước hình ảnh dạng hiển thị (WxH)
    /// </summary>
    [Display(Name = "Kích thước hình")]
    public string FormattedImageDimensions { get; set; }

    /// <summary>
    /// Đường dẫn đầy đủ của hình ảnh
    /// </summary>
    public string FullImagePath { get; set; }

    /// <summary>
    /// Tên hiển thị đầy đủ cho sản phẩm/biến thể
    /// </summary>
    [Display(Name = "Sản phẩm")]
    public string ProductDisplayName { get; set; }

    /// <summary>
    /// Tên hiển thị cho hình ảnh
    /// </summary>
    [Display(Name = "Hình ảnh")]
    public string ImageDisplayName { get; set; }

    /// <summary>
    /// Trạng thái hiển thị
    /// </summary>
    [Display(Name = "Trạng thái")]
    public string StatusDisplay { get; set; }

    #endregion

    #region Methods

    /// <summary>
    /// Cập nhật các thuộc tính hiển thị
    /// </summary>
    public void UpdateDisplayProperties()
    {
        try
        {
            // Cập nhật tên file từ đường dẫn
            if (!string.IsNullOrEmpty(ImagePath))
            {
                FileName = Path.GetFileName(ImagePath);
            }

            // Cập nhật kích thước file
            if (ImageSize > 0)
            {
                if (ImageSize < 1024)
                    FormattedImageSize = $"{ImageSize} B";
                else if (ImageSize < 1024 * 1024)
                    FormattedImageSize = $"{ImageSize / 1024:F1} KB";
                else
                    FormattedImageSize = $"{ImageSize / (1024 * 1024):F1} MB";
            }

            // Cập nhật kích thước hình ảnh
            if (ImageWidth > 0 && ImageHeight > 0)
            {
                FormattedImageDimensions = $"{ImageWidth} x {ImageHeight}";
            }

            // Cập nhật đường dẫn đầy đủ
            FullImagePath = ImagePath;

            // Cập nhật tên hiển thị hình ảnh
            ImageDisplayName = !string.IsNullOrEmpty(Caption) ? Caption : FileName;

            // Cập nhật trạng thái
            StatusDisplay = IsActive ? "Hoạt động" : "Không hoạt động";
        }
        catch (Exception)
        {
            // Ignore errors in display properties update
        }
    }

    #endregion
}