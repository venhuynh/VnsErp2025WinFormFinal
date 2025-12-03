using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Logger.Interfaces;

namespace Bll.Common.ImageStorage
{
    /// <summary>
    /// Implementation cho Local File System Storage
    /// Fallback option khi không có NAS
    /// Hỗ trợ lưu trữ nhiều loại file: images, PDF, DOCX, XLSX, etc.
    /// </summary>
    public class LocalImageStorageService : IImageStorageService, IFileStorageService
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

        /// <summary>
        /// Generate relative path dựa trên FileCategory và entityId
        /// </summary>
        private string GenerateRelativePath(FileCategory category, string fileName, Guid? entityId)
        {
            var year = DateTime.Now.Year;
            var month = DateTime.Now.Month.ToString("00");

            return category switch
            {
                // Image categories (backward compatibility)
                FileCategory.Product => entityId.HasValue
                    ? $"{_config.ProductsPath}/{entityId.Value}/{year}/{month}/{fileName}"
                    : $"{_config.ProductsPath}/{year}/{month}/{fileName}",
                FileCategory.ProductVariant => entityId.HasValue
                    ? $"{_config.ProductsPath}/Variants/{entityId.Value}/{year}/{month}/{fileName}"
                    : $"{_config.ProductsPath}/Variants/{year}/{month}/{fileName}",
                FileCategory.StockInOut => $"{_config.StockInOutPath}/{year}/{month}/{fileName}",
                FileCategory.Company => entityId.HasValue
                    ? $"{_config.CompanyPath}/{entityId.Value}_{fileName}"
                    : $"{_config.CompanyPath}/{fileName}",
                FileCategory.Avatar => entityId.HasValue
                    ? $"{_config.AvatarsPath}/{entityId.Value}_{fileName}"
                    : $"{_config.AvatarsPath}/{fileName}",
                FileCategory.Temp => $"{_config.TempPath}/{year}/{month}/{fileName}",
                
                // Document categories (mới)
                FileCategory.StockInOutDocument => $"Documents/StockInOut/{year}/{month}/{fileName}",
                FileCategory.BusinessPartnerDocument => entityId.HasValue
                    ? $"Documents/BusinessPartner/{entityId.Value}/{year}/{month}/{fileName}"
                    : $"Documents/BusinessPartner/{year}/{month}/{fileName}",
                FileCategory.Document => $"Documents/General/{year}/{month}/{fileName}",
                FileCategory.Report => $"Documents/Reports/{year}/{month}/{fileName}",
                
                _ => $"{_config.TempPath}/{fileName}"
            };
        }

        /// <summary>
        /// Generate relative path dựa trên ImageCategory và entityId (backward compatibility)
        /// </summary>
        private string GenerateRelativePath(ImageCategory category, string fileName, Guid? entityId)
        {
            // Convert ImageCategory sang FileCategory
            var fileCategory = category switch
            {
                ImageCategory.Product => FileCategory.Product,
                ImageCategory.ProductVariant => FileCategory.ProductVariant,
                ImageCategory.StockInOut => FileCategory.StockInOut,
                ImageCategory.Company => FileCategory.Company,
                ImageCategory.Avatar => FileCategory.Avatar,
                ImageCategory.Temp => FileCategory.Temp,
                _ => FileCategory.Temp
            };

            return GenerateRelativePath(fileCategory, fileName, entityId);
        }

        private string CalculateChecksum(byte[] data)
        {
            using var md5 = System.Security.Cryptography.MD5.Create();
            var hash = md5.ComputeHash(data);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }

        #endregion

        #region IFileStorageService Implementation

