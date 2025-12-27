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
using DevExpress.XtraReports.UI;
using DTO.Inventory.InventoryManagement;
using DTO.Inventory.StockIn;
using Inventory.OverlayForm;
using Inventory.StockIn.InPhieu;
using Inventory.StockIn.NhapBaoHanh;
using Inventory.StockIn.NhapHangThuongMai;
using Inventory.StockIn.NhapLuuChuyenKho;
using Inventory.StockIn.NhapNoiBo;
using Inventory.StockIn.NhapThietBiMuon;
using Inventory.Report.PhieuXuat;
using Inventory.StockOut.XuatBaoHanh;
using Inventory.StockOut.XuatChoThueMuon;
using Inventory.StockOut.XuatHangThuongMai;
using Inventory.StockOut.XuatLuuChuyenKho;
using Inventory.StockOut.XuatNoiBo;
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
            InPhieuGiaoHangBarButtonItem.ItemClick += InPhieuGiaoHangBarButtonItem_ItemClick;
            ThemHinhAnhBarButtonItem.ItemClick += ThemHinhAnhBarButtonItem_ItemClick;
            AttachFileBarButtonItem.ItemClick += AttachFileBarButtonItem_ItemClick;
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

            // Lấy DTO từ row được chọn để xác định loại nhập xuất
            var focusedRowHandle = StockInOutMasterHistoryDtoGridView.FocusedRowHandle;
            if (focusedRowHandle < 0)
            {
                MsgBox.ShowWarning("Vui lòng chọn phiếu nhập xuất kho để xem chi tiết.");
                return;
            }

            if (StockInOutMasterHistoryDtoGridView.GetRow(focusedRowHandle) is not StockInOutMasterHistoryDto selectedDto)
            {
                MsgBox.ShowWarning("Không thể lấy thông tin phiếu được chọn.");
                return;
            }

            // Tạo form dựa vào loại nhập xuất
            Form detailForm = selectedDto.LoaiNhapXuatKho switch
            {
                // Các trường hợp Nhập
                LoaiNhapXuatKhoEnum.NhapHangThuongMai => new FrmNhapKhoThuongMai(_selectedStockInOutMasterId.Value),
                LoaiNhapXuatKhoEnum.NhapThietBiMuonThue => new FrmNhapThietBiMuon(_selectedStockInOutMasterId.Value),
                LoaiNhapXuatKhoEnum.NhapNoiBo => new FrmNhapNoiBo(_selectedStockInOutMasterId.Value),
                LoaiNhapXuatKhoEnum.NhapLuuChuyenKho => new FrmNhapLuuChuyenKho(_selectedStockInOutMasterId.Value),
                LoaiNhapXuatKhoEnum.NhapHangBaoHanh => new FrmNhapBaoHanh(_selectedStockInOutMasterId.Value),
                
                // Các trường hợp Xuất
                LoaiNhapXuatKhoEnum.XuatHangThuongMai => new FrmXuatKhoThuongMai(_selectedStockInOutMasterId.Value),
                LoaiNhapXuatKhoEnum.XuatThietBiMuonThue => new FrmXuatThietBiChoThueMuon(_selectedStockInOutMasterId.Value),
                LoaiNhapXuatKhoEnum.XuatNoiBo => new FrmXuatNoiBo(_selectedStockInOutMasterId.Value),
                LoaiNhapXuatKhoEnum.XuatLuuChuyenKho => new FrmXuatLuuChuyenKho(_selectedStockInOutMasterId.Value),
                LoaiNhapXuatKhoEnum.XuatHangBaoHanh => new FrmXuatBaoHanh(_selectedStockInOutMasterId.Value),
                
                // Trường hợp mặc định
                _ => new FrmNhapKhoThuongMai(_selectedStockInOutMasterId.Value) // Default: dùng FrmNhapKhoThuongMai
            };

            // Mở form chi tiết với OverlayManager
            using (OverlayManager.ShowScope(this))
            using (detailForm)
            {
                detailForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                detailForm.StartPosition = FormStartPosition.CenterParent;
                detailForm.ShowDialog(this);
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

            // Lấy DTO từ row được chọn để xác định loại nhập xuất
            var focusedRowHandle = StockInOutMasterHistoryDtoGridView.FocusedRowHandle;
            if (focusedRowHandle < 0)
            {
                MsgBox.ShowWarning("Vui lòng chọn phiếu nhập xuất kho để in.");
                return;
            }

            if (StockInOutMasterHistoryDtoGridView.GetRow(focusedRowHandle) is not StockInOutMasterHistoryDto selectedDto)
            {
                MsgBox.ShowWarning("Không thể lấy thông tin phiếu được chọn.");
                return;
            }

            // Tạo report dựa vào loại nhập xuất
            using (OverlayManager.ShowScope(this))
            {
                try
                {
                    XtraReport report = selectedDto.LoaiNhapXuatKho switch
                    {
                        // Các trường hợp Nhập
                        LoaiNhapXuatKhoEnum.NhapHangThuongMai => new InPhieuNhapKho(_selectedStockInOutMasterId.Value),
                        LoaiNhapXuatKhoEnum.NhapThietBiMuonThue => new InPhieuNhapXuatThietBiChoMuon(_selectedStockInOutMasterId.Value),
                        LoaiNhapXuatKhoEnum.NhapNoiBo => new InPhieuNhapXuatNoiBo(_selectedStockInOutMasterId.Value),
                        LoaiNhapXuatKhoEnum.NhapHangBaoHanh => new InPhieuBaoHanh(_selectedStockInOutMasterId.Value),
                        
                        // Các trường hợp Xuất - sử dụng report chung cho cả nhập và xuất
                        LoaiNhapXuatKhoEnum.XuatHangThuongMai => new InPhieuNhapKho(_selectedStockInOutMasterId.Value), // Report nhập cũng dùng được cho xuất
                        LoaiNhapXuatKhoEnum.XuatThietBiMuonThue => new InPhieuNhapXuatThietBiChoMuon(_selectedStockInOutMasterId.Value),
                        LoaiNhapXuatKhoEnum.XuatNoiBo => new InPhieuNhapXuatNoiBo(_selectedStockInOutMasterId.Value),
                        LoaiNhapXuatKhoEnum.XuatHangBaoHanh => new InPhieuBaoHanh(_selectedStockInOutMasterId.Value),
                        
                        // Trường hợp mặc định
                        _ => new InPhieuNhapKho(_selectedStockInOutMasterId.Value) // Default: dùng InPhieuNhapKho cho NhapLuuChuyenKho, XuatLuuChuyenKho và các loại khác
                    };

                    // Hiển thị preview bằng ReportPrintTool
                    using var printTool = new DevExpress.XtraReports.UI.ReportPrintTool(report);
                    printTool.ShowPreviewDialog();
                }
                catch (Exception printEx)
                {
                    _logger.Error($"InPhieuBarButtonItem_ItemClick: Lỗi in phiếu: {printEx.Message}", printEx);
                    MsgBox.ShowError($"Lỗi in phiếu: {printEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            _logger.Error("InPhieuBarButtonItem_ItemClick: Exception occurred", ex);
            MsgBox.ShowError($"Lỗi in phiếu: {ex.Message}");
        }
    }

    /// <summary>
    /// Event handler cho nút In phiếu giao hàng
    /// Chỉ mở màn hình cho 1 phiếu xuất được chọn, sử dụng OverlayManager
    /// </summary>
    private void InPhieuGiaoHangBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
        try
        {
            // Kiểm tra số lượng phiếu được chọn - chỉ cho phép 1 phiếu
            var selectedCount = StockInOutMasterHistoryDtoGridView.SelectedRowsCount;
            if (selectedCount == 0)
            {
                MsgBox.ShowWarning("Vui lòng chọn một phiếu xuất kho để in phiếu giao hàng.");
                return;
            }

            if (selectedCount > 1)
            {
                MsgBox.ShowWarning("Chỉ cho phép in phiếu giao hàng cho 1 phiếu. Vui lòng bỏ chọn bớt.");
                return;
            }

            // Kiểm tra ID phiếu được chọn
            if (!_selectedStockInOutMasterId.HasValue || _selectedStockInOutMasterId.Value == Guid.Empty)
            {
                MsgBox.ShowWarning("Vui lòng chọn phiếu xuất kho để in phiếu giao hàng.");
                return;
            }

            // Lấy DTO từ row được chọn để xác định loại nhập xuất
            var focusedRowHandle = StockInOutMasterHistoryDtoGridView.FocusedRowHandle;
            if (focusedRowHandle < 0)
            {
                MsgBox.ShowWarning("Vui lòng chọn phiếu xuất kho để in phiếu giao hàng.");
                return;
            }

            if (StockInOutMasterHistoryDtoGridView.GetRow(focusedRowHandle) is not StockInOutMasterHistoryDto selectedDto)
            {
                MsgBox.ShowWarning("Không thể lấy thông tin phiếu được chọn.");
                return;
            }

            // Kiểm tra phiếu được chọn có phải là phiếu xuất không
            var enumName = selectedDto.LoaiNhapXuatKho.ToString();
            if (!enumName.StartsWith("Xuat"))
            {
                MsgBox.ShowWarning("Chỉ có thể in phiếu giao hàng cho các phiếu xuất kho. Phiếu được chọn không phải là phiếu xuất.");
                return;
            }

            // Tạo report phiếu giao hàng
            using (OverlayManager.ShowScope(this))
            {
                try
                {
                    var report = new RpPhieuGiaoHang(_selectedStockInOutMasterId.Value);

                    // Hiển thị preview bằng ReportPrintTool
                    using var printTool = new DevExpress.XtraReports.UI.ReportPrintTool(report);
                    printTool.ShowPreviewDialog();
                }
                catch (Exception printEx)
                {
                    _logger.Error($"InPhieuGiaoHangBarButtonItem_ItemClick: Lỗi in phiếu giao hàng: {printEx.Message}", printEx);
                    MsgBox.ShowError($"Lỗi in phiếu giao hàng: {printEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            _logger.Error("InPhieuGiaoHangBarButtonItem_ItemClick: Exception occurred", ex);
            MsgBox.ShowError($"Lỗi in phiếu giao hàng: {ex.Message}");
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
    /// Event handler cho nút Thêm chứng từ
    /// Chỉ mở màn hình cho 1 phiếu được chọn, sử dụng OverlayManager
    /// </summary>
    private void AttachFileBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
        try
        {
            // Kiểm tra số lượng phiếu được chọn - chỉ cho phép 1 phiếu
            var selectedCount = StockInOutMasterHistoryDtoGridView.SelectedRowsCount;
            switch (selectedCount)
            {
                case 0:
                    MsgBox.ShowWarning("Vui lòng chọn một phiếu nhập xuất kho để thêm chứng từ.");
                    return;
                case > 1:
                    MsgBox.ShowWarning("Chỉ cho phép thêm chứng từ cho 1 phiếu. Vui lòng bỏ chọn bớt.");
                    return;
            }

            // Kiểm tra ID phiếu được chọn
            if (!_selectedStockInOutMasterId.HasValue || _selectedStockInOutMasterId.Value == Guid.Empty)
            {
                MsgBox.ShowWarning("Vui lòng chọn phiếu nhập xuất kho để thêm chứng từ.");
                return;
            }

            // Mở form thêm chứng từ với OverlayManager
            using (OverlayManager.ShowScope(this))
            {
                using var form = new FrmAddStockInOutDocumentDto(_selectedStockInOutMasterId.Value);
                    
                form.StartPosition = FormStartPosition.CenterParent;
                form.ShowDialog(this);
            }

        }
        catch (Exception ex)
        {
            _logger.Error("AttachFileBarButtonItem_ItemClick: Exception occurred", ex);
            MsgBox.ShowError($"Lỗi mở form thêm chứng từ: {ex.Message}");
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

            // Kiểm tra phiếu được chọn có phải là phiếu xuất không (cho nút In phiếu giao hàng)
            bool isStockOut = false;
            if (hasSelection && selectedCount == 1)
            {
                var focusedRowHandle = StockInOutMasterHistoryDtoGridView.FocusedRowHandle;
                if (focusedRowHandle >= 0)
                {
                    if (StockInOutMasterHistoryDtoGridView.GetRow(focusedRowHandle) is StockInOutMasterHistoryDto dto)
                    {
                        var enumName = dto.LoaiNhapXuatKho.ToString();
                        isStockOut = enumName.StartsWith("Xuat");
                    }
                }
            }

            // Tất cả các nút: chỉ khi chọn đúng 1 dòng
            ChiTietPhieuNhapXuatBarButtonItem.Enabled = hasSelection && selectedCount == 1;
            InPhieuBarButtonItem.Enabled = hasSelection && selectedCount == 1;
            InPhieuGiaoHangBarButtonItem.Enabled = hasSelection && selectedCount == 1 && isStockOut;
            ThemHinhAnhBarButtonItem.Enabled = hasSelection && selectedCount == 1;
            AttachFileBarButtonItem.Enabled = hasSelection && selectedCount == 1;
            XoaPhieuBarButtonItem.Enabled = hasSelection && selectedCount == 1;
        }
        catch (Exception ex)
        {
            _logger.Error("UpdateButtonStates: Exception occurred", ex);
        }
    }

    /// <summary>
    /// Cập nhật thông tin tổng kết dữ liệu với HTML formatting chuyên nghiệp
    /// Sử dụng các tag HTML chuẩn của DevExpress: &lt;b&gt;, &lt;i&gt;, &lt;color&gt;
    /// </summary>
    private void UpdateDataSummary()
    {
        try
        {
            var totalRows = StockInOutMasterHistoryDtoGridView.RowCount;
            var selectedRows = StockInOutMasterHistoryDtoGridView.SelectedRowsCount;

            // Tính tổng riêng cho phiếu nhập và phiếu xuất
            int countNhap = 0; // Số lượng phiếu nhập
            int countXuat = 0; // Số lượng phiếu xuất
            
            // Tổng cho phiếu nhập
            decimal nhapQuantity = 0;
            decimal nhapAmount = 0;
            decimal nhapVat = 0;
            decimal nhapAmountIncludedVat = 0;
            
            // Tổng cho phiếu xuất
            decimal xuatQuantity = 0;
            decimal xuatAmount = 0;
            decimal xuatVat = 0;
            decimal xuatAmountIncludedVat = 0;

            for (int i = 0; i < totalRows; i++)
            {
                if (StockInOutMasterHistoryDtoGridView.GetRow(i) is StockInOutMasterHistoryDto dto)
                {
                    // Đếm số phiếu nhập và xuất dựa vào tên enum
                    var enumName = dto.LoaiNhapXuatKho.ToString();
                    if (enumName.StartsWith("Nhap"))
                    {
                        countNhap++;
                        nhapQuantity += dto.TotalQuantity;
                        nhapAmount += dto.TotalAmount;
                        nhapVat += dto.TotalVat;
                        nhapAmountIncludedVat += dto.TotalAmountIncludedVat;
                    }
                    else if (enumName.StartsWith("Xuat"))
                    {
                        countXuat++;
                        xuatQuantity += dto.TotalQuantity;
                        xuatAmount += dto.TotalAmount;
                        xuatVat += dto.TotalVat;
                        xuatAmountIncludedVat += dto.TotalAmountIncludedVat;
                    }
                }
            }

            // Cập nhật tổng số phiếu nhập xuất với HTML formatting
            if (DataSummaryBarStaticItem != null)
            {
                if (totalRows == 0)
                {
                    // Không có dữ liệu - hiển thị màu xám, italic
                    DataSummaryBarStaticItem.Caption = @"<color=gray><i>Chưa có dữ liệu</i></color>";
                }
                else
                {
                    // Có dữ liệu - format chuyên nghiệp theo cấu trúc mới
                    // Dòng 1: Tổng số lượng phiếu nhập xuất
                    var summary = $@"<color=gray>Tổng:</color> <b><color=blue>{totalRows:N0}</color></b> <color=gray>phiếu nhập xuất</color>";
                    
                    // Dòng 2: Thông tin phiếu xuất
                    summary += $@"<color=gray> | Phiếu Xuất:</color> <b><color=blue>{countXuat:N0}</color></b> <color=gray>phiếu</color>";
                    summary += $@" - <color=gray>SL:</color> <b><color=blue>{xuatQuantity:N2}</color></b>";
                    summary += $@" - <color=gray>Trước VAT:</color> <b><color=blue>{xuatAmount:N0}</color></b>";
                    summary += $@" - <color=gray>VAT:</color> <b><color=blue>{xuatVat:N0}</color></b>";
                    summary += $@" - <color=gray>Sau VAT:</color> <b><color=blue>{xuatAmountIncludedVat:N0}</color></b>";
                    
                    // Dòng 3: Thông tin phiếu nhập
                    summary += $@"<color=gray> | Phiếu Nhập:</color> <b><color=blue>{countNhap:N0}</color></b> <color=gray>phiếu</color>";
                    summary += $@" - <color=gray>SL:</color> <b><color=blue>{nhapQuantity:N2}</color></b>";
                    summary += $@" - <color=gray>Trước VAT:</color> <b><color=blue>{nhapAmount:N0}</color></b>";
                    summary += $@" - <color=gray>VAT:</color> <b><color=blue>{nhapVat:N0}</color></b>";
                    summary += $@" - <color=gray>Sau VAT:</color> <b><color=blue>{nhapAmountIncludedVat:N0}</color></b>";
                    
                    DataSummaryBarStaticItem.Caption = summary;
                }
            }

            // Cập nhật số dòng đã chọn với HTML formatting
            if (SelectedRowBarStaticItem != null)
            {
                if (selectedRows > 0)
                {
                    // Có chọn dòng - format chuyên nghiệp
                    SelectedRowBarStaticItem.Caption = 
                        $@"<color=gray>Đã chọn:</color> <b><color=blue>{selectedRows:N0}</color></b> <color=gray>dòng</color>";
                }
                else
                {
                    // Không chọn dòng - hiển thị màu xám, italic
                    SelectedRowBarStaticItem.Caption = @"<color=gray><i>Chưa chọn dòng nào</i></color>";
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