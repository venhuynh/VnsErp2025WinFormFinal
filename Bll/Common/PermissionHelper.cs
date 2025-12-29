using System;
using System.Collections.Generic;
using System.Linq;
using DTO.VersionAndUserManagementDto;

namespace Bll.Common;

/// <summary>
/// Helper class để kiểm tra quyền truy cập dễ dàng
/// Sử dụng singleton pattern để cache permissions
/// </summary>
public static class PermissionHelper
{
    private static PermissionBll _permissionBll;
    private static readonly object _lockObject = new object();

    /// <summary>
    /// Lấy instance của PermissionBll (lazy initialization)
    /// </summary>
    private static PermissionBll GetPermissionBll()
    {
        if (_permissionBll == null)
        {
            lock (_lockObject)
            {
                if (_permissionBll == null)
                {
                    _permissionBll = new PermissionBll();
                }
            }
        }

        return _permissionBll;
    }

    #region Permission Checking Methods

    /// <summary>
    /// Kiểm tra user có quyền thực hiện action trên entity không
    /// </summary>
    public static bool HasPermission(Guid userId, string entityName, string action)
    {
        if (userId == Guid.Empty || string.IsNullOrWhiteSpace(entityName) || string.IsNullOrWhiteSpace(action))
            return false;

        return GetPermissionBll().HasPermission(userId, entityName, action);
    }

    /// <summary>
    /// Kiểm tra user có quyền Read trên entity
    /// </summary>
    public static bool CanRead(Guid userId, string entityName)
    {
        return HasPermission(userId, entityName, "Read");
    }

    /// <summary>
    /// Kiểm tra user có quyền Create trên entity
    /// </summary>
    public static bool CanCreate(Guid userId, string entityName)
    {
        return HasPermission(userId, entityName, "Create");
    }

    /// <summary>
    /// Kiểm tra user có quyền Update trên entity
    /// </summary>
    public static bool CanUpdate(Guid userId, string entityName)
    {
        return HasPermission(userId, entityName, "Update");
    }

    /// <summary>
    /// Kiểm tra user có quyền Delete trên entity
    /// </summary>
    public static bool CanDelete(Guid userId, string entityName)
    {
        return HasPermission(userId, entityName, "Delete");
    }

    /// <summary>
    /// Kiểm tra user có quyền Approve trên entity
    /// </summary>
    public static bool CanApprove(Guid userId, string entityName)
    {
        return HasPermission(userId, entityName, "Approve");
    }

    /// <summary>
    /// Kiểm tra user có quyền Export trên entity
    /// </summary>
    public static bool CanExport(Guid userId, string entityName)
    {
        return HasPermission(userId, entityName, "Export");
    }

    /// <summary>
    /// Kiểm tra user có quyền Import trên entity
    /// </summary>
    public static bool CanImport(Guid userId, string entityName)
    {
        return HasPermission(userId, entityName, "Import");
    }

    /// <summary>
    /// Kiểm tra user có quyền Print trên entity
    /// </summary>
    public static bool CanPrint(Guid userId, string entityName)
    {
        return HasPermission(userId, entityName, "Print");
    }

    #endregion

    #region Role Checking Methods

    /// <summary>
    /// Kiểm tra user có role không
    /// </summary>
    public static bool UserHasRole(Guid userId, string roleName)
    {
        if (userId == Guid.Empty || string.IsNullOrWhiteSpace(roleName))
            return false;

        return GetPermissionBll().UserHasRole(userId, roleName);
    }

    /// <summary>
    /// Kiểm tra user có role Administrator không
    /// </summary>
    public static bool IsAdministrator(Guid userId)
    {
        return UserHasRole(userId, "Administrator");
    }

    /// <summary>
    /// Kiểm tra user có role Manager không
    /// </summary>
    public static bool IsManager(Guid userId)
    {
        return UserHasRole(userId, "Manager");
    }

    /// <summary>
    /// Kiểm tra user có role User không
    /// </summary>
    public static bool IsUser(Guid userId)
    {
        return UserHasRole(userId, "User");
    }

    /// <summary>
    /// Kiểm tra user có role Viewer không
    /// </summary>
    public static bool IsViewer(Guid userId)
    {
        return UserHasRole(userId, "Viewer");
    }

    #endregion

    #region Permission Retrieval Methods

