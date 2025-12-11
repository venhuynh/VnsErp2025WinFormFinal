using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraLayout;

namespace Common.Utils
{
    /// <summary>
    /// Helper class để đánh dấu các trường bắt buộc nhập trong DevExpress LayoutControl
    /// 
    /// <example>
    /// Cách sử dụng trong Form:
    /// <code>
    /// public partial class FrmExample : XtraForm
    /// {
    ///     private void KhoiTaoForm()
    ///     {
    ///         // Đánh dấu các trường bắt buộc dựa vào DTO
    ///         RequiredFieldHelper.MarkRequiredFields(this, typeof(ExampleDto));
    ///         
    ///         // Hoặc với logger tùy chỉnh
    ///         RequiredFieldHelper.MarkRequiredFields(
    ///             this, 
    ///             typeof(ExampleDto),
    ///             nullValuePrompt: "Vui lòng nhập thông tin",
    ///             logger: (msg, ex) => _logger?.LogError(msg, ex)
    ///         );
    ///     }
    /// }
    /// </code>
    /// </example>
    /// </summary>
    public static class RequiredFieldHelper
    {
        /// <summary>
        /// Đánh dấu các layout item tương ứng với thuộc tính có [Required] bằng dấu * đỏ
        /// </summary>
        /// <param name="rootControl">Control gốc chứa LayoutControl (thường là Form hoặc UserControl)</param>
        /// <param name="dtoType">Kiểu DTO/Model chứa các thuộc tính có [Required]</param>
        /// <param name="nullValuePrompt">Thông báo gợi ý khi trường trống (mặc định: "Bắt buộc nhập")</param>
        /// <param name="logger">Logger để ghi log lỗi (optional)</param>
        /// <example>
        /// <code>
        /// // Trong constructor hoặc Load event của Form
        /// RequiredFieldHelper.MarkRequiredFields(this, typeof(MyDto));
        /// </code>
        /// </example>
        public static void MarkRequiredFields(Control rootControl, Type dtoType, string nullValuePrompt = @"Bắt buộc nhập", Action<string, Exception> logger = null)
        {
            try
            {
                if (rootControl == null)
                    throw new ArgumentNullException(nameof(rootControl));
                
                if (dtoType == null)
                    throw new ArgumentNullException(nameof(dtoType));

                var requiredProps = dtoType
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.GetCustomAttributes(typeof(RequiredAttribute), true).Any())
                    .ToList();

                if (!requiredProps.Any())
                    return;

                var allLayoutItems = GetAllLayoutControlItems(rootControl);

                // Bật HTML string cho tất cả layout items
                foreach (var it in allLayoutItems)
                {
                    it.AllowHtmlStringInCaption = true;
                }

                foreach (var prop in requiredProps)
                {
                    var propName = prop.Name;
                    var item = allLayoutItems.FirstOrDefault(it => IsEditorMatchProperty(it.Control, propName));
                    if (item == null) continue;

                    // Thêm dấu * đỏ nếu chưa có
                    if (!(item.Text?.Contains("*") ?? false))
                    {
                        var baseCaption = string.IsNullOrWhiteSpace(item.Text) ? propName : item.Text;
                        item.Text = baseCaption + @" <color=red>*</color>";
                    }

                    // Thiết lập NullValuePrompt cho các editor
                    if (item.Control is BaseEdit be)
                    {
                        if (be.Properties is RepositoryItemTextEdit txtProps)
                        {
                            txtProps.NullValuePrompt = nullValuePrompt;
                            txtProps.NullValuePromptShowForEmptyValue = true;
                        }
                        else if (be.Properties is RepositoryItemMemoEdit memoProps)
                        {
                            memoProps.NullValuePrompt = nullValuePrompt;
                            memoProps.NullValuePromptShowForEmptyValue = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger?.Invoke(@"Lỗi đánh dấu trường bắt buộc", ex);
            }
        }

        /// <summary>
        /// Kiểm tra editor có match với property name không
        /// </summary>
        /// <param name="editor">Control editor cần kiểm tra</param>
        /// <param name="propName">Tên property cần so khớp</param>
        /// <returns>True nếu editor match với property name</returns>
        private static bool IsEditorMatchProperty(Control editor, string propName)
        {
            if (editor == null) return false;
            var name = editor.Name ?? string.Empty;
            
            // Tạo danh sách candidates để match
            var candidates = new List<string> { name };
            
            // Xử lý các loại control khác nhau
            // TextEdit: EmployeeCodeTextEdit -> EmployeeCode
            if (name.EndsWith("TextEdit", StringComparison.OrdinalIgnoreCase))
            {
                candidates.Add(name.Replace("TextEdit", string.Empty));
            }
            
            // SearchLookUpEdit: BranchIdSearchLookupEdit -> BranchId
            if (name.EndsWith("SearchLookupEdit", StringComparison.OrdinalIgnoreCase))
            {
                candidates.Add(name.Replace("SearchLookupEdit", string.Empty));
            }
            
            // DateEdit: HireDateDateEdit -> HireDate
            if (name.EndsWith("DateEdit", StringComparison.OrdinalIgnoreCase))
            {
                candidates.Add(name.Replace("DateEdit", string.Empty));
            }
            
            // ComboBoxEdit: GenderComboBoxEdit -> Gender
            if (name.EndsWith("ComboBoxEdit", StringComparison.OrdinalIgnoreCase))
            {
                candidates.Add(name.Replace("ComboBoxEdit", string.Empty));
            }
            
            // MemoEdit: NotesTextEdit -> Notes (có thể là TextEdit hoặc MemoEdit)
            if (name.EndsWith("MemoEdit", StringComparison.OrdinalIgnoreCase))
            {
                candidates.Add(name.Replace("MemoEdit", string.Empty));
            }
            
            // ToggleSwitch: IsActiveToggleSwitch -> IsActive
            if (name.EndsWith("ToggleSwitch", StringComparison.OrdinalIgnoreCase))
            {
                candidates.Add(name.Replace("ToggleSwitch", string.Empty));
            }
            
            // Xử lý prefix "txt" (nếu có)
            if (name.StartsWith("txt", StringComparison.OrdinalIgnoreCase))
            {
                candidates.Add(name.Substring(3));
            }
            
            return candidates.Any(c => string.Equals(c, propName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Lấy tất cả LayoutControlItem trong control
        /// </summary>
        /// <param name="root">Control gốc</param>
        /// <returns>Danh sách tất cả LayoutControlItem</returns>
        private static List<LayoutControlItem> GetAllLayoutControlItems(Control root)
        {
            var result = new List<LayoutControlItem>();
            if (root == null) return result;
            
            var layoutControls = root.Controls.OfType<LayoutControl>().ToList();
            var nested = root.Controls.Cast<Control>().SelectMany(GetAllLayoutControlItems).ToList();
            
            foreach (var lc in layoutControls)
            {
                if (lc.Root != null)
                {
                    CollectLayoutItems(lc.Root, result);
                }
            }

            result.AddRange(nested);
            return result;
        }

        /// <summary>
        /// Thu thập LayoutControlItem từ BaseLayoutItem
        /// </summary>
        /// <param name="baseItem">BaseLayoutItem gốc</param>
        /// <param name="collector">Danh sách để thu thập</param>
        private static void CollectLayoutItems(BaseLayoutItem baseItem, List<LayoutControlItem> collector)
        {
            switch (baseItem)
            {
                case null:
                    return;
                case LayoutControlItem lci:
                    collector.Add(lci);
                    break;
                case LayoutControlGroup group:
                {
                    foreach (BaseLayoutItem child in group.Items)
                    {
                        CollectLayoutItems(child, collector);
                    }
                    break;
                }
            }
        }
    }
}

