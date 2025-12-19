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

/// <summary>
/// Repository cho DeviceTransactionHistory entity
/// Cung cấp các phương thức truy vấn và thao tác với lịch sử giao dịch thiết bị
/// </summary>
public class DeviceTransactionHistoryRepository : IDeviceTransactionHistoryRepository
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
    /// Khởi tạo một instance mới của class DeviceTransactionHistoryRepository.
    /// </summary>
    /// <param name="connectionString">Chuỗi kết nối cơ sở dữ liệu</param>
    /// <exception cref="ArgumentNullException">Ném ra khi connectionString là null</exception>
    public DeviceTransactionHistoryRepository(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _logger = LoggerFactory.CreateLogger(LogCategory.DAL);
        _logger.Info("DeviceTransactionHistoryRepository được khởi tạo với connection string");
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
        loadOptions.LoadWith<DeviceTransactionHistory>(h => h.Device);
        loadOptions.LoadWith<Device>(d => d.ProductVariant);
        loadOptions.LoadWith<ProductVariant>(v => v.ProductService);
        context.LoadOptions = loadOptions;

        return context;
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
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetById: Lấy lịch sử giao dịch, Id={0}", id);

            var history = context.DeviceTransactionHistories.FirstOrDefault(h => h.Id == id);

            if (history == null)
            {
                _logger.Warning("GetById: Không tìm thấy lịch sử giao dịch, Id={0}", id);
            }
            else
            {
                _logger.Info("GetById: Lấy lịch sử giao dịch thành công, Id={0}", id);
            }

            return history;
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
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetByDeviceId: Lấy danh sách lịch sử giao dịch, DeviceId={0}", deviceId);

            var histories = context.DeviceTransactionHistories
                .Where(h => h.DeviceId == deviceId)
                .OrderByDescending(h => h.OperationDate)
                .ThenByDescending(h => h.CreatedDate)
                .ToList();

            _logger.Info("GetByDeviceId: Lấy được {0} bản ghi lịch sử", histories.Count);
            return histories;
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
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetByOperationType: Lấy danh sách lịch sử giao dịch, OperationType={0}", operationType);

            var histories = context.DeviceTransactionHistories
                .Where(h => h.OperationType == operationType)
                .OrderByDescending(h => h.OperationDate)
                .ThenByDescending(h => h.CreatedDate)
                .ToList();

            _logger.Info("GetByOperationType: Lấy được {0} bản ghi lịch sử", histories.Count);
            return histories;
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
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("Query: Bắt đầu query lịch sử giao dịch, DeviceId={0}, OperationType={1}, FromDate={2}, ToDate={3}, ReferenceId={4}, ReferenceType={5}, PerformedBy={6}, Keyword={7}",
                deviceId, operationType, fromDate, toDate, referenceId, referenceType, performedBy, keyword ?? "null");

            var query = context.DeviceTransactionHistories.AsQueryable();

            // Filter theo DeviceId
            if (deviceId.HasValue)
            {
                query = query.Where(h => h.DeviceId == deviceId.Value);
            }

            // Filter theo OperationType
            if (operationType.HasValue)
            {
                query = query.Where(h => h.OperationType == operationType.Value);
            }

            // Filter theo khoảng thời gian
            if (fromDate.HasValue || toDate.HasValue)
            {
                if (fromDate.HasValue && toDate.HasValue)
                {
                    query = query.Where(h => h.OperationDate.Date >= fromDate.Value.Date &&
                                            h.OperationDate.Date <= toDate.Value.Date);
                }
                else if (fromDate.HasValue)
                {
                    query = query.Where(h => h.OperationDate.Date >= fromDate.Value.Date);
                }
                else if (toDate.HasValue)
                {
                    query = query.Where(h => h.OperationDate.Date <= toDate.Value.Date);
                }
            }

            // Filter theo ReferenceId
            if (referenceId.HasValue)
            {
                query = query.Where(h => h.ReferenceId.HasValue && h.ReferenceId.Value == referenceId.Value);
            }

            // Filter theo ReferenceType
            if (referenceType.HasValue)
            {
                query = query.Where(h => h.ReferenceType.HasValue && h.ReferenceType.Value == referenceType.Value);
            }

            // Filter theo PerformedBy
            if (performedBy.HasValue)
            {
                query = query.Where(h => h.PerformedBy.HasValue && h.PerformedBy.Value == performedBy.Value);
            }

            // Filter theo keyword (tìm trong Information)
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var searchText = keyword.Trim();
                query = query.Where(h =>
                    (h.Information != null && h.Information.Contains(searchText)) ||
                    (h.Notes != null && h.Notes.Contains(searchText)));
            }

            // Sắp xếp theo ngày mới nhất
            var result = query
                .OrderByDescending(h => h.OperationDate)
                .ThenByDescending(h => h.CreatedDate)
                .ToList();

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
        using var context = CreateNewContext();
        try
        {
            if (history == null)
                throw new ArgumentNullException(nameof(history));

            _logger.Debug("SaveOrUpdate: Bắt đầu lưu lịch sử giao dịch, Id={0}, DeviceId={1}, OperationType={2}",
                history.Id, history.DeviceId, history.OperationType);

            var existingHistory = context.DeviceTransactionHistories.FirstOrDefault(h => h.Id == history.Id);

            if (existingHistory == null)
            {
                // Thêm mới
                if (history.Id == Guid.Empty)
                {
                    history.Id = Guid.NewGuid();
                }
                if (history.CreatedDate == default(DateTime))
                {
                    history.CreatedDate = DateTime.Now;
                }
                if (history.OperationDate == default(DateTime))
                {
                    history.OperationDate = DateTime.Now;
                }
                context.DeviceTransactionHistories.InsertOnSubmit(history);
                _logger.Info("SaveOrUpdate: Thêm mới lịch sử giao dịch, Id={0}", history.Id);
            }
            else
            {
                // Cập nhật
                existingHistory.DeviceId = history.DeviceId;
                existingHistory.OperationType = history.OperationType;
                existingHistory.OperationDate = history.OperationDate;
                existingHistory.ReferenceId = history.ReferenceId;
                existingHistory.ReferenceType = history.ReferenceType;
                existingHistory.Information = history.Information;
                existingHistory.HtmlInformation = history.HtmlInformation;
                existingHistory.PerformedBy = history.PerformedBy;
                existingHistory.Notes = history.Notes;
                _logger.Info("SaveOrUpdate: Cập nhật lịch sử giao dịch, Id={0}", history.Id);
            }

            context.SubmitChanges();
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
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("Delete: Bắt đầu xóa lịch sử giao dịch, Id={0}", id);

            var history = context.DeviceTransactionHistories.FirstOrDefault(h => h.Id == id);
            if (history == null)
            {
                _logger.Warning("Delete: Không tìm thấy lịch sử giao dịch với Id={0}", id);
                return;
            }

            context.DeviceTransactionHistories.DeleteOnSubmit(history);
            context.SubmitChanges();

            _logger.Info("Delete: Đã xóa lịch sử giao dịch, Id={0}", id);
        }
        catch (Exception ex)
        {
            _logger.Error($"Delete: Lỗi xóa lịch sử giao dịch: {ex.Message}", ex);
            throw;
        }
    }

    #endregion
}
