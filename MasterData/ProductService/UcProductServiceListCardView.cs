using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bll.Common;
using Bll.MasterData.ProductServiceBll;
using Bll.Utils;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Card;
using DevExpress.XtraSplashScreen;
using MasterData.ProductService.Converters;
using MasterData.ProductService.Dto;

namespace MasterData.ProductService
{
    /// <summary>
    /// UserControl quản lý danh sách sản phẩm/dịch vụ dạng CardView.
    /// Cung cấp chức năng CRUD đầy đủ với CardView, pagination, tìm kiếm toàn diện và giao diện thân thiện.
    /// </summary>
    public partial class UcProductServiceListCardView : XtraUserControl
    {
        #region ========== KHAI BÁO BIẾN ==========

        /// <summary>
        /// Business Logic Layer cho sản phẩm/dịch vụ
        /// </summary>
        private readonly ProductServiceBll _productServiceBll = new ProductServiceBll();

        /// <summary>
        /// Danh sách ID của các sản phẩm/dịch vụ được chọn
        /// </summary>
        private List<Guid> _selectedProductServiceIds = new List<Guid>();

        /// <summary>
        /// Guard tránh gọi LoadDataAsync song song
        /// </summary>
        private bool _isLoading;

        /// <summary>
        /// Index trang hiện tại (0-based)
        /// </summary>
        private int _currentPageIndex;

        /// <summary>
        /// Số dòng trên mỗi trang
        /// </summary>
        private int _pageSize = 50;

        /// <summary>
        /// Tổng số dòng dữ liệu
        /// </summary>
        private int _totalCount;

        /// <summary>
        /// Tổng số trang
        /// </summary>
        private int _totalPages;

        #endregion

        #region ========== CONSTRUCTOR & PUBLIC METHODS ==========

        /// <summary>
        /// Khởi tạo control, đăng ký event UI
        /// </summary>
        public UcProductServiceListCardView()
        {
            InitializeComponent();

            // Toolbar events
            ListDataBarButtonItem.ItemClick += ListDataBarButtonItem_ItemClick;
            NewBarButtonItem.ItemClick += NewBarButtonItem_ItemClick;
            EditBarButtonItem.ItemClick += EditBarButtonItem_ItemClick;
            DeleteBarButtonItem.ItemClick += DeleteBarButtonItem_ItemClick;
            CountVariantAndImageBarButtonItem.ItemClick += CountVariantAndImageBarButtonItem_ItemClick;

            // Grid events
            ProductServiceListCardView.SelectionChanged += ProductServiceListCardView_SelectionChanged;
            ProductServiceListCardView.CustomDrawCardCaption += ProductServiceListCardView_CustomDrawCardCaption;
            ProductServiceListCardView.KeyDown += ProductServiceListCardView_KeyDown;

            // Set custom caption format
            ProductServiceListCardView.CardCaptionFormat = @"Sản phẩm dịch vụ thứ {0}";


            // Filter events
            DataFilterBtn.ItemClick += DataFilterBtn_ItemClick;


            UpdateButtonStates();
        }

        /// <summary>
        /// Thực hiện operation async với WaitingForm1 hiển thị
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

        #region ========== SỰ KIỆN FORM ==========

        #region ========== SỰ KIỆN TOOLBAR ==========

