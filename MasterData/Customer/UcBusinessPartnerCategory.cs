using Bll.Common;
using Bll.MasterData.Customer;
using Bll.Utils;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraTreeList;
using MasterData.Converters;
using MasterData.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevExpress.XtraTreeList.Nodes;

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

            // TreeList events
            treeList1.SelectionChanged += TreeList1_SelectionChanged;
            treeList1.AfterCheckNode += TreeList1_AfterCheckNode;
            treeList1.CustomDrawNodeIndicator += TreeList1_CustomDrawNodeIndicator;
            treeList1.CustomDrawNodeCell += TreeList1_CustomDrawNodeCell;

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
        /// TreeList selection thay đổi -> cập nhật danh sách Id đã chọn và trạng thái nút.
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
        /// Event handler khi checkbox của node thay đổi.
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
        /// Cập nhật danh sách selected category IDs.
        /// </summary>
        private void UpdateSelectedCategoryIds()
        {
            _selectedCategoryIds.Clear();
            
            System.Diagnostics.Debug.WriteLine("=== UpdateSelectedCategoryIds ===");
            System.Diagnostics.Debug.WriteLine($"Total nodes in TreeList: {treeList1.Nodes.Count}");
            
            // Lấy tất cả nodes đã được chọn (bao gồm cả checkbox selection)
            foreach (TreeListNode node in treeList1.Nodes)
            {
                System.Diagnostics.Debug.WriteLine($"Checking root node: {node.GetDisplayText("CategoryName")}, Checked: {node.Checked}");
                CheckNodeRecursive(node);
            }
            
            System.Diagnostics.Debug.WriteLine($"Final selected IDs: {string.Join(", ", _selectedCategoryIds)}");
        }

        /// <summary>
        /// Kiểm tra node và các child nodes một cách đệ quy.
        /// </summary>
        private void CheckNodeRecursive(TreeListNode node)
        {
            string nodeName = node.GetDisplayText("CategoryName");
            bool isChecked = node.Checked;
            bool isSelected = treeList1.Selection.Contains(node);
            
            System.Diagnostics.Debug.WriteLine($"  Checking node: {nodeName}, Checked: {isChecked}, Selected: {isSelected}");
            
            if (isChecked || isSelected)
            {
                // Lấy dữ liệu từ binding source dựa trên node
                var index = treeList1.GetVisibleIndexByNode(node);
                System.Diagnostics.Debug.WriteLine($"    Node index: {index}, BindingSource count: {businessPartnerCategoryDtoBindingSource.Count}");
                
                if (index >= 0 && businessPartnerCategoryDtoBindingSource.Count > index)
                {
                    if (businessPartnerCategoryDtoBindingSource[index] is BusinessPartnerCategoryDto dto)
                    {
                        if (!_selectedCategoryIds.Contains(dto.Id))
                        {
                            _selectedCategoryIds.Add(dto.Id);
                            System.Diagnostics.Debug.WriteLine($"    Added ID: {dto.Id} for {dto.CategoryName}");
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
        /// Vẽ STT dòng cho TreeList.
        /// </summary>
        private void TreeList1_CustomDrawNodeIndicator(object sender, DevExpress.XtraTreeList.CustomDrawNodeIndicatorEventArgs e)
        {
            // Vẽ số thứ tự dòng cho TreeList
            if (e.Node != null)
            {
                var index = treeList1.GetVisibleIndexByNode(e.Node);
                if (index >= 0)
                {
                    // Vẽ số thứ tự vào indicator
                    e.Cache.DrawString((index + 1).ToString(), e.Appearance.Font, 
                        e.Appearance.GetForeBrush(e.Cache), e.Bounds, 
                        System.Drawing.StringFormat.GenericDefault);
                    e.Handled = true;
                }
            }
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
        /// Cấu hình TreeList để hiển thị dữ liệu xuống dòng (word wrap) cho các cột văn bản dài.
        /// </summary>
        private void ConfigureMultiLineGridView()
        {
            try
            {
                // Cấu hình TreeList để hiển thị hierarchical
                //treeList1.OptionsView.ShowIndicator = true;
                //treeList1.OptionsView.AutoWidth = false;
                //treeList1.OptionsView.ShowHorzLines = true;
                //treeList1.OptionsView.ShowVertLines = true;
                //treeList1.OptionsView.EnableAppearanceEvenRow = true;
                
                // Cấu hình TreeList để hiển thị dạng cây
                //treeList1.OptionsView.ShowTreeLines = DefaultBoolean.True;
                treeList1.OptionsView.ShowButtons = true;
                treeList1.OptionsView.ShowRoot = true;

                // RepositoryItemMemoEdit cho wrap text
                var memo = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit
                {
                    WordWrap = true,
                    AutoHeight = false
                };
                memo.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;

                // Áp dụng cho các cột có khả năng dài
                ApplyMemoEditorToColumn("CategoryName", memo);
                ApplyMemoEditorToColumn("Description", memo);

                // Tùy chọn hiển thị: căn giữa tiêu đề cho đẹp
                treeList1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                treeList1.Appearance.HeaderPanel.Options.UseTextOptions = true;
                
                // Cấu hình màu sắc cho các level khác nhau
                //treeList1.Appearance.Row.BackColor = System.Drawing.Color.White;
                //treeList1.Appearance.EvenRow.BackColor = System.Drawing.Color.FromArgb(248, 248, 248);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void ApplyMemoEditorToColumn(string fieldName, DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit memo)
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
        /// Tô màu/định dạng cell theo số lượng đối tác.
        /// </summary>
        private void TreeList1_CustomDrawNodeCell(object sender, DevExpress.XtraTreeList.CustomDrawNodeCellEventArgs e)
        {
            try
            {
                var treeList = sender as DevExpress.XtraTreeList.TreeList;
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
                System.Drawing.Color backColor;
                
                // Màu sắc dựa trên level trong cây
                if (row.Level == 0) // Root categories
                {
                    backColor = row.PartnerCount == 0 ? 
                        System.Drawing.Color.LightGray : 
                        System.Drawing.Color.LightBlue;
                }
                else // Sub-categories
                {
                    if (row.PartnerCount == 0)
                    {
                        backColor = System.Drawing.Color.FromArgb(245, 245, 245); // Very light gray
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
                        backColor = System.Drawing.Color.LightCyan; // Nhiều đối tác
                    }
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

            // Debug: Kiểm tra danh sách selected IDs
            System.Diagnostics.Debug.WriteLine($"Selected Category IDs: {string.Join(", ", _selectedCategoryIds)}");

            var confirmMessage = _selectedCategoryIds.Count == 1
                ? "Bạn có chắc muốn xóa dòng dữ liệu đã chọn?"
                : $"Bạn có chắc muốn xóa {_selectedCategoryIds.Count} dòng dữ liệu đã chọn?";

            if (!MsgBox.GetConfirmFromYesNoDialog(confirmMessage)) return;

            try
            {
                await ExecuteWithWaitingFormAsync(async () =>
                {
                    // Xóa theo thứ tự: con trước, cha sau để tránh lỗi foreign key constraint
                    await DeleteCategoriesInOrder(_selectedCategoryIds.ToList());
                    //// Gọi LoadDataAsyncWithoutSplash để tránh xung đột WaitingForm1
                    //await LoadDataAsyncWithoutSplash();
                    //ClearSelectionState();
                });
                
                ListDataBarButtonItem.PerformClick();
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
            var rowCount = treeList1.VisibleNodesCount;
            if (rowCount <= 0)
            {
                ShowInfo("Không có dữ liệu để xuất.");
                return;
            }

            // Export TreeList data
            try
            {
                var saveDialog = new System.Windows.Forms.SaveFileDialog
                {
                    Filter = "Excel Files (*.xlsx)|*.xlsx|All Files (*.*)|*.*",
                    FileName = "BusinessPartnerCategories.xlsx"
                };

                if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
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
                
                // Debug: Kiểm tra dữ liệu counts
                System.Diagnostics.Debug.WriteLine("=== LoadDataAsyncWithoutSplash Debug ===");
                System.Diagnostics.Debug.WriteLine($"Total categories: {categories.Count}");
                System.Diagnostics.Debug.WriteLine($"Total counts: {counts.Count}");
                
                foreach (var count in counts)
                {
                    var category = categories.FirstOrDefault(c => c.Id == count.Key);
                    System.Diagnostics.Debug.WriteLine($"Category: {category?.CategoryName ?? "Unknown"}, Count: {count.Value}");
                }
                
                // Tạo cấu trúc cây hierarchical
                var dtoList = categories.ToDtosWithHierarchy(counts).ToList();
                
                // Debug: Kiểm tra DTOs
                foreach (var dto in dtoList)
                {
                    System.Diagnostics.Debug.WriteLine($"DTO: {dto.CategoryName}, Level: {dto.Level}, PartnerCount: {dto.PartnerCount}");
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
        /// Bind danh sách DTO vào Grid và cấu hình hiển thị.
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
                    System.Diagnostics.Debug.WriteLine($"Lỗi xóa category {item.Category.CategoryName}: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Tính level của category trong cây phân cấp.
        /// </summary>
        private int CalculateCategoryLevel(Dal.DataContext.BusinessPartnerCategory category, 
            Dictionary<Guid, Dal.DataContext.BusinessPartnerCategory> categoryDict)
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
                System.Diagnostics.Debug.WriteLine($"Error clearing checkboxes: {ex.Message}");
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
