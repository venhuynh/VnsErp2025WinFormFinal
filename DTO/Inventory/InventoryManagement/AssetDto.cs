using Dal.DataContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace DTO.Inventory.InventoryManagement;

/// <summary>
/// Data Transfer Object cho tài sản
/// Dùng cho Query và truyền dữ liệu giữa Service ↔ WinForms
/// Map với bảng Asset trong database
/// </summary>
public class AssetDto
{
    #region Properties - Thông tin cơ bản

    /// <summary>
    /// ID duy nhất của tài sản
    /// Map với: Asset.Id
    /// </summary>
    [DisplayName("ID")]
    [Display(Order = -1)]
    public Guid Id { get; set; }

    /// <summary>
    /// Mã tài sản
    /// Map với: Asset.AssetCode
    /// </summary>
    [DisplayName("Mã tài sản")]
    [Display(Order = 1)]
    [Required(ErrorMessage = "Mã tài sản không được để trống")]
    [StringLength(50, ErrorMessage = "Mã tài sản không được vượt quá 50 ký tự")]
    public string AssetCode { get; set; }

    /// <summary>
    /// Tên tài sản
    /// Map với: Asset.AssetName
    /// </summary>
    [DisplayName("Tên tài sản")]
    [Display(Order = 2)]
    [Required(ErrorMessage = "Tên tài sản không được để trống")]
    [StringLength(255, ErrorMessage = "Tên tài sản không được vượt quá 255 ký tự")]
    public string AssetName { get; set; }

    /// <summary>
    /// Loại tài sản (0: Tài sản cố định, 1: Tài sản lưu động, 2: Tài sản vô hình)
    /// Map với: Asset.AssetType
    /// </summary>
    [DisplayName("Loại tài sản")]
    [Display(Order = 3)]
    [Range(0, 2, ErrorMessage = "Loại tài sản phải trong khoảng 0-2")]
    public int AssetType { get; set; }

    /// <summary>
    /// Hiển thị loại tài sản
    /// </summary>
    [DisplayName("Loại tài sản")]
    [Display(Order = 4)]
    public string AssetTypeDisplay
    {
        get
        {
            return AssetType switch
            {
                0 => "Tài sản cố định",
                1 => "Tài sản lưu động",
                2 => "Tài sản vô hình",
                _ => "Không xác định"
            };
        }
    }

    /// <summary>
    /// Danh mục tài sản (0: Máy móc thiết bị, 1: Phương tiện vận tải, 2: Nhà cửa, 3: Đất đai, 4: Khác)
    /// Map với: Asset.AssetCategory
    /// </summary>
    [DisplayName("Danh mục")]
    [Display(Order = 5)]
    [Range(0, 4, ErrorMessage = "Danh mục tài sản phải trong khoảng 0-4")]
    public int AssetCategory { get; set; }

    /// <summary>
    /// Hiển thị danh mục tài sản
    /// </summary>
    [DisplayName("Danh mục")]
    [Display(Order = 6)]
    public string AssetCategoryDisplay
    {
        get
        {
            return AssetCategory switch
            {
                0 => "Máy móc thiết bị",
                1 => "Phương tiện vận tải",
                2 => "Nhà cửa",
                3 => "Đất đai",
                4 => "Khác",
                _ => "Không xác định"
            };
        }
    }

    /// <summary>
    /// Mô tả tài sản
    /// Map với: Asset.Description
    /// </summary>
    [DisplayName("Mô tả")]
    [Display(Order = 7)]
    [StringLength(1000, ErrorMessage = "Mô tả không được vượt quá 1000 ký tự")]
    public string Description { get; set; }

    #endregion

    #region Properties - Liên kết sản phẩm

    /// <summary>
    /// ID biến thể sản phẩm (nếu tài sản liên kết với sản phẩm)
    /// Map với: Asset.ProductVariantId
    /// </summary>
    [DisplayName("ID Sản phẩm")]
    [Display(Order = 10)]
    public Guid? ProductVariantId { get; set; }

    /// <summary>
    /// Mã biến thể sản phẩm
    /// Map với: ProductVariant.VariantCode
    /// </summary>
    [DisplayName("Mã biến thể")]
    [Display(Order = 11)]
    [StringLength(50, ErrorMessage = "Mã biến thể không được vượt quá 50 ký tự")]
    public string ProductVariantCode { get; set; }

