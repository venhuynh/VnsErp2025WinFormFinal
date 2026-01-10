using Bll.Inventory.InventoryManagement;
using Bll.MasterData.ProductServiceBll;
using Common.Utils;
using DTO.Inventory;
using DTO.Inventory.InventoryManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DTO.Inventory.StockInOut;

namespace Inventory.OverlayForm
{
    public partial class FrmGetIdentifierForStockOut : DevExpress.XtraEditors.XtraForm
    {
        #region ========== FIELDS & PROPERTIES ==========

        /// <summary>
        /// Business Logic Layer cho ProductVariantIdentifier
        /// </summary>
        private readonly ProductVariantIdentifierBll _productVariantIdentifierBll = new ProductVariantIdentifierBll();

        /// <summary>
        /// Business Logic Layer cho ProductVariant
        /// </summary>
        private readonly ProductVariantBll _productVariantBll = new ProductVariantBll();

        /// <summary>
        /// Danh sách StockInOutDetailForUIDto (biến toàn cục)
        /// </summary>
        private List<StockInOutDetailForUIDto> _stockInOutDetailList = new List<StockInOutDetailForUIDto>();

        /// <summary>
        /// Danh sách tất cả identifier đã quét (để tránh trùng)
        /// </summary>
        private readonly List<ProductVariantIdentifierDto> _identifierValues = new List<ProductVariantIdentifierDto>();

        /// <summary>
        /// Map ProductVariantId -> danh sách ProductVariantIdentifierDto thuộc biến thể đó
        /// </summary>
        private readonly Dictionary<Guid, List<ProductVariantIdentifierDto>> _variantIdentifierMap = new Dictionary<Guid, List<ProductVariantIdentifierDto>>();

        /// <summary>
        /// Danh sách StockInOutDetailForUIDto đã xử lý (trả về cho form cha)
        /// </summary>
        public List<StockInOutDetailForUIDto> ResultStockInOutDetailList { get; private set; }

        /// <summary>
        /// Danh sách ProductVariantIdentifierDto đã xử lý (trả về cho form cha)
        /// </summary>
        public List<ProductVariantIdentifierDto> ResultIdentifierValues { get; private set; }

        #endregion

        #region ========== CONSTRUCTOR ==========

        public FrmGetIdentifierForStockOut()
        {
            InitializeComponent();
            InitializeControl();
        }

        #endregion

        #region ========== INITIALIZATION ==========

        /// <summary>
        /// Khởi tạo control và events
        /// </summary>
        private void InitializeControl()
        {
            // Khởi tạo danh sách rỗng
            _stockInOutDetailList = new List<StockInOutDetailForUIDto>();
            stockInOutDetailForUIDtoBindingSource.DataSource = _stockInOutDetailList;
            _identifierValues.Clear();
            _variantIdentifierMap.Clear();

            // Khởi tạo events
            InitializeEvents();
        }

        /// <summary>
        /// Khởi tạo các event handlers
        /// </summary>
        private void InitializeEvents()
        {
            // Event khi giá trị IdentifierValueTextEdit thay đổi (để xử lý paste text có TAB/ENTER)
            IdentifierValueTextEdit.EditValueChanged += IdentifierValueTextEdit_EditValueChanged;
            IdentifierValueTextEdit.KeyDown += IdentifierValueTextEdit_KeyDown;

            //Event nút thêm vào
            AddHyperlinkLabelControl.Click += AddHyperlinkLabelControl_Click;
            
            // Event cho nút xóa
            RemoveHyperlinkLabelControl.Click += RemoveHyperlinkLabelControl_Click;
            
            // Event cho nút kết thúc
            FinishedHyperlinkLabelControl.Click += FinishedHyperlinkLabelControl_Click;
            
            // Event khi form đóng
            FormClosing += FrmGetIdentifierForStockOut_FormClosing;
        }

        #endregion

        #region ========== EVENT HANDLERS ==========

