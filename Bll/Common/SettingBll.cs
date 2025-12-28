using System;
using System.Collections.Generic;
using System.Linq;
using Dal.Connection;
using Dal.DataAccess.Implementations;
using Dal.DataAccess.Interfaces;
using Dal.DataContext;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;

namespace Bll.Common;

/// <summary>
/// Business Logic Layer cho quản lý Settings
/// </summary>
public class SettingBll
{
    #region Fields

    private ISettingRepository _dataAccess;
    private readonly ILogger _logger;
    private readonly object _lockObject = new object();

    #endregion

    #region Constructors

    /// <summary>
    /// Constructor mặc định
    /// </summary>
    public SettingBll()
    {
        _logger = LoggerFactory.CreateLogger(LogCategory.BLL);
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Lấy hoặc khởi tạo Repository (lazy initialization)
    /// </summary>
    private ISettingRepository GetDataAccess()
    {
        if (_dataAccess == null)
        {
            lock (_lockObject)
            {
                if (_dataAccess == null)
                {
                    try
                    {
                        var globalConnectionString = ApplicationStartupManager.Instance.GetGlobalConnectionString();
                        if (string.IsNullOrEmpty(globalConnectionString))
                        {
                            throw new InvalidOperationException(
                                "Không có global connection string. Ứng dụng chưa được khởi tạo hoặc chưa sẵn sàng.");
                        }

                        _dataAccess = new SettingRepository(globalConnectionString);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"Lỗi khi khởi tạo SettingRepository: {ex.Message}", ex);
                        throw new InvalidOperationException(
                            "Không thể khởi tạo kết nối database. Vui lòng kiểm tra cấu hình database.", ex);
                    }
                }
            }
        }

        return _dataAccess;
    }

    #endregion

    #region ========== READ OPERATIONS ==========

    /// <summary>
    /// Lấy giá trị setting theo category và key
    /// </summary>
    /// <param name="category">Category của setting</param>
    /// <param name="settingKey">Key của setting</param>
    /// <param name="defaultValue">Giá trị mặc định nếu không tìm thấy</param>
    /// <returns>Giá trị setting hoặc defaultValue</returns>
    public string GetValue(string category, string settingKey, string defaultValue = "")
    {
        try
        {
            _logger?.Debug($"Lấy giá trị setting {category}.{settingKey}");
            return GetDataAccess().GetValue(category, settingKey, defaultValue);
        }
        catch (Exception ex)
        {
            _logger?.Error($"Lỗi khi lấy giá trị setting {category}.{settingKey}: {ex.Message}", ex);
            return defaultValue;
        }
    }

    /// <summary>
    /// Lấy giá trị setting và giải mã nếu được mã hóa
    /// </summary>
    /// <param name="category">Category của setting</param>
    /// <param name="settingKey">Key của setting</param>
    /// <param name="defaultValue">Giá trị mặc định nếu không tìm thấy</param>
    /// <returns>Giá trị setting đã giải mã hoặc defaultValue</returns>
    public string GetDecryptedValue(string category, string settingKey, string defaultValue = "")
    {
        try
        {
            _logger?.Debug($"Lấy và giải mã giá trị setting {category}.{settingKey}");
            return GetDataAccess().GetDecryptedValue(category, settingKey, defaultValue);
        }
        catch (Exception ex)
        {
            _logger?.Error($"Lỗi khi lấy và giải mã giá trị setting {category}.{settingKey}: {ex.Message}", ex);
            return defaultValue;
        }
    }

