using Dal.Connection;
using Dal.DataAccess.Implementations.Inventory.InventoryManagement;
using Dal.DataAccess.Implementations.Inventory.StockIn;
using Dal.DataAccess.Interfaces.Inventory.InventoryManagement;
using Dal.DataContext;
using Logger;
using Logger.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logger.Interfaces;

namespace Bll.Inventory.InventoryManagement;

/// <summary>
/// Business Logic Layer cho StockInOutMaster (Phiếu nhập xuất kho)
/// </summary>
public class StockInOutMasterBll
{
    #region Fields

    private IStockInOutMasterRepository _dataAccess;
    private Dal.DataAccess.Interfaces.Inventory.StockIn.IStockInRepository _stockInRepository;
    private readonly ILogger _logger;
    private readonly object _lockObject = new object();
    private readonly object _stockInRepositoryLock = new object();

    #endregion

    #region Constructors

    /// <summary>
    /// Constructor mặc định
    /// </summary>
    public StockInOutMasterBll()
    {
        _logger = LoggerFactory.CreateLogger(LogCategory.BLL);
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Lấy hoặc khởi tạo Repository (lazy initialization)
    /// </summary>
    private IStockInOutMasterRepository GetDataAccess()
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

                        _dataAccess = new StockInOutMasterRepository(globalConnectionString);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"Lỗi khởi tạo StockInOutMasterRepository: {ex.Message}", ex);
                        throw;
                    }
                }
            }
        }

        return _dataAccess;
    }

    /// <summary>
    /// Lấy hoặc khởi tạo StockInRepository (lazy initialization)
    /// Sử dụng để query history vì method QueryHistory nằm trong StockInRepository
    /// </summary>
    private Dal.DataAccess.Interfaces.Inventory.StockIn.IStockInRepository GetStockInRepository()
    {
        if (_stockInRepository == null)
        {
            lock (_stockInRepositoryLock)
            {
                if (_stockInRepository == null)
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

                        _stockInRepository = new StockInRepository(globalConnectionString);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"Lỗi khởi tạo StockInRepository: {ex.Message}", ex);
                        throw;
                    }
                }
            }
        }

        return _stockInRepository;
    }

    #endregion

    #region Query Operations

    /// <summary>
    /// Query lịch sử nhập xuất kho với filter
    /// </summary>
    /// <param name="query">Query criteria</param>
    /// <returns>Danh sách StockInOutMaster entities</returns>
    public List<StockInOutMaster> QueryHistory(Dal.DataAccess.Implementations.Inventory.StockIn.StockInHistoryQueryCriteria query)
    {
        try
        {
            _logger.Debug("QueryHistory: Bắt đầu query lịch sử, FromDate={0}, ToDate={1}", query.FromDate, query.ToDate);

            var entities = GetStockInRepository().QueryHistory(query);

            _logger.Info("QueryHistory: Query thành công, ResultCount={0}", entities.Count);
            return entities;
        }
        catch (Exception ex)
        {
            _logger.Error($"QueryHistory: Lỗi query lịch sử: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Đếm số lượng bản ghi theo query (không phân trang)
    /// </summary>
    /// <param name="query">Query criteria</param>
    /// <returns>Tổng số bản ghi</returns>
    public int CountHistory(Dal.DataAccess.Implementations.Inventory.StockIn.StockInHistoryQueryCriteria query)
    {
        try
        {
            _logger.Debug("CountHistory: Bắt đầu đếm, FromDate={0}, ToDate={1}", query.FromDate, query.ToDate);

            var count = GetStockInRepository().CountHistory(query);

            _logger.Info("CountHistory: Đếm thành công, Count={0}", count);
            return count;
        }
        catch (Exception ex)
        {
            _logger.Error($"CountHistory: Lỗi đếm: {ex.Message}", ex);
            throw;
        }
    }

    #endregion
}