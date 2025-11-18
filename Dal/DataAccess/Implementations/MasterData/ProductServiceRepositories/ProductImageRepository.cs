using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using Dal.DataAccess.Interfaces.MasterData.ProductServiceRepositories;
using Dal.DataContext;
using Dal.Exceptions;
using Logger;
using Logger.Configuration;
using CustomLogger = Logger.Interfaces.ILogger;

namespace Dal.DataAccess.Implementations.MasterData.ProductServiceRepositories;

/// <summary>
/// Data Access Layer cho ProductImage
/// </summary>
public class ProductImageRepository : IProductImageRepository
{
    #region Private Fields

    /// <summary>
    /// Chuỗi kết nối cơ sở dữ liệu để tạo DataContext mới cho mỗi operation.
    /// </summary>
    private readonly string _connectionString;

    /// <summary>
    /// Instance logger để theo dõi các thao tác và lỗi
    /// </summary>
    private readonly CustomLogger _logger;

    #endregion

    #region Constructor

    /// <summary>
    /// Khởi tạo một instance mới của class ProductImageRepository.
    /// </summary>
    /// <param name="connectionString">Chuỗi kết nối cơ sở dữ liệu</param>
    /// <exception cref="ArgumentNullException">Ném ra khi connectionString là null</exception>
    public ProductImageRepository(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _logger = LoggerFactory.CreateLogger(LogCategory.DAL);
        _logger.Info("ProductImageRepository được khởi tạo với connection string");
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Tạo DataContext mới cho mỗi operation để tránh cache issue
    /// </summary>
    /// <returns>DataContext mới</returns>
    private VnsErp2025DataContext CreateNewContext()
    {
        var context = new VnsErp2025DataContext(_connectionString);

        // Configure eager loading cho navigation properties
        var loadOptions = new DataLoadOptions();
        context.LoadOptions = loadOptions;

        return context;
    }

    /// <summary>
    /// Lấy SortOrder tiếp theo cho sản phẩm
    /// </summary>
    /// <param name="productId">ID sản phẩm</param>
    /// <returns>SortOrder tiếp theo</returns>
    private int GetNextSortOrder(Guid? productId)
    {
        try
        {
            using var context = CreateNewContext();
            var maxOrder = context.ProductImages
                .Where(x => x.ProductId == productId)
                .Max(x => (int?)x.SortOrder) ?? 0;
            return maxOrder + 1;
        }
        catch (Exception ex)
        {
            _logger.Warning($"Lỗi khi lấy SortOrder tiếp theo cho productId {productId}: {ex.Message}. Trả về 1.");
            return 1;
        }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Lấy danh sách hình ảnh của sản phẩm/dịch vụ (bao gồm ImageData)
    /// </summary>
    /// <param name="productId">ID sản phẩm/dịch vụ</param>
    /// <returns>Danh sách hình ảnh metadata</returns>
    public List<ProductImage> GetByProductId(Guid productId)
    {
        try
        {
            using var context = CreateNewContext();
            
            // Load entities trực tiếp từ database
            var entities = context.ProductImages
                .Where(x => x.ProductId == productId && x.IsActive == true)
                .OrderBy(x => x.SortOrder)
                .ThenBy(x => x.CreatedDate)
                .ToList();

            // Map sang ProductImage objects
            var result = entities.Select(x => new ProductImage
            {
                Id = x.Id,
                ProductId = x.ProductId,
                VariantId = x.VariantId,
                ImagePath = x.ImagePath,
                SortOrder = x.SortOrder,
                IsPrimary = x.IsPrimary,
                ImageData = x.ImageData, // Load ImageData để hiển thị hình ảnh
                ImageType = x.ImageType,
                ImageSize = x.ImageSize,
                ImageWidth = x.ImageWidth,
                ImageHeight = x.ImageHeight,
                Caption = x.Caption,
                AltText = x.AltText,
                IsActive = x.IsActive,
                CreatedDate = x.CreatedDate,
                ModifiedDate = x.ModifiedDate
            }).ToList();
            
            _logger.Debug($"Đã lấy {result.Count} hình ảnh cho sản phẩm: {productId}");
            return result;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy danh sách hình ảnh cho sản phẩm '{productId}': {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy danh sách hình ảnh cho sản phẩm '{productId}': {ex.Message}", ex);
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
            using var context = CreateNewContext();
            
            // Load entity trực tiếp từ database
            var entity = context.ProductImages
                .FirstOrDefault(x => x.Id == imageId);

            if (entity == null)
            {
                _logger.Debug($"Không tìm thấy hình ảnh với ID: {imageId}");
                return null;
            }

            // Map sang ProductImage object
            var result = new ProductImage
            {
                Id = entity.Id,
                ProductId = entity.ProductId,
                VariantId = entity.VariantId,
                ImagePath = entity.ImagePath,
                SortOrder = entity.SortOrder,
                IsPrimary = entity.IsPrimary,
                ImageData = entity.ImageData, // Load ImageData để hiển thị hình ảnh
                ImageType = entity.ImageType,
                ImageSize = entity.ImageSize,
                ImageWidth = entity.ImageWidth,
                ImageHeight = entity.ImageHeight,
                Caption = entity.Caption,
                AltText = entity.AltText,
                IsActive = entity.IsActive,
                CreatedDate = entity.CreatedDate,
                ModifiedDate = entity.ModifiedDate
            };
            
            _logger.Debug($"Đã lấy hình ảnh theo ID: {imageId} - {entity.ImagePath}");
            return result;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy hình ảnh '{imageId}': {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy hình ảnh '{imageId}': {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Lấy ImageData của một hình ảnh cụ thể (lazy loading)
    /// </summary>
    /// <param name="imageId">ID hình ảnh</param>
    /// <returns>ImageData binary</returns>
    public byte[] GetImageData(Guid imageId)
    {
        try
        {
            using var context = CreateNewContext();
            var imageData = context.ProductImages
                .Where(x => x.Id == imageId)
                .Select(x => x.ImageData != null ? x.ImageData.ToArray() : null)
                .FirstOrDefault();
            
            if (imageData != null)
            {
                _logger.Debug($"Đã lấy ImageData cho hình ảnh: {imageId} ({imageData.Length} bytes)");
            }
            else
            {
                _logger.Debug($"Không tìm thấy ImageData cho hình ảnh: {imageId}");
            }
            
            return imageData;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy ImageData cho hình ảnh '{imageId}': {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy ImageData cho hình ảnh '{imageId}': {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Lấy hình ảnh chính của sản phẩm/dịch vụ (không bao gồm ImageData)
    /// </summary>
    /// <param name="productId">ID sản phẩm/dịch vụ</param>
    /// <returns>Hình ảnh chính hoặc null</returns>
    public ProductImage GetPrimaryByProductId(Guid productId)
    {
        try
        {
            using var context = CreateNewContext();
            
            // Load entity trực tiếp từ database
            var entity = context.ProductImages
                .FirstOrDefault(x => x.ProductId == productId && (x.IsPrimary ?? false) == true && x.IsActive == true);

            if (entity == null)
            {
                _logger.Debug($"Không tìm thấy hình ảnh chính cho sản phẩm: {productId}");
                return null;
            }

            // Map sang ProductImage object
            var result = new ProductImage
            {
                Id = entity.Id,
                ProductId = entity.ProductId,
                VariantId = entity.VariantId,
                ImagePath = entity.ImagePath,
                SortOrder = entity.SortOrder,
                IsPrimary = entity.IsPrimary,
                ImageType = entity.ImageType,
                ImageSize = entity.ImageSize,
                ImageWidth = entity.ImageWidth,
                ImageHeight = entity.ImageHeight,
                Caption = entity.Caption,
                AltText = entity.AltText,
                IsActive = entity.IsActive,
                CreatedDate = entity.CreatedDate,
                ModifiedDate = entity.ModifiedDate
                // Không load ImageData
            };
            
            _logger.Debug($"Đã lấy hình ảnh chính cho sản phẩm: {productId} - {entity.ImagePath}");
            return result;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy hình ảnh chính cho sản phẩm '{productId}': {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy hình ảnh chính cho sản phẩm '{productId}': {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Lưu hoặc cập nhật hình ảnh
    /// </summary>
    /// <param name="productImage">Hình ảnh cần lưu</param>
    public void SaveOrUpdate(ProductImage productImage)
    {
        try
        {
            if (productImage == null)
                throw new ArgumentNullException(nameof(productImage));

            using var context = CreateNewContext();
            
            // Nếu là hình ảnh chính, bỏ IsPrimary của các hình ảnh khác
            if ((productImage.IsPrimary ?? false) == true)
            {
                var existingPrimary = context.ProductImages
                    .Where(x => x.ProductId == productImage.ProductId && (x.IsPrimary ?? false) == true && x.Id != productImage.Id)
                    .ToList();
                
                foreach (var img in existingPrimary)
                {
                    img.IsPrimary = false;
                }
                
                if (existingPrimary.Any())
                {
                    _logger.Debug($"Đã bỏ IsPrimary của {existingPrimary.Count} hình ảnh khác cho sản phẩm: {productImage.ProductId}");
                }
            }

            var existing = productImage.Id != Guid.Empty ? 
                context.ProductImages.FirstOrDefault(x => x.Id == productImage.Id) : null;

            if (existing == null)
            {
                // Thêm mới
                if (productImage.Id == Guid.Empty)
                    productImage.Id = Guid.NewGuid();
                
                // Thiết lập giá trị mặc định
                if (productImage.CreatedDate == null)
                    productImage.CreatedDate = DateTime.Now;
                if (productImage.IsActive == null)
                    productImage.IsActive = true;
                if (productImage.SortOrder == null)
                    productImage.SortOrder = GetNextSortOrder(productImage.ProductId);

                context.ProductImages.InsertOnSubmit(productImage);
                context.SubmitChanges();
                
                _logger.Info($"Đã thêm mới hình ảnh: {productImage.Id} - {productImage.ImagePath}");
            }
            else
            {
                // Cập nhật
                existing.ProductId = productImage.ProductId;
                existing.VariantId = productImage.VariantId;
                existing.ImagePath = productImage.ImagePath;
                existing.SortOrder = productImage.SortOrder;
                existing.IsPrimary = productImage.IsPrimary;
                existing.ImageData = productImage.ImageData;
                existing.ImageType = productImage.ImageType;
                existing.ImageSize = productImage.ImageSize;
                existing.ImageWidth = productImage.ImageWidth;
                existing.ImageHeight = productImage.ImageHeight;
                existing.Caption = productImage.Caption;
                existing.AltText = productImage.AltText;
                existing.IsActive = productImage.IsActive;
                existing.ModifiedDate = DateTime.Now;
                
                context.SubmitChanges();
                
                _logger.Info($"Đã cập nhật hình ảnh: {existing.Id} - {existing.ImagePath}");
            }
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lưu hình ảnh '{productImage?.ImagePath}': {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lưu hình ảnh '{productImage?.ImagePath}': {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Xóa hình ảnh (soft delete)
    /// </summary>
    /// <param name="imageId">ID hình ảnh</param>
    public void Delete(Guid imageId)
    {
        try
        {
            using var context = CreateNewContext();
            var image = context.ProductImages.FirstOrDefault(x => x.Id == imageId);
            if (image != null)
            {
                image.IsActive = false;
                image.ModifiedDate = DateTime.Now;
                context.SubmitChanges();
                
                _logger.Info($"Đã xóa (soft delete) hình ảnh: {imageId} - {image.ImagePath}");
            }
            else
            {
                _logger.Warning($"Không tìm thấy hình ảnh để xóa: {imageId}");
            }
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi xóa hình ảnh '{imageId}': {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi xóa hình ảnh '{imageId}': {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Xóa vĩnh viễn hình ảnh
    /// </summary>
    /// <param name="imageId">ID hình ảnh</param>
    public void DeletePermanent(Guid imageId)
    {
        try
        {
            using var context = CreateNewContext();
            var image = context.ProductImages.FirstOrDefault(x => x.Id == imageId);
            if (image != null)
            {
                context.ProductImages.DeleteOnSubmit(image);
                context.SubmitChanges();
                
                _logger.Info($"Đã xóa vĩnh viễn hình ảnh: {imageId} - {image.ImagePath}");
            }
            else
            {
                _logger.Warning($"Không tìm thấy hình ảnh để xóa vĩnh viễn: {imageId}");
            }
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi xóa vĩnh viễn hình ảnh '{imageId}': {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi xóa vĩnh viễn hình ảnh '{imageId}': {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Kiểm tra xem sản phẩm có hình ảnh chính chưa
    /// </summary>
    /// <param name="productId">ID sản phẩm</param>
    /// <returns>True nếu đã có hình ảnh chính</returns>
    public bool HasPrimaryImage(Guid productId)
    {
        try
        {
            using var context = CreateNewContext();
            var result = context.ProductImages
                .Any(x => x.ProductId == productId && (x.IsPrimary ?? false) == true && x.IsActive == true);
            
            _logger.Debug($"HasPrimaryImage check cho productId {productId}: {result}");
            return result;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi kiểm tra hình ảnh chính cho sản phẩm '{productId}': {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi kiểm tra hình ảnh chính cho sản phẩm '{productId}': {ex.Message}", ex);
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
            using var context = CreateNewContext();
            var image = context.ProductImages.FirstOrDefault(x => x.Id == imageId);
            if (image != null)
            {
                // Bỏ IsPrimary của các hình ảnh khác cùng sản phẩm
                var otherImages = context.ProductImages
                    .Where(x => x.ProductId == image.ProductId && x.Id != imageId)
                    .ToList();
                
                foreach (var img in otherImages)
                {
                    img.IsPrimary = false;
                }

                // Đặt hình ảnh này làm chính
                image.IsPrimary = true;
                image.ModifiedDate = DateTime.Now;
                context.SubmitChanges();
                
                _logger.Info($"Đã đặt hình ảnh làm chính: {imageId} - {image.ImagePath} (đã bỏ IsPrimary của {otherImages.Count} hình ảnh khác)");
            }
            else
            {
                _logger.Warning($"Không tìm thấy hình ảnh để đặt làm chính: {imageId}");
            }
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi đặt hình ảnh làm chính '{imageId}': {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi đặt hình ảnh làm chính '{imageId}': {ex.Message}", ex);
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
                _logger.Debug("SearchByProductIds: Danh sách productIds rỗng");
                return new List<ProductImage>();
            }

            using var context = CreateNewContext();
            
            // Load entities trực tiếp từ database
            var entities = context.ProductImages
                .Where(x => x.IsActive == true && productIds.Contains(x.ProductId.Value))
                .OrderBy(x => (x.IsPrimary ?? false) ? 0 : 1) // Ưu tiên ảnh chính
                .ThenBy(x => x.SortOrder)
                .ThenBy(x => x.CreatedDate)
                .ToList();

            // Map sang ProductImage objects
            var result = entities.Select(x => new ProductImage
            {
                Id = x.Id,
                ProductId = x.ProductId,
                VariantId = x.VariantId,
                ImagePath = x.ImagePath,
                SortOrder = x.SortOrder,
                IsPrimary = x.IsPrimary,
                ImageData = x.ImageData, // Load ImageData để hiển thị hình ảnh
                ImageType = x.ImageType,
                ImageSize = x.ImageSize,
                ImageWidth = x.ImageWidth,
                ImageHeight = x.ImageHeight,
                Caption = x.Caption,
                AltText = x.AltText,
                IsActive = x.IsActive,
                CreatedDate = x.CreatedDate,
                ModifiedDate = x.ModifiedDate
            }).ToList();
            
            _logger.Debug($"Đã tìm kiếm {result.Count} hình ảnh cho {productIds.Count} sản phẩm");
            return result;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi tìm kiếm hình ảnh theo sản phẩm: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi tìm kiếm hình ảnh theo sản phẩm: {ex.Message}", ex);
        }
    }

    #endregion
}
