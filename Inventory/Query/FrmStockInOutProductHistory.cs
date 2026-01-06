using Bll.Inventory.InventoryManagement;
using Common.Common;
using Common.Helpers;
using Common.Utils;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraReports.UI;
using DTO.Inventory.InventoryManagement;
using Inventory.OverlayForm;
using Inventory.StockIn.NhapBaoHanh;
using Inventory.StockIn.NhapHangThuongMai;
using Inventory.StockIn.NhapLuuChuyenKho;
using Inventory.StockIn.NhapNoiBo;
using Inventory.StockIn.NhapThietBiMuon;
using Inventory.StockOut.XuatBaoHanh;
using Inventory.StockOut.XuatChoThueMuon;
using Inventory.StockOut.XuatHangThuongMai;
using Inventory.StockOut.XuatLuuChuyenKho;
using Inventory.StockOut.XuatNoiBo;
using Inventory.Management;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Inventory.ProductVariantIdentifier;

namespace Inventory.Query;

/// <summary>
/// Form xem lịch sử sản phẩm nhập xuất kho
/// Cung cấp chức năng tìm kiếm theo từ khóa và khoảng thời gian
/// </summary>
public partial class FrmStockInOutProductHistory : DevExpress.XtraEditors.XtraForm
{
    #region ========== FIELDS & PROPERTIES ==========

    /// <summary>
    /// Business Logic Layer cho lịch sử sản phẩm nhập xuất kho
    /// </summary>
    private readonly StockInOutDetailBll _stockInOutDetailBll = new();

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

    public FrmStockInOutProductHistory()
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