        /// <summary>
        /// Xử lý khi giá trị IdentifierValueTextEdit thay đổi
        /// </summary>
        private void IdentifierValueTextEdit_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                var text = IdentifierValueTextEdit.Text;
                if (string.IsNullOrWhiteSpace(text))
                {
                    return;
                }

                // Kiểm tra nếu text có chứa TAB hoặc ENTER
                bool containsTabOrEnter = text.Contains("\t") || text.Contains("\r") || text.Contains("\n");
                
                if (containsTabOrEnter)
                {
                    // Tách text thành các identifier (theo TAB, ENTER, hoặc cả hai)
                    var identifiers = text.Split(new[] { "\t", "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(s => s.Trim())
                        .Where(s => !string.IsNullOrWhiteSpace(s))
                        .ToList();

                    // Xử lý từng identifier
                    foreach (var identifierValue in identifiers)
                    {
                        ProcessIdentifier(identifierValue);
                    }

                    // Xóa text sau khi xử lý
                    IdentifierValueTextEdit.Text = string.Empty;
                    IdentifierValueTextEdit.Focus();
                }
                // Nếu không có TAB/ENTER, không xử lý ở đây (sẽ xử lý trong KeyDown khi nhấn Enter/Tab)
            }
            catch (Exception ex)
            {
                MsgBox.ShowError($"Lỗi khi xử lý định danh: {ex.Message}");
            }
        }

        /// <summary>
        /// Xử lý một identifier cụ thể
        /// </summary>
        private void ProcessIdentifier(string identifierValue)
        {
            if (string.IsNullOrWhiteSpace(identifierValue))
            {
                return;
            }

            // Tìm kiếm ProductVariantIdentifier
            var identifier = _productVariantIdentifierBll.FindByIdentifier(identifierValue);
            if (identifier == null)
            {
                // Không tìm thấy, có thể hiển thị thông báo hoặc bỏ qua
                return;
            }

            // Tránh quét trùng cùng identifier (kiểm tra theo Id)
            if (_identifierValues.Any(x => x.Id == identifier.Id))
            {
                // Định danh đã tồn tại, bỏ qua
                return;
            }

            // Lấy thông tin ProductVariant trước khi thêm vào danh sách (để tránh thêm vào danh sách nếu productVariant không tồn tại)
            var productVariant = _productVariantBll.GetById(identifier.ProductVariantId);
            if (productVariant == null)
            {
                MsgBox.ShowError($"Không tìm thấy thông tin sản phẩm với ID: {identifier.ProductVariantId}");
                return;
            }

            // Thêm vào danh sách identifier toàn cục
            _identifierValues.Add(identifier);

            // Lấy list identifier theo ProductVariant
            if (!_variantIdentifierMap.TryGetValue(identifier.ProductVariantId, out var identifiersForVariant))
            {
                identifiersForVariant = new List<ProductVariantIdentifierDto>();
                _variantIdentifierMap[identifier.ProductVariantId] = identifiersForVariant;
            }

            identifiersForVariant.Add(identifier);

            // Kiểm tra xem đã tồn tại trong danh sách chưa (theo ProductVariantId)
            var existingItem = _stockInOutDetailList.FirstOrDefault(x => x.ProductVariantId == identifier.ProductVariantId);

            if (existingItem != null)
            {
                // Cập nhật số lượng bằng số identifier đã quét cho biến thể này
                existingItem.StockOutQty = identifiersForVariant.Count;
                existingItem.GhiChu = GetIdentifierNamesText(identifiersForVariant);
            }
            else
            {
                // Tạo StockInOutDetailForUIDto mới
                var newDetail = new StockInOutDetailForUIDto
                {
                    Id = Guid.NewGuid(),
                    ProductVariantId = identifier.ProductVariantId,
                    ProductVariantCode = productVariant.VariantCode,
                    ProductVariantName = productVariant.VariantFullName ?? string.Empty,
                    UnitOfMeasureName = productVariant.UnitName ?? string.Empty,
                    StockOutQty = identifiersForVariant.Count, // bằng số identifier đã quét
                    UnitPrice = 0,
                    Vat = 0,
                    GhiChu = GetIdentifierNamesText(identifiersForVariant)
                };

                // Thêm vào danh sách
                _stockInOutDetailList.Add(newDetail);
            }

            // Cập nhật LineNumber cho tất cả các dòng
            UpdateLineNumbers();

            // Refresh binding source
            stockInOutDetailForUIDtoBindingSource.ResetBindings(false);
        }

