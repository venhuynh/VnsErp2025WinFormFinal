using Bll.MasterData.CustomerBll;
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
using DTO.MasterData.CustomerPartner;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MasterData.Customer
{
    /// <summary>
    /// User Control quản lý danh sách chi nhánh đối tác.
    /// Cung cấp giao diện hiển thị, tìm kiếm, thêm mới, sửa, xóa và xuất dữ liệu chi nhánh.
    /// </summary>
    public partial class FrmBusinessPartnerSite : XtraForm
    {
        #region ========== KHAI BÁO BIẾN ==========

        /// <summary>
        /// Business Logic Layer cho chi nhánh đối tác
        /// </summary>
        private readonly BusinessPartnerSiteBll _businessPartnerSiteBll;

        /// <summary>
        /// Danh sách dữ liệu chi nhánh hiện tại
        /// </summary>
        private List<BusinessPartnerSiteListDto> _dataList;

        /// <summary>
        /// Chi nhánh được chọn hiện tại
        /// </summary>
        private BusinessPartnerSiteListDto _selectedItem;

        /// <summary>
        /// Trạng thái đang tải dữ liệu (guard tránh gọi LoadDataAsync song song)
        /// </summary>
        private bool _isLoading;

        #endregion

        #region ========== CONSTRUCTOR & PUBLIC METHODS ==========

        /// <summary>
        /// Khởi tạo User Control quản lý chi nhánh đối tác.
        /// </summary>
        public FrmBusinessPartnerSite()
        {
            InitializeComponent();
            _businessPartnerSiteBll = new BusinessPartnerSiteBll();
            _dataList = new List<BusinessPartnerSiteListDto>();
            InitializeEvents();
            ConfigureMultiLineGridView();
            UpdateButtonStates();

            // Setup SuperToolTips
            SetupSuperToolTips();
        }

        #endregion

        #region ========== KHỞI TẠO FORM ==========

        /// <summary>
        /// Khởi tạo các sự kiện cho User Control
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
            BusinessPartnerSiteListDtoGridView.SelectionChanged += BusinessPartnerSiteListDtoGridView_SelectionChanged;
            BusinessPartnerSiteListDtoGridView.DoubleClick += BusinessPartnerSiteListDtoGridView_DoubleClick;
            BusinessPartnerSiteListDtoGridView.CustomDrawRowIndicator += BusinessPartnerSiteListDtoGridView_CustomDrawRowIndicator;
            BusinessPartnerSiteListDtoGridView.RowCellStyle += BusinessPartnerSiteListDtoGridView_RowCellStyle;

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
                var entities = await Task.Run(() => _businessPartnerSiteBll.GetAll());
                _dataList = entities.ToSiteListDtos().ToList();
                
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
        private void BindGrid(List<BusinessPartnerSiteListDto> data)
        {
            businessPartnerSiteListDtoBindingSource.DataSource = data;
            BusinessPartnerSiteListDtoGridView.BestFitColumns();
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
            
            DataSummaryBarStaticItem.Caption = @$"Tổng: {totalCount} | Hoạt động: {activeCount}";
        }

        /// <summary>
        /// Cập nhật một dòng trong datasource thay vì reload toàn bộ (cải thiện UX)
        /// </summary>
        /// <param name="updatedDto">DTO đã được cập nhật</param>
        private void UpdateSingleRowInDataSource(BusinessPartnerSiteListDto updatedDto)
        {
            try
            {
                if (updatedDto == null || businessPartnerSiteListDtoBindingSource.DataSource == null)
                {
                    return;
                }

                // Tìm dòng cần update trong datasource
                if (businessPartnerSiteListDtoBindingSource.DataSource is List<BusinessPartnerSiteListDto> dataList)
                {
                    var index = dataList.FindIndex(d => d.Id == updatedDto.Id);
                    if (index >= 0)
                    {
                        // Update dòng hiện có
                        dataList[index] = updatedDto;
                        
                        // Refresh binding source để cập nhật UI
                        businessPartnerSiteListDtoBindingSource.ResetBindings(false);
                        
                        // Refresh grid view để hiển thị thay đổi
                        var rowHandle = BusinessPartnerSiteListDtoGridView.GetRowHandle(index);
                        if (rowHandle >= 0)
                        {
                            BusinessPartnerSiteListDtoGridView.RefreshRow(rowHandle);
                        }
                    }
                    else
                    {
                        // Nếu không tìm thấy (trường hợp thêm mới), thêm vào đầu danh sách
                        dataList.Insert(0, updatedDto);
                        businessPartnerSiteListDtoBindingSource.ResetBindings(false);
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
                using (OverlayManager.ShowScope(this))
                {
                    using (var form = new FrmBusinessPartnerSiteDetail(Guid.Empty))
                    {
                        form.SiteSaved += (updatedDto) =>
                        {
                            // Cập nhật datasource với DTO mới (thêm mới)
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
                    ShowInfo("Vui lòng chọn chi nhánh cần chỉnh sửa.");
                    return;
                }

                try
                {
                    using (OverlayManager.ShowScope(this))
                    {
                        using (var form = new FrmBusinessPartnerSiteDetail(_selectedItem.Id))
                        {
                            form.SiteSaved += (updatedDto) =>
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
                    ShowInfo("Vui lòng chọn chi nhánh cần xóa.");
                    return;
                }

                var confirmMessage = $"Bạn có chắc muốn xóa chi nhánh '{_selectedItem.SiteName}'?";
                if (!MsgBox.ShowYesNo(confirmMessage)) return;

                try
                {
                    await ExecuteWithWaitingFormAsync(async () =>
                    {
                        var success = await Task.Run(() => _businessPartnerSiteBll.DeleteSite(_selectedItem.Id));
                        if (success)
                        {
                            ShowInfo("Xóa chi nhánh thành công!");
                            await LoadDataAsyncWithoutSplash();
                        }
                        else
                        {
                            ShowError("Không thể xóa chi nhánh. Vui lòng thử lại.");
                        }
                    });
                }
                catch (Exception ex)
                {
                    ShowError(ex, "Lỗi xóa chi nhánh");
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
            var rowCount = GridViewHelper.GetDisplayRowCount(BusinessPartnerSiteListDtoGridView) ?? 0;
            if (rowCount <= 0)
            {
                ShowInfo("Không có dữ liệu để xuất.");
                return;
            }

            GridViewHelper.ExportGridControl(BusinessPartnerSiteListDtoGridView, "BusinessPartnerSites.xlsx");
        }

        /// <summary>
        /// Xử lý sự kiện thay đổi selection trên GridView
        /// </summary>
        private void BusinessPartnerSiteListDtoGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (sender is GridView view && view.FocusedRowHandle >= 0)
                {
                    _selectedItem = view.GetFocusedRow() as BusinessPartnerSiteListDto;
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
        private async void BusinessPartnerSiteListDtoGridView_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (_selectedItem != null)
                {
                    using (OverlayManager.ShowScope(this))
                    {
                        using (var form = new FrmBusinessPartnerSiteDetail(_selectedItem.Id))
                        {
                            form.SiteSaved += (updatedDto) =>
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
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khi xem chi tiết");
            }
        }

        /// <summary>
        /// Xử lý sự kiện vẽ số thứ tự dòng
        /// </summary>
        private void BusinessPartnerSiteListDtoGridView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            // Sử dụng helper chung để vẽ số thứ tự dòng
            GridViewHelper.CustomDrawRowIndicator(BusinessPartnerSiteListDtoGridView, e);
        }

        /// <summary>
        /// Xử lý sự kiện tô màu dòng theo trạng thái
        /// </summary>
        private void BusinessPartnerSiteListDtoGridView_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            try
            {
                if (sender is not GridView view) return;
                if (e.RowHandle < 0) return;
                if (view.GetRow(e.RowHandle) is not BusinessPartnerSiteListDto row) return;
                // Không ghi đè màu khi đang chọn để giữ màu chọn mặc định của DevExpress
                if (view.IsRowSelected(e.RowHandle)) return;
                
                // Nếu chi nhánh không hoạt động: làm nổi bật rõ ràng hơn
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
                BusinessPartnerSiteListDtoGridView.OptionsView.RowAutoHeight = true;

                // RepositoryItemMemoEdit cho wrap text
                var memo = new RepositoryItemMemoEdit
                {
                    WordWrap = true,
                    AutoHeight = false
                };
                memo.Appearance.TextOptions.WordWrap = WordWrap.Wrap;

                // Áp dụng cho các cột có khả năng dài
                ApplyMemoEditorToColumn("SiteName", memo);
                ApplyMemoEditorToColumn("SiteFullAddress", memo);
                ApplyMemoEditorToColumn("Address", memo);
                ApplyMemoEditorToColumn("Notes", memo);

                // Tùy chọn hiển thị: căn giữa tiêu đề cho đẹp
                BusinessPartnerSiteListDtoGridView.Appearance.HeaderPanel.TextOptions.HAlignment = HorzAlignment.Center;
                BusinessPartnerSiteListDtoGridView.Appearance.HeaderPanel.Options.UseTextOptions = true;
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
            var col = BusinessPartnerSiteListDtoGridView.Columns[fieldName];
            if (col == null) return;
            // Thêm repository vào GridControl nếu chưa có
            if (!BusinessPartnerSiteListDtoGridControl.RepositoryItems.Contains(memo))
            {
                BusinessPartnerSiteListDtoGridControl.RepositoryItems.Add(memo);
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
                var rowCount = GridViewHelper.GetDisplayRowCount(BusinessPartnerSiteListDtoGridView) ?? 0;
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
            BusinessPartnerSiteListDtoGridView.ClearSelection();
            BusinessPartnerSiteListDtoGridView.FocusedRowHandle = GridControl.InvalidRowHandle;
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
                SelectedRowBarStaticItem.Caption = @$"Đang chọn: {_selectedItem.SiteName}";
            }
            else
            {
                SelectedRowBarStaticItem.Caption = @"Chưa chọn dòng nào";
            }
        }

        #endregion

        #region ========== TIỆN ÍCH HỖ TRỢ ==========

        /// <summary>
        /// Thiết lập SuperToolTip cho các controls trong UserControl
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
                        content: "Tải lại danh sách chi nhánh đối tác từ hệ thống."
                    );
                }

                if (NewBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        NewBarButtonItem,
                        title: "<b><color=Green>➕ Thêm mới</color></b>",
                        content: "Thêm mới chi nhánh đối tác vào hệ thống."
                    );
                }

                if (EditBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        EditBarButtonItem,
                        title: "<b><color=Orange>✏️ Sửa</color></b>",
                        content: "Chỉnh sửa thông tin chi nhánh đối tác đã chọn."
                    );
                }

                if (DeleteBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        DeleteBarButtonItem,
                        title: "<b><color=Red>🗑️ Xóa</color></b>",
                        content: "Xóa chi nhánh đối tác đã chọn khỏi hệ thống."
                    );
                }

                if (ExportBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        ExportBarButtonItem,
                        title: "<b><color=Purple>📊 Xuất Excel</color></b>",
                        content: "Xuất danh sách chi nhánh đối tác ra file Excel."
                    );
                }
            }
            catch (Exception ex)
            {
                // Ignore lỗi setup SuperToolTip để không chặn UserControl
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
