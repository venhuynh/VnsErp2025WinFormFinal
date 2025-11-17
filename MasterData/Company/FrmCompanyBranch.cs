using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bll.Common;
using Bll.MasterData.Company;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSplashScreen;
using DTO.MasterData;
using DTO.MasterData.Company;
using MasterData.Company.Converters;
using MasterData.Company.Dto;

namespace MasterData.Company
{
    /// <summary>
    /// User Control quản lý danh sách chi nhánh công ty.
    /// Cung cấp giao diện hiển thị, tìm kiếm, thêm mới, sửa, xóa và xuất dữ liệu chi nhánh.
    /// </summary>
    public partial class FrmCompanyBranch : XtraForm
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
        public FrmCompanyBranch()
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

            // Setup SuperToolTips
            SetupSuperTips();
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

            if (!MsgBox.ShowYesNo(confirmMessage)) return;

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

        #region ========== SUPERTOOLTIP ==========

        /// <summary>
        /// Thiết lập SuperToolTip cho tất cả các controls trong UserControl
        /// </summary>
        private void SetupSuperTips()
        {
            try
            {
                SetupBarButtonSuperTips();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khi setup SuperToolTip");
            }
        }

        /// <summary>
        /// Thiết lập SuperToolTip cho các BarButtonItem
        /// </summary>
        private void SetupBarButtonSuperTips()
        {
            // SuperTip cho nút Tải dữ liệu
            SuperToolTipHelper.SetBarButtonSuperTip(
                ListDataBarButtonItem,
                title: @"<b><color=Blue>🔄 Tải dữ liệu</color></b>",
                content: @"Tải lại <b>danh sách chi nhánh công ty</b> từ database.<br/><br/><b>Chức năng:</b><br/>• Tải lại toàn bộ danh sách chi nhánh từ database<br/>• Hiển thị WaitForm trong quá trình tải<br/>• Cập nhật GridView với dữ liệu mới nhất<br/>• Tự động cập nhật thống kê (tổng số chi nhánh, số dòng đã chọn)<br/>• Xóa selection hiện tại sau khi tải<br/><br/><b>Quy trình:</b><br/>• Kiểm tra guard tránh re-entrancy (_isLoading)<br/>• Hiển thị WaitForm1<br/>• Gọi CompanyBranchBll.GetAllAsync() để lấy dữ liệu<br/>• Convert Entity → DTO<br/>• Bind dữ liệu vào GridView<br/>• Tự động fit columns<br/>• Cập nhật thống kê<br/>• Đóng WaitForm1<br/><br/><b>Kết quả:</b><br/>• GridView hiển thị danh sách chi nhánh mới nhất<br/>• Selection được xóa<br/>• Thống kê được cập nhật<br/><br/><color=Gray>Lưu ý:</color> Nút này sẽ tải lại toàn bộ dữ liệu từ database. Nếu đang có selection, selection sẽ bị xóa sau khi tải xong."
            );

            // SuperTip cho nút Thêm mới
            SuperToolTipHelper.SetBarButtonSuperTip(
                NewBarButtonItem,
                title: @"<b><color=Green>➕ Thêm mới</color></b>",
                content: @"Mở form <b>thêm mới chi nhánh công ty</b>.<br/><br/><b>Chức năng:</b><br/>• Mở form FrmCompanyBranchDetail ở chế độ thêm mới<br/>• Hiển thị overlay trên UserControl<br/>• Tự động tải lại dữ liệu sau khi đóng form<br/>• Cập nhật trạng thái các nút toolbar<br/><br/><b>Quy trình:</b><br/>• Hiển thị OverlayManager trên UserControl<br/>• Tạo form FrmCompanyBranchDetail với Guid.Empty (thêm mới)<br/>• Hiển thị form dạng modal dialog<br/>• Sau khi đóng form: Tải lại dữ liệu<br/>• Cập nhật trạng thái các nút toolbar<br/>• Đóng OverlayManager<br/><br/><b>Yêu cầu:</b><br/>• Form sẽ tự động lấy CompanyId từ database<br/>• Người dùng cần nhập đầy đủ thông tin bắt buộc<br/><br/><b>Kết quả:</b><br/>• Nếu lưu thành công: Danh sách được tải lại với chi nhánh mới<br/>• Nếu hủy: Không có thay đổi<br/><br/><color=Gray>Lưu ý:</color> Form sẽ được hiển thị ở chế độ modal, bạn phải đóng form trước khi có thể thao tác với UserControl này."
            );

            // SuperTip cho nút Sửa
            SuperToolTipHelper.SetBarButtonSuperTip(
                EditBarButtonItem,
                title: @"<b><color=Orange>✏️ Sửa</color></b>",
                content: @"Mở form <b>chỉnh sửa chi nhánh công ty</b>.<br/><br/><b>Chức năng:</b><br/>• Mở form FrmCompanyBranchDetail ở chế độ chỉnh sửa<br/>• Hiển thị overlay trên UserControl<br/>• Tự động tải lại dữ liệu sau khi đóng form<br/>• Cập nhật trạng thái các nút toolbar<br/><br/><b>Quy trình:</b><br/>• Kiểm tra selection: Phải chọn đúng 1 dòng<br/>• Hiển thị OverlayManager trên UserControl<br/>• Tạo form FrmCompanyBranchDetail với ID chi nhánh đã chọn<br/>• Hiển thị form dạng modal dialog<br/>• Sau khi đóng form: Tải lại dữ liệu<br/>• Cập nhật trạng thái các nút toolbar<br/>• Đóng OverlayManager<br/><br/><b>Yêu cầu:</b><br/>• Phải chọn đúng 1 dòng trong GridView<br/>• Nếu chọn nhiều hơn 1 dòng: Hiển thị thông báo yêu cầu bỏ chọn bớt<br/>• Nếu không chọn dòng nào: Hiển thị thông báo yêu cầu chọn dòng<br/><br/><b>Kết quả:</b><br/>• Nếu lưu thành công: Danh sách được tải lại với dữ liệu đã cập nhật<br/>• Nếu hủy: Không có thay đổi<br/><br/><color=Gray>Lưu ý:</color> Nút này chỉ được kích hoạt khi chọn đúng 1 dòng. Form sẽ được hiển thị ở chế độ modal."
            );

            // SuperTip cho nút Xóa
            SuperToolTipHelper.SetBarButtonSuperTip(
                DeleteBarButtonItem,
                title: @"<b><color=Red>🗑️ Xóa</color></b>",
                content: @"Xóa <b>chi nhánh công ty</b> đã chọn.<br/><br/><b>Chức năng:</b><br/>• Xóa một hoặc nhiều chi nhánh đã chọn<br/>• Validate business rules trước khi xóa<br/>• Hiển thị dialog xác nhận<br/>• Tự động tải lại dữ liệu sau khi xóa<br/><br/><b>Quy trình:</b><br/>• Kiểm tra selection: Phải chọn ít nhất 1 dòng<br/>• Validate business rules: Không cho phép xóa tất cả chi nhánh<br/>• Hiển thị dialog xác nhận (Yes/No)<br/>• Nếu xác nhận: Hiển thị WaitForm1<br/>• Xóa từng chi nhánh đã chọn qua CompanyBranchBll.Delete()<br/>• Tải lại dữ liệu<br/>• Đóng WaitForm1<br/><br/><b>Business Rules:</b><br/>• Không cho phép xóa nếu sẽ không còn chi nhánh nào<br/>• Công ty phải có ít nhất một chi nhánh<br/>• Không cho phép xóa chi nhánh cuối cùng<br/><br/><b>Yêu cầu:</b><br/>• Phải chọn ít nhất 1 dòng trong GridView<br/>• Phải xác nhận qua dialog Yes/No<br/>• Phải pass business rules validation<br/><br/><b>Kết quả:</b><br/>• Nếu thành công: Danh sách được tải lại, các chi nhánh đã chọn bị xóa<br/>• Nếu lỗi: Hiển thị thông báo lỗi, dữ liệu không thay đổi<br/><br/><color=Gray>Lưu ý:</color> Nút này chỉ được kích hoạt khi chọn ít nhất 1 dòng. Hệ thống sẽ không cho phép xóa tất cả chi nhánh để đảm bảo công ty luôn có ít nhất một chi nhánh."
            );

            // SuperTip cho nút Xuất dữ liệu
            SuperToolTipHelper.SetBarButtonSuperTip(
                ExportBarButtonItem,
                title: @"<b><color=Purple>📊 Xuất dữ liệu</color></b>",
                content: @"Xuất <b>danh sách chi nhánh công ty</b> ra file Excel.<br/><br/><b>Chức năng:</b><br/>• Xuất toàn bộ dữ liệu trong GridView ra file Excel<br/>• Hiển thị SaveFileDialog để chọn vị trí lưu file<br/>• Tự động đặt tên file mặc định: CompanyBranches.xlsx<br/>• Hiển thị thông báo thành công sau khi xuất<br/><br/><b>Quy trình:</b><br/>• Kiểm tra có dữ liệu trong GridView không<br/>• Hiển thị SaveFileDialog với filter Excel Files (*.xlsx)<br/>• Nếu người dùng chọn vị trí lưu: Gọi GridView.ExportToXlsx()<br/>• Hiển thị thông báo thành công<br/><br/><b>Yêu cầu:</b><br/>• Phải có ít nhất 1 dòng dữ liệu trong GridView<br/>• Người dùng phải chọn vị trí lưu file<br/><br/><b>Kết quả:</b><br/>• File Excel được tạo tại vị trí đã chọn<br/>• File chứa toàn bộ dữ liệu hiển thị trong GridView<br/>• Hiển thị thông báo thành công<br/><br/><color=Gray>Lưu ý:</color> Nút này chỉ được kích hoạt khi có dữ liệu trong GridView. File Excel sẽ chứa toàn bộ dữ liệu đang hiển thị, bao gồm cả các cột đã được cấu hình trong GridView."
            );
        }

        #endregion
    }
}
