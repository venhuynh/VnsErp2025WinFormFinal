using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
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
                if (LocToanBoBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        LocToanBoBarButtonItem,
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
                if (barButtonItem2 != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        barButtonItem2,
                        title: @"<b><color=Green>➕ Thêm mới</color></b>",
                        content: @"Thêm mới định danh sản phẩm vào hệ thống.<br/><br/><b>Chức năng:</b><br/>• Mở form thêm mới định danh sản phẩm<br/>• Cho phép nhập các loại định danh: SerialNumber, PartNumber, QRCode, SKU, RFID, MACAddress, IMEI, AssetTag, LicenseKey, UPC, EAN, ID, OtherIdentifier<br/><br/><color=Gray>Lưu ý:</color> Chức năng này đang được phát triển."
                    );
                }

                // SuperTip cho nút Điều chỉnh
                if (barButtonItem3 != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        barButtonItem3,
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
                LocToanBoBarButtonItem.ItemClick += LocToanBoBarButtonItem_ItemClick;
                ExportFileBarButtonItem.ItemClick += ExportFileBarButtonItem_ItemClick;
                barButtonItem1.ItemClick += BarButtonItem1_ItemClick;
                barButtonItem2.ItemClick += BarButtonItem2_ItemClick;
                barButtonItem3.ItemClick += BarButtonItem3_ItemClick;
                barButtonItem4.ItemClick += BarButtonItem4_ItemClick;
                barButtonItem5.ItemClick += BarButtonItem5_ItemClick;

                // GridView events
                ProductVariantIdentifierDtoGridView.DoubleClick += ProductVariantIdentifierDtoGridView_DoubleClick;
                ProductVariantIdentifierDtoGridView.FocusedRowChanged += ProductVariantIdentifierDtoGridView_FocusedRowChanged;
                ProductVariantIdentifierDtoGridView.SelectionChanged += ProductVariantIdentifierDtoGridView_SelectionChanged;
                ProductVariantIdentifierDtoGridView.CustomDrawRowIndicator += ProductVariantIdentifierDtoGridView_CustomDrawRowIndicator;
                ProductVariantIdentifierDtoGridView.CustomColumnDisplayText += ProductVariantIdentifierDtoGridView_CustomColumnDisplayText;
                ProductVariantIdentifierDtoGridView.CellValueChanged += ProductVariantIdentifierDtoGridView_CellValueChanged;

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
                LoadData();
            }
            catch (Exception ex)
            {
                _logger.Error("LocToanBoBarButtonItem_ItemClick: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi tải dữ liệu: {ex.Message}");
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
                // TODO: Mở form thêm mới định danh
                // Có thể cần StockInOutProductHistoryDto hoặc ProductVariantId
                MsgBox.ShowWarning("Chức năng thêm mới đang được phát triển.");
            }
            catch (Exception ex)
            {
                _logger.Error("BarButtonItem2_ItemClick: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi: {ex.Message}");
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

                // TODO: Mở form điều chỉnh với selectedDto và selectedDto.Id
                // Cần tạo StockInOutProductHistoryDto từ ProductVariantIdentifierDto hoặc lấy từ database
                MsgBox.ShowWarning("Chức năng điều chỉnh đang được phát triển.");
            }
            catch (Exception ex)
            {
                _logger.Error("BarButtonItem3_ItemClick: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi: {ex.Message}");
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
                // Xử lý cột Status - hiển thị description thay vì enum value
                if (e.Column == colStatus && e.Value is ProductVariantIdentifierStatusEnum status)
                {
                    var field = status.GetType().GetField(status.ToString());
                    if (field != null)
                    {
                        var attribute = field.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false)
                            .FirstOrDefault() as System.ComponentModel.DescriptionAttribute;
                        if (attribute != null)
                        {
                            e.DisplayText = attribute.Description;
                        }
                    }
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
        /// Event handler khi giá trị cell thay đổi (khi user edit)
        /// </summary>
        private void ProductVariantIdentifierDtoGridView_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                // Chỉ xử lý khi edit cột Status
                if (e.Column == colStatus)
                {
                    var rowHandle = e.RowHandle;
                    if (rowHandle < 0) return;

                    var dto = ProductVariantIdentifierDtoGridView.GetRow(rowHandle) as ProductVariantIdentifierDto;
                    if (dto == null || dto.Id == Guid.Empty)
                    {
                        MsgBox.ShowWarning("Không thể lấy thông tin định danh để cập nhật.");
                        return;
                    }

                    // Kiểm tra giá trị mới có hợp lệ không
                    if (e.Value is ProductVariantIdentifierStatusEnum newStatus)
                    {
                        // Cập nhật Status trong DTO
                        dto.Status = newStatus;
                        dto.StatusDate = DateTime.Now;
                        // TODO: Có thể cần cập nhật StatusChangedBy từ current user

                        // Lưu vào database
                        try
                        {
                            _productVariantIdentifierBll.SaveOrUpdate(dto);
                            _logger.Info($"Đã cập nhật Status cho ProductVariantIdentifier {dto.Id} thành {newStatus}");
                            
                            // Refresh grid để hiển thị dữ liệu mới nhất
                            ProductVariantIdentifierDtoGridView.RefreshRow(rowHandle);
                        }
                        catch (Exception ex)
                        {
                            _logger.Error($"Lỗi khi lưu thay đổi Status: {ex.Message}", ex);
                            MsgBox.ShowError($"Lỗi khi lưu thay đổi: {ex.Message}");
                            
                            // Rollback - reload dữ liệu từ database
                            LoadData();
                        }
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
                    // Lấy dữ liệu từ BLL
                    var dtos = _productVariantIdentifierBll.GetAll();

                    // Update UI
                    productVariantIdentifierDtoBindingSource.DataSource = dtos;
                    productVariantIdentifierDtoBindingSource.ResetBindings(false);

                    // Update summary
                    UpdateDataSummary();
                    UpdateButtonStates();
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
                barButtonItem3.Enabled = hasSelection;

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
        /// Load danh sách ProductVariantIdentifierStatusEnum vào RepositoryItemComboBox
        /// </summary>
        private void LoadProductVariantIdentifierStatusEnumRepositoryComboBox()
        {
            try
            {
                ProductVariantIdentifierStatusEnumComboBox.Items.Clear();

                // Load tất cả các giá trị enum
                foreach (ProductVariantIdentifierStatusEnum value in Enum.GetValues(typeof(ProductVariantIdentifierStatusEnum)))
                {
                    ProductVariantIdentifierStatusEnumComboBox.Items.Add(value);
                }

                // Cấu hình ComboBox
                ProductVariantIdentifierStatusEnumComboBox.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                ProductVariantIdentifierStatusEnumComboBox.ShowDropDown = DevExpress.XtraEditors.Controls.ShowDropDown.SingleClick;

                // Sử dụng CustomDisplayText để hiển thị Description thay vì tên enum
                ProductVariantIdentifierStatusEnumComboBox.CustomDisplayText += ProductVariantIdentifierStatusEnumRepositoryComboBox_CustomDisplayText;
            }
            catch (Exception ex)
            {
                _logger.Error($"LoadProductVariantIdentifierStatusEnumRepositoryComboBox: Exception occurred - {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Event handler để hiển thị Description thay vì tên enum trong RepositoryItemComboBox
        /// </summary>
        private void ProductVariantIdentifierStatusEnumRepositoryComboBox_CustomDisplayText(object sender, DevExpress.XtraEditors.Controls.CustomDisplayTextEventArgs e)
        {
            try
            {
                if (e.Value is ProductVariantIdentifierStatusEnum enumValue)
                {
                    // Lấy Description từ DescriptionAttribute
                    var field = enumValue.GetType().GetField(enumValue.ToString());
                    if (field != null)
                    {
                        if (field.GetCustomAttributes(typeof(DescriptionAttribute), false)
                                .FirstOrDefault() is DescriptionAttribute attribute)
                        {
                            e.DisplayText = attribute.Description;
                            return;
                        }
                    }
                    // Nếu không có Description, hiển thị tên enum
                    e.DisplayText = enumValue.ToString();
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"ProductVariantIdentifierStatusEnumRepositoryComboBox_CustomDisplayText: Exception occurred - {ex.Message}", ex);
                // Nếu có lỗi, hiển thị tên enum mặc định
                if (e.Value is ProductVariantIdentifierStatusEnum enumValue)
                {
                    e.DisplayText = enumValue.ToString();
                }
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