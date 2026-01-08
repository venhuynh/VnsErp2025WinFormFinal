using Bll.Common;
using Bll.Inventory.InventoryManagement;
using Bll.MasterData.ProductServiceBll;
using Common.Helpers;
using Common.Utils;
using DevExpress.BarCodes;
using DevExpress.Drawing.Extensions;
using DTO.Inventory.InventoryManagement;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using QRCodeCompactionMode = DevExpress.BarCodes.QRCodeCompactionMode;

namespace Inventory.ProductVariantIdentifier
{
    public partial class FrmQrCodePrintPreview : DevExpress.XtraEditors.XtraForm
    {
        #region ========== FIELDS & PROPERTIES ==========

        /// <summary>
        /// A collection of product variant identifiers used for QR code print preview functionality.
        /// </summary>
        /// <remarks>
        /// Each item in the collection represents a <see cref="DTO.Inventory.InventoryManagement.ProductVariantIdentifierDto"/> 
        /// containing detailed information about a specific product variant.
        /// </remarks>
        private readonly List<ProductVariantIdentifierDto> _productVariantIdentifiers = [];

        /// <summary>
        /// Business Logic Layer cho ProductVariant
        /// </summary>
        private readonly ProductVariantBll _productVariantBll = new ProductVariantBll();

        /// <summary>
        /// Business Logic Layer cho ProductVariantIdentifier
        /// </summary>
        private readonly ProductVariantIdentifierBll _productVariantIdentifierBll = new ProductVariantIdentifierBll();

        /// <summary>
        /// Business Logic Layer cho Settings
        /// </summary>
        private readonly SettingBll _settingBll = new SettingBll();

        #endregion

        #region ========== CONSTRUCTOR ==========

        public FrmQrCodePrintPreview(List<ProductVariantIdentifierDto> productVariantIdentifiers)
        {
            _productVariantIdentifiers = productVariantIdentifiers;

            InitializeComponent();
        }

        #endregion

        #region ========== FORM EVENTS ==========

        private void FrmQrCodePrintPreview_Load(object sender, EventArgs e)
        {
            InitializeLogTextBox();
            InitializePrinters();
            LoadQrCodePrinterSetting();
            LoadPrintSummaryInfo();
            SetupSuperToolTips();
            InitializeEvents();
        }

        #endregion

        #region ========== INITIALIZATION ==========