    /// <summary>
    /// Tên biến thể sản phẩm đầy đủ
    /// Map với: ProductVariant.VariantFullName
    /// </summary>
    [DisplayName("Tên sản phẩm")]
    [Display(Order = 12)]
    [StringLength(500, ErrorMessage = "Tên sản phẩm không được vượt quá 500 ký tự")]
    public string ProductVariantFullName { get; set; }

    #endregion

    #region Properties - Thông tin công ty và địa điểm

    /// <summary>
    /// ID công ty
    /// Map với: Asset.CompanyId
    /// </summary>
    [DisplayName("ID Công ty")]
    [Display(Order = 20)]
    [Required(ErrorMessage = "ID công ty không được để trống")]
    public Guid CompanyId { get; set; }

    /// <summary>
    /// Tên công ty
    /// Map với: Company.CompanyName
    /// </summary>
    [DisplayName("Công ty")]
    [Display(Order = 21)]
    [StringLength(255, ErrorMessage = "Tên công ty không được vượt quá 255 ký tự")]
    public string CompanyName { get; set; }

    /// <summary>
    /// ID chi nhánh
    /// Map với: Asset.BranchId
    /// </summary>
    [DisplayName("ID Chi nhánh")]
    [Display(Order = 22)]
    public Guid? BranchId { get; set; }

    /// <summary>
    /// Tên chi nhánh
    /// Map với: CompanyBranch.BranchName
    /// </summary>
    [DisplayName("Chi nhánh")]
    [Display(Order = 23)]
    [StringLength(255, ErrorMessage = "Tên chi nhánh không được vượt quá 255 ký tự")]
    public string BranchName { get; set; }

    /// <summary>
    /// Mã chi nhánh
    /// Map với: CompanyBranch.BranchCode
    /// </summary>
    [DisplayName("Mã chi nhánh")]
    [Display(Order = 24)]
    [StringLength(50, ErrorMessage = "Mã chi nhánh không được vượt quá 50 ký tự")]
    public string BranchCode { get; set; }

    /// <summary>
    /// ID phòng ban
    /// Map với: Asset.DepartmentId
    /// </summary>
    [DisplayName("ID Phòng ban")]
    [Display(Order = 25)]
    public Guid? DepartmentId { get; set; }

    /// <summary>
    /// Tên phòng ban
    /// Map với: Department.DepartmentName
    /// </summary>
    [DisplayName("Phòng ban")]
    [Display(Order = 26)]
    [StringLength(255, ErrorMessage = "Tên phòng ban không được vượt quá 255 ký tự")]
    public string DepartmentName { get; set; }

    /// <summary>
    /// ID nhân viên phụ trách
    /// Map với: Asset.AssignedEmployeeId
    /// </summary>
    [DisplayName("ID Nhân viên")]
    [Display(Order = 27)]
    public Guid? AssignedEmployeeId { get; set; }

    /// <summary>
    /// Tên nhân viên phụ trách
    /// Map với: Employee.EmployeeName
    /// </summary>
    [DisplayName("Nhân viên phụ trách")]
    [Display(Order = 28)]
    [StringLength(255, ErrorMessage = "Tên nhân viên không được vượt quá 255 ký tự")]
    public string AssignedEmployeeName { get; set; }

    /// <summary>
    /// Vị trí địa lý
    /// Map với: Asset.Location
    /// </summary>
    [DisplayName("Vị trí")]
    [Display(Order = 29)]
    [StringLength(500, ErrorMessage = "Vị trí không được vượt quá 500 ký tự")]
    public string Location { get; set; }

    #endregion

    #region Properties - Thông tin tài chính

    /// <summary>
    /// Giá mua
    /// Map với: Asset.PurchasePrice
    /// </summary>
    [DisplayName("Giá mua")]
    [Display(Order = 30)]
    [Range(0, double.MaxValue, ErrorMessage = "Giá mua phải >= 0")]
    public decimal PurchasePrice { get; set; }

