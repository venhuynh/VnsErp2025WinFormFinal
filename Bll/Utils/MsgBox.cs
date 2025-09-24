using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.Xpo.Logger;

namespace Bll.Utils
{
    /// <summary>
    /// Tiện ích hiển thị hộp thoại dựa trên DevExpress XtraMessageBox.
    /// - Luôn gắn Owner để không xuất hiện riêng trong Taskbar và đảm bảo modal.
    /// - Hỗ trợ hiển thị nội dung HTML bằng XtraMessageBoxArgs (AllowHtmlText=True).
    /// - Cung cấp API thông báo, cảnh báo, lỗi, xác nhận (Yes/No, Yes/No/Cancel).
    /// </summary>
    public static class MsgBox
    {
        // Lấy owner an toàn để hộp thoại luôn modal theo form cha (không có entry riêng trên Taskbar)
		private static IWin32Window GetOwnerWindow()
		{
			var active = Form.ActiveForm;
			if (active != null && !active.IsDisposed) return active;

			for (int i = 0; i < Application.OpenForms.Count; i++)
			{
				var form = Application.OpenForms[i];
				if (form != null && form.Visible && !form.IsDisposed) return form;
			}
			return null;
		}

		/// <summary>
		/// Quy đổi MessageBoxIcon hệ thống sang Icon tương ứng.
		/// </summary>
		private static Icon GetSystemIcon(MessageBoxIcon icon)
		{
			return icon switch
			{
				MessageBoxIcon.Error => SystemIcons.Error,
				MessageBoxIcon.Warning => SystemIcons.Warning,
				MessageBoxIcon.Information => SystemIcons.Information,
				MessageBoxIcon.Question => SystemIcons.Question,
				_ => null
			};
		}

		/// <summary>
		/// Chuyển đổi kiểu nút chuẩn thành danh sách DialogResult dành cho XtraMessageBoxArgs.
		/// </summary>
		private static DialogResult[] ToDialogResults(MessageBoxButtons buttons)
		{
			switch (buttons)
			{
				case MessageBoxButtons.OK:
					return new[] { DialogResult.OK };
				case MessageBoxButtons.OKCancel:
					return new[] { DialogResult.OK, DialogResult.Cancel };
				case MessageBoxButtons.YesNo:
					return new[] { DialogResult.Yes, DialogResult.No };
				case MessageBoxButtons.YesNoCancel:
					return new[] { DialogResult.Yes, DialogResult.No, DialogResult.Cancel };
				case MessageBoxButtons.RetryCancel:
					return new[] { DialogResult.Retry, DialogResult.Cancel };
				case MessageBoxButtons.AbortRetryIgnore:
					return new[] { DialogResult.Abort, DialogResult.Retry, DialogResult.Ignore };
				default:
					return new[] { DialogResult.OK };
			}
		}

		// Tạo args để hiển thị HTML, đảm bảo Owner và không hiện trên Taskbar
		private static XtraMessageBoxArgs CreateHtmlArgs(string htmlText, string title, MessageBoxButtons buttons, MessageBoxIcon icon)
		{
			var owner = GetOwnerWindow();
			var args = new XtraMessageBoxArgs
			{
				Caption = title,
				Text = htmlText,
				Buttons = ToDialogResults(buttons),
				Icon = GetSystemIcon(icon),
				AllowHtmlText = DefaultBoolean.True,
				Owner = owner
			};

			args.Showing += (_, e) =>
			{
				if (e.Form is XtraMessageBoxForm frm)
				{
					frm.ShowInTaskbar = false;
					frm.StartPosition = FormStartPosition.CenterParent;
				}
			};

			return args;
		}

        /// <summary>
        /// Dự phòng để tích hợp logger nếu cần trong tương lai.
        /// </summary>
        public static void Configure(ILogger logger)
        {
        }

		/// <summary>
		/// Hiển thị thông báo dạng thông tin (Information).
		/// </summary>
		public static void ShowInfo(string messageText, string title = "Thông báo")
		{
			var owner = GetOwnerWindow();
			XtraMessageBox.Show(owner, messageText, title, MessageBoxButtons.OK,
				MessageBoxIcon.Information);
		}

