using Bll.Inventory.InventoryManagement;
using Common.Common;
using Common.Helpers;
using Common.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.Data;
using DTO.Inventory.InventoryManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;

namespace Inventory.Management
{
    /// <summary>
    /// Form hiển thị danh sách tồn kho theo tháng sử dụng GridView
    /// </summary>
    public partial class FrmInventoryBalanceDto : XtraForm
    {
        #region ========== KHAI BÁO BIẾN ==========

        private List<InventoryBalanceDto> _dataSource;
        private InventoryBalanceBll _inventoryBalanceBll;

        #endregion

        #region ========== CONSTRUCTOR ==========

        /// <summary>
        /// Constructor mặc định
        /// </summary>
        public FrmInventoryBalanceDto()
        {
            InitializeComponent();
            InitializeBll();
            InitializeGridView();
            InitializeFilters();
            InitializeEvents();
        }

        #endregion

        #region ========== KHỞI TẠO FORM ==========

        /// <summary>
        /// Khởi tạo BLL
        /// </summary>
        private void InitializeBll()
        {
            try
            {
                _inventoryBalanceBll = new InventoryBalanceBll();
            }
            catch (Exception ex)
            {
                MsgBox.ShowError($"Lỗi khởi tạo dịch vụ tồn kho: {ex.Message}");
                _inventoryBalanceBll = null;
            }
        }

        /// <summary>
        /// Khởi tạo cấu hình GridView
        /// </summary>
        private void InitializeGridView()
        {
            // Cấu hình group theo GroupCaption
            InventoryBalanceDtoBandedGridView.SortInfo.Clear();
            InventoryBalanceDtoBandedGridView.SortInfo.Add(
                new DevExpress.XtraGrid.Columns.GridColumnSortInfo(colGroupCaption, DevExpress.Data.ColumnSortOrder.Ascending));
            
            // Enable footer để hiển thị summary
            InventoryBalanceDtoBandedGridView.OptionsView.ShowFooter = true;
            
            // Cấu hình SummaryItem cho các cột số lượng
            colOpeningBalance.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            colOpeningBalance.SummaryItem.DisplayFormat = "{0:N2}";
            
            colTotalInQty.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            colTotalInQty.SummaryItem.DisplayFormat = "{0:N2}";
            
            colTotalOutQty.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            colTotalOutQty.SummaryItem.DisplayFormat = "{0:N2}";
            
            colClosingBalance.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            colClosingBalance.SummaryItem.DisplayFormat = "{0:N2}";
            
            // Cấu hình SummaryItem cho các cột giá trị
            colOpeningValue.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            colOpeningValue.SummaryItem.DisplayFormat = "{0:N0}";
            
            colTotalInValue.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            colTotalInValue.SummaryItem.DisplayFormat = "{0:N0}";
            
            colTotalOutValue.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            colTotalOutValue.SummaryItem.DisplayFormat = "{0:N0}";
            
            colTotalInAmountIncludedVat.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            colTotalInAmountIncludedVat.SummaryItem.DisplayFormat = "{0:N0}";
            
            colTotalInVatAmount.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            colTotalInVatAmount.SummaryItem.DisplayFormat = "{0:N0}";
            
            colTotalOutAmountIncludedVat.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            colTotalOutAmountIncludedVat.SummaryItem.DisplayFormat = "{0:N0}";
            
            colTotalOutVatAmount.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            colTotalOutVatAmount.SummaryItem.DisplayFormat = "{0:N0}";
        }

        /// <summary>
        /// Khởi tạo giá trị mặc định cho filters
        /// </summary>
        private void InitializeFilters()
        {
            try
            {
                // Năm: năm hiện tại
                NamBarEditItem.EditValue = DateTime.Now.Year;

                // Tháng: tháng hiện tại
                ThangBarEditItem.EditValue = DateTime.Now.Month;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error initializing filters: {ex.Message}");
            }
        }

