using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace Bll.Common.ImageService
{
    /// <summary>
    /// Interface cho Image Service với CDN và Cache support
    /// </summary>
    public interface IImageService
{
    /// <summary>
    /// Lấy URL của hình ảnh (CDN hoặc local)
    /// </summary>
    /// <param name="imageId">ID hình ảnh</param>
    /// <param name="size">Kích thước (thumbnail, medium, large)</param>
    /// <returns>URL hình ảnh</returns>
    Task<string> GetImageUrlAsync(Guid imageId, ImageSize size = ImageSize.Original);

    /// <summary>
    /// Lấy hình ảnh từ cache hoặc tạo mới
    /// </summary>
    /// <param name="imageId">ID hình ảnh</param>
    /// <param name="size">Kích thước</param>
    /// <returns>Hình ảnh</returns>
    Task<Image> GetImageAsync(Guid imageId, ImageSize size = ImageSize.Original);

    /// <summary>
    /// Upload hình ảnh lên CDN
    /// </summary>
    /// <param name="imageData">Dữ liệu hình ảnh</param>
    /// <param name="fileName">Tên file</param>
    /// <returns>URL CDN</returns>
    Task<string> UploadToCdnAsync(byte[] imageData, string fileName);

    /// <summary>
    /// Tạo multiple sizes cho hình ảnh
    /// </summary>
    /// <param name="imageData">Dữ liệu hình ảnh gốc</param>
    /// <param name="imageId">ID hình ảnh</param>
    /// <returns>Dictionary chứa các sizes</returns>
    Task<Dictionary<ImageSize, string>> GenerateImageSizesAsync(byte[] imageData, Guid imageId);

    /// <summary>
    /// Xóa cache của hình ảnh
    /// </summary>
    /// <param name="imageId">ID hình ảnh</param>
    Task InvalidateCacheAsync(Guid imageId);
}

/// <summary>
/// Các kích thước hình ảnh được hỗ trợ
/// </summary>
public enum ImageSize
{
    Thumbnail = 150,    // 150x150
    Small = 300,        // 300x300
    Medium = 600,       // 600x600
    Large = 1200,       // 1200x1200
    Original = 0        // Kích thước gốc
}
}