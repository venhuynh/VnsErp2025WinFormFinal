using Bll.Common;
using Bll.MasterData.Company;
using Bll.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraLayout;
using MasterData.Company.Converters;
using MasterData.Company.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MasterData.Company
{
    public partial class FrmPositionDetail : XtraForm
    {
        #region ========== KHAI BÁO BIẾN ==========

        /// <summary>
        /// ID chức vụ (Guid.Empty cho thêm mới)
        /// </summary>
        private readonly Guid _positionId;

        /// <summary>
        /// DTO chức vụ
        /// </summary>
        private PositionDto _positionDto;

        /// <summary>
        /// BLL xử lý chức vụ
        /// </summary>
        private readonly PositionBll _positionBll;

        /// <summary>
        /// Trạng thái form (true: chỉnh sửa, false: thêm mới)
        /// </summary>
        private bool _isEditMode => _positionId != Guid.Empty;

        #endregion

        #region ========== CONSTRUCTOR ==========

        /// <summary>
        /// Khởi tạo form chi tiết chức vụ.
        /// </summary>
        /// <param name="positionId">ID chức vụ (Guid.Empty cho thêm mới)</param>
        public FrmPositionDetail(Guid positionId)
        {
            InitializeComponent();
            _positionId = positionId;
            _positionBll = new PositionBll();
            
            InitializeForm();
        }

        /// <summary>
        /// Khởi tạo form chi tiết chức vụ (thêm mới).
        /// </summary>
        public FrmPositionDetail() : this(Guid.Empty)
        {
        }

        #endregion

        #region ========== INITIALIZATION ==========

        /// <summary>
        /// Khởi tạo form
        /// </summary>
        private void InitializeForm()
        {
            try
            {
                // Thiết lập tiêu đề form
                Text = _isEditMode ? "Chỉnh sửa chức vụ" : "Thêm mới chức vụ";

                // Khởi tạo DTO
                InitializeDto();

                // Thiết lập data binding
                SetupDataBinding();

                // Đánh dấu các trường bắt buộc
                MarkRequiredFields();
                
                // Load dữ liệu nếu là chế độ chỉnh sửa
                if (_isEditMode)
                {
                    LoadData();
                }
                else
                {
                    // Set default values for new position
                    SetDefaultValues();
                }

                // Thiết lập sự kiện
                SetupEvents();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khởi tạo form");
            }
        }

        /// <summary>
        /// Khởi tạo DTO
        /// </summary>
        private void InitializeDto()
        {
            _positionDto = new PositionDto
            {
                Id = _positionId,
                IsActive = true // Default value
            };
            
            if (!_isEditMode)
            {
                // Lấy CompanyId từ database (vì chỉ có 1 Company duy nhất)
                var companyId = GetCompanyIdFromDatabase();
                if (companyId == Guid.Empty)
                {
                    ShowError("Không tìm thấy thông tin công ty trong hệ thống.");
                    Close();
                    return;
                }
                
                _positionDto.CompanyId = companyId;
            }
        }

        /// <summary>
        /// Thiết lập data binding
        /// </summary>
        private void SetupDataBinding()
        {
            try
            {
                // Bind các control với DTO
                PositionCodeTextEdit.DataBindings.Add("Text", _positionDto, "PositionCode", false, DataSourceUpdateMode.OnPropertyChanged);
                PositionNameTextEdit.DataBindings.Add("Text", _positionDto, "PositionName", false, DataSourceUpdateMode.OnPropertyChanged);
                DescriptionTextEdit.DataBindings.Add("Text", _positionDto, "Description", false, DataSourceUpdateMode.OnPropertyChanged);
                IsActiveToogleSwitch.DataBindings.Add("EditValue", _positionDto, "IsActive", false, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi thiết lập data binding");
            }
        }


        /// <summary>
        /// Thiết lập sự kiện
        /// </summary>
        private void SetupEvents()
        {
            try
            {
                Load += FrmPositionDetail_Load;
                FormClosing += FrmPositionDetail_FormClosing;
                SaveBarButtonItem.ItemClick += SaveBarButtonItem_ItemClick;
                CloseBarButtonItem.ItemClick += CloseBarButtonItem_ItemClick;
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi thiết lập sự kiện");
            }
        }

        /// <summary>
        /// Đánh dấu các trường bắt buộc
        /// </summary>
        private void MarkRequiredFields()
        {
            try
            {
                // Đánh dấu các trường bắt buộc theo DataAnnotations của DTO
                MarkRequiredFields(typeof(PositionDto));
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi đánh dấu trường bắt buộc");
            }
        }

        /// <summary>
        /// Đánh dấu các layout item tương ứng với thuộc tính có [Required] bằng dấu * đỏ
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
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi đánh dấu trường bắt buộc");
            }
        }

        /// <summary>
        /// Kiểm tra editor có match với property không
        /// </summary>
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

        /// <summary>
        /// Lấy tất cả LayoutControlItem trong form
        /// </summary>
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

        /// <summary>
        /// Thu thập LayoutControlItem từ BaseLayoutItem
        /// </summary>
        private static void CollectLayoutItems(BaseLayoutItem baseItem, List<LayoutControlItem> collector)
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

        /// <summary>
        /// Thiết lập giá trị mặc định cho chức vụ mới
        /// </summary>
        private void SetDefaultValues()
        {
            try
            {
                _positionDto.IsActive = true;
                _positionDto.IsManagerLevel = false;
                
                // Enable mã chức vụ khi tạo mới (cho phép nhập)
                PositionCodeTextEdit.Properties.ReadOnly = false;
                PositionCodeTextEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
                PositionCodeTextEdit.Enabled = true; // Enable hoàn toàn
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi thiết lập giá trị mặc định");
            }
        }

        #endregion

        #region ========== DATA LOADING ==========

        /// <summary>
        /// Load dữ liệu chức vụ
        /// </summary>
        private void LoadData()
        {
            try
            {
                var position = _positionBll.GetById(_positionId);
                if (position == null)
                {
                    ShowError("Không tìm thấy thông tin chức vụ");
                    DialogResult = DialogResult.Cancel;
                    return;
                }

                _positionDto = position.ToDto();
                
                // Disable mã chức vụ khi edit (không cho phép thay đổi)
                PositionCodeTextEdit.Properties.ReadOnly = true;
                PositionCodeTextEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
                PositionCodeTextEdit.Enabled = false; // Disable hoàn toàn
                
                // Refresh data binding
                RefreshDataBinding();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi tải dữ liệu");
            }
        }

        /// <summary>
        /// Refresh data binding
        /// </summary>
        private void RefreshDataBinding()
        {
            try
            {
                // Clear existing bindings
                PositionCodeTextEdit.DataBindings.Clear();
                PositionNameTextEdit.DataBindings.Clear();
                DescriptionTextEdit.DataBindings.Clear();
                IsActiveToogleSwitch.DataBindings.Clear();

                // Re-bind
                SetupDataBinding();
                
                // Đánh dấu lại các trường bắt buộc
                MarkRequiredFields();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi refresh data binding");
            }
        }

        #endregion

        #region ========== EVENT HANDLERS ==========

        /// <summary>
        /// Xử lý sự kiện Load form
        /// </summary>
        private void FrmPositionDetail_Load(object sender, EventArgs e)
        {
            try
            {
                // Focus vào control đầu tiên
                PositionCodeTextEdit.Focus();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi load form");
            }
        }

        /// <summary>
        /// Xử lý sự kiện FormClosing
        /// </summary>
        private void FrmPositionDetail_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                // Kiểm tra nếu có thay đổi dữ liệu
                if (HasDataChanged() && DialogResult != DialogResult.OK)
                {
                    var result = MsgBox.GetConfirmFromYesNoDialog("Dữ liệu đã thay đổi. Bạn có muốn lưu không?");
                    if (result)
                    {
                        if (!SaveData())
                        {
                            e.Cancel = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi đóng form");
            }
        }

        /// <summary>
        /// Xử lý sự kiện click button Save
        /// </summary>
        private void SaveBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (SaveData())
                {
                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi lưu dữ liệu");
            }
        }

        /// <summary>
        /// Xử lý sự kiện click button Close
        /// </summary>
        private void CloseBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi hủy");
            }
        }

        #endregion

        #region ========== DATA VALIDATION & SAVING ==========

        /// <summary>
        /// Kiểm tra dữ liệu có thay đổi không
        /// </summary>
        /// <returns>True nếu có thay đổi, False nếu không</returns>
        private bool HasDataChanged()
        {
            try
            {
                // TODO: Implement logic to check if data has changed
                return true; // Tạm thời return true
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi kiểm tra thay đổi dữ liệu");
                return false;
            }
        }

        /// <summary>
        /// Validate dữ liệu trước khi lưu
        /// </summary>
        /// <returns>True nếu hợp lệ, False nếu không</returns>
        private bool ValidateData()
        {
            try
            {
                // Clear hết lỗi
                dxErrorProvider1.ClearErrors();
                
                // Validate business rules
                // Validate mã chức vụ - chỉ kiểm tra khi tạo mới
                if (!_isEditMode && string.IsNullOrWhiteSpace(_positionDto.PositionCode))
                {
                    dxErrorProvider1.SetError(PositionCodeTextEdit, "Mã chức vụ không được để trống");
                    PositionCodeTextEdit.Focus();
                    return false;
                }

                if (string.IsNullOrWhiteSpace(_positionDto.PositionName))
                {
                    dxErrorProvider1.SetError(PositionNameTextEdit, "Tên chức vụ không được để trống");
                    PositionNameTextEdit.Focus();
                    return false;
                }

                // Validate trùng lặp mã chức vụ - chỉ kiểm tra khi tạo mới
                if (!_isEditMode && _positionBll.IsPositionCodeExists(_positionDto.PositionCode, _positionDto.Id))
                {
                    dxErrorProvider1.SetError(PositionCodeTextEdit, $"Mã chức vụ '{_positionDto.PositionCode}' đã tồn tại trong hệ thống");
                    PositionCodeTextEdit.Focus();
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi validate dữ liệu");
                return false;
            }
        }

        /// <summary>
        /// Lưu dữ liệu
        /// </summary>
        /// <returns>True nếu lưu thành công, False nếu không</returns>
        private bool SaveData()
        {
            try
            {
                // Validate dữ liệu
                if (!ValidateData())
                {
                    return false;
                }

                // Đảm bảo CompanyId được set đúng
                if (_positionDto.CompanyId == Guid.Empty)
                {
                    var companyId = GetCompanyIdFromDatabase();
                    if (companyId == Guid.Empty)
                    {
                        ShowError("Không tìm thấy thông tin công ty trong hệ thống.");
                        return false;
                    }
                    _positionDto.CompanyId = companyId;
                }

                // Convert DTO to Entity
                var position = _positionDto.ToEntity();

                // Lưu dữ liệu
                if (_isEditMode)
                {
                    _positionBll.Update(position);
                }
                else
                {
                    _positionBll.Insert(position);
                }

                ShowInfo(_isEditMode ? "Cập nhật chức vụ thành công!" : "Thêm mới chức vụ thành công!");
                return true;
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi lưu dữ liệu");
                return false;
            }
        }

        #endregion

        #region ========== UTILITY METHODS ==========

        /// <summary>
        /// Hiển thị thông tin
        /// </summary>
        private void ShowInfo(string message)
        {
            MsgBox.ShowInfo(message);
        }

        /// <summary>
        /// Hiển thị lỗi
        /// </summary>
        private void ShowError(string message)
        {
            MsgBox.ShowError(message);
        }

        /// <summary>
        /// Hiển thị lỗi với exception
        /// </summary>
        private void ShowError(Exception ex, string context = null)
        {
            MsgBox.ShowException(string.IsNullOrWhiteSpace(context)
                ? ex
                : new Exception(context + ": " + ex.Message, ex));
        }

        /// <summary>
        /// Lấy CompanyId từ database (vì chỉ có 1 Company duy nhất)
        /// </summary>
        /// <returns>CompanyId hoặc Guid.Empty nếu không tìm thấy</returns>
        private Guid GetCompanyIdFromDatabase()
        {
            try
            {
                // Sử dụng CompanyBll để lấy Company duy nhất
                var companyBll = new CompanyBll();
                var company = companyBll.GetCompany();
                
                if (company != null)
                {
                    // Cast về Company entity và lấy Id
                    if (company is Dal.DataContext.Company companyEntity)
                    {
                        return companyEntity.Id;
                    }
                }
                
                return Guid.Empty;
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi lấy CompanyId từ database");
                return Guid.Empty;
            }
        }

        #endregion
    }
}