        /// <summary>
        /// Khởi tạo events
        /// </summary>
        private void InitializeEvents()
        {
            // Bar button events
            XemBaoCaoBarButtonItem.ItemClick += XemBaoCaoBarButtonItem_ItemClick;
            ExportFileBarButtonItem.ItemClick += ExportFileBarButtonItem_ItemClick;
            TongKetBarButtonItem.ItemClick += TongKetBarButtonItem_ItemClick;

            // BandedGridView events
            InventoryBalanceDtoBandedGridView.SelectionChanged += InventoryBalanceDtoGridView_SelectionChanged;
            InventoryBalanceDtoBandedGridView.CustomSummaryCalculate += InventoryBalanceDtoBandedGridView_CustomSummaryCalculate;
            InventoryBalanceDtoBandedGridView.CustomDrawFooterCell += InventoryBalanceDtoBandedGridView_CustomDrawFooterCell;
            InventoryBalanceDtoBandedGridView.CustomDrawRowIndicator += InventoryBalanceDtoBandedGridView_CustomDrawRowIndicator;
            InventoryBalanceDtoBandedGridView.CustomColumnDisplayText += InventoryBalanceDtoBandedGridView_CustomColumnDisplayText;
            InventoryBalanceDtoBandedGridView.RowCellStyle += InventoryBalanceDtoBandedGridView_RowCellStyle;

            // Form events
            Load += FrmInventoryBalanceDto_Load;
        }

        #endregion

        #region ========== SỰ KIỆN FORM ==========

        /// <summary>
        /// Event handler khi form được load
        /// </summary>
        private void FrmInventoryBalanceDto_Load(object sender, EventArgs e)
        {
            // Không tự động load, user phải click "Xem" để load
        }

