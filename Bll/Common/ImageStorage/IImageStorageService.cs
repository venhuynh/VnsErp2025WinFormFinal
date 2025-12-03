using System;
using System.Threading.Tasks;

namespace Bll.Common.ImageStorage
{
    /// <summary>
    /// Interface cho Image Storage Service
    /// Kế thừa từ IFileStorageService để hỗ trợ lưu trữ nhiều loại file
    /// Giữ backward compatibility với các method riêng cho images
    /// </summary>
    public interface IImageStorageService : IFileStorageService
    {
        /// <summary>
        /// Lưu hình ảnh vào storage (backward compatibility)
        /// </summary>
        /// <param name="imageData">Dữ liệu hình ảnh (byte array)</param>
        /// <param name="fileName">Tên file</param>
        /// <param name="category">Danh mục hình ảnh</param>
        /// <param name="entityId">ID của entity (optional, dùng để tổ chức thư mục)</param>
        /// <param name="generateThumbnail">Có tạo thumbnail không</param>
        /// <returns>Kết quả lưu trữ</returns>
        Task<ImageStorageResult> SaveImageAsync(
            byte[] imageData,
            string fileName,
            ImageCategory category,
            Guid? entityId = null,
            bool generateThumbnail = false);

        /// <summary>
        /// Lấy hình ảnh từ storage (backward compatibility)
        /// </summary>
        /// <param name="relativePath">Đường dẫn tương đối</param>
        /// <returns>Dữ liệu hình ảnh (byte array)</returns>
        Task<byte[]> GetImageAsync(string relativePath);

        /// <summary>
        /// Xóa hình ảnh từ storage (backward compatibility)
        /// </summary>
        /// <param name="relativePath">Đường dẫn tương đối</param>
        /// <returns>True nếu xóa thành công</returns>
        Task<bool> DeleteImageAsync(string relativePath);

        /// <summary>
        /// Kiểm tra file tồn tại (backward compatibility)
        /// </summary>
        /// <param name="relativePath">Đường dẫn tương đối</param>
        /// <returns>True nếu file tồn tại</returns>
        Task<bool> ImageExistsAsync(string relativePath);

        /// <summary>
        /// Verify file integrity bằng checksum (backward compatibility)
        /// </summary>
        /// <param name="relativePath">Đường dẫn tương đối</param>
        /// <param name="checksum">Checksum để so sánh</param>
        /// <returns>True nếu checksum khớp</returns>
        Task<bool> VerifyImageAsync(string relativePath, string checksum);
    }
}