        /// <summary>
        /// Người dùng bấm "Danh sách" để tải dữ liệu
        /// </summary>
        private async void ListDataBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// Người dùng bấm "Mới"
        /// </summary>
        private async void NewBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Tham khảo mẫu ConfigSqlServerInfoBarButtonItem_ItemClick: dùng ShowScope để auto-close overlay
            try
            {
                using (OverlayManager.ShowScope(this))
                {
                    using (var form = new FrmProductServiceDetail(Guid.Empty))
                    {
                        form.StartPosition = FormStartPosition.CenterParent;
                        form.ShowDialog(this);

                        await LoadDataAsyncWithoutSplash();
                        UpdateButtonStates();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi hiển thị màn hình thêm mới");
            }
        }

        #endregion

        #region ========== SỰ KIỆN CARDVIEW ==========

        /// <summary>
        /// CardView selection thay đổi -> cập nhật danh sách Id đã chọn và trạng thái nút
        /// </summary>
        private void ProductServiceListCardView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                // Lấy danh sách các row được chọn từ CardView
                if (sender is CardView cardView)
                {
                    _selectedProductServiceIds.Clear();

                    // Lấy tất cả các row được chọn
                    var selectedRowHandles = cardView.GetSelectedRows();

                    foreach (var rowHandle in selectedRowHandles)
                    {
                        if (rowHandle < 0) continue;
                        if (cardView.GetRow(rowHandle) is ProductServiceDto dto)
                        {
                            _selectedProductServiceIds.Add(dto.Id);
                        }
                    }
                }

                UpdateButtonStates();
                UpdateStatusBar();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        /// <summary>
        /// Custom draw card caption để hiển thị màu sắc theo trạng thái sản phẩm
        /// </summary>
        private void ProductServiceListCardView_CustomDrawCardCaption(object sender, CardCaptionCustomDrawEventArgs e)
        {
            try
            {
                // Lấy dữ liệu từ CardView
                if (!(sender is CardView cardView) || e.RowHandle < 0) return;

                var dto = (ProductServiceDto)cardView.GetRow(e.RowHandle);
                if (dto == null) return;

                // Tùy chỉnh màu sắc theo trạng thái hoạt động
                if (!dto.IsActive)
                {
                    // Sản phẩm không còn sử dụng - màu đỏ
                    e.Appearance.BackColor = Color.LightPink;
                    e.Appearance.ForeColor = Color.DarkRed;
                    e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Strikeout);
                }
                else if (dto.IsService)
                {
                    // Dịch vụ đang hoạt động - màu xanh
                    e.Appearance.BackColor = Color.LightCyan;
                    e.Appearance.ForeColor = Color.DarkBlue;
                }
                else
                {
                    // Sản phẩm đang hoạt động - màu vàng
                    e.Appearance.BackColor = Color.LightYellow;
                    e.Appearance.ForeColor = Color.DarkGreen;
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi custom draw card caption");
            }
        }


        /// <summary>
        /// Xử lý phím tắt cho CardView
        /// </summary>
        private void ProductServiceListCardView_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                switch (e.KeyCode)
                {
                    case Keys.A when e.Control:
                        // Ctrl+A: Chọn tất cả
                        SelectAllCards();
                        e.Handled = true;
                        break;
                    case Keys.Escape:
                        // Escape: Bỏ chọn tất cả
                        DeselectAllCards();
                        e.Handled = true;
                        break;
                    case Keys.Delete:
                        // Delete: Xóa các item được chọn
                        if (_selectedProductServiceIds.Count > 0)
                        {
                            DeleteBarButtonItem_ItemClick(null, null);
                            e.Handled = true;
                        }

                        break;
                    case Keys.F2:
                        // F2: Chỉnh sửa item được chọn
                        if (_selectedProductServiceIds.Count == 1)
                        {
                            EditBarButtonItem_ItemClick(null, null);
                            e.Handled = true;
                        }

                        break;

                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi xử lý phím tắt");
            }
        }

        /// <summary>
        /// Cập nhật trạng thái các nút toolbar dựa trên selection
        /// </summary>
        private void UpdateButtonStates()
        {
            try
            {
                var selectedCount = _selectedProductServiceIds?.Count ?? 0;
                // Edit: chỉ khi chọn đúng 1 dòng
                if (EditBarButtonItem != null)
                    EditBarButtonItem.Enabled = selectedCount == 1;
                // Delete: khi chọn >= 1 dòng
                if (DeleteBarButtonItem != null)
                    DeleteBarButtonItem.Enabled = selectedCount >= 1;
                // Count: chỉ khi chọn >= 1 dòng (giống như Delete)
                if (CountVariantAndImageBarButtonItem != null)
                    CountVariantAndImageBarButtonItem.Enabled = selectedCount >= 1;


            }
            catch
            {
                // ignore
            }
        }

        /// <summary>
        /// Cập nhật status bar với thông tin selection và data summary
        /// </summary>
        private void UpdateStatusBar()
        {
            try
            {
                UpdateSelectedRowStatus();
                UpdateDataSummaryStatus();
            }
            catch
            {
                // ignore
            }
        }

        /// <summary>
        /// Cập nhật thông tin số dòng đang được chọn
        /// </summary>
        private void UpdateSelectedRowStatus()
        {
            try
            {
                if (SelectedRowBarStaticItem == null) return;

                var selectedCount = _selectedProductServiceIds?.Count ?? 0;
                if (selectedCount == 0)
                {
                    SelectedRowBarStaticItem.Caption = @"Chưa chọn sản phẩm dịch vụ nào";
                }
                else if (selectedCount == 1)
                {
                    SelectedRowBarStaticItem.Caption = @"Đang chọn sản phẩm dịch vụ thứ 1";
                }
                else
                {
                    SelectedRowBarStaticItem.Caption = $@"Đang chọn {selectedCount} sản phẩm dịch vụ";
                }
            }
            catch
            {
                // ignore
            }
        }

