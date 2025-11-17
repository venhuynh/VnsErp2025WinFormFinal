using Dal.DataContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    [DisplayName("Thành phố")]
    [StringLength(100, ErrorMessage = "Thành phố không được vượt quá 100 ký tự")]
    public string City { get; set; }

    [DisplayName("Địa chỉ đầy đủ")]
    [StringLength(500, ErrorMessage = "Địa chỉ đầy đủ không được vượt quá 500 ký tự")]
    public string FullAddressName { get; set; }

    [DisplayName("Trạng thái")]
    public bool IsActive { get; set; }

    [DisplayName("Ngày tạo")]
    [DataType(DataType.DateTime)]
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Thông tin đối tác dưới dạng HTML theo format DevExpress
    /// Sử dụng các tag HTML chuẩn của DevExpress: &lt;b&gt;, &lt;i&gt;, &lt;color&gt;, &lt;size&gt;
    /// Tham khảo: https://docs.devexpress.com/WindowsForms/4874/common-features/html-text-formatting
    /// </summary>
    [DisplayName("Thông tin HTML")]
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
            var city = City ?? string.Empty;
            var fullAddress = FullAddressName ?? string.Empty;
            var statusText = IsActive ? "Đang hoạt động" : "Ngừng hoạt động";
            var statusColor = IsActive ? "#4CAF50" : "#F44336";

            // Format chuyên nghiệp với visual hierarchy rõ ràng
            // - Tên đối tác: font lớn, bold, màu xanh đậm (primary)
            // - Mã đối tác: font nhỏ hơn, màu xám
            // - Loại đối tác: highlight với màu khác nhau
            // - Thông tin chi tiết: font nhỏ hơn, màu xám cho label, đen cho value
            // - Trạng thái: highlight với màu xanh (active) hoặc đỏ (inactive)

            var html = $"<size=12><b><color='blue'>{partnerName}</color></b></size>";

            if (!string.IsNullOrWhiteSpace(partnerCode))
            {
                html += $" <size=9><color='#757575'>({partnerCode})</color></size>";
            }

            html += "<br>";

            if (!string.IsNullOrWhiteSpace(partnerTypeName))
            {
                var typeColor = PartnerType == 1 ? "#2196F3" : PartnerType == 2 ? "#FF9800" : "#9C27B0"; // Customer: Blue, Vendor: Orange, Both: Purple
                html += $"<size=9><color='#757575'>Loại:</color></size> <size=10><color='{typeColor}'><b>{partnerTypeName}</b></color></size><br>";
            }

            if (!string.IsNullOrWhiteSpace(fullAddress))
            {
                html += $"<size=9><color='#757575'>Địa chỉ:</color></size> <size=10><color='#212121'><b>{fullAddress}</b></color></size><br>";
            }
            else if (!string.IsNullOrWhiteSpace(city))
            {
                html += $"<size=9><color='#757575'>Thành phố:</color></size> <size=10><color='#212121'><b>{city}</b></color></size><br>";
            }

            if (!string.IsNullOrWhiteSpace(taxCode))
            {
                html += $"<size=9><color='#757575'>MST:</color></size> <size=10><color='#212121'><b>{taxCode}</b></color></size>";
            }

            if (!string.IsNullOrWhiteSpace(phone))
            {
                if (!string.IsNullOrWhiteSpace(taxCode))
                    html += " | ";
                html += $"<size=9><color='#757575'>ĐT:</color></size> <size=10><color='#212121'><b>{phone}</b></color></size>";
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                if (!string.IsNullOrWhiteSpace(taxCode) || !string.IsNullOrWhiteSpace(phone))
                    html += " | ";
                html += $"<size=9><color='#757575'>Email:</color></size> <size=10><color='#212121'><b>{email}</b></color></size>";
            }

            if (!string.IsNullOrWhiteSpace(taxCode) || !string.IsNullOrWhiteSpace(phone) || !string.IsNullOrWhiteSpace(email))
            {
                html += "<br>";
            }

            html += $"<size=9><color='#757575'>Trạng thái:</color></size> <size=10><color='{statusColor}'><b>{statusText}</b></color></size>";

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
            City = entity.City,
            IsActive = entity.IsActive,
            CreatedDate = entity.CreatedDate
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
            City = entity.City,
            IsActive = entity.IsActive,
            CreatedDate = entity.CreatedDate
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