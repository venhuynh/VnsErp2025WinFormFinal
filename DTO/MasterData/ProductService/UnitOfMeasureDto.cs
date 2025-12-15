using Dal.DataContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;

namespace DTO.MasterData.ProductService;

/// <summary>
/// Data Transfer Object cho UnitOfMeasure entity
/// Chứa thông tin đơn vị tính
/// Tối ưu cho hiển thị trong GridView và ComboBox
/// </summary>
public class UnitOfMeasureDto : INotifyPropertyChanged
{
    #region Private Fields
    private string _code;
    private string _name;
    private string _description;
    private bool _isActive;
    private int _productVariantCount;
    #endregion

    #region Properties

    /// <summary>
    /// ID duy nhất của đơn vị tính
    /// </summary>
    [DisplayName("ID")]
    [Display(Order = -1)]
    public Guid Id { get; set; }

    /// <summary>
    /// Mã đơn vị tính (từ UnitOfMeasure.Code)
    /// </summary>
    [DisplayName("Mã đơn vị")]
    [Display(Order = 1)]
    [Required(ErrorMessage = "Mã đơn vị không được để trống")]
    [StringLength(20, ErrorMessage = "Mã đơn vị không được vượt quá 20 ký tự")]
    public string Code 
    { 
        get => _code;
        set => SetProperty(ref _code, value);
    }

    /// <summary>
    /// Tên đơn vị tính (từ UnitOfMeasure.Name)
    /// </summary>
    [DisplayName("Tên đơn vị")]
    [Display(Order = 2)]
    [Required(ErrorMessage = "Tên đơn vị không được để trống")]
    [StringLength(100, ErrorMessage = "Tên đơn vị không được vượt quá 100 ký tự")]
    public string Name 
    { 
        get => _name;
        set => SetProperty(ref _name, value);
    }

    /// <summary>
    /// Mô tả đơn vị tính (từ UnitOfMeasure.Description)
    /// </summary>
    [DisplayName("Mô tả")]
    [Display(Order = 3)]
    [StringLength(255, ErrorMessage = "Mô tả không được vượt quá 255 ký tự")]
    public string Description 
    { 
        get => _description;
        set => SetProperty(ref _description, value);
    }

    /// <summary>
    /// Trạng thái hoạt động (từ UnitOfMeasure.IsActive)
    /// </summary>
    [DisplayName("Trạng thái")]
    [Display(Order = 4)]
    [Description("Trạng thái hoạt động của đơn vị tính")]
    public bool IsActive 
    { 
        get => _isActive;
        set => SetProperty(ref _isActive, value);
    }

    /// <summary>
    /// Số lượng ProductVariant sử dụng đơn vị tính này
    /// </summary>
    [DisplayName("Số biến thể")]
    [Display(Order = 5)]
    [Description("Số lượng biến thể sản phẩm sử dụng đơn vị tính này")]
    public int ProductVariantCount 
    { 
        get => _productVariantCount;
        set => SetProperty(ref _productVariantCount, value);
    }

    #endregion

    #region Constructor

    /// <summary>
    /// Khởi tạo DTO với giá trị mặc định
    /// </summary>
    public UnitOfMeasureDto()
    {
        Id = Guid.NewGuid();
        IsActive = true;
    }

    /// <summary>
    /// Khởi tạo DTO với thông tin cơ bản
    /// </summary>
    /// <param name="code">Mã đơn vị</param>
    /// <param name="name">Tên đơn vị</param>
    public UnitOfMeasureDto(string code, string name)
        : this()
    {
        Code = code;
        Name = name;
    }

    /// <summary>
    /// Khởi tạo DTO với đầy đủ thông tin
    /// </summary>
    /// <param name="code">Mã đơn vị</param>
    /// <param name="name">Tên đơn vị</param>
    /// <param name="description">Mô tả</param>
    /// <param name="isActive">Trạng thái hoạt động</param>
    public UnitOfMeasureDto(string code, string name, string description, bool isActive = true)
        : this(code, name)
    {
        Description = description;
        IsActive = isActive;
    }

    #endregion

    #region Computed Properties

    /// <summary>
    /// Hiển thị trạng thái dạng text
    /// </summary>
    [DisplayName("Trạng thái")]
    [Description("Trạng thái hiển thị của đơn vị tính")]
    public string StatusDisplay => IsActive ? "Hoạt động" : "Không hoạt động";

