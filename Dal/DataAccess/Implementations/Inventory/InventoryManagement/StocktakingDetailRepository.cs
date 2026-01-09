using Dal.DataAccess.Interfaces.Inventory.InventoryManagement;
using Dal.DataContext;
using Dal.DtoConverter.Inventory.StockTakking;
using DTO.Inventory.StockTakking;
using Logger;
using Logger.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using CustomLogger = Logger.Interfaces.ILogger;

namespace Dal.DataAccess.Implementations.Inventory.InventoryManagement;

/// <summary>
/// Repository implementation cho StocktakingDetail
/// Quản lý các thao tác CRUD với bảng StocktakingDetail (Chi tiết kiểm kho)
/// </summary>
public class StocktakingDetailRepository : IStocktakingDetailRepository
{
    #region Private Fields

    private readonly string _connectionString;
    private readonly CustomLogger _logger;

    #endregion

    #region Constructor

    public StocktakingDetailRepository(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _logger = LoggerFactory.CreateLogger(LogCategory.DAL);
        _logger.Info("StocktakingDetailRepository được khởi tạo với connection string");
    }

    #endregion

    #region Helper Methods

    private VnsErp2025DataContext CreateNewContext()
    {
        return new VnsErp2025DataContext(_connectionString);
    }

    private Dictionary<Guid, (string ProductVariantName, string ProductVariantCode)> GetProductVariantDict(
        VnsErp2025DataContext context,
        IEnumerable<StocktakingDetail> details)
    {
        var productVariantIds = details
            .Where(d => d.ProductVariantId != Guid.Empty)
            .Select(d => d.ProductVariantId)
            .Distinct()
            .ToList();

        if (!productVariantIds.Any())
            return new Dictionary<Guid, (string, string)>();

        var productVariants = context.ProductVariants
            .Where(pv => productVariantIds.Contains(pv.Id))
            .Select(pv => new
            {
                pv.Id,
                pv.VariantFullName,
                pv.VariantCode
            })
            .ToList();

        return productVariants.ToDictionary(
            pv => pv.Id,
            pv => (pv.VariantFullName, pv.VariantCode)
        );
    }

    private (string ProductVariantName, string ProductVariantCode) GetProductVariantInfo(
        VnsErp2025DataContext context,
        Guid productVariantId)
    {
        if (productVariantId == Guid.Empty)
            return (null, null);

        var productVariant = context.ProductVariants.FirstOrDefault(pv => pv.Id == productVariantId);
        if (productVariant == null)
            return (null, null);

        return (productVariant.VariantFullName, productVariant.VariantCode);
    }

    #endregion

    #region ========== READ OPERATIONS ==========

    public List<StocktakingDetailDto> GetAll()
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetAll: Lấy tất cả chi tiết kiểm kho");

            var entities = context.StocktakingDetails
                .Where(d => !d.IsDeleted)
                .ToList();

            var productVariantDict = GetProductVariantDict(context, entities);

