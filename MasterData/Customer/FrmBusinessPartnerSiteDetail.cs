using Bll.MasterData.CustomerBll;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraSplashScreen;
using DTO.MasterData.CustomerPartner;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common.Common;
using Common.Utils;

namespace MasterData.Customer
{
    /// <summary>
    /// Form thêm mới/sửa chi nhánh đối tác.
    /// Cung cấp giao diện nhập liệu đầy đủ với validation và lookup đối tác.
    /// </summary>
    public partial class FrmBusinessPartnerSiteDetail : XtraForm
    {
        #region ========== EVENTS ==========

        /// <summary>
        /// Event được trigger khi lưu thành công, trả về DTO đã được cập nhật
        /// </summary>
        public event Action<BusinessPartnerSiteListDto> SiteSaved;

        #endregion

        #region ========== KHAI BÁO BIẾN ==========

        /// <summary>
        /// Business Logic Layer cho chi nhánh đối tác
        /// </summary>
        private readonly BusinessPartnerSiteBll _businessPartnerSiteBll;

        /// <summary>
        /// Business Logic Layer cho đối tác
        /// </summary>
        private readonly BusinessPartnerBll _businessPartnerBll;

        /// <summary>
        /// ID chi nhánh đối tác được chọn
        /// </summary>
        private readonly Guid _siteId;

        /// <summary>
        /// Dữ liệu chi nhánh hiện tại
        /// </summary>
        private BusinessPartnerSiteDto _currentSite;

        /// <summary>
        /// Trạng thái chỉnh sửa
        /// </summary>
        private readonly bool _isEditMode;

        #endregion

        #region ========== CONSTRUCTOR & PUBLIC METHODS ==========

        /// <summary>
        /// Khởi tạo form cho chế độ thêm mới/sửa chi nhánh đối tác.
        /// </summary>
        /// <param name="siteId">ID chi nhánh (Guid.Empty cho thêm mới)</param>
        public FrmBusinessPartnerSiteDetail(Guid siteId)
        {
            InitializeComponent();
            _businessPartnerSiteBll = new BusinessPartnerSiteBll();
            _businessPartnerBll = new BusinessPartnerBll();
            _siteId = siteId;
            _isEditMode = siteId != Guid.Empty;

            InitializeForm();

            SaveBarButtonItem.ItemClick += SaveBarButtonItem_ItemClick;
            CloseBarButtonItem.ItemClick += CloseBarButtonItem_ItemClick;
            PartnerNameTextEdit.EditValueChanged += PartnerNameTextEdit_EditValueChanged;
        }

        #endregion

        #region ========== KHỞI TẠO FORM ==========

        /// <summary>
        /// Khởi tạo form cho chế độ thêm mới/sửa.
        /// </summary>
        private void InitializeForm()
        {
            // Cấu hình form
            Text = _isEditMode ? "Chỉnh sửa chi nhánh" : "Thêm mới chi nhánh";

            // Load dữ liệu Business Partner
            LoadBusinessPartners();

            // Load dữ liệu site nếu là edit mode
            if (_isEditMode)
            {
                LoadSiteData();
            }
            else
            {
                // Set default values for new site
                IsActiveCheckEdit.EditValue = true;
                IsDefaultCheckEdit.EditValue = false;
                SiteTypeComboBoxEdit.SelectedIndex = -1; // Không chọn mặc định
            }

            // Setup validation
            SetupValidation();
            
            // Thiết lập SuperToolTip cho các controls
            SetupSuperToolTips();
        }

        #endregion

        #region ========== QUẢN LÝ DỮ LIỆU ==========

        /// <summary>
        /// Load danh sách đối tác vào binding source
        /// </summary>
        private async void LoadBusinessPartners()
        {
            try
            {
                var partners = await Task.Run(() => _businessPartnerBll.GetAll());
                var partnerListDtos = partners.ToBusinessPartnerListDtos().ToList();

                businessPartnerListDtoBindingSource.DataSource = partnerListDtos;
            }
            catch (Exception ex)
            {
                MsgBox.ShowError($"Lỗi tải danh sách đối tác: {ex.Message}");
            }
        }

