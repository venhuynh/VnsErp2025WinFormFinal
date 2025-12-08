using Dal.DataContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DTO.MasterData.CustomerPartner;

public class BusinessPartnerListDto
{
    [DisplayName("ID")]
    public Guid Id { get; set; }

    [DisplayName("Mã đối tác")]
    [Required(ErrorMessage = "Mã đối tác không được để trống")]
    [StringLength(50, ErrorMessage = "Mã đối tác không được vượt quá 50 ký tự")]
    public string PartnerCode { get; set; }

    [DisplayName("Tên đối tác")]
    [Required(ErrorMessage = "Tên đối tác không được để trống")]
    [StringLength(255, ErrorMessage = "Tên đối tác không được vượt quá 255 ký tự")]
    public string PartnerName { get; set; }

    [DisplayName("Loại đối tác")]
    public int PartnerType { get; set; }

    [DisplayName("Loại đối tác")]
    public string PartnerTypeName { get; set; } // Customer / Vendor / Both

    [DisplayName("Mã số thuế")]
    [StringLength(50, ErrorMessage = "Mã số thuế không được vượt quá 50 ký tự")]
    public string TaxCode { get; set; }

    [DisplayName("Số điện thoại")]
    [Phone(ErrorMessage = "Số điện thoại không đúng định dạng")]
    [StringLength(50, ErrorMessage = "Số điện thoại không được vượt quá 50 ký tự")]
    public string Phone { get; set; }

