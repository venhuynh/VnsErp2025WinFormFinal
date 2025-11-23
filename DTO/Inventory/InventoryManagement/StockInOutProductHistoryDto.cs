using DTO.Inventory.StockIn;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace DTO.Inventory.InventoryManagement;

/// <summary>
/// Data Transfer Object cho lịch sử sản phẩm nhập xuất kho (StockInOutDetail)
/// Dùng để hiển thị thông tin lịch sử các sản phẩm/dịch vụ trong phiếu nhập xuất kho
/// </summary>
public class StockInOutProductHistoryDto
{
    #region Properties - Thông tin cơ bản

    /// <summary>
    /// ID duy nhất của chi tiết phiếu nhập xuất kho
    /// </summary>
    [DisplayName("ID")]
    [Display(Order = -1)]
    public Guid Id { get; set; }

    /// <summary>
    /// ID phiếu nhập xuất kho (Master)
    /// </summary>
    [DisplayName("ID Phiếu")]
    [Display(Order = 1)]
    public Guid StockInOutMasterId { get; set; }

    /// <summary>
    /// Số phiếu nhập xuất kho (từ Master)
    /// </summary>
    [DisplayName("Số phiếu")]
    [Display(Order = 2)]
    public string VocherNumber { get; set; }

    /// <summary>
    /// Ngày nhập xuất kho (từ Master)
    /// </summary>
    [DisplayName("Ngày nhập xuất")]
    [Display(Order = 3)]
    public DateTime StockInOutDate { get; set; }

    /// <summary>
    /// ID biến thể sản phẩm
    /// </summary>
    [DisplayName("ID Biến thể")]
    [Display(Order = 4)]
    public Guid ProductVariantId { get; set; }

    #endregion

    #region Properties - Thông tin sản phẩm

    /// <summary>
    /// Mã sản phẩm/dịch vụ (từ ProductService)
    /// </summary>
    [DisplayName("Mã sản phẩm")]
    [Display(Order = 10)]
    public string ProductCode { get; set; }

    /// <summary>
    /// Tên sản phẩm/dịch vụ (từ ProductService)
    /// </summary>
    [DisplayName("Tên sản phẩm")]
    [Display(Order = 11)]
    public string ProductName { get; set; }

    /// <summary>
    /// Mã biến thể sản phẩm
    /// </summary>
    [DisplayName("Mã biến thể")]
    [Display(Order = 12)]
    public string ProductVariantCode { get; set; }

    /// <summary>
    /// Tên biến thể sản phẩm đầy đủ
    /// </summary>
    [DisplayName("Tên biến thể")]
    [Display(Order = 13)]
    public string ProductVariantFullName { get; set; }

    /// <summary>
    /// ID đơn vị tính
    /// </summary>
    [DisplayName("ID ĐVT")]
    [Display(Order = 14)]
    public Guid? UnitOfMeasureId { get; set; }

    /// <summary>
    /// Mã đơn vị tính
    /// </summary>
    [DisplayName("Mã ĐVT")]
    [Display(Order = 15)]
    public string UnitOfMeasureCode { get; set; }

    /// <summary>
    /// Tên đơn vị tính
    /// </summary>
    [DisplayName("Đơn vị tính")]
    [Display(Order = 16)]
    public string UnitOfMeasureName { get; set; }

    #endregion

    #region Properties - Số lượng và giá

    /// <summary>
    /// Số lượng nhập
    /// </summary>
    [DisplayName("SL nhập")]
    [Display(Order = 20)]
    public decimal StockInQty { get; set; }

    /// <summary>
    /// Số lượng xuất
    /// </summary>
    [DisplayName("SL xuất")]
    [Display(Order = 21)]
    public decimal StockOutQty { get; set; }

    /// <summary>
    /// Đơn giá
    /// </summary>
    [DisplayName("Đơn giá")]
    [Display(Order = 22)]
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Thuế suất VAT (%)
    /// </summary>
    [DisplayName("VAT (%)")]
    [Display(Order = 23)]
    public decimal Vat { get; set; }

    /// <summary>
    /// Số tiền VAT
    /// </summary>
    [DisplayName("Tiền VAT")]
    [Display(Order = 24)]
    public decimal VatAmount { get; set; }

