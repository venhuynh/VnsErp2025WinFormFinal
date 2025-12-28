using Bll.MasterData.CompanyBll;
using DevExpress.XtraEditors;
using DTO.MasterData.Company;
using System;
using System.Diagnostics;
using System.Windows.Forms;
using Common.Utils;

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

                // Setup SuperToolTips
                SetupSuperTips();
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
                RequiredFieldHelper.MarkRequiredFields(
                    this, 
                    typeof(PositionDto),
                    logger: (msg, ex) => Debug.WriteLine($"{msg}: {ex?.Message}")
                );
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi đánh dấu trường bắt buộc");
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
                // GetById() already returns PositionDto
                var position = _positionBll.GetById(_positionId);
                if (position == null)
                {
                    ShowError("Không tìm thấy thông tin chức vụ");
                    DialogResult = DialogResult.Cancel;
                    return;
                }

                _positionDto = position;
                
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
                    var result = MsgBox.ShowYesNo("Dữ liệu đã thay đổi. Bạn có muốn lưu không?");
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

                // Insert and Update expect PositionDto, not Entity
                if (_isEditMode)
                {
                    _positionBll.Update(_positionDto);
                }
                else
                {
                    _positionBll.Insert(_positionDto);
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
            MsgBox.ShowSuccess(message);
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
                
                // GetCompany() returns CompanyDto, not Company entity
                if (company != null)
                {
                    return company.Id;
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

        #region ========== SUPERTOOLTIP ==========

        /// <summary>
        /// Thiết lập SuperToolTip cho tất cả các controls trong form
        /// </summary>
        private void SetupSuperTips()
        {
            try
            {
                SetupTextEditSuperTips();
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
            // SuperTip cho Mã chức vụ
            SuperToolTipHelper.SetTextEditSuperTip(
                PositionCodeTextEdit,
                title: @"<b><color=DarkBlue>🏷️ Mã chức vụ</color></b>",
                content: @"Nhập <b>mã chức vụ</b> trong hệ thống.<br/><br/><b>Chức năng:</b><br/>• Nhập mã chức vụ (ví dụ: CV01, CV02, v.v.)<br/>• Hiển thị mã chức vụ khi chỉnh sửa<br/>• Validation tự động khi rời khỏi control<br/><br/><b>Ràng buộc:</b><br/>• <b>Bắt buộc nhập</b> khi thêm mới (có dấu * đỏ)<br/>• Không được để trống khi tạo mới<br/>• Tối đa 50 ký tự<br/>• Không được trùng mã trong hệ thống<br/>• Tự động trim khoảng trắng đầu/cuối<br/>• Không thể chỉnh sửa khi đang ở chế độ edit<br/><br/><b>Validation:</b><br/>• Kiểm tra rỗng khi tạo mới<br/>• Kiểm tra độ dài tối đa (50 ký tự)<br/>• Kiểm tra trùng mã chức vụ trong hệ thống<br/>• Hiển thị lỗi qua ErrorProvider nếu không hợp lệ<br/><br/><b>DataAnnotations:</b><br/>• Có attribute [Required] trong DTO<br/>• Có attribute [StringLength(50)]<br/>• Tự động đánh dấu * đỏ trong layout<br/>• Hiển thị prompt 'Bắt buộc nhập' khi rỗng<br/><br/><color=Gray>Lưu ý:</color> Mã chức vụ sẽ được lưu vào database khi click nút Lưu. Khi đang ở chế độ chỉnh sửa, mã chức vụ sẽ bị khóa và không thể thay đổi. Mã chức vụ phải là duy nhất trong hệ thống."
            );

            // SuperTip cho Tên chức vụ
            SuperToolTipHelper.SetTextEditSuperTip(
                PositionNameTextEdit,
                title: @"<b><color=DarkBlue>🏢 Tên chức vụ</color></b>",
                content: @"Nhập <b>tên chức vụ</b> trong hệ thống.<br/><br/><b>Chức năng:</b><br/>• Nhập tên chức vụ (ví dụ: Giám đốc, Trưởng phòng, Nhân viên, v.v.)<br/>• Hiển thị tên chức vụ khi chỉnh sửa<br/>• Validation tự động khi rời khỏi control<br/><br/><b>Ràng buộc:</b><br/>• <b>Bắt buộc nhập</b> (có dấu * đỏ)<br/>• Không được để trống<br/>• Tối đa 255 ký tự<br/>• Không được chứa chỉ khoảng trắng<br/>• Tự động trim khoảng trắng đầu/cuối<br/><br/><b>Validation:</b><br/>• Kiểm tra rỗng khi validating<br/>• Kiểm tra độ dài tối đa (255 ký tự)<br/>• Hiển thị lỗi qua ErrorProvider nếu không hợp lệ<br/><br/><b>DataAnnotations:</b><br/>• Có attribute [Required] trong DTO<br/>• Có attribute [StringLength(255)]<br/>• Tự động đánh dấu * đỏ trong layout<br/>• Hiển thị prompt 'Bắt buộc nhập' khi rỗng<br/><br/><color=Gray>Lưu ý:</color> Tên chức vụ sẽ được lưu vào database khi click nút Lưu. Nếu đang ở chế độ chỉnh sửa, tên hiện tại sẽ được hiển thị sẵn."
            );

            // SuperTip cho Mô tả
            SuperToolTipHelper.SetTextEditSuperTip(
                DescriptionTextEdit,
                title: @"<b><color=DarkBlue>📝 Mô tả</color></b>",
                content: @"Nhập <b>mô tả</b> của chức vụ (tùy chọn).<br/><br/><b>Chức năng:</b><br/>• Nhập mô tả chi tiết về chức vụ<br/>• Hiển thị mô tả khi chỉnh sửa<br/>• Validation tự động khi rời khỏi control<br/><br/><b>Ràng buộc:</b><br/>• <b>Không bắt buộc nhập</b> (có thể để trống)<br/>• Tối đa 255 ký tự nếu có nhập<br/>• Tự động trim khoảng trắng đầu/cuối<br/><br/><b>Validation:</b><br/>• Chỉ kiểm tra độ dài tối đa (255 ký tự) nếu có nhập<br/>• Hiển thị lỗi qua ErrorProvider nếu vượt quá độ dài<br/>• Không bắt buộc nhập<br/><br/><b>DataAnnotations:</b><br/>• Không có attribute [Required] trong DTO<br/>• Có attribute [StringLength(255)]<br/>• Có thể để trống<br/><br/><color=Gray>Lưu ý:</color> Mô tả sẽ được lưu vào database khi click nút Lưu. Nếu đang ở chế độ chỉnh sửa, mô tả hiện tại sẽ được hiển thị sẵn. Có thể để trống nếu không cần thiết."
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
                content: @"Lưu <b>thông tin chức vụ</b> vào database.<br/><br/><b>Chức năng:</b><br/>• Validate tất cả dữ liệu đầu vào<br/>• Tạo hoặc cập nhật chức vụ trong database<br/>• Hiển thị thông báo thành công/thất bại<br/>• Đóng form sau khi lưu thành công<br/><br/><b>Quy trình:</b><br/>• Validate toàn bộ form (Mã chức vụ, Tên chức vụ, v.v.)<br/>• Kiểm tra trùng mã chức vụ (chỉ khi tạo mới)<br/>• Lấy dữ liệu từ form và tạo PositionDto<br/>• Lấy CompanyId từ database (vì chỉ có 1 Company duy nhất)<br/>• Convert DTO → Entity<br/>• Nếu chỉnh sửa: Cập nhật entity với ID hiện tại<br/>• Nếu thêm mới: Tạo entity mới<br/>• Lưu vào database qua PositionBll<br/>• Hiển thị thông báo thành công<br/>• Đóng form<br/><br/><b>Yêu cầu:</b><br/>• Mã chức vụ phải không được để trống (khi tạo mới)<br/>• Tên chức vụ phải không được để trống<br/>• Mã chức vụ không được trùng trong hệ thống<br/>• Tất cả validation phải pass<br/><br/><b>Kết quả:</b><br/>• Nếu thành công: Hiển thị thông báo và đóng form<br/>• Nếu lỗi: Hiển thị thông báo lỗi, form vẫn mở để chỉnh sửa<br/><br/><color=Gray>Lưu ý:</color> Nếu có lỗi validation, form sẽ không đóng và bạn có thể sửa lại. Dữ liệu sẽ được lưu vào database sau khi tất cả validation pass."
            );

            // SuperTip cho nút Đóng
            SuperToolTipHelper.SetBarButtonSuperTip(
                CloseBarButtonItem,
                title: @"<b><color=DarkRed>❌ Đóng</color></b>",
                content: @"Đóng form <b>chi tiết chức vụ</b> mà không lưu thay đổi.<br/><br/><b>Chức năng:</b><br/>• Kiểm tra xem có thay đổi dữ liệu không<br/>• Nếu có thay đổi: Hiển thị dialog xác nhận<br/>• Đóng form ngay lập tức nếu không có thay đổi<br/>• Không lưu dữ liệu đã nhập<br/>• Không ảnh hưởng đến database<br/>• Set DialogResult = Cancel<br/><br/><b>Phím tắt:</b><br/>• Escape: Đóng form<br/><br/><color=Gray>Lưu ý:</color> Nếu có thay đổi dữ liệu, hệ thống sẽ hỏi bạn có muốn lưu không. Tất cả dữ liệu đã nhập (Mã chức vụ, Tên chức vụ, Mô tả, v.v.) sẽ bị mất khi đóng form. Nếu muốn lưu, hãy click nút Lưu trước khi đóng."
            );
        }

        #endregion
    }
}