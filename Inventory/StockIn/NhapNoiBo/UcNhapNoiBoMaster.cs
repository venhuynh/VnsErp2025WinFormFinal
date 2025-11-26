using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bll.Inventory.StockIn;
using Bll.MasterData.CompanyBll;
using Bll.MasterData.CustomerBll;
using Common;
using Common.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using DTO.Inventory.StockIn;
using DTO.Inventory.StockIn.NhapNoiBo;
using DTO.MasterData.Company;
using DTO.MasterData.CustomerPartner;

namespace Inventory.StockIn.NhapNoiBo;

public partial class UcNhapNoiBoMaster : XtraUserControl
{
    #region ========== KHAI BÁO BIẾN ==========


    /// <summary>
    /// Business Logic Layer cho CompanyBranch (dùng cho Warehouse lookup)
    /// </summary>
    private readonly CompanyBranchBll _companyBranchBll = new();

    /// <summary>
    /// Business Logic Layer cho BusinessPartnerSite (dùng cho Supplier lookup)
    /// </summary>
    private readonly BusinessPartnerSiteBll _businessPartnerSiteBll = new();

    /// <summary>
    /// Business Logic Layer cho StockIn (dùng để lấy số thứ tự tiếp theo)
    /// </summary>
    private readonly StockInBll _stockInBll = new();

    /// <summary>
    /// Flag đánh dấu Warehouse datasource đã được load chưa
    /// </summary>
    private bool _isWarehouseDataSourceLoaded;

    /// <summary>
    /// Flag đánh dấu Supplier datasource đã được load chưa
    /// </summary>
    private bool _isSupplierDataSourceLoaded;

    private Guid _stockInOutMasterId = Guid.Empty;

    /// <summary>
    /// DTO cho phiếu nhập nội bộ
    /// </summary>
    private NhapNoiBoMasterDto _stockInMasterDto;

    #endregion

    #region ========== CONSTRUCTOR ==========

    public UcNhapNoiBoMaster()
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
            // Khởi tạo DTO
            InitializeDto();


            // Setup SearchLookUpEdit cho Warehouse
            //SetupLookupEdits();

