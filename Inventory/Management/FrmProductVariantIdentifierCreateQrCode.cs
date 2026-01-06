using Bll.Inventory.InventoryManagement;
using Common.Helpers;
using Common.Utils;
using DevExpress.BarCodes;
using DevExpress.Data;
using DevExpress.Drawing.Extensions;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DTO.Inventory.InventoryManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using QRCodeCompactionMode = DevExpress.BarCodes.QRCodeCompactionMode;

namespace Inventory.Management
{
    public partial class FrmProductVariantIdentifierCreateQrCode : XtraForm
    {

        #region ========== FIELDS & PROPERTIES ==========
        private readonly StockInOutProductHistoryDto _selectedDto;

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


        /// <summary>
        /// Cache danh sách ID đã có trong database (lazy load)
        /// </summary>
        private HashSet<Guid> _existingIdsCache = null;

        /// <summary>
        /// Flag đánh dấu đã load cache chưa
        /// </summary>
        private bool _isIdsCacheLoaded = false;

        #endregion

        public FrmProductVariantIdentifierCreateQrCode(StockInOutProductHistoryDto selectedDto)
        {
            _selectedDto = selectedDto;
            InitializeComponent();
            InitializeProductVariantIdentifierGrid();
            InitializeQrCodePictureEdit();
            InitializeBarButtonEvents();
            InitializeFormEvents();
        }

        /// <summary>
        /// Khởi tạo các event handlers cho form
        /// </summary>
        private void InitializeFormEvents()
        {
            try
            {
                Load += FrmProductVariantIdentifierCreateQrCode_Load;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"InitializeFormEvents: Exception occurred - {ex.Message}");
            }
        }

