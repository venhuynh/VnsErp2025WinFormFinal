using Dal.DataAccess.MasterData.ProductServiceDal;
using Dal.DataContext;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Bll.MasterData.ProductService
{
    /// <summary>
    /// Business Logic Layer cho ProductImage
    /// </summary>
    public class ProductImageBll
    {
        #region Fields

        private readonly ProductImageDataAccess _dataAccess = new ProductImageDataAccess();

        #endregion

        #region Public Methods

        /// <summary>
        /// Lấy danh sách hình ảnh của sản phẩm/dịch vụ
        /// </summary>
        /// <param name="productId">ID sản phẩm/dịch vụ</param>
        /// <returns>Danh sách hình ảnh</returns>
        public List<ProductImage> GetByProductId(Guid productId)
        {
            try
            {
                return _dataAccess.GetByProductId(productId);
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi lấy danh sách hình ảnh cho sản phẩm '{productId}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy hình ảnh chính của sản phẩm/dịch vụ
        /// </summary>
        /// <param name="productId">ID sản phẩm/dịch vụ</param>
        /// <returns>Hình ảnh chính hoặc null</returns>
        public ProductImage GetPrimaryByProductId(Guid productId)
        {
            try
            {
                return _dataAccess.GetPrimaryByProductId(productId);
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi lấy hình ảnh chính cho sản phẩm '{productId}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lưu hình ảnh từ file và thông tin metadata
        /// </summary>
        /// <param name="productId">ID sản phẩm/dịch vụ</param>
        /// <param name="imageFilePath">Đường dẫn file ảnh</param>
        /// <param name="isPrimary">Có phải hình ảnh chính không</param>
        /// <param name="caption">Chú thích</param>
        /// <param name="altText">Alt text</param>
        /// <returns>ProductImage đã lưu</returns>
        public ProductImage SaveImageFromFile(Guid productId, string imageFilePath, bool isPrimary = false, string caption = null, string altText = null)
        {
            try
            {
                if (!File.Exists(imageFilePath))
                    throw new BusinessLogicException($"File ảnh không tồn tại: {imageFilePath}");

                // Tạo thư mục đích nếu chưa có
                var targetDirectory = GetPhotoDirectory();
                if (!Directory.Exists(targetDirectory))
                    Directory.CreateDirectory(targetDirectory);

                // Tạo tên file mới để tránh trùng lặp
                var fileExtension = Path.GetExtension(imageFilePath);
                var fileName = $"{productId}_{DateTime.Now:yyyyMMdd_HHmmss}_{Guid.NewGuid().ToString("N").Substring(0, 8)}{fileExtension}";
                var targetFilePath = Path.Combine(targetDirectory, fileName);

                // Copy file vào thư mục đích
                File.Copy(imageFilePath, targetFilePath, true);

                // Đọc thông tin ảnh
                var imageInfo = GetImageInfo(imageFilePath);

                // Tạo ProductImage entity
                var productImage = new ProductImage
                {
                    Id = Guid.NewGuid(),
                    ProductId = productId,
                    ImagePath = targetFilePath,
                    SortOrder = isPrimary ? 0 : _dataAccess.GetByProductId(productId).Count + 1,
                    IsPrimary = isPrimary,
                    ImageData = File.ReadAllBytes(imageFilePath),
                    ImageType = fileExtension.TrimStart('.').ToLower(),
                    ImageSize = new FileInfo(imageFilePath).Length,
                    ImageWidth = imageInfo.Width,
                    ImageHeight = imageInfo.Height,
                    Caption = caption,
                    AltText = altText,
                    IsActive = true,
                    CreatedDate = DateTime.Now
                };

                // Lưu vào database
                _dataAccess.SaveOrUpdate(productImage);

                return productImage;
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi lưu hình ảnh từ file '{imageFilePath}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Cập nhật hình ảnh chính cho sản phẩm/dịch vụ
        /// </summary>
        /// <param name="productId">ID sản phẩm/dịch vụ</param>
        /// <param name="imageFilePath">Đường dẫn file ảnh mới</param>
        /// <returns>ProductImage đã cập nhật</returns>
        public ProductImage UpdatePrimaryImage(Guid productId, string imageFilePath)
        {
            try
            {
                if (!File.Exists(imageFilePath))
                    throw new BusinessLogicException($"File ảnh không tồn tại: {imageFilePath}");

                // Tạo thư mục đích nếu chưa có
                var targetDirectory = GetPhotoDirectory();
                if (!Directory.Exists(targetDirectory))
                    Directory.CreateDirectory(targetDirectory);

                // Tạo tên file mới với extension .jpg để đảm bảo định dạng nhất quán
                var fileName = $"{productId}_primary_{DateTime.Now:yyyyMMdd_HHmmss}_{Guid.NewGuid().ToString("N").Substring(0, 8)}.jpg";
                var targetFilePath = Path.Combine(targetDirectory, fileName);

                // Nén ảnh trước khi lưu để đảm bảo kích thước hợp lý
                using (var compressedImage = CompressImage(imageFilePath, 85, 2048))
                {
                    // Lưu ảnh đã nén vào thư mục đích với phương thức an toàn
                    SaveImageSafely(compressedImage, targetFilePath);
                }

                // Đọc thông tin ảnh từ file đã nén
                var imageInfo = GetImageInfo(targetFilePath);

                // Kiểm tra xem đã có hình ảnh chính chưa
                var existingPrimary = _dataAccess.GetPrimaryByProductId(productId);
                
                ProductImage productImage;
                if (existingPrimary != null)
                {
                    // Cập nhật hình ảnh chính hiện có
                    productImage = existingPrimary;
                    productImage.ImagePath = targetFilePath;
                    productImage.ImageData = File.ReadAllBytes(targetFilePath);
                    productImage.ImageType = "jpg";
                    productImage.ImageSize = new FileInfo(targetFilePath).Length;
                    productImage.ImageWidth = imageInfo.Width;
                    productImage.ImageHeight = imageInfo.Height;
                    productImage.ModifiedDate = DateTime.Now;
                }
                else
                {
                    // Tạo mới hình ảnh chính
                    productImage = new ProductImage
                    {
                        Id = Guid.NewGuid(),
                        ProductId = productId,
                        ImagePath = targetFilePath,
                        SortOrder = 0,
                        IsPrimary = true,
                        ImageData = File.ReadAllBytes(targetFilePath),
                        ImageType = "jpg",
                        ImageSize = new FileInfo(targetFilePath).Length,
                        ImageWidth = imageInfo.Width,
                        ImageHeight = imageInfo.Height,
                        Caption = "Hình ảnh chính",
                        AltText = "Hình ảnh chính của sản phẩm",
                        IsActive = true,
                        CreatedDate = DateTime.Now
                    };
                }

                // Lưu vào database
                _dataAccess.SaveOrUpdate(productImage);

                return productImage;
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi cập nhật hình ảnh chính cho sản phẩm '{productId}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Xóa hình ảnh
        /// </summary>
        /// <param name="imageId">ID hình ảnh</param>
        public void DeleteImage(Guid imageId)
        {
            try
            {
                _dataAccess.Delete(imageId);
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi xóa hình ảnh '{imageId}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Xóa tất cả hình ảnh của sản phẩm/dịch vụ
        /// </summary>
        /// <param name="productId">ID sản phẩm/dịch vụ</param>
        public void DeleteAllImages(Guid productId)
        {
            try
            {
                var images = _dataAccess.GetByProductId(productId);
                foreach (var image in images)
                {
                    _dataAccess.Delete(image.Id);
                }
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi xóa tất cả hình ảnh của sản phẩm '{productId}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Đặt hình ảnh làm hình ảnh chính
        /// </summary>
        /// <param name="imageId">ID hình ảnh</param>
        public void SetAsPrimary(Guid imageId)
        {
            try
            {
                _dataAccess.SetAsPrimary(imageId);
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi đặt hình ảnh làm chính '{imageId}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Nén hình ảnh mà không thay đổi kích thước (dimensions)
        /// </summary>
        /// <param name="imageFilePath">Đường dẫn file ảnh gốc</param>
        /// <param name="quality">Chất lượng nén (0-100). Mặc định là 80</param>
        /// <param name="maxDimension">Kích thước tối đa cho mỗi chiều (pixel). Mặc định là 4096</param>
        /// <returns>Đối tượng Image đã được nén và resize nếu cần</returns>
        public Image CompressImage(string imageFilePath, long quality = 80L, int maxDimension = 4096)
        {
            try
            {
                if (!File.Exists(imageFilePath))
                    throw new BusinessLogicException($"File ảnh không tồn tại: {imageFilePath}");

                using var originalImage = Image.FromFile(imageFilePath);
                
                return CompressImage(originalImage, quality, maxDimension);
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi nén hình ảnh từ file '{imageFilePath}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Nén hình ảnh mà không thay đổi kích thước (dimensions)
        /// </summary>
        /// <param name="originalImage">Đối tượng Image gốc</param>
        /// <param name="quality">Chất lượng nén (0-100). Mặc định là 80</param>
        /// <param name="maxDimension">Kích thước tối đa cho mỗi chiều (pixel). Mặc định là 4096</param>
        /// <returns>Đối tượng Image đã được nén và resize nếu cần</returns>
        public Image CompressImage(Image originalImage, long quality = 80L, int maxDimension = 4096)
        {
            try
            {
                if (originalImage == null)
                    throw new ArgumentNullException(nameof(originalImage));

                // Tính toán kích thước mới nếu ảnh quá lớn
                var newSize = CalculateNewSize(originalImage.Width, originalImage.Height, maxDimension);
                
                // Tạo ảnh mới với kích thước đã tính toán
                using (var resizedImage = new Bitmap(newSize.Width, newSize.Height))
                {
                    using (var graphics = Graphics.FromImage(resizedImage))
                    {
                        // Thiết lập chất lượng vẽ
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        graphics.SmoothingMode = SmoothingMode.HighQuality;
                        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        graphics.CompositingQuality = CompositingQuality.HighQuality;

                        // Vẽ ảnh gốc lên ảnh mới
                        graphics.DrawImage(originalImage, 0, 0, newSize.Width, newSize.Height);
                    }

                    // Nén ảnh với chất lượng JPEG và trả về ảnh mới
                    return CompressToJpeg(resizedImage, quality);
                }
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi nén hình ảnh: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Nén ảnh thành JPEG với chất lượng cụ thể
        /// </summary>
        /// <param name="image">Ảnh cần nén</param>
        /// <param name="quality">Chất lượng (0-100)</param>
        /// <returns>Ảnh đã nén</returns>
        private Image CompressToJpeg(Image image, long quality)
        {
            try
            {
                // Lấy JPEG codec
                var jpegCodec = GetEncoder(ImageFormat.Jpeg);
                if (jpegCodec == null)
                {
                    // Nếu không có JPEG codec, trả về ảnh gốc
                    return new Bitmap(image);
                }

                // Thiết lập tham số nén
                var encoderParams = new EncoderParameters(1);
                encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, quality);

                // Tạo bản sao của ảnh để tránh lỗi GDI+
                using (var imageCopy = new Bitmap(image))
                {
                    // Nén ảnh
                    using var ms = new MemoryStream();
                    
                    imageCopy.Save(ms, jpegCodec, encoderParams);
                    ms.Position = 0;
                    
                    // Tạo ảnh mới từ MemoryStream
                    return new Bitmap(ms);
                }
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi nén JPEG: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Tính toán kích thước mới để giữ tỷ lệ và không vượt quá kích thước tối đa
        /// </summary>
        /// <param name="originalWidth">Chiều rộng gốc</param>
        /// <param name="originalHeight">Chiều cao gốc</param>
        /// <param name="maxDimension">Kích thước tối đa cho mỗi chiều</param>
        /// <returns>Kích thước mới</returns>
        private Size CalculateNewSize(int originalWidth, int originalHeight, int maxDimension)
        {
            // Nếu ảnh đã nhỏ hơn kích thước tối đa, giữ nguyên
            if (originalWidth <= maxDimension && originalHeight <= maxDimension)
            {
                return new Size(originalWidth, originalHeight);
            }

            // Tính tỷ lệ để giảm kích thước
            double ratio = Math.Min((double)maxDimension / originalWidth, (double)maxDimension / originalHeight);
            
            return new Size(
                (int)(originalWidth * ratio),
                (int)(originalHeight * ratio)
            );
        }

        /// <summary>
        /// Lấy ImageCodecInfo cho một định dạng ảnh cụ thể
        /// </summary>
        /// <param name="format">Định dạng ảnh</param>
        /// <returns>ImageCodecInfo hoặc null</returns>
        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            try
            {
                var codecs = ImageCodecInfo.GetImageEncoders();
                foreach (var codec in codecs)
                {
                    if (codec.FormatID == format.Guid)
                    {
                        return codec;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Lấy đường dẫn thư mục PHOTO/PRODUCTSERVICE
        /// </summary>
        /// <returns>Đường dẫn thư mục</returns>
        private string GetPhotoDirectory()
        {
            var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            return Path.Combine(appDirectory, "PHOTO", "PRODUCTSERVICE");
        }

        /// <summary>
        /// Lưu ảnh an toàn vào file với error handling
        /// </summary>
        /// <param name="image">Ảnh cần lưu</param>
        /// <param name="filePath">Đường dẫn file đích</param>
        private void SaveImageSafely(Image image, string filePath)
        {
            try
            {
                // Tạo bản sao của ảnh để tránh lỗi GDI+
                using (var imageCopy = new Bitmap(image))
                {
                    // Lấy JPEG codec
                    var jpegCodec = GetEncoder(ImageFormat.Jpeg);
                    if (jpegCodec != null)
                    {
                        // Sử dụng JPEG codec với chất lượng cao
                        var encoderParams = new EncoderParameters(1);
                        encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, 95L);
                        
                        imageCopy.Save(filePath, jpegCodec, encoderParams);
                    }
                    else
                    {
                        // Fallback nếu không có JPEG codec
                        imageCopy.Save(filePath, ImageFormat.Jpeg);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi lưu ảnh vào file '{filePath}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy thông tin kích thước ảnh
        /// </summary>
        /// <param name="imagePath">Đường dẫn file ảnh</param>
        /// <returns>Thông tin kích thước</returns>
        private (int Width, int Height) GetImageInfo(string imagePath)
        {
            try
            {
                using (var image = Image.FromFile(imagePath))
                {
                    return (image.Width, image.Height);
                }
            }
            catch
            {
                return (0, 0);
            }
        }

        #endregion
    }
}
