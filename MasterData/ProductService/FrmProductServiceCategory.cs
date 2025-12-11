using Bll.MasterData.ProductServiceBll;
using Common.Common;
using Common.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSplashScreen;
using DTO.MasterData.ProductService;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MasterData.ProductService
{
    /// <summary>
    /// Form quản lý danh sách danh mục sản phẩm/dịch vụ.
    /// Cung cấp giao diện hiển thị dạng danh sách, tìm kiếm, thêm mới, sửa, xóa và xuất dữ liệu danh mục.
    /// </summary>
    public partial class FrmProductServiceCategory : XtraForm
    {
        #region ========== KHAI BÁO BIẾN ==========

        /// <summary>
        /// Business Logic Layer cho danh mục sản phẩm/dịch vụ
        /// </summary>
        private readonly ProductServiceCategoryBll _productServiceCategoryBll = new ProductServiceCategoryBll();

        /// <summary>
        /// Logger cho logging
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Danh sách ID danh mục được chọn
        /// </summary>
        private readonly List<Guid> _selectedCategoryIds = [];

        /// <summary>
        /// Trạng thái đang tải dữ liệu (guard tránh gọi LoadDataAsync song song)
        /// </summary>
        private bool _isLoading;

        #endregion

        #region ========== CONSTRUCTOR & PUBLIC METHODS ==========

        /// <summary>
        /// Khởi tạo Form quản lý danh sách danh mục sản phẩm/dịch vụ.
        /// </summary>
        public FrmProductServiceCategory()
        {
            InitializeComponent();

            // Khởi tạo logger
            _logger = LoggerFactory.CreateLogger(LogCategory.UI);

            // Toolbar events
            ListDataBarButtonItem.ItemClick += ListDataBarButtonItem_ItemClick;
            NewBarButtonItem.ItemClick += NewBarButtonItem_ItemClick;
            EditBarButtonItem.ItemClick += EditBarButtonItem_ItemClick;
            DeleteBarButtonItem.ItemClick += DeleteBarButtonItem_ItemClick;
            ExportBarButtonItem.ItemClick += ExportBarButtonItem_ItemClick;

            // GridView events
            ProductServiceCategoryDtoGridView.SelectionChanged += ProductServiceCategoryDtoGridView_SelectionChanged;
            ProductServiceCategoryDtoGridView.CustomDrawRowIndicator += ProductServiceCategoryDtoGridView_CustomDrawRowIndicator;

            UpdateButtonStates();

            // Setup SuperToolTips
            SetupSuperToolTips();
        }

        #endregion

        #region ========== QUẢN LÝ DỮ LIỆU ==========

        /// <summary>
        /// Tải dữ liệu và bind vào GridView (Async, hiển thị WaitForm).
        /// </summary>
        private async Task LoadDataAsync()
        {
            if (_isLoading) return; // tránh re-entrancy
            _isLoading = true;
            try
            {
                await ExecuteWithWaitingFormAsync(async () => { await LoadDataAsyncWithoutSplash(); });
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi tải dữ liệu");
            }
            finally
            {
                _isLoading = false;
            }
        }

        /// <summary>
        /// Tải dữ liệu và bind vào GridView (Async, không hiển thị WaitForm).
        /// </summary>
        private async Task LoadDataAsyncWithoutSplash()
        {
            try
            {
                var (categories, counts) = await _productServiceCategoryBll.GetCategoriesWithCountsAsync();

                // Log: Kiểm tra dữ liệu counts
                _logger.Debug("=== LoadDataAsyncWithoutSplash Debug ===");
                _logger.Debug("Total categories: {0}", categories.Count);
                _logger.Debug("Total counts: {0}", counts.Count);

                foreach (var count in counts)
                {
                    var category = categories.FirstOrDefault(c => c.Id == count.Key);
                    _logger.Debug("Category: {0}, Count: {1}", category?.CategoryName ?? "Unknown", count.Value);
                }

                // Tạo cấu trúc cây hierarchical
                var dtoList = categories.ToDtosWithHierarchy(counts).ToList();

                // Log: Kiểm tra DTOs
                foreach (var dto in dtoList)
                {
                    _logger.Debug("DTO: {0}, Level: {1}, ProductCount: {2}", dto.CategoryName, dto.Level, dto.ProductCount);
                }

                BindGrid(dtoList);
                // UpdateButtonStates() sẽ được gọi trong BindGrid -> ClearSelectionState()
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi tải dữ liệu");
            }
        }

        /// <summary>
        /// Bind danh sách DTO vào GridView và cấu hình hiển thị.
        /// </summary>
        private void BindGrid(List<ProductServiceCategoryDto> data)
        {
            // Clear selection trước khi bind data mới
            ClearSelectionState();

            productServiceCategoryDtoBindingSource.DataSource = data;
            ProductServiceCategoryDtoGridView.BestFitColumns();
            ConfigureMultiLineGridView();

            // Đảm bảo selection được clear sau khi bind
            ClearSelectionState();
            
            // Cập nhật summary và selection info
            UpdateStatusBar();
        }

        #endregion

        #region ========== SỰ KIỆN FORM ==========

        /// <summary>
        /// Xử lý sự kiện click button Tải dữ liệu
        /// </summary>
        private async void ListDataBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi tải dữ liệu");
            }
        }

        /// <summary>
        /// Xử lý sự kiện click button Thêm mới
        /// </summary>
        private async void NewBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                using (OverlayManager.ShowScope(this))
                {
                    using (var form = new FrmProductServiceCategoryDetail(Guid.Empty))
                    {
                        form.StartPosition = FormStartPosition.CenterParent;
                        form.ShowDialog(this);

                        await LoadDataAsync();
                        UpdateButtonStates();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi hiển thị màn hình thêm mới");
            }
        }

        /// <summary>
        /// Xử lý sự kiện click button Điều chỉnh
        /// </summary>
        private async void EditBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                // Chỉ cho phép chỉnh sửa 1 dòng dữ liệu
                if (_selectedCategoryIds == null || _selectedCategoryIds.Count == 0)
                {
                    ShowInfo("Vui lòng chọn một dòng để chỉnh sửa.");
                    return;
                }

                if (_selectedCategoryIds.Count > 1)
                {
                    ShowInfo("Chỉ cho phép chỉnh sửa 1 dòng. Vui lòng bỏ chọn bớt.");
                    return;
                }

                var id = _selectedCategoryIds[0];
                var focusedRowHandle = ProductServiceCategoryDtoGridView.FocusedRowHandle;
                ProductServiceCategoryDto dto = null;

                if (focusedRowHandle >= 0)
                {
                    // Lấy dữ liệu từ focused row
                    dto = ProductServiceCategoryDtoGridView.GetRow(focusedRowHandle) as ProductServiceCategoryDto;
                }

                if (dto == null || dto.Id != id)
                {
                    // Tìm đúng DTO theo Id trong datasource nếu FocusedRow không khớp selection
                    if (productServiceCategoryDtoBindingSource.DataSource is IEnumerable list)
                    {
                        foreach (var item in list)
                        {
                            if (item is ProductServiceCategoryDto x && x.Id == id)
                            {
                                dto = x;
                                break;
                            }
                        }
                    }
                }

                if (dto == null)
                {
                    ShowInfo("Không thể xác định dòng được chọn để chỉnh sửa.");
                    return;
                }

                try
                {
                    using (OverlayManager.ShowScope(this))
                    {
                        using (var form = new FrmProductServiceCategoryDetail(dto.Id))
                        {
                            form.StartPosition = FormStartPosition.CenterParent;
                            form.ShowDialog(this);

                            await LoadDataAsync();
                            UpdateButtonStates();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ShowError(ex, "Lỗi hiển thị màn hình điều chỉnh");
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// Xử lý sự kiện click button Xóa
        /// </summary>
        private async void DeleteBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (_selectedCategoryIds == null || _selectedCategoryIds.Count == 0)
                {
                    ShowInfo("Vui lòng chọn ít nhất một dòng để xóa.");
                    return;
                }

                // Log: Kiểm tra danh sách selected IDs
                _logger.Debug("Selected Category IDs: {0}", string.Join(", ", _selectedCategoryIds));

                var confirmMessage = _selectedCategoryIds.Count == 1
                    ? "Bạn có chắc muốn xóa dòng dữ liệu đã chọn? (Sản phẩm/dịch vụ sẽ được chuyển sang 'Phân loại chưa đặt tên')"
                    : $"Bạn có chắc muốn xóa {_selectedCategoryIds.Count} dòng dữ liệu đã chọn? (Sản phẩm/dịch vụ sẽ được chuyển sang 'Phân loại chưa đặt tên')";

                if (!MsgBox.ShowYesNo(confirmMessage)) return;

                try
                {
                    await ExecuteWithWaitingFormAsync(async () =>
                    {
                        // Xóa theo thứ tự: con trước, cha sau để tránh lỗi foreign key constraint
                        await _productServiceCategoryBll.DeleteCategoriesWithProductMigration(_selectedCategoryIds.ToList());
                    });

                    ListDataBarButtonItem.PerformClick();
                }
                catch (Exception ex)
                {
                    ShowError(ex, "Lỗi xóa dữ liệu");
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// Xử lý sự kiện click button Xuất dữ liệu
        /// </summary>
        private void ExportBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Chỉ cho phép xuất khi có dữ liệu hiển thị
            var rowCount = ProductServiceCategoryDtoGridView.RowCount;
            if (rowCount <= 0)
            {
                ShowInfo("Không có dữ liệu để xuất.");
                return;
            }

            // Export GridView data
            try
            {
                var saveDialog = new SaveFileDialog
                {
                    Filter = @"Excel Files (*.xlsx)|*.xlsx|All Files (*.*)|*.*",
                    FileName = "ProductServiceCategories.xlsx"
                };

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    ProductServiceCategoryDtoGridControl.ExportToXlsx(saveDialog.FileName);
                    ShowInfo("Xuất dữ liệu thành công!");
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi xuất dữ liệu");
            }
        }

        /// <summary>
        /// Xử lý sự kiện thay đổi selection trên GridView
        /// </summary>
        private void ProductServiceCategoryDtoGridView_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                // Cập nhật danh sách selected IDs khi selection thay đổi
                UpdateSelectedCategoryIds();
                UpdateButtonStates();
                UpdateStatusBar();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        /// <summary>
        /// Xử lý sự kiện vẽ số thứ tự dòng cho GridView
        /// </summary>
        private void ProductServiceCategoryDtoGridView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        #endregion

        #region ========== XỬ LÝ DỮ LIỆU ==========

        /// <summary>
        /// Cấu hình GridView để hiển thị dữ liệu với format chuyên nghiệp.
        /// </summary>
        private void ConfigureMultiLineGridView()
        {
            try
            {
                // Cấu hình sắp xếp mặc định theo SortOrder, sau đó CategoryName
                if (ProductServiceCategoryDtoGridView.Columns["SortOrder"] != null)
                {
                    ProductServiceCategoryDtoGridView.Columns["SortOrder"].SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
                }
                else if (ProductServiceCategoryDtoGridView.Columns["CategoryName"] != null)
                {
                    ProductServiceCategoryDtoGridView.Columns["CategoryName"].SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        #endregion

        #region ========== TIỆN ÍCH ==========

        /// <summary>
        /// Thực hiện operation async với WaitingForm1 hiển thị.
        /// </summary>
        /// <param name="operation">Operation async cần thực hiện</param>
        private async Task ExecuteWithWaitingFormAsync(Func<Task> operation)
        {
            try
            {
                // Hiển thị WaitingForm1
                SplashScreenManager.ShowForm(typeof(WaitForm1));

                // Thực hiện operation
                await operation();
            }
            finally
            {
                // Đóng WaitingForm1
                SplashScreenManager.CloseForm();
            }
        }

        /// <summary>
        /// Cập nhật trạng thái các nút toolbar dựa trên selection
        /// </summary>
        private void UpdateButtonStates()
        {
            try
            {
                var selectedCount = _selectedCategoryIds?.Count ?? 0;
                // Edit: chỉ khi chọn đúng 1 dòng
                if (EditBarButtonItem != null)
                    EditBarButtonItem.Enabled = selectedCount == 1;
                // Delete: khi chọn >= 1 dòng
                if (DeleteBarButtonItem != null)
                    DeleteBarButtonItem.Enabled = selectedCount >= 1;
                // Export: chỉ khi có dữ liệu hiển thị
                var rowCount = ProductServiceCategoryDtoGridView.RowCount;
                if (ExportBarButtonItem != null)
                    ExportBarButtonItem.Enabled = rowCount > 0;
            }
            catch
            {
                // ignore
            }
        }

        /// <summary>
        /// Cập nhật status bar
        /// </summary>
        private void UpdateStatusBar()
        {
            try
            {
                UpdateSelectedRowStatus();
                UpdateDataSummaryStatus();
            }
            catch
            {
                // ignore
            }
        }

        /// <summary>
        /// Cập nhật thông tin số dòng đang được chọn với HTML formatting
        /// </summary>
        private void UpdateSelectedRowStatus()
        {
            try
            {
                if (CurrentSelectBarStaticItem == null) return;

                var selectedCount = _selectedCategoryIds?.Count ?? 0;
                if (selectedCount == 0)
                {
                    CurrentSelectBarStaticItem.Caption = @"<color=gray>Chưa chọn dòng nào</color>";
                }
                else if (selectedCount == 1)
                {
                    // Hiển thị thông tin chi tiết của dòng được chọn
                    var selectedId = _selectedCategoryIds[0];
                    var selectedDto = productServiceCategoryDtoBindingSource.Cast<ProductServiceCategoryDto>()
                        .FirstOrDefault(d => d.Id == selectedId);

                    if (selectedDto != null)
                    {
                        var statusColor = selectedDto.IsActive ? "#4CAF50" : "#F44336";
                        var statusText = selectedDto.IsActive ? "Hoạt động" : "Ngừng";
                        
                        var html = $"<b><color=blue>{selectedCount}</color></b> dòng: <b><color=blue>{selectedDto.CategoryName}</color></b>";
                        
                        if (!string.IsNullOrWhiteSpace(selectedDto.CategoryCode))
                        {
                            html += $" <color=#757575>({selectedDto.CategoryCode})</color>";
                        }
                        
                        html += $" | Trạng thái: <b><color={statusColor}>{statusText}</color></b>";
                        
                        if (selectedDto.ProductCount > 0)
                        {
                            html += $" | Sản phẩm/DV: <b><color=orange>{selectedDto.ProductCount:N0}</color></b>";
                        }
                        
                        CurrentSelectBarStaticItem.Caption = html;
                    }
                    else
                    {
                        CurrentSelectBarStaticItem.Caption = $@"<b><color=blue>Đang chọn {selectedCount} dòng</color></b>";
                    }
                }
                else
                {
                    CurrentSelectBarStaticItem.Caption = $@"<b><color=blue>Đang chọn {selectedCount} dòng</color></b>";
                }
            }
            catch
            {
                // ignore
            }
        }

        /// <summary>
        /// Cập nhật thông tin tổng kết dữ liệu với HTML formatting
        /// </summary>
        private void UpdateDataSummaryStatus()
        {
            try
            {
                if (DataSummaryBarStaticItem == null) return;

                var currentData = productServiceCategoryDtoBindingSource.DataSource as List<ProductServiceCategoryDto>;
                if (currentData == null || !currentData.Any())
                {
                    DataSummaryBarStaticItem.Caption = @"Chưa có dữ liệu";
                    return;
                }

                var totalCount = currentData.Count;
                var activeCount = currentData.Count(x => x.IsActive);
                var inactiveCount = currentData.Count(x => !x.IsActive);
                var totalProducts = currentData.Sum(x => x.ProductCount);

                // Tạo HTML content với màu sắc
                var summary = $"<b>Tổng số: {totalCount}</b> | " +
                             $"<color=green>Hoạt động: {activeCount}</color> | " +
                             $"<color=red>Không hoạt động: {inactiveCount}</color>";

                // Thêm thông tin về tổng số sản phẩm/dịch vụ nếu có
                if (totalProducts > 0)
                {
                    summary += $" | <color=orange>Sản phẩm/DV: {totalProducts:N0}</color>";
                }

                DataSummaryBarStaticItem.Caption = summary;
            }
            catch
            {
                // ignore
            }
        }

        /// <summary>
        /// Cập nhật danh sách selected category IDs.
        /// </summary>
        private void UpdateSelectedCategoryIds()
        {
            _selectedCategoryIds.Clear();

            _logger.Debug("=== UpdateSelectedCategoryIds ===");
            _logger.Debug("Total rows in GridView: {0}", ProductServiceCategoryDtoGridView.RowCount);

            // Lấy tất cả rows đã được chọn
            var selectedRows = ProductServiceCategoryDtoGridView.GetSelectedRows();
            foreach (int rowHandle in selectedRows)
            {
                if (rowHandle >= 0)
                {
                    var dto = ProductServiceCategoryDtoGridView.GetRow(rowHandle) as ProductServiceCategoryDto;
                    if (dto != null && !_selectedCategoryIds.Contains(dto.Id))
                    {
                        _selectedCategoryIds.Add(dto.Id);
                        _logger.Debug("    Added ID: {0} for {1}", dto.Id, dto.CategoryName);
                    }
                }
            }

            _logger.Debug("Final selected IDs: {0}", string.Join(", ", _selectedCategoryIds));
        }

        /// <summary>
        /// Xóa trạng thái chọn hiện tại trên GridView.
        /// </summary>
        private void ClearSelectionState()
        {
            _selectedCategoryIds.Clear();

            // Clear tất cả selection
            ProductServiceCategoryDtoGridView.ClearSelection();
            ProductServiceCategoryDtoGridView.FocusedRowHandle = DevExpress.XtraGrid.GridControl.InvalidRowHandle;

            UpdateButtonStates();
            UpdateStatusBar();
        }

        #endregion

        #region ========== TIỆN ÍCH HỖ TRỢ ==========

        /// <summary>
        /// Thiết lập SuperToolTip cho các controls trong Form
        /// </summary>
        private void SetupSuperToolTips()
        {
            try
            {
                if (ListDataBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        ListDataBarButtonItem,
                        title: "<b><color=Blue>🔄 Tải dữ liệu</color></b>",
                        content: "Tải lại danh sách danh mục sản phẩm/dịch vụ từ hệ thống."
                    );
                }

                if (NewBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        NewBarButtonItem,
                        title: "<b><color=Green>➕ Thêm mới</color></b>",
                        content: "Thêm mới danh mục sản phẩm/dịch vụ vào hệ thống."
                    );
                }

                if (EditBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        EditBarButtonItem,
                        title: "<b><color=Orange>✏️ Điều chỉnh</color></b>",
                        content: "Chỉnh sửa thông tin danh mục sản phẩm/dịch vụ đã chọn."
                    );
                }

                if (DeleteBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        DeleteBarButtonItem,
                        title: "<b><color=Red>🗑️ Xóa</color></b>",
                        content: "Xóa các danh mục sản phẩm/dịch vụ đã chọn. Sản phẩm/dịch vụ sẽ được chuyển sang 'Phân loại chưa đặt tên'."
                    );
                }

                if (ExportBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        ExportBarButtonItem,
                        title: "<b><color=Purple>📊 Xuất Excel</color></b>",
                        content: "Xuất danh sách danh mục sản phẩm/dịch vụ ra file Excel."
                    );
                }
            }
            catch (Exception ex)
            {
                // Ignore lỗi setup SuperToolTip để không chặn Form
                _logger.Warning("Lỗi setup SuperToolTip: {0}", ex.Message);
            }
        }

        /// <summary>
        /// Hiển thị thông tin.
        /// </summary>
        private void ShowInfo(string message)
        {
            MsgBox.ShowSuccess(message);
        }

        /// <summary>
        /// Hiển thị lỗi với thông tin ngữ cảnh.
        /// </summary>
        private void ShowError(Exception ex, string context = null)
        {
            MsgBox.ShowException(string.IsNullOrWhiteSpace(context)
                ? ex
                : new Exception(context + ": " + ex.Message, ex));
        }

        #endregion

    }
}