    [DisplayName("Email")]
    [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
    [StringLength(100, ErrorMessage = "Email không được vượt quá 100 ký tự")]
    public string Email { get; set; }

    [DisplayName("Website")]
    [StringLength(100, ErrorMessage = "Website không được vượt quá 100 ký tự")]
    public string Website { get; set; }

    [DisplayName("Địa chỉ")]
    [StringLength(255, ErrorMessage = "Địa chỉ không được vượt quá 255 ký tự")]
    public string Address { get; set; }

    [DisplayName("Thành phố")]
    [StringLength(100, ErrorMessage = "Thành phố không được vượt quá 100 ký tự")]
    public string City { get; set; }

    [DisplayName("Quốc gia")]
    [StringLength(100, ErrorMessage = "Quốc gia không được vượt quá 100 ký tự")]
    public string Country { get; set; }

    [DisplayName("Trạng thái")]
    public bool IsActive { get; set; }

    [DisplayName("Ngày tạo")]
    [DataType(DataType.DateTime)]
    public DateTime CreatedDate { get; set; }

    [DisplayName("Ngày cập nhật")]
    [DataType(DataType.DateTime)]
    public DateTime? UpdatedDate { get; set; }

    [DisplayName("Người tạo ID")]
    public Guid? CreatedBy { get; set; }

    [DisplayName("Người tạo")]
    public string CreatedByName { get; set; }

    [DisplayName("Người cập nhật ID")]
    public Guid? ModifiedBy { get; set; }

    [DisplayName("Người cập nhật")]
    public string ModifiedByName { get; set; }

    [DisplayName("Danh sách danh mục")]
    [Description("Danh sách tên các danh mục phân loại (ngăn cách bởi dấu phẩy)")]
    public string CategoryNames { get; set; }

    /// <summary>
    /// Địa chỉ đầy đủ được tính từ Address, City, Country
    /// </summary>
    [DisplayName("Địa chỉ đầy đủ")]
    [Description("Địa chỉ đầy đủ được tính từ Address, City, Country")]
    public string FullAddressName
    {
        get
        {
            var addressParts = new List<string>();
            if (!string.IsNullOrWhiteSpace(Address))
                addressParts.Add(Address);
            if (!string.IsNullOrWhiteSpace(City))
                addressParts.Add(City);
            if (!string.IsNullOrWhiteSpace(Country))
                addressParts.Add(Country);
            return string.Join(", ", addressParts);
        }
    }

    /// <summary>
    /// Thông tin đối tác dưới dạng HTML theo format DevExpress
    /// Sử dụng các tag HTML chuẩn của DevExpress: &lt;b&gt;, &lt;i&gt;, &lt;color&gt;, &lt;size&gt;
    /// Tham khảo: https://docs.devexpress.com/WindowsForms/4874/common-features/html-text-formatting
    /// </summary>
    [DisplayName("Thông tin HTML")]
    [Description("Thông tin đối tác dưới dạng HTML")]
    public string ThongTinHtml
    {
        get
        {
            var partnerName = PartnerName ?? string.Empty;
            var partnerCode = PartnerCode ?? string.Empty;
            var partnerTypeName = PartnerTypeName ?? string.Empty;
            var taxCode = TaxCode ?? string.Empty;
            var phone = Phone ?? string.Empty;
            var email = Email ?? string.Empty;
            var website = Website ?? string.Empty;
            var fullAddress = FullAddressName;
            var categoryNames = CategoryNames ?? string.Empty;
            var statusText = IsActive ? "Đang hoạt động" : "Ngừng hoạt động";
            var statusColor = IsActive ? "#4CAF50" : "#F44336";

            // Format chuyên nghiệp với visual hierarchy rõ ràng
            // - Tên đối tác: font lớn, bold, màu xanh đậm (primary)
            // - Mã đối tác: font nhỏ hơn, màu xám
            // - Loại đối tác: highlight với màu khác nhau
            // - Thông tin chi tiết: font nhỏ hơn, màu xám cho label, đen cho value
            // - Trạng thái: highlight với màu xanh (active) hoặc đỏ (inactive)

            var html = $"<b><color='blue'>{partnerName}</color></b>";

            if (!string.IsNullOrWhiteSpace(partnerCode))
            {
                html += $" <color='#757575'>({partnerCode})</color>";
            }

            html += "<br>";

            var infoParts = new List<string>();

            if (!string.IsNullOrWhiteSpace(partnerTypeName))
            {
                var typeColor = PartnerType == 1 ? "#2196F3" : PartnerType == 2 ? "#FF9800" : "#9C27B0"; // Customer: Blue, Vendor: Orange, Both: Purple
                infoParts.Add($"<color='#757575'>Loại:</color> <color='{typeColor}'><b>{partnerTypeName}</b></color>");
            }

            if (!string.IsNullOrWhiteSpace(categoryNames))
            {
                infoParts.Add($"<color='#757575'>Danh mục:</color> <b>{categoryNames}</b>");
            }

            if (infoParts.Any())
            {
                html += string.Join(" | ", infoParts) + "<br>";
            }

            if (!string.IsNullOrWhiteSpace(fullAddress))
            {
                html += $"<color='#757575'>Địa chỉ:</color> <b>{fullAddress}</b><br>";
            }

            var contactParts = new List<string>();
            if (!string.IsNullOrWhiteSpace(taxCode))
            {
                contactParts.Add($"<color='#757575'>MST:</color> <b>{taxCode}</b>");
            }
            if (!string.IsNullOrWhiteSpace(phone))
            {
                contactParts.Add($"<color='#757575'>ĐT:</color> <b>{phone}</b>");
            }
            if (!string.IsNullOrWhiteSpace(email))
            {
                contactParts.Add($"<color='#757575'>Email:</color> <b>{email}</b>");
            }
            if (!string.IsNullOrWhiteSpace(website))
            {
                contactParts.Add($"<color='#757575'>Web:</color> <b>{website}</b>");
            }

            if (contactParts.Any())
            {
                html += string.Join(" | ", contactParts) + "<br>";
            }

            html += $"<color='#757575'>Trạng thái:</color> <color='{statusColor}'><b>{statusText}</b></color>";

            return html;
        }
    }

    /// <summary>
    /// Thông tin audit (người tạo/sửa) dưới dạng HTML theo format DevExpress
    /// </summary>
    [DisplayName("Thông tin audit HTML")]
    [Description("Thông tin người tạo và cập nhật dưới dạng HTML")]
    public string AuditInfoHtml
    {
        get
        {
            var html = string.Empty;
            var infoParts = new List<string>();

            // Người tạo
            if (CreatedDate != default(DateTime))
            {
                var createdInfo = $"<color='#757575'>Tạo:</color> <b>{CreatedDate:dd/MM/yyyy HH:mm}</b>";
                if (!string.IsNullOrWhiteSpace(CreatedByName))
                {
                    createdInfo += $" <color='#757575'>bởi</color> <b>{CreatedByName}</b>";
                }
                infoParts.Add(createdInfo);
            }

            // Người cập nhật
            if (UpdatedDate.HasValue)
            {
                var modifiedInfo = $"<color='#757575'>Sửa:</color> <b>{UpdatedDate.Value:dd/MM/yyyy HH:mm}</b>";
                if (!string.IsNullOrWhiteSpace(ModifiedByName))
                {
                    modifiedInfo += $" <color='#757575'>bởi</color> <b>{ModifiedByName}</b>";
                }
                infoParts.Add(modifiedInfo);
            }

            if (infoParts.Any())
            {
                html = string.Join("<br>", infoParts);
            }

            return html;
        }
    }
}

/// <summary>
/// Converter cho BusinessPartner Entity và DTO
/// </summary>
public static class BusinessPartnerConverters
{
    /// <summary>
    /// Chuyển đổi BusinessPartner Entity sang BusinessPartnerListDto
    /// </summary>
    /// <param name="entity">BusinessPartner Entity</param>
    /// <returns>BusinessPartnerListDto</returns>
    public static BusinessPartnerListDto ToListDto(this BusinessPartner entity)
    {
        if (entity == null) return null;

        return new BusinessPartnerListDto
        {
            Id = entity.Id,
            PartnerCode = entity.PartnerCode,
            PartnerName = entity.PartnerName,
            PartnerType = entity.PartnerType,
            PartnerTypeName = ResolvePartnerTypeName(entity.PartnerType),
            TaxCode = entity.TaxCode,
            Phone = entity.Phone,
            Email = entity.Email,
            Website = entity.Website,
            Address = entity.Address,
            City = entity.City,
            Country = entity.Country,
            IsActive = entity.IsActive,
            CreatedDate = entity.CreatedDate,
            UpdatedDate = entity.UpdatedDate,
            CreatedBy = entity.CreatedBy,
            ModifiedBy = entity.ModifiedBy
        };
    }

