using Bll.Inventory.InventoryManagement;
using Common.Helpers;
using Common.Utils;
using DevExpress.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DTO.Inventory.InventoryManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Forms;

namespace Inventory.Management
{
    public partial class FrmProductVariantIdentifierAddEdit : XtraForm
    {
        #region ========== FIELDS & PROPERTIES ==========

        /// <summary>
        /// DTO chứa thông tin sản phẩm được chọn
        /// </summary>
        private readonly StockInOutProductHistoryDto _selectedDto;

        /// <summary>
        /// Business Logic Layer cho ProductVariantIdentifier
        /// </summary>
        private readonly ProductVariantIdentifierBll _productVariantIdentifierBll = new ProductVariantIdentifierBll();

        /// <summary>
        /// Cache danh sách ID đã có trong database (lazy load)
        /// </summary>
        private HashSet<Guid> _existingIdsCache = null;

        /// <summary>
        /// Flag đánh dấu đã load cache chưa
        /// </summary>
        private bool _isIdsCacheLoaded = false;

        private Guid _productVariantIdentifierId = Guid.Empty;
        #endregion

        #region ========== CONSTRUCTOR ==========

        public FrmProductVariantIdentifierAddEdit(StockInOutProductHistoryDto selectedDto, Guid productVariantIdentifierId)
        {
            _selectedDto = selectedDto;
            _productVariantIdentifierId = productVariantIdentifierId;
            InitializeComponent();
            InitializeProductVariantIdentifierGrid();
            InitializeBarButtonEvents();
            InitializeFormEvents();
            SetupSuperToolTips();
        }

        #endregion

        #region ========== INITIALIZATION ==========

        /// <summary>
        /// Thiết lập SuperToolTip cho tất cả các controls trong form
        /// </summary>
        private void SetupSuperToolTips()
        {
            try
            {
                // SuperTip cho nút Mới
                if (NewIdentifierBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        NewIdentifierBarButtonItem,
                        title: @"<b><color=Green>➕ Mới</color></b>",
                        content: @"Reset lại toàn bộ dữ liệu và thêm dòng mới vào grid.<br/><br/><b>Chức năng:</b><br/>• Reset lại ProductVariantIdentifierId để thêm mới hoàn toàn<br/>• Xóa tất cả dữ liệu trong grid<br/>• Thêm dòng mới vào grid<br/>• Focus vào cột IdentifierType để bắt đầu nhập liệu<br/><br/><b>Quy trình:</b><br/>1. Reset _productVariantIdentifierId = Guid.Empty<br/>2. Xóa tất cả dữ liệu trong grid<br/>3. Refresh grid để cập nhật UI<br/>4. Reset cache ID<br/>5. Thêm dòng mới và focus vào cột IdentifierType<br/><br/><color=Gray>Lưu ý:</color> Tất cả dữ liệu chưa lưu sẽ bị mất khi click nút này."
                    );
                }

                // SuperTip cho nút Lưu
                if (SaveBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        SaveBarButtonItem,
                        title: @"<b><color=Green>💾 Lưu</color></b>",
                        content: @"Lưu tất cả định danh sản phẩm vào database.<br/><br/><b>Chức năng:</b><br/>• Validate dữ liệu trước khi lưu<br/>• Kiểm tra trùng lặp với database<br/>• Convert items từ grid sang DTO<br/>• Lưu vào bảng ProductVariantIdentifier<br/><br/><b>Quy trình:</b><br/>1. Validate dữ liệu (kiểm tra có dữ liệu, trùng lặp, enum trùng nhau)<br/>2. Lấy danh sách items từ grid<br/>3. Convert items sang DTO<br/>4. Gọi BLL.SaveOrUpdate() để lưu<br/>5. Hiển thị thông báo thành công<br/><br/><b>Validation:</b><br/>• Phải có ít nhất một định danh<br/>• Không được trùng lặp với database<br/>• Mỗi loại định danh chỉ được sử dụng một lần<br/>• Phải có ProductVariantId hợp lệ<br/><br/><b>Thông tin lưu:</b><br/>• ProductVariantId: Từ DTO đã chọn<br/>• Các định danh: SerialNumber, PartNumber, QRCode, SKU, RFID, MACAddress, IMEI, AssetTag, LicenseKey, UPC, EAN, ID, OtherIdentifier<br/><br/><color=Gray>Lưu ý:</color> Hệ thống sẽ kiểm tra trùng lặp và hiển thị cảnh báo nếu có."
                    );
                }

