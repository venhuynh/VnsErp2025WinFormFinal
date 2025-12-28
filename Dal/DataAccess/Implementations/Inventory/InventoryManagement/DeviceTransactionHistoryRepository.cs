using Dal.DataAccess.Interfaces.Inventory.InventoryManagement;
using Dal.DataContext;
using Dal.DtoConverter;
using DTO.DeviceAssetManagement;
using Logger;
using Logger.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using CustomLogger = Logger.Interfaces.ILogger;

namespace Dal.DataAccess.Implementations.Inventory.InventoryManagement;

/// <summary>
/// Repository cho DeviceTransactionHistory entity
/// Cung cấp các phương thức truy vấn và thao tác với lịch sử giao dịch thiết bị
/// </summary>
public class DeviceTransactionHistoryRepository : IDeviceTransactionHistoryRepository
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
    /// Khởi tạo một instance mới của class DeviceTransactionHistoryRepository.
    /// </summary>
    /// <param name="connectionString">Chuỗi kết nối cơ sở dữ liệu</param>
    /// <exception cref="ArgumentNullException">Ném ra khi connectionString là null</exception>
    public DeviceTransactionHistoryRepository(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _logger = LoggerFactory.CreateLogger(LogCategory.DAL);
        _logger.Info("DeviceTransactionHistoryRepository được khởi tạo với connection string");
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
        loadOptions.LoadWith<DeviceTransactionHistory>(h => h.Device);
        loadOptions.LoadWith<Device>(d => d.ProductVariant);
        loadOptions.LoadWith<ProductVariant>(v => v.ProductService);
        context.LoadOptions = loadOptions;

        return context;
    }

    /// <summary>
    /// Lấy dictionary thông tin Device cho danh sách DeviceTransactionHistory
    /// </summary>
    private Dictionary<Guid, (string DeviceName, string DeviceCode, string DeviceInfo)> GetDeviceDict(VnsErp2025DataContext context, IEnumerable<DeviceTransactionHistory> histories)
    {
        var deviceIds = histories
            .Where(h => h.DeviceId != Guid.Empty)
            .Select(h => h.DeviceId)
            .Distinct()
            .ToList();

        if (!deviceIds.Any())
            return new Dictionary<Guid, (string, string, string)>();

        var devices = context.Devices
            .Where(d => deviceIds.Contains(d.Id))
            .Select(d => new
            {
                d.Id,
                ProductVariantName = d.ProductVariant != null ? d.ProductVariant.VariantFullName : null,
                ProductVariantCode = d.ProductVariant != null ? d.ProductVariant.VariantCode : null,
                d.SerialNumber,
                d.IMEI,
                d.MACAddress,
                d.AssetTag,
                d.LicenseKey
            })
            .ToList();

        var result = new Dictionary<Guid, (string, string, string)>();
        foreach (var device in devices)
        {
            var deviceInfoParts = new List<string>();
            if (!string.IsNullOrWhiteSpace(device.SerialNumber))
                deviceInfoParts.Add($"Serial: {device.SerialNumber}");
            if (!string.IsNullOrWhiteSpace(device.IMEI))
                deviceInfoParts.Add($"IMEI: {device.IMEI}");
            if (!string.IsNullOrWhiteSpace(device.MACAddress))
                deviceInfoParts.Add($"MAC: {device.MACAddress}");
            if (!string.IsNullOrWhiteSpace(device.AssetTag))
                deviceInfoParts.Add($"AssetTag: {device.AssetTag}");
            if (!string.IsNullOrWhiteSpace(device.LicenseKey))
                deviceInfoParts.Add($"License: {device.LicenseKey}");

            result[device.Id] = (
                device.ProductVariantName,
                device.ProductVariantCode,
                deviceInfoParts.Any() ? string.Join(" | ", deviceInfoParts) : null
            );
        }

        return result;
    }

    #endregion

    #region ========== READ OPERATIONS ==========

    /// <summary>
    /// Lấy DeviceTransactionHistory theo ID
    /// </summary>
    /// <param name="id">ID của bản ghi lịch sử</param>
    /// <returns>DeviceTransactionHistoryDto hoặc null</returns>
    public DeviceTransactionHistoryDto GetById(Guid id)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetById: Lấy lịch sử giao dịch, Id={0}", id);

            var entity = context.DeviceTransactionHistories.FirstOrDefault(h => h.Id == id);

            if (entity == null)
            {
                _logger.Warning("GetById: Không tìm thấy lịch sử giao dịch, Id={0}", id);
                return null;
            }

            // Fetch related data trước khi convert để tránh DataContext disposed errors
            string deviceName = null;
            string deviceCode = null;
            string deviceInfo = null;
            if (entity.DeviceId != Guid.Empty)
            {
                var device = context.Devices.FirstOrDefault(d => d.Id == entity.DeviceId);
                if (device != null)
                {
                    if (device.ProductVariant != null)
                    {
                        deviceName = device.ProductVariant.VariantFullName;
                        deviceCode = device.ProductVariant.VariantCode;
                    }

                    var deviceInfoParts = new List<string>();
                    if (!string.IsNullOrWhiteSpace(device.SerialNumber))
                        deviceInfoParts.Add($"Serial: {device.SerialNumber}");
                    if (!string.IsNullOrWhiteSpace(device.IMEI))
                        deviceInfoParts.Add($"IMEI: {device.IMEI}");
                    if (!string.IsNullOrWhiteSpace(device.MACAddress))
                        deviceInfoParts.Add($"MAC: {device.MACAddress}");
                    if (!string.IsNullOrWhiteSpace(device.AssetTag))
                        deviceInfoParts.Add($"AssetTag: {device.AssetTag}");
                    if (!string.IsNullOrWhiteSpace(device.LicenseKey))
                        deviceInfoParts.Add($"License: {device.LicenseKey}");
                    if (deviceInfoParts.Any())
                        deviceInfo = string.Join(" | ", deviceInfoParts);
                }
            }

            _logger.Info("GetById: Lấy lịch sử giao dịch thành công, Id={0}", id);
            return entity.ToDto(deviceName, deviceCode, deviceInfo);
        }
        catch (Exception ex)
        {
            _logger.Error($"GetById: Lỗi lấy lịch sử giao dịch: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Lấy danh sách lịch sử giao dịch theo DeviceId
    /// </summary>
    /// <param name="deviceId">ID thiết bị</param>
    /// <returns>Danh sách DeviceTransactionHistoryDto, sắp xếp theo ngày mới nhất</returns>
    public List<DeviceTransactionHistoryDto> GetByDeviceId(Guid deviceId)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetByDeviceId: Lấy danh sách lịch sử giao dịch, DeviceId={0}", deviceId);

            var entities = context.DeviceTransactionHistories
                .Where(h => h.DeviceId == deviceId)
                .OrderByDescending(h => h.OperationDate)
                .ThenByDescending(h => h.CreatedDate)
                .ToList();

            // Fetch related data trước khi convert để tránh DataContext disposed errors
            var deviceDict = GetDeviceDict(context, entities);

            _logger.Info("GetByDeviceId: Lấy được {0} bản ghi lịch sử", entities.Count);
            return entities.ToDtoList(deviceDict);
        }
        catch (Exception ex)
        {
            _logger.Error($"GetByDeviceId: Lỗi lấy danh sách lịch sử giao dịch: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Lấy danh sách lịch sử giao dịch theo loại thao tác
    /// </summary>
    /// <param name="operationType">Loại thao tác (0=Import, 1=Export, 2=Allocation, 3=Recovery, 4=Transfer, 5=Maintenance, 6=StatusChange, 7=Other)</param>
    /// <returns>Danh sách DeviceTransactionHistoryDto</returns>
    public List<DeviceTransactionHistoryDto> GetByOperationType(int operationType)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetByOperationType: Lấy danh sách lịch sử giao dịch, OperationType={0}", operationType);

            var entities = context.DeviceTransactionHistories
                .Where(h => h.OperationType == operationType)
                .OrderByDescending(h => h.OperationDate)
                .ThenByDescending(h => h.CreatedDate)
                .ToList();

            // Fetch related data trước khi convert để tránh DataContext disposed errors
            var deviceDict = GetDeviceDict(context, entities);

            _logger.Info("GetByOperationType: Lấy được {0} bản ghi lịch sử", entities.Count);
            return entities.ToDtoList(deviceDict);
        }
        catch (Exception ex)
        {
            _logger.Error($"GetByOperationType: Lỗi lấy danh sách lịch sử giao dịch: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Query lịch sử giao dịch với các filter
    /// </summary>
    /// <param name="deviceId">ID thiết bị (nullable)</param>
    /// <param name="operationType">Loại thao tác (nullable)</param>
    /// <param name="fromDate">Từ ngày (nullable)</param>
    /// <param name="toDate">Đến ngày (nullable)</param>
    /// <param name="referenceId">ID tham chiếu (nullable)</param>
    /// <param name="referenceType">Loại tham chiếu (nullable)</param>
    /// <param name="performedBy">Người thực hiện (nullable)</param>
    /// <param name="keyword">Từ khóa tìm kiếm trong Information (nullable)</param>
    /// <returns>Danh sách DeviceTransactionHistoryDto, sắp xếp theo ngày mới nhất</returns>
    public List<DeviceTransactionHistoryDto> Query(
        Guid? deviceId = null,
        int? operationType = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        Guid? referenceId = null,
        int? referenceType = null,
        Guid? performedBy = null,
        string keyword = null)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("Query: Bắt đầu query lịch sử giao dịch, DeviceId={0}, OperationType={1}, FromDate={2}, ToDate={3}, ReferenceId={4}, ReferenceType={5}, PerformedBy={6}, Keyword={7}",
                deviceId, operationType, fromDate, toDate, referenceId, referenceType, performedBy, keyword ?? "null");

            var query = context.DeviceTransactionHistories.AsQueryable();

            // Filter theo DeviceId
            if (deviceId.HasValue)
            {
                query = query.Where(h => h.DeviceId == deviceId.Value);
            }

            // Filter theo OperationType
            if (operationType.HasValue)
            {
                query = query.Where(h => h.OperationType == operationType.Value);
            }

            // Filter theo khoảng thời gian
            if (fromDate.HasValue || toDate.HasValue)
            {
                if (fromDate.HasValue && toDate.HasValue)
                {
                    query = query.Where(h => h.OperationDate.Date >= fromDate.Value.Date &&
                                            h.OperationDate.Date <= toDate.Value.Date);
                }
                else if (fromDate.HasValue)
                {
                    query = query.Where(h => h.OperationDate.Date >= fromDate.Value.Date);
                }
                else if (toDate.HasValue)
                {
                    query = query.Where(h => h.OperationDate.Date <= toDate.Value.Date);
                }
            }

            // Filter theo ReferenceId
            if (referenceId.HasValue)
            {
                query = query.Where(h => h.ReferenceId.HasValue && h.ReferenceId.Value == referenceId.Value);
            }

            // Filter theo ReferenceType
            if (referenceType.HasValue)
            {
                query = query.Where(h => h.ReferenceType.HasValue && h.ReferenceType.Value == referenceType.Value);
            }

            // Filter theo PerformedBy
            if (performedBy.HasValue)
            {
                query = query.Where(h => h.PerformedBy.HasValue && h.PerformedBy.Value == performedBy.Value);
            }

            // Filter theo keyword (tìm trong Information)
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var searchText = keyword.Trim();
                query = query.Where(h =>
                    (h.Information != null && h.Information.Contains(searchText)) ||
                    (h.Notes != null && h.Notes.Contains(searchText)));
            }

            // Sắp xếp theo ngày mới nhất
            var entities = query
                .OrderByDescending(h => h.OperationDate)
                .ThenByDescending(h => h.CreatedDate)
                .ToList();

            // Fetch related data trước khi convert để tránh DataContext disposed errors
            var deviceDict = GetDeviceDict(context, entities);

            _logger.Info("Query: Query thành công, ResultCount={0}", entities.Count);
            return entities.ToDtoList(deviceDict);
        }
        catch (Exception ex)
        {
            _logger.Error($"Query: Lỗi query lịch sử giao dịch: {ex.Message}", ex);
            throw;
        }
    }

    #endregion

    #region ========== CREATE/UPDATE OPERATIONS ==========

    /// <summary>
    /// Lưu hoặc cập nhật DeviceTransactionHistory
    /// </summary>
    /// <param name="dto">DeviceTransactionHistoryDto cần lưu</param>
    /// <returns>DeviceTransactionHistoryDto đã được lưu</returns>
    public DeviceTransactionHistoryDto SaveOrUpdate(DeviceTransactionHistoryDto dto)
    {
        using var context = CreateNewContext();
        try
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            _logger.Debug("SaveOrUpdate: Bắt đầu lưu lịch sử giao dịch, Id={0}, DeviceId={1}, OperationType={2}",
                dto.Id, dto.DeviceId, dto.OperationType);

            var existingEntity = dto.Id != Guid.Empty ? 
                context.DeviceTransactionHistories.FirstOrDefault(h => h.Id == dto.Id) : null;

            DeviceTransactionHistory entity;
            if (existingEntity == null)
            {
                // Thêm mới
                entity = dto.ToEntity();
                if (entity.Id == Guid.Empty)
                {
                    entity.Id = Guid.NewGuid();
                    dto.Id = entity.Id;
                }
                if (entity.CreatedDate == default(DateTime))
                {
                    entity.CreatedDate = DateTime.Now;
                    dto.CreatedDate = entity.CreatedDate;
                }
                if (entity.OperationDate == default(DateTime))
                {
                    entity.OperationDate = DateTime.Now;
                    dto.OperationDate = entity.OperationDate;
                }
                context.DeviceTransactionHistories.InsertOnSubmit(entity);
                _logger.Info("SaveOrUpdate: Thêm mới lịch sử giao dịch, Id={0}", entity.Id);
            }
            else
            {
                // Cập nhật
                dto.ToEntity(existingEntity);
                entity = existingEntity;
                _logger.Info("SaveOrUpdate: Cập nhật lịch sử giao dịch, Id={0}", entity.Id);
            }

            context.SubmitChanges();
            _logger.Info("SaveOrUpdate: Lưu lịch sử giao dịch thành công, Id={0}", entity.Id);

            // Load lại entity và fetch related data để convert sang DTO
            var savedEntity = context.DeviceTransactionHistories.FirstOrDefault(h => h.Id == entity.Id);
            if (savedEntity == null)
                return null;

            // Fetch related data trước khi convert để tránh DataContext disposed errors
            string deviceName = null;
            string deviceCode = null;
            string deviceInfo = null;
            if (savedEntity.DeviceId != Guid.Empty)
            {
                var device = context.Devices.FirstOrDefault(d => d.Id == savedEntity.DeviceId);
                if (device != null)
                {
                    if (device.ProductVariant != null)
                    {
                        deviceName = device.ProductVariant.VariantFullName;
                        deviceCode = device.ProductVariant.VariantCode;
                    }

                    var deviceInfoParts = new List<string>();
                    if (!string.IsNullOrWhiteSpace(device.SerialNumber))
                        deviceInfoParts.Add($"Serial: {device.SerialNumber}");
                    if (!string.IsNullOrWhiteSpace(device.IMEI))
                        deviceInfoParts.Add($"IMEI: {device.IMEI}");
                    if (!string.IsNullOrWhiteSpace(device.MACAddress))
                        deviceInfoParts.Add($"MAC: {device.MACAddress}");
                    if (!string.IsNullOrWhiteSpace(device.AssetTag))
                        deviceInfoParts.Add($"AssetTag: {device.AssetTag}");
                    if (!string.IsNullOrWhiteSpace(device.LicenseKey))
                        deviceInfoParts.Add($"License: {device.LicenseKey}");
                    if (deviceInfoParts.Any())
                        deviceInfo = string.Join(" | ", deviceInfoParts);
                }
            }

            return savedEntity.ToDto(deviceName, deviceCode, deviceInfo);
        }
        catch (Exception ex)
        {
            _logger.Error($"SaveOrUpdate: Lỗi lưu lịch sử giao dịch: {ex.Message}", ex);
            throw;
        }
    }

    #endregion

    #region ========== DELETE OPERATIONS ==========

    /// <summary>
    /// Xóa DeviceTransactionHistory
    /// </summary>
    /// <param name="id">ID của bản ghi cần xóa</param>
    public void Delete(Guid id)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("Delete: Bắt đầu xóa lịch sử giao dịch, Id={0}", id);

            var history = context.DeviceTransactionHistories.FirstOrDefault(h => h.Id == id);
            if (history == null)
            {
                _logger.Warning("Delete: Không tìm thấy lịch sử giao dịch với Id={0}", id);
                return;
            }

            context.DeviceTransactionHistories.DeleteOnSubmit(history);
            context.SubmitChanges();

            _logger.Info("Delete: Đã xóa lịch sử giao dịch, Id={0}", id);
        }
        catch (Exception ex)
        {
            _logger.Error($"Delete: Lỗi xóa lịch sử giao dịch: {ex.Message}", ex);
            throw;
        }
    }

    #endregion
}
