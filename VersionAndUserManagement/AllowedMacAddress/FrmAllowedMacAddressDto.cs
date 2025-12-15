using Bll.Common;
using Common.Common;
using Common.Helpers;
using Common.Utils;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSplashScreen;
using DTO.VersionAndUserManagementDto;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VersionAndUserManagement.AllowedMacAddress
{
    /// <summary>
    /// Form quản lý danh sách MAC address được phép sử dụng ứng dụng.
    /// Cung cấp giao diện hiển thị, tìm kiếm, thêm mới, sửa, xóa và xuất dữ liệu MAC address.
    /// </summary>
    public partial class FrmAllowedMacAddressDto : DevExpress.XtraEditors.XtraForm
    {
        #region ========== KHAI BÁO BIẾN ==========

        /// <summary>
        /// Business Logic Layer cho MAC address được phép
        /// </summary>
        private readonly AllowedMacAddressBll _allowedMacAddressBll;

        /// <summary>
        /// Danh sách dữ liệu MAC address hiện tại
        /// </summary>
        private List<AllowedMacAddressDto> _dataList;

        /// <summary>
        /// MAC address được chọn hiện tại
        /// </summary>
        private AllowedMacAddressDto _selectedItem;

        /// <summary>
        /// Trạng thái đang tải dữ liệu (guard tránh gọi LoadDataAsync song song)
        /// </summary>
        private bool _isLoading;

        #endregion

        #region ========== CONSTRUCTOR & PUBLIC METHODS ==========

        /// <summary>
        /// Khởi tạo Form quản lý MAC address được phép.
        /// </summary>
        public FrmAllowedMacAddressDto()
        {
            InitializeComponent();
            _allowedMacAddressBll = new AllowedMacAddressBll();
            _dataList = new List<AllowedMacAddressDto>();
            InitializeEvents();
            ConfigureMultiLineGridView();
            UpdateButtonStates();

            // Setup SuperToolTips
            SetupSuperToolTips();

            // Tự động tải dữ liệu khi form load
            this.Load += async (s, e) => await LoadDataAsync();
        }

        #endregion

        #region ========== KHỞI TẠO FORM ==========

        /// <summary>
        /// Khởi tạo các sự kiện cho Form
        /// </summary>
        private void InitializeEvents()
        {
            // Bar button events
            ListDataBarButtonItem.ItemClick += ListDataBarButtonItem_ItemClick;
            NewBarButtonItem.ItemClick += NewBarButtonItem_ItemClick;
            EditBarButtonItem.ItemClick += EditBarButtonItem_ItemClick;
            DeleteBarButtonItem.ItemClick += DeleteBarButtonItem_ItemClick;
            ExportBarButtonItem.ItemClick += ExportBarButtonItem_ItemClick;

            // Grid events
            AllowedMacAddressDtoGridView.SelectionChanged += AllowedMacAddressDtoGridView_SelectionChanged;
            AllowedMacAddressDtoGridView.DoubleClick += AllowedMacAddressDtoGridView_DoubleClick;
            AllowedMacAddressDtoGridView.CustomDrawRowIndicator += AllowedMacAddressDtoGridView_CustomDrawRowIndicator;
            AllowedMacAddressDtoGridView.RowCellStyle += AllowedMacAddressDtoGridView_RowCellStyle;

            // Cấu hình HtmlHypertextLabel để enable HTML rendering
            if (HtmlHypertextLabel != null)
            {
                HtmlHypertextLabel.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            }
        }

        #endregion

        #region ========== QUẢN LÝ DỮ LIỆU ==========

        /// <summary>
        /// Tải dữ liệu và bind vào Grid (Async, hiển thị WaitForm).
        /// </summary>
        private async Task LoadDataAsync()
        {
            if (_isLoading) return; // tránh re-entrancy
            _isLoading = true;
            try
            {
                await ExecuteWithWaitingFormAsync(async () =>
                {
                    await LoadDataAsyncWithoutSplash();
                });
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi tải dữ liệu");
            }
            finally
            {
                _isLoading = false;
            }
        }

        /// <summary>
        /// Tải dữ liệu và bind vào Grid (Async, không hiển thị WaitForm).
        /// </summary>
        private async Task LoadDataAsyncWithoutSplash()
        {
            try
            {
                var dtos = await Task.Run(() => _allowedMacAddressBll.GetAll());
                _dataList = dtos;

                BindGrid(_dataList);
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi tải dữ liệu");
            }
        }

        /// <summary>
        /// Bind danh sách DTO vào Grid và cấu hình hiển thị.
        /// </summary>
        private void BindGrid(List<AllowedMacAddressDto> data)
        {
            allowedMacAddressDtoBindingSource.DataSource = data;
            AllowedMacAddressDtoGridView.BestFitColumns();
            ConfigureMultiLineGridView();
            UpdateDataSummary();
            UpdateButtonStates();
        }

        /// <summary>
        /// Cập nhật thông tin tổng hợp dữ liệu
        /// </summary>
        private void UpdateDataSummary()
        {
            var totalCount = _dataList?.Count ?? 0;
            var activeCount = _dataList?.Count(x => x.IsActive) ?? 0;

            DataSummaryBarStaticItem.Caption = $@"Tổng: {totalCount} | Hoạt động: {activeCount}";
        }

        /// <summary>
        /// Cập nhật một dòng trong datasource thay vì reload toàn bộ (cải thiện UX)
        /// </summary>
        /// <param name="updatedDto">DTO đã được cập nhật</param>
        private void UpdateSingleRowInDataSource(AllowedMacAddressDto updatedDto)
        {
            try
            {
                if (updatedDto == null || allowedMacAddressDtoBindingSource.DataSource == null)
                {
                    return;
                }

                // Tìm dòng cần update trong datasource
                if (allowedMacAddressDtoBindingSource.DataSource is List<AllowedMacAddressDto> dataList)
                {
                    var index = dataList.FindIndex(d => d.Id == updatedDto.Id);
                    if (index >= 0)
                    {
                        // Update dòng hiện có
                        dataList[index] = updatedDto;

                        // Refresh binding source để cập nhật UI
                        allowedMacAddressDtoBindingSource.ResetBindings(false);

                        // Refresh grid view để hiển thị thay đổi
                        var rowHandle = AllowedMacAddressDtoGridView.GetRowHandle(index);
                        if (rowHandle >= 0)
                        {
                            AllowedMacAddressDtoGridView.RefreshRow(rowHandle);
                        }
                    }
                    else
                    {
                        // Nếu không tìm thấy (trường hợp thêm mới), thêm vào đầu danh sách
                        dataList.Insert(0, updatedDto);
                        allowedMacAddressDtoBindingSource.ResetBindings(false);
                    }

                    // Cập nhật summary
                    _dataList = dataList;
                    UpdateDataSummary();
                }
            }
            catch (Exception ex)
            {
                // Nếu có lỗi khi update, fallback về reload toàn bộ
                System.Diagnostics.Debug.WriteLine($"Lỗi update single row: {ex.Message}");
                _ = LoadDataAsync();
            }
        }

        #endregion

        #region ========== SỰ KIỆN FORM ==========

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
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// Xử lý sự kiện click button Thêm mới
        /// </summary>
        private async void NewBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                using var form = new FrmAllowedMacAddressDtoAddEdit(Guid.Empty);
                form.MacAddressSaved += UpdateSingleRowInDataSource;
                form.StartPosition = FormStartPosition.CenterParent;
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    UpdateButtonStates();
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
        private async void EditBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (_selectedItem == null)
                {
                    ShowInfo("Vui lòng chọn MAC address cần chỉnh sửa.");
                    return;
                }

                try
                {
                    using (var form = new FrmAllowedMacAddressDtoAddEdit(_selectedItem.Id))
                    {
                        form.MacAddressSaved += (updatedDto) =>
                        {
                            // Cập nhật datasource với DTO đã được cập nhật
                            UpdateSingleRowInDataSource(updatedDto);
                        };
                        form.StartPosition = FormStartPosition.CenterParent;
                        if (form.ShowDialog(this) == DialogResult.OK)
                        {
                            UpdateButtonStates();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ShowError(ex, "Lỗi hiển thị màn hình điều chỉnh");
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// Xử lý sự kiện click button Xóa
        /// </summary>
        private async void DeleteBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (_selectedItem == null)
                {
                    ShowInfo("Vui lòng chọn MAC address cần xóa.");
                    return;
                }

                var confirmMessage = $"Bạn có chắc muốn xóa MAC address '{_selectedItem.MacAddress}'?";
                if (!MsgBox.ShowYesNo(confirmMessage)) return;

                try
                {
                    await ExecuteWithWaitingFormAsync(async () =>
                    {
                        await Task.Run(() => _allowedMacAddressBll.Delete(_selectedItem.Id));
                        ShowInfo("Xóa MAC address thành công!");
                        await LoadDataAsyncWithoutSplash();
                    });
                }
                catch (Exception ex)
                {
                    ShowError(ex, "Lỗi xóa MAC address");
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// Xử lý sự kiện click button Xuất dữ liệu
        /// </summary>
        private void ExportBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Chỉ cho phép xuất khi có dữ liệu hiển thị
            var rowCount = GridViewHelper.GetDisplayRowCount(AllowedMacAddressDtoGridView) ?? 0;
            if (rowCount <= 0)
            {
                ShowInfo("Không có dữ liệu để xuất.");
                return;
            }

            GridViewHelper.ExportGridControl(AllowedMacAddressDtoGridView, "AllowedMacAddresses.xlsx");
        }

        /// <summary>
        /// Xử lý sự kiện thay đổi selection trên GridView
        /// </summary>
        private void AllowedMacAddressDtoGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (sender is GridView view && view.FocusedRowHandle >= 0)
                {
                    _selectedItem = view.GetFocusedRow() as AllowedMacAddressDto;
                    UpdateSelectedRowInfo();
                }
                else
                {
                    _selectedItem = null;
                    SelectedRowBarStaticItem.Caption = @"Chưa chọn dòng nào";
                }
                UpdateButtonStates();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khi chọn dòng");
            }
        }

        /// <summary>
        /// Xử lý sự kiện double click trên GridView
        /// </summary>
        private async void AllowedMacAddressDtoGridView_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (_selectedItem != null)
                {
                    using (var form = new FrmAllowedMacAddressDtoAddEdit(_selectedItem.Id))
                    {
                        form.MacAddressSaved += (updatedDto) =>
                        {
                            // Cập nhật datasource với DTO đã được cập nhật
                            UpdateSingleRowInDataSource(updatedDto);
                        };
                        form.StartPosition = FormStartPosition.CenterParent;
                        if (form.ShowDialog(this) == DialogResult.OK)
                        {
                            UpdateButtonStates();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khi xem chi tiết");
            }
        }

        /// <summary>
        /// Xử lý sự kiện vẽ số thứ tự dòng
        /// </summary>
        private void AllowedMacAddressDtoGridView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            // Sử dụng helper chung để vẽ số thứ tự dòng
            GridViewHelper.CustomDrawRowIndicator(AllowedMacAddressDtoGridView, e);
        }

        /// <summary>
        /// Xử lý sự kiện tô màu dòng theo trạng thái
        /// </summary>
        private void AllowedMacAddressDtoGridView_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            try
            {
                if (sender is not GridView view) return;
                if (e.RowHandle < 0) return;
                if (view.GetRow(e.RowHandle) is not AllowedMacAddressDto row) return;
                // Không ghi đè màu khi đang chọn để giữ màu chọn mặc định của DevExpress
                if (view.IsRowSelected(e.RowHandle)) return;

                // Nếu MAC address không hoạt động: làm nổi bật rõ ràng hơn
                if (row.IsActive) return;
                e.Appearance.BackColor = Color.FromArgb(255, 205, 210); // đỏ nhạt nhưng đậm hơn (Light Red)
                e.Appearance.ForeColor = Color.DarkRed;
                e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Strikeout);
            }
            catch (Exception)
            {
                // ignore style errors
            }
        }

        #endregion

        #region ========== XỬ LÝ DỮ LIỆU ==========

        /// <summary>
        /// Cấu hình GridView để hiển thị dữ liệu xuống dòng (word wrap) cho các cột văn bản dài.
        /// Đồng thời bật tự động tính chiều cao dòng để hiển thị đầy đủ nội dung.
        /// </summary>
        private void ConfigureMultiLineGridView()
        {
            try
            {
                // Bật tự động điều chỉnh chiều cao dòng để wrap nội dung
                AllowedMacAddressDtoGridView.OptionsView.RowAutoHeight = true;

                // RepositoryItemMemoEdit cho wrap text
                var memo = new RepositoryItemMemoEdit
                {
                    WordWrap = true,
                    AutoHeight = false
                };
                memo.Appearance.TextOptions.WordWrap = WordWrap.Wrap;

                // Áp dụng cho các cột có khả năng dài
                ApplyMemoEditorToColumn("Description", memo);
                ApplyMemoEditorToColumn("ComputerName", memo);

                // Tùy chọn hiển thị: căn giữa tiêu đề cho đẹp
                AllowedMacAddressDtoGridView.Appearance.HeaderPanel.TextOptions.HAlignment = HorzAlignment.Center;
                AllowedMacAddressDtoGridView.Appearance.HeaderPanel.Options.UseTextOptions = true;
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// Áp dụng RepositoryItemMemoEdit cho cột cụ thể
        /// </summary>
        /// <param name="fieldName">Tên field của cột</param>
        /// <param name="memo">RepositoryItemMemoEdit</param>
        private void ApplyMemoEditorToColumn(string fieldName, RepositoryItemMemoEdit memo)
        {
            var col = AllowedMacAddressDtoGridView.Columns[fieldName];
            if (col == null) return;
            // Thêm repository vào GridControl nếu chưa có
            if (!AllowedMacAddressDtoGridControl.RepositoryItems.Contains(memo))
            {
                AllowedMacAddressDtoGridControl.RepositoryItems.Add(memo);
            }
            col.ColumnEdit = memo;
        }

        #endregion

        #region ========== TIỆN ÍCH ==========

        /// <summary>
        /// Thực hiện operation async với WaitingForm1 hiển thị.
        /// </summary>
        /// <param name="operation">Operation async cần thực hiện</param>
        private async Task ExecuteWithWaitingFormAsync(Func<Task> operation)
        {
            try
            {
                // Hiển thị WaitingForm1
                SplashScreenManager.ShowForm(typeof(WaitForm1));

                // Thực hiện operation
                await operation();
            }
            finally
            {
                // Đóng WaitingForm1
                SplashScreenManager.CloseForm();
            }
        }

        /// <summary>
        /// Cập nhật trạng thái các nút toolbar dựa trên selection
        /// </summary>
        private void UpdateButtonStates()
        {
            try
            {
                var hasSelection = _selectedItem != null;

                // Edit: chỉ khi chọn đúng 1 dòng
                if (EditBarButtonItem != null)
                    EditBarButtonItem.Enabled = hasSelection;

                // Delete: khi chọn >= 1 dòng
                if (DeleteBarButtonItem != null)
                    DeleteBarButtonItem.Enabled = hasSelection;

                // Export: chỉ khi có dữ liệu hiển thị
                var rowCount = GridViewHelper.GetDisplayRowCount(AllowedMacAddressDtoGridView) ?? 0;
                if (ExportBarButtonItem != null)
                    ExportBarButtonItem.Enabled = rowCount > 0;
            }
            catch
            {
                // ignore
            }
        }

        /// <summary>
        /// Xóa trạng thái chọn hiện tại trên Grid.
        /// </summary>
        private void ClearSelectionState()
        {
            _selectedItem = null;
            AllowedMacAddressDtoGridView.ClearSelection();
            AllowedMacAddressDtoGridView.FocusedRowHandle = GridControl.InvalidRowHandle;
            UpdateSelectedRowInfo();
            UpdateButtonStates();
        }

        /// <summary>
        /// Cập nhật thông tin dòng được chọn
        /// </summary>
        private void UpdateSelectedRowInfo()
        {
            if (_selectedItem != null)
            {
                SelectedRowBarStaticItem.Caption = @$"Đang chọn: {_selectedItem.MacAddress}";
            }
            else
            {
                SelectedRowBarStaticItem.Caption = @"Chưa chọn dòng nào";
            }
        }

        #endregion

        #region ========== TIỆN ÍCH HỖ TRỢ ==========

        /// <summary>
        /// Thiết lập SuperToolTip cho các controls trong Form
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
                        content: "Tải lại danh sách MAC address được phép từ hệ thống."
                    );
                }

                if (NewBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        NewBarButtonItem,
                        title: "<b><color=Green>➕ Thêm mới</color></b>",
                        content: "Thêm mới MAC address vào danh sách được phép."
                    );
                }

                if (EditBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        EditBarButtonItem,
                        title: "<b><color=Orange>✏️ Sửa</color></b>",
                        content: "Chỉnh sửa thông tin MAC address đã chọn."
                    );
                }

                if (DeleteBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        DeleteBarButtonItem,
                        title: "<b><color=Red>🗑️ Xóa</color></b>",
                        content: "Xóa MAC address đã chọn khỏi danh sách được phép."
                    );
                }

                if (ExportBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        ExportBarButtonItem,
                        title: "<b><color=Purple>📊 Xuất Excel</color></b>",
                        content: "Xuất danh sách MAC address ra file Excel."
                    );
                }
            }
            catch (Exception ex)
            {
                // Ignore lỗi setup SuperToolTip để không chặn Form
                System.Diagnostics.Debug.WriteLine($"Lỗi setup SuperToolTip: {ex.Message}");
            }
        }

        /// <summary>
        /// Hiển thị thông tin.
        /// </summary>
        private void ShowInfo(string message)
        {
            MsgBox.ShowSuccess(message);
        }

        /// <summary>
        /// Hiển thị lỗi với thông tin ngữ cảnh.
        /// </summary>
        private void ShowError(Exception ex, string context = null)
        {
            if (string.IsNullOrWhiteSpace(context))
                MsgBox.ShowException(ex);
            else
                MsgBox.ShowException(new Exception(context + ": " + ex.Message, ex));
        }

        /// <summary>
        /// Hiển thị lỗi với thông báo.
        /// </summary>
        private void ShowError(string message)
        {
            MsgBox.ShowError(message);
        }

        #endregion
    }
}
