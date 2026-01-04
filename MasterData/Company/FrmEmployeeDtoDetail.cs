using Bll.MasterData.CompanyBll;
using Common.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using DTO.MasterData.Company;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Common.Utils.AlertHelper;

namespace MasterData.Company
{
    public partial class FrmEmployeeDtoDetail : XtraForm
    {
        #region ========== FIELDS ==========

        private readonly Guid _employeeId;
        private readonly EmployeeBll _employeeBll;
        private readonly CompanyBranchBll _companyBranchBll;
        private readonly DepartmentBll _departmentBll;
        private readonly PositionBll _positionBll;
        private EmployeeDto _currentEmployee;
        private readonly bool _isEditMode;
        
        // Biến lưu trữ ID của các lookup
        private Guid? _branchId;
        private Guid? _departmentId;
        private Guid? _positionId;

        #endregion

        #region ========== CONSTRUCTOR ==========

        /// <summary>
        /// Constructor cho thêm mới nhân viên
        /// </summary>
        public FrmEmployeeDtoDetail()
        {
            InitializeComponent();
            _employeeId = Guid.Empty;
            _employeeBll = new EmployeeBll();
            _companyBranchBll = new CompanyBranchBll();
            _departmentBll = new DepartmentBll();
            _positionBll = new PositionBll();
            _isEditMode = false;
            InitializeForm();
        }

        /// <summary>
        /// Constructor cho điều chỉnh nhân viên
        /// </summary>
        /// <param name="employeeId">ID của nhân viên cần điều chỉnh</param>
        public FrmEmployeeDtoDetail(Guid employeeId)
        {
            InitializeComponent();
            _employeeId = employeeId;
            _employeeBll = new EmployeeBll();
            _companyBranchBll = new CompanyBranchBll();
            _departmentBll = new DepartmentBll();
            _positionBll = new PositionBll();
            _isEditMode = employeeId != Guid.Empty;
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
                    LoadEmployeeData();
                    Text = @"Chỉnh sửa nhân viên";
                }
                else
                {
                    Text = @"Thêm mới nhân viên";
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
                // Load CompanyBranch data cho BranchIdSearchLookupEdit
                LoadCompanyBranches();
                
                // Load Department data cho DepartmentIdSearchLookupEdit
                LoadDepartments();
                
                // Load Position data cho PositionIdSearchLookupEdit
                LoadPositions();
                
                // Setup Gender ComboBox
                SetupGenderComboBox();
                
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
                if (colDepartmentInfoHtml != null)
                {
                    colDepartmentInfoHtml.FieldName = "DepartmentInfoHtml";
                    colDepartmentInfoHtml.Visible = true;
                    colDepartmentInfoHtml.VisibleIndex = 0;
                }

                // Setup Position SearchLookUpEdit
                if (colThongTinHtml != null)
                {
                    colThongTinHtml.FieldName = "ThongTinHtml";
                    colThongTinHtml.Visible = true;
                    colThongTinHtml.VisibleIndex = 0;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Lỗi setup SearchLookUpEdit: {ex.Message}");
            }
        }

