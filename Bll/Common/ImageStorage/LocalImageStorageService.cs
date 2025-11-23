using System;
using System.IO;
using System.Threading.Tasks;
using Logger.Interfaces;

namespace Bll.Common.ImageStorage
{
    /// <summary>
    /// Implementation cho Local File System Storage
    /// Fallback option khi không có NAS
    /// </summary>
    public class LocalImageStorageService : IImageStorageService
    {
        #region Fields

        private readonly ImageStorageConfiguration _config;
        private readonly ILogger _logger;
        private readonly string _localBasePath;

        #endregion

        #region Constructor

        public LocalImageStorageService(ImageStorageConfiguration config, ILogger logger)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            // Use local path from config or default to application directory
            _localBasePath = string.IsNullOrEmpty(config.NASBasePath)
                ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images")
                : config.NASBasePath;

            // Ensure base directory exists
            if (!Directory.Exists(_localBasePath))
            {
                Directory.CreateDirectory(_localBasePath);
            }
        }

        #endregion

        #region IImageStorageService Implementation

        public async Task<ImageStorageResult> SaveImageAsync(
            byte[] imageData,
            string fileName,
            ImageCategory category,
            Guid? entityId = null,
            bool generateThumbnail = false)
        {
            try
            {
                _logger.Debug("LocalImageStorageService.SaveImageAsync: Bắt đầu lưu hình ảnh, FileName={0}", fileName);

                // Validate
                ValidateImage(imageData, fileName);

                // Generate path (same logic as NAS)
                var relativePath = GenerateRelativePath(category, fileName, entityId);
                var fullPath = Path.Combine(_localBasePath, relativePath);

                // Ensure directory exists
                var directory = Path.GetDirectoryName(fullPath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Save file
                await Task.Run(() => File.WriteAllBytes(fullPath, imageData));

                // Calculate checksum
                var checksum = CalculateChecksum(imageData);

                _logger.Info("LocalImageStorageService.SaveImageAsync: Đã lưu hình ảnh, Path={0}", relativePath);

                return new ImageStorageResult
                {
                    Success = true,
                    RelativePath = relativePath,
                    FullPath = fullPath,
                    FileName = fileName,
                    FileSize = imageData.Length,
                    Checksum = checksum
                };
            }
            catch (Exception ex)
            {
                _logger.Error($"LocalImageStorageService.SaveImageAsync: Lỗi {ex.Message}", ex);
                return new ImageStorageResult
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    Exception = ex
                };
            }
        }

        public async Task<byte[]> GetImageAsync(string relativePath)
        {
            try
            {
                var fullPath = Path.Combine(_localBasePath, relativePath);
                if (!File.Exists(fullPath))
                {
                    return null;
                }

                return await Task.Run(() => File.ReadAllBytes(fullPath));
            }
            catch (Exception ex)
            {
                _logger.Error($"LocalImageStorageService.GetImageAsync: Lỗi {ex.Message}", ex);
                return null;
            }
        }

        public Task<byte[]> GetThumbnailAsync(string relativePath)
        {
            // For local storage, thumbnail generation can be done on-demand
            // This is a simplified implementation
            return GetImageAsync(relativePath);
        }

        public async Task<bool> DeleteImageAsync(string relativePath)
        {
            try
            {
                var fullPath = Path.Combine(_localBasePath, relativePath);
                if (File.Exists(fullPath))
                {
                    await Task.Run(() => File.Delete(fullPath));
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.Error($"LocalImageStorageService.DeleteImageAsync: Lỗi {ex.Message}", ex);
                return false;
            }
        }

        public Task<bool> ImageExistsAsync(string relativePath)
        {
            try
            {
                var fullPath = Path.Combine(_localBasePath, relativePath);
                return Task.FromResult(File.Exists(fullPath));
            }
            catch
            {
                return Task.FromResult(false);
            }
        }

        public async Task<bool> VerifyImageAsync(string relativePath, string checksum)
        {
            try
            {
                var imageData = await GetImageAsync(relativePath);
                if (imageData == null)
                {
                    return false;
                }

                var calculatedChecksum = CalculateChecksum(imageData);
                return string.Equals(calculatedChecksum, checksum, StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        public Task<string> GenerateThumbnailAsync(string originalRelativePath, int width = 200, int height = 200)
        {
            // Delegate to NAS implementation logic if needed
            // For now, return null as placeholder
            return Task.FromResult<string>(null);
        }

        public async Task<string> CalculateChecksumAsync(string relativePath)
        {
            try
            {
                var imageData = await GetImageAsync(relativePath);
                if (imageData == null)
                {
                    return null;
                }

                return CalculateChecksum(imageData);
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region Private Methods

        private void ValidateImage(byte[] imageData, string fileName)
        {
            if (imageData == null || imageData.Length == 0)
            {
                throw new ArgumentException("ImageData cannot be null or empty", nameof(imageData));
            }

            if (imageData.Length > _config.MaxFileSize)
            {
                throw new ArgumentException($"File size exceeds maximum allowed size", nameof(imageData));
            }

            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentException("FileName cannot be null or empty", nameof(fileName));
            }
        }

        private string GenerateRelativePath(ImageCategory category, string fileName, Guid? entityId)
        {
            var year = DateTime.Now.Year;
            var month = DateTime.Now.Month.ToString("00");

            return category switch
            {
                ImageCategory.Product => entityId.HasValue
                    ? $"{_config.ProductsPath}/{entityId.Value}/{year}/{month}/{fileName}"
                    : $"{_config.ProductsPath}/{year}/{month}/{fileName}",
                ImageCategory.StockInOut => $"{_config.StockInOutPath}/{year}/{month}/{fileName}",
                ImageCategory.Company => entityId.HasValue
                    ? $"{_config.CompanyPath}/{entityId.Value}_{fileName}"
                    : $"{_config.CompanyPath}/{fileName}",
                ImageCategory.Avatar => entityId.HasValue
                    ? $"{_config.AvatarsPath}/{entityId.Value}_{fileName}"
                    : $"{_config.AvatarsPath}/{fileName}",
                _ => $"{_config.TempPath}/{fileName}"
            };
        }

        private string CalculateChecksum(byte[] data)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                var hash = md5.ComputeHash(data);
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }

        #endregion
    }
}

