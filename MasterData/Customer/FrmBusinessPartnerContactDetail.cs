using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Bll.MasterData.Customer;
using Bll.Utils;
using Dal.DataContext;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout;
using MasterData.Customer.Dto;

namespace MasterData.Customer
{
    public partial class FrmBusinessPartnerContactDetail : XtraForm
    {
        #region Fields

        private readonly Guid _contactId;
        private readonly BusinessPartnerContactBll _bll = new BusinessPartnerContactBll();
        private bool _isEditMode => _contactId != Guid.Empty;

        #endregion

        public FrmBusinessPartnerContactDetail()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Khởi tạo form theo chế độ thêm mới/chỉnh sửa dựa vào contactId.
        /// </summary>
        /// <param name="contactId">ID liên hệ; Guid.Empty nếu thêm mới</param>
        public FrmBusinessPartnerContactDetail(Guid contactId)
        {
            InitializeComponent();
            _contactId = contactId;
            InitializeForm();

            SaveBarButtonItem.ItemClick += SaveBarButtonItem_ItemClick;
            CloseBarButtonItem.ItemClick += CloseBarButtonItem_ItemClick;
        }

        /// <summary>
        /// Khởi tạo form, thiết lập tiêu đề, nạp lookup, và tải dữ liệu nếu đang chỉnh sửa.
        /// </summary>
        private void InitializeForm()
        {
            try
            {
                Text = _isEditMode ? "Điều chỉnh liên hệ đối tác" : "Thêm mới liên hệ đối tác";
                LoadPartnerLookup();
                // Đánh dấu các trường bắt buộc theo DataAnnotations của DTO
                MarkRequiredFields(typeof(BusinessPartnerContactDto));
                if (_isEditMode)
                {
                    LoadContactData();
                }
                var first = FindControlByName(this, "FullNameTextEdit") as BaseEdit;
                first?.Focus();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khởi tạo form");
            }
        }

        /// <summary>
        /// Nạp dữ liệu cho PartnerNameSearchLookUpEdit (businessPartnerListDtoBindingSource)
        /// </summary>
        private void LoadPartnerLookup()
        {
            try
            {
                var partnerBll = new BusinessPartnerBll();
                var partners = partnerBll.GetAll();
                var list = partners
                    .Select(p => new BusinessPartnerListDto { Id = p.Id, PartnerName = p.PartnerName })
                    .ToList();

                if (businessPartnerListDtoBindingSource != null)
                {
                    businessPartnerListDtoBindingSource.DataSource = list;
                }

                if (FindControlByName(this, "PartnerNameSearchLookUpEdit") is SearchLookUpEdit lookup)
                {
                    lookup.Properties.DataSource = businessPartnerListDtoBindingSource;
                    lookup.Properties.DisplayMember = "PartnerName";
                    lookup.Properties.ValueMember = "Id";
                    lookup.Properties.NullText = "Chọn đối tác";

                    // Cấu hình searchLookUpEdit1View (GridView)
                    if (lookup.Properties.View is GridView view)
                    {
                        view.OptionsView.ShowIndicator = true;
                        view.OptionsView.ShowGroupPanel = false;
                        view.FocusRectStyle = DrawFocusRectStyle.RowFocus;
                        view.OptionsSelection.EnableAppearanceFocusedCell = false;

                        // Tạo cột nếu designer chưa thêm
                        if (view.Columns["PartnerName"] == null)
                        {
                            var colName = view.Columns.AddVisible("PartnerName", "Tên đối tác");
                            colName.VisibleIndex = 0;
                        }
                        if (view.Columns["Id"] == null)
                        {
                            var colId = view.Columns.AddVisible("Id", "ID");
                            colId.Visible = false;
                        }

                        view.BestFitColumns();
                    }
                }
            }
            catch (Exception ex)
            {
                // Không chặn form nếu lookup lỗi, chỉ ghi log UI
                ShowError(ex, "Lỗi nạp danh sách đối tác");
            }
        }

        /// <summary>
        /// Tải dữ liệu chi tiết liên hệ khi ở chế độ chỉnh sửa.
        /// </summary>
        private void LoadContactData()
        {
            try
            {
                var entity = _bll.GetById(_contactId);
                if (entity == null)
                {
                    ShowError("Không tìm thấy liên hệ đối tác");
                    DialogResult = DialogResult.Cancel;
                    return;
                }

                BindDataToControls(entity);
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi tải dữ liệu liên hệ");
            }
        }

