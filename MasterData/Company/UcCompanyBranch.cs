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
    /// User Control quản lý danh sách chi nhánh công ty.
    /// Cung cấp giao diện hiển thị, tìm kiếm, thêm mới, sửa, xóa và xuất dữ liệu chi nhánh.
    /// </summary>
    public partial class UcCompanyBranch : XtraUserControl
    {
        #region ========== KHAI BÁO BIẾN ==========

        /// <summary>
        /// Business Logic Layer cho chi nhánh công ty
        /// </summary>
        private readonly CompanyBranchBll _companyBranchBll = new CompanyBranchBll();

        /// <summary>
        /// Danh sách ID chi nhánh được chọn
        /// </summary>
        private List<Guid> _selectedBranchIds = new List<Guid>();

        /// <summary>
        /// Trạng thái đang tải dữ liệu (guard tránh gọi LoadDataAsync song song)
        /// </summary>
        private bool _isLoading;

        #endregion

        #region ========== CONSTRUCTOR & PUBLIC METHODS ==========

        /// <summary>
        /// Khởi tạo User Control quản lý danh sách chi nhánh công ty.
        /// </summary>
        public UcCompanyBranch()
        {
            InitializeComponent();

            // Toolbar events
            ListDataBarButtonItem.ItemClick += ListDataBarButtonItem_ItemClick;
            NewBarButtonItem.ItemClick += NewBarButtonItem_ItemClick;
            EditBarButtonItem.ItemClick += EditBarButtonItem_ItemClick;
            DeleteBarButtonItem.ItemClick += DeleteBarButtonItem_ItemClick;
            ExportBarButtonItem.ItemClick += ExportBarButtonItem_ItemClick;

            // Grid events
            CompanyBranchGridView.SelectionChanged += CompanyBranchGridView_SelectionChanged;
            CompanyBranchGridView.DoubleClick += CompanyBranchGridView_DoubleClick;
            CompanyBranchGridView.CustomDrawRowIndicator += CompanyBranchGridView_CustomDrawRowIndicator;
            CompanyBranchGridView.RowCellStyle += CompanyBranchGridView_RowCellStyle;

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
                var branches = await _companyBranchBll.GetAllAsync();
                var dtoList = branches.Select(b => b.ToDto()).ToList();
                
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
        private void BindGrid(List<CompanyBranchDto> data)
        {
            // Clear selection trước khi bind data mới
            ClearSelectionState();
            
            companyBranchDtoBindingSource.DataSource = data;
            CompanyBranchGridView.BestFitColumns();
            
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
                    using (var form = new FrmCompanyBranchDetail(Guid.Empty))
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
            if (_selectedBranchIds == null || _selectedBranchIds.Count == 0)
            {
                ShowInfo("Vui lòng chọn một dòng để chỉnh sửa.");
                return;
            }
            if (_selectedBranchIds.Count > 1)
            {
                ShowInfo("Chỉ cho phép chỉnh sửa 1 dòng. Vui lòng bỏ chọn bớt.");
                return;
            }

            var id = _selectedBranchIds[0];
            try
            {
                using (OverlayManager.ShowScope(this))
                {
                    using (var form = new FrmCompanyBranchDetail(id))
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
            if (_selectedBranchIds == null || _selectedBranchIds.Count == 0)
            {
                ShowInfo("Vui lòng chọn ít nhất một dòng để xóa.");
                return;
            }

            // Kiểm tra logic business: không cho phép xóa nếu Company không còn chi nhánh nào
            if (!await ValidateDeleteBusinessRules())
            {
                return;
            }

            var confirmMessage = _selectedBranchIds.Count == 1
                ? "Bạn có chắc muốn xóa dòng dữ liệu đã chọn?"
                : $"Bạn có chắc muốn xóa {_selectedBranchIds.Count} dòng dữ liệu đã chọn?";

            if (!MsgBox.GetConfirmFromYesNoDialog(confirmMessage)) return;

            try
            {
                await ExecuteWithWaitingFormAsync(() =>
                {
                    foreach (var id in _selectedBranchIds)
                    {
                        _companyBranchBll.Delete(id);
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
            var rowCount = CompanyBranchGridView.RowCount;
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
                    FileName = "CompanyBranches.xlsx"
                };

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    CompanyBranchGridView.ExportToXlsx(saveDialog.FileName);
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
        private void CompanyBranchGridView_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                UpdateSelectedBranchIds();
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
        private async void CompanyBranchGridView_DoubleClick(object sender, EventArgs e)
        {
            if (_selectedBranchIds == null || _selectedBranchIds.Count == 0)
            {
                ShowInfo("Vui lòng chọn một dòng để xem chi tiết.");
                return;
            }
            if (_selectedBranchIds.Count > 1)
            {
                ShowInfo("Chỉ cho phép xem chi tiết 1 dòng. Vui lòng bỏ chọn bớt.");
                return;
            }

            var id = _selectedBranchIds[0];
            try
            {
                using (OverlayManager.ShowScope(this))
                {
                    using (var form = new FrmCompanyBranchDetail(id))
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
        private void CompanyBranchGridView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        /// <summary>
        /// Xử lý sự kiện tô màu cell theo trạng thái
        /// </summary>
        private void CompanyBranchGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            try
            {
                var gridView = sender as GridView;
                if (gridView == null) return;
                
                var row = gridView.GetRow(e.RowHandle) as CompanyBranchDto;
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
        /// Cập nhật danh sách selected branch IDs.
        /// </summary>
        private void UpdateSelectedBranchIds()
        {
            _selectedBranchIds.Clear();
            
            var selectedRows = CompanyBranchGridView.GetSelectedRows();
            foreach (var rowHandle in selectedRows)
            {
                if (rowHandle >= 0)
                {
                    if (CompanyBranchGridView.GetRow(rowHandle) is CompanyBranchDto dto && !_selectedBranchIds.Contains(dto.Id))
                    {
                        _selectedBranchIds.Add(dto.Id);
                    }
                }
            }
        }

        /// <summary>
        /// Xóa trạng thái chọn hiện tại trên Grid.
        /// </summary>
        private void ClearSelectionState()
        {
            _selectedBranchIds.Clear();
            CompanyBranchGridView.ClearSelection();
            CompanyBranchGridView.FocusedRowHandle = GridControl.InvalidRowHandle;
            UpdateButtonStates();
        }

        #endregion

        #region ========== BUSINESS RULES VALIDATION ==========

        /// <summary>
        /// Validate business rules cho việc xóa chi nhánh
        /// </summary>
        /// <returns>True nếu được phép xóa, False nếu không</returns>
        private async Task<bool> ValidateDeleteBusinessRules()
        {
            try
            {
                // Lấy tất cả chi nhánh hiện có
                var allBranches = await _companyBranchBll.GetAllAsync();
                var totalBranches = allBranches.Count;
                var selectedBranches = _selectedBranchIds.Count;

                // Kiểm tra: không cho phép xóa nếu sẽ không còn chi nhánh nào
                if (totalBranches - selectedBranches <= 0)
                {
                    ShowInfo("Không thể xóa tất cả chi nhánh. Công ty phải có ít nhất một chi nhánh.");
                    return false;
                }

                // Kiểm tra: không cho phép xóa nếu chỉ còn 1 chi nhánh và đang chọn xóa chi nhánh đó
                if (totalBranches == 1 && selectedBranches == 1)
                {
                    ShowInfo("Không thể xóa chi nhánh cuối cùng. Công ty phải có ít nhất một chi nhánh.");
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
                var selectedCount = _selectedBranchIds?.Count ?? 0;
                // Edit: chỉ khi chọn đúng 1 dòng
                if (EditBarButtonItem != null)
                    EditBarButtonItem.Enabled = selectedCount == 1;
                // Delete: khi chọn >= 1 dòng
                if (DeleteBarButtonItem != null)
                    DeleteBarButtonItem.Enabled = selectedCount >= 1;
                // Export: chỉ khi có dữ liệu hiển thị
                var rowCount = CompanyBranchGridView.RowCount;
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
                var totalRows = CompanyBranchGridView.RowCount;
                var selectedRows = _selectedBranchIds?.Count ?? 0;
                
                DataSummaryBarStaticItem.Caption = $"Tổng: {totalRows} chi nhánh";
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
