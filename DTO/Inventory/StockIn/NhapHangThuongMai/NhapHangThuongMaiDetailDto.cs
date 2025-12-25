using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using DTO.DeviceAssetManagement;
using DTO.Inventory.InventoryManagement;

namespace DTO.Inventory.StockIn.NhapHangThuongMai;

/// <summary>
/// Data Transfer Object cho chi tiết phiếu nhập kho
/// Dùng cho GridControl và truyền dữ liệu giữa Service ↔ WinForms
/// Map với bảng StockInOutDetail trong database
/// </summary>
public class NhapHangThuongMaiDetailDto
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
    /// Số lượng xuất (dùng cho phiếu xuất kho, mặc định = 0 cho phiếu nhập)
    /// Map với: StockInOutDetail.StockOutQty
    /// </summary>
    [DisplayName("SL xuất")]
    [Display(Order = 21)]
    [Range(0, double.MaxValue, ErrorMessage = "Số lượng xuất phải lớn hơn hoặc bằng 0")]
    public decimal StockOutQty { get; set; }

    /// <summary>
    /// Đơn giá
    /// Map với: StockInOutDetail.UnitPrice
    /// </summary>
    [DisplayName("Đơn giá")]
    [Display(Order = 22)]
    [Required(ErrorMessage = "Đơn giá không được để trống")]
    [Range(0, double.MaxValue, ErrorMessage = "Đơn giá phải lớn hơn hoặc bằng 0")]
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Tỷ lệ VAT (%)
    /// Map với: StockInOutDetail.Vat
    /// </summary>
    [DisplayName("VAT (%)")]
    [Display(Order = 23)]
    [Range(0, 100, ErrorMessage = "VAT phải từ 0 đến 100")]
    public decimal Vat { get; set; } = 8;

    /// <summary>
    /// Tổng tiền (chưa VAT) - Computed property
    /// Tính toán: StockInQty * UnitPrice
    /// Map với: StockInOutDetail.TotalAmount (lưu vào DB khi save)
    /// </summary>
    [DisplayName("Tổng tiền")]
    [Display(Order = 25)]
    [Range(0, double.MaxValue, ErrorMessage = "Tổng tiền phải lớn hơn hoặc bằng 0")]
    public decimal TotalAmount => StockInQty * UnitPrice;

    /// <summary>
    /// Số tiền VAT - Computed property
    /// Tính toán: TotalAmount * (Vat / 100)
    /// Map với: StockInOutDetail.VatAmount (lưu vào DB khi save)
    /// </summary>
    [DisplayName("Số tiền VAT")]
    [Display(Order = 24)]
    [Range(0, double.MaxValue, ErrorMessage = "Số tiền VAT phải lớn hơn hoặc bằng 0")]
    public decimal VatAmount => TotalAmount * (Vat / 100);

    /// <summary>
    /// Tổng tiền bao gồm VAT - Computed property
    /// Tính toán: TotalAmount + VatAmount
    /// Map với: StockInOutDetail.TotalAmountIncludedVat (lưu vào DB khi save)
    /// </summary>
    [DisplayName("Tổng tiền gồm VAT")]
    [Display(Order = 26)]
    [Range(0, double.MaxValue, ErrorMessage = "Tổng tiền gồm VAT phải lớn hơn hoặc bằng 0")]
    public decimal TotalAmountIncludedVat => TotalAmount + VatAmount;

    /// <summary>
    /// Ghi chú tình trạng sản phẩm
    /// </summary>
    [DisplayName("Tình trạng")]
    [Display(Order = 30)]
    public string GhiChu { get; set; } = "Bình thường";

    /// <summary>
    /// Danh sách thông tin bảo hành cho sản phẩm này
    /// </summary>
    [DisplayName("Thông tin bảo hành")]
    [Display(Order = 31)]
    public List<WarrantyDto> Warranties { get; set; } = [];

    /// <summary>
    /// Danh sách thông tin định danh thiết bị (Device) cho sản phẩm này
    /// </summary>
    [DisplayName("Thông tin định danh thiết bị")]
    [Display(Order = 32)]
    public List<DeviceDto> Devices { get; set; } = [];

    /// <summary>
    /// Thông tin chi tiết phiếu nhập dưới dạng HTML theo format DevExpress
    /// Sử dụng các tag HTML chuẩn của DevExpress: &lt;b&gt;, &lt;i&gt;, &lt;color&gt;, &lt;size&gt;
    /// Tham khảo: https://docs.devexpress.com/WindowsForms/4874/common-features/html-text-formatting
    /// </summary>
    [DisplayName("Thông tin HTML")]
    [Display(Order = 33)]
    [Description("Thông tin chi tiết phiếu nhập dưới dạng HTML")]
    public string FullNameHtml
    {
        get
        {
            var productVariantName = ProductVariantName ?? string.Empty;
            var productVariantCode = ProductVariantCode ?? string.Empty;
            var unitName = UnitOfMeasureName ?? string.Empty;
            var unitCode = UnitOfMeasureCode ?? string.Empty;

            // Format chuyên nghiệp với visual hierarchy rõ ràng (chỉ sử dụng color, không dùng size và weight)
            // - Tên sản phẩm: màu xanh đậm (primary)
            // - Mã sản phẩm: màu xám
            // - Đơn vị tính: màu xám cho label, đen cho value
            // - Số lượng và giá: màu xám cho label, đen cho value

            var html = string.Empty;

            // Tên sản phẩm (nổi bật nhất)
            if (!string.IsNullOrWhiteSpace(productVariantName))
            {
                html += $"<color='blue'>{productVariantName}</color>";
            }

            // Mã sản phẩm (nếu có)
            if (!string.IsNullOrWhiteSpace(productVariantCode))
            {
                if (!string.IsNullOrWhiteSpace(productVariantName))
                {
                    html += $" <color='#757575'>({productVariantCode})</color>";
                }
                else
                {
                    html += $"<color='blue'>{productVariantCode}</color>";
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

                html += $"<color='#757575'>Đơn vị tính:</color> <color='#212121'>{unitDisplay}</color><br>";
            }

            // Số lượng nhập
            if (StockInQty > 0)
            {
                html += $"<color='#757575'>Số lượng:</color> <color='#212121'>{StockInQty:N2}</color>";
            }

            // Đơn giá
            if (UnitPrice > 0)
            {
                if (StockInQty > 0)
                    html += " | ";
                html += $"<color='#757575'>Đơn giá:</color> <color='#212121'>{UnitPrice:N0}</color>";
            }

            // VAT
            if (Vat > 0)
            {
                if (StockInQty > 0 || UnitPrice > 0)
                    html += " | ";
                html += $"<color='#757575'>VAT:</color> <color='#212121'>{Vat}%</color>";
            }

            if (StockInQty > 0 || UnitPrice > 0 || Vat > 0)
            {
                html += "<br>";
            }

            // Tổng tiền
            if (TotalAmountIncludedVat > 0)
            {
                html += $"<color='#757575'>Tổng tiền:</color> <color='#2196F3'>{TotalAmountIncludedVat:N0}</color>";
            }

            return html;
        }
    }

    #endregion
}

