using Bll.MasterData.CompanyBll;
using Common.Utils;
using DevExpress.XtraEditors;
using DTO.MasterData.Company;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Common.Utils.AlertHelper;

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

                // Setup SuperToolTips
                SetupSuperTips();
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi khởi tạo form: {ex.Message}", "Lỗi", this);
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
                
                // Load Department data cho ParentDepartmentNameSearchLookup
                LoadDepartments();
                
                // Setup SearchLookUpEdit để hiển thị HTML
                SetupSearchLookUpEdits();
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi load datasource: {ex.Message}", "Lỗi", this);
            }
        }

        /// <summary>
        /// Setup các SearchLookUpEdit để hiển thị HTML đẹp
        /// </summary>
        private void SetupSearchLookUpEdits()
        {
            try
            {
                // Setup Branch SearchLookUpEdit
                if (colBranchInfoHtml != null)
                {
                    colBranchInfoHtml.FieldName = "BranchInfoHtml";
                    colBranchInfoHtml.Visible = true;
                    colBranchInfoHtml.VisibleIndex = 0;
                }

                // Setup Department SearchLookUpEdit
                if (colFullPathHtml != null)
                {
                    colFullPathHtml.FieldName = "FullPathHtml";
                    colFullPathHtml.Visible = true;
                    colFullPathHtml.VisibleIndex = 0;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Lỗi setup SearchLookUpEdit: {ex.Message}");
            }
        }

        /// <summary>
        /// Load danh sách chi nhánh công ty cho Lookup (tối ưu hiệu năng)
        /// </summary>
        private void LoadCompanyBranches()
        {
            try
            {
                // GetActiveBranchesForLookup() already returns List<CompanyBranchDto>
                var companyBranches = _companyBranchBll.GetActiveBranchesForLookup();

                companyBranchLookupDtoBindingSource.DataSource = companyBranches;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi load danh sách chi nhánh: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Load danh sách phòng ban cho ParentDepartmentNameSearchLookup (tối ưu hiệu năng)
        /// </summary>
        private void LoadDepartments()
        {
            try
            {
                // GetAll() already returns List<DepartmentDto>
                // Convert to DepartmentLookupDto with FullPath calculation
                var departments = _departmentBll.GetAll();
                
                // Create dictionary for FullPath calculation
                var departmentDict = departments.ToDictionary(d => d.Id);
                
                // Convert to LookupDto with FullPath
                var departmentLookupDtos = departments.Select(d => new DepartmentLookupDto
                {
                    Id = d.Id,
                    CompanyId = d.CompanyId,
                    BranchId = d.BranchId,
                    DepartmentCode = d.DepartmentCode,
                    DepartmentName = d.DepartmentName,
                    ParentId = d.ParentId,
                    IsActive = d.IsActive,
                    FullPath = d.FullPath ?? d.DepartmentName // Use FullPath from DTO if available
                }).ToList();

                // QUAN TRỌNG: Nếu đang ở chế độ edit, loại trừ department hiện tại khỏi danh sách parent
                // (để tránh chọn chính nó làm parent - gây circular reference)
                // Điều này đảm bảo data source của phòng ban cha không có chính nó
                if (_isEditMode && _departmentId != Guid.Empty)
                {
                    departmentLookupDtos = departmentLookupDtos
                        .Where(d => d.Id != _departmentId)
                        .ToList();
                }

                departmentLookupDtoBindingSource.DataSource = departmentLookupDtos;
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

                // Load department từ repository
                var department = _departmentBll.GetById(_departmentId);
                if (department == null)
                {
                    ShowError("Không tìm thấy phòng ban", "Lỗi", this);
                    Close();
                    return;
                }

                // GetById() already returns DepartmentDto
                // Load all departments to calculate FullPath if needed
                var allDepartments = _departmentBll.GetAll();
                
                // Use the DTO directly (it should already have FullPath calculated)
                _currentDepartment = department;

                // QUAN TRỌNG: Đảm bảo branch hiện tại có trong datasource (kể cả khi không active)
                // Vì khi edit, cần hiển thị branch hiện tại dù nó có active hay không
                if (_currentDepartment.BranchId.HasValue)
                {
                    var branchId = _currentDepartment.BranchId.Value;
                    var branchExists = companyBranchLookupDtoBindingSource.Cast<CompanyBranchDto>()
                        .Any(b => b.Id == branchId);
                    
                    if (!branchExists)
                    {
                        // Branch không tồn tại trong datasource (có thể đã bị inactive)
                        // Load branch này và thêm vào datasource
                        var branch = _companyBranchBll.GetById(branchId);
                        if (branch != null)
                        {
                            // Use CompanyBranchDto directly
                            var currentList = companyBranchLookupDtoBindingSource.Cast<CompanyBranchDto>().ToList();
                            currentList.Add(branch);
                            companyBranchLookupDtoBindingSource.DataSource = currentList;
                        }
                    }
                }

                // QUAN TRỌNG: Đảm bảo parent department hiện tại có trong datasource (nếu có)
                // Lưu ý: department hiện tại đã bị filter ra khỏi datasource để tránh circular reference
                if (_currentDepartment.ParentId.HasValue)
                {
                    var parentId = _currentDepartment.ParentId.Value;
                    var parentExists = departmentLookupDtoBindingSource.Cast<DepartmentLookupDto>()
                        .Any(d => d.Id == parentId);
                    
                    if (!parentExists)
                    {
                        // Parent department không tồn tại trong datasource
                        // Load parent department này và thêm vào datasource (nếu không phải chính nó)
                        if (parentId != _departmentId)
                        {
                            var parentDepartment = _departmentBll.GetById(parentId);
                            if (parentDepartment != null)
                            {
                                // Convert DepartmentDto to DepartmentLookupDto
                                var parentLookupDto = new DepartmentLookupDto
                                {
                                    Id = parentDepartment.Id,
                                    CompanyId = parentDepartment.CompanyId,
                                    BranchId = parentDepartment.BranchId,
                                    DepartmentCode = parentDepartment.DepartmentCode,
                                    DepartmentName = parentDepartment.DepartmentName,
                                    ParentId = parentDepartment.ParentId,
                                    IsActive = parentDepartment.IsActive,
                                    FullPath = parentDepartment.FullPath ?? parentDepartment.DepartmentName
                                };
                                var currentList = departmentLookupDtoBindingSource.Cast<DepartmentLookupDto>().ToList();
                                currentList.Add(parentLookupDto);
                                departmentLookupDtoBindingSource.DataSource = currentList;
                            }
                        }
                    }
                }

                // Refresh datasource để đảm bảo dữ liệu mới nhất
                companyBranchLookupDtoBindingSource.ResetBindings(false);
                departmentLookupDtoBindingSource.ResetBindings(false);

                // Bind dữ liệu vào controls
                BindDepartmentToControls();
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi load dữ liệu phòng ban: {ex.Message}", "Lỗi", this);
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

                // QUAN TRỌNG: Disable các controls không cho phép thay đổi khi edit
                // Đảm bảo rằng khi ở chế độ edit thì không cho phép thay đổi:
                // - Mã phòng ban
                // - Chi nhánh (BranchNameSearchLookupedit)
                // - Phòng ban cha (ParentDepartmentNameSearchLookup)
                SetControlsReadOnly(true);

                // Set branch selection và cập nhật biến
                // QUAN TRỌNG: Kiểm tra giá trị có tồn tại trong datasource trước khi bind
                if (_currentDepartment.BranchId.HasValue)
                {
                    var branchId = _currentDepartment.BranchId.Value;
                    
                    // Kiểm tra branch có tồn tại trong datasource không
                    var branchExists = companyBranchLookupDtoBindingSource.Cast<CompanyBranchDto>()
                        .Any(b => b.Id == branchId);
                    
                    if (branchExists)
                    {
                        BranchNameSearchLookupedit.EditValue = branchId;
                        _branchId = branchId;
                    }
                    else
                    {
                        // Branch không tồn tại trong datasource (có thể đã bị inactive)
                        // Vẫn set giá trị để hiển thị, nhưng có thể không hiển thị text
                        BranchNameSearchLookupedit.EditValue = branchId;
                        _branchId = branchId;
                        Debug.WriteLine($"Warning: BranchId {branchId} không tồn tại trong datasource (có thể đã bị inactive)");
                    }
                }
                else
                {
                    BranchNameSearchLookupedit.EditValue = null;
                    _branchId = null;
                }

                // Set parent department selection và cập nhật biến
                // QUAN TRỌNG: Kiểm tra giá trị có tồn tại trong datasource trước khi bind
                if (_currentDepartment.ParentId.HasValue)
                {
                    var parentId = _currentDepartment.ParentId.Value;
                    
                    // Kiểm tra parent department có tồn tại trong datasource không
                    // (Lưu ý: department hiện tại đã bị filter ra khỏi datasource)
                    var parentExists = departmentLookupDtoBindingSource.Cast<DepartmentLookupDto>()
                        .Any(d => d.Id == parentId);
                    
                    if (parentExists)
                    {
                        ParentDepartmentNameSearchLookup.EditValue = parentId;
                        _parentId = parentId;
                    }
                    else
                    {
                        // Parent department không tồn tại trong datasource
                        // (có thể đã bị filter ra vì là chính nó, hoặc đã bị xóa)
                        // Vẫn set giá trị để hiển thị, nhưng có thể không hiển thị text
                        ParentDepartmentNameSearchLookup.EditValue = parentId;
                        _parentId = parentId;
                        Debug.WriteLine($"Warning: ParentId {parentId} không tồn tại trong datasource (có thể đã bị filter hoặc xóa)");
                    }
                }
                else
                {
                    ParentDepartmentNameSearchLookup.EditValue = null;
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
                ParentDepartmentNameSearchLookup.EditValue = null;
                
                // Enable các controls khi tạo mới (cho phép nhập/chọn)
                SetControlsReadOnly(false);
                
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
                ParentDepartmentNameSearchLookup.EditValueChanged += ParentDepartmentNameTextEdit_EditValueChanged;
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
                ShowError($"Lỗi lưu phòng ban: {ex.Message}", "Lỗi", this);
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
                ShowError($"Lỗi đóng form: {ex.Message}", "Lỗi", this);
            }
        }

        /// <summary>
        /// Xử lý sự kiện thay đổi giá trị BranchNameSearchLookupedit
        /// QUAN TRỌNG: Khi ở chế độ edit, control đã bị disable nên event này sẽ không được trigger
        /// </summary>
        private void BranchNameSearchLookupedit_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                // Đảm bảo không cho phép thay đổi khi ở chế độ edit
                if (_isEditMode)
                {
                    // Nếu đang ở chế độ edit, giữ nguyên giá trị cũ
                    if (_branchId.HasValue)
                    {
                        BranchNameSearchLookupedit.EditValue = _branchId.Value;
                    }
                    return;
                }

                // Chỉ cập nhật khi ở chế độ tạo mới
                if (BranchNameSearchLookupedit.EditValue != null && 
                    Guid.TryParse(BranchNameSearchLookupedit.EditValue.ToString(), out var branchId))
                {
                    _branchId = branchId;
                }
                else
                {
                    _branchId = null;
                }
            }
            catch (Exception ex)
            {
                // Log lỗi nhưng không throw để không làm crash form
                Debug.WriteLine($"Lỗi cập nhật BranchId: {ex.Message}");
            }
        }

        /// <summary>
        /// Xử lý sự kiện thay đổi giá trị ParentDepartmentNameSearchLookup
        /// QUAN TRỌNG: Khi ở chế độ edit, control đã bị disable nên event này sẽ không được trigger
        /// </summary>
        private void ParentDepartmentNameTextEdit_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                // Đảm bảo không cho phép thay đổi khi ở chế độ edit
                if (_isEditMode)
                {
                    // Nếu đang ở chế độ edit, giữ nguyên giá trị cũ
                    if (_parentId.HasValue)
                    {
                        ParentDepartmentNameSearchLookup.EditValue = _parentId.Value;
                    }
                    else
                    {
                        ParentDepartmentNameSearchLookup.EditValue = null;
                    }
                    return;
                }

                // Chỉ cập nhật khi ở chế độ tạo mới
                if (ParentDepartmentNameSearchLookup.EditValue != null && 
                    Guid.TryParse(ParentDepartmentNameSearchLookup.EditValue.ToString(), out var parentId))
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
                // Log lỗi nhưng không throw để không làm crash form
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
                    await _departmentBll.UpdateAsync(department);
                    ShowSuccess("Cập nhật phòng ban thành công", "Thành công", this);
                }
                else
                {
                    await _departmentBll.CreateAsync(department);
                    ShowSuccess("Tạo mới phòng ban thành công", "Thành công", this);
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
        /// <returns>DepartmentDto</returns>
        private DepartmentDto GetDepartmentFromControls()
        {
            try
            {
                // Lấy CompanyId từ database
                var companyId = GetCompanyIdFromDatabase();
                if (companyId == Guid.Empty)
                {
                    throw new Exception("Không tìm thấy thông tin công ty trong hệ thống.");
                }

                // Create DTO from controls and saved variables
                var department = new DepartmentDto
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

                // Cast về Company entity và lấy Id
                return company?.Id ?? Guid.Empty;
            }
            catch (Exception ex)
            {
                // Log lỗi nhưng không throw để không làm crash form
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
                RequiredFieldHelper.MarkRequiredFields(
                    this, 
                    typeof(DepartmentDto),
                    logger: (msg, ex) => Debug.WriteLine($"{msg}: {ex?.Message}")
                );
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Lỗi setup advanced validation: {ex.Message}");
            }
        }

        /// <summary>
        /// Thiết lập trạng thái ReadOnly cho các controls
        /// QUAN TRỌNG: Khi ở chế độ edit (readOnly = true), các controls này sẽ bị khóa hoàn toàn
        /// </summary>
        /// <param name="readOnly">True để disable (chế độ edit), False để enable (chế độ tạo mới)</param>
        private void SetControlsReadOnly(bool readOnly)
        {
            try
            {
                // Mã phòng ban - không cho phép thay đổi khi edit
                DepartmentCodeTextEdit.Properties.ReadOnly = readOnly;
                DepartmentCodeTextEdit.Properties.AllowNullInput = readOnly 
                    ? DevExpress.Utils.DefaultBoolean.False 
                    : DevExpress.Utils.DefaultBoolean.True;
                DepartmentCodeTextEdit.Enabled = !readOnly;

                // Chi nhánh SearchLookUpEdit - QUAN TRỌNG: Không cho phép thay đổi khi edit
                // Đảm bảo rằng khi ở chế độ edit thì không cho phép thay đổi thông tin của chi nhánh
                BranchNameSearchLookupedit.Properties.ReadOnly = readOnly;
                BranchNameSearchLookupedit.Enabled = !readOnly;
                // Thêm AllowNullInput = False để đảm bảo không thể clear giá trị khi edit
                if (readOnly)
                {
                    BranchNameSearchLookupedit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
                }

                // Phòng ban cha SearchLookUpEdit - không cho phép thay đổi khi edit
                ParentDepartmentNameSearchLookup.Properties.ReadOnly = readOnly;
                ParentDepartmentNameSearchLookup.Enabled = !readOnly;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Lỗi set controls read-only: {ex.Message}");
            }
        }

        #endregion

        #region ========== SUPERTOOLTIP ==========

        /// <summary>
        /// Thiết lập SuperToolTip cho tất cả các controls trong form
        /// </summary>
        private void SetupSuperTips()
        {
            try
            {
                SetupTextEditSuperTips();
                SetupSearchLookupSuperTips();
                SetupBarButtonSuperTips();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Lỗi khi setup SuperToolTip: {ex.Message}");
            }
        }

        /// <summary>
        /// Thiết lập SuperToolTip cho các TextEdit controls
        /// </summary>
        private void SetupTextEditSuperTips()
        {
            // SuperTip cho Mã phòng ban
            SuperToolTipHelper.SetTextEditSuperTip(
                DepartmentCodeTextEdit,
                title: @"<b><color=DarkBlue>🏷️ Mã phòng ban</color></b>",
                content: @"Nhập <b>mã phòng ban</b> trong hệ thống.<br/><br/><b>Chức năng:</b><br/>• Nhập mã phòng ban (ví dụ: PB01, PB02, v.v.)<br/>• Hiển thị mã phòng ban khi chỉnh sửa<br/>• Validation tự động khi rời khỏi control<br/><br/><b>Ràng buộc:</b><br/>• <b>Bắt buộc nhập</b> khi thêm mới (có dấu * đỏ)<br/>• Không được để trống khi tạo mới<br/>• Tối đa 50 ký tự<br/>• Tự động trim khoảng trắng đầu/cuối<br/>• Không thể chỉnh sửa khi đang ở chế độ edit<br/><br/><b>Validation:</b><br/>• Kiểm tra rỗng khi tạo mới<br/>• Kiểm tra độ dài tối đa (50 ký tự)<br/>• Hiển thị lỗi qua ErrorProvider nếu không hợp lệ<br/><br/><b>DataAnnotations:</b><br/>• Có attribute [Required] trong DTO<br/>• Có attribute [StringLength(50)]<br/>• Tự động đánh dấu * đỏ trong layout<br/>• Hiển thị prompt 'Bắt buộc nhập' khi rỗng<br/><br/><color=Gray>Lưu ý:</color> Mã phòng ban sẽ được lưu vào database khi click nút Lưu. Khi đang ở chế độ chỉnh sửa, mã phòng ban sẽ bị khóa và không thể thay đổi."
            );

            // SuperTip cho Tên phòng ban
            SuperToolTipHelper.SetTextEditSuperTip(
                DepartmentNameTextEdit,
                title: @"<b><color=DarkBlue>🏢 Tên phòng ban</color></b>",
                content: @"Nhập <b>tên phòng ban</b> trong hệ thống.<br/><br/><b>Chức năng:</b><br/>• Nhập tên phòng ban (ví dụ: Phòng Kinh doanh, Phòng Kỹ thuật, v.v.)<br/>• Hiển thị tên phòng ban khi chỉnh sửa<br/>• Validation tự động khi rời khỏi control<br/><br/><b>Ràng buộc:</b><br/>• <b>Bắt buộc nhập</b> (có dấu * đỏ)<br/>• Không được để trống<br/>• Tối đa 255 ký tự<br/>• Không được chứa chỉ khoảng trắng<br/>• Tự động trim khoảng trắng đầu/cuối<br/><br/><b>Validation:</b><br/>• Kiểm tra rỗng khi validating<br/>• Kiểm tra độ dài tối đa (255 ký tự)<br/>• Hiển thị lỗi qua ErrorProvider nếu không hợp lệ<br/><br/><b>DataAnnotations:</b><br/>• Có attribute [Required] trong DTO<br/>• Có attribute [StringLength(255)]<br/>• Tự động đánh dấu * đỏ trong layout<br/>• Hiển thị prompt 'Bắt buộc nhập' khi rỗng<br/><br/><color=Gray>Lưu ý:</color> Tên phòng ban sẽ được lưu vào database khi click nút Lưu. Nếu đang ở chế độ chỉnh sửa, tên hiện tại sẽ được hiển thị sẵn."
            );

            // SuperTip cho Mô tả
            SuperToolTipHelper.SetTextEditSuperTip(
                DescriptionTextEdit,
                title: @"<b><color=DarkBlue>📝 Mô tả</color></b>",
                content: @"Nhập <b>mô tả</b> của phòng ban (tùy chọn).<br/><br/><b>Chức năng:</b><br/>• Nhập mô tả chi tiết về phòng ban<br/>• Hiển thị mô tả khi chỉnh sửa<br/>• Validation tự động khi rời khỏi control<br/><br/><b>Ràng buộc:</b><br/>• <b>Không bắt buộc nhập</b> (có thể để trống)<br/>• Tối đa 255 ký tự nếu có nhập<br/>• Tự động trim khoảng trắng đầu/cuối<br/><br/><b>Validation:</b><br/>• Chỉ kiểm tra độ dài tối đa (255 ký tự) nếu có nhập<br/>• Hiển thị lỗi qua ErrorProvider nếu vượt quá độ dài<br/>• Không bắt buộc nhập<br/><br/><b>DataAnnotations:</b><br/>• Không có attribute [Required] trong DTO<br/>• Có attribute [StringLength(255)]<br/>• Có thể để trống<br/><br/><color=Gray>Lưu ý:</color> Mô tả sẽ được lưu vào database khi click nút Lưu. Nếu đang ở chế độ chỉnh sửa, mô tả hiện tại sẽ được hiển thị sẵn. Có thể để trống nếu không cần thiết."
            );
        }

        /// <summary>
        /// Thiết lập SuperToolTip cho các SearchLookupEdit controls
        /// </summary>
        private void SetupSearchLookupSuperTips()
        {
            // SuperTip cho Chi nhánh
            SuperToolTipHelper.SetBaseEditSuperTip(
                BranchNameSearchLookupedit,
                title: @"<b><color=DarkBlue>🏢 Chi nhánh</color></b>",
                content: @"Chọn <b>chi nhánh</b> mà phòng ban thuộc về.<br/><br/><b>Chức năng:</b><br/>• Chọn chi nhánh từ danh sách dropdown<br/>• Hiển thị tên chi nhánh đã chọn<br/>• Validation tự động khi rời khỏi control<br/><br/><b>Ràng buộc:</b><br/>• <b>Bắt buộc chọn</b> (có dấu * đỏ)<br/>• Không được để trống<br/>• Phải chọn một chi nhánh hợp lệ<br/><br/><b>Validation:</b><br/>• Kiểm tra rỗng khi validating<br/>• Kiểm tra chi nhánh có tồn tại không<br/>• Hiển thị lỗi qua ErrorProvider nếu không hợp lệ<br/><br/><b>DataAnnotations:</b><br/>• BranchId không có attribute [Required] trong DTO nhưng được validate trong form<br/>• Có thể để trống về mặt DTO nhưng form yêu cầu bắt buộc<br/><br/><color=Gray>Lưu ý:</color> Chi nhánh sẽ được lưu vào database khi click nút Lưu. Danh sách chi nhánh được load từ database và chỉ hiển thị các chi nhánh đang hoạt động."
            );

            // SuperTip cho Phòng ban cha
            SuperToolTipHelper.SetBaseEditSuperTip(
                ParentDepartmentNameSearchLookup,
                title: @"<b><color=DarkBlue>👥 Phòng ban cha</color></b>",
                content: @"Chọn <b>phòng ban cha</b> (tùy chọn) để tạo cấu trúc phân cấp.<br/><br/><b>Chức năng:</b><br/>• Chọn phòng ban cha từ danh sách dropdown<br/>• Hiển thị tên phòng ban cha đã chọn<br/>• Tạo cấu trúc phân cấp phòng ban<br/><br/><b>Ràng buộc:</b><br/>• <b>Không bắt buộc chọn</b> (có thể để trống)<br/>• Có thể để trống nếu phòng ban không có phòng ban cha<br/>• Phải chọn một phòng ban hợp lệ nếu có chọn<br/><br/><b>Validation:</b><br/>• Không bắt buộc nhập<br/>• Kiểm tra phòng ban có tồn tại không nếu có chọn<br/>• Hiển thị lỗi qua ErrorProvider nếu không hợp lệ<br/><br/><b>DataAnnotations:</b><br/>• ParentId không có attribute [Required] trong DTO<br/>• Có thể để trống (NULL)<br/><br/><color=Gray>Lưu ý:</color> Phòng ban cha sẽ được lưu vào database khi click nút Lưu. Nếu để trống, phòng ban này sẽ là phòng ban cấp cao nhất. Danh sách phòng ban được load từ database."
            );
        }

        /// <summary>
        /// Thiết lập SuperToolTip cho các BarButtonItem
        /// </summary>
        private void SetupBarButtonSuperTips()
        {
            // SuperTip cho nút Lưu
            SuperToolTipHelper.SetBarButtonSuperTip(
                SaveBarButtonItem,
                title: @"<b><color=Blue>💾 Lưu</color></b>",
                content: @"Lưu <b>thông tin phòng ban</b> vào database.<br/><br/><b>Chức năng:</b><br/>• Validate tất cả dữ liệu đầu vào<br/>• Tạo hoặc cập nhật phòng ban trong database<br/>• Hiển thị thông báo thành công/thất bại<br/>• Đóng form sau khi lưu thành công<br/><br/><b>Quy trình:</b><br/>• Validate toàn bộ form (Mã phòng ban, Tên phòng ban, Chi nhánh, v.v.)<br/>• Lấy dữ liệu từ form và tạo Department Entity<br/>• Lấy CompanyId từ database (vì chỉ có 1 Company duy nhất)<br/>• Nếu chỉnh sửa: Cập nhật entity với ID hiện tại<br/>• Nếu thêm mới: Tạo entity mới với ID mới<br/>• Lưu vào database qua DepartmentBll<br/>• Hiển thị thông báo thành công<br/>• Đóng form<br/><br/><b>Yêu cầu:</b><br/>• Mã phòng ban phải không được để trống (khi tạo mới)<br/>• Tên phòng ban phải không được để trống<br/>• Chi nhánh phải được chọn<br/>• Tất cả validation phải pass<br/><br/><b>Kết quả:</b><br/>• Nếu thành công: Hiển thị thông báo và đóng form<br/>• Nếu lỗi: Hiển thị thông báo lỗi, form vẫn mở để chỉnh sửa<br/><br/><color=Gray>Lưu ý:</color> Nếu có lỗi validation, form sẽ không đóng và bạn có thể sửa lại. Dữ liệu sẽ được lưu vào database sau khi tất cả validation pass."
            );

            // SuperTip cho nút Đóng
            SuperToolTipHelper.SetBarButtonSuperTip(
                CloseBarButtonItem,
                title: @"<b><color=DarkRed>❌ Đóng</color></b>",
                content: @"Đóng form <b>chi tiết phòng ban</b> mà không lưu thay đổi.<br/><br/><b>Chức năng:</b><br/>• Đóng form ngay lập tức<br/>• Không lưu dữ liệu đã nhập<br/>• Không ảnh hưởng đến database<br/>• Set DialogResult = Cancel<br/><br/><b>Phím tắt:</b><br/>• Escape: Đóng form<br/><br/><color=Gray>Lưu ý:</color> Tất cả dữ liệu đã nhập (Mã phòng ban, Tên phòng ban, Chi nhánh, v.v.) sẽ bị mất khi đóng form. Nếu muốn lưu, hãy click nút Lưu trước khi đóng."
            );
        }

        #endregion
    }
}