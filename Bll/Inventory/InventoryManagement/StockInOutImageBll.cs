using Dal.Connection;
using Dal.DataAccess.Implementations.Inventory.InventoryManagement;
using Dal.DataAccess.Interfaces.Inventory.InventoryManagement;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;
using System;

namespace Bll.Inventory.InventoryManagement;

public class StockInOutImageBll
{
    #region Fields

    private IStockInOutImageRepository _dataAccess;
    private readonly ILogger _logger;
    private readonly object _lockObject = new object();

    #endregion

    #region Constructors

    /// <summary>
    /// Constructor mặc định
    /// </summary>
    public StockInOutImageBll()
    {
        _logger = LoggerFactory.CreateLogger(LogCategory.BLL);
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Lấy hoặc khởi tạo Repository (lazy initialization)
    /// </summary>
    private IStockInOutImageRepository GetDataAccess()
    {
        if (_dataAccess == null)
        {
            lock (_lockObject)
            {
                if (_dataAccess == null)
                {
                    try
                    {
                        // Sử dụng global connection string từ ApplicationStartupManager
                        var globalConnectionString = ApplicationStartupManager.Instance.GetGlobalConnectionString();
                        if (string.IsNullOrEmpty(globalConnectionString))
                        {
                            throw new InvalidOperationException(
                                "Không có global connection string. Ứng dụng chưa được khởi tạo hoặc chưa sẵn sàng.");
                        }

                        _dataAccess = new StockInOutImageRepository(globalConnectionString);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"Lỗi khởi tạo StockInOutImageRepository: {ex.Message}", ex);
                        throw;
                    }
                }
            }
        }

        return _dataAccess;
    }

    #endregion

    #region Business Methods

    /// <summary>
    /// Lưu hoặc cập nhật hình ảnh nhập/xuất kho
    /// </summary>
    /// <param name="stockInOutImage">Entity hình ảnh cần lưu</param>
    public void SaveOrUpdate(Dal.DataContext.StockInOutImage stockInOutImage)
    {
        try
        {
            if (stockInOutImage == null)
                throw new ArgumentNullException(nameof(stockInOutImage));

            _logger.Debug("SaveOrUpdate: Bắt đầu lưu hình ảnh, Id={0}, StockInOutMasterId={1}", 
                stockInOutImage.Id, stockInOutImage.StockInOutMasterId);

            GetDataAccess().SaveOrUpdate(stockInOutImage);

            _logger.Info("SaveOrUpdate: Lưu hình ảnh thành công, Id={0}", stockInOutImage.Id);
        }
        catch (Exception ex)
        {
            _logger.Error($"SaveOrUpdate: Lỗi lưu hình ảnh: {ex.Message}", ex);
            throw;
        }
    }

    #endregion
}