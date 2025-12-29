using Dal.Connection;
using Dal.DataAccess.Implementations.Inventory.StockIn;
using Dal.DataAccess.Interfaces.Inventory.StockIn;
using Dal.DtoConverter.Inventory;
using DTO.Inventory;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;
using System;

namespace Bll.Inventory.StockInOut
{
    /// <summary>
    /// Business Logic Layer cho StockIn (Phiếu nhập kho)
    /// </summary>
    public class StockInOutBll
    {
        #region Fields

        private IStockInRepository _dataAccess;
        private readonly ILogger _logger;
        private readonly object _lockObject = new object();

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor mặc định
        /// </summary>
        public StockInOutBll()
        {
            _logger = LoggerFactory.CreateLogger(LogCategory.BLL);
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Lấy hoặc khởi tạo Repository (lazy initialization)
        /// </summary>
        private IStockInRepository GetDataAccess()
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

                            _dataAccess = new StockInRepository(globalConnectionString);
                        }
                        catch (Exception ex)
                        {
                            _logger.Error($"Lỗi khởi tạo StockInRepository: {ex.Message}", ex);
                            throw;
                        }
                    }
                }
            }

            return _dataAccess;
        }

        #endregion

        #region UpSert



        #endregion

        #region Retrieve

        /// <summary>
        /// Lấy thông tin master phiếu nhập/xuất kho theo ID và chuyển đổi sang DTO
        /// </summary>
        /// <param name="stockInOutMasterId">ID phiếu nhập/xuất kho</param>
        /// <returns>StockInOutMasterForUIDto nếu tìm thấy, null nếu không tìm thấy</returns>
        public StockInOutMasterForUIDto GetStockInOutMasterForUIDtoById(Guid stockInOutMasterId)
        {
            try
            {
                if (stockInOutMasterId == Guid.Empty)
                {
                    _logger?.Warning("GetMasterById: stockInOutMasterId là Guid.Empty");
                    return null;
                }

                // Lấy entity từ repository
                return GetDataAccess().GetStockInOutMasterForUIDtoById(stockInOutMasterId);
                
            }
            catch (Exception ex)
            {
                _logger?.Error($"GetMasterById: Lỗi lấy thông tin master: {ex.Message}", ex);
                throw;
            }
        }

        #endregion


    }
}
