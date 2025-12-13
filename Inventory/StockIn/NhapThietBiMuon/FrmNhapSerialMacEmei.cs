using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bll.Inventory.InventoryManagement;
using Bll.Inventory.StockInOut;
using Common.Common;
using Common.Enums;
using Common.Utils;
using DevExpress.XtraEditors;
using DTO.Inventory.InventoryManagement;
using DTO.Inventory.StockIn.NhapHangThuongMai;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;

namespace Inventory.StockIn.NhapThietBiMuon
{
    /// <summary>
    /// Form nhập định danh thiết bị (Serial Number, MAC Address, IMEI, etc.) cho phiếu nhập/xuất kho.
    /// Cung cấp chức năng nhập thông tin định danh cho các thiết bị trong phiếu nhập/xuất kho.
    /// </summary>
    public partial class FrmNhapSerialMacEmei : XtraForm
    {
        #region ========== FIELDS & PROPERTIES ==========

        /// <summary>
        /// ID phiếu nhập/xuất kho
        /// </summary>
        public Guid StockInOutMasterId { get; private set; }

        /// <summary>
        /// Business Logic Layer cho nhập quản lý tài sản thiết bị
        /// </summary>
        private readonly DeviceBll _deviceBll = new DeviceBll();

        /// <summary>
        /// Business Logic Layer cho phiếu nhập kho
        /// </summary>
        private readonly StockInOutBll _stockInBll = new StockInOutBll();

        /// <summary>
        /// Logger để ghi log các sự kiện
        /// </summary>
        private readonly ILogger _logger = LoggerFactory.CreateLogger(LogCategory.UI);

        /// <summary>
        /// Trạng thái có thay đổi dữ liệu chưa lưu
        /// </summary>
        private bool _hasUnsavedChanges;

        #endregion

        #region ========== CONSTRUCTOR ==========

        /// <summary>
        /// Constructor với StockInOutMasterId
        /// </summary>
        /// <param name="stockInOutMasterId">ID phiếu nhập/xuất kho</param>
        public FrmNhapSerialMacEmei(Guid stockInOutMasterId)
        {
            InitializeComponent();
            StockInOutMasterId = stockInOutMasterId;
            Load += FrmWarranty_Load;
        }

        #endregion

        #region ========== INITIALIZATION ==========

        /// <summary>
        /// Event handler khi form được load
        /// </summary>
        private async void FrmWarranty_Load(object sender, EventArgs e)
        {
            try
            {
                _logger.Debug("FrmWarranty_Load: Form loading, StockInOutMasterId={0}", StockInOutMasterId);

                // Đánh dấu các trường bắt buộc nhập
                MarkRequiredFields();

                // Setup SuperToolTips
                SetupSuperToolTips();

                // Setup events
                SetupEvents();

                // Load datasource với SplashScreen
                await LoadDataSourcesAsync();

                // Load danh sách thiết bị hiện có
                await LoadDevicesAsync();

                // Reset form về trạng thái ban đầu
                ResetForm();

                _logger.Info("FrmWarranty_Load: Form loaded successfully");
            }
            catch (Exception ex)
            {
                _logger.Error("FrmWarranty_Load: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi khởi tạo form: {ex.Message}");
            }
        }

        /// <summary>
        /// Load tất cả datasource cho các controls với SplashScreen
        /// </summary>
        private async Task LoadDataSourcesAsync()
        {
            try
            {
                _logger.Debug("LoadDataSourcesAsync: Starting to load datasources");

                // Hiển thị SplashScreen
                SplashScreenHelper.ShowWaitingSplashScreen();

                try
                {
                    // Load danh sách StockInOutDetail để hiển thị trong SearchLookUpEdit
                    await Task.Run(() =>
                    {
                        var details = _stockInBll.GetDetailsByMasterId(StockInOutMasterId);

                        // Sử dụng converter trong DTO - chỉ định rõ StockInOutProductHistoryDtoConverter
                        var detailDtos = StockInOutProductHistoryDtoConverter.ToDtoList(details);

                        // Update UI thread
                        BeginInvoke(new Action(() =>
                        {
                            stockInOutProductHistoryDtoBindingSource.DataSource = detailDtos;
                            stockInOutProductHistoryDtoBindingSource.ResetBindings(false);

                            // Khởi tạo DeviceIdentifierComboBoxEdit với danh sách enum
                            DeviceIdentifierComboBoxEdit.Properties.Items.Clear();
                            foreach (DeviceIdentifierEnum identifierType in Enum.GetValues(typeof(DeviceIdentifierEnum)))
                            {
                                DeviceIdentifierComboBoxEdit.Properties.Items.Add(identifierType);
                            }
                        }));
                    });

                    _logger.Info("LoadDataSourcesAsync: All datasources loaded successfully");
                }
                finally
                {
                    // Đóng SplashScreen
                    SplashScreenHelper.CloseSplashScreen();
                }
            }
            catch (Exception ex)
            {
                _logger.Error("LoadDataSourcesAsync: Exception occurred", ex);
                SplashScreenHelper.CloseSplashScreen();
                MsgBox.ShowError($"Lỗi tải dữ liệu: {ex.Message}");
            }
        }

