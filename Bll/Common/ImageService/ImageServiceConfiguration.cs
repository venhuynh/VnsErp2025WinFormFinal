using System;

namespace Bll.Common.ImageService
{
    /// <summary>
    /// Configuration cho Image Service
    /// </summary>
    public static class ImageServiceConfiguration
    {
        #region CDN Configuration

        public static string CdnBaseUrl => GetAppSetting("CdnBaseUrl", "");
        public static bool UseCdn => bool.Parse(GetAppSetting("UseCdn", "false"));
        public static string CdnAccessKey => GetAppSetting("CdnAccessKey", "");
        public static string CdnSecretKey => GetAppSetting("CdnSecretKey", "");

        #endregion

        #region Local Storage Configuration

        public static string LocalImagePath => GetAppSetting("LocalImagePath", "~/Images/");
        public static string PhotoDirectory => GetAppSetting("PhotoDirectory", "PHOTO/PRODUCTSERVICE");
        public static bool EnableFileSystemStorage => bool.Parse(GetAppSetting("EnableFileSystemStorage", "true"));

        #endregion

        #region Cache Configuration

        public static int MemoryCacheSize => int.Parse(GetAppSetting("MemoryCacheSize", "100"));
        public static int CacheExpirationMinutes => int.Parse(GetAppSetting("CacheExpirationMinutes", "60"));
        public static bool EnableMemoryCache => bool.Parse(GetAppSetting("EnableMemoryCache", "true"));

        #endregion

        #region Image Processing Configuration

        public static int DefaultImageQuality => int.Parse(GetAppSetting("DefaultImageQuality", "80"));
        public static int MaxImageWidth => int.Parse(GetAppSetting("MaxImageWidth", "4096"));
        public static int MaxImageHeight => int.Parse(GetAppSetting("MaxImageHeight", "4096"));
        public static long MaxFileSize => long.Parse(GetAppSetting("MaxFileSize", "10485760")); // 10MB
        public static bool EnableProgressiveJpeg => bool.Parse(GetAppSetting("EnableProgressiveJpeg", "true"));

        #endregion

        #region Security Configuration

        public static bool EnableImageValidation => bool.Parse(GetAppSetting("EnableImageValidation", "true"));
        public static bool EnableMetadataSanitization => bool.Parse(GetAppSetting("EnableMetadataSanitization", "true"));
        public static bool EnableContentScanning => bool.Parse(GetAppSetting("EnableContentScanning", "false"));
        public static string[] AllowedImageFormats => GetAppSetting("AllowedImageFormats", "jpg,jpeg,png,gif,bmp,webp").Split(',');

        #endregion

        #region Cleanup Configuration

        public static bool EnableAutoCleanup => bool.Parse(GetAppSetting("EnableAutoCleanup", "true"));
        public static int CleanupIntervalDays => int.Parse(GetAppSetting("CleanupIntervalDays", "7"));
        public static int OrphanedFileRetentionDays => int.Parse(GetAppSetting("OrphanedFileRetentionDays", "30"));
        public static bool EnableDuplicateDetection => bool.Parse(GetAppSetting("EnableDuplicateDetection", "true"));

        #endregion

        #region Performance Configuration

        public static bool EnableLazyLoading => bool.Parse(GetAppSetting("EnableLazyLoading", "true"));
        public static bool EnableAsyncProcessing => bool.Parse(GetAppSetting("EnableAsyncProcessing", "true"));
        public static int MaxConcurrentOperations => int.Parse(GetAppSetting("MaxConcurrentOperations", "10"));
        public static bool EnableCompression => bool.Parse(GetAppSetting("EnableCompression", "true"));

        #endregion

        #region Logging Configuration

        public static bool EnableImageLogging => bool.Parse(GetAppSetting("EnableImageLogging", "false"));
        public static string LogLevel => GetAppSetting("ImageLogLevel", "Info");
        public static bool LogPerformanceMetrics => bool.Parse(GetAppSetting("LogPerformanceMetrics", "false"));

        #endregion

        #region Helper Methods

        /// <summary>
        /// Lấy app setting với fallback value
        /// </summary>
        private static string GetAppSetting(string key, string defaultValue = "")
        {
            // Sử dụng hardcoded values thay vì ConfigurationManager
            switch (key)
            {
                case "CdnBaseUrl": return "";
                case "UseCdn": return "false";
                case "CdnAccessKey": return "";
                case "CdnSecretKey": return "";
                case "LocalImagePath": return "~/Images/";
                case "PhotoDirectory": return "PHOTO/PRODUCTSERVICE";
                case "EnableFileSystemStorage": return "true";
                case "MemoryCacheSize": return "100";
                case "CacheExpirationMinutes": return "60";
                case "EnableMemoryCache": return "true";
                case "DefaultImageQuality": return "80";
                case "MaxImageWidth": return "4096";
                case "MaxImageHeight": return "4096";
                case "MaxFileSize": return "10485760";
                case "EnableProgressiveJpeg": return "true";
                case "EnableImageValidation": return "true";
                case "EnableMetadataSanitization": return "true";
                case "EnableContentScanning": return "false";
                case "AllowedImageFormats": return "jpg,jpeg,png,gif,bmp,webp";
                case "EnableAutoCleanup": return "true";
                case "CleanupIntervalDays": return "7";
                case "OrphanedFileRetentionDays": return "30";
                case "EnableDuplicateDetection": return "true";
                case "EnableLazyLoading": return "true";
                case "EnableAsyncProcessing": return "true";
                case "MaxConcurrentOperations": return "10";
                case "EnableCompression": return "true";
                case "EnableImageLogging": return "false";
                case "ImageLogLevel": return "Info";
                case "LogPerformanceMetrics": return "false";
                default: return defaultValue;
            }
        }

        /// <summary>
        /// Lấy full path cho local storage
        /// </summary>
        public static string GetFullLocalPath()
        {
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            return System.IO.Path.Combine(basePath, PhotoDirectory);
        }

        /// <summary>
        /// Kiểm tra xem format có được hỗ trợ không
        /// </summary>
        public static bool IsFormatAllowed(string format)
        {
            if (string.IsNullOrEmpty(format)) return false;
            
            var lowerFormat = format.ToLower().TrimStart('.');
            foreach (var allowed in AllowedImageFormats)
            {
                if (allowed.Trim().Equals(lowerFormat, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Lấy kích thước tối đa cho format cụ thể
        /// </summary>
        public static long GetMaxFileSizeForFormat(string format)
        {
            switch (format?.ToLower())
            {
                case "webp":
                    return MaxFileSize * 2; // WebP có thể lớn hơn
                case "png":
                    return MaxFileSize * 3; // PNG có thể rất lớn
                case "gif":
                    return MaxFileSize / 2; // GIF thường nhỏ hơn
                default:
                    return MaxFileSize;
            }
        }

        #endregion
    }
}
