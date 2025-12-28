using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using DTO.MasterData.CustomerPartner;
using System;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bll.MasterData.CustomerBll;
using Common.Common;
using Common.Utils;

namespace MasterData.Customer
{
    /// <summary>
    /// Form thêm mới liên hệ đối tác.
    /// Cung cấp giao diện nhập liệu đầy đủ với validation và lookup chi nhánh đối tác.
    /// </summary>
    public partial class FrmBusinessPartnerContactDetail : XtraForm
    {
        #region ========== KHAI BÁO BIẾN ==========

        /// <summary>
        /// Business Logic Layer cho liên hệ đối tác
        /// </summary>
        private readonly BusinessPartnerContactBll _bll = new();

        /// <summary>
        /// ID chi nhánh đối tác được chọn
        /// </summary>
        private Guid? _selectedSiteId;
        
        /// <summary>
        /// Trạng thái đang tải dữ liệu nguồn
        /// </summary>
        private bool _isLoadingDataSources;

        #endregion

        #region ========== CONSTRUCTOR & PUBLIC METHODS ==========

        /// <summary>
        /// Khởi tạo form cho chế độ thêm mới liên hệ đối tác.
        /// </summary>
        public FrmBusinessPartnerContactDetail()
        {
            InitializeComponent();
            InitializeForm();

            SaveBarButtonItem.ItemClick += SaveBarButtonItem_ItemClick;
            CloseBarButtonItem.ItemClick += CloseBarButtonItem_ItemClick;

            PartnerNameSearchLookUpEdit.EditValueChanged += PartnerNameSearchLookUpEdit_EditValueChanged;
        }

        #endregion

        #region ========== KHỞI TẠO FORM ==========

        /// <summary>
        /// Khởi tạo form cho chế độ thêm mới.
        /// </summary>
        private async void InitializeForm()
        {
            try
            {
                Text = @"Thêm mới liên hệ đối tác";
                
                // Đánh dấu các trường bắt buộc theo DataAnnotations của DTO
                RequiredFieldHelper.MarkRequiredFields(this, typeof(BusinessPartnerContactDto));
                
                // Thiết lập SuperToolTip cho các controls
                SetupSuperToolTips();
                
                // Load datasources cho chế độ thêm mới
                await LoadDataSourcesAsync();
                
                var first = FindControlByName(this, "FullNameTextEdit") as BaseEdit;
                first?.Focus();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khởi tạo form");
            }
        }

        #endregion

        #region ========== QUẢN LÝ DỮ LIỆU ==========

        /// <summary>
        /// Load tất cả các nguồn dữ liệu cần thiết cho form
        /// </summary>
        private async Task LoadDataSourcesAsync()
        {
            if (_isLoadingDataSources) return;
            _isLoadingDataSources = true;
            try
            {
                await ExecuteWithWaitingFormAsync(async () =>
                {
                    // Chỉ load datasource cho chế độ thêm mới
                    await LoadBusinessPartnerSitesDataSourceAsync();
                });
            }
            catch (Exception ex)
            {
                MsgBox.ShowError($"Lỗi tải dữ liệu: {ex.Message}");
            }
            finally
            {
                _isLoadingDataSources = false;
            }
        }

