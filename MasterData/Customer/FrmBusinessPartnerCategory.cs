using Bll.MasterData.CustomerBll;
using Common.Common;
using Common.Utils;
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

                // Calculate hierarchy properties for DTOs (Level, FullPath, HasChildren, PartnerCount)
                var dtoList = CalculateHierarchyProperties(categories, counts).ToList();

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
            
            // Cập nhật summary và selection info
            UpdateDataSummary();
            UpdateCurrentSelection();
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
                UpdateCurrentSelection();
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
        /// Nếu danh mục có đối tác, sẽ tự động di chuyển các đối tác sang danh mục "Chưa phân loại" trước khi xóa.
        /// </summary>
        private async Task DeleteCategoriesInOrder(List<Guid> categoryIds)
        {
            if (categoryIds == null || categoryIds.Count == 0) return;

            // Lấy tất cả categories để xác định thứ tự xóa
            var allCategories = await _businessPartnerCategoryBll.GetAllAsync();
            var categoryDict = allCategories.ToDictionary(c => c.Id);

            // Tạo danh sách categories cần xóa với thông tin level và số lượng đối tác
            var categoriesToDelete = categoryIds.Select(id =>
            {
                var category = categoryDict.TryGetValue(id, out var value) ? value : null;
                if (category == null) return null;

                // Tính level để xác định thứ tự xóa (level cao hơn = xóa trước)
                // Use DTO version of CalculateCategoryLevel
                var level = CalculateCategoryLevel(category, categoryDict);
                
                // Kiểm tra số lượng đối tác
                var partnerCount = _businessPartnerCategoryBll.GetPartnerCount(id);
                
                return new { Category = category, Level = level, PartnerCount = partnerCount };
            }).Where(x => x != null).OrderByDescending(x => x.Level).ToList();

            // Thông báo cho người dùng về các danh mục có đối tác sẽ được di chuyển
            var categoriesWithPartners = categoriesToDelete.Where(x => x.PartnerCount > 0).ToList();
            if (categoriesWithPartners.Any())
            {
                var message = "Các danh mục sau đây có đối tác và sẽ được di chuyển sang danh mục 'Chưa phân loại' trước khi xóa:\n\n";
                foreach (var item in categoriesWithPartners)
                {
                    message += $"• {item.Category.CategoryName}: {item.PartnerCount:N0} đối tác\n";
                }
                message += "\nBạn có muốn tiếp tục?";
                
                if (!MsgBox.ShowYesNo(message))
                {
                    return; // Người dùng hủy
                }
            }

            // Xóa theo thứ tự từ level cao xuống level thấp
            int totalMovedPartners = 0;
            foreach (var item in categoriesToDelete)
            {
                try
                {
                    // Repository sẽ tự động di chuyển đối tác sang "Chưa phân loại" nếu có
                    if (item.PartnerCount > 0)
                    {
                        _logger.Info("Đang xóa danh mục '{0}' có {1} đối tác. Các đối tác sẽ được di chuyển sang 'Chưa phân loại'", 
                            item.Category.CategoryName, item.PartnerCount);
                        totalMovedPartners += item.PartnerCount;
                    }
                    
                    _businessPartnerCategoryBll.Delete(item.Category.Id);
                    
                    _logger.Info("Đã xóa danh mục: {0}", item.Category.CategoryName);
                }
                catch (Exception ex)
                {
                    // Log lỗi nhưng tiếp tục xóa các item khác
                    _logger.Error("Lỗi xóa category {0}: {1}", item.Category.CategoryName, ex.Message);
                    throw; // Re-throw để form có thể hiển thị lỗi
                }
            }

            // Thông báo kết quả
            if (totalMovedPartners > 0)
            {
                ShowInfo($"Đã xóa {categoriesToDelete.Count} danh mục. {totalMovedPartners:N0} đối tác đã được di chuyển sang danh mục 'Chưa phân loại'.");
            }
            else
            {
                ShowInfo($"Đã xóa {categoriesToDelete.Count} danh mục thành công.");
            }
        }

        /// <summary>
        /// Tính toán các thuộc tính hierarchical cho DTOs (Level, FullPath, HasChildren, PartnerCount)
        /// </summary>
        private IEnumerable<BusinessPartnerCategoryDto> CalculateHierarchyProperties(
            List<BusinessPartnerCategoryDto> categories,
            Dictionary<Guid, int> counts)
        {
            if (categories == null || !categories.Any()) return [];

            var categoryDict = categories.ToDictionary(c => c.Id);

            // Calculate hierarchy properties for each DTO
            foreach (var category in categories)
            {
                // Set PartnerCount from counts dictionary
                category.PartnerCount = counts.TryGetValue(category.Id, out var count) ? count : 0;

                // Calculate Level
                category.Level = CalculateCategoryLevel(category, categoryDict);

                // Calculate HasChildren
                category.HasChildren = categories.Any(c => c.ParentId == category.Id);

                // Calculate FullPath
                category.FullPath = CalculateFullPath(category, categoryDict);

                // Set ParentCategoryName
                if (category.ParentId.HasValue && categoryDict.TryGetValue(category.ParentId.Value, out var parent))
                {
                    category.ParentCategoryName = parent.CategoryName;
                }
            }

            // Sort hierarchically: parent before children
            return SortHierarchical(categories);
        }

        /// <summary>
        /// Tính level của category trong cây phân cấp (works with DTOs).
        /// </summary>
        private int CalculateCategoryLevel(BusinessPartnerCategoryDto category,
            Dictionary<Guid, BusinessPartnerCategoryDto> categoryDict)
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
        /// Tính toán đường dẫn đầy đủ từ gốc đến category (works with DTOs).
        /// </summary>
        private string CalculateFullPath(BusinessPartnerCategoryDto category,
            Dictionary<Guid, BusinessPartnerCategoryDto> categoryDict)
        {
            var pathParts = new List<string> { category.CategoryName };
            var current = category;

            while (current.ParentId.HasValue && categoryDict.ContainsKey(current.ParentId.Value))
            {
                current = categoryDict[current.ParentId.Value];
                pathParts.Insert(0, current.CategoryName);
                if (pathParts.Count > 10) break; // Tránh infinite loop
            }

            return string.Join(" > ", pathParts);
        }

        /// <summary>
        /// Sắp xếp danh sách DTO theo cấu trúc cây (parent trước, children sau).
        /// </summary>
        private IEnumerable<BusinessPartnerCategoryDto> SortHierarchical(
            List<BusinessPartnerCategoryDto> dtoList)
        {
            var result = new List<BusinessPartnerCategoryDto>();
            var dtoDict = dtoList.ToDictionary(d => d.Id);

            // Add root categories first (ParentId = null)
            var rootCategories = dtoList.Where(d => !d.ParentId.HasValue).OrderBy(d => d.CategoryName);

            foreach (var root in rootCategories)
            {
                result.Add(root);
                AddChildrenRecursive(root, dtoDict, result);
            }

            return result;
        }

        /// <summary>
        /// Thêm children một cách đệ quy.
        /// </summary>
        private void AddChildrenRecursive(BusinessPartnerCategoryDto parent,
            Dictionary<Guid, BusinessPartnerCategoryDto> dtoDict, List<BusinessPartnerCategoryDto> result)
        {
            var children = dtoDict.Values
                .Where(d => d.ParentId == parent.Id)
                .OrderBy(d => d.CategoryName);

            foreach (var child in children)
            {
                result.Add(child);
                AddChildrenRecursive(child, dtoDict, result);
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
        /// Cập nhật thông tin tổng kết dữ liệu với format HTML
        /// </summary>
        private void UpdateDataSummary()
        {
            try
            {
                if (DataSummaryBarStaticItem == null) return;

                var totalRows = BusinessPartnerCategoryDtoGridView.RowCount;
                var activeCount = 0;
                var inactiveCount = 0;
                var totalPartners = 0;

                // Đếm số lượng active/inactive và tổng số đối tác
                if (businessPartnerCategoryDtoBindingSource.DataSource is IEnumerable<BusinessPartnerCategoryDto> data)
                {
                    foreach (var dto in data)
                    {
                        if (dto.IsActive)
                            activeCount++;
                        else
                            inactiveCount++;
                        totalPartners += dto.PartnerCount;
                    }
                }

                // Format HTML - font bình thường, chỉ in đậm và làm nổi bật các con số
                string html;
                if (totalRows == 0)
                {
                    html = "<color='#757575'>Chưa có dữ liệu</color>";
                }
                else
                {
                    html = "Tổng: <b><color='#1976D2'>{0:N0}</color></b> danh mục";
                    html = string.Format(html, totalRows);
                    
                    if (activeCount > 0 || inactiveCount > 0)
                    {
                        html += $" | <b><color='#4CAF50'>{activeCount:N0}</color></b> hoạt động";
                        if (inactiveCount > 0)
                        {
                            html += $" | <b><color='#F44336'>{inactiveCount:N0}</color></b> ngừng";
                        }
                    }
                    
                    if (totalPartners > 0)
                    {
                        html += $" | <b><color='#FF9800'>{totalPartners:N0}</color></b> đối tác";
                    }
                }

                DataSummaryBarStaticItem.Caption = html;
            }
            catch (Exception ex)
            {
                _logger.Warning("Lỗi cập nhật DataSummary: {0}", ex.Message);
            }
        }

        /// <summary>
        /// Cập nhật thông tin selection hiện tại với format HTML
        /// </summary>
        private void UpdateCurrentSelection()
        {
            try
            {
                if (CurrentSelectBarStaticItem == null) return;

                var selectedCount = _selectedCategoryIds?.Count ?? 0;
                string html;

                if (selectedCount == 0)
                {
                    html = "<color='#757575'>Chưa chọn dòng nào</color>";
                }
                else if (selectedCount == 1)
                {
                    // Hiển thị thông tin chi tiết của dòng được chọn
                    var selectedId = _selectedCategoryIds[0];
                    var selectedDto = businessPartnerCategoryDtoBindingSource.Cast<BusinessPartnerCategoryDto>()
                        .FirstOrDefault(d => d.Id == selectedId);

                    if (selectedDto != null)
                    {
                        var statusColor = selectedDto.IsActive ? "#4CAF50" : "#F44336";
                        var statusText = selectedDto.IsActive ? "Hoạt động" : "Ngừng";
                        
                        html = $"<b><color='#1976D2'>{selectedCount:N0}</color></b> dòng: <b><color='blue'>{selectedDto.CategoryName}</color></b>";
                        
                        if (!string.IsNullOrWhiteSpace(selectedDto.CategoryCode))
                        {
                            html += $" <color='#757575'>({selectedDto.CategoryCode})</color>";
                        }
                        
                        html += $" | Trạng thái: <b><color='{statusColor}'>{statusText}</color></b>";
                        
                        if (selectedDto.PartnerCount > 0)
                        {
                            html += $" | Đối tác: <b><color='#FF9800'>{selectedDto.PartnerCount:N0}</color></b>";
                        }
                    }
                    else
                    {
                        html = $"<b><color='#1976D2'>{selectedCount:N0}</color></b> dòng được chọn";
                    }
                }
                else
                {
                    html = $"<b><color='#1976D2'>{selectedCount:N0}</color></b> dòng được chọn";
                }

                CurrentSelectBarStaticItem.Caption = html;
            }
            catch (Exception ex)
            {
                _logger.Warning("Lỗi cập nhật CurrentSelection: {0}", ex.Message);
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
            UpdateCurrentSelection();
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
