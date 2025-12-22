using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace DTO.Inventory.InventoryManagement;

/// <summary>
/// Enum định nghĩa trạng thái thiết bị
/// </summary>
public enum DeviceStatusEnum
{
    /// <summary>
    /// Đang trong kho VNS
    /// </summary>
    [Description("Đang trong kho VNS")]
    Available = 0,

    /// <summary>
    /// Đang sử dụng tại VNS
    /// </summary>
    [Description("Đang sử dụng tại VNS")]
    InUse = 1,

    /// <summary>
    /// Đang gửi bảo hành
    /// </summary>
    [Description("Đang gửi bảo hành")]
    Maintenance = 2,

    /// <summary>
    /// Đã hỏng
    /// </summary>
    [Description("Đã hỏng")]
    Broken = 3,

    /// <summary>
    /// Đã thanh lý
    /// </summary>
    [Description("Đã thanh lý")]
    Disposed = 4,

    /// <summary>
    /// Đã giao cho khách hàng
    /// </summary>
    [Description("Đã giao cho khách hàng")]
    Reserved = 5
}

/// <summary>
/// Data Transfer Object cho Device entity
/// Chứa thông tin thiết bị: định danh, trạng thái, ghi chú
/// </summary>
public class DeviceDto
{
    #region Properties - Thông tin cơ bản

    /// <summary>
    /// ID duy nhất của thiết bị
    /// </summary>
    [DisplayName("ID")]
    [Display(Order = -1)]
    public Guid Id { get; set; }

    /// <summary>
    /// ID sản phẩm dịch vụ (ProductVariant)
    /// </summary>
    [DisplayName("ID Sản phẩm")]
    [Display(Order = 0)]
    [Required(ErrorMessage = "ID sản phẩm dịch vụ không được để trống")]
    public Guid ProductVariantId { get; set; }

    /// <summary>
    /// ID chi tiết phiếu nhập/xuất kho
    /// </summary>
    [DisplayName("ID Chi tiết phiếu")]
    [Display(Order = 1)]
    public Guid? StockInOutDetailId { get; set; }

    /// <summary>
    /// ID bảo hành
    /// </summary>
    [DisplayName("ID Bảo hành")]
    [Display(Order = 2)]
    public Guid? WarrantyId { get; set; }

    #endregion

    #region Properties - Định danh thiết bị (Device Identifiers)

    /// <summary>
    /// Số Serial Number
    /// </summary>
    [DisplayName("Serial Number")]
    [Display(Order = 10)]
    [StringLength(100, ErrorMessage = "Serial Number không được vượt quá 100 ký tự")]
    public string SerialNumber { get; set; }

    /// <summary>
    /// Địa chỉ MAC
    /// </summary>
    [DisplayName("MAC Address")]
    [Display(Order = 11)]
    [StringLength(50, ErrorMessage = "MAC Address không được vượt quá 50 ký tự")]
    public string MACAddress { get; set; }

    /// <summary>
    /// Số IMEI
    /// </summary>
    [DisplayName("IMEI")]
    [Display(Order = 12)]
    [StringLength(50, ErrorMessage = "IMEI không được vượt quá 50 ký tự")]
    public string IMEI { get; set; }

    /// <summary>
    /// Asset Tag
    /// </summary>
    [DisplayName("Asset Tag")]
    [Display(Order = 13)]
    [StringLength(50, ErrorMessage = "Asset Tag không được vượt quá 50 ký tự")]
    public string AssetTag { get; set; }

    /// <summary>
    /// License Key
    /// </summary>
    [DisplayName("License Key")]
    [Display(Order = 14)]
    [StringLength(255, ErrorMessage = "License Key không được vượt quá 255 ký tự")]
    public string LicenseKey { get; set; }

    /// <summary>
    /// Host Name (cho thiết bị mạng)
    /// </summary>
    [DisplayName("Host Name")]
    [Display(Order = 15)]
    [StringLength(100, ErrorMessage = "Host Name không được vượt quá 100 ký tự")]
    public string HostName { get; set; }

