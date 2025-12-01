using Bll.Inventory.StockIn;
using Common.Common;
using Common.Utils;
using Dal.DataContext;
using DevExpress.XtraReports.UI;
using DTO.Inventory.StockOut.XuatLuuChuyenKho;
using Inventory.StockIn.InPhieu;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Inventory.StockIn.NhapThietBiMuon;

namespace Inventory.StockOut.XuatLuuChuyenKho;

public partial class FrmXuatLuuChuyenKho : DevExpress.XtraEditors.XtraForm
{
    #region ========== FIELDS & PROPERTIES ==========

    /// <summary>
    /// Business Logic Layer cho StockIn/StockOut (dùng chung cho cả nhập và xuất)
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
    /// ID phiếu xuất kho hiện tại (nếu đang edit)
    /// </summary>
    private Guid _currentStockOutId;

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
    public FrmXuatLuuChuyenKho()
    {
        InitializeComponent();
        Load += FrmXuatLuuChuyenKho_Load;
        _currentStockOutId = Guid.Empty;
    }

    /// <summary>
    /// Constructor với ID phiếu xuất kho (mở để xem/sửa)
    /// </summary>
    /// <param name="stockOutId">ID phiếu xuất kho</param>
    public FrmXuatLuuChuyenKho(Guid stockOutId)
    {
        InitializeComponent();
        Load += FrmXuatLuuChuyenKho_Load;

        // Gán ID phiếu xuất kho hiện tại
        _currentStockOutId = stockOutId;
    }

    #endregion

    #region ========== INITIALIZATION ==========

    /// <summary>
    /// Event handler khi form được load
    /// </summary>
    private async void FrmXuatLuuChuyenKho_Load(object sender, EventArgs e)
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

