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
/// Repository implementation cho StocktakingAdjustment
/// Quản lý các thao tác CRUD với bảng StocktakingAdjustment (Điều chỉnh kho sau kiểm kho)
/// </summary>
public class StocktakingAdjustmentRepository : IStocktakingAdjustmentRepository
{
    #region Private Fields

    private readonly string _connectionString;
    private readonly CustomLogger _logger;

    #endregion

    #region Constructor

    public StocktakingAdjustmentRepository(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _logger = LoggerFactory.CreateLogger(LogCategory.DAL);
        _logger.Info("StocktakingAdjustmentRepository được khởi tạo với connection string");
    }

    #endregion

    #region Helper Methods

    private VnsErp2025DataContext CreateNewContext()
    {
        return new VnsErp2025DataContext(_connectionString);
    }

    private Dictionary<Guid, (string ProductVariantName, string ProductVariantCode)> GetProductVariantDict(
        VnsErp2025DataContext context,
        IEnumerable<StocktakingAdjustment> adjustments)
    {
        var productVariantIds = adjustments
            .Where(a => a.ProductVariantId != Guid.Empty)
            .Select(a => a.ProductVariantId)
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

    public List<StocktakingAdjustmentDto> GetAll()
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetAll: Lấy tất cả điều chỉnh kiểm kho");

            var entities = context.StocktakingAdjustments
                .Where(a => !a.IsDeleted)
                .ToList();

            var productVariantDict = GetProductVariantDict(context, entities);

            _logger.Info("GetAll: Lấy được {0} điều chỉnh kiểm kho", entities.Count);
            return entities.ToDtoList(productVariantDict);
        }
        catch (Exception ex)
        {
            _logger.Error($"GetAll: Lỗi lấy danh sách điều chỉnh kiểm kho: {ex.Message}", ex);
            throw;
        }
    }

    public StocktakingAdjustmentDto GetById(Guid id)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetById: Lấy điều chỉnh kiểm kho, Id={0}", id);

            var entity = context.StocktakingAdjustments.FirstOrDefault(a => a.Id == id && !a.IsDeleted);

            if (entity == null)
            {
                _logger.Warning("GetById: Không tìm thấy điều chỉnh kiểm kho, Id={0}", id);
                return null;
            }

            var (productVariantName, productVariantCode) = GetProductVariantInfo(context, entity.ProductVariantId);

            _logger.Info("GetById: Lấy điều chỉnh kiểm kho thành công, Id={0}", id);
            return entity.ToDto(productVariantName, productVariantCode);
        }
        catch (Exception ex)
        {
            _logger.Error($"GetById: Lỗi lấy điều chỉnh kiểm kho: {ex.Message}", ex);
            throw;
        }
    }

    public List<StocktakingAdjustmentDto> GetByStocktakingMasterId(Guid stocktakingMasterId)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetByStocktakingMasterId: Lấy danh sách điều chỉnh, StocktakingMasterId={0}", stocktakingMasterId);

            var entities = context.StocktakingAdjustments
                .Where(a => a.StocktakingMasterId == stocktakingMasterId && !a.IsDeleted)
                .ToList();

            var productVariantDict = GetProductVariantDict(context, entities);

            _logger.Info("GetByStocktakingMasterId: Lấy được {0} điều chỉnh", entities.Count);
            return entities.ToDtoList(productVariantDict);
        }
        catch (Exception ex)
        {
            _logger.Error($"GetByStocktakingMasterId: Lỗi lấy danh sách điều chỉnh: {ex.Message}", ex);
            throw;
        }
    }

    public List<StocktakingAdjustmentDto> GetByStocktakingDetailId(Guid stocktakingDetailId)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetByStocktakingDetailId: Lấy danh sách điều chỉnh, StocktakingDetailId={0}", stocktakingDetailId);

            var entities = context.StocktakingAdjustments
                .Where(a => a.StocktakingDetailId == stocktakingDetailId && !a.IsDeleted)
                .ToList();

            var productVariantDict = GetProductVariantDict(context, entities);

            _logger.Info("GetByStocktakingDetailId: Lấy được {0} điều chỉnh", entities.Count);
            return entities.ToDtoList(productVariantDict);
        }
        catch (Exception ex)
        {
            _logger.Error($"GetByStocktakingDetailId: Lỗi lấy danh sách điều chỉnh: {ex.Message}", ex);
            throw;
        }
    }

    public List<StocktakingAdjustmentDto> GetUnapplied(Guid? stocktakingMasterId = null)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetUnapplied: Lấy danh sách điều chỉnh chưa áp dụng, StocktakingMasterId={0}", stocktakingMasterId);

            var query = context.StocktakingAdjustments
                .Where(a => !a.IsApplied && !a.IsDeleted);

            if (stocktakingMasterId.HasValue)
            {
                query = query.Where(a => a.StocktakingMasterId == stocktakingMasterId.Value);
            }

            var entities = query.ToList();

            var productVariantDict = GetProductVariantDict(context, entities);

            _logger.Info("GetUnapplied: Lấy được {0} điều chỉnh chưa áp dụng", entities.Count);
            return entities.ToDtoList(productVariantDict);
        }
        catch (Exception ex)
        {
            _logger.Error($"GetUnapplied: Lỗi lấy danh sách điều chỉnh: {ex.Message}", ex);
            throw;
        }
    }

    #endregion

    #region ========== CREATE/UPDATE OPERATIONS ==========

    public StocktakingAdjustmentDto SaveOrUpdate(StocktakingAdjustmentDto dto)
    {
        var context = new VnsErp2025DataContext(_connectionString);
        try
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            _logger.Debug("SaveOrUpdate: Bắt đầu lưu điều chỉnh kiểm kho, Id={0}", dto.Id);

            var existingEntity = dto.Id != Guid.Empty ?
                context.StocktakingAdjustments.FirstOrDefault(a => a.Id == dto.Id) : null;

            StocktakingAdjustment entity;
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
                context.StocktakingAdjustments.InsertOnSubmit(entity);
                _logger.Info("SaveOrUpdate: Thêm mới điều chỉnh kiểm kho, Id={0}", entity.Id);
            }
            else
            {
                dto.ToEntity(existingEntity);
                existingEntity.UpdatedDate = DateTime.Now;
                entity = existingEntity;
                _logger.Info("SaveOrUpdate: Cập nhật điều chỉnh kiểm kho, Id={0}", entity.Id);
            }

            context.SubmitChanges();
            _logger.Info("SaveOrUpdate: Lưu điều chỉnh kiểm kho thành công, Id={0}", entity.Id);

            var savedEntity = context.StocktakingAdjustments.FirstOrDefault(a => a.Id == entity.Id);
            if (savedEntity == null)
                return null;

            var (productVariantName, productVariantCode) = GetProductVariantInfo(context, savedEntity.ProductVariantId);

            return savedEntity.ToDto(productVariantName, productVariantCode);
        }
        catch (Exception ex)
        {
            _logger.Error($"SaveOrUpdate: Lỗi lưu điều chỉnh kiểm kho: {ex.Message}", ex);
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
            _logger.Debug("Delete: Xóa điều chỉnh kiểm kho, Id={0}", id);

            var entity = context.StocktakingAdjustments.FirstOrDefault(a => a.Id == id);

            if (entity == null)
            {
                _logger.Warning("Delete: Không tìm thấy điều chỉnh kiểm kho để xóa, Id={0}", id);
                return false;
            }

            entity.IsDeleted = true;
            entity.UpdatedDate = DateTime.Now;

            context.SubmitChanges();
            _logger.Info("Delete: Xóa điều chỉnh kiểm kho thành công (soft delete), Id={0}", id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.Error($"Delete: Lỗi xóa điều chỉnh kiểm kho: {ex.Message}", ex);
            throw;
        }
    }

    #endregion
}
