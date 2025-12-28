using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using Common.Appconfig;
using Dal.DataAccess.Interfaces;
using Dal.DataContext;
using Dal.Exceptions;
using Logger;
using Logger.Configuration;
using CustomLogger = Logger.Interfaces.ILogger;

namespace Dal.DataAccess.Implementations;

/// <summary>
/// Repository quản lý Settings
/// </summary>
public class SettingRepository : ISettingRepository
{
    #region Private Fields

    private readonly string _connectionString;
    private readonly CustomLogger _logger;

    #endregion

    #region Constructor

    /// <summary>
    /// Khởi tạo một instance mới của class SettingRepository
    /// </summary>
    /// <param name="connectionString">Chuỗi kết nối cơ sở dữ liệu</param>
    /// <exception cref="ArgumentNullException">Ném ra khi connectionString là null</exception>
    public SettingRepository(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _logger = LoggerFactory.CreateLogger(LogCategory.DAL);
        _logger.Info("SettingRepository được khởi tạo");
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Tạo DataContext mới cho mỗi operation
    /// </summary>
    /// <returns>DataContext mới</returns>
    private VnsErp2025DataContext CreateNewContext()
    {
        var context = new VnsErp2025DataContext(_connectionString);
        var loadOptions = new DataLoadOptions();
        context.LoadOptions = loadOptions;
        return context;
    }

    #endregion

    #region ========== READ OPERATIONS ==========

    /// <summary>
    /// Lấy giá trị setting theo category và key
    /// </summary>
    public string GetValue(string category, string settingKey, string defaultValue = "")
    {
        try
        {
            using var context = CreateNewContext();
            var setting = context.Settings
                .FirstOrDefault(s => s.Category == category && s.SettingKey == settingKey && s.IsActive);

            if (setting == null)
                return defaultValue;

            return setting.SettingValue ?? defaultValue;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy giá trị setting {category}.{settingKey}: {ex.Message}", ex);
            return defaultValue;
        }
    }

    /// <summary>
    /// Lấy giá trị setting và giải mã nếu được mã hóa
    /// </summary>
    public string GetDecryptedValue(string category, string settingKey, string defaultValue = "")
    {
        try
        {
            using var context = CreateNewContext();
            var setting = context.Settings
                .FirstOrDefault(s => s.Category == category && s.SettingKey == settingKey && s.IsActive);

            if (setting == null)
                return defaultValue;

            var value = setting.SettingValue ?? defaultValue;

            // Nếu được mã hóa, giải mã
            if (setting.IsEncrypted && !string.IsNullOrEmpty(value))
            {
                try
                {
                    value = VntaCrypto.Decrypt(value);
                }
                catch (Exception ex)
                {
                    _logger.Warning($"Không thể giải mã setting {category}.{settingKey}: {ex.Message}");
                    // Trả về giá trị gốc nếu không giải mã được
                }
            }

            return value;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy và giải mã giá trị setting {category}.{settingKey}: {ex.Message}", ex);
            return defaultValue;
        }
    }

    /// <summary>
    /// Lấy setting entity theo category và key
    /// </summary>
    public Setting GetByCategoryAndKey(string category, string settingKey)
    {
        try
        {
            using var context = CreateNewContext();
            return context.Settings
                .FirstOrDefault(s => s.Category == category && s.SettingKey == settingKey);
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy setting {category}.{settingKey}: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy setting {category}.{settingKey}: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Lấy tất cả settings theo category
    /// </summary>
    public List<Setting> GetAllByCategory(string category, bool isActiveOnly = true)
    {
        try
        {
            using var context = CreateNewContext();
            var query = context.Settings.Where(s => s.Category == category);

            if (isActiveOnly)
            {
                query = query.Where(s => s.IsActive);
            }

            return query.OrderBy(s => s.DisplayOrder).ThenBy(s => s.SettingKey).ToList();
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy tất cả settings của category {category}: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy tất cả settings của category {category}: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Lấy tất cả settings theo category và group
    /// </summary>
    public List<Setting> GetByCategoryAndGroup(string category, string groupName, bool isActiveOnly = true)
    {
        try
        {
            using var context = CreateNewContext();
            var query = context.Settings
                .Where(s => s.Category == category && s.GroupName == groupName);

            if (isActiveOnly)
            {
                query = query.Where(s => s.IsActive);
            }

            return query.OrderBy(s => s.DisplayOrder).ThenBy(s => s.SettingKey).ToList();
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy settings của category {category} và group {groupName}: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy settings của category {category} và group {groupName}: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Kiểm tra setting có tồn tại không
    /// </summary>
    public bool Exists(string category, string settingKey)
    {
        try
        {
            using var context = CreateNewContext();
            return context.Settings
                .Any(s => s.Category == category && s.SettingKey == settingKey);
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi kiểm tra tồn tại setting {category}.{settingKey}: {ex.Message}", ex);
            return false;
        }
    }

    #endregion

    #region ========== UPDATE OPERATIONS ==========

    /// <summary>
    /// Lưu hoặc cập nhật giá trị setting
    /// </summary>
    public void SetValue(string category, string settingKey, string settingValue, string valueType = "String", string updatedBy = null, bool encrypt = false)
    {
        try
        {
            using var context = CreateNewContext();

            var setting = context.Settings
                .FirstOrDefault(s => s.Category == category && s.SettingKey == settingKey);

            var valueToSave = settingValue ?? string.Empty;

            // Mã hóa nếu cần
            if (encrypt && !string.IsNullOrEmpty(valueToSave))
            {
                valueToSave = VntaCrypto.Encrypt(valueToSave);
            }

            if (setting == null)
            {
                // Tạo mới
                setting = new Setting
                {
                    Category = category,
                    SettingKey = settingKey,
                    SettingValue = valueToSave,
                    ValueType = valueType,
                    IsEncrypted = encrypt,
                    IsActive = true,
                    CreatedDate = DateTime.Now,
                    CreatedBy = updatedBy
                };
                context.Settings.InsertOnSubmit(setting);
            }
            else
            {
                // Cập nhật
                setting.SettingValue = valueToSave;
                setting.ValueType = valueType;
                setting.IsEncrypted = encrypt;
                setting.UpdatedDate = DateTime.Now;
                setting.UpdatedBy = updatedBy;
            }

            context.SubmitChanges();
            _logger.Info($"Đã lưu setting {category}.{settingKey}");
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lưu setting {category}.{settingKey}: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lưu setting {category}.{settingKey}: {ex.Message}", ex);
        }
    }

    #endregion
}
