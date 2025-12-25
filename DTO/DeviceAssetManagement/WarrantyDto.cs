using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Common.Enums;
using Dal.DataContext;

namespace DTO.DeviceAssetManagement;

/// <summary>
/// Data Transfer Object cho thông tin bảo hành
/// </summary>
public class WarrantyDto
{
    #region Properties - Thông tin cơ bản

    /// <summary>
    /// ID duy nhất của bảo hành
    /// </summary>
    [DisplayName("ID")]
    [Display(Order = -1)]
    public Guid Id { get; set; }

    /// <summary>
    /// ID của Device (thiết bị) liên kết với bảo hành
    /// </summary>
    [DisplayName("ID Thiết bị")]
    [Display(Order = 1)]
    public Guid? DeviceId { get; set; }

    /// <summary>
    /// Kiểu bảo hành
    /// </summary>
    [DisplayName("Kiểu BH")]
    [Display(Order = 2)]
    [Required(ErrorMessage = "Kiểu bảo hành không được để trống")]
    public LoaiBaoHanhEnum WarrantyType { get; set; }

    /// <summary>
    /// Ngày bắt đầu bảo hành
    /// </summary>
    [DisplayName("Ngày bắt đầu BH")]
    [Display(Order = 3)]
    public DateTime? WarrantyFrom { get; set; }

    /// <summary>
    /// Số tháng bảo hành
    /// </summary>
    [DisplayName("Số tháng BH")]
    [Display(Order = 4)]
    [Required(ErrorMessage = "Số tháng bảo hành không được để trống")]
    [Range(0, int.MaxValue, ErrorMessage = "Số tháng bảo hành phải lớn hơn hoặc bằng 0")]
    public int MonthOfWarranty { get; set; }

    /// <summary>
    /// Ngày kết thúc bảo hành
    /// </summary>
    [DisplayName("Ngày kết thúc BH")]
    [Display(Order = 5)]
    public DateTime? WarrantyUntil { get; set; }

    /// <summary>
    /// Trạng thái bảo hành
    /// </summary>
    [DisplayName("Trạng thái BH")]
    [Display(Order = 6)]
    [Required(ErrorMessage = "Trạng thái bảo hành không được để trống")]
    public TrangThaiBaoHanhEnum WarrantyStatus { get; set; }

    /// <summary>
    /// Ghi chú
    /// </summary>
    [DisplayName("Ghi chú")]
    [Display(Order = 7)]
    [StringLength(1000, ErrorMessage = "Ghi chú không được vượt quá 1000 ký tự")]
    public string Notes { get; set; }

    /// <summary>
    /// Trạng thái hoạt động
    /// </summary>
    [DisplayName("Hoạt động")]
    [Display(Order = 8)]
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Ngày tạo
    /// </summary>
    [DisplayName("Ngày tạo")]
    [Display(Order = 9)]
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Ngày cập nhật
    /// </summary>
    [DisplayName("Ngày cập nhật")]
    [Display(Order = 10)]
    public DateTime? UpdatedDate { get; set; }

    /// <summary>
    /// ID người tạo
    /// </summary>
    [DisplayName("Người tạo")]
    [Display(Order = 11)]
    public Guid? CreatedBy { get; set; }

    /// <summary>
    /// ID người cập nhật
    /// </summary>
    [DisplayName("Người cập nhật")]
    [Display(Order = 12)]
    public Guid? UpdatedBy { get; set; }

    #endregion

    #region Properties - Thông tin hiển thị (Display)

    /// <summary>
    /// Tên sản phẩm dịch vụ (lấy từ ProductVariant) - giữ lại để tương thích
    /// </summary>
    [DisplayName("Tên sản phẩm biến thể")]
    [Display(Order = 15)]
    public string ProductVariantName { get; set; }

    /// <summary>
    /// Thông tin Device (SerialNumber, IMEI, MACAddress, AssetTag, LicenseKey) - chỉ đọc
    /// </summary>
    [DisplayName("Thông tin thiết bị")]
    [Display(Order = 16)]
    public string DeviceInfo { get; set; }

    /// <summary>
    /// Tên kiểu bảo hành (hiển thị)
    /// </summary>
    [DisplayName("Kiểu BH")]
    [Display(Order = 17)]
    public string WarrantyTypeName { get; set; }

