using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bll.Inventory.InventoryManagement;
using Common.Common;
using Common.Helpers;
using Common.Utils;
using Dal.DataAccess.Implementations.Inventory.StockIn;
using DevExpress.XtraGrid.Views.Grid;
using DTO.Inventory.InventoryManagement;
using Inventory.OverlayForm;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;

namespace Inventory.Query;

/// <summary>
/// Form xem lịch sử nhập xuất kho
/// Cung cấp chức năng xem, tìm kiếm, in phiếu và quản lý hình ảnh/bảo hành
/// </summary>
public partial class FrmStockInOutMasterHistory : DevExpress.XtraEditors.XtraForm
{
    #region ========== FIELDS & PROPERTIES ==========

    /// <summary>
    /// Business Logic Layer cho phiếu nhập xuất kho
    /// </summary>
    private readonly StockInOutMasterBll _stockInOutMasterBll = new();

    /// <summary>
    /// Logger để ghi log các sự kiện
    /// </summary>
    private readonly ILogger _logger = LoggerFactory.CreateLogger(LogCategory.UI);

    /// <summary>
    /// ID phiếu nhập xuất kho được chọn
    /// </summary>
    private Guid? _selectedStockInOutMasterId;

    #endregion

    #region ========== CONSTRUCTOR ==========

    public FrmStockInOutMasterHistory()
    {
        InitializeComponent();
        InitializeForm();
    }

    #endregion

    #region ========== INITIALIZATION ==========

    /// <summary>
    /// Khởi tạo form
    /// </summary>
    private void InitializeForm()
    {
        try
        {
            // Setup events
            SetupEvents();

            // Khởi tạo giá trị mặc định cho date pickers
            InitializeDateFilters();
        }
        catch (Exception ex)
        {
            _logger.Error("InitializeForm: Exception occurred", ex);
            MsgBox.ShowError($"Lỗi khởi tạo form: {ex.Message}");
        }
    }

    /// <summary>
    /// Khởi tạo giá trị mặc định cho date filters
    /// </summary>
    private void InitializeDateFilters()
    {
        try
        {
            // Từ ngày: đầu tháng hiện tại
            var fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            TuNgayBarEditItem.EditValue = fromDate;

            // Đến ngày: ngày hiện tại
            DenNgayBarEditItem.EditValue = DateTime.Now;
        }
        catch (Exception ex)
        {
            _logger.Error("InitializeDateFilters: Exception occurred", ex);
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
            XemBaoCaoBarButtonItem.ItemClick += XemBaoCaoBarButtonItem_ItemClick;
            ChiTietPhieuNhapXuatBarButtonItem.ItemClick += ChiTietPhieuNhapXuatBarButtonItem_ItemClick;
            InPhieuBarButtonItem.ItemClick += InPhieuBarButtonItem_ItemClick;
            NhapBaoHanhBarButtonItem.ItemClick += NhapBaoHanhBarButtonItem_ItemClick;
            ThemHinhAnhBarButtonItem.ItemClick += ThemHinhAnhBarButtonItem_ItemClick;
            XoaPhieuBarButtonItem.ItemClick += XoaPhieuBarButtonItem_ItemClick;

            // GridView events
            StockInOutMasterHistoryDtoGridView.DoubleClick += StockInOutMasterHistoryDtoGridView_DoubleClick;
            StockInOutMasterHistoryDtoGridView.FocusedRowChanged += StockInOutMasterHistoryDtoGridView_FocusedRowChanged;
            StockInOutMasterHistoryDtoGridView.SelectionChanged += StockInOutMasterHistoryDtoGridView_SelectionChanged;
            StockInOutMasterHistoryDtoGridView.CustomDrawRowIndicator += StockInOutMasterHistoryDtoGridView_CustomDrawRowIndicator;
            StockInOutMasterHistoryDtoGridView.RowCellStyle += StockInOutMasterHistoryDtoGridView_RowCellStyle;

            // Form events
            Load += FrmStockInOutHistory_Load;
        }
        catch (Exception ex)
        {
            _logger.Error("SetupEvents: Exception occurred", ex);
        }
    }

    #endregion

    #region ========== EVENT HANDLERS ==========

