using Dal.Connection;
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

    private readonly ILogger _logger;
    private readonly object _lockObject = new object();
    private VnsErp2025DataContext _dataContext;

    #endregion

    #region Constructors

    public ApplicationUserBll()
    {
        _logger = LoggerFactory.CreateLogger(LogCategory.BLL);
    }

    #endregion

    #region Helper Methods

    private VnsErp2025DataContext GetDataContext()
    {
        if (_dataContext == null)
        {
            lock (_lockObject)
            {
                if (_dataContext == null)
                {
                    try
                    {
                        var globalConnectionString = ApplicationStartupManager.Instance.GetGlobalConnectionString();
                        if (string.IsNullOrEmpty(globalConnectionString))
                        {
                            throw new InvalidOperationException(
                                "Không có global connection string. Ứng dụng chưa được khởi tạo hoặc chưa sẵn sàng.");
                        }

                        _dataContext = new VnsErp2025DataContext(globalConnectionString);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"Lỗi khi khởi tạo VnsErp2025DataContext: {ex.Message}", ex);
                        throw new InvalidOperationException(
                            "Không thể khởi tạo kết nối database. Vui lòng kiểm tra cấu hình database.", ex);
                    }
                }
            }
        }

        return _dataContext;
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
            var context = GetDataContext();
            var users = context.ApplicationUsers.ToList();
            return users.ToDtos();
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy danh sách người dùng: {ex.Message}", ex);
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
            var context = GetDataContext();
            var user = context.ApplicationUsers.FirstOrDefault(u => u.Id == id);
            return user?.ToDto();
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy người dùng theo ID: {ex.Message}", ex);
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
            var context = GetDataContext();
            var entity = dto.ToEntity();
            
            if (entity.Id == Guid.Empty)
            {
                entity.Id = Guid.NewGuid();
            }

            context.ApplicationUsers.InsertOnSubmit(entity);
            context.SubmitChanges();

            _logger.Info($"Đã tạo mới người dùng: {entity.UserName} (ID: {entity.Id})");
            return entity.ToDto();
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi tạo mới người dùng: {ex.Message}", ex);
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
            var context = GetDataContext();
            var existingEntity = context.ApplicationUsers.FirstOrDefault(u => u.Id == dto.Id);
            
            if (existingEntity == null)
            {
                throw new InvalidOperationException($"Không tìm thấy người dùng với ID: {dto.Id}");
            }

            var updatedEntity = dto.ToEntity(existingEntity);
            context.SubmitChanges();

            _logger.Info($"Đã cập nhật người dùng: {updatedEntity.UserName} (ID: {updatedEntity.Id})");
            return updatedEntity.ToDto();
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi cập nhật người dùng: {ex.Message}", ex);
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
            var context = GetDataContext();
            var entity = context.ApplicationUsers.FirstOrDefault(u => u.Id == id);
            
            if (entity == null)
            {
                throw new InvalidOperationException($"Không tìm thấy người dùng với ID: {id}");
            }

            context.ApplicationUsers.DeleteOnSubmit(entity);
            context.SubmitChanges();

            _logger.Info($"Đã xóa người dùng: {entity.UserName} (ID: {id})");
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi xóa người dùng: {ex.Message}", ex);
            throw;
        }
    }

    #endregion
}
