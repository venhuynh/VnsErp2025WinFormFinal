using Common.Common;
using Common.Utils;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bll.Inventory.StockIn;
using System.Linq;
using Inventory.StockIn.InPhieu;
using DTO.Inventory.StockIn;

namespace Inventory.StockIn
{
    public partial class FrmNhapKhoThuongMai02 : DevExpress.XtraEditors.XtraForm
    {
        #region ========== FIELDS & PROPERTIES ==========

        /// <summary>
        /// Business Logic Layer cho StockIn
        /// </summary>
        private readonly StockInBll _stockInBll = new StockInBll();

        /// <summary>
        /// Logger để ghi log các sự kiện
        /// </summary>
        private readonly ILogger _logger = LoggerFactory.CreateLogger(LogCategory.UI);

        /// <summary>
        /// Trạng thái có thay đổi dữ liệu chưa lưu
        /// </summary>
        private bool _hasUnsavedChanges;

        /// <summary>
        /// ID phiếu nhập kho hiện tại (nếu đang edit)
        /// </summary>
        private Guid _currentStockInId;

        /// <summary>
        /// Flag đánh dấu đang trong quá trình đóng form sau khi lưu thành công
        /// Dùng để tránh hỏi lại khi Close() được gọi từ BeginInvoke
        /// </summary>
        private bool _isClosingAfterSave;

        #endregion

        #region ========== CONSTRUCTOR ==========

        /// <summary>
        /// Constructor mặc định (tạo phiếu mới)
        /// </summary>
        public FrmNhapKhoThuongMai02()
        {
            InitializeComponent();
            Load += FrmNhapKhoThuongMai02_Load;
            _currentStockInId = Guid.Empty;
        }

        /// <summary>
        /// Constructor với ID phiếu nhập kho (mở để xem/sửa)
        /// </summary>
        /// <param name="stockInId">ID phiếu nhập kho</param>
        public FrmNhapKhoThuongMai02(Guid stockInId)
        {
            InitializeComponent();
            Load += FrmNhapKhoThuongMai02_Load;

            // Gán ID phiếu nhập kho hiện tại
            _currentStockInId = stockInId;
        }

        #endregion

        #region ========== INITIALIZATION ==========

        /// <summary>
        /// Event handler khi form được load
        /// </summary>
        private async void FrmNhapKhoThuongMai02_Load(object sender, EventArgs e)
        {
            try
            {
                _logger.Debug("FrmNhapKhoThuongMai02_Load: Form loading");

                // Setup event handlers
                SetupEvents();

                // Đảm bảo form đã được hiển thị và sẵn sàng trước khi show splash screen
                // Refresh form để đảm bảo nó đã được render
                Refresh();
                Application.DoEvents(); // Cho phép form render xong

                // Load datasource với SplashScreen (với owner là form này)
                //await LoadDataSourcesAsync();

                // Nếu _currentStockInId có giá trị thì load dữ liệu vào UI của 2 UserControl
                if (_currentStockInId != Guid.Empty)
                {
                    _logger.Debug("FrmNhapKhoThuongMai02_Load: Loading existing stock in data, StockInId={0}", _currentStockInId);

                    // Load dữ liệu từ ID vào các user controls
                    //await LoadDataAsync(_currentStockInId);

                    //FIXME: Tạo hàm LoadDataAsync trong user controls để load dữ liệu từ _currentStockInId
                    await ucStockInMaster1.LoadDataAsync(_currentStockInId);
                    //await ucStockInDetail1.LoadDataAsync(_currentStockInId);
                }
                else
                {
                    // Reset form về trạng thái ban đầu (tạo phiếu mới)
                    ResetForm();
                }

                _logger.Info("FrmNhapKhoThuongMai02_Load: Form loaded successfully");
            }
            catch (Exception ex)
            {
                _logger.Error("FrmNhapKhoThuongMai02_Load: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi khởi tạo form: {ex.Message}");
            }
        }

