using Bll.Inventory.InventoryManagement;
using Common.Utils;
using DTO.Inventory.InventoryManagement;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Inventory.Management
{
    public partial class FrmCreateNewSerialNumber : DevExpress.XtraEditors.XtraForm
    {
        #region ========== FIELDS & PROPERTIES ==========

        /// <summary>
        /// DTO chứa thông tin sản phẩm được chọn
        /// </summary>
        private readonly StockInOutProductHistoryDto _selectedDto;

        /// <summary>
        /// Business Logic Layer cho ProductVariantIdentifier
        /// </summary>
        private readonly ProductVariantIdentifierBll _productVariantIdentifierBll = new ProductVariantIdentifierBll();

        #endregion

        #region ========== CONSTRUCTOR ==========

        public FrmCreateNewSerialNumber(StockInOutProductHistoryDto selectedDto)
        {
            _selectedDto = selectedDto ?? throw new ArgumentNullException(nameof(selectedDto));
            InitializeComponent();
            InitializeEvents();
            LoadData();
        }

        #endregion

        #region ========== INITIALIZATION ==========

        /// <summary>
        /// Khởi tạo các event handlers
        /// </summary>
        private void InitializeEvents()
        {
            SerialNumberQtyTextEdit.EditValueChanged += SerialNumberQtyTextEdit_EditValueChanged;
            CloseBarButtonItem.ItemClick += CloseBarButtonItem_ItemClick;
            SaveBarButtonItem.ItemClick += SaveBarButtonItem_ItemClick;
        }

        /// <summary>
        /// Load dữ liệu từ DTO vào các control
        /// </summary>
        private void LoadData()
        {
            try
            {
                // Hiển thị số phiếu nhập xuất
                VoucerNumberTextEdit.Text = _selectedDto.VocherNumber ?? string.Empty;

                // Tính số lượng sản phẩm nhập xuất (lấy số lượng lớn hơn giữa nhập và xuất)
                decimal quantity = _selectedDto.StockInQty > 0 ? _selectedDto.StockInQty : _selectedDto.StockOutQty;
                SerialNumberQtyTextEdit.Text = quantity.ToString();

                // Hiển thị thông tin sản phẩm
                if (!string.IsNullOrWhiteSpace(_selectedDto.ProductVariantFullName))
                {
                    ProductVariantFullNameSimpleLabelItem.Text = $"<b>Sản phẩm:</b> {_selectedDto.ProductVariantFullName}";
                }

                // Tự động tạo serial numbers ban đầu
                GenerateSerialNumbers();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LoadData: Exception occurred - {ex.Message}");
                MsgBox.ShowError($"Lỗi tải dữ liệu: {ex.Message}");
            }
        }

        #endregion

        #region ========== EVENT HANDLERS ==========

        /// <summary>
        /// Event handler khi số lượng serial number thay đổi
        /// </summary>
        private void SerialNumberQtyTextEdit_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                GenerateSerialNumbers();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"SerialNumberQtyTextEdit_EditValueChanged: Exception occurred - {ex.Message}");
            }
        }

        /// <summary>
        /// Event handler khi click nút Lưu
        /// </summary>
        private void SaveBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                // Validate dữ liệu trước khi lưu
                var (isValid, errorMessage) = ValidateBeforeSave();
                if (!isValid)
                {
                    MsgBox.ShowWarning(errorMessage);
                    return;
                }

                // Lấy danh sách serial numbers từ MemoEdit
                var serialNumbers = GetSerialNumbersFromMemo();
                if (serialNumbers == null || serialNumbers.Count == 0)
                {
                    MsgBox.ShowWarning("Không có serial number nào để lưu.");
                    return;
                }

                // Xác nhận trước khi lưu
                var confirmMessage = $"Bạn có chắc chắn muốn lưu {serialNumbers.Count} serial number(s) vào hệ thống?";
                if (!MsgBox.ShowYesNo(confirmMessage, "Xác nhận lưu"))
                {
                    return;
                }

                // Lưu từng serial number
                int successCount = 0;
                int failCount = 0;
                var failedSerialNumbers = new List<string>();

                //FIXME: Phải lặp từng dòng trong SerialNumberMemoEdit để lưu từng số serial vào database
                foreach (var serialNumber in serialNumbers)
                {
                    try
                    {
                        // Kiểm tra xem serial number đã tồn tại chưa
                        var existing = _productVariantIdentifierBll.FindBySerialNumber(serialNumber);
                        if (existing != null)
                        {
                            failedSerialNumbers.Add($"{serialNumber} (đã tồn tại)");
                            failCount++;
                            continue;
                        }

                        // Tạo DTO mới
                        var dto = new ProductVariantIdentifierDto
                        {
                            Id = Guid.NewGuid(),
                            ProductVariantId = _selectedDto.ProductVariantId,
                            ProductVariantFullName = _selectedDto.ProductVariantFullName,
                            SerialNumber = serialNumber,
                            Status = ProductVariantIdentifierStatusEnum.AtVnsWarehouse,
                            IsActive = true,
                            CreatedDate = DateTime.Now,
                            SourceType = 0, // Manual
                            SourceReference = $"Từ phiếu nhập/xuất: {_selectedDto.VocherNumber}"
                        };

                        // Lưu vào database
                        _productVariantIdentifierBll.SaveOrUpdate(dto);
                        successCount++;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"SaveBarButtonItem_ItemClick: Lỗi lưu serial number {serialNumber}: {ex.Message}");
                        failedSerialNumbers.Add($"{serialNumber} ({ex.Message})");
                        failCount++;
                    }
                }

                // Hiển thị kết quả
                if (successCount > 0 && failCount == 0)
                {
                    MsgBox.ShowSuccess($"Đã lưu thành công {successCount} serial number(s).");
                    this.Close();
                }
                else if (successCount > 0 && failCount > 0)
                {
                    var message = $"Đã lưu thành công {successCount} serial number(s).\n\n" +
                                  $"Không thể lưu {failCount} serial number(s):\n" +
                                  string.Join("\n", failedSerialNumbers.Take(10));
                    if (failedSerialNumbers.Count > 10)
                    {
                        message += $"\n... và {failedSerialNumbers.Count - 10} serial number(s) khác.";
                    }
                    MsgBox.ShowWarning(message);
                }
                else
                {
                    var message = $"Không thể lưu bất kỳ serial number nào:\n" +
                                  string.Join("\n", failedSerialNumbers.Take(10));
                    if (failedSerialNumbers.Count > 10)
                    {
                        message += $"\n... và {failedSerialNumbers.Count - 10} serial number(s) khác.";
                    }
                    MsgBox.ShowError(message);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"SaveBarButtonItem_ItemClick: Exception occurred - {ex.Message}");
                MsgBox.ShowError($"Lỗi khi lưu: {ex.Message}");
            }
        }

        /// <summary>
        /// Event handler khi click nút Đóng
        /// </summary>
        private void CloseBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        #endregion

        #region ========== HELPER METHODS ==========

        /// <summary>
        /// Validate dữ liệu trước khi lưu
        /// </summary>
        private (bool isValid, string errorMessage) ValidateBeforeSave()
        {
            // Kiểm tra ProductVariantId
            if (_selectedDto.ProductVariantId == Guid.Empty)
            {
                return (false, "ProductVariantId không hợp lệ.");
            }

            // Kiểm tra có serial numbers không
            var serialNumbers = GetSerialNumbersFromMemo();
            if (serialNumbers == null || serialNumbers.Count == 0)
            {
                return (false, "Không có serial number nào để lưu.");
            }

            // Kiểm tra voucher number
            if (string.IsNullOrWhiteSpace(VoucerNumberTextEdit.Text))
            {
                return (false, "Số phiếu nhập/xuất không được để trống.");
            }

            return (true, string.Empty);
        }

        /// <summary>
        /// Lấy danh sách serial numbers từ MemoEdit
        /// </summary>
        private List<string> GetSerialNumbersFromMemo()
        {
            try
            {
                var text = SerialNumberMemoEdit.Text?.Trim() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(text))
                {
                    return new List<string>();
                }

                // Split theo newline và filter các dòng không rỗng
                var serialNumbers = text
                    .Split(new[] { Environment.NewLine, "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim())
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .Distinct()
                    .ToList();

                return serialNumbers;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetSerialNumbersFromMemo: Exception occurred - {ex.Message}");
                return new List<string>();
            }
        }

        /// <summary>
        /// Tự động tạo serial numbers theo quy tắc {VoucherNumber}-XXX
        /// </summary>
        private void GenerateSerialNumbers()
        {
            try
            {
                string voucherNumber = VoucerNumberTextEdit.Text?.Trim() ?? string.Empty;
                string qtyText = SerialNumberQtyTextEdit.Text?.Trim() ?? string.Empty;

                // Kiểm tra điều kiện
                if (string.IsNullOrWhiteSpace(voucherNumber))
                {
                    SerialNumberMemoEdit.Text = string.Empty;
                    return;
                }

                if (string.IsNullOrWhiteSpace(qtyText))
                {
                    SerialNumberMemoEdit.Text = string.Empty;
                    return;
                }

                // Parse số lượng
                if (!int.TryParse(qtyText, out int quantity) || quantity <= 0)
                {
                    SerialNumberMemoEdit.Text = string.Empty;
                    return;
                }

                // Tạo danh sách serial numbers
                var serialNumbers = new List<string>();
                for (int i = 1; i <= quantity; i++)
                {
                    // Format: {VoucherNumber}-XXX (XXX là số chạy với 3 chữ số, ví dụ: 001, 002, 003...)
                    string serialNumber = $"{voucherNumber}-{i:D3}";
                    serialNumbers.Add(serialNumber);
                }

                // Hiển thị vào MemoEdit (mỗi serial number trên một dòng)
                SerialNumberMemoEdit.Text = string.Join(Environment.NewLine, serialNumbers);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GenerateSerialNumbers: Exception occurred - {ex.Message}");
                SerialNumberMemoEdit.Text = string.Empty;
            }
        }

        #endregion
    }
}