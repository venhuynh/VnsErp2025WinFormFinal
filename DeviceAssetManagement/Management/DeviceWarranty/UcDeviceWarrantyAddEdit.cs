using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common.Common;
using Common.Enums;
using Common.Utils;
using DTO.DeviceAssetManagement;

namespace DeviceAssetManagement.Management.DeviceWarranty
{
    public partial class UcDeviceWarrantyAddEdit : DevExpress.XtraEditors.XtraUserControl
    {
        public UcDeviceWarrantyAddEdit()
        {
            InitializeComponent();
            InitializeControl();
        }

        /// <summary>
        /// Khởi tạo control
        /// </summary>
        private void InitializeControl()
        {
            try
            {
                // Load LoaiBaoHanhEnumComboBox với các giá trị enum
                LoadLoaiBaoHanhEnumComboBox();

                // Đăng ký các events cho WarrantyDtoGridView
                InitializeGridViewEvents();
            }
            catch (Exception ex)
            {
                // Log error nếu có logger, hoặc show message
                System.Diagnostics.Debug.WriteLine($"InitializeControl: Exception occurred - {ex.Message}");
            }
        }

        /// <summary>
        /// Khởi tạo các event handlers cho WarrantyDtoGridView
        /// </summary>
        private void InitializeGridViewEvents()
        {
            try
            {
                // Event để validate và convert giá trị trước khi set vào property
                WarrantyDtoGridView.ValidatingEditor += WarrantyDtoGridView_ValidatingEditor;

                // Event để hiển thị HTML với màu sắc cho colWarrantyType
                WarrantyDtoGridView.CustomColumnDisplayText += WarrantyDtoGridView_CustomColumnDisplayText;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"InitializeGridViewEvents: Exception occurred - {ex.Message}");
            }
        }

        /// <summary>
        /// Load LoaiBaoHanhEnumComboBox với các giá trị enum và HTML tag
        /// </summary>
        private void LoadLoaiBaoHanhEnumComboBox()
        {
            try
            {
                // Xóa các items cũ
                LoaiBaoHanhEnumComboBox.Items.Clear();

                // Thêm các giá trị enum với HTML tag để hiển thị màu sắc
                // Màu sắc được thiết lập trong CustomDisplayText event
                foreach (LoaiBaoHanhEnum value in Enum.GetValues(typeof(LoaiBaoHanhEnum)))
                {
                    int index = ApplicationEnumUtils.GetValue(value);

                    // Lấy Description và màu sắc
                    var description = GetWarrantyTypeDescription(value);
                    var colorHex = WarrantyDto.GetWarrantyTypeColor(value);

                    // Tạo HTML với màu sắc
                    string itemName = $"<color='{colorHex}'>{description}</color>";

                    LoaiBaoHanhEnumComboBox.Items.Insert(index, itemName);
                }

                // Sử dụng CustomDisplayText để hiển thị text tương ứng
                LoaiBaoHanhEnumComboBox.CustomDisplayText += LoaiBaoHanhEnumComboBox_CustomDisplayText;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LoadLoaiBaoHanhEnumComboBox: Exception occurred - {ex.Message}");
            }
        }

        /// <summary>
        /// Event handler để hiển thị Description với màu sắc HTML trong LoaiBaoHanhEnumComboBox
        /// </summary>
        private void LoaiBaoHanhEnumComboBox_CustomDisplayText(object sender, DevExpress.XtraEditors.Controls.CustomDisplayTextEventArgs e)
        {
            try
            {
                if (e.Value == null) return;

                LoaiBaoHanhEnum warrantyTypeValue;

                // Nếu giá trị là string (Description), convert về enum
                if (e.Value is string stringValue)
                {
                    var warrantyTypeEnum = GetWarrantyTypeEnumFromDescription(stringValue);
                    if (!warrantyTypeEnum.HasValue)
                    {
                        e.DisplayText = stringValue;
                        return;
                    }
                    warrantyTypeValue = warrantyTypeEnum.Value;
                }
                else if (e.Value is LoaiBaoHanhEnum enumValue)
                {
                    warrantyTypeValue = enumValue;
                }
                else if (e.Value is int intValue && Enum.IsDefined(typeof(LoaiBaoHanhEnum), intValue))
                {
                    warrantyTypeValue = (LoaiBaoHanhEnum)intValue;
                }
                else
                {
                    return;
                }

                // Lấy Description và màu sắc
                var description = GetWarrantyTypeDescription(warrantyTypeValue);
                var colorHex = WarrantyDto.GetWarrantyTypeColor(warrantyTypeValue);

                // Tạo HTML với màu sắc
                e.DisplayText = $"<color='{colorHex}'>{description}</color>";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LoaiBaoHanhEnumComboBox_CustomDisplayText: Exception occurred - {ex.Message}");
                // Nếu có lỗi, hiển thị giá trị mặc định
                e.DisplayText = e.Value?.ToString() ?? string.Empty;
            }
        }

        /// <summary>
        /// Lấy Description từ enum value
        /// </summary>
        /// <param name="warrantyType">Giá trị enum</param>
        /// <returns>Description hoặc tên enum nếu không có Description</returns>
        private string GetWarrantyTypeDescription(LoaiBaoHanhEnum warrantyType)
        {
            try
            {
                // Thử sử dụng ApplicationEnumUtils trước
                if (ApplicationEnumUtils.GetDescription(warrantyType) != null)
                {
                    return ApplicationEnumUtils.GetDescription(warrantyType);
                }

                // Nếu không có, sử dụng DescriptionAttribute
                var fieldInfo = warrantyType.GetType().GetField(warrantyType.ToString());
                if (fieldInfo == null) return warrantyType.ToString();

                var descriptionAttribute = fieldInfo.GetCustomAttribute<DescriptionAttribute>();
                return descriptionAttribute?.Description ?? warrantyType.ToString();
            }
            catch
            {
                return warrantyType.ToString();
            }
        }

