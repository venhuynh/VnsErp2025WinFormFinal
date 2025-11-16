using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bll.Common;
using Bll.MasterData.Customer;
using Bll.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraLayout;
using MasterData.Customer.Converters;
using MasterData.Customer.Dto;

namespace MasterData.Customer
{
    /// <summary>
    /// Form thêm mới/sửa chi nhánh đối tác.
    /// Cung cấp giao diện nhập liệu đầy đủ với validation và lookup đối tác.
    /// </summary>
    public partial class FrmBusinessPartnerSiteDetail : XtraForm
    {
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
            }

            // Setup validation
            SetupValidation();
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
            ContactPersonTextEdit.EditValue = _currentSite.ContactPerson;
            PhoneTextEdit.EditValue = _currentSite.Phone;
            EmailTextEdit.EditValue = _currentSite.Email;
            IsActiveCheckEdit.EditValue = _currentSite.IsActive;
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

                using var splash = new WaitForm1();

                splash.Show();
                splash.Update();

                var success = await SaveData();

                if (success)
                {
                    MsgBox.ShowSuccess(_isEditMode
                        ? "Cập nhật chi nhánh thành công!"
                        : "Thêm mới chi nhánh thành công!");
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MsgBox.ShowError("Không thể lưu dữ liệu. Vui lòng thử lại.");
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowError($"Lỗi lưu dữ liệu: {ex.Message}");
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
                ContactPerson = ContactPersonTextEdit.Text?.Trim(),
                Phone = PhoneTextEdit.Text?.Trim(),
                Email = EmailTextEdit.Text?.Trim(),
                IsActive = (bool)IsActiveCheckEdit.EditValue
            };
        }

        /// <summary>
        /// Lưu dữ liệu (thêm mới/cập nhật) khi người dùng bấm Lưu.
        /// </summary>
        private async Task<bool> SaveData()
        {
            try
            {
                var siteDto = GetDataFromControls();

                // Chuyển đổi DTO sang Entity (tuân thủ quy tắc kiến trúc)
                var entity = siteDto.ToEntity();

                if (_isEditMode)
                {
                    return await Task.Run(() => _businessPartnerSiteBll.UpdateSite(entity));
                }

                return await Task.Run(() => _businessPartnerSiteBll.CreateSite(entity));
            }
            catch (Exception ex)
            {
                MsgBox.ShowError($"Lỗi lưu dữ liệu: {ex.Message}");
                return false;
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
            MarkRequiredFields(typeof(BusinessPartnerSiteDto));
        }

        #endregion

        #region ========== TIỆN ÍCH HỖ TRỢ ==========

        /// <summary>
        /// Đánh dấu các layout item tương ứng với thuộc tính có [Required] bằng dấu * đỏ.
        /// Quy ước mapping control theo tên thuộc tính (từ editor được gán vào LayoutControlItem.Control):
        /// - Editor: "txt" + PropertyName, PropertyName + "TextEdit", hoặc chính PropertyName (BaseEdit)
        /// </summary>
        private void MarkRequiredFields(Type dtoType)
        {
            try
            {
                var requiredProps = dtoType
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.GetCustomAttributes(typeof(RequiredAttribute),
                        true).Any())
                    .ToList();

                var allLayoutItems = GetAllLayoutControlItems(this);

                foreach (var it in allLayoutItems)
                {
                    it.AllowHtmlStringInCaption = true;
                }

                foreach (var prop in requiredProps)
                {
                    var propName = prop.Name;
                    var item = allLayoutItems.FirstOrDefault(it => IsEditorMatchProperty(it.Control, propName));
                    if (item == null) continue;

                    if (!(item.Text?.Contains("*") ?? false))
                    {
                        var baseCaption = string.IsNullOrWhiteSpace(item.Text) ? propName : item.Text;
                        item.Text = baseCaption + @" <color=red>*</color>";
                    }

                    if (item.Control is BaseEdit be &&
                        be.Properties is RepositoryItemTextEdit txtProps)
                    {
                        txtProps.NullValuePrompt = @"Bắt buộc nhập";
                        txtProps.NullValuePromptShowForEmptyValue = true;
                    }
                }
            }
            catch
            {
                // ignore marking errors
            }
        }

        private static bool IsEditorMatchProperty(Control editor, string propName)
        {
            if (editor == null) return false;
            var name = editor.Name ?? string.Empty;
            string[] candidates =
            {
                name,
                name.Replace("txt", string.Empty),
                name.Replace("TextEdit", string.Empty)
            };
            return candidates.Any(c => string.Equals(c, propName, StringComparison.OrdinalIgnoreCase));
        }

        private static List<LayoutControlItem> GetAllLayoutControlItems(Control root)
        {
            var result = new List<LayoutControlItem>();
            if (root == null) return result;
            var layoutControls = root.Controls.OfType<LayoutControl>().ToList();
            var nested = root.Controls.Cast<Control>().SelectMany(GetAllLayoutControlItems).ToList();
            foreach (var lc in layoutControls)
            {
                if (lc.Root != null)
                {
                    CollectLayoutItems(lc.Root, result);
                }
            }

            result.AddRange(nested);
            return result;
        }

        private static void CollectLayoutItems(BaseLayoutItem baseItem,
            List<LayoutControlItem> collector)
        {
            switch (baseItem)
            {
                case null:
                    return;
                case LayoutControlItem lci:
                    collector.Add(lci);
                    break;
                case LayoutControlGroup group:
                {
                    foreach (BaseLayoutItem child in group.Items)
                    {
                        CollectLayoutItems(child, collector);
                    }

                    break;
                }
            }
        }

        #endregion
    }
}