using Bll.Common;
using Bll.MasterData.Customer;
using Bll.Utils;
using Dal.DataContext;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraLayout;
using DevExpress.XtraSplashScreen;
using MasterData.Customer.Converters;
using MasterData.Customer.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

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
                MarkRequiredFields(typeof(BusinessPartnerContactDto));
                
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
            var sites = await Task.Run(() => siteBll.GetAll());
            
            // Lọc chỉ các chi nhánh đang hoạt động và có thông tin đầy đủ
            var list = sites.ToSiteListDtos()
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
        private void SaveBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!ValidateInput()) return;
            
            try
            {
                var entity = GetDataFromControls();
                
                // Kiểm tra SiteId trước khi lưu
                if (_selectedSiteId == null || _selectedSiteId == Guid.Empty)
                {
                    ShowError("Vui lòng chọn chi nhánh đối tác");
                    return;
                }
                
                // Chỉ thêm mới, không update
                _bll.Add(entity);
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

        #endregion

        #region ========== XỬ LÝ DỮ LIỆU ==========

        /// <summary>
        /// Thu thập dữ liệu từ các control để tạo entity lưu xuống DB.
        /// </summary>
        private BusinessPartnerContact GetDataFromControls()
        {
            // Lấy IsActive từ control nếu có, nếu không thì dùng default
            bool isActive = true; // Default value
            if (FindControlByName(this, "IsActiveCheckEdit") is CheckEdit isActiveCheck)
            {
                isActive = isActiveCheck.Checked;
            }
            
            var entity = new BusinessPartnerContact
            {
                Id = Guid.NewGuid(),
                SiteId = _selectedSiteId ?? Guid.Empty,
                FullName = FullNameTextEdit?.EditValue?.ToString(),
                Position = PositionTextEdit?.EditValue?.ToString(),
                Phone = PhoneTextEdit?.EditValue?.ToString(),
                Email = EmailTextEdit?.EditValue?.ToString(),
                IsPrimary = IsPrimaryCheckEdit?.Checked ?? false,
                IsActive = isActive
            };
            return entity;
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
            MsgBox.ShowInfo(message);
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
                    .Where(p => p.GetCustomAttributes(typeof(RequiredAttribute), true).Any())
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

                    if (item.Control is BaseEdit be && be.Properties is RepositoryItemTextEdit txtProps)
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
            var layoutControls = root.Controls.OfType<LayoutControl>().ToList();
            var nested = root.Controls.Cast<Control>().SelectMany(c => GetAllLayoutControlItems(c)).ToList();
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