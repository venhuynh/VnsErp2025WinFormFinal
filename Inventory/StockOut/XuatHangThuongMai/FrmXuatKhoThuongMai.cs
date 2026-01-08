using Bll.Inventory.InventoryManagement;
using Bll.Inventory.StockInOut;
using Common.Common;
using Common.Utils;
using DevExpress.XtraEditors;
using DTO.Inventory;
using DTO.Inventory.InventoryManagement;
using Inventory.OverlayForm;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventory.StockOut.XuatHangThuongMai;

public partial class FrmXuatKhoThuongMai : XtraForm
{
    #region ========== FIELDS & PROPERTIES ==========

    /// <summary>
    /// Business Logic Layer cho StockIn (dùng chung cho cả nhập và xuất kho)
    /// </summary>
    private readonly StockInOutBll _stockInBll = new StockInOutBll();

    /// <summary>
    /// Business Logic Layer cho ProductVariantIdentifierHistory
    /// </summary>
    private readonly ProductVariantIdentifierHistoryBll _productVariantIdentifierHistoryBll = new ProductVariantIdentifierHistoryBll();

    /// <summary>
    /// Logger để ghi log các sự kiện
    /// </summary>
    private readonly ILogger _logger = LoggerFactory.CreateLogger(LogCategory.UI);

    /// <summary>
    /// Trạng thái có thay đổi dữ liệu chưa lưu
    /// </summary>
    private bool _hasUnsavedChanges;

    /// <summary>
    /// ID phiếu xuất kho hiện tại (nếu đang edit)
    /// </summary>
    private Guid _currentStockInOutMaster;

    /// <summary>
    /// Flag đánh dấu đang trong quá trình đóng form sau khi lưu thành công
    /// Dùng để tránh hỏi lại khi Close() được gọi từ BeginInvoke
    /// </summary>
    private bool _isClosingAfterSave;

    /// <summary>
    /// Danh sách ProductVariantIdentifierDto từ form đọc QR code
    /// </summary>
    private List<ProductVariantIdentifierDto> _productVariantIdentifierDtos = new List<ProductVariantIdentifierDto>();

    #endregion

    #region ========== CONSTRUCTOR ==========

    /// <summary>
    /// Constructor mặc định (tạo phiếu mới)
    /// </summary>
    public FrmXuatKhoThuongMai()
    {
        InitializeComponent();
        Load += FrmXuatKhoThuongMai_Load;
        _currentStockInOutMaster = Guid.Empty;
    }

    /// <summary>
    /// Constructor với ID phiếu xuất kho (mở để xem/sửa)
    /// </summary>
    /// <param name="stockInOutMaster">ID phiếu xuất kho</param>
    public FrmXuatKhoThuongMai(Guid stockInOutMaster)
    {
        InitializeComponent();
        Load += FrmXuatKhoThuongMai_Load;

        // Gán ID phiếu xuất kho hiện tại
        _currentStockInOutMaster = stockInOutMaster;
    }

    #endregion

    #region ========== INITIALIZATION ==========

