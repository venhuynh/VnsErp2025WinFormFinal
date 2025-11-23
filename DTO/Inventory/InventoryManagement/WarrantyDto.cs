using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Dal.DataContext;
using DTO.Inventory.StockIn;
using Common.Enums;

namespace DTO.Inventory.InventoryManagement;

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
    /// ID chi tiết phiếu nhập/xuất kho
    /// </summary>
    [DisplayName("ID Chi tiết phiếu")]
    [Display(Order = 1)]
    [Required(ErrorMessage = "ID chi tiết phiếu nhập/xuất kho không được để trống")]
    public Guid StockInOutDetailId { get; set; }

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
    /// Thông tin sản phẩm duy nhất (Serial Number, IMEI, v.v.)
    /// </summary>
    [DisplayName("Thông tin SP duy nhất")]
    [Display(Order = 7)]
    [Required(ErrorMessage = "Thông tin sản phẩm duy nhất không được để trống")]
    [StringLength(200, ErrorMessage = "Thông tin sản phẩm duy nhất không được vượt quá 200 ký tự")]
    public string UniqueProductInfo { get; set; }

    #endregion

    #region Properties - Thông tin hiển thị (Display)

    /// <summary>
    /// Tên sản phẩm dịch vụ (lấy từ ProductVariant)
    /// </summary>
    [DisplayName("Tên sản phẩm")]
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
    /// Tổng hợp thông tin bảo hành dưới dạng HTML (chỉ đọc)
    /// Hiển thị đầy đủ thông tin: tên sản phẩm, sản phẩm, kiểu bảo hành, trạng thái, thời gian bảo hành, tình trạng
    /// Sử dụng các tag HTML chuẩn của DevExpress: &lt;b&gt;, &lt;i&gt;, &lt;color&gt;, &lt;size&gt;
    /// Tham khảo: https://docs.devexpress.com/WindowsForms/4874/common-features/html-text-formatting
    /// </summary>
    [DisplayName("Thông tin tổng hợp")]
    [Display(Order = 15)]
    [Description("Tổng hợp đầy đủ thông tin bảo hành dưới dạng HTML")]
    public string FullInfo
    {
        get
        {
            var uniqueProductInfo = UniqueProductInfo ?? string.Empty;
            var warrantyStatusName = WarrantyStatusName ?? string.Empty;
            var warrantyStatusText = WarrantyStatusText ?? string.Empty;

            // Xác định màu sắc cho trạng thái bảo hành
            var statusColor = GetWarrantyStatusColor(WarrantyStatus);
                
            // Xác định màu sắc cho tình trạng (còn/hết hạn)
            var statusTextColor = IsWarrantyExpired ? "#F44336" : "#4CAF50";

            // Format chuyên nghiệp với visual hierarchy rõ ràng
            // - Thông tin sản phẩm: font lớn, bold, màu xanh đậm (primary)
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

            // Thông tin sản phẩm duy nhất (Serial Number, IMEI, v.v.)
            if (!string.IsNullOrWhiteSpace(uniqueProductInfo))
            {
                html += $"<size=9><color='blue'>Serial/IMEI:</color></size> <size=10><color='blue'><b>{uniqueProductInfo}</b></color></size><br>";
            }

            // Kiểu bảo hành
            var warrantyTypeName = WarrantyTypeName ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(warrantyTypeName))
            {
                html += $"<size=9><color='#757575'>Kiểu BH:</color></size> <size=10><color='#212121'><b>{warrantyTypeName}</b></color></size><br>";
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
    /// <returns>Mã màu hex</returns>
    private string GetWarrantyStatusColor(TrangThaiBaoHanhEnum status)
    {
        return status switch
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
            StockInOutDetailId = entity.StockInOutDetailId,
            WarrantyFrom = entity.WarrantyFrom,
            MonthOfWarranty = entity.MonthOfWarranty,
            WarrantyUntil = entity.WarrantyUntil,
            UniqueProductInfo = entity.UniqueProductInfo
        };

        // Lấy tên sản phẩm từ ProductVariant thông qua StockInOutDetail
        if (entity.StockInOutDetail != null && entity.StockInOutDetail.ProductVariant != null)
        {
            var productVariant = entity.StockInOutDetail.ProductVariant;
            // Ưu tiên VariantFullName, nếu không có thì lấy từ ProductService.Name
            if (!string.IsNullOrWhiteSpace(productVariant.VariantFullName))
            {
                dto.ProductVariantName = productVariant.VariantFullName;
            }
            else if (productVariant.ProductService != null && !string.IsNullOrWhiteSpace(productVariant.ProductService.Name))
            {
                dto.ProductVariantName = productVariant.ProductService.Name;
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
            StockInOutDetailId = dto.StockInOutDetailId,
            WarrantyType = (int)dto.WarrantyType, // Chuyển đổi enum sang int
            WarrantyFrom = dto.WarrantyFrom,
            MonthOfWarranty = dto.MonthOfWarranty,
            WarrantyUntil = dto.WarrantyUntil,
            WarrantyStatus = (int)dto.WarrantyStatus, // Chuyển đổi enum sang int
            UniqueProductInfo = dto.UniqueProductInfo
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