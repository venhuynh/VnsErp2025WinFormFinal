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
                    return;

                var dto = _dataSource[e.DataSourceIndex];
                if (dto == null)
                    return;

                byte[] imageData = dto.ImageData;

                // Nếu ImageData chưa có, load từ storage (NAS/Local)
                // Kiểm tra BLL đã được khởi tạo và có RelativePath
                if ((imageData == null || imageData.Length == 0) &&
                    _stockInOutImageBll != null &&
                    !string.IsNullOrEmpty(dto.RelativePath))
                {
                    try
                    {
                        // Load từ storage thông qua BLL
                        // BLL sẽ:
                        // 1. Lấy metadata từ database (RelativePath)
                        // 2. Sử dụng ImageStorageService để đọc file từ NAS/Local storage
                        imageData = await _stockInOutImageBll.GetImageDataAsync(dto.Id);
                        
                        // Cache lại vào DTO để không phải load lại lần sau
                        if (imageData != null && imageData.Length > 0)
                        {
                            dto.ImageData = imageData;
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log lỗi nhưng không throw để không làm gián đoạn hiển thị
                        System.Diagnostics.Debug.WriteLine($"Lỗi load image từ storage (NAS/Local): {ex.Message}");
                        // Có thể log chi tiết hơn nếu cần
                        if (dto != null)
                        {
                            System.Diagnostics.Debug.WriteLine($"  - ImageId: {dto.Id}, RelativePath: {dto.RelativePath}");
                        }
                    }
                }

                // Convert byte[] thành Image và tạo thumbnail
                if (imageData != null && imageData.Length > 0)
                {
                    using (var ms = new MemoryStream(imageData))
                    {
                        var image = Image.FromStream(ms);
                        // Sử dụng CreateThumbnailImage để tạo thumbnail với kích thước mong muốn
                        e.ThumbnailImage = e.CreateThumbnailImage(image, e.DesiredThumbnailSize);
                    }
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
                        entities = _stockInOutImageBll.GetByStockInOutMasterId(_stockInOutMasterId.Value);

                        // Filter theo keyword và date nếu có (vì GetByStockInOutMasterId không hỗ trợ filter)
                        if (!string.IsNullOrWhiteSpace(keyword))
                        {
                            var keywordLower = keyword.Trim().ToLower();
                            entities = entities.Where(e =>
                                (!string.IsNullOrEmpty(e.FileName) && e.FileName.ToLower().Contains(keywordLower)) ||
                                (!string.IsNullOrEmpty(e.RelativePath) && e.RelativePath.ToLower().Contains(keywordLower))
                            ).ToList();
                        }

                        // Filter theo date
                        entities = entities.Where(e => 
                            e.CreateDate >= fromDate.Date && 
                            e.CreateDate <= toDate.Date.AddDays(1).AddTicks(-1)
                        ).ToList();
                    }
                    else
                    {
                        // Query hình ảnh theo khoảng thời gian và từ khóa
                        entities = _stockInOutImageBll.QueryImages(
                            fromDate.Date, 
                            toDate.Date.AddDays(1).AddTicks(-1), 
                            keyword);
                    }

                    var dtos = MapEntitiesToDtos(entities);
                    LoadData(dtos);
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
                WarrantyCheckListDtoGridControl.DataSource = null;
                return;
            }

            // Lưu reference để sử dụng trong GetThumbnailImage event
            _dataSource = images;
            stockInOutImageDtoBindingSource.DataSource = images;
            winExplorerView1.RefreshData();
            UpdateStatusBar();
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