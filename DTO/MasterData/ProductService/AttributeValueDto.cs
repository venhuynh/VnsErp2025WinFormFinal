using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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

    [DisplayName("Kiểu dữ liệu")]
    [Display(Order = 2)]
    public string AttributeDataType { get; set; }

    [DisplayName("Mô tả thuộc tính")]
    [Display(Order = 3)]
    public string AttributeDescription { get; set; }
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

    #region Computed Properties

    /// <summary>
    /// Thông tin thuộc tính dưới dạng HTML theo format DevExpress
    /// Sử dụng các tag HTML chuẩn của DevExpress: &lt;b&gt;, &lt;i&gt;, &lt;color&gt;
    /// Format giống ProductServiceCategoryDto.CategoryInfoHtml (không dùng &lt;size&gt;)
    /// Tham khảo: https://docs.devexpress.com/WindowsForms/4874/common-features/html-text-formatting
    /// </summary>
    [DisplayName("Tên thuộc tính đầy đủ")]
    [Description("Thông tin thuộc tính dưới dạng HTML")]
    public string AttributeInfoHtml
    {
        get
        {
            var html = string.Empty;

            // Tên thuộc tính (màu xanh, đậm)
            var attributeName = AttributeName ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(attributeName))
            {
                html += $"<b><color='blue'>{attributeName}</color></b>";
            }

            // Kiểu dữ liệu (nếu có, màu xám)
            var dataType = AttributeDataType ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(dataType))
            {
                if (!string.IsNullOrWhiteSpace(html))
                    html += " ";
                html += $"<color='#757575'>({dataType})</color>";
            }

            html += "<br>";

            // Thông tin bổ sung
            var infoParts = new List<string>();

            // Kiểu dữ liệu
            if (!string.IsNullOrWhiteSpace(dataType))
            {
                infoParts.Add($"<color='#757575'>Kiểu dữ liệu:</color> <b>{dataType}</b>");
            }

            if (infoParts.Any())
            {
                html += string.Join(" | ", infoParts) + "<br>";
            }

            // Mô tả (nếu có)
            if (!string.IsNullOrWhiteSpace(AttributeDescription))
            {
                html += $"<color='#757575'>Mô tả:</color> <b>{AttributeDescription}</b>";
            }

            return html;
        }
    }
    /// <summary>
    /// Thông tin giá trị thuộc tính dưới dạng HTML theo format DevExpress
    /// Sử dụng các tag HTML chuẩn của DevExpress: &lt;b&gt;, &lt;i&gt;, &lt;color&gt;
    /// Format tương tự AttributeDto.ThongTinHtml
    /// Tham khảo: https://docs.devexpress.com/WindowsForms/4874/common-features/html-text-formatting
    /// </summary>
    [DisplayName("Thông tin HTML")]
    [Description("Thông tin giá trị thuộc tính dưới dạng HTML")]
    public string ThongTinHtml
    {
        get
        {
            var html = string.Empty;

            // Tên thuộc tính (màu xanh, đậm)
            var attributeName = AttributeName ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(attributeName))
            {
                html += $"<b><color='blue'>{attributeName}</color></b>";
            }

            // Kiểu dữ liệu (nếu có, màu xám)
            var dataType = AttributeDataType ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(dataType))
            {
                if (!string.IsNullOrWhiteSpace(html))
                    html += " ";
                html += $"<color='#757575'>({dataType})</color>";
            }

            html += "<br>";

            // Thông tin bổ sung
            var infoParts = new List<string>();

            // Giá trị
            var value = Value ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(value))
            {
                infoParts.Add($"<color='#757575'>Giá trị:</color> <b>{value}</b>");
            }

            // Kiểu dữ liệu
            if (!string.IsNullOrWhiteSpace(dataType))
            {
                infoParts.Add($"<color='#757575'>Kiểu dữ liệu:</color> <b>{dataType}</b>");
            }

            if (infoParts.Any())
            {
                html += string.Join(" | ", infoParts) + "<br>";
            }

            // Mô tả (nếu có)
            var description = AttributeDescription ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(description))
            {
                html += $"<color='#757575'>Mô tả:</color> <b>{description}</b>";
            }

            return html;
        }
    }
    #endregion

    #region Validation Helpers

    public string GetDisplay() => string.IsNullOrWhiteSpace(AttributeName) ? Value : $"{AttributeName}: {Value}";
    #endregion

    #region Clone/Update

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
