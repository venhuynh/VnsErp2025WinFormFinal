using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Bll.MasterData.ProductServiceBll;
using Common.Helpers;
using Common.Utils;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using MasterData.ProductService.Converters;
using MasterData.ProductService.Dto;

namespace MasterData.ProductService
{
    /// <summary>
    /// Form quản lý Đơn Vị Tính (UnitOfMeasure) dạng popup.
    /// Cung cấp chức năng CRUD đầy đủ với validation nghiệp vụ và giao diện thân thiện.
    /// </summary>
    public partial class FrmUnitOfMeasure : XtraForm
    {
        #region ========== KHAI BÁO BIẾN ==========

        /// <summary>
        /// Danh sách đơn vị tính để hiển thị trên grid
        /// </summary>
        private readonly BindingList<UnitOfMeasureDto> _units = new BindingList<UnitOfMeasureDto>();

        /// <summary>
        /// Đơn vị tính hiện tại đang được chỉnh sửa
        /// </summary>
        private UnitOfMeasureDto _current;

        /// <summary>
        /// Danh sách ID của các đơn vị tính được chọn trên grid
        /// </summary>
        private List<Guid> _selectedUnitIds = new List<Guid>();

        /// <summary>
        /// Trạng thái đang trong quá trình chỉnh sửa (true) hay không (false)
        /// </summary>
        private bool _isEditing;

        /// <summary>
        /// Business Logic Layer cho đơn vị tính
        /// </summary>
        private readonly UnitOfMeasureBll _unitBll = new UnitOfMeasureBll();

        /// <summary>
        /// Kết quả trả về cho form gọi (danh sách đơn vị tính)
        /// </summary>
        private IList<UnitOfMeasureDto> ResultUnits => _units.ToList();

        #endregion

        #region ========== CONSTRUCTOR & PUBLIC METHODS ==========

        /// <summary>
        /// Constructor mặc định
        /// </summary>
        public FrmUnitOfMeasure()
        {
            InitializeComponent();
            InitializeBehavior();
        }

        /// <summary>
        /// Constructor với dữ liệu khởi tạo
        /// </summary>
        /// <param name="initialUnits">Danh sách đơn vị tính ban đầu</param>
        public FrmUnitOfMeasure(IEnumerable<UnitOfMeasureDto> initialUnits) : this()
        {
            if (initialUnits != null)
            {
                foreach (var u in initialUnits)
                {
                    _units.Add(u);
                }
            }
        }

        /// <summary>
        /// Hiển thị form quản lý đơn vị tính dạng dialog
        /// </summary>
        /// <param name="owner">Form cha</param>
        /// <param name="initialUnits">Danh sách đơn vị tính ban đầu</param>
        /// <returns>Danh sách đơn vị tính sau khi chỉnh sửa</returns>
        public static IList<UnitOfMeasureDto> ShowManage(IWin32Window owner, IEnumerable<UnitOfMeasureDto> initialUnits)
        {
            using (var frm = new FrmUnitOfMeasure(initialUnits))
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog(owner);
                return frm.ResultUnits;
            }
        }

        /// <summary>
        /// API: Thêm mới đơn vị tính từ code bên ngoài
        /// </summary>
        /// <param name="code">Mã đơn vị tính</param>
        /// <param name="name">Tên đơn vị tính</param>
        /// <param name="description">Mô tả</param>
        /// <param name="isActive">Trạng thái hoạt động</param>
        /// <returns>Đối tượng UnitOfMeasureDto đã tạo</returns>
        public UnitOfMeasureDto AddNew(string code = "", string name = "", string description = "",
            bool isActive = true)
        {
            var dto = new UnitOfMeasureDto
            {
                Code = code,
                Name = name,
                Description = description,
                IsActive = isActive
            };
            _units.Add(dto);
            _current = dto;
            return dto;
        }

        #endregion

        #region ========== KHỞI TẠO FORM ==========

