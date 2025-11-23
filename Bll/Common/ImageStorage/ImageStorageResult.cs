namespace Bll.Common.ImageStorage
{
    /// <summary>
    /// Kết quả lưu trữ hình ảnh
    /// </summary>
    public class ImageStorageResult
    {
        /// <summary>
        /// Thành công hay không
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Đường dẫn tương đối từ root của storage
        /// </summary>
        public string RelativePath { get; set; }

        /// <summary>
        /// Đường dẫn đầy đủ (UNC path cho NAS, local path cho local storage)
        /// </summary>
        public string FullPath { get; set; }

        /// <summary>
        /// Tên file
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Kích thước file (bytes)
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// Checksum (MD5 hoặc SHA256) để verify integrity
        /// </summary>
        public string Checksum { get; set; }

        /// <summary>
        /// Đường dẫn tương đối của thumbnail (nếu có)
        /// </summary>
        public string ThumbnailRelativePath { get; set; }

        /// <summary>
        /// Đường dẫn đầy đủ của thumbnail
        /// </summary>
        public string ThumbnailFullPath { get; set; }

        /// <summary>
        /// Thông báo lỗi (nếu có)
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Exception (nếu có) - chỉ dùng cho logging, không serialize
        /// </summary>
        public System.Exception Exception { get; set; }
    }
}