    /// <summary>
    /// Hiển thị tên đầy đủ của đơn vị (Code - Name)
    /// </summary>
    [DisplayName("Tên đầy đủ")]
    [Description("Tên đầy đủ của đơn vị tính")]
    public string FullName => $"{Code} - {Name}";

    /// <summary>
    /// Hiển thị thông tin đơn vị với mô tả (Code - Name - Description)
    /// </summary>
    [DisplayName("Thông tin đầy đủ")]
    [Description("Thông tin đầy đủ của đơn vị tính")]
    public string FullInfo
    {
        get
        {
            var info = $"{Code} - {Name}";
            if (!string.IsNullOrWhiteSpace(Description))
            {
                info += $" ({Description})";
            }
            return info;
        }
    }

    /// <summary>
    /// Hiển thị số lượng ProductVariant dạng text
    /// </summary>
    [DisplayName("Số biến thể")]
    [Description("Số lượng biến thể sản phẩm sử dụng đơn vị tính này")]
    public string ProductVariantCountDisplay => ProductVariantCount == 0 
        ? "Không có biến thể" 
        : ProductVariantCount == 1 
            ? "1 biến thể" 
            : $"{ProductVariantCount} biến thể";

    /// <summary>
    /// Thông tin đơn vị tính dưới dạng HTML theo format DevExpress
    /// Sử dụng các tag HTML chuẩn của DevExpress: &lt;b&gt;, &lt;i&gt;, &lt;color&gt;, &lt;size&gt;
    /// Tham khảo: https://docs.devexpress.com/WindowsForms/4874/common-features/html-text-formatting
    /// </summary>
    [DisplayName("Thông tin HTML")]
    [Description("Thông tin đơn vị tính dưới dạng HTML")]
    public string DisplayHtml
    {
        get
        {
            var code = Code ?? string.Empty;
            var name = Name ?? string.Empty;
            var description = Description ?? string.Empty;
            var statusText = IsActive ? "Đang hoạt động" : "Ngừng hoạt động";
            var statusColor = IsActive ? "#4CAF50" : "#F44336";

            // Format chuyên nghiệp với visual hierarchy rõ ràng
            // - Tên đơn vị: bold, màu xanh đậm (primary)
            // - Mã đơn vị: màu xám, highlight với màu cam
            // - Mô tả: màu xám cho label, đen cho value
            // - Trạng thái: highlight với màu xanh (active) hoặc đỏ (inactive)

            // Dòng đầu: Tên đơn vị (primary) - bold, màu xanh
            var html = $"<b><color='blue'>{name}</color></b>";

            // Mã đơn vị - hiển thị cùng dòng với tên, màu xám
            if (!string.IsNullOrWhiteSpace(code))
            {
                html += $" <color='#757575'>({code})</color>";
            }

            html += "<br>";

            // Mã đơn vị - mỗi thông tin trên 1 hàng (nếu có)
            if (!string.IsNullOrWhiteSpace(code))
            {
                html += $"<color='#757575'>Mã đơn vị:</color> <color='#FF9800'><b>{code}</b></color><br>";
            }

            // Mô tả - mỗi thông tin trên 1 hàng
            if (!string.IsNullOrWhiteSpace(description))
            {
                html += $"<color='#757575'>Mô tả:</color> <color='#212121'><b>{description}</b></color><br>";
            }

            // Trạng thái - highlight với màu
            html += $"<color='#757575'>Trạng thái:</color> <color='{statusColor}'><b>{statusText}</b></color>";

            // Số lượng ProductVariant sử dụng - hiển thị nếu có
            if (ProductVariantCount > 0)
            {
                html += "<br>";
                var variantCountText = ProductVariantCount == 1 
                    ? "1 biến thể sản phẩm" 
                    : $"{ProductVariantCount} biến thể sản phẩm";
                html += $"<color='#757575'>Số biến thể:</color> <color='#2196F3'><b>{variantCountText}</b></color>";
            }

            return html;
        }
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Kiểm tra xem đơn vị có hợp lệ không
    /// </summary>
    /// <returns>True nếu hợp lệ</returns>
    public bool IsValid()
    {
        return !string.IsNullOrWhiteSpace(Code) &&
               !string.IsNullOrWhiteSpace(Name);
    }

    /// <summary>
    /// Lấy danh sách lỗi validation
    /// </summary>
    /// <returns>Danh sách lỗi</returns>
    public List<string> GetValidationErrors()
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(Code))
            errors.Add("Mã đơn vị không được để trống");

