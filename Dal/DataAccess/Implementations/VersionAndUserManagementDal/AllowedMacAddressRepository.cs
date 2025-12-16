using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Threading.Tasks;
using Dal.DataAccess.Interfaces.VersionAndUserManagementDal;
using Dal.DataContext;
using Dal.Exceptions;
using Logger;
using Logger.Configuration;
using CustomLogger = Logger.Interfaces.ILogger;

namespace Dal.DataAccess.Implementations.VersionAndUserManagementDal;

/// <summary>
/// Repository quản lý MAC address được phép
/// </summary>
public class AllowedMacAddressRepository : IAllowedMacAddressRepository
{
    private readonly string _connectionString;
    private readonly CustomLogger _logger;

    public AllowedMacAddressRepository(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _logger = LoggerFactory.CreateLogger(LogCategory.DAL);
        _logger.Info("AllowedMacAddressRepository được khởi tạo");
    }

    private VnsErp2025DataContext CreateNewContext()
    {
        var context = new VnsErp2025DataContext(_connectionString);
        var loadOptions = new DataLoadOptions();
        context.LoadOptions = loadOptions;
        return context;
    }

    /// <summary>
    /// Chuẩn hóa MAC address để so sánh (loại bỏ dấu gạch ngang và chuyển thành chữ hoa)
    /// </summary>
    private string NormalizeMacAddress(string macAddress)
    {
        if (string.IsNullOrWhiteSpace(macAddress))
            return string.Empty;

        return macAddress.Replace("-", "").Replace(":", "").ToUpperInvariant();
    }

