using System;
using Bll.Common;

namespace Bll.Common.ImageStorage
{
    /// <summary>
    /// Configuration cho Image Storage Service
    /// </summary>
    public class ImageStorageConfiguration
    {
        #region Storage Type

        /// <summary>
        /// Loại storage: NAS, Local, Cloud
        /// </summary>
        public string StorageType { get; set; } = "NAS";

        #endregion

        #region NAS Configuration

        /// <summary>
        /// Tên server NAS (ví dụ: \\192.168.1.100)
        /// </summary>
        public string NASServerName { get; set; }

        /// <summary>
        /// Tên share folder trên NAS (ví dụ: ERP_Images)
        /// </summary>
        public string NASShareName { get; set; } = "ERP_Images";

        /// <summary>
        /// Đường dẫn đầy đủ đến NAS share (ví dụ: \\192.168.1.100\ERP_Images)
        /// </summary>
        public string NASBasePath { get; set; }

        /// <summary>
        /// Username để kết nối NAS
        /// </summary>
        public string NASUsername { get; set; }

        /// <summary>
        /// Password để kết nối NAS (nên được encrypt)
        /// </summary>
        public string NASPassword { get; set; }

        /// <summary>
        /// Protocol: SMB, NFS, FTP
        /// </summary>
        public string NASProtocol { get; set; } = "SMB";

        /// <summary>
        /// Timeout kết nối NAS (seconds)
        /// </summary>
        public int NASConnectionTimeout { get; set; } = 30;

        /// <summary>
        /// Số lần retry khi kết nối thất bại
        /// </summary>
        public int NASRetryAttempts { get; set; } = 3;

        #endregion

        #region Path Configuration

        /// <summary>
        /// Đường dẫn cho hình ảnh sản phẩm
        /// </summary>
        public string ProductsPath { get; set; } = "Products";

        /// <summary>
        /// Đường dẫn cho hình ảnh phiếu nhập/xuất
        /// </summary>
        public string StockInOutPath { get; set; } = "StockInOut";

        /// <summary>
        /// Đường dẫn cho logo công ty
        /// </summary>
        public string CompanyPath { get; set; } = "Company";

        /// <summary>
        /// Đường dẫn cho avatar
        /// </summary>
        public string AvatarsPath { get; set; } = "Avatars";

        /// <summary>
        /// Đường dẫn cho file tạm
        /// </summary>
        public string TempPath { get; set; } = "Temp";

        #endregion

        #region Thumbnail Configuration

        /// <summary>
        /// Bật/tắt tạo thumbnail tự động
        /// </summary>
        public bool EnableThumbnailGeneration { get; set; } = true;

        /// <summary>
        /// Chiều rộng thumbnail (pixels)
        /// </summary>
        public int ThumbnailWidth { get; set; } = 200;

        /// <summary>
        /// Chiều cao thumbnail (pixels)
        /// </summary>
        public int ThumbnailHeight { get; set; } = 200;

        /// <summary>
        /// Chất lượng thumbnail (1-100)
        /// </summary>
        public int ThumbnailQuality { get; set; } = 80;

        #endregion

        #region Image Processing

        /// <summary>
        /// Bật/tắt nén hình ảnh
        /// </summary>
        public bool EnableImageCompression { get; set; } = true;

        /// <summary>
        /// Chất lượng nén (1-100)
        /// </summary>
        public int ImageQuality { get; set; } = 80;

        /// <summary>
        /// Kích thước file tối đa (bytes)
        /// </summary>
        public long MaxFileSize { get; set; } = 10485760; // 10MB

        /// <summary>
        /// Các extension được phép
        /// </summary>
        public string[] AllowedExtensions { get; set; } = { "jpg", "jpeg", "png", "gif", "bmp", "webp" };

        #endregion

        #region File Management

        /// <summary>
        /// Bật/tắt verify file integrity
        /// </summary>
        public bool EnableFileVerification { get; set; } = true;

        /// <summary>
        /// Khoảng thời gian verify file (hours)
        /// </summary>
        public int FileVerificationIntervalHours { get; set; } = 24;

        /// <summary>
        /// Bật/tắt auto cleanup orphaned files
        /// </summary>
        public bool EnableAutoCleanup { get; set; } = true;

        /// <summary>
        /// Số ngày giữ lại orphaned files
        /// </summary>
        public int OrphanedFileRetentionDays { get; set; } = 30;

        #endregion

        #region Performance

        /// <summary>
        /// Bật/tắt cache
        /// </summary>
        public bool EnableCache { get; set; } = true;

        /// <summary>
        /// Kích thước cache (MB)
        /// </summary>
        public int CacheSizeMB { get; set; } = 500;

        /// <summary>
        /// Bật/tắt async processing
        /// </summary>
        public bool EnableAsyncProcessing { get; set; } = true;

        /// <summary>
        /// Số lượng operations đồng thời tối đa
        /// </summary>
        public int MaxConcurrentOperations { get; set; } = 10;

        #endregion

        #region Static Factory Methods

