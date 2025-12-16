using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dal.DataContext;

namespace Dal.DataAccess.Interfaces.VersionAndUserManagementDal;

/// <summary>
/// Data Access cho hệ thống quản lý quyền truy cập (Permission Management System).
/// Cung cấp các phương thức quản lý Role, Permission, UserRole, RolePermission, UserPermission.
/// </summary>
public interface IPermissionRepository
{
    #region Permission CRUD

    /// <summary>
    /// Tạo permission mới
    /// </summary>
    Permission Create(Permission permission);

    /// <summary>
    /// Tạo permission mới (async)
    /// </summary>
    Task<Permission> CreateAsync(Permission permission);

    /// <summary>
    /// Lấy permission theo ID
    /// </summary>
    Permission GetPermissionById(Guid id);

    /// <summary>
    /// Lấy permission theo ID (async)
    /// </summary>
    Task<Permission> GetPermissionByIdAsync(Guid id);

    /// <summary>
    /// Lấy permission theo EntityName và Action
    /// </summary>
    Permission GetPermission(string entityName, string action);

    /// <summary>
    /// Lấy tất cả permissions
    /// </summary>
    List<Permission> GetAllPermissions();

    /// <summary>
    /// Lấy tất cả permissions (async)
    /// </summary>
    Task<List<Permission>> GetAllPermissionsAsync();

    /// <summary>
    /// Lấy permissions theo EntityName
    /// </summary>
    List<Permission> GetPermissionsByEntity(string entityName);

    /// <summary>
    /// Lấy permissions theo EntityName (async)
    /// </summary>
    Task<List<Permission>> GetPermissionsByEntityAsync(string entityName);

    /// <summary>
    /// Cập nhật permission
    /// </summary>
    Permission UpdatePermission(Permission permission);

    /// <summary>
    /// Cập nhật permission (async)
    /// </summary>
    Task<Permission> UpdatePermissionAsync(Permission permission);

    /// <summary>
    /// Xóa permission
    /// </summary>
    void DeletePermission(Guid id);

    /// <summary>
    /// Xóa permission (async)
    /// </summary>
    Task DeletePermissionAsync(Guid id);

    #endregion

    #region Role CRUD

    /// <summary>
    /// Tạo role mới
    /// </summary>
    Role CreateRole(Role role);

    /// <summary>
    /// Tạo role mới (async)
    /// </summary>
    Task<Role> CreateRoleAsync(Role role);

    /// <summary>
    /// Lấy role theo ID
    /// </summary>
    Role GetRoleById(Guid id);

    /// <summary>
    /// Lấy role theo ID (async)
    /// </summary>
    Task<Role> GetRoleByIdAsync(Guid id);

    /// <summary>
    /// Lấy role theo tên
    /// </summary>
    Role GetRoleByName(string name);

    /// <summary>
    /// Lấy tất cả roles
    /// </summary>
    List<Role> GetAllRoles();

    /// <summary>
    /// Lấy tất cả roles (async)
    /// </summary>
    Task<List<Role>> GetAllRolesAsync();

    /// <summary>
    /// Lấy các roles đang active
    /// </summary>
    List<Role> GetActiveRoles();

    /// <summary>
    /// Cập nhật role
    /// </summary>
    Role UpdateRole(Role role);

    /// <summary>
    /// Cập nhật role (async)
    /// </summary>
    Task<Role> UpdateRoleAsync(Role role);

    /// <summary>
    /// Xóa role (chỉ xóa được nếu không phải system role)
    /// </summary>
    void DeleteRole(Guid id);

    /// <summary>
    /// Xóa role (async)
    /// </summary>
    Task DeleteRoleAsync(Guid id);

    #endregion

    #region UserRole Management

    /// <summary>
    /// Gán role cho user
    /// </summary>
    void AssignRoleToUser(Guid userId, Guid roleId, Guid? assignedBy = null);

    /// <summary>
    /// Gán role cho user (async)
    /// </summary>
    Task AssignRoleToUserAsync(Guid userId, Guid roleId, Guid? assignedBy = null);

    /// <summary>
    /// Gỡ role khỏi user
    /// </summary>
    void RemoveRoleFromUser(Guid userId, Guid roleId);

