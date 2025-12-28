using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using Dal.DataAccess.Interfaces.Inventory.InventoryManagement;
using Dal.DataContext;
using Dal.Exceptions;
using Logger;
using Logger.Configuration;
using CustomLogger = Logger.Interfaces.ILogger;

namespace Dal.DataAccess.Implementations.Inventory.InventoryManagement;

/// <summary>
/// Data Access Layer cho DeviceImage
/// </summary>
public class DeviceImageRepository : IDeviceImageRepository
{
    #region Private Fields

    /// <summary>
    /// Chuỗi kết nối cơ sở dữ liệu để tạo DataContext mới cho mỗi operation.
    /// </summary>
    private readonly string _connectionString;

    /// <summary>
    /// Instance logger để theo dõi các thao tác và lỗi
    /// </summary>
    private readonly CustomLogger _logger;

    #endregion

    #region Constructor

    /// <summary>
    /// Khởi tạo một instance mới của class DeviceImageRepository.
    /// </summary>
    /// <param name="connectionString">Chuỗi kết nối cơ sở dữ liệu</param>
    /// <exception cref="ArgumentNullException">Ném ra khi connectionString là null</exception>
    public DeviceImageRepository(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _logger = LoggerFactory.CreateLogger(LogCategory.DAL);
        _logger.Info("DeviceImageRepository được khởi tạo với connection string");
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Tạo DataContext mới cho mỗi operation để tránh cache issue
    /// </summary>
    /// <returns>DataContext mới</returns>
    private VnsErp2025DataContext CreateNewContext()
    {
        var context = new VnsErp2025DataContext(_connectionString);

        // Configure eager loading cho navigation properties
        var loadOptions = new DataLoadOptions();
        loadOptions.LoadWith<DeviceImage>(d => d.Device);
        context.LoadOptions = loadOptions;

        return context;
    }

    #endregion

    #region ========== READ OPERATIONS ==========

    /// <summary>
    /// Lấy tất cả hình ảnh
    /// </summary>
    /// <returns>Danh sách tất cả hình ảnh metadata</returns>
    public List<DeviceImage> GetAll()
    {
        try
        {
            using var context = CreateNewContext();
            
            // Load tất cả entities từ database
            var entities = context.DeviceImages
                .OrderBy(x => x.DeviceId)
                .ThenBy(x => x.CreateDate)
                .ToList();
            
            _logger.Debug($"Đã lấy {entities.Count} hình ảnh từ tất cả thiết bị");
            return entities;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy tất cả hình ảnh: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy tất cả hình ảnh: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Lấy danh sách hình ảnh của thiết bị
    /// </summary>
    /// <param name="deviceId">ID thiết bị</param>
    /// <returns>Danh sách hình ảnh metadata</returns>
    public List<DeviceImage> GetByDeviceId(Guid deviceId)
    {
        try
        {
            using var context = CreateNewContext();
            
            // Load entities trực tiếp từ database
            var entities = context.DeviceImages
                .Where(x => x.DeviceId == deviceId)
                .OrderBy(x => x.DisplayOrder)
                .ThenBy(x => x.CreateDate)
                .ToList();
            
            _logger.Debug($"Đã lấy {entities.Count} hình ảnh cho thiết bị: {deviceId}");
            return entities;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy danh sách hình ảnh cho thiết bị '{deviceId}': {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy danh sách hình ảnh cho thiết bị '{deviceId}': {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Lấy hình ảnh theo ID
    /// </summary>
    /// <param name="imageId">ID hình ảnh</param>
    /// <returns>Hình ảnh hoặc null</returns>
    public DeviceImage GetById(Guid imageId)
    {
        try
        {
            using var context = CreateNewContext();
            
            // Load entity trực tiếp từ database
            var entity = context.DeviceImages
                .FirstOrDefault(x => x.Id == imageId);

            if (entity == null)
            {
                _logger.Debug($"Không tìm thấy hình ảnh với ID: {imageId}");
                return null;
            }
            
            _logger.Debug($"Đã lấy hình ảnh theo ID: {imageId} - {entity.FileName ?? entity.RelativePath ?? "N/A"}");
            return entity;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy hình ảnh '{imageId}': {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy hình ảnh '{imageId}': {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Lấy hình ảnh chính của thiết bị
    /// </summary>
    /// <param name="deviceId">ID thiết bị</param>
    /// <returns>Hình ảnh chính hoặc null</returns>
    public DeviceImage GetPrimaryByDeviceId(Guid deviceId)
    {
        try
        {
            using var context = CreateNewContext();
            
            // Load entity trực tiếp từ database - lấy hình ảnh có IsPrimary = true hoặc hình ảnh đầu tiên
            var entity = context.DeviceImages
                .Where(x => x.DeviceId == deviceId)
                .OrderByDescending(x => x.IsPrimary)
                .ThenBy(x => x.DisplayOrder)
                .ThenBy(x => x.CreateDate)
                .FirstOrDefault();

            if (entity == null)
            {
                _logger.Debug($"Không tìm thấy hình ảnh cho thiết bị: {deviceId}");
                return null;
            }
            
            _logger.Debug($"Đã lấy hình ảnh chính cho thiết bị: {deviceId} - {entity.FileName ?? entity.RelativePath ?? "N/A"}");
            return entity;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy hình ảnh chính cho thiết bị '{deviceId}': {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy hình ảnh chính cho thiết bị '{deviceId}': {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Tìm kiếm hình ảnh theo danh sách DeviceId
    /// </summary>
    /// <param name="deviceIds">Danh sách ID thiết bị</param>
    /// <returns>Danh sách hình ảnh phù hợp</returns>
    public List<DeviceImage> SearchByDeviceIds(List<Guid> deviceIds)
    {
        try
        {
            if (deviceIds == null || !deviceIds.Any())
            {
                _logger.Debug("SearchByDeviceIds: Danh sách deviceIds rỗng");
                return new List<DeviceImage>();
            }

            using var context = CreateNewContext();
            
            // Load entities trực tiếp từ database
            var entities = context.DeviceImages
                .Where(x => deviceIds.Contains(x.DeviceId))
                .OrderBy(x => x.DeviceId)
                .ThenBy(x => x.DisplayOrder)
                .ThenBy(x => x.CreateDate)
                .ToList();
            
            _logger.Debug($"Đã tìm kiếm {entities.Count} hình ảnh cho {deviceIds.Count} thiết bị");
            return entities;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi tìm kiếm hình ảnh theo thiết bị: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi tìm kiếm hình ảnh theo thiết bị: {ex.Message}", ex);
        }
    }

    #endregion

    #region ========== CREATE/UPDATE OPERATIONS ==========

    /// <summary>
    /// Lưu hoặc cập nhật hình ảnh
    /// </summary>
    /// <param name="deviceImage">Hình ảnh cần lưu</param>
    public void SaveOrUpdate(DeviceImage deviceImage)
    {
        try
        {
            if (deviceImage == null)
                throw new ArgumentNullException(nameof(deviceImage));

            using var context = CreateNewContext();

            var existing = deviceImage.Id != Guid.Empty ? 
                context.DeviceImages.FirstOrDefault(x => x.Id == deviceImage.Id) : null;

            if (existing == null)
            {
                // Thêm mới
                if (deviceImage.Id == Guid.Empty)
                    deviceImage.Id = Guid.NewGuid();
                
                // Thiết lập giá trị mặc định
                if (deviceImage.CreateDate == default(DateTime))
                    deviceImage.CreateDate = DateTime.Now;
                if (deviceImage.CreateBy == Guid.Empty)
                    deviceImage.CreateBy = Guid.Empty; // Cần set từ context user
                if (deviceImage.ModifiedBy == Guid.Empty)
                    deviceImage.ModifiedBy = Guid.Empty; // Cần set từ context user

                context.DeviceImages.InsertOnSubmit(deviceImage);
                context.SubmitChanges();
                
                _logger.Info($"Đã thêm mới hình ảnh: {deviceImage.Id} - {deviceImage.FileName ?? deviceImage.RelativePath ?? "N/A"}");
            }
            else
            {
                // Cập nhật
                existing.DeviceId = deviceImage.DeviceId;
                existing.ImageData = deviceImage.ImageData;
                existing.FileName = deviceImage.FileName;
                existing.RelativePath = deviceImage.RelativePath;
                existing.FullPath = deviceImage.FullPath;
                existing.StorageType = deviceImage.StorageType;
                existing.FileSize = deviceImage.FileSize;
                existing.FileExtension = deviceImage.FileExtension;
                existing.MimeType = deviceImage.MimeType;
                existing.Checksum = deviceImage.Checksum;
                existing.FileExists = deviceImage.FileExists;
                existing.LastVerified = deviceImage.LastVerified;
                existing.MigrationStatus = deviceImage.MigrationStatus;
                existing.Caption = deviceImage.Caption;
                existing.AltText = deviceImage.AltText;
                existing.IsPrimary = deviceImage.IsPrimary;
                existing.DisplayOrder = deviceImage.DisplayOrder;
                existing.IsActive = deviceImage.IsActive;
                existing.ModifiedDate = DateTime.Now;
                existing.ModifiedBy = deviceImage.ModifiedBy;
                
                context.SubmitChanges();
                
                _logger.Info($"Đã cập nhật hình ảnh: {existing.Id} - {existing.FileName ?? existing.RelativePath ?? "N/A"}");
            }
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lưu hình ảnh '{deviceImage?.FileName ?? deviceImage?.RelativePath ?? "N/A"}': {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lưu hình ảnh: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Đặt hình ảnh làm hình ảnh chính
    /// </summary>
    /// <param name="imageId">ID hình ảnh</param>
    public void SetAsPrimary(Guid imageId)
    {
        try
        {
            using var context = CreateNewContext();
            var image = context.DeviceImages.FirstOrDefault(x => x.Id == imageId);
            if (image != null)
            {
                // Bỏ IsPrimary của tất cả hình ảnh khác của cùng thiết bị
                var otherImages = context.DeviceImages
                    .Where(x => x.DeviceId == image.DeviceId && x.Id != imageId)
                    .ToList();
                
                foreach (var otherImage in otherImages)
                {
                    otherImage.IsPrimary = false;
                }
                
                // Đặt hình ảnh này làm chính
                image.IsPrimary = true;
                image.ModifiedDate = DateTime.Now;
                
                context.SubmitChanges();
                
                _logger.Info($"Đã đánh dấu hình ảnh làm chính: {imageId} - {image.FileName ?? image.RelativePath ?? "N/A"}");
            }
            else
            {
                _logger.Warning($"Không tìm thấy hình ảnh để đặt làm chính: {imageId}");
            }
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi đặt hình ảnh làm chính '{imageId}': {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi đặt hình ảnh làm chính '{imageId}': {ex.Message}", ex);
        }
    }

    #endregion

    #region ========== DELETE OPERATIONS ==========

    /// <summary>
    /// Xóa hình ảnh (soft delete nếu có IsActive, hoặc hard delete)
    /// </summary>
    /// <param name="imageId">ID hình ảnh</param>
    public void Delete(Guid imageId)
    {
        try
        {
            using var context = CreateNewContext();
            var image = context.DeviceImages.FirstOrDefault(x => x.Id == imageId);
            if (image != null)
            {
                // Soft delete nếu có IsActive property
                if (image.IsActive.HasValue)
                {
                    image.IsActive = false;
                    image.ModifiedDate = DateTime.Now;
                    context.SubmitChanges();
                    _logger.Info($"Đã soft delete hình ảnh: {imageId} - {image.FileName ?? image.RelativePath ?? "N/A"}");
                }
                else
                {
                    // Hard delete nếu không có IsActive
                    context.DeviceImages.DeleteOnSubmit(image);
                    context.SubmitChanges();
                    _logger.Info($"Đã xóa hình ảnh: {imageId} - {image.FileName ?? image.RelativePath ?? "N/A"}");
                }
            }
            else
            {
                _logger.Warning($"Không tìm thấy hình ảnh để xóa: {imageId}");
            }
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi xóa hình ảnh '{imageId}': {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi xóa hình ảnh '{imageId}': {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Xóa vĩnh viễn hình ảnh
    /// </summary>
    /// <param name="imageId">ID hình ảnh</param>
    public void DeletePermanent(Guid imageId)
    {
        try
        {
            using var context = CreateNewContext();
            var image = context.DeviceImages.FirstOrDefault(x => x.Id == imageId);
            if (image != null)
            {
                context.DeviceImages.DeleteOnSubmit(image);
                context.SubmitChanges();
                _logger.Info($"Đã xóa vĩnh viễn hình ảnh: {imageId} - {image.FileName ?? image.RelativePath ?? "N/A"}");
            }
            else
            {
                _logger.Warning($"Không tìm thấy hình ảnh để xóa vĩnh viễn: {imageId}");
            }
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi xóa vĩnh viễn hình ảnh '{imageId}': {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi xóa vĩnh viễn hình ảnh '{imageId}': {ex.Message}", ex);
        }
    }

    #endregion
}