        /// <summary>
        /// Setup Gender ComboBox với các giá trị
        /// </summary>
        private void SetupGenderComboBox()
        {
            try
            {
                GenderComboBoxEdit.Properties.Items.Clear();
                GenderComboBoxEdit.Properties.Items.Add("Nam");
                GenderComboBoxEdit.Properties.Items.Add("Nữ");
                GenderComboBoxEdit.Properties.Items.Add("Khác");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Lỗi setup Gender ComboBox: {ex.Message}");
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
        /// Load danh sách phòng ban cho DepartmentIdSearchLookupEdit (tối ưu hiệu năng)
        /// </summary>
        private void LoadDepartments()
        {
            try
            {
                // GetAll() already returns List<DepartmentDto>
                // Convert to DepartmentLookupDto with FullPath
                var departments = _departmentBll.GetAll();
                
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

                departmentLookupDtoBindingSource.DataSource = departmentLookupDtos;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi load danh sách phòng ban: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Load danh sách chức vụ cho PositionIdSearchLookupEdit
        /// </summary>
        private void LoadPositions()
        {
            try
            {
                // GetAll() already returns List<PositionDto>
                var positions = _positionBll.GetAll();

                positionDtoBindingSource.DataSource = positions;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi load danh sách chức vụ: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Load dữ liệu nhân viên khi edit
        /// </summary>
        private void LoadEmployeeData()
        {
            try
            {
                // Chỉ load data khi thực sự là edit mode và có employeeId hợp lệ
                if (!_isEditMode || _employeeId == Guid.Empty)
                {
                    return;
                }

                // GetById() already returns EmployeeDto
                var employee = _employeeBll.GetById(_employeeId);
                if (employee == null)
                {
                    ShowError("Không tìm thấy nhân viên", "Lỗi", this);
                    Close();
                    return;
                }

                // Use DTO directly
                _currentEmployee = employee;

                // QUAN TRỌNG: Đảm bảo branch hiện tại có trong datasource (kể cả khi không active)
                if (_currentEmployee.BranchId.HasValue)
                {
                    var branchId = _currentEmployee.BranchId.Value;
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

                // QUAN TRỌNG: Đảm bảo department hiện tại có trong datasource (nếu có)
                if (_currentEmployee.DepartmentId.HasValue)
                {
                    var departmentId = _currentEmployee.DepartmentId.Value;
                    var departmentExists = departmentLookupDtoBindingSource.Cast<DepartmentLookupDto>()
                        .Any(d => d.Id == departmentId);
                    
                    if (!departmentExists)
                    {
                        // Department không tồn tại trong datasource
                        // Load department này và thêm vào datasource
                        var department = _departmentBll.GetById(departmentId);
                        if (department != null)
                        {
                            // Convert DepartmentDto to DepartmentLookupDto
                            var departmentLookupDto = new DepartmentLookupDto
                            {
                                Id = department.Id,
                                CompanyId = department.CompanyId,
                                BranchId = department.BranchId,
                                DepartmentCode = department.DepartmentCode,
                                DepartmentName = department.DepartmentName,
                                ParentId = department.ParentId,
                                IsActive = department.IsActive,
                                FullPath = department.FullPath ?? department.DepartmentName
                            };
                            var currentList = departmentLookupDtoBindingSource.Cast<DepartmentLookupDto>().ToList();
                            currentList.Add(departmentLookupDto);
                            departmentLookupDtoBindingSource.DataSource = currentList;
                        }
                    }
                }

                // QUAN TRỌNG: Đảm bảo position hiện tại có trong datasource (nếu có)
                if (_currentEmployee.PositionId.HasValue)
                {
                    var positionId = _currentEmployee.PositionId.Value;
                    var positionExists = positionDtoBindingSource.Cast<PositionDto>()
                        .Any(p => p.Id == positionId);
                    
                    if (!positionExists)
                    {
                        // Position không tồn tại trong datasource
                        // Load position này và thêm vào datasource
                        var position = _positionBll.GetById(positionId);
                        if (position != null)
                        {
                            // GetById() already returns PositionDto
                            var currentList = positionDtoBindingSource.Cast<PositionDto>().ToList();
                            currentList.Add(position);
                            positionDtoBindingSource.DataSource = currentList;
                        }
                    }
                }

                // Refresh datasource để đảm bảo dữ liệu mới nhất
                companyBranchLookupDtoBindingSource.ResetBindings(false);
                departmentLookupDtoBindingSource.ResetBindings(false);
                positionDtoBindingSource.ResetBindings(false);

                // Bind dữ liệu vào controls
                BindEmployeeToControls();
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi load dữ liệu nhân viên: {ex.Message}", "Lỗi", this);
            }
        }

        /// <summary>
        /// Bind dữ liệu nhân viên vào controls
        /// </summary>
        private void BindEmployeeToControls()
        {
            try
            {
                if (_currentEmployee == null) return;

                // Bind data to controls
                EmployeeCodeTextEdit.EditValue = _currentEmployee.EmployeeCode;
                FullNameTextEdit.EditValue = _currentEmployee.FullName;
                GenderComboBoxEdit.EditValue = _currentEmployee.Gender;
                BirthDateDateEdit.EditValue = _currentEmployee.BirthDate;
                PhoneTextEdit.EditValue = _currentEmployee.Phone;
                EmailTextEdit.EditValue = _currentEmployee.Email;
                HireDateDateEdit.EditValue = _currentEmployee.HireDate;
                ResignDateDateEdit.EditValue = _currentEmployee.ResignDate;
                MobileTextEdit.EditValue = _currentEmployee.Mobile;
                IsActiveToggleSwitch.EditValue = _currentEmployee.IsActive;
                NotesTextEdit.EditValue = _currentEmployee.Notes;

                // Bind Avatar
                if (_currentEmployee.AvatarThumbnailData != null && _currentEmployee.AvatarThumbnailData.Length > 0)
                {
                    using var ms = new MemoryStream(_currentEmployee.AvatarThumbnailData);
                    AvatarPictureEdit.Image = Image.FromStream(ms);
                }
                else
                {
                    AvatarPictureEdit.Image = null;
                }

                // QUAN TRỌNG: Disable các controls không cho phép thay đổi khi edit
                SetControlsReadOnly(true);

                // Set branch selection và cập nhật biến
                if (_currentEmployee.BranchId.HasValue)
                {
                    var branchId = _currentEmployee.BranchId.Value;
                    var branchExists = companyBranchLookupDtoBindingSource.Cast<CompanyBranchDto>()
                        .Any(b => b.Id == branchId);
                    
                    if (branchExists)
                    {
                        BranchIdSearchLookupEdit.EditValue = branchId;
                        _branchId = branchId;
                    }
                    else
                    {
                        BranchIdSearchLookupEdit.EditValue = branchId;
                        _branchId = branchId;
                        Debug.WriteLine($"Warning: BranchId {branchId} không tồn tại trong datasource");
                    }
                }
                else
                {
                    BranchIdSearchLookupEdit.EditValue = null;
                    _branchId = null;
                }

                // Set department selection và cập nhật biến
                if (_currentEmployee.DepartmentId.HasValue)
                {
                    var departmentId = _currentEmployee.DepartmentId.Value;
                    var departmentExists = departmentLookupDtoBindingSource.Cast<DepartmentLookupDto>()
                        .Any(d => d.Id == departmentId);
                    
                    if (departmentExists)
                    {
                        DepartmentIdSearchLookupEdit.EditValue = departmentId;
                        _departmentId = departmentId;
                    }
                    else
                    {
                        DepartmentIdSearchLookupEdit.EditValue = departmentId;
                        _departmentId = departmentId;
                        Debug.WriteLine($"Warning: DepartmentId {departmentId} không tồn tại trong datasource");
                    }
                }
                else
                {
                    DepartmentIdSearchLookupEdit.EditValue = null;
                    _departmentId = null;
                }

                // Set position selection và cập nhật biến
                if (_currentEmployee.PositionId.HasValue)
                {
                    var positionId = _currentEmployee.PositionId.Value;
                    var positionExists = positionDtoBindingSource.Cast<PositionDto>()
                        .Any(p => p.Id == positionId);
                    
                    if (positionExists)
                    {
                        PositionIdSearchLookupEdit.EditValue = positionId;
                        _positionId = positionId;
                    }
                    else
                    {
                        PositionIdSearchLookupEdit.EditValue = positionId;
                        _positionId = positionId;
                        Debug.WriteLine($"Warning: PositionId {positionId} không tồn tại trong datasource");
                    }
                }
                else
                {
                    PositionIdSearchLookupEdit.EditValue = null;
                    _positionId = null;
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
                EmployeeCodeTextEdit.EditValue = string.Empty;
                FullNameTextEdit.EditValue = string.Empty;
                GenderComboBoxEdit.EditValue = null;
                BirthDateDateEdit.EditValue = null;
                PhoneTextEdit.EditValue = string.Empty;
                EmailTextEdit.EditValue = string.Empty;
                HireDateDateEdit.EditValue = null;
                ResignDateDateEdit.EditValue = null;
                MobileTextEdit.EditValue = string.Empty;
                IsActiveToggleSwitch.EditValue = true;
                NotesTextEdit.EditValue = string.Empty;
                AvatarPictureEdit.Image = null;
                BranchIdSearchLookupEdit.EditValue = null;
                DepartmentIdSearchLookupEdit.EditValue = null;
                PositionIdSearchLookupEdit.EditValue = null;
                
                // Enable các controls khi tạo mới (cho phép nhập/chọn)
                SetControlsReadOnly(false);
                
                // Khởi tạo các biến ID
                _branchId = null;
                _departmentId = null;
                _positionId = null;
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
                BranchIdSearchLookupEdit.EditValueChanged += BranchIdSearchLookupEdit_EditValueChanged;
                DepartmentIdSearchLookupEdit.EditValueChanged += DepartmentIdSearchLookupEdit_EditValueChanged;
                PositionIdSearchLookupEdit.EditValueChanged += PositionIdSearchLookupEdit_EditValueChanged;
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
                    await SaveEmployee();
                }
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi lưu nhân viên: {ex.Message}", "Lỗi", this);
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
        /// Xử lý sự kiện thay đổi giá trị BranchIdSearchLookupEdit
        /// </summary>
        private void BranchIdSearchLookupEdit_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                // Cập nhật giá trị cho cả chế độ tạo mới và chỉnh sửa
                if (BranchIdSearchLookupEdit.EditValue != null && 
                    Guid.TryParse(BranchIdSearchLookupEdit.EditValue.ToString(), out var branchId))
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
                Debug.WriteLine($"Lỗi cập nhật BranchId: {ex.Message}");
            }
        }

        /// <summary>
        /// Xử lý sự kiện thay đổi giá trị DepartmentIdSearchLookupEdit
        /// </summary>
        private void DepartmentIdSearchLookupEdit_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                // Cập nhật giá trị cho cả chế độ tạo mới và chỉnh sửa
                if (DepartmentIdSearchLookupEdit.EditValue != null && 
                    Guid.TryParse(DepartmentIdSearchLookupEdit.EditValue.ToString(), out var departmentId))
                {
                    _departmentId = departmentId;
                }
                else
                {
                    _departmentId = null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Lỗi cập nhật DepartmentId: {ex.Message}");
            }
        }

        /// <summary>
        /// Xử lý sự kiện thay đổi giá trị PositionIdSearchLookupEdit
        /// </summary>
        private void PositionIdSearchLookupEdit_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                // Cập nhật giá trị cho cả chế độ tạo mới và chỉnh sửa
                if (PositionIdSearchLookupEdit.EditValue != null && 
                    Guid.TryParse(PositionIdSearchLookupEdit.EditValue.ToString(), out var positionId))
                {
                    _positionId = positionId;
                }
                else
                {
                    _positionId = null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Lỗi cập nhật PositionId: {ex.Message}");
            }
        }

        #endregion

        #region ========== VALIDATION & SAVE ==========

        /// <summary>
        /// Validate form data sử dụng DataAnnotations
        /// </summary>
        /// <returns>True nếu hợp lệ</returns>
        private bool ValidateForm()
        {
            try
            {
                dxErrorProvider1.ClearErrors();

                // Convert Entity sang DTO để validate (vì DataAnnotations chỉ hoạt động với DTO)
                var dto = GetEmployeeDtoFromControls();
                if (dto == null)
                {
                    ShowError("Không thể convert dữ liệu sang DTO để validate", "Lỗi", this);
                    return false;
                }

                // Validate bằng DataAnnotations trên DTO
                var context = new ValidationContext(dto, serviceProvider: null, items: null);
                var results = new List<ValidationResult>();
                bool isValid = Validator.TryValidateObject(dto, context, results, validateAllProperties: true);

                if (!isValid)
                {
                    // Hiển thị lỗi cho từng field
                    foreach (var result in results)
                    {
                        foreach (var memberName in result.MemberNames)
                        {
                            var control = FindControlByPropertyName(memberName);
                            if (control != null)
                            {
                                dxErrorProvider1.SetError(control, result.ErrorMessage, ErrorType.Critical);
                            }
                        }
                    }

                    // Focus vào control đầu tiên có lỗi
                    var firstErrorControl = results
                        .SelectMany(r => r.MemberNames)
                        .Select(FindControlByPropertyName)
                        .FirstOrDefault(c => c != null);

                    if (firstErrorControl != null)
                    {
                        firstErrorControl.Focus();
                    }

                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi validate dữ liệu: {ex.Message}", "Lỗi", this);
                return false;
            }
        }

        /// <summary>
        /// Lấy EmployeeDto từ controls để validate
        /// </summary>
        /// <returns>EmployeeDto</returns>
        private EmployeeDto GetEmployeeDtoFromControls()
        {
            try
            {
                var dto = new EmployeeDto
                {
                    EmployeeCode = EmployeeCodeTextEdit.Text?.Trim(),
                    FullName = FullNameTextEdit.Text?.Trim(),
                    Gender = GenderComboBoxEdit.Text?.Trim(),
                    BirthDate = BirthDateDateEdit.EditValue as DateTime?,
                    Phone = PhoneTextEdit.Text?.Trim(),
                    Email = EmailTextEdit.Text?.Trim(),
                    HireDate = HireDateDateEdit.EditValue as DateTime?,
                    ResignDate = ResignDateDateEdit.EditValue as DateTime?,
                    Mobile = MobileTextEdit.Text?.Trim(),
                    IsActive = (bool)IsActiveToggleSwitch.EditValue,
                    Notes = NotesTextEdit.Text?.Trim(),
                    BranchId = _branchId,
                    DepartmentId = _departmentId,
                    PositionId = _positionId
                };

                // Set ID nếu đang edit
                if (_isEditMode)
                {
                    dto.Id = _employeeId;
                }

                return dto;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Lỗi get DTO từ controls: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Tìm control theo tên property trong DTO
        /// </summary>
        private Control FindControlByPropertyName(string propertyName)
        {
            return propertyName switch
            {
                nameof(EmployeeDto.EmployeeCode) => EmployeeCodeTextEdit,
                nameof(EmployeeDto.FullName) => FullNameTextEdit,
                nameof(EmployeeDto.Gender) => GenderComboBoxEdit,
                nameof(EmployeeDto.BirthDate) => BirthDateDateEdit,
                nameof(EmployeeDto.Phone) => PhoneTextEdit,
                nameof(EmployeeDto.Email) => EmailTextEdit,
                nameof(EmployeeDto.HireDate) => HireDateDateEdit,
                nameof(EmployeeDto.ResignDate) => ResignDateDateEdit,
                nameof(EmployeeDto.Mobile) => MobileTextEdit,
                nameof(EmployeeDto.IsActive) => IsActiveToggleSwitch,
                nameof(EmployeeDto.Notes) => NotesTextEdit,
                nameof(EmployeeDto.BranchId) => BranchIdSearchLookupEdit,
                nameof(EmployeeDto.DepartmentId) => DepartmentIdSearchLookupEdit,
                nameof(EmployeeDto.PositionId) => PositionIdSearchLookupEdit,
                _ => null
            };
        }

        /// <summary>
        /// Save employee data
        /// </summary>
        private async Task SaveEmployee()
        {
            try
            {
                var employee = GetEmployeeFromControls();

                if (_isEditMode)
                {
                    _employeeBll.SaveOrUpdate(employee);
                    ShowSuccess("Cập nhật nhân viên thành công", "Thành công", this);
                }
                else
                {
                    _employeeBll.SaveOrUpdate(employee);
                    ShowSuccess("Tạo mới nhân viên thành công", "Thành công", this);
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lưu nhân viên: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Get employee data from controls
        /// </summary>
        /// <returns>EmployeeDto</returns>
        private EmployeeDto GetEmployeeFromControls()
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
                var employee = new EmployeeDto
                {
                    EmployeeCode = EmployeeCodeTextEdit.Text?.Trim(),
                    FullName = FullNameTextEdit.Text?.Trim(),
                    Gender = GenderComboBoxEdit.Text?.Trim(),
                    BirthDate = BirthDateDateEdit.EditValue as DateTime?,
                    Phone = PhoneTextEdit.Text?.Trim(),
                    Email = EmailTextEdit.Text?.Trim(),
                    HireDate = HireDateDateEdit.EditValue as DateTime?,
                    ResignDate = ResignDateDateEdit.EditValue as DateTime?,
                    Mobile = MobileTextEdit.Text?.Trim(),
                    IsActive = (bool)IsActiveToggleSwitch.EditValue,
                    Notes = NotesTextEdit.Text?.Trim(),
                    CompanyId = companyId,
                    BranchId = _branchId,
                    DepartmentId = _departmentId,
                    PositionId = _positionId
                };

                // Xử lý Avatar (EmployeeDto.AvatarThumbnailData is byte[], not Binary)
                if (AvatarPictureEdit.Image != null)
                {
                    using var ms = new MemoryStream();
                    AvatarPictureEdit.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    employee.AvatarThumbnailData = ms.ToArray();
                }

                if (_isEditMode)
                {
                    employee.Id = _employeeId;
                    // Preserve CreatedDate from current employee if editing
                    if (_currentEmployee != null)
                    {
                        employee.CreatedDate = _currentEmployee.CreatedDate;
                    }
                }
                else
                {
                    employee.Id = Guid.NewGuid();
                    employee.CreatedDate = DateTime.Now;
                }

                return employee;
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
                    typeof(EmployeeDto),
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
        /// QUAN TRỌNG: Chỉ Mã nhân viên bị khóa khi edit, các control khác đều cho phép chỉnh sửa
        /// </summary>
        /// <param name="readOnly">True để disable (chế độ edit), False để enable (chế độ tạo mới)</param>
        private void SetControlsReadOnly(bool readOnly)
        {
            try
            {
                // Mã nhân viên - không cho phép thay đổi khi edit (duy nhất control bị khóa)
                EmployeeCodeTextEdit.Properties.ReadOnly = readOnly;
                EmployeeCodeTextEdit.Properties.AllowNullInput = readOnly 
                    ? DevExpress.Utils.DefaultBoolean.False 
                    : DevExpress.Utils.DefaultBoolean.True;
                EmployeeCodeTextEdit.Enabled = !readOnly;

                // Chi nhánh, Phòng ban, Chức vụ - CHO PHÉP chỉnh sửa trong chế độ edit
                // Không set ReadOnly hoặc Enabled = false cho các controls này
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
                SetupDateEditSuperTips();
                SetupComboBoxEditSuperTips();
                SetupToggleSwitchSuperTips();
                SetupMemoEditSuperTips();
                SetupPictureEditSuperTips();
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
            // SuperTip cho Mã nhân viên
            SuperToolTipHelper.SetTextEditSuperTip(
                EmployeeCodeTextEdit,
                title: @"<b><color=DarkBlue>🏷️ Mã nhân viên</color></b>",
                content: @"Nhập <b>mã nhân viên</b> trong hệ thống.<br/><br/><b>Chức năng:</b><br/>• Nhập mã nhân viên (ví dụ: NV001, NV002, v.v.)<br/>• Hiển thị mã nhân viên khi chỉnh sửa<br/>• Validation tự động khi rời khỏi control<br/><br/><b>Ràng buộc:</b><br/>• <b>Bắt buộc nhập</b> khi thêm mới (có dấu * đỏ)<br/>• Không được để trống khi tạo mới<br/>• Tối đa 50 ký tự<br/>• Tự động trim khoảng trắng đầu/cuối<br/>• Không thể chỉnh sửa khi đang ở chế độ edit<br/><br/><b>Validation:</b><br/>• Kiểm tra rỗng khi tạo mới<br/>• Kiểm tra độ dài tối đa (50 ký tự)<br/>• Hiển thị lỗi qua ErrorProvider nếu không hợp lệ<br/><br/><b>DataAnnotations:</b><br/>• Có attribute [Required] trong DTO<br/>• Có attribute [StringLength(50)]<br/>• Tự động đánh dấu * đỏ trong layout<br/>• Hiển thị prompt 'Bắt buộc nhập' khi rỗng<br/><br/><color=Gray>Lưu ý:</color> Mã nhân viên sẽ được lưu vào database khi click nút Lưu. Khi đang ở chế độ chỉnh sửa, mã nhân viên sẽ bị khóa và không thể thay đổi."
            );

            // SuperTip cho Họ và tên
            SuperToolTipHelper.SetTextEditSuperTip(
                FullNameTextEdit,
                title: @"<b><color=DarkBlue>👤 Họ và tên</color></b>",
                content: @"Nhập <b>họ và tên</b> của nhân viên.<br/><br/><b>Chức năng:</b><br/>• Nhập họ và tên đầy đủ của nhân viên<br/>• Hiển thị họ và tên khi chỉnh sửa<br/>• Validation tự động khi rời khỏi control<br/><br/><b>Ràng buộc:</b><br/>• <b>Bắt buộc nhập</b> (có dấu * đỏ)<br/>• Không được để trống<br/>• Tối đa 100 ký tự<br/>• Tự động trim khoảng trắng đầu/cuối<br/><br/><b>Validation:</b><br/>• Kiểm tra rỗng khi validating<br/>• Kiểm tra độ dài tối đa (100 ký tự)<br/>• Hiển thị lỗi qua ErrorProvider nếu không hợp lệ<br/><br/><b>DataAnnotations:</b><br/>• Có attribute [Required] trong DTO<br/>• Có attribute [StringLength(100)]<br/>• Tự động đánh dấu * đỏ trong layout<br/>• Hiển thị prompt 'Bắt buộc nhập' khi rỗng<br/><br/><color=Gray>Lưu ý:</color> Họ và tên sẽ được lưu vào database khi click nút Lưu."
            );

            // SuperTip cho Số điện thoại
            if (PhoneTextEdit != null)
            {
                SuperToolTipHelper.SetTextEditSuperTip(
                    PhoneTextEdit,
                    title: @"<b><color=DarkBlue>📞 Số điện thoại</color></b>",
                    content: @"Nhập <b>số điện thoại</b> của nhân viên (tùy chọn).<br/><br/><b>Chức năng:</b><br/>• Nhập số điện thoại liên hệ<br/>• Hiển thị số điện thoại khi chỉnh sửa<br/>• Validation tự động khi rời khỏi control<br/><br/><b>Ràng buộc:</b><br/>• Không bắt buộc (có thể để trống)<br/>• Tối đa 50 ký tự<br/>• Chỉ cho phép số, dấu +, dấu -, dấu cách, dấu ngoặc<br/>• Tự động trim khoảng trắng đầu/cuối<br/><br/><b>Validation:</b><br/>• Kiểm tra format nếu có nhập<br/>• Kiểm tra độ dài tối đa (50 ký tự)<br/>• Hiển thị lỗi qua ErrorProvider nếu không hợp lệ<br/><br/><b>DataAnnotations:</b><br/>• Có attribute [StringLength(50)]<br/>• Không có attribute [Required]<br/><br/><color=Gray>Lưu ý:</color> Số điện thoại sẽ được lưu vào database khi click nút Lưu."
                );
            }

            // SuperTip cho Email
            if (EmailTextEdit != null)
            {
                SuperToolTipHelper.SetTextEditSuperTip(
                    EmailTextEdit,
                    title: @"<b><color=DarkBlue>📧 Email</color></b>",
                    content: @"Nhập <b>email</b> của nhân viên (tùy chọn).<br/><br/><b>Chức năng:</b><br/>• Nhập địa chỉ email liên hệ<br/>• Hiển thị email khi chỉnh sửa<br/>• Validation tự động khi rời khỏi control<br/><br/><b>Ràng buộc:</b><br/>• Không bắt buộc (có thể để trống)<br/>• Tối đa 100 ký tự<br/>• Phải đúng định dạng email nếu có nhập<br/>• Tự động trim khoảng trắng đầu/cuối<br/><br/><b>Validation:</b><br/>• Kiểm tra format email nếu có nhập<br/>• Kiểm tra độ dài tối đa (100 ký tự)<br/>• Hiển thị lỗi qua ErrorProvider nếu không hợp lệ<br/><br/><b>DataAnnotations:</b><br/>• Có attribute [StringLength(100)]<br/>• Không có attribute [Required]<br/><br/><color=Gray>Lưu ý:</color> Email sẽ được lưu vào database khi click nút Lưu."
                );
            }

            // SuperTip cho Số điện thoại di động
            if (MobileTextEdit != null)
            {
                SuperToolTipHelper.SetTextEditSuperTip(
                    MobileTextEdit,
                    title: @"<b><color=DarkBlue>📱 Số điện thoại di động</color></b>",
                    content: @"Nhập <b>số điện thoại di động</b> của nhân viên (tùy chọn).<br/><br/><b>Chức năng:</b><br/>• Nhập số điện thoại di động liên hệ<br/>• Hiển thị số điện thoại di động khi chỉnh sửa<br/>• Validation tự động khi rời khỏi control<br/><br/><b>Ràng buộc:</b><br/>• Không bắt buộc (có thể để trống)<br/>• Tối đa 50 ký tự<br/>• Chỉ cho phép số, dấu +, dấu -, dấu cách, dấu ngoặc<br/>• Tự động trim khoảng trắng đầu/cuối<br/><br/><b>Validation:</b><br/>• Kiểm tra format nếu có nhập<br/>• Kiểm tra độ dài tối đa (50 ký tự)<br/>• Hiển thị lỗi qua ErrorProvider nếu không hợp lệ<br/><br/><b>DataAnnotations:</b><br/>• Có attribute [StringLength(50)]<br/>• Không có attribute [Required]<br/><br/><color=Gray>Lưu ý:</color> Số điện thoại di động sẽ được lưu vào database khi click nút Lưu."
                );
            }
        }

        /// <summary>
        /// Thiết lập SuperToolTip cho các DateEdit controls
        /// </summary>
        private void SetupDateEditSuperTips()
        {
            // SuperTip cho Ngày sinh
            if (BirthDateDateEdit != null)
            {
                SuperToolTipHelper.SetBaseEditSuperTip(
                    BirthDateDateEdit,
                    title: @"<b><color=DarkBlue>🎂 Ngày sinh</color></b>",
                    content: @"Chọn <b>ngày sinh</b> của nhân viên (tùy chọn).<br/><br/><b>Chức năng:</b><br/>• Chọn ngày sinh của nhân viên<br/>• Hiển thị ngày sinh khi chỉnh sửa<br/>• Validation tự động khi rời khỏi control<br/><br/><b>Ràng buộc:</b><br/>• Không bắt buộc (có thể để trống)<br/>• Có thể để trống nếu không biết ngày sinh<br/>• Phải là ngày hợp lệ nếu có chọn<br/><br/><b>Validation:</b><br/>• Không bắt buộc nhập<br/>• Kiểm tra ngày hợp lệ nếu có chọn<br/>• Hiển thị lỗi qua ErrorProvider nếu không hợp lệ<br/><br/><color=Gray>Lưu ý:</color> Ngày sinh sẽ được lưu vào database khi click nút Lưu."
                );
            }

            // SuperTip cho Ngày vào làm
            if (HireDateDateEdit != null)
            {
                SuperToolTipHelper.SetBaseEditSuperTip(
                    HireDateDateEdit,
                    title: @"<b><color=DarkBlue>📅 Ngày vào làm</color></b>",
                    content: @"Chọn <b>ngày vào làm</b> của nhân viên (tùy chọn).<br/><br/><b>Chức năng:</b><br/>• Chọn ngày nhân viên bắt đầu làm việc<br/>• Hiển thị ngày vào làm khi chỉnh sửa<br/>• Validation tự động khi rời khỏi control<br/><br/><b>Ràng buộc:</b><br/>• Không bắt buộc (có thể để trống)<br/>• Có thể để trống nếu chưa có ngày vào làm<br/>• Phải là ngày hợp lệ nếu có chọn<br/><br/><b>Validation:</b><br/>• Không bắt buộc nhập<br/>• Kiểm tra ngày hợp lệ nếu có chọn<br/>• Hiển thị lỗi qua ErrorProvider nếu không hợp lệ<br/><br/><color=Gray>Lưu ý:</color> Ngày vào làm sẽ được lưu vào database khi click nút Lưu."
                );
            }

            // SuperTip cho Ngày nghỉ việc
            if (ResignDateDateEdit != null)
            {
                SuperToolTipHelper.SetBaseEditSuperTip(
                    ResignDateDateEdit,
                    title: @"<b><color=DarkBlue>🚪 Ngày nghỉ việc</color></b>",
                    content: @"Chọn <b>ngày nghỉ việc</b> của nhân viên (tùy chọn).<br/><br/><b>Chức năng:</b><br/>• Chọn ngày nhân viên nghỉ việc<br/>• Hiển thị ngày nghỉ việc khi chỉnh sửa<br/>• Validation tự động khi rời khỏi control<br/><br/><b>Ràng buộc:</b><br/>• Không bắt buộc (có thể để trống)<br/>• Có thể để trống nếu nhân viên vẫn đang làm việc<br/>• Phải là ngày hợp lệ nếu có chọn<br/><br/><b>Validation:</b><br/>• Không bắt buộc nhập<br/>• Kiểm tra ngày hợp lệ nếu có chọn<br/>• Hiển thị lỗi qua ErrorProvider nếu không hợp lệ<br/><br/><color=Gray>Lưu ý:</color> Ngày nghỉ việc sẽ được lưu vào database khi click nút Lưu."
                );
            }
        }

        /// <summary>
        /// Thiết lập SuperToolTip cho các ComboBoxEdit controls
        /// </summary>
        private void SetupComboBoxEditSuperTips()
        {
            // SuperTip cho Giới tính
            if (GenderComboBoxEdit != null)
            {
                SuperToolTipHelper.SetBaseEditSuperTip(
                    GenderComboBoxEdit,
                    title: @"<b><color=DarkBlue>⚧️ Giới tính</color></b>",
                    content: @"Chọn <b>giới tính</b> của nhân viên (tùy chọn).<br/><br/><b>Chức năng:</b><br/>• Chọn giới tính từ danh sách (Nam, Nữ, Khác)<br/>• Hiển thị giới tính khi chỉnh sửa<br/>• Validation tự động khi rời khỏi control<br/><br/><b>Ràng buộc:</b><br/>• Không bắt buộc (có thể để trống)<br/>• Có thể để trống nếu không muốn cung cấp thông tin<br/>• Phải chọn một giá trị hợp lệ nếu có chọn<br/><br/><b>Validation:</b><br/>• Không bắt buộc nhập<br/>• Kiểm tra giá trị hợp lệ nếu có chọn<br/>• Hiển thị lỗi qua ErrorProvider nếu không hợp lệ<br/><br/><b>DataAnnotations:</b><br/>• Có attribute [StringLength(10)]<br/>• Không có attribute [Required]<br/><br/><color=Gray>Lưu ý:</color> Giới tính sẽ được lưu vào database khi click nút Lưu."
                );
            }
        }

        /// <summary>
        /// Thiết lập SuperToolTip cho các ToggleSwitch controls
        /// </summary>
        private void SetupToggleSwitchSuperTips()
        {
            // SuperTip cho Trạng thái hoạt động
            if (IsActiveToggleSwitch != null)
            {
                SuperToolTipHelper.SetBaseEditSuperTip(
                    IsActiveToggleSwitch,
                    title: @"<b><color=DarkBlue>✅ Trạng thái hoạt động</color></b>",
                    content: @"Chọn <b>trạng thái hoạt động</b> của nhân viên.<br/><br/><b>Chức năng:</b><br/>• Bật/tắt trạng thái hoạt động của nhân viên<br/>• Hiển thị trạng thái khi chỉnh sửa<br/>• Mặc định: Đang làm việc (true)<br/><br/><b>Ràng buộc:</b><br/>• <b>Bắt buộc chọn</b> (có dấu * đỏ)<br/>• Không được để trống<br/>• Mặc định: true (Đang làm việc)<br/><br/><b>Validation:</b><br/>• Kiểm tra giá trị không được null<br/>• Hiển thị lỗi qua ErrorProvider nếu không hợp lệ<br/><br/><b>DataAnnotations:</b><br/>• Có attribute [Required] trong DTO<br/>• Mặc định: true<br/><br/><color=Gray>Lưu ý:</color> Trạng thái hoạt động sẽ được lưu vào database khi click nút Lưu. Khi tắt, nhân viên sẽ không còn hoạt động trong hệ thống."
                );
            }
        }

        /// <summary>
        /// Thiết lập SuperToolTip cho các MemoEdit controls
        /// </summary>
        private void SetupMemoEditSuperTips()
        {
            // SuperTip cho Ghi chú
            if (NotesTextEdit != null)
            {
                SuperToolTipHelper.SetBaseEditSuperTip(
                    NotesTextEdit,
                    title: @"<b><color=DarkBlue>📝 Ghi chú</color></b>",
                    content: @"Nhập <b>ghi chú</b> hoặc mô tả bổ sung về nhân viên (tùy chọn).<br/><br/><b>Chức năng:</b><br/>• Lưu thông tin bổ sung về nhân viên<br/>• Ghi chú về kỹ năng, kinh nghiệm, v.v.<br/>• Hỗ trợ nhiều dòng văn bản<br/><br/><b>Ràng buộc:</b><br/>• Không bắt buộc (có thể để trống)<br/>• Tối đa 1000 ký tự<br/><br/><b>Validation:</b><br/>• Kiểm tra độ dài tối đa (1000 ký tự) nếu có nhập<br/>• Hiển thị lỗi qua ErrorProvider nếu vượt quá độ dài<br/><br/><b>DataAnnotations:</b><br/>• Có attribute [StringLength(1000)]<br/>• Không có attribute [Required]<br/><br/><color=Gray>Lưu ý:</color> Ghi chú sẽ được lưu vào database khi click nút Lưu."
                );
            }
        }

        /// <summary>
        /// Thiết lập SuperToolTip cho các PictureEdit controls
        /// </summary>
        private void SetupPictureEditSuperTips()
        {
            // SuperTip cho Ảnh đại diện
            if (AvatarPictureEdit != null)
            {
                SuperToolTipHelper.SetBaseEditSuperTip(
                    AvatarPictureEdit,
                    title: @"<b><color=DarkBlue>🖼️ Ảnh đại diện</color></b>",
                    content: @"Chọn hoặc upload <b>ảnh đại diện</b> cho nhân viên (tùy chọn).<br/><br/><b>Chức năng:</b><br/>• Upload ảnh đại diện từ file<br/>• Paste ảnh từ clipboard<br/>• Copy ảnh vào clipboard<br/>• Xóa ảnh đại diện<br/>• Hiển thị ảnh đại diện khi chỉnh sửa<br/><br/><b>Ràng buộc:</b><br/>• Không bắt buộc (có thể để trống)<br/>• Kích thước tối đa: 10MB<br/>• Định dạng hỗ trợ: JPG, PNG, GIF<br/><br/><b>Validation:</b><br/>• Kiểm tra kích thước file (tối đa 10MB)<br/>• Kiểm tra định dạng file (JPG, PNG, GIF)<br/>• Hiển thị lỗi qua ErrorProvider nếu không hợp lệ<br/><br/><b>Chức năng:</b><br/>• Load: Chọn file ảnh từ máy tính<br/>• Paste: Dán ảnh từ clipboard<br/>• Copy: Copy ảnh vào clipboard<br/>• Delete: Xóa ảnh đại diện<br/><br/><color=Gray>Lưu ý:</color> Ảnh đại diện sẽ được lưu vào database dưới dạng thumbnail khi click nút Lưu. Hệ thống sẽ tự động resize ảnh để tối ưu dung lượng."
                );
            }
        }

        /// <summary>
        /// Thiết lập SuperToolTip cho các SearchLookupEdit controls
        /// </summary>
        private void SetupSearchLookupSuperTips()
        {
            // SuperTip cho Chi nhánh
            SuperToolTipHelper.SetBaseEditSuperTip(
                BranchIdSearchLookupEdit,
                title: @"<b><color=DarkBlue>🏢 Chi nhánh</color></b>",
                content: @"Chọn <b>chi nhánh</b> mà nhân viên thuộc về (tùy chọn).<br/><br/><b>Chức năng:</b><br/>• Chọn chi nhánh từ danh sách dropdown<br/>• Hiển thị thông tin chi nhánh đã chọn<br/>• Validation tự động khi rời khỏi control<br/><br/><b>Ràng buộc:</b><br/>• <b>Không bắt buộc chọn</b> (có thể để trống)<br/>• Có thể để trống nếu nhân viên không thuộc chi nhánh cụ thể<br/>• Phải chọn một chi nhánh hợp lệ nếu có chọn<br/><br/><b>Validation:</b><br/>• Không bắt buộc nhập<br/>• Kiểm tra chi nhánh có tồn tại không nếu có chọn<br/>• Hiển thị lỗi qua ErrorProvider nếu không hợp lệ<br/><br/><color=Gray>Lưu ý:</color> Chi nhánh sẽ được lưu vào database khi click nút Lưu. Danh sách chi nhánh được load từ database và chỉ hiển thị các chi nhánh đang hoạt động."
            );

            // SuperTip cho Phòng ban
            SuperToolTipHelper.SetBaseEditSuperTip(
                DepartmentIdSearchLookupEdit,
                title: @"<b><color=DarkBlue>🏢 Phòng ban</color></b>",
                content: @"Chọn <b>phòng ban</b> mà nhân viên thuộc về (tùy chọn).<br/><br/><b>Chức năng:</b><br/>• Chọn phòng ban từ danh sách dropdown<br/>• Hiển thị thông tin phòng ban đã chọn<br/>• Validation tự động khi rời khỏi control<br/><br/><b>Ràng buộc:</b><br/>• <b>Không bắt buộc chọn</b> (có thể để trống)<br/>• Có thể để trống nếu nhân viên không thuộc phòng ban cụ thể<br/>• Phải chọn một phòng ban hợp lệ nếu có chọn<br/><br/><b>Validation:</b><br/>• Không bắt buộc nhập<br/>• Kiểm tra phòng ban có tồn tại không nếu có chọn<br/>• Hiển thị lỗi qua ErrorProvider nếu không hợp lệ<br/><br/><color=Gray>Lưu ý:</color> Phòng ban sẽ được lưu vào database khi click nút Lưu. Danh sách phòng ban được load từ database."
            );

            // SuperTip cho Chức vụ
            SuperToolTipHelper.SetBaseEditSuperTip(
                PositionIdSearchLookupEdit,
                title: @"<b><color=DarkBlue>💼 Chức vụ</color></b>",
                content: @"Chọn <b>chức vụ</b> của nhân viên (tùy chọn).<br/><br/><b>Chức năng:</b><br/>• Chọn chức vụ từ danh sách dropdown<br/>• Hiển thị thông tin chức vụ đã chọn<br/>• Validation tự động khi rời khỏi control<br/><br/><b>Ràng buộc:</b><br/>• <b>Không bắt buộc chọn</b> (có thể để trống)<br/>• Có thể để trống nếu nhân viên chưa có chức vụ<br/>• Phải chọn một chức vụ hợp lệ nếu có chọn<br/><br/><b>Validation:</b><br/>• Không bắt buộc nhập<br/>• Kiểm tra chức vụ có tồn tại không nếu có chọn<br/>• Hiển thị lỗi qua ErrorProvider nếu không hợp lệ<br/><br/><color=Gray>Lưu ý:</color> Chức vụ sẽ được lưu vào database khi click nút Lưu. Danh sách chức vụ được load từ database."
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
                content: @"Lưu <b>thông tin nhân viên</b> vào database.<br/><br/><b>Chức năng:</b><br/>• Validate tất cả dữ liệu đầu vào<br/>• Tạo hoặc cập nhật nhân viên trong database<br/>• Hiển thị thông báo thành công/thất bại<br/>• Đóng form sau khi lưu thành công<br/><br/><b>Quy trình:</b><br/>• Validate toàn bộ form (Mã nhân viên, Họ và tên, Email, v.v.)<br/>• Lấy dữ liệu từ form và tạo Employee Entity<br/>• Lấy CompanyId từ database (vì chỉ có 1 Company duy nhất)<br/>• Nếu chỉnh sửa: Cập nhật entity với ID hiện tại<br/>• Nếu thêm mới: Tạo entity mới với ID mới<br/>• Lưu vào database qua EmployeeBll<br/>• Hiển thị thông báo thành công<br/>• Đóng form<br/><br/><b>Yêu cầu:</b><br/>• Mã nhân viên phải không được để trống (khi tạo mới)<br/>• Họ và tên phải không được để trống<br/>• Email phải đúng định dạng (nếu có nhập)<br/>• Tất cả validation phải pass<br/><br/><b>Kết quả:</b><br/>• Nếu thành công: Hiển thị thông báo và đóng form<br/>• Nếu lỗi: Hiển thị thông báo lỗi, form vẫn mở để chỉnh sửa<br/><br/><color=Gray>Lưu ý:</color> Nếu có lỗi validation, form sẽ không đóng và bạn có thể sửa lại. Dữ liệu sẽ được lưu vào database sau khi tất cả validation pass."
            );

            // SuperTip cho nút Đóng
            SuperToolTipHelper.SetBarButtonSuperTip(
                CloseBarButtonItem,
                title: @"<b><color=DarkRed>❌ Đóng</color></b>",
                content: @"Đóng form <b>chi tiết nhân viên</b> mà không lưu thay đổi.<br/><br/><b>Chức năng:</b><br/>• Đóng form ngay lập tức<br/>• Không lưu dữ liệu đã nhập<br/>• Không ảnh hưởng đến database<br/>• Set DialogResult = Cancel<br/><br/><b>Phím tắt:</b><br/>• Escape: Đóng form<br/><br/><color=Gray>Lưu ý:</color> Tất cả dữ liệu đã nhập (Mã nhân viên, Họ và tên, Chi nhánh, v.v.) sẽ bị mất khi đóng form. Nếu muốn lưu, hãy click nút Lưu trước khi đóng."
            );
        }

        #endregion
    }
}