    /// <summary>
    /// Tên trạng thái bảo hành (hiển thị)
    /// </summary>
    [DisplayName("Trạng thái")]
    [Display(Order = 18)]
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
    /// Thông tin bảo hành dưới dạng HTML (chỉ đọc)
    /// Hiển thị: kiểu bảo hành, từ ngày/thời gian bảo hành/đến ngày, tình trạng còn bảo hành hay không
    /// Sử dụng các tag HTML chuẩn của DevExpress: &lt;b&gt;, &lt;color&gt;, &lt;size&gt;
    /// Tham khảo: https://docs.devexpress.com/WindowsForms/4874/common-features/html-text-formatting
    /// </summary>
    [DisplayName("Thông tin BH HTML")]
    [Display(Order = 15)]
    [Description("Thông tin bảo hành dưới dạng HTML")]
    public string WarrantyInfoHtml
    {
        get
        {
            var warrantyStatusText = WarrantyStatusText ?? string.Empty;

            // Xác định màu sắc cho loại bảo hành
            var typeColor = GetWarrantyTypeColor(WarrantyType);
                
            // Xác định màu sắc cho tình trạng (còn/hết hạn)
            var statusTextColor = IsWarrantyExpired ? "red" : "green";

            var html = string.Empty;

            // Kiểu bảo hành với màu sắc tương ứng
            var warrantyTypeName = WarrantyTypeName ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(warrantyTypeName))
            {
                html += $"<size=11><color='{typeColor}'><b>{warrantyTypeName}</b></color></size><br>";
            }

            // Thời gian bảo hành (Từ ngày, thời gian bảo hành, đến ngày)
            var timeParts = new List<string>();
            if (WarrantyFrom.HasValue)
            {
                timeParts.Add($"Từ: {WarrantyFrom.Value:dd/MM/yyyy}");
            }
            if (MonthOfWarranty > 0)
            {
                timeParts.Add($"{MonthOfWarranty} tháng");
            }
            if (WarrantyUntil.HasValue)
            {
                timeParts.Add($"Đến: {WarrantyUntil.Value:dd/MM/yyyy}");
            }
            if (timeParts.Any())
            {
                html += $"<size=10><color='#212121'><b>{string.Join(" - ", timeParts)}</b></color></size><br>";
            }

            // Tình trạng bảo hành (còn/hết hạn)
            if (!string.IsNullOrWhiteSpace(warrantyStatusText))
            {
                html += $"<size=10><color='{statusTextColor}'><b>{warrantyStatusText}</b></color></size>";
            }

            return html;
        }
    }

    /// <summary>
    /// Lấy màu sắc tương ứng với trạng thái bảo hành
    /// </summary>
    /// <param name="status">Trạng thái bảo hành</param>
    /// <returns>Tên màu (theo chuẩn HTML/CSS)</returns>
    private string GetWarrantyStatusColor(TrangThaiBaoHanhEnum status)
    {
        return status switch
        {
            TrangThaiBaoHanhEnum.ChoXuLy => "orange",      // Orange - Chờ xử lý
            TrangThaiBaoHanhEnum.DangBaoHanh => "blue",    // Blue - Đang bảo hành
            TrangThaiBaoHanhEnum.DaHoanThanh => "green",   // Green - Đã hoàn thành
            TrangThaiBaoHanhEnum.DaTuChoi => "red",         // Red - Đã từ chối
            TrangThaiBaoHanhEnum.DaHuy => "gray",           // Gray - Đã hủy
            _ => "black"                                     // Default - Black
        };
    }

    /// <summary>
    /// Lấy màu sắc tương ứng với loại bảo hành
    /// </summary>
    /// <param name="warrantyType">Loại bảo hành</param>
    /// <returns>Tên màu (theo chuẩn HTML/CSS)</returns>
    public static string GetWarrantyTypeColor(LoaiBaoHanhEnum warrantyType)
    {
        return warrantyType switch
        {
            LoaiBaoHanhEnum.NCCToVNS => "purple",           // Purple - Bảo hành từ NCC -> VNS
            LoaiBaoHanhEnum.VNSToKhachHang => "cyan",      // Cyan - Bảo hành từ VNS -> Khách hàng
            _ => "black"                                     // Default - Black
        };
    }

    #endregion
}

