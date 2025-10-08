using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bll.Common;
using Bll.MasterData.Company;
using Bll.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSplashScreen;
using MasterData.Company.Converters;
using MasterData.Company.Dto;

namespace MasterData.Company
{
    /// <summary>
    /// User Control quản lý danh sách chức vụ.
    /// Cung cấp giao diện hiển thị, tìm kiếm, thêm mới, sửa, xóa và xuất dữ liệu chức vụ.
    /// </summary>
    public partial class UcPosition : XtraUserControl
    {
        #region ========== KHAI BÁO BIẾN ==========

        /// <summary>
        /// Business Logic Layer cho chức vụ
        /// </summary>
        private readonly PositionBll _positionBll = new PositionBll();

        /// <summary>
        /// Danh sách ID chức vụ được chọn
        /// </summary>
        private List<Guid> _selectedPositionIds = new List<Guid>();

        /// <summary>
        /// Trạng thái đang tải dữ liệu (guard tránh gọi LoadDataAsync song song)
        /// </summary>
        private bool _isLoading;

        #endregion

        #region ========== CONSTRUCTOR & PUBLIC METHODS ==========

        /// <summary>
        /// Khởi tạo User Control quản lý danh sách chức vụ.
        /// </summary>
        public UcPosition()
        {
            InitializeComponent();

            // Toolbar events
            ListDataBarButtonItem.ItemClick += ListDataBarButtonItem_ItemClick;
            NewBarButtonItem.ItemClick += NewBarButtonItem_ItemClick;
            EditBarButtonItem.ItemClick += EditBarButtonItem_ItemClick;
            DeleteBarButtonItem.ItemClick += DeleteBarButtonItem_ItemClick;
            ExportBarButtonItem.ItemClick += ExportBarButtonItem_ItemClick;

            // Grid events
            PositionGridView.SelectionChanged += PositionGridView_SelectionChanged;
            PositionGridView.DoubleClick += PositionGridView_DoubleClick;
            PositionGridView.CustomDrawRowIndicator += PositionGridView_CustomDrawRowIndicator;
            PositionGridView.RowCellStyle += PositionGridView_RowCellStyle;

            UpdateButtonStates();
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
                var positions = await _positionBll.GetAllAsync();
                var dtoList = positions.Select(p => p.ToDto()).ToList();
                
                BindGrid(dtoList);
                UpdateDataSummary();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi tải dữ liệu");
            }
        }