            // Nếu _currentStockOutId có giá trị thì load dữ liệu vào UI của 2 UserControl
            if (_currentStockOutId != Guid.Empty)
            {

                // Load dữ liệu từ ID vào các user controls
                //await LoadDataAsync(_currentStockOutId);

                //FIXME: Tạo hàm LoadDataAsync trong user controls để load dữ liệu từ _currentStockOutId
                await ucXuatChuyenKhoMaster1.LoadDataAsync(_currentStockOutId);
                await ucXuatLuuChuyenKhoDetail1.LoadDataAsyncForEdit(_currentStockOutId);
            }
            else
            {
                // Reset form về trạng thái ban đầu (tạo phiếu mới)
                ResetForm();
            }

        }
        catch (Exception ex)
        {
            _logger.Error("FrmXuatLuuChuyenKho_Load: Exception occurred", ex);
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
            LuuPhieuBarButtonItem.ItemClick += LuuPhieuBarButtonItem_ItemClick;
            InPhieuBarButtonItem.ItemClick += InPhieuBarButtonItem_ItemClick;
            NhapQuanLyTaiSanBarButtonItem.ItemClick += NhapQuanLyTaiSanBarButtonItem_ItemClick;
            ThemHinhAnhBarButtonItem.ItemClick += ThemHinhAnhBarButtonItem_ItemClick;
            CloseBarButtonItem.ItemClick += CloseBarButtonItem_ItemClick;

            // Form events
            FormClosing += FrmXuatLuuChuyenKho_FormClosing;
            KeyDown += FrmXuatLuuChuyenKho_KeyDown;
            KeyPreview = true; // Cho phép form xử lý phím tắt trước

            // Detail control events - theo dõi thay đổi để đánh dấu có thay đổi chưa lưu và cập nhật tổng lên master
            ucXuatLuuChuyenKhoDetail1.DetailDataChanged += UcXuatLuuChuyenKhoDetail1_DetailDataChanged;

            // Setup phím tắt và hiển thị hướng dẫn
            SetupKeyboardShortcuts();
            UpdateHotKeyBarStaticItem();

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
            // F4: Nhập bảo hành
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
                             @"<b><color=Blue>F4</color></b> Nhập bảo hành | " +
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

    #endregion

    #region ========== EVENT HANDLERS ==========

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

            // Lấy StockInOutMasterId từ _currentStockOutId (phải đã được lưu)
            Guid stockInOutMasterId;

            // Kiểm tra phiếu đã được lưu chưa
            if (_currentStockOutId != Guid.Empty)
            {
                stockInOutMasterId = _currentStockOutId;
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
                        while (_currentStockOutId == Guid.Empty && (DateTime.Now - startTime) < timeout)
                        {
                            await Task.Delay(100);
                        }

                        // Kiểm tra lại sau khi lưu
                        if (_currentStockOutId != Guid.Empty)
                        {
                            stockInOutMasterId = _currentStockOutId;
                        }
                        else
                        {
                            // Lưu thất bại hoặc timeout, không in phiếu
                            _logger.Warning(
                                "InPhieuBarButtonItem_ItemClick: Save failed, timeout, or cancelled, cannot print");
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
                    _logger.Warning(
                        "InPhieuBarButtonItem_ItemClick: Cannot print - Form not saved and no unsaved changes");
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
                _logger.Warning(
                    "InPhieuBarButtonItem_ItemClick: StockInOutMasterId is still Empty after save attempt");
                return;
            }

            // In phiếu xuất lưu chuyển kho với preview
            try
            {
                _logger.Debug("InPhieuBarButtonItem_ItemClick: Bắt đầu in phiếu, StockInOutMasterId={0}", stockInOutMasterId);

                // Tạo và load report - sử dụng InPhieuNhapXuatNoiBo cho xuất lưu chuyển kho
                var report = new InPhieuNhapXuatNoiBo(stockInOutMasterId);

                // Hiển thị preview bằng ReportPrintTool
                using (var printTool = new ReportPrintTool(report))
                {
                    printTool.ShowPreviewDialog();
                }

                _logger.Info("InPhieuBarButtonItem_ItemClick: In phiếu thành công, StockInOutMasterId={0}", stockInOutMasterId);
            }
            catch (Exception printEx)
            {
                _logger.Error($"InPhieuBarButtonItem_ItemClick: Lỗi in phiếu: {printEx.Message}", printEx);
                MsgBox.ShowError($"Lỗi in phiếu: {printEx.Message}");
            }

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
    private async void NhapQuanLyTaiSanBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
        try
        {

            // Lấy StockInOutMasterId từ _currentStockOutId (phải đã được lưu)
            Guid stockInOutMasterId;

            // Kiểm tra phiếu đã được lưu chưa
            if (_currentStockOutId != Guid.Empty)
            {
                stockInOutMasterId = _currentStockOutId;
            }
            else
            {
                // Phiếu chưa được lưu - kiểm tra có thay đổi chưa lưu không
                if (_hasUnsavedChanges)
                {
                    // Hỏi người dùng có muốn lưu trước không
                    if (MsgBox.ShowYesNo(
                            "Phiếu xuất kho chưa được lưu. Bạn có muốn lưu trước khi nhập bảo hành không?",
                            "Xác nhận",
                            this))
                    {
                        // Gọi nút Lưu để lưu phiếu
                        LuuPhieuBarButtonItem_ItemClick(null, null);

                        // Đợi cho đến khi lưu hoàn tất (tối đa 10 giây)
                        var timeout = TimeSpan.FromSeconds(10);
                        var startTime = DateTime.Now;
                        while (_currentStockOutId == Guid.Empty && (DateTime.Now - startTime) < timeout)
                        {
                            await Task.Delay(100);
                        }

                        // Kiểm tra lại sau khi lưu
                        if (_currentStockOutId != Guid.Empty)
                        {
                            stockInOutMasterId = _currentStockOutId;
                        }
                        else
                        {
                            // Lưu thất bại hoặc timeout, không mở form nhập bảo hành
                            _logger.Warning(
                                "NhapQuanLyTaiSanBarButtonItem_ItemClick: Save failed, timeout, or cancelled, cannot open warranty form");
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
                    MsgBox.ShowError(
                        "Vui lòng nhập và lưu phiếu xuất kho trước khi nhập bảo hành.",
                        "Lỗi",
                        this);
                    _logger.Warning(
                        "NhapQuanLyTaiSanBarButtonItem_ItemClick: Cannot add warranty - Form not saved and no unsaved changes");
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
                _logger.Warning(
                    "NhapQuanLyTaiSanBarButtonItem_ItemClick: StockInOutMasterId is still Empty after save attempt");
                return;
            }

            // Mở form nhập bảo hành với StockInOutMasterId (sử dụng OverlayManager để hiển thị)
            using (OverlayManager.ShowScope(this))
            {
                using (var frmDeviceIdentifierInput = new FrmNhapSerialMacEmei(stockInOutMasterId))
                {
                    frmDeviceIdentifierInput.StartPosition = FormStartPosition.CenterParent;
                    frmDeviceIdentifierInput.ShowDialog(this);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.Error("NhapQuanLyTaiSanBarButtonItem_ItemClick: Exception occurred", ex);
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
            // Lấy StockInOutMasterId từ _currentStockOutId (phải đã được lưu)
            Guid stockInOutMasterId = Guid.Empty;

            // Kiểm tra phiếu đã được lưu chưa
            if (_currentStockOutId != Guid.Empty)
            {
                stockInOutMasterId = _currentStockOutId;
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
                    _logger.Warning(
                        "ThemHinhAnhBarButtonItem_ItemClick: Cannot add images - Form not saved and no unsaved changes");
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
                _logger.Warning(
                    "ThemHinhAnhBarButtonItem_ItemClick: StockInOutMasterId is still Empty after save attempt");
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
    private void FrmXuatLuuChuyenKho_KeyDown(object sender, KeyEventArgs e)
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
                    NhapQuanLyTaiSanBarButtonItem_ItemClick(null, null);
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
            _logger.Error("FrmXuatLuuChuyenKho_KeyDown: Exception occurred", ex);
            MsgBox.ShowError($"Lỗi xử lý phím tắt: {ex.Message}");
        }
    }

    /// <summary>
    /// Event handler khi dữ liệu detail thay đổi
    /// </summary>
    private void UcXuatLuuChuyenKhoDetail1_DetailDataChanged(object sender, EventArgs e)
    {
        try
        {
            // Đánh dấu có thay đổi chưa lưu
            MarkAsChanged();

            // Tính tổng số lượng từ detail (xuất lưu chuyển kho dùng StockOutQty)
            var details = ucXuatLuuChuyenKhoDetail1.GetDetails();
            var totalQuantity = Enumerable.Sum<StockInOutDetail>(details, d => d.StockOutQty);

            // Cập nhật tổng lên master (chỉ có totalQuantity)
            ucXuatChuyenKhoMaster1.UpdateTotals(totalQuantity, 0, 0, 0);
        }
        catch (Exception ex)
        {
            _logger.Error("UcXuatLuuChuyenKhoDetail1_DetailDataChanged: Exception occurred", ex);
            MsgBox.ShowError($"Lỗi cập nhật tổng hợp: {ex.Message}");
        }
    }

    /// <summary>
    /// Event handler khi form đang đóng
    /// Sử dụng ShowYesNoCancel để đơn giản hóa logic: Yes = Lưu và đóng, No = Đóng không lưu, Cancel = Hủy
    /// </summary>
    private async void FrmXuatLuuChuyenKho_FormClosing(object sender, FormClosingEventArgs e)
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
                                _logger.Warning(
                                    "FrmXuatLuuChuyenKho_FormClosing: Save failed, form will remain open");
                                e.Cancel = true;
                            }
                        }
                        catch (Exception saveEx)
                        {
                            _logger.Error("FrmXuatLuuChuyenKho_FormClosing: Exception during save operation",
                                saveEx);
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
            _logger.Error("FrmXuatLuuChuyenKho_FormClosing: Exception occurred", ex);
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
            // Clear master control
            ucXuatChuyenKhoMaster1.ClearData();

            // Clear detail control
            ucXuatLuuChuyenKhoDetail1.ClearData();

            // Reset tổng về 0
            ucXuatChuyenKhoMaster1.UpdateTotals(0, 0, 0, 0);

            // Reset state
            _currentStockOutId = Guid.Empty;
            _isClosingAfterSave = false; // Reset flag khi reset form
            MarkAsSaved();
        }
        catch (Exception ex)
        {
            _logger.Error("ResetForm: Exception occurred", ex);
            MsgBox.ShowError($"Lỗi reset form: {ex.Message}");
        }
    }

    #endregion

    #region Helper Methods - DTO to Entity Conversion

    /// <summary>
    /// Map XuatLuuChuyenKhoMasterDto sang StockInOutMaster entity
    /// </summary>
    private StockInOutMaster MapMasterDtoToEntity(XuatLuuChuyenKhoMasterDto dto)
    {
        return new StockInOutMaster
        {
            Id = dto.Id,
            StockInOutDate = dto.StockOutDate,
            VocherNumber = dto.StockOutNumber,
            StockInOutType = (int)dto.LoaiNhapXuatKho,
            VoucherStatus = (int)dto.TrangThai,
            WarehouseId = dto.WarehouseStockOutId, // Kho xuất (kho nguồn)
            PurchaseOrderId = null, // Xuất lưu chuyển kho không có PurchaseOrder
            PartnerSiteId = dto.WarehouseStockInId != Guid.Empty ? dto.WarehouseStockInId : (Guid?)null, // Kho nhập (kho đích) - có thể map sang PartnerSiteId nếu cần
            Notes = dto.Notes ?? string.Empty,
            TotalQuantity = dto.TotalQuantity,
            TotalAmount = 0, // Xuất lưu chuyển kho không có giá trị tiền
            TotalVat = 0, // Xuất lưu chuyển kho không có VAT
            TotalAmountIncludedVat = 0, // Xuất lưu chuyển kho không có tổng tiền
            NguoiNhanHang = dto.NguoiNhanHang ?? string.Empty,
            NguoiGiaoHang = dto.NguoiGiaoHang ?? string.Empty
        };
    }

    #endregion

    #region Save Operations

    /// <summary>
    /// Lưu dữ liệu async
    /// Đảm bảo validate data từ các user control hợp lệ trước khi gọi BLL
    /// </summary>
    private async Task<bool> SaveDataAsync()
    {
        try
        {

            // ========== BƯỚC 1: VALIDATE VÀ LẤY DỮ LIỆU TỪ MASTER CONTROL ==========
            var masterDto = ucXuatChuyenKhoMaster1.GetDto();
            if (masterDto == null)
            {
                _logger.Warning("SaveDataAsync: Master validation failed - GetDto() returned null");
                MsgBox.ShowWarning("Vui lòng kiểm tra và điền đầy đủ thông tin phiếu xuất kho", "Cảnh báo", this);
                return false;
            }

            // Validate thêm business rules cho Master
            if (masterDto.WarehouseStockOutId == Guid.Empty)
            {
                _logger.Warning("SaveDataAsync: Master validation failed - WarehouseStockOutId is Empty");
                MsgBox.ShowWarning("Vui lòng chọn kho xuất", "Cảnh báo", this);
                return false;
            }

            if (masterDto.WarehouseStockInId == Guid.Empty)
            {
                _logger.Warning("SaveDataAsync: Master validation failed - WarehouseStockInId is Empty");
                MsgBox.ShowWarning("Vui lòng chọn kho nhập", "Cảnh báo", this);
                return false;
            }

            // ========== BƯỚC 2: VALIDATE VÀ LẤY DỮ LIỆU TỪ DETAIL CONTROL ==========
            // Validate tất cả các rows trong grid
            if (!ucXuatLuuChuyenKhoDetail1.ValidateAll())
            {
                _logger.Warning("SaveDataAsync: Detail validation failed - ValidateAll() returned false");
                // ValidateAll() đã hiển thị thông báo lỗi chi tiết
                return false;
            }

            // Lấy danh sách detail entities (GetDetails() trả về List<StockInOutDetail>)
            var detailEntities = ucXuatLuuChuyenKhoDetail1.GetDetails();
            if (detailEntities == null || detailEntities.Count == 0)
            {
                _logger.Warning("SaveDataAsync: No details found");
                MsgBox.ShowWarning("Vui lòng thêm ít nhất một dòng chi tiết", "Cảnh báo", this);
                return false;
            }

            // Validate thêm business rules cho từng detail entity
            var validationErrors = new List<string>();
            for (var i = 0; i < detailEntities.Count; i++)
            {
                var detail = detailEntities[i];
                var lineNumber = i + 1; // Entity không có LineNumber, tính từ index

                if (detail.ProductVariantId == Guid.Empty)
                {
                    validationErrors.Add($"Dòng {lineNumber}: Vui lòng chọn hàng hóa");
                }

                if (detail.StockOutQty <= 0)
                {
                    validationErrors.Add($"Dòng {lineNumber}: Số lượng xuất phải lớn hơn 0");
                }
            }

            if (validationErrors.Any())
            {
                _logger.Warning("SaveDataAsync: Detail business rules validation failed, Errors={0}",
                    string.Join("; ", validationErrors));
                MsgBox.ShowError($"Có lỗi trong dữ liệu chi tiết:\n\n{string.Join("\n", validationErrors)}",
                    "Lỗi validation", this);
                return false;
            }

            // ========== BƯỚC 3: CHUYỂN ĐỔI MASTER DTO SANG ENTITY ==========
            // Convert Master DTO sang entity để truyền vào BLL
            // Detail entities đã được trả về trực tiếp từ GetDetails(), không cần convert
            var masterEntity = MapMasterDtoToEntity(masterDto);

            // ========== BƯỚC 4: TẤT CẢ VALIDATION ĐÃ PASS - GỌI BLL ĐỂ LƯU ==========
            // Tất cả validation đã được thực hiện ở bước 1 và 2
            // StockInBll.SaveAsync sẽ có thêm validation layer nhưng chủ yếu là double-check
            var savedMasterId = await _stockInBll.SaveAsync(masterEntity, detailEntities);

            // ========== BƯỚC 5: CẬP NHẬT STATE SAU KHI LƯU THÀNH CÔNG ==========
            // Cập nhật ID sau khi lưu
            masterDto.Id = savedMasterId;
            _currentStockOutId = savedMasterId;

            // Set master ID cho detail control để đồng bộ
            ucXuatLuuChuyenKhoDetail1.SetStockOutMasterId(savedMasterId);
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