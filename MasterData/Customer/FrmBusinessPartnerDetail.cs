using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bll.MasterData.Customer;
using Bll.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraLayout;
using MasterData.Customer.Converters;
using MasterData.Customer.Dto;

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
                MarkRequiredFields(typeof(BusinessPartnerListDto));

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
        /// Đánh dấu các layout item tương ứng với thuộc tính có [Required] bằng dấu * đỏ.
        /// Quy ước mapping control theo tên thuộc tính (từ editor được gán vào LayoutControlItem.Control):
        /// - Editor: "txt" + PropertyName, PropertyName + "TextEdit", hoặc PropertyName (BaseEdit)
        /// </summary>
        /// <param name="dtoType">Kiểu DTO để đọc DataAnnotations</param>
        private void MarkRequiredFields(Type dtoType)
        {
            var requiredProps = dtoType
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.GetCustomAttributes(typeof(RequiredAttribute), true).Any())
                .ToList();

            // Lấy tất cả LayoutControlItems trên form
            var allLayoutItems = GetAllLayoutControlItems(this);

            // Bật HtmlString cho tất cả caption của LayoutControlItem
            foreach (var it in allLayoutItems)
            {
                it.AllowHtmlStringInCaption = true;
            }

            foreach (var prop in requiredProps)
            {
                var propName = prop.Name;

                // Tìm item có editor tên khớp property theo quy ước
                var item = allLayoutItems.FirstOrDefault(it => IsEditorMatchProperty(it.Control, propName));
                if (item == null) continue;

                // Đặt caption có dấu * đỏ bằng HTML (giữ nguyên caption cũ)
                if (!(item.Text?.Contains("*") ?? false))
                {
                    var baseCaption = string.IsNullOrWhiteSpace(item.Text) ? propName : item.Text;
                    item.Text = baseCaption + " <color=red>*</color>";
                }

                // Optional: gợi ý cho editor (nếu là TextEdit)
                if (item.Control is BaseEdit be && be.Properties is RepositoryItemTextEdit txtProps)
                {
                    txtProps.NullValuePrompt = "Bắt buộc nhập";
                    txtProps.NullValuePromptShowForEmptyValue = true;
                }
            }
        }

        private static bool IsEditorMatchProperty(Control editor, string propName)
        {
            if (editor == null) return false;
            var name = editor.Name ?? string.Empty;
            // so khớp không phân biệt hoa thường, loại bỏ prefix/suffix phổ biến
            string[] candidates = {
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

            // Tìm tất cả LayoutControl trong cây control
            var layoutControls = root.Controls.OfType<LayoutControl>().ToList();
            var nested = root.Controls.Cast<Control>().SelectMany(c => GetAllLayoutControlItems(c)).ToList();
            // Thu thập items từ từng LayoutControl
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

        private static void CollectLayoutItems(BaseLayoutItem baseItem, List<LayoutControlItem> collector)
        {
            if (baseItem == null) return;
            if (baseItem is LayoutControlItem lci)
            {
                collector.Add(lci);
            }

            if (baseItem is LayoutControlGroup group)
            {
                foreach (BaseLayoutItem child in group.Items)
                {
                    CollectLayoutItems(child, collector);
                }
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

                MsgBox.ShowInfo("Lưu dữ liệu đối tác thành công");
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
                CreditLimit = decimal.TryParse(CreditLimitTextEdit?.EditValue?.ToString(), out var cl) ? cl : (decimal?)null,
                PaymentTerm = PaymentTermTextEdit?.EditValue?.ToString(),
                IsActive = (IsActiveToggleSwitch?.EditValue as bool?) ?? true,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            };
            return dto;
        }
    }
}
