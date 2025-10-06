using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dal.DataAccess.MasterData.ProductServiceDal;
using Dal.DataContext;

namespace Bll.MasterData.ProductServiceBll
{
    /// <summary>
    /// Business Logic Layer cho ProductImage
    /// </summary>
    public class ProductImageBll
    {
        #region Constants

        /// <summary>
        /// Thư mục gốc chứa ảnh sản phẩm
        /// </summary>
        private const string PHOTO_ROOT_DIRECTORY = "PHOTO";

        /// <summary>
        /// Thư mục con chứa ảnh sản phẩm/dịch vụ
        /// </summary>
        private const string PRODUCTSERVICE_PHOTO_DIRECTORY = "PRODUCTSERVICE";

        /// <summary>
        /// Thư mục con chứa ảnh biến thể sản phẩm
        /// </summary>
        private const string PRODUCTVARIANT_PHOTO_DIRECTORY = "PRODUCTVARIANT";

        /// <summary>
        /// Thư mục con chứa ảnh thumbnail
        /// </summary>
        private const string THUMBNAIL_PHOTO_DIRECTORY = "THUMBNAIL";

        /// <summary>
        /// Thư mục con chứa ảnh đã nén
        /// </summary>
        private const string COMPRESSED_PHOTO_DIRECTORY = "COMPRESSED";

        /// <summary>
        /// Định dạng tên file cho ảnh sản phẩm
        /// </summary>
        private const string PRODUCT_IMAGE_FILENAME_FORMAT = "{0}_{1:yyyyMMdd_HHmmss}_{2}{3}";

        /// <summary>
        /// Định dạng tên file cho ảnh chính
        /// </summary>
        private const string PRIMARY_IMAGE_FILENAME_FORMAT = "{0}_primary_{1:yyyyMMdd_HHmmss}_{2}.jpg";

        /// <summary>
        /// Định dạng tên file cho thumbnail
        /// </summary>
        private const string THUMBNAIL_FILENAME_FORMAT = "{0}_thumb_{1:yyyyMMdd_HHmmss}_{2}.jpg";

        /// <summary>
        /// Chất lượng nén mặc định cho ảnh
        /// </summary>
        private const long DEFAULT_COMPRESSION_QUALITY = 85L;

        /// <summary>
        /// Kích thước tối đa mặc định cho mỗi chiều (pixel)
        /// </summary>
        private const int DEFAULT_MAX_DIMENSION = 2048;

        /// <summary>
        /// Kích thước tối đa cho thumbnail (pixel)
        /// </summary>
        private const int THUMBNAIL_MAX_DIMENSION = 300;

        /// <summary>
        /// Chất lượng nén cho thumbnail
        /// </summary>
        private const long THUMBNAIL_COMPRESSION_QUALITY = 75L;

