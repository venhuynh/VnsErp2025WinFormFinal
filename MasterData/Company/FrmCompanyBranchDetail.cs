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
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MasterData.Company
{
    public partial class FrmCompanyBranchDetail : XtraForm
    {
        #region ========== KHAI BÁO BIẾN ==========

        /// <summary>
        /// ID chi nhánh công ty (Guid.Empty cho thêm mới)
        /// </summary>
        private readonly Guid _companyBranchId;

        /// <summary>
        /// DTO chi nhánh công ty
        /// </summary>
        private CompanyBranchDto _companyBranchDto;

        /// <summary>
        /// BLL xử lý chi nhánh công ty
        /// </summary>
        private readonly CompanyBranchBll _companyBranchBll;

        /// <summary>
        /// Trạng thái form (true: chỉnh sửa, false: thêm mới)
        /// </summary>
        private bool _isEditMode => _companyBranchId != Guid.Empty;

        #endregion

        #region ========== CONSTRUCTOR ==========

        /// <summary>
        /// Khởi tạo form chi tiết chi nhánh công ty.
        /// </summary>
        /// <param name="companyBranchId">ID chi nhánh công ty (Guid.Empty cho thêm mới)</param>
        public FrmCompanyBranchDetail(Guid companyBranchId)
        {
            InitializeComponent();
            _companyBranchId = companyBranchId;
            _companyBranchBll = new CompanyBranchBll();
            
            InitializeForm();
        }

        /// <summary>
        /// Khởi tạo form chi tiết chi nhánh công ty (thêm mới).
        /// </summary>
        public FrmCompanyBranchDetail() : this(Guid.Empty)
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
                Text = _isEditMode ? "Chỉnh sửa chi nhánh công ty" : "Thêm mới chi nhánh công ty";

                // Khởi tạo DTO
                InitializeDto();

                // Thiết lập data binding
                SetupDataBinding();

                // Thiết lập validation
                SetupValidation();

                // Load dữ liệu nếu là chế độ chỉnh sửa
                if (_isEditMode)
                {
                    LoadData();
                }
                else
                {
                    // Set default values for new branch
                    IsActiveToggleSwitch.EditValue = true;
                }

                // Thiết lập sự kiện
                SetupEvents();

                // Setup advanced validation with DataAnnotations
                SetupAdvancedValidation();
            }
            catch (Exception ex)
            {
                MsgBox.ShowError($"Lỗi khởi tạo form: {ex.Message}");
            }
        }

        /// <summary>
        /// Khởi tạo DTO
        /// </summary>
        private void InitializeDto()
        {
            _companyBranchDto = new CompanyBranchDto();
            
            if (!_isEditMode)
            {
                // Lấy CompanyId từ database (vì chỉ có 1 Company duy nhất)
                var companyId = GetCompanyIdFromDatabase();
                if (companyId == Guid.Empty)
                {
                    MsgBox.ShowError("Không tìm thấy thông tin công ty trong hệ thống.");
                    Close();
                    return;
                }
                
                _companyBranchDto.CompanyId = companyId;
            }
        }

        /// <summary>
        /// Thiết lập data binding (đơn giản hóa)
        /// </summary>
        private void SetupDataBinding()
        {
            // Không cần data binding phức tạp, sử dụng manual binding trong GetDataFromControls()
        }

        /// <summary>
        /// Thiết lập validation (đơn giản hóa)
        /// </summary>
        private void SetupValidation()
        {
            // Validation được thực hiện trong ValidateForm() method
            // Không cần setup validation rules phức tạp ở đây
        }

        /// <summary>
        /// Thiết lập sự kiện
        /// </summary>
        private void SetupEvents()
        {
            // Sự kiện cho các button
            SaveBarButtonItem.ItemClick += SaveBarButtonItem_ItemClick;
            CloseBarButtonItem.ItemClick += CloseBarButtonItem_ItemClick;

            // Sự kiện cho form
            Load += FrmCompanyBranchDetail_Load;
            FormClosing += FrmCompanyBranchDetail_FormClosing;

            // Sự kiện auto code generation
            BranchNameTextEdit.EditValueChanged += BranchNameTextEdit_EditValueChanged;
        }

        #endregion

        #region ========== DATA OPERATIONS ==========

        /// <summary>
        /// Load dữ liệu chi nhánh công ty
        /// </summary>
        private void LoadData()
        {
            try
            {
                if (_companyBranchId == Guid.Empty)
                    return;

                var companyBranchEntity = _companyBranchBll.GetById(_companyBranchId);
                _companyBranchDto = companyBranchEntity?.ToDto();
                if (_companyBranchDto == null)
                {
                    MsgBox.ShowWarning("Không tìm thấy thông tin chi nhánh công ty.");
                    Close();
                    return;
                }

                // Bind data to controls
                BindDataToControls();
            }
            catch (Exception ex)
            {
                MsgBox.ShowError($"Lỗi load dữ liệu: {ex.Message}");
            }
        }


        /// <summary>
        /// Lưu dữ liệu (async với WaitForm)
        /// </summary>
        private async void SaveData()
        {
            try
            {
                // Validate form
                if (!ValidateForm())
                    return;

                using var splash = new WaitForm1();
                splash.Show();
                splash.Update();

                var success = await SaveDataAsync();

                if (success)
                {
                    MsgBox.ShowInfo(_isEditMode ? "Cập nhật chi nhánh công ty thành công!" : "Thêm mới chi nhánh công ty thành công!");
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MsgBox.ShowError(_isEditMode ? "Cập nhật chi nhánh công ty thất bại!" : "Thêm mới chi nhánh công ty thất bại!");
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowError($"Lỗi lưu dữ liệu: {ex.Message}");
            }
        }

        /// <summary>
        /// Lưu dữ liệu async
        /// </summary>
        private async Task<bool> SaveDataAsync()
        {
            try
            {
                // Lấy dữ liệu từ controls
                var branchDto = GetDataFromControls();
                
                // Chuyển đổi DTO sang Entity (tuân thủ quy tắc Dal -> Bll -> DTO)
                var branchEntity = branchDto.ToEntity();

                if (_isEditMode)
                {
                    await Task.Run(() => _companyBranchBll.Update(branchEntity));
                    return true;
                }
                else
                {
                    var newId = await Task.Run(() => _companyBranchBll.Insert(branchEntity));
                    return newId != Guid.Empty;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowError($"Lỗi lưu dữ liệu: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Thu thập dữ liệu từ các control để tạo DTO lưu xuống DB
        /// </summary>
        private CompanyBranchDto GetDataFromControls()
        {
            // Lấy CompanyId từ database hoặc từ DTO hiện tại
            var companyId = _isEditMode ? 
                (_companyBranchDto?.CompanyId ?? Guid.Empty) : 
                GetCompanyIdFromDatabase();
            
            if (companyId == Guid.Empty)
            {
                throw new InvalidOperationException("CompanyId không được để trống.");
            }

            return new CompanyBranchDto
            {
                Id = _companyBranchDto?.Id ?? Guid.Empty,
                CompanyId = companyId,
                BranchCode = BranchCodeTextEdit.Text?.Trim(),
                BranchName = BranchNameTextEdit.Text?.Trim(),
                Address = AddressTextEdit.Text?.Trim(),
                Phone = PhoneTextEdit.Text?.Trim(),
                Email = EmailTextEdit.Text?.Trim(),
                ManagerName = ManagerNameTextEdit.Text?.Trim(),
                IsActive = IsActiveToggleSwitch.EditValue != null ? (bool)IsActiveToggleSwitch.EditValue : true,
                CreatedDate = _companyBranchDto?.CreatedDate ?? DateTime.Now,
                ModifiedDate = DateTime.Now
            };
        }

        /// <summary>
        /// Bind dữ liệu chi nhánh vào các control
        /// </summary>
        private void BindDataToControls()
        {
            if (_companyBranchDto == null) return;

            // Bind data to controls
            BranchCodeTextEdit.EditValue = _companyBranchDto.BranchCode;
            BranchNameTextEdit.EditValue = _companyBranchDto.BranchName;
            AddressTextEdit.EditValue = _companyBranchDto.Address;
            PhoneTextEdit.EditValue = _companyBranchDto.Phone;
            EmailTextEdit.EditValue = _companyBranchDto.Email;
            ManagerNameTextEdit.EditValue = _companyBranchDto.ManagerName;
            IsActiveToggleSwitch.EditValue = _companyBranchDto.IsActive;
        }

        /// <summary>
        /// Cập nhật DTO từ form (legacy method - giữ lại để tương thích)
        /// </summary>
        private void UpdateDtoFromForm()
        {
            _companyBranchDto = GetDataFromControls();
        }

        #endregion

        #region ========== VALIDATION ==========

        /// <summary>
        /// Validate form với advanced validation
        /// </summary>
        /// <returns></returns>
        private bool ValidateForm()
        {
            try
            {
                // Clear previous errors
                dxErrorProvider1.ClearErrors();

                bool isValid = true;

                // Validate required fields
                if (string.IsNullOrWhiteSpace(BranchCodeTextEdit.Text))
                {
                    dxErrorProvider1.SetError(BranchCodeTextEdit, "Mã chi nhánh không được để trống", ErrorType.Critical);
                    BranchCodeTextEdit.Focus();
                    isValid = false;
                }

                if (string.IsNullOrWhiteSpace(BranchNameTextEdit.Text))
                {
                    dxErrorProvider1.SetError(BranchNameTextEdit, "Tên chi nhánh không được để trống", ErrorType.Critical);
                    BranchNameTextEdit.Focus();
                    isValid = false;
                }

                // Validate email format với regex
                if (!string.IsNullOrWhiteSpace(EmailTextEdit.Text) && 
                    !Regex.IsMatch(EmailTextEdit.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    dxErrorProvider1.SetError(EmailTextEdit, "Email không đúng định dạng", ErrorType.Warning);
                    EmailTextEdit.Focus();
                    isValid = false;
                }

                // Validate business rules
                if (!ValidateBusinessRules())
                {
                    isValid = false;
                }

                return isValid;
            }
            catch (Exception ex)
            {
                MsgBox.ShowError($"Lỗi validate form: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Validate business rules
        /// </summary>
        /// <returns></returns>
        private bool ValidateBusinessRules()
        {
            try
            {
                // Lấy CompanyId từ database hoặc từ DTO hiện tại
                var companyId = _isEditMode ? 
                    (_companyBranchDto?.CompanyId ?? Guid.Empty) : 
                    GetCompanyIdFromDatabase();
                    
                if (companyId == Guid.Empty)
                {
                    MsgBox.ShowError("ID công ty không hợp lệ.");
                    return false;
                }

                // Check duplicate branch code trong cùng company
                if (!string.IsNullOrWhiteSpace(BranchCodeTextEdit.Text))
                {
                    var existingBranch = _companyBranchBll.GetByBranchCode(BranchCodeTextEdit.Text.Trim());
                    if (existingBranch != null && existingBranch.Id != _companyBranchId)
                    {
                        // Kiểm tra xem có cùng company không
                        if (existingBranch.CompanyId == companyId)
                        {
                            dxErrorProvider1.SetError(BranchCodeTextEdit, "Mã chi nhánh đã tồn tại trong công ty này");
                            return false;
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MsgBox.ShowError($"Lỗi validate business rules: {ex.Message}");
                return false;
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
                MarkRequiredFields(typeof(CompanyBranchDto));
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

        /// <summary>
        /// Kiểm tra email hợp lệ
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region ========== EVENT HANDLERS ==========

        /// <summary>
        /// Sự kiện Load form
        /// </summary>
        private void FrmCompanyBranchDetail_Load(object sender, EventArgs e)
        {
            try
            {
                // Focus vào trường đầu tiên
                BranchCodeTextEdit.Focus();
            }
            catch (Exception ex)
            {
                MsgBox.ShowError($"Lỗi load form: {ex.Message}");
            }
        }

        /// <summary>
        /// Sự kiện FormClosing
        /// </summary>
        private void FrmCompanyBranchDetail_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                // Kiểm tra có thay đổi dữ liệu không
                if (HasDataChanged())
                {
                    var result = MsgBox.GetConfirmFromYesNoCancelDialog("Dữ liệu đã thay đổi. Bạn có muốn lưu không?", "Xác nhận");
                    if (result == DialogResult.Yes)
                    {
                        SaveData();
                        e.Cancel = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowError($"Lỗi đóng form: {ex.Message}");
            }
        }

        /// <summary>
        /// Sự kiện click Save button
        /// </summary>
        private void SaveBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                SaveData();
            }
            catch (Exception ex)
            {
                MsgBox.ShowError($"Lỗi lưu dữ liệu: {ex.Message}");
            }
        }

        /// <summary>
        /// Sự kiện click Close button
        /// </summary>
        private void CloseBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                Close();
            }
            catch (Exception ex)
            {
                MsgBox.ShowError($"Lỗi đóng form: {ex.Message}");
            }
        }


        /// <summary>
        /// Sự kiện thay đổi tên chi nhánh - tự động tạo mã chi nhánh
        /// </summary>
        private void BranchNameTextEdit_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                // Chỉ tự động tạo mã chi nhánh khi thêm mới (không phải edit)
                if (_isEditMode) return;

                // Lấy tên chi nhánh được nhập
                if (string.IsNullOrWhiteSpace(BranchNameTextEdit?.Text)) return;

                var branchName = BranchNameTextEdit.Text.Trim();

                // Tự động tạo mã chi nhánh từ tên
                var autoGeneratedCode = GenerateBranchCodeFromName(branchName);

                // Chỉ set nếu BranchCode đang trống hoặc chưa có giá trị
                if (string.IsNullOrWhiteSpace(BranchCodeTextEdit?.Text))
                {
                    if (BranchCodeTextEdit != null) BranchCodeTextEdit.EditValue = autoGeneratedCode;
                }
            }
            catch (Exception ex)
            {
                // Log lỗi nhưng không làm gián đoạn user experience
                Debug.WriteLine($"Lỗi tự động tạo mã chi nhánh: {ex.Message}");
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
        /// Kiểm tra dữ liệu có thay đổi không (đơn giản hóa)
        /// </summary>
        /// <returns></returns>
        private bool HasDataChanged()
        {
            try
            {
                // Chỉ kiểm tra khi đang edit mode
                if (!_isEditMode || _companyBranchDto == null)
                    return false;

                // So sánh với dữ liệu gốc
                var currentData = GetDataFromControls();
                return !string.Equals(_companyBranchDto.BranchCode, currentData.BranchCode) ||
                       !string.Equals(_companyBranchDto.BranchName, currentData.BranchName) ||
                       !string.Equals(_companyBranchDto.Address, currentData.Address) ||
                       !string.Equals(_companyBranchDto.Phone, currentData.Phone) ||
                       !string.Equals(_companyBranchDto.Email, currentData.Email) ||
                       !string.Equals(_companyBranchDto.ManagerName, currentData.ManagerName) ||
                       _companyBranchDto.IsActive != currentData.IsActive;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Lỗi kiểm tra thay đổi dữ liệu: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Tạo mã chi nhánh từ tên chi nhánh
        /// </summary>
        /// <param name="branchName">Tên chi nhánh</param>
        /// <returns>Mã chi nhánh được tạo</returns>
        private string GenerateBranchCodeFromName(string branchName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(branchName))
                    return string.Empty;

                // Loại bỏ dấu và ký tự đặc biệt
                var cleanName = RemoveVietnameseAccents(branchName);
                
                // Chuyển thành chữ hoa và loại bỏ khoảng trắng
                cleanName = cleanName.ToUpper().Replace(" ", "");

                // Lấy 6 ký tự đầu
                var baseCode = cleanName.Length > 6 ? cleanName.Substring(0, 6) : cleanName;

                // Tìm mã tiếp theo
                var nextCode = GenerateNextBranchCode(baseCode);

                return nextCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Lỗi tạo mã chi nhánh: {ex.Message}");
                return branchName.Substring(0, Math.Min(6, branchName.Length)).ToUpper();
            }
        }

        /// <summary>
        /// Loại bỏ dấu tiếng Việt
        /// </summary>
        /// <param name="text">Văn bản có dấu</param>
        /// <returns>Văn bản không dấu</returns>
        private string RemoveVietnameseAccents(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            var normalized = text.Normalize(System.Text.NormalizationForm.FormD);
            var result = new System.Text.StringBuilder();

            foreach (char c in normalized)
            {
                var category = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
                if (category != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    result.Append(c);
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Tạo mã chi nhánh tiếp theo
        /// </summary>
        /// <param name="baseCode">Mã cơ sở</param>
        /// <returns>Mã chi nhánh tiếp theo</returns>
        private string GenerateNextBranchCode(string baseCode)
        {
            try
            {
                // Lấy tất cả mã chi nhánh hiện có
                var existingCodes = GetExistingBranchCodes();

                // Tìm số thứ tự tiếp theo
                var nextNumber = 1;
                var pattern = baseCode;

                foreach (var existingCode in existingCodes)
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

                // Tạo mã mới với định dạng: BASE01, BASE02, etc.
                return $"{baseCode}{nextNumber:D2}";
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Lỗi tạo mã tiếp theo: {ex.Message}");
                return $"{baseCode}01";
            }
        }

        /// <summary>
        /// Lấy danh sách mã chi nhánh hiện có (trong Company duy nhất)
        /// </summary>
        /// <returns>Danh sách mã chi nhánh</returns>
        private List<string> GetExistingBranchCodes()
        {
            try
            {
                // Lấy tất cả chi nhánh trong Company duy nhất
                var companyId = GetCompanyIdFromDatabase();
                if (companyId == Guid.Empty)
                    return new List<string>();

                var allBranches = _companyBranchBll.GetByCompanyId(companyId);

                // Lọc các mã chi nhánh
                var branchCodes = allBranches
                    .Where(b => !string.IsNullOrWhiteSpace(b.BranchCode))
                    .Select(b => b.BranchCode)
                    .ToList();

                return branchCodes;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Lỗi lấy danh sách mã chi nhánh: {ex.Message}");
                return new List<string>();
            }
        }

        #endregion
    }
}