using System;
using System.Drawing;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;

namespace Common.Utils
{
    /// <summary>
    /// Helper class để hiển thị DevExpress Input Box với các tùy chọn khác nhau
    /// </summary>
    public static class InputBoxHelper
    {
        #region Simple Input Box Methods

        /// <summary>
        /// Hiển thị Input Box đơn giản với TextEdit editor
        /// </summary>
        /// <param name="prompt">Text hiển thị phía trên editor</param>
        /// <param name="title">Tiêu đề dialog</param>
        /// <param name="defaultValue">Giá trị mặc định</param>
        /// <returns>Giá trị nhập vào hoặc String.Empty nếu Cancel</returns>
        public static string ShowTextInput(string prompt, string title = "Nhập liệu", string defaultValue = "")
        {
            try
            {
                var result = XtraInputBox.Show(prompt, title, defaultValue);
                return result ?? string.Empty;
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(new Exception($"Lỗi hiển thị Input Box: {ex.Message}", ex));
                return string.Empty;
            }
        }

        /// <summary>
        /// Hiển thị Input Box để nhập số nguyên
        /// </summary>
        /// <param name="prompt">Text hiển thị phía trên editor</param>
        /// <param name="title">Tiêu đề dialog</param>
        /// <param name="defaultValue">Giá trị mặc định</param>
        /// <param name="minValue">Giá trị tối thiểu</param>
        /// <param name="maxValue">Giá trị tối đa</param>
        /// <returns>Giá trị số nguyên hoặc null nếu Cancel</returns>
        public static int? ShowIntegerInput(string prompt, string title = "Nhập số", int defaultValue = 0, int? minValue = null, int? maxValue = null)
        {
            try
            {
                var args = new XtraInputBoxArgs
                {
                    Caption = title,
                    Prompt = prompt,
                    DefaultButtonIndex = 0
                };

                // Tạo SpinEdit cho số nguyên
                var spinEdit = new SpinEdit();
                spinEdit.Properties.IsFloatValue = false;
                spinEdit.Properties.MinValue = minValue ?? int.MinValue;
                spinEdit.Properties.MaxValue = maxValue ?? int.MaxValue;
                spinEdit.Properties.Mask.EditMask = "d";
                
                args.Editor = spinEdit;
                args.DefaultResponse = defaultValue;

                var result = XtraInputBox.Show(args);
                return result != null ? Convert.ToInt32(result) : (int?)null;
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(new Exception($"Lỗi hiển thị Input Box số: {ex.Message}", ex));
                return null;
            }
        }

        /// <summary>
        /// Hiển thị Input Box để nhập số thập phân
        /// </summary>
        /// <param name="prompt">Text hiển thị phía trên editor</param>
        /// <param name="title">Tiêu đề dialog</param>
        /// <param name="defaultValue">Giá trị mặc định</param>
        /// <param name="decimalPlaces">Số chữ số thập phân</param>
        /// <param name="minValue">Giá trị tối thiểu</param>
        /// <param name="maxValue">Giá trị tối đa</param>
        /// <returns>Giá trị số thập phân hoặc null nếu Cancel</returns>
        public static decimal? ShowDecimalInput(string prompt, string title = "Nhập số", decimal defaultValue = 0, int decimalPlaces = 2, decimal? minValue = null, decimal? maxValue = null)
        {
            try
            {
                var args = new XtraInputBoxArgs
                {
                    Caption = title,
                    Prompt = prompt,
                    DefaultButtonIndex = 0
                };

                // Tạo SpinEdit cho số thập phân
                var spinEdit = new SpinEdit();
                spinEdit.Properties.IsFloatValue = true;
                spinEdit.Properties.MinValue = minValue ?? decimal.MinValue;
                spinEdit.Properties.MaxValue = maxValue ?? decimal.MaxValue;
                spinEdit.Properties.Mask.EditMask = $"f{decimalPlaces}";
                
                args.Editor = spinEdit;
                args.DefaultResponse = defaultValue;

                var result = XtraInputBox.Show(args);
                return result != null ? Convert.ToDecimal(result) : (decimal?)null;
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(new Exception($"Lỗi hiển thị Input Box số thập phân: {ex.Message}", ex));
                return null;
            }
        }

