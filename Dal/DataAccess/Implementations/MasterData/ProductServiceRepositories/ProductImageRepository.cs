using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using Dal.DataAccess.Interfaces.MasterData.ProductServiceRepositories;
using Dal.DataContext;
using Dal.DtoConverter;
using Dal.Exceptions;
using DTO.MasterData.ProductService;
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

    #region ========== HELPER METHODS ==========

    /// <summary>
    /// Tạo DataContext mới cho mỗi operation để tránh cache issue
    /// </summary>
    /// <returns>DataContext mới</returns>
    private VnsErp2025DataContext CreateNewContext()
    {
        var context = new VnsErp2025DataContext(_connectionString);

        // Configure eager loading cho navigation properties nếu cần
        var loadOptions = new DataLoadOptions();
        // Note: ProductImage không có navigation properties cần eager load
        context.LoadOptions = loadOptions;

        return context;
    }

    #endregion

    #region ========== CREATE OPERATIONS ==========

    /// <summary>
    /// Lưu hoặc cập nhật hình ảnh
    /// </summary>
    /// <param name="dto">ProductImageDto</param>
    public void SaveOrUpdate(ProductImageDto dto)
    {
        try
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            using var context = CreateNewContext();

            var existing = dto.Id != Guid.Empty ? 
                context.ProductImages.FirstOrDefault(x => x.Id == dto.Id) : null;

            if (existing == null)
            {
                // Thêm mới - Map trực tiếp từ DTO sang Entity
                var entity = new ProductImage
                {
                    Id = dto.Id != Guid.Empty ? dto.Id : Guid.NewGuid(),
                    ProductId = dto.ProductId,
                    ImageData = dto.ImageData != null ? new System.Data.Linq.Binary(dto.ImageData) : null,
                    FileName = dto.FileName,
                    RelativePath = dto.RelativePath,
                    FullPath = dto.FullPath,
                    StorageType = dto.StorageType,
                    FileSize = dto.FileSize,
                    FileExtension = dto.FileExtension,
                    MimeType = dto.MimeType,
                    Checksum = dto.Checksum,
                    FileExists = dto.FileExists,
                    LastVerified = dto.LastVerified,
                    MigrationStatus = dto.MigrationStatus,
                    CreateDate = dto.CreateDate != default(DateTime) ? dto.CreateDate : DateTime.Now,
                    CreateBy = dto.CreateBy != Guid.Empty ? dto.CreateBy : Guid.Empty,
                    ModifiedDate = dto.ModifiedDate,
                    ModifiedBy = dto.ModifiedBy
                };

                context.ProductImages.InsertOnSubmit(entity);
                context.SubmitChanges();
                
                _logger.Info($"Đã thêm mới hình ảnh: {entity.Id} - {entity.FileName ?? entity.RelativePath ?? "N/A"}");
            }
            else
            {
                // Cập nhật - Map trực tiếp từ DTO sang Entity
                existing.ProductId = dto.ProductId;
                existing.ImageData = dto.ImageData != null ? new System.Data.Linq.Binary(dto.ImageData) : existing.ImageData;
                existing.FileName = dto.FileName;
                existing.RelativePath = dto.RelativePath;
                existing.FullPath = dto.FullPath;
                existing.StorageType = dto.StorageType;
                existing.FileSize = dto.FileSize;
                existing.FileExtension = dto.FileExtension;
                existing.MimeType = dto.MimeType;
                existing.Checksum = dto.Checksum;
                existing.FileExists = dto.FileExists;
                existing.LastVerified = dto.LastVerified;
                existing.MigrationStatus = dto.MigrationStatus;
                existing.ModifiedDate = DateTime.Now;
                existing.ModifiedBy = dto.ModifiedBy != Guid.Empty ? dto.ModifiedBy : existing.ModifiedBy;
                
                context.SubmitChanges();
                
                _logger.Info($"Đã cập nhật hình ảnh: {existing.Id} - {existing.FileName ?? existing.RelativePath ?? "N/A"}");
            }
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lưu hình ảnh '{dto?.FileName ?? dto?.RelativePath ?? "N/A"}': {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lưu hình ảnh: {ex.Message}", ex);
        }
    }

    #endregion

    #region ========== READ OPERATIONS ==========

    /// <summary>
    /// Lấy tất cả hình ảnh
    /// </summary>
    /// <returns>Danh sách tất cả hình ảnh metadata</returns>
    public List<ProductImageDto> GetAll()
    {
        try
        {
            using var context = CreateNewContext();
            
            // Load tất cả entities từ database
            var entities = context.ProductImages
                .OrderBy(x => x.ProductId)
                .ThenBy(x => x.CreateDate)
                .ToList();
            
            // Chuyển đổi sang DTO
            var dtos = entities.Select(e => e.ToDto()).ToList();
            
            _logger.Debug($"Đã lấy {dtos.Count} hình ảnh từ tất cả sản phẩm");
            return dtos;
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
    public List<ProductImageDto> GetByProductId(Guid productId)
    {
        try
        {
            using var context = CreateNewContext();
            
            // Load entities trực tiếp từ database
            var entities = context.ProductImages
                .Where(x => x.ProductId == productId)
                .OrderBy(x => x.CreateDate)
                .ToList();
            
            // Chuyển đổi sang DTO và tính toán ImageSequenceNumber
            var dtos = new List<ProductImageDto>();
            int sequenceNumber = 1;
            foreach (var entity in entities)
            {
                var dto = entity.ToDto(imageSequenceNumber: sequenceNumber);
                dtos.Add(dto);
                sequenceNumber++;
            }
            
            _logger.Debug($"Đã lấy {dtos.Count} hình ảnh cho sản phẩm: {productId}");
            return dtos;
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
    public ProductImageDto GetById(Guid imageId)
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
            return entity.ToDto();
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy hình ảnh '{imageId}': {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy hình ảnh '{imageId}': {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Lấy hình ảnh chính của sản phẩm/dịch vụ (lấy hình ảnh đầu tiên)
    /// </summary>
    /// <param name="productId">ID sản phẩm/dịch vụ</param>
    /// <returns>Hình ảnh chính hoặc null</returns>
    public ProductImageDto GetPrimaryByProductId(Guid productId)
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
            return entity.ToDto(imageSequenceNumber: 1);
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy hình ảnh chính cho sản phẩm '{productId}': {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy hình ảnh chính cho sản phẩm '{productId}': {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Tìm kiếm hình ảnh theo danh sách ProductId
    /// </summary>
    /// <param name="productIds">Danh sách ID sản phẩm/dịch vụ</param>
    /// <returns>Danh sách hình ảnh phù hợp</returns>
    public List<ProductImageDto> SearchByProductIds(List<Guid> productIds)
    {
        try
        {
            if (productIds == null || !productIds.Any())
            {
                _logger.Debug("SearchByProductIds: Danh sách productIds rỗng");
                return new List<ProductImageDto>();
            }

            using var context = CreateNewContext();
            
            // Load entities trực tiếp từ database
            var entities = context.ProductImages
                .Where(x => productIds.Contains(x.ProductId.Value))
                .OrderBy(x => x.CreateDate)
                .ToList();
            
            // Chuyển đổi sang DTO
            var dtos = entities.Select(e => e.ToDto()).ToList();
            
            _logger.Debug($"Đã tìm kiếm {dtos.Count} hình ảnh cho {productIds.Count} sản phẩm");
            return dtos;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi tìm kiếm hình ảnh theo sản phẩm: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi tìm kiếm hình ảnh theo sản phẩm: {ex.Message}", ex);
        }
    }

    #endregion

    #region ========== UPDATE OPERATIONS ==========
    // Update operations are handled by SaveOrUpdate method
    #endregion

    #region ========== DELETE OPERATIONS ==========

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

    #endregion

    #region ========== BUSINESS LOGIC METHODS ==========

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

    #endregion
}
