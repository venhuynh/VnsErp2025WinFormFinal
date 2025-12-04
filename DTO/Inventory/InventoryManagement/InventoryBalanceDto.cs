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
    /// Mã sản phẩm
    /// Map với: ProductVariant.VariantCode
    /// </summary>
    [DisplayName("Mã sản phẩm")]
    [Display(Order = 6)]
    [StringLength(50, ErrorMessage = "Mã sản phẩm không được vượt quá 50 ký tự")]
    public string ProductCode { get; set; }

    /// <summary>
    /// Năm kỳ
    /// Map với: InventoryBalance.PeriodYear
    /// </summary>
    [DisplayName("Năm")]
    [Display(Order = 7)]
    [Range(2000, 9999, ErrorMessage = "Năm phải trong khoảng 2000-9999")]
    public int PeriodYear { get; set; }

    /// <summary>
    /// Tháng kỳ (1-12)
    /// Map với: InventoryBalance.PeriodMonth
    /// </summary>
    [DisplayName("Tháng")]
    [Display(Order = 8)]
    [Range(1, 12, ErrorMessage = "Tháng phải trong khoảng 1-12")]
    public int PeriodMonth { get; set; }

    /// <summary>
    /// Hiển thị kỳ: "YYYY/MM" hoặc "Tháng MM/YYYY"
    /// </summary>
    [DisplayName("Kỳ")]
    [Display(Order = 9)]
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
    [Display(Order = 10)]
    public string PeriodFullDisplay
    {
        get
        {
            if (PeriodYear > 0 && PeriodMonth > 0)
                return $"Tháng {PeriodMonth:D2}/{PeriodYear}";
            return string.Empty;
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

/// <summary>
/// Converter giữa InventoryBalance entity và InventoryBalanceDto
/// </summary>
public static class InventoryBalanceDtoConverter
{
    #region Entity to DTO

    /// <summary>
    /// Chuyển đổi InventoryBalance entity thành InventoryBalanceDto
    /// Tự động map navigation properties nếu đã được load trong entity
    /// </summary>
    /// <param name="entity">InventoryBalance entity</param>
    /// <returns>InventoryBalanceDto</returns>
    public static InventoryBalanceDto ToDto(this InventoryBalance entity)
    {
        if (entity == null) return null;

        var dto = new InventoryBalanceDto
        {
            Id = entity.Id,
            WarehouseId = entity.WarehouseId,
            ProductVariantId = entity.ProductVariantId,
            PeriodYear = entity.PeriodYear,
            PeriodMonth = entity.PeriodMonth,
            OpeningBalance = entity.OpeningBalance,
            TotalInQty = entity.TotalInQty,
            TotalOutQty = entity.TotalOutQty,
            ClosingBalance = entity.ClosingBalance,
            OpeningValue = entity.OpeningValue,
            TotalInValue = entity.TotalInValue,
            TotalOutValue = entity.TotalOutValue,
            ClosingValue = entity.ClosingValue,
            TotalInVatAmount = entity.TotalInVatAmount,
            TotalOutVatAmount = entity.TotalOutVatAmount,
            TotalInAmountIncludedVat = entity.TotalInAmountIncludedVat,
            TotalOutAmountIncludedVat = entity.TotalOutAmountIncludedVat,
            IsLocked = entity.IsLocked,
            LockedDate = entity.LockedDate,
            LockedBy = entity.LockedBy,
            LockReason = entity.LockReason,
            IsVerified = entity.IsVerified,
            VerifiedDate = entity.VerifiedDate,
            VerifiedBy = entity.VerifiedBy,
            VerificationNotes = entity.VerificationNotes,
            IsApproved = entity.IsApproved,
            ApprovedDate = entity.ApprovedDate,
            ApprovedBy = entity.ApprovedBy,
            ApprovalNotes = entity.ApprovalNotes,
            Status = entity.Status,
            Notes = entity.Notes,
            IsActive = entity.IsActive,
            IsDeleted = entity.IsDeleted,
            CreateDate = entity.CreateDate,
            CreateBy = entity.CreateBy,
            ModifiedDate = entity.ModifiedDate,
            ModifiedBy = entity.ModifiedBy,
            DeletedDate = entity.DeletedDate,
            DeletedBy = entity.DeletedBy
        };

        // Map navigation properties nếu đã được load trong entity
        // Warehouse
        if (entity.CompanyBranch != null)
        {
            dto.WarehouseName = entity.CompanyBranch.BranchName;
            dto.WarehouseCode = entity.CompanyBranch.BranchCode;
        }

        // Product
        if (entity.ProductVariant != null)
        {
            dto.ProductCode = entity.ProductVariant.VariantCode;

            if (entity.ProductVariant.ProductService != null)
            {
                dto.ProductName = entity.ProductVariant.ProductService.Name;
            }
            else
            {
                dto.ProductName = entity.ProductVariant.VariantFullName;
            }
        }

        // User names
        if (entity.ApplicationUser1 != null) // CreateBy
        {
            dto.CreatedByName = entity.ApplicationUser1.UserName;
        }

        if (entity.ApplicationUser4 != null) // ModifiedBy
        {
            dto.ModifiedByName = entity.ApplicationUser4.UserName;
        }

        if (entity.ApplicationUser2 != null) // DeletedBy
        {
            dto.DeletedByName = entity.ApplicationUser2.UserName;
        }

        if (entity.ApplicationUser3 != null) // LockedBy
        {
            dto.LockedByName = entity.ApplicationUser3.UserName;
        }

        if (entity.ApplicationUser5 != null) // VerifiedBy
        {
            dto.VerifiedByName = entity.ApplicationUser5.UserName;
        }

        if (entity.ApplicationUser != null) // ApprovedBy
        {
            dto.ApprovedByName = entity.ApplicationUser.UserName;
        }

        return dto;
    }

    /// <summary>
    /// Chuyển đổi danh sách InventoryBalance entities thành danh sách InventoryBalanceDto
    /// </summary>
    /// <param name="entities">Danh sách InventoryBalance entities</param>
    /// <returns>Danh sách InventoryBalanceDto</returns>
    public static List<InventoryBalanceDto> ToDtoList(this IEnumerable<InventoryBalance> entities)
    {
        if (entities == null) return [];

        List<InventoryBalanceDto> list = [];
        foreach (var entity in entities)
        {
            var dto = entity.ToDto();
            if (dto != null) list.Add(dto);
        }

        return list;
    }

    #endregion

    #region DTO to Entity

    /// <summary>
    /// Chuyển đổi InventoryBalanceDto thành InventoryBalance entity
    /// </summary>
    /// <param name="dto">InventoryBalanceDto</param>
    /// <param name="existingEntity">Entity hiện có (nếu đang update), null nếu tạo mới</param>
    /// <returns>InventoryBalance entity</returns>
    public static InventoryBalance ToEntity(this InventoryBalanceDto dto, InventoryBalance existingEntity = null)
    {
        if (dto == null) return null;

        var entity = existingEntity ?? new InventoryBalance();

        // Chỉ map các properties có thể chỉnh sửa, không map navigation properties
        entity.WarehouseId = dto.WarehouseId;
        entity.ProductVariantId = dto.ProductVariantId;
        entity.PeriodYear = dto.PeriodYear;
        entity.PeriodMonth = dto.PeriodMonth;
        entity.OpeningBalance = dto.OpeningBalance;
        entity.TotalInQty = dto.TotalInQty;
        entity.TotalOutQty = dto.TotalOutQty;
        entity.ClosingBalance = dto.ClosingBalance;
        entity.OpeningValue = dto.OpeningValue;
        entity.TotalInValue = dto.TotalInValue;
        entity.TotalOutValue = dto.TotalOutValue;
        entity.ClosingValue = dto.ClosingValue;
        entity.TotalInVatAmount = dto.TotalInVatAmount;
        entity.TotalOutVatAmount = dto.TotalOutVatAmount;
        entity.TotalInAmountIncludedVat = dto.TotalInAmountIncludedVat;
        entity.TotalOutAmountIncludedVat = dto.TotalOutAmountIncludedVat;
        entity.IsLocked = dto.IsLocked;
        entity.LockedDate = dto.LockedDate;
        entity.LockedBy = dto.LockedBy;
        entity.LockReason = dto.LockReason;
        entity.IsVerified = dto.IsVerified;
        entity.VerifiedDate = dto.VerifiedDate;
        entity.VerifiedBy = dto.VerifiedBy;
        entity.VerificationNotes = dto.VerificationNotes;
        entity.IsApproved = dto.IsApproved;
        entity.ApprovedDate = dto.ApprovedDate;
        entity.ApprovedBy = dto.ApprovedBy;
        entity.ApprovalNotes = dto.ApprovalNotes;
        entity.Status = dto.Status;
        entity.Notes = dto.Notes;
        entity.IsActive = dto.IsActive;
        entity.IsDeleted = dto.IsDeleted;

        // Chỉ set ID và audit fields nếu là entity mới
        if (existingEntity == null)
        {
            entity.Id = dto.Id != Guid.Empty ? dto.Id : Guid.NewGuid();
            entity.CreateDate = DateTime.Now;
            entity.CreateBy = dto.CreateBy;
        }
        else
        {
            // Update: chỉ cập nhật ModifiedDate và ModifiedBy (sẽ được BLL set)
            // Không thay đổi CreateDate và CreateBy
        }

        // ModifiedDate và ModifiedBy sẽ được BLL set khi save
        entity.ModifiedDate = dto.ModifiedDate;
        entity.ModifiedBy = dto.ModifiedBy;
        entity.DeletedDate = dto.DeletedDate;
        entity.DeletedBy = dto.DeletedBy;

        return entity;
    }

    /// <summary>
    /// Chuyển đổi danh sách InventoryBalanceDto thành danh sách InventoryBalance entities
    /// </summary>
    /// <param name="dtos">Danh sách InventoryBalanceDto</param>
    /// <returns>Danh sách InventoryBalance entities</returns>
    public static List<InventoryBalance> ToEntityList(this IEnumerable<InventoryBalanceDto> dtos)
    {
        if (dtos == null) return [];

        List<InventoryBalance> list = [];
        foreach (var dto in dtos)
        {
            var entity = dto.ToEntity();
            if (entity != null) list.Add(entity);
        }

        return list;
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Map navigation properties từ entity sang DTO (warehouse, product, users)
    /// Method này được gọi từ BLL sau khi đã load navigation properties
    /// </summary>
    /// <param name="dto">DTO cần map navigation properties</param>
    /// <param name="warehouseName">Tên kho</param>
    /// <param name="warehouseCode">Mã kho</param>
    /// <param name="productName">Tên sản phẩm</param>
    /// <param name="productCode">Mã sản phẩm</param>
    /// <param name="createdByName">Tên người tạo</param>
    /// <param name="modifiedByName">Tên người sửa</param>
    /// <param name="deletedByName">Tên người xóa</param>
    /// <param name="lockedByName">Tên người khóa</param>
    /// <param name="verifiedByName">Tên người xác thực</param>
    /// <param name="approvedByName">Tên người phê duyệt</param>
    /// <returns>DTO đã được map navigation properties</returns>
    public static InventoryBalanceDto MapNavigationProperties(
        this InventoryBalanceDto dto,
        string warehouseName = null,
        string warehouseCode = null,
        string productName = null,
        string productCode = null,
        string createdByName = null,
        string modifiedByName = null,
        string deletedByName = null,
        string lockedByName = null,
        string verifiedByName = null,
        string approvedByName = null)
    {
        if (dto == null) return null;

        dto.WarehouseName = warehouseName;
        dto.WarehouseCode = warehouseCode;
        dto.ProductName = productName;
        dto.ProductCode = productCode;
        dto.CreatedByName = createdByName;
        dto.ModifiedByName = modifiedByName;
        dto.DeletedByName = deletedByName;
        dto.LockedByName = lockedByName;
        dto.VerifiedByName = verifiedByName;
        dto.ApprovedByName = approvedByName;

        return dto;
    }

    #endregion
}

