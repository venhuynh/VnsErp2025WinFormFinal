using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Threading.Tasks;
using Dal.DataAccess.Interfaces.VersionAndUserManagementDal;
using Dal.DataContext;
using Dal.DtoConverter;
using Dal.DtoConverter.VersionAndUserManagement;
using Dal.Exceptions;
using DTO.VersionAndUserManagementDto;
using Logger;
using Logger.Configuration;
using CustomLogger = Logger.Interfaces.ILogger;

namespace Dal.DataAccess.Implementations.VersionAndUserManagementDal;

/// <summary>
/// Repository quản lý phiên bản ứng dụng
/// </summary>
public class ApplicationVersionRepository : IApplicationVersionRepository
{
    #region Private Fields

    private readonly string _connectionString;
    private readonly CustomLogger _logger;

    #endregion

    #region Constructor

    public ApplicationVersionRepository(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _logger = LoggerFactory.CreateLogger(LogCategory.DAL);
        _logger.Info("ApplicationVersionRepository được khởi tạo");
    }

    #endregion

    #region ========== HELPER METHODS ==========

    /// <summary>
    /// Tạo DataContext mới cho mỗi operation
    /// </summary>
    /// <returns>DataContext mới</returns>
    private VnsErp2025DataContext CreateNewContext()
    {
        // DEBUG: Log connection string để kiểm tra database
        _logger.Debug($"Creating context with connection string: {_connectionString}");
        
        var context = new VnsErp2025DataContext(_connectionString);
        var loadOptions = new DataLoadOptions();
        context.LoadOptions = loadOptions;
        return context;
    }

    #endregion

    #region ========== READ OPERATIONS ==========

    /// <summary>
    /// Lấy phiên bản đang hoạt động
    /// </summary>
    public ApplicationVersionDto GetActiveVersion()
    {
        try
        {
            using var context = CreateNewContext();
            var entity = context.VnsErpApplicationVersions.FirstOrDefault(x => x.IsActive);
            
            _logger.Debug($"GetActiveVersion: {(entity != null ? $"Found version {entity.Version}" : "No active version found")}");
            return entity?.ToDto();
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy phiên bản đang hoạt động: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy phiên bản đang hoạt động: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Lấy tất cả phiên bản
    /// </summary>
    public List<ApplicationVersionDto> GetAllVersions()
    {
        try
        {
            using var context = CreateNewContext();
            var entities = context.VnsErpApplicationVersions
                .OrderByDescending(v => v.ReleaseDate)
                .ToList();
            
            _logger.Debug($"GetAllVersions: Found {entities.Count} versions");
            return entities.ToDtos();
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy tất cả phiên bản: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy tất cả phiên bản: {ex.Message}", ex);
        }
    }

    #endregion

    #region ========== CREATE OPERATIONS ==========

    /// <summary>
    /// Tạo phiên bản mới
    /// </summary>
    public ApplicationVersionDto Create(ApplicationVersionDto dto)
    {
        try
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            using var context = CreateNewContext();
            
            // Convert DTO to Entity
            var entity = dto.ToEntity();
            
            if (entity.Id == Guid.Empty)
                entity.Id = Guid.NewGuid();

            if (entity.CreateDate == default(DateTime))
                entity.CreateDate = DateTime.Now;

            context.VnsErpApplicationVersions.InsertOnSubmit(entity);
            context.SubmitChanges();
            
            _logger.Info($"Đã tạo phiên bản mới: {entity.Version} (ID: {entity.Id})");
            return entity.ToDto();
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi tạo phiên bản: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi tạo phiên bản: {ex.Message}", ex);
        }
    }

    #endregion

    #region ========== UPDATE OPERATIONS ==========

    /// <summary>
    /// Cập nhật phiên bản
    /// </summary>
    public ApplicationVersionDto Update(ApplicationVersionDto dto)
    {
        try
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            using var context = CreateNewContext();
            var existing = context.VnsErpApplicationVersions
                .FirstOrDefault(v => v.Id == dto.Id);

            if (existing == null)
                throw new DataAccessException($"Không tìm thấy phiên bản với ID: {dto.Id}");

            // Convert DTO to Entity (update existing)
            dto.ToEntity(existing);
            existing.ModifiedDate = DateTime.Now;

            context.SubmitChanges();
            
            _logger.Info($"Đã cập nhật phiên bản: {dto.Version} (ID: {dto.Id})");
            return existing.ToDto();
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi cập nhật phiên bản: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi cập nhật phiên bản: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Đặt một phiên bản làm Active và vô hiệu hóa các phiên bản khác
    /// </summary>
    public void SetActiveVersion(Guid versionId)
    {
        try
        {
            using var context = CreateNewContext();
            
            // Vô hiệu hóa tất cả phiên bản
            var allVersions = context.VnsErpApplicationVersions.ToList();
            foreach (var v in allVersions)
            {
                v.IsActive = false;
                v.ModifiedDate = DateTime.Now;
            }

            // Đặt phiên bản được chọn làm Active
            var targetVersion = allVersions.FirstOrDefault(v => v.Id == versionId);
            if (targetVersion == null)
                throw new DataAccessException($"Không tìm thấy phiên bản với ID: {versionId}");

            targetVersion.IsActive = true;
            targetVersion.ModifiedDate = DateTime.Now;

            context.SubmitChanges();
            
            _logger.Info($"Đã đặt phiên bản {targetVersion.Version} (ID: {versionId}) làm Active");
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi đặt phiên bản Active: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi đặt phiên bản Active: {ex.Message}", ex);
        }
    }

    #endregion
}
