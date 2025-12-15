using Bll.Common;
using Common.Common;
using Common.Helpers;
using Common.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using DTO.VersionAndUserManagementDto;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors.DXErrorProvider;

namespace VersionAndUserManagement.AllowedMacAddress
{
    /// <summary>
    /// Form thêm mới/chỉnh sửa MAC address được phép
    /// </summary>
    public partial class FrmAllowedMacAddressDtoAddEdit : DevExpress.XtraEditors.XtraForm
    {
        #region ========== EVENTS ==========

        /// <summary>
        /// Event được trigger khi lưu thành công, trả về DTO đã được cập nhật
        /// </summary>
        public event Action<AllowedMacAddressDto> MacAddressSaved;

        #endregion

        #region ========== KHAI BÁO BIẾN ==========

        /// <summary>
        /// Business Logic Layer cho MAC address được phép
        /// </summary>
        private readonly AllowedMacAddressBll _allowedMacAddressBll;

        /// <summary>
        /// ID MAC address được chọn
        /// </summary>
        private readonly Guid _macAddressId;

        /// <summary>
        /// Dữ liệu MAC address hiện tại
        /// </summary>
        private AllowedMacAddressDto _currentMacAddress;

        /// <summary>
        /// Trạng thái chỉnh sửa
        /// </summary>
        private readonly bool _isEditMode;

        #endregion

        #region ========== CONSTRUCTOR & PUBLIC METHODS ==========

        /// <summary>
        /// Khởi tạo form cho chế độ thêm mới/sửa MAC address.
        /// </summary>
        /// <param name="macAddressId">ID MAC address (Guid.Empty cho thêm mới)</param>
        public FrmAllowedMacAddressDtoAddEdit(Guid macAddressId)
        {
            InitializeComponent();
            _allowedMacAddressBll = new AllowedMacAddressBll();
            _macAddressId = macAddressId;
            _isEditMode = macAddressId != Guid.Empty;

            InitializeForm();

            SaveBarButtonItem.ItemClick += SaveBarButtonItem_ItemClick;
            CloseBarButtonItem.ItemClick += CloseBarButtonItem_ItemClick;
        }

        #endregion

        #region ========== KHỞI TẠO FORM ==========

        /// <summary>
        /// Khởi tạo form cho chế độ thêm mới/sửa.
        /// </summary>
        private void InitializeForm()
        {
            // Cấu hình form
            Text = _isEditMode ? "Chỉnh sửa MAC address" : "Thêm mới MAC address";

            // Load dữ liệu MAC address nếu là edit mode
            if (_isEditMode)
            {
                LoadMacAddressData();
            }
            else
            {
                // Set default values for new MAC address
                IsActiveCheckEdit.EditValue = true;
            }

            // Setup validation
            SetupValidation();

            // Thiết lập SuperToolTip cho các controls
            SetupSuperToolTips();
        }

        #endregion

        #region ========== QUẢN LÝ DỮ LIỆU ==========

        /// <summary>
        /// Load dữ liệu MAC address để chỉnh sửa
        /// </summary>
        private void LoadMacAddressData()
        {
            try
            {
                var macAddresses = _allowedMacAddressBll.GetAll();
                var macAddress = macAddresses.FirstOrDefault(m => m.Id == _macAddressId);
                
                if (macAddress != null)
                {
                    _currentMacAddress = macAddress;
                    BindDataToControls();
                }
                else
                {
                    MsgBox.ShowError("Không tìm thấy thông tin MAC address.");
                    Close();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowError($"Lỗi tải dữ liệu MAC address: {ex.Message}");
            }
        }

        /// <summary>
        /// Bind dữ liệu MAC address vào các control
        /// </summary>
        private void BindDataToControls()
        {
            if (_currentMacAddress == null) return;

            // Bind data to controls
            MacAddressTextEdit.EditValue = _currentMacAddress.MacAddress;
            ComputerNameTextEdit.EditValue = _currentMacAddress.ComputerName;
            DescriptionTextEdit.EditValue = _currentMacAddress.Description;
            IsActiveCheckEdit.EditValue = _currentMacAddress.IsActive;

            // Disable MAC address khi edit (không cho phép thay đổi)
            MacAddressTextEdit.Properties.ReadOnly = true;
            MacAddressTextEdit.Enabled = false;
        }

        #endregion

        #region ========== SỰ KIỆN FORM ==========

        /// <summary>
        /// Xử lý sự kiện click button Lưu
        /// </summary>
        private async void SaveBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (!ValidateForm())
                    return;

                // Lưu dữ liệu với waiting form
                await ExecuteWithWaitingFormAsync(async () =>
                {
                    await SaveMacAddressAsync();
                });

                // Thông báo thành công và đóng form
                MsgBox.ShowSuccess(_isEditMode
                    ? "Cập nhật MAC address thành công!"
                    : "Thêm mới MAC address thành công!");
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lưu dữ liệu MAC address");
            }
        }

