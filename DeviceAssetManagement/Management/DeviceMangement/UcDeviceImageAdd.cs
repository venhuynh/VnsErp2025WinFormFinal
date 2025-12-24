using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bll.Inventory.InventoryManagement;
using Common.Common;
using Common.Helpers;
using Common.Utils;
using DTO.Inventory.InventoryManagement;

namespace DeviceAssetManagement.Management.DeviceMangement
{
    public partial class UcDeviceImageAdd : DevExpress.XtraEditors.XtraUserControl
    {
        #region ========== KHAI BÁO BIẾN ==========

        /// <summary>
        /// Business Logic Layer cho hình ảnh thiết bị
        /// </summary>
        private DeviceImageBll _deviceImageBll;

        /// <summary>
        /// Business Logic Layer cho thiết bị
        /// </summary>
        private DeviceBll _deviceBll;

        /// <summary>
        /// Danh sách thiết bị đã chọn từ form cha
        /// </summary>
        private List<DeviceDto> _selectedDevices;

        #endregion

        #region ========== CONSTRUCTOR & PUBLIC METHODS ==========

        /// <summary>
        /// Constructor mặc định
        /// </summary>
        public UcDeviceImageAdd()
        {
            InitializeComponent();
            InitializeLogTextBox();
            InitializeBll();
            InitializeEvents();

            // Thiết lập SuperToolTip cho các controls
            SetupSuperToolTips();

            // Khởi tạo danh sách thiết bị đã chọn
            _selectedDevices = new List<DeviceDto>();
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
                    LogTextBoxHelper.AppendInfo(LogTextBox, "Form thêm hình ảnh thiết bị đã được khởi tạo");
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
                _deviceImageBll = new DeviceImageBll();
                _deviceBll = new DeviceBll();
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
                _deviceImageBll = null; // Set null để disable các chức năng upload
                DisableUploadControls(); // Disable các control liên quan
            }
            catch (Exception ex)
            {
                var errorMessage = "Lỗi khởi tạo dịch vụ lưu trữ hình ảnh: " + ex.Message;
                LogTextBoxHelper.AppendError(LogTextBox, "Lỗi khởi tạo dịch vụ lưu trữ hình ảnh", ex);
                Common.Utils.MsgBox.ShowError(errorMessage, "Lỗi", this);
                _deviceImageBll = null; // Set null để disable các chức năng upload
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

        #region ========== QUẢN LÝ DỮ LIỆU ==========

        /// <summary>
        /// Load danh sách thiết bị đã chọn từ form cha
        /// Tương tự như UcDeviceDtoAddStockInOutHistory.LoadSelectedDevices
        /// </summary>
        /// <param name="selectedDevices">Danh sách thiết bị đã chọn</param>
        public void LoadSelectedDevices(List<DeviceDto> selectedDevices)
        {
            try
            {
                // Lưu danh sách thiết bị đã chọn
                _selectedDevices = selectedDevices ?? new List<DeviceDto>();

                if (_selectedDevices.Count == 0)
                {
                    LogTextBoxHelper.AppendWarning(LogTextBox, "Không có thiết bị nào được chọn");
                    return;
                }

                LogTextBoxHelper.AppendInfo(LogTextBox, $"Đã nhận {_selectedDevices.Count} thiết bị để thêm hình ảnh");

                // Hiển thị thông tin các thiết bị đã chọn
                foreach (var device in _selectedDevices)
                {
                    if (device != null)
                    {
                        var deviceInfo = $"{device.SerialNumber ?? "N/A"} - {device.ProductVariantName ?? "N/A"}";
                        LogTextBoxHelper.AppendInfo(LogTextBox, $"  • {deviceInfo}");
                    }
                }
            }
            catch (Exception ex)
            {
                LogTextBoxHelper.AppendError(LogTextBox, "Lỗi khi load danh sách thiết bị", ex);
                ShowError(ex, "Lỗi khi load danh sách thiết bị");
            }
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
                if (_deviceImageBll == null)
                {
                    LogTextBoxHelper.AppendError(LogTextBox, "Dịch vụ lưu trữ hình ảnh chưa được cấu hình");
                    ShowError("Dịch vụ lưu trữ hình ảnh chưa được cấu hình. " +
                              "Vui lòng kiểm tra lại cấu hình trong App.config và khởi động lại ứng dụng.");
                    return;
                }

                // Kiểm tra có thiết bị nào được chọn không
                if (_selectedDevices == null || _selectedDevices.Count == 0)
                {
                    LogTextBoxHelper.AppendWarning(LogTextBox, "Vui lòng chọn thiết bị trước khi thêm hình ảnh");
                    ShowError("Vui lòng chọn thiết bị trước khi thêm hình ảnh.");
                    return;
                }

                // Cấu hình OpenFileDialog để chọn nhiều hình ảnh
                xtraOpenFileDialog1.Filter = @"Hình ảnh|*.jpg;*.jpeg;*.png;*.gif;*.bmp|Tất cả files|*.*";
                xtraOpenFileDialog1.Multiselect = true;
                xtraOpenFileDialog1.Title = @"Chọn hình ảnh cho thiết bị";

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
        /// Xử lý các hình ảnh đã chọn cho tất cả các thiết bị đã chọn
        /// </summary>
        /// <param name="imagePaths">Danh sách đường dẫn hình ảnh</param>
        private async Task ProcessSelectedImagesAsync(string[] imagePaths)
        {
            try
            {
                // Hiển thị WaitingForm sử dụng SplashScreenHelper để đảm bảo thread safety
                // Đảm bảo được gọi trên UI thread - sử dụng BeginInvoke để tránh deadlock
                if (InvokeRequired)
                {
                    BeginInvoke(new Action(() => SplashScreenHelper.ShowWaitingSplashScreen()));
                }
                else
                {
                    SplashScreenHelper.ShowWaitingSplashScreen();
                }

                try
                {
                    // Sử dụng ConfigureAwait(false) để tránh deadlock và COM context issues
                    await ProcessSelectedImagesWithoutSplashAsync(imagePaths).ConfigureAwait(false);
                }
                finally
                {
                    // Đóng WaitingForm - đảm bảo được gọi trên UI thread
                    if (InvokeRequired)
                    {
                        BeginInvoke(new Action(() => SplashScreenHelper.CloseSplashScreen()));
                    }
                    else
                    {
                        SplashScreenHelper.CloseSplashScreen();
                    }
                }
            }
            catch (Exception ex)
            {
                // Đảm bảo đóng splash screen ngay cả khi có lỗi
                if (InvokeRequired)
                {
                    BeginInvoke(new Action(() =>
                    {
                        SplashScreenHelper.CloseSplashScreen();
                        ShowError(ex, "Lỗi khi xử lý hình ảnh");
                    }));
                }
                else
                {
                    SplashScreenHelper.CloseSplashScreen();
                    ShowError(ex, "Lỗi khi xử lý hình ảnh");
                }
            }
        }

        /// <summary>
        /// Xử lý các hình ảnh đã chọn (không hiển thị WaitingForm)
        /// Xử lý cho tất cả các thiết bị trong danh sách đã chọn
        /// </summary>
        /// <param name="imagePaths">Danh sách đường dẫn hình ảnh</param>
        private async Task ProcessSelectedImagesWithoutSplashAsync(string[] imagePaths)
        {
            var totalDevices = _selectedDevices.Count;
            var totalFiles = imagePaths.Length;
            var totalOperations = totalDevices * totalFiles;
            var currentOperation = 0;
            var successCount = 0;
            var errorCount = 0;
            var errorMessages = new List<string>();

            LogTextBoxHelper.AppendInfo(LogTextBox, $"Bắt đầu xử lý {totalFiles} hình ảnh cho {totalDevices} thiết bị...");
            LogTextBoxHelper.AppendLine(LogTextBox, "");

            // Xử lý từng thiết bị
            foreach (var device in _selectedDevices)
            {
                if (device == null || device.Id == Guid.Empty)
                    continue;

                var deviceInfo = $"{device.SerialNumber ?? "N/A"} - {device.ProductVariantName ?? "N/A"}";
                LogTextBoxHelper.AppendInfo(LogTextBox, $"Thiết bị: {deviceInfo}");

                // Xử lý từng hình ảnh cho thiết bị này
                for (int i = 0; i < imagePaths.Length; i++)
                {
                    var imagePath = imagePaths[i];
                    var fileName = Path.GetFileName(imagePath);
                    currentOperation++;

                    try
                    {
                        LogTextBoxHelper.AppendInfo(LogTextBox, $"  [{currentOperation}/{totalOperations}] Đang xử lý: {fileName}");

                        // Lưu hình ảnh sử dụng BLL - sử dụng ConfigureAwait(false) để tránh deadlock
                        var success = await SaveImageFromFileAsync(device.Id, imagePath).ConfigureAwait(false);

                        if (success)
                        {
                            successCount++;
                            LogTextBoxHelper.AppendSuccess(LogTextBox, $"  [{currentOperation}/{totalOperations}] Đã lưu thành công: {fileName}");
                        }
                        else
                        {
                            errorCount++;
                            var errorMsg = $"Không thể lưu hình ảnh";
                            errorMessages.Add($"{deviceInfo} - {fileName}: {errorMsg}");
                            LogTextBoxHelper.AppendError(LogTextBox, $"  [{currentOperation}/{totalOperations}] Lỗi: {fileName} - {errorMsg}");
                        }
                    }
                    catch (Exception ex)
                    {
                        errorCount++;
                        errorMessages.Add($"{deviceInfo} - {fileName}: {ex.Message}");
                        LogTextBoxHelper.AppendError(LogTextBox, $"  [{currentOperation}/{totalOperations}] Lỗi: {fileName}", ex);
                    }
                }

                LogTextBoxHelper.AppendLine(LogTextBox, "");
            }

            // Tóm tắt kết quả
            LogTextBoxHelper.AppendLine(LogTextBox, "");
            LogTextBoxHelper.AppendInfo(LogTextBox, $"Hoàn thành xử lý: {successCount} thành công, {errorCount} lỗi");

            // Hiển thị kết quả - đảm bảo được gọi trên UI thread
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => ShowImageProcessingResult(successCount, errorCount, errorMessages)));
            }
            else
            {
                ShowImageProcessingResult(successCount, errorCount, errorMessages);
            }
        }

