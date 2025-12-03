using Logger.Interfaces;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows.Forms;
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
                // Normalize path để đảm bảo tất cả đều dùng backslash cho Windows
                var normalizedRelativePath = NormalizePath(relativePath);
                var fullPath = Path.Combine(_config.NASBasePath, normalizedRelativePath);
                
                // Log để debug
                _logger.Info("SaveImageAsync: Tạo file path");
                _logger.Info("  - NASBasePath: {0}", _config.NASBasePath);
                _logger.Info("  - RelativePath (original): {0}", relativePath);
                _logger.Info("  - RelativePath (normalized): {0}", normalizedRelativePath);
                _logger.Info("  - FullPath: {0}", fullPath);

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
                _logger.Info("SaveImageAsync: Bắt đầu lưu file vào đường dẫn: {0}", fullPath);
                await Task.Run(() => File.WriteAllBytes(fullPath, imageData));
                _logger.Info("SaveImageAsync: Đã lưu file thành công vào đường dẫn: {0}", fullPath);

                // 5. Calculate checksum
                var checksum = CalculateChecksum(imageData);
                _logger.Debug("SaveImageAsync: Checksum: {0}", checksum);

                // 6. Generate thumbnail if requested
                string thumbnailPath = null;
                string thumbnailFullPath = null;
                if (generateThumbnail && _config.EnableThumbnailGeneration)
                {
                    _logger.Info("SaveImageAsync: Bắt đầu tạo thumbnail cho: {0}", normalizedRelativePath);
                    thumbnailPath = await GenerateThumbnailAsync(normalizedRelativePath, _config.ThumbnailWidth, _config.ThumbnailHeight);
                    if (!string.IsNullOrEmpty(thumbnailPath))
                    {
                        thumbnailFullPath = Path.Combine(_config.NASBasePath, NormalizePath(thumbnailPath));
                        _logger.Info("SaveImageAsync: Đã tạo thumbnail tại đường dẫn: {0}", thumbnailFullPath);
                    }
                }

                // Log tổng kết với đầy đủ thông tin đường dẫn
                _logger.Info("═══════════════════════════════════════════════════════════");
                _logger.Info("SaveImageAsync: ĐÃ LƯU HÌNH ẢNH THÀNH CÔNG");
                _logger.Info("  - FileName: {0}", fileName);
                _logger.Info("  - Category: {0}", category);
                _logger.Info("  - EntityId: {0}", entityId?.ToString() ?? "N/A");
                _logger.Info("  - RelativePath: {0}", normalizedRelativePath);
                _logger.Info("  - FullPath: {0}", fullPath);
                _logger.Info("  - FileSize: {0} bytes ({1:F2} KB)", imageData.Length, imageData.Length / 1024.0);
                _logger.Info("  - Checksum: {0}", checksum);
                if (!string.IsNullOrEmpty(thumbnailFullPath))
                {
                    _logger.Info("  - ThumbnailPath: {0}", thumbnailFullPath);
                }
                _logger.Info("═══════════════════════════════════════════════════════════");

                return new ImageStorageResult
                {
                    Success = true,
                    RelativePath = normalizedRelativePath, // Lưu normalized path để đảm bảo consistency
                    FullPath = fullPath,
                    FileName = fileName,
                    FileSize = imageData.Length,
                    Checksum = checksum,
                    ThumbnailRelativePath = thumbnailPath != null ? NormalizePath(thumbnailPath) : null,
                    ThumbnailFullPath = thumbnailFullPath
                };
            }
            catch (UnauthorizedAccessException ex)
            {
                var errorMessage = $"SaveImageAsync: Lỗi quyền truy cập NAS\n" +
                                  $"  - FileName: {fileName}\n" +
                                  $"  - Category: {category}\n" +
                                  $"  - NASBasePath: {_config.NASBasePath}\n" +
                                  $"  - Error: {ex.Message}";
                _logger.Error(errorMessage, ex);
                return new ImageStorageResult
                {
                    Success = false,
                    ErrorMessage = $"Không có quyền truy cập NAS: {ex.Message}",
                    Exception = ex
                };
            }
            catch (IOException ex)
            {
                var errorMessage = $"SaveImageAsync: Lỗi I/O khi lưu file\n" +
                                  $"  - FileName: {fileName}\n" +
                                  $"  - Category: {category}\n" +
                                  $"  - NASBasePath: {_config.NASBasePath}\n" +
                                  $"  - Error: {ex.Message}";
                _logger.Error(errorMessage, ex);
                return new ImageStorageResult
                {
                    Success = false,
                    ErrorMessage = $"Lỗi I/O: {ex.Message}",
                    Exception = ex
                };
            }
            catch (Exception ex)
            {
                var errorMessage = $"SaveImageAsync: Lỗi không xác định\n" +
                                  $"  - FileName: {fileName}\n" +
                                  $"  - Category: {category}\n" +
                                  $"  - NASBasePath: {_config.NASBasePath}\n" +
                                  $"  - Error: {ex.Message}";
                _logger.Error(errorMessage, ex);
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
            // Khai báo biến ở ngoài try block để có thể sử dụng trong catch blocks
            string normalizedRelativePath = null;
            string fullPath = null;
            
            try
            {
                if (string.IsNullOrEmpty(relativePath))
                {
                    throw new ArgumentException(@"RelativePath cannot be null or empty", nameof(relativePath));
                }

                // Normalize relativePath: thay thế forward slash thành backslash cho Windows path
                normalizedRelativePath = NormalizePath(relativePath);
                
                // Combine path
                fullPath = Path.Combine(_config.NASBasePath, normalizedRelativePath);
                
                // Normalize fullPath để đảm bảo tất cả đều dùng backslash
                fullPath = Path.GetFullPath(fullPath);

                // Log chi tiết để debug
                _logger.Info("GetImageAsync: Bắt đầu đọc file");
                _logger.Info("  - NASBasePath: {0}", _config.NASBasePath);
                _logger.Info("  - RelativePath (original): {0}", relativePath);
                _logger.Info("  - RelativePath (normalized): {0}", normalizedRelativePath);
                _logger.Info("  - FullPath: {0}", fullPath);
                _logger.Info("  - File.Exists: {0}", File.Exists(fullPath));

                if (!File.Exists(fullPath))
                {
                    var errorMessage = $"GetImageAsync: File không tồn tại\n" +
                                      $"  - NASBasePath: {_config.NASBasePath}\n" +
                                      $"  - RelativePath: {relativePath}\n" +
                                      $"  - Normalized RelativePath: {normalizedRelativePath}\n" +
                                      $"  - FullPath: {fullPath}";
                    
                    _logger.Warning(errorMessage);
                    
                    // Hiển thị MessageBox để user biết lỗi
                    MessageBox.Show(
                        errorMessage,
                        "Lỗi đọc file từ NAS",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    
                    return null;
                }

                _logger.Info("GetImageAsync: Bắt đầu đọc file từ đường dẫn: {0}", fullPath);
                var imageData = await Task.Run(() => File.ReadAllBytes(fullPath));
                
                // Log tổng kết với đầy đủ thông tin đường dẫn
                _logger.Info("═══════════════════════════════════════════════════════════");
                _logger.Info("GetImageAsync: ĐÃ ĐỌC HÌNH ẢNH THÀNH CÔNG");
                _logger.Info("  - RelativePath (original): {0}", relativePath);
                _logger.Info("  - RelativePath (normalized): {0}", normalizedRelativePath);
                _logger.Info("  - FullPath: {0}", fullPath);
                _logger.Info("  - FileSize: {0} bytes ({1:F2} KB)", imageData.Length, imageData.Length / 1024.0);
                _logger.Info("═══════════════════════════════════════════════════════════");

                return imageData;
            }
            catch (UnauthorizedAccessException ex)
            {
                // Tính toán lại nếu chưa có (trường hợp exception xảy ra trước khi tính toán)
                if (string.IsNullOrEmpty(normalizedRelativePath))
                {
                    normalizedRelativePath = NormalizePath(relativePath);
                }
                if (string.IsNullOrEmpty(fullPath))
                {
                    fullPath = Path.Combine(_config.NASBasePath, normalizedRelativePath);
                    fullPath = Path.GetFullPath(fullPath);
                }
                
                var errorMessage = $"GetImageAsync: Lỗi quyền truy cập file\n" +
                                  $"  - NASBasePath: {_config.NASBasePath}\n" +
                                  $"  - RelativePath (original): {relativePath}\n" +
                                  $"  - RelativePath (normalized): {normalizedRelativePath}\n" +
                                  $"  - FullPath: {fullPath}\n" +
                                  $"  - Error: {ex.Message}";
                
                _logger.Error("═══════════════════════════════════════════════════════════");
                _logger.Error("GetImageAsync: LỖI QUYỀN TRUY CẬP");
                _logger.Error(errorMessage);
                _logger.Error("═══════════════════════════════════════════════════════════", ex);
                
                // Hiển thị MessageBox để user biết lỗi
                MessageBox.Show(
                    errorMessage,
                    "Lỗi quyền truy cập NAS",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                
                throw;
            }
            catch (IOException ex)
            {
                // Tính toán lại nếu chưa có (trường hợp exception xảy ra trước khi tính toán)
                if (string.IsNullOrEmpty(normalizedRelativePath))
                {
                    normalizedRelativePath = NormalizePath(relativePath);
                }
                if (string.IsNullOrEmpty(fullPath))
                {
                    fullPath = Path.Combine(_config.NASBasePath, normalizedRelativePath);
                    fullPath = Path.GetFullPath(fullPath);
                }
                
                var errorMessage = $"GetImageAsync: Lỗi I/O khi đọc file\n" +
                                  $"  - NASBasePath: {_config.NASBasePath}\n" +
                                  $"  - RelativePath (original): {relativePath}\n" +
                                  $"  - RelativePath (normalized): {normalizedRelativePath}\n" +
                                  $"  - FullPath: {fullPath}\n" +
                                  $"  - Error: {ex.Message}";
                
                _logger.Error("═══════════════════════════════════════════════════════════");
                _logger.Error("GetImageAsync: LỖI I/O");
                _logger.Error(errorMessage);
                _logger.Error("═══════════════════════════════════════════════════════════", ex);
                
                // Hiển thị MessageBox để user biết lỗi
                MessageBox.Show(
                    errorMessage,
                    "Lỗi I/O",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                
                throw;
            }
            catch (Exception ex)
            {
                // Tính toán lại nếu chưa có (trường hợp exception xảy ra trước khi tính toán)
                if (string.IsNullOrEmpty(normalizedRelativePath))
                {
                    normalizedRelativePath = NormalizePath(relativePath);
                }
                if (string.IsNullOrEmpty(fullPath))
                {
                    fullPath = Path.Combine(_config.NASBasePath, normalizedRelativePath);
                    fullPath = Path.GetFullPath(fullPath);
                }
                
                var errorMessage = $"GetImageAsync: Lỗi đọc file\n" +
                                  $"  - NASBasePath: {_config.NASBasePath}\n" +
                                  $"  - RelativePath (original): {relativePath}\n" +
                                  $"  - RelativePath (normalized): {normalizedRelativePath}\n" +
                                  $"  - FullPath: {fullPath}\n" +
                                  $"  - Error: {ex.Message}";
                
                _logger.Error("═══════════════════════════════════════════════════════════");
                _logger.Error("GetImageAsync: LỖI KHÔNG XÁC ĐỊNH");
                _logger.Error(errorMessage);
                _logger.Error("═══════════════════════════════════════════════════════════", ex);
                
                // Hiển thị MessageBox để user biết lỗi
                MessageBox.Show(
                    errorMessage + $"\n\nChi tiết:\n{ex}",
                    "Lỗi đọc file từ NAS",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                
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

                // Normalize path
                var normalizedRelativePath = NormalizePath(relativePath);
                
                // Thumbnail path: {originalPath}_thumb.jpg
                var thumbnailPath = GetThumbnailPath(normalizedRelativePath);
                var normalizedThumbnailPath = NormalizePath(thumbnailPath);
                var thumbnailFullPath = Path.Combine(_config.NASBasePath, normalizedThumbnailPath);
                
                // Log để debug
                _logger.Debug("GetThumbnailAsync: Bắt đầu đọc thumbnail");
                _logger.Debug("  - RelativePath (original): {0}", relativePath);
                _logger.Debug("  - RelativePath (normalized): {0}", normalizedRelativePath);
                _logger.Debug("  - ThumbnailPath: {0}", normalizedThumbnailPath);
                _logger.Debug("  - ThumbnailFullPath: {0}", thumbnailFullPath);

                if (!File.Exists(thumbnailFullPath))
                {
                    _logger.Debug("GetThumbnailAsync: Thumbnail không tồn tại, tạo mới, Path={0}", normalizedRelativePath);
                    // Generate thumbnail on the fly
                    await GenerateThumbnailAsync(normalizedRelativePath, _config.ThumbnailWidth, _config.ThumbnailHeight);
                    // Re-get thumbnail path sau khi generate
                    thumbnailPath = GetThumbnailPath(normalizedRelativePath);
                    normalizedThumbnailPath = NormalizePath(thumbnailPath);
                    thumbnailFullPath = Path.Combine(_config.NASBasePath, normalizedThumbnailPath);
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

                // Normalize path
                var normalizedRelativePath = NormalizePath(relativePath);
                var fullPath = Path.Combine(_config.NASBasePath, normalizedRelativePath);
                
                // Log để debug
                _logger.Debug("DeleteImageAsync: Bắt đầu xóa file");
                _logger.Debug("  - RelativePath (original): {0}", relativePath);
                _logger.Debug("  - RelativePath (normalized): {0}", normalizedRelativePath);
                _logger.Debug("  - FullPath: {0}", fullPath);

                if (!File.Exists(fullPath))
                {
                    _logger.Warning("DeleteImageAsync: File không tồn tại, Path={0}", fullPath);
                    return false;
                }

                await Task.Run(() => File.Delete(fullPath));

                // Also delete thumbnail if exists
                var thumbnailPath = GetThumbnailPath(normalizedRelativePath);
                var normalizedThumbnailPath = NormalizePath(thumbnailPath);
                var thumbnailFullPath = Path.Combine(_config.NASBasePath, normalizedThumbnailPath);
                if (File.Exists(thumbnailFullPath))
                {
                    await Task.Run(() => File.Delete(thumbnailFullPath));
                }

                _logger.Info("DeleteImageAsync: Đã xóa file, Path={0}", normalizedRelativePath);
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

                // Normalize path
                var normalizedRelativePath = NormalizePath(relativePath);
                var fullPath = Path.Combine(_config.NASBasePath, normalizedRelativePath);
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

                // Normalize path
                var normalizedOriginalRelativePath = NormalizePath(originalRelativePath);
                var originalFullPath = Path.Combine(_config.NASBasePath, normalizedOriginalRelativePath);
                
                // Log để debug
                _logger.Debug("GenerateThumbnailAsync: Bắt đầu tạo thumbnail");
                _logger.Debug("  - OriginalRelativePath (original): {0}", originalRelativePath);
                _logger.Debug("  - OriginalRelativePath (normalized): {0}", normalizedOriginalRelativePath);
                _logger.Debug("  - OriginalFullPath: {0}", originalFullPath);
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
                var thumbnailPath = GetThumbnailPath(normalizedOriginalRelativePath);
                var normalizedThumbnailPath = NormalizePath(thumbnailPath);
                var thumbnailFullPath = Path.Combine(_config.NASBasePath, normalizedThumbnailPath);

                var thumbnailDirectory = Path.GetDirectoryName(thumbnailFullPath);
                if (!Directory.Exists(thumbnailDirectory))
                {
                    if (thumbnailDirectory != null) 
                        Directory.CreateDirectory(thumbnailDirectory);
                }

                await Task.Run(() => File.WriteAllBytes(thumbnailFullPath, thumbnailData));

                _logger.Info("GenerateThumbnailAsync: Đã tạo thumbnail, Path={0}", normalizedThumbnailPath);
                return normalizedThumbnailPath; // Trả về normalized path để đảm bảo consistency
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
        /// Normalize path: thay thế forward slash thành backslash cho Windows path
        /// Đảm bảo path nhất quán khi combine với NASBasePath
        /// </summary>
        private string NormalizePath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return path;

            // Thay thế forward slash thành backslash cho Windows path
            return path.Replace('/', '\\');
        }

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
        /// Sử dụng Path.Combine để đảm bảo path separator đúng cho Windows (backslash)
        /// </summary>
        private string GenerateRelativePath(ImageCategory category, string fileName, Guid? entityId)
        {
            var year = DateTime.Now.Year;
            var month = DateTime.Now.Month.ToString("00");

            // Sử dụng Path.Combine thay vì string interpolation với forward slash
            // để đảm bảo path separator đúng cho Windows
            return category switch
            {
                ImageCategory.Product => entityId.HasValue
                    ? Path.Combine(_config.ProductsPath, entityId.Value.ToString(), year.ToString(), month, fileName)
                    : Path.Combine(_config.ProductsPath, year.ToString(), month, fileName),
                ImageCategory.ProductVariant => entityId.HasValue
                    ? Path.Combine(_config.ProductsPath, "Variants", entityId.Value.ToString(), year.ToString(), month, fileName)
                    : Path.Combine(_config.ProductsPath, "Variants", year.ToString(), month, fileName),
                ImageCategory.StockInOut => Path.Combine(_config.StockInOutPath, year.ToString(), month, fileName),
                ImageCategory.Company => entityId.HasValue
                    ? Path.Combine(_config.CompanyPath, $"{entityId.Value}_{fileName}")
                    : Path.Combine(_config.CompanyPath, fileName),
                ImageCategory.Avatar => entityId.HasValue
                    ? Path.Combine(_config.AvatarsPath, $"{entityId.Value}_{fileName}")
                    : Path.Combine(_config.AvatarsPath, fileName),
                ImageCategory.Temp => Path.Combine(_config.TempPath, year.ToString(), month, fileName),
                _ => Path.Combine(_config.TempPath, fileName)
            };
        }

        /// <summary>
        /// Get thumbnail path từ original path
        /// Sử dụng Path.Combine để đảm bảo path separator đúng cho Windows (backslash)
        /// </summary>
        private string GetThumbnailPath(string originalRelativePath)
        {
            // Normalize path trước khi xử lý
            var normalizedPath = NormalizePath(originalRelativePath);
            var directory = Path.GetDirectoryName(normalizedPath);
            var fileName = Path.GetFileNameWithoutExtension(normalizedPath);

            // Thumbnail: {directory}\{fileName}_thumb.jpg (sử dụng Path.Combine để đảm bảo backslash)
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