    /// <summary>
    /// Event handler khi form được load
    /// </summary>
    private async void FrmXuatKhoThuongMai_Load(object sender, EventArgs e)
    {
        try
        {
            // Setup event handlers
            SetupEvents();

            // Đảm bảo form đã được hiển thị và sẵn sàng trước khi show splash screen
            // Refresh form để đảm bảo nó đã được render
            Refresh();
            Application.DoEvents(); // Cho phép form render xong

            // Load datasource với SplashScreen (với owner là form này)
            //await LoadDataSourcesAsync();

            // Nếu _currentStockInOutMaster có giá trị thì load dữ liệu vào UI của 2 UserControl
            if (_currentStockInOutMaster != Guid.Empty)
            {

                // Load dữ liệu từ ID vào các user controls
                await ucXuatHangThuongMaiMasterDto1.LoadDataAsync(_currentStockInOutMaster);
                await ucXuatHangThuongMaiDetailDto1.LoadDataAsyncForEdit(_currentStockInOutMaster);
            }
            else
            {
                // Reset form về trạng thái ban đầu (tạo phiếu mới)
                ResetForm();
            }

        }
        catch (Exception ex)
        {
            _logger.Error("FrmXuatKhoThuongMai_Load: Exception occurred", ex);
            MsgBox.ShowError($"Lỗi khởi tạo form: {ex.Message}");
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
            ReloadDataSourceBarButtonItem.ItemClick += ReloadDataSourceBarButtonItem_ItemClick;
            ReadQrCodeBarButtonItem.ItemClick += ReadQrCodeBarButtonItem_ItemClick;
            LuuPhieuBarButtonItem.ItemClick += LuuPhieuBarButtonItem_ItemClick;
            InPhieuBarButtonItem.ItemClick += InPhieuBarButtonItem_ItemClick;
            ThemHinhAnhBarButtonItem.ItemClick += ThemHinhAnhBarButtonItem_ItemClick;
            CloseBarButtonItem.ItemClick += CloseBarButtonItem_ItemClick;

            // Form events
            FormClosing += FrmXuatKhoThuongMai_FormClosing;
            KeyDown += FrmXuatKhoThuongMai_KeyDown;
            KeyPreview = true; // Cho phép form xử lý phím tắt trước

            // Detail control events - theo dõi thay đổi để đánh dấu có thay đổi chưa lưu và cập nhật tổng lên master
            ucXuatHangThuongMaiDetailDto1.DetailDataChanged += UcXuatHangThuongMaiDetailDto1_DetailDataChanged;

            // Setup phím tắt và hiển thị hướng dẫn
            SetupKeyboardShortcuts();
            UpdateHotKeyBarStaticItem();

            // Setup SuperToolTips
            SetupSuperToolTips();

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
    }

    /// <summary>
    /// Đánh dấu form đã lưu (không còn thay đổi)
    /// </summary>
    private void MarkAsSaved()
    {
        _hasUnsavedChanges = false;
        _isClosingAfterSave = false; // Reset flag khi đánh dấu đã lưu
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
            // F4: Đọc QR Code
            // F5: Thêm hình ảnh
            // ESC: Đóng form

            // Lưu ý: DevExpress BarButtonItem không hỗ trợ trực tiếp ItemShortcut
            // Nên sẽ xử lý qua KeyDown event của form

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
                @"<b><color=Blue>F4</color></b> Đọc QR Code | " +
                @"<b><color=Blue>F5</color></b> Thêm hình ảnh | " +
                @"<b><color=Blue>ESC</color></b> Đóng | " +
                @"<b><color=Blue>Insert</color></b> Thêm dòng | " +
                @"<b><color=Blue>Delete</color></b> Xóa dòng | " +
                @"<b><color=Blue>Enter</color></b> Hoàn thành dòng";

            HotKeyBarStaticItem.Caption = hotKeyText;
            HotKeyBarStaticItem.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;

        }
        catch (Exception ex)
        {
            _logger.Error("UpdateHotKeyBarStaticItem: Exception occurred", ex);
        }
    }

    /// <summary>
    /// Thiết lập SuperToolTip cho các BarButtonItem
    /// </summary>
    private void SetupSuperToolTips()
    {
        try
        {
            // SuperToolTip cho ReloadDataSourceBarButtonItem
            if (ReloadDataSourceBarButtonItem != null)
            {
                SuperToolTipHelper.SetBarButtonSuperTip(
                    ReloadDataSourceBarButtonItem,
                    title: "<b><color=Blue>🔄 Làm mới dữ liệu</color></b>",
                    content: "Làm mới lại các datasource trong form.<br/><br/><b>Chức năng:</b><br/>• Reload danh sách biến thể sản phẩm trong chi tiết<br/>• Reload danh sách kho và khách hàng trong master<br/><br/><color=Gray>Lưu ý:</color> Sử dụng khi dữ liệu lookup đã thay đổi trong database và cần cập nhật lại."
                );
            }
        }
        catch (Exception ex)
        {
            _logger.Error("SetupSuperToolTips: Exception occurred", ex);
        }
    }

    #endregion

    #region ========== EVENT HANDLERS ==========