            _logger.Info("GetAll: Lấy được {0} chi tiết kiểm kho", entities.Count);
            return entities.ToDtoList(productVariantDict);
        }
        catch (Exception ex)
        {
            _logger.Error($"GetAll: Lỗi lấy danh sách chi tiết kiểm kho: {ex.Message}", ex);
            throw;
        }
    }

    public StocktakingDetailDto GetById(Guid id)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetById: Lấy chi tiết kiểm kho, Id={0}", id);

            var entity = context.StocktakingDetails.FirstOrDefault(d => d.Id == id && !d.IsDeleted);

            if (entity == null)
            {
                _logger.Warning("GetById: Không tìm thấy chi tiết kiểm kho, Id={0}", id);
                return null;
            }

            var (productVariantName, productVariantCode) = GetProductVariantInfo(context, entity.ProductVariantId);

            _logger.Info("GetById: Lấy chi tiết kiểm kho thành công, Id={0}", id);
            return entity.ToDto(productVariantName, productVariantCode);
        }
        catch (Exception ex)
        {
            _logger.Error($"GetById: Lỗi lấy chi tiết kiểm kho: {ex.Message}", ex);
            throw;
        }
    }

    public List<StocktakingDetailDto> GetByStocktakingMasterId(Guid stocktakingMasterId)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetByStocktakingMasterId: Lấy danh sách chi tiết kiểm kho, StocktakingMasterId={0}", stocktakingMasterId);

            var entities = context.StocktakingDetails
                .Where(d => d.StocktakingMasterId == stocktakingMasterId && !d.IsDeleted)
                .ToList();

            var productVariantDict = GetProductVariantDict(context, entities);

            _logger.Info("GetByStocktakingMasterId: Lấy được {0} chi tiết kiểm kho", entities.Count);
            return entities.ToDtoList(productVariantDict);
        }
        catch (Exception ex)
        {
            _logger.Error($"GetByStocktakingMasterId: Lỗi lấy danh sách chi tiết kiểm kho: {ex.Message}", ex);
            throw;
        }
    }

    public List<StocktakingDetailDto> GetByProductVariantId(Guid productVariantId)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetByProductVariantId: Lấy danh sách chi tiết kiểm kho, ProductVariantId={0}", productVariantId);

            var entities = context.StocktakingDetails
                .Where(d => d.ProductVariantId == productVariantId && !d.IsDeleted)
                .ToList();

            var productVariantDict = GetProductVariantDict(context, entities);

            _logger.Info("GetByProductVariantId: Lấy được {0} chi tiết kiểm kho", entities.Count);
            return entities.ToDtoList(productVariantDict);
        }
        catch (Exception ex)
        {
            _logger.Error($"GetByProductVariantId: Lỗi lấy danh sách chi tiết kiểm kho: {ex.Message}", ex);
            throw;
        }
    }

    public List<StocktakingDetailDto> GetUncountedByStocktakingMasterId(Guid stocktakingMasterId)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetUncountedByStocktakingMasterId: Lấy danh sách chi tiết chưa kiểm đếm, StocktakingMasterId={0}", stocktakingMasterId);

            var entities = context.StocktakingDetails
                .Where(d => d.StocktakingMasterId == stocktakingMasterId && !d.IsCounted && !d.IsDeleted)
                .ToList();

            var productVariantDict = GetProductVariantDict(context, entities);

            _logger.Info("GetUncountedByStocktakingMasterId: Lấy được {0} chi tiết chưa kiểm đếm", entities.Count);
            return entities.ToDtoList(productVariantDict);
        }
        catch (Exception ex)
        {
            _logger.Error($"GetUncountedByStocktakingMasterId: Lỗi lấy danh sách chi tiết kiểm kho: {ex.Message}", ex);
            throw;
        }
    }

    #endregion

    #region ========== CREATE/UPDATE OPERATIONS ==========

    public StocktakingDetailDto SaveOrUpdate(StocktakingDetailDto dto)
    {
        var context = new VnsErp2025DataContext(_connectionString);
        try
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            _logger.Debug("SaveOrUpdate: Bắt đầu lưu chi tiết kiểm kho, Id={0}", dto.Id);

            var existingEntity = dto.Id != Guid.Empty ?
                context.StocktakingDetails.FirstOrDefault(d => d.Id == dto.Id) : null;

            StocktakingDetail entity;
            if (existingEntity == null)
            {
                entity = dto.ToEntity();
                if (entity.Id == Guid.Empty)
                {
                    entity.Id = Guid.NewGuid();
                    dto.Id = entity.Id;
                }
                if (entity.CreatedDate == null)
                {
                    entity.CreatedDate = DateTime.Now;
                    dto.CreatedDate = entity.CreatedDate;
                }
                context.StocktakingDetails.InsertOnSubmit(entity);
                _logger.Info("SaveOrUpdate: Thêm mới chi tiết kiểm kho, Id={0}", entity.Id);
            }
            else
            {
                dto.ToEntity(existingEntity);
                existingEntity.UpdatedDate = DateTime.Now;
                entity = existingEntity;
                _logger.Info("SaveOrUpdate: Cập nhật chi tiết kiểm kho, Id={0}", entity.Id);
            }

            context.SubmitChanges();
            _logger.Info("SaveOrUpdate: Lưu chi tiết kiểm kho thành công, Id={0}", entity.Id);

            var savedEntity = context.StocktakingDetails.FirstOrDefault(d => d.Id == entity.Id);
            if (savedEntity == null)
                return null;

            var (productVariantName, productVariantCode) = GetProductVariantInfo(context, savedEntity.ProductVariantId);

            return savedEntity.ToDto(productVariantName, productVariantCode);
        }
        catch (Exception ex)
        {
            _logger.Error($"SaveOrUpdate: Lỗi lưu chi tiết kiểm kho: {ex.Message}", ex);
            throw;
        }
        finally
        {
            context.Dispose();
        }
    }

    public List<StocktakingDetailDto> SaveOrUpdateList(List<StocktakingDetailDto> dtos)
    {
        var context = new VnsErp2025DataContext(_connectionString);
        try
        {
            if (dtos == null || dtos.Count == 0)
                return new List<StocktakingDetailDto>();

            _logger.Debug("SaveOrUpdateList: Bắt đầu lưu {0} chi tiết kiểm kho", dtos.Count);

            var savedIds = new List<Guid>();

            foreach (var dto in dtos)
            {
                var existingEntity = dto.Id != Guid.Empty ?
                    context.StocktakingDetails.FirstOrDefault(d => d.Id == dto.Id) : null;

                StocktakingDetail entity;
                if (existingEntity == null)
                {
                    entity = dto.ToEntity();
                    if (entity.Id == Guid.Empty)
                    {
                        entity.Id = Guid.NewGuid();
                        dto.Id = entity.Id;
                    }
                    if (entity.CreatedDate == null)
                    {
                        entity.CreatedDate = DateTime.Now;
                        dto.CreatedDate = entity.CreatedDate;
                    }
                    context.StocktakingDetails.InsertOnSubmit(entity);
                }
                else
                {
                    dto.ToEntity(existingEntity);
                    existingEntity.UpdatedDate = DateTime.Now;
                    entity = existingEntity;
                }
                savedIds.Add(entity.Id);
            }

            context.SubmitChanges();
            _logger.Info("SaveOrUpdateList: Lưu {0} chi tiết kiểm kho thành công", dtos.Count);

            // Load lại tất cả entities đã lưu
            var savedEntities = context.StocktakingDetails
                .Where(d => savedIds.Contains(d.Id))
                .ToList();

            var productVariantDict = GetProductVariantDict(context, savedEntities);

            return savedEntities.ToDtoList(productVariantDict);
        }
        catch (Exception ex)
        {
            _logger.Error($"SaveOrUpdateList: Lỗi lưu danh sách chi tiết kiểm kho: {ex.Message}", ex);
            throw;
        }
        finally
        {
            context.Dispose();
        }
    }

    #endregion

    #region ========== DELETE OPERATIONS ==========

    public bool Delete(Guid id)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("Delete: Xóa chi tiết kiểm kho, Id={0}", id);

            var entity = context.StocktakingDetails.FirstOrDefault(d => d.Id == id);

            if (entity == null)
            {
                _logger.Warning("Delete: Không tìm thấy chi tiết kiểm kho để xóa, Id={0}", id);
                return false;
            }

            entity.IsDeleted = true;
            entity.UpdatedDate = DateTime.Now;

            context.SubmitChanges();
            _logger.Info("Delete: Xóa chi tiết kiểm kho thành công (soft delete), Id={0}", id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.Error($"Delete: Lỗi xóa chi tiết kiểm kho: {ex.Message}", ex);
            throw;
        }
    }

    public int DeleteByStocktakingMasterId(Guid stocktakingMasterId)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("DeleteByStocktakingMasterId: Xóa chi tiết kiểm kho, StocktakingMasterId={0}", stocktakingMasterId);

            var entities = context.StocktakingDetails
                .Where(d => d.StocktakingMasterId == stocktakingMasterId && !d.IsDeleted)
                .ToList();

            var count = entities.Count;
            foreach (var entity in entities)
            {
                entity.IsDeleted = true;
                entity.UpdatedDate = DateTime.Now;
            }

            context.SubmitChanges();
            _logger.Info("DeleteByStocktakingMasterId: Xóa {0} chi tiết kiểm kho thành công", count);
            return count;
        }
        catch (Exception ex)
        {
            _logger.Error($"DeleteByStocktakingMasterId: Lỗi xóa chi tiết kiểm kho: {ex.Message}", ex);
            throw;
        }
    }

    #endregion
}
