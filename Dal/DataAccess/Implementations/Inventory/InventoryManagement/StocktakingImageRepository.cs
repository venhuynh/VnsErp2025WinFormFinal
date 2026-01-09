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
/// Repository implementation cho StocktakingImage
/// Quản lý các thao tác CRUD với bảng StocktakingImage (Hình ảnh kiểm kho)
/// </summary>
public class StocktakingImageRepository : IStocktakingImageRepository
{
    #region Private Fields

    private readonly string _connectionString;
    private readonly CustomLogger _logger;

    #endregion

    #region Constructor

    public StocktakingImageRepository(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _logger = LoggerFactory.CreateLogger(LogCategory.DAL);
        _logger.Info("StocktakingImageRepository được khởi tạo với connection string");
    }

    #endregion

    #region Helper Methods

    private VnsErp2025DataContext CreateNewContext()
    {
        return new VnsErp2025DataContext(_connectionString);
    }

    #endregion

    #region ========== READ OPERATIONS ==========

    public List<StocktakingImageDto> GetAll()
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetAll: Lấy tất cả hình ảnh kiểm kho");

            var entities = context.StocktakingImages.ToList();

            _logger.Info("GetAll: Lấy được {0} hình ảnh kiểm kho", entities.Count);
            return entities.ToDtos();
        }
        catch (Exception ex)
        {
            _logger.Error($"GetAll: Lỗi lấy danh sách hình ảnh kiểm kho: {ex.Message}", ex);
            throw;
        }
    }

    public StocktakingImageDto GetById(Guid id)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetById: Lấy hình ảnh kiểm kho, Id={0}", id);

            var entity = context.StocktakingImages.FirstOrDefault(i => i.Id == id);

            if (entity == null)
            {
                _logger.Warning("GetById: Không tìm thấy hình ảnh kiểm kho, Id={0}", id);
                return null;
            }

            _logger.Info("GetById: Lấy hình ảnh kiểm kho thành công, Id={0}", id);
            return entity.ToDto();
        }
        catch (Exception ex)
        {
            _logger.Error($"GetById: Lỗi lấy hình ảnh kiểm kho: {ex.Message}", ex);
            throw;
        }
    }

    public List<StocktakingImageDto> GetByStocktakingMasterId(Guid? stocktakingMasterId)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetByStocktakingMasterId: Lấy danh sách hình ảnh, StocktakingMasterId={0}", stocktakingMasterId);

            var entities = context.StocktakingImages
                .Where(i => i.StocktakingMasterId == stocktakingMasterId)
                .ToList();

            _logger.Info("GetByStocktakingMasterId: Lấy được {0} hình ảnh", entities.Count);
            return entities.ToDtos();
        }
        catch (Exception ex)
        {
            _logger.Error($"GetByStocktakingMasterId: Lỗi lấy danh sách hình ảnh: {ex.Message}", ex);
            throw;
        }
    }

    public List<StocktakingImageDto> GetByStocktakingDetailId(Guid? stocktakingDetailId)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetByStocktakingDetailId: Lấy danh sách hình ảnh, StocktakingDetailId={0}", stocktakingDetailId);

            var entities = context.StocktakingImages
                .Where(i => i.StocktakingDetailId == stocktakingDetailId)
                .ToList();

            _logger.Info("GetByStocktakingDetailId: Lấy được {0} hình ảnh", entities.Count);
            return entities.ToDtos();
        }
        catch (Exception ex)
        {
            _logger.Error($"GetByStocktakingDetailId: Lỗi lấy danh sách hình ảnh: {ex.Message}", ex);
            throw;
        }
    }

    #endregion

    #region ========== CREATE/UPDATE OPERATIONS ==========

    public StocktakingImageDto SaveOrUpdate(StocktakingImageDto dto)
    {
        var context = new VnsErp2025DataContext(_connectionString);
        try
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            _logger.Debug("SaveOrUpdate: Bắt đầu lưu hình ảnh kiểm kho, Id={0}", dto.Id);

            var existingEntity = dto.Id != Guid.Empty ?
                context.StocktakingImages.FirstOrDefault(i => i.Id == dto.Id) : null;

            StocktakingImage entity;
            if (existingEntity == null)
            {
                entity = dto.ToEntity();
                if (entity.Id == Guid.Empty)
                {
                    entity.Id = Guid.NewGuid();
                    dto.Id = entity.Id;
                }
                if (entity.CreateDate == default(DateTime))
                {
                    entity.CreateDate = DateTime.Now;
                    dto.CreateDate = entity.CreateDate;
                }
                if (entity.CreateBy == Guid.Empty)
                {
                    entity.CreateBy = dto.CreateBy;
                }
                context.StocktakingImages.InsertOnSubmit(entity);
                _logger.Info("SaveOrUpdate: Thêm mới hình ảnh kiểm kho, Id={0}", entity.Id);
            }
            else
            {
                dto.ToEntity(existingEntity);
                existingEntity.ModifiedDate = DateTime.Now;
                entity = existingEntity;
                _logger.Info("SaveOrUpdate: Cập nhật hình ảnh kiểm kho, Id={0}", entity.Id);
            }

            context.SubmitChanges();
            _logger.Info("SaveOrUpdate: Lưu hình ảnh kiểm kho thành công, Id={0}", entity.Id);

            var savedEntity = context.StocktakingImages.FirstOrDefault(i => i.Id == entity.Id);
            if (savedEntity == null)
                return null;

            return savedEntity.ToDto();
        }
        catch (Exception ex)
        {
            _logger.Error($"SaveOrUpdate: Lỗi lưu hình ảnh kiểm kho: {ex.Message}", ex);
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
            _logger.Debug("Delete: Xóa hình ảnh kiểm kho, Id={0}", id);

            var entity = context.StocktakingImages.FirstOrDefault(i => i.Id == id);

            if (entity == null)
            {
                _logger.Warning("Delete: Không tìm thấy hình ảnh kiểm kho để xóa, Id={0}", id);
                return false;
            }

            // Hard delete cho hình ảnh
            context.StocktakingImages.DeleteOnSubmit(entity);
            context.SubmitChanges();

            _logger.Info("Delete: Xóa hình ảnh kiểm kho thành công, Id={0}", id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.Error($"Delete: Lỗi xóa hình ảnh kiểm kho: {ex.Message}", ex);
            throw;
        }
    }

    public int DeleteByStocktakingMasterId(Guid stocktakingMasterId)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("DeleteByStocktakingMasterId: Xóa hình ảnh, StocktakingMasterId={0}", stocktakingMasterId);

            var entities = context.StocktakingImages
                .Where(i => i.StocktakingMasterId == stocktakingMasterId)
                .ToList();

            var count = entities.Count;
            context.StocktakingImages.DeleteAllOnSubmit(entities);
            context.SubmitChanges();

            _logger.Info("DeleteByStocktakingMasterId: Xóa {0} hình ảnh thành công", count);
            return count;
        }
        catch (Exception ex)
        {
            _logger.Error($"DeleteByStocktakingMasterId: Lỗi xóa hình ảnh: {ex.Message}", ex);
            throw;
        }
    }

    public int DeleteByStocktakingDetailId(Guid stocktakingDetailId)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("DeleteByStocktakingDetailId: Xóa hình ảnh, StocktakingDetailId={0}", stocktakingDetailId);

            var entities = context.StocktakingImages
                .Where(i => i.StocktakingDetailId == stocktakingDetailId)
                .ToList();

            var count = entities.Count;
            context.StocktakingImages.DeleteAllOnSubmit(entities);
            context.SubmitChanges();

            _logger.Info("DeleteByStocktakingDetailId: Xóa {0} hình ảnh thành công", count);
            return count;
        }
        catch (Exception ex)
        {
            _logger.Error($"DeleteByStocktakingDetailId: Lỗi xóa hình ảnh: {ex.Message}", ex);
            throw;
        }
    }

    #endregion
}