        /// <summary>
        /// Khởi tạo các thành phần và sự kiện của form
        /// </summary>
        private void InitializeBehavior()
        {
            // Thiết lập binding source cho grid
            unitOfMeasureDtoBindingSource.DataSource = _units;

            // Đăng ký sự kiện cho grid
            RegisterGridEvents();

            // Đăng ký sự kiện cho toolbar
            RegisterToolbarEvents();

            // Thiết lập phím tắt
            SetupKeyboardShortcuts();

            // Tạo bản ghi mặc định nếu danh sách trống
            EnsureDefaultRecord();

            // Cấu hình grid và trạng thái nút
            ConfigureGrid();
            UpdateButtonStates();

            // Đánh dấu các trường bắt buộc theo DataAnnotations của DTO
            RequiredFieldHelper.MarkRequiredFields(this, typeof(UnitOfMeasureDto));

            // Thiết lập SuperToolTip cho các controls
            SetupSuperToolTips();

            // Làm mới dữ liệu khi form hiển thị
            RefreshData();
        }

        /// <summary>
        /// Đăng ký sự kiện cho grid
        /// </summary>
        private void RegisterGridEvents()
        {
            AttributeGridView.SelectionChanged += UnitGridView_SelectionChanged;
            AttributeGridView.CustomDrawRowIndicator += UnitGridView_CustomDrawRowIndicator;
            AttributeGridView.DoubleClick += (s, e) => BeginEditSelected();
        }

        /// <summary>
        /// Đăng ký sự kiện cho toolbar
        /// </summary>
        private void RegisterToolbarEvents()
        {
            SaveBarButtonItem.ItemClick += SaveBarButtonItem_ItemClick;
            EditBarButtonItem.ItemClick += EditBarButtonItem_ItemClick;
            DeleteBarButtonItem.ItemClick += DeleteBarButtonItem_ItemClick;
            RefreshBarButtonItem.ItemClick += RefreshBarButtonItem_ItemClick;
            AddNewBarButtonItem.ItemClick += AddNewBarButtonItem_ItemClick;
        }

        /// <summary>
        /// Thiết lập phím tắt
        /// </summary>
        private void SetupKeyboardShortcuts()
        {
            KeyPreview = true;
            KeyDown += FrmUnitOfMeasure_KeyDown;
        }

        /// <summary>
        /// Đảm bảo có ít nhất 1 bản ghi mặc định
        /// </summary>
        private void EnsureDefaultRecord()
        {
            if (_units.Count == 0)
            {
                _units.Add(new UnitOfMeasureDto());
            }
        }

        /// <summary>
        /// Cấu hình hiển thị grid
        /// </summary>
        private void ConfigureGrid()
        {
            try
            {
                AttributeGridView.IndicatorWidth = 50;
                AttributeGridView.OptionsView.ShowAutoFilterRow = true;
                AttributeGridView.OptionsView.ShowGroupPanel = false;
                
                // Cấu hình cột Mô tả để hiển thị đầy đủ nội dung với xuống dòng
                if (AttributeGridView.Columns["colDescription1"] != null)
                {
                    AttributeGridView.Columns["colDescription1"].OptionsColumn.AllowSort = DefaultBoolean.True;
                    AttributeGridView.Columns["colDescription1"].OptionsColumn.FixedWidth = false;
                    AttributeGridView.Columns["colDescription1"].Width = 200; // Đặt chiều rộng phù hợp
                }
                
                AttributeGridView.BestFitColumns();
            }
            catch
            {
                // Bỏ qua lỗi cấu hình grid
            }
        }

        #endregion

        #region ========== SỰ KIỆN GRID ==========

