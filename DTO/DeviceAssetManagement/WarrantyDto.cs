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
    /// Mã sản phẩm/dịch vụ gốc (từ ProductService.Code)
    /// </summary>
    [DisplayName("Mã sản phẩm")]
    [Display(Order = 10)]
    public string ProductCode { get; set; }

    /// <summary>
    /// Tên sản phẩm/dịch vụ gốc (từ ProductService.Name)
    /// </summary>
    [DisplayName("Tên sản phẩm")]
    [Display(Order = 11)]
    public string ProductName { get; set; }

    /// <summary>
    /// Mã biến thể (từ ProductVariant.VariantCode)
    /// </summary>
    [DisplayName("Mã biến thể")]
    [Display(Order = 12)]
    public string VariantCode { get; set; }

    /// <summary>
    /// Mã đơn vị tính (từ UnitOfMeasure.Code)
    /// </summary>
    [DisplayName("Mã đơn vị")]
    [Display(Order = 13)]
    public string UnitCode { get; set; }

    /// <summary>
    /// Tên đơn vị tính (từ UnitOfMeasure.Name)
    /// </summary>
    [DisplayName("Tên đơn vị")]
    [Display(Order = 14)]
    public string UnitName { get; set; }

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
    /// Hiển thị: thông tin định danh thiết bị, loại bảo hành, trạng thái bảo hành, từ ngày/đến ngày/thời gian bảo hành, tình trạng bảo hành
    /// Sử dụng các tag HTML chuẩn của DevExpress: &lt;b&gt;, &lt;i&gt;, &lt;color&gt;, &lt;size&gt;
    /// Tham khảo: https://docs.devexpress.com/WindowsForms/4874/common-features/html-text-formatting
    /// </summary>
    [DisplayName("Thông tin BH HTML")]
    [Display(Order = 15)]
    [Description("Thông tin bảo hành dưới dạng HTML")]
    public string WarrantyInfoHtml
    {
        get
        {
            var warrantyStatusName = WarrantyStatusName ?? string.Empty;
            var warrantyStatusText = WarrantyStatusText ?? string.Empty;

            // Xác định màu sắc cho trạng thái bảo hành
            var statusColor = GetWarrantyStatusColor(WarrantyStatus);
            
            // Xác định màu sắc cho loại bảo hành
            var typeColor = GetWarrantyTypeColor(WarrantyType);
                
            // Xác định màu sắc cho tình trạng (còn/hết hạn)
            var statusTextColor = IsWarrantyExpired ? "red" : "green";

            var html = string.Empty;

            // Thông tin định danh thiết bị (SerialNumber, IMEI, MACAddress, AssetTag, LicenseKey)
            var deviceInfo = DeviceInfo ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(deviceInfo))
            {
                html += $"<size=10><color='#757575'>Thiết bị:</color></size> <size=11><color='blue'><b>{deviceInfo}</b></color></size><br>";
            }

            // Loại bảo hành với màu sắc tương ứng
            var warrantyTypeName = WarrantyTypeName ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(warrantyTypeName))
            {
                html += $"<size=10><color='#757575'>Loại BH:</color></size> <size=11><color='{typeColor}'><b>{warrantyTypeName}</b></color></size><br>";
            }

            // Trạng thái bảo hành với màu sắc tương ứng
            if (!string.IsNullOrWhiteSpace(warrantyStatusName))
            {
                html += $"<size=10><color='#757575'>Trạng thái:</color></size> <size=11><color='{statusColor}'><b>{warrantyStatusName}</b></color></size><br>";
            }

            // Thời gian bảo hành (Từ ngày, đến ngày, số tháng)
            var timeParts = new List<string>();
            if (WarrantyFrom.HasValue)
            {
                timeParts.Add($"Từ: {WarrantyFrom.Value:dd/MM/yyyy}");
            }
            if (WarrantyUntil.HasValue)
            {
                timeParts.Add($"Đến: {WarrantyUntil.Value:dd/MM/yyyy}");
            }
            if (MonthOfWarranty > 0)
            {
                timeParts.Add($"{MonthOfWarranty} tháng");
            }
            if (timeParts.Any())
            {
                html += $"<size=10><color='#757575'>Thời gian:</color></size> <size=11><color='#212121'><b>{string.Join(" - ", timeParts)}</b></color></size><br>";
            }

            // Tình trạng bảo hành (còn/hết hạn)
            if (!string.IsNullOrWhiteSpace(warrantyStatusText))
            {
                html += $"<size=10><color='#757575'>Tình trạng:</color></size> <size=11><color='{statusTextColor}'><b>{warrantyStatusText}</b></color></size>";
            }

            return html;
        }
    }

    /// <summary>
    /// Tổng hợp thông tin bảo hành dưới dạng HTML (chỉ đọc)
    /// Hiển thị đầy đủ thông tin: tên sản phẩm, sản phẩm, kiểu bảo hành, trạng thái, thời gian bảo hành, tình trạng
    /// Sử dụng các tag HTML chuẩn của DevExpress: &lt;b&gt;, &lt;i&gt;, &lt;color&gt;, &lt;size&gt;
    /// Tham khảo: https://docs.devexpress.com/WindowsForms/4874/common-features/html-text-formatting
    /// </summary>
    [DisplayName("Thông tin tổng hợp")]
    [Display(Order = 16)]
    [Description("Tổng hợp đầy đủ thông tin bảo hành dưới dạng HTML")]
    public string FullInfo
    {
        get
        {
            var warrantyStatusName = WarrantyStatusName ?? string.Empty;
            var warrantyStatusText = WarrantyStatusText ?? string.Empty;

            // Xác định màu sắc cho trạng thái bảo hành
            var statusColor = GetWarrantyStatusColor(WarrantyStatus);
            
            // Xác định màu sắc cho loại bảo hành
            var typeColor = GetWarrantyTypeColor(WarrantyType);
                
            // Xác định màu sắc cho tình trạng (còn/hết hạn)
            var statusTextColor = IsWarrantyExpired ? "red" : "green";

            // Format chuyên nghiệp với visual hierarchy rõ ràng
            // - Thông tin sản phẩm: font lớn, bold, màu xanh đậm (primary)
            // - Loại bảo hành: highlight với màu tương ứng (Purple/Cyan)
            // - Trạng thái bảo hành: highlight với màu tương ứng
            // - Thời gian bảo hành: font nhỏ hơn, màu xám cho label, đen cho value
            // - Tình trạng: highlight với màu xanh (còn) hoặc đỏ (hết hạn)

            var html = string.Empty;

            // Tên sản phẩm dịch vụ (nổi bật nhất)
            var productVariantName = ProductVariantName ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(productVariantName))
            {
                html += $"<size=12><b><color='blue'>{productVariantName}</color></b></size>";
                html += "<br>";
            }

            // Kiểu bảo hành với màu sắc tương ứng
            var warrantyTypeName = WarrantyTypeName ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(warrantyTypeName))
            {
                html += $"<size=9><color='#757575'>Kiểu BH:</color></size> <size=10><color='{typeColor}'><b>{warrantyTypeName}</b></color></size><br>";
            }

            // Trạng thái bảo hành
            if (!string.IsNullOrWhiteSpace(warrantyStatusName))
            {
                html += $"<size=9><color='#757575'>Trạng thái:</color></size> <size=10><color='{statusColor}'><b>{warrantyStatusName}</b></color></size><br>";
            }

            // Thời gian bảo hành
            var timeParts = new List<string>();
            if (WarrantyFrom.HasValue)
            {
                timeParts.Add($"Từ: {WarrantyFrom.Value:dd/MM/yyyy}");
            }
            if (WarrantyUntil.HasValue)
            {
                timeParts.Add($"Đến: {WarrantyUntil.Value:dd/MM/yyyy}");
            }
            if (MonthOfWarranty > 0)
            {
                timeParts.Add($"{MonthOfWarranty} tháng");
            }
            if (timeParts.Any())
            {
                html += $"<size=9><color='#757575'>Thời gian:</color></size> <size=10><color='#212121'><b>{string.Join(" - ", timeParts)}</b></color></size><br>";
            }

            // Tình trạng (còn/hết hạn)
            if (!string.IsNullOrWhiteSpace(warrantyStatusText))
            {
                html += $"<size=9><color='#757575'>Tình trạng:</color></size> <size=10><color='{statusTextColor}'><b>{warrantyStatusText}</b></color></size>";
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

    /// <summary>
    /// Tổng hợp thông tin bảo hành dưới dạng HTML (chỉ đọc)
    /// Hiển thị thông tin: tên sản phẩm, thông tin thiết bị, loại bảo hành, trạng thái, thời gian, tình trạng
    /// Sử dụng các tag HTML chuẩn của DevExpress: &lt;color&gt;
    /// Tham khảo: https://docs.devexpress.com/WindowsForms/4874/common-features/html-text-formatting
    /// </summary>
    [DisplayName("Thông tin tổng hợp (HTML)")]
    [Display(Order = 200)]
    [Description("Tổng hợp thông tin bảo hành dưới dạng HTML")]
    public string DeviceHtmlInfo
    {
        get
        {
            var html = string.Empty;

            // Tên sản phẩm dịch vụ
            var productName = ProductName ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(productName))
            {
                html += $"<color='blue'>{productName}</color>";
                html += "<br>";
            }

            // Thông tin thiết bị (DeviceInfo)
            var deviceInfo = DeviceInfo ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(deviceInfo))
            {
                html += $"<color='#757575'>Thiết bị:</color> <b><color='blue'>{deviceInfo}</color></b><br>";
            }

            // Thông tin bảo hành
            var warrantyParts = new List<string>();
            
            // Loại bảo hành
            var warrantyTypeName = WarrantyTypeName ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(warrantyTypeName))
            {
                var typeColor = GetWarrantyTypeColor(WarrantyType);
                warrantyParts.Add($"<color='{typeColor}'><b>Loại: {warrantyTypeName}</b></color>");
            }
            
            // Trạng thái bảo hành
            var warrantyStatusName = WarrantyStatusName ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(warrantyStatusName))
            {
                var statusColor = GetWarrantyStatusColor(WarrantyStatus);
                warrantyParts.Add($"<color='{statusColor}'><b>Trạng thái: {warrantyStatusName}</b></color>");
            }
            
            // Thời gian bảo hành
            var timeParts = new List<string>();
            if (WarrantyFrom.HasValue)
            {
                timeParts.Add($"Từ: {WarrantyFrom.Value:dd/MM/yyyy}");
            }
            if (WarrantyUntil.HasValue)
            {
                timeParts.Add($"Đến: {WarrantyUntil.Value:dd/MM/yyyy}");
            }
            if (MonthOfWarranty > 0)
            {
                timeParts.Add($"{MonthOfWarranty} tháng");
            }
            if (timeParts.Any())
            {
                warrantyParts.Add($"Thời gian: {string.Join(" - ", timeParts)}");
            }
            
            // Tình trạng bảo hành
            var warrantyStatusText = WarrantyStatusText ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(warrantyStatusText))
            {
                var statusTextColor = IsWarrantyExpired ? "red" : "green";
                warrantyParts.Add($"<color='{statusTextColor}'><b>Tình trạng: {warrantyStatusText}</b></color>");
            }
            
            if (warrantyParts.Any())
            {
                html += $"<color='#757575'>Bảo hành:</color> {string.Join(" | ", warrantyParts)}<br>";
            }

            // Ghi chú
            if (!string.IsNullOrWhiteSpace(Notes))
            {
                html += $"<color='#757575'>Ghi chú:</color> <color='#212121'>{Notes}</color><br>";
            }

            return html;
        }
    }

    /// <summary>
    /// Ngày tháng bắt đầu bảo hành của NCC -> VNS
    /// Chỉ trả về giá trị khi WarrantyType == NCCToVNS
    /// Khi set, sẽ cập nhật WarrantyFrom và đặt WarrantyType = NCCToVNS
    /// </summary>
    [DisplayName("Ngày bắt đầu BH NCC -> VNS")]
    [Display(Order = 201)]
    [Description("Ngày tháng bắt đầu bảo hành của NCC cho VNS")]
    public DateTime? WarrantyNccToVnsFrom
    {
        get
        {
            return WarrantyType == LoaiBaoHanhEnum.NCCToVNS ? WarrantyFrom : null;
        }
        set
        {
            if (value.HasValue)
            {
                WarrantyType = LoaiBaoHanhEnum.NCCToVNS;
                WarrantyFrom = value;
            }
            else if (WarrantyType == LoaiBaoHanhEnum.NCCToVNS)
            {
                WarrantyFrom = null;
            }
        }
    }

    /// <summary>
    /// Thời gian bảo hành (Tháng) của NCC -> VNS
    /// Chỉ trả về giá trị khi WarrantyType == NCCToVNS
    /// Khi set, sẽ cập nhật MonthOfWarranty và đặt WarrantyType = NCCToVNS
    /// </summary>
    [DisplayName("Thời gian BH (Tháng) NCC -> VNS")]
    [Display(Order = 202)]
    [Description("Thời gian bảo hành (số tháng) của NCC cho VNS")]
    public int? WarrantyNccToVnsMonthOfWarranty
    {
        get
        {
            return WarrantyType == LoaiBaoHanhEnum.NCCToVNS ? MonthOfWarranty : (int?)null;
        }
        set
        {
            if (value.HasValue)
            {
                WarrantyType = LoaiBaoHanhEnum.NCCToVNS;
                MonthOfWarranty = value.Value;
            }
            else if (WarrantyType == LoaiBaoHanhEnum.NCCToVNS)
            {
                MonthOfWarranty = 0;
            }
        }
    }

    /// <summary>
    /// Ngày tháng hết hạn bảo hành của NCC -> VNS
    /// Chỉ trả về giá trị khi WarrantyType == NCCToVNS
    /// Khi set, sẽ cập nhật WarrantyUntil và đặt WarrantyType = NCCToVNS
    /// </summary>
    [DisplayName("Ngày hết hạn BH NCC -> VNS")]
    [Display(Order = 203)]
    [Description("Ngày tháng hết hạn bảo hành của NCC cho VNS")]
    public DateTime? WarrantyNccToVnsUntil
    {
        get
        {
            return WarrantyType == LoaiBaoHanhEnum.NCCToVNS ? WarrantyUntil : null;
        }
        set
        {
            if (value.HasValue)
            {
                WarrantyType = LoaiBaoHanhEnum.NCCToVNS;
                WarrantyUntil = value;
            }
            else if (WarrantyType == LoaiBaoHanhEnum.NCCToVNS)
            {
                WarrantyUntil = null;
            }
        }
    }

    /// <summary>
    /// Thông tin bảo hành NCC -> VNS dưới dạng HTML (chỉ đọc)
    /// Chỉ hiển thị khi WarrantyType == NCCToVNS
    /// Hiển thị: tên sản phẩm, thông tin thiết bị, trạng thái, thời gian, tình trạng
    /// Sử dụng các tag HTML chuẩn của DevExpress: &lt;color&gt;, &lt;b&gt;
    /// Tham khảo: https://docs.devexpress.com/WindowsForms/4874/common-features/html-text-formatting
    /// </summary>
    [DisplayName("Bảo hành NCC -> VNS")]
    [Display(Order = 204)]
    [Description("Thông tin bảo hành của NCC cho VNS dưới dạng HTML")]
    public string WarrantyNccToVnsHtml
    {
        get
        {
            // Chỉ hiển thị nếu WarrantyType là NCCToVNS
            if (WarrantyType != LoaiBaoHanhEnum.NCCToVNS)
            {
                return string.Empty;
            }

            return BuildWarrantyHtmlInfo("purple");
        }
    }

    /// <summary>
    /// Ngày tháng bắt đầu bảo hành của VNS -> Khách hàng
    /// Chỉ trả về giá trị khi WarrantyType == VNSToKhachHang
    /// Khi set, sẽ cập nhật WarrantyFrom và đặt WarrantyType = VNSToKhachHang
    /// </summary>
    [DisplayName("Ngày bắt đầu BH VNS -> Khách hàng")]
    [Display(Order = 205)]
    [Description("Ngày tháng bắt đầu bảo hành của VNS cho khách hàng")]
    public DateTime? WarrantyVnsToKhachHangFrom
    {
        get
        {
            return WarrantyType == LoaiBaoHanhEnum.VNSToKhachHang ? WarrantyFrom : null;
        }
        set
        {
            if (value.HasValue)
            {
                WarrantyType = LoaiBaoHanhEnum.VNSToKhachHang;
                WarrantyFrom = value;
            }
            else if (WarrantyType == LoaiBaoHanhEnum.VNSToKhachHang)
            {
                WarrantyFrom = null;
            }
        }
    }

    /// <summary>
    /// Thời gian bảo hành (Tháng) của VNS -> Khách hàng
    /// Chỉ trả về giá trị khi WarrantyType == VNSToKhachHang
    /// Khi set, sẽ cập nhật MonthOfWarranty và đặt WarrantyType = VNSToKhachHang
    /// </summary>
    [DisplayName("Thời gian BH (Tháng) VNS -> Khách hàng")]
    [Display(Order = 206)]
    [Description("Thời gian bảo hành (số tháng) của VNS cho khách hàng")]
    public int? WarrantyVnsToKhachHangMonthOfWarranty
    {
        get
        {
            return WarrantyType == LoaiBaoHanhEnum.VNSToKhachHang ? MonthOfWarranty : (int?)null;
        }
        set
        {
            if (value.HasValue)
            {
                WarrantyType = LoaiBaoHanhEnum.VNSToKhachHang;
                MonthOfWarranty = value.Value;
            }
            else if (WarrantyType == LoaiBaoHanhEnum.VNSToKhachHang)
            {
                MonthOfWarranty = 0;
            }
        }
    }

    /// <summary>
    /// Ngày tháng hết hạn bảo hành của VNS -> Khách hàng
    /// Chỉ trả về giá trị khi WarrantyType == VNSToKhachHang
    /// Khi set, sẽ cập nhật WarrantyUntil và đặt WarrantyType = VNSToKhachHang
    /// </summary>
    [DisplayName("Ngày hết hạn BH VNS -> Khách hàng")]
    [Display(Order = 207)]
    [Description("Ngày tháng hết hạn bảo hành của VNS cho khách hàng")]
    public DateTime? WarrantyVnsToKhachHangUntil
    {
        get
        {
            return WarrantyType == LoaiBaoHanhEnum.VNSToKhachHang ? WarrantyUntil : null;
        }
        set
        {
            if (value.HasValue)
            {
                WarrantyType = LoaiBaoHanhEnum.VNSToKhachHang;
                WarrantyUntil = value;
            }
            else if (WarrantyType == LoaiBaoHanhEnum.VNSToKhachHang)
            {
                WarrantyUntil = null;
            }
        }
    }

    /// <summary>
    /// Thông tin bảo hành VNS -> Khách hàng dưới dạng HTML (chỉ đọc)
    /// Chỉ hiển thị khi WarrantyType == VNSToKhachHang
    /// Hiển thị: tên sản phẩm, thông tin thiết bị, trạng thái, thời gian, tình trạng
    /// Sử dụng các tag HTML chuẩn của DevExpress: &lt;color&gt;, &lt;b&gt;
    /// Tham khảo: https://docs.devexpress.com/WindowsForms/4874/common-features/html-text-formatting
    /// </summary>
    [DisplayName("Bảo hành VNS -> Khách hàng")]
    [Display(Order = 208)]
    [Description("Thông tin bảo hành của VNS cho khách hàng dưới dạng HTML")]
    public string WarrantyVnsToKhachHangHtml
    {
        get
        {
            // Chỉ hiển thị nếu WarrantyType là VNSToKhachHang
            if (WarrantyType != LoaiBaoHanhEnum.VNSToKhachHang)
            {
                return string.Empty;
            }

            return BuildWarrantyHtmlInfo("cyan");
        }
    }

    /// <summary>
    /// Helper method để xây dựng HTML thông tin bảo hành
    /// </summary>
    /// <param name="typeColor">Màu sắc cho loại bảo hành</param>
    /// <returns>Chuỗi HTML</returns>
    private string BuildWarrantyHtmlInfo(string typeColor)
    {
        var html = string.Empty;

        //// Tên sản phẩm dịch vụ
        //var productName = ProductName ?? string.Empty;
        //if (!string.IsNullOrWhiteSpace(productName))
        //{
        //    html += $"<color='blue'><b>{productName}</b></color>";
        //    html += "<br>";
        //}

        //// Thông tin thiết bị (DeviceInfo)
        //var deviceInfo = DeviceInfo ?? string.Empty;
        //if (!string.IsNullOrWhiteSpace(deviceInfo))
        //{
        //    html += $"<color='#757575'>Thiết bị:</color> <b><color='blue'>{deviceInfo}</color></b><br>";
        //}

        // Loại bảo hành với màu sắc tương ứng
        var warrantyTypeName = WarrantyTypeName ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(warrantyTypeName))
        {
            html += $"<color='#757575'>Loại BH:</color> <color='{typeColor}'><b>{warrantyTypeName}</b></color><br>";
        }

        // Trạng thái bảo hành
        var warrantyStatusName = WarrantyStatusName ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(warrantyStatusName))
        {
            var statusColor = GetWarrantyStatusColor(WarrantyStatus);
            html += $"<color='#757575'>Trạng thái:</color> <color='{statusColor}'><b>{warrantyStatusName}</b></color><br>";
        }

        // Thời gian bảo hành
        var timeParts = new List<string>();
        if (WarrantyFrom.HasValue)
        {
            timeParts.Add($"Từ: {WarrantyFrom.Value:dd/MM/yyyy}");
        }
        if (WarrantyUntil.HasValue)
        {
            timeParts.Add($"Đến: {WarrantyUntil.Value:dd/MM/yyyy}");
        }
        if (MonthOfWarranty > 0)
        {
            timeParts.Add($"{MonthOfWarranty} tháng");
        }
        if (timeParts.Any())
        {
            html += $"<color='#757575'>Thời gian:</color> <b>{string.Join(" - ", timeParts)}</b><br>";
        }

        // Tình trạng bảo hành
        var warrantyStatusText = WarrantyStatusText ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(warrantyStatusText))
        {
            var statusTextColor = IsWarrantyExpired ? "red" : "green";
            html += $"<color='#757575'>Tình trạng:</color> <color='{statusTextColor}'><b>{warrantyStatusText}</b></color><br>";
        }

        // Ghi chú
        if (!string.IsNullOrWhiteSpace(Notes))
        {
            html += $"<color='#757575'>Ghi chú:</color> <color='#212121'>{Notes}</color>";
        }

        return html;
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

        // Lấy thông tin sản phẩm từ ProductVariant thông qua Device (theo pattern ProductVariantDto)
        if (entity.Device != null && entity.Device.ProductVariant != null)
        {
            var productVariant = entity.Device.ProductVariant;
            
            // Lấy mã biến thể (không cần navigation property)
            dto.VariantCode = productVariant.VariantCode;
            
            // Lấy thông tin ProductService - sử dụng try-catch để tránh lazy loading error
            try
            {
                if (productVariant.ProductService != null)
                {
                    dto.ProductCode = productVariant.ProductService.Code;
                    dto.ProductName = productVariant.ProductService.Name;
                }
            }
            catch
            {
                // Navigation property chưa được load, bỏ qua
            }
            
            // Lấy thông tin đơn vị tính - sử dụng try-catch để tránh lazy loading error
            try
            {
                if (productVariant.UnitOfMeasure != null)
                {
                    dto.UnitCode = productVariant.UnitOfMeasure.Code;
                    dto.UnitName = productVariant.UnitOfMeasure.Name;
                }
            }
            catch
            {
                // Navigation property chưa được load, bỏ qua
            }
            
            // Giữ lại ProductVariantName để tương thích (ưu tiên VariantFullName, nếu không có thì lấy từ ProductService.Name)
            if (!string.IsNullOrWhiteSpace(productVariant.VariantFullName))
            {
                dto.ProductVariantName = productVariant.VariantFullName;
            }
            else if (!string.IsNullOrWhiteSpace(dto.ProductName))
            {
                dto.ProductVariantName = dto.ProductName;
            }
            
            // Cập nhật DeviceInfo: thêm tên sản phẩm vào đầu (chỉ tên, không có mã, số lượng, đơn vị tính)
            if (!string.IsNullOrWhiteSpace(dto.ProductName))
            {
                var deviceInfoParts = new List<string> { dto.ProductName };
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

        // Lấy thông tin sản phẩm từ ProductVariant (theo pattern ProductVariantDto)
        if (device.ProductVariant != null)
        {
            var productVariant = device.ProductVariant;
            
            // Lấy mã biến thể (không cần navigation property)
            warrantyDto.VariantCode = productVariant.VariantCode;
            
            // Lấy thông tin ProductService - sử dụng try-catch để tránh lazy loading error
            try
            {
                if (productVariant.ProductService != null)
                {
                    warrantyDto.ProductCode = productVariant.ProductService.Code;
                    warrantyDto.ProductName = productVariant.ProductService.Name;
                }
            }
            catch
            {
                // Navigation property chưa được load, bỏ qua
            }
            
            // Lấy thông tin đơn vị tính - sử dụng try-catch để tránh lazy loading error
            try
            {
                if (productVariant.UnitOfMeasure != null)
                {
                    warrantyDto.UnitCode = productVariant.UnitOfMeasure.Code;
                    warrantyDto.UnitName = productVariant.UnitOfMeasure.Name;
                }
            }
            catch
            {
                // Navigation property chưa được load, bỏ qua
            }
            
            // Giữ lại ProductVariantName để tương thích
            if (!string.IsNullOrWhiteSpace(productVariant.VariantFullName))
            {
                warrantyDto.ProductVariantName = productVariant.VariantFullName;
            }
            else if (!string.IsNullOrWhiteSpace(warrantyDto.ProductName))
            {
                warrantyDto.ProductVariantName = warrantyDto.ProductName;
            }
        }

        // Lấy thông tin Device (SerialNumber, IMEI, MACAddress, AssetTag, LicenseKey)
        var deviceInfoParts = new List<string>();
        
        // Thêm tên sản phẩm vào đầu danh sách (chỉ tên, không có mã, số lượng, đơn vị tính)
        if (!string.IsNullOrWhiteSpace(warrantyDto.ProductName))
        {
            deviceInfoParts.Add(warrantyDto.ProductName);
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