    /// <summary>
    /// Địa chỉ IP
    /// </summary>
    [DisplayName("IP Address")]
    [Display(Order = 16)]
    [StringLength(50, ErrorMessage = "IP Address không được vượt quá 50 ký tự")]
    public string IPAddress { get; set; }

    /// <summary>
    /// Lấy giá trị định danh theo DeviceIdentifierEnum
    /// </summary>
    /// <param name="identifierType">Loại định danh</param>
    /// <returns>Giá trị định danh hoặc null</returns>
    public string GetIdentifierValue(DeviceIdentifierEnum identifierType)
    {
        return identifierType switch
        {
            DeviceIdentifierEnum.SerialNumber => SerialNumber,
            DeviceIdentifierEnum.MAC => MACAddress,
            DeviceIdentifierEnum.IMEI => IMEI,
            DeviceIdentifierEnum.AssetTag => AssetTag,
            DeviceIdentifierEnum.LicenseKey => LicenseKey,
            _ => null
        };
    }

    /// <summary>
    /// Đặt giá trị định danh theo DeviceIdentifierEnum
    /// </summary>
    /// <param name="identifierType">Loại định danh</param>
    /// <param name="value">Giá trị cần đặt</param>
    public void SetIdentifierValue(DeviceIdentifierEnum identifierType, string value)
    {
        switch (identifierType)
        {
            case DeviceIdentifierEnum.SerialNumber:
                SerialNumber = value;
                break;
            case DeviceIdentifierEnum.MAC:
                MACAddress = value;
                break;
            case DeviceIdentifierEnum.IMEI:
                IMEI = value;
                break;
            case DeviceIdentifierEnum.AssetTag:
                AssetTag = value;
                break;
            case DeviceIdentifierEnum.LicenseKey:
                LicenseKey = value;
                break;
        }
    }

    /// <summary>
    /// Lấy danh sách tất cả các định danh có giá trị
    /// </summary>
    /// <returns>Dictionary chứa loại định danh và giá trị</returns>
    public Dictionary<DeviceIdentifierEnum, string> GetAllIdentifiers()
    {
        var identifiers = new Dictionary<DeviceIdentifierEnum, string>();
        
        if (!string.IsNullOrWhiteSpace(SerialNumber))
            identifiers[DeviceIdentifierEnum.SerialNumber] = SerialNumber;
        if (!string.IsNullOrWhiteSpace(MACAddress))
            identifiers[DeviceIdentifierEnum.MAC] = MACAddress;
        if (!string.IsNullOrWhiteSpace(IMEI))
            identifiers[DeviceIdentifierEnum.IMEI] = IMEI;
        if (!string.IsNullOrWhiteSpace(AssetTag))
            identifiers[DeviceIdentifierEnum.AssetTag] = AssetTag;
        if (!string.IsNullOrWhiteSpace(LicenseKey))
            identifiers[DeviceIdentifierEnum.LicenseKey] = LicenseKey;

        return identifiers;
    }

    /// <summary>
    /// Thông tin sản phẩm duy nhất (Serial Number, IMEI, v.v.) - Backward compatibility
    /// Lấy giá trị định danh đầu tiên có giá trị
    /// </summary>
    [DisplayName("Thông tin SP duy nhất")]
    [Display(Order = 7)]
    [Description("Thông tin định danh thiết bị (Serial Number, IMEI, MAC, v.v.)")]
    public string UniqueProductInfo
    {
        get
        {
            // Ưu tiên SerialNumber, sau đó IMEI, MAC, AssetTag, LicenseKey
            return SerialNumber ?? IMEI ?? MACAddress ?? AssetTag ?? LicenseKey ?? string.Empty;
        }
        set
        {
            // Nếu có giá trị và SerialNumber chưa có, đặt vào SerialNumber
            if (!string.IsNullOrWhiteSpace(value) && string.IsNullOrWhiteSpace(SerialNumber))
            {
                SerialNumber = value;
            }
        }
    }

