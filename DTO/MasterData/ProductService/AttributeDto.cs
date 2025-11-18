using Dal.DataContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using Attribute = Dal.DataContext.Attribute;

namespace DTO.MasterData.ProductService;

/// <summary>
/// Data Transfer Object cho entity Attribute (thuộc tính biến thể)
/// </summary>
public class AttributeDto : INotifyPropertyChanged
{
    #region Private Fields
    private string _name;
    private string _dataType;
    private string _description;
    #endregion

    #region Properties
    [DisplayName("ID")]
    [Display(Order = -1)]
    public Guid Id { get; set; }

    [DisplayName("Tên thuộc tính")]
    [Display(Order = 1)]
    [Required(ErrorMessage = "Tên thuộc tính không được để trống")]
    [StringLength(100, ErrorMessage = "Tên thuộc tính không được vượt quá 100 ký tự")]
    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    [DisplayName("Kiểu dữ liệu")]
    [Display(Order = 2)]
    [Required(ErrorMessage = "Kiểu dữ liệu không được để trống")]
    [StringLength(50, ErrorMessage = "Kiểu dữ liệu không được vượt quá 50 ký tự")]
    public string DataType
    {
        get => _dataType;
        set => SetProperty(ref _dataType, value);
    }

    [DisplayName("Mô tả")]
    [Display(Order = 3)]
    [StringLength(255, ErrorMessage = "Mô tả không được vượt quá 255 ký tự")]
    public string Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }

    [DisplayName("Giá trị")]
    [Description("Danh sách giá trị có thể của thuộc tính")]
    public List<AttributeValueDto> AttributeValues { get; set; }
    #endregion

    #region Constructors
    public AttributeDto()
    {
        Id = Guid.NewGuid();
        AttributeValues = new List<AttributeValueDto>();
    }

    public AttributeDto(string name, string dataType)
        : this()
    {
        Name = name;
        DataType = dataType;
    }
    #endregion

    #region Computed Properties
    [DisplayName("Tên đầy đủ")]
    public string FullInfo
    {
        get
        {
            var info = $"{Name} ({DataType})";
            if (!string.IsNullOrWhiteSpace(Description))
            {
                info += $" - {Description}";
            }
            return info;
        }
    }
    #endregion

    #region Validation Helpers
    public bool IsValid()
    {
        return !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(DataType);
    }

    public List<string> GetValidationErrors()
    {
        var errors = new List<string>();
        if (string.IsNullOrWhiteSpace(Name)) errors.Add("Tên thuộc tính không được để trống");
        if (string.IsNullOrWhiteSpace(DataType)) errors.Add("Kiểu dữ liệu không được để trống");
        if (Name != null && Name.Length > 100) errors.Add("Tên thuộc tính không được vượt quá 100 ký tự");
        if (DataType != null && DataType.Length > 50) errors.Add("Kiểu dữ liệu không được vượt quá 50 ký tự");
        if (Description != null && Description.Length > 255) errors.Add("Mô tả không được vượt quá 255 ký tự");
        return errors;
    }
    #endregion

    #region Clone/Update
    public AttributeDto Clone()
    {
        return new AttributeDto
        {
            Id = Id,
            Name = Name,
            DataType = DataType,
            Description = Description,
            AttributeValues = AttributeValues?.ConvertAll(v => v.Clone())
        };
    }

    public void UpdateFrom(AttributeDto other)
    {
        if (other == null) return;
        Name = other.Name;
        DataType = other.DataType;
        Description = other.Description;
    }
    #endregion

    #region INotifyPropertyChanged
    public event PropertyChangedEventHandler PropertyChanged;

    protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion

    #region Overrides
    public override bool Equals(object obj)
    {
        if (obj is AttributeDto other) return Id == other.Id;
        return false;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public override string ToString()
    {
        return FullInfo;
    }
    #endregion
}

/// <summary>
/// Converter giữa Attribute/AttributeValue/VariantAttribute (Dal) và DTO tương ứng
/// </summary>
public static class AttributeConverters
{
    #region Attribute -> AttributeDto
    public static AttributeDto ToDto(this Attribute entity)
    {
        if (entity == null) return null;
        return new AttributeDto
        {
            Id = entity.Id,
            Name = entity.Name,
            DataType = entity.DataType,
            Description = entity.Description,
            AttributeValues = entity.AttributeValues?.Select(v => v.ToDto(entity.Name)).ToList()
        };
    }

    public static List<AttributeDto> ToDtoList(this IEnumerable<Attribute> entities)
    {
        if (entities == null) return new List<AttributeDto>();
        return entities.Select(e => e.ToDto()).ToList();
    }

    public static Attribute ToEntity(this AttributeDto dto)
    {
        if (dto == null) return null;
        return new Attribute
        {
            Id = dto.Id,
            Name = dto.Name,
            DataType = dto.DataType,
            Description = dto.Description
        };
    }
    #endregion

    #region AttributeValue -> AttributeValueDto
    public static AttributeValueDto ToDto(this AttributeValue entity)
    {
        if (entity == null) return null;
        return new AttributeValueDto
        {
            Id = entity.Id,
            AttributeId = entity.AttributeId,
            Value = entity.Value,
            AttributeName = entity.Attribute?.Name
        };
    }

    /// <summary>
    /// Chuyển đổi AttributeValue sang DTO với truyền sẵn AttributeName để tránh lazy-load khi DataContext đã dispose
    /// </summary>
    public static AttributeValueDto ToDto(this AttributeValue entity, string attributeName)
    {
        if (entity == null) return null;
        return new AttributeValueDto
        {
            Id = entity.Id,
            AttributeId = entity.AttributeId,
            Value = entity.Value,
            AttributeName = attributeName ?? entity.Attribute?.Name
        };
    }

    public static List<AttributeValueDto> ToDtoList(this IEnumerable<AttributeValue> entities)
    {
        if (entities == null) return new List<AttributeValueDto>();
        return entities.Select(e => e.ToDto()).ToList();
    }

    public static AttributeValue ToEntity(this AttributeValueDto dto)
    {
        if (dto == null) return null;
        return new AttributeValue
        {
            Id = dto.Id,
            AttributeId = dto.AttributeId,
            Value = dto.Value
        };
    }
    #endregion

    #region VariantAttribute -> VariantAttributeDto
    public static VariantAttributeDto ToDto(this VariantAttribute entity)
    {
        if (entity == null) return null;
        return new VariantAttributeDto
        {
            VariantId = entity.VariantId,
            AttributeId = entity.AttributeId,
            AttributeValueId = entity.AttributeValueId,
            AttributeName = entity.Attribute?.Name,
            AttributeValue = entity.AttributeValue?.Value
        };
    }

    public static VariantAttribute ToEntity(this VariantAttributeDto dto)
    {
        if (dto == null) return null;
        return new VariantAttribute
        {
            VariantId = dto.VariantId,
            AttributeId = dto.AttributeId,
            AttributeValueId = dto.AttributeValueId
        };
    }
    #endregion
}