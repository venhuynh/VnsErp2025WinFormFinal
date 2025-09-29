using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Bll.MasterData.ProductService;

namespace Bll.Common.ImageService
{
    /// <summary>
    /// Service xử lý validation và security cho hình ảnh
    /// </summary>
    public class ImageValidationService
    {
        #region Constants

        private static readonly Dictionary<string, byte[]> ImageSignatures = new Dictionary<string, byte[]>
        {
            { "jpg", new byte[] { 0xFF, 0xD8, 0xFF } },
            { "png", new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A } },
            { "gif", new byte[] { 0x47, 0x49, 0x46, 0x38 } },
            { "bmp", new byte[] { 0x42, 0x4D } },
            { "webp", new byte[] { 0x52, 0x49, 0x46, 0x46 } }
        };

        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
        private const long MaxFileSize = 10 * 1024 * 1024; // 10MB
        private const int MaxImageWidth = 4096;
        private const int MaxImageHeight = 4096;
        private const int MinImageWidth = 10;
        private const int MinImageHeight = 10;

        #endregion

        #region Public Methods

        /// <summary>
        /// Validate hình ảnh cho thumbnail upload (chấp nhận mọi kích thước, sẽ resize sau)
        /// </summary>
        /// <param name="imageData">Dữ liệu hình ảnh</param>
        /// <param name="fileName">Tên file (optional)</param>
        /// <returns>Validation result</returns>
        public ImageValidationResult ValidateImageForThumbnail(byte[] imageData, string fileName = null)
        {
            var result = new ImageValidationResult();

            try
            {
                // 1. Kiểm tra null/empty
                if (imageData == null || imageData.Length == 0)
                {
                    result.AddError("Dữ liệu hình ảnh không được null hoặc empty");
                    return result;
                }

                // 2. Kiểm tra file extension
                if (!string.IsNullOrEmpty(fileName))
                {
                    var extension = Path.GetExtension(fileName).ToLower();
                    if (!AllowedExtensions.Contains(extension))
                    {
                        result.AddError($"Định dạng file không được hỗ trợ: {extension}");
                        return result;
                    }
                }

                // 3. Kiểm tra file signature (magic bytes)
                var signatureCheck = ValidateFileSignature(imageData);
                if (!signatureCheck.IsValid)
                {
                    result.AddError($"File signature không hợp lệ: {signatureCheck.ErrorMessage}");
                    return result;
                }

                // 4. Kiểm tra bằng GDI+ (có thể throw exception nếu file bị corrupt)
                var imageInfo = ValidateWithGdiPlus(imageData);
                if (!imageInfo.IsValid)
                {
                    result.AddError($"Hình ảnh không hợp lệ: {imageInfo.ErrorMessage}");
                    return result;
                }

                // 5. Kiểm tra kích thước tối thiểu (không giới hạn kích thước tối đa)
                if (imageInfo.Width < MinImageWidth || imageInfo.Height < MinImageHeight)
                {
                    result.AddError($"Kích thước hình ảnh quá nhỏ. Tối thiểu: {MinImageWidth}x{MinImageHeight}");
                    return result;
                }

                // 6. Thêm thông tin kích thước vào warnings (không phải error)
                if (imageInfo.Width > MaxImageWidth || imageInfo.Height > MaxImageHeight)
                {
                    result.AddWarning($"Kích thước hình ảnh lớn ({imageInfo.Width}x{imageInfo.Height}). Sẽ được resize tự động.");
                }

                // 7. Thêm thông tin file size vào warnings
                if (imageData.Length > MaxFileSize)
                {
                    result.AddWarning($"Kích thước file lớn ({(imageData.Length / (1024.0 * 1024.0)):F1}MB). Sẽ được nén tự động.");
                }

                result.IsValid = true;
                result.ImageInfo = new ImageInfo
                {
                    Width = imageInfo.Width,
                    Height = imageInfo.Height,
                    Format = imageInfo.Format ?? signatureCheck.DetectedFormat
                };
            }
            catch (Exception ex)
            {
                result.AddError($"Lỗi validation: {ex.Message}");
            }

            return result;
        }

