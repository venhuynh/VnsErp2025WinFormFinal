using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DTO.Inventory.InventoryManagement;

/// <summary>
/// Data Transfer Object cho lịch sử nhập xuất kho (StockInOutMaster)
/// Dùng để hiển thị thông tin lịch sử các phiếu nhập xuất kho
/// </summary>
public class StockInOutMasterHistoryDto
{
    #region Properties - Thông tin cơ bản

    /// <summary>
    /// ID duy nhất của phiếu nhập xuất kho
    /// </summary>
    [DisplayName("ID")]
    [Display(Order = -1)]
    public Guid Id { get; set; }

    /// <summary>
    /// Ngày nhập xuất kho
    /// </summary>
    [DisplayName("Ngày nhập xuất")]
    [Display(Order = 1)]
    public DateTime StockInOutDate { get; set; }

    /// <summary>
    /// Số phiếu nhập xuất kho
    /// </summary>
    [DisplayName("Số phiếu")]
    [Display(Order = 2)]
    public string VocherNumber { get; set; }

    /// <summary>
    /// Loại nhập xuất kho
    /// </summary>
    [DisplayName("Loại nhập xuất")]
    [Display(Order = 3)]
    public int StockInOutType { get; set; }

    #endregion

    #region Properties - Thông tin liên kết

    /// <summary>
    /// ID kho (WarehouseId)
    /// </summary>
    [DisplayName("ID Kho")]
    [Display(Order = 10)]
    public Guid WarehouseId { get; set; }

    /// <summary>
    /// Tên kho nhập xuất
    /// </summary>
    [DisplayName("Kho nhập xuất")]
    [Display(Order = 11)]
    public string WarehouseName { get; set; }

    /// <summary>
    /// ID đơn mua hàng (PO)
    /// </summary>
    [DisplayName("ID PO")]
    [Display(Order = 12)]
    public Guid? PurchaseOrderId { get; set; }

    /// <summary>
    /// ID địa điểm đối tác (PartnerSiteId)
    /// </summary>
    [DisplayName("ID Địa điểm đối tác")]
    [Display(Order = 13)]
    public Guid PartnerSiteId { get; set; }

    /// <summary>
    /// Tên khách hàng (hoặc nhà cung cấp)
    /// </summary>
    [DisplayName("Tên khách hàng")]
    [Display(Order = 14)]
    public string CustomerName { get; set; }

    #endregion

    #region Properties - Thông tin bổ sung

    /// <summary>
    /// Ghi chú
    /// </summary>
    [DisplayName("Ghi chú")]
    [Display(Order = 20)]
    public string Notes { get; set; }

    #endregion

    #region Properties - Tổng hợp

    /// <summary>
    /// Tổng số lượng
    /// </summary>
    [DisplayName("Tổng số lượng")]
    [Display(Order = 30)]
    public decimal TotalQuantity { get; set; }

    /// <summary>
    /// Tổng giá trị (chưa VAT)
    /// </summary>
    [DisplayName("Tổng tiền chưa VAT")]
    [Display(Order = 31)]
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Tổng VAT
    /// </summary>
    [DisplayName("Tổng VAT")]
    [Display(Order = 32)]
    public decimal TotalVat { get; set; }

    /// <summary>
    /// Tổng tiền bao gồm VAT
    /// </summary>
    [DisplayName("Tổng tiền bao gồm VAT")]
    [Display(Order = 33)]
    public decimal TotalAmountIncludedVat { get; set; }

    #endregion

    #region Properties - Thông tin hệ thống

    /// <summary>
    /// ID người tạo
    /// </summary>
    [DisplayName("ID Người tạo")]
    [Display(Order = 40)]
    public Guid? CreatedBy { get; set; }

    /// <summary>
    /// Ngày tạo
    /// </summary>
    [DisplayName("Ngày tạo")]
    [Display(Order = 41)]
    public DateTime? CreatedDate { get; set; }

    /// <summary>
    /// ID người cập nhật
    /// </summary>
    [DisplayName("ID Người cập nhật")]
    [Display(Order = 42)]
    public Guid? UpdatedBy { get; set; }

    /// <summary>
    /// Ngày cập nhật
    /// </summary>
    [DisplayName("Ngày cập nhật")]
    [Display(Order = 43)]
    public DateTime? UpdatedDate { get; set; }

    #endregion

    #region Properties - Hiển thị

    /// <summary>
    /// Mô tả sơ bộ chi tiết phiếu nhập xuất
    /// Hiển thị tóm tắt các sản phẩm trong phiếu (ví dụ: "3 sản phẩm: iPhone 15 Pro (2), MacBook Pro (1)")
    /// </summary>
    [DisplayName("Mô tả chi tiết")]
    [Display(Order = 49)]
    [Description("Mô tả sơ bộ các sản phẩm trong phiếu nhập xuất")]
    public string DetailsSummary { get; set; }

