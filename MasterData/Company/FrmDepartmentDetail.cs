using Bll.MasterData.Company;
using Dal.DataContext;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraLayout;
using MasterData.Company.Converters;
using MasterData.Company.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MasterData.Company
{
    public partial class FrmDepartmentDetail : XtraForm
    {
        #region ========== FIELDS ==========

        private readonly Guid _departmentId;
        private readonly DepartmentBll _departmentBll;
        private readonly CompanyBranchBll _companyBranchBll;
        private DepartmentDto _currentDepartment;
        private readonly bool _isEditMode;
        
        // Biến lưu trữ ID của chi nhánh và phòng ban cha
        private Guid? _branchId;
        private Guid? _parentId;

        #endregion

        #region ========== CONSTRUCTOR ==========

        public FrmDepartmentDetail()
        {
            InitializeComponent();
            _departmentId = Guid.Empty;
            _departmentBll = new DepartmentBll();
            _companyBranchBll = new CompanyBranchBll();
            _isEditMode = false;
            InitializeForm();
        }

        public FrmDepartmentDetail(Guid departmentId)
        {
            InitializeComponent();
            _departmentId = departmentId;
            _departmentBll = new DepartmentBll();
            _companyBranchBll = new CompanyBranchBll();
            _isEditMode = departmentId != Guid.Empty;
            InitializeForm();
        }

        #endregion

        #region ========== FORM INITIALIZATION ==========

        /// <summary>
        /// Khởi tạo form
        /// </summary>
        private void InitializeForm()
        {
            try
            {
                // Load datasources
                LoadDataSources();
                
                // Setup form based on mode
                if (_isEditMode)
                {
                    LoadDepartmentData();
                    Text = @"Chỉnh sửa phòng ban";
                }
                else
                {
                    Text = @"Thêm mới phòng ban";
                    SetDefaultValues();
                }

                // Setup event handlers
                SetupEventHandlers();

                // Setup advanced validation with DataAnnotations
                SetupAdvancedValidation();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Lỗi khởi tạo form: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Load các datasource cho form
        /// </summary>
        private void LoadDataSources()
        {
            try
            {
                // Load CompanyBranch data cho BranchNameSearchLookupedit
                LoadCompanyBranches();
                
                // Load Department data cho ParentDepartmentNameTextEdit
                LoadDepartments();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Lỗi load datasource: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Load danh sách chi nhánh công ty
        /// </summary>
        private void LoadCompanyBranches()
        {
            try
            {
                var companyBranches = _companyBranchBll.GetActiveBranches();
                var companyBranchDtos = companyBranches.Select(cb => cb.ToDto()).ToList();

                companyBranchDtoBindingSource.DataSource = companyBranchDtos;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi load danh sách chi nhánh: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Load danh sách phòng ban cho ParentDepartmentNameTextEdit
        /// </summary>
        private void LoadDepartments()
        {
            try
            {
                var departments = _departmentBll.GetAll();
                var departmentDtos = departments.Select(d => d.ToDto()).ToList();

                departmentDtoBindingSource.DataSource = departmentDtos;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi load danh sách phòng ban: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Load dữ liệu phòng ban khi edit
        /// </summary>
        private void LoadDepartmentData()
        {
            try
            {
                // Chỉ load data khi thực sự là edit mode và có departmentId hợp lệ
                if (!_isEditMode || _departmentId == Guid.Empty)
                {
                    return;
                }

                var department = _departmentBll.GetById(_departmentId);
                if (department == null)
                {
                    XtraMessageBox.Show("Không tìm thấy phòng ban", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Close();
                    return;
                }

                _currentDepartment = department.ToDto();
                BindDepartmentToControls();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Lỗi load dữ liệu phòng ban: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Bind dữ liệu phòng ban vào controls
        /// </summary>
        private void BindDepartmentToControls()
        {
            try
            {
                if (_currentDepartment == null) return;

                // Bind data to controls
                DepartmentCodeTextEdit.EditValue = _currentDepartment.DepartmentCode;
                DepartmentNameTextEdit.EditValue = _currentDepartment.DepartmentName;
                DescriptionTextEdit.EditValue = _currentDepartment.Description;
                IsActiveToogleSwitch.EditValue = _currentDepartment.IsActive;

                // Disable mã phòng ban khi edit (không cho phép thay đổi)
                DepartmentCodeTextEdit.Properties.ReadOnly = true;
                DepartmentCodeTextEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
                DepartmentCodeTextEdit.Enabled = false; // Disable hoàn toàn

                // Set branch selection và cập nhật biến
                if (_currentDepartment.BranchId.HasValue)
                {
                    BranchNameSearchLookupedit.EditValue = _currentDepartment.BranchId.Value;
                    _branchId = _currentDepartment.BranchId.Value;
                }
                else
                {
                    BranchNameSearchLookupedit.EditValue = null;
                    _branchId = null;
                }

                // Set parent department selection và cập nhật biến
                if (_currentDepartment.ParentId.HasValue)
                {
                    ParentDepartmentNameTextEdit.EditValue = _currentDepartment.ParentId.Value;
                    _parentId = _currentDepartment.ParentId.Value;
                }
                else
                {
                    ParentDepartmentNameTextEdit.EditValue = null;
                    _parentId = null; // ParentId có thể là NULL
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi bind dữ liệu: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Set giá trị mặc định cho form mới
        /// </summary>
        private void SetDefaultValues()
        {
            try
            {
                DepartmentCodeTextEdit.EditValue = string.Empty;
                DepartmentNameTextEdit.EditValue = string.Empty;
                DescriptionTextEdit.EditValue = string.Empty;
                IsActiveToogleSwitch.EditValue = true;
                BranchNameSearchLookupedit.EditValue = null;
                ParentDepartmentNameTextEdit.EditValue = null;
                
                // Enable mã phòng ban khi tạo mới (cho phép nhập)
                DepartmentCodeTextEdit.Properties.ReadOnly = false;
                DepartmentCodeTextEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
                DepartmentCodeTextEdit.Enabled = true; // Enable hoàn toàn
                
                // Khởi tạo các biến ID
                _branchId = null;
                _parentId = null; // ParentId có thể là NULL
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi set giá trị mặc định: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Setup event handlers
        /// </summary>
        private void SetupEventHandlers()
        {
            try
            {
                SaveBarButtonItem.ItemClick += SaveBarButtonItem_ItemClick;
                CloseBarButtonItem.ItemClick += CloseBarButtonItem_ItemClick;
                
                // Event handlers cho lookup controls
                BranchNameSearchLookupedit.EditValueChanged += BranchNameSearchLookupedit_EditValueChanged;
                ParentDepartmentNameTextEdit.EditValueChanged += ParentDepartmentNameTextEdit_EditValueChanged;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi setup event handlers: {ex.Message}", ex);
            }
        }

        #endregion

        #region ========== EVENT HANDLERS ==========

        /// <summary>
        /// Xử lý sự kiện click Save button
        /// </summary>
        private async void SaveBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (ValidateForm())
                {
                    await SaveDepartment();
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Lỗi lưu phòng ban: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Xử lý sự kiện click Close button
        /// </summary>
        private void CloseBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                Close();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Lỗi đóng form: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Xử lý sự kiện thay đổi giá trị BranchNameSearchLookupedit
        /// </summary>
        private void BranchNameSearchLookupedit_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (BranchNameSearchLookupedit.EditValue != null && 
                    Guid.TryParse(BranchNameSearchLookupedit.EditValue.ToString(), out var branchId))
                {
                    _branchId = branchId;
                    Debug.WriteLine($"BranchId updated to: {branchId}");
                }
                else
                {
                    _branchId = null;
                    Debug.WriteLine("BranchId set to null");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Lỗi cập nhật BranchId: {ex.Message}");
            }
        }

        /// <summary>
        /// Xử lý sự kiện thay đổi giá trị ParentDepartmentNameTextEdit
        /// </summary>
        private void ParentDepartmentNameTextEdit_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (ParentDepartmentNameTextEdit.EditValue != null && 
                    Guid.TryParse(ParentDepartmentNameTextEdit.EditValue.ToString(), out var parentId))
                {
                    _parentId = parentId;
                }
                else
                {
                    _parentId = null; // ParentId có thể là NULL
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Lỗi cập nhật ParentId: {ex.Message}");
            }
        }

        #endregion

        #region ========== VALIDATION & SAVE ==========

        /// <summary>
        /// Validate form data
        /// </summary>
        /// <returns>True nếu hợp lệ</returns>
        private bool ValidateForm()
        {
            try
            {
                dxErrorProvider1.ClearErrors();

                bool isValid = true;

                // Validate Department Code - chỉ kiểm tra khi tạo mới
                if (!_isEditMode && string.IsNullOrWhiteSpace(DepartmentCodeTextEdit.Text))
                {
                    dxErrorProvider1.SetError(DepartmentCodeTextEdit, "Mã phòng ban không được để trống");
                    isValid = false;
                }

                // Validate Department Name
                if (string.IsNullOrWhiteSpace(DepartmentNameTextEdit.Text))
                {
                    dxErrorProvider1.SetError(DepartmentNameTextEdit, "Tên phòng ban không được để trống");
                    isValid = false;
                }

                // Validate Branch selection - kiểm tra _branchId không được bỏ trống
                if (_branchId == null)
                {
                    dxErrorProvider1.SetError(BranchNameSearchLookupedit, "Vui lòng chọn chi nhánh");
                    isValid = false;
                }

                return isValid;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi validate form: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Save department data
        /// </summary>
        private async Task SaveDepartment()
        {
            try
            {
                var department = GetDepartmentFromControls();

                if (_isEditMode)
                {
                    // Debug: Kiểm tra entity trước khi update
                    Debug.WriteLine($"Before UpdateAsync - Department.BranchId: {department.BranchId}");
                    Debug.WriteLine($"Before UpdateAsync - Department.ParentId: {department.ParentId}");
                    Debug.WriteLine($"Before UpdateAsync - Department.Id: {department.Id}");
                    
                    await _departmentBll.UpdateAsync(department);
                    XtraMessageBox.Show("Cập nhật phòng ban thành công", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    await _departmentBll.CreateAsync(department);
                    XtraMessageBox.Show("Tạo mới phòng ban thành công", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lưu phòng ban: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Get department data from controls
        /// </summary>
        /// <returns>Department entity</returns>
        private Department GetDepartmentFromControls()
        {
            try
            {
                // Lấy CompanyId từ database
                var companyId = GetCompanyIdFromDatabase();
                if (companyId == Guid.Empty)
                {
                    throw new Exception("Không tìm thấy thông tin công ty trong hệ thống.");
                }

                // Debug: Kiểm tra giá trị _branchId
                Debug.WriteLine($"GetDepartmentFromControls - _branchId: {_branchId}");
                Debug.WriteLine($"GetDepartmentFromControls - _parentId: {_parentId}");

                // Khởi tạo luôn Entity từ controls và biến đã lưu
                var department = new Department
                {
                    DepartmentCode = DepartmentCodeTextEdit.Text.Trim(),
                    DepartmentName = DepartmentNameTextEdit.Text.Trim(),
                    Description = DescriptionTextEdit.Text?.Trim(),
                    IsActive = (bool)IsActiveToogleSwitch.EditValue,
                    CompanyId = companyId, // Lấy từ current company context
                    BranchId = _branchId, // Sử dụng biến đã lưu
                    ParentId = _parentId  // Sử dụng biến đã lưu (có thể là NULL)
                };

                if (_isEditMode)
                {
                    department.Id = _departmentId;
                }
                else
                {
                    department.Id = Guid.NewGuid();
                }

                return department;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi get dữ liệu từ controls: {ex.Message}", ex);
            }
        }

        #endregion

        #region ========== HELPER METHODS ==========

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
                Debug.WriteLine($"Lỗi lấy CompanyId từ database: {ex.Message}");
                return Guid.Empty;
            }
        }

        /// <summary>
        /// Setup advanced validation với DataAnnotations reflection
        /// </summary>
        private void SetupAdvancedValidation()
        {
            try
            {
                // Đánh dấu các trường bắt buộc theo DataAnnotations của DTO
                MarkRequiredFields(typeof(DepartmentDto));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Lỗi setup advanced validation: {ex.Message}");
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
                Debug.WriteLine($"Lỗi đánh dấu trường bắt buộc: {ex.Message}");
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

        #endregion
    }
}