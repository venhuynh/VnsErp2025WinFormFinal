using Bll.Inventory.InventoryManagement;
using Bll.Inventory.StockInOut;
using Bll.MasterData.CompanyBll;
using Common;
using Common.Utils;
using DTO.Inventory;
using System;
using System.Linq;
using System.Threading.Tasks;
using DTO.Inventory.InventoryManagement;

namespace Inventory.StockOut.XuatLuuChuyenKho;

public partial class UcXuatChuyenKhoMaster : DevExpress.XtraEditors.XtraUserControl
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

    private Guid _selectedWarehouseOutId = Guid.Empty; // Kho xuất (kho nguồn)

    private Guid _selectedWarehouseInId = Guid.Empty; // Kho nhận (kho đích)

    #endregion

    #region ========== CONSTRUCTOR ==========

    public UcXuatChuyenKhoMaster()
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
            // Setup Warehouse SearchLookUpEdit cho cả 2 kho
            WarehouseStockInNameSearchLookupEdit.Properties.DataSource = companyBranchDtoBindingSource;
            WarehouseStockInNameSearchLookupEdit.Properties.ValueMember = "Id";
            WarehouseStockInNameSearchLookupEdit.Properties.DisplayMember = "ThongTinHtml";
            WarehouseStockInNameSearchLookupEdit.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            WarehouseStockInNameSearchLookupEdit.Properties.PopupView = CompanyBranchDtoSearchLookUpEdit1View;

            WarehouseStockOutNameSearchLookupEdit.Properties.DataSource = companyBranchDtoBindingSource;
            WarehouseStockOutNameSearchLookupEdit.Properties.ValueMember = "Id";
            WarehouseStockOutNameSearchLookupEdit.Properties.DisplayMember = "ThongTinHtml";
            WarehouseStockOutNameSearchLookupEdit.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            WarehouseStockOutNameSearchLookupEdit.Properties.PopupView = CompanyBranchDtoSearchLookUpEdit1View;

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
            Load += UcStockInMaster_Load;

            //Sự kiện của WarehouseNameSearchLookupEdit
            WarehouseStockInNameSearchLookupEdit.Popup += WarehouseNameSearchLookupEdit_Popup;
            WarehouseStockInNameSearchLookupEdit.EditValueChanged += WarehouseNameSearchLookupEdit_EditValueChanged;

            //Sự kiện của WarehouseStockOutNameSearchLookupEdit
            WarehouseStockOutNameSearchLookupEdit.Popup += WarehouseStockOutNameSearchLookupEdit_Popup;
            WarehouseStockOutNameSearchLookupEdit.EditValueChanged += WarehouseStockOutNameSearchLookupEdit_EditValueChanged;


            StockInDateDateEdit.EditValueChanged += StockOutDateDateEdit_EditValueChanged;
        }
        catch (Exception ex)
        {
            ShowError(ex, "Lỗi thiết lập sự kiện");
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
        // SuperTip cho Số phiếu xuất lưu chuyển kho
        if (StockInNumberTextEdit != null)
        {
            SuperToolTipHelper.SetTextEditSuperTip(
                StockInNumberTextEdit,
                title: @"<b><color=DarkBlue>📄 Số phiếu xuất lưu chuyển kho</color></b>",
                content: @"Số phiếu xuất lưu chuyển kho được tạo tự động theo format: <b>PXK-MMYY-NNXXX</b><br/><br/><b>Format:</b><br/>• PXK: Phiếu xuất kho<br/>• MM: Tháng (2 ký tự)<br/>• YY: Năm (2 ký tự cuối)<br/>• NN: Index của Loại xuất kho (2 ký tự)<br/>• XXX: Số thứ tự phiếu (3 ký tự từ 001 đến 999)<br/><br/><b>Chức năng:</b><br/>• Tự động tạo khi thay đổi ngày xuất lưu chuyển kho<br/>• Tự động tạo khi thay đổi loại xuất kho<br/>• Query database để lấy số thứ tự tiếp theo<br/>• Đảm bảo số phiếu duy nhất trong cùng tháng/năm/loại<br/><br/><b>Ràng buộc:</b><br/>• <b>Bắt buộc nhập</b> (có dấu * đỏ)<br/>• Không được để trống<br/>• Tối đa 50 ký tự<br/><br/><color=Gray>Lưu ý:</color> Số phiếu xuất lưu chuyển kho sẽ được lưu vào database khi lưu phiếu."
            );
        }


        // SuperTip cho Người nhận hàng
        if (NguoiNhanHangTextEdit != null)
        {
            SuperToolTipHelper.SetTextEditSuperTip(
                NguoiNhanHangTextEdit,
                title: @"<b><color=DarkBlue>👤 Người nhận hàng</color></b>",
                content: @"Nhập tên người nhận hàng tại kho đích (kho nhận).<br/><br/><b>Chức năng:</b><br/>• Ghi nhận thông tin người nhận hàng tại kho đích<br/>• Hỗ trợ tra cứu và theo dõi quá trình lưu chuyển kho<br/><br/><b>Ràng buộc:</b><br/>• Không bắt buộc (có thể để trống)<br/>• Tối đa 500 ký tự<br/><br/><color=Gray>Lưu ý:</color> Thông tin này sẽ được lưu vào database khi lưu phiếu lưu chuyển kho."
            );
        }

        // SuperTip cho Người giao hàng
        if (NguoiGiaoHangTextEdit != null)
        {
            SuperToolTipHelper.SetTextEditSuperTip(
                NguoiGiaoHangTextEdit,
                title: @"<b><color=DarkBlue>🚚 Người giao hàng</color></b>",
                content: @"Nhập tên người giao hàng tại kho nguồn (kho xuất).<br/><br/><b>Chức năng:</b><br/>• Ghi nhận thông tin người giao hàng tại kho nguồn<br/>• Hỗ trợ tra cứu và theo dõi quá trình lưu chuyển kho<br/><br/><b>Ràng buộc:</b><br/>• Không bắt buộc (có thể để trống)<br/>• Tối đa 500 ký tự<br/><br/><color=Gray>Lưu ý:</color> Thông tin này sẽ được lưu vào database khi lưu phiếu lưu chuyển kho."
            );
        }
    }

    /// <summary>
    /// Thiết lập SuperToolTip cho các DateEdit controls
    /// </summary>
    private void SetupDateEditSuperTips()
    {
        // SuperTip cho Ngày xuất lưu chuyển kho
        if (StockInDateDateEdit != null)
        {
            SuperToolTipHelper.SetBaseEditSuperTip(
                StockInDateDateEdit,
                title: @"<b><color=DarkBlue>📅 Ngày xuất lưu chuyển kho</color></b>",
                content: @"Chọn ngày xuất lưu chuyển kho cho phiếu.<br/><br/><b>Chức năng:</b><br/>• Xác định thời điểm xuất lưu chuyển kho<br/>• Tự động tạo số phiếu xuất lưu chuyển kho dựa trên ngày<br/>• Format số phiếu: PXK-MMYY-NNXXX (MM, YY từ ngày này)<br/>• Query database để lấy số thứ tự tiếp theo trong tháng/năm<br/><br/><b>Ràng buộc:</b><br/>• <b>Bắt buộc nhập</b> (có dấu * đỏ)<br/>• Không được để trống<br/>• Mặc định: Ngày hiện tại<br/><br/><b>Validation:</b><br/>• Kiểm tra rỗng khi validating<br/>• Hiển thị lỗi qua ErrorProvider nếu không hợp lệ<br/><br/><color=Gray>Lưu ý:</color> Khi thay đổi ngày xuất lưu chuyển kho, hệ thống sẽ tự động tạo lại số phiếu theo format mới."
            );
        }
    }

    /// <summary>
    /// Thiết lập SuperToolTip cho các SearchLookUpEdit controls
    /// </summary>
    private void SetupSearchLookupEditSuperTips()
    {
        // SuperTip cho Kho nhận (kho đích)
        if (WarehouseStockInNameSearchLookupEdit != null)
        {
            SuperToolTipHelper.SetBaseEditSuperTip(
                WarehouseStockInNameSearchLookupEdit,
                title: @"<b><color=DarkBlue>🏢 Kho nhận (Kho đích)</color></b>",
                content: @"Chọn kho nhận hàng (kho đích) từ danh sách chi nhánh (Company Branch) đang hoạt động.<br/><br/><b>Chức năng:</b><br/>• Chọn kho nhận hàng trong quá trình xuất lưu chuyển kho<br/>• Hiển thị thông tin kho dạng HTML (mã, tên)<br/>• Tự động cập nhật PartnerSiteId vào Entity (để lưu thông tin kho đích)<br/><br/><b>Ràng buộc:</b><br/>• <b>Bắt buộc chọn</b> (có dấu * đỏ)<br/>• Không được để trống<br/>• Không được trùng với kho xuất<br/>• Chỉ hiển thị các chi nhánh đang hoạt động (IsActive = true)<br/><br/><b>Data Source:</b><br/>• Load từ CompanyBranchBll.GetAll()<br/>• Filter chỉ lấy các chi nhánh đang hoạt động<br/>• Sắp xếp theo tên chi nhánh<br/><br/><b>Validation:</b><br/>• Kiểm tra rỗng khi validating<br/>• Kiểm tra không được trùng với kho xuất<br/>• Hiển thị lỗi qua ErrorProvider nếu không hợp lệ<br/><br/><color=Gray>Lưu ý:</color> Kho nhận sẽ được lưu vào PartnerSiteId trong database khi lưu phiếu xuất lưu chuyển kho."
            );
        }

        // SuperTip cho Kho xuất (kho nguồn)
        if (WarehouseStockOutNameSearchLookupEdit != null)
        {
            SuperToolTipHelper.SetBaseEditSuperTip(
                WarehouseStockOutNameSearchLookupEdit,
                title: @"<b><color=DarkBlue>📤 Kho xuất (Kho nguồn)</color></b>",
                content: @"Chọn kho xuất hàng (kho nguồn) từ danh sách chi nhánh (Company Branch) đang hoạt động.<br/><br/><b>Chức năng:</b><br/>• Chọn kho xuất hàng trong quá trình xuất lưu chuyển kho<br/>• Hiển thị thông tin kho dạng HTML (mã, tên)<br/>• Tự động cập nhật WarehouseId vào Entity (để lưu thông tin kho nguồn)<br/><br/><b>Ràng buộc:</b><br/>• <b>Bắt buộc chọn</b> (có dấu * đỏ)<br/>• Không được để trống<br/>• Không được trùng với kho nhận<br/>• Chỉ hiển thị các chi nhánh đang hoạt động (IsActive = true)<br/><br/><b>Data Source:</b><br/>• Load từ CompanyBranchBll.GetAll()<br/>• Filter chỉ lấy các chi nhánh đang hoạt động<br/>• Sắp xếp theo tên chi nhánh<br/><br/><b>Validation:</b><br/>• Kiểm tra rỗng khi validating<br/>• Kiểm tra không được trùng với kho nhận<br/>• Hiển thị lỗi qua ErrorProvider nếu không hợp lệ<br/><br/><color=Gray>Lưu ý:</color> Kho xuất sẽ được lưu vào WarehouseId trong database khi lưu phiếu xuất lưu chuyển kho."
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
                content: @"Nhập ghi chú hoặc mô tả bổ sung cho phiếu xuất lưu chuyển kho.<br/><br/><b>Chức năng:</b><br/>• Lưu thông tin bổ sung về phiếu xuất lưu chuyển kho<br/>• Ghi chú về lý do xuất lưu chuyển kho, điều kiện vận chuyển, phương tiện vận chuyển, thời gian giao nhận, v.v.<br/>• Hỗ trợ nhiều dòng văn bản<br/><br/><b>Ràng buộc:</b><br/>• Không bắt buộc (có thể để trống)<br/>• Không giới hạn độ dài<br/><br/><color=Gray>Lưu ý:</color> Ghi chú sẽ được lưu vào database khi lưu phiếu xuất lưu chuyển kho."
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
            // Load warehouse datasource
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

    private void UcStockInMaster_Load(object sender, EventArgs e)
    {
        // Control đã được load
        StockInDateDateEdit.EditValue = DateTime.Now;
    }

    private void StockOutDateDateEdit_EditValueChanged(object sender, EventArgs e)
    {
        try
        {
            if (StockInDateDateEdit.EditValue is not DateTime selectedDate) return;
            
            // Tạo số phiếu xuất tự động
            GenerateStockOutNumber(selectedDate);
                    
            // Xóa lỗi validation nếu có
            dxErrorProvider1.SetError(StockInDateDateEdit, string.Empty);
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
            if (WarehouseStockInNameSearchLookupEdit.EditValue is Guid warehouseId && warehouseId != Guid.Empty)
            {
                // Kiểm tra không được trùng với kho xuất
                var warehouseOutId = _selectedWarehouseOutId != Guid.Empty 
                    ? _selectedWarehouseOutId 
                    : (WarehouseStockOutNameSearchLookupEdit.EditValue is Guid wOutId ? wOutId : Guid.Empty);
                
                if (warehouseOutId == warehouseId)
                {
                    dxErrorProvider1.SetError(WarehouseStockInNameSearchLookupEdit, "Kho nhận và kho xuất không được trùng nhau");
                    return;
                }

                // Cập nhật WarehouseInId vào _selectedWarehouseInId
                _selectedWarehouseInId = warehouseId;
                    
                // Xóa lỗi validation nếu có
                dxErrorProvider1.SetError(WarehouseStockInNameSearchLookupEdit, string.Empty);
            }
            else
            {
                _selectedWarehouseInId = Guid.Empty;
            }
        }
        catch (Exception ex)
        {
            ShowError(ex, "Lỗi xử lý thay đổi kho nhận");
        }
    }

    private void WarehouseStockOutNameSearchLookupEdit_EditValueChanged(object sender, EventArgs e)
    {
        try
        {
            if (WarehouseStockOutNameSearchLookupEdit.EditValue is Guid warehouseId && warehouseId != Guid.Empty)
            {
                // Kiểm tra không được trùng với kho nhận
                var warehouseInId = _selectedWarehouseInId != Guid.Empty 
                    ? _selectedWarehouseInId 
                    : (WarehouseStockInNameSearchLookupEdit.EditValue is Guid wInId ? wInId : Guid.Empty);
                
                if (warehouseInId == warehouseId)
                {
                    dxErrorProvider1.SetError(WarehouseStockOutNameSearchLookupEdit, "Kho xuất và kho nhận không được trùng nhau");
                    return;
                }

                // Cập nhật WarehouseOutId vào _selectedWarehouseOutId
                _selectedWarehouseOutId = warehouseId;
                    
                // Xóa lỗi validation nếu có
                dxErrorProvider1.SetError(WarehouseStockOutNameSearchLookupEdit, string.Empty);
            }
            else
            {
                _selectedWarehouseOutId = Guid.Empty;
            }
        }
        catch (Exception ex)
        {
            ShowError(ex, "Lỗi xử lý thay đổi kho xuất");
        }
    }

    private async void WarehouseStockOutNameSearchLookupEdit_Popup(object sender, EventArgs e)
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
            if (StockInDateDateEdit.EditValue is null)
            {
                // Hiển thị lỗi
                dxErrorProvider1.SetError(StockInDateDateEdit, "Ngày xuất không được để trống");
                return false;
            }

            // Số phiếu xuất kho không được để trống
            if (string.IsNullOrWhiteSpace(StockInNumberTextEdit.Text))
            {
                dxErrorProvider1.SetError(StockInNumberTextEdit, "Số phiếu xuất không được để trống");
                return false;
            }

            // Kiểm tra độ dài số phiếu xuất kho (tối đa 50 ký tự)
            if (StockInNumberTextEdit.Text.Length > 50)
            {
                dxErrorProvider1.SetError(StockInNumberTextEdit, "Số phiếu xuất không được vượt quá 50 ký tự");
                return false;
            }

            // Kho xuất không được để trống
            var warehouseOutId = _selectedWarehouseOutId != Guid.Empty 
                ? _selectedWarehouseOutId 
                : (WarehouseStockOutNameSearchLookupEdit.EditValue is Guid wOutId ? wOutId : Guid.Empty);
            
            if (warehouseOutId == Guid.Empty)
            {
                dxErrorProvider1.SetError(WarehouseStockOutNameSearchLookupEdit, "Kho xuất không được để trống");
                return false;
            }

            // Kho nhận không được để trống
            var warehouseInId = _selectedWarehouseInId != Guid.Empty 
                ? _selectedWarehouseInId 
                : (WarehouseStockInNameSearchLookupEdit.EditValue is Guid wInId ? wInId : Guid.Empty);
            
            if (warehouseInId == Guid.Empty)
            {
                dxErrorProvider1.SetError(WarehouseStockInNameSearchLookupEdit, "Kho nhận không được để trống");
                return false;
            }

            // Kiểm tra kho xuất và kho nhận không được trùng nhau
            if (warehouseOutId == warehouseInId)
            {
                dxErrorProvider1.SetError(WarehouseStockOutNameSearchLookupEdit, "Kho xuất và kho nhận không được trùng nhau");
                dxErrorProvider1.SetError(WarehouseStockInNameSearchLookupEdit, "Kho xuất và kho nhận không được trùng nhau");
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
                VoucherNumber = StockInNumberTextEdit.Text?.Trim() ?? string.Empty,
                StockOutDate = StockInDateDateEdit.EditValue is DateTime date ? date : DateTime.Now,
                LoaiNhapXuatKho = LoaiNhapXuatKhoEnum.XuatHangThuongMai,
                TrangThai = TrangThaiPhieuNhapEnum.TaoMoi, // Mặc định là Tạo mới khi tạo mới

                // Thông tin bổ sung
                Notes = NotesTextEdit.Text?.Trim() ?? string.Empty,
                NguoiNhanHang = NguoiNhanHangTextEdit.Text?.Trim() ?? string.Empty,
                NguoiGiaoHang = NguoiGiaoHangTextEdit.Text?.Trim() ?? string.Empty
            };

            // Lấy thông tin Warehouse từ selected item trong SearchLookUpEdit
            var warehouseId = _selectedWarehouseInId != Guid.Empty
                ? _selectedWarehouseInId
                : (WarehouseStockInNameSearchLookupEdit.EditValue is Guid wId ? wId : Guid.Empty);

            if (warehouseId != Guid.Empty)
            {
                dto.WarehouseId = warehouseId;

            }

            // Lấy thông tin Customer từ selected item trong SearchLookUpEdit
            var customerId = _selectedWarehouseOutId != Guid.Empty
                ? _selectedWarehouseOutId
                : (WarehouseStockOutNameSearchLookupEdit.EditValue is Guid cId ? cId : Guid.Empty);

            if (customerId != Guid.Empty)
            {
                dto.CustomerId = customerId;
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
            StockInDateDateEdit.EditValue = masterDto.StockOutDate;
            StockInNumberTextEdit.EditValue = masterDto.VoucherNumber;
            
            NotesTextEdit.EditValue = masterDto.Notes;
            NguoiNhanHangTextEdit.EditValue = masterDto.NguoiNhanHang;
            NguoiGiaoHangTextEdit.EditValue = masterDto.NguoiGiaoHang;

            // Load datasource cho Warehouse trước khi set EditValue
            await LoadWarehouseDataSourceAsync();
            
            // Set kho xuất (WarehouseId)
            WarehouseStockOutNameSearchLookupEdit.EditValue = masterDto.WarehouseId;

            // Set kho nhận (CustomerId - lưu kho đích)
            if (masterDto.CustomerId.HasValue)
            {
                WarehouseStockInNameSearchLookupEdit.EditValue = masterDto.CustomerId;
            }
            else
            {
                WarehouseStockInNameSearchLookupEdit.EditValue = null;
            }
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
            if (WarehouseStockOutNameSearchLookupEdit != null)
            {
                WarehouseStockOutNameSearchLookupEdit.EditValue = null;
            }

            if (WarehouseStockInNameSearchLookupEdit != null)
            {
                WarehouseStockInNameSearchLookupEdit.EditValue = null;
            }

            // Reset TextEdit
            if (StockInNumberTextEdit != null)
            {
                StockInNumberTextEdit.Text = string.Empty;
            }

            // Reset DateEdit
            if (StockInDateDateEdit != null)
            {
                StockInDateDateEdit.EditValue = DateTime.Now;
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
    /// <param name="totalAmount">Tổng tiền chưa VAT</param>
    /// <param name="totalVat">Tổng VAT</param>
    /// <param name="totalAmountIncludedVat">Tổng tiền bao gồm VAT</param>
    public void UpdateTotals(decimal totalQuantity, decimal totalAmount, decimal totalVat, decimal totalAmountIncludedVat)
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
            var loaiNhapXuatKho = LoaiNhapXuatKhoEnum.XuatLuuChuyenKho;

            // Gọi BLL để tạo số phiếu tự động (tự động xác định PNK hay PXK)
            var voucherNumber = _stockInOutMasterBll.GenerateVoucherNumber(stockOutDate, loaiNhapXuatKho);

            if (StockInNumberTextEdit != null)
            {
                StockInNumberTextEdit.Text = voucherNumber;
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