        /// <summary>
        /// Load danh sách chi nhánh vào binding source - chỉ hiển thị các chi nhánh đang hoạt động
        /// </summary>
        private async Task LoadBusinessPartnerSitesDataSourceAsync()
        {
            var siteBll = new BusinessPartnerSiteBll();
            // GetAll() already returns List<BusinessPartnerSiteDto>
            var sites = await Task.Run(() => siteBll.GetAll());
            
            // Convert BusinessPartnerSiteDto to BusinessPartnerSiteListDto manually
            // because ToSiteListDtos() expects entities, not DTOs
            var list = sites
                .Select(s => new BusinessPartnerSiteListDto
                {
                    Id = s.Id,
                    PartnerId = s.PartnerId,
                    SiteCode = s.SiteCode,
                    PartnerName = s.PartnerName,
                    PartnerCode = s.PartnerCode,
                    PartnerType = s.PartnerType,
                    PartnerTypeName = s.PartnerTypeName,
                    PartnerTaxCode = s.PartnerTaxCode,
                    PartnerWebsite = s.PartnerWebsite,
                    PartnerPhone = s.PartnerPhone,
                    PartnerEmail = s.PartnerEmail,
                    SiteName = s.SiteName,
                    Address = s.Address,
                    City = s.City,
                    Province = s.Province,
                    Country = s.Country,
                    PostalCode = s.PostalCode,
                    District = s.District,
                    Phone = s.Phone,
                    Email = s.Email,
                    IsDefault = s.IsDefault,
                    IsActive = s.IsActive,
                    SiteType = s.SiteType,
                    SiteTypeName = s.SiteTypeName,
                    Notes = s.Notes,
                    GoogleMapUrl = s.GoogleMapUrl,
                    CreatedDate = s.CreatedDate,
                    UpdatedDate = s.UpdatedDate
                })
                .Where(s => s.IsActive && !string.IsNullOrWhiteSpace(s.SiteName))
                .OrderBy(s => s.PartnerName)
                .ThenBy(s => s.SiteName)
                .ToList();

            if (businessPartnerSiteListDtoBindingSource != null)
            {
                businessPartnerSiteListDtoBindingSource.DataSource = list;
            }

        }

        #endregion

        #region ========== SỰ KIỆN FORM ==========

        /// <summary>
        /// Xử lý sự kiện click button Lưu
        /// </summary>
        private async void SaveBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!ValidateInput()) return;
            
