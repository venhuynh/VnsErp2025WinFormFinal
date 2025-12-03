using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.WinExplorer;
using DevExpress.Utils;
using DTO.Inventory.Query;
using Bll.Inventory.InventoryManagement;
using Dal.DataContext;
using Common.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common.Common;

namespace Inventory.Query
{
    /// <summary>
    /// Form hiển thị danh sách hình ảnh nhập/xuất kho sử dụng WinExplorerView
    /// </summary>
    public partial class FrmStockInOutImageLookup : XtraForm
    {
        #region ========== KHAI BÁO BIẾN ==========

        private Guid? _stockInOutMasterId;
        private List<StockInOutImageDto> _dataSource;
        private StockInOutImageBll _stockInOutImageBll;

        #endregion

        #region ========== CONSTRUCTOR ==========

        /// <summary>
        /// Constructor mặc định - hiển thị tất cả hình ảnh
        /// </summary>
        public FrmStockInOutImageLookup()
        {
            InitializeComponent();
            InitializeBll();
            InitializeWinExplorerView();
            InitializeDateFilters();
            InitializeEvents();
        }

        /// <summary>
        /// Constructor với StockInOutMasterId - chỉ hiển thị hình ảnh của phiếu nhập/xuất cụ thể
        /// </summary>
        /// <param name="stockInOutMasterId">ID phiếu nhập/xuất kho</param>
        public FrmStockInOutImageLookup(Guid stockInOutMasterId) : this()
        {
            _stockInOutMasterId = stockInOutMasterId;
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
                _stockInOutImageBll = new StockInOutImageBll();
            }
            catch (Exception ex)
            {
                MsgBox.ShowError($"Lỗi khởi tạo dịch vụ hình ảnh: {ex.Message}");
                _stockInOutImageBll = null;
            }
        }

