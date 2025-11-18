using System;
using System.Windows.Forms;

namespace Common.Helpers;

/// <summary>
/// Helper class để quản lý việc append text thread-safe cho LogTextBox
/// </summary>
public static class LogTextBoxHelper
{
    /// <summary>
    /// Append text to LogTextBox thread-safe
    /// </summary>
    /// <param name="logTextBox">LogTextBox control</param>
    /// <param name="text">Text to append</param>
    private static void AppendText(TextBox logTextBox, string text)
    {
        if (logTextBox == null) return;

        if (logTextBox.InvokeRequired)
        {
            logTextBox.Invoke(new Action<TextBox, string>(AppendText), logTextBox, text);
        }
        else
        {
            logTextBox.AppendText(text);
        }
    }

    /// <summary>
    /// Append text to LogTextBox thread-safe với timestamp
    /// </summary>
    /// <param name="logTextBox">LogTextBox control</param>
    /// <param name="text">Text to append</param>
    /// <param name="includeTimestamp">Có bao gồm timestamp không</param>
    private static void AppendText(TextBox logTextBox, string text, bool includeTimestamp = false)
    {
        if (logTextBox == null) return;

        string finalText = text;
        if (includeTimestamp)
        {
            finalText = $"[{DateTime.Now:HH:mm:ss}] {text}";
        }

        AppendText(logTextBox, finalText);
    }

    /// <summary>
    /// Clear LogTextBox thread-safe
    /// </summary>
    /// <param name="logTextBox">LogTextBox control</param>
    public static void Clear(TextBox logTextBox)
    {
        if (logTextBox == null) return;

        if (logTextBox.InvokeRequired)
        {
            logTextBox.Invoke(new Action<TextBox>(Clear), logTextBox);
        }
        else
        {
            logTextBox.Clear();
        }
    }

    /// <summary>
    /// Set text to LogTextBox thread-safe
    /// </summary>
    /// <param name="logTextBox">LogTextBox control</param>
    /// <param name="text">Text to set</param>
    public static void SetText(TextBox logTextBox, string text)
    {
        if (logTextBox == null) return;

        if (logTextBox.InvokeRequired)
        {
            logTextBox.Invoke(new Action<TextBox, string>(SetText), logTextBox, text);
        }
        else
        {
            logTextBox.Text = text;
        }
    }

    /// <summary>
    /// Append line to LogTextBox thread-safe
    /// </summary>
    /// <param name="logTextBox">LogTextBox control</param>
    /// <param name="text">Text to append</param>
    public static void AppendLine(TextBox logTextBox, string text)
    {
        AppendText(logTextBox, text + Environment.NewLine);
    }

    /// <summary>
    /// Append line to LogTextBox thread-safe với timestamp
    /// </summary>
    /// <param name="logTextBox">LogTextBox control</param>
    /// <param name="text">Text to append</param>
    /// <param name="includeTimestamp">Có bao gồm timestamp không</param>
    private static void AppendLine(TextBox logTextBox, string text, bool includeTimestamp = false)
    {
        AppendText(logTextBox, text + Environment.NewLine, includeTimestamp);
    }

    /// <summary>
    /// Append error message to LogTextBox thread-safe
    /// </summary>
    /// <param name="logTextBox">LogTextBox control</param>
    /// <param name="message">Error message</param>
    /// <param name="exception">Exception object</param>
    public static void AppendError(TextBox logTextBox, string message, Exception exception = null)
    {
        string errorText = $"[ERROR] {message}";
        if (exception != null)
        {
            errorText += $" - {exception.Message}";
        }
        AppendLine(logTextBox, errorText, true);
    }

    /// <summary>
    /// Append info message to LogTextBox thread-safe
    /// </summary>
    /// <param name="logTextBox">LogTextBox control</param>
    /// <param name="message">Info message</param>
    public static void AppendInfo(TextBox logTextBox, string message)
    {
        AppendLine(logTextBox, $"[INFO] {message}", true);
    }

    /// <summary>
    /// Append warning message to LogTextBox thread-safe
    /// </summary>
    /// <param name="logTextBox">LogTextBox control</param>
    /// <param name="message">Warning message</param>
    public static void AppendWarning(TextBox logTextBox, string message)
    {
        AppendLine(logTextBox, $"[WARNING] {message}", true);
    }

    /// <summary>
    /// Append success message to LogTextBox thread-safe
    /// </summary>
    /// <param name="logTextBox">LogTextBox control</param>
    /// <param name="message">Success message</param>
    public static void AppendSuccess(TextBox logTextBox, string message)
    {
        AppendLine(logTextBox, $"[SUCCESS] {message}", true);
    }

    /// <summary>
    /// Initialize LogTextBox with console-like appearance
    /// </summary>
    /// <param name="logTextBox">LogTextBox control to initialize</param>
    public static void InitializeLogTextBox(TextBox logTextBox)
    {
        if (logTextBox == null) return;

        if (logTextBox.InvokeRequired)
        {
            logTextBox.Invoke(new Action<TextBox>(InitializeLogTextBox), logTextBox);
        }
        else
        {
            logTextBox.Multiline = true;
            logTextBox.ReadOnly = true;
            logTextBox.BackColor = System.Drawing.Color.Black;
            logTextBox.ForeColor = System.Drawing.Color.LimeGreen;
            logTextBox.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular);
            logTextBox.ScrollBars = ScrollBars.Both;
            logTextBox.WordWrap = false;
        }
    }

    /// <summary>
    /// Auto-scroll to bottom of LogTextBox
    /// </summary>
    /// <param name="logTextBox">LogTextBox control</param>
    private static void ScrollToBottom(TextBox logTextBox)
    {
        if (logTextBox == null) return;

        if (logTextBox.InvokeRequired)
        {
            logTextBox.Invoke(new Action<TextBox>(ScrollToBottom), logTextBox);
        }
        else
        {
            logTextBox.SelectionStart = logTextBox.Text.Length;
            logTextBox.ScrollToCaret();
        }
    }

    /// <summary>
    /// Append text and auto-scroll to bottom
    /// </summary>
    /// <param name="logTextBox">LogTextBox control</param>
    /// <param name="text">Text to append</param>
    /// <param name="autoScroll">Auto scroll to bottom after append</param>
    public static void AppendTextWithScroll(TextBox logTextBox, string text, bool autoScroll = true)
    {
        AppendText(logTextBox, text);
        if (autoScroll)
        {
            ScrollToBottom(logTextBox);
        }
    }

    /// <summary>
    /// Append line and auto-scroll to bottom
    /// </summary>
    /// <param name="logTextBox">LogTextBox control</param>
    /// <param name="text">Text to append</param>
    /// <param name="includeTimestamp">Include timestamp</param>
    /// <param name="autoScroll">Auto scroll to bottom after append</param>
    public static void AppendLineWithScroll(TextBox logTextBox, string text, bool includeTimestamp = false, bool autoScroll = true)
    {
        AppendLine(logTextBox, text, includeTimestamp);
        if (autoScroll)
        {
            ScrollToBottom(logTextBox);
        }
    }
}