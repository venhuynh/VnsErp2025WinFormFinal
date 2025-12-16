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
/// Repository quản lý phiên bản ứng dụng
/// </summary>
public class ApplicationVersionRepository : IApplicationVersionRepository
{
    private readonly string _connectionString;
    private readonly CustomLogger _logger;

    public ApplicationVersionRepository(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _logger = LoggerFactory.CreateLogger(LogCategory.DAL);
        _logger.Info("ApplicationVersionRepository được khởi tạo");
    }

    private VnsErp2025DataContext CreateNewContext()
    {
        // DEBUG: Log connection string để kiểm tra database
        _logger.Debug($"Creating context with connection string: {_connectionString}");
        
        var context = new VnsErp2025DataContext(_connectionString);
        var loadOptions = new DataLoadOptions();
        context.LoadOptions = loadOptions;
        return context;
    }

    public VnsErpApplicationVersion GetActiveVersion()
    {
        try
        {
            using var context = CreateNewContext();
            var version = context.VnsErpApplicationVersions.FirstOrDefault(x => x.IsActive);
            
            _logger.Debug($"GetActiveVersion: {(version != null ? $"Found version {version.Version}" : "No active version found")}");
            return version;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy phiên bản đang hoạt động: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy phiên bản đang hoạt động: {ex.Message}", ex);
        }
    }

    public async Task<VnsErpApplicationVersion> GetActiveVersionAsync()
    {
        try
        {
            using var context = CreateNewContext();
            var versions = await Task.Run(() => 
                context.VnsErpApplicationVersions
                    .Where(v => v.IsActive)
                    .ToList());
            
            var version = versions.FirstOrDefault();
            _logger.Debug($"GetActiveVersionAsync: {(version != null ? $"Found version {version.Version}" : "No active version found")}");
            return version;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy phiên bản đang hoạt động (async): {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy phiên bản đang hoạt động: {ex.Message}", ex);
        }
    }

    public List<VnsErpApplicationVersion> GetAllVersions()
    {
        try
        {
            using var context = CreateNewContext();
            var versions = context.VnsErpApplicationVersions
                .OrderByDescending(v => v.ReleaseDate)
                .ToList();
            
            _logger.Debug($"GetAllVersions: Found {versions.Count} versions");
            return versions;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy tất cả phiên bản: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy tất cả phiên bản: {ex.Message}", ex);
        }
    }

    public async Task<List<VnsErpApplicationVersion>> GetAllVersionsAsync()
    {
        try
        {
            using var context = CreateNewContext();
            var versions = await Task.Run(() => 
                context.VnsErpApplicationVersions
                    .OrderByDescending(v => v.ReleaseDate)
                    .ToList());
            
            _logger.Debug($"GetAllVersionsAsync: Found {versions.Count} versions");
            return versions;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy tất cả phiên bản (async): {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy tất cả phiên bản: {ex.Message}", ex);
        }
    }

    public VnsErpApplicationVersion GetById(Guid id)
    {
        try
        {
            using var context = CreateNewContext();
            var version = context.VnsErpApplicationVersions
                .FirstOrDefault(v => v.Id == id);
            
            _logger.Debug($"GetById: {(version != null ? $"Found version {version.Version}" : "Version not found")}");
            return version;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy phiên bản theo ID: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy phiên bản theo ID: {ex.Message}", ex);
        }
    }

    public async Task<VnsErpApplicationVersion> GetByIdAsync(Guid id)
    {
        try
        {
            using var context = CreateNewContext();
            var versions = await Task.Run(() => 
                context.VnsErpApplicationVersions
                    .Where(v => v.Id == id)
                    .ToList());
            
            var version = versions.FirstOrDefault();
            _logger.Debug($"GetByIdAsync: {(version != null ? $"Found version {version.Version}" : "Version not found")}");
            return version;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy phiên bản theo ID (async): {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy phiên bản theo ID: {ex.Message}", ex);
        }
    }

    public VnsErpApplicationVersion Create(VnsErpApplicationVersion version)
    {
        try
        {
            if (version == null)
                throw new ArgumentNullException(nameof(version));

            if (version.Id == Guid.Empty)
                version.Id = Guid.NewGuid();

            if (version.CreateDate == default(DateTime))
                version.CreateDate = DateTime.Now;

            using var context = CreateNewContext();
            context.VnsErpApplicationVersions.InsertOnSubmit(version);
            context.SubmitChanges();
            
            _logger.Info($"Đã tạo phiên bản mới: {version.Version} (ID: {version.Id})");
            return version;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi tạo phiên bản: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi tạo phiên bản: {ex.Message}", ex);
        }
    }

    public async Task<VnsErpApplicationVersion> CreateAsync(VnsErpApplicationVersion version)
    {
        return await Task.Run(() => Create(version));
    }

    public VnsErpApplicationVersion Update(VnsErpApplicationVersion version)
    {
        try
        {
            if (version == null)
                throw new ArgumentNullException(nameof(version));

            using var context = CreateNewContext();
            var existing = context.VnsErpApplicationVersions
                .FirstOrDefault(v => v.Id == version.Id);

            if (existing == null)
                throw new DataAccessException($"Không tìm thấy phiên bản với ID: {version.Id}");

            existing.Version = version.Version;
            existing.ReleaseDate = version.ReleaseDate;
            existing.IsActive = version.IsActive;
            existing.Description = version.Description;
            existing.ReleaseNote = version.ReleaseNote;
            existing.ModifiedDate = DateTime.Now;
            existing.ModifiedBy = version.ModifiedBy;

            context.SubmitChanges();
            
            _logger.Info($"Đã cập nhật phiên bản: {version.Version} (ID: {version.Id})");
            return existing;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi cập nhật phiên bản: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi cập nhật phiên bản: {ex.Message}", ex);
        }
    }

    public async Task<VnsErpApplicationVersion> UpdateAsync(VnsErpApplicationVersion version)
    {
        return await Task.Run(() => Update(version));
    }

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

    public async Task SetActiveVersionAsync(Guid versionId)
    {
        await Task.Run(() => SetActiveVersion(versionId));
    }
}