        /// <summary>
        /// Load tất cả datasource cho các SearchLookUpEdit với SplashScreen
        /// SplashScreen sẽ được hiển thị với owner là form này để đảm bảo hiển thị đúng vị trí
        /// </summary>
        private async Task LoadDataSourcesAsync()
        {
            try
            {
                _logger.Debug("LoadDataSourcesAsync: Starting to load datasources");

                // Hiển thị SplashScreen với owner là form này
                // Sử dụng SplashScreenManager trực tiếp để set owner
                SplashScreenHelper.ShowWaitingSplashScreen();

                try
                {
                    // Load datasource cho UcStockInMaster (Warehouse và Supplier)
                    _logger.Debug("LoadDataSourcesAsync: Loading lookup data for UcStockInMaster");
                    //await ucStockInMaster1.LoadLookupDataAsync();

                    // Load datasource cho UcStockInDetail (ProductVariant)
                    _logger.Debug("LoadDataSourcesAsync: Loading product variants for UcStockInDetail");
                    await ucStockInDetail1.LoadProductVariantsAsync();

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
        /// Hiển thị SplashScreen với owner là form này
        /// Đảm bảo SplashScreen hiển thị đúng vị trí và có owner đúng
        /// Sử dụng SplashScreenHelper và SplashScreenManager với owner parameter
        /// </summary>
        private void ShowSplashScreenWithOwner()
        {
            try
            {
                // Đảm bảo form đã được hiển thị
                if (!Visible)
                {
                    Show();
                }

                // Đảm bảo form đã được activate
                if (!IsHandleCreated)
                {
                    CreateHandle();
                }

                // Đảm bảo form được bring to front và activate
                BringToFront();
                Activate();

                // Đóng splash screen hiện tại nếu có (sử dụng SplashScreenHelper)
                SplashScreenHelper.CloseSplashScreen();

                // Hiển thị SplashScreen với owner là form này
                // Sử dụng SplashScreenManager.ShowForm với owner parameter
                // Khi có owner, SplashScreen sẽ tự động hiển thị trên owner form và là topmost
                DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(
                    this, // Owner form - đảm bảo SplashScreen hiển thị trên form này
                    typeof(VnsSplashScreen),
                    false, // fadeIn
                    false, // fadeOut
                    false); // useFadeInOutAnimation

                // Đảm bảo SplashScreen được bring to front sau khi hiển thị
                // Sử dụng BeginInvoke để đảm bảo SplashScreen đã được tạo xong
                BeginInvoke(new Action(() =>
                {
                    try
                    {
                        // Kiểm tra SplashScreen đã hiển thị chưa
                        if (DevExpress.XtraSplashScreen.SplashScreenManager.Default != null &&
                            DevExpress.XtraSplashScreen.SplashScreenManager.Default.IsSplashFormVisible)
                        {
                            // Đảm bảo owner form vẫn ở front
                            BringToFront();
                            
                            _logger.Debug("ShowSplashScreenWithOwner: SplashScreen displayed with owner={0}", Name);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.Warning("ShowSplashScreenWithOwner: Exception in BeginInvoke: {0}", ex.Message);
                    }
                }));

                _logger.Debug("ShowSplashScreenWithOwner: SplashScreen display initiated with owner={0}", Name);
            }
            catch (Exception ex)
            {
                _logger.Error("ShowSplashScreenWithOwner: Exception occurred", ex);
                // Fallback: sử dụng helper nếu có lỗi
                SplashScreenHelper.ShowVnsSplashScreen("VnsErp2025", "Đang tải dữ liệu...");
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
                NhapLaiBarButtonItem.ItemClick += NhapLaiBarButtonItem_ItemClick;
                LuuPhieuBarButtonItem.ItemClick += LuuPhieuBarButtonItem_ItemClick;
                InPhieuBarButtonItem.ItemClick += InPhieuBarButtonItem_ItemClick;
                NhapBaoHanhBarButtonItem.ItemClick += NhapBaoHanhBarButtonItem_ItemClick;
                ThemHinhAnhBarButtonItem.ItemClick += ThemHinhAnhBarButtonItem_ItemClick;
                CloseBarButtonItem.ItemClick += CloseBarButtonItem_ItemClick;

                // Form events
                FormClosing += FrmNhapKhoThuongMai02_FormClosing;
                KeyDown += FrmNhapKhoThuongMai02_KeyDown;
                KeyPreview = true; // Cho phép form xử lý phím tắt trước

                // Detail control events - theo dõi thay đổi để đánh dấu có thay đổi chưa lưu và cập nhật tổng lên master
                ucStockInDetail1.DetailDataChanged += UcStockInDetail1_DetailDataChanged;

                // Setup phím tắt và hiển thị hướng dẫn
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
            _isClosingAfterSave = false; // Reset flag khi đánh dấu đã lưu
            _logger.Debug("MarkAsSaved: Form marked as saved");
        }

        /// <summary>
        /// Thiết lập phím tắt cho các nút chức năng
        /// </summary>
        private void SetupKeyboardShortcuts()
        {
            try
            {
                // Gán phím tắt cho các BarButtonItem
                // F1: Nhập lại
                // F2: Lưu phiếu
                // F3: In phiếu
                // F4: Nhập bảo hành
                // F5: Thêm hình ảnh
                // ESC: Đóng form

                // Lưu ý: DevExpress BarButtonItem không hỗ trợ trực tiếp ItemShortcut
                // Nên sẽ xử lý qua KeyDown event của form
                
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
                if (HotKeyBarStaticItem == null) return;

                // Tạo nội dung HTML với các phím tắt
                var hotKeyText = @"<color=Gray>Phím tắt:</color> " +
                    @"<b><color=Blue>F1</color></b> Nhập lại | " +
                    @"<b><color=Blue>F2</color></b> Lưu phiếu | " +
                    @"<b><color=Blue>F3</color></b> In phiếu | " +
                    @"<b><color=Blue>F4</color></b> Nhập bảo hành | " +
                    @"<b><color=Blue>F5</color></b> Thêm hình ảnh | " +
                    @"<b><color=Blue>ESC</color></b> Đóng | " +
                    @"<b><color=Blue>Insert</color></b> Thêm dòng | " +
                    @"<b><color=Blue>Delete</color></b> Xóa dòng | " +
                    @"<b><color=Blue>Enter</color></b> Hoàn thành dòng";

                HotKeyBarStaticItem.Caption = hotKeyText;
                HotKeyBarStaticItem.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                
                _logger.Debug("UpdateHotKeyBarStaticItem: Hot key bar updated");
            }
            catch (Exception ex)
            {
                _logger.Error("UpdateHotKeyBarStaticItem: Exception occurred", ex);
            }
        }

        #endregion

        #region ========== EVENT HANDLERS ==========

        /// <summary>
        /// Event handler cho nút Nhập lại
        /// </summary>
        private void NhapLaiBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                _logger.Debug("NhapLaiBarButtonItem_ItemClick: Reset button clicked");

                // Kiểm tra có thay đổi chưa lưu không
                if (_hasUnsavedChanges)
                {
                    var confirm = MsgBox.ShowYesNo(
                        "Bạn có thay đổi chưa lưu. Bạn có chắc chắn muốn nhập lại?",
                        "Xác nhận nhập lại",
                        this);
                    
                    if (!confirm)
                    {
                        _logger.Debug("NhapLaiBarButtonItem_ItemClick: User cancelled reset");
                        return;
                    }
                }

                ResetForm();
                _logger.Info("NhapLaiBarButtonItem_ItemClick: Form reset successfully");
            }
            catch (Exception ex)
            {
                _logger.Error("NhapLaiBarButtonItem_ItemClick: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi nhập lại: {ex.Message}");
            }
        }

        /// <summary>
        /// Event handler cho nút Lưu phiếu
        /// </summary>
        private async void LuuPhieuBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                _logger.Debug("LuuPhieuBarButtonItem_ItemClick: Save button clicked");

                // Disable button để tránh double-click
                LuuPhieuBarButtonItem.Enabled = false;

                try
                {
                    // Validate và lưu dữ liệu
                    var success = await SaveDataAsync();

                    if (success)
                    {
                        MsgBox.ShowSuccess("Lưu phiếu nhập kho thành công!", "Thành công", this);
                        MarkAsSaved();
                        _logger.Info("LuuPhieuBarButtonItem_ItemClick: Data saved successfully");
                    }
                }
                finally
                {
                    // Re-enable button
                    LuuPhieuBarButtonItem.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                _logger.Error("LuuPhieuBarButtonItem_ItemClick: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi lưu phiếu: {ex.Message}");
            }
        }

        /// <summary>
        /// Event handler cho nút In phiếu
        /// </summary>
        private async void InPhieuBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                _logger.Debug("InPhieuBarButtonItem_ItemClick: Print button clicked");

                // Lấy StockInOutMasterId từ _currentStockInId (phải đã được lưu)
                Guid stockInOutMasterId;
                
                // Kiểm tra phiếu đã được lưu chưa
                if (_currentStockInId != Guid.Empty)
                {
                    stockInOutMasterId = _currentStockInId;
                }
                else
                {
                    // Phiếu chưa được lưu - kiểm tra có thay đổi chưa lưu không
                    if (_hasUnsavedChanges)
                    {
                        // Hỏi người dùng có muốn lưu trước không
                        if (MsgBox.ShowYesNo(
                                "Phiếu nhập kho chưa được lưu. Bạn có muốn lưu trước khi in không?",
                                "Xác nhận",
                                this))
                        {
                            // Gọi nút Lưu để lưu phiếu
                            LuuPhieuBarButtonItem_ItemClick(null, null);
                            
                            // Đợi cho đến khi lưu hoàn tất (tối đa 10 giây)
                            var timeout = TimeSpan.FromSeconds(10);
                            var startTime = DateTime.Now;
                            while (_currentStockInId == Guid.Empty && (DateTime.Now - startTime) < timeout)
                            {
                                await Task.Delay(100);
                            }
                            
                            // Kiểm tra lại sau khi lưu
                            if (_currentStockInId != Guid.Empty)
                            {
                                stockInOutMasterId = _currentStockInId;
                            }
                            else
                            {
                                // Lưu thất bại hoặc timeout, không in phiếu
                                _logger.Warning("InPhieuBarButtonItem_ItemClick: Save failed, timeout, or cancelled, cannot print");
                                return;
                            }
                        }
                        else
                        {
                            // Người dùng chọn không lưu
                            _logger.Debug("InPhieuBarButtonItem_ItemClick: User chose not to save");
                            return;
                        }
                    }
                    else
                    {
                        // Không có thay đổi chưa lưu và chưa có ID - yêu cầu lưu
                        MsgBox.ShowWarning(
                            "Vui lòng nhập và lưu phiếu nhập kho trước khi in.",
                            "Cảnh báo",
                            this);
                        _logger.Warning("InPhieuBarButtonItem_ItemClick: Cannot print - Form not saved and no unsaved changes");
                        return;
                    }
                }

                // Kiểm tra lại StockInOutMasterId sau khi lưu (nếu có)
                if (stockInOutMasterId == Guid.Empty)
                {
                    MsgBox.ShowWarning(
                        "Không thể lấy ID phiếu nhập kho. Vui lòng thử lại.",
                        "Cảnh báo",
                        this);
                    _logger.Warning("InPhieuBarButtonItem_ItemClick: StockInOutMasterId is still Empty after save attempt");
                    return;
                }

                // In phiếu nhập kho với preview
                StockInReportHelper.PrintStockInVoucher(stockInOutMasterId);

                _logger.Info("InPhieuBarButtonItem_ItemClick: Print voucher completed, StockInOutMasterId={0}", stockInOutMasterId);
            }
            catch (Exception ex)
            {
                _logger.Error("InPhieuBarButtonItem_ItemClick: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi in phiếu: {ex.Message}");
            }
        }