        /// <summary>
        /// Cập nhật thông tin tổng kết dữ liệu với HTML formatting và pagination info
        /// </summary>
        private void UpdateDataSummaryStatus()
        {
            try
            {
                if (DataSummaryBarStaticItem == null) return;

                var currentData = productServiceDtoBindingSource.DataSource as List<ProductServiceDto>;
                if (currentData == null || !currentData.Any())
                {
                    DataSummaryBarStaticItem.Caption = @"Chưa có dữ liệu";
                    return;
                }

                var currentPageCount = currentData.Count;
                var productCount = currentData.Count(x => !x.IsService);
                var serviceCount = currentData.Count(x => x.IsService);
                var activeCount = currentData.Count(x => x.IsActive);
                var inactiveCount = currentData.Count(x => !x.IsActive);

                // Tạo HTML content với màu sắc và pagination info
                string summary;
                if (_pageSize == int.MaxValue)
                {
                    // Trường hợp "Tất cả" - không phân trang
                    summary = "<b>Tất cả dữ liệu</b> | " +
                              $"<b>Hiển thị: {currentPageCount}/{_totalCount}</b> | " +
                              $"<color=blue>Sản phẩm: {productCount}</color> | " +
                              $"<color=green>Dịch vụ: {serviceCount}</color> | " +
                              $"<color=green>Hoạt động: {activeCount}</color> | " +
                              $"<color=red>Không hoạt động: {inactiveCount}</color>";
                }
                else
                {
                    // Trường hợp có phân trang
                    summary = $"<b>Trang {_currentPageIndex + 1}/{_totalPages}</b> | " +
                              $"<b>Hiển thị: {currentPageCount}/{_totalCount}</b> | " +
                              $"<color=blue>Sản phẩm: {productCount}</color> | " +
                              $"<color=green>Dịch vụ: {serviceCount}</color> | " +
                              $"<color=green>Hoạt động: {activeCount}</color> | " +
                              $"<color=red>Không hoạt động: {inactiveCount}</color>";
                }

                DataSummaryBarStaticItem.Caption = summary;
            }
            catch
            {
                // ignore
            }
        }

        #endregion

        #endregion

        #region ========== SỰ KIỆN FILTER & SEARCH ==========

        /// <summary>
        /// Người dùng bấm "Lọc dữ liệu" để tìm kiếm toàn diện
        /// </summary>
        private async void DataFilterBtn_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                // Hiển thị menu tùy chọn tìm kiếm
                var searchOption = ShowSearchOptionsDialog();

                if (searchOption == SearchOption.Cancel)
                {
                    return;
                }

                var searchKeyword = "";

                if (searchOption == SearchOption.SimpleSearch)
                {
                    // Tìm kiếm đơn giản
                    searchKeyword = InputBoxHelper.ShowTextInput(
                        "Nhập từ khóa để tìm kiếm trong tất cả các cột:",
                        "Tìm Kiếm Toàn Diện"
                    );
                }
                else if (searchOption == SearchOption.AdvancedSearch)
                {
                    // Tìm kiếm nâng cao
                    searchKeyword = ShowAdvancedSearchDialog();
                }

                // Nếu user không nhập gì hoặc Cancel
                if (string.IsNullOrWhiteSpace(searchKeyword))
                {
                    return;
                }

