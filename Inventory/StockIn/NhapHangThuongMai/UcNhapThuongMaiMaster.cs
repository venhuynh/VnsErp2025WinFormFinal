using Bll.Inventory.InventoryManagement;
using Bll.Inventory.StockInOut;
using Bll.MasterData.CompanyBll;
using Bll.MasterData.CustomerBll;
using Common;
using Common.Utils;
using DevExpress.XtraEditors;
using DTO.Inventory;
using DTO.Inventory.InventoryManagement;
using System;
using System.Threading.Tasks;

namespace Inventory.StockIn.NhapHangThuongMai
{
    public partial class UcNhapThuongMaiMaster : XtraUserControl
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

        public UcNhapThuongMaiMaster()
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

                //Sự kiện của SupplierNameSearchLookupEdit
                SupplierNameSearchLookupEdit.Popup += SupplierNameSearchLookupEdit_Popup;
                SupplierNameSearchLookupEdit.EditValueChanged += SupplierNameTextEdit_EditValueChanged;

                StockInDateDateEdit.EditValueChanged += StockInDateDateEdit_EditValueChanged;
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi thiết lập sự kiện");
            }
        }

        private async void SupplierNameSearchLookupEdit_Popup(object sender, EventArgs e)
        {
            try
            {
                await LoadSupplierDataSourceAsync();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi tải dữ liệu nhà cung cấp");
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
                    content:
                    @"Số phiếu nhập kho được tạo tự động theo format: <b>PNK-MMYY-NNXXX</b><br/><br/><b>Format:</b><br/>• PNK: Phiếu nhập kho<br/>• MM: Tháng (2 ký tự)<br/>• YY: Năm (2 ký tự cuối)<br/>• NN: Index của Loại nhập kho (2 ký tự)<br/>• XXX: Số thứ tự phiếu (3 ký tự từ 001 đến 999)<br/><br/><b>Chức năng:</b><br/>• Tự động tạo khi thay đổi ngày nhập kho<br/>• Tự động tạo khi thay đổi loại nhập kho<br/>• Query database để lấy số thứ tự tiếp theo<br/>• Đảm bảo số phiếu duy nhất trong cùng tháng/năm/loại<br/><br/><b>Ràng buộc:</b><br/>• <b>Bắt buộc nhập</b> (có dấu * đỏ)<br/>• Không được để trống<br/>• Tối đa 50 ký tự<br/><br/><color=Gray>Lưu ý:</color> Số phiếu nhập kho sẽ được lưu vào database khi lưu phiếu nhập."
                );
            }

            // SuperTip cho Mã đơn hàng mua
            if (PurchaseOrderSearchLookupEdit != null)
            {
                SuperToolTipHelper.SetTextEditSuperTip(
                    PurchaseOrderSearchLookupEdit,
                    title: @"<b><color=DarkBlue>🛒 Mã đơn hàng mua</color></b>",
                    content:
                    @"Nhập hoặc chọn mã đơn hàng mua (Purchase Order) liên quan đến phiếu nhập kho này.<br/><br/><b>Chức năng:</b><br/>• Liên kết phiếu nhập kho với đơn hàng mua<br/>• Tra cứu thông tin đơn hàng mua<br/>• Theo dõi quá trình nhập hàng theo đơn hàng<br/><br/><b>Ràng buộc:</b><br/>• Không bắt buộc (có thể để trống)<br/>• Tối đa 50 ký tự<br/><br/><color=Gray>Lưu ý:</color> Trường này là tùy chọn, chỉ điền khi phiếu nhập kho liên quan đến một đơn hàng mua cụ thể."
                );
            }

            // SuperTip cho Người nhận hàng
            if (NguoiNhanHangTextEdit != null)
            {
                SuperToolTipHelper.SetTextEditSuperTip(
                    NguoiNhanHangTextEdit,
                    title: @"<b><color=DarkBlue>👤 Người nhận hàng</color></b>",
                    content:
                    @"Nhập tên người nhận hàng tại kho.<br/><br/><b>Chức năng:</b><br/>• Ghi nhận thông tin người nhận hàng<br/>• Hỗ trợ tra cứu và theo dõi<br/><br/><b>Ràng buộc:</b><br/>• Không bắt buộc (có thể để trống)<br/>• Tối đa 500 ký tự<br/><br/><color=Gray>Lưu ý:</color> Thông tin này sẽ được lưu vào database khi lưu phiếu nhập kho."
                );
            }

            // SuperTip cho Người giao hàng
            if (NguoiGiaoHangTextEdit != null)
            {
                SuperToolTipHelper.SetTextEditSuperTip(
                    NguoiGiaoHangTextEdit,
                    title: @"<b><color=DarkBlue>🚚 Người giao hàng</color></b>",
                    content:
                    @"Nhập tên người giao hàng từ nhà cung cấp.<br/><br/><b>Chức năng:</b><br/>• Ghi nhận thông tin người giao hàng<br/>• Hỗ trợ tra cứu và theo dõi<br/><br/><b>Ràng buộc:</b><br/>• Không bắt buộc (có thể để trống)<br/>• Tối đa 500 ký tự<br/><br/><color=Gray>Lưu ý:</color> Thông tin này sẽ được lưu vào database khi lưu phiếu nhập kho."
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
                    content:
                    @"Chọn ngày nhập kho cho phiếu nhập.<br/><br/><b>Chức năng:</b><br/>• Xác định thời điểm nhập kho<br/>• Tự động tạo số phiếu nhập kho dựa trên ngày<br/>• Format số phiếu: PNK-MMYY-NNXXX (MM, YY từ ngày này)<br/>• Query database để lấy số thứ tự tiếp theo trong tháng/năm<br/><br/><b>Ràng buộc:</b><br/>• <b>Bắt buộc nhập</b> (có dấu * đỏ)<br/>• Không được để trống<br/>• Mặc định: Ngày hiện tại<br/><br/><b>Validation:</b><br/>• Kiểm tra rỗng khi validating<br/>• Hiển thị lỗi qua ErrorProvider nếu không hợp lệ<br/><br/><color=Gray>Lưu ý:</color> Khi thay đổi ngày nhập kho, hệ thống sẽ tự động tạo lại số phiếu nhập kho theo format mới."
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
                    @"Chọn kho nhập hàng từ danh sách chi nhánh (Company Branch) đang hoạt động.<br/><br/><b>Chức năng:</b><br/>• Chọn kho nhập hàng<br/>• Hiển thị thông tin kho dạng HTML (mã, tên)<br/>• Tự động cập nhật WarehouseId, WarehouseCode, WarehouseName vào DTO<br/><br/><b>Ràng buộc:</b><br/>• <b>Bắt buộc chọn</b> (có dấu * đỏ)<br/>• Không được để trống<br/>• Chỉ hiển thị các chi nhánh đang hoạt động (IsActive = true)<br/><br/><b>Data Source:</b><br/>• Load từ CompanyBranchBll.GetAll()<br/>• Filter chỉ lấy các chi nhánh đang hoạt động<br/>• Sắp xếp theo tên chi nhánh<br/><br/><b>Validation:</b><br/>• Kiểm tra rỗng khi validating<br/>• Hiển thị lỗi qua ErrorProvider nếu không hợp lệ<br/><br/><color=Gray>Lưu ý:</color> Kho nhập sẽ được lưu vào database khi lưu phiếu nhập kho."
                );
            }

            // SuperTip cho Nhà cung cấp
            if (SupplierNameSearchLookupEdit != null)
            {
                SuperToolTipHelper.SetBaseEditSuperTip(
                    SupplierNameSearchLookupEdit,
                    title: @"<b><color=DarkBlue>🏭 Nhà cung cấp</color></b>",
                    content:
                    @"Chọn nhà cung cấp từ danh sách chi nhánh đối tác (Business Partner Site) đang hoạt động.<br/><br/><b>Chức năng:</b><br/>• Chọn nhà cung cấp<br/>• Hiển thị thông tin nhà cung cấp dạng HTML (mã, tên)<br/>• Tự động cập nhật SupplierId, SupplierName vào DTO<br/><br/><b>Ràng buộc:</b><br/>• Không bắt buộc (có thể để trống)<br/>• Chỉ hiển thị các chi nhánh đối tác đang hoạt động (IsActive = true)<br/><br/><b>Data Source:</b><br/>• Load từ BusinessPartnerSiteBll.GetAll()<br/>• Filter chỉ lấy các chi nhánh đối tác đang hoạt động<br/>• Sắp xếp theo tên chi nhánh<br/><br/><color=Gray>Lưu ý:</color> Trường này là tùy chọn, chỉ điền khi phiếu nhập kho có nhà cung cấp cụ thể."
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
                    @"Nhập ghi chú hoặc mô tả bổ sung cho phiếu nhập kho.<br/><br/><b>Chức năng:</b><br/>• Lưu thông tin bổ sung về phiếu nhập kho<br/>• Ghi chú về lý do nhập kho, điều kiện nhập hàng, v.v.<br/>• Hỗ trợ nhiều dòng văn bản<br/><br/><b>Ràng buộc:</b><br/>• Không bắt buộc (có thể để trống)<br/>• Không giới hạn độ dài<br/><br/><color=Gray>Lưu ý:</color> Ghi chú sẽ được lưu vào database khi lưu phiếu nhập kho."
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
                // Load cả 2 datasource song song để tối ưu performance
                await Task.WhenAll(
                    LoadWarehouseDataSourceAsync(forceRefresh: true),
                    LoadSupplierDataSourceAsync(forceRefresh: true)
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
                // Load danh sách CompanyBranchDto từ CompanyBranchBll (dùng làm Warehouse)
                companyBranchLookupDtoBindingSource.DataSource = await Task.Run(() => _companyBranchBll.GetAll());
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi tải dữ liệu kho");
                throw;
            }
        }

        /// <summary>
        /// Load datasource cho Supplier (BusinessPartnerSite) - Load toàn bộ danh sách
        /// </summary>
        /// <param name="forceRefresh">Nếu true, sẽ load lại từ database ngay cả khi đã load trước đó</param>
        private async Task LoadSupplierDataSourceAsync(bool forceRefresh = false)
        {
            try
            {
                businessPartnerSiteListDtoBindingSource.DataSource = await Task.Run(() => _businessPartnerSiteBll.GetAll());
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi tải dữ liệu nhà cung cấp");
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

        private void StockInDateDateEdit_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (StockInDateDateEdit.EditValue is not DateTime selectedDate) return;
                
                // Tạo số phiếu nhập tự động
                GenerateStockInNumber(selectedDate);
                        
                // Xóa lỗi validation nếu có
                dxErrorProvider1.SetError(StockInDateDateEdit, string.Empty);
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

        private void SupplierNameTextEdit_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (SupplierNameSearchLookupEdit.EditValue is Guid supplierId && supplierId != Guid.Empty)
                {
                    // Cập nhật PartnerSiteId vào _selectedPartnerSiteId
                    _selectedPartnerSiteId = supplierId;
                        
                    // Xóa lỗi validation nếu có
                    dxErrorProvider1.SetError(SupplierNameSearchLookupEdit, string.Empty);
                }
                else
                {
                    _selectedPartnerSiteId = Guid.Empty;
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi xử lý thay đổi nhà cung cấp");
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
                    dxErrorProvider1.SetError(StockInDateDateEdit, "Ngày nhập không được để trống");
                    return false;
                }

                // Số phiếu nhập kho không được để trống
                if (string.IsNullOrWhiteSpace(StockInNumberTextEdit.Text))
                {
                    dxErrorProvider1.SetError(StockInNumberTextEdit, "Số phiếu nhập không được để trống");
                    return false;
                }

                // Kiểm tra độ dài số phiếu nhập kho (tối đa 50 ký tự)
                if (StockInNumberTextEdit.Text.Length > 50)
                {
                    dxErrorProvider1.SetError(StockInNumberTextEdit, "Số phiếu nhập không được vượt quá 50 ký tự");
                    return false;
                }

                // Kho nhập không được để trống
                if (WarehouseNameSearchLookupEdit.EditValue is not Guid warehouseId || warehouseId == Guid.Empty)
                {
                    dxErrorProvider1.SetError(WarehouseNameSearchLookupEdit, "Kho nhập không được để trống");
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
                    LoaiNhapXuatKho = LoaiNhapXuatKhoEnum.NhapHangThuongMai,
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

                }

                // Lấy thông tin Customer từ selected item trong SearchLookUpEdit
                var customerId = _selectedPartnerSiteId != Guid.Empty
                    ? _selectedPartnerSiteId
                    : (SupplierNameSearchLookupEdit.EditValue is Guid cId ? cId : Guid.Empty);

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
                    throw new InvalidOperationException($"Không tìm thấy phiếu nhập kho với ID: {stockInOutMasterId}");
                }

                // Set dữ liệu cho các control đơn giản (không cần datasource)
                StockInDateDateEdit.EditValue = masterDto.StockOutDate;
                StockInNumberTextEdit.EditValue = masterDto.VoucherNumber;
                
                NotesTextEdit.EditValue = masterDto.Notes;
                NguoiNhanHangTextEdit.EditValue = masterDto.NguoiNhanHang;
                NguoiGiaoHangTextEdit.EditValue = masterDto.NguoiGiaoHang;

                // Load datasource cho Warehouse trước khi set EditValue
                await LoadWarehouseDataSourceAsync();
                WarehouseNameSearchLookupEdit.EditValue = masterDto.WarehouseId;

                // Load datasource cho Supplier nếu có CustomerId
                if (masterDto.CustomerId.HasValue)
                {
                    await LoadSupplierDataSourceAsync();
                    SupplierNameSearchLookupEdit.EditValue = masterDto.CustomerId;
                }
                else
                {
                    SupplierNameSearchLookupEdit.EditValue = null;
                }

                // Thông tin liên kết - SalesOrder (PurchaseOrder trong DB)
                if (masterDto.SalesOrderId.HasValue)
                {
                    PurchaseOrderSearchLookupEdit.EditValue = masterDto.SalesOrderId;
                }
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
                // Reset tất cả các controls về giá trị mặc định
                // Reset SearchLookUpEdit - phải set EditValue = null để xóa selection
                if (WarehouseNameSearchLookupEdit != null)
                {
                    WarehouseNameSearchLookupEdit.EditValue = null;
                }

                if (SupplierNameSearchLookupEdit != null)
                {
                    SupplierNameSearchLookupEdit.EditValue = null;
                }

                // Reset TextEdit
                if (StockInNumberTextEdit != null)
                {
                    StockInNumberTextEdit.Text = string.Empty;
                }

                if (PurchaseOrderSearchLookupEdit != null)
                {
                    PurchaseOrderSearchLookupEdit.Text = string.Empty;
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

        #endregion

        #region ========== HELPER METHODS ==========

        /// <summary>
        /// Tạo số phiếu nhập kho tự động
        /// Sử dụng BLL để tự động xác định PNK hay PXK dựa trên LoaiNhapXuatKhoEnum
        /// </summary>
        /// <param name="stockInDate">Ngày nhập kho</param>
        private void GenerateStockInNumber(DateTime stockInDate)
        {
            try
            {
                // Lấy loại nhập/xuất kho
                var loaiNhapXuatKho = LoaiNhapXuatKhoEnum.NhapHangThuongMai;

                // Gọi BLL để tạo số phiếu tự động (tự động xác định PNK hay PXK)
                var voucherNumber = _stockInOutMasterBll.GenerateVoucherNumber(stockInDate, loaiNhapXuatKho);

                if (StockInNumberTextEdit != null)
                {
                    StockInNumberTextEdit.Text = voucherNumber;
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi tạo số phiếu nhập");
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
}