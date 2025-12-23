using Dal.Connection;
using Dal.DataAccess.Implementations.Inventory.InventoryManagement;
using Dal.DataAccess.Interfaces.Inventory.InventoryManagement;
using Dal.DataContext;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;
using System;
using System.Collections.Generic;

namespace Bll.Inventory.InventoryManagement
{
    /// <summary>
    /// Business Logic Layer cho DeviceTransactionHistory (Lịch sử giao dịch thiết bị)
    /// </summary>
    public class DeviceTransactionHistoryBll
    {
        #region Fields

        private IDeviceTransactionHistoryRepository _deviceTransactionHistoryRepository;
        private readonly ILogger _logger;
        private readonly object _deviceTransactionHistoryRepositoryLock = new object();

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor mặc định
        /// </summary>
        public DeviceTransactionHistoryBll()
        {
            _logger = LoggerFactory.CreateLogger(LogCategory.BLL);
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Lấy hoặc khởi tạo DeviceTransactionHistoryRepository (lazy initialization)
        /// </summary>
        private IDeviceTransactionHistoryRepository GetDeviceTransactionHistoryRepository()
        {
            if (_deviceTransactionHistoryRepository == null)
            {
                lock (_deviceTransactionHistoryRepositoryLock)
                {
                    if (_deviceTransactionHistoryRepository == null)
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

                            _deviceTransactionHistoryRepository = new DeviceTransactionHistoryRepository(globalConnectionString);
                        }
                        catch (Exception ex)
                        {
                            _logger.Error($"Lỗi khởi tạo DeviceTransactionHistoryRepository: {ex.Message}", ex);
                            throw;
                        }
                    }
                }
            }

            return _deviceTransactionHistoryRepository;
        }

        #endregion

        #region Query Operations

