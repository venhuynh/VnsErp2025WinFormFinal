using Bll.MasterData.CustomerBll;
using Common.Helpers;
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
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common.Common;
using Common.Utils;

namespace MasterData.Customer
{
    /// <summary>
    /// User Control quản lý danh sách đối tác.
    /// Cung cấp giao diện hiển thị, tìm kiếm, thêm mới, sửa, xóa và xuất dữ liệu đối tác.
    /// </summary>
    public partial class FrmBusinessPartnerList : XtraForm
    {
        #region ========== KHAI BÁO BIẾN ==========

        /// <summary>
        /// Business Logic Layer cho đối tác
        /// </summary>
        private readonly BusinessPartnerBll _businessPartnerBll = new BusinessPartnerBll();

        /// <summary>
        /// Danh sách ID đối tác được chọn
        /// </summary>
        private List<Guid> _selectedPartnerIds = new List<Guid>();

        /// <summary>
        /// Trạng thái đang tải dữ liệu (guard tránh gọi LoadDataAsync song song)
        /// </summary>
        private bool _isLoading;

        /// <summary>
        /// RowHandle đang được edit (để lấy PartnerId khi upload logo)
        /// </summary>
        private int _editingRowHandle = GridControl.InvalidRowHandle;

        #endregion

        #region ========== CONSTRUCTOR & PUBLIC METHODS ==========

        /// <summary>
        /// Khởi tạo User Control quản lý danh sách đối tác.
        /// </summary>
        public FrmBusinessPartnerList()
        {
            InitializeComponent();

            // Toolbar events
            ListDataBarButtonItem.ItemClick += ListDataBarButtonItem_ItemClick;
            NewBarButtonItem.ItemClick += NewBarButtonItem_ItemClick;
            EditBarButtonItem.ItemClick += EditBarButtonItem_ItemClick;
            DeleteBarButtonItem.ItemClick += DeleteBarButtonItem_ItemClick;
            ExportBarButtonItem.ItemClick += ExportBarButtonItem_ItemClick;

            // Grid events
            BusinessPartnerListGridView.SelectionChanged += BusinessPartnerListGridView_SelectionChanged;
            BusinessPartnerListGridView.CustomDrawRowIndicator += BusinessPartnerListGridView_CustomDrawRowIndicator;
            BusinessPartnerListGridView.RowCellStyle += BusinessPartnerListGridView_RowCellStyle;
            BusinessPartnerListGridView.ShownEditor += BusinessPartnerListGridView_ShownEditor;
            BusinessPartnerListGridView.HiddenEditor += BusinessPartnerListGridView_HiddenEditor;

            // RepositoryItemPictureEdit events
            PartnerLogoRepositoryItemPictureEdit.ImageChanged += PartnerLogoRepositoryItemPictureEdit_ImageChanged;

            UpdateButtonStates();

            // Setup SuperToolTips
            SetupSuperToolTips();

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
                System.Diagnostics.Debug.WriteLine("[Form] LoadDataAsyncWithoutSplash: Bắt đầu gọi BLL.GetAllAsync()");
                var entities = await _businessPartnerBll.GetAllAsync();
                System.Diagnostics.Debug.WriteLine($"[Form] LoadDataAsyncWithoutSplash: Đã nhận được {entities?.Count ?? 0} entities từ BLL");
                
                System.Diagnostics.Debug.WriteLine("[Form] LoadDataAsyncWithoutSplash: Bắt đầu lấy categoryDict");
                var categoryDict = await _businessPartnerBll.GetCategoryDictAsync();
                System.Diagnostics.Debug.WriteLine($"[Form] LoadDataAsyncWithoutSplash: Đã lấy được {categoryDict?.Count ?? 0} categories");
                
                System.Diagnostics.Debug.WriteLine("[Form] LoadDataAsyncWithoutSplash: Bắt đầu convert sang DTO với categoryDict");
                var dtoList = entities.ToBusinessPartnerListDtos(categoryDict).ToList();
                System.Diagnostics.Debug.WriteLine($"[Form] LoadDataAsyncWithoutSplash: Đã convert được {dtoList.Count} DTOs");
                
                System.Diagnostics.Debug.WriteLine("[Form] LoadDataAsyncWithoutSplash: Bắt đầu bind vào grid");
                BindGrid(dtoList);
                System.Diagnostics.Debug.WriteLine("[Form] LoadDataAsyncWithoutSplash: Hoàn thành");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[Form] LoadDataAsyncWithoutSplash: LỖI: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"[Form] LoadDataAsyncWithoutSplash: StackTrace: {ex.StackTrace}");
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
                    using (var form = new FrmBusinessPartnerDetail(Guid.Empty))
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
                if (businessPartnerListDtoBindingSource.DataSource is IEnumerable list)
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

