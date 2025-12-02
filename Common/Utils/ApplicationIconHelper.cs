using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Common.Utils
{
    /// <summary>
    /// Helper class để quản lý icon của ứng dụng
    /// </summary>
    public static class ApplicationIconHelper
    {
        #region ========== CONSTANTS ==========

        private const string DEFAULT_ICON_NAME = "VNS Logo - Website.ico";

        #endregion

        #region ========== PROPERTIES ==========

        private static Icon _applicationIcon;
        
        /// <summary>
        /// Icon mặc định của ứng dụng
        /// </summary>
        public static Icon ApplicationIcon
        {
            get
            {
                _applicationIcon ??= LoadApplicationIcon();
                return _applicationIcon;
            }
        }

        #endregion

        #region ========== PUBLIC METHODS ==========

        /// <summary>
        /// Thiết lập icon cho form
        /// </summary>
        public static void SetFormIcon(Form form)
        {
            if (form == null || form.IsDisposed)
                return;

            try
            {
                // Đảm bảo form đã được khởi tạo đầy đủ trước khi set icon
                if (!form.IsHandleCreated)
                {
                    // Nếu handle chưa được tạo, đăng ký event để set icon sau khi form load
                    form.Load += (sender, e) =>
                    {
                        try
                        {
                            var icon = ApplicationIcon;
                            if (icon != null && !form.IsDisposed)
                            {
                                form.Icon = icon;
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"Lỗi thiết lập icon cho form khi load: {ex.Message}");
                        }
                    };
                    return;
                }

                var icon = ApplicationIcon;
                if (icon != null)
                {
                    form.Icon = icon;
                }
            }
            catch (Exception ex)
            {
                // Không throw exception, chỉ log để không làm crash ứng dụng
                System.Diagnostics.Debug.WriteLine($"Lỗi thiết lập icon cho form {form?.Name ?? "Unknown"}: {ex.Message}");
            }
        }

        /// <summary>
        /// Thiết lập icon cho tất cả các form đang mở
        /// </summary>
        public static void SetIconsForAllOpenForms()
        {
            try
            {
                foreach (Form form in Application.OpenForms)
                {
                    SetFormIcon(form);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi thiết lập icon cho các form: {ex.Message}");
            }
        }

        /// <summary>
        /// Lấy icon từ Resources hoặc file system
        /// </summary>
        public static Icon GetApplicationIcon()
        {
            return ApplicationIcon;
        }

        #endregion

        #region ========== PRIVATE METHODS ==========

        /// <summary>
        /// Load icon từ Resources hoặc file system
        /// </summary>
        private static Icon LoadApplicationIcon()
        {
            // Ưu tiên 1: Thử load từ Properties.Resources (nếu có)
            // Lưu ý: Cần thêm reference đến VnsErp2025 project hoặc thêm icon vào Common.Properties.Resources
            try
            {
                // Thử load từ VnsErp2025.Properties.Resources nếu assembly được load
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                foreach (var assembly in assemblies)
                {
                    try
                    {
                        var resourcesType = assembly.GetType("VnsErp2025.Properties.Resources");
                        if (resourcesType != null)
                        {
                            var iconProperty = resourcesType.GetProperty("AppIcon",
                                BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.IgnoreCase);

                            if (iconProperty != null)
                            {
                                if (iconProperty.GetValue(null) is Icon icon)
                                {
                                    return icon;
                                }
                            }

                            // Thử với tên khác
                            iconProperty = resourcesType.GetProperty("VNS_Logo_Website",
                                BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.IgnoreCase);

                            if (iconProperty != null)
                            {
                                if (iconProperty.GetValue(null) is Icon icon)
                                {
                                    return icon;
                                }
                            }
                        }
                    }
                    catch
                    {
                        // Tiếp tục thử assembly khác
                    }
                }
            }
            catch
            {
                // Tiếp tục thử file system
            }

            // Ưu tiên 2: Load từ Application.ExecutablePath (exe location)
            try
            {
                var exePath = Application.ExecutablePath;
                if (!string.IsNullOrEmpty(exePath))
                {
                    var exeDir = Path.GetDirectoryName(exePath);
                    if (exeDir != null)
                    {
                        var iconPath = Path.Combine(exeDir, DEFAULT_ICON_NAME);
                        if (File.Exists(iconPath))
                        {
                            try
                            {
                                return new Icon(iconPath);
                            }
                            catch
                            {
                                // Tiếp tục thử các cách khác
                            }
                        }
                    }
                }
            }
            catch
            {
                // Tiếp tục thử cách khác
            }

            // Ưu tiên 3: Load từ file system (cùng thư mục với assembly)
            try
            {
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
                        // Thử nhiều đường dẫn có thể
                        var possiblePaths = new[]
                        {
                            Path.Combine(assemblyDir, DEFAULT_ICON_NAME),
                            Path.Combine(assemblyDir, "..", DEFAULT_ICON_NAME),
                            Path.Combine(assemblyDir, "..", "..", DEFAULT_ICON_NAME),
                            Path.Combine(assemblyDir, "..", "..", "..", DEFAULT_ICON_NAME),
                            Path.Combine(assemblyDir, "VnsErp2025", DEFAULT_ICON_NAME),
                            Path.Combine(assemblyDir, "Resources", DEFAULT_ICON_NAME)
                        };

                        foreach (var iconPath in possiblePaths)
                        {
                            var fullPath = Path.GetFullPath(iconPath);
                            if (File.Exists(fullPath))
                            {
                                try
                                {
                                    return new Icon(fullPath);
                                }
                                catch
                                {
                                    // Tiếp tục thử đường dẫn khác
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi load icon từ file system: {ex.Message}");
            }

            // Fallback: Trả về null nếu không tìm thấy
            return null;
        }

        #endregion
    }
}

