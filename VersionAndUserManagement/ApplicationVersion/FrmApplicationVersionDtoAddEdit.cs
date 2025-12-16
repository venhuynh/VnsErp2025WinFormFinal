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

namespace VersionAndUserManagement.ApplicationVersion
{
    /// <summary>
    /// Form chỉnh sửa phiên bản ứng dụng
    /// </summary>
    public partial class FrmApplicationVersionDtoAddEdit : DevExpress.XtraEditors.XtraForm
    {
        #region ========== EVENTS ==========

        /// <summary>
        /// Event được trigger khi lưu thành công, trả về DTO đã được cập nhật
        /// </summary>
        public event Action<ApplicationVersionDto> VersionSaved;

        #endregion

        #region ========== KHAI BÁO BIẾN ==========

        /// <summary>
        /// Business Logic Layer cho phiên bản ứng dụng
        /// </summary>
        private readonly ApplicationVersionBll _applicationVersionBll;

        /// <summary>
        /// ID phiên bản được chọn
        /// </summary>
        private readonly Guid _versionId;

        /// <summary>
        /// Dữ liệu phiên bản hiện tại
        /// </summary>
        private ApplicationVersionDto _currentVersion;

        /// <summary>
        /// Trạng thái chỉnh sửa
        /// </summary>
        private readonly bool _isEditMode;

        #endregion

        #region ========== CONSTRUCTOR & PUBLIC METHODS ==========

        /// <summary>
        /// Khởi tạo form cho chế độ chỉnh sửa phiên bản.
        /// </summary>
        /// <param name="versionId">ID phiên bản (Guid.Empty không được phép, phải có ID để chỉnh sửa)</param>
        public FrmApplicationVersionDtoAddEdit(Guid versionId)
        {
            InitializeComponent();
            _applicationVersionBll = new ApplicationVersionBll();
            _versionId = versionId;
            _isEditMode = versionId != Guid.Empty;

            if (!_isEditMode)
            {
                throw new ArgumentException("Phải có ID phiên bản để chỉnh sửa. Không hỗ trợ thêm mới từ form này.");
            }

            InitializeForm();

            SaveBarButtonItem.ItemClick += SaveBarButtonItem_ItemClick;
            CloseBarButtonItem.ItemClick += CloseBarButtonItem_ItemClick;
        }

        #endregion

        #region ========== KHỞI TẠO FORM ==========

        /// <summary>
        /// Khởi tạo form cho chế độ chỉnh sửa.
        /// </summary>
        private void InitializeForm()
        {
            // Cấu hình form
            Text = "Chỉnh sửa phiên bản";

            // Load dữ liệu phiên bản
            LoadVersionData();

            // Setup validation
            SetupValidation();

            // Thiết lập SuperToolTip cho các controls
            SetupSuperToolTips();
        }

        #endregion

        #region ========== QUẢN LÝ DỮ LIỆU ==========

        /// <summary>
        /// Load dữ liệu phiên bản để chỉnh sửa
        /// </summary>
        private void LoadVersionData()
        {
            try
            {
                var versions = _applicationVersionBll.GetAllVersions();
                var version = versions.FirstOrDefault(v => v.Id == _versionId);
                
                if (version != null)
                {
                    _currentVersion = version;
                    BindDataToControls();
                }
                else
                {
                    MsgBox.ShowError("Không tìm thấy thông tin phiên bản.");
                    Close();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowError($"Lỗi tải dữ liệu phiên bản: {ex.Message}");
            }
        }

        /// <summary>
        /// Bind dữ liệu phiên bản vào các control
        /// </summary>
        private void BindDataToControls()
        {
            if (_currentVersion == null) return;

            // Bind data to controls
            VersionTextEdit.EditValue = _currentVersion.Version;
            ReleaseDateDateEdit.EditValue = _currentVersion.ReleaseDate;
            IsActiveCheckEdit.EditValue = _currentVersion.IsActive;
            DescriptionTextEdit.EditValue = _currentVersion.Description;
            ReleaseNoteTextEdit.EditValue = _currentVersion.ReleaseNote;

            // Disable Version khi edit (không cho phép thay đổi phiên bản)
            VersionTextEdit.Properties.ReadOnly = true;
            VersionTextEdit.Enabled = false;
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
                    await SaveVersionAsync();
                });