        /// <summary>
        /// Lấy DeviceTransactionHistory theo ID
        /// </summary>
        /// <param name="id">ID của bản ghi lịch sử</param>
        /// <returns>DeviceTransactionHistory entity hoặc null</returns>
        public DeviceTransactionHistory GetById(Guid id)
        {
            try
            {
                _logger.Debug("GetById: Lấy lịch sử giao dịch, Id={0}", id);

                var result = GetDeviceTransactionHistoryRepository().GetById(id);

                if (result != null)
                {
                    _logger.Info("GetById: Lấy lịch sử giao dịch thành công, Id={0}", id);
                }
                else
                {
                    _logger.Warning("GetById: Không tìm thấy lịch sử giao dịch, Id={0}", id);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.Error($"GetById: Lỗi lấy lịch sử giao dịch: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Lấy danh sách lịch sử giao dịch theo DeviceId
        /// </summary>
        /// <param name="deviceId">ID thiết bị</param>
        /// <returns>Danh sách DeviceTransactionHistory entities, sắp xếp theo ngày mới nhất</returns>
        public List<DeviceTransactionHistory> GetByDeviceId(Guid deviceId)
        {
            try
            {
                _logger.Debug("GetByDeviceId: Lấy danh sách lịch sử giao dịch, DeviceId={0}", deviceId);

                var result = GetDeviceTransactionHistoryRepository().GetByDeviceId(deviceId);

                _logger.Info("GetByDeviceId: Lấy được {0} bản ghi lịch sử", result.Count);
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error($"GetByDeviceId: Lỗi lấy danh sách lịch sử giao dịch: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Lấy danh sách lịch sử giao dịch theo loại thao tác
        /// </summary>
        /// <param name="operationType">Loại thao tác (0=Import, 1=Export, 2=Allocation, 3=Recovery, 4=Transfer, 5=Maintenance, 6=StatusChange, 7=Other)</param>
        /// <returns>Danh sách DeviceTransactionHistory entities</returns>
        public List<DeviceTransactionHistory> GetByOperationType(int operationType)
        {
            try
            {
                _logger.Debug("GetByOperationType: Lấy danh sách lịch sử giao dịch, OperationType={0}", operationType);

                var result = GetDeviceTransactionHistoryRepository().GetByOperationType(operationType);

                _logger.Info("GetByOperationType: Lấy được {0} bản ghi lịch sử", result.Count);
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error($"GetByOperationType: Lỗi lấy danh sách lịch sử giao dịch: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Query lịch sử giao dịch với các filter
        /// </summary>
        /// <param name="deviceId">ID thiết bị (nullable)</param>
        /// <param name="operationType">Loại thao tác (nullable)</param>
        /// <param name="fromDate">Từ ngày (nullable)</param>
        /// <param name="toDate">Đến ngày (nullable)</param>
        /// <param name="referenceId">ID tham chiếu (nullable)</param>
        /// <param name="referenceType">Loại tham chiếu (nullable)</param>
        /// <param name="performedBy">Người thực hiện (nullable)</param>
        /// <param name="keyword">Từ khóa tìm kiếm trong Information (nullable)</param>
        /// <returns>Danh sách DeviceTransactionHistory entities, sắp xếp theo ngày mới nhất</returns>
        public List<DeviceTransactionHistory> Query(
            Guid? deviceId = null,
            int? operationType = null,
            DateTime? fromDate = null,
            DateTime? toDate = null,
            Guid? referenceId = null,
            int? referenceType = null,
            Guid? performedBy = null,
            string keyword = null)
        {
            try
            {
                _logger.Debug("Query: Bắt đầu query lịch sử giao dịch, DeviceId={0}, OperationType={1}, FromDate={2}, ToDate={3}, ReferenceId={4}, ReferenceType={5}, PerformedBy={6}, Keyword={7}",
                    deviceId, operationType, fromDate, toDate, referenceId, referenceType, performedBy, keyword ?? "null");

                var result = GetDeviceTransactionHistoryRepository().Query(
                    deviceId, operationType, fromDate, toDate, referenceId, referenceType, performedBy, keyword);

                _logger.Info("Query: Query thành công, ResultCount={0}", result.Count);
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error($"Query: Lỗi query lịch sử giao dịch: {ex.Message}", ex);
                throw;
            }
        }

        #endregion

        #region Save Operations

        /// <summary>
        /// Lưu hoặc cập nhật DeviceTransactionHistory
        /// </summary>
        /// <param name="history">DeviceTransactionHistory entity cần lưu</param>
        public void SaveOrUpdate(DeviceTransactionHistory history)
        {
            try
            {
                if (history == null)
                    throw new ArgumentNullException(nameof(history));

                _logger.Debug("SaveOrUpdate: Bắt đầu lưu lịch sử giao dịch, Id={0}, DeviceId={1}, OperationType={2}",
                    history.Id, history.DeviceId, history.OperationType);

                GetDeviceTransactionHistoryRepository().SaveOrUpdate(history);

                _logger.Info("SaveOrUpdate: Lưu lịch sử giao dịch thành công, Id={0}", history.Id);
            }
            catch (Exception ex)
            {
                _logger.Error($"SaveOrUpdate: Lỗi lưu lịch sử giao dịch: {ex.Message}", ex);
                throw;
            }
        }

        #endregion

        #region Delete Operations

        /// <summary>
        /// Xóa DeviceTransactionHistory
        /// </summary>
        /// <param name="id">ID của bản ghi cần xóa</param>
        public void Delete(Guid id)
        {
            try
            {
                _logger.Debug("Delete: Bắt đầu xóa lịch sử giao dịch, Id={0}", id);

                GetDeviceTransactionHistoryRepository().Delete(id);

                _logger.Info("Delete: Xóa lịch sử giao dịch thành công, Id={0}", id);
            }
            catch (Exception ex)
            {
                _logger.Error($"Delete: Lỗi xóa lịch sử giao dịch: {ex.Message}", ex);
                throw;
            }
        }

        #endregion
    }
}

