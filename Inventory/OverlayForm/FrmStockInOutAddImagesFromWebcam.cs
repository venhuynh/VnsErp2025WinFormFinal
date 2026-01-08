using Bll.Inventory.InventoryManagement;
using Bll.Inventory.StockInOut;
using Common.Common;
using Common.Utils;
using DTO.Inventory;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventory.OverlayForm
{
    public partial class FrmStockInOutAddImagesFromWebcam : DevExpress.XtraEditors.XtraForm
    {
        #region ========== KHAI BÁO BIẾN ==========

        /// <summary>
        /// ID phiếu nhập/xuất kho
        /// </summary>
        private Guid StockInOutMasterId { get; set; }

        /// <summary>
        /// Business Logic Layer cho StockInOut
        /// </summary>
        private StockInOutBll _stockInOutBll;

        /// <summary>
        /// Business Logic Layer cho StockInOutImage
        /// </summary>
        private StockInOutImageBll _stockInOutImageBll;

        /// <summary>
        /// Logger để ghi log các sự kiện
        /// </summary>
        private readonly ILogger _logger = LoggerFactory.CreateLogger(LogCategory.UI);

        /// <summary>
        /// Danh sách ảnh đã chụp (chưa lưu)
        /// </summary>
        private List<CapturedImage> _capturedImages = new List<CapturedImage>();

        #endregion

        #region ========== NESTED CLASS ==========

        /// <summary>
        /// Class lưu thông tin ảnh đã chụp
        /// </summary>
        private class CapturedImage
        {
            public byte[] ImageData { get; set; }
            public DateTime CaptureTime { get; set; }
            public string FileName { get; set; }
        }

        #endregion

        #region ========== CONSTRUCTOR ==========

        /// <summary>
        /// Constructor với StockInOutMasterId
        /// </summary>
        /// <param name="stockInOutMasterId">ID phiếu nhập/xuất kho</param>
        public FrmStockInOutAddImagesFromWebcam(Guid stockInOutMasterId)
        {
            InitializeComponent();
            StockInOutMasterId = stockInOutMasterId;
            InitializeBll();
            InitializeEvents();
            Load += FrmStockInOutAddImagesFromWebcam_Load;
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
                _stockInOutBll = new StockInOutBll();
                _stockInOutImageBll = new StockInOutImageBll();
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi khởi tạo BLL: {ex.Message}", ex);
                MsgBox.ShowError($"Lỗi khởi tạo dịch vụ: {ex.Message}");
            }
        }

        /// <summary>
        /// Khởi tạo các event handlers
        /// </summary>
        private void InitializeEvents()
        {
            try
            {
                // CameraControl events - Double click để chụp ảnh
                if (cameraControl1 != null)
                {
                    cameraControl1.DoubleClick += CameraControl1_DoubleClick;
                }

                // Bar button events
                SaveBarButtonItem.ItemClick += SaveBarButtonItem_ItemClick;
                CloseBarButtonItem.ItemClick += CloseBarButtonItem_ItemClick;

                // Form events
                FormClosing += FrmStockInOutAddImagesFromWebcam_FormClosing;
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi khởi tạo events: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Event handler khi form được load
        /// </summary>
        private async void FrmStockInOutAddImagesFromWebcam_Load(object sender, EventArgs e)
        {
            try
            {
                await LoadStockInOutInfoAsync();
                await LoadExistingImagesAsync();
                
                // Bắt đầu capture từ camera
                if (cameraControl1 != null)
                {
                    try
                    {
                        cameraControl1.Start();
                    }
                    catch (Exception camEx)
                    {
                        _logger.Warning($"Không thể khởi động camera: {camEx.Message}");
                        MsgBox.ShowWarning("Không thể khởi động camera. Vui lòng kiểm tra kết nối webcam.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi load form: {ex.Message}", ex);
                MsgBox.ShowError($"Lỗi khởi tạo form: {ex.Message}");
            }
        }

        /// <summary>
        /// Load và hiển thị thông tin phiếu nhập/xuất
        /// </summary>
        private async Task LoadStockInOutInfoAsync()
        {
            try
            {
                if (_stockInOutBll == null || StockInOutMasterId == Guid.Empty)
                {
                    StockInOutInfoSimpleLabelItem.Text = @"<color='Gray'><i>Không có thông tin phiếu nhập/xuất</i></color>";
                    return;
                }

                // Lấy thông tin phiếu từ BLL
                var masterDto = _stockInOutBll.GetStockInOutMasterForUIDtoById(StockInOutMasterId);
                if (masterDto == null)
                {
                    StockInOutInfoSimpleLabelItem.Text = @"<color='Gray'><i>Không tìm thấy thông tin phiếu nhập/xuất</i></color>";
                    return;
                }

                // Format thông tin phiếu dưới dạng HTML
                var infoHtml = FormatStockInOutInfo(masterDto);
                StockInOutInfoSimpleLabelItem.Text = infoHtml;
            }
            catch (Exception ex)
            {
                StockInOutInfoSimpleLabelItem.Text = $@"<color='Red'>Lỗi: {ex.Message}</color>";
            }
        }

        /// <summary>
        /// Format thông tin phiếu nhập/xuất dưới dạng HTML
        /// </summary>
        /// <param name="masterDto">Thông tin phiếu nhập/xuất</param>
        /// <returns>Chuỗi HTML đã format</returns>
        private string FormatStockInOutInfo(StockInOutMasterForUIDto masterDto)
        {
            if (masterDto == null) return string.Empty;

            var html = new StringBuilder();
            html.Append("<b><color='Blue'>Thông tin phiếu nhập/xuất kho</color></b><br>");

            // Số phiếu
            if (!string.IsNullOrWhiteSpace(masterDto.VoucherNumber))
            {
                var voucherNumber = EscapeHtml(masterDto.VoucherNumber);
                html.Append($"<color='Gray'>Số phiếu:</color> <b><color='Blue'>{voucherNumber}</color></b>");
            }

            // Ngày nhập/xuất
            if (masterDto.StockOutDate != default(DateTime))
            {
                if (html.Length > 0) html.Append(" | ");
                html.Append($"<color='Gray'>Ngày:</color> <b>{masterDto.StockOutDate:dd/MM/yyyy}</b>");
            }

            // Loại nhập/xuất
            var loaiNhapXuatKhoName = ApplicationEnumUtils.GetDescription(masterDto.LoaiNhapXuatKho);
            if (!string.IsNullOrWhiteSpace(loaiNhapXuatKhoName))
            {
                if (html.Length > 0) html.Append("<br>");
                var loaiName = EscapeHtml(loaiNhapXuatKhoName);
                html.Append($"<color='Gray'>Loại:</color> <b><color='Green'>{loaiName}</color></b>");
            }

            // Kho
            if (!string.IsNullOrWhiteSpace(masterDto.WarehouseName))
            {
                if (html.Length > 0) html.Append(" | ");
                var warehouseName = EscapeHtml(masterDto.WarehouseName);
                html.Append($"<color='Gray'>Kho:</color> <b>{warehouseName}</b>");
            }

            // Khách hàng/Nhà cung cấp
            if (!string.IsNullOrWhiteSpace(masterDto.CustomerName))
            {
                if (html.Length > 0) html.Append("<br>");
                var customerName = EscapeHtml(masterDto.CustomerName);
                html.Append($"<color='Gray'>Đối tác:</color> <b>{customerName}</b>");
            }

            // Ghi chú (nếu có)
            if (!string.IsNullOrWhiteSpace(masterDto.Notes))
            {
                if (html.Length > 0) html.Append("<br>");
                var notes = masterDto.Notes.Length > 100 ? masterDto.Notes.Substring(0, 100) + "..." : masterDto.Notes;
                var notesEscaped = EscapeHtml(notes);
                html.Append($"<color='Gray'>Ghi chú:</color> <i>{notesEscaped}</i>");
            }

            return html.ToString();
        }

        /// <summary>
        /// Escape các ký tự đặc biệt trong HTML để tránh lỗi hiển thị
        /// </summary>
        /// <param name="text">Text cần escape</param>
        /// <returns>Text đã được escape</returns>
        private string EscapeHtml(string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            
            return text.Replace("&", "&amp;")
                       .Replace("<", "&lt;")
                       .Replace(">", "&gt;")
                       .Replace("\"", "&quot;")
                       .Replace("'", "&#39;");
        }

        #endregion

        #region ========== CAMERA EVENTS ==========

        /// <summary>
        /// Event handler khi double click trên CameraControl để chụp ảnh
        /// </summary>
        private void CameraControl1_DoubleClick(object sender, EventArgs e)
        {
            CaptureImageFromCamera();
        }

        #endregion

        #region ========== CAMERA METHODS ==========

        /// <summary>
        /// Chụp ảnh từ camera và thêm vào danh sách
        /// </summary>
        private void CaptureImageFromCamera()
        {
            try
            {
                if (cameraControl1 == null)
                {
                    MsgBox.ShowWarning("Camera control chưa được khởi tạo.");
                    return;
                }

                // Chụp ảnh từ camera sử dụng TakeSnapshot method
                Bitmap snapshot = null;
                try
                {
                    snapshot = cameraControl1.TakeSnapshot();
                }
                catch (Exception snapEx)
                {
                    _logger.Error($"Lỗi khi chụp ảnh: {snapEx.Message}", snapEx);
                    MsgBox.ShowWarning("Không thể chụp ảnh. Vui lòng đảm bảo camera đang hoạt động.");
                    return;
                }

                if (snapshot == null)
                {
                    MsgBox.ShowWarning("Không thể chụp ảnh. Vui lòng thử lại.");
                    return;
                }

                // Chuyển đổi Image sang byte array
                byte[] imageBytes;
                using (snapshot)
                using (var ms = new MemoryStream())
                {
                    // Lưu dưới dạng JPEG với chất lượng 90%
                    var jpegCodec = ImageCodecInfo.GetImageEncoders().FirstOrDefault(c => c.FormatID == ImageFormat.Jpeg.Guid);
                    var encoderParams = new EncoderParameters(1);
                    // Sử dụng System.Drawing.Imaging.Encoder để tránh ambiguous
                    encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 90);

                    if (jpegCodec != null)
                    {
                        snapshot.Save(ms, jpegCodec, encoderParams);
                    }
                    else
                    {
                        snapshot.Save(ms, ImageFormat.Jpeg);
                    }

                    imageBytes = ms.ToArray();
                }

                // Thêm vào danh sách ảnh đã chụp
                var capturedImage = new CapturedImage
                {
                    ImageData = imageBytes,
                    CaptureTime = DateTime.Now,
                    FileName = $"Webcam_{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid():N}.jpg"
                };

                _capturedImages.Add(capturedImage);

                // Cập nhật grid hiển thị
                RefreshCapturedImagesGrid();

                // Hiển thị thông báo
                MsgBox.ShowSuccess($"Đã chụp ảnh thành công! (Tổng: {_capturedImages.Count} ảnh)");

                _logger.Info($"Đã chụp ảnh từ webcam, Size={imageBytes.Length} bytes, TotalCaptured={_capturedImages.Count}");
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi khi chụp ảnh từ camera: {ex.Message}", ex);
                MsgBox.ShowError($"Lỗi chụp ảnh: {ex.Message}");
            }
        }

        #endregion

        #region ========== EVENT HANDLERS ==========

        /// <summary>
        /// Event handler cho nút Lưu
        /// </summary>
        private async void SaveBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            
        }

        /// <summary>
        /// Event handler cho nút Đóng
        /// </summary>
        private void CloseBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                Close();
            }
            catch (Exception ex)
            {
                _logger.Error("CloseBarButtonItem_ItemClick: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi đóng form: {ex.Message}");
            }
        }

        /// <summary>
        /// Event handler khi form đang đóng
        /// </summary>
        private void FrmStockInOutAddImagesFromWebcam_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                // Kiểm tra có ảnh chưa lưu không
                if (_capturedImages.Count > 0)
                {
                    var result = MsgBox.ShowYesNo(
                        $"Bạn có {_capturedImages.Count} ảnh chưa lưu. Bạn có muốn đóng form không?",
                        "Xác nhận đóng",
                        this);

                    if (!result)
                    {
                        e.Cancel = true;
                        return;
                    }
                }

                // Dừng camera khi đóng form
                if (cameraControl1 != null)
                {
                    try
                    {
                        cameraControl1.Stop();
                    }
                    catch (Exception stopEx)
                    {
                        // Ignore lỗi khi dừng camera
                        _logger.Warning($"Lỗi khi dừng camera: {stopEx.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("FrmStockInOutAddImagesFromWebcam_FormClosing: Exception occurred", ex);
            }
        }

        #endregion

        #region ========== HELPER METHODS ==========

        /// <summary>
        /// Load danh sách ảnh đã lưu từ database
        /// Chú ý: Chưa triển khai đọc ghi database theo yêu cầu
        /// </summary>
        private async Task LoadExistingImagesAsync()
        {
            try
            {
                if (_stockInOutImageBll == null || StockInOutMasterId == Guid.Empty)
                {
                    return;
                }

                // TODO: Triển khai load danh sách ảnh đã lưu từ database
                // Hiện tại chỉ hiển thị thông báo
                await Task.Run(() =>
                {
                    try
                    {
                        // Tạm thời không load từ database
                        // var existingImages = _stockInOutImageBll.GetByStockInOutMasterId(StockInOutMasterId);
                        
                        BeginInvoke(new Action(() =>
                        {
                            // Hiển thị danh sách rỗng tạm thời
                            var imageList = new List<object>();

                            StockInOutImageGridControl.DataSource = imageList;
                            StockInOutImageGridControl.RefreshDataSource();

                            // Cập nhật thống kê
                            SoLuongTaoQrCodeBarStaticItem.Caption = $"Số lượng ảnh đã lưu: <b><color='Blue'>0</color></b>";
                        }));
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"Lỗi load danh sách ảnh: {ex.Message}", ex);
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi LoadExistingImagesAsync: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Refresh grid hiển thị danh sách ảnh đã chụp
        /// </summary>
        private void RefreshCapturedImagesGrid()
        {
            try
            {
                // Tạo danh sách DTO từ captured images để hiển thị trong grid
                var imageList = _capturedImages.Select((img, index) => new
                {
                    Index = index + 1,
                    FileName = img.FileName,
                    CaptureTime = img.CaptureTime,
                    Size = img.ImageData.Length,
                    ImageData = img.ImageData
                }).ToList();

                StockInOutImageGridControl.DataSource = imageList;
                StockInOutImageGridControl.RefreshDataSource();

                // Cập nhật thống kê
                SoLuongNhapXuatBarStaticItem.Caption = $"Số lượng ảnh đã chụp: <b><color='Blue'>{_capturedImages.Count}</color></b>";
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi refresh grid: {ex.Message}", ex);
            }
        }

        #endregion
    }
}