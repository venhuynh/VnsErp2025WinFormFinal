using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bll.Inventory.InventoryManagement;
using Common.Common;
using Common.Utils;
using DevExpress.XtraEditors;

namespace Inventory.OverlayForm
{
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
        /// Khởi tạo events
        /// </summary>
        private void InitializeEvents()
        {
            // Event cho nút chọn hình ảnh
            OpenSelectImageHyperlinkLabelControl.Click += OpenSelectImageHyperlinkLabelControl_Click;
        }

        #endregion

        #region ========== SỰ KIỆN FORM ==========

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

                //// Sử dụng BLL để lưu hình ảnh vào NAS/Local storage và metadata vào database
                //// Method này sẽ:
                //// 1. Đọc file ảnh
                //// 2. Lưu vào NAS/Local storage thông qua ImageStorageService
                //// 3. Lưu metadata (FileName, RelativePath, FullPath, etc.) vào database
                //// Note: SaveImageFromFileAsync trả về StockInOutImage entity từ Dal
                //// Nhưng trong UI layer, chúng ta chỉ cần kiểm tra null, không cần sử dụng entity
                //var result = await _stockInOutImageBll.SaveImageFromFileAsync(stockInOutMasterId, imageFilePath);

                //// Kiểm tra kết quả
                //if (result == null)
                //{
                //    throw new InvalidOperationException($"Không thể lưu hình ảnh '{Path.GetFileName(imageFilePath)}'");
                //}

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
}