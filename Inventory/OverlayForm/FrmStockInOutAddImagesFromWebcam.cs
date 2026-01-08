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
using DTO.Inventory.Query;

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
        /// Ảnh hiện tại đã chụp (chưa thêm vào danh sách)
        /// </summary>
        private StockInOutImageDto _currentImageDto;

        /// <summary>
        /// Bitmap snapshot hiện tại để hiển thị preview (dùng cho xoay ảnh)
        /// </summary>
        private Bitmap _currentSnapshot;

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
                CaptureBarButtonItem.ItemClick += CaptureBarButtonItem_Click;
                
                
                // Bar button events
                SaveBarButtonItem.ItemClick += SaveBarButtonItem_ItemClick;

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
        private Task LoadStockInOutInfoAsync()
        {
            try
            {
                if (_stockInOutBll == null || StockInOutMasterId == Guid.Empty)
                {
                    StockInOutInfoSimpleLabelItem.Text = @"<color='Gray'><i>Không có thông tin phiếu nhập/xuất</i></color>";
                    return Task.CompletedTask;
                }

                // Lấy thông tin phiếu từ BLL
                var masterDto = _stockInOutBll.GetStockInOutMasterForUIDtoById(StockInOutMasterId);
                if (masterDto == null)
                {
                    StockInOutInfoSimpleLabelItem.Text = @"<color='Gray'><i>Không tìm thấy thông tin phiếu nhập/xuất</i></color>";
                    return Task.CompletedTask;
                }

                // Format thông tin phiếu dưới dạng HTML
                var infoHtml = FormatStockInOutInfo(masterDto);
                StockInOutInfoSimpleLabelItem.Text = infoHtml;
            }
            catch (Exception ex)
            {
                StockInOutInfoSimpleLabelItem.Text = $@"<color='Red'>Lỗi: {ex.Message}</color>";
            }

            return Task.CompletedTask;
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
        /// Event handler khi click vào CaptureHyperlinkLabelControl để chụp ảnh
        /// Chỉ chụp và hiển thị trên camera control, không thêm vào danh sách
        /// </summary>
        private void CaptureBarButtonItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (cameraControl1 == null)
                {
                    MsgBox.ShowWarning("Camera control chưa được khởi tạo.");
                    return;
                }

                // Chụp ảnh từ camera sử dụng TakeSnapshot method
                Bitmap snapshot;
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

                // Lưu snapshot để có thể xoay sau này
                if (_currentSnapshot != null)
                {
                    _currentSnapshot.Dispose();
                }
                _currentSnapshot = new Bitmap(snapshot); // Clone snapshot để giữ lại


                // Chuyển đổi Image sang byte array
                byte[] imageBytes;
                using (var ms = new MemoryStream())
                {
                    // Lưu dưới dạng JPEG với chất lượng 90%
                    var jpegCodec = ImageCodecInfo.GetImageEncoders().FirstOrDefault(c => c.FormatID == ImageFormat.Jpeg.Guid);

                    if (jpegCodec != null)
                    {
                        using var encoderParams = new EncoderParameters(1);
                        encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 90L);
                        _currentSnapshot.Save(ms, jpegCodec, encoderParams);
                    }
                    else
                    {
                        _currentSnapshot.Save(ms, ImageFormat.Jpeg);
                    }

                    imageBytes = ms.ToArray();
                }

                // Tạo StockInOutImageDto từ ảnh đã chụp (chưa thêm vào danh sách)
                var captureTime = DateTime.Now;
                _currentImageDto = new StockInOutImageDto
                {
                    Id = Guid.NewGuid(),
                    StockInOutMasterId = StockInOutMasterId,
                    ImageData = imageBytes,
                    FileName = $"Webcam_{captureTime:yyyyMMddHHmmss}_{Guid.NewGuid():N}.jpg",
                    FileSize = imageBytes.Length,
                    CreateDate = captureTime,
                    FileExtension = ".jpg",
                    MimeType = "image/jpeg"
                };

                // Thêm vào datasource và hiển thị trên gridview
                if (stockInOutImageDtoBindingSource != null)
                {
                    // Lấy danh sách hiện tại từ BindingSource
                    if (stockInOutImageDtoBindingSource.DataSource is not List<StockInOutImageDto> currentList)
                    {
                        // Nếu chưa có danh sách, tạo mới
                        currentList = new List<StockInOutImageDto>();
                        stockInOutImageDtoBindingSource.DataSource = currentList;
                    }

                    // Thêm ảnh mới vào danh sách
                    currentList.Add(_currentImageDto);
                    stockInOutImageDtoBindingSource.ResetBindings(false);

                    // Refresh grid để hiển thị
                    StockInOutImageGridView.RefreshData();

                    // Cập nhật thống kê
                    var totalCount = currentList.Count;

                    _logger.Info($"Đã thêm ảnh vào datasource và hiển thị trên gridview, TotalCount={totalCount}");
                }
                else
                {
                    _logger.Warning("stockInOutImageDtoBindingSource chưa được khởi tạo, không thể thêm vào gridview.");
                }

                // Hiển thị thông báo
                MsgBox.ShowSuccess("Đã chụp ảnh thành công! Bạn có thể xoay ảnh nếu cần, sau đó nhấn 'Thêm vào' để thêm vào danh sách.");

                _logger.Info($"Đã chụp ảnh từ webcam để preview, Size={imageBytes.Length} bytes");
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi khi chụp ảnh từ camera: {ex.Message}", ex);
                MsgBox.ShowError($"Lỗi chụp ảnh: {ex.Message}");
            }
        }

        /// <summary>
        /// Event handler khi click vào AddToGridViewHyperlinkLabelControl để thêm ảnh vào danh sách
        /// </summary>
        private void AddToGridViewHyperlinkLabelControl_Click(object sender, EventArgs e)
        {
            try
            {
                if (_currentImageDto == null)
                {
                    MsgBox.ShowWarning("Chưa có ảnh nào được chụp. Vui lòng chụp ảnh trước khi thêm vào danh sách.");
                    return;
                }

                // Thêm vào stockInOutImageDtoBindingSource
                if (stockInOutImageDtoBindingSource == null)
                {
                    _logger.Error("stockInOutImageDtoBindingSource chưa được khởi tạo.");
                    MsgBox.ShowError("Lỗi: BindingSource chưa được khởi tạo.");
                    return;
                }

                // Lấy danh sách hiện tại từ BindingSource
                if (stockInOutImageDtoBindingSource.DataSource is not List<StockInOutImageDto> currentList)
                {
                    // Nếu chưa có danh sách, tạo mới
                    currentList = new List<StockInOutImageDto>();
                    stockInOutImageDtoBindingSource.DataSource = currentList;
                }

                // Thêm ảnh mới vào danh sách
                currentList.Add(_currentImageDto);
                stockInOutImageDtoBindingSource.ResetBindings(false);

                // Clear ảnh hiện tại
                _currentImageDto = null;
                if (_currentSnapshot != null)
                {
                    _currentSnapshot.Dispose();
                    _currentSnapshot = null;
                }

                // Cập nhật thống kê
                var totalCount = currentList.Count;

                // Hiển thị thông báo
                MsgBox.ShowSuccess($"Đã thêm ảnh vào danh sách! (Tổng: {totalCount} ảnh)");

                _logger.Info($"Đã thêm ảnh vào danh sách, TotalCaptured={totalCount}");
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi khi thêm ảnh vào danh sách: {ex.Message}", ex);
                MsgBox.ShowError($"Lỗi thêm ảnh: {ex.Message}");
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
        /// Event handler khi form đang đóng
        /// </summary>
        private void FrmStockInOutAddImagesFromWebcam_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                // Kiểm tra nếu có ảnh chưa lưu trong BindingSource
                var currentList = stockInOutImageDtoBindingSource?.DataSource as List<StockInOutImageDto>;
                var unsavedCount = currentList?.Count ?? 0;
                
                if (unsavedCount > 0)
                {
                    var result = MsgBox.ShowYesNo(
                        $"Bạn có {unsavedCount} ảnh chưa lưu. Bạn có muốn đóng form không?",
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

                // Dispose snapshot nếu có
                if (_currentSnapshot != null)
                {
                    _currentSnapshot.Dispose();
                    _currentSnapshot = null;
                }

            }
            catch (Exception ex)
            {
                _logger.Error("FrmStockInOutAddImagesFromWebcam_FormClosing: Exception occurred", ex);
            }
        }

        #endregion

        #region ========== GRID EVENTS ==========

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
                            // Hiển thị danh sách rỗng tạm thời (sử dụng stockInOutImageDtoBindingSource)
                            var imageList = new List<StockInOutImageDto>();

                            if (stockInOutImageDtoBindingSource != null)
                            {
                                stockInOutImageDtoBindingSource.DataSource = imageList;
                                stockInOutImageDtoBindingSource.ResetBindings(false);
                            }

                            
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
                if (stockInOutImageDtoBindingSource == null) return;

                // Reset bindings để cập nhật grid
                stockInOutImageDtoBindingSource.ResetBindings(false);
                StockInOutImageGridView.RefreshData();

                // Cập nhật thống kê
                var currentList = stockInOutImageDtoBindingSource.DataSource as List<StockInOutImageDto>;
                var totalCount = currentList?.Count ?? 0;
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi refresh grid: {ex.Message}", ex);
            }
        }

        #endregion
    }
}