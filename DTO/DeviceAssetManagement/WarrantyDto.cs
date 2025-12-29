using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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
    private bool IsWarrantyExpired
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
    private string WarrantyStatusText
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
            LoaiBaoHanhEnum.VNSToKhachHang => "blue",      // Cyan - Bảo hành từ VNS -> Khách hàng
            _ => "black"                                     // Default - Black
        };
    }

    #endregion
}

/// <summary>
/// Enum định nghĩa các kiểu bảo hành
/// </summary>
public enum LoaiBaoHanhEnum
{
    /// <summary>
    /// Bảo hành từ Nhà cung cấp -> VNS
    /// </summary>
    [Description("NCC -> VNS")]
    NCCToVNS = 0,

    /// <summary>
    /// Bảo hành từ VNS -> Khách hàng
    /// </summary>
    [Description("VNS -> Khách hàng")]
    VNSToKhachHang = 1
}