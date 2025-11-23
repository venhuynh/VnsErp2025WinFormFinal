using Logger.Interfaces;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
// ReSharper disable InconsistentNaming

namespace Bll.Common.ImageStorage
{
    /// <summary>
    /// Implementation cho NAS Storage (Synology)
    /// Sử dụng SMB/CIFS protocol để truy cập NAS
    /// </summary>
    public class NASImageStorageService : IImageStorageService
    {
        #region Fields

        private readonly ImageStorageConfiguration _config;
        private readonly ILogger _logger;

        #endregion

        #region Constructor

        public NASImageStorageService(ImageStorageConfiguration config, ILogger logger)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            // Validate configuration
            if (string.IsNullOrEmpty(_config.NASBasePath))
            {
                throw new ArgumentException(@"NASBasePath is required", nameof(config));
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
                _logger.Debug("SaveImageAsync: Bắt đầu lưu hình ảnh, FileName={0}, Category={1}", fileName, category);

                // 1. Validate
                ValidateImage(imageData, fileName);

                // 2. Generate file path
                var relativePath = GenerateRelativePath(category, fileName, entityId);
                var fullPath = Path.Combine(_config.NASBasePath, relativePath);

                // 3. Ensure directory exists
                var directory = Path.GetDirectoryName(fullPath);
                if (!Directory.Exists(directory))
                {
                    if (directory != null)
                    {
                        Directory.CreateDirectory(directory);
                        _logger.Debug("SaveImageAsync: Đã tạo thư mục {0}", directory);
                    }
                }

                // 4. Save file
                await Task.Run(() => File.WriteAllBytes(fullPath, imageData));

                // 5. Calculate checksum
                var checksum = CalculateChecksum(imageData);

                // 6. Generate thumbnail if requested
                string thumbnailPath = null;
                string thumbnailFullPath = null;
                if (generateThumbnail && _config.EnableThumbnailGeneration)
                {
                    thumbnailPath = await GenerateThumbnailAsync(relativePath, _config.ThumbnailWidth, _config.ThumbnailHeight);
                    if (!string.IsNullOrEmpty(thumbnailPath))
                    {
                        thumbnailFullPath = Path.Combine(_config.NASBasePath, thumbnailPath);
                    }
                }

                _logger.Info("SaveImageAsync: Đã lưu hình ảnh thành công, RelativePath={0}, FileSize={1}", relativePath, imageData.Length);

                return new ImageStorageResult
                {
                    Success = true,
                    RelativePath = relativePath,
                    FullPath = fullPath,
                    FileName = fileName,
                    FileSize = imageData.Length,
                    Checksum = checksum,
                    ThumbnailRelativePath = thumbnailPath,
                    ThumbnailFullPath = thumbnailFullPath
                };
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.Error("SaveImageAsync: Lỗi quyền truy cập NAS", ex);
                return new ImageStorageResult
                {
                    Success = false,
                    ErrorMessage = $"Không có quyền truy cập NAS: {ex.Message}",
                    Exception = ex
                };
            }
            catch (IOException ex)
            {
                _logger.Error("SaveImageAsync: Lỗi I/O khi lưu file", ex);
                return new ImageStorageResult
                {
                    Success = false,
                    ErrorMessage = $"Lỗi I/O: {ex.Message}",
                    Exception = ex
                };
            }
            catch (Exception ex)
            {
                _logger.Error($"SaveImageAsync: Lỗi không xác định: {ex.Message}", ex);
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
                if (string.IsNullOrEmpty(relativePath))
                {
                    throw new ArgumentException(@"RelativePath cannot be null or empty", nameof(relativePath));
                }

                var fullPath = Path.Combine(_config.NASBasePath, relativePath);

                if (!File.Exists(fullPath))
                {
                    _logger.Warning("GetImageAsync: File không tồn tại, Path={0}", fullPath);
                    return null;
                }

                var imageData = await Task.Run(() => File.ReadAllBytes(fullPath));
                _logger.Debug("GetImageAsync: Đã đọc file, Path={0}, Size={1}", relativePath, imageData.Length);

                return imageData;
            }
            catch (Exception ex)
            {
                _logger.Error($"GetImageAsync: Lỗi đọc file {relativePath}: {ex.Message}", ex);
                throw;
            }
        }

