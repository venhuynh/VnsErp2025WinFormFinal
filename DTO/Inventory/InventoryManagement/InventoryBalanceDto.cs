using Dal.DataContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace DTO.Inventory.InventoryManagement;

/// <summary>
/// Data Transfer Object cho tồn kho theo tháng
/// Dùng cho Query và truyền dữ liệu giữa Service ↔ WinForms
/// Map với bảng InventoryBalance trong database
/// </summary>
public class InventoryBalanceDto
{
    #region Properties - Thông tin cơ bản

    /// <summary>
    /// ID duy nhất của tồn kho
    /// Map với: InventoryBalance.Id
    /// </summary>
    [DisplayName("ID")]
    [Display(Order = -1)]
    public Guid Id { get; set; }

    /// <summary>
    /// ID kho (Warehouse)
    /// Map với: InventoryBalance.WarehouseId
    /// </summary>
    [DisplayName("ID Kho")]
    [Display(Order = 1)]
    [Required(ErrorMessage = "ID kho không được để trống")]
    public Guid WarehouseId { get; set; }

    /// <summary>
    /// Tên kho
    /// Map với: CompanyBranch.BranchName
    /// </summary>
    [DisplayName("Tên kho")]
    [Display(Order = 2)]
    [StringLength(255, ErrorMessage = "Tên kho không được vượt quá 255 ký tự")]
    public string WarehouseName { get; set; }

    /// <summary>
    /// Mã kho
    /// Map với: CompanyBranch.BranchCode
    /// </summary>
    [DisplayName("Mã kho")]
    [Display(Order = 3)]
    [StringLength(50, ErrorMessage = "Mã kho không được vượt quá 50 ký tự")]
    public string WarehouseCode { get; set; }

    /// <summary>
    /// ID biến thể sản phẩm
    /// Map với: InventoryBalance.ProductVariantId
    /// </summary>
    [DisplayName("ID Sản phẩm")]
    [Display(Order = 4)]
    [Required(ErrorMessage = "ID sản phẩm không được để trống")]
    public Guid ProductVariantId { get; set; }

    /// <summary>
    /// Tên sản phẩm
    /// Map với: ProductVariant.VariantFullName hoặc ProductService.Name
    /// </summary>
    [DisplayName("Tên sản phẩm")]
    [Display(Order = 5)]
    [StringLength(500, ErrorMessage = "Tên sản phẩm không được vượt quá 500 ký tự")]
    public string ProductName { get; set; }

    /// <summary>
    /// Mã sản phẩm/dịch vụ (từ ProductService)
    /// Map với: ProductService.Code
    /// </summary>
    [DisplayName("Mã sản phẩm")]
    [Display(Order = 6)]
    [StringLength(50, ErrorMessage = "Mã sản phẩm không được vượt quá 50 ký tự")]
    public string ProductCode { get; set; }

    /// <summary>
    /// Mã biến thể sản phẩm
    /// Map với: ProductVariant.VariantCode
    /// </summary>
    [DisplayName("Mã biến thể")]
    [Display(Order = 7)]
    [StringLength(50, ErrorMessage = "Mã biến thể không được vượt quá 50 ký tự")]
    public string ProductVariantCode { get; set; }

    /// <summary>
    /// Tên biến thể sản phẩm đầy đủ
    /// Map với: ProductVariant.VariantFullName
    /// </summary>
    [DisplayName("Tên biến thể")]
    [Display(Order = 8)]
    [StringLength(500, ErrorMessage = "Tên biến thể không được vượt quá 500 ký tự")]
    public string ProductVariantFullName { get; set; }

    /// <summary>
    /// ID đơn vị tính
    /// Map với: ProductVariant.UnitId
    /// </summary>
    [DisplayName("ID ĐVT")]
    [Display(Order = 9)]
    public Guid? UnitOfMeasureId { get; set; }

    /// <summary>
    /// Mã đơn vị tính
    /// Map với: UnitOfMeasure.Code
    /// </summary>
    [DisplayName("Mã ĐVT")]
    [Display(Order = 10)]
    [StringLength(50, ErrorMessage = "Mã đơn vị tính không được vượt quá 50 ký tự")]
    public string UnitOfMeasureCode { get; set; }

