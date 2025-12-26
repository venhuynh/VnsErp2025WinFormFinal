using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bll.Inventory.InventoryManagement;
using Bll.Inventory.StockInOut;
using Bll.MasterData.ProductServiceBll;
using Common.Common;
using Common.Enums;
using Common.Utils;
using DevExpress.XtraEditors;
using DTO.DeviceAssetManagement;
using DTO.Inventory.InventoryManagement;
using DTO.Inventory.StockIn.NhapHangThuongMai;
using DTO.MasterData.ProductService;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;

namespace Inventory.OverlayForm;

/// <summary>
/// Form nhập bảo hành cho phiếu nhập/xuất kho.
/// Cung cấp chức năng nhập thông tin bảo hành cho các sản phẩm trong phiếu nhập/xuất kho.
/// </summary>
public partial class FrmWarranty : XtraForm
{
    #region ========== FIELDS & PROPERTIES ==========

    /// <summary>
    /// ID phiếu nhập/xuất kho
    /// </summary>
    public Guid StockInOutMasterId { get; private set; }

    /// <summary>
    /// Business Logic Layer cho bảo hành
    /// </summary>
    private readonly WarrantyBll _warrantyBll = new WarrantyBll();

    /// <summary>
    /// Business Logic Layer cho phiếu nhập kho
    /// </summary>
    private readonly StockInOutBll _stockInBll = new StockInOutBll();

    /// <summary>
    /// Business Logic Layer cho ProductVariant
    /// </summary>
    private readonly ProductVariantBll _productVariantBll = new ProductVariantBll();

    /// <summary>
    /// Business Logic Layer cho Device
    /// </summary>
    private readonly DeviceBll _deviceBll = new DeviceBll();

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
    public FrmWarranty(Guid stockInOutMasterId)
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

