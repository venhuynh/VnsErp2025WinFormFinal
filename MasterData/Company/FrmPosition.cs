using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bll.Common;
using Bll.MasterData.CompanyBll;
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
    public partial class FrmPosition : XtraForm
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
        public FrmPosition()
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

            if (!MsgBox.ShowYesNo(confirmMessage)) return;

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
                content: @"Tải lại <b>danh sách chức vụ</b> từ database.<br/><br/><b>Chức năng:</b><br/>• Tải lại toàn bộ danh sách chức vụ từ database<br/>• Hiển thị WaitForm trong quá trình tải<br/>• Cập nhật GridView với dữ liệu mới nhất<br/>• Tự động cập nhật thống kê (tổng số chức vụ, số dòng đã chọn)<br/>• Xóa selection hiện tại sau khi tải<br/>• Tự động expand tất cả groups trong GridView<br/><br/><b>Quy trình:</b><br/>• Kiểm tra guard tránh re-entrancy (_isLoading)<br/>• Hiển thị WaitForm1<br/>• Gọi PositionBll.GetAllAsync() để lấy dữ liệu<br/>• Convert Entity → DTO<br/>• Bind dữ liệu vào GridView<br/>• Tự động fit columns<br/>• Tự động expand tất cả groups<br/>• Cập nhật thống kê<br/>• Đóng WaitForm1<br/><br/><b>Kết quả:</b><br/>• GridView hiển thị danh sách chức vụ mới nhất<br/>• Selection được xóa<br/>• Thống kê được cập nhật<br/><br/><color=Gray>Lưu ý:</color> Nút này sẽ tải lại toàn bộ dữ liệu từ database. Nếu đang có selection, selection sẽ bị xóa sau khi tải xong."
            );

            // SuperTip cho nút Thêm mới
            SuperToolTipHelper.SetBarButtonSuperTip(
                NewBarButtonItem,
                title: @"<b><color=Green>➕ Thêm mới</color></b>",
                content: @"Mở form <b>thêm mới chức vụ</b>.<br/><br/><b>Chức năng:</b><br/>• Mở form FrmPositionDetail ở chế độ thêm mới<br/>• Hiển thị overlay trên UserControl<br/>• Tự động tải lại dữ liệu sau khi đóng form<br/>• Cập nhật trạng thái các nút toolbar<br/><br/><b>Quy trình:</b><br/>• Hiển thị OverlayManager trên UserControl<br/>• Tạo form FrmPositionDetail với Guid.Empty (thêm mới)<br/>• Hiển thị form dạng modal dialog<br/>• Sau khi đóng form: Tải lại dữ liệu<br/>• Cập nhật trạng thái các nút toolbar<br/>• Đóng OverlayManager<br/><br/><b>Yêu cầu:</b><br/>• Form sẽ tự động lấy CompanyId từ database<br/>• Người dùng cần nhập đầy đủ thông tin bắt buộc (Mã chức vụ, Tên chức vụ)<br/><br/><b>Kết quả:</b><br/>• Nếu lưu thành công: Danh sách được tải lại với chức vụ mới<br/>• Nếu hủy: Không có thay đổi<br/><br/><color=Gray>Lưu ý:</color> Form sẽ được hiển thị ở chế độ modal, bạn phải đóng form trước khi có thể thao tác với UserControl này."
            );

            // SuperTip cho nút Sửa
            SuperToolTipHelper.SetBarButtonSuperTip(
                EditBarButtonItem,
                title: @"<b><color=Orange>✏️ Sửa</color></b>",
                content: @"Mở form <b>chỉnh sửa chức vụ</b>.<br/><br/><b>Chức năng:</b><br/>• Mở form FrmPositionDetail ở chế độ chỉnh sửa<br/>• Hiển thị overlay trên UserControl<br/>• Tự động tải lại dữ liệu sau khi đóng form<br/>• Cập nhật trạng thái các nút toolbar<br/><br/><b>Quy trình:</b><br/>• Kiểm tra selection: Phải chọn đúng 1 dòng<br/>• Hiển thị OverlayManager trên UserControl<br/>• Tạo form FrmPositionDetail với ID chức vụ đã chọn<br/>• Hiển thị form dạng modal dialog<br/>• Sau khi đóng form: Tải lại dữ liệu<br/>• Cập nhật trạng thái các nút toolbar<br/>• Đóng OverlayManager<br/><br/><b>Yêu cầu:</b><br/>• Phải chọn đúng 1 dòng trong GridView<br/>• Nếu chọn nhiều hơn 1 dòng: Hiển thị thông báo yêu cầu bỏ chọn bớt<br/>• Nếu không chọn dòng nào: Hiển thị thông báo yêu cầu chọn dòng<br/><br/><b>Kết quả:</b><br/>• Nếu lưu thành công: Danh sách được tải lại với dữ liệu đã cập nhật<br/>• Nếu hủy: Không có thay đổi<br/><br/><color=Gray>Lưu ý:</color> Nút này chỉ được kích hoạt khi chọn đúng 1 dòng. Form sẽ được hiển thị ở chế độ modal. Mã chức vụ sẽ bị khóa và không thể chỉnh sửa."
            );

            // SuperTip cho nút Xóa
            SuperToolTipHelper.SetBarButtonSuperTip(
                DeleteBarButtonItem,
                title: @"<b><color=Red>🗑️ Xóa</color></b>",
                content: @"Xóa <b>chức vụ</b> đã chọn.<br/><br/><b>Chức năng:</b><br/>• Xóa một hoặc nhiều chức vụ đã chọn<br/>• Validate business rules trước khi xóa<br/>• Hiển thị dialog xác nhận<br/>• Tự động tải lại dữ liệu sau khi xóa<br/><br/><b>Quy trình:</b><br/>• Kiểm tra selection: Phải chọn ít nhất 1 dòng<br/>• Validate business rules: Không cho phép xóa tất cả chức vụ<br/>• Hiển thị dialog xác nhận (Yes/No)<br/>• Nếu xác nhận: Hiển thị WaitForm1<br/>• Xóa từng chức vụ đã chọn qua PositionBll.Delete()<br/>• Tải lại dữ liệu<br/>• Đóng WaitForm1<br/><br/><b>Business Rules:</b><br/>• Không cho phép xóa nếu sẽ không còn chức vụ nào<br/>• Công ty phải có ít nhất một chức vụ<br/>• Không cho phép xóa chức vụ cuối cùng<br/><br/><b>Yêu cầu:</b><br/>• Phải chọn ít nhất 1 dòng trong GridView<br/>• Phải xác nhận qua dialog Yes/No<br/>• Phải pass business rules validation<br/><br/><b>Kết quả:</b><br/>• Nếu thành công: Danh sách được tải lại, các chức vụ đã chọn bị xóa<br/>• Nếu lỗi: Hiển thị thông báo lỗi, dữ liệu không thay đổi<br/><br/><color=Gray>Lưu ý:</color> Nút này chỉ được kích hoạt khi chọn ít nhất 1 dòng. Hệ thống sẽ không cho phép xóa tất cả chức vụ để đảm bảo công ty luôn có ít nhất một chức vụ."
            );

            // SuperTip cho nút Xuất dữ liệu
            SuperToolTipHelper.SetBarButtonSuperTip(
                ExportBarButtonItem,
                title: @"<b><color=Purple>📊 Xuất dữ liệu</color></b>",
                content: @"Xuất <b>danh sách chức vụ</b> ra file Excel.<br/><br/><b>Chức năng:</b><br/>• Xuất toàn bộ dữ liệu trong GridView ra file Excel<br/>• Hiển thị SaveFileDialog để chọn vị trí lưu file<br/>• Tự động đặt tên file mặc định: Positions.xlsx<br/>• Hiển thị thông báo thành công sau khi xuất<br/><br/><b>Quy trình:</b><br/>• Kiểm tra có dữ liệu trong GridView không<br/>• Hiển thị SaveFileDialog với filter Excel Files (*.xlsx)<br/>• Nếu người dùng chọn vị trí lưu: Gọi GridView.ExportToXlsx()<br/>• Hiển thị thông báo thành công<br/><br/><b>Yêu cầu:</b><br/>• Phải có ít nhất 1 dòng dữ liệu trong GridView<br/>• Người dùng phải chọn vị trí lưu file<br/><br/><b>Kết quả:</b><br/>• File Excel được tạo tại vị trí đã chọn<br/>• File chứa toàn bộ dữ liệu hiển thị trong GridView<br/>• Hiển thị thông báo thành công<br/><br/><color=Gray>Lưu ý:</color> Nút này chỉ được kích hoạt khi có dữ liệu trong GridView. File Excel sẽ chứa toàn bộ dữ liệu đang hiển thị, bao gồm cả các cột đã được cấu hình trong GridView."
            );
        }

        #endregion
    }
}
