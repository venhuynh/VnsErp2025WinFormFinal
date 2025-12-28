using System;
using System.Collections.Generic;
using DTO.DeviceAssetManagement;

namespace Dal.DataAccess.Interfaces.Inventory.InventoryManagement;

/// <summary>
/// Interface cho DeviceTransactionHistoryRepository
/// Cung cấp các phương thức truy vấn và thao tác với lịch sử giao dịch thiết bị
/// </summary>
public interface IDeviceTransactionHistoryRepository
{
    #region ========== READ OPERATIONS ==========

    /// <summary>
    /// Lấy DeviceTransactionHistory theo ID
    /// </summary>
    /// <param name="id">ID của bản ghi lịch sử</param>
    /// <returns>DeviceTransactionHistoryDto hoặc null</returns>
    DeviceTransactionHistoryDto GetById(Guid id);

    /// <summary>
    /// Lấy danh sách lịch sử giao dịch theo DeviceId
    /// </summary>
    /// <param name="deviceId">ID thiết bị</param>
    /// <returns>Danh sách DeviceTransactionHistoryDto, sắp xếp theo ngày mới nhất</returns>
    List<DeviceTransactionHistoryDto> GetByDeviceId(Guid deviceId);

    /// <summary>
    /// Lấy danh sách lịch sử giao dịch theo loại thao tác
    /// </summary>
    /// <param name="operationType">Loại thao tác (0=Import, 1=Export, 2=Allocation, 3=Recovery, 4=Transfer, 5=Maintenance, 6=StatusChange, 7=Other)</param>
    /// <returns>Danh sách DeviceTransactionHistoryDto</returns>
    List<DeviceTransactionHistoryDto> GetByOperationType(int operationType);

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
    /// <returns>Danh sách DeviceTransactionHistoryDto, sắp xếp theo ngày mới nhất</returns>
    List<DeviceTransactionHistoryDto> Query(
        Guid? deviceId = null,
        int? operationType = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        Guid? referenceId = null,
        int? referenceType = null,
        Guid? performedBy = null,
        string keyword = null);

    #endregion

    #region ========== CREATE/UPDATE OPERATIONS ==========

    /// <summary>
    /// Lưu hoặc cập nhật DeviceTransactionHistory
    /// </summary>
    /// <param name="dto">DeviceTransactionHistoryDto cần lưu</param>
    /// <returns>DeviceTransactionHistoryDto đã được lưu</returns>
    DeviceTransactionHistoryDto SaveOrUpdate(DeviceTransactionHistoryDto dto);

    #endregion

    #region ========== DELETE OPERATIONS ==========

    /// <summary>
    /// Xóa DeviceTransactionHistory
    /// </summary>
    /// <param name="id">ID của bản ghi cần xóa</param>
    void Delete(Guid id);

    #endregion
}
