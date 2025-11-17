using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Windows.Forms;
using DevExpress.XtraEditors.DXErrorProvider;

namespace Common.Helpers;

/// <summary>
/// Helper class đơn giản để validation với DXErrorProvider và Data Annotations
/// </summary>
public class ValidationHelper(DXErrorProvider errorProvider)
{
    private readonly DXErrorProvider _errorProvider = errorProvider ?? throw new ArgumentNullException(nameof(errorProvider));
    private readonly Dictionary<Control, PropertyInfo> _controlPropertyMap = new();

    /// <summary>
    /// Map control với property của DTO
    /// </summary>
    /// <param name="control">Control trên form</param>
    /// <param name="dtoType">Type của DTO</param>
    /// <param name="propertyName">Tên property trong DTO</param>
    public ValidationHelper MapControl(Control control, Type dtoType, string propertyName)
    {
        var property = dtoType.GetProperty(propertyName);
        if (property != null)
        {
            _controlPropertyMap[control] = property;
        }
        return this;
    }

    /// <summary>
    /// Validate một control dựa trên Data Annotations của property tương ứng
    /// </summary>
    /// <param name="control">Control cần validate</param>
    /// <param name="value">Giá trị cần validate</param>
    /// <returns>True nếu hợp lệ</returns>
    private bool ValidateControl(Control control, object value)
    {
        if (!_controlPropertyMap.TryGetValue(control, out var property))
            return true;

        var validationAttributes = property.GetCustomAttributes<ValidationAttribute>();

        _errorProvider.SetError(control, null); // Clear previous error

        foreach (var attribute in validationAttributes)
        {
            if (!attribute.IsValid(value))
            {
                var errorMessage = attribute.FormatErrorMessage(property.Name);
                _errorProvider.SetError(control, errorMessage);
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Validate tất cả controls đã được map
    /// </summary>
    /// <param name="dto">DTO object chứa dữ liệu</param>
    /// <returns>True nếu tất cả hợp lệ</returns>
    public bool ValidateAll(object dto)
    {
        bool isValid = true;

        foreach (var kvp in _controlPropertyMap)
        {
            var control = kvp.Key;
            var property = kvp.Value;
            var value = property.GetValue(dto);

            if (!ValidateControl(control, value))
            {
                isValid = false;
            }
        }

        return isValid;
    }

    /// <summary>
    /// Validate control với giá trị từ control text
    /// </summary>
    /// <param name="control">Control cần validate</param>
    /// <returns>True nếu hợp lệ</returns>
    public bool ValidateControlValue(Control control)
    {
        return ValidateControl(control, control.Text);
    }

    /// <summary>
    /// Clear tất cả errors
    /// </summary>
    public void ClearAllErrors()
    {
        _errorProvider.ClearErrors();
    }

    /// <summary>
    /// Clear error cho một control cụ thể
    /// </summary>
    /// <param name="control">Control cần clear error</param>
    public void ClearError(Control control)
    {
        _errorProvider.SetError(control, null);
    }

    /// <summary>
    /// Set error message cho control
    /// </summary>
    /// <param name="control">Control</param>
    /// <param name="errorMessage">Error message</param>
    public void SetError(Control control, string errorMessage)
    {
        _errorProvider.SetError(control, errorMessage);
    }
}