    /// <summary>
    /// Thành tiền (chưa VAT)
    /// </summary>
    [DisplayName("Thành tiền")]
    [Display(Order = 25)]
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Tổng tiền bao gồm VAT
    /// </summary>
    [DisplayName("Tổng tiền gồm VAT")]
    [Display(Order = 26)]
    public decimal TotalAmountIncludedVat { get; set; }

    #endregion

    #region Properties - Thông tin liên kết

    /// <summary>
    /// ID kho (từ Master)
    /// </summary>
    [DisplayName("ID Kho")]
    [Display(Order = 30)]
    public Guid WarehouseId { get; set; }

    /// <summary>
    /// Tên kho nhập xuất (từ Master)
    /// </summary>
    [DisplayName("Kho nhập xuất")]
    [Display(Order = 31)]
    public string WarehouseName { get; set; }

    /// <summary>
    /// Tên khách hàng (từ Master)
    /// </summary>
    [DisplayName("Tên khách hàng")]
    [Display(Order = 32)]
    public string CustomerName { get; set; }

    #endregion

    #region Properties - Hiển thị

    /// <summary>
    /// Nội dung tổng quát sản phẩm nhập xuất theo định dạng HTML theo chuẩn DevExpress
    /// Sử dụng các tag HTML chuẩn của DevExpress: &lt;b&gt;, &lt;i&gt;, &lt;color&gt;, &lt;size&gt;
    /// Tham khảo: https://docs.devexpress.com/WindowsForms/4874/common-features/html-text-formatting
    /// Format tương tự ProductVariantListDto.FullNameHtml để đảm bảo consistency
    /// </summary>
    [DisplayName("Nội dung HTML")]
    [Display(Order = 50)]
    [Description("Nội dung tổng quát sản phẩm nhập xuất dưới dạng HTML")]
    public string FullContentHtml
    {
        get
        {
            var productName = ProductName ?? string.Empty;
            var productCode = ProductCode ?? string.Empty;
            var variantCode = ProductVariantCode ?? string.Empty;
            var variantFullName = ProductVariantFullName ?? string.Empty;
            var unitName = UnitOfMeasureName ?? UnitOfMeasureCode ?? string.Empty;
            var vocherNumber = VocherNumber ?? string.Empty;

            // Format chuyên nghiệp với visual hierarchy rõ ràng (tham khảo ProductVariantListDto.FullNameHtml)
            // - Tên sản phẩm: font lớn, bold, màu xanh đậm (primary)
            // - Mã sản phẩm: font nhỏ hơn, màu xám
            // - Mã biến thể: font nhỏ hơn, màu cam (#FF9800)
            // - Tên biến thể đầy đủ: font nhỏ hơn, màu xám cho label, đen cho value
            // - Đơn vị tính: font nhỏ hơn, màu xám cho label, đen cho value
            // - Thông tin phiếu, kho, khách hàng: font nhỏ hơn, màu xám cho label, đen cho value
            // - Số lượng, giá, VAT: font nhỏ hơn, màu xám cho label, đen cho value
            // - Tổng tiền: font nhỏ hơn, màu xám cho label, xanh cho value

            var html = string.Empty;

            // === PHẦN 1: THÔNG TIN SẢN PHẨM (giống ProductVariantListDto) ===
            
            // Tên sản phẩm (nổi bật nhất)
            html += $"<size=12><b><color='blue'>{productName}</color></b></size>";

            // Mã sản phẩm
            if (!string.IsNullOrWhiteSpace(productCode))
            {
                html += $" <size=9><color='#757575'>({productCode})</color></size>";
            }

            html += "<br>";

            // Mã biến thể (màu cam)
            if (!string.IsNullOrWhiteSpace(variantCode))
            {
                html += $"<size=9><color='#757575'>Mã biến thể:</color></size> <size=10><color='#FF9800'><b>{variantCode}</b></color></size>";
            }

            // Tên biến thể đầy đủ
            if (!string.IsNullOrWhiteSpace(variantFullName))
            {
                if (!string.IsNullOrWhiteSpace(variantCode))
                    html += " | ";
                html += $"<size=9><color='#757575'>Tên biến thể:</color></size> <size=10><color='#212121'><b>{variantFullName}</b></color></size>";
            }

            if (!string.IsNullOrWhiteSpace(variantCode) || !string.IsNullOrWhiteSpace(variantFullName))
            {
                html += "<br>";
            }

            // Đơn vị tính
            if (!string.IsNullOrWhiteSpace(unitName))
            {
                html += $"<size=9><color='#757575'>Đơn vị tính:</color></size> <size=10><color='#212121'><b>{unitName}</b></color></size><br>";
            }

            // === PHẦN 2: THÔNG TIN PHIẾU NHẬP XUẤT ===

            // Số phiếu và ngày
            if (!string.IsNullOrWhiteSpace(vocherNumber))
            {
                html += $"<size=9><color='#757575'>Phiếu:</color></size> <size=10><color='#212121'><b>{vocherNumber}</b></color></size>";
                
                if (StockInOutDate != default(DateTime))
                {
                    html += $" <size=9><color='#757575'>({StockInOutDate:dd/MM/yyyy})</color></size>";
                }
                html += "<br>";
            }
            else if (StockInOutDate != default(DateTime))
            {
                html += $"<size=9><color='#757575'>Ngày:</color></size> <size=10><color='#212121'><b>{StockInOutDate:dd/MM/yyyy}</b></color></size><br>";
            }

            // Kho nhập xuất
            if (!string.IsNullOrWhiteSpace(WarehouseName))
            {
                html += $"<size=9><color='#757575'>Kho:</color></size> <size=10><color='#212121'><b>{WarehouseName}</b></color></size>";
            }

            // Tên khách hàng
            if (!string.IsNullOrWhiteSpace(CustomerName))
            {
                if (!string.IsNullOrWhiteSpace(WarehouseName))
                    html += " | ";
                html += $"<size=9><color='#757575'>Khách hàng:</color></size> <size=10><color='#212121'><b>{CustomerName}</b></color></size>";
            }

            if (!string.IsNullOrWhiteSpace(WarehouseName) || !string.IsNullOrWhiteSpace(CustomerName))
            {
                html += "<br>";
            }

            // === PHẦN 3: SỐ LƯỢNG VÀ GIÁ ===

            // Số lượng nhập
            if (StockInQty > 0)
            {
                var qtyText = $"{StockInQty:N2}";
                if (!string.IsNullOrWhiteSpace(unitName))
                {
                    qtyText += $" {unitName}";
                }
                html += $"<size=9><color='#757575'>SL nhập:</color></size> <size=10><color='#212121'><b>{qtyText}</b></color></size>";
            }

            // Số lượng xuất
            if (StockOutQty > 0)
            {
                if (StockInQty > 0)
                    html += " | ";
                var qtyText = $"{StockOutQty:N2}";
                if (!string.IsNullOrWhiteSpace(unitName))
                {
                    qtyText += $" {unitName}";
                }
                html += $"<size=9><color='#757575'>SL xuất:</color></size> <size=10><color='#212121'><b>{qtyText}</b></color></size>";
            }

            // Đơn giá
            if (UnitPrice > 0)
            {
                if (StockInQty > 0 || StockOutQty > 0)
                    html += " | ";
                html += $"<size=9><color='#757575'>Đơn giá:</color></size> <size=10><color='#212121'><b>{UnitPrice:N0}</b></color></size>";
            }

            if (StockInQty > 0 || StockOutQty > 0 || UnitPrice > 0)
            {
                html += "<br>";
            }

            // === PHẦN 4: VAT VÀ TỔNG TIỀN ===

            // VAT
            if (Vat > 0)
            {
                html += $"<size=9><color='#757575'>VAT:</color></size> <size=10><color='#212121'><b>{Vat}%</b></color></size>";
            }

            // Tiền VAT
            if (VatAmount > 0)
            {
                if (Vat > 0)
                    html += " | ";
                html += $"<size=9><color='#757575'>Tiền VAT:</color></size> <size=10><color='#212121'><b>{VatAmount:N0}</b></color></size>";
            }

            // Thành tiền
            if (TotalAmount > 0)
            {
                if (Vat > 0 || VatAmount > 0)
                    html += " | ";
                html += $"<size=9><color='#757575'>Thành tiền:</color></size> <size=10><color='#212121'><b>{TotalAmount:N0}</b></color></size>";
            }

            if (Vat > 0 || VatAmount > 0 || TotalAmount > 0)
            {
                html += "<br>";
            }

            // Tổng tiền bao gồm VAT (nổi bật - màu xanh)
            if (TotalAmountIncludedVat > 0)
            {
                html += $"<size=9><color='#757575'>Tổng tiền gồm VAT:</color></size> <size=10><color='#2196F3'><b>{TotalAmountIncludedVat:N0}</b></color></size>";
            }

            return html;
        }
    }