        /// <summary>
        /// Lưu hình ảnh từ file vào NAS/Local storage và metadata vào database
        /// </summary>
        /// <param name="deviceId">ID thiết bị</param>
        /// <param name="imageFilePath">Đường dẫn file ảnh</param>
        /// <returns>True nếu lưu thành công</returns>
        private async Task<bool> SaveImageFromFileAsync(Guid deviceId, string imageFilePath)
        {
            try
            {
                // Kiểm tra BLL đã được khởi tạo chưa
                if (_deviceImageBll == null)
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
                // Sử dụng ConfigureAwait(false) để tránh deadlock và COM context issues
                var deviceImage = await _deviceImageBll.SaveImageFromFileAsync(deviceId, imageFilePath).ConfigureAwait(false);

                // Kiểm tra kết quả
                if (deviceImage == null)
                {
                    throw new InvalidOperationException($"Không thể lưu hình ảnh '{Path.GetFileName(imageFilePath)}'");
                }

                LogTextBoxHelper.AppendInfo(LogTextBox, $"  Đã lưu vào: {deviceImage.RelativePath ?? deviceImage.FullPath}");

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
                
                // Trigger event để form cha refresh dữ liệu
                OnImageSaved();
            }
            else
            {
                MsgBox.ShowError(message);
            }
        }

        /// <summary>
        /// Event được trigger khi hình ảnh được lưu thành công
        /// </summary>
        public event EventHandler ImageSaved;

        /// <summary>
        /// Trigger event ImageSaved
        /// </summary>
        protected virtual void OnImageSaved()
        {
            ImageSaved?.Invoke(this, EventArgs.Empty);
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
                        content: "Chọn một hoặc nhiều hình ảnh để thêm vào các thiết bị đã chọn.<br/><br/>" +
                                "<b>Chức năng:</b><br/>" +
                                "• Upload hình ảnh cho tất cả các thiết bị đã chọn từ màn hình quản lý<br/>" +
                                "• Hỗ trợ chọn nhiều hình ảnh cùng lúc<br/>" +
                                "• Tự động lưu vào NAS/Local storage và metadata vào database<br/>" +
                                "• Hiển thị tiến trình và kết quả chi tiết<br/><br/>" +
                                "<color=Gray>Lưu ý:</color> Thiết bị sẽ được nhận từ màn hình quản lý thiết bị."
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