using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using Bll.Inventory.InventoryManagement;
using Common.Common;
using Common.Helpers;
using Common.Utils;
using DevExpress.XtraGrid.Views.Grid;
using DTO.Inventory.InventoryManagement;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;

namespace Inventory.ProductVariantIdentifier
{
    public partial class FrmProductVariantIdentifier : DevExpress.XtraEditors.XtraForm
    {
        #region ========== FIELDS & PROPERTIES ==========

        /// <summary>
        /// Business Logic Layer cho ProductVariantIdentifier
        /// </summary>
        private readonly ProductVariantIdentifierBll _productVariantIdentifierBll = new ProductVariantIdentifierBll();

        /// <summary>
        /// Logger để ghi log các sự kiện
        /// </summary>
        private readonly ILogger _logger = LoggerFactory.CreateLogger(LogCategory.UI);

        /// <summary>
        /// Bộ dữ liệu đầy đủ để phục vụ bộ lọc
        /// </summary>
        private List<ProductVariantIdentifierDto> _allData = new List<ProductVariantIdentifierDto>();

        /// <summary>
        /// Tiêu chí lọc hiện tại
        /// </summary>
        private FilterCriteria _filterCriteria = new FilterCriteria();

        #endregion

        #region ========== CONSTRUCTOR ==========

        public FrmProductVariantIdentifier()
        {
            InitializeComponent();
            InitializeForm();
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
                // Setup events
                SetupEvents();

                // Setup SuperToolTips
                SetupSuperToolTips();

                // Load enum vào ComboBox
                LoadProductVariantIdentifierStatusEnumRepositoryComboBox();

                // Cấu hình GridView để cho phép edit colStatus
                ConfigureGridViewEditing();

                // Load dữ liệu ban đầu
                //LoadData();
            }
            catch (Exception ex)
            {
                _logger.Error("InitializeForm: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi khởi tạo form: {ex.Message}");
            }
        }

        /// <summary>
        /// Thiết lập SuperToolTip cho tất cả các controls trong form
        /// </summary>
        private void SetupSuperToolTips()
        {
            try
            {
                // SuperTip cho nút Lọc toàn bộ
                if (FilterShowAllBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        FilterShowAllBarButtonItem,
                        title: @"<b><color=Blue>🔍 Lọc toàn bộ</color></b>",
                        content: @"Tải lại toàn bộ danh sách định danh sản phẩm từ database.<br/><br/><b>Chức năng:</b><br/>• Load tất cả định danh sản phẩm từ database<br/>• Refresh grid để hiển thị dữ liệu mới nhất<br/>• Cập nhật thống kê tổng hợp<br/><br/><color=Gray>Lưu ý:</color> Dữ liệu sẽ được tải từ database, có thể mất thời gian nếu có nhiều dữ liệu."
                    );
                }

                // SuperTip cho nút Xuất file
                if (ExportFileBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        ExportFileBarButtonItem,
                        title: @"<b><color=Green>📤 Xuất file</color></b>",
                        content: @"Xuất danh sách định danh sản phẩm ra file Excel.<br/><br/><b>Chức năng:</b><br/>• Xuất dữ liệu hiện tại trong grid ra file Excel (.xlsx)<br/>• Hỗ trợ chọn đường dẫn lưu file<br/>• Tên file mặc định: <b>Bảng định danh sản phẩm_YYYYMMDD_HHMMSS.xlsx</b><br/><br/><b>Định dạng:</b><br/>• File Excel (.xlsx)<br/>• Bao gồm tất cả các cột hiển thị trong grid<br/><br/><color=Gray>Lưu ý:</color> Chỉ xuất dữ liệu đang hiển thị trong grid."
                    );
                }