        /// <summary>
        /// Load danh sách thiết bị hiện có
        /// </summary>
        private async Task LoadDevicesAsync()
        {
            try
            {
                _logger.Debug("LoadDevicesAsync: Loading devices, StockInOutMasterId={0}", StockInOutMasterId);

                await Task.Run(() =>
                {
                    // Lấy danh sách Device từ BLL
                    var devices = _deviceBll.GetByStockInOutMasterId(StockInOutMasterId);

                    // Convert sang DTO
                    var deviceDtos = devices.ToDtoList();

                    // Update UI thread
                    BeginInvoke(new Action(() =>
                    {
                        deviceDtoBindingSource.DataSource = deviceDtos;
                        deviceDtoBindingSource.ResetBindings(false);
                    }));
                });

                _logger.Info("LoadDevicesAsync: Loaded devices successfully");
            }
            catch (Exception ex)
            {
                _logger.Error("LoadDevicesAsync: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi tải danh sách thiết bị: {ex.Message}");
            }
        }


        /// <summary>
        /// Setup các event handlers
        /// </summary>
        private void SetupEvents()
        {
            try
            {
                // Bar button events
                SaveBarButtonItem.ItemClick += SaveBarButtonItem_ItemClick;
                CloseBarButtonItem.ItemClick += CloseBarButtonItem_ItemClick;

                // Hyperlink events
                ThemVaoHyperlinkLabelControl.Click += ThemVaoHyperlinkLabelControl_Click;
                BoRaHyperlinkLabelControl.Click += BoRaHyperlinkLabelControl_Click;

                // Grid events
                WarrantyDtoGridView.RowClick += WarrantyDtoGridView_RowClick;

                // ComboBox events
                DeviceIdentifierComboBoxEdit.EditValueChanged += DeviceIdentifierComboBoxEdit_EditValueChanged;

                // SearchLookUpEdit events - clear lỗi khi thay đổi
                StockInOutDetailIdSearchLookUpEdit.EditValueChanged += StockInOutDetailIdSearchLookUpEdit_EditValueChanged;

                // Text edit events - ENTER key để thêm vào grid và clear lỗi khi thay đổi
                UniqueProductInfoTextEdit.EditValueChanged += UniqueProductInfoTextEdit_EditValueChanged;
                UniqueProductInfoTextEdit.KeyDown += UniqueProductInfoTextEdit_KeyDown;

                // Setup phím tắt
                SetupKeyboardShortcuts();
                UpdateHotKeyBarStaticItem();

                _logger.Debug("SetupEvents: Events setup completed");
            }
            catch (Exception ex)
            {
                _logger.Error("SetupEvents: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi setup events: {ex.Message}");
            }
        }

        /// <summary>
        /// Thiết lập phím tắt cho các nút chức năng
        /// </summary>
        private void SetupKeyboardShortcuts()
        {
            try
            {
                KeyDown += FrmWarranty_KeyDown;
                KeyPreview = true;

                _logger.Debug("SetupKeyboardShortcuts: Keyboard shortcuts configured");
            }
            catch (Exception ex)
            {
                _logger.Error("SetupKeyboardShortcuts: Exception occurred", ex);
            }
        }