            try
            {
                using (OverlayManager.ShowScope(this))
                {
                    using (var form = new FrmBusinessPartnerDetail(dto.Id))
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
            if (_selectedPartnerIds == null || _selectedPartnerIds.Count == 0)
            {
                ShowInfo("Vui lòng chọn ít nhất một dòng để xóa.");
                return;
            }

            var confirmMessage = _selectedPartnerIds.Count == 1
                ? "Bạn có chắc muốn xóa dòng dữ liệu đã chọn?"
                : $"Bạn có chắc muốn xóa {_selectedPartnerIds.Count} dòng dữ liệu đã chọn?";

            if (!MsgBox.ShowYesNo(confirmMessage)) return;

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
        /// Xử lý sự kiện click button Xuất dữ liệu
        /// </summary>
        private void ExportBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
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

        /// <summary>
        /// Xử lý sự kiện thay đổi selection trên GridView
        /// </summary>
        private void BusinessPartnerListGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
        /// Xử lý sự kiện vẽ số thứ tự dòng
        /// </summary>
        private void BusinessPartnerListGridView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            // Sử dụng helper chung để vẽ số thứ tự dòng
            GridViewHelper.CustomDrawRowIndicator(BusinessPartnerListGridView, e);
        }

        /// <summary>
        /// Xử lý sự kiện tô màu dòng theo trạng thái/loại đối tác
        /// </summary>
        private void BusinessPartnerListGridView_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            try
            {
                if (sender is not GridView view) return;
                if (e.RowHandle < 0) return;
                if (view.GetRow(e.RowHandle) is not BusinessPartnerListDto row) return;
                // Không ghi đè màu khi đang chọn để giữ màu chọn mặc định của DevExpress
                if (view.IsRowSelected(e.RowHandle)) return;
                
                
                // Nếu đối tác không hoạt động: làm nổi bật rõ ràng hơn
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

        /// <summary>
        /// Áp dụng RepositoryItemMemoEdit cho cột cụ thể
        /// </summary>
        /// <param name="fieldName">Tên field của cột</param>
        /// <param name="memo">RepositoryItemMemoEdit</param>
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
        /// Xóa trạng thái chọn hiện tại trên Grid.
        /// </summary>
        private void ClearSelectionState()
        {
            _selectedPartnerIds.Clear();
            BusinessPartnerListGridView.ClearSelection();
            BusinessPartnerListGridView.FocusedRowHandle = GridControl.InvalidRowHandle;
            UpdateButtonStates();
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
                        content: "Tải lại danh sách đối tác từ hệ thống."
                    );
                }

                if (NewBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        NewBarButtonItem,
                        title: "<b><color=Green>➕ Thêm mới</color></b>",
                        content: "Thêm mới đối tác vào hệ thống."
                    );
                }

                if (EditBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        EditBarButtonItem,
                        title: "<b><color=Orange>✏️ Sửa</color></b>",
                        content: "Chỉnh sửa thông tin đối tác đã chọn."
                    );
                }

                if (DeleteBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        DeleteBarButtonItem,
                        title: "<b><color=Red>🗑️ Xóa</color></b>",
                        content: "Xóa các đối tác đã chọn khỏi hệ thống."
                    );
                }

                if (ExportBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        ExportBarButtonItem,
                        title: "<b><color=Purple>📊 Xuất Excel</color></b>",
                        content: "Xuất danh sách đối tác ra file Excel."
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