    #endregion

    #region Properties - Trạng thái và loại thiết bị

    /// <summary>
    /// Trạng thái thiết bị
    /// </summary>
    [DisplayName("Trạng thái")]
    [Display(Order = 30)]
    [Required(ErrorMessage = "Trạng thái thiết bị không được để trống")]
    public DeviceStatusEnum Status { get; set; }

    /// <summary>
    /// Loại thiết bị (0: Hardware, 1: Software, 2: Network, 3: Other)
    /// </summary>
    [DisplayName("Loại thiết bị")]
    [Display(Order = 31)]
    [Required(ErrorMessage = "Loại thiết bị không được để trống")]
    public int DeviceType { get; set; }

    #endregion

    #region Properties - Ghi chú

    /// <summary>
    /// Ghi chú
    /// </summary>
    [DisplayName("Ghi chú")]
    [Display(Order = 50)]
    [StringLength(1000, ErrorMessage = "Ghi chú không được vượt quá 1000 ký tự")]
    public string Notes { get; set; }

    #endregion

    #region Properties - Audit & Status

    /// <summary>
    /// Đang hoạt động
    /// </summary>
    [DisplayName("Đang hoạt động")]
    [Display(Order = 70)]
    [Required(ErrorMessage = "Trạng thái hoạt động không được để trống")]
    public bool IsActive { get; set; }

    /// <summary>
    /// Ngày tạo
    /// </summary>
    [DisplayName("Ngày tạo")]
    [Display(Order = 71)]
    [Required(ErrorMessage = "Ngày tạo không được để trống")]
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Ngày cập nhật
    /// </summary>
    [DisplayName("Ngày cập nhật")]
    [Display(Order = 72)]
    public DateTime? UpdatedDate { get; set; }

    /// <summary>
    /// Người tạo
    /// </summary>
    [DisplayName("Người tạo")]
    [Display(Order = 73)]
    public Guid? CreatedBy { get; set; }

    /// <summary>
    /// Người cập nhật
    /// </summary>
    [DisplayName("Người cập nhật")]
    [Display(Order = 74)]
    public Guid? UpdatedBy { get; set; }

    #endregion

    #region Properties - Thông tin hiển thị (Display - chỉ đọc, không lưu vào database)

    /// <summary>
    /// Tên sản phẩm dịch vụ (lấy từ ProductVariant)
    /// </summary>
    [DisplayName("Tên sản phẩm")]
    [Display(Order = 100)]
    public string ProductVariantName { get; set; }

    /// <summary>
    /// Mã sản phẩm dịch vụ (lấy từ ProductVariant)
    /// </summary>
    [DisplayName("Mã sản phẩm")]
    [Display(Order = 101)]
    public string ProductVariantCode { get; set; }

    /// <summary>
    /// Tên trạng thái thiết bị (hiển thị)
    /// </summary>
    [DisplayName("Trạng thái thiết bị")]
    [Display(Order = 106)]
    public string StatusName { get; set; }

    /// <summary>
    /// Tên loại thiết bị (hiển thị)
    /// </summary>
    [DisplayName("Loại thiết bị")]
    [Display(Order = 107)]
    public string DeviceTypeName { get; set; }