        /// <summary>
        /// Lưu file vào storage (hỗ trợ mọi loại file: images, PDF, DOCX, XLSX, etc.)
        /// </summary>
        public async Task<FileStorageResult> SaveFileAsync(
            byte[] fileData,
            string fileName,
            FileCategory category,
            Guid? entityId = null,
            bool generateThumbnail = false)
        {
            try
            {
                _logger.Debug("LocalImageStorageService.SaveFileAsync: Bắt đầu lưu file, FileName={0}", fileName);

                // Validate file
                ValidateFile(fileData, fileName);

                // Generate path
                var relativePath = GenerateRelativePath(category, fileName, entityId);
                var fullPath = Path.Combine(_localBasePath, relativePath);

                // Ensure directory exists
                var directory = Path.GetDirectoryName(fullPath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Save file
                await Task.Run(() => File.WriteAllBytes(fullPath, fileData));

                // Calculate checksum
                var checksum = CalculateChecksum(fileData);

                // Get MIME type
                var fileExtension = Path.GetExtension(fileName).TrimStart('.').ToLower();
                var mimeType = GetMimeType(fileExtension);

                _logger.Info("LocalImageStorageService.SaveFileAsync: Đã lưu file, Path={0}", relativePath);

                return new FileStorageResult
                {
                    Success = true,
                    RelativePath = relativePath,
                    FullPath = fullPath,
                    FileName = fileName,
                    FileSize = fileData.Length,
                    Checksum = checksum,
                    MimeType = mimeType,
                    FileExtension = fileExtension
                };
            }
            catch (Exception ex)
            {
                _logger.Error($"LocalImageStorageService.SaveFileAsync: Lỗi {ex.Message}", ex);
                return new FileStorageResult
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    Exception = ex
                };
            }
        }

        /// <summary>
        /// Lấy file từ storage
        /// </summary>
        public async Task<byte[]> GetFileAsync(string relativePath)
        {
            // Sử dụng chung logic với GetImageAsync
            return await GetImageAsync(relativePath);
        }

        /// <summary>
        /// Xóa file từ storage
        /// </summary>
        public async Task<bool> DeleteFileAsync(string relativePath)
        {
            // Sử dụng chung logic với DeleteImageAsync
            return await DeleteImageAsync(relativePath);
        }

        /// <summary>
        /// Kiểm tra file tồn tại
        /// </summary>
        public async Task<bool> FileExistsAsync(string relativePath)
        {
            // Sử dụng chung logic với ImageExistsAsync
            return await ImageExistsAsync(relativePath);
        }

        /// <summary>
        /// Verify file integrity bằng checksum
        /// </summary>
        public async Task<bool> VerifyFileAsync(string relativePath, string checksum)
        {
            // Sử dụng chung logic với VerifyImageAsync
            return await VerifyImageAsync(relativePath, checksum);
        }

        #endregion

        #region Helper Methods - File Storage

        /// <summary>
        /// Validate file trước khi lưu (mở rộng từ ValidateImage để hỗ trợ nhiều loại file)
        /// </summary>
        private void ValidateFile(byte[] fileData, string fileName)
        {
            if (fileData == null || fileData.Length == 0)
            {
                throw new ArgumentException("FileData cannot be null or empty", nameof(fileData));
            }

            if (fileData.Length > _config.MaxFileSize)
            {
                throw new ArgumentException($"File size exceeds maximum allowed size", nameof(fileData));
            }

            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentException("FileName cannot be null or empty", nameof(fileName));
            }

            var extension = Path.GetExtension(fileName).TrimStart('.').ToLower();
            
            // Mở rộng danh sách allowed extensions để hỗ trợ nhiều loại file
            var allowedExtensions = new List<string>(_config.AllowedExtensions);
            
            // Thêm các extension cho documents
            var documentExtensions = new[] { "pdf", "doc", "docx", "xls", "xlsx", "txt", "zip", "rar" };
            foreach (var ext in documentExtensions)
            {
                if (!allowedExtensions.Contains(ext))
                {
                    allowedExtensions.Add(ext);
                }
            }

            if (string.IsNullOrEmpty(extension) || !allowedExtensions.Contains(extension))
            {
                throw new ArgumentException($"File extension '{extension}' is not allowed. Allowed extensions: {string.Join(", ", allowedExtensions)}", nameof(fileName));
            }
        }

        /// <summary>
        /// Lấy MIME type từ file extension
        /// </summary>
        private string GetMimeType(string fileExtension)
        {
            if (string.IsNullOrEmpty(fileExtension))
                return "application/octet-stream";

            var ext = fileExtension.TrimStart('.').ToLower();
            return ext switch
            {
                "pdf" => "application/pdf",
                "doc" => "application/msword",
                "docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                "xls" => "application/vnd.ms-excel",
                "xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "jpg" or "jpeg" => "image/jpeg",
                "png" => "image/png",
                "gif" => "image/gif",
                "bmp" => "image/bmp",
                "txt" => "text/plain",
                "zip" => "application/zip",
                "rar" => "application/x-rar-compressed",
                _ => "application/octet-stream"
            };
        }

        #endregion
    }
}