        /// <summary>
        /// Hiển thị Input Box để chọn ngày tháng
        /// </summary>
        /// <param name="prompt">Text hiển thị phía trên editor</param>
        /// <param name="title">Tiêu đề dialog</param>
        /// <param name="defaultValue">Giá trị mặc định</param>
        /// <param name="minDate">Ngày tối thiểu</param>
        /// <param name="maxDate">Ngày tối đa</param>
        /// <returns>Ngày tháng hoặc null nếu Cancel</returns>
        public static DateTime? ShowDateInput(string prompt, string title = "Chọn ngày", DateTime? defaultValue = null, DateTime? minDate = null, DateTime? maxDate = null)
        {
            try
            {
                var args = new XtraInputBoxArgs
                {
                    Caption = title,
                    Prompt = prompt,
                    DefaultButtonIndex = 0
                };

                // Tạo DateEdit
                var dateEdit = new DateEdit();
                dateEdit.Properties.CalendarView = CalendarView.TouchUI;
                dateEdit.Properties.Mask.EditMask = "dd/MM/yyyy";
                dateEdit.Properties.MinValue = minDate ?? DateTime.MinValue;
                dateEdit.Properties.MaxValue = maxDate ?? DateTime.MaxValue;
                
                args.Editor = dateEdit;
                args.DefaultResponse = defaultValue ?? DateTime.Now.Date;

                var result = XtraInputBox.Show(args);
                return result != null ? Convert.ToDateTime(result) : (DateTime?)null;
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(new Exception($"Lỗi hiển thị Input Box ngày: {ex.Message}", ex));
                return null;
            }
        }

        /// <summary>
        /// Hiển thị Input Box để chọn từ ComboBox
        /// </summary>
        /// <param name="prompt">Text hiển thị phía trên editor</param>
        /// <param name="title">Tiêu đề dialog</param>
        /// <param name="items">Danh sách items</param>
        /// <param name="defaultValue">Giá trị mặc định</param>
        /// <returns>Giá trị được chọn hoặc null nếu Cancel</returns>
        public static object ShowComboBoxInput(string prompt, string title = "Chọn giá trị", object[] items = null, object defaultValue = null)
        {
            try
            {
                var args = new XtraInputBoxArgs
                {
                    Caption = title,
                    Prompt = prompt,
                    DefaultButtonIndex = 0
                };

                // Tạo ComboBoxEdit
                var comboEdit = new ComboBoxEdit();
                if (items != null)
                {
                    comboEdit.Properties.Items.AddRange(items);
                }
                comboEdit.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
                
                args.Editor = comboEdit;
                args.DefaultResponse = defaultValue;

                var result = XtraInputBox.Show(args);
                return result;
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(new Exception($"Lỗi hiển thị Input Box ComboBox: {ex.Message}", ex));
                return null;
            }
        }

        #endregion

        #region Advanced Input Box Methods

        /// <summary>
        /// Hiển thị Input Box với editor tùy chỉnh
        /// </summary>
        /// <param name="prompt">Text hiển thị phía trên editor</param>
        /// <param name="title">Tiêu đề dialog</param>
        /// <param name="editor">Editor tùy chỉnh</param>
        /// <param name="defaultValue">Giá trị mặc định</param>
        /// <param name="defaultButtonIndex">Index của button mặc định (0=OK, 1=Cancel)</param>
        /// <param name="icon">Icon của dialog</param>
        /// <returns>Giá trị từ editor hoặc null nếu Cancel</returns>
        public static object ShowCustomInput(string prompt, string title, BaseEdit editor, object defaultValue = null, int defaultButtonIndex = 0, Icon icon = null)
        {
            try
            {
                var args = new XtraInputBoxArgs
                {
                    Caption = title,
                    Prompt = prompt,
                    Editor = editor,
                    DefaultResponse = defaultValue,
                    DefaultButtonIndex = defaultButtonIndex
                };

                // Thêm icon nếu có
                if (icon != null)
                {
                    args.Showing += (sender, e) =>
                    {
                        e.MessageBoxForm.Icon = icon;
                    };
                }

                var result = XtraInputBox.Show(args);
                return result;
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(new Exception($"Lỗi hiển thị Input Box tùy chỉnh: {ex.Message}", ex));
                return null;
            }
        }