            // Setup SuperToolTips
            SetupSuperToolTips();
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
    /// Thiết lập SuperToolTip cho tất cả các controls trong form
    /// </summary>
    private void SetupSuperToolTips()
    {
        try
        {
            // SuperTip cho Từ khóa tìm kiếm
            if (KeyWordBarEditItem != null)
            {
                var keywordSuperTip = SuperToolTipHelper.CreateSuperToolTip(
                    title: @"<b><color=DarkBlue>🔍 Từ khóa tìm kiếm</color></b>",
                    content: @"Nhập từ khóa để tìm kiếm trong lịch sử sản phẩm nhập xuất kho.<br/><br/><b>Chức năng:</b><br/>• Tìm kiếm theo từ khóa trong các trường: Mã sản phẩm, Tên sản phẩm, Mã biến thể, Tên biến thể, Số phiếu<br/>• Hỗ trợ tìm kiếm không dấu<br/>• Tìm kiếm không phân biệt hoa thường<br/><br/><b>Cách sử dụng:</b><br/>• Nhập từ khóa vào ô này<br/>• Click nút <b>Xem báo cáo</b> để thực hiện tìm kiếm<br/>• Để trống để xem tất cả dữ liệu trong khoảng thời gian<br/><br/><color=Gray>Lưu ý:</color> Tìm kiếm sẽ kết hợp với điều kiện lọc theo ngày."
                );
                KeyWordBarEditItem.SuperTip = keywordSuperTip;
            }

            // SuperTip cho Từ ngày
            if (TuNgayBarEditItem != null)
            {
                var tuNgaySuperTip = SuperToolTipHelper.CreateSuperToolTip(
                    title: @"<b><color=DarkBlue>📅 Từ ngày</color></b>",
                    content: @"Chọn ngày bắt đầu để lọc lịch sử sản phẩm nhập xuất kho.<br/><br/><b>Chức năng:</b><br/>• Lọc dữ liệu từ ngày này trở đi<br/>• Mặc định: Đầu tháng hiện tại<br/>• Kết hợp với <b>Đến ngày</b> để tạo khoảng thời gian<br/><br/><b>Ràng buộc:</b><br/>• Phải nhỏ hơn hoặc bằng <b>Đến ngày</b><br/>• Không được để trống<br/><br/><color=Gray>Lưu ý:</color> Hệ thống sẽ tự động kiểm tra và cảnh báo nếu Từ ngày > Đến ngày."
                );
                TuNgayBarEditItem.SuperTip = tuNgaySuperTip;
            }

            // SuperTip cho Đến ngày
            if (DenNgayBarEditItem != null)
            {
                var denNgaySuperTip = SuperToolTipHelper.CreateSuperToolTip(
                    title: @"<b><color=DarkBlue>📅 Đến ngày</color></b>",
                    content: @"Chọn ngày kết thúc để lọc lịch sử sản phẩm nhập xuất kho.<br/><br/><b>Chức năng:</b><br/>• Lọc dữ liệu đến ngày này<br/>• Mặc định: Ngày hiện tại<br/>• Kết hợp với <b>Từ ngày</b> để tạo khoảng thời gian<br/><br/><b>Ràng buộc:</b><br/>• Phải lớn hơn hoặc bằng <b>Từ ngày</b><br/>• Không được để trống<br/><br/><color=Gray>Lưu ý:</color> Hệ thống sẽ tự động kiểm tra và cảnh báo nếu Từ ngày > Đến ngày."
                );
                DenNgayBarEditItem.SuperTip = denNgaySuperTip;
            }

            // SuperTip cho nút Xem báo cáo
            if (XemBaoCaoBarButtonItem != null)
            {
                SuperToolTipHelper.SetBarButtonSuperTip(
                    XemBaoCaoBarButtonItem,
                    title: @"<b><color=Blue>📊 Xem báo cáo</color></b>",
                    content: @"Tải và hiển thị lịch sử sản phẩm nhập xuất kho theo điều kiện đã chọn.<br/><br/><b>Chức năng:</b><br/>• Tải dữ liệu từ database theo điều kiện:<br/>  - Khoảng thời gian (Từ ngày - Đến ngày)<br/>  - Từ khóa tìm kiếm (nếu có)<br/>• Hiển thị kết quả trong grid<br/>• Cập nhật thống kê tổng hợp<br/><br/><b>Quy trình:</b><br/>1. Validate điều kiện tìm kiếm<br/>2. Hiển thị SplashScreen trong khi tải<br/>3. Query dữ liệu từ database<br/>4. Hiển thị kết quả trong grid<br/>5. Cập nhật thống kê<br/><br/><color=Gray>Lưu ý:</color> Dữ liệu sẽ được tải bất đồng bộ để không block UI."
                );
            }

            // SuperTip cho nút Chi tiết phiếu nhập xuất
            if (ChiTietPhieuNhapXuatBarButtonItem != null)
            {
                SuperToolTipHelper.SetBarButtonSuperTip(
                    ChiTietPhieuNhapXuatBarButtonItem,
                    title: @"<b><color=Green>📄 Chi tiết phiếu nhập xuất</color></b>",
                    content: @"Mở form chi tiết phiếu nhập xuất kho cho sản phẩm được chọn.<br/><br/><b>Chức năng:</b><br/>• Mở form chi tiết tương ứng với loại phiếu:<br/>  - Nhập hàng thương mại<br/>  - Nhập thiết bị mượn thuê<br/>  - Nhập nội bộ<br/>  - Nhập lưu chuyển kho<br/>  - Nhập hàng bảo hành<br/>  - Xuất hàng thương mại<br/>  - Xuất thiết bị cho thuê mượn<br/>  - Xuất nội bộ<br/>  - Xuất lưu chuyển kho<br/>  - Xuất hàng bảo hành<br/>• Load dữ liệu từ ID phiếu đã chọn<br/><br/><b>Yêu cầu:</b><br/>• Chỉ cho phép chọn 1 sản phẩm<br/>• Phải có StockInOutMasterId hợp lệ<br/><br/><color=Gray>Lưu ý:</color> Form sẽ được mở với OverlayManager để tạo hiệu ứng overlay."
                );
            }

            // SuperTip cho nút In phiếu
            if (InPhieuBarButtonItem != null)
            {
                SuperToolTipHelper.SetBarButtonSuperTip(
                    InPhieuBarButtonItem,
                    title: @"<b><color=Orange>🖨️ In phiếu</color></b>",
                    content: @"In phiếu nhập xuất kho cho sản phẩm được chọn.<br/><br/><b>Chức năng:</b><br/>• Tạo và hiển thị preview của phiếu nhập/xuất<br/>• Cho phép in hoặc xuất PDF<br/>• Hỗ trợ nhiều loại phiếu khác nhau<br/><br/><b>Yêu cầu:</b><br/>• Chỉ cho phép chọn 1 sản phẩm<br/>• Phải có StockInOutMasterId hợp lệ<br/><br/><color=Gray>Lưu ý:</color> Chức năng này đang được phát triển."
                );
            }

            // SuperTip cho nút Thêm hình ảnh
            if (ThemHinhAnhBarButtonItem != null)
            {
                SuperToolTipHelper.SetBarButtonSuperTip(
                    ThemHinhAnhBarButtonItem,
                    title: @"<b><color=Purple>📷 Thêm hình ảnh</color></b>",
                    content: @"Thêm hình ảnh cho phiếu nhập xuất kho.<br/><br/><b>Chức năng:</b><br/>• Mở form thêm hình ảnh cho phiếu<br/>• Upload và quản lý hình ảnh liên quan đến phiếu<br/>• Hỗ trợ nhiều định dạng hình ảnh<br/><br/><b>Yêu cầu:</b><br/>• Chỉ cho phép chọn 1 sản phẩm<br/>• Phải có StockInOutMasterId hợp lệ<br/><br/><color=Gray>Lưu ý:</color> Form sẽ được mở với OverlayManager."
                );
            }

            // SuperTip cho nút Xóa phiếu
            if (XoaPhieuBarButtonItem != null)
            {
                SuperToolTipHelper.SetBarButtonSuperTip(
                    XoaPhieuBarButtonItem,
                    title: @"<b><color=Red>🗑️ Xóa phiếu</color></b>",
                    content: @"Xóa các sản phẩm nhập xuất kho được chọn.<br/><br/><b>Chức năng:</b><br/>• Xóa các StockInOutDetail được chọn<br/>• Tự động xóa StockInOutMaster nếu không còn detail nào<br/>• Cho phép xóa nhiều dòng cùng lúc<br/><br/><b>Quy trình:</b><br/>1. Hiển thị xác nhận xóa<br/>2. Xóa từng detail được chọn<br/>3. Kiểm tra và xóa master nếu không còn detail<br/>4. Reload dữ liệu sau khi xóa<br/>5. Hiển thị kết quả<br/><br/><b>Yêu cầu:</b><br/>• Phải chọn ít nhất 1 sản phẩm<br/>• Phải có Id (StockInOutDetailId) hợp lệ<br/><br/><color=Red>⚠️ Cảnh báo:</color> Hành động này không thể hoàn tác!"
                );
            }

            // SuperTip cho nút Nhập định danh sản phẩm
            if (NhapDinhDanhSPBarButtonItem != null)
            {
                SuperToolTipHelper.SetBarButtonSuperTip(
                    NhapDinhDanhSPBarButtonItem,
                    title: @"<b><color=Teal>🏷️ Nhập định danh sản phẩm</color></b>",
                    content: @"Nhập định danh sản phẩm (Serial Number, MAC, IMEI, v.v.).<br/><br/><b>Chức năng:</b><br/>• Mở form nhập định danh sản phẩm<br/>• Nhập các loại định danh: SerialNumber, PartNumber, QRCode, SKU, RFID, MACAddress, IMEI, AssetTag, LicenseKey, UPC, EAN, ID, OtherIdentifier<br/><br/><b>Yêu cầu:</b><br/>• Chỉ cho phép chọn 1 sản phẩm<br/>• Phải có ProductVariantId hợp lệ<br/><br/><color=Gray>Lưu ý:</color> Chức năng này đang được phát triển."
                );
            }

            // SuperTip cho nút Tạo Serial Number
            if (CreateSerialNumberBarButtonItem != null)
            {
                SuperToolTipHelper.SetBarButtonSuperTip(
                    CreateSerialNumberBarButtonItem,
                    title: @"<b><color=Green>🔢 Tạo Serial Number</color></b>",
                    content: @"Tạo serial numbers tự động cho sản phẩm được chọn.<br/><br/><b>Chức năng:</b><br/>• Mở form tạo serial numbers tự động<br/>• Tạo serial numbers theo format: <b>{VoucherNumber}-XXX</b><br/>• Tự động điền số lượng từ phiếu nhập/xuất<br/>• Lưu từng serial number vào database<br/><br/><b>Quy trình:</b><br/>1. Chọn sản phẩm từ grid<br/>2. Mở form FrmCreateNewSerialNumber<br/>3. Tự động tạo serial numbers<br/>4. Lưu vào bảng ProductVariantIdentifier<br/><br/><b>Yêu cầu:</b><br/>• Chỉ cho phép chọn 1 sản phẩm<br/>• Phải có ProductVariantId và Id hợp lệ<br/><br/><color=Gray>Lưu ý:</color> Serial numbers sẽ được kiểm tra trùng lặp trước khi lưu."
                );
            }

            // SuperTip cho nút Identifier
            if (IdentifiterBarButtonItem != null)
            {
                SuperToolTipHelper.SetBarButtonSuperTip(
                    IdentifiterBarButtonItem,
                    title: @"<b><color=Blue>🔖 Quản lý định danh</color></b>",
                    content: @"Quản lý các định danh sản phẩm (Serial Number, Part Number, QR Code, v.v.).<br/><br/><b>Chức năng:</b><br/>• Mở form quản lý định danh sản phẩm<br/>• Thêm/sửa/xóa các loại định danh:<br/>  - SerialNumber, PartNumber, QRCode<br/>  - SKU, RFID, MACAddress, IMEI<br/>  - AssetTag, LicenseKey, UPC, EAN<br/>  - ID, OtherIdentifier<br/>• Validate và kiểm tra trùng lặp<br/><br/><b>Yêu cầu:</b><br/>• Chỉ cho phép chọn 1 sản phẩm<br/>• Phải có ProductVariantId hợp lệ<br/><br/><color=Gray>Lưu ý:</color> Form sẽ được mở với OverlayManager. Bạn có thể quản lý nhiều loại định danh trong cùng một form."
                );
            }

        }
        catch (Exception ex)
        {
            _logger.Error("SetupSuperToolTips: Exception occurred", ex);
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
            ThemHinhAnhBarButtonItem.ItemClick += ThemHinhAnhBarButtonItem_ItemClick;
            XoaPhieuBarButtonItem.ItemClick += XoaPhieuBarButtonItem_ItemClick;
            NhapDinhDanhSPBarButtonItem.ItemClick += NhapDinhDanhSPBarButtonItem_ItemClick;
            CreateSerialNumberBarButtonItem.ItemClick += CreateSerialNumberBarButtonItem_ItemClick;
            IdentifiterBarButtonItem.ItemClick += IdentifiterBarButtonItem_ItemClick;


            // GridView events
            StockInOutProductHistoryDtoGridView.DoubleClick += StockInOutProductHistoryDtoGridView_DoubleClick;
            StockInOutProductHistoryDtoGridView.FocusedRowChanged += StockInOutProductHistoryDtoGridView_FocusedRowChanged;
            StockInOutProductHistoryDtoGridView.SelectionChanged += StockInOutProductHistoryDtoGridView_SelectionChanged;
            StockInOutProductHistoryDtoGridView.CustomDrawRowIndicator += StockInOutProductHistoryDtoGridView_CustomDrawRowIndicator;
            StockInOutProductHistoryDtoGridView.RowCellStyle += StockInOutProductHistoryDtoGridView_RowCellStyle;
            StockInOutProductHistoryDtoGridView.CustomColumnDisplayText += StockInOutProductHistoryDtoGridView_CustomColumnDisplayText;

            // Form events
            Load += FrmStockInOutProductHistory_Load;
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
    private void FrmStockInOutProductHistory_Load(object sender, EventArgs e)
    {

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
    /// Mở form tương ứng dựa vào LoaiNhapXuatKhoEnum và load dữ liệu từ ID đã chọn
    /// </summary>
    private void ChiTietPhieuNhapXuatBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
        try
        {
            // Kiểm tra số lượng phiếu được chọn - chỉ cho phép 1 phiếu
            var selectedStockInOutMasterIds =
                GridViewHelper.GetSelectedRowColumnValues<Guid>(StockInOutProductHistoryDtoGridView,
                    "StockInOutMasterId");
            if (selectedStockInOutMasterIds.Count == 0)
            {
                MsgBox.ShowWarning("Vui lòng chọn một sản phẩm để xem chi tiết phiếu.");
                return;
            }

            if (selectedStockInOutMasterIds.Count > 1)
            {
                MsgBox.ShowWarning("Chỉ cho phép xem chi tiết 1 phiếu. Vui lòng bỏ chọn bớt.");
                return;
            }

            _selectedStockInOutMasterId = selectedStockInOutMasterIds[0];

            // Kiểm tra ID phiếu được chọn
            if (!_selectedStockInOutMasterId.HasValue || _selectedStockInOutMasterId.Value == Guid.Empty)
            {
                MsgBox.ShowWarning("Vui lòng chọn sản phẩm để xem chi tiết phiếu.");
                return;
            }

            // Lấy DTO từ row được chọn để xác định loại nhập xuất
            var focusedRowHandle = StockInOutProductHistoryDtoGridView.FocusedRowHandle;
            if (focusedRowHandle < 0)
            {
                MsgBox.ShowWarning("Vui lòng chọn sản phẩm để xem chi tiết phiếu.");
                return;
            }

            if (StockInOutProductHistoryDtoGridView.GetRow(focusedRowHandle) is not StockInOutProductHistoryDto selectedDto)
            {
                MsgBox.ShowWarning("Không thể lấy thông tin sản phẩm được chọn.");
                return;
            }

            //FIXME: Điều chỉnh thêm các trường hợp khác cho đúng với LoaiNhapXuatKhoEnum
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

            // Mở form chi tiết
            OpenDetailForm(detailForm);
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
            var selectedCount = StockInOutProductHistoryDtoGridView.SelectedRowsCount;
            if (selectedCount == 0)
            {
                MsgBox.ShowWarning("Vui lòng chọn một sản phẩm để in phiếu.");
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
                MsgBox.ShowWarning("Vui lòng chọn sản phẩm để in phiếu.");
                return;
            }

            // Lấy DTO từ row được chọn để xác định loại nhập xuất
            var focusedRowHandle = StockInOutProductHistoryDtoGridView.FocusedRowHandle;
            if (focusedRowHandle < 0)
            {
                MsgBox.ShowWarning("Vui lòng chọn sản phẩm để in phiếu.");
                return;
            }

            if (StockInOutProductHistoryDtoGridView.GetRow(focusedRowHandle) is not StockInOutProductHistoryDto selectedDto)
            {
                MsgBox.ShowWarning("Không thể lấy thông tin sản phẩm được chọn.");
                return;
            }

            // TODO: Implement print functionality
        }
        catch (Exception ex)
        {
            _logger.Error("InPhieuBarButtonItem_ItemClick: Exception occurred", ex);
            MsgBox.ShowError($"Lỗi in phiếu: {ex.Message}");
        }
    }


    /// <summary>
    /// Event handler cho nút Nhập định danh sản phẩm
    /// Chỉ mở màn hình cho 1 phiếu được chọn, sử dụng OverlayManager
    /// </summary>
    private void NhapDinhDanhSPBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
        try
        {
            // Kiểm tra số lượng phiếu được chọn - chỉ cho phép 1 phiếu
            var selectedCount = StockInOutProductHistoryDtoGridView.SelectedRowsCount;
            if (selectedCount == 0)
            {
                MsgBox.ShowWarning("Vui lòng chọn một sản phẩm để nhập định danh.");
                return;
            }

            if (selectedCount > 1)
            {
                MsgBox.ShowWarning("Chỉ cho phép nhập định danh cho 1 phiếu. Vui lòng bỏ chọn bớt.");
                return;
            }

            // Kiểm tra ID phiếu được chọn
            if (!_selectedStockInOutMasterId.HasValue || _selectedStockInOutMasterId.Value == Guid.Empty)
            {
                MsgBox.ShowWarning("Vui lòng chọn sản phẩm để nhập định danh.");
                return;
            }

            // TODO: Form FrmNhapSerialMacEmei đã bị xóa, cần thay thế bằng form mới
            MsgBox.ShowWarning("Chức năng này đang được phát triển.");
        }
        catch (Exception ex)
        {
            _logger.Error("NhapDinhDanhSPBarButtonItem_ItemClick: Exception occurred", ex);
            MsgBox.ShowError($"Lỗi mở form nhập định danh: {ex.Message}");
        }
    }

    /// <summary>
    /// Handles the click event for the "Create Serial Number" button.
    /// </summary>
    /// <param name="sender">The source of the event, typically the button that was clicked.</param>
    /// <param name="e">An <see cref="DevExpress.XtraBars.ItemClickEventArgs"/> that contains the event data.</param>
    /// <remarks>
    /// This method performs the following actions:
    /// - Validates the selected rows in the grid to ensure only one row is selected.
    /// - Retrieves the data transfer object (DTO) of the selected product.
    /// - Validates the <c>ProductVariantId</c> and <c>Id</c> of the selected product.
    /// - Opens a form to create a new QR code for the selected product.
    /// - Displays appropriate warning messages if validation fails.
    /// - Logs any exceptions that occur during execution.
    /// </remarks>
    /// <exception cref="Exception">Logs and displays an error message if an exception occurs.</exception>
    private void CreateSerialNumberBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
        try
        {
            // Kiểm tra số lượng dòng được chọn - chỉ cho phép 1 dòng
            var selectedCount = StockInOutProductHistoryDtoGridView.SelectedRowsCount;
            if (selectedCount == 0)
            {
                MsgBox.ShowWarning("Vui lòng chọn một sản phẩm để tạo mã QR.");
                return;
            }

            if (selectedCount > 1)
            {
                MsgBox.ShowWarning("Chỉ cho phép tạo mã QR cho 1 sản phẩm. Vui lòng bỏ chọn bớt.");
                return;
            }

            // Lấy DTO từ row được chọn
            var focusedRowHandle = StockInOutProductHistoryDtoGridView.FocusedRowHandle;
            if (focusedRowHandle < 0)
            {
                MsgBox.ShowWarning("Vui lòng chọn sản phẩm để tạo mã QR.");
                return;
            }

            if (StockInOutProductHistoryDtoGridView.GetRow(focusedRowHandle) is not StockInOutProductHistoryDto selectedDto)
            {
                MsgBox.ShowWarning("Không thể lấy thông tin sản phẩm được chọn.");
                return;
            }

            // Kiểm tra ProductVariantId
            if (selectedDto.ProductVariantId == Guid.Empty)
            {
                MsgBox.ShowWarning("Sản phẩm được chọn không có ProductVariantId hợp lệ.");
                return;
            }

            // Kiểm tra Id (StockInOutDetailId)
            if (selectedDto.Id == Guid.Empty)
            {
                MsgBox.ShowWarning("Sản phẩm được chọn không có Id hợp lệ.");
                return;
            }

            // Mở form tạo QR code với OverlayManager
            using (OverlayManager.ShowScope(this))
            {
                using var form = new FrmCreateNewSerialNumber(selectedDto);
                
                form.ShowDialog(this);
            }
        }
        catch (Exception ex)
        {
            _logger.Error("CreateSerialNumberBarButtonItem_ItemClick: Exception occurred", ex);
            MsgBox.ShowError($"Lỗi mở form tạo mã QR: {ex.Message}");
        }
    }