                // SuperTip cho nút LS Nhập/Xuất
                if (barButtonItem1 != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        barButtonItem1,
                        title: @"<b><color=Orange>📋 Lịch sử Nhập/Xuất</color></b>",
                        content: @"Xem lịch sử nhập xuất kho cho sản phẩm của định danh được chọn.<br/><br/><b>Chức năng:</b><br/>• Mở form lịch sử nhập xuất kho<br/>• Lọc theo ProductVariantId của định danh được chọn<br/>• Hiển thị tất cả các phiếu nhập/xuất liên quan<br/><br/><b>Yêu cầu:</b><br/>• Phải chọn một định danh<br/>• Định danh phải có ProductVariantId hợp lệ<br/><br/><color=Gray>Lưu ý:</color> Chức năng này đang được phát triển."
                    );
                }

                // SuperTip cho nút Thêm mới
                if (AddNewBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        AddNewBarButtonItem,
                        title: @"<b><color=Green>➕ Thêm mới</color></b>",
                        content: @"Thêm mới định danh sản phẩm vào hệ thống.<br/><br/><b>Chức năng:</b><br/>• Mở form thêm mới định danh sản phẩm<br/>• Cho phép nhập các loại định danh: SerialNumber, PartNumber, QRCode, SKU, RFID, MACAddress, IMEI, AssetTag, LicenseKey, UPC, EAN, ID, OtherIdentifier<br/><br/><color=Gray>Lưu ý:</color> Chức năng này đang được phát triển."
                    );
                }

                // SuperTip cho nút Điều chỉnh
                if (EditBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        EditBarButtonItem,
                        title: @"<b><color=Orange>✏️ Điều chỉnh</color></b>",
                        content: @"Chỉnh sửa thông tin định danh sản phẩm đã chọn.<br/><br/><b>Chức năng:</b><br/>• Mở form chỉnh sửa định danh<br/>• Load dữ liệu từ định danh được chọn<br/>• Cho phép sửa các loại định danh và thông tin khác<br/><br/><b>Yêu cầu:</b><br/>• Phải chọn một định danh<br/>• Định danh phải có Id hợp lệ<br/><br/><color=Gray>Lưu ý:</color> Chức năng này đang được phát triển."
                    );
                }

                // SuperTip cho nút Xóa
                if (barButtonItem4 != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        barButtonItem4,
                        title: @"<b><color=Red>🗑️ Xóa</color></b>",
                        content: @"Xóa các định danh sản phẩm được chọn.<br/><br/><b>Chức năng:</b><br/>• Xóa các định danh được chọn khỏi database<br/>• Cho phép xóa nhiều định danh cùng lúc<br/>• Hiển thị xác nhận trước khi xóa<br/><br/><b>Quy trình:</b><br/>1. Hiển thị xác nhận xóa<br/>2. Xóa từng định danh được chọn<br/>3. Reload dữ liệu sau khi xóa<br/>4. Hiển thị kết quả<br/><br/><b>Yêu cầu:</b><br/>• Phải chọn ít nhất một định danh<br/>• Định danh phải có Id hợp lệ<br/><br/><color=Red>⚠️ Cảnh báo:</color> Hành động này không thể hoàn tác!"
                    );
                }

                // SuperTip cho nút In tem
                if (barButtonItem5 != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        barButtonItem5,
                        title: @"<b><color=Purple>🏷️ In tem</color></b>",
                        content: @"In tem/label cho các định danh sản phẩm được chọn.<br/><br/><b>Chức năng:</b><br/>• Tạo và in tem/label cho định danh<br/>• Hỗ trợ in nhiều định danh cùng lúc<br/>• Có thể in QR code, barcode, hoặc thông tin định danh<br/><br/><b>Yêu cầu:</b><br/>• Phải chọn ít nhất một định danh<br/><br/><color=Gray>Lưu ý:</color> Chức năng này đang được phát triển."
                    );
                }

            }
            catch (Exception ex)
            {
                _logger.Error("SetupSuperToolTips: Exception occurred", ex);
            }
        }

        /// <summary>
        /// Setup các event handlers
        /// </summary>
        private void SetupEvents()
        {
            try
            {
                // Bar button events
                FilterShowAllBarButtonItem.ItemClick += LocToanBoBarButtonItem_ItemClick;
                FilterByStatusBarButtonItem.ItemClick += FilterByStatusBarButtonItem_ItemClick;
                FilterByUpdateDateBarButtonItem.ItemClick += FilterByUpdateDateBarButtonItem_ItemClick;
                FilterByIdentifierValueBarButtonItem.ItemClick += FilterByIdentifierValueBarButtonItem_ItemClick;
                FilterByProductVariantKeyWordBarButtonItem.ItemClick += FilterByProductVariantKeyWordBarButtonItem_ItemClick;
                ExportFileBarButtonItem.ItemClick += ExportFileBarButtonItem_ItemClick;
                barButtonItem1.ItemClick += BarButtonItem1_ItemClick;
                AddNewBarButtonItem.ItemClick += BarButtonItem2_ItemClick;
                EditBarButtonItem.ItemClick += BarButtonItem3_ItemClick;
                barButtonItem4.ItemClick += BarButtonItem4_ItemClick;
                barButtonItem5.ItemClick += BarButtonItem5_ItemClick;

                // GridView events
                ProductVariantIdentifierDtoGridView.DoubleClick += ProductVariantIdentifierDtoGridView_DoubleClick;
                ProductVariantIdentifierDtoGridView.FocusedRowChanged += ProductVariantIdentifierDtoGridView_FocusedRowChanged;
                ProductVariantIdentifierDtoGridView.SelectionChanged += ProductVariantIdentifierDtoGridView_SelectionChanged;
                ProductVariantIdentifierDtoGridView.CustomDrawRowIndicator += ProductVariantIdentifierDtoGridView_CustomDrawRowIndicator;
                ProductVariantIdentifierDtoGridView.CustomColumnDisplayText += ProductVariantIdentifierDtoGridView_CustomColumnDisplayText;
                ProductVariantIdentifierDtoGridView.CellValueChanged += ProductVariantIdentifierDtoGridView_CellValueChanged;
                ProductVariantIdentifierDtoGridView.ValidatingEditor += ProductVariantIdentifierDtoGridView_ValidatingEditor;

                // Form events
                Load += FrmProductVariantIdentifier_Load;
            }
            catch (Exception ex)
            {
                _logger.Error("SetupEvents: Exception occurred", ex);
            }
        }

        #endregion

        #region ========== EVENT HANDLERS ==========

        /// <summary>
        /// Event handler khi form được load
        /// </summary>
        private void FrmProductVariantIdentifier_Load(object sender, EventArgs e)
        {
            try
            {
                // Có thể thêm logic khởi tạo khi form load
            }
            catch (Exception ex)
            {
                _logger.Error("FrmProductVariantIdentifier_Load: Exception occurred", ex);
            }
        }

        /// <summary>
        /// Event handler cho nút Lọc toàn bộ
        /// </summary>
        private void LocToanBoBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                // Reset bộ lọc và tải lại dữ liệu
                _filterCriteria.Reset();
                LoadData();
            }
            catch (Exception ex)
            {
                _logger.Error("LocToanBoBarButtonItem_ItemClick: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi tải dữ liệu: {ex.Message}");
            }
        }

        /// <summary>
        /// Lọc theo trạng thái
        /// </summary>
        private void FilterByStatusBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                var chosen = ShowStatusSelector();
                if (!chosen.HasValue) return;

                _filterCriteria.Status = chosen.Value;
                ApplyFilterAndBind();
            }
            catch (Exception ex)
            {
                _logger.Error("FilterByStatusBarButtonItem_ItemClick: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi lọc theo tình trạng: {ex.Message}");
            }
        }

        /// <summary>
        /// Lọc theo ngày cập nhật (From/To)
        /// </summary>
        private void FilterByUpdateDateBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                var fromStr = ShowTextInput("Từ ngày (yyyy-MM-dd)", "Lọc theo ngày cập nhật", _filterCriteria.UpdatedFrom?.ToString("yyyy-MM-dd"));
                var toStr = ShowTextInput("Đến ngày (yyyy-MM-dd)", "Lọc theo ngày cập nhật", _filterCriteria.UpdatedTo?.ToString("yyyy-MM-dd"));

                DateTime? from = null;
                DateTime? to = null;

                if (!string.IsNullOrWhiteSpace(fromStr) && DateTime.TryParse(fromStr, out var f))
                    from = f.Date;
                if (!string.IsNullOrWhiteSpace(toStr) && DateTime.TryParse(toStr, out var t))
                    to = t.Date;

                _filterCriteria.UpdatedFrom = from;
                _filterCriteria.UpdatedTo = to;

                ApplyFilterAndBind();
            }
            catch (Exception ex)
            {
                _logger.Error("FilterByUpdateDateBarButtonItem_ItemClick: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi lọc theo ngày cập nhật: {ex.Message}");
            }
        }

        /// <summary>
        /// Lọc theo giá trị định danh (tìm trong tất cả loại định danh)
        /// </summary>
        private void FilterByIdentifierValueBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                var keyword = ShowTextInput("Nhập từ khóa định danh (Serial, QR, SKU, ...)", "Lọc theo định danh", _filterCriteria.IdentifierKeyword ?? string.Empty);
                if (keyword == null) return; // cancel

                _filterCriteria.IdentifierKeyword = string.IsNullOrWhiteSpace(keyword) ? null : keyword;
                ApplyFilterAndBind();
            }
            catch (Exception ex)
            {
                _logger.Error("FilterByIdentifierValueBarButtonItem_ItemClick: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi lọc theo định danh: {ex.Message}");
            }
        }

        /// <summary>
        /// Lọc theo tên sản phẩm (ProductVariant keyword)
        /// </summary>
        private void FilterByProductVariantKeyWordBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                var keyword = ShowTextInput("Nhập từ khóa sản phẩm", "Lọc theo tên sản phẩm", _filterCriteria.ProductVariantKeyword ?? string.Empty);
                if (keyword == null) return; // cancel

                _filterCriteria.ProductVariantKeyword = string.IsNullOrWhiteSpace(keyword) ? null : keyword;
                ApplyFilterAndBind();
            }
            catch (Exception ex)
            {
                _logger.Error("FilterByProductVariantKeyWordBarButtonItem_ItemClick: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi lọc theo từ khóa sản phẩm: {ex.Message}");
            }
        }

        /// <summary>
        /// Event handler cho nút Xuất file
        /// </summary>
        private void ExportFileBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                using (var saveDialog = new SaveFileDialog())
                {
                    saveDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                    saveDialog.FilterIndex = 1;
                    saveDialog.FileName = $"Bảng định danh sản phẩm_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        ProductVariantIdentifierDtoGridControl.ExportToXlsx(saveDialog.FileName);
                        MsgBox.ShowSuccess($"Đã xuất file thành công:\n{saveDialog.FileName}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("ExportFileBarButtonItem_ItemClick: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi xuất file: {ex.Message}");
            }
        }

        /// <summary>
        /// Event handler cho nút LS Nhập/Xuất
        /// </summary>
        private void BarButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                var focusedRowHandle = ProductVariantIdentifierDtoGridView.FocusedRowHandle;
                if (focusedRowHandle < 0)
                {
                    MsgBox.ShowWarning("Vui lòng chọn một định danh để xem lịch sử nhập/xuất.");
                    return;
                }

                if (ProductVariantIdentifierDtoGridView.GetRow(focusedRowHandle) is not ProductVariantIdentifierDto selectedDto)
                {
                    MsgBox.ShowWarning("Không thể lấy thông tin định danh được chọn.");
                    return;
                }

                if (selectedDto.ProductVariantId == Guid.Empty)
                {
                    MsgBox.ShowWarning("Định danh được chọn không có ProductVariantId hợp lệ.");
                    return;
                }

                // TODO: Mở form lịch sử nhập/xuất cho ProductVariantId này
                MsgBox.ShowWarning("Chức năng xem lịch sử nhập/xuất đang được phát triển.");
            }
            catch (Exception ex)
            {
                _logger.Error("BarButtonItem1_ItemClick: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi: {ex.Message}");
            }
        }

        /// <summary>
        /// Event handler cho nút Thêm mới
        /// </summary>
        private void BarButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                // Mở form thêm mới với Guid.Empty (thêm mới hoàn toàn)
                using (var form = new FrmProductVariantIdentifierAddEdit(Guid.Empty))
                {
                    form.ShowDialog(this);
                    
                    // Reload dữ liệu sau khi form đóng (nếu có thay đổi)
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                _logger.Error("BarButtonItem2_ItemClick: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi mở form thêm mới: {ex.Message}");
            }
        }

        /// <summary>
        /// Event handler cho nút Điều chỉnh
        /// </summary>
        private void BarButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                var focusedRowHandle = ProductVariantIdentifierDtoGridView.FocusedRowHandle;
                if (focusedRowHandle < 0)
                {
                    MsgBox.ShowWarning("Vui lòng chọn một định danh để điều chỉnh.");
                    return;
                }

                if (ProductVariantIdentifierDtoGridView.GetRow(focusedRowHandle) is not ProductVariantIdentifierDto selectedDto)
                {
                    MsgBox.ShowWarning("Không thể lấy thông tin định danh được chọn.");
                    return;
                }

                if (selectedDto.Id == Guid.Empty)
                {
                    MsgBox.ShowWarning("Định danh được chọn không có Id hợp lệ.");
                    return;
                }

                // Mở form điều chỉnh với selectedDto.Id
                using (var form = new FrmProductVariantIdentifierAddEdit(selectedDto.Id))
                {
                    form.ShowDialog(this);
                    
                    // Reload dữ liệu sau khi form đóng (nếu có thay đổi)
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                _logger.Error("BarButtonItem3_ItemClick: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi mở form điều chỉnh: {ex.Message}");
            }
        }

        /// <summary>
        /// Event handler cho nút Xóa
        /// </summary>
        private void BarButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                var selectedCount = ProductVariantIdentifierDtoGridView.SelectedRowsCount;
                if (selectedCount == 0)
                {
                    MsgBox.ShowWarning("Vui lòng chọn ít nhất một định danh để xóa.");
                    return;
                }

                var selectedRowHandles = ProductVariantIdentifierDtoGridView.GetSelectedRows();
                var selectedDtos = selectedRowHandles
                    .Select(handle => ProductVariantIdentifierDtoGridView.GetRow(handle) as ProductVariantIdentifierDto)
                    .Where(dto => dto != null && dto.Id != Guid.Empty)
                    .ToList();

                if (selectedDtos.Count == 0)
                {
                    MsgBox.ShowWarning("Không có định danh hợp lệ để xóa.");
                    return;
                }

                // Hiển thị confirmation dialog
                var confirmMessage = selectedDtos.Count == 1
                    ? $"Bạn có chắc muốn xóa định danh:\n<b>{GetIdentifierDisplayName(selectedDtos[0])}</b>?\n\n" +
                      "Hành động này không thể hoàn tác!"
                    : $"Bạn có chắc muốn xóa <b>{selectedDtos.Count}</b> định danh?\n\n" +
                      "Hành động này không thể hoàn tác!";

                if (!MsgBox.ShowYesNo(confirmMessage, "Xác nhận xóa"))
                {
                    return;
                }

                // Thực hiện xóa
                var deletedCount = 0;
                foreach (var dto in selectedDtos)
                {
                    try
                    {
                        _productVariantIdentifierBll.Delete(dto.Id);
                        deletedCount++;
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"BarButtonItem4_ItemClick: Lỗi xóa định danh {dto.Id}: {ex.Message}", ex);
                    }
                }

                // Reload data sau khi xóa thành công
                LoadData();

                if (deletedCount == selectedDtos.Count)
                {
                    MsgBox.ShowSuccess($"Đã xóa thành công {deletedCount} định danh.");
                }
                else
                {
                    MsgBox.ShowWarning($"Đã xóa {deletedCount}/{selectedDtos.Count} định danh. Vui lòng kiểm tra lại.");
                }
            }
            catch (Exception ex)
            {
                _logger.Error("BarButtonItem4_ItemClick: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi xóa định danh: {ex.Message}");
            }
        }

        /// <summary>
        /// Event handler cho nút In tem
        /// </summary>
        private void BarButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                var selectedCount = ProductVariantIdentifierDtoGridView.SelectedRowsCount;
                if (selectedCount == 0)
                {
                    MsgBox.ShowWarning("Vui lòng chọn ít nhất một định danh để in tem.");
                    return;
                }

                // TODO: Implement print label functionality
                MsgBox.ShowWarning("Chức năng in tem đang được phát triển.");
            }
            catch (Exception ex)
            {
                _logger.Error("BarButtonItem5_ItemClick: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi: {ex.Message}");
            }
        }

        /// <summary>
        /// Event handler khi double click trên GridView
        /// </summary>
        private void ProductVariantIdentifierDtoGridView_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                // Mở form điều chỉnh khi double click
                BarButtonItem3_ItemClick(sender, null);
            }
            catch (Exception ex)
            {
                _logger.Error("ProductVariantIdentifierDtoGridView_DoubleClick: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi: {ex.Message}");
            }
        }

        /// <summary>
        /// Event handler khi row được chọn thay đổi
        /// </summary>
        private void ProductVariantIdentifierDtoGridView_FocusedRowChanged(object sender,
            DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                UpdateButtonStates();
            }
            catch (Exception ex)
            {
                _logger.Error("ProductVariantIdentifierDtoGridView_FocusedRowChanged: Exception occurred", ex);
            }
        }

        /// <summary>
        /// Event handler khi selection thay đổi trên GridView
        /// </summary>
        private void ProductVariantIdentifierDtoGridView_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                UpdateButtonStates();
                UpdateDataSummary();
            }
            catch (Exception ex)
            {
                _logger.Error("ProductVariantIdentifierDtoGridView_SelectionChanged: Exception occurred", ex);
            }
        }

        /// <summary>
        /// Xử lý sự kiện vẽ số thứ tự dòng cho GridView
        /// </summary>
        private void ProductVariantIdentifierDtoGridView_CustomDrawRowIndicator(object sender,
            RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                GridViewHelper.CustomDrawRowIndicator(ProductVariantIdentifierDtoGridView, e);
            }
            catch (Exception ex)
            {
                _logger.Error("ProductVariantIdentifierDtoGridView_CustomDrawRowIndicator: Exception occurred", ex);
            }
        }

        /// <summary>
        /// Xử lý sự kiện custom column display text
        /// </summary>
        private void ProductVariantIdentifierDtoGridView_CustomColumnDisplayText(object sender,
            DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            try
            {
                // Xử lý cột Status - hiển thị description với màu sắc thay vì enum value
                if (e.Column == colStatus)
                {
                    ProductVariantIdentifierStatusEnum statusValue;

                    // Convert giá trị về enum
                    if (e.Value is ProductVariantIdentifierStatusEnum enumValue)
                    {
                        statusValue = enumValue;
                    }
                    else if (e.Value is int intValue && Enum.IsDefined(typeof(ProductVariantIdentifierStatusEnum), intValue))
                    {
                        statusValue = (ProductVariantIdentifierStatusEnum)intValue;
                    }
                    else if (e.Value is string stringValue)
                    {
                        // Nếu là string, thử strip HTML và convert
                        var cleanString = StripHtmlTags(stringValue);
                        var statusEnum = GetStatusEnumFromDescription(cleanString);
                        if (!statusEnum.HasValue)
                        {
                            return; // Không thể convert, giữ nguyên giá trị
                        }
                        statusValue = statusEnum.Value;
                    }
                    else
                    {
                        return;
                    }

                    // Lấy Description và màu sắc
                    var description = ApplicationEnumUtils.GetDescription(statusValue);
                    var colorName = GetStatusColor(statusValue);

                    // Tạo HTML với màu sắc theo chuẩn DevExpress
                    e.DisplayText = $"<color='{colorName}'>{description}</color>";
                }

                // Xử lý cột IsActive - hiển thị "Có"/"Không" thay vì True/False
                if (e.Column == colIsActive)
                {
                    if (e.Value is bool isActive)
                    {
                        e.DisplayText = isActive ? "Có" : "Không";
                    }
                    else if (e.Value == null || e.Value == DBNull.Value)
                    {
                        e.DisplayText = string.Empty;
                    }
                }

                // Xử lý cột UpdatedDate - ẩn nếu null
                if (e.Column == colUpdatedDate)
                {
                    if (e.Value == null || e.Value == DBNull.Value)
                    {
                        e.DisplayText = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("ProductVariantIdentifierDtoGridView_CustomColumnDisplayText: Exception occurred", ex);
            }
        }

        /// <summary>
        /// Cấu hình GridView để cho phép edit colStatus, colNotes và colIsActive
        /// </summary>
        private void ConfigureGridViewEditing()
        {
            try
            {
                // Cho phép edit colStatus, colNotes và colIsActive
                colStatus.OptionsColumn.AllowEdit = true;
                colStatus.OptionsColumn.ReadOnly = false;
                colNotes.OptionsColumn.AllowEdit = true;
                colNotes.OptionsColumn.ReadOnly = false;
                colIsActive.OptionsColumn.AllowEdit = true;
                colIsActive.OptionsColumn.ReadOnly = false;
            }
            catch (Exception ex)
            {
                _logger.Error("ConfigureGridViewEditing: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi cấu hình GridView: {ex.Message}");
            }
        }

        /// <summary>
        /// Event handler để validate và convert giá trị trước khi set vào property
        /// Xử lý conversion từ string (Description) sang enum
        /// </summary>
        private void ProductVariantIdentifierDtoGridView_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            try
            {
                var view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
                if (view == null) return;

                var focusedColumn = view.FocusedColumn;
                if (focusedColumn != colStatus) return;

                // Nếu giá trị đã là enum, giữ nguyên
                if (e.Value is ProductVariantIdentifierStatusEnum)
                {
                    e.Valid = true;
                }
                // Nếu giá trị là int, convert về enum
                else if (e.Value is int intValue && Enum.IsDefined(typeof(ProductVariantIdentifierStatusEnum), intValue))
                {
                    e.Value = (ProductVariantIdentifierStatusEnum)intValue;
                    e.Valid = true;
                }
                // Nếu giá trị là string (Description có thể chứa HTML), convert về enum
                else if (e.Value is string statusDescription)
                {
                    // Strip HTML tags nếu có
                    var cleanDescription = StripHtmlTags(statusDescription);
                    var statusEnum = GetStatusEnumFromDescription(cleanDescription);
                    if (statusEnum.HasValue)
                    {
                        // Set lại giá trị là enum để DevExpress có thể bind đúng
                        e.Value = statusEnum.Value;
                        e.Valid = true;
                    }
                    else
                    {
                        _logger.Warning($"ProductVariantIdentifierDtoGridView_ValidatingEditor: Cannot convert status description '{statusDescription}' to enum");
                        e.ErrorText = $"Không thể chuyển đổi trạng thái '{statusDescription}'";
                        e.Valid = false;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("ProductVariantIdentifierDtoGridView_ValidatingEditor: Exception occurred", ex);
                e.ErrorText = $"Lỗi xử lý giá trị: {ex.Message}";
                e.Valid = false;
            }
        }

        /// <summary>
        /// Lấy enum value từ Description string (có thể chứa HTML tags)
        /// </summary>
        /// <param name="description">Description string (có thể chứa HTML tags)</param>
        /// <returns>Enum value hoặc null nếu không tìm thấy</returns>
        private ProductVariantIdentifierStatusEnum? GetStatusEnumFromDescription(string description)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(description))
                    return null;

                // Strip HTML tags nếu có
                var cleanDescription = StripHtmlTags(description);

                // Duyệt qua tất cả các giá trị enum để tìm Description khớp
                foreach (ProductVariantIdentifierStatusEnum enumValue in Enum.GetValues(typeof(ProductVariantIdentifierStatusEnum)))
                {
                    var field = enumValue.GetType().GetField(enumValue.ToString());
                    if (field != null)
                    {
                        if (field.GetCustomAttributes(typeof(DescriptionAttribute), false)
                                .FirstOrDefault() is DescriptionAttribute attribute)
                        {
                            if (string.Equals(attribute.Description, cleanDescription, StringComparison.OrdinalIgnoreCase))
                            {
                                return enumValue;
                            }
                        }
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.Error($"GetStatusEnumFromDescription: Exception occurred for '{description}'", ex);
                return null;
            }
        }

        /// <summary>
        /// Loại bỏ HTML tags từ string
        /// </summary>
        /// <param name="htmlString">String chứa HTML tags</param>
        /// <returns>String không có HTML tags</returns>
        private string StripHtmlTags(string htmlString)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(htmlString))
                    return htmlString;

                // Loại bỏ các HTML tags phổ biến của DevExpress: <color>, <b>, <i>, <size>, etc.
                var result = htmlString;

                // Loại bỏ <color='...'> và </color>
                result = System.Text.RegularExpressions.Regex.Replace(result, @"<color=['""][^'""]*['""]>", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result, @"</color>", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // Loại bỏ các tags khác nếu có
                result = System.Text.RegularExpressions.Regex.Replace(result, @"<[^>]+>", "");

                return result.Trim();
            }
            catch (Exception ex)
            {
                _logger.Error($"StripHtmlTags: Exception occurred for '{htmlString}'", ex);
                return htmlString;
            }
        }

        /// <summary>
        /// Event handler khi giá trị cell thay đổi (khi user edit)
        /// Xử lý cả colStatus và colNotes
        /// </summary>
        private void ProductVariantIdentifierDtoGridView_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                // Chỉ xử lý khi edit cột Status, Notes hoặc IsActive
                if (e.Column != colStatus && e.Column != colNotes && e.Column != colIsActive)
                    return;

                var rowHandle = e.RowHandle;
                if (rowHandle < 0) return;

                var dto = ProductVariantIdentifierDtoGridView.GetRow(rowHandle) as ProductVariantIdentifierDto;
                if (dto == null || dto.Id == Guid.Empty)
                {
                    MsgBox.ShowWarning("Không thể lấy thông tin định danh để cập nhật.");
                    return;
                }

                bool hasChanges = false;

                // Xử lý cột Status
                if (e.Column == colStatus)
                {
                    // Kiểm tra giá trị mới có hợp lệ không
                    if (e.Value is ProductVariantIdentifierStatusEnum newStatus)
                    {
                        // Cập nhật Status trong DTO
                        dto.Status = newStatus;
                        dto.StatusDate = DateTime.Now;
                        // TODO: Có thể cần cập nhật StatusChangedBy từ current user
                        hasChanges = true;
                        _logger.Debug($"ProductVariantIdentifierDtoGridView_CellValueChanged: Status changed to {newStatus} for {dto.Id}");
                    }
                }

                // Xử lý cột Notes
                if (e.Column == colNotes)
                {
                    // Cập nhật Notes trong DTO
                    dto.Notes = e.Value?.ToString() ?? string.Empty;
                    hasChanges = true;
                    _logger.Debug($"ProductVariantIdentifierDtoGridView_CellValueChanged: Notes changed for {dto.Id}");
                }

                // Xử lý cột IsActive
                if (e.Column == colIsActive)
                {
                    // Cập nhật IsActive trong DTO
                    if (e.Value is bool isActive)
                    {
                        dto.IsActive = isActive;
                    }
                    else if (e.Value != null)
                    {
                        // Convert các giá trị khác về bool
                        dto.IsActive = Convert.ToBoolean(e.Value);
                    }
                    hasChanges = true;
                    _logger.Debug($"ProductVariantIdentifierDtoGridView_CellValueChanged: IsActive changed to {dto.IsActive} for {dto.Id}");
                }

                // Lưu vào database nếu có thay đổi
                if (hasChanges)
                {
                    try
                    {
                        _productVariantIdentifierBll.SaveOrUpdate(dto);
                        _logger.Info($"Đã cập nhật {e.Column.FieldName} cho ProductVariantIdentifier {dto.Id}");
                        
                        // Refresh grid để hiển thị dữ liệu mới nhất
                        ProductVariantIdentifierDtoGridView.RefreshRow(rowHandle);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"Lỗi khi lưu thay đổi {e.Column.FieldName}: {ex.Message}", ex);
                        MsgBox.ShowError($"Lỗi khi lưu thay đổi: {ex.Message}");
                        
                        // Rollback - reload dữ liệu từ database
                        LoadData();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("ProductVariantIdentifierDtoGridView_CellValueChanged: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi khi xử lý thay đổi: {ex.Message}");
            }
        }

        #endregion

        #region ========== DATA LOADING ==========

        /// <summary>
        /// Load dữ liệu định danh sản phẩm
        /// </summary>
        private void LoadData()
        {
            try
            {
                // Hiển thị SplashScreen
                SplashScreenHelper.ShowWaitingSplashScreen();

                try
                {
                    // Lấy dữ liệu từ BLL và lưu vào bộ dữ liệu gốc
                    _allData = _productVariantIdentifierBll.GetAll() ?? new List<ProductVariantIdentifierDto>();

                    // Áp dụng tiêu chí lọc hiện tại
                    ApplyFilterAndBind();
                }
                finally
                {
                    // Đóng SplashScreen
                    SplashScreenHelper.CloseSplashScreen();
                }
            }
            catch (Exception ex)
            {
                _logger.Error("LoadData: Exception occurred", ex);
                SplashScreenHelper.CloseSplashScreen();
                MsgBox.ShowError($"Lỗi tải dữ liệu: {ex.Message}");
            }
        }

        /// <summary>
        /// Cập nhật trạng thái các nút
        /// </summary>
        private void UpdateButtonStates()
        {
            try
            {
                var focusedRowHandle = ProductVariantIdentifierDtoGridView.FocusedRowHandle;
                var hasSelection = focusedRowHandle >= 0;
                var selectedCount = ProductVariantIdentifierDtoGridView.SelectedRowsCount;

                // Các nút chỉ cho phép 1 dòng: LS Nhập/Xuất, Điều chỉnh
                barButtonItem1.Enabled = hasSelection;
                EditBarButtonItem.Enabled = hasSelection;

                // Các nút cho phép nhiều dòng: Xóa, In tem
                barButtonItem4.Enabled = selectedCount > 0;
                barButtonItem5.Enabled = selectedCount > 0;
            }
            catch (Exception ex)
            {
                _logger.Error("UpdateButtonStates: Exception occurred", ex);
            }
        }

        /// <summary>
        /// Cập nhật thông tin tổng kết dữ liệu với HTML formatting
        /// </summary>
        private void UpdateDataSummary()
        {
            try
            {
                var totalRows = ProductVariantIdentifierDtoGridView.RowCount;
                var selectedRows = ProductVariantIdentifierDtoGridView.SelectedRowsCount;

                // Cập nhật tổng số định danh với HTML formatting
                if (DataSummaryBarStaticItem != null)
                {
                    if (totalRows == 0)
                    {
                        DataSummaryBarStaticItem.Caption = @"<color=#757575><i>Chưa có dữ liệu</i></color>";
                    }
                    else
                    {
                        DataSummaryBarStaticItem.Caption =
                            $@"<size=9><color=#757575>Tổng:</color></size> " +
                            $@"<b><color=blue>{totalRows:N0}</color></b> " +
                            $@"<size=9><color=#757575>định danh</color></size>";
                    }
                }

                // Cập nhật số dòng đã chọn với HTML formatting
                if (SelectedRowBarStaticItem != null)
                {
                    if (selectedRows > 0)
                    {
                        SelectedRowBarStaticItem.Caption =
                            $@"<size=9><color=#757575>Đã chọn:</color></size> " +
                            $@"<b><color=blue>{selectedRows:N0}</color></b> " +
                            $@"<size=9><color=#757575>dòng</color></size>";
                    }
                    else
                    {
                        SelectedRowBarStaticItem.Caption = @"<color=#757575><i>Chưa chọn dòng nào</i></color>";
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("UpdateDataSummary: Exception occurred", ex);
            }
        }

        #endregion

        #region ========== HELPER METHODS ==========

        /// <summary>
        /// Áp dụng bộ lọc đang chọn và bind ra grid
        /// </summary>
        private void ApplyFilterAndBind()
        {
            try
            {
                IEnumerable<ProductVariantIdentifierDto> query = _allData;

                // Lọc theo Status
                if (_filterCriteria.Status.HasValue)
                {
                    query = query.Where(x => x.Status == _filterCriteria.Status.Value);
                }

                // Lọc theo ngày cập nhật
                if (_filterCriteria.UpdatedFrom.HasValue)
                {
                    query = query.Where(x => x.UpdatedDate.HasValue && x.UpdatedDate.Value.Date >= _filterCriteria.UpdatedFrom.Value.Date);
                }
                if (_filterCriteria.UpdatedTo.HasValue)
                {
                    query = query.Where(x => x.UpdatedDate.HasValue && x.UpdatedDate.Value.Date <= _filterCriteria.UpdatedTo.Value.Date);
                }

                // Lọc theo từ khóa định danh (search trên tất cả identifier)
                if (!string.IsNullOrWhiteSpace(_filterCriteria.IdentifierKeyword))
                {
                    var keyword = _filterCriteria.IdentifierKeyword.Trim().ToLowerInvariant();
                    query = query.Where(x =>
                        (x.SerialNumber ?? string.Empty).ToLowerInvariant().Contains(keyword) ||
                        (x.PartNumber ?? string.Empty).ToLowerInvariant().Contains(keyword) ||
                        (x.QRCode ?? string.Empty).ToLowerInvariant().Contains(keyword) ||
                        (x.SKU ?? string.Empty).ToLowerInvariant().Contains(keyword) ||
                        (x.RFID ?? string.Empty).ToLowerInvariant().Contains(keyword) ||
                        (x.MACAddress ?? string.Empty).ToLowerInvariant().Contains(keyword) ||
                        (x.IMEI ?? string.Empty).ToLowerInvariant().Contains(keyword) ||
                        (x.AssetTag ?? string.Empty).ToLowerInvariant().Contains(keyword) ||
                        (x.LicenseKey ?? string.Empty).ToLowerInvariant().Contains(keyword) ||
                        (x.UPC ?? string.Empty).ToLowerInvariant().Contains(keyword) ||
                        (x.EAN ?? string.Empty).ToLowerInvariant().Contains(keyword) ||
                        (x.ID ?? string.Empty).ToLowerInvariant().Contains(keyword) ||
                        (x.OtherIdentifier ?? string.Empty).ToLowerInvariant().Contains(keyword)
                    );
                }

                // Lọc theo từ khóa sản phẩm
                if (!string.IsNullOrWhiteSpace(_filterCriteria.ProductVariantKeyword))
                {
                    var keyword = _filterCriteria.ProductVariantKeyword.Trim().ToLowerInvariant();
                    query = query.Where(x =>
                        (x.ProductVariantFullName ?? string.Empty).ToLowerInvariant().Contains(keyword));
                }

                var filtered = query.ToList();

                productVariantIdentifierDtoBindingSource.DataSource = filtered;
                productVariantIdentifierDtoBindingSource.ResetBindings(false);

                UpdateDataSummary();
                UpdateButtonStates();
            }
            catch (Exception ex)
            {
                _logger.Error("ApplyFilterAndBind: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi áp dụng bộ lọc: {ex.Message}");
            }
        }

        /// <summary>
        /// Reset bộ lọc về mặc định và apply
        /// </summary>
        private void ResetFilterAndApply()
        {
            _filterCriteria.Reset();
            ApplyFilterAndBind();
        }

        /// <summary>
        /// Hiển thị input box nhập text (TextEdit)
        /// </summary>
        private string ShowTextInput(string prompt, string caption, string defaultValue = "")
        {
            var args = new XtraInputBoxArgs
            {
                Caption = caption,
                Prompt = prompt,
                DefaultButtonIndex = 0,
                DefaultResponse = defaultValue ?? string.Empty
            };
            var editor = new TextEdit();
            args.Editor = editor;
            var result = XtraInputBox.Show(args);
            return result?.ToString();
        }

        /// <summary>
        /// Hiển thị combobox chọn Status
        /// </summary>
        private ProductVariantIdentifierStatusEnum? ShowStatusSelector()
        {
            var args = new XtraInputBoxArgs
            {
                Caption = "Lọc theo tình trạng",
                Prompt = "Chọn tình trạng",
                DefaultButtonIndex = 0
            };

            var combo = new ComboBoxEdit
            {
                Properties =
                {
                    TextEditStyle = TextEditStyles.DisableTextEditor,
                    AllowNullInput = DevExpress.Utils.DefaultBoolean.True
                }
            };

            foreach (ProductVariantIdentifierStatusEnum value in Enum.GetValues(typeof(ProductVariantIdentifierStatusEnum)))
            {
                var desc = ApplicationEnumUtils.GetDescription(value);
                combo.Properties.Items.Add(desc);
            }

            // đặt mặc định nếu có
            if (_filterCriteria.Status.HasValue)
            {
                var currentDesc = ApplicationEnumUtils.GetDescription(_filterCriteria.Status.Value);
                combo.EditValue = currentDesc;
            }

            args.Editor = combo;

            var result = XtraInputBox.Show(args);
            if (result == null) return null; // cancel

            var selectedDesc = result.ToString();
            foreach (ProductVariantIdentifierStatusEnum value in Enum.GetValues(typeof(ProductVariantIdentifierStatusEnum)))
            {
                if (string.Equals(ApplicationEnumUtils.GetDescription(value), selectedDesc, StringComparison.OrdinalIgnoreCase))
                    return value;
            }

            return null;
        }

        /// <summary>
        /// Tiêu chí lọc dữ liệu
        /// </summary>
        private class FilterCriteria
        {
            public ProductVariantIdentifierStatusEnum? Status { get; set; }
            public DateTime? UpdatedFrom { get; set; }
            public DateTime? UpdatedTo { get; set; }
            public string IdentifierKeyword { get; set; }
            public string ProductVariantKeyword { get; set; }

            public void Reset()
            {
                Status = null;
                UpdatedFrom = null;
                UpdatedTo = null;
                IdentifierKeyword = null;
                ProductVariantKeyword = null;
            }
        }

        /// <summary>
        /// Load danh sách ProductVariantIdentifierStatusEnum vào RepositoryItemComboBox
        /// </summary>
        private void LoadProductVariantIdentifierStatusEnumRepositoryComboBox()
        {
            try
            {
                ProductVariantIdentifierStatusEnumComboBox.Items.Clear();

                // Load tất cả các giá trị enum với màu sắc HTML
                foreach (ProductVariantIdentifierStatusEnum value in Enum.GetValues(typeof(ProductVariantIdentifierStatusEnum)))
                {
                    int index = ApplicationEnumUtils.GetValue(value);
                    string description = ApplicationEnumUtils.GetDescription(value);
                    string colorHex = GetStatusColor(value);

                    // Tạo HTML với màu sắc theo chuẩn DevExpress
                    string itemName = $"<color='{colorHex}'>{description}</color>";

                    // Insert vào ComboBox với index để sắp xếp đúng thứ tự
                    ProductVariantIdentifierStatusEnumComboBox.Items.Insert(index, itemName);
                }

                // Cấu hình ComboBox
                ProductVariantIdentifierStatusEnumComboBox.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                ProductVariantIdentifierStatusEnumComboBox.ShowDropDown = DevExpress.XtraEditors.Controls.ShowDropDown.SingleClick;

                // Sử dụng CustomDisplayText để hiển thị Description với màu sắc
                ProductVariantIdentifierStatusEnumComboBox.CustomDisplayText += ProductVariantIdentifierStatusEnumRepositoryComboBox_CustomDisplayText;
            }
            catch (Exception ex)
            {
                _logger.Error($"LoadProductVariantIdentifierStatusEnumRepositoryComboBox: Exception occurred - {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy màu sắc tương ứng với trạng thái ProductVariantIdentifier
        /// </summary>
        /// <param name="status">Trạng thái ProductVariantIdentifier</param>
        /// <returns>Tên màu (color name) theo chuẩn DevExpress</returns>
        private string GetStatusColor(ProductVariantIdentifierStatusEnum status)
        {
            return status switch
            {
                ProductVariantIdentifierStatusEnum.AtVnsWarehouse => "green",           // Green - Tại kho VNS
                ProductVariantIdentifierStatusEnum.ExportedToCustomer => "blue",          // Blue - Đã xuất cho KH
                ProductVariantIdentifierStatusEnum.InstallingAtCustomerSite => "orange", // Orange - Đang lắp đặt tại site KH
                ProductVariantIdentifierStatusEnum.UnderWarrantyAtSupplier => "purple",  // Purple - Đang gửi Bảo hành NCC
                ProductVariantIdentifierStatusEnum.DamagedAtVnsWarehouse => "red",      // Red - Đã hư hỏng (Tại kho VNS)
                ProductVariantIdentifierStatusEnum.Disposed => "gray",                   // Gray - Đã thanh lý
                _ => "black"                                                             // Default - Black
            };
        }

        /// <summary>
        /// Event handler để hiển thị Description với màu sắc trong RepositoryItemComboBox
        /// </summary>
        private void ProductVariantIdentifierStatusEnumRepositoryComboBox_CustomDisplayText(object sender, DevExpress.XtraEditors.Controls.CustomDisplayTextEventArgs e)
        {
            try
            {
                if (e.Value == null) return;

                ProductVariantIdentifierStatusEnum statusValue;

                // Nếu giá trị là string (Description với HTML), convert về enum
                if (e.Value is string stringValue)
                {
                    var statusEnum = GetStatusEnumFromDescription(stringValue);
                    if (!statusEnum.HasValue)
                    {
                        e.DisplayText = stringValue; // Giữ nguyên nếu không convert được
                        return;
                    }
                    statusValue = statusEnum.Value;
                }
                else if (e.Value is ProductVariantIdentifierStatusEnum enumValue)
                {
                    statusValue = enumValue;
                }
                else if (e.Value is int intValue && Enum.IsDefined(typeof(ProductVariantIdentifierStatusEnum), intValue))
                {
                    statusValue = (ProductVariantIdentifierStatusEnum)intValue;
                }
                else
                {
                    return;
                }

                // Lấy Description và màu sắc
                var description = ApplicationEnumUtils.GetDescription(statusValue);
                var colorHex = GetStatusColor(statusValue);

                // Tạo HTML với màu sắc theo chuẩn DevExpress
                e.DisplayText = $"<color='{colorHex}'>{description}</color>";
            }
            catch (Exception ex)
            {
                _logger.Error($"ProductVariantIdentifierStatusEnumRepositoryComboBox_CustomDisplayText: Exception occurred - {ex.Message}", ex);
                // Nếu có lỗi, hiển thị giá trị mặc định
                e.DisplayText = e.Value?.ToString() ?? string.Empty;
            }
        }

        /// <summary>
        /// Lấy tên hiển thị của định danh từ DTO
        /// </summary>
        private string GetIdentifierDisplayName(ProductVariantIdentifierDto dto)
        {
            if (dto == null) return "N/A";

            // Ưu tiên hiển thị SerialNumber, nếu không có thì hiển thị định danh đầu tiên có giá trị
            if (!string.IsNullOrWhiteSpace(dto.SerialNumber))
                return $"Serial: {dto.SerialNumber}";

            if (!string.IsNullOrWhiteSpace(dto.PartNumber))
                return $"Part: {dto.PartNumber}";

            if (!string.IsNullOrWhiteSpace(dto.SKU))
                return $"SKU: {dto.SKU}";

            if (!string.IsNullOrWhiteSpace(dto.ProductVariantFullName))
                return dto.ProductVariantFullName;

            return dto.Id.ToString();
        }

        #endregion
    }
}