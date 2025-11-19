using Bll.Inventory;
using Common.Common;
using Common.Utils;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

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

                // Detail control events - theo dõi thay đổi để đánh dấu có thay đổi chưa lưu và cập nhật tổng lên master
                ucStockInDetail1.DetailDataChanged += UcStockInDetail1_DetailDataChanged;

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
            _logger.Debug("MarkAsSaved: Form marked as saved");
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
        private void FrmNhapKhoThuongMai02_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                _logger.Debug("FrmNhapKhoThuongMai02_FormClosing: Form closing, HasUnsavedChanges={0}", _hasUnsavedChanges);

                // Kiểm tra có thay đổi chưa lưu không
                if (_hasUnsavedChanges)
                {
                    var confirm = MsgBox.ShowYesNo(
                        "Bạn có thay đổi chưa lưu. Bạn có chắc chắn muốn đóng form?",
                        "Xác nhận đóng",
                        this);
                    
                    _logger.Debug("FrmNhapKhoThuongMai02_FormClosing: User confirmation result={0}", confirm);
                    
                    if (!confirm)
                    {
                        // Người dùng chọn "Không" - không muốn đóng, cancel việc đóng form
                        e.Cancel = true;
                        _logger.Debug("FrmNhapKhoThuongMai02_FormClosing: User cancelled closing, form will remain open");
                        return;
                    }
                    
                    // Người dùng chọn "Có" - muốn đóng, đảm bảo không cancel
                    e.Cancel = false;
                    _logger.Debug("FrmNhapKhoThuongMai02_FormClosing: User confirmed closing, form will close");
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

                // 3. Set StockInMasterId cho các detail rows
                if (masterDto.Id == Guid.Empty)
                {
                    // Tạo mới ID cho master
                    masterDto.Id = Guid.NewGuid();
                    _logger.Debug("SaveDataAsync: Generated new master ID={0}", masterDto.Id);
                }

                // Set master ID cho tất cả detail rows
                ucStockInDetail1.SetStockInMasterId(masterDto.Id);
                foreach (var detail in detailDtos)
                {
                    detail.StockInOutMasterId = masterDto.Id;
                }

                // 4. Lưu dữ liệu vào database
                // TODO: Implement StockInBll.SaveAsync method
                // await _stockInBll.SaveAsync(masterDto, detailDtos);

                // Temporary: Show message that save is not yet implemented
                _logger.Warning("SaveDataAsync: StockInBll.SaveAsync not yet implemented");
                MsgBox.ShowWarning(
                    "Chức năng lưu dữ liệu đang được phát triển.\n" +
                    $"Master: {masterDto.StockInNumber}\n" +
                    $"Details: {detailDtos.Count} dòng",
                    "Thông báo",
                    this);

                // Update current ID
                _currentStockInId = masterDto.Id;

                _logger.Info("SaveDataAsync: Save operation completed (simulated)");
                return true; // Return true for now since we're simulating
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