        /// <summary>
        /// Lấy enum value từ Description string (có thể chứa HTML tags)
        /// </summary>
        /// <param name="description">Description string (có thể chứa HTML tags)</param>
        /// <returns>Enum value hoặc null nếu không tìm thấy</returns>
        private LoaiBaoHanhEnum? GetWarrantyTypeEnumFromDescription(string description)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(description))
                    return null;

                // Strip HTML tags nếu có
                var cleanDescription = StripHtmlTags(description);

                // Duyệt qua tất cả các giá trị enum để tìm Description khớp
                foreach (LoaiBaoHanhEnum enumValue in Enum.GetValues(typeof(LoaiBaoHanhEnum)))
                {
                    var enumDescription = GetWarrantyTypeDescription(enumValue);
                    if (string.Equals(enumDescription, cleanDescription, StringComparison.OrdinalIgnoreCase))
                    {
                        return enumValue;
                    }
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Loại bỏ HTML tags từ string
        /// </summary>
        /// <param name="htmlString">String chứa HTML tags</param>
        /// <returns>String không có HTML tags</returns>
        private string StripHtmlTags(string htmlString)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(htmlString))
                    return htmlString;

                var result = htmlString;

                // Loại bỏ <color='...'> và </color>
                result = System.Text.RegularExpressions.Regex.Replace(result, @"<color=['""][^'""]*['""]>", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result, @"</color>", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // Loại bỏ các tags khác nếu có
                result = System.Text.RegularExpressions.Regex.Replace(result, @"<[^>]+>", "");

                return result.Trim();
            }
            catch
            {
                return htmlString;
            }
        }

        #region ========== GRIDVIEW EVENTS ==========

        /// <summary>
        /// Event handler để validate và convert giá trị trước khi set vào property
        /// Xử lý conversion từ string (HTML Description) sang enum
        /// </summary>
        private void WarrantyDtoGridView_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            try
            {
                var view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
                if (view == null) return;

                var focusedColumn = view.FocusedColumn;
                if (focusedColumn != colWarrantyType) return;

                // Nếu giá trị là string (Description với HTML), convert về enum
                if (e.Value is string warrantyTypeDescription)
                {
                    var warrantyTypeEnum = GetWarrantyTypeEnumFromDescription(warrantyTypeDescription);
                    if (warrantyTypeEnum.HasValue)
                    {
                        // Set lại giá trị là enum để DevExpress có thể bind đúng
                        e.Value = warrantyTypeEnum.Value;
                        e.Valid = true;
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"WarrantyDtoGridView_ValidatingEditor: Cannot convert warranty type description '{warrantyTypeDescription}' to enum");
                        e.ErrorText = $"Không thể chuyển đổi loại bảo hành '{warrantyTypeDescription}'";
                        e.Valid = false;
                    }
                }
                // Nếu giá trị đã là enum, giữ nguyên
                else if (e.Value is LoaiBaoHanhEnum)
                {
                    e.Valid = true;
                }
                // Nếu giá trị là int, convert về enum
                else if (e.Value is int intValue && Enum.IsDefined(typeof(LoaiBaoHanhEnum), intValue))
                {
                    e.Value = (LoaiBaoHanhEnum)intValue;
                    e.Valid = true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"WarrantyDtoGridView_ValidatingEditor: Exception occurred - {ex.Message}");
                e.ErrorText = $"Lỗi xử lý giá trị: {ex.Message}";
                e.Valid = false;
            }
        }

        /// <summary>
        /// Event handler để hiển thị HTML với màu sắc cho colWarrantyType trong GridView
        /// </summary>
        private void WarrantyDtoGridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            try
            {
                // Chỉ xử lý cột WarrantyType
                if (e.Column != colWarrantyType || e.Value == null)
                    return;

                LoaiBaoHanhEnum warrantyTypeValue;

                // Convert giá trị về enum
                if (e.Value is LoaiBaoHanhEnum enumValue)
                {
                    warrantyTypeValue = enumValue;
                }
                else if (e.Value is int intValue && Enum.IsDefined(typeof(LoaiBaoHanhEnum), intValue))
                {
                    warrantyTypeValue = (LoaiBaoHanhEnum)intValue;
                }
                else if (e.Value is string stringValue)
                {
                    // Nếu là string, thử strip HTML và convert
                    var cleanString = StripHtmlTags(stringValue);
                    var warrantyTypeEnum = GetWarrantyTypeEnumFromDescription(cleanString);
                    if (!warrantyTypeEnum.HasValue)
                    {
                        return; // Không thể convert, giữ nguyên giá trị
                    }
                    warrantyTypeValue = warrantyTypeEnum.Value;
                }
                else
                {
                    return;
                }

                // Lấy Description và màu sắc
                var description = GetWarrantyTypeDescription(warrantyTypeValue);
                var colorHex = WarrantyDto.GetWarrantyTypeColor(warrantyTypeValue);

                // Tạo HTML với màu sắc
                e.DisplayText = $"<color='{colorHex}'>{description}</color>";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"WarrantyDtoGridView_CustomColumnDisplayText: Exception occurred - {ex.Message}");
                // Nếu có lỗi, giữ nguyên giá trị mặc định
            }
        }

        #endregion
    }
}