    /// <summary>
    /// Tên đơn vị tính
    /// Map với: UnitOfMeasure.Name
    /// </summary>
    [DisplayName("Đơn vị tính")]
    [Display(Order = 11)]
    [StringLength(100, ErrorMessage = "Tên đơn vị tính không được vượt quá 100 ký tự")]
    public string UnitOfMeasureName { get; set; }

    /// <summary>
    /// Năm kỳ
    /// Map với: InventoryBalance.PeriodYear
    /// </summary>
    [DisplayName("Năm")]
    [Display(Order = 12)]
    [Range(2000, 9999, ErrorMessage = "Năm phải trong khoảng 2000-9999")]
    public int PeriodYear { get; set; }

    /// <summary>
    /// Tháng kỳ (1-12)
    /// Map với: InventoryBalance.PeriodMonth
    /// </summary>
    [DisplayName("Tháng")]
    [Display(Order = 13)]
    [Range(1, 12, ErrorMessage = "Tháng phải trong khoảng 1-12")]
    public int PeriodMonth { get; set; }

    /// <summary>
    /// Hiển thị kỳ: "YYYY/MM" hoặc "Tháng MM/YYYY"
    /// </summary>
    [DisplayName("Kỳ")]
    [Display(Order = 14)]
    public string PeriodDisplay
    {
        get
        {
            if (PeriodYear > 0 && PeriodMonth > 0)
                return $"{PeriodYear}/{PeriodMonth:D2}";
            return string.Empty;
        }
    }

    /// <summary>
    /// Hiển thị kỳ đầy đủ: "Tháng MM/YYYY"
    /// </summary>
    [DisplayName("Kỳ (đầy đủ)")]
    [Display(Order = 15)]
    public string PeriodFullDisplay
    {
        get
        {
            if (PeriodYear > 0 && PeriodMonth > 0)
                return $"Tháng {PeriodMonth:D2}/{PeriodYear}";
            return string.Empty;
        }
    }

    /// <summary>
    /// Thông tin kỳ dưới dạng HTML theo format DevExpress
    /// Sử dụng các tag HTML chuẩn của DevExpress: &lt;b&gt;, &lt;i&gt;, &lt;color&gt;, &lt;size&gt;
    /// Tham khảo: https://docs.devexpress.com/WindowsForms/4874/common-features/html-text-formatting
    /// Format tương tự ProductVariantListDto.FullNameHtml để đảm bảo consistency
    /// </summary>
    [DisplayName("Kỳ HTML")]
    [Display(Order = 16)]
    [Description("Thông tin kỳ dưới dạng HTML")]
    public string PeriodHtml
    {
        get
        {
            if (PeriodYear <= 0 || PeriodMonth <= 0)
                return string.Empty;

            // Format chuyên nghiệp với visual hierarchy rõ ràng (tham khảo ProductVariantListDto.FullNameHtml)
            // - Kỳ chính: font lớn (12), bold, màu xanh đậm (primary)
            // - Thông tin chi tiết: font nhỏ hơn (9), màu xám
            var html = $"<size=12><color='#757575'>Tháng {PeriodMonth:D2}/{PeriodYear}</color></size>";
            return html;
        }
    }

    /// <summary>
    /// Thông tin kho dưới dạng HTML theo format DevExpress
    /// Sử dụng các tag HTML chuẩn của DevExpress: &lt;b&gt;, &lt;i&gt;, &lt;color&gt;, &lt;size&gt;
    /// Tham khảo: https://docs.devexpress.com/WindowsForms/4874/common-features/html-text-formatting
    /// Format tương tự ProductVariantListDto.FullNameHtml để đảm bảo consistency
    /// </summary>
    [DisplayName("Thông tin kho HTML")]
    [Display(Order = 17)]
    [Description("Thông tin kho dưới dạng HTML")]
    public string WarehouseHtml
    {
        get
        {
            var warehouseName = WarehouseName ?? string.Empty;
            var warehouseCode = WarehouseCode ?? string.Empty;

            if (string.IsNullOrWhiteSpace(warehouseName) && string.IsNullOrWhiteSpace(warehouseCode))
                return string.Empty;

            // Format chuyên nghiệp với visual hierarchy rõ ràng (tham khảo ProductVariantListDto.FullNameHtml)
            // - Tên kho: font lớn (12), bold, màu xanh đậm (primary)
            // - Mã kho: font nhỏ hơn (9), màu xám
            var html = $"<size=12><b><color='blue'>{warehouseName}</color></b></size>";

            if (!string.IsNullOrWhiteSpace(warehouseCode))
            {
                html += $" <size=9><color='#757575'>({warehouseCode})</color></size>";
            }

            return html;
        }
    }

