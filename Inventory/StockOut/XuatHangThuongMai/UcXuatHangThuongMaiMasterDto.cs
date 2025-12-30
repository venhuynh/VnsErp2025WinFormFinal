using Bll.Inventory.InventoryManagement;
using Bll.Inventory.StockInOut;
using Bll.MasterData.CompanyBll;
using Bll.MasterData.CustomerBll;
using Common;
using Common.Utils;
using DevExpress.XtraEditors;
using DTO.Inventory;
using System;
using System.Linq;
using System.Threading.Tasks;
using DTO.Inventory.InventoryManagement;

namespace Inventory.StockOut.XuatHangThuongMai;

public partial class UcXuatHangThuongMaiMasterDto : XtraUserControl
{
    #region ========== KHAI BÁO BIẾN ==========

    /// <summary>
    /// Business Logic Layer cho CompanyBranch (dùng cho Warehouse lookup)
    /// </summary>
    private readonly CompanyBranchBll _companyBranchBll = new();

    /// <summary>
    /// Business Logic Layer cho BusinessPartnerSite (dùng cho Customer lookup)
    /// </summary>
    private readonly BusinessPartnerSiteBll _businessPartnerSiteBll = new();

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

    private Guid _selectedPartnerSiteId = Guid.Empty;

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
            Load += UcXuatHangThuongMaiMasterDto_Load;

            //Sự kiện của WarehouseNameSearchLookupEdit
            WarehouseNameSearchLookupEdit.Popup += WarehouseNameSearchLookupEdit_Popup;
            WarehouseNameSearchLookupEdit.EditValueChanged += WarehouseNameSearchLookupEdit_EditValueChanged;

            //Sự kiện của CustomerNameSearchLookupEdit
            CustomerNameSearchLookupEdit.Popup += CustomerNameSearchLookupEdit_Popup;
            CustomerNameSearchLookupEdit.EditValueChanged += CustomerNameSearchLookupEdit_EditValueChanged;