    /// <summary>
    /// Nội dung tổng quát phiếu nhập xuất theo định dạng HTML theo chuẩn DevExpress
    /// Sử dụng các tag HTML chuẩn của DevExpress: &lt;b&gt;, &lt;i&gt;, &lt;color&gt;, &lt;size&gt;
    /// Tham khảo: https://docs.devexpress.com/WindowsForms/4874/common-features/html-text-formatting
    /// </summary>
    [DisplayName("Nội dung HTML")]
    [Display(Order = 50)]
    [Description("Nội dung tổng quát phiếu nhập xuất dưới dạng HTML")]
    public string FullContentHtml
    {
        get
        {
            var vocherNumber = VocherNumber ?? string.Empty;
            var notes = Notes ?? string.Empty;

            // Format chuyên nghiệp với visual hierarchy rõ ràng
            // - Số phiếu: font lớn, bold, màu xanh đậm (primary)
            // - Ngày nhập xuất: font nhỏ hơn, màu xám
            // - Loại và trạng thái: font nhỏ hơn, màu xám cho label, đen cho value
            // - Tổng hợp: font nhỏ hơn, màu xám cho label, đen/xanh cho value

            var html = string.Empty;

            // Số phiếu (nổi bật nhất)
            if (!string.IsNullOrWhiteSpace(vocherNumber))
            {
                html += $"<size=12><b><color='blue'>{vocherNumber}</color></b></size>";
            }

            // Ngày nhập xuất
            if (StockInOutDate != default(DateTime))
            {
                if (!string.IsNullOrWhiteSpace(vocherNumber))
                {
                    html += $" <size=9><color='#757575'>({StockInOutDate:dd/MM/yyyy})</color></size>";
                }
                else
                {
                    html += $"<size=12><b><color='blue'>{StockInOutDate:dd/MM/yyyy}</color></b></size>";
                }
            }

            if (!string.IsNullOrWhiteSpace(vocherNumber) || StockInOutDate != default(DateTime))
            {
                html += "<br>";
            }

            // Loại nhập xuất
            if (StockInOutType != 0)
            {
                html += $"<size=9><color='#757575'>Loại:</color></size> <size=10><color='#212121'><b>{StockInOutType}</b></color></size>";
            }

            if (StockInOutType != 0)
            {
                html += "<br>";
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

            // Mô tả chi tiết
            if (!string.IsNullOrWhiteSpace(DetailsSummary))
            {
                html += $"<size=9><color='#757575'><i>{DetailsSummary}</i></color></size>";
                html += "<br>";
            }

            // Tổng số lượng
            if (TotalQuantity > 0)
            {
                html += $"<size=9><color='#757575'>Tổng số lượng:</color></size> <size=10><color='#212121'><b>{TotalQuantity:N2}</b></color></size>";
            }

            // Tổng tiền chưa VAT
            if (TotalAmount > 0)
            {
                if (TotalQuantity > 0)
                    html += " | ";
                html += $"<size=9><color='#757575'>Tổng tiền:</color></size> <size=10><color='#212121'><b>{TotalAmount:N0}</b></color></size>";
            }

            // VAT
            if (TotalVat > 0)
            {
                if (TotalQuantity > 0 || TotalAmount > 0)
                    html += " | ";
                html += $"<size=9><color='#757575'>VAT:</color></size> <size=10><color='#212121'><b>{TotalVat:N0}</b></color></size>";
            }

            if (TotalQuantity > 0 || TotalAmount > 0 || TotalVat > 0)
            {
                html += "<br>";
            }

            // Tổng tiền bao gồm VAT (nổi bật)
            if (TotalAmountIncludedVat > 0)
            {
                html += $"<size=9><color='#757575'>Tổng tiền gồm VAT:</color></size> <size=10><color='#2196F3'><b>{TotalAmountIncludedVat:N0}</b></color></size>";
            }

            // Ghi chú (nếu có)
            if (!string.IsNullOrWhiteSpace(notes))
            {
                if (TotalQuantity > 0 || TotalAmount > 0 || TotalVat > 0 || TotalAmountIncludedVat > 0)
                {
                    html += "<br>";
                }
                html += $"<size=9><color='#757575'><i>{notes}</i></color></size>";
            }

            return html;
        }
    }

    #endregion
}

/// <summary>
/// Converter giữa StockInOutMaster entity và StockInOutMasterHistoryDto
/// </summary>
public static class StockInOutMasterHistoryDtoConverter
{
    #region Entity to DTO

