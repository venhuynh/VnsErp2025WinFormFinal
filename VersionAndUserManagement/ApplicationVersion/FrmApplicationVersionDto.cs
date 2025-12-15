using Bll.Common;
using Common.Helpers;
using Common.Utils;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSplashScreen;
using DTO.VersionAndUserManagementDto;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common.Common;
using DevExpress.Data;
using DevExpress.XtraGrid;

namespace VersionAndUserManagement.ApplicationVersion
{
    /// <summary>
    /// Form quản lý danh sách phiên bản ứng dụng.
    /// Cung cấp giao diện hiển thị, tìm kiếm, cập nhật phiên bản từ Assembly và xuất dữ liệu.
    /// </summary>
    public partial class FrmApplicationVersionDto : DevExpress.XtraEditors.XtraForm
    {
        #region ========== KHAI BÁO BIẾN ==========

        /// <summary>
        /// Business Logic Layer cho phiên bản ứng dụng
        /// </summary>
        private readonly ApplicationVersionBll _applicationVersionBll;

        /// <summary>
        /// Danh sách dữ liệu phiên bản hiện tại
        /// </summary>
        private List<ApplicationVersionDto> _dataList;

        /// <summary>
        /// Phiên bản được chọn hiện tại
        /// </summary>
        private ApplicationVersionDto _selectedItem;

        /// <summary>
        /// Trạng thái đang tải dữ liệu (guard tránh gọi LoadDataAsync song song)
        /// </summary>
        private bool _isLoading;

        #endregion

        #region ========== CONSTRUCTOR & PUBLIC METHODS ==========

