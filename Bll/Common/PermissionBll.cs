using Dal.Connection;
using Dal.DataAccess.Interfaces.VersionAndUserManagementDal;
using Dal.DataContext;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bll.Common;

/// <summary>
/// Business Logic Layer cho hệ thống quản lý quyền truy cập (Permission Management System)
/// </summary>
public class PermissionBll
{
    #region Fields

    private IPermissionRepository _dataAccess;
    private readonly ILogger _logger;
    private readonly object _lockObject = new object();

    #endregion

    #region Constructors

    public PermissionBll()
    {
        _logger = LoggerFactory.CreateLogger(LogCategory.BLL);
    }

    #endregion

    #region Helper Methods

    private IPermissionRepository GetDataAccess()
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

                        _dataAccess = new Dal.DataAccess.Implementations.VersionAndUserManagementDal.PermissionRepository(globalConnectionString);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"Lỗi khi khởi tạo PermissionRepository: {ex.Message}", ex);
                        throw new InvalidOperationException(
                            "Không thể khởi tạo kết nối database. Vui lòng kiểm tra cấu hình database.", ex);
                    }
                }
            }
        }

        return _dataAccess;
    }

    #endregion

    #region Permission Management

    /// <summary>
    /// Kiểm tra user có quyền thực hiện action trên entity không
    /// </summary>
    public bool HasPermission(Guid userId, string entityName, string action)
    {
        try
        {
            if (userId == Guid.Empty)
                return false;

            if (string.IsNullOrWhiteSpace(entityName) || string.IsNullOrWhiteSpace(action))
                return false;

            return GetDataAccess().HasPermission(userId, entityName, action);
        }
        catch (Exception ex)
        {
            _logger?.Error($"Lỗi khi kiểm tra quyền: {ex.Message}", ex);
            return false; // Deny by default
        }
    }

    /// <summary>
    /// Kiểm tra quyền cho user hiện tại (từ session/context)
    /// Lưu ý: Cần implement GetCurrentUserId() để lấy user ID từ session
    /// </summary>
    public bool HasPermission(string entityName, string action)
    {
        var currentUserId = GetCurrentUserId();
        if (currentUserId == Guid.Empty)
        {
            _logger?.Warning("Không thể lấy User ID hiện tại");
            return false;
        }

        return HasPermission(currentUserId, entityName, action);
    }

    /// <summary>
    /// Lấy tất cả quyền của user
    /// </summary>
    public List<Permission> GetUserPermissions(Guid userId)
    {
        try
        {
            if (userId == Guid.Empty)
                return new List<Permission>();

            return GetDataAccess().GetUserPermissions(userId);
        }
        catch (Exception ex)
        {
            _logger?.Error($"Lỗi khi lấy quyền của user: {ex.Message}", ex);
            return new List<Permission>();
        }
    }

    /// <summary>
    /// Lấy quyền của user theo entity
    /// </summary>
    public List<Permission> GetUserPermissionsByEntity(Guid userId, string entityName)
    {
        try
        {
            if (userId == Guid.Empty || string.IsNullOrWhiteSpace(entityName))
                return new List<Permission>();

            return GetDataAccess().GetUserPermissionsByEntity(userId, entityName);
        }
        catch (Exception ex)
        {
            _logger?.Error($"Lỗi khi lấy quyền theo entity: {ex.Message}", ex);
            return new List<Permission>();
        }
    }

    /// <summary>
    /// Kiểm tra user có quyền Read trên entity
    /// </summary>
    public bool CanRead(Guid userId, string entityName)
    {
        return HasPermission(userId, entityName, "Read");
    }

    /// <summary>
    /// Kiểm tra user có quyền Create trên entity
    /// </summary>
    public bool CanCreate(Guid userId, string entityName)
    {
        return HasPermission(userId, entityName, "Create");
    }

    /// <summary>
    /// Kiểm tra user có quyền Update trên entity
    /// </summary>
    public bool CanUpdate(Guid userId, string entityName)
    {
        return HasPermission(userId, entityName, "Update");
    }

    /// <summary>
    /// Kiểm tra user có quyền Delete trên entity
    /// </summary>
    public bool CanDelete(Guid userId, string entityName)
    {
        return HasPermission(userId, entityName, "Delete");
    }

    /// <summary>
    /// Kiểm tra user có quyền Approve trên entity
    /// </summary>
    public bool CanApprove(Guid userId, string entityName)
    {
        return HasPermission(userId, entityName, "Approve");
    }

    #endregion

    #region Role Management

    /// <summary>
    /// Lấy tất cả roles
    /// </summary>
    public List<Role> GetAllRoles()
    {
        try
        {
            return GetDataAccess().GetAllRoles();
        }
        catch (Exception ex)
        {
            _logger?.Error($"Lỗi khi lấy tất cả roles: {ex.Message}", ex);
            return new List<Role>();
        }
    }

    /// <summary>
    /// Lấy role theo ID
    /// </summary>
    public Role GetRoleById(Guid id)
    {
        try
        {
            return GetDataAccess().GetRoleById(id);
        }
        catch (Exception ex)
        {
            _logger?.Error($"Lỗi khi lấy role: {ex.Message}", ex);
            return null;
        }
    }

    /// <summary>
    /// Lấy role theo tên
    /// </summary>
    public Role GetRoleByName(string name)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            return GetDataAccess().GetRoleByName(name);
        }
        catch (Exception ex)
        {
            _logger?.Error($"Lỗi khi lấy role theo tên: {ex.Message}", ex);
            return null;
        }
    }

    /// <summary>
    /// Tạo role mới
    /// </summary>
    public Role CreateRole(Role role)
    {
        try
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            _logger?.Info($"Tạo role mới: {role.Name}");
            return GetDataAccess().CreateRole(role);
        }
        catch (Exception ex)
        {
            _logger?.Error($"Lỗi khi tạo role: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Cập nhật role
    /// </summary>
    public Role UpdateRole(Role role)
    {
        try
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            _logger?.Info($"Cập nhật role: {role.Name}");
            return GetDataAccess().UpdateRole(role);
        }
        catch (Exception ex)
        {
            _logger?.Error($"Lỗi khi cập nhật role: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Xóa role
    /// </summary>
    public void DeleteRole(Guid id)
    {
        try
        {
            _logger?.Info($"Xóa role: {id}");
            GetDataAccess().DeleteRole(id);
        }
        catch (Exception ex)
        {
            _logger?.Error($"Lỗi khi xóa role: {ex.Message}", ex);
            throw;
        }
    }

    #endregion

    #region UserRole Management

    /// <summary>
    /// Gán role cho user
    /// </summary>
    public void AssignRoleToUser(Guid userId, Guid roleId, Guid? assignedBy = null)
    {
        try
        {
            _logger?.Info($"Gán role {roleId} cho user {userId}");
            GetDataAccess().AssignRoleToUser(userId, roleId, assignedBy);
        }
        catch (Exception ex)
        {
            _logger?.Error($"Lỗi khi gán role cho user: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Gỡ role khỏi user
    /// </summary>
    public void RemoveRoleFromUser(Guid userId, Guid roleId)
    {
        try
        {
            _logger?.Info($"Gỡ role {roleId} khỏi user {userId}");
            GetDataAccess().RemoveRoleFromUser(userId, roleId);
        }
        catch (Exception ex)
        {
            _logger?.Error($"Lỗi khi gỡ role khỏi user: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Lấy tất cả roles của user
    /// </summary>
    public List<Role> GetUserRoles(Guid userId)
    {
        try
        {
            if (userId == Guid.Empty)
                return new List<Role>();

            return GetDataAccess().GetUserRoles(userId);
        }
        catch (Exception ex)
        {
            _logger?.Error($"Lỗi khi lấy roles của user: {ex.Message}", ex);
            return new List<Role>();
        }
    }

    /// <summary>
    /// Kiểm tra user có role không
    /// </summary>
    public bool UserHasRole(Guid userId, Guid roleId)
    {
        try
        {
            if (userId == Guid.Empty || roleId == Guid.Empty)
                return false;

            return GetDataAccess().UserHasRole(userId, roleId);
        }
        catch (Exception ex)
        {
            _logger?.Error($"Lỗi khi kiểm tra user có role: {ex.Message}", ex);
            return false;
        }
    }

    /// <summary>
    /// Kiểm tra user có role theo tên không
    /// </summary>
    public bool UserHasRole(Guid userId, string roleName)
    {
        try
        {
            if (userId == Guid.Empty || string.IsNullOrWhiteSpace(roleName))
                return false;

            var role = GetRoleByName(roleName);
            if (role == null)
                return false;

            return UserHasRole(userId, role.Id);
        }
        catch (Exception ex)
        {
            _logger?.Error($"Lỗi khi kiểm tra user có role: {ex.Message}", ex);
            return false;
        }
    }

    #endregion

    #region RolePermission Management

    /// <summary>
    /// Gán permission cho role
    /// </summary>
    public void AssignPermissionToRole(Guid roleId, Guid permissionId, bool isGranted = true)
    {
        try
        {
            _logger?.Info($"Gán permission {permissionId} cho role {roleId}");
            GetDataAccess().AssignPermissionToRole(roleId, permissionId, isGranted);
        }
        catch (Exception ex)
        {
            _logger?.Error($"Lỗi khi gán permission cho role: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Gỡ permission khỏi role
    /// </summary>
    public void RemovePermissionFromRole(Guid roleId, Guid permissionId)
    {
        try
        {
            _logger?.Info($"Gỡ permission {permissionId} khỏi role {roleId}");
            GetDataAccess().RemovePermissionFromRole(roleId, permissionId);
        }
        catch (Exception ex)
        {
            _logger?.Error($"Lỗi khi gỡ permission khỏi role: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Lấy tất cả permissions của role
    /// </summary>
    public List<Permission> GetRolePermissions(Guid roleId)
    {
        try
        {
            if (roleId == Guid.Empty)
                return new List<Permission>();

            return GetDataAccess().GetRolePermissions(roleId);
        }
        catch (Exception ex)
        {
            _logger?.Error($"Lỗi khi lấy permissions của role: {ex.Message}", ex);
            return new List<Permission>();
        }
    }

    /// <summary>
    /// Gán nhiều permissions cho role
    /// </summary>
    public void AssignPermissionsToRole(Guid roleId, List<Guid> permissionIds, bool isGranted = true)
    {
        try
        {
            _logger?.Info($"Gán {permissionIds.Count} permissions cho role {roleId}");
            GetDataAccess().AssignPermissionsToRole(roleId, permissionIds, isGranted);
        }
        catch (Exception ex)
        {
            _logger?.Error($"Lỗi khi gán permissions cho role: {ex.Message}", ex);
            throw;
        }
    }

    #endregion

    #region Permission CRUD

    /// <summary>
    /// Lấy tất cả permissions
    /// </summary>
    public List<Permission> GetAllPermissions()
    {
        try
        {
            return GetDataAccess().GetAllPermissions();
        }
        catch (Exception ex)
        {
            _logger?.Error($"Lỗi khi lấy tất cả permissions: {ex.Message}", ex);
            return new List<Permission>();
        }
    }

    /// <summary>
    /// Lấy permissions theo entity
    /// </summary>
    public List<Permission> GetPermissionsByEntity(string entityName)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(entityName))
                return new List<Permission>();

            return GetDataAccess().GetPermissionsByEntity(entityName);
        }
        catch (Exception ex)
        {
            _logger?.Error($"Lỗi khi lấy permissions theo entity: {ex.Message}", ex);
            return new List<Permission>();
        }
    }

    /// <summary>
    /// Lấy permission theo EntityName và Action
    /// </summary>
    public Permission GetPermission(string entityName, string action)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(entityName) || string.IsNullOrWhiteSpace(action))
                return null;

            return GetDataAccess().GetPermission(entityName, action);
        }
        catch (Exception ex)
        {
            _logger?.Error($"Lỗi khi lấy permission: {ex.Message}", ex);
            return null;
        }
    }

    #endregion

    #region Private Helper Methods

    /// <summary>
    /// Lấy User ID hiện tại từ session/context
    /// TODO: Implement method này để lấy user ID từ session hoặc context hiện tại
    /// </summary>
    private Guid GetCurrentUserId()
    {
        // TODO: Implement logic để lấy user ID từ session/context
        // Ví dụ:
        // return SessionManager.CurrentUser?.Id ?? Guid.Empty;
        // hoặc
        // return ApplicationContext.CurrentUserId;
        
        return Guid.Empty; // Tạm thời return Empty, cần implement sau
    }

    #endregion
}
