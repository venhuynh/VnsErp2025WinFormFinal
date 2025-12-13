using Bll.Inventory.InventoryManagement;
using Bll.MasterData.CompanyBll;
using Common;
using Common.Utils;
using Dal.DataContext;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using DTO.Inventory.StockIn;
using DTO.Inventory.StockOut.XuatNoiBo;
using DTO.MasterData.Company;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bll.Inventory.StockInOut;
using DTO.Inventory.InventoryManagement;

namespace Inventory.StockOut.XuatNoiBo;

public partial class UcXuatNoiBoMasterDto : XtraUserControl
{
    #region ========== KHAI BÁO BIẾN ==========


    /// <summary>
    /// Business Logic Layer cho CompanyBranch (dùng cho Warehouse lookup)
    /// </summary>
    private readonly CompanyBranchBll _companyBranchBll = new();


    /// <summary>
    /// Business Logic Layer cho StockIn (dùng để lấy master entity)
    /// </summary>
    private readonly StockInOutBll _stockInBll = new();

    /// <summary>
    /// Business Logic Layer cho StockInOutMaster (dùng để tạo số phiếu)
    /// </summary>
    private readonly StockInOutMasterBll _stockInOutMasterBll = new();

    /// <summary>
    /// Flag đánh dấu Warehouse datasource đã được load chưa
    /// </summary>
    private bool _isWarehouseDataSourceLoaded;

    private Guid _stockInOutMasterId = Guid.Empty;

    /// <summary>
    /// StockInOutMaster entity
    /// </summary>
    private StockInOutMaster _stockOutMaster;

    #endregion

    #region ========== CONSTRUCTOR ==========

    public UcXuatNoiBoMasterDto()
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
        _stockOutMaster = new StockInOutMaster
        {
            Id = Guid.Empty,
            VocherNumber = null,
            StockInOutDate = DateTime.Now,
            StockInOutType = (int)LoaiNhapXuatKhoEnum.XuatNoiBo,
            VoucherStatus = (int)TrangThaiPhieuNhapEnum.TaoMoi,
            WarehouseId = Guid.Empty,
            PartnerSiteId = null, // Xuất nội bộ không có customer
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
                typeof(XuatNoiBoMasterDto),
                nullValuePrompt: "Bắt buộc nhập",
                logger: (msg, ex) => System.Diagnostics.Debug.WriteLine($"{msg}: {ex?.Message}")
            );