    public bool IsMacAddressAllowed(string macAddress)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(macAddress))
                return false;

            var normalizedMac = NormalizeMacAddress(macAddress);
            
            using var context = CreateNewContext();
            // Lấy tất cả MAC address đang active từ database trước
            var allowedMacs = context.GetTable<AllowedMacAddress>()
                .Where(m => m.IsActive)
                .ToList();
            
            // Sau đó normalize và so sánh trong memory
            var allowed = allowedMacs.Any(m => NormalizeMacAddress(m.MacAddress) == normalizedMac);
            
            _logger.Debug($"IsMacAddressAllowed: MAC {macAddress} is {(allowed ? "allowed" : "not allowed")}");
            return allowed;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi kiểm tra MAC address: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi kiểm tra MAC address: {ex.Message}", ex);
        }
    }

    public async Task<bool> IsMacAddressAllowedAsync(string macAddress)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(macAddress))
                return false;

            var normalizedMac = NormalizeMacAddress(macAddress);
            
            using var context = CreateNewContext();
            var allowedMacs = await Task.Run(() => 
                context.GetTable<AllowedMacAddress>()
                    .Where(m => m.IsActive)
                    .ToList());
            
            var allowed = allowedMacs.Any(m => NormalizeMacAddress(m.MacAddress) == normalizedMac);
            _logger.Debug($"IsMacAddressAllowedAsync: MAC {macAddress} is {(allowed ? "allowed" : "not allowed")}");
            return allowed;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi kiểm tra MAC address (async): {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi kiểm tra MAC address: {ex.Message}", ex);
        }
    }

    public List<AllowedMacAddress> GetAll()
    {
        try
        {
            using var context = CreateNewContext();
            var macAddresses = context.GetTable<AllowedMacAddress>()
                .OrderBy(m => m.MacAddress)
                .ToList();
            
            _logger.Debug($"GetAll: Found {macAddresses.Count} MAC addresses");
            return macAddresses;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy tất cả MAC address: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy tất cả MAC address: {ex.Message}", ex);
        }
    }

    public async Task<List<AllowedMacAddress>> GetAllAsync()
    {
        try
        {
            using var context = CreateNewContext();
            var macAddresses = await Task.Run(() => 
                context.GetTable<AllowedMacAddress>()
                    .OrderBy(m => m.MacAddress)
                    .ToList());
            
            _logger.Debug($"GetAllAsync: Found {macAddresses.Count} MAC addresses");
            return macAddresses;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy tất cả MAC address (async): {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy tất cả MAC address: {ex.Message}", ex);
        }
    }

    public AllowedMacAddress GetById(Guid id)
    {
        try
        {
            using var context = CreateNewContext();
            var macAddress = context.GetTable<AllowedMacAddress>()
                .FirstOrDefault(m => m.Id == id);
            
            _logger.Debug($"GetById: {(macAddress != null ? $"Found MAC {macAddress.MacAddress}" : "MAC address not found")}");
            return macAddress;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy MAC address theo ID: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy MAC address theo ID: {ex.Message}", ex);
        }
    }

    public async Task<AllowedMacAddress> GetByIdAsync(Guid id)
    {
        try
        {
            using var context = CreateNewContext();
            var macAddresses = await Task.Run(() => 
                context.GetTable<AllowedMacAddress>()
                    .Where(m => m.Id == id)
                    .ToList());
            
            var macAddress = macAddresses.FirstOrDefault();
            _logger.Debug($"GetByIdAsync: {(macAddress != null ? $"Found MAC {macAddress.MacAddress}" : "MAC address not found")}");
            return macAddress;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy MAC address theo ID (async): {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy MAC address theo ID: {ex.Message}", ex);
        }
    }

    public AllowedMacAddress Create(AllowedMacAddress macAddress)
    {
        try
        {
            if (macAddress == null)
                throw new ArgumentNullException(nameof(macAddress));

            if (macAddress.Id == Guid.Empty)
                macAddress.Id = Guid.NewGuid();

            if (macAddress.CreateDate == default(DateTime))
                macAddress.CreateDate = DateTime.Now;

            using var context = CreateNewContext();
            context.GetTable<AllowedMacAddress>().InsertOnSubmit(macAddress);
            context.SubmitChanges();
            
            _logger.Info($"Đã tạo MAC address mới: {macAddress.MacAddress} (ID: {macAddress.Id})");
            return macAddress;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi tạo MAC address: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi tạo MAC address: {ex.Message}", ex);
        }
    }

    public async Task<AllowedMacAddress> CreateAsync(AllowedMacAddress macAddress)
    {
        return await Task.Run(() => Create(macAddress));
    }

    public AllowedMacAddress Update(AllowedMacAddress macAddress)
    {
        try
        {
            if (macAddress == null)
                throw new ArgumentNullException(nameof(macAddress));

            using var context = CreateNewContext();
            var existing = context.GetTable<AllowedMacAddress>()
                .FirstOrDefault(m => m.Id == macAddress.Id);

            if (existing == null)
                throw new DataAccessException($"Không tìm thấy MAC address với ID: {macAddress.Id}");

            existing.MacAddress = macAddress.MacAddress;
            existing.ComputerName = macAddress.ComputerName;
            existing.Description = macAddress.Description;
            existing.IsActive = macAddress.IsActive;
            existing.ModifiedDate = DateTime.Now;
            existing.ModifiedBy = macAddress.ModifiedBy;

            context.SubmitChanges();
            
            _logger.Info($"Đã cập nhật MAC address: {macAddress.MacAddress} (ID: {macAddress.Id})");
            return existing;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi cập nhật MAC address: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi cập nhật MAC address: {ex.Message}", ex);
        }
    }

    public async Task<AllowedMacAddress> UpdateAsync(AllowedMacAddress macAddress)
    {
        return await Task.Run(() => Update(macAddress));
    }

    public void Delete(Guid id)
    {
        try
        {
            using var context = CreateNewContext();
            var macAddress = context.GetTable<AllowedMacAddress>()
                .FirstOrDefault(m => m.Id == id);

            if (macAddress == null)
                throw new DataAccessException($"Không tìm thấy MAC address với ID: {id}");

            context.GetTable<AllowedMacAddress>().DeleteOnSubmit(macAddress);
            context.SubmitChanges();
            
            _logger.Info($"Đã xóa MAC address: {macAddress.MacAddress} (ID: {id})");
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi xóa MAC address: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi xóa MAC address: {ex.Message}", ex);
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        await Task.Run(() => Delete(id));
    }
}
