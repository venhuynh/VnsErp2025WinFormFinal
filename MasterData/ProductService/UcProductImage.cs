using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Bll.Common;
using Bll.MasterData.ProductServiceBll;
using Bll.Utils;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.WinExplorer;
using DevExpress.XtraSplashScreen;
using MasterData.ProductService.Dto;

namespace MasterData.ProductService
{
    /// <summary>
    /// UserControl quản lý hình ảnh sản phẩm - hiển thị danh sách hình ảnh với WinExplorerView.
    /// Cung cấp chức năng tìm kiếm, xem chi tiết, và quản lý hình ảnh sản phẩm.
    /// </summary>
    public partial class UcProductImage : XtraUserControl
    {
        #region ========== KHAI BÁO BIẾN ==========

        /// <summary>
        /// Business Logic Layer cho hình ảnh sản phẩm
        /// </summary>
        private ProductImageBll _productImageBll;

        /// <summary>
        /// ID sản phẩm hiện tại đang xem hình ảnh
        /// </summary>
        private Guid? _currentProductId;

        /// <summary>
        /// Danh sách hình ảnh hiện tại
        /// </summary>
        private List<ProductImageDto> _imageList;

        #endregion

        #region ========== CONSTRUCTOR & PUBLIC METHODS ==========

        /// <summary>
        /// Khởi tạo UserControl quản lý hình ảnh sản phẩm.
        /// </summary>
        public UcProductImage()
        {
            InitializeComponent();

            // Cấu hình columns
            ConfigureColumns();

            InitializeBll();
            InitializeEvents();
            InitializePerformanceOptimizations();
        }

        #endregion

        #region ========== KHỞI TẠO FORM ==========