            try
            {
                // Kiểm tra SiteId trước khi lưu
                if (_selectedSiteId == null || _selectedSiteId == Guid.Empty)
                {
                    ShowError("Vui lòng chọn chi nhánh đối tác");
                    return;
                }
                
                await ExecuteWithWaitingFormAsync(() =>
                {
                    var dto = GetDataFromControls();
                    
                    // Kiểm tra và validate avatar trước khi lưu
                    if (dto.Avatar != null && dto.Avatar.Length > 0)
                    {
                        // Kiểm tra kích thước hình ảnh (tối đa 10MB)
                        const int maxSizeInBytes = 10 * 1024 * 1024; // 10MB
                        if (dto.Avatar.Length > maxSizeInBytes)
                        {
                            throw new Exception("Hình ảnh quá lớn! Vui lòng chọn hình ảnh nhỏ hơn 10MB.");
                        }
                        
                        // Kiểm tra format hình ảnh
                        if (!IsValidImageFormat(dto.Avatar))
                        {
                            throw new Exception("Định dạng hình ảnh không được hỗ trợ! Vui lòng chọn file JPG, PNG hoặc GIF.");
                        }
                    }
                    
                    // Lưu DTO (thêm mới) - Id sẽ được tạo tự động trong Add method
                    // Avatar đã được set vào DTO, nên sẽ được lưu cùng lúc
                    var savedId = _bll.Add(dto);
                    return Task.CompletedTask;
                });
                
                ShowInfo("Thêm mới liên hệ đối tác thành công!");
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi thêm mới liên hệ");
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
        
        /// <summary>
        /// Kiểm tra định dạng hình ảnh có hợp lệ không (JPG, PNG, GIF)
        /// </summary>
        private bool IsValidImageFormat(byte[] imageBytes)
        {
            if (imageBytes == null || imageBytes.Length < 4) return false;

            // Kiểm tra magic bytes
            // JPEG: FF D8 FF
            if (imageBytes[0] == 0xFF && imageBytes[1] == 0xD8 && imageBytes[2] == 0xFF)
                return true;

            // PNG: 89 50 4E 47
            if (imageBytes[0] == 0x89 && imageBytes[1] == 0x50 && imageBytes[2] == 0x4E && imageBytes[3] == 0x47)
                return true;

            // GIF: 47 49 46 38 (GIF8)
            if (imageBytes[0] == 0x47 && imageBytes[1] == 0x49 && imageBytes[2] == 0x46 && imageBytes[3] == 0x38)
                return true;

            return false;
        }

        #endregion

        #region ========== XỬ LÝ DỮ LIỆU ==========

        /// <summary>
        /// Thu thập dữ liệu từ các control để tạo DTO lưu xuống DB.
        /// </summary>
        private BusinessPartnerContactDto GetDataFromControls()
        {
            // Không set Id ở đây - để Add method trong BLL tự động tạo Id mới
            // If in edit mode, Id will be set from existing DTO
            var dto = new BusinessPartnerContactDto
            {
                Id = Guid.Empty, // BLL will auto-generate new Id when adding
                SiteId = _selectedSiteId ?? Guid.Empty,
                FullName = FullNameTextEdit?.EditValue?.ToString(),
                Position = PositionTextEdit?.EditValue?.ToString(),
                Phone = PhoneTextEdit?.EditValue?.ToString(),
                Email = EmailTextEdit?.EditValue?.ToString(),
                IsPrimary = IsPrimaryCheckEdit?.Checked ?? false,
                IsActive = IsActiveToggleSwitch?.EditValue as bool? ?? true
            };
            
            // Xử lý avatar từ PictureEdit - BusinessPartnerContactDto uses byte[] for Avatar
            if (AvatarThumbnailDataPictureEdit?.Image != null)
            {
                var imageBytes = ImageToByteArray(AvatarThumbnailDataPictureEdit.Image);
                dto.Avatar = imageBytes;
            }
            else
            {
                dto.Avatar = null;
            }
            
            return dto;
        }
        
        /// <summary>
        /// Convert Image to byte array
        /// </summary>
        private byte[] ImageToByteArray(System.Drawing.Image image)
        {
            if (image == null) return null;
            
            using (var ms = new System.IO.MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Kiểm tra hợp lệ dữ liệu bắt buộc sử dụng dxErrorProvider1
        /// </summary>
        private bool ValidateInput()
        {
            bool isValid = true;
            
            // Clear tất cả errors trước khi validate
            dxErrorProvider1.ClearErrors();

            // Validate Họ và tên (bắt buộc)
            if (string.IsNullOrWhiteSpace(FullNameTextEdit?.EditValue?.ToString()))
            {
                dxErrorProvider1.SetError(FullNameTextEdit, "Họ và tên không được để trống");
                isValid = false;
            }

            // Validate Chi nhánh (bắt buộc)
            if (_selectedSiteId == null || _selectedSiteId == Guid.Empty)
            {
                if (PartnerNameSearchLookUpEdit != null)
                {
                    dxErrorProvider1.SetError(PartnerNameSearchLookUpEdit, "Vui lòng chọn chi nhánh đối tác");
                }
                isValid = false;
            }

            // Validate Email format (nếu có)
            var email = EmailTextEdit?.EditValue?.ToString().Trim();
            if (!string.IsNullOrWhiteSpace(email))
            {
                if (!IsValidEmail(email))
                {
                    dxErrorProvider1.SetError(EmailTextEdit, "Email không đúng định dạng");
                    isValid = false;
                }
            }

            // Validate Phone format (nếu có)
            var phone = PhoneTextEdit?.EditValue?.ToString().Trim();
            if (!string.IsNullOrWhiteSpace(phone))
            {
                if (!IsValidPhone(phone))
                {
                    dxErrorProvider1.SetError(PhoneTextEdit, "Số điện thoại không đúng định dạng");
                    isValid = false;
                }
                else if (_bll.IsPhoneExists(phone, Guid.Empty))
                {
                    dxErrorProvider1.SetError(PhoneTextEdit, "Số điện thoại đã tồn tại trong hệ thống");
                    isValid = false;
                }
            }

            return isValid;
        }

        /// <summary>
        /// Kiểm tra email có hợp lệ không
        /// </summary>
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Kiểm tra số điện thoại có hợp lệ không (chỉ cho phép số, dấu +, dấu - và khoảng trắng)
        /// </summary>
        private bool IsValidPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) return true;
            
            // Loại bỏ khoảng trắng và kiểm tra
            var cleanPhone = phone.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "");
            
            // Kiểm tra có ít nhất 10 ký tự và chỉ chứa số hoặc dấu +
            return cleanPhone.Length >= 10 && 
                   (cleanPhone.All(c => char.IsDigit(c)) || 
                    (cleanPhone.StartsWith("+") && cleanPhone.Substring(1).All(c => char.IsDigit(c))));
        }

