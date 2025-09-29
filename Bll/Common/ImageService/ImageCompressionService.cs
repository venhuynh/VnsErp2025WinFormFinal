using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Bll.MasterData.ProductService;

namespace Bll.Common.ImageService
{
    /// <summary>
    /// Service xử lý nén hình ảnh với multiple formats và strategies
    /// </summary>
    public class ImageCompressionService
    {
        #region Public Methods

        /// <summary>
        /// Nén hình ảnh với strategy tối ưu
        /// </summary>
        /// <param name="imageData">Dữ liệu hình ảnh gốc</param>
        /// <param name="targetSize">Kích thước mục tiêu (bytes)</param>
        /// <param name="maxDimension">Kích thước tối đa (pixels)</param>
        /// <param name="format">Định dạng output</param>
        /// <returns>Dữ liệu hình ảnh đã nén</returns>
        public byte[] CompressImage(byte[] imageData, long targetSize = 100000, int maxDimension = 2048, ImageFormat format = null)
        {
            try
            {
                if (imageData == null || imageData.Length == 0)
                    throw new ArgumentException("Image data không được null hoặc empty");

                using (var originalImage = Image.FromStream(new MemoryStream(imageData)))
                {
                    // Xác định format tối ưu
                    var optimalFormat = format ?? DetermineOptimalFormat(originalImage, targetSize);
                    
                    // Tính toán kích thước mới
                    var newSize = CalculateOptimalSize(originalImage.Width, originalImage.Height, maxDimension, targetSize);
                    
                    // Nén với strategy phù hợp
                    return CompressWithStrategy(originalImage, newSize, targetSize, optimalFormat);
                }
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi nén hình ảnh: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Nén hình ảnh với WebP format (tối ưu cho web)
        /// </summary>
        public byte[] CompressToWebP(byte[] imageData, int quality = 80)
        {
            try
            {
                // TODO: Implement WebP compression using WebP library
                // Hiện tại fallback về JPEG
                return CompressToJpeg(imageData, quality);
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi nén WebP: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Nén hình ảnh với JPEG format
        /// </summary>
        public byte[] CompressToJpeg(byte[] imageData, int quality = 80)
        {
            try
            {
                using (var originalImage = Image.FromStream(new MemoryStream(imageData)))
                {
                    var jpegCodec = GetEncoder(ImageFormat.Jpeg);
                    if (jpegCodec == null)
                        throw new BusinessLogicException("Không tìm thấy JPEG codec");

                    var encoderParams = new EncoderParameters(1);
                    encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, quality);

                    using (var ms = new MemoryStream())
                    {
                        originalImage.Save(ms, jpegCodec, encoderParams);
                        return ms.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi nén JPEG: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Nén hình ảnh với PNG format (tối ưu cho ảnh có transparency)
        /// </summary>
        public byte[] CompressToPng(byte[] imageData, int compressionLevel = 6)
        {
            try
            {
                using (var originalImage = Image.FromStream(new MemoryStream(imageData)))
                {
                    var pngCodec = GetEncoder(ImageFormat.Png);
                    if (pngCodec == null)
                        throw new BusinessLogicException("Không tìm thấy PNG codec");

                    var encoderParams = new EncoderParameters(1);
                    encoderParams.Param[0] = new EncoderParameter(Encoder.Compression, compressionLevel);

                    using (var ms = new MemoryStream())
                    {
                        originalImage.Save(ms, pngCodec, encoderParams);
                        return ms.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi nén PNG: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Tạo progressive JPEG (tải dần)
        /// </summary>
        public byte[] CreateProgressiveJpeg(byte[] imageData, int quality = 80)
        {
            try
            {
                using (var originalImage = Image.FromStream(new MemoryStream(imageData)))
                {
                    var jpegCodec = GetEncoder(ImageFormat.Jpeg);
                    if (jpegCodec == null)
                        throw new BusinessLogicException("Không tìm thấy JPEG codec");

                    var encoderParams = new EncoderParameters(2);
                    encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, quality);
                    encoderParams.Param[1] = new EncoderParameter(Encoder.ScanMethod, (int)EncoderValue.ScanMethodInterlaced);

                    using (var ms = new MemoryStream())
                    {
                        originalImage.Save(ms, jpegCodec, encoderParams);
                        return ms.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi tạo progressive JPEG: {ex.Message}", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Xác định format tối ưu dựa trên loại ảnh và target size
        /// </summary>
        private ImageFormat DetermineOptimalFormat(Image image, long targetSize)
        {
            // Nếu ảnh có transparency, dùng PNG
            if (HasTransparency(image))
                return ImageFormat.Png;

            // Nếu target size nhỏ, dùng JPEG
            if (targetSize < 50000)
                return ImageFormat.Jpeg;

            // Mặc định dùng JPEG
            return ImageFormat.Jpeg;
        }

        /// <summary>
        /// Kiểm tra ảnh có transparency không
        /// </summary>
        private bool HasTransparency(Image image)
        {
            try
            {
                return image.PixelFormat == PixelFormat.Format32bppArgb ||
                       image.PixelFormat == PixelFormat.Format32bppPArgb ||
                       image.PixelFormat == PixelFormat.Format8bppIndexed;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Tính toán kích thước tối ưu
        /// </summary>
        private Size CalculateOptimalSize(int originalWidth, int originalHeight, int maxDimension, long targetSize)
        {
            // Tính kích thước dựa trên maxDimension
            var sizeByDimension = CalculateSizeByDimension(originalWidth, originalHeight, maxDimension);
            
            // Tính kích thước dựa trên targetSize
            var sizeByTarget = CalculateSizeByTarget(originalWidth, originalHeight, targetSize);
            
            // Chọn kích thước nhỏ hơn
            if (sizeByDimension.Width * sizeByDimension.Height < sizeByTarget.Width * sizeByTarget.Height)
                return sizeByDimension;
            else
                return sizeByTarget;
        }

        /// <summary>
        /// Tính kích thước dựa trên maxDimension
        /// </summary>
        private Size CalculateSizeByDimension(int originalWidth, int originalHeight, int maxDimension)
        {
            if (originalWidth <= maxDimension && originalHeight <= maxDimension)
                return new Size(originalWidth, originalHeight);

            double ratio = Math.Min((double)maxDimension / originalWidth, (double)maxDimension / originalHeight);
            
            return new Size(
                (int)(originalWidth * ratio),
                (int)(originalHeight * ratio)
            );
        }

        /// <summary>
        /// Tính kích thước dựa trên targetSize
        /// </summary>
        private Size CalculateSizeByTarget(int originalWidth, int originalHeight, long targetSize)
        {
            // Ước tính kích thước dựa trên target size
            var currentPixels = originalWidth * originalHeight;
            var targetPixels = (int)(targetSize * 8); // Ước tính rough
            
            if (currentPixels <= targetPixels)
                return new Size(originalWidth, originalHeight);

            double ratio = Math.Sqrt((double)targetPixels / currentPixels);
            
            return new Size(
                (int)(originalWidth * ratio),
                (int)(originalHeight * ratio)
            );
        }

        /// <summary>
        /// Nén với strategy phù hợp
        /// </summary>
        private byte[] CompressWithStrategy(Image originalImage, Size newSize, long targetSize, ImageFormat format)
        {
            // Resize ảnh nếu cần
            Image imageToCompress = originalImage;
            if (newSize.Width != originalImage.Width || newSize.Height != originalImage.Height)
            {
                imageToCompress = new Bitmap(newSize.Width, newSize.Height);
                using (var graphics = Graphics.FromImage(imageToCompress))
                {
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    
                    graphics.DrawImage(originalImage, 0, 0, newSize.Width, newSize.Height);
                }
            }

            try
            {
                // Nén theo format
                switch (format.Guid)
                {
                    case var g when g == ImageFormat.Jpeg.Guid:
                        return CompressToJpeg(ImageToByteArray(imageToCompress), 80);
                    case var g when g == ImageFormat.Png.Guid:
                        return CompressToPng(ImageToByteArray(imageToCompress), 6);
                    default:
                        return CompressToJpeg(ImageToByteArray(imageToCompress), 80);
                }
            }
            finally
            {
                if (imageToCompress != originalImage)
                    imageToCompress.Dispose();
            }
        }

        /// <summary>
        /// Chuyển Image thành byte array
        /// </summary>
        private byte[] ImageToByteArray(Image image)
        {
            using (var ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Lấy ImageCodecInfo cho format cụ thể
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

        #endregion
    }
}
