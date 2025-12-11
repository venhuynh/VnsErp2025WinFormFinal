using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bll.Common;
using Bll.MasterData.ProductServiceBll;
using Common.Common;
using Common.Helpers;
using Common.Utils;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSplashScreen;
using DTO.MasterData.ProductService;

namespace MasterData.ProductService
{
    /// <summary>
    /// UserControl quản lý danh sách sản phẩm/dịch vụ.
    /// Cung cấp chức năng CRUD đầy đủ với GridView, pagination, tìm kiếm toàn diện và giao diện thân thiện.
    /// </summary>
    public partial class FrmProductServiceList : XtraForm
    {
        #region ========== KHAI BÁO BIẾN ==========

        /// <summary>
        /// Business Logic Layer cho sản phẩm/dịch vụ
        /// </summary>
        private readonly ProductServiceBll _productServiceBll = new ProductServiceBll();

        /// <summary>
        /// Danh sách ID của các sản phẩm/dịch vụ được chọn
        /// </summary>
        private List<Guid> _selectedProductServiceIds = new List<Guid>();

        /// <summary>
        /// Guard tránh gọi LoadDataAsync song song
        /// </summary>
        private bool _isLoading;

        /// <summary>
        /// RowHandle đang được edit (để lấy ProductServiceId khi upload thumbnail)
        /// </summary>
        private int _editingRowHandle = GridControl.InvalidRowHandle;


        #endregion

        #region ========== CONSTRUCTOR & PUBLIC METHODS ==========

        /// <summary>
        /// Khởi tạo control, đăng ký event UI
        /// </summary>
        public FrmProductServiceList()
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
            ProductServiceGridView.SelectionChanged += ProductServiceGridView_SelectionChanged;
            ProductServiceGridView.CustomDrawRowIndicator +=
                ProductServiceGridView_CustomDrawRowIndicator;
            ProductServiceGridView.RowCellStyle += ProductServiceGridView_RowCellStyle;
            ProductServiceGridView.ShownEditor += ProductServiceGridView_ShownEditor;
            ProductServiceGridView.HiddenEditor += ProductServiceGridView_HiddenEditor;

            UpdateButtonStates();
        }

        /// <summary>
        /// Thực hiện operation async với WaitingForm1 hiển thị
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

        #region ========== SỰ KIỆN TOOLBAR ==========

