using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.Utils.Html;
using DevExpress.Utils.Svg;
using DevExpress.XtraBars.Alerter;

namespace Common.Utils
{
    /// <summary>
    /// Utility class để hiển thị toast notifications (Alert) với HTML templates
    /// Tương tự AlertControl của DevExpress, nhưng đơn giản hóa và dễ sử dụng hơn
    /// </summary>
    public static class AlertHelper
    {
        #region ========== STATIC FIELDS ==========

        private static AlertControl _alertControl;
        private static SvgImageCollection _svgImages;

        #endregion

        #region ========== INITIALIZATION ==========

        /// <summary>
        /// Khởi tạo AlertControl với cấu hình mặc định
        /// </summary>
        private static AlertControl GetAlertControl()
        {
            if (_alertControl == null)
            {
                _alertControl = new AlertControl
                {
                    AutoFormDelay = 5000, // 5 giây
                    FormDisplaySpeed = AlertFormDisplaySpeed.Fast,
                    FormLocation = AlertFormLocation.BottomRight,
                    FormShowingEffect = AlertFormShowingEffect.SlideHorizontal,
                    FormMaxCount = 5,
                    HtmlImages = GetSvgImages() // Set HtmlImages vào AlertControl
                };
                
                // Xử lý click events
                _alertControl.HtmlElementMouseClick += AlertControl_HtmlElementMouseClick;
                // Xử lý BeforeFormShow để set HtmlTemplate
                _alertControl.BeforeFormShow += AlertControl_BeforeFormShow;
            }
            return _alertControl;
        }
        
        // Lưu template tạm thời để set trong BeforeFormShow
        // Sử dụng queue để hỗ trợ nhiều alerts đồng thời
        private static readonly Queue<HtmlTemplate> TemplateQueue = new();

        /// <summary>
        /// Xử lý click events trên alert
        /// </summary>
        private static void AlertControl_HtmlElementMouseClick(object sender, AlertHtmlElementMouseEventArgs e)
        {
            // Đóng alert khi click vào nút close
            if (e.ElementId == "closeButton" || e.ParentHasId("closeButton"))
            {
                e.HtmlPopup.Close();
            }
        }