/// <summary>
/// Converter giữa StockInOutDetail entity và NhapHangThuongMaiDetailDto
/// </summary>
public static class StockInDetailDtoConverter
{
    #region Entity to DTO

    /// <summary>
    /// Chuyển đổi StockInOutDetail entity thành NhapHangThuongMaiDetailDto
    /// </summary>
    /// <param name="entity">StockInOutDetail entity</param>
    /// <returns>NhapHangThuongMaiDetailDto</returns>
    public static NhapHangThuongMaiDetailDto ToDto(this Dal.DataContext.StockInOutDetail entity)
    {
        if (entity == null) return null;

        var dto = new NhapHangThuongMaiDetailDto
        {
            Id = entity.Id,
            StockInOutMasterId = entity.StockInOutMasterId,
            ProductVariantId = entity.ProductVariantId,
            ProductVariantCode = entity.ProductVariant?.VariantCode ?? string.Empty,
            StockInQty = entity.StockInQty,
            StockOutQty = entity.StockOutQty,
            UnitPrice = entity.UnitPrice,
            Vat = entity.Vat,
            LineNumber = 0, // Sẽ được cập nhật sau nếu cần
            GhiChu = entity.GhiChu ?? string.Empty
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
    /// Chuyển đổi danh sách StockInOutDetail entities thành danh sách NhapHangThuongMaiDetailDto
    /// </summary>
    /// <param name="entities">Danh sách StockInOutDetail entities</param>
    /// <returns>Danh sách NhapHangThuongMaiDetailDto</returns>
    public static List<NhapHangThuongMaiDetailDto> ToDtoList(this IEnumerable<Dal.DataContext.StockInOutDetail> entities)
    {
        if (entities == null) return [];

        return entities.Select(entity => entity.ToDto()).ToList();
    }

    #endregion
}