    #endregion
}

/// <summary>
/// Converter giữa StockInOutDetail entity và StockInOutProductHistoryDto
/// </summary>
public static class StockInOutProductHistoryDtoConverter
{
    #region Entity to DTO

    /// <summary>
    /// Chuyển đổi StockInOutDetail entity thành StockInOutProductHistoryDto
    /// </summary>
    /// <param name="entity">StockInOutDetail entity</param>
    /// <returns>StockInOutProductHistoryDto</returns>
    public static StockInOutProductHistoryDto ToDto(this Dal.DataContext.StockInOutDetail entity)
    {
        if (entity == null) return null;

        var dto = new StockInOutProductHistoryDto
        {
            Id = entity.Id,
            StockInOutMasterId = entity.StockInOutMasterId,
            ProductVariantId = entity.ProductVariantId,
            StockInQty = entity.StockInQty,
            StockOutQty = entity.StockOutQty,
            UnitPrice = entity.UnitPrice,
            Vat = entity.Vat,
            VatAmount = entity.VatAmount,
            TotalAmount = entity.TotalAmount,
            TotalAmountIncludedVat = entity.TotalAmountIncludedVat
        };

        // Lấy thông tin từ ProductVariant
        if (entity.ProductVariant != null)
        {
            dto.ProductVariantCode = entity.ProductVariant.VariantCode;
            dto.ProductVariantFullName = entity.ProductVariant.VariantFullName ?? string.Empty;

            dto.UnitOfMeasureId = entity.ProductVariant.UnitId;

            // Lấy thông tin đơn vị tính nếu có
            if (entity.ProductVariant.UnitOfMeasure != null)
            {
                dto.UnitOfMeasureCode = entity.ProductVariant.UnitOfMeasure.Code;
                dto.UnitOfMeasureName = entity.ProductVariant.UnitOfMeasure.Name;
            }

            // Lấy thông tin từ ProductService (sản phẩm/dịch vụ gốc)
            if (entity.ProductVariant.ProductService != null)
            {
                dto.ProductCode = entity.ProductVariant.ProductService.Code ?? string.Empty;
                dto.ProductName = entity.ProductVariant.ProductService.Name ?? string.Empty;
            }
        }

        // Lấy thông tin từ StockInOutMaster
        if (entity.StockInOutMaster != null)
        {
            dto.VocherNumber = entity.StockInOutMaster.VocherNumber;
            dto.StockInOutDate = entity.StockInOutMaster.StockInOutDate;
            dto.WarehouseId = entity.StockInOutMaster.WarehouseId;
            dto.WarehouseName = entity.StockInOutMaster.CompanyBranch?.BranchName;
            dto.CustomerName = entity.StockInOutMaster.BusinessPartnerSite?.BusinessPartner?.PartnerName ?? 
                              entity.StockInOutMaster.BusinessPartnerSite?.SiteName;
        }

        return dto;
    }

    /// <summary>
    /// Chuyển đổi danh sách StockInOutDetail entities thành danh sách StockInOutProductHistoryDto
    /// </summary>
    /// <param name="entities">Danh sách StockInOutDetail entities</param>
    /// <returns>Danh sách StockInOutProductHistoryDto</returns>
    public static List<StockInOutProductHistoryDto> ToDtoList(this IEnumerable<Dal.DataContext.StockInOutDetail> entities)
    {
        if (entities == null) return [];

        return entities.Select(entity => entity.ToDto()).ToList();
    }

    #endregion
}
