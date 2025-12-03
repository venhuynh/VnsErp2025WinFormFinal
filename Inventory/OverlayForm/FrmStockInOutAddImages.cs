using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bll.Inventory.InventoryManagement;
using Common.Common;
using Common.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.WinExplorer;
using DevExpress.Utils;
using DTO.Inventory.Query;
using Dal.DataContext;

namespace Inventory.OverlayForm;

/// <summary>
/// Form thêm hình ảnh cho phiếu nhập/xuất kho.
/// Cung cấp chức năng chọn và upload nhiều hình ảnh cho phiếu nhập/xuất kho.
/// </summary>
public partial class FrmStockInOutAddImages : XtraForm
{
    #region ========== KHAI BÁO BIẾN ==========

    /// <summary>
    /// Business Logic Layer cho hình ảnh nhập/xuất kho
    /// </summary>
    private StockInOutImageBll _stockInOutImageBll;

    /// <summary>
    /// ID phiếu nhập/xuất kho
    /// </summary>
    private Guid StockInOutMasterId { get; set; }

    /// <summary>
    /// Danh sách hình ảnh hiện tại
    /// </summary>
    private List<StockInOutImageDto> _dataSource;

    #endregion

    #region ========== CONSTRUCTOR ==========

    /// <summary>
    /// Constructor với StockInOutMasterId
    /// </summary>
    /// <param name="stockInOutMasterId">ID phiếu nhập/xuất kho</param>
    public FrmStockInOutAddImages(Guid stockInOutMasterId)
    {
        InitializeComponent();
        StockInOutMasterId = stockInOutMasterId;
        InitializeBll();
        InitializeWinExplorerView();
        InitializeDateFilters();
        InitializeEvents();
        SetupSuperToolTips();
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
            _stockInOutImageBll = null; // Set null để disable các chức năng upload
            DisableUploadControls(); // Disable các control liên quan
        }
        catch (Exception ex)
        {
            var errorMessage = "Lỗi khởi tạo dịch vụ lưu trữ hình ảnh: " + ex.Message;
            Common.Utils.MsgBox.ShowError(errorMessage, "Lỗi", this);
            _stockInOutImageBll = null; // Set null để disable các chức năng upload
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
            if (OpenSelectImageHyperlinkLabelControl != null)
            {
                OpenSelectImageHyperlinkLabelControl.Enabled = false;
                OpenSelectImageHyperlinkLabelControl.Text = "Chức năng upload hình ảnh đã bị vô hiệu hóa do thiếu cấu hình";
            }
        }
        catch (Exception ex)
        {
            // Log lỗi nhưng không throw để form vẫn có thể mở được
            System.Diagnostics.Debug.WriteLine($"Error disabling upload controls: {ex.Message}");
        }
    }

    /// <summary>
    /// Khởi tạo cấu hình WinExplorerView
    /// </summary>
    private void InitializeWinExplorerView()
    {
        // Cấu hình ColumnSet
        winExplorerView.ColumnSet.TextColumn = colFileName;
        winExplorerView.ColumnSet.DescriptionColumn = colFileSizeDisplay;
        winExplorerView.ColumnSet.ExtraLargeImageColumn = colImageData;
        winExplorerView.ColumnSet.LargeImageColumn = colImageData;
        winExplorerView.ColumnSet.MediumImageColumn = colImageData;
        winExplorerView.ColumnSet.SmallImageColumn = colImageData;

        // Đăng ký event để xử lý thumbnail
        winExplorerView.GetThumbnailImage += WinExplorerView_GetThumbnailImage;
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
        // Event cho nút chọn hình ảnh
        OpenSelectImageHyperlinkLabelControl.Click += OpenSelectImageHyperlinkLabelControl_Click;

        // Bar button events
        XemBarButtonItem.ItemClick += XemBarButtonItem_ItemClick;
        XoaBarButtonItem.ItemClick += XoaBarButtonItem_ItemClick;

        // GridView events
        winExplorerView.SelectionChanged += WinExplorerView_SelectionChanged;

        // Form events
        Load += FrmStockInOutAddImages_Load;
    }

    #endregion

    #region ========== SỰ KIỆN FORM ==========

    /// <summary>
    /// Event handler khi form được load
    /// </summary>
    private async void FrmStockInOutAddImages_Load(object sender, EventArgs e)
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
    /// Event handler cho nút Xóa
    /// </summary>
    private async void XoaBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
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
    /// Event handler khi selection thay đổi
    /// </summary>
    private void WinExplorerView_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
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
    /// Load từ storage nếu ImageData chưa có trong DTO
    /// </summary>
    private async void WinExplorerView_GetThumbnailImage(object sender, ThumbnailImageEventArgs e)
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

            // Nếu ImageData chưa có, load từ storage
            if ((imageData == null || imageData.Length == 0) &&
                _stockInOutImageBll != null &&
                !string.IsNullOrEmpty(dto.RelativePath))
            {
                try
                {
                    imageData = await _stockInOutImageBll.GetImageDataAsync(dto.Id);
                    // Cache lại vào DTO để không phải load lại lần sau
                    if (imageData != null)
                    {
                        dto.ImageData = imageData;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Lỗi load image từ storage: {ex.Message}");
                }
            }

            // Convert byte[] thành Image
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
            // Log error nếu cần
            System.Diagnostics.Debug.WriteLine($"Lỗi load thumbnail: {ex.Message}");
        }
    }

    /// <summary>
    /// Xử lý sự kiện click nút chọn hình ảnh
    /// </summary>
    private async void OpenSelectImageHyperlinkLabelControl_Click(object sender, EventArgs e)
    {
        try
        {
            // Kiểm tra BLL đã được khởi tạo chưa
            if (_stockInOutImageBll == null)
            {
                ShowError("Dịch vụ lưu trữ hình ảnh chưa được cấu hình. " +
                          "Vui lòng kiểm tra lại cấu hình trong App.config và khởi động lại ứng dụng.");
                return;
            }

            // Kiểm tra StockInOutMasterId hợp lệ
            if (StockInOutMasterId == Guid.Empty)
            {
                ShowError("ID phiếu nhập/xuất kho không hợp lệ.");
                return;
            }

            // Cấu hình OpenFileDialog để chọn nhiều hình ảnh
            xtraOpenFileDialog1.Filter = @"Hình ảnh|*.jpg;*.jpeg;*.png;*.gif;*.bmp|Tất cả files|*.*";
            xtraOpenFileDialog1.Multiselect = true;
            xtraOpenFileDialog1.Title = @"Chọn hình ảnh cho phiếu nhập/xuất kho";

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
                var keyword = KeyWordBarEditItem.EditValue?.ToString() ?? string.Empty;
                var fromDate = TuNgayBarEditItem.EditValue as DateTime?;
                var toDate = DenNgayBarEditItem.EditValue as DateTime?;

                // Load hình ảnh theo StockInOutMasterId
                var entities = _stockInOutImageBll.GetByStockInOutMasterId(StockInOutMasterId);
                
                // Filter theo keyword và date nếu có
                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    var keywordLower = keyword.ToLower();
                    entities = entities.Where(e => 
                        (!string.IsNullOrEmpty(e.FileName) && e.FileName.ToLower().Contains(keywordLower)) ||
                        (!string.IsNullOrEmpty(e.RelativePath) && e.RelativePath.ToLower().Contains(keywordLower))
                    ).ToList();
                }

                if (fromDate.HasValue)
                {
                    entities = entities.Where(e => e.CreateDate >= fromDate.Value).ToList();
                }

                if (toDate.HasValue)
                {
                    var toDateEnd = toDate.Value.Date.AddDays(1).AddTicks(-1);
                    entities = entities.Where(e => e.CreateDate <= toDateEnd).ToList();
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
            StockInOutImageGridControl.DataSource = null;
            return;
        }

        // Lưu reference để sử dụng trong GetThumbnailImage event
        _dataSource = images;
        stockInOutImageDtoBindingSource.DataSource = images;
        winExplorerView.RefreshData();
        UpdateStatusBar();
    }

    /// <summary>
    /// Map entities sang DTOs
    /// </summary>
    private List<StockInOutImageDto> MapEntitiesToDtos(List<StockInOutImage> entities)
    {
        if (entities == null)
            return new List<StockInOutImageDto>();

        return entities.Select(entity => new StockInOutImageDto
        {
            Id = entity.Id,
            StockInOutMasterId = entity.StockInOutMasterId,
            // ImageData sẽ được load từ storage trong GetThumbnailImage event nếu cần
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
        var selectedRows = winExplorerView.GetSelectedRows();

        foreach (int rowHandle in selectedRows)
        {
            var dto = winExplorerView.GetRow(rowHandle) as StockInOutImageDto;
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
            var selectedCount = winExplorerView.SelectedRowsCount;

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

    #endregion

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
                var success = await SaveImageFromFileAsync(StockInOutMasterId, imagePath);
                    
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
            
        // Đóng màn hình nếu có ít nhất một hình ảnh thành công
        if (successCount > 0)
        {
            Close();
        }
    }

    /// <summary>
    /// Lưu hình ảnh từ file vào NAS/Local storage và metadata vào database
    /// </summary>
    /// <param name="stockInOutMasterId">ID phiếu nhập/xuất kho</param>
    /// <param name="imageFilePath">Đường dẫn file ảnh</param>
    /// <returns>True nếu lưu thành công</returns>
    private async Task<bool> SaveImageFromFileAsync(Guid stockInOutMasterId, string imageFilePath)
    {
        try
        {
            // Kiểm tra BLL đã được khởi tạo chưa
            if (_stockInOutImageBll == null)
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
            var stockInOutImage = await _stockInOutImageBll.SaveImageFromFileAsync(stockInOutMasterId, imageFilePath);

            // Kiểm tra kết quả
            if (stockInOutImage == null)
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

    /// <summary>
    /// Thực hiện operation async với WaitingForm hiển thị
    /// </summary>
    /// <param name="operation">Operation async cần thực hiện</param>
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
    /// Thiết lập SuperToolTip cho các controls trong form
    /// </summary>
    private void SetupSuperToolTips()
    {
        try
        {
            if (OpenSelectImageHyperlinkLabelControl != null)
            {
                var superTip = SuperToolTipHelper.CreateSuperToolTip(
                    title: "<b><color=Green>🖼️ Chọn hình ảnh</color></b>",
                    content: @"Chọn một hoặc nhiều hình ảnh để thêm vào phiếu nhập/xuất kho."
                );
                OpenSelectImageHyperlinkLabelControl.SuperTip = superTip;
            }
        }
        catch (Exception ex)
        {
            // Ignore lỗi setup SuperToolTip để không chặn form
            System.Diagnostics.Debug.WriteLine($"Lỗi setup SuperToolTip: {ex.Message}");
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

    #endregion
}