    /// <summary>
    /// Chuyển đổi BusinessPartner Entity sang BusinessPartnerListDto với tên loại đối tác
    /// </summary>
    /// <param name="entity">BusinessPartner Entity</param>
    /// <param name="partnerTypeNameResolver">Function để resolve tên loại đối tác</param>
    /// <returns>BusinessPartnerListDto</returns>
    public static BusinessPartnerListDto ToListDto(this BusinessPartner entity,
        Func<int, string> partnerTypeNameResolver)
    {
        if (entity == null) return null;

        return new BusinessPartnerListDto
        {
            Id = entity.Id,
            PartnerCode = entity.PartnerCode,
            PartnerName = entity.PartnerName,
            PartnerType = entity.PartnerType,
            PartnerTypeName = partnerTypeNameResolver?.Invoke(entity.PartnerType) ??
                              ResolvePartnerTypeName(entity.PartnerType),
            TaxCode = entity.TaxCode,
            Phone = entity.Phone,
            Email = entity.Email,
            Website = entity.Website,
            Address = entity.Address,
            City = entity.City,
            Country = entity.Country,
            IsActive = entity.IsActive,
            CreatedDate = entity.CreatedDate,
            UpdatedDate = entity.UpdatedDate,
            CreatedBy = entity.CreatedBy,
            ModifiedBy = entity.ModifiedBy
        };
    }

    /// <summary>
    /// Chuyển đổi danh sách BusinessPartner Entity sang danh sách BusinessPartnerListDto
    /// </summary>
    /// <param name="entities">Danh sách BusinessPartner Entity</param>
    /// <returns>Danh sách BusinessPartnerListDto</returns>
    public static IEnumerable<BusinessPartnerListDto> ToBusinessPartnerListDtos(
        this IEnumerable<BusinessPartner> entities)
    {
        if (entities == null) return Enumerable.Empty<BusinessPartnerListDto>();

        return entities.Select(entity => entity.ToListDto());
    }

    /// <summary>
    /// Chuyển đổi danh sách BusinessPartner Entity sang danh sách BusinessPartnerListDto với resolver
    /// </summary>
    /// <param name="entities">Danh sách BusinessPartner Entity</param>
    /// <param name="partnerTypeNameResolver">Function để resolve tên loại đối tác</param>
    /// <returns>Danh sách BusinessPartnerListDto</returns>
    public static IEnumerable<BusinessPartnerListDto> ToBusinessPartnerListDtos(
        this IEnumerable<BusinessPartner> entities, Func<int, string> partnerTypeNameResolver)
    {
        if (entities == null) return Enumerable.Empty<BusinessPartnerListDto>();

        return entities.Select(entity => entity.ToListDto(partnerTypeNameResolver));
    }

    /// <summary>
    /// Chuyển đổi BusinessPartner Entity sang BusinessPartnerListDto với category names và user names
    /// </summary>
    /// <param name="entity">BusinessPartner Entity</param>
    /// <param name="categoryNames">Danh sách tên danh mục (ngăn cách bởi dấu phẩy)</param>
    /// <param name="createdByName">Tên người tạo</param>
    /// <param name="modifiedByName">Tên người cập nhật</param>
    /// <returns>BusinessPartnerListDto</returns>
    public static BusinessPartnerListDto ToListDtoWithNames(this BusinessPartner entity,
        string categoryNames = null, string createdByName = null, string modifiedByName = null)
    {
        if (entity == null) return null;

        var dto = entity.ToListDto();
        dto.CategoryNames = categoryNames;
        dto.CreatedByName = createdByName;
        dto.ModifiedByName = modifiedByName;
        return dto;
    }

