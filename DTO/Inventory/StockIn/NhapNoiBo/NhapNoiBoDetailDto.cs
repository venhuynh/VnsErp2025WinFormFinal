using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DTO.Inventory.StockIn.NhapNoiBo;

/// <summary>
/// Data Transfer Object cho chi tiết phiếu nhập kho
/// Dùng cho GridControl và truyền dữ liệu giữa Service ↔ WinForms
/// Map với bảng StockInOutDetail trong database
/// </summary>
public class NhapNoiBoDetailDto
{
    #region Properties - Thông tin cơ bản (map với DB)

    /// <summary>
    /// ID duy nhất của chi tiết phiếu nhập
    /// Map với: StockInOutDetail.Id
    /// </summary>
    [DisplayName("ID")]
    [Display(Order = -1)]
    public Guid Id { get; set; }

    /// <summary>
    /// ID phiếu nhập kho (Master)
    /// Map với: StockInOutDetail.StockInOutMasterId
    /// </summary>
    [DisplayName("ID Phiếu nhập")]
    [Display(Order = 0)]
    [Required(ErrorMessage = "ID phiếu nhập không được để trống")]
    public Guid StockInOutMasterId { get; set; }

    /// <summary>
    /// ID biến thể sản phẩm (ProductVariant)
    /// Map với: StockInOutDetail.ProductVariantId
    /// </summary>
    [DisplayName("ID Biến thể")]
    [Display(Order = 1)]
    [Required(ErrorMessage = "Biến thể sản phẩm không được để trống")]
    public Guid ProductVariantId { get; set; }

    /// <summary>
    /// Thứ tự dòng (dùng cho UI, không có trong DB)
    /// </summary>
    [DisplayName("STT")]
    [Display(Order = 2)]
    public int LineNumber { get; set; }

    #endregion

    #region Properties - Thông tin hàng hóa (hiển thị, lấy từ ProductVariant)

    /// <summary>
    /// Mã biến thể sản phẩm (để hiển thị)
    /// Lấy từ ProductVariant.VariantCode
    /// </summary>
    [DisplayName("Mã hàng")]
    [Display(Order = 10)]
    public string ProductVariantCode { get; set; }

    /// <summary>
    /// Tên biến thể sản phẩm (để hiển thị)
    /// Lấy từ ProductVariant.VariantFullName hoặc ProductService.Name
    /// </summary>
    [DisplayName("Tên hàng")]
    [Display(Order = 11)]
    public string ProductVariantName { get; set; }

    /// <summary>
    /// ID đơn vị tính (để hiển thị)
    /// Lấy từ ProductVariant.UnitId
    /// </summary>
    [DisplayName("ID ĐVT")]
    [Display(Order = 12)]
    public Guid? UnitOfMeasureId { get; set; }

    /// <summary>
    /// Mã đơn vị tính (để hiển thị)
    /// Lấy từ UnitOfMeasure.Code
    /// </summary>
    [DisplayName("Mã ĐVT")]
    [Display(Order = 13)]
    public string UnitOfMeasureCode { get; set; }

    /// <summary>
    /// Tên đơn vị tính (để hiển thị)
    /// Lấy từ UnitOfMeasure.Name
    /// </summary>
    [DisplayName("Đơn vị tính")]
    [Display(Order = 14)]
    public string UnitOfMeasureName { get; set; }

    #endregion

    #region Properties - Số lượng và giá (map với DB)

    /// <summary>
    /// Số lượng nhập
    /// Map với: StockInOutDetail.StockInQty
    /// </summary>
    [DisplayName("SL nhập")]
    [Display(Order = 20)]
    [Required(ErrorMessage = "Số lượng nhập không được để trống")]
    [Range(0, double.MaxValue, ErrorMessage = "Số lượng nhập phải lớn hơn hoặc bằng 0")]
    public decimal StockInQty { get; set; }

    /// <summary>
    /// Ghi chú tình trạng sản phẩm
    /// </summary>
    [DisplayName("Tình trạng")]
    [Display(Order = 27)]
    public string GhiChu { get; set; } = "Bình thường";

    /// <summary>
    /// Danh sách thông tin bảo hành cho sản phẩm này
    /// </summary>
    [DisplayName("Thông tin bảo hành")]
    [Display(Order = 28)]
    public List<DTO.Inventory.InventoryManagement.WarrantyDto> Warranties { get; set; } = new List<DTO.Inventory.InventoryManagement.WarrantyDto>();

