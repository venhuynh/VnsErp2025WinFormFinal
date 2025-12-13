using Bll.Inventory.InventoryManagement;
using Bll.MasterData.CompanyBll;
using Bll.MasterData.CustomerBll;
using Common.Utils;
using Dal.DataContext;
using DevExpress.XtraEditors.DXErrorProvider;
using DTO.Inventory.StockIn;
using DTO.Inventory.StockOut.XuatThietBiChoThueMuon;
using DTO.MasterData.Company;
using DTO.MasterData.CustomerPartner;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bll.Inventory.StockInOut;
using DTO.Inventory.InventoryManagement;

namespace Inventory.StockOut.XuatChoThueMuon;

public partial class UcXuatThietBiChoThueMuonMasterDto : DevExpress.XtraEditors.XtraUserControl
{
    #region ========== KHAI BÁO BIẾN ==========

    /// <summary>
    /// StockInOutMaster entity
    /// </summary>
    private StockInOutMaster _stockOutMaster;

    /// <summary>
    /// Business Logic Layer cho CompanyBranch (dùng cho Warehouse lookup)
    /// </summary>
    private readonly CompanyBranchBll _companyBranchBll = new();

    /// <summary>
    /// Business Logic Layer cho BusinessPartnerSite (dùng cho Customer lookup)
    /// </summary>
    private readonly BusinessPartnerSiteBll _businessPartnerSiteBll = new();

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

    /// <summary>
    /// Flag đánh dấu Customer datasource đã được load chưa
    /// </summary>
    private bool _isCustomerDataSourceLoaded;

    private Guid _stockInOutMasterId = Guid.Empty;

    #endregion

    #region ========== CONSTRUCTOR ==========

    public UcXuatThietBiChoThueMuonMasterDto()
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

            // Đánh dấu các trường bắt buộc
            MarkRequiredFields();

