using Common.Common;
using Common.Utils;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bll.Inventory.StockIn;
using System.Linq;

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
        private Guid _currentStockInId = Guid.Empty;

        /// <summary>
        /// Flag đánh dấu đang trong quá trình đóng form sau khi lưu thành công
        /// Dùng để tránh hỏi lại khi Close() được gọi từ BeginInvoke
        /// </summary>
        private bool _isClosingAfterSave;

        #endregion

        #region ========== CONSTRUCTOR ==========

        public FrmNhapKhoThuongMai02()
        {
            InitializeComponent();
            this.Load += FrmNhapKhoThuongMai02_Load;
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

                // Load datasource với SplashScreen
                await LoadDataSourcesAsync();

                // Reset form về trạng thái ban đầu
                ResetForm();

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
        /// </summary>
        private async Task LoadDataSourcesAsync()
        {
            try
            {
                _logger.Debug("LoadDataSourcesAsync: Starting to load datasources");

                // Hiển thị SplashScreen
                SplashScreenHelper.ShowVnsSplashScreen("VnsErp2025", "Đang tải dữ liệu...");

                try
                {
                    // Load datasource cho UcStockInMaster (Warehouse và Supplier)
                    _logger.Debug("LoadDataSourcesAsync: Loading lookup data for UcStockInMaster");
                    await ucStockInMaster1.LoadLookupDataAsync();

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
                this.FormClosing += FrmNhapKhoThuongMai02_FormClosing;
                this.KeyDown += FrmNhapKhoThuongMai02_KeyDown;
                this.KeyPreview = true; // Cho phép form xử lý phím tắt trước

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
        private void InPhieuBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                _logger.Debug("InPhieuBarButtonItem_ItemClick: Print button clicked");

                // TODO: Implement print functionality
                MsgBox.ShowWarning("Chức năng in phiếu đang được phát triển.", "Thông báo", this);
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
        private void NhapBaoHanhBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                _logger.Debug("NhapBaoHanhBarButtonItem_ItemClick: Warranty input button clicked");

                // TODO: Implement warranty input functionality
                MsgBox.ShowWarning("Chức năng nhập bảo hành đang được phát triển.", "Thông báo", this);
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

                // TODO: Implement add image functionality
                MsgBox.ShowWarning("Chức năng thêm hình ảnh đang được phát triển.", "Thông báo", this);
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
                this.Close();
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
                    // Hỏi lần 1: Có muốn lưu và đóng không?
                    var saveAndClose = MsgBox.ShowYesNo(
                        "Bạn có thay đổi chưa lưu. Bạn có muốn lưu và đóng form?",
                        "Xác nhận đóng",
                        this,
                        yesButtonText: "Lưu và đóng",
                        noButtonText: "Đóng không lưu");
                    
                    _logger.Debug("FrmNhapKhoThuongMai02_FormClosing: Save and close result={0}", saveAndClose);
                    
                    if (saveAndClose)
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
                                this.BeginInvoke(new Action(() =>
                                {
                                    this.Close();
                                }));
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
                        
                        return;
                    }
                    else
                    {
                        // Người dùng chọn "Đóng không lưu", hỏi xác nhận lần 2
                        var confirmClose = MsgBox.ShowYesNo(
                            "Bạn có chắc chắn muốn đóng form mà không lưu thay đổi?",
                            "Xác nhận đóng",
                            this,
                            yesButtonText: "Đóng",
                            noButtonText: "Hủy");
                        
                        _logger.Debug("FrmNhapKhoThuongMai02_FormClosing: Confirm close without save result={0}", confirmClose);
                        
                        if (confirmClose)
                        {
                            // Người dùng xác nhận đóng không lưu
                            e.Cancel = false;
                            _logger.Debug("FrmNhapKhoThuongMai02_FormClosing: User confirmed closing without save, form will close");
                        }
                        else
                        {
                            // Người dùng chọn "Hủy" - không muốn đóng
                            e.Cancel = true;
                            _logger.Debug("FrmNhapKhoThuongMai02_FormClosing: User cancelled closing, form will remain open");
                        }
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
        /// </summary>
        private async Task<bool> SaveDataAsync()
        {
            try
            {
                _logger.Debug("SaveDataAsync: Starting save operation");

                // 1. Validate và lấy dữ liệu từ Master control
                var masterDto = ucStockInMaster1.GetDto();
                if (masterDto == null)
                {
                    _logger.Warning("SaveDataAsync: Master validation failed");
                    return false;
                }

                // 2. Validate và lấy dữ liệu từ Detail control
                if (!ucStockInDetail1.ValidateAll())
                {
                    _logger.Warning("SaveDataAsync: Detail validation failed");
                    return false;
                }

                var detailDtos = ucStockInDetail1.GetDetails();
                if (detailDtos == null || detailDtos.Count == 0)
                {
                    _logger.Warning("SaveDataAsync: No details found");
                    MsgBox.ShowWarning("Vui lòng thêm ít nhất một dòng chi tiết", "Cảnh báo", this);
                    return false;
                }

                // 3. Lưu dữ liệu vào database
                // Validation đã được thực hiện ở bước 1 và 2
                // StockInBll.SaveAsync sẽ tự động tạo ID mới nếu masterDto.Id == Guid.Empty
                var savedMasterId = await _stockInBll.SaveAsync(masterDto, detailDtos);

                // 4. Cập nhật ID sau khi lưu
                masterDto.Id = savedMasterId;
                _currentStockInId = savedMasterId;

                // Set master ID cho detail control để đồng bộ
                ucStockInDetail1.SetStockInMasterId(savedMasterId);

                _logger.Info("SaveDataAsync: Save operation completed successfully, MasterId={0}", savedMasterId);
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
