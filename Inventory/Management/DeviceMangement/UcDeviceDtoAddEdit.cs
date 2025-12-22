using Bll.Inventory.InventoryManagement;
using Bll.MasterData.ProductServiceBll;
using Common.Common;
using Common.Helpers;
using Common.Utils;
using Dal.DataContext;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DTO.Inventory.InventoryManagement;
using DTO.MasterData.ProductService;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.Data;

namespace Inventory.Management.DeviceMangement
{
    public partial class UcDeviceDtoAddEdit : DevExpress.XtraEditors.XtraUserControl
    {
        #region ========== FIELDS & PROPERTIES ==========

        /// <summary>
        /// Business Logic Layer cho biến thể sản phẩm
        /// </summary>
        private readonly ProductVariantBll _productVariantBll = new ProductVariantBll();

        /// <summary>
        /// Business Logic Layer cho thiết bị
        /// </summary>
        private readonly DeviceBll _deviceBll = new DeviceBll();

        /// <summary>
        /// Logger để ghi log các sự kiện
        /// </summary>
        private readonly ILogger _logger = LoggerFactory.CreateLogger(LogCategory.UI);

        /// <summary>
        /// Trạng thái đang tải dữ liệu (guard tránh gọi LoadProductVariantsAsync song song)
        /// </summary>
        private bool _isLoadingProductVariants;

        /// <summary>
        /// Flag đánh dấu ProductVariant datasource đã được load chưa
        /// </summary>
        private bool _isProductVariantDataSourceLoaded;

        #endregion

        #region ========== DEVICE IDENTIFIER ITEM CLASS ==========

        /// <summary>
        /// Class đơn giản để bind vào IdentifierValueGridView
        /// </summary>
        public class DeviceIdentifierItem
        {
            /// <summary>
            /// ID duy nhất của item
            /// </summary>
            public Guid Id { get; set; } = Guid.NewGuid();

            /// <summary>
            /// Loại định danh (DeviceIdentifierEnum)
            /// </summary>
            [DisplayName("Kiểu định danh")]
            [Required(ErrorMessage = "Vui lòng chọn kiểu định danh")]
            public DeviceIdentifierEnum IdentifierType { get; set; }

            /// <summary>
            /// Giá trị định danh
            /// </summary>
            [DisplayName("Giá trị")]
            [Required(ErrorMessage = "Vui lòng nhập giá trị định danh")]
            [StringLength(255, ErrorMessage = "Giá trị định danh không được vượt quá 255 ký tự")]
            public string Value { get; set; }
        }

        #endregion

        #region ========== CONSTRUCTOR ==========

        public UcDeviceDtoAddEdit()
        {
            InitializeComponent();
            InitializeControl();
        }

        #endregion

        #region ========== INITIALIZATION ==========

        /// <summary>
        /// Khởi tạo control
        /// </summary>
        private void InitializeControl()
        {
            try
            {
                // Khởi tạo binding source với danh sách rỗng
                identifierValueBindingSource.DataSource = new List<DeviceIdentifierItem>();

                // Setup events
                InitializeEvents();

                // Setup SuperToolTips
                SetupSuperToolTips();
            }
            catch (Exception ex)
            {
                _logger.Error("InitializeControl: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi khởi tạo control: {ex.Message}");
            }
        }

        /// <summary>
        /// Khởi tạo các event handlers
        /// </summary>
        private void InitializeEvents()
        {
            // Event khi SearchLookUpEdit popup - load dữ liệu nếu chưa load
            ProductVariantSearchLookUpEdit.Properties.Popup += SearchLookUpEdit1_Popup;

            // Load DeviceIdentifierEnum vào ComboBox
            LoadDeviceIdentifierEnumComboBox();

            // Setup GridView events
            InitializeGridViewEvents();

            // Setup Save button event
            SaveHyperlinkLabelControl.Click += SaveHyperlinkLabelControl_Click;
        }

        /// <summary>
        /// Khởi tạo các event handlers cho GridView
        /// </summary>
        private void InitializeGridViewEvents()
        {
            // Event khi thay đổi cell value
            IdentifierValueGridView.CellValueChanged += IdentifierValueGridView_CellValueChanged;

            // Event khi thêm/xóa dòng
            IdentifierValueGridView.InitNewRow += IdentifierValueGridView_InitNewRow;
            IdentifierValueGridView.RowDeleted += IdentifierValueGridView_RowDeleted;

            // Event khi validate cell và row
            IdentifierValueGridView.ValidateRow += IdentifierValueGridView_ValidateRow;
            IdentifierValueGridView.ValidatingEditor += IdentifierValueGridView_ValidatingEditor;

            // Event custom draw row indicator
            IdentifierValueGridView.CustomDrawRowIndicator += IdentifierValueGridView_CustomDrawRowIndicator;

            // Event xử lý phím tắt cho GridView (Insert, Delete, Enter)
            IdentifierValueGridView.KeyDown += IdentifierValueGridView_KeyDown;
        }

