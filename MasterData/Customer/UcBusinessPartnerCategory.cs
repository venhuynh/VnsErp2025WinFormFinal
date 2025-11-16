using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bll.Common;
using Bll.MasterData.Customer;
using Bll.Utils;
using Dal.DataContext;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using MasterData.Customer.Converters;
using MasterData.Customer.Dto;

namespace MasterData.Customer
{
    /// <summary>
    /// User Control quản lý danh sách danh mục đối tác.
    /// Cung cấp giao diện hiển thị dạng cây, tìm kiếm, thêm mới, sửa, xóa và xuất dữ liệu danh mục.
    /// </summary>
    public partial class UcBusinessPartnerCategory : XtraUserControl
    {
        #region ========== KHAI BÁO BIẾN ==========

        /// <summary>
        /// Business Logic Layer cho danh mục đối tác
        /// </summary>
        private readonly BusinessPartnerCategoryBll _businessPartnerCategoryBll = new BusinessPartnerCategoryBll();

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
        public UcBusinessPartnerCategory()
        {
            InitializeComponent();

            // Toolbar events
            ListDataBarButtonItem.ItemClick += ListDataBarButtonItem_ItemClick;
            NewBarButtonItem.ItemClick += NewBarButtonItem_ItemClick;
            EditBarButtonItem.ItemClick += EditBarButtonItem_ItemClick;
            DeleteBarButtonItem.ItemClick += DeleteBarButtonItem_ItemClick;
            ExportBarButtonItem.ItemClick += ExportBarButtonItem_ItemClick;

            // TreeList events
            treeList1.SelectionChanged += TreeList1_SelectionChanged;
            treeList1.AfterCheckNode += TreeList1_AfterCheckNode;
            treeList1.CustomDrawNodeIndicator += TreeList1_CustomDrawNodeIndicator;
            treeList1.CustomDrawNodeCell += TreeList1_CustomDrawNodeCell;

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
                var (categories, counts) = await _businessPartnerCategoryBll.GetCategoriesWithCountsAsync();

                // Debug: Kiểm tra dữ liệu counts
                Debug.WriteLine("=== LoadDataAsyncWithoutSplash Debug ===");
                Debug.WriteLine($"Total categories: {categories.Count}");
                Debug.WriteLine($"Total counts: {counts.Count}");

                foreach (var count in counts)
                {
                    var category = categories.FirstOrDefault(c => c.Id == count.Key);
                    Debug.WriteLine($"Category: {category?.CategoryName ?? "Unknown"}, Count: {count.Value}");
                }

                // Tạo cấu trúc cây hierarchical
                var dtoList = categories.ToDtosWithHierarchy(counts).ToList();

                // Debug: Kiểm tra DTOs
                foreach (var dto in dtoList)
                {
                    Debug.WriteLine($"DTO: {dto.CategoryName}, Level: {dto.Level}, PartnerCount: {dto.PartnerCount}");
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
        /// Bind danh sách DTO vào TreeList và cấu hình hiển thị.
        /// </summary>
        private void BindGrid(List<BusinessPartnerCategoryDto> data)
        {
            // Clear selection trước khi bind data mới
            ClearSelectionState();

            businessPartnerCategoryDtoBindingSource.DataSource = data;
            treeList1.BestFitColumns();
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
                var focusedNode = treeList1.FocusedNode;
                BusinessPartnerCategoryDto dto = null;

                if (focusedNode != null)
                {
                    // Lấy dữ liệu từ focused node
                    dto = businessPartnerCategoryDtoBindingSource.Current as BusinessPartnerCategoryDto;
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

                // Debug: Kiểm tra danh sách selected IDs
                Debug.WriteLine($"Selected Category IDs: {string.Join(", ", _selectedCategoryIds)}");

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
            var rowCount = treeList1.VisibleNodesCount;
            if (rowCount <= 0)
            {
                ShowInfo("Không có dữ liệu để xuất.");
                return;
            }

            // Export TreeList data
            try
            {
                var saveDialog = new SaveFileDialog
                {
                    Filter = @"Excel Files (*.xlsx)|*.xlsx|All Files (*.*)|*.*",
                    FileName = "BusinessPartnerCategories.xlsx"
                };

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    treeList1.ExportToXlsx(saveDialog.FileName);
                    ShowInfo("Xuất dữ liệu thành công!");
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi xuất dữ liệu");
            }
        }

        /// <summary>
        /// Xử lý sự kiện thay đổi selection trên TreeList
        /// </summary>
        private void TreeList1_SelectionChanged(object sender, EventArgs e)
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
        /// Xử lý sự kiện thay đổi checkbox trên TreeList
        /// </summary>
        private void TreeList1_AfterCheckNode(object sender, NodeEventArgs e)
        {
            try
            {
                // Cập nhật danh sách selected IDs khi checkbox thay đổi
                UpdateSelectedCategoryIds();
                UpdateButtonStates();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        /// <summary>
        /// Xử lý sự kiện vẽ số thứ tự dòng cho TreeList
        /// </summary>
        private void TreeList1_CustomDrawNodeIndicator(object sender, CustomDrawNodeIndicatorEventArgs e)
        {
            // Vẽ số thứ tự dòng cho TreeList
            if (e.Node == null) return;


            var index = treeList1.GetVisibleIndexByNode(e.Node);
            if (index >= 0)
            {
                // Vẽ số thứ tự vào indicator
                e.Cache.DrawString((index + 1).ToString(), e.Appearance.Font,
                    e.Appearance.GetForeBrush(e.Cache), e.Bounds,
                    StringFormat.GenericDefault);
                e.Handled = true;
            }
        }

        /// <summary>
        /// Xử lý sự kiện tô màu cell theo số lượng đối tác
        /// </summary>
        private void TreeList1_CustomDrawNodeCell(object sender, CustomDrawNodeCellEventArgs e)
        {
            try
            {
                var treeList = sender as TreeList;
                if (treeList == null) return;
                if (e.Node == null) return;

                // Lấy index từ node
                var index = treeList.GetVisibleIndexByNode(e.Node);
                if (index < 0 || index >= businessPartnerCategoryDtoBindingSource.Count) return;

                var row = businessPartnerCategoryDtoBindingSource[index] as BusinessPartnerCategoryDto;
                if (row == null) return;

                // Không ghi đè màu khi đang chọn để giữ màu chọn mặc định của DevExpress
                if (treeList.Selection.Contains(e.Node)) return;

                // Nền theo số lượng đối tác và level
                Color backColor;

                // Màu sắc dựa trên level trong cây
                if (row.Level == 0) // Root categories
                {
                    backColor = row.PartnerCount == 0 ? Color.LightGray : Color.LightBlue;
                }
                else // Sub-categories
                {
                    if (row.PartnerCount == 0)
                    {
                        backColor = Color.FromArgb(245, 245, 245); // Very light gray
                    }
                    else if (row.PartnerCount <= 5)
                    {
                        backColor = Color.LightYellow; // Ít đối tác
                    }
                    else if (row.PartnerCount <= 20)
                    {
                        backColor = Color.LightGreen; // Trung bình
                    }
                    else
                    {
                        backColor = Color.LightCyan; // Nhiều đối tác
                    }
                }

                e.Appearance.BackColor = backColor;
                e.Appearance.ForeColor = Color.Black;
                e.Appearance.Options.UseBackColor = true;
                e.Appearance.Options.UseForeColor = true;
            }
            catch (Exception)
            {
                // ignore style errors
            }
        }

        #endregion

        #region ========== XỬ LÝ DỮ LIỆU ==========

        /// <summary>
        /// Cấu hình TreeList để hiển thị dữ liệu xuống dòng (word wrap) cho các cột văn bản dài.
        /// </summary>
        private void ConfigureMultiLineGridView()
        {
            try
            {
                // Cấu hình TreeList để hiển thị dạng cây
                treeList1.OptionsView.ShowButtons = true;
                treeList1.OptionsView.ShowRoot = true;

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
                treeList1.Appearance.HeaderPanel.TextOptions.HAlignment = HorzAlignment.Center;
                treeList1.Appearance.HeaderPanel.Options.UseTextOptions = true;
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
            var col = treeList1.Columns[fieldName];
            if (col == null) return;
            // Thêm repository vào TreeList nếu chưa có
            if (!treeList1.RepositoryItems.Contains(memo))
            {
                treeList1.RepositoryItems.Add(memo);
            }

            col.ColumnEdit = memo;
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
                    Debug.WriteLine($"Lỗi xóa category {item.Category.CategoryName}: {ex.Message}");
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
                var rowCount = treeList1.VisibleNodesCount;
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

            Debug.WriteLine("=== UpdateSelectedCategoryIds ===");
            Debug.WriteLine($"Total nodes in TreeList: {treeList1.Nodes.Count}");

            // Lấy tất cả nodes đã được chọn (bao gồm cả checkbox selection)
            foreach (TreeListNode node in treeList1.Nodes)
            {
                Debug.WriteLine($"Checking root node: {node.GetDisplayText("CategoryName")}, Checked: {node.Checked}");
                CheckNodeRecursive(node);
            }

            Debug.WriteLine($"Final selected IDs: {string.Join(", ", _selectedCategoryIds)}");
        }

        /// <summary>
        /// Kiểm tra node và các child nodes một cách đệ quy.
        /// </summary>
        private void CheckNodeRecursive(TreeListNode node)
        {
            string nodeName = node.GetDisplayText("CategoryName");
            bool isChecked = node.Checked;
            bool isSelected = treeList1.Selection.Contains(node);

            Debug.WriteLine($"  Checking node: {nodeName}, Checked: {isChecked}, Selected: {isSelected}");

            if (isChecked || isSelected)
            {
                // Lấy dữ liệu từ binding source dựa trên node
                var index = treeList1.GetVisibleIndexByNode(node);
                Debug.WriteLine(
                    $"    Node index: {index}, BindingSource count: {businessPartnerCategoryDtoBindingSource.Count}");

                if (index >= 0 && businessPartnerCategoryDtoBindingSource.Count > index)
                {
                    if (businessPartnerCategoryDtoBindingSource[index] is BusinessPartnerCategoryDto dto)
                    {
                        if (!_selectedCategoryIds.Contains(dto.Id))
                        {
                            _selectedCategoryIds.Add(dto.Id);
                            Debug.WriteLine($"    Added ID: {dto.Id} for {dto.CategoryName}");
                        }
                    }
                }
            }

            // Kiểm tra các child nodes
            foreach (TreeListNode childNode in node.Nodes)
            {
                CheckNodeRecursive(childNode);
            }
        }

        /// <summary>
        /// Xóa trạng thái chọn hiện tại trên TreeList.
        /// </summary>
        private void ClearSelectionState()
        {
            _selectedCategoryIds.Clear();

            // Clear tất cả selection (cả checkbox và regular selection)
            treeList1.ClearSelection();
            treeList1.FocusedNode = null;

            // Clear tất cả checkbox states
            ClearAllCheckBoxes();

            UpdateButtonStates();
        }

        /// <summary>
        /// Clear tất cả checkbox states trong TreeList.
        /// </summary>
        private void ClearAllCheckBoxes()
        {
            try
            {
                // Disable events tạm thời để tránh trigger UpdateSelectedCategoryIds
                treeList1.AfterCheckNode -= TreeList1_AfterCheckNode;

                // Clear tất cả checkbox states
                foreach (TreeListNode node in treeList1.Nodes)
                {
                    ClearNodeCheckBoxes(node);
                }

                // Re-enable events
                treeList1.AfterCheckNode += TreeList1_AfterCheckNode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error clearing checkboxes: {ex.Message}");
            }
        }

        /// <summary>
        /// Clear checkbox của node và tất cả child nodes.
        /// </summary>
        private void ClearNodeCheckBoxes(TreeListNode node)
        {
            if (node != null)
            {
                node.Checked = false;

                // Clear child nodes recursively
                foreach (TreeListNode childNode in node.Nodes)
                {
                    ClearNodeCheckBoxes(childNode);
                }
            }
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
            MsgBox.ShowException(string.IsNullOrWhiteSpace(context)
                ? ex
                : new Exception(context + ": " + ex.Message, ex));
        }

        #endregion

    }
}