    /// <summary>
    /// Event handler cho nút Identifier
    /// Mở form tạo QR code cho sản phẩm được chọn, truyền ProductVariantId và Id (StockInOutDetailId)
    /// </summary>
    private void IdentifiterBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
        try
        {
            // Kiểm tra số lượng dòng được chọn - chỉ cho phép 1 dòng
            var selectedCount = StockInOutProductHistoryDtoGridView.SelectedRowsCount;
            if (selectedCount == 0)
            {
                MsgBox.ShowWarning("Vui lòng chọn một sản phẩm để quản lý định danh.");
                return;
            }

            if (selectedCount > 1)
            {
                MsgBox.ShowWarning("Chỉ cho phép quản lý định danh cho 1 sản phẩm. Vui lòng bỏ chọn bớt.");
                return;
            }

            // Lấy DTO từ row được chọn
            var focusedRowHandle = StockInOutProductHistoryDtoGridView.FocusedRowHandle;
            if (focusedRowHandle < 0)
            {
                MsgBox.ShowWarning("Vui lòng chọn sản phẩm để quản lý định danh.");
                return;
            }

            if (StockInOutProductHistoryDtoGridView.GetRow(focusedRowHandle) is not StockInOutProductHistoryDto selectedDto)
            {
                MsgBox.ShowWarning("Không thể lấy thông tin sản phẩm được chọn.");
                return;
            }

            // Kiểm tra ProductVariantId
            if (selectedDto.ProductVariantId == Guid.Empty)
            {
                MsgBox.ShowWarning("Sản phẩm được chọn không có ProductVariantId hợp lệ.");
                return;
            }

            // Kiểm tra Id (StockInOutDetailId)
            if (selectedDto.Id == Guid.Empty)
            {
                MsgBox.ShowWarning("Sản phẩm được chọn không có Id hợp lệ.");
                return;
            }

            // Mở form tạo QR code với OverlayManager
            using (OverlayManager.ShowScope(this))
            {
                using var form = new FrmProductVariantIdentifierAddEdit(selectedDto,Guid.Empty);
                
                form.ShowDialog(this);
            }
        }
        catch (Exception ex)
        {
            _logger.Error("IdentifiterBarButtonItem_ItemClick: Exception occurred", ex);
            MsgBox.ShowError($"Lỗi mở form quản lý định danh: {ex.Message}");
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
            var selectedCount = StockInOutProductHistoryDtoGridView.SelectedRowsCount;
            switch (selectedCount)
            {
                case 0:
                    MsgBox.ShowWarning("Vui lòng chọn một sản phẩm để thêm hình ảnh.");
                    return;
                case > 1:
                    MsgBox.ShowWarning("Chỉ cho phép thêm hình ảnh cho 1 phiếu. Vui lòng bỏ chọn bớt.");
                    return;
            }

            // Kiểm tra ID phiếu được chọn
            if (!_selectedStockInOutMasterId.HasValue || _selectedStockInOutMasterId.Value == Guid.Empty)
            {
                MsgBox.ShowWarning("Vui lòng chọn sản phẩm để thêm hình ảnh.");
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
    /// Xóa các StockInOutDetail được chọn, nếu master không còn detail nào thì xóa luôn master
    /// Cho phép xóa nhiều dòng cùng lúc
    /// </summary>
    private async void XoaPhieuBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
        try
        {
            // Kiểm tra số lượng sản phẩm được chọn
            var selectedCount = StockInOutProductHistoryDtoGridView.SelectedRowsCount;
            if (selectedCount == 0)
            {
                MsgBox.ShowWarning("Vui lòng chọn ít nhất một sản phẩm để xóa.");
                return;
            }

            // Lấy tất cả các dòng được chọn
            var selectedRowHandles = StockInOutProductHistoryDtoGridView.GetSelectedRows();
            var selectedDtos = selectedRowHandles
                .Select(handle => StockInOutProductHistoryDtoGridView.GetRow(handle) as StockInOutProductHistoryDto)
                .Where(dto => dto != null && dto.Id != Guid.Empty)
                .ToList();

            if (selectedDtos.Count == 0)
            {
                MsgBox.ShowWarning("Không có sản phẩm hợp lệ để xóa.");
                return;
            }

            // Hiển thị confirmation dialog
            var confirmMessage = selectedDtos.Count == 1
                ? $"Bạn có chắc muốn xóa sản phẩm:\n<b>{GetProductDisplayName(selectedDtos[0])}</b>\n" +
                  $"Trong phiếu: <b>{selectedDtos[0].VocherNumber ?? "N/A"}</b>?\n\n" +
                  "Hành động này không thể hoàn tác!"
                : $"Bạn có chắc muốn xóa <b>{selectedDtos.Count}</b> sản phẩm?\n\n" +
                  "Hành động này không thể hoàn tác!";

            if (!MsgBox.ShowYesNo(confirmMessage, "Xác nhận xóa"))
            {
                return;
            }

            // Thực hiện xóa với OverlayManager
            var deletedCount = 0;
            var masterIdsToCheck = new System.Collections.Generic.HashSet<Guid>();


            await Task.Run(() =>
            {
                try
                {
                    foreach (var dto in selectedDtos)
                    {
                        try
                        {
                            // Xóa detail
                            var masterId = _stockInOutDetailBll.Delete(dto.Id);

                            masterIdsToCheck.Add(masterId);
                            deletedCount++;
                        }
                        catch (Exception ex)
                        {
                            _logger.Error($"XoaPhieuBarButtonItem_ItemClick: Lỗi xóa detail {dto.Id}: {ex.Message}",
                                ex);
                            // Tiếp tục xóa các detail khác
                        }
                    }

                    // Kiểm tra và xóa các master không còn detail
                    foreach (var masterId in masterIdsToCheck)
                    {
                        try
                        {
                            var hasRemainingDetails = _stockInOutDetailBll.HasRemainingDetails(masterId);

                            if (hasRemainingDetails) continue;

                            // Nếu không còn detail nào, xóa luôn master
                            var stockInOutMasterBll = new StockInOutMasterBll();
                            stockInOutMasterBll.Delete(masterId);
                        }
                        catch (Exception ex)
                        {
                            _logger.Error(
                                $"XoaPhieuBarButtonItem_ItemClick: Lỗi kiểm tra/xóa master {masterId}: {ex.Message}",
                                ex);
                            // Tiếp tục với master khác
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error("XoaPhieuBarButtonItem_ItemClick: Exception during delete operation", ex);
                    BeginInvoke(new Action(() => { MsgBox.ShowError($"Lỗi xóa sản phẩm: {ex.Message}"); }));
                    throw;
                }
            });


            // Reload data sau khi xóa thành công
            await LoadDataAsync();

            if (deletedCount == selectedDtos.Count)
            {
                MsgBox.ShowSuccess($"Đã xóa thành công {deletedCount} sản phẩm.");
            }
            else
            {
                MsgBox.ShowWarning($"Đã xóa {deletedCount}/{selectedDtos.Count} sản phẩm. Vui lòng kiểm tra lại.");
            }

        }
        catch (Exception ex)
        {
            _logger.Error("XoaPhieuBarButtonItem_ItemClick: Exception occurred", ex);
            MsgBox.ShowError($"Lỗi xóa sản phẩm: {ex.Message}");
        }
    }

    /// <summary>
    /// Lấy tên hiển thị của sản phẩm từ DTO
    /// </summary>
    private string GetProductDisplayName(StockInOutProductHistoryDto dto)
    {
        if (dto == null) return "N/A";

        if (!string.IsNullOrWhiteSpace(dto.ProductName))
            return dto.ProductName;

        if (!string.IsNullOrWhiteSpace(dto.ProductVariantFullName))
            return dto.ProductVariantFullName;

        return dto.ProductVariantCode ?? "N/A";
    }

    /// <summary>
    /// Event handler khi double click trên GridView
    /// </summary>
    private void StockInOutProductHistoryDtoGridView_DoubleClick(object sender, EventArgs e)
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
            _logger.Error("StockInOutProductHistoryDtoGridView_DoubleClick: Exception occurred", ex);
            MsgBox.ShowError($"Lỗi: {ex.Message}");
        }
    }

    /// <summary>
    /// Event handler khi row được chọn thay đổi
    /// </summary>
    private void StockInOutProductHistoryDtoGridView_FocusedRowChanged(object sender,
        DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
    {
        try
        {
            UpdateSelectedItem();
        }
        catch (Exception ex)
        {
            _logger.Error("StockInOutProductHistoryDtoGridView_FocusedRowChanged: Exception occurred", ex);
        }
    }

    /// <summary>
    /// Event handler khi selection thay đổi trên GridView
    /// </summary>
    private void StockInOutProductHistoryDtoGridView_SelectionChanged(object sender, EventArgs e)
    {
        try
        {
            UpdateSelectedItem();
            UpdateDataSummary();
        }
        catch (Exception ex)
        {
            _logger.Error("StockInOutProductHistoryDtoGridView_SelectionChanged: Exception occurred", ex);
        }
    }

    /// <summary>
    /// Xử lý sự kiện vẽ số thứ tự dòng cho GridView
    /// Sử dụng helper từ Common để hiển thị số thứ tự (1, 2, 3...)
    /// </summary>
    private void StockInOutProductHistoryDtoGridView_CustomDrawRowIndicator(object sender,
        RowIndicatorCustomDrawEventArgs e)
    {
        try
        {
            GridViewHelper.CustomDrawRowIndicator(StockInOutProductHistoryDtoGridView, e);
        }
        catch (Exception ex)
        {
            _logger.Error("StockInOutProductHistoryDtoGridView_CustomDrawRowIndicator: Exception occurred", ex);
        }
    }


    /// <summary>
    /// Xử lý sự kiện custom column display text
    /// Ẩn các giá trị 0 cho các cột numeric
    /// </summary>
    private void StockInOutProductHistoryDtoGridView_CustomColumnDisplayText(object sender,
        DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
    {
        try
        {
            // Chỉ xử lý các cột numeric có thể có giá trị 0
            if (e.Column == colStockInQty ||
                e.Column == colStockOutQty ||
                e.Column == colUnitPrice ||
                e.Column == colVatAmount ||
                e.Column == colTotalAmount ||
                e.Column == colTotalAmountIncludedVat)
            {
                // Lấy giá trị thực tế từ cell
                var value = e.Value;

                // Kiểm tra nếu giá trị là 0 hoặc null
                if (value == null || value == DBNull.Value)
                {
                    e.DisplayText = string.Empty;
                    return;
                }

                // Chuyển đổi sang decimal để so sánh
                if (decimal.TryParse(value.ToString(), out decimal decimalValue))
                {
                    if (decimalValue == 0)
                    {
                        e.DisplayText = string.Empty;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.Error("StockInOutProductHistoryDtoGridView_CustomColumnDisplayText: Exception occurred", ex);
            // Không hiển thị lỗi để không làm gián đoạn hiển thị
        }
    }

    /// <summary>
    /// Xử lý sự kiện tô màu cell theo trạng thái hoặc điều kiện
    /// Tô màu nền dòng theo loại phiếu: Nhập (xanh lá), Xuất (đỏ/cam), Điều chỉnh (vàng)
    /// </summary>
    private void StockInOutProductHistoryDtoGridView_RowCellStyle(object sender,
        RowCellStyleEventArgs e)
    {
        try
        {
            if (sender is not GridView gridView) return;

            // Bỏ qua các dòng không hợp lệ (header, footer, group row)
            if (e.RowHandle < 0) return;

            // Không tô màu khi dòng đang được chọn để giữ màu chọn mặc định của DevExpress
            if (gridView.IsRowSelected(e.RowHandle)) return;

            if (gridView.GetRow(e.RowHandle) is not StockInOutProductHistoryDto dto) return;

            // Xác định loại phiếu dựa trên StockInQty và StockOutQty
            System.Drawing.Color backColor;

            if (dto.StockInQty > 0 && dto.StockOutQty == 0)
            {
                // Phiếu nhập: màu xanh lá nhạt
                backColor = System.Drawing.Color.LightGreen;
            }
            else if (dto.StockOutQty > 0 && dto.StockInQty == 0)
            {
                // Phiếu xuất: màu đỏ/cam nhạt
                backColor = System.Drawing.Color.LightCoral;
            }
            else if (dto.StockInQty > 0 && dto.StockOutQty > 0)
            {
                // Điều chỉnh (cả nhập và xuất): màu vàng nhạt
                backColor = System.Drawing.Color.LightYellow;
            }
            else
            {
                // Trường hợp đặc biệt (cả hai đều = 0): màu trắng
                backColor = System.Drawing.Color.White;
            }

            // Áp dụng màu nền cho toàn bộ dòng
            e.Appearance.BackColor = backColor;
            e.Appearance.ForeColor = System.Drawing.Color.Black; // Chữ đen để tương phản tốt
            e.Appearance.Options.UseBackColor = true;
            e.Appearance.Options.UseForeColor = true;
        }
        catch (Exception ex)
        {
            _logger.Error("StockInOutProductHistoryDtoGridView_RowCellStyle: Exception occurred", ex);
            // Ignore style errors để không ảnh hưởng đến hiển thị
        }
    }


    #endregion

    #region ========== DATA LOADING ==========

    /// <summary>
    /// Load dữ liệu lịch sử sản phẩm nhập xuất kho
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

                // Lấy từ khóa tìm kiếm
                var keyword = KeyWordBarEditItem.EditValue as string;
                if (string.IsNullOrWhiteSpace(keyword))
                {
                    keyword = null;
                }

                // Lấy dữ liệu từ BLL (trả về DTOs)
                var dtos = _stockInOutDetailBll.QueryProductHistoryDto(
                    fromDate.Date,
                    toDate.Date.AddDays(1).AddTicks(-1), // Đến cuối ngày
                    keyword);

                // Update UI thread
                BeginInvoke(new Action(() =>
                {
                    stockInOutProductHistoryDtoBindingSource.DataSource = dtos;
                    stockInOutProductHistoryDtoBindingSource.ResetBindings(false);

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
            var focusedRowHandle = StockInOutProductHistoryDtoGridView.FocusedRowHandle;

            if (focusedRowHandle >= 0)
            {
                if (StockInOutProductHistoryDtoGridView.GetRow(focusedRowHandle) is StockInOutProductHistoryDto dto)
                {
                    _selectedStockInOutMasterId = dto.StockInOutMasterId;
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
            var selectedCount = StockInOutProductHistoryDtoGridView.SelectedRowsCount;

            // Các nút chỉ cho phép 1 dòng: Chi tiết, In phiếu, Nhập bảo hành, Thêm hình ảnh, Nhập định danh, Tạo mã QR, Identifier
            ChiTietPhieuNhapXuatBarButtonItem.Enabled = hasSelection && selectedCount == 1;
            InPhieuBarButtonItem.Enabled = hasSelection && selectedCount == 1;
            NhapBaoHanhBarButtonItem.Enabled = hasSelection && selectedCount == 1;
            ThemHinhAnhBarButtonItem.Enabled = hasSelection && selectedCount == 1;
            NhapDinhDanhSPBarButtonItem.Enabled = hasSelection && selectedCount == 1;
            CreateSerialNumberBarButtonItem.Enabled = hasSelection && selectedCount == 1;
            IdentifiterBarButtonItem.Enabled = hasSelection && selectedCount == 1;

            // Nút Xóa: cho phép xóa nhiều dòng (chỉ cần có selection)
            XoaPhieuBarButtonItem.Enabled = selectedCount > 0;
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
            var totalRows = StockInOutProductHistoryDtoGridView.RowCount;
            var selectedRows = StockInOutProductHistoryDtoGridView.SelectedRowsCount;

            // Cập nhật tổng số sản phẩm nhập xuất với HTML formatting
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
                    // Text "sản phẩm nhập xuất" màu xám
                    DataSummaryBarStaticItem.Caption =
                        $@"<size=9><color=#757575>Tổng:</color></size> " +
                        $@"<b><color=blue>{totalRows:N0}</color></b> " +
                        $@"<size=9><color=#757575>sản phẩm nhập xuất</color></size>";
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

    /// <summary>
    /// Hiển thị preview của report bằng ReportPrintTool
    /// </summary>
    /// <param name="report">Report cần in</param>
    private void PrintReport(XtraReport report)
    {
        try
        {
            if (report == null)
            {
                _logger.Warning("PrintReport: Report is null");
                MsgBox.ShowWarning("Không thể tạo report để in.");
                return;
            }

            using var printTool = new ReportPrintTool(report);
            printTool.ShowPreviewDialog();
        }
        catch (Exception ex)
        {
            _logger.Error($"PrintReport: Lỗi in report: {ex.Message}", ex);
            MsgBox.ShowError($"Lỗi in phiếu: {ex.Message}");
        }
    }

    /// <summary>
    /// Mở form chi tiết phiếu nhập xuất kho
    /// </summary>
    /// <param name="form">Form cần mở (sẽ được dispose sau khi đóng)</param>
    private void OpenDetailForm(Form form)
    {
        try
        {
            if (form == null)
            {
                _logger.Warning("OpenDetailForm: Form is null");
                MsgBox.ShowWarning("Không thể tạo form để xem chi tiết.");
                return;
            }

            using (OverlayManager.ShowScope(this))
            using (form)
            {
                form.FormBorderStyle = FormBorderStyle.FixedToolWindow;

                form.StartPosition = FormStartPosition.CenterParent;
                form.ShowDialog(this);
            }
        }
        catch (Exception ex)
        {
            _logger.Error($"OpenDetailForm: Lỗi mở form: {ex.Message}", ex);
            MsgBox.ShowError($"Lỗi mở form chi tiết: {ex.Message}");
        }
    }

    #endregion
}