            // Đánh dấu các trường bắt buộc
            MarkRequiredFields();

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
    /// Khởi tạo DTO
    /// </summary>
    private void InitializeDto()
    {
        _stockInMasterDto = new NhapNoiBoMasterDto
        {
            Id = Guid.Empty,
            StockInNumber = null,
            StockInDate = DateTime.Now,
            LoaiNhapXuatKho = LoaiNhapXuatKhoEnum.NhapNoiBo,
            TrangThai = TrangThaiPhieuNhapEnum.TaoMoi,
            WarehouseId = Guid.Empty,
            WarehouseCode = null,
            WarehouseName = null,
            Notes = null,
            NguoiNhanHang = null,
            NguoiGiaoHang = null
        };

        // Khởi tạo các giá trị tổng hợp bằng method SetTotals() vì các property giờ là computed (read-only)
        _stockInMasterDto.SetTotals(0);
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
    /// Đánh dấu các trường bắt buộc
    /// </summary>
    private void MarkRequiredFields()
    {
        try
        {
            RequiredFieldHelper.MarkRequiredFields(
                this,
                typeof(NhapNoiBoMasterDto),
                logger: (msg, ex) => System.Diagnostics.Debug.WriteLine($"{msg}: {ex?.Message}")
            );

            // Xử lý đặc biệt cho WarehouseId (control là WarehouseNameSearchLookupEdit)
            // Vì RequiredFieldHelper không thể tự động match WarehouseId với WarehouseNameSearchLookupEdit
            if (ItemForWarehouseName != null && !ItemForWarehouseName.Text.Contains("*"))
            {
                ItemForWarehouseName.AllowHtmlStringInCaption = true;
                var baseCaption = string.IsNullOrWhiteSpace(ItemForWarehouseName.Text) ? "Kho nhập" : ItemForWarehouseName.Text;
                ItemForWarehouseName.Text = baseCaption + @" <color=red>*</color>";
            }
        }
        catch (Exception ex)
        {
            ShowError(ex, "Lỗi đánh dấu trường bắt buộc");
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
            WarehouseNameSearchLookupEdit.Popup += WarehouseNameSearchLookupEdit_Popup;
            WarehouseNameSearchLookupEdit.EditValueChanged += WarehouseNameSearchLookupEdit_EditValueChanged;


            StockInDateDateEdit.EditValueChanged += StockInDateDateEdit_EditValueChanged;

            StockInNumberTextEdit.EditValueChanged += StockInNumberTextEdit_EditValueChanged;

            // Sự kiện của NguoiNhanHangTextEdit và NguoiGiaoHangTextEdit
            NguoiNhanHangTextEdit.EditValueChanged += NguoiNhanHangTextEdit_EditValueChanged;
            NguoiGiaoHangTextEdit.EditValueChanged += NguoiGiaoHangTextEdit_EditValueChanged;
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
            // Chỉ load nếu chưa load hoặc datasource rỗng
            if (!_isWarehouseDataSourceLoaded ||
                companyBranchDtoBindingSource.DataSource == null ||
                (companyBranchDtoBindingSource.DataSource is List<CompanyBranchDto> list && list.Count == 0))
            {
                await LoadWarehouseDataSourceAsync();
                _isWarehouseDataSourceLoaded = true;
            }
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
        // SuperTip cho Số phiếu nhập kho
        if (StockInNumberTextEdit != null)
        {
            SuperToolTipHelper.SetTextEditSuperTip(
                StockInNumberTextEdit,
                title: @"<b><color=DarkBlue>📄 Số phiếu nhập kho</color></b>",
                content: @"Số phiếu nhập kho được tạo tự động theo format: <b>PNK-MMYY-NNXXX</b><br/><br/><b>Format:</b><br/>• PNK: Phiếu nhập kho<br/>• MM: Tháng (2 ký tự)<br/>• YY: Năm (2 ký tự cuối)<br/>• NN: Index của Loại nhập kho (2 ký tự)<br/>• XXX: Số thứ tự phiếu (3 ký tự từ 001 đến 999)<br/><br/><b>Chức năng:</b><br/>• Tự động tạo khi thay đổi ngày nhập kho<br/>• Tự động tạo khi thay đổi loại nhập kho<br/>• Query database để lấy số thứ tự tiếp theo<br/>• Đảm bảo số phiếu duy nhất trong cùng tháng/năm/loại<br/><br/><b>Ràng buộc:</b><br/>• <b>Bắt buộc nhập</b> (có dấu * đỏ)<br/>• Không được để trống<br/>• Tối đa 50 ký tự<br/><br/><color=Gray>Lưu ý:</color> Số phiếu nhập kho sẽ được lưu vào database khi lưu phiếu nhập."
            );
        }


        // SuperTip cho Người nhận hàng
        if (NguoiNhanHangTextEdit != null)
        {
            SuperToolTipHelper.SetTextEditSuperTip(
                NguoiNhanHangTextEdit,
                title: @"<b><color=DarkBlue>👤 Người nhận hàng</color></b>",
                content: @"Nhập tên người nhận hàng tại kho.<br/><br/><b>Chức năng:</b><br/>• Ghi nhận thông tin người nhận hàng<br/>• Hỗ trợ tra cứu và theo dõi<br/><br/><b>Ràng buộc:</b><br/>• Không bắt buộc (có thể để trống)<br/>• Tối đa 500 ký tự<br/><br/><color=Gray>Lưu ý:</color> Thông tin này sẽ được lưu vào database khi lưu phiếu nhập kho."
            );
        }

        // SuperTip cho Người giao hàng
        if (NguoiGiaoHangTextEdit != null)
        {
            SuperToolTipHelper.SetTextEditSuperTip(
                NguoiGiaoHangTextEdit,
                title: @"<b><color=DarkBlue>🚚 Người giao hàng</color></b>",
                content: @"Nhập tên người giao hàng từ nhà cung cấp.<br/><br/><b>Chức năng:</b><br/>• Ghi nhận thông tin người giao hàng<br/>• Hỗ trợ tra cứu và theo dõi<br/><br/><b>Ràng buộc:</b><br/>• Không bắt buộc (có thể để trống)<br/>• Tối đa 500 ký tự<br/><br/><color=Gray>Lưu ý:</color> Thông tin này sẽ được lưu vào database khi lưu phiếu nhập kho."
            );
        }
    }

    /// <summary>
    /// Thiết lập SuperToolTip cho các DateEdit controls
    /// </summary>
    private void SetupDateEditSuperTips()
    {
        // SuperTip cho Ngày nhập kho
        if (StockInDateDateEdit != null)
        {
            SuperToolTipHelper.SetBaseEditSuperTip(
                StockInDateDateEdit,
                title: @"<b><color=DarkBlue>📅 Ngày nhập kho</color></b>",
                content: @"Chọn ngày nhập kho cho phiếu nhập.<br/><br/><b>Chức năng:</b><br/>• Xác định thời điểm nhập kho<br/>• Tự động tạo số phiếu nhập kho dựa trên ngày<br/>• Format số phiếu: PNK-MMYY-NNXXX (MM, YY từ ngày này)<br/>• Query database để lấy số thứ tự tiếp theo trong tháng/năm<br/><br/><b>Ràng buộc:</b><br/>• <b>Bắt buộc nhập</b> (có dấu * đỏ)<br/>• Không được để trống<br/>• Mặc định: Ngày hiện tại<br/><br/><b>Validation:</b><br/>• Kiểm tra rỗng khi validating<br/>• Hiển thị lỗi qua ErrorProvider nếu không hợp lệ<br/><br/><color=Gray>Lưu ý:</color> Khi thay đổi ngày nhập kho, hệ thống sẽ tự động tạo lại số phiếu nhập kho theo format mới."
            );
        }
    }

    /// <summary>
    /// Thiết lập SuperToolTip cho các SearchLookUpEdit controls
    /// </summary>
    private void SetupSearchLookupEditSuperTips()
    {
        // SuperTip cho Kho nhập
        if (WarehouseNameSearchLookupEdit != null)
        {
            SuperToolTipHelper.SetBaseEditSuperTip(
                WarehouseNameSearchLookupEdit,
                title: @"<b><color=DarkBlue>🏢 Kho nhập</color></b>",
                content: @"Chọn kho nhập hàng từ danh sách chi nhánh (Company Branch) đang hoạt động.<br/><br/><b>Chức năng:</b><br/>• Chọn kho nhập hàng<br/>• Hiển thị thông tin kho dạng HTML (mã, tên)<br/>• Tự động cập nhật WarehouseId, WarehouseCode, WarehouseName vào DTO<br/><br/><b>Ràng buộc:</b><br/>• <b>Bắt buộc chọn</b> (có dấu * đỏ)<br/>• Không được để trống<br/>• Chỉ hiển thị các chi nhánh đang hoạt động (IsActive = true)<br/><br/><b>Data Source:</b><br/>• Load từ CompanyBranchBll.GetAll()<br/>• Filter chỉ lấy các chi nhánh đang hoạt động<br/>• Sắp xếp theo tên chi nhánh<br/><br/><b>Validation:</b><br/>• Kiểm tra rỗng khi validating<br/>• Hiển thị lỗi qua ErrorProvider nếu không hợp lệ<br/><br/><color=Gray>Lưu ý:</color> Kho nhập sẽ được lưu vào database khi lưu phiếu nhập kho."
            );
        }

        // SuperTip cho Nhà cung cấp - Đã xóa vì nhập nội bộ không cần nhà cung cấp
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
                content: @"Nhập ghi chú hoặc mô tả bổ sung cho phiếu nhập kho.<br/><br/><b>Chức năng:</b><br/>• Lưu thông tin bổ sung về phiếu nhập kho<br/>• Ghi chú về lý do nhập kho, điều kiện nhập hàng, v.v.<br/>• Hỗ trợ nhiều dòng văn bản<br/><br/><b>Ràng buộc:</b><br/>• Không bắt buộc (có thể để trống)<br/>• Không giới hạn độ dài<br/><br/><color=Gray>Lưu ý:</color> Ghi chú sẽ được lưu vào database khi lưu phiếu nhập kho."
            );
        }
    }