        /// <summary>
        /// Cập nhật nội dung HotKeyBarStaticItem để hiển thị hướng dẫn phím tắt
        /// </summary>
        private void UpdateHotKeyBarStaticItem()
        {
            try
            {
                if (FormHotKeyBarStaticItem == null) return;

                var hotKeyText = @"<color=Gray>Phím tắt:</color> " +
                    @"<b><color=Blue>F2</color></b> Lưu | " +
                    @"<b><color=Blue>F3</color></b> Thêm vào | " +
                    @"<b><color=Blue>F4</color></b> Bỏ ra | " +
                    @"<b><color=Blue>ESC</color></b> Đóng";

                FormHotKeyBarStaticItem.Caption = hotKeyText;
                FormHotKeyBarStaticItem.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;

                _logger.Debug("UpdateHotKeyBarStaticItem: Hot key bar updated");
            }
            catch (Exception ex)
            {
                _logger.Error("UpdateHotKeyBarStaticItem: Exception occurred", ex);
            }
        }

        /// <summary>
        /// Reset form về trạng thái ban đầu
        /// </summary>
        private void ResetForm()
        {
            try
            {
                _logger.Debug("ResetForm: Resetting form to initial state");

                // Clear input controls
                StockInOutDetailIdSearchLookUpEdit.EditValue = null;
                DeviceIdentifierComboBoxEdit.EditValue = null;
                UniqueProductInfoTextEdit.EditValue = null;

                // Reset state
                MarkAsSaved();

                _logger.Info("ResetForm: Form reset successfully");
            }
            catch (Exception ex)
            {
                _logger.Error("ResetForm: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi reset form: {ex.Message}");
            }
        }

        /// <summary>
        /// Đánh dấu form có thay đổi chưa lưu
        /// </summary>
        private void MarkAsChanged()
        {
            _hasUnsavedChanges = true;
            _logger.Debug("MarkAsChanged: Form marked as having unsaved changes");
        }

        /// <summary>
        /// Đánh dấu form đã lưu (không còn thay đổi)
        /// </summary>
        private void MarkAsSaved()
        {
            _hasUnsavedChanges = false;
            _logger.Debug("MarkAsSaved: Form marked as saved");
        }

        /// <summary>
        /// Đánh dấu các trường bắt buộc nhập
        /// </summary>
        private void MarkRequiredFields()
        {
            try
            {
                RequiredFieldHelper.MarkRequiredFields(
                    this,
                    typeof(DeviceDto),
                    logger: (msg, ex) => _logger?.Error($"{msg}: {ex?.Message}")
                );

                _logger.Debug("MarkRequiredFields: Required fields marked successfully");
            }
            catch (Exception ex)
            {
                _logger.Error("MarkRequiredFields: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi đánh dấu trường bắt buộc: {ex.Message}");
            }
        }

        /// <summary>
        /// Thiết lập SuperToolTip cho tất cả các controls trong Form
        /// </summary>
        private void SetupSuperToolTips()
        {
            try
            {
                SetupTextEditSuperTips();
                SetupSearchLookupEditSuperTips();

                _logger.Debug("SetupSuperToolTips: SuperToolTips setup completed");
            }
            catch (Exception ex)
            {
                _logger.Error("SetupSuperToolTips: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi thiết lập SuperToolTip: {ex.Message}");
            }
        }

        /// <summary>
        /// Thiết lập SuperToolTip cho các TextEdit controls
        /// </summary>
        private void SetupTextEditSuperTips()
        {
            // SuperTip cho Thông tin định danh thiết bị
            if (UniqueProductInfoTextEdit != null)
            {
                SuperToolTipHelper.SetTextEditSuperTip(
                    UniqueProductInfoTextEdit,
                    title: @"<b><color=DarkBlue>🔢 Giá trị định danh</color></b>",
                    content: @"Nhập giá trị định danh thiết bị (Serial Number, MAC Address, IMEI, Asset Tag, License Key) để xác định thiết bị cụ thể.<br/><br/><b>Chức năng:</b><br/>• Xác định thiết bị cụ thể trong hệ thống<br/>• Tra cứu thông tin thiết bị theo định danh<br/>• Đảm bảo tính duy nhất của thiết bị<br/><br/><b>Ràng buộc:</b><br/>• <b>Bắt buộc nhập</b> (có dấu * đỏ)<br/>• Không được để trống<br/>• Tối đa 255 ký tự<br/>• Phải duy nhất (không được trùng lặp trong danh sách)<br/><br/><b>Validation:</b><br/>• Kiểm tra rỗng khi validating<br/>• Kiểm tra trùng lặp khi thêm vào grid<br/>• Kiểm tra trùng lặp khi lưu<br/>• Hiển thị lỗi qua ErrorProvider nếu không hợp lệ<br/><br/><b>Phím tắt:</b><br/>• Nhấn <b>ENTER</b> để thêm vào danh sách thiết bị<br/><br/><color=Gray>Lưu ý:</color> Thông tin này sẽ được lưu vào database khi lưu thiết bị."
                );
            }
        }

