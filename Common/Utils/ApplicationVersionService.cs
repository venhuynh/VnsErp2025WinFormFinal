using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utils
{
    /// <summary>
    /// Service quản lý phiên bản và cập nhật ứng dụng
    /// </summary>
    public class ApplicationVersionService
    {
        #region ========== CONSTANTS ==========

        private const string DEFAULT_UPDATE_SERVER_URL = "https://updates.vns.local/api";
        private const int DEFAULT_TIMEOUT_SECONDS = 30;

        #endregion

        #region ========== PROPERTIES ==========

        /// <summary>
        /// URL của update server
        /// </summary>
        public string UpdateServerUrl { get; set; }

        /// <summary>
        /// Timeout cho HTTP requests (seconds)
        /// </summary>
        public int TimeoutSeconds { get; set; }

        #endregion

        #region ========== CONSTRUCTOR ==========

        public ApplicationVersionService()
        {
            UpdateServerUrl = GetUpdateServerUrlFromConfig();
            TimeoutSeconds = DEFAULT_TIMEOUT_SECONDS;
        }

        #endregion

        #region ========== VERSION INFO ==========

        /// <summary>
        /// Lấy phiên bản hiện tại của ứng dụng
        /// </summary>
        public Version GetCurrentVersion()
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                var version = assembly.GetName().Version;
                return version ?? new Version(1, 0, 0, 0);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi lấy phiên bản hiện tại: {ex.Message}");
                return new Version(1, 0, 0, 0);
            }
        }

        /// <summary>
        /// Lấy phiên bản dạng string (Major.Minor.Build)
        /// </summary>
        public string GetCurrentVersionString()
        {
            var version = GetCurrentVersion();
            return $"{version.Major}.{version.Minor}.{version.Build}";
        }

        /// <summary>
        /// Lấy thông tin phiên bản từ server
        /// </summary>
        public async Task<VersionInfo> GetLatestVersionInfoAsync()
        {
            try
            {
                using var httpClient = new HttpClient();
                httpClient.Timeout = TimeSpan.FromSeconds(TimeoutSeconds);
                var url = $"{UpdateServerUrl}/version/current";
                    
                var response = await httpClient.GetStringAsync(url);
                var versionInfo = DeserializeVersionInfo(response);
                    
                return versionInfo;
            }
            catch (Exception ex)
            {
                // Không log lỗi network để tránh spam log khi không có update server
                // Chỉ log ở Debug level
                System.Diagnostics.Debug.WriteLine($"Lỗi lấy thông tin phiên bản từ server: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Kiểm tra xem có bản cập nhật mới không
        /// </summary>
        public async Task<bool> CheckForUpdatesAsync()
        {
            try
            {
                var latestVersionInfo = await GetLatestVersionInfoAsync();
                if (latestVersionInfo == null)
                    return false;

                var currentVersion = GetCurrentVersion();
                var latestVersion = ParseVersion(latestVersionInfo.Version);

                return latestVersion > currentVersion;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi kiểm tra cập nhật: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// So sánh hai phiên bản
        /// </summary>
        public VersionComparisonResult CompareVersions(Version current, Version latest)
        {
            if (latest > current)
                return VersionComparisonResult.NewerAvailable;
            if (latest < current)
                return VersionComparisonResult.CurrentIsNewer;
            return VersionComparisonResult.Same;
        }

        #endregion

        #region ========== UPDATE DOWNLOAD ==========

        /// <summary>
        /// Tải bản cập nhật
        /// </summary>
        public async Task<bool> DownloadUpdateAsync(VersionInfo versionInfo, IProgress<DownloadProgress> progress = null)
        {
            try
            {
                if (string.IsNullOrEmpty(versionInfo.DownloadUrl))
                {
                    throw new ArgumentException("DownloadUrl không được để trống");
                }

                var downloadPath = GetUpdateDownloadPath();
                Directory.CreateDirectory(downloadPath);

                var fileName = Path.GetFileName(versionInfo.DownloadUrl);
                if (string.IsNullOrEmpty(fileName))
                {
                    fileName = $"VnsErp2025_{versionInfo.Version}.exe";
                }

                var filePath = Path.Combine(downloadPath, fileName);

                using var httpClient = new HttpClient();
                httpClient.Timeout = TimeSpan.FromMinutes(10); // Timeout dài hơn cho download

                using (var response = await httpClient.GetAsync(versionInfo.DownloadUrl, HttpCompletionOption.ResponseHeadersRead))
                {
                    response.EnsureSuccessStatusCode();

                    var totalBytes = response.Content.Headers.ContentLength ?? 0;
                    var downloadedBytes = 0L;

                    using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
                    using var contentStream = await response.Content.ReadAsStreamAsync();
                    var buffer = new byte[8192];
                    int bytesRead;

                    while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        await fileStream.WriteAsync(buffer, 0, bytesRead);
                        downloadedBytes += bytesRead;

                        if (progress != null && totalBytes > 0)
                        {
                            var progressPercent = (int)((downloadedBytes * 100) / totalBytes);
                            progress.Report(new DownloadProgress
                            {
                                BytesDownloaded = downloadedBytes,
                                TotalBytes = totalBytes,
                                Percentage = progressPercent
                            });
                        }
                    }
                }

                // Verify checksum nếu có
                if (!string.IsNullOrEmpty(versionInfo.Checksum))
                {
                    if (!VerifyChecksum(filePath, versionInfo.Checksum))
                    {
                        File.Delete(filePath);
                        throw new Exception("Checksum không khớp. File có thể bị hỏng.");
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi tải bản cập nhật: {ex.Message}");
                return false;
            }
        }

        #endregion

        #region ========== HELPER METHODS ==========

        private string GetUpdateServerUrlFromConfig()
        {
            try
            {
                // Đọc từ App.config hoặc registry
                var configValue = System.Configuration.ConfigurationManager.AppSettings["UpdateServerUrl"];
                return string.IsNullOrEmpty(configValue) ? DEFAULT_UPDATE_SERVER_URL : configValue;
            }
            catch
            {
                return DEFAULT_UPDATE_SERVER_URL;
            }
        }

        private string GetUpdateDownloadPath()
        {
            try
            {
                var configPath = System.Configuration.ConfigurationManager.AppSettings["UpdateDownloadPath"];
                if (!string.IsNullOrEmpty(configPath))
                {
                    // Thay thế environment variables
                    configPath = Environment.ExpandEnvironmentVariables(configPath);
                    return configPath;
                }
            }
            catch
            {
                // Ignore
            }

            // Default: %TEMP%\VnsErp2025\Updates
            var tempPath = Path.Combine(Path.GetTempPath(), "VnsErp2025", "Updates");
            return tempPath;
        }

        private Version ParseVersion(string versionString)
        {
            try
            {
                return Version.Parse(versionString);
            }
            catch
            {
                // Thử parse với format Major.Minor.Build
                var parts = versionString.Split('.');
                if (parts.Length >= 2)
                {
                    var major = int.Parse(parts[0]);
                    var minor = int.Parse(parts[1]);
                    var build = parts.Length >= 3 ? int.Parse(parts[2]) : 0;
                    return new Version(major, minor, build);
                }
                return new Version(1, 0, 0);
            }
        }

        private bool VerifyChecksum(string filePath, string expectedChecksum)
        {
            try
            {
                // TODO: Implement checksum verification (SHA256)
                // Có thể sử dụng System.Security.Cryptography.SHA256
                return true; // Placeholder
            }
            catch
            {
                return false;
            }
        }

        private VersionInfo DeserializeVersionInfo(string json)
        {
            try
            {
                // Simple JSON deserialization without external library
                // For production, consider using System.Text.Json or Newtonsoft.Json
                var serializer = new DataContractJsonSerializer(typeof(VersionInfo));
                using var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
                return (VersionInfo)serializer.ReadObject(stream);
            }
            catch
            {
                // Fallback: Manual parsing for basic structure
                // This is a simplified version - for production use proper JSON library
                return ParseVersionInfoManually(json);
            }
        }

        private VersionInfo ParseVersionInfoManually(string json)
        {
            // Simplified manual parsing - chỉ dùng khi không có JSON library
            // Trong production nên dùng System.Text.Json hoặc Newtonsoft.Json
            var info = new VersionInfo();
            
            // Extract version
            var versionMatch = System.Text.RegularExpressions.Regex.Match(json, @"""version""\s*:\s*""([^""]+)""");
            if (versionMatch.Success)
                info.Version = versionMatch.Groups[1].Value;

            // Extract downloadUrl
            var urlMatch = System.Text.RegularExpressions.Regex.Match(json, @"""downloadUrl""\s*:\s*""([^""]+)""");
            if (urlMatch.Success)
                info.DownloadUrl = urlMatch.Groups[1].Value;

            // Extract fileSize
            var sizeMatch = System.Text.RegularExpressions.Regex.Match(json, @"""fileSize""\s*:\s*(\d+)");
            if (sizeMatch.Success && long.TryParse(sizeMatch.Groups[1].Value, out var size))
                info.FileSize = size;

            // Extract isMandatory
            var mandatoryMatch = System.Text.RegularExpressions.Regex.Match(json, @"""isMandatory""\s*:\s*(true|false)");
            if (mandatoryMatch.Success)
                info.IsMandatory = mandatoryMatch.Groups[1].Value == "true";

            return info;
        }

        #endregion
    }

    #region ========== SUPPORTING CLASSES ==========

    /// <summary>
    /// Thông tin phiên bản từ server
    /// </summary>
    [Serializable]
    public class VersionInfo
    {
        public string Version { get; set; }

        public DateTime ReleaseDate { get; set; }

        public bool IsMandatory { get; set; }

        public string MinSupportedVersion { get; set; }

        public string DownloadUrl { get; set; }

        public long FileSize { get; set; }

        public string Checksum { get; set; }

        public ReleaseNotes ReleaseNotes { get; set; }

        public string[] Changes { get; set; }
    }

    /// <summary>
    /// Release notes đa ngôn ngữ
    /// </summary>
    [Serializable]
    public class ReleaseNotes
    {
        public string English { get; set; }

        public string Vietnamese { get; set; }
    }

    /// <summary>
    /// Kết quả so sánh phiên bản
    /// </summary>
    public enum VersionComparisonResult
    {
        Same,
        NewerAvailable,
        CurrentIsNewer
    }

    /// <summary>
    /// Tiến trình download
    /// </summary>
    public class DownloadProgress
    {
        public long BytesDownloaded { get; set; }
        public long TotalBytes { get; set; }
        public int Percentage { get; set; }
    }

    #endregion
}

