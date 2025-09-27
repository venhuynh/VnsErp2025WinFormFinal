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
using MasterData.Converters.Customer;
using MasterData.Dto.Customer;

namespace MasterData.Customer
{
    /// <summary>
    /// Danh sách Đối tác - chỉ giữ event handler, mọi xử lý tách thành phương thức riêng.
    /// </summary>
    public partial class UcBusinessPartnerList : DevExpress.XtraEditors.XtraUserControl
    {
        #region Fields

        private readonly BusinessPartnerBll _businessPartnerBll = new BusinessPartnerBll();
        private List<Guid> _selectedPartnerIds = new List<Guid>();
        private bool _isLoading; // guard tránh gọi LoadDataAsync song song (Splash đã hiển thị)

        #endregion

        #region Constructor

        /// <summary>
        /// Khởi tạo control, đăng ký event UI.
        /// </summary>
        public UcBusinessPartnerList()
        {
            InitializeComponent();

            // Toolbar events
            NewBarButtonItem.ItemClick += NewBarButtonItem_ItemClick;
            EditBarButtonItem.ItemClick += EditBarButtonItem_ItemClick;
            DeleteBarButtonItem.ItemClick += DeleteBarButtonItem_ItemClick;
            ExportBarButtonItem.ItemClick += ExportBarButtonItem_ItemClick;

            // Grid events
            BusinessPartnerListGridView.SelectionChanged += BusinessPartnerListGridView_SelectionChanged;
            BusinessPartnerListGridView.CustomDrawRowIndicator += BusinessPartnerListGridView_CustomDrawRowIndicator;
            BusinessPartnerListGridView.RowCellStyle += BusinessPartnerListGridView_RowCellStyle;

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
            // Tham khảo mẫu ConfigSqlServerInfoBarButtonItem_ItemClick: dùng ShowScope để auto-close overlay
            try
            {
                using (OverlayManager.ShowScope(this))
                {
                    using (var form = new FrmBusinessPartnerDetail(Guid.Empty))
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
        private void BusinessPartnerListGridView_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            try
            {
                _selectedPartnerIds = GridViewHelper.GetSelectedRowColumnValues<Guid>(sender, nameof(BusinessPartnerListDto.Id));
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
        private void BusinessPartnerListGridView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            // Sử dụng helper chung để vẽ số thứ tự dòng
            GridViewHelper.CustomDrawRowIndicator(BusinessPartnerListGridView, e);
        }

        private void UpdateButtonStates()
        {
            try
            {
                var selectedCount = _selectedPartnerIds?.Count ?? 0;
                // Edit: chỉ khi chọn đúng 1 dòng
                if (EditBarButtonItem != null)
                    EditBarButtonItem.Enabled = selectedCount == 1;
                // Delete: khi chọn >= 1 dòng
                if (DeleteBarButtonItem != null)
                    DeleteBarButtonItem.Enabled = selectedCount >= 1;
                // Export: chỉ khi có dữ liệu hiển thị
                var rowCount = GridViewHelper.GetDisplayRowCount(BusinessPartnerListGridView) ?? 0;
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
        /// Đồng thời bật tự động tính chiều cao dòng để hiển thị đầy đủ nội dung.
        /// </summary>
        private void ConfigureMultiLineGridView()
        {
            try
            {
                // Bật tự động điều chỉnh chiều cao dòng để wrap nội dung
                BusinessPartnerListGridView.OptionsView.RowAutoHeight = true;

                // RepositoryItemMemoEdit cho wrap text
                var memo = new RepositoryItemMemoEdit
                {
                    WordWrap = true,
                    AutoHeight = false
                };
                memo.Appearance.TextOptions.WordWrap = WordWrap.Wrap;

                // Áp dụng cho các cột có khả năng dài
                ApplyMemoEditorToColumn("PartnerName", memo);
                ApplyMemoEditorToColumn("Address", memo); // nếu cột tồn tại trong view list
                ApplyMemoEditorToColumn("Email", memo);
                ApplyMemoEditorToColumn("City", memo);

                // Tùy chọn hiển thị: căn giữa tiêu đề cho đẹp
                BusinessPartnerListGridView.Appearance.HeaderPanel.TextOptions.HAlignment = HorzAlignment.Center;
                BusinessPartnerListGridView.Appearance.HeaderPanel.Options.UseTextOptions = true;
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void ApplyMemoEditorToColumn(string fieldName, RepositoryItemMemoEdit memo)
        {
            var col = BusinessPartnerListGridView.Columns[fieldName];
            if (col == null) return;
            // Thêm repository vào GridControl nếu chưa có
            if (!BusinessPartnerListGridControl.RepositoryItems.Contains(memo))
            {
                BusinessPartnerListGridControl.RepositoryItems.Add(memo);
            }
            col.ColumnEdit = memo;
        }

        /// <summary>
        /// Tô màu/định dạng dòng theo trạng thái/loại đối tác.
        /// </summary>
        private void BusinessPartnerListGridView_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            try
            {
                var view = sender as GridView;
                if (view == null) return;
                if (e.RowHandle < 0) return;
                var row = view.GetRow(e.RowHandle) as BusinessPartnerListDto;
                if (row == null) return;
                // Không ghi đè màu khi đang chọn để giữ màu chọn mặc định của DevExpress
                if (view.IsRowSelected(e.RowHandle)) return;
                // Nền theo phân loại (PartnerType) với màu tương phản rõ
                // 1: Khách hàng (xanh), 2: Nhà cung cấp (vàng), 3: Cả hai (xanh lá)
                System.Drawing.Color backColor;
                switch (row.PartnerType)
                {
                    case 1:
                        backColor = System.Drawing.Color.LightSkyBlue; // xanh nổi bật
                        break;
                    case 2:
                        backColor = System.Drawing.Color.Moccasin; // vàng nổi bật
                        break;
                    case 3:
                        backColor = System.Drawing.Color.MediumAquamarine; // xanh lá nổi bật
                        break;
                    default:
                        backColor = System.Drawing.Color.White;
                        break;
                }

                e.Appearance.BackColor = backColor;
                e.Appearance.ForeColor = System.Drawing.Color.Black; // chữ đen tương phản tốt trên các màu nền trên
                e.Appearance.Options.UseBackColor = true;
                e.Appearance.Options.UseForeColor = true;

                // Nếu đối tác không hoạt động: làm nổi bật rõ ràng hơn
                if (row.IsActive == false)
                {
                    e.Appearance.BackColor = System.Drawing.Color.FromArgb(255, 205, 210); // đỏ nhạt nhưng đậm hơn (Light Red)
                    e.Appearance.ForeColor = System.Drawing.Color.DarkRed;
                    e.Appearance.Font = new System.Drawing.Font(e.Appearance.Font, System.Drawing.FontStyle.Strikeout);
                }
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
            if (_selectedPartnerIds == null || _selectedPartnerIds.Count == 0)
            {
                ShowInfo("Vui lòng chọn một dòng để chỉnh sửa.");
                return;
            }
            if (_selectedPartnerIds.Count > 1)
            {
                ShowInfo("Chỉ cho phép chỉnh sửa 1 dòng. Vui lòng bỏ chọn bớt.");
                return;
            }

            var id = _selectedPartnerIds[0];
            var dto = BusinessPartnerListGridView.GetFocusedRow() as BusinessPartnerListDto;
            if (dto == null || dto.Id != id)
            {
                // Tìm đúng DTO theo Id trong datasource nếu FocusedRow không khớp selection
                if (businessPartnerListDtoBindingSource.DataSource is System.Collections.IEnumerable list)
                {
                    foreach (var item in list)
                    {
                        if (item is BusinessPartnerListDto x && x.Id == id)
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

            // Tham khảo mẫu ConfigSqlServerInfoBarButtonItem_ItemClick: dùng ShowScope để auto-close overlay
            try
            {
                using (OverlayManager.ShowScope(this))
                {
                    using (var form = new FrmBusinessPartnerDetail(dto.Id))
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
            if (_selectedPartnerIds == null || _selectedPartnerIds.Count == 0)
            {
                ShowInfo("Vui lòng chọn ít nhất một dòng để xóa.");
                return;
            }

            var confirmMessage = _selectedPartnerIds.Count == 1
                ? "Bạn có chắc muốn xóa dòng dữ liệu đã chọn?"
                : $"Bạn có chắc muốn xóa {_selectedPartnerIds.Count} dòng dữ liệu đã chọn?";

            if (!MsgBox.GetConfirmFromYesNoDialog(confirmMessage)) return;

            try
            {
                await ExecuteWithWaitingFormAsync(async () =>
                {
                    foreach (var id in _selectedPartnerIds.ToList())
                    {
                        _businessPartnerBll.Delete(id);
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
        /// Người dùng bấm "Xuất".
        /// </summary>
        private void ExportBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // Chỉ cho phép xuất khi có dữ liệu hiển thị
            var rowCount = GridViewHelper.GetDisplayRowCount(BusinessPartnerListGridView) ?? 0;
            if (rowCount <= 0)
            {
                ShowInfo("Không có dữ liệu để xuất.");
                return;
            }

            GridViewHelper.ExportGridControl(BusinessPartnerListGridView, "BusinessPartners.xlsx");
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
                var entities = await _businessPartnerBll.GetAllAsync();
                var dtoList = entities.Select(x => x.ToListDto(ResolvePartnerTypeName)).ToList();
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
        private void BindGrid(List<BusinessPartnerListDto> data)
        {
            businessPartnerListDtoBindingSource.DataSource = data;
            BusinessPartnerListGridView.BestFitColumns();
            ConfigureMultiLineGridView();
            UpdateButtonStates();
        }

        /// <summary>
        /// Xóa trạng thái chọn hiện tại trên Grid.
        /// </summary>
        private void ClearSelectionState()
        {
            _selectedPartnerIds.Clear();
            BusinessPartnerListGridView.ClearSelection();
            BusinessPartnerListGridView.FocusedRowHandle = DevExpress.XtraGrid.GridControl.InvalidRowHandle;
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

        /// <summary>
        /// Resolve text hiển thị từ PartnerType.
        /// </summary>
        private static string ResolvePartnerTypeName(int type)
        {
            switch (type)
            {
                case 1: return "Khách hàng";
                case 2: return "Nhà cung cấp";
                case 3: return "Khách hàng & Nhà cung cấp";
                default: return "Không xác định";
            }
        }

        #endregion
    }
}