    /// <summary>
    /// Chuyển đổi StockInOutMaster entity thành StockInOutMasterHistoryDto
    /// </summary>
    /// <param name="entity">StockInOutMaster entity</param>
    /// <returns>StockInOutMasterHistoryDto</returns>
    public static StockInOutMasterHistoryDto ToDto(this Dal.DataContext.StockInOutMaster entity)
    {
        if (entity == null) return null;

        var dto = new StockInOutMasterHistoryDto
        {
            Id = entity.Id,
            StockInOutDate = entity.StockInOutDate,
            VocherNumber = entity.VocherNumber,
            StockInOutType = entity.StockInOutType,
            WarehouseId = entity.WarehouseId,
            WarehouseName = entity.CompanyBranch?.BranchName,
            PurchaseOrderId = entity.PurchaseOrderId,
            PartnerSiteId = entity.PartnerSiteId,
            CustomerName = entity.BusinessPartnerSite?.BusinessPartner?.PartnerName ?? 
                          entity.BusinessPartnerSite?.SiteName,
            Notes = entity.Notes,
            TotalQuantity = entity.TotalQuantity,
            TotalAmount = entity.TotalAmount,
            TotalVat = entity.TotalVat,
            TotalAmountIncludedVat = entity.TotalAmountIncludedVat,
            CreatedBy = entity.CreatedBy,
            CreatedDate = entity.CreatedDate,
            UpdatedBy = entity.UpdatedBy,
            UpdatedDate = entity.UpdatedDate,
            DetailsSummary = BuildDetailsSummary(entity)
        };

        return dto;
    }

    /// <summary>
    /// Chuyển đổi danh sách StockInOutMaster entities thành danh sách StockInOutMasterHistoryDto
    /// </summary>
    /// <param name="entities">Danh sách StockInOutMaster entities</param>
    /// <returns>Danh sách StockInOutMasterHistoryDto</returns>
    public static List<StockInOutMasterHistoryDto> ToDtoList(this IEnumerable<Dal.DataContext.StockInOutMaster> entities)
    {
        if (entities == null) return [];

        return entities.Select(entity => entity.ToDto()).ToList();
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Xây dựng mô tả sơ bộ chi tiết phiếu nhập xuất
    /// </summary>
    /// <param name="entity">StockInOutMaster entity</param>
    /// <returns>Mô tả sơ bộ (ví dụ: "3 sản phẩm: iPhone 15 Pro (2), MacBook Pro (1)")</returns>
    private static string BuildDetailsSummary(Dal.DataContext.StockInOutMaster entity)
    {
        if (entity == null || entity.StockInOutDetails == null || !entity.StockInOutDetails.Any())
        {
            return string.Empty;
        }

        try
        {
            var details = entity.StockInOutDetails.ToList();
            var totalItems = details.Count;

            // Nhóm theo sản phẩm và tính tổng số lượng
            var productGroups = details
                .Where(d => d.ProductVariant != null)
                .GroupBy(d => new
                {
                    ProductName = d.ProductVariant.ProductService?.Name ?? 
                                  d.ProductVariant.VariantFullName ?? 
                                  "Không xác định",
                    ProductCode = d.ProductVariant.VariantCode ?? string.Empty
                })
                .Select(g => new
                {
                    ProductName = g.Key.ProductName,
                    ProductCode = g.Key.ProductCode,
                    TotalQty = g.Sum(d => d.StockInQty + d.StockOutQty)
                })
                .OrderBy(p => p.ProductName)
                .ToList();

            if (!productGroups.Any())
            {
                return $"{totalItems} sản phẩm";
            }

            // Tạo mô tả: "3 sản phẩm: iPhone 15 Pro (2), MacBook Pro (1)"
            var summary = $"{totalItems} sản phẩm";
            
            if (productGroups.Count <= 3)
            {
                // Nếu ít hơn hoặc bằng 3 sản phẩm, liệt kê tất cả
                var items = productGroups.Select(p => 
                {
                    var name = !string.IsNullOrWhiteSpace(p.ProductName) ? p.ProductName : p.ProductCode;
                    return $"{name} ({p.TotalQty:N0})";
                });
                summary += ": " + string.Join(", ", items);
            }
            else
            {
                // Nếu nhiều hơn 3 sản phẩm, chỉ liệt kê 3 đầu tiên
                var topItems = productGroups.Take(3).Select(p => 
                {
                    var name = !string.IsNullOrWhiteSpace(p.ProductName) ? p.ProductName : p.ProductCode;
                    return $"{name} ({p.TotalQty:N0})";
                });
                summary += ": " + string.Join(", ", topItems) + $", ... (+{productGroups.Count - 3} sản phẩm khác)";
            }

            return summary;
        }
        catch
        {
            // Nếu có lỗi, trả về số lượng đơn giản
            return $"{entity.StockInOutDetails.Count} sản phẩm";
        }
    }

    #endregion
}