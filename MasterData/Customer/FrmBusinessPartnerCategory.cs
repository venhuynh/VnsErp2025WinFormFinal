using Bll.MasterData.CustomerBll;
using Common.Common;
using Common.Utils;
using Dal.DataContext;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraGrid.Views.Grid;
using DTO.MasterData.CustomerPartner;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MasterData.Customer
{
    /// <summary>
    /// User Control quản lý danh sách danh mục đối tác.
    /// Cung cấp giao diện hiển thị dạng cây, tìm kiếm, thêm mới, sửa, xóa và xuất dữ liệu danh mục.
    /// </summary>
    public partial class FrmBusinessPartnerCategory : XtraForm
    {
        #region ========== KHAI BÁO BIẾN ==========

        /// <summary>
        /// Business Logic Layer cho danh mục đối tác
        /// </summary>
        private readonly BusinessPartnerCategoryBll _businessPartnerCategoryBll = new BusinessPartnerCategoryBll();

        /// <summary>
        /// Logger cho logging
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Danh sách ID danh mục được chọn
        /// </summary>
        private readonly List<Guid> _selectedCategoryIds = [];

        /// <summary>
        /// Trạng thái đang tải dữ liệu (guard tránh gọi LoadDataAsync song song)
        /// </summary>
        private bool _isLoading;

        #endregion

        #region ========== CONSTRUCTOR & PUBLIC METHODS ==========

        /// <summary>
        /// Khởi tạo User Control quản lý danh sách danh mục đối tác.
        /// </summary>
        public FrmBusinessPartnerCategory()
        {
            InitializeComponent();

            // Khởi tạo logger
            _logger = LoggerFactory.CreateLogger(LogCategory.UI);

            // Toolbar events
            ListDataBarButtonItem.ItemClick += ListDataBarButtonItem_ItemClick;
            NewBarButtonItem.ItemClick += NewBarButtonItem_ItemClick;
            EditBarButtonItem.ItemClick += EditBarButtonItem_ItemClick;
            DeleteBarButtonItem.ItemClick += DeleteBarButtonItem_ItemClick;
            ExportBarButtonItem.ItemClick += ExportBarButtonItem_ItemClick;

            // GridView events
            BusinessPartnerCategoryDtoGridView.SelectionChanged += BusinessPartnerCategoryDtoGridView_SelectionChanged;
            BusinessPartnerCategoryDtoGridView.CustomDrawRowIndicator += BusinessPartnerCategoryDtoGridView_CustomDrawRowIndicator;

            UpdateButtonStates();

            // Setup SuperToolTips
            SetupSuperToolTips();
        }

        #endregion

        #region ========== QUẢN LÝ DỮ LIỆU ==========

        /// <summary>
        /// Tải dữ liệu và bind vào TreeList (Async, hiển thị WaitForm).
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
        /// Tải dữ liệu và bind vào TreeList (Async, không hiển thị WaitForm).
        /// </summary>
        private async Task LoadDataAsyncWithoutSplash()
        {
            try
            {
                var (categories, counts) = 
                    await _businessPartnerCategoryBll.GetCategoriesWithCountsAsync();

                // Log: Kiểm tra dữ liệu counts
                _logger.Debug("=== LoadDataAsyncWithoutSplash Debug ===");
                _logger.Debug("Total categories: {0}", categories.Count);
                _logger.Debug("Total counts: {0}", counts.Count);

                foreach (var count in counts)
                {
                    var category = categories.FirstOrDefault(c => c.Id == count.Key);
                    _logger.Debug("Category: {0}, Count: {1}", category?.CategoryName ?? "Unknown", count.Value);
                }

                // Tạo cấu trúc cây hierarchical
                var dtoList = categories.ToDtosWithHierarchy(counts).ToList();

                // Log: Kiểm tra DTOs
                foreach (var dto in dtoList)
                {
                    _logger.Debug("DTO: {0}, Level: {1}, PartnerCount: {2}", dto.CategoryName, dto.Level, dto.PartnerCount);
                }

                BindGrid(dtoList);
                // UpdateButtonStates() sẽ được gọi trong BindGrid -> ClearSelectionState()
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi tải dữ liệu");
            }
        }

        /// <summary>
        /// Bind danh sách DTO vào GridView và cấu hình hiển thị.
        /// </summary>
        private void BindGrid(List<BusinessPartnerCategoryDto> data)
        {
            // Clear selection trước khi bind data mới
            ClearSelectionState();

            businessPartnerCategoryDtoBindingSource.DataSource = data;
            BusinessPartnerCategoryDtoGridView.BestFitColumns();
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
                    using (var form = new FrmBusinessPartnerCategoryDetail(Guid.Empty))
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
                var focusedRowHandle = BusinessPartnerCategoryDtoGridView.FocusedRowHandle;
                BusinessPartnerCategoryDto dto = null;

                if (focusedRowHandle >= 0)
                {
                    // Lấy dữ liệu từ focused row
                    dto = BusinessPartnerCategoryDtoGridView.GetRow(focusedRowHandle) as BusinessPartnerCategoryDto;
                }

                if (dto == null || dto.Id != id)
                {
                    // Tìm đúng DTO theo Id trong datasource nếu FocusedRow không khớp selection
                    if (businessPartnerCategoryDtoBindingSource.DataSource is IEnumerable list)
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
                if (_selectedCategoryIds == null || _selectedCategoryIds.Count == 0)
                {
                    ShowInfo("Vui lòng chọn ít nhất một dòng để xóa.");
                    return;
                }

                // Log: Kiểm tra danh sách selected IDs
                _logger.Debug("Selected Category IDs: {0}", string.Join(", ", _selectedCategoryIds));

                var confirmMessage = _selectedCategoryIds.Count == 1
                    ? "Bạn có chắc muốn xóa dòng dữ liệu đã chọn?"
                    : $"Bạn có chắc muốn xóa {_selectedCategoryIds.Count} dòng dữ liệu đã chọn?";

                if (!MsgBox.ShowYesNo(confirmMessage)) return;

                try
                {
                    await ExecuteWithWaitingFormAsync(async () =>
                    {
                        // Xóa theo thứ tự: con trước, cha sau để tránh lỗi foreign key constraint
                        await DeleteCategoriesInOrder(_selectedCategoryIds.ToList());
                    });

                    ListDataBarButtonItem.PerformClick();
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
            var rowCount = BusinessPartnerCategoryDtoGridView.RowCount;
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
                    FileName = "BusinessPartnerCategories.xlsx"
                };

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    BusinessPartnerCategoryDtoGridView.ExportToXlsx(saveDialog.FileName);
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
        private void BusinessPartnerCategoryDtoGridView_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                // Cập nhật danh sách selected IDs khi selection thay đổi
                UpdateSelectedCategoryIds();
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
        private void BusinessPartnerCategoryDtoGridView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
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
                // Cấu hình sắp xếp mặc định theo SortOrder, sau đó CategoryName
                if (BusinessPartnerCategoryDtoGridView.Columns["colSortOrder"] != null)
                {
                    BusinessPartnerCategoryDtoGridView.Columns["colSortOrder"].SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
                }
                else if (BusinessPartnerCategoryDtoGridView.Columns["colCategoryName"] != null)
                {
                    BusinessPartnerCategoryDtoGridView.Columns["colCategoryName"].SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// Xóa các danh mục theo thứ tự: con trước, cha sau để tránh lỗi foreign key constraint.
        /// </summary>
        private async Task DeleteCategoriesInOrder(List<Guid> categoryIds)
        {
            if (categoryIds == null || categoryIds.Count == 0) return;

            // Lấy tất cả categories để xác định thứ tự xóa
            var allCategories = await _businessPartnerCategoryBll.GetAllAsync();
            var categoryDict = allCategories.ToDictionary(c => c.Id);

            // Tạo danh sách categories cần xóa với thông tin level
            var categoriesToDelete = categoryIds.Select(id =>
            {
                var category = categoryDict.TryGetValue(id, out var value) ? value : null;
                if (category == null) return null;

                // Tính level để xác định thứ tự xóa (level cao hơn = xóa trước)
                var level = CalculateCategoryLevel(category, categoryDict);
                return new { Category = category, Level = level };
            }).Where(x => x != null).OrderByDescending(x => x.Level).ToList();

            // Xóa theo thứ tự từ level cao xuống level thấp
            foreach (var item in categoriesToDelete)
            {
                try
                {
                    _businessPartnerCategoryBll.Delete(item.Category.Id);
                }
                catch (Exception ex)
                {
                    // Log lỗi nhưng tiếp tục xóa các item khác
                    _logger.Error("Lỗi xóa category {0}: {1}", item.Category.CategoryName, ex.Message);
                }
            }
        }

        /// <summary>
        /// Tính level của category trong cây phân cấp.
        /// </summary>
        private int CalculateCategoryLevel(BusinessPartnerCategory category,
            Dictionary<Guid, BusinessPartnerCategory> categoryDict)
        {
            int level = 0;
            var current = category;
            while (current.ParentId.HasValue && categoryDict.ContainsKey(current.ParentId.Value))
            {
                level++;
                current = categoryDict[current.ParentId.Value];
                if (level > 10) break; // Tránh infinite loop
            }

            return level;
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
                var selectedCount = _selectedCategoryIds?.Count ?? 0;
                // Edit: chỉ khi chọn đúng 1 dòng
                if (EditBarButtonItem != null)
                    EditBarButtonItem.Enabled = selectedCount == 1;
                // Delete: khi chọn >= 1 dòng
                if (DeleteBarButtonItem != null)
                    DeleteBarButtonItem.Enabled = selectedCount >= 1;
                // Export: chỉ khi có dữ liệu hiển thị
                var rowCount = BusinessPartnerCategoryDtoGridView.RowCount;
                if (ExportBarButtonItem != null)
                    ExportBarButtonItem.Enabled = rowCount > 0;
            }
            catch
            {
                // ignore
            }
        }

        /// <summary>
        /// Cập nhật danh sách selected category IDs.
        /// </summary>
        private void UpdateSelectedCategoryIds()
        {
            _selectedCategoryIds.Clear();

            _logger.Debug("=== UpdateSelectedCategoryIds ===");
            _logger.Debug("Total rows in GridView: {0}", BusinessPartnerCategoryDtoGridView.RowCount);

            // Lấy tất cả rows đã được chọn
            var selectedRows = BusinessPartnerCategoryDtoGridView.GetSelectedRows();
            foreach (int rowHandle in selectedRows)
            {
                if (rowHandle >= 0)
                {
                    var dto = BusinessPartnerCategoryDtoGridView.GetRow(rowHandle) as BusinessPartnerCategoryDto;
                    if (dto != null && !_selectedCategoryIds.Contains(dto.Id))
                    {
                        _selectedCategoryIds.Add(dto.Id);
                        _logger.Debug("    Added ID: {0} for {1}", dto.Id, dto.CategoryName);
                    }
                }
            }

            _logger.Debug("Final selected IDs: {0}", string.Join(", ", _selectedCategoryIds));
        }

        /// <summary>
        /// Xóa trạng thái chọn hiện tại trên GridView.
        /// </summary>
        private void ClearSelectionState()
        {
            _selectedCategoryIds.Clear();

            // Clear tất cả selection
            BusinessPartnerCategoryDtoGridView.ClearSelection();
            BusinessPartnerCategoryDtoGridView.FocusedRowHandle = DevExpress.XtraGrid.GridControl.InvalidRowHandle;

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
                        content: "Tải lại danh sách danh mục đối tác từ hệ thống."
                    );
                }

                if (NewBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        NewBarButtonItem,
                        title: "<b><color=Green>➕ Thêm mới</color></b>",
                        content: "Thêm mới danh mục đối tác vào hệ thống."
                    );
                }

                if (EditBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        EditBarButtonItem,
                        title: "<b><color=Orange>✏️ Sửa</color></b>",
                        content: "Chỉnh sửa thông tin danh mục đối tác đã chọn."
                    );
                }

                if (DeleteBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        DeleteBarButtonItem,
                        title: "<b><color=Red>🗑️ Xóa</color></b>",
                        content: "Xóa các danh mục đối tác đã chọn khỏi hệ thống."
                    );
                }

                if (ExportBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        ExportBarButtonItem,
                        title: "<b><color=Purple>📊 Xuất Excel</color></b>",
                        content: "Xuất danh sách danh mục đối tác ra file Excel."
                    );
                }
            }
            catch (Exception ex)
            {
                // Ignore lỗi setup SuperToolTip để không chặn UserControl
                _logger.Warning("Lỗi setup SuperToolTip: {0}", ex.Message);
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
