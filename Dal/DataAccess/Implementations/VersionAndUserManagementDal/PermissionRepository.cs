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
/// Repository quản lý hệ thống quyền truy cập (Permission Management System)
/// </summary>
public class PermissionRepository : IPermissionRepository
{
    private readonly string _connectionString;
    private readonly CustomLogger _logger;

    public PermissionRepository(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _logger = LoggerFactory.CreateLogger(LogCategory.DAL);
        _logger.Info("PermissionRepository được khởi tạo");
    }

    private VnsErp2025DataContext CreateNewContext()
    {
        var context = new VnsErp2025DataContext(_connectionString);
        var loadOptions = new DataLoadOptions();
        // Load navigation properties nếu cần
        context.LoadOptions = loadOptions;
        return context;
    }

    #region Permission CRUD

    public Permission Create(Permission permission)
    {
        try
        {
            if (permission == null)
                throw new ArgumentNullException(nameof(permission));

            if (permission.Id == Guid.Empty)
                permission.Id = Guid.NewGuid();

            // Validate
            if (string.IsNullOrWhiteSpace(permission.EntityName))
                throw new ArgumentException("EntityName không được để trống", nameof(permission));

            if (string.IsNullOrWhiteSpace(permission.Action))
                throw new ArgumentException("Action không được để trống", nameof(permission));

            // Kiểm tra duplicate
            if (GetPermission(permission.EntityName, permission.Action) != null)
                throw new DataAccessException($"Permission '{permission.EntityName}.{permission.Action}' đã tồn tại");

            using var context = CreateNewContext();
            context.Permissions.InsertOnSubmit(permission);
            context.SubmitChanges();

            _logger.Info($"Đã tạo Permission: {permission.EntityName}.{permission.Action}");
            return permission;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi tạo Permission: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi tạo Permission: {ex.Message}", ex);
        }
    }

    public async Task<Permission> CreateAsync(Permission permission)
    {
        return await Task.Run(() => Create(permission));
    }

    public Permission GetPermissionById(Guid id)
    {
        try
        {
            using var context = CreateNewContext();
            return context.Permissions.FirstOrDefault(p => p.Id == id);
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy Permission theo ID: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy Permission: {ex.Message}", ex);
        }
    }

    public async Task<Permission> GetPermissionByIdAsync(Guid id)
    {
        return await Task.Run(() => GetPermissionById(id));
    }

