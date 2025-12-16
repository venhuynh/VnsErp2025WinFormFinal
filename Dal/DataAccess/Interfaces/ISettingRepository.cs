using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dal.DataContext;

namespace Dal.DataAccess.Interfaces;

/// <summary>
/// Interface cho Repository quản lý Settings
/// </summary>
public interface ISettingRepository
{
    /// <summary>
    /// Lấy giá trị setting theo category và key
    /// </summary>
    /// <param name="category">Category của setting</param>
    /// <param name="settingKey">Key của setting</param>
    /// <param name="defaultValue">Giá trị mặc định nếu không tìm thấy</param>
    /// <returns>Giá trị setting hoặc defaultValue</returns>
    string GetValue(string category, string settingKey, string defaultValue = "");

    /// <summary>
    /// Lấy giá trị setting theo category và key (async)
    /// </summary>
    /// <param name="category">Category của setting</param>
    /// <param name="settingKey">Key của setting</param>
    /// <param name="defaultValue">Giá trị mặc định nếu không tìm thấy</param>
    /// <returns>Giá trị setting hoặc defaultValue</returns>
    Task<string> GetValueAsync(string category, string settingKey, string defaultValue = "");

    /// <summary>
    /// Lấy giá trị setting và giải mã nếu được mã hóa
    /// </summary>
    /// <param name="category">Category của setting</param>
    /// <param name="settingKey">Key của setting</param>
    /// <param name="defaultValue">Giá trị mặc định nếu không tìm thấy</param>
    /// <returns>Giá trị setting đã giải mã hoặc defaultValue</returns>
    string GetDecryptedValue(string category, string settingKey, string defaultValue = "");

    /// <summary>
    /// Lấy giá trị setting và giải mã nếu được mã hóa (async)
    /// </summary>
    /// <param name="category">Category của setting</param>
    /// <param name="settingKey">Key của setting</param>
    /// <param name="defaultValue">Giá trị mặc định nếu không tìm thấy</param>
    /// <returns>Giá trị setting đã giải mã hoặc defaultValue</returns>
    Task<string> GetDecryptedValueAsync(string category, string settingKey, string defaultValue = "");

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

    /// <summary>
    /// Lưu hoặc cập nhật giá trị setting (async)
    /// </summary>
    /// <param name="category">Category của setting</param>
    /// <param name="settingKey">Key của setting</param>
    /// <param name="settingValue">Giá trị setting</param>
    /// <param name="valueType">Loại dữ liệu (String, Int, Bool, DateTime, Decimal, JSON)</param>
    /// <param name="updatedBy">Người cập nhật</param>
    /// <param name="encrypt">Có mã hóa giá trị không (mặc định false)</param>
    Task SetValueAsync(string category, string settingKey, string settingValue, string valueType = "String", string updatedBy = null, bool encrypt = false);

    /// <summary>
    /// Lấy setting entity theo category và key
    /// </summary>
    /// <param name="category">Category của setting</param>
    /// <param name="settingKey">Key của setting</param>
    /// <returns>Setting entity hoặc null</returns>
    Setting GetByCategoryAndKey(string category, string settingKey);

    /// <summary>
    /// Lấy setting entity theo category và key (async)
    /// </summary>
    /// <param name="category">Category của setting</param>
    /// <param name="settingKey">Key của setting</param>
    /// <returns>Setting entity hoặc null</returns>
    Task<Setting> GetByCategoryAndKeyAsync(string category, string settingKey);

    /// <summary>
    /// Lấy tất cả settings theo category
    /// </summary>
    /// <param name="category">Category của setting</param>
    /// <param name="isActiveOnly">Chỉ lấy settings đang active</param>
    /// <returns>Danh sách settings</returns>
    List<Setting> GetAllByCategory(string category, bool isActiveOnly = true);

    /// <summary>
    /// Lấy tất cả settings theo category (async)
    /// </summary>
    /// <param name="category">Category của setting</param>
    /// <param name="isActiveOnly">Chỉ lấy settings đang active</param>
    /// <returns>Danh sách settings</returns>
    Task<List<Setting>> GetAllByCategoryAsync(string category, bool isActiveOnly = true);

    /// <summary>
    /// Lấy tất cả settings theo category và group
    /// </summary>
    /// <param name="category">Category của setting</param>
    /// <param name="groupName">Tên nhóm</param>
    /// <param name="isActiveOnly">Chỉ lấy settings đang active</param>
    /// <returns>Danh sách settings</returns>
    List<Setting> GetByCategoryAndGroup(string category, string groupName, bool isActiveOnly = true);

    /// <summary>
    /// Lấy tất cả settings theo category và group (async)
    /// </summary>
    /// <param name="category">Category của setting</param>
    /// <param name="groupName">Tên nhóm</param>
    /// <param name="isActiveOnly">Chỉ lấy settings đang active</param>
    /// <returns>Danh sách settings</returns>
    Task<List<Setting>> GetByCategoryAndGroupAsync(string category, string groupName, bool isActiveOnly = true);

    /// <summary>
    /// Tạo setting mới
    /// </summary>
    /// <param name="setting">Setting entity</param>
    /// <returns>Setting đã tạo</returns>
    Setting Create(Setting setting);

    /// <summary>
    /// Tạo setting mới (async)
    /// </summary>
    /// <param name="setting">Setting entity</param>
    /// <returns>Setting đã tạo</returns>
    Task<Setting> CreateAsync(Setting setting);

    /// <summary>
    /// Cập nhật setting
    /// </summary>
    /// <param name="setting">Setting entity</param>
    /// <returns>Setting đã cập nhật</returns>
    Setting Update(Setting setting);

    /// <summary>
    /// Cập nhật setting (async)
    /// </summary>
    /// <param name="setting">Setting entity</param>
    /// <returns>Setting đã cập nhật</returns>
    Task<Setting> UpdateAsync(Setting setting);

    /// <summary>
    /// Xóa setting theo ID
    /// </summary>
    /// <param name="settingId">ID của setting</param>
    void Delete(int settingId);

    /// <summary>
    /// Xóa setting theo ID (async)
    /// </summary>
    /// <param name="settingId">ID của setting</param>
    Task DeleteAsync(int settingId);

    /// <summary>
    /// Kiểm tra setting có tồn tại không
    /// </summary>
    /// <param name="category">Category của setting</param>
    /// <param name="settingKey">Key của setting</param>
    /// <returns>True nếu tồn tại</returns>
    bool Exists(string category, string settingKey);

    /// <summary>
    /// Kiểm tra setting có tồn tại không (async)
    /// </summary>
    /// <param name="category">Category của setting</param>
    /// <param name="settingKey">Key của setting</param>
    /// <returns>True nếu tồn tại</returns>
    Task<bool> ExistsAsync(string category, string settingKey);
}