                // Thông báo thành công và đóng form
                MsgBox.ShowSuccess("Cập nhật phiên bản thành công!");
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lưu dữ liệu phiên bản");
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
        private ApplicationVersionDto GetDataFromControls()
        {
            return new ApplicationVersionDto
            {
                Id = _currentVersion.Id,
                Version = VersionTextEdit.Text?.Trim() ?? string.Empty,
                ReleaseDate = (DateTime)(ReleaseDateDateEdit.EditValue ?? DateTime.Now),
                IsActive = (bool)(IsActiveCheckEdit.EditValue ?? false),
                Description = DescriptionTextEdit.Text?.Trim(),
                ReleaseNote = ReleaseNoteTextEdit.Text?.Trim(),
                CreateDate = _currentVersion.CreateDate,
                CreateBy = _currentVersion.CreateBy,
                ModifiedDate = DateTime.Now,
                ModifiedBy = null // TODO: Lấy từ user đang đăng nhập
            };
        }

        /// <summary>
        /// Lưu dữ liệu phiên bản và trigger event VersionSaved
        /// </summary>
        private async Task SaveVersionAsync()
        {
            // Bước 1: Thu thập dữ liệu từ form và build DTO
            var versionDto = GetDataFromControls();

            // Bước 2: Lưu DTO qua BLL
            ApplicationVersionDto savedDto = await Task.Run(() => _applicationVersionBll.UpdateVersion(versionDto));

            // Bước 3: Trigger event để form cha có thể update datasource
            if (savedDto != null)
            {
                VersionSaved?.Invoke(savedDto);
            }
        }

        /// <summary>
        /// Kiểm tra hợp lệ dữ liệu bắt buộc sử dụng dxErrorProvider1
        /// </summary>
        private bool ValidateForm()
        {
            dxErrorProvider1.ClearErrors();

            // Version bắt buộc (nhưng đã disable nên không cần validate)
            if (string.IsNullOrWhiteSpace(VersionTextEdit?.Text))
            {
                dxErrorProvider1.SetError(VersionTextEdit, "Phiên bản không được để trống",
                    ErrorType.Critical);
                return false;
            }

            // ReleaseDate bắt buộc
            if (ReleaseDateDateEdit.EditValue == null)
            {
                dxErrorProvider1.SetError(ReleaseDateDateEdit, "Ngày phát hành không được để trống",
                    ErrorType.Critical);
                ReleaseDateDateEdit?.Focus();
                return false;
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
            RequiredFieldHelper.MarkRequiredFields(this, typeof(ApplicationVersionDto));
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
                if (VersionTextEdit != null)
                {
                    SuperToolTipHelper.SetTextEditSuperTip(
                        VersionTextEdit,
                        title: "<b><color=DarkBlue>🔖 Phiên bản</color></b>",
                        content: "Phiên bản của ứng dụng (ví dụ: 1.0.0.0). Trường này không thể thay đổi khi chỉnh sửa."
                    );
                }

                if (ReleaseDateDateEdit != null)
                {
                    SuperToolTipHelper.SetBaseEditSuperTip(
                        ReleaseDateDateEdit,
                        title: "<b><color=DarkBlue>📅 Ngày phát hành</color></b>",
                        content: "Ngày phát hành phiên bản này. Trường này là bắt buộc."
                    );
                }

                if (DescriptionTextEdit != null)
                {
                    SuperToolTipHelper.SetBaseEditSuperTip(
                        DescriptionTextEdit,
                        title: "<b><color=DarkBlue>📝 Mô tả</color></b>",
                        content: "Nhập mô tả bổ sung về phiên bản này (tối đa 500 ký tự)."
                    );
                }

                if (ReleaseNoteTextEdit != null)
                {
                    SuperToolTipHelper.SetBaseEditSuperTip(
                        ReleaseNoteTextEdit,
                        title: "<b><color=DarkBlue>📋 Ghi chú phát hành</color></b>",
                        content: "Nhập ghi chú phát hành chi tiết về phiên bản này (tối đa 1000 ký tự)."
                    );
                }

                if (IsActiveCheckEdit != null)
                {
                    SuperToolTipHelper.SetBaseEditSuperTip(
                        IsActiveCheckEdit,
                        title: "<b><color=DarkBlue>✅ Đang hoạt động</color></b>",
                        content: "Đánh dấu phiên bản này có đang được sử dụng hay không. Chỉ một phiên bản có thể Active tại một thời điểm."
                    );
                }

                if (SaveBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        SaveBarButtonItem,
                        title: "<b><color=Blue>💾 Lưu</color></b>",
                        content: "Lưu các thay đổi thông tin phiên bản vào hệ thống."
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