        /// <summary>
        /// Khởi tạo Form quản lý phiên bản ứng dụng.
        /// </summary>
        public FrmApplicationVersionDto()
        {
            InitializeComponent();
            _applicationVersionBll = new ApplicationVersionBll();
            _dataList = new List<ApplicationVersionDto>();
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
            GetNewVersionButtonItem.ItemClick += GetNewVersionButtonItem_ItemClick;
            ExportBarButtonItem.ItemClick += ExportBarButtonItem_ItemClick;

            // Grid events
            ApplicationVersionDtoGridView.SelectionChanged += ApplicationVersionDtoGridView_SelectionChanged;
            ApplicationVersionDtoGridView.DoubleClick += ApplicationVersionDtoGridView_DoubleClick;
            ApplicationVersionDtoGridView.CustomDrawRowIndicator += ApplicationVersionDtoGridView_CustomDrawRowIndicator;
            ApplicationVersionDtoGridView.RowCellStyle += ApplicationVersionDtoGridView_RowCellStyle;

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
                var dtos = await Task.Run(() => _applicationVersionBll.GetAllVersions());
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
        private void BindGrid(List<ApplicationVersionDto> data)
        {
            applicationVersionDtoBindingSource.DataSource = data;
            ApplicationVersionDtoGridView.BestFitColumns();
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
        private void UpdateSingleRowInDataSource(ApplicationVersionDto updatedDto)
        {
            try
            {
                if (updatedDto == null || applicationVersionDtoBindingSource.DataSource == null)
                {
                    return;
                }

                // Tìm dòng cần update trong datasource
                if (applicationVersionDtoBindingSource.DataSource is List<ApplicationVersionDto> dataList)
                {
                    var index = dataList.FindIndex(d => d.Id == updatedDto.Id);
                    if (index >= 0)
                    {
                        // Update dòng hiện có
                        dataList[index] = updatedDto;

                        // Refresh binding source để cập nhật UI
                        applicationVersionDtoBindingSource.ResetBindings(false);

                        // Refresh grid view để hiển thị thay đổi
                        var rowHandle = ApplicationVersionDtoGridView.GetRowHandle(index);
                        if (rowHandle >= 0)
                        {
                            ApplicationVersionDtoGridView.RefreshRow(rowHandle);
                        }
                    }
                    else
                    {
                        // Nếu không tìm thấy (trường hợp thêm mới), thêm vào đầu danh sách
                        dataList.Insert(0, updatedDto);
                        applicationVersionDtoBindingSource.ResetBindings(false);
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
        /// Xử lý sự kiện click button Tìm phiên bản mới
        /// </summary>
        private async void GetNewVersionButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                var currentVersion = _applicationVersionBll.GetCurrentApplicationVersion();
                var confirmMessage = $"Bạn có muốn cập nhật phiên bản hiện tại '{currentVersion}' vào database không?";
                
                if (!MsgBox.ShowYesNo(confirmMessage)) return;

                try
                {
                    await ExecuteWithWaitingFormAsync(async () =>
                    {
                        await Task.Run(() => _applicationVersionBll.UpdateVersionFromAssembly());
                        ShowInfo($"Đã cập nhật phiên bản '{currentVersion}' vào database thành công!");
                        await LoadDataAsyncWithoutSplash();
                    });
                }
                catch (Exception ex)
                {
                    ShowError(ex, "Lỗi cập nhật phiên bản");
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
            var rowCount = GridViewHelper.GetDisplayRowCount(ApplicationVersionDtoGridView) ?? 0;
            if (rowCount <= 0)
            {
                ShowInfo("Không có dữ liệu để xuất.");
                return;
            }

            GridViewHelper.ExportGridControl(ApplicationVersionDtoGridView, "ApplicationVersions.xlsx");
        }

        /// <summary>
        /// Xử lý sự kiện thay đổi selection trên GridView
        /// </summary>
        private void ApplicationVersionDtoGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (sender is GridView view && view.FocusedRowHandle >= 0)
                {
                    _selectedItem = view.GetFocusedRow() as ApplicationVersionDto;
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
        private void ApplicationVersionDtoGridView_DoubleClick(object sender, EventArgs e)
        {
            // Có thể mở form chi tiết nếu cần trong tương lai
            // Hiện tại chỉ hiển thị thông tin trong grid
        }

        /// <summary>
        /// Xử lý sự kiện vẽ số thứ tự dòng
        /// </summary>
        private void ApplicationVersionDtoGridView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            // Sử dụng helper chung để vẽ số thứ tự dòng
            GridViewHelper.CustomDrawRowIndicator(ApplicationVersionDtoGridView, e);
        }

        /// <summary>
        /// Xử lý sự kiện tô màu dòng theo trạng thái
        /// </summary>
        private void ApplicationVersionDtoGridView_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            try
            {
                if (sender is not GridView view) return;
                if (e.RowHandle < 0) return;
                if (view.GetRow(e.RowHandle) is not ApplicationVersionDto row) return;
                // Không ghi đè màu khi đang chọn để giữ màu chọn mặc định của DevExpress
                if (view.IsRowSelected(e.RowHandle)) return;

                // Nếu phiên bản không hoạt động: làm nổi bật rõ ràng hơn
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
                ApplicationVersionDtoGridView.OptionsView.RowAutoHeight = true;

                // RepositoryItemMemoEdit cho wrap text
                var memo = new RepositoryItemMemoEdit
                {
                    WordWrap = true,
                    AutoHeight = false
                };
                memo.Appearance.TextOptions.WordWrap = WordWrap.Wrap;

                // Áp dụng cho các cột có khả năng dài
                ApplyMemoEditorToColumn("Version", memo);
                ApplyMemoEditorToColumn("Description", memo);

                // Tùy chọn hiển thị: căn giữa tiêu đề cho đẹp
                ApplicationVersionDtoGridView.Appearance.HeaderPanel.TextOptions.HAlignment = HorzAlignment.Center;
                ApplicationVersionDtoGridView.Appearance.HeaderPanel.Options.UseTextOptions = true;
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
            var col = ApplicationVersionDtoGridView.Columns[fieldName];
            if (col == null) return;
            // Thêm repository vào GridControl nếu chưa có
            if (!ApplicationVersionDtoGridControl.RepositoryItems.Contains(memo))
            {
                ApplicationVersionDtoGridControl.RepositoryItems.Add(memo);
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
                // Export: chỉ khi có dữ liệu hiển thị
                var rowCount = GridViewHelper.GetDisplayRowCount(ApplicationVersionDtoGridView) ?? 0;
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
            ApplicationVersionDtoGridView.ClearSelection();
            ApplicationVersionDtoGridView.FocusedRowHandle = GridControl.InvalidRowHandle;
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
                SelectedRowBarStaticItem.Caption = @$"Đang chọn: {_selectedItem.Version}";
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
                        content: "Tải lại danh sách phiên bản ứng dụng từ hệ thống."
                    );
                }

                if (GetNewVersionButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        GetNewVersionButtonItem,
                        title: "<b><color=Green>🔍 Tìm phiên bản mới</color></b>",
                        content: "Cập nhật phiên bản hiện tại của ứng dụng vào database."
                    );
                }

                if (ExportBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        ExportBarButtonItem,
                        title: "<b><color=Purple>📊 Xuất Excel</color></b>",
                        content: "Xuất danh sách phiên bản ra file Excel."
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