    /// <summary>
    /// Ngày mua
    /// Map với: Asset.PurchaseDate
    /// </summary>
    [DisplayName("Ngày mua")]
    [Display(Order = 31)]
    public DateTime? PurchaseDate { get; set; }

    /// <summary>
    /// Tên nhà cung cấp
    /// Map với: Asset.SupplierName
    /// </summary>
    [DisplayName("Nhà cung cấp")]
    [Display(Order = 32)]
    [StringLength(255, ErrorMessage = "Tên nhà cung cấp không được vượt quá 255 ký tự")]
    public string SupplierName { get; set; }

    /// <summary>
    /// Số hóa đơn
    /// Map với: Asset.InvoiceNumber
    /// </summary>
    [DisplayName("Số hóa đơn")]
    [Display(Order = 33)]
    [StringLength(100, ErrorMessage = "Số hóa đơn không được vượt quá 100 ký tự")]
    public string InvoiceNumber { get; set; }

    /// <summary>
    /// Ngày hóa đơn
    /// Map với: Asset.InvoiceDate
    /// </summary>
    [DisplayName("Ngày hóa đơn")]
    [Display(Order = 34)]
    public DateTime? InvoiceDate { get; set; }

    #endregion

    #region Properties - Khấu hao

    /// <summary>
    /// Phương pháp khấu hao (0: Đường thẳng, 1: Số dư giảm dần, 2: Khác)
    /// Map với: Asset.DepreciationMethod
    /// </summary>
    [DisplayName("Phương pháp khấu hao")]
    [Display(Order = 40)]
    [Range(0, 2, ErrorMessage = "Phương pháp khấu hao phải trong khoảng 0-2")]
    public int DepreciationMethod { get; set; }

    /// <summary>
    /// Hiển thị phương pháp khấu hao
    /// </summary>
    [DisplayName("Phương pháp khấu hao")]
    [Display(Order = 41)]
    public string DepreciationMethodDisplay
    {
        get
        {
            return DepreciationMethod switch
            {
                0 => "Đường thẳng",
                1 => "Số dư giảm dần",
                2 => "Khác",
                _ => "Không xác định"
            };
        }
    }

    /// <summary>
    /// Tỷ lệ khấu hao (%)
    /// Map với: Asset.DepreciationRate
    /// </summary>
    [DisplayName("Tỷ lệ khấu hao (%)")]
    [Display(Order = 42)]
    [Range(0, 100, ErrorMessage = "Tỷ lệ khấu hao phải trong khoảng 0-100")]
    public decimal? DepreciationRate { get; set; }

    /// <summary>
    /// Thời gian sử dụng hữu ích (tháng)
    /// Map với: Asset.UsefulLife
    /// </summary>
    [DisplayName("Thời gian sử dụng (tháng)")]
    [Display(Order = 43)]
    [Range(1, int.MaxValue, ErrorMessage = "Thời gian sử dụng phải > 0")]
    public int? UsefulLife { get; set; }

    /// <summary>
    /// Ngày bắt đầu khấu hao
    /// Map với: Asset.DepreciationStartDate
    /// </summary>
    [DisplayName("Ngày bắt đầu khấu hao")]
    [Display(Order = 44)]
    public DateTime? DepreciationStartDate { get; set; }

    /// <summary>
    /// Khấu hao lũy kế
    /// Map với: Asset.AccumulatedDepreciation
    /// </summary>
    [DisplayName("Khấu hao lũy kế")]
    [Display(Order = 45)]
    [Range(0, double.MaxValue, ErrorMessage = "Khấu hao lũy kế phải >= 0")]
    public decimal AccumulatedDepreciation { get; set; }

    /// <summary>
    /// Giá trị hiện tại (PurchasePrice - AccumulatedDepreciation)
    /// Map với: Asset.CurrentValue
    /// </summary>
    [DisplayName("Giá trị hiện tại")]
    [Display(Order = 46)]
    public decimal? CurrentValue { get; set; }

    #endregion

    #region Properties - Trạng thái và tình trạng

    /// <summary>
    /// Trạng thái (0: Đang sử dụng, 1: Bảo trì, 2: Ngừng sử dụng, 3: Thanh lý, 4: Khác)
    /// Map với: Asset.Status
    /// </summary>
    [DisplayName("Trạng thái")]
    [Display(Order = 50)]
    [Range(0, 4, ErrorMessage = "Trạng thái phải trong khoảng 0-4")]
    public int Status { get; set; }

