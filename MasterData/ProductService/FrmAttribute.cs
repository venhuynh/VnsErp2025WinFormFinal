using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Bll.MasterData.ProductServiceBll;
using Bll.Utils;
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
    /// Form quản lý Thuộc Tính (Attribute) dạng popup.
    /// Cung cấp chức năng CRUD đầy đủ với validation nghiệp vụ và giao diện thân thiện.
    /// </summary>
    public partial class FrmAttribute : XtraForm
    {
        #region ========== KHAI BÁO BIẾN ==========

        /// <summary>
        /// Danh sách thuộc tính để hiển thị trên grid
        /// </summary>
        private readonly BindingList<AttributeDto> _attributes = new BindingList<AttributeDto>();

        /// <summary>
        /// Thuộc tính hiện tại đang được chỉnh sửa
        /// </summary>
        private AttributeDto _current;

        /// <summary>
        /// Danh sách ID của các thuộc tính được chọn trên grid
        /// </summary>
        private List<Guid> _selectedAttributeIds = new List<Guid>();

        /// <summary>
        /// Trạng thái đang trong quá trình chỉnh sửa (true) hay không (false)
        /// </summary>
        private bool _isEditing;

        /// <summary>
        /// Business Logic Layer cho thuộc tính
        /// </summary>
        private readonly AttributeBll _AttributeBll = new AttributeBll();

        /// <summary>
        /// Kết quả trả về cho form gọi (danh sách thuộc tính)
        /// </summary>
        private IList<AttributeDto> ResultAttributes => _attributes.ToList();

        #endregion

        #region ========== CONSTRUCTOR & PUBLIC METHODS ==========

        /// <summary>
        /// Constructor mặc định
        /// </summary>
        public FrmAttribute()
        {
            InitializeComponent();
            InitializeBehavior();
        }

        /// <summary>
        /// Constructor với dữ liệu khởi tạo
        /// </summary>
        /// <param name="initialAttributes">Danh sách thuộc tính ban đầu</param>
        public FrmAttribute(IEnumerable<AttributeDto> initialAttributes) : this()
        {
            if (initialAttributes != null)
            {
                foreach (var a in initialAttributes)
                {
                    _attributes.Add(a);
                }
            }
        }

        /// <summary>
        /// Hiển thị form quản lý thuộc tính dạng dialog
        /// </summary>
        /// <param name="owner">Form cha</param>
        /// <param name="initialAttributes">Danh sách thuộc tính ban đầu</param>
        /// <returns>Danh sách thuộc tính sau khi chỉnh sửa</returns>
        public static IList<AttributeDto> ShowManage(IWin32Window owner, IEnumerable<AttributeDto> initialAttributes)
        {
            using (var frm = new FrmAttribute(initialAttributes))
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog(owner);
                return frm.ResultAttributes;
            }
        }

        /// <summary>
        /// API: Thêm mới thuộc tính từ code bên ngoài
        /// </summary>
        /// <param name="name">Tên thuộc tính</param>
        /// <param name="dataType">Kiểu dữ liệu</param>
        /// <param name="description">Mô tả</param>
        /// <returns>Đối tượng AttributeDto đã tạo</returns>
        public AttributeDto AddNew(string name = "", string dataType = "", string description = "")
        {
            var dto = new AttributeDto
            {
                Name = name,
                DataType = dataType,
                Description = description
            };
            _attributes.Add(dto);
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
            attributeDtoBindingSource.DataSource = _attributes;

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
            RequiredFieldHelper.MarkRequiredFields(this, typeof(AttributeDto));

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
            AttributeGridView.SelectionChanged += AttributeGridView_SelectionChanged;
            AttributeGridView.CustomDrawRowIndicator += AttributeGridView_CustomDrawRowIndicator;
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
            KeyDown += FrmAttribute_KeyDown;
        }

        /// <summary>
        /// Đảm bảo có ít nhất 1 bản ghi mặc định
        /// </summary>
        private void EnsureDefaultRecord()
        {
            if (_attributes.Count == 0)
            {
                _attributes.Add(new AttributeDto());
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
        private void AttributeGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                _selectedAttributeIds = GridViewHelper.GetSelectedRowColumnValues<Guid>(sender, nameof(AttributeDto.Id));
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
        private void AttributeGridView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
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
            AddNewAttribute();
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
        private void FrmAttribute_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                SaveCurrent();
                e.Handled = true;
            }
            else if (e.Control && e.KeyCode == Keys.N)
            {
                AddNewAttribute();
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
        /// Bắt đầu chỉnh sửa thuộc tính được chọn
        /// </summary>
        private void BeginEditSelected()
        {
            // Kiểm tra số lượng dòng được chọn
            if (_selectedAttributeIds == null || _selectedAttributeIds.Count == 0)
            {
                ShowInfo("Vui lòng chọn một dòng để chỉnh sửa.");
                return;
            }

            if (_selectedAttributeIds.Count > 1)
            {
                ShowInfo("Chỉ cho phép chỉnh sửa 1 dòng. Vui lòng bỏ chọn bớt.");
                return;
            }

            // Tìm đối tượng được chọn
            var selectedDto = FindSelectedAttribute();
            if (selectedDto == null)
            {
                ShowInfo("Không thể xác định dòng được chọn để chỉnh sửa.");
                return;
            }

            // Kiểm tra phụ thuộc dữ liệu trước khi cho phép edit
            if (selectedDto.Id != Guid.Empty && _AttributeBll.HasDependencies(selectedDto.Id))
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
        /// Tìm thuộc tính được chọn từ danh sách
        /// </summary>
        /// <returns>Đối tượng AttributeDto được chọn hoặc null</returns>
        private AttributeDto FindSelectedAttribute()
        {
            var id = _selectedAttributeIds[0];

            // Ưu tiên lấy từ FocusedRow nếu khớp Id
            if (AttributeGridView.GetFocusedRow() is AttributeDto focused && focused.Id == id)
            {
                return focused;
            }

            // Tìm đúng DTO trong datasource theo Id
            if (attributeDtoBindingSource.DataSource is IEnumerable<AttributeDto> list)
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
            NameTextEdit.EditValue = _current.Name;
            DataTypeTextEdit.EditValue = _current.DataType;
            DescriptionTextEdit.EditValue = _current.Description;

            // Focus vào trường tên và chọn toàn bộ text
            NameTextEdit.Focus();
            NameTextEdit.SelectAll();
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
                    ShowInfo("Đã lưu thuộc tính");

                    // Làm mới dữ liệu và clear selection
                    RefreshBarButtonItem.PerformClick();
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi lưu thuộc tính");
            }
        }

        /// <summary>
        /// Chuyển dữ liệu từ editors vào đối tượng hiện tại
        /// </summary>
        private void BindDataFromEditors()
        {
            if (_current != null)
            {
                _current.Name = NameTextEdit.EditValue?.ToString().Trim() ?? string.Empty;
                _current.DataType = DataTypeTextEdit.EditValue?.ToString().Trim() ?? string.Empty;
                _current.Description = DescriptionTextEdit.EditValue?.ToString().Trim() ?? string.Empty;
            }
        }

        /// <summary>
        /// Kiểm tra trùng lặp và lưu dữ liệu
        /// </summary>
        /// <returns>True nếu lưu thành công</returns>
        private bool ValidateAndSaveData()
        {
            // Lấy giá trị từ editors để kiểm tra trùng lặp
            var nameToCheck = NameTextEdit.EditValue?.ToString().Trim() ?? string.Empty;

            // Kiểm tra trùng tên
            if (_AttributeBll.IsNameExists(nameToCheck, _current.Id == Guid.Empty ? null : (Guid?)_current.Id))
            {
                dxErrorProvider1.SetError(NameTextEdit, "Tên thuộc tính đã tồn tại");
                ShowWarning("Tên thuộc tính đã tồn tại. Vui lòng chọn tên khác.");
                return false;
            }

            // Lưu xuống database qua BLL
            var entity = _current.ToEntity();
            _AttributeBll.SaveOrUpdate(entity);
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
            var name = NameTextEdit.EditValue?.ToString().Trim() ?? string.Empty;
            var dataType = DataTypeTextEdit.EditValue?.ToString().Trim() ?? string.Empty;
            var description = DescriptionTextEdit.EditValue?.ToString().Trim() ?? string.Empty;

            var errors = new List<string>();
            bool isValid = true;

            // Validate tên thuộc tính
            if (!ValidateName(name, errors))
                isValid = false;

            // Validate kiểu dữ liệu
            if (!ValidateDataType(dataType, errors))
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
        /// Validate tên thuộc tính
        /// </summary>
        /// <param name="name">Tên thuộc tính</param>
        /// <param name="errors">Danh sách lỗi</param>
        /// <returns>True nếu hợp lệ</returns>
        private bool ValidateName(string name, List<string> errors)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                errors.Add("Tên thuộc tính không được để trống");
                dxErrorProvider1.SetError(NameTextEdit, "Tên thuộc tính không được để trống");
                return false;
            }

            if (name.Length > 100)
            {
                errors.Add("Tên thuộc tính không được vượt quá 100 ký tự");
                dxErrorProvider1.SetError(NameTextEdit, "Tên thuộc tính không được vượt quá 100 ký tự");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validate kiểu dữ liệu
        /// </summary>
        /// <param name="dataType">Kiểu dữ liệu</param>
        /// <param name="errors">Danh sách lỗi</param>
        /// <returns>True nếu hợp lệ</returns>
        private bool ValidateDataType(string dataType, List<string> errors)
        {
            if (string.IsNullOrWhiteSpace(dataType))
            {
                errors.Add("Kiểu dữ liệu không được để trống");
                dxErrorProvider1.SetError(DataTypeTextEdit, "Kiểu dữ liệu không được để trống");
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
            dxErrorProvider1.SetError(NameTextEdit, string.Empty);
            dxErrorProvider1.SetError(DataTypeTextEdit, string.Empty);
            dxErrorProvider1.SetError(DescriptionTextEdit, string.Empty);
        }

        #endregion

        #region ========== CHỨC NĂNG XÓA ==========

        /// <summary>
        /// Xóa các thuộc tính được chọn
        /// </summary>
        private void DeleteSelected()
        {
            var count = _selectedAttributeIds?.Count ?? 0;
            if (count == 0)
            {
                ShowInfo("Vui lòng chọn ít nhất 1 dòng để xóa.");
                return;
            }

            // Xác nhận xóa
            var confirmMsg = count == 1
                ? "Bạn có chắc muốn xóa thuộc tính đã chọn?"
                : $"Bạn có chắc muốn xóa {count} thuộc tính đã chọn?";
            if (!MsgBox.ShowYesNo(confirmMsg)) return;

            try
            {
                // Thu thập các DTO được chọn để xóa
                var toDelete = CollectSelectedAttributes();

                // Xóa từng thuộc tính
                foreach (var dto in toDelete)
                {
                    if (ValidateAndDeleteAttribute(dto))
                    {
                        _AttributeBll.Delete(dto.Id);
                        _attributes.Remove(dto);
                    }
                }

                UpdateButtonStates();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi xóa thuộc tính");
                return;
            }

            // Đảm bảo có ít nhất 1 bản ghi
            EnsureAtLeastOneRecord();
            ClearSelectionAndRefresh();
        }

        /// <summary>
        /// Thu thập các thuộc tính được chọn
        /// </summary>
        /// <returns>Danh sách thuộc tính cần xóa</returns>
        private List<AttributeDto> CollectSelectedAttributes()
        {
            var toDelete = new List<AttributeDto>();
            if (attributeDtoBindingSource.DataSource is IEnumerable<AttributeDto> list)
            {
                foreach (var item in list)
                {
                    if (_selectedAttributeIds.Contains(item.Id))
                        toDelete.Add(item);
                }
            }

            return toDelete;
        }

        /// <summary>
        /// Kiểm tra và xóa thuộc tính
        /// </summary>
        /// <param name="dto">Thuộc tính cần xóa</param>
        /// <returns>True nếu có thể xóa</returns>
        private bool ValidateAndDeleteAttribute(AttributeDto dto)
        {
            // Chặn xóa khi còn dữ liệu phụ thuộc
            if (_AttributeBll.HasDependencies(dto.Id))
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
            if (_attributes.Count == 0)
            {
                _attributes.Add(new AttributeDto());
            }
        }

        /// <summary>
        /// Xóa selection và làm mới grid
        /// </summary>
        private void ClearSelectionAndRefresh()
        {
            ClearSelectionState();
            AttributeGridView.FocusedRowHandle = Math.Max(0, _attributes.Count - 1);
            AttributeGridView.RefreshData();
        }

        #endregion

        #region ========== CHỨC NĂNG THÊM MỚI ==========

        /// <summary>
        /// Tạo mới thuộc tính
        /// </summary>
        private void AddNewAttribute()
        {
            try
            {
                // Xóa selection và validation hiện tại
                ClearSelectionState();
                ClearValidation();
                _current = null;

                // Tạo thuộc tính mới
                var newAttribute = new AttributeDto();
                _attributes.Add(newAttribute);
                _current = newAttribute;

                // Xóa các editor để nhập dữ liệu mới
                ClearEditors();

                // Focus vào trường tên và chọn toàn bộ text
                NameTextEdit.Focus();
                NameTextEdit.SelectAll();

                // Thiết lập trạng thái chỉnh sửa
                _isEditing = true;
                UpdateButtonStates();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi tạo mới thuộc tính");
            }
        }

        /// <summary>
        /// Xóa dữ liệu trong các editor
        /// </summary>
        private void ClearEditors()
        {
            NameTextEdit.EditValue = null;
            DataTypeTextEdit.EditValue = null;
            DescriptionTextEdit.EditValue = null;
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
                var entities = _AttributeBll.GetAll();
                var dtos = entities.ToDtoList();
                SetData(dtos);
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi tải danh sách thuộc tính");
            }
        }

        /// <summary>
        /// Thiết lập dữ liệu cho form (API từ form gọi)
        /// </summary>
        /// <param name="attributes">Danh sách thuộc tính</param>
        private void SetData(IEnumerable<AttributeDto> attributes)
        {
            _attributes.Clear();
            if (attributes != null)
            {
                foreach (var a in attributes)
                {
                    _attributes.Add(a);
                }
            }

            EnsureAtLeastOneEmptyRecord();
            attributeDtoBindingSource.ResetBindings(false);
        }

        /// <summary>
        /// Đảm bảo có ít nhất 1 bản ghi trống để tạo mới
        /// </summary>
        private void EnsureAtLeastOneEmptyRecord()
        {
            if (_attributes.Count == 0)
            {
                _attributes.Add(new AttributeDto());
            }
        }

        /// <summary>
        /// Xóa trạng thái selection
        /// </summary>
        private void ClearSelectionState()
        {
            _selectedAttributeIds.Clear();
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
                var selectedCount = _selectedAttributeIds?.Count ?? 0;

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
                if (NameTextEdit != null)
                {
                    SuperToolTipHelper.SetTextEditSuperTip(
                        NameTextEdit,
                        title: "<b><color=DarkBlue>📝 Tên thuộc tính</color></b>",
                        content: "Nhập tên thuộc tính. Trường này là bắt buộc."
                    );
                }

                if (DataTypeTextEdit != null)
                {
                    SuperToolTipHelper.SetTextEditSuperTip(
                        DataTypeTextEdit,
                        title: "<b><color=DarkBlue>🔢 Kiểu dữ liệu</color></b>",
                        content: "Nhập kiểu dữ liệu (ví dụ: string, int, decimal, date, bool). Trường này là bắt buộc."
                    );
                }

                if (DescriptionTextEdit != null)
                {
                    SuperToolTipHelper.SetBaseEditSuperTip(
                        DescriptionTextEdit,
                        title: "<b><color=DarkBlue>📄 Mô tả</color></b>",
                        content: "Nhập mô tả chi tiết về thuộc tính (tối đa 255 ký tự)."
                    );
                }

                if (SaveBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        SaveBarButtonItem,
                        title: "<b><color=Blue>💾 Lưu</color></b>",
                        content: "Lưu thông tin thuộc tính vào hệ thống."
                    );
                }

                if (EditBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        EditBarButtonItem,
                        title: "<b><color=Orange>✏️ Điều chỉnh</color></b>",
                        content: "Chỉnh sửa thông tin thuộc tính đã chọn."
                    );
                }

                if (DeleteBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        DeleteBarButtonItem,
                        title: "<b><color=Red>🗑️ Xóa</color></b>",
                        content: "Xóa các thuộc tính đã chọn khỏi hệ thống."
                    );
                }

                if (AddNewBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        AddNewBarButtonItem,
                        title: "<b><color=Green>➕ Thêm mới</color></b>",
                        content: "Thêm mới thuộc tính vào hệ thống."
                    );
                }

                if (RefreshBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        RefreshBarButtonItem,
                        title: "<b><color=Blue>🔄 Làm mới</color></b>",
                        content: "Tải lại danh sách thuộc tính từ hệ thống."
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