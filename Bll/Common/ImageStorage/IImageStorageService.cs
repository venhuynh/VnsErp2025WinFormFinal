using System;
using System.Threading.Tasks;

namespace Bll.Common.ImageStorage
{
    /// <summary>
    /// Interface cho Image Storage Service
    /// Hỗ trợ nhiều loại storage: NAS, Local, Cloud
    /// </summary>
    public interface IImageStorageService
    {
        /// <summary>
        /// Lưu hình ảnh vào storage
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
        /// Lấy hình ảnh từ storage
        /// </summary>
        /// <param name="relativePath">Đường dẫn tương đối</param>
        /// <returns>Dữ liệu hình ảnh (byte array)</returns>
        Task<byte[]> GetImageAsync(string relativePath);

        /// <summary>
        /// Lấy thumbnail từ storage
        /// </summary>
        /// <param name="relativePath">Đường dẫn tương đối của ảnh gốc</param>
        /// <returns>Dữ liệu thumbnail (byte array)</returns>
        Task<byte[]> GetThumbnailAsync(string relativePath);

        /// <summary>
        /// Xóa hình ảnh từ storage
        /// </summary>
        /// <param name="relativePath">Đường dẫn tương đối</param>
        /// <returns>True nếu xóa thành công</returns>
        Task<bool> DeleteImageAsync(string relativePath);

        /// <summary>
        /// Kiểm tra file tồn tại
        /// </summary>
        /// <param name="relativePath">Đường dẫn tương đối</param>
        /// <returns>True nếu file tồn tại</returns>
        Task<bool> ImageExistsAsync(string relativePath);

        /// <summary>
        /// Verify file integrity bằng checksum
        /// </summary>
        /// <param name="relativePath">Đường dẫn tương đối</param>
        /// <param name="checksum">Checksum để so sánh</param>
        /// <returns>True nếu checksum khớp</returns>
        Task<bool> VerifyImageAsync(string relativePath, string checksum);

        /// <summary>
        /// Generate thumbnail từ original image
        /// </summary>
        /// <param name="originalRelativePath">Đường dẫn tương đối của ảnh gốc</param>
        /// <param name="width">Chiều rộng thumbnail</param>
        /// <param name="height">Chiều cao thumbnail</param>
        /// <returns>Đường dẫn tương đối của thumbnail</returns>
        Task<string> GenerateThumbnailAsync(string originalRelativePath, int width = 200, int height = 200);

        /// <summary>
        /// Tính checksum của file
        /// </summary>
        /// <param name="relativePath">Đường dẫn tương đối</param>
        /// <returns>Checksum (MD5 hoặc SHA256)</returns>
        Task<string> CalculateChecksumAsync(string relativePath);
    }
}

