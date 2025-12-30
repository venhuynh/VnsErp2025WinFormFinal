using Dal.DataAccess.Interfaces.VersionAndUserManagementDal;
using Dal.DataContext;
using Dal.Exceptions;
using DTO.VersionAndUserManagementDto;
using Logger;
using Logger.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using CustomLogger = Logger.Interfaces.ILogger;

namespace Dal.DataAccess.Implementations.VersionAndUserManagementDal;

/// <summary>
/// Repository quản lý MAC address được phép
/// </summary>
public class AllowedMacAddressRepository : IAllowedMacAddressRepository
{
    #region Private Fields

    private readonly string _connectionString;
    private readonly CustomLogger _logger;

    #endregion

    #region Constructor

    public AllowedMacAddressRepository(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _logger = LoggerFactory.CreateLogger(LogCategory.DAL);
        _logger.Info("AllowedMacAddressRepository được khởi tạo");
    }

    #endregion

    #region ========== HELPER METHODS ==========

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

    /// <summary>
    /// Chuẩn hóa MAC address để so sánh (loại bỏ dấu gạch ngang và chuyển thành chữ hoa)
    /// </summary>
    private string NormalizeMacAddress(string macAddress)
    {
        if (string.IsNullOrWhiteSpace(macAddress))
            return string.Empty;

        return macAddress.Replace("-", "").Replace(":", "").ToUpperInvariant();
    }

    #endregion

    #region ========== READ OPERATIONS ==========

    /// <summary>
    /// Kiểm tra MAC address có được phép không
    /// </summary>
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

    /// <summary>
    /// Lấy tất cả MAC address được phép
    /// </summary>
    public List<AllowedMacAddressDto> GetAll()
    {
        try
        {
            using var context = CreateNewContext();
            var entities = context.GetTable<AllowedMacAddress>()
                .OrderBy(m => m.MacAddress)
                .ToList();
            
            _logger.Debug($"GetAll: Found {entities.Count} MAC addresses");
            
            // Map trực tiếp từ Entity sang DTO
            var dtos = entities.Select(e => new AllowedMacAddressDto
            {
                Id = e.Id,
                MacAddress = e.MacAddress,
                ComputerName = e.ComputerName,
                Description = e.Description,
                IsActive = e.IsActive,
                CreateDate = e.CreateDate,
                CreateBy = e.CreateBy,
                ModifiedDate = e.ModifiedDate,
                ModifiedBy = e.ModifiedBy
            }).ToList();
            
            return dtos;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy tất cả MAC address: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy tất cả MAC address: {ex.Message}", ex);
        }
    }

    #endregion

    #region ========== CREATE OPERATIONS ==========

    /// <summary>
    /// Tạo MAC address mới
    /// </summary>
    public AllowedMacAddressDto Create(AllowedMacAddressDto dto)
    {
        try
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            using var context = CreateNewContext();
            
            // Map trực tiếp từ DTO sang Entity
            var entity = new AllowedMacAddress
            {
                Id = dto.Id != Guid.Empty ? dto.Id : Guid.NewGuid(),
                MacAddress = dto.MacAddress,
                ComputerName = dto.ComputerName,
                Description = dto.Description,
                IsActive = dto.IsActive,
                CreateDate = dto.CreateDate != default(DateTime) ? dto.CreateDate : DateTime.Now,
                CreateBy = dto.CreateBy,
                ModifiedDate = dto.ModifiedDate,
                ModifiedBy = dto.ModifiedBy
            };

            context.GetTable<AllowedMacAddress>().InsertOnSubmit(entity);
            context.SubmitChanges();
            
            _logger.Info($"Đã tạo MAC address mới: {entity.MacAddress} (ID: {entity.Id})");
            
            // Map trực tiếp từ Entity sang DTO
            return new AllowedMacAddressDto
            {
                Id = entity.Id,
                MacAddress = entity.MacAddress,
                ComputerName = entity.ComputerName,
                Description = entity.Description,
                IsActive = entity.IsActive,
                CreateDate = entity.CreateDate,
                CreateBy = entity.CreateBy,
                ModifiedDate = entity.ModifiedDate,
                ModifiedBy = entity.ModifiedBy
            };
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi tạo MAC address: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi tạo MAC address: {ex.Message}", ex);
        }
    }

    #endregion

    #region ========== UPDATE OPERATIONS ==========

    /// <summary>
    /// Cập nhật MAC address
    /// </summary>
    public AllowedMacAddressDto Update(AllowedMacAddressDto dto)
    {
        try
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            using var context = CreateNewContext();
            var existing = context.GetTable<AllowedMacAddress>()
                .FirstOrDefault(m => m.Id == dto.Id);

            if (existing == null)
                throw new DataAccessException($"Không tìm thấy MAC address với ID: {dto.Id}");

            // Map trực tiếp từ DTO sang Entity (update existing)
            existing.MacAddress = dto.MacAddress;
            existing.ComputerName = dto.ComputerName;
            existing.Description = dto.Description;
            existing.IsActive = dto.IsActive;
            existing.ModifiedDate = DateTime.Now;
            existing.ModifiedBy = dto.ModifiedBy;

            context.SubmitChanges();
            
            _logger.Info($"Đã cập nhật MAC address: {dto.MacAddress} (ID: {dto.Id})");
            
            // Map trực tiếp từ Entity sang DTO
            return new AllowedMacAddressDto
            {
                Id = existing.Id,
                MacAddress = existing.MacAddress,
                ComputerName = existing.ComputerName,
                Description = existing.Description,
                IsActive = existing.IsActive,
                CreateDate = existing.CreateDate,
                CreateBy = existing.CreateBy,
                ModifiedDate = existing.ModifiedDate,
                ModifiedBy = existing.ModifiedBy
            };
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi cập nhật MAC address: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi cập nhật MAC address: {ex.Message}", ex);
        }
    }

    #endregion

    #region ========== DELETE OPERATIONS ==========

    /// <summary>
    /// Xóa MAC address
    /// </summary>
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

    #endregion
}
