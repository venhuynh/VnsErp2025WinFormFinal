using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Common.Enums;

namespace DTO.Inventory.InventoryManagement;

/// <summary>
/// Data Transfer Object cho Device entity
/// Chứa đầy đủ thông tin thiết bị bao gồm định danh, vị trí, trạng thái, bảo hành
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

    #region Properties - Vị trí và phân bổ (Location & Assignment)

    /// <summary>
    /// ID công ty
    /// </summary>
    [DisplayName("ID Công ty")]
    [Display(Order = 20)]
    public Guid? CompanyId { get; set; }

    /// <summary>
    /// ID chi nhánh
    /// </summary>
    [DisplayName("ID Chi nhánh")]
    [Display(Order = 21)]
    public Guid? BranchId { get; set; }

    /// <summary>
    /// ID phòng ban
    /// </summary>
    [DisplayName("ID Phòng ban")]
    [Display(Order = 22)]
    public Guid? DepartmentId { get; set; }

    /// <summary>
    /// ID nhân viên được gán
    /// </summary>
    [DisplayName("ID Nhân viên")]
    [Display(Order = 23)]
    public Guid? AssignedEmployeeId { get; set; }

    /// <summary>
    /// Vị trí cụ thể (phòng, tầng, v.v.)
    /// </summary>
    [DisplayName("Vị trí")]
    [Display(Order = 24)]
    [StringLength(500, ErrorMessage = "Vị trí không được vượt quá 500 ký tự")]
    public string Location { get; set; }

    #endregion

    #region Properties - Trạng thái và loại thiết bị

    /// <summary>
    /// Trạng thái thiết bị (0: Available, 1: InUse, 2: Maintenance, 3: Broken, 4: Disposed, 5: Reserved)
    /// </summary>
    [DisplayName("Trạng thái")]
    [Display(Order = 30)]
    [Required(ErrorMessage = "Trạng thái thiết bị không được để trống")]
    public int Status { get; set; }

    /// <summary>
    /// Loại thiết bị (0: Hardware, 1: Software, 2: Network, 3: Other)
    /// </summary>
    [DisplayName("Loại thiết bị")]
    [Display(Order = 31)]
    [Required(ErrorMessage = "Loại thiết bị không được để trống")]
    public int DeviceType { get; set; }

    #endregion

    #region Properties - Thông tin ngày tháng

    /// <summary>
    /// Ngày mua
    /// </summary>
    [DisplayName("Ngày mua")]
    [Display(Order = 40)]
    public DateTime? PurchaseDate { get; set; }

    /// <summary>
    /// Ngày lắp đặt/cài đặt
    /// </summary>
    [DisplayName("Ngày lắp đặt")]
    [Display(Order = 41)]
    public DateTime? InstallationDate { get; set; }

    /// <summary>
    /// Ngày bảo trì cuối
    /// </summary>
    [DisplayName("Ngày bảo trì cuối")]
    [Display(Order = 42)]
    public DateTime? LastMaintenanceDate { get; set; }

    /// <summary>
    /// Ngày bảo trì tiếp theo
    /// </summary>
    [DisplayName("Ngày bảo trì tiếp theo")]
    [Display(Order = 43)]
    public DateTime? NextMaintenanceDate { get; set; }

    #endregion

    #region Properties - Thông tin bổ sung

    /// <summary>
    /// Nhà sản xuất
    /// </summary>
    [DisplayName("Nhà sản xuất")]
    [Display(Order = 50)]
    [StringLength(255, ErrorMessage = "Nhà sản xuất không được vượt quá 255 ký tự")]
    public string Manufacturer { get; set; }

    /// <summary>
    /// Model
    /// </summary>
    [DisplayName("Model")]
    [Display(Order = 51)]
    [StringLength(255, ErrorMessage = "Model không được vượt quá 255 ký tự")]
    public string Model { get; set; }

    /// <summary>
    /// Thông số kỹ thuật chi tiết
    /// </summary>
    [DisplayName("Thông số kỹ thuật")]
    [Display(Order = 52)]
    [StringLength(2000, ErrorMessage = "Thông số kỹ thuật không được vượt quá 2000 ký tự")]
    public string Specifications { get; set; }

    /// <summary>
    /// Cấu hình (cho phần mềm)
    /// </summary>
    [DisplayName("Cấu hình")]
    [Display(Order = 53)]
    [StringLength(2000, ErrorMessage = "Cấu hình không được vượt quá 2000 ký tự")]
    public string Configuration { get; set; }

    /// <summary>
    /// Ghi chú
    /// </summary>
    [DisplayName("Ghi chú")]
    [Display(Order = 54)]
    [StringLength(1000, ErrorMessage = "Ghi chú không được vượt quá 1000 ký tự")]
    public string Notes { get; set; }

    #endregion

    #region Properties - Bảo hành (Warranty Information)

    /// <summary>
    /// Kiểu bảo hành
    /// </summary>
    [DisplayName("Kiểu BH")]
    [Display(Order = 60)]
    public LoaiBaoHanhEnum? WarrantyType { get; set; }

    /// <summary>
    /// Ngày bắt đầu bảo hành
    /// </summary>
    [DisplayName("Ngày bắt đầu BH")]
    [Display(Order = 61)]
    public DateTime? WarrantyFrom { get; set; }

    /// <summary>
    /// Số tháng bảo hành
    /// </summary>
    [DisplayName("Số tháng BH")]
    [Display(Order = 62)]
    [Range(0, int.MaxValue, ErrorMessage = "Số tháng bảo hành phải lớn hơn hoặc bằng 0")]
    public int? MonthOfWarranty { get; set; }

    /// <summary>
    /// Ngày kết thúc bảo hành
    /// </summary>
    [DisplayName("Ngày kết thúc BH")]
    [Display(Order = 63)]
    public DateTime? WarrantyUntil { get; set; }

    /// <summary>
    /// Trạng thái bảo hành
    /// </summary>
    [DisplayName("Trạng thái BH")]
    [Display(Order = 64)]
    public TrangThaiBaoHanhEnum? WarrantyStatus { get; set; }

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

    #region Properties - Thông tin hiển thị (Display)

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
    /// Tên công ty (hiển thị)
    /// </summary>
    [DisplayName("Công ty")]
    [Display(Order = 102)]
    public string CompanyName { get; set; }

    /// <summary>
    /// Tên chi nhánh (hiển thị)
    /// </summary>
    [DisplayName("Chi nhánh")]
    [Display(Order = 103)]
    public string BranchName { get; set; }

    /// <summary>
    /// Tên phòng ban (hiển thị)
    /// </summary>
    [DisplayName("Phòng ban")]
    [Display(Order = 104)]
    public string DepartmentName { get; set; }

    /// <summary>
    /// Tên nhân viên được gán (hiển thị)
    /// </summary>
    [DisplayName("Nhân viên")]
    [Display(Order = 105)]
    public string AssignedEmployeeName { get; set; }

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
    /// Tên kiểu bảo hành (hiển thị)
    /// </summary>
    [DisplayName("Kiểu BH")]
    [Display(Order = 108)]
    public string WarrantyTypeName { get; set; }

    /// <summary>
    /// Tên trạng thái bảo hành (hiển thị)
    /// </summary>
    [DisplayName("Trạng thái BH")]
    [Display(Order = 109)]
    public string WarrantyStatusName { get; set; }

    /// <summary>
    /// Kiểm tra bảo hành đã hết hạn chưa (chỉ đọc)
    /// </summary>
    [DisplayName("Hết hạn BH")]
    [Display(Order = 13)]
    [Description("True nếu bảo hành đã hết hạn, False nếu còn bảo hành")]
    public bool IsWarrantyExpired
    {
        get
        {
            // Nếu không có ngày hết hạn, coi như chưa hết hạn
            if (!WarrantyUntil.HasValue)
                return false;

            // So sánh với ngày hiện tại (chỉ so sánh ngày, không so sánh giờ)
            var today = DateTime.Now.Date;
            var warrantyUntilDate = WarrantyUntil.Value.Date;

            return warrantyUntilDate < today;
        }
    }

    /// <summary>
    /// Tình trạng bảo hành (chỉ đọc) - "Còn bảo hành" hoặc "Hết hạn bảo hành"
    /// </summary>
    [DisplayName("Tình trạng BH")]
    [Display(Order = 14)]
    [Description("Tình trạng bảo hành hiện tại")]
    public string WarrantyStatusText
    {
        get
        {
            if (IsWarrantyExpired)
                return "Hết hạn bảo hành";

            // Nếu không có ngày hết hạn, không thể xác định
            if (!WarrantyUntil.HasValue)
                return "Chưa xác định";

            return "Còn bảo hành";
        }
    }

    /// <summary>
    /// Tổng hợp thông tin thiết bị dưới dạng HTML (chỉ đọc)
    /// Hiển thị đầy đủ thông tin: tên sản phẩm, định danh, vị trí, trạng thái, bảo hành
    /// Sử dụng các tag HTML chuẩn của DevExpress: &lt;b&gt;, &lt;i&gt;, &lt;color&gt;, &lt;size&gt;
    /// Tham khảo: https://docs.devexpress.com/WindowsForms/4874/common-features/html-text-formatting
    /// </summary>
    [DisplayName("Thông tin tổng hợp")]
    [Display(Order = 200)]
    [Description("Tổng hợp đầy đủ thông tin thiết bị dưới dạng HTML")]
    public string FullInfo
    {
        get
        {
            var html = string.Empty;

            // Tên sản phẩm dịch vụ (nổi bật nhất)
            var productVariantName = ProductVariantName ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(productVariantName))
            {
                html += $"<size=12><b><color='blue'>{productVariantName}</color></b></size>";
                html += "<br>";
            }

            // Định danh thiết bị (Serial Number, IMEI, MAC, v.v.)
            var identifiers = GetAllIdentifiers();
            if (identifiers.Any())
            {
                var identifierParts = identifiers.Select(kvp => 
                    $"{GetIdentifierDisplayName(kvp.Key)}: {kvp.Value}");
                html += $"<size=9><color='#757575'>Định danh:</color></size> <size=10><color='#212121'><b>{string.Join(" | ", identifierParts)}</b></color></size><br>";
            }

            // Vị trí và phân bổ
            var locationParts = new List<string>();
            if (!string.IsNullOrWhiteSpace(CompanyName))
                locationParts.Add($"Công ty: {CompanyName}");
            if (!string.IsNullOrWhiteSpace(BranchName))
                locationParts.Add($"Chi nhánh: {BranchName}");
            if (!string.IsNullOrWhiteSpace(DepartmentName))
                locationParts.Add($"Phòng ban: {DepartmentName}");
            if (!string.IsNullOrWhiteSpace(AssignedEmployeeName))
                locationParts.Add($"NV: {AssignedEmployeeName}");
            if (!string.IsNullOrWhiteSpace(Location))
                locationParts.Add($"Vị trí: {Location}");
            
            if (locationParts.Any())
            {
                html += $"<size=9><color='#757575'>Vị trí:</color></size> <size=10><color='#212121'><b>{string.Join(" - ", locationParts)}</b></color></size><br>";
            }

            // Trạng thái và loại thiết bị
            var statusParts = new List<string>();
            if (!string.IsNullOrWhiteSpace(StatusName))
                statusParts.Add($"Trạng thái: {StatusName}");
            if (!string.IsNullOrWhiteSpace(DeviceTypeName))
                statusParts.Add($"Loại: {DeviceTypeName}");
            
            if (statusParts.Any())
            {
                html += $"<size=9><color='#757575'>Thiết bị:</color></size> <size=10><color='#212121'><b>{string.Join(" | ", statusParts)}</b></color></size><br>";
            }

            // Thông tin bảo hành (nếu có)
            if (WarrantyId.HasValue)
            {
                var warrantyTypeName = WarrantyTypeName ?? string.Empty;
                var warrantyStatusName = WarrantyStatusName ?? string.Empty;
                var warrantyStatusText = WarrantyStatusText ?? string.Empty;

                if (!string.IsNullOrWhiteSpace(warrantyTypeName) || !string.IsNullOrWhiteSpace(warrantyStatusName))
                {
                    var warrantyParts = new List<string>();
                    if (!string.IsNullOrWhiteSpace(warrantyTypeName))
                        warrantyParts.Add($"Kiểu: {warrantyTypeName}");
                    if (!string.IsNullOrWhiteSpace(warrantyStatusName))
                    {
                        var statusColor = GetWarrantyStatusColor(WarrantyStatus);
                        warrantyParts.Add($"<color='{statusColor}'>{warrantyStatusName}</color>");
                    }
                    if (!string.IsNullOrWhiteSpace(warrantyStatusText))
                    {
                        var statusTextColor = IsWarrantyExpired ? "#F44336" : "#4CAF50";
                        warrantyParts.Add($"<color='{statusTextColor}'>{warrantyStatusText}</color>");
                    }

                    if (warrantyParts.Any())
                    {
                        html += $"<size=9><color='#757575'>Bảo hành:</color></size> <size=10><b>{string.Join(" | ", warrantyParts)}</b></size><br>";
                    }

                    // Thời gian bảo hành
                    var timeParts = new List<string>();
                    if (WarrantyFrom.HasValue)
                        timeParts.Add($"Từ: {WarrantyFrom.Value:dd/MM/yyyy}");
                    if (WarrantyUntil.HasValue)
                        timeParts.Add($"Đến: {WarrantyUntil.Value:dd/MM/yyyy}");
                    if (MonthOfWarranty.HasValue && MonthOfWarranty.Value > 0)
                        timeParts.Add($"{MonthOfWarranty.Value} tháng");
                    
                    if (timeParts.Any())
                    {
                        html += $"<size=9><color='#757575'>Thời gian BH:</color></size> <size=10><color='#212121'><b>{string.Join(" - ", timeParts)}</b></color></size>";
                    }
                }
            }

            return html;
        }
    }

    /// <summary>
    /// Lấy tên hiển thị của loại định danh
    /// </summary>
    /// <param name="identifierType">Loại định danh</param>
    /// <returns>Tên hiển thị</returns>
    private string GetIdentifierDisplayName(DeviceIdentifierEnum identifierType)
    {
        var field = identifierType.GetType().GetField(identifierType.ToString());
        var attribute = field?.GetCustomAttributes(typeof(DescriptionAttribute), false)
            .FirstOrDefault() as DescriptionAttribute;
        return attribute?.Description ?? identifierType.ToString();
    }

    /// <summary>
    /// Lấy màu sắc tương ứng với trạng thái bảo hành
    /// </summary>
    /// <param name="status">Trạng thái bảo hành</param>
    /// <returns>Mã màu hex</returns>
    private string GetWarrantyStatusColor(TrangThaiBaoHanhEnum? status)
    {
        if (!status.HasValue)
            return "#212121";

        return status.Value switch
        {
            TrangThaiBaoHanhEnum.ChoXuLy => "#FF9800",      // Orange - Chờ xử lý
            TrangThaiBaoHanhEnum.DangBaoHanh => "#2196F3", // Blue - Đang bảo hành
            TrangThaiBaoHanhEnum.DaHoanThanh => "#4CAF50", // Green - Đã hoàn thành
            TrangThaiBaoHanhEnum.DaTuChoi => "#F44336",     // Red - Đã từ chối
            TrangThaiBaoHanhEnum.DaHuy => "#9E9E9E",        // Grey - Đã hủy
            _ => "#212121"                                   // Default - Black
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
            IPAddress = entity.IPAddress
        };

        // Lấy thông tin ProductVariant nếu có
        if (entity.ProductVariant != null)
        {
            dto.ProductVariantName = entity.ProductVariant.VariantFullName;
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
            Status = 0, // Available
            DeviceType = 0, // Hardware
            IsActive = true,
            CreatedDate = DateTime.Now
        };

        return entity;
    }

    #endregion
}