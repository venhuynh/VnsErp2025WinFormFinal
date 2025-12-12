using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bll.MasterData.ProductServiceBll;
using Common.Common;
using Common.Helpers;
using Common.Utils;
using DevExpress.XtraEditors;
using DTO.MasterData.ProductService;

namespace MasterData.ProductService
{
    /// <summary>
    /// Form thêm hình ảnh sản phẩm.
    /// Cung cấp chức năng chọn và upload nhiều hình ảnh cho sản phẩm với validation và giao diện thân thiện.
    /// </summary>
    public partial class FrmProductImageAdd : XtraForm
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
        /// ID sản phẩm để thêm hình ảnh
        /// </summary>
        private Guid ProductId { get; set; }

        #endregion

        #region ========== CONSTRUCTOR & PUBLIC METHODS ==========

        /// <summary>
        /// Constructor mặc định
        /// </summary>
        public FrmProductImageAdd()
        {
            InitializeComponent();
            InitializeLogTextBox();
            InitializeBll();
            InitializeEvents();
            
            // Thiết lập SuperToolTip cho các controls
            SetupSuperToolTips();
            
            LoadProductList();
        }

        #endregion

        #region ========== KHỞI TẠO FORM ==========

        /// <summary>
        /// Khởi tạo LogTextBox
        /// </summary>
        private void InitializeLogTextBox()
        {
            try
            {
                if (LogTextBox != null)
                {
                    LogTextBoxHelper.InitializeLogTextBox(LogTextBox);
                    LogTextBoxHelper.AppendInfo(LogTextBox, "Form thêm hình ảnh sản phẩm đã được khởi tạo");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khởi tạo LogTextBox: {ex.Message}");
            }
        }

        /// <summary>
        /// Khởi tạo BLL
        /// </summary>
        private void InitializeBll()
        {
            try
            {
                LogTextBoxHelper.AppendInfo(LogTextBox, "Đang khởi tạo dịch vụ lưu trữ hình ảnh...");
                _productImageBll = new ProductImageBll();
                _productServiceBll = new ProductServiceBll();
                LogTextBoxHelper.AppendSuccess(LogTextBox, "Khởi tạo dịch vụ lưu trữ hình ảnh thành công");
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

                LogTextBoxHelper.AppendError(LogTextBox, "Không thể khởi tạo dịch vụ lưu trữ hình ảnh", ex);
                Common.Utils.MsgBox.ShowWarning(errorMessage, "Cảnh báo cấu hình", this);
                _productImageBll = null; // Set null để disable các chức năng upload
                DisableUploadControls(); // Disable các control liên quan
            }
            catch (Exception ex)
            {
                var errorMessage = "Lỗi khởi tạo dịch vụ lưu trữ hình ảnh: " + ex.Message;
                LogTextBoxHelper.AppendError(LogTextBox, "Lỗi khởi tạo dịch vụ lưu trữ hình ảnh", ex);
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
        /// Khởi tạo events
        /// </summary>
        private void InitializeEvents()
        {
            // Event cho nút chọn hình ảnh
            OpenSelectImageHyperlinkLabelControl.Click += OpenSelectImageHyperlinkLabelControl_Click;
            
            // Event cho SearchLookupEdit chọn sản phẩm
            ProductServiceSearchLookupEdit.EditValueChanged += ProductServiceSearchLookupEdit_EditValueChanged;
        }

        #endregion

        #region ========== QUẢN LÝ DỮ LIỆU ==========

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

        /// <summary>
        /// Load danh sách sản phẩm/dịch vụ
        /// </summary>
        private async void LoadProductList()
        {
            await ExecuteWithWaitingFormAsync(async () =>
            {
                await LoadProductListAsync();
            });
        }

        /// <summary>
        /// Load danh sách sản phẩm/dịch vụ (async, không hiển thị WaitForm)
        /// </summary>
        private async Task LoadProductListAsync()
        {
            try
            {
                LogTextBoxHelper.AppendInfo(LogTextBox, "Đang tải danh sách sản phẩm/dịch vụ...");

                // Get all data
                var entities = await _productServiceBll.GetAllAsync();

                // Convert to DTOs (without counting to improve performance)
                var dtoList = entities.ToDtoList(
                    categoryId => _productServiceBll.GetCategoryName(categoryId)
                ).ToList();
                
                // Bind trực tiếp vào productServiceDtoBindingSource
                productServiceDtoBindingSource.DataSource = dtoList;

                LogTextBoxHelper.AppendSuccess(LogTextBox, $"Đã tải {dtoList.Count} sản phẩm/dịch vụ");

                // Nếu có ProductId được set, tự động chọn sản phẩm đó
                if (ProductId != Guid.Empty)
                {
                    SelectProduct(ProductId);
                }
            }
            catch (Exception ex)
            {
                LogTextBoxHelper.AppendError(LogTextBox, "Lỗi khi tải danh sách sản phẩm/dịch vụ", ex);
                ShowError(ex, "Lỗi khi tải danh sách sản phẩm/dịch vụ");
            }
        }

        /// <summary>
        /// Chọn sản phẩm theo ID
        /// </summary>
        /// <param name="productId">ID sản phẩm</param>
        private void SelectProduct(Guid productId)
        {
            try
            {
                // Tìm sản phẩm trong binding source
                for (int i = 0; i < productServiceDtoBindingSource.Count; i++)
                {
                    if (productServiceDtoBindingSource[i] is ProductServiceDto product && product.Id == productId)
                    {
                        productServiceDtoBindingSource.Position = i;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khi chọn sản phẩm");
            }
        }

        #endregion

        #region ========== SỰ KIỆN FORM ==========

        /// <summary>
        /// Xử lý sự kiện thay đổi giá trị SearchLookupEdit
        /// </summary>
        private void ProductServiceSearchLookupEdit_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                // EditValue trả về Guid (ID của sản phẩm)
                if (ProductServiceSearchLookupEdit.EditValue is Guid productId)
                {
                    // Cập nhật ProductId
                    ProductId = productId;
                    
                    // Lấy tên sản phẩm để hiển thị trong log
                    var selectedProduct = productServiceDtoBindingSource.Current as ProductServiceDto;
                    var productDisplay = selectedProduct != null 
                        ? $"{selectedProduct.Code} - {selectedProduct.Name}" 
                        : productId.ToString();
                    LogTextBoxHelper.AppendInfo(LogTextBox, $"Đã chọn sản phẩm: {productDisplay}");
                }
                else
                {
                    // Reset ProductId nếu không có sản phẩm nào được chọn
                    ProductId = Guid.Empty;
                    LogTextBoxHelper.AppendInfo(LogTextBox, "Đã bỏ chọn sản phẩm");
                }
            }
            catch (Exception ex)
            {
                LogTextBoxHelper.AppendError(LogTextBox, "Lỗi khi chọn sản phẩm", ex);
                ShowError(ex, "Lỗi khi chọn sản phẩm");
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
                if (_productImageBll == null)
                {
                    LogTextBoxHelper.AppendError(LogTextBox, "Dịch vụ lưu trữ hình ảnh chưa được cấu hình");
                    ShowError("Dịch vụ lưu trữ hình ảnh chưa được cấu hình. " +
                              "Vui lòng kiểm tra lại cấu hình trong App.config và khởi động lại ứng dụng.");
                    return;
                }

                // Kiểm tra đã chọn sản phẩm chưa
                if (ProductId == Guid.Empty)
                {
                    LogTextBoxHelper.AppendWarning(LogTextBox, "Vui lòng chọn sản phẩm trước khi thêm hình ảnh");
                    ShowError("Vui lòng chọn sản phẩm trước khi thêm hình ảnh.");
                    return;
                }

                // Cấu hình OpenFileDialog để chọn nhiều hình ảnh
                xtraOpenFileDialog1.Filter = @"Hình ảnh|*.jpg;*.jpeg;*.png;*.gif;*.bmp|Tất cả files|*.*";
                xtraOpenFileDialog1.Multiselect = true;
                xtraOpenFileDialog1.Title = @"Chọn hình ảnh cho sản phẩm";

                // Hiển thị dialog chọn file
                if (xtraOpenFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    var selectedFiles = xtraOpenFileDialog1.FileNames;
                    if (selectedFiles.Length > 0)
                    {
                        LogTextBoxHelper.AppendInfo(LogTextBox, $"Đã chọn {selectedFiles.Length} hình ảnh để upload");
                        await ProcessSelectedImagesAsync(selectedFiles);
                    }
                }
            }
            catch (Exception ex)
            {
                LogTextBoxHelper.AppendError(LogTextBox, "Lỗi khi chọn hình ảnh", ex);
                ShowError(ex, "Lỗi khi chọn hình ảnh");
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
            var totalFiles = imagePaths.Length;

            LogTextBoxHelper.AppendInfo(LogTextBox, $"Bắt đầu xử lý {totalFiles} hình ảnh...");
            LogTextBoxHelper.AppendLine(LogTextBox, "");

            for (int i = 0; i < imagePaths.Length; i++)
            {
                var imagePath = imagePaths[i];
                var fileName = Path.GetFileName(imagePath);
                var currentIndex = i + 1;

                try
                {
                    LogTextBoxHelper.AppendInfo(LogTextBox, $"[{currentIndex}/{totalFiles}] Đang xử lý: {fileName}");

                    // Lưu hình ảnh sử dụng BLL
                    var success = await SaveImageFromFileAsync(ProductId, imagePath);

                    if (success)
                    {
                        successCount++;
                        LogTextBoxHelper.AppendSuccess(LogTextBox, $"[{currentIndex}/{totalFiles}] Đã lưu thành công: {fileName}");
                    }
                    else
                    {
                        errorCount++;
                        var errorMsg = $"Không thể lưu hình ảnh";
                        errorMessages.Add($"{fileName}: {errorMsg}");
                        LogTextBoxHelper.AppendError(LogTextBox, $"[{currentIndex}/{totalFiles}] Lỗi: {fileName} - {errorMsg}");
                    }
                }
                catch (Exception ex)
                {
                    errorCount++;
                    errorMessages.Add($"{fileName}: {ex.Message}");
                    LogTextBoxHelper.AppendError(LogTextBox, $"[{currentIndex}/{totalFiles}] Lỗi: {fileName}", ex);
                }
            }

            // Tóm tắt kết quả
            LogTextBoxHelper.AppendLine(LogTextBox, "");
            LogTextBoxHelper.AppendInfo(LogTextBox, $"Hoàn thành xử lý: {successCount} thành công, {errorCount} lỗi");

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
        /// <param name="productId">ID sản phẩm</param>
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

                // Lấy thông tin file
                var fileInfo = new FileInfo(imageFilePath);
                var fileSize = fileInfo.Length;
                var fileSizeMb = fileSize / (1024.0 * 1024.0);
                LogTextBoxHelper.AppendInfo(LogTextBox, $"  Kích thước file: {fileSizeMb:F2} MB");

                // Sử dụng BLL để lưu hình ảnh vào NAS/Local storage và metadata vào database
                // Method này sẽ:
                // 1. Đọc file ảnh
                // 2. Lưu vào NAS/Local storage thông qua ImageStorageService
                // 3. Lưu metadata (FileName, RelativePath, FullPath, etc.) vào database
                var productImage = await _productImageBll.SaveImageFromFileAsync(productId, imageFilePath);

                // Kiểm tra kết quả
                if (productImage == null)
                {
                    throw new InvalidOperationException($"Không thể lưu hình ảnh '{Path.GetFileName(imageFilePath)}'");
                }

                LogTextBoxHelper.AppendInfo(LogTextBox, $"  Đã lưu vào: {productImage.RelativePath ?? productImage.FullPath}");

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

        #region ========== TIỆN ÍCH ==========

        /// <summary>
        /// Thiết lập SuperToolTip cho các controls trong form
        /// </summary>
        private void SetupSuperToolTips()
        {
            try
            {
                if (ProductServiceSearchLookupEdit != null)
                {
                    SuperToolTipHelper.SetBaseEditSuperTip(
                        ProductServiceSearchLookupEdit,
                        title: "<b><color=DarkBlue>📦 Sản phẩm/Dịch vụ</color></b>",
                        content: "Chọn sản phẩm hoặc dịch vụ để thêm hình ảnh. Trường này là bắt buộc."
                    );
                }

                if (OpenSelectImageHyperlinkLabelControl != null)
                {
                    var superTip = SuperToolTipHelper.CreateSuperToolTip(
                        title: "<b><color=Green>🖼️ Chọn hình ảnh</color></b>",
                        content: "Chọn một hoặc nhiều hình ảnh để thêm vào sản phẩm/dịch vụ."
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
}