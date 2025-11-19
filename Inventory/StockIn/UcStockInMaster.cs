using Bll.MasterData.CompanyBll;
using Bll.MasterData.CustomerBll;
using Common;
using Common.Utils;
using DevExpress.XtraEditors.DXErrorProvider;
using DTO.Inventory.StockIn;
using DTO.MasterData.Company;
using DTO.MasterData.CustomerPartner;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventory.StockIn
{
    public partial class UcStockInMaster : DevExpress.XtraEditors.XtraUserControl
    {
        #region ========== KHAI BÁO BIẾN ==========

        /// <summary>
        /// DTO chứa dữ liệu phiếu nhập kho
        /// </summary>
        private StockInMasterDto _stockInMasterDto;

        /// <summary>
        /// Business Logic Layer cho CompanyBranch (dùng cho Warehouse lookup)
        /// </summary>
        private readonly CompanyBranchBll _companyBranchBll = new CompanyBranchBll();

        /// <summary>
        /// Business Logic Layer cho BusinessPartnerSite (dùng cho Supplier lookup)
        /// </summary>
        private readonly BusinessPartnerSiteBll _businessPartnerSiteBll = new BusinessPartnerSiteBll();

        #endregion

        #region ========== CONSTRUCTOR ==========

        public UcStockInMaster()
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
                SetupLookupEdits();

                // Đánh dấu các trường bắt buộc
                MarkRequiredFields();

                // Setup events
                SetupEvents();

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
            _stockInMasterDto = new StockInMasterDto
            {
                Id = Guid.Empty,
                StockInNumber = null,
                StockInDate = DateTime.Now,
                LoaiNhapKho = LoaiNhapKhoEnum.ThuongMai,
                TrangThai = TrangThaiPhieuNhapEnum.TaoMoi,
                WarehouseId = Guid.Empty,
                WarehouseCode = null,
                WarehouseName = null,
                PurchaseOrderId = null,
                PurchaseOrderNumber = null,
                SupplierId = Guid.Empty,
                SupplierName = null,
                Notes = null
            };

            // Khởi tạo các giá trị tổng hợp bằng method SetTotals() vì các property giờ là computed (read-only)
            _stockInMasterDto.SetTotals(0, 0, 0, 0);
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
                    typeof(StockInMasterDto),
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
                StockInDateDateEdit.EditValueChanged += StockInDateDateEdit_EditValueChanged;
                WarehouseNameSearchLookupEdit.EditValueChanged += WarehouseNameSearchLookupEdit_EditValueChanged;
                SupplierNameTextEdit.EditValueChanged += SupplierNameTextEdit_EditValueChanged;
                StockInNumberTextEdit.EditValueChanged += StockInNumberTextEdit_EditValueChanged;
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi thiết lập sự kiện");
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
                // Load danh sách CompanyBranchDto từ CompanyBranchBll (dùng làm Warehouse)
                var branches = await Task.Run(() => _companyBranchBll.GetAll());
                var warehouseDtos = branches
                    .Where(b => b.IsActive) // Chỉ lấy các chi nhánh đang hoạt động
                    .Select(b => b.ToDto())
                    .OrderBy(b => b.BranchName)
                    .ToList();

                companyBranchDtoBindingSource.DataSource = warehouseDtos;

                // Load danh sách BusinessPartnerSite từ BusinessPartnerSiteBll (dùng cho Supplier lookup)
                var sites = await Task.Run(() => _businessPartnerSiteBll.GetAll());
                var siteDtos = sites
                    .Where(s => s.IsActive) // Chỉ lấy các chi nhánh đang hoạt động
                    .ToSiteListDtos()
                    .OrderBy(s => s.SiteName)
                    .ToList();

                businessPartnerSiteListDtoBindingSource.DataSource = siteDtos;
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi tải dữ liệu lookup");
            }
        }

        /// <summary>
        /// Load dữ liệu từ DTO vào controls
        /// </summary>
        private void LoadData(StockInMasterDto dto)
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
                PurchaseOrderSearchLookupEdit,
                NotesTextEdit
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
                    var selectedWarehouse = companyBranchDtoBindingSource.Cast<CompanyBranchDto>()
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

        private void SupplierNameTextEdit_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (SupplierNameTextEdit.EditValue is Guid supplierId && supplierId != Guid.Empty)
                {
                    _stockInMasterDto.SupplierId = supplierId;
                    
                    // Lấy thông tin chi nhánh đối tác từ binding source
                    var selectedSite = businessPartnerSiteListDtoBindingSource.Cast<BusinessPartnerSiteListDto>()
                        .FirstOrDefault(s => s.Id == supplierId);
                    
                    if (selectedSite != null)
                    {
                        _stockInMasterDto.SupplierName = selectedSite.SiteName;
                    }
                    
                    // Xóa lỗi validation nếu có
                    dxErrorProvider1.SetError(SupplierNameTextEdit, string.Empty);
                }
                else
                {
                    _stockInMasterDto.SupplierId = Guid.Empty;
                    _stockInMasterDto.SupplierName = null;
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi xử lý thay đổi nhà cung cấp");
            }
        }

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
                        var selectedWarehouse = companyBranchDtoBindingSource.Cast<CompanyBranchDto>()
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

                // Cập nhật từ Supplier SearchLookUpEdit
                if (SupplierNameTextEdit != null)
                {
                    if (SupplierNameTextEdit.EditValue is Guid supplierId && supplierId != Guid.Empty)
                    {
                        _stockInMasterDto.SupplierId = supplierId;
                        
                        // Lấy thông tin chi nhánh đối tác từ binding source
                        var selectedSite = businessPartnerSiteListDtoBindingSource.Cast<BusinessPartnerSiteListDto>()
                            .FirstOrDefault(s => s.Id == supplierId);
                        
                        if (selectedSite != null)
                        {
                            _stockInMasterDto.SupplierName = selectedSite.SiteName;
                        }
                    }
                    else
                    {
                        _stockInMasterDto.SupplierId = Guid.Empty;
                        _stockInMasterDto.SupplierName = null;
                    }
                }

                // Cập nhật các trường khác nếu có
                // TODO: Thêm các control khác nếu cần
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi cập nhật dữ liệu từ controls");
            }
        }

        /// <summary>
        /// Validate dữ liệu input và hiển thị lỗi bằng dxErrorProvider
        /// </summary>
        public bool ValidateInput()
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
                nameof(StockInMasterDto.StockInNumber) => StockInNumberTextEdit,
                nameof(StockInMasterDto.StockInDate) => StockInDateDateEdit,
                nameof(StockInMasterDto.WarehouseId) => WarehouseNameSearchLookupEdit,
                nameof(StockInMasterDto.WarehouseCode) => WarehouseNameSearchLookupEdit,
                nameof(StockInMasterDto.WarehouseName) => WarehouseNameSearchLookupEdit,
                nameof(StockInMasterDto.SupplierId) => SupplierNameTextEdit,
                nameof(StockInMasterDto.SupplierName) => SupplierNameTextEdit,
                nameof(StockInMasterDto.PurchaseOrderNumber) => PurchaseOrderSearchLookupEdit,
                nameof(StockInMasterDto.Notes) => NotesTextEdit,
                _ => null
            };
        }

        #endregion

        #region ========== PUBLIC METHODS ==========

        /// <summary>
        /// Lấy DTO từ controls sau khi validate các trường bắt buộc
        /// </summary>
        /// <returns>StockInMasterDto nếu validation thành công, null nếu có lỗi</returns>
        public StockInMasterDto GetDto()
        {
            try
            {
                // Cập nhật DTO từ controls trước khi validate
                UpdateDtoFromControls();

                // Validate các trường bắt buộc
                if (!ValidateInput())
                {
                    return null; // Validation thất bại
                }

                return _stockInMasterDto;
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi lấy dữ liệu");
                return null;
            }
        }

        /// <summary>
        /// Set DTO và load vào controls
        /// </summary>
        public void SetDto(StockInMasterDto dto)
        {
            LoadData(dto);
        }

        /// <summary>
        /// Clear dữ liệu và reset về trạng thái ban đầu
        /// </summary>
        public void ClearData()
        {
            InitializeDto();
            dxErrorProvider1.ClearErrors();
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
                _stockInMasterDto.SetTotals(totalQuantity, totalAmount, totalVat, totalAmountIncludedVat);

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

        #endregion

        #region ========== HELPER METHODS ==========

        /// <summary>
        /// Tạo số phiếu nhập kho theo format: PNK-MMYY-NNXXX
        /// PNK: Phiếu nhập kho
        /// MM: Tháng (2 ký tự)
        /// YY: Năm (2 ký tự)
        /// NN: Index của LoaiNhapKhoEnum (2 ký tự)
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
                var loaiNhapKhoIndex = ((int)_stockInMasterDto.LoaiNhapKho).ToString("D2"); // NN (2 ký tự)

                // Lấy số thứ tự tiếp theo
                var nextSequence = GetNextSequenceNumber(stockInDate, _stockInMasterDto.LoaiNhapKho);

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
        /// <param name="loaiNhapKho">Loại nhập kho</param>
        /// <returns>Số thứ tự tiếp theo (1-999)</returns>
        private int GetNextSequenceNumber(DateTime stockInDate, LoaiNhapKhoEnum loaiNhapKho)
        {
            try
            {
                // TODO: Query database để lấy số thứ tự tiếp theo
                // Tạm thời trả về 1, sau này sẽ implement query database
                // Format cần tìm: PNK-MMYY-NNXXX
                // Trong đó MM, YY, NN đã biết, cần tìm XXX lớn nhất + 1

                // Ví dụ query:
                // SELECT MAX(CAST(SUBSTRING(StockInNumber, LEN(StockInNumber) - 2, 3) AS INT))
                // FROM StockInMaster
                // WHERE StockInNumber LIKE 'PNK-MMYY-NN%'
                //   AND YEAR(StockInDate) = YYYY
                //   AND MONTH(StockInDate) = MM
                //   AND LoaiNhapKho = loaiNhapKho

                // Tạm thời return 1
                return 1;
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi lấy số thứ tự phiếu nhập");
                return 1; // Fallback
            }
        }

        /// <summary>
        /// Hiển thị lỗi
        /// </summary>
        private void ShowError(Exception ex, string message)
        {
            DevExpress.XtraEditors.XtraMessageBox.Show(
                $"{message}: {ex.Message}",
                "Lỗi",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }

        /// <summary>
        /// Hiển thị lỗi
        /// </summary>
        private void ShowError(string message)
        {
            DevExpress.XtraEditors.XtraMessageBox.Show(
                message,
                "Lỗi",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }

        #endregion
    }
}
