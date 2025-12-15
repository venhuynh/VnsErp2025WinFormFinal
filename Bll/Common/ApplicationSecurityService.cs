using System;
using System.Windows.Forms;
using Bll.Common;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;

namespace Bll.Common;

/// <summary>
/// Service kiểm tra bảo mật ứng dụng khi khởi động
/// Kiểm tra phiên bản và MAC address
/// </summary>
public class ApplicationSecurityService
{
    private readonly ApplicationVersionBll _versionBll;
    private readonly AllowedMacAddressBll _macAddressBll;
    private readonly ILogger _logger;

    public ApplicationSecurityService()
    {
        _versionBll = new ApplicationVersionBll();
        _macAddressBll = new AllowedMacAddressBll();
        _logger = LoggerFactory.CreateLogger(LogCategory.BLL);
    }

    /// <summary>
    /// Kết quả kiểm tra bảo mật
    /// </summary>
    public class SecurityCheckResult
    {
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; }
        public string CurrentVersion { get; set; }
        public string DatabaseVersion { get; set; }
        public string MacAddress { get; set; }
    }

    /// <summary>
    /// Kiểm tra bảo mật ứng dụng (version và MAC address)
    /// </summary>
    /// <returns>SecurityCheckResult</returns>
    public SecurityCheckResult CheckSecurity()
    {
        var result = new SecurityCheckResult();

        try
        {
            _logger?.Info("Bắt đầu kiểm tra bảo mật ứng dụng");

            // 1. Kiểm tra phiên bản
            result.CurrentVersion = _versionBll.GetCurrentApplicationVersion();
            var activeVersion = _versionBll.GetActiveVersion();
            result.DatabaseVersion = activeVersion?.Version;

            if (activeVersion != null)
            {
                var isVersionValid = _versionBll.IsVersionValid();
                if (!isVersionValid)
                {
                    result.IsValid = false;
                    result.ErrorMessage = $"Phiên bản ứng dụng không khớp!\n\n" +
                                        $"Phiên bản hiện tại: {result.CurrentVersion}\n" +
                                        $"Phiên bản yêu cầu: {result.DatabaseVersion}\n\n" +
                                        $"Vui lòng cập nhật ứng dụng lên phiên bản {result.DatabaseVersion} để tiếp tục sử dụng.";
                    _logger?.Warning($"Kiểm tra phiên bản thất bại: {result.ErrorMessage}");
                    return result;
                }
            }
            else
            {
                _logger?.Info("Không có phiên bản Active trong database, bỏ qua kiểm tra phiên bản");
            }

            // 2. Kiểm tra MAC address
            result.MacAddress = _macAddressBll.GetCurrentMacAddress();
            if (string.IsNullOrWhiteSpace(result.MacAddress))
            {
                result.IsValid = false;
                result.ErrorMessage = "Không thể lấy địa chỉ MAC của máy tính.\n\n" +
                                   "Vui lòng liên hệ quản trị viên để được hỗ trợ.";
                _logger?.Warning($"Không thể lấy MAC address: {result.ErrorMessage}");
                return result;
            }

            var isMacAllowed = _macAddressBll.IsCurrentMacAddressAllowed();
            if (!isMacAllowed)
            {
                result.IsValid = false;
                result.ErrorMessage = $"Máy tính này không được phép sử dụng ứng dụng!\n\n" +
                                    $"Địa chỉ MAC: {result.MacAddress}\n" +
                                    $"Tên máy tính: {Environment.MachineName}\n\n" +
                                    $"Vui lòng liên hệ quản trị viên để được thêm vào danh sách được phép.";
                _logger?.Warning($"MAC address không được phép: {result.ErrorMessage}");
                return result;
            }

            // Tất cả kiểm tra đều pass
            result.IsValid = true;
            _logger?.Info($"Kiểm tra bảo mật thành công. Version: {result.CurrentVersion}, MAC: {result.MacAddress}");

            return result;
        }
        catch (Exception ex)
        {
            _logger?.Error($"Lỗi khi kiểm tra bảo mật: {ex.Message}", ex);
            result.IsValid = false;
            result.ErrorMessage = $"Lỗi khi kiểm tra bảo mật ứng dụng:\n\n{ex.Message}\n\n" +
                                "Vui lòng liên hệ quản trị viên để được hỗ trợ.";
            return result;
        }
    }

    /// <summary>
    /// Hiển thị thông báo lỗi và thoát ứng dụng
    /// </summary>
    /// <param name="result">Kết quả kiểm tra bảo mật</param>
    public void ShowErrorAndExit(SecurityCheckResult result)
    {
        if (result == null || result.IsValid)
            return;

        MessageBox.Show(
            result.ErrorMessage,
            "Lỗi bảo mật ứng dụng",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error
        );

        Application.Exit();
    }
}