    /// <summary>
    /// Lấy tất cả quyền của user
    /// </summary>
    public static List<PermissionDto> GetUserPermissions(Guid userId)
    {
        if (userId == Guid.Empty)
            return new List<PermissionDto>();

        // GetUserPermissions() already returns List<PermissionDto>
        return GetPermissionBll().GetUserPermissions(userId);
    }

    /// <summary>
    /// Lấy quyền của user theo entity
    /// </summary>
    public static List<PermissionDto> GetUserPermissionsByEntity(Guid userId, string entityName)
    {
        if (userId == Guid.Empty || string.IsNullOrWhiteSpace(entityName))
            return new List<PermissionDto>();

        // GetUserPermissionsByEntity() already returns List<PermissionDto>
        return GetPermissionBll().GetUserPermissionsByEntity(userId, entityName);
    }

    /// <summary>
    /// Lấy danh sách actions mà user có quyền trên entity
    /// </summary>
    public static List<string> GetUserActionsForEntity(Guid userId, string entityName)
    {
        var permissions = GetUserPermissionsByEntity(userId, entityName);
        return permissions.Select(p => p.Action).Distinct().ToList();
    }

    #endregion

    #region Batch Permission Checking

    /// <summary>
    /// Kiểm tra user có tất cả các quyền trong danh sách không
    /// </summary>
    public static bool HasAllPermissions(Guid userId, List<(string EntityName, string Action)> permissions)
    {
        if (userId == Guid.Empty || permissions == null || permissions.Count == 0)
            return false;

        return permissions.All(p => HasPermission(userId, p.EntityName, p.Action));
    }

    /// <summary>
    /// Kiểm tra user có ít nhất một quyền trong danh sách không
    /// </summary>
    public static bool HasAnyPermission(Guid userId, List<(string EntityName, string Action)> permissions)
    {
        if (userId == Guid.Empty || permissions == null || permissions.Count == 0)
            return false;

        return permissions.Any(p => HasPermission(userId, p.EntityName, p.Action));
    }

    #endregion

    #region Entity-Specific Permission Helpers

    /// <summary>
    /// Kiểm tra quyền trên ProductService
    /// </summary>
    public static class ProductService
    {
        public static bool CanRead(Guid userId) => PermissionHelper.CanRead(userId, "ProductService");
        public static bool CanCreate(Guid userId) => PermissionHelper.CanCreate(userId, "ProductService");
        public static bool CanUpdate(Guid userId) => PermissionHelper.CanUpdate(userId, "ProductService");
        public static bool CanDelete(Guid userId) => PermissionHelper.CanDelete(userId, "ProductService");
    }

    /// <summary>
    /// Kiểm tra quyền trên BusinessPartner
    /// </summary>
    public static class BusinessPartner
    {
        public static bool CanRead(Guid userId) => PermissionHelper.CanRead(userId, "BusinessPartner");
        public static bool CanCreate(Guid userId) => PermissionHelper.CanCreate(userId, "BusinessPartner");
        public static bool CanUpdate(Guid userId) => PermissionHelper.CanUpdate(userId, "BusinessPartner");
        public static bool CanDelete(Guid userId) => PermissionHelper.CanDelete(userId, "BusinessPartner");
    }

    /// <summary>
    /// Kiểm tra quyền trên StockInOutMaster
    /// </summary>
    public static class StockInOutMaster
    {
        public static bool CanRead(Guid userId) => PermissionHelper.CanRead(userId, "StockInOutMaster");
        public static bool CanCreate(Guid userId) => PermissionHelper.CanCreate(userId, "StockInOutMaster");
        public static bool CanUpdate(Guid userId) => PermissionHelper.CanUpdate(userId, "StockInOutMaster");
        public static bool CanDelete(Guid userId) => PermissionHelper.CanDelete(userId, "StockInOutMaster");
        public static bool CanApprove(Guid userId) => PermissionHelper.CanApprove(userId, "StockInOutMaster");
    }

    /// <summary>
    /// Kiểm tra quyền trên Employee
    /// </summary>
    public static class Employee
    {
        public static bool CanRead(Guid userId) => PermissionHelper.CanRead(userId, "Employee");
        public static bool CanCreate(Guid userId) => PermissionHelper.CanCreate(userId, "Employee");
        public static bool CanUpdate(Guid userId) => PermissionHelper.CanUpdate(userId, "Employee");
        public static bool CanDelete(Guid userId) => PermissionHelper.CanDelete(userId, "Employee");
    }

    #endregion
}
