using Dal.DataAccess.Interfaces.Inventory.InventoryManagement;
using Dal.DataContext;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;
using System;
using System.Collections.Generic;
using Dal.Connection;
using Dal.DataAccess.Implementations.Inventory.InventoryManagement;

namespace Bll.Inventory.InventoryManagement;

/// <summary>
/// Business Logic Layer cho StockInOutProductHistory (Lịch sử sản phẩm nhập xuất kho)
/// </summary>
public class StockInOutDetailBll
{
    #region Fields

    private IStockInOutDetailRepository _stockInOutDetailRepository;
    private readonly ILogger _logger;
    private readonly object _stockInOutDetailRepositoryLock = new object();

    #endregion

    #region Constructors

    /// <summary>
    /// Constructor mặc định
    /// </summary>
    public StockInOutDetailBll()
    {
        _logger = LoggerFactory.CreateLogger(LogCategory.BLL);
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Lấy hoặc khởi tạo StockInOutDetailRepository (lazy initialization)
    /// </summary>
    private IStockInOutDetailRepository GetStockInOutDetailRepository()
    {
        if (_stockInOutDetailRepository == null)
        {
            lock (_stockInOutDetailRepositoryLock)
            {
                if (_stockInOutDetailRepository == null)
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

                        _stockInOutDetailRepository = new StockInOutDetailRepository(globalConnectionString);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"Lỗi khởi tạo StockInOutDetailRepository: {ex.Message}", ex);
                        throw;
                    }
                }
            }
        }

        return _stockInOutDetailRepository;
    }

    #endregion

    #region Query Operations

    /// <summary>
    /// Query lịch sử sản phẩm nhập xuất kho với filter
    /// Tìm kiếm trong StockInOutDetail và các bảng liên quan (ProductVariant, ProductService)
    /// </summary>
    /// <param name="fromDate">Từ ngày</param>
    /// <param name="toDate">Đến ngày</param>
    /// <param name="keyword">Từ khóa tìm kiếm (tìm trong mã hàng, tên hàng, số phiếu)</param>
    /// <returns>Danh sách StockInOutDetail entities</returns>
    public List<StockInOutDetail> QueryProductHistory(DateTime fromDate, DateTime toDate, string keyword = null)
    {
        try
        {
            _logger.Debug("QueryProductHistory: Bắt đầu query lịch sử sản phẩm, FromDate={0}, ToDate={1}, Keyword={2}", 
                fromDate, toDate, keyword ?? "null");

            // Gọi Repository để thực hiện query
            var result = GetStockInOutDetailRepository().QueryProductHistory(fromDate, toDate, keyword);

            _logger.Info("QueryProductHistory: Query thành công, ResultCount={0}", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.Error($"QueryProductHistory: Lỗi query lịch sử sản phẩm: {ex.Message}", ex);
            throw;
        }
    }

    #endregion
}