    /// <summary>
    /// Tổng hợp thông tin thiết bị dưới dạng HTML (chỉ đọc)
    /// Hiển thị thông tin: tên sản phẩm, định danh, liên kết, audit
    /// Sử dụng các tag HTML chuẩn của DevExpress: &lt;color&gt;
    /// Tham khảo: https://docs.devexpress.com/WindowsForms/4874/common-features/html-text-formatting
    /// </summary>
    [DisplayName("Thông tin tổng hợp (HTML)")]
    [Display(Order = 200)]
    [Description("Tổng hợp thông tin thiết bị dưới dạng HTML")]
    public string HtmlInfo
    {
        get
        {
            var html = string.Empty;

            // Tên sản phẩm dịch vụ
            var productVariantName = ProductVariantName ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(productVariantName))
            {
                html += $"<color='blue'>{productVariantName}</color>";
                if (!string.IsNullOrWhiteSpace(ProductVariantCode))
                {
                    html += $" <color='#757575'>({ProductVariantCode})</color>";
                }
                html += "<br>";
            }

            // Định danh thiết bị (Serial Number, IMEI, MAC, v.v.)
            var identifierParts = new List<string>();
            if (!string.IsNullOrWhiteSpace(SerialNumber))
                identifierParts.Add($"Serial: {SerialNumber}");
            if (!string.IsNullOrWhiteSpace(IMEI))
                identifierParts.Add($"IMEI: {IMEI}");
            if (!string.IsNullOrWhiteSpace(MACAddress))
                identifierParts.Add($"MAC: {MACAddress}");
            if (!string.IsNullOrWhiteSpace(AssetTag))
                identifierParts.Add($"AssetTag: {AssetTag}");
            if (!string.IsNullOrWhiteSpace(LicenseKey))
                identifierParts.Add($"License: {LicenseKey}");
            if (!string.IsNullOrWhiteSpace(HostName))
                identifierParts.Add($"Host: {HostName}");
            if (!string.IsNullOrWhiteSpace(IPAddress))
                identifierParts.Add($"IP: {IPAddress}");

            if (identifierParts.Any())
            {
                html += $"<color='#757575'>Định danh:</color> <color='#212121'>{string.Join(" | ", identifierParts)}</color><br>";
            }

            // Thông tin liên kết
            var linkParts = new List<string>();
            if (StockInOutDetailId.HasValue)
                linkParts.Add($"Phiếu: {StockInOutDetailId.Value}");
            if (WarrantyId.HasValue)
                linkParts.Add($"Bảo hành: {WarrantyId.Value}");

            if (linkParts.Any())
            {
                html += $"<color='#757575'>Liên kết:</color> <color='#212121'>{string.Join(" | ", linkParts)}</color><br>";
            }

            // Thông tin audit
            var auditParts = new List<string>();
            if (CreatedDate != default(DateTime))
                auditParts.Add($"Tạo: {CreatedDate:dd/MM/yyyy HH:mm}");
            if (UpdatedDate.HasValue)
                auditParts.Add($"Cập nhật: {UpdatedDate.Value:dd/MM/yyyy HH:mm}");

            if (auditParts.Any())
            {
                html += $"<color='#9E9E9E'>{string.Join(" | ", auditParts)}</color>";
            }

            return html;
        }
    }

    /// <summary>
    /// Lấy text tương ứng với trạng thái thiết bị
    /// </summary>
    /// <param name="status">Trạng thái thiết bị</param>
    /// <returns>Text hiển thị</returns>
    private string GetStatusText(DeviceStatusEnum status)
    {
        var field = status.GetType().GetField(status.ToString());
        if (field == null) return status.ToString();

        var attribute = field.GetCustomAttribute<DescriptionAttribute>();
        return attribute?.Description ?? status.ToString();
    }

    /// <summary>
    /// Lấy màu sắc tương ứng với trạng thái thiết bị
    /// </summary>
    /// <param name="status">Trạng thái thiết bị</param>
    /// <returns>Mã màu hex</returns>
    private string GetStatusColor(DeviceStatusEnum status)
    {
        return status switch
        {
            DeviceStatusEnum.Available => "#4CAF50",      // Green - Đang trong kho VNS
            DeviceStatusEnum.InUse => "#2196F3",            // Blue - Đang sử dụng tại VNS
            DeviceStatusEnum.Maintenance => "#FF9800",     // Orange - Đang gửi bảo hành
            DeviceStatusEnum.Broken => "#F44336",            // Red - Đã hỏng
            DeviceStatusEnum.Disposed => "#9E9E9E",        // Grey - Đã thanh lý
            DeviceStatusEnum.Reserved => "#9C27B0",         // Purple - Đã giao cho khách hàng
            _ => "#212121"                                  // Default - Black
        };
    }

    #endregion
}