        #endregion

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
        /// Lấy hình ảnh theo ID
        /// </summary>
        /// <param name="imageId">ID hình ảnh</param>
        /// <returns>Hình ảnh hoặc null</returns>
        public ProductImage GetById(Guid imageId)
        {
            try
            {
                return _dataAccess.GetById(imageId);
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi lấy hình ảnh '{imageId}': {ex.Message}", ex);
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
                var fileName = string.Format(PRODUCT_IMAGE_FILENAME_FORMAT, 
                    productId, 
                    DateTime.Now, 
                    Guid.NewGuid().ToString("N").Substring(0, 8), 
                    fileExtension);
                var targetFilePath = Path.Combine(targetDirectory, fileName);

                // Copy file vào thư mục đích
                File.Copy(imageFilePath, targetFilePath, true);

                // Đọc thông tin ảnh
                var imageInfo = GetImageInfo(imageFilePath);

                // Lấy SortOrder tiếp theo
                var nextSortOrder = GetNextSortOrder(productId);

                // Tạo ProductImage entity
                var productImage = new ProductImage
                {
                    Id = Guid.NewGuid(),
                    ProductId = productId,
                    ImagePath = targetFilePath,
                    SortOrder = isPrimary ? 0 : nextSortOrder,
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
                var fileName = string.Format(PRIMARY_IMAGE_FILENAME_FORMAT, 
                    productId, 
                    DateTime.Now, 
                    Guid.NewGuid().ToString("N").Substring(0, 8));
                var targetFilePath = Path.Combine(targetDirectory, fileName);

                // Nén ảnh trước khi lưu để đảm bảo kích thước hợp lý
                using (var compressedImage = CompressImage(imageFilePath, DEFAULT_COMPRESSION_QUALITY, DEFAULT_MAX_DIMENSION))
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
        /// Xóa hình ảnh hoàn chỉnh (database + file + cập nhật ProductService)
        /// </summary>
        /// <param name="imageId">ID hình ảnh</param>
        public void DeleteImageComplete(Guid imageId)
        {
            try
            {
                // 1. Lấy thông tin hình ảnh trước khi xóa
                var imageInfo = _dataAccess.GetById(imageId);
                if (imageInfo == null)
                {
                    throw new BusinessLogicException($"Không tìm thấy hình ảnh với ID '{imageId}'");
                }

                var productId = imageInfo.ProductId;
                var imagePath = imageInfo.ImagePath;
                var isPrimary = imageInfo.IsPrimary ?? false;

                // 2. Xóa file vật lý nếu tồn tại
                if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
                {
                    try
                    {
                        File.Delete(imagePath);
                        System.Diagnostics.Debug.WriteLine($"Đã xóa file: {imagePath}");
                    }
                    catch (Exception fileEx)
                    {
                        System.Diagnostics.Debug.WriteLine($"Không thể xóa file '{imagePath}': {fileEx.Message}");
                        // Không throw exception vì có thể file đã bị xóa hoặc không có quyền
                    }
                }

                // 3. Xóa thumbnail nếu có
                DeleteThumbnailIfExists(imageInfo);

                // 4. Xóa trong database
                _dataAccess.Delete(imageId);

                // 5. Cập nhật ProductService nếu đây là ảnh chính
                if (isPrimary && productId.HasValue)
                {
                    UpdateProductServiceAfterPrimaryImageDelete(productId.Value);
                }

                System.Diagnostics.Debug.WriteLine($"Đã xóa hoàn chỉnh hình ảnh '{imageId}'");
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi xóa hoàn chỉnh hình ảnh '{imageId}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Xóa thumbnail nếu tồn tại
        /// </summary>
        /// <param name="imageInfo">Thông tin hình ảnh</param>
        private void DeleteThumbnailIfExists(ProductImage imageInfo)
        {
            try
            {
                if (imageInfo?.ProductId == null) return;

                var thumbnailDirectory = GetThumbnailDirectory();
                var thumbnailPattern = $"{imageInfo.ProductId}_thumb_*";
                var thumbnailFiles = Directory.GetFiles(thumbnailDirectory, thumbnailPattern);

                foreach (var thumbnailFile in thumbnailFiles)
                {
                    try
                    {
                        File.Delete(thumbnailFile);
                        System.Diagnostics.Debug.WriteLine($"Đã xóa thumbnail: {thumbnailFile}");
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Không thể xóa thumbnail '{thumbnailFile}': {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khi xóa thumbnail: {ex.Message}");
            }
        }

        /// <summary>
        /// Cập nhật ProductService sau khi xóa ảnh chính
        /// </summary>
        /// <param name="productId">ID sản phẩm</param>
        private void UpdateProductServiceAfterPrimaryImageDelete(Guid productId)
        {
            try
            {
                // TODO: Implement logic để cập nhật ProductService
                // Có thể cần:
                // 1. Xóa đường dẫn ảnh chính trong ProductService
                // 2. Đặt ảnh chính mới nếu có ảnh khác
                // 3. Cập nhật trạng thái sản phẩm
                
                System.Diagnostics.Debug.WriteLine($"Cần cập nhật ProductService cho sản phẩm '{productId}' sau khi xóa ảnh chính");
                
                // Tạm thời chỉ log - cần implement logic cụ thể
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khi cập nhật ProductService: {ex.Message}");
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
        /// Xóa hình ảnh (alias cho DeleteImage)
        /// </summary>
        /// <param name="imageId">ID hình ảnh</param>
        public void Delete(Guid imageId)
        {
            DeleteImage(imageId);
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
        /// Tạo thumbnail cho hình ảnh
        /// </summary>
        /// <param name="productId">ID sản phẩm</param>
        /// <param name="imageFilePath">Đường dẫn file ảnh gốc</param>
        /// <returns>Đường dẫn file thumbnail</returns>
        public string CreateThumbnail(Guid productId, string imageFilePath)
        {
            try
            {
                if (!File.Exists(imageFilePath))
                    throw new BusinessLogicException($"File ảnh không tồn tại: {imageFilePath}");

                // Tạo thư mục thumbnail nếu chưa có
                var thumbnailDirectory = GetThumbnailDirectory();
                if (!Directory.Exists(thumbnailDirectory))
                    Directory.CreateDirectory(thumbnailDirectory);

                // Tạo tên file thumbnail
                var thumbnailFileName = string.Format(THUMBNAIL_FILENAME_FORMAT, 
                    productId, 
                    DateTime.Now, 
                    Guid.NewGuid().ToString("N").Substring(0, 8));
                var thumbnailFilePath = Path.Combine(thumbnailDirectory, thumbnailFileName);

                // Tạo thumbnail với kích thước và chất lượng phù hợp
                using (var thumbnailImage = CompressImage(imageFilePath, THUMBNAIL_COMPRESSION_QUALITY, THUMBNAIL_MAX_DIMENSION))
                {
                    SaveImageSafely(thumbnailImage, thumbnailFilePath);
                }

                return thumbnailFilePath;
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi tạo thumbnail cho hình ảnh '{imageFilePath}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Nén hình ảnh mà không thay đổi kích thước (dimensions)
        /// </summary>
        /// <param name="imageFilePath">Đường dẫn file ảnh gốc</param>
        /// <param name="quality">Chất lượng nén (0-100). Mặc định sử dụng DEFAULT_COMPRESSION_QUALITY</param>
        /// <param name="maxDimension">Kích thước tối đa cho mỗi chiều (pixel). Mặc định sử dụng DEFAULT_MAX_DIMENSION</param>
        /// <returns>Đối tượng Image đã được nén và resize nếu cần</returns>
        public Image CompressImage(string imageFilePath, long quality = -1, int maxDimension = -1)
        {
            try
            {
                if (!File.Exists(imageFilePath))
                    throw new BusinessLogicException($"File ảnh không tồn tại: {imageFilePath}");

                // Sử dụng giá trị mặc định nếu không được chỉ định
                var actualQuality = quality == -1 ? DEFAULT_COMPRESSION_QUALITY : quality;
                var actualMaxDimension = maxDimension == -1 ? DEFAULT_MAX_DIMENSION : maxDimension;

                using var originalImage = Image.FromFile(imageFilePath);
                
                return CompressImage(originalImage, actualQuality, actualMaxDimension);
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
        /// <param name="quality">Chất lượng nén (0-100). Mặc định sử dụng DEFAULT_COMPRESSION_QUALITY</param>
        /// <param name="maxDimension">Kích thước tối đa cho mỗi chiều (pixel). Mặc định sử dụng DEFAULT_MAX_DIMENSION</param>
        /// <returns>Đối tượng Image đã được nén và resize nếu cần</returns>
        public Image CompressImage(Image originalImage, long quality = -1, int maxDimension = -1)
        {
            try
            {
                if (originalImage == null)
                    throw new ArgumentNullException(nameof(originalImage));

                // Sử dụng giá trị mặc định nếu không được chỉ định
                var actualQuality = quality == -1 ? DEFAULT_COMPRESSION_QUALITY : quality;
                var actualMaxDimension = maxDimension == -1 ? DEFAULT_MAX_DIMENSION : maxDimension;

                // Tính toán kích thước mới nếu ảnh quá lớn
                var newSize = CalculateNewSize(originalImage.Width, originalImage.Height, actualMaxDimension);
                
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
                    return CompressToJpeg(resizedImage, actualQuality);
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
            return Path.Combine(appDirectory, PHOTO_ROOT_DIRECTORY, PRODUCTSERVICE_PHOTO_DIRECTORY);
        }

        /// <summary>
        /// Lấy đường dẫn thư mục cho ảnh biến thể sản phẩm
        /// </summary>
        /// <returns>Đường dẫn thư mục</returns>
        private string GetProductVariantPhotoDirectory()
        {
            var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            return Path.Combine(appDirectory, PHOTO_ROOT_DIRECTORY, PRODUCTVARIANT_PHOTO_DIRECTORY);
        }

        /// <summary>
        /// Lấy đường dẫn thư mục thumbnail
        /// </summary>
        /// <returns>Đường dẫn thư mục</returns>
        private string GetThumbnailDirectory()
        {
            var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            return Path.Combine(appDirectory, PHOTO_ROOT_DIRECTORY, THUMBNAIL_PHOTO_DIRECTORY);
        }

        /// <summary>
        /// Lấy đường dẫn thư mục ảnh đã nén
        /// </summary>
        /// <returns>Đường dẫn thư mục</returns>
        private string GetCompressedPhotoDirectory()
        {
            var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            return Path.Combine(appDirectory, PHOTO_ROOT_DIRECTORY, COMPRESSED_PHOTO_DIRECTORY);
        }

        /// <summary>
        /// Lấy đường dẫn thư mục dựa trên loại ảnh
        /// </summary>
        /// <param name="imageType">Loại ảnh (Product, Variant, Thumbnail, Compressed)</param>
        /// <returns>Đường dẫn thư mục</returns>
        private string GetPhotoDirectoryByType(string imageType)
        {
            var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            
            return imageType?.ToUpper() switch
            {
                "VARIANT" => Path.Combine(appDirectory, PHOTO_ROOT_DIRECTORY, PRODUCTVARIANT_PHOTO_DIRECTORY),
                "THUMBNAIL" => Path.Combine(appDirectory, PHOTO_ROOT_DIRECTORY, THUMBNAIL_PHOTO_DIRECTORY),
                "COMPRESSED" => Path.Combine(appDirectory, PHOTO_ROOT_DIRECTORY, COMPRESSED_PHOTO_DIRECTORY),
                _ => Path.Combine(appDirectory, PHOTO_ROOT_DIRECTORY, PRODUCTSERVICE_PHOTO_DIRECTORY)
            };
        }

        /// <summary>
        /// Tạo tên file theo định dạng chuẩn
        /// </summary>
        /// <param name="productId">ID sản phẩm</param>
        /// <param name="imageType">Loại ảnh (primary, thumb, normal)</param>
        /// <param name="fileExtension">Extension file</param>
        /// <returns>Tên file</returns>
        private string GenerateFileName(Guid productId, string imageType, string fileExtension = ".jpg")
        {
            var timestamp = DateTime.Now;
            var uniqueId = Guid.NewGuid().ToString("N").Substring(0, 8);
            
            return imageType?.ToLower() switch
            {
                "primary" => string.Format(PRIMARY_IMAGE_FILENAME_FORMAT, productId, timestamp, uniqueId),
                "thumb" => string.Format(THUMBNAIL_FILENAME_FORMAT, productId, timestamp, uniqueId),
                _ => string.Format(PRODUCT_IMAGE_FILENAME_FORMAT, productId, timestamp, uniqueId, fileExtension)
            };
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

        /// <summary>
        /// Lấy SortOrder tiếp theo cho sản phẩm
        /// </summary>
        /// <param name="productId">ID sản phẩm</param>
        /// <returns>SortOrder tiếp theo</returns>
        private int GetNextSortOrder(Guid productId)
        {
            try
            {
                // Tính toán SortOrder trực tiếp thay vì gọi GetByProductId
                var existingImages = _dataAccess.GetByProductId(productId);
                return existingImages.Count + 1;
            }
            catch (Exception)
            {
                // Fallback: trả về 1 nếu có lỗi
                return 1;
            }
        }

        /// <summary>
        /// Tìm kiếm hình ảnh theo danh sách ProductId
        /// </summary>
        /// <param name="productIds">Danh sách ID sản phẩm/dịch vụ</param>
        /// <returns>Danh sách hình ảnh phù hợp</returns>
        public List<ProductImage> SearchByProductIds(List<Guid> productIds)
        {
            try
            {
                if (productIds == null || !productIds.Any())
                {
                    return new List<ProductImage>();
                }

                return _dataAccess.SearchByProductIds(productIds);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi tìm kiếm hình ảnh theo sản phẩm: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Tìm kiếm hình ảnh theo danh sách ProductId (Async)
        /// </summary>
        /// <param name="productIds">Danh sách ID sản phẩm/dịch vụ</param>
        /// <returns>Danh sách hình ảnh phù hợp</returns>
        public async Task<List<ProductImage>> SearchByProductIdsAsync(List<Guid> productIds)
        {
            try
            {
                if (productIds == null || !productIds.Any())
                {
                    return new List<ProductImage>();
                }

                return await Task.Run(() => _dataAccess.SearchByProductIds(productIds));
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi tìm kiếm hình ảnh theo sản phẩm: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Tìm kiếm hình ảnh theo từ khóa (tìm kiếm trong ProductService và Category trước)
        /// </summary>
        /// <param name="searchKeyword">Từ khóa tìm kiếm</param>
        /// <returns>Danh sách hình ảnh phù hợp</returns>
        public async Task<List<ProductImage>> SearchAsync(string searchKeyword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchKeyword))
                {
                    return new List<ProductImage>();
                }

                // Tìm kiếm sản phẩm/dịch vụ trước
                var productServiceBll = new ProductServiceBll();
                var searchResults = await productServiceBll.SearchAsync(searchKeyword.Trim());
                
                if (!searchResults.Any())
                {
                    return new List<ProductImage>();
                }

                // Lấy danh sách ProductId từ kết quả tìm kiếm
                var productIds = searchResults.Select(x => x.Id).ToList();
                
                // Tìm kiếm hình ảnh theo danh sách ProductId
                return await SearchByProductIdsAsync(productIds);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi tìm kiếm hình ảnh: {ex.Message}", ex);
            }
        }

        #endregion
    }
}