    /// <summary>
    /// Thông tin biến thể sản phẩm dưới dạng HTML theo format DevExpress
    /// Sử dụng các tag HTML chuẩn của DevExpress: &lt;b&gt;, &lt;i&gt;, &lt;color&gt;, &lt;size&gt;
    /// Tham khảo: https://docs.devexpress.com/WindowsForms/4874/common-features/html-text-formatting
    /// Format tương tự StockInOutProductHistoryDto.ProductVariantFullNameHtml để đảm bảo consistency
    /// </summary>
    [DisplayName("Thông tin sản phẩm HTML")]
    [Display(Order = 18)]
    [Description("Thông tin biến thể sản phẩm dưới dạng HTML")]
    public string ProductHtml
    {
        get
        {
            var productName = ProductName ?? string.Empty;
            var productCode = ProductCode ?? string.Empty;
            var variantCode = ProductVariantCode ?? string.Empty;
            var variantFullName = ProductVariantFullName ?? string.Empty;
            var unitName = UnitOfMeasureName ?? UnitOfMeasureCode ?? string.Empty;

            // Format chuyên nghiệp với visual hierarchy rõ ràng (tham khảo StockInOutProductHistoryDto.ProductVariantFullNameHtml)
            // - Tên sản phẩm: font lớn, bold, màu xanh đậm (primary)
            // - Mã sản phẩm: font nhỏ hơn, màu xám
            // - Mã biến thể: font nhỏ hơn, màu cam (#FF9800)
            // - Tên biến thể đầy đủ: font nhỏ hơn, màu xám cho label, đen cho value
            // - Đơn vị tính: font nhỏ hơn, màu xám cho label, đen cho value

            var html = $"<size=12><b><color='blue'>{productName}</color></b></size>";

            if (!string.IsNullOrWhiteSpace(productCode))
            {
                html += $" <size=9><color='#757575'>({productCode})</color></size>";
            }

            html += "<br>";

            if (!string.IsNullOrWhiteSpace(variantCode))
            {
                html += $"<size=9><color='#757575'>Mã biến thể:</color></size> <size=10><color='#FF9800'><b>{variantCode}</b></color></size>";
            }

            if (!string.IsNullOrWhiteSpace(variantFullName))
            {
                if (!string.IsNullOrWhiteSpace(variantCode))
                    html += "<br/>";
                html += $"<size=9><color='#757575'>Tên biến thể:</color></size> <size=10><color='#212121'><b>{variantFullName}</b></color></size>";
            }

            if (!string.IsNullOrWhiteSpace(variantCode) || !string.IsNullOrWhiteSpace(variantFullName))
            {
                html += "<br>";
            }

            if (!string.IsNullOrWhiteSpace(unitName))
            {
                html += $"<size=9><color='#757575'>Đơn vị tính:</color></size> <size=10><color='#212121'><b>{unitName}</b></color></size>";
            }

            return html;
        }
    }