        #endregion

        #region ========== PRODUCT VARIANT MANAGEMENT ==========

        /// <summary>
        /// Load danh sách biến thể sản phẩm vào SearchLookUpEdit
        /// Method này được gọi từ form khi FormLoad
        /// Sử dụng SplashScreen để tăng trải nghiệm người dùng và ngăn người dùng thao tác khi đang load data
        /// </summary>
        /// <param name="forceRefresh">Nếu true, sẽ load lại từ database ngay cả khi đã load trước đó</param>
        private async Task LoadProductVariantsAsync(bool forceRefresh = false)
        {
            if (_isLoadingProductVariants) return;
            _isLoadingProductVariants = true;

            try
            {
                // Nếu đã load và không force refresh, không load lại
                if (_isProductVariantDataSourceLoaded && !forceRefresh &&
                    productVariantListDtoBindingSource.DataSource != null &&
                    productVariantListDtoBindingSource.DataSource is List<ProductVariantListDto> existingList &&
                    existingList.Count > 0)
                {
                    return;
                }

                // Hiển thị SplashScreen để thông báo đang load dữ liệu
                SplashScreenHelper.ShowWaitingSplashScreen();

                try
                {
                    // Lấy dữ liệu Entity từ BLL với thông tin đầy đủ
                    var variants = await _productVariantBll.GetAllInUseWithDetailsAsync();

                    // Convert Entity sang ProductVariantListDto
                    var variantListDtos = await ConvertToVariantListDtosAsync(variants);

                    // Bind dữ liệu vào BindingSource
                    productVariantListDtoBindingSource.DataSource = variantListDtos;
                    productVariantListDtoBindingSource.ResetBindings(false);

                    _isProductVariantDataSourceLoaded = true;
                }
                finally
                {
                    // Đóng SplashScreen sau khi hoàn thành hoặc có lỗi
                    SplashScreenHelper.CloseSplashScreen();
                }
            }
            catch (Exception ex)
            {
                _logger.Error("LoadProductVariantsAsync: Exception occurred", ex);
                SplashScreenHelper.CloseSplashScreen(); // Đảm bảo đóng SplashScreen khi có lỗi
                MsgBox.ShowError($"Lỗi tải danh sách biến thể sản phẩm: {ex.Message}");
            }
            finally
            {
                _isLoadingProductVariants = false;
            }
        }

        /// <summary>
        /// Reload ProductVariant datasource (public method để gọi từ form)
        /// </summary>
        public async Task ReloadProductVariantDataSourceAsync()
        {
            try
            {
                await LoadProductVariantsAsync(forceRefresh: true);
            }
            catch (Exception ex)
            {
                _logger.Error("ReloadProductVariantDataSourceAsync: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi reload datasource biến thể sản phẩm: {ex.Message}");
            }
        }

