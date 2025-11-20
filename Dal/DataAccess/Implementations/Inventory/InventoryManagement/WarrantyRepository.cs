using Dal.DataAccess.Interfaces.Inventory.InventoryManagement;
using Dal.DataContext;
using Logger;
using Logger.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using CustomLogger = Logger.Interfaces.ILogger;

namespace Dal.DataAccess.Implementations.Inventory.InventoryManagement;

public class WarrantyRepository : IWarrantyRepository
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
    /// Khởi tạo một instance mới của class WarrantyRepository.
    /// </summary>
    /// <param name="connectionString">Chuỗi kết nối cơ sở dữ liệu</param>
    /// <exception cref="ArgumentNullException">Ném ra khi connectionString là null</exception>
    public WarrantyRepository(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _logger = LoggerFactory.CreateLogger(LogCategory.DAL);
        _logger.Info("WarrantyRepository được khởi tạo với connection string");
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
        loadOptions.LoadWith<Warranty>(w => w.StockInOutDetail);
        loadOptions.LoadWith<StockInOutDetail>(d => d.ProductVariant);
        loadOptions.LoadWith<ProductVariant>(v => v.ProductService);
        context.LoadOptions = loadOptions;

        return context;
    }

    #endregion

    #region Query Operations

    /// <summary>
    /// Lấy danh sách bảo hành theo StockInOutMasterId
    /// </summary>
    /// <param name="stockInOutMasterId">ID phiếu nhập/xuất kho</param>
    /// <returns>Danh sách Warranty entities</returns>
    public List<Warranty> GetByStockInOutMasterId(Guid stockInOutMasterId)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetByStockInOutMasterId: Lấy danh sách bảo hành, StockInOutMasterId={0}", stockInOutMasterId);

            var warranties = (from w in context.Warranties
                             join d in context.StockInOutDetails on w.StockInOutDetailId equals d.Id
                             where d.StockInOutMasterId == stockInOutMasterId
                             select w).ToList();

            _logger.Info("GetByStockInOutMasterId: Lấy được {0} bảo hành", warranties.Count);
            return warranties;
        }
        catch (Exception ex)
        {
            _logger.Error($"GetByStockInOutMasterId: Lỗi lấy danh sách bảo hành: {ex.Message}", ex);
            throw;
        }
    }

    #endregion

    #region Save Operations

    /// <summary>
    /// Lưu hoặc cập nhật bảo hành
    /// </summary>
    /// <param name="warranty">Entity bảo hành cần lưu</param>
    public void SaveOrUpdate(Warranty warranty)
    {
        if (warranty == null)
            throw new ArgumentNullException(nameof(warranty));

        using var context = CreateNewContext();
        try
        {
            _logger.Debug("SaveOrUpdate: Bắt đầu lưu bảo hành, Id={0}, StockInOutDetailId={1}", 
                warranty.Id, warranty.StockInOutDetailId);

            var existing = warranty.Id != Guid.Empty ? 
                context.Warranties.FirstOrDefault(x => x.Id == warranty.Id) : null;

            if (existing == null)
            {
                // Thêm mới
                if (warranty.Id == Guid.Empty)
                    warranty.Id = Guid.NewGuid();
                
                context.Warranties.InsertOnSubmit(warranty);
                context.SubmitChanges();
                
                _logger.Info("SaveOrUpdate: Đã thêm mới bảo hành, Id={0}", warranty.Id);
            }
            else
            {
                // Cập nhật
                existing.StockInOutDetailId = warranty.StockInOutDetailId;
                existing.WarrantyType = warranty.WarrantyType;
                existing.WarrantyFrom = warranty.WarrantyFrom;
                existing.MonthOfWarranty = warranty.MonthOfWarranty;
                existing.WarrantyUntil = warranty.WarrantyUntil;
                existing.WarrantyStatus = warranty.WarrantyStatus;
                existing.UniqueProductInfo = warranty.UniqueProductInfo;
                
                context.SubmitChanges();
                
                _logger.Info("SaveOrUpdate: Đã cập nhật bảo hành, Id={0}", existing.Id);
            }
        }
        catch (Exception ex)
        {
            _logger.Error($"SaveOrUpdate: Lỗi lưu bảo hành: {ex.Message}", ex);
            throw;
        }
    }

    #endregion

    #region Delete Operations

    /// <summary>
    /// Xóa bảo hành
    /// </summary>
    /// <param name="warrantyId">ID bảo hành cần xóa</param>
    public void Delete(Guid warrantyId)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("Delete: Bắt đầu xóa bảo hành, Id={0}", warrantyId);

            var warranty = context.Warranties.FirstOrDefault(x => x.Id == warrantyId);
            if (warranty == null)
            {
                _logger.Warning("Delete: Không tìm thấy bảo hành với Id={0}", warrantyId);
                return;
            }

            context.Warranties.DeleteOnSubmit(warranty);
            context.SubmitChanges();
            
            _logger.Info("Delete: Đã xóa bảo hành, Id={0}", warrantyId);
        }
        catch (Exception ex)
        {
            _logger.Error($"Delete: Lỗi xóa bảo hành: {ex.Message}", ex);
            throw;
        }
    }

    #endregion
}