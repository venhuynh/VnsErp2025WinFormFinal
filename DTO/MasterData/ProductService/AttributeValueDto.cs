using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace DTO.MasterData.ProductService;

/// <summary>
/// Data Transfer Object cho entity AttributeValue (giá trị của thuộc tính)
/// </summary>
public class AttributeValueDto : INotifyPropertyChanged
{
    #region Private Fields
    private string _value;
    #endregion

    #region Properties
    [DisplayName("ID")]
    [Display(Order = -1)]
    public Guid Id { get; set; }

    [DisplayName("ID Thuộc tính")]
    [Display(Order = -1)]
    [Required(ErrorMessage = "Thuộc tính không được để trống")]
    public Guid AttributeId { get; set; }

    [DisplayName("Giá trị")]
    [Display(Order = 1)]
    [Required(ErrorMessage = "Giá trị không được để trống")]
    [StringLength(255, ErrorMessage = "Giá trị không được vượt quá 255 ký tự")]
    public string Value
    {
        get => _value;
        set => SetProperty(ref _value, value);
    }

    // Thông tin hiển thị mở rộng (tùy chọn)
    [DisplayName("Tên thuộc tính")]
    public string AttributeName { get; set; }
    #endregion

    #region Constructors
    public AttributeValueDto()
    {
        Id = Guid.NewGuid();
    }

    public AttributeValueDto(Guid attributeId, string value)
        : this()
    {
        AttributeId = attributeId;
        Value = value;
    }
    #endregion

    #region Validation Helpers
    public bool IsValid()
    {
        return AttributeId != Guid.Empty && !string.IsNullOrWhiteSpace(Value);
    }

    public string GetDisplay() => string.IsNullOrWhiteSpace(AttributeName) ? Value : $"{AttributeName}: {Value}";
    #endregion

    #region Clone/Update
    public AttributeValueDto Clone()
    {
        return new AttributeValueDto
        {
            Id = Id,
            AttributeId = AttributeId,
            Value = Value,
            AttributeName = AttributeName
        };
    }

    public void UpdateFrom(AttributeValueDto other)
    {
        if (other == null) return;
        AttributeId = other.AttributeId;
        Value = other.Value;
        AttributeName = other.AttributeName;
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
        if (obj is AttributeValueDto other) return Id == other.Id;
        return false;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public override string ToString()
    {
        return GetDisplay();
    }
    #endregion
}