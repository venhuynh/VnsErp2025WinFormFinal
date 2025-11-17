using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Bll.Utils
{
    /// <summary>
    /// Helper class để tạo và thiết lập SuperToolTip cho các controls trong DevExpress
    /// 
    /// <example>
    /// Cách sử dụng trong Form:
    /// <code>
    /// public partial class FrmExample : XtraForm
    /// {
    ///     private void SetupSuperTips()
    ///     {
    ///         // Tạo SuperTip cho TextEdit
    ///         var textEditSuperTip = SuperToolTipHelper.CreateSuperToolTip(
    ///             title: "&lt;b&gt;&lt;color=DarkBlue&gt;🏢 Tên chức vụ&lt;/color&gt;&lt;/b&gt;",
    ///             content: "Nhập tên chức vụ trong hệ thống..."
    ///         );
    ///         TenChucVuTextEdit.SuperTip = textEditSuperTip;
    ///         
    ///         // Tạo SuperTip cho BarButtonItem
    ///         var saveSuperTip = SuperToolTipHelper.CreateSuperToolTip(
    ///             title: "&lt;b&gt;&lt;color=Blue&gt;💾 Lưu&lt;/color&gt;&lt;/b&gt;",
    ///             content: "Lưu thông tin vào database..."
    ///         );
    ///         SaveBarButtonItem.SuperTip = saveSuperTip;
    ///     }
    /// }
    /// </code>
    /// </example>
    /// </summary>
    public static class SuperToolTipHelper
    {
        /// <summary>
        /// Tạo SuperToolTip với title và content được chỉ định
        /// </summary>
        /// <param name="title">Tiêu đề của tooltip (hỗ trợ HTML)</param>
        /// <param name="content">Nội dung của tooltip (hỗ trợ HTML)</param>
        /// <returns>SuperToolTip instance đã được cấu hình</returns>
        /// <example>
        /// <code>
        /// var superTip = SuperToolTipHelper.CreateSuperToolTip(
        ///     title: "&lt;b&gt;&lt;color=DarkBlue&gt;🏢 Tên chức vụ&lt;/color&gt;&lt;/b&gt;",
        ///     content: "Nhập tên chức vụ trong hệ thống..."
        /// );
        /// </code>
        /// </example>
        public static SuperToolTip CreateSuperToolTip(string title, string content)
        {
            if (string.IsNullOrWhiteSpace(title) && string.IsNullOrWhiteSpace(content))
            {
                throw new ArgumentException("Title hoặc Content phải có ít nhất một giá trị không rỗng.");
            }

            var superTip = new SuperToolTip
            {
                AllowHtmlText = DefaultBoolean.True
            };

            // Thêm title nếu có
            if (!string.IsNullOrWhiteSpace(title))
            {
                var titleItem = new ToolTipTitleItem
                {
                    Text = title
                };
                superTip.Items.Add(titleItem);
            }

            // Thêm content nếu có
            if (!string.IsNullOrWhiteSpace(content))
            {
                var contentItem = new ToolTipItem
                {
                    Text = content
                };
                superTip.Items.Add(contentItem);
            }

            return superTip;
        }

        /// <summary>
        /// Thiết lập SuperToolTip cho một TextEdit control
        /// </summary>
        /// <param name="textEdit">TextEdit control cần thiết lập</param>
        /// <param name="title">Tiêu đề của tooltip (hỗ trợ HTML)</param>
        /// <param name="content">Nội dung của tooltip (hỗ trợ HTML)</param>
        /// <exception cref="ArgumentNullException">Nếu textEdit là null</exception>
        /// <example>
        /// <code>
        /// SuperToolTipHelper.SetTextEditSuperTip(
        ///     TenChucVuTextEdit,
        ///     title: "&lt;b&gt;&lt;color=DarkBlue&gt;🏢 Tên chức vụ&lt;/color&gt;&lt;/b&gt;",
        ///     content: "Nhập tên chức vụ trong hệ thống..."
        /// );
        /// </code>
        /// </example>
        public static void SetTextEditSuperTip(TextEdit textEdit, string title, string content)
        {
            if (textEdit == null)
                throw new ArgumentNullException(nameof(textEdit));

            var superTip = CreateSuperToolTip(title, content);
            textEdit.SuperTip = superTip;
        }

        /// <summary>
        /// Thiết lập SuperToolTip cho một BaseEdit control (TextEdit, MemoEdit, DateEdit, etc.)
        /// </summary>
        /// <param name="baseEdit">BaseEdit control cần thiết lập</param>
        /// <param name="title">Tiêu đề của tooltip (hỗ trợ HTML)</param>
        /// <param name="content">Nội dung của tooltip (hỗ trợ HTML)</param>
        /// <exception cref="ArgumentNullException">Nếu baseEdit là null</exception>
        /// <example>
        /// <code>
        /// SuperToolTipHelper.SetBaseEditSuperTip(
        ///     EmailTextEdit,
        ///     title: "&lt;b&gt;&lt;color=DarkBlue&gt;📧 Email&lt;/color&gt;&lt;/b&gt;",
        ///     content: "Nhập địa chỉ email..."
        /// );
        /// </code>
        /// </example>
        public static void SetBaseEditSuperTip(BaseEdit baseEdit, string title, string content)
        {
            if (baseEdit == null)
                throw new ArgumentNullException(nameof(baseEdit));

            var superTip = CreateSuperToolTip(title, content);
            baseEdit.SuperTip = superTip;
        }

        /// <summary>
        /// Thiết lập SuperToolTip cho một BarButtonItem
        /// </summary>
        /// <param name="barButtonItem">BarButtonItem cần thiết lập</param>
        /// <param name="title">Tiêu đề của tooltip (hỗ trợ HTML)</param>
        /// <param name="content">Nội dung của tooltip (hỗ trợ HTML)</param>
        /// <exception cref="ArgumentNullException">Nếu barButtonItem là null</exception>
        /// <example>
        /// <code>
        /// SuperToolTipHelper.SetBarButtonSuperTip(
        ///     SaveBarButtonItem,
        ///     title: "&lt;b&gt;&lt;color=Blue&gt;💾 Lưu&lt;/color&gt;&lt;/b&gt;",
        ///     content: "Lưu thông tin vào database..."
        /// );
        /// </code>
        /// </example>
        public static void SetBarButtonSuperTip(BarButtonItem barButtonItem, string title, string content)
        {
            if (barButtonItem == null)
                throw new ArgumentNullException(nameof(barButtonItem));

            var superTip = CreateSuperToolTip(title, content);
            barButtonItem.SuperTip = superTip;
        }

        /// <summary>
        /// Thiết lập SuperToolTip cho nhiều TextEdit controls cùng lúc
        /// </summary>
        /// <param name="textEditConfigs">Dictionary chứa TextEdit và cấu hình tooltip (title, content)</param>
        /// <example>
        /// <code>
        /// var configs = new Dictionary&lt;TextEdit, (string title, string content)&gt;
        /// {
        ///     { TenChucVuTextEdit, ("Title 1", "Content 1") },
        ///     { NoiLamViecTextEdit, ("Title 2", "Content 2") }
        /// };
        /// SuperToolTipHelper.SetTextEditSuperTips(configs);
        /// </code>
        /// </example>
        public static void SetTextEditSuperTips(Dictionary<TextEdit, (string title, string content)> textEditConfigs)
        {
            if (textEditConfigs == null)
                return;

            foreach (var config in textEditConfigs)
            {
                if (config.Key != null)
                {
                    SetTextEditSuperTip(config.Key, config.Value.title, config.Value.content);
                }
            }
        }

        /// <summary>
        /// Thiết lập SuperToolTip cho nhiều BarButtonItem cùng lúc
        /// </summary>
        /// <param name="barButtonConfigs">Dictionary chứa BarButtonItem và cấu hình tooltip (title, content)</param>
        /// <example>
        /// <code>
        /// var configs = new Dictionary&lt;BarButtonItem, (string title, string content)&gt;
        /// {
        ///     { SaveBarButtonItem, ("Lưu", "Lưu thông tin...") },
        ///     { CloseBarButtonItem, ("Đóng", "Đóng form...") }
        /// };
        /// SuperToolTipHelper.SetBarButtonSuperTips(configs);
        /// </code>
        /// </example>
        public static void SetBarButtonSuperTips(Dictionary<BarButtonItem, (string title, string content)> barButtonConfigs)
        {
            if (barButtonConfigs == null)
                return;

            foreach (var config in barButtonConfigs)
            {
                if (config.Key != null)
                {
                    SetBarButtonSuperTip(config.Key, config.Value.title, config.Value.content);
                }
            }
        }

        /// <summary>
        /// Thiết lập SuperToolTip cho nhiều BaseEdit controls cùng lúc
        /// </summary>
        /// <param name="baseEditConfigs">Dictionary chứa BaseEdit và cấu hình tooltip (title, content)</param>
        /// <example>
        /// <code>
        /// var configs = new Dictionary&lt;BaseEdit, (string title, string content)&gt;
        /// {
        ///     { EmailTextEdit, ("Email", "Nhập email...") },
        ///     { PhoneTextEdit, ("Phone", "Nhập số điện thoại...") }
        /// };
        /// SuperToolTipHelper.SetBaseEditSuperTips(configs);
        /// </code>
        /// </example>
        public static void SetBaseEditSuperTips(Dictionary<BaseEdit, (string title, string content)> baseEditConfigs)
        {
            if (baseEditConfigs == null)
                return;

            foreach (var config in baseEditConfigs)
            {
                if (config.Key != null)
                {
                    SetBaseEditSuperTip(config.Key, config.Value.title, config.Value.content);
                }
            }
        }

        /// <summary>
        /// Tạo SuperToolTip với nhiều items (title và nhiều content items)
        /// </summary>
        /// <param name="title">Tiêu đề của tooltip (hỗ trợ HTML)</param>
        /// <param name="contentItems">Danh sách các content items (hỗ trợ HTML)</param>
        /// <returns>SuperToolTip instance đã được cấu hình</returns>
        /// <example>
        /// <code>
        /// var superTip = SuperToolTipHelper.CreateSuperToolTipWithMultipleContents(
        ///     title: "&lt;b&gt;Hướng dẫn&lt;/b&gt;",
        ///     contentItems: new[] { "Bước 1: ...", "Bước 2: ...", "Bước 3: ..." }
        /// );
        /// </code>
        /// </example>
        public static SuperToolTip CreateSuperToolTipWithMultipleContents(string title, params string[] contentItems)
        {
            if (string.IsNullOrWhiteSpace(title) && (contentItems == null || contentItems.Length == 0))
            {
                throw new ArgumentException("Title hoặc ContentItems phải có ít nhất một giá trị không rỗng.");
            }

            var superTip = new SuperToolTip
            {
                AllowHtmlText = DefaultBoolean.True
            };

            // Thêm title nếu có
            if (!string.IsNullOrWhiteSpace(title))
            {
                var titleItem = new ToolTipTitleItem
                {
                    Text = title
                };
                superTip.Items.Add(titleItem);
            }

            // Thêm các content items
            if (contentItems != null && contentItems.Length > 0)
            {
                foreach (var content in contentItems)
                {
                    if (!string.IsNullOrWhiteSpace(content))
                    {
                        var contentItem = new ToolTipItem
                        {
                            Text = content
                        };
                        superTip.Items.Add(contentItem);
                    }
                }
            }

            return superTip;
        }
    }
}