        /// <summary>
        /// Validate hình ảnh với tất cả security checks (strict validation)
        /// </summary>
        /// <param name="imageData">Dữ liệu hình ảnh</param>
        /// <param name="fileName">Tên file (optional)</param>
        /// <returns>Validation result</returns>
        public ImageValidationResult ValidateImage(byte[] imageData, string fileName = null)
        {
            var result = new ImageValidationResult();

            try
            {
                // 1. Kiểm tra null/empty
                if (imageData == null || imageData.Length == 0)
                {
                    result.AddError("Dữ liệu hình ảnh không được null hoặc empty");
                    return result;
                }

                // 2. Kiểm tra file size
                if (imageData.Length > MaxFileSize)
                {
                    result.AddError($"Kích thước file quá lớn. Tối đa: {MaxFileSize / (1024 * 1024)}MB");
                    return result;
                }

                // 3. Kiểm tra file extension
                if (!string.IsNullOrEmpty(fileName))
                {
                    var extension = Path.GetExtension(fileName).ToLower();
                    if (!AllowedExtensions.Contains(extension))
                    {
                        result.AddError($"Định dạng file không được hỗ trợ: {extension}");
                        return result;
                    }
                }

                // 4. Kiểm tra file signature (magic bytes)
                var signatureCheck = ValidateFileSignature(imageData);
                if (!signatureCheck.IsValid)
                {
                    result.AddError($"File signature không hợp lệ: {signatureCheck.ErrorMessage}");
                    return result;
                }

                // 5. Kiểm tra bằng GDI+ (có thể throw exception nếu file bị corrupt)
                var imageInfo = ValidateWithGdiPlus(imageData);
                if (!imageInfo.IsValid)
                {
                    result.AddError($"Hình ảnh không hợp lệ: {imageInfo.ErrorMessage}");
                    return result;
                }

                // 6. Kiểm tra kích thước hình ảnh
                if (imageInfo.Width < MinImageWidth || imageInfo.Height < MinImageHeight)
                {
                    result.AddError($"Kích thước hình ảnh quá nhỏ. Tối thiểu: {MinImageWidth}x{MinImageHeight}");
                    return result;
                }

                if (imageInfo.Width > MaxImageWidth || imageInfo.Height > MaxImageHeight)
                {
                    result.AddError($"Kích thước hình ảnh quá lớn. Tối đa: {MaxImageWidth}x{MaxImageHeight}");
                    return result;
                }

                // 7. Kiểm tra aspect ratio
                var aspectRatio = (double)imageInfo.Width / imageInfo.Height;
                if (aspectRatio > 10 || aspectRatio < 0.1)
                {
                    result.AddError("Tỷ lệ khung hình không hợp lệ");
                    return result;
                }

                // 8. Kiểm tra metadata (EXIF)
                var metadataCheck = ValidateMetadata(imageData);
                if (!metadataCheck.IsValid)
                {
                    result.AddWarning($"Metadata có vấn đề: {metadataCheck.ErrorMessage}");
                }

                result.IsValid = true;
                //result.ImageInfo = imageInfo;
                result.DetectedFormat = signatureCheck.DetectedFormat;

                return result;
            }
            catch (Exception ex)
            {
                result.AddError($"Lỗi không mong muốn khi validate hình ảnh: {ex.Message}");
                return result;
            }
        }

