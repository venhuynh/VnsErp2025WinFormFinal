using Bll.MasterData.CompanyBll;
using Common.Common;
using Common.Utils;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSplashScreen;
using DTO.MasterData.Company;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MasterData.Company
{
    /// <summary>
    /// Form quản lý danh sách phòng ban.
    /// Cung cấp giao diện hiển thị dạng GridView, tìm kiếm, thêm mới, sửa, xóa và xuất dữ liệu phòng ban.
    /// </summary>
    public partial class FrmDepartment : XtraForm
    {
        #region ========== KHAI BÁO BIẾN ==========

        /// <summary>
        /// Business Logic Layer cho phòng ban
        /// </summary>
        private readonly DepartmentBll _departmentBll = new DepartmentBll();

        /// <summary>
        /// Danh sách ID phòng ban được chọn
        /// </summary>
        private readonly List<Guid> _selectedDepartmentIds = [];

        /// <summary>
        /// Trạng thái đang tải dữ liệu (guard tránh gọi LoadDataAsync song song)
        /// </summary>
        private bool _isLoading;

        #endregion

        #region ========== CONSTRUCTOR ==========

        /// <summary>
        /// Khởi tạo Form quản lý danh sách phòng ban.
        /// </summary>
        public FrmDepartment()
        {
            InitializeComponent();

            // Toolbar events
            ListDataBarButtonItem.ItemClick += ListDataBarButtonItem_ItemClick;
            NewBarButtonItem.ItemClick += NewBarButtonItem_ItemClick;
            EditBarButtonItem.ItemClick += EditBarButtonItem_ItemClick;
            DeleteBarButtonItem.ItemClick += DeleteBarButtonItem_ItemClick;
            ExportBarButtonItem.ItemClick += ExportBarButtonItem_ItemClick;

            // GridView events
            DepartmentDtoGridView.SelectionChanged += DepartmentDtoGridView_SelectionChanged;
            DepartmentDtoGridView.CustomDrawRowIndicator += DepartmentDtoGridView_CustomDrawRowIndicator;

            UpdateButtonStates();

            // Setup SuperToolTips
            SetupSuperToolTips();
        }

        #endregion

        #region ========== QUẢN LÝ DỮ LIỆU ==========

        /// <summary>
        /// Tải dữ liệu và bind vào GridView (Async, hiển thị WaitForm).
        /// </summary>
        private async Task LoadDataAsync()
        {
            if (_isLoading) return; // tránh re-entrancy
            _isLoading = true;
            try
            {
                await ExecuteWithWaitingFormAsync(async () => { await LoadDataAsyncWithoutSplash(); });
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
        /// Tải dữ liệu và bind vào GridView (Async, không hiển thị WaitForm).
        /// </summary>
        private async Task LoadDataAsyncWithoutSplash()
        {
            try
            {
                // Lấy entities từ BLL (async method)
                var departmentEntities = await _departmentBll.GetAllAsync();

                // Tạo dictionary để tính FullPath
                var departmentDict = departmentEntities.ToDictionary(d => d.Id);

                // Convert entities to DTOs với departmentDict để tính FullPath
                var dtoList = departmentEntities.ToDepartmentDtos(departmentDict).ToList();

                BindGrid(dtoList);
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi tải dữ liệu");
            }
        }

        /// <summary>
        /// Bind danh sách DTO vào GridView và cấu hình hiển thị.
        /// </summary>
        private void BindGrid(List<DepartmentDto> data)
        {
            // Clear selection trước khi bind data mới
            ClearSelectionState();

            departmentDtoBindingSource.DataSource = data;
            DepartmentDtoGridView.BestFitColumns();
            ConfigureMultiLineGridView();

            // Đảm bảo selection được clear sau khi bind
            ClearSelectionState();
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
                ShowError(ex, "Lỗi tải dữ liệu");
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
                    using (var form = new FrmDepartmentDetail(Guid.Empty))
                    {
                        form.StartPosition = FormStartPosition.CenterParent;
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
        /// Xử lý sự kiện click button Sửa
        /// </summary>
        private async void EditBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                // Chỉ cho phép chỉnh sửa 1 dòng dữ liệu
                if (_selectedDepartmentIds == null || _selectedDepartmentIds.Count == 0)
                {
                    ShowInfo("Vui lòng chọn một dòng để chỉnh sửa.");
                    return;
                }

                if (_selectedDepartmentIds.Count > 1)
                {
                    ShowInfo("Chỉ cho phép chỉnh sửa 1 dòng. Vui lòng bỏ chọn bớt.");
                    return;
                }

                var id = _selectedDepartmentIds[0];
                var focusedRowHandle = DepartmentDtoGridView.FocusedRowHandle;
                DepartmentDto dto = null;

                if (focusedRowHandle >= 0)
                {
                    // Lấy dữ liệu từ focused row
                    dto = DepartmentDtoGridView.GetRow(focusedRowHandle) as DepartmentDto;
                }

                if (dto == null || dto.Id != id)
                {
                    // Tìm đúng DTO theo Id trong datasource nếu FocusedRow không khớp selection
                    if (departmentDtoBindingSource.DataSource is IEnumerable<DepartmentDto> list)
                    {
                        dto = list.FirstOrDefault(x => x.Id == id);
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
                        using (var form = new FrmDepartmentDetail(dto.Id))
                        {
                            form.StartPosition = FormStartPosition.CenterParent;
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
                if (_selectedDepartmentIds == null || _selectedDepartmentIds.Count == 0)
                {
                    ShowInfo("Vui lòng chọn ít nhất một dòng để xóa.");
                    return;
                }

                var confirmMessage = _selectedDepartmentIds.Count == 1
                    ? "Bạn có chắc muốn xóa dòng dữ liệu đã chọn?"
                    : $"Bạn có chắc muốn xóa {_selectedDepartmentIds.Count} dòng dữ liệu đã chọn?";

                if (!MsgBox.ShowYesNo(confirmMessage)) return;

                try
                {
                    await ExecuteWithWaitingFormAsync(async () =>
                    {
                        var success = await Task.Run(() =>
                            _departmentBll.DeleteMultiple(_selectedDepartmentIds.ToList()));
                        if (success)
                        {
                            ShowInfo($"Đã xóa {_selectedDepartmentIds.Count} phòng ban thành công.");
                        }
                        else
                        {
                            ShowError(new Exception("Không thể xóa phòng ban. Vui lòng thử lại."), null);
                        }
                    });

                    await LoadDataAsync();
                }
                catch (Exception ex)
                {
                    ShowError(ex, "Lỗi xóa dữ liệu");
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
            var rowCount = DepartmentDtoGridView.RowCount;
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
                    FileName = "Departments.xlsx"
                };

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    DepartmentDtoGridView.ExportToXlsx(saveDialog.FileName);
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
        private void DepartmentDtoGridView_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                // Cập nhật danh sách selected IDs khi selection thay đổi
                UpdateSelectedDepartmentIds();
                UpdateButtonStates();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        /// <summary>
        /// Xử lý sự kiện vẽ số thứ tự dòng cho GridView
        /// </summary>
        private void DepartmentDtoGridView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        #endregion

        #region ========== XỬ LÝ DỮ LIỆU ==========

        /// <summary>
        /// Cấu hình GridView để hiển thị dữ liệu với format chuyên nghiệp.
        /// </summary>
        private void ConfigureMultiLineGridView()
        {
            try
            {
                // Cấu hình sắp xếp mặc định theo DepartmentCode, sau đó DepartmentName
                if (DepartmentDtoGridView.Columns["colDepartmentCode"] != null)
                {
                    DepartmentDtoGridView.Columns["colDepartmentCode"].SortOrder =
                        DevExpress.Data.ColumnSortOrder.Ascending;
                }
                else if (DepartmentDtoGridView.Columns["colDepartmentName"] != null)
                {
                    DepartmentDtoGridView.Columns["colDepartmentName"].SortOrder =
                        DevExpress.Data.ColumnSortOrder.Ascending;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in ConfigureMultiLineGridView: {ex.Message}");
            }
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
                var selectedCount = _selectedDepartmentIds?.Count ?? 0;
                // Edit: chỉ khi chọn đúng 1 dòng
                if (EditBarButtonItem != null)
                    EditBarButtonItem.Enabled = selectedCount == 1;
                // Delete: khi chọn >= 1 dòng
                if (DeleteBarButtonItem != null)
                    DeleteBarButtonItem.Enabled = selectedCount >= 1;
                // Export: chỉ khi có dữ liệu hiển thị
                var rowCount = DepartmentDtoGridView.RowCount;
                if (ExportBarButtonItem != null)
                    ExportBarButtonItem.Enabled = rowCount > 0;
            }
            catch
            {
                // ignore
            }
        }

        /// <summary>
        /// Cập nhật danh sách selected department IDs.
        /// </summary>
        private void UpdateSelectedDepartmentIds()
        {
            _selectedDepartmentIds.Clear();

            // Lấy tất cả rows đã được chọn
            var selectedRows = DepartmentDtoGridView.GetSelectedRows();
            foreach (int rowHandle in selectedRows)
            {
                if (rowHandle >= 0)
                {
                    var dto = DepartmentDtoGridView.GetRow(rowHandle) as DepartmentDto;
                    if (dto != null && !_selectedDepartmentIds.Contains(dto.Id))
                    {
                        _selectedDepartmentIds.Add(dto.Id);
                    }
                }
            }
        }

        /// <summary>
        /// Xóa trạng thái chọn hiện tại trên GridView.
        /// </summary>
        private void ClearSelectionState()
        {
            _selectedDepartmentIds.Clear();

            // Clear tất cả selection
            DepartmentDtoGridView.ClearSelection();
            DepartmentDtoGridView.FocusedRowHandle = DevExpress.XtraGrid.GridControl.InvalidRowHandle;

            UpdateButtonStates();
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
                        content: "Tải lại danh sách phòng ban từ hệ thống."
                    );
                }

                if (NewBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        NewBarButtonItem,
                        title: "<b><color=Green>➕ Thêm mới</color></b>",
                        content: "Thêm mới phòng ban vào hệ thống."
                    );
                }

                if (EditBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        EditBarButtonItem,
                        title: "<b><color=Orange>✏️ Sửa</color></b>",
                        content: "Chỉnh sửa thông tin phòng ban đã chọn."
                    );
                }

                if (DeleteBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        DeleteBarButtonItem,
                        title: "<b><color=Red>🗑️ Xóa</color></b>",
                        content: "Xóa các phòng ban đã chọn khỏi hệ thống."
                    );
                }

                if (ExportBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        ExportBarButtonItem,
                        title: "<b><color=Purple>📊 Xuất Excel</color></b>",
                        content: "Xuất danh sách phòng ban ra file Excel."
                    );
                }
            }
            catch (Exception ex)
            {
                // Ignore lỗi setup SuperToolTip để không chặn Form
                System.Diagnostics.Debug.WriteLine($"Error in SetupSuperToolTips: {ex.Message}");
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
            MsgBox.ShowException(string.IsNullOrWhiteSpace(context)
                ? ex
                : new Exception(context + ": " + ex.Message, ex));
        }

        #endregion
    }
}