using Dal.Connection;
using Dal.DataAccess.Implementations.Inventory.Assembly;
using Dal.DataAccess.Interfaces.Inventory.Assembly;
using Dal.DataContext;
using DTO.Inventory.Assembly;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bll.Inventory.Assembly;

/// <summary>
/// Business Logic Layer cho ProductBOM (Bill of Materials)
/// </summary>
public class ProductBOMBll
{
    #region Fields

    private IProductBOMRepository _dataAccess;
    private readonly ILogger _logger;
    private readonly object _lockObject = new object();

    #endregion

    #region Constructors

    public ProductBOMBll()
    {
        _logger = LoggerFactory.CreateLogger(LogCategory.BLL);
    }

    #endregion

    #region Helper Methods

    private IProductBOMRepository GetDataAccess()
    {
        if (_dataAccess == null)
        {
            lock (_lockObject)
            {
                if (_dataAccess == null)
                {
                    var connectionString = ConnectionStringManager.GetConnectionString();
                    _dataAccess = new ProductBOMRepository(connectionString);
                }
            }
        }
        return _dataAccess;
    }

    #endregion

    #region CRUD Operations

    /// <summary>
    /// Lưu hoặc cập nhật BOM
    /// </summary>
    public void SaveOrUpdate(ProductBOMDto dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto));

        if (!dto.IsValid())
            throw new ArgumentException("Dữ liệu BOM không hợp lệ", nameof(dto));

        try
        {
            _logger.Debug("SaveOrUpdate: Bắt đầu lưu BOM, Id={0}, ProductVariantId={1}, ComponentVariantId={2}",
                dto.Id, dto.ProductVariantId, dto.ComponentVariantId);

            // Kiểm tra trùng lặp
            var exists = GetDataAccess().Exists(dto.ProductVariantId, dto.ComponentVariantId, 
                dto.Id != Guid.Empty ? dto.Id : (Guid?)null);
            if (exists)
            {
                throw new InvalidOperationException(
                    $"BOM đã tồn tại cho sản phẩm {dto.ProductVariantCode} và linh kiện {dto.ComponentVariantCode}");
            }

            // Kiểm tra sản phẩm không thể là linh kiện của chính nó
            if (dto.ProductVariantId == dto.ComponentVariantId)
            {
                throw new InvalidOperationException("Sản phẩm không thể là linh kiện của chính nó");
            }

            // Convert DTO to Entity
            var entity = MapDtoToEntity(dto);
            entity.ModifiedDate = DateTime.Now;

            GetDataAccess().SaveOrUpdate(entity);
            _logger.Info("SaveOrUpdate: Lưu BOM thành công, Id={0}", entity.Id);
        }
        catch (Exception ex)
        {
            _logger.Error($"SaveOrUpdate: Lỗi lưu BOM: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Xóa BOM
    /// </summary>
    public void Delete(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("ID không được để trống", nameof(id));

        try
        {
            _logger.Debug("Delete: Bắt đầu xóa BOM, Id={0}", id);
            GetDataAccess().Delete(id);
            _logger.Info("Delete: Xóa BOM thành công, Id={0}", id);
        }
        catch (Exception ex)
        {
            _logger.Error($"Delete: Lỗi xóa BOM: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Lấy BOM theo ID
    /// </summary>
    public ProductBOMDto GetById(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("ID không được để trống", nameof(id));

        try
        {
            var entity = GetDataAccess().GetById(id);
            if (entity == null)
                return null;

            return MapEntityToDto(entity);
        }
        catch (Exception ex)
        {
            _logger.Error($"GetById: Lỗi lấy BOM: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Lấy danh sách BOM theo ProductVariantId
    /// </summary>
    public List<ProductBOMDto> GetByProductVariantId(Guid productVariantId)
    {
        if (productVariantId == Guid.Empty)
            throw new ArgumentException("ProductVariantId không được để trống", nameof(productVariantId));

        try
        {
            var entities = GetDataAccess().GetByProductVariantId(productVariantId);
            return entities.Select(MapEntityToDto).ToList();
        }
        catch (Exception ex)
        {
            _logger.Error($"GetByProductVariantId: Lỗi lấy danh sách BOM: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Lấy tất cả BOM đang hoạt động
    /// </summary>
    public List<ProductBOMDto> GetAllActive()
    {
        try
        {
            var entities = GetDataAccess().GetAllActive();
            return entities.Select(MapEntityToDto).ToList();
        }
        catch (Exception ex)
        {
            _logger.Error($"GetAllActive: Lỗi lấy danh sách BOM: {ex.Message}", ex);
            throw;
        }
    }

    #endregion

    #region Mapping Methods

    private ProductBOM MapDtoToEntity(ProductBOMDto dto)
    {
        return new ProductBOM
        {
            Id = dto.Id == Guid.Empty ? Guid.NewGuid() : dto.Id,
            ProductVariantId = dto.ProductVariantId,
            ComponentVariantId = dto.ComponentVariantId,
            Quantity = dto.Quantity,
            UnitId = dto.UnitId,
            Notes = dto.Notes,
            IsActive = dto.IsActive,
            CreatedDate = dto.CreatedDate == default(DateTime) ? DateTime.Now : dto.CreatedDate,
            ModifiedDate = DateTime.Now
        };
    }

    private ProductBOMDto MapEntityToDto(ProductBOM entity)
    {
        return new ProductBOMDto
        {
            Id = entity.Id,
            ProductVariantId = entity.ProductVariantId,
            ComponentVariantId = entity.ComponentVariantId,
            Quantity = entity.Quantity,
            UnitId = entity.UnitId,
            Notes = entity.Notes,
            IsActive = entity.IsActive,
            CreatedDate = entity.CreatedDate,
            ModifiedDate = entity.ModifiedDate,
            // Load navigation properties if available
            ProductVariantCode = entity.ProductVariant?.VariantCode ?? string.Empty,
            ProductVariantName = entity.ProductVariant?.VariantFullName ?? entity.ProductVariant?.ProductService?.Name ?? string.Empty,
            ComponentVariantCode = entity.ComponentVariant?.VariantCode ?? string.Empty,
            ComponentVariantName = entity.ComponentVariant?.VariantFullName ?? entity.ComponentVariant?.ProductService?.Name ?? string.Empty,
            UnitCode = entity.UnitOfMeasure?.Code ?? string.Empty,
            UnitName = entity.UnitOfMeasure?.Name ?? string.Empty
        };
    }

    #endregion
}