    /// <summary>
    /// Trạng thái khóa dưới dạng HTML theo format DevExpress
    /// Sử dụng các tag HTML chuẩn của DevExpress: &lt;b&gt;, &lt;i&gt;, &lt;color&gt;, &lt;size&gt;
    /// Tham khảo: https://docs.devexpress.com/WindowsForms/4874/common-features/html-text-formatting
    /// Format tương tự ProductVariantListDto.FullNameHtml để đảm bảo consistency
    /// </summary>
    [DisplayName("Trạng thái HTML")]
    [Display(Order = 19)]
    [Description("Trạng thái khóa dưới dạng HTML")]
    public string StatusHtml
    {
        get
        {
            var statusText = IsLocked ? "Khóa" : "Không khóa";
            var statusColor = IsLocked ? "#FF9800" : "#4CAF50"; // Cam cho khóa, xanh lá cho không khóa

            // Format chuyên nghiệp với visual hierarchy rõ ràng (tham khảo ProductVariantListDto.FullNameHtml)
            // - Label: font nhỏ (9), màu xám
            // - Value: font lớn hơn (10), bold, màu theo trạng thái
            var html = $"<size=9><color='#757575'>Trạng thái:</color></size> <size=10><color='{statusColor}'><b>{statusText}</b></color></size>";

            // Thêm thông tin người khóa và ngày khóa nếu đã khóa
            if (IsLocked && LockedDate.HasValue)
            {
                html += "<br>";
                var lockedDateStr = LockedDate.Value.ToString("dd/MM/yyyy");
                html += $"<size=9><color='#757575'>Ngày khóa:</color></size> <size=9><color='#212121'><b>{lockedDateStr}</b></color></size>";
                
                if (!string.IsNullOrWhiteSpace(LockedByName))
                {
                    html += "<br>";
                    html += $"<size=9><color='#757575'>Người khóa:</color></size> <size=9><color='#212121'><b>{LockedByName}</b></color></size>";
                }
            }

            return html;
        }
    }

    #endregion

    #region Properties - Thông tin tồn kho (số lượng)

    /// <summary>
    /// Tồn đầu kỳ (số lượng)
    /// Map với: InventoryBalance.OpeningBalance
    /// </summary>
    [DisplayName("Tồn đầu kỳ")]
    [Display(Order = 20)]
    [Required(ErrorMessage = "Tồn đầu kỳ không được để trống")]
    public decimal OpeningBalance { get; set; }

    /// <summary>
    /// Tổng nhập trong kỳ (số lượng)
    /// Map với: InventoryBalance.TotalInQty
    /// </summary>
    [DisplayName("Tổng nhập")]
    [Display(Order = 21)]
    [Required(ErrorMessage = "Tổng nhập không được để trống")]
    public decimal TotalInQty { get; set; }

    /// <summary>
    /// Tổng xuất trong kỳ (số lượng)
    /// Map với: InventoryBalance.TotalOutQty
    /// </summary>
    [DisplayName("Tổng xuất")]
    [Display(Order = 22)]
    [Required(ErrorMessage = "Tổng xuất không được để trống")]
    public decimal TotalOutQty { get; set; }

    /// <summary>
    /// Tồn cuối kỳ (số lượng) = OpeningBalance + TotalInQty - TotalOutQty
    /// Map với: InventoryBalance.ClosingBalance
    /// </summary>
    [DisplayName("Tồn cuối kỳ")]
    [Display(Order = 23)]
    [Required(ErrorMessage = "Tồn cuối kỳ không được để trống")]
    public decimal ClosingBalance { get; set; }

    #endregion

    #region Properties - Thông tin giá trị (chưa VAT)

    /// <summary>
    /// Giá trị tồn đầu kỳ
    /// Map với: InventoryBalance.OpeningValue
    /// </summary>
    [DisplayName("Giá trị tồn đầu kỳ")]
    [Display(Order = 30)]
    public decimal? OpeningValue { get; set; }

    /// <summary>
    /// Tổng giá trị nhập (chưa VAT)
    /// Map với: InventoryBalance.TotalInValue
    /// </summary>
    [DisplayName("Tổng giá trị nhập")]
    [Display(Order = 31)]
    public decimal? TotalInValue { get; set; }

    /// <summary>
    /// Tổng giá trị xuất (chưa VAT)
    /// Map với: InventoryBalance.TotalOutValue
    /// </summary>
    [DisplayName("Tổng giá trị xuất")]
    [Display(Order = 32)]
    public decimal? TotalOutValue { get; set; }

