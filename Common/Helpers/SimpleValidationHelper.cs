using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using DevExpress.XtraEditors.DXErrorProvider;

namespace Common.Helpers;

/// <summary>
/// Helper class for simplified validation using Data Annotations and DXErrorProvider
/// </summary>
public class SimpleValidationHelper
{
    private readonly DXErrorProvider _errorProvider;
    private readonly Dictionary<Control, string> _controlMappings;

    /// <summary>
    /// Initializes a new instance of SimpleValidationHelper
    /// </summary>
    /// <param name="errorProvider">The DXErrorProvider instance to use for displaying errors</param>
    public SimpleValidationHelper(DXErrorProvider errorProvider)
    {
        _errorProvider = errorProvider ?? throw new ArgumentNullException(nameof(errorProvider));
        _controlMappings = new Dictionary<Control, string>();
    }

    /// <summary>
    /// Maps a control to a property name for validation
    /// </summary>
    /// <param name="control">The control to map</param>
    /// <param name="propertyName">The property name in the DTO</param>
    public void MapControl(Control control, string propertyName)
    {
        if (control == null) throw new ArgumentNullException(nameof(control));
        if (string.IsNullOrWhiteSpace(propertyName)) throw new ArgumentException(@"Property name cannot be null or empty", nameof(propertyName));

        _controlMappings[control] = propertyName;
    }

    /// <summary>
    /// Validates a single control against its mapped property
    /// </summary>
    /// <param name="control">The control to validate</param>
    /// <param name="dataObject">The data object containing the property to validate</param>
    /// <returns>True if validation passes, false otherwise</returns>
    public bool ValidateControl(Control control, object dataObject)
    {
        if (control == null || dataObject == null) return true;

        if (!_controlMappings.TryGetValue(control, out string propertyName))
            return true;

        var property = dataObject.GetType().GetProperty(propertyName);
        if (property == null) return true;

        var value = property.GetValue(dataObject);
        var validationContext = new ValidationContext(dataObject)
        {
            MemberName = propertyName,
            DisplayName = GetDisplayName(property)
        };

        var validationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateProperty(value, validationContext, validationResults);

        // Clear any existing error for this control
        _errorProvider.SetError(control, string.Empty);

        if (!isValid && validationResults.Count > 0)
        {
            // Show the first validation error
            _errorProvider.SetError(control, validationResults[0].ErrorMessage);
            return false;
        }

        return true;
    }

    /// <summary>
    /// Validates all mapped controls against their corresponding properties
    /// </summary>
    /// <param name="dataObject">The data object to validate</param>
    /// <returns>True if all validations pass, false otherwise</returns>
    public bool ValidateAll(object dataObject)
    {
        if (dataObject == null) return false;

        bool allValid = true;

        // First, perform full object validation to catch complex validations like RequiredIf
        var validationContext = new ValidationContext(dataObject);
        var validationResults = new List<ValidationResult>();
        bool objectValid = Validator.TryValidateObject(dataObject, validationContext, validationResults, true);

        // Clear all existing errors
        ClearAllErrors();

        if (!objectValid)
        {
            // Map validation results to controls
            foreach (var validationResult in validationResults)
            {
                if (validationResult.MemberNames?.Any() == true)
                {
                    foreach (var memberName in validationResult.MemberNames)
                    {
                        var control = _controlMappings.FirstOrDefault(x => x.Value == memberName).Key;
                        if (control != null)
                        {
                            _errorProvider.SetError(control, validationResult.ErrorMessage);
                            allValid = false;
                        }
                    }
                }
            }
        }

        return allValid;
    }

    /// <summary>
    /// Clears all validation errors
    /// </summary>
    public void ClearAllErrors()
    {
        foreach (var control in _controlMappings.Keys)
        {
            _errorProvider.SetError(control, string.Empty);
        }
    }

    /// <summary>
    /// Gets the display name for a property
    /// </summary>
    /// <param name="property">The property to get display name for</param>
    /// <returns>The display name or property name if no display name is defined</returns>
    private string GetDisplayName(PropertyInfo property)
    {
        var displayAttribute = property.GetCustomAttribute<DisplayNameAttribute>();
        return displayAttribute?.DisplayName ?? property.Name;
    }

    /// <summary>
    /// Gets the number of mapped controls
    /// </summary>
    public int MappedControlsCount => _controlMappings.Count;

    /// <summary>
    /// Checks if a control is mapped
    /// </summary>
    /// <param name="control">The control to check</param>
    /// <returns>True if the control is mapped, false otherwise</returns>
    public bool IsControlMapped(Control control)
    {
        return _controlMappings.ContainsKey(control);
    }

    /// <summary>
    /// Gets the property name mapped to a control
    /// </summary>
    /// <param name="control">The control to get property name for</param>
    /// <returns>The property name or null if not mapped</returns>
    public string GetMappedPropertyName(Control control)
    {
        return _controlMappings.TryGetValue(control, out string propertyName) ? propertyName : null;
    }
}