        /// <summary>
        /// Sanitize hình ảnh (xóa metadata, resize nếu cần)
        /// </summary>
        /// <param name="imageData">Dữ liệu hình ảnh gốc</param>
        /// <param name="maxWidth">Chiều rộng tối đa</param>
        /// <param name="maxHeight">Chiều cao tối đa</param>
        /// <returns>Dữ liệu hình ảnh đã được sanitize</returns>
        public byte[] SanitizeImage(byte[] imageData, int maxWidth = 2048, int maxHeight = 2048)
        {
            try
            {
                using (var originalImage = Image.FromStream(new MemoryStream(imageData)))
                {
                    // Tính toán kích thước mới
                    var newSize = CalculateNewSize(originalImage.Width, originalImage.Height, maxWidth, maxHeight);
                    
                    // Tạo hình ảnh mới (sẽ tự động loại bỏ metadata)
                    using (var sanitizedImage = new Bitmap(newSize.Width, newSize.Height))
                    {
                        using (var graphics = Graphics.FromImage(sanitizedImage))
                        {
                            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                            graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                            graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                            
                            graphics.DrawImage(originalImage, 0, 0, newSize.Width, newSize.Height);
                        }

                        // Lưu với JPEG format (loại bỏ metadata)
                        using (var ms = new MemoryStream())
                        {
                            var jpegCodec = GetEncoder(ImageFormat.Jpeg);
                            if (jpegCodec != null)
                            {
                                var encoderParams = new EncoderParameters(1);
                                encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, 90L);
                                sanitizedImage.Save(ms, jpegCodec, encoderParams);
                            }
                            else
                            {
                                sanitizedImage.Save(ms, ImageFormat.Jpeg);
                            }
                            
                            return ms.ToArray();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi sanitize hình ảnh: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Tính hash của hình ảnh để detect duplicate
        /// </summary>
        /// <param name="imageData">Dữ liệu hình ảnh</param>
        /// <returns>Hash string</returns>
        public string CalculateImageHash(byte[] imageData)
        {
            try
            {
                using (var sha256 = SHA256.Create())
                {
                    var hashBytes = sha256.ComputeHash(imageData);
                    return Convert.ToBase64String(hashBytes);
                }
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi tính hash hình ảnh: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Kiểm tra xem hình ảnh có chứa nội dung không phù hợp không
        /// </summary>
        /// <param name="imageData">Dữ liệu hình ảnh</param>
        /// <returns>True nếu có vấn đề</returns>
        public bool ContainsInappropriateContent(byte[] imageData)
        {
            try
            {
                // TODO: Implement AI-based content detection
                // Hiện tại chỉ là placeholder
                
                // Kiểm tra một số pattern đơn giản
                var suspiciousPatterns = new byte[][]
                {
                    new byte[] { 0x4D, 0x5A }, // PE header
                    new byte[] { 0x50, 0x4B }, // ZIP header
                    new byte[] { 0x25, 0x50, 0x44, 0x46 } // PDF header
                };

                foreach (var pattern in suspiciousPatterns)
                {
                    if (ContainsPattern(imageData, pattern))
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi kiểm tra nội dung: {ex.Message}", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Validate file signature
        /// </summary>
        private (bool IsValid, string ErrorMessage, string DetectedFormat) ValidateFileSignature(byte[] imageData)
        {
            try
            {
                foreach (var signature in ImageSignatures)
                {
                    if (imageData.Length >= signature.Value.Length)
                    {
                        bool matches = true;
                        for (int i = 0; i < signature.Value.Length; i++)
                        {
                            if (imageData[i] != signature.Value[i])
                            {
                                matches = false;
                                break;
                            }
                        }

                        if (matches)
                        {
                            return (true, null, signature.Key);
                        }
                    }
                }

                return (false, "File signature không khớp với bất kỳ định dạng hình ảnh nào được hỗ trợ", null);
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi khi kiểm tra file signature: {ex.Message}", null);
            }
        }

        /// <summary>
        /// Validate với GDI+
        /// </summary>
        private (bool IsValid, string ErrorMessage, int Width, int Height, string Format) ValidateWithGdiPlus(byte[] imageData)
        {
            try
            {
                using (var image = Image.FromStream(new MemoryStream(imageData)))
                {
                    string format = "unknown";
                    if (image.RawFormat.Equals(ImageFormat.Jpeg))
                        format = "jpg";
                    else if (image.RawFormat.Equals(ImageFormat.Png))
                        format = "png";
                    else if (image.RawFormat.Equals(ImageFormat.Gif))
                        format = "gif";
                    else if (image.RawFormat.Equals(ImageFormat.Bmp))
                        format = "bmp";
                    
                    return (true, null, image.Width, image.Height, format);
                }
            }
            catch (Exception ex)
            {
                return (false, ex.Message, 0, 0, null);
            }
        }

        /// <summary>
        /// Validate metadata
        /// </summary>
        private (bool IsValid, string ErrorMessage) ValidateMetadata(byte[] imageData)
        {
            try
            {
                using (var image = Image.FromStream(new MemoryStream(imageData)))
                {
                    // Kiểm tra một số metadata có thể gây vấn đề
                    var propertyItems = image.PropertyItems;
                    
                    // TODO: Implement specific metadata validation
                    // Ví dụ: kiểm tra GPS data, camera info, etc.
                    
                    return (true, null);
                }
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        /// <summary>
        /// Tính toán kích thước mới
        /// </summary>
        private Size CalculateNewSize(int originalWidth, int originalHeight, int maxWidth, int maxHeight)
        {
            if (originalWidth <= maxWidth && originalHeight <= maxHeight)
                return new Size(originalWidth, originalHeight);

            double ratio = Math.Min((double)maxWidth / originalWidth, (double)maxHeight / originalHeight);
            
            return new Size(
                (int)(originalWidth * ratio),
                (int)(originalHeight * ratio)
            );
        }

        /// <summary>
        /// Lấy ImageCodecInfo
        /// </summary>
        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            try
            {
                var codecs = ImageCodecInfo.GetImageEncoders();
                return codecs.FirstOrDefault(codec => codec.FormatID == format.Guid);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Kiểm tra pattern trong byte array
        /// </summary>
        private bool ContainsPattern(byte[] data, byte[] pattern)
        {
            for (int i = 0; i <= data.Length - pattern.Length; i++)
            {
                bool matches = true;
                for (int j = 0; j < pattern.Length; j++)
                {
                    if (data[i + j] != pattern[j])
                    {
                        matches = false;
                        break;
                    }
                }
                if (matches) return true;
            }
            return false;
        }

        #endregion
    }

    /// <summary>
    /// Kết quả validation hình ảnh
    /// </summary>
    public class ImageValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public List<string> Warnings { get; set; } = new List<string>();
        public ImageInfo ImageInfo { get; set; }
        public string DetectedFormat { get; set; }

        // Convenience properties for easy access
        public int Width => ImageInfo?.Width ?? 0;
        public int Height => ImageInfo?.Height ?? 0;
        public string Format => ImageInfo?.Format ?? DetectedFormat;

        public void AddError(string error)
        {
            Errors.Add(error);
        }

        public void AddWarning(string warning)
        {
            Warnings.Add(warning);
        }
    }

    /// <summary>
    /// Thông tin hình ảnh
    /// </summary>
    public class ImageInfo
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public long FileSize { get; set; }
        public string Format { get; set; }
        public double AspectRatio => Height > 0 ? (double)Width / Height : 0;
    }
}
