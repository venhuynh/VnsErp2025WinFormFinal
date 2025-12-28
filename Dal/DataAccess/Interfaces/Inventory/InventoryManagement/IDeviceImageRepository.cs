using System;
using System.Collections.Generic;
using Dal.DataContext;

namespace Dal.DataAccess.Interfaces.Inventory.InventoryManagement;

/// <summary>
/// Data Access Layer cho DeviceImage
/// </summary>
public interface IDeviceImageRepository
{
    #region ========== READ OPERATIONS ==========

    /// <summary>
    /// Lấy tất cả hình ảnh
    /// </summary>
    /// <returns>Danh sách tất cả DeviceImage</returns>
    List<DeviceImage> GetAll();

    /// <summary>
    /// Lấy danh sách hình ảnh theo DeviceId
    /// </summary>
    /// <param name="deviceId">Id của Device</param>
    /// <returns>Danh sách DeviceImage</returns>
    List<DeviceImage> GetByDeviceId(Guid deviceId);

    /// <summary>
    /// Lấy DeviceImage theo Id
    /// </summary>
    /// <param name="imageId">Id của DeviceImage</param>
    /// <returns>DeviceImage hoặc null nếu không tìm thấy</returns>
    DeviceImage GetById(Guid imageId);

    /// <summary>
    /// Lấy hình ảnh chính (primary) của Device
    /// </summary>
    /// <param name="deviceId">Id của Device</param>
    /// <returns>DeviceImage chính hoặc null</returns>
    DeviceImage GetPrimaryByDeviceId(Guid deviceId);

    /// <summary>
    /// Tìm kiếm DeviceImage theo danh sách DeviceIds
    /// </summary>
    /// <param name="deviceIds">Danh sách Device Ids</param>
    /// <returns>Danh sách DeviceImage</returns>
    List<DeviceImage> SearchByDeviceIds(List<Guid> deviceIds);

    #endregion

    #region ========== CREATE/UPDATE OPERATIONS ==========

    /// <summary>
    /// Lưu hoặc cập nhật DeviceImage
    /// </summary>
    /// <param name="deviceImage">DeviceImage cần lưu hoặc cập nhật</param>
    void SaveOrUpdate(DeviceImage deviceImage);

    /// <summary>
    /// Đặt hình ảnh làm hình chính cho Device
    /// </summary>
    /// <param name="imageId">Id của DeviceImage cần đặt làm chính</param>
    void SetAsPrimary(Guid imageId);

    #endregion

    #region ========== DELETE OPERATIONS ==========

    /// <summary>
    /// Xóa DeviceImage (soft delete - đánh dấu IsActive = false nếu có, hoặc hard delete)
    /// </summary>
    /// <param name="imageId">Id của DeviceImage cần xóa</param>
    void Delete(Guid imageId);

    /// <summary>
    /// Xóa vĩnh viễn DeviceImage khỏi database
    /// </summary>
    /// <param name="imageId">Id của DeviceImage cần xóa vĩnh viễn</param>
    void DeletePermanent(Guid imageId);

    #endregion
}