        /// <summary>
        /// Load configuration từ Database (bảng Setting)
        /// </summary>
        public static ImageStorageConfiguration LoadFromConfig()
        {
            var config = new ImageStorageConfiguration();
            var settingBll = new SettingBll();

            try
            {
                // Storage Type - Đọc từ database
                config.StorageType = settingBll.GetValue("NAS", "StorageType", "NAS");

                // NAS Configuration - Đọc từ database và giải mã nếu cần
                config.NASServerName = settingBll.GetDecryptedValue("NAS", "ServerName", "");
                config.NASShareName = settingBll.GetDecryptedValue("NAS", "ShareName", "ERP_Images");
                config.NASBasePath = settingBll.GetDecryptedValue("NAS", "BasePath", "");
                config.NASUsername = settingBll.GetDecryptedValue("NAS", "Username", "");
                config.NASPassword = settingBll.GetDecryptedValue("NAS", "Password", "");
                config.NASProtocol = settingBll.GetValue("NAS", "Protocol", "SMB");
                
                var timeoutStr = settingBll.GetValue("NAS", "ConnectionTimeout", "30");
                config.NASConnectionTimeout = int.TryParse(timeoutStr, out int timeout) ? timeout : 30;
                
                var retryStr = settingBll.GetValue("NAS", "RetryAttempts", "3");
                config.NASRetryAttempts = int.TryParse(retryStr, out int retry) ? retry : 3;

                // Path Configuration - Đọc từ database
                config.ProductsPath = settingBll.GetValue("ImageStorage", "Path.Products", "Products");
                config.StockInOutPath = settingBll.GetValue("ImageStorage", "Path.StockInOut", "StockInOut");
                config.CompanyPath = settingBll.GetValue("ImageStorage", "Path.Company", "Company");
                config.AvatarsPath = settingBll.GetValue("ImageStorage", "Path.Avatars", "Avatars");
                config.TempPath = settingBll.GetValue("ImageStorage", "Path.Temp", "Temp");

                // Thumbnail Configuration - Đọc từ database
                var thumbnailEnableStr = settingBll.GetValue("ImageStorage", "Thumbnail.Enable", "true");
                config.EnableThumbnailGeneration = bool.TryParse(thumbnailEnableStr, out bool thumbnailEnable) && thumbnailEnable;
                
                var thumbnailWidthStr = settingBll.GetValue("ImageStorage", "Thumbnail.Width", "200");
                config.ThumbnailWidth = int.TryParse(thumbnailWidthStr, out int thumbnailWidth) ? thumbnailWidth : 200;
                
                var thumbnailHeightStr = settingBll.GetValue("ImageStorage", "Thumbnail.Height", "200");
                config.ThumbnailHeight = int.TryParse(thumbnailHeightStr, out int thumbnailHeight) ? thumbnailHeight : 200;
                
                var thumbnailQualityStr = settingBll.GetValue("ImageStorage", "Thumbnail.Quality", "80");
                config.ThumbnailQuality = int.TryParse(thumbnailQualityStr, out int thumbnailQuality) ? thumbnailQuality : 80;

                // Image Processing - Đọc từ database
                var compressionEnableStr = settingBll.GetValue("ImageStorage", "Compression.Enable", "true");
                config.EnableImageCompression = bool.TryParse(compressionEnableStr, out bool compressionEnable) && compressionEnable;
                
                var imageQualityStr = settingBll.GetValue("ImageStorage", "Compression.Quality", "80");
                config.ImageQuality = int.TryParse(imageQualityStr, out int imageQuality) ? imageQuality : 80;
                
                var maxFileSizeStr = settingBll.GetValue("ImageStorage", "MaxFileSize", "10485760");
                config.MaxFileSize = long.TryParse(maxFileSizeStr, out long maxFileSize) ? maxFileSize : 10485760;
                
                var allowedExts = settingBll.GetValue("ImageStorage", "AllowedExtensions", "jpg,jpeg,png,gif,bmp,webp");
                config.AllowedExtensions = allowedExts.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                // File Management - Đọc từ database
                var verificationEnableStr = settingBll.GetValue("ImageStorage", "Verification.Enable", "true");
                config.EnableFileVerification = bool.TryParse(verificationEnableStr, out bool verificationEnable) && verificationEnable;
                
                var verificationIntervalStr = settingBll.GetValue("ImageStorage", "Verification.IntervalHours", "24");
                config.FileVerificationIntervalHours = int.TryParse(verificationIntervalStr, out int verificationInterval) ? verificationInterval : 24;
                
                var cleanupEnableStr = settingBll.GetValue("ImageStorage", "Cleanup.Enable", "true");
                config.EnableAutoCleanup = bool.TryParse(cleanupEnableStr, out bool cleanupEnable) && cleanupEnable;
                
                var retentionDaysStr = settingBll.GetValue("ImageStorage", "Cleanup.OrphanedFileRetentionDays", "30");
                config.OrphanedFileRetentionDays = int.TryParse(retentionDaysStr, out int retentionDays) ? retentionDays : 30;

                // Performance - Đọc từ database
                var cacheEnableStr = settingBll.GetValue("ImageStorage", "Cache.Enable", "true");
                config.EnableCache = bool.TryParse(cacheEnableStr, out bool cacheEnable) && cacheEnable;
                
                var cacheSizeStr = settingBll.GetValue("ImageStorage", "Cache.SizeMB", "500");
                config.CacheSizeMB = int.TryParse(cacheSizeStr, out int cacheSize) ? cacheSize : 500;
                
                var asyncEnableStr = settingBll.GetValue("ImageStorage", "Async.Enable", "true");
                config.EnableAsyncProcessing = bool.TryParse(asyncEnableStr, out bool asyncEnable) && asyncEnable;
                
                var maxConcurrentStr = settingBll.GetValue("ImageStorage", "Async.MaxConcurrent", "10");
                config.MaxConcurrentOperations = int.TryParse(maxConcurrentStr, out int maxConcurrent) ? maxConcurrent : 10;

                // Build NASBasePath if not provided
                if (string.IsNullOrEmpty(config.NASBasePath) && !string.IsNullOrEmpty(config.NASServerName))
                {
                    config.NASBasePath = $"{config.NASServerName}\\{config.NASShareName}";
                }
            }
            catch (Exception ex)
            {
                // Log error but continue with defaults
                System.Diagnostics.Debug.WriteLine($"Error loading ImageStorageConfiguration from database: {ex.Message}");
            }

            return config;
        }

        #endregion
    }
}

