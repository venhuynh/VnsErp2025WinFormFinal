using Dal.DataAccess.Interfaces.Inventory.InventoryManagement;
using Dal.DataContext;
using Logger;
using Logger.Configuration;
using System;
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
        loadOptions.LoadWith<Warranty>(w => w.StockInOutDetail);
        loadOptions.LoadWith<StockInOutDetail>(d => d.ProductVariant);
        loadOptions.LoadWith<ProductVariant>(v => v.ProductService);
        context.LoadOptions = loadOptions;

        return context;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Lấy VocherNumber từ StockInOutMaster theo ID
    /// </summary>
    /// <param name="id">ID của StockInOutMaster</param>
    /// <returns>VocherNumber hoặc null nếu không tìm thấy</returns>
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
            _logger.Error($"GetVocherNumber: Lỗi lấy VocherNumber cho ID={id}: {ex.Message}", ex);
            throw;
        }
    }

    #endregion
}