        /// <summary>
        /// Event handler khi SearchLookUpEdit popup
        /// Chỉ load dữ liệu nếu chưa load hoặc datasource rỗng
        /// </summary>
        private async void SearchLookUpEdit1_Popup(object sender, EventArgs e)
        {
            try
            {
                // Chỉ load nếu chưa load hoặc datasource rỗng
                if (!_isProductVariantDataSourceLoaded ||
                    productVariantListDtoBindingSource.DataSource == null ||
                    (productVariantListDtoBindingSource.DataSource is List<ProductVariantListDto> list && list.Count == 0))
                {
                    await LoadProductVariantsAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.Error("SearchLookUpEdit1_Popup: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi tải dữ liệu biến thể sản phẩm: {ex.Message}");
            }
        }

        /// <summary>
        /// Convert Entity sang ProductVariantListDto (Async)
        /// Sử dụng extension method ToListDto() có sẵn trong DTO và bổ sung các field còn thiếu
        /// </summary>
        private Task<List<ProductVariantListDto>> ConvertToVariantListDtosAsync(List<ProductVariant> variants)
        {
            try
            {
                var result = new List<ProductVariantListDto>();

                foreach (var variant in variants)
                {
                    // Sử dụng extension method ToListDto() có sẵn trong DTO
                    var dto = variant.ToListDto();
                    if (dto == null) continue;

                    result.Add(dto);
                }

                return Task.FromResult(result);
            }
            catch (Exception ex)
            {
                _logger.Error("ConvertToVariantListDtosAsync: Exception occurred", ex);
                throw new Exception($"Lỗi convert sang ProductVariantListDto: {ex.Message}", ex);
            }
        }

        #endregion

        #region ========== DEVICE IDENTIFIER ENUM MANAGEMENT ==========

        /// <summary>
        /// Load danh sách DeviceIdentifierEnum vào ComboBox
        /// Sử dụng Description attribute để hiển thị text
        /// </summary>
        private void LoadDeviceIdentifierEnumComboBox()
        {
            try
            {
                DeviceIdentifierEnumComboBox.Items.Clear();

                // Load tất cả các giá trị enum trực tiếp
                foreach (DeviceIdentifierEnum identifierType in Enum.GetValues(typeof(DeviceIdentifierEnum)))
                {
                    DeviceIdentifierEnumComboBox.Items.Add(identifierType);
                }

                // Cấu hình ComboBox
                DeviceIdentifierEnumComboBox.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                DeviceIdentifierEnumComboBox.ShowDropDown = DevExpress.XtraEditors.Controls.ShowDropDown.SingleClick;

                // Sử dụng CustomDisplayText để hiển thị Description thay vì tên enum
                DeviceIdentifierEnumComboBox.CustomDisplayText += DeviceIdentifierEnumComboBox_CustomDisplayText;
            }
            catch (Exception ex)
            {
                _logger.Error("LoadDeviceIdentifierEnumComboBox: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi load danh sách kiểu định danh: {ex.Message}");
            }
        }

        /// <summary>
        /// Event handler để hiển thị Description thay vì tên enum trong ComboBox
        /// </summary>
        private void DeviceIdentifierEnumComboBox_CustomDisplayText(object sender, DevExpress.XtraEditors.Controls.CustomDisplayTextEventArgs e)
        {
            try
            {
                if (e.Value is DeviceIdentifierEnum enumValue)
                {
                    // Lấy Description từ attribute
                    e.DisplayText = GetEnumDescription(enumValue);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("DeviceIdentifierEnumComboBox_CustomDisplayText: Exception occurred", ex);
                // Nếu có lỗi, hiển thị tên enum mặc định
                if (e.Value is DeviceIdentifierEnum enumValue)
                {
                    e.DisplayText = enumValue.ToString();
                }
            }
        }

        /// <summary>
        /// Lấy Description từ enum value
        /// </summary>
        /// <param name="enumValue">Giá trị enum</param>
        /// <returns>Description hoặc tên enum nếu không có Description</returns>
        private string GetEnumDescription(DeviceIdentifierEnum enumValue)
        {
            try
            {
                var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
                if (fieldInfo == null) return enumValue.ToString();

                var descriptionAttribute = fieldInfo.GetCustomAttribute<DescriptionAttribute>();
                return descriptionAttribute?.Description ?? enumValue.ToString();
            }
            catch (Exception ex)
            {
                _logger.Error($"GetEnumDescription: Exception occurred for {enumValue}", ex);
                return enumValue.ToString();
            }
        }

        #endregion

        #region ========== GRIDVIEW EVENT HANDLERS ==========

        /// <summary>
        /// Custom draw row indicator để hiển thị số thứ tự
        /// </summary>
        private void IdentifierValueGridView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            GridViewHelper.CustomDrawRowIndicator(IdentifierValueGridView, e);
        }

        /// <summary>
        /// Xử lý sự kiện khi giá trị cell thay đổi trong GridView
        /// </summary>
        private void IdentifierValueGridView_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                var fieldName = e.Column?.FieldName;
                var rowHandle = e.RowHandle;

                if (rowHandle < 0)
                {
                    return; // Bỏ qua new row
                }

                // Lấy row data từ GridView
                if (IdentifierValueGridView.GetRow(rowHandle) is not DeviceIdentifierItem rowData)
                {
                    _logger.Warning("CellValueChanged: Row data is null, RowHandle={0}", rowHandle);
                    return;
                }

                // Refresh grid để hiển thị thay đổi
                IdentifierValueGridView.RefreshRow(rowHandle);
            }
            catch (Exception ex)
            {
                _logger.Error("CellValueChanged: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi xử lý thay đổi cell: {ex.Message}");
            }
        }