        /// <summary>
        /// Người dùng bấm "Danh sách" để tải dữ liệu
        /// </summary>
        private async void ListDataBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            await LoadDataAsync();
        }

        /// <summary>
        /// Người dùng bấm "Mới"
        /// </summary>
        private async void NewBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Tham khảo mẫu ConfigSqlServerInfoBarButtonItem_ItemClick: dùng ShowScope để auto-close overlay
            try
            {
                using (OverlayManager.ShowScope(this))
                {
                    using (var form = new FrmProductServiceDetail(Guid.Empty))
                    {
                        form.StartPosition = FormStartPosition.CenterParent;
                        form.ShowDialog(this);

                        await LoadDataAsyncWithoutSplash();
                        UpdateButtonStates();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi hiển thị màn hình thêm mới");
            }
        }

        #endregion

        #region ========== SỰ KIỆN GRID ==========

        /// <summary>
        /// Grid selection thay đổi -> cập nhật danh sách Id đã chọn và trạng thái nút
        /// </summary>
        private void ProductServiceGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                _selectedProductServiceIds =
                    GridViewHelper.GetSelectedRowColumnValues<Guid>(sender, nameof(ProductServiceDto.Id));
                UpdateButtonStates();
                UpdateStatusBar();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        /// <summary>
        /// Vẽ STT dòng cho GridView
        /// </summary>
        private void ProductServiceGridView_CustomDrawRowIndicator(object sender,
            RowIndicatorCustomDrawEventArgs rowIndicatorCustomDrawEventArgs)
        {
            // Sử dụng helper chung để vẽ số thứ tự dòng
            GridViewHelper.CustomDrawRowIndicator(ProductServiceGridView, rowIndicatorCustomDrawEventArgs);
        }

        /// <summary>
        /// Cập nhật trạng thái các nút toolbar dựa trên selection
        /// </summary>
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
                var rowCount = GridViewHelper.GetDisplayRowCount(ProductServiceGridView) ?? 0;
                if (ExportBarButtonItem != null)
                    ExportBarButtonItem.Enabled = rowCount > 0;
            }
            catch
            {
                // ignore
            }
        }

        /// <summary>
        /// Cập nhật status bar với thông tin selection và data summary
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
        /// Cập nhật thông tin số dòng đang được chọn
        /// </summary>
        private void UpdateSelectedRowStatus()
        {
            try
            {
                if (SelectedRowBarStaticItem == null) return;

                var selectedCount = _selectedProductServiceIds?.Count ?? 0;
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
        /// Cập nhật thông tin tổng kết dữ liệu với HTML formatting và pagination info
        /// </summary>
        private void UpdateDataSummaryStatus()
        {
            try
            {
                if (DataSummaryBarStaticItem == null) return;

                var currentData = productServiceDtoBindingSource.DataSource as List<ProductServiceDto>;
                if (currentData == null || !currentData.Any())
                {
                    DataSummaryBarStaticItem.Caption = @"Chưa có dữ liệu";
                    return;
                }

                var totalCount = currentData.Count;
                var productCount = currentData.Count(x => !x.IsService);
                var serviceCount = currentData.Count(x => x.IsService);
                var activeCount = currentData.Count(x => x.IsActive);
                var inactiveCount = currentData.Count(x => !x.IsActive);

                // Tạo HTML content với màu sắc
                var summary = $"<b>Tổng số: {totalCount}</b> | " +
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

        /// <summary>
        /// Xử lý sự kiện khi editor được hiển thị (lưu rowHandle đang edit)
        /// </summary>
        private void ProductServiceGridView_ShownEditor(object sender, EventArgs e)
        {
            try
            {
                if (sender is not GridView view) return;
                _editingRowHandle = view.FocusedRowHandle;
            }
            catch (Exception)
            {
                // ignore
            }
        }

        /// <summary>
        /// Xử lý sự kiện khi editor bị ẩn (clear rowHandle)
        /// </summary>
        private void ProductServiceGridView_HiddenEditor(object sender, EventArgs e)
        {
            try
            {
                _editingRowHandle = GridControl.InvalidRowHandle;
            }
            catch (Exception)
            {
                // ignore
            }
        }

        /// <summary>
        /// Xử lý sự kiện ImageChanged của RepositoryItemPictureEdit để cập nhật thumbnail sản phẩm/dịch vụ
        /// </summary>
        private async void ProductThumbnailRepositoryItemPictureEdit_ImageChanged(object sender, EventArgs e)
        {
            if (_isLoading) return;

            try
            {
                if (sender is not PictureEdit pictureEdit) return;

                // Lấy row đang được edit
                if (_editingRowHandle < 0 || _editingRowHandle == GridControl.InvalidRowHandle)
                {
                    // Fallback: lấy từ focused row
                    _editingRowHandle = ProductServiceGridView.FocusedRowHandle;
                }

                if (_editingRowHandle < 0 || _editingRowHandle == GridControl.InvalidRowHandle)
                {
                    return; // Không có row nào đang được edit
                }

                // Lấy DTO từ row
                if (ProductServiceGridView.GetRow(_editingRowHandle) is not ProductServiceDto productDto)
                {
                    return;
                }

                var productId = productDto.Id;

                // Xử lý upload thumbnail
                await ExecuteWithWaitingFormAsync(async () =>
                {
                    if (pictureEdit.Image != null)
                    {
                        // Trường hợp có hình ảnh mới - UPLOAD
                        var imageBytes = ImageToByteArray(pictureEdit.Image);

                        // Kiểm tra kích thước hình ảnh (tối đa 10MB)
                        const int maxSizeInBytes = 10 * 1024 * 1024; // 10MB
                        if (imageBytes.Length > maxSizeInBytes)
                        {
                            MsgBox.ShowWarning("Hình ảnh quá lớn! Vui lòng chọn hình ảnh nhỏ hơn 10MB.");
                            return;
                        }

                        // Kiểm tra format hình ảnh
                        if (!IsValidImageFormat(imageBytes))
                        {
                            MsgBox.ShowWarning(
                                "Định dạng hình ảnh không được hỗ trợ! Vui lòng chọn file JPG, PNG hoặc GIF.");
                            return;
                        }

                        // Upload thumbnail (lưu ảnh gốc lên NAS và thumbnail đã resize vào database)
                        // Sử dụng thumbnailMaxDimension = 120px để phù hợp với Width của cột thumbnail
                        const int thumbnailMaxDimension = 120;
                        await _productServiceBll.UpdateThumbnailImageAsync(productId, imageBytes, thumbnailMaxDimension);

                        ShowInfo("Đã cập nhật ảnh đại diện sản phẩm/dịch vụ thành công!");

                        // Reload data để cập nhật thumbnail mới
                        await LoadDataAsyncWithoutSplash();
                    }
                    else
                    {
                        // Trường hợp hình ảnh bị xóa - XÓA thumbnail
                        await _productServiceBll.UpdateThumbnailImageAsync(productId, null);

                        ShowInfo("Đã xóa ảnh đại diện sản phẩm/dịch vụ thành công!");

                        // Reload data để cập nhật
                        await LoadDataAsyncWithoutSplash();
                    }
                });
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi cập nhật ảnh đại diện sản phẩm/dịch vụ");
            }
        }

        #endregion

        #region ========== CẤU HÌNH GRID ==========


        /// <summary>
        /// Tô màu/định dạng dòng theo trạng thái/loại sản phẩm/dịch vụ
        /// </summary>
        private void ProductServiceGridView_RowCellStyle(object sender,
            RowCellStyleEventArgs rowCellStyleEventArgs)
        {
            try
            {
                var view = sender as GridView;
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

                rowCellStyleEventArgs.Appearance.BackColor = backColor;
                rowCellStyleEventArgs.Appearance.ForeColor =
                    Color.Black; // chữ đen tương phản tốt trên các màu nền trên
                rowCellStyleEventArgs.Appearance.Options.UseBackColor = true;
                rowCellStyleEventArgs.Appearance.Options.UseForeColor = true;

                // Nếu sản phẩm/dịch vụ không hoạt động: làm nổi bật rõ ràng hơn
                if (!row.IsActive)
                {
                    rowCellStyleEventArgs.Appearance.ForeColor = Color.DarkRed;
                    rowCellStyleEventArgs.Appearance.Font =
                        new Font(rowCellStyleEventArgs.Appearance.Font, FontStyle.Strikeout);
                }
            }
            catch (Exception)
            {
                // ignore style errors
            }
        }

        #endregion

        #region ========== SỰ KIỆN CRUD ==========

        /// <summary>
        /// Người dùng bấm "Điều chỉnh"
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
            var dto = ProductServiceGridView.GetFocusedRow() as ProductServiceDto;
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
                using (OverlayManager.ShowScope(this))
                {
                    using (var form = new FrmProductServiceDetail(dto.Id))
                    {
                        form.StartPosition = FormStartPosition.CenterParent;
                        form.ShowDialog(this);

                        await LoadDataAsyncWithoutSplash();
                        UpdateButtonStates();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi hiển thị màn hình điều chỉnh");
            }
        }

        /// <summary>
        /// Người dùng bấm "Xóa"
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

            if (!MsgBox.ShowYesNo(confirmMessage)) return;

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
        /// Người dùng bấm "Xuất"
        /// </summary>
        private void ExportBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Chỉ cho phép xuất khi có dữ liệu hiển thị
            var rowCount = GridViewHelper.GetDisplayRowCount(ProductServiceGridView) ?? 0;
            if (rowCount <= 0)
            {
                ShowInfo("Không có dữ liệu để xuất.");
                return;
            }

            GridViewHelper.ExportGridControl(ProductServiceGridView, "ProductServices.xlsx");
        }

        /// <summary>
        /// Người dùng bấm "Đếm số lượng" - đếm VariantCount và ImageCount cho các sản phẩm/dịch vụ được chọn
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

                // Thực hiện đếm với splash screen
                _ = ExecuteWithWaitingFormAsync(async () => { await CountSelectedProductsAsync(); });
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khi đếm số lượng biến thể và hình ảnh");
            }
        }

        #endregion

        #region ========== TẢI DỮ LIỆU ==========

        /// <summary>
        /// Tải dữ liệu và bind vào Grid (Async, hiển thị WaitForm)
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
        /// Load tất cả dữ liệu không phân trang
        /// Tối ưu hiệu suất bằng cách load categories một lần vào dictionary
        /// </summary>
        private async Task LoadDataAsyncWithoutSplash()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("[Form] LoadDataAsyncWithoutSplash: Bắt đầu gọi BLL.GetAllAsync()");
                // Get all data
                var entities = await _productServiceBll.GetAllAsync();
                System.Diagnostics.Debug.WriteLine($"[Form] LoadDataAsyncWithoutSplash: Đã nhận được {entities?.Count ?? 0} entities từ BLL");

                System.Diagnostics.Debug.WriteLine("[Form] LoadDataAsyncWithoutSplash: Bắt đầu lấy categoryDict");
                // Load tất cả categories một lần vào dictionary để tối ưu hiệu suất
                var categoryDict = await _productServiceBll.GetCategoryDictAsync();
                System.Diagnostics.Debug.WriteLine($"[Form] LoadDataAsyncWithoutSplash: Đã lấy được {categoryDict?.Count ?? 0} categories");

                System.Diagnostics.Debug.WriteLine("[Form] LoadDataAsyncWithoutSplash: Bắt đầu convert sang DTO với categoryDict");
                // Convert to DTOs với categoryDict (tối ưu hơn resolver functions)
                var dtoList = entities.ToDtoList(categoryDict).ToList();
                System.Diagnostics.Debug.WriteLine($"[Form] LoadDataAsyncWithoutSplash: Đã convert được {dtoList.Count} DTOs");

                System.Diagnostics.Debug.WriteLine("[Form] LoadDataAsyncWithoutSplash: Bắt đầu bind vào grid");
                BindGrid(dtoList);
                System.Diagnostics.Debug.WriteLine("[Form] LoadDataAsyncWithoutSplash: Hoàn thành");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[Form] LoadDataAsyncWithoutSplash: LỖI: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"[Form] LoadDataAsyncWithoutSplash: StackTrace: {ex.StackTrace}");
                ShowError(ex, "Lỗi tải dữ liệu");
            }
        }

        /// <summary>
        /// Đếm số lượng biến thể và hình ảnh cho các sản phẩm/dịch vụ được chọn.
        /// Sử dụng method optimization mới
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

                // Đếm số lượng cho các sản phẩm được chọn sử dụng async method
                var counts = await _productServiceBll.GetCountsForProductsAsync(_selectedProductServiceIds.ToList());

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

                // Hiển thị thông báo đã đếm xong số lượng cho các sản phẩm được chọn
                MsgBox.ShowSuccess(
                    $"Đã đếm xong số lượng biến thể và hình ảnh cho {_selectedProductServiceIds.Count} sản phẩm/dịch vụ được chọn.");

                // Cập nhật status bar với thông tin mới
                UpdateStatusBar();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khi đếm số lượng cho các sản phẩm được chọn");
            }
        }

        /// <summary>
        /// Bind danh sách DTO vào Grid và cấu hình hiển thị
        /// </summary>
        private void BindGrid(List<ProductServiceDto> data)
        {
            productServiceDtoBindingSource.DataSource = data;
            ProductServiceGridView.BestFitColumns();
            UpdateButtonStates();
            UpdateStatusBar();
        }

        /// <summary>
        /// Xóa trạng thái chọn hiện tại trên Grid
        /// </summary>
        private void ClearSelectionState()
        {
            _selectedProductServiceIds.Clear();
            ProductServiceGridView.ClearSelection();
            ProductServiceGridView.FocusedRowHandle = GridControl.InvalidRowHandle;
            UpdateButtonStates();
            UpdateStatusBar();
        }

        #endregion

        #region ========== PHÂN TRANG ==========

        // Note: Pagination đã được loại bỏ, chỉ load tất cả dữ liệu
        // Các method GetPagedAsync và GetCountAsync vẫn được giữ lại trong BLL/Repository
        // để có thể sử dụng ở các form khác nếu cần

        #endregion

        #region ========== TIỆN ÍCH ==========

        /// <summary>
        /// Hiển thị thông tin
        /// </summary>
        private void ShowInfo(string message)
        {
            MsgBox.ShowSuccess(message);
        }

        /// <summary>
        /// Hiển thị lỗi với thông tin ngữ cảnh
        /// </summary>
        private void ShowError(Exception ex, string context = null)
        {
            if (string.IsNullOrWhiteSpace(context))
                MsgBox.ShowException(ex);
            else
                MsgBox.ShowException(new Exception(context + ": " + ex.Message, ex));
        }

        /// <summary>
        /// Chuyển đổi Image sang byte array
        /// </summary>
        private byte[] ImageToByteArray(Image image)
        {
            if (image == null) return null;

            using (var ms = new MemoryStream())
            {
                // Lưu với format JPEG để giảm kích thước
                image.Save(ms, ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Kiểm tra định dạng hình ảnh có hợp lệ không (JPG, PNG, GIF)
        /// </summary>
        private bool IsValidImageFormat(byte[] imageBytes)
        {
            if (imageBytes == null || imageBytes.Length < 4) return false;

            // Kiểm tra magic bytes
            // JPEG: FF D8 FF
            if (imageBytes[0] == 0xFF && imageBytes[1] == 0xD8 && imageBytes[2] == 0xFF)
                return true;

            // PNG: 89 50 4E 47
            if (imageBytes[0] == 0x89 && imageBytes[1] == 0x50 && imageBytes[2] == 0x4E && imageBytes[3] == 0x47)
                return true;

            // GIF: 47 49 46 38 (GIF8)
            if (imageBytes[0] == 0x47 && imageBytes[1] == 0x49 && imageBytes[2] == 0x46 && imageBytes[3] == 0x38)
                return true;

            return false;
        }

        #endregion
    }
}

