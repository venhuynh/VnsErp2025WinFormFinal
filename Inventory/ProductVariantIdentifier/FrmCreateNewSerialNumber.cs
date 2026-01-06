using System;
using System.Collections.Generic;
using System.Linq;
using Bll.Inventory.InventoryManagement;
using Common.Utils;
using DTO.Inventory.InventoryManagement;

namespace Inventory.ProductVariantIdentifier
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
            SetupSuperToolTips();
            LoadData();
        }

        #endregion

        #region ========== INITIALIZATION ==========

        /// <summary>
        /// Thiết lập SuperToolTip cho tất cả các controls trong form
        /// </summary>
        private void SetupSuperToolTips()
        {
            try
            {
                // SuperTip cho Số phiếu nhập/xuất
                if (VoucerNumberTextEdit != null)
                {
                    SuperToolTipHelper.SetTextEditSuperTip(
                        VoucerNumberTextEdit,
                        title: @"<b><color=DarkBlue>📄 Số phiếu nhập/xuất</color></b>",
                        content: @"Số phiếu nhập/xuất kho được hiển thị từ thông tin phiếu đã chọn.<br/><br/><b>Chức năng:</b><br/>• Hiển thị số phiếu nhập/xuất từ DTO<br/>• Dùng để tạo serial numbers theo format: <b>{VoucherNumber}-XXX</b><br/>• Không thể chỉnh sửa (read-only)<br/><br/><b>Format Serial Number:</b><br/>• <b>{VoucherNumber}-001</b><br/>• <b>{VoucherNumber}-002</b><br/>• <b>{VoucherNumber}-003</b><br/>• ...<br/><br/><color=Gray>Lưu ý:</color> Serial numbers sẽ được tạo tự động dựa trên số phiếu này và số lượng sản phẩm."
                    );
                }

                // SuperTip cho Số lượng sản phẩm
                if (SerialNumberQtyTextEdit != null)
                {
                    SuperToolTipHelper.SetTextEditSuperTip(
                        SerialNumberQtyTextEdit,
                        title: @"<b><color=DarkBlue>🔢 Số lượng sản phẩm</color></b>",
                        content: @"Số lượng sản phẩm nhập/xuất được lấy từ phiếu nhập/xuất.<br/><br/><b>Chức năng:</b><br/>• Hiển thị số lượng sản phẩm từ DTO (StockInQty hoặc StockOutQty)<br/>• Khi thay đổi, tự động tạo lại danh sách serial numbers<br/>• Mỗi sản phẩm sẽ có một serial number duy nhất<br/><br/><b>Quy tắc tạo Serial Number:</b><br/>• Format: <b>{VoucherNumber}-XXX</b><br/>• XXX là số chạy từ 001 đến số lượng sản phẩm<br/>• Ví dụ: Nếu số lượng = 5, sẽ tạo:<br/>  • <b>PNK-001</b><br/>  • <b>PNK-002</b><br/>  • <b>PNK-003</b><br/>  • <b>PNK-004</b><br/>  • <b>PNK-005</b><br/><br/><b>Ràng buộc:</b><br/>• Phải là số nguyên dương (> 0)<br/>• Tối đa 999999 sản phẩm<br/><br/><color=Gray>Lưu ý:</color> Bạn có thể chỉnh sửa số lượng để tạo số lượng serial numbers khác."
                    );
                }

                // SuperTip cho Danh sách Serial Numbers
                if (SerialNumberMemoEdit != null)
                {
                    SuperToolTipHelper.SetBaseEditSuperTip(
                        SerialNumberMemoEdit,
                        title: @"<b><color=DarkBlue>📋 Danh sách Serial Numbers</color></b>",
                        content: @"Danh sách các serial numbers được tạo tự động.<br/><br/><b>Chức năng:</b><br/>• Hiển thị danh sách serial numbers đã được tạo tự động<br/>• Mỗi serial number trên một dòng<br/>• Format: <b>{VoucherNumber}-XXX</b> (XXX là số chạy 3 chữ số)<br/><br/><b>Tự động tạo khi:</b><br/>• Form được load lần đầu<br/>• Số lượng sản phẩm thay đổi<br/>• Số phiếu nhập/xuất thay đổi<br/><br/><b>Chỉnh sửa:</b><br/>• Bạn có thể chỉnh sửa thủ công danh sách serial numbers<br/>• Mỗi dòng là một serial number<br/>• Có thể thêm/xóa/sửa các dòng<br/><br/><b>Lưu vào database:</b><br/>• Khi click nút <b>Lưu</b>, từng serial number sẽ được lưu vào bảng <b>ProductVariantIdentifier</b><br/>• Cột <b>SerialNumber</b> sẽ chứa giá trị serial number<br/>• Hệ thống sẽ kiểm tra trùng lặp trước khi lưu<br/><br/><color=Gray>Lưu ý:</color> Serial numbers trùng lặp sẽ không được lưu và sẽ hiển thị cảnh báo."
                    );
                }

                // SuperTip cho nút Lưu
                if (SaveBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        SaveBarButtonItem,
                        title: @"<b><color=Green>💾 Lưu</color></b>",
                        content: @"Lưu tất cả serial numbers vào database.<br/><br/><b>Chức năng:</b><br/>• Lưu từng serial number vào bảng <b>ProductVariantIdentifier</b><br/>• Cột <b>SerialNumber</b> sẽ chứa giá trị serial number<br/>• Tự động kiểm tra trùng lặp trước khi lưu<br/>• Hiển thị kết quả lưu (thành công/thất bại)<br/><br/><b>Quy trình:</b><br/>1. Validate dữ liệu (ProductVariantId, serial numbers, voucher number)<br/>2. Xác nhận với người dùng<br/>3. Lặp qua từng serial number:<br/>   • Kiểm tra trùng lặp<br/>   • Tạo DTO mới với thông tin đầy đủ<br/>   • Lưu vào database<br/>4. Hiển thị kết quả và đóng form nếu thành công<br/><br/><b>Thông tin lưu:</b><br/>• ProductVariantId: Từ DTO đã chọn<br/>• SerialNumber: Giá trị serial number<br/>• Status: <b>AtVnsWarehouse</b> (Tại kho VNS)<br/>• SourceType: <b>Manual</b> (Nhập thủ công)<br/>• SourceReference: Tham chiếu đến phiếu nhập/xuất<br/><br/><color=Gray>Lưu ý:</color> Serial numbers đã tồn tại sẽ không được lưu và sẽ hiển thị trong danh sách lỗi."
                    );
                }

                // SuperTip cho nút Đóng
                if (CloseBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        CloseBarButtonItem,
                        title: @"<b><color=Red>❌ Đóng</color></b>",
                        content: @"Đóng form và không lưu dữ liệu.<br/><br/><b>Chức năng:</b><br/>• Đóng form hiện tại<br/>• Không lưu dữ liệu vào database<br/>• Hủy bỏ mọi thay đổi chưa lưu<br/><br/><color=Gray>Lưu ý:</color> Nếu bạn đã chỉnh sửa serial numbers, hãy nhớ click <b>Lưu</b> trước khi đóng form."
                    );
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"SetupSuperToolTips: Exception occurred - {ex.Message}");
            }
        }

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