    /// <summary>
    /// Giá trị tồn cuối kỳ
    /// Map với: InventoryBalance.ClosingValue
    /// </summary>
    [DisplayName("Giá trị tồn cuối kỳ")]
    [Display(Order = 33)]
    public decimal? ClosingValue { get; set; }

    #endregion

    #region Properties - Thông tin VAT

    /// <summary>
    /// Tổng tiền VAT nhập
    /// Map với: InventoryBalance.TotalInVatAmount
    /// </summary>
    [DisplayName("Tổng VAT nhập")]
    [Display(Order = 40)]
    public decimal? TotalInVatAmount { get; set; }

    /// <summary>
    /// Tổng tiền VAT xuất
    /// Map với: InventoryBalance.TotalOutVatAmount
    /// </summary>
    [DisplayName("Tổng VAT xuất")]
    [Display(Order = 41)]
    public decimal? TotalOutVatAmount { get; set; }

    /// <summary>
    /// Tổng tiền nhập (có VAT) = TotalInValue + TotalInVatAmount
    /// Map với: InventoryBalance.TotalInAmountIncludedVat
    /// </summary>
    [DisplayName("Tổng tiền nhập (có VAT)")]
    [Display(Order = 42)]
    public decimal? TotalInAmountIncludedVat { get; set; }

    /// <summary>
    /// Tổng tiền xuất (có VAT) = TotalOutValue + TotalOutVatAmount
    /// Map với: InventoryBalance.TotalOutAmountIncludedVat
    /// </summary>
    [DisplayName("Tổng tiền xuất (có VAT)")]
    [Display(Order = 43)]
    public decimal? TotalOutAmountIncludedVat { get; set; }

    #endregion

    #region Properties - Trạng thái khóa

    /// <summary>
    /// Đã khóa chưa (không cho phép chỉnh sửa)
    /// Map với: InventoryBalance.IsLocked
    /// </summary>
    [DisplayName("Đã khóa")]
    [Display(Order = 50)]
    public bool IsLocked { get; set; }

    /// <summary>
    /// Ngày khóa
    /// Map với: InventoryBalance.LockedDate
    /// </summary>
    [DisplayName("Ngày khóa")]
    [Display(Order = 51)]
    public DateTime? LockedDate { get; set; }

    /// <summary>
    /// ID người khóa
    /// Map với: InventoryBalance.LockedBy
    /// </summary>
    [DisplayName("ID Người khóa")]
    [Display(Order = 52)]
    public Guid? LockedBy { get; set; }

    /// <summary>
    /// Tên người khóa
    /// Map với: ApplicationUser.UserName
    /// </summary>
    [DisplayName("Người khóa")]
    [Display(Order = 53)]
    [StringLength(50, ErrorMessage = "Tên người khóa không được vượt quá 50 ký tự")]
    public string LockedByName { get; set; }

    /// <summary>
    /// Lý do khóa
    /// Map với: InventoryBalance.LockReason
    /// </summary>
    [DisplayName("Lý do khóa")]
    [Display(Order = 54)]
    [StringLength(500, ErrorMessage = "Lý do khóa không được vượt quá 500 ký tự")]
    public string LockReason { get; set; }

    #endregion

    #region Properties - Trạng thái xác thực

    /// <summary>
    /// Đã xác thực chưa
    /// Map với: InventoryBalance.IsVerified
    /// </summary>
    [DisplayName("Đã xác thực")]
    [Display(Order = 60)]
    public bool IsVerified { get; set; }

    /// <summary>
    /// Ngày xác thực
    /// Map với: InventoryBalance.VerifiedDate
    /// </summary>
    [DisplayName("Ngày xác thực")]
    [Display(Order = 61)]
    public DateTime? VerifiedDate { get; set; }

    /// <summary>
    /// ID người xác thực
    /// Map với: InventoryBalance.VerifiedBy
    /// </summary>
    [DisplayName("ID Người xác thực")]
    [Display(Order = 62)]
    public Guid? VerifiedBy { get; set; }

