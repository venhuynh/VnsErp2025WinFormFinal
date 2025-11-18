using Bll.MasterData.Customer;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.DXErrorProvider;
using DTO.MasterData.CustomerPartner;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bll.MasterData.CustomerBll;

namespace MasterData.Customer
{
    public partial class FrmBusinessPartnerDetail : XtraForm
    {
        #region Fields

        private readonly BusinessPartnerBll _bll = new BusinessPartnerBll();
        private readonly Guid _businessPartnerId;

        #endregion

        #region Constructor

        public FrmBusinessPartnerDetail(Guid businessPartnerId)
        {
            _businessPartnerId = businessPartnerId;

            InitializeComponent();
            Shown += FrmBusinessPartnerDetail_Shown;
        }

        #endregion
         

        /// <summary>
        /// Khi form hiển thị: đánh dấu các trường bắt buộc theo DataAnnotations của DTO.
        /// </summary>
        /// <summary>
        /// Form shown: mark required captions, setup partner type list, load detail if editing.
        /// </summary>
        private void FrmBusinessPartnerDetail_Shown(object sender, EventArgs e)
        {
            try
            {
                // Đánh dấu Required dựa trên BusinessPartnerListDto
                RequiredFieldHelper.MarkRequiredFields(this, typeof(BusinessPartnerListDto));
                
                // Thiết lập SuperToolTip cho các controls
                SetupSuperToolTips();

                // Cấu hình combobox Loại đối tác
                SetupPartnerTypeComboBox();

                // Nếu chỉnh sửa thì nạp dữ liệu chi tiết
                if (_businessPartnerId != Guid.Empty)
                {
                    _ = LoadDetailAsync(_businessPartnerId);
                }
            }
            catch (Exception)
            {
                // ignore - tránh chặn hiển thị form nếu không tìm thấy control tương ứng
            }
        }

