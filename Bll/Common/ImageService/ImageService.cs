using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bll.MasterData.ProductService;
using Dal.Configuration;
using Dal.DataAccess.MasterData.ProductServiceDataAccess;

namespace Bll.Common.ImageService
{
    /// <summary>
    /// Image Service với CDN và Cache support
    /// </summary>
    public class ImageService : IImageService
    {
        #region Fields

        private readonly ProductImageDataAccess _imageDataAccess;
        private readonly string _cdnBaseUrl;
        private readonly string _localImagePath;
        private readonly bool _useCdn;
        private readonly Dictionary<string, Image> _memoryCache;

        #endregion

        #region Constructor

        public ImageService()
        {
            _imageDataAccess = new ProductImageDataAccess();
            _cdnBaseUrl = GetAppSetting("CdnBaseUrl", "");
            _localImagePath = GetAppSetting("LocalImagePath", "~/Images/");
            _useCdn = bool.Parse(GetAppSetting("UseCdn", "false"));
            _memoryCache = new Dictionary<string, Image>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Lấy URL của hình ảnh (CDN hoặc local)
        /// </summary>
        public async Task<string> GetImageUrlAsync(Guid imageId, ImageSize size = ImageSize.Original)
        {
            try
            {
                var cacheKey = $"url_{imageId}_{size}";
                
                if (_useCdn && !string.IsNullOrEmpty(_cdnBaseUrl))
                {
                    // Sử dụng CDN URL
                    var fileName = await GetCdnFileNameAsync(imageId, size);
                    return $"{_cdnBaseUrl.TrimEnd('/')}/{fileName}";
                }
                else
                {
                    // Sử dụng local URL
                    return $"/api/images/{imageId}?size={size}";
                }
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi lấy URL hình ảnh '{imageId}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy hình ảnh từ cache hoặc tạo mới
        /// </summary>
        public async Task<Image> GetImageAsync(Guid imageId, ImageSize size = ImageSize.Original)
        {
            try
            {
                var cacheKey = $"{imageId}_{size}";
                
                // Kiểm tra memory cache
                if (_memoryCache.ContainsKey(cacheKey))
                {
                    return _memoryCache[cacheKey];
                }

                // Lấy dữ liệu hình ảnh từ database
                var imageData = _imageDataAccess.GetImageData(imageId);
                if (imageData == null || imageData.Length == 0)
                {
                    return null;
                }

                // Tạo hình ảnh với kích thước phù hợp
                Image result;
                using (var originalImage = Image.FromStream(new MemoryStream(imageData)))
                {
                    if (size == ImageSize.Original)
                    {
                        result = new Bitmap(originalImage);
                    }
                    else
                    {
                        result = await ResizeImageAsync(originalImage, (int)size);
                    }
                }

                // Cache trong memory (giới hạn 100 items)
                if (_memoryCache.Count >= 100)
                {
                    var firstKey = _memoryCache.Keys.First();
                    _memoryCache[firstKey]?.Dispose();
                    _memoryCache.Remove(firstKey);
                }
                _memoryCache[cacheKey] = result;

                return result;
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi lấy hình ảnh '{imageId}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Upload hình ảnh lên CDN
        /// </summary>
        public async Task<string> UploadToCdnAsync(byte[] imageData, string fileName)
        {
            try
            {
                if (!_useCdn || string.IsNullOrEmpty(_cdnBaseUrl))
                {
                    throw new BusinessLogicException("CDN không được cấu hình");
                }

                // TODO: Implement actual CDN upload logic (AWS S3, Azure Blob, etc.)
                // Đây là placeholder implementation
                await Task.Delay(100); // Simulate upload time
                
                return $"{_cdnBaseUrl.TrimEnd('/')}/{fileName}";
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi upload lên CDN: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Tạo multiple sizes cho hình ảnh
        /// </summary>
        public async Task<Dictionary<ImageSize, string>> GenerateImageSizesAsync(byte[] imageData, Guid imageId)
        {
            try
            {
                var result = new Dictionary<ImageSize, string>();
                
                using (var originalImage = Image.FromStream(new MemoryStream(imageData)))
                {
                    foreach (ImageSize size in Enum.GetValues(typeof(ImageSize)))
                    {
                        if (size == ImageSize.Original) continue;

                        var resizedImage = await ResizeImageAsync(originalImage, (int)size);
                        var fileName = $"{imageId}_{size}.jpg";
                        
                        // Lưu file hoặc upload lên CDN
                        if (_useCdn)
                        {
                            using (var ms = new MemoryStream())
                            {
                                resizedImage.Save(ms, ImageFormat.Jpeg);
                                var url = await UploadToCdnAsync(ms.ToArray(), fileName);
                                result[size] = url;
                            }
                        }
                        else
                        {
                            var filePath = Path.Combine(_localImagePath, fileName);
                            resizedImage.Save(filePath, ImageFormat.Jpeg);
                            result[size] = $"/Images/{fileName}";
                        }
                        
                        resizedImage.Dispose();
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi tạo multiple sizes: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Xóa cache của hình ảnh
        /// </summary>
        public async Task InvalidateCacheAsync(Guid imageId)
        {
            try
            {
                var keysToRemove = _memoryCache.Keys.Where(k => k.StartsWith(imageId.ToString())).ToList();
                
                foreach (var key in keysToRemove)
                {
                    _memoryCache[key]?.Dispose();
                    _memoryCache.Remove(key);
                }

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi xóa cache hình ảnh '{imageId}': {ex.Message}", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Lấy app setting với fallback value
        /// </summary>
        private string GetAppSetting(string key, string defaultValue = "")
        {
            // Sử dụng hardcoded values thay vì ConfigurationManager
            switch (key)
            {
                case "CdnBaseUrl": return "";
                case "LocalImagePath": return "~/Images/";
                case "UseCdn": return "false";
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
        /// Resize hình ảnh với chất lượng cao
        /// </summary>
        private async Task<Image> ResizeImageAsync(Image originalImage, int maxSize)
        {
            try
            {
                var newSize = CalculateNewSize(originalImage.Width, originalImage.Height, maxSize);
                
                var resizedImage = new Bitmap(newSize.Width, newSize.Height);
                using (var graphics = Graphics.FromImage(resizedImage))
                {
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    
                    graphics.DrawImage(originalImage, 0, 0, newSize.Width, newSize.Height);
                }

                await Task.CompletedTask;
                return resizedImage;
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi resize hình ảnh: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Tính toán kích thước mới
        /// </summary>
        private Size CalculateNewSize(int originalWidth, int originalHeight, int maxSize)
        {
            if (originalWidth <= maxSize && originalHeight <= maxSize)
            {
                return new Size(originalWidth, originalHeight);
            }

            double ratio = Math.Min((double)maxSize / originalWidth, (double)maxSize / originalHeight);
            
            return new Size(
                (int)(originalWidth * ratio),
                (int)(originalHeight * ratio)
            );
        }

        /// <summary>
        /// Lấy tên file CDN
        /// </summary>
        private async Task<string> GetCdnFileNameAsync(Guid imageId, ImageSize size)
        {
            // TODO: Implement logic để lấy tên file từ database hoặc generate
            await Task.CompletedTask;
            return $"{imageId}_{size}.jpg";
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            foreach (var image in _memoryCache.Values)
            {
                image?.Dispose();
            }
            _memoryCache.Clear();
        }

        #endregion
    }
}