/// <summary>
/// Converter giữa Warranty entity và WarrantyDto
/// </summary>
public static class WarrantyDtoConverter
{
    #region Entity to DTO

    /// <summary>
    /// Chuyển đổi Warranty entity thành WarrantyDto
    /// </summary>
    /// <param name="entity">Warranty entity</param>
    /// <returns>WarrantyDto</returns>
    public static WarrantyDto ToDto(this Warranty entity)
    {
        if (entity == null) return null;

        var dto = new WarrantyDto
        {
            Id = entity.Id,
            DeviceId = entity.DeviceId,
            WarrantyFrom = entity.WarrantyFrom,
            MonthOfWarranty = entity.MonthOfWarranty,
            WarrantyUntil = entity.WarrantyUntil,
            Notes = entity.Notes,
            IsActive = entity.IsActive,
            CreatedDate = entity.CreatedDate,
            UpdatedDate = entity.UpdatedDate,
            CreatedBy = entity.CreatedBy,
            UpdatedBy = entity.UpdatedBy
        };

        // Lấy thông tin Device (SerialNumber, IMEI, MACAddress, AssetTag, LicenseKey)
        if (entity.Device != null)
        {
            var deviceInfoParts = new List<string>();
            
            // Thông tin sản phẩm sẽ được set sau khi lấy từ ProductVariant
            
            if (!string.IsNullOrWhiteSpace(entity.Device.SerialNumber))
                deviceInfoParts.Add($"S/N: {entity.Device.SerialNumber}");
            if (!string.IsNullOrWhiteSpace(entity.Device.IMEI))
                deviceInfoParts.Add($"IMEI: {entity.Device.IMEI}");
            if (!string.IsNullOrWhiteSpace(entity.Device.MACAddress))
                deviceInfoParts.Add($"MAC: {entity.Device.MACAddress}");
            if (!string.IsNullOrWhiteSpace(entity.Device.AssetTag))
                deviceInfoParts.Add($"Asset: {entity.Device.AssetTag}");
            if (!string.IsNullOrWhiteSpace(entity.Device.LicenseKey))
                deviceInfoParts.Add($"License: {entity.Device.LicenseKey}");
            
            dto.DeviceInfo = string.Join(" | ", deviceInfoParts);
        }

        // Lấy thông tin sản phẩm từ ProductVariant thông qua Device
        if (entity.Device != null && entity.Device.ProductVariant != null)
        {
            var productVariant = entity.Device.ProductVariant;
            
            // Giữ lại ProductVariantName để tương thích (ưu tiên VariantFullName, nếu không có thì lấy từ ProductService.Name)
            if (!string.IsNullOrWhiteSpace(productVariant.VariantFullName))
            {
                dto.ProductVariantName = productVariant.VariantFullName;
            }
            else
            {
                // Lấy tên từ ProductService nếu có
                try
                {
                    if (productVariant.ProductService != null)
                    {
                        dto.ProductVariantName = productVariant.ProductService.Name;
                    }
                }
                catch
                {
                    // Navigation property chưa được load, bỏ qua
                }
            }
            
            // Cập nhật DeviceInfo: thêm tên sản phẩm vào đầu (chỉ tên, không có mã, số lượng, đơn vị tính)
            if (!string.IsNullOrWhiteSpace(dto.ProductVariantName))
            {
                var deviceInfoParts = new List<string> { dto.ProductVariantName };
                if (!string.IsNullOrWhiteSpace(dto.DeviceInfo))
                {
                    deviceInfoParts.AddRange(dto.DeviceInfo.Split(new[] { " | " }, StringSplitOptions.None));
                }
                dto.DeviceInfo = string.Join(" | ", deviceInfoParts);
            }
        }

        // Chuyển đổi WarrantyType từ int sang enum
        if (Enum.IsDefined(typeof(LoaiBaoHanhEnum), entity.WarrantyType))
        {
            dto.WarrantyType = (LoaiBaoHanhEnum)entity.WarrantyType;
        }
        else
        {
            // Nếu giá trị không hợp lệ, mặc định là NCCToVNS
            dto.WarrantyType = LoaiBaoHanhEnum.NCCToVNS;
        }

        // Lấy tên kiểu bảo hành từ Description attribute
        dto.WarrantyTypeName = GetEnumDescription(dto.WarrantyType);

        // Chuyển đổi WarrantyStatus từ int sang enum
        if (Enum.IsDefined(typeof(TrangThaiBaoHanhEnum), entity.WarrantyStatus))
        {
            dto.WarrantyStatus = (TrangThaiBaoHanhEnum)entity.WarrantyStatus;
        }
        else
        {
            // Nếu giá trị không hợp lệ, mặc định là ChoXuLy
            dto.WarrantyStatus = TrangThaiBaoHanhEnum.ChoXuLy;
        }

        // Lấy tên trạng thái từ Description attribute
        dto.WarrantyStatusName = GetEnumDescription(dto.WarrantyStatus);

        return dto;
    }