        /// <summary>
        /// Khởi tạo Business Logic Layer
        /// </summary>
        private void InitializeBll()
        {
            _productImageBll = new ProductImageBll();
            _imageList = new List<ProductImageDto>();
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
                ProductImageServiceGWinExplorerView.OptionsImageLoad.AsyncLoad = true;
                ProductImageServiceGWinExplorerView.OptionsImageLoad.AnimationType = ImageContentAnimationType.Slide;
                ProductImageServiceGWinExplorerView.OptionsImageLoad.CacheThumbnails = true;
                ProductImageServiceGWinExplorerView.OptionsImageLoad.LoadThumbnailImagesFromDataSource = true;

                
                // Cấu hình selection options
                ProductImageServiceGWinExplorerView.OptionsSelection.AllowMarqueeSelection = true;
                ProductImageServiceGWinExplorerView.OptionsSelection.ItemSelectionMode = IconItemSelectionMode.Click;
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
                ProductImageServiceGWinExplorerView.ContextButtonOptions.BottomPanelColor = Color.FromArgb(160, SystemColors.Control);
                ProductImageServiceGWinExplorerView.ContextButtonOptions.TopPanelColor = Color.FromArgb(160, SystemColors.Control);
                ProductImageServiceGWinExplorerView.ContextButtonOptions.Indent = 3;
                
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
                ProductImageServiceGWinExplorerView.OptionsView.Style = WinExplorerViewStyle.Medium;
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
                ProductImageServiceGWinExplorerView.OptionsImageLoad.AsyncLoad = true;
                ProductImageServiceGWinExplorerView.OptionsImageLoad.CacheThumbnails = true;
                ProductImageServiceGWinExplorerView.OptionsImageLoad.LoadThumbnailImagesFromDataSource = true;
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
            btnAddImage.Click += BtnAddImage_Click;
            
            // Event cho SearchByKeyworkButtonEdit
            SearchByKeyworkButtonEdit.ButtonClick += SearchByKeyworkButtonEdit_ButtonClick;
            SearchByKeyworkButtonEdit.KeyDown += SearchByKeyworkButtonEdit_KeyDown;
            
            ProductImageServiceGWinExplorerView.DoubleClick += ProductImageServiceGWinExplorerView_DoubleClick;
            ProductImageServiceGWinExplorerView.SelectionChanged += ProductImageServiceGWinExplorerView_SelectionChanged;
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
                
                if (!_currentProductId.HasValue)
                {
                    _imageList.Clear();
                    ProductImageServiceGridControl.DataSource = null;
                    return;
                }

                // Lấy danh sách hình ảnh từ BLL
                var images = _productImageBll.GetByProductId(_currentProductId.Value);
                
                // Convert sang DTO - Tối ưu hóa bằng cách chỉ load ImageData khi cần thiết
                _imageList = images.Select(img => new ProductImageDto
                {
                    Id = img.Id,
                    ProductId = img.ProductId ?? Guid.Empty,
                    VariantId = img.VariantId,
                    ImagePath = img.ImagePath,
                    SortOrder = img.SortOrder ?? 0,
                    IsPrimary = img.IsPrimary ?? false,
                    ImageData = img.ImageData?.ToArray(), // Chỉ load khi cần thiết
                    ImageType = img.ImageType,
                    ImageSize = img.ImageSize ?? 0,
                    ImageWidth = img.ImageWidth ?? 0,
                    ImageHeight = img.ImageHeight ?? 0,
                    Caption = img.Caption,
                    AltText = img.AltText,
                    IsActive = img.IsActive ?? false,
                    CreatedDate = img.CreatedDate ?? DateTime.Now,
                    ModifiedDate = img.ModifiedDate
                }).ToList();

                // Sắp xếp theo sản phẩm để tạo separator tự nhiên
                _imageList = _imageList.OrderBy(x => x.ProductName).ThenBy(x => x.SortOrder).ToList();

                // Hiển thị thông tin trong ResultMemoEdit
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
        /// Hiển thị form thêm hình ảnh
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

        /// <summary>
        /// Bind dữ liệu vào grid và cấu hình hiển thị
        /// </summary>
        private void BindGrid(List<ProductImageDto> imageList)
        {
            try
            {
                ProductImageServiceGridControl.DataSource = null;

                // Bind data
                ProductImageServiceGridControl.DataSource = imageList;
                
                // Cấu hình WinExplorerView
                ConfigureWinExplorerView();
                
                // Refresh grid
                ProductImageServiceGridControl.RefreshDataSource();
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
                ProductImageServiceGWinExplorerView.OptionsView.Style = WinExplorerViewStyle.Medium;

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
                ProductImageServiceGWinExplorerView.OptionsViewStyles.ExtraLarge.ImageSize = new Size(maxWidth, (int)(maxWidth * 0.6));
                ProductImageServiceGWinExplorerView.OptionsViewStyles.Large.ImageSize = new Size(maxWidth, (int)(maxWidth * 0.6));
                ProductImageServiceGWinExplorerView.OptionsViewStyles.Medium.ImageSize = new Size(maxWidth / 2, (int)(maxWidth * 0.3));
                ProductImageServiceGWinExplorerView.OptionsViewStyles.Small.ImageSize = new Size(maxWidth / 3, (int)(maxWidth * 0.2));

                // Cấu hình List và Tiles view
                ProductImageServiceGWinExplorerView.OptionsViewStyles.List.ImageSize = new Size(maxWidth / 4, (int)(maxWidth * 0.15));
                ProductImageServiceGWinExplorerView.OptionsViewStyles.Tiles.ImageSize = new Size(maxWidth / 2, (int)(maxWidth * 0.3));
                ProductImageServiceGWinExplorerView.OptionsViewStyles.Content.ImageSize = new Size(maxWidth / 3, (int)(maxWidth * 0.2));
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
                ProductImageServiceGWinExplorerView.ContextButtonOptions.BottomPanelColor = Color.FromArgb(160, SystemColors.Control);
                ProductImageServiceGWinExplorerView.ContextButtonOptions.TopPanelColor = Color.FromArgb(160, SystemColors.Control);
                ProductImageServiceGWinExplorerView.ContextButtonOptions.Indent = 3;
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
                    ProductImageServiceGWinExplorerView.Columns.Clear();
                
                // Thêm các columns theo thứ tự ưu tiên
                ProductImageServiceGWinExplorerView.Columns.Add(new GridColumn { FieldName = "Id", Visible = false });
                ProductImageServiceGWinExplorerView.Columns.Add(new GridColumn { FieldName = "Caption", Caption = @"Tên hình ảnh" });
                ProductImageServiceGWinExplorerView.Columns.Add(new GridColumn { FieldName = "AltText", Caption = @"Mô tả" });
                ProductImageServiceGWinExplorerView.Columns.Add(new GridColumn { FieldName = "ImageData", Caption = @"Hình ảnh" });
                ProductImageServiceGWinExplorerView.Columns.Add(new GridColumn { FieldName = "IsPrimary", Caption = @"Ảnh chính" });
                ProductImageServiceGWinExplorerView.Columns.Add(new GridColumn { FieldName = "ProductName", Caption = @"Sản phẩm", Visible = false });
                ProductImageServiceGWinExplorerView.Columns.Add(new GridColumn { FieldName = "ImageType", Caption = @"Loại ảnh", Visible = false });
                ProductImageServiceGWinExplorerView.Columns.Add(new GridColumn { FieldName = "ImageSize", Caption = @"Kích thước", Visible = false });
                ProductImageServiceGWinExplorerView.Columns.Add(new GridColumn { FieldName = "SortOrder", Caption = @"Thứ tự", Visible = false });

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
                ProductImageServiceGWinExplorerView.ColumnSet.ExtraLargeImageColumn = ProductImageServiceGWinExplorerView.Columns["ImageData"];
                ProductImageServiceGWinExplorerView.ColumnSet.LargeImageColumn = ProductImageServiceGWinExplorerView.Columns["ImageData"];
                ProductImageServiceGWinExplorerView.ColumnSet.MediumImageColumn = ProductImageServiceGWinExplorerView.Columns["ImageData"];
                ProductImageServiceGWinExplorerView.ColumnSet.SmallImageColumn = ProductImageServiceGWinExplorerView.Columns["ImageData"];
                
                // Cấu hình description column
                ProductImageServiceGWinExplorerView.ColumnSet.DescriptionColumn = ProductImageServiceGWinExplorerView.Columns["AltText"];
                
                // Cấu hình checkbox column cho ảnh chính
                ProductImageServiceGWinExplorerView.ColumnSet.CheckBoxColumn = ProductImageServiceGWinExplorerView.Columns["IsPrimary"];
                
                // Cấu hình group column để nhóm theo sản phẩm
                ProductImageServiceGWinExplorerView.ColumnSet.GroupColumn = ProductImageServiceGWinExplorerView.Columns["ProductName"];
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
        private void BtnAddImage_Click(object sender, EventArgs e)
        {
            ShowAddImageForm();
        }

        /// <summary>
        /// Xử lý sự kiện click nút Search
        /// </summary>
        private void SearchByKeyworkButtonEdit_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            PerformSearch();
        }

        /// <summary>
        /// Xử lý sự kiện nhấn phím Enter trong SearchByKeyworkButtonEdit
        /// </summary>
        private void SearchByKeyworkButtonEdit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                PerformSearch();
            }
        }

