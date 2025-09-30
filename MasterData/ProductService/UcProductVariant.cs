using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bll.Common;
using Bll.MasterData.ProductServiceBll;
using Bll.Utils;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSplashScreen;
using MasterData.ProductService.Dto;

namespace MasterData.ProductService
{
    /// <summary>
    /// User Control cho quản lý ProductVariant với AdvBandedGridView
    /// Hiển thị danh sách biến thể sản phẩm với thông tin đầy đủ
    /// </summary>
    public partial class UcProductVariant : XtraUserControl
    {
        #region Fields

        private readonly ProductServiceBll _productServiceBll = new ProductServiceBll();
        private List<Guid> _selectedProductVariantIds = new List<Guid>();
        private bool _isLoading; // guard tránh gọi LoadDataAsync song song (Splash đã hiển thị)
        
        // Pagination fields
        private int _currentPageIndex;
        private int _pageSize = 50;
        private int _totalCount;
        private int _totalPages;

        #endregion

        #region Constructor

        /// <summary>
        /// Khởi tạo control, đăng ký event UI.
        /// </summary>
        public UcProductVariant()
        {
            InitializeComponent();

            // Toolbar events
            ListDataBarButtonItem.ItemClick += ListDataBarButtonItem_ItemClick;
            NewBarButtonItem.ItemClick += NewBarButtonItem_ItemClick;
            EditBarButtonItem.ItemClick += EditBarButtonItem_ItemClick;
            DeleteBarButtonItem.ItemClick += DeleteBarButtonItem_ItemClick;
            ExportBarButtonItem.ItemClick += ExportBarButtonItem_ItemClick;
            CountVariantAndImageBarButtonItem.ItemClick += CountVariantAndImageBarButtonItem_ItemClick;

            // Grid events
            ProductVariantAdvBandedGridView.SelectionChanged += ProductVariantAdvBandedGridView_SelectionChanged;
            ProductVariantAdvBandedGridView.CustomDrawRowIndicator += ProductVariantAdvBandedGridView_CustomDrawRowIndicator;
            ProductVariantAdvBandedGridView.RowCellStyle += ProductVariantAdvBandedGridView_RowCellStyle;

            // Pagination events
            PageBarEditItem.EditValueChanged += PageBarEditItem_EditValueChanged;
            RecordNumberBarEditItem.EditValueChanged += RecordNumberBarEditItem_EditValueChanged;

            // Filter events
            DataFilterBtn.ItemClick += DataFilterBtn_ItemClick;

            // Initialize pagination control
            InitializePaginationControl();
            InitializeRecordNumberControl();

            UpdateButtonStates();
        }

        #endregion

        #region Private Helper Methods

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

        #endregion

        #region Event Handlers

        /// <summary>
        /// Người dùng bấm "Danh sách" để tải dữ liệu.
        /// </summary>
        private async void ListDataBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            await LoadDataAsync();
        }

        /// <summary>
        /// Người dùng bấm "Mới" để tạo ProductVariant mới.
        /// </summary>
        private void NewBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                using (OverlayManager.ShowScope(this))
                {
                    using (var form = new FrmProductVariantDetail(Guid.Empty))
                    {
                        form.StartPosition = FormStartPosition.CenterParent;
                        form.ShowDialog(this);

                        _ = LoadDataAsyncWithoutSplash();
                        UpdateButtonStates();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi hiển thị màn hình thêm mới biến thể sản phẩm");
            }
        }

