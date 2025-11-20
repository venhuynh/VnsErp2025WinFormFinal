using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DTO.Inventory.StockIn;

/// <summary>
/// Data Transfer Object cho chi tiết phiếu nhập kho
/// Dùng cho GridControl và truyền dữ liệu giữa Service ↔ WinForms
/// Map với bảng StockInOutDetail trong database
/// </summary>
public class StockInDetailDto
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

    #endregion
}

/// <summary>
/// Converter giữa StockInOutDetail entity và StockInDetailDto
/// </summary>
public static class StockInDetailDtoConverter
{
    #region Entity to DTO

    /// <summary>
    /// Chuyển đổi StockInOutDetail entity thành StockInDetailDto
    /// </summary>
    /// <param name="entity">StockInOutDetail entity</param>
    /// <returns>StockInDetailDto</returns>
    public static StockInDetailDto ToDto(this Dal.DataContext.StockInOutDetail entity)
    {
        if (entity == null) return null;

        var dto = new StockInDetailDto
        {
            Id = entity.Id,
            StockInOutMasterId = entity.StockInOutMasterId,
            ProductVariantId = entity.ProductVariantId,
            StockInQty = entity.StockInQty,
            StockOutQty = entity.StockOutQty,
            UnitPrice = entity.UnitPrice,
            Vat = entity.Vat,
            LineNumber = 0 // Sẽ được cập nhật sau nếu cần
        };

        // Lấy thông tin ProductVariant nếu có
        if (entity.ProductVariant != null)
        {
            dto.ProductVariantCode = entity.ProductVariant.VariantCode;

            // Lấy tên tương tự như FullName của ProductVariantDto
            // Format: ProductName (ProductCode) - VariantCode | VariantFullName - UnitName
            var productName = entity.ProductVariant.ProductService?.Name ?? string.Empty;
            var productCode = entity.ProductVariant.ProductService?.Code ?? string.Empty;
            var variantCode = entity.ProductVariant.VariantCode ?? string.Empty;
            var variantFullName = entity.ProductVariant.VariantFullName ?? string.Empty;
            var unitName = entity.ProductVariant.UnitOfMeasure?.Name ?? string.Empty;

            // Tạo tên hiển thị tương tự FullName của ProductVariantListDto
            var nameParts = new List<string>();

            // Phần 1: Tên sản phẩm (và mã nếu có)
            if (!string.IsNullOrWhiteSpace(productName))
            {
                if (!string.IsNullOrWhiteSpace(productCode))
                {
                    nameParts.Add($"{productName} ({productCode})");
                }
                else
                {
                    nameParts.Add(productName);
                }
            }

            // Phần 2: Mã biến thể và tên biến thể
            var variantParts = new List<string>();
            if (!string.IsNullOrWhiteSpace(variantCode))
            {
                variantParts.Add(variantCode);
            }
            if (!string.IsNullOrWhiteSpace(variantFullName))
            {
                variantParts.Add(variantFullName);
            }
            
            if (variantParts.Count > 0)
            {
                nameParts.Add(string.Join(" | ", variantParts));
            }

            // Phần 3: Đơn vị tính
            if (!string.IsNullOrWhiteSpace(unitName))
            {
                nameParts.Add($"({unitName})");
            }

            // Kết hợp tất cả các phần
            if (nameParts.Count > 0)
            {
                dto.ProductVariantName = string.Join(" - ", nameParts);
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
    /// Chuyển đổi danh sách StockInOutDetail entities thành danh sách StockInDetailDto
    /// </summary>
    /// <param name="entities">Danh sách StockInOutDetail entities</param>
    /// <returns>Danh sách StockInDetailDto</returns>
    public static List<StockInDetailDto> ToDtoList(this IEnumerable<Dal.DataContext.StockInOutDetail> entities)
    {
        if (entities == null) return [];

        return entities.Select(entity => entity.ToDto()).ToList();
    }

    #endregion
}