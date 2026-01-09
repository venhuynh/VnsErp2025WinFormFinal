using Bll.Inventory.StockTakking;
using Common.Common;
using Common.Helpers;
using Common.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DTO.Inventory.StockTakking;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Inventory.StockTakking
{
    public partial class FrmStocktakingMaster : XtraForm
    {
        #region ========== FIELDS & PROPERTIES ==========

        /// <summary>
        /// Business Logic Layer cho StocktakingMaster
        /// </summary>
        private readonly StocktakingMasterBll _stocktakingMasterBll = new StocktakingMasterBll();

        /// <summary>
        /// Logger để ghi log các sự kiện
        /// </summary>
        private readonly ILogger _logger = LoggerFactory.CreateLogger(LogCategory.UI);

        /// <summary>
        /// Bộ dữ liệu đầy đủ để phục vụ bộ lọc
        /// </summary>
        private List<StocktakingMasterDto> _allData = new List<StocktakingMasterDto>();

        #endregion

        #region ========== CONSTRUCTOR ==========

        public FrmStocktakingMaster()
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
                // SuperTip cho nút Danh sách
                if (LoadDataBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        LoadDataBarButtonItem,
                        title: @"<b><color=Blue>📋 Danh sách</color></b>",
                        content: @"Tải lại toàn bộ danh sách phiếu kiểm kho từ database.<br/><br/><b>Chức năng:</b><br/>• Load tất cả phiếu kiểm kho từ database<br/>• Refresh grid để hiển thị dữ liệu mới nhất<br/>• Cập nhật thống kê tổng hợp<br/><br/><color=Gray>Lưu ý:</color> Dữ liệu sẽ được tải từ database, có thể mất thời gian nếu có nhiều dữ liệu."
                    );
                }

                // SuperTip cho nút Xuất file
                if (ExportFileBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        ExportFileBarButtonItem,
                        title: @"<b><color=Green>📤 Xuất file</color></b>",
                        content: @"Xuất danh sách phiếu kiểm kho ra file Excel.<br/><br/><b>Chức năng:</b><br/>• Xuất dữ liệu hiện tại trong grid ra file Excel (.xlsx)<br/>• Hỗ trợ chọn đường dẫn lưu file<br/>• Tên file mặc định: <b>Bảng kiểm kho_YYYYMMDD_HHMMSS.xlsx</b><br/><br/><b>Định dạng:</b><br/>• File Excel (.xlsx)<br/>• Bao gồm tất cả các cột hiển thị trong grid<br/><br/><color=Gray>Lưu ý:</color> Chỉ xuất dữ liệu đang hiển thị trong grid."
                    );
                }

                // SuperTip cho nút Thêm mới
                if (AddNewBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        AddNewBarButtonItem,
                        title: @"<b><color=Green>➕ Thêm mới</color></b>",
                        content: @"Thêm mới phiếu kiểm kho vào hệ thống.<br/><br/><b>Chức năng:</b><br/>• Mở form thêm mới phiếu kiểm kho<br/>• Cho phép nhập thông tin phiếu kiểm kho<br/><br/><color=Gray>Lưu ý:</color> Chức năng này đang được phát triển."
                    );
                }

                // SuperTip cho nút Điều chỉnh
                if (EditBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        EditBarButtonItem,
                        title: @"<b><color=Orange>✏️ Điều chỉnh</color></b>",
                        content: @"Chỉnh sửa thông tin phiếu kiểm kho đã chọn.<br/><br/><b>Chức năng:</b><br/>• Mở form chỉnh sửa phiếu kiểm kho<br/>• Load dữ liệu từ phiếu kiểm kho được chọn<br/>• Cho phép sửa thông tin phiếu kiểm kho<br/><br/><b>Yêu cầu:</b><br/>• Phải chọn một phiếu kiểm kho<br/>• Phiếu kiểm kho phải có Id hợp lệ<br/><br/><color=Gray>Lưu ý:</color> Chức năng này đang được phát triển."
                    );
                }

                // SuperTip cho nút Xóa
                if (barButtonItem4 != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        barButtonItem4,
                        title: @"<b><color=Red>🗑️ Xóa</color></b>",
                        content: @"Xóa các phiếu kiểm kho được chọn.<br/><br/><b>Chức năng:</b><br/>• Xóa các phiếu kiểm kho được chọn khỏi database<br/>• Cho phép xóa nhiều phiếu kiểm kho cùng lúc<br/>• Hiển thị xác nhận trước khi xóa<br/><br/><b>Quy trình:</b><br/>1. Hiển thị xác nhận xóa<br/>2. Xóa từng phiếu kiểm kho được chọn<br/>3. Reload dữ liệu sau khi xóa<br/>4. Hiển thị kết quả<br/><br/><b>Yêu cầu:</b><br/>• Phải chọn ít nhất một phiếu kiểm kho<br/>• Phiếu kiểm kho phải có Id hợp lệ<br/><br/><color=Red>⚠️ Cảnh báo:</color> Hành động này không thể hoàn tác!"
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
                LoadDataBarButtonItem.ItemClick += LoadDataBarButtonItem_ItemClick;
                ExportFileBarButtonItem.ItemClick += ExportFileBarButtonItem_ItemClick;
                AddNewBarButtonItem.ItemClick += AddNewBarButtonItem_ItemClick;
                EditBarButtonItem.ItemClick += EditBarButtonItem_ItemClick;
                barButtonItem4.ItemClick += DeleteBarButtonItem_ItemClick;

                // GridView events
                ProductVariantIdentifierDtoGridView.DoubleClick += ProductVariantIdentifierDtoGridView_DoubleClick;
                ProductVariantIdentifierDtoGridView.FocusedRowChanged += ProductVariantIdentifierDtoGridView_FocusedRowChanged;
                ProductVariantIdentifierDtoGridView.SelectionChanged += ProductVariantIdentifierDtoGridView_SelectionChanged;
                ProductVariantIdentifierDtoGridView.CustomDrawRowIndicator += ProductVariantIdentifierDtoGridView_CustomDrawRowIndicator;

                // Form events
                Load += FrmStocktakingMaster_Load;
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
        private void FrmStocktakingMaster_Load(object sender, EventArgs e)
        {
            try
            {
                // Có thể thêm logic khởi tạo khi form load
            }
            catch (Exception ex)
            {
                _logger.Error("FrmStocktakingMaster_Load: Exception occurred", ex);
            }
        }

        /// <summary>
        /// Event handler cho nút Danh sách
        /// </summary>
        private void LoadDataBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                LoadData();
            }
            catch (Exception ex)
            {
                _logger.Error("LoadDataBarButtonItem_ItemClick: Exception occurred", ex);
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
                    saveDialog.FileName = $"Bảng kiểm kho_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

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
        /// Event handler cho nút Thêm mới
        /// </summary>
        private void AddNewBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                // Mở form thêm mới với OverlayManager (Guid.Empty = thêm mới)
                using (OverlayManager.ShowScope(this))
                using (var form = new FrmFrmStocktakingMasterAddEdit(Guid.Empty))
                {
                    form.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                    form.StartPosition = FormStartPosition.CenterParent;
                    form.ShowDialog(this);
                    
                    // Reload dữ liệu sau khi form đóng (nếu có thay đổi)
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                _logger.Error("AddNewBarButtonItem_ItemClick: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi mở form thêm mới: {ex.Message}");
            }
        }

        /// <summary>
        /// Event handler cho nút Điều chỉnh
        /// </summary>
        private void EditBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                var focusedRowHandle = ProductVariantIdentifierDtoGridView.FocusedRowHandle;
                if (focusedRowHandle < 0)
                {
                    MsgBox.ShowWarning("Vui lòng chọn một phiếu kiểm kho để điều chỉnh.");
                    return;
                }

                if (ProductVariantIdentifierDtoGridView.GetRow(focusedRowHandle) is not StocktakingMasterDto selectedDto)
                {
                    MsgBox.ShowWarning("Không thể lấy thông tin phiếu kiểm kho được chọn.");
                    return;
                }

                if (selectedDto.Id == Guid.Empty)
                {
                    MsgBox.ShowWarning("Phiếu kiểm kho được chọn không có Id hợp lệ.");
                    return;
                }

                // Mở form điều chỉnh với OverlayManager (truyền selectedDto.Id)
                using (OverlayManager.ShowScope(this))
                using (var form = new FrmFrmStocktakingMasterAddEdit(selectedDto.Id))
                {
                    form.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                    form.StartPosition = FormStartPosition.CenterParent;
                    form.ShowDialog(this);
                    
                    // Reload dữ liệu sau khi form đóng (nếu có thay đổi)
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                _logger.Error("EditBarButtonItem_ItemClick: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi mở form điều chỉnh: {ex.Message}");
            }
        }

        /// <summary>
        /// Event handler cho nút Xóa
        /// </summary>
        private void DeleteBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                var selectedCount = ProductVariantIdentifierDtoGridView.SelectedRowsCount;
                if (selectedCount == 0)
                {
                    MsgBox.ShowWarning("Vui lòng chọn ít nhất một phiếu kiểm kho để xóa.");
                    return;
                }

                var selectedRowHandles = ProductVariantIdentifierDtoGridView.GetSelectedRows();
                var selectedDtos = selectedRowHandles
                    .Select(handle => ProductVariantIdentifierDtoGridView.GetRow(handle) as StocktakingMasterDto)
                    .Where(dto => dto != null && dto.Id != Guid.Empty)
                    .ToList();

                if (selectedDtos.Count == 0)
                {
                    MsgBox.ShowWarning("Không có phiếu kiểm kho hợp lệ để xóa.");
                    return;
                }

                // Hiển thị confirmation dialog
                var confirmMessage = selectedDtos.Count == 1
                    ? $"Bạn có chắc muốn xóa phiếu kiểm kho:\n<b>{GetStocktakingDisplayName(selectedDtos[0])}</b>?\n\n" +
                      "Hành động này không thể hoàn tác!"
                    : $"Bạn có chắc muốn xóa <b>{selectedDtos.Count}</b> phiếu kiểm kho?\n\n" +
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
                        _stocktakingMasterBll.Delete(dto.Id);
                        deletedCount++;
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"DeleteBarButtonItem_ItemClick: Lỗi xóa phiếu kiểm kho {dto.Id}: {ex.Message}", ex);
                    }
                }

                // Reload data sau khi xóa thành công
                LoadData();

                if (deletedCount == selectedDtos.Count)
                {
                    MsgBox.ShowSuccess($"Đã xóa thành công {deletedCount} phiếu kiểm kho.");
                }
                else
                {
                    MsgBox.ShowWarning($"Đã xóa {deletedCount}/{selectedDtos.Count} phiếu kiểm kho. Vui lòng kiểm tra lại.");
                }
            }
            catch (Exception ex)
            {
                _logger.Error("DeleteBarButtonItem_ItemClick: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi xóa phiếu kiểm kho: {ex.Message}");
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
                EditBarButtonItem_ItemClick(sender, null);
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

        #endregion

        #region ========== DATA LOADING ==========

        /// <summary>
        /// Load dữ liệu phiếu kiểm kho
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
                    _allData = _stocktakingMasterBll.GetAll() ?? new List<StocktakingMasterDto>();

                    // Bind dữ liệu vào grid
                    stocktakingMasterDtoBindingSource.DataSource = _allData;
                    stocktakingMasterDtoBindingSource.ResetBindings(false);

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

                // Các nút chỉ cho phép 1 dòng: Điều chỉnh
                EditBarButtonItem.Enabled = hasSelection;

                // Các nút cho phép nhiều dòng: Xóa
                barButtonItem4.Enabled = selectedCount > 0;
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

                // Cập nhật tổng số phiếu kiểm kho với HTML formatting
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
                            $@"<size=9><color=#757575>phiếu kiểm kho</color></size>";
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
        /// Lấy tên hiển thị của phiếu kiểm kho từ DTO
        /// </summary>
        private string GetStocktakingDisplayName(StocktakingMasterDto dto)
        {
            if (dto == null) return "N/A";

            // Ưu tiên hiển thị VoucherNumber, nếu không có thì hiển thị Id
            if (!string.IsNullOrWhiteSpace(dto.VoucherNumber))
                return $"Số phiếu: {dto.VoucherNumber}";

            if (dto.StocktakingDate != default(DateTime))
                return $"Ngày: {dto.StocktakingDate:dd/MM/yyyy}";

            return dto.Id.ToString();
        }

        #endregion
    }
}
