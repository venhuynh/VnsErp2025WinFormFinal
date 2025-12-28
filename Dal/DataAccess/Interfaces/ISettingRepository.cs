using System.Collections.Generic;
using Dal.DataContext;

namespace Dal.DataAccess.Interfaces;

/// <summary>
/// Interface cho Repository quản lý Settings
/// </summary>
public interface ISettingRepository
{
    #region ========== READ OPERATIONS ==========

    /// <summary>
    /// Lấy giá trị setting theo category và key
    /// </summary>
    /// <param name="category">Category của setting</param>
    /// <param name="settingKey">Key của setting</param>
    /// <param name="defaultValue">Giá trị mặc định nếu không tìm thấy</param>
    /// <returns>Giá trị setting hoặc defaultValue</returns>
    string GetValue(string category, string settingKey, string defaultValue = "");

    /// <summary>
    /// Lấy giá trị setting và giải mã nếu được mã hóa
    /// </summary>
    /// <param name="category">Category của setting</param>
    /// <param name="settingKey">Key của setting</param>
    /// <param name="defaultValue">Giá trị mặc định nếu không tìm thấy</param>
    /// <returns>Giá trị setting đã giải mã hoặc defaultValue</returns>
    string GetDecryptedValue(string category, string settingKey, string defaultValue = "");

    /// <summary>
    /// Lấy setting entity theo category và key
    /// </summary>
    /// <param name="category">Category của setting</param>
    /// <param name="settingKey">Key của setting</param>
    /// <returns>Setting entity hoặc null</returns>
    Setting GetByCategoryAndKey(string category, string settingKey);

    /// <summary>
    /// Lấy tất cả settings theo category
    /// </summary>
    /// <param name="category">Category của setting</param>
    /// <param name="isActiveOnly">Chỉ lấy settings đang active</param>
    /// <returns>Danh sách settings</returns>
    List<Setting> GetAllByCategory(string category, bool isActiveOnly = true);

    /// <summary>
    /// Lấy tất cả settings theo category và group
    /// </summary>
    /// <param name="category">Category của setting</param>
    /// <param name="groupName">Tên nhóm</param>
    /// <param name="isActiveOnly">Chỉ lấy settings đang active</param>
    /// <returns>Danh sách settings</returns>
    List<Setting> GetByCategoryAndGroup(string category, string groupName, bool isActiveOnly = true);

    /// <summary>
    /// Kiểm tra setting có tồn tại không
    /// </summary>
    /// <param name="category">Category của setting</param>
    /// <param name="settingKey">Key của setting</param>
    /// <returns>True nếu tồn tại</returns>
    bool Exists(string category, string settingKey);

    #endregion

    #region ========== UPDATE OPERATIONS ==========

    /// <summary>
    /// Lưu hoặc cập nhật giá trị setting
    /// </summary>
    /// <param name="category">Category của setting</param>
    /// <param name="settingKey">Key của setting</param>
    /// <param name="settingValue">Giá trị setting</param>
    /// <param name="valueType">Loại dữ liệu (String, Int, Bool, DateTime, Decimal, JSON)</param>
    /// <param name="updatedBy">Người cập nhật</param>
    /// <param name="encrypt">Có mã hóa giá trị không (mặc định false)</param>
    void SetValue(string category, string settingKey, string settingValue, string valueType = "String", string updatedBy = null, bool encrypt = false);

    #endregion
}
