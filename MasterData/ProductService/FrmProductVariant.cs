using Bll.MasterData.ProductServiceBll;
using Common.Common;
using Common.Helpers;
using Common.Utils;
using DevExpress.Data;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSplashScreen;
using DTO.MasterData.ProductService;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MasterData.ProductService
{
    /// <summary>
    /// UserControl quản lý danh sách biến thể sản phẩm.
    /// Cung cấp chức năng CRUD đầy đủ với giao diện thân thiện.
    /// </summary>
    public partial class FrmProductVariant : XtraForm
    {
        #region ========== KHAI BÁO BIẾN ==========

        /// <summary>
        /// Business Logic Layer cho biến thể sản phẩm
        /// </summary>
        private readonly ProductVariantBll _productVariantBll = new ProductVariantBll();

        /// <summary>
        /// Danh sách ID biến thể đang được chọn
        /// </summary>
        private List<Guid> _selectedVariantIds = new List<Guid>();

        /// <summary>
        /// Trạng thái đang tải dữ liệu (guard tránh gọi LoadDataAsync song song)
        /// </summary>
        private bool _isLoading;

        /// <summary>
        /// Trạng thái splash screen đang hiển thị (guard tránh hiển thị splash screen nhiều lần)
        /// </summary>
        private bool _isSplashVisible;

        /// <summary>
        /// RowHandle đang được edit (để lấy ProductVariantId khi upload thumbnail)
        /// </summary>
        private int _editingRowHandle = GridControl.InvalidRowHandle;

        #endregion

        #region ========== CONSTRUCTOR & PUBLIC METHODS ==========

        /// <summary>
        /// Khởi tạo UserControl quản lý biến thể sản phẩm.
        /// </summary>
        public FrmProductVariant()
        {
            InitializeComponent();
            
            // Đăng ký event handlers
            ListDataBarButtonItem.ItemClick += ListDataBarButtonItem_ItemClick;
            NewBarButtonItem.ItemClick += NewBarButtonItem_ItemClick;
            EditBarButtonItem.ItemClick += EditBarButtonItem_ItemClick;
            DeleteBarButtonItem.ItemClick += DeleteBarButtonItem_ItemClick;
            CountVariantAndImageBarButtonItem.ItemClick += CountVariantAndImageBarButtonItem_ItemClick;
            ExportBarButtonItem.ItemClick += ExportBarButtonItem_ItemClick;
            UpdateProductVariantFullNameBarButtonItem.ItemClick += UpdateProductVariantFullNameBarButtonItem_ItemClick;

            // Grid events
            ProductVariantListGridView.SelectionChanged += ProductServiceMasterDetailViewGridView_SelectionChanged;
            ProductVariantListGridView.CustomDrawRowIndicator += ProductVariantListGridView_CustomDrawRowIndicator;
            ProductVariantListGridView.ShownEditor += ProductVariantListGridView_ShownEditor;
            ProductVariantListGridView.HiddenEditor += ProductVariantListGridView_HiddenEditor;

            // PictureEdit events
            ThumbnailItemPictureEdit.ImageChanged += ThumbnailItemPictureEdit_ImageChanged;

            // Thiết lập SuperToolTip cho các controls
            SetupSuperToolTips();

            UpdateButtonStates();
        }

        #endregion

        #region ========== SỰ KIỆN BUTTON ==========

        /// <summary>
        /// Người dùng bấm "Danh sách" để tải dữ liệu.
        /// </summary>
        private async void ListDataBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        /// <summary>
        /// Người dùng bấm "Mới".
        /// </summary>
        private void NewBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                // Mở form thêm mới biến thể
                var form = new FrmProductVariantDetail(Guid.Empty);
                form.ShowDialog();

                // Refresh dữ liệu sau khi đóng form (luôn refresh để đảm bảo dữ liệu mới nhất)
                ListDataBarButtonItem.PerformClick();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi hiển thị màn hình thêm mới");
            }
        }

        /// <summary>
        /// Người dùng bấm "Điều chỉnh".
        /// </summary>
        private void EditBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                // Kiểm tra có chọn đúng 1 dòng không
                if (_selectedVariantIds == null || _selectedVariantIds.Count != 1)
                {
                    ShowInfo("Vui lòng chọn đúng 1 biến thể để chỉnh sửa.");
                    return;
                }

                // Lấy ID biến thể đã chọn
                var variantId = _selectedVariantIds.First();
                
                // Mở form chỉnh sửa biến thể
                var form = new FrmProductVariantDetail(variantId);
                form.ShowDialog();
                
                // Refresh dữ liệu sau khi đóng form (sử dụng SmartRefreshAsync để tránh ObjectDisposedException)
                ListDataBarButtonItem.PerformClick();
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
            try
            {
                // Kiểm tra có chọn dòng nào không
                if (_selectedVariantIds == null || _selectedVariantIds.Count == 0)
                {
                    ShowInfo("Vui lòng chọn biến thể cần xóa.");
                    return;
                }

                // Xác nhận xóa
                var selectedCount = _selectedVariantIds.Count;
                var message = selectedCount == 1 
                    ? "Bạn có chắc chắn muốn xóa biến thể đã chọn?" 
                    : $"Bạn có chắc chắn muốn xóa {selectedCount} biến thể đã chọn?";
                
                if (!MsgBox.ShowYesNo(message, "Xác nhận xóa"))
                {
                    return;
                }

                // Thực hiện xóa
                await ExecuteWithWaitingFormAsync(async () =>
                {
                    var deletedCount = 0;
                    var errorCount = 0;
                    var errors = new List<string>();

                    foreach (var variantId in _selectedVariantIds)
                    {
                        try
                        {
                            await _productVariantBll.DeleteAsync(variantId);
                            deletedCount++;
                        }
                        catch (Exception ex)
                        {
                            errorCount++;
                            errors.Add($"ID {variantId}: {ex.Message}");
                        }
                    }

                    // Hiển thị kết quả
                    if (deletedCount > 0 && errorCount == 0)
                    {
                        ShowInfo($"Đã xóa thành công {deletedCount} biến thể.");
                        // Sử dụng LoadDataAsyncWithoutSplash để tránh nested splash screen
                        await LoadDataAsyncWithoutSplash();
                        
                        // Clear selection và cập nhật UI sau khi xóa thành công
                        _selectedVariantIds.Clear();
                        UpdateButtonStates();
                        UpdateStatusBar();
                    }
                    else if (deletedCount > 0 && errorCount > 0)
                    {
                        var errorMessage = string.Join("\n", errors);
                        ShowError(new Exception($"Xóa thành công {deletedCount} biến thể, lỗi {errorCount} biến thể:\n{errorMessage}"));
                        // Sử dụng LoadDataAsyncWithoutSplash để tránh nested splash screen
                        await LoadDataAsyncWithoutSplash();
                        
                        // Clear selection và cập nhật UI sau khi xóa một phần thành công
                        _selectedVariantIds.Clear();
                        UpdateButtonStates();
                        UpdateStatusBar();
                    }
                    else
                    {
                        var errorMessage = string.Join("\n", errors);
                        ShowError(new Exception($"Không thể xóa biến thể nào:\n{errorMessage}"));
                        // Không clear selection nếu xóa thất bại hoàn toàn
                    }
                });
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi xóa dữ liệu");
            }
        }

        /// <summary>
        /// Người dùng bấm "Thống kê".
        /// </summary>
        private async void CountVariantAndImageBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                // Kiểm tra có chọn dòng nào không
                if (_selectedVariantIds == null || _selectedVariantIds.Count == 0)
                {
                    ShowInfo("Vui lòng chọn biến thể cần thống kê.");
                    return;
                }

                // Thực hiện thống kê
                await ExecuteWithWaitingFormAsync(async () =>
                {
                    var selectedCount = _selectedVariantIds.Count;
                    var totalImageCount = 0;
                    var activeVariantCount = 0;
                    var inactiveVariantCount = 0;
                    var errors = new List<string>();

                    foreach (var variantId in _selectedVariantIds)
                    {
                        try
                        {
                            // Lấy thông tin biến thể
                            var variant = await _productVariantBll.GetByIdAsync(variantId);
                            if (variant != null)
                            {
                                // Đếm hình ảnh (tạm thời set 0 vì không load được navigation properties)
                                totalImageCount += 0; // variant.ProductImages?.Count ?? 0;
                                
                                // Đếm trạng thái
                                if (variant.IsActive)
                                    activeVariantCount++;
                                else
                                    inactiveVariantCount++;
                            }
                        }
                        catch (Exception ex)
                        {
                            errors.Add($"ID {variantId}: {ex.Message}");
                        }
                    }

                    // Hiển thị kết quả thống kê
                    var result = $"<b>Thống kê {selectedCount} biến thể đã chọn:</b>\n\n" +
                               $"• <color=green>Hoạt động: {activeVariantCount}</color>\n" +
                               $"• <color=red>Không hoạt động: {inactiveVariantCount}</color>\n" +
                               $"• <b>Tổng hình ảnh: {totalImageCount}</b>";

                    if (errors.Any())
                    {
                        result += $"\n\n<color=red>Lỗi khi thống kê:</color>\n{string.Join("\n", errors)}";
                    }

                    ShowInfo(result);
                });
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khi thống kê");
            }
        }

        /// <summary>
        /// Người dùng bấm "Xuất".
        /// </summary>
        private void ExportBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                GridViewHelper.ExportGridControl(ProductVariantListGridView, $"ProductVariants_{DateTime.Now:yyyyMMdd_HHmmss}");
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi xuất dữ liệu");
            }
        }

        /// <summary>
        /// Người dùng bấm "Cập nhật tên" để cập nhật VariantFullName cho tất cả biến thể.
        /// </summary>
        private async void UpdateProductVariantFullNameBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                // Xác nhận cập nhật
                if (!MsgBox.ShowYesNo("Bạn có chắc chắn muốn cập nhật tên đầy đủ cho tất cả biến thể?\n\nThao tác này sẽ cập nhật cột VariantFullName với format:\nTên sản phẩm - Đơn vị tính - Mã biến thể - Các thông tin biến thể", "Xác nhận cập nhật"))
                {
                    return;
                }

                // Thực hiện cập nhật
                await ExecuteWithWaitingFormAsync(async () =>
                {
                    var updatedCount = 0;
                    try
                    {
                        // Gọi BLL để cập nhật
                        await _productVariantBll.UpdateAllVariantFullNamesAsync();
                        
                        // Lấy số lượng biến thể đã cập nhật
                        var allVariants = await _productVariantBll.GetAllAsync();
                        updatedCount = allVariants.Count;

                        ShowInfo($"Đã cập nhật thành công tên đầy đủ cho {updatedCount} biến thể.");
                        
                        // Refresh dữ liệu để hiển thị thay đổi
                        await LoadDataAsyncWithoutSplash();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Lỗi cập nhật tên đầy đủ: {ex.Message}", ex);
                    }
                });
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi cập nhật tên đầy đủ");
            }
        }

        /// <summary>
        /// Grid selection thay đổi -> cập nhật danh sách Id đã chọn và trạng thái nút.
        /// </summary>
        private void ProductServiceMasterDetailViewGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                UpdateSelectedVariantIds();
                UpdateButtonStates();
                UpdateStatusBar();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        /// <summary>
        /// Variant grid selection thay đổi -> cập nhật danh sách Id đã chọn và trạng thái nút.
        /// </summary>
        private void VariantGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                UpdateSelectedVariantIds();
                UpdateButtonStates();
                UpdateStatusBar();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        /// <summary>
        /// Custom draw row indicator để hiển thị số thứ tự dòng
        /// </summary>
        private void ProductVariantListGridView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                // Chỉ hiển thị số thứ tự cho data rows, không hiển thị cho group rows
                if (e.Info.IsRowIndicator && e.RowHandle >= 0)
                {
                    // Tính số thứ tự (bắt đầu từ 1)
                    var rowNumber = e.RowHandle + 1;
                    
                    // Hiển thị số thứ tự
                    e.Info.DisplayText = rowNumber.ToString();
                }
            }
            catch (Exception)
            {
                // Nếu có lỗi, hiển thị text mặc định
                e.Info.DisplayText = "";
            }
        }

        /// <summary>
        /// Xử lý sự kiện khi editor được hiển thị (lưu rowHandle đang edit)
        /// </summary>
        private void ProductVariantListGridView_ShownEditor(object sender, EventArgs e)
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
        private void ProductVariantListGridView_HiddenEditor(object sender, EventArgs e)
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
        /// Xử lý sự kiện ImageChanged của RepositoryItemPictureEdit để cập nhật thumbnail biến thể sản phẩm
        /// </summary>
        private async void ThumbnailItemPictureEdit_ImageChanged(object sender, EventArgs e)
        {
            if (_isLoading) return;

            try
            {
                if (sender is not PictureEdit pictureEdit) return;

                // Lấy row đang được edit
                if (_editingRowHandle < 0 || _editingRowHandle == GridControl.InvalidRowHandle)
                {
                    // Fallback: lấy từ focused row
                    _editingRowHandle = ProductVariantListGridView.FocusedRowHandle;
                }

                if (_editingRowHandle < 0 || _editingRowHandle == GridControl.InvalidRowHandle)
                {
                    return; // Không có row nào đang được edit
                }

                // Lấy DTO từ row
                if (ProductVariantListGridView.GetRow(_editingRowHandle) is not ProductVariantListDto variantDto)
                {
                    return;
                }

                var variantId = variantDto.Id;

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
                        // Sử dụng method mới chỉ cập nhật thumbnail, không ảnh hưởng đến các trường khác
                        const int thumbnailMaxDimension = 120;
                        await _productVariantBll.UpdateThumbnailImageOnlyAsync(variantId, imageBytes, thumbnailMaxDimension);

                        ShowInfo("Đã cập nhật ảnh đại diện biến thể sản phẩm thành công!");

                        // Reload data để cập nhật thumbnail mới
                        await LoadDataAsyncWithoutSplash();
                    }
                    else
                    {
                        // Trường hợp hình ảnh bị xóa - XÓA thumbnail
                        // Sử dụng method mới chỉ xóa thumbnail, không ảnh hưởng đến các trường khác
                        await _productVariantBll.UpdateThumbnailImageOnlyAsync(variantId, null);

                        ShowInfo("Đã xóa ảnh đại diện biến thể sản phẩm thành công!");

                        // Reload data để cập nhật
                        await LoadDataAsyncWithoutSplash();
                    }
                });
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi cập nhật ảnh đại diện biến thể sản phẩm");
            }
        }

        /// <summary>
        /// Cập nhật danh sách ID biến thể đã chọn
        /// </summary>
        private void UpdateSelectedVariantIds()
        {
            try
            {
                _selectedVariantIds.Clear();
                
                var selectedRows = ProductVariantListGridView.GetSelectedRows();
                foreach (var rowHandle in selectedRows)
                {
                    if (rowHandle >= 0)
                    {
                        var dto = ProductVariantListGridView.GetRow(rowHandle) as ProductVariantListDto;
                        if (dto != null)
                        {
                            _selectedVariantIds.Add(dto.Id);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi cập nhật danh sách đã chọn");
            }
        }

        #endregion

        #region ========== QUẢN LÝ DỮ LIỆU ==========

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
        /// Sử dụng ProductVariantListDto cho danh sách biến thể.
        /// </summary>
        private async Task LoadDataAsyncWithoutSplash()
        {
            try
            {
                // GetAllAsync() already returns List<ProductVariantDto>
                var variants = await _productVariantBll.GetAllAsync();
                
                // Convert ProductVariantDto to ProductVariantListDto
                var variantListDtos = await ConvertToVariantListDtosAsync(variants);
                
                // Bind dữ liệu vào grid
                BindGrid(variantListDtos);
                
                // Clear selection và cập nhật UI sau khi load dữ liệu mới
                _selectedVariantIds.Clear();
                UpdateButtonStates();
                UpdateStatusBar();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi tải dữ liệu");
            }
        }




        /// <summary>
        /// Convert ProductVariantDto sang ProductVariantListDto (Async)
        /// Resize thumbnail images về kích thước cố định
        /// </summary>
        private Task<List<ProductVariantListDto>> ConvertToVariantListDtosAsync(List<ProductVariantDto> variants)
        {
            try
            {
                // Manually convert ProductVariantDto to ProductVariantListDto
                var result = variants.Select(v => new ProductVariantListDto
                {
                    Id = v.Id,
                    ProductCode = v.ProductCode,
                    ProductName = v.ProductName,
                    VariantCode = v.VariantCode,
                    VariantFullName = v.VariantName, // Map VariantName to VariantFullName
                    UnitName = v.UnitName,
                    IsActive = v.IsActive,
                    ThumbnailImage = v.ThumbnailImage,
                    ImageCount = v.ImageCount,
                    FullVariantInfo = v // Store full variant info for later use
                }).ToList();
                
                // Resize tất cả thumbnail images về kích thước cố định (60x60 pixels)
                const int thumbnailSize = 60;
                foreach (var dto in result)
                {
                    if (dto.ThumbnailImage != null && dto.ThumbnailImage.Length > 0)
                    {
                        try
                        {
                            dto.ThumbnailImage = ResizeThumbnailImage(dto.ThumbnailImage, thumbnailSize, thumbnailSize);
                        }
                        catch (Exception ex)
                        {
                            // Nếu resize lỗi, giữ nguyên hình ảnh gốc
                            System.Diagnostics.Debug.WriteLine($"Lỗi resize thumbnail cho variant {dto.Id}: {ex.Message}");
                        }
                    }
                }
                
                return Task.FromResult(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi convert sang ProductVariantListDto: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Resize thumbnail image về kích thước cố định
        /// </summary>
        /// <param name="imageBytes">Mảng byte của hình ảnh gốc</param>
        /// <param name="width">Chiều rộng mong muốn</param>
        /// <param name="height">Chiều cao mong muốn</param>
        /// <returns>Mảng byte của hình ảnh đã resize</returns>
        private byte[] ResizeThumbnailImage(byte[] imageBytes, int width, int height)
        {
            if (imageBytes == null || imageBytes.Length == 0)
                return imageBytes;

            try
            {
                using (var ms = new MemoryStream(imageBytes))
                using (var originalImage = Image.FromStream(ms))
                {
                    // Tính toán kích thước mới giữ nguyên tỷ lệ
                    var newSize = CalculateNewSize(originalImage.Width, originalImage.Height, width, height);
                    
                    // Tạo bitmap mới với kích thước đã tính
                    using (var resizedImage = new Bitmap(newSize.Width, newSize.Height))
                    {
                        using (var graphics = Graphics.FromImage(resizedImage))
                        {
                            // Cấu hình chất lượng vẽ cao
                            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                            graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                            graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                            
                            // Vẽ hình ảnh gốc lên bitmap mới với kích thước mới
                            graphics.DrawImage(originalImage, 0, 0, newSize.Width, newSize.Height);
                        }
                        
                        // Chuyển đổi bitmap thành mảng byte
                        using (var msOutput = new MemoryStream())
                        {
                            resizedImage.Save(msOutput, ImageFormat.Png);
                            return msOutput.ToArray();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Nếu có lỗi, trả về hình ảnh gốc
                System.Diagnostics.Debug.WriteLine($"Lỗi resize thumbnail: {ex.Message}");
                return imageBytes;
            }
        }

        /// <summary>
        /// Tính toán kích thước mới giữ nguyên tỷ lệ khung hình
        /// </summary>
        private Size CalculateNewSize(int originalWidth, int originalHeight, int maxWidth, int maxHeight)
        {
            // Nếu hình ảnh nhỏ hơn kích thước mong muốn, giữ nguyên
            if (originalWidth <= maxWidth && originalHeight <= maxHeight)
            {
                return new Size(originalWidth, originalHeight);
            }

            // Tính tỷ lệ để giữ nguyên aspect ratio
            var ratio = Math.Min((double)maxWidth / originalWidth, (double)maxHeight / originalHeight);

            return new Size(
                (int)(originalWidth * ratio),
                (int)(originalHeight * ratio)
            );
        }


        /// <summary>
        /// Bind danh sách ProductVariantListDto vào Grid và cấu hình hiển thị.
        /// </summary>
        private void BindGrid(List<ProductVariantListDto> data)
        {
            try
            {
                // Bind dữ liệu vào BindingSource
                productVariantListDtoBindingSource.DataSource = data;
                
                // Bind vào GridControl
                ProductVariantListGridControl.DataSource = productVariantListDtoBindingSource;
                
                // Cấu hình grid
                ProductVariantListGridView.BestFitColumns();
                
                // Cập nhật trạng thái
                UpdateButtonStates();
                UpdateStatusBar();
                
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi bind dữ liệu vào grid");
            }
        }


        #endregion

        #region ========== TIỆN ÍCH ==========

        /// <summary>
        /// Thiết lập SuperToolTip cho các controls trong UserControl
        /// </summary>
        private void SetupSuperToolTips()
        {
            try
            {
                if (ListDataBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        ListDataBarButtonItem,
                        title: "<b><color=Blue>📋 Danh sách</color></b>",
                        content: "Tải lại danh sách biến thể sản phẩm từ hệ thống."
                    );
                }

                if (NewBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        NewBarButtonItem,
                        title: "<b><color=Green>➕ Mới</color></b>",
                        content: "Thêm mới biến thể sản phẩm vào hệ thống."
                    );
                }

                if (EditBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        EditBarButtonItem,
                        title: "<b><color=Orange>✏️ Điều chỉnh</color></b>",
                        content: "Chỉnh sửa thông tin biến thể sản phẩm đã chọn."
                    );
                }

                if (DeleteBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        DeleteBarButtonItem,
                        title: "<b><color=Red>🗑️ Xóa</color></b>",
                        content: "Xóa các biến thể sản phẩm đã chọn khỏi hệ thống."
                    );
                }

                if (CountVariantAndImageBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        CountVariantAndImageBarButtonItem,
                        title: "<b><color=Purple>📊 Thống kê</color></b>",
                        content: "Thống kê số lượng hình ảnh và trạng thái cho các biến thể được chọn."
                    );
                }

                if (ExportBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        ExportBarButtonItem,
                        title: "<b><color=Purple>📤 Xuất</color></b>",
                        content: "Xuất danh sách biến thể sản phẩm ra file Excel."
                    );
                }

                if (UpdateProductVariantFullNameBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        UpdateProductVariantFullNameBarButtonItem,
                        title: "<b><color=Blue>🔄 Cập nhật tên</color></b>",
                        content: "Cập nhật tên đầy đủ (VariantFullName) cho tất cả biến thể.\nFormat: Tên sản phẩm - Đơn vị tính - Mã biến thể - Các thông tin biến thể"
                    );
                }
            }
            catch (Exception ex)
            {
                // Ignore lỗi setup SuperToolTip để không chặn UserControl
                System.Diagnostics.Debug.WriteLine($"Lỗi setup SuperToolTip: {ex.Message}");
            }
        }

        /// <summary>
        /// Thực hiện operation async với WaitingForm1 hiển thị.
        /// </summary>
        /// <param name="operation">Operation async cần thực hiện</param>
        private async Task ExecuteWithWaitingFormAsync(Func<Task> operation)
        {
            // Kiểm tra splash screen đã hiển thị chưa
            if (_isSplashVisible)
            {
                // Nếu đã hiển thị, chỉ thực hiện operation mà không hiển thị splash
                await operation();
                return;
            }

            try
            {
                // Đánh dấu splash screen đang hiển thị
                _isSplashVisible = true;
                
                // Hiển thị WaitingForm1
                SplashScreenManager.ShowForm(typeof(WaitForm1));

                // Thực hiện operation
                await operation();
            }
            finally
            {
                // Đóng WaitingForm1
                SplashScreenManager.CloseForm();
                
                // Đánh dấu splash screen đã đóng
                _isSplashVisible = false;
            }
        }

        /// <summary>
        /// Cập nhật trạng thái các nút
        /// </summary>
        private void UpdateButtonStates()
        {
            try
            {
                var selectedCount = _selectedVariantIds?.Count ?? 0;
                
                // Edit: chỉ khi chọn đúng 1 dòng
                if (EditBarButtonItem != null)
                    EditBarButtonItem.Enabled = selectedCount == 1;
                    
                // Delete: khi chọn >= 1 dòng
                if (DeleteBarButtonItem != null)
                    DeleteBarButtonItem.Enabled = selectedCount >= 1;
                    
                // Count: chỉ khi chọn >= 1 dòng
                if (CountVariantAndImageBarButtonItem != null)
                    CountVariantAndImageBarButtonItem.Enabled = selectedCount >= 1;
                    
                // Export: luôn enable (có thể xuất tất cả dữ liệu)
                if (ExportBarButtonItem != null)
                    ExportBarButtonItem.Enabled = true;
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
        /// Cập nhật thông tin số dòng đang được chọn.
        /// </summary>
        private void UpdateSelectedRowStatus()
        {
            try
            {
                if (SelectedRowBarStaticItem == null) return;

                var selectedCount = _selectedVariantIds?.Count ?? 0;
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
        /// Cập nhật thông tin tổng kết dữ liệu.
        /// </summary>
        private void UpdateDataSummaryStatus()
        {
            try
            {
                if (DataSummaryBarStaticItem == null) return;

                var currentData = productVariantListDtoBindingSource.DataSource as List<ProductVariantListDto>;
                if (currentData == null || !currentData.Any())
                {
                    DataSummaryBarStaticItem.Caption = @"Chưa có dữ liệu";
                    return;
                }

                var variantCount = currentData.Count;
                var activeVariantCount = currentData.Count(x => x.IsActive);
                var inactiveVariantCount = currentData.Count(x => !x.IsActive);
                var totalImageCount = currentData.Sum(x => x.ImageCount);

                var summary = $"<b>Biến thể: {variantCount}</b> | " +
                             $"<color=green>Hoạt động: {activeVariantCount}</color> | " +
                             $"<color=red>Không hoạt động: {inactiveVariantCount}</color> | " +
                             $"<b>Hình ảnh: {totalImageCount}</b>";

                DataSummaryBarStaticItem.Caption = summary;
            }
            catch
            {
                // ignore
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