    #endregion

    #region ========== DATA LOADING ==========

    /// <summary>
    /// Load dữ liệu lookup (Warehouse và Supplier)
    /// Method này được gọi từ form khi FormLoad
    /// </summary>
    public async Task LoadLookupDataAsync()
    {
        try
        {
            // Reset flags để đảm bảo load lại khi form load
            _isWarehouseDataSourceLoaded = false;
            _isSupplierDataSourceLoaded = false;

            // Load warehouse datasource (nhập nội bộ không cần supplier)
            await LoadWarehouseDataSourceAsync(forceRefresh: true);
        }
        catch (Exception ex)
        {
            ShowError(ex, "Lỗi tải dữ liệu lookup");
        }
    }

    /// <summary>
    /// Load datasource cho Warehouse (CompanyBranch) - Load toàn bộ danh sách
    /// </summary>
    /// <param name="forceRefresh">Nếu true, sẽ load lại từ database ngay cả khi đã load trước đó</param>
    private async Task LoadWarehouseDataSourceAsync(bool forceRefresh = false)
    {
        try
        {
            // Nếu đã load và không force refresh, không load lại
            if (_isWarehouseDataSourceLoaded && !forceRefresh &&
                companyBranchDtoBindingSource.DataSource != null &&
                companyBranchDtoBindingSource.DataSource is List<CompanyBranchDto> existingList &&
                existingList.Count > 0)
            {
                return;
            }

            // Load danh sách CompanyBranchDto từ CompanyBranchBll (dùng làm Warehouse)
            var branches = await Task.Run(() => _companyBranchBll.GetAll());
            var warehouseDtos = branches
                .Where(b => b.IsActive) // Chỉ lấy các chi nhánh đang hoạt động
                .Select(b => b.ToDto())
                .OrderBy(b => b.BranchName)
                .ToList();

            companyBranchDtoBindingSource.DataSource = warehouseDtos;
            _isWarehouseDataSourceLoaded = true;
        }
        catch (Exception ex)
        {
            ShowError(ex, "Lỗi tải dữ liệu kho");
            throw;
        }
    }