        /// <summary>
        /// Thiết lập SuperToolTip cho các controls trong form
        /// </summary>
        private void SetupSuperToolTips()
        {
            try
            {
                if (PartnerCodeTextEdit != null)
                {
                    SuperToolTipHelper.SetTextEditSuperTip(
                        PartnerCodeTextEdit,
                        title: "<b><color=DarkBlue>🔖 Mã đối tác</color></b>",
                        content: "Nhập mã đối tác duy nhất trong hệ thống. Trường này là bắt buộc."
                    );
                }

                if (PartnerNameTextEdit != null)
                {
                    SuperToolTipHelper.SetTextEditSuperTip(
                        PartnerNameTextEdit,
                        title: "<b><color=DarkBlue>🏢 Tên đối tác</color></b>",
                        content: "Nhập tên đầy đủ của đối tác. Trường này là bắt buộc."
                    );
                }

                if (PartnerTypeNameComboBoxEdit != null)
                {
                    SuperToolTipHelper.SetBaseEditSuperTip(
                        PartnerTypeNameComboBoxEdit,
                        title: "<b><color=DarkBlue>📂 Loại đối tác</color></b>",
                        content: "Chọn loại đối tác: Khách hàng, Nhà cung cấp, hoặc cả hai."
                    );
                }

                if (TaxCodeTextEdit != null)
                {
                    SuperToolTipHelper.SetTextEditSuperTip(
                        TaxCodeTextEdit,
                        title: "<b><color=DarkBlue>📋 Mã số thuế</color></b>",
                        content: "Nhập mã số thuế của đối tác."
                    );
                }

                if (PhoneTextEdit != null)
                {
                    SuperToolTipHelper.SetTextEditSuperTip(
                        PhoneTextEdit,
                        title: "<b><color=DarkBlue>📞 Số điện thoại</color></b>",
                        content: "Nhập số điện thoại liên hệ của đối tác."
                    );
                }

                if (EmailTextEdit != null)
                {
                    SuperToolTipHelper.SetTextEditSuperTip(
                        EmailTextEdit,
                        title: "<b><color=DarkBlue>📧 Email</color></b>",
                        content: "Nhập địa chỉ email liên hệ của đối tác."
                    );
                }

                if (SaveBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        SaveBarButtonItem,
                        title: "<b><color=Blue>💾 Lưu</color></b>",
                        content: "Lưu thông tin đối tác vào hệ thống."
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
        /// Cấu hình dữ liệu cho ComboBoxEdit 'PartnerTypeNameComboBoxEdit' với 3 lựa chọn chuẩn.
        /// </summary>
        private void SetupPartnerTypeComboBox()
        {
            var combo = FindControlByName(this, "PartnerTypeNameComboBoxEdit") as ComboBoxEdit;
            if (combo == null) return;

            combo.Properties.Items.Clear();
            combo.Properties.Items.AddRange(new[]
            {
                "Khách hàng",
                "Nhà cung cấp",
                "Khách hàng & Nhà cung cấp"
            });
            combo.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
        }

        private static Control FindControlByName(Control root, string name)
        {
            if (root == null || string.IsNullOrWhiteSpace(name)) return null;
            var found = root.Controls.Find(name, true).FirstOrDefault();
            return found;
        }

        /// <summary>
        /// Nạp dữ liệu chi tiết đối tác theo Id vào các control (asynchronous).
        /// </summary>
        private async Task LoadDetailAsync(Guid id)
        {
            try
            {
                // Gọi BLL lấy entity (có thể chuyển sang DTO nếu cần)
                var entity = await _bll.GetByIdAsync(id);
                if (entity == null) return;

                // Map trực tiếp vào các control đã được Designer tạo tương ứng BusinessPartnerListDto + mở rộng
                if (PartnerCodeTextEdit != null) PartnerCodeTextEdit.EditValue = entity.PartnerCode;
                if (PartnerNameTextEdit != null) PartnerNameTextEdit.EditValue = entity.PartnerName;
                if (TaxCodeTextEdit != null) TaxCodeTextEdit.EditValue = entity.TaxCode;
                if (PhoneTextEdit != null) PhoneTextEdit.EditValue = entity.Phone;
                if (EmailTextEdit != null) EmailTextEdit.EditValue = entity.Email;
                if (WebsiteTextEdit != null) WebsiteTextEdit.EditValue = entity.Website;
                if (AddressTextEdit != null) AddressTextEdit.EditValue = entity.Address;
                if (CityTextEdit != null) CityTextEdit.EditValue = entity.City;
                if (CountryTextEdit != null) CountryTextEdit.EditValue = entity.Country;
                if (ContactPersonTextEdit != null) ContactPersonTextEdit.EditValue = entity.ContactPerson;
                if (ContactPositionTextEdit != null) ContactPositionTextEdit.EditValue = entity.ContactPosition;
                if (BankAccountTextEdit != null) BankAccountTextEdit.EditValue = entity.BankAccount;
                if (BankNameTextEdit != null) BankNameTextEdit.EditValue = entity.BankName;
                if (CreditLimitTextEdit != null) CreditLimitTextEdit.EditValue = entity.CreditLimit;
                if (PaymentTermTextEdit != null) PaymentTermTextEdit.EditValue = entity.PaymentTerm;
                if (IsActiveToggleSwitch != null) IsActiveToggleSwitch.EditValue = entity.IsActive;

                if (PartnerTypeNameComboBoxEdit != null)
                {
                    switch (entity.PartnerType)
                    {
                        case 1: PartnerTypeNameComboBoxEdit.SelectedIndex = 0; break;
                        case 2: PartnerTypeNameComboBoxEdit.SelectedIndex = 1; break;
                        case 3: PartnerTypeNameComboBoxEdit.SelectedIndex = 2; break;
                        default: PartnerTypeNameComboBoxEdit.SelectedIndex = -1; break;
                    }
                }
            }
            catch (Exception)
            {
                // ignore lỗi nạp chi tiết để không chặn UI
            }
        }

        private void SetTextIfExist(string controlName, string value)
        {
            if (FindControlByName(this, controlName) is BaseEdit edit)
            {
                edit.EditValue = value;
            }
        }

        #region Event Handlers

        /// <summary>
        /// Lưu dữ liệu (thêm mới/cập nhật) khi người dùng bấm Lưu.
        /// </summary>
        private void SaveBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                // 1) Validate input theo mẫu
                if (!ValidateInput()) return;

                // 2) Gom dữ liệu vào DTO (Detail) để chuyển đổi qua converter
                var detailDto = BuildDetailDtoFromForm();

                // 3) Converter DTO -> Entity và lưu qua BLL
                var existing = detailDto.Id != Guid.Empty ? _bll.GetById(detailDto.Id) : null;
                var entity = detailDto.ToEntity(existing);
                _bll.SaveOrUpdate(entity);

                MsgBox.ShowSuccess("Lưu dữ liệu đối tác thành công");
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi lưu dữ liệu đối tác");
            }
        }

        /// <summary>
        /// Đóng form khi người dùng bấm Đóng.
        /// </summary>
        private void CloseBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            Close();
        }

        #endregion

