using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;

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

    /// <summary>
    /// Thông tin thuộc tính dưới dạng HTML theo format DevExpress
    /// Sử dụng các tag HTML chuẩn của DevExpress: &lt;b&gt;, &lt;i&gt;, &lt;color&gt;
    /// Format giống ProductServiceCategoryDto.CategoryInfoHtml (không dùng &lt;size&gt;)
    /// Tham khảo: https://docs.devexpress.com/WindowsForms/4874/common-features/html-text-formatting
    /// </summary>
    [DisplayName("Thông tin HTML")]
    [Description("Thông tin thuộc tính dưới dạng HTML")]
    public string ThongTinHtml
    {
        get
        {
            var html = string.Empty;

            // Tên thuộc tính (màu xanh, đậm)
            var attributeName = Name ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(attributeName))
            {
                html += $"<b><color='blue'>{attributeName}</color></b>";
            }

            // Kiểu dữ liệu (nếu có, màu xám)
            var dataType = DataType ?? string.Empty;
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

            // Số lượng giá trị (nếu có)
            var valueCount = AttributeValues?.Count ?? 0;
            if (valueCount > 0)
            {
                infoParts.Add($"<color='#757575'>Số giá trị:</color> <b>{valueCount}</b>");
            }

            if (infoParts.Any())
            {
                html += string.Join(" | ", infoParts) + "<br>";
            }

            // Mô tả (nếu có)
            if (!string.IsNullOrWhiteSpace(Description))
            {
                html += $"<color='#757575'>Mô tả:</color> <b>{Description}</b>";
            }

            return html;
        }
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