    /// <summary>
    /// Load datasource cho Supplier (BusinessPartnerSite) - Đã xóa vì nhập nội bộ không cần nhà cung cấp
    /// </summary>
    /// <param name="forceRefresh">Nếu true, sẽ load lại từ database ngay cả khi đã load trước đó</param>
    private async Task LoadSupplierDataSourceAsync(bool forceRefresh = false)
    {
        // Nhập nội bộ không cần supplier, method này giữ lại để tương thích nhưng không làm gì
        await Task.CompletedTask;
    }

    /// <summary>
    /// Load single Warehouse record theo ID và set vào datasource
    /// Chỉ load đúng 1 record để tối ưu performance
    /// </summary>
    /// <param name="warehouseId">ID của Warehouse (CompanyBranch)</param>
    private async Task LoadSingleWarehouseByIdAsync(Guid warehouseId)
    {
        try
        {
            if (warehouseId == Guid.Empty)
            {
                // Nếu ID rỗng, set datasource rỗng
                companyBranchDtoBindingSource.DataSource = new List<CompanyBranchDto>();
                // Không đánh dấu đã load vì datasource rỗng
                _isWarehouseDataSourceLoaded = false;
                return;
            }

            // Load chỉ 1 record theo ID
            var branch = await Task.Run(() => _companyBranchBll.GetById(warehouseId));
            if (branch != null)
            {
                var warehouseDto = branch.ToDto();
                // Set datasource chỉ chứa 1 record
                companyBranchDtoBindingSource.DataSource = new List<CompanyBranchDto> { warehouseDto };
                // Đánh dấu chưa load full list (khi popup sẽ load full)
                _isWarehouseDataSourceLoaded = false;
            }
            else
            {
                // Nếu không tìm thấy, set datasource rỗng
                companyBranchDtoBindingSource.DataSource = new List<CompanyBranchDto>();
                _isWarehouseDataSourceLoaded = false;
            }
        }
        catch (Exception ex)
        {
            ShowError(ex, "Lỗi tải dữ liệu kho");
            throw;
        }
    }

    /// <summary>
    /// Load single Supplier record theo ID - Đã xóa vì nhập nội bộ không cần nhà cung cấp
    /// </summary>
    /// <param name="supplierId">ID của Supplier (BusinessPartnerSite)</param>
    private async Task LoadSingleSupplierByIdAsync(Guid? supplierId)
    {
        // Nhập nội bộ không cần supplier, method này giữ lại để tương thích nhưng không làm gì
        await Task.CompletedTask;
    }