        /// <summary>
        /// Event handler cho nút Xem
        /// </summary>
        private async void XemBaoCaoBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khi tải dữ liệu");
            }
        }

        /// <summary>
        /// Event handler khi selection thay đổi
        /// </summary>
        private void InventoryBalanceDtoGridView_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                UpdateStatusBar();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating status bar: {ex.Message}");
            }
        }

        /// <summary>
        /// Event handler cho nút Xuất file
        /// </summary>
        private void ExportFileBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                // Kiểm tra có dữ liệu không
                var rowCount = InventoryBalanceDtoBandedGridView.RowCount;
                if (rowCount == 0)
                {
                    MsgBox.ShowWarning("Không có dữ liệu để xuất. Vui lòng tải dữ liệu trước.");
                    return;
                }

                // Lấy năm/tháng để đặt tên file
                var periodYear = NamBarEditItem.EditValue as int? ?? DateTime.Now.Year;
                var periodMonth = ThangBarEditItem.EditValue as int? ?? DateTime.Now.Month;
                var defaultFileName = $"TonKho_{periodYear}_{periodMonth:D2}";

                // Sử dụng Common helper để xuất file
                GridViewHelper.ExportGridControl(InventoryBalanceDtoBandedGridView, defaultFileName);
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khi xuất file");
            }
        }

        /// <summary>
        /// Event handler cho nút Tổng kết
        /// </summary>
        private async void TongKetBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (_inventoryBalanceBll == null)
                {
                    MsgBox.ShowWarning("Dịch vụ tồn kho chưa được khởi tạo.");
                    return;
                }

                // Lấy kỳ được chọn
                var periodYear = NamBarEditItem.EditValue as int? ?? DateTime.Now.Year;
                var periodMonth = ThangBarEditItem.EditValue as int? ?? DateTime.Now.Month;

                // Validate period
                if (periodYear < 2000 || periodYear > 9999)
                {
                    MsgBox.ShowWarning("Năm phải trong khoảng 2000-9999.");
                    return;
                }

                if (periodMonth < 1 || periodMonth > 12)
                {
                    MsgBox.ShowWarning("Tháng phải trong khoảng 1-12.");
                    return;
                }

                // Xác nhận có muốn chạy lại tổng kết
                var confirmMessage = $"Bạn có chắc chắn muốn tính lại tổng kết cho kỳ {periodYear}/{periodMonth:D2}?\n\n" +
                                    "Hệ thống sẽ tính lại tổng nhập/xuất từ các phiếu nhập xuất kho và cập nhật lại dữ liệu tồn kho.\n\n" +
                                    "Lưu ý: Chỉ có thể thực hiện khi dữ liệu chưa bị khóa.";

                if (!MsgBox.ShowYesNo(confirmMessage, "Xác nhận tổng kết"))
                {
                    return;
                }

                // Thực hiện tổng kết tại Repository
                await ExecuteWithWaitingFormAsync(async () =>
                {
                    var updatedCount = _inventoryBalanceBll.RecalculateSummary(periodYear, periodMonth);
                    
                    // Reload dữ liệu sau khi tổng kết xong (không dùng ExecuteWithWaitingFormAsync để tránh lồng nhau)
                    await ReloadDataWithoutWaitingFormAsync();
                    
                    MsgBox.ShowSuccess($"Đã tính lại tổng kết thành công cho {updatedCount} tồn kho trong kỳ {periodYear}/{periodMonth:D2}.\n\n" +
                                   "Dữ liệu đã được cập nhật và tải lại.", "Tổng kết thành công");
                });
            }
            catch (InvalidOperationException ex)
            {
                // Lỗi do dữ liệu bị khóa hoặc các lỗi business logic
                MsgBox.ShowWarning(ex.Message);
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khi tính tổng kết");
            }
        }

        #endregion

        #region ========== LOAD DỮ LIỆU ==========

        /// <summary>
        /// Load dữ liệu tồn kho từ database
        /// </summary>
        private async Task LoadDataAsync()
        {
            try
            {
                if (_inventoryBalanceBll == null)
                {
                    MsgBox.ShowWarning("Dịch vụ tồn kho chưa được khởi tạo.");
                    LoadData([]);
                    return;
                }

                await ExecuteWithWaitingFormAsync(() =>
                {
                    // Lấy filter criteria
                    var periodYear = NamBarEditItem.EditValue as int? ?? DateTime.Now.Year;
                    var periodMonth = ThangBarEditItem.EditValue as int? ?? DateTime.Now.Month;

                    // Validate period
                    if (periodYear < 2000 || periodYear > 9999)
                    {
                        MsgBox.ShowWarning("Năm phải trong khoảng 2000-9999.");
                        LoadData([]);
                        return Task.CompletedTask;
                    }

                    if (periodMonth < 1 || periodMonth > 12)
                    {
                        MsgBox.ShowWarning("Tháng phải trong khoảng 1-12.");
                        LoadData([]);
                        return Task.CompletedTask;
                    }

                    // Query tồn kho theo kỳ (UI -> BLL -> DAL)
                    // Repository đã tự động load navigation properties qua DataLoadOptions
                    var entities = _inventoryBalanceBll.QueryBalances(
                        warehouseId: null,
                        productVariantId: null,
                        periodYear: periodYear,
                        periodMonth: periodMonth);

                    // Convert entities sang DTOs sử dụng converter
                    // Converter tự động map navigation properties từ entity sang DTO
                    var dtos = entities.ToDtoList();

                    // Set data source
                    LoadData(dtos);
                    return Task.CompletedTask;
                });
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khi tải dữ liệu");
                LoadData([]);
            }
        }

        /// <summary>
        /// Load dữ liệu tồn kho
        /// </summary>
        private void LoadData(List<InventoryBalanceDto> balances)
        {
            if (balances == null)
            {
                _dataSource = null;
                inventoryBalanceDtoBindingSource.DataSource = null;
                inventoryBalanceDtoBindingSource.ResetBindings(false);
                InventoryBalanceDtoBandedGridView.RefreshData();
                UpdateStatusBar();
                return;
            }

            // Lưu reference
            _dataSource = balances;
            inventoryBalanceDtoBindingSource.DataSource = balances;
            inventoryBalanceDtoBindingSource.ResetBindings(false);

            // Force refresh GridView
            InventoryBalanceDtoBandedGridView.RefreshData();

            UpdateStatusBar();

            // Log để debug
            System.Diagnostics.Debug.WriteLine($"LoadData: Đã load {balances.Count} tồn kho vào grid");
        }


        /// <summary>
        /// Cập nhật status bar
        /// </summary>
        private void UpdateStatusBar()
        {
            try
            {
                var totalCount = _dataSource?.Count ?? 0;
                var selectedCount = InventoryBalanceDtoBandedGridView.SelectedRowsCount;

                DataSummaryBarStaticItem.Caption = $@"Tổng số: <b>{totalCount}</b> tồn kho";
                SelectedRowBarStaticItem.Caption = selectedCount > 0
                    ? $"Đã chọn: <b>{selectedCount}</b> tồn kho"
                    : "Chưa chọn dòng nào";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating status bar: {ex.Message}");
            }
        }

        /// <summary>
        /// Load dữ liệu mà không hiển thị WaitingForm (dùng khi đã có WaitingForm đang hiển thị)
        /// </summary>
        private async Task ReloadDataWithoutWaitingFormAsync()
        {
            try
            {
                if (_inventoryBalanceBll == null)
                {
                    return;
                }

                // Lấy filter criteria
                var periodYear = NamBarEditItem.EditValue as int? ?? DateTime.Now.Year;
                var periodMonth = ThangBarEditItem.EditValue as int? ?? DateTime.Now.Month;

                // Validate period
                if (periodYear < 2000 || periodYear > 9999 || periodMonth < 1 || periodMonth > 12)
                {
                    return;
                }

                // Query tồn kho theo kỳ (UI -> BLL -> DAL)
                var entities = _inventoryBalanceBll.QueryBalances(
                    warehouseId: null,
                    productVariantId: null,
                    periodYear: periodYear,
                    periodMonth: periodMonth);

                // Convert entities sang DTOs sử dụng converter
                var dtos = entities.ToDtoList();

                // Set data source
                LoadData(dtos);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error reloading data: {ex.Message}");
                LoadData([]);
            }
        }

        /// <summary>
        /// Thực hiện operation async với WaitingForm hiển thị
        /// </summary>
        private async Task ExecuteWithWaitingFormAsync(Func<Task> operation)
        {
            try
            {
                // Kiểm tra xem đã có WaitingForm đang hiển thị chưa
                if (!SplashScreenHelper.IsSplashScreenVisible())
                {
                    // Hiển thị WaitingForm
                    DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitForm1));
                }

                // Thực hiện operation
                await operation();
            }
            catch (Exception e)
            {
                MsgBox.ShowException(e);
            }
            finally
            {
                // Đóng WaitingForm nếu đang hiển thị
                if (SplashScreenHelper.IsSplashScreenVisible())
                {
                    DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
                }
            }
        }

        #endregion

        #region ========== SUMMARY EVENTS ==========

        /// <summary>
        /// Xử lý tính toán summary cho các cột
        /// </summary>
        private void InventoryBalanceDtoBandedGridView_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            try
            {
                var view = sender as DevExpress.XtraGrid.Views.BandedGrid.BandedGridView;
                if (view == null) return;

                var summaryItem = e.Item as DevExpress.XtraGrid.GridSummaryItem;
                var fieldName = summaryItem?.FieldName;
                if (string.IsNullOrEmpty(fieldName)) return;

                if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Start)
                {
                    // Khởi tạo giá trị ban đầu
                    e.TotalValue = 0m;
                }
                else if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Calculate)
                {
                    // Lấy giá trị cell hiện tại
                    var cellValue = view.GetRowCellValue(e.RowHandle, fieldName);
                    if (cellValue == null || cellValue == DBNull.Value) return;

                    var current = (decimal)e.TotalValue;
                    decimal cellDecimal = 0m;

                    // Xử lý nullable decimal
                    if (cellValue is decimal dec)
                        cellDecimal = dec;
                    else if (decimal.TryParse(cellValue.ToString(), out var parsed))
                        cellDecimal = parsed;

                    e.TotalValue = current + cellDecimal;
                }
                else if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Finalize)
                {
                    // Giữ nguyên giá trị đã tính
                }
            }
            catch (Exception)
            {
                e.TotalValue = 0m;
            }
        }

        /// <summary>
        /// Vẽ footer cell với format đẹp và màu sắc
        /// </summary>
        private void InventoryBalanceDtoBandedGridView_CustomDrawFooterCell(object sender, FooterCellCustomDrawEventArgs e)
        {
            try
            {
                var view = sender as DevExpress.XtraGrid.Views.BandedGrid.BandedGridView;
                if (view == null || e.Info?.SummaryItem == null)
                {
                    e.Handled = false;
                    return;
                }

                string fieldName = e.Info.SummaryItem.FieldName;
                if (string.IsNullOrEmpty(fieldName))
                {
                    e.Handled = false;
                    return;
                }

                // Kiểm tra các đối tượng cần thiết
                if (e.Appearance == null || e.Cache == null || e.Cache.Graphics == null)
                {
                    e.Handled = false;
                    return;
                }

                e.Handled = true; // Báo rằng ta tự vẽ toàn bộ ô footer

                // Vẽ nền
                e.Appearance.FillRectangle(e.Cache, e.Bounds);

                // Lấy giá trị tổng hợp
                var summaryValue = e.Info.Value;
                if (summaryValue == null || summaryValue == DBNull.Value)
                {
                    return;
                }

                // Font cho footer
                var appearanceFont = e.Appearance.Font ?? new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular);
                using var fontBold = new System.Drawing.Font(appearanceFont.FontFamily, appearanceFont.Size, System.Drawing.FontStyle.Bold);

                var g = e.Cache.Graphics;

                // Parse giá trị
                decimal totalValue = 0m;
                if (summaryValue is decimal dec)
                    totalValue = dec;
                else if (decimal.TryParse(summaryValue.ToString(), out var parsed))
                    totalValue = parsed;

                // Ẩn giá trị 0
                if (Math.Abs(totalValue) < 0.001m)
                {
                    return;
                }

                // Xác định màu và format dựa trên field name
                System.Drawing.Brush brush;
                string formatString;

                if (fieldName == "OpeningBalance" || fieldName == "ClosingBalance" || 
                    fieldName == "OpeningValue")
                {
                    // Màu xanh dương cho tồn đầu/cuối kỳ và giá trị đầu kỳ
                    brush = System.Drawing.Brushes.Blue;
                    formatString = fieldName.Contains("Value") ? "{0:N0}" : "{0:N2}";
                }
                else if (fieldName.Contains("In") || fieldName.Contains("Nhập"))
                {
                    // Màu xanh lá cho các cột nhập
                    brush = System.Drawing.Brushes.Green;
                    formatString = fieldName.Contains("Qty") ? "{0:N2}" : "{0:N0}";
                }
                else if (fieldName.Contains("Out") || fieldName.Contains("Xuất"))
                {
                    // Màu đỏ cho các cột xuất
                    brush = System.Drawing.Brushes.Red;
                    formatString = fieldName.Contains("Qty") ? "{0:N2}" : "{0:N0}";
                }
                else
                {
                    // Màu mặc định
                    brush = System.Drawing.Brushes.Black;
                    formatString = "{0:N2}";
                }

                // Format và hiển thị giá trị
                var displayText = string.Format(System.Globalization.CultureInfo.CurrentCulture, formatString, totalValue);
                var size = g.MeasureString(displayText, fontBold);
                var x = e.Bounds.X + (e.Bounds.Width - size.Width) / 2;
                var y = e.Bounds.Y + (e.Bounds.Height - size.Height) / 2;
                g.DrawString(displayText, fontBold, brush, x, y);
            }
            catch (Exception)
            {
                e.Handled = false; // Để DevExpress tự vẽ nếu có lỗi
            }
        }

        /// <summary>
        /// Xử lý hiển thị tùy chỉnh cho các cột trong GridView
        /// Ẩn các giá trị "0" trên các cột số lượng và giá trị để giao diện gọn gàng hơn
        /// </summary>
        private void InventoryBalanceDtoBandedGridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            try
            {
                if (e.Column == null || e.Value == null)
                    return;

                var fieldName = e.Column.FieldName;

                // Xử lý các cột số lượng (decimal)
                if (fieldName == "OpeningBalance" || fieldName == "TotalInQty" || 
                    fieldName == "TotalOutQty" || fieldName == "ClosingBalance")
                {
                    // Kiểm tra nếu giá trị là 0
                    if (e.Value is decimal decimalValue && Math.Abs(decimalValue) < 0.001m)
                    {
                        e.DisplayText = ""; // Ẩn giá trị 0
                        return;
                    }
                    if (e.Value is double doubleValue && Math.Abs(doubleValue) < 0.001)
                    {
                        e.DisplayText = ""; // Ẩn giá trị 0
                        return;
                    }
                    if (e.Value is float floatValue && Math.Abs(floatValue) < 0.001f)
                    {
                        e.DisplayText = ""; // Ẩn giá trị 0
                        return;
                    }
                }

                // Xử lý các cột giá trị (nullable decimal)
                if (fieldName == "OpeningValue" || fieldName == "TotalInValue" || 
                    fieldName == "TotalOutValue" || fieldName == "TotalInAmountIncludedVat" ||
                    fieldName == "TotalInVatAmount" || fieldName == "TotalOutAmountIncludedVat" ||
                    fieldName == "TotalOutVatAmount" || fieldName == "ClosingValue")
                {
                    // Kiểm tra nếu giá trị là 0 hoặc null
                    if (e.Value == null || e.Value == DBNull.Value)
                    {
                        e.DisplayText = ""; // Ẩn giá trị null
                        return;
                    }
                    
                    // Xử lý decimal
                    if (e.Value is decimal decimalValue)
                    {
                        if (Math.Abs(decimalValue) < 0.001m)
                        {
                            e.DisplayText = ""; // Ẩn giá trị 0
                            return;
                        }
                    }
                    // Xử lý nullable decimal
                    else if (e.Value is decimal?)
                    {
                        var nullableDec = (decimal?)e.Value;
                        if (!nullableDec.HasValue || Math.Abs(nullableDec.Value) < 0.001m)
                        {
                            e.DisplayText = ""; // Ẩn giá trị null hoặc 0
                            return;
                        }
                    }
                    // Xử lý double
                    else if (e.Value is double doubleValue)
                    {
                        if (Math.Abs(doubleValue) < 0.001)
                        {
                            e.DisplayText = ""; // Ẩn giá trị 0
                            return;
                        }
                    }
                    // Xử lý float
                    else if (e.Value is float floatValue)
                    {
                        if (Math.Abs(floatValue) < 0.001f)
                        {
                            e.DisplayText = ""; // Ẩn giá trị 0
                            return;
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Ignore errors để tránh crash UI
            }
        }

        /// <summary>
        /// Xử lý tô màu cho cells dựa trên điều kiện
        /// </summary>
        private void InventoryBalanceDtoBandedGridView_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            try
            {
                if (sender is not BandedGridView) return;

                // Kiểm tra nếu đây là dòng dữ liệu hợp lệ
                if (e.RowHandle < 0) return;

                // Lấy dữ liệu từ row
                var row = InventoryBalanceDtoBandedGridView.GetRow(e.RowHandle) as InventoryBalanceDto;
                if (row == null) return;

                // Highlight các dòng có trạng thái khóa
                if (e.Column.FieldName == "StatusHtml" || e.Column.FieldName == "StatusText")
                {
                    if (row.IsLocked)
                    {
                        // Màu cam nhạt cho dòng đã khóa
                        e.Appearance.BackColor = System.Drawing.Color.FromArgb(255, 243, 224);
                        e.Appearance.BackColor2 = System.Drawing.Color.FromArgb(255, 243, 224);
                    }
                }

                // Highlight các cột số lượng nếu có giá trị âm (bất thường)
                if (e.Column.FieldName == "ClosingBalance" || e.Column.FieldName == "TotalInQty" || 
                    e.Column.FieldName == "TotalOutQty")
                {
                    var cellValue = e.CellValue;
                    if (cellValue != null && cellValue != DBNull.Value)
                    {
                        if (cellValue is decimal dec && dec < 0)
                        {
                            // Màu đỏ nhạt cho giá trị âm
                            e.Appearance.BackColor = System.Drawing.Color.FromArgb(255, 235, 238);
                            e.Appearance.BackColor2 = System.Drawing.Color.FromArgb(255, 235, 238);
                            e.Appearance.ForeColor = System.Drawing.Color.DarkRed;
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Ignore errors
            }
        }

        /// <summary>
        /// Vẽ số thứ tự cho rows
        /// </summary>
        private void InventoryBalanceDtoBandedGridView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                GridViewHelper.CustomDrawRowIndicator(InventoryBalanceDtoBandedGridView, e);
            }
            catch (Exception)
            {
                // Ignore errors
            }
        }

        #endregion

        #region ========== TỔNG KẾT ==========

        /// <summary>
        /// Class chứa thông tin tổng kết
        /// </summary>
        private class SummaryInfo
        {
            public int PeriodYear { get; set; }
            public int PeriodMonth { get; set; }
            public int TotalItems { get; set; }
            
            // Tổng số lượng
            public decimal TotalOpeningBalance { get; set; }
            public decimal TotalInQty { get; set; }
            public decimal TotalOutQty { get; set; }
            public decimal TotalClosingBalance { get; set; }
            
            // Tổng giá trị
            public decimal? TotalOpeningValue { get; set; }
            public decimal? TotalInValue { get; set; }
            public decimal? TotalOutValue { get; set; }
            public decimal? TotalInAmountIncludedVat { get; set; }
            public decimal? TotalInVatAmount { get; set; }
            public decimal? TotalOutAmountIncludedVat { get; set; }
            public decimal? TotalOutVatAmount { get; set; }
            public decimal? TotalClosingValue { get; set; }
        }

        /// <summary>
        /// Tính tổng kết từ danh sách tồn kho
        /// </summary>
        private SummaryInfo CalculateSummary(List<InventoryBalanceDto> balances)
        {
            if (balances == null || balances.Count == 0)
                return new SummaryInfo();

            var periodYear = NamBarEditItem.EditValue as int? ?? DateTime.Now.Year;
            var periodMonth = ThangBarEditItem.EditValue as int? ?? DateTime.Now.Month;

            var summary = new SummaryInfo
            {
                PeriodYear = periodYear,
                PeriodMonth = periodMonth,
                TotalItems = balances.Count,
                
                // Tổng số lượng
                TotalOpeningBalance = balances.Sum(b => b.OpeningBalance),
                TotalInQty = balances.Sum(b => b.TotalInQty),
                TotalOutQty = balances.Sum(b => b.TotalOutQty),
                TotalClosingBalance = balances.Sum(b => b.ClosingBalance),
                
                // Tổng giá trị
                TotalOpeningValue = balances.Where(b => b.OpeningValue.HasValue).Sum(b => b.OpeningValue.Value),
                TotalInValue = balances.Where(b => b.TotalInValue.HasValue).Sum(b => b.TotalInValue.Value),
                TotalOutValue = balances.Where(b => b.TotalOutValue.HasValue).Sum(b => b.TotalOutValue.Value),
                TotalInAmountIncludedVat = balances.Where(b => b.TotalInAmountIncludedVat.HasValue).Sum(b => b.TotalInAmountIncludedVat.Value),
                TotalInVatAmount = balances.Where(b => b.TotalInVatAmount.HasValue).Sum(b => b.TotalInVatAmount.Value),
                TotalOutAmountIncludedVat = balances.Where(b => b.TotalOutAmountIncludedVat.HasValue).Sum(b => b.TotalOutAmountIncludedVat.Value),
                TotalOutVatAmount = balances.Where(b => b.TotalOutVatAmount.HasValue).Sum(b => b.TotalOutVatAmount.Value)
            };

            // Tính ClosingValue
            var openingValue = summary.TotalOpeningValue ?? 0;
            var inValue = summary.TotalInValue ?? 0;
            var outValue = summary.TotalOutValue ?? 0;
            summary.TotalClosingValue = openingValue + inValue - outValue;

            return summary;
        }

        /// <summary>
        /// Hiển thị dialog tổng kết
        /// </summary>
        private void ShowSummaryDialog(SummaryInfo summary)
        {
            var periodText = $"{summary.PeriodYear}/{summary.PeriodMonth:D2}";
            
            var message = $@"<b>TỔNG KẾT TỒN KHO KỲ {periodText}</b>

            <b>Số lượng bản ghi:</b> {summary.TotalItems:N0}

            <b>━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━</b>
            <b>TỔNG KẾT SỐ LƯỢNG:</b>
              • Tồn đầu kỳ: {summary.TotalOpeningBalance:N2}
              • Tổng nhập: {summary.TotalInQty:N2}
              • Tổng xuất: {summary.TotalOutQty:N2}
              • Tồn cuối kỳ: {summary.TotalClosingBalance:N2}

            <b>━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━</b>
            <b>TỔNG KẾT GIÁ TRỊ:</b>
              • Giá trị đầu kỳ: {(summary.TotalOpeningValue.HasValue ? summary.TotalOpeningValue.Value.ToString("N0") : "N/A")}
              • Giá trị nhập: {(summary.TotalInValue.HasValue ? summary.TotalInValue.Value.ToString("N0") : "N/A")}
              • Giá trị xuất: {(summary.TotalOutValue.HasValue ? summary.TotalOutValue.Value.ToString("N0") : "N/A")}
              • VAT nhập: {(summary.TotalInVatAmount.HasValue ? summary.TotalInVatAmount.Value.ToString("N0") : "N/A")}
              • VAT xuất: {(summary.TotalOutVatAmount.HasValue ? summary.TotalOutVatAmount.Value.ToString("N0") : "N/A")}
              • Tổng tiền nhập (có VAT): {(summary.TotalInAmountIncludedVat.HasValue ? summary.TotalInAmountIncludedVat.Value.ToString("N0") : "N/A")}
              • Tổng tiền xuất (có VAT): {(summary.TotalOutAmountIncludedVat.HasValue ? summary.TotalOutAmountIncludedVat.Value.ToString("N0") : "N/A")}
              • Giá trị cuối kỳ: {(summary.TotalClosingValue.HasValue ? summary.TotalClosingValue.Value.ToString("N0") : "N/A")}";

            MsgBox.ShowSuccess(message, "Tổng kết tồn kho");
        }

        #endregion

        #region ========== TIỆN ÍCH ==========

        /// <summary>
        /// Hiển thị lỗi với thông tin ngữ cảnh
        /// </summary>
        private void ShowError(Exception ex, string context = null)
        {
            MsgBox.ShowException(
                string.IsNullOrWhiteSpace(context) ? ex : new Exception($"{context}: {ex.Message}", ex));
        }

        #endregion
    }
}