        /// <summary>
        /// Bind entity vào các control trên form.
        /// </summary>
        private void BindDataToControls(BusinessPartnerContact entity)
        {
            SetTextIfExist("FullNameTextEdit", entity.FullName);
            SetTextIfExist("PositionTextEdit", entity.Position);
            SetTextIfExist("PhoneTextEdit", entity.Phone);
            SetTextIfExist("EmailTextEdit", entity.Email);

            if (FindControlByName(this, "IsPrimaryCheckEdit") is CheckEdit chk)
            {
                chk.Checked = entity.IsPrimary;
            }

            if (FindControlByName(this, "PartnerNameSearchLookUpEdit") is SearchLookUpEdit partnerLookup)
            {
                partnerLookup.EditValue = entity.SiteId;
            }
        }

        private static Control FindControlByName(Control root, string name)
        {
            if (root == null || string.IsNullOrWhiteSpace(name)) return null;
            var found = root.Controls.Find(name, true).FirstOrDefault();
            return found;
        }

        private void SetTextIfExist(string controlName, string value)
        {
            if (FindControlByName(this, controlName) is BaseEdit edit)
            {
                edit.EditValue = value;
            }
        }

        #region Toolbar Events

        /// <summary>
        /// Người dùng bấm Lưu.
        /// </summary>
        private void SaveBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!ValidateInput()) return;
            try
            {
                var entity = GetDataFromControls();
                _bll.SaveOrUpdate(entity);
                ShowInfo(_isEditMode ? "Cập nhật liên hệ đối tác thành công!" : "Thêm mới liên hệ đối tác thành công!");
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi lưu dữ liệu liên hệ");
            }
        }

        /// <summary>
        /// Người dùng bấm Đóng.
        /// </summary>
        private void CloseBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        #endregion

        /// <summary>
        /// Thu thập dữ liệu từ các control để tạo entity lưu xuống DB.
        /// </summary>
        private BusinessPartnerContact GetDataFromControls()
        {
            var entity = new BusinessPartnerContact
            {
                Id = _contactId,
                SiteId = GetGuidFromEditor("PartnerNameSearchLookUpEdit") ?? Guid.Empty,
                FullName = GetStringFromEditor("FullNameTextEdit"),
                Position = GetStringFromEditor("PositionTextEdit"),
                Phone = GetStringFromEditor("PhoneTextEdit"),
                Email = GetStringFromEditor("EmailTextEdit"),
                IsPrimary = GetBoolFromEditor("IsPrimaryCheckEdit") ?? false
            };
            return entity;
        }

        /// <summary>
        /// Kiểm tra hợp lệ dữ liệu bắt buộc.
        /// </summary>
        private bool ValidateInput()
        {
            if (FindControlByName(this, "FullNameTextEdit") is BaseEdit fullName && string.IsNullOrWhiteSpace(fullName.EditValue?.ToString()))
            {
                ShowError("Họ và tên không được để trống");
                fullName.Focus();
                return false;
            }

            // Kiểm tra trùng số điện thoại (cho phép rỗng)
            var phone = GetStringFromEditor("PhoneTextEdit")?.Trim();
            if (!string.IsNullOrWhiteSpace(phone))
            {
                if (_bll.IsPhoneExists(phone, _contactId))
                {
                    var phoneEdit = FindControlByName(this, "PhoneTextEdit") as BaseEdit;
                    phoneEdit?.Focus();
                    ShowError("Số điện thoại đã tồn tại trong hệ thống");
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Lấy Guid từ editor theo tên control.
        /// </summary>
        private Guid? GetGuidFromEditor(string name)
        {
            if (FindControlByName(this, name) is BaseEdit edit)
            {
                Guid parsed;
                if (Guid.TryParse(edit.EditValue?.ToString(), out parsed))
                    return parsed;
            }
            return null;
        }

        /// <summary>
        /// Lấy string từ editor theo tên control.
        /// </summary>
        private string GetStringFromEditor(string name)
        {
            if (FindControlByName(this, name) is BaseEdit edit)
            {
                return edit.EditValue?.ToString();
            }
            return null;
        }

        /// <summary>
        /// Lấy bool từ editor theo tên control.
        /// </summary>
        private bool? GetBoolFromEditor(string name)
        {
            if (FindControlByName(this, name) is CheckEdit chk)
            {
                return chk.Checked;
            }
            if (FindControlByName(this, name) is BaseEdit edit)
            {
                if (edit.EditValue is bool b) return b;
            }
            return null;
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
                        item.Text = baseCaption + " <color=red>*</color>";
                    }

                    if (item.Control is BaseEdit be && be.Properties is RepositoryItemTextEdit txtProps)
                    {
                        txtProps.NullValuePrompt = "Bắt buộc nhập";
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
    }
}