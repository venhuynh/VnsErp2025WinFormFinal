using Bll.MasterData.ProductServiceBll;
using Common.Common;
using Common.Utils;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.WinExplorer;
using DevExpress.XtraSplashScreen;
using DTO.MasterData.ProductService;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MasterData.ProductService
{
    /// <summary>
    /// UserControl quản lý hình ảnh sản phẩm - hiển thị danh sách hình ảnh với WinExplorerView.
    /// Cung cấp chức năng tìm kiếm, xem chi tiết, và quản lý hình ảnh sản phẩm.
    /// </summary>
    public partial class FrmProductImage : XtraForm
    {
        #region ========== KHAI BÁO BIẾN ==========

        /// <summary>
        /// Business Logic Layer cho hình ảnh sản phẩm
        /// </summary>
        private ProductImageBll _productImageBll;

        /// <summary>
        /// Business Logic Layer cho sản phẩm/dịch vụ
        /// </summary>
        private ProductServiceBll _productServiceBll;

        /// <summary>
        /// ID sản phẩm hiện tại đang xem hình ảnh
        /// </summary>
        private Guid? _currentProductId;

        /// <summary>
        /// Thiết lập ProductId và load hình ảnh
        /// </summary>
        /// <param name="productId">ID sản phẩm/dịch vụ</param>
        public void SetProductId(Guid? productId)
        {
            _currentProductId = productId;
            if (productId.HasValue)
            {
                LoadImagesWithWaitingForm();
            }
            else
            {
                ResetImageSelection();
            }
        }

        /// <summary>
        /// Danh sách hình ảnh hiện tại
        /// </summary>
        private List<ProductImageDto> _imageList;

        /// <summary>
        /// OpenFileDialog để chọn hình ảnh
        /// </summary>
        private XtraOpenFileDialog xtraOpenFileDialog1;

        #endregion

        #region ========== CONSTRUCTOR & PUBLIC METHODS ==========

        /// <summary>
        /// Khởi tạo UserControl quản lý hình ảnh sản phẩm.
        /// </summary>
        public FrmProductImage()
        {
            InitializeComponent();

            // Cấu hình columns
            ConfigureColumns();

            InitializeBll();
            InitializeEvents();
            InitializePerformanceOptimizations();
            
            // Thiết lập SuperToolTip cho các controls
            SetupSuperToolTips();
        }

        #endregion

        #region ========== KHỞI TẠO FORM ==========

        /// <summary>
        /// Khởi tạo Business Logic Layer với error handling đầy đủ
        /// </summary>
        private void InitializeBll()
        {
            try
            {
                _productImageBll = new ProductImageBll();
                _productServiceBll = new ProductServiceBll();
                _imageList = new List<ProductImageDto>();
            }
            catch (InvalidOperationException ex)
            {
                // Lỗi cấu hình Image Storage
                var errorMessage = "Không thể khởi tạo dịch vụ lưu trữ hình ảnh.\n\n" +
                                   "Nguyên nhân: " + ex.Message + "\n\n" +
                                   "Vui lòng kiểm tra cấu hình trong App.config:\n" +
                                   "- ImageStorage.StorageType (NAS hoặc Local)\n" +
                                   "- Nếu dùng NAS: ImageStorage.NAS.BasePath hoặc ImageStorage.NAS.ServerName + ImageStorage.NAS.ShareName\n" +
                                   "- Nếu dùng Local: ImageStorage.Local.BasePath\n\n" +
                                   "Form sẽ được mở nhưng chức năng upload hình ảnh sẽ bị vô hiệu hóa.";

                Common.Utils.MsgBox.ShowWarning(errorMessage, "Cảnh báo cấu hình", this);
                _productImageBll = null; // Set null để disable các chức năng upload
                DisableUploadControls(); // Disable các control liên quan
            }
            catch (Exception ex)
            {
                var errorMessage = "Lỗi khởi tạo dịch vụ lưu trữ hình ảnh: " + ex.Message;
                Common.Utils.MsgBox.ShowError(errorMessage, "Lỗi", this);
                _productImageBll = null; // Set null để disable các chức năng upload
                DisableUploadControls(); // Disable các control liên quan
            }
        }

        /// <summary>
        /// Disable các control liên quan đến upload hình ảnh khi BLL không khởi tạo được
        /// </summary>
        private void DisableUploadControls()
        {
            try
            {
                if (AddProductImagesBarButtonItem != null)
                {
                    AddProductImagesBarButtonItem.Enabled = false;
                    AddProductImagesBarButtonItem.Hint = "Chức năng upload hình ảnh đã bị vô hiệu hóa do thiếu cấu hình";
                }
            }
            catch (Exception ex)
            {
                // Log lỗi nhưng không throw để form vẫn có thể mở được
                System.Diagnostics.Debug.WriteLine($"Error disabling upload controls: {ex.Message}");
            }
        }

        /// <summary>
        /// Khởi tạo các tối ưu hóa hiệu suất theo ContextButtons pattern
        /// </summary>
        private void InitializePerformanceOptimizations()
        {
            try
            {
                // Cấu hình Hardware Acceleration
                ConfigureHardwareAcceleration();

                // Cấu hình Multiple Animation Types
                ConfigureAnimationTypes();

                // Cấu hình Dynamic Image Sizing
                ConfigureDynamicImageSizing();

                // Cấu hình Async Image Loading
                ConfigureAsyncImageLoading();
            }
            catch (Exception ex)
            {
                MsgBox.ShowError($"Lỗi khi khởi tạo tối ưu hóa hiệu suất: {ex.Message}");
            }
        }

        /// <summary>
        /// Cấu hình Hardware Acceleration
        /// </summary>
        private void ConfigureHardwareAcceleration()
        {
            try
            {
                // Cấu hình WinExplorerView để sử dụng hardware acceleration
                ProductImageDtoWinExplorerView.OptionsImageLoad.AsyncLoad = true;
                ProductImageDtoWinExplorerView.OptionsImageLoad.AnimationType = ImageContentAnimationType.Slide;
                ProductImageDtoWinExplorerView.OptionsImageLoad.CacheThumbnails = true;
                ProductImageDtoWinExplorerView.OptionsImageLoad.LoadThumbnailImagesFromDataSource = true;

                
                // Cấu hình selection options
                ProductImageDtoWinExplorerView.OptionsSelection.AllowMarqueeSelection = true;
                ProductImageDtoWinExplorerView.OptionsSelection.ItemSelectionMode = IconItemSelectionMode.Click;
            }
            catch (Exception ex)
            {
                MsgBox.ShowError($"Lỗi khi cấu hình Hardware Acceleration: {ex.Message}");
            }
        }

        /// <summary>
        /// Cấu hình Multiple Animation Types
        /// </summary>
        private void ConfigureAnimationTypes()
        {
            try
            {

                // Cấu hình màu sắc cho context buttons với transparency
                ProductImageDtoWinExplorerView.ContextButtonOptions.BottomPanelColor = Color.FromArgb(160, SystemColors.Control);
                ProductImageDtoWinExplorerView.ContextButtonOptions.TopPanelColor = Color.FromArgb(160, SystemColors.Control);
                ProductImageDtoWinExplorerView.ContextButtonOptions.Indent = 3;
                
            }
            catch (Exception ex)
            {
                MsgBox.ShowError($"Lỗi khi cấu hình Animation Types: {ex.Message}");
            }
        }

        /// <summary>
        /// Cấu hình Dynamic Image Sizing
        /// </summary>
        private void ConfigureDynamicImageSizing()
        {
            try
            {
                // Cấu hình kích thước hình ảnh với giá trị mặc định lớn nhất
                SetGalleryImageSize();

                // Cấu hình view style tối ưu
                ProductImageDtoWinExplorerView.OptionsView.Style = WinExplorerViewStyle.Medium;
            }
            catch (Exception ex)
            {
                MsgBox.ShowError($"Lỗi khi cấu hình Dynamic Image Sizing: {ex.Message}");
            }
        }

        /// <summary>
        /// Cấu hình Async Image Loading
        /// </summary>
        private void ConfigureAsyncImageLoading()
        {
            try
            {
                // Cấu hình async loading cho hiệu suất tốt hơn
                ProductImageDtoWinExplorerView.OptionsImageLoad.AsyncLoad = true;
                ProductImageDtoWinExplorerView.OptionsImageLoad.CacheThumbnails = true;
                ProductImageDtoWinExplorerView.OptionsImageLoad.LoadThumbnailImagesFromDataSource = true;
            }
            catch (Exception ex)
            {
                MsgBox.ShowError($"Lỗi khi cấu hình Async Image Loading: {ex.Message}");
            }
        }

        /// <summary>
        /// Khởi tạo events
        /// </summary>
        private void InitializeEvents()
        {
            // Event cho nút Add Image
            if (AddProductImagesBarButtonItem != null)
            {
                AddProductImagesBarButtonItem.ItemClick += AddProductImagesBarButtonItem_ItemClick;
            }
            
            // Event cho XemBaoCaoBarButtonItem (nút tìm kiếm)
            if (TimKiemHinhAnhBarButtonItem != null)
            {
                TimKiemHinhAnhBarButtonItem.ItemClick += TimKiemHinhAnhBarButtonItem_ItemClick;
            }
            
            // Event cho KeywordBarEditItem (BarEditItem) - lắng nghe Enter key
            if (KeywordBarEditItem != null && repositoryItemTextEdit2 != null)
            {
                // Lắng nghe KeyDown từ repository item
                repositoryItemTextEdit2.KeyDown += RepositoryItemTextEdit2_KeyDown;
            }
            
            if (ProductImageDtoWinExplorerView != null)
            {
                ProductImageDtoWinExplorerView.DoubleClick += ProductImageDtoWinExplorerView_DoubleClick;
                ProductImageDtoWinExplorerView.SelectionChanged += ProductImageDtoWinExplorerView_SelectionChanged;
            }
            
            // Event cho nút Xóa hình ảnh
            if (XoaHinhAnhDuocChonBarButtonItem != null)
            {
                XoaHinhAnhDuocChonBarButtonItem.ItemClick += XoaPhieuBarButtonItem_ItemClick;
            }
        }

        /// <summary>
        /// Xử lý sự kiện KeyDown của repositoryItemTextEdit2 (khi nhấn Enter trong KeywordBarEditItem)
        /// </summary>
        private void RepositoryItemTextEdit2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                PerformSearch();
            }
        }

        /// <summary>
        /// Xử lý sự kiện ItemClick của AddProductImagesBarButtonItem
        /// Mở form FrmProductImageAdd để thêm hình ảnh mà không cần chọn sản phẩm trước
        /// </summary>
        private void AddProductImagesBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowAddImageForm();
        }

        /// <summary>
        /// Load danh sách hình ảnh (synchronous fallback)
        /// </summary>
        private void LoadImages()
        {
            try
            {
                // Reset trước khi load dữ liệu mới
                ResetImageSelection();
                
                // GetAll() and GetByProductId() already return List<ProductImageDto>
                List<ProductImageDto> imageDtos;
                
                if (!_currentProductId.HasValue)
                {
                    // Nếu không có ProductId, load tất cả hình ảnh
                    imageDtos = _productImageBll.GetAll();
                }
                else
                {
                    // Nếu có ProductId, chỉ load hình ảnh của sản phẩm đó
                    imageDtos = _productImageBll.GetByProductId(_currentProductId.Value);
                }

                // Already DTOs, no conversion needed
                _imageList = imageDtos;

                // Hiển thị thông tin trong DataSummaryBarStaticItem
                ShowImageSummary();

                // Bind data và cấu hình grid
                BindGrid(_imageList);
            }
            catch (Exception ex)
            {
                MsgBox.ShowError($"Lỗi khi tải danh sách hình ảnh: {ex.Message}");
            }
        }

        /// <summary>
        /// Load danh sách hình ảnh với WaitingForm
        /// </summary>
        private void LoadImagesWithWaitingForm()
        {
            try
            {
                SplashScreenManager.ShowForm(typeof(WaitForm1));
                LoadImages();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            finally
            {
                SplashScreenManager.CloseForm();
            }
        }

        /// <summary>
        /// Map entities sang DTOs (DEPRECATED - BLL already returns DTOs)
        /// ImageData (thumbnail) được load trực tiếp từ DTO để tăng tốc độ hiển thị và cải thiện UX
        /// Thumbnail đã được lưu trong database, không cần load từ NAS/Local storage
        /// </summary>
        private List<ProductImageDto> MapEntitiesToDtos(List<ProductImageDto> dtos)
        {
            if (dtos == null)
                return new List<ProductImageDto>();

            // Load tất cả ProductService để tránh truy cập navigation property sau khi DataContext bị dispose
            // Tạo dictionary để lookup nhanh theo ProductId
            var productServiceDict = new Dictionary<Guid, ProductServiceDto>();
            try
            {
                if (_productServiceBll != null)
                {
                    // GetAll() already returns List<ProductServiceDto>
                    var productServices = _productServiceBll.GetAll();
                    foreach (var ps in productServices)
                    {
                        if (ps.Id != Guid.Empty)
                        {
                            productServiceDict[ps.Id] = ps;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log lỗi nhưng không throw để không chặn việc load hình ảnh
                System.Diagnostics.Debug.WriteLine($"Lỗi khi load ProductService: {ex.Message}");
            }

            // Group theo ProductId để tính số thứ tự hình ảnh trong mỗi sản phẩm
            var groupedByProduct = dtos
                .GroupBy(d => d.ProductId)
                .ToList();

            var processedDtos = new List<ProductImageDto>();

            foreach (var group in groupedByProduct)
            {
                // Sắp xếp theo CreateDate để xác định số thứ tự
                var sortedImages = group.OrderBy(d => d.CreateDate).ToList();
                
                for (int i = 0; i < sortedImages.Count; i++)
                {
                    var dto = sortedImages[i];
                    
                    // Lấy ProductService từ dictionary thay vì navigation property để tránh lỗi DataContext disposed
                    ProductServiceDto productService = null;
                    if (dto.ProductId != Guid.Empty && productServiceDict.TryGetValue(dto.ProductId.Value, out var value))
                    {
                        productService = value;
                    }

                    // ImageData in DTO is already byte[], no conversion needed
                    // Update DTO with additional information
                    dto.ProductCode = productService?.Code;
                    dto.ProductName = productService?.Name;
                    // Số thứ tự hình ảnh trong sản phẩm (bắt đầu từ 1)
                    dto.ImageSequenceNumber = i + 1;

                    processedDtos.Add(dto);
                }
            }

            // Sắp xếp theo sản phẩm để tạo separator tự nhiên
            return processedDtos.OrderBy(x => x.ProductName ?? "").ThenBy(x => x.ImageSequenceNumber).ToList();
        }

        /// <summary>
        /// Hiển thị form thêm hình ảnh (backward compatibility - giữ lại để tương thích)
        /// </summary>
        private void ShowAddImageForm()
        {
            try
            {
                // Sử dụng OverlayManager.ShowScope để auto-close overlay
                using (OverlayManager.ShowScope(this))
                {
                    using (var addImageForm = new FrmProductImageAdd())
                    {
                        // Cấu hình form
                        addImageForm.Text = @"Thêm hình ảnh sản phẩm";
                        addImageForm.StartPosition = FormStartPosition.CenterParent;
                        
                        // Hiển thị form dạng dialog
                        addImageForm.ShowDialog(this);
                        
                        // Reload danh sách hình ảnh sau khi đóng form
                        ReloadDataSource(); // Sử dụng reload thông minh
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowError($"Lỗi khi mở form thêm hình ảnh: {ex.Message}");
            }
        }

        #region ========== XỬ LÝ HÌNH ẢNH ==========

        /// <summary>
        /// Xử lý các hình ảnh đã chọn
        /// </summary>
        /// <param name="imagePaths">Danh sách đường dẫn hình ảnh</param>
        private async Task ProcessSelectedImagesAsync(string[] imagePaths)
        {
            try
            {
                await ExecuteWithWaitingFormAsync(async () =>
                {
                    await ProcessSelectedImagesWithoutSplashAsync(imagePaths);
                });
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khi xử lý hình ảnh");
            }
        }

        /// <summary>
        /// Xử lý các hình ảnh đã chọn (không hiển thị WaitingForm)
        /// </summary>
        /// <param name="imagePaths">Danh sách đường dẫn hình ảnh</param>
        private async Task ProcessSelectedImagesWithoutSplashAsync(string[] imagePaths)
        {
            var successCount = 0;
            var errorCount = 0;
            var errorMessages = new List<string>();

            foreach (var imagePath in imagePaths)
            {
                try
                {
                    // Lưu hình ảnh sử dụng BLL
                    var success = await SaveImageFromFileAsync(_currentProductId.Value, imagePath);

                    if (success)
                    {
                        successCount++;
                    }
                    else
                    {
                        errorCount++;
                        errorMessages.Add($"{Path.GetFileName(imagePath)}: Không thể lưu hình ảnh");
                    }
                }
                catch (Exception ex)
                {
                    errorCount++;
                    errorMessages.Add($"{Path.GetFileName(imagePath)}: {ex.Message}");
                }
            }

            // Hiển thị kết quả
            ShowImageProcessingResult(successCount, errorCount, errorMessages);

            // Reload danh sách hình ảnh nếu có ít nhất một hình ảnh thành công
            if (successCount > 0)
            {
                ReloadDataSource();
            }
        }

        /// <summary>
        /// Lưu hình ảnh từ file vào NAS/Local storage và metadata vào database
        /// </summary>
        /// <param name="productId">ID sản phẩm/dịch vụ</param>
        /// <param name="imageFilePath">Đường dẫn file ảnh</param>
        /// <returns>True nếu lưu thành công</returns>
        private async Task<bool> SaveImageFromFileAsync(Guid productId, string imageFilePath)
        {
            try
            {
                // Kiểm tra BLL đã được khởi tạo chưa
                if (_productImageBll == null)
                {
                    throw new InvalidOperationException(
                        "Dịch vụ lưu trữ hình ảnh chưa được cấu hình. " +
                        "Vui lòng kiểm tra lại cấu hình trong App.config và khởi động lại ứng dụng.");
                }

                if (!File.Exists(imageFilePath))
                {
                    throw new FileNotFoundException($"File ảnh không tồn tại: {imageFilePath}");
                }

                // Sử dụng BLL để lưu hình ảnh vào NAS/Local storage và metadata vào database
                // Method này sẽ:
                // 1. Đọc file ảnh
                // 2. Lưu vào NAS/Local storage thông qua ImageStorageService
                // 3. Lưu metadata (FileName, RelativePath, FullPath, etc.) vào database
                var productImage = await _productImageBll.SaveImageFromFileAsync(productId, imageFilePath, isPrimary: false);

                // Kiểm tra kết quả
                if (productImage == null)
                {
                    throw new InvalidOperationException($"Không thể lưu hình ảnh '{Path.GetFileName(imageFilePath)}'");
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lưu hình ảnh '{Path.GetFileName(imageFilePath)}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Hiển thị kết quả xử lý hình ảnh
        /// </summary>
        /// <param name="successCount">Số hình ảnh thành công</param>
        /// <param name="errorCount">Số hình ảnh lỗi</param>
        /// <param name="errorMessages">Danh sách lỗi</param>
        private void ShowImageProcessingResult(int successCount, int errorCount, List<string> errorMessages)
        {
            var message = "Kết quả xử lý hình ảnh:\n\n";
            message += $"✅ Thành công: {successCount} hình ảnh\n";
            message += $"❌ Lỗi: {errorCount} hình ảnh\n\n";

            if (errorCount > 0 && errorMessages.Any())
            {
                message += "Chi tiết lỗi:\n";
                foreach (var error in errorMessages.Take(5)) // Chỉ hiển thị 5 lỗi đầu tiên
                {
                    message += $"• {error}\n";
                }
                if (errorMessages.Count > 5)
                {
                    message += $"• ... và {errorMessages.Count - 5} lỗi khác\n";
                }
            }

            if (successCount > 0)
            {
                message += "\n🎉 Hình ảnh đã được lưu thành công!";
                MsgBox.ShowSuccess(message);
            }
            else
            {
                MsgBox.ShowError(message);
            }
        }

        #endregion

        /// <summary>
        /// Bind dữ liệu vào grid và cấu hình hiển thị
        /// </summary>
        private void BindGrid(List<ProductImageDto> imageList)
        {
            try
            {
                ProductImageDtoGridControl.DataSource = null;

                // Bind data
                ProductImageDtoGridControl.DataSource = imageList;
                
                // Cấu hình WinExplorerView
                ConfigureWinExplorerView();
                
                // Refresh grid
                ProductImageDtoGridControl.RefreshDataSource();
            }
            catch (Exception ex)
            {
                MsgBox.ShowError($"Lỗi khi bind dữ liệu vào grid: {ex.Message}");
            }
        }

        /// <summary>
        /// Cấu hình WinExplorerView - Tối ưu hóa theo DevExpress demo
        /// </summary>
        private void ConfigureWinExplorerView()
        {
            try
            {
                // Cấu hình view style tối ưu
                ProductImageDtoWinExplorerView.OptionsView.Style = WinExplorerViewStyle.Medium;

                // Cấu hình image size với giá trị mặc định lớn nhất
                SetGalleryImageSize();

                // Cấu hình context buttons nếu cần
                ConfigureContextButtons();
            }
            catch (Exception ex)
            {
                MsgBox.ShowError($"Lỗi khi cấu hình WinExplorerView: {ex.Message}");
            }
        }

        /// <summary>
        /// Cấu hình kích thước hình ảnh với giá trị mặc định lớn nhất
        /// </summary>
        private void SetGalleryImageSize()
        {
            try
            {
                // Sử dụng kích thước mặc định lớn nhất cho hiển thị tối ưu
                const int maxWidth = 300;

                // Cấu hình kích thước hình ảnh cho tất cả view styles với tỷ lệ tối ưu
                ProductImageDtoWinExplorerView.OptionsViewStyles.ExtraLarge.ImageSize = new Size(maxWidth, (int)(maxWidth * 0.6));
                ProductImageDtoWinExplorerView.OptionsViewStyles.Large.ImageSize = new Size(maxWidth, (int)(maxWidth * 0.6));
                ProductImageDtoWinExplorerView.OptionsViewStyles.Medium.ImageSize = new Size(maxWidth / 2, (int)(maxWidth * 0.3));
                ProductImageDtoWinExplorerView.OptionsViewStyles.Small.ImageSize = new Size(maxWidth / 3, (int)(maxWidth * 0.2));

                // Cấu hình List và Tiles view
                ProductImageDtoWinExplorerView.OptionsViewStyles.List.ImageSize = new Size(maxWidth / 4, (int)(maxWidth * 0.15));
                ProductImageDtoWinExplorerView.OptionsViewStyles.Tiles.ImageSize = new Size(maxWidth / 2, (int)(maxWidth * 0.3));
                ProductImageDtoWinExplorerView.OptionsViewStyles.Content.ImageSize = new Size(maxWidth / 3, (int)(maxWidth * 0.2));
            }
            catch (Exception ex)
            {
                MsgBox.ShowError($"Lỗi khi cấu hình kích thước hình ảnh: {ex.Message}");
            }
        }

        /// <summary>
        /// Cấu hình context buttons cho WinExplorerView với Multiple Animation Types
        /// </summary>
        private void ConfigureContextButtons()
        {
            try
            {
                // Cấu hình màu sắc cho context buttons với transparency
                ProductImageDtoWinExplorerView.ContextButtonOptions.BottomPanelColor = Color.FromArgb(160, SystemColors.Control);
                ProductImageDtoWinExplorerView.ContextButtonOptions.TopPanelColor = Color.FromArgb(160, SystemColors.Control);
                ProductImageDtoWinExplorerView.ContextButtonOptions.Indent = 3;
            }
            catch (Exception ex)
            {
                MsgBox.ShowError($"Lỗi khi cấu hình context buttons: {ex.Message}");
            }
        }

        /// <summary>
        /// Cấu hình columns cho WinExplorerView - Tối ưu hóa theo DevExpress demo
        /// </summary>
        private void ConfigureColumns()
        {
            try
            {
                // Clear columns trước khi thêm mới
                    ProductImageDtoWinExplorerView.Columns.Clear();
                
                // Thêm các columns theo thứ tự ưu tiên
                ProductImageDtoWinExplorerView.Columns.Add(new GridColumn { FieldName = "Id", Visible = false });
                ProductImageDtoWinExplorerView.Columns.Add(new GridColumn { FieldName = "Caption", Caption = @"Tên hình ảnh" });
                ProductImageDtoWinExplorerView.Columns.Add(new GridColumn { FieldName = "AltText", Caption = @"Mô tả" });
                ProductImageDtoWinExplorerView.Columns.Add(new GridColumn { FieldName = "ImageData", Caption = @"Hình ảnh" });
                ProductImageDtoWinExplorerView.Columns.Add(new GridColumn { FieldName = "IsPrimary", Caption = @"Ảnh chính" });
                ProductImageDtoWinExplorerView.Columns.Add(new GridColumn { FieldName = "ProductName", Caption = @"Sản phẩm", Visible = false });
                ProductImageDtoWinExplorerView.Columns.Add(new GridColumn { FieldName = "ImageType", Caption = @"Loại ảnh", Visible = false });
                ProductImageDtoWinExplorerView.Columns.Add(new GridColumn { FieldName = "ImageSize", Caption = @"Kích thước", Visible = false });
                ProductImageDtoWinExplorerView.Columns.Add(new GridColumn { FieldName = "SortOrder", Caption = @"Thứ tự", Visible = false });

                // Cấu hình ColumnSet theo DevExpress demo pattern
                ConfigureColumnSet();
            }
            catch (Exception ex)
            {
                MsgBox.ShowError($"Lỗi khi cấu hình columns: {ex.Message}");
            }
        }

        /// <summary>
        /// Cấu hình ColumnSet cho WinExplorerView theo DevExpress demo
        /// </summary>
        private void ConfigureColumnSet()
        {
            try
            {
                // Cấu hình image columns cho tất cả view styles
                ProductImageDtoWinExplorerView.ColumnSet.ExtraLargeImageColumn = ProductImageDtoWinExplorerView.Columns["ImageData"];
                ProductImageDtoWinExplorerView.ColumnSet.LargeImageColumn = ProductImageDtoWinExplorerView.Columns["ImageData"];
                ProductImageDtoWinExplorerView.ColumnSet.MediumImageColumn = ProductImageDtoWinExplorerView.Columns["ImageData"];
                ProductImageDtoWinExplorerView.ColumnSet.SmallImageColumn = ProductImageDtoWinExplorerView.Columns["ImageData"];
                
                // Cấu hình description column
                ProductImageDtoWinExplorerView.ColumnSet.DescriptionColumn = ProductImageDtoWinExplorerView.Columns["AltText"];
                
                // Cấu hình checkbox column cho ảnh chính
                ProductImageDtoWinExplorerView.ColumnSet.CheckBoxColumn = ProductImageDtoWinExplorerView.Columns["IsPrimary"];
                
                // Cấu hình group column để nhóm theo sản phẩm
                ProductImageDtoWinExplorerView.ColumnSet.GroupColumn = ProductImageDtoWinExplorerView.Columns["ProductName"];
            }
            catch (Exception ex)
            {
                MsgBox.ShowError($"Lỗi khi cấu hình ColumnSet: {ex.Message}");
            }
        }

        #endregion

        #region ========== QUẢN LÝ DỮ LIỆU ==========

        /// <summary>
        /// Xử lý sự kiện click nút Add Image
        /// </summary>
        private async Task BtnAddImage_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra BLL đã được khởi tạo chưa
                if (_productImageBll == null)
                {
                    ShowError("Dịch vụ lưu trữ hình ảnh chưa được cấu hình. " +
                              "Vui lòng kiểm tra lại cấu hình trong App.config và khởi động lại ứng dụng.");
                    return;
                }

                // Kiểm tra ProductId hợp lệ
                if (!_currentProductId.HasValue || _currentProductId.Value == Guid.Empty)
                {
                    ShowError("Vui lòng chọn sản phẩm trước khi thêm hình ảnh.");
                    return;
                }

                // Cấu hình OpenFileDialog để chọn nhiều hình ảnh
                if (xtraOpenFileDialog1 == null)
                {
                    xtraOpenFileDialog1 = new XtraOpenFileDialog();
                }
                
                xtraOpenFileDialog1.Filter = @"Hình ảnh|*.jpg;*.jpeg;*.png;*.gif;*.bmp|Tất cả files|*.*";
                xtraOpenFileDialog1.Multiselect = true;
                xtraOpenFileDialog1.Title = @"Chọn hình ảnh cho sản phẩm/dịch vụ";

                // Hiển thị dialog chọn file
                if (xtraOpenFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    var selectedFiles = xtraOpenFileDialog1.FileNames;
                    if (selectedFiles.Length > 0)
                    {
                        await ProcessSelectedImagesAsync(selectedFiles);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khi chọn hình ảnh");
            }
        }

        /// <summary>
        /// Xử lý sự kiện click nút Search (XemBaoCaoBarButtonItem)
        /// </summary>
        private void TimKiemHinhAnhBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PerformSearch();
        }

        /// <summary>
        /// Xử lý sự kiện click nút Xóa hình ảnh (XoaPhieuBarButtonItem)
        /// Xóa hình ảnh được chọn trên NAS và trong database
        /// </summary>
        private async void XoaPhieuBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                // Lấy danh sách hình ảnh được chọn
                var selectedImages = GetSelectedImages();
                
                if (selectedImages == null || selectedImages.Count == 0)
                {
                    MsgBox.ShowWarning("Vui lòng chọn ít nhất một hình ảnh để xóa.");
                    return;
                }

                // Xác nhận xóa
                var imageCount = selectedImages.Count;
                var confirmMessage = imageCount == 1
                    ? $"Bạn có chắc chắn muốn xóa hình ảnh này?\n\n" +
                      $"• Tên: {selectedImages[0].FileName ?? selectedImages[0].Caption ?? "N/A"}\n" +
                      $"• Hình ảnh sẽ bị xóa khỏi database và storage (NAS/Local)\n" +
                      $"• Thao tác này không thể hoàn tác"
                    : $"Bạn có chắc chắn muốn xóa {imageCount} hình ảnh đã chọn?\n\n" +
                      $"• Tất cả hình ảnh sẽ bị xóa khỏi database và storage (NAS/Local)\n" +
                      $"• Thao tác này không thể hoàn tác";

                if (!MsgBox.ShowYesNo(confirmMessage, "Xác nhận xóa"))
                {
                    return;
                }

                // Xóa hình ảnh với WaitingForm
                await ExecuteWithWaitingFormAsync(async () =>
                {
                    await DeleteSelectedImagesAsync(selectedImages);
                });
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khi xóa hình ảnh");
            }
        }

        /// <summary>
        /// Xóa các hình ảnh đã chọn (xóa trên NAS và trong database)
        /// </summary>
        /// <param name="selectedImages">Danh sách hình ảnh cần xóa</param>
        private async Task DeleteSelectedImagesAsync(List<ProductImageDto> selectedImages)
        {
            var successCount = 0;
            var errorCount = 0;
            var errorMessages = new List<string>();
            var deletedImageIds = new HashSet<Guid>(); // Lưu danh sách ID đã xóa thành công

            foreach (var imageDto in selectedImages)
            {
                try
                {
                    // Sử dụng DeleteImageCompleteAsync để xóa cả file trên NAS và record trong database
                    await _productImageBll.DeleteImageCompleteAsync(imageDto.Id);
                    deletedImageIds.Add(imageDto.Id);
                    successCount++;
                }
                catch (Exception ex)
                {
                    errorCount++;
                    var fileName = imageDto.FileName ?? imageDto.Caption ?? imageDto.Id.ToString();
                    errorMessages.Add($"{fileName}: {ex.Message}");
                }
            }

            // Hiển thị kết quả
            ShowDeleteResult(successCount, errorCount, errorMessages, selectedImages.Count);

            // Chỉ remove các DTO đã xóa thành công khỏi datasource (không reload tất cả để tăng UX)
            if (successCount > 0 && deletedImageIds.Any())
            {
                RemoveDeletedImagesFromDataSource(deletedImageIds);
            }
        }

        /// <summary>
        /// Remove các hình ảnh đã xóa khỏi datasource mà không cần reload tất cả (tăng UX)
        /// </summary>
        /// <param name="deletedImageIds">Danh sách ID hình ảnh đã xóa thành công</param>
        private void RemoveDeletedImagesFromDataSource(HashSet<Guid> deletedImageIds)
        {
            try
            {
                if (_imageList == null || !_imageList.Any())
                    return;

                // Remove các DTO đã xóa khỏi _imageList
                var removedCount = _imageList.RemoveAll(dto => dto != null && deletedImageIds.Contains(dto.Id));

                if (removedCount > 0)
                {
                    // Refresh datasource
                    ProductImageDtoGridControl.RefreshDataSource();
                    
                    // Clear selection
                    ProductImageDtoWinExplorerView.ClearSelection();
                    
                    // Cập nhật summary
                    ShowImageSummary();
                    
                    // Cập nhật status bar (sau khi xóa, không còn selection nào)
                    if (SelectedRowBarStaticItem != null)
                    {
                        SelectedRowBarStaticItem.Caption = "Đã chọn: 0";
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Lỗi khi remove hình ảnh đã xóa khỏi datasource: {ex.Message}");
                // Nếu có lỗi, fallback về reload toàn bộ
                // Lưu ý: Không gọi ReloadDataSource() ở đây vì có thể đang trong splash form
                // Thay vào đó, chỉ log lỗi và để user tự reload nếu cần
            }
        }

        /// <summary>
        /// Hiển thị kết quả xóa hình ảnh
        /// </summary>
        private void ShowDeleteResult(int successCount, int errorCount, List<string> errorMessages, int totalCount)
        {
            var message = "Kết quả xóa hình ảnh:\n\n";
            message += $"✅ Đã xóa: {successCount}/{totalCount} hình ảnh\n";
            
            if (errorCount > 0)
            {
                message += $"❌ Lỗi: {errorCount} hình ảnh\n\n";
                
                if (errorMessages.Any())
                {
                    message += "Chi tiết lỗi:\n";
                    foreach (var error in errorMessages.Take(5))
                    {
                        message += $"• {error}\n";
                    }
                    if (errorMessages.Count > 5)
                    {
                        message += $"• ... và {errorMessages.Count - 5} lỗi khác\n";
                    }
                }
            }

            if (successCount > 0)
            {
                message += "\n🎉 Đã xóa thành công!";
                MsgBox.ShowSuccess(message);
            }
            else
            {
                MsgBox.ShowError(message);
            }
        }

        /// <summary>
        /// Thực hiện tìm kiếm hình ảnh
        /// </summary>
        private void PerformSearch()
        {
            try
            {
                var searchKeyword = KeywordBarEditItem?.EditValue?.ToString()?.Trim();
                if (string.IsNullOrWhiteSpace(searchKeyword))
                {
                    // Nếu không có từ khóa, load lại tất cả hình ảnh
                    LoadImagesWithWaitingForm();
                    return;
                }

                ExecuteWithWaitingForm(() =>
                {
                    // Tìm kiếm sản phẩm/dịch vụ trước để lấy thông tin
                    var productServiceBll = new ProductServiceBll();
                    var productSearchResults = productServiceBll.Search(searchKeyword);
                    
                    if (!productSearchResults.Any())
                    {
                        // Không tìm thấy sản phẩm nào
                        _imageList.Clear();
                        BindGrid(_imageList);
                        
                        return;
                    }

                    // Tìm kiếm hình ảnh theo danh sách sản phẩm tìm được
                    var productIds = productSearchResults.Select(x => x.Id).ToList();
                    var searchResults = _productImageBll.SearchByProductIds(productIds);
                    
                    // Convert sang DTO và thêm thông tin sản phẩm
                    _imageList = searchResults.Select(img => 
                    {
                        var product = productSearchResults.FirstOrDefault(p => p.Id == img.ProductId);
                        if (product != null)
                        {
                            var dto = new ProductImageDto
                            {
                                Id = img.Id,
                                ProductId = img.ProductId ?? Guid.Empty,
                                ProductName = product.Name,
                                ImageData = img.ImageData?.ToArray(),
                                ModifiedDate = img.ModifiedDate,
                                FileName = img.FileName
                            };


                            return dto;
                        }

                        return null;
                    }).ToList();

                    // Filter null items
                    _imageList = _imageList.Where(x => x != null).ToList();

                    // ProductImageDto không còn UpdateDisplayProperties method
                    // ProductDisplayName là computed property, không cần set

                    // Sắp xếp theo sản phẩm để tạo separator tự nhiên
                    _imageList = _imageList.OrderBy(x => x.ProductName).ThenBy(x => x.SortOrder).ToList();

                    // Bind data
                    BindGrid(_imageList);
                    
                    // Hiển thị kết quả tìm kiếm với thông tin sản phẩm
                    ShowSearchResult(searchKeyword, _imageList.Count, productSearchResults.Count);
                });
            }
            catch (Exception ex)
            {
                MsgBox.ShowError($"Lỗi khi tìm kiếm hình ảnh: {ex.Message}");
            }
        }

        /// <summary>
        /// Hiển thị thông tin tổng quan về hình ảnh
        /// </summary>
        private void ShowImageSummary()
        {
            try
            {
                if (!_imageList.Any())
                {
                    if (DataSummaryBarStaticItem != null)
                    {
                        DataSummaryBarStaticItem.Caption = "📊 Không có hình ảnh nào để hiển thị";
                    }
                    return;
                }

                var productGroups = _imageList.GroupBy(x => x.ProductName).ToList();
                var totalSize = _imageList.Sum(x => x.ImageSize);
                var primaryImages = _imageList.Count(x => x.IsPrimary);
                var averageSize = _imageList.Any() ? totalSize / _imageList.Count : 0;

                var message = $"📊 Thông tin hình ảnh{Environment.NewLine}{Environment.NewLine}" +
                            $"📦 Tổng số sản phẩm: {productGroups.Count} sản phẩm{Environment.NewLine}" +
                            $"🖼️ Tổng số hình ảnh: {_imageList.Count} hình ảnh{Environment.NewLine}" +
                            $"⭐ Ảnh chính: {primaryImages} hình ảnh{Environment.NewLine}" +
                            $"📏 Kích thước trung bình: {(averageSize / 1024.0):F1} KB{Environment.NewLine}" +
                            $"💾 Tổng dung lượng: {(totalSize / 1024.0 / 1024.0):F2} MB{Environment.NewLine}{Environment.NewLine}" +
                            $"📋 Chi tiết theo sản phẩm:{Environment.NewLine}";

                foreach (var group in productGroups.OrderBy(g => g.Key))
                {
                    var groupSize = group.Sum(x => x.ImageSize);
                    var groupPrimary = group.Count(x => x.IsPrimary);
                    message += $"• {group.Key}: {group.Count()} hình ảnh";
                    if (groupPrimary > 0) message += $" ({groupPrimary} ảnh chính)";
                    message += $" - {(groupSize / 1024.0):F1} KB{Environment.NewLine}";
                }

                message += $"{Environment.NewLine}⏰ Thời gian: {DateTime.Now:HH:mm:ss dd/MM/yyyy}";

                if (DataSummaryBarStaticItem != null)
                {
                    DataSummaryBarStaticItem.Caption = message;
                }
            }
            catch (Exception ex)
            {
                if (DataSummaryBarStaticItem != null)
                {
                    DataSummaryBarStaticItem.Caption = $@"Lỗi khi hiển thị thông tin hình ảnh: {ex.Message}";
                }
            }
        }

        /// <summary>
        /// Hiển thị kết quả tìm kiếm với Environment.NewLine
        /// </summary>
        private void ShowSearchResult(string searchKeyword, int imageCount, int productCount = 0)
        {
            try
            {
                var message = $"🔍 Kết quả tìm kiếm hình ảnh{Environment.NewLine}{Environment.NewLine}" +
                            $"📝 Từ khóa: '{searchKeyword}'{Environment.NewLine}" +
                            $"📦 Sản phẩm tìm được: {productCount} sản phẩm/dịch vụ{Environment.NewLine}" +
                            $"🖼️ Hình ảnh tìm được: {imageCount} hình ảnh{Environment.NewLine}{Environment.NewLine}";

                if (productCount == 0)
                {
                    message += $"❌ Không tìm thấy sản phẩm/dịch vụ nào phù hợp{Environment.NewLine}{Environment.NewLine}" +
                              $"💡 Gợi ý:{Environment.NewLine}" +
                              $"• Kiểm tra lại từ khóa{Environment.NewLine}" +
                              $"• Thử từ khóa ngắn hơn{Environment.NewLine}" +
                              $"• Tìm kiếm theo tên sản phẩm, mã sản phẩm, danh mục{Environment.NewLine}" +
                              "• Sử dụng ký tự đại diện (*) để tìm kiếm mở rộng";
                }
                else if (imageCount == 0)
                {
                    message += $"✅ Tìm thấy {productCount} sản phẩm/dịch vụ{Environment.NewLine}" +
                              $"❌ Nhưng không có hình ảnh nào{Environment.NewLine}{Environment.NewLine}" +
                              $"💡 Gợi ý:{Environment.NewLine}" +
                              $"• Các sản phẩm này có thể chưa có hình ảnh{Environment.NewLine}" +
                              $"• Thử tìm kiếm với sản phẩm khác{Environment.NewLine}" +
                              "• Kiểm tra xem sản phẩm đã được upload hình ảnh chưa";
                }
                else
                {
                    message += $"✅ Tìm thấy {imageCount} hình ảnh từ {productCount} sản phẩm/dịch vụ{Environment.NewLine}{Environment.NewLine}" +
                              $"🔍 Logic tìm kiếm:{Environment.NewLine}" +
                              $"1️⃣ Tìm kiếm từ khóa trong sản phẩm/dịch vụ{Environment.NewLine}" +
                              $"2️⃣ Lấy hình ảnh của các sản phẩm tìm được{Environment.NewLine}" +
                              $"3️⃣ Hiển thị hình ảnh với thông tin sản phẩm{Environment.NewLine}{Environment.NewLine}" +
                              $"📊 Thống kê chi tiết:{Environment.NewLine}" +
                              $"• Tỷ lệ tìm thấy: {(productCount > 0 ? (imageCount * 100.0 / productCount).ToString("F1") : "0")}%{Environment.NewLine}" +
                              $"• Trung bình: {(productCount > 0 ? (imageCount / (double)productCount).ToString("F1") : "0")} hình ảnh/sản phẩm";
                }

                // Thêm thông tin thời gian tìm kiếm
                message += $"{Environment.NewLine}{Environment.NewLine}⏰ Thời gian: {DateTime.Now:HH:mm:ss dd/MM/yyyy}";

                // Hiển thị kết quả trong DataSummaryBarStaticItem
                if (DataSummaryBarStaticItem != null)
                {
                    DataSummaryBarStaticItem.Caption = message;
                }
            }
            catch (Exception ex)
            {
                if (DataSummaryBarStaticItem != null)
                {
                    DataSummaryBarStaticItem.Caption = $@"Tìm thấy {imageCount} hình ảnh từ {productCount} sản phẩm cho từ khóa: '{searchKeyword}'{Environment.NewLine}" +
                                        $@"Lỗi: {ex.Message}";
                }
            }
        }

        #endregion

        #region ========== TIỆN ÍCH ==========

        /// <summary>
        /// Reload datasource thông minh - kiểm tra trạng thái hiện tại để reload phù hợp
        /// </summary>
        private void ReloadDataSource()
        {
            try
            {
                // Reset các biến và thông tin hình ảnh đã chọn trước đó
                ResetImageSelection();
                
                var searchKeyword = KeywordBarEditItem?.EditValue?.ToString()?.Trim();
                
                if (string.IsNullOrWhiteSpace(searchKeyword))
                {
                    // Nếu không có từ khóa tìm kiếm, load lại tất cả hình ảnh
                    LoadImagesWithWaitingForm();
                }
                else
                {
                    // Nếu có từ khóa tìm kiếm, thực hiện lại tìm kiếm
                    PerformSearch();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowError($"Lỗi khi reload datasource: {ex.Message}");
            }
        }

        /// <summary>
        /// Reset các biến và thông tin hình ảnh đã chọn trước đó
        /// </summary>
        private void ResetImageSelection()
        {
            try
            {
                // Clear selection trong WinExplorerView
                ProductImageDtoWinExplorerView.ClearSelection();
                
                // Reset image list
                _imageList?.Clear();
                
                // Clear datasource
                ProductImageDtoGridControl.DataSource = null;
                
                // Clear result summary
                if (DataSummaryBarStaticItem != null)
                {
                    DataSummaryBarStaticItem.Caption = "Chưa có dữ liệu";
                }
                
                // Clear search keyword (optional - có thể giữ lại để user tiếp tục tìm kiếm)
                // KeywordBarEditItem.EditValue = string.Empty;
                
                // Refresh grid để cập nhật UI
                ProductImageDtoGridControl.RefreshDataSource();
                
                Debug.WriteLine("Đã reset selection và clear datasource");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Lỗi khi reset image selection: {ex.Message}");
            }
        }

        /// <summary>
        /// Thực hiện operation với WaitingForm1 hiển thị
        /// </summary>
        private void ExecuteWithWaitingForm(Action operation)
        {
            try
            {
                SplashScreenManager.ShowForm(typeof(WaitForm1));
                operation();
            }
            finally
            {
                SplashScreenManager.CloseForm();
            }
        }

        /// <summary>
        /// Thực hiện operation async với WaitingForm hiển thị
        /// </summary>
        /// <param name="operation">Operation async cần thực hiện</param>
        private async Task ExecuteWithWaitingFormAsync(Func<Task> operation)
        {
            try
            {
                // Hiển thị WaitingForm
                SplashScreenManager.ShowForm(typeof(WaitForm1));

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
                SplashScreenManager.CloseForm();
            }
        }


        /// <summary>
        /// Cập nhật Image Size với giá trị mặc định lớn nhất
        /// </summary>
        public void SetImageSize()
        {
            try
            {
                SetGalleryImageSize();
            }
            catch (Exception ex)
            {
                MsgBox.ShowError($"Lỗi khi cập nhật image size: {ex.Message}");
            }
        }


        #endregion

        /// <summary>
        /// Xử lý sự kiện Double Click để mở màn hình hiển thị chi tiết hình ảnh với kích thước thật
        /// </summary>
        private void ProductImageDtoWinExplorerView_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                // Lấy hình ảnh được chọn
                var selectedImage = GetSelectedImage();
                if (selectedImage == null)
                {
                    MsgBox.ShowWarning("Vui lòng chọn một hình ảnh để xem chi tiết.");
                    return;
                }

                // Mở form FrmProductImageDetail với topmost
                ShowProductImageDetailForm(selectedImage);
            }
            catch (Exception ex)
            {
                MsgBox.ShowError($"Lỗi khi mở màn hình chi tiết hình ảnh: {ex.Message}");
            }
        }

        /// <summary>
        /// Xử lý sự kiện Selection Changed để hiển thị thông tin chi tiết tại debug console
        /// </summary>
        private void ProductImageDtoWinExplorerView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                // Lấy hình ảnh được chọn
                var selectedImage = GetSelectedImage();
                if (selectedImage == null) return;

                // Hiển thị thông tin chi tiết tại debug console
                ShowImageDetailToConsole(selectedImage);
            }
            catch (Exception ex)
            {
                // Log lỗi vào debug console thay vì hiển thị message box
                Debug.WriteLine($"Lỗi khi hiển thị thông tin hình ảnh: {ex.Message}");
            }
        }

        /// <summary>
        /// Lấy hình ảnh đang được chọn
        /// </summary>
        private ProductImageDto GetSelectedImage()
        {
            try
            {
                var selectedRowHandles = ProductImageDtoWinExplorerView.GetSelectedRows();
                if (selectedRowHandles == null || selectedRowHandles.Length == 0) return null;

                var rowHandle = selectedRowHandles[0];
                if (rowHandle < 0) return null;

                return ProductImageDtoWinExplorerView.GetRow(rowHandle) as ProductImageDto;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Lỗi khi lấy hình ảnh được chọn: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Lấy danh sách tất cả hình ảnh được chọn
        /// </summary>
        private List<ProductImageDto> GetSelectedImages()
        {
            var selectedImages = new List<ProductImageDto>();
            try
            {
                var selectedRowHandles = ProductImageDtoWinExplorerView.GetSelectedRows();
                if (selectedRowHandles == null || selectedRowHandles.Length == 0)
                    return selectedImages;

                foreach (int rowHandle in selectedRowHandles)
                {
                    if (rowHandle < 0) continue;
                    
                    var dto = ProductImageDtoWinExplorerView.GetRow(rowHandle) as ProductImageDto;
                    if (dto != null)
                    {
                        selectedImages.Add(dto);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Lỗi khi lấy danh sách hình ảnh được chọn: {ex.Message}");
            }

            return selectedImages;
        }

        /// <summary>
        /// Hiển thị thông tin chi tiết hình ảnh tại debug console
        /// </summary>
        private void ShowImageDetailToConsole(ProductImageDto imageDto)
        {
            try
            {
                if (imageDto == null) return;

                var consoleMessage = "=== THÔNG TIN CHI TIẾT HÌNH ẢNH ===" + Environment.NewLine +
                                   $"Tên: {imageDto.Caption ?? "Không có"}" + Environment.NewLine +
                                   $"Mô tả: {imageDto.AltText ?? "Không có"}" + Environment.NewLine +
                                   $"Sản phẩm: {imageDto.ProductName ?? "Không xác định"}" + Environment.NewLine +
                                   $"ID: {imageDto.Id}" + Environment.NewLine +
                                   $"Ảnh chính: {(imageDto.IsPrimary ? "Có" : "Không")}" + Environment.NewLine +
                                   $"Kích thước: {imageDto.ImageWidth}x{imageDto.ImageHeight} pixels" + Environment.NewLine +
                                   $"Dung lượng: {(imageDto.ImageSize / 1024.0):F1} KB" + Environment.NewLine +
                                   $"Loại file: {imageDto.ImageType ?? "Không xác định"}" + Environment.NewLine +
                                   $"Thứ tự: {imageDto.SortOrder}" + Environment.NewLine +
                                   $"Ngày tạo: {imageDto.CreatedDate:dd/MM/yyyy HH:mm:ss}" + Environment.NewLine;

                if (imageDto.ModifiedDate.HasValue)
                {
                    consoleMessage += $"Ngày sửa: {imageDto.ModifiedDate.Value:dd/MM/yyyy HH:mm:ss}" + Environment.NewLine;
                }

                if (!string.IsNullOrEmpty(imageDto.ImagePath))
                {
                    consoleMessage += $"Đường dẫn: {imageDto.ImagePath}" + Environment.NewLine;
                }

                consoleMessage += $"Trạng thái: {(imageDto.IsActive ? "Hoạt động" : "Không hoạt động")}" + Environment.NewLine +
                                 $"Có dữ liệu ảnh: {(imageDto.ImageData != null && imageDto.ImageData.Length > 0 ? "Có" : "Không")}" + Environment.NewLine;

                if (imageDto.ImageData != null && imageDto.ImageData.Length > 0)
                {
                    consoleMessage += $"Kích thước dữ liệu: {imageDto.ImageData.Length} bytes" + Environment.NewLine;
                }

                consoleMessage += $"Thời gian xem: {DateTime.Now:HH:mm:ss dd/MM/yyyy}" + Environment.NewLine +
                                 "=================================";

                // Hiển thị tại debug console
                Debug.WriteLine(consoleMessage);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Lỗi khi hiển thị thông tin chi tiết: {ex.Message}");
            }
        }

        /// <summary>
        /// Thiết lập SuperToolTip cho các controls trong form
        /// </summary>
        private void SetupSuperToolTips()
        {
            try
            {
                // SuperToolTip cho nút Thêm hình ảnh
                if (AddProductImagesBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        AddProductImagesBarButtonItem,
                        title: "<b><color=Green>➕ Thêm hình ảnh</color></b>",
                        content: "Chọn một hoặc nhiều hình ảnh để thêm vào sản phẩm/dịch vụ.\n\n" +
                                "Hỗ trợ các định dạng: JPG, JPEG, PNG, GIF, BMP.\n" +
                                "Hình ảnh sẽ được lưu vào NAS/Local storage và metadata vào database."
                    );
                }

                // SuperToolTip cho ô tìm kiếm
                if (KeywordBarEditItem != null)
                {
                    var superTip = SuperToolTipHelper.CreateSuperToolTip(
                        title: "<b><color=Blue>🔍 Tìm kiếm</color></b>",
                        content: "Tìm kiếm hình ảnh theo tên sản phẩm/dịch vụ.\n\n" +
                                "• Nhấn Enter hoặc click nút Xem để thực hiện tìm kiếm\n" +
                                "• Tìm kiếm theo: tên sản phẩm, mã sản phẩm, danh mục\n" +
                                "• Để trống để hiển thị tất cả hình ảnh"
                    );
                    KeywordBarEditItem.SuperTip = superTip;
                }

                // SuperToolTip cho nút Xem
                if (TimKiemHinhAnhBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        TimKiemHinhAnhBarButtonItem,
                        title: "<b><color=Blue>👁️ Xem</color></b>",
                        content: "Thực hiện tìm kiếm hình ảnh theo từ khóa đã nhập.\n\n" +
                                "Kết quả sẽ hiển thị tất cả hình ảnh của các sản phẩm/dịch vụ phù hợp."
                    );
                }

                // SuperToolTip cho nút Xóa
                if (XoaHinhAnhDuocChonBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        XoaHinhAnhDuocChonBarButtonItem,
                        title: "<b><color=Red>🗑️ Xóa</color></b>",
                        content: "Xóa hình ảnh đã chọn.\n\n" +
                                "• Chọn một hoặc nhiều hình ảnh từ danh sách\n" +
                                "• Hình ảnh sẽ bị xóa khỏi database và storage\n" +
                                "• Thao tác này không thể hoàn tác"
                    );
                }

                // SuperToolTip cho nút Tải về
                if (XuatFileBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        XuatFileBarButtonItem,
                        title: "<b><color=Orange>📥 Tải về</color></b>",
                        content: "Tải xuống hình ảnh đã chọn.\n\n" +
                                "• Chọn một hoặc nhiều hình ảnh từ danh sách\n" +
                                "• Hình ảnh sẽ được tải xuống máy tính\n" +
                                "• Hỗ trợ tải nhiều hình ảnh cùng lúc"
                    );
                }

                // SuperToolTip cho nút Mặc định
                if (barButtonItem1 != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        barButtonItem1,
                        title: "<b><color=Gray>⚙️ Mặc định</color></b>",
                        content: "Khôi phục cài đặt mặc định cho hiển thị hình ảnh.\n\n" +
                                "• Khôi phục kích thước hình ảnh mặc định\n" +
                                "• Khôi phục style hiển thị mặc định"
                    );
                }


                // SuperToolTip cho Status Bar Items
                if (DataSummaryBarStaticItem != null)
                {
                    var summarySuperTip = SuperToolTipHelper.CreateSuperToolTip(
                        title: "<b><color=Teal>📊 Tổng kết</color></b>",
                        content: "Hiển thị thông tin tổng quan về hình ảnh.\n\n" +
                                "• Tổng số sản phẩm có hình ảnh\n" +
                                "• Tổng số hình ảnh\n" +
                                "• Kích thước trung bình và tổng dung lượng"
                    );
                    DataSummaryBarStaticItem.SuperTip = summarySuperTip;
                }

                if (SelectedRowBarStaticItem != null)
                {
                    var selectedSuperTip = SuperToolTipHelper.CreateSuperToolTip(
                        title: "<b><color=Teal>👆 Đang chọn</color></b>",
                        content: "Hiển thị số lượng hình ảnh đang được chọn.\n\n" +
                                "• Click vào hình ảnh để chọn\n" +
                                "• Có thể chọn nhiều hình ảnh cùng lúc"
                    );
                    SelectedRowBarStaticItem.SuperTip = selectedSuperTip;
                }
            }
            catch (Exception ex)
            {
                // Ignore lỗi setup SuperToolTip để không chặn form
                Debug.WriteLine($"Lỗi setup SuperToolTip: {ex.Message}");
            }
        }

        /// <summary>
        /// Hiển thị lỗi với thông tin ngữ cảnh
        /// </summary>
        private void ShowError(Exception ex, string context = null)
        {
            MsgBox.ShowException(
                string.IsNullOrWhiteSpace(context) ? ex : new Exception($"{context}: {ex.Message}", ex));
        }

        /// <summary>
        /// Hiển thị lỗi với thông báo
        /// </summary>
        private void ShowError(string message)
        {
            MsgBox.ShowError(message);
        }

        /// <summary>
        /// Mở form FrmProductImageDetail để hiển thị chi tiết hình ảnh với topmost
        /// </summary>
        private void ShowProductImageDetailForm(ProductImageDto imageDto)
        {
            try
            {
                if (imageDto == null) return;

                // Tạo form FrmProductImageDetail với ID hình ảnh
                using var detailForm = new FrmProductImageDetail(imageDto.Id);
                // Cấu hình form
                detailForm.Text = $@"Chi tiết hình ảnh: {imageDto.Caption ?? "Không có tên"}";
                detailForm.StartPosition = FormStartPosition.CenterParent;
                detailForm.TopMost = true; // Đặt form ở topmost
                detailForm.WindowState = FormWindowState.Normal;
                    
                // Lưu trạng thái trước khi mở form
                var originalImageCount = _imageList?.Count ?? 0;
                    
                // Hiển thị form
                var dialogResult = detailForm.ShowDialog(this);
                    
                // Chỉ reload nếu có thay đổi dữ liệu (xóa hình ảnh)
                if (detailForm.WasImageDeleted || (_imageList?.Count ?? 0) != originalImageCount)
                {
                    Debug.WriteLine("Phát hiện thay đổi dữ liệu, reloading datasource...");
                    ReloadDataSource();
                }
                else
                {
                    Debug.WriteLine("Không có thay đổi dữ liệu, không cần reload");
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowError($"Lỗi khi mở form chi tiết hình ảnh: {ex.Message}");
            }
        }

    }
}