        /// <summary>
        /// Bind danh sách DTO vào Grid và cấu hình hiển thị.
        /// </summary>
        private void BindGrid(List<PositionDto> data)
        {
            // Clear selection trước khi bind data mới
            ClearSelectionState();
            
            positionDtoBindingSource.DataSource = data;
            PositionGridView.BestFitColumns();
            
            PositionGridView.ExpandAllGroups();
            
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
                    using (var form = new FrmPositionDetail(Guid.Empty))
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
            if (_selectedPositionIds == null || _selectedPositionIds.Count == 0)
            {
                ShowInfo("Vui lòng chọn một dòng để chỉnh sửa.");
                return;
            }
            if (_selectedPositionIds.Count > 1)
            {
                ShowInfo("Chỉ cho phép chỉnh sửa 1 dòng. Vui lòng bỏ chọn bớt.");
                return;
            }

            var id = _selectedPositionIds[0];
            try
            {
                using (OverlayManager.ShowScope(this))
                {
                    using (var form = new FrmPositionDetail(id))
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

        /// <summary>
        /// Xử lý sự kiện click button Xóa
        /// </summary>
        private async void DeleteBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (_selectedPositionIds == null || _selectedPositionIds.Count == 0)
            {
                ShowInfo("Vui lòng chọn ít nhất một dòng để xóa.");
                return;
            }

            // Kiểm tra logic business: không cho phép xóa nếu Company không còn chức vụ nào
            if (!await ValidateDeleteBusinessRules())
            {
                return;
            }

            var confirmMessage = _selectedPositionIds.Count == 1
                ? "Bạn có chắc muốn xóa dòng dữ liệu đã chọn?"
                : $"Bạn có chắc muốn xóa {_selectedPositionIds.Count} dòng dữ liệu đã chọn?";

            if (!MsgBox.GetConfirmFromYesNoDialog(confirmMessage)) return;

            try
            {
                await ExecuteWithWaitingFormAsync(() =>
                {
                    foreach (var id in _selectedPositionIds)
                    {
                        _positionBll.Delete(id);
                    }

                    return Task.CompletedTask;
                });
                
                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi xóa dữ liệu");
            }
        }

        /// <summary>
        /// Xử lý sự kiện click button Xuất dữ liệu
        /// </summary>
        private void ExportBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            var rowCount = PositionGridView.RowCount;
            if (rowCount <= 0)
            {
                ShowInfo("Không có dữ liệu để xuất.");
                return;
            }

            try
            {
                var saveDialog = new SaveFileDialog
                {
                    Filter = "Excel Files (*.xlsx)|*.xlsx|All Files (*.*)|*.*",
                    FileName = "Positions.xlsx"
                };

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    PositionGridView.ExportToXlsx(saveDialog.FileName);
                    ShowInfo("Xuất dữ liệu thành công!");
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi xuất dữ liệu");
            }
        }

        /// <summary>
        /// Xử lý sự kiện thay đổi selection trên Grid
        /// </summary>
        private void PositionGridView_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                UpdateSelectedPositionIds();
                UpdateButtonStates();
                UpdateDataSummary();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        /// <summary>
        /// Xử lý sự kiện double click trên Grid
        /// </summary>
        private async void PositionGridView_DoubleClick(object sender, EventArgs e)
        {
            if (_selectedPositionIds == null || _selectedPositionIds.Count == 0)
            {
                ShowInfo("Vui lòng chọn một dòng để xem chi tiết.");
                return;
            }
            if (_selectedPositionIds.Count > 1)
            {
                ShowInfo("Chỉ cho phép xem chi tiết 1 dòng. Vui lòng bỏ chọn bớt.");
                return;
            }

            var id = _selectedPositionIds[0];
            try
            {
                using (OverlayManager.ShowScope(this))
                {
                    using (var form = new FrmPositionDetail(id))
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
                ShowError(ex, "Lỗi hiển thị màn hình chi tiết");
            }
        }

        /// <summary>
        /// Xử lý sự kiện vẽ số thứ tự dòng cho Grid
        /// </summary>
        private void PositionGridView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        /// <summary>
        /// Xử lý sự kiện tô màu cell theo trạng thái
        /// </summary>
        private void PositionGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            try
            {
                var gridView = sender as GridView;
                if (gridView == null) return;
                
                var row = gridView.GetRow(e.RowHandle) as PositionDto;
                if (row == null) return;
                
                // Format các dòng dữ liệu không hoạt động với màu chữ đỏ
                if (!row.IsActive)
                {
                    e.Appearance.ForeColor = Color.Red;
                }
            }
            catch (Exception)
            {
                // ignore style errors
            }
        }

        #endregion

        #region ========== XỬ LÝ DỮ LIỆU ==========

        /// <summary>
        /// Cập nhật danh sách selected position IDs.
        /// </summary>
        private void UpdateSelectedPositionIds()
        {
            _selectedPositionIds.Clear();
            
            var selectedRows = PositionGridView.GetSelectedRows();
            foreach (var rowHandle in selectedRows)
            {
                if (rowHandle >= 0)
                {
                    if (PositionGridView.GetRow(rowHandle) is PositionDto dto && !_selectedPositionIds.Contains(dto.Id))
                    {
                        _selectedPositionIds.Add(dto.Id);
                    }
                }
            }
        }

        /// <summary>
        /// Xóa trạng thái chọn hiện tại trên Grid.
        /// </summary>
        private void ClearSelectionState()
        {
            _selectedPositionIds.Clear();
            PositionGridView.ClearSelection();
            PositionGridView.FocusedRowHandle = GridControl.InvalidRowHandle;
            UpdateButtonStates();
        }

        #endregion

        #region ========== BUSINESS RULES VALIDATION ==========

        /// <summary>
        /// Validate business rules cho việc xóa chức vụ
        /// </summary>
        /// <returns>True nếu được phép xóa, False nếu không</returns>
        private async Task<bool> ValidateDeleteBusinessRules()
        {
            try
            {
                // Lấy tất cả chức vụ hiện có
                var allPositions = await _positionBll.GetAllAsync();
                var totalPositions = allPositions.Count;
                var selectedPositions = _selectedPositionIds.Count;

                // Kiểm tra: không cho phép xóa nếu sẽ không còn chức vụ nào
                if (totalPositions - selectedPositions <= 0)
                {
                    ShowInfo("Không thể xóa tất cả chức vụ. Công ty phải có ít nhất một chức vụ.");
                    return false;
                }

                // Kiểm tra: không cho phép xóa nếu chỉ còn 1 chức vụ và đang chọn xóa chức vụ đó
                if (totalPositions == 1 && selectedPositions == 1)
                {
                    ShowInfo("Không thể xóa chức vụ cuối cùng. Công ty phải có ít nhất một chức vụ.");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi kiểm tra business rules");
                return false;
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
                var selectedCount = _selectedPositionIds?.Count ?? 0;
                // Edit: chỉ khi chọn đúng 1 dòng
                if (EditBarButtonItem != null)
                    EditBarButtonItem.Enabled = selectedCount == 1;
                // Delete: khi chọn >= 1 dòng
                if (DeleteBarButtonItem != null)
                    DeleteBarButtonItem.Enabled = selectedCount >= 1;
                // Export: chỉ khi có dữ liệu hiển thị
                var rowCount = PositionGridView.RowCount;
                if (ExportBarButtonItem != null)
                    ExportBarButtonItem.Enabled = rowCount > 0;
            }
            catch
            {
                // ignore
            }
        }

        /// <summary>
        /// Cập nhật thông tin tổng kết dữ liệu
        /// </summary>
        private void UpdateDataSummary()
        {
            try
            {
                var totalRows = PositionGridView.RowCount;
                var selectedRows = _selectedPositionIds?.Count ?? 0;
                
                DataSummaryBarStaticItem.Caption = $"Tổng: {totalRows} chức vụ";
                SelectedRowBarStaticItem.Caption = selectedRows > 0 ? $"Đã chọn: {selectedRows} dòng" : "Chưa chọn dòng nào";
            }
            catch
            {
                // ignore
            }
        }

        #endregion

        #region ========== TIỆN ÍCH HỖ TRỢ ==========

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
            MsgBox.ShowException(string.IsNullOrWhiteSpace(context)
                ? ex
                : new Exception(context + ": " + ex.Message, ex));
        }

        #endregion
    }
}