        if (string.IsNullOrWhiteSpace(Name))
            errors.Add("Tên đơn vị không được để trống");

        if (Code != null && Code.Length > 20)
            errors.Add("Mã đơn vị không được vượt quá 20 ký tự");

        if (Name != null && Name.Length > 100)
            errors.Add("Tên đơn vị không được vượt quá 100 ký tự");

        if (Description != null && Description.Length > 255)
            errors.Add("Mô tả không được vượt quá 255 ký tự");

        return errors;
    }

    /// <summary>
    /// Tạo bản sao của DTO
    /// </summary>
    /// <returns>Bản sao mới</returns>
    public UnitOfMeasureDto Clone()
    {
        return new UnitOfMeasureDto
        {
            Id = Id,
            Code = Code,
            Name = Name,
            Description = Description,
            IsActive = IsActive,
            ProductVariantCount = ProductVariantCount
        };
    }

    /// <summary>
    /// Cập nhật thông tin từ DTO khác
    /// </summary>
    /// <param name="other">DTO nguồn</param>
    public void UpdateFrom(UnitOfMeasureDto other)
    {
        if (other == null) return;

        Code = other.Code;
        Name = other.Name;
        Description = other.Description;
        IsActive = other.IsActive;
        ProductVariantCount = other.ProductVariantCount;
    }

    /// <summary>
    /// Reset về trạng thái ban đầu
    /// </summary>
    public void Reset()
    {
        Code = string.Empty;
        Name = string.Empty;
        Description = string.Empty;
        IsActive = true;
        ProductVariantCount = 0;
    }

    /// <summary>
    /// Tạo đơn vị mặc định
    /// </summary>
    /// <returns>Đơn vị mặc định</returns>
    public static UnitOfMeasureDto CreateDefault()
    {
        return new UnitOfMeasureDto("PCS", "Cái", "Đơn vị tính mặc định");
    }

    #endregion

    #region INotifyPropertyChanged Implementation

    /// <summary>
    /// Event được trigger khi property thay đổi
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// Helper method để set property và trigger PropertyChanged event
    /// </summary>
    /// <typeparam name="T">Kiểu dữ liệu của property</typeparam>
    /// <param name="field">Private field</param>
    /// <param name="value">Giá trị mới</param>
    /// <param name="propertyName">Tên property (tự động lấy từ caller)</param>
    /// <returns>True nếu giá trị thay đổi</returns>
    protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return false;

        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    /// <summary>
    /// Trigger PropertyChanged event
    /// </summary>
    /// <param name="propertyName">Tên property thay đổi</param>
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion

    #region Override Methods

    /// <summary>
    /// So sánh hai DTO
    /// </summary>
    /// <param name="obj">Đối tượng so sánh</param>
    /// <returns>True nếu bằng nhau</returns>
    public override bool Equals(object obj)
    {
        if (obj is UnitOfMeasureDto other)
        {
            return Id == other.Id;
        }

        return false;
    }

    /// <summary>
    /// Lấy hash code
    /// </summary>
    /// <returns>Hash code</returns>
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    /// <summary>
    /// Chuyển đổi thành string
    /// </summary>
    /// <returns>String representation</returns>
    public override string ToString()
    {
        return FullName;
    }

    #endregion
}

/// <summary>
/// Converter giữa UnitOfMeasure entity và UnitOfMeasureDto
/// </summary>
public static class UnitOfMeasureConverters
{
    #region Entity to DTO

    /// <summary>
    /// Chuyển đổi UnitOfMeasure entity thành UnitOfMeasureDto
    /// </summary>
    /// <param name="entity">UnitOfMeasure entity</param>
    /// <returns>UnitOfMeasureDto</returns>
    public static UnitOfMeasureDto ToDto(this UnitOfMeasure entity)
    {
        if (entity == null) return null;

        return new UnitOfMeasureDto
        {
            Id = entity.Id,
            Code = entity.Code,
            Name = entity.Name,
            Description = entity.Description,
            IsActive = entity.IsActive,
            ProductVariantCount = entity.ProductVariants?.Count ?? 0
        };
    }

