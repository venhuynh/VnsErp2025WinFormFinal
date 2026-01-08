using Bll.Inventory.InventoryManagement;
using Bll.MasterData.ProductServiceBll;
using Common.Utils;
using DevExpress.XtraEditors;
using DTO.Inventory;
using DTO.Inventory.InventoryManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

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
        private readonly HashSet<string> _identifierValues = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Map ProductVariantId -> danh sách identifier thuộc biến thể đó
        /// </summary>
        private readonly Dictionary<Guid, List<string>> _variantIdentifierMap = new Dictionary<Guid, List<string>>();

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

            // Tránh quét trùng cùng identifier
            if (_identifierValues.Contains(identifierValue))
            {
                // Định danh đã tồn tại, bỏ qua
                return;
            }

            // Thêm vào danh sách identifier toàn cục
            _identifierValues.Add(identifierValue);

            // Lấy list identifier theo ProductVariant
            if (!_variantIdentifierMap.TryGetValue(identifier.ProductVariantId, out var identifiersForVariant))
            {
                identifiersForVariant = new List<string>();
                _variantIdentifierMap[identifier.ProductVariantId] = identifiersForVariant;
            }

            identifiersForVariant.Add(identifierValue);

            // Lấy thông tin ProductVariant
            var productVariant = _productVariantBll.GetById(identifier.ProductVariantId);
            if (productVariant == null)
            {
                MsgBox.ShowError($"Không tìm thấy thông tin sản phẩm với ID: {identifier.ProductVariantId}");
                return;
            }

            // Kiểm tra xem đã tồn tại trong danh sách chưa (theo ProductVariantId)
            var existingItem = _stockInOutDetailList.FirstOrDefault(x => x.ProductVariantId == identifier.ProductVariantId);

            if (existingItem != null)
            {
                // Cập nhật số lượng bằng số identifier đã quét cho biến thể này
                existingItem.StockOutQty = identifiersForVariant.Count;
                existingItem.GhiChu = $"Định danh: {string.Join(", ", identifiersForVariant)}";
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
                    GhiChu = $"Định danh: {string.Join(", ", identifiersForVariant)}"
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