		/// <summary>
		/// Hiển thị thông báo lỗi đơn giản (Error).
		/// </summary>
		public static void ShowError(string messageText, string title = "Lỗi")
		{
			var owner = GetOwnerWindow();
			XtraMessageBox.Show(owner, messageText, title, MessageBoxButtons.OK,
				MessageBoxIcon.Error);
		}

		/// <summary>
		/// Hiển thị chi tiết Exception. Khi chạy dưới debugger sẽ kèm StackTrace/InnerException.
		/// </summary>
		public static void ShowException(Exception ex, string title = "Lỗi")
        {
            // Determine message based on debug mode
            string message;
            if (Debugger.IsAttached)
            {
                message = $"Message: {ex.Message}\n\n" +
                         $"Source: {ex.Source}\n\n" +
                         $"StackTrace: {ex.StackTrace}";

                if (ex.InnerException != null)
                {
                    message += $"\n\nInner Exception:\n" +
                              $"Message: {ex.InnerException.Message}\n" +
                              $"StackTrace: {ex.InnerException.StackTrace}";
                }
            }
            else
            {
                message = ex.InnerException?.Message ?? ex.Message;
            }

			var owner = GetOwnerWindow();
			XtraMessageBox.Show(owner, message, title, MessageBoxButtons.OK,
				MessageBoxIcon.Error);
        }

		/// <summary>
		/// Hiển thị cảnh báo (Warning).
		/// </summary>
		public static void ShowWarning(string messageText, string title = "Cảnh báo")
		{
			var owner = GetOwnerWindow();
			XtraMessageBox.Show(owner, messageText, title, MessageBoxButtons.OK,
				MessageBoxIcon.Warning);
		}

		/// <summary>
		/// Hộp thoại xác nhận Yes/No.
		/// </summary>
		public static bool GetConfirmFromYesNoDialog(string message, string title = "Xác nhận")
		{
			var owner = GetOwnerWindow();
			var result = XtraMessageBox.Show(owner, message, title,
				MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			return result == DialogResult.Yes;
		}

		/// <summary>
		/// Hộp thoại xác nhận Yes/No/Cancel.
		/// </summary>
		public static DialogResult GetConfirmFromYesNoCancelDialog(string message, string title = "Xác nhận")
		{
			var owner = GetOwnerWindow();
			return XtraMessageBox.Show(owner, message, title,
				MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
		}

		// ====== API hỗ trợ HTML ======

		/// <summary>
		/// Thông báo dạng HTML (Information).
		/// </summary>
		public static void ShowHtmlInfo(string html, string title = "Thông báo")
		{
			var args = CreateHtmlArgs(html, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
			XtraMessageBox.Show(args);
		}

		/// <summary>
		/// Cảnh báo dạng HTML (Warning).
		/// </summary>
		public static void ShowHtmlWarning(string html, string title = "Cảnh báo")
		{
			var args = CreateHtmlArgs(html, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
			XtraMessageBox.Show(args);
		}

		/// <summary>
		/// Lỗi dạng HTML (Error).
		/// </summary>
		public static void ShowHtmlError(string html, string title = "Lỗi")
		{
			var args = CreateHtmlArgs(html, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
			XtraMessageBox.Show(args);
		}

		/// <summary>
		/// Xác nhận Yes/No dạng HTML.
		/// </summary>
		public static bool GetConfirmFromYesNoHtml(string html, string title = "Xác nhận")
		{
			var args = CreateHtmlArgs(html, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			var dr = XtraMessageBox.Show(args);
			return dr == DialogResult.Yes;
		}

		/// <summary>
		/// Xác nhận Yes/No/Cancel dạng HTML.
		/// </summary>
		public static DialogResult GetConfirmFromYesNoCancelHtml(string html, string title = "Xác nhận")
		{
			var args = CreateHtmlArgs(html, title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
			return XtraMessageBox.Show(args);
		}
    }
}