    /// <summary>
    /// Event handler khi form được load
    /// </summary>
    private void FrmStockInOutHistory_Load(object sender, EventArgs e)
    {
        try
        {
            
        }
        catch (Exception ex)
        {
            _logger.Error("FrmStockInOutHistory_Load: Exception occurred", ex);
            MsgBox.ShowError($"Lỗi khởi tạo form: {ex.Message}");
        }
    }

    /// <summary>
    /// Event handler cho nút Xem báo cáo
    /// </summary>
    private async void XemBaoCaoBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
        try
        {
            await LoadDataAsync();
        }
        catch (Exception ex)
        {
            _logger.Error("XemBaoCaoBarButtonItem_ItemClick: Exception occurred", ex);
            MsgBox.ShowError($"Lỗi tải dữ liệu: {ex.Message}");
        }
    }

    /// <summary>
    /// Event handler cho nút Chi tiết phiếu nhập xuất
    /// Mở form FrmNhapKhoThuongMai02 và load dữ liệu từ ID đã chọn
    /// </summary>
    private void ChiTietPhieuNhapXuatBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
        try
        {
            // Kiểm tra số lượng phiếu được chọn - chỉ cho phép 1 phiếu
            var selectedCount = StockInOutMasterHistoryDtoGridView.SelectedRowsCount;
            if (selectedCount == 0)
            {
                MsgBox.ShowWarning("Vui lòng chọn một phiếu nhập xuất kho để xem chi tiết.");
                return;
            }

            if (selectedCount > 1)
            {
                MsgBox.ShowWarning("Chỉ cho phép xem chi tiết 1 phiếu. Vui lòng bỏ chọn bớt.");
                return;
            }

            // Kiểm tra ID phiếu được chọn
            if (!_selectedStockInOutMasterId.HasValue || _selectedStockInOutMasterId.Value == Guid.Empty)
            {
                MsgBox.ShowWarning("Vui lòng chọn phiếu nhập xuất kho để xem chi tiết.");
                return;
            }

            // Mở form chi tiết với OverlayManager và load dữ liệu
            using (OverlayManager.ShowScope(this))
            {
                using var form = new Inventory.StockIn.NhapHangThuongMai.FrmNhapKhoThuongMai(_selectedStockInOutMasterId.Value);
                    
                // Đảm bảo form chưa được hiển thị
                form.Visible = false;
                form.StartPosition = FormStartPosition.CenterParent;
                        
                // Load dữ liệu từ ID trước khi hiển thị form
                //await form.LoadDataAsync(_selectedStockInOutMasterId.Value);
                        
                // Đảm bảo form vẫn chưa visible trước khi show dialog
                if (form.Visible)
                {
                    form.Visible = false;
                }
                        
                // Hiển thị form
                form.ShowDialog(this);
            }

        }
        catch (Exception ex)
        {
            _logger.Error("ChiTietPhieuNhapXuatBarButtonItem_ItemClick: Exception occurred", ex);
            MsgBox.ShowError($"Lỗi mở form chi tiết: {ex.Message}");
        }
    }

    /// <summary>
    /// Event handler cho nút In phiếu
    /// Chỉ mở màn hình cho 1 phiếu được chọn, sử dụng OverlayManager
    /// </summary>
    private void InPhieuBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
        try
        {
            // Kiểm tra số lượng phiếu được chọn - chỉ cho phép 1 phiếu
            var selectedCount = StockInOutMasterHistoryDtoGridView.SelectedRowsCount;
            if (selectedCount == 0)
            {
                MsgBox.ShowWarning("Vui lòng chọn một phiếu nhập xuất kho để in.");
                return;
            }

            if (selectedCount > 1)
            {
                MsgBox.ShowWarning("Chỉ cho phép in 1 phiếu. Vui lòng bỏ chọn bớt.");
                return;
            }

            // Kiểm tra ID phiếu được chọn
            if (!_selectedStockInOutMasterId.HasValue || _selectedStockInOutMasterId.Value == Guid.Empty)
            {
                MsgBox.ShowWarning("Vui lòng chọn phiếu nhập xuất kho để in.");
                return;
            }

            // In phiếu nhập kho với preview, sử dụng OverlayManager
            using (OverlayManager.ShowScope(this))
            {
                Inventory.StockIn.InPhieu.StockInReportHelper.PrintStockInVoucher(_selectedStockInOutMasterId.Value);
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
    /// Chỉ mở màn hình cho 1 phiếu được chọn, sử dụng OverlayManager
    /// </summary>
    private void NhapBaoHanhBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
        try
        {
            // Kiểm tra số lượng phiếu được chọn - chỉ cho phép 1 phiếu
            var selectedCount = StockInOutMasterHistoryDtoGridView.SelectedRowsCount;
            if (selectedCount == 0)
            {
                MsgBox.ShowWarning("Vui lòng chọn một phiếu nhập xuất kho để nhập bảo hành.");
                return;
            }

            if (selectedCount > 1)
            {
                MsgBox.ShowWarning("Chỉ cho phép nhập bảo hành cho 1 phiếu. Vui lòng bỏ chọn bớt.");
                return;
            }

            // Kiểm tra ID phiếu được chọn
            if (!_selectedStockInOutMasterId.HasValue || _selectedStockInOutMasterId.Value == Guid.Empty)
            {
                MsgBox.ShowWarning("Vui lòng chọn phiếu nhập xuất kho để nhập bảo hành.");
                return;
            }

            // Mở form nhập bảo hành với OverlayManager
            using (OverlayManager.ShowScope(this))
            {
                using var form = new FrmWarranty(_selectedStockInOutMasterId.Value);
                    
                form.StartPosition = FormStartPosition.CenterParent;
                form.ShowDialog(this);
            }
        }
        catch (Exception ex)
        {
            _logger.Error("NhapBaoHanhBarButtonItem_ItemClick: Exception occurred", ex);
            MsgBox.ShowError($"Lỗi mở form bảo hành: {ex.Message}");
        }
    }

    /// <summary>
    /// Event handler cho nút Thêm hình ảnh
    /// Chỉ mở màn hình cho 1 phiếu được chọn, sử dụng OverlayManager
    /// </summary>
    private void ThemHinhAnhBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
        try
        {

            // Kiểm tra số lượng phiếu được chọn - chỉ cho phép 1 phiếu
            var selectedCount = StockInOutMasterHistoryDtoGridView.SelectedRowsCount;
            switch (selectedCount)
            {
                case 0:
                    MsgBox.ShowWarning("Vui lòng chọn một phiếu nhập xuất kho để thêm hình ảnh.");
                    return;
                case > 1:
                    MsgBox.ShowWarning("Chỉ cho phép thêm hình ảnh cho 1 phiếu. Vui lòng bỏ chọn bớt.");
                    return;
            }

            // Kiểm tra ID phiếu được chọn
            if (!_selectedStockInOutMasterId.HasValue || _selectedStockInOutMasterId.Value == Guid.Empty)
            {
                MsgBox.ShowWarning("Vui lòng chọn phiếu nhập xuất kho để thêm hình ảnh.");
                return;
            }

            // Mở form thêm hình ảnh với OverlayManager
            using (OverlayManager.ShowScope(this))
            {
                using var form = new FrmStockInOutAddImages(_selectedStockInOutMasterId.Value);
                    
                form.StartPosition = FormStartPosition.CenterParent;
                form.ShowDialog(this);
            }

        }
        catch (Exception ex)
        {
            _logger.Error("ThemHinhAnhBarButtonItem_ItemClick: Exception occurred", ex);
            MsgBox.ShowError($"Lỗi mở form thêm hình ảnh: {ex.Message}");
        }
    }

    /// <summary>
    /// Event handler cho nút Xóa phiếu
    /// Chỉ cho phép xóa 1 phiếu được chọn, sử dụng OverlayManager và confirmation dialog
    /// </summary>
    private async void XoaPhieuBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
        try
        {
            // Kiểm tra số lượng phiếu được chọn - chỉ cho phép 1 phiếu
            var selectedCount = StockInOutMasterHistoryDtoGridView.SelectedRowsCount;
            if (selectedCount == 0)
            {
                MsgBox.ShowWarning("Vui lòng chọn một phiếu nhập xuất kho để xóa.");
                return;
            }

            if (selectedCount > 1)
            {
                MsgBox.ShowWarning("Chỉ cho phép xóa 1 phiếu. Vui lòng bỏ chọn bớt.");
                return;
            }

            // Kiểm tra ID phiếu được chọn
            if (!_selectedStockInOutMasterId.HasValue || _selectedStockInOutMasterId.Value == Guid.Empty)
            {
                MsgBox.ShowWarning("Vui lòng chọn phiếu nhập xuất kho để xóa.");
                return;
            }

            // Lấy thông tin phiếu để hiển thị trong confirmation
            var focusedRowHandle = StockInOutMasterHistoryDtoGridView.FocusedRowHandle;
            var dto = StockInOutMasterHistoryDtoGridView.GetRow(focusedRowHandle) as StockInOutMasterHistoryDto;
            var voucherNumber = dto?.VocherNumber ?? "N/A";

            // Hiển thị confirmation dialog
            var confirmMessage = $"Bạn có chắc muốn xóa phiếu nhập xuất kho:\n<b>{voucherNumber}</b>?\n\n" +
                                 "Hành động này không thể hoàn tác!";
                
            if (!MsgBox.ShowYesNo(confirmMessage, "Xác nhận xóa"))
            {
                return;
            }

            // Thực hiện xóa với OverlayManager
            using (OverlayManager.ShowScope(this))
            {
                await Task.Run(() =>
                {
                    try
                    {
                        _stockInOutMasterBll.Delete(_selectedStockInOutMasterId.Value);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error("XoaPhieuBarButtonItem_ItemClick: Exception during delete operation", ex);
                        BeginInvoke(new Action(() =>
                        {
                            MsgBox.ShowError($"Lỗi xóa phiếu: {ex.Message}");
                        }));
                        throw;
                    }
                });
            }

            // Reload data sau khi xóa thành công
            await LoadDataAsync();

            MsgBox.ShowSuccess("Xóa phiếu nhập xuất kho thành công.");

        }
        catch (Exception ex)
        {
            _logger.Error("XoaPhieuBarButtonItem_ItemClick: Exception occurred", ex);
            MsgBox.ShowError($"Lỗi xóa phiếu: {ex.Message}");
        }
    }

    /// <summary>
    /// Event handler khi double click trên GridView
    /// </summary>
    private void StockInOutMasterHistoryDtoGridView_DoubleClick(object sender, EventArgs e)
    {
        try
        {
            if (_selectedStockInOutMasterId.HasValue && _selectedStockInOutMasterId.Value != Guid.Empty)
            {
                // Có thể mở form chi tiết hoặc in phiếu
                InPhieuBarButtonItem_ItemClick(sender, null);
            }
        }
        catch (Exception ex)
        {
            _logger.Error("StockInOutMasterHistoryDtoGridView_DoubleClick: Exception occurred", ex);
            MsgBox.ShowError($"Lỗi: {ex.Message}");
        }
    }

    /// <summary>
    /// Event handler khi row được chọn thay đổi
    /// </summary>
    private void StockInOutMasterHistoryDtoGridView_FocusedRowChanged(object sender, 
        DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
    {
        try
        {
            UpdateSelectedItem();
        }
        catch (Exception ex)
        {
            _logger.Error("StockInOutMasterHistoryDtoGridView_FocusedRowChanged: Exception occurred", ex);
        }
    }

    /// <summary>
    /// Event handler khi selection thay đổi trên GridView
    /// </summary>
    private void StockInOutMasterHistoryDtoGridView_SelectionChanged(object sender, EventArgs e)
    {
        try
        {
            UpdateSelectedItem();
            UpdateDataSummary();
        }
        catch (Exception ex)
        {
            _logger.Error("StockInOutMasterHistoryDtoGridView_SelectionChanged: Exception occurred", ex);
        }
    }

    /// <summary>
    /// Xử lý sự kiện vẽ số thứ tự dòng cho GridView
    /// Sử dụng helper từ Common để hiển thị số thứ tự (1, 2, 3...)
    /// </summary>
    private void StockInOutMasterHistoryDtoGridView_CustomDrawRowIndicator(object sender, 
        DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
    {
        try
        {
            GridViewHelper.CustomDrawRowIndicator(StockInOutMasterHistoryDtoGridView, e);
        }
        catch (Exception ex)
        {
            _logger.Error("StockInOutMasterHistoryDtoGridView_CustomDrawRowIndicator: Exception occurred", ex);
        }
    }

    /// <summary>
    /// Xử lý sự kiện tô màu cell theo trạng thái hoặc điều kiện
    /// Có thể mở rộng để format theo các điều kiện khác nhau
    /// </summary>
    private void StockInOutMasterHistoryDtoGridView_RowCellStyle(object sender, 
        DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
    {
        try
        {
            if (sender is not GridView gridView) return;

            _ = gridView.GetRow(e.RowHandle) as StockInOutMasterHistoryDto;

            // Có thể thêm logic format theo điều kiện ở đây
            // Ví dụ: format theo loại nhập xuất, tổng tiền, v.v.
            // Hiện tại để trống, có thể mở rộng sau
        }
        catch (Exception ex)
        {
            MsgBox.ShowException(ex);
            // Ignore style errors để không ảnh hưởng đến hiển thị
        }
    }

    #endregion

    #region ========== DATA LOADING ==========

    /// <summary>
    /// Load dữ liệu lịch sử nhập xuất kho
    /// </summary>
    private async Task LoadDataAsync()
    {
        try
        {
            // Hiển thị SplashScreen
            SplashScreenHelper.ShowWaitingSplashScreen();

            try
            {
                await LoadDataWithoutSplashAsync();
            }
            finally
            {
                // Đóng SplashScreen
                SplashScreenHelper.CloseSplashScreen();
            }
        }
        catch (Exception ex)
        {
            _logger.Error("LoadDataAsync: Exception occurred", ex);
            SplashScreenHelper.CloseSplashScreen();
            MsgBox.ShowError($"Lỗi tải dữ liệu: {ex.Message}");
        }
    }

    /// <summary>
    /// Load dữ liệu không hiển thị SplashScreen (dùng cho refresh)
    /// </summary>
    private async Task LoadDataWithoutSplashAsync()
    {
        await Task.Run(() =>
        {
            try
            {
                // Lấy giá trị từ date pickers
                var fromDate = TuNgayBarEditItem.EditValue as DateTime? ?? 
                               new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                var toDate = DenNgayBarEditItem.EditValue as DateTime? ?? DateTime.Now;

                // Validate date range
                if (fromDate > toDate)
                {
                    BeginInvoke(new Action(() =>
                    {
                        MsgBox.ShowWarning("Từ ngày không được lớn hơn đến ngày.");
                    }));
                    return;
                }

                // Tạo query criteria
                var query = new StockInHistoryQueryCriteria
                {
                    FromDate = fromDate.Date,
                    ToDate = toDate.Date.AddDays(1).AddTicks(-1) // Đến cuối ngày
                };

                // Lấy dữ liệu từ BLL
                var entities = _stockInOutMasterBll.QueryHistory(query);

                // Convert sang DTO
                var dtos = entities.Select(e => e.ToDto()).ToList();

                // Update UI thread
                BeginInvoke(new Action(() =>
                {
                    stockInOutMasterHistoryDtoBindingSource.DataSource = dtos;
                    stockInOutMasterHistoryDtoBindingSource.ResetBindings(false);

                    // Reset selection
                    _selectedStockInOutMasterId = null;
                    UpdateButtonStates();
                    UpdateDataSummary();
                }));
            }
            catch (Exception ex)
            {
                _logger.Error("LoadDataWithoutSplashAsync: Exception occurred", ex);
                BeginInvoke(new Action(() =>
                {
                    MsgBox.ShowError($"Lỗi tải dữ liệu: {ex.Message}");
                }));
            }
        });
    }

    /// <summary>
    /// Cập nhật item được chọn từ GridView
    /// </summary>
    private void UpdateSelectedItem()
    {
        try
        {
            var focusedRowHandle = StockInOutMasterHistoryDtoGridView.FocusedRowHandle;
                
            if (focusedRowHandle >= 0)
            {
                if (StockInOutMasterHistoryDtoGridView.GetRow(focusedRowHandle) is StockInOutMasterHistoryDto dto)
                {
                    _selectedStockInOutMasterId = dto.Id;
                }
                else
                {
                    _selectedStockInOutMasterId = null;
                }
            }
            else
            {
                _selectedStockInOutMasterId = null;
            }

            UpdateButtonStates();
        }
        catch (Exception ex)
        {
            _logger.Error("UpdateSelectedItem: Exception occurred", ex);
        }
    }

    /// <summary>
    /// Cập nhật trạng thái các nút
    /// </summary>
    private void UpdateButtonStates()
    {
        try
        {
            var hasSelection = _selectedStockInOutMasterId.HasValue && 
                               _selectedStockInOutMasterId.Value != Guid.Empty;

            // Lấy số lượng dòng được chọn
            var selectedCount = StockInOutMasterHistoryDtoGridView.SelectedRowsCount;

            // Tất cả các nút: chỉ khi chọn đúng 1 dòng
            ChiTietPhieuNhapXuatBarButtonItem.Enabled = hasSelection && selectedCount == 1;
            InPhieuBarButtonItem.Enabled = hasSelection && selectedCount == 1;
            NhapBaoHanhBarButtonItem.Enabled = hasSelection && selectedCount == 1;
            ThemHinhAnhBarButtonItem.Enabled = hasSelection && selectedCount == 1;
            XoaPhieuBarButtonItem.Enabled = hasSelection && selectedCount == 1;
        }
        catch (Exception ex)
        {
            _logger.Error("UpdateButtonStates: Exception occurred", ex);
        }
    }

    /// <summary>
    /// Cập nhật thông tin tổng kết dữ liệu với HTML formatting chuyên nghiệp
    /// Sử dụng các tag HTML chuẩn của DevExpress: &lt;b&gt;, &lt;i&gt;, &lt;color&gt;, &lt;size&gt;
    /// </summary>
    private void UpdateDataSummary()
    {
        try
        {
            var totalRows = StockInOutMasterHistoryDtoGridView.RowCount;
            var selectedRows = StockInOutMasterHistoryDtoGridView.SelectedRowsCount;

            // Cập nhật tổng số phiếu nhập xuất với HTML formatting
            if (DataSummaryBarStaticItem != null)
            {
                if (totalRows == 0)
                {
                    // Không có dữ liệu - hiển thị màu xám, italic
                    DataSummaryBarStaticItem.Caption = @"<color=#757575><i>Chưa có dữ liệu</i></color>";
                }
                else
                {
                    // Có dữ liệu - format chuyên nghiệp:
                    // Label "Tổng:" màu xám, size nhỏ
                    // Số lượng màu xanh đậm, bold
                    // Text "phiếu nhập xuất" màu xám
                    DataSummaryBarStaticItem.Caption = 
                        $@"<size=9><color=#757575>Tổng:</color></size> " +
                        $@"<b><color=blue>{totalRows:N0}</color></b> " +
                        $@"<size=9><color=#757575>phiếu nhập xuất</color></size>";
                }
            }

            // Cập nhật số dòng đã chọn với HTML formatting
            if (SelectedRowBarStaticItem != null)
            {
                if (selectedRows > 0)
                {
                    // Có chọn dòng - format chuyên nghiệp:
                    // Label "Đã chọn:" màu xám, size nhỏ
                    // Số lượng màu xanh đậm, bold
                    // Text "dòng" màu xám
                    SelectedRowBarStaticItem.Caption = 
                        $@"<size=9><color=#757575>Đã chọn:</color></size> " +
                        $@"<b><color=blue>{selectedRows:N0}</color></b> " +
                        $@"<size=9><color=#757575>dòng</color></size>";
                }
                else
                {
                    // Không chọn dòng - hiển thị màu xám, italic
                    SelectedRowBarStaticItem.Caption = @"<color=#757575><i>Chưa chọn dòng nào</i></color>";
                }
            }
        }
        catch (Exception ex)
        {
            _logger.Error("UpdateDataSummary: Exception occurred", ex);
        }
    }

    #endregion
}