    /// <summary>
    /// Chuyển đổi danh sách BusinessPartner Entity sang danh sách BusinessPartnerListDto với category names và user names
    /// </summary>
    /// <param name="entities">Danh sách BusinessPartner Entity</param>
    /// <param name="categoryNamesResolver">Function để resolve category names theo PartnerId</param>
    /// <param name="createdByNameResolver">Function để resolve created by name theo CreatedBy</param>
    /// <param name="modifiedByNameResolver">Function để resolve modified by name theo ModifiedBy</param>
    /// <returns>Danh sách BusinessPartnerListDto</returns>
    public static IEnumerable<BusinessPartnerListDto> ToBusinessPartnerListDtosWithNames(
        this IEnumerable<BusinessPartner> entities,
        Func<Guid, string> categoryNamesResolver = null,
        Func<Guid?, string> createdByNameResolver = null,
        Func<Guid?, string> modifiedByNameResolver = null)
    {
        if (entities == null) return Enumerable.Empty<BusinessPartnerListDto>();
        
        return entities.Select(entity =>
        {
            var categoryNames = categoryNamesResolver?.Invoke(entity.Id);
            var createdByName = createdByNameResolver?.Invoke(entity.CreatedBy);
            var modifiedByName = modifiedByNameResolver?.Invoke(entity.ModifiedBy);
            return entity.ToListDtoWithNames(categoryNames, createdByName, modifiedByName);
        });
    }

    /// <summary>
    /// Chuyển đổi BusinessPartner Entity sang BusinessPartnerDetailDto
    /// </summary>
    /// <param name="entity">BusinessPartner Entity</param>
    /// <returns>BusinessPartnerDetailDto</returns>
    public static BusinessPartnerDetailDto ToDetailDto(this BusinessPartner entity)
    {
        if (entity == null) return null;

        return new BusinessPartnerDetailDto
        {
            Id = entity.Id,
            PartnerCode = entity.PartnerCode,
            PartnerName = entity.PartnerName,
            PartnerType = entity.PartnerType,
            TaxCode = entity.TaxCode,
            Phone = entity.Phone,
            Email = entity.Email,
            Website = entity.Website,
            Address = entity.Address,
            City = entity.City,
            Country = entity.Country,
            ContactPerson = entity.ContactPerson,
            ContactPosition = entity.ContactPosition,
            BankAccount = entity.BankAccount,
            BankName = entity.BankName,
            CreditLimit = entity.CreditLimit,
            PaymentTerm = entity.PaymentTerm,
            IsActive = entity.IsActive,
            CreatedDate = entity.CreatedDate,
            UpdatedDate = entity.UpdatedDate
        };
    }

    /// <summary>
    /// Chuyển đổi BusinessPartnerDetailDto sang BusinessPartner Entity
    /// </summary>
    /// <param name="dto">BusinessPartnerDetailDto</param>
    /// <param name="existingEntity">Entity hiện tại (cho update)</param>
    /// <returns>BusinessPartner Entity</returns>
    public static BusinessPartner ToEntity(this BusinessPartnerDetailDto dto, BusinessPartner existingEntity = null)
    {
        if (dto == null) return null;

        var entity = existingEntity ?? new BusinessPartner();

        // Chỉ set ID nếu là entity mới
        if (existingEntity == null && dto.Id != Guid.Empty)
        {
            entity.Id = dto.Id;
        }

        entity.PartnerCode = dto.PartnerCode;
        entity.PartnerName = dto.PartnerName;
        entity.PartnerType = dto.PartnerType;
        entity.TaxCode = dto.TaxCode;
        entity.Phone = dto.Phone;
        entity.Email = dto.Email;
        entity.Website = dto.Website;
        entity.Address = dto.Address;
        entity.City = dto.City;
        entity.Country = dto.Country;
        entity.ContactPerson = dto.ContactPerson;
        entity.ContactPosition = dto.ContactPosition;
        entity.BankAccount = dto.BankAccount;
        entity.BankName = dto.BankName;
        entity.CreditLimit = dto.CreditLimit;
        entity.PaymentTerm = dto.PaymentTerm;
        entity.IsActive = dto.IsActive;
        entity.CreatedDate = dto.CreatedDate;
        entity.UpdatedDate = dto.UpdatedDate;

        return entity;
    }

    /// <summary>
    /// Resolve tên loại đối tác từ PartnerType
    /// </summary>
    /// <param name="partnerType">PartnerType (int)</param>
    /// <returns>Tên loại đối tác</returns>
    private static string ResolvePartnerTypeName(int partnerType)
    {
        return partnerType switch
        {
            1 => "Khách hàng",
            2 => "Nhà cung cấp",
            3 => "Cả hai",
            _ => "Không xác định"
        };
    }
}