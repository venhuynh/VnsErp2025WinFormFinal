using System;
using System.Linq;
using System.Windows.Forms;
using Bll.Common;
using Common.Utils;
using DTO.Common;
using DevExpress.XtraEditors;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;

namespace VnsErp2025.Form;

/// <summary>
/// Form quản lý phiên bản ứng dụng
/// Cho phép cập nhật phiên bản khi release
/// </summary>
public partial class FrmApplicationVersionManagement : DevExpress.XtraEditors.XtraForm
{
    private readonly ApplicationVersionBll _versionBll;
    private readonly ILogger _logger;

    public FrmApplicationVersionManagement()
    {
        InitializeComponent();
        _versionBll = new ApplicationVersionBll();
        _logger = LoggerFactory.CreateLogger(LogCategory.UI);
        InitializeForm();
    }

    private void InitializeForm()
    {
        try
        {
            LoadVersions();
            LoadCurrentVersion();
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi khởi tạo form: {ex.Message}", ex);
            MsgBox.ShowError($"Lỗi khởi tạo form: {ex.Message}");
        }
    }

    private void LoadCurrentVersion()
    {
        try
        {
            var currentVersion = _versionBll.GetCurrentApplicationVersion();
            txtCurrentVersion.Text = currentVersion;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy phiên bản hiện tại: {ex.Message}", ex);
        }
    }

    private void LoadVersions()
    {
        try
        {
            var versions = _versionBll.GetAllVersions();
            gridControlVersions.DataSource = versions;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi tải danh sách phiên bản: {ex.Message}", ex);
            MsgBox.ShowError($"Lỗi khi tải danh sách phiên bản: {ex.Message}");
        }
    }

    private void btnUpdateFromAssembly_Click(object sender, EventArgs e)
    {
        try
        {
            var currentVersion = _versionBll.GetCurrentApplicationVersion();
            
            if (MsgBox.ShowQuestion($"Bạn có chắc chắn muốn cập nhật phiên bản trong database thành {currentVersion}?\n\n" +
                                   "Phiên bản này sẽ được đặt làm Active và các phiên bản khác sẽ bị vô hiệu hóa.") != DialogResult.Yes)
            {
                return;
            }

            _versionBll.UpdateVersionFromAssembly(
                description: txtDescription.Text,
                userId: null // TODO: Lấy từ user đang đăng nhập
            );

            MsgBox.ShowSuccess("Đã cập nhật phiên bản thành công!");
            LoadVersions();
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi cập nhật phiên bản: {ex.Message}", ex);
            MsgBox.ShowError($"Lỗi khi cập nhật phiên bản: {ex.Message}");
        }
    }

    private void btnSetActive_Click(object sender, EventArgs e)
    {
        try
        {
            var selectedVersion = gridViewVersions.GetFocusedRow() as ApplicationVersionDto;
            if (selectedVersion == null)
            {
                MsgBox.ShowWarning("Vui lòng chọn một phiên bản để đặt làm Active.");
                return;
            }

            if (MsgBox.ShowQuestion($"Bạn có chắc chắn muốn đặt phiên bản {selectedVersion.Version} làm Active?\n\n" +
                                   "Các phiên bản khác sẽ bị vô hiệu hóa.") != DialogResult.Yes)
            {
                return;
            }

            _versionBll.SetActiveVersion(selectedVersion.Id);
            MsgBox.ShowSuccess("Đã đặt phiên bản làm Active thành công!");
            LoadVersions();
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi đặt phiên bản Active: {ex.Message}", ex);
            MsgBox.ShowError($"Lỗi khi đặt phiên bản Active: {ex.Message}");
        }
    }

    private void btnRefresh_Click(object sender, EventArgs e)
    {
        LoadVersions();
        LoadCurrentVersion();
    }
}
