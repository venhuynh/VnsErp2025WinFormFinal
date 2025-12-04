using Bll.Inventory.InventoryManagement;
using Common.Common;
using Common.Helpers;
using Common.Utils;
using DevExpress.XtraEditors;
using DTO.Inventory.InventoryManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventory.Management
{
    /// <summary>
    /// Form hiển thị danh sách tồn kho theo tháng sử dụng GridView
    /// </summary>
    public partial class FrmInventoryBalanceDto : XtraForm
    {
        #region ========== KHAI BÁO BIẾN ==========

        private List<InventoryBalanceDto> _dataSource;
        private InventoryBalanceBll _inventoryBalanceBll;

        #endregion

        #region ========== CONSTRUCTOR ==========

        /// <summary>
        /// Constructor mặc định
        /// </summary>
        public FrmInventoryBalanceDto()
        {
            InitializeComponent();
            InitializeBll();
            InitializeGridView();
            InitializeFilters();
            InitializeEvents();
        }

        #endregion

        #region ========== KHỞI TẠO FORM ==========

        /// <summary>
        /// Khởi tạo BLL
        /// </summary>
        private void InitializeBll()
        {
            try
            {
                _inventoryBalanceBll = new InventoryBalanceBll();
            }
            catch (Exception ex)
            {
                MsgBox.ShowError($"Lỗi khởi tạo dịch vụ tồn kho: {ex.Message}");
                _inventoryBalanceBll = null;
            }
        }

        /// <summary>
        /// Khởi tạo cấu hình GridView
        /// </summary>
        private void InitializeGridView()
        {
            // Cấu hình group theo GroupCaption
            InventoryBalanceDtoGridView.SortInfo.Clear();
            InventoryBalanceDtoGridView.SortInfo.Add(
                new DevExpress.XtraGrid.Columns.GridColumnSortInfo(colGroupCaption, DevExpress.Data.ColumnSortOrder.Ascending));
        }

        /// <summary>
        /// Khởi tạo giá trị mặc định cho filters
        /// </summary>
        private void InitializeFilters()
        {
            try
            {
                // Năm: năm hiện tại
                NamBarEditItem.EditValue = DateTime.Now.Year;

                // Tháng: tháng hiện tại
                ThangBarEditItem.EditValue = DateTime.Now.Month;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error initializing filters: {ex.Message}");
            }
        }

        /// <summary>
        /// Khởi tạo events
        /// </summary>
        private void InitializeEvents()
        {
            // Bar button events
            XemBaoCaoBarButtonItem.ItemClick += XemBaoCaoBarButtonItem_ItemClick;
            ExportFileBarButtonItem.ItemClick += ExportFileBarButtonItem_ItemClick;

            // GridView events
            InventoryBalanceDtoGridView.SelectionChanged += InventoryBalanceDtoGridView_SelectionChanged;

            // Form events
            Load += FrmInventoryBalanceDto_Load;
        }

        #endregion

        #region ========== SỰ KIỆN FORM ==========

        /// <summary>
        /// Event handler khi form được load
        /// </summary>
        private void FrmInventoryBalanceDto_Load(object sender, EventArgs e)
        {
            // Không tự động load, user phải click "Xem" để load
        }

        /// <summary>
        /// Event handler cho nút Xem
        /// </summary>
        private async void XemBaoCaoBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khi tải dữ liệu");
            }
        }

        /// <summary>
        /// Event handler khi selection thay đổi
        /// </summary>
        private void InventoryBalanceDtoGridView_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                UpdateStatusBar();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating status bar: {ex.Message}");
            }
        }

        /// <summary>
        /// Event handler cho nút Xuất file
        /// </summary>
        private void ExportFileBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                // Kiểm tra có dữ liệu không
                var rowCount = InventoryBalanceDtoGridView.RowCount;
                if (rowCount == 0)
                {
                    MsgBox.ShowWarning("Không có dữ liệu để xuất. Vui lòng tải dữ liệu trước.");
                    return;
                }

                // Lấy năm/tháng để đặt tên file
                var periodYear = NamBarEditItem.EditValue as int? ?? DateTime.Now.Year;
                var periodMonth = ThangBarEditItem.EditValue as int? ?? DateTime.Now.Month;
                var defaultFileName = $"TonKho_{periodYear}_{periodMonth:D2}";

                // Sử dụng Common helper để xuất file
                GridViewHelper.ExportGridControl(InventoryBalanceDtoGridView, defaultFileName);
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khi xuất file");
            }
        }

        #endregion

        #region ========== LOAD DỮ LIỆU ==========

        /// <summary>
        /// Load dữ liệu tồn kho từ database
        /// </summary>
        private async Task LoadDataAsync()
        {
            try
            {
                if (_inventoryBalanceBll == null)
                {
                    MsgBox.ShowWarning("Dịch vụ tồn kho chưa được khởi tạo.");
                    LoadData([]);
                    return;
                }

                await ExecuteWithWaitingFormAsync(() =>
                {
                    // Lấy filter criteria
                    var periodYear = NamBarEditItem.EditValue as int? ?? DateTime.Now.Year;
                    var periodMonth = ThangBarEditItem.EditValue as int? ?? DateTime.Now.Month;

                    // Validate period
                    if (periodYear < 2000 || periodYear > 9999)
                    {
                        MsgBox.ShowWarning("Năm phải trong khoảng 2000-9999.");
                        LoadData([]);
                        return Task.CompletedTask;
                    }

                    if (periodMonth < 1 || periodMonth > 12)
                    {
                        MsgBox.ShowWarning("Tháng phải trong khoảng 1-12.");
                        LoadData([]);
                        return Task.CompletedTask;
                    }

                    // Query tồn kho theo kỳ (UI -> BLL -> DAL)
                    // Repository đã tự động load navigation properties qua DataLoadOptions
                    var entities = _inventoryBalanceBll.QueryBalances(
                        warehouseId: null,
                        productVariantId: null,
                        periodYear: periodYear,
                        periodMonth: periodMonth);

                    // Convert entities sang DTOs sử dụng converter
                    // Converter tự động map navigation properties từ entity sang DTO
                    var dtos = entities.ToDtoList();

                    // Set data source
                    LoadData(dtos);
                    return Task.CompletedTask;
                });
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khi tải dữ liệu");
                LoadData([]);
            }
        }

        /// <summary>
        /// Load dữ liệu tồn kho
        /// </summary>
        public void LoadData(List<InventoryBalanceDto> balances)
        {
            if (balances == null)
            {
                _dataSource = null;
                inventoryBalanceDtoBindingSource.DataSource = null;
                inventoryBalanceDtoBindingSource.ResetBindings(false);
                InventoryBalanceDtoGridView.RefreshData();
                UpdateStatusBar();
                return;
            }

            // Lưu reference
            _dataSource = balances;
            inventoryBalanceDtoBindingSource.DataSource = balances;
            inventoryBalanceDtoBindingSource.ResetBindings(false);

            // Force refresh GridView
            InventoryBalanceDtoGridView.RefreshData();

            UpdateStatusBar();

            // Log để debug
            System.Diagnostics.Debug.WriteLine($"LoadData: Đã load {balances.Count} tồn kho vào grid");
        }


        /// <summary>
        /// Cập nhật status bar
        /// </summary>
        private void UpdateStatusBar()
        {
            try
            {
                var totalCount = _dataSource?.Count ?? 0;
                var selectedCount = InventoryBalanceDtoGridView.SelectedRowsCount;

                DataSummaryBarStaticItem.Caption = $@"Tổng số: <b>{totalCount}</b> tồn kho";
                SelectedRowBarStaticItem.Caption = selectedCount > 0
                    ? $"Đã chọn: <b>{selectedCount}</b> tồn kho"
                    : "Chưa chọn dòng nào";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating status bar: {ex.Message}");
            }
        }

        /// <summary>
        /// Thực hiện operation async với WaitingForm hiển thị
        /// </summary>
        private async Task ExecuteWithWaitingFormAsync(Func<Task> operation)
        {
            try
            {
                // Hiển thị WaitingForm
                DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitForm1));

                // Thực hiện operation
                await operation();
            }
            catch (Exception e)
            {
                MsgBox.ShowException(e);
            }
            finally
            {
                // Đóng WaitingForm
                DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
            }
        }

        #endregion

        #region ========== TIỆN ÍCH ==========

        /// <summary>
        /// Hiển thị lỗi với thông tin ngữ cảnh
        /// </summary>
        private void ShowError(Exception ex, string context = null)
        {
            MsgBox.ShowException(
                string.IsNullOrWhiteSpace(context) ? ex : new Exception($"{context}: {ex.Message}", ex));
        }

        #endregion
    }
}