        /// <summary>
        /// Xử lý khi nhấn phím TAB hoặc ENTER trong IdentifierValueTextEdit
        /// </summary>
        private void IdentifierValueTextEdit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                var identifierValue = IdentifierValueTextEdit.Text?.Trim();
                if (!string.IsNullOrWhiteSpace(identifierValue))
                {
                    // Xử lý identifier hiện tại
                    ProcessIdentifier(identifierValue);
                    
                    // Xóa text và focus lại
                    IdentifierValueTextEdit.Text = string.Empty;
                    IdentifierValueTextEdit.Focus();
                }
                
                // Ngăn không cho TAB chuyển focus sang control khác
                if (e.KeyCode == Keys.Tab)
                {
                    e.Handled = true;
                }
            }
        }

        /// <summary>
        /// Handles the click event for the "Add" hyperlink label control.
        /// </summary>
        /// <param name="sender">The source of the event, typically the control that was clicked.</param>
        /// <param name="e">An <see cref="EventArgs"/> instance containing the event data.</param>

        private void AddHyperlinkLabelControl_Click(object sender, EventArgs e)
        {
            var identifierValue = IdentifierValueTextEdit.Text?.Trim();
            if (!string.IsNullOrWhiteSpace(identifierValue))
            {
                // Xử lý identifier hiện tại
                ProcessIdentifier(identifierValue);

                // Xóa text và focus lại
                IdentifierValueTextEdit.Text = string.Empty;
                IdentifierValueTextEdit.Focus();
            }
        }


        /// <summary>
        /// Xử lý khi click nút xóa (RemoveHyperlinkLabelControl)
        /// </summary>
        private void RemoveHyperlinkLabelControl_Click(object sender, EventArgs e)
        {
            try
            {
                // Lấy các dòng được chọn trong GridView
                var selectedRowHandles = StockInOutDetailForUIDtoGridView.GetSelectedRows();
                
                if (selectedRowHandles == null || selectedRowHandles.Length == 0)
                {
                    MsgBox.ShowWarning("Vui lòng chọn ít nhất một dòng để xóa.");
                    return;
                }

                // Lấy danh sách ProductVariantId từ các dòng được chọn
                var productVariantIdsToRemove = new HashSet<Guid>();
                var itemsToRemove = new List<StockInOutDetailForUIDto>();

                foreach (var rowHandle in selectedRowHandles)
                {
                    if (rowHandle >= 0)
                    {
                        if (StockInOutDetailForUIDtoGridView.GetRow(rowHandle) is StockInOutDetailForUIDto rowData)
                        {
                            productVariantIdsToRemove.Add(rowData.ProductVariantId);
                            itemsToRemove.Add(rowData);
                        }
                    }
                }

                if (productVariantIdsToRemove.Count == 0)
                {
                    return;
                }

                // Xác nhận xóa
                var confirmMessage = $"Bạn có chắc chắn muốn xóa {itemsToRemove.Count} dòng đã chọn?";
                if (MsgBox.ShowYesNoCancel(confirmMessage) != DialogResult.Yes)
                {
                    return;
                }

                // Xóa các dòng khỏi danh sách
                foreach (var item in itemsToRemove)
                {
                    _stockInOutDetailList.Remove(item);
                }

                // Xóa tất cả identifier có ProductVariantId tương ứng khỏi _identifierValues
                _identifierValues.RemoveAll(x => productVariantIdsToRemove.Contains(x.ProductVariantId));

                // Xóa khỏi _variantIdentifierMap
                foreach (var productVariantId in productVariantIdsToRemove)
                {
                    _variantIdentifierMap.Remove(productVariantId);
                }

                // Cập nhật LineNumber cho tất cả các dòng còn lại
                UpdateLineNumbers();

                // Refresh binding source
                stockInOutDetailForUIDtoBindingSource.ResetBindings(false);

                AlertHelper.ShowInfo($"Đã xóa {itemsToRemove.Count} dòng thành công.");
            }
            catch (Exception ex)
            {
                MsgBox.ShowError($"Lỗi khi xóa dòng: {ex.Message}");
            }
        }

        /// <summary>
        /// Xử lý khi click nút kết thúc (FinishedHyperlinkLabelControl)
        /// </summary>
        private void FinishedHyperlinkLabelControl_Click(object sender, EventArgs e)
        {
            HandleFormClosing();
        }

        /// <summary>
        /// Xử lý khi form đóng (FormClosing event)
        /// </summary>
        private void FrmGetIdentifierForStockOut_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Chỉ xử lý nếu chưa set DialogResult (tức là không phải từ nút Finished)
            if (DialogResult == DialogResult.None)
            {
                e.Cancel = !HandleFormClosing();
            }
        }

        /// <summary>
        /// Xử lý logic đóng form (dùng chung cho cả nút Finished và FormClosing)
        /// </summary>
        /// <returns>True nếu cho phép đóng form, False nếu hủy đóng</returns>
        private bool HandleFormClosing()
        {
            try
            {
                // Kiểm tra nếu cả 2 danh sách đều rỗng
                bool isEmpty = (_stockInOutDetailList == null || _stockInOutDetailList.Count == 0) &&
                               (_identifierValues == null || _identifierValues.Count == 0);

                if (isEmpty)
                {
                    // Xác nhận lại trước khi đóng
                    var confirmMessage = "Bạn chưa thêm bất kỳ sản phẩm nào. Bạn có chắc chắn muốn đóng màn hình?";
                    if (MsgBox.ShowYesNoCancel(confirmMessage) != DialogResult.Yes)
                    {
                        return false;
                    }
                }

                // Gán kết quả vào properties để trả về cho form cha
                ResultStockInOutDetailList = _stockInOutDetailList.ToList();
                ResultIdentifierValues = _identifierValues.ToList();

                // Đóng form với DialogResult.OK
                DialogResult = DialogResult.OK;
                return true;
            }
            catch (Exception ex)
            {
                MsgBox.ShowError($"Lỗi khi kết thúc: {ex.Message}");
                return false;
            }
        }

        #endregion

        #region ========== HELPER METHODS ==========

        /// <summary>
        /// Cập nhật LineNumber cho tất cả các dòng trong danh sách
        /// </summary>
        private void UpdateLineNumbers()
        {
            for (int i = 0; i < _stockInOutDetailList.Count; i++)
            {
                _stockInOutDetailList[i].LineNumber = i + 1;
            }
        }

        /// <summary>
        /// Lấy danh sách tên định danh kèm giá trị từ danh sách ProductVariantIdentifierDto (loại trừ ID)
        /// </summary>
        private string GetIdentifierNamesText(List<ProductVariantIdentifierDto> identifiers)
        {
            // Dictionary để nhóm các giá trị theo loại định danh
            var identifierGroups = new Dictionary<string, List<string>>();

            foreach (var identifier in identifiers)
            {
                // Serial Number
                if (!string.IsNullOrWhiteSpace(identifier.SerialNumber))
                {
                    if (!identifierGroups.ContainsKey("Serial Number"))
                        identifierGroups["Serial Number"] = new List<string>();
                    identifierGroups["Serial Number"].Add(identifier.SerialNumber);
                }
                // Part Number
                if (!string.IsNullOrWhiteSpace(identifier.PartNumber))
                {
                    if (!identifierGroups.ContainsKey("Part Number"))
                        identifierGroups["Part Number"] = new List<string>();
                    identifierGroups["Part Number"].Add(identifier.PartNumber);
                }
                // QR Code
                if (!string.IsNullOrWhiteSpace(identifier.QRCode))
                {
                    if (!identifierGroups.ContainsKey("QR Code"))
                        identifierGroups["QR Code"] = new List<string>();
                    identifierGroups["QR Code"].Add(identifier.QRCode);
                }
                // SKU
                if (!string.IsNullOrWhiteSpace(identifier.SKU))
                {
                    if (!identifierGroups.ContainsKey("SKU"))
                        identifierGroups["SKU"] = new List<string>();
                    identifierGroups["SKU"].Add(identifier.SKU);
                }
                // RFID
                if (!string.IsNullOrWhiteSpace(identifier.RFID))
                {
                    if (!identifierGroups.ContainsKey("RFID"))
                        identifierGroups["RFID"] = new List<string>();
                    identifierGroups["RFID"].Add(identifier.RFID);
                }
                // MAC Address
                if (!string.IsNullOrWhiteSpace(identifier.MACAddress))
                {
                    if (!identifierGroups.ContainsKey("MAC Address"))
                        identifierGroups["MAC Address"] = new List<string>();
                    identifierGroups["MAC Address"].Add(identifier.MACAddress);
                }
                // IMEI
                if (!string.IsNullOrWhiteSpace(identifier.IMEI))
                {
                    if (!identifierGroups.ContainsKey("IMEI"))
                        identifierGroups["IMEI"] = new List<string>();
                    identifierGroups["IMEI"].Add(identifier.IMEI);
                }
                // Asset Tag
                if (!string.IsNullOrWhiteSpace(identifier.AssetTag))
                {
                    if (!identifierGroups.ContainsKey("Asset Tag"))
                        identifierGroups["Asset Tag"] = new List<string>();
                    identifierGroups["Asset Tag"].Add(identifier.AssetTag);
                }
                // License Key
                if (!string.IsNullOrWhiteSpace(identifier.LicenseKey))
                {
                    if (!identifierGroups.ContainsKey("License Key"))
                        identifierGroups["License Key"] = new List<string>();
                    identifierGroups["License Key"].Add(identifier.LicenseKey);
                }
                // UPC
                if (!string.IsNullOrWhiteSpace(identifier.UPC))
                {
                    if (!identifierGroups.ContainsKey("UPC"))
                        identifierGroups["UPC"] = new List<string>();
                    identifierGroups["UPC"].Add(identifier.UPC);
                }
                // EAN
                if (!string.IsNullOrWhiteSpace(identifier.EAN))
                {
                    if (!identifierGroups.ContainsKey("EAN"))
                        identifierGroups["EAN"] = new List<string>();
                    identifierGroups["EAN"].Add(identifier.EAN);
                }
                // Other Identifier
                if (!string.IsNullOrWhiteSpace(identifier.OtherIdentifier))
                {
                    if (!identifierGroups.ContainsKey("Other Identifier"))
                        identifierGroups["Other Identifier"] = new List<string>();
                    identifierGroups["Other Identifier"].Add(identifier.OtherIdentifier);
                }
                // Loại trừ ID như yêu cầu
            }

            if (identifierGroups.Count == 0)
            {
                return "Định danh";
            }

            // Tạo chuỗi kết quả: "Tên loại: giá trị1, giá trị2, ..."
            var resultParts = identifierGroups.Select(kvp =>
            {
                var values = string.Join(", ", kvp.Value);
                return $"{kvp.Key}: {values}";
            });

            return $"{string.Join("; ", resultParts)}";
        }

        #endregion

        #region ========== PUBLIC METHODS ==========

        /// <summary>
        /// Lấy danh sách StockInOutDetailForUIDto
        /// </summary>
        public List<StockInOutDetailForUIDto> GetStockInOutDetailList()
        {
            return _stockInOutDetailList.ToList(); // Trả về bản copy để tránh thay đổi từ bên ngoài
        }

        /// <summary>
        /// Xóa tất cả các item trong danh sách
        /// </summary>
        public void ClearList()
        {
            _stockInOutDetailList.Clear();
            _identifierValues.Clear();
            _variantIdentifierMap.Clear();
            UpdateLineNumbers();
            stockInOutDetailForUIDtoBindingSource.ResetBindings(false);
        }

        #endregion
    }
}