    /// <summary>
    /// Hiển thị trạng thái
    /// </summary>
    [DisplayName("Trạng thái")]
    [Display(Order = 51)]
    public string StatusDisplay
    {
        get
        {
            return Status switch
            {
                0 => "Đang sử dụng",
                1 => "Bảo trì",
                2 => "Ngừng sử dụng",
                3 => "Thanh lý",
                4 => "Khác",
                _ => "Không xác định"
            };
        }
    }

    /// <summary>
    /// Tình trạng (0: Mới, 1: Tốt, 2: Khá, 3: Trung bình, 4: Kém)
    /// Map với: Asset.Condition
    /// </summary>
    [DisplayName("Tình trạng")]
    [Display(Order = 52)]
    [Range(0, 4, ErrorMessage = "Tình trạng phải trong khoảng 0-4")]
    public int Condition { get; set; }

    /// <summary>
    /// Hiển thị tình trạng
    /// </summary>
    [DisplayName("Tình trạng")]
    [Display(Order = 53)]
    public string ConditionDisplay
    {
        get
        {
            return Condition switch
            {
                0 => "Mới",
                1 => "Tốt",
                2 => "Khá",
                3 => "Trung bình",
                4 => "Kém",
                _ => "Không xác định"
            };
        }
    }

    #endregion

    #region Properties - Bảo hành

    /// <summary>
    /// ID bảo hành
    /// Map với: Asset.WarrantyId
    /// </summary>
    [DisplayName("ID Bảo hành")]
    [Display(Order = 60)]
    public Guid? WarrantyId { get; set; }

    /// <summary>
    /// Tên bảo hành
    /// Map với: Warranty.WarrantyName
    /// </summary>
    [DisplayName("Bảo hành")]
    [Display(Order = 61)]
    [StringLength(255, ErrorMessage = "Tên bảo hành không được vượt quá 255 ký tự")]
    public string WarrantyName { get; set; }

    /// <summary>
    /// Ngày hết hạn bảo hành
    /// Map với: Asset.WarrantyExpiryDate
    /// </summary>
    [DisplayName("Ngày hết hạn BH")]
    [Display(Order = 62)]
    public DateTime? WarrantyExpiryDate { get; set; }

    #endregion

    #region Properties - Thông tin kỹ thuật

    /// <summary>
    /// Số seri
    /// Map với: Asset.SerialNumber
    /// </summary>
    [DisplayName("Số seri")]
    [Display(Order = 70)]
    [StringLength(100, ErrorMessage = "Số seri không được vượt quá 100 ký tự")]
    public string SerialNumber { get; set; }

    /// <summary>
    /// Nhà sản xuất
    /// Map với: Asset.Manufacturer
    /// </summary>
    [DisplayName("Nhà sản xuất")]
    [Display(Order = 71)]
    [StringLength(255, ErrorMessage = "Tên nhà sản xuất không được vượt quá 255 ký tự")]
    public string Manufacturer { get; set; }

    /// <summary>
    /// Model
    /// Map với: Asset.Model
    /// </summary>
    [DisplayName("Model")]
    [Display(Order = 72)]
    [StringLength(100, ErrorMessage = "Model không được vượt quá 100 ký tự")]
    public string Model { get; set; }

    /// <summary>
    /// Thông số kỹ thuật
    /// Map với: Asset.Specifications
    /// </summary>
    [DisplayName("Thông số kỹ thuật")]
    [Display(Order = 73)]
    [StringLength(2000, ErrorMessage = "Thông số kỹ thuật không được vượt quá 2000 ký tự")]
    public string Specifications { get; set; }

    /// <summary>
    /// Ghi chú
    /// Map với: Asset.Notes
    /// </summary>
    [DisplayName("Ghi chú")]
    [Display(Order = 74)]
    [StringLength(1000, ErrorMessage = "Ghi chú không được vượt quá 1000 ký tự")]
    public string Notes { get; set; }

    #endregion

    #region Properties - Audit

