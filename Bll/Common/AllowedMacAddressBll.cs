using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using Dal.Connection;
using Dal.DataAccess.Implementations.Common;
using Dal.DataAccess.Interfaces.Common;
using Dal.DataContext;
using DTO.Common;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;

namespace Bll.Common;

/// <summary>
/// Business Logic Layer cho quản lý MAC address được phép
/// </summary>
public class AllowedMacAddressBll
{
    #region Fields

    private IAllowedMacAddressRepository _dataAccess;
    private readonly ILogger _logger;
    private readonly object _lockObject = new object();

    #endregion

    #region Constructors

    public AllowedMacAddressBll()
    {
        _logger = LoggerFactory.CreateLogger(LogCategory.BLL);
    }

    #endregion

    #region Helper Methods

    private IAllowedMacAddressRepository GetDataAccess()
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

                        _dataAccess = new AllowedMacAddressRepository(globalConnectionString);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"Lỗi khi khởi tạo AllowedMacAddressRepository: {ex.Message}", ex);
                        throw new InvalidOperationException(
                            "Không thể khởi tạo kết nối database. Vui lòng kiểm tra cấu hình database.", ex);
                    }
                }
            }
        }

        return _dataAccess;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Lấy địa chỉ MAC của máy tính hiện tại
    /// </summary>
    /// <returns>Địa chỉ MAC đầu tiên tìm thấy hoặc null</returns>
    public string GetCurrentMacAddress()
    {
        try
        {
            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces()
                .Where(ni => ni.OperationalStatus == OperationalStatus.Up &&
                             ni.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .ToList();

            foreach (var ni in networkInterfaces)
            {
                var macAddress = ni.GetPhysicalAddress();
                if (macAddress != null && macAddress.ToString() != "000000000000")
                {
                    var macString = FormatMacAddress(macAddress.ToString());
                    _logger?.Debug($"Tìm thấy MAC address: {macString}");
                    return macString;
                }
            }

            _logger?.Warning("Không tìm thấy MAC address hợp lệ");
            return null;
        }
        catch (Exception ex)
        {
            _logger?.Error($"Lỗi khi lấy MAC address: {ex.Message}", ex);
            return null;
        }
    }

    /// <summary>
    /// Lấy tất cả địa chỉ MAC của máy tính hiện tại
    /// </summary>
    /// <returns>Danh sách địa chỉ MAC</returns>
    public List<string> GetAllCurrentMacAddresses()
    {
        try
        {
            var macAddresses = new List<string>();
            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces()
                .Where(ni => ni.OperationalStatus == OperationalStatus.Up &&
                             ni.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .ToList();

            foreach (var ni in networkInterfaces)
            {
                var macAddress = ni.GetPhysicalAddress();
                if (macAddress != null && macAddress.ToString() != "000000000000")
                {
                    var macString = FormatMacAddress(macAddress.ToString());
                    macAddresses.Add(macString);
                }
            }

            _logger?.Debug($"Tìm thấy {macAddresses.Count} MAC address(es)");
            return macAddresses;
        }
        catch (Exception ex)
        {
            _logger?.Error($"Lỗi khi lấy tất cả MAC address: {ex.Message}", ex);
            return new List<string>();
        }
    }

    /// <summary>
    /// Kiểm tra MAC address hiện tại có được phép không
    /// </summary>
    /// <returns>True nếu được phép, False nếu không</returns>
    public bool IsCurrentMacAddressAllowed()
    {
        try
        {
            var currentMac = GetCurrentMacAddress();
            if (string.IsNullOrWhiteSpace(currentMac))
            {
                _logger?.Warning("Không thể lấy MAC address, từ chối truy cập");
                return false;
            }

            var isAllowed = GetDataAccess().IsMacAddressAllowed(currentMac);
            
            if (!isAllowed)
            {
                _logger?.Warning($"MAC address {currentMac} không được phép sử dụng ứng dụng");
            }
            else
            {
                _logger?.Info($"MAC address {currentMac} được phép sử dụng ứng dụng");
            }

            return isAllowed;
        }
        catch (Exception ex)
        {
            _logger?.Error($"Lỗi khi kiểm tra MAC address: {ex.Message}", ex);
            // Trong trường hợp lỗi, từ chối truy cập để đảm bảo an toàn
            return false;
        }
    }

    /// <summary>
    /// Kiểm tra một MAC address có được phép không
    /// </summary>
    /// <param name="macAddress">Địa chỉ MAC cần kiểm tra</param>
    /// <returns>True nếu được phép, False nếu không</returns>
    public bool IsMacAddressAllowed(string macAddress)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(macAddress))
                return false;

            return GetDataAccess().IsMacAddressAllowed(macAddress);
        }
        catch (Exception ex)
        {
            _logger?.Error($"Lỗi khi kiểm tra MAC address: {ex.Message}", ex);
            return false;
        }
    }

    /// <summary>
    /// Lấy tất cả MAC address được phép
    /// </summary>
    /// <returns>Danh sách MAC address</returns>
    public List<AllowedMacAddressDto> GetAll()
    {
        try
        {
            _logger?.Info("Bắt đầu lấy tất cả MAC address được phép");
            var macAddresses = GetDataAccess().GetAll();
            var dtos = macAddresses.ConvertAll(ToDto);
            _logger?.Info($"Hoàn thành lấy tất cả MAC address: {dtos.Count} MAC address(es)");
            return dtos;
        }
        catch (Exception ex)
        {
            _logger?.Error($"Lỗi khi lấy tất cả MAC address: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Tạo MAC address mới
    /// </summary>
    /// <param name="dto">AllowedMacAddressDto</param>
    /// <returns>AllowedMacAddressDto đã tạo</returns>
    public AllowedMacAddressDto Create(AllowedMacAddressDto dto)
    {
        try
        {
            _logger?.Info($"Bắt đầu tạo MAC address mới: {dto.MacAddress}");
            var entity = ToEntity(dto);
            var created = GetDataAccess().Create(entity);
            var result = ToDto(created);
            _logger?.Info($"Hoàn thành tạo MAC address mới: {result.MacAddress}");
            return result;
        }
        catch (Exception ex)
        {
            _logger?.Error($"Lỗi khi tạo MAC address: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Cập nhật MAC address
    /// </summary>
    /// <param name="dto">AllowedMacAddressDto</param>
    /// <returns>AllowedMacAddressDto đã cập nhật</returns>
    public AllowedMacAddressDto Update(AllowedMacAddressDto dto)
    {
        try
        {
            _logger?.Info($"Bắt đầu cập nhật MAC address: {dto.MacAddress}");
            var entity = ToEntity(dto);
            var updated = GetDataAccess().Update(entity);
            var result = ToDto(updated);
            _logger?.Info($"Hoàn thành cập nhật MAC address: {result.MacAddress}");
            return result;
        }
        catch (Exception ex)
        {
            _logger?.Error($"Lỗi khi cập nhật MAC address: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Xóa MAC address
    /// </summary>
    /// <param name="id">ID MAC address cần xóa</param>
    public void Delete(Guid id)
    {
        try
        {
            _logger?.Info($"Bắt đầu xóa MAC address: {id}");
            GetDataAccess().Delete(id);
            _logger?.Info($"Hoàn thành xóa MAC address: {id}");
        }
        catch (Exception ex)
        {
            _logger?.Error($"Lỗi khi xóa MAC address: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Thêm MAC address hiện tại vào danh sách được phép
    /// </summary>
    /// <param name="computerName">Tên máy tính (tùy chọn)</param>
    /// <param name="description">Mô tả (tùy chọn)</param>
    /// <param name="userId">ID người tạo</param>
    /// <returns>AllowedMacAddressDto đã tạo</returns>
    public AllowedMacAddressDto AddCurrentMacAddress(string computerName = null, string description = null, Guid? userId = null)
    {
        try
        {
            var currentMac = GetCurrentMacAddress();
            if (string.IsNullOrWhiteSpace(currentMac))
            {
                throw new InvalidOperationException("Không thể lấy MAC address của máy tính hiện tại");
            }

            if (string.IsNullOrWhiteSpace(computerName))
            {
                computerName = Environment.MachineName;
            }

            var dto = new AllowedMacAddressDto
            {
                Id = Guid.NewGuid(),
                MacAddress = currentMac,
                ComputerName = computerName,
                Description = description ?? $"MAC address của {computerName}",
                IsActive = true,
                CreateDate = DateTime.Now,
                CreateBy = userId
            };

            return Create(dto);
        }
        catch (Exception ex)
        {
            _logger?.Error($"Lỗi khi thêm MAC address hiện tại: {ex.Message}", ex);
            throw;
        }
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Format MAC address thành dạng XX-XX-XX-XX-XX-XX
    /// </summary>
    private string FormatMacAddress(string macAddress)
    {
        if (string.IsNullOrWhiteSpace(macAddress))
            return string.Empty;

        // Loại bỏ dấu gạch ngang và khoảng trắng
        macAddress = macAddress.Replace("-", "").Replace(":", "").Replace(" ", "").ToUpperInvariant();

        // Thêm dấu gạch ngang mỗi 2 ký tự
        if (macAddress.Length == 12)
        {
            return $"{macAddress.Substring(0, 2)}-{macAddress.Substring(2, 2)}-{macAddress.Substring(4, 2)}-{macAddress.Substring(6, 2)}-{macAddress.Substring(8, 2)}-{macAddress.Substring(10, 2)}";
        }

        return macAddress;
    }

    private AllowedMacAddressDto ToDto(AllowedMacAddress entity)
    {
        if (entity == null)
            return null;

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

    private AllowedMacAddress ToEntity(AllowedMacAddressDto dto)
    {
        if (dto == null)
            return null;

        return new AllowedMacAddress
        {
            Id = dto.Id,
            MacAddress = dto.MacAddress,
            ComputerName = dto.ComputerName,
            Description = dto.Description,
            IsActive = dto.IsActive,
            CreateDate = dto.CreateDate,
            CreateBy = dto.CreateBy,
            ModifiedDate = dto.ModifiedDate,
            ModifiedBy = dto.ModifiedBy
        };
    }

    #endregion
}
