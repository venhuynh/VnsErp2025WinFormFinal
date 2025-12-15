using Dal.Connection;
using Dal.DataAccess.Implementations.VersionAndUserManagementDal;
using Dal.DataAccess.Interfaces.VersionAndUserManagementDal;
using Dal.DataContext;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Bll.Common;

/// <summary>
/// Business Logic Layer cho quản lý phiên bản ứng dụng
/// </summary>
public class ApplicationVersionBll
{
    #region Fields

    private IApplicationVersionRepository _dataAccess;
    private readonly ILogger _logger;
    private readonly object _lockObject = new object();

    #endregion

    #region Constructors

    public ApplicationVersionBll()
    {
        _logger = LoggerFactory.CreateLogger(LogCategory.BLL);
    }

    #endregion

    #region Helper Methods

    private IApplicationVersionRepository GetDataAccess()
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

                        _dataAccess = new ApplicationVersionRepository(globalConnectionString);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"Lỗi khi khởi tạo ApplicationVersionRepository: {ex.Message}", ex);
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
    /// Lấy phiên bản đang hoạt động từ database
    /// </summary>
    /// <returns>ApplicationVersionDto hoặc null</returns>
    public ApplicationVersionDto GetActiveVersion()
    {
        try
        {
            _logger?.Info("Bắt đầu lấy phiên bản đang hoạt động");
            var version = GetDataAccess().GetActiveVersion();
            
            if (version == null)
            {
                _logger?.Warning("Không tìm thấy phiên bản đang hoạt động trong database");
                return null;
            }

            var dto = ToDto(version);
            _logger?.Info($"Hoàn thành lấy phiên bản đang hoạt động: {dto.Version}");
            return dto;
        }
        catch (Exception ex)
        {
            _logger?.Error($"Lỗi khi lấy phiên bản đang hoạt động: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Lấy phiên bản hiện tại của ứng dụng từ Assembly
    /// </summary>
    /// <returns>Phiên bản dạng string (ví dụ: "1.0.0.0")</returns>
    public string GetCurrentApplicationVersion()
    {
        try
        {
            var assembly = Assembly.GetExecutingAssembly();
            var version = assembly.GetName().Version;
            return version?.ToString() ?? "1.0.0.0";
        }
        catch (Exception ex)
        {
            _logger?.Error($"Lỗi khi lấy phiên bản từ Assembly: {ex.Message}", ex);
            return "1.0.0.0";
        }
    }

    /// <summary>
    /// Kiểm tra phiên bản ứng dụng có khớp với phiên bản trong database không
    /// </summary>
    /// <returns>True nếu khớp, False nếu không khớp</returns>
    public bool IsVersionValid()
    {
        try
        {
            var currentVersion = GetCurrentApplicationVersion();
            var activeVersion = GetActiveVersion();

            if (activeVersion == null)
            {
                _logger?.Warning("Không có phiên bản Active trong database, cho phép sử dụng");
                return true; // Nếu chưa có phiên bản trong DB, cho phép sử dụng
            }

            var isValid = currentVersion == activeVersion.Version;
            
            if (!isValid)
            {
                _logger?.Warning($"Phiên bản không khớp! Ứng dụng: {currentVersion}, Database: {activeVersion.Version}");
            }
            else
            {
                _logger?.Info($"Phiên bản khớp: {currentVersion}");
            }

            return isValid;
        }
        catch (Exception ex)
        {
            _logger?.Error($"Lỗi khi kiểm tra phiên bản: {ex.Message}", ex);
            // Trong trường hợp lỗi, cho phép sử dụng để tránh block ứng dụng
            return true;
        }
    }

    /// <summary>
    /// Lấy tất cả phiên bản
    /// </summary>
    /// <returns>Danh sách phiên bản</returns>
    public List<ApplicationVersionDto> GetAllVersions()
    {
        try
        {
            _logger?.Info("Bắt đầu lấy tất cả phiên bản");
            var versions = GetDataAccess().GetAllVersions();
            var dtos = versions.ConvertAll(ToDto);
            _logger?.Info($"Hoàn thành lấy tất cả phiên bản: {dtos.Count} phiên bản");
            return dtos;
        }
        catch (Exception ex)
        {
            _logger?.Error($"Lỗi khi lấy tất cả phiên bản: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Tạo phiên bản mới
    /// </summary>
    /// <param name="dto">ApplicationVersionDto</param>
    /// <returns>ApplicationVersionDto đã tạo</returns>
    public ApplicationVersionDto CreateVersion(ApplicationVersionDto dto)
    {
        try
        {
            _logger?.Info($"Bắt đầu tạo phiên bản mới: {dto.Version}");
            var entity = ToEntity(dto);
            var created = GetDataAccess().Create(entity);
            var result = ToDto(created);
            _logger?.Info($"Hoàn thành tạo phiên bản mới: {result.Version}");
            return result;
        }
        catch (Exception ex)
        {
            _logger?.Error($"Lỗi khi tạo phiên bản: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Cập nhật phiên bản
    /// </summary>
    /// <param name="dto">ApplicationVersionDto</param>
    /// <returns>ApplicationVersionDto đã cập nhật</returns>
    public ApplicationVersionDto UpdateVersion(ApplicationVersionDto dto)
    {
        try
        {
            _logger?.Info($"Bắt đầu cập nhật phiên bản: {dto.Version}");
            var entity = ToEntity(dto);
            var updated = GetDataAccess().Update(entity);
            var result = ToDto(updated);
            _logger?.Info($"Hoàn thành cập nhật phiên bản: {result.Version}");
            return result;
        }
        catch (Exception ex)
        {
            _logger?.Error($"Lỗi khi cập nhật phiên bản: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Đặt một phiên bản làm Active
    /// </summary>
    /// <param name="versionId">ID phiên bản</param>
    public void SetActiveVersion(Guid versionId)
    {
        try
        {
            _logger?.Info($"Bắt đầu đặt phiên bản làm Active: {versionId}");
            GetDataAccess().SetActiveVersion(versionId);
            _logger?.Info($"Hoàn thành đặt phiên bản làm Active: {versionId}");
        }
        catch (Exception ex)
        {
            _logger?.Error($"Lỗi khi đặt phiên bản Active: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Cập nhật phiên bản từ Assembly version hiện tại
    /// </summary>
    /// <param name="description">Mô tả phiên bản</param>
    /// <param name="userId">ID người tạo/cập nhật</param>
    public void UpdateVersionFromAssembly(string description = null, Guid? userId = null)
    {
        try
        {
            var currentVersion = GetCurrentApplicationVersion();
            _logger?.Info($"Bắt đầu cập nhật phiên bản từ Assembly: {currentVersion}");

            var activeVersion = GetActiveVersion();
            
            if (activeVersion != null && activeVersion.Version == currentVersion)
            {
                _logger?.Info($"Phiên bản {currentVersion} đã tồn tại và đang Active");
                return;
            }

            // Tạo hoặc cập nhật phiên bản
            var dto = new ApplicationVersionDto
            {
                Id = activeVersion?.Id ?? Guid.NewGuid(),
                Version = currentVersion,
                ReleaseDate = DateTime.Now,
                IsActive = true,
                Description = description ?? $"Phiên bản {currentVersion}",
                CreateDate = activeVersion?.CreateDate ?? DateTime.Now,
                CreateBy = userId,
                ModifiedDate = DateTime.Now,
                ModifiedBy = userId
            };

            if (activeVersion == null)
            {
                CreateVersion(dto);
            }
            else
            {
                UpdateVersion(dto);
            }

            // Đặt làm Active
            SetActiveVersion(dto.Id);
            
            _logger?.Info($"Hoàn thành cập nhật phiên bản từ Assembly: {currentVersion}");
        }
        catch (Exception ex)
        {
            _logger?.Error($"Lỗi khi cập nhật phiên bản từ Assembly: {ex.Message}", ex);
            throw;
        }
    }

    #endregion

    #region Private Methods

    private ApplicationVersionDto ToDto(ApplicationVersion entity)
    {
        if (entity == null)
            return null;

        return new ApplicationVersionDto
        {
            Id = entity.Id,
            Version = entity.Version,
            ReleaseDate = entity.ReleaseDate,
            IsActive = entity.IsActive,
            Description = entity.Description,
            CreateDate = entity.CreateDate,
            CreateBy = entity.CreateBy,
            ModifiedDate = entity.ModifiedDate,
            ModifiedBy = entity.ModifiedBy
        };
    }

    private ApplicationVersion ToEntity(ApplicationVersionDto dto)
    {
        if (dto == null)
            return null;

        return new ApplicationVersion
        {
            Id = dto.Id,
            Version = dto.Version,
            ReleaseDate = dto.ReleaseDate,
            IsActive = dto.IsActive,
            Description = dto.Description,
            CreateDate = dto.CreateDate,
            CreateBy = dto.CreateBy,
            ModifiedDate = dto.ModifiedDate,
            ModifiedBy = dto.ModifiedBy
        };
    }

    #endregion
}