    /// <summary>
    /// Trạng thái hoạt động
    /// Map với: Asset.IsActive
    /// </summary>
    [DisplayName("Hoạt động")]
    [Display(Order = 100)]
    public bool IsActive { get; set; }

    /// <summary>
    /// Đã xóa chưa
    /// Map với: Asset.IsDeleted
    /// </summary>
    [DisplayName("Đã xóa")]
    [Display(Order = 101)]
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Ngày tạo
    /// Map với: Asset.CreateDate
    /// </summary>
    [DisplayName("Ngày tạo")]
    [Display(Order = 102)]
    public DateTime CreateDate { get; set; }

    /// <summary>
    /// ID người tạo
    /// Map với: Asset.CreateBy
    /// </summary>
    [DisplayName("ID Người tạo")]
    [Display(Order = 103)]
    public Guid? CreateBy { get; set; }

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
    /// Map với: Asset.ModifiedDate
    /// </summary>
    [DisplayName("Ngày sửa")]
    [Display(Order = 105)]
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// ID người sửa
    /// Map với: Asset.ModifiedBy
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
    /// Map với: Asset.DeletedDate
    /// </summary>
    [DisplayName("Ngày xóa")]
    [Display(Order = 108)]
    public DateTime? DeletedDate { get; set; }

    /// <summary>
    /// ID người xóa
    /// Map với: Asset.DeletedBy
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
/// Converter giữa Asset entity và AssetDto
/// </summary>
public static class AssetDtoConverter
{
    #region Entity to DTO