        /// <summary>
        /// Thực hiện tìm kiếm hình ảnh
        /// </summary>
        private void PerformSearch()
        {
            try
            {
                var searchKeyword = SearchByKeyworkButtonEdit.Text?.Trim();
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
                                VariantId = img.VariantId,
                                ImagePath = img.ImagePath,
                                SortOrder = img.SortOrder ?? 0,
                                IsPrimary = img.IsPrimary ?? false,
                                ImageData = img.ImageData?.ToArray(),
                                ImageType = img.ImageType,
                                ImageSize = img.ImageSize ?? 0,
                                ImageWidth = img.ImageWidth ?? 0,
                                ImageHeight = img.ImageHeight ?? 0,
                                Caption = img.Caption,
                                AltText = img.AltText,
                                IsActive = img.IsActive ?? false,
                                CreatedDate = img.CreatedDate ?? DateTime.Now,
                                ModifiedDate = img.ModifiedDate
                            };

                            // Thêm thông tin sản phẩm vào caption nếu có
                            dto.Caption = $"{img.Caption} ({product.Name})";
                            dto.AltText = $"{img.AltText} - Sản phẩm: {product.Name}";

                            return dto;
                        }

                        return null;
                    }).ToList();

                    // Filter null items
                    _imageList = _imageList.Where(x => x != null).ToList();

                    // Cập nhật display properties
                    foreach (var dto in _imageList)
                    {
                        dto.UpdateDisplayProperties();
                        // Set ProductDisplayName để hiển thị
                        dto.ProductDisplayName = $"Sản phẩm: {dto.ProductName ?? "Không xác định"}";
                    }

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
                    ResultMemoEdit.Text = $@"📊 Không có hình ảnh nào để hiển thị{Environment.NewLine}{Environment.NewLine}" +
                                        $@"⏰ Thời gian: {DateTime.Now:HH:mm:ss dd/MM/yyyy}";
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

                ResultMemoEdit.Text = message;
            }
            catch (Exception ex)
            {
                ResultMemoEdit.Text = $@"Lỗi khi hiển thị thông tin hình ảnh: {ex.Message}";
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

                // Hiển thị kết quả trong ResultMemoEdit
                ResultMemoEdit.Text = message;
            }
            catch (Exception ex)
            {
                ResultMemoEdit.Text = $@"Tìm thấy {imageCount} hình ảnh từ {productCount} sản phẩm cho từ khóa: '{searchKeyword}'{Environment.NewLine}" +
                                    $@"Lỗi: {ex.Message}";
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
                
                var searchKeyword = SearchByKeyworkButtonEdit.Text?.Trim();
                
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
                ProductImageServiceGWinExplorerView.ClearSelection();
                
                // Reset image list
                _imageList?.Clear();
                
                // Clear datasource
                ProductImageServiceGridControl.DataSource = null;
                
                // Clear result memo edit
                ResultMemoEdit.Text = string.Empty;
                
                // Clear search keyword (optional - có thể giữ lại để user tiếp tục tìm kiếm)
                // SearchByKeyworkButtonEdit.Text = string.Empty;
                
                // Refresh grid để cập nhật UI
                ProductImageServiceGridControl.RefreshDataSource();
                
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
        private void ProductImageServiceGWinExplorerView_DoubleClick(object sender, EventArgs e)
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
        private void ProductImageServiceGWinExplorerView_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
                var selectedRowHandles = ProductImageServiceGWinExplorerView.GetSelectedRows();
                if (selectedRowHandles == null || selectedRowHandles.Length == 0) return null;

                var rowHandle = selectedRowHandles[0];
                if (rowHandle < 0) return null;

                return ProductImageServiceGWinExplorerView.GetRow(rowHandle) as ProductImageDto;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Lỗi khi lấy hình ảnh được chọn: {ex.Message}");
                return null;
            }
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
        /// Mở form FrmProductImageDetail để hiển thị chi tiết hình ảnh với topmost
        /// </summary>
        private void ShowProductImageDetailForm(ProductImageDto imageDto)
        {
            try
            {
                if (imageDto == null) return;

                // Tạo form FrmProductImageDetail với ID hình ảnh
                using (var detailForm = new FrmProductImageDetail(imageDto.Id))
                {
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
            }
            catch (Exception ex)
            {
                MsgBox.ShowError($"Lỗi khi mở form chi tiết hình ảnh: {ex.Message}");
            }
        }

    }
}
