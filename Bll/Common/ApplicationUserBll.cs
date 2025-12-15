using Dal.Connection;
using Dal.DataAccess.Interfaces;
using Dal.DataContext;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using DTO.VersionAndUserManagementDto;

namespace Bll.Common;

/// <summary>
/// Business Logic Layer cho quản lý người dùng ứng dụng
/// </summary>
public class ApplicationUserBll
{
    #region Fields

    private IApplicationUserRepository _dataAccess;
    private readonly ILogger _logger;
    private readonly object _lockObject = new object();

    #endregion

    #region Constructors

    public ApplicationUserBll()
    {
        _logger = LoggerFactory.CreateLogger(LogCategory.BLL);
    }

    #endregion

    #region Helper Methods

    private IApplicationUserRepository GetDataAccess()
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

                        _dataAccess = new Dal.DataAccess.Implementations.ApplicationUserRepository(globalConnectionString);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"Lỗi khi khởi tạo ApplicationUserRepository: {ex.Message}", ex);
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
    /// Lấy tất cả người dùng
    /// </summary>
    /// <returns>Danh sách ApplicationUserDto</returns>
    public List<ApplicationUserDto> GetAll()
    {
        try
        {
            _logger?.Info("Bắt đầu lấy tất cả người dùng");
            var users = GetDataAccess().GetAll();
            
            // Load Employee info và convert sang DTO
            var dtos = users.Select(LoadEmployeeInfoAndConvertToDto).ToList();
            
            _logger?.Info($"Hoàn thành lấy tất cả người dùng: {dtos.Count} user(s)");
            return dtos;
        }
        catch (Exception ex)
        {
            _logger?.Error($"Lỗi khi lấy tất cả người dùng: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Lấy người dùng theo ID
    /// </summary>
    /// <param name="id">ID người dùng</param>
    /// <returns>ApplicationUserDto hoặc null</returns>
    public ApplicationUserDto GetById(Guid id)
    {
        try
        {
            _logger?.Info($"Bắt đầu lấy người dùng theo ID: {id}");
            var user = GetDataAccess().GetById(id);
            
            // Load Employee info và convert sang DTO
            var dto = LoadEmployeeInfoAndConvertToDto(user);
            
            _logger?.Info($"Hoàn thành lấy người dùng theo ID: {(dto != null ? dto.UserName : "not found")}");
            return dto;
        }
        catch (Exception ex)
        {
            _logger?.Error($"Lỗi khi lấy người dùng theo ID: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Tạo mới người dùng
    /// </summary>
    /// <param name="dto">ApplicationUserDto</param>
    /// <returns>ApplicationUserDto đã được tạo</returns>
    public ApplicationUserDto Create(ApplicationUserDto dto)
        {
            try
            {
                _logger?.Info($"Bắt đầu tạo người dùng mới: {dto.UserName}");
                var entity = dto.ToEntity();
                var created = GetDataAccess().Create(entity);
                
                // Load Employee và navigation properties trước khi convert sang DTO
                var result = LoadEmployeeInfoAndConvertToDto(created);
                
                _logger?.Info($"Hoàn thành tạo người dùng mới: {result.UserName}");
                return result;
            }
            catch (Exception ex)
            {
                _logger?.Error($"Lỗi khi tạo người dùng: {ex.Message}", ex);
                throw;
            }
        }

    /// <summary>
    /// Cập nhật người dùng
    /// </summary>
    /// <param name="dto">ApplicationUserDto</param>
    /// <returns>ApplicationUserDto đã được cập nhật</returns>
    public ApplicationUserDto Update(ApplicationUserDto dto)
        {
            try
            {
                _logger?.Info($"Bắt đầu cập nhật người dùng: {dto.UserName}");
                var entity = dto.ToEntity();
                var updated = GetDataAccess().Update(entity);
                
                // Load Employee và navigation properties trước khi convert sang DTO
                var result = LoadEmployeeInfoAndConvertToDto(updated);
                
                _logger?.Info($"Hoàn thành cập nhật người dùng: {result.UserName}");
                return result;
            }
            catch (Exception ex)
            {
                _logger?.Error($"Lỗi khi cập nhật người dùng: {ex.Message}", ex);
                throw;
            }
        }

    /// <summary>
    /// Xóa người dùng
    /// </summary>
    /// <param name="id">ID người dùng</param>
    public void Delete(Guid id)
    {
        try
        {
            _logger?.Info($"Bắt đầu xóa người dùng: {id}");
            GetDataAccess().Delete(id);
            _logger?.Info($"Hoàn thành xóa người dùng: {id}");
        }
        catch (Exception ex)
        {
            _logger?.Error($"Lỗi khi xóa người dùng: {ex.Message}", ex);
            throw;
        }
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Load Employee và navigation properties, sau đó convert sang DTO
    /// Tránh ObjectDisposedException khi truy cập navigation properties sau khi DataContext dispose
    /// </summary>
    private ApplicationUserDto LoadEmployeeInfoAndConvertToDto(ApplicationUser entity)
    {
        if (entity == null)
            return null;

        var dto = entity.ToDto();

        // Nếu ToDto() không load được Employee (DataContext đã dispose), load lại bằng DataContext mới
        if (entity.EmployeeId.HasValue && string.IsNullOrEmpty(dto.EmployeeCode))
        {
            try
            {
                var connectionString = ApplicationStartupManager.Instance.GetGlobalConnectionString();
                using var context = new VnsErp2025DataContext(connectionString);
                
                var employee = context.Employees.FirstOrDefault(e => e.Id == entity.EmployeeId.Value);
                if (employee != null)
                {
                    dto.EmployeeCode = employee.EmployeeCode;
                    dto.EmployeeFullName = employee.FullName;

                    // Load Department
                    if (employee.DepartmentId.HasValue)
                    {
                        var department = context.Departments.FirstOrDefault(d => d.Id == employee.DepartmentId.Value);
                        if (department != null)
                        {
                            dto.DepartmentName = department.DepartmentName;
                        }
                    }

                    // Load Position
                    if (employee.PositionId.HasValue)
                    {
                        var position = context.Positions.FirstOrDefault(p => p.Id == employee.PositionId.Value);
                        if (position != null)
                        {
                            dto.PositionName = position.PositionName;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger?.Warning($"Không thể load thông tin Employee cho user {entity.UserName}: {ex.Message}");
            }
        }

        return dto;
    }

    #endregion
}
