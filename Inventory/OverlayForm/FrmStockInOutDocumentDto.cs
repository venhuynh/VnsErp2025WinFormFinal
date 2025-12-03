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
using DTO.Inventory.InventoryManagement;

namespace Inventory.OverlayForm
{
    /// <summary>
    /// Form thêm chứng từ cho phiếu nhập/xuất kho.
    /// Cung cấp chức năng chọn và upload nhiều chứng từ (PDF, DOCX, XLSX, etc.) cho phiếu nhập/xuất kho.
    /// </summary>
    public partial class FrmStockInOutDocumentDto : XtraForm
    {
        #region ========== KHAI BÁO BIẾN ==========

        /// <summary>
        /// Business Logic Layer cho chứng từ nhập/xuất kho
        /// </summary>
        private StockInOutDocumentBll _stockInOutDocumentBll;

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
        public FrmStockInOutDocumentDto(Guid stockInOutMasterId)
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
                _stockInOutDocumentBll = new StockInOutDocumentBll();
            }
            catch (InvalidOperationException ex)
            {
                // Lỗi cấu hình File Storage
                var errorMessage = "Không thể khởi tạo dịch vụ lưu trữ chứng từ.\n\n" +
                                   "Nguyên nhân: " + ex.Message + "\n\n" +
                                   "Vui lòng kiểm tra cấu hình trong App.config:\n" +
                                   "- ImageStorage.StorageType (NAS hoặc Local)\n" +
                                   "- Nếu dùng NAS: ImageStorage.NAS.BasePath hoặc ImageStorage.NAS.ServerName + ImageStorage.NAS.ShareName\n" +
                                   "- Nếu dùng Local: ImageStorage.Local.BasePath\n\n" +
                                   "Form sẽ được mở nhưng chức năng upload chứng từ sẽ bị vô hiệu hóa.";

                Common.Utils.MsgBox.ShowWarning(errorMessage, "Cảnh báo cấu hình", this);
                _stockInOutDocumentBll = null; // Set null để disable các chức năng upload
                DisableUploadControls(); // Disable các control liên quan
            }
            catch (Exception ex)
            {
                var errorMessage = "Lỗi khởi tạo dịch vụ lưu trữ chứng từ: " + ex.Message;
                Common.Utils.MsgBox.ShowError(errorMessage, "Lỗi", this);
                _stockInOutDocumentBll = null; // Set null để disable các chức năng upload
                DisableUploadControls(); // Disable các control liên quan
            }
        }


        /// <summary>
        /// Disable các control liên quan đến upload chứng từ khi BLL không khởi tạo được
        /// </summary>
        private void DisableUploadControls()
        {
            try
            {
                if (OpenSelectDocumentHyperlinkLabelControl != null)
                {
                    OpenSelectDocumentHyperlinkLabelControl.Enabled = false;
                    OpenSelectDocumentHyperlinkLabelControl.Text = "Chức năng upload chứng từ đã bị vô hiệu hóa do thiếu cấu hình";
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
            // Event cho nút chọn chứng từ
            OpenSelectDocumentHyperlinkLabelControl.Click += OpenSelectDocumentHyperlinkLabelControl_Click;
        }

        #endregion

        #region ========== SỰ KIỆN FORM ==========

        /// <summary>
        /// Xử lý sự kiện click nút chọn chứng từ
        /// </summary>
        private async void OpenSelectDocumentHyperlinkLabelControl_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra BLL đã được khởi tạo chưa
                if (_stockInOutDocumentBll == null)
                {
                    ShowError("Dịch vụ lưu trữ chứng từ chưa được cấu hình. " +
                              "Vui lòng kiểm tra lại cấu hình trong App.config và khởi động lại ứng dụng.");
                    return;
                }

                // Kiểm tra StockInOutMasterId hợp lệ
                if (StockInOutMasterId == Guid.Empty)
                {
                    ShowError("ID phiếu nhập/xuất kho không hợp lệ.");
                    return;
                }

                // Cấu hình OpenFileDialog để chọn nhiều file chứng từ
                xtraOpenFileDialog1.Filter = @"Chứng từ|*.pdf;*.doc;*.docx;*.xls;*.xlsx;*.txt|PDF|*.pdf|Word|*.doc;*.docx|Excel|*.xls;*.xlsx|Tất cả files|*.*";
                xtraOpenFileDialog1.Multiselect = true;
                xtraOpenFileDialog1.Title = @"Chọn chứng từ cho phiếu nhập/xuất kho";

                // Hiển thị dialog chọn file
                if (xtraOpenFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    var selectedFiles = xtraOpenFileDialog1.FileNames;
                    if (selectedFiles.Length > 0)
                    {
                        await ProcessSelectedDocumentsAsync(selectedFiles);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khi chọn chứng từ");
            }
        }

        #endregion

        #region ========== XỬ LÝ CHỨNG TỪ ==========

        /// <summary>
        /// Xử lý các chứng từ đã chọn
        /// </summary>
        /// <param name="documentPaths">Danh sách đường dẫn chứng từ</param>
        private async Task ProcessSelectedDocumentsAsync(string[] documentPaths)
        {
            try
            {
                await ExecuteWithWaitingFormAsync(async () =>
                {
                    await ProcessSelectedDocumentsWithoutSplashAsync(documentPaths);
                });
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khi xử lý chứng từ");
            }
        }

        /// <summary>
        /// Xử lý các chứng từ đã chọn (không hiển thị WaitingForm)
        /// </summary>
        /// <param name="documentPaths">Danh sách đường dẫn chứng từ</param>
        private async Task ProcessSelectedDocumentsWithoutSplashAsync(string[] documentPaths)
        {
            var successCount = 0;
            var errorCount = 0;
            var errorMessages = new List<string>();

            // Sử dụng DocumentType mặc định là "Chứng từ khác"
            var documentType = (int)DocumentTypeEnum.Other;

            foreach (var documentPath in documentPaths)
            {
                try
                {
                    // Lưu chứng từ sử dụng BLL
                    var success = await SaveDocumentFromFileAsync(StockInOutMasterId, documentPath, documentType);

                    if (success)
                    {
                        successCount++;
                    }
                    else
                    {
                        errorCount++;
                        errorMessages.Add($"{Path.GetFileName(documentPath)}: Không thể lưu chứng từ");
                    }
                }
                catch (Exception ex)
                {
                    errorCount++;
                    errorMessages.Add($"{Path.GetFileName(documentPath)}: {ex.Message}");
                }
            }

            // Hiển thị kết quả
            ShowDocumentProcessingResult(successCount, errorCount, errorMessages);

            // Đóng màn hình nếu có ít nhất một chứng từ thành công
            if (successCount > 0)
            {
                Close();
            }
        }

        /// <summary>
        /// Lưu chứng từ từ file vào NAS/Local storage và metadata vào database
        /// </summary>
        /// <param name="stockInOutMasterId">ID phiếu nhập/xuất kho</param>
        /// <param name="documentFilePath">Đường dẫn file chứng từ</param>
        /// <param name="documentType">Loại chứng từ</param>
        /// <returns>True nếu lưu thành công</returns>
        private async Task<bool> SaveDocumentFromFileAsync(Guid stockInOutMasterId, string documentFilePath, int documentType)
        {
            try
            {
                // Kiểm tra BLL đã được khởi tạo chưa
                if (_stockInOutDocumentBll == null)
                {
                    throw new InvalidOperationException(
                        "Dịch vụ lưu trữ chứng từ chưa được cấu hình. " +
                        "Vui lòng kiểm tra lại cấu hình trong App.config và khởi động lại ứng dụng.");
                }

                if (!File.Exists(documentFilePath))
                {
                    throw new FileNotFoundException($"File chứng từ không tồn tại: {documentFilePath}");
                }

                // Sử dụng BLL để lưu chứng từ vào NAS/Local storage và metadata vào database
                // Method này sẽ:
                // 1. Đọc file chứng từ
                // 2. Lưu vào NAS/Local storage thông qua FileStorageService
                // 3. Lưu metadata (FileName, RelativePath, FullPath, etc.) vào database
                var stockInOutDocument = await _stockInOutDocumentBll.SaveDocumentFromFileAsync(
                    stockInOutMasterId: stockInOutMasterId,
                    businessPartnerId: null,
                    documentFilePath: documentFilePath,
                    documentType: documentType,
                    documentCategory: null,
                    documentNumber: null,
                    documentDate: null,
                    displayName: Path.GetFileNameWithoutExtension(documentFilePath),
                    description: null
                );

                // Kiểm tra kết quả
                if (stockInOutDocument == null)
                {
                    throw new InvalidOperationException($"Không thể lưu chứng từ '{Path.GetFileName(documentFilePath)}'");
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lưu chứng từ '{Path.GetFileName(documentFilePath)}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Hiển thị kết quả xử lý chứng từ
        /// </summary>
        /// <param name="successCount">Số chứng từ thành công</param>
        /// <param name="errorCount">Số chứng từ lỗi</param>
        /// <param name="errorMessages">Danh sách lỗi</param>
        private void ShowDocumentProcessingResult(int successCount, int errorCount, List<string> errorMessages)
        {
            var message = "Kết quả xử lý chứng từ:\n\n";
            message += $"✅ Thành công: {successCount} chứng từ\n";
            message += $"❌ Lỗi: {errorCount} chứng từ\n\n";

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
                message += "\n🎉 Chứng từ đã được lưu thành công!";
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
                if (OpenSelectDocumentHyperlinkLabelControl != null)
                {
                    var superTip = SuperToolTipHelper.CreateSuperToolTip(
                        title: "<b><color=Green>📎 Chọn chứng từ</color></b>",
                        content: @"Chọn một hoặc nhiều file chứng từ (PDF, DOCX, XLSX, etc.) để thêm vào phiếu nhập/xuất kho."
                    );
                    OpenSelectDocumentHyperlinkLabelControl.SuperTip = superTip;
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