        /// <summary>
        /// Xử lý khi thay đổi lựa chọn trên grid
        /// </summary>
        private void UnitGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                _selectedUnitIds = GridViewHelper.GetSelectedRowColumnValues<Guid>(sender, nameof(UnitOfMeasureDto.Id));
                UpdateButtonStates();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        /// <summary>
        /// Vẽ chỉ số dòng cho grid
        /// </summary>
        private void UnitGridView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                GridViewHelper.CustomDrawRowIndicator(AttributeGridView, e);
            }
            catch
            {
                // Bỏ qua lỗi vẽ chỉ số dòng
            }
        }

        #endregion

        #region ========== SỰ KIỆN TOOLBAR ==========

        /// <summary>
        /// Xử lý khi nhấn nút Lưu
        /// </summary>
        private void SaveBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            SaveCurrent();
        }

        /// <summary>
        /// Xử lý khi nhấn nút Điều chỉnh
        /// </summary>
        private void EditBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            BeginEditSelected();
        }

        /// <summary>
        /// Xử lý khi nhấn nút Xóa
        /// </summary>
        private void DeleteBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DeleteSelected();
        }

        /// <summary>
        /// Xử lý khi nhấn nút Thêm mới
        /// </summary>
        private void AddNewBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            AddNewUnit();
        }

        /// <summary>
        /// Xử lý khi nhấn nút Làm mới
        /// </summary>
        private void RefreshBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            RefreshData();
        }

        /// <summary>
        /// Xử lý phím tắt
        /// </summary>
        private void FrmUnitOfMeasure_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                SaveCurrent();
                e.Handled = true;
            }
            else if (e.Control && e.KeyCode == Keys.N)
            {
                AddNewUnit();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Escape && _isEditing)
            {
                CancelEdit();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Delete)
            {
                DeleteSelected();
                e.Handled = true;
            }
        }

        #endregion

        #region ========== CHỨC NĂNG CHỈNH SỬA ==========

        /// <summary>
        /// Bắt đầu chỉnh sửa đơn vị tính được chọn
        /// </summary>
        private void BeginEditSelected()
        {
            // Kiểm tra số lượng dòng được chọn
            if (_selectedUnitIds == null || _selectedUnitIds.Count == 0)
            {
                ShowInfo("Vui lòng chọn một dòng để chỉnh sửa.");
                return;
            }

            if (_selectedUnitIds.Count > 1)
            {
                ShowInfo("Chỉ cho phép chỉnh sửa 1 dòng. Vui lòng bỏ chọn bớt.");
                return;
            }

            // Tìm đối tượng được chọn
            var selectedDto = FindSelectedUnit();
            if (selectedDto == null)
            {
                ShowInfo("Không thể xác định dòng được chọn để chỉnh sửa.");
                return;
            }

            // Kiểm tra phụ thuộc dữ liệu trước khi cho phép edit
            if (selectedDto.Id != Guid.Empty && _unitBll.HasDependencies(selectedDto.Id))
            {
                ShowWarning($"Không thể chỉnh sửa '{selectedDto.Name}' vì còn dữ liệu phụ thuộc. Việc sửa đổi có thể ảnh hưởng đến tính toàn vẹn dữ liệu.");
                return;
            }

            // Thiết lập trạng thái chỉnh sửa
            _current = selectedDto;
            LoadDataToEditors();
            SetEditingMode();
        }

        /// <summary>
        /// Tìm đơn vị tính được chọn từ danh sách
        /// </summary>
        /// <returns>Đối tượng UnitOfMeasureDto được chọn hoặc null</returns>
        private UnitOfMeasureDto FindSelectedUnit()
        {
            var id = _selectedUnitIds[0];

            // Ưu tiên lấy từ FocusedRow nếu khớp Id
            if (AttributeGridView.GetFocusedRow() is UnitOfMeasureDto focused && focused.Id == id)
            {
                return focused;
            }

            // Tìm đúng DTO trong datasource theo Id
            if (unitOfMeasureDtoBindingSource.DataSource is IEnumerable<UnitOfMeasureDto> list)
            {
                foreach (var item in list)
                {
                    if (item.Id == id)
                    {
                        return item;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Nạp dữ liệu từ đối tượng hiện tại vào các editor
        /// </summary>
        private void LoadDataToEditors()
        {
            CodeTextEdit.EditValue = _current.Code;
            NameTextEdit.EditValue = _current.Name;
            DescriptionTextEdit.EditValue = _current.Description;
            IsActiveCheckEdit.EditValue = _current.IsActive;

            // Focus vào trường mã và chọn toàn bộ text
            CodeTextEdit.Focus();
            CodeTextEdit.SelectAll();
        }

        /// <summary>
        /// Thiết lập trạng thái chỉnh sửa
        /// </summary>
        private void SetEditingMode()
        {
            _isEditing = true;
            UpdateButtonStates();
        }

        /// <summary>
        /// Hủy bỏ quá trình chỉnh sửa
        /// </summary>
        private void CancelEdit()
        {
            try
            {
                _isEditing = false;
                _current = null;
                ClearValidation();
                ClearEditors();
                UpdateButtonStates();
                ShowInfo("Đã hủy chỉnh sửa");
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi hủy chỉnh sửa");
            }
        }

        #endregion

        #region ========== CHỨC NĂNG LƯU DỮ LIỆU ==========

        /// <summary>
        /// Lưu dữ liệu hiện tại
        /// </summary>
        private void SaveCurrent()
        {
            // Kiểm tra validation
            ClearValidation();
            if (!ValidateCurrent()) return;

            // Kiểm tra có đối tượng để lưu
            if (_current == null)
            {
                ShowInfo("Vui lòng chọn 1 dòng và bấm Điều chỉnh trước khi lưu.");
                return;
            }

            try
            {
                // Chuyển dữ liệu từ editors vào đối tượng hiện tại
                BindDataFromEditors();

                // Kiểm tra trùng lặp và lưu dữ liệu
                if (ValidateAndSaveData())
                {
                    // Cập nhật trạng thái sau khi lưu thành công
                    _isEditing = false;
                    ShowInfo("Đã lưu đơn vị tính");

                    // Làm mới dữ liệu và clear selection
                    RefreshBarButtonItem.PerformClick();
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi lưu đơn vị tính");
            }
        }

        /// <summary>
        /// Chuyển dữ liệu từ editors vào đối tượng hiện tại
        /// </summary>
        private void BindDataFromEditors()
        {
            if (_current != null)
            {
                _current.Code = CodeTextEdit.EditValue?.ToString().Trim() ?? string.Empty;
                _current.Name = NameTextEdit.EditValue?.ToString().Trim() ?? string.Empty;
                _current.Description = DescriptionTextEdit.EditValue?.ToString().Trim() ?? string.Empty;
                _current.IsActive = IsActiveCheckEdit.EditValue as bool? ?? true;
            }
        }

        /// <summary>
        /// Kiểm tra trùng lặp và lưu dữ liệu
        /// </summary>
        /// <returns>True nếu lưu thành công</returns>
        private bool ValidateAndSaveData()
        {
            // Clear validation errors trước khi kiểm tra duplicate
            ClearValidationErrors();

            // Lấy giá trị từ editors để kiểm tra trùng lặp
            var codeToCheck = CodeTextEdit.EditValue?.ToString().Trim() ?? string.Empty;
            var nameToCheck = NameTextEdit.EditValue?.ToString().Trim() ?? string.Empty;

            // Kiểm tra trùng mã
            if (_unitBll.IsCodeExists(codeToCheck, _current.Id == Guid.Empty ? null : (Guid?)_current.Id))
            {
                dxErrorProvider1.SetError(CodeTextEdit, "Mã đơn vị tính đã tồn tại");
                ShowWarning("Mã đơn vị tính đã tồn tại. Vui lòng chọn mã khác.");
                return false;
            }

            // Kiểm tra trùng tên
            if (_unitBll.IsNameExists(nameToCheck, _current.Id == Guid.Empty ? null : (Guid?)_current.Id))
            {
                dxErrorProvider1.SetError(NameTextEdit, "Tên đơn vị tính đã tồn tại");
                ShowWarning("Tên đơn vị tính đã tồn tại. Vui lòng chọn tên khác.");
                return false;
            }

            // Lưu xuống database qua BLL
            var entity = _current.ToEntity();
            _unitBll.SaveOrUpdate(entity);
            _current.Id = entity.Id; // Cập nhật ID nếu là bản ghi mới

            AttributeGridView.RefreshData();
            return true;
        }

        #endregion

        #region ========== VALIDATION ==========

        /// <summary>
        /// Kiểm tra tính hợp lệ của dữ liệu hiện tại
        /// </summary>
        /// <returns>True nếu dữ liệu hợp lệ</returns>
        private bool ValidateCurrent()
        {
            if (_current == null)
                return false;

            // Xóa các lỗi validation trước đó
            ClearValidation();

            // Lấy giá trị từ editors (không phải từ _current object)
            var code = CodeTextEdit.EditValue?.ToString().Trim() ?? string.Empty;
            var name = NameTextEdit.EditValue?.ToString().Trim() ?? string.Empty;
            var description = DescriptionTextEdit.EditValue?.ToString().Trim() ?? string.Empty;

            var errors = new List<string>();
            bool isValid = true;

            // Validate mã đơn vị
            if (!ValidateCode(code, errors))
                isValid = false;

            // Validate tên đơn vị
            if (!ValidateName(name, errors))
                isValid = false;

            // Validate mô tả
            if (!ValidateDescription(description, errors))
                isValid = false;

            // Hiển thị thông báo lỗi nếu có
            if (!isValid)
            {
                ShowWarning("Dữ liệu không hợp lệ:\n" + string.Join("\n", errors));
            }

            return isValid;
        }

        /// <summary>
        /// Validate mã đơn vị tính
        /// </summary>
        /// <param name="code">Mã đơn vị tính</param>
        /// <param name="errors">Danh sách lỗi</param>
        /// <returns>True nếu hợp lệ</returns>
        private bool ValidateCode(string code, List<string> errors)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                errors.Add("Mã đơn vị tính không được để trống");
                dxErrorProvider1.SetError(CodeTextEdit, "Mã đơn vị tính không được để trống");
                return false;
            }

            if (code.Contains(" "))
            {
                errors.Add("Mã đơn vị tính không được chứa khoảng trắng");
                dxErrorProvider1.SetError(CodeTextEdit, "Mã đơn vị tính không được chứa khoảng trắng");
                return false;
            }

            if (code.Length > 20)
            {
                errors.Add("Mã đơn vị tính không được vượt quá 20 ký tự");
                dxErrorProvider1.SetError(CodeTextEdit, "Mã đơn vị tính không được vượt quá 20 ký tự");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validate tên đơn vị tính
        /// </summary>
        /// <param name="name">Tên đơn vị tính</param>
        /// <param name="errors">Danh sách lỗi</param>
        /// <returns>True nếu hợp lệ</returns>
        private bool ValidateName(string name, List<string> errors)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                errors.Add("Tên đơn vị tính không được để trống");
                dxErrorProvider1.SetError(NameTextEdit, "Tên đơn vị tính không được để trống");
                return false;
            }

            if (name.Length > 100)
            {
                errors.Add("Tên đơn vị tính không được vượt quá 100 ký tự");
                dxErrorProvider1.SetError(NameTextEdit, "Tên đơn vị tính không được vượt quá 100 ký tự");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validate mô tả
        /// </summary>
        /// <param name="description">Mô tả</param>
        /// <param name="errors">Danh sách lỗi</param>
        /// <returns>True nếu hợp lệ</returns>
        private bool ValidateDescription(string description, List<string> errors)
        {
            if (!string.IsNullOrWhiteSpace(description) && description.Length > 255)
            {
                errors.Add("Mô tả không được vượt quá 255 ký tự");
                dxErrorProvider1.SetError(DescriptionTextEdit, "Mô tả không được vượt quá 255 ký tự");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Xóa tất cả lỗi validation
        /// </summary>
        private void ClearValidation()
        {
            dxErrorProvider1.SetError(CodeTextEdit, string.Empty);
            dxErrorProvider1.SetError(NameTextEdit, string.Empty);
            dxErrorProvider1.SetError(DescriptionTextEdit, string.Empty);
            dxErrorProvider1.SetError(IsActiveCheckEdit, string.Empty);
        }

        /// <summary>
        /// Xóa lỗi validation cho các trường chính
        /// </summary>
        private void ClearValidationErrors()
        {
            dxErrorProvider1.SetError(CodeTextEdit, string.Empty);
            dxErrorProvider1.SetError(NameTextEdit, string.Empty);
        }

        #endregion

        #region ========== CHỨC NĂNG XÓA ==========

        /// <summary>
        /// Xóa các đơn vị tính được chọn
        /// </summary>
        private void DeleteSelected()
        {
            var count = _selectedUnitIds?.Count ?? 0;
            if (count == 0)
            {
                ShowInfo("Vui lòng chọn ít nhất 1 dòng để xóa.");
                return;
            }

            // Xác nhận xóa
            var confirmMsg = count == 1
                ? "Bạn có chắc muốn xóa đơn vị tính đã chọn?"
                : $"Bạn có chắc muốn xóa {count} đơn vị tính đã chọn?";
            if (!MsgBox.ShowYesNo(confirmMsg)) return;

            try
            {
                // Thu thập các DTO được chọn để xóa
                var toDelete = CollectSelectedUnits();

                // Xóa từng đơn vị tính
                foreach (var dto in toDelete)
                {
                    if (ValidateAndDeleteUnit(dto))
                    {
                        _unitBll.Delete(dto.Id);
                        _units.Remove(dto);
                    }
                }

                UpdateButtonStates();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi xóa đơn vị tính");
                return;
            }

            // Đảm bảo có ít nhất 1 bản ghi
            EnsureAtLeastOneRecord();
            ClearSelectionAndRefresh();
        }

        /// <summary>
        /// Thu thập các đơn vị tính được chọn
        /// </summary>
        /// <returns>Danh sách đơn vị tính cần xóa</returns>
        private List<UnitOfMeasureDto> CollectSelectedUnits()
        {
            var toDelete = new List<UnitOfMeasureDto>();
            if (unitOfMeasureDtoBindingSource.DataSource is IEnumerable<UnitOfMeasureDto> list)
            {
                foreach (var item in list)
                {
                    if (_selectedUnitIds.Contains(item.Id))
                        toDelete.Add(item);
                }
            }

            return toDelete;
        }

        /// <summary>
        /// Kiểm tra và xóa đơn vị tính
        /// </summary>
        /// <param name="dto">Đơn vị tính cần xóa</param>
        /// <returns>True nếu có thể xóa</returns>
        private bool ValidateAndDeleteUnit(UnitOfMeasureDto dto)
        {
            // Chặn xóa khi còn dữ liệu phụ thuộc
            if (_unitBll.HasDependencies(dto.Id))
            {
                ShowWarning($"Không thể xóa '{dto.Name}' vì còn dữ liệu phụ thuộc.");
                return false;
            }

            // Nếu đang edit record bị xóa thì cancel edit
            if (_current != null && _current.Id == dto.Id)
            {
                _isEditing = false;
                _current = null;
            }

            return true;
        }

        /// <summary>
        /// Đảm bảo có ít nhất 1 bản ghi
        /// </summary>
        private void EnsureAtLeastOneRecord()
        {
            if (_units.Count == 0)
            {
                _units.Add(new UnitOfMeasureDto());
            }
        }

        /// <summary>
        /// Xóa selection và làm mới grid
        /// </summary>
        private void ClearSelectionAndRefresh()
        {
            ClearSelectionState();
            AttributeGridView.FocusedRowHandle = Math.Max(0, _units.Count - 1);
            AttributeGridView.RefreshData();
        }

        #endregion

        #region ========== CHỨC NĂNG THÊM MỚI ==========

        /// <summary>
        /// Tạo mới đơn vị tính
        /// </summary>
        private void AddNewUnit()
        {
            try
            {
                // Xóa selection và validation hiện tại
                ClearSelectionState();
                ClearValidation();
                _current = null;

                // Tạo đơn vị tính mới
                var newUnit = new UnitOfMeasureDto();
                _units.Add(newUnit);
                _current = newUnit;

                // Xóa các editor để nhập dữ liệu mới
                ClearEditors();
                SetDefaultValues();

                // Focus vào trường mã và chọn toàn bộ text
                CodeTextEdit.Focus();
                CodeTextEdit.SelectAll();

                // Thiết lập trạng thái chỉnh sửa
                _isEditing = true;
                UpdateButtonStates();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi tạo mới đơn vị tính");
            }
        }

        /// <summary>
        /// Xóa dữ liệu trong các editor
        /// </summary>
        private void ClearEditors()
        {
            CodeTextEdit.EditValue = null;
            NameTextEdit.EditValue = null;
            DescriptionTextEdit.EditValue = null;
            IsActiveCheckEdit.EditValue = null;
        }

        /// <summary>
        /// Thiết lập giá trị mặc định cho editor
        /// </summary>
        private void SetDefaultValues()
        {
            IsActiveCheckEdit.EditValue = true; // Mặc định là hoạt động
        }

        #endregion

        #region ========== QUẢN LÝ DỮ LIỆU ==========

        /// <summary>
        /// Làm mới dữ liệu
        /// </summary>
        private void RefreshData()
        {
            try
            {
                // Xóa selection và trạng thái hiện tại
                ClearSelectionState();
                ResetEditingState();

                // Xóa validation và editor values
                ClearValidation();
                ClearEditors();

                // Tải lại dữ liệu từ database
                LoadData();

                // Đảm bảo có ít nhất 1 bản ghi trống để tạo mới
                EnsureAtLeastOneEmptyRecord();

                UpdateButtonStates();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi làm mới dữ liệu");
            }
        }

        /// <summary>
        /// Reset trạng thái chỉnh sửa
        /// </summary>
        private void ResetEditingState()
        {
            _current = null;
            _isEditing = false;
        }

        /// <summary>
        /// Tải dữ liệu từ database và hiển thị lên grid
        /// </summary>
        public void LoadData()
        {
            try
            {
                var entities = _unitBll.GetAll();
                var dtos = entities.ToDtoList();
                SetData(dtos);
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi tải danh sách đơn vị tính");
            }
        }

        /// <summary>
        /// Thiết lập dữ liệu cho form (API từ form gọi)
        /// </summary>
        /// <param name="units">Danh sách đơn vị tính</param>
        private void SetData(IEnumerable<UnitOfMeasureDto> units)
        {
            _units.Clear();
            if (units != null)
            {
                foreach (var u in units)
                {
                    _units.Add(u);
                }
            }

            EnsureAtLeastOneEmptyRecord();
            unitOfMeasureDtoBindingSource.ResetBindings(false);
        }

        /// <summary>
        /// Đảm bảo có ít nhất 1 bản ghi trống để tạo mới
        /// </summary>
        private void EnsureAtLeastOneEmptyRecord()
        {
            if (_units.Count == 0)
            {
                _units.Add(new UnitOfMeasureDto());
            }
        }

        /// <summary>
        /// Xóa trạng thái selection
        /// </summary>
        private void ClearSelectionState()
        {
            _selectedUnitIds.Clear();
            AttributeGridView.ClearSelection();
            // Note: Không set _isEditing = false ở đây vì có thể đang trong quá trình edit
        }

        #endregion

        #region ========== QUẢN LÝ TRẠNG THÁI NÚT ==========

        /// <summary>
        /// Cập nhật trạng thái enabled/disabled của các nút trên toolbar
        /// </summary>
        private void UpdateButtonStates()
        {
            try
            {
                var selectedCount = _selectedUnitIds?.Count ?? 0;

                // Nút Điều chỉnh: enabled khi chọn đúng 1 dòng và không đang edit
                if (EditBarButtonItem != null)
                    EditBarButtonItem.Enabled = selectedCount == 1 && !_isEditing;

                // Nút Xóa: enabled khi chọn ít nhất 1 dòng và không đang edit
                if (DeleteBarButtonItem != null)
                    DeleteBarButtonItem.Enabled = selectedCount >= 1 && !_isEditing;

                // Nút Lưu: enabled khi đang edit và có _current
                if (SaveBarButtonItem != null)
                    SaveBarButtonItem.Enabled = _isEditing && _current != null;

                // Nút Thêm mới: enabled khi không đang edit
                if (AddNewBarButtonItem != null)
                    AddNewBarButtonItem.Enabled = !_isEditing;
            }
            catch
            {
                // Bỏ qua lỗi cập nhật trạng thái nút
            }
        }

        #endregion

        #region ========== TIỆN ÍCH HIỂN THỊ ==========

        /// <summary>
        /// Thiết lập SuperToolTip cho các controls trong form
        /// </summary>
        private void SetupSuperToolTips()
        {
            try
            {
                if (CodeTextEdit != null)
                {
                    SuperToolTipHelper.SetTextEditSuperTip(
                        CodeTextEdit,
                        title: "<b><color=DarkBlue>🔖 Mã đơn vị</color></b>",
                        content: "Nhập mã đơn vị tính (không có khoảng trắng). Trường này là bắt buộc."
                    );
                }

                if (NameTextEdit != null)
                {
                    SuperToolTipHelper.SetTextEditSuperTip(
                        NameTextEdit,
                        title: "<b><color=DarkBlue>📏 Tên đơn vị</color></b>",
                        content: "Nhập tên đơn vị tính. Trường này là bắt buộc."
                    );
                }

                if (DescriptionTextEdit != null)
                {
                    SuperToolTipHelper.SetBaseEditSuperTip(
                        DescriptionTextEdit,
                        title: "<b><color=DarkBlue>📝 Mô tả</color></b>",
                        content: "Nhập mô tả chi tiết về đơn vị tính (tối đa 255 ký tự)."
                    );
                }

                if (IsActiveCheckEdit != null)
                {
                    SuperToolTipHelper.SetBaseEditSuperTip(
                        IsActiveCheckEdit,
                        title: "<b><color=DarkBlue>✅ Trạng thái</color></b>",
                        content: "Đánh dấu nếu đơn vị tính đang hoạt động."
                    );
                }

                if (SaveBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        SaveBarButtonItem,
                        title: "<b><color=Blue>💾 Lưu</color></b>",
                        content: "Lưu thông tin đơn vị tính vào hệ thống."
                    );
                }

                if (EditBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        EditBarButtonItem,
                        title: "<b><color=Orange>✏️ Điều chỉnh</color></b>",
                        content: "Chỉnh sửa thông tin đơn vị tính đã chọn."
                    );
                }

                if (DeleteBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        DeleteBarButtonItem,
                        title: "<b><color=Red>🗑️ Xóa</color></b>",
                        content: "Xóa các đơn vị tính đã chọn khỏi hệ thống."
                    );
                }

                if (AddNewBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        AddNewBarButtonItem,
                        title: "<b><color=Green>➕ Thêm mới</color></b>",
                        content: "Thêm mới đơn vị tính vào hệ thống."
                    );
                }

                if (RefreshBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        RefreshBarButtonItem,
                        title: "<b><color=Blue>🔄 Làm mới</color></b>",
                        content: "Tải lại danh sách đơn vị tính từ hệ thống."
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
        /// Hiển thị thông báo thông tin
        /// </summary>
        /// <param name="message">Nội dung thông báo</param>
        private void ShowInfo(string message)
        {
            MsgBox.ShowSuccess(message);
        }

        /// <summary>
        /// Hiển thị thông báo cảnh báo
        /// </summary>
        /// <param name="message">Nội dung cảnh báo</param>
        private void ShowWarning(string message)
        {
            MsgBox.ShowWarning(message);
        }

        /// <summary>
        /// Hiển thị thông báo lỗi
        /// </summary>
        /// <param name="ex">Exception</param>
        /// <param name="context">Ngữ cảnh lỗi</param>
        private void ShowError(Exception ex, string context = null)
        {
            if (string.IsNullOrWhiteSpace(context))
                MsgBox.ShowException(ex);
            else
                MsgBox.ShowException(new Exception(context + ": " + ex.Message, ex));
        }

        #endregion
    }
}