        /// <summary>
        /// Hiển thị Input Box với validation
        /// </summary>
        /// <param name="prompt">Text hiển thị phía trên editor</param>
        /// <param name="title">Tiêu đề dialog</param>
        /// <param name="defaultValue">Giá trị mặc định</param>
        /// <param name="validator">Function để validate input</param>
        /// <param name="errorMessage">Thông báo lỗi khi validation fail</param>
        /// <returns>Giá trị hợp lệ hoặc null nếu Cancel</returns>
        public static string ShowValidatedInput(string prompt, string title = "Nhập liệu", string defaultValue = "", Func<string, bool> validator = null, string errorMessage = "Giá trị không hợp lệ")
        {
            try
            {
                string result;
                do
                {
                    result = ShowTextInput(prompt, title, defaultValue);
                    
                    // Nếu user Cancel hoặc không có validator
                    if (string.IsNullOrEmpty(result) || validator == null)
                        break;
                    
                    // Validate input
                    if (!validator(result))
                    {
                        MsgBox.ShowWarning(errorMessage);
                        defaultValue = result; // Sử dụng giá trị vừa nhập làm default cho lần tiếp theo
                    }
                    else
                    {
                        break; // Input hợp lệ
                    }
                } while (true);

                return result;
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(new Exception($"Lỗi hiển thị Input Box có validation: {ex.Message}", ex));
                return string.Empty;
            }
        }

        #endregion

        #region Specialized Input Methods

        /// <summary>
        /// Hiển thị Input Box để nhập mã code (với validation)
        /// </summary>
        /// <param name="prompt">Text hiển thị</param>
        /// <param name="title">Tiêu đề dialog</param>
        /// <param name="defaultValue">Giá trị mặc định</param>
        /// <param name="allowEmpty">Cho phép để trống</param>
        /// <returns>Mã code hoặc null nếu Cancel</returns>
        public static string ShowCodeInput(string prompt = "Nhập mã code", string title = "Mã Code", string defaultValue = "", bool allowEmpty = false)
        {
            return ShowValidatedInput(prompt, title, defaultValue, 
                input => allowEmpty || !string.IsNullOrWhiteSpace(input), 
                "Mã code không được để trống");
        }

        /// <summary>
        /// Hiển thị Input Box để nhập tên (với validation)
        /// </summary>
        /// <param name="prompt">Text hiển thị</param>
        /// <param name="title">Tiêu đề dialog</param>
        /// <param name="defaultValue">Giá trị mặc định</param>
        /// <param name="maxLength">Độ dài tối đa</param>
        /// <returns>Tên hoặc null nếu Cancel</returns>
        public static string ShowNameInput(string prompt = "Nhập tên", string title = "Tên", string defaultValue = "", int maxLength = 255)
        {
            return ShowValidatedInput(prompt, title, defaultValue, 
                input => !string.IsNullOrWhiteSpace(input) && input.Length <= maxLength, 
                $"Tên không được để trống và không quá {maxLength} ký tự");
        }

        /// <summary>
        /// Hiển thị Input Box để nhập số lượng (với validation)
        /// </summary>
        /// <param name="prompt">Text hiển thị</param>
        /// <param name="title">Tiêu đề dialog</param>
        /// <param name="defaultValue">Giá trị mặc định</param>
        /// <param name="minValue">Giá trị tối thiểu</param>
        /// <param name="maxValue">Giá trị tối đa</param>
        /// <returns>Số lượng hoặc null nếu Cancel</returns>
        public static int? ShowQuantityInput(string prompt = "Nhập số lượng", string title = "Số Lượng", int defaultValue = 1, int minValue = 1, int maxValue = 999999)
        {
            return ShowIntegerInput(prompt, title, defaultValue, minValue, maxValue);
        }

        /// <summary>
        /// Hiển thị Input Box để nhập giá tiền (với validation)
        /// </summary>
        /// <param name="prompt">Text hiển thị</param>
        /// <param name="title">Tiêu đề dialog</param>
        /// <param name="defaultValue">Giá trị mặc định</param>
        /// <param name="minValue">Giá trị tối thiểu</param>
        /// <param name="maxValue">Giá trị tối đa</param>
        /// <returns>Giá tiền hoặc null nếu Cancel</returns>
        public static decimal? ShowPriceInput(string prompt = "Nhập giá tiền", string title = "Giá Tiền", decimal defaultValue = 0, decimal minValue = 0, decimal maxValue = 999999999)
        {
            return ShowDecimalInput(prompt, title, defaultValue, 2, minValue, maxValue);
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// Kiểm tra xem Input Box có được hỗ trợ không
        /// </summary>
        /// <returns>True nếu được hỗ trợ</returns>
        public static bool IsSupported()
        {
            try
            {
                // Test simple input box
                XtraInputBox.Show("Test", "Test", "Test");
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Lấy thông tin version của DevExpress Input Box
        /// </summary>
        /// <returns>Thông tin version</returns>
        public static string GetVersionInfo()
        {
            try
            {
                return $"DevExpress Input Box - Version: {typeof(XtraInputBox).Assembly.GetName().Version}";
            }
            catch
            {
                return "DevExpress Input Box - Version: Unknown";
            }
        }

        #endregion
    }
}