        /// <summary>
        /// Load dữ liệu chi nhánh để chỉnh sửa
        /// </summary>
        private async void LoadSiteData()
        {
            try
            {
                var site = await Task.Run(() => _businessPartnerSiteBll.GetById(_siteId));
                if (site != null)
                {
                    _currentSite = site.ToSiteDto();
                    BindDataToControls();
                }
                else
                {
                    MsgBox.ShowError("Không tìm thấy thông tin chi nhánh.");
                    Close();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowError($"Lỗi tải dữ liệu chi nhánh: {ex.Message}");
            }
        }

        /// <summary>
        /// Bind dữ liệu chi nhánh vào các control
        /// </summary>
        private void BindDataToControls()
        {
            if (_currentSite == null) return;

            // Bind data to controls
            PartnerNameTextEdit.EditValue = _currentSite.PartnerId;
            SiteCodeTextEdit.EditValue = _currentSite.SiteCode;
            SiteNameTextEdit.EditValue = _currentSite.SiteName;
            AddressTextEdit.EditValue = _currentSite.Address;
            CityTextEdit.EditValue = _currentSite.City;
            ProvinceTextEdit.EditValue = _currentSite.Province;
            CountryTextEdit.EditValue = _currentSite.Country;
            PostalCodeTextEdit.EditValue = _currentSite.PostalCode;
            DistrictTextEdit.EditValue = _currentSite.District;
            PhoneTextEdit.EditValue = _currentSite.Phone;
            EmailTextEdit.EditValue = _currentSite.Email;
            IsActiveCheckEdit.EditValue = _currentSite.IsActive;
            IsDefaultCheckEdit.EditValue = _currentSite.IsDefault ?? false;
            
            // Bind SiteType - map từ int? sang index của ComboBox
            if (_currentSite.SiteType.HasValue)
            {
                var siteTypeIndex = _currentSite.SiteType.Value - 1; // 1->0, 2->1, 3->2, 4->3
                if (siteTypeIndex >= 0 && siteTypeIndex < SiteTypeComboBoxEdit.Properties.Items.Count)
                {
                    SiteTypeComboBoxEdit.SelectedIndex = siteTypeIndex;
                }
            }
            else
            {
                SiteTypeComboBoxEdit.SelectedIndex = -1;
            }
            
            NotesMemoEdit.EditValue = _currentSite.Notes;
            GoogleMapUrlTextEdit.EditValue = _currentSite.GoogleMapUrl;
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
                    await SaveBusinessPartnerSiteAsync();
                });

                // Thông báo thành công và đóng form
                MsgBox.ShowSuccess(_isEditMode
                    ? "Cập nhật chi nhánh thành công!"
                    : "Thêm mới chi nhánh thành công!");
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lưu dữ liệu chi nhánh");
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
        /// Xử lý sự kiện thay đổi đối tác - tự động tạo mã chi nhánh
        /// </summary>
        private void PartnerNameTextEdit_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                // Chỉ tự động tạo mã chi nhánh khi thêm mới (không phải edit)
                if (_isEditMode) return;

                // Lấy đối tác được chọn
                if (PartnerNameTextEdit?.EditValue == null) return;

                var selectedPartnerId = (Guid)PartnerNameTextEdit.EditValue;
                var selectedPartner = businessPartnerListDtoBindingSource.Cast<BusinessPartnerListDto>()
                    .FirstOrDefault(p => p.Id == selectedPartnerId);

                if (selectedPartner == null) return;

                // Tự động tạo mã chi nhánh với định dạng: MãĐốiTác-XX
                var partnerCode = selectedPartner.PartnerCode?.Trim();
                if (string.IsNullOrWhiteSpace(partnerCode)) return;

                // Tìm mã chi nhánh tiếp theo cho đối tác này
                var nextSiteCode = GenerateNextSiteCode(partnerCode);