    /// <summary>
    /// Lấy setting entity theo category và key
    /// </summary>
    /// <param name="category">Category của setting</param>
    /// <param name="settingKey">Key của setting</param>
    /// <returns>Setting entity hoặc null</returns>
    public Setting GetByCategoryAndKey(string category, string settingKey)
    {
        try
        {
            _logger?.Debug($"Lấy setting entity {category}.{settingKey}");
            return GetDataAccess().GetByCategoryAndKey(category, settingKey);
        }
        catch (Exception ex)
        {
            _logger?.Error($"Lỗi khi lấy setting entity {category}.{settingKey}: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Lấy tất cả settings theo category
    /// </summary>
    /// <param name="category">Category của setting</param>
    /// <param name="isActiveOnly">Chỉ lấy settings đang active</param>
    /// <returns>Danh sách settings</returns>
    public List<Setting> GetAllByCategory(string category, bool isActiveOnly = true)
    {
        try
        {
            _logger?.Debug($"Lấy tất cả settings của category {category}");
            return GetDataAccess().GetAllByCategory(category, isActiveOnly);
        }
        catch (Exception ex)
        {
            _logger?.Error($"Lỗi khi lấy tất cả settings của category {category}: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Lấy settings theo category và group
    /// </summary>
    /// <param name="category">Category của setting</param>
    /// <param name="groupName">Tên nhóm</param>
    /// <param name="isActiveOnly">Chỉ lấy settings đang active</param>
    /// <returns>Danh sách settings</returns>
    public List<Setting> GetByCategoryAndGroup(string category, string groupName, bool isActiveOnly = true)
    {
        try
        {
            _logger?.Debug($"Lấy settings của category {category} và group {groupName}");
            return GetDataAccess().GetByCategoryAndGroup(category, groupName, isActiveOnly);
        }
        catch (Exception ex)
        {
            _logger?.Error($"Lỗi khi lấy settings của category {category} và group {groupName}: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Kiểm tra setting có tồn tại không
    /// </summary>
    /// <param name="category">Category của setting</param>
    /// <param name="settingKey">Key của setting</param>
    /// <returns>True nếu tồn tại</returns>
    public bool Exists(string category, string settingKey)
    {
        try
        {
            return GetDataAccess().Exists(category, settingKey);
        }
        catch (Exception ex)
        {
            _logger?.Error($"Lỗi khi kiểm tra tồn tại setting {category}.{settingKey}: {ex.Message}", ex);
            return false;
        }
    }

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
    public void SetValue(string category, string settingKey, string settingValue, string valueType = "String", string updatedBy = null, bool encrypt = false)
    {
        try
        {
            _logger?.Info($"Lưu setting {category}.{settingKey}");
            GetDataAccess().SetValue(category, settingKey, settingValue, valueType, updatedBy, encrypt);
            _logger?.Info($"Đã lưu setting {category}.{settingKey} thành công");
        }
        catch (Exception ex)
        {
            _logger?.Error($"Lỗi khi lưu setting {category}.{settingKey}: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Lưu nhiều settings cùng lúc
    /// </summary>
    /// <param name="category">Category của settings</param>
    /// <param name="settings">Dictionary chứa các settings cần lưu (key: settingKey, value: settingValue)</param>
    /// <param name="valueType">Loại dữ liệu mặc định</param>
    /// <param name="updatedBy">Người cập nhật</param>
    public void SetMultipleValues(string category, Dictionary<string, string> settings, string valueType = "String", string updatedBy = null)
    {
        try
        {
            _logger?.Info($"Lưu {settings.Count} settings của category {category}");
            var dataAccess = GetDataAccess();

            foreach (var setting in settings)
            {
                // Xác định xem có cần mã hóa không (dựa vào key)
                bool encrypt = setting.Key.ToLower().Contains("password") || setting.Key.ToLower().Contains("secret");
                dataAccess.SetValue(category, setting.Key, setting.Value, valueType, updatedBy, encrypt);
            }

            _logger?.Info($"Đã lưu {settings.Count} settings thành công");
        }
        catch (Exception ex)
        {
            _logger?.Error($"Lỗi khi lưu nhiều settings: {ex.Message}", ex);
            throw;
        }
    }

    #endregion

    #region ========== BUSINESS LOGIC METHODS ==========

    /// <summary>
    /// Lấy tất cả NAS settings
    /// </summary>
    /// <returns>Dictionary chứa các NAS settings (key: settingKey, value: settingValue đã giải mã)</returns>
    public Dictionary<string, string> GetNASSettings()
    {
        try
        {
            _logger?.Debug("Lấy tất cả NAS settings");
            var settings = GetAllByCategory("NAS", true);
            var result = new Dictionary<string, string>();

            foreach (var setting in settings)
            {
                var value = setting.IsEncrypted
                    ? GetDataAccess().GetDecryptedValue("NAS", setting.SettingKey, "")
                    : setting.SettingValue ?? "";

                result[setting.SettingKey] = value;
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger?.Error($"Lỗi khi lấy NAS settings: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Lưu NAS settings
    /// </summary>
    /// <param name="nasSettings">Dictionary chứa các NAS settings (key: settingKey, value: settingValue)</param>
    /// <param name="updatedBy">Người cập nhật</param>
    public void SaveNASSettings(Dictionary<string, string> nasSettings, string updatedBy = null)
    {
        try
        {
            _logger?.Info($"Lưu {nasSettings.Count} NAS settings");
            var dataAccess = GetDataAccess();

            foreach (var setting in nasSettings)
            {
                // Xác định valueType và encrypt dựa trên key
                string valueType = "String";
                bool encrypt = false;

                if (setting.Key == "ConnectionTimeout" || setting.Key == "RetryAttempts" || setting.Key == "CacheSize")
                {
                    valueType = "Int";
                }
                else if (setting.Key == "EnableCache")
                {
                    valueType = "Bool";
                }
                else if (setting.Key == "Password")
                {
                    encrypt = true;
                }

                dataAccess.SetValue("NAS", setting.Key, setting.Value, valueType, updatedBy, encrypt);
            }

            _logger?.Info($"Đã lưu {nasSettings.Count} NAS settings thành công");
        }
        catch (Exception ex)
        {
            _logger?.Error($"Lỗi khi lưu NAS settings: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Lấy tất cả ImageStorage settings
    /// </summary>
    /// <returns>Dictionary chứa các ImageStorage settings (key: settingKey, value: settingValue)</returns>
    public Dictionary<string, string> GetImageStorageSettings()
    {
        try
        {
            _logger?.Debug("Lấy tất cả ImageStorage settings");
            var settings = GetAllByCategory("ImageStorage", true);
            var result = new Dictionary<string, string>();

            foreach (var setting in settings)
            {
                var value = setting.IsEncrypted
                    ? GetDataAccess().GetDecryptedValue("ImageStorage", setting.SettingKey, "")
                    : setting.SettingValue ?? "";

                result[setting.SettingKey] = value;
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger?.Error($"Lỗi khi lấy ImageStorage settings: {ex.Message}", ex);
            throw;
        }
    }

    #endregion
}