    /// <summary>
    /// Load dữ liệu từ DTO vào controls
    /// </summary>
    private void LoadData(NhapNoiBoMasterDto dto)
    {
        try
        {
            if (dto == null)
            {
                InitializeDto();
                return;
            }

            _stockInMasterDto = dto;

            // Refresh tất cả bindings
            RefreshAllBindings();

        }
        catch (Exception ex)
        {
            ShowError(ex, "Lỗi load dữ liệu");
        }
    }

    /// <summary>
    /// Refresh tất cả data bindings
    /// </summary>
    private void RefreshAllBindings()
    {
        var controls = new Control[]
        {
            StockInNumberTextEdit,
            StockInDateDateEdit,
            NotesTextEdit,
            NguoiNhanHangTextEdit,
            NguoiGiaoHangTextEdit
        };

        foreach (var control in controls)
        {
            if (control != null)
            {
                foreach (Binding binding in control.DataBindings)
                {
                    binding.ReadValue();
                }
            }
        }
    }

    #endregion

    #region ========== EVENT HANDLERS ==========

    private void UcStockInMaster_Load(object sender, EventArgs e)
    {
        // Control đã được load
        StockInDateDateEdit.EditValue = DateTime.Now;
    }

    private void StockInDateDateEdit_EditValueChanged(object sender, EventArgs e)
    {
        try
        {
            if (StockInDateDateEdit.EditValue is DateTime selectedDate)
            {
                // Cập nhật ngày vào DTO
                _stockInMasterDto.StockInDate = selectedDate;

                // Tạo số phiếu nhập tự động
                GenerateStockInNumber(selectedDate);

                // Xóa lỗi validation nếu có
                dxErrorProvider1.SetError(StockInDateDateEdit, string.Empty);
            }
        }
        catch (Exception ex)
        {
            ShowError(ex, "Lỗi tạo số phiếu nhập");
        }
    }

    private void WarehouseNameSearchLookupEdit_EditValueChanged(object sender, EventArgs e)
    {
        try
        {
            if (WarehouseNameSearchLookupEdit.EditValue is Guid warehouseId && warehouseId != Guid.Empty)
            {
                _stockInMasterDto.WarehouseId = warehouseId;

                // Lấy thông tin chi nhánh từ binding source
                var selectedWarehouse = Enumerable.Cast<CompanyBranchDto>(companyBranchDtoBindingSource)
                    .FirstOrDefault(w => w.Id == warehouseId);

                if (selectedWarehouse != null)
                {
                    _stockInMasterDto.WarehouseCode = selectedWarehouse.BranchCode;
                    _stockInMasterDto.WarehouseName = selectedWarehouse.BranchName;
                }

                // Xóa lỗi validation nếu có
                dxErrorProvider1.SetError(WarehouseNameSearchLookupEdit, string.Empty);
            }
            else
            {
                _stockInMasterDto.WarehouseId = Guid.Empty;
                _stockInMasterDto.WarehouseCode = null;
                _stockInMasterDto.WarehouseName = null;
            }
        }
        catch (Exception ex)
        {
            ShowError(ex, "Lỗi xử lý thay đổi kho");
        }
    }

    // Event handler cho Supplier - Đã xóa vì nhập nội bộ không cần nhà cung cấp

    private void StockInNumberTextEdit_EditValueChanged(object sender, EventArgs e)
    {
        try
        {
            if (StockInNumberTextEdit != null)
            {
                _stockInMasterDto.StockInNumber = StockInNumberTextEdit.Text?.Trim();

                // Xóa lỗi validation nếu có
                dxErrorProvider1.SetError(StockInNumberTextEdit, string.Empty);
            }
        }
        catch (Exception ex)
        {
            ShowError(ex, "Lỗi xử lý thay đổi số phiếu nhập");
        }
    }