                // Chỉ set nếu SiteCode đang trống hoặc chưa có giá trị
                if (string.IsNullOrWhiteSpace(SiteCodeTextEdit?.Text))
                {
                    if (SiteCodeTextEdit != null) SiteCodeTextEdit.EditValue = nextSiteCode;
                }
            }
            catch (Exception ex)
            {
                // Log lỗi nhưng không làm gián đoạn user experience
                Debug.WriteLine($"Lỗi tự động tạo mã chi nhánh: {ex.Message}");
            }
        }

        #endregion

        #region ========== XỬ LÝ DỮ LIỆU ==========

        /// <summary>
        /// Thu thập dữ liệu từ các control để tạo DTO lưu xuống DB.
        /// </summary>
        private BusinessPartnerSiteDto GetDataFromControls()
        {
            // Map SiteType từ ComboBox index sang int? (0->1, 1->2, 2->3, 3->4)
            int? siteType = null;
            if (SiteTypeComboBoxEdit.SelectedIndex >= 0)
            {
                siteType = SiteTypeComboBoxEdit.SelectedIndex + 1;
            }

            return new BusinessPartnerSiteDto
            {
                Id = _currentSite?.Id ?? Guid.Empty,
                PartnerId = (Guid)PartnerNameTextEdit.EditValue,
                SiteCode = SiteCodeTextEdit.Text.Trim(),
                SiteName = SiteNameTextEdit.Text.Trim(),
                Address = AddressTextEdit.Text?.Trim(),
                City = CityTextEdit.Text?.Trim(),
                Province = ProvinceTextEdit.Text?.Trim(),
                Country = CountryTextEdit.Text?.Trim(),
                PostalCode = PostalCodeTextEdit.Text?.Trim(),
                District = DistrictTextEdit.Text?.Trim(),
                Phone = PhoneTextEdit.Text?.Trim(),
                Email = EmailTextEdit.Text?.Trim(),
                IsActive = (bool)IsActiveCheckEdit.EditValue,
                IsDefault = IsDefaultCheckEdit.EditValue as bool?,
                SiteType = siteType,
                Notes = NotesMemoEdit.Text?.Trim(),
                GoogleMapUrl = GoogleMapUrlTextEdit.Text?.Trim()
            };
        }

        /// <summary>
        /// Lưu dữ liệu chi nhánh và trigger event SiteSaved
        /// </summary>
        private async Task SaveBusinessPartnerSiteAsync()
        {
            // Bước 1: Thu thập dữ liệu từ form và build DTO
            var siteDto = GetDataFromControls();

            // Bước 2: Convert DTO -> Entity
            var existing = _isEditMode ? _businessPartnerSiteBll.GetById(siteDto.Id) : null;
            var entity = siteDto.ToEntity(existing);

            // Bước 3: Lưu entity qua BLL
            Guid savedSiteId;
            if (_isEditMode)
            {
                var success = await Task.Run(() => _businessPartnerSiteBll.UpdateSite(entity));
                if (!success)
                {
                    throw new Exception("Không thể cập nhật chi nhánh. Có thể mã chi nhánh đã tồn tại.");
                }
                savedSiteId = entity.Id;
            }
            else
            {
                var success = await Task.Run(() => _businessPartnerSiteBll.CreateSite(entity));
                if (!success)
                {
                    throw new Exception("Không thể tạo mới chi nhánh. Có thể mã chi nhánh đã tồn tại.");
                }
                // Entity.Id đã được set trong SaveOrUpdate (trong CreateSite)
                savedSiteId = entity.Id;
            }

            // Bước 4: Lấy lại entity đã lưu và convert sang BusinessPartnerSiteListDto để trigger event
            var savedEntity = await Task.Run(() => _businessPartnerSiteBll.GetById(savedSiteId));
            if (savedEntity != null)
            {
                // Convert single entity sang ListDto (sử dụng ToSiteListDtos với một phần tử)
                var listDtos = new[] { savedEntity }.ToSiteListDtos().ToList();
                var listDto = listDtos.FirstOrDefault();
                
                // Trigger event để form cha có thể update datasource
                if (listDto != null)
                {
                    SiteSaved?.Invoke(listDto);
                }
            }
        }

        /// <summary>
        /// Kiểm tra hợp lệ dữ liệu bắt buộc sử dụng dxErrorProvider1
        /// </summary>
        private bool ValidateForm()
        {
            dxErrorProvider1.ClearErrors();

            // SiteCode bắt buộc
            if (string.IsNullOrWhiteSpace(SiteCodeTextEdit?.Text))
            {
                dxErrorProvider1.SetError(SiteCodeTextEdit, "Mã chi nhánh không được để trống",
                    ErrorType.Critical);
                SiteCodeTextEdit?.Focus();
                return false;
            }

            // SiteName bắt buộc
            if (string.IsNullOrWhiteSpace(SiteNameTextEdit?.Text))
            {
                dxErrorProvider1.SetError(SiteNameTextEdit, "Tên chi nhánh không được để trống",
                    ErrorType.Critical);
                SiteNameTextEdit?.Focus();
                return false;
            }

            // PartnerName bắt buộc
            if (PartnerNameTextEdit?.EditValue == null)
            {
                dxErrorProvider1.SetError(PartnerNameTextEdit, "Vui lòng chọn đối tác",
                    ErrorType.Critical);
                PartnerNameTextEdit?.Focus();
                return false;
            }

            // Email validation
            if (!string.IsNullOrWhiteSpace(EmailTextEdit?.Text) &&
                !Regex.IsMatch(EmailTextEdit.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                dxErrorProvider1.SetError(EmailTextEdit, "Email không đúng định dạng",
                    ErrorType.Warning);
                EmailTextEdit?.Focus();
                return false;
            }

            return true;
        }

        #endregion

        #region ========== TIỆN ÍCH ==========

        /// <summary>
        /// Tạo mã chi nhánh tiếp theo cho đối tác
        /// </summary>
        /// <param name="partnerCode">Mã đối tác</param>
        /// <returns>Mã chi nhánh tiếp theo (ví dụ: PARTNER-01, PARTNER-02)</returns>
        private string GenerateNextSiteCode(string partnerCode)
        {
            try
            {
                // Lấy tất cả mã chi nhánh hiện có cho đối tác này
                var existingSiteCodes = GetExistingSiteCodesForPartner(partnerCode);

                // Tìm số thứ tự tiếp theo
                var nextNumber = 1;
                var pattern = $"{partnerCode}-";

                foreach (var existingCode in existingSiteCodes)
                {
                    if (existingCode.StartsWith(pattern))
                    {
                        var suffix = existingCode.Substring(pattern.Length);
                        if (int.TryParse(suffix, out var number))
                        {
                            nextNumber = Math.Max(nextNumber, number + 1);
                        }
                    }
                }

                // Tạo mã mới với định dạng: PARTNER-01, PARTNER-02, etc.
                return $"{partnerCode}-{nextNumber:D2}";
            }
            catch
            {
                // Fallback: trả về mã đơn giản
                return $"{partnerCode}-01";
            }
        }

        /// <summary>
        /// Lấy danh sách mã chi nhánh hiện có cho đối tác
        /// </summary>
        /// <param name="partnerCode">Mã đối tác</param>
        /// <returns>Danh sách mã chi nhánh</returns>
        private List<string> GetExistingSiteCodesForPartner(string partnerCode)
        {
            try
            {
                // Lấy tất cả chi nhánh hiện có
                var allSites = _businessPartnerSiteBll.GetAll();

                // Lọc các chi nhánh của đối tác này
                var partnerSites = allSites.Where(s =>
                        s.BusinessPartner?.PartnerCode == partnerCode)
                    .Select(s => s.SiteCode)
                    .Where(code => !string.IsNullOrWhiteSpace(code))
                    .ToList();

                return partnerSites;
            }
            catch
            {
                // Fallback: trả về danh sách rỗng
                return [];
            }
        }

        /// <summary>
        /// Setup validation cho form
        /// </summary>
        private void SetupValidation()
        {
            // Đánh dấu các trường bắt buộc theo DataAnnotations của DTO
            RequiredFieldHelper.MarkRequiredFields(this, typeof(BusinessPartnerSiteDto));
        }

        #endregion

        #region ========== TIỆN ÍCH ==========

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
                if (PartnerNameTextEdit != null)
                {
                    SuperToolTipHelper.SetBaseEditSuperTip(
                        PartnerNameTextEdit,
                        title: "<b><color=DarkBlue>🏢 Đối tác</color></b>",
                        content: "Chọn đối tác mà chi nhánh này thuộc về. Trường này là bắt buộc."
                    );
                }

                if (SiteCodeTextEdit != null)
                {
                    SuperToolTipHelper.SetTextEditSuperTip(
                        SiteCodeTextEdit,
                        title: "<b><color=DarkBlue>🔖 Mã chi nhánh</color></b>",
                        content: "Nhập mã chi nhánh duy nhất. Trường này là bắt buộc. Mã sẽ được tự động tạo khi chọn đối tác (chế độ thêm mới)."
                    );
                }

                if (SiteNameTextEdit != null)
                {
                    SuperToolTipHelper.SetTextEditSuperTip(
                        SiteNameTextEdit,
                        title: "<b><color=DarkBlue>📍 Tên chi nhánh</color></b>",
                        content: "Nhập tên chi nhánh. Trường này là bắt buộc."
                    );
                }

                if (AddressTextEdit != null)
                {
                    SuperToolTipHelper.SetTextEditSuperTip(
                        AddressTextEdit,
                        title: "<b><color=DarkBlue>🏠 Địa chỉ</color></b>",
                        content: "Nhập địa chỉ chi tiết của chi nhánh."
                    );
                }

                if (PhoneTextEdit != null)
                {
                    SuperToolTipHelper.SetTextEditSuperTip(
                        PhoneTextEdit,
                        title: "<b><color=DarkBlue>📞 Số điện thoại</color></b>",
                        content: "Nhập số điện thoại liên hệ của chi nhánh."
                    );
                }

                if (EmailTextEdit != null)
                {
                    SuperToolTipHelper.SetTextEditSuperTip(
                        EmailTextEdit,
                        title: "<b><color=DarkBlue>📧 Email</color></b>",
                        content: "Nhập địa chỉ email liên hệ của chi nhánh."
                    );
                }

                if (PostalCodeTextEdit != null)
                {
                    SuperToolTipHelper.SetTextEditSuperTip(
                        PostalCodeTextEdit,
                        title: "<b><color=DarkBlue>📮 Mã bưu điện</color></b>",
                        content: "Nhập mã bưu điện của chi nhánh."
                    );
                }

                if (DistrictTextEdit != null)
                {
                    SuperToolTipHelper.SetTextEditSuperTip(
                        DistrictTextEdit,
                        title: "<b><color=DarkBlue>🏘️ Quận/Huyện</color></b>",
                        content: "Nhập quận/huyện của chi nhánh."
                    );
                }

                if (IsDefaultCheckEdit != null)
                {
                    SuperToolTipHelper.SetBaseEditSuperTip(
                        IsDefaultCheckEdit,
                        title: "<b><color=DarkBlue>⭐ Mặc định</color></b>",
                        content: "Đánh dấu chi nhánh này là địa chỉ mặc định của đối tác."
                    );
                }

                if (SiteTypeComboBoxEdit != null)
                {
                    SuperToolTipHelper.SetBaseEditSuperTip(
                        SiteTypeComboBoxEdit,
                        title: "<b><color=DarkBlue>🏢 Loại địa điểm</color></b>",
                        content: "Chọn loại địa điểm: Trụ sở chính, Chi nhánh, Kho hàng, hoặc Văn phòng đại diện."
                    );
                }

                if (NotesMemoEdit != null)
                {
                    SuperToolTipHelper.SetBaseEditSuperTip(
                        NotesMemoEdit,
                        title: "<b><color=DarkBlue>📝 Ghi chú</color></b>",
                        content: "Nhập ghi chú bổ sung về chi nhánh (tối đa 1000 ký tự)."
                    );
                }

                if (GoogleMapUrlTextEdit != null)
                {
                    SuperToolTipHelper.SetTextEditSuperTip(
                        GoogleMapUrlTextEdit,
                        title: "<b><color=DarkBlue>🗺️ Google Map URL</color></b>",
                        content: "Nhập đường dẫn Google Map của chi nhánh (tối đa 1000 ký tự)."
                    );
                }

                if (SaveBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        SaveBarButtonItem,
                        title: "<b><color=Blue>💾 Lưu</color></b>",
                        content: "Lưu thông tin chi nhánh đối tác vào hệ thống."
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