        /// <summary>
        /// Khởi tạo các sự kiện
        /// </summary>
        private void InitializeEvents()
        {
            try
            {
                PrintHyperlinkLabelControl.Click += PrintHyperlinkLabelControl_Click;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khởi tạo events: {ex.Message}");
            }
        }

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
                    LogTextBoxHelper.AppendInfo(LogTextBox, "Form in tem QR Code đã được khởi tạo");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khởi tạo LogTextBox: {ex.Message}");
            }
        }

        /// <summary>
        /// Khởi tạo danh sách máy in
        /// </summary>
        private void InitializePrinters()
        {
            try
            {
                if (PrinterComboBoxEdit != null)
                {
                    LogTextBoxHelper.AppendInfo(LogTextBox, "Đang tải danh sách máy in...");

                    // Lấy danh sách máy in đã cài đặt
                    var printers = new List<string>();
                    foreach (string printerName in PrinterSettings.InstalledPrinters)
                    {
                        printers.Add(printerName);
                    }

                    // Load vào ComboBoxEdit
                    PrinterComboBoxEdit.Properties.Items.Clear();
                    PrinterComboBoxEdit.Properties.Items.AddRange(printers);

                    // Chọn máy in mặc định nếu có
                    if (printers.Count > 0)
                    {
                        try
                        {
                            var defaultPrinter = new PrinterSettings();
                            if (!string.IsNullOrEmpty(defaultPrinter.PrinterName) && printers.Contains(defaultPrinter.PrinterName))
                            {
                                PrinterComboBoxEdit.SelectedItem = defaultPrinter.PrinterName;
                                LogTextBoxHelper.AppendSuccess(LogTextBox, $"Đã chọn máy in mặc định: {defaultPrinter.PrinterName}");
                            }
                            else
                            {
                                PrinterComboBoxEdit.SelectedIndex = 0;
                                LogTextBoxHelper.AppendInfo(LogTextBox, $"Đã chọn máy in đầu tiên: {printers[0]}");
                            }
                        }
                        catch
                        {
                            // Nếu không lấy được máy in mặc định, chọn máy in đầu tiên
                            if (printers.Count > 0)
                            {
                                PrinterComboBoxEdit.SelectedIndex = 0;
                                LogTextBoxHelper.AppendInfo(LogTextBox, $"Đã chọn máy in đầu tiên: {printers[0]}");
                            }
                        }
                    }

                    LogTextBoxHelper.AppendSuccess(LogTextBox, $"Đã tải {printers.Count} máy in");
                }
            }
            catch (Exception ex)
            {
                LogTextBoxHelper.AppendError(LogTextBox, "Lỗi khi tải danh sách máy in", ex);
                System.Diagnostics.Debug.WriteLine($"Lỗi khởi tạo danh sách máy in: {ex.Message}");
            }
        }

        /// <summary>
        /// Load cấu hình máy in và khổ giấy từ database và gán vào các controls
        /// </summary>
        private void LoadQrCodePrinterSetting()
        {
            try
            {
                LogTextBoxHelper.AppendInfo(LogTextBox, "Đang tải cấu hình máy in và khổ giấy từ database...");

                var settings = _settingBll.GetQrCodePrinterSettings();
                if (settings == null || settings.Count == 0)
                {
                    LogTextBoxHelper.AppendInfo(LogTextBox, "Chưa có cấu hình máy in và khổ giấy được lưu.");
                    return;
                }

                // Lấy tên máy in
                if (settings.ContainsKey("PrinterName") && !string.IsNullOrWhiteSpace(settings["PrinterName"]))
                {
                    string savedPrinterName = settings["PrinterName"];

                    // Kiểm tra xem máy in có trong danh sách không
                    if (PrinterComboBoxEdit != null && PrinterComboBoxEdit.Properties.Items.Contains(savedPrinterName))
                    {
                        PrinterComboBoxEdit.SelectedItem = savedPrinterName;
                        LogTextBoxHelper.AppendSuccess(LogTextBox, $"Đã chọn máy in đã lưu: {savedPrinterName}");
                    }
                    else
                    {
                        LogTextBoxHelper.AppendWarning(LogTextBox, $"Máy in đã lưu '{savedPrinterName}' không còn trong danh sách máy in.");
                    }
                }

                // Lấy chiều rộng giấy
                if (settings.ContainsKey("PrintWidthMm") && !string.IsNullOrWhiteSpace(settings["PrintWidthMm"]))
                {
                    if (float.TryParse(settings["PrintWidthMm"], out float printWidthMm) && printWidthMm > 0)
                    {
                        if (PrintWidthTextEdit != null)
                        {
                            PrintWidthTextEdit.Text = printWidthMm.ToString("F2");
                            LogTextBoxHelper.AppendInfo(LogTextBox, $"Đã gán chiều rộng giấy: {printWidthMm}mm");
                        }
                    }
                }

                // Lấy chiều cao giấy
                if (settings.ContainsKey("PrintHeightMm") && !string.IsNullOrWhiteSpace(settings["PrintHeightMm"]))
                {
                    if (float.TryParse(settings["PrintHeightMm"], out float printHeightMm) && printHeightMm > 0)
                    {
                        if (PrintHeightTextEdit != null)
                        {
                            PrintHeightTextEdit.Text = printHeightMm.ToString("F2");
                            LogTextBoxHelper.AppendInfo(LogTextBox, $"Đã gán chiều cao giấy: {printHeightMm}mm");
                        }
                    }
                }

                // Lấy thời gian cập nhật
                if (settings.ContainsKey("LastUpdated") && !string.IsNullOrWhiteSpace(settings["LastUpdated"]))
                {
                    if (DateTime.TryParse(settings["LastUpdated"], out DateTime lastUpdated))
                    {
                        LogTextBoxHelper.AppendInfo(LogTextBox, $"Cấu hình được cập nhật lần cuối: {lastUpdated:yyyy-MM-dd HH:mm:ss}");
                    }
                }

                LogTextBoxHelper.AppendSuccess(LogTextBox, "Đã tải cấu hình máy in và khổ giấy từ database thành công.");
            }
            catch (Exception ex)
            {
                LogTextBoxHelper.AppendError(LogTextBox, "Lỗi khi tải cấu hình máy in và khổ giấy", ex);
                System.Diagnostics.Debug.WriteLine($"LoadQrCodePrinterSetting: Exception occurred - {ex.Message}");
                // Không throw exception để không ảnh hưởng đến quá trình khởi tạo form
            }
        }

        /// <summary>
        /// Load thông tin tóm tắt các DTO chuẩn bị in vào PrintQrInforSimpleLabelItem
        /// </summary>
        private void LoadPrintSummaryInfo()
        {
            try
            {
                if (PrintQrInforSimpleLabelItem == null)
                {
                    return;
                }

                if (_productVariantIdentifiers == null || _productVariantIdentifiers.Count == 0)
                {
                    PrintQrInforSimpleLabelItem.Text = @"<b><color=Orange>⚠️ Cảnh báo:</color></b> Không có định danh nào để in.";
                    return;
                }

                var html = new StringBuilder();
                html.Append("<b><color=Blue>📋 Thông tin in QR Code</color></b><br/>");
                html.Append("<br/>");

                // Tổng số lượng
                html.Append($"<b>Tổng số:</b> <color=Green>{_productVariantIdentifiers.Count}</color> định danh<br/>");

                // Thống kê theo ProductVariant
                var variantGroups = _productVariantIdentifiers
                    .Where(dto => dto.ProductVariantId != Guid.Empty)
                    .GroupBy(dto => dto.ProductVariantId)
                    .ToList();

                if (variantGroups.Count > 0)
                {
                    html.Append($"<b>Số loại sản phẩm:</b> <color=Green>{variantGroups.Count}</color><br/>");

                    // Hiển thị top 5 sản phẩm có nhiều định danh nhất
                    var topVariants = variantGroups
                        .OrderByDescending(g => g.Count())
                        .Take(5)
                        .ToList();

                    if (topVariants.Count > 0)
                    {
                        html.Append("<br/>");
                        html.Append("<b>Top sản phẩm:</b><br/>");
                        foreach (var group in topVariants)
                        {
                            var firstDto = group.First();
                            string variantName = !string.IsNullOrWhiteSpace(firstDto.ProductVariantFullName)
                                ? firstDto.ProductVariantFullName
                                : "N/A";

                            // Giới hạn độ dài tên sản phẩm
                            if (variantName.Length > 50)
                            {
                                variantName = variantName.Substring(0, 47) + "...";
                            }

                            html.Append($"  • <color=Gray>{variantName}</color>: <b>{group.Count()}</b> định danh<br/>");
                        }

                        if (variantGroups.Count > 5)
                        {
                            html.Append($"  <color=Gray>... và {variantGroups.Count - 5} sản phẩm khác</color><br/>");
                        }
                    }
                }

                // Thống kê các loại định danh có giá trị
                html.Append("<b>Loại định danh có dữ liệu:</b><br/>");

                var identifierTypes = new Dictionary<string, int>
                {
                    { "SerialNumber", _productVariantIdentifiers.Count(dto => !string.IsNullOrWhiteSpace(dto.SerialNumber)) },
                    { "PartNumber", _productVariantIdentifiers.Count(dto => !string.IsNullOrWhiteSpace(dto.PartNumber)) },
                    { "QRCode", _productVariantIdentifiers.Count(dto => !string.IsNullOrWhiteSpace(dto.QRCode)) },
                    { "SKU", _productVariantIdentifiers.Count(dto => !string.IsNullOrWhiteSpace(dto.SKU)) },
                    { "RFID", _productVariantIdentifiers.Count(dto => !string.IsNullOrWhiteSpace(dto.RFID)) },
                    { "MACAddress", _productVariantIdentifiers.Count(dto => !string.IsNullOrWhiteSpace(dto.MACAddress)) },
                    { "IMEI", _productVariantIdentifiers.Count(dto => !string.IsNullOrWhiteSpace(dto.IMEI)) },
                    { "AssetTag", _productVariantIdentifiers.Count(dto => !string.IsNullOrWhiteSpace(dto.AssetTag)) }
                };

                var availableTypes = identifierTypes
                    .Where(kvp => kvp.Value > 0)
                    .OrderByDescending(kvp => kvp.Value)
                    .ToList();

                if (availableTypes.Count > 0)
                {
                    foreach (var type in availableTypes)
                    {
                        html.Append($"  • <b>{type.Key}:</b> <color=Green>{type.Value}</color><br/>");
                    }
                }
                else
                {
                    html.Append("  <color=Gray>Không có định danh nào có dữ liệu</color><br/>");
                }

                PrintQrInforSimpleLabelItem.Text = html.ToString();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LoadPrintSummaryInfo: Exception occurred - {ex.Message}");
                if (PrintQrInforSimpleLabelItem != null)
                {
                    PrintQrInforSimpleLabelItem.Text = "<b><color=Red>❌ Lỗi:</color></b> Không thể tải thông tin tóm tắt.";
                }
            }
        }

        #endregion

        #region ========== SUPER TOOLTIPS ==========

        /// <summary>
        /// Thiết lập SuperToolTip cho tất cả các controls trong form
        /// </summary>
        private void SetupSuperToolTips()
        {
            try
            {
                // SuperTip cho ComboBox chọn máy in
                if (PrinterComboBoxEdit != null)
                {
                    SuperToolTipHelper.SetBaseEditSuperTip(
                        PrinterComboBoxEdit,
                        title: @"<b><color=Blue>🖨️ Chọn máy in</color></b>",
                        content: @"Chọn máy in để in tem QR Code.<br/><br/><b>Chức năng:</b><br/>• Hiển thị danh sách tất cả máy in đã cài đặt trong hệ thống<br/>• Tự động chọn máy in mặc định hoặc máy in đã lưu từ lần in trước<br/>• Cấu hình máy in sẽ được lưu tự động sau khi in thành công<br/><br/><color=Gray>Lưu ý:</color> Nếu máy in đã lưu không còn trong danh sách, hệ thống sẽ chọn máy in mặc định."
                    );
                }

                // SuperTip cho TextEdit chiều rộng giấy
                if (PrintWidthTextEdit != null)
                {
                    SuperToolTipHelper.SetBaseEditSuperTip(
                        PrintWidthTextEdit,
                        title: @"<b><color=Green>📏 Chiều rộng giấy</color></b>",
                        content: @"Nhập chiều rộng của khổ giấy tính bằng mm (milimet).<br/><br/><b>Ví dụ:</b><br/>• 50mm - Kích thước label nhỏ<br/>• 100mm - Kích thước label trung bình<br/>• 210mm - Kích thước A4<br/><br/><b>Mặc định:</b> 50mm<br/><b>Đơn vị:</b> mm (milimet)<br/><br/><color=Gray>Lưu ý:</color> Giá trị này sẽ được lưu tự động sau khi in thành công."
                    );
                }

                // SuperTip cho TextEdit chiều cao giấy
                if (PrintHeightTextEdit != null)
                {
                    SuperToolTipHelper.SetBaseEditSuperTip(
                        PrintHeightTextEdit,
                        title: @"<b><color=Green>📏 Chiều cao giấy</color></b>",
                        content: @"Nhập chiều cao của khổ giấy tính bằng mm (milimet).<br/><br/><b>Ví dụ:</b><br/>• 50mm - Kích thước label vuông<br/>• 100mm - Kích thước label dài<br/>• 297mm - Kích thước A4<br/><br/><b>Mặc định:</b> 50mm<br/><b>Đơn vị:</b> mm (milimet)<br/><br/><color=Gray>Lưu ý:</color> Giá trị này sẽ được lưu tự động sau khi in thành công."
                    );
                }

                // SuperTip cho HyperlinkLabelControl nút in tem
                if (PrintHyperlinkLabelControl != null)
                {
                    PrintHyperlinkLabelControl.SuperTip = SuperToolTipHelper.CreateSuperToolTip(
                        title: @"<b><color=Green>🖨️ In tem QR Code</color></b>",
                        content: @"In nhiều tem QR Code cho các định danh đã chọn.<br/><br/><b>Quy trình:</b><br/>1. Tạo và lưu QR code vào database cho từng định danh<br/>2. In từng QR code ra giấy với kích thước đã chọn<br/>3. Tự động lưu cấu hình máy in và khổ giấy<br/><br/><b>Yêu cầu:</b><br/>• Phải chọn máy in<br/>• Phải nhập kích thước giấy hợp lệ (rộng và cao > 0)<br/>• Phải có ít nhất một định danh để in<br/><br/><b>Định dạng in:</b><br/>• QR code hiển thị bên trái<br/>• Thông tin sản phẩm và định danh hiển thị bên phải<br/>• Tự động căn giữa và scale để vừa với khổ giấy<br/><br/><color=Gray>Lưu ý:</color> Mỗi định danh sẽ được in trên một trang riêng."
                    );
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"SetupSuperToolTips: Exception occurred - {ex.Message}");
            }
        }

        #endregion

        #region ========== PRINT QR CODE ==========

        /// <summary>
        /// Xử lý sự kiện khi click nút in tem
        /// </summary>
        private void PrintHyperlinkLabelControl_Click(object sender, EventArgs e)
        {
            try
            {
                if (_productVariantIdentifiers == null || _productVariantIdentifiers.Count == 0)
                {
                    LogTextBoxHelper.AppendWarning(LogTextBox, "Không có mã QR code nào để in.");
                    MsgBox.ShowWarning("Không có mã QR code nào để in.");
                    return;
                }

                // Lấy thông tin máy in từ màn hình
                if (PrinterComboBoxEdit.SelectedItem == null)
                {
                    LogTextBoxHelper.AppendWarning(LogTextBox, "Vui lòng chọn máy in.");
                    MsgBox.ShowWarning("Vui lòng chọn máy in.");
                    return;
                }

                string printerName = PrinterComboBoxEdit.SelectedItem.ToString();
                var printerSettings = new PrinterSettings
                {
                    PrinterName = printerName
                };

                // Lấy kích thước giấy từ màn hình
                if (!float.TryParse(PrintWidthTextEdit.Text, out float printWidthMm) || printWidthMm <= 0)
                {
                    LogTextBoxHelper.AppendWarning(LogTextBox, "Kích thước chiều rộng không hợp lệ.");
                    MsgBox.ShowWarning("Kích thước chiều rộng không hợp lệ.");
                    return;
                }

                if (!float.TryParse(PrintHeightTextEdit.Text, out float printHeightMm) || printHeightMm <= 0)
                {
                    LogTextBoxHelper.AppendWarning(LogTextBox, "Kích thước chiều cao không hợp lệ.");
                    MsgBox.ShowWarning("Kích thước chiều cao không hợp lệ.");
                    return;
                }

                LogTextBoxHelper.AppendInfo(LogTextBox, $"Bắt đầu tạo và lưu QR code cho {_productVariantIdentifiers.Count} định danh...");

                // Tạo và lưu QR code vào database cho từng identifier
                GenerateAndSaveQrCodesForAllIdentifiers();

                LogTextBoxHelper.AppendInfo(LogTextBox, $"Bắt đầu in {_productVariantIdentifiers.Count} mã QR code...");
                LogTextBoxHelper.AppendInfo(LogTextBox, $"Máy in: {printerName}");
                LogTextBoxHelper.AppendInfo(LogTextBox, $"Kích thước giấy: {printWidthMm}mm x {printHeightMm}mm");

                // In nhiều QR code
                PrintMultipleQrCodes(printerSettings, printWidthMm, printHeightMm);
            }
            catch (Exception ex)
            {
                LogTextBoxHelper.AppendError(LogTextBox, "Lỗi khi in QR code", ex);
                MsgBox.ShowError($"Lỗi khi in QR code: {ex.Message}");
            }
        }

        /// <summary>
        /// In nhiều QR code
        /// </summary>
        /// <param name="printerSettings">Cài đặt máy in</param>
        /// <param name="printWidthMm">Chiều rộng giấy (mm)</param>
        /// <param name="printHeightMm">Chiều cao giấy (mm)</param>
        private void PrintMultipleQrCodes(PrinterSettings printerSettings, float printWidthMm, float printHeightMm)
        {
            try
            {
                if (printerSettings == null)
                {
                    LogTextBoxHelper.AppendWarning(LogTextBox, "Không có thông tin máy in.");
                    return;
                }

                if (_productVariantIdentifiers == null || _productVariantIdentifiers.Count == 0)
                {
                    LogTextBoxHelper.AppendWarning(LogTextBox, "Không có mã QR code nào để in.");
                    return;
                }

                // Tạo PrintDocument
                using var printDocument = new PrintDocument();
                printDocument.PrinterSettings = printerSettings;

                // Convert kích thước từ mm sang 1/100 inch (đơn vị của PaperSize)
                const float mmToHundredthsInch = 3.937f;
                var widthInHundredthsInch = (int)(printWidthMm * mmToHundredthsInch);
                var heightInHundredthsInch = (int)(printHeightMm * mmToHundredthsInch);

                // Set kích thước trang tùy chỉnh theo kích thước label
                printDocument.DefaultPageSettings.PaperSize = new PaperSize(
                    $"Custom {printWidthMm}mm x {printHeightMm}mm",
                    widthInHundredthsInch,
                    heightInHundredthsInch);

                // Set margins = 0 để in đầy trang
                printDocument.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);

                // Biến để theo dõi identifier hiện tại đang in
                int currentIndex = 0;
                var identifiers = _productVariantIdentifiers.ToList();

                // Event handler để vẽ QR code
                printDocument.PrintPage += (sender, e) =>
                {
                    try
                    {
                        if (currentIndex >= identifiers.Count)
                        {
                            e.HasMorePages = false;
                            return;
                        }

                        var identifier = identifiers[currentIndex];
                        var qrImage = LoadQrCodeImage(identifier);

                        if (qrImage == null)
                        {
                            LogTextBoxHelper.AppendWarning(LogTextBox, $"Không thể tạo QR code cho identifier {currentIndex + 1}/{identifiers.Count}");
                            currentIndex++;
                            e.HasMorePages = currentIndex < identifiers.Count;
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
                                System.Diagnostics.Debug.WriteLine($"PrintPage: Không thể lấy VariantNameForReport từ BLL - {ex.Message}");
                            }
                        }

                        // Fallback về ProductVariantFullName nếu không lấy được VariantNameForReport
                        if (string.IsNullOrWhiteSpace(variantNameForReport))
                        {
                            variantNameForReport = identifier.ProductVariantFullName;
                        }

                        // Hiển thị thông tin sản phẩm và identifier
                        using var font = new Font("Arial", 6, FontStyle.Regular);
                        using var brush = new SolidBrush(Color.Black);

                        float infoX = dividerX + margin;
                        float spacing = 1;
                        var sf = new StringFormat
                        {
                            Alignment = StringAlignment.Near,
                            LineAlignment = StringAlignment.Near
                        };

                        // Chuẩn bị danh sách thông tin identifier
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

                        // Dispose QR image sau khi vẽ
                        qrImage?.Dispose();

                        // Chuyển sang identifier tiếp theo
                        currentIndex++;
                        e.HasMorePages = currentIndex < identifiers.Count;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"PrintPage: Exception occurred - {ex.Message}");
                        e.Cancel = true;
                    }
                };

                // In
                printDocument.Print();
                LogTextBoxHelper.AppendSuccess(LogTextBox, $"Đã gửi lệnh in {identifiers.Count} mã QR code thành công.");

                // Lưu cấu hình máy in và khổ giấy sau khi in xong thành công
                SaveQrCodePrinterSetting(printerSettings, printWidthMm, printHeightMm);

                MsgBox.ShowSuccess($"Đã gửi lệnh in {identifiers.Count} mã QR code thành công.");
            }
            catch (Exception ex)
            {
                LogTextBoxHelper.AppendError(LogTextBox, "Lỗi khi in nhiều QR code", ex);
                MsgBox.ShowError($"Lỗi khi in nhiều QR code: {ex.Message}");
            }
        }

        #endregion

        #region ========== QR CODE GENERATION ==========

        /// <summary>
        /// Tạo và lưu QR code vào database cho tất cả các identifier
        /// </summary>
        private void GenerateAndSaveQrCodesForAllIdentifiers()
        {
            try
            {
                if (_productVariantIdentifiers == null || _productVariantIdentifiers.Count == 0)
                {
                    return;
                }

                int successCount = 0;
                int errorCount = 0;

                for (int i = 0; i < _productVariantIdentifiers.Count; i++)
                {
                    var identifier = _productVariantIdentifiers[i];
                    try
                    {
                        LogTextBoxHelper.AppendInfo(LogTextBox, $"[{i + 1}/{_productVariantIdentifiers.Count}] Đang tạo QR code cho: {GetIdentifierDisplayName(identifier)}");

                        if (GenerateAndSaveQrCodeForIdentifier(identifier))
                        {
                            successCount++;
                            LogTextBoxHelper.AppendSuccess(LogTextBox, $"[{i + 1}/{_productVariantIdentifiers.Count}] Đã tạo và lưu QR code thành công");
                        }
                        else
                        {
                            errorCount++;
                            LogTextBoxHelper.AppendWarning(LogTextBox, $"[{i + 1}/{_productVariantIdentifiers.Count}] Không thể tạo QR code");
                        }
                    }
                    catch (Exception ex)
                    {
                        errorCount++;
                        LogTextBoxHelper.AppendError(LogTextBox, $"[{i + 1}/{_productVariantIdentifiers.Count}] Lỗi khi tạo QR code: {ex.Message}", ex);
                    }
                }

                LogTextBoxHelper.AppendInfo(LogTextBox, $"Hoàn thành: {successCount} thành công, {errorCount} lỗi");
            }
            catch (Exception ex)
            {
                LogTextBoxHelper.AppendError(LogTextBox, "Lỗi khi tạo và lưu QR code cho tất cả identifier", ex);
            }
        }

        /// <summary>
        /// Tạo và lưu QR code vào database cho một identifier
        /// </summary>
        /// <param name="identifier">ProductVariantIdentifierDto</param>
        /// <returns>True nếu thành công, False nếu thất bại</returns>
        private bool GenerateAndSaveQrCodeForIdentifier(ProductVariantIdentifierDto identifier)
        {
            try
            {
                if (identifier == null || identifier.Id == Guid.Empty)
                {
                    return false;
                }

                // Tạo QR code image
                Image qrImage = null;

                // Ưu tiên 1: Tạo từ giá trị QRCode nếu có
                if (!string.IsNullOrWhiteSpace(identifier.QRCode))
                {
                    qrImage = GenerateQrCodeFromValue(identifier.QRCode);
                }

                // Ưu tiên 2: Tạo từ các giá trị định danh khác
                if (qrImage == null)
                {
                    var payload = BuildQrCodePayload(identifier);
                    if (!string.IsNullOrWhiteSpace(payload))
                    {
                        qrImage = GenerateQrCodeFromValue(payload);
                    }
                }

                if (qrImage == null)
                {
                    return false;
                }

                // Convert image thành byte array
                byte[] imageBytes;
                using (var ms = new MemoryStream())
                {
                    qrImage.Save(ms, ImageFormat.Png);
                    imageBytes = ms.ToArray();
                }

                // Lấy DTO mới nhất từ database
                var dto = _productVariantIdentifierBll.GetById(identifier.Id);
                if (dto == null)
                {
                    qrImage?.Dispose();
                    return false;
                }

                // Cập nhật QR code image
                dto.QRCodeImage = imageBytes;
                dto.QRCodeImagePath = null;
                dto.QRCodeImageFullPath = null;
                dto.QRCodeImageFileName = null;
                dto.QRCodeImageStorageType = "DB";

                // Lưu vào database
                _productVariantIdentifierBll.SaveOrUpdate(dto);

                // Cập nhật lại identifier trong danh sách
                identifier.QRCodeImage = dto.QRCodeImage;
                identifier.QRCodeImagePath = dto.QRCodeImagePath;
                identifier.QRCodeImageFullPath = dto.QRCodeImageFullPath;
                identifier.QRCodeImageFileName = dto.QRCodeImageFileName;
                identifier.QRCodeImageStorageType = dto.QRCodeImageStorageType;

                // Dispose image
                qrImage?.Dispose();

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GenerateAndSaveQrCodeForIdentifier: Exception occurred - {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Load QR code image từ identifier
        /// </summary>
        /// <param name="identifier">ProductVariantIdentifierDto</param>
        /// <returns>Image QR code hoặc null nếu có lỗi</returns>
        private Image LoadQrCodeImage(ProductVariantIdentifierDto identifier)
        {
            try
            {
                if (identifier == null)
                {
                    return null;
                }

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
                        return qrImage;
                    }
                    catch
                    {
                        // Ignore và thử cách khác
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
                            return qrImage;
                        }
                    }
                    catch
                    {
                        // Ignore và thử cách khác
                    }
                }

                // Ưu tiên 3: Tạo QR code từ giá trị QRCode
                if (qrImage == null && !string.IsNullOrWhiteSpace(identifier.QRCode))
                {
                    try
                    {
                        qrImage = GenerateQrCodeFromValue(identifier.QRCode);
                        if (qrImage != null)
                            return qrImage;
                    }
                    catch
                    {
                        // Ignore và thử cách khác
                    }
                }

                // Ưu tiên 4: Tạo QR code từ các giá trị định danh
                if (qrImage == null)
                {
                    try
                    {
                        var payload = BuildQrCodePayload(identifier);
                        if (!string.IsNullOrWhiteSpace(payload))
                        {
                            qrImage = GenerateQrCodeFromValue(payload);
                        }
                    }
                    catch
                    {
                        // Ignore
                    }
                }

                return qrImage;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LoadQrCodeImage: Exception occurred - {ex.Message}");
                return null;
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
                System.Diagnostics.Debug.WriteLine($"GenerateQrCodeFromValue: Exception occurred for value '{value}' - {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Tạo payload cho QR code từ các giá trị định danh
        /// </summary>
        /// <param name="identifier">ProductVariantIdentifierDto</param>
        /// <returns>Chuỗi payload</returns>
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
                System.Diagnostics.Debug.WriteLine($"BuildQrCodePayload: Exception occurred - {ex.Message}");
                return string.Empty;
            }
        }

        #endregion

        #region ========== SETTINGS MANAGEMENT ==========

        /// <summary>
        /// Lưu cấu hình máy in và khổ giấy vào database
        /// </summary>
        /// <param name="printerSettings">Cài đặt máy in</param>
        /// <param name="printWidthMm">Chiều rộng giấy (mm)</param>
        /// <param name="printHeightMm">Chiều cao giấy (mm)</param>
        private void SaveQrCodePrinterSetting(PrinterSettings printerSettings, float printWidthMm, float printHeightMm)
        {
            try
            {
                if (printerSettings == null)
                {
                    LogTextBoxHelper.AppendWarning(LogTextBox, "Không có thông tin máy in để lưu cấu hình.");
                    return;
                }

                LogTextBoxHelper.AppendInfo(LogTextBox, "Đang lưu cấu hình máy in và khổ giấy...");

                string printerName = printerSettings.PrinterName ?? "";
                string category = "QRCodePrinter";
                string updatedBy = System.Security.Principal.WindowsIdentity.GetCurrent().Name ?? "System";
                DateTime lastUpdated = DateTime.Now;

                // Lưu từng thuộc tính vào database
                _settingBll.SetValue(category, "PrinterName", printerName, "String", updatedBy, false);
                _settingBll.SetValue(category, "PrintWidthMm", printWidthMm.ToString("F2"), "Decimal", updatedBy, false);
                _settingBll.SetValue(category, "PrintHeightMm", printHeightMm.ToString("F2"), "Decimal", updatedBy, false);
                _settingBll.SetValue(category, "LastUpdated", lastUpdated.ToString("yyyy-MM-dd HH:mm:ss"), "DateTime", updatedBy, false);

                LogTextBoxHelper.AppendSuccess(LogTextBox, $"Đã lưu cấu hình: Máy in={printerName}, Khổ giấy={printWidthMm}mm x {printHeightMm}mm");
            }
            catch (Exception ex)
            {
                LogTextBoxHelper.AppendError(LogTextBox, "Lỗi khi lưu cấu hình máy in và khổ giấy", ex);
                System.Diagnostics.Debug.WriteLine($"SaveQrCodePrinterSetting: Exception occurred - {ex.Message}");
                // Không throw exception để không ảnh hưởng đến quá trình in
            }
        }

        #endregion

        #region ========== HELPER METHODS ==========

        /// <summary>
        /// Lấy tên hiển thị của identifier
        /// </summary>
        /// <param name="identifier">ProductVariantIdentifierDto</param>
        /// <returns>Tên hiển thị</returns>
        private string GetIdentifierDisplayName(ProductVariantIdentifierDto identifier)
        {
            if (identifier == null)
                return "N/A";

            if (!string.IsNullOrWhiteSpace(identifier.SerialNumber))
                return identifier.SerialNumber;
            if (!string.IsNullOrWhiteSpace(identifier.PartNumber))
                return identifier.PartNumber;
            if (!string.IsNullOrWhiteSpace(identifier.QRCode))
                return identifier.QRCode;
            if (!string.IsNullOrWhiteSpace(identifier.SKU))
                return identifier.SKU;
            if (!string.IsNullOrWhiteSpace(identifier.ID))
                return identifier.ID;

            return identifier.Id.ToString();
        }

        #endregion
    }
}