    private void NguoiNhanHangTextEdit_EditValueChanged(object sender, EventArgs e)
    {
        try
        {
            if (NguoiNhanHangTextEdit != null)
            {
                _stockInMasterDto.NguoiNhanHang = NguoiNhanHangTextEdit.Text?.Trim();

                // Xóa lỗi validation nếu có
                dxErrorProvider1.SetError(NguoiNhanHangTextEdit, string.Empty);
            }
        }
        catch (Exception ex)
        {
            ShowError(ex, "Lỗi xử lý thay đổi người nhận hàng");
        }
    }

    private void NguoiGiaoHangTextEdit_EditValueChanged(object sender, EventArgs e)
    {
        try
        {
            if (NguoiGiaoHangTextEdit != null)
            {
                _stockInMasterDto.NguoiGiaoHang = NguoiGiaoHangTextEdit.Text?.Trim();

                // Xóa lỗi validation nếu có
                dxErrorProvider1.SetError(NguoiGiaoHangTextEdit, string.Empty);
            }
        }
        catch (Exception ex)
        {
            ShowError(ex, "Lỗi xử lý thay đổi người giao hàng");
        }
    }

    #endregion

    #region ========== VALIDATION ==========

    /// <summary>
    /// Cập nhật DTO từ controls
    /// </summary>
    private void UpdateDtoFromControls()
    {
        try
        {
            // Cập nhật từ TextEdit
            if (StockInNumberTextEdit != null)
            {
                _stockInMasterDto.StockInNumber = StockInNumberTextEdit.Text?.Trim();
            }

            // Cập nhật từ DateEdit
            if (StockInDateDateEdit != null && StockInDateDateEdit.EditValue is DateTime date)
            {
                _stockInMasterDto.StockInDate = date;
            }

            // Cập nhật từ Warehouse SearchLookUpEdit
            if (WarehouseNameSearchLookupEdit != null)
            {
                if (WarehouseNameSearchLookupEdit.EditValue is Guid warehouseId && warehouseId != Guid.Empty)
                {
                    _stockInMasterDto.WarehouseId = warehouseId;

                    // Lấy thông tin chi nhánh từ binding source
                    var selectedWarehouse = Enumerable.Cast<CompanyBranchDto>(companyBranchDtoBindingSource)
                        .FirstOrDefault(w => w.Id == warehouseId);

                    if (selectedWarehouse != null)
                    {
                        _stockInMasterDto.WarehouseCode = selectedWarehouse.BranchCode;
                        _stockInMasterDto.WarehouseName = selectedWarehouse.BranchName;
                    }
                }
                else
                {
                    _stockInMasterDto.WarehouseId = Guid.Empty;
                    _stockInMasterDto.WarehouseCode = null;
                    _stockInMasterDto.WarehouseName = null;
                }
            }

            // Cập nhật từ Supplier SearchLookUpEdit - Đã xóa vì nhập nội bộ không cần nhà cung cấp

            // Cập nhật từ NguoiNhanHangTextEdit
            if (NguoiNhanHangTextEdit != null)
            {
                _stockInMasterDto.NguoiNhanHang = NguoiNhanHangTextEdit.Text?.Trim();
            }

            // Cập nhật từ NguoiGiaoHangTextEdit
            if (NguoiGiaoHangTextEdit != null)
            {
                _stockInMasterDto.NguoiGiaoHang = NguoiGiaoHangTextEdit.Text?.Trim();
            }
        }
        catch (Exception ex)
        {
            ShowError(ex, "Lỗi cập nhật dữ liệu từ controls");
        }
    }