    /// <summary>
    /// Chuyển đổi UnitOfMeasure entity thành UnitOfMeasureDto với đếm số lượng ProductVariant
    /// </summary>
    /// <param name="entity">UnitOfMeasure entity</param>
    /// <param name="context">DataContext để đếm ProductVariant</param>
    /// <returns>UnitOfMeasureDto</returns>
    public static UnitOfMeasureDto ToDto(this UnitOfMeasure entity, VnsErp2025DataContext context)
    {
        if (entity == null) return null;

        var dto = new UnitOfMeasureDto
        {
            Id = entity.Id,
            Code = entity.Code,
            Name = entity.Name,
            Description = entity.Description,
            IsActive = entity.IsActive
        };

        // Đếm số lượng ProductVariant sử dụng đơn vị tính này
        try
        {
            if (context != null && entity.Id != Guid.Empty)
            {
                dto.ProductVariantCount = context.ProductVariants.Count(x => x.UnitId == entity.Id);
            }
            else
            {
                dto.ProductVariantCount = 0;
            }
        }
        catch
        {
            // Nếu có lỗi khi đếm, set về 0
            dto.ProductVariantCount = 0;
        }

        return dto;
    }

    /// <summary>
    /// Chuyển đổi danh sách UnitOfMeasure entities thành danh sách UnitOfMeasureDto
    /// </summary>
    /// <param name="entities">Danh sách UnitOfMeasure entities</param>
    /// <returns>Danh sách UnitOfMeasureDto</returns>
    public static List<UnitOfMeasureDto> ToDtoList(this IEnumerable<UnitOfMeasure> entities)
    {
        if (entities == null) return new List<UnitOfMeasureDto>();

        return entities.Select(entity => entity.ToDto()).ToList();
    }

    /// <summary>
    /// Chuyển đổi danh sách UnitOfMeasure entities thành danh sách UnitOfMeasureDto với đếm số lượng ProductVariant
    /// Tối ưu: Đếm tất cả trong một query thay vì đếm từng entity
    /// </summary>
    /// <param name="entities">Danh sách UnitOfMeasure entities</param>
    /// <param name="context">DataContext để đếm ProductVariant</param>
    /// <returns>Danh sách UnitOfMeasureDto</returns>
    public static List<UnitOfMeasureDto> ToDtoList(this IEnumerable<UnitOfMeasure> entities, VnsErp2025DataContext context)
    {
        if (entities == null) return new List<UnitOfMeasureDto>();

        var entityList = entities.ToList();
        if (entityList.Count == 0) return new List<UnitOfMeasureDto>();

        // Tạo dictionary để lưu số lượng ProductVariant theo UnitOfMeasureId
        var variantCountDict = new Dictionary<Guid, int>();

        try
        {
            if (context != null)
            {
                // Lấy danh sách UnitOfMeasureId
                var unitIds = entityList.Where(e => e.Id != Guid.Empty).Select(e => e.Id).ToList();
                
                if (unitIds.Any())
                {
                    // Đếm tất cả ProductVariant theo UnitId trong một query
                    var counts = context.ProductVariants
                        .Where(pv => unitIds.Contains(pv.UnitId))
                        .GroupBy(pv => pv.UnitId)
                        .Select(g => new { UnitId = g.Key, Count = g.Count() })
                        .ToList();

                    // Tạo dictionary để lookup nhanh
                    foreach (var count in counts)
                    {
                        variantCountDict[count.UnitId] = count.Count;
                    }
                }
            }
        }
        catch
        {
            // Nếu có lỗi, tất cả sẽ có count = 0
        }

        // Convert entities sang DTOs với số lượng đã đếm
        return entityList.Select(entity =>
        {
            var dto = new UnitOfMeasureDto
            {
                Id = entity.Id,
                Code = entity.Code,
                Name = entity.Name,
                Description = entity.Description,
                IsActive = entity.IsActive,
                ProductVariantCount = variantCountDict.ContainsKey(entity.Id) ? variantCountDict[entity.Id] : 0
            };
            return dto;
        }).ToList();
    }