                // Thực hiện tìm kiếm với WaitingForm
                await ExecuteWithWaitingFormAsync(async () =>
                {
                    await PerformComprehensiveSearchAsync(searchKeyword.Trim());
                });
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi thực hiện tìm kiếm toàn diện");
            }
        }

        #endregion

        #region ========== QUẢN LÝ DỮ LIỆU ==========

        /// <summary>
        /// Tải dữ liệu và bind vào Grid (Async, hiển thị WaitForm)
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
        /// Tải dữ liệu và bind vào Grid (Async, không hiển thị WaitForm).
        /// Sử dụng pagination để tối ưu performance
        /// </summary>
        private async Task LoadDataAsyncWithoutSplash()
        {
            try
            {
                // Reset pagination
                _currentPageIndex = 0;

                // Get total count
                _totalCount = await _productServiceBll.GetCountAsync();

                // Xử lý trường hợp "Tất cả" (không phân trang)
                if (_pageSize == int.MaxValue)
                {
                    _totalPages = 1; // Chỉ có 1 trang
                    await LoadAllDataAsync();
                }
                else
                {
                    _totalPages = (int)Math.Ceiling((double)_totalCount / _pageSize);

                    // Update pagination control first (removed for CardView)

                    // Load first page
                    await LoadPageAsync(_currentPageIndex);
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi tải dữ liệu");
            }
        }

        /// <summary>
        /// Load tất cả dữ liệu (không phân trang)
        /// </summary>
        private async Task LoadAllDataAsync()
        {
            try
            {
                // Get all data
                var entities = await _productServiceBll.GetAllAsync();

                // Convert to DTOs (without counting to improve performance)
                var dtoList = entities.ToDtoList(categoryId => _productServiceBll.GetCategoryName(categoryId)
                ).ToList();

                BindGrid(dtoList);
                _currentPageIndex = 0;

                // Update pagination control (disable pagination) - removed for CardView
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi tải tất cả dữ liệu");
            }
        }

        /// <summary>
        /// Load dữ liệu cho một trang cụ thể
        /// </summary>
        /// <param name="pageIndex">Index của trang (0-based)</param>
        private async Task LoadPageAsync(int pageIndex)
        {
            try
            {
                // Get paged data using optimization methods
                var entities = await _productServiceBll.GetPagedAsync(
                    pageIndex, _pageSize);

                // Convert to DTOs (without counting to improve performance)
                var dtoList = entities.ToDtoList(categoryId => _productServiceBll.GetCategoryName(categoryId)
                ).ToList();

                BindGrid(dtoList);
                _currentPageIndex = pageIndex;

                // Update pagination control - removed for CardView
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi tải trang dữ liệu");
            }
        }

        /// <summary>
        /// Bind danh sách DTO vào Grid và cấu hình hiển thị
        /// </summary>
        private void BindGrid(List<ProductServiceDto> data)
        {
            productServiceDtoBindingSource.DataSource = data;
            // CardView doesn't have BestFitColumns method
            UpdateButtonStates();
            UpdateStatusBar();
        }

        #endregion

        #region ========== SỰ KIỆN CRUD ==========

        /// <summary>
        /// Người dùng bấm "Điều chỉnh"
        /// </summary>
        private async void EditBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                // Chỉ cho phép chỉnh sửa 1 dòng dữ liệu
                if (_selectedProductServiceIds == null || _selectedProductServiceIds.Count == 0)
                {
                    ShowInfo("Vui lòng chọn một dòng để chỉnh sửa.");
                    return;
                }

                if (_selectedProductServiceIds.Count > 1)
                {
                    ShowInfo("Chỉ cho phép chỉnh sửa 1 dòng. Vui lòng bỏ chọn bớt.");
                    return;
                }

                var id = _selectedProductServiceIds[0];
                var selectedDtos = GetSelectedProductServiceDtos();
                var dto = selectedDtos.FirstOrDefault(x => x.Id == id);

                if (dto == null)
                {
                    // Fallback: tìm trong datasource
                    if (productServiceDtoBindingSource.DataSource is IEnumerable<ProductServiceDto> list)
                    {
                        dto = list.FirstOrDefault(x => x.Id == id);
                    }
                }

                if (dto == null)
                {
                    ShowInfo("Không thể xác định dòng được chọn để chỉnh sửa.");
                    return;
                }

                // Tham khảo mẫu ConfigSqlServerInfoBarButtonItem_ItemClick: dùng ShowScope để auto-close overlay
                try
                {
                    using (OverlayManager.ShowScope(this))
                    {
                        using (var form = new FrmProductServiceDetail(dto.Id))
                        {
                            form.StartPosition = FormStartPosition.CenterParent;
                            form.ShowDialog(this);

                            await LoadDataAsyncWithoutSplash();
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
        /// Người dùng bấm "Xóa"
        /// </summary>
        private async void DeleteBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (_selectedProductServiceIds == null || _selectedProductServiceIds.Count == 0)
                {
                    ShowInfo("Vui lòng chọn ít nhất một dòng để xóa.");
                    return;
                }

                var confirmMessage = _selectedProductServiceIds.Count == 1
                    ? "Bạn có chắc muốn xóa dòng dữ liệu đã chọn?"
                    : $"Bạn có chắc muốn xóa {_selectedProductServiceIds.Count} dòng dữ liệu đã chọn?";

                if (!MsgBox.ShowYesNo(confirmMessage)) return;

                try
                {
                    await ExecuteWithWaitingFormAsync(async () =>
                    {
                        foreach (var id in _selectedProductServiceIds.ToList())
                        {
                            _productServiceBll.Delete(id);
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
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// Người dùng bấm "Đếm số lượng" - đếm VariantCount và ImageCount cho các sản phẩm/dịch vụ được chọn
        /// </summary>
        private void CountVariantAndImageBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                // Kiểm tra có dòng nào được chọn không
                if (_selectedProductServiceIds == null || !_selectedProductServiceIds.Any())
                {
                    ShowInfo("Vui lòng chọn ít nhất một dòng dữ liệu để đếm số lượng.");
                    return;
                }

                // Thực hiện đếm với splash screen
                _ = ExecuteWithWaitingFormAsync(async () => { await CountSelectedProductsAsync(); });
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khi đếm số lượng biến thể và hình ảnh");
            }
        }

        #endregion

        #region ========== TÌM KIẾM ==========

        /// <summary>
        /// Enum cho các tùy chọn tìm kiếm
        /// </summary>
        private enum SearchOption
        {
            Cancel,
            SimpleSearch,
            AdvancedSearch
        }

        /// <summary>
        /// Hiển thị dialog chọn tùy chọn tìm kiếm
        /// </summary>
        /// <returns>Tùy chọn được chọn</returns>
        private SearchOption ShowSearchOptionsDialog()
        {
            try
            {
                var options = new object[] { "Tìm kiếm đơn giản", "Tìm kiếm nâng cao" };
                var result = InputBoxHelper.ShowComboBoxInput(
                    "Chọn loại tìm kiếm:",
                    "Tùy Chọn Tìm Kiếm",
                    options,
                    "Tìm kiếm đơn giản"
                );

                if (result == null) return SearchOption.Cancel;

                var selectedOption = result.ToString();
                switch (selectedOption)
                {
                    case "Tìm kiếm đơn giản":
                        return SearchOption.SimpleSearch;
                    case "Tìm kiếm nâng cao":
                        return SearchOption.AdvancedSearch;
                    default:
                        return SearchOption.Cancel;
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi hiển thị tùy chọn tìm kiếm");
                return SearchOption.Cancel;
            }
        }

        /// <summary>
        /// Hiển thị dialog tìm kiếm nâng cao
        /// </summary>
        /// <returns>Từ khóa tìm kiếm</returns>
        private string ShowAdvancedSearchDialog()
        {
            try
            {
                // Tạo MemoEdit cho nhập nhiều từ khóa
                var memoEdit = new MemoEdit();
                memoEdit.Properties.MaxLength = 500;
                memoEdit.Properties.WordWrap = true;
                memoEdit.Height = 100;
                memoEdit.Properties.NullText = @"Nhập từ khóa tìm kiếm (mỗi dòng một từ khóa)...";

                var result = InputBoxHelper.ShowCustomInput(
                    "Nhập từ khóa tìm kiếm (mỗi dòng một từ khóa):",
                    "Tìm Kiếm Nâng Cao",
                    memoEdit,
                    ""
                );

                return result?.ToString() ?? "";
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi hiển thị dialog tìm kiếm nâng cao");
                return "";
            }
        }

        /// <summary>
        /// Thực hiện tìm kiếm toàn diện trong tất cả các cột
        /// </summary>
        /// <param name="searchKeyword">Từ khóa tìm kiếm (có thể chứa nhiều từ khóa phân cách bởi dòng mới)</param>
        private async Task PerformComprehensiveSearchAsync(string searchKeyword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchKeyword))
                {
                    await LoadDataAsyncWithoutSplash();
                    return;
                }

                // Phân tích từ khóa tìm kiếm
                var searchKeywords = ParseSearchKeywords(searchKeyword);

                // Tìm kiếm trong database với từ khóa đầu tiên (hoặc từ khóa chính)
                var primaryKeyword = searchKeywords.FirstOrDefault() ?? searchKeyword.Trim();
                var searchResults = await _productServiceBll.SearchAsync(primaryKeyword);

                // Convert to DTOs
                var dtoList = searchResults.ToDtoList(categoryId => _productServiceBll.GetCategoryName(categoryId)
                ).ToList();

                // Thực hiện tìm kiếm bổ sung với tất cả từ khóa
                var filteredResults = PerformAdvancedClientSideFiltering(dtoList, searchKeywords);

                // Highlight từ khóa tìm kiếm trong kết quả
                var highlightedResults = HighlightSearchKeywords(filteredResults, searchKeywords);

                // Bind kết quả tìm kiếm với highlight
                BindGridWithHighlight(highlightedResults);

                // Cập nhật status bar
                UpdateStatusBar();

                // Hiển thị thông báo chi tiết
                ShowComprehensiveSearchResult(searchKeyword, filteredResults.Count, dtoList.Count,
                    searchKeywords.Count);
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi thực hiện tìm kiếm toàn diện");
            }
        }

        /// <summary>
        /// Phân tích từ khóa tìm kiếm thành danh sách
        /// </summary>
        /// <param name="searchKeyword">Từ khóa gốc</param>
        /// <returns>Danh sách từ khóa</returns>
        private List<string> ParseSearchKeywords(string searchKeyword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchKeyword))
                    return new List<string>();

                // Tách theo dòng mới và loại bỏ khoảng trắng
                var keywords = searchKeyword
                    .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(k => k.Trim())
                    .Where(k => !string.IsNullOrWhiteSpace(k))
                    .ToList();

                return keywords;
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi phân tích từ khóa tìm kiếm");
                return new List<string> { searchKeyword?.Trim() };
            }
        }

        /// <summary>
        /// Thực hiện lọc dữ liệu nâng cao với nhiều từ khóa
        /// </summary>
        /// <param name="dataList">Danh sách dữ liệu</param>
        /// <param name="searchKeywords">Danh sách từ khóa</param>
        /// <returns>Danh sách đã lọc</returns>
        private List<ProductServiceDto> PerformAdvancedClientSideFiltering(List<ProductServiceDto> dataList,
            List<string> searchKeywords)
        {
            try
            {
                if (searchKeywords == null || !searchKeywords.Any() || dataList == null || !dataList.Any())
                    return dataList;

                // Nếu chỉ có 1 từ khóa, sử dụng method cũ
                if (searchKeywords.Count == 1)
                {
                    return PerformClientSideFiltering(dataList, searchKeywords[0]);
                }

                // Tìm kiếm với nhiều từ khóa (tất cả từ khóa phải match)
                return dataList.Where(dto =>
                    searchKeywords.All(keyword =>
                        IsKeywordMatch(dto, keyword.ToLower().Trim())
                    )
                ).ToList();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi lọc dữ liệu nâng cao");
                return dataList; // Trả về dữ liệu gốc nếu có lỗi
            }
        }

        /// <summary>
        /// Kiểm tra xem một DTO có match với từ khóa không
        /// </summary>
        /// <param name="dto">DTO cần kiểm tra</param>
        /// <param name="keyword">Từ khóa</param>
        /// <returns>True nếu match</returns>
        private bool IsKeywordMatch(ProductServiceDto dto, string keyword)
        {
            try
            {
                return
                    // Tìm kiếm trong các trường text
                    (dto.Code?.ToLower().Contains(keyword) == true) ||
                    (dto.Name?.ToLower().Contains(keyword) == true) ||
                    (dto.Description?.ToLower().Contains(keyword) == true) ||
                    (dto.CategoryName?.ToLower().Contains(keyword) == true) ||
                    (dto.TypeDisplay?.ToLower().Contains(keyword) == true) ||
                    (dto.StatusDisplay?.ToLower().Contains(keyword) == true) ||

                    // Tìm kiếm trong các trường số
                    (dto.VariantCount.ToString().Contains(keyword)) ||
                    (dto.ImageCount.ToString().Contains(keyword)) ||

                    // Tìm kiếm trong các trường boolean
                    (dto.IsActive.ToString().ToLower().Contains(keyword)) ||
                    (dto.IsService.ToString().ToLower().Contains(keyword)) ||

                    // Tìm kiếm trong ID (nếu cần)
                    (dto.Id.ToString().ToLower().Contains(keyword));
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Thực hiện lọc dữ liệu phía client (bổ sung cho database search)
        /// </summary>
        /// <param name="dataList">Danh sách dữ liệu</param>
        /// <param name="searchKeyword">Từ khóa tìm kiếm</param>
        /// <returns>Danh sách đã lọc</returns>
        private List<ProductServiceDto> PerformClientSideFiltering(List<ProductServiceDto> dataList,
            string searchKeyword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchKeyword) || dataList == null || !dataList.Any())
                    return dataList;

                var keyword = searchKeyword.ToLower().Trim();

                return dataList.Where(dto =>
                    // Tìm kiếm trong các trường text
                    (dto.Code?.ToLower().Contains(keyword) == true) ||
                    (dto.Name?.ToLower().Contains(keyword) == true) ||
                    (dto.Description?.ToLower().Contains(keyword) == true) ||
                    (dto.CategoryName?.ToLower().Contains(keyword) == true) ||
                    (dto.TypeDisplay?.ToLower().Contains(keyword) == true) ||
                    (dto.StatusDisplay?.ToLower().Contains(keyword) == true) ||

                    // Tìm kiếm trong các trường số
                    (dto.VariantCount.ToString().Contains(keyword)) ||
                    (dto.ImageCount.ToString().Contains(keyword)) ||

                    // Tìm kiếm trong các trường boolean
                    (dto.IsActive.ToString().ToLower().Contains(keyword)) ||
                    (dto.IsService.ToString().ToLower().Contains(keyword)) ||

                    // Tìm kiếm trong ID (nếu cần)
                    (dto.Id.ToString().ToLower().Contains(keyword))
                ).ToList();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi lọc dữ liệu phía client");
                return dataList; // Trả về dữ liệu gốc nếu có lỗi
            }
        }

        /// <summary>
        /// Highlight từ khóa tìm kiếm trong danh sách DTO
        /// </summary>
        /// <param name="dtoList">Danh sách DTO</param>
        /// <param name="searchKeywords">Danh sách từ khóa tìm kiếm</param>
        /// <returns>Danh sách DTO với highlight</returns>
        private List<ProductServiceDto> HighlightSearchKeywords(List<ProductServiceDto> dtoList,
            List<string> searchKeywords)
        {
            try
            {
                if (searchKeywords == null || !searchKeywords.Any() || dtoList == null || !dtoList.Any())
                    return dtoList;

                var highlightedList = new List<ProductServiceDto>();

                foreach (var dto in dtoList)
                {
                    // Tạo bản sao để không ảnh hưởng đến dữ liệu gốc
                    var highlightedDto = new ProductServiceDto
                    {
                        Id = dto.Id,
                        Code = HighlightText(dto.Code, searchKeywords),
                        Name = HighlightText(dto.Name, searchKeywords),
                        Description = HighlightText(dto.Description, searchKeywords),
                        CategoryName = HighlightText(dto.CategoryName, searchKeywords),
                        // TypeDisplay và StatusDisplay là read-only, không thể assign
                        VariantCount = dto.VariantCount,
                        ImageCount = dto.ImageCount,
                        IsActive = dto.IsActive,
                        IsService = dto.IsService,
                        ThumbnailImage = dto.ThumbnailImage
                    };

                    highlightedList.Add(highlightedDto);
                }

                return highlightedList;
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi highlight từ khóa tìm kiếm");
                return dtoList; // Trả về dữ liệu gốc nếu có lỗi
            }
        }

        /// <summary>
        /// Highlight từ khóa trong text sử dụng DevExpress HTML syntax
        /// </summary>
        /// <param name="text">Text cần highlight</param>
        /// <param name="keywords">Danh sách từ khóa</param>
        /// <returns>Text đã được highlight</returns>
        private string HighlightText(string text, List<string> keywords)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(text) || keywords == null || !keywords.Any())
                    return text;

                var highlightedText = text;

                foreach (var keyword in keywords)
                {
                    if (string.IsNullOrWhiteSpace(keyword))
                        continue;

                    // Tìm kiếm case-insensitive
                    var regex = new Regex(
                        Regex.Escape(keyword),
                        RegexOptions.IgnoreCase
                    );

                    // Thay thế với DevExpress HTML syntax
                    // Sử dụng <color> và <b> tags theo DevExpress documentation
                    highlightedText = regex.Replace(highlightedText,
                        $"<color='red'><b>{keyword}</b></color>");
                }

                return highlightedText;
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi highlight text");
                return text; // Trả về text gốc nếu có lỗi
            }
        }

        /// <summary>
        /// Bind dữ liệu vào grid với HTML formatting support
        /// </summary>
        /// <param name="dtoList">Danh sách DTO với HTML highlight</param>
        private void BindGridWithHighlight(List<ProductServiceDto> dtoList)
        {
            try
            {
                // Bind dữ liệu
                productServiceDtoBindingSource.DataSource = dtoList;
                // CardView doesn't have BestFitColumns method
                UpdateButtonStates();
                UpdateStatusBar();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi bind dữ liệu với highlight vào grid");
            }
        }

        /// <summary>
        /// Hiển thị kết quả tìm kiếm toàn diện
        /// </summary>
        /// <param name="searchKeyword">Từ khóa tìm kiếm</param>
        /// <param name="filteredCount">Số kết quả sau khi lọc</param>
        /// <param name="totalCount">Tổng số kết quả từ database</param>
        /// <param name="keywordCount">Số lượng từ khóa</param>
        private void ShowComprehensiveSearchResult(string searchKeyword, int filteredCount, int totalCount,
            int keywordCount = 1)
        {
            try
            {
                var message = "🔍 <b>Kết quả tìm kiếm toàn diện</b><br/><br/>" +
                              $"📝 <b>Từ khóa:</b> '{searchKeyword}'<br/>" +
                              $"🔢 <b>Số từ khóa:</b> {keywordCount}<br/>" +
                              $"📊 <b>Kết quả:</b> {filteredCount} dòng<br/>" +
                              $"🗃️ <b>Từ database:</b> {totalCount} dòng<br/><br/>";

                if (filteredCount == 0)
                {
                    message += "❌ <b>Không tìm thấy kết quả nào phù hợp</b><br/><br/>" +
                               "💡 <b>Gợi ý:</b><br/>" +
                               "• Kiểm tra lại từ khóa<br/>" +
                               "• Thử từ khóa ngắn hơn<br/>" +
                               "• Sử dụng từ khóa tiếng Việt không dấu";

                    if (keywordCount > 1)
                    {
                        message += "<br/>• Thử giảm số lượng từ khóa<br/>" +
                                   "• Đảm bảo tất cả từ khóa đều có trong dữ liệu";
                    }
                }
                else if (filteredCount < totalCount)
                {
                    message += $"✅ <b>Tìm thấy {filteredCount} kết quả phù hợp</b><br/><br/>" +
                               "🔍 <b>Tìm kiếm trong:</b><br/>" +
                               "• Mã sản phẩm/dịch vụ<br/>" +
                               "• Tên sản phẩm/dịch vụ<br/>" +
                               "• Mô tả<br/>" +
                               "• Tên danh mục<br/>" +
                               "• Loại (Sản phẩm/Dịch vụ)<br/>" +
                               "• Trạng thái<br/>" +
                               "• Số lượng biến thể/hình ảnh";

                    if (keywordCount > 1)
                    {
                        message +=
                            $"<br/><br/>🎯 <b>Tìm kiếm nâng cao:</b> Tất cả {keywordCount} từ khóa phải có trong cùng một dòng";
                    }
                }
                else
                {
                    message += $"✅ <b>Tìm thấy {filteredCount} kết quả</b><br/><br/>" +
                               "🎯 <b>Tất cả kết quả từ database đều phù hợp</b>";

                    if (keywordCount > 1)
                    {
                        message +=
                            $"<br/><br/>🔍 <b>Tìm kiếm nâng cao:</b> Tất cả {keywordCount} từ khóa đều có trong dữ liệu";
                    }
                }

                // Sử dụng helper method với HTML support
                ShowHtmlMessageBox(message, "Kết Quả Tìm Kiếm");
            }
            catch (Exception)
            {
                // Fallback message nếu có lỗi
                MsgBox.ShowSuccess($"Tìm thấy {filteredCount} kết quả cho từ khóa: '{searchKeyword}'");
            }
        }

        /// <summary>
        /// Hiển thị message box với HTML formatting
        /// </summary>
        /// <param name="message">Nội dung message (có thể chứa HTML)</param>
        /// <param name="title">Tiêu đề dialog</param>
        /// <param name="icon">Icon của dialog</param>
        private void ShowHtmlMessageBox(string message, string title = "Thông báo",
            MessageBoxIcon icon = MessageBoxIcon.Information)
        {
            try
            {
                XtraMessageBox.Show(
                    message,
                    title,
                    MessageBoxButtons.OK,
                    icon,
                    DefaultBoolean.True // Enable HTML
                );
            }
            catch (Exception)
            {
                // Fallback về MsgBox thông thường nếu có lỗi
                MsgBox.ShowSuccess(message.Replace("<br/>", "\n").Replace("<b>", "").Replace("</b>", ""));
            }
        }

        #endregion

        #region ========== TIỆN ÍCH ==========

        /// <summary>
        /// Đếm số lượng biến thể và hình ảnh cho các sản phẩm/dịch vụ được chọn.
        /// Sử dụng method optimization mới
        /// </summary>
        private async Task CountSelectedProductsAsync()
        {
            try
            {
                if (_selectedProductServiceIds == null || !_selectedProductServiceIds.Any())
                    return;

                // Lấy dữ liệu hiện tại từ grid
                var currentData = productServiceDtoBindingSource.DataSource as List<ProductServiceDto>;
                if (currentData == null)
                    return;

                // Đếm số lượng cho các sản phẩm được chọn sử dụng async method
                var counts = await _productServiceBll.GetCountsForProductsAsync(_selectedProductServiceIds.ToList());

                // Cập nhật dữ liệu với số lượng đã đếm
                foreach (var dto in currentData)
                {
                    if (counts.ContainsKey(dto.Id))
                    {
                        dto.VariantCount = counts[dto.Id].VariantCount;
                        dto.ImageCount = counts[dto.Id].ImageCount;
                    }
                }

                // Refresh grid để hiển thị số lượng mới
                productServiceDtoBindingSource.ResetBindings(false);

                // Hiển thị thông báo đã đếm xong số lượng cho các sản phẩm được chọn
                MsgBox.ShowSuccess(
                    $"Đã đếm xong số lượng biến thể và hình ảnh cho {_selectedProductServiceIds.Count} sản phẩm/dịch vụ được chọn.");

                // Cập nhật status bar với thông tin mới
                UpdateStatusBar();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khi đếm số lượng cho các sản phẩm được chọn");
            }
        }

        /// <summary>
        /// Xóa trạng thái chọn hiện tại trên Grid
        /// </summary>
        private void ClearSelectionState()
        {
            _selectedProductServiceIds.Clear();
            ProductServiceListCardView.ClearSelection();
            ProductServiceListCardView.FocusedRowHandle = GridControl.InvalidRowHandle;
            UpdateButtonStates();
            UpdateStatusBar();
        }

        /// <summary>
        /// Lấy danh sách các DTO được chọn
        /// </summary>
        /// <returns>Danh sách ProductServiceDto được chọn</returns>
        private List<ProductServiceDto> GetSelectedProductServiceDtos()
        {
            var selectedDtos = new List<ProductServiceDto>();

            try
            {
                var selectedRowHandles = ProductServiceListCardView.GetSelectedRows();

                selectedDtos.AddRange((from rowHandle in selectedRowHandles
                    where rowHandle >= 0
                    select ProductServiceListCardView.GetRow(rowHandle)).OfType<ProductServiceDto>());
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi lấy danh sách sản phẩm được chọn");
            }

            return selectedDtos;
        }

        /// <summary>
        /// Chọn tất cả các card trong CardView
        /// </summary>
        private void SelectAllCards()
        {
            try
            {
                ProductServiceListCardView.SelectAll();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi chọn tất cả");
            }
        }

        /// <summary>
        /// Bỏ chọn tất cả các card
        /// </summary>
        private void DeselectAllCards()
        {
            try
            {
                ProductServiceListCardView.ClearSelection();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi bỏ chọn tất cả");
            }
        }

        /// <summary>
        /// Hiển thị thông tin
        /// </summary>
        private void ShowInfo(string message)
        {
            MsgBox.ShowSuccess(message);
        }

        /// <summary>
        /// Hiển thị lỗi với thông tin ngữ cảnh
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