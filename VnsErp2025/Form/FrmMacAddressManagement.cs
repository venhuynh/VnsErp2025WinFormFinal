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
/// Form quản lý MAC address được phép sử dụng ứng dụng
/// </summary>
public partial class FrmMacAddressManagement : DevExpress.XtraEditors.XtraForm
{
    private readonly AllowedMacAddressBll _macAddressBll;
    private readonly ILogger _logger;

    public FrmMacAddressManagement()
    {
        InitializeComponent();
        _macAddressBll = new AllowedMacAddressBll();
        _logger = LoggerFactory.CreateLogger(LogCategory.UI);
        InitializeForm();
    }

    private void InitializeForm()
    {
        try
        {
            LoadCurrentMacAddress();
            LoadMacAddresses();
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi khởi tạo form: {ex.Message}", ex);
            MsgBox.ShowError($"Lỗi khởi tạo form: {ex.Message}");
        }
    }

    private void LoadCurrentMacAddress()
    {
        try
        {
            var currentMac = _macAddressBll.GetCurrentMacAddress();
            txtCurrentMacAddress.Text = currentMac ?? "Không tìm thấy";
            
            var computerName = System.Environment.MachineName;
            txtCurrentComputerName.Text = computerName;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy MAC address hiện tại: {ex.Message}", ex);
        }
    }

    private void LoadMacAddresses()
    {
        try
        {
            var macAddresses = _macAddressBll.GetAll();
            gridControlMacAddresses.DataSource = macAddresses;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi tải danh sách MAC address: {ex.Message}", ex);
            MsgBox.ShowError($"Lỗi khi tải danh sách MAC address: {ex.Message}");
        }
    }

    private void btnAddCurrent_Click(object sender, EventArgs e)
    {
        try
        {
            var currentMac = _macAddressBll.GetCurrentMacAddress();
            if (string.IsNullOrWhiteSpace(currentMac))
            {
                MsgBox.ShowWarning("Không thể lấy MAC address của máy tính hiện tại.");
                return;
            }

            // Kiểm tra xem đã tồn tại chưa
            var existing = _macAddressBll.GetAll()
                .FirstOrDefault(m => m.MacAddress.Equals(currentMac, StringComparison.OrdinalIgnoreCase));

            if (existing != null)
            {
                MsgBox.ShowWarning($"MAC address {currentMac} đã tồn tại trong danh sách.");
                return;
            }

            if (MsgBox.ShowQuestion($"Bạn có chắc chắn muốn thêm MAC address {currentMac} vào danh sách được phép?") != DialogResult.Yes)
            {
                return;
            }

            _macAddressBll.AddCurrentMacAddress(
                computerName: txtCurrentComputerName.Text,
                description: txtDescription.Text,
                userId: null // TODO: Lấy từ user đang đăng nhập
            );

            MsgBox.ShowSuccess("Đã thêm MAC address thành công!");
            LoadMacAddresses();
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi thêm MAC address: {ex.Message}", ex);
            MsgBox.ShowError($"Lỗi khi thêm MAC address: {ex.Message}");
        }
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(txtMacAddress.Text))
            {
                MsgBox.ShowWarning("Vui lòng nhập địa chỉ MAC.");
                return;
            }

            // Kiểm tra xem đã tồn tại chưa
            var existing = _macAddressBll.GetAll()
                .FirstOrDefault(m => m.MacAddress.Equals(txtMacAddress.Text, StringComparison.OrdinalIgnoreCase));

            if (existing != null)
            {
                MsgBox.ShowWarning($"MAC address {txtMacAddress.Text} đã tồn tại trong danh sách.");
                return;
            }

            var dto = new AllowedMacAddressDto
            {
                Id = Guid.NewGuid(),
                MacAddress = txtMacAddress.Text.Trim(),
                ComputerName = txtComputerName.Text.Trim(),
                Description = txtDescription.Text.Trim(),
                IsActive = true,
                CreateDate = DateTime.Now,
                CreateBy = null // TODO: Lấy từ user đang đăng nhập
            };

            _macAddressBll.Create(dto);
            MsgBox.ShowSuccess("Đã thêm MAC address thành công!");
            
            // Clear form
            txtMacAddress.Text = string.Empty;
            txtComputerName.Text = string.Empty;
            txtDescription.Text = string.Empty;
            
            LoadMacAddresses();
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi thêm MAC address: {ex.Message}", ex);
            MsgBox.ShowError($"Lỗi khi thêm MAC address: {ex.Message}");
        }
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            var selected = gridViewMacAddresses.GetFocusedRow() as AllowedMacAddressDto;
            if (selected == null)
            {
                MsgBox.ShowWarning("Vui lòng chọn một MAC address để xóa.");
                return;
            }

            if (MsgBox.ShowQuestion($"Bạn có chắc chắn muốn xóa MAC address {selected.MacAddress}?") != DialogResult.Yes)
            {
                return;
            }

            _macAddressBll.Delete(selected.Id);
            MsgBox.ShowSuccess("Đã xóa MAC address thành công!");
            LoadMacAddresses();
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi xóa MAC address: {ex.Message}", ex);
            MsgBox.ShowError($"Lỗi khi xóa MAC address: {ex.Message}");
        }
    }

    private void btnToggleActive_Click(object sender, EventArgs e)
    {
        try
        {
            var selected = gridViewMacAddresses.GetFocusedRow() as AllowedMacAddressDto;
            if (selected == null)
            {
                MsgBox.ShowWarning("Vui lòng chọn một MAC address để cập nhật.");
                return;
            }

            selected.IsActive = !selected.IsActive;
            _macAddressBll.Update(selected);
            
            var status = selected.IsActive ? "kích hoạt" : "vô hiệu hóa";
            MsgBox.ShowSuccess($"Đã {status} MAC address thành công!");
            LoadMacAddresses();
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi cập nhật MAC address: {ex.Message}", ex);
            MsgBox.ShowError($"Lỗi khi cập nhật MAC address: {ex.Message}");
        }
    }

    private void btnRefresh_Click(object sender, EventArgs e)
    {
        LoadMacAddresses();
        LoadCurrentMacAddress();
    }
}
