using Bll.Inventory.InventoryManagement;
using Common.Common;
using Common.Helpers;
using Common.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DTO.Inventory.InventoryManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DTO.DeviceAssetManagement;

namespace Inventory.Management
{
    /// <summary>
    /// Form quản lý tài sản sử dụng GridView
    /// </summary>
    public partial class FrmAssetDtoManagement : XtraForm
    {
        #region ========== KHAI BÁO BIẾN ==========

        private List<AssetDto> _dataSource;
        private AssetBll _assetBll;

        #endregion

        #region ========== CONSTRUCTOR ==========

        /// <summary>
        /// Constructor mặc định
        /// </summary>
        public FrmAssetDtoManagement()
        {
            InitializeComponent();
            InitializeBll();
            InitializeGridView();
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
                _assetBll = new AssetBll();
            }
            catch (Exception ex)
            {
                MsgBox.ShowError($"Lỗi khởi tạo dịch vụ tài sản: {ex.Message}");
                _assetBll = null;
            }
        }

        /// <summary>
        /// Khởi tạo cấu hình GridView
        /// </summary>
        private void InitializeGridView()
        {
            // Cấu hình sắp xếp mặc định
            AssetDtoGridView.SortInfo.Clear();
            //AssetDtoGridView.SortInfo.Add(
            //    new DevExpress.XtraGrid.Columns.GridColumnSortInfo(colCreateDate, DevExpress.Data.ColumnSortOrder.Descending));
            
            // Enable footer để hiển thị summary (nếu cần)
            AssetDtoGridView.OptionsView.ShowFooter = false;
            
            // Cấu hình GridView cho UX tốt nhất
            AssetDtoGridView.OptionsView.EnableAppearanceEvenRow = true;
            AssetDtoGridView.OptionsView.EnableAppearanceOddRow = true;
            AssetDtoGridView.OptionsView.ShowIndicator = true;
            AssetDtoGridView.OptionsBehavior.AutoPopulateColumns = false;
            AssetDtoGridView.OptionsCustomization.AllowQuickHideColumns = true;
            AssetDtoGridView.OptionsCustomization.AllowColumnResizing = true;
            AssetDtoGridView.OptionsCustomization.AllowColumnMoving = true;
            AssetDtoGridView.OptionsCustomization.AllowGroup = false;
            AssetDtoGridView.OptionsFind.AlwaysVisible = true;
            AssetDtoGridView.OptionsFind.FindMode = DevExpress.XtraEditors.FindMode.Always;
            AssetDtoGridView.OptionsSelection.EnableAppearanceFocusedCell = true;
            AssetDtoGridView.OptionsSelection.EnableAppearanceFocusedRow = true;
        }

        /// <summary>
        /// Khởi tạo events
        /// </summary>
        private void InitializeEvents()
        {
            // Bar button events
            XemBarButtonItem.ItemClick += XemBarButtonItem_ItemClick;
            ThemMoiBarButtonItem.ItemClick += ThemMoiBarButtonItem_ItemClick;
            SuaBarButtonItem.ItemClick += SuaBarButtonItem_ItemClick;
            XoaBarButtonItem.ItemClick += XoaBarButtonItem_ItemClick;
            ExportFileBarButtonItem.ItemClick += ExportFileBarButtonItem_ItemClick;

            // GridView events
            AssetDtoGridView.SelectionChanged += AssetDtoGridView_SelectionChanged;
            AssetDtoGridView.CustomDrawRowIndicator += AssetDtoGridView_CustomDrawRowIndicator;
            AssetDtoGridView.RowCellStyle += AssetDtoGridView_RowCellStyle;
            AssetDtoGridView.DoubleClick += AssetDtoGridView_DoubleClick;

            // Form events
            Load += FrmAssetDtoManagement_Load;
        }

        #endregion

        #region ========== SỰ KIỆN FORM ==========

        /// <summary>
        /// Event handler khi form được load
        /// </summary>
        private void FrmAssetDtoManagement_Load(object sender, EventArgs e)
        {
            // Không tự động load, user phải click "Xem" để load
        }