        /// <summary>
        /// Grid selection thay đổi -> cập nhật danh sách Id đã chọn và trạng thái nút.
        /// </summary>
        private void ProductVariantAdvBandedGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                _selectedProductVariantIds = GridViewHelper.GetSelectedRowColumnValues<Guid>(sender, nameof(ProductVariantDetailDto.Id));
                UpdateButtonStates();
                UpdateStatusBar();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        /// <summary>
        /// Vẽ STT dòng.
        /// </summary>
        private void ProductVariantAdvBandedGridView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs rowIndicatorCustomDrawEventArgs)
        {
            // Sử dụng helper chung để vẽ số thứ tự dòng
            GridViewHelper.CustomDrawRowIndicator(ProductVariantAdvBandedGridView, rowIndicatorCustomDrawEventArgs);
        }

        /// <summary>
        /// Tô màu/định dạng dòng theo trạng thái biến thể.
        /// </summary>
        private void ProductVariantAdvBandedGridView_RowCellStyle(object sender, RowCellStyleEventArgs rowCellStyleEventArgs)
        {
            try
            {
                var view = sender as AdvBandedGridView;
                if (view == null) return;
                if (rowCellStyleEventArgs.RowHandle < 0) return;
                var row = view.GetRow(rowCellStyleEventArgs.RowHandle) as ProductVariantDetailDto;
                if (row == null) return;
                
                // Không ghi đè màu khi đang chọn để giữ màu chọn mặc định của DevExpress
                if (view.IsRowSelected(rowCellStyleEventArgs.RowHandle)) return;
                
                // Nền theo trạng thái hoạt động
                Color backColor;
                if (row.IsActive)
                {
                    backColor = Color.LightGreen; // xanh nhạt cho biến thể hoạt động
                }
                else
                {
                    backColor = Color.LightPink; // hồng nhạt cho biến thể không hoạt động
                }

                rowCellStyleEventArgs.Appearance.BackColor = backColor;
                rowCellStyleEventArgs.Appearance.ForeColor = Color.Black; // chữ đen tương phản tốt
                rowCellStyleEventArgs.Appearance.Options.UseBackColor = true;
                rowCellStyleEventArgs.Appearance.Options.UseForeColor = true;

                // Nếu biến thể không hoạt động: làm nổi bật rõ ràng hơn
                if (!row.IsActive)
                {
                    rowCellStyleEventArgs.Appearance.ForeColor = Color.DarkRed;
                    rowCellStyleEventArgs.Appearance.Font = new Font(rowCellStyleEventArgs.Appearance.Font, FontStyle.Strikeout);
                }
            }
            catch (Exception)
            {
                // ignore style errors
            }
        }

        #endregion

        #region Button State Management

        private void UpdateButtonStates()
        {
            try
            {
                var selectedCount = _selectedProductVariantIds?.Count ?? 0;
                // Edit: chỉ khi chọn đúng 1 dòng
                if (EditBarButtonItem != null)
                    EditBarButtonItem.Enabled = selectedCount == 1;
                // Delete: khi chọn >= 1 dòng
                if (DeleteBarButtonItem != null)
                    DeleteBarButtonItem.Enabled = selectedCount >= 1;
                // Count: chỉ khi chọn >= 1 dòng (giống như Delete)
                if (CountVariantAndImageBarButtonItem != null)
                    CountVariantAndImageBarButtonItem.Enabled = selectedCount >= 1;
                // Export: chỉ khi có dữ liệu hiển thị
                var rowCount = GridViewHelper.GetDisplayRowCount(ProductVariantAdvBandedGridView) ?? 0;
                if (ExportBarButtonItem != null)
                    ExportBarButtonItem.Enabled = rowCount > 0;
            }
            catch
            {
                // ignore
            }
        }

        #endregion

        #region Status Bar Management

        /// <summary>
        /// Cập nhật status bar với thông tin selection và data summary.
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
        /// Cập nhật thông tin số dòng đang được chọn.
        /// </summary>
        private void UpdateSelectedRowStatus()
        {
            try
            {
                if (SelectedRowBarStaticItem == null) return;

                var selectedCount = _selectedProductVariantIds?.Count ?? 0;
                if (selectedCount == 0)
                {
                    SelectedRowBarStaticItem.Caption = @"Chưa chọn dòng nào";
                }
                else if (selectedCount == 1)
                {
                    SelectedRowBarStaticItem.Caption = @"Đang chọn 1 dòng";
                }
                else
                {
                    SelectedRowBarStaticItem.Caption = $@"Đang chọn {selectedCount} dòng";
                }
            }
            catch
            {
                // ignore
            }
        }

        /// <summary>
        /// Cập nhật thông tin tổng kết dữ liệu với HTML formatting và pagination info.
        /// </summary>
        private void UpdateDataSummaryStatus()
        {
            try
            {
                if (DataSummaryBarStaticItem == null) return;

                var currentData = productVariantDtoBindingSource.DataSource as List<ProductVariantDetailDto>;
                if (currentData == null || !currentData.Any())
                {
                    DataSummaryBarStaticItem.Caption = @"Chưa có dữ liệu";
                    return;
                }

                var currentPageCount = currentData.Count;
                var productCount = currentData.Count(x => !x.IsService);
                var serviceCount = currentData.Count(x => x.IsService);
                var activeCount = currentData.Count(x => x.IsActive);
                var inactiveCount = currentData.Count(x => !x.IsActive);

                // Tạo HTML content với màu sắc và pagination info
                string summary;
                if (_pageSize == int.MaxValue)
                {
                    // Trường hợp "Tất cả" - không phân trang
                    summary = "<b>Tất cả dữ liệu</b> | " +
                             $"<b>Hiển thị: {currentPageCount}/{_totalCount}</b> | " +
                             $"<color=blue>Biến thể sản phẩm: {productCount}</color> | " +
                             $"<color=green>Biến thể dịch vụ: {serviceCount}</color> | " +
                             $"<color=green>Hoạt động: {activeCount}</color> | " +
                             $"<color=red>Không hoạt động: {inactiveCount}</color>";
                }
                else
                {
                    // Trường hợp có phân trang
                    summary = $"<b>Trang {_currentPageIndex + 1}/{_totalPages}</b> | " +
                             $"<b>Hiển thị: {currentPageCount}/{_totalCount}</b> | " +
                             $"<color=blue>Biến thể sản phẩm: {productCount}</color> | " +
                             $"<color=green>Biến thể dịch vụ: {serviceCount}</color> | " +
                             $"<color=green>Hoạt động: {activeCount}</color> | " +
                             $"<color=red>Không hoạt động: {inactiveCount}</color>";
                }

                DataSummaryBarStaticItem.Caption = summary;
            }
            catch
            {
                // ignore
            }
        }

        #endregion

        #region Data Loading Methods (Placeholder)

        /// <summary>
        /// Tải dữ liệu và bind vào Grid (Async, hiển thị WaitForm).
        /// TODO: Triển khai sau khi có BLL cho ProductVariant
        /// </summary>
        private async Task LoadDataAsync()
        {
            if (_isLoading) return; // tránh re-entrancy
            _isLoading = true;
            try
            {
                await ExecuteWithWaitingFormAsync(async () =>
                {
                    await LoadDataAsyncWithoutSplash();
                });
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
        /// Tải dữ liệu và bind vào Grid (Async, không hiển thị WaitForm).
        /// TODO: Triển khai sau khi có BLL cho ProductVariant
        /// </summary>
        private async Task LoadDataAsyncWithoutSplash()
        {
            try
            {
                // TODO: Implement actual data loading
                // For now, just show empty list
                var emptyList = new List<ProductVariantDetailDto>();
                BindGrid(emptyList);
                
                // Reset pagination
                _currentPageIndex = 0;
                _totalCount = 0;
                _totalPages = 1;
                UpdatePaginationControl();
                UpdateButtonStates();
                UpdateStatusBar();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi tải dữ liệu");
            }
        }

        /// <summary>
        /// Bind danh sách DTO vào Grid và cấu hình hiển thị.
        /// </summary>
        private void BindGrid(List<ProductVariantDetailDto> data)
        {
            productVariantDtoBindingSource.DataSource = data;
            ProductVariantAdvBandedGridView.BestFitColumns();
            ConfigureMultiLineGridView();
            UpdateButtonStates();
            UpdateStatusBar();
        }

        #endregion

        #region Grid Configuration Methods

        /// <summary>
        /// Cấu hình AdvBandedGridView để hiển thị dữ liệu xuống dòng (word wrap) cho các cột văn bản dài.
        /// Đồng thời bật tự động tính chiều cao dòng để hiển thị đầy đủ nội dung.
        /// </summary>
        private void ConfigureMultiLineGridView()
        {
            try
            {
                // Bật tự động điều chỉnh chiều cao dòng để wrap nội dung
                ProductVariantAdvBandedGridView.OptionsView.RowAutoHeight = true;

                // RepositoryItemMemoEdit cho wrap text
                var memo = new RepositoryItemMemoEdit
                {
                    WordWrap = true,
                    AutoHeight = false
                };
                memo.Appearance.TextOptions.WordWrap = WordWrap.Wrap;

                // Cấu hình cột ảnh thumbnail
                ConfigureThumbnailColumn();

                // Cấu hình filter và search
                ConfigureFilterAndSearch();

                // Tùy chọn hiển thị: căn giữa tiêu đề cho đẹp
                ProductVariantAdvBandedGridView.Appearance.HeaderPanel.TextOptions.HAlignment = HorzAlignment.Center;
                ProductVariantAdvBandedGridView.Appearance.HeaderPanel.Options.UseTextOptions = true;
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// Cấu hình cột ảnh thumbnail
        /// </summary>
        private void ConfigureThumbnailColumn()
        {
            try
            {
                // RepositoryItemPictureEdit cho hiển thị ảnh
                var pictureEdit = new RepositoryItemPictureEdit
                {
                    SizeMode = PictureSizeMode.Stretch
                };

                // Thêm repository vào GridControl
                if (!ProductVariantGridControl.RepositoryItems.Contains(pictureEdit))
                {
                    ProductVariantGridControl.RepositoryItems.Add(pictureEdit);
                }

                // Áp dụng cho cột thumbnail
                colThumbnailImage.ColumnEdit = pictureEdit;
                colThumbnailImage.OptionsColumn.FixedWidth = true;
                colThumbnailImage.Width = 80;
                colThumbnailImage.Caption = @"Ảnh đại diện";
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// Cấu hình filter và search cho grid
        /// </summary>
        private void ConfigureFilterAndSearch()
        {
            try
            {
                // Bật filter row
                ProductVariantAdvBandedGridView.OptionsView.ShowAutoFilterRow = true;
                
                // Bật search
                ProductVariantAdvBandedGridView.OptionsFind.AlwaysVisible = true;
                ProductVariantAdvBandedGridView.OptionsFind.FindMode = FindMode.Always;
                ProductVariantAdvBandedGridView.OptionsFind.FindNullPrompt = "Tìm kiếm trong tất cả dữ liệu...";
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        #endregion

        #region CRUD Event Handlers (Placeholder)

        /// <summary>
        /// Người dùng bấm "Điều chỉnh".
        /// TODO: Triển khai sau
        /// </summary>
        private void EditBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            ShowInfo("Chức năng chỉnh sửa sẽ được triển khai sau");
        }

        /// <summary>
        /// Người dùng bấm "Xóa".
        /// TODO: Triển khai sau
        /// </summary>
        private void DeleteBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            ShowInfo("Chức năng xóa sẽ được triển khai sau");
        }

        /// <summary>
        /// Người dùng bấm "Xuất".
        /// TODO: Triển khai sau
        /// </summary>
        private void ExportBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            ShowInfo("Chức năng xuất sẽ được triển khai sau");
        }

        /// <summary>
        /// Người dùng bấm "Thống kê".
        /// TODO: Triển khai sau
        /// </summary>
        private void CountVariantAndImageBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            ShowInfo("Chức năng thống kê sẽ được triển khai sau");
        }

        /// <summary>
        /// Người dùng bấm "Tìm kiếm".
        /// TODO: Triển khai sau
        /// </summary>
        private void DataFilterBtn_ItemClick(object sender, ItemClickEventArgs e)
        {
            ShowInfo("Chức năng tìm kiếm sẽ được triển khai sau");
        }

        #endregion

        #region Pagination Methods (Placeholder)

        /// <summary>
        /// Khởi tạo pagination control
        /// </summary>
        private void InitializePaginationControl()
        {
            try
            {
                // Cấu hình ComboBox cho pagination
                repositoryItemSpinEdit1.Buttons.Clear();
                repositoryItemSpinEdit1.Buttons.Add(new EditorButton(ButtonPredefines.Combo));
                
                // Set initial value
                PageBarEditItem.EditValue = 1;
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khởi tạo pagination control");
            }
        }

        /// <summary>
        /// Khởi tạo record number control
        /// </summary>
        private void InitializeRecordNumberControl()
        {
            try
            {
                // Cấu hình ComboBox cho record number
                repositoryItemComboBox1.Items.Clear();
                repositoryItemComboBox1.TextEditStyle = TextEditStyles.DisableTextEditor;
                
                // Thêm các options: "20", "50", "100", "Tất cả"
                repositoryItemComboBox1.Items.AddRange(new object[] { "20", "50", "100", "Tất cả" });
                
                // Set initial value
                RecordNumberBarEditItem.EditValue = "50";
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khởi tạo record number control");
            }
        }

        /// <summary>
        /// Cập nhật pagination control với danh sách trang
        /// </summary>
        private void UpdatePaginationControl()
        {
            try
            {
                if (PageBarEditItem?.Edit == null) return;

                // Set current page
                var currentPageNumber = _currentPageIndex + 1;
                PageBarEditItem.EditValue = currentPageNumber;

                // Enable/disable based on total pages
                PageBarEditItem.Enabled = _totalPages > 1;
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi cập nhật pagination control");
            }
        }

        /// <summary>
        /// Event handler khi user thay đổi trang trong pagination control
        /// TODO: Triển khai sau
        /// </summary>
        private void PageBarEditItem_EditValueChanged(object sender, EventArgs e)
        {
            ShowInfo("Chức năng phân trang sẽ được triển khai sau");
        }

        /// <summary>
        /// Event handler khi user thay đổi số dòng trên mỗi trang
        /// TODO: Triển khai sau
        /// </summary>
        private void RecordNumberBarEditItem_EditValueChanged(object sender, EventArgs e)
        {
            ShowInfo("Chức năng thay đổi số dòng sẽ được triển khai sau");
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// Hiển thị thông tin.
        /// </summary>
        private void ShowInfo(string message)
        {
            MsgBox.ShowInfo(message);
        }

        /// <summary>
        /// Hiển thị lỗi với thông tin ngữ cảnh.
        /// </summary>
        private void ShowError(Exception ex, string context = null)
        {
            if (string.IsNullOrWhiteSpace(context))
                MsgBox.ShowException(ex);
            else
                MsgBox.ShowException(new Exception(context + ": " + ex.Message, ex));
        }

        #endregion
    }
}
