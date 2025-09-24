using System;
using System.Diagnostics;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.Xpo.Logger;

namespace Bll.Utils
{
    public static class MsgBox
    {
        private static ILogger _logger;

        public static void Configure(ILogger logger)
        {
            _logger = logger;
        }

        public static void ShowInfo(string messageText, string title = "Thông báo")
        {
            XtraMessageBox.Show(messageText, title, MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        public static void ShowError(string messageText, string title = "Lỗi")
        {
            XtraMessageBox.Show(messageText, title, MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

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

            XtraMessageBox.Show(message, title, MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        public static void ShowWarning(string messageText, string title = "Cảnh báo")
        {
            XtraMessageBox.Show(messageText, title, MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }

        public static bool GetConfirmFromYesNoDialog(string message, string title = "Xác nhận")
        {
            var result = XtraMessageBox.Show(message, title,
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return result == DialogResult.Yes;
        }

        public static DialogResult GetConfirmFromYesNoCancelDialog(string message, string title = "Xác nhận")
        {
            return XtraMessageBox.Show(message, title,
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
        }
    }
}