            StockOutDateDateEdit.EditValueChanged += StockOutDateDateEdit_EditValueChanged;
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
            await LoadCustomerDataSourceAsync();
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
            await LoadWarehouseDataSourceAsync();
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
            // Load cả 2 datasource song song để tối ưu performance
            await Task.WhenAll(
                LoadWarehouseDataSourceAsync(),
                LoadCustomerDataSourceAsync()
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

    /// <summary>
    /// Load datasource cho Customer (BusinessPartnerSite) - Load toàn bộ danh sách
    /// </summary>
    private async Task LoadCustomerDataSourceAsync()
    {
        try
        {
            businessPartnerSiteListDtoBindingSource.DataSource = await Task.Run(() => _businessPartnerSiteBll.GetAll());
        }
        catch (Exception ex)
        {
            ShowError(ex, "Lỗi tải dữ liệu khách hàng");
            throw;
        }
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

    private void CustomerNameSearchLookupEdit_EditValueChanged(object sender, EventArgs e)
    {
        try
        {
            if (CustomerNameSearchLookupEdit.EditValue is Guid customerId && customerId != Guid.Empty)
            {
                // Cập nhật PartnerSiteId vào _selectedPartnerSiteId
                _selectedPartnerSiteId = customerId;

                // Xóa lỗi validation nếu có
                dxErrorProvider1.SetError(CustomerNameSearchLookupEdit, string.Empty);
            }
            else
            {
                _selectedPartnerSiteId = Guid.Empty;
            }
        }
        catch (Exception ex)
        {
            ShowError(ex, "Lỗi xử lý thay đổi khách hàng");
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

            // Khách hàng không được để trống
            var customerId = _selectedPartnerSiteId != Guid.Empty
                ? _selectedPartnerSiteId
                : (CustomerNameSearchLookupEdit.EditValue is Guid cId ? cId : Guid.Empty);

            if (customerId == Guid.Empty)
            {
                dxErrorProvider1.SetError(CustomerNameSearchLookupEdit, "Khách hàng không được để trống");
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
                LoaiNhapXuatKho = LoaiNhapXuatKhoEnum.XuatHangThuongMai,
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

            // Lấy thông tin Customer từ selected item trong SearchLookUpEdit
            var customerId = _selectedPartnerSiteId != Guid.Empty
                ? _selectedPartnerSiteId
                : (CustomerNameSearchLookupEdit.EditValue is Guid cId ? cId : Guid.Empty);

            if (customerId != Guid.Empty)
            {
                dto.CustomerId = customerId;

                // Lấy thông tin chi tiết từ selected row hoặc binding source
                var customer =
                    CustomerNameSearchLookupEdit.GetSelectedDataRow() as
                        DTO.MasterData.CustomerPartner.BusinessPartnerSiteListDto;
                if (customer == null &&
                    businessPartnerSiteListDtoBindingSource.DataSource is System.Collections.IList customerList)
                {
                    customer = customerList.Cast<DTO.MasterData.CustomerPartner.BusinessPartnerSiteListDto>()
                        .FirstOrDefault(c => c.Id == customerId);
                }

                if (customer != null)
                {
                    dto.CustomerName = customer.SiteName ?? string.Empty;
                }
            }

            // SalesOrderId nếu có (SalesOrderSearchLookupEdit là TextEdit, không lưu ID)
            // TODO: Nếu cần lưu SalesOrderId, cần thay đổi thành SearchLookUpEdit

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

            // Load datasource cho Customer nếu có CustomerId
            if (masterDto.CustomerId.HasValue)
            {
                await LoadCustomerDataSourceAsync();
                CustomerNameSearchLookupEdit.EditValue = masterDto.CustomerId;
            }
            else
            {
                CustomerNameSearchLookupEdit.EditValue = null;
            }

            // SalesOrderId nếu có (lưu vào SalesOrderSearchLookupEdit dạng text)
            if (masterDto.SalesOrderId.HasValue)
            {
                // TODO: Nếu cần hiển thị SalesOrderNumber, cần load từ SalesOrder entity
                SalesOrderSearchLookupEdit.Text = masterDto.SalesOrderId.ToString();
            }
            else
            {
                SalesOrderSearchLookupEdit.Text = string.Empty;
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
            if (WarehouseNameSearchLookupEdit != null)
            {
                WarehouseNameSearchLookupEdit.EditValue = null;
            }

            if (CustomerNameSearchLookupEdit != null)
            {
                CustomerNameSearchLookupEdit.EditValue = null;
            }

            // Reset TextEdit
            if (StockOutNumberTextEdit != null)
            {
                StockOutNumberTextEdit.Text = string.Empty;
            }

            if (SalesOrderSearchLookupEdit != null)
            {
                SalesOrderSearchLookupEdit.Text = string.Empty;
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
    /// <param name="totalAmount">Tổng tiền chưa VAT</param>
    /// <param name="totalVat">Tổng VAT</param>
    /// <param name="totalAmountIncludedVat">Tổng tiền bao gồm VAT</param>
    public void UpdateTotals(decimal totalQuantity, decimal totalAmount, decimal totalVat,
        decimal totalAmountIncludedVat)
    {
        try
        {
            // Cập nhật trực tiếp vào các SimpleLabelItem để hiển thị
            UpdateTotalQuantityLabel(totalQuantity);
            UpdateTotalAmountLabel(totalAmount);
            UpdateTotalVatLabel(totalVat);
            UpdateTotalAmountIncludedVatLabel(totalAmountIncludedVat);
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
    /// Cập nhật label tổng tiền chưa VAT
    /// </summary>
    private void UpdateTotalAmountLabel(decimal value)
    {
        if (TotalAmountSimpleLabelItem != null)
        {
            TotalAmountSimpleLabelItem.Text = FormatCurrency(value);
        }
    }

    /// <summary>
    /// Cập nhật label tổng VAT
    /// </summary>
    private void UpdateTotalVatLabel(decimal value)
    {
        if (TotalVatSimpleLabelItem != null)
        {
            TotalVatSimpleLabelItem.Text = FormatCurrency(value);
        }
    }

    /// <summary>
    /// Cập nhật label tổng tiền bao gồm VAT
    /// </summary>
    private void UpdateTotalAmountIncludedVatLabel(decimal value)
    {
        if (TotalAmountIncludedVatSimpleLabelItem != null)
        {
            TotalAmountIncludedVatSimpleLabelItem.Text = FormatCurrency(value);
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
            var loaiNhapXuatKho = LoaiNhapXuatKhoEnum.XuatHangThuongMai;

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