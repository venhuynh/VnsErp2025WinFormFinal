using System;
using System.Collections.Generic;
using DTO.VersionAndUserManagementDto;

namespace Dal.DataAccess.Interfaces.VersionAndUserManagementDal;

/// <summary>
/// Data Access cho hệ thống quản lý quyền truy cập (Permission Management System).
/// Cung cấp các phương thức quản lý Role, Permission, UserRole, RolePermission, UserPermission.
/// </summary>
public interface IPermissionRepository
{
    #region ========== READ OPERATIONS ==========

    /// <summary>
    /// Lấy permission theo EntityName và Action
    /// </summary>
    PermissionDto GetPermission(string entityName, string action);

    /// <summary>
    /// Lấy tất cả permissions
    /// </summary>
    List<PermissionDto> GetAllPermissions();

    /// <summary>
    /// Lấy permissions theo EntityName
    /// </summary>
    List<PermissionDto> GetPermissionsByEntity(string entityName);

    /// <summary>
    /// Lấy role theo ID
    /// </summary>
    RoleDto GetRoleById(Guid id);

    /// <summary>
    /// Lấy role theo tên
    /// </summary>
    RoleDto GetRoleByName(string name);

    /// <summary>
    /// Lấy tất cả roles
    /// </summary>
    List<RoleDto> GetAllRoles();

    /// <summary>
    /// Lấy tất cả roles của user
    /// </summary>
    List<RoleDto> GetUserRoles(Guid userId);

    /// <summary>
    /// Lấy tất cả permissions của role
    /// </summary>
    List<PermissionDto> GetRolePermissions(Guid roleId);

    /// <summary>
    /// Lấy tất cả permissions của user (từ role + override)
    /// </summary>
    List<PermissionDto> GetUserPermissions(Guid userId);

    /// <summary>
    /// Lấy danh sách quyền của user theo entity
    /// </summary>
    List<PermissionDto> GetUserPermissionsByEntity(Guid userId, string entityName);

    /// <summary>
    /// Kiểm tra user có quyền thực hiện action trên entity không
    /// </summary>
    bool HasPermission(Guid userId, string entityName, string action);

    /// <summary>
    /// Kiểm tra user có role không
    /// </summary>
    bool UserHasRole(Guid userId, Guid roleId);

    #endregion

    #region ========== CREATE OPERATIONS ==========

    /// <summary>
    /// Tạo role mới
    /// </summary>
    RoleDto CreateRole(RoleDto role);

    #endregion

    #region ========== UPDATE OPERATIONS ==========

    /// <summary>
    /// Cập nhật role
    /// </summary>
    RoleDto UpdateRole(RoleDto role);

    /// <summary>
    /// Gán role cho user
    /// </summary>
    void AssignRoleToUser(Guid userId, Guid roleId, Guid? assignedBy = null);

    /// <summary>
    /// Gỡ role khỏi user
    /// </summary>
    void RemoveRoleFromUser(Guid userId, Guid roleId);

    /// <summary>
    /// Gán permission cho role
    /// </summary>
    void AssignPermissionToRole(Guid roleId, Guid permissionId, bool isGranted = true);

    /// <summary>
    /// Gỡ permission khỏi role
    /// </summary>
    void RemovePermissionFromRole(Guid roleId, Guid permissionId);

    /// <summary>
    /// Gán nhiều permissions cho role
    /// </summary>
    void AssignPermissionsToRole(Guid roleId, List<Guid> permissionIds, bool isGranted = true);

    #endregion

    #region ========== DELETE OPERATIONS ==========

    /// <summary>
    /// Xóa role (chỉ xóa được nếu không phải system role)
    /// </summary>
    void DeleteRole(Guid id);

    #endregion
}
