using Bll.Common;
using Bll.MasterData.ProductService;
using Bll.Utils;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using MasterData.ProductService.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MasterData.ProductService.Dto;

namespace MasterData.ProductService
{
    /// <summary>
    /// Danh sách Danh mục Sản phẩm/Dịch vụ - chỉ giữ event handler, mọi xử lý tách thành phương thức riêng.
    /// </summary>
    public partial class UcProductServiceCategory : DevExpress.XtraEditors.XtraUserControl
    {
        #region Fields

        private readonly ProductServiceCategoryBll _productServiceCategoryBll = new ProductServiceCategoryBll();
        private List<Guid> _selectedCategoryIds = new List<Guid>();
        private bool _isLoading; // guard tránh gọi LoadDataAsync song song

        #endregion

        #region Constructor

        /// <summary>
        /// Khởi tạo control, đăng ký event UI.
        /// </summary>
        public UcProductServiceCategory()
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
                    using (var form = new FrmProductServiceCategoryDetail(Guid.Empty))
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
                System.Diagnostics.Debug.WriteLine($"    Node index: {index}, BindingSource count: {productServiceCategoryDtoBindingSource.Count}");
                
                if (index >= 0 && productServiceCategoryDtoBindingSource.Count > index)
                {
                    if (productServiceCategoryDtoBindingSource[index] is ProductServiceCategoryDto dto)
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
        private void TreeList1_CustomDrawNodeIndicator(object sender, CustomDrawNodeIndicatorEventArgs e)
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
                var rowCount = treeList1.VisibleNodesCount;
                
                System.Diagnostics.Debug.WriteLine($"=== UpdateButtonStates ===");
                System.Diagnostics.Debug.WriteLine($"SelectedCount: {selectedCount}, RowCount: {rowCount}");
                
                // Edit: chỉ khi chọn đúng 1 dòng
                if (EditBarButtonItem != null)
                {
                    EditBarButtonItem.Enabled = selectedCount == 1;
                    System.Diagnostics.Debug.WriteLine($"EditBarButtonItem.Enabled: {EditBarButtonItem.Enabled}");
                }
                
                // Delete: khi chọn >= 1 dòng
                if (DeleteBarButtonItem != null)
                {
                    DeleteBarButtonItem.Enabled = selectedCount >= 1;
                    System.Diagnostics.Debug.WriteLine($"DeleteBarButtonItem.Enabled: {DeleteBarButtonItem.Enabled}");
                }
                
                // Export: chỉ khi có dữ liệu hiển thị
                if (ExportBarButtonItem != null)
                {
                    ExportBarButtonItem.Enabled = rowCount > 0;
                    System.Diagnostics.Debug.WriteLine($"ExportBarButtonItem.Enabled: {ExportBarButtonItem.Enabled}");
                }
                
                System.Diagnostics.Debug.WriteLine("=== End UpdateButtonStates ===");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in UpdateButtonStates: {ex.Message}");
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
                treeList1.OptionsView.ShowButtons = true;
                treeList1.OptionsView.ShowRoot = true;
                treeList1.OptionsView.ShowHorzLines = true; // Hiển thị đường kẻ ngang
                treeList1.OptionsView.ShowVertLines = true; // Hiển thị đường kẻ dọc
                treeList1.OptionsView.EnableAppearanceEvenRow = true; // Bật màu xen kẽ cho dòng chẵn
                treeList1.OptionsView.EnableAppearanceOddRow = true; // Bật màu xen kẽ cho dòng lẻ

                // Cấu hình không cho edit
                treeList1.OptionsBehavior.Editable = false;
                treeList1.OptionsSelection.EnableAppearanceFocusedCell = false;

                // RepositoryItemMemoEdit cho wrap text
                var memo = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit
                {
                    WordWrap = true,
                    AutoHeight = false,
                    ReadOnly = true // Không cho edit
                };
                memo.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;

                // Áp dụng cho các cột có khả năng dài
                ApplyMemoEditorToColumn("CategoryName", memo);
                ApplyMemoEditorToColumn("Description", memo);

                // Tùy chọn hiển thị: căn giữa tiêu đề cho đẹp
                treeList1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                treeList1.Appearance.HeaderPanel.Options.UseTextOptions = true;

                // Cấu hình màu sắc cho dòng chẵn/lẻ
                treeList1.Appearance.EvenRow.BackColor = System.Drawing.Color.FromArgb(248, 248, 248); // Màu xám nhạt
                treeList1.Appearance.OddRow.BackColor = System.Drawing.Color.White; // Màu trắng

                // Cấu hình màu đường kẻ
                treeList1.Appearance.HorzLine.BackColor = System.Drawing.Color.FromArgb(200, 200, 200); // Màu xám nhạt
                treeList1.Appearance.VertLine.BackColor = System.Drawing.Color.FromArgb(200, 200, 200); // Màu xám nhạt

                // Cấu hình màu header
                treeList1.Appearance.HeaderPanel.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);
                treeList1.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.Black;
                treeList1.Appearance.HeaderPanel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
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
        /// Tô màu/định dạng cell theo số lượng sản phẩm/dịch vụ.
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
                if (index < 0 || index >= productServiceCategoryDtoBindingSource.Count) return;
                
                var row = productServiceCategoryDtoBindingSource[index] as ProductServiceCategoryDto;
                if (row == null) return;
                
                // Không ghi đè màu khi đang chọn để giữ màu chọn mặc định của DevExpress
                if (treeList.Selection.Contains(e.Node)) return;