    /// <summary>
    /// Chuyển đổi danh sách Warranty entities thành danh sách WarrantyDto
    /// </summary>
    /// <param name="entities">Danh sách Warranty entities</param>
    /// <returns>Danh sách WarrantyDto</returns>
    public static List<WarrantyDto> ToDtoList(this IEnumerable<Warranty> entities)
    {
        if (entities == null) return new List<WarrantyDto>();

        return entities.Select(entity => entity.ToDto()).ToList();
    }

    /// <summary>
    /// Tạo danh sách WarrantyDto từ danh sách Device
    /// Với mỗi Device, tìm Warranty tương ứng nếu có thì convert, nếu không thì tạo WarrantyDto mặc định
    /// </summary>
    /// <param name="devices">Danh sách Device entities</param>
    /// <param name="warrantyDict">Dictionary chứa Warranty theo DeviceId để lookup nhanh (optional, nếu null thì tất cả Device sẽ tạo WarrantyDto mặc định)</param>
    /// <returns>Danh sách WarrantyDto</returns>
    public static List<WarrantyDto> ToWarrantyDtoListFromDevices(
        this IEnumerable<Dal.DataContext.Device> devices,
        Dictionary<Guid, Warranty> warrantyDict = null)
    {
        if (devices == null) return new List<WarrantyDto>();

        var warrantyDtos = new List<WarrantyDto>();

        foreach (var device in devices)
        {
            if (device == null) continue;

            WarrantyDto warrantyDto;

            // Tìm Warranty cho Device này
            Warranty warranty = null;
            if (warrantyDict != null && device.Id != Guid.Empty)
            {
                warrantyDict.TryGetValue(device.Id, out warranty);
            }

            if (warranty != null)
            {
                // Nếu có Warranty, convert sang DTO
                warrantyDto = warranty.ToDto();
            }
            else
            {
                // Nếu không có Warranty, tạo WarrantyDto với thông tin "chưa có thông tin bảo hành"
                warrantyDto = device.ToWarrantyDtoForDeviceWithoutWarranty();
            }

            if (warrantyDto != null)
            {
                warrantyDtos.Add(warrantyDto);
            }
        }

        return warrantyDtos;
    }

