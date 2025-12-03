using System;
using System.Threading.Tasks;

namespace Bll.Common.ImageStorage
{
    /// <summary>
    /// Interface cho File Storage Service (tổng quát cho mọi loại file)
    /// Hỗ trợ nhiều loại storage: NAS, Local, Cloud
    /// Mở rộng từ IImageStorageService để hỗ trợ lưu trữ nhiều loại file khác nhau
    /// </summary>
    public interface IFileStorageService
    {
        /// <summary>
        /// Lưu file vào storage (hỗ trợ mọi loại file: images, PDF, DOCX, XLSX, etc.)
        /// </summary>
        /// <param name="fileData">Dữ liệu file (byte array)</param>
        /// <param name="fileName">Tên file</param>
        /// <param name="category">Danh mục file (FileCategory hoặc ImageCategory)</param>
        /// <param name="entityId">ID của entity (optional, dùng để tổ chức thư mục)</param>
        /// <param name="generateThumbnail">Có tạo thumbnail không (chỉ áp dụng cho image files)</param>
        /// <returns>Kết quả lưu trữ</returns>
        Task<FileStorageResult> SaveFileAsync(
            byte[] fileData,
            string fileName,
            FileCategory category,
            Guid? entityId = null,
            bool generateThumbnail = false);

        /// <summary>
        /// Lấy file từ storage
        /// </summary>
        /// <param name="relativePath">Đường dẫn tương đối</param>
        /// <returns>Dữ liệu file (byte array)</returns>
        Task<byte[]> GetFileAsync(string relativePath);

        /// <summary>
        /// Lấy thumbnail từ storage (chỉ áp dụng cho image files)
        /// </summary>
        /// <param name="relativePath">Đường dẫn tương đối của file gốc</param>
        /// <returns>Dữ liệu thumbnail (byte array) hoặc null nếu không phải image</returns>
        Task<byte[]> GetThumbnailAsync(string relativePath);

        /// <summary>
        /// Xóa file từ storage
        /// </summary>
        /// <param name="relativePath">Đường dẫn tương đối</param>
        /// <returns>True nếu xóa thành công</returns>
        Task<bool> DeleteFileAsync(string relativePath);

        /// <summary>
        /// Kiểm tra file tồn tại
        /// </summary>
        /// <param name="relativePath">Đường dẫn tương đối</param>
        /// <returns>True nếu file tồn tại</returns>
        Task<bool> FileExistsAsync(string relativePath);

        /// <summary>
        /// Verify file integrity bằng checksum
        /// </summary>
        /// <param name="relativePath">Đường dẫn tương đối</param>
        /// <param name="checksum">Checksum để so sánh</param>
        /// <returns>True nếu checksum khớp</returns>
        Task<bool> VerifyFileAsync(string relativePath, string checksum);

        /// <summary>
        /// Generate thumbnail từ original file (chỉ áp dụng cho image files)
        /// </summary>
        /// <param name="originalRelativePath">Đường dẫn tương đối của file gốc</param>
        /// <param name="width">Chiều rộng thumbnail</param>
        /// <param name="height">Chiều cao thumbnail</param>
        /// <returns>Đường dẫn tương đối của thumbnail hoặc null nếu không phải image</returns>
        Task<string> GenerateThumbnailAsync(string originalRelativePath, int width = 200, int height = 200);

        /// <summary>
        /// Tính checksum của file
        /// </summary>
        /// <param name="relativePath">Đường dẫn tương đối</param>
        /// <returns>Checksum (MD5 hoặc SHA256)</returns>
        Task<string> CalculateChecksumAsync(string relativePath);
    }
}