        /// <summary>
        /// Event handler cho nút Nhập bảo hành
        /// </summary>
        private async void NhapBaoHanhBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                _logger.Debug("NhapBaoHanhBarButtonItem_ItemClick: Warranty input button clicked");

                // Lấy StockInOutMasterId từ _currentStockInId (phải đã được lưu)
                Guid stockInOutMasterId;
                
                // Kiểm tra phiếu đã được lưu chưa
                if (_currentStockInId != Guid.Empty)
                {
                    stockInOutMasterId = _currentStockInId;
                }
                else
                {
                    // Phiếu chưa được lưu - kiểm tra có thay đổi chưa lưu không
                    if (_hasUnsavedChanges)
                    {
                        // Hỏi người dùng có muốn lưu trước không
                        if (MsgBox.ShowYesNo(
                                "Phiếu nhập kho chưa được lưu. Bạn có muốn lưu trước khi nhập bảo hành không?",
                                "Xác nhận",
                                this))
                        {
                            // Gọi nút Lưu để lưu phiếu
                            LuuPhieuBarButtonItem_ItemClick(null, null);
                            
                            // Đợi cho đến khi lưu hoàn tất (tối đa 10 giây)
                            var timeout = TimeSpan.FromSeconds(10);
                            var startTime = DateTime.Now;
                            while (_currentStockInId == Guid.Empty && (DateTime.Now - startTime) < timeout)
                            {
                                await Task.Delay(100);
                            }
                            
                            // Kiểm tra lại sau khi lưu
                            if (_currentStockInId != Guid.Empty)
                            {
                                stockInOutMasterId = _currentStockInId;
                            }
                            else
                            {
                                // Lưu thất bại hoặc timeout, không mở form nhập bảo hành
                                _logger.Warning("NhapBaoHanhBarButtonItem_ItemClick: Save failed, timeout, or cancelled, cannot open warranty form");
                                return;
                            }
                        }
                        else
                        {
                            // Người dùng chọn không lưu
                            _logger.Debug("NhapBaoHanhBarButtonItem_ItemClick: User chose not to save");
                            return;
                        }
                    }
                    else
                    {
                        // Không có thay đổi chưa lưu và chưa có ID - yêu cầu lưu
                        MsgBox.ShowError(
                            "Vui lòng nhập và lưu phiếu nhập kho trước khi nhập bảo hành.",
                            "Lỗi",
                            this);
                        _logger.Warning("NhapBaoHanhBarButtonItem_ItemClick: Cannot add warranty - Form not saved and no unsaved changes");
                        return;
                    }
                }

