using Bll.Inventory.InventoryManagement;
using Common.Common;
using Common.Helpers;
using Common.Utils;
using Dal.DataAccess.Implementations.Inventory.StockIn;
using DTO.Inventory.InventoryManagement;
using Inventory.StockIn.InPhieu;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventory.InventoryManagement
{
    /// <summary>
    /// Form xem lịch sử nhập xuất kho
    /// Cung cấp chức năng xem, tìm kiếm, in phiếu và quản lý hình ảnh/bảo hành
    /// </summary>
    public partial class FrmStockInOutHistory : DevExpress.XtraEditors.XtraForm
    {
        #region ========== FIELDS & PROPERTIES ==========

        /// <summary>
        /// Business Logic Layer cho phiếu nhập xuất kho
        /// </summary>
        private readonly StockInOutMasterBll _stockInOutMasterBll = new StockInOutMasterBll();

        /// <summary>
        /// Logger để ghi log các sự kiện
        /// </summary>
        private readonly ILogger _logger = LoggerFactory.CreateLogger(LogCategory.UI);

        /// <summary>
        /// ID phiếu nhập xuất kho được chọn
        /// </summary>
        private Guid? _selectedStockInOutMasterId;

        #endregion

        #region ========== CONSTRUCTOR ==========

        public FrmStockInOutHistory()
        {
            InitializeComponent();
            InitializeForm();
        }

        #endregion

        #region ========== INITIALIZATION ==========

        /// <summary>
        /// Khởi tạo form
        /// </summary>
        private void InitializeForm()
        {
            try
            {
                _logger.Debug("InitializeForm: Initializing form");

                // Cấu hình GridView
                //ConfigureGridView();

                // Setup events
                SetupEvents();

                // Khởi tạo giá trị mặc định cho date pickers
                InitializeDateFilters();

                _logger.Info("InitializeForm: Form initialized successfully");
            }
            catch (Exception ex)
            {
                _logger.Error("InitializeForm: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi khởi tạo form: {ex.Message}");
            }
        }

        /// <summary>
        /// Khởi tạo giá trị mặc định cho date filters
        /// </summary>
        private void InitializeDateFilters()
        {
            try
            {
                // Từ ngày: đầu tháng hiện tại
                var fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                TuNgayBarEditItem.EditValue = fromDate;

                // Đến ngày: ngày hiện tại
                DenNgayBarEditItem.EditValue = DateTime.Now;
            }
            catch (Exception ex)
            {
                _logger.Error("InitializeDateFilters: Exception occurred", ex);
            }
        }

        /// <summary>
        /// Setup các event handlers
        /// </summary>
        private void SetupEvents()
        {
            try
            {
                // Bar button events
                XemBaoCaoBarButtonItem.ItemClick += XemBaoCaoBarButtonItem_ItemClick;
                InPhieuBarButtonItem.ItemClick += InPhieuBarButtonItem_ItemClick;
                NhapBaoHanhBarButtonItem.ItemClick += NhapBaoHanhBarButtonItem_ItemClick;
                ThemHinhAnhBarButtonItem.ItemClick += ThemHinhAnhBarButtonItem_ItemClick;

                // GridView events
                StockInOutMasterHistoryDtoGridView.DoubleClick += StockInOutMasterHistoryDtoGridView_DoubleClick;
                StockInOutMasterHistoryDtoGridView.FocusedRowChanged += StockInOutMasterHistoryDtoGridView_FocusedRowChanged;

                // Form events
                Load += FrmStockInOutHistory_Load;
            }
            catch (Exception ex)
            {
                _logger.Error("SetupEvents: Exception occurred", ex);
            }
        }

        /// <summary>
        /// Cấu hình GridView để hiển thị dữ liệu theo chuẩn Common
        /// </summary>
        private void ConfigureGridView()
        {
            // Cấu hình GridView để hiển thị multi-line và wrap text
            var textColumns = new List<string> { "FullContentHtml" };
            GridViewHelper.ConfigureMultiLineGridView(
                StockInOutMasterHistoryDtoGridView,
                textColumns,
                enableAutoFilter: true,
                centerHeaders: false); // Đã format header trong Designer, không cần centerHeaders từ Helper

            // Cấu hình filter cho cột FullContentHtml
            if (colFullContentHtml != null)
            {
                colFullContentHtml.OptionsFilter.AllowFilter = true;
                colFullContentHtml.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
            }

            // Cấu hình hiển thị (một số đã được set trong Designer, nhưng giữ lại để đảm bảo)
            StockInOutMasterHistoryDtoGridView.OptionsView.ShowGroupPanel = false;
            StockInOutMasterHistoryDtoGridView.OptionsView.RowAutoHeight = true;
            StockInOutMasterHistoryDtoGridView.OptionsSelection.MultiSelect = true;
            StockInOutMasterHistoryDtoGridView.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            StockInOutMasterHistoryDtoGridView.OptionsFind.AlwaysVisible = true;
        }

        #endregion

        #region ========== EVENT HANDLERS ==========

        /// <summary>
        /// Event handler khi form được load
        /// </summary>
        private async void FrmStockInOutHistory_Load(object sender, EventArgs e)
        {
            try
            {
                _logger.Debug("FrmStockInOutHistory_Load: Form loading");

                // Tự động load data khi form được mở
                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                _logger.Error("FrmStockInOutHistory_Load: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi khởi tạo form: {ex.Message}");
            }
        }

        /// <summary>
        /// Event handler cho nút Xem báo cáo
        /// </summary>
        private async void XemBaoCaoBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                _logger.Debug("XemBaoCaoBarButtonItem_ItemClick: View report button clicked");

                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                _logger.Error("XemBaoCaoBarButtonItem_ItemClick: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi tải dữ liệu: {ex.Message}");
            }
        }

        /// <summary>
        /// Event handler cho nút In phiếu
        /// </summary>
        private void InPhieuBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                _logger.Debug("InPhieuBarButtonItem_ItemClick: Print button clicked");

                if (!_selectedStockInOutMasterId.HasValue || _selectedStockInOutMasterId.Value == Guid.Empty)
                {
                    MsgBox.ShowWarning("Vui lòng chọn phiếu nhập xuất kho để in.");
                    return;
                }

                // In phiếu nhập kho với preview
                StockInReportHelper.PrintStockInVoucher(_selectedStockInOutMasterId.Value);

                _logger.Info("InPhieuBarButtonItem_ItemClick: Print voucher completed, StockInOutMasterId={0}", 
                    _selectedStockInOutMasterId.Value);
            }
            catch (Exception ex)
            {
                _logger.Error("InPhieuBarButtonItem_ItemClick: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi in phiếu: {ex.Message}");
            }
        }

        /// <summary>
        /// Event handler cho nút Nhập bảo hành
        /// </summary>
        private void NhapBaoHanhBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                _logger.Debug("NhapBaoHanhBarButtonItem_ItemClick: Warranty button clicked");

                if (!_selectedStockInOutMasterId.HasValue || _selectedStockInOutMasterId.Value == Guid.Empty)
                {
                    MsgBox.ShowWarning("Vui lòng chọn phiếu nhập xuất kho để nhập bảo hành.");
                    return;
                }

                // Mở form nhập bảo hành
                using (var form = new FrmWarranty(_selectedStockInOutMasterId.Value))
                {
                    form.StartPosition = FormStartPosition.CenterParent;
                    form.ShowDialog(this);
                }

                _logger.Info("NhapBaoHanhBarButtonItem_ItemClick: Warranty form opened, StockInOutMasterId={0}", 
                    _selectedStockInOutMasterId.Value);
            }
            catch (Exception ex)
            {
                _logger.Error("NhapBaoHanhBarButtonItem_ItemClick: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi mở form bảo hành: {ex.Message}");
            }
        }

        /// <summary>
        /// Event handler cho nút Thêm hình ảnh
        /// </summary>
        private void ThemHinhAnhBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                _logger.Debug("ThemHinhAnhBarButtonItem_ItemClick: Add image button clicked");

                if (!_selectedStockInOutMasterId.HasValue || _selectedStockInOutMasterId.Value == Guid.Empty)
                {
                    MsgBox.ShowWarning("Vui lòng chọn phiếu nhập xuất kho để thêm hình ảnh.");
                    return;
                }

                // Mở form thêm hình ảnh
                using (var form = new FrmStockInOutAddImages(_selectedStockInOutMasterId.Value))
                {
                    form.StartPosition = FormStartPosition.CenterParent;
                    form.ShowDialog(this);
                }

                _logger.Info("ThemHinhAnhBarButtonItem_ItemClick: Add image form opened, StockInOutMasterId={0}", 
                    _selectedStockInOutMasterId.Value);
            }
            catch (Exception ex)
            {
                _logger.Error("ThemHinhAnhBarButtonItem_ItemClick: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi mở form thêm hình ảnh: {ex.Message}");
            }
        }

        /// <summary>
        /// Event handler khi double click trên GridView
        /// </summary>
        private void StockInOutMasterHistoryDtoGridView_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (_selectedStockInOutMasterId.HasValue && _selectedStockInOutMasterId.Value != Guid.Empty)
                {
                    // Có thể mở form chi tiết hoặc in phiếu
                    InPhieuBarButtonItem_ItemClick(sender, null);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("StockInOutMasterHistoryDtoGridView_DoubleClick: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi: {ex.Message}");
            }
        }

        /// <summary>
        /// Event handler khi row được chọn thay đổi
        /// </summary>
        private void StockInOutMasterHistoryDtoGridView_FocusedRowChanged(object sender, 
            DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                UpdateSelectedItem();
            }
            catch (Exception ex)
            {
                _logger.Error("StockInOutMasterHistoryDtoGridView_FocusedRowChanged: Exception occurred", ex);
            }
        }

        #endregion

        #region ========== DATA LOADING ==========

        /// <summary>
        /// Load dữ liệu lịch sử nhập xuất kho
        /// </summary>
        private async Task LoadDataAsync()
        {
            try
            {
                _logger.Debug("LoadDataAsync: Starting to load history data");

                // Hiển thị SplashScreen
                SplashScreenHelper.ShowWaitingSplashScreen();

                try
                {
                    await LoadDataWithoutSplashAsync();
                }
                finally
                {
                    // Đóng SplashScreen
                    SplashScreenHelper.CloseSplashScreen();
                }

                _logger.Info("LoadDataAsync: History data loaded successfully");
            }
            catch (Exception ex)
            {
                _logger.Error("LoadDataAsync: Exception occurred", ex);
                SplashScreenHelper.CloseSplashScreen();
                MsgBox.ShowError($"Lỗi tải dữ liệu: {ex.Message}");
            }
        }

        /// <summary>
        /// Load dữ liệu không hiển thị SplashScreen (dùng cho refresh)
        /// </summary>
        private async Task LoadDataWithoutSplashAsync()
        {
            await Task.Run(() =>
            {
                try
                {
                    // Lấy giá trị từ date pickers
                    var fromDate = TuNgayBarEditItem.EditValue as DateTime? ?? 
                        new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    var toDate = DenNgayBarEditItem.EditValue as DateTime? ?? DateTime.Now;

                    // Validate date range
                    if (fromDate > toDate)
                    {
                        BeginInvoke(new Action(() =>
                        {
                            MsgBox.ShowWarning("Từ ngày không được lớn hơn đến ngày.");
                        }));
                        return;
                    }

                    // Tạo query criteria
                    var query = new StockInHistoryQueryCriteria
                    {
                        FromDate = fromDate.Date,
                        ToDate = toDate.Date.AddDays(1).AddTicks(-1) // Đến cuối ngày
                    };

                    // Lấy dữ liệu từ BLL
                    var entities = _stockInOutMasterBll.QueryHistory(query);

                    // Convert sang DTO
                    var dtos = entities.Select(e => e.ToDto()).ToList();

                    // Update UI thread
                    BeginInvoke(new Action(() =>
                    {
                        stockInOutMasterHistoryDtoBindingSource.DataSource = dtos;
                        stockInOutMasterHistoryDtoBindingSource.ResetBindings(false);

                        // Reset selection
                        _selectedStockInOutMasterId = null;
                        UpdateButtonStates();
                    }));

                    _logger.Info("LoadDataWithoutSplashAsync: Loaded {0} records", dtos.Count);
                }
                catch (Exception ex)
                {
                    _logger.Error("LoadDataWithoutSplashAsync: Exception occurred", ex);
                    BeginInvoke(new Action(() =>
                    {
                        MsgBox.ShowError($"Lỗi tải dữ liệu: {ex.Message}");
                    }));
                }
            });
        }

        /// <summary>
        /// Cập nhật item được chọn từ GridView
        /// </summary>
        private void UpdateSelectedItem()
        {
            try
            {
                var focusedRowHandle = StockInOutMasterHistoryDtoGridView.FocusedRowHandle;
                
                if (focusedRowHandle >= 0)
                {
                    var dto = StockInOutMasterHistoryDtoGridView.GetRow(focusedRowHandle) 
                        as StockInOutMasterHistoryDto;
                    
                    if (dto != null)
                    {
                        _selectedStockInOutMasterId = dto.Id;
                    }
                    else
                    {
                        _selectedStockInOutMasterId = null;
                    }
                }
                else
                {
                    _selectedStockInOutMasterId = null;
                }

                UpdateButtonStates();
            }
            catch (Exception ex)
            {
                _logger.Error("UpdateSelectedItem: Exception occurred", ex);
            }
        }

        /// <summary>
        /// Cập nhật trạng thái các nút
        /// </summary>
        private void UpdateButtonStates()
        {
            try
            {
                var hasSelection = _selectedStockInOutMasterId.HasValue && 
                    _selectedStockInOutMasterId.Value != Guid.Empty;

                InPhieuBarButtonItem.Enabled = hasSelection;
                NhapBaoHanhBarButtonItem.Enabled = hasSelection;
                ThemHinhAnhBarButtonItem.Enabled = hasSelection;
            }
            catch (Exception ex)
            {
                _logger.Error("UpdateButtonStates: Exception occurred", ex);
            }
        }

        #endregion
    }
}