    /// <summary>
    /// Thông tin chi tiết phiếu nhập dưới dạng HTML theo format DevExpress
    /// Sử dụng các tag HTML chuẩn của DevExpress: &lt;b&gt;, &lt;i&gt;, &lt;color&gt;, &lt;size&gt;
    /// Tham khảo: https://docs.devexpress.com/WindowsForms/4874/common-features/html-text-formatting
    /// </summary>
    [DisplayName("Thông tin HTML")]
    [Display(Order = 30)]
    [Description("Thông tin chi tiết phiếu nhập dưới dạng HTML")]
    public string FullNameHtml
    {
        get
        {
            var productVariantName = ProductVariantName ?? string.Empty;
            var productVariantCode = ProductVariantCode ?? string.Empty;
            var unitName = UnitOfMeasureName ?? string.Empty;
            var unitCode = UnitOfMeasureCode ?? string.Empty;

            // Format chuyên nghiệp với visual hierarchy rõ ràng
            // - Tên sản phẩm: font lớn, bold, màu xanh đậm (primary)
            // - Mã sản phẩm: font nhỏ hơn, màu xám
            // - Đơn vị tính: font nhỏ hơn, màu xám cho label, đen cho value
            // - Số lượng và giá: font nhỏ hơn, màu xám cho label, đen cho value

            var html = string.Empty;

            // Tên sản phẩm (nổi bật nhất)
            if (!string.IsNullOrWhiteSpace(productVariantName))
            {
                html += $"<size=12><b><color='blue'>{productVariantName}</color></b></size>";
            }

            // Mã sản phẩm (nếu có)
            if (!string.IsNullOrWhiteSpace(productVariantCode))
            {
                if (!string.IsNullOrWhiteSpace(productVariantName))
                {
                    html += $" <size=9><color='#757575'>({productVariantCode})</color></size>";
                }
                else
                {
                    html += $"<size=12><b><color='blue'>{productVariantCode}</color></b></size>";
                }
            }

            if (!string.IsNullOrWhiteSpace(productVariantName) || !string.IsNullOrWhiteSpace(productVariantCode))
            {
                html += "<br>";
            }

            // Đơn vị tính
            if (!string.IsNullOrWhiteSpace(unitCode) || !string.IsNullOrWhiteSpace(unitName))
            {
                var unitDisplay = string.IsNullOrWhiteSpace(unitCode)
                    ? unitName
                    : string.IsNullOrWhiteSpace(unitName)
                        ? unitCode
                        : $"{unitCode} - {unitName}";

                html += $"<size=9><color='#757575'>Đơn vị tính:</color></size> <size=10><color='#212121'><b>{unitDisplay}</b></color></size><br>";
            }

            // Số lượng nhập
            if (StockInQty > 0)
            {
                html += $"<size=9><color='#757575'>Số lượng:</color></size> <size=10><color='#212121'><b>{StockInQty:N2}</b></color></size>";
            }

            return html;
        }
    }

    #endregion
}

/// <summary>
/// Converter giữa StockInOutDetail entity và NhapThietBiMuonDetailDto
/// </summary>
public static class NhapNoiBoConverter
{
    #region Entity to DTO

    /// <summary>
    /// Chuyển đổi StockInOutDetail entity thành NhapThietBiMuonDetailDto
    /// </summary>
    /// <param name="entity">StockInOutDetail entity</param>
    /// <returns>NhapThietBiMuonDetailDto</returns>
    public static NhapNoiBoDetailDto ToNhapNoiBoDetailDtolDto(this Dal.DataContext.StockInOutDetail entity)
    {
        if (entity == null) return null;

        var dto = new NhapNoiBoDetailDto
        {
            Id = entity.Id,
            StockInOutMasterId = entity.StockInOutMasterId,
            ProductVariantId = entity.ProductVariantId,
            ProductVariantCode = entity.ProductVariant?.VariantCode,
            StockInQty = entity.StockInQty,
            GhiChu = entity.GhiChu ?? "Bình thường",
            LineNumber = 0 // Sẽ được cập nhật sau nếu cần
        };

        // Lấy thông tin ProductVariant nếu có
        if (entity.ProductVariant != null)
        {
            dto.ProductVariantCode = entity.ProductVariant.VariantCode;

            // Lấy tên đơn giản - tương tự FullName của ProductVariantListDto
            // Format: VariantCode - ProductName VariantFullName (UnitName)
            // FullNameHtml sẽ xử lý format HTML phức tạp hơn
            var productName = entity.ProductVariant.ProductService?.Name ?? string.Empty;
            var variantCode = entity.ProductVariant.VariantCode ?? string.Empty;
            var variantFullName = entity.ProductVariant.VariantFullName ?? string.Empty;
            var unitName = entity.ProductVariant.UnitOfMeasure?.Name ?? string.Empty;

            // Tạo tên hiển thị đơn giản (text thuần, không HTML) - tương tự FullName của ProductVariantListDto
            var nameParts = new List<string>();
            
            if (!string.IsNullOrWhiteSpace(variantCode))
            {
                nameParts.Add(variantCode);
            }

            if (!string.IsNullOrWhiteSpace(productName))
            {
                nameParts.Add(productName);
            }

            if (!string.IsNullOrWhiteSpace(variantFullName))
            {
                nameParts.Add(variantFullName);
            }

            if (nameParts.Count > 0)
            {
                dto.ProductVariantName = string.Join(" ", nameParts);
                
                if (!string.IsNullOrWhiteSpace(unitName))
                {
                    dto.ProductVariantName += $" ({unitName})";
                }
            }
            else
            {
                // Fallback: Nếu không có thông tin, sử dụng VariantCode hoặc ProductName
                dto.ProductVariantName = !string.IsNullOrWhiteSpace(variantCode) 
                    ? variantCode 
                    : productName;
            }

            // Lấy thông tin đơn vị tính
            dto.UnitOfMeasureId = entity.ProductVariant.UnitId;
            
            if (entity.ProductVariant.UnitOfMeasure != null)
            {
                dto.UnitOfMeasureCode = entity.ProductVariant.UnitOfMeasure.Code;
                dto.UnitOfMeasureName = entity.ProductVariant.UnitOfMeasure.Name;
            }
        }

        return dto;
    }

    /// <summary>
    /// Chuyển đổi danh sách StockInOutDetail entities thành danh sách NhapThietBiMuonDetailDto
    /// </summary>
    /// <param name="entities">Danh sách StockInOutDetail entities</param>
    /// <returns>Danh sách NhapThietBiMuonDetailDto</returns>
    public static List<NhapNoiBoDetailDto> ToDtoList(this IEnumerable<Dal.DataContext.StockInOutDetail> entities)
    {
        if (entities == null) return new List<NhapNoiBoDetailDto>();

        return entities.Select(entity => entity.ToNhapNoiBoDetailDtolDto()).Where(dto => dto != null).ToList();
    }

    #endregion
}