                // Kiểm tra lại StockInOutMasterId sau khi lưu (nếu có)
                if (stockInOutMasterId == Guid.Empty)
                {
                    MsgBox.ShowWarning(
                        "Không thể lấy ID phiếu nhập kho. Vui lòng thử lại.",
                        "Cảnh báo",
                        this);
                    _logger.Warning("NhapBaoHanhBarButtonItem_ItemClick: StockInOutMasterId is still Empty after save attempt");
                    return;
                }

                // Mở form nhập bảo hành với StockInOutMasterId (sử dụng OverlayManager để hiển thị)
                using (OverlayManager.ShowScope(this))
                {
                    using (var frmWarranty = new InventoryManagement.FrmWarranty(stockInOutMasterId))
                    {
                        frmWarranty.StartPosition = FormStartPosition.CenterParent;
                        frmWarranty.ShowDialog(this);
                    }
                }

                _logger.Info("NhapBaoHanhBarButtonItem_ItemClick: Warranty form opened with StockInOutMasterId={0}", stockInOutMasterId);
            }
            catch (Exception ex)
            {
                _logger.Error("NhapBaoHanhBarButtonItem_ItemClick: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi nhập bảo hành: {ex.Message}");
            }
        }

        /// <summary>
        /// Event handler cho nút Thêm hình ảnh
        /// </summary>
        private void ThemHinhAnhBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                _logger.Debug("ThemHinhAnhBarButtonItem_ItemClick: Add image button clicked");

                // Lấy StockInOutMasterId từ _currentStockInId (phải đã được lưu)
                Guid stockInOutMasterId = Guid.Empty;
                
                // Kiểm tra phiếu đã được lưu chưa
                if (_currentStockInId != Guid.Empty)
                {
                    stockInOutMasterId = _currentStockInId;
                }
                else
                {
                    // Phiếu chưa được lưu - kiểm tra có thay đổi chưa lưu không
                    if (_hasUnsavedChanges)
                    {
                        // Hỏi người dùng có muốn lưu trước không
                        if (MsgBox.ShowYesNo(
                                "Phiếu nhập kho chưa được lưu. Bạn có muốn lưu trước khi thêm hình ảnh không?",
                                "Xác nhận",
                                this))
                        {
                            // Gọi nút Lưu để lưu phiếu
                            LuuPhieuBarButtonItem_ItemClick(null, null);
                            
                        }
                        else
                        {
                            // Người dùng chọn không lưu
                            _logger.Debug("ThemHinhAnhBarButtonItem_ItemClick: User chose not to save");
                            return;
                        }
                    }
                    else
                    {
                        // Không có thay đổi chưa lưu và chưa có ID - yêu cầu lưu
                        MsgBox.ShowError(
                            "Vui lòng nhập và lưu phiếu nhập kho trước khi thêm hình ảnh.",
                            "Lỗi",
                            this);
                        _logger.Warning("ThemHinhAnhBarButtonItem_ItemClick: Cannot add images - Form not saved and no unsaved changes");
                        return;
                    }
                }

                // Kiểm tra lại StockInOutMasterId sau khi lưu (nếu có)
                if (stockInOutMasterId == Guid.Empty)
                {
                    MsgBox.ShowWarning(
                        "Không thể lấy ID phiếu nhập kho. Vui lòng thử lại.",
                        "Cảnh báo",
                        this);
                    _logger.Warning("ThemHinhAnhBarButtonItem_ItemClick: StockInOutMasterId is still Empty after save attempt");
                    return;
                }

                // Mở form thêm hình ảnh với StockInOutMasterId (sử dụng OverlayManager để hiển thị)
                using (OverlayManager.ShowScope(this))
                {
                    using (var frmAddImages = new InventoryManagement.FrmStockInOutAddImages(stockInOutMasterId))
                    {
                        frmAddImages.StartPosition = FormStartPosition.CenterParent;
                        frmAddImages.ShowDialog(this);
                    }
                }

                _logger.Info("ThemHinhAnhBarButtonItem_ItemClick: Add images form opened with StockInOutMasterId={0}", stockInOutMasterId);
            }
            catch (Exception ex)
            {
                _logger.Error("ThemHinhAnhBarButtonItem_ItemClick: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi thêm hình ảnh: {ex.Message}");
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
                Close();
            }
            catch (Exception ex)
            {
                _logger.Error("CloseBarButtonItem_ItemClick: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi đóng form: {ex.Message}");
            }
        }

        /// <summary>
        /// Event handler xử lý phím tắt
        /// </summary>
        private void FrmNhapKhoThuongMai02_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                // Chỉ xử lý phím tắt cho form (F1-F5, ESC)
                // Phím Insert, Delete, Enter sẽ được xử lý trực tiếp trong GridView
                // để tránh conflict khi đang edit trong grid

                // Xử lý phím tắt cho form
                switch (e.KeyCode)
                {
                    case Keys.F1:
                        // F1: Nhập lại
                        e.Handled = true;
                        NhapLaiBarButtonItem_ItemClick(null, null);
                        break;

                    case Keys.F2:
                        // F2: Lưu phiếu
                        e.Handled = true;
                        LuuPhieuBarButtonItem_ItemClick(null, null);
                        break;

                    case Keys.F3:
                        // F3: In phiếu
                        e.Handled = true;
                        InPhieuBarButtonItem_ItemClick(null, null);
                        break;

                    case Keys.F4:
                        // F4: Nhập bảo hành
                        e.Handled = true;
                        NhapBaoHanhBarButtonItem_ItemClick(null, null);
                        break;

                    case Keys.F5:
                        // F5: Thêm hình ảnh
                        e.Handled = true;
                        ThemHinhAnhBarButtonItem_ItemClick(null, null);
                        break;

                    case Keys.Escape:
                        // ESC: Đóng form (chỉ khi không đang edit trong grid)
                        if (!(ActiveControl is DevExpress.XtraEditors.BaseEdit baseEdit && baseEdit.IsEditorActive))
                        {
                            e.Handled = true;
                            CloseBarButtonItem_ItemClick(null, null);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.Error("FrmNhapKhoThuongMai02_KeyDown: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi xử lý phím tắt: {ex.Message}");
            }
        }

        /// <summary>
        /// Event handler khi dữ liệu detail thay đổi
        /// </summary>
        private void UcStockInDetail1_DetailDataChanged(object sender, EventArgs e)
        {
            try
            {
                _logger.Debug("UcStockInDetail1_DetailDataChanged: Detail data changed, updating totals");

                // Đánh dấu có thay đổi chưa lưu
                MarkAsChanged();

                // Tính tổng từ detail
                var (totalQuantity, totalAmount, totalVat, totalAmountIncludedVat) = ucStockInDetail1.CalculateTotals();

                // Cập nhật tổng lên master
                ucStockInMaster1.UpdateTotals(totalQuantity, totalAmount, totalVat, totalAmountIncludedVat);

                _logger.Debug("UcStockInDetail1_DetailDataChanged: Totals updated - Quantity={0}, Amount={1}, Vat={2}, Total={3}",
                    totalQuantity, totalAmount, totalVat, totalAmountIncludedVat);
            }
            catch (Exception ex)
            {
                _logger.Error("UcStockInDetail1_DetailDataChanged: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi cập nhật tổng hợp: {ex.Message}");
            }
        }

        /// <summary>
        /// Event handler khi form đang đóng
        /// Sử dụng ShowYesNoCancel để đơn giản hóa logic: Yes = Lưu và đóng, No = Đóng không lưu, Cancel = Hủy
        /// </summary>
        private async void FrmNhapKhoThuongMai02_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                _logger.Debug("FrmNhapKhoThuongMai02_FormClosing: Form closing, HasUnsavedChanges={0}, IsClosingAfterSave={1}", 
                    _hasUnsavedChanges, _isClosingAfterSave);

                // Nếu đang trong quá trình đóng sau khi lưu thành công, cho phép đóng luôn
                if (_isClosingAfterSave)
                {
                    _logger.Debug("FrmNhapKhoThuongMai02_FormClosing: Closing after save, allowing close");
                    e.Cancel = false;
                    return;
                }

                // Kiểm tra có thay đổi chưa lưu không
                if (_hasUnsavedChanges)
                {
                    // Sử dụng ShowYesNoCancel với 3 lựa chọn:
                    // Yes = Lưu và đóng
                    // No = Đóng không lưu
                    // Cancel = Hủy (giữ form mở)
                    var result = MsgBox.ShowYesNoCancel(
                        "Bạn có thay đổi chưa lưu. Bạn muốn làm gì?",
                        "Xác nhận đóng",
                        this,
                        yesButtonText: "Lưu và đóng",
                        noButtonText: "Đóng không lưu",
                        cancelButtonText: "Hủy");
                    
                    _logger.Debug("FrmNhapKhoThuongMai02_FormClosing: User choice result={0}", result);
                    
                    if (result == DialogResult.Yes)
                    {
                        // Người dùng chọn "Lưu và đóng"
                        // Cancel việc đóng form tạm thời để lưu dữ liệu
                        e.Cancel = true;
                        
                        try
                        {
                            _logger.Debug("FrmNhapKhoThuongMai02_FormClosing: User chose to save and close, starting save operation");
                            
                            // Lưu dữ liệu
                            var saveSuccess = await SaveDataAsync();
                            
                            if (saveSuccess)
                            {
                                // Lưu thành công, đánh dấu đã lưu và chuẩn bị đóng form
                                _logger.Info("FrmNhapKhoThuongMai02_FormClosing: Save successful, closing form");
                                
                                // Đánh dấu đã lưu để không hỏi lại
                                MarkAsSaved();
                                
                                // Set flag để tránh hỏi lại khi Close() được gọi
                                _isClosingAfterSave = true;
                                
                                // Sử dụng BeginInvoke để đóng form sau khi event handler kết thúc
                                BeginInvoke(new Action(Close));
                            }
                            else
                            {
                                // Lưu thất bại, giữ form mở
                                _logger.Warning("FrmNhapKhoThuongMai02_FormClosing: Save failed, form will remain open");
                                e.Cancel = true;
                            }
                        }
                        catch (Exception saveEx)
                        {
                            _logger.Error("FrmNhapKhoThuongMai02_FormClosing: Exception during save operation", saveEx);
                            // Lỗi khi lưu, giữ form mở
                            e.Cancel = true;
                        }
                    }
                    else if (result == DialogResult.No)
                    {
                        // Người dùng chọn "Đóng không lưu" - cho phép đóng form
                        e.Cancel = false;
                        _logger.Debug("FrmNhapKhoThuongMai02_FormClosing: User chose to close without save, form will close");
                    }
                    else // DialogResult.Cancel
                    {
                        // Người dùng chọn "Hủy" - không muốn đóng, giữ form mở
                        e.Cancel = true;
                        _logger.Debug("FrmNhapKhoThuongMai02_FormClosing: User cancelled closing, form will remain open");
                    }
                }
                else
                {
                    // Không có thay đổi chưa lưu, cho phép đóng form
                    e.Cancel = false;
                    _logger.Debug("FrmNhapKhoThuongMai02_FormClosing: No unsaved changes, form will close");
                }

                _logger.Info("FrmNhapKhoThuongMai02_FormClosing: Form closing completed, Cancel={0}", e.Cancel);
            }
            catch (Exception ex)
            {
                _logger.Error("FrmNhapKhoThuongMai02_FormClosing: Exception occurred", ex);
                // Nếu có lỗi, vẫn cho phép đóng form (không cancel)
                e.Cancel = false;
            }
        }

        #endregion

        #region ========== DATA OPERATIONS ==========

        /// <summary>
        /// Reset form về trạng thái ban đầu
        /// </summary>
        private void ResetForm()
        {
            try
            {
                _logger.Debug("ResetForm: Resetting form to initial state");

                // Clear master control
                ucStockInMaster1.ClearData();

                // Clear detail control
                ucStockInDetail1.ClearData();

                // Reset tổng về 0
                ucStockInMaster1.UpdateTotals(0, 0, 0, 0);

                // Reset state
                _currentStockInId = Guid.Empty;
                _isClosingAfterSave = false; // Reset flag khi reset form
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
        /// Lưu dữ liệu async
        /// Đảm bảo validate data từ các user control hợp lệ trước khi gọi BLL
        /// </summary>
        private async Task<bool> SaveDataAsync()
        {
            try
            {
                _logger.Debug("SaveDataAsync: Starting save operation");

                // ========== BƯỚC 1: VALIDATE VÀ LẤY DỮ LIỆU TỪ MASTER CONTROL ==========
                _logger.Debug("SaveDataAsync: Step 1 - Validating Master control");
                var masterDto = ucStockInMaster1.GetDto();
                if (masterDto == null)
                {
                    _logger.Warning("SaveDataAsync: Master validation failed - GetDto() returned null");
                    MsgBox.ShowWarning("Vui lòng kiểm tra và điền đầy đủ thông tin phiếu nhập kho", "Cảnh báo", this);
                    return false;
                }

                // Validate thêm business rules cho Master
                if (masterDto.WarehouseId == Guid.Empty)
                {
                    _logger.Warning("SaveDataAsync: Master validation failed - WarehouseId is Empty");
                    MsgBox.ShowWarning("Vui lòng chọn kho nhập", "Cảnh báo", this);
                    return false;
                }

                _logger.Debug("SaveDataAsync: Master validation passed, WarehouseId={0}, StockInNumber={1}", 
                    masterDto.WarehouseId, masterDto.StockInNumber);

                // ========== BƯỚC 2: VALIDATE VÀ LẤY DỮ LIỆU TỪ DETAIL CONTROL ==========
                _logger.Debug("SaveDataAsync: Step 2 - Validating Detail control");
                
                // Validate tất cả các rows trong grid
                if (!ucStockInDetail1.ValidateAll())
                {
                    _logger.Warning("SaveDataAsync: Detail validation failed - ValidateAll() returned false");
                    // ValidateAll() đã hiển thị thông báo lỗi chi tiết
                    return false;
                }

                // Lấy danh sách detail DTOs
                var detailDtos = ucStockInDetail1.GetDetails();
                if (detailDtos == null || detailDtos.Count == 0)
                {
                    _logger.Warning("SaveDataAsync: No details found");
                    MsgBox.ShowWarning("Vui lòng thêm ít nhất một dòng chi tiết", "Cảnh báo", this);
                    return false;
                }

                // Validate thêm business rules cho từng detail
                var validationErrors = new List<string>();
                for (int i = 0; i < detailDtos.Count; i++)
                {
                    var detail = detailDtos[i];
                    var lineNumber = detail.LineNumber > 0 ? detail.LineNumber : (i + 1);

                    if (detail.ProductVariantId == Guid.Empty)
                    {
                        validationErrors.Add($"Dòng {lineNumber}: Vui lòng chọn hàng hóa");
                    }

                    if (detail.StockInQty <= 0)
                    {
                        validationErrors.Add($"Dòng {lineNumber}: Số lượng nhập phải lớn hơn 0");
                    }

                    if (detail.UnitPrice < 0)
                    {
                        validationErrors.Add($"Dòng {lineNumber}: Đơn giá không được âm");
                    }

                    if (detail.Vat < 0 || detail.Vat > 100)
                    {
                        validationErrors.Add($"Dòng {lineNumber}: VAT phải từ 0 đến 100");
                    }
                }

                if (validationErrors.Any())
                {
                    _logger.Warning("SaveDataAsync: Detail business rules validation failed, Errors={0}", 
                        string.Join("; ", validationErrors));
                    MsgBox.ShowError($"Có lỗi trong dữ liệu chi tiết:\n\n{string.Join("\n", validationErrors)}", "Lỗi validation", this);
                    return false;
                }

                _logger.Debug("SaveDataAsync: Detail validation passed, DetailCount={0}", detailDtos.Count);

                // ========== BƯỚC 3: TẤT CẢ VALIDATION ĐÃ PASS - GỌI BLL ĐỂ LƯU ==========
                _logger.Debug("SaveDataAsync: Step 3 - All validations passed, calling BLL.SaveAsync");
                
                // Tất cả validation đã được thực hiện ở bước 1 và 2
                // StockInBll.SaveAsync sẽ có thêm validation layer nhưng chủ yếu là double-check
                var savedMasterId = await _stockInBll.SaveAsync(masterDto, detailDtos);

                // ========== BƯỚC 4: CẬP NHẬT STATE SAU KHI LƯU THÀNH CÔNG ==========
                _logger.Debug("SaveDataAsync: Step 4 - Updating state after successful save");
                
                // Cập nhật ID sau khi lưu
                masterDto.Id = savedMasterId;
                _currentStockInId = savedMasterId;

                // Set master ID cho detail control để đồng bộ
                ucStockInDetail1.SetStockInMasterId(savedMasterId);

                _logger.Info("SaveDataAsync: Save operation completed successfully, MasterId={0}", savedMasterId);
                return true;
            }
            catch (ArgumentException argEx)
            {
                // Lỗi validation từ BLL (đã được validate nhưng double-check)
                _logger.Warning("SaveDataAsync: Validation error from BLL: {0}", argEx.Message);
                MsgBox.ShowWarning(argEx.Message, "Cảnh báo", this);
                return false;
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