        /// <summary>
        /// Event handler khi form được load
        /// </summary>
        private void FrmProductVariantIdentifierCreateQrCode_Load(object sender, EventArgs e)
        {
            try
            {
                // Load ProductVariantFullName khi form được load
                LoadProductVariantInfo();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"FrmProductVariantIdentifierCreateQrCode_Load: Exception occurred - {ex.Message}");
                MsgBox.ShowError($"Lỗi tải thông tin sản phẩm: {ex.Message}");
            }
        }

        /// <summary>
        /// Khởi tạo các event handlers cho bar buttons
        /// </summary>
        private void InitializeBarButtonEvents()
        {
            try
            {
                TaoQcCodeBarButtonItem.ItemClick += TaoQcCodeBarButtonItem_ItemClick;
                SaveBarButtonItem.ItemClick += SaveBarButtonItem_ItemClick;
                InQrCodeBarButtonItem.ItemClick += InQrCodeBarButtonItem_ItemClick;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"InitializeBarButtonEvents: Exception occurred - {ex.Message}");
            }
        }

        /// <summary>
        /// Xử lý sự kiện khi click nút "Tạo QR"
        /// </summary>
        private void TaoQcCodeBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                GenerateQrCode();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"TaoQcCodeBarButtonItem_ItemClick: Exception occurred - {ex.Message}");
                MsgBox.ShowError($"Lỗi tạo QR code: {ex.Message}");
            }
        }

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
                    MsgBox.ShowWarning("Chưa có mã QR code để in. Vui lòng tạo QR code trước.");
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
                System.Diagnostics.Debug.WriteLine($"InQrCodeBarButtonItem_ItemClick: Exception occurred - {ex.Message}");
                MsgBox.ShowError($"Lỗi in QR code: {ex.Message}");
            }
        }

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

                // Tạo PrintDocument
                using (var printDocument = new PrintDocument())
                {
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

                            // Tính toán vị trí để căn giữa QR code trên trang
                            var pageBounds = e.PageBounds;
                            var imageSize = qrImage.Size;

                            // Scale QR code để vừa với kích thước trang đã thiết lập (giữ nguyên tỷ lệ)
                            // Sử dụng toàn bộ không gian trang (không có margin vì đã set margins = 0)
                            float scaleX = (float)pageBounds.Width / imageSize.Width;
                            float scaleY = (float)pageBounds.Height / imageSize.Height;
                            float scale = Math.Min(scaleX, scaleY); // Scale để vừa với trang

                            var scaledWidth = imageSize.Width * scale;
                            var scaledHeight = imageSize.Height * scale;

                            // Căn giữa
                            var x = (pageBounds.Width - scaledWidth) / 2;
                            var y = (pageBounds.Height - scaledHeight) / 2;

                            // Vẽ QR code
                            var rect = new RectangleF(x, y, scaledWidth, scaledHeight);
                            e.Graphics.DrawImage(qrImage, rect);

                            // Vẽ thông tin sản phẩm phía dưới QR code (nếu có)
                            if (_selectedDto != null && !string.IsNullOrWhiteSpace(_selectedDto.ProductVariantFullName))
                            {
                                using (var font = new Font("Arial", 10, FontStyle.Regular))
                                using (var brush = new SolidBrush(Color.Black))
                                {
                                    var textY = y + scaledHeight + 20;
                                    var textRect = new RectangleF(0, textY, pageBounds.Width, 50);
                                    var sf = new StringFormat
                                    {
                                        Alignment = StringAlignment.Center,
                                        LineAlignment = StringAlignment.Near
                                    };
                                    e.Graphics.DrawString(_selectedDto.ProductVariantFullName, font, brush, textRect, sf);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"PrintPage: Exception occurred - {ex.Message}");
                            e.Cancel = true;
                        }
                    };

                    // In
                    printDocument.Print();
                    MsgBox.ShowSuccess("Đã gửi lệnh in QR code thành công.");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"PrintQrCode: Exception occurred - {ex.Message}");
                MsgBox.ShowError($"Lỗi in QR code: {ex.Message}");
            }
        }

        /// <summary>
        /// Xử lý sự kiện khi click nút "Lưu"
        /// </summary>
        private void SaveBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                // Validate dữ liệu trước khi lưu
                var (isValid, errorMessage) = ValidateDataBeforeSave();
                if (!isValid)
                {
                    MsgBox.ShowWarning(errorMessage);
                    return;
                }

                // Lấy danh sách items từ grid
                var items = GetIdentifierItems();
                if (items == null || items.Count == 0)
                {
                    MsgBox.ShowWarning("Chưa có dữ liệu để lưu. Vui lòng thêm ít nhất một định danh.");
                    return;
                }

                // Convert items sang DTOs và lưu
                int successCount = 0;
                int errorCount = 0;
                var errorMessages = new List<string>();

                foreach (var item in items)
                {
                    try
                    {
                        var dto = ConvertItemToDto(item);
                        if (dto == null)
                        {
                            errorCount++;
                            errorMessages.Add($"Dòng {items.IndexOf(item) + 1}: Không thể convert sang DTO");
                            continue;
                        }

                        // Lưu vào database
                        var result = _productVariantIdentifierBll.SaveOrUpdate(dto);
                        if (result != null)
                        {
                            successCount++;
                        }
                        else
                        {
                            errorCount++;
                            errorMessages.Add($"Dòng {items.IndexOf(item) + 1}: Lưu thất bại");
                        }
                    }
                    catch (Exception ex)
                    {
                        errorCount++;
                        errorMessages.Add($"Dòng {items.IndexOf(item) + 1}: {ex.Message}");
                        System.Diagnostics.Debug.WriteLine($"SaveBarButtonItem_ItemClick: Error saving item {item.Id} - {ex.Message}");
                    }
                }

                // Hiển thị kết quả
                if (errorCount == 0)
                {
                    MsgBox.ShowSuccess($"Đã lưu thành công {successCount} định danh.");
                    // Reset cache để reload dữ liệu mới
                    ResetIdsCache();
                }
                else if (successCount > 0)
                {
                    var errorText = string.Join("\n", errorMessages);
                    MsgBox.ShowWarning($"Đã lưu thành công {successCount} định danh.\nCó {errorCount} lỗi:\n{errorText}");
                    ResetIdsCache();
                }
                else
                {
                    var errorText = string.Join("\n", errorMessages);
                    MsgBox.ShowError($"Lưu thất bại. Các lỗi:\n{errorText}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"SaveBarButtonItem_ItemClick: Exception occurred - {ex.Message}");
                MsgBox.ShowError($"Lỗi lưu dữ liệu: {ex.Message}");
            }
        }

        /// <summary>
        /// Lấy Description từ ProductVariantIdentifierEnum value
        /// </summary>
        /// <param name="identifierType">Giá trị enum</param>
        /// <returns>Description hoặc tên enum nếu không có Description</returns>
        private string GetProductVariantIdentifierDescription(ProductVariantIdentifierEnum identifierType)
        {
            try
            {
                return ApplicationEnumUtils.GetDescription(identifierType);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetProductVariantIdentifierDescription: Exception occurred for {identifierType} - {ex.Message}");
                return identifierType.ToString();
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
                System.Diagnostics.Debug.WriteLine($"InitializeQrCodePictureEdit: Exception occurred - {ex.Message}");
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

        /// <summary>
        /// Khởi tạo ProductVariantIdentifierGrid
        /// </summary>
        private void InitializeProductVariantIdentifierGrid()
        {
            try
            {
                // Khởi tạo binding source với danh sách rỗng
                productVariantIdentifierItemBindingSource.DataSource = new List<ProductVariantIdentifierItem>();

                // Load enum vào RepositoryItemComboBox
                LoadProductVariantIdentifierEnumRepositoryComboBox();

                // Setup events
                InitializeGridEvents();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"InitializeProductVariantIdentifierGrid: Exception occurred - {ex.Message}");
                MsgBox.ShowError($"Lỗi khởi tạo grid: {ex.Message}");
            }
        }

        /// <summary>
        /// Khởi tạo các event handlers cho grid
        /// </summary>
        private void InitializeGridEvents()
        {
            try
            {
                ProductVariantIdentifierGridView.CellValueChanged += ProductVariantIdentifierGridView_CellValueChanged;
                ProductVariantIdentifierGridView.InitNewRow += ProductVariantIdentifierGridView_InitNewRow;
                ProductVariantIdentifierGridView.RowDeleted += ProductVariantIdentifierGridView_RowDeleted;
                ProductVariantIdentifierGridView.ValidateRow += ProductVariantIdentifierGridView_ValidateRow;
                ProductVariantIdentifierGridView.ValidatingEditor += ProductVariantIdentifierGridView_ValidatingEditor;
                ProductVariantIdentifierGridView.CustomDrawRowIndicator += ProductVariantIdentifierGridView_CustomDrawRowIndicator;
                ProductVariantIdentifierGridView.KeyDown += ProductVariantIdentifierGridView_KeyDown;
                ProductVariantIdentifierGridView.ShownEditor += ProductVariantIdentifierGridView_ShownEditor;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"InitializeGridEvents: Exception occurred - {ex.Message}");
            }
        }

        /// <summary>
        /// Load danh sách ProductVariantIdentifierEnum vào RepositoryItemComboBox
        /// </summary>
        private void LoadProductVariantIdentifierEnumRepositoryComboBox()
        {
            try
            {
                ProductVariantIdentifierEnumComboBox.Items.Clear();

                // Load tất cả các giá trị enum
                foreach (ProductVariantIdentifierEnum value in Enum.GetValues(typeof(ProductVariantIdentifierEnum)))
                {
                    int index = ApplicationEnumUtils.GetValue(value);
                    ProductVariantIdentifierEnumComboBox.Items.Insert(index, value);
                }

                // Cấu hình ComboBox
                ProductVariantIdentifierEnumComboBox.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                ProductVariantIdentifierEnumComboBox.ShowDropDown = DevExpress.XtraEditors.Controls.ShowDropDown.SingleClick;

                // Sử dụng CustomDisplayText để hiển thị Description thay vì tên enum
                ProductVariantIdentifierEnumComboBox.CustomDisplayText += ProductVariantIdentifierEnumRepositoryComboBox_CustomDisplayText;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LoadProductVariantIdentifierEnumRepositoryComboBox: Exception occurred - {ex.Message}");
            }
        }

        /// <summary>
        /// Event handler để hiển thị Description thay vì tên enum trong RepositoryItemComboBox
        /// </summary>
        private void ProductVariantIdentifierEnumRepositoryComboBox_CustomDisplayText(object sender, DevExpress.XtraEditors.Controls.CustomDisplayTextEventArgs e)
        {
            try
            {
                if (e.Value is ProductVariantIdentifierEnum enumValue)
                {
                    // Lấy Description từ helper method
                    e.DisplayText = GetProductVariantIdentifierDescription(enumValue);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ProductVariantIdentifierEnumRepositoryComboBox_CustomDisplayText: Exception occurred - {ex.Message}");
                // Nếu có lỗi, hiển thị tên enum mặc định
                if (e.Value is ProductVariantIdentifierEnum enumValue)
                {
                    e.DisplayText = enumValue.ToString();
                }
            }
        }

        #region ========== GRIDVIEW EVENT HANDLERS ==========

        /// <summary>
        /// Custom draw row indicator để hiển thị số thứ tự
        /// </summary>
        private void ProductVariantIdentifierGridView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            GridViewHelper.CustomDrawRowIndicator(ProductVariantIdentifierGridView, e);
        }

        /// <summary>
        /// Xử lý sự kiện khi giá trị cell thay đổi trong GridView
        /// </summary>
        private void ProductVariantIdentifierGridView_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                var rowHandle = e.RowHandle;

                // Lấy row data từ GridView (bao gồm cả new row)
                var rowData = ProductVariantIdentifierGridView.GetRow(rowHandle) as ProductVariantIdentifierItem;
                if (rowData == null)
                {
                    System.Diagnostics.Debug.WriteLine($"CellValueChanged: Row data is null, RowHandle={rowHandle}");
                    return;
                }

                // Nếu thay đổi là IdentifierType
                if (e.Column != null && e.Column.FieldName == "IdentifierType")
                {
                    // Nếu chọn ID, tự động tạo GUID
                    if (rowData.IdentifierType == ProductVariantIdentifierEnum.ID)
                    {
                        // Chỉ tạo GUID nếu Value chưa có hoặc đang rỗng
                        if (string.IsNullOrWhiteSpace(rowData.Value))
                        {
                            // Tự động tạo GUID mới làm giá trị (đảm bảo không trùng)
                            rowData.Value = GenerateUniqueGuidAsString();

                            // Refresh để hiển thị giá trị mới
                            ProductVariantIdentifierGridView.RefreshRow(rowHandle);
                            
                            // Focus vào cột Value để user có thể thấy giá trị đã được tạo
                            var valueColumn = ProductVariantIdentifierGridView.Columns["Value"];
                            if (valueColumn != null)
                            {
                                ProductVariantIdentifierGridView.FocusedColumn = valueColumn;
                            }
                        }
                    }
                    else
                    {
                        // Với các loại khác (SerialNumber, Barcode, etc.), focus vào cột Value để user nhập
                        if (string.IsNullOrWhiteSpace(rowData.Value))
                        {
                            var valueColumn = ProductVariantIdentifierGridView.Columns["Value"];
                            if (valueColumn != null)
                            {
                                // Delay một chút để đảm bảo grid đã refresh
                                Timer timer = new Timer();
                                timer.Interval = 100;
                                timer.Tick += (s, args) =>
                                {
                                    timer.Stop();
                                    timer.Dispose();
                                    ProductVariantIdentifierGridView.FocusedColumn = valueColumn;
                                    ProductVariantIdentifierGridView.ShowEditor();
                                };
                                timer.Start();
                            }
                        }
                    }
                }

                // Refresh grid để hiển thị thay đổi (chỉ nếu không phải new row)
                if (rowHandle >= 0)
                {
                    ProductVariantIdentifierGridView.RefreshRow(rowHandle);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"CellValueChanged: Exception occurred - {ex.Message}");
                MsgBox.ShowError($"Lỗi xử lý thay đổi cell: {ex.Message}");
            }
        }

        /// <summary>
        /// Xử lý sự kiện khi thêm dòng mới
        /// </summary>
        private void ProductVariantIdentifierGridView_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            try
            {
                var rowData = ProductVariantIdentifierGridView.GetRow(e.RowHandle) as ProductVariantIdentifierItem;
                if (rowData == null)
                {
                    System.Diagnostics.Debug.WriteLine($"InitNewRow: Row data is null, RowHandle={e.RowHandle}");
                    return;
                }

                // Gán ID mới nếu chưa có
                if (rowData.Id == Guid.Empty)
                {
                    rowData.Id = Guid.NewGuid();
                }

                // Set mặc định cho IdentifierType
                if (rowData.IdentifierType == default(ProductVariantIdentifierEnum))
                {
                    rowData.IdentifierType = ProductVariantIdentifierEnum.SerialNumber;
                }

                // Nếu IdentifierType là ID, tự động tạo GUID cho Value
                if (rowData.IdentifierType == ProductVariantIdentifierEnum.ID && string.IsNullOrWhiteSpace(rowData.Value))
                {
                    rowData.Value = GenerateUniqueGuidAsString();
                    // Refresh để hiển thị giá trị mới
                    ProductVariantIdentifierGridView.RefreshRow(e.RowHandle);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"InitNewRow: Exception occurred - {ex.Message}");
                MsgBox.ShowError($"Lỗi thêm dòng: {ex.Message}");
            }
        }

        /// <summary>
        /// Xử lý sự kiện khi xóa dòng
        /// </summary>
        private void ProductVariantIdentifierGridView_RowDeleted(object sender, RowDeletedEventArgs rowDeletedEventArgs)
        {
            try
            {
                // Có thể thêm logic xử lý khi xóa dòng ở đây
                System.Diagnostics.Debug.WriteLine("RowDeleted: Row deleted successfully");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"RowDeleted: Exception occurred - {ex.Message}");
                MsgBox.ShowError($"Lỗi xóa dòng: {ex.Message}");
            }
        }

        /// <summary>
        /// Xử lý sự kiện validate editor (trước khi commit giá trị)
        /// </summary>
        private void ProductVariantIdentifierGridView_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            try
            {
                var column = ProductVariantIdentifierGridView.FocusedColumn;
                var fieldName = column?.FieldName;

                if (string.IsNullOrEmpty(fieldName)) return;

                switch (fieldName)
                {
                    // Validate IdentifierType: Phải chọn loại định danh
                    case "IdentifierType":
                        ValidateIdentifierType(e);
                        break;
                    // Validate Value: Phải nhập giá trị
                    case "Value":
                        ValidateValue(e);
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ValidatingEditor: Exception occurred - {ex.Message}");
                e.ErrorText = $"Lỗi validate: {ex.Message}";
                e.Valid = false;
            }
        }

        /// <summary>
        /// Validate row data (sau khi commit giá trị)
        /// </summary>
        private void ProductVariantIdentifierGridView_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            try
            {
                var rowData = e.Row as ProductVariantIdentifierItem;
                if (rowData == null)
                {
                    System.Diagnostics.Debug.WriteLine("ValidateRow: Row data is null");
                    e.Valid = false;
                    e.ErrorText = "Dữ liệu không hợp lệ";
                    return;
                }

                // Validate bằng DataAnnotations
                var validationResults = new List<ValidationResult>();
                var validationContext = new ValidationContext(rowData);
                var isValid = Validator.TryValidateObject(rowData, validationContext, validationResults, true);

                if (!isValid)
                {
                    var errors = string.Join("\n", validationResults.Select(r => r.ErrorMessage));
                    System.Diagnostics.Debug.WriteLine($"ValidateRow: DataAnnotations validation failed, Errors={errors}");
                    e.Valid = false;
                    e.ErrorText = errors;
                    return;
                }

                // Validate business rules
                if (string.IsNullOrWhiteSpace(rowData.Value))
                {
                    System.Diagnostics.Debug.WriteLine("ValidateRow: Value is empty");
                    e.Valid = false;
                    e.ErrorText = "Vui lòng nhập giá trị định danh";
                    return;
                }

                e.Valid = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ValidateRow: Exception occurred - {ex.Message}");
                e.Valid = false;
                e.ErrorText = $"Lỗi validate: {ex.Message}";
            }
        }

        /// <summary>
        /// Event handler xử lý phím tắt trong GridView
        /// </summary>
        private void ProductVariantIdentifierGridView_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                var gridView = ProductVariantIdentifierGridView;
                if (gridView == null) return;

                switch (e.KeyCode)
                {
                    case Keys.Insert:
                        // Insert: Thêm dòng mới
                        if (!e.Control && !e.Shift && !e.Alt)
                        {
                            e.Handled = true;
                            gridView.AddNewRow();
                            // Focus vào cột IdentifierType
                            var identifierTypeColumn = gridView.Columns["IdentifierType"];
                            if (identifierTypeColumn != null)
                            {
                                gridView.FocusedColumn = identifierTypeColumn;
                            }
                        }
                        break;

                    case Keys.Delete:
                        // Delete: Xóa dòng được chọn
                        if (!e.Control && !e.Shift && !e.Alt)
                        {
                            e.Handled = true;
                            var selectedRows = gridView.GetSelectedRows();
                            if (selectedRows != null && selectedRows.Length > 0)
                            {
                                foreach (var rowHandle in selectedRows)
                                {
                                    if (rowHandle >= 0)
                                    {
                                        gridView.DeleteRow(rowHandle);
                                    }
                                }
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"KeyDown: Exception occurred - {ex.Message}");
                MsgBox.ShowError($"Lỗi xử lý phím tắt: {ex.Message}");
            }
        }

        /// <summary>
        /// Xử lý sự kiện khi editor được hiển thị
        /// </summary>
        private void ProductVariantIdentifierGridView_ShownEditor(object sender, EventArgs e)
        {
            try
            {
                var gridView = ProductVariantIdentifierGridView;
                if (gridView == null) return;

                var rowHandle = gridView.FocusedRowHandle;
                var column = gridView.FocusedColumn;

                // Chỉ xử lý khi đang edit cột IdentifierType
                if (column != null && column.FieldName == "IdentifierType")
                {
                    var rowData = gridView.GetRow(rowHandle) as ProductVariantIdentifierItem;
                    if (rowData != null && rowData.IdentifierType == ProductVariantIdentifierEnum.ID)
                    {
                        // Nếu đã chọn ID nhưng chưa có Value, tạo GUID
                        if (string.IsNullOrWhiteSpace(rowData.Value))
                        {
                            rowData.Value = GenerateUniqueGuidAsString();
                            gridView.RefreshRow(rowHandle);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ShownEditor: Exception occurred - {ex.Message}");
            }
        }

        /// <summary>
        /// Validate IdentifierType: Phải chọn loại định danh
        /// </summary>
        private void ValidateIdentifierType(DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            if (e.Value == null)
            {
                e.ErrorText = "Vui lòng chọn kiểu định danh";
                e.Valid = false;
                return;
            }

            if (!(e.Value is ProductVariantIdentifierEnum))
            {
                e.ErrorText = "Kiểu định danh không hợp lệ";
                e.Valid = false;
                return;
            }

            e.Valid = true;
        }

        /// <summary>
        /// Validate Value: Phải nhập giá trị
        /// </summary>
        private void ValidateValue(DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            if (e.Value == null || string.IsNullOrWhiteSpace(e.Value.ToString()))
            {
                e.ErrorText = "Vui lòng nhập giá trị định danh";
                e.Valid = false;
                return;
            }

            var value = e.Value.ToString().Trim();
            if (value.Length > 500)
            {
                e.ErrorText = "Giá trị định danh không được vượt quá 500 ký tự";
                e.Valid = false;
                return;
            }

            e.Valid = true;
        }

        #endregion

        #region ========== PUBLIC METHODS ==========

        /// <summary>
        /// Lấy danh sách ProductVariantIdentifierItem từ BindingSource
        /// Tham khảo cách xử lý từ UcNhapHangThuongMaiDetail.GetDetails()
        /// </summary>
        public List<ProductVariantIdentifierItem> GetIdentifierItems()
        {
            try
            {
                var items = new List<ProductVariantIdentifierItem>();

                foreach (var item in productVariantIdentifierItemBindingSource)
                {
                    if (item is ProductVariantIdentifierItem identifierItem)
                    {
                        items.Add(identifierItem);
                    }
                }

                return items;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetIdentifierItems: Exception occurred - {ex.Message}");
                return new List<ProductVariantIdentifierItem>();
            }
        }

        /// <summary>
        /// Load danh sách ProductVariantIdentifierItem vào grid
        /// </summary>
        public void LoadIdentifierItems(List<ProductVariantIdentifierItem> items)
        {
            try
            {
                if (items == null)
                {
                    items = new List<ProductVariantIdentifierItem>();
                }

                productVariantIdentifierItemBindingSource.DataSource = items;
                productVariantIdentifierItemBindingSource.ResetBindings(false);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LoadIdentifierItems: Exception occurred - {ex.Message}");
                MsgBox.ShowError($"Lỗi load dữ liệu: {ex.Message}");
            }
        }

        /// <summary>
        /// Xóa tất cả các dòng trong grid
        /// </summary>
        public void ClearIdentifierItems()
        {
            try
            {
                productVariantIdentifierItemBindingSource.DataSource = new List<ProductVariantIdentifierItem>();
                productVariantIdentifierItemBindingSource.ResetBindings(false);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ClearIdentifierItems: Exception occurred - {ex.Message}");
            }
        }

        #endregion

        #region ========== GUID GENERATION ==========

        /// <summary>
        /// Load cache danh sách ID đã có trong database
        /// </summary>
        private void LoadExistingIdsCache()
        {
            if (_isIdsCacheLoaded)
            {
                return;
            }

            try
            {
                _existingIdsCache = new HashSet<Guid>();

                // Lấy danh sách ID từ database
                var allIdentifiers = _productVariantIdentifierBll.GetAll();
                foreach (var identifier in allIdentifiers)
                {
                    _existingIdsCache.Add(identifier.Id);
                }

                _isIdsCacheLoaded = true;
                System.Diagnostics.Debug.WriteLine($"LoadExistingIdsCache: Đã load {_existingIdsCache.Count} ID từ database");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LoadExistingIdsCache: Exception occurred - {ex.Message}");
                // Nếu có lỗi, khởi tạo empty set để tiếp tục
                _existingIdsCache = new HashSet<Guid>();
                _isIdsCacheLoaded = true;
            }
        }

        /// <summary>
        /// Lấy danh sách ID từ grid hiện tại (chưa lưu)
        /// </summary>
        /// <returns>HashSet chứa các ID trong grid</returns>
        private HashSet<Guid> GetCurrentGridIds()
        {
            var gridIds = new HashSet<Guid>();

            try
            {
                foreach (var item in productVariantIdentifierItemBindingSource)
                {
                    if (item is ProductVariantIdentifierItem identifierItem)
                    {
                        // Lấy ID từ Value nếu IdentifierType là ID
                        if (identifierItem.IdentifierType == ProductVariantIdentifierEnum.ID &&
                            !string.IsNullOrWhiteSpace(identifierItem.Value) &&
                            Guid.TryParse(identifierItem.Value, out Guid valueGuid))
                        {
                            gridIds.Add(valueGuid);
                        }

                        // Cũng thêm Id của item vào để tránh trùng
                        if (identifierItem.Id != Guid.Empty)
                        {
                            gridIds.Add(identifierItem.Id);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetCurrentGridIds: Exception occurred - {ex.Message}");
            }

            return gridIds;
        }

        /// <summary>
        /// Tạo GUID mới đảm bảo không trùng với ID có sẵn trong ProductVariantIdentifier
        /// </summary>
        /// <returns>GUID dưới dạng string</returns>
        private string GenerateUniqueGuidAsString()
        {
            // Load cache nếu chưa load
            LoadExistingIdsCache();

            // Lấy danh sách ID từ grid hiện tại
            var gridIds = GetCurrentGridIds();

            // Hợp nhất tất cả ID đã có
            var allExistingIds = new HashSet<Guid>(_existingIdsCache);
            foreach (var gridId in gridIds)
            {
                allExistingIds.Add(gridId);
            }

            // Tạo GUID mới cho đến khi không trùng
            Guid newGuid;
            int maxAttempts = 100; // Giới hạn số lần thử để tránh vòng lặp vô hạn
            int attempts = 0;

            do
            {
                newGuid = Guid.NewGuid();
                attempts++;

                if (attempts >= maxAttempts)
                {
                    System.Diagnostics.Debug.WriteLine("GenerateUniqueGuidAsString: Đã đạt giới hạn số lần thử, sử dụng GUID cuối cùng");
                    break;
                }
            }
            while (allExistingIds.Contains(newGuid));

            // Thêm GUID mới vào cache để tránh trùng trong cùng session
            allExistingIds.Add(newGuid);
            _existingIdsCache.Add(newGuid);

            System.Diagnostics.Debug.WriteLine($"GenerateUniqueGuidAsString: Đã tạo GUID mới sau {attempts} lần thử");
            return newGuid.ToString();
        }

        /// <summary>
        /// Reset cache ID (dùng khi cần reload từ database)
        /// </summary>
        public void ResetIdsCache()
        {
            _isIdsCacheLoaded = false;
            _existingIdsCache = null;
        }

        #endregion

        #region ========== LOAD PRODUCT VARIANT ==========

        /// <summary>
        /// Load và hiển thị ProductVariantFullName dựa vào ProductVariantId
        /// </summary>
        private void LoadProductVariantInfo()
        {
            try
            {
                // Kiểm tra ProductVariantId
                ProductVariantFullNameSimpleLabelItem.Text = _selectedDto.ProductVariantFullName;

                SoLuongNhapXuatBarStaticItem.Caption =
                    $@"Số lượng nhập/xuất: <color='red'><b> {(_selectedDto.StockInQty > 0 ? _selectedDto.StockInQty : _selectedDto.StockOutQty)}</color></b>";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LoadProductVariantInfo: Exception occurred - {ex.Message}");
                SetProductVariantDisplayText($"Lỗi tải thông tin: {ex.Message}");
            }
        }

        /// <summary>
        /// Set text cho ProductVariant display control (hỗ trợ cả HtmlContentControl và MemoEdit)
        /// </summary>
        /// <param name="text">Text hoặc HTML content cần hiển thị</param>
        private void SetProductVariantDisplayText(string text)
        {
            try
            {
                // Kiểm tra xem có HtmlContentControl không (nếu đã thay đổi trong Designer)

                if (Controls.Find("ProductVariantFullNameHtmlContentControl", true)
                        .FirstOrDefault() is HtmlContentControl htmlContentControl)
                {
                    // Sử dụng HtmlContentControl - render HTML tốt nhất
                    htmlContentControl.HtmlTemplate.Template = text;
                }
                else if (ProductVariantFullNameSimpleLabelItem != null)
                {
                    // Fallback: sử dụng MemoEdit
                    ProductVariantFullNameSimpleLabelItem.Text = text;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"SetProductVariantDisplayText: Exception occurred - {ex.Message}");
            }
        }


        #endregion

        #region ========== SAVE DATA ==========

        /// <summary>
        /// Validate dữ liệu trước khi lưu (bao gồm kiểm tra trùng lặp với database)
        /// </summary>
        /// <returns>Tuple (isValid, errorMessage)</returns>
        private (bool isValid, string errorMessage) ValidateDataBeforeSave()
        {
            try
            {
                // Validate cơ bản trước (giống như ValidateDataBeforeGenerateQrCode)
                var (isValid, errorMessage) = ValidateDataBeforeGenerateQrCode();
                if (!isValid)
                {
                    return (false, errorMessage);
                }

                // Lấy danh sách items từ grid
                var items = GetIdentifierItems();
                if (items == null || items.Count == 0)
                {
                    return (false, "Chưa có dữ liệu để lưu. Vui lòng thêm ít nhất một định danh.");
                }

                // Kiểm tra trùng lặp với database
                LoadExistingIdsCache();
                
                // Lấy tất cả identifier hiện có trong database để kiểm tra trùng lặp
                var existingIdentifiers = _productVariantIdentifierBll.GetAll();
                
                // Kiểm tra từng item xem có trùng với database không
                foreach (var item in items)
                {
                    var dto = ConvertItemToDto(item);
                    if (dto == null) continue;

                    // Kiểm tra trùng lặp theo từng loại identifier
                    var duplicate = existingIdentifiers.FirstOrDefault(existing =>
                    {
                        // Bỏ qua chính nó nếu đang update
                        if (existing.Id == dto.Id && dto.Id != Guid.Empty)
                        {
                            return false;
                        }

                        // Kiểm tra trùng theo từng loại identifier
                        switch (item.IdentifierType)
                        {
                            case ProductVariantIdentifierEnum.SerialNumber:
                                return !string.IsNullOrWhiteSpace(existing.SerialNumber) &&
                                       existing.SerialNumber == item.Value &&
                                       existing.ProductVariantId == dto.ProductVariantId;
                            case ProductVariantIdentifierEnum.Barcode:
                                return !string.IsNullOrWhiteSpace(existing.Barcode) &&
                                       existing.Barcode == item.Value &&
                                       existing.ProductVariantId == dto.ProductVariantId;
                            case ProductVariantIdentifierEnum.QRCode:
                                return !string.IsNullOrWhiteSpace(existing.QRCode) &&
                                       existing.QRCode == item.Value &&
                                       existing.ProductVariantId == dto.ProductVariantId;
                            case ProductVariantIdentifierEnum.SKU:
                                return !string.IsNullOrWhiteSpace(existing.SKU) &&
                                       existing.SKU == item.Value &&
                                       existing.ProductVariantId == dto.ProductVariantId;
                            case ProductVariantIdentifierEnum.RFID:
                                return !string.IsNullOrWhiteSpace(existing.RFID) &&
                                       existing.RFID == item.Value &&
                                       existing.ProductVariantId == dto.ProductVariantId;
                            case ProductVariantIdentifierEnum.MACAddress:
                                return !string.IsNullOrWhiteSpace(existing.MACAddress) &&
                                       existing.MACAddress == item.Value &&
                                       existing.ProductVariantId == dto.ProductVariantId;
                            case ProductVariantIdentifierEnum.IMEI:
                                return !string.IsNullOrWhiteSpace(existing.IMEI) &&
                                       existing.IMEI == item.Value &&
                                       existing.ProductVariantId == dto.ProductVariantId;
                            case ProductVariantIdentifierEnum.AssetTag:
                                return !string.IsNullOrWhiteSpace(existing.AssetTag) &&
                                       existing.AssetTag == item.Value &&
                                       existing.ProductVariantId == dto.ProductVariantId;
                            case ProductVariantIdentifierEnum.LicenseKey:
                                return !string.IsNullOrWhiteSpace(existing.LicenseKey) &&
                                       existing.LicenseKey == item.Value &&
                                       existing.ProductVariantId == dto.ProductVariantId;
                            case ProductVariantIdentifierEnum.UPC:
                                return !string.IsNullOrWhiteSpace(existing.UPC) &&
                                       existing.UPC == item.Value &&
                                       existing.ProductVariantId == dto.ProductVariantId;
                            case ProductVariantIdentifierEnum.EAN:
                                return !string.IsNullOrWhiteSpace(existing.EAN) &&
                                       existing.EAN == item.Value &&
                                       existing.ProductVariantId == dto.ProductVariantId;
                            case ProductVariantIdentifierEnum.ID:
                                return !string.IsNullOrWhiteSpace(existing.ID) &&
                                       existing.ID == item.Value &&
                                       existing.ProductVariantId == dto.ProductVariantId;
                            case ProductVariantIdentifierEnum.OtherIdentifier:
                                return !string.IsNullOrWhiteSpace(existing.OtherIdentifier) &&
                                       existing.OtherIdentifier == item.Value &&
                                       existing.ProductVariantId == dto.ProductVariantId;
                            default:
                                return false;
                        }
                    });

                    if (duplicate != null)
                    {
                        var identifierTypeName = GetProductVariantIdentifierDescription(item.IdentifierType);
                        return (false, $"Giá trị \"{item.Value}\" của loại định danh \"{identifierTypeName}\" đã tồn tại trong hệ thống. Vui lòng kiểm tra lại.");
                    }
                }

                // Kiểm tra phải có hình mã QR Code trước khi lưu
                if (QrCodePictureEdit == null || QrCodePictureEdit.Image == null)
                {
                    return (false, "Chưa có hình mã QR Code. Vui lòng tạo QR Code trước khi lưu.");
                }

                // Kiểm tra phải có dữ liệu thông số của QR Code
                var qrCodePayload = BuildQrCodePayload();
                if (string.IsNullOrWhiteSpace(qrCodePayload))
                {
                    return (false, "Chưa có dữ liệu thông số của QR Code. Vui lòng tạo QR Code trước khi lưu.");
                }

                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ValidateDataBeforeSave: Exception occurred - {ex.Message}");
                return (false, $"Lỗi kiểm tra dữ liệu: {ex.Message}");
            }
        }

        /// <summary>
        /// Convert ProductVariantIdentifierItem sang ProductVariantIdentifierDto
        /// Cập nhật luôn hình ảnh và giá trị QR Code để lưu vào database
        /// </summary>
        /// <param name="item">ProductVariantIdentifierItem</param>
        /// <returns>ProductVariantIdentifierDto hoặc null nếu không thể convert</returns>
        private ProductVariantIdentifierDto ConvertItemToDto(ProductVariantIdentifierItem item)
        {
            try
            {
                if (item == null)
                {
                    return null;
                }

                var dto = new ProductVariantIdentifierDto
                {
                    Id = item.Id != Guid.Empty ? item.Id : Guid.NewGuid(),
                    ProductVariantId = _selectedDto.ProductVariantId,
                };

                // Map giá trị vào property tương ứng theo IdentifierType
                switch (item.IdentifierType)
                {
                    case ProductVariantIdentifierEnum.SerialNumber:
                        dto.SerialNumber = item.Value;
                        break;
                    case ProductVariantIdentifierEnum.Barcode:
                        dto.Barcode = item.Value;
                        break;
                    case ProductVariantIdentifierEnum.QRCode:
                        dto.QRCode = item.Value;
                        break;
                    case ProductVariantIdentifierEnum.SKU:
                        dto.SKU = item.Value;
                        break;
                    case ProductVariantIdentifierEnum.RFID:
                        dto.RFID = item.Value;
                        break;
                    case ProductVariantIdentifierEnum.MACAddress:
                        dto.MACAddress = item.Value;
                        break;
                    case ProductVariantIdentifierEnum.IMEI:
                        dto.IMEI = item.Value;
                        break;
                    case ProductVariantIdentifierEnum.AssetTag:
                        dto.AssetTag = item.Value;
                        break;
                    case ProductVariantIdentifierEnum.LicenseKey:
                        dto.LicenseKey = item.Value;
                        break;
                    case ProductVariantIdentifierEnum.UPC:
                        dto.UPC = item.Value;
                        break;
                    case ProductVariantIdentifierEnum.EAN:
                        dto.EAN = item.Value;
                        break;
                    case ProductVariantIdentifierEnum.ID:
                        dto.ID = item.Value;
                        break;
                    case ProductVariantIdentifierEnum.OtherIdentifier:
                        dto.OtherIdentifier = item.Value;
                        break;
                    default:
                        System.Diagnostics.Debug.WriteLine($"ConvertItemToDto: Unknown IdentifierType {item.IdentifierType}");
                        return null;
                }

                // Cập nhật giá trị QR Code từ payload (cho tất cả items)
                var qrCodePayload = BuildQrCodePayload();
                if (!string.IsNullOrWhiteSpace(qrCodePayload))
                {
                    // Set QRCode property với giá trị payload (nếu chưa có)
                    if (string.IsNullOrWhiteSpace(dto.QRCode))
                    {
                        dto.QRCode = qrCodePayload;
                    }
                }

                // Cập nhật hình ảnh QR Code nếu có
                if (QrCodePictureEdit != null && QrCodePictureEdit.Image != null)
                {
                    try
                    {
                        // Convert image sang byte array
                        byte[] imageBytes = ImageToByteArray(QrCodePictureEdit.Image);
                        if (imageBytes != null && imageBytes.Length > 0)
                        {
                            // Lưu image vào DTO (có thể cần property để lưu byte array)
                            // Hoặc lưu vào temp file và set path
                            // Tạm thời, BLL sẽ xử lý việc lưu file image
                            // Ở đây chỉ đảm bảo có image data
                            
                            // Generate file name cho QR code image
                            var fileName = $"QRCode_{dto.Id}_{DateTime.Now:yyyyMMddHHmmss}.png";
                            dto.QRCodeImageFileName = fileName;
                            
                            // Note: Việc lưu file image vào NAS sẽ được xử lý ở BLL layer
                            // DTO chỉ chứa metadata về image
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"ConvertItemToDto: Error converting QR code image - {ex.Message}");
                        // Không throw exception, chỉ log để không block việc lưu dữ liệu khác
                    }
                }

                return dto;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ConvertItemToDto: Exception occurred - {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Convert Image sang byte array
        /// </summary>
        /// <param name="image">Image object</param>
        /// <returns>Byte array hoặc null nếu có lỗi</returns>
        private byte[] ImageToByteArray(Image image)
        {
            try
            {
                if (image == null)
                {
                    return null;
                }

                using (var ms = new System.IO.MemoryStream())
                {
                    // Lưu image dưới dạng PNG
                    image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    return ms.ToArray();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ImageToByteArray: Exception occurred - {ex.Message}");
                return null;
            }
        }

        #endregion

        #region ========== QR CODE GENERATION ==========

        /// <summary>
        /// Tạo chuỗi payload từ các giá trị trong grid (các giá trị cách nhau bởi |)
        /// </summary>
        /// <returns>Chuỗi payload hoặc string.Empty nếu không có dữ liệu</returns>
        private string BuildQrCodePayload()
        {
            try
            {
                var items = GetIdentifierItems();
                if (items == null || items.Count == 0)
                {
                    return string.Empty;
                }

                // Tạo chuỗi: Value|Value|Value|... (chỉ lấy các giá trị, cách nhau bởi |)
                var payloadBuilder = new StringBuilder();
                for (int i = 0; i < items.Count; i++)
                {
                    if (i > 0)
                    {
                        payloadBuilder.Append("|");
                    }
                    payloadBuilder.Append(items[i].Value);
                }

                return payloadBuilder.ToString();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"BuildQrCodePayload: Exception occurred - {ex.Message}");
                return string.Empty;
            }
        }

        /// <summary>
        /// Validate dữ liệu trước khi tạo QR code
        /// Tham khảo cách xử lý từ UcNhapHangThuongMaiDetail.ValidateAll()
        /// </summary>
        /// <returns>Tuple (isValid, errorMessage)</returns>
        private (bool isValid, string errorMessage) ValidateDataBeforeGenerateQrCode()
        {
            try
            {
                // Lấy dữ liệu từ BindingSource (không lấy từ GridView để tránh tính cả new row chưa commit)
                var items = new List<ProductVariantIdentifierItem>();

                foreach (var item in productVariantIdentifierItemBindingSource)
                {
                    if (item is ProductVariantIdentifierItem identifierItem)
                    {
                        if(string.IsNullOrWhiteSpace(identifierItem.Value))
                            return (false, $"Dòng {identifierItem.IdentifierType} chưa có giá trị");

                        items.Add(identifierItem);
                    }
                }

                // Kiểm tra có dữ liệu không
                if (items.Count == 0)
                {
                    return (false, "Chưa có dữ liệu để tạo QR code. Vui lòng thêm ít nhất một định danh.");
                }


                // Kiểm tra có ít nhất một dòng có giá trị hợp lệ để tạo QR code
                var validItems = items
                    .Where(item => item.IdentifierType != default(ProductVariantIdentifierEnum) &&
                                   !string.IsNullOrWhiteSpace(item.Value))
                    .ToList();

                if (validItems.Count == 0)
                {
                    return (false, "Chưa có dữ liệu hợp lệ để tạo QR code. Vui lòng nhập đầy đủ Kiểu định danh và Giá trị cho ít nhất một dòng.");
                }

                // Kiểm tra enum trùng nhau
                var identifierTypeGroups = validItems
                    .GroupBy(item => item.IdentifierType)
                    .Where(g => g.Count() > 1)
                    .ToList();

                if (identifierTypeGroups.Count > 0)
                {
                    var duplicateTypes = identifierTypeGroups
                        .Select(g => GetProductVariantIdentifierDescription(g.Key))
                        .ToList();
                    
                    var duplicateTypesText = string.Join(", ", duplicateTypes);
                    return (false, $"Có loại định danh bị trùng lặp: {duplicateTypesText}. Mỗi loại định danh chỉ được sử dụng một lần.");
                }

                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ValidateDataBeforeGenerateQrCode: Exception occurred - {ex.Message}");
                return (false, $"Lỗi kiểm tra dữ liệu: {ex.Message}");
            }
        }

        /// <summary>
        /// Tạo và hiển thị QR code từ dữ liệu trong grid
        /// </summary>
        private void GenerateQrCode()
        {
            try
            {
                // Validate dữ liệu trước khi tạo QR code
                var (isValid, errorMessage) = ValidateDataBeforeGenerateQrCode();
                if (!isValid)
                {
                    MsgBox.ShowWarning(errorMessage);
                    
                    // Xóa QR code nếu có
                    if (QrCodePictureEdit.Image != null)
                    {
                        QrCodePictureEdit.Image.Dispose();
                        QrCodePictureEdit.Image = null;
                    }
                    
                    // Hiển thị thông báo lỗi
                    if (QrCodeValueMemoEdit != null)
                    {
                        QrCodeValueMemoEdit.Text = $@"Giá trị QR: {errorMessage}";
                    }
                    return;
                }

                var payload = BuildQrCodePayload();
                
                // Hiển thị payload lên QrValueSimpleLabelItem
                if (QrCodeValueMemoEdit != null)
                {
                    QrCodeValueMemoEdit.Text = string.IsNullOrWhiteSpace(payload) 
                        ? "Giá trị QR: (Chưa có dữ liệu)" 
                        : $"Giá trị QR: {payload}";
                }

                // Nếu không có payload, xóa QR code
                if (string.IsNullOrWhiteSpace(payload))
                {
                    if (QrCodePictureEdit.Image != null)
                    {
                        QrCodePictureEdit.Image.Dispose();
                        QrCodePictureEdit.Image = null;
                    }
                    return;
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
                barCode.CodeBinaryData = Encoding.UTF8.GetBytes(payload);
                barCode.Options.QRCode.CompactionMode = QRCodeCompactionMode.Byte;
                barCode.Options.QRCode.ErrorLevel = QRCodeErrorLevel.Q; // Mức lỗi Q (25%)
                barCode.Options.QRCode.ShowCodeText = false;

                // Dispose image cũ nếu có
                QrCodePictureEdit.Image?.Dispose();
                
                // Hiển thị QR code mới
                QrCodePictureEdit.Image = barCode.BarCodeImage.ConvertToGdiPlusImage();
                
                System.Diagnostics.Debug.WriteLine($"GenerateQrCode: Đã tạo QR code thành công ({payload.Length} ký tự)");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GenerateQrCode: Exception occurred - {ex.Message}");
                MsgBox.ShowError($"Lỗi tạo QR code: {ex.Message}");
            }
        }

        #endregion
    }
}