        /// <summary>
        /// Event handler cho nút Xem
        /// </summary>
        private async void XemBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
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
        /// Event handler cho nút Thêm mới
        /// </summary>
        private void ThemMoiBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                // TODO: Mở form chi tiết để thêm mới tài sản
                MsgBox.ShowWarning("Chức năng Thêm mới tài sản đang được triển khai.\n\n" +
                              "Form chi tiết sẽ được tạo sau.", "Chức năng đang phát triển");
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khi thêm mới tài sản");
            }
        }

        /// <summary>
        /// Event handler cho nút Sửa
        /// </summary>
        private void SuaBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                var selectedRows = AssetDtoGridView.GetSelectedRows();
                if (selectedRows == null || selectedRows.Length == 0)
                {
                    MsgBox.ShowWarning("Vui lòng chọn tài sản cần sửa.");
                    return;
                }

                if (selectedRows.Length > 1)
                {
                    MsgBox.ShowWarning("Vui lòng chọn chỉ một tài sản để sửa.");
                    return;
                }

                var rowHandle = selectedRows[0];
                var assetDto = AssetDtoGridView.GetRow(rowHandle) as AssetDto;
                if (assetDto == null)
                {
                    MsgBox.ShowWarning("Không thể lấy thông tin tài sản được chọn.");
                    return;
                }

                // TODO: Mở form chi tiết để sửa tài sản
                MsgBox.ShowWarning($"Chức năng Sửa tài sản '{assetDto.AssetCode}' đang được triển khai.\n\n" +
                                   "Form chi tiết sẽ được tạo sau.", "Chức năng đang phát triển");
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khi sửa tài sản");
            }
        }

        /// <summary>
        /// Event handler cho nút Xóa
        /// </summary>
        private async void XoaBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (_assetBll == null)
                {
                    MsgBox.ShowWarning("Dịch vụ tài sản chưa được khởi tạo.");
                    return;
                }

                var selectedRows = AssetDtoGridView.GetSelectedRows();
                if (selectedRows == null || selectedRows.Length == 0)
                {
                    MsgBox.ShowWarning("Vui lòng chọn tài sản cần xóa.");
                    return;
                }

                // Lấy danh sách tài sản được chọn
                var assetsToDelete = new List<AssetDto>();
                foreach (var rowHandle in selectedRows)
                {
                    var assetDto = AssetDtoGridView.GetRow(rowHandle) as AssetDto;
                    if (assetDto != null)
                    {
                        assetsToDelete.Add(assetDto);
                    }
                }

                if (assetsToDelete.Count == 0)
                {
                    MsgBox.ShowWarning("Không có tài sản nào được chọn để xóa.");
                    return;
                }

                // Xác nhận xóa
                var confirmMessage = assetsToDelete.Count == 1
                    ? $"Bạn có chắc chắn muốn xóa tài sản '{assetsToDelete[0].AssetCode}' - '{assetsToDelete[0].AssetName}'?\n\n" +
                      "Hành động này không thể hoàn tác."
                    : $"Bạn có chắc chắn muốn xóa {assetsToDelete.Count} tài sản đã chọn?\n\n" +
                      "Hành động này không thể hoàn tác.";

                if (!MsgBox.ShowYesNo(confirmMessage, "Xác nhận xóa"))
                {
                    return;
                }

                // Thực hiện xóa
                await ExecuteWithWaitingFormAsync(async () =>
                {
                    int deletedCount = 0;
                    foreach (var assetDto in assetsToDelete)
                    {
                        try
                        {
                            _assetBll.Delete(assetDto.Id);
                            deletedCount++;
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"Error deleting asset {assetDto.AssetCode}: {ex.Message}");
                            // Tiếp tục xóa các tài sản khác
                        }
                    }

                    // Reload dữ liệu sau khi xóa
                    await ReloadDataWithoutWaitingFormAsync();

                    if (deletedCount > 0)
                    {
                        MsgBox.ShowSuccess($"Đã xóa thành công {deletedCount} tài sản.", "Xóa thành công");
                    }
                    else
                    {
                        MsgBox.ShowWarning("Không có tài sản nào được xóa.");
                    }
                });
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khi xóa tài sản");
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
                var rowCount = AssetDtoGridView.RowCount;
                if (rowCount == 0)
                {
                    MsgBox.ShowWarning("Không có dữ liệu để xuất. Vui lòng tải dữ liệu trước.");
                    return;
                }

                var defaultFileName = $"TaiSan_{DateTime.Now:yyyyMMdd_HHmmss}";

                // Sử dụng Common helper để xuất file
                GridViewHelper.ExportGridControl(AssetDtoGridView, defaultFileName);
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khi xuất file");
            }
        }

        /// <summary>
        /// Event handler khi selection thay đổi
        /// </summary>
        private void AssetDtoGridView_SelectionChanged(object sender, EventArgs e)
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
        /// Event handler khi double click vào row
        /// </summary>
        private void AssetDtoGridView_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                // Mở form chi tiết để sửa (tương tự như nút Sửa)
                SuaBarButtonItem_ItemClick(sender, null);
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khi mở chi tiết tài sản");
            }
        }

        /// <summary>
        /// Event handler cho CustomDrawRowIndicator
        /// </summary>
        private void AssetDtoGridView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                GridViewHelper.CustomDrawRowIndicator(AssetDtoGridView, e);
            }
            catch (Exception)
            {
                // Ignore errors
            }
        }

        /// <summary>
        /// Event handler cho RowCellStyle
        /// </summary>
        private void AssetDtoGridView_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            try
            {
                if (sender is not GridView) return;

                // Kiểm tra nếu đây là dòng dữ liệu hợp lệ
                if (e.RowHandle < 0) return;

                // Lấy dữ liệu từ row
                var row = AssetDtoGridView.GetRow(e.RowHandle) as AssetDto;
                if (row == null) return;

                // Highlight các tài sản có trạng thái "Ngừng sử dụng" hoặc "Thanh lý"
                if (e.Column.FieldName == "StatusDisplay")
                {
                    if (row.Status == 2) // Ngừng sử dụng
                    {
                        e.Appearance.BackColor = System.Drawing.Color.FromArgb(255, 243, 224);
                        e.Appearance.BackColor2 = System.Drawing.Color.FromArgb(255, 243, 224);
                    }
                    else if (row.Status == 3) // Thanh lý
                    {
                        e.Appearance.BackColor = System.Drawing.Color.FromArgb(255, 235, 238);
                        e.Appearance.BackColor2 = System.Drawing.Color.FromArgb(255, 235, 238);
                    }
                }

                // Highlight các tài sản có tình trạng "Kém"
                if (e.Column.FieldName == "ConditionDisplay")
                {
                    if (row.Condition == 4) // Kém
                    {
                        e.Appearance.BackColor = System.Drawing.Color.FromArgb(255, 235, 238);
                        e.Appearance.BackColor2 = System.Drawing.Color.FromArgb(255, 235, 238);
                        e.Appearance.ForeColor = System.Drawing.Color.DarkRed;
                    }
                }

                // Highlight các tài sản có giá trị hiện tại âm (bất thường)
                if (e.Column.FieldName == "CurrentValue")
                {
                    if (row.CurrentValue.HasValue && row.CurrentValue.Value < 0)
                    {
                        e.Appearance.BackColor = System.Drawing.Color.FromArgb(255, 235, 238);
                        e.Appearance.BackColor2 = System.Drawing.Color.FromArgb(255, 235, 238);
                        e.Appearance.ForeColor = System.Drawing.Color.DarkRed;
                    }
                }
            }
            catch (Exception)
            {
                // Ignore errors
            }
        }

        #endregion

        #region ========== LOAD DỮ LIỆU ==========

        /// <summary>
        /// Load dữ liệu tài sản từ database
        /// </summary>
        private async Task LoadDataAsync()
        {
            try
            {
                if (_assetBll == null)
                {
                    MsgBox.ShowWarning("Dịch vụ tài sản chưa được khởi tạo.");
                    LoadData([]);
                    return;
                }

                await ExecuteWithWaitingFormAsync(() =>
                {
                    // Query tất cả tài sản (UI -> BLL -> DAL)
                    // Repository đã tự động load navigation properties qua DataLoadOptions
                    var entities = _assetBll.QueryAssets(
                        companyId: null,
                        branchId: null,
                        departmentId: null,
                        employeeId: null,
                        productVariantId: null,
                        assetType: null,
                        assetCategory: null,
                        status: null,
                        condition: null,
                        fromDate: null,
                        toDate: null,
                        isActive: true); // Chỉ lấy tài sản đang hoạt động

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
        /// Load dữ liệu tài sản
        /// </summary>
        private void LoadData(List<AssetDto> assets)
        {
            if (assets == null)
            {
                _dataSource = null;
                assetDtoBindingSource.DataSource = null;
                assetDtoBindingSource.ResetBindings(false);
                AssetDtoGridView.RefreshData();
                UpdateStatusBar();
                return;
            }

            // Lưu reference
            _dataSource = assets;
            assetDtoBindingSource.DataSource = assets;
            assetDtoBindingSource.ResetBindings(false);

            // Force refresh GridView
            AssetDtoGridView.RefreshData();

            UpdateStatusBar();

            // Log để debug
            System.Diagnostics.Debug.WriteLine($"LoadData: Đã load {assets.Count} tài sản vào grid");
        }

        /// <summary>
        /// Cập nhật status bar
        /// </summary>
        private void UpdateStatusBar()
        {
            try
            {
                var totalCount = _dataSource?.Count ?? 0;
                var selectedCount = AssetDtoGridView.SelectedRowsCount;

                // Tính tổng giá trị hiện tại
                var totalCurrentValue = _dataSource?
                    .Where(a => a.CurrentValue.HasValue)
                    .Sum(a => a.CurrentValue.Value) ?? 0m;

                DataSummaryBarStaticItem.Caption = $@"Tổng số: <b>{totalCount}</b> tài sản | " +
                                                   $@"Tổng giá trị: <b>{totalCurrentValue:N0}</b> VNĐ";
                SelectedRowBarStaticItem.Caption = selectedCount > 0
                    ? $"Đã chọn: <b>{selectedCount}</b> tài sản"
                    : "Chưa chọn dòng nào";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating status bar: {ex.Message}");
            }
        }

        /// <summary>
        /// Load dữ liệu mà không hiển thị WaitingForm (dùng khi đã có WaitingForm đang hiển thị)
        /// </summary>
        private async Task ReloadDataWithoutWaitingFormAsync()
        {
            try
            {
                if (_assetBll == null)
                {
                    return;
                }

                // Query tất cả tài sản (UI -> BLL -> DAL)
                var entities = _assetBll.QueryAssets(
                    companyId: null,
                    branchId: null,
                    departmentId: null,
                    employeeId: null,
                    productVariantId: null,
                    assetType: null,
                    assetCategory: null,
                    status: null,
                    condition: null,
                    fromDate: null,
                    toDate: null,
                    isActive: true);

                // Convert entities sang DTOs sử dụng converter
                var dtos = entities.ToDtoList();

                // Set data source
                LoadData(dtos);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error reloading data: {ex.Message}");
                LoadData([]);
            }
        }

        /// <summary>
        /// Thực hiện operation async với WaitingForm hiển thị
        /// </summary>
        private async Task ExecuteWithWaitingFormAsync(Func<Task> operation)
        {
            try
            {
                // Kiểm tra xem đã có WaitingForm đang hiển thị chưa
                if (!SplashScreenHelper.IsSplashScreenVisible())
                {
                    // Hiển thị WaitingForm
                    DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitForm1));
                }

                // Thực hiện operation
                await operation();
            }
            catch (Exception e)
            {
                MsgBox.ShowException(e);
            }
            finally
            {
                // Đóng WaitingForm nếu đang hiển thị
                if (SplashScreenHelper.IsSplashScreenVisible())
                {
                    DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
                }
            }
        }

        #endregion

        #region ========== HELPER METHODS ==========

        /// <summary>
        /// Hiển thị lỗi
        /// </summary>
        private void ShowError(Exception ex, string title)
        {
            MsgBox.ShowError($"{title}: {ex.Message}", title);
            System.Diagnostics.Debug.WriteLine($"{title}: {ex}");
        }

        #endregion
    }
}