        #endregion

        #region ========== TIỆN ÍCH ==========

        /// <summary>
        /// Tìm control theo tên trong cây control
        /// </summary>
        /// <param name="root">Control gốc để tìm kiếm</param>
        /// <param name="name">Tên control cần tìm</param>
        /// <returns>Control tìm thấy hoặc null</returns>
        private static Control FindControlByName(Control root, string name)
        {
            if (root == null || string.IsNullOrWhiteSpace(name)) return null;
            
            // Tìm kiếm đệ quy trong tất cả controls
            return FindControlRecursive(root, name);
        }

        /// <summary>
        /// Tìm kiếm đệ quy control theo tên
        /// </summary>
        /// <param name="parent">Control cha</param>
        /// <param name="name">Tên control cần tìm</param>
        /// <returns>Control tìm thấy hoặc null</returns>
        private static Control FindControlRecursive(Control parent, string name)
        {
            if (parent == null) return null;
            
            // Kiểm tra control hiện tại
            if (parent.Name == name) return parent;
            
            // Tìm kiếm trong tất cả controls con
            foreach (Control child in parent.Controls)
            {
                var found = FindControlRecursive(child, name);
                if (found != null) return found;
            }
            
            return null;
        }

        /// <summary>
        /// Clear error cho control khi user thay đổi dữ liệu
        /// </summary>
        private void ClearErrorForControl(string controlName)
        {
            switch (controlName)
            {
                case "FullNameTextEdit":
                    dxErrorProvider1.SetError(FullNameTextEdit, "");
                    break;
                case "PartnerNameSearchLookUpEdit":
                    dxErrorProvider1.SetError(PartnerNameSearchLookUpEdit, "");
                    break;
                case "EmailTextEdit":
                    dxErrorProvider1.SetError(EmailTextEdit, "");
                    break;
                case "PhoneTextEdit":
                    dxErrorProvider1.SetError(PhoneTextEdit, "");
                    break;
            }
        }

        /// <summary>
        /// Event handler cho PartnerNameSearchLookUpEdit - Lưu SiteId khi user chọn
        /// </summary>
        private void PartnerNameSearchLookUpEdit_EditValueChanged(object sender, EventArgs e)
        {
            if (sender is SearchLookUpEdit lookup)
            {
                if (lookup.EditValue != null && Guid.TryParse(lookup.EditValue.ToString(), out var siteId))
                {
                    _selectedSiteId = siteId;
                }
                else
                {
                    _selectedSiteId = null;
                }
                
                // Clear error khi user chọn
                ClearErrorForControl("PartnerNameSearchLookUpEdit");
            }
        }


        /// <summary>
        /// Hiển thị thông tin.
        /// </summary>
        private void ShowInfo(string message)
        {
            MsgBox.ShowSuccess(message);
        }

        /// <summary>
        /// Hiển thị lỗi.
        /// </summary>
        private void ShowError(string message)
        {
            MsgBox.ShowError(message);
        }

        /// <summary>
        /// Hiển thị lỗi với ngữ cảnh.
        /// </summary>
        private void ShowError(Exception ex, string context = null)
        {
            if (string.IsNullOrWhiteSpace(context))
                MsgBox.ShowException(ex);
            else
                MsgBox.ShowException(new Exception(context + ": " + ex.Message, ex));
        }

        #endregion

        #region ========== TIỆN ÍCH HỖ TRỢ ==========

