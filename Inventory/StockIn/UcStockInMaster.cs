using Bll.MasterData.Company;
using Bll.MasterData.Customer;
using Bll.Utils;
using DevExpress.XtraEditors.DXErrorProvider;
using DTO.Inventory.StockIn;
using DTO.MasterData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraGrid.Columns;
using DTO.MasterData.Company;

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

                // Khởi tạo binding sources
                InitializeBindingSources();

                // Thiết lập data binding
                SetupDataBinding();

                // Setup SearchLookUpEdit cho Warehouse
                SetupLookupEdits();

                // Đánh dấu các trường bắt buộc
                MarkRequiredFields();

                // Setup events
                SetupEvents();

                // Load dữ liệu lookup
                LoadLookupDataAsync();
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
                StockInDate = DateTime.Now,
                TrangThai = TrangThaiPhieuNhapEnum.TaoMoi,
                LoaiNhapKho = LoaiNhapKhoEnum.ThuongMai,
                PhuongThucNhapKho = PhuongThucNhapKhoEnum.NhapTrucTiep
            };
        }

        /// <summary>
        /// Khởi tạo binding sources
        /// </summary>
        private void InitializeBindingSources()
        {
            
        }

        /// <summary>
        /// Thiết lập data binding
        /// </summary>
        private void SetupDataBinding()
        {
            try
            {
                //// Bind các control với DTO
                //StockInNumberTextEdit.DataBindings.Add("Text", _stockInMasterDto, nameof(StockInMasterDto.StockInNumber), false, DataSourceUpdateMode.OnPropertyChanged);
                //StockInDateDateEdit.DataBindings.Add("EditValue", _stockInMasterDto, nameof(StockInMasterDto.StockInDate), false, DataSourceUpdateMode.OnPropertyChanged);
                //PurchaseOrderSearchLookupEdit.DataBindings.Add("Text", _stockInMasterDto, nameof(StockInMasterDto.PurchaseOrderNumber), false, DataSourceUpdateMode.OnPropertyChanged);
                //NotesTextEdit.DataBindings.Add("Text", _stockInMasterDto, nameof(StockInMasterDto.Notes), false, DataSourceUpdateMode.OnPropertyChanged);
                //TotalQuantityTextEdit.DataBindings.Add("EditValue", _stockInMasterDto, nameof(StockInMasterDto.TotalQuantity), false, DataSourceUpdateMode.OnPropertyChanged);
                //ThanTienChuaVatTextEdit.DataBindings.Add("EditValue", _stockInMasterDto, nameof(StockInMasterDto.TotalAmount), false, DataSourceUpdateMode.OnPropertyChanged);
                //VatTextEdit.DataBindings.Add("EditValue", _stockInMasterDto, nameof(StockInMasterDto.TotalVat), false, DataSourceUpdateMode.OnPropertyChanged);
                //TongTienBaoGomVatTextEdit.DataBindings.Add("EditValue", _stockInMasterDto, nameof(StockInMasterDto.TotalAmountIncludedVat), false, DataSourceUpdateMode.OnPropertyChanged);

                //// Bind Warehouse SearchLookUpEdit
                //WarehouseNameTextEdit.DataBindings.Add("EditValue", _stockInMasterDto, nameof(StockInMasterDto.WarehouseId), false, DataSourceUpdateMode.OnPropertyChanged);

                //// Bind Supplier SearchLookUpEdit
                //SupplierNameTextEdit.DataBindings.Add("EditValue", _stockInMasterDto, nameof(StockInMasterDto.SupplierId), false, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi thiết lập data binding");
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
                WarehouseNameSearchLookupEdit.Properties.PopupView = searchLookUpEdit1View;
                
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
        /// </summary>
        private async void LoadLookupDataAsync()
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

                // Load danh sách Supplier từ BusinessPartner (lọc theo PartnerType = Supplier)
                //var partners = await Task.Run(() => _businessPartnerBll.GetAll());
                //var suppliers = partners
                //    .ToBusinessPartnerListDtos()
                //    .Where(p => p.PartnerType == 2 || p.PartnerType == 3) // Supplier hoặc Both
                //    .OrderBy(p => p.PartnerName)
                //    .Select(p => new
                //    {
                //        Id = p.Id,
                //        SupplierCode = p.PartnerCode,
                //        SupplierName = p.PartnerName
                //    })
                //    .ToList();

                //_supplierBindingSource.DataSource = suppliers;
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
                NotesTextEdit,
                TotalQuantityTextEdit,
                ThanTienChuaVatTextEdit,
                VatTextEdit,
                TongTienBaoGomVatTextEdit
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
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi tạo số phiếu nhập");
            }
        }

        private void WarehouseNameTextEdit_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                //if (WarehouseNameSearchLookupEdit.EditValue is Guid warehouseId && warehouseId != Guid.Empty)
                //{
                //    var selectedWarehouse = _warehouseBindingSource.Cast<CompanyBranchDto>()
                //        .FirstOrDefault(w => w.Id == warehouseId);
                    
                //    if (selectedWarehouse != null)
                //    {
                //        _stockInMasterDto.WarehouseId = warehouseId;
                //        _stockInMasterDto.WarehouseCode = selectedWarehouse.BranchCode;
                //        _stockInMasterDto.WarehouseName = selectedWarehouse.BranchName;
                //    }
                //}
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi xử lý thay đổi kho");
            }
        }

        private void SupplierNameTextEdit_EditValueChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    if (SupplierNameTextEdit.EditValue is Guid supplierId && supplierId != Guid.Empty)
            //    {
            //        var selectedSupplier = _supplierBindingSource.Cast<dynamic>()
            //            .FirstOrDefault(s => s.Id == supplierId);
                    
            //        if (selectedSupplier != null)
            //        {
            //            _stockInMasterDto.SupplierId = supplierId;
            //            _stockInMasterDto.SupplierCode = selectedSupplier.SupplierCode;
            //            _stockInMasterDto.SupplierName = selectedSupplier.SupplierName;
            //        }
            //    }
            //    else
            //    {
            //        _stockInMasterDto.SupplierId = null;
            //        _stockInMasterDto.SupplierCode = null;
            //        _stockInMasterDto.SupplierName = null;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ShowError(ex, "Lỗi xử lý thay đổi nhà cung cấp");
            //}
        }

        #endregion

        #region ========== VALIDATION ==========

        /// <summary>
        /// Validate dữ liệu input
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
                    //// Hiển thị lỗi cho từng field
                    //foreach (var result in results)
                    //{
                    //    foreach (var memberName in result.MemberNames)
                    //    {
                    //        var control = FindControlByPropertyName(memberName);
                    //        if (control != null)
                    //        {
                    //            dxErrorProvider1.SetError(control, result.ErrorMessage, ErrorType.Critical);
                    //        }
                    //    }
                    //}

                    //// Focus vào control đầu tiên có lỗi
                    //var firstErrorControl = results
                    //    .SelectMany(r => r.MemberNames)
                    //    .Select(FindControlByPropertyName)
                    //    .FirstOrDefault(c => c != null);
                    
                    //if (firstErrorControl != null)
                    //{
                    //    firstErrorControl.Focus();
                    //}

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

        ///// <summary>
        ///// Tìm control theo tên property
        ///// </summary>
        //private Control FindControlByPropertyName(string propertyName)
        //{
        //    return propertyName switch
        //    {
        //        nameof(StockInMasterDto.StockInNumber) => StockInNumberTextEdit,
        //        nameof(StockInMasterDto.StockInDate) => StockInDateDateEdit,
        //        nameof(StockInMasterDto.WarehouseId) => WarehouseNameSearchLookupEdit,
        //        nameof(StockInMasterDto.PurchaseOrderNumber) => PurchaseOrderSearchLookupEdit,
        //        nameof(StockInMasterDto.SupplierId) => SupplierNameTextEdit,
        //        nameof(StockInMasterDto.Notes) => NotesTextEdit,
        //        _ => null
        //    };
        //}

        #endregion

        #region ========== PUBLIC METHODS ==========

        /// <summary>
        /// Lấy DTO từ controls
        /// </summary>
        public StockInMasterDto GetDto()
        {
            return _stockInMasterDto;
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