    /// <summary>
    /// Tên người xác thực
    /// Map với: ApplicationUser.UserName
    /// </summary>
    [DisplayName("Người xác thực")]
    [Display(Order = 63)]
    [StringLength(50, ErrorMessage = "Tên người xác thực không được vượt quá 50 ký tự")]
    public string VerifiedByName { get; set; }

    /// <summary>
    /// Ghi chú xác thực
    /// Map với: InventoryBalance.VerificationNotes
    /// </summary>
    [DisplayName("Ghi chú xác thực")]
    [Display(Order = 64)]
    [StringLength(1000, ErrorMessage = "Ghi chú xác thực không được vượt quá 1000 ký tự")]
    public string VerificationNotes { get; set; }

    #endregion

    #region Properties - Trạng thái phê duyệt

    /// <summary>
    /// Đã phê duyệt chưa
    /// Map với: InventoryBalance.IsApproved
    /// </summary>
    [DisplayName("Đã phê duyệt")]
    [Display(Order = 70)]
    public bool IsApproved { get; set; }

    /// <summary>
    /// Ngày phê duyệt
    /// Map với: InventoryBalance.ApprovedDate
    /// </summary>
    [DisplayName("Ngày phê duyệt")]
    [Display(Order = 71)]
    public DateTime? ApprovedDate { get; set; }

    /// <summary>
    /// ID người phê duyệt
    /// Map với: InventoryBalance.ApprovedBy
    /// </summary>
    [DisplayName("ID Người phê duyệt")]
    [Display(Order = 72)]
    public Guid? ApprovedBy { get; set; }

    /// <summary>
    /// Tên người phê duyệt
    /// Map với: ApplicationUser.UserName
    /// </summary>
    [DisplayName("Người phê duyệt")]
    [Display(Order = 73)]
    [StringLength(50, ErrorMessage = "Tên người phê duyệt không được vượt quá 50 ký tự")]
    public string ApprovedByName { get; set; }

    /// <summary>
    /// Ghi chú phê duyệt
    /// Map với: InventoryBalance.ApprovalNotes
    /// </summary>
    [DisplayName("Ghi chú phê duyệt")]
    [Display(Order = 74)]
    [StringLength(1000, ErrorMessage = "Ghi chú phê duyệt không được vượt quá 1000 ký tự")]
    public string ApprovalNotes { get; set; }

    #endregion

    #region Properties - Trạng thái tổng quát

    /// <summary>
    /// Trạng thái (0: Draft, 1: Locked, 2: Verified, 3: Approved, 4: Rejected)
    /// Map với: InventoryBalance.Status
    /// </summary>
    [DisplayName("Trạng thái")]
    [Display(Order = 80)]
    public int Status { get; set; }

    /// <summary>
    /// Tên trạng thái (hiển thị)
    /// </summary>
    [DisplayName("Trạng thái (text)")]
    [Display(Order = 81)]
    public string StatusText
    {
        get
        {
            return Status switch
            {
                0 => "Nháp",
                1 => "Đã khóa",
                2 => "Đã xác thực",
                3 => "Đã phê duyệt",
                4 => "Đã từ chối",
                _ => "Không xác định"
            };
        }
    }

    /// <summary>
    /// Ghi chú chung
    /// Map với: InventoryBalance.Notes
    /// </summary>
    [DisplayName("Ghi chú")]
    [Display(Order = 82)]
    [StringLength(1000, ErrorMessage = "Ghi chú không được vượt quá 1000 ký tự")]
    public string Notes { get; set; }

    #endregion

    #region Properties - Display và Grouping

    /// <summary>
    /// Caption hiển thị trên UI: "Kho - Sản phẩm - Kỳ"
    /// Ví dụ: "Kho A - Sản phẩm X - 2025/12"
    /// </summary>
    [DisplayName("Caption")]
    [Display(Order = 90)]
    public string DisplayCaption
    {
        get
        {
            var parts = new List<string>();
            
            if (!string.IsNullOrWhiteSpace(WarehouseName))
                parts.Add(WarehouseName);
            else if (!string.IsNullOrWhiteSpace(WarehouseCode))
                parts.Add(WarehouseCode);
            
            if (!string.IsNullOrWhiteSpace(ProductName))
                parts.Add(ProductName);
            else if (!string.IsNullOrWhiteSpace(ProductCode))
                parts.Add(ProductCode);
            
            if (!string.IsNullOrWhiteSpace(PeriodDisplay))
                parts.Add(PeriodDisplay);
            
            return parts.Count > 0 ? string.Join(" - ", parts) : Id.ToString();
        }
    }

