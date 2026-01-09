using Dal.DataAccess.Interfaces.Inventory.InventoryManagement;
using Dal.DataContext;
using Dal.DtoConverter;
using DTO.Inventory.InventoryManagement;
using Logger;
using Logger.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using Dal.DtoConverter.MasterData.ProductService;
using CustomLogger = Logger.Interfaces.ILogger;

namespace Dal.DataAccess.Implementations.Inventory.InventoryManagement;

/// <summary>
/// Repository implementation cho ProductVariantIdentifierHistory
/// Quản lý các thao tác CRUD với bảng ProductVariantIdentifierHistory (Lịch sử thay đổi định danh biến thể sản phẩm)
/// </summary>
public class ProductVariantIdentifierHistoryRepository : IProductVariantIdentifierHistoryRepository
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
    /// Khởi tạo một instance mới của class ProductVariantIdentifierHistoryRepository.
    /// </summary>
    /// <param name="connectionString">Chuỗi kết nối cơ sở dữ liệu</param>
    /// <exception cref="ArgumentNullException">Ném ra khi connectionString là null</exception>
    public ProductVariantIdentifierHistoryRepository(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _logger = LoggerFactory.CreateLogger(LogCategory.DAL);
        _logger.Info("ProductVariantIdentifierHistoryRepository được khởi tạo với connection string");
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
        // Load ProductVariant và ProductVariantIdentifier
        loadOptions.LoadWith<ProductVariantIdentifierHistory>(h => h.ProductVariant);
        loadOptions.LoadWith<ProductVariantIdentifierHistory>(h => h.ProductVariantIdentifier);
        context.LoadOptions = loadOptions;

        return context;
    }

    /// <summary>
    /// Lấy dictionary thông tin ProductVariant cho danh sách ProductVariantIdentifierHistory
    /// Bao gồm: ProductVariantFullName
    /// </summary>
    private Dictionary<Guid, string> GetProductVariantDict(
        VnsErp2025DataContext context, 
        IEnumerable<ProductVariantIdentifierHistory> histories)
    {
        var productVariantIds = histories
            .Where(h => h.ProductVariantId != Guid.Empty)
            .Select(h => h.ProductVariantId)
            .Distinct()
            .ToList();

        if (!productVariantIds.Any())
            return new Dictionary<Guid, string>();

        var productVariants = context.ProductVariants
            .Where(pv => productVariantIds.Contains(pv.Id))
            .Select(pv => new
            {
                pv.Id,
                pv.VariantFullName
            })
            .ToList();

        return productVariants.ToDictionary(
            pv => pv.Id,
            pv => pv.VariantFullName
        );
    }

    /// <summary>
    /// Lấy thông tin ProductVariant cho một ProductVariantIdentifierHistory
    /// </summary>
    private string GetProductVariantFullName(
        VnsErp2025DataContext context, 
        Guid productVariantId)
    {
        if (productVariantId == Guid.Empty)
            return null;

        var productVariant = context.ProductVariants.FirstOrDefault(pv => pv.Id == productVariantId);
        return productVariant?.VariantFullName;
    }

    #endregion

    #region ========== READ OPERATIONS ==========

    /// <summary>
    /// Lấy tất cả ProductVariantIdentifierHistory
    /// </summary>
    /// <returns>Danh sách tất cả ProductVariantIdentifierHistoryDto</returns>
    public List<ProductVariantIdentifierHistoryDto> GetAll()
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetAll: Lấy tất cả lịch sử thay đổi định danh sản phẩm");

            var entities = context.ProductVariantIdentifierHistories
                .OrderByDescending(h => h.ChangeDate)
                .ToList();

            // Fetch related data trước khi convert để tránh DataContext disposed errors
            var productVariantDict = GetProductVariantDict(context, entities);

            _logger.Info("GetAll: Lấy được {0} bản ghi lịch sử", entities.Count);
            return entities.ToDtoList(productVariantDict);
        }
        catch (Exception ex)
        {
            _logger.Error($"GetAll: Lỗi lấy danh sách lịch sử thay đổi: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Lấy ProductVariantIdentifierHistory theo ID
    /// </summary>
    /// <param name="id">ID của ProductVariantIdentifierHistory</param>
    /// <returns>ProductVariantIdentifierHistoryDto hoặc null</returns>
    public ProductVariantIdentifierHistoryDto GetById(Guid id)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetById: Lấy lịch sử thay đổi, Id={0}", id);

            var entity = context.ProductVariantIdentifierHistories.FirstOrDefault(h => h.Id == id);

            if (entity == null)
            {
                _logger.Warning("GetById: Không tìm thấy lịch sử thay đổi, Id={0}", id);
                return null;
            }

            // Fetch related data trước khi convert để tránh DataContext disposed errors
            var productVariantFullName = GetProductVariantFullName(context, entity.ProductVariantId);

            _logger.Info("GetById: Lấy lịch sử thay đổi thành công, Id={0}", id);
            return entity.ToDto(productVariantFullName);
        }
        catch (Exception ex)
        {
            _logger.Error($"GetById: Lỗi lấy lịch sử thay đổi: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Lấy danh sách ProductVariantIdentifierHistory theo ProductVariantIdentifierId
    /// </summary>
    /// <param name="productVariantIdentifierId">ID định danh sản phẩm</param>
    /// <returns>Danh sách ProductVariantIdentifierHistoryDto</returns>
    public List<ProductVariantIdentifierHistoryDto> GetByProductVariantIdentifierId(Guid productVariantIdentifierId)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetByProductVariantIdentifierId: Lấy danh sách lịch sử thay đổi, ProductVariantIdentifierId={0}", productVariantIdentifierId);

            var entities = context.ProductVariantIdentifierHistories
                .Where(h => h.ProductVariantIdentifierId == productVariantIdentifierId)
                .OrderByDescending(h => h.ChangeDate)
                .ToList();

            // Fetch related data trước khi convert để tránh DataContext disposed errors
            var productVariantDict = GetProductVariantDict(context, entities);

            _logger.Info("GetByProductVariantIdentifierId: Lấy được {0} bản ghi lịch sử", entities.Count);
            return entities.ToDtoList(productVariantDict);
        }
        catch (Exception ex)
        {
            _logger.Error($"GetByProductVariantIdentifierId: Lỗi lấy danh sách lịch sử thay đổi: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Lấy danh sách ProductVariantIdentifierHistory theo ProductVariantId
    /// </summary>
    /// <param name="productVariantId">ID biến thể sản phẩm</param>
    /// <returns>Danh sách ProductVariantIdentifierHistoryDto</returns>
    public List<ProductVariantIdentifierHistoryDto> GetByProductVariantId(Guid productVariantId)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetByProductVariantId: Lấy danh sách lịch sử thay đổi, ProductVariantId={0}", productVariantId);

            var entities = context.ProductVariantIdentifierHistories
                .Where(h => h.ProductVariantId == productVariantId)
                .OrderByDescending(h => h.ChangeDate)
                .ToList();

            // Fetch related data trước khi convert để tránh DataContext disposed errors
            var productVariantDict = GetProductVariantDict(context, entities);

            _logger.Info("GetByProductVariantId: Lấy được {0} bản ghi lịch sử", entities.Count);
            return entities.ToDtoList(productVariantDict);
        }
        catch (Exception ex)
        {
            _logger.Error($"GetByProductVariantId: Lỗi lấy danh sách lịch sử thay đổi: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Lấy danh sách ProductVariantIdentifierHistory theo loại thay đổi (ChangeType)
    /// </summary>
    /// <param name="changeType">Loại thay đổi</param>
    /// <returns>Danh sách ProductVariantIdentifierHistoryDto</returns>
    public List<ProductVariantIdentifierHistoryDto> GetByChangeType(int changeType)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetByChangeType: Lấy danh sách lịch sử thay đổi, ChangeType={0}", changeType);

            var entities = context.ProductVariantIdentifierHistories
                .Where(h => h.ChangeType == changeType)
                .OrderByDescending(h => h.ChangeDate)
                .ToList();

            // Fetch related data trước khi convert để tránh DataContext disposed errors
            var productVariantDict = GetProductVariantDict(context, entities);

            _logger.Info("GetByChangeType: Lấy được {0} bản ghi lịch sử với ChangeType={1}", entities.Count, changeType);
            return entities.ToDtoList(productVariantDict);
        }
        catch (Exception ex)
        {
            _logger.Error($"GetByChangeType: Lỗi lấy danh sách lịch sử thay đổi: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Lấy danh sách ProductVariantIdentifierHistory theo loại thay đổi (ChangeTypeEnum)
    /// </summary>
    /// <param name="changeTypeEnum">Loại thay đổi dưới dạng enum</param>
    /// <returns>Danh sách ProductVariantIdentifierHistoryDto</returns>
    public List<ProductVariantIdentifierHistoryDto> GetByChangeTypeEnum(ProductVariantIdentifierHistoryChangeTypeEnum changeTypeEnum)
    {
        // Convert enum to int value
        var dto = new ProductVariantIdentifierHistoryDto { ChangeTypeEnum = changeTypeEnum };
        return GetByChangeType(dto.ChangeType);
    }

    #endregion

    #region ========== CREATE/UPDATE OPERATIONS ==========

    /// <summary>
    /// Lưu hoặc cập nhật ProductVariantIdentifierHistory
    /// </summary>
    /// <param name="dto">ProductVariantIdentifierHistoryDto cần lưu</param>
    /// <returns>ProductVariantIdentifierHistoryDto đã được lưu</returns>
    public ProductVariantIdentifierHistoryDto SaveOrUpdate(ProductVariantIdentifierHistoryDto dto)
    {
        var context = new VnsErp2025DataContext(_connectionString);
        try
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            _logger.Debug("SaveOrUpdate: Bắt đầu lưu lịch sử thay đổi, Id={0}, ProductVariantIdentifierId={1}",
                dto.Id, dto.ProductVariantIdentifierId);

            var existingEntity = dto.Id != Guid.Empty ? 
                context.ProductVariantIdentifierHistories.FirstOrDefault(h => h.Id == dto.Id) : null;

            ProductVariantIdentifierHistory entity;
            if (existingEntity == null)
            {
                // Thêm mới
                entity = dto.ToEntity();
                if (entity.Id == Guid.Empty)
                {
                    entity.Id = Guid.NewGuid();
                    dto.Id = entity.Id;
                }
                if (entity.ChangeDate == default(DateTime))
                {
                    entity.ChangeDate = DateTime.Now;
                    dto.ChangeDate = entity.ChangeDate;
                }
                context.ProductVariantIdentifierHistories.InsertOnSubmit(entity);
                _logger.Info("SaveOrUpdate: Thêm mới lịch sử thay đổi, Id={0}", entity.Id);
            }
            else
            {
                // Cập nhật
                dto.ToEntity(existingEntity);
                entity = existingEntity;
                _logger.Info("SaveOrUpdate: Cập nhật lịch sử thay đổi, Id={0}", entity.Id);
            }

            context.SubmitChanges();
            _logger.Info("SaveOrUpdate: Lưu lịch sử thay đổi thành công, Id={0}", entity.Id);
            
            // Load lại entity và fetch related data để convert sang DTO
            var savedEntity = context.ProductVariantIdentifierHistories.FirstOrDefault(h => h.Id == entity.Id);
            if (savedEntity == null)
                return null;

            // Fetch related data trước khi convert để tránh DataContext disposed errors
            var productVariantFullName = GetProductVariantFullName(context, savedEntity.ProductVariantId);

            return savedEntity.ToDto(productVariantFullName);
        }
        catch (Exception ex)
        {
            _logger.Error($"SaveOrUpdate: Lỗi lưu lịch sử thay đổi: {ex.Message}", ex);
            throw;
        }
        finally
        {
            context.Dispose();
        }
    }

    #endregion

    #region ========== DELETE OPERATIONS ==========

    /// <summary>
    /// Xóa ProductVariantIdentifierHistory theo ID
    /// </summary>
    /// <param name="id">ID của ProductVariantIdentifierHistory cần xóa</param>
    /// <returns>True nếu xóa thành công, False nếu không tìm thấy</returns>
    public bool Delete(Guid id)
    {
        using var context = CreateNewContext();
        var entity = context.ProductVariantIdentifierHistories.FirstOrDefault(h => h.Id == id);

        try
        {
            _logger.Debug("Delete: Xóa lịch sử thay đổi, Id={0}", id);

            if (entity == null)
            {
                _logger.Warning("Delete: Không tìm thấy lịch sử thay đổi để xóa, Id={0}", id);
                return false;
            }

            context.ProductVariantIdentifierHistories.DeleteOnSubmit(entity);
            context.SubmitChanges();
            
            _logger.Info("Delete: Xóa lịch sử thay đổi thành công, Id={0}", id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.Error($"Delete: Lỗi xóa lịch sử thay đổi: {ex.Message}", ex);
            throw;
        }
    }

    #endregion
}
