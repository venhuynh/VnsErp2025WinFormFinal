using Dal.DataAccess.Interfaces.Inventory.Assembly;
using Dal.DataContext;
using Logger;
using Logger.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using CustomLogger = Logger.Interfaces.ILogger;

namespace Dal.DataAccess.Implementations.Inventory.Assembly;

/// <summary>
/// Repository implementation cho ProductBOM (Bill of Materials)
/// </summary>
public class ProductBOMRepository : IProductBOMRepository
{
    #region Private Fields

    private readonly string _connectionString;
    private readonly CustomLogger _logger;

    #endregion

    #region Constructor

    public ProductBOMRepository(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _logger = LoggerFactory.CreateLogger(LogCategory.DAL);
        _logger.Info("ProductBOMRepository được khởi tạo");
    }

    #endregion

    #region Helper Methods

    private VnsErp2025DataContext CreateNewContext()
    {
        var context = new VnsErp2025DataContext(_connectionString);

        var loadOptions = new DataLoadOptions();
        loadOptions.LoadWith<ProductBOM>(b => b.ProductVariant1); // ProductVariant (sản phẩm hoàn chỉnh)
        loadOptions.LoadWith<ProductBOM>(b => b.ProductVariant); // ComponentVariant (linh kiện)
        loadOptions.LoadWith<ProductVariant>(v => v.ProductService);
        loadOptions.LoadWith<ProductBOM>(b => b.UnitOfMeasure);
        context.LoadOptions = loadOptions;

        return context;
    }

    #endregion

    #region Save Operations

    public void SaveOrUpdate(ProductBOM bom)
    {
        if (bom == null)
            throw new ArgumentNullException(nameof(bom));

        using var context = CreateNewContext();
        try
        {
            _logger.Debug("SaveOrUpdate: Bắt đầu lưu BOM, Id={0}, ProductVariantId={1}, ComponentVariantId={2}",
                bom.Id, bom.ProductVariantId, bom.ComponentVariantId);

            var existing = bom.Id != Guid.Empty ?
                context.ProductBOMs.FirstOrDefault(x => x.Id == bom.Id) : null;

            if (existing == null)
            {
                // Thêm mới
                if (bom.Id == Guid.Empty)
                    bom.Id = Guid.NewGuid();

                bom.CreatedDate = DateTime.Now;
                bom.ModifiedDate = DateTime.Now;

                context.ProductBOMs.InsertOnSubmit(bom);
                context.SubmitChanges();

                _logger.Info("SaveOrUpdate: Đã thêm mới BOM, Id={0}", bom.Id);
            }
            else
            {
                // Cập nhật
                existing.ProductVariantId = bom.ProductVariantId;
                existing.ComponentVariantId = bom.ComponentVariantId;
                existing.Quantity = bom.Quantity;
                existing.UnitId = bom.UnitId;
                existing.Notes = bom.Notes;
                existing.IsActive = bom.IsActive;
                existing.ModifiedDate = DateTime.Now;

                context.SubmitChanges();

                _logger.Info("SaveOrUpdate: Đã cập nhật BOM, Id={0}", existing.Id);
            }
        }
        catch (Exception ex)
        {
            _logger.Error($"SaveOrUpdate: Lỗi lưu BOM: {ex.Message}", ex);
            throw;
        }
    }

    public void Delete(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("ID không được để trống", nameof(id));

        using var context = CreateNewContext();
        try
        {
            _logger.Debug("Delete: Bắt đầu xóa BOM, Id={0}", id);

            var bom = context.ProductBOMs.FirstOrDefault(x => x.Id == id);
            if (bom == null)
            {
                _logger.Warning("Delete: Không tìm thấy BOM với Id={0}", id);
                return;
            }

            context.ProductBOMs.DeleteOnSubmit(bom);
            context.SubmitChanges();

            _logger.Info("Delete: Đã xóa BOM, Id={0}", id);
        }
        catch (Exception ex)
        {
            _logger.Error($"Delete: Lỗi xóa BOM: {ex.Message}", ex);
            throw;
        }
    }

    #endregion

    #region Query Operations

    public ProductBOM GetById(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("ID không được để trống", nameof(id));

        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetById: Lấy BOM, Id={0}", id);

            var bom = context.ProductBOMs.FirstOrDefault(x => x.Id == id);
            return bom;
        }
        catch (Exception ex)
        {
            _logger.Error($"GetById: Lỗi lấy BOM: {ex.Message}", ex);
            throw;
        }
    }

    public List<ProductBOM> GetByProductVariantId(Guid productVariantId)
    {
        if (productVariantId == Guid.Empty)
            throw new ArgumentException("ProductVariantId không được để trống", nameof(productVariantId));

        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetByProductVariantId: Lấy danh sách BOM, ProductVariantId={0}", productVariantId);

            var boms = context.ProductBOMs
                .Where(x => x.ProductVariantId == productVariantId && x.IsActive)
                .ToList();

            _logger.Info("GetByProductVariantId: Đã lấy {0} BOM", boms.Count);
            return boms;
        }
        catch (Exception ex)
        {
            _logger.Error($"GetByProductVariantId: Lỗi lấy danh sách BOM: {ex.Message}", ex);
            throw;
        }
    }

    public List<ProductBOM> GetAllActive()
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetAllActive: Lấy tất cả BOM đang hoạt động");

            var boms = context.ProductBOMs
                .Where(x => x.IsActive)
                .ToList();

            _logger.Info("GetAllActive: Đã lấy {0} BOM", boms.Count);
            return boms;
        }
        catch (Exception ex)
        {
            _logger.Error($"GetAllActive: Lỗi lấy danh sách BOM: {ex.Message}", ex);
            throw;
        }
    }

    public bool Exists(Guid productVariantId, Guid componentVariantId, Guid? excludeId = null)
    {
        if (productVariantId == Guid.Empty || componentVariantId == Guid.Empty)
            return false;

        using var context = CreateNewContext();
        try
        {
            var query = context.ProductBOMs
                .Where(x => x.ProductVariantId == productVariantId 
                    && x.ComponentVariantId == componentVariantId);

            if (excludeId.HasValue && excludeId.Value != Guid.Empty)
            {
                query = query.Where(x => x.Id != excludeId.Value);
            }

            return query.Any();
        }
        catch (Exception ex)
        {
            _logger.Error($"Exists: Lỗi kiểm tra BOM: {ex.Message}", ex);
            throw;
        }
    }

    #endregion
}

