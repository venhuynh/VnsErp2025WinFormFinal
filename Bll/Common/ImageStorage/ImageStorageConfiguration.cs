using System;
using System.Configuration;
using Dal.Configuration;

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
        /// Load configuration từ App.config
        /// </summary>
        public static ImageStorageConfiguration LoadFromConfig()
        {
            var config = new ImageStorageConfiguration();

            try
            {
                // Storage Type
                config.StorageType = GetAppSetting("ImageStorage.StorageType", "NAS");

                // NAS Configuration
                config.NASServerName = GetAppSetting("ImageStorage.NAS.ServerName", "");
                config.NASShareName = GetAppSetting("ImageStorage.NAS.ShareName", "ERP_Images");
                config.NASBasePath = GetAppSetting("ImageStorage.NAS.BasePath", "");
                config.NASUsername = GetAppSetting("ImageStorage.NAS.Username", "");
                config.NASPassword = GetAppSetting("ImageStorage.NAS.Password", "");
                config.NASProtocol = GetAppSetting("ImageStorage.NAS.Protocol", "SMB");
                config.NASConnectionTimeout = int.Parse(GetAppSetting("ImageStorage.NAS.ConnectionTimeout", "30"));
                config.NASRetryAttempts = int.Parse(GetAppSetting("ImageStorage.NAS.RetryAttempts", "3"));

                // Path Configuration
                config.ProductsPath = GetAppSetting("ImageStorage.Path.Products", "Products");
                config.StockInOutPath = GetAppSetting("ImageStorage.Path.StockInOut", "StockInOut");
                config.CompanyPath = GetAppSetting("ImageStorage.Path.Company", "Company");
                config.AvatarsPath = GetAppSetting("ImageStorage.Path.Avatars", "Avatars");
                config.TempPath = GetAppSetting("ImageStorage.Path.Temp", "Temp");

                // Thumbnail Configuration
                config.EnableThumbnailGeneration = bool.Parse(GetAppSetting("ImageStorage.Thumbnail.Enable", "true"));
                config.ThumbnailWidth = int.Parse(GetAppSetting("ImageStorage.Thumbnail.Width", "200"));
                config.ThumbnailHeight = int.Parse(GetAppSetting("ImageStorage.Thumbnail.Height", "200"));
                config.ThumbnailQuality = int.Parse(GetAppSetting("ImageStorage.Thumbnail.Quality", "80"));

                // Image Processing
                config.EnableImageCompression = bool.Parse(GetAppSetting("ImageStorage.Compression.Enable", "true"));
                config.ImageQuality = int.Parse(GetAppSetting("ImageStorage.Compression.Quality", "80"));
                config.MaxFileSize = long.Parse(GetAppSetting("ImageStorage.MaxFileSize", "10485760"));
                var allowedExts = GetAppSetting("ImageStorage.AllowedExtensions", "jpg,jpeg,png,gif,bmp,webp");
                config.AllowedExtensions = allowedExts.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                // File Management
                config.EnableFileVerification = bool.Parse(GetAppSetting("ImageStorage.Verification.Enable", "true"));
                config.FileVerificationIntervalHours = int.Parse(GetAppSetting("ImageStorage.Verification.IntervalHours", "24"));
                config.EnableAutoCleanup = bool.Parse(GetAppSetting("ImageStorage.Cleanup.Enable", "true"));
                config.OrphanedFileRetentionDays = int.Parse(GetAppSetting("ImageStorage.Cleanup.OrphanedFileRetentionDays", "30"));

                // Performance
                config.EnableCache = bool.Parse(GetAppSetting("ImageStorage.Cache.Enable", "true"));
                config.CacheSizeMB = int.Parse(GetAppSetting("ImageStorage.Cache.SizeMB", "500"));
                config.EnableAsyncProcessing = bool.Parse(GetAppSetting("ImageStorage.Async.Enable", "true"));
                config.MaxConcurrentOperations = int.Parse(GetAppSetting("ImageStorage.Async.MaxConcurrent", "10"));

                // Build NASBasePath if not provided
                if (string.IsNullOrEmpty(config.NASBasePath) && !string.IsNullOrEmpty(config.NASServerName))
                {
                    config.NASBasePath = $"{config.NASServerName}\\{config.NASShareName}";
                }
            }
            catch (Exception ex)
            {
                // Log error but continue with defaults
                System.Diagnostics.Debug.WriteLine($"Error loading ImageStorageConfiguration: {ex.Message}");
            }

            return config;
        }

        /// <summary>
        /// Get app setting với fallback
        /// </summary>
        private static string GetAppSetting(string key, string defaultValue = "")
        {
            try
            {
                var value = System.Configuration.ConfigurationManager.AppSettings[key];
                return string.IsNullOrEmpty(value) ? defaultValue : value;
            }
            catch
            {
                return defaultValue;
            }
        }

        #endregion
    }
}

