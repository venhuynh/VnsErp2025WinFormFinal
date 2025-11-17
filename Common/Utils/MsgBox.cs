using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.Utils.Html;
using DevExpress.Utils.Svg;
using DevExpress.XtraEditors;

namespace Common.Utils
{
    /// <summary>
    /// Enhanced Message Box utility với HTML/CSS templates cải tiến
    /// Dựa trên DevExpress HTML Templates best practices
    /// </summary>
    public static class MsgBox
    {
        #region ========== STATIC IMAGE COLLECTION ==========

        private static SvgImageCollection _svgImages;

        /// <summary>
        /// Khởi tạo SvgImageCollection với images từ thư mục Resources
        /// Ưu tiên load từ Properties.Resources (nếu có), sau đó thử từ file system
        /// </summary>
        private static SvgImageCollection GetSvgImages()
        {
            if (_svgImages != null)
                return _svgImages;

            _svgImages = new SvgImageCollection();

            try
            {
                // Ưu tiên 1: Thử load từ Properties.Resources (nếu Close.svg đã được thêm vào Resources)
                try
                {
                    var resourcesType = typeof(Properties.Resources);
                    var closeProperty = resourcesType.GetProperty("Close",
                        BindingFlags.NonPublic |
                        BindingFlags.Static |
                        BindingFlags.Public);

                    if (closeProperty != null)
                    {
                        var svgImage = closeProperty.GetValue(null) as SvgImage;
                        if (svgImage != null)
                        {
                            _svgImages.Add("close", svgImage);
                            // Không return sớm, tiếp tục load các icon khác
                        }
                    }
                }
                catch
                {
                    // Nếu không có trong Resources, tiếp tục thử file system
                }

                // Ưu tiên 2: Load từ thư mục Resources (file system)
                // Chỉ load nếu chưa có trong collection
                if (!_svgImages.ContainsKey("close"))
                {
                    var assembly = Assembly.GetExecutingAssembly();
                    var assemblyLocation = assembly.Location;

                    if (string.IsNullOrEmpty(assemblyLocation))
                    {
                        // Fallback: sử dụng CodeBase nếu Location không có
                        var codeBase = assembly.CodeBase;
                        if (!string.IsNullOrEmpty(codeBase))
                        {
                            var uri = new UriBuilder(codeBase);
                            assemblyLocation = Uri.UnescapeDataString(uri.Path);
                        }
                    }

                    if (!string.IsNullOrEmpty(assemblyLocation))
                    {
                        var assemblyDir = Path.GetDirectoryName(assemblyLocation);

                        // Tìm thư mục Resources từ assembly directory
                        // Thử nhiều đường dẫn có thể
                        if (assemblyDir != null)
                        {
                            var possiblePaths = new[]
                            {
                                Path.Combine(assemblyDir, "Resources"),
                                Path.Combine(assemblyDir, "..", "Resources"),
                                Path.Combine(assemblyDir, "..", "..", "Resources"),
                                Path.Combine(assemblyDir, "..", "..", "..", "Resources"),
                                Path.Combine(assemblyDir, "_02_Common", "Utils", "Images"),
                                Path.Combine(assemblyDir, "Utils", "Images")
                            };

                            string resourcesPath = null;
                            foreach (var path in possiblePaths)
                            {
                                var fullPath = Path.GetFullPath(path);
                                if (Directory.Exists(fullPath))
                                {
                                    resourcesPath = fullPath;
                                    break;
                                }
                            }

                            if (resourcesPath != null)
                            {
                                // Load Close.svg từ thư mục Resources (chỉ nếu chưa có)
                                if (!_svgImages.ContainsKey("close"))
                                {
                                    var closePath = Path.Combine(resourcesPath, "Close.svg");
                                    if (File.Exists(closePath))
                                    {
                                        var svgImage = SvgImage.FromFile(closePath);
                                        _svgImages.Add("close", svgImage);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                // Nếu không load được từ file, sử dụng default hoặc bỏ qua
            }

            return _svgImages;
        }

        /// <summary>
        /// Load icon security_warningcircled2.svg từ Resources hoặc file system
        /// </summary>
        private static SvgImage LoadWarningIcon()
        {
            try
            {
                // Ưu tiên 1: Thử load từ Properties.Resources
                try
                {
                    var resourcesType = typeof(Properties.Resources);
                    var warningProperty = resourcesType.GetProperty("security_warningcircled2",
                        BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.IgnoreCase);

                    if (warningProperty != null)
                    {
                        if (warningProperty.GetValue(null) is SvgImage svgImage)
                        {
                            return svgImage;
                        }
                    }
                }
                catch
                {
                    // Tiếp tục thử file system
                }

                // Ưu tiên 2: Load từ file system
                var assembly = Assembly.GetExecutingAssembly();
                var assemblyLocation = assembly.Location;

                if (string.IsNullOrEmpty(assemblyLocation))
                {
                    var codeBase = assembly.CodeBase;
                    if (!string.IsNullOrEmpty(codeBase))
                    {
                        var uri = new UriBuilder(codeBase);
                        assemblyLocation = Uri.UnescapeDataString(uri.Path);
                    }
                }

                if (!string.IsNullOrEmpty(assemblyLocation))
                {
                    var assemblyDir = Path.GetDirectoryName(assemblyLocation);
                    if (assemblyDir != null)
                    {
                        var possiblePaths = new[]
                        {
                            Path.Combine(assemblyDir, "Resources"),
                            Path.Combine(assemblyDir, "..", "Resources"),
                            Path.Combine(assemblyDir, "..", "..", "Resources"),
                            Path.Combine(assemblyDir, "..", "..", "..", "Resources"),
                            Path.Combine(assemblyDir, "_02_Common", "Utils", "Images"),
                            Path.Combine(assemblyDir, "Utils", "Images")
                        };

                        foreach (var path in possiblePaths)
                        {
                            var fullPath = Path.GetFullPath(path);
                            if (Directory.Exists(fullPath))
                            {
                                var warningIconPath = Path.Combine(fullPath, "security_warningcircled2.svg");
                                if (File.Exists(warningIconPath))
                                {
                                    return SvgImage.FromFile(warningIconPath);
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                // Nếu không load được, trả về null
            }

            return null;
        }

        /// <summary>
        /// Load icon about.svg từ Resources hoặc file system
        /// </summary>
        private static SvgImage LoadAboutIcon()
        {
            try
            {
                // Ưu tiên 1: Thử load từ Properties.Resources
                try
                {
                    var resourcesType = typeof(Properties.Resources);
                    var aboutProperty = resourcesType.GetProperty("about",
                        BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.IgnoreCase);

                    if (aboutProperty != null)
                    {
                        if (aboutProperty.GetValue(null) is SvgImage svgImage)
                        {
                            return svgImage;
                        }
                    }
                }
                catch
                {
                    // Tiếp tục thử file system
                }

                // Ưu tiên 2: Load từ file system
                var assembly = Assembly.GetExecutingAssembly();
                var assemblyLocation = assembly.Location;

                if (string.IsNullOrEmpty(assemblyLocation))
                {
                    var codeBase = assembly.CodeBase;
                    if (!string.IsNullOrEmpty(codeBase))
                    {
                        var uri = new UriBuilder(codeBase);
                        assemblyLocation = Uri.UnescapeDataString(uri.Path);
                    }
                }

                if (!string.IsNullOrEmpty(assemblyLocation))
                {
                    var assemblyDir = Path.GetDirectoryName(assemblyLocation);
                    if (assemblyDir != null)
                    {
                        var possiblePaths = new[]
                        {
                            Path.Combine(assemblyDir, "Resources"),
                            Path.Combine(assemblyDir, "..", "Resources"),
                            Path.Combine(assemblyDir, "..", "..", "Resources"),
                            Path.Combine(assemblyDir, "..", "..", "..", "Resources"),
                            Path.Combine(assemblyDir, "_02_Common", "Utils", "Images"),
                            Path.Combine(assemblyDir, "Utils", "Images")
                        };

                        foreach (var path in possiblePaths)
                        {
                            var fullPath = Path.GetFullPath(path);
                            if (Directory.Exists(fullPath))
                            {
                                var aboutIconPath = Path.Combine(fullPath, "about.svg");
                                if (File.Exists(aboutIconPath))
                                {
                                    return SvgImage.FromFile(aboutIconPath);
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                // Nếu không load được, trả về null
            }

            return null;
        }

        /// <summary>
        /// Load icon caution.svg từ Resources hoặc file system
        /// </summary>
        private static SvgImage LoadCautionIcon()
        {
            try
            {
                // Ưu tiên 1: Thử load từ Properties.Resources
                try
                {
                    var resourcesType = typeof(Properties.Resources);
                    var cautionProperty = resourcesType.GetProperty("caution",
                        BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.IgnoreCase);

                    if (cautionProperty != null)
                    {
                        if (cautionProperty.GetValue(null) is SvgImage svgImage)
                        {
                            return svgImage;
                        }
                    }
                }
                catch
                {
                    // Tiếp tục thử file system
                }

                // Ưu tiên 2: Load từ file system
                var assembly = Assembly.GetExecutingAssembly();
                var assemblyLocation = assembly.Location;

                if (string.IsNullOrEmpty(assemblyLocation))
                {
                    var codeBase = assembly.CodeBase;
                    if (!string.IsNullOrEmpty(codeBase))
                    {
                        var uri = new UriBuilder(codeBase);
                        assemblyLocation = Uri.UnescapeDataString(uri.Path);
                    }
                }

                if (!string.IsNullOrEmpty(assemblyLocation))
                {
                    var assemblyDir = Path.GetDirectoryName(assemblyLocation);
                    if (assemblyDir != null)
                    {
                        var possiblePaths = new[]
                        {
                            Path.Combine(assemblyDir, "Resources"),
                            Path.Combine(assemblyDir, "..", "Resources"),
                            Path.Combine(assemblyDir, "..", "..", "Resources"),
                            Path.Combine(assemblyDir, "..", "..", "..", "Resources"),
                            Path.Combine(assemblyDir, "_02_Common", "Utils", "Images"),
                            Path.Combine(assemblyDir, "Utils", "Images")
                        };

                        foreach (var path in possiblePaths)
                        {
                            var fullPath = Path.GetFullPath(path);
                            if (Directory.Exists(fullPath))
                            {
                                var cautionIconPath = Path.Combine(fullPath, "caution.svg");
                                if (File.Exists(cautionIconPath))
                                {
                                    return SvgImage.FromFile(cautionIconPath);
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                // Nếu không load được, trả về null
            }

            return null;
        }

        #endregion
        #region ========== DELETE CONFIRMATION DIALOG ==========

        /// <summary>
        /// Hiển thị hộp thoại xác nhận xóa với template cải tiến
        /// </summary>
        /// <param name="message">Thông điệp xác nhận (có thể chứa HTML)</param>
        /// <param name="caption">Tiêu đề (mặc định: "Xác nhận xóa")</param>
        /// <param name="owner">Parent form (tùy chọn)</param>
        /// <param name="deleteButtonText">Text của nút xóa (mặc định: "XÓA")</param>
        /// <param name="cancelButtonText">Text của nút hủy (mặc định: "HỦY")</param>
        /// <returns>True nếu người dùng chọn Xóa, False nếu chọn Hủy</returns>
        public static bool ShowDeleteConfirmation(string message, string caption = "Xác nhận xóa",
            IWin32Window owner = null, string deleteButtonText = "XÓA", string cancelButtonText = "HỦY")
        {
            // Chuyển đổi \n thành <br> và convert HTML tags cho DevExpress
            var htmlMessage = message?.Replace("\n", "<br>") ?? string.Empty;
            htmlMessage = ConvertHtmlForDevExpress(htmlMessage);

            var template = CreateDeleteConfirmationTemplate(caption, htmlMessage, deleteButtonText, cancelButtonText);

            var args = new XtraMessageBoxArgs
            {
                Owner = owner,
                AllowHtmlText = DefaultBoolean.True,
                Buttons = new[] { DialogResult.OK, DialogResult.Cancel },
                DefaultButtonIndex = 1, // Default là Cancel (an toàn hơn)
                HtmlImages = GetSvgImages()
            };

            args.HtmlTemplate.Assign(template);
            var result = XtraMessageBox.Show(args);
            return result == DialogResult.OK;
        }

        /// <summary>
        /// Tạo HTML template cho hộp thoại xác nhận xóa
        /// </summary>
        private static HtmlTemplate CreateDeleteConfirmationTemplate(string caption, string htmlMessage, string deleteButtonText, string cancelButtonText)
        {
            try
            {
                // Escape caption nhưng giữ nguyên HTML trong message (đã được xử lý)
                var escapedCaption = HtmlEncode(caption ?? "Xác nhận xóa");

                // Template HTML cho hộp thoại xác nhận xóa
                var template = @"<div class=""frame"" id=""frame"">
    <div class=""header"">
        <div class=""caption"">" + escapedCaption + @"</div>
        <div class=""close-button"" id=""closebutton"">
            <img src=""close"" class=""close-button-img"" id=""close"">
        </div>
    </div>
    <div class=""message-text"" id=""content"">
        <div class=""message text"">" + htmlMessage + @"</div>
    </div>
    <div class=""buttons"">
        <div class=""message button"" tabindex=""1"" id=""dialogresult-ok"">" + HtmlEncode(deleteButtonText ?? "XÓA") + @"</div>
        <div class=""message button"" tabindex=""2"" id=""dialogresult-cancel"">" + HtmlEncode(cancelButtonText ?? "HỦY") + @"</div>
    </div>
</div>";

                // CSS riêng cho Delete Confirmation với @Critical
                var styles = @"body {
    padding: 20px;
    font-size: 14px;
    font-family: 'Segoe UI';
}
.frame {
    width: 450px;
    color: @ControlText;
    background-color: @Window;
    border: 1px solid @Critical;
    border-radius: 16px;
    display: flex;
    flex-direction: column;
    justify-content: center;
    box-shadow: 0px 8px 16px @Critical/0.6;
}
.header {
    padding: 8px;
    color: @White;
    background-color: @Critical;
    border-radius: 15px 15px 0px 0px;
    display: flex;
    justify-content: space-between;
    align-items: center;
}
.header-element {
    margin: 0px 10px;
}
.caption {
    margin: 0px 10px;
    font-weight: bold;
}
.close-button {
    padding: 8px;
    border-radius: 5px;
}
.close-button:hover {
    background-color: @WindowText/0.1;
}
.close-button:active {
    background-color: @ControlText/0.05;
}
.close-button-img {
    fill: White;
    width: 18px;
    height: 18px;
    opacity: 0.8;
}
.message-text {
    display: flex;
    align-items: center;
    flex-direction: column;
    padding: 10px;
    white-space: normal;
    word-wrap: break-word;
    color: @ControlText;
    text-align: center;
}
.message {
    margin: 7px;
}
.buttons {
    display: flex;
    align-items: center;
    flex-direction: column;
    padding: 10px;
}
.button {
    color: @Critical;
    padding: 8px 24px;
    border: 1px solid @Critical;
    border-radius: 5px;
    margin: 7px;
}
.button:hover {
    color: @White;
    background-color: @Critical;
    box-shadow: 0px 0px 10px @Critical/0.5;
}";

                return new HtmlTemplate(template, styles);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khi tạo DeleteConfirmationTemplate: {ex.Message}");
                return null;
            }
        }

        #endregion



        #region ========== SUCCESS DIALOG ==========

        /// <summary>
        /// Hiển thị thông báo thành công với template cải tiến
        /// </summary>
        public static void ShowSuccess(string message, string caption = "Thành công",
            IWin32Window owner = null)
        {
            // Chuyển đổi \n thành <br> và convert HTML tags cho DevExpress
            var htmlMessage = message?.Replace("\n", "<br>") ?? string.Empty;
            htmlMessage = ConvertHtmlForDevExpress(htmlMessage);

            // Tạo SvgImageCollection và load about icon
            var svgImages = GetSvgImages();

            // Load và thêm about icon vào collection nếu chưa có
            var aboutIcon = LoadAboutIcon();
            if (aboutIcon != null && !svgImages.ContainsKey("about"))
            {
                svgImages.Add("about", aboutIcon);
            }

            var template = CreateSuccessTemplate(caption, htmlMessage);

            var args = new XtraMessageBoxArgs
            {
                Owner = owner,
                AllowHtmlText = DefaultBoolean.True,
                Buttons = [DialogResult.OK],
                DefaultButtonIndex = 0,
                HtmlImages = svgImages
            };

            args.HtmlTemplate.Assign(template);
            XtraMessageBox.Show(args);
        }

        private static HtmlTemplate CreateSuccessTemplate(string caption, string htmlMessage)
        {
            try
            {
                // Escape caption nhưng giữ nguyên HTML trong message (đã được xử lý)
                var escapedCaption = HtmlEncode(caption ?? "Thành công");

                // Template HTML cho hộp thoại thành công
                var template = @"<div class=""frame"" id=""frame"">
    <div class=""header"">
        <div class=""header-left"">
            <img src=""about"" class=""success-icon"">
            <div class=""caption"">" + escapedCaption + @"</div>
        </div>
        <div class=""close-button"" id=""closebutton"">
            <img src=""close"" class=""close-button-img"" id=""close"">
        </div>
    </div>
    <div class=""content"" id=""content"">
        <div class=""message text"">" + htmlMessage + @"</div>
        <div class=""message button"" tabindex=""1"" id=""dialogresult-ok"">OK</div>
    </div>
</div>";

                // CSS riêng cho Success với @Success
                var styles = @"body {
                        padding: 20px;
                        font-size: 14px;
                        font-family: 'Segoe UI';
                    }
                    .frame {
                        width: 450px;
                        color: @ControlText;
                        background-color: @Window;
                        border: 1px solid @Success;
                        border-radius: 16px;
                        display: flex;
                        flex-direction: column;
                        justify-content: center;
                        box-shadow: 0px 8px 16px @Success/0.6;
                    }
                    .header {
                        padding: 8px;
                        color: @White;
                        background-color: @Success;
                        border-radius: 15px 15px 0px 0px;
                        display: flex;
                        justify-content: space-between;
                        align-items: center;
                    }
                    .header-left {
                        display: flex;
                        align-items: center;
                        gap: 10px;
                    }
                    .success-icon {
                        width: 24px;
                        height: 24px;
                        fill: @White;
                    }
                    .caption {
                        margin: 0px;
                        font-weight: bold;
                    }
                    .close-button {
                        padding: 8px;
                        border-radius: 5px;
                    }
                    .close-button:hover {
                        background-color: @WindowText/0.1;
                    }
                    .close-button:active {
                        background-color: @ControlText/0.05;
                    }
                    .close-button-img {
                        fill: White;
                        width: 18px;
                        height: 18px;
                        opacity: 0.8;
                    }
                    .content {
                        display: flex;
                        align-items: center;
                        flex-direction: column;
                        padding: 10px;
                    }
                    .message {
                        margin: 7px;
                    }
                    .text {
                        color: @ControlText;
                        text-align: center;
                        white-space: normal;
                        word-wrap: break-word;
                    }
                    .button {
                        color: @Success;
                        padding: 8px 24px;
                        border: 1px solid @Success;
                        border-radius: 5px;
                    }
                    .button:hover {
                        color: @White;
                        background-color: @Success;
                        box-shadow: 0px 0px 10px @Success/0.5;
                    }";

                return new HtmlTemplate(template, styles);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khi tạo SuccessTemplate: {ex.Message}");
                return null;
            }
        }

        #endregion

        #region ========== WARNING DIALOG ==========

        /// <summary>
        /// Hiển thị thông báo cảnh báo với template cải tiến
        /// </summary>
        public static void ShowWarning(string message, string caption = "Cảnh báo",
            IWin32Window owner = null)
        {
            // Chuyển đổi \n thành <br> và convert HTML tags cho DevExpress
            var htmlMessage = message?.Replace("\n", "<br>") ?? string.Empty;
            htmlMessage = ConvertHtmlForDevExpress(htmlMessage);

            // Tạo SvgImageCollection và load warning icon
            var svgImages = GetSvgImages();

            // Load và thêm warning icon vào collection nếu chưa có
            var warningIcon = LoadWarningIcon();
            if (warningIcon != null && !svgImages.ContainsKey("security_warningcircled2"))
            {
                svgImages.Add("security_warningcircled2", warningIcon);
            }

            var template = CreateWarningTemplate(caption, htmlMessage);

            var args = new XtraMessageBoxArgs
            {
                Owner = owner,
                AllowHtmlText = DefaultBoolean.True,
                Buttons = [DialogResult.OK],
                DefaultButtonIndex = 0,
                HtmlImages = svgImages
            };

            args.HtmlTemplate.Assign(template);
            XtraMessageBox.Show(args);
        }

        private static HtmlTemplate CreateWarningTemplate(string caption, string htmlMessage)
        {
            try
            {
                // Escape caption nhưng giữ nguyên HTML trong message (đã được xử lý)
                var escapedCaption = HtmlEncode(caption ?? "Cảnh báo");

                // Template HTML cho hộp thoại cảnh báo
                var template = @"<div class=""frame"" id=""frame"">
                    <div class=""header"">
                        <div class=""header-left"">
                            <img src=""security_warningcircled2"" class=""warning-icon"">
                            <div class=""caption"">" + escapedCaption + @"</div>
                        </div>
                        <div class=""close-button"" id=""closebutton"">
                            <img src=""close"" class=""close-button-img"" id=""close"">
                        </div>
                    </div>
                    <div class=""content"" id=""content"">
                        <div class=""message text"">" + htmlMessage + @"</div>
                        <div class=""message button"" tabindex=""1"" id=""dialogresult-ok"">OK</div>
                    </div>
                </div>";

                // CSS riêng cho Warning với @Warning
                var styles = @"body {
                    padding: 20px;
                    font-size: 14px;
                    font-family: 'Segoe UI';
                }
                .frame {
                    width: 450px;
                    color: @ControlText;
                    background-color: @Window;
                    border: 1px solid @Warning;
                    border-radius: 16px;
                    display: flex;
                    flex-direction: column;
                    justify-content: center;
                    box-shadow: 0px 8px 16px @Warning/0.6;
                }
                .header {
                    padding: 8px;
                    color: @White;
                    background-color: @Warning;
                    border-radius: 15px 15px 0px 0px;
                    display: flex;
                    justify-content: space-between;
                    align-items: center;
                }
                .header-left {
                    display: flex;
                    align-items: center;
                    gap: 10px;
                }
                .warning-icon {
                    width: 24px;
                    height: 24px;
                    fill: @White;
                }
                .caption {
                    margin: 0px;
                    font-weight: bold;
                }
                .close-button {
                    padding: 8px;
                    border-radius: 5px;
                }
                .close-button:hover {
                    background-color: @WindowText/0.1;
                }
                .close-button:active {
                    background-color: @ControlText/0.05;
                }
                .close-button-img {
                    fill: White;
                    width: 18px;
                    height: 18px;
                    opacity: 0.8;
                }
                .content {
                    display: flex;
                    align-items: center;
                    flex-direction: column;
                    padding: 10px;
                }
                .message {
                    margin: 7px;
                }
                .text {
                    color: @ControlText;
                    text-align: center;
                    white-space: normal;
                    word-wrap: break-word;
                }
                .button {
                    color: @Warning;
                    padding: 8px 24px;
                    border: 1px solid @Warning;
                    border-radius: 5px;
                }
                .button:hover {
                    color: @White;
                    background-color: @Warning;
                    box-shadow: 0px 0px 10px @Warning/0.5;
                }";

                return new HtmlTemplate(template, styles);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khi tạo WarningTemplate: {ex.Message}");
                return null;
            }
        }

        #endregion

        #region ========== ERROR DIALOG ==========

        /// <summary>
        /// Hiển thị thông báo lỗi với template cải tiến
        /// </summary>
        public static void ShowError(string message, string caption = "Lỗi",
            IWin32Window owner = null)
        {
            // Chuyển đổi \n thành <br> và convert HTML tags cho DevExpress
            var htmlMessage = message?.Replace("\n", "<br>") ?? string.Empty;
            htmlMessage = ConvertHtmlForDevExpress(htmlMessage);

            // Tạo SvgImageCollection và load caution icon
            var svgImages = GetSvgImages();

            // Load và thêm caution icon vào collection nếu chưa có
            var cautionIcon = LoadCautionIcon();
            if (cautionIcon != null && !svgImages.ContainsKey("caution"))
            {
                svgImages.Add("caution", cautionIcon);
            }

            var template = CreateErrorTemplate(caption, htmlMessage);

            var args = new XtraMessageBoxArgs
            {
                Owner = owner,
                AllowHtmlText = DefaultBoolean.True,
                Buttons = [DialogResult.OK],
                DefaultButtonIndex = 0,
                HtmlImages = svgImages
            };

            args.HtmlTemplate.Assign(template);
            XtraMessageBox.Show(args);
        }

        private static HtmlTemplate CreateErrorTemplate(string caption, string htmlMessage)
        {
            try
            {
                // Escape caption nhưng giữ nguyên HTML trong message (đã được xử lý)
                var escapedCaption = HtmlEncode(caption ?? "Lỗi");

                // Template HTML cho hộp thoại lỗi
                var template = @"<div class=""frame"" id=""frame"">
    <div class=""header"">
        <div class=""header-left"">
            <img src=""caution"" class=""error-icon"">
            <div class=""caption"">" + escapedCaption + @"</div>
        </div>
        <div class=""close-button"" id=""closebutton"">
            <img src=""close"" class=""close-button-img"" id=""close"">
        </div>
    </div>
    <div class=""content"" id=""content"">
        <div class=""message text"">" + htmlMessage + @"</div>
        <div class=""message button"" tabindex=""1"" id=""dialogresult-ok"">OK</div>
    </div>
</div>";

                // CSS riêng cho Error với @Critical
                var styles = @"body {
    padding: 20px;
    font-size: 14px;
    font-family: 'Segoe UI';
}
.frame {
    width: 450px;
    color: @ControlText;
    background-color: @Window;
    border: 1px solid @Critical;
    border-radius: 16px;
    display: flex;
    flex-direction: column;
    justify-content: center;
    box-shadow: 0px 8px 16px @Critical/0.6;
}
.header {
    padding: 8px;
    color: @White;
    background-color: @Critical;
    border-radius: 15px 15px 0px 0px;
    display: flex;
    justify-content: space-between;
    align-items: center;
}
.header-left {
    display: flex;
    align-items: center;
    gap: 10px;
}
.error-icon {
    width: 24px;
    height: 24px;
    fill: @White;
}
.caption {
    margin: 0px;
    font-weight: bold;
}
.close-button {
    padding: 8px;
    border-radius: 5px;
}
.close-button:hover {
    background-color: @WindowText/0.1;
}
.close-button:active {
    background-color: @ControlText/0.05;
}
.close-button-img {
    fill: White;
    width: 18px;
    height: 18px;
    opacity: 0.8;
}
.content {
    display: flex;
    align-items: center;
    flex-direction: column;
    padding: 10px;
}
.message {
    margin: 7px;
}
.text {
    color: @ControlText;
    text-align: center;
    white-space: normal;
    word-wrap: break-word;
}
.button {
    color: @Critical;
    padding: 8px 24px;
    border: 1px solid @Critical;
    border-radius: 5px;
}
.button:hover {
    color: @White;
    background-color: @Critical;
    box-shadow: 0px 0px 10px @Critical/0.5;
}";

                return new HtmlTemplate(template, styles);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khi tạo ErrorTemplate: {ex.Message}");
                return null;
            }
        }

        #endregion

        #region ========== YES/NO CONFIRMATION DIALOG ==========

        /// <summary>
        /// Hiển thị hộp thoại xác nhận Yes/No với template cải tiến
        /// </summary>
        /// <param name="message">Thông điệp xác nhận (có thể chứa HTML)</param>
        /// <param name="caption">Tiêu đề (mặc định: "Xác nhận")</param>
        /// <param name="owner">Parent form (tùy chọn)</param>
        /// <param name="yesButtonText">Text của nút Yes (mặc định: "Có")</param>
        /// <param name="noButtonText">Text của nút No (mặc định: "Không")</param>
        /// <returns>True nếu người dùng chọn Yes, False nếu chọn No</returns>
        public static bool ShowYesNo(string message, string caption = "Xác nhận",
            IWin32Window owner = null, string yesButtonText = "Có", string noButtonText = "Không")
        {
            // Chuyển đổi \n thành <br> và convert HTML tags cho DevExpress
            var htmlMessage = message?.Replace("\n", "<br>") ?? string.Empty;
            htmlMessage = ConvertHtmlForDevExpress(htmlMessage);

            var template = CreateYesNoTemplate(caption, htmlMessage, yesButtonText, noButtonText);

            var args = new XtraMessageBoxArgs
            {
                Owner = owner,
                AllowHtmlText = DefaultBoolean.True,
                Buttons = new[] { DialogResult.Yes, DialogResult.No },
                DefaultButtonIndex = 1, // Default là No (an toàn hơn)
                HtmlImages = GetSvgImages()
            };

            args.HtmlTemplate.Assign(template);
            var result = XtraMessageBox.Show(args);
            return result == DialogResult.Yes;
        }

        private static HtmlTemplate CreateYesNoTemplate(string caption, string htmlMessage, string yesButtonText, string noButtonText)
        {
            try
            {
                // Escape caption nhưng giữ nguyên HTML trong message (đã được xử lý)
                var escapedCaption = HtmlEncode(caption ?? "Xác nhận");

                // Template HTML cho hộp thoại xác nhận Yes/No
                var template = @"<div class=""frame"" id=""frame"">
    <div class=""header"">
        <div class=""caption"">" + escapedCaption + @"</div>
        <div class=""close-button"" id=""closebutton"">
            <img src=""close"" class=""close-button-img"" id=""close"">
        </div>
    </div>
    <div class=""message-text"" id=""content"">
        <div class=""message text"">" + htmlMessage + @"</div>
    </div>
    <div class=""buttons"">
        <div class=""message button"" tabindex=""1"" id=""dialogresult-yes"">" + HtmlEncode(yesButtonText ?? "Có") + @"</div>
        <div class=""message button"" tabindex=""2"" id=""dialogresult-no"">" + HtmlEncode(noButtonText ?? "Không") + @"</div>
    </div>
</div>";

                // CSS riêng cho Yes/No Confirmation với @Critical (màu giống DeleteConfirmation)
                // Format giống hệt CreateDeleteConfirmationTemplateArlerLayoutControlGroup
                var styles = @"body {
    padding: 20px;
    font-size: 14px;
    font-family: 'Segoe UI';
}
.frame {
    width: 450px;
    color: @ControlText;
    background-color: @Window;
    border: 1px solid @Critical;
    border-radius: 16px;
    display: flex;
    flex-direction: column;
    justify-content: center;
    box-shadow: 0px 8px 16px @Critical/0.6;
}
.header {
    padding: 8px;
    color: @White;
    background-color: @Critical;
    border-radius: 15px 15px 0px 0px;
    display: flex;
    justify-content: space-between;
    align-items: center;
}
.header-element {
    margin: 0px 10px;
}
.caption {
    margin: 0px 10px;
    font-weight: bold;
}
.close-button {
    padding: 8px;
    border-radius: 5px;
}
.close-button:hover {
    background-color: @WindowText/0.1;
}
.close-button:active {
    background-color: @ControlText/0.05;
}
.close-button-img {
    fill: White;
    width: 18px;
    height: 18px;
    opacity: 0.8;
}
.message-text {
    display: flex;
    align-items: center;
    flex-direction: column;
    padding: 10px;
    white-space: normal;
    word-wrap: break-word;
    color: @ControlText;
    text-align: center;
}
.message {
    margin: 7px;
}
.buttons {
    display: flex;
    align-items: center;
    flex-direction: column;
    padding: 10px;
}
.button {
    color: @Critical;
    padding: 8px 24px;
    border: 1px solid @Critical;
    border-radius: 5px;
    margin: 7px;
}
.button:hover {
    color: @White;
    background-color: @Critical;
    box-shadow: 0px 0px 10px @Critical/0.5;
}";

                return new HtmlTemplate(template, styles);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khi tạo YesNoTemplate: {ex.Message}");
                return null;
            }
        }

        #endregion

        #region ========== EXCEPTION DIALOG ==========

        /// <summary>
        /// Hiển thị thông báo lỗi từ Exception với template cải tiến
        /// </summary>
        public static void ShowException(Exception ex, string caption = "Lỗi hệ thống", IWin32Window owner = null)
        {
            var message = $"Đã xảy ra lỗi: {ex.Message}";

            // Thêm thông tin inner exception nếu có
            if (ex.InnerException != null)
            {
                message += $"\n\n<b>Chi tiết:</b> {ex.InnerException.Message}";
            }

            // Trong môi trường debug, hiển thị stack trace
#if DEBUG
            message += $"\n\n<b>Stack Trace:</b>\n<pre>{ex.StackTrace}</pre>";
#endif

            ShowError(message, caption, owner);
        }

        #endregion

        #region ========== HELPER METHODS ==========

        /// <summary>
        /// Escape HTML special characters
        /// </summary>
        private static string HtmlEncode(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            return text
                .Replace("&", "&amp;")
                .Replace("<", "&lt;")
                .Replace(">", "&gt;")
                .Replace("\"", "&quot;")
                .Replace("'", "&#39;");
        }

        /// <summary>
        /// Chuyển đổi HTML tags sang format mà DevExpress hỗ trợ
        /// DevExpress trong HtmlTemplate có thể không hỗ trợ một số tags, nên chuyển sang inline styles
        /// </summary>
        private static string ConvertHtmlForDevExpress(string html)
        {
            if (string.IsNullOrEmpty(html))
                return string.Empty;

            var result = html;

            // Chuyển <b> và <strong> thành inline style với font-weight: bold
            // Vì DevExpress có thể không render các tags này trong HtmlTemplate
            result = System.Text.RegularExpressions.Regex.Replace(
                result,
                @"<b>([^<]*)</b>",
                @"<span style=""font-weight: bold"">$1</span>",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            result = System.Text.RegularExpressions.Regex.Replace(
                result,
                @"<strong>([^<]*)</strong>",
                @"<span style=""font-weight: bold"">$1</span>",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            // Xử lý các trường hợp <b> không có closing tag hoặc nested
            result = System.Text.RegularExpressions.Regex.Replace(
                result,
                @"<b>",
                @"<span style=""font-weight: bold"">",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            result = result.Replace("</b>", "</span>");
            result = result.Replace("<strong>", "<span style=\"font-weight: bold\">");
            result = result.Replace("</strong>", "</span>");

            // Chuyển <color=...> thành <span style="color:...">
            result = System.Text.RegularExpressions.Regex.Replace(
                result,
                @"<color=([^>]+)>",
                @"<span style=""color:$1"">",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            result = result.Replace("</color>", "</span>");

            return result;
        }

        #endregion
    }
}

