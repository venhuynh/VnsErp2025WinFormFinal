using Dal.Connection;
using Dal.DataAccess.Implementations.Inventory.InventoryManagement;
using Dal.DataAccess.Interfaces.Inventory.InventoryManagement;
using Dal.DataContext;
using DTO.Inventory.InventoryManagement;
using DTO.Inventory.Report;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Bll.Inventory.InventoryManagement;

/// <summary>
/// Business Logic Layer cho StockInOutReport (Báo cáo phiếu nhập xuất kho)
/// </summary>
public class StockInOutReportBll
{
    #region Fields

    private IStockInOutMasterRepository _dataAccess;
    private readonly ILogger _logger;
    private readonly object _lockObject = new();

    #endregion

    #region Constructors

    /// <summary>
    /// Constructor mặc định
    /// </summary>
    public StockInOutReportBll()
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
    /// Lấy Description từ enum value
    /// </summary>
    /// <typeparam name="T">Kiểu enum</typeparam>
    /// <param name="enumValue">Giá trị enum</param>
    /// <returns>Description hoặc tên enum nếu không có Description</returns>
    private static string GetEnumDescription<T>(T enumValue) where T : Enum
    {
        var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
        if (fieldInfo == null) return enumValue.ToString();

        var descriptionAttribute = fieldInfo.GetCustomAttribute<System.ComponentModel.DescriptionAttribute>();
        return descriptionAttribute?.Description ?? enumValue.ToString();
    }

    #endregion

    #region Query Operations

    /// <summary>
    /// Lấy thông tin đầy đủ của StockInOutReportDto dựa vào ID của StockInOutMaster
    /// Bao gồm cả master data và detail data với tất cả navigation properties
    /// </summary>
    /// <param name="masterId">ID của StockInOutMaster</param>
    /// <returns>StockInOutReportDto đầy đủ thông tin hoặc null nếu không tìm thấy</returns>
    public StockInOutReportDto GetReportDtoById(Guid masterId)
    {
        try
        {
            _logger.Debug("GetReportDtoById: Bắt đầu lấy thông tin report, MasterId={0}", masterId);

            // Lấy StockInOutMaster với tất cả navigation properties từ repository
            return GetDataAccess().GetStockInOutReportDtoByMasterId(masterId);
            
        }
        catch (Exception ex)
        {
            _logger.Error($"GetReportDtoById: Lỗi lấy thông tin report: {ex.Message}", ex);
            throw;
        }
    }

    #endregion

    
}
