using Bll.Inventory.InventoryManagement;
using Common.Common;
using Common.Utils;
using DevExpress.XtraEditors;
using DTO.Inventory.InventoryManagement;
using DTO.Inventory.StockIn;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventory.Query
{
    /// <summary>
    /// Form hiển thị danh sách chứng từ nhập/xuất kho sử dụng GridView
    /// </summary>
    public partial class FrmStockInOutDocumentDtoLookup : XtraForm
    {
        #region ========== KHAI BÁO BIẾN ==========

        private Guid? _stockInOutMasterId;
        private List<StockInOutDocumentDto> _dataSource;
        private StockInOutDocumentBll _stockInOutDocumentBll;

        #endregion

        #region ========== CONSTRUCTOR ==========

        /// <summary>
        /// Constructor mặc định - hiển thị tất cả chứng từ
        /// </summary>
        public FrmStockInOutDocumentDtoLookup()
        {
            InitializeComponent();
            InitializeBll();
            InitializeGridView();
            InitializeDateFilters();
            InitializeEvents();
        }

        /// <summary>
        /// Constructor với StockInOutMasterId - chỉ hiển thị chứng từ của phiếu nhập/xuất cụ thể
        /// </summary>
        /// <param name="stockInOutMasterId">ID phiếu nhập/xuất kho</param>
        public FrmStockInOutDocumentDtoLookup(Guid stockInOutMasterId) : this()
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
                _stockInOutDocumentBll = new StockInOutDocumentBll();
            }
            catch (Exception ex)
            {
                MsgBox.ShowError($"Lỗi khởi tạo dịch vụ chứng từ: {ex.Message}");
                _stockInOutDocumentBll = null;
            }
        }

        /// <summary>
        /// Khởi tạo cấu hình GridView
        /// </summary>
        private void InitializeGridView()
        {
            // Cấu hình group theo GroupCaption
            StockInOutDocumentDtoGridView.SortInfo.Clear();
            StockInOutDocumentDtoGridView.SortInfo.Add(
                new DevExpress.XtraGrid.Columns.GridColumnSortInfo(colGroupCaption, DevExpress.Data.ColumnSortOrder.Ascending));
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
            OpenFileBarButtonItem.ItemClick += XuatFileBarButtonItem_ItemClick;

            // GridView events
            StockInOutDocumentDtoGridView.SelectionChanged += StockInOutDocumentDtoGridView_SelectionChanged;
            StockInOutDocumentDtoGridView.DoubleClick += StockInOutDocumentDtoGridView_DoubleClick;

            // Form events
            Load += FrmStockInOutDocumentDtoLookup_Load;
        }

        #endregion

        #region ========== SỰ KIỆN FORM ==========

        /// <summary>
        /// Event handler khi form được load
        /// </summary>
        private void FrmStockInOutDocumentDtoLookup_Load(object sender, EventArgs e)
        {
            // Không tự động load, user phải click "Xem" để load
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
                var selectedDocuments = GetSelectedDocuments();
                if (selectedDocuments == null || selectedDocuments.Count == 0)
                {
                    MsgBox.ShowWarning("Vui lòng chọn ít nhất một chứng từ để xóa.");
                    return;
                }

                var result = MsgBox.ShowYesNo(
                    $"Bạn có chắc chắn muốn xóa {selectedDocuments.Count} chứng từ đã chọn?",
                    "Xác nhận xóa",
                    this);

                if (result)
                {
                    await DeleteSelectedDocumentsAsync(selectedDocuments);
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khi xóa chứng từ");
            }
        }

        /// <summary>
        /// Event handler cho nút Mở file - Tải về thư mục Download và mở file
        /// </summary>
        private async void XuatFileBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                var selectedDocuments = GetSelectedDocuments();
                if (selectedDocuments == null || selectedDocuments.Count == 0)
                {
                    MsgBox.ShowWarning("Vui lòng chọn ít nhất một chứng từ để mở.");
                    return;
                }

                // Chỉ mở file đầu tiên được chọn
                var documentToOpen = selectedDocuments.First();
                await DownloadAndOpenDocumentAsync(documentToOpen);
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khi mở file");
            }
        }

        /// <summary>
        /// Event handler khi selection thay đổi
        /// </summary>
        private void StockInOutDocumentDtoGridView_SelectionChanged(object sender, EventArgs e)
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
        /// Event handler khi double click trên GridView
        /// </summary>
        private async void StockInOutDocumentDtoGridView_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                var focusedRowHandle = StockInOutDocumentDtoGridView.FocusedRowHandle;
                if (focusedRowHandle < 0) return;
                if (StockInOutDocumentDtoGridView.GetRow(focusedRowHandle) is StockInOutDocumentDto dto)
                {
                    // Tải xuống chứng từ được double click
                    await DownloadDocumentAsync(dto);
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khi mở chứng từ");
            }
        }

        #endregion

        #region ========== LOAD DỮ LIỆU ==========

        /// <summary>
        /// Load dữ liệu chứng từ từ database
        /// </summary>
        private async Task LoadDataAsync()
        {
            try
            {
                if (_stockInOutDocumentBll == null)
                {
                    MsgBox.ShowWarning("Dịch vụ chứng từ chưa được khởi tạo.");
                    LoadData([]);
                    return;
                }

                await ExecuteWithWaitingFormAsync(() =>
                {
                    // Lấy filter criteria - chỉ theo ngày tháng
                    var fromDate = TuNgayBarEditItem.EditValue as DateTime? ?? 
                                   new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    var toDate = DenNgayBarEditItem.EditValue as DateTime? ?? DateTime.Now;

                    // Validate date range
                    if (fromDate > toDate)
                    {
                        MsgBox.ShowWarning("Từ ngày không được lớn hơn đến ngày.");
                        LoadData([]);
                        return Task.CompletedTask;
                    }

                    List<Dal.DataContext.StockInOutDocument> entities;

                    if (_stockInOutMasterId.HasValue)
                    {
                        // Load chứng từ theo StockInOutMasterId
                        entities = _stockInOutDocumentBll.GetByStockInOutMasterId(_stockInOutMasterId.Value);
                    }
                    else
                    {
                        // Query chứng từ theo khoảng thời gian
                        entities = _stockInOutDocumentBll.QueryDocuments(
                            fromDate: fromDate.Date, 
                            toDate: toDate.Date.AddDays(1).AddTicks(-1));
                    }

                    // Map entities sang DTOs
                    var dtos = MapEntitiesToDtos(entities);
                    
                    // Filter bỏ các records không hợp lệ (không có RelativePath hoặc FileName)
                    var validDtos = dtos.Where(dto => 
                        !string.IsNullOrWhiteSpace(dto.RelativePath) && 
                        !string.IsNullOrWhiteSpace(dto.FileName)
                    ).ToList();
                    
                    var invalidCount = dtos.Count - validDtos.Count;
                    if (invalidCount > 0)
                    {
                        System.Diagnostics.Debug.WriteLine($"LoadDataAsync: Filter bỏ {invalidCount} chứng từ không hợp lệ (không có RelativePath hoặc FileName)");
                    }
                    
                    // Set data source
                    LoadData(validDtos);
                    return Task.CompletedTask;
                });
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khi tải dữ liệu");
                LoadData([]);
            }
        }

        /// <summary>
        /// Load dữ liệu chứng từ
        /// </summary>
        private void LoadData(List<StockInOutDocumentDto> documents)
        {
            if (documents == null)
            {
                _dataSource = null;
                stockInOutDocumentDtoBindingSource.DataSource = null;
                stockInOutDocumentDtoBindingSource.ResetBindings(false);
                StockInOutDocumentDtoGridView.RefreshData();
                UpdateStatusBar();
                return;
            }

            // Lưu reference
            _dataSource = documents;
            stockInOutDocumentDtoBindingSource.DataSource = documents;
            stockInOutDocumentDtoBindingSource.ResetBindings(false);
            
            // Force refresh GridView
            StockInOutDocumentDtoGridView.RefreshData();
            
            UpdateStatusBar();
            
            // Log để debug
            System.Diagnostics.Debug.WriteLine($"LoadData: Đã load {documents.Count} chứng từ vào grid");
        }

        /// <summary>
        /// Map entities sang DTOs
        /// </summary>
        private List<StockInOutDocumentDto> MapEntitiesToDtos(List<Dal.DataContext.StockInOutDocument> entities)
        {
            if (entities == null)
                return [];

            var dtos = new List<StockInOutDocumentDto>();

            foreach (var entity in entities)
            {
                var master = entity.StockInOutMaster;

                var dto = new StockInOutDocumentDto
                {
                    Id = entity.Id,
                    StockInOutMasterId = entity.StockInOutMasterId,
                    BusinessPartnerId = entity.BusinessPartnerId,
                    DocumentType = entity.DocumentType,
                    DocumentCategory = entity.DocumentCategory,
                    DocumentNumber = entity.DocumentNumber,
                    DocumentDate = entity.DocumentDate,
                    IssueDate = entity.IssueDate,
                    ExpiryDate = entity.ExpiryDate,
                    Amount = entity.Amount,
                    Currency = entity.Currency,
                    FileName = entity.FileName,
                    DisplayName = entity.DisplayName,
                    Description = entity.Description,
                    RelativePath = entity.RelativePath,
                    FullPath = entity.FullPath,
                    StorageType = entity.StorageType,
                    FileSize = entity.FileSize,
                    FileExtension = entity.FileExtension,
                    MimeType = entity.MimeType,
                    FileExists = entity.FileExists,
                    IsVerified = entity.IsVerified,
                    AccessLevel = entity.AccessLevel,
                    IsConfidential = entity.IsConfidential,
                    CreateDate = entity.CreateDate,
                    CreateBy = entity.CreateBy,
                    ModifiedDate = entity.ModifiedDate,
                    IsActive = entity.IsActive
                };

                // Map DocumentTypeText
                if (Enum.IsDefined(typeof(DocumentTypeEnum), dto.DocumentType))
                {
                    dto.DocumentTypeText = GetEnumDescription((DocumentTypeEnum)dto.DocumentType);
                }

                // Map DocumentCategoryText
                if (dto.DocumentCategory.HasValue && Enum.IsDefined(typeof(DocumentCategoryEnum), dto.DocumentCategory.Value))
                {
                    dto.DocumentCategoryText = GetEnumDescription((DocumentCategoryEnum)dto.DocumentCategory.Value);
                }

                // Map AccessLevelText
                if (dto.AccessLevel.HasValue && Enum.IsDefined(typeof(DocumentAccessLevelEnum), dto.AccessLevel.Value))
                {
                    dto.AccessLevelText = GetEnumDescription((DocumentAccessLevelEnum)dto.AccessLevel.Value);
                }

                // Load StockInOutMaster nếu có
                if (master != null)
                {
                    dto.VocherNumber = master.VocherNumber;
                    dto.StockInOutDate = master.StockInOutDate;
                    dto.LoaiNhapXuatKho = (LoaiNhapXuatKhoEnum?)master.StockInOutType;
                    if (dto.LoaiNhapXuatKho.HasValue)
                    {
                        dto.LoaiNhapXuatKhoText = GetLoaiNhapXuatKhoText(dto.LoaiNhapXuatKho.Value);
                    }
                }

                dtos.Add(dto);
            }

            return dtos;
        }

        /// <summary>
        /// Lấy danh sách chứng từ được chọn
        /// </summary>
        private List<StockInOutDocumentDto> GetSelectedDocuments()
        {
            var selectedDocuments = new List<StockInOutDocumentDto>();
            var selectedRows = StockInOutDocumentDtoGridView.GetSelectedRows();

            foreach (int rowHandle in selectedRows)
            {
                if (StockInOutDocumentDtoGridView.GetRow(rowHandle) is StockInOutDocumentDto dto)
                {
                    selectedDocuments.Add(dto);
                }
            }

            return selectedDocuments;
        }

        /// <summary>
        /// Cập nhật status bar
        /// </summary>
        private void UpdateStatusBar()
        {
            try
            {
                var totalCount = _dataSource?.Count ?? 0;
                var selectedCount = StockInOutDocumentDtoGridView.SelectedRowsCount;

                DataSummaryBarStaticItem.Caption = $@"Tổng số: <b>{totalCount}</b> chứng từ";
                SelectedRowBarStaticItem.Caption = selectedCount > 0
                    ? $"Đã chọn: <b>{selectedCount}</b> chứng từ"
                    : "Chưa chọn dòng nào";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating status bar: {ex.Message}");
            }
        }

        /// <summary>
        /// Xóa các chứng từ đã chọn
        /// </summary>
        private async Task DeleteSelectedDocumentsAsync(List<StockInOutDocumentDto> selectedDocuments)
        {
            try
            {
                if (_stockInOutDocumentBll == null)
                {
                    ShowError("Dịch vụ chứng từ chưa được khởi tạo.");
                    return;
                }

                await ExecuteWithWaitingFormAsync(async () =>
                {
                    var successCount = 0;
                    var errorCount = 0;
                    var errorMessages = new List<string>();

                    foreach (var dto in selectedDocuments)
                    {
                        try
                        {
                            await _stockInOutDocumentBll.DeleteDocumentAsync(dto.Id);
                            successCount++;
                        }
                        catch (Exception ex)
                        {
                            errorCount++;
                            errorMessages.Add($"{dto.FileName}: {ex.Message}");
                        }
                    }

                    // Hiển thị kết quả
                    var message = $"Kết quả xóa chứng từ:\n\n";
                    message += $"✅ Thành công: {successCount} chứng từ\n";
                    message += $"❌ Lỗi: {errorCount} chứng từ\n";

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
                ShowError(ex, "Lỗi khi xóa chứng từ");
            }
        }

        /// <summary>
        /// Tải xuống một chứng từ
        /// </summary>
        private async Task DownloadDocumentAsync(StockInOutDocumentDto dto)
        {
            try
            {
                using var saveDialog = new SaveFileDialog();
                saveDialog.FileName = dto.FileName ?? $"Document_{dto.Id:N}";
                saveDialog.Filter = @"Tất cả files|*.*";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    await ExecuteWithWaitingFormAsync(async () =>
                    {
                        var documentData = await _stockInOutDocumentBll.GetDocumentDataAsync(dto.Id);
                        if (documentData != null && documentData.Length > 0)
                        {
                            await Task.Run(() => File.WriteAllBytes(saveDialog.FileName, documentData));
                            MsgBox.ShowSuccess($"Đã tải xuống chứng từ thành công:\n{saveDialog.FileName}");
                        }
                        else
                        {
                            MsgBox.ShowWarning("Không thể tải xuống chứng từ. File có thể không tồn tại trên storage.");
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khi tải xuống chứng từ");
            }
        }

        /// <summary>
        /// Tải chứng từ về thư mục Download và mở file
        /// </summary>
        private async Task DownloadAndOpenDocumentAsync(StockInOutDocumentDto dto)
        {
            try
            {
                if (_stockInOutDocumentBll == null)
                {
                    MsgBox.ShowWarning("Dịch vụ chứng từ chưa được khởi tạo.");
                    return;
                }

                await ExecuteWithWaitingFormAsync(async () =>
                {
                    // Lấy đường dẫn thư mục Download
                    var downloadsPath = GetDownloadsFolderPath();
                    if (string.IsNullOrWhiteSpace(downloadsPath) || !Directory.Exists(downloadsPath))
                    {
                        MsgBox.ShowError("Không thể tìm thấy thư mục Download.");
                        return;
                    }

                    // Lấy document data từ storage
                    var documentData = await _stockInOutDocumentBll.GetDocumentDataAsync(dto.Id);
                    if (documentData == null || documentData.Length == 0)
                    {
                        MsgBox.ShowWarning("Không thể đọc dữ liệu chứng từ. File có thể không tồn tại trên storage.");
                        return;
                    }

                    // Tạo tên file để lưu
                    string fileName = dto.FileName;
                    if (string.IsNullOrWhiteSpace(fileName))
                    {
                        var extension = dto.FileExtension ?? ".pdf";
                        if (!extension.StartsWith("."))
                        {
                            extension = "." + extension;
                        }
                        fileName = $"Document_{dto.Id:N}{extension}";
                    }

                    // Đảm bảo tên file hợp lệ (loại bỏ các ký tự không hợp lệ)
                    fileName = Path.GetFileName(fileName);
                    if (string.IsNullOrWhiteSpace(fileName))
                    {
                        fileName = $"Document_{dto.Id:N}.pdf";
                    }

                    // Tạo đường dẫn đầy đủ trong thư mục Download
                    var targetPath = Path.Combine(downloadsPath, fileName);

                    // Nếu file đã tồn tại, thêm số thứ tự vào tên file
                    if (File.Exists(targetPath))
                    {
                        var nameWithoutExt = Path.GetFileNameWithoutExtension(fileName);
                        var extension = Path.GetExtension(fileName);
                        int counter = 1;
                        do
                        {
                            fileName = $"{nameWithoutExt}_{counter}{extension}";
                            targetPath = Path.Combine(downloadsPath, fileName);
                            counter++;
                        } while (File.Exists(targetPath) && counter < 1000);
                    }

                    // Lưu file vào thư mục Download
                    await Task.Run(() =>
                    {
                        File.WriteAllBytes(targetPath, documentData);
                    });

                    // Mở file bằng ứng dụng mặc định
                    try
                    {
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = targetPath,
                            UseShellExecute = true
                        });
                    }
                    catch (Exception ex)
                    {
                        // Nếu không mở được file, vẫn thông báo đã tải về thành công
                        System.Diagnostics.Debug.WriteLine($"Không thể mở file: {ex.Message}");
                        MsgBox.ShowSuccess($"Đã tải xuống chứng từ thành công:\n{targetPath}\n\nKhông thể mở file tự động. Vui lòng mở thủ công.");
                    }
                });
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khi tải và mở chứng từ");
            }
        }

        /// <summary>
        /// Lấy đường dẫn thư mục Download của user
        /// </summary>
        private string GetDownloadsFolderPath()
        {
            try
            {
                // Cách 1: Sử dụng Environment.SpecialFolder.UserProfile + "Downloads"
                var userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                var downloadsPath = Path.Combine(userProfile, "Downloads");
                
                if (Directory.Exists(downloadsPath))
                {
                    return downloadsPath;
                }

                // Cách 2: Thử đọc từ Registry (nếu user đã thay đổi vị trí Downloads)
                try
                {
                    using var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders");
                    if (key != null)
                    {
                        var downloads = key.GetValue("{374DE290-123F-4565-9164-39C4925E467B}");
                        if (downloads != null)
                        {
                            var customDownloadsPath = downloads.ToString();
                            if (Directory.Exists(customDownloadsPath))
                            {
                                return customDownloadsPath;
                            }
                        }
                    }
                }
                catch
                {
                    // Ignore registry errors, fallback to default path
                }

                // Cách 3: Trả về đường dẫn mặc định (ngay cả khi không tồn tại, sẽ tạo sau)
                return downloadsPath;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting Downloads folder: {ex.Message}");
                return null;
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
        /// Lấy description từ enum bằng DescriptionAttribute
        /// </summary>
        private string GetEnumDescription(Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            if (fieldInfo != null)
            {
                var attributes = fieldInfo.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                if (attributes.Length > 0)
                {
                    return ((System.ComponentModel.DescriptionAttribute)attributes[0]).Description;
                }
            }
            return value.ToString();
        }

        /// <summary>
        /// Lấy text hiển thị cho LoaiNhapXuatKhoEnum
        /// </summary>
        private string GetLoaiNhapXuatKhoText(LoaiNhapXuatKhoEnum loai)
        {
            var fieldInfo = loai.GetType().GetField(loai.ToString());
            if (fieldInfo != null)
            {
                var attributes = fieldInfo.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                if (attributes.Length > 0)
                {
                    return ((System.ComponentModel.DescriptionAttribute)attributes[0]).Description;
                }
            }
            return loai.ToString();
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
}
