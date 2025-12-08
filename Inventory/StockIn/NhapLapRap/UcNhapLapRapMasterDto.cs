using DevExpress.XtraEditors;
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
using Bll.Inventory.InventoryManagement;
using Bll.Inventory.StockIn;
using Bll.MasterData.CompanyBll;
using Common.Utils;
using Dal.DataContext;
using DevExpress.XtraEditors.DXErrorProvider;
using DTO.Inventory.InventoryManagement;
using DTO.Inventory.StockIn.NhapLapRap;
using DTO.MasterData.Company;

namespace Inventory.StockIn.NhapLapRap
{
    public partial class UcNhapLapRapMasterDto : DevExpress.XtraEditors.XtraUserControl
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

        public UcNhapLapRapMasterDto()
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
                // Không chạy trong design mode để tránh lỗi load type
                if (DesignMode) return;

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
                // Trong design mode, không hiển thị lỗi
                if (!DesignMode)
                {
                    ShowError(ex, "Lỗi khởi tạo control");
                }
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
                StockInOutType = (int)LoaiNhapXuatKhoEnum.NhapSanPhamLapRap,
                VoucherStatus = (int)TrangThaiPhieuNhapEnum.TaoMoi,
                WarehouseId = Guid.Empty,
                PartnerSiteId = null, // Nhập lắp ráp không có customer
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
                // Không chạy trong design mode để tránh lỗi load type
                if (DesignMode) return;

                RequiredFieldHelper.MarkRequiredFields(
                    this,
                    typeof(NhapLapRapMasterDto),
                    nullValuePrompt: "Bắt buộc nhập",
                    logger: (msg, ex) => System.Diagnostics.Debug.WriteLine($"{msg}: {ex?.Message}")
                );