    /// <summary>
    /// Chuyển đổi Asset entity thành AssetDto
    /// Tự động map navigation properties nếu đã được load trong entity
    /// </summary>
    /// <param name="entity">Asset entity</param>
    /// <returns>AssetDto</returns>
    public static AssetDto ToDto(this Asset entity)
    {
        if (entity == null) return null;

        var dto = new AssetDto
        {
            Id = entity.Id,
            AssetCode = entity.AssetCode,
            AssetName = entity.AssetName,
            AssetType = entity.AssetType,
            AssetCategory = entity.AssetCategory,
            Description = entity.Description,
            ProductVariantId = entity.ProductVariantId,
            CompanyId = entity.CompanyId,
            BranchId = entity.BranchId,
            DepartmentId = entity.DepartmentId,
            AssignedEmployeeId = entity.AssignedEmployeeId,
            Location = entity.Location,
            PurchasePrice = entity.PurchasePrice,
            PurchaseDate = entity.PurchaseDate,
            SupplierName = entity.SupplierName,
            InvoiceNumber = entity.InvoiceNumber,
            InvoiceDate = entity.InvoiceDate,
            DepreciationMethod = entity.DepreciationMethod,
            DepreciationRate = entity.DepreciationRate,
            UsefulLife = entity.UsefulLife,
            DepreciationStartDate = entity.DepreciationStartDate,
            AccumulatedDepreciation = entity.AccumulatedDepreciation,
            CurrentValue = entity.CurrentValue,
            Status = entity.Status,
            Condition = entity.Condition,
            WarrantyId = entity.WarrantyId,
            WarrantyExpiryDate = entity.WarrantyExpiryDate,
            SerialNumber = entity.SerialNumber,
            Manufacturer = entity.Manufacturer,
            Model = entity.Model,
            Specifications = entity.Specifications,
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
        // ProductVariant
        if (entity.ProductVariant != null)
        {
            dto.ProductVariantCode = entity.ProductVariant.VariantCode;
            dto.ProductVariantFullName = entity.ProductVariant.VariantFullName ?? string.Empty;
        }

        // Company
        if (entity.Company != null)
        {
            dto.CompanyName = entity.Company.CompanyName;
        }

        // Branch
        if (entity.CompanyBranch != null)
        {
            dto.BranchName = entity.CompanyBranch.BranchName;
            dto.BranchCode = entity.CompanyBranch.BranchCode;
        }

        // Department
        if (entity.Department != null)
        {
            dto.DepartmentName = entity.Department.DepartmentName;
        }

        // Employee
        if (entity.Employee != null)
        {
            dto.AssignedEmployeeName = entity.Employee.FullName;
        }

        // Warranty
        if (entity.Warranty != null)
        {
            // Warranty không có WarrantyName, sử dụng UniqueProductInfo hoặc tạo display string
            if (!string.IsNullOrWhiteSpace(entity.Warranty.UniqueProductInfo))
            {
                dto.WarrantyName = entity.Warranty.UniqueProductInfo;
            }
            else if (entity.Warranty.WarrantyUntil.HasValue)
            {
                dto.WarrantyName = $"Bảo hành {entity.Warranty.WarrantyType} - {entity.Warranty.WarrantyUntil.Value:dd/MM/yyyy}";
            }
            else
            {
                dto.WarrantyName = $"Bảo hành {entity.Warranty.WarrantyType}";
            }
        }

        // User names
        if (entity.ApplicationUser != null) // CreateBy
        {
            dto.CreatedByName = entity.ApplicationUser.UserName;
        }

        if (entity.ApplicationUser1 != null) // ModifiedBy
        {
            dto.ModifiedByName = entity.ApplicationUser1.UserName;
        }

        if (entity.ApplicationUser2 != null) // DeletedBy
        {
            dto.DeletedByName = entity.ApplicationUser2.UserName;
        }

        return dto;
    }

    /// <summary>
    /// Chuyển đổi danh sách Asset entities thành danh sách AssetDto
    /// </summary>
    /// <param name="entities">Danh sách Asset entities</param>
    /// <returns>Danh sách AssetDto</returns>
    public static List<AssetDto> ToDtoList(this IEnumerable<Asset> entities)
    {
        if (entities == null) return [];

        List<AssetDto> list = [];
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
    /// Chuyển đổi AssetDto thành Asset entity
    /// </summary>
    /// <param name="dto">AssetDto</param>
    /// <param name="existingEntity">Entity hiện có (nếu đang update), null nếu tạo mới</param>
    /// <returns>Asset entity</returns>
    public static Asset ToEntity(this AssetDto dto, Asset existingEntity = null)
    {
        if (dto == null) return null;

        var entity = existingEntity ?? new Asset();

        // Chỉ map các properties có thể chỉnh sửa, không map navigation properties
        entity.AssetCode = dto.AssetCode;
        entity.AssetName = dto.AssetName;
        entity.AssetType = dto.AssetType;
        entity.AssetCategory = dto.AssetCategory;
        entity.Description = dto.Description;
        entity.ProductVariantId = dto.ProductVariantId;
        entity.CompanyId = dto.CompanyId;
        entity.BranchId = dto.BranchId;
        entity.DepartmentId = dto.DepartmentId;
        entity.AssignedEmployeeId = dto.AssignedEmployeeId;
        entity.Location = dto.Location;
        entity.PurchasePrice = dto.PurchasePrice;
        entity.PurchaseDate = dto.PurchaseDate;
        entity.SupplierName = dto.SupplierName;
        entity.InvoiceNumber = dto.InvoiceNumber;
        entity.InvoiceDate = dto.InvoiceDate;
        entity.DepreciationMethod = dto.DepreciationMethod;
        entity.DepreciationRate = dto.DepreciationRate;
        entity.UsefulLife = dto.UsefulLife;
        entity.DepreciationStartDate = dto.DepreciationStartDate;
        entity.AccumulatedDepreciation = dto.AccumulatedDepreciation;
        entity.CurrentValue = dto.CurrentValue;
        entity.Status = dto.Status;
        entity.Condition = dto.Condition;
        entity.WarrantyId = dto.WarrantyId;
        entity.WarrantyExpiryDate = dto.WarrantyExpiryDate;
        entity.SerialNumber = dto.SerialNumber;
        entity.Manufacturer = dto.Manufacturer;
        entity.Model = dto.Model;
        entity.Specifications = dto.Specifications;
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
    /// Chuyển đổi danh sách AssetDto thành danh sách Asset entities
    /// </summary>
    /// <param name="dtos">Danh sách AssetDto</param>
    /// <returns>Danh sách Asset entities</returns>
    public static List<Asset> ToEntityList(this IEnumerable<AssetDto> dtos)
    {
        if (dtos == null) return [];

        List<Asset> list = [];
        foreach (var dto in dtos)
        {
            var entity = dto.ToEntity();
            if (entity != null) list.Add(entity);
        }

        return list;
    }

    #endregion
}

