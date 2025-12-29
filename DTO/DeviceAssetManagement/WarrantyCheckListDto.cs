using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace DTO.DeviceAssetManagement;

/// <summary>
/// Data Transfer Object cho thông tin bảo hành
/// </summary>
public class WarrantyCheckListDto
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
    /// Thông tin Device (SerialNumber, IMEI, MACAddress, AssetTag, LicenseKey) - chỉ đọc
    /// </summary>
    [DisplayName("Thông tin thiết bị")]
    [Display(Order = 7)]
    public string DeviceInfo { get; set; }

    /// <summary>
    /// Ghi chú
    /// </summary>
    [DisplayName("Ghi chú")]
    [Display(Order = 8)]
    [StringLength(1000, ErrorMessage = "Ghi chú không được vượt quá 1000 ký tự")]
    public string Notes { get; set; }

    #endregion

    #region Properties - Thông tin hiển thị (Display)

    /// <summary>
    /// Mã sản phẩm/dịch vụ (từ ProductService)
    /// </summary>
    [DisplayName("Mã sản phẩm")]
    [Display(Order = 9)]
    public string ProductCode { get; set; }

    /// <summary>
    /// Tên sản phẩm/dịch vụ (từ ProductService)
    /// </summary>
    [DisplayName("Tên sản phẩm")]
    [Display(Order = 10)]
    public string ProductName { get; set; }

    /// <summary>
    /// Tên sản phẩm dịch vụ (lấy từ ProductVariant)
    /// </summary>
    [DisplayName("Tên biến thể")]
    [Display(Order = 10)]
    public string ProductVariantName { get; set; }

    /// <summary>
    /// Tên kiểu bảo hành (hiển thị)
    /// </summary>
    [DisplayName("Kiểu BH")]
    [Display(Order = 11)]
    public string WarrantyTypeName { get; set; }

    /// <summary>
    /// Tên trạng thái bảo hành (hiển thị)
    /// </summary>
    [DisplayName("Trạng thái")]
    [Display(Order = 12)]
    public string WarrantyStatusName { get; set; }

    /// <summary>
    /// Tên khách hàng (lấy từ StockInOutMaster)
    /// </summary>
    [DisplayName("Khách hàng")]
    [Display(Order = 13)]
    public string CustomerName { get; set; }

    /// <summary>
    /// Kiểm tra bảo hành đã hết hạn chưa (chỉ đọc)
    /// </summary>
    [DisplayName("Hết hạn BH")]
    [Display(Order = 14)]
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
    [Display(Order = 15)]
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
    /// Tổng hợp thông tin sản phẩm dưới dạng HTML (chỉ đọc)
    /// Hiển thị: UniqueProductInfo, thông tin sản phẩm dịch vụ, thông tin biến thể, thông tin khách hàng
    /// Sử dụng các tag HTML chuẩn của DevExpress: &lt;b&gt;, &lt;i&gt;, &lt;color&gt;, &lt;size&gt;
    /// Tham khảo: https://docs.devexpress.com/WindowsForms/4874/common-features/html-text-formatting
    /// </summary>
    [DisplayName("Thông tin SP HTML")]
    [Display(Order = 16)]
    [Description("Tổng hợp thông tin sản phẩm dưới dạng HTML")]
    public string UniqueProductInfoHtml
    {
        get
        {
            var html = string.Empty;

            // DeviceInfo (SerialNumber, IMEI, MACAddress, v.v.) - nổi bật nhất
            // Format chuyên nghiệp với visual hierarchy rõ ràng
            // - DeviceInfo: font lớn (12), bold, màu xanh đậm (primary)
            var deviceInfo = DeviceInfo ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(deviceInfo))
            {
                html += $"<size=12><b><color='blue'>{deviceInfo}</color></b></size>";
                html += "<br>";
            }

            // Thông tin sản phẩm dịch vụ (ProductService)
            // - Tên sản phẩm: font lớn (12), bold, màu xanh đậm (primary)
            // - Mã sản phẩm: font nhỏ hơn (9), màu xám (#757575)
            var productName = ProductName ?? string.Empty;
            var productCode = ProductCode ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(productName))
            {
                html += $"<size=12><b><color='blue'>{productName}</color></b></size>";
                if (!string.IsNullOrWhiteSpace(productCode))
                {
                    html += $" <size=9><color='#757575'>({productCode})</color></size>";
                }
                html += "<br>";
            }
            else if (!string.IsNullOrWhiteSpace(productCode))
            {
                // Nếu chỉ có mã sản phẩm
                html += $"<size=12><b><color='blue'>{productCode}</color></b></size>";
                html += "<br>";
            }

            // Thông tin biến thể (ProductVariantName)
            // - Label "Biến thể:" màu xám (#757575), size nhỏ (9)
            // - Value màu đen (#212121), size 10, bold
            var productVariantName = ProductVariantName ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(productVariantName))
            {
                html += $"<size=10><color='#212121'><b>{productVariantName}</b></color></size>";
                html += "<br>";
            }

            // Thông tin khách hàng
            // - Label "Khách hàng:" màu xám (#757575), size nhỏ (9)
            // - Value màu đen (#212121), size 10, bold
            var customerName = CustomerName ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(customerName))
            {
                html += $"<size=9><color='#757575'>Khách hàng:</color></size> <size=10><color='#212121'><b>{customerName}</b></color></size>";
            }

            return html;
        }
    }

    #endregion
}
