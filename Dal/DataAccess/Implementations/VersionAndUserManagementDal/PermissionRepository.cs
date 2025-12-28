using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Threading.Tasks;
using Dal.DataAccess.Interfaces.VersionAndUserManagementDal;
using Dal.DataContext;
using Dal.DtoConverter;
using Dal.Exceptions;
using DTO.VersionAndUserManagementDto;
using Logger;
using Logger.Configuration;
using CustomLogger = Logger.Interfaces.ILogger;

namespace Dal.DataAccess.Implementations.VersionAndUserManagementDal;

/// <summary>
/// Repository quản lý hệ thống quyền truy cập (Permission Management System)
/// </summary>
public class PermissionRepository : IPermissionRepository
{
    #region Private Fields

    private readonly string _connectionString;
    private readonly CustomLogger _logger;

    #endregion

    #region Constructor

    public PermissionRepository(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _logger = LoggerFactory.CreateLogger(LogCategory.DAL);
        _logger.Info("PermissionRepository được khởi tạo");
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
        // Load navigation properties nếu cần
        context.LoadOptions = loadOptions;
        return context;
    }

    #endregion

    #region ========== READ OPERATIONS ==========

    /// <summary>
    /// Lấy permission theo EntityName và Action
    /// </summary>
    public PermissionDto GetPermission(string entityName, string action)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(entityName) || string.IsNullOrWhiteSpace(action))
                return null;

            using var context = CreateNewContext();
            var entity = context.Permissions.FirstOrDefault(p => 
                p.EntityName == entityName && 
                p.Action == action && 
                p.IsActive);
            
            return entity?.ToDto();
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy Permission: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy Permission: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Lấy tất cả permissions
    /// </summary>
    public List<PermissionDto> GetAllPermissions()
    {
        try
        {
            using var context = CreateNewContext();
            var entities = context.Permissions
                .Where(p => p.IsActive)
                .OrderBy(p => p.EntityName)
                .ThenBy(p => p.Action)
                .ToList();
            
            return entities.ToDtos();
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy tất cả Permissions: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy Permissions: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Lấy permissions theo EntityName
    /// </summary>
    public List<PermissionDto> GetPermissionsByEntity(string entityName)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(entityName))
                return new List<PermissionDto>();

            using var context = CreateNewContext();
            var entities = context.Permissions
                .Where(p => p.EntityName == entityName && p.IsActive)
                .OrderBy(p => p.Action)
                .ToList();
            
            return entities.ToDtos();
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy Permissions theo Entity: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy Permissions: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Lấy role theo ID
    /// </summary>
    public RoleDto GetRoleById(Guid id)
    {
        try
        {
            using var context = CreateNewContext();
            var entity = context.Roles.FirstOrDefault(r => r.Id == id);
            return entity?.ToDto();
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy Role theo ID: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy Role: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Lấy role theo tên
    /// </summary>
    public RoleDto GetRoleByName(string name)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            using var context = CreateNewContext();
            var entity = context.Roles.FirstOrDefault(r => r.Name == name);
            return entity?.ToDto();
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy Role theo tên: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy Role: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Lấy tất cả roles
    /// </summary>
    public List<RoleDto> GetAllRoles()
    {
        try
        {
            using var context = CreateNewContext();
            var entities = context.Roles
                .OrderBy(r => r.Name)
                .ToList();
            
            return entities.ToDtos();
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy tất cả Roles: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy Roles: {ex.Message}", ex);
        }
    }

    #endregion

    #region ========== CREATE OPERATIONS ==========

    /// <summary>
    /// Tạo role mới
    /// </summary>
    public RoleDto CreateRole(RoleDto dto)
    {
        try
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Name không được để trống", nameof(dto));

            // Kiểm tra duplicate
            if (GetRoleByName(dto.Name) != null)
                throw new DataAccessException($"Role '{dto.Name}' đã tồn tại");

            using var context = CreateNewContext();
            
            // Convert DTO to Entity
            var entity = dto.ToEntity();
            
            if (entity.Id == Guid.Empty)
                entity.Id = Guid.NewGuid();

            context.Roles.InsertOnSubmit(entity);
            context.SubmitChanges();

            _logger.Info($"Đã tạo Role: {entity.Name}");
            return entity.ToDto();
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi tạo Role: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi tạo Role: {ex.Message}", ex);
        }
    }

    #endregion

    #region ========== UPDATE OPERATIONS ==========

    /// <summary>
    /// Cập nhật role
    /// </summary>
    public RoleDto UpdateRole(RoleDto dto)
    {
        try
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            using var context = CreateNewContext();
            var existing = context.Roles.FirstOrDefault(r => r.Id == dto.Id);

            if (existing == null)
                throw new DataAccessException($"Không tìm thấy Role với ID: {dto.Id}");

            // Không cho phép đổi tên nếu là system role
            if (existing.IsSystemRole && existing.Name != dto.Name)
                throw new DataAccessException("Không thể đổi tên System Role");

            // Convert DTO to Entity (update existing)
            dto.ToEntity(existing);
            existing.ModifiedDate = DateTime.Now;

            context.SubmitChanges();
            _logger.Info($"Đã cập nhật Role: {existing.Name}");
            return existing.ToDto();
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi cập nhật Role: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi cập nhật Role: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Gán role cho user
    /// </summary>
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

    /// <summary>
    /// Gỡ role khỏi user
    /// </summary>
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

    /// <summary>
    /// Lấy tất cả roles của user
    /// </summary>
    public List<RoleDto> GetUserRoles(Guid userId)
    {
        try
        {
            using var context = CreateNewContext();
            var entities = context.UserRoles
                .Where(ur => ur.UserId == userId && ur.IsActive)
                .Select(ur => ur.Role)
                .Where(r => r.IsActive)
                .ToList();
            
            return entities.ToDtos();
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy Roles của User: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy Roles: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Kiểm tra user có role không
    /// </summary>
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

    /// <summary>
    /// Gán permission cho role
    /// </summary>
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

    /// <summary>
    /// Gỡ permission khỏi role
    /// </summary>
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

    /// <summary>
    /// Lấy tất cả permissions của role
    /// </summary>
    public List<PermissionDto> GetRolePermissions(Guid roleId)
    {
        try
        {
            using var context = CreateNewContext();
            var entities = context.RolePermissions
                .Where(rp => rp.RoleId == roleId && rp.IsGranted)
                .Select(rp => rp.Permission)
                .Where(p => p.IsActive)
                .ToList();
            
            return entities.ToDtos();
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy Permissions của Role: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy Permissions: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Gán nhiều permissions cho role
    /// </summary>
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

    #region ========== DELETE OPERATIONS ==========

    /// <summary>
    /// Xóa role (chỉ xóa được nếu không phải system role)
    /// </summary>
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

    #endregion

    #region ========== PERMISSION CHECKING ==========

    /// <summary>
    /// Lấy tất cả permissions của user (từ role + override)
    /// </summary>
    public List<PermissionDto> GetUserPermissions(Guid userId)
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

            return allPermissions.ToDtos();
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy Permissions của User: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy Permissions: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Lấy danh sách quyền của user theo entity
    /// </summary>
    public List<PermissionDto> GetUserPermissionsByEntity(Guid userId, string entityName)
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

    /// <summary>
    /// Kiểm tra user có quyền thực hiện action trên entity không
    /// </summary>
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

    #endregion
}