                // Xử lý đặc biệt cho WarehouseId (control là WarehouseNameSearchLookupEdit)
                // Vì RequiredFieldHelper không thể tự động match WarehouseId với WarehouseNameSearchLookupEdit
                if (ItemForWarehouseName != null && !ItemForWarehouseName.Text.Contains("*"))
                {
                    ItemForWarehouseName.AllowHtmlStringInCaption = true;
                    var baseCaption = string.IsNullOrWhiteSpace(ItemForWarehouseName.Text)
                        ? "Kho nhập"
                        : ItemForWarehouseName.Text;
                    ItemForWarehouseName.Text = baseCaption + @" <color=red>*</color>";
                }
            }
            catch (Exception ex)
            {
                // Trong design mode, không hiển thị lỗi
                if (!DesignMode)
                {
                    ShowError(ex, "Lỗi đánh dấu trường bắt buộc");
                }
            }
        }

        /// <summary>
        /// Thiết lập sự kiện
        /// </summary>
        private void SetupEvents()
        {
            try
            {
                Load += UcNhapLapRapMasterDto_Load;

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
            // SuperTip cho Số phiếu nhập lắp ráp
            if (StockInNumberTextEdit != null)
            {
                SuperToolTipHelper.SetTextEditSuperTip(
                    StockInNumberTextEdit,
                    title: @"<b><color=DarkBlue>📄 Số phiếu nhập lắp ráp</color></b>",
                    content:
                    @"Số phiếu nhập lắp ráp được tạo tự động theo format: <b>PNK-MMYY-NNXXX</b><br/><br/><b>Format:</b><br/>• PNK: Phiếu nhập kho<br/>• MM: Tháng (2 ký tự)<br/>• YY: Năm (2 ký tự cuối)<br/>• NN: Index của Loại nhập kho (2 ký tự)<br/>• XXX: Số thứ tự phiếu (3 ký tự từ 001 đến 999)<br/><br/><b>Chức năng:</b><br/>• Tự động tạo khi thay đổi ngày nhập lắp ráp<br/>• Tự động tạo khi thay đổi loại nhập kho<br/>• Query database để lấy số thứ tự tiếp theo<br/>• Đảm bảo số phiếu duy nhất trong cùng tháng/năm/loại<br/><br/><b>Ràng buộc:</b><br/>• <b>Bắt buộc nhập</b> (có dấu * đỏ)<br/>• Không được để trống<br/>• Tối đa 50 ký tự<br/><br/><color=Gray>Lưu ý:</color> Số phiếu nhập lắp ráp sẽ được lưu vào database khi lưu phiếu nhập."
                );
            }


            // SuperTip cho Người nhận hàng
            if (NguoiNhanHangTextEdit != null)
            {
                SuperToolTipHelper.SetTextEditSuperTip(
                    NguoiNhanHangTextEdit,
                    title: @"<b><color=DarkBlue>👤 Người nhận hàng</color></b>",
                    content:
                    @"Nhập tên người nhận hàng lắp ráp tại kho.<br/><br/><b>Chức năng:</b><br/>• Ghi nhận thông tin người nhận hàng lắp ráp<br/>• Hỗ trợ tra cứu và theo dõi<br/><br/><b>Ràng buộc:</b><br/>• <b>Bắt buộc nhập</b> (có dấu * đỏ)<br/>• Tối đa 500 ký tự<br/><br/><color=Gray>Lưu ý:</color> Thông tin này sẽ được lưu vào database khi lưu phiếu nhập lắp ráp."
                );
            }

            // SuperTip cho Người giao hàng
            if (NguoiGiaoHangTextEdit != null)
            {
                SuperToolTipHelper.SetTextEditSuperTip(
                    NguoiGiaoHangTextEdit,
                    title: @"<b><color=DarkBlue>🚚 Người giao hàng</color></b>",
                    content:
                    @"Nhập tên người giao hàng lắp ráp (từ chi nhánh/đơn vị).<br/><br/><b>Chức năng:</b><br/>• Ghi nhận thông tin người giao hàng lắp ráp<br/>• Hỗ trợ tra cứu và theo dõi<br/><br/><b>Ràng buộc:</b><br/>• <b>Bắt buộc nhập</b> (có dấu * đỏ)<br/>• Tối đa 500 ký tự<br/><br/><color=Gray>Lưu ý:</color> Thông tin này sẽ được lưu vào database khi lưu phiếu nhập lắp ráp."
                );
            }
        }

        /// <summary>
        /// Thiết lập SuperToolTip cho các DateEdit controls
        /// </summary>
        private void SetupDateEditSuperTips()
        {
            // SuperTip cho Ngày nhập lắp ráp
            if (StockInDateDateEdit != null)
            {
                SuperToolTipHelper.SetBaseEditSuperTip(
                    StockInDateDateEdit,
                    title: @"<b><color=DarkBlue>📅 Ngày nhập lắp ráp</color></b>",
                    content:
                    @"Chọn ngày nhập lắp ráp cho phiếu nhập.<br/><br/><b>Chức năng:</b><br/>• Xác định thời điểm nhập lắp ráp<br/>• Tự động tạo số phiếu nhập lắp ráp dựa trên ngày<br/>• Format số phiếu: PNK-MMYY-NNXXX (MM, YY từ ngày này)<br/>• Query database để lấy số thứ tự tiếp theo trong tháng/năm<br/><br/><b>Ràng buộc:</b><br/>• <b>Bắt buộc nhập</b> (có dấu * đỏ)<br/>• Không được để trống<br/>• Mặc định: Ngày hiện tại<br/><br/><b>Validation:</b><br/>• Kiểm tra rỗng khi validating<br/>• Hiển thị lỗi qua ErrorProvider nếu không hợp lệ<br/><br/><color=Gray>Lưu ý:</color> Khi thay đổi ngày nhập lắp ráp, hệ thống sẽ tự động tạo lại số phiếu nhập lắp ráp theo format mới."
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
                    content:
                    @"Chọn kho nhập lắp ráp từ danh sách chi nhánh (Company Branch) đang hoạt động.<br/><br/><b>Chức năng:</b><br/>• Chọn kho nhập lắp ráp<br/>• Hiển thị thông tin kho dạng HTML (mã, tên)<br/>• Tự động cập nhật WarehouseId vào Entity<br/><br/><b>Ràng buộc:</b><br/>• <b>Bắt buộc chọn</b> (có dấu * đỏ)<br/>• Không được để trống<br/>• Chỉ hiển thị các chi nhánh đang hoạt động (IsActive = true)<br/><br/><b>Data Source:</b><br/>• Load từ CompanyBranchBll.GetAll()<br/>• Filter chỉ lấy các chi nhánh đang hoạt động<br/>• Sắp xếp theo tên chi nhánh<br/><br/><b>Validation:</b><br/>• Kiểm tra rỗng khi validating<br/>• Hiển thị lỗi qua ErrorProvider nếu không hợp lệ<br/><br/><color=Gray>Lưu ý:</color> Kho nhập lắp ráp sẽ được lưu vào database khi lưu phiếu nhập lắp ráp."
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
                    @"Nhập ghi chú hoặc mô tả bổ sung cho phiếu nhập lắp ráp.<br/><br/><b>Chức năng:</b><br/>• Lưu thông tin bổ sung về phiếu nhập lắp ráp<br/>• Ghi chú về lý do nhập lắp ráp, điều kiện nhập kho, nguồn gốc hàng hóa, v.v.<br/>• Hỗ trợ nhiều dòng văn bản<br/><br/><b>Ràng buộc:</b><br/>• Không bắt buộc (có thể để trống)<br/>• Tối đa 500 ký tự<br/><br/><color=Gray>Lưu ý:</color> Ghi chú sẽ được lưu vào database khi lưu phiếu nhập lắp ráp."
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

                // Load warehouse datasource (nhập lắp ráp không cần customer)
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
        /// Map StockInOutMaster entity sang NhapLapRapMasterDto
        /// </summary>
        private NhapLapRapMasterDto MapEntityToDto(StockInOutMaster entity)
        {
            if (entity == null) return null;

            var dto = new NhapLapRapMasterDto
            {
                Id = entity.Id,
                StockInNumber = entity.VocherNumber ?? string.Empty,
                StockInDate = entity.StockInOutDate,
                LoaiNhapXuatKho = (LoaiNhapXuatKhoEnum)entity.StockInOutType,
                TrangThai = (TrangThaiPhieuNhapEnum)entity.VoucherStatus,
                WarehouseId = entity.WarehouseId,
                WarehouseCode = entity.CompanyBranch?.BranchCode ?? string.Empty,
                WarehouseName = entity.CompanyBranch?.BranchName ?? string.Empty,
                Notes = entity.Notes ?? string.Empty,
                NguoiNhanHang = entity.NguoiNhanHang ?? string.Empty,
                NguoiGiaoHang = entity.NguoiGiaoHang ?? string.Empty
            };

            // Gán giá trị tổng hợp từ entity (chỉ có TotalQuantity cho nhập lắp ráp)
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

        private void UcNhapLapRapMasterDto_Load(object sender, EventArgs e)
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
                    // Cập nhật ngày vào Entity
                    _stockInMaster.StockInOutDate = selectedDate;

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
                    // Cập nhật WarehouseId vào Entity
                    _stockInMaster.WarehouseId = warehouseId;
                    _stockInMaster.PartnerSiteId = null;

                    // Xóa lỗi validation nếu có
                    dxErrorProvider1.SetError(WarehouseNameSearchLookupEdit, string.Empty);
                }
                else
                {
                    _stockInMaster.WarehouseId = Guid.Empty;
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi xử lý thay đổi kho");
            }
        }

        private void StockInNumberTextEdit_EditValueChanged(object sender, EventArgs e)
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
                ShowError(ex, "Lỗi xử lý thay đổi số phiếu nhập");
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
                if (StockInDateDateEdit is { EditValue: DateTime date })
                {
                    _stockInMaster.StockInOutDate = date;
                }

                // Cập nhật từ Warehouse SearchLookUpEdit
                if (WarehouseNameSearchLookupEdit != null)
                {
                    if (WarehouseNameSearchLookupEdit.EditValue is Guid warehouseId && warehouseId != Guid.Empty)
                    {
                        _stockInMaster.WarehouseId = warehouseId;
                    }
                    else
                    {
                        _stockInMaster.WarehouseId = Guid.Empty;
                    }
                }

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
                nameof(NhapLapRapMasterDto.StockInNumber) => StockInNumberTextEdit,
                nameof(NhapLapRapMasterDto.StockInDate) => StockInDateDateEdit,
                nameof(NhapLapRapMasterDto.WarehouseId) => WarehouseNameSearchLookupEdit,
                nameof(NhapLapRapMasterDto.WarehouseCode) => WarehouseNameSearchLookupEdit,
                nameof(NhapLapRapMasterDto.WarehouseName) => WarehouseNameSearchLookupEdit,
                nameof(NhapLapRapMasterDto.Notes) => NotesTextEdit,
                nameof(NhapLapRapMasterDto.NguoiNhanHang) => NguoiNhanHangTextEdit,
                nameof(NhapLapRapMasterDto.NguoiGiaoHang) => NguoiGiaoHangTextEdit,
                _ => null
            };
        }

        #endregion

        #region ========== PUBLIC METHODS ==========

        /// <summary>
        /// Lấy DTO từ Entity sau khi validate các trường bắt buộc
        /// </summary>
        /// <returns>NhapLapRapMasterDto nếu validation thành công, null nếu có lỗi</returns>
        public NhapLapRapMasterDto GetDto()
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
                _stockInMaster.StockInOutType = (int)LoaiNhapXuatKhoEnum.NhapSanPhamLapRap;

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
        /// Load dữ liệu master từ ID phiếu nhập kho
        /// </summary>
        /// <param name="stockInOutMasterId">ID phiếu nhập kho</param>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task LoadDataAsync(Guid stockInOutMasterId)
        {
            try
            {
                _stockInOutMasterId = stockInOutMasterId;

                // Lấy master entity từ BLL
                var masterEntity = _stockInBll.GetMasterById(stockInOutMasterId);

                // Gán entity vào _stockInMaster
                _stockInMaster = masterEntity ??
                                  throw new InvalidOperationException(
                                      $"Không tìm thấy phiếu nhập kho với ID: {stockInOutMasterId}");

                // Set dữ liệu cho các control đơn giản (không cần datasource)
                StockInDateDateEdit.EditValue = masterEntity.StockInOutDate;
                StockInNumberTextEdit.EditValue = masterEntity.VocherNumber;
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
                // Khởi tạo lại Entity
                InitializeEntity();

                // Reset tất cả các controls về giá trị mặc định
                // Reset SearchLookUpEdit - phải set EditValue = null để xóa selection
                if (WarehouseNameSearchLookupEdit != null)
                {
                    WarehouseNameSearchLookupEdit.EditValue = null;
                }

                // Xuất lắp ráp không có Supplier control

                // Reset TextEdit
                if (StockInNumberTextEdit != null)
                {
                    StockInNumberTextEdit.Text = string.Empty;
                }

                // Reset DateEdit
                if (StockInDateDateEdit != null)
                {
                    StockInDateDateEdit.EditValue = DateTime.Now;
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
        public void UpdateTotals(decimal totalQuantity)
        {
            try
            {
                // Cập nhật trực tiếp vào Entity
                _stockInMaster.TotalQuantity = totalQuantity;

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
        /// Tạo số phiếu nhập kho tự động
        /// </summary>
        private void GenerateStockInNumber(DateTime date)
        {
            try
            {
                // Chỉ tạo số phiếu nếu chưa có hoặc đang ở trạng thái tạo mới
                if (!string.IsNullOrWhiteSpace(_stockInMaster.VocherNumber) &&
                    _stockInMaster.VoucherStatus != (int)TrangThaiPhieuNhapEnum.TaoMoi)
                {
                    return;
                }

                // Gọi BLL để tạo số phiếu tự động (tự động xác định PNK hay PXK)
                var voucherNumber =
                    _stockInOutMasterBll.GenerateVoucherNumber(date, LoaiNhapXuatKhoEnum.NhapSanPhamLapRap);

                // Cập nhật vào Entity và control
                _stockInMaster.VocherNumber = voucherNumber;
                if (StockInNumberTextEdit != null)
                {
                    StockInNumberTextEdit.Text = voucherNumber;
                }
            }
            catch (Exception)
            {
                //ShowError(ex, "Lỗi tạo số phiếu nhập kho");
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
}