            // Load danh sách bảo hành hiện có
            await LoadWarrantiesAsync();

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
                // Load danh sách StockInDetail để hiển thị trong SearchLookUpEdit
                await Task.Run(() =>
                {
                    var details = _stockInBll.GetDetailsByMasterId(StockInOutMasterId);
                        
                    // Sử dụng converter trong DTO - chỉ định rõ StockInDetailDtoConverter
                    var detailDtos = StockInDetailDtoConverter.ToDtoList(details);
                        
                    // Update UI thread
                    BeginInvoke(new Action(() =>
                    {
                        stockInDetailDtoBindingSource.DataSource = detailDtos;
                        stockInDetailDtoBindingSource.ResetBindings(false);
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
    /// Load danh sách bảo hành hiện có
    /// </summary>
    private async Task LoadWarrantiesAsync()
    {
        try
        {
            _logger.Debug("LoadWarrantiesAsync: Loading warranties, StockInOutMasterId={0}", StockInOutMasterId);

            await Task.Run(() =>
            {
                var warranties = _warrantyBll.GetByStockInOutMasterId(StockInOutMasterId);
                var warrantyDtos = warranties.Select(w => w.ToDto()).ToList();

                // Update UI thread
                BeginInvoke(new Action(() =>
                {
                    warrantyDtoBindingSource.DataSource = warrantyDtos;
                    warrantyDtoBindingSource.ResetBindings(false);
                }));
            });

            _logger.Info("LoadWarrantiesAsync: Loaded warranties successfully");
        }
        catch (Exception ex)
        {
            _logger.Error("LoadWarrantiesAsync: Exception occurred", ex);
            MsgBox.ShowError($"Lỗi tải danh sách bảo hành: {ex.Message}");
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

            // Date and numeric edit events
            MonthOfWarrantyTextEdit.EditValueChanged += MonthOfWarrantyTextEdit_EditValueChanged;
            WarrantyFromDateEdit.EditValueChanged += WarrantyFromDateEdit_EditValueChanged;

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
            WarrantyFromDateEdit.EditValue = DateTime.Now; // Set ngày hiện tại khi khởi tạo
            MonthOfWarrantyTextEdit.EditValue = null;
            WarrantyUntilDateEdit.EditValue = null;
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
                typeof(WarrantyDto),
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
            SetupDateEditSuperTips();
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
        // SuperTip cho Số tháng bảo hành
        if (MonthOfWarrantyTextEdit != null)
        {
            SuperToolTipHelper.SetTextEditSuperTip(
                MonthOfWarrantyTextEdit,
                title: @"<b><color=DarkBlue>📅 Số tháng bảo hành</color></b>",
                content: @"Nhập số tháng bảo hành cho sản phẩm.<br/><br/><b>Chức năng:</b><br/>• Xác định thời gian bảo hành (tính bằng tháng)<br/>• Tự động tính toán ngày kết thúc bảo hành dựa trên ngày bắt đầu<br/>• Format: Số nguyên dương (N0)<br/><br/><b>Ràng buộc:</b><br/>• <b>Bắt buộc nhập</b> (có dấu * đỏ)<br/>• Phải lớn hơn 0<br/>• Không được để trống<br/><br/><b>Validation:</b><br/>• Kiểm tra rỗng khi validating<br/>• Hiển thị lỗi qua ErrorProvider nếu không hợp lệ<br/><br/><color=Gray>Lưu ý:</color> Khi thay đổi số tháng bảo hành, hệ thống sẽ tự động cập nhật ngày kết thúc bảo hành."
            );
        }

        // SuperTip cho Thông tin sản phẩm duy nhất
        if (UniqueProductInfoTextEdit != null)
        {
            SuperToolTipHelper.SetTextEditSuperTip(
                UniqueProductInfoTextEdit,
                title: @"<b><color=DarkBlue>🔢 Thông tin sản phẩm duy nhất</color></b>",
                content: @"Nhập thông tin sản phẩm duy nhất (Serial Number, IMEI, v.v.) để xác định sản phẩm cụ thể.<br/><br/><b>Chức năng:</b><br/>• Xác định sản phẩm cụ thể trong hệ thống<br/>• Tra cứu thông tin bảo hành theo serial/IMEI<br/>• Đảm bảo tính duy nhất của sản phẩm<br/><br/><b>Ràng buộc:</b><br/>• <b>Bắt buộc nhập</b> (có dấu * đỏ)<br/>• Không được để trống<br/>• Tối đa 200 ký tự<br/>• Phải duy nhất (không được trùng lặp trong danh sách)<br/><br/><b>Validation:</b><br/>• Kiểm tra rỗng khi validating<br/>• Kiểm tra trùng lặp khi thêm vào grid<br/>• Kiểm tra trùng lặp khi lưu<br/>• Hiển thị lỗi qua ErrorProvider nếu không hợp lệ<br/><br/><b>Phím tắt:</b><br/>• Nhấn <b>ENTER</b> để thêm vào danh sách bảo hành<br/><br/><color=Gray>Lưu ý:</color> Thông tin này sẽ được lưu vào database khi lưu bảo hành."
            );
        }
    }

    /// <summary>
    /// Thiết lập SuperToolTip cho các DateEdit controls
    /// </summary>
    private void SetupDateEditSuperTips()
    {
        // SuperTip cho Ngày bắt đầu bảo hành
        if (WarrantyFromDateEdit != null)
        {
            SuperToolTipHelper.SetBaseEditSuperTip(
                WarrantyFromDateEdit,
                title: @"<b><color=DarkBlue>📅 Ngày bắt đầu bảo hành</color></b>",
                content: @"Chọn ngày bắt đầu bảo hành cho sản phẩm.<br/><br/><b>Chức năng:</b><br/>• Xác định thời điểm bắt đầu bảo hành<br/>• Tự động tính toán ngày kết thúc bảo hành dựa trên số tháng bảo hành<br/>• Format: dd/MM/yyyy<br/><br/><b>Ràng buộc:</b><br/>• Không bắt buộc (có thể để trống)<br/>• Mặc định: Ngày hiện tại khi khởi tạo form<br/><br/><b>Validation:</b><br/>• Kiểm tra hợp lệ của ngày<br/>• Hiển thị lỗi qua ErrorProvider nếu không hợp lệ<br/><br/><color=Gray>Lưu ý:</color> Khi thay đổi ngày bắt đầu bảo hành, hệ thống sẽ tự động cập nhật ngày kết thúc bảo hành."
            );
        }

        // SuperTip cho Ngày kết thúc bảo hành
        if (WarrantyUntilDateEdit != null)
        {
            SuperToolTipHelper.SetBaseEditSuperTip(
                WarrantyUntilDateEdit,
                title: @"<b><color=DarkBlue>📅 Ngày kết thúc bảo hành</color></b>",
                content: @"Ngày kết thúc bảo hành được tính tự động dựa trên ngày bắt đầu và số tháng bảo hành.<br/><br/><b>Chức năng:</b><br/>• Hiển thị ngày kết thúc bảo hành<br/>• Tự động tính toán từ ngày bắt đầu + số tháng bảo hành<br/>• Format: dd/MM/yyyy<br/><br/><b>Ràng buộc:</b><br/>• Không bắt buộc (có thể để trống)<br/>• Tự động tính toán khi có đầy đủ thông tin<br/><br/><b>Tính toán:</b><br/>• WarrantyUntil = WarrantyFrom + MonthOfWarranty<br/>• Cập nhật tự động khi thay đổi WarrantyFrom hoặc MonthOfWarranty<br/><br/><color=Gray>Lưu ý:</color> Ngày này sẽ được lưu vào database khi lưu bảo hành."
            );
        }
    }

    /// <summary>
    /// Thiết lập SuperToolTip cho các SearchLookUpEdit controls
    /// </summary>
    private void SetupSearchLookupEditSuperTips()
    {
        // SuperTip cho Sản phẩm bảo hành
        if (StockInOutDetailIdSearchLookUpEdit != null)
        {
            SuperToolTipHelper.SetBaseEditSuperTip(
                StockInOutDetailIdSearchLookUpEdit,
                title: @"<b><color=DarkBlue>📦 Sản phẩm bảo hành</color></b>",
                content: @"Chọn sản phẩm từ danh sách chi tiết phiếu nhập/xuất kho để nhập bảo hành.<br/><br/><b>Chức năng:</b><br/>• Chọn sản phẩm từ danh sách chi tiết phiếu nhập/xuất kho<br/>• Hiển thị thông tin sản phẩm dạng HTML (mã, tên, đơn vị tính, số lượng, giá)<br/>• Tự động cập nhật StockInOutDetailId vào DTO<br/><br/><b>Ràng buộc:</b><br/>• <b>Bắt buộc chọn</b> (có dấu * đỏ)<br/>• Không được để trống<br/>• Chỉ hiển thị các sản phẩm trong phiếu nhập/xuất kho hiện tại<br/><br/><b>Data Source:</b><br/>• Load từ StockInBll.GetDetailsByMasterId()<br/>• Filter theo StockInOutMasterId<br/>• Hiển thị thông tin sản phẩm dạng HTML (FullNameHtml)<br/><br/><b>Validation:</b><br/>• Kiểm tra rỗng khi validating<br/>• Hiển thị lỗi qua ErrorProvider nếu không hợp lệ<br/><br/><color=Gray>Lưu ý:</color> Sản phẩm được chọn sẽ được lưu vào database khi lưu bảo hành."
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
                    
                MsgBox.ShowSuccess("Lưu bảo hành thành công!", "Thành công", this);
                MarkAsSaved();
                await LoadWarrantiesAsync(); // Reload danh sách
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
            MsgBox.ShowError($"Lỗi lưu bảo hành: {ex.Message}");
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
                MsgBox.ShowWarning("Vui lòng chọn sản phẩm bảo hành", "Cảnh báo", this);
                return;
            }

            var warrantyFrom = WarrantyFromDateEdit.EditValue as DateTime?;
            var monthOfWarranty = Convert.ToInt32(MonthOfWarrantyTextEdit.EditValue ?? 0);
            var warrantyUntil = WarrantyUntilDateEdit.EditValue as DateTime?;
            var deviceInfo = UniqueProductInfoTextEdit.EditValue?.ToString() ?? string.Empty;

            // Tìm hoặc tạo Device từ StockInOutDetailId và deviceInfo
            Guid? deviceId = null;
            if (!string.IsNullOrWhiteSpace(deviceInfo) && stockInOutDetailId.HasValue)
            {
                // Tìm Device theo StockInOutDetailId và deviceInfo (SerialNumber, IMEI, etc.)
                var devices = _deviceBll.GetByStockInOutDetailId(stockInOutDetailId.Value);
                var device = devices.FirstOrDefault(d => 
                    (!string.IsNullOrWhiteSpace(d.SerialNumber) && d.SerialNumber.Equals(deviceInfo, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrWhiteSpace(d.IMEI) && d.IMEI.Equals(deviceInfo, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrWhiteSpace(d.MACAddress) && d.MACAddress.Equals(deviceInfo, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrWhiteSpace(d.AssetTag) && d.AssetTag.Equals(deviceInfo, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrWhiteSpace(d.LicenseKey) && d.LicenseKey.Equals(deviceInfo, StringComparison.OrdinalIgnoreCase))
                );
                
                if (device != null)
                {
                    deviceId = device.Id;
                }
                // Nếu không tìm thấy, sẽ tạo Device mới khi save (xử lý trong SaveDataAsync)
            }

            // Tạo WarrantyDto mới
            var warrantyDto = new WarrantyDto
            {
                Id = Guid.NewGuid(),
                DeviceId = deviceId,
                WarrantyType = LoaiBaoHanhEnum.NCCToVNS, // Mặc định
                WarrantyFrom = warrantyFrom,
                MonthOfWarranty = monthOfWarranty,
                WarrantyUntil = warrantyUntil,
                WarrantyStatus = TrangThaiBaoHanhEnum.ChoXuLy, // Mặc định
                DeviceInfo = deviceInfo, // Lưu tạm thông tin device
                Notes = deviceInfo // Lưu vào Notes để backup
            };

            // Tính toán WarrantyUntil nếu có WarrantyFrom và MonthOfWarranty
            if (warrantyFrom.HasValue && monthOfWarranty > 0 && !warrantyUntil.HasValue)
            {
                warrantyDto.WarrantyUntil = warrantyFrom.Value.AddMonths(monthOfWarranty);
            }

            // Lấy tên sản phẩm từ selected detail
            if (stockInDetailDtoBindingSource.Current is NhapHangThuongMaiDetailDto selectedDetail)
            {
                warrantyDto.ProductVariantName = selectedDetail.ProductVariantName;
            }

            // Thêm vào grid
            if (warrantyDtoBindingSource.DataSource is not List<WarrantyDto> currentList)
            {
                currentList = new List<WarrantyDto>();
                warrantyDtoBindingSource.DataSource = currentList;
            }

            currentList.Add(warrantyDto);
            warrantyDtoBindingSource.ResetBindings(false);

            WarrantyDtoGridView.ExpandAllGroups();
                
            // Đánh dấu có thay đổi
            MarkAsChanged();

            // Clear input controls
            UniqueProductInfoTextEdit.Text = string.Empty;

            _logger.Info("ThemVaoHyperlinkLabelControl_Click: Warranty added to grid, Id={0}", warrantyDto.Id);
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

            var selectedRow = WarrantyDtoGridView.GetFocusedRow() as WarrantyDto;
            if (selectedRow == null)
            {
                MsgBox.ShowWarning("Vui lòng chọn dòng cần xóa", "Cảnh báo", this);
                return;
            }

            var result = MsgBox.ShowYesNo(
                "Bạn có chắc chắn muốn xóa bảo hành này?",
                "Xác nhận xóa",
                this);

            if (!result) return;

            if (warrantyDtoBindingSource.DataSource is not List<WarrantyDto> currentList) return;
                
            currentList.Remove(selectedRow);
            warrantyDtoBindingSource.ResetBindings(false);
            MarkAsChanged();
            _logger.Info("BoRaHyperlinkLabelControl_Click: Warranty removed from grid, Id={0}", selectedRow.Id);
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
            if (WarrantyDtoGridView.GetRow(e.RowHandle) is not WarrantyDto selectedRow) return;
                
            // Load thông tin vào input controls
            // Lấy StockInOutDetailId từ Device nếu có
            Guid? stockInOutDetailId = null;
            if (selectedRow.DeviceId.HasValue)
            {
                var device = _deviceBll.GetById(selectedRow.DeviceId.Value);
                if (device != null && device.StockInOutDetailId.HasValue)
                {
                    stockInOutDetailId = device.StockInOutDetailId.Value;
                }
            }
            StockInOutDetailIdSearchLookUpEdit.EditValue = stockInOutDetailId;
            WarrantyFromDateEdit.EditValue = selectedRow.WarrantyFrom ?? DateTime.Now; // Nếu null thì set ngày hiện tại
            MonthOfWarrantyTextEdit.EditValue = selectedRow.MonthOfWarranty;
            WarrantyUntilDateEdit.EditValue = selectedRow.WarrantyUntil;
            UniqueProductInfoTextEdit.EditValue = selectedRow.DeviceInfo ?? selectedRow.Notes;
        }
        catch (Exception ex)
        {
            _logger.Error("WarrantyDtoGridView_RowClick: Exception occurred", ex);
        }
    }

    /// <summary>
    /// Event handler khi thay đổi số tháng bảo hành - tự động cập nhật ngày kết thúc bảo hành
    /// </summary>
    private void MonthOfWarrantyTextEdit_EditValueChanged(object sender, EventArgs e)
    {
        try
        {
            // Validate và clear lỗi nếu giá trị hợp lệ
            var monthOfWarranty = Convert.ToInt32(MonthOfWarrantyTextEdit.EditValue ?? 0);
            if (monthOfWarranty > 0)
            {
                // Giá trị hợp lệ, clear lỗi
                dxErrorProvider1.SetError(MonthOfWarrantyTextEdit, string.Empty);
            }

            // Tính toán ngày kết thúc bảo hành
            CalculateWarrantyUntil();
        }
        catch (Exception ex)
        {
            _logger.Error("MonthOfWarrantyTextEdit_EditValueChanged: Exception occurred", ex);
        }
    }

    /// <summary>
    /// Event handler khi thay đổi ngày bắt đầu bảo hành - tự động cập nhật ngày kết thúc bảo hành
    /// </summary>
    private void WarrantyFromDateEdit_EditValueChanged(object sender, EventArgs e)
    {
        try
        {
            // Clear lỗi nếu có (ngày bắt đầu không bắt buộc nhưng vẫn có thể có lỗi validation khác)
            if (WarrantyFromDateEdit.EditValue is DateTime)
            {
                dxErrorProvider1.SetError(WarrantyFromDateEdit, string.Empty);
            }

            // Tính toán ngày kết thúc bảo hành
            CalculateWarrantyUntil();
        }
        catch (Exception ex)
        {
            _logger.Error("WarrantyFromDateEdit_EditValueChanged: Exception occurred", ex);
        }
    }

    /// <summary>
    /// Tính toán và cập nhật ngày kết thúc bảo hành dựa trên ngày bắt đầu và số tháng bảo hành
    /// </summary>
    private void CalculateWarrantyUntil()
    {
        try
        {
            var warrantyFrom = WarrantyFromDateEdit.EditValue as DateTime?;
            var monthOfWarranty = Convert.ToInt32(MonthOfWarrantyTextEdit.EditValue ?? 0);

            // Chỉ tính toán nếu có đầy đủ thông tin
            if (warrantyFrom.HasValue && monthOfWarranty > 0)
            {
                var warrantyUntil = warrantyFrom.Value.AddMonths(monthOfWarranty);
                WarrantyUntilDateEdit.EditValue = warrantyUntil;
                    
                _logger.Debug("CalculateWarrantyUntil: Calculated WarrantyUntil={0} from WarrantyFrom={1} and MonthOfWarranty={2}", 
                    warrantyUntil, warrantyFrom.Value, monthOfWarranty);
            }
            else if (!warrantyFrom.HasValue || monthOfWarranty <= 0)
            {
                // Nếu thiếu thông tin, clear WarrantyUntil
                WarrantyUntilDateEdit.EditValue = null;
            }
        }
        catch (Exception ex)
        {
            _logger.Error("CalculateWarrantyUntil: Exception occurred", ex);
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

                // Lấy thông tin ProductVariant từ selected detail
                if (stockInDetailDtoBindingSource.Current is NhapHangThuongMaiDetailDto selectedDetail)
                {
                    // Lấy ProductVariantListDto từ ProductVariantId
                    ProductVariantListDto variantListDto = null;
                    try
                    {
                        if (selectedDetail.ProductVariantId != Guid.Empty)
                        {
                            var variantEntity = _productVariantBll.GetById(selectedDetail.ProductVariantId);
                            if (variantEntity != null)
                            {
                                variantListDto = variantEntity.ToListDto();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.Warning("StockInOutDetailIdSearchLookUpEdit_EditValueChanged: Không thể lấy ProductVariantListDto, ProductVariantId={0}, Error={1}", 
                            selectedDetail.ProductVariantId, ex.Message);
                    }

                    // Tạo HTML string từ thông tin ProductVariant (không dùng size và weight)
                    var html = string.Empty;

                    // Sử dụng VariantFullName từ ProductVariantListDto nếu có, nếu không thì fallback về ProductVariantName
                    var variantFullName = variantListDto?.VariantFullName ?? selectedDetail.ProductVariantName ?? string.Empty;
                    var variantCode = variantListDto?.VariantCode ?? selectedDetail.ProductVariantCode ?? string.Empty;
                    var unitName = variantListDto?.UnitName ?? selectedDetail.UnitOfMeasureName ?? string.Empty;

                    // Hiển thị VariantFullName (nổi bật nhất)
                    if (!string.IsNullOrWhiteSpace(variantFullName))
                    {
                        html += $"<color='blue'>{variantFullName}</color>";
                    }

                    // Mã biến thể (nếu có và khác với VariantFullName)
                    if (!string.IsNullOrWhiteSpace(variantCode) && !variantFullName.Contains(variantCode))
                    {
                        if (!string.IsNullOrWhiteSpace(variantFullName))
                        {
                            html += $" <color='#757575'>({variantCode})</color>";
                        }
                        else
                        {
                            html += $"<color='blue'>{variantCode}</color>";
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(variantFullName) || !string.IsNullOrWhiteSpace(variantCode))
                    {
                        html += "<br>";
                    }

                    // Đơn vị tính
                    if (!string.IsNullOrWhiteSpace(unitName))
                    {
                        html += $"<color='#757575'>Đơn vị tính:</color> <color='#212121'>{unitName}</color><br>";
                    }

                    // Số lượng nhập
                    if (selectedDetail.StockInQty > 0)
                    {
                        html += $"<color='#757575'>SL nhập: </color><color='#212121'>{selectedDetail.StockInQty:N0}</color>";
                        if (!string.IsNullOrWhiteSpace(unitName))
                        {
                            html += $" {unitName}";
                        }
                    }

                    // Số lượng xuất
                    if (selectedDetail.StockOutQty > 0)
                    {
                        if (selectedDetail.StockInQty > 0)
                        {
                            html += " | ";
                        }
                        html += $"<color='#757575'>SL xuất: </color><color='#212121'>{selectedDetail.StockOutQty:N0}</color>";
                        if (!string.IsNullOrWhiteSpace(unitName))
                        {
                            html += $" {unitName}";
                        }
                    }

                    // Cập nhật ProductVarianFullInfoHypertextLabel với thông tin đầy đủ
                    ProductVarianFullInfoHypertextLabel.Text = html;

                    _logger.Debug("StockInOutDetailIdSearchLookUpEdit_EditValueChanged: Updated ProductVarianFullInfoHypertextLabel, StockInOutDetailId={0}, VariantFullName={1}", 
                        stockInOutDetailId, variantFullName);
                }
                else
                {
                    // Nếu không tìm thấy detail, clear label
                    ProductVarianFullInfoHypertextLabel.Text = "Thông tin đầy đủ của sản phẩm bảo hành";
                    _logger.Warning("StockInOutDetailIdSearchLookUpEdit_EditValueChanged: Selected detail not found, StockInOutDetailId={0}", stockInOutDetailId);
                }
            }
            else
            {
                // Nếu không có giá trị, reset về mặc định
                ProductVarianFullInfoHypertextLabel.Text = "Thông tin đầy đủ của sản phẩm bảo hành";
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

            // Validate UniqueProductInfo - đảm bảo duy nhất không được trùng lặp
            var uniqueProductInfo = UniqueProductInfoTextEdit.EditValue?.ToString()?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(uniqueProductInfo))
            {
                dxErrorProvider1.SetError(UniqueProductInfoTextEdit, "Vui lòng nhập thông tin sản phẩm duy nhất");
                return false;
            }

            // Kiểm tra trùng lặp trong grid hiện tại (case-insensitive)
            // Nếu đang edit một dòng (có dòng được chọn trong grid), bỏ qua dòng đó khi kiểm tra trùng lặp
            if (warrantyDtoBindingSource.DataSource is List<WarrantyDto> currentList && currentList.Any())
            {
                // Lấy dòng đang được chọn trong grid (nếu có)
                WarrantyDto selectedRow = null;
                var focusedRowHandle = WarrantyDtoGridView.FocusedRowHandle;
                if (focusedRowHandle >= 0)
                {
                    selectedRow = WarrantyDtoGridView.GetRow(focusedRowHandle) as WarrantyDto;
                }

                // Kiểm tra trùng lặp, bỏ qua dòng đang được chọn (nếu đang edit)
                var isDuplicate = currentList.Any(w => 
                    w != selectedRow && // Bỏ qua dòng đang được chọn nếu đang edit
                    !string.IsNullOrWhiteSpace(w.DeviceInfo) &&
                    string.Equals(w.DeviceInfo.Trim(), uniqueProductInfo, StringComparison.OrdinalIgnoreCase));
                    
                if (isDuplicate)
                {
                    dxErrorProvider1.SetError(UniqueProductInfoTextEdit, "Thông tin thiết bị đã tồn tại trong danh sách. Vui lòng nhập giá trị khác.");
                    _logger.Warning("ValidateInput: Duplicate DeviceInfo detected, value={0}", uniqueProductInfo);
                    return false;
                }
            }

            dxErrorProvider1.SetError(UniqueProductInfoTextEdit, string.Empty);

            // Validate MonthOfWarranty
            var monthOfWarranty = Convert.ToInt32(MonthOfWarrantyTextEdit.EditValue ?? 0);
            if (monthOfWarranty <= 0)
            {
                dxErrorProvider1.SetError(MonthOfWarrantyTextEdit, "Số tháng bảo hành phải lớn hơn 0");
                return false;
            }
            dxErrorProvider1.SetError(MonthOfWarrantyTextEdit, string.Empty);

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

            // Lấy danh sách WarrantyDto từ grid
            var warrantyDtos = warrantyDtoBindingSource.Cast<WarrantyDto>().ToList();
            if (warrantyDtos.Count == 0)
            {
                MsgBox.ShowWarning("Không có bảo hành nào để lưu", "Cảnh báo", this);
                return false;
            }

            // Validate tất cả các dòng
            var validationErrors = new List<string>();
            for (int i = 0; i < warrantyDtos.Count; i++)
            {
                var warranty = warrantyDtos[i];
                // Kiểm tra DeviceId hoặc DeviceInfo (cần có ít nhất một trong hai)
                if (!warranty.DeviceId.HasValue && string.IsNullOrWhiteSpace(warranty.DeviceInfo))
                {
                    validationErrors.Add($"Dòng {i + 1}: Vui lòng chọn sản phẩm bảo hành và nhập thông tin thiết bị");
                }
                if (warranty.MonthOfWarranty <= 0)
                {
                    validationErrors.Add($"Dòng {i + 1}: Số tháng bảo hành phải lớn hơn 0");
                }
            }

            // Kiểm tra trùng lặp DeviceInfo (case-insensitive)
            var duplicateGroups = warrantyDtos
                .Where(w => !string.IsNullOrWhiteSpace(w.DeviceInfo))
                .GroupBy(w => w.DeviceInfo.Trim(), StringComparer.OrdinalIgnoreCase)
                .Where(g => g.Count() > 1)
                .ToList();

            if (duplicateGroups.Any())
            {
                foreach (var group in duplicateGroups)
                {
                    var duplicateIndices = warrantyDtos
                        .Select((w, index) => new { w, index })
                        .Where(x => string.Equals(x.w.DeviceInfo?.Trim(), group.Key, StringComparison.OrdinalIgnoreCase))
                        .Select(x => x.index + 1)
                        .ToList();
                        
                    validationErrors.Add($"Thông tin thiết bị '{group.Key}' bị trùng lặp ở các dòng: {string.Join(", ", duplicateIndices)}");
                }
                    
                _logger.Warning("SaveDataAsync: Duplicate DeviceInfo detected, count={0}", duplicateGroups.Count);
            }

            if (validationErrors.Any())
            {
                MsgBox.ShowError($"Có lỗi trong dữ liệu:\n\n{string.Join("\n", validationErrors)}", "Lỗi validation", this);
                return false;
            }

            // Lưu từng bảo hành - tìm hoặc tạo Device trước
            await Task.Run(() =>
            {
                foreach (var warrantyDto in warrantyDtos)
                {
                    // Tìm hoặc tạo Device từ DeviceInfo và StockInOutDetailId
                    Guid? deviceId = warrantyDto.DeviceId;
                    
                    if (!deviceId.HasValue && !string.IsNullOrWhiteSpace(warrantyDto.DeviceInfo))
                    {
                        // Tìm Device từ DeviceInfo trong StockInOutMaster
                        var devices = _deviceBll.GetByStockInOutMasterId(StockInOutMasterId);
                        var device = devices.FirstOrDefault(d => 
                            (!string.IsNullOrWhiteSpace(d.SerialNumber) && d.SerialNumber.Equals(warrantyDto.DeviceInfo, StringComparison.OrdinalIgnoreCase)) ||
                            (!string.IsNullOrWhiteSpace(d.IMEI) && d.IMEI.Equals(warrantyDto.DeviceInfo, StringComparison.OrdinalIgnoreCase)) ||
                            (!string.IsNullOrWhiteSpace(d.MACAddress) && d.MACAddress.Equals(warrantyDto.DeviceInfo, StringComparison.OrdinalIgnoreCase)) ||
                            (!string.IsNullOrWhiteSpace(d.AssetTag) && d.AssetTag.Equals(warrantyDto.DeviceInfo, StringComparison.OrdinalIgnoreCase)) ||
                            (!string.IsNullOrWhiteSpace(d.LicenseKey) && d.LicenseKey.Equals(warrantyDto.DeviceInfo, StringComparison.OrdinalIgnoreCase))
                        );
                        
                        if (device != null)
                        {
                            deviceId = device.Id;
                        }
                        // Nếu không tìm thấy Device, cần tạo mới (nhưng cần StockInOutDetailId và ProductVariantId)
                        // Tạm thời bỏ qua, sẽ xử lý sau hoặc yêu cầu người dùng tạo Device trước
                    }
                    
                    // Cập nhật DeviceId vào warrantyDto
                    warrantyDto.DeviceId = deviceId;
                    
                    var warranty = warrantyDto.ToEntity();
                    _warrantyBll.SaveOrUpdate(warranty);
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