    #endregion

    #region DTO to Entity

    /// <summary>
    /// Chuyển đổi UnitOfMeasureDto thành UnitOfMeasure entity
    /// </summary>
    /// <param name="dto">UnitOfMeasureDto</param>
    /// <returns>UnitOfMeasure entity</returns>
    public static UnitOfMeasure ToEntity(this UnitOfMeasureDto dto)
    {
        if (dto == null) return null;

        return new UnitOfMeasure
        {
            Id = dto.Id,
            Code = dto.Code,
            Name = dto.Name,
            Description = dto.Description,
            IsActive = dto.IsActive
        };
    }

    /// <summary>
    /// Chuyển đổi danh sách UnitOfMeasureDto thành danh sách UnitOfMeasure entities
    /// </summary>
    /// <param name="dtos">Danh sách UnitOfMeasureDto</param>
    /// <returns>Danh sách UnitOfMeasure entities</returns>
    public static List<UnitOfMeasure> ToEntityList(this IEnumerable<UnitOfMeasureDto> dtos)
    {
        if (dtos == null) return new List<UnitOfMeasure>();

        return dtos.Select(dto => dto.ToEntity()).ToList();
    }

    #endregion

    #region Update Entity from DTO

    /// <summary>
    /// Cập nhật UnitOfMeasure entity từ UnitOfMeasureDto
    /// </summary>
    /// <param name="entity">UnitOfMeasure entity cần cập nhật</param>
    /// <param name="dto">UnitOfMeasureDto chứa dữ liệu mới</param>
    public static void UpdateFromDto(this UnitOfMeasure entity, UnitOfMeasureDto dto)
    {
        if (entity == null || dto == null) return;

        entity.Code = dto.Code;
        entity.Name = dto.Name;
        entity.Description = dto.Description;
        entity.IsActive = dto.IsActive;
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Tạo UnitOfMeasureDto mới với thông tin cơ bản
    /// </summary>
    /// <param name="code">Mã đơn vị</param>
    /// <param name="name">Tên đơn vị</param>
    /// <param name="description">Mô tả</param>
    /// <param name="isActive">Trạng thái hoạt động</param>
    /// <returns>UnitOfMeasureDto mới</returns>
    public static UnitOfMeasureDto CreateNew(
        string code,
        string name,
        string description = null,
        bool isActive = true)
    {
        return new UnitOfMeasureDto(code, name, description, isActive);
    }

    /// <summary>
    /// Tạo danh sách UnitOfMeasureDto từ template
    /// </summary>
    /// <param name="units">Danh sách thông tin đơn vị (Code, Name, Description)</param>
    /// <returns>Danh sách UnitOfMeasureDto</returns>
    public static List<UnitOfMeasureDto> CreateBulk(
        IEnumerable<(string code, string name, string description)> units)
    {
        if (units == null) return new List<UnitOfMeasureDto>();

        return units.Select(unit => new UnitOfMeasureDto(
            unit.code,
            unit.name,
            unit.description
        )).ToList();
    }

    /// <summary>
    /// Tạo UnitOfMeasureDto từ UnitOfMeasure entity với thông tin đầy đủ (bao gồm đếm ProductVariant)
    /// </summary>
    /// <param name="entity">UnitOfMeasure entity</param>
    /// <param name="context">DataContext để đếm ProductVariant (optional)</param>
    /// <returns>UnitOfMeasureDto với thông tin đầy đủ</returns>
    public static UnitOfMeasureDto ToFullDto(this UnitOfMeasure entity, VnsErp2025DataContext context = null)
    {
        if (entity == null) return null;

        if (context != null)
        {
            return entity.ToDto(context);
        }

        return new UnitOfMeasureDto
        {
            Id = entity.Id,
            Code = entity.Code,
            Name = entity.Name,
            Description = entity.Description,
            IsActive = entity.IsActive,
            ProductVariantCount = 0 // Mặc định = 0 nếu không có context
        };
    }

    /// <summary>
    /// Tạo danh sách UnitOfMeasureDto từ danh sách UnitOfMeasure entities với thông tin đầy đủ (bao gồm đếm ProductVariant)
    /// </summary>
    /// <param name="entities">Danh sách UnitOfMeasure entities</param>
    /// <param name="context">DataContext để đếm ProductVariant (optional)</param>
    /// <returns>Danh sách UnitOfMeasureDto với thông tin đầy đủ</returns>
    public static List<UnitOfMeasureDto> ToFullDtoList(this IEnumerable<UnitOfMeasure> entities, VnsErp2025DataContext context = null)
    {
        if (entities == null) return new List<UnitOfMeasureDto>();

        if (context != null)
        {
            return entities.ToDtoList(context);
        }

        return entities.Select(entity => entity.ToFullDto()).ToList();
    }

    #endregion

    #region Validation Helpers

    /// <summary>
    /// Kiểm tra tính hợp lệ của UnitOfMeasureDto
    /// </summary>
    /// <param name="dto">UnitOfMeasureDto cần kiểm tra</param>
    /// <returns>True nếu hợp lệ</returns>
    public static bool IsValidDto(this UnitOfMeasureDto dto)
    {
        return dto?.IsValid() == true;
    }

    /// <summary>
    /// Lấy danh sách lỗi validation của UnitOfMeasureDto
    /// </summary>
    /// <param name="dto">UnitOfMeasureDto cần kiểm tra</param>
    /// <returns>Danh sách lỗi</returns>
    public static List<string> GetValidationErrors(this UnitOfMeasureDto dto)
    {
        return dto?.GetValidationErrors() ?? new List<string> { "DTO không được null" };
    }

    #endregion

    #region Search and Filter Helpers

    /// <summary>
    /// Tìm UnitOfMeasureDto theo mã
    /// </summary>
    /// <param name="dtos">Danh sách UnitOfMeasureDto</param>
    /// <param name="code">Mã cần tìm</param>
    /// <returns>UnitOfMeasureDto tìm được hoặc null</returns>
    public static UnitOfMeasureDto FindByCode(this IEnumerable<UnitOfMeasureDto> dtos, string code)
    {
        if (dtos == null || string.IsNullOrWhiteSpace(code)) return null;

        return dtos.FirstOrDefault(dto =>
            string.Equals(dto.Code, code, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Tìm UnitOfMeasureDto theo tên
    /// </summary>
    /// <param name="dtos">Danh sách UnitOfMeasureDto</param>
    /// <param name="name">Tên cần tìm</param>
    /// <returns>UnitOfMeasureDto tìm được hoặc null</returns>
    public static UnitOfMeasureDto FindByName(this IEnumerable<UnitOfMeasureDto> dtos, string name)
    {
        if (dtos == null || string.IsNullOrWhiteSpace(name)) return null;

        return dtos.FirstOrDefault(dto =>
            string.Equals(dto.Name, name, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Lọc UnitOfMeasureDto theo trạng thái hoạt động
    /// </summary>
    /// <param name="dtos">Danh sách UnitOfMeasureDto</param>
    /// <param name="isActive">Trạng thái cần lọc</param>
    /// <returns>Danh sách UnitOfMeasureDto đã lọc</returns>
    public static List<UnitOfMeasureDto> FilterByStatus(this IEnumerable<UnitOfMeasureDto> dtos, bool isActive)
    {
        if (dtos == null) return new List<UnitOfMeasureDto>();

        return dtos.Where(dto => dto.IsActive == isActive).ToList();
    }

    /// <summary>
    /// Tìm kiếm UnitOfMeasureDto theo từ khóa
    /// </summary>
    /// <param name="dtos">Danh sách UnitOfMeasureDto</param>
    /// <param name="keyword">Từ khóa tìm kiếm</param>
    /// <returns>Danh sách UnitOfMeasureDto tìm được</returns>
    public static List<UnitOfMeasureDto> Search(this IEnumerable<UnitOfMeasureDto> dtos, string keyword)
    {
        if (dtos == null) return new List<UnitOfMeasureDto>();
        if (string.IsNullOrWhiteSpace(keyword)) return dtos.ToList();

        var lowerKeyword = keyword.ToLower();
        return dtos.Where(dto =>
            (dto.Code?.ToLower().Contains(lowerKeyword) == true) ||
            (dto.Name?.ToLower().Contains(lowerKeyword) == true) ||
            (dto.Description?.ToLower().Contains(lowerKeyword) == true)
        ).ToList();
    }

    #endregion
}