        /// <summary>
        /// Thiết lập SuperToolTip cho các SearchLookUpEdit controls
        /// </summary>
        private void SetupSearchLookupEditSuperTips()
        {
            // SuperTip cho Sản phẩm thiết bị
            if (StockInOutDetailIdSearchLookUpEdit != null)
            {
                SuperToolTipHelper.SetBaseEditSuperTip(
                    StockInOutDetailIdSearchLookUpEdit,
                    title: @"<b><color=DarkBlue>📦 Sản phẩm thiết bị</color></b>",
                    content: @"Chọn sản phẩm từ danh sách chi tiết phiếu nhập/xuất kho để nhập định danh thiết bị.<br/><br/><b>Chức năng:</b><br/>• Chọn sản phẩm từ danh sách chi tiết phiếu nhập/xuất kho<br/>• Hiển thị thông tin sản phẩm dạng HTML (mã, tên, đơn vị tính, số lượng)<br/>• Tự động cập nhật StockInOutDetailId vào DTO<br/><br/><b>Ràng buộc:</b><br/>• <b>Bắt buộc chọn</b> (có dấu * đỏ)<br/>• Không được để trống<br/>• Chỉ hiển thị các sản phẩm trong phiếu nhập/xuất kho hiện tại<br/><br/><b>Data Source:</b><br/>• Load từ StockInBll.GetDetailsByMasterId()<br/>• Filter theo StockInOutMasterId<br/>• Hiển thị thông tin sản phẩm dạng HTML (ProductVariantFullNameHtml)<br/><br/><b>Validation:</b><br/>• Kiểm tra rỗng khi validating<br/>• Hiển thị lỗi qua ErrorProvider nếu không hợp lệ<br/><br/><color=Gray>Lưu ý:</color> Sản phẩm được chọn sẽ được lưu vào database khi lưu thiết bị."
                );
            }
        }

        #endregion

        #region ========== EVENT HANDLERS ==========

        /// <summary>
        /// Event handler cho nút Lưu
        /// </summary>
        private async void SaveBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                _logger.Debug("SaveBarButtonItem_ItemClick: Save button clicked");

                // Disable button để tránh double-click
                SaveBarButtonItem.Enabled = false;

                try
                {
                    // Validate và lưu dữ liệu
                    var success = await SaveDataAsync();

                    if (!success) return;

                    MsgBox.ShowSuccess("Lưu định danh thiết bị thành công!", "Thành công", this);
                    MarkAsSaved();
                    await LoadDevicesAsync(); // Reload danh sách
                    ResetForm(); // Reset form
                    _logger.Info("SaveBarButtonItem_ItemClick: Data saved successfully");
                }
                finally
                {
                    // Re-enable button
                    SaveBarButtonItem.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                _logger.Error("SaveBarButtonItem_ItemClick: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi lưu định danh thiết bị: {ex.Message}");
            }
        }

        /// <summary>
        /// Event handler cho nút Đóng
        /// </summary>
        private void CloseBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                _logger.Debug("CloseBarButtonItem_ItemClick: Close button clicked");