        /// <summary>
        /// Xử lý sự kiện click button Đóng
        /// </summary>
        private void CloseBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }


        #endregion

        #region ========== XỬ LÝ DỮ LIỆU ==========

        /// <summary>
        /// Thu thập dữ liệu từ các control để tạo DTO lưu xuống DB.
        /// </summary>
        private AllowedMacAddressDto GetDataFromControls()
        {
            // Chuẩn hóa MAC address (format: XX-XX-XX-XX-XX-XX)
            var macAddress = MacAddressTextEdit.Text?.Trim() ?? string.Empty;
            var normalizedMac = macAddress.Replace("-", "").Replace(":", "").ToUpperInvariant();

            if (normalizedMac.Length == 12)
            {
                macAddress = $"{normalizedMac.Substring(0, 2)}-{normalizedMac.Substring(2, 2)}-{normalizedMac.Substring(4, 2)}-{normalizedMac.Substring(6, 2)}-{normalizedMac.Substring(8, 2)}-{normalizedMac.Substring(10, 2)}";
            }

            return new AllowedMacAddressDto
            {
                Id = _currentMacAddress?.Id ?? Guid.Empty,
                MacAddress = macAddress,
                ComputerName = ComputerNameTextEdit.Text?.Trim(),
                Description = DescriptionTextEdit.Text?.Trim(),
                IsActive = (bool)(IsActiveCheckEdit.EditValue ?? true),
                CreateDate = _currentMacAddress?.CreateDate ?? DateTime.Now,
                CreateBy = _currentMacAddress?.CreateBy,
                ModifiedDate = DateTime.Now,
                ModifiedBy = null // TODO: Lấy từ user đang đăng nhập
            };
        }

        /// <summary>
        /// Lưu dữ liệu MAC address và trigger event MacAddressSaved
        /// </summary>
        private async Task SaveMacAddressAsync()
        {
            // Bước 1: Thu thập dữ liệu từ form và build DTO
            var macAddressDto = GetDataFromControls();

            // Bước 2: Lưu DTO qua BLL
            AllowedMacAddressDto savedDto;
            if (_isEditMode)
            {
                savedDto = await Task.Run(() => _allowedMacAddressBll.Update(macAddressDto));
            }
            else
            {
                savedDto = await Task.Run(() => _allowedMacAddressBll.Create(macAddressDto));
            }

            // Bước 3: Trigger event để form cha có thể update datasource
            if (savedDto != null)
                {
                MacAddressSaved?.Invoke(savedDto);
            }
        }

        /// <summary>
        /// Kiểm tra hợp lệ dữ liệu bắt buộc sử dụng dxErrorProvider1
        /// </summary>
        private bool ValidateForm()
        {
            dxErrorProvider1.ClearErrors();

            // MAC address bắt buộc
            if (string.IsNullOrWhiteSpace(MacAddressTextEdit?.Text))
            {
                dxErrorProvider1.SetError(MacAddressTextEdit, "Địa chỉ MAC không được để trống",
                    ErrorType.Critical);
                MacAddressTextEdit?.Focus();
                return false;
            }

            // Validate format MAC address (có thể có dấu gạch ngang hoặc không)
            var macAddress = MacAddressTextEdit.Text.Trim();
            var normalizedMac = macAddress.Replace("-", "").Replace(":", "").ToUpperInvariant();

            if (normalizedMac.Length != 12 || !System.Text.RegularExpressions.Regex.IsMatch(normalizedMac, @"^[0-9A-F]{12}$"))
            {
                dxErrorProvider1.SetError(MacAddressTextEdit, "Địa chỉ MAC không đúng định dạng (ví dụ: XX-XX-XX-XX-XX-XX hoặc XXXXXXXXXXXX)",
                    ErrorType.Critical);
                MacAddressTextEdit?.Focus();
                return false;
            }

            // Validate trùng lặp MAC address - chỉ kiểm tra khi tạo mới
            if (!_isEditMode)
            {
                var existing = _allowedMacAddressBll.GetAll()
                    .FirstOrDefault(m => m.MacAddress.Replace("-", "").Replace(":", "").ToUpperInvariant() == normalizedMac);

                if (existing != null)
                {
                    dxErrorProvider1.SetError(MacAddressTextEdit, $"MAC address '{macAddress}' đã tồn tại trong hệ thống",
                        ErrorType.Critical);
                    MacAddressTextEdit?.Focus();
                    return false;
                }
            }

            return true;
        }

        #endregion

        #region ========== TIỆN ÍCH ==========

        /// <summary>
        /// Setup validation cho form
        /// </summary>
        private void SetupValidation()
        {
            // Đánh dấu các trường bắt buộc theo DataAnnotations của DTO
            RequiredFieldHelper.MarkRequiredFields(this, typeof(AllowedMacAddressDto));
        }

        /// <summary>
        /// Thực thi async operation với waiting form (hiển thị splash screen)
        /// </summary>
        /// <param name="operation">Operation async cần thực thi</param>
        private async Task ExecuteWithWaitingFormAsync(Func<Task> operation)
        {
            try
            {
                // Hiển thị waiting form
                SplashScreenManager.ShowForm(typeof(WaitForm1));

                // Thực hiện operation
                await operation();
            }
            finally
            {
                // Đóng waiting form
                SplashScreenManager.CloseForm();
            }
        }

        #endregion

        #region ========== TIỆN ÍCH HỖ TRỢ ==========

        /// <summary>
        /// Hiển thị lỗi qua XtraMessageBox với thông báo tiếng Việt
        /// </summary>
        /// <param name="ex">Exception cần hiển thị</param>
        /// <param name="action">Tên hành động đang thực hiện khi xảy ra lỗi</param>
        private void ShowError(Exception ex, string action)
        {
            MsgBox.ShowException(ex, $"Lỗi {action}");
        }

        /// <summary>
        /// Thiết lập SuperToolTip cho các controls trong form
        /// </summary>
        private void SetupSuperToolTips()
        {
            try
            {
                if (MacAddressTextEdit != null)
                {
                    var macAddressTip = _isEditMode
                        ? "Địa chỉ MAC của máy tính được phép sử dụng ứng dụng. Trường này không thể thay đổi khi chỉnh sửa."
                        : "Nhập địa chỉ MAC của máy tính (ví dụ: XX-XX-XX-XX-XX-XX hoặc XXXXXXXXXXXX). Trường này là bắt buộc. Có thể dùng nút 'Thêm MAC hiện tại' để tự động điền.";
                    
                    SuperToolTipHelper.SetTextEditSuperTip(
                        MacAddressTextEdit,
                        title: "<b><color=DarkBlue>🔖 Địa chỉ MAC</color></b>",
                        content: macAddressTip
                    );
                }

                if (ComputerNameTextEdit != null)
                {
                    SuperToolTipHelper.SetTextEditSuperTip(
                        ComputerNameTextEdit,
                        title: "<b><color=DarkBlue>💻 Tên máy tính</color></b>",
                        content: "Nhập tên máy tính (tùy chọn). Tên máy tính sẽ được tự động điền khi dùng nút 'Thêm MAC hiện tại'."
                    );
                }

                if (DescriptionTextEdit != null)
                {
                    SuperToolTipHelper.SetBaseEditSuperTip(
                        DescriptionTextEdit,
                        title: "<b><color=DarkBlue>📝 Mô tả</color></b>",
                        content: "Nhập mô tả bổ sung về MAC address này (tối đa 500 ký tự)."
                    );
                }

                if (IsActiveCheckEdit != null)
                {
                    SuperToolTipHelper.SetBaseEditSuperTip(
                        IsActiveCheckEdit,
                        title: "<b><color=DarkBlue>✅ Đang hoạt động</color></b>",
                        content: "Đánh dấu MAC address này có đang được phép sử dụng ứng dụng hay không."
                    );
                }

                if (SaveBarButtonItem != null)
                {
                    var saveTip = _isEditMode
                        ? "Lưu các thay đổi thông tin MAC address vào hệ thống."
                        : "Lưu thông tin MAC address mới vào hệ thống.";
                    
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        SaveBarButtonItem,
                        title: "<b><color=Blue>💾 Lưu</color></b>",
                        content: saveTip
                    );
                }

                if (CloseBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        CloseBarButtonItem,
                        title: "<b><color=Red>❌ Đóng</color></b>",
                        content: "Đóng form mà không lưu thay đổi."
                    );
                }
            }
            catch (Exception ex)
            {
                // Ignore lỗi setup SuperToolTip để không chặn form
                System.Diagnostics.Debug.WriteLine($"Lỗi setup SuperToolTip: {ex.Message}");
            }
        }

        #endregion
    }
}
