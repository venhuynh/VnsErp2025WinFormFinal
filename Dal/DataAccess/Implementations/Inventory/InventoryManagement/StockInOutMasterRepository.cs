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

public class StockInOutMasterRepository : IStockInOutMasterRepository
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
    public StockInOutMasterRepository(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _logger = LoggerFactory.CreateLogger(LogCategory.DAL);
        _logger.Info("StockInOutMasterRepository được khởi tạo với connection string");
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
        loadOptions.LoadWith<Warranty>(w => w.Device);
        loadOptions.LoadWith<Device>(d => d.StockInOutDetail);
        loadOptions.LoadWith<StockInOutDetail>(d => d.ProductVariant);
        loadOptions.LoadWith<ProductVariant>(v => v.ProductService);
        context.LoadOptions = loadOptions;

        return context;
    }

    /// <summary>
    /// Tạo DataContext mới với eager loading cho StockInOutMaster
    /// </summary>
    /// <returns>DataContext mới</returns>
    private VnsErp2025DataContext CreateNewContextForMaster()
    {
        var context = new VnsErp2025DataContext(_connectionString);

        // Configure eager loading cho navigation properties của StockInOutMaster
        var loadOptions = new DataLoadOptions();
        loadOptions.LoadWith<StockInOutMaster>(m => m.CompanyBranch);
        loadOptions.LoadWith<StockInOutMaster>(m => m.BusinessPartnerSite);
        loadOptions.LoadWith<BusinessPartnerSite>(s => s.BusinessPartner);
        context.LoadOptions = loadOptions;

        return context;
    }

    /// <summary>
    /// Tạo DataContext mới với eager loading cho StockInOutMaster và StockInOutDetail
    /// </summary>
    /// <returns>DataContext mới</returns>
    private VnsErp2025DataContext CreateNewContextForMasterWithDetails()
    {
        var context = new VnsErp2025DataContext(_connectionString);

        // Configure eager loading cho navigation properties
        var loadOptions = new DataLoadOptions();
        
        // Load navigation properties cho StockInOutMaster
        loadOptions.LoadWith<StockInOutMaster>(m => m.CompanyBranch);
        loadOptions.LoadWith<StockInOutMaster>(m => m.BusinessPartnerSite);
        loadOptions.LoadWith<BusinessPartnerSite>(s => s.BusinessPartner);
        loadOptions.LoadWith<StockInOutMaster>(m => m.StockInOutDetails);
        
        // Load navigation properties cho StockInOutDetail
        loadOptions.LoadWith<StockInOutDetail>(d => d.ProductVariant);
        loadOptions.LoadWith<ProductVariant>(v => v.ProductService);
        loadOptions.LoadWith<ProductVariant>(v => v.UnitOfMeasure);
        
        context.LoadOptions = loadOptions;

        return context;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Lấy VoucherNumber từ StockInOutMaster theo ID
    /// </summary>
    /// <param name="id">ID của StockInOutMaster</param>
    /// <returns>VoucherNumber hoặc null nếu không tìm thấy</returns>
    public string GetVocherNumber(Guid id)
    {
        using var context = CreateNewContext();
        try
        {
            var master = context.StockInOutMasters.FirstOrDefault(m => m.Id == id);
            return master?.VocherNumber;
        }
        catch (Exception ex)
        {
            _logger.Error($"GetVocherNumber: Lỗi lấy VoucherNumber cho ID={id}: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Lấy danh sách StockInOutMaster theo danh sách ID
    /// </summary>
    /// <param name="masterIds">Danh sách ID của StockInOutMaster</param>
    /// <returns>Danh sách StockInOutMaster entities với navigation properties đã load</returns>
    public List<StockInOutMaster> GetMastersByIds(List<Guid> masterIds)
    {
        using var context = CreateNewContextForMaster();
        try
        {
            if (masterIds == null || masterIds.Count == 0)
            {
                _logger.Debug("GetMastersByIds: Danh sách MasterIds rỗng");
                return new List<StockInOutMaster>();
            }

            _logger.Debug("GetMastersByIds: Bắt đầu query, MasterIds count={0}", masterIds.Count);

            var masters = context.StockInOutMasters
                .Where(m => masterIds.Contains(m.Id))
                .OrderByDescending(m => m.StockInOutDate)
                .ThenByDescending(m => m.VocherNumber ?? string.Empty)
                .ToList();

            _logger.Info("GetMastersByIds: Query thành công, ResultCount={0}", masters.Count);
            return masters;
        }
        catch (Exception ex)
        {
            _logger.Error($"GetMastersByIds: Lỗi query: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Lấy StockInOutMaster theo ID với tất cả navigation properties bao gồm cả details
    /// </summary>
    /// <param name="id">ID của StockInOutMaster</param>
    /// <returns>StockInOutMaster entity với tất cả navigation properties đã load hoặc null nếu không tìm thấy</returns>
    public StockInOutMaster GetMasterByIdWithDetails(Guid id)
    {
        using var context = CreateNewContextForMasterWithDetails();
        try
        {
            _logger.Debug("GetMasterByIdWithDetails: Bắt đầu query, Id={0}", id);

            var master = context.StockInOutMasters.FirstOrDefault(m => m.Id == id);

            if (master == null)
            {
                _logger.Warning("GetMasterByIdWithDetails: Không tìm thấy StockInOutMaster với Id={0}", id);
                return null;
            }

            _logger.Info("GetMasterByIdWithDetails: Query thành công, Id={0}, DetailCount={1}", 
                id, master.StockInOutDetails?.Count ?? 0);
            return master;
        }
        catch (Exception ex)
        {
            _logger.Error($"GetMasterByIdWithDetails: Lỗi query: {ex.Message}", ex);
            throw;
        }
    }

    #endregion
}