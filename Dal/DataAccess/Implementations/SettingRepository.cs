using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Threading.Tasks;
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

    #region ISettingRepository Implementation

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

    public async Task<string> GetValueAsync(string category, string settingKey, string defaultValue = "")
    {
        try
        {
            using var context = CreateNewContext();
            var settings = await Task.Run(() =>
                context.Settings
                    .Where(s => s.Category == category && s.SettingKey == settingKey && s.IsActive)
                    .ToList());

            var setting = settings.FirstOrDefault();
            return setting?.SettingValue ?? defaultValue;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy giá trị setting {category}.{settingKey} (async): {ex.Message}", ex);
            return defaultValue;
        }
    }

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

    public async Task<string> GetDecryptedValueAsync(string category, string settingKey, string defaultValue = "")
    {
        try
        {
            using var context = CreateNewContext();
            var settings = await Task.Run(() =>
                context.Settings
                    .Where(s => s.Category == category && s.SettingKey == settingKey && s.IsActive)
                    .ToList());

            var setting = settings.FirstOrDefault();
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
                }
            }

            return value;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy và giải mã giá trị setting {category}.{settingKey} (async): {ex.Message}", ex);
            return defaultValue;
        }
    }

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

    public async Task SetValueAsync(string category, string settingKey, string settingValue, string valueType = "String", string updatedBy = null, bool encrypt = false)
    {
        try
        {
            using var context = CreateNewContext();

            var settings = await Task.Run(() =>
                context.Settings
                    .Where(s => s.Category == category && s.SettingKey == settingKey)
                    .ToList());

            var setting = settings.FirstOrDefault();

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

            await Task.Run(() => context.SubmitChanges());
            _logger.Info($"Đã lưu setting {category}.{settingKey} (async)");
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lưu setting {category}.{settingKey} (async): {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lưu setting {category}.{settingKey}: {ex.Message}", ex);
        }
    }

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

    public async Task<Setting> GetByCategoryAndKeyAsync(string category, string settingKey)
    {
        try
        {
            using var context = CreateNewContext();
            var settings = await Task.Run(() =>
                context.Settings
                    .Where(s => s.Category == category && s.SettingKey == settingKey)
                    .ToList());

            return settings.FirstOrDefault();
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy setting {category}.{settingKey} (async): {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy setting {category}.{settingKey}: {ex.Message}", ex);
        }
    }

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

    public async Task<List<Setting>> GetAllByCategoryAsync(string category, bool isActiveOnly = true)
    {
        try
        {
            using var context = CreateNewContext();
            var query = context.Settings.Where(s => s.Category == category);

            if (isActiveOnly)
            {
                query = query.Where(s => s.IsActive);
            }

            return await Task.Run(() =>
                query.OrderBy(s => s.DisplayOrder).ThenBy(s => s.SettingKey).ToList());
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy tất cả settings của category {category} (async): {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy tất cả settings của category {category}: {ex.Message}", ex);
        }
    }

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

    public async Task<List<Setting>> GetByCategoryAndGroupAsync(string category, string groupName, bool isActiveOnly = true)
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

            return await Task.Run(() =>
                query.OrderBy(s => s.DisplayOrder).ThenBy(s => s.SettingKey).ToList());
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy settings của category {category} và group {groupName} (async): {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy settings của category {category} và group {groupName}: {ex.Message}", ex);
        }
    }

    public Setting Create(Setting setting)
    {
        try
        {
            using var context = CreateNewContext();

            if (setting.CreatedDate == default(DateTime))
            {
                setting.CreatedDate = DateTime.Now;
            }

            // Mã hóa nếu cần
            if (setting.IsEncrypted && !string.IsNullOrEmpty(setting.SettingValue))
            {
                setting.SettingValue = VntaCrypto.Encrypt(setting.SettingValue);
            }

            context.Settings.InsertOnSubmit(setting);
            context.SubmitChanges();

            _logger.Info($"Đã tạo setting {setting.Category}.{setting.SettingKey}");
            return setting;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi tạo setting: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi tạo setting: {ex.Message}", ex);
        }
    }

    public async Task<Setting> CreateAsync(Setting setting)
    {
        try
        {
            using var context = CreateNewContext();

            if (setting.CreatedDate == default(DateTime))
            {
                setting.CreatedDate = DateTime.Now;
            }

            // Mã hóa nếu cần
            if (setting.IsEncrypted && !string.IsNullOrEmpty(setting.SettingValue))
            {
                setting.SettingValue = VntaCrypto.Encrypt(setting.SettingValue);
            }

            context.Settings.InsertOnSubmit(setting);
            await Task.Run(() => context.SubmitChanges());

            _logger.Info($"Đã tạo setting {setting.Category}.{setting.SettingKey} (async)");
            return setting;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi tạo setting (async): {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi tạo setting: {ex.Message}", ex);
        }
    }

    public Setting Update(Setting setting)
    {
        try
        {
            using var context = CreateNewContext();

            var existingSetting = context.Settings
                .FirstOrDefault(s => s.SettingId == setting.SettingId);

            if (existingSetting == null)
            {
                throw new DataAccessException($"Không tìm thấy setting với ID {setting.SettingId}");
            }

            // Cập nhật các thuộc tính
            existingSetting.Category = setting.Category;
            existingSetting.SettingKey = setting.SettingKey;
            existingSetting.SettingValue = setting.SettingValue;
            existingSetting.ValueType = setting.ValueType;
            existingSetting.Description = setting.Description;
            existingSetting.IsEncrypted = setting.IsEncrypted;
            existingSetting.IsSystem = setting.IsSystem;
            existingSetting.IsActive = setting.IsActive;
            existingSetting.GroupName = setting.GroupName;
            existingSetting.DisplayOrder = setting.DisplayOrder;
            existingSetting.UpdatedDate = DateTime.Now;
            existingSetting.UpdatedBy = setting.UpdatedBy;

            // Mã hóa nếu cần
            if (existingSetting.IsEncrypted && !string.IsNullOrEmpty(existingSetting.SettingValue))
            {
                // Chỉ mã hóa nếu chưa được mã hóa (kiểm tra prefix)
                if (!VntaCrypto.IsEncrypted(existingSetting.SettingValue))
                {
                    existingSetting.SettingValue = VntaCrypto.Encrypt(existingSetting.SettingValue);
                }
            }

            context.SubmitChanges();

            _logger.Info($"Đã cập nhật setting {existingSetting.Category}.{existingSetting.SettingKey}");
            return existingSetting;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi cập nhật setting: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi cập nhật setting: {ex.Message}", ex);
        }
    }

    public async Task<Setting> UpdateAsync(Setting setting)
    {
        try
        {
            using var context = CreateNewContext();

            var settings = await Task.Run(() =>
                context.Settings
                    .Where(s => s.SettingId == setting.SettingId)
                    .ToList());

            var existingSetting = settings.FirstOrDefault();

            if (existingSetting == null)
            {
                throw new DataAccessException($"Không tìm thấy setting với ID {setting.SettingId}");
            }

            // Cập nhật các thuộc tính
            existingSetting.Category = setting.Category;
            existingSetting.SettingKey = setting.SettingKey;
            existingSetting.SettingValue = setting.SettingValue;
            existingSetting.ValueType = setting.ValueType;
            existingSetting.Description = setting.Description;
            existingSetting.IsEncrypted = setting.IsEncrypted;
            existingSetting.IsSystem = setting.IsSystem;
            existingSetting.IsActive = setting.IsActive;
            existingSetting.GroupName = setting.GroupName;
            existingSetting.DisplayOrder = setting.DisplayOrder;
            existingSetting.UpdatedDate = DateTime.Now;
            existingSetting.UpdatedBy = setting.UpdatedBy;

            // Mã hóa nếu cần
            if (existingSetting.IsEncrypted && !string.IsNullOrEmpty(existingSetting.SettingValue))
            {
                if (!VntaCrypto.IsEncrypted(existingSetting.SettingValue))
                {
                    existingSetting.SettingValue = VntaCrypto.Encrypt(existingSetting.SettingValue);
                }
            }

            await Task.Run(() => context.SubmitChanges());

            _logger.Info($"Đã cập nhật setting {existingSetting.Category}.{existingSetting.SettingKey} (async)");
            return existingSetting;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi cập nhật setting (async): {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi cập nhật setting: {ex.Message}", ex);
        }
    }

    public void Delete(int settingId)
    {
        try
        {
            using var context = CreateNewContext();

            var setting = context.Settings.FirstOrDefault(s => s.SettingId == settingId);
            if (setting == null)
            {
                throw new DataAccessException($"Không tìm thấy setting với ID {settingId}");
            }

            context.Settings.DeleteOnSubmit(setting);
            context.SubmitChanges();

            _logger.Info($"Đã xóa setting với ID {settingId}");
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi xóa setting với ID {settingId}: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi xóa setting: {ex.Message}", ex);
        }
    }

    public async Task DeleteAsync(int settingId)
    {
        try
        {
            using var context = CreateNewContext();

            var settings = await Task.Run(() =>
                context.Settings
                    .Where(s => s.SettingId == settingId)
                    .ToList());

            var setting = settings.FirstOrDefault();
            if (setting == null)
            {
                throw new DataAccessException($"Không tìm thấy setting với ID {settingId}");
            }

            context.Settings.DeleteOnSubmit(setting);
            await Task.Run(() => context.SubmitChanges());

            _logger.Info($"Đã xóa setting với ID {settingId} (async)");
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi xóa setting với ID {settingId} (async): {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi xóa setting: {ex.Message}", ex);
        }
    }

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

    public async Task<bool> ExistsAsync(string category, string settingKey)
    {
        try
        {
            using var context = CreateNewContext();
            return await Task.Run(() =>
                context.Settings
                    .Any(s => s.Category == category && s.SettingKey == settingKey));
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi kiểm tra tồn tại setting {category}.{settingKey} (async): {ex.Message}", ex);
            return false;
        }
    }

    #endregion
}
