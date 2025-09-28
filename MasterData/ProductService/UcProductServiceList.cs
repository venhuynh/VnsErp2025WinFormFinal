using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Bll.Common;
using Bll.MasterData.ProductService;
using Bll.Utils;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSplashScreen;
using MasterData.ProductService.Converters;
using MasterData.ProductService.Dto;

namespace MasterData.ProductService
{
    /// <summary>
    /// Danh sách Sản phẩm/Dịch vụ - chỉ giữ event handler, mọi xử lý tách thành phương thức riêng.
    /// </summary>
    public partial class UcProductServiceList : XtraUserControl
    {
        #region Fields

        private readonly ProductServiceBll _productServiceBll = new ProductServiceBll();
        private List<Guid> _selectedProductServiceIds = new List<Guid>();
        private bool _isLoading; // guard tránh gọi LoadDataAsync song song (Splash đã hiển thị)

        #endregion

        #region Constructor

        /// <summary>
        /// Khởi tạo control, đăng ký event UI.
        /// </summary>
        public UcProductServiceList()
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
            ProductServiceAdvBandedGridView.SelectionChanged += ProductServiceAdvBandedGridView_SelectionChanged;
            ProductServiceAdvBandedGridView.CustomDrawRowIndicator += ProductServiceAdvBandedGridView_CustomDrawRowIndicator;
            ProductServiceAdvBandedGridView.RowCellStyle += ProductServiceAdvBandedGridView_RowCellStyle;

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
        /// Người dùng bấm "Mới".
        /// </summary>
        private async void NewBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Tham khảo mẫu ConfigSqlServerInfoBarButtonItem_ItemClick: dùng ShowScope để auto-close overlay
            try
            {
                // TODO: Cần tạo FrmProductServiceDetail form
                ShowInfo("Chức năng thêm mới sản phẩm/dịch vụ chưa được triển khai. Vui lòng tạo form FrmProductServiceDetail trước.");
                
                // using (OverlayManager.ShowScope(this))
                // {
                //     using (var form = new FrmProductServiceDetail(Guid.Empty))
                //     {
                //         form.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
                //         form.ShowDialog(this);
                //
                //         await LoadDataAsync();
                //         UpdateButtonStates();
                //     }
                // }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi hiển thị màn hình thêm mới");
            }
        }