        /// <summary>
        /// Phím tắt lưu (Ctrl+S) và đóng (Esc).
        /// </summary>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.S))
            {
                SaveBarButtonItem_ItemClick(null, null);
                return true;
            }

            if (keyData == Keys.Escape)
            {
                CloseBarButtonItem_ItemClick(null, null);
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>
        /// Thiết lập SuperToolTip cho các controls trong form
        /// </summary>
        private void SetupSuperToolTips()
        {
            try
            {
                if (FullNameTextEdit != null)
                {
                    SuperToolTipHelper.SetTextEditSuperTip(
                        FullNameTextEdit,
                        title: "<b><color=DarkBlue>👤 Họ và tên</color></b>",
                        content: "Nhập họ và tên đầy đủ của người liên hệ. Trường này là bắt buộc."
                    );
                }

                if (PartnerNameSearchLookUpEdit != null)
                {
                    SuperToolTipHelper.SetBaseEditSuperTip(
                        PartnerNameSearchLookUpEdit,
                        title: "<b><color=DarkBlue>🏢 Chi nhánh đối tác</color></b>",
                        content: "Chọn chi nhánh đối tác mà liên hệ này thuộc về. Trường này là bắt buộc."
                    );
                }

                if (PositionTextEdit != null)
                {
                    SuperToolTipHelper.SetTextEditSuperTip(
                        PositionTextEdit,
                        title: "<b><color=DarkBlue>💼 Chức vụ</color></b>",
                        content: "Nhập chức vụ của người liên hệ."
                    );
                }

                if (PhoneTextEdit != null)
                {
                    SuperToolTipHelper.SetTextEditSuperTip(
                        PhoneTextEdit,
                        title: "<b><color=DarkBlue>📞 Số điện thoại</color></b>",
                        content: "Nhập số điện thoại liên hệ (định dạng: số, dấu +, dấu -, khoảng trắng)."
                    );
                }

                if (EmailTextEdit != null)
                {
                    SuperToolTipHelper.SetTextEditSuperTip(
                        EmailTextEdit,
                        title: "<b><color=DarkBlue>📧 Email</color></b>",
                        content: "Nhập địa chỉ email liên hệ (định dạng: example@domain.com)."
                    );
                }

                if (IsPrimaryCheckEdit != null)
                {
                    SuperToolTipHelper.SetBaseEditSuperTip(
                        IsPrimaryCheckEdit,
                        title: "<b><color=DarkBlue>⭐ Liên hệ chính</color></b>",
                        content: "Đánh dấu nếu đây là liên hệ chính của chi nhánh."
                    );
                }

                if (IsActiveToggleSwitch != null)
                {
                    SuperToolTipHelper.SetBaseEditSuperTip(
                        IsActiveToggleSwitch,
                        title: "<b><color=DarkBlue>🔄 Trạng thái</color></b>",
                        content: "Bật/tắt trạng thái hoạt động của liên hệ."
                    );
                }

                if (AvatarThumbnailDataPictureEdit != null)
                {
                    SuperToolTipHelper.SetBaseEditSuperTip(
                        AvatarThumbnailDataPictureEdit,
                        title: "<b><color=DarkBlue>📷 Ảnh đại diện</color></b>",
                        content: "Chọn ảnh đại diện cho liên hệ (JPG, PNG, GIF, tối đa 10MB)."
                    );
                }

                if (SaveBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        SaveBarButtonItem,
                        title: "<b><color=Blue>💾 Lưu</color></b>",
                        content: "Lưu thông tin liên hệ đối tác vào hệ thống."
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

        /// <summary>
        /// Thực thi operation với splash screen
        /// </summary>
        private async Task ExecuteWithWaitingFormAsync(Func<Task> operation)
        {
            try
            {
                SplashScreenManager.ShowForm(typeof(WaitForm1));
                await operation();
            }
            finally
            {
                SplashScreenManager.CloseForm();
            }
        }

        #endregion
    }
}