    /// <summary>
    /// Tạo WarrantyDto cho Device không có Warranty
    /// Hiển thị thông tin "chưa có thông tin bảo hành"
    /// </summary>
    /// <param name="device">Device entity</param>
    /// <returns>WarrantyDto với thông tin "chưa có thông tin bảo hành"</returns>
    public static WarrantyDto ToWarrantyDtoForDeviceWithoutWarranty(this Dal.DataContext.Device device)
    {
        if (device == null) return null;

        var warrantyDto = new WarrantyDto
        {
            Id = Guid.Empty, // Không có ID vì không có Warranty
            DeviceId = device.Id,
            WarrantyType = LoaiBaoHanhEnum.NCCToVNS, // Giá trị mặc định
            WarrantyFrom = null,
            WarrantyUntil = null,
            MonthOfWarranty = 0,
            WarrantyStatus = TrangThaiBaoHanhEnum.ChoXuLy, // Giá trị mặc định
            Notes = "Chưa có thông tin bảo hành",
            IsActive = false,
            CreatedDate = DateTime.Now,
            UpdatedDate = null,
            CreatedBy = null,
            UpdatedBy = null
        };

        // Lấy thông tin sản phẩm từ ProductVariant
        if (device.ProductVariant != null)
        {
            var productVariant = device.ProductVariant;
            
            // Giữ lại ProductVariantName để tương thích
            if (!string.IsNullOrWhiteSpace(productVariant.VariantFullName))
            {
                warrantyDto.ProductVariantName = productVariant.VariantFullName;
            }
            else
            {
                // Lấy tên từ ProductService nếu có
                try
                {
                    if (productVariant.ProductService != null)
                    {
                        warrantyDto.ProductVariantName = productVariant.ProductService.Name;
                    }
                }
                catch
                {
                    // Navigation property chưa được load, bỏ qua
                }
            }
        }

        // Lấy thông tin Device (SerialNumber, IMEI, MACAddress, AssetTag, LicenseKey)
        var deviceInfoParts = new List<string>();
        
        // Thêm tên sản phẩm vào đầu danh sách (chỉ tên, không có mã, số lượng, đơn vị tính)
        if (!string.IsNullOrWhiteSpace(warrantyDto.ProductVariantName))
        {
            deviceInfoParts.Add(warrantyDto.ProductVariantName);
        }
        
        if (!string.IsNullOrWhiteSpace(device.SerialNumber))
            deviceInfoParts.Add($"S/N: {device.SerialNumber}");
        if (!string.IsNullOrWhiteSpace(device.IMEI))
            deviceInfoParts.Add($"IMEI: {device.IMEI}");
        if (!string.IsNullOrWhiteSpace(device.MACAddress))
            deviceInfoParts.Add($"MAC: {device.MACAddress}");
        if (!string.IsNullOrWhiteSpace(device.AssetTag))
            deviceInfoParts.Add($"Asset: {device.AssetTag}");
        if (!string.IsNullOrWhiteSpace(device.LicenseKey))
            deviceInfoParts.Add($"License: {device.LicenseKey}");

        warrantyDto.DeviceInfo = deviceInfoParts.Any() 
            ? string.Join(" | ", deviceInfoParts) 
            : "Thiết bị không có thông tin định danh";

        // Set các giá trị hiển thị
        warrantyDto.WarrantyTypeName = "Chưa có";
        warrantyDto.WarrantyStatusName = "Chưa có thông tin bảo hành";
        // WarrantyStatusText là read-only property, sẽ tự động trả về "Chưa xác định" vì WarrantyUntil = null

        return warrantyDto;
    }

    #endregion

    #region DTO to Entity

    /// <summary>
    /// Chuyển đổi WarrantyDto thành Warranty entity
    /// </summary>
    /// <param name="dto">WarrantyDto</param>
    /// <returns>Warranty entity</returns>
    public static Warranty ToEntity(this WarrantyDto dto)
    {
        if (dto == null) return null;

        return new Warranty
        {
            Id = dto.Id,
            DeviceId = dto.DeviceId,
            WarrantyType = (int)dto.WarrantyType, // Chuyển đổi enum sang int
            WarrantyFrom = dto.WarrantyFrom,
            MonthOfWarranty = dto.MonthOfWarranty,
            WarrantyUntil = dto.WarrantyUntil,
            WarrantyStatus = (int)dto.WarrantyStatus, // Chuyển đổi enum sang int
            Notes = dto.Notes,
            IsActive = dto.IsActive,
            CreatedDate = dto.CreatedDate,
            UpdatedDate = dto.UpdatedDate,
            CreatedBy = dto.CreatedBy,
            UpdatedBy = dto.UpdatedBy
        };
    }

    /// <summary>
    /// Chuyển đổi danh sách WarrantyDto thành danh sách Warranty entities
    /// </summary>
    /// <param name="dtos">Danh sách WarrantyDto</param>
    /// <returns>Danh sách Warranty entities</returns>
    public static List<Warranty> ToEntityList(this IEnumerable<WarrantyDto> dtos)
    {
        if (dtos == null) return new List<Warranty>();

        return dtos.Select(dto => dto.ToEntity()).ToList();
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Lấy Description từ enum value (generic method)
    /// </summary>
    /// <typeparam name="T">Kiểu enum</typeparam>
    /// <param name="enumValue">Giá trị enum</param>
    /// <returns>Description hoặc tên enum nếu không có Description</returns>
    private static string GetEnumDescription<T>(T enumValue) where T : Enum
    {
        var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
        if (fieldInfo == null) return enumValue.ToString();

        var descriptionAttribute = fieldInfo.GetCustomAttribute<DescriptionAttribute>();
        return descriptionAttribute?.Description ?? enumValue.ToString();
    }

    #endregion
}