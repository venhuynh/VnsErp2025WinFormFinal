using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bll.Common;
using Bll.MasterData.ProductServiceBll;
using Common.Helpers;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSplashScreen;
using MasterData.ProductService.Converters;
using MasterData.ProductService.Dto;

namespace MasterData.ProductService
{
    /// <summary>
    /// UserControl quản lý danh sách sản phẩm/dịch vụ.
    /// Cung cấp chức năng CRUD đầy đủ với GridView, pagination, tìm kiếm toàn diện và giao diện thân thiện.
    /// </summary>
    public partial class FrmProductServiceList : XtraForm
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
        public FrmProductServiceList()
        {
            InitializeComponent();

            // Toolbar events
            ListDataBarButtonItem.ItemClick += ListDataBarButtonItem_ItemClick;
            NewBarButtonItem.ItemClick += NewBarButtonItem_ItemClick;
            EditBarButtonItem.ItemClick += EditBarButtonItem_ItemClick;
            DeleteBarButtonItem.ItemClick += DeleteBarButtonItem_ItemClick;
            ExportBarButtonItem.ItemClick += ExportBarButtonItem_ItemClick;
            CountVariantAndImageBarButtonItem.ItemClick += CountVariantAndImageBarButtonItem_ItemClick;

            // Grid events
            ProductServiceAdvBandedGridView.SelectionChanged += ProductServiceAdvBandedGridView_SelectionChanged;
            ProductServiceAdvBandedGridView.CustomDrawRowIndicator +=
                ProductServiceAdvBandedGridView_CustomDrawRowIndicator;
            ProductServiceAdvBandedGridView.RowCellStyle += ProductServiceAdvBandedGridView_RowCellStyle;

            // Pagination events
            PageBarEditItem.EditValueChanged += PageBarEditItem_EditValueChanged;
            RecordNumberBarEditItem.EditValueChanged += RecordNumberBarEditItem_EditValueChanged;

            // Filter events
            DataFilterBtn.ItemClick += DataFilterBtn_ItemClick;

            // Initialize pagination control
            InitializePaginationControl();

            InitializeRecordNumberControl();

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
            await LoadDataAsync();
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

        #region ========== SỰ KIỆN GRID ==========

        /// <summary>
        /// Grid selection thay đổi -> cập nhật danh sách Id đã chọn và trạng thái nút
        /// </summary>
        private void ProductServiceAdvBandedGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                _selectedProductServiceIds =
                    GridViewHelper.GetSelectedRowColumnValues<Guid>(sender, nameof(ProductServiceDto.Id));
                UpdateButtonStates();
                UpdateStatusBar();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        /// <summary>
        /// Vẽ STT dòng cho GridView
        /// </summary>
        private void ProductServiceAdvBandedGridView_CustomDrawRowIndicator(object sender,
            RowIndicatorCustomDrawEventArgs rowIndicatorCustomDrawEventArgs)
        {
            // Sử dụng helper chung để vẽ số thứ tự dòng
            GridViewHelper.CustomDrawRowIndicator(ProductServiceAdvBandedGridView, rowIndicatorCustomDrawEventArgs);
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
                // Export: chỉ khi có dữ liệu hiển thị
                var rowCount = GridViewHelper.GetDisplayRowCount(ProductServiceAdvBandedGridView) ?? 0;
                if (ExportBarButtonItem != null)
                    ExportBarButtonItem.Enabled = rowCount > 0;
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
                    SelectedRowBarStaticItem.Caption = @"Chưa chọn dòng nào";
                }
                else if (selectedCount == 1)
                {
                    SelectedRowBarStaticItem.Caption = @"Đang chọn 1 dòng";
                }
                else
                {
                    SelectedRowBarStaticItem.Caption = $@"Đang chọn {selectedCount} dòng";
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

                string searchKeyword = "";

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
                if (selectedOption == "Tìm kiếm đơn giản")
                    return SearchOption.SimpleSearch;
                if (selectedOption == "Tìm kiếm nâng cao")
                    return SearchOption.AdvancedSearch;
                return SearchOption.Cancel;
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

        #endregion

        #region ========== QUẢN LÝ DỮ LIỆU ==========

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

        #endregion

        #region ========== CẤU HÌNH GRID ==========

        /// <summary>
        /// Cấu hình AdvBandedGridView để hiển thị dữ liệu xuống dòng (word wrap) cho các cột văn bản dài.
        /// Đồng thời bật tự động tính chiều cao dòng để hiển thị đầy đủ nội dung.
        /// </summary>
        private void ConfigureMultiLineGridView()
        {
            try
            {
                // Bật tự động điều chỉnh chiều cao dòng để wrap nội dung
                ProductServiceAdvBandedGridView.OptionsView.RowAutoHeight = true;

                // RepositoryItemMemoEdit cho wrap text
                var memo = new RepositoryItemMemoEdit
                {
                    WordWrap = true,
                    AutoHeight = false
                };
                memo.Appearance.TextOptions.WordWrap = WordWrap.Wrap;

                // Áp dụng cho các cột có khả năng dài
                //ApplyMemoEditorToColumn("Name", memo);
                //ApplyMemoEditorToColumn("Description", memo);
                //ApplyMemoEditorToColumn("CategoryName", memo);

                // Cấu hình cột ảnh thumbnail
                ConfigureThumbnailColumn();

                // Cấu hình filter và search
                ConfigureFilterAndSearch();

                // Tùy chọn hiển thị: căn giữa tiêu đề cho đẹp
                ProductServiceAdvBandedGridView.Appearance.HeaderPanel.TextOptions.HAlignment = HorzAlignment.Center;
                ProductServiceAdvBandedGridView.Appearance.HeaderPanel.Options.UseTextOptions = true;
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// Cấu hình cột ảnh thumbnail
        /// </summary>
        private void ConfigureThumbnailColumn()
        {
            try
            {
                // RepositoryItemPictureEdit cho hiển thị ảnh
                var pictureEdit = new RepositoryItemPictureEdit
                {
                    SizeMode = PictureSizeMode.Stretch
                };

                // Thêm repository vào GridControl
                if (!ProductServiceListGridControl.RepositoryItems.Contains(pictureEdit))
                {
                    ProductServiceListGridControl.RepositoryItems.Add(pictureEdit);
                }

                // Áp dụng cho cột thumbnail
                colThumbnail.ColumnEdit = pictureEdit;
                colThumbnail.OptionsColumn.FixedWidth = true;
                colThumbnail.Width = 80;
                colThumbnail.Caption = @"Ảnh đại diện";
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// Cấu hình filter và search cho grid
        /// </summary>
        private void ConfigureFilterAndSearch()
        {
            try
            {
                // Bật filter row
                ProductServiceAdvBandedGridView.OptionsView.ShowAutoFilterRow = true;

                // Bật search
                ProductServiceAdvBandedGridView.OptionsFind.AlwaysVisible = true;
                ProductServiceAdvBandedGridView.OptionsFind.FindMode = FindMode.Always;
                ProductServiceAdvBandedGridView.OptionsFind.FindNullPrompt = "Tìm kiếm trong tất cả dữ liệu...";

                // Cấu hình filter cho từng cột
                ConfigureColumnFilters();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// Cấu hình filter cho từng cột
        /// </summary>
        private void ConfigureColumnFilters()
        {
            try
            {
                // Cột Code - text filter
                if (colCode != null)
                {
                    colCode.OptionsFilter.AutoFilterCondition = AutoFilterCondition.Contains;
                }

                // Cột Name - text filter
                if (colName != null)
                {
                    colName.OptionsFilter.AutoFilterCondition = AutoFilterCondition.Contains;
                }

                // Cột CategoryName - list filter
                if (colCategoryName != null)
                {
                    colCategoryName.OptionsFilter.AutoFilterCondition = AutoFilterCondition.Contains;
                }

                // Cột TypeDisplay - list filter
                if (colTypeDisplay != null)
                {
                    colTypeDisplay.OptionsFilter.AutoFilterCondition = AutoFilterCondition.Contains;
                }

                // Cột StatusDisplay - list filter
                if (colStatusDisplay != null)
                {
                    colStatusDisplay.OptionsFilter.AutoFilterCondition = AutoFilterCondition.Contains;
                }

                // Cột IsActive - checkbox filter
                if (colIsActive != null)
                {
                    colIsActive.OptionsFilter.AutoFilterCondition = AutoFilterCondition.Equals;
                }

                // Cột Description - text filter
                if (colDescription != null)
                {
                    colDescription.OptionsFilter.AutoFilterCondition = AutoFilterCondition.Contains;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void ApplyMemoEditorToColumn(string fieldName, RepositoryItemMemoEdit memo)
        {
            var col = ProductServiceAdvBandedGridView.Columns[fieldName];
            if (col == null) return;
            // Thêm repository vào GridControl nếu chưa có
            if (!ProductServiceListGridControl.RepositoryItems.Contains(memo))
            {
                ProductServiceListGridControl.RepositoryItems.Add(memo);
            }

            col.ColumnEdit = memo;
        }

        /// <summary>
        /// Tô màu/định dạng dòng theo trạng thái/loại sản phẩm/dịch vụ
        /// </summary>
        private void ProductServiceAdvBandedGridView_RowCellStyle(object sender,
            RowCellStyleEventArgs rowCellStyleEventArgs)
        {
            try
            {
                var view = sender as AdvBandedGridView;
                if (view == null) return;
                if (rowCellStyleEventArgs.RowHandle < 0) return;
                var row = view.GetRow(rowCellStyleEventArgs.RowHandle) as ProductServiceDto;
                if (row == null) return;
                // Không ghi đè màu khi đang chọn để giữ màu chọn mặc định của DevExpress
                if (view.IsRowSelected(rowCellStyleEventArgs.RowHandle)) return;
                // Nền theo loại (IsService) với màu tương phản rõ
                Color backColor;
                if (row.IsService)
                {
                    backColor = Color.LightCyan; // xanh nhạt cho dịch vụ
                }
                else
                {
                    backColor = Color.LightYellow; // vàng nhạt cho sản phẩm
                }

                rowCellStyleEventArgs.Appearance.BackColor = backColor;
                rowCellStyleEventArgs.Appearance.ForeColor =
                    Color.Black; // chữ đen tương phản tốt trên các màu nền trên
                rowCellStyleEventArgs.Appearance.Options.UseBackColor = true;
                rowCellStyleEventArgs.Appearance.Options.UseForeColor = true;

                // Nếu sản phẩm/dịch vụ không hoạt động: làm nổi bật rõ ràng hơn
                if (!row.IsActive)
                {
                    //rowCellStyleEventArgs.Appearance.BackColor = System.Drawing.Color.FromArgb(255, 205, 210); // đỏ nhạt nhưng đậm hơn (Light Red)
                    rowCellStyleEventArgs.Appearance.ForeColor = Color.DarkRed;
                    rowCellStyleEventArgs.Appearance.Font =
                        new Font(rowCellStyleEventArgs.Appearance.Font, FontStyle.Strikeout);
                }
            }
            catch (Exception)
            {
                // ignore style errors
            }
        }

        #endregion

        #region ========== SỰ KIỆN CRUD ==========

        /// <summary>
        /// Người dùng bấm "Điều chỉnh"
        /// </summary>
        private async void EditBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
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
            var dto = ProductServiceAdvBandedGridView.GetFocusedRow() as ProductServiceDto;
            if (dto == null || dto.Id != id)
            {
                // Tìm đúng DTO theo Id trong datasource nếu FocusedRow không khớp selection
                if (productServiceDtoBindingSource.DataSource is IEnumerable list)
                {
                    foreach (var item in list)
                    {
                        if (item is ProductServiceDto x && x.Id == id)
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

        /// <summary>
        /// Người dùng bấm "Xóa"
        /// </summary>
        private async void DeleteBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
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

        /// <summary>
        /// Người dùng bấm "Xuất"
        /// </summary>
        private void ExportBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Chỉ cho phép xuất khi có dữ liệu hiển thị
            var rowCount = GridViewHelper.GetDisplayRowCount(ProductServiceAdvBandedGridView) ?? 0;
            if (rowCount <= 0)
            {
                ShowInfo("Không có dữ liệu để xuất.");
                return;
            }

            GridViewHelper.ExportGridControl(ProductServiceAdvBandedGridView, "ProductServices.xlsx");
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

                // Hiển thị thông báo cho user biết đang xử lý

                // Thực hiện đếm với splash screen
                _ = ExecuteWithWaitingFormAsync(async () => { await CountSelectedProductsAsync(); });
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khi đếm số lượng biến thể và hình ảnh");
            }
        }

        #endregion

        #region ========== TẢI DỮ LIỆU ==========

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

                    // Update pagination control first
                    UpdatePaginationControl();

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

                // Update pagination control (disable pagination)
                UpdatePaginationControl();
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

                // Update pagination control
                UpdatePaginationControl();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi tải trang dữ liệu");
            }
        }

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
        /// Bind danh sách DTO vào Grid và cấu hình hiển thị
        /// </summary>
        private void BindGrid(List<ProductServiceDto> data)
        {
            productServiceDtoBindingSource.DataSource = data;
            ProductServiceAdvBandedGridView.BestFitColumns();
            ConfigureMultiLineGridView();
            UpdateButtonStates();
            UpdateStatusBar();
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
                ProductServiceAdvBandedGridView.BestFitColumns();
                ConfigureMultiLineGridView();
                UpdateButtonStates();
                UpdateStatusBar();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi bind dữ liệu với highlight vào grid");
            }
        }


        /// <summary>
        /// Xóa trạng thái chọn hiện tại trên Grid
        /// </summary>
        private void ClearSelectionState()
        {
            _selectedProductServiceIds.Clear();
            ProductServiceAdvBandedGridView.ClearSelection();
            ProductServiceAdvBandedGridView.FocusedRowHandle = GridControl.InvalidRowHandle;
            UpdateButtonStates();
            UpdateStatusBar();
        }

        #endregion

        #region ========== PHÂN TRANG ==========

        /// <summary>
        /// Khởi tạo pagination control
        /// </summary>
        private void InitializePaginationControl()
        {
            try
            {
                // Cấu hình ComboBox cho pagination
                repositoryItemComboBox1.Items.Clear();
                repositoryItemComboBox1.TextEditStyle = TextEditStyles.DisableTextEditor;

                // Set initial value
                PageBarEditItem.EditValue = "1";
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khởi tạo pagination control");
            }
        }

        /// <summary>
        /// Khởi tạo record number control
        /// </summary>
        private void InitializeRecordNumberControl()
        {
            try
            {
                // Cấu hình ComboBox cho record number
                repositoryItemComboBox2.Items.Clear();
                repositoryItemComboBox2.TextEditStyle = TextEditStyles.DisableTextEditor;

                // Thêm các options: "20", "50", "100", "Tất cả"
                repositoryItemComboBox2.Items.AddRange(new object[] { "20", "50", "100", "Tất cả" });

                // Set initial value
                RecordNumberBarEditItem.EditValue = "50";
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khởi tạo record number control");
            }
        }

        /// <summary>
        /// Cập nhật pagination control với danh sách trang
        /// </summary>
        private void UpdatePaginationControl()
        {
            try
            {
                if (PageBarEditItem?.Edit == null) return;

                // Clear existing items
                repositoryItemComboBox1.Items.Clear();

                // Xử lý trường hợp "Tất cả" (không phân trang)
                if (_pageSize == int.MaxValue)
                {
                    repositoryItemComboBox1.Items.Add("1");
                    PageBarEditItem.EditValue = "1";
                    PageBarEditItem.Enabled = false; // Disable pagination
                }
                else
                {
                    // Add page numbers
                    for (int i = 1; i <= _totalPages; i++)
                    {
                        repositoryItemComboBox1.Items.Add(i.ToString());
                    }

                    // Set current page
                    var currentPageNumber = _currentPageIndex + 1;
                    PageBarEditItem.EditValue = currentPageNumber.ToString();

                    // Enable/disable based on total pages
                    PageBarEditItem.Enabled = _totalPages > 1;
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi cập nhật pagination control");
            }
        }

        /// <summary>
        /// Event handler khi user thay đổi trang trong pagination control
        /// </summary>
        private async void PageBarEditItem_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (PageBarEditItem.EditValue == null) return;

                var pageNumberText = PageBarEditItem.EditValue.ToString();
                if (int.TryParse(pageNumberText, out int pageNumber))
                {
                    var pageIndex = pageNumber - 1; // Convert to 0-based index

                    if (pageIndex >= 0 && pageIndex < _totalPages && pageIndex != _currentPageIndex)
                    {
                        // Hiển thị WaitingForm1 giống như nút List
                        await ExecuteWithWaitingFormAsync(async () =>
                        {
                            await LoadPageAsync(pageIndex);
                            UpdateStatusBar();
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi chuyển trang");
            }
        }

        /// <summary>
        /// Event handler khi user thay đổi số dòng trên mỗi trang
        /// </summary>
        private async void RecordNumberBarEditItem_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (RecordNumberBarEditItem.EditValue == null) return;

                var recordNumberText = RecordNumberBarEditItem.EditValue.ToString();
                var newPageSize = ParseRecordNumber(recordNumberText);

                if (newPageSize > 0 && newPageSize != _pageSize)
                {
                    _pageSize = newPageSize;
                    _currentPageIndex = 0; // Reset về trang đầu

                    // Hiển thị WaitingForm1 giống như nút List
                    await ExecuteWithWaitingFormAsync(async () =>
                    {
                        // Reload data với page size mới
                        await LoadDataAsyncWithoutSplash();
                    });
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi thay đổi số dòng trên trang");
            }
        }

        /// <summary>
        /// Parse record number text thành page size
        /// </summary>
        /// <param name="recordNumberText">Text từ combo box</param>
        /// <returns>Page size (0 nếu không hợp lệ)</returns>
        private int ParseRecordNumber(string recordNumberText)
        {
            if (string.IsNullOrWhiteSpace(recordNumberText))
                return 0;

            switch (recordNumberText.Trim())
            {
                case "20":
                    return 20;
                case "50":
                    return 50;
                case "100":
                    return 100;
                case "Tất cả":
                    return int.MaxValue; // Load tất cả dữ liệu
                default:
                    // Thử parse số
                    if (int.TryParse(recordNumberText, out int number) && number > 0)
                        return number;
                    return 0;
            }
        }


        #endregion

        #region ========== TIỆN ÍCH ==========

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