    /// <summary>
    /// Event handler cho nút Reload DataSource
    /// </summary>
    private async void ReloadDataSourceBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
        try
        {
            // Disable button để tránh double-click
            ReloadDataSourceBarButtonItem.Enabled = false;

            try
            {
                // Reload datasource cho cả 2 UserControl
                await Task.WhenAll(
                    ucXuatHangThuongMaiMasterDto1.LoadLookupDataAsync(),
                    ucXuatHangThuongMaiDetailDto1.ReloadProductVariantDataSourceAsync()
                );

                AlertHelper.ShowSuccess("Đã làm mới dữ liệu thành công!", "Thành công", this);
            }
            finally
            {
                // Re-enable button
                ReloadDataSourceBarButtonItem.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            _logger.Error("ReloadDataSourceBarButtonItem_ItemClick: Exception occurred", ex);
            MsgBox.ShowError($"Lỗi làm mới dữ liệu: {ex.Message}");
        }
    }

    /// <summary>
    /// Event handler cho nút Nhập lại
    /// </summary>
    private void NhapLaiBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
        try
        {

            // Kiểm tra có thay đổi chưa lưu không
            if (_hasUnsavedChanges)
            {
                var confirm = MsgBox.ShowYesNo(
                    "Bạn có thay đổi chưa lưu. Bạn có chắc chắn muốn nhập lại?",
                    "Xác nhận nhập lại",
                    this);

                if (!confirm)
                {
                    return;
                }
            }

            ResetForm();
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

            // Disable button để tránh double-click
            LuuPhieuBarButtonItem.Enabled = false;

            try
            {
                // Validate và lưu dữ liệu
                var success = await SaveDataAsync();

                if (!success) return;
                MsgBox.ShowSuccess("Lưu phiếu xuất kho thành công!", "Thành công", this);
                MarkAsSaved();
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

            // Lấy StockInOutMasterId từ _currentStockInOutMaster (phải đã được lưu)
            Guid stockInOutMasterId;

            // Kiểm tra phiếu đã được lưu chưa
            if (_currentStockInOutMaster != Guid.Empty)
            {
                stockInOutMasterId = _currentStockInOutMaster;
            }
            else
            {
                // Phiếu chưa được lưu - kiểm tra có thay đổi chưa lưu không
                if (_hasUnsavedChanges)
                {
                    // Hỏi người dùng có muốn lưu trước không
                    if (MsgBox.ShowYesNo(
                            "Phiếu xuất kho chưa được lưu. Bạn có muốn lưu trước khi in không?",
                            "Xác nhận",
                            this))
                    {
                        // Gọi nút Lưu để lưu phiếu
                        LuuPhieuBarButtonItem_ItemClick(null, null);

                        // Đợi cho đến khi lưu hoàn tất (tối đa 10 giây)
                        var timeout = TimeSpan.FromSeconds(10);
                        var startTime = DateTime.Now;
                        while (_currentStockInOutMaster == Guid.Empty && (DateTime.Now - startTime) < timeout)
                        {
                            await Task.Delay(100);
                        }

                        // Kiểm tra lại sau khi lưu
                        if (_currentStockInOutMaster != Guid.Empty)
                        {
                            stockInOutMasterId = _currentStockInOutMaster;
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
                        return;
                    }
                }
                else
                {
                    // Không có thay đổi chưa lưu và chưa có ID - yêu cầu lưu
                    MsgBox.ShowWarning(
                        "Vui lòng nhập và lưu phiếu xuất kho trước khi in.",
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
                    "Không thể lấy ID phiếu xuất kho. Vui lòng thử lại.",
                    "Cảnh báo",
                    this);
                _logger.Warning("InPhieuBarButtonItem_ItemClick: StockInOutMasterId is still Empty after save attempt");
            }

            // In phiếu nhập kho với preview
            // TODO: Implement print functionality

        }
        catch (Exception ex)
        {
            _logger.Error("InPhieuBarButtonItem_ItemClick: Exception occurred", ex);
            MsgBox.ShowError($"Lỗi in phiếu: {ex.Message}");
        }
    }

    /// <summary>
    /// Event handler cho nút Đọc QR Code
    /// </summary>
    private async void ReadQrCodeBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
        try
        {
            // Mở form đọc QR code (không cần lưu phiếu trước)
            using (OverlayManager.ShowScope(this))
            {
                using (var frmGetIdentifier = new FrmGetIdentifierForStockOut())
                {
                    frmGetIdentifier.StartPosition = FormStartPosition.CenterParent;
                    
                    if (frmGetIdentifier.ShowDialog(this) == DialogResult.OK)
                    {
                        // Lấy danh sách chi tiết từ form đọc QR code
                        var newDetails = frmGetIdentifier.GetStockInOutDetailList();

                        // Lấy danh sách identifier values từ form đọc QR code
                        if (frmGetIdentifier.ResultIdentifierValues != null)
                        {
                            _productVariantIdentifierDtos = frmGetIdentifier.ResultIdentifierValues;
                        }

                        if (newDetails != null && newDetails.Count > 0)
                        {
                            // Thêm hoặc merge vào grid hiện tại
                            await ucXuatHangThuongMaiDetailDto1.AddOrMergeDetailsAsync(newDetails);
                            
                            // Đánh dấu có thay đổi
                            MarkAsChanged();
                            
                            AlertHelper.ShowSuccess($"Đã thêm {newDetails.Count} sản phẩm từ QR code vào phiếu xuất kho.", "Thành công", this);
                        }
                        else
                        {
                            AlertHelper.ShowInfo("Không có sản phẩm nào được thêm vào.", "Thông tin", this);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.Error("ReadQrCodeBarButtonItem_ItemClick: Exception occurred", ex);
            MsgBox.ShowError($"Lỗi đọc QR code: {ex.Message}");
        }
    }

    /// <summary>
    /// Event handler cho nút Thêm hình ảnh
    /// </summary>
    private void ThemHinhAnhBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
        try
        {
            // Lấy StockInOutMasterId từ _currentStockInOutMaster (phải đã được lưu)
            Guid stockInOutMasterId = Guid.Empty;

            // Kiểm tra phiếu đã được lưu chưa
            if (_currentStockInOutMaster != Guid.Empty)
            {
                stockInOutMasterId = _currentStockInOutMaster;
            }
            else
            {
                // Phiếu chưa được lưu - kiểm tra có thay đổi chưa lưu không
                if (_hasUnsavedChanges)
                {
                    // Hỏi người dùng có muốn lưu trước không
                    if (MsgBox.ShowYesNo(
                            "Phiếu xuất kho chưa được lưu. Bạn có muốn lưu trước khi thêm hình ảnh không?",
                            "Xác nhận",
                            this))
                    {
                        // Gọi nút Lưu để lưu phiếu
                        LuuPhieuBarButtonItem_ItemClick(null, null);

                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    // Không có thay đổi chưa lưu và chưa có ID - yêu cầu lưu
                    MsgBox.ShowError(
                        "Vui lòng nhập và lưu phiếu xuất kho trước khi thêm hình ảnh.",
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
                    "Không thể lấy ID phiếu xuất kho. Vui lòng thử lại.",
                    "Cảnh báo",
                    this);
                _logger.Warning("ThemHinhAnhBarButtonItem_ItemClick: StockInOutMasterId is still Empty after save attempt");
                return;
            }

            // Mở form thêm hình ảnh với StockInOutMasterId (sử dụng OverlayManager để hiển thị)
            using (OverlayManager.ShowScope(this))
            {
                using (var frmAddImages = new FrmStockInOutAddImagesFromFile(stockInOutMasterId))
                {
                    frmAddImages.StartPosition = FormStartPosition.CenterParent;
                    frmAddImages.ShowDialog(this);
                }
            }
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
    private void FrmXuatKhoThuongMai_KeyDown(object sender, KeyEventArgs e)
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
                    // F4: Đọc QR Code
                    e.Handled = true;
                    ReadQrCodeBarButtonItem_ItemClick(null, null);
                    break;

                case Keys.F5:
                    // F5: Thêm hình ảnh
                    e.Handled = true;
                    ThemHinhAnhBarButtonItem_ItemClick(null, null);
                    break;

                case Keys.Escape:
                    // ESC: Đóng form (chỉ khi không đang edit trong grid)
                    if (!(ActiveControl is BaseEdit baseEdit && baseEdit.IsEditorActive))
                    {
                        e.Handled = true;
                        CloseBarButtonItem_ItemClick(null, null);
                    }
                    break;
            }
        }
        catch (Exception ex)
        {
            _logger.Error("FrmXuatKhoThuongMai_KeyDown: Exception occurred", ex);
            MsgBox.ShowError($"Lỗi xử lý phím tắt: {ex.Message}");
        }
    }

    /// <summary>
    /// Event handler khi dữ liệu detail thay đổi
    /// </summary>
    private void UcXuatHangThuongMaiDetailDto1_DetailDataChanged(object sender, EventArgs e)
    {
        try
        {
            // Đánh dấu có thay đổi chưa lưu
            MarkAsChanged();

            // Tính tổng từ detail
            var (totalQuantity, totalAmount, totalVat, totalAmountIncludedVat) = ucXuatHangThuongMaiDetailDto1.CalculateTotals();

            // Cập nhật tổng lên master
            ucXuatHangThuongMaiMasterDto1.UpdateTotals(totalQuantity, totalAmount, totalVat, totalAmountIncludedVat);
        }
        catch (Exception ex)
        {
            _logger.Error("UcXuatHangThuongMaiDetailDto1_DetailDataChanged: Exception occurred", ex);
            MsgBox.ShowError($"Lỗi cập nhật tổng hợp: {ex.Message}");
        }
    }

    /// <summary>
    /// Event handler khi form đang đóng
    /// Sử dụng ShowYesNoCancel để đơn giản hóa logic: Yes = Lưu và đóng, No = Đóng không lưu, Cancel = Hủy
    /// </summary>
    private async void FrmXuatKhoThuongMai_FormClosing(object sender, FormClosingEventArgs e)
    {
        try
        {
            // Nếu đang trong quá trình đóng sau khi lưu thành công, cho phép đóng luôn
            if (_isClosingAfterSave)
            {
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

                switch (result)
                {
                    case DialogResult.Yes:
                        // Người dùng chọn "Lưu và đóng"
                        // Cancel việc đóng form tạm thời để lưu dữ liệu
                        e.Cancel = true;

                        try
                        {
                            // Lưu dữ liệu
                            var saveSuccess = await SaveDataAsync();

                            if (saveSuccess)
                            {
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
                                _logger.Warning("FrmXuatKhoThuongMai_FormClosing: Save failed, form will remain open");
                                e.Cancel = true;
                            }
                        }
                        catch (Exception saveEx)
                        {
                            _logger.Error("FrmXuatKhoThuongMai_FormClosing: Exception during save operation", saveEx);
                            // Lỗi khi lưu, giữ form mở
                            e.Cancel = true;
                        }

                        break;
                    case DialogResult.No:
                        // Người dùng chọn "Đóng không lưu" - cho phép đóng form
                        e.Cancel = false;
                        break;
                    // DialogResult.Cancel
                    default:
                        // Người dùng chọn "Hủy" - không muốn đóng, giữ form mở
                        e.Cancel = true;
                        break;
                }
            }
            else
            {
                // Không có thay đổi chưa lưu, cho phép đóng form
                e.Cancel = false;
            }

        }
        catch (Exception ex)
        {
            _logger.Error("FrmXuatKhoThuongMai_FormClosing: Exception occurred", ex);
            // Nếu có lỗi, vẫn cho phép đóng form (không cancel)
            e.Cancel = false;
        }
    }

    #endregion

    #region ========== DATA OPERATIONS ==========

    #region Helper Methods - DTO to Entity Conversion

    #endregion

    /// <summary>
    /// Reset form về trạng thái ban đầu
    /// </summary>
    private void ResetForm()
    {
        try
        {
            // Clear master control
            ucXuatHangThuongMaiMasterDto1.ClearData();

            // Clear detail control
            ucXuatHangThuongMaiDetailDto1.ClearData();

            // Reset tổng về 0
            ucXuatHangThuongMaiMasterDto1.UpdateTotals(0, 0, 0, 0);

            // Reset state
            _currentStockInOutMaster = Guid.Empty;
            _isClosingAfterSave = false; // Reset flag khi reset form
            MarkAsSaved();
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

            // ========== BƯỚC 1: VALIDATE VÀ LẤY DỮ LIỆU TỪ MASTER CONTROL ==========
            var masterDto = ucXuatHangThuongMaiMasterDto1.GetDto();
            if (masterDto == null)
            {
                _logger.Warning("SaveDataAsync: Master validation failed - GetDto() returned null");
                MsgBox.ShowWarning("Vui lòng kiểm tra và điền đầy đủ thông tin phiếu xuất kho", "Cảnh báo", this);
                return false;
            }

            // Validate thêm business rules cho Master
            if (masterDto.WarehouseId == Guid.Empty)
            {
                _logger.Warning("SaveDataAsync: Master validation failed - WarehouseId is Empty");
                MsgBox.ShowWarning("Vui lòng chọn kho xuất", "Cảnh báo", this);
                return false;
            }

            // ========== BƯỚC 2: VALIDATE VÀ LẤY DỮ LIỆU TỪ DETAIL CONTROL ==========
            // Validate tất cả các rows trong grid
            if (!ucXuatHangThuongMaiDetailDto1.ValidateAll())
            {
                _logger.Warning("SaveDataAsync: Detail validation failed - ValidateAll() returned false");
                // ValidateAll() đã hiển thị thông báo lỗi chi tiết
                return false;
            }

            // Lấy danh sách detail DTOs (GetDetails() trả về List<StockInOutDetailForUIDto>)
            var detailDtos = ucXuatHangThuongMaiDetailDto1.GetDetails();
            if (detailDtos == null || detailDtos.Count == 0)
            {
                _logger.Warning("SaveDataAsync: No details found");
                MsgBox.ShowWarning("Vui lòng thêm ít nhất một dòng chi tiết", "Cảnh báo", this);
                return false;
            }

            // ========== BƯỚC 3: TẤT CẢ VALIDATION ĐÃ PASS - GỌI BLL ĐỂ LƯU ==========
            // Dựa vào giá trị của _currentStockInOutMaster để xác định là Insert hay Update
            // Nếu _currentStockInOutMaster == Guid.Empty: Tạo mới (Insert)
            // Nếu _currentStockInOutMaster != Guid.Empty: Cập nhật (Update)

            Guid savedMasterId;

            if (_currentStockInOutMaster == Guid.Empty)
            {
                // Trường hợp tạo mới: Gọi SaveAsync để insert
                _logger.Info("SaveDataAsync: Creating new warranty input voucher");
                savedMasterId = await _stockInBll.SaveAsync(masterDto, detailDtos);
            }
            else
            {
                // Trường hợp cập nhật: Set ID vào masterDto và gọi UpdateAsync để update
                _logger.Info("SaveDataAsync: Updating existing warranty input voucher, Id={0}", _currentStockInOutMaster);
                masterDto.Id = _currentStockInOutMaster;
                savedMasterId = await _stockInBll.UpdateAsync(masterDto, detailDtos);
            }

            // ========== BƯỚC 4: CẬP NHẬT STATE SAU KHI LƯU THÀNH CÔNG ==========
            // Cập nhật ID sau khi lưu
            masterDto.Id = savedMasterId;
            _currentStockInOutMaster = savedMasterId;

            // Set master ID cho detail control để đồng bộ
            ucXuatHangThuongMaiDetailDto1.SetStockOutMasterId(savedMasterId);

            // ========== BƯỚC 5: CẬP NHẬT PRODUCTVARIANTIDENTIFIERHISTORY ==========
            // Cập nhật lịch sử thay đổi cho các identifier đã quét từ QR code
            if (_productVariantIdentifierDtos != null && _productVariantIdentifierDtos.Count > 0)
            {
                await UpdateProductVariantIdentifierHistoryAsync(masterDto);
            }

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

    /// <summary>
    /// Cập nhật ProductVariantIdentifierHistory cho các identifier đã quét từ QR code
    /// </summary>
    /// <param name="masterDto">Thông tin phiếu xuất kho</param>
    private async Task UpdateProductVariantIdentifierHistoryAsync(StockInOutMasterForUIDto masterDto)
    {
        try
        {
            if (masterDto == null || _productVariantIdentifierDtos == null || _productVariantIdentifierDtos.Count == 0)
            {
                return;
            }

            _logger.Info("UpdateProductVariantIdentifierHistoryAsync: Bắt đầu cập nhật lịch sử cho {0} identifier", _productVariantIdentifierDtos.Count);

            // Tạo Value string chứa thông tin phiếu xuất kho
            var valueParts = new List<string>();

            // Thông tin khách hàng
            if (!string.IsNullOrWhiteSpace(masterDto.CustomerName))
            {
                valueParts.Add($"Khách hàng: {masterDto.CustomerName}");
            }

            // Thông tin kho xuất
            if (!string.IsNullOrWhiteSpace(masterDto.WarehouseName))
            {
                valueParts.Add($"Kho: {masterDto.WarehouseName}");
            }

            // Thông tin số phiếu
            if (!string.IsNullOrWhiteSpace(masterDto.VoucherNumber))
            {
                valueParts.Add($"Số phiếu: {masterDto.VoucherNumber}");
            }

            // Thông tin loại xuất kho (lấy từ enum bằng ApplicationEnumUtils)
            var loaiNhapXuatKhoName = ApplicationEnumUtils.GetDescription(masterDto.LoaiNhapXuatKho);
            if (!string.IsNullOrWhiteSpace(loaiNhapXuatKhoName))
            {
                valueParts.Add($"Loại: {loaiNhapXuatKhoName}");
            }

            // Gộp tất cả thành một chuỗi (chỉ khi có ít nhất một phần tử)
            var value = valueParts.Count > 0 
                ? string.Join(" | ", valueParts) 
                : string.Empty;

            // Lấy ChangedBy từ ApplicationUser hiện tại (nếu có)
            Guid? changedBy = null;
            try
            {
                // Có thể lấy từ ApplicationUserManager hoặc tương tự
                // Tạm thời để null, có thể mở rộng sau
            }
            catch
            {
                // Ignore nếu không lấy được
            }

            // Lặp qua từng identifier và tạo history
            var successCount = 0;
            var errorCount = 0;

            foreach (var identifierDto in _productVariantIdentifierDtos)
            {
                try
                {
                    // Tạo DTO lịch sử với ChangeTypeEnum là Xuat (xuất kho)
                    // Tham khảo logic từ UcProductVariantIdentifierTransactionHistory
                    var historyDto = new ProductVariantIdentifierHistoryDto
                    {
                        Id = Guid.NewGuid(),
                        ProductVariantIdentifierId = identifierDto.Id,
                        ProductVariantId = identifierDto.ProductVariantId,
                        ProductVariantFullName = identifierDto.ProductVariantFullName, // Thêm ProductVariantFullName như trong UcProductVariantIdentifierTransactionHistory
                        ChangeTypeEnum = ProductVariantIdentifierHistoryChangeTypeEnum.Xuat,
                        ChangeDate = masterDto.StockOutDate != default(DateTime)
                            ? masterDto.StockOutDate
                            : DateTime.Now,
                        Value = value,
                        Notes = $"Xuất kho từ phiếu: {masterDto.VoucherNumber ?? "N/A"}",
                        ChangedBy = changedBy
                    };

                    // Lưu bản ghi lịch sử
                    _productVariantIdentifierHistoryBll.SaveOrUpdate(historyDto);
                    successCount++;

                    _logger.Debug("UpdateProductVariantIdentifierHistoryAsync: Đã lưu lịch sử cho identifier Id={0}", identifierDto.Id);
                }
                catch (Exception ex)
                {
                    errorCount++;
                    _logger.Error($"UpdateProductVariantIdentifierHistoryAsync: Lỗi khi lưu lịch sử cho identifier Id={identifierDto.Id}: {ex.Message}", ex);
                    // Tiếp tục với identifier tiếp theo, không throw exception
                }
            }

            _logger.Info("UpdateProductVariantIdentifierHistoryAsync: Hoàn thành - Thành công: {0}, Lỗi: {1}", successCount, errorCount);

            // Clear danh sách identifier sau khi đã cập nhật xong
            _productVariantIdentifierDtos.Clear();
        }
        catch (Exception ex)
        {
            _logger.Error("UpdateProductVariantIdentifierHistoryAsync: Exception occurred", ex);
            // Không throw exception để không block việc lưu phiếu
            // Chỉ log lỗi
        }
    }

    #endregion
}