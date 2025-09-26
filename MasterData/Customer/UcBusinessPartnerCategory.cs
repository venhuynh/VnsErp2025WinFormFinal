using Bll.MasterData.Customer;
using Bll.Utils;
using Bll.Common;
using DevExpress.Utils;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSplashScreen;
using MasterData.Converters;
using MasterData.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasterData.Customer
{
    /// <summary>
    /// Danh sách Danh mục Đối tác - chỉ giữ event handler, mọi xử lý tách thành phương thức riêng.
    /// </summary>
    public partial class UcBusinessPartnerCategory : DevExpress.XtraEditors.XtraUserControl
    {
        #region Fields

        private readonly BusinessPartnerCategoryBll _businessPartnerCategoryBll = new BusinessPartnerCategoryBll();
        private List<Guid> _selectedCategoryIds = new List<Guid>();
        private bool _isLoading; // guard tránh gọi LoadDataAsync song song

        #endregion

        #region Constructor

        /// <summary>
        /// Khởi tạo control, đăng ký event UI.
        /// </summary>
        public UcBusinessPartnerCategory()
        {
            InitializeComponent();

            // Toolbar events
            ListDataBarButtonItem.ItemClick += ListDataBarButtonItem_ItemClick;
            NewBarButtonItem.ItemClick += NewBarButtonItem_ItemClick;
            EditBarButtonItem.ItemClick += EditBarButtonItem_ItemClick;
            DeleteBarButtonItem.ItemClick += DeleteBarButtonItem_ItemClick;
            ExportBarButtonItem.ItemClick += ExportBarButtonItem_ItemClick;

            // Grid events
            BusinessPartnerCategoryGridView.SelectionChanged += BusinessPartnerCategoryGridView_SelectionChanged;
            BusinessPartnerCategoryGridView.CustomDrawRowIndicator += BusinessPartnerCategoryGridView_CustomDrawRowIndicator;
            BusinessPartnerCategoryGridView.RowCellStyle += BusinessPartnerCategoryGridView_RowCellStyle;

            UpdateButtonStates();
        }

        #endregion

        #region Private Helper Methods

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

        #endregion

        #region Event Handlers

        /// <summary>
        /// Người dùng bấm "Danh sách" để tải dữ liệu.
        /// </summary>
        private async void ListDataBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            await LoadDataAsync();
        }

        /// <summary>
        /// Người dùng bấm "Mới".
        /// </summary>
        private async void NewBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                using (OverlayManager.ShowScope(this))
                {
                    using (var form = new FrmBusinessPartnerCategoryDetail(Guid.Empty))
                    {
                        form.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
                        form.ShowDialog(this);

                        await LoadDataAsync();
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
        /// Grid selection thay đổi -> cập nhật danh sách Id đã chọn và trạng thái nút.
        /// </summary>
        private void BusinessPartnerCategoryGridView_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            try
            {
                _selectedCategoryIds = GridViewHelper.GetSelectedRowColumnValues<Guid>(sender, nameof(BusinessPartnerCategoryDto.Id));
                UpdateButtonStates();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        /// <summary>
        /// Vẽ STT dòng.
        /// </summary>
        private void BusinessPartnerCategoryGridView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            // Sử dụng helper chung để vẽ số thứ tự dòng
            GridViewHelper.CustomDrawRowIndicator(BusinessPartnerCategoryGridView, e);
        }

        private void UpdateButtonStates()
        {
            try
            {
                var selectedCount = _selectedCategoryIds?.Count ?? 0;
                // Edit: chỉ khi chọn đúng 1 dòng
                if (EditBarButtonItem != null)
                    EditBarButtonItem.Enabled = selectedCount == 1;
                // Delete: khi chọn >= 1 dòng
                if (DeleteBarButtonItem != null)
                    DeleteBarButtonItem.Enabled = selectedCount >= 1;
                // Export: chỉ khi có dữ liệu hiển thị
                var rowCount = GridViewHelper.GetDisplayRowCount(BusinessPartnerCategoryGridView) ?? 0;
                if (ExportBarButtonItem != null)
                    ExportBarButtonItem.Enabled = rowCount > 0;
            }
            catch
            {
                // ignore
            }
        }

        /// <summary>
        /// Cấu hình GridView để hiển thị dữ liệu xuống dòng (word wrap) cho các cột văn bản dài.
        /// </summary>
        private void ConfigureMultiLineGridView()
        {
            try
            {
                // Bật tự động điều chỉnh chiều cao dòng để wrap nội dung
                BusinessPartnerCategoryGridView.OptionsView.RowAutoHeight = true;

                // RepositoryItemMemoEdit cho wrap text
                var memo = new RepositoryItemMemoEdit
                {
                    WordWrap = true,
                    AutoHeight = false
                };
                memo.Appearance.TextOptions.WordWrap = WordWrap.Wrap;

                // Áp dụng cho các cột có khả năng dài
                ApplyMemoEditorToColumn("CategoryName", memo);
                ApplyMemoEditorToColumn("Description", memo);

                // Tùy chọn hiển thị: căn giữa tiêu đề cho đẹp
                BusinessPartnerCategoryGridView.Appearance.HeaderPanel.TextOptions.HAlignment = HorzAlignment.Center;
                BusinessPartnerCategoryGridView.Appearance.HeaderPanel.Options.UseTextOptions = true;
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void ApplyMemoEditorToColumn(string fieldName, RepositoryItemMemoEdit memo)
        {
            var col = BusinessPartnerCategoryGridView.Columns[fieldName];
            if (col == null) return;
            // Thêm repository vào GridControl nếu chưa có
            if (!BusinessPartnerCategoryGridControl.RepositoryItems.Contains(memo))
            {
                BusinessPartnerCategoryGridControl.RepositoryItems.Add(memo);
            }
            col.ColumnEdit = memo;
        }

        /// <summary>
        /// Tô màu/định dạng dòng theo số lượng đối tác.
        /// </summary>
        private void BusinessPartnerCategoryGridView_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            try
            {
                var view = sender as GridView;
                if (view == null) return;
                if (e.RowHandle < 0) return;
                var row = view.GetRow(e.RowHandle) as BusinessPartnerCategoryDto;
                if (row == null) return;
                // Không ghi đè màu khi đang chọn để giữ màu chọn mặc định của DevExpress
                if (view.IsRowSelected(e.RowHandle)) return;

                // Nền theo số lượng đối tác
                System.Drawing.Color backColor;
                if (row.PartnerCount == 0)
                {
                    backColor = System.Drawing.Color.LightGray; // Không có đối tác
                }
                else if (row.PartnerCount <= 5)
                {
                    backColor = System.Drawing.Color.LightYellow; // Ít đối tác
                }
                else if (row.PartnerCount <= 20)
                {
                    backColor = System.Drawing.Color.LightGreen; // Trung bình
                }
                else
                {
                    backColor = System.Drawing.Color.LightBlue; // Nhiều đối tác
                }

                e.Appearance.BackColor = backColor;
                e.Appearance.ForeColor = System.Drawing.Color.Black;
                e.Appearance.Options.UseBackColor = true;
                e.Appearance.Options.UseForeColor = true;
            }
            catch (Exception)
            {
                // ignore style errors
            }
        }

        /// <summary>
        /// Người dùng bấm "Điều chỉnh".
        /// </summary>
        private async void EditBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // Chỉ cho phép chỉnh sửa 1 dòng dữ liệu
            if (_selectedCategoryIds == null || _selectedCategoryIds.Count == 0)
            {
                ShowInfo("Vui lòng chọn một dòng để chỉnh sửa.");
                return;
            }
            if (_selectedCategoryIds.Count > 1)
            {
                ShowInfo("Chỉ cho phép chỉnh sửa 1 dòng. Vui lòng bỏ chọn bớt.");
                return;
            }

            var id = _selectedCategoryIds[0];
            var dto = BusinessPartnerCategoryGridView.GetFocusedRow() as BusinessPartnerCategoryDto;
            if (dto == null || dto.Id != id)
            {
                // Tìm đúng DTO theo Id trong datasource nếu FocusedRow không khớp selection
                if (businessPartnerCategoryDtoBindingSource.DataSource is System.Collections.IEnumerable list)
                {
                    foreach (var item in list)
                    {
                        if (item is BusinessPartnerCategoryDto x && x.Id == id)
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
                    using (var form = new FrmBusinessPartnerCategoryDetail(dto.Id))
                    {
                        form.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
                        form.ShowDialog(this);
                        
                        await LoadDataAsync();
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
        /// Người dùng bấm "Xóa".
        /// </summary>
        private async void DeleteBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (_selectedCategoryIds == null || _selectedCategoryIds.Count == 0)
            {
                ShowInfo("Vui lòng chọn ít nhất một dòng để xóa.");
                return;
            }

            // Kiểm tra xem có danh mục nào đang được sử dụng không
            var categoriesInUse = new List<string>();
            foreach (var id in _selectedCategoryIds)
            {
                if (_businessPartnerCategoryBll.HasPartners(id))
                {
                    var category = _businessPartnerCategoryBll.GetById(id);
                    if (category != null)
                    {
                        categoriesInUse.Add(category.CategoryName);
                    }
                }
            }

            if (categoriesInUse.Count > 0)
            {
                var message = $"Không thể xóa các danh mục sau vì đang được sử dụng:\n{string.Join("\n", categoriesInUse)}";
                ShowInfo(message);
                return;
            }

            var confirmMessage = _selectedCategoryIds.Count == 1
                ? "Bạn có chắc muốn xóa dòng dữ liệu đã chọn?"
                : $"Bạn có chắc muốn xóa {_selectedCategoryIds.Count} dòng dữ liệu đã chọn?";

            if (!MsgBox.GetConfirmFromYesNoDialog(confirmMessage)) return;

            try
            {
                await ExecuteWithWaitingFormAsync(async () =>
                {
                    foreach (var id in _selectedCategoryIds.ToList())
                    {
                        _businessPartnerCategoryBll.Delete(id);
                    }
                    ClearSelectionState();
                    await LoadDataAsync();
                });
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi xóa dữ liệu");
            }
        }

        /// <summary>
        /// Người dùng bấm "Xuất".
        /// </summary>
        private void ExportBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // Chỉ cho phép xuất khi có dữ liệu hiển thị
            var rowCount = GridViewHelper.GetDisplayRowCount(BusinessPartnerCategoryGridView) ?? 0;
            if (rowCount <= 0)
            {
                ShowInfo("Không có dữ liệu để xuất.");
                return;
            }

            GridViewHelper.ExportGridControl(BusinessPartnerCategoryGridView, "BusinessPartnerCategories.xlsx");
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Tải dữ liệu và bind vào Grid (Async, hiển thị WaitForm).
        /// </summary>
        private async System.Threading.Tasks.Task LoadDataAsync()
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
        private async System.Threading.Tasks.Task LoadDataAsyncWithoutSplash()
        {
            try
            {
                var (categories, counts) = await _businessPartnerCategoryBll.GetCategoriesWithCountsAsync();
                var dtoList = categories.ToDtosWithMappingCount(counts).ToList();
                BindGrid(dtoList);
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi tải dữ liệu");
            }
        }

        /// <summary>
        /// Bind danh sách DTO vào Grid và cấu hình hiển thị.
        /// </summary>
        private void BindGrid(List<BusinessPartnerCategoryDto> data)
        {
            businessPartnerCategoryDtoBindingSource.DataSource = data;
            BusinessPartnerCategoryGridView.BestFitColumns();
            ConfigureMultiLineGridView();
            UpdateButtonStates();
        }

        /// <summary>
        /// Xóa trạng thái chọn hiện tại trên Grid.
        /// </summary>
        private void ClearSelectionState()
        {
            _selectedCategoryIds.Clear();
            BusinessPartnerCategoryGridView.ClearSelection();
            BusinessPartnerCategoryGridView.FocusedRowHandle = DevExpress.XtraGrid.GridControl.InvalidRowHandle;
            UpdateButtonStates();
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

        #endregion

    }
}
