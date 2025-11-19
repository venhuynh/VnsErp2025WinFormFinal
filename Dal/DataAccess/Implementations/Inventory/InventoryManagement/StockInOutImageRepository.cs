using Dal.DataAccess.Interfaces.Inventory.InventoryManagement;
using Dal.DataContext;
using Logger;
using Logger.Configuration;
using System;
using System.Data.Linq;
using System.Linq;
using CustomLogger = Logger.Interfaces.ILogger;
namespace Dal.DataAccess.Implementations.Inventory.InventoryManagement;

public class StockInOutImageRepository : IStockInOutImageRepository
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
    /// Khởi tạo một instance mới của class StockInRepository.
    /// </summary>
    /// <param name="connectionString">Chuỗi kết nối cơ sở dữ liệu</param>
    /// <exception cref="ArgumentNullException">Ném ra khi connectionString là null</exception>
    public StockInOutImageRepository(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _logger = LoggerFactory.CreateLogger(LogCategory.DAL);
        _logger.Info("StockInOutImageRepository được khởi tạo với connection string");
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
        context.LoadOptions = loadOptions;

        return context;
    }

    #endregion

    #region Save Operations

    /// <summary>
    /// Lưu hoặc cập nhật hình ảnh nhập/xuất kho
    /// </summary>
    /// <param name="stockInOutImage">Entity hình ảnh cần lưu</param>
    public void SaveOrUpdate(StockInOutImage stockInOutImage)
    {
        if (stockInOutImage == null)
            throw new ArgumentNullException(nameof(stockInOutImage));

        using var context = CreateNewContext();
        try
        {
            _logger.Debug("SaveOrUpdate: Bắt đầu lưu hình ảnh, Id={0}, StockInOutMasterId={1}", 
                stockInOutImage.Id, stockInOutImage.StockInOutMasterId);

            var existing = stockInOutImage.Id != Guid.Empty ? 
                context.StockInOutImages.FirstOrDefault(x => x.Id == stockInOutImage.Id) : null;

            if (existing == null)
            {
                // Thêm mới
                if (stockInOutImage.Id == Guid.Empty)
                    stockInOutImage.Id = Guid.NewGuid();
                
                // Thiết lập giá trị mặc định
                if (stockInOutImage.CreateDate == default(DateTime))
                    stockInOutImage.CreateDate = DateTime.Now;

                context.StockInOutImages.InsertOnSubmit(stockInOutImage);
                context.SubmitChanges();
                
                _logger.Info("SaveOrUpdate: Đã thêm mới hình ảnh, Id={0}", stockInOutImage.Id);
            }
            else
            {
                // Cập nhật
                existing.StockInOutMasterId = stockInOutImage.StockInOutMasterId;
                existing.ImageData = stockInOutImage.ImageData;
                existing.ModifiedDate = DateTime.Now;
                existing.ModifiedBy = stockInOutImage.ModifiedBy;
                
                context.SubmitChanges();
                
                _logger.Info("SaveOrUpdate: Đã cập nhật hình ảnh, Id={0}", existing.Id);
            }
        }
        catch (Exception ex)
        {
            _logger.Error($"SaveOrUpdate: Lỗi lưu hình ảnh: {ex.Message}", ex);
            throw;
        }
    }

    #endregion

}