                // Kiểm tra có thay đổi chưa lưu không
                if (_hasUnsavedChanges)
                {
                    var result = MsgBox.ShowYesNoCancel(
                        "Bạn có thay đổi chưa lưu. Bạn muốn làm gì?",
                        "Xác nhận đóng",
                        this,
                        yesButtonText: "Lưu và đóng",
                        noButtonText: "Đóng không lưu",
                        cancelButtonText: "Hủy");

                    if (result == DialogResult.Yes)
                    {
                        // Lưu và đóng
                        SaveBarButtonItem_ItemClick(null, null);
                        Close();
                    }
                    else if (result == DialogResult.No)
                    {
                        // Đóng không lưu
                        Close();
                    }
                    // Cancel: giữ form mở
                }
                else
                {
                    Close();
                }
            }
            catch (Exception ex)
            {
                _logger.Error("CloseBarButtonItem_ItemClick: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi đóng form: {ex.Message}");
            }
        }

        /// <summary>
        /// Event handler cho nút Thêm vào
        /// </summary>
        private void ThemVaoHyperlinkLabelControl_Click(object sender, EventArgs e)
        {
            try
            {
                _logger.Debug("ThemVaoHyperlinkLabelControl_Click: Add button clicked");

                // Validate input
                if (!ValidateInput())
                {
                    return;
                }

                // Lấy giá trị từ controls
                var stockInOutDetailId = StockInOutDetailIdSearchLookUpEdit.EditValue as Guid?;
                if (!stockInOutDetailId.HasValue || stockInOutDetailId.Value == Guid.Empty)
                {
                    MsgBox.ShowWarning("Vui lòng chọn sản phẩm thiết bị", "Cảnh báo", this);
                    return;
                }

                var deviceIdentifier = DeviceIdentifierComboBoxEdit.EditValue as DeviceIdentifierEnum?;
                if (!deviceIdentifier.HasValue)
                {
                    MsgBox.ShowWarning("Vui lòng chọn loại định danh", "Cảnh báo", this);
                    return;
                }

                var identifierValue = UniqueProductInfoTextEdit.EditValue?.ToString()?.Trim() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(identifierValue))
                {
                    MsgBox.ShowWarning("Vui lòng nhập giá trị định danh", "Cảnh báo", this);
                    return;
                }

                // Lấy thông tin sản phẩm từ selected detail
                StockInOutProductHistoryDto selectedDetail = null;
                if (stockInOutProductHistoryDtoBindingSource.Current is StockInOutProductHistoryDto detail)
                {
                    selectedDetail = detail;
                }

                if (selectedDetail == null)
                {
                    MsgBox.ShowWarning("Không tìm thấy thông tin sản phẩm", "Cảnh báo", this);
                    return;
                }

                // Tạo DeviceDto mới
                var deviceDto = new DeviceDto
                {
                    Id = Guid.NewGuid(),
                    ProductVariantId = selectedDetail.ProductVariantId,
                    StockInOutDetailId = stockInOutDetailId.Value,
                    ProductVariantName = selectedDetail.ProductVariantFullName
                };

                // Đặt giá trị định danh theo loại đã chọn
                deviceDto.SetIdentifierValue(deviceIdentifier.Value, identifierValue);

                // Thêm vào grid
                if (deviceDtoBindingSource.DataSource is not List<DeviceDto> currentList)
                {
                    currentList = new List<DeviceDto>();
                    deviceDtoBindingSource.DataSource = currentList;
                }

                currentList.Add(deviceDto);
                deviceDtoBindingSource.ResetBindings(false);

                WarrantyDtoGridView.ExpandAllGroups();

                // Đánh dấu có thay đổi
                MarkAsChanged();

                // Clear input controls
                UniqueProductInfoTextEdit.Text = string.Empty;

                _logger.Info("ThemVaoHyperlinkLabelControl_Click: Device added to grid, Id={0}", deviceDto.Id);
            }
            catch (Exception ex)
            {
                _logger.Error("ThemVaoHyperlinkLabelControl_Click: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi thêm bảo hành: {ex.Message}");
            }
        }

        /// <summary>
        /// Event handler cho nút Bỏ ra
        /// </summary>
        private void BoRaHyperlinkLabelControl_Click(object sender, EventArgs e)
        {
            try
            {
                _logger.Debug("BoRaHyperlinkLabelControl_Click: Remove button clicked");

                var selectedRow = WarrantyDtoGridView.GetFocusedRow() as DeviceDto;
                if (selectedRow == null)
                {
                    MsgBox.ShowWarning("Vui lòng chọn dòng cần xóa", "Cảnh báo", this);
                    return;
                }

                var result = MsgBox.ShowYesNo(
                    "Bạn có chắc chắn muốn xóa thiết bị này?",
                    "Xác nhận xóa",
                    this);

                if (!result) return;

                if (deviceDtoBindingSource.DataSource is not List<DeviceDto> currentList) return;

                currentList.Remove(selectedRow);
                deviceDtoBindingSource.ResetBindings(false);
                MarkAsChanged();
                _logger.Info("BoRaHyperlinkLabelControl_Click: Device removed from grid, Id={0}", selectedRow.Id);
            }
            catch (Exception ex)
            {
                _logger.Error("BoRaHyperlinkLabelControl_Click: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi xóa bảo hành: {ex.Message}");
            }
        }

        /// <summary>
        /// Event handler khi click vào row trong grid
        /// </summary>
        private void WarrantyDtoGridView_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            try
            {
                if (WarrantyDtoGridView.GetRow(e.RowHandle) is not DeviceDto selectedRow) return;

                // Load thông tin vào input controls
                StockInOutDetailIdSearchLookUpEdit.EditValue = selectedRow.StockInOutDetailId;

                // Tìm loại định danh có giá trị và hiển thị
                var identifiers = selectedRow.GetAllIdentifiers();
                if (identifiers.Any())
                {
                    var identifier = identifiers.First();
                    DeviceIdentifierComboBoxEdit.EditValue = identifier.Key;
                    UniqueProductInfoTextEdit.EditValue = identifier.Value;
                }
                else
                {
                    DeviceIdentifierComboBoxEdit.EditValue = null;
                    UniqueProductInfoTextEdit.EditValue = null;
                }
            }
            catch (Exception ex)
            {
                _logger.Error("WarrantyDtoGridView_RowClick: Exception occurred", ex);
            }
        }

        /// <summary>
        /// Event handler khi thay đổi loại định danh
        /// </summary>
        private void DeviceIdentifierComboBoxEdit_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                // Clear lỗi nếu có
                if (DeviceIdentifierComboBoxEdit.EditValue is DeviceIdentifierEnum)
                {
                    dxErrorProvider1.SetError(DeviceIdentifierComboBoxEdit, string.Empty);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("DeviceIdentifierComboBoxEdit_EditValueChanged: Exception occurred", ex);
            }
        }

        /// <summary>
        /// Event handler khi thay đổi giá trị trong UniqueProductInfoTextEdit - clear lỗi nếu giá trị hợp lệ
        /// </summary>
        private void UniqueProductInfoTextEdit_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                // Validate và clear lỗi nếu giá trị hợp lệ
                var uniqueProductInfo = UniqueProductInfoTextEdit.EditValue?.ToString().Trim() ?? string.Empty;
                if (!string.IsNullOrWhiteSpace(uniqueProductInfo))
                {
                    // Giá trị hợp lệ, clear lỗi
                    dxErrorProvider1.SetError(UniqueProductInfoTextEdit, string.Empty);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("UniqueProductInfoTextEdit_EditValueChanged: Exception occurred", ex);
            }
        }

        /// <summary>
        /// Event handler khi thay đổi giá trị trong StockInOutDetailIdSearchLookUpEdit - clear lỗi nếu giá trị hợp lệ
        /// </summary>
        private void StockInOutDetailIdSearchLookUpEdit_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                // Validate và clear lỗi nếu giá trị hợp lệ
                if (StockInOutDetailIdSearchLookUpEdit.EditValue is Guid stockInOutDetailId && stockInOutDetailId != Guid.Empty)
                {
                    // Giá trị hợp lệ, clear lỗi
                    dxErrorProvider1.SetError(StockInOutDetailIdSearchLookUpEdit, string.Empty);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("StockInOutDetailIdSearchLookUpEdit_EditValueChanged: Exception occurred", ex);
            }
        }

        /// <summary>
        /// Event handler khi nhấn phím trong UniqueProductInfoTextEdit - ENTER để thêm vào grid
        /// </summary>
        private void UniqueProductInfoTextEdit_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                // Khi nhấn ENTER, gọi ThemVaoHyperlinkLabelControl_Click
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    e.SuppressKeyPress = true; // Ngăn không cho phát ra tiếng beep

                    _logger.Debug("UniqueProductInfoTextEdit_KeyDown: Enter key pressed, calling ThemVaoHyperlinkLabelControl_Click");
                    ThemVaoHyperlinkLabelControl_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("UniqueProductInfoTextEdit_KeyDown: Exception occurred", ex);
            }
        }

        /// <summary>
        /// Event handler xử lý phím tắt
        /// </summary>
        private void FrmWarranty_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                // Chỉ xử lý phím tắt khi không đang edit trong control
                if (ActiveControl is BaseEdit { IsEditorActive: true })
                {
                    // Nếu đang edit trong control, chỉ xử lý một số phím đặc biệt
                    // Các phím khác sẽ được xử lý bởi control đó
                    return;
                }

                switch (e.KeyCode)
                {
                    case Keys.F2:
                        // F2: Lưu
                        e.Handled = true;
                        SaveBarButtonItem_ItemClick(null, null);
                        break;

                    case Keys.F3:
                        // F3: Thêm vào
                        e.Handled = true;
                        ThemVaoHyperlinkLabelControl_Click(null, null);
                        break;

                    case Keys.F4:
                        // F4: Bỏ ra
                        e.Handled = true;
                        BoRaHyperlinkLabelControl_Click(null, null);
                        break;

                    case Keys.Escape:
                        // ESC: Đóng form
                        e.Handled = true;
                        CloseBarButtonItem_ItemClick(null, null);
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.Error("FrmWarranty_KeyDown: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi xử lý phím tắt: {ex.Message}");
            }
        }

        #endregion

        #region ========== DATA OPERATIONS ==========

        /// <summary>
        /// Validate input trước khi thêm vào grid
        /// </summary>
        private bool ValidateInput()
        {
            try
            {
                // Validate StockInOutDetailId
                if (StockInOutDetailIdSearchLookUpEdit.EditValue is not Guid stockInOutDetailId || stockInOutDetailId == Guid.Empty)
                {
                    dxErrorProvider1.SetError(StockInOutDetailIdSearchLookUpEdit, "Vui lòng chọn sản phẩm bảo hành");
                    return false;
                }
                dxErrorProvider1.SetError(StockInOutDetailIdSearchLookUpEdit, string.Empty);

                // Validate DeviceIdentifier
                var deviceIdentifier = DeviceIdentifierComboBoxEdit.EditValue as DeviceIdentifierEnum?;
                if (!deviceIdentifier.HasValue)
                {
                    dxErrorProvider1.SetError(DeviceIdentifierComboBoxEdit, "Vui lòng chọn loại định danh");
                    return false;
                }
                dxErrorProvider1.SetError(DeviceIdentifierComboBoxEdit, string.Empty);

                // Validate UniqueProductInfo - đảm bảo duy nhất không được trùng lặp
                var identifierValue = UniqueProductInfoTextEdit.EditValue?.ToString()?.Trim() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(identifierValue))
                {
                    dxErrorProvider1.SetError(UniqueProductInfoTextEdit, "Vui lòng nhập giá trị định danh");
                    return false;
                }

                // Kiểm tra trùng lặp trong grid hiện tại (case-insensitive)
                // Nếu đang edit một dòng (có dòng được chọn trong grid), bỏ qua dòng đó khi kiểm tra trùng lặp
                if (deviceDtoBindingSource.DataSource is List<DeviceDto> currentList && currentList.Any())
                {
                    // Lấy dòng đang được chọn trong grid (nếu có)
                    DeviceDto selectedRow = null;
                    var focusedRowHandle = WarrantyDtoGridView.FocusedRowHandle;
                    if (focusedRowHandle >= 0)
                    {
                        selectedRow = WarrantyDtoGridView.GetRow(focusedRowHandle) as DeviceDto;
                    }

                    // Kiểm tra trùng lặp, bỏ qua dòng đang được chọn (nếu đang edit)
                    var isDuplicate = currentList.Any(d =>
                        d != selectedRow && // Bỏ qua dòng đang được chọn nếu đang edit
                        d.GetIdentifierValue(deviceIdentifier.Value) != null &&
                        string.Equals(d.GetIdentifierValue(deviceIdentifier.Value).Trim(), identifierValue, StringComparison.OrdinalIgnoreCase));

                    if (isDuplicate)
                    {
                        dxErrorProvider1.SetError(UniqueProductInfoTextEdit, "Giá trị định danh đã tồn tại trong danh sách. Vui lòng nhập giá trị khác.");
                        _logger.Warning("ValidateInput: Duplicate identifier value detected, type={0}, value={1}", deviceIdentifier.Value, identifierValue);
                        return false;
                    }
                }

                dxErrorProvider1.SetError(UniqueProductInfoTextEdit, string.Empty);

                return true;
            }
            catch (Exception ex)
            {
                _logger.Error("ValidateInput: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi validation: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Lưu dữ liệu async
        /// </summary>
        private async Task<bool> SaveDataAsync()
        {
            try
            {
                _logger.Debug("SaveDataAsync: Starting save operation");

                // Lấy danh sách DeviceDto từ grid
                var deviceDtos = Enumerable.Cast<DeviceDto>(deviceDtoBindingSource).ToList();
                if (deviceDtos.Count == 0)
                {
                    MsgBox.ShowWarning("Không có thiết bị nào để lưu", "Cảnh báo", this);
                    return false;
                }

                // Validate tất cả các dòng
                var validationErrors = new List<string>();
                for (int i = 0; i < deviceDtos.Count; i++)
                {
                    var device = deviceDtos[i];
                    if (!device.StockInOutDetailId.HasValue || device.StockInOutDetailId.Value == Guid.Empty)
                    {
                        validationErrors.Add($"Dòng {i + 1}: Vui lòng chọn sản phẩm thiết bị");
                    }
                    if (device.ProductVariantId == Guid.Empty)
                    {
                        validationErrors.Add($"Dòng {i + 1}: Thiếu thông tin sản phẩm");
                    }

                    // Kiểm tra có ít nhất một định danh
                    var identifiers = device.GetAllIdentifiers();
                    if (!identifiers.Any())
                    {
                        validationErrors.Add($"Dòng {i + 1}: Vui lòng nhập ít nhất một giá trị định danh");
                    }
                }

                // Kiểm tra trùng lặp định danh (case-insensitive) - kiểm tra từng loại định danh
                foreach (DeviceIdentifierEnum identifierType in Enum.GetValues(typeof(DeviceIdentifierEnum)))
                {
                    var devicesWithIdentifier = deviceDtos
                        .Where(d => !string.IsNullOrWhiteSpace(d.GetIdentifierValue(identifierType)))
                        .ToList();

                    if (devicesWithIdentifier.Count > 1)
                    {
                        var duplicateGroups = devicesWithIdentifier
                            .GroupBy(d => d.GetIdentifierValue(identifierType).Trim(), StringComparer.OrdinalIgnoreCase)
                            .Where(g => g.Count() > 1)
                            .ToList();

                        if (duplicateGroups.Any())
                        {
                            foreach (var group in duplicateGroups)
                            {
                                var duplicateIndices = deviceDtos
                                    .Select((d, index) => new { d, index })
                                    .Where(x => string.Equals(x.d.GetIdentifierValue(identifierType)?.Trim(), group.Key, StringComparison.OrdinalIgnoreCase))
                                    .Select(x => x.index + 1)
                                    .ToList();

                                validationErrors.Add($"{identifierType} '{group.Key}' bị trùng lặp ở các dòng: {string.Join(", ", duplicateIndices)}");
                            }
                        }
                    }
                }

                if (validationErrors.Any())
                {
                    MsgBox.ShowError($"Có lỗi trong dữ liệu:\n\n{string.Join("\n", validationErrors)}", "Lỗi validation", this);
                    return false;
                }

                // Lưu từng thiết bị qua BLL
                await Task.Run(() =>
                {
                    foreach (var deviceDto in deviceDtos)
                    {
                        // Convert DTO sang Entity
                        var device = deviceDto.ToEntity();

                        // Lưu qua BLL
                        _deviceBll.SaveOrUpdate(device);
                    }
                });

                _logger.Info("SaveDataAsync: Save operation completed successfully");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error("SaveDataAsync: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi lưu dữ liệu: {ex.Message}");
                return false;
            }
        }

        #endregion
    }
}
