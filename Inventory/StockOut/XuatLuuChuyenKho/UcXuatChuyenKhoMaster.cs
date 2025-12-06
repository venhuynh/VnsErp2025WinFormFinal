using Bll.Inventory.InventoryManagement;
using Bll.Inventory.StockIn;
using Bll.MasterData.CompanyBll;
using Bll.MasterData.CustomerBll;
using Common;
using Common.Utils;
using Dal.DataContext;
using DevExpress.XtraEditors;
using DTO.Inventory.StockIn;
using DTO.Inventory.StockOut.XuatLuuChuyenKho;
using DTO.MasterData.Company;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors.DXErrorProvider;
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
    /// Business Logic Layer cho StockIn (dùng để lấy master entity)
    /// </summary>
    private readonly StockInBll _stockInBll = new();

    /// <summary>
    /// Business Logic Layer cho StockInOutMaster (dùng để tạo số phiếu)
    /// </summary>
    private readonly StockInOutMasterBll _stockInOutMasterBll = new();

    /// <summary>
    /// Business Logic Layer cho BusinessPartner (dùng để đồng bộ CompanyBranch sang BusinessPartnerSite)
    /// </summary>
    private readonly BusinessPartnerBll _businessPartnerBll = new();

    /// <summary>
    /// Business Logic Layer cho BusinessPartnerSite (dùng để đồng bộ CompanyBranch sang BusinessPartnerSite)
    /// </summary>
    private readonly BusinessPartnerSiteBll _businessPartnerSiteBll = new();

    /// <summary>
    /// Flag đánh dấu Warehouse datasource đã được load chưa
    /// </summary>
    private bool _isWarehouseDataSourceLoaded;


    private Guid _stockInOutMasterId = Guid.Empty;

    /// <summary>
    /// StockInOutMaster entity
    /// </summary>
    private StockInOutMaster _stockInMaster;

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
            // Khởi tạo Entity
            InitializeEntity();


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
    /// Khởi tạo Entity
    /// </summary>
    private void InitializeEntity()
    {
        _stockInMaster = new StockInOutMaster
        {
            Id = Guid.Empty,
            VocherNumber = null,
            StockInOutDate = DateTime.Now,
            StockInOutType = (int)LoaiNhapXuatKhoEnum.XuatLuuChuyenKho,
            VoucherStatus = (int)TrangThaiPhieuNhapEnum.TaoMoi,
            WarehouseId = Guid.Empty, // Kho xuất (kho nguồn)
            PartnerSiteId = null, // Kho nhập (kho đích) sẽ được lưu ở đây hoặc field khác
            Notes = null,
            TotalQuantity = 0,
            TotalAmount = 0,
            TotalVat = 0,
            TotalAmountIncludedVat = 0,
            NguoiNhanHang = null,
            NguoiGiaoHang = null
        };
    }


    /// <summary>
    /// Setup SearchLookUpEdit cho Warehouse
    /// </summary>
    private void SetupLookupEdits()
    {
        try
        {
            // Setup Warehouse SearchLookUpEdit
            WarehouseStockInNameSearchLookupEdit.Properties.DataSource = companyBranchDtoBindingSource;
            WarehouseStockInNameSearchLookupEdit.Properties.ValueMember = "Id";
            WarehouseStockInNameSearchLookupEdit.Properties.DisplayMember = "ThongTinHtml";
            WarehouseStockInNameSearchLookupEdit.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            WarehouseStockInNameSearchLookupEdit.Properties.PopupView = CompanyBranchDtoSearchLookUpEdit1View;

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
                typeof(XuatLuuChuyenKhoMasterDto),
                logger: (msg, ex) => System.Diagnostics.Debug.WriteLine($"{msg}: {ex?.Message}")
            );

            // Xử lý đặc biệt cho WarehouseStockOutId và WarehouseStockInId
            // Vì RequiredFieldHelper không thể tự động match với SearchLookupEdit
            if (ItemForKhoXuat != null && !ItemForKhoXuat.Text.Contains("*"))
            {
                ItemForKhoXuat.AllowHtmlStringInCaption = true;
                var baseCaption = string.IsNullOrWhiteSpace(ItemForKhoXuat.Text) ? "Kho xuất" : ItemForKhoXuat.Text;
                ItemForKhoXuat.Text = baseCaption + @" <color=red>*</color>";
            }
            
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
            WarehouseStockInNameSearchLookupEdit.Popup += WarehouseNameSearchLookupEdit_Popup;
            WarehouseStockInNameSearchLookupEdit.EditValueChanged += WarehouseNameSearchLookupEdit_EditValueChanged;

            //Sự kiện của WarehouseStockOutNameSearchLookupEdit
            WarehouseStockOutNameSearchLookupEdit.Popup += WarehouseStockOutNameSearchLookupEdit_Popup;
            WarehouseStockOutNameSearchLookupEdit.EditValueChanged += WarehouseStockOutNameSearchLookupEdit_EditValueChanged;


            StockInDateDateEdit.EditValueChanged += StockOutDateDateEdit_EditValueChanged;

            StockInNumberTextEdit.EditValueChanged += StockOutNumberTextEdit_EditValueChanged;

            // Sự kiện của NguoiNhanHangTextEdit và NguoiGiaoHangTextEdit
            NguoiNhanHangTextEdit.EditValueChanged += NguoiNhanHangTextEdit_EditValueChanged;
            NguoiGiaoHangTextEdit.EditValueChanged += NguoiGiaoHangTextEdit_EditValueChanged;
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
    /// Load dữ liệu lookup (Warehouse và Supplier)
    /// Method này được gọi từ form khi FormLoad
    /// </summary>
    public async Task LoadLookupDataAsync()
    {
        try
        {
            // Reset flags để đảm bảo load lại khi form load
            _isWarehouseDataSourceLoaded = false;

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

            // Đảm bảo rằng branches được đồng bộ sang BusinessPartnerSite và các bảng có liên quan vì xuất lưu chuyển kho
            await EnsureBranchesSyncedToBusinessPartnerSitesAsync(branches);

        }
        catch (Exception ex)
        {
            ShowError(ex, "Lỗi tải dữ liệu kho");
            throw;
        }
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
    /// Map StockInOutMaster entity sang XuatLuuChuyenKhoMasterDto
    /// </summary>
    private XuatLuuChuyenKhoMasterDto MapEntityToDto(StockInOutMaster entity)
    {
        if (entity == null) return null;

        // Load kho đích (kho nhập) từ PartnerSiteId nếu có
        // Với lưu chuyển kho, PartnerSiteId lưu ID của BusinessPartnerSite (kho đích)
        // Cần lấy BusinessPartnerSite trước, sau đó tìm CompanyBranch tương ứng
        CompanyBranch destinationWarehouse = null;
        if (entity.PartnerSiteId.HasValue && entity.PartnerSiteId.Value != Guid.Empty)
        {
            // Lấy BusinessPartnerSite từ PartnerSiteId
            var businessPartnerSite = _businessPartnerSiteBll.GetById(entity.PartnerSiteId.Value);
            if (businessPartnerSite != null && businessPartnerSite.SiteCode.StartsWith("BRANCH-"))
            {
                // Lấy BranchCode từ SiteCode (format: "BRANCH-{BranchCode}")
                var branchCode = businessPartnerSite.SiteCode.Substring(7); // Bỏ qua "BRANCH-"
                destinationWarehouse = _companyBranchBll.GetAll()
                    .FirstOrDefault(b => b.BranchCode == branchCode);
            }
        }

        var dto = new XuatLuuChuyenKhoMasterDto
        {
            Id = entity.Id,
            StockOutNumber = entity.VocherNumber ?? string.Empty,
            StockOutDate = entity.StockInOutDate,
            LoaiNhapXuatKho = (LoaiNhapXuatKhoEnum)entity.StockInOutType,
            TrangThai = (TrangThaiPhieuNhapEnum)entity.VoucherStatus,
            WarehouseStockOutId = entity.WarehouseId, // Kho xuất (kho nguồn)
            WarehouseStockOutCode = entity.CompanyBranch?.BranchCode ?? string.Empty,
            WarehouseStockOutName = entity.CompanyBranch?.BranchName ?? string.Empty,
            WarehouseStockInId = entity.PartnerSiteId ?? Guid.Empty, // Kho nhập (kho đích) - lưu trong PartnerSiteId
            WarehouseStockInCode = destinationWarehouse?.BranchCode ?? string.Empty,
            WarehouseStockInName = destinationWarehouse?.BranchName ?? string.Empty,
            Notes = entity.Notes ?? string.Empty,
            NguoiNhanHang = entity.NguoiNhanHang ?? string.Empty,
            NguoiGiaoHang = entity.NguoiGiaoHang ?? string.Empty
        };

        // Gán giá trị tổng hợp từ entity
        dto.SetTotals(entity.TotalQuantity);

        return dto;
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

    private void StockOutDateDateEdit_EditValueChanged(object sender, EventArgs e)
    {
        try
        {
            if (StockInDateDateEdit.EditValue is DateTime selectedDate)
            {
                // Cập nhật ngày vào Entity
                _stockInMaster.StockInOutDate = selectedDate;

                // Tạo số phiếu xuất tự động
                GenerateStockOutNumber(selectedDate);

                // Xóa lỗi validation nếu có
                dxErrorProvider1.SetError(StockInDateDateEdit, string.Empty);
            }
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
                if (_stockInMaster.WarehouseId == warehouseId)
                {
                    dxErrorProvider1.SetError(WarehouseStockInNameSearchLookupEdit, "Kho nhập và kho xuất không được trùng nhau");
                    return;
                }

                // Lấy CompanyBranch từ warehouseId
                var branch = _companyBranchBll.GetById(warehouseId);
                if (branch == null)
                {
                    dxErrorProvider1.SetError(WarehouseStockInNameSearchLookupEdit, "Không tìm thấy thông tin kho");
                    return;
                }

                // Tìm BusinessPartnerSite tương ứng với CompanyBranch (dùng SiteCode = "BRANCH-{BranchCode}")
                var siteCode = $"BRANCH-{branch.BranchCode}";
                var businessPartnerSite = _businessPartnerSiteBll.GetAll()
                    .FirstOrDefault(s => s.SiteCode == siteCode);

                if (businessPartnerSite == null)
                {
                    // Nếu chưa có, đảm bảo đồng bộ trước
                    EnsureBranchesSyncedToBusinessPartnerSitesAsync(new List<CompanyBranch> { branch }).Wait();
                    businessPartnerSite = _businessPartnerSiteBll.GetAll()
                        .FirstOrDefault(s => s.SiteCode == siteCode);
                }

                // Cập nhật PartnerSiteId vào Entity (kho nhập - kho đích)
                _stockInMaster.PartnerSiteId = businessPartnerSite?.Id;

                // Xóa lỗi validation nếu có
                dxErrorProvider1.SetError(WarehouseStockInNameSearchLookupEdit, string.Empty);
            }
            else
            {
                _stockInMaster.PartnerSiteId = null;
            }
        }
        catch (Exception ex)
        {
            ShowError(ex, "Lỗi xử lý thay đổi kho nhập");
        }
    }


    private void WarehouseStockOutNameSearchLookupEdit_EditValueChanged(object sender, EventArgs e)
    {
        try
        {
            if (WarehouseStockOutNameSearchLookupEdit.EditValue is Guid warehouseId && warehouseId != Guid.Empty)
            {
                // Kiểm tra không được trùng với kho nhập
                if (_stockInMaster.PartnerSiteId == warehouseId)
                {
                    dxErrorProvider1.SetError(WarehouseStockOutNameSearchLookupEdit, "Kho xuất và kho nhập không được trùng nhau");
                    return;
                }

                // Cập nhật WarehouseId vào Entity (kho xuất - kho nguồn)
                _stockInMaster.WarehouseId = warehouseId;

                // Xóa lỗi validation nếu có
                dxErrorProvider1.SetError(WarehouseStockOutNameSearchLookupEdit, string.Empty);
            }
            else
            {
                _stockInMaster.WarehouseId = Guid.Empty;
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

    // Event handler cho Supplier - Đã xóa vì nhập nội bộ không cần nhà cung cấp

    private void StockOutNumberTextEdit_EditValueChanged(object sender, EventArgs e)
    {
        try
        {
            if (StockInNumberTextEdit != null)
            {
                _stockInMaster.VocherNumber = StockInNumberTextEdit.Text?.Trim();

                // Xóa lỗi validation nếu có
                dxErrorProvider1.SetError(StockInNumberTextEdit, string.Empty);
            }
        }
        catch (Exception ex)
        {
            ShowError(ex, "Lỗi xử lý thay đổi số phiếu xuất");
        }
    }

    private void NguoiNhanHangTextEdit_EditValueChanged(object sender, EventArgs e)
    {
        try
        {
            if (NguoiNhanHangTextEdit != null)
            {
                _stockInMaster.NguoiNhanHang = NguoiNhanHangTextEdit.Text?.Trim();

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
                _stockInMaster.NguoiGiaoHang = NguoiGiaoHangTextEdit.Text?.Trim();

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
                _stockInMaster.VocherNumber = StockInNumberTextEdit.Text?.Trim();
            }

            // Cập nhật từ DateEdit
            if (StockInDateDateEdit != null && StockInDateDateEdit.EditValue is DateTime date)
            {
                _stockInMaster.StockInOutDate = date;
            }

            // Cập nhật từ Warehouse StockOut SearchLookUpEdit (kho xuất - kho nguồn)
            if (WarehouseStockOutNameSearchLookupEdit != null)
            {
                if (WarehouseStockOutNameSearchLookupEdit.EditValue is Guid warehouseId && warehouseId != Guid.Empty)
                {
                    _stockInMaster.WarehouseId = warehouseId;
                }
                else
                {
                    _stockInMaster.WarehouseId = Guid.Empty;
                }
            }

            // Cập nhật từ Warehouse StockIn SearchLookUpEdit (kho nhập - kho đích)
            // Lưu ý: PartnerSiteId đã được set trong event handler WarehouseNameSearchLookupEdit_EditValueChanged
            // Nên ở đây chỉ cần đảm bảo giá trị đã được cập nhật đúng
            // (Event handler đã xử lý việc lấy BusinessPartnerSiteId từ CompanyBranch)

            // Cập nhật từ NguoiNhanHangTextEdit
            if (NguoiNhanHangTextEdit != null)
            {
                _stockInMaster.NguoiNhanHang = NguoiNhanHangTextEdit.Text?.Trim();
            }

            // Cập nhật từ NguoiGiaoHangTextEdit
            if (NguoiGiaoHangTextEdit != null)
            {
                _stockInMaster.NguoiGiaoHang = NguoiGiaoHangTextEdit.Text?.Trim();
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

            // Convert Entity sang DTO để validate (vì DataAnnotations chỉ hoạt động với DTO)
            var dto = MapEntityToDto(_stockInMaster);
            if (dto == null)
            {
                ShowError("Không thể convert entity sang DTO để validate");
                return false;
            }

            // Validate bằng DataAnnotations trên DTO
            var context = new ValidationContext(dto, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(dto, context, results, validateAllProperties: true);

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
            nameof(XuatLuuChuyenKhoMasterDto.StockOutNumber) => StockInNumberTextEdit,
            nameof(XuatLuuChuyenKhoMasterDto.StockOutDate) => StockInDateDateEdit,
            nameof(XuatLuuChuyenKhoMasterDto.WarehouseStockOutId) => WarehouseStockOutNameSearchLookupEdit,
            nameof(XuatLuuChuyenKhoMasterDto.WarehouseStockOutCode) => WarehouseStockOutNameSearchLookupEdit,
            nameof(XuatLuuChuyenKhoMasterDto.WarehouseStockOutName) => WarehouseStockOutNameSearchLookupEdit,
            nameof(XuatLuuChuyenKhoMasterDto.WarehouseStockInId) => WarehouseStockInNameSearchLookupEdit,
            nameof(XuatLuuChuyenKhoMasterDto.WarehouseStockInCode) => WarehouseStockInNameSearchLookupEdit,
            nameof(XuatLuuChuyenKhoMasterDto.WarehouseStockInName) => WarehouseStockInNameSearchLookupEdit,
            nameof(XuatLuuChuyenKhoMasterDto.Notes) => NotesTextEdit,
            nameof(XuatLuuChuyenKhoMasterDto.NguoiNhanHang) => NguoiNhanHangTextEdit,
            nameof(XuatLuuChuyenKhoMasterDto.NguoiGiaoHang) => NguoiGiaoHangTextEdit,
            _ => null
        };
    }

    #endregion

    #region ========== PUBLIC METHODS ==========

    /// <summary>
    /// Lấy DTO từ Entity sau khi validate các trường bắt buộc
    /// </summary>
    /// <returns>XuatLuuChuyenKhoMasterDto nếu validation thành công, null nếu có lỗi</returns>
    public XuatLuuChuyenKhoMasterDto GetDto()
    {
        try
        {
            // Cập nhật Entity từ controls trước khi validate
            UpdateDtoFromControls();

            // Validate các trường bắt buộc
            if (!ValidateInput())
            {
                return null; // Validation thất bại
            }

            // Cập nhật lại Id và LoaiNhapXuatKho vào Entity
            _stockInMaster.Id = _stockInOutMasterId;
            _stockInMaster.StockInOutType = (int)LoaiNhapXuatKhoEnum.XuatLuuChuyenKho;

            // Convert Entity sang DTO để trả về
            return MapEntityToDto(_stockInMaster);
        }
        catch (Exception ex)
        {
            ShowError(ex, "Lỗi lấy dữ liệu");
            return null;
        }
    }

    /// <summary>
    /// Load dữ liệu master từ ID phiếu xuất kho
    /// </summary>
    /// <param name="stockInOutMasterId">ID phiếu xuất kho</param>
    public async Task LoadDataAsync(Guid stockInOutMasterId)
    {
        try
        {
            _stockInOutMasterId = stockInOutMasterId;

            // Lấy master entity từ BLL
            var masterEntity = _stockInBll.GetMasterById(stockInOutMasterId);
            if (masterEntity == null)
            {
                throw new InvalidOperationException($"Không tìm thấy phiếu xuất kho với ID: {stockInOutMasterId}");
            }

            // Gán entity vào _stockInMaster
            _stockInMaster = masterEntity;

            // Set dữ liệu cho các control đơn giản (không cần datasource)
            StockInDateDateEdit.EditValue = masterEntity.StockInOutDate;
            StockInNumberTextEdit.EditValue = masterEntity.VocherNumber;
            NotesTextEdit.EditValue = masterEntity.Notes;
            NguoiNhanHangTextEdit.EditValue = masterEntity.NguoiNhanHang;
            NguoiGiaoHangTextEdit.EditValue = masterEntity.NguoiGiaoHang;

            // Load datasource cho Warehouse StockOut (kho xuất - kho nguồn) trước khi set EditValue
            await LoadSingleWarehouseByIdAsync(masterEntity.WarehouseId);
            WarehouseStockOutNameSearchLookupEdit.EditValue = masterEntity.WarehouseId;

            // Load datasource cho Warehouse StockIn (kho nhập - kho đích) nếu có PartnerSiteId
            if (masterEntity.PartnerSiteId.HasValue && masterEntity.PartnerSiteId.Value != Guid.Empty)
            {
                // Lấy BusinessPartnerSite từ PartnerSiteId
                var businessPartnerSite = _businessPartnerSiteBll.GetById(masterEntity.PartnerSiteId.Value);
                if (businessPartnerSite != null && businessPartnerSite.SiteCode.StartsWith("BRANCH-"))
                {
                    // Lấy BranchCode từ SiteCode (format: "BRANCH-{BranchCode}")
                    var branchCode = businessPartnerSite.SiteCode.Substring(7); // Bỏ qua "BRANCH-"
                    var destinationBranch = _companyBranchBll.GetAll()
                        .FirstOrDefault(b => b.BranchCode == branchCode);
                    
                    if (destinationBranch != null)
                    {
                        await LoadSingleWarehouseByIdAsync(destinationBranch.Id);
                        WarehouseStockInNameSearchLookupEdit.EditValue = destinationBranch.Id;
                    }
                }
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
            // Khởi tạo lại Entity
            InitializeEntity();

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
            // Cập nhật trực tiếp vào Entity
            _stockInMaster.TotalQuantity = totalQuantity;

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
    /// Đảm bảo rằng các CompanyBranch được đồng bộ sang BusinessPartnerSite
    /// Để có thể lưu kho đích (kho nhận) vào PartnerSiteId trong StockInOutMaster
    /// </summary>
    /// <param name="branches">Danh sách CompanyBranch cần đồng bộ</param>
    private async Task EnsureBranchesSyncedToBusinessPartnerSitesAsync(List<CompanyBranch> branches)
    {
        try
        {
            if (branches == null || branches.Count == 0)
                return;

            await Task.Run(() =>
            {
                // 1. Tìm hoặc tạo BusinessPartner đặc biệt cho công ty (dùng cho lưu chuyển kho nội bộ)
                const string companyPartnerCode = "COMPANY-INTERNAL";
                var companyPartner = _businessPartnerBll.GetAll()
                    .FirstOrDefault(p => p.PartnerCode == companyPartnerCode);

                if (companyPartner == null)
                {
                    // Tạo BusinessPartner mới cho công ty
                    companyPartner = new BusinessPartner
                    {
                        Id = Guid.NewGuid(),
                        PartnerCode = companyPartnerCode,
                        PartnerName = "Công ty nội bộ (Lưu chuyển kho)",
                        PartnerType = 3, // Cả khách hàng và nhà cung cấp
                        IsActive = true,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = null
                    };
                    _businessPartnerBll.SaveOrUpdate(companyPartner);
                }

                // 2. Với mỗi CompanyBranch, tìm hoặc tạo BusinessPartnerSite tương ứng
                foreach (var branch in branches)
                {
                    if (!branch.IsActive)
                        continue; // Chỉ đồng bộ các branch đang hoạt động

                    // Tìm BusinessPartnerSite theo SiteCode (dùng format: BRANCH-{BranchCode})
                    var siteCode = $"BRANCH-{branch.BranchCode}";
                    var existingSite = _businessPartnerSiteBll.GetAll()
                        .FirstOrDefault(s => s.SiteCode == siteCode);

                    if (existingSite == null)
                    {
                        // Tạo BusinessPartnerSite mới tương ứng với CompanyBranch
                        var newSite = new BusinessPartnerSite
                        {
                            Id = Guid.NewGuid(),
                            PartnerId = companyPartner.Id,
                            SiteCode = siteCode,
                            SiteName = branch.BranchName,
                            Address = branch.Address ?? string.Empty,
                            City = string.Empty, // CompanyBranch không có City, có thể lấy từ Company nếu cần
                            Province = string.Empty,
                            Country = string.Empty,
                            ContactPerson = branch.ManagerName ?? string.Empty,
                            Phone = branch.Phone ?? string.Empty,
                            Email = branch.Email ?? string.Empty,
                            IsDefault = false,
                            IsActive = branch.IsActive,
                            CreatedDate = DateTime.Now,
                            UpdatedDate = null
                        };

                        _businessPartnerSiteBll.SaveOrUpdate(newSite);
                    }
                    else
                    {
                        // Cập nhật thông tin BusinessPartnerSite nếu có thay đổi
                        var needsUpdate = false;
                        if (existingSite.SiteName != branch.BranchName)
                        {
                            existingSite.SiteName = branch.BranchName;
                            needsUpdate = true;
                        }
                        if (existingSite.Address != (branch.Address ?? string.Empty))
                        {
                            existingSite.Address = branch.Address ?? string.Empty;
                            needsUpdate = true;
                        }
                        if (existingSite.ContactPerson != (branch.ManagerName ?? string.Empty))
                        {
                            existingSite.ContactPerson = branch.ManagerName ?? string.Empty;
                            needsUpdate = true;
                        }
                        if (existingSite.Phone != (branch.Phone ?? string.Empty))
                        {
                            existingSite.Phone = branch.Phone ?? string.Empty;
                            needsUpdate = true;
                        }
                        if (existingSite.Email != (branch.Email ?? string.Empty))
                        {
                            existingSite.Email = branch.Email ?? string.Empty;
                            needsUpdate = true;
                        }
                        if (existingSite.IsActive != branch.IsActive)
                        {
                            existingSite.IsActive = branch.IsActive;
                            needsUpdate = true;
                        }

                        if (needsUpdate)
                        {
                            existingSite.UpdatedDate = DateTime.Now;
                            _businessPartnerSiteBll.SaveOrUpdate(existingSite);
                        }
                    }
                }
            });
        }
        catch (Exception ex)
        {
            // Log lỗi nhưng không làm gián đoạn user experience
            System.Diagnostics.Debug.WriteLine($"Lỗi đồng bộ CompanyBranch sang BusinessPartnerSite: {ex.Message}");
            // Không throw exception để không làm gián đoạn quá trình load dữ liệu
        }
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
            // Chỉ tạo số phiếu nếu chưa có hoặc đang ở trạng thái tạo mới
            if (!string.IsNullOrWhiteSpace(_stockInMaster.VocherNumber) &&
                _stockInMaster.VoucherStatus != (int)TrangThaiPhieuNhapEnum.TaoMoi)
            {
                return;
            }

            // Lấy loại nhập/xuất kho từ Entity
            var loaiNhapXuatKho = (LoaiNhapXuatKhoEnum)_stockInMaster.StockInOutType;

            // Gọi BLL để tạo số phiếu tự động (tự động xác định PNK hay PXK)
            var voucherNumber = _stockInOutMasterBll.GenerateVoucherNumber(stockOutDate, loaiNhapXuatKho);

            // Cập nhật vào Entity và control
            _stockInMaster.VocherNumber = voucherNumber;
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