            // Xử lý đặc biệt cho WarehouseId (control là WarehouseNameSearchLookupEdit)
            // Vì RequiredFieldHelper không thể tự động match WarehouseId với WarehouseNameSearchLookupEdit
            if (ItemForWarehouseName != null && !ItemForWarehouseName.Text.Contains("*"))
            {
                ItemForWarehouseName.AllowHtmlStringInCaption = true;
                var baseCaption = string.IsNullOrWhiteSpace(ItemForWarehouseName.Text) ? "Kho xuất" : ItemForWarehouseName.Text;
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
            Load += UcXuatNoiBoMasterDto_Load;

            //Sự kiện của WarehouseNameSearchLookupEdit
            WarehouseNameSearchLookupEdit.Popup += WarehouseNameSearchLookupEdit_Popup;
            WarehouseNameSearchLookupEdit.EditValueChanged += WarehouseNameSearchLookupEdit_EditValueChanged;

            StockOutDateDateEdit.EditValueChanged += StockOutDateDateEdit_EditValueChanged;

            StockOutNumberTextEdit.EditValueChanged += StockOutNumberTextEdit_EditValueChanged;

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
        // SuperTip cho Số phiếu xuất nội bộ
        if (StockOutNumberTextEdit != null)
        {
            SuperToolTipHelper.SetTextEditSuperTip(
                StockOutNumberTextEdit,
                title: @"<b><color=DarkBlue>📄 Số phiếu xuất nội bộ</color></b>",
                content: @"Số phiếu xuất nội bộ được tạo tự động theo format: <b>PXK-MMYY-NNXXX</b><br/><br/><b>Format:</b><br/>• PXK: Phiếu xuất kho<br/>• MM: Tháng (2 ký tự)<br/>• YY: Năm (2 ký tự cuối)<br/>• NN: Index của Loại xuất kho (2 ký tự)<br/>• XXX: Số thứ tự phiếu (3 ký tự từ 001 đến 999)<br/><br/><b>Chức năng:</b><br/>• Tự động tạo khi thay đổi ngày xuất nội bộ<br/>• Tự động tạo khi thay đổi loại xuất kho<br/>• Query database để lấy số thứ tự tiếp theo<br/>• Đảm bảo số phiếu duy nhất trong cùng tháng/năm/loại<br/><br/><b>Ràng buộc:</b><br/>• <b>Bắt buộc nhập</b> (có dấu * đỏ)<br/>• Không được để trống<br/>• Tối đa 50 ký tự<br/><br/><color=Gray>Lưu ý:</color> Số phiếu xuất nội bộ sẽ được lưu vào database khi lưu phiếu xuất."
            );
        }


        // SuperTip cho Người nhận hàng
        if (NguoiNhanHangTextEdit != null)
        {
            SuperToolTipHelper.SetTextEditSuperTip(
                NguoiNhanHangTextEdit,
                title: @"<b><color=DarkBlue>👤 Người nhận hàng</color></b>",
                content: @"Nhập tên người nhận hàng nội bộ tại kho.<br/><br/><b>Chức năng:</b><br/>• Ghi nhận thông tin người nhận hàng nội bộ<br/>• Hỗ trợ tra cứu và theo dõi<br/><br/><b>Ràng buộc:</b><br/>• <b>Bắt buộc nhập</b> (có dấu * đỏ)<br/>• Tối đa 500 ký tự<br/><br/><color=Gray>Lưu ý:</color> Thông tin này sẽ được lưu vào database khi lưu phiếu xuất nội bộ."
            );
        }

        // SuperTip cho Người giao hàng
        if (NguoiGiaoHangTextEdit != null)
        {
            SuperToolTipHelper.SetTextEditSuperTip(
                NguoiGiaoHangTextEdit,
                title: @"<b><color=DarkBlue>🚚 Người giao hàng</color></b>",
                content: @"Nhập tên người giao hàng nội bộ (từ chi nhánh/đơn vị nội bộ).<br/><br/><b>Chức năng:</b><br/>• Ghi nhận thông tin người giao hàng nội bộ<br/>• Hỗ trợ tra cứu và theo dõi<br/><br/><b>Ràng buộc:</b><br/>• <b>Bắt buộc nhập</b> (có dấu * đỏ)<br/>• Tối đa 500 ký tự<br/><br/><color=Gray>Lưu ý:</color> Thông tin này sẽ được lưu vào database khi lưu phiếu xuất nội bộ."
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
                title: @"<b><color=DarkBlue>📅 Ngày xuất nội bộ</color></b>",
                content: @"Chọn ngày xuất nội bộ cho phiếu xuất.<br/><br/><b>Chức năng:</b><br/>• Xác định thời điểm xuất nội bộ<br/>• Tự động tạo số phiếu xuất nội bộ dựa trên ngày<br/>• Format số phiếu: PXK-MMYY-NNXXX (MM, YY từ ngày này)<br/>• Query database để lấy số thứ tự tiếp theo trong tháng/năm<br/><br/><b>Ràng buộc:</b><br/>• <b>Bắt buộc nhập</b> (có dấu * đỏ)<br/>• Không được để trống<br/>• Mặc định: Ngày hiện tại<br/><br/><b>Validation:</b><br/>• Kiểm tra rỗng khi validating<br/>• Hiển thị lỗi qua ErrorProvider nếu không hợp lệ<br/><br/><color=Gray>Lưu ý:</color> Khi thay đổi ngày xuất nội bộ, hệ thống sẽ tự động tạo lại số phiếu xuất nội bộ theo format mới."
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
                content: @"Chọn kho xuất nội bộ từ danh sách chi nhánh (Company Branch) đang hoạt động.<br/><br/><b>Chức năng:</b><br/>• Chọn kho xuất nội bộ<br/>• Hiển thị thông tin kho dạng HTML (mã, tên)<br/>• Tự động cập nhật WarehouseId vào Entity<br/><br/><b>Ràng buộc:</b><br/>• <b>Bắt buộc chọn</b> (có dấu * đỏ)<br/>• Không được để trống<br/>• Chỉ hiển thị các chi nhánh đang hoạt động (IsActive = true)<br/><br/><b>Data Source:</b><br/>• Load từ CompanyBranchBll.GetAll()<br/>• Filter chỉ lấy các chi nhánh đang hoạt động<br/>• Sắp xếp theo tên chi nhánh<br/><br/><b>Validation:</b><br/>• Kiểm tra rỗng khi validating<br/>• Hiển thị lỗi qua ErrorProvider nếu không hợp lệ<br/><br/><color=Gray>Lưu ý:</color> Kho xuất nội bộ sẽ được lưu vào database khi lưu phiếu xuất nội bộ."
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
                content: @"Nhập ghi chú hoặc mô tả bổ sung cho phiếu xuất nội bộ.<br/><br/><b>Chức năng:</b><br/>• Lưu thông tin bổ sung về phiếu xuất nội bộ<br/>• Ghi chú về lý do xuất nội bộ, điều kiện chuyển kho, đích đến hàng hóa, v.v.<br/>• Hỗ trợ nhiều dòng văn bản<br/><br/><b>Ràng buộc:</b><br/>• Không bắt buộc (có thể để trống)<br/>• Tối đa 500 ký tự<br/><br/><color=Gray>Lưu ý:</color> Ghi chú sẽ được lưu vào database khi lưu phiếu xuất nội bộ."
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
            // Reset flags để đảm bảo load lại khi form load
            _isWarehouseDataSourceLoaded = false;

            // Load warehouse datasource (xuất nội bộ không cần customer)
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
    /// Map StockInOutMaster entity sang XuatNoiBoMasterDto
    /// </summary>
    private XuatNoiBoMasterDto MapEntityToDto(StockInOutMaster entity)
    {
        if (entity == null) return null;

        var dto = new XuatNoiBoMasterDto
        {
            Id = entity.Id,
            StockOutNumber = entity.VocherNumber ?? string.Empty,
            StockOutDate = entity.StockInOutDate,
            LoaiNhapXuatKho = (LoaiNhapXuatKhoEnum)entity.StockInOutType,
            TrangThai = (TrangThaiPhieuNhapEnum)entity.VoucherStatus,
            WarehouseId = entity.WarehouseId,
            WarehouseCode = entity.CompanyBranch?.BranchCode ?? string.Empty,
            WarehouseName = entity.CompanyBranch?.BranchName ?? string.Empty,
            Notes = entity.Notes ?? string.Empty,
            NguoiNhanHang = entity.NguoiNhanHang ?? string.Empty,
            NguoiGiaoHang = entity.NguoiGiaoHang ?? string.Empty
        };

        // Gán giá trị tổng hợp từ entity (chỉ có TotalQuantity cho xuất nội bộ)
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
            StockOutNumberTextEdit,
            StockOutDateDateEdit,
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

    private void UcXuatNoiBoMasterDto_Load(object sender, EventArgs e)
    {
        // Control đã được load
        StockOutDateDateEdit.EditValue = DateTime.Now;
    }

    private void StockOutDateDateEdit_EditValueChanged(object sender, EventArgs e)
    {
        try
        {
            if (StockOutDateDateEdit.EditValue is DateTime selectedDate)
            {
                // Cập nhật ngày vào Entity
                _stockOutMaster.StockInOutDate = selectedDate;

                // Tạo số phiếu xuất tự động
                GenerateStockOutNumber(selectedDate);

                // Xóa lỗi validation nếu có
                dxErrorProvider1.SetError(StockOutDateDateEdit, string.Empty);
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
            if (WarehouseNameSearchLookupEdit.EditValue is Guid warehouseId && warehouseId != Guid.Empty)
            {
                // Cập nhật WarehouseId vào Entity
                _stockOutMaster.WarehouseId = warehouseId;
                _stockOutMaster.PartnerSiteId = null;

                // Xóa lỗi validation nếu có
                dxErrorProvider1.SetError(WarehouseNameSearchLookupEdit, string.Empty);
            }
            else
            {
                _stockOutMaster.WarehouseId = Guid.Empty;
            }
        }
        catch (Exception ex)
        {
            ShowError(ex, "Lỗi xử lý thay đổi kho");
        }
    }

    private void StockOutNumberTextEdit_EditValueChanged(object sender, EventArgs e)
    {
        try
        {
            if (StockOutNumberTextEdit != null)
            {
                _stockOutMaster.VocherNumber = StockOutNumberTextEdit.Text?.Trim();

                // Xóa lỗi validation nếu có
                dxErrorProvider1.SetError(StockOutNumberTextEdit, string.Empty);
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
                _stockOutMaster.NguoiNhanHang = NguoiNhanHangTextEdit.Text?.Trim();

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
                _stockOutMaster.NguoiGiaoHang = NguoiGiaoHangTextEdit.Text?.Trim();

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
            if (StockOutNumberTextEdit != null)
            {
                _stockOutMaster.VocherNumber = StockOutNumberTextEdit.Text?.Trim();
            }

            // Cập nhật từ DateEdit
            if (StockOutDateDateEdit is { EditValue: DateTime date })
            {
                _stockOutMaster.StockInOutDate = date;
            }

            // Cập nhật từ Warehouse SearchLookUpEdit
            if (WarehouseNameSearchLookupEdit != null)
            {
                if (WarehouseNameSearchLookupEdit.EditValue is Guid warehouseId && warehouseId != Guid.Empty)
                {
                    _stockOutMaster.WarehouseId = warehouseId;
                }
                else
                {
                    _stockOutMaster.WarehouseId = Guid.Empty;
                }
            }

            // Cập nhật từ NguoiNhanHangTextEdit
            if (NguoiNhanHangTextEdit != null)
            {
                _stockOutMaster.NguoiNhanHang = NguoiNhanHangTextEdit.Text?.Trim();
            }

            // Cập nhật từ NguoiGiaoHangTextEdit
            if (NguoiGiaoHangTextEdit != null)
            {
                _stockOutMaster.NguoiGiaoHang = NguoiGiaoHangTextEdit.Text?.Trim();
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
            var dto = MapEntityToDto(_stockOutMaster);
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

                firstErrorControl?.Focus();

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
            nameof(XuatNoiBoMasterDto.StockOutNumber) => StockOutNumberTextEdit,
            nameof(XuatNoiBoMasterDto.StockOutDate) => StockOutDateDateEdit,
            nameof(XuatNoiBoMasterDto.WarehouseId) => WarehouseNameSearchLookupEdit,
            nameof(XuatNoiBoMasterDto.WarehouseCode) => WarehouseNameSearchLookupEdit,
            nameof(XuatNoiBoMasterDto.WarehouseName) => WarehouseNameSearchLookupEdit,
            nameof(XuatNoiBoMasterDto.Notes) => NotesTextEdit,
            nameof(XuatNoiBoMasterDto.NguoiNhanHang) => NguoiNhanHangTextEdit,
            nameof(XuatNoiBoMasterDto.NguoiGiaoHang) => NguoiGiaoHangTextEdit,
            _ => null
        };
    }

    #endregion

    #region ========== PUBLIC METHODS ==========

    /// <summary>
    /// Lấy DTO từ Entity sau khi validate các trường bắt buộc
    /// </summary>
    /// <returns>XuatNoiBoMasterDto nếu validation thành công, null nếu có lỗi</returns>
    public XuatNoiBoMasterDto GetDto()
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
            _stockOutMaster.Id = _stockInOutMasterId;
            _stockOutMaster.StockInOutType = (int)LoaiNhapXuatKhoEnum.XuatNoiBo;

            // Convert Entity sang DTO để trả về
            return MapEntityToDto(_stockOutMaster);
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
    /// <exception cref="InvalidOperationException"></exception>
    public async Task LoadDataAsync(Guid stockInOutMasterId)
    {
        try
        {
            _stockInOutMasterId = stockInOutMasterId;

            // Lấy master entity từ BLL
            var masterEntity = _stockInBll.GetMasterById(stockInOutMasterId);

            // Gán entity vào _stockOutMaster
            _stockOutMaster = masterEntity ?? throw new InvalidOperationException($"Không tìm thấy phiếu xuất kho với ID: {stockInOutMasterId}");

            // Set dữ liệu cho các control đơn giản (không cần datasource)
            StockOutDateDateEdit.EditValue = masterEntity.StockInOutDate;
            StockOutNumberTextEdit.EditValue = masterEntity.VocherNumber;
            NotesTextEdit.EditValue = masterEntity.Notes;
            NguoiNhanHangTextEdit.EditValue = masterEntity.NguoiNhanHang;
            NguoiGiaoHangTextEdit.EditValue = masterEntity.NguoiGiaoHang;

            // Load datasource cho Warehouse trước khi set EditValue
            await LoadSingleWarehouseByIdAsync(masterEntity.WarehouseId);
            WarehouseNameSearchLookupEdit.EditValue = masterEntity.WarehouseId;

            // Cập nhật tổng hợp
            UpdateTotals(masterEntity.TotalQuantity);
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
            if (WarehouseNameSearchLookupEdit != null)
            {
                WarehouseNameSearchLookupEdit.EditValue = null;
            }

            // Nhập nội bộ không có Supplier control

            // Reset TextEdit
            if (StockOutNumberTextEdit != null)
            {
                StockOutNumberTextEdit.Text = string.Empty;
            }

            // Reset DateEdit
            if (StockOutDateDateEdit != null)
            {
                StockOutDateDateEdit.EditValue = DateTime.Now;
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
    public void UpdateTotals(decimal totalQuantity)
    {
        try
        {
            // Cập nhật trực tiếp vào Entity
            _stockOutMaster.TotalQuantity = totalQuantity;

            // Cập nhật trực tiếp vào các SimpleLabelItem để hiển thị
            if (TotalQuantitySimpleLabelItem != null)
            {
                TotalQuantitySimpleLabelItem.Text = totalQuantity.ToString("N2");
            }
        }
        catch (Exception ex)
        {
            ShowError(ex, "Lỗi cập nhật tổng hợp");
        }
    }


    #endregion

    #region ========== HELPER METHODS ==========

    /// <summary>
    /// Tạo số phiếu xuất kho tự động
    /// </summary>
    private void GenerateStockOutNumber(DateTime date)
    {
        try
        {
            // Chỉ tạo số phiếu nếu chưa có hoặc đang ở trạng thái tạo mới
            if (!string.IsNullOrWhiteSpace(_stockOutMaster.VocherNumber) &&
                _stockOutMaster.VoucherStatus != (int)TrangThaiPhieuNhapEnum.TaoMoi)
            {
                return;
            }

            // Gọi BLL để tạo số phiếu tự động (tự động xác định PNK hay PXK)
            var voucherNumber = _stockInOutMasterBll.GenerateVoucherNumber(date, LoaiNhapXuatKhoEnum.XuatNoiBo);
            
            // Cập nhật vào Entity và control
            _stockOutMaster.VocherNumber = voucherNumber;
            if (StockOutNumberTextEdit != null)
            {
                StockOutNumberTextEdit.Text = voucherNumber;
            }
        }
        catch (Exception)
        {
            //ShowError(ex, "Lỗi tạo số phiếu xuất kho");
        }
    }

    /// <summary>
    /// Hiển thị lỗi
    /// </summary>
    private void ShowError(Exception ex, string message)
    {
        MsgBox.ShowError($"{message}: {ex.Message}");
    }

    /// <summary>
    /// Hiển thị lỗi
    /// </summary>
    private void ShowError(string message)
    {
        MsgBox.ShowError(message);
    }

    #endregion
}