using Bll.Common;
using Bll.MasterData.Customer;
using Bll.Utils;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraSplashScreen;
using MasterData.Customer.Converters;
using MasterData.Customer.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MasterData.Customer
{
    public partial class UcBusinessPartnerSite : DevExpress.XtraEditors.XtraUserControl
    {
        private readonly BusinessPartnerSiteBll _businessPartnerSiteBll;
        private List<BusinessPartnerSiteListDto> _dataList;
        private BusinessPartnerSiteListDto _selectedItem;
        private bool _isLoading; // guard tránh gọi LoadDataAsync song song

        public UcBusinessPartnerSite()
        {
            InitializeComponent();
            _businessPartnerSiteBll = new BusinessPartnerSiteBll();
            _dataList = new List<BusinessPartnerSiteListDto>();
            InitializeEvents();
            ConfigureMultiLineGridView();
            UpdateButtonStates();
        }

        #region Initialize Events

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

        #region Data Loading

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

        private void LoadData()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                
                // Load data from BLL
                var entities = _businessPartnerSiteBll.GetAll();
                _dataList = entities.ToSiteListDtos().ToList();
                
                // Bind to grid
                businessPartnerSiteListDtoBindingSource.DataSource = _dataList;

                BusinessPartnerSiteListDtoAdvBandedGridView.BestFitColumns();

                // Update summary
                UpdateDataSummary();
            }
            catch (Exception ex)
            {
                MsgBox.ShowError($"Lỗi khi tải dữ liệu: {ex.Message}");
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void UpdateDataSummary()
        {
            var totalCount = _dataList?.Count ?? 0;
            var activeCount = _dataList?.Count(x => x.IsActive) ?? 0;
            
            DataSummaryBarStaticItem.Caption = $"Tổng: {totalCount} | Hoạt động: {activeCount}";
        }

        #endregion

        #region Event Handlers

        private async void ListDataBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            await LoadDataAsync();
        }

        private async void NewBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                using (OverlayManager.ShowScope(this))
                {
                    // TODO: Implement new site form
                    // using (var form = new FrmBusinessPartnerSiteDetail(Guid.Empty))
                    // {
                    //     form.StartPosition = FormStartPosition.CenterParent;
                    //     form.ShowDialog(this);
                    //     await LoadDataAsyncWithoutSplash();
                    //     UpdateButtonStates();
                    // }
                    MsgBox.ShowInfo("Chức năng thêm mới chi nhánh đang được phát triển");
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi hiển thị màn hình thêm mới");
            }
        }

        private async void EditBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
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
                    // TODO: Implement edit site form
                    // using (var form = new FrmBusinessPartnerSiteDetail(_selectedItem.Id))
                    // {
                    //     form.StartPosition = FormStartPosition.CenterParent;
                    //     form.ShowDialog(this);
                    //     await LoadDataAsyncWithoutSplash();
                    //     UpdateButtonStates();
                    // }
                    ShowInfo($"Chỉnh sửa chi nhánh: {_selectedItem.SiteName}");
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi hiển thị màn hình điều chỉnh");
            }
        }

        private async void DeleteBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (_selectedItem == null)
            {
                ShowInfo("Vui lòng chọn chi nhánh cần xóa.");
                return;
            }

            var confirmMessage = $"Bạn có chắc muốn xóa chi nhánh '{_selectedItem.SiteName}'?";
            if (!MsgBox.GetConfirmFromYesNoDialog(confirmMessage)) return;

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

        private void ExportBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
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
                    Filter = "Excel Files (*.xlsx)|*.xlsx|All Files (*.*)|*.*",
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
                    SelectedRowBarStaticItem.Caption = "Chưa chọn dòng nào";
                }
                UpdateButtonStates();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khi chọn dòng");
            }
        }

        private async void AdvBandedGridView1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (_selectedItem != null)
                {
                    // TODO: Implement view/edit details
                    // using (OverlayManager.ShowScope(this))
                    // {
                    //     using (var form = new FrmBusinessPartnerSiteDetail(_selectedItem.Id))
                    //     {
                    //         form.StartPosition = FormStartPosition.CenterParent;
                    //         form.ShowDialog(this);
                    //         await LoadDataAsyncWithoutSplash();
                    //         UpdateButtonStates();
                    //     }
                    // }
                    ShowInfo($"Xem chi tiết chi nhánh: {_selectedItem.SiteName}");
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khi xem chi tiết");
            }
        }

        #endregion

        #region Grid Configuration

        /// <summary>
        /// Cấu hình GridView để hiển thị multi-line và wrap text
        /// </summary>
        private void ConfigureMultiLineGridView()
        {
            try
            {
                // Cấu hình RepositoryItemMemoEdit cho wrap text
                var memo = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit
                {
                    WordWrap = true,
                    AutoHeight = true
                };
                memo.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;

                // Áp dụng cho các cột có khả năng dài
                ApplyMemoEditorToColumn("SiteName", memo);
                ApplyMemoEditorToColumn("SiteFullAddress", memo);
                ApplyMemoEditorToColumn("ContactPerson", memo);

                // Cấu hình hiển thị: căn giữa tiêu đề cho đẹp
                BusinessPartnerSiteListDtoAdvBandedGridView.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                BusinessPartnerSiteListDtoAdvBandedGridView.Appearance.HeaderPanel.Options.UseTextOptions = true;

                // Cấu hình màu sắc cho các dòng
                BusinessPartnerSiteListDtoAdvBandedGridView.Appearance.EvenRow.BackColor = System.Drawing.Color.FromArgb(248, 248, 248);
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
        private void ApplyMemoEditorToColumn(string fieldName, DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit memo)
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

        #region Helper Methods

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

        private void UpdateSelectedRowInfo()
        {
            if (_selectedItem != null)
            {
                SelectedRowBarStaticItem.Caption = $"Đang chọn: {_selectedItem.SiteName}";
            }
            else
            {
                SelectedRowBarStaticItem.Caption = "Chưa chọn dòng nào";
            }
        }

        /// <summary>
        /// Hiển thị thông tin.
        /// </summary>
        private void ShowInfo(string message)
        {
            MsgBox.ShowInfo(message);
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
