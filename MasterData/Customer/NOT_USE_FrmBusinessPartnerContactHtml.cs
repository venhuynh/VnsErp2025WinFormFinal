using Bll.MasterData.CustomerBll;
using Common.Common;
using Common.Helpers;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Tile;
using DevExpress.XtraSplashScreen;
using DTO.MasterData.CustomerPartner;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common.Utils;

namespace MasterData.Customer
{
    /// <summary>
    /// Form hiển thị danh sách liên hệ đối tác dưới dạng TileView với HTML formatting
    /// Tham khảo: DevExpress HTML Demos - TileViewModule
    /// </summary>
    public partial class NOT_USE_FrmBusinessPartnerContactHtml : XtraForm
    {
        #region ========== KHAI BÁO BIẾN ==========

        /// <summary>
        /// Business Logic Layer cho liên hệ đối tác
        /// </summary>
        private readonly BusinessPartnerContactBll _contactBll = new BusinessPartnerContactBll();

        /// <summary>
        /// Danh sách ID của các liên hệ được chọn
        /// </summary>
        private readonly List<Guid> _selectedContactIds = new List<Guid>();

        /// <summary>
        /// Trạng thái đang tải dữ liệu (guard tránh gọi LoadDataAsync song song)
        /// </summary>
        private bool _isLoading;

        #endregion

        #region ========== CONSTRUCTOR ==========

        /// <summary>
        /// Khởi tạo form
        /// </summary>
        public NOT_USE_FrmBusinessPartnerContactHtml()
        {
            InitializeComponent();
            RegisterEvents();
            // Template đã được setup trong Designer từ resources, không cần ConfigureTileView nữa
            // Chỉ cần cấu hình các options khác
            ConfigureTileViewOptions();
            SetupHtmlTemplateEvents();
            UpdateButtonStates();
            SetupSuperToolTips();
            LoadDataAsync();
        }

        #endregion

        #region ========== CẤU HÌNH TILEVIEW ==========