            // Setup events
            SetupEvents();
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
            StockInOutType = (int)LoaiNhapXuatKhoEnum.XuatThietBiMuonThue,
            VoucherStatus = (int)TrangThaiPhieuNhapEnum.TaoMoi,
            WarehouseId = Guid.Empty,
            PurchaseOrderId = null,
            PartnerSiteId = null, // Dùng để lưu CustomerId
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
    /// Đánh dấu các trường bắt buộc
    /// </summary>
    private void MarkRequiredFields()
    {
        try
        {
            RequiredFieldHelper.MarkRequiredFields(
                this,
                typeof(XuatThietBiChoThueMuonMasterDto),
                nullValuePrompt: "Bắt buộc nhập",
                logger: (msg, ex) => System.Diagnostics.Debug.WriteLine($"{msg}: {ex?.Message}")
            );

            // Xử lý đặc biệt cho WarehouseId
            if (ItemForWarehouseName != null && !ItemForWarehouseName.Text.Contains("*"))
            {
                ItemForWarehouseName.AllowHtmlStringInCaption = true;
                var baseCaption = string.IsNullOrWhiteSpace(ItemForWarehouseName.Text)
                    ? "Kho xuất"
                    : ItemForWarehouseName.Text;
                ItemForWarehouseName.Text = baseCaption + @" <color=red>*</color>";
            }

            // Xử lý đặc biệt cho CustomerId
            if (ItemForCustomerName != null && !ItemForCustomerName.Text.Contains("*"))
            {
                ItemForCustomerName.AllowHtmlStringInCaption = true;
                var baseCaption = string.IsNullOrWhiteSpace(ItemForCustomerName.Text)
                    ? "Khách hàng"
                    : ItemForCustomerName.Text;
                ItemForCustomerName.Text = baseCaption + @" <color=red>*</color>";
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
            Load += UcXuatThietBiChoThueMuonMasterDto_Load;

            //Sự kiện của WarehouseNameSearchLookupEdit
            WarehouseNameSearchLookupEdit.Popup += WarehouseNameSearchLookupEdit_Popup;
            WarehouseNameSearchLookupEdit.EditValueChanged += WarehouseNameSearchLookupEdit_EditValueChanged;

            //Sự kiện của CustomerNameSearchLookupEdit
            CustomerNameSearchLookupEdit.Popup += CustomerNameSearchLookupEdit_Popup;
            CustomerNameSearchLookupEdit.EditValueChanged += CustomerNameSearchLookupEdit_EditValueChanged;

            StockOutDateDateEdit.EditValueChanged += StockOutDateDateEdit_EditValueChanged;
            StockOutNumberTextEdit.EditValueChanged += StockOutNumberTextEdit_EditValueChanged;
            NguoiNhanHangTextEdit.EditValueChanged += NguoiNhanHangTextEdit_EditValueChanged;
            NguoiGiaoHangTextEdit.EditValueChanged += NguoiGiaoHangTextEdit_EditValueChanged;
        }
        catch (Exception ex)
        {
            ShowError(ex, "Lỗi thiết lập sự kiện");
        }
    }

    #endregion

    #region ========== EVENT HANDLERS ==========

    private void UcXuatThietBiChoThueMuonMasterDto_Load(object sender, EventArgs e)
    {
        StockOutDateDateEdit.EditValue = DateTime.Now;
    }

    private async void WarehouseNameSearchLookupEdit_Popup(object sender, EventArgs e)
    {
        try
        {
            if (!_isWarehouseDataSourceLoaded)
            {
                await LoadWarehouseDataSourceAsync();
            }
        }
        catch (Exception ex)
        {
            ShowError(ex, "Lỗi tải dữ liệu kho");
        }
    }

    private async void CustomerNameSearchLookupEdit_Popup(object sender, EventArgs e)
    {
        try
        {
            if (!_isCustomerDataSourceLoaded)
            {
                await LoadCustomerDataSourceAsync();
            }
        }
        catch (Exception ex)
        {
            ShowError(ex, "Lỗi tải dữ liệu khách hàng");
        }
    }

    private void WarehouseNameSearchLookupEdit_EditValueChanged(object sender, EventArgs e)
    {
        try
        {
            if (WarehouseNameSearchLookupEdit.EditValue is Guid warehouseId && warehouseId != Guid.Empty)
            {
                _stockOutMaster.WarehouseId = warehouseId;
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

    private void CustomerNameSearchLookupEdit_EditValueChanged(object sender, EventArgs e)
    {
        try
        {
            if (CustomerNameSearchLookupEdit.EditValue is Guid customerId && customerId != Guid.Empty)
            {
                _stockOutMaster.PartnerSiteId = customerId;
                dxErrorProvider1.SetError(CustomerNameSearchLookupEdit, string.Empty);
            }
            else
            {
                _stockOutMaster.PartnerSiteId = null;
            }
        }
        catch (Exception ex)
        {
            ShowError(ex, "Lỗi xử lý thay đổi khách hàng");
        }
    }

    private void StockOutDateDateEdit_EditValueChanged(object sender, EventArgs e)
    {
        try
        {
            if (StockOutDateDateEdit.EditValue is DateTime selectedDate)
            {
                _stockOutMaster.StockInOutDate = selectedDate;
                GenerateStockOutNumber(selectedDate);
                dxErrorProvider1.SetError(StockOutDateDateEdit, string.Empty);
            }
        }
        catch (Exception ex)
        {
            ShowError(ex, "Lỗi tạo số phiếu xuất");
        }
    }

    private void StockOutNumberTextEdit_EditValueChanged(object sender, EventArgs e)
    {
        try
        {
            _stockOutMaster.VocherNumber = StockOutNumberTextEdit.Text?.Trim();
            dxErrorProvider1.SetError(StockOutNumberTextEdit, string.Empty);
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
            _stockOutMaster.NguoiNhanHang = NguoiNhanHangTextEdit.Text?.Trim();
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
            _stockOutMaster.NguoiGiaoHang = NguoiGiaoHangTextEdit.Text?.Trim();
        }
        catch (Exception ex)
        {
            ShowError(ex, "Lỗi xử lý thay đổi người giao hàng");
        }
    }

    #endregion

    #region ========== DATA LOADING ==========

    private async Task LoadWarehouseDataSourceAsync()
    {
        try
        {
            var branches = await Task.Run(() => _companyBranchBll.GetAll());
            var branchDtos = branches.Select(b => b.ToDto()).ToList();
            companyBranchDtoBindingSource.DataSource = branchDtos;
            _isWarehouseDataSourceLoaded = true;
        }
        catch (Exception ex)
        {
            ShowError(ex, "Lỗi tải dữ liệu kho");
        }
    }

    private async Task LoadCustomerDataSourceAsync()
    {
        try
        {
            var sites = await Task.Run(() => _businessPartnerSiteBll.GetAll());
            var siteDtos = sites.ToSiteListDtos().ToList();
            businessPartnerSiteListDtoBindingSource.DataSource = siteDtos;
            _isCustomerDataSourceLoaded = true;
        }
        catch (Exception ex)
        {
            ShowError(ex, "Lỗi tải dữ liệu khách hàng");
        }
    }

    private async Task LoadSingleWarehouseByIdAsync(Guid warehouseId)
    {
        try
        {
            if (warehouseId == Guid.Empty)
            {
                companyBranchDtoBindingSource.DataSource = new List<CompanyBranchDto>();
                _isWarehouseDataSourceLoaded = false;
                return;
            }

            var branch = await Task.Run(() => _companyBranchBll.GetById(warehouseId));
            if (branch != null)
            {
                var warehouseDto = branch.ToDto();
                companyBranchDtoBindingSource.DataSource = new List<CompanyBranchDto> { warehouseDto };
                _isWarehouseDataSourceLoaded = false;
            }
            else
            {
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

    private async Task LoadSingleCustomerByIdAsync(Guid customerId)
    {
        try
        {
            if (customerId == Guid.Empty)
            {
                businessPartnerSiteListDtoBindingSource.DataSource = new List<BusinessPartnerSiteListDto>();
                _isCustomerDataSourceLoaded = false;
                return;
            }

            var site = await Task.Run(() => _businessPartnerSiteBll.GetById(customerId));
            if (site != null)
            {
                var siteDtos = new List<BusinessPartnerSite> { site }.ToSiteListDtos().ToList();
                businessPartnerSiteListDtoBindingSource.DataSource = siteDtos;
                _isCustomerDataSourceLoaded = false;
            }
            else
            {
                businessPartnerSiteListDtoBindingSource.DataSource = new List<BusinessPartnerSiteListDto>();
                _isCustomerDataSourceLoaded = false;
            }
        }
        catch (Exception ex)
        {
            ShowError(ex, "Lỗi tải dữ liệu khách hàng");
            throw;
        }
    }

    #endregion

    #region ========== DTO CONVERSION ==========

    private XuatThietBiChoThueMuonMasterDto MapEntityToDto(StockInOutMaster entity)
    {
        if (entity == null) return null;

        var dto = new XuatThietBiChoThueMuonMasterDto
        {
            Id = entity.Id,
            StockOutNumber = entity.VocherNumber ?? string.Empty,
            StockOutDate = entity.StockInOutDate,
            LoaiNhapXuatKho = (LoaiNhapXuatKhoEnum)entity.StockInOutType,
            TrangThai = (TrangThaiPhieuNhapEnum)entity.VoucherStatus,
            WarehouseId = entity.WarehouseId,
            WarehouseCode = entity.CompanyBranch?.BranchCode ?? string.Empty,
            WarehouseName = entity.CompanyBranch?.BranchName ?? string.Empty,
            CustomerId = entity.PartnerSiteId,
            CustomerName = entity.BusinessPartnerSite?.BusinessPartner?.PartnerName ??
                           entity.BusinessPartnerSite?.SiteName ??
                           string.Empty,
            Notes = entity.Notes ?? string.Empty,
            NguoiNhanHang = entity.NguoiNhanHang ?? string.Empty,
            NguoiGiaoHang = entity.NguoiGiaoHang ?? string.Empty
        };

        // Set tổng số lượng bằng method SetTotals (vì TotalQuantity là read-only)
        dto.SetTotals(entity.TotalQuantity);

        return dto;
    }

    private void UpdateDtoFromControls()
    {
        try
        {
            if (StockOutNumberTextEdit != null)
            {
                _stockOutMaster.VocherNumber = StockOutNumberTextEdit.Text?.Trim();
            }

            if (StockOutDateDateEdit is { EditValue: DateTime date })
            {
                _stockOutMaster.StockInOutDate = date;
            }

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

            if (CustomerNameSearchLookupEdit != null)
            {
                if (CustomerNameSearchLookupEdit.EditValue is Guid customerId && customerId != Guid.Empty)
                {
                    _stockOutMaster.PartnerSiteId = customerId;
                }
                else
                {
                    _stockOutMaster.PartnerSiteId = null;
                }
            }

            if (NotesTextEdit != null)
            {
                _stockOutMaster.Notes = NotesTextEdit.Text?.Trim();
            }

            if (NguoiNhanHangTextEdit != null)
            {
                _stockOutMaster.NguoiNhanHang = NguoiNhanHangTextEdit.Text?.Trim();
            }

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

    private bool ValidateInput()
    {
        try
        {
            dxErrorProvider1.ClearErrors();

            var dto = MapEntityToDto(_stockOutMaster);
            if (dto == null)
            {
                ShowError("Không thể convert entity sang DTO để validate");
                return false;
            }

            var context = new ValidationContext(dto, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(dto, context, results, validateAllProperties: true);

            if (!isValid)
            {
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

    private Control FindControlByPropertyName(string propertyName)
    {
        return propertyName switch
        {
            nameof(XuatThietBiChoThueMuonMasterDto.StockOutNumber) => StockOutNumberTextEdit,
            nameof(XuatThietBiChoThueMuonMasterDto.StockOutDate) => StockOutDateDateEdit,
            nameof(XuatThietBiChoThueMuonMasterDto.WarehouseId) => WarehouseNameSearchLookupEdit,
            nameof(XuatThietBiChoThueMuonMasterDto.WarehouseCode) => WarehouseNameSearchLookupEdit,
            nameof(XuatThietBiChoThueMuonMasterDto.WarehouseName) => WarehouseNameSearchLookupEdit,
            nameof(XuatThietBiChoThueMuonMasterDto.CustomerId) => CustomerNameSearchLookupEdit,
            nameof(XuatThietBiChoThueMuonMasterDto.CustomerName) => CustomerNameSearchLookupEdit,
            nameof(XuatThietBiChoThueMuonMasterDto.Notes) => NotesTextEdit,
            nameof(XuatThietBiChoThueMuonMasterDto.NguoiNhanHang) => NguoiNhanHangTextEdit,
            nameof(XuatThietBiChoThueMuonMasterDto.NguoiGiaoHang) => NguoiGiaoHangTextEdit,
            _ => null
        };
    }

    #endregion

    #region ========== PUBLIC METHODS ==========

    public XuatThietBiChoThueMuonMasterDto GetDto()
    {
        try
        {
            UpdateDtoFromControls();

            if (!ValidateInput())
            {
                return null;
            }

            _stockOutMaster.Id = _stockInOutMasterId;
            _stockOutMaster.StockInOutType = (int)LoaiNhapXuatKhoEnum.XuatThietBiMuonThue;

            return MapEntityToDto(_stockOutMaster);
        }
        catch (Exception ex)
        {
            ShowError(ex, "Lỗi lấy dữ liệu");
            return null;
        }
    }

    public async Task LoadDataAsync(Guid stockInOutMasterId)
    {
        try
        {
            _stockInOutMasterId = stockInOutMasterId;

            var masterEntity = _stockInBll.GetMasterById(stockInOutMasterId);

            _stockOutMaster = masterEntity ?? throw new InvalidOperationException($"Không tìm thấy phiếu xuất kho với ID: {stockInOutMasterId}");

            StockOutDateDateEdit.EditValue = masterEntity.StockInOutDate;
            StockOutNumberTextEdit.EditValue = masterEntity.VocherNumber;
            NotesTextEdit.EditValue = masterEntity.Notes;
            NguoiNhanHangTextEdit.EditValue = masterEntity.NguoiNhanHang;
            NguoiGiaoHangTextEdit.EditValue = masterEntity.NguoiGiaoHang;

            await LoadSingleWarehouseByIdAsync(masterEntity.WarehouseId);
            WarehouseNameSearchLookupEdit.EditValue = masterEntity.WarehouseId;

            if (masterEntity.PartnerSiteId.HasValue)
            {
                await LoadSingleCustomerByIdAsync(masterEntity.PartnerSiteId.Value);
                CustomerNameSearchLookupEdit.EditValue = masterEntity.PartnerSiteId;
            }
            else
            {
                CustomerNameSearchLookupEdit.EditValue = null;
            }

            UpdateTotals(
                masterEntity.TotalQuantity,
                0,
                0,
                0
            );
        }
        catch (Exception ex)
        {
            ShowError(ex, "Lỗi tải dữ liệu phiếu xuất kho");
            throw;
        }
    }

    public void ClearData()
    {
        try
        {
            InitializeEntity();

            if (WarehouseNameSearchLookupEdit != null)
            {
                WarehouseNameSearchLookupEdit.EditValue = null;
            }

            if (CustomerNameSearchLookupEdit != null)
            {
                CustomerNameSearchLookupEdit.EditValue = null;
            }

            if (StockOutNumberTextEdit != null)
            {
                StockOutNumberTextEdit.Text = string.Empty;
            }

            if (StockOutDateDateEdit != null)
            {
                StockOutDateDateEdit.EditValue = DateTime.Now;
                GenerateStockOutNumber(DateTime.Now);
            }

            if (NotesTextEdit != null)
            {
                NotesTextEdit.Text = string.Empty;
            }

            if (NguoiNhanHangTextEdit != null)
            {
                NguoiNhanHangTextEdit.Text = string.Empty;
            }

            if (NguoiGiaoHangTextEdit != null)
            {
                NguoiGiaoHangTextEdit.Text = string.Empty;
            }

            dxErrorProvider1.ClearErrors();
            UpdateTotals(0, 0, 0, 0);
        }
        catch (Exception ex)
        {
            ShowError(ex, "Lỗi xóa dữ liệu");
        }
    }

    public void UpdateTotals(decimal totalQuantity, decimal totalAmount, decimal totalVat,
        decimal totalAmountIncludedVat)
    {
        try
        {
            _stockOutMaster.TotalQuantity = totalQuantity;
            _stockOutMaster.TotalAmount = totalAmount;
            _stockOutMaster.TotalVat = totalVat;
            _stockOutMaster.TotalAmountIncludedVat = totalAmountIncludedVat;

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

    private void GenerateStockOutNumber(DateTime date)
    {
        try
        {
            var voucherNumber = _stockInOutMasterBll.GenerateVoucherNumber(date, LoaiNhapXuatKhoEnum.XuatThietBiMuonThue);
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

    private void ShowError(Exception ex, string message)
    {
        MsgBox.ShowError($"{message}: {ex.Message}");
    }

    private void ShowError(string message)
    {
        MsgBox.ShowError(message);
    }

    #endregion
}
