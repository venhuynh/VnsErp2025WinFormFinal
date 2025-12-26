using DTO.Inventory.StockIn;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

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
    [DisplayName("Loại nhập xuất-Int")]
    [Display(Order = 3)]
    public int StockInOutType { get; set; }

    /// <summary>
    /// Loại nhập kho
    /// </summary>
    [DisplayName("Loại nhập xuất-Enum")]
    [Display(Order = 4)]
    public LoaiNhapXuatKhoEnum LoaiNhapXuatKho { get; set; }

    /// <summary>
    /// Tên loại nhập kho (hiển thị)
    /// </summary>
    [DisplayName("Loại nhập xuất")]
    [Display(Order = 5)]
    public string LoaiNhapXuatKhoName { get; set; }

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
    public Guid? PartnerSiteId { get; set; }

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

    /// <summary>
    /// Người nhận hàng
    /// </summary>
    [DisplayName("Người nhận hàng")]
    [Display(Order = 21)]
    public string NguoiNhanHang { get; set; }

    /// <summary>
    /// Người giao hàng
    /// </summary>
    [DisplayName("Người giao hàng")]
    [Display(Order = 22)]
    public string NguoiGiaoHang { get; set; }

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
    /// Sử dụng các tag HTML chuẩn của DevExpress: &lt;b&gt;, &lt;i&gt;, &lt;color&gt;
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
            // - Số phiếu: bold, màu xanh đậm (primary)
            // - Ngày nhập xuất: màu xám
            // - Loại và trạng thái: màu xám cho label, đen cho value
            // - Tổng hợp: màu xám cho label, đen/xanh cho value

            var html = string.Empty;

            // Số phiếu (nổi bật nhất)
            if (!string.IsNullOrWhiteSpace(vocherNumber))
            {
                html += $"<b><color='blue'>{vocherNumber}</color></b>";
            }

            // Ngày nhập xuất
            if (StockInOutDate != default(DateTime))
            {
                if (!string.IsNullOrWhiteSpace(vocherNumber))
                {
                    html += $" <color='#blue'>({StockInOutDate:dd/MM/yyyy})</color>";
                }
                else
                {
                    html += $"<b><color='blue'>{StockInOutDate:dd/MM/yyyy}</color></b>";
                }
            }

            //if (!string.IsNullOrWhiteSpace(vocherNumber) || StockInOutDate != default(DateTime))
            //{
            //    html += "<br>";
            //}

            // Loại nhập xuất (sử dụng LoaiNhapXuatKhoName nếu có, fallback về StockInOutType)
            if (!string.IsNullOrWhiteSpace(LoaiNhapXuatKhoName))
            {
                html += $"<color='#757575'> | Loại:</color> <color='#blue'><b>{LoaiNhapXuatKhoName}</b></color>";
            }
            else if (StockInOutType != 0)
            {
                html += $"<color='#757575'>Loại:</color> <color='#blue'><b>{StockInOutType}</b></color>";
            }

            if (!string.IsNullOrWhiteSpace(LoaiNhapXuatKhoName) || StockInOutType != 0)
            {
                html += "<br>";
            }

            // Kho nhập xuất
            if (!string.IsNullOrWhiteSpace(WarehouseName))
            {
                html += $"<color='#757575'>Kho:</color> <color='#212121'><b>{WarehouseName}</b></color>";
            }

            // Tên khách hàng
            if (!string.IsNullOrWhiteSpace(CustomerName))
            {
                if (!string.IsNullOrWhiteSpace(WarehouseName))
                    html += " | ";
                html += $"<color='#757575'>Khách hàng:</color> <color='#blue'><b>{CustomerName}</b></color>";
            }

            if (!string.IsNullOrWhiteSpace(WarehouseName) || !string.IsNullOrWhiteSpace(CustomerName))
            {
                html += "<br>";
            }

            // Mô tả chi tiết
            if (!string.IsNullOrWhiteSpace(DetailsSummary))
            {
                html += $"<color='#blue'><i>{DetailsSummary}</i></color>";
                html += "<br>";
            }

            //// Tổng số lượng
            //if (TotalQuantity > 0)
            //{
            //    html += $"<color='#757575'>Tổng số lượng:</color> <color='#212121'><b>{TotalQuantity:N2}</b></color>";
            //}

            //// Tổng tiền chưa VAT
            //if (TotalAmount > 0)
            //{
            //    if (TotalQuantity > 0)
            //        html += " | ";
            //    html += $"<color='#757575'>Tổng tiền:</color> <color='#212121'><b>{TotalAmount:N0}</b></color>";
            //}

            //// VAT
            //if (TotalVat > 0)
            //{
            //    if (TotalQuantity > 0 || TotalAmount > 0)
            //        html += " | ";
            //    html += $"<color='#757575'>VAT:</color> <color='#212121'><b>{TotalVat:N0}</b></color>";
            //}

            //if (TotalQuantity > 0 || TotalAmount > 0 || TotalVat > 0)
            //{
            //    html += "<br>";
            //}

            //// Tổng tiền bao gồm VAT (nổi bật)
            //if (TotalAmountIncludedVat > 0)
            //{
            //    html += $"<color='#757575'>Tổng tiền gồm VAT:</color> <color='#2196F3'><b>{TotalAmountIncludedVat:N0}</b></color>";
            //}

            // Người nhận hàng và người giao hàng
            var nguoiNhanHang = NguoiNhanHang ?? string.Empty;
            var nguoiGiaoHang = NguoiGiaoHang ?? string.Empty;
            
            if (!string.IsNullOrWhiteSpace(nguoiNhanHang) || !string.IsNullOrWhiteSpace(nguoiGiaoHang))
            {
                //if (TotalQuantity > 0 || TotalAmount > 0 || TotalVat > 0 || TotalAmountIncludedVat > 0 || !string.IsNullOrWhiteSpace(notes))
                //{
                //    html += "<br>";
                //}
                
                if (!string.IsNullOrWhiteSpace(nguoiNhanHang))
                {
                    html += $"<color='#757575'>Người nhận:</color> <color='#212121'><b>{nguoiNhanHang}</b></color>";
                }
                
                if (!string.IsNullOrWhiteSpace(nguoiGiaoHang))
                {
                    if (!string.IsNullOrWhiteSpace(nguoiNhanHang))
                        html += " | ";
                    html += $"<color='#757575'>Người giao:</color> <color='#212121'><b>{nguoiGiaoHang}</b></color>";
                }
            }

            // Ghi chú (nếu có)
            if (!string.IsNullOrWhiteSpace(notes))
            {
                if (TotalQuantity > 0 || TotalAmount > 0 || TotalVat > 0 || TotalAmountIncludedVat > 0 || 
                    !string.IsNullOrWhiteSpace(nguoiNhanHang) || !string.IsNullOrWhiteSpace(nguoiGiaoHang))
                {
                    html += "<br>";
                }
                html += $"<color='#757575'><i>{notes}</i></color>";
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
            NguoiNhanHang = entity.NguoiNhanHang,
            NguoiGiaoHang = entity.NguoiGiaoHang,
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

        // Chuyển đổi StockInOutType từ int sang LoaiNhapXuatKhoEnum
        if (Enum.IsDefined(typeof(LoaiNhapXuatKhoEnum), entity.StockInOutType))
        {
            dto.LoaiNhapXuatKho = (LoaiNhapXuatKhoEnum)entity.StockInOutType;
        }
        else
        {
            // Nếu giá trị không hợp lệ, mặc định là Khac
            dto.LoaiNhapXuatKho = LoaiNhapXuatKhoEnum.Khac;
        }

        // Lấy tên loại nhập xuất từ Description attribute
        dto.LoaiNhapXuatKhoName = GetEnumDescription(dto.LoaiNhapXuatKho);

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
    /// Lấy Description từ enum value (generic method)
    /// </summary>
    /// <typeparam name="T">Kiểu enum</typeparam>
    /// <param name="enumValue">Giá trị enum</param>
    /// <returns>Description hoặc tên enum nếu không có Description</returns>
    private static string GetEnumDescription<T>(T enumValue) where T : Enum
    {
        var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
        if (fieldInfo == null) return enumValue.ToString();

        var descriptionAttribute = fieldInfo.GetCustomAttribute<System.ComponentModel.DescriptionAttribute>();
        return descriptionAttribute?.Description ?? enumValue.ToString();
    }

    /// <summary>
    /// Xây dựng mô tả sơ bộ chi tiết phiếu nhập xuất
    /// </summary>
    /// <param name="entity">StockInOutMaster entity</param>
    /// <returns>Mô tả sơ bộ (ví dụ: "3 sản phẩm: iPhone 15 Pro (2), MacBook Pro (1)")</returns>
    private static string BuildDetailsSummary(Dal.DataContext.StockInOutMaster entity)
    {
        if (entity == null)
        {
            return string.Empty;
        }

        try
        {
            // Kiểm tra xem StockInOutDetails có sẵn không (đã được load)
            // Nếu DataContext đã bị dispose, sẽ throw exception
            if (entity.StockInOutDetails == null)
            {
                return string.Empty;
            }

            // Thử truy cập để kiểm tra xem DataContext còn sống không
            var details = entity.StockInOutDetails.ToList();
            
            if (details == null || details.Count == 0)
            {
                return string.Empty;
            }

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
        catch (System.ObjectDisposedException)
        {
            // DataContext đã bị dispose, không thể truy cập StockInOutDetails
            // Trả về string rỗng để tránh lỗi
            return string.Empty;
        }
        catch (Exception)
        {
            // Nếu có lỗi khác, trả về string rỗng để tránh crash
            return string.Empty;
        }
    }

    #endregion
}