    /// <summary>
    /// Validate dữ liệu input và hiển thị lỗi bằng dxErrorProvider
    /// </summary>
    private bool ValidateInput()
    {
        try
        {
            dxErrorProvider1.ClearErrors();

            // Validate bằng DataAnnotations
            var context = new ValidationContext(_stockInMasterDto, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(_stockInMasterDto, context, results, validateAllProperties: true);

            if (!isValid)
            {
                // Hiển thị lỗi cho từng field
                foreach (var result in results)
                {
                    foreach (var memberName in result.MemberNames)
                    {
                        var control = FindControlByPropertyName(memberName);
                        if (control != null)
                        {
                            dxErrorProvider1.SetError(control, result.ErrorMessage, ErrorType.Critical);
                        }
                    }
                }

                // Focus vào control đầu tiên có lỗi
                var firstErrorControl = results
                    .SelectMany(r => r.MemberNames)
                    .Select(FindControlByPropertyName)
                    .FirstOrDefault(c => c != null);

                if (firstErrorControl != null)
                {
                    firstErrorControl.Focus();
                }

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

    /// <summary>
    /// Tìm control theo tên property trong DTO
    /// </summary>
    private Control FindControlByPropertyName(string propertyName)
    {
        return propertyName switch
        {
            nameof(NhapNoiBoMasterDto.StockInNumber) => StockInNumberTextEdit,
            nameof(NhapNoiBoMasterDto.StockInDate) => StockInDateDateEdit,
            nameof(NhapNoiBoMasterDto.WarehouseId) => WarehouseNameSearchLookupEdit,
            nameof(NhapNoiBoMasterDto.WarehouseCode) => WarehouseNameSearchLookupEdit,
            nameof(NhapNoiBoMasterDto.WarehouseName) => WarehouseNameSearchLookupEdit,
            nameof(NhapNoiBoMasterDto.Notes) => NotesTextEdit,
            nameof(NhapNoiBoMasterDto.NguoiNhanHang) => NguoiNhanHangTextEdit,
            nameof(NhapNoiBoMasterDto.NguoiGiaoHang) => NguoiGiaoHangTextEdit,
            _ => null
        };
    }

    #endregion

    #region ========== PUBLIC METHODS ==========

    /// <summary>
    /// Lấy DTO từ controls sau khi validate các trường bắt buộc
    /// </summary>
    /// <returns>NhapThietBiMuonMasterDto nếu validation thành công, null nếu có lỗi</returns>
    public NhapNoiBoMasterDto GetDto()
    {
        try
        {
            // Cập nhật DTO từ controls trước khi validate
            //UpdateDtoFromControls();

            // Validate các trường bắt buộc
            if (!ValidateInput())
            {
                return null; // Validation thất bại
            }

            //Cập nhật lại Id
            _stockInMasterDto.Id = _stockInOutMasterId;

            //Cập nhật lại LoaiNhapXuatKho
            _stockInMasterDto.LoaiNhapXuatKho = LoaiNhapXuatKhoEnum.NhapNoiBo;

            return _stockInMasterDto;
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

            // Lấy master entity từ BLL
            var masterEntity = _stockInBll.GetMasterById(stockInOutMasterId);
            if (masterEntity == null)
            {
                throw new InvalidOperationException($"Không tìm thấy phiếu nhập kho với ID: {stockInOutMasterId}");
            }

            // Set dữ liệu cho các control đơn giản (không cần datasource)
            StockInDateDateEdit.EditValue = masterEntity.StockInOutDate;
            StockInNumberTextEdit.EditValue = masterEntity.VocherNumber;
            NotesTextEdit.EditValue = masterEntity.Notes;
            NguoiNhanHangTextEdit.EditValue = masterEntity.NguoiNhanHang;
            NguoiGiaoHangTextEdit.EditValue = masterEntity.NguoiGiaoHang;

            // Load datasource cho Warehouse trước khi set EditValue
            await LoadSingleWarehouseByIdAsync(masterEntity.WarehouseId);
            WarehouseNameSearchLookupEdit.EditValue = masterEntity.WarehouseId;

            // Nhập nội bộ không cần supplier, không load supplier data

            NotesTextEdit.Text = masterEntity.Notes;

        }
        catch (Exception ex)
        {
            ShowError(ex, "Lỗi tải dữ liệu phiếu nhập kho");
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
            // Khởi tạo lại DTO
            InitializeDto();

            // Reset tất cả các controls về giá trị mặc định
            // Reset SearchLookUpEdit - phải set EditValue = null để xóa selection
            if (WarehouseNameSearchLookupEdit != null)
            {
                WarehouseNameSearchLookupEdit.EditValue = null;
            }

            // Nhập nội bộ không có Supplier control

            // Reset TextEdit
            if (StockInNumberTextEdit != null)
            {
                StockInNumberTextEdit.Text = string.Empty;
            }


            // Reset DateEdit
            if (StockInDateDateEdit != null)
            {
                StockInDateDateEdit.EditValue = DateTime.Now;
                // Tạo lại số phiếu nhập kho sau khi reset ngày
                GenerateStockInNumber(DateTime.Now);
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

            // Refresh bindings để đảm bảo UI được cập nhật
            RefreshAllBindings();

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
            // Sử dụng method SetTotals() vì các property giờ là computed (read-only)
            _stockInMasterDto.SetTotals(totalQuantity);

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

    /// <summary>
    /// Format tiền tệ (không có chữ số thập phân)
    /// </summary>
    private string FormatCurrency(decimal value)
    {
        return value.ToString(ApplicationConstants.CURRENCY_FORMAT);
    }

    #endregion

    #region ========== HELPER METHODS ==========

    /// <summary>
    /// Tạo số phiếu nhập kho theo format: PNK-MMYY-NNXXX
    /// PNK: Phiếu nhập kho
    /// MM: Tháng (2 ký tự)
    /// YY: Năm (2 ký tự)
    /// NN: Index của LoaiNhapXuatKhoEnum (2 ký tự)
    /// XXX: Số thứ tự phiếu (3 ký tự từ 001 đến 999)
    /// </summary>
    /// <param name="stockInDate">Ngày nhập kho</param>
    private void GenerateStockInNumber(DateTime stockInDate)
    {
        try
        {
            // Chỉ tạo số phiếu nếu chưa có hoặc đang ở trạng thái tạo mới
            if (!string.IsNullOrWhiteSpace(_stockInMasterDto.StockInNumber) &&
                _stockInMasterDto.TrangThai != TrangThaiPhieuNhapEnum.TaoMoi)
            {
                return;
            }

            // Lấy thông tin từ DTO
            var month = stockInDate.Month.ToString("D2"); // MM
            var year = stockInDate.Year.ToString().Substring(2); // YY (2 ký tự cuối)
            var loaiNhapKhoIndex = ((int)_stockInMasterDto.LoaiNhapXuatKho).ToString("D2"); // NN (2 ký tự)

            // Lấy số thứ tự tiếp theo
            var nextSequence = GetNextSequenceNumber(stockInDate, _stockInMasterDto.LoaiNhapXuatKho);

            // Tạo số phiếu: PNK-MMYY-NNXXX
            var stockInNumber = $"PNK-{month}{year}-{loaiNhapKhoIndex}{nextSequence:D3}";

            // Cập nhật vào DTO và control
            _stockInMasterDto.StockInNumber = stockInNumber;
            if (StockInNumberTextEdit != null)
            {
                StockInNumberTextEdit.Text = stockInNumber;
            }
        }
        catch (Exception ex)
        {
            ShowError(ex, "Lỗi tạo số phiếu nhập");
        }
    }

    /// <summary>
    /// Lấy số thứ tự tiếp theo cho phiếu nhập kho
    /// </summary>
    /// <param name="stockInDate">Ngày nhập kho</param>
    /// <param name="loaiNhapXuatKho">Loại nhập kho</param>
    /// <returns>Số thứ tự tiếp theo (1-999)</returns>
    private int GetNextSequenceNumber(DateTime stockInDate, LoaiNhapXuatKhoEnum loaiNhapXuatKho)
    {
        try
        {
            // Query database để lấy số thứ tự tiếp theo thông qua BLL
            // Format cần tìm: PNK-MMYY-NNXXX
            // Trong đó MM, YY, NN đã biết, cần tìm XXX lớn nhất + 1
            var nextSequence = _stockInBll.GetNextSequenceNumber(stockInDate, loaiNhapXuatKho);
            return nextSequence;
        }
        catch (Exception ex)
        {
            ShowError(ex, "Lỗi lấy số thứ tự phiếu nhập");
            return 1; // Fallback
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

    /// <summary>
    /// Hiển thị lỗi
    /// </summary>
    private void ShowError(string message)
    {
        try
        {
            // Tìm parent form để làm owner cho MsgBox
            var parentForm = this.FindForm();

            // Sử dụng MsgBox.ShowError
            MsgBox.ShowError(message, "Lỗi", parentForm);
        }
        catch
        {
            // Fallback nếu có lỗi khi hiển thị MsgBox
            System.Diagnostics.Debug.WriteLine($"Lỗi: {message}");
        }
    }

    #endregion
}