/// <summary>
/// Converter giữa Device entity và DeviceDto
/// </summary>
public static class DeviceDtoConverter
{
    #region Entity to DTO

    /// <summary>
    /// Chuyển đổi Device entity thành DeviceDto
    /// </summary>
    /// <param name="entity">Device entity</param>
    /// <returns>DeviceDto</returns>
    public static DeviceDto ToDto(this Dal.DataContext.Device entity)
    {
        if (entity == null) return null;

        var dto = new DeviceDto
        {
            Id = entity.Id,
            ProductVariantId = entity.ProductVariantId,
            StockInOutDetailId = entity.StockInOutDetailId,
            WarrantyId = entity.WarrantyId,
            SerialNumber = entity.SerialNumber,
            MACAddress = entity.MACAddress,
            IMEI = entity.IMEI,
            AssetTag = entity.AssetTag,
            LicenseKey = entity.LicenseKey,
            HostName = entity.HostName,
            IPAddress = entity.IPAddress,
            Status = (DeviceStatusEnum)entity.Status, // Convert int to enum
            DeviceType = entity.DeviceType,
            Notes = entity.Notes,
            IsActive = entity.IsActive,
            CreatedDate = entity.CreatedDate,
            UpdatedDate = entity.UpdatedDate,
            CreatedBy = entity.CreatedBy,
            UpdatedBy = entity.UpdatedBy
        };

        // Lấy thông tin ProductVariant nếu có
        if (entity.ProductVariant != null)
        {
            dto.ProductVariantName = entity.ProductVariant.VariantFullName;
            dto.ProductVariantCode = entity.ProductVariant.VariantCode;
        }

        return dto;
    }

    /// <summary>
    /// Chuyển đổi danh sách Device entities thành danh sách DeviceDto
    /// </summary>
    /// <param name="entities">Danh sách Device entities</param>
    /// <returns>Danh sách DeviceDto</returns>
    public static List<DeviceDto> ToDtoList(this IEnumerable<Dal.DataContext.Device> entities)
    {
        if (entities == null) return new List<DeviceDto>();
        return entities.Select(entity => entity.ToDto()).ToList();
    }

    #endregion

    #region DTO to Entity

    /// <summary>
    /// Chuyển đổi DeviceDto thành Device entity
    /// </summary>
    /// <param name="dto">DeviceDto</param>
    /// <returns>Device entity</returns>
    public static Dal.DataContext.Device ToEntity(this DeviceDto dto)
    {
        if (dto == null) return null;

        var entity = new Dal.DataContext.Device
        {
            Id = dto.Id == Guid.Empty ? Guid.NewGuid() : dto.Id,
            ProductVariantId = dto.ProductVariantId,
            StockInOutDetailId = dto.StockInOutDetailId,
            WarrantyId = dto.WarrantyId,
            SerialNumber = dto.SerialNumber,
            MACAddress = dto.MACAddress,
            IMEI = dto.IMEI,
            AssetTag = dto.AssetTag,
            LicenseKey = dto.LicenseKey,
            HostName = dto.HostName,
            IPAddress = dto.IPAddress,
            Status = (int)dto.Status, // Convert enum to int
            DeviceType = dto.DeviceType,
            Notes = dto.Notes,
            IsActive = dto.IsActive,
            CreatedDate = dto.CreatedDate == default(DateTime) ? DateTime.Now : dto.CreatedDate,
            UpdatedDate = dto.UpdatedDate,
            CreatedBy = dto.CreatedBy,
            UpdatedBy = dto.UpdatedBy
        };

        return entity;
    }

    #endregion
}
