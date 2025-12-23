using System;
using System.Collections.Generic;
using Dal.DataContext;

namespace Dal.DataAccess.Interfaces.Inventory.InventoryManagement;

/// <summary>
/// Interface cho DeviceTransactionHistoryRepository
/// Cung cấp các phương thức truy vấn và thao tác với lịch sử giao dịch thiết bị
/// </summary>
public interface IDeviceTransactionHistoryRepository
{
    /// <summary>
    /// Lấy DeviceTransactionHistory theo ID
    /// </summary>
    /// <param name="id">ID của bản ghi lịch sử</param>
    /// <returns>DeviceTransactionHistory entity hoặc null</returns>
    DeviceTransactionHistory GetById(Guid id);

    /// <summary>
    /// Lấy danh sách lịch sử giao dịch theo DeviceId
    /// </summary>
    /// <param name="deviceId">ID thiết bị</param>
    /// <returns>Danh sách DeviceTransactionHistory entities, sắp xếp theo ngày mới nhất</returns>
    List<DeviceTransactionHistory> GetByDeviceId(Guid deviceId);

    /// <summary>
    /// Lấy danh sách lịch sử giao dịch theo loại thao tác
    /// </summary>
    /// <param name="operationType">Loại thao tác (0=Import, 1=Export, 2=Allocation, 3=Recovery, 4=Transfer, 5=Maintenance, 6=StatusChange, 7=Other)</param>
    /// <returns>Danh sách DeviceTransactionHistory entities</returns>
    List<DeviceTransactionHistory> GetByOperationType(int operationType);

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
    List<DeviceTransactionHistory> Query(
        Guid? deviceId = null,
        int? operationType = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        Guid? referenceId = null,
        int? referenceType = null,
        Guid? performedBy = null,
        string keyword = null);

    /// <summary>
    /// Lưu hoặc cập nhật DeviceTransactionHistory
    /// </summary>
    /// <param name="history">DeviceTransactionHistory entity cần lưu</param>
    void SaveOrUpdate(DeviceTransactionHistory history);

    /// <summary>
    /// Xóa DeviceTransactionHistory
    /// </summary>
    /// <param name="id">ID của bản ghi cần xóa</param>
    void Delete(Guid id);
}
