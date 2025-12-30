using Bll.Inventory.InventoryManagement;
using Bll.Inventory.StockInOut;
using Bll.MasterData.CompanyBll;
using Common;
using Common.Utils;
using DTO.Inventory;
using DTO.Inventory.InventoryManagement;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.StockOut.XuatLapRap
{
    public partial class UcXuatLapRapMasterDto : DevExpress.XtraEditors.XtraUserControl
    {
        #region ========== KHAI BÁO BIẾN ==========

        /// <summary>
        /// Business Logic Layer cho CompanyBranch (dùng cho Warehouse lookup)
        /// </summary>
        private readonly CompanyBranchBll _companyBranchBll = new();

        /// <summary>
        /// Business Logic Layer cho StockIn (dùng để lấy master DTO)
        /// </summary>
        private readonly StockInOutBll _stockInOutBll = new();

        /// <summary>
        /// Business Logic Layer cho StockInOutMaster (dùng để tạo số phiếu)
        /// </summary>
        private readonly StockInOutMasterBll _stockInOutMasterBll = new();

        private Guid _stockInOutMasterId = Guid.Empty;

        private Guid _selectedWarehouseId = Guid.Empty;

        #endregion

        #region ========== CONSTRUCTOR ==========

        public UcXuatLapRapMasterDto()
        {
            InitializeComponent();
            InitializeControl();
        }

        #endregion

        #region ========== INITIALIZATION ==========

        /// <summary>
        /// Khởi tạo control
        /// </summary>
        private void InitializeControl()
        {
            try
            {
                // Setup SearchLookUpEdit cho Warehouse
                SetupLookupEdits();

                // Setup events
                SetupEvents();

                // Setup SuperToolTips
                SetupSuperToolTips();

                // Không load dữ liệu lookup ở đây, sẽ được gọi từ form khi FormLoad
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khởi tạo control");
            }
        }

        /// <summary>
        /// Setup SearchLookUpEdit cho Warehouse
        /// </summary>
        private void SetupLookupEdits()
        {
            try
            {
                // Setup Warehouse SearchLookUpEdit
                WarehouseNameSearchLookupEdit.Properties.DataSource = companyBranchDtoBindingSource;
                WarehouseNameSearchLookupEdit.Properties.ValueMember = "Id";
                WarehouseNameSearchLookupEdit.Properties.DisplayMember = "ThongTinHtml";
                WarehouseNameSearchLookupEdit.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
                WarehouseNameSearchLookupEdit.Properties.PopupView = CompanyBranchDtoSearchLookUpEdit1View;

                // Đảm bảo column ThongTinHtml được cấu hình đúng (đã có sẵn trong Designer)
                if (colThongTinHtml != null)
                {
                    colThongTinHtml.FieldName = "ThongTinHtml";
                    colThongTinHtml.Visible = true;
                    colThongTinHtml.VisibleIndex = 0;
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi thiết lập lookup edits");
            }
        }

        /// <summary>
        /// Thiết lập sự kiện
        /// </summary>
        private void SetupEvents()
        {
            try
            {
                Load += UcXuatLapRapMasterDto_Load;

                //Sự kiện của WarehouseNameSearchLookupEdit
                WarehouseNameSearchLookupEdit.Popup += WarehouseNameSearchLookupEdit_Popup;
                WarehouseNameSearchLookupEdit.EditValueChanged += WarehouseNameSearchLookupEdit_EditValueChanged;

                StockOutDateDateEdit.EditValueChanged += StockOutDateDateEdit_EditValueChanged;
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi thiết lập sự kiện");
            }
        }

        private async void WarehouseNameSearchLookupEdit_Popup(object sender, EventArgs e)
        {
            try
            {
                await LoadWarehouseDataSourceAsync();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi tải dữ liệu kho");
            }
        }

        #endregion

        #region ========== SUPERTOOLTIP ==========

        /// <summary>
        /// Thiết lập SuperToolTip cho tất cả các controls trong UserControl
        /// </summary>
        private void SetupSuperToolTips()
        {
            try
            {
                SetupTextEditSuperTips();
                SetupDateEditSuperTips();
                SetupSearchLookupEditSuperTips();
                SetupMemoEditSuperTips();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi thiết lập SuperToolTip");
            }
        }

        /// <summary>
        /// Thiết lập SuperToolTip cho các TextEdit controls
        /// </summary>
        private void SetupTextEditSuperTips()
        {
            // SuperTip cho Số phiếu xuất nội bộ
            if (StockOutNumberTextEdit != null)
            {
                SuperToolTipHelper.SetTextEditSuperTip(
                    StockOutNumberTextEdit,
                    title: @"<b><color=DarkBlue>📄 Số phiếu xuất lắp ráp</color></b>",
                    content:
                    @"Số phiếu xuất lắp ráp được tạo tự động theo format: <b>PXK-MMYY-NNXXX</b><br/><br/><b>Format:</b><br/>• PXK: Phiếu xuất kho<br/>• MM: Tháng (2 ký tự)<br/>• YY: Năm (2 ký tự cuối)<br/>• NN: Index của Loại xuất kho (2 ký tự)<br/>• XXX: Số thứ tự phiếu (3 ký tự từ 001 đến 999)<br/><br/><b>Chức năng:</b><br/>• Tự động tạo khi thay đổi ngày xuất lắp ráp<br/>• Tự động tạo khi thay đổi loại xuất kho<br/>• Query database để lấy số thứ tự tiếp theo<br/>• Đảm bảo số phiếu duy nhất trong cùng tháng/năm/loại<br/><br/><b>Ràng buộc:</b><br/>• <b>Bắt buộc nhập</b> (có dấu * đỏ)<br/>• Không được để trống<br/>• Tối đa 50 ký tự<br/><br/><color=Gray>Lưu ý:</color> Số phiếu xuất lắp ráp sẽ được lưu vào database khi lưu phiếu xuất."
                );
            }


            // SuperTip cho Người nhận hàng
            if (NguoiNhanHangTextEdit != null)
            {
                SuperToolTipHelper.SetTextEditSuperTip(
                    NguoiNhanHangTextEdit,
                    title: @"<b><color=DarkBlue>👤 Người nhận hàng</color></b>",
                    content:
                    @"Nhập tên người nhận hàng lắp ráp tại kho.<br/><br/><b>Chức năng:</b><br/>• Ghi nhận thông tin người nhận hàng lắp ráp<br/>• Hỗ trợ tra cứu và theo dõi<br/><br/><b>Ràng buộc:</b><br/>• <b>Bắt buộc nhập</b> (có dấu * đỏ)<br/>• Tối đa 500 ký tự<br/><br/><color=Gray>Lưu ý:</color> Thông tin này sẽ được lưu vào database khi lưu phiếu xuất lắp ráp."
                );
            }

            // SuperTip cho Người giao hàng
            if (NguoiGiaoHangTextEdit != null)
            {
                SuperToolTipHelper.SetTextEditSuperTip(
                    NguoiGiaoHangTextEdit,
                    title: @"<b><color=DarkBlue>🚚 Người giao hàng</color></b>",
                    content:
                    @"Nhập tên người giao hàng lắp ráp (từ chi nhánh/đơn vị).<br/><br/><b>Chức năng:</b><br/>• Ghi nhận thông tin người giao hàng lắp ráp<br/>• Hỗ trợ tra cứu và theo dõi<br/><br/><b>Ràng buộc:</b><br/>• <b>Bắt buộc nhập</b> (có dấu * đỏ)<br/>• Tối đa 500 ký tự<br/><br/><color=Gray>Lưu ý:</color> Thông tin này sẽ được lưu vào database khi lưu phiếu xuất lắp ráp."
                );
            }
        }

        /// <summary>
        /// Thiết lập SuperToolTip cho các DateEdit controls
        /// </summary>
        private void SetupDateEditSuperTips()
        {
            // SuperTip cho Ngày xuất nội bộ
            if (StockOutDateDateEdit != null)
            {
                SuperToolTipHelper.SetBaseEditSuperTip(
                    StockOutDateDateEdit,
                    title: @"<b><color=DarkBlue>📅 Ngày xuất lắp ráp</color></b>",
                    content:
                    @"Chọn ngày xuất lắp ráp cho phiếu xuất.<br/><br/><b>Chức năng:</b><br/>• Xác định thời điểm xuất lắp ráp<br/>• Tự động tạo số phiếu xuất lắp ráp dựa trên ngày<br/>• Format số phiếu: PXK-MMYY-NNXXX (MM, YY từ ngày này)<br/>• Query database để lấy số thứ tự tiếp theo trong tháng/năm<br/><br/><b>Ràng buộc:</b><br/>• <b>Bắt buộc nhập</b> (có dấu * đỏ)<br/>• Không được để trống<br/>• Mặc định: Ngày hiện tại<br/><br/><b>Validation:</b><br/>• Kiểm tra rỗng khi validating<br/>• Hiển thị lỗi qua ErrorProvider nếu không hợp lệ<br/><br/><color=Gray>Lưu ý:</color> Khi thay đổi ngày xuất lắp ráp, hệ thống sẽ tự động tạo lại số phiếu xuất lắp ráp theo format mới."
                );
            }
        }

        /// <summary>
        /// Thiết lập SuperToolTip cho các SearchLookUpEdit controls
        /// </summary>
        private void SetupSearchLookupEditSuperTips()
        {
            // SuperTip cho Kho xuất
            if (WarehouseNameSearchLookupEdit != null)
            {
                SuperToolTipHelper.SetBaseEditSuperTip(
                    WarehouseNameSearchLookupEdit,
                    title: @"<b><color=DarkBlue>🏢 Kho xuất</color></b>",
                    content:
                    @"Chọn kho xuất lắp ráp từ danh sách chi nhánh (Company Branch) đang hoạt động.<br/><br/><b>Chức năng:</b><br/>• Chọn kho xuất lắp ráp<br/>• Hiển thị thông tin kho dạng HTML (mã, tên)<br/>• Tự động cập nhật WarehouseId vào Entity<br/><br/><b>Ràng buộc:</b><br/>• <b>Bắt buộc chọn</b> (có dấu * đỏ)<br/>• Không được để trống<br/>• Chỉ hiển thị các chi nhánh đang hoạt động (IsActive = true)<br/><br/><b>Data Source:</b><br/>• Load từ CompanyBranchBll.GetAll()<br/>• Filter chỉ lấy các chi nhánh đang hoạt động<br/>• Sắp xếp theo tên chi nhánh<br/><br/><b>Validation:</b><br/>• Kiểm tra rỗng khi validating<br/>• Hiển thị lỗi qua ErrorProvider nếu không hợp lệ<br/><br/><color=Gray>Lưu ý:</color> Kho xuất lắp ráp sẽ được lưu vào database khi lưu phiếu xuất lắp ráp."
                );
            }
        }

        /// <summary>
        /// Thiết lập SuperToolTip cho các MemoEdit controls
        /// </summary>
        private void SetupMemoEditSuperTips()
        {
            // SuperTip cho Ghi chú
            if (NotesTextEdit != null)
            {
                SuperToolTipHelper.SetBaseEditSuperTip(
                    NotesTextEdit,
                    title: @"<b><color=DarkBlue>📝 Ghi chú</color></b>",
                    content:
                    @"Nhập ghi chú hoặc mô tả bổ sung cho phiếu xuất lắp ráp.<br/><br/><b>Chức năng:</b><br/>• Lưu thông tin bổ sung về phiếu xuất lắp ráp<br/>• Ghi chú về lý do xuất lắp ráp, điều kiện chuyển kho, đích đến hàng hóa, v.v.<br/>• Hỗ trợ nhiều dòng văn bản<br/><br/><b>Ràng buộc:</b><br/>• Không bắt buộc (có thể để trống)<br/>• Tối đa 500 ký tự<br/><br/><color=Gray>Lưu ý:</color> Ghi chú sẽ được lưu vào database khi lưu phiếu xuất lắp ráp."
                );
            }
        }

        #endregion

        #region ========== DATA LOADING ==========

        /// <summary>
        /// Load dữ liệu lookup (Warehouse)
        /// Method này được gọi từ form khi FormLoad
        /// </summary>
        public async Task LoadLookupDataAsync()
        {
            try
            {
                // Load warehouse datasource (xuất lắp ráp không cần customer)
                await LoadWarehouseDataSourceAsync();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi tải dữ liệu lookup");
            }
        }

        /// <summary>
        /// Load datasource cho Warehouse (CompanyBranch) - Load toàn bộ danh sách
        /// </summary>
        private async Task LoadWarehouseDataSourceAsync()
        {
            try
            {
                // Load danh sách CompanyBranchDto từ CompanyBranchBll (dùng làm Warehouse)
                companyBranchDtoBindingSource.DataSource = await Task.Run(() => _companyBranchBll.GetAll());
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi tải dữ liệu kho");
                throw;
            }
        }

        #endregion

        #region ========== EVENT HANDLERS ==========

        private void UcXuatLapRapMasterDto_Load(object sender, EventArgs e)
        {
            // Control đã được load
            StockOutDateDateEdit.EditValue = DateTime.Now;
        }

        private void StockOutDateDateEdit_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (StockOutDateDateEdit.EditValue is not DateTime selectedDate) return;

                // Tạo số phiếu xuất tự động
                GenerateStockOutNumber(selectedDate);

                // Xóa lỗi validation nếu có
                dxErrorProvider1.SetError(StockOutDateDateEdit, string.Empty);
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi tạo số phiếu xuất");
            }
        }

        private void WarehouseNameSearchLookupEdit_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (WarehouseNameSearchLookupEdit.EditValue is Guid warehouseId && warehouseId != Guid.Empty)
                {
                    // Cập nhật WarehouseId vào _selectedWarehouseId
                    _selectedWarehouseId = warehouseId;

                    // Xóa lỗi validation nếu có
                    dxErrorProvider1.SetError(WarehouseNameSearchLookupEdit, string.Empty);
                }
                else
                {
                    _selectedWarehouseId = Guid.Empty;
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi xử lý thay đổi kho");
            }
        }

        #endregion

        #region ========== VALIDATION ==========

        /// <summary>
        /// Validate dữ liệu input và hiển thị lỗi bằng dxErrorProvider
        /// </summary>
        private bool ValidateInput()
        {
            try
            {
                dxErrorProvider1.ClearErrors();

                //Ngày tháng không được để trống
                if (StockOutDateDateEdit.EditValue is null)
                {
                    // Hiển thị lỗi
                    dxErrorProvider1.SetError(StockOutDateDateEdit, "Ngày xuất không được để trống");
                    return false;
                }

                // Số phiếu xuất kho không được để trống
                if (string.IsNullOrWhiteSpace(StockOutNumberTextEdit.Text))
                {
                    dxErrorProvider1.SetError(StockOutNumberTextEdit, "Số phiếu xuất không được để trống");
                    return false;
                }

                // Kiểm tra độ dài số phiếu xuất kho (tối đa 50 ký tự)
                if (StockOutNumberTextEdit.Text.Length > 50)
                {
                    dxErrorProvider1.SetError(StockOutNumberTextEdit, "Số phiếu xuất không được vượt quá 50 ký tự");
                    return false;
                }

                // Kho xuất không được để trống
                var warehouseId = _selectedWarehouseId != Guid.Empty
                    ? _selectedWarehouseId
                    : (WarehouseNameSearchLookupEdit.EditValue is Guid wId ? wId : Guid.Empty);

                if (warehouseId == Guid.Empty)
                {
                    dxErrorProvider1.SetError(WarehouseNameSearchLookupEdit, "Kho xuất không được để trống");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi validate dữ liệu");
                return false;
            }
        }

        #endregion

        #region ========== PUBLIC METHODS ==========

        /// <summary>
        /// Lấy DTO từ các control sau khi validate các trường bắt buộc
        /// </summary>
        /// <returns>StockInOutMasterForUIDto nếu validation thành công, null nếu có lỗi</returns>
        public StockInOutMasterForUIDto GetDto()
        {
            try
            {
                // Validate các trường bắt buộc
                if (!ValidateInput())
                {
                    return null; // Validation thất bại
                }

                // Khai báo DTO và gán các giá trị
                var dto = new StockInOutMasterForUIDto
                {
                    // Thông tin cơ bản
                    Id = _stockInOutMasterId,
                    VoucherNumber = StockOutNumberTextEdit.Text?.Trim() ?? string.Empty,
                    StockOutDate = StockOutDateDateEdit.EditValue is DateTime date ? date : DateTime.Now,
                    LoaiNhapXuatKho = LoaiNhapXuatKhoEnum.XuatLinhKienLapRap,
                    TrangThai = TrangThaiPhieuNhapEnum.TaoMoi, // Mặc định là Tạo mới khi tạo mới

                    // Thông tin bổ sung
                    Notes = NotesTextEdit.Text?.Trim() ?? string.Empty,
                    NguoiNhanHang = NguoiNhanHangTextEdit.Text?.Trim() ?? string.Empty,
                    NguoiGiaoHang = NguoiGiaoHangTextEdit.Text?.Trim() ?? string.Empty
                };

                // Lấy thông tin Warehouse từ selected item trong SearchLookUpEdit
                var warehouseId = _selectedWarehouseId != Guid.Empty
                    ? _selectedWarehouseId
                    : (WarehouseNameSearchLookupEdit.EditValue is Guid wId ? wId : Guid.Empty);

                if (warehouseId != Guid.Empty)
                {
                    dto.WarehouseId = warehouseId;

                    // Lấy thông tin chi tiết từ selected row hoặc binding source
                    var warehouse =
                        WarehouseNameSearchLookupEdit.GetSelectedDataRow() as DTO.MasterData.Company.CompanyBranchDto;
                    if (warehouse == null &&
                        companyBranchDtoBindingSource.DataSource is System.Collections.IList warehouseList)
                    {
                        warehouse = warehouseList.Cast<DTO.MasterData.Company.CompanyBranchDto>()
                            .FirstOrDefault(w => w.Id == warehouseId);
                    }

                    if (warehouse != null)
                    {
                        dto.WarehouseCode = warehouse.BranchCode ?? string.Empty;
                        dto.WarehouseName = warehouse.BranchName ?? string.Empty;
                    }
                }

                // Khởi tạo tổng hợp với giá trị 0 (sẽ được cập nhật từ detail sau)
                dto.SetTotals(0, 0, 0, 0);

                return dto;
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi lấy dữ liệu");
                return null;
            }
        }

        /// <summary>
        /// Load dữ liệu master từ ID phiếu nhập xuất kho
        /// </summary>
        /// <param name="stockInOutMasterId">ID phiếu nhập xuất kho</param>
        public async Task LoadDataAsync(Guid stockInOutMasterId)
        {
            try
            {
                _stockInOutMasterId = stockInOutMasterId;

                // Lấy master DTO từ BLL
                var masterDto = _stockInOutBll.GetStockInOutMasterForUIDtoById(stockInOutMasterId);
                if (masterDto == null)
                {
                    throw new InvalidOperationException($"Không tìm thấy phiếu xuất kho với ID: {stockInOutMasterId}");
                }

                // Set dữ liệu cho các control đơn giản (không cần datasource)
                StockOutDateDateEdit.EditValue = masterDto.StockOutDate;
                StockOutNumberTextEdit.EditValue = masterDto.VoucherNumber;

                NotesTextEdit.EditValue = masterDto.Notes;
                NguoiNhanHangTextEdit.EditValue = masterDto.NguoiNhanHang;
                NguoiGiaoHangTextEdit.EditValue = masterDto.NguoiGiaoHang;

                // Load datasource cho Warehouse trước khi set EditValue
                await LoadWarehouseDataSourceAsync();
                WarehouseNameSearchLookupEdit.EditValue = masterDto.WarehouseId;
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi tải dữ liệu phiếu xuất kho");
                throw;
            }
        }

        /// <summary>
        /// Clear dữ liệu và reset về trạng thái ban đầu
        /// </summary>
        public void ClearData()
        {
            try
            {
                // Reset tất cả các controls về giá trị mặc định
                // Reset SearchLookUpEdit - phải set EditValue = null để xóa selection
                if (WarehouseNameSearchLookupEdit != null)
                {
                    WarehouseNameSearchLookupEdit.EditValue = null;
                }

                // Reset TextEdit
                if (StockOutNumberTextEdit != null)
                {
                    StockOutNumberTextEdit.Text = string.Empty;
                }

                // Reset DateEdit
                if (StockOutDateDateEdit != null)
                {
                    StockOutDateDateEdit.EditValue = DateTime.Now;
                    // Tạo lại số phiếu xuất kho sau khi reset ngày
                    GenerateStockOutNumber(DateTime.Now);
                }

                // Reset MemoEdit
                if (NotesTextEdit != null)
                {
                    NotesTextEdit.Text = string.Empty;
                }

                // Reset NguoiNhanHangTextEdit và NguoiGiaoHangTextEdit
                if (NguoiNhanHangTextEdit != null)
                {
                    NguoiNhanHangTextEdit.Text = string.Empty;
                }

                if (NguoiGiaoHangTextEdit != null)
                {
                    NguoiGiaoHangTextEdit.Text = string.Empty;
                }

                // Clear errors
                dxErrorProvider1.ClearErrors();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi xóa dữ liệu");
            }
        }

        /// <summary>
        /// Cập nhật các giá trị tổng hợp từ detail
        /// </summary>
        /// <param name="totalQuantity">Tổng số lượng</param>
        public void UpdateTotals(decimal totalQuantity)
        {
            try
            {
                // Cập nhật trực tiếp vào các SimpleLabelItem để hiển thị
                UpdateTotalQuantityLabel(totalQuantity);
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi cập nhật tổng hợp");
            }
        }

        /// <summary>
        /// Cập nhật label tổng số lượng
        /// </summary>
        private void UpdateTotalQuantityLabel(decimal value)
        {
            if (TotalQuantitySimpleLabelItem != null)
            {
                TotalQuantitySimpleLabelItem.Text = FormatQuantity(value);
            }
        }

        /// <summary>
        /// Format số lượng (có 2 chữ số thập phân)
        /// </summary>
        private string FormatQuantity(decimal value)
        {
            return value.ToString(ApplicationConstants.QUANTITY_FORMAT);
        }


        #endregion

        #region ========== HELPER METHODS ==========

        /// <summary>
        /// Tạo số phiếu xuất kho tự động
        /// Sử dụng BLL để tự động xác định PNK hay PXK dựa trên LoaiNhapXuatKhoEnum
        /// </summary>
        /// <param name="stockOutDate">Ngày xuất kho</param>
        private void GenerateStockOutNumber(DateTime stockOutDate)
        {
            try
            {
                // Lấy loại nhập/xuất kho
                var loaiNhapXuatKho = LoaiNhapXuatKhoEnum.XuatLinhKienLapRap;

                // Gọi BLL để tạo số phiếu tự động (tự động xác định PNK hay PXK)
                var voucherNumber = _stockInOutMasterBll.GenerateVoucherNumber(stockOutDate, loaiNhapXuatKho);

                if (StockOutNumberTextEdit != null)
                {
                    StockOutNumberTextEdit.Text = voucherNumber;
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi tạo số phiếu xuất");
            }
        }

        /// <summary>
        /// Hiển thị lỗi từ Exception
        /// </summary>
        private void ShowError(Exception ex, string message)
        {
            try
            {
                // Tìm parent form để làm owner cho MsgBox
                var parentForm = this.FindForm();

                // Sử dụng MsgBox.ShowException hoặc ShowError với thông báo chi tiết
                if (ex != null)
                {
                    MsgBox.ShowException(ex, message, parentForm);
                }
                else
                {
                    MsgBox.ShowError(message, "Lỗi", parentForm);
                }
            }
            catch
            {
                // Fallback nếu có lỗi khi hiển thị MsgBox
                System.Diagnostics.Debug.WriteLine($"Lỗi: {message}: {ex?.Message}");
            }
        }

        #endregion
    }
}