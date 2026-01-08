using Bll.Inventory.InventoryManagement;
using Bll.MasterData.ProductServiceBll;
using Common.Utils;
using DevExpress.BarCodes;
using DevExpress.Drawing.Extensions;
using DevExpress.XtraEditors;
using DTO.Inventory.InventoryManagement;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using QRCodeCompactionMode = DevExpress.BarCodes.QRCodeCompactionMode;

namespace Inventory.ProductVariantIdentifier
{
    public partial class NOT_USE_FrmProductVariantIdentifierQrCode : XtraForm
    {
        #region ========== FIELDS & PROPERTIES ==========

        /// <summary>
        /// Định danh sản phẩm cần in QR Code
        /// </summary>
        private readonly ProductVariantIdentifierDto _identifier;

        /// <summary>
        /// Delegate thông báo cập nhật DTO sau khi lưu QR
        /// </summary>
        public event Action<ProductVariantIdentifierDto> IdentifierUpdated;

        /// <summary>
        /// Logger để ghi log các sự kiện
        /// </summary>
        private readonly ILogger _logger = LoggerFactory.CreateLogger(LogCategory.UI);

        /// <summary>
        /// Business Logic Layer cho ProductVariant
        /// </summary>
        private readonly ProductVariantBll _productVariantBll = new ProductVariantBll();

        /// <summary>
        /// Business Logic Layer cho ProductVariantIdentifier
        /// </summary>
        private readonly ProductVariantIdentifierBll _productVariantIdentifierBll = new ProductVariantIdentifierBll();

        /// <summary>
        /// Kích thước in QR code (Width x Height) tính bằng mm
        /// Mặc định: 50mm x 50mm (kích thước label tiêu chuẩn)
        /// </summary>
        private float _printWidthMm = 50.0f;
        private float _printHeightMm = 50.0f;

        /// <summary>
        /// Kích thước in QR code - Width (mm)
        /// </summary>
        public float PrintWidthMm
        {
            get => _printWidthMm;
            set
            {
                if (value > 0)
                {
                    _printWidthMm = value;
                }
            }
        }

        /// <summary>
        /// Kích thước in QR code - Height (mm)
        /// </summary>
        public float PrintHeightMm
        {
            get => _printHeightMm;
            set
            {
                if (value > 0)
                {
                    _printHeightMm = value;
                }
            }
        }

        #endregion

        #region ========== CONSTRUCTOR ==========

        /// <summary>
        /// Constructor mặc định
        /// </summary>
        public NOT_USE_FrmProductVariantIdentifierQrCode()
        {
            InitializeComponent();
            InitializeForm();
        }

        /// <summary>
        /// Constructor với một định danh sản phẩm
        /// </summary>
        /// <param name="identifier">Định danh sản phẩm cần in QR Code</param>
        public NOT_USE_FrmProductVariantIdentifierQrCode(ProductVariantIdentifierDto identifier) : this()
        {
            _identifier = identifier;
        }

        #endregion

        #region ========== INITIALIZATION ==========

        /// <summary>
        /// Khởi tạo form
        /// </summary>
        private void InitializeForm()
        {
            try
            {
                InitializeQrCodePictureEdit();
                InitializeBarButtonEvents();
                InitializeFormEvents();
            }
            catch (Exception ex)
            {
                _logger.Error("InitializeForm: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi khởi tạo form: {ex.Message}");
            }
        }

        /// <summary>
        /// Khởi tạo các event handlers cho form
        /// </summary>
        private void InitializeFormEvents()
        {
            try
            {
                Load += FrmProductVariantIdentifierQrCode_Load;
                FormClosed += FrmProductVariantIdentifierQrCode_FormClosed;
            }
            catch (Exception ex)
            {
                _logger.Error("InitializeFormEvents: Exception occurred", ex);
            }
        }

        /// <summary>
        /// Event handler khi form được load
        /// </summary>
        private void FrmProductVariantIdentifierQrCode_Load(object sender, EventArgs e)
        {
            try
            {
                LoadQrCode();
            }
            catch (Exception ex)
            {
                _logger.Error("FrmProductVariantIdentifierQrCode_Load: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi tải QR code: {ex.Message}");
            }
        }