        public async Task<byte[]> GetThumbnailAsync(string relativePath)
        {
            try
            {
                if (string.IsNullOrEmpty(relativePath))
                {
                    throw new ArgumentException(@"RelativePath cannot be null or empty", nameof(relativePath));
                }

                // Thumbnail path: {originalPath}_thumb.jpg
                var thumbnailPath = GetThumbnailPath(relativePath);
                var thumbnailFullPath = Path.Combine(_config.NASBasePath, thumbnailPath);

                if (!File.Exists(thumbnailFullPath))
                {
                    _logger.Debug("GetThumbnailAsync: Thumbnail không tồn tại, tạo mới, Path={0}", relativePath);
                    // Generate thumbnail on the fly
                    await GenerateThumbnailAsync(relativePath, _config.ThumbnailWidth, _config.ThumbnailHeight);
                }

                if (File.Exists(thumbnailFullPath))
                {
                    var thumbnailData = await Task.Run(() => File.ReadAllBytes(thumbnailFullPath));
                    return thumbnailData;
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.Error($"GetThumbnailAsync: Lỗi đọc thumbnail {relativePath}: {ex.Message}", ex);
                return null;
            }
        }

        public async Task<bool> DeleteImageAsync(string relativePath)
        {
            try
            {
                if (string.IsNullOrEmpty(relativePath))
                {
                    return false;
                }

                var fullPath = Path.Combine(_config.NASBasePath, relativePath);

                if (!File.Exists(fullPath))
                {
                    _logger.Warning("DeleteImageAsync: File không tồn tại, Path={0}", fullPath);
                    return false;
                }

                await Task.Run(() => File.Delete(fullPath));

                // Also delete thumbnail if exists
                var thumbnailPath = GetThumbnailPath(relativePath);
                var thumbnailFullPath = Path.Combine(_config.NASBasePath, thumbnailPath);
                if (File.Exists(thumbnailFullPath))
                {
                    await Task.Run(() => File.Delete(thumbnailFullPath));
                }

                _logger.Info("DeleteImageAsync: Đã xóa file, Path={0}", relativePath);
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error($"DeleteImageAsync: Lỗi xóa file {relativePath}: {ex.Message}", ex);
                return false;
            }
        }

        public async Task<bool> ImageExistsAsync(string relativePath)
        {
            try
            {
                if (string.IsNullOrEmpty(relativePath))
                {
                    return false;
                }

                var fullPath = Path.Combine(_config.NASBasePath, relativePath);
                return await Task.Run(() => File.Exists(fullPath));
            }
            catch (Exception ex)
            {
                _logger.Error($"ImageExistsAsync: Lỗi kiểm tra file {relativePath}: {ex.Message}", ex);
                return false;
            }
        }

        public async Task<bool> VerifyImageAsync(string relativePath, string checksum)
        {
            try
            {
                if (string.IsNullOrEmpty(relativePath) || string.IsNullOrEmpty(checksum))
                {
                    return false;
                }

                var imageData = await GetImageAsync(relativePath);
                if (imageData == null)
                {
                    return false;
                }

                var calculatedChecksum = CalculateChecksum(imageData);
                return string.Equals(calculatedChecksum, checksum, StringComparison.OrdinalIgnoreCase);
            }
            catch (Exception ex)
            {
                _logger.Error($"VerifyImageAsync: Lỗi verify file {relativePath}: {ex.Message}", ex);
                return false;
            }
        }

        public async Task<string> GenerateThumbnailAsync(string originalRelativePath, int width = 200, int height = 200)
        {
            try
            {
                if (string.IsNullOrEmpty(originalRelativePath))
                {
                    throw new ArgumentException(@"OriginalRelativePath cannot be null or empty", nameof(originalRelativePath));
                }

                var originalFullPath = Path.Combine(_config.NASBasePath, originalRelativePath);
                if (!File.Exists(originalFullPath))
                {
                    _logger.Warning("GenerateThumbnailAsync: File gốc không tồn tại, Path={0}", originalFullPath);
                    return null;
                }

                // Read original image
                byte[] imageData;
                using (var fs = new FileStream(originalFullPath, FileMode.Open, FileAccess.Read))
                {
                    imageData = new byte[fs.Length];
                    // ReSharper disable once MustUseReturnValue
                    await fs.ReadAsync(imageData, 0, (int)fs.Length).ConfigureAwait(false);
                }

                // Generate thumbnail
                byte[] thumbnailData;
                using (var originalImage = Image.FromStream(new MemoryStream(imageData)))
                {
                    var thumbnail = ResizeImage(originalImage, width, height);
                    thumbnailData = ImageToByteArray(thumbnail, _config.ThumbnailQuality);
                    thumbnail.Dispose();
                }

                // Save thumbnail
                var thumbnailPath = GetThumbnailPath(originalRelativePath);
                var thumbnailFullPath = Path.Combine(_config.NASBasePath, thumbnailPath);

                var thumbnailDirectory = Path.GetDirectoryName(thumbnailFullPath);
                if (!Directory.Exists(thumbnailDirectory))
                {
                    if (thumbnailDirectory != null) 
                        Directory.CreateDirectory(thumbnailDirectory);
                }

                await Task.Run(() => File.WriteAllBytes(thumbnailFullPath, thumbnailData));

                _logger.Info("GenerateThumbnailAsync: Đã tạo thumbnail, Path={0}", thumbnailPath);
                return thumbnailPath;
            }
            catch (Exception ex)
            {
                _logger.Error($"GenerateThumbnailAsync: Lỗi tạo thumbnail {originalRelativePath}: {ex.Message}", ex);
                return null;
            }
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
            catch (Exception ex)
            {
                _logger.Error($"CalculateChecksumAsync: Lỗi tính checksum {relativePath}: {ex.Message}", ex);
                return null;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Validate hình ảnh trước khi lưu
        /// </summary>
        private void ValidateImage(byte[] imageData, string fileName)
        {
            if (imageData == null || imageData.Length == 0)
            {
                throw new ArgumentException(@"ImageData cannot be null or empty", nameof(imageData));
            }

            if (imageData.Length > _config.MaxFileSize)
            {
                throw new ArgumentException($@"File size ({imageData.Length}) exceeds maximum allowed size ({_config.MaxFileSize})", nameof(imageData));
            }

            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentException(@"FileName cannot be null or empty", nameof(fileName));
            }

            var extension = Path.GetExtension(fileName).TrimStart('.').ToLower();
            if (string.IsNullOrEmpty(extension) || !_config.AllowedExtensions.Contains(extension))
            {
                throw new ArgumentException($@"File extension '{extension}' is not allowed. Allowed extensions: {string.Join(", ", _config.AllowedExtensions)}", nameof(fileName));
            }
        }

        /// <summary>
        /// Generate relative path dựa trên category và entityId
        /// </summary>
        private string GenerateRelativePath(ImageCategory category, string fileName, Guid? entityId)
        {
            var year = DateTime.Now.Year;
            var month = DateTime.Now.Month.ToString("00");

            return category switch
            {
                ImageCategory.Product => entityId.HasValue
                    ? $"{_config.ProductsPath}/{entityId.Value}/{year}/{month}/{fileName}"
                    : $"{_config.ProductsPath}/{year}/{month}/{fileName}",
                ImageCategory.ProductVariant => entityId.HasValue
                    ? $"{_config.ProductsPath}/Variants/{entityId.Value}/{year}/{month}/{fileName}"
                    : $"{_config.ProductsPath}/Variants/{year}/{month}/{fileName}",
                ImageCategory.StockInOut => $"{_config.StockInOutPath}/{year}/{month}/{fileName}",
                ImageCategory.Company => entityId.HasValue
                    ? $"{_config.CompanyPath}/{entityId.Value}_{fileName}"
                    : $"{_config.CompanyPath}/{fileName}",
                ImageCategory.Avatar => entityId.HasValue
                    ? $"{_config.AvatarsPath}/{entityId.Value}_{fileName}"
                    : $"{_config.AvatarsPath}/{fileName}",
                ImageCategory.Temp => $"{_config.TempPath}/{year}/{month}/{fileName}",
                _ => $"{_config.TempPath}/{fileName}"
            };
        }

        /// <summary>
        /// Get thumbnail path từ original path
        /// </summary>
        private string GetThumbnailPath(string originalRelativePath)
        {
            var directory = Path.GetDirectoryName(originalRelativePath);
            var fileName = Path.GetFileNameWithoutExtension(originalRelativePath);

            // Thumbnail: {directory}/{fileName}_thumb.jpg
            return directory != null ? Path.Combine(directory, $"{fileName}_thumb.jpg") : null;
        }

        /// <summary>
        /// Resize image
        /// </summary>
        private Image ResizeImage(Image originalImage, int width, int height)
        {
            var newSize = CalculateNewSize(originalImage.Width, originalImage.Height, width, height);
            var resizedImage = new Bitmap(newSize.Width, newSize.Height);

            using var graphics = Graphics.FromImage(resizedImage);
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphics.CompositingQuality = CompositingQuality.HighQuality;

            graphics.DrawImage(originalImage, 0, 0, newSize.Width, newSize.Height);

            return resizedImage;
        }

        /// <summary>
        /// Calculate new size maintaining aspect ratio
        /// </summary>
        private Size CalculateNewSize(int originalWidth, int originalHeight, int maxWidth, int maxHeight)
        {
            if (originalWidth <= maxWidth && originalHeight <= maxHeight)
            {
                return new Size(originalWidth, originalHeight);
            }

            var ratio = Math.Min((double)maxWidth / originalWidth, (double)maxHeight / originalHeight);

            return new Size(
                (int)(originalWidth * ratio),
                (int)(originalHeight * ratio)
            );
        }

        /// <summary>
        /// Convert Image to byte array
        /// </summary>
        private byte[] ImageToByteArray(Image image, int quality = 80)
        {
            using var ms = new MemoryStream();
            var encoder = ImageCodecInfo.GetImageEncoders()
                .FirstOrDefault(c => c.FormatID == ImageFormat.Jpeg.Guid);
            
            var encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, quality);

            if (encoder != null)
            {
                image.Save(ms, encoder, encoderParams);
            }
            else
            {
                image.Save(ms, ImageFormat.Jpeg);
            }

            return ms.ToArray();
        }

        /// <summary>
        /// Calculate MD5 checksum
        /// </summary>
        private string CalculateChecksum(byte[] data)
        {
            using var md5 = MD5.Create();
            
            var hash = md5.ComputeHash(data);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }

        #endregion
    }
}