        /// <summary>
        /// Validate input theo thứ tự, đặt lỗi và focus control đầu tiên không hợp lệ.
        /// </summary>
        /// <summary>
        /// Validate input theo thứ tự, đặt lỗi và focus control không hợp lệ đầu tiên.
        /// </summary>
        private bool ValidateInput()
        {
            dxErrorProvider1.ClearErrors();

            // PartnerCode bắt buộc
            if (string.IsNullOrWhiteSpace(PartnerCodeTextEdit?.Text))
            {
                dxErrorProvider1.SetError(PartnerCodeTextEdit, "Mã đối tác không được để trống", ErrorType.Critical);
                PartnerCodeTextEdit?.Focus();
                return false;
            }

            // Kiểm tra trùng lặp PartnerCode (chỉ khi thêm mới hoặc mã đã thay đổi)
            var partnerCode = PartnerCodeTextEdit.Text.Trim();
            if (_businessPartnerId == Guid.Empty) // Thêm mới
            {
                if (_bll.IsCodeExists(partnerCode))
                {
                    dxErrorProvider1.SetError(PartnerCodeTextEdit, "Mã đối tác đã tồn tại trong hệ thống", ErrorType.Critical);
                    PartnerCodeTextEdit?.Focus();
                    return false;
                }
            }
            else // Cập nhật
            {
                var existingPartner = _bll.GetById(_businessPartnerId);
                if (existingPartner != null && existingPartner.PartnerCode != partnerCode)
                {
                    if (_bll.IsCodeExists(partnerCode))
                    {
                        dxErrorProvider1.SetError(PartnerCodeTextEdit, "Mã đối tác đã tồn tại trong hệ thống", ErrorType.Critical);
                        PartnerCodeTextEdit?.Focus();
                        return false;
                    }
                }
            }

            // PartnerName bắt buộc
            if (string.IsNullOrWhiteSpace(PartnerNameTextEdit?.Text))
            {
                dxErrorProvider1.SetError(PartnerNameTextEdit, "Tên đối tác không được để trống", ErrorType.Critical);
                PartnerNameTextEdit?.Focus();
                return false;
            }

            // Loại đối tác (khuyến nghị bắt buộc chọn)
            if (PartnerTypeNameComboBoxEdit != null && PartnerTypeNameComboBoxEdit.SelectedIndex < 0)
            {
                dxErrorProvider1.SetError(PartnerTypeNameComboBoxEdit, "Vui lòng chọn loại đối tác", ErrorType.Warning);
                PartnerTypeNameComboBoxEdit.Focus();
                return false;
            }

            return true;
        }

        /// <summary>
        /// Thu thập dữ liệu từ Form thành BusinessPartnerDetailDto (đủ trường để lưu).
        /// </summary>
        private BusinessPartnerDetailDto BuildDetailDtoFromForm()
        {
            var dto = new BusinessPartnerDetailDto
            {
                Id = _businessPartnerId,
                PartnerCode = PartnerCodeTextEdit?.EditValue?.ToString(),
                PartnerName = PartnerNameTextEdit?.EditValue?.ToString(),
                PartnerType = PartnerTypeNameComboBoxEdit == null ? 0 :
                    (PartnerTypeNameComboBoxEdit.SelectedIndex == 0 ? 1 :
                    (PartnerTypeNameComboBoxEdit.SelectedIndex == 1 ? 2 :
                    (PartnerTypeNameComboBoxEdit.SelectedIndex == 2 ? 3 : 0))),
                TaxCode = TaxCodeTextEdit?.EditValue?.ToString(),
                Phone = PhoneTextEdit?.EditValue?.ToString(),
                Email = EmailTextEdit?.EditValue?.ToString(),
                Website = WebsiteTextEdit?.EditValue?.ToString(),
                Address = AddressTextEdit?.EditValue?.ToString(),
                City = CityTextEdit?.EditValue?.ToString(),
                Country = CountryTextEdit?.EditValue?.ToString(),
                ContactPerson = ContactPersonTextEdit?.EditValue?.ToString(),
                ContactPosition = ContactPositionTextEdit?.EditValue?.ToString(),
                BankAccount = BankAccountTextEdit?.EditValue?.ToString(),
                BankName = BankNameTextEdit?.EditValue?.ToString(),
                CreditLimit = decimal.TryParse(CreditLimitTextEdit?.EditValue?.ToString(), out var cl) ? cl : null,
                PaymentTerm = PaymentTermTextEdit?.EditValue?.ToString(),
                IsActive = (IsActiveToggleSwitch?.EditValue as bool?) ?? true,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            };
            return dto;
        }
    }
}