        /// <summary>
        /// Cấu hình các options của TileView (template đã được setup trong Designer từ resources)
        /// </summary>
        private void ConfigureTileViewOptions()
        {
            try
            {
                // Cấu hình TileView options (template đã được load từ resources trong Designer)
                tileView1.OptionsTiles.LayoutMode = DevExpress.XtraGrid.Views.Tile.TileViewLayoutMode.List;
                tileView1.OptionsTiles.Orientation = Orientation.Vertical;
                tileView1.OptionsTiles.ItemSize = new System.Drawing.Size(380, 180);
                tileView1.OptionsTiles.ItemPadding = new Padding(10);
                tileView1.OptionsTiles.Padding = new Padding(10);
                tileView1.OptionsTiles.AllowPressAnimation = false;
                tileView1.OptionsBehavior.AllowSmoothScrolling = true;
                tileView1.OptionsTiles.ScrollMode = DevExpress.XtraEditors.TileControlScrollMode.TouchScrollBar;
                
                // Enable multi-selection (TileView chỉ hỗ trợ MultiSelect, không có MultiSelectMode)
                tileView1.OptionsSelection.MultiSelect = true;
                
                // Setup grouping theo PartnerName
                tileView1.ColumnSet.GroupColumn = colPartnerName;
                tileView1.OptionsTiles.GroupTextPadding = new Padding(15, 10, 0, 10);
                tileView1.OptionsTiles.IndentBetweenGroups = 0;
                
                // Sort theo PartnerName và FullName
                tileView1.SortInfo.Clear();
                tileView1.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
                    new DevExpress.XtraGrid.Columns.GridColumnSortInfo(colPartnerName, DevExpress.Data.ColumnSortOrder.Ascending),
                    new DevExpress.XtraGrid.Columns.GridColumnSortInfo(colFullName, DevExpress.Data.ColumnSortOrder.Ascending)
                });
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(new Exception("Lỗi cấu hình TileView: " + ex.Message, ex));
            }
        }

        /// <summary>
        /// Đăng ký các event handlers cho HTML template (onclick methods)
        /// DevExpress tự động tìm method có tên tương ứng với onclick handler
        /// </summary>
        private void SetupHtmlTemplateEvents()
        {
            // Không cần đăng ký gì, DevExpress tự động tìm method OnCheckboxClick từ onclick="OnCheckboxClick"
            // Method OnCheckboxClick đã được định nghĩa và sẽ được gọi tự động
        }

        #endregion

        #region ========== QUẢN LÝ DỮ LIỆU ==========

        /// <summary>
        /// Tải dữ liệu và bind vào TileView (Async, hiển thị WaitForm)
        /// </summary>
        private async Task LoadDataAsync()
        {
            if (_isLoading) return;

            try
            {
                _isLoading = true;
                await ExecuteWithWaitingFormAsync(async () => { await LoadDataAsyncWithoutSplash(); });
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(new Exception("Lỗi tải dữ liệu: " + ex.Message, ex));
            }
            finally
            {
                _isLoading = false;
            }
        }

        /// <summary>
        /// Tải dữ liệu không có splash screen
        /// </summary>
        private Task LoadDataAsyncWithoutSplash()
        {
            try
            {
                // Lấy dữ liệu từ BLL
                var contacts = _contactBll.GetAll();
                var contactDtos = contacts.Select(e => e.ToDto()).ToList();

                // Bind dữ liệu vào GridControl
                businessPartnerContactDtoBindingSource.DataSource = contactDtos;

                // Cập nhật trạng thái button và status bar
                UpdateButtonStates();
                UpdateStatusBar();

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(new Exception("Lỗi tải dữ liệu: " + ex.Message, ex));
                return Task.CompletedTask;
            }
        }

        /// <summary>
        /// Execute action với WaitingForm
        /// </summary>
        private async Task ExecuteWithWaitingFormAsync(Func<Task> action)
        {
            var splashShown = false;
            try
            {
                // Kiểm tra xem đã có splash screen chưa bằng cách thử đóng trước
                try
                {
                    SplashScreenManager.CloseForm();
                }
                catch
                {
                    // Nếu không có splash screen thì sẽ có exception, bỏ qua
                }

                // Hiển thị WaitingForm1
                SplashScreenManager.ShowForm(typeof(WaitForm1));
                splashShown = true;

                // Thực hiện operation
                await action();
            }
            finally
            {
                // Đóng splash screen
                if (splashShown)
                {
                    try
                    {
                        SplashScreenManager.CloseForm();
                    }
                    catch
                    {
                        // Bỏ qua nếu có lỗi khi đóng
                    }
                }
            }
        }

        #endregion

        #region ========== SỰ KIỆN FORM ==========

        /// <summary>
        /// Đăng ký các sự kiện UI
        /// </summary>
        private void RegisterEvents()
        {
            // Bar button events
            ListDataBarButtonItem.ItemClick += ListDataBarButtonItem_ItemClick;
            NewBarButtonItem.ItemClick += NewBarButtonItem_ItemClick;
            EditBarButtonItem.ItemClick += EditBarButtonItem_ItemClick;
            DeleteBarButtonItem.ItemClick += DeleteBarButtonItem_ItemClick;

            // TileView events
            tileView1.SelectionChanged += TileView1_SelectionChanged;
            tileView1.HtmlElementMouseClick += TileView1_HtmlElementMouseClick;
        }

        /// <summary>
        /// Xử lý sự kiện click button Tải dữ liệu
        /// </summary>
        private async void ListDataBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi tải dữ liệu");
            }
        }

        /// <summary>
        /// Xử lý sự kiện click button Thêm mới
        /// </summary>
        private void NewBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                using (OverlayManager.ShowScope(this))
                {
                    using (var form = new FrmBusinessPartnerContactDetail())
                    {
                        form.StartPosition = FormStartPosition.CenterParent;
                        form.ShowDialog(this);

                        // Reload data sau khi đóng form
                        if (form.DialogResult == DialogResult.OK)
                        {
                            _ = LoadDataAsync();
                        }

                        UpdateButtonStates();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi hiển thị màn hình thêm mới");
            }
        }

        /// <summary>
        /// Xử lý sự kiện click button Sửa
        /// </summary>
        private void EditBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Chỉ cho phép chỉnh sửa 1 dòng dữ liệu
            if (_selectedContactIds == null || _selectedContactIds.Count == 0)
            {
                ShowInfo("Vui lòng chọn một dòng để chỉnh sửa.");
                return;
            }
            if (_selectedContactIds.Count > 1)
            {
                ShowInfo("Chỉ cho phép chỉnh sửa 1 dòng. Vui lòng bỏ chọn bớt.");
                return;
            }

            var id = _selectedContactIds[0];
            var dto = tileView1.GetFocusedRow() as BusinessPartnerContactDto;
            if (dto == null || dto.Id != id)
            {
                // Tìm đúng DTO theo Id trong datasource nếu FocusedRow không khớp selection
                if (businessPartnerContactDtoBindingSource.DataSource is IEnumerable list)
                {
                    foreach (var item in list)
                    {
                        if (item is BusinessPartnerContactDto x && x.Id == id)
                        {
                            dto = x;
                            break;
                        }
                    }
                }
            }

            if (dto == null)
            {
                ShowInfo("Không thể xác định dòng được chọn để chỉnh sửa.");
                return;
            }

            try
            {
                using (OverlayManager.ShowScope(this))
                {
                    // FrmBusinessPartnerContactDetail hiện chỉ hỗ trợ thêm mới
                    // TODO: Cần cải thiện FrmBusinessPartnerContactDetail để hỗ trợ edit mode với ContactId
                    using (var form = new FrmBusinessPartnerContactDetail())
                    {
                        form.StartPosition = FormStartPosition.CenterParent;
                        form.ShowDialog(this);

                        // Reload data sau khi đóng form
                        if (form.DialogResult == DialogResult.OK)
                        {
                            _ = LoadDataAsync();
                        }

                        UpdateButtonStates();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi hiển thị màn hình điều chỉnh");
            }
        }

        /// <summary>
        /// Xử lý sự kiện click button Xóa
        /// </summary>
        private async void DeleteBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (_selectedContactIds == null || _selectedContactIds.Count == 0)
            {
                ShowInfo("Vui lòng chọn ít nhất một dòng để xóa.");
                return;
            }

            var confirmMessage = _selectedContactIds.Count == 1
                ? "Bạn có chắc muốn xóa dòng dữ liệu đã chọn?"
                : $"Bạn có chắc muốn xóa {_selectedContactIds.Count} dòng dữ liệu đã chọn?";

            if (!MsgBox.ShowYesNo(confirmMessage)) return;

            try
            {
                await ExecuteWithWaitingFormAsync(async () =>
                {
                    foreach (var id in _selectedContactIds.ToList())
                    {
                        _contactBll.Delete(id);
                    }
                    ClearSelectionState();
                    // Gọi LoadDataAsyncWithoutSplash để tránh xung đột WaitingForm1
                    await LoadDataAsyncWithoutSplash();
                });
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi xóa dữ liệu");
            }
        }
         
        /// <summary>
        /// Xử lý sự kiện click vào checkbox trong HTML template
        /// Method này được gọi từ onclick="OnCheckboxClick" trong template
        /// </summary>
        private void OnCheckboxClick(object sender, DevExpress.XtraGrid.Views.Tile.TileViewHtmlElementMouseEventArgs e)
        {
            try
            {
                var rowHandle = e.RowHandle;
                if (tileView1.GetRow(rowHandle) is BusinessPartnerContactDto dto)
                {
                    // Toggle selection
                    if (tileView1.IsRowSelected(rowHandle))
                    {
                        tileView1.UnselectRow(rowHandle);
                    }
                    else
                    {
                        tileView1.SelectRow(rowHandle);
                    }
                    
                    // Cập nhật selection state
                    UpdateSelectedContactIds();
                    UpdateButtonStates();
                    UpdateStatusBar();
                }
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        /// <summary>
        /// Xử lý sự kiện click vào HTML elements trong TileView (fallback)
        /// </summary>
        private void TileView1_HtmlElementMouseClick(object sender, DevExpress.XtraGrid.Views.Tile.TileViewHtmlElementMouseEventArgs e)
        {
            try
            {
                // Kiểm tra xem có click vào checkbox không (fallback nếu onclick không hoạt động)
                if (e.ElementId == "checkbox" || e.ParentHasId("checkbox"))
                {
                    OnCheckboxClick(sender, e);
                }
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        /// <summary>
        /// Cập nhật danh sách selected contact IDs từ TileView selection và update Selected property
        /// </summary>
        private void UpdateSelectedContactIds()
        {
            try
            {
                _selectedContactIds.Clear();
                var selectedRows = tileView1.GetSelectedRows();
                var selectedRowHandles = new HashSet<int>(selectedRows);
                
                // Update Selected property cho tất cả rows
                for (int i = 0; i < tileView1.RowCount; i++)
                {
                    if (tileView1.GetRow(i) is BusinessPartnerContactDto dto)
                    {
                        dto.Selected = selectedRowHandles.Contains(i);
                        if (dto.Selected)
                        {
                            _selectedContactIds.Add(dto.Id);
                        }
                    }
                }
                
                // Refresh để cập nhật checkbox state
                tileView1.RefreshData();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        /// <summary>
        /// Xử lý sự kiện thay đổi selection trên TileView
        /// </summary>
        private void TileView1_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            try
            {
                UpdateSelectedContactIds();
                UpdateButtonStates();
                UpdateStatusBar();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        #endregion

        #region ========== TIỆN ÍCH ==========

        /// <summary>
        /// Cập nhật trạng thái các nút toolbar dựa trên selection
        /// </summary>
        private void UpdateButtonStates()
        {
            try
            {
                var selectedCount = _selectedContactIds?.Count ?? 0;
                var rowCount = tileView1.RowCount;

                // Edit: chỉ khi chọn đúng 1 dòng
                if (EditBarButtonItem != null)
                    EditBarButtonItem.Enabled = selectedCount == 1;

                // Delete: khi chọn >= 1 dòng
                if (DeleteBarButtonItem != null)
                    DeleteBarButtonItem.Enabled = selectedCount >= 1;

                // Export: chỉ khi có dữ liệu hiển thị
                if (ExportBarButtonItem != null)
                    ExportBarButtonItem.Enabled = rowCount > 0;
            }
            catch
            {
                // ignore
            }
        }

        /// <summary>
        /// Cập nhật status bar
        /// </summary>
        private void UpdateStatusBar()
        {
            try
            {
                var rowCount = tileView1.RowCount;
                var selectedCount = _selectedContactIds?.Count ?? 0;

                if (DataSummaryBarStaticItem != null)
                    DataSummaryBarStaticItem.Caption = $"Tổng số: {rowCount} liên hệ";

                if (CurrentSelectBarStaticItem != null)
                    CurrentSelectBarStaticItem.Caption = $"Đã chọn: {selectedCount} liên hệ";
            }
            catch
            {
                // ignore
            }
        }

        /// <summary>
        /// Xóa trạng thái chọn hiện tại trên TileView
        /// </summary>
        private void ClearSelectionState()
        {
            _selectedContactIds.Clear();
            tileView1.ClearSelection();
            tileView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.InvalidRowHandle;
            UpdateButtonStates();
            UpdateStatusBar();
        }

        /// <summary>
        /// Thiết lập SuperToolTip cho các controls trong form
        /// </summary>
        private void SetupSuperToolTips()
        {
            try
            {
                if (ListDataBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        ListDataBarButtonItem,
                        title: "<b><color=Blue>🔄 Tải dữ liệu</color></b>",
                        content: "Tải lại danh sách liên hệ đối tác từ hệ thống."
                    );
                }

                if (NewBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        NewBarButtonItem,
                        title: "<b><color=Green>➕ Thêm mới</color></b>",
                        content: "Thêm mới liên hệ đối tác vào hệ thống."
                    );
                }

                if (EditBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        EditBarButtonItem,
                        title: "<b><color=Orange>✏️ Sửa</color></b>",
                        content: "Chỉnh sửa thông tin liên hệ đối tác đã chọn."
                    );
                }

                if (DeleteBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        DeleteBarButtonItem,
                        title: "<b><color=Red>🗑️ Xóa</color></b>",
                        content: "Xóa các liên hệ đối tác đã chọn khỏi hệ thống."
                    );
                }

                if (ExportBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        ExportBarButtonItem,
                        title: "<b><color=Purple>📊 Xuất Excel</color></b>",
                        content: "Xuất danh sách liên hệ đối tác ra file Excel."
                    );
                }
            }
            catch (Exception ex)
            {
                // Ignore lỗi setup SuperToolTip để không chặn form
                System.Diagnostics.Debug.WriteLine($"Lỗi setup SuperToolTip: {ex.Message}");
            }
        }

        /// <summary>
        /// Hiển thị thông tin
        /// </summary>
        private void ShowInfo(string message)
        {
            MsgBox.ShowSuccess(message);
        }

        /// <summary>
        /// Hiển thị lỗi với thông tin ngữ cảnh
        /// </summary>
        private void ShowError(Exception ex, string context = null)
        {
            if (string.IsNullOrWhiteSpace(context))
                MsgBox.ShowException(ex);
            else
                MsgBox.ShowException(new Exception(context + ": " + ex.Message, ex));
        }

        #endregion
    }
}