        /// <summary>
        /// Xử lý sự kiện khi thêm dòng mới
        /// </summary>
        private void IdentifierValueGridView_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            try
            {
                var rowData = IdentifierValueGridView.GetRow(e.RowHandle) as DeviceIdentifierItem;
                if (rowData == null)
                {
                    _logger.Warning("InitNewRow: Row data is null, RowHandle={0}", e.RowHandle);
                    return;
                }

                // Gán ID mới nếu chưa có
                if (rowData.Id == Guid.Empty)
                {
                    rowData.Id = Guid.NewGuid();
                }

                // Set mặc định cho IdentifierType
                if (rowData.IdentifierType == default(DeviceIdentifierEnum))
                {
                    rowData.IdentifierType = DeviceIdentifierEnum.SerialNumber;
                }
            }
            catch (Exception ex)
            {
                _logger.Error("InitNewRow: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi thêm dòng: {ex.Message}");
            }
        }

        /// <summary>
        /// Xử lý sự kiện khi xóa dòng
        /// </summary>
        private void IdentifierValueGridView_RowDeleted(object sender, RowDeletedEventArgs e)
        {
            try
            {
                // Có thể thêm logic xử lý khi xóa dòng ở đây
                _logger.Debug("RowDeleted: Row deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.Error("RowDeleted: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi xóa dòng: {ex.Message}");
            }
        }

        /// <summary>
        /// Xử lý sự kiện validate editor (trước khi commit giá trị)
        /// </summary>
        private void IdentifierValueGridView_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            try
            {
                var column = IdentifierValueGridView.FocusedColumn;
                var fieldName = column?.FieldName;

                if (string.IsNullOrEmpty(fieldName)) return;

                switch (fieldName)
                {
                    // Validate IdentifierType: Phải chọn loại định danh
                    case "IdentifierType":
                        ValidateIdentifierType(e);
                        break;
                    // Validate Value: Phải nhập giá trị
                    case "Value":
                        ValidateValue(e);
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.Error("ValidatingEditor: Exception occurred", ex);
                e.ErrorText = $"Lỗi validate: {ex.Message}";
                e.Valid = false;
            }
        }

        /// <summary>
        /// Validate row data (sau khi commit giá trị)
        /// </summary>
        private void IdentifierValueGridView_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            try
            {
                var rowData = e.Row as DeviceIdentifierItem;
                if (rowData == null)
                {
                    _logger.Warning("ValidateRow: Row data is null");
                    e.Valid = false;
                    e.ErrorText = "Dữ liệu không hợp lệ";
                    return;
                }

                // Validate bằng DataAnnotations
                var validationResults = new List<ValidationResult>();
                var validationContext = new ValidationContext(rowData);
                var isValid = Validator.TryValidateObject(rowData, validationContext, validationResults, true);

                if (!isValid)
                {
                    var errors = string.Join("\n", validationResults.Select(r => r.ErrorMessage));
                    _logger.Warning("ValidateRow: DataAnnotations validation failed, Errors={0}", errors);
                    e.Valid = false;
                    e.ErrorText = errors;
                    return;
                }

                // Validate business rules
                if (string.IsNullOrWhiteSpace(rowData.Value))
                {
                    _logger.Warning("ValidateRow: Value is empty");
                    e.Valid = false;
                    e.ErrorText = "Vui lòng nhập giá trị định danh";
                    return;
                }

                e.Valid = true;
            }
            catch (Exception ex)
            {
                _logger.Error("ValidateRow: Exception occurred", ex);
                e.Valid = false;
                e.ErrorText = $"Lỗi validate: {ex.Message}";
            }
        }

        /// <summary>
        /// Event handler xử lý phím tắt trong GridView
        /// </summary>
        private void IdentifierValueGridView_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                var gridView = IdentifierValueGridView;
                if (gridView == null) return;

                switch (e.KeyCode)
                {
                    case Keys.Insert:
                        // Insert: Thêm dòng mới
                        if (!e.Control && !e.Shift && !e.Alt)
                        {
                            e.Handled = true;
                            gridView.AddNewRow();
                            // Focus vào cột IdentifierType
                            var identifierTypeColumn = gridView.Columns["IdentifierType"];
                            if (identifierTypeColumn != null)
                            {
                                gridView.FocusedColumn = identifierTypeColumn;
                            }
                        }
                        break;

                    case Keys.Delete:
                        // Delete: Xóa dòng được chọn
                        if (!e.Control && !e.Shift && !e.Alt)
                        {
                            var focusedRowHandle = gridView.FocusedRowHandle;
                            if (focusedRowHandle >= 0)
                            {
                                e.Handled = true;
                                gridView.DeleteRow(focusedRowHandle);
                            }
                        }
                        break;

                    case Keys.Enter:
                        // Enter: Hoàn thành nhập dòng (commit row)
                        if (!e.Control && !e.Shift && !e.Alt)
                        {
                            var focusedRowHandle = gridView.FocusedRowHandle;

                            // Nếu đang ở new row (rowHandle < 0), commit row
                            if (focusedRowHandle == DevExpress.XtraGrid.GridControl.NewItemRowHandle)
                            {
                                e.Handled = true;
                                // Validate row trước khi commit
                                if (gridView.PostEditor())
                                {
                                    gridView.UpdateCurrentRow();
                                }
                            }
                            // Nếu đang ở dòng đã có, di chuyển xuống dòng tiếp theo hoặc thêm dòng mới
                            else if (focusedRowHandle >= 0)
                            {
                                // Nếu đang ở cột cuối cùng, di chuyển xuống dòng tiếp theo
                                var focusedColumn = gridView.FocusedColumn;
                                var lastColumn = gridView.VisibleColumns[gridView.VisibleColumns.Count - 1];

                                if (focusedColumn == lastColumn)
                                {
                                    e.Handled = true;

                                    // Di chuyển xuống dòng tiếp theo hoặc thêm dòng mới
                                    var nextRowHandle = focusedRowHandle + 1;
                                    if (nextRowHandle < gridView.RowCount)
                                    {
                                        gridView.FocusedRowHandle = nextRowHandle;
                                        gridView.FocusedColumn = gridView.Columns[0]; // Focus vào cột đầu tiên
                                    }
                                    else
                                    {
                                        // Thêm dòng mới
                                        gridView.AddNewRow();
                                        var identifierTypeColumn = gridView.Columns["IdentifierType"];
                                        if (identifierTypeColumn != null)
                                        {
                                            gridView.FocusedColumn = identifierTypeColumn;
                                        }
                                    }
                                }
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.Error("IdentifierValueGridView_KeyDown: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi xử lý phím tắt: {ex.Message}");
            }
        }

        /// <summary>
        /// Validate IdentifierType: Phải chọn loại định danh
        /// </summary>
        private void ValidateIdentifierType(DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            if (e.Value == null)
            {
                e.ErrorText = "Vui lòng chọn kiểu định danh";
                e.Valid = false;
                return;
            }

            if (!(e.Value is DeviceIdentifierEnum))
            {
                e.ErrorText = "Kiểu định danh không hợp lệ";
                e.Valid = false;
                return;
            }

            e.Valid = true;
        }

        /// <summary>
        /// Validate Value: Phải nhập giá trị
        /// </summary>
        private void ValidateValue(DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            if (e.Value == null || string.IsNullOrWhiteSpace(e.Value.ToString()))
            {
                e.ErrorText = "Vui lòng nhập giá trị định danh";
                e.Valid = false;
                return;
            }

            var value = e.Value.ToString().Trim();
            if (value.Length > 255)
            {
                e.ErrorText = "Giá trị định danh không được vượt quá 255 ký tự";
                e.Valid = false;
                return;
            }

            e.Valid = true;
        }

        #endregion

        #region ========== PUBLIC METHODS ==========

        /// <summary>
        /// Lấy danh sách DeviceIdentifierItem từ grid
        /// </summary>
        public List<DeviceIdentifierItem> GetIdentifierItems()
        {
            try
            {
                var items = new List<DeviceIdentifierItem>();

                foreach (var item in identifierValueBindingSource)
                {
                    if (item is DeviceIdentifierItem identifierItem)
                    {
                        items.Add(identifierItem);
                    }
                }

                return items;
            }
            catch (Exception ex)
            {
                _logger.Error("GetIdentifierItems: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi lấy danh sách định danh: {ex.Message}");
                return new List<DeviceIdentifierItem>();
            }
        }

        /// <summary>
        /// Load danh sách DeviceIdentifierItem vào grid
        /// </summary>
        public void LoadIdentifierItems(List<DeviceIdentifierItem> items)
        {
            try
            {
                items ??= new List<DeviceIdentifierItem>();
                identifierValueBindingSource.DataSource = items;
                identifierValueBindingSource.ResetBindings(false);
                IdentifierValueGridView.RefreshData();
            }
            catch (Exception ex)
            {
                _logger.Error("LoadIdentifierItems: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi tải danh sách định danh: {ex.Message}");
            }
        }

        /// <summary>
        /// Clear dữ liệu và reset về trạng thái ban đầu
        /// </summary>
        public void ClearData()
        {
            try
            {
                identifierValueBindingSource.DataSource = new List<DeviceIdentifierItem>();
                identifierValueBindingSource.ResetBindings(false);
            }
            catch (Exception ex)
            {
                _logger.Error("ClearData: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi xóa dữ liệu: {ex.Message}");
            }
        }

        /// <summary>
        /// Validate tất cả các dòng trong grid
        /// </summary>
        public bool ValidateAll()
        {
            try
            {
                var items = identifierValueBindingSource.Cast<DeviceIdentifierItem>().ToList();

                if (items.Count == 0)
                {
                    _logger.Warning("ValidateAll: No items found");
                    MsgBox.ShowWarning("Vui lòng thêm ít nhất một định danh");
                    return false;
                }

                foreach (var item in items)
                {
                    var validationResults = new List<ValidationResult>();
                    var validationContext = new ValidationContext(item);
                    var isValid = Validator.TryValidateObject(item, validationContext, validationResults, true);

                    if (!isValid)
                    {
                        var errors = string.Join("\n", validationResults.Select(r => r.ErrorMessage));
                        _logger.Warning("ValidateAll: Item validation failed, Errors={0}", errors);
                        MsgBox.ShowError($"Định danh có lỗi:\n{errors}");
                        return false;
                    }

                    // Validate business rules
                    if (string.IsNullOrWhiteSpace(item.Value))
                    {
                        _logger.Warning("ValidateAll: Item has empty Value");
                        MsgBox.ShowError("Vui lòng nhập giá trị định danh");
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.Error("ValidateAll: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi validate: {ex.Message}");
                return false;
            }
        }

        #endregion

        #region ========== SAVE OPERATIONS ==========

        /// <summary>
        /// Event handler khi click nút Lưu
        /// </summary>
        private async void SaveHyperlinkLabelControl_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate và lưu dữ liệu
                var success = await SaveDataAsync();
                if (success)
                {
                    MsgBox.ShowSuccess("Lưu thông tin thiết bị thành công!");
                    // Có thể trigger event để form cha refresh danh sách
                    OnDeviceSaved();
                }
            }
            catch (Exception ex)
            {
                _logger.Error("SaveHyperlinkLabelControl_Click: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi lưu thông tin thiết bị: {ex.Message}");
            }
        }

        /// <summary>
        /// Lưu dữ liệu thiết bị vào database
        /// </summary>
        private async Task<bool> SaveDataAsync()
        {
            try
            {
                _logger.Debug("SaveDataAsync: Starting save operation");

                // Validate dữ liệu trước khi lưu
                var validationErrors = ValidateBeforeSave();
                if (validationErrors.Any())
                {
                    MsgBox.ShowError($"Có lỗi trong dữ liệu:\n\n{string.Join("\n", validationErrors)}", "Lỗi validation");
                    return false;
                }

                // Hiển thị SplashScreen
                SplashScreenHelper.ShowWaitingSplashScreen();

                try
                {
                    // Tạo DeviceDto từ dữ liệu đã nhập
                    var deviceDto = CreateDeviceDtoFromInput();

                    // Convert DTO sang Entity
                    var device = deviceDto.ToEntity();

                    // Lưu qua BLL
                    await Task.Run(() =>
                    {
                        _deviceBll.SaveOrUpdate(device);
                    });

                    _logger.Info("SaveDataAsync: Save operation completed successfully");
                    return true;
                }
                finally
                {
                    SplashScreenHelper.CloseSplashScreen();
                }
            }
            catch (Exception ex)
            {
                _logger.Error("SaveDataAsync: Exception occurred", ex);
                SplashScreenHelper.CloseSplashScreen();
                MsgBox.ShowError($"Lỗi lưu dữ liệu: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Validate dữ liệu trước khi lưu
        /// </summary>
        private List<string> ValidateBeforeSave()
        {
            var validationErrors = new List<string>();

            try
            {
                // Validate ProductVariantId
                var productVariantId = ProductVariantSearchLookUpEdit.EditValue;
                if (productVariantId == null || productVariantId == DBNull.Value)
                {
                    validationErrors.Add("Vui lòng chọn hàng hóa dịch vụ");
                }
                else if (!(productVariantId is Guid guidValue) || guidValue == Guid.Empty)
                {
                    validationErrors.Add("Hàng hóa dịch vụ không hợp lệ");
                }

                // Validate DeviceIdentifierItems
                var identifierItems = GetIdentifierItems();
                if (identifierItems.Count == 0)
                {
                    validationErrors.Add("Vui lòng thêm ít nhất một định danh thiết bị");
                }

                // Validate mỗi item
                for (int i = 0; i < identifierItems.Count; i++)
                {
                    var item = identifierItems[i];
                    var rowNumber = i + 1;

                    if (string.IsNullOrWhiteSpace(item.Value))
                    {
                        validationErrors.Add($"Dòng {rowNumber}: Vui lòng nhập giá trị định danh");
                    }
                }

                // Validate unique: Mỗi DeviceIdentifierEnum chỉ có một giá trị duy nhất
                var duplicateErrors = ValidateUniqueIdentifiers(identifierItems);
                validationErrors.AddRange(duplicateErrors);

                // Validate tất cả các dòng trong grid
                if (!ValidateAll())
                {
                    validationErrors.Add("Có lỗi validation trong danh sách định danh");
                }
            }
            catch (Exception ex)
            {
                _logger.Error("ValidateBeforeSave: Exception occurred", ex);
                validationErrors.Add($"Lỗi validate: {ex.Message}");
            }

            return validationErrors;
        }

        /// <summary>
        /// Validate đảm bảo mỗi DeviceIdentifierEnum chỉ có một giá trị duy nhất trong datatable
        /// </summary>
        private List<string> ValidateUniqueIdentifiers(List<DeviceIdentifierItem> identifierItems)
        {
            var validationErrors = new List<string>();

            try
            {
                // Kiểm tra trùng lặp cho từng loại định danh
                foreach (DeviceIdentifierEnum identifierType in Enum.GetValues(typeof(DeviceIdentifierEnum)))
                {
                    var itemsWithType = identifierItems
                        .Where(item => item.IdentifierType == identifierType && !string.IsNullOrWhiteSpace(item.Value))
                        .ToList();

                    if (itemsWithType.Count > 1)
                    {
                        // Kiểm tra xem có giá trị trùng lặp không (case-insensitive)
                        var duplicateGroups = itemsWithType
                            .GroupBy(item => item.Value.Trim(), StringComparer.OrdinalIgnoreCase)
                            .Where(g => g.Count() > 1)
                            .ToList();

                        if (duplicateGroups.Any())
                        {
                            foreach (var group in duplicateGroups)
                            {
                                var duplicateIndices = identifierItems
                                    .Select((item, index) => new { item, index })
                                    .Where(x => x.item.IdentifierType == identifierType &&
                                                string.Equals(x.item.Value?.Trim(), group.Key, StringComparison.OrdinalIgnoreCase))
                                    .Select(x => x.index + 1)
                                    .ToList();

                                var identifierTypeName = GetEnumDescription(identifierType);
                                validationErrors.Add($"{identifierTypeName} '{group.Key}' bị trùng lặp ở các dòng: {string.Join(", ", duplicateIndices)}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("ValidateUniqueIdentifiers: Exception occurred", ex);
                validationErrors.Add($"Lỗi kiểm tra trùng lặp: {ex.Message}");
            }

            return validationErrors;
        }

        /// <summary>
        /// Tạo DeviceDto từ dữ liệu đã nhập
        /// </summary>
        private DeviceDto CreateDeviceDtoFromInput()
        {
            try
            {
                // Lấy ProductVariantId
                var productVariantId = ProductVariantSearchLookUpEdit.EditValue;
                Guid productVariantGuid = Guid.Empty;
                if (productVariantId is Guid guid)
                {
                    productVariantGuid = guid;
                }
                else if (productVariantId != null && Guid.TryParse(productVariantId.ToString(), out var parsedGuid))
                {
                    productVariantGuid = parsedGuid;
                }

                if (productVariantGuid == Guid.Empty)
                {
                    throw new InvalidOperationException("ProductVariantId không hợp lệ");
                }

                // Tạo DeviceDto
                var deviceDto = new DeviceDto
                {
                    Id = Guid.NewGuid(), // Tạo ID mới cho thiết bị mới
                    ProductVariantId = productVariantGuid,
                    IsActive = true,
                    CreatedDate = DateTime.Now,
                    Status = DeviceStatusEnum.Available, // Đang trong kho VNS (mặc định)
                    DeviceType = 0 // Hardware (mặc định)
                };

                // Lấy danh sách DeviceIdentifierItem và set vào DeviceDto
                var identifierItems = GetIdentifierItems();
                foreach (var item in identifierItems)
                {
                    if (!string.IsNullOrWhiteSpace(item.Value))
                    {
                        deviceDto.SetIdentifierValue(item.IdentifierType, item.Value.Trim());
                    }
                }

                return deviceDto;
            }
            catch (Exception ex)
            {
                _logger.Error("CreateDeviceDtoFromInput: Exception occurred", ex);
                throw new Exception($"Lỗi tạo DeviceDto: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Event được trigger khi thiết bị được lưu thành công
        /// </summary>
        public event EventHandler DeviceSaved;

        /// <summary>
        /// Trigger event DeviceSaved
        /// </summary>
        protected virtual void OnDeviceSaved()
        {
            DeviceSaved?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region ========== SUPERTOOLTIP ==========

        /// <summary>
        /// Thiết lập SuperToolTip cho tất cả các controls trong UserControl
        /// </summary>
        private void SetupSuperToolTips()
        {
            try
            {
                SetupSearchLookUpEditSuperTips();

                SetupHyperlinkLabelControlSuperTips();
            }
            catch (Exception ex)
            {
                _logger.Error("SetupSuperToolTips: Exception occurred", ex);
            }
        }

        /// <summary>
        /// Thiết lập SuperToolTip cho SearchLookUpEdit controls
        /// </summary>
        private void SetupSearchLookUpEditSuperTips()
        {
            // SuperTip cho ProductVariantSearchLookUpEdit (chọn hàng hóa dịch vụ)
            if (ProductVariantSearchLookUpEdit != null)
            {
                SuperToolTipHelper.SetBaseEditSuperTip(
                    ProductVariantSearchLookUpEdit,
                    title: @"<b><color=DarkBlue>📦 Chọn hàng hóa dịch vụ</color></b>",
                    content: @"Chọn <b>hàng hóa dịch vụ</b> (ProductVariant) cho thiết bị.<br/><br/><b>Chức năng:</b><br/>• Hiển thị danh sách hàng hóa dịch vụ đang sử dụng<br/>• Tự động load dữ liệu khi mở popup (lazy loading)<br/>• Hiển thị thông tin đầy đủ dưới dạng HTML<br/>• Tìm kiếm theo mã, tên, danh mục<br/><br/><b>Hiển thị:</b><br/>• Mã hàng hóa dịch vụ<br/>• Tên đầy đủ (VariantFullName)<br/>• Danh mục, đơn vị tính<br/>• Format HTML với màu sắc và định dạng đẹp<br/><br/><b>Validation:</b><br/>• <b>Bắt buộc chọn</b> trước khi lưu<br/>• Kiểm tra ProductVariantId không được rỗng<br/><br/><color=Gray>Lưu ý:</color> Chỉ hiển thị các hàng hóa dịch vụ đang sử dụng (IsActive = true)."
                );
            }
        }
         

        /// <summary>
        /// Thiết lập SuperToolTip cho HyperlinkLabelControl controls
        /// </summary>
        private void SetupHyperlinkLabelControlSuperTips()
        {
            // SuperTip cho nút Lưu
            if (SaveHyperlinkLabelControl != null)
            {
                SaveHyperlinkLabelControl.SuperTip = SuperToolTipHelper.CreateSuperToolTip(
                    title: @"<b><color=Green>💾 Lưu thiết bị</color></b>",
                    content: @"Lưu <b>thông tin thiết bị</b> vào database.<br/><br/><b>Chức năng:</b><br/>• Validate toàn bộ dữ liệu trước khi lưu<br/>• Tạo DeviceDto từ dữ liệu đã nhập<br/>• Convert DTO sang Entity<br/>• Lưu vào database qua BLL<br/>• Trigger event DeviceSaved để form cha refresh<br/><br/><b>Validation:</b><br/>• Kiểm tra đã chọn hàng hóa dịch vụ<br/>• Kiểm tra có ít nhất một định danh<br/>• Validate từng định danh (loại và giá trị)<br/>• Kiểm tra tính duy nhất của định danh<br/><br/><b>Dữ liệu lưu:</b><br/>• ProductVariantId<br/>• Các định danh (SerialNumber, IMEI, MAC, v.v.)<br/>• Trạng thái mặc định (Available - Đang trong kho VNS)<br/>• Loại thiết bị mặc định (Hardware)<br/>• Ngày tạo, người tạo<br/><br/><b>Xử lý lỗi:</b><br/>• Hiển thị danh sách lỗi validation nếu có<br/>• Hiển thị thông báo lỗi chi tiết nếu lưu thất bại<br/><br/><color=Gray>Lưu ý:</color> Sau khi lưu thành công, form cha sẽ tự động refresh danh sách thiết bị."
                );
            }
        }

        #endregion
    }
}