        /// <summary>
        /// Xử lý BeforeFormShow để set HtmlTemplate
        /// </summary>
        private static void AlertControl_BeforeFormShow(object sender, AlertFormEventArgs e)
        {
            try
            {
                // Kiểm tra null để tránh NullReferenceException
                if (e?.HtmlPopup?.HtmlTemplate == null)
                {
                    return;
                }

                // Lấy template từ queue (FIFO)
                if (TemplateQueue.Count > 0)
                {
                    var template = TemplateQueue.Dequeue();
                    if (template != null)
                    {
                        e.HtmlPopup.HtmlTemplate.Assign(template);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log lỗi nhưng không throw để không làm crash ứng dụng
                System.Diagnostics.Debug.WriteLine($"Lỗi trong AlertControl_BeforeFormShow: {ex.Message}");
            }
        }

        /// <summary>
        /// Khởi tạo SvgImageCollection với images từ Resources
        /// Ưu tiên load từ Properties.Resources (nếu có), sau đó thử từ file system (theo pattern MsgBox.cs)
        /// </summary>
        private static SvgImageCollection GetSvgImages()
        {
            if (_svgImages != null)
                return _svgImages;

            _svgImages = new SvgImageCollection();
            
            try
            {
                // Ưu tiên 1: Thử load từ Properties.Resources (theo pattern MsgBox.cs)
                try
                {
                    var resourcesType = typeof(Properties.Resources);
                    var closeProperty = resourcesType.GetProperty("Close", 
                        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
                    
                    if (closeProperty != null)
                    {
                        var svgImage = closeProperty.GetValue(null) as SvgImage;
                        if (svgImage != null)
                        {
                            // Thêm với key "close" cho MsgBox
                            _svgImages.Add("close", svgImage);
                            // Thêm với key "message_close" cho AlertHelper template (theo pattern DevExpress demo)
                            _svgImages.Add("message_close", svgImage);
                        }
                    }
                }
                catch
                {
                    // Nếu không có trong Resources, tiếp tục thử file system
                }
                
                // Ưu tiên 2: Load từ thư mục Resources (file system) - chỉ nếu chưa load được từ Resources
                if (!_svgImages.ContainsKey("message_close"))
                {
                    var assembly = System.Reflection.Assembly.GetExecutingAssembly();
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
                        var assemblyDir = System.IO.Path.GetDirectoryName(assemblyLocation);
                        if (assemblyDir != null)
                        {
                            var possiblePaths = new[]
                            {
                                System.IO.Path.Combine(assemblyDir, "Resources"),
                                System.IO.Path.Combine(assemblyDir, "..", "Resources"),
                                System.IO.Path.Combine(assemblyDir, "..", "..", "Resources"),
                                System.IO.Path.Combine(assemblyDir, "_02_Common", "Utils", "Images")
                            };
                        
                            foreach (var path in possiblePaths)
                            {
                                var fullPath = System.IO.Path.GetFullPath(path);
                                if (System.IO.Directory.Exists(fullPath))
                                {
                                    // Load các SVG icons cần thiết
                                    var closePath = System.IO.Path.Combine(fullPath, "Close.svg");
                                    if (System.IO.File.Exists(closePath))
                                    {
                                        var closeImage = SvgImage.FromFile(closePath);
                                        // Thêm với key "close" cho MsgBox
                                        if (!_svgImages.ContainsKey("close"))
                                        {
                                            _svgImages.Add("close", closeImage);
                                        }
                                        // Thêm với key "message_close" cho AlertHelper template (theo pattern DevExpress demo)
                                        if (!_svgImages.ContainsKey("message_close"))
                                        {
                                            _svgImages.Add("message_close", closeImage);
                                        }
                                    }
                                
                                    // Load check icon cho Success alert
                                    var checkPath = System.IO.Path.Combine(fullPath, "check.svg");
                                    if (System.IO.File.Exists(checkPath) && !_svgImages.ContainsKey("check"))
                                    {
                                        var checkImage = SvgImage.FromFile(checkPath);
                                        _svgImages.Add("check", checkImage);
                                    }
                                
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                // Nếu không load được, sử dụng default
            }

            return _svgImages;
        }

        #endregion

        #region ========== PUBLIC METHODS ==========

        /// <summary>
        /// Hiển thị alert thông báo thành công với template và CSS riêng
        /// </summary>
        /// <param name="message">Nội dung thông báo</param>
        /// <param name="caption">Tiêu đề (tùy chọn)</param>
        /// <param name="owner">Form owner (tùy chọn)</param>
        /// <param name="autoCloseDelay">Thời gian tự động đóng (ms, mặc định 3000)</param>
        public static void ShowSuccess(string message, string caption = "Thành công", 
            IWin32Window owner = null, int autoCloseDelay = 3000)
        {
            try
            {
                // Xử lý message trước khi set vào AlertInfo
                var htmlMessage = message?.Replace("\n", "<br>") ?? string.Empty;
                htmlMessage = ConvertHtmlForDevExpress(htmlMessage);

                // Tạo SvgImageCollection và load icons (theo pattern MsgBox.cs)
                var svgImages = GetSvgImages();
                
                // Load và thêm icon about.svg vào collection nếu chưa có (giống MsgBox.cs)
                var aboutIcon = LoadAboutIcon();
                if (aboutIcon != null && !svgImages.ContainsKey("about"))
                {
                    svgImages.Add("about", aboutIcon);
                }

                // Tạo template riêng cho Success với CSS đặc biệt
                var template = CreateSuccessTemplate();
                if (template != null)
                {
                    // Set template name và tag (theo pattern DevExpress demo)
                    template.Name = "SuccessTemplate";
                    template.Tag = "Success";
                }

                // Tạo AlertControl riêng cho Success với cấu hình đặc biệt (theo pattern DevExpress demo)
                var alertControl = new AlertControl
                {
                    FormLocation = AlertFormLocation.BottomRight,
                    FormShowingEffect = AlertFormShowingEffect.SlideVertical,
                    FormDisplaySpeed = AlertFormDisplaySpeed.Moderate,
                    AutoFormDelay = autoCloseDelay,
                    FormMaxCount = 0, // Không giới hạn số lượng alert
                    HtmlImages = svgImages, // Gán SvgImageCollection đã có check icon
                    AllowHtmlText = true // Cho phép hiển thị HTML
                };

                // Thêm template vào HtmlTemplates collection (theo pattern DevExpress demo)
                if (template != null)
                {
                    alertControl.HtmlTemplates.AddRange([template]);
                }

                // Subscribe BeforeFormShow để set template (theo pattern DevExpress demo)
                alertControl.BeforeFormShow += (_, e) =>
                {
                    if (template != null && e?.HtmlPopup?.HtmlTemplate != null)
                    {
                        e.HtmlPopup.HtmlTemplate.Assign(template);
                    }
                    
                    // Cho phép hiển thị HTML trong AlertControl (theo tài liệu DevExpress)
                    // Set AllowHtmlText trên AlertForm để ${Text} render HTML
                    if (e?.AlertForm != null)
                    {
                        try
                        {
                            e.AlertForm.AllowHtmlText = true;
                        }
                        catch
                        {
                            // Nếu không có AllowHtmlText property, tiếp tục
                        }
                    }
                };

                // Subscribe click event để đóng alert (theo pattern DevExpress demo)
                alertControl.HtmlElementMouseClick += (_, e) =>
                {
                    if (e.ElementId == "closeButton" || e.ParentHasId("closeButton"))
                    {
                        e.HtmlPopup.Close();
                    }
                };

                // Tạo AlertInfo (theo pattern DevExpress demo: GetCurrentAlertInfo)
                var alertInfo = new AlertInfo(caption, htmlMessage)
                {
                    AutoCloseFormOnClick = true
                };
                
                // Set HtmlText để cho phép hiển thị HTML (thay vì Text)
                // DevExpress sẽ tự động sử dụng HtmlText nếu có, nếu không sẽ dùng Text
                try
                {
                    var htmlTextProperty = typeof(AlertInfo).GetProperty("HtmlText", 
                        System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                    if (htmlTextProperty != null && htmlTextProperty.CanWrite)
                    {
                        htmlTextProperty.SetValue(alertInfo, htmlMessage);
                    }
                }
                catch
                {
                    // Nếu không có HtmlText property, tiếp tục sử dụng Text
                }

                // Xác định form owner
                var ownerForm = GetOwnerForm(owner);

                alertControl.Show(ownerForm, alertInfo);
            }
            catch (Exception)
            {
                // Fallback: Sử dụng MessageBox nếu AlertControl lỗi
                MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Hiển thị alert cảnh báo với template và CSS riêng
        /// </summary>
        public static void ShowWarning(string message, string caption = "Cảnh báo", 
            IWin32Window owner = null, int autoCloseDelay = 5000)
        {
            ShowTypedAlert(message, caption, AlertType.Warning, owner, autoCloseDelay);
        }

        /// <summary>
        /// Hiển thị alert thông tin với template và CSS riêng
        /// </summary>
        public static void ShowInfo(string message, string caption = "Thông tin", 
            IWin32Window owner = null, int autoCloseDelay = 5000)
        {
            ShowTypedAlert(message, caption, AlertType.Information, owner, autoCloseDelay);
        }

        /// <summary>
        /// Hiển thị alert lỗi với template và CSS riêng
        /// </summary>
        public static void ShowError(string message, string caption = "Lỗi", 
            IWin32Window owner = null, int autoCloseDelay = 7000)
        {
            ShowTypedAlert(message, caption, AlertType.Error, owner, autoCloseDelay);
        }

        /// <summary>
        /// Hiển thị alert với template riêng cho từng loại (theo pattern ShowSuccess)
        /// </summary>
        private static void ShowTypedAlert(string message, string caption, AlertType alertType, 
            IWin32Window owner = null, int autoCloseDelay = 5000)
        {
            try
            {
                // Xử lý message trước khi set vào AlertInfo
                var htmlMessage = message?.Replace("\n", "<br>") ?? string.Empty;
                htmlMessage = ConvertHtmlForDevExpress(htmlMessage);

                // Tạo SvgImageCollection và load icons
                var svgImages = GetSvgImages();
                
                // Load icon tương ứng với alertType
                var icon = LoadAlertIcon(alertType);
                var iconKey = GetIconKey(alertType);
                if (icon != null && !svgImages.ContainsKey(iconKey))
                {
                    svgImages.Add(iconKey, icon);
                }

                // Tạo template riêng cho từng loại alert
                var template = CreateTypedTemplate(alertType);
                if (template != null)
                {
                    template.Name = $"{alertType}Template";
                    template.Tag = alertType.ToString();
                }

                // Tạo AlertControl với cấu hình đặc biệt
                var alertControl = new AlertControl
                {
                    FormLocation = AlertFormLocation.BottomRight,
                    FormShowingEffect = AlertFormShowingEffect.SlideVertical,
                    FormDisplaySpeed = AlertFormDisplaySpeed.Moderate,
                    AutoFormDelay = autoCloseDelay,
                    FormMaxCount = 0,
                    HtmlImages = svgImages,
                    AllowHtmlText = true // Cho phép hiển thị HTML
                };

                // Thêm template vào HtmlTemplates collection
                if (template != null)
                {
                    alertControl.HtmlTemplates.AddRange([template]);
                }

                // Subscribe BeforeFormShow để set template
                alertControl.BeforeFormShow += (sender, e) =>
                {
                    if (template != null && e?.HtmlPopup?.HtmlTemplate != null)
                    {
                        e.HtmlPopup.HtmlTemplate.Assign(template);
                    }
                    
                    // Cho phép hiển thị HTML trong AlertControl (theo tài liệu DevExpress)
                    // Set AllowHtmlText trên AlertForm để ${Text} render HTML
                    if (e?.AlertForm != null)
                    {
                        try
                        {
                            e.AlertForm.AllowHtmlText = true;
                        }
                        catch
                        {
                            // Nếu không có AllowHtmlText property, tiếp tục
                        }
                    }
                };

                // Subscribe click event để đóng alert
                alertControl.HtmlElementMouseClick += (sender, e) =>
                {
                    if (e.ElementId == "closeButton" || e.ParentHasId("closeButton"))
                    {
                        e.HtmlPopup.Close();
                    }
                };

                // Tạo AlertInfo
                var alertInfo = new AlertInfo(caption, htmlMessage)
                {
                    AutoCloseFormOnClick = true
                };
                
                // Set HtmlText để cho phép hiển thị HTML (thay vì Text)
                // DevExpress sẽ tự động sử dụng HtmlText nếu có, nếu không sẽ dùng Text
                try
                {
                    var htmlTextProperty = typeof(AlertInfo).GetProperty("HtmlText", 
                        System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                    if (htmlTextProperty != null && htmlTextProperty.CanWrite)
                    {
                        htmlTextProperty.SetValue(alertInfo, htmlMessage);
                    }
                }
                catch
                {
                    // Nếu không có HtmlText property, tiếp tục sử dụng Text
                }

                // Xác định form owner
                var ownerForm = GetOwnerForm(owner);

                alertControl.Show(ownerForm, alertInfo);
            }
            catch (Exception)
            {
                // Fallback: Sử dụng MessageBox nếu AlertControl lỗi
                MessageBox.Show(message, caption, MessageBoxButtons.OK, 
                    alertType == AlertType.Error ? MessageBoxIcon.Error :
                    alertType == AlertType.Warning ? MessageBoxIcon.Warning :
                    MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Hiển thị alert tùy chỉnh (giữ lại cho backward compatibility)
        /// </summary>
        /// <param name="message">Nội dung thông báo</param>
        /// <param name="caption">Tiêu đề</param>
        /// <param name="alertType">Loại alert</param>
        /// <param name="owner">Form owner</param>
        /// <param name="autoCloseDelay">Thời gian tự động đóng (ms)</param>
        private static void ShowAlert(string message, string caption, AlertType alertType, 
            IWin32Window owner = null, int autoCloseDelay = 5000)
        {
            try
            {
                var alertControl = GetAlertControl();
                alertControl.AutoFormDelay = autoCloseDelay;

                // Xử lý message trước khi set vào AlertInfo (theo pattern DevExpress)
                // Chuyển đổi \n thành <br> và convert HTML tags cho DevExpress
                var htmlMessage = message?.Replace("\n", "<br>") ?? string.Empty;
                htmlMessage = ConvertHtmlForDevExpress(htmlMessage);

                // Tạo AlertInfo với message đã được xử lý HTML
                // DevExpress sẽ tự động thay thế ${Caption} và ${Text} từ AlertInfo
                var alertInfo = new AlertInfo(caption, htmlMessage)
                {
                    AutoCloseFormOnClick = true,
                };
                
                // Set HtmlText để cho phép hiển thị HTML (thay vì Text)
                // DevExpress sẽ tự động sử dụng HtmlText nếu có, nếu không sẽ dùng Text
                try
                {
                    var htmlTextProperty = typeof(AlertInfo).GetProperty("HtmlText", 
                        System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                    if (htmlTextProperty != null && htmlTextProperty.CanWrite)
                    {
                        htmlTextProperty.SetValue(alertInfo, htmlMessage);
                    }
                }
                catch
                {
                    // Nếu không có HtmlText property, tiếp tục sử dụng Text
                }

                // Set icon dựa trên alertType (DevExpress sẽ thay thế ${SvgImage})
                SetAlertIcon(alertInfo, alertType);
                
                // Tạo HTML template với placeholders ${Caption}, ${Text}, ${SvgImage}
                // Template sẽ được set trong BeforeFormShow event
                var template = CreateAlertTemplate(alertType);
                if (template != null)
                {
                    TemplateQueue.Enqueue(template);
                }

                // Xác định form owner
                Form ownerForm = null;
                if (owner != null)
                {
                    ownerForm = owner as Form;
                    if (ownerForm == null)
                    {
                        var control = Control.FromHandle(owner.Handle);
                        ownerForm = control as Form;
                        if (ownerForm == null && control != null)
                        {
                            // Tìm form cha
                            ownerForm = control.FindForm();
                        }
                    }
                }
                else
                {
                    ownerForm = Form.ActiveForm;
                    if (ownerForm == null && Application.OpenForms.Count > 0)
                    {
                        var firstForm = Application.OpenForms[0];
                        ownerForm = (Form)firstForm;
                    }
                }

                alertControl.Show(ownerForm, alertInfo);
            }
            catch (Exception)
            {
                // Fallback: Sử dụng MessageBox nếu AlertControl lỗi
                MessageBox.Show(message, caption, MessageBoxButtons.OK, 
                    alertType == AlertType.Error ? MessageBoxIcon.Error :
                    alertType == AlertType.Warning ? MessageBoxIcon.Warning :
                    MessageBoxIcon.Information);
            }
        }

        #endregion

        #region ========== TEMPLATE CREATION ==========

        /// <summary>
        /// Tạo HTML template riêng cho từng loại alert với CSS đặc biệt
        /// </summary>
        private static HtmlTemplate CreateTypedTemplate(AlertType alertType)
        {
            return alertType switch
            {
                AlertType.Success => CreateSuccessTemplate(),
                AlertType.Warning => CreateWarningTemplate(),
                AlertType.Error => CreateErrorTemplate(),
                _ => CreateInfoTemplate()
            };
        }

        /// <summary>
        /// Tạo HTML template riêng cho Success alert với CSS đặc biệt
        /// </summary>
        private static HtmlTemplate CreateSuccessTemplate()
        {
            try
            {
                // Template HTML với icon check (theo pattern MsgBox.cs)
                // Sử dụng key "check" thay vì placeholder ${SvgImage}
                var template = @"<div class=""container"">
                                <div class=""popup"">
                                    <div class=""stripe""></div>
                                    <div class=""content"">
                                        <div class=""header"">
                                            <div class=""header-left"">
                                                <div class=""icon-container"">
                                                    <img class=""icon"" src=""about"">
                                                </div>
                                                <div class=""caption"">${Caption}</div>
                                            </div>
                                            <div id=""closeButton"" class=""close-button"">
                                                <img class=""close-icon"" src=""message_close"">
                                            </div>
                                        </div>
                                        <div class=""message"">
                                            <div class=""text"">${Text}</div>
                                        </div>
                                    </div>
                                </div>
                            </div>";

                // CSS riêng cho Success với @Success (tham khảo MsgBox.cs)
                var styles = @".container {
                    width: 378px;
                    height: auto;
                    padding: 7px 12px 12px 7px;
                }
                .popup {
                    background-color: @Window/0.95;
                    border-radius: 6px;
                    border-style: solid;
                    border-width: 1px 1px 1px 0px;
                    box-shadow: 2px 2px 12px @Black/0.2;
                    border-color: @Black/0.3;
                    display: flex;
                    flex-direction: row;
                }
                .content {
                    width: 100%;
                    display: flex;
                    flex-direction: column;
                    background-color: @Black/0.015;
                }
                .stripe {
                    width: 3px;
                    background-color: @Success;
                    height: 100%;
                    border-radius: 6px 0px 0px 6px;
                }
                .header {
                    padding: 8px;
                    color: @White;
                    background-color: @Success;
                    border-radius: 0px 6px 0px 0px;
                    display: flex;
                    flex-direction: row;
                    justify-content: space-between;
                    align-items: center;
                }
                .header-left {
                    display: flex;
                    flex-direction: row;
                    align-items: center;
                    gap: 8px;
                }
                .message {
                    display: flex;
                    flex-direction: column;
                    padding: 8px;
                    font-family: 'Segoe UI';
                    color: @Success;
                }
                .icon-container {
                    padding: 0px;
                }
                .icon {
                    width: 22px;
                    height: 22px;
                    fill: @White;
                }
                .caption {
                    font-size: 11pt;
                    font-weight: bold;
                    padding: 0px;
                    color: @White;
                }
                .text {
                    font-size: 10.5pt;
                    padding: 0px 6px 6px 6px;
                }
                .close-button {
                    padding: 4px;
                    border-radius: 4px;
                }
                .close-button:hover {
                    background-color: @WindowText/0.1;
                }
                .close-button:active {
                    background-color: @ControlText/0.05;
                }
                .close-icon {
                    width: 18px;
                    height: 18px;
                    opacity: 0.8;
                }";

                return new HtmlTemplate(template, styles);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khi tạo SuccessTemplate: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Tạo HTML template riêng cho Warning alert với CSS đặc biệt
        /// </summary>
        private static HtmlTemplate CreateWarningTemplate()
        {
            try
            {
                var template = @"<div class=""container"">
    <div class=""popup"">
        <div class=""stripe""></div>
        <div class=""content"">
            <div class=""header"">
                <div class=""header-left"">
                    <div class=""icon-container"">
                        <img class=""icon"" src=""warning"">
                    </div>
                    <div class=""caption"">${Caption}</div>
                </div>
                <div id=""closeButton"" class=""close-button"">
                    <img class=""close-icon"" src=""message_close"">
                </div>
            </div>
            <div class=""message"">
                <div class=""text"">${Text}</div>
            </div>
        </div>
    </div>
</div>";

                // CSS riêng cho Warning với @Warning (tham khảo MsgBox.cs)
                var styles = @".container {
    width: 378px;
    height: auto;
    padding: 7px 12px 12px 7px;
}
.popup {
    background-color: @Window/0.95;
    border-radius: 6px;
    border-style: solid;
    border-width: 1px 1px 1px 0px;
    box-shadow: 2px 2px 12px @Black/0.2;
    border-color: @Black/0.3;
    display: flex;
    flex-direction: row;
}
.content {
    width: 100%;
    display: flex;
    flex-direction: column;
    background-color: @Black/0.015;
}
.stripe {
    width: 3px;
    background-color: @Warning;
    height: 100%;
    border-radius: 6px 0px 0px 6px;
}
.header {
    padding: 8px;
    color: @White;
    background-color: @Warning;
    border-radius: 0px 6px 0px 0px;
    display: flex;
    flex-direction: row;
    justify-content: space-between;
    align-items: center;
}
.header-left {
    display: flex;
    flex-direction: row;
    align-items: center;
    gap: 8px;
}
.message {
    display: flex;
    flex-direction: column;
    padding: 8px;
    font-family: 'Segoe UI';
    color: @Warning;
}
.icon-container {
    padding: 0px;
}
.icon {
    width: 22px;
    height: 22px;
    fill: @White;
}
.caption {
    font-size: 11pt;
    font-weight: bold;
    padding: 0px;
    color: @White;
}
.text {
    font-size: 10.5pt;
    padding: 0px 6px 6px 6px;
}
.close-button {
    padding: 4px;
    border-radius: 4px;
}
.close-button:hover {
    background-color: @WindowText/0.1;
}
.close-button:active {
    background-color: @ControlText/0.05;
}
.close-icon {
    width: 18px;
    height: 18px;
    opacity: 0.8;
}";

                return new HtmlTemplate(template, styles);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khi tạo WarningTemplate: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Tạo HTML template riêng cho Error alert với CSS đặc biệt
        /// </summary>
        private static HtmlTemplate CreateErrorTemplate()
        {
            try
            {
                var template = @"<div class=""container"">
    <div class=""popup"">
        <div class=""stripe""></div>
        <div class=""content"">
            <div class=""header"">
                <div class=""header-left"">
                    <div class=""icon-container"">
                        <img class=""icon"" src=""error"">
                    </div>
                    <div class=""caption"">${Caption}</div>
                </div>
                <div id=""closeButton"" class=""close-button"">
                    <img class=""close-icon"" src=""message_close"">
                </div>
            </div>
            <div class=""message"">
                <div class=""text"">${Text}</div>
            </div>
        </div>
    </div>
</div>";

                // CSS riêng cho Error với @Critical (tham khảo MsgBox.cs)
                var styles = @".container {
    width: 378px;
    height: auto;
    padding: 7px 12px 12px 7px;
}
.popup {
    background-color: @Window/0.95;
    border-radius: 6px;
    border-style: solid;
    border-width: 1px 1px 1px 0px;
    box-shadow: 2px 2px 12px @Black/0.2;
    border-color: @Black/0.3;
    display: flex;
    flex-direction: row;
}
.content {
    width: 100%;
    display: flex;
    flex-direction: column;
    background-color: @Black/0.015;
}
.stripe {
    width: 3px;
    background-color: @Critical;
    height: 100%;
    border-radius: 6px 0px 0px 6px;
}
.header {
    padding: 8px;
    color: @White;
    background-color: @Critical;
    border-radius: 0px 6px 0px 0px;
    display: flex;
    flex-direction: row;
    justify-content: space-between;
    align-items: center;
}
.header-left {
    display: flex;
    flex-direction: row;
    align-items: center;
    gap: 8px;
}
.message {
    display: flex;
    flex-direction: column;
    padding: 8px;
    font-family: 'Segoe UI';
    color: @WindowText;
}
.icon-container {
    padding: 0px;
}
.icon {
    width: 22px;
    height: 22px;
    fill: @White;
}
.caption {
    font-size: 11pt;
    font-weight: bold;
    padding: 0px;
    color: @White;
}
.text {
    font-size: 10.5pt;
    padding: 0px 6px 6px 6px;
}
.close-button {
    padding: 4px;
    border-radius: 4px;
}
.close-button:hover {
    background-color: @WindowText/0.1;
}
.close-button:active {
    background-color: @ControlText/0.05;
}
.close-icon {
    width: 18px;
    height: 18px;
    opacity: 0.8;
}";

                return new HtmlTemplate(template, styles);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khi tạo ErrorTemplate: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Tạo HTML template riêng cho Info alert với CSS đặc biệt
        /// </summary>
        private static HtmlTemplate CreateInfoTemplate()
        {
            try
            {
                var template = @"<div class=""container"">
    <div class=""popup"">
        <div class=""stripe""></div>
        <div class=""content"">
            <div class=""header"">
                <div class=""header-left"">
                    <div class=""icon-container"">
                        <img class=""icon"" src=""info"">
                    </div>
                    <div class=""caption"">${Caption}</div>
                </div>
                <div id=""closeButton"" class=""close-button"">
                    <img class=""close-icon"" src=""message_close"">
                </div>
            </div>
            <div class=""message"">
                <div class=""text"">${Text}</div>
            </div>
        </div>
    </div>
</div>";

                // CSS riêng cho Info với @Info (tham khảo MsgBox.cs)
                var styles = @".container {
    width: 378px;
    height: auto;
    padding: 7px 12px 12px 7px;
}
.popup {
    background-color: @Window/0.95;
    border-radius: 6px;
    border-style: solid;
    border-width: 1px 1px 1px 0px;
    box-shadow: 2px 2px 12px @Black/0.2;
    border-color: @Black/0.3;
    display: flex;
    flex-direction: row;
}
.content {
    width: 100%;
    display: flex;
    flex-direction: column;
    background-color: @Black/0.015;
}
.stripe {
    width: 3px;
    background-color: @Success;
    height: 100%;
    border-radius: 6px 0px 0px 6px;
}
.header {
    padding: 8px;
    color: @White;
    background-color: @Success;
    border-radius: 0px 6px 0px 0px;
    display: flex;
    flex-direction: row;
    justify-content: space-between;
    align-items: center;
}
.header-left {
    display: flex;
    flex-direction: row;
    align-items: center;
    gap: 8px;
}
.message {
    display: flex;
    flex-direction: column;
    padding: 8px;
    font-family: 'Segoe UI';
    color: @Hyperlink;
}
.icon-container {
    padding: 0px;
}
.icon {
    width: 22px;
    height: 22px;
    fill: @White;
}
.caption {
    font-size: 11pt;
    font-weight: bold;
    padding: 0px;
    color: @White;
}
.text {
    font-size: 10.5pt;
    padding: 0px 6px 6px 6px;
}
.close-button {
    padding: 4px;
    border-radius: 4px;
}
.close-button:hover {
    background-color: @WindowText/0.1;
}
.close-button:active {
    background-color: @ControlText/0.05;
}
.close-icon {
    width: 18px;
    height: 18px;
    opacity: 0.8;
}";

                return new HtmlTemplate(template, styles);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khi tạo InfoTemplate: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Tạo HTML template cho alert dựa trên loại
        /// Sử dụng placeholders ${Caption}, ${Text}, ${SvgImage} như trong template mẫu DevExpress
        /// </summary>
        /// <param name="alertType">Loại alert</param>
        private static HtmlTemplate CreateAlertTemplate(AlertType alertType)
        {
            try
            {
                var stripeColor = GetStripeColor(alertType);
                
                // Template HTML theo đúng mẫu của DevExpress với placeholders
                // DevExpress sẽ tự động thay thế ${Caption}, ${Text}, ${SvgImage} từ AlertInfo
                var template = @"<div class=""container"">
    <div class=""popup"">
        <div class=""stripe""></div>
        <div class=""content"">
            <div class=""icon-container"">
                <img class=""icon"" src='${SvgImage}'>
            </div>
            <div class=""message"">
                <div class=""caption"">${Caption}</div>
                <div class=""text"">${Text}</div>
            </div>
            <div id=""closeButton"" class=""close-button"">
                <img class=""close-icon"" src=""message_close"">
            </div>
        </div>
    </div>
</div>";

                // CSS theo đúng mẫu của DevExpress (không có body style)
                var styles = $@".container {{
    width: 378px;
    height: auto;
    padding: 7px 12px 12px 7px;
}}
.popup {{
    background-color: @Window/0.95;
    border-radius: 6px;
    border-style: solid;
    border-width: 1px 1px 1px 0px;
    box-shadow: 2px 2px 12px @Black/0.2;
    border-color: @Black/0.3;
    display: flex;
    flex-direction: row;
}}
.content {{
    width: 100%;
    display: flex;
    flex-direction: row;
    align-items: center;
    background-color: @Black/0.015;
}}
.stripe {{
    width: 3px;
    background-color: {stripeColor};
    height: 100%;
    border-radius: 6px 0px 0px 6px;
}}
.message {{
    display: flex;
    flex-direction: column;
    padding: 8px;
    font-family: 'Segoe UI';
    color: @WindowText;
}}
.icon-container {{
    padding: 8px;
}}
.icon {{
    width: 22px;
    height: 22px;
}}
.caption {{
    font-size: 11pt;
    font-weight: bold;
    padding: 6px;
}}
.text {{
    font-size: 10.5pt;
    padding: 0px 6px 6px 6px;
}}
.close-button {{
    padding: 8px;
    border-radius: 4px 0px 0px 4px;
}}
.close-button:hover {{
    background-color: @Black/0.1;
}}
.close-button:active {{
    background-color: @Black/0.2;
}}
.close-icon {{
    width: 22px;
    height: 22px;
}}";

                return new HtmlTemplate(template, styles);
            }
            catch (Exception ex)
            {
                // Log lỗi và trả về null để fallback về MessageBox
                System.Diagnostics.Debug.WriteLine($"Lỗi khi tạo HtmlTemplate: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Lấy color token dựa trên loại alert
        /// </summary>
        private static string GetColorToken(AlertType alertType)
        {
            return alertType switch
            {
                AlertType.Success => "@Success",
                AlertType.Warning => "@Warning",
                AlertType.Error => "@Critical",
                _ => "@Info"
            };
        }

        /// <summary>
        /// Lấy border color dựa trên loại alert
        /// </summary>
        private static string GetBorderColor(AlertType alertType)
        {
            return alertType switch
            {
                AlertType.Success => "@Success",
                AlertType.Warning => "@Warning",
                AlertType.Error => "@Critical",
                _ => "@Info"
            };
        }

        /// <summary>
        /// Lấy màu stripe (thanh màu bên trái) dựa trên loại alert
        /// </summary>
        private static string GetStripeColor(AlertType alertType)
        {
            return alertType switch
            {
                AlertType.Success => "@Success",
                AlertType.Warning => "@Warning",
                AlertType.Error => "@Critical",
                AlertType.Information => "@Accent",
                _ => "@Black/0.8"
            };
        }

        /// <summary>
        /// Lấy form owner từ IWin32Window
        /// </summary>
        private static Form GetOwnerForm(IWin32Window owner)
        {
            Form ownerForm;
            if (owner != null)
            {
                ownerForm = owner as Form;
                if (ownerForm == null)
                {
                    var control = Control.FromHandle(owner.Handle);
                    ownerForm = control as Form;
                    if (ownerForm == null && control != null)
                    {
                        ownerForm = control.FindForm();
                    }
                }
            }
            else
            {
                ownerForm = Form.ActiveForm;
                if (ownerForm == null && Application.OpenForms.Count > 0)
                {
                    var firstForm = Application.OpenForms[0];
                    ownerForm = (Form)firstForm;
                }
            }
            return ownerForm;
        }

        /// <summary>
        /// Lấy icon key dựa trên alertType
        /// </summary>
        private static string GetIconKey(AlertType alertType)
        {
            return alertType switch
            {
                AlertType.Success => "about",
                AlertType.Warning => "warning",
                AlertType.Error => "error",
                _ => "info"
            };
        }

        /// <summary>
        /// Load icon dựa trên alertType từ Resources hoặc file system
        /// </summary>
        private static SvgImage LoadAlertIcon(AlertType alertType)
        {
            var iconKey = GetIconKey(alertType);
            
            // Xác định tên file icon (Success, Warning và Error sử dụng tên file khác như MsgBox)
            string iconFileName;
            string resourcePropertyName;
            
            if (alertType == AlertType.Success)
            {
                iconFileName = "about.svg";
                resourcePropertyName = "about";
            }
            else if (alertType == AlertType.Warning)
            {
                iconFileName = "security_warningcircled2.svg";
                resourcePropertyName = "security_warningcircled2";
            }
            else if (alertType == AlertType.Error)
            {
                iconFileName = "caution.svg";
                resourcePropertyName = "caution";
            }
            else
            {
                iconFileName = iconKey + ".svg";
                resourcePropertyName = iconKey;
            }
            
            try
            {
                // Ưu tiên 1: Thử load từ Properties.Resources
                try
                {
                    var resourcesType = typeof(Properties.Resources);
                    var iconProperty = resourcesType.GetProperty(resourcePropertyName, 
                        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.IgnoreCase);
                    
                    if (iconProperty != null)
                    {
                        if (iconProperty.GetValue(null) is SvgImage svgImage)
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
                var assembly = System.Reflection.Assembly.GetExecutingAssembly();
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
                    var assemblyDir = System.IO.Path.GetDirectoryName(assemblyLocation);
                    if (assemblyDir != null)
                    {
                        var possiblePaths = new[]
                        {
                            System.IO.Path.Combine(assemblyDir, "Resources"),
                            System.IO.Path.Combine(assemblyDir, "..", "Resources"),
                            System.IO.Path.Combine(assemblyDir, "..", "..", "Resources"),
                            System.IO.Path.Combine(assemblyDir, "_02_Common", "Utils", "Images")
                        };
                    
                        foreach (var path in possiblePaths)
                        {
                            var fullPath = System.IO.Path.GetFullPath(path);
                            if (System.IO.Directory.Exists(fullPath))
                            {
                                var iconPath = System.IO.Path.Combine(fullPath, iconFileName);
                                if (System.IO.File.Exists(iconPath))
                                {
                                    return SvgImage.FromFile(iconPath);
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
        /// Load icon about.svg từ Resources hoặc file system (theo pattern MsgBox.cs)
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
                        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.IgnoreCase);
                    
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
                var assembly = System.Reflection.Assembly.GetExecutingAssembly();
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
                    var assemblyDir = System.IO.Path.GetDirectoryName(assemblyLocation);
                    if (assemblyDir != null)
                    {
                        var possiblePaths = new[]
                        {
                            System.IO.Path.Combine(assemblyDir, "Resources"),
                            System.IO.Path.Combine(assemblyDir, "..", "Resources"),
                            System.IO.Path.Combine(assemblyDir, "..", "..", "Resources"),
                            System.IO.Path.Combine(assemblyDir, "_02_Common", "Utils", "Images")
                        };
                    
                        foreach (var path in possiblePaths)
                        {
                            var fullPath = System.IO.Path.GetFullPath(path);
                            if (System.IO.Directory.Exists(fullPath))
                            {
                                var aboutPath = System.IO.Path.Combine(fullPath, "about.svg");
                                if (System.IO.File.Exists(aboutPath))
                                {
                                    return SvgImage.FromFile(aboutPath);
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
        /// Set icon cho AlertInfo dựa trên loại alert
        /// DevExpress sẽ tự động thay thế ${SvgImage} từ AlertInfo.Image
        /// </summary>
        private static void SetAlertIcon(AlertInfo alertInfo, AlertType alertType)
        {
            // DevExpress AlertControl sẽ tự động sử dụng icon mặc định dựa trên AlertInfo
            // Nếu muốn set icon tùy chỉnh, có thể sử dụng:
            // alertInfo.Image = SvgImage.FromFile("path/to/icon.svg");
            // Hoặc sử dụng SvgImageCollection đã được set trong AlertControl.HtmlImages
            
            // Hiện tại để DevExpress tự động xử lý icon từ AlertInfo
            // Icon sẽ được thay thế vào placeholder ${SvgImage} trong template
        }

        /// <summary>
        /// Encode HTML để tránh XSS
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
        /// Convert HTML tags cho DevExpress
        /// </summary>
        private static string ConvertHtmlForDevExpress(string html)
        {
            if (string.IsNullOrEmpty(html))
                return string.Empty;

            // Chuyển \n thành <br>
            html = html.Replace("\n", "<br>");

            // Chuyển <b> và <strong> thành <span style="font-weight: bold">
            html = System.Text.RegularExpressions.Regex.Replace(html, 
                @"<(b|strong)>", "<span style=\"font-weight: bold\">", 
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            html = System.Text.RegularExpressions.Regex.Replace(html, 
                @"</(b|strong)>", "</span>", 
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            // Chuyển <color=...> thành <span style="color:...">
            html = System.Text.RegularExpressions.Regex.Replace(html, 
                @"<color=([^>]+)>", "<span style=\"color:$1\">", 
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            html = html.Replace("</color>", "</span>");

            return html;
        }

        #endregion

        #region ========== ENUMS ==========

        /// <summary>
        /// Loại alert
        /// </summary>
        private enum AlertType
        {
            Success,
            Warning,
            Error,
            Information
        }

        #endregion
    }
}

