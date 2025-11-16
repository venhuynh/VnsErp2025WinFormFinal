using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bll.Common;
using Bll.MasterData.Customer;
using Bll.Utils;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraSplashScreen;
using MasterData.Customer.Converters;
using MasterData.Customer.Dto;

namespace MasterData.Customer
{
    /// <summary>
    /// User Control quản lý danh sách chi nhánh đối tác.
    /// Cung cấp giao diện hiển thị, tìm kiếm, thêm mới, sửa, xóa và xuất dữ liệu chi nhánh.
    /// </summary>
    public partial class UcBusinessPartnerSite : XtraUserControl
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
        public UcBusinessPartnerSite()
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
            BusinessPartnerSiteListDtoAdvBandedGridView.SelectionChanged += AdvBandedGridView1_SelectionChanged;
            BusinessPartnerSiteListDtoAdvBandedGridView.DoubleClick += AdvBandedGridView1_DoubleClick;
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
            // Clear selection trước khi bind data mới
            ClearSelectionState();
            
            businessPartnerSiteListDtoBindingSource.DataSource = data;
            BusinessPartnerSiteListDtoAdvBandedGridView.BestFitColumns();
            ConfigureMultiLineGridView();
            
            // Update summary
            UpdateDataSummary();
            
            // Đảm bảo selection được clear sau khi bind
            ClearSelectionState();
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
                        form.StartPosition = FormStartPosition.CenterParent;
                        if (form.ShowDialog(this) == DialogResult.OK)
                        {
                            await LoadDataAsyncWithoutSplash();
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
                            form.StartPosition = FormStartPosition.CenterParent;
                            if (form.ShowDialog(this) == DialogResult.OK)
                            {
                                await LoadDataAsyncWithoutSplash();
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
            var rowCount = BusinessPartnerSiteListDtoAdvBandedGridView.RowCount;
            if (rowCount <= 0)
            {
                ShowInfo("Không có dữ liệu để xuất.");
                return;
            }

            // Export GridView data
            try
            {
                var saveDialog = new SaveFileDialog
                {
                    Filter = @"Excel Files (*.xlsx)|*.xlsx|All Files (*.*)|*.*",
                    FileName = "BusinessPartnerSites.xlsx"
                };

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    BusinessPartnerSiteListDtoAdvBandedGridView.ExportToXlsx(saveDialog.FileName);
                    ShowInfo("Xuất dữ liệu thành công!");
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi xuất dữ liệu");
            }
        }

        /// <summary>
        /// Xử lý sự kiện thay đổi selection trên GridView
        /// </summary>
        private void AdvBandedGridView1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                var view = sender as AdvBandedGridView;
                if (view?.FocusedRowHandle >= 0)
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
        private async void AdvBandedGridView1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (_selectedItem != null)
                {
                    using (OverlayManager.ShowScope(this))
                    {
                        using (var form = new FrmBusinessPartnerSiteDetail(_selectedItem.Id))
                        {
                            form.StartPosition = FormStartPosition.CenterParent;
                            if (form.ShowDialog(this) == DialogResult.OK)
                            {
                                await LoadDataAsyncWithoutSplash();
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

        #endregion

        #region ========== XỬ LÝ DỮ LIỆU ==========

        /// <summary>
        /// Cấu hình GridView để hiển thị multi-line và wrap text
        /// </summary>
        private void ConfigureMultiLineGridView()
        {
            try
            {
                // Cấu hình RepositoryItemMemoEdit cho wrap text
                var memo = new RepositoryItemMemoEdit
                {
                    WordWrap = true,
                    AutoHeight = true
                };
                memo.Appearance.TextOptions.WordWrap = WordWrap.Wrap;

                // Áp dụng cho các cột có khả năng dài
                ApplyMemoEditorToColumn("SiteName", memo);
                ApplyMemoEditorToColumn("SiteFullAddress", memo);
                ApplyMemoEditorToColumn("ContactPerson", memo);

                // Cấu hình hiển thị: căn giữa tiêu đề cho đẹp
                BusinessPartnerSiteListDtoAdvBandedGridView.Appearance.HeaderPanel.TextOptions.HAlignment = HorzAlignment.Center;
                BusinessPartnerSiteListDtoAdvBandedGridView.Appearance.HeaderPanel.Options.UseTextOptions = true;

                // Cấu hình màu sắc cho các dòng
                BusinessPartnerSiteListDtoAdvBandedGridView.Appearance.EvenRow.BackColor = Color.FromArgb(248, 248, 248);
                BusinessPartnerSiteListDtoAdvBandedGridView.OptionsView.EnableAppearanceEvenRow = true;

                // Cấu hình auto height cho các dòng
                BusinessPartnerSiteListDtoAdvBandedGridView.OptionsView.RowAutoHeight = true;
            }
            catch (Exception ex)
            {
                MsgBox.ShowError($"Lỗi cấu hình grid: {ex.Message}");
            }
        }

        /// <summary>
        /// Áp dụng RepositoryItemMemoEdit cho cột cụ thể
        /// </summary>
        /// <param name="fieldName">Tên field của cột</param>
        /// <param name="memo">RepositoryItemMemoEdit</param>
        private void ApplyMemoEditorToColumn(string fieldName, RepositoryItemMemoEdit memo)
        {
            var col = BusinessPartnerSiteListDtoAdvBandedGridView.Columns[fieldName];
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
                var rowCount = BusinessPartnerSiteListDtoAdvBandedGridView.RowCount;
                if (ExportBarButtonItem != null)
                    ExportBarButtonItem.Enabled = rowCount > 0;
            }
            catch
            {
                // ignore
            }
        }

        /// <summary>
        /// Xóa trạng thái chọn hiện tại trên GridView.
        /// </summary>
        private void ClearSelectionState()
        {
            _selectedItem = null;
            BusinessPartnerSiteListDtoAdvBandedGridView.ClearSelection();
            BusinessPartnerSiteListDtoAdvBandedGridView.FocusedRowHandle = -1;
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
