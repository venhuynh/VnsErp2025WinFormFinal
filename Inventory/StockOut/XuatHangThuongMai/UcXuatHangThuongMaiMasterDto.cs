using Bll.Inventory.InventoryManagement;
using Bll.Inventory.StockIn;
using Bll.MasterData.CompanyBll;
using Bll.MasterData.CustomerBll;
using Common.Utils;
using Dal.DataContext;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using DTO.Inventory.StockIn;
using DTO.Inventory.StockOut.XuatHangThuongMai;
using DTO.MasterData.Company;
using DTO.MasterData.CustomerPartner;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventory.StockOut.XuatHangThuongMai;

public partial class UcXuatHangThuongMaiMasterDto : XtraUserControl
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
    private readonly StockInBll _stockInBll = new();

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

    public UcXuatHangThuongMaiMasterDto()
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
            StockInOutType = (int)LoaiNhapXuatKhoEnum.XuatHangThuongMai,
            VoucherStatus = (int)TrangThaiPhieuNhapEnum.TaoMoi,
            WarehouseId = Guid.Empty,
            PurchaseOrderId = null, // Dùng để lưu SalesOrderId
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
                typeof(XuatHangThuongMaiMasterDto),
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
            Load += UcXuatHangThuongMaiMasterDto_Load;

            //Sự kiện của WarehouseNameSearchLookupEdit
            WarehouseNameSearchLookupEdit.Popup += WarehouseNameSearchLookupEdit_Popup;
            WarehouseNameSearchLookupEdit.EditValueChanged += WarehouseNameSearchLookupEdit_EditValueChanged;

            //Sự kiện của CustomerNameSearchLookupEdit
            CustomerNameSearchLookupEdit.Popup += CustomerNameSearchLookupEdit_Popup;
            CustomerNameSearchLookupEdit.EditValueChanged += CustomerNameSearchLookupEdit_EditValueChanged;

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

    private async void CustomerNameSearchLookupEdit_Popup(object sender, EventArgs e)
    {
        try
        {
            // Chỉ load nếu chưa load hoặc datasource rỗng
            if (!_isCustomerDataSourceLoaded ||
                businessPartnerSiteListDtoBindingSource.DataSource == null ||
                (businessPartnerSiteListDtoBindingSource.DataSource is List<BusinessPartnerSiteListDto> list &&
                 list.Count == 0))
            {
                await LoadCustomerDataSourceAsync();
                _isCustomerDataSourceLoaded = true;
            }
        }
        catch (Exception ex)
        {
            ShowError(ex, "Lỗi tải dữ liệu khách hàng");
        }
    }

    private async void WarehouseNameSearchLookupEdit_Popup(object sender, EventArgs e)
    {
        try
        {
            // Chỉ load nếu chưa load hoặc datasource rỗng
            if (_isWarehouseDataSourceLoaded &&
                companyBranchDtoBindingSource.DataSource != null &&
                companyBranchDtoBindingSource.DataSource is not List<CompanyBranchDto> { Count: 0 }) return;

            await LoadWarehouseDataSourceAsync();

            _isWarehouseDataSourceLoaded = true;
        }
        catch (Exception ex)
        {
            ShowError(ex, "Lỗi tải dữ liệu kho");
        }
    }

    #endregion

    #region ========== DATA LOADING ==========

    /// <summary>
    /// Load dữ liệu lookup (Warehouse và Customer)
    /// Method này được gọi từ form khi FormLoad
    /// </summary>
    public async Task LoadLookupDataAsync()
    {
        try
        {
            // Reset flags để đảm bảo load lại khi form load
            _isWarehouseDataSourceLoaded = false;
            _isCustomerDataSourceLoaded = false;

            // Load cả 2 datasource song song để tối ưu performance
            await Task.WhenAll(
                LoadWarehouseDataSourceAsync(forceRefresh: true),
                LoadCustomerDataSourceAsync(forceRefresh: true)
            );
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
    /// Load datasource cho Customer (BusinessPartnerSite) - Load toàn bộ danh sách
    /// </summary>
    /// <param name="forceRefresh">Nếu true, sẽ load lại từ database ngay cả khi đã load trước đó</param>
    private async Task LoadCustomerDataSourceAsync(bool forceRefresh = false)
    {
        try
        {
            // Nếu đã load và không force refresh, không load lại
            if (_isCustomerDataSourceLoaded && !forceRefresh &&
                businessPartnerSiteListDtoBindingSource.DataSource != null &&
                businessPartnerSiteListDtoBindingSource.DataSource is List<BusinessPartnerSiteListDto> existingList &&
                existingList.Count > 0)
            {
                return;
            }

            // Load danh sách BusinessPartnerSite từ BusinessPartnerSiteBll (dùng cho Customer lookup)
            var sites = await Task.Run(() => _businessPartnerSiteBll.GetAll());
            var siteDtos = sites
                .Where(s => s.IsActive) // Chỉ lấy các chi nhánh đang hoạt động
                .ToSiteListDtos()
                .OrderBy(s => s.SiteName)
                .ToList();

            businessPartnerSiteListDtoBindingSource.DataSource = siteDtos;
            _isCustomerDataSourceLoaded = true;
        }
        catch (Exception ex)
        {
            ShowError(ex, "Lỗi tải dữ liệu khách hàng");
            throw;
        }
    }

    /// <summary>
    /// Map StockInOutMaster entity sang XuatHangThuongMaiMasterDto
    /// </summary>
    private XuatHangThuongMaiMasterDto MapEntityToDto(StockInOutMaster entity)
    {
        if (entity == null) return null;

        var dto = new XuatHangThuongMaiMasterDto
        {
            Id = entity.Id,
            StockOutNumber = entity.VocherNumber ?? string.Empty,
            StockOutDate = entity.StockInOutDate,
            LoaiNhapXuatKho = (LoaiNhapXuatKhoEnum)entity.StockInOutType,
            TrangThai = (TrangThaiPhieuNhapEnum)entity.VoucherStatus,
            WarehouseId = entity.WarehouseId,
            WarehouseCode = entity.CompanyBranch?.BranchCode ?? string.Empty,
            WarehouseName = entity.CompanyBranch?.BranchName ?? string.Empty,
            SalesOrderId = entity.PurchaseOrderId, // Dùng PurchaseOrderId để lưu SalesOrderId
            SalesOrderNumber = string.Empty, // TODO: Lấy từ SalesOrder entity nếu cần
            CustomerId = entity.PartnerSiteId, // Dùng PartnerSiteId để lưu CustomerId
            CustomerName = entity.BusinessPartnerSite?.BusinessPartner?.PartnerName ??
                           entity.BusinessPartnerSite?.SiteName ??
                           string.Empty,
            Notes = entity.Notes ?? string.Empty,
            NguoiNhanHang = entity.NguoiNhanHang ?? string.Empty,
            NguoiGiaoHang = entity.NguoiGiaoHang ?? string.Empty
        };

        // Gán các giá trị tổng hợp từ entity
        dto.SetTotals(
            entity.TotalQuantity,
            entity.TotalAmount,
            entity.TotalVat,
            entity.TotalAmountIncludedVat
        );

        return dto;
    }

    #endregion

    #region ========== EVENT HANDLERS ==========

    private void UcXuatHangThuongMaiMasterDto_Load(object sender, EventArgs e)
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

    private void CustomerNameSearchLookupEdit_EditValueChanged(object sender, EventArgs e)
    {
        try
        {
            if (CustomerNameSearchLookupEdit.EditValue is Guid customerId && customerId != Guid.Empty)
            {
                // Cập nhật PartnerSiteId vào Entity (dùng để lưu CustomerId)
                _stockOutMaster.PartnerSiteId = customerId;

                // Xóa lỗi validation nếu có
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

            // Cập nhật từ SalesOrder SearchLookUpEdit
            if (SalesOrderSearchLookupEdit != null)
            {
                // SalesOrder là TextEdit, chỉ lưu số đơn bán, không lưu ID
                // TODO: Nếu cần lưu SalesOrderId, cần thay đổi thành SearchLookUpEdit
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

            // Cập nhật từ Customer SearchLookUpEdit
            if (CustomerNameSearchLookupEdit != null)
            {
                if (CustomerNameSearchLookupEdit.EditValue is Guid customerId && customerId != Guid.Empty)
                {
                    _stockOutMaster.PartnerSiteId = customerId; // Dùng PartnerSiteId để lưu CustomerId
                }
                else
                {
                    _stockOutMaster.PartnerSiteId = null;
                }
            }

            // Cập nhật từ NotesTextEdit
            if (NotesTextEdit != null)
            {
                _stockOutMaster.Notes = NotesTextEdit.Text?.Trim();
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
            nameof(XuatHangThuongMaiMasterDto.StockOutNumber) => StockOutNumberTextEdit,
            nameof(XuatHangThuongMaiMasterDto.StockOutDate) => StockOutDateDateEdit,
            nameof(XuatHangThuongMaiMasterDto.WarehouseId) => WarehouseNameSearchLookupEdit,
            nameof(XuatHangThuongMaiMasterDto.WarehouseCode) => WarehouseNameSearchLookupEdit,
            nameof(XuatHangThuongMaiMasterDto.WarehouseName) => WarehouseNameSearchLookupEdit,
            nameof(XuatHangThuongMaiMasterDto.CustomerId) => CustomerNameSearchLookupEdit,
            nameof(XuatHangThuongMaiMasterDto.CustomerName) => CustomerNameSearchLookupEdit,
            nameof(XuatHangThuongMaiMasterDto.SalesOrderNumber) => SalesOrderSearchLookupEdit,
            nameof(XuatHangThuongMaiMasterDto.Notes) => NotesTextEdit,
            nameof(XuatHangThuongMaiMasterDto.NguoiNhanHang) => NguoiNhanHangTextEdit,
            nameof(XuatHangThuongMaiMasterDto.NguoiGiaoHang) => NguoiGiaoHangTextEdit,
            _ => null
        };
    }

    #endregion

    #region ========== PUBLIC METHODS ==========

    /// <summary>
    /// Lấy DTO từ Entity sau khi validate các trường bắt buộc
    /// </summary>
    /// <returns>XuatHangThuongMaiMasterDto nếu validation thành công, null nếu có lỗi</returns>
    public XuatHangThuongMaiMasterDto GetDto()
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
            _stockOutMaster.StockInOutType = (int)LoaiNhapXuatKhoEnum.XuatHangThuongMai;

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

            // Gán entity vào _stockOutMaster
            _stockOutMaster = masterEntity;

            // Set dữ liệu cho các control đơn giản (không cần datasource)
            StockOutDateDateEdit.EditValue = masterEntity.StockInOutDate;
            StockOutNumberTextEdit.EditValue = masterEntity.VocherNumber;
            SalesOrderSearchLookupEdit.EditValue = masterEntity.PurchaseOrderId?.ToString() ?? string.Empty; // TODO: Lấy SalesOrderNumber từ entity
            NotesTextEdit.EditValue = masterEntity.Notes;
            NguoiNhanHangTextEdit.EditValue = masterEntity.NguoiNhanHang;
            NguoiGiaoHangTextEdit.EditValue = masterEntity.NguoiGiaoHang;

            // Load datasource cho Warehouse trước khi set EditValue
            await LoadSingleWarehouseByIdAsync(masterEntity.WarehouseId);
            WarehouseNameSearchLookupEdit.EditValue = masterEntity.WarehouseId;

            // Load datasource cho Customer nếu có PartnerSiteId
            if (masterEntity.PartnerSiteId.HasValue)
            {
                await LoadSingleCustomerByIdAsync(masterEntity.PartnerSiteId.Value);
                CustomerNameSearchLookupEdit.EditValue = masterEntity.PartnerSiteId;
            }
            else
            {
                CustomerNameSearchLookupEdit.EditValue = null;
            }

            // Cập nhật tổng hợp
            UpdateTotals(
                masterEntity.TotalQuantity,
                masterEntity.TotalAmount,
                masterEntity.TotalVat,
                masterEntity.TotalAmountIncludedVat
            );
        }
        catch (Exception ex)
        {
            ShowError(ex, "Lỗi tải dữ liệu phiếu xuất kho");
            throw;
        }
    }

    /// <summary>
    /// Load single Warehouse record theo ID và set vào datasource
    /// </summary>
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

    /// <summary>
    /// Load single Customer record theo ID và set vào datasource
    /// </summary>
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
                var siteDtos = new List<Dal.DataContext.BusinessPartnerSite> { site }.ToSiteListDtos().ToList();
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

            if (SalesOrderSearchLookupEdit != null)
            {
                SalesOrderSearchLookupEdit.Text = string.Empty;
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

            // Clear errors
            dxErrorProvider1.ClearErrors();

            // Reset totals
            UpdateTotals(0, 0, 0, 0);
        }
        catch (Exception ex)
        {
            ShowError(ex, "Lỗi xóa dữ liệu");
        }
    }

    /// <summary>
    /// Cập nhật các giá trị tổng hợp từ detail
    /// </summary>
    public void UpdateTotals(decimal totalQuantity, decimal totalAmount, decimal totalVat,
        decimal totalAmountIncludedVat)
    {
        try
        {
            // Cập nhật trực tiếp vào Entity
            _stockOutMaster.TotalQuantity = totalQuantity;
            _stockOutMaster.TotalAmount = totalAmount;
            _stockOutMaster.TotalVat = totalVat;
            _stockOutMaster.TotalAmountIncludedVat = totalAmountIncludedVat;

            // Cập nhật trực tiếp vào các SimpleLabelItem để hiển thị
            if (TotalQuantitySimpleLabelItem != null)
            {
                TotalQuantitySimpleLabelItem.Text = totalQuantity.ToString("N2");
            }

            if (TotalAmountSimpleLabelItem != null)
            {
                TotalAmountSimpleLabelItem.Text = totalAmount.ToString("N0");
            }

            if (TotalVatSimpleLabelItem != null)
            {
                TotalVatSimpleLabelItem.Text = totalVat.ToString("N0");
            }

            if (TotalAmountIncludedVatSimpleLabelItem != null)
            {
                TotalAmountIncludedVatSimpleLabelItem.Text = totalAmountIncludedVat.ToString("N0");
            }
        }
        catch (Exception ex)
        {
            ShowError(ex, "Lỗi cập nhật tổng hợp");
        }
    }

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
            var voucherNumber = _stockInOutMasterBll.GenerateVoucherNumber(date, LoaiNhapXuatKhoEnum.XuatHangThuongMai);
            
            // Cập nhật vào Entity và control
            _stockOutMaster.VocherNumber = voucherNumber;
            if (StockOutNumberTextEdit != null)
            {
                StockOutNumberTextEdit.Text = voucherNumber;
            }
        }
        catch (Exception ex)
        {
            ShowError(ex, "Lỗi tạo số phiếu xuất kho");
        }
    }

    #endregion

    #region ========== SUPERTOOLTIP ==========

    /// <summary>
    /// Thiết lập SuperToolTip cho tất cả các controls trong UserControl
    /// </summary>
    private void SetupSuperToolTips()
    {
        // TODO: Implement SuperToolTips nếu cần
    }

    #endregion

    #region ========== HELPER METHODS ==========

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