        /// <summary>
        /// Khi form đóng, cập nhật lại DTO đã chọn để hiển thị hình QR mới nhất
        /// </summary>
        private void FrmProductVariantIdentifierQrCode_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (_identifier == null || _identifier.Id == Guid.Empty)
                    return;

                // Lấy lại thông tin mới nhất từ database
                var refreshed = _productVariantIdentifierBll.GetById(_identifier.Id);
                if (refreshed == null)
                    return;

                // Cập nhật các trường hình ảnh vào DTO gốc
                _identifier.QRCodeImage = refreshed.QRCodeImage;
                _identifier.QRCodeImagePath = refreshed.QRCodeImagePath;
                _identifier.QRCodeImageFullPath = refreshed.QRCodeImageFullPath;
                _identifier.QRCodeImageFileName = refreshed.QRCodeImageFileName;
                _identifier.QRCodeImageStorageType = refreshed.QRCodeImageStorageType;
                _identifier.QRCodeImageLocked = refreshed.QRCodeImageLocked;
                _identifier.QRCodeImageLockedBy = refreshed.QRCodeImageLockedBy;
                _identifier.QRCodeImageLockedDate = refreshed.QRCodeImageLockedDate;

                // Notify subscriber (parent form) về DTO đã cập nhật
                IdentifierUpdated?.Invoke(_identifier);
            }
            catch (Exception ex)
            {
                _logger.Error("FrmProductVariantIdentifierQrCode_FormClosed: Exception occurred", ex);
            }
        }

        /// <summary>
        /// Khởi tạo các event handlers cho bar buttons
        /// </summary>
        private void InitializeBarButtonEvents()
        {
            try
            {
                InQrCodeBarButtonItem.ItemClick += InQrCodeBarButtonItem_ItemClick;
                CloseBarButtonItem.ItemClick += CloseBarButtonItem_ItemClick;
                DownloadQrCodeBarButtonItem.ItemClick += DownloadQrCodeBarButtonItem_ItemClick;
            }
            catch (Exception ex)
            {
                _logger.Error("InitializeBarButtonEvents: Exception occurred", ex);
            }
        }

        /// <summary>
        /// Khởi tạo QrCodePictureEdit - vô hiệu hóa click phải chuột
        /// </summary>
        private void InitializeQrCodePictureEdit()
        {
            try
            {
                // Vô hiệu hóa context menu của DevExpress PictureEdit
                QrCodePictureEdit.Properties.ContextMenuStrip = null;
                
                // Tạo một context menu rỗng để chặn context menu mặc định
                var emptyContextMenu = new ContextMenuStrip();
                QrCodePictureEdit.ContextMenuStrip = emptyContextMenu;
                
                // Vô hiệu hóa click phải chuột bằng cách xử lý sự kiện
                QrCodePictureEdit.MouseDown += QrCodePictureEdit_MouseDown;
            }
            catch (Exception ex)
            {
                _logger.Error("InitializeQrCodePictureEdit: Exception occurred", ex);
            }
        }

        /// <summary>
        /// Xử lý sự kiện MouseDown để vô hiệu hóa click phải chuột
        /// </summary>
        private void QrCodePictureEdit_MouseDown(object sender, MouseEventArgs e)
        {
            // Vô hiệu hóa click phải chuột - không làm gì cả
            if (e.Button == MouseButtons.Right)
            {
                // Không xử lý gì, ngăn context menu hiển thị
                return;
            }
        }

        #endregion

        #region ========== EVENT HANDLERS ==========

        /// <summary>
        /// Xử lý sự kiện khi click nút "In QR Code"
        /// Cho phép chọn máy in trước khi in
        /// </summary>
        private void InQrCodeBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                // Kiểm tra xem đã có QR code chưa
                if (QrCodePictureEdit == null || QrCodePictureEdit.Image == null)
                {
                    MsgBox.ShowWarning("Chưa có mã QR code để in. Vui lòng tải QR code trước.");
                    return;
                }

                // Hiển thị dialog chọn máy in
                using (var printDialog = new PrintDialog())
                {
                    // Lấy danh sách máy in có sẵn
                    printDialog.AllowPrintToFile = false;
                    printDialog.AllowSomePages = false;
                    printDialog.UseEXDialog = true;

                    // Hiển thị dialog chọn máy in
                    if (printDialog.ShowDialog() != DialogResult.OK)
                    {
                        // Người dùng hủy chọn máy in
                        return;
                    }

                    // In QR code với máy in đã chọn
                    PrintQrCode(printDialog.PrinterSettings);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("InQrCodeBarButtonItem_ItemClick: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi in QR code: {ex.Message}");
            }
        }

        /// <summary>
        /// Xử lý sự kiện khi click nút "Đóng"
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
            }
        }

        /// <summary>
        /// Xử lý sự kiện khi click nút "Tải QR"
        /// </summary>
        private void DownloadQrCodeBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                // Kiểm tra xem đã có QR code chưa
                if (QrCodePictureEdit == null || QrCodePictureEdit.Image == null)
                {
                    MsgBox.ShowWarning("Chưa có mã QR code để tải. Vui lòng tải QR code trước.");
                    return;
                }

                // Hiển thị dialog lưu file
                using var saveDialog = new SaveFileDialog();
                saveDialog.Filter = @"PNG files (*.png)|*.png|JPEG files (*.jpg)|*.jpg|All files (*.*)|*.*";
                saveDialog.FilterIndex = 1;
                saveDialog.FileName = $"QRCode_{DateTime.Now:yyyyMMdd_HHmmss}.png";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    // Lưu hình ảnh
                    var image = QrCodePictureEdit.Image;
                    var format = System.Drawing.Imaging.ImageFormat.Png;
                    if (saveDialog.FilterIndex == 2)
                    {
                        format = System.Drawing.Imaging.ImageFormat.Jpeg;
                    }

                    image.Save(saveDialog.FileName, format);
                    MsgBox.ShowSuccess($"Đã lưu QR code thành công:\n{saveDialog.FileName}");
                }
            }
            catch (Exception ex)
            {
                _logger.Error("DownloadQrCodeBarButtonItem_ItemClick: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi tải QR code: {ex.Message}");
            }
        }

        #endregion

        #region ========== LOAD QR CODE ==========

        /// <summary>
        /// Load và hiển thị QR code từ định danh
        /// </summary>
        private void LoadQrCode()
        {
            try
            {
                if (_identifier == null)
                {
                    MsgBox.ShowWarning("Không có dữ liệu định danh để hiển thị QR code.");
                    return;
                }

                var identifier = _identifier;

                Image qrImage = null;

                // Ưu tiên 1: Load từ QRCodeImage (byte array)
                if (identifier.QRCodeImage != null && identifier.QRCodeImage.Length > 0)
                {
                    try
                    {
                        using (var ms = new MemoryStream(identifier.QRCodeImage))
                        {
                            qrImage = Image.FromStream(ms);
                        }
                        _logger.Debug("LoadQrCode: Đã load QR code từ QRCodeImage (byte array)");
                    }
                    catch (Exception ex)
                    {
                        _logger.Warning($"LoadQrCode: Không thể load từ QRCodeImage - {ex.Message}");
                    }
                }

                // Ưu tiên 2: Load từ file path
                if (qrImage == null && !string.IsNullOrWhiteSpace(identifier.QRCodeImageFullPath))
                {
                    try
                    {
                        if (File.Exists(identifier.QRCodeImageFullPath))
                        {
                            qrImage = Image.FromFile(identifier.QRCodeImageFullPath);
                            _logger.Debug($"LoadQrCode: Đã load QR code từ file: {identifier.QRCodeImageFullPath}");
                        }
                        else
                        {
                            _logger.Warning($"LoadQrCode: File không tồn tại: {identifier.QRCodeImageFullPath}");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.Warning($"LoadQrCode: Không thể load từ file path - {ex.Message}");
                    }
                }

                // Ưu tiên 3: Tạo QR code từ giá trị QRCode
                if (qrImage == null && !string.IsNullOrWhiteSpace(identifier.QRCode))
                {
                    try
                    {
                        qrImage = GenerateQrCodeFromValue(identifier.QRCode);
                        _logger.Debug("LoadQrCode: Đã tạo QR code từ giá trị QRCode");
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"LoadQrCode: Không thể tạo QR code từ giá trị - {ex.Message}", ex);
                    }
                }

                // Ưu tiên 4: Tạo QR code từ các giá trị định danh khác (SerialNumber, PartNumber, etc.)
                if (qrImage == null)
                {
                    try
                    {
                        var payload = BuildQrCodePayload(identifier);
                        if (!string.IsNullOrWhiteSpace(payload))
                        {
                            qrImage = GenerateQrCodeFromValue(payload);
                            _logger.Debug("LoadQrCode: Đã tạo QR code từ các giá trị định danh");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"LoadQrCode: Không thể tạo QR code từ các giá trị định danh - {ex.Message}", ex);
                    }
                }

                // Hiển thị QR code
                if (qrImage != null)
                {
                    // Dispose image cũ nếu có
                    QrCodePictureEdit.Image?.Dispose();
                    QrCodePictureEdit.Image = qrImage;
                }
                else
                {
                    MsgBox.ShowWarning("Không thể load hoặc tạo QR code. Vui lòng kiểm tra dữ liệu định danh.");
                }
            }
            catch (Exception ex)
            {
                _logger.Error("LoadQrCode: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi tải QR code: {ex.Message}");
            }
        }

        /// <summary>
        /// Tạo chuỗi payload từ các giá trị định danh
        /// </summary>
        /// <param name="identifier">Định danh sản phẩm</param>
        /// <returns>Chuỗi payload hoặc string.Empty nếu không có dữ liệu</returns>
        private string BuildQrCodePayload(ProductVariantIdentifierDto identifier)
        {
            try
            {
                if (identifier == null)
                {
                    return string.Empty;
                }

                var values = new List<string>();

                // Lấy tất cả các giá trị định danh không rỗng
                if (!string.IsNullOrWhiteSpace(identifier.SerialNumber))
                    values.Add(identifier.SerialNumber);
                if (!string.IsNullOrWhiteSpace(identifier.PartNumber))
                    values.Add(identifier.PartNumber);
                if (!string.IsNullOrWhiteSpace(identifier.QRCode))
                    values.Add(identifier.QRCode);
                if (!string.IsNullOrWhiteSpace(identifier.SKU))
                    values.Add(identifier.SKU);
                if (!string.IsNullOrWhiteSpace(identifier.RFID))
                    values.Add(identifier.RFID);
                if (!string.IsNullOrWhiteSpace(identifier.MACAddress))
                    values.Add(identifier.MACAddress);
                if (!string.IsNullOrWhiteSpace(identifier.IMEI))
                    values.Add(identifier.IMEI);
                if (!string.IsNullOrWhiteSpace(identifier.AssetTag))
                    values.Add(identifier.AssetTag);
                if (!string.IsNullOrWhiteSpace(identifier.LicenseKey))
                    values.Add(identifier.LicenseKey);
                if (!string.IsNullOrWhiteSpace(identifier.UPC))
                    values.Add(identifier.UPC);
                if (!string.IsNullOrWhiteSpace(identifier.EAN))
                    values.Add(identifier.EAN);
                if (!string.IsNullOrWhiteSpace(identifier.ID))
                    values.Add(identifier.ID);
                if (!string.IsNullOrWhiteSpace(identifier.OtherIdentifier))
                    values.Add(identifier.OtherIdentifier);

                // Tạo chuỗi: Value|Value|Value|... (các giá trị cách nhau bởi |)
                return string.Join("|", values);
            }
            catch (Exception ex)
            {
                _logger.Error("BuildQrCodePayload: Exception occurred", ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// Tạo QR code từ giá trị string
        /// </summary>
        /// <param name="value">Giá trị để tạo QR code</param>
        /// <returns>Image QR code hoặc null nếu có lỗi</returns>
        private Image GenerateQrCodeFromValue(string value)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    return null;
                }

                // Tạo QR code
                using var barCode = new BarCode();
                barCode.Symbology = Symbology.QRCode;
                barCode.BackColor = Color.White;
                barCode.ForeColor = Color.Black;
                barCode.RotationAngle = 0;
                barCode.DpiX = 96;
                barCode.DpiY = 96;
                barCode.Module = 2; // Kích thước module
                barCode.CodeBinaryData = Encoding.UTF8.GetBytes(value);
                barCode.Options.QRCode.CompactionMode = QRCodeCompactionMode.Byte;
                barCode.Options.QRCode.ErrorLevel = QRCodeErrorLevel.Q; // Mức lỗi Q (25%)
                barCode.Options.QRCode.ShowCodeText = false;

                // Convert và trả về image
                return barCode.BarCodeImage.ConvertToGdiPlusImage();
            }
            catch (Exception ex)
            {
                _logger.Error($"GenerateQrCodeFromValue: Exception occurred for value '{value}'", ex);
                return null;
            }
        }

        #endregion

        #region ========== PRINT QR CODE ==========

        /// <summary>
        /// In QR code với máy in đã chọn
        /// </summary>
        /// <param name="printerSettings">Cài đặt máy in</param>
        private void PrintQrCode(PrinterSettings printerSettings)
        {
            try
            {
                if (printerSettings == null)
                {
                    MsgBox.ShowWarning("Không có thông tin máy in.");
                    return;
                }

                if (QrCodePictureEdit?.Image == null)
                {
                    MsgBox.ShowWarning("Chưa có mã QR code để in.");
                    return;
                }

                // Lấy định danh để hiển thị thông tin sản phẩm
                var identifier = _identifier;

                // Tạo PrintDocument
                using var printDocument = new PrintDocument();
                // Set máy in
                printDocument.PrinterSettings = printerSettings;

                // Convert kích thước từ mm sang 1/100 inch (đơn vị của PaperSize)
                // 1 inch = 25.4 mm, nên 1 mm = 100/25.4 = 3.937 1/100 inch
                const float mmToHundredthsInch = 3.937f;
                var widthInHundredthsInch = (int)(_printWidthMm * mmToHundredthsInch);
                var heightInHundredthsInch = (int)(_printHeightMm * mmToHundredthsInch);

                // Set kích thước trang tùy chỉnh theo kích thước label
                printDocument.DefaultPageSettings.PaperSize = new PaperSize(
                    $"Custom {_printWidthMm}mm x {_printHeightMm}mm",
                    widthInHundredthsInch,
                    heightInHundredthsInch);

                // Set margins = 0 để in đầy trang
                printDocument.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);

                // Event handler để vẽ QR code
                printDocument.PrintPage += (sender, e) =>
                {
                    try
                    {
                        var qrImage = QrCodePictureEdit.Image;
                        if (qrImage == null)
                        {
                            e.Cancel = true;
                            return;
                        }

                        var pageBounds = e.PageBounds;
                        var imageSize = qrImage.Size;

                        // Chia không gian thành 2 phần: bên trái cho QR code, bên phải cho thông tin
                        float margin = 3; // Margin nhỏ xung quanh
                        float dividerX = pageBounds.Width / 2; // Đường chia đôi
                        float qrAreaWidth = dividerX - margin * 1.5f; // Nửa bên trái trừ margin
                        float infoAreaWidth = pageBounds.Width - dividerX - margin * 1.5f; // Nửa bên phải trừ margin
                        float availableHeight = pageBounds.Height - margin * 2; // Chiều cao khả dụng

                        // ========== VẼ QR CODE BÊN TRÁI ==========
                        // Scale QR code để vừa với nửa bên trái (giữ nguyên tỷ lệ)
                        float qrScaleX = qrAreaWidth / imageSize.Width;
                        float qrScaleY = availableHeight / imageSize.Height;
                        float qrScale = Math.Min(qrScaleX, qrScaleY);

                        var qrScaledWidth = imageSize.Width * qrScale;
                        var qrScaledHeight = imageSize.Height * qrScale;

                        // Căn giữa QR code trong nửa bên trái
                        float qrX = margin;
                        float qrY = margin + (availableHeight - qrScaledHeight) / 2;

                        // Vẽ QR code
                        var qrRect = new RectangleF(qrX, qrY, qrScaledWidth, qrScaledHeight);
                        e.Graphics.DrawImage(qrImage, qrRect);

                        // ========== VẼ THÔNG TIN SẢN PHẨM BÊN PHẢI ==========
                        if (identifier != null)
                        {
                            string variantNameForReport = null;
                            
                            // Gọi BLL để lấy VariantNameForReport từ ProductVariantId
                            if (identifier.ProductVariantId != Guid.Empty)
                            {
                                try
                                {
                                    var productVariant = _productVariantBll.GetById(identifier.ProductVariantId);
                                    if (productVariant != null)
                                    {
                                        variantNameForReport = productVariant.VariantNameForReport;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    _logger.Warning($"PrintPage: Không thể lấy VariantNameForReport từ BLL - {ex.Message}");
                                }
                            }

                            // Fallback về ProductVariantFullName nếu không lấy được VariantNameForReport
                            if (string.IsNullOrWhiteSpace(variantNameForReport))
                            {
                                variantNameForReport = identifier.ProductVariantFullName;
                            }

                            // Hiển thị thông tin sản phẩm và identifier
                            // Sử dụng font size 6 cho toàn bộ thông tin
                            using var font = new Font("Arial", 6, FontStyle.Regular);
                            using var brush = new SolidBrush(Color.Black);
                            
                            float infoX = dividerX + margin;
                            float spacing = 1;
                            var sf = new StringFormat
                            {
                                Alignment = StringAlignment.Near,
                                LineAlignment = StringAlignment.Near
                            };

                            // Chuẩn bị danh sách thông tin identifier (không bao gồm ID và Status)
                            var identifierInfo = new List<string>();
                            
                            if (!string.IsNullOrWhiteSpace(identifier.SerialNumber))
                                identifierInfo.Add($"Serial: {identifier.SerialNumber}");
                            if (!string.IsNullOrWhiteSpace(identifier.PartNumber))
                                identifierInfo.Add($"Part: {identifier.PartNumber}");
                            if (!string.IsNullOrWhiteSpace(identifier.QRCode))
                                identifierInfo.Add($"QR: {identifier.QRCode}");
                            if (!string.IsNullOrWhiteSpace(identifier.SKU))
                                identifierInfo.Add($"SKU: {identifier.SKU}");
                            if (!string.IsNullOrWhiteSpace(identifier.RFID))
                                identifierInfo.Add($"RFID: {identifier.RFID}");
                            if (!string.IsNullOrWhiteSpace(identifier.MACAddress))
                                identifierInfo.Add($"MAC: {identifier.MACAddress}");
                            if (!string.IsNullOrWhiteSpace(identifier.IMEI))
                                identifierInfo.Add($"IMEI: {identifier.IMEI}");
                            if (!string.IsNullOrWhiteSpace(identifier.AssetTag))
                                identifierInfo.Add($"Asset: {identifier.AssetTag}");
                            if (!string.IsNullOrWhiteSpace(identifier.LicenseKey))
                                identifierInfo.Add($"License: {identifier.LicenseKey}");
                            if (!string.IsNullOrWhiteSpace(identifier.UPC))
                                identifierInfo.Add($"UPC: {identifier.UPC}");
                            if (!string.IsNullOrWhiteSpace(identifier.EAN))
                                identifierInfo.Add($"EAN: {identifier.EAN}");
                            if (!string.IsNullOrWhiteSpace(identifier.OtherIdentifier))
                                identifierInfo.Add($"Other: {identifier.OtherIdentifier}");

                            // Tính tổng chiều cao của tất cả thông tin để căn giữa
                            float totalHeight = 0;
                            
                            // Tính chiều cao của tên sản phẩm
                            if (!string.IsNullOrWhiteSpace(variantNameForReport))
                            {
                                var titleSize = e.Graphics.MeasureString(variantNameForReport, font, (int)infoAreaWidth, sf);
                                totalHeight += titleSize.Height + spacing;
                            }

                            // Tính chiều cao của tất cả identifier
                            foreach (var info in identifierInfo)
                            {
                                var textSize = e.Graphics.MeasureString(info, font, (int)infoAreaWidth, sf);
                                totalHeight += textSize.Height + spacing;
                            }

                            // Trừ spacing cuối cùng
                            if (totalHeight > 0)
                                totalHeight -= spacing;

                            // Căn giữa thông tin sản phẩm với QR code
                            float startY = qrY + (qrScaledHeight - totalHeight) / 2;
                            // Đảm bảo không vượt quá margin
                            if (startY < margin)
                                startY = margin;

                            float currentY = startY;

                            // Vẽ tên sản phẩm (VariantNameForReport) với wrap text
                            if (!string.IsNullOrWhiteSpace(variantNameForReport))
                            {
                                var titleSize = e.Graphics.MeasureString(variantNameForReport, font, (int)infoAreaWidth, sf);
                                var actualTitleRect = new RectangleF(infoX, currentY, infoAreaWidth, titleSize.Height);
                                
                                e.Graphics.DrawString(variantNameForReport, font, brush, actualTitleRect, sf);
                                currentY += titleSize.Height + spacing;
                            }

                            // Hiển thị tất cả các dòng identifier với wrap text
                            foreach (var info in identifierInfo)
                            {
                                if (currentY >= pageBounds.Height - margin)
                                    break; // Dừng nếu vượt quá chiều cao trang

                                // Tính chiều cao thực tế của text (có thể xuống nhiều dòng)
                                var textSize = e.Graphics.MeasureString(info, font, (int)infoAreaWidth, sf);
                                
                                // Chỉ vẽ nếu còn đủ không gian
                                if (currentY + textSize.Height <= pageBounds.Height - margin)
                                {
                                    var actualInfoRect = new RectangleF(infoX, currentY, infoAreaWidth, textSize.Height);
                                    e.Graphics.DrawString(info, font, brush, actualInfoRect, sf);
                                    currentY += textSize.Height + spacing;
                                }
                                else
                                {
                                    break; // Không đủ không gian, dừng lại
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.Error("PrintPage: Exception occurred", ex);
                        e.Cancel = true;
                    }
                };

                // In
                printDocument.Print();
                MsgBox.ShowSuccess("Đã gửi lệnh in QR code thành công.");

                // Lưu hình ảnh QR code vào database sau khi in
                SaveQrCodeImageToDatabase();
            }
            catch (Exception ex)
            {
                _logger.Error("PrintQrCode: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi in QR code: {ex.Message}");
            }
        }

        #endregion

        #region ========== SAVE QR CODE IMAGE ==========

        /// <summary>
        /// Lưu hình ảnh QR code hiện tại vào database
        /// </summary>
        private void SaveQrCodeImageToDatabase()
        {
            try
            {
                if (_identifier == null || _identifier.Id == Guid.Empty)
                    return;

                if (QrCodePictureEdit?.Image == null)
                    return;

                using var ms = new MemoryStream();
                QrCodePictureEdit.Image.Save(ms, ImageFormat.Png);
                var imageBytes = ms.ToArray();

                // Lấy DTO mới nhất và lưu hình ảnh trực tiếp vào database (không lưu NAS)
                var dto = _productVariantIdentifierBll.GetById(_identifier.Id);
                if (dto == null)
                    return;

                dto.QRCodeImage = imageBytes;
                dto.QRCodeImagePath = null;
                dto.QRCodeImageFullPath = null;
                dto.QRCodeImageFileName = null;
                dto.QRCodeImageStorageType = "DB";

                _productVariantIdentifierBll.SaveOrUpdate(dto);

                // Đồng bộ lại DTO hiện tại
                _identifier.QRCodeImage = dto.QRCodeImage;
                _identifier.QRCodeImagePath = dto.QRCodeImagePath;
                _identifier.QRCodeImageFullPath = dto.QRCodeImageFullPath;
                _identifier.QRCodeImageFileName = dto.QRCodeImageFileName;
                _identifier.QRCodeImageStorageType = dto.QRCodeImageStorageType;
            }
            catch (Exception ex)
            {
                _logger.Error("SaveQrCodeImageToDatabase: Exception occurred", ex);
                // Không chặn luồng chính; chỉ log lỗi
            }
        }

        #endregion
    }
}