        /// <summary>
        /// Grid selection thay đổi -> cập nhật danh sách Id đã chọn và trạng thái nút.
        /// </summary>
        private void ProductServiceAdvBandedGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                _selectedProductServiceIds = GridViewHelper.GetSelectedRowColumnValues<Guid>(sender, nameof(ProductServiceDto.Id));
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
        private void ProductServiceAdvBandedGridView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs rowIndicatorCustomDrawEventArgs)
        {
            // Sử dụng helper chung để vẽ số thứ tự dòng
            GridViewHelper.CustomDrawRowIndicator(ProductServiceAdvBandedGridView, rowIndicatorCustomDrawEventArgs);
        }

        private void UpdateButtonStates()
        {
            try
            {
                var selectedCount = _selectedProductServiceIds?.Count ?? 0;
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
                var rowCount = GridViewHelper.GetDisplayRowCount(ProductServiceAdvBandedGridView) ?? 0;
                if (ExportBarButtonItem != null)
                    ExportBarButtonItem.Enabled = rowCount > 0;
            }
            catch
            {
                // ignore
            }
        }

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

                var selectedCount = _selectedProductServiceIds?.Count ?? 0;
                if (selectedCount == 0)
                {
                    SelectedRowBarStaticItem.Caption = "Chưa chọn dòng nào";
                }
                else if (selectedCount == 1)
                {
                    SelectedRowBarStaticItem.Caption = "Đang chọn 1 dòng";
                }
                else
                {
                    SelectedRowBarStaticItem.Caption = $"Đang chọn {selectedCount} dòng";
                }
            }
            catch
            {
                // ignore
            }
        }

        /// <summary>
        /// Cập nhật thông tin tổng kết dữ liệu với HTML formatting.
        /// </summary>
        private void UpdateDataSummaryStatus()
        {
            try
            {
                if (DataSummaryBarStaticItem == null) return;

                var currentData = productServiceDtoBindingSource.DataSource as List<ProductServiceDto>;
                if (currentData == null || !currentData.Any())
                {
                    DataSummaryBarStaticItem.Caption = "Chưa có dữ liệu";
                    return;
                }

                var totalCount = currentData.Count;
                var productCount = currentData.Count(x => !x.IsService);
                var serviceCount = currentData.Count(x => x.IsService);
                var activeCount = currentData.Count(x => x.IsActive);
                var inactiveCount = currentData.Count(x => !x.IsActive);

                // Tạo HTML content với màu sắc
                var summary = $"<b>Tổng: {totalCount}</b> | " +
                             $"<color=blue>Sản phẩm: {productCount}</color> | " +
                             $"<color=green>Dịch vụ: {serviceCount}</color> | " +
                             $"<color=green>Hoạt động: {activeCount}</color> | " +
                             $"<color=red>Không hoạt động: {inactiveCount}</color>";

                DataSummaryBarStaticItem.Caption = summary;
            }
            catch
            {
                // ignore
            }
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
                ProductServiceAdvBandedGridView.OptionsView.RowAutoHeight = true;

                // RepositoryItemMemoEdit cho wrap text
                var memo = new RepositoryItemMemoEdit
                {
                    WordWrap = true,
                    AutoHeight = false
                };
                memo.Appearance.TextOptions.WordWrap = WordWrap.Wrap;

                // Áp dụng cho các cột có khả năng dài
                ApplyMemoEditorToColumn("Name", memo);
                ApplyMemoEditorToColumn("Description", memo);
                ApplyMemoEditorToColumn("CategoryName", memo);

                // Cấu hình cột ảnh thumbnail
                ConfigureThumbnailColumn();

                // Tùy chọn hiển thị: căn giữa tiêu đề cho đẹp
                ProductServiceAdvBandedGridView.Appearance.HeaderPanel.TextOptions.HAlignment = HorzAlignment.Center;
                ProductServiceAdvBandedGridView.Appearance.HeaderPanel.Options.UseTextOptions = true;
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
                if (!ProductServiceListGridControl.RepositoryItems.Contains(pictureEdit))
                {
                    ProductServiceListGridControl.RepositoryItems.Add(pictureEdit);
                }

                // Áp dụng cho cột thumbnail
                colThumbnail.ColumnEdit = pictureEdit;
                colThumbnail.OptionsColumn.FixedWidth = true;
                colThumbnail.Width = 80;
                colThumbnail.Caption = "Ảnh đại diện";
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void ApplyMemoEditorToColumn(string fieldName, RepositoryItemMemoEdit memo)
        {
            var col = ProductServiceAdvBandedGridView.Columns[fieldName];
            if (col == null) return;
            // Thêm repository vào GridControl nếu chưa có
            if (!ProductServiceListGridControl.RepositoryItems.Contains(memo))
            {
                ProductServiceListGridControl.RepositoryItems.Add(memo);
            }
            col.ColumnEdit = memo;
        }

        /// <summary>
        /// Tô màu/định dạng dòng theo trạng thái/loại sản phẩm/dịch vụ.
        /// </summary>
        private void ProductServiceAdvBandedGridView_RowCellStyle(object sender, RowCellStyleEventArgs rowCellStyleEventArgs)
        {
            try
            {
                var view = sender as AdvBandedGridView;
                if (view == null) return;
                if (rowCellStyleEventArgs.RowHandle < 0) return;
                var row = view.GetRow(rowCellStyleEventArgs.RowHandle) as ProductServiceDto;
                if (row == null) return;
                // Không ghi đè màu khi đang chọn để giữ màu chọn mặc định của DevExpress
                if (view.IsRowSelected(rowCellStyleEventArgs.RowHandle)) return;
                // Nền theo loại (IsService) với màu tương phản rõ
                Color backColor;
                if (row.IsService)
                {
                    backColor = Color.LightCyan; // xanh nhạt cho dịch vụ
                }
                else
                {
                    backColor = Color.LightYellow; // vàng nhạt cho sản phẩm
                }

                //rowCellStyleEventArgs.Appearance.BackColor = backColor;
                rowCellStyleEventArgs.Appearance.ForeColor = Color.Black; // chữ đen tương phản tốt trên các màu nền trên
                rowCellStyleEventArgs.Appearance.Options.UseBackColor = true;
                rowCellStyleEventArgs.Appearance.Options.UseForeColor = true;

                // Nếu sản phẩm/dịch vụ không hoạt động: làm nổi bật rõ ràng hơn
                if (!row.IsActive)
                {
                    //rowCellStyleEventArgs.Appearance.BackColor = System.Drawing.Color.FromArgb(255, 205, 210); // đỏ nhạt nhưng đậm hơn (Light Red)
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

        #region CRUD Event Handlers

        /// <summary>
        /// Người dùng bấm "Điều chỉnh".
        /// </summary>
        private async void EditBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Chỉ cho phép chỉnh sửa 1 dòng dữ liệu
            if (_selectedProductServiceIds == null || _selectedProductServiceIds.Count == 0)
            {
                ShowInfo("Vui lòng chọn một dòng để chỉnh sửa.");
                return;
            }
            if (_selectedProductServiceIds.Count > 1)
            {
                ShowInfo("Chỉ cho phép chỉnh sửa 1 dòng. Vui lòng bỏ chọn bớt.");
                return;
            }

            var id = _selectedProductServiceIds[0];
            var dto = ProductServiceAdvBandedGridView.GetFocusedRow() as ProductServiceDto;
            if (dto == null || dto.Id != id)
            {
                // Tìm đúng DTO theo Id trong datasource nếu FocusedRow không khớp selection
                if (productServiceDtoBindingSource.DataSource is IEnumerable list)
                {
                    foreach (var item in list)
                    {
                        if (item is ProductServiceDto x && x.Id == id)
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

            // Tham khảo mẫu ConfigSqlServerInfoBarButtonItem_ItemClick: dùng ShowScope để auto-close overlay
            try
            {
                // TODO: Cần tạo FrmProductServiceDetail form
                ShowInfo("Chức năng chỉnh sửa sản phẩm/dịch vụ chưa được triển khai. Vui lòng tạo form FrmProductServiceDetail trước.");
                
                // using (OverlayManager.ShowScope(this))
                // {
                //     using (var form = new FrmProductServiceDetail(dto.Id))
                //     {
                //         form.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
                //         form.ShowDialog(this);
                //         
                //         await LoadDataAsync();
                //         UpdateButtonStates();
                //     }
                // }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi hiển thị màn hình điều chỉnh");
            }
        }

        /// <summary>
        /// Người dùng bấm "Xóa".
        /// </summary>
        private async void DeleteBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (_selectedProductServiceIds == null || _selectedProductServiceIds.Count == 0)
            {
                ShowInfo("Vui lòng chọn ít nhất một dòng để xóa.");
                return;
            }

            var confirmMessage = _selectedProductServiceIds.Count == 1
                ? "Bạn có chắc muốn xóa dòng dữ liệu đã chọn?"
                : $"Bạn có chắc muốn xóa {_selectedProductServiceIds.Count} dòng dữ liệu đã chọn?";

            if (!MsgBox.GetConfirmFromYesNoDialog(confirmMessage)) return;

            try
            {
                await ExecuteWithWaitingFormAsync(async () =>
                {
                    foreach (var id in _selectedProductServiceIds.ToList())
                    {
                        _productServiceBll.Delete(id);
                    }
                    ClearSelectionState();
                    // Gọi LoadDataAsyncWithoutSplash để tránh xung đột WaitingForm1
                    await LoadDataAsyncWithoutSplash();
                });
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi xóa dữ liệu");
            }
        }

        /// <summary>
        /// Người dùng bấm "Xuất".
        /// </summary>
        private void ExportBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Chỉ cho phép xuất khi có dữ liệu hiển thị
            var rowCount = GridViewHelper.GetDisplayRowCount(ProductServiceAdvBandedGridView) ?? 0;
            if (rowCount <= 0)
            {
                ShowInfo("Không có dữ liệu để xuất.");
                return;
            }

            GridViewHelper.ExportGridControl(ProductServiceAdvBandedGridView, "ProductServices.xlsx");
        }

        /// <summary>
        /// Người dùng bấm "Đếm số lượng" - đếm VariantCount và ImageCount cho các sản phẩm/dịch vụ được chọn.
        /// </summary>
        private void CountVariantAndImageBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                // Kiểm tra có dòng nào được chọn không
                if (_selectedProductServiceIds == null || !_selectedProductServiceIds.Any())
                {
                    ShowInfo("Vui lòng chọn ít nhất một dòng dữ liệu để đếm số lượng.");
                    return;
                }

                // Hiển thị thông báo cho user biết đang xử lý
                var selectedCount = _selectedProductServiceIds.Count;
                
                // Thực hiện đếm với splash screen
                _ = ExecuteWithWaitingFormAsync(async () =>
                {
                    await CountSelectedProductsAsync();
                });
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khi đếm số lượng biến thể và hình ảnh");
            }
        }

        #endregion

        #region Data Loading Methods

        /// <summary>
        /// Tải dữ liệu và bind vào Grid (Async, hiển thị WaitForm).
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
        /// </summary>
        private async Task LoadDataAsyncWithoutSplash()
        {
            try
            {
                var entities = await _productServiceBll.GetAllAsync();
                // Không đếm VariantCount và ImageCount khi load để tăng tốc độ
                var dtoList = entities.ToDtoList(
                    categoryId => _productServiceBll.GetCategoryName(categoryId)
                ).ToList();
                BindGrid(dtoList);
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi tải dữ liệu");
            }
        }

        /// <summary>
        /// Tải dữ liệu với đếm số lượng biến thể và hình ảnh (chậm hơn, dùng khi cần thiết).
        /// </summary>
        private async Task LoadDataWithCountingAsync()
        {
            try
            {
                var entities = await _productServiceBll.GetAllAsync();
                // Đếm VariantCount và ImageCount - chậm hơn nhưng đầy đủ thông tin
                var dtoList = entities.ToDtoList(
                    categoryId => _productServiceBll.GetCategoryName(categoryId),
                    productId => _productServiceBll.GetVariantCount(productId),
                    productId => _productServiceBll.GetImageCount(productId)
                ).ToList();
                BindGrid(dtoList);
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi tải dữ liệu với đếm số lượng");
            }
        }

        /// <summary>
        /// Đếm số lượng biến thể và hình ảnh cho các sản phẩm/dịch vụ được chọn.
        /// </summary>
        private async Task CountSelectedProductsAsync()
        {
            try
            {
                if (_selectedProductServiceIds == null || !_selectedProductServiceIds.Any())
                    return;

                // Lấy dữ liệu hiện tại từ grid
                var currentData = productServiceDtoBindingSource.DataSource as List<ProductServiceDto>;
                if (currentData == null)
                    return;

                // Đếm số lượng cho các sản phẩm được chọn
                var counts = await Task.Run(() => _productServiceBll.GetCountsForProducts(_selectedProductServiceIds.ToList()));

                // Cập nhật dữ liệu với số lượng đã đếm
                foreach (var dto in currentData)
                {
                    if (counts.ContainsKey(dto.Id))
                    {
                        dto.VariantCount = counts[dto.Id].VariantCount;
                        dto.ImageCount = counts[dto.Id].ImageCount;
                    }
                }

                
                
                // Refresh grid để hiển thị số lượng mới
                productServiceDtoBindingSource.ResetBindings(false);

                //Hiển thị thông báo đã đếm xong số lượng cho các sản phẩm được chọn
                MsgBox.ShowInfo($"Đã đếm xong số lượng biến thể và hình ảnh cho {_selectedProductServiceIds.Count} sản phẩm/dịch vụ được chọn.");

                // Cập nhật status bar với thông tin mới
                UpdateStatusBar();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khi đếm số lượng cho các sản phẩm được chọn");
            }
        }

        /// <summary>
        /// Bind danh sách DTO vào Grid và cấu hình hiển thị.
        /// </summary>
        private void BindGrid(List<ProductServiceDto> data)
        {
            productServiceDtoBindingSource.DataSource = data;
            ProductServiceAdvBandedGridView.BestFitColumns();
            ConfigureMultiLineGridView();
            UpdateButtonStates();
            UpdateStatusBar();
        }

        /// <summary>
        /// Xóa trạng thái chọn hiện tại trên Grid.
        /// </summary>
        private void ClearSelectionState()
        {
            _selectedProductServiceIds.Clear();
            ProductServiceAdvBandedGridView.ClearSelection();
            ProductServiceAdvBandedGridView.FocusedRowHandle = GridControl.InvalidRowHandle;
            UpdateButtonStates();
            UpdateStatusBar();
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
