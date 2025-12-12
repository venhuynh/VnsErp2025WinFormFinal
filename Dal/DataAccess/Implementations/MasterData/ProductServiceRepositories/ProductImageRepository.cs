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

    #endregion

    #region Public Methods

    /// <summary>
    /// Lấy tất cả hình ảnh
    /// </summary>
    /// <returns>Danh sách tất cả hình ảnh metadata</returns>
    public List<ProductImage> GetAll()
    {
        try
        {
            using var context = CreateNewContext();
            
            // Load tất cả entities từ database
            var entities = context.ProductImages
                .OrderBy(x => x.ProductId)
                .ThenBy(x => x.CreateDate)
                .ToList();
            
            _logger.Debug($"Đã lấy {entities.Count} hình ảnh từ tất cả sản phẩm");
            return entities;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy tất cả hình ảnh: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy tất cả hình ảnh: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Lấy danh sách hình ảnh của sản phẩm/dịch vụ
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
                .Where(x => x.ProductId == productId)
                .OrderBy(x => x.CreateDate)
                .ToList();
            
            _logger.Debug($"Đã lấy {entities.Count} hình ảnh cho sản phẩm: {productId}");
            return entities;
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
            
            _logger.Debug($"Đã lấy hình ảnh theo ID: {imageId} - {entity.FileName ?? entity.RelativePath ?? "N/A"}");
            return entity;
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
    /// Lấy hình ảnh chính của sản phẩm/dịch vụ (lấy hình ảnh đầu tiên)
    /// </summary>
    /// <param name="productId">ID sản phẩm/dịch vụ</param>
    /// <returns>Hình ảnh chính hoặc null</returns>
    public ProductImage GetPrimaryByProductId(Guid productId)
    {
        try
        {
            using var context = CreateNewContext();
            
            // Load entity trực tiếp từ database - lấy hình ảnh đầu tiên
            var entity = context.ProductImages
                .Where(x => x.ProductId == productId)
                .OrderBy(x => x.CreateDate)
                .FirstOrDefault();

            if (entity == null)
            {
                _logger.Debug($"Không tìm thấy hình ảnh cho sản phẩm: {productId}");
                return null;
            }
            
            _logger.Debug($"Đã lấy hình ảnh đầu tiên cho sản phẩm: {productId} - {entity.FileName ?? entity.RelativePath ?? "N/A"}");
            return entity;
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

            var existing = productImage.Id != Guid.Empty ? 
                context.ProductImages.FirstOrDefault(x => x.Id == productImage.Id) : null;

            if (existing == null)
            {
                // Thêm mới
                if (productImage.Id == Guid.Empty)
                    productImage.Id = Guid.NewGuid();
                
                // Thiết lập giá trị mặc định
                if (productImage.CreateDate == default(DateTime))
                    productImage.CreateDate = DateTime.Now;
                if (productImage.CreateBy == Guid.Empty)
                    productImage.CreateBy = Guid.Empty; // Cần set từ context user
                if (productImage.ModifiedBy == Guid.Empty)
                    productImage.ModifiedBy = Guid.Empty; // Cần set từ context user

                context.ProductImages.InsertOnSubmit(productImage);
                context.SubmitChanges();
                
                _logger.Info($"Đã thêm mới hình ảnh: {productImage.Id} - {productImage.FileName ?? productImage.RelativePath ?? "N/A"}");
            }
            else
            {
                // Cập nhật
                existing.ProductId = productImage.ProductId;
                existing.ImageData = productImage.ImageData;
                existing.FileName = productImage.FileName;
                existing.RelativePath = productImage.RelativePath;
                existing.FullPath = productImage.FullPath;
                existing.StorageType = productImage.StorageType;
                existing.FileSize = productImage.FileSize;
                existing.FileExtension = productImage.FileExtension;
                existing.MimeType = productImage.MimeType;
                existing.Checksum = productImage.Checksum;
                existing.FileExists = productImage.FileExists;
                existing.LastVerified = productImage.LastVerified;
                existing.MigrationStatus = productImage.MigrationStatus;
                existing.ModifiedDate = DateTime.Now;
                existing.ModifiedBy = productImage.ModifiedBy;
                
                context.SubmitChanges();
                
                _logger.Info($"Đã cập nhật hình ảnh: {existing.Id} - {existing.FileName ?? existing.RelativePath ?? "N/A"}");
            }
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lưu hình ảnh '{productImage?.FileName ?? productImage?.RelativePath ?? "N/A"}': {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lưu hình ảnh: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Xóa hình ảnh (hard delete - vì không có IsActive property)
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
                context.ProductImages.DeleteOnSubmit(image);
                context.SubmitChanges();
                
                _logger.Info($"Đã xóa hình ảnh: {imageId} - {image.FileName ?? image.RelativePath ?? "N/A"}");
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
        // Vì không có soft delete, Delete và DeletePermanent giống nhau
        Delete(imageId);
    }

    /// <summary>
    /// Kiểm tra xem sản phẩm có hình ảnh chính chưa (kiểm tra xem có hình ảnh nào không)
    /// </summary>
    /// <param name="productId">ID sản phẩm</param>
    /// <returns>True nếu đã có hình ảnh</returns>
    public bool HasPrimaryImage(Guid productId)
    {
        try
        {
            using var context = CreateNewContext();
            var result = context.ProductImages
                .Any(x => x.ProductId == productId);
            
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
    /// Đặt hình ảnh làm hình ảnh chính (không có IsPrimary property, chỉ log)
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
                // Vì không có IsPrimary property, chỉ log thông tin
                _logger.Info($"Đã đánh dấu hình ảnh làm chính (log only): {imageId} - {image.FileName ?? image.RelativePath ?? "N/A"}");
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
                .Where(x => productIds.Contains(x.ProductId.Value))
                .OrderBy(x => x.CreateDate)
                .ToList();
            
            _logger.Debug($"Đã tìm kiếm {entities.Count} hình ảnh cho {productIds.Count} sản phẩm");
            return entities;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi tìm kiếm hình ảnh theo sản phẩm: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi tìm kiếm hình ảnh theo sản phẩm: {ex.Message}", ex);
        }
    }

    #endregion
}