                // SuperTip cho nút Đóng
                if (CloseBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        CloseBarButtonItem,
                        title: @"<b><color=Red>❌ Đóng</color></b>",
                        content: @"Đóng form và không lưu dữ liệu.<br/><br/><b>Chức năng:</b><br/>• Đóng form hiện tại<br/>• Không lưu dữ liệu vào database<br/>• Hủy bỏ mọi thay đổi chưa lưu<br/><br/><color=Gray>Lưu ý:</color> Nếu bạn đã chỉnh sửa định danh, hãy nhớ click <b>Lưu</b> trước khi đóng form."
                    );
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"SetupSuperToolTips: Exception occurred - {ex.Message}");
            }
        }

        /// <summary>
        /// Khởi tạo các event handlers cho form
        /// </summary>
        private void InitializeFormEvents()
        {
            try
            {
                Load += FrmProductVariantIdentifierAddEdit_Load;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"InitializeFormEvents: Exception occurred - {ex.Message}");
            }
        }

        /// <summary>
        /// Event handler khi form được load
        /// </summary>
        private void FrmProductVariantIdentifierAddEdit_Load(object sender, EventArgs e)
        {
            try
            {
                // Load ProductVariantFullName khi form được load
                LoadProductVariantInfo();

                if (_productVariantIdentifierId != Guid.Empty)
                    // Load dữ liệu từ database
                    LoadDataFromDatabase();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    $"FrmProductVariantIdentifierAddEdit_Load: Exception occurred - {ex.Message}");
                MsgBox.ShowError($"Lỗi tải thông tin sản phẩm: {ex.Message}");
            }
        }

        /// <summary>
        /// Khởi tạo các event handlers cho bar buttons
        /// </summary>
        private void InitializeBarButtonEvents()
        {
            try
            {
                NewIdentifierBarButtonItem.ItemClick += NewIdentifierBarButtonItem_ItemClick;
                SaveBarButtonItem.ItemClick += SaveBarButtonItem_ItemClick;
                CloseBarButtonItem.ItemClick += CloseBarButtonItem_ItemClick;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"InitializeBarButtonEvents: Exception occurred - {ex.Message}");
            }
        }

        /// <summary>
        /// Khởi tạo ProductVariantIdentifierGrid
        /// </summary>
        private void InitializeProductVariantIdentifierGrid()
        {
            try
            {
                // Khởi tạo binding source với danh sách rỗng
                productVariantIdentifierItemBindingSource.DataSource = new List<ProductVariantIdentifierItem>();

                // Load enum vào RepositoryItemComboBox
                LoadProductVariantIdentifierEnumRepositoryComboBox();

                // Setup events
                InitializeGridEvents();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"InitializeProductVariantIdentifierGrid: Exception occurred - {ex.Message}");
                MsgBox.ShowError($"Lỗi khởi tạo grid: {ex.Message}");
            }
        }

        /// <summary>
        /// Khởi tạo các event handlers cho grid
        /// </summary>
        private void InitializeGridEvents()
        {
            try
            {
                ProductVariantIdentifierGridView.CellValueChanged += ProductVariantIdentifierGridView_CellValueChanged;
                ProductVariantIdentifierGridView.InitNewRow += ProductVariantIdentifierGridView_InitNewRow;
                ProductVariantIdentifierGridView.RowDeleted += ProductVariantIdentifierGridView_RowDeleted;
                ProductVariantIdentifierGridView.ValidateRow += ProductVariantIdentifierGridView_ValidateRow;
                ProductVariantIdentifierGridView.ValidatingEditor += ProductVariantIdentifierGridView_ValidatingEditor;
                ProductVariantIdentifierGridView.CustomDrawRowIndicator += ProductVariantIdentifierGridView_CustomDrawRowIndicator;
                ProductVariantIdentifierGridView.KeyDown += ProductVariantIdentifierGridView_KeyDown;
                ProductVariantIdentifierGridView.ShownEditor += ProductVariantIdentifierGridView_ShownEditor;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"InitializeGridEvents: Exception occurred - {ex.Message}");
            }
        }

        /// <summary>
        /// Load danh sách ProductVariantIdentifierEnum vào RepositoryItemComboBox
        /// </summary>
        private void LoadProductVariantIdentifierEnumRepositoryComboBox()
        {
            try
            {
                ProductVariantIdentifierEnumComboBox.Items.Clear();

                // Load tất cả các giá trị enum
                foreach (ProductVariantIdentifierEnum value in Enum.GetValues(typeof(ProductVariantIdentifierEnum)))
                {
                    int index = ApplicationEnumUtils.GetValue(value);
                    ProductVariantIdentifierEnumComboBox.Items.Insert(index, value);
                }

                // Cấu hình ComboBox
                ProductVariantIdentifierEnumComboBox.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                ProductVariantIdentifierEnumComboBox.ShowDropDown = DevExpress.XtraEditors.Controls.ShowDropDown.SingleClick;

                // Sử dụng CustomDisplayText để hiển thị Description thay vì tên enum
                ProductVariantIdentifierEnumComboBox.CustomDisplayText += ProductVariantIdentifierEnumRepositoryComboBox_CustomDisplayText;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LoadProductVariantIdentifierEnumRepositoryComboBox: Exception occurred - {ex.Message}");
            }
        }

        /// <summary>
        /// Event handler để hiển thị Description thay vì tên enum trong RepositoryItemComboBox
        /// </summary>
        private void ProductVariantIdentifierEnumRepositoryComboBox_CustomDisplayText(object sender, DevExpress.XtraEditors.Controls.CustomDisplayTextEventArgs e)
        {
            try
            {
                if (e.Value is ProductVariantIdentifierEnum enumValue)
                {
                    // Lấy Description từ helper method
                    e.DisplayText = GetProductVariantIdentifierDescription(enumValue);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ProductVariantIdentifierEnumRepositoryComboBox_CustomDisplayText: Exception occurred - {ex.Message}");
                // Nếu có lỗi, hiển thị tên enum mặc định
                if (e.Value is ProductVariantIdentifierEnum enumValue)
                {
                    e.DisplayText = enumValue.ToString();
                }
            }
        }

        #endregion

        #region ========== EVENT HANDLERS ==========

        /// <summary>
        /// Event handler cho nút "Mới" - Reset lại toàn bộ dữ liệu và thêm dòng mới vào grid
        /// </summary>
        private void NewIdentifierBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                // Reset lại _ProductVariantIdentifierId để thêm mới hoàn toàn
                _productVariantIdentifierId = Guid.Empty;

                // Reset lại toàn bộ dữ liệu của ProductVariantIdentifierGridView
                ResetProductVariantIdentifierGrid();
                
                // Thêm dòng mới vào grid
                ProductVariantIdentifierGridView.AddNewRow();
                
                // Focus vào cột IdentifierType
                var identifierTypeColumn = ProductVariantIdentifierGridView.Columns["IdentifierType"];
                if (identifierTypeColumn != null)
                {
                    ProductVariantIdentifierGridView.FocusedColumn = identifierTypeColumn;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"NewIdentifierBarButtonItem_ItemClick: Exception occurred - {ex.Message}");
                MsgBox.ShowError($"Lỗi reset dữ liệu: {ex.Message}");
            }
        }

        /// <summary>
        /// Event handler cho nút "Lưu"
        /// </summary>
        private void SaveBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                // Validate dữ liệu trước khi lưu
                var (isValid, errorMessage) = ValidateDataBeforeSave();
                if (!isValid)
                {
                    MsgBox.ShowWarning(errorMessage);
                    return;
                }

                // Lấy danh sách items từ grid
                var items = GetIdentifierItems();
                if (items == null || items.Count == 0)
                {
                    MsgBox.ShowWarning("Chưa có dữ liệu để lưu. Vui lòng thêm ít nhất một định danh.");
                    return;
                }

                var dto = ConvertItemToDto(items);

                // Gọi BLL để lưu dữ liệu
                _productVariantIdentifierBll.SaveOrUpdate(dto);

                // Hiển thị thông báo thành công
                MsgBox.ShowSuccess("Lưu dữ liệu thành công");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"SaveBarButtonItem_ItemClick: Exception occurred - {ex.Message}");
                MsgBox.ShowError($"Lỗi lưu dữ liệu: {ex.Message}");
            }
        }

        /// <summary>
        /// Event handler cho nút "Đóng"
        /// </summary>
        private void CloseBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                Close();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"CloseBarButtonItem_ItemClick: Exception occurred - {ex.Message}");
                MsgBox.ShowError($"Lỗi đóng form: {ex.Message}");
            }
        }

        #endregion

        #region ========== GRIDVIEW EVENT HANDLERS ==========

        /// <summary>
        /// Custom draw row indicator để hiển thị số thứ tự
        /// </summary>
        private void ProductVariantIdentifierGridView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            GridViewHelper.CustomDrawRowIndicator(ProductVariantIdentifierGridView, e);
        }

        /// <summary>
        /// Xử lý sự kiện khi giá trị cell thay đổi trong GridView
        /// </summary>
        private void ProductVariantIdentifierGridView_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                var rowHandle = e.RowHandle;

                // Lấy row data từ GridView (bao gồm cả new row)
                var rowData = ProductVariantIdentifierGridView.GetRow(rowHandle) as ProductVariantIdentifierItem;
                if (rowData == null)
                {
                    System.Diagnostics.Debug.WriteLine($"CellValueChanged: Row data is null, RowHandle={rowHandle}");
                    return;
                }

                // Nếu thay đổi là IdentifierType
                if (e.Column != null && e.Column.FieldName == "IdentifierType")
                {
                    // Nếu chọn ID, tự động tạo GUID
                    if (rowData.IdentifierType == ProductVariantIdentifierEnum.ID)
                    {
                        // Chỉ tạo GUID nếu Value chưa có hoặc đang rỗng
                        if (string.IsNullOrWhiteSpace(rowData.Value))
                        {
                            // Tự động tạo GUID mới làm giá trị (đảm bảo không trùng)
                            rowData.Value = GenerateUniqueGuidAsString();

                            // Refresh để hiển thị giá trị mới
                            ProductVariantIdentifierGridView.RefreshRow(rowHandle);
                            
                            // Focus vào cột Value để user có thể thấy giá trị đã được tạo
                            var valueColumn = ProductVariantIdentifierGridView.Columns["Value"];
                            if (valueColumn != null)
                            {
                                ProductVariantIdentifierGridView.FocusedColumn = valueColumn;
                            }
                        }
                    }
                    else
                    {
                        // Với các loại khác (SerialNumber, PartNumber, etc.), focus vào cột Value để user nhập
                        if (string.IsNullOrWhiteSpace(rowData.Value))
                        {
                            var valueColumn = ProductVariantIdentifierGridView.Columns["Value"];
                            if (valueColumn != null)
                            {
                                // Delay một chút để đảm bảo grid đã refresh
                                Timer timer = new Timer();
                                timer.Interval = 100;
                                timer.Tick += (s, args) =>
                                {
                                    timer.Stop();
                                    timer.Dispose();
                                    ProductVariantIdentifierGridView.FocusedColumn = valueColumn;
                                    ProductVariantIdentifierGridView.ShowEditor();
                                };
                                timer.Start();
                            }
                        }
                    }
                }

                // Refresh grid để hiển thị thay đổi (chỉ nếu không phải new row)
                if (rowHandle >= 0)
                {
                    ProductVariantIdentifierGridView.RefreshRow(rowHandle);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"CellValueChanged: Exception occurred - {ex.Message}");
                MsgBox.ShowError($"Lỗi xử lý thay đổi cell: {ex.Message}");
            }
        }

        /// <summary>
        /// Xử lý sự kiện khi thêm dòng mới
        /// </summary>
        private void ProductVariantIdentifierGridView_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            try
            {
                var rowData = ProductVariantIdentifierGridView.GetRow(e.RowHandle) as ProductVariantIdentifierItem;
                if (rowData == null)
                {
                    System.Diagnostics.Debug.WriteLine($"InitNewRow: Row data is null, RowHandle={e.RowHandle}");
                    return;
                }

                // Gán ID mới nếu chưa có
                if (rowData.Id == Guid.Empty)
                {
                    rowData.Id = Guid.NewGuid();
                }

                // Set mặc định cho IdentifierType
                if (rowData.IdentifierType == default(ProductVariantIdentifierEnum))
                {
                    rowData.IdentifierType = ProductVariantIdentifierEnum.SerialNumber;
                }

                // Nếu IdentifierType là ID, tự động tạo GUID cho Value
                if (rowData.IdentifierType == ProductVariantIdentifierEnum.ID && string.IsNullOrWhiteSpace(rowData.Value))
                {
                    rowData.Value = GenerateUniqueGuidAsString();
                    // Refresh để hiển thị giá trị mới
                    ProductVariantIdentifierGridView.RefreshRow(e.RowHandle);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"InitNewRow: Exception occurred - {ex.Message}");
                MsgBox.ShowError($"Lỗi thêm dòng: {ex.Message}");
            }
        }

        /// <summary>
        /// Xử lý sự kiện khi xóa dòng
        /// </summary>
        private void ProductVariantIdentifierGridView_RowDeleted(object sender, RowDeletedEventArgs rowDeletedEventArgs)
        {
            try
            {
                // Nếu dòng đã được lưu vào database, xóa khỏi database
                if (rowDeletedEventArgs.Row is ProductVariantIdentifierItem rowData && rowData.Id != Guid.Empty)
                {
                    try
                    {
                        _productVariantIdentifierBll.Delete(rowData.Id);
                        System.Diagnostics.Debug.WriteLine($"RowDeleted: Đã xóa định danh khỏi database, Id={rowData.Id}");
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"RowDeleted: Lỗi xóa khỏi database - {ex.Message}");
                        // Không throw exception để không block việc xóa dòng khỏi grid
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"RowDeleted: Exception occurred - {ex.Message}");
            }
        }

        /// <summary>
        /// Xử lý sự kiện validate editor (trước khi commit giá trị)
        /// </summary>
        private void ProductVariantIdentifierGridView_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            try
            {
                var column = ProductVariantIdentifierGridView.FocusedColumn;
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
                System.Diagnostics.Debug.WriteLine($"ValidatingEditor: Exception occurred - {ex.Message}");
                e.ErrorText = $"Lỗi validate: {ex.Message}";
                e.Valid = false;
            }
        }

        /// <summary>
        /// Validate row data (sau khi commit giá trị)
        /// </summary>
        private void ProductVariantIdentifierGridView_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            try
            {
                var rowData = e.Row as ProductVariantIdentifierItem;
                if (rowData == null)
                {
                    System.Diagnostics.Debug.WriteLine("ValidateRow: Row data is null");
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
                    System.Diagnostics.Debug.WriteLine($"ValidateRow: DataAnnotations validation failed, Errors={errors}");
                    e.Valid = false;
                    e.ErrorText = errors;
                    return;
                }

                // Validate business rules
                if (string.IsNullOrWhiteSpace(rowData.Value))
                {
                    System.Diagnostics.Debug.WriteLine("ValidateRow: Value is empty");
                    e.Valid = false;
                    e.ErrorText = "Vui lòng nhập giá trị định danh";
                    return;
                }

                e.Valid = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ValidateRow: Exception occurred - {ex.Message}");
                e.Valid = false;
                e.ErrorText = $"Lỗi validate: {ex.Message}";
            }
        }

        /// <summary>
        /// Event handler xử lý phím tắt trong GridView
        /// </summary>
        private void ProductVariantIdentifierGridView_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                var gridView = ProductVariantIdentifierGridView;
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
                            e.Handled = true;
                            var selectedRows = gridView.GetSelectedRows();
                            if (selectedRows != null && selectedRows.Length > 0)
                            {
                                foreach (var rowHandle in selectedRows)
                                {
                                    if (rowHandle >= 0)
                                    {
                                        gridView.DeleteRow(rowHandle);
                                    }
                                }
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"KeyDown: Exception occurred - {ex.Message}");
                MsgBox.ShowError($"Lỗi xử lý phím tắt: {ex.Message}");
            }
        }

        /// <summary>
        /// Xử lý sự kiện khi editor được hiển thị
        /// </summary>
        private void ProductVariantIdentifierGridView_ShownEditor(object sender, EventArgs e)
        {
            try
            {
                var gridView = ProductVariantIdentifierGridView;
                if (gridView == null) return;

                var rowHandle = gridView.FocusedRowHandle;
                var column = gridView.FocusedColumn;

                // Chỉ xử lý khi đang edit cột IdentifierType
                if (column != null && column.FieldName == "IdentifierType")
                {
                    var rowData = gridView.GetRow(rowHandle) as ProductVariantIdentifierItem;
                    if (rowData != null && rowData.IdentifierType == ProductVariantIdentifierEnum.ID)
                    {
                        // Nếu đã chọn ID nhưng chưa có Value, tạo GUID
                        if (string.IsNullOrWhiteSpace(rowData.Value))
                        {
                            rowData.Value = GenerateUniqueGuidAsString();
                            gridView.RefreshRow(rowHandle);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ShownEditor: Exception occurred - {ex.Message}");
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

            if (!(e.Value is ProductVariantIdentifierEnum))
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
            if (value.Length > 500)
            {
                e.ErrorText = "Giá trị định danh không được vượt quá 500 ký tự";
                e.Valid = false;
                return;
            }

            e.Valid = true;
        }

        #endregion

        #region ========== DATA LOADING ==========

        /// <summary>
        /// Load dữ liệu từ database dựa vào ProductVariantId
        /// </summary>
        private void LoadDataFromDatabase()
        {
            try
            {
                if (_selectedDto == null || _selectedDto.ProductVariantId == Guid.Empty)
                {
                    System.Diagnostics.Debug.WriteLine("LoadDataFromDatabase: ProductVariantId is empty");
                    return;
                }

                // Lấy danh sách identifier từ database
                var dtos = _productVariantIdentifierBll.GetById(_productVariantIdentifierId);

                //// Convert DTOs sang Items
                //var items = dtos.Select(dto => ConvertDtoToItem(dto)).Where(item => item != null).ToList();

                //// Load vào grid
                //LoadIdentifierItems(items);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LoadDataFromDatabase: Exception occurred - {ex.Message}");
                MsgBox.ShowError($"Lỗi tải dữ liệu từ database: {ex.Message}");
            }
        }

        /// <summary>
        /// Load và hiển thị ProductVariantFullName dựa vào ProductVariantId
        /// </summary>
        private void LoadProductVariantInfo()
        {
            try
            {
                // Kiểm tra ProductVariantId
                ProductVariantFullNameSimpleLabelItem.Text = _selectedDto?.ProductVariantFullName ?? "N/A";

                SoLuongNhapXuatBarStaticItem.Caption =
                    $@"Số lượng nhập/xuất: <color='red'><b> {(_selectedDto?.StockInQty > 0 ? _selectedDto.StockInQty : _selectedDto?.StockOutQty ?? 0)}</color></b>";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LoadProductVariantInfo: Exception occurred - {ex.Message}");
                ProductVariantFullNameSimpleLabelItem.Text = $"Lỗi tải thông tin: {ex.Message}";
            }
        }

        #endregion

        #region ========== PUBLIC METHODS ==========

        /// <summary>
        /// Lấy danh sách ProductVariantIdentifierItem từ BindingSource
        /// </summary>
        public List<ProductVariantIdentifierItem> GetIdentifierItems()
        {
            try
            {
                var items = new List<ProductVariantIdentifierItem>();

                foreach (var item in productVariantIdentifierItemBindingSource)
                {
                    if (item is ProductVariantIdentifierItem identifierItem)
                    {
                        items.Add(identifierItem);
                    }
                }

                return items;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetIdentifierItems: Exception occurred - {ex.Message}");
                return new List<ProductVariantIdentifierItem>();
            }
        }

        /// <summary>
        /// Load danh sách ProductVariantIdentifierItem vào grid
        /// </summary>
        public void LoadIdentifierItems(List<ProductVariantIdentifierItem> items)
        {
            try
            {
                if (items == null)
                {
                    items = new List<ProductVariantIdentifierItem>();
                }

                productVariantIdentifierItemBindingSource.DataSource = items;
                productVariantIdentifierItemBindingSource.ResetBindings(false);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LoadIdentifierItems: Exception occurred - {ex.Message}");
                MsgBox.ShowError($"Lỗi load dữ liệu: {ex.Message}");
            }
        }

        /// <summary>
        /// Xóa tất cả các dòng trong grid
        /// </summary>
        public void ClearIdentifierItems()
        {
            try
            {
                productVariantIdentifierItemBindingSource.DataSource = new List<ProductVariantIdentifierItem>();
                productVariantIdentifierItemBindingSource.ResetBindings(false);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ClearIdentifierItems: Exception occurred - {ex.Message}");
            }
        }

        /// <summary>
        /// Reset lại toàn bộ dữ liệu của ProductVariantIdentifierGridView
        /// Xóa tất cả dữ liệu, refresh grid và reset cache
        /// </summary>
        private void ResetProductVariantIdentifierGrid()
        {
            try
            {
                // Xóa tất cả dữ liệu trong grid
                ClearIdentifierItems();

                // Refresh grid để cập nhật UI
                ProductVariantIdentifierGridView.RefreshData();

                // Reset cache để đảm bảo không có dữ liệu cũ
                ResetIdsCache();

                System.Diagnostics.Debug.WriteLine("ResetProductVariantIdentifierGrid: Đã reset toàn bộ dữ liệu grid thành công");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ResetProductVariantIdentifierGrid: Exception occurred - {ex.Message}");
                MsgBox.ShowError($"Lỗi reset dữ liệu grid: {ex.Message}");
            }
        }

        #endregion

        #region ========== VALIDATION & CONVERSION ==========

        /// <summary>
        /// Validate dữ liệu trước khi lưu (bao gồm kiểm tra trùng lặp với database)
        /// </summary>
        /// <returns>Tuple (isValid, errorMessage)</returns>
        private (bool isValid, string errorMessage) ValidateDataBeforeSave()
        {
            try
            {
                // Lấy danh sách items từ grid
                var items = GetIdentifierItems();
                if (items == null || items.Count == 0)
                {
                    return (false, "Chưa có dữ liệu để lưu. Vui lòng thêm ít nhất một định danh.");
                }

                // Kiểm tra trùng lặp với database
                LoadExistingIdsCache();
                
                // Lấy tất cả identifier hiện có trong database để kiểm tra trùng lặp
                var existingIdentifiers = _productVariantIdentifierBll.GetAll();

                var dto = ConvertItemToDto(items);
                if (dto == null) 
                    return (false, "Lỗi chuyển đổi dữ liệu.");
                
                // Kiểm tra từng item xem có trùng với database không
                foreach (var item in items)
                {
                    // Kiểm tra trùng lặp theo từng loại identifier
                    var duplicate = existingIdentifiers.FirstOrDefault(existing =>
                    {
                        // Bỏ qua chính nó nếu đang update
                        if (existing.Id == dto.Id && dto.Id != Guid.Empty)
                        {
                            return false;
                        }

                        // Kiểm tra trùng theo từng loại identifier
                        return CheckDuplicateIdentifier(existing, item, dto);
                    });

                    if (duplicate != null)
                    {
                        var identifierTypeName = GetProductVariantIdentifierDescription(item.IdentifierType);
                        return (false, $"Giá trị \"{item.Value}\" của loại định danh \"{identifierTypeName}\" đã tồn tại trong hệ thống. Vui lòng kiểm tra lại.");
                    }
                }

                // Kiểm tra enum trùng nhau trong cùng grid
                var identifierTypeGroups = items
                    .GroupBy(item => item.IdentifierType)
                    .Where(g => g.Count() > 1)
                    .ToList();

                if (identifierTypeGroups.Count > 0)
                {
                    var duplicateTypes = identifierTypeGroups
                        .Select(g => GetProductVariantIdentifierDescription(g.Key))
                        .ToList();
                    
                    var duplicateTypesText = string.Join(", ", duplicateTypes);
                    return (false, $"Có loại định danh bị trùng lặp: {duplicateTypesText}. Mỗi loại định danh chỉ được sử dụng một lần.");
                }

                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ValidateDataBeforeSave: Exception occurred - {ex.Message}");
                return (false, $"Lỗi kiểm tra dữ liệu: {ex.Message}");
            }
        }

        /// <summary>
        /// Kiểm tra trùng lặp identifier giữa existing và item
        /// </summary>
        private bool CheckDuplicateIdentifier(ProductVariantIdentifierDto existing, ProductVariantIdentifierItem item, ProductVariantIdentifierDto dto)
        {
            if (existing.ProductVariantId != dto.ProductVariantId)
            {
                return false;
            }

            switch (item.IdentifierType)
            {
                case ProductVariantIdentifierEnum.SerialNumber:
                    return !string.IsNullOrWhiteSpace(existing.SerialNumber) &&
                           existing.SerialNumber == item.Value;
                case ProductVariantIdentifierEnum.PartNumber:
                    return !string.IsNullOrWhiteSpace(existing.PartNumber) &&
                           existing.PartNumber == item.Value;
                case ProductVariantIdentifierEnum.QRCode:
                    return !string.IsNullOrWhiteSpace(existing.QRCode) &&
                           existing.QRCode == item.Value;
                case ProductVariantIdentifierEnum.SKU:
                    return !string.IsNullOrWhiteSpace(existing.SKU) &&
                           existing.SKU == item.Value;
                case ProductVariantIdentifierEnum.RFID:
                    return !string.IsNullOrWhiteSpace(existing.RFID) &&
                           existing.RFID == item.Value;
                case ProductVariantIdentifierEnum.MACAddress:
                    return !string.IsNullOrWhiteSpace(existing.MACAddress) &&
                           existing.MACAddress == item.Value;
                case ProductVariantIdentifierEnum.IMEI:
                    return !string.IsNullOrWhiteSpace(existing.IMEI) &&
                           existing.IMEI == item.Value;
                case ProductVariantIdentifierEnum.AssetTag:
                    return !string.IsNullOrWhiteSpace(existing.AssetTag) &&
                           existing.AssetTag == item.Value;
                case ProductVariantIdentifierEnum.LicenseKey:
                    return !string.IsNullOrWhiteSpace(existing.LicenseKey) &&
                           existing.LicenseKey == item.Value;
                case ProductVariantIdentifierEnum.UPC:
                    return !string.IsNullOrWhiteSpace(existing.UPC) &&
                           existing.UPC == item.Value;
                case ProductVariantIdentifierEnum.EAN:
                    return !string.IsNullOrWhiteSpace(existing.EAN) &&
                           existing.EAN == item.Value;
                case ProductVariantIdentifierEnum.ID:
                    return !string.IsNullOrWhiteSpace(existing.ID) &&
                           existing.ID == item.Value;
                case ProductVariantIdentifierEnum.OtherIdentifier:
                    return !string.IsNullOrWhiteSpace(existing.OtherIdentifier) &&
                           existing.OtherIdentifier == item.Value;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Convert ProductVariantIdentifierItem sang ProductVariantIdentifierDto
        /// </summary>
        /// <param name="items"></param>
        /// <returns>ProductVariantIdentifierDto hoặc null nếu không thể convert</returns>
        private ProductVariantIdentifierDto ConvertItemToDto(List<ProductVariantIdentifierItem> items)
        {
            try
            {
                if (!items.Any())
                {
                    return null;
                }

                var dto = new ProductVariantIdentifierDto
                {
                    Id = _productVariantIdentifierId == Guid.Empty ? Guid.NewGuid() : _productVariantIdentifierId,
                    ProductVariantId = _selectedDto.ProductVariantId,
                };

                foreach (var item in items)
                {

                    // Map giá trị vào property tương ứng theo IdentifierType
                    switch (item.IdentifierType)
                    {
                        case ProductVariantIdentifierEnum.SerialNumber:
                            dto.SerialNumber = item.Value;
                            break;
                        case ProductVariantIdentifierEnum.PartNumber:
                            dto.PartNumber = item.Value;
                            break;
                        case ProductVariantIdentifierEnum.QRCode:
                            dto.QRCode = item.Value;
                            break;
                        case ProductVariantIdentifierEnum.SKU:
                            dto.SKU = item.Value;
                            break;
                        case ProductVariantIdentifierEnum.RFID:
                            dto.RFID = item.Value;
                            break;
                        case ProductVariantIdentifierEnum.MACAddress:
                            dto.MACAddress = item.Value;
                            break;
                        case ProductVariantIdentifierEnum.IMEI:
                            dto.IMEI = item.Value;
                            break;
                        case ProductVariantIdentifierEnum.AssetTag:
                            dto.AssetTag = item.Value;
                            break;
                        case ProductVariantIdentifierEnum.LicenseKey:
                            dto.LicenseKey = item.Value;
                            break;
                        case ProductVariantIdentifierEnum.UPC:
                            dto.UPC = item.Value;
                            break;
                        case ProductVariantIdentifierEnum.EAN:
                            dto.EAN = item.Value;
                            break;
                        case ProductVariantIdentifierEnum.ID:
                            dto.ID = item.Value;
                            break;
                        case ProductVariantIdentifierEnum.OtherIdentifier:
                            dto.OtherIdentifier = item.Value;
                            break;
                        default:
                            System.Diagnostics.Debug.WriteLine(
                                $"ConvertItemToDto: Unknown IdentifierType {item.IdentifierType}");
                            return null;
                    }
                }

                return dto;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ConvertItemToDto: Exception occurred - {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Convert ProductVariantIdentifierDto sang ProductVariantIdentifierItem
        /// </summary>
        /// <param name="dto">ProductVariantIdentifierDto</param>
        /// <returns>ProductVariantIdentifierItem hoặc null nếu không thể convert</returns>
        private ProductVariantIdentifierItem ConvertDtoToItem(ProductVariantIdentifierDto dto)
        {
            try
            {
                if (dto == null)
                {
                    return null;
                }

                var item = new ProductVariantIdentifierItem
                {
                    Id = dto.Id,
                };

                // Xác định IdentifierType và Value từ DTO
                if (!string.IsNullOrWhiteSpace(dto.SerialNumber))
                {
                    item.IdentifierType = ProductVariantIdentifierEnum.SerialNumber;
                    item.Value = dto.SerialNumber;
                }
                else if (!string.IsNullOrWhiteSpace(dto.PartNumber))
                {
                    item.IdentifierType = ProductVariantIdentifierEnum.PartNumber;
                    item.Value = dto.PartNumber;
                }
                else if (!string.IsNullOrWhiteSpace(dto.QRCode))
                {
                    item.IdentifierType = ProductVariantIdentifierEnum.QRCode;
                    item.Value = dto.QRCode;
                }
                else if (!string.IsNullOrWhiteSpace(dto.SKU))
                {
                    item.IdentifierType = ProductVariantIdentifierEnum.SKU;
                    item.Value = dto.SKU;
                }
                else if (!string.IsNullOrWhiteSpace(dto.RFID))
                {
                    item.IdentifierType = ProductVariantIdentifierEnum.RFID;
                    item.Value = dto.RFID;
                }
                else if (!string.IsNullOrWhiteSpace(dto.MACAddress))
                {
                    item.IdentifierType = ProductVariantIdentifierEnum.MACAddress;
                    item.Value = dto.MACAddress;
                }
                else if (!string.IsNullOrWhiteSpace(dto.IMEI))
                {
                    item.IdentifierType = ProductVariantIdentifierEnum.IMEI;
                    item.Value = dto.IMEI;
                }
                else if (!string.IsNullOrWhiteSpace(dto.AssetTag))
                {
                    item.IdentifierType = ProductVariantIdentifierEnum.AssetTag;
                    item.Value = dto.AssetTag;
                }
                else if (!string.IsNullOrWhiteSpace(dto.LicenseKey))
                {
                    item.IdentifierType = ProductVariantIdentifierEnum.LicenseKey;
                    item.Value = dto.LicenseKey;
                }
                else if (!string.IsNullOrWhiteSpace(dto.UPC))
                {
                    item.IdentifierType = ProductVariantIdentifierEnum.UPC;
                    item.Value = dto.UPC;
                }
                else if (!string.IsNullOrWhiteSpace(dto.EAN))
                {
                    item.IdentifierType = ProductVariantIdentifierEnum.EAN;
                    item.Value = dto.EAN;
                }
                else if (!string.IsNullOrWhiteSpace(dto.ID))
                {
                    item.IdentifierType = ProductVariantIdentifierEnum.ID;
                    item.Value = dto.ID;
                }
                else if (!string.IsNullOrWhiteSpace(dto.OtherIdentifier))
                {
                    item.IdentifierType = ProductVariantIdentifierEnum.OtherIdentifier;
                    item.Value = dto.OtherIdentifier;
                }
                else
                {
                    // Không có identifier nào được set, return null
                    System.Diagnostics.Debug.WriteLine("ConvertDtoToItem: DTO không có identifier nào được set");
                    return null;
                }

                return item;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ConvertDtoToItem: Exception occurred - {ex.Message}");
                return null;
            }
        }

        #endregion

        #region ========== GUID GENERATION ==========

        /// <summary>
        /// Load cache danh sách ID đã có trong database
        /// </summary>
        private void LoadExistingIdsCache()
        {
            if (_isIdsCacheLoaded)
            {
                return;
            }

            try
            {
                _existingIdsCache = new HashSet<Guid>();

                // Lấy danh sách ID từ database
                var allIdentifiers = _productVariantIdentifierBll.GetAll();
                foreach (var identifier in allIdentifiers)
                {
                    _existingIdsCache.Add(identifier.Id);
                }

                _isIdsCacheLoaded = true;
                System.Diagnostics.Debug.WriteLine($"LoadExistingIdsCache: Đã load {_existingIdsCache.Count} ID từ database");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LoadExistingIdsCache: Exception occurred - {ex.Message}");
                // Nếu có lỗi, khởi tạo empty set để tiếp tục
                _existingIdsCache = new HashSet<Guid>();
                _isIdsCacheLoaded = true;
            }
        }

        /// <summary>
        /// Lấy danh sách ID từ grid hiện tại (chưa lưu)
        /// </summary>
        /// <returns>HashSet chứa các ID trong grid</returns>
        private HashSet<Guid> GetCurrentGridIds()
        {
            var gridIds = new HashSet<Guid>();

            try
            {
                foreach (var item in productVariantIdentifierItemBindingSource)
                {
                    if (item is ProductVariantIdentifierItem identifierItem)
                    {
                        // Lấy ID từ Value nếu IdentifierType là ID
                        if (identifierItem.IdentifierType == ProductVariantIdentifierEnum.ID &&
                            !string.IsNullOrWhiteSpace(identifierItem.Value) &&
                            Guid.TryParse(identifierItem.Value, out Guid valueGuid))
                        {
                            gridIds.Add(valueGuid);
                        }

                        // Cũng thêm Id của item vào để tránh trùng
                        if (identifierItem.Id != Guid.Empty)
                        {
                            gridIds.Add(identifierItem.Id);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetCurrentGridIds: Exception occurred - {ex.Message}");
            }

            return gridIds;
        }

        /// <summary>
        /// Tạo GUID mới đảm bảo không trùng với ID có sẵn trong ProductVariantIdentifier
        /// </summary>
        /// <returns>GUID dưới dạng string</returns>
        private string GenerateUniqueGuidAsString()
        {
            // Load cache nếu chưa load
            LoadExistingIdsCache();

            // Lấy danh sách ID từ grid hiện tại
            var gridIds = GetCurrentGridIds();

            // Hợp nhất tất cả ID đã có
            var allExistingIds = new HashSet<Guid>(_existingIdsCache);
            foreach (var gridId in gridIds)
            {
                allExistingIds.Add(gridId);
            }

            // Tạo GUID mới cho đến khi không trùng
            Guid newGuid;
            int maxAttempts = 100; // Giới hạn số lần thử để tránh vòng lặp vô hạn
            int attempts = 0;

            do
            {
                newGuid = Guid.NewGuid();
                attempts++;

                if (attempts >= maxAttempts)
                {
                    System.Diagnostics.Debug.WriteLine("GenerateUniqueGuidAsString: Đã đạt giới hạn số lần thử, sử dụng GUID cuối cùng");
                    break;
                }
            }
            while (allExistingIds.Contains(newGuid));

            // Thêm GUID mới vào cache để tránh trùng trong cùng session
            allExistingIds.Add(newGuid);
            _existingIdsCache.Add(newGuid);

            System.Diagnostics.Debug.WriteLine($"GenerateUniqueGuidAsString: Đã tạo GUID mới sau {attempts} lần thử");
            return newGuid.ToString();
        }

        /// <summary>
        /// Reset cache ID (dùng khi cần reload từ database)
        /// </summary>
        public void ResetIdsCache()
        {
            _isIdsCacheLoaded = false;
            _existingIdsCache = null;
        }

        #endregion

        #region ========== HELPER METHODS ==========

        /// <summary>
        /// Lấy Description từ ProductVariantIdentifierEnum value
        /// </summary>
        /// <param name="identifierType">Giá trị enum</param>
        /// <returns>Description hoặc tên enum nếu không có Description</returns>
        private string GetProductVariantIdentifierDescription(ProductVariantIdentifierEnum identifierType)
        {
            try
            {
                return ApplicationEnumUtils.GetDescription(identifierType);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetProductVariantIdentifierDescription: Exception occurred for {identifierType} - {ex.Message}");
                return identifierType.ToString();
            }
        }

        #endregion
    }
}