        /// <summary>
        /// Khởi tạo cấu hình WinExplorerView
        /// </summary>
        private void InitializeWinExplorerView()
        {
            // Cấu hình ColumnSet
            winExplorerView1.ColumnSet.TextColumn = colFileName;
            winExplorerView1.ColumnSet.DescriptionColumn = colFileSizeDisplay;
            winExplorerView1.ColumnSet.ExtraLargeImageColumn = colImageData;
            winExplorerView1.ColumnSet.LargeImageColumn = colImageData;
            winExplorerView1.ColumnSet.MediumImageColumn = colImageData;
            winExplorerView1.ColumnSet.SmallImageColumn = colImageData;

            // Đảm bảo async loading được bật để load nhiều thumbnails cùng lúc
            winExplorerView1.OptionsImageLoad.AsyncLoad = true;
            winExplorerView1.OptionsImageLoad.CacheThumbnails = true;

            // Đăng ký event để xử lý thumbnail
            winExplorerView1.GetThumbnailImage += WinExplorerView1_GetThumbnailImage;
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
                System.Diagnostics.Debug.WriteLine($"Error initializing date filters: {ex.Message}");
            }
        }

        /// <summary>
        /// Khởi tạo events
        /// </summary>
        private void InitializeEvents()
        {
            // Bar button events
            XemBaoCaoBarButtonItem.ItemClick += XemBaoCaoBarButtonItem_ItemClick;
            XoaPhieuBarButtonItem.ItemClick += XoaPhieuBarButtonItem_ItemClick;
            XuatFileBarButtonItem.ItemClick += XuatFileBarButtonItem_ItemClick;

            // GridView events
            winExplorerView1.SelectionChanged += WinExplorerView1_SelectionChanged;

            // Form events
            Load += FrmStockInOutImageLookup_Load;
        }

        #endregion

        #region ========== SỰ KIỆN FORM ==========

        /// <summary>
        /// Event handler khi form được load
        /// </summary>
        private async void FrmStockInOutImageLookup_Load(object sender, EventArgs e)
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
        /// Event handler cho nút Xóa
        /// </summary>
        private async void XoaPhieuBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                var selectedImages = GetSelectedImages();
                if (selectedImages == null || selectedImages.Count == 0)
                {
                    MsgBox.ShowWarning("Vui lòng chọn ít nhất một hình ảnh để xóa.");
                    return;
                }

                var result = MsgBox.ShowYesNo(
                    $"Bạn có chắc chắn muốn xóa {selectedImages.Count} hình ảnh đã chọn?",
                    "Xác nhận xóa",
                    this);

                if (result)
                {
                    await DeleteSelectedImagesAsync(selectedImages);
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khi xóa hình ảnh");
            }
        }

        /// <summary>
        /// Event handler cho nút Xuất file
        /// </summary>
        private void XuatFileBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                var selectedImages = GetSelectedImages();
                if (selectedImages == null || selectedImages.Count == 0)
                {
                    MsgBox.ShowWarning("Vui lòng chọn ít nhất một hình ảnh để xuất.");
                    return;
                }

                // TODO: Implement export functionality
                MsgBox.ShowWarning("Chức năng xuất file đang được phát triển.");
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khi xuất file");
            }
        }

        /// <summary>
        /// Event handler khi selection thay đổi
        /// </summary>
        private void WinExplorerView1_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
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
        /// Xử lý event GetThumbnailImage để convert ImageData (byte[]) thành Image
        /// Load từ storage (NAS/Local) nếu ImageData chưa có trong DTO
        /// Logic tương tự FrmStockInOutAddImages để đảm bảo nhất quán
        /// </summary>
        private async void WinExplorerView1_GetThumbnailImage(object sender, ThumbnailImageEventArgs e)
        {
            try
            {
                // Sử dụng DataSourceIndex để lấy dữ liệu từ DataSource
                if (_dataSource == null || e.DataSourceIndex < 0 || e.DataSourceIndex >= _dataSource.Count)
                {
                    System.Diagnostics.Debug.WriteLine($"GetThumbnailImage: Invalid DataSourceIndex={e.DataSourceIndex}, Count={_dataSource?.Count ?? 0}");
                    return;
                }

                var dto = _dataSource[e.DataSourceIndex];
                if (dto == null)
                {
                    System.Diagnostics.Debug.WriteLine($"GetThumbnailImage: DTO is null at index {e.DataSourceIndex}");
                    return;
                }

                System.Diagnostics.Debug.WriteLine($"GetThumbnailImage: Loading thumbnail for index {e.DataSourceIndex}, ImageId={dto.Id}, FileName={dto.FileName}");

                byte[] imageData = dto.ImageData;

                // Nếu ImageData chưa có, load từ storage (NAS/Local)
                // Logic tương tự SaveImageFromFileAsync:
                // 1. Query database để lấy metadata (RelativePath) - đã có trong DTO từ MapEntitiesToDtos
                // 2. Sử dụng RelativePath để load trực tiếp từ storage (NAS/Local) thông qua ImageStorageService
                if ((imageData == null || imageData.Length == 0) &&
                    _stockInOutImageBll != null &&
                    !string.IsNullOrEmpty(dto.RelativePath))
                {
                    try
                    {
                        System.Diagnostics.Debug.WriteLine($"GetThumbnailImage: Loading image from storage for index {e.DataSourceIndex}, RelativePath={dto.RelativePath}");
                        
                        // Tối ưu: Sử dụng RelativePath trực tiếp từ DTO (đã query từ database trong LoadDataAsync)
                        // Thay vì query database lại trong GetImageDataAsync, sử dụng GetImageDataByRelativePathAsync
                        // Logic tương tự SaveImageFromFileAsync: sử dụng ImageStorageService để đọc file từ storage
                        imageData = await _stockInOutImageBll.GetImageDataByRelativePathAsync(dto.RelativePath);
                        
                        // Cache lại vào DTO để không phải load lại lần sau
                        if (imageData != null && imageData.Length > 0)
                        {
                            dto.ImageData = imageData;
                            System.Diagnostics.Debug.WriteLine($"GetThumbnailImage: Successfully loaded image data, Size={imageData.Length} bytes");
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine($"GetThumbnailImage: Image data is null or empty after loading from RelativePath={dto.RelativePath}");
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log lỗi nhưng không throw để không làm gián đoạn hiển thị
                        System.Diagnostics.Debug.WriteLine($"Lỗi load image từ storage (NAS/Local): {ex.Message}");
                        System.Diagnostics.Debug.WriteLine($"  - StackTrace: {ex.StackTrace}");
                        // Có thể log chi tiết hơn nếu cần
                        if (dto != null)
                        {
                            System.Diagnostics.Debug.WriteLine($"  - ImageId: {dto.Id}, RelativePath: {dto.RelativePath}");
                        }
                    }
                }
                else if (imageData == null || imageData.Length == 0)
                {
                    System.Diagnostics.Debug.WriteLine($"GetThumbnailImage: Skipping load - BLL={_stockInOutImageBll != null}, RelativePath={!string.IsNullOrEmpty(dto.RelativePath)}");
                }

                // Convert byte[] thành Image và tạo thumbnail
                if (imageData != null && imageData.Length > 0)
                {
                    using (var ms = new MemoryStream(imageData))
                    {
                        var image = Image.FromStream(ms);
                        // Sử dụng CreateThumbnailImage để tạo thumbnail với kích thước mong muốn
                        e.ThumbnailImage = e.CreateThumbnailImage(image, e.DesiredThumbnailSize);
                        System.Diagnostics.Debug.WriteLine($"GetThumbnailImage: Successfully created thumbnail for index {e.DataSourceIndex}, FileName={dto.FileName}");
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"GetThumbnailImage: No image data for index {e.DataSourceIndex}, FileName={dto.FileName}, RelativePath={dto.RelativePath}");
                }
            }
            catch (Exception ex)
            {
                // Log error để debug nhưng không throw để không làm gián đoạn hiển thị
                System.Diagnostics.Debug.WriteLine($"Lỗi load thumbnail: {ex.Message}");
            }
        }

        #endregion

        #region ========== LOAD DỮ LIỆU ==========

        /// <summary>
        /// Load dữ liệu hình ảnh từ database
        /// </summary>
        public async Task LoadDataAsync()
        {
            try
            {
                if (_stockInOutImageBll == null)
                {
                    MsgBox.ShowWarning("Dịch vụ hình ảnh chưa được khởi tạo.");
                    LoadData(new List<StockInOutImageDto>());
                    return;
                }

                await ExecuteWithWaitingFormAsync(async () =>
                {
                    // Lấy filter criteria
                    var keyword = KeyWordBarEditItem.EditValue?.ToString();
                    if (string.IsNullOrWhiteSpace(keyword))
                    {
                        keyword = null;
                    }

                    var fromDate = TuNgayBarEditItem.EditValue as DateTime? ?? 
                                   new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    var toDate = DenNgayBarEditItem.EditValue as DateTime? ?? DateTime.Now;

                    // Validate date range
                    if (fromDate > toDate)
                    {
                        MsgBox.ShowWarning("Từ ngày không được lớn hơn đến ngày.");
                        LoadData(new List<StockInOutImageDto>());
                        return;
                    }

                    List<StockInOutImage> entities;

                    if (_stockInOutMasterId.HasValue)
                    {
                        // Load hình ảnh theo StockInOutMasterId
                        // Khi có StockInOutMasterId, load TẤT CẢ hình ảnh của phiếu đó
                        // KHÔNG filter theo date vì mục đích là xem tất cả hình ảnh của phiếu
                        entities = _stockInOutImageBll.GetByStockInOutMasterId(_stockInOutMasterId.Value);

                        // Chỉ filter theo keyword nếu có (không filter theo date)
                        if (!string.IsNullOrWhiteSpace(keyword))
                        {
                            var keywordLower = keyword.Trim().ToLower();
                            entities = entities.Where(e =>
                                (!string.IsNullOrEmpty(e.FileName) && e.FileName.ToLower().Contains(keywordLower)) ||
                                (!string.IsNullOrEmpty(e.RelativePath) && e.RelativePath.ToLower().Contains(keywordLower)) ||
                                (!string.IsNullOrEmpty(e.FullPath) && e.FullPath.ToLower().Contains(keywordLower))
                            ).ToList();
                        }
                    }
                    else
                    {
                        // Query hình ảnh theo khoảng thời gian và từ khóa
                        // Khi không có StockInOutMasterId, filter theo date range và keyword
                        entities = _stockInOutImageBll.QueryImages(
                            fromDate.Date, 
                            toDate.Date.AddDays(1).AddTicks(-1), 
                            keyword);
                    }

                    // Log số lượng entities trước khi map
                    System.Diagnostics.Debug.WriteLine($"LoadDataAsync: Load được {entities?.Count ?? 0} hình ảnh từ database");

                    var dtos = MapEntitiesToDtos(entities);
                    
                    // Filter bỏ các records không hợp lệ (không có RelativePath hoặc FileName)
                    // Vì không thể load thumbnail nếu không có RelativePath
                    var validDtos = dtos.Where(dto => 
                        !string.IsNullOrWhiteSpace(dto.RelativePath) && 
                        !string.IsNullOrWhiteSpace(dto.FileName)
                    ).ToList();
                    
                    var invalidCount = dtos.Count - validDtos.Count;
                    if (invalidCount > 0)
                    {
                        System.Diagnostics.Debug.WriteLine($"LoadDataAsync: Filter bỏ {invalidCount} hình ảnh không hợp lệ (không có RelativePath hoặc FileName)");
                    }
                    
                    // Log số lượng DTOs sau khi filter
                    System.Diagnostics.Debug.WriteLine($"LoadDataAsync: Còn lại {validDtos.Count} hình ảnh hợp lệ sau khi filter");
                    
                    LoadData(validDtos);
                });
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khi tải dữ liệu");
                LoadData(new List<StockInOutImageDto>());
            }
        }

        /// <summary>
        /// Load dữ liệu hình ảnh
        /// </summary>
        public void LoadData(List<StockInOutImageDto> images)
        {
            if (images == null)
            {
                _dataSource = null;
                stockInOutImageDtoBindingSource.DataSource = null;
                stockInOutImageDtoBindingSource.ResetBindings(false);
                winExplorerView1.RefreshData();
                UpdateStatusBar();
                return;
            }

            // Lưu reference để sử dụng trong GetThumbnailImage event
            _dataSource = images;
            stockInOutImageDtoBindingSource.DataSource = images;
            stockInOutImageDtoBindingSource.ResetBindings(false);
            
            // Log chi tiết về các hình ảnh hợp lệ
            var validImages = images.Where(img => !string.IsNullOrWhiteSpace(img.RelativePath) && !string.IsNullOrWhiteSpace(img.FileName)).ToList();
            System.Diagnostics.Debug.WriteLine($"LoadData: Tổng số hình ảnh: {images.Count}, Hợp lệ: {validImages.Count}");
            if (validImages.Count < images.Count)
            {
                var invalidImages = images.Where(img => string.IsNullOrWhiteSpace(img.RelativePath) || string.IsNullOrWhiteSpace(img.FileName)).ToList();
                System.Diagnostics.Debug.WriteLine($"LoadData: Có {invalidImages.Count} hình ảnh không hợp lệ (không có RelativePath hoặc FileName)");
                foreach (var invalid in invalidImages.Take(5))
                {
                    System.Diagnostics.Debug.WriteLine($"  - ImageId: {invalid.Id}, FileName: '{invalid.FileName}', RelativePath: '{invalid.RelativePath}'");
                }
            }
            
            // Force refresh WinExplorerView để trigger GetThumbnailImage event cho tất cả items
            winExplorerView1.RefreshData();
            
            // Preload tất cả thumbnails trong background để đảm bảo tất cả hình ảnh được load
            // WinExplorerView chỉ load thumbnail cho items visible, nên cần preload để đảm bảo tất cả đều có thumbnail
            PreloadAllThumbnailsAsync(validImages);
            
            // Sử dụng BeginInvoke để đảm bảo UI được update sau khi data được set
            BeginInvoke(new Action(() =>
            {
                winExplorerView1.LayoutChanged();
                winExplorerView1.RefreshData();
            }));
            
            UpdateStatusBar();
            
            // Log để debug
            System.Diagnostics.Debug.WriteLine($"LoadData: Đã load {images.Count} hình ảnh vào grid");
        }

        /// <summary>
        /// Map entities sang DTOs
        /// ImageData sẽ được load lazy từ storage (NAS/Local) trong GetThumbnailImage event
        /// để tối ưu hiệu suất và tránh load tất cả ảnh cùng lúc
        /// </summary>
        private List<StockInOutImageDto> MapEntitiesToDtos(List<StockInOutImage> entities)
        {
            if (entities == null)
                return new List<StockInOutImageDto>();

            return entities.Select(entity => new StockInOutImageDto
            {
                Id = entity.Id,
                StockInOutMasterId = entity.StockInOutMasterId,
                // ImageData sẽ được load từ storage (NAS/Local) trong GetThumbnailImage event nếu cần
                // Sử dụng lazy loading để tối ưu hiệu suất
                ImageData = null,
                FileName = entity.FileName,
                RelativePath = entity.RelativePath,
                FullPath = entity.FullPath,
                StorageType = entity.StorageType,
                FileSize = entity.FileSize,
                FileExtension = entity.FileExtension,
                MimeType = entity.MimeType,
                Checksum = entity.Checksum,
                FileExists = entity.FileExists,
                LastVerified = entity.LastVerified,
                MigrationStatus = entity.MigrationStatus,
                CreateDate = entity.CreateDate,
                CreateBy = entity.CreateBy,
                ModifiedDate = entity.ModifiedDate,
                ModifiedBy = entity.ModifiedBy
            }).ToList();
        }

        /// <summary>
        /// Lấy danh sách hình ảnh được chọn
        /// </summary>
        public List<StockInOutImageDto> GetSelectedImages()
        {
            var selectedImages = new List<StockInOutImageDto>();
            var selectedRows = winExplorerView1.GetSelectedRows();

            foreach (int rowHandle in selectedRows)
            {
                var dto = winExplorerView1.GetRow(rowHandle) as StockInOutImageDto;
                if (dto != null)
                {
                    selectedImages.Add(dto);
                }
            }

            return selectedImages;
        }

        /// <summary>
        /// Cập nhật status bar
        /// </summary>
        private void UpdateStatusBar()
        {
            try
            {
                var totalCount = _dataSource?.Count ?? 0;
                var selectedCount = winExplorerView1.SelectedRowsCount;

                DataSummaryBarStaticItem.Caption = $"Tổng số: <b>{totalCount}</b> hình ảnh";
                SelectedRowBarStaticItem.Caption = selectedCount > 0
                    ? $"Đã chọn: <b>{selectedCount}</b> hình ảnh"
                    : "Chưa chọn dòng nào";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating status bar: {ex.Message}");
            }
        }

        /// <summary>
        /// Xóa các hình ảnh đã chọn
        /// </summary>
        private async Task DeleteSelectedImagesAsync(List<StockInOutImageDto> selectedImages)
        {
            try
            {
                if (_stockInOutImageBll == null)
                {
                    ShowError("Dịch vụ hình ảnh chưa được khởi tạo.");
                    return;
                }

                await ExecuteWithWaitingFormAsync(async () =>
                {
                    var successCount = 0;
                    var errorCount = 0;
                    var errorMessages = new List<string>();

                    foreach (var dto in selectedImages)
                    {
                        try
                        {
                            await _stockInOutImageBll.DeleteImageAsync(dto.Id);
                            successCount++;
                        }
                        catch (Exception ex)
                        {
                            errorCount++;
                            errorMessages.Add($"{dto.FileName}: {ex.Message}");
                        }
                    }

                    // Hiển thị kết quả
                    var message = $"Kết quả xóa hình ảnh:\n\n";
                    message += $"✅ Thành công: {successCount} hình ảnh\n";
                    message += $"❌ Lỗi: {errorCount} hình ảnh\n";

                    if (errorCount > 0 && errorMessages.Any())
                    {
                        message += "\nChi tiết lỗi:\n";
                        foreach (var error in errorMessages.Take(5))
                        {
                            message += $"• {error}\n";
                        }
                    }

                    if (successCount > 0)
                    {
                        MsgBox.ShowSuccess(message);
                        // Reload data
                        await LoadDataAsync();
                    }
                    else
                    {
                        MsgBox.ShowError(message);
                    }
                });
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khi xóa hình ảnh");
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

        /// <summary>
        /// Preload tất cả thumbnails trong background để đảm bảo tất cả hình ảnh được load
        /// WinExplorerView chỉ load thumbnail cho items visible, nên cần preload để đảm bảo tất cả đều có thumbnail
        /// </summary>
        private async void PreloadAllThumbnailsAsync(List<StockInOutImageDto> validImages)
        {
            if (validImages == null || validImages.Count == 0 || _stockInOutImageBll == null)
                return;

            try
            {
                System.Diagnostics.Debug.WriteLine($"PreloadAllThumbnailsAsync: Bắt đầu preload {validImages.Count} thumbnails");
                
                // Preload tất cả thumbnails trong background
                await System.Threading.Tasks.Task.Run(async () =>
                {
                    var loadedCount = 0;
                    var errorCount = 0;
                    
                    foreach (var dto in validImages)
                    {
                        try
                        {
                            // Skip nếu đã có ImageData
                            if (dto.ImageData != null && dto.ImageData.Length > 0)
                            {
                                loadedCount++;
                                continue;
                            }

                            // Skip nếu không có RelativePath
                            if (string.IsNullOrWhiteSpace(dto.RelativePath))
                            {
                                continue;
                            }

                            // Load image data từ storage
                            var imageData = await _stockInOutImageBll.GetImageDataByRelativePathAsync(dto.RelativePath);
                            
                            if (imageData != null && imageData.Length > 0)
                            {
                                // Cache vào DTO trên UI thread
                                if (InvokeRequired)
                                {
                                    BeginInvoke(new Action(() =>
                                    {
                                        dto.ImageData = imageData;
                                    }));
                                }
                                else
                                {
                                    dto.ImageData = imageData;
                                }
                                
                                loadedCount++;
                            }
                            else
                            {
                                errorCount++;
                            }
                        }
                        catch (Exception ex)
                        {
                            errorCount++;
                            System.Diagnostics.Debug.WriteLine($"PreloadAllThumbnailsAsync: Lỗi load thumbnail cho {dto.FileName}: {ex.Message}");
                        }
                    }
                    
                    System.Diagnostics.Debug.WriteLine($"PreloadAllThumbnailsAsync: Hoàn thành - Loaded: {loadedCount}, Errors: {errorCount}");
                    
                    // Force refresh WinExplorerView sau khi preload xong để hiển thị tất cả thumbnails
                    // WinExplorerView cần được refresh để trigger GetThumbnailImage event cho tất cả items
                    // Cần refresh nhiều lần để đảm bảo tất cả items đều được render
                    if (InvokeRequired)
                    {
                        BeginInvoke(new Action(() =>
                        {
                            // Refresh ngay lập tức để trigger GetThumbnailImage cho các items visible
                            winExplorerView1.RefreshData();
                            winExplorerView1.LayoutChanged();
                            
                            System.Diagnostics.Debug.WriteLine($"PreloadAllThumbnailsAsync: Đã refresh WinExplorerView lần 1 sau khi preload {loadedCount} thumbnails");
                            
                            // Refresh lại sau một khoảng thời gian để đảm bảo tất cả thumbnails được hiển thị
                            // Đặc biệt là các items không visible ban đầu
                            System.Threading.Tasks.Task.Delay(300).ContinueWith(_ =>
                            {
                                if (InvokeRequired)
                                {
                                    BeginInvoke(new Action(() =>
                                    {
                                        winExplorerView1.RefreshData();
                                        winExplorerView1.LayoutChanged();
                                        
                                        // Force refresh lại một lần nữa để đảm bảo
                                        System.Threading.Tasks.Task.Delay(200).ContinueWith(__ =>
                                        {
                                            if (InvokeRequired)
                                            {
                                                BeginInvoke(new Action(() =>
                                                {
                                                    winExplorerView1.RefreshData();
                                                    System.Diagnostics.Debug.WriteLine($"PreloadAllThumbnailsAsync: Đã refresh WinExplorerView lần 3 sau khi preload");
                                                }));
                                            }
                                        });
                                        
                                        System.Diagnostics.Debug.WriteLine($"PreloadAllThumbnailsAsync: Đã refresh WinExplorerView lần 2 sau khi preload");
                                    }));
                                }
                            });
                        }));
                    }
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"PreloadAllThumbnailsAsync: Lỗi: {ex.Message}");
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

        /// <summary>
        /// Hiển thị lỗi với thông báo
        /// </summary>
        private void ShowError(string message)
        {
            MsgBox.ShowError(message);
        }

        #endregion
    }
}