    public Permission GetPermission(string entityName, string action)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(entityName) || string.IsNullOrWhiteSpace(action))
                return null;

            using var context = CreateNewContext();
            return context.Permissions.FirstOrDefault(p => 
                p.EntityName == entityName && 
                p.Action == action && 
                p.IsActive);
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy Permission: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy Permission: {ex.Message}", ex);
        }
    }

    public List<Permission> GetAllPermissions()
    {
        try
        {
            using var context = CreateNewContext();
            return context.Permissions
                .Where(p => p.IsActive)
                .OrderBy(p => p.EntityName)
                .ThenBy(p => p.Action)
                .ToList();
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy tất cả Permissions: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy Permissions: {ex.Message}", ex);
        }
    }

    public async Task<List<Permission>> GetAllPermissionsAsync()
    {
        return await Task.Run(() => GetAllPermissions());
    }

    public List<Permission> GetPermissionsByEntity(string entityName)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(entityName))
                return new List<Permission>();

            using var context = CreateNewContext();
            return context.Permissions
                .Where(p => p.EntityName == entityName && p.IsActive)
                .OrderBy(p => p.Action)
                .ToList();
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy Permissions theo Entity: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy Permissions: {ex.Message}", ex);
        }
    }

    public async Task<List<Permission>> GetPermissionsByEntityAsync(string entityName)
    {
        return await Task.Run(() => GetPermissionsByEntity(entityName));
    }

    public Permission UpdatePermission(Permission permission)
    {
        try
        {
            if (permission == null)
                throw new ArgumentNullException(nameof(permission));

            using var context = CreateNewContext();
            var existing = context.Permissions.FirstOrDefault(p => p.Id == permission.Id);

            if (existing == null)
                throw new DataAccessException($"Không tìm thấy Permission với ID: {permission.Id}");

            existing.Description = permission.Description;
            existing.IsActive = permission.IsActive;

            context.SubmitChanges();
            _logger.Info($"Đã cập nhật Permission: {existing.EntityName}.{existing.Action}");
            return existing;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi cập nhật Permission: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi cập nhật Permission: {ex.Message}", ex);
        }
    }

    public async Task<Permission> UpdatePermissionAsync(Permission permission)
    {
        return await Task.Run(() => UpdatePermission(permission));
    }

    public void DeletePermission(Guid id)
    {
        try
        {
            using var context = CreateNewContext();
            var permission = context.Permissions.FirstOrDefault(p => p.Id == id);

            if (permission == null)
                throw new DataAccessException($"Không tìm thấy Permission với ID: {id}");

            // Soft delete
            permission.IsActive = false;
            context.SubmitChanges();

            _logger.Info($"Đã xóa Permission: {permission.EntityName}.{permission.Action}");
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi xóa Permission: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi xóa Permission: {ex.Message}", ex);
        }
    }

    public async Task DeletePermissionAsync(Guid id)
    {
        await Task.Run(() => DeletePermission(id));
    }

    #endregion

    #region Role CRUD

    public Role CreateRole(Role role)
    {
        try
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            if (role.Id == Guid.Empty)
                role.Id = Guid.NewGuid();

            if (string.IsNullOrWhiteSpace(role.Name))
                throw new ArgumentException("Name không được để trống", nameof(role));

            // Kiểm tra duplicate
            if (GetRoleByName(role.Name) != null)
                throw new DataAccessException($"Role '{role.Name}' đã tồn tại");

            using var context = CreateNewContext();
            context.Roles.InsertOnSubmit(role);
            context.SubmitChanges();

            _logger.Info($"Đã tạo Role: {role.Name}");
            return role;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi tạo Role: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi tạo Role: {ex.Message}", ex);
        }
    }

    public async Task<Role> CreateRoleAsync(Role role)
    {
        return await Task.Run(() => CreateRole(role));
    }

    public Role GetRoleById(Guid id)
    {
        try
        {
            using var context = CreateNewContext();
            return context.Roles.FirstOrDefault(r => r.Id == id);
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy Role theo ID: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy Role: {ex.Message}", ex);
        }
    }

    public async Task<Role> GetRoleByIdAsync(Guid id)
    {
        return await Task.Run(() => GetRoleById(id));
    }

    public Role GetRoleByName(string name)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            using var context = CreateNewContext();
            return context.Roles.FirstOrDefault(r => r.Name == name);
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy Role theo tên: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy Role: {ex.Message}", ex);
        }
    }

    public List<Role> GetAllRoles()
    {
        try
        {
            using var context = CreateNewContext();
            return context.Roles
                .OrderBy(r => r.Name)
                .ToList();
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy tất cả Roles: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy Roles: {ex.Message}", ex);
        }
    }

    public async Task<List<Role>> GetAllRolesAsync()
    {
        return await Task.Run(() => GetAllRoles());
    }

    public List<Role> GetActiveRoles()
    {
        try
        {
            using var context = CreateNewContext();
            return context.Roles
                .Where(r => r.IsActive)
                .OrderBy(r => r.Name)
                .ToList();
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy Active Roles: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy Roles: {ex.Message}", ex);
        }
    }

    public Role UpdateRole(Role role)
    {
        try
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            using var context = CreateNewContext();
            var existing = context.Roles.FirstOrDefault(r => r.Id == role.Id);

            if (existing == null)
                throw new DataAccessException($"Không tìm thấy Role với ID: {role.Id}");

            // Không cho phép đổi tên nếu là system role
            if (existing.IsSystemRole && existing.Name != role.Name)
                throw new DataAccessException("Không thể đổi tên System Role");

            existing.Name = role.Name;
            existing.Description = role.Description;
            existing.IsActive = role.IsActive;
            existing.ModifiedDate = DateTime.Now;
            existing.ModifiedBy = role.ModifiedBy;

            context.SubmitChanges();
            _logger.Info($"Đã cập nhật Role: {existing.Name}");
            return existing;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi cập nhật Role: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi cập nhật Role: {ex.Message}", ex);
        }
    }

    public async Task<Role> UpdateRoleAsync(Role role)
    {
        return await Task.Run(() => UpdateRole(role));
    }

    public void DeleteRole(Guid id)
    {
        try
        {
            using var context = CreateNewContext();
            var role = context.Roles.FirstOrDefault(r => r.Id == id);

            if (role == null)
                throw new DataAccessException($"Không tìm thấy Role với ID: {id}");

            if (role.IsSystemRole)
                throw new DataAccessException("Không thể xóa System Role");

            // Soft delete
            role.IsActive = false;
            context.SubmitChanges();

            _logger.Info($"Đã xóa Role: {role.Name}");
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi xóa Role: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi xóa Role: {ex.Message}", ex);
        }
    }

    public async Task DeleteRoleAsync(Guid id)
    {
        await Task.Run(() => DeleteRole(id));
    }

    #endregion

    #region UserRole Management

    public void AssignRoleToUser(Guid userId, Guid roleId, Guid? assignedBy = null)
    {
        try
        {
            using var context = CreateNewContext();
            
            // Kiểm tra đã tồn tại chưa
            var existing = context.UserRoles.FirstOrDefault(ur => ur.UserId == userId && ur.RoleId == roleId);
            if (existing != null)
            {
                if (!existing.IsActive)
                {
                    existing.IsActive = true;
                    existing.AssignedDate = DateTime.Now;
                    existing.AssignedBy = assignedBy;
                    context.SubmitChanges();
                }
                return;
            }

            var userRole = new UserRole
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                RoleId = roleId,
                IsActive = true,
                AssignedDate = DateTime.Now,
                AssignedBy = assignedBy
            };

            context.UserRoles.InsertOnSubmit(userRole);
            context.SubmitChanges();

            _logger.Info($"Đã gán Role {roleId} cho User {userId}");
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi gán Role cho User: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi gán Role: {ex.Message}", ex);
        }
    }

    public async Task AssignRoleToUserAsync(Guid userId, Guid roleId, Guid? assignedBy = null)
    {
        await Task.Run(() => AssignRoleToUser(userId, roleId, assignedBy));
    }

    public void RemoveRoleFromUser(Guid userId, Guid roleId)
    {
        try
        {
            using var context = CreateNewContext();
            var userRole = context.UserRoles.FirstOrDefault(ur => ur.UserId == userId && ur.RoleId == roleId);

            if (userRole == null)
                throw new DataAccessException($"User không có Role này");

            // Soft delete
            userRole.IsActive = false;
            context.SubmitChanges();

            _logger.Info($"Đã gỡ Role {roleId} khỏi User {userId}");
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi gỡ Role khỏi User: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi gỡ Role: {ex.Message}", ex);
        }
    }

    public async Task RemoveRoleFromUserAsync(Guid userId, Guid roleId)
    {
        await Task.Run(() => RemoveRoleFromUser(userId, roleId));
    }

    public List<Role> GetUserRoles(Guid userId)
    {
        try
        {
            using var context = CreateNewContext();
            return context.UserRoles
                .Where(ur => ur.UserId == userId && ur.IsActive)
                .Select(ur => ur.Role)
                .Where(r => r.IsActive)
                .ToList();
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy Roles của User: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy Roles: {ex.Message}", ex);
        }
    }

    public async Task<List<Role>> GetUserRolesAsync(Guid userId)
    {
        return await Task.Run(() => GetUserRoles(userId));
    }

    public List<ApplicationUser> GetRoleUsers(Guid roleId)
    {
        try
        {
            using var context = CreateNewContext();
            return context.UserRoles
                .Where(ur => ur.RoleId == roleId && ur.IsActive)
                .Select(ur => ur.ApplicationUser)
                .Where(u => u.Active)
                .ToList();
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy Users của Role: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy Users: {ex.Message}", ex);
        }
    }

    public bool UserHasRole(Guid userId, Guid roleId)
    {
        try
        {
            using var context = CreateNewContext();
            return context.UserRoles
                .Any(ur => ur.UserId == userId && ur.RoleId == roleId && ur.IsActive);
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi kiểm tra User có Role: {ex.Message}", ex);
            return false;
        }
    }

    public async Task<bool> UserHasRoleAsync(Guid userId, Guid roleId)
    {
        return await Task.Run(() => UserHasRole(userId, roleId));
    }

    #endregion

    #region RolePermission Management

    public void AssignPermissionToRole(Guid roleId, Guid permissionId, bool isGranted = true)
    {
        try
        {
            using var context = CreateNewContext();
            
            var existing = context.RolePermissions.FirstOrDefault(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);
            if (existing != null)
            {
                existing.IsGranted = isGranted;
                context.SubmitChanges();
                return;
            }

            var rolePermission = new RolePermission
            {
                Id = Guid.NewGuid(),
                RoleId = roleId,
                PermissionId = permissionId,
                IsGranted = isGranted
            };

            context.RolePermissions.InsertOnSubmit(rolePermission);
            context.SubmitChanges();

            _logger.Info($"Đã gán Permission {permissionId} cho Role {roleId}");
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi gán Permission cho Role: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi gán Permission: {ex.Message}", ex);
        }
    }

    public async Task AssignPermissionToRoleAsync(Guid roleId, Guid permissionId, bool isGranted = true)
    {
        await Task.Run(() => AssignPermissionToRole(roleId, permissionId, isGranted));
    }

    public void RemovePermissionFromRole(Guid roleId, Guid permissionId)
    {
        try
        {
            using var context = CreateNewContext();
            var rolePermission = context.RolePermissions.FirstOrDefault(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);

            if (rolePermission != null)
            {
                context.RolePermissions.DeleteOnSubmit(rolePermission);
                context.SubmitChanges();
                _logger.Info($"Đã gỡ Permission {permissionId} khỏi Role {roleId}");
            }
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi gỡ Permission khỏi Role: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi gỡ Permission: {ex.Message}", ex);
        }
    }

    public async Task RemovePermissionFromRoleAsync(Guid roleId, Guid permissionId)
    {
        await Task.Run(() => RemovePermissionFromRole(roleId, permissionId));
    }

    public List<Permission> GetRolePermissions(Guid roleId)
    {
        try
        {
            using var context = CreateNewContext();
            return context.RolePermissions
                .Where(rp => rp.RoleId == roleId && rp.IsGranted)
                .Select(rp => rp.Permission)
                .Where(p => p.IsActive)
                .ToList();
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy Permissions của Role: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy Permissions: {ex.Message}", ex);
        }
    }

    public async Task<List<Permission>> GetRolePermissionsAsync(Guid roleId)
    {
        return await Task.Run(() => GetRolePermissions(roleId));
    }

    public void AssignPermissionsToRole(Guid roleId, List<Guid> permissionIds, bool isGranted = true)
    {
        try
        {
            using var context = CreateNewContext();
            using var transaction = context.Connection.BeginTransaction();
            
            try
            {
                foreach (var permissionId in permissionIds)
                {
                    var existing = context.RolePermissions.FirstOrDefault(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);
                    if (existing != null)
                    {
                        existing.IsGranted = isGranted;
                    }
                    else
                    {
                        var rolePermission = new RolePermission
                        {
                            Id = Guid.NewGuid(),
                            RoleId = roleId,
                            PermissionId = permissionId,
                            IsGranted = isGranted
                        };
                        context.RolePermissions.InsertOnSubmit(rolePermission);
                    }
                }

                context.SubmitChanges();
                transaction.Commit();
                _logger.Info($"Đã gán {permissionIds.Count} Permissions cho Role {roleId}");
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi gán nhiều Permissions cho Role: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi gán Permissions: {ex.Message}", ex);
        }
    }

    #endregion

    #region UserPermission Management

    public void AssignPermissionToUser(Guid userId, Guid permissionId, bool isGranted = true, Guid? createdBy = null)
    {
        try
        {
            using var context = CreateNewContext();
            
            var existing = context.UserPermissions.FirstOrDefault(up => up.UserId == userId && up.PermissionId == permissionId);
            if (existing != null)
            {
                existing.IsGranted = isGranted;
                existing.IsOverride = true;
                context.SubmitChanges();
                return;
            }

            var userPermission = new UserPermission
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                PermissionId = permissionId,
                IsGranted = isGranted,
                IsOverride = true,
                CreatedBy = createdBy
            };

            context.UserPermissions.InsertOnSubmit(userPermission);
            context.SubmitChanges();

            _logger.Info($"Đã gán Permission {permissionId} cho User {userId}");
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi gán Permission cho User: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi gán Permission: {ex.Message}", ex);
        }
    }

    public async Task AssignPermissionToUserAsync(Guid userId, Guid permissionId, bool isGranted = true, Guid? createdBy = null)
    {
        await Task.Run(() => AssignPermissionToUser(userId, permissionId, isGranted, createdBy));
    }

    public void RemovePermissionFromUser(Guid userId, Guid permissionId)
    {
        try
        {
            using var context = CreateNewContext();
            var userPermission = context.UserPermissions.FirstOrDefault(up => up.UserId == userId && up.PermissionId == permissionId);

            if (userPermission != null)
            {
                context.UserPermissions.DeleteOnSubmit(userPermission);
                context.SubmitChanges();
                _logger.Info($"Đã gỡ Permission {permissionId} khỏi User {userId}");
            }
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi gỡ Permission khỏi User: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi gỡ Permission: {ex.Message}", ex);
        }
    }

    public async Task RemovePermissionFromUserAsync(Guid userId, Guid permissionId)
    {
        await Task.Run(() => RemovePermissionFromUser(userId, permissionId));
    }

    public List<Permission> GetUserPermissions(Guid userId)
    {
        try
        {
            using var context = CreateNewContext();
            
            // Lấy quyền từ UserPermission (Override) - ưu tiên
            var userPermissions = context.UserPermissions
                .Where(up => up.UserId == userId && up.IsGranted)
                .Select(up => up.Permission)
                .Where(p => p.IsActive)
                .ToList();

            // Lấy quyền từ Role
            var rolePermissions = context.UserRoles
                .Where(ur => ur.UserId == userId && ur.IsActive)
                .SelectMany(ur => ur.Role.RolePermissions)
                .Where(rp => rp.IsGranted)
                .Select(rp => rp.Permission)
                .Where(p => p.IsActive)
                .ToList();

            // Kết hợp và loại bỏ duplicate (ưu tiên UserPermission)
            var allPermissions = userPermissions
                .Union(rolePermissions.Where(rp => !userPermissions.Any(up => up.Id == rp.Id)))
                .ToList();

            return allPermissions;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy Permissions của User: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy Permissions: {ex.Message}", ex);
        }
    }

    public async Task<List<Permission>> GetUserPermissionsAsync(Guid userId)
    {
        return await Task.Run(() => GetUserPermissions(userId));
    }

    #endregion

    #region Permission Checking

    public bool HasPermission(Guid userId, string entityName, string action)
    {
        try
        {
            // Kiểm tra UserPermission trước (Override)
            using var context = CreateNewContext();
            
            var userPermission = context.UserPermissions
                .Where(up => up.UserId == userId && up.IsGranted)
                .Select(up => up.Permission)
                .FirstOrDefault(p => p.EntityName == entityName && p.Action == action && p.IsActive);

            if (userPermission != null)
                return true;

            // Kiểm tra từ Role
            var hasRolePermission = context.UserRoles
                .Where(ur => ur.UserId == userId && ur.IsActive)
                .SelectMany(ur => ur.Role.RolePermissions)
                .Where(rp => rp.IsGranted)
                .Select(rp => rp.Permission)
                .Any(p => p.EntityName == entityName && p.Action == action && p.IsActive);

            return hasRolePermission;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi kiểm tra quyền: {ex.Message}", ex);
            return false; // Deny by default
        }
    }

    public async Task<bool> HasPermissionAsync(Guid userId, string entityName, string action)
    {
        return await Task.Run(() => HasPermission(userId, entityName, action));
    }

    public bool HasPermissionUsingSp(Guid userId, string entityName, string action)
    {
        try
        {
            using var context = CreateNewContext();
            var result = context.ExecuteQuery<PermissionCheckResult>(
                "EXEC sp_CheckUserPermission @p0, @p1, @p2",
                userId, entityName, action).FirstOrDefault();

            return result?.HasPermission ?? false;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi kiểm tra quyền bằng SP: {ex.Message}", ex);
            return false;
        }
    }

    public List<Permission> GetUserPermissionsByEntity(Guid userId, string entityName)
    {
        try
        {
            var allPermissions = GetUserPermissions(userId);
            return allPermissions
                .Where(p => p.EntityName == entityName)
                .ToList();
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy Permissions theo Entity: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy Permissions: {ex.Message}", ex);
        }
    }

    #endregion

    #region Helper Classes

    private class PermissionCheckResult
    {
        public bool HasPermission { get; set; }
    }

    #endregion
}