                // Chỉ tô màu cho cột ProductCount để hiển thị trạng thái
                if (e.Column.FieldName == "ProductCount")
                {
                    System.Drawing.Color backColor;
                    System.Drawing.Color foreColor = System.Drawing.Color.Black;
                    
                    // Màu sắc dựa trên số lượng sản phẩm/dịch vụ
                    if (row.ProductCount == 0)
                    {
                        backColor = System.Drawing.Color.FromArgb(255, 240, 240); // Màu đỏ nhạt
                        foreColor = System.Drawing.Color.FromArgb(150, 0, 0); // Màu đỏ đậm
                    }
                    else if (row.ProductCount <= 5)
                    {
                        backColor = System.Drawing.Color.FromArgb(255, 255, 200); // Màu vàng nhạt
                        foreColor = System.Drawing.Color.FromArgb(150, 100, 0); // Màu cam đậm
                    }
                    else if (row.ProductCount <= 20)
                    {
                        backColor = System.Drawing.Color.FromArgb(240, 255, 240); // Màu xanh lá nhạt
                        foreColor = System.Drawing.Color.FromArgb(0, 100, 0); // Màu xanh lá đậm
                    }
                    else
                    {
                        backColor = System.Drawing.Color.FromArgb(240, 248, 255); // Màu xanh dương nhạt
                        foreColor = System.Drawing.Color.FromArgb(0, 0, 150); // Màu xanh dương đậm
                    }

                    e.Appearance.BackColor = backColor;
                    e.Appearance.ForeColor = foreColor;
                    e.Appearance.Options.UseBackColor = true;
                    e.Appearance.Options.UseForeColor = true;
                }
                else
                {
                    // Cho các cột khác, sử dụng màu mặc định (trắng/xám xen kẽ)
                    // Không cần set màu gì, để DevExpress tự động xử lý
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
            ProductServiceCategoryDto dto = null;
            
            if (focusedNode != null)
            {
                // Lấy dữ liệu từ focused node
                dto = productServiceCategoryDtoBindingSource.Current as ProductServiceCategoryDto;
            }
            
            if (dto == null || dto.Id != id)
            {
                // Tìm đúng DTO theo Id trong datasource nếu FocusedRow không khớp selection
                if (productServiceCategoryDtoBindingSource.DataSource is System.Collections.IEnumerable list)
                {
                    foreach (var item in list)
                    {
                        if (item is ProductServiceCategoryDto x && x.Id == id)
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
                    using (var form = new FrmProductServiceCategoryDetail(dto.Id))
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
                ? "Bạn có chắc muốn xóa dòng dữ liệu đã chọn? (Sản phẩm/dịch vụ sẽ được chuyển sang 'Phân loại chưa đặt tên')"
                : $"Bạn có chắc muốn xóa {_selectedCategoryIds.Count} dòng dữ liệu đã chọn? (Sản phẩm/dịch vụ sẽ được chuyển sang 'Phân loại chưa đặt tên')";

            if (!MsgBox.GetConfirmFromYesNoDialog(confirmMessage)) return;

            try
            {
                await ExecuteWithWaitingFormAsync(async () =>
                {
                    // Xóa theo thứ tự: con trước, cha sau để tránh lỗi foreign key constraint
                    await DeleteCategoriesWithProductMigration(_selectedCategoryIds.ToList());
                });
                
                // Clear selection state trước khi reload
                ClearSelectionState();
                
                // Reload dữ liệu
                await LoadDataAsync();
                UpdateButtonStates();
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
                    FileName = "ProductServiceCategories.xlsx"
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
                var (categories, counts) = await _productServiceCategoryBll.GetCategoriesWithCountsAsync();
                
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
                    System.Diagnostics.Debug.WriteLine($"DTO: {dto.CategoryName}, Level: {dto.Level}, ProductCount: {dto.ProductCount}");
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
        private void BindGrid(List<ProductServiceCategoryDto> data)
        {
            // Clear selection trước khi bind data mới
            ClearSelectionState();
            
            productServiceCategoryDtoBindingSource.DataSource = data;
            treeList1.BestFitColumns();
            ConfigureMultiLineGridView();
            
            // Đảm bảo selection được clear sau khi bind và update button states
            ClearSelectionState();
        }

        /// <summary>
        /// Xóa các danh mục với logic di chuyển sản phẩm/dịch vụ sang "Phân loại chưa đặt tên".
        /// </summary>
        private async Task DeleteCategoriesWithProductMigration(List<Guid> categoryIds)
        {
            if (categoryIds == null || categoryIds.Count == 0) return;

            // Lấy tất cả categories để xác định thứ tự xóa
            var allCategories = await _productServiceCategoryBll.GetAllAsync();
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
                    // Xóa danh mục (logic migration đã được xử lý trong DataAccess)
                    _productServiceCategoryBll.Delete(item.Category.Id);
                    System.Diagnostics.Debug.WriteLine($"Đã xóa danh mục: {item.Category.CategoryName}");
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
        private int CalculateCategoryLevel(Dal.DataContext.ProductServiceCategory category, 
            Dictionary<Guid, Dal.DataContext.ProductServiceCategory> categoryDict)
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
            System.Diagnostics.Debug.WriteLine("=== ClearSelectionState ===");
            System.Diagnostics.Debug.WriteLine($"Before clear: _selectedCategoryIds.Count = {_selectedCategoryIds.Count}");
            
            _selectedCategoryIds.Clear();
            
            // Clear tất cả selection (cả checkbox và regular selection)
            treeList1.ClearSelection();
            treeList1.FocusedNode = null;
            
            // Clear tất cả checkbox states
            ClearAllCheckBoxes();
            
            System.Diagnostics.Debug.WriteLine($"After clear: _selectedCategoryIds.Count = {_selectedCategoryIds.Count}");
            System.Diagnostics.Debug.WriteLine("=== End ClearSelectionState ===");
            
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