    /// <summary>
    /// Group caption để nhóm trong UI: "Kỳ - Kho"
    /// Ví dụ: "2025/12 - Kho A"
    /// </summary>
    [DisplayName("Group Caption")]
    [Display(Order = 91)]
    public string GroupCaption
    {
        get
        {
            var parts = new List<string>();
            
            if (!string.IsNullOrWhiteSpace(PeriodDisplay))
                parts.Add(PeriodDisplay);
            
            if (!string.IsNullOrWhiteSpace(WarehouseName))
                parts.Add(WarehouseName);
            else if (!string.IsNullOrWhiteSpace(WarehouseCode))
                parts.Add(WarehouseCode);
            
            return parts.Count > 0 ? string.Join(" - ", parts) : PeriodDisplay ?? string.Empty;
        }
    }

    #endregion

    #region Properties - Audit

    /// <summary>
    /// Trạng thái hoạt động
    /// Map với: InventoryBalance.IsActive
    /// </summary>
    [DisplayName("Hoạt động")]
    [Display(Order = 100)]
    public bool IsActive { get; set; }

    /// <summary>
    /// Đã xóa chưa
    /// Map với: InventoryBalance.IsDeleted
    /// </summary>
    [DisplayName("Đã xóa")]
    [Display(Order = 101)]
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Ngày tạo
    /// Map với: InventoryBalance.CreateDate
    /// </summary>
    [DisplayName("Ngày tạo")]
    [Display(Order = 102)]
    public DateTime CreateDate { get; set; }

    /// <summary>
    /// ID người tạo
    /// Map với: InventoryBalance.CreateBy
    /// </summary>
    [DisplayName("ID Người tạo")]
    [Display(Order = 103)]
    public Guid CreateBy { get; set; }

    /// <summary>
    /// Tên người tạo
    /// Map với: ApplicationUser.UserName
    /// </summary>
    [DisplayName("Người tạo")]
    [Display(Order = 104)]
    [StringLength(50, ErrorMessage = "Tên người tạo không được vượt quá 50 ký tự")]
    public string CreatedByName { get; set; }

    /// <summary>
    /// Ngày sửa
    /// Map với: InventoryBalance.ModifiedDate
    /// </summary>
    [DisplayName("Ngày sửa")]
    [Display(Order = 105)]
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// ID người sửa
    /// Map với: InventoryBalance.ModifiedBy
    /// </summary>
    [DisplayName("ID Người sửa")]
    [Display(Order = 106)]
    public Guid? ModifiedBy { get; set; }

    /// <summary>
    /// Tên người sửa
    /// Map với: ApplicationUser.UserName
    /// </summary>
    [DisplayName("Người sửa")]
    [Display(Order = 107)]
    [StringLength(50, ErrorMessage = "Tên người sửa không được vượt quá 50 ký tự")]
    public string ModifiedByName { get; set; }

    /// <summary>
    /// Ngày xóa
    /// Map với: InventoryBalance.DeletedDate
    /// </summary>
    [DisplayName("Ngày xóa")]
    [Display(Order = 108)]
    public DateTime? DeletedDate { get; set; }

    /// <summary>
    /// ID người xóa
    /// Map với: InventoryBalance.DeletedBy
    /// </summary>
    [DisplayName("ID Người xóa")]
    [Display(Order = 109)]
    public Guid? DeletedBy { get; set; }

    /// <summary>
    /// Tên người xóa
    /// Map với: ApplicationUser.UserName
    /// </summary>
    [DisplayName("Người xóa")]
    [Display(Order = 110)]
    [StringLength(50, ErrorMessage = "Tên người xóa không được vượt quá 50 ký tự")]
    public string DeletedByName { get; set; }

    #endregion
}