    /// <summary>
    /// Gỡ role khỏi user (async)
    /// </summary>
    Task RemoveRoleFromUserAsync(Guid userId, Guid roleId);

    /// <summary>
    /// Lấy tất cả roles của user
    /// </summary>
    List<Role> GetUserRoles(Guid userId);

    /// <summary>
    /// Lấy tất cả roles của user (async)
    /// </summary>
    Task<List<Role>> GetUserRolesAsync(Guid userId);

    /// <summary>
    /// Lấy tất cả users của role
    /// </summary>
    List<ApplicationUser> GetRoleUsers(Guid roleId);

    /// <summary>
    /// Kiểm tra user có role không
    /// </summary>
    bool UserHasRole(Guid userId, Guid roleId);

    /// <summary>
    /// Kiểm tra user có role không (async)
    /// </summary>
    Task<bool> UserHasRoleAsync(Guid userId, Guid roleId);

    #endregion

    #region RolePermission Management

    /// <summary>
    /// Gán permission cho role
    /// </summary>
    void AssignPermissionToRole(Guid roleId, Guid permissionId, bool isGranted = true);

    /// <summary>
    /// Gán permission cho role (async)
    /// </summary>
    Task AssignPermissionToRoleAsync(Guid roleId, Guid permissionId, bool isGranted = true);

    /// <summary>
    /// Gỡ permission khỏi role
    /// </summary>
    void RemovePermissionFromRole(Guid roleId, Guid permissionId);

    /// <summary>
    /// Gỡ permission khỏi role (async)
    /// </summary>
    Task RemovePermissionFromRoleAsync(Guid roleId, Guid permissionId);

    /// <summary>
    /// Lấy tất cả permissions của role
    /// </summary>
    List<Permission> GetRolePermissions(Guid roleId);

    /// <summary>
    /// Lấy tất cả permissions của role (async)
    /// </summary>
    Task<List<Permission>> GetRolePermissionsAsync(Guid roleId);

    /// <summary>
    /// Gán nhiều permissions cho role
    /// </summary>
    void AssignPermissionsToRole(Guid roleId, List<Guid> permissionIds, bool isGranted = true);

    #endregion

    #region UserPermission Management

    /// <summary>
    /// Gán permission trực tiếp cho user (override)
    /// </summary>
    void AssignPermissionToUser(Guid userId, Guid permissionId, bool isGranted = true, Guid? createdBy = null);

    /// <summary>
    /// Gán permission trực tiếp cho user (async)
    /// </summary>
    Task AssignPermissionToUserAsync(Guid userId, Guid permissionId, bool isGranted = true, Guid? createdBy = null);

    /// <summary>
    /// Gỡ permission khỏi user
    /// </summary>
    void RemovePermissionFromUser(Guid userId, Guid permissionId);

    /// <summary>
    /// Gỡ permission khỏi user (async)
    /// </summary>
    Task RemovePermissionFromUserAsync(Guid userId, Guid permissionId);

    /// <summary>
    /// Lấy tất cả permissions của user (từ role + override)
    /// </summary>
    List<Permission> GetUserPermissions(Guid userId);

    /// <summary>
    /// Lấy tất cả permissions của user (async)
    /// </summary>
    Task<List<Permission>> GetUserPermissionsAsync(Guid userId);

    #endregion

    #region Permission Checking

    /// <summary>
    /// Kiểm tra user có quyền thực hiện action trên entity không
    /// </summary>
    bool HasPermission(Guid userId, string entityName, string action);

    /// <summary>
    /// Kiểm tra user có quyền thực hiện action trên entity không (async)
    /// </summary>
    Task<bool> HasPermissionAsync(Guid userId, string entityName, string action);

    /// <summary>
    /// Kiểm tra user có quyền thực hiện action trên entity không (sử dụng stored procedure)
    /// </summary>
    bool HasPermissionUsingSp(Guid userId, string entityName, string action);

    /// <summary>
    /// Lấy danh sách quyền của user theo entity
    /// </summary>
    List<Permission> GetUserPermissionsByEntity(Guid userId, string entityName);

    #endregion
}
