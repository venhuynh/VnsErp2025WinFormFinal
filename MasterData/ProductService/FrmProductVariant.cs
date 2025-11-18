using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bll.Common;
using Bll.MasterData.ProductServiceBll;
using Common.Common;
using Common.Helpers;
using Common.Utils;
using Dal.DataContext;
using DevExpress.Data;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSplashScreen;
using DTO.MasterData.ProductService;

namespace MasterData.ProductService
{
    /// <summary>
    /// UserControl quản lý danh sách biến thể sản phẩm.
    /// Cung cấp chức năng CRUD đầy đủ với giao diện thân thiện.
    /// </summary>
    public partial class FrmProductVariant : XtraForm
    {
        #region ========== KHAI BÁO BIẾN ==========

        /// <summary>
        /// Business Logic Layer cho biến thể sản phẩm
        /// </summary>
        private readonly ProductVariantBll _productVariantBll = new ProductVariantBll();

        /// <summary>
        /// Danh sách ID biến thể đang được chọn
        /// </summary>
        private List<Guid> _selectedVariantIds = new List<Guid>();

        /// <summary>
        /// Trạng thái đang tải dữ liệu (guard tránh gọi LoadDataAsync song song)
        /// </summary>
        private bool _isLoading;

        /// <summary>
        /// Trạng thái splash screen đang hiển thị (guard tránh hiển thị splash screen nhiều lần)
        /// </summary>
        private bool _isSplashVisible;

        #endregion

        #region ========== CONSTRUCTOR & PUBLIC METHODS ==========

        /// <summary>
        /// Khởi tạo UserControl quản lý biến thể sản phẩm.
        /// </summary>
        public FrmProductVariant()
        {
            InitializeComponent();
            
            // Đăng ký event handlers
            ListDataBarButtonItem.ItemClick += ListDataBarButtonItem_ItemClick;
            NewBarButtonItem.ItemClick += NewBarButtonItem_ItemClick;
            EditBarButtonItem.ItemClick += EditBarButtonItem_ItemClick;
            DeleteBarButtonItem.ItemClick += DeleteBarButtonItem_ItemClick;
            CountVariantAndImageBarButtonItem.ItemClick += CountVariantAndImageBarButtonItem_ItemClick;
            ExportBarButtonItem.ItemClick += ExportBarButtonItem_ItemClick;

            // Grid events
            ProductVariantListGridView.SelectionChanged += ProductServiceMasterDetailViewGridView_SelectionChanged;
            ProductVariantListGridView.CustomDrawRowIndicator += ProductVariantListGridView_CustomDrawRowIndicator;
            VariantGridView.SelectionChanged += VariantGridView_SelectionChanged;

            // Thiết lập SuperToolTip cho các controls
            SetupSuperToolTips();

            UpdateButtonStates();
        }

        #endregion

        #region ========== SỰ KIỆN BUTTON ==========

        /// <summary>
        /// Người dùng bấm "Danh sách" để tải dữ liệu.
        /// </summary>
        private async void ListDataBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            await LoadDataAsync();
        }

        /// <summary>
        /// Người dùng bấm "Mới".
        /// </summary>
        private void NewBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                // Mở form thêm mới biến thể
                var form = new FrmProductVariantDetail(Guid.Empty);
                form.ShowDialog();

                // Refresh dữ liệu sau khi đóng form (luôn refresh để đảm bảo dữ liệu mới nhất)
                ListDataBarButtonItem.PerformClick();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi hiển thị màn hình thêm mới");
            }
        }

        /// <summary>
        /// Người dùng bấm "Điều chỉnh".
        /// </summary>
        private void EditBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                // Kiểm tra có chọn đúng 1 dòng không
                if (_selectedVariantIds == null || _selectedVariantIds.Count != 1)
                {
                    ShowInfo("Vui lòng chọn đúng 1 biến thể để chỉnh sửa.");
                    return;
                }

                // Lấy ID biến thể đã chọn
                var variantId = _selectedVariantIds.First();
                
                // Mở form chỉnh sửa biến thể
                var form = new FrmProductVariantDetail(variantId);
                form.ShowDialog();
                
                // Refresh dữ liệu sau khi đóng form (sử dụng SmartRefreshAsync để tránh ObjectDisposedException)
                ListDataBarButtonItem.PerformClick();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi hiển thị màn hình điều chỉnh");
            }
        }

        /// <summary>
        /// Người dùng bấm "Xóa".
        /// </summary>
        private async void DeleteBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                // Kiểm tra có chọn dòng nào không
                if (_selectedVariantIds == null || _selectedVariantIds.Count == 0)
                {
                    ShowInfo("Vui lòng chọn biến thể cần xóa.");
                    return;
                }

                // Xác nhận xóa
                var selectedCount = _selectedVariantIds.Count;
                var message = selectedCount == 1 
                    ? "Bạn có chắc chắn muốn xóa biến thể đã chọn?" 
                    : $"Bạn có chắc chắn muốn xóa {selectedCount} biến thể đã chọn?";
                
                if (!MsgBox.ShowYesNo(message, "Xác nhận xóa"))
                {
                    return;
                }

                // Thực hiện xóa
                await ExecuteWithWaitingFormAsync(async () =>
                {
                    var deletedCount = 0;
                    var errorCount = 0;
                    var errors = new List<string>();

                    foreach (var variantId in _selectedVariantIds)
                    {
                        try
                        {
                            await _productVariantBll.DeleteAsync(variantId);
                            deletedCount++;
                        }
                        catch (Exception ex)
                        {
                            errorCount++;
                            errors.Add($"ID {variantId}: {ex.Message}");
                        }
                    }

                    // Hiển thị kết quả
                    if (deletedCount > 0 && errorCount == 0)
                    {
                        ShowInfo($"Đã xóa thành công {deletedCount} biến thể.");
                        // Sử dụng LoadDataAsyncWithoutSplash để tránh nested splash screen
                        await LoadDataAsyncWithoutSplash();
                        
                        // Clear selection và cập nhật UI sau khi xóa thành công
                        _selectedVariantIds.Clear();
                        UpdateButtonStates();
                        UpdateStatusBar();
                    }
                    else if (deletedCount > 0 && errorCount > 0)
                    {
                        var errorMessage = string.Join("\n", errors);
                        ShowError(new Exception($"Xóa thành công {deletedCount} biến thể, lỗi {errorCount} biến thể:\n{errorMessage}"));
                        // Sử dụng LoadDataAsyncWithoutSplash để tránh nested splash screen
                        await LoadDataAsyncWithoutSplash();
                        
                        // Clear selection và cập nhật UI sau khi xóa một phần thành công
                        _selectedVariantIds.Clear();
                        UpdateButtonStates();
                        UpdateStatusBar();
                    }
                    else
                    {
                        var errorMessage = string.Join("\n", errors);
                        ShowError(new Exception($"Không thể xóa biến thể nào:\n{errorMessage}"));
                        // Không clear selection nếu xóa thất bại hoàn toàn
                    }
                });
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi xóa dữ liệu");
            }
        }

        /// <summary>
        /// Người dùng bấm "Thống kê".
        /// </summary>
        private async void CountVariantAndImageBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                // Kiểm tra có chọn dòng nào không
                if (_selectedVariantIds == null || _selectedVariantIds.Count == 0)
                {
                    ShowInfo("Vui lòng chọn biến thể cần thống kê.");
                    return;
                }

                // Thực hiện thống kê
                await ExecuteWithWaitingFormAsync(async () =>
                {
                    var selectedCount = _selectedVariantIds.Count;
                    var totalImageCount = 0;
                    var activeVariantCount = 0;
                    var inactiveVariantCount = 0;
                    var errors = new List<string>();

                    foreach (var variantId in _selectedVariantIds)
                    {
                        try
                        {
                            // Lấy thông tin biến thể
                            var variant = await _productVariantBll.GetByIdAsync(variantId);
                            if (variant != null)
                            {
                                // Đếm hình ảnh (tạm thời set 0 vì không load được navigation properties)
                                totalImageCount += 0; // variant.ProductImages?.Count ?? 0;
                                
                                // Đếm trạng thái
                                if (variant.IsActive)
                                    activeVariantCount++;
                                else
                                    inactiveVariantCount++;
                            }
                        }
                        catch (Exception ex)
                        {
                            errors.Add($"ID {variantId}: {ex.Message}");
                        }
                    }

                    // Hiển thị kết quả thống kê
                    var result = $"<b>Thống kê {selectedCount} biến thể đã chọn:</b>\n\n" +
                               $"• <color=green>Hoạt động: {activeVariantCount}</color>\n" +
                               $"• <color=red>Không hoạt động: {inactiveVariantCount}</color>\n" +
                               $"• <b>Tổng hình ảnh: {totalImageCount}</b>";

                    if (errors.Any())
                    {
                        result += $"\n\n<color=red>Lỗi khi thống kê:</color>\n{string.Join("\n", errors)}";
                    }

                    ShowInfo(result);
                });
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khi thống kê");
            }
        }

        /// <summary>
        /// Người dùng bấm "Xuất".
        /// </summary>
        private void ExportBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                GridViewHelper.ExportGridControl(ProductVariantListGridView, $"ProductVariants_{DateTime.Now:yyyyMMdd_HHmmss}");
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi xuất dữ liệu");
            }
        }

        /// <summary>
        /// Grid selection thay đổi -> cập nhật danh sách Id đã chọn và trạng thái nút.
        /// </summary>
        private void ProductServiceMasterDetailViewGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                UpdateSelectedVariantIds();
                UpdateButtonStates();
                UpdateStatusBar();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        /// <summary>
        /// Variant grid selection thay đổi -> cập nhật danh sách Id đã chọn và trạng thái nút.
        /// </summary>
        private void VariantGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                UpdateSelectedVariantIds();
                UpdateButtonStates();
                UpdateStatusBar();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        /// <summary>
        /// Custom draw row indicator để hiển thị số thứ tự dòng
        /// </summary>
        private void ProductVariantListGridView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                // Chỉ hiển thị số thứ tự cho data rows, không hiển thị cho group rows
                if (e.Info.IsRowIndicator && e.RowHandle >= 0)
                {
                    // Tính số thứ tự (bắt đầu từ 1)
                    var rowNumber = e.RowHandle + 1;
                    
                    // Hiển thị số thứ tự
                    e.Info.DisplayText = rowNumber.ToString();
                }
            }
            catch (Exception)
            {
                // Nếu có lỗi, hiển thị text mặc định
                e.Info.DisplayText = "";
            }
        }

        /// <summary>
        /// Cập nhật danh sách ID biến thể đã chọn
        /// </summary>
        private void UpdateSelectedVariantIds()
        {
            try
            {
                _selectedVariantIds.Clear();
                
                var selectedRows = ProductVariantListGridView.GetSelectedRows();
                foreach (var rowHandle in selectedRows)
                {
                    if (rowHandle >= 0)
                    {
                        var dto = ProductVariantListGridView.GetRow(rowHandle) as ProductVariantListDto;
                        if (dto != null)
                        {
                            _selectedVariantIds.Add(dto.Id);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi cập nhật danh sách đã chọn");
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
        /// Sử dụng ProductVariantListDto cho danh sách biến thể.
        /// </summary>
        private async Task LoadDataAsyncWithoutSplash()
        {
            try
            {
                // Lấy dữ liệu Entity từ BLL (tuân thủ Bll -> Entity)
                // Sử dụng GetAllAsync để tránh lỗi ObjectDisposedException
                var variants = await _productVariantBll.GetAllAsync();
                
                // Convert Entity sang ProductVariantListDto trong GUI (tuân thủ Entity -> DTO)
                var variantListDtos = await ConvertToVariantListDtosAsync(variants);
                
                // Bind dữ liệu vào grid
                BindGrid(variantListDtos);
                
                // Clear selection và cập nhật UI sau khi load dữ liệu mới
                _selectedVariantIds.Clear();
                UpdateButtonStates();
                UpdateStatusBar();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi tải dữ liệu");
            }
        }




        /// <summary>
        /// Convert Entity sang ProductVariantListDto (Async)
        /// </summary>
        private async Task<List<ProductVariantListDto>> ConvertToVariantListDtosAsync(List<ProductVariant> variants)
        {
            try
            {
                var result = new List<ProductVariantListDto>();
                
                foreach (var variant in variants)
                {
                    var dto = new ProductVariantListDto
                    {
                        Id = variant.Id,
                        ProductCode = variant.ProductService?.Code ?? "",
                        ProductName = variant.ProductService?.Name ?? "",
                        VariantCode = variant.VariantCode,
                        VariantFullName = !string.IsNullOrWhiteSpace(variant.VariantFullName) 
                            ? variant.VariantFullName 
                            : await BuildVariantFullNameAsync(variant), // Fallback nếu VariantFullName chưa được cập nhật
                        UnitName = variant.UnitOfMeasure?.Name ?? "",
                        IsActive = variant.IsActive,
                        ImageCount = 0 // Tạm thời set 0 vì không load được navigation properties
                    };
                    
                    result.Add(dto);
                }
                
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi convert sang ProductVariantListDto: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Xây dựng tên đầy đủ của biến thể từ các thuộc tính (Async)
        /// Format: Attribute1: Value1, Attribute2: Value2, ...
        /// </summary>
        private Task<string> BuildVariantFullNameAsync(ProductVariant variant)
        {
            try
            {
                // Load thông tin thuộc tính từ BLL
                var attributeValues = _productVariantBll.GetAttributeValues(variant.Id);
                
                if (attributeValues == null || !attributeValues.Any())
                {
                    return Task.FromResult(variant.VariantCode); // Nếu không có thuộc tính, trả về mã biến thể
                }

                var attributeParts = new List<string>();
                
                foreach (var (_, attributeName, value) in attributeValues)
                {
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        attributeParts.Add($"{attributeName}: {value}");
                    }
                }

                if (attributeParts.Any())
                {
                    return Task.FromResult(string.Join(", ", attributeParts));
                }

                return Task.FromResult(variant.VariantCode); // Fallback về mã biến thể nếu không có giá trị thuộc tính
            }
            catch (Exception)
            {
                // Nếu có lỗi, trả về mã biến thể
                return Task.FromResult(variant.VariantCode);
            }
        }


        /// <summary>
        /// Bind danh sách ProductVariantListDto vào Grid và cấu hình hiển thị.
        /// </summary>
        private void BindGrid(List<ProductVariantListDto> data)
        {
            try
            {
                // Bind dữ liệu vào BindingSource
                productVariantListDtoBindingSource.DataSource = data;
                
                // Bind vào GridControl
                ProductVariantListGridControl.DataSource = productVariantListDtoBindingSource;
                
                // Cấu hình grid
                ProductVariantListGridView.BestFitColumns();
                
                // Cập nhật trạng thái
                UpdateButtonStates();
                UpdateStatusBar();
                
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi bind dữ liệu vào grid");
            }
        }


        #endregion

        #region ========== TIỆN ÍCH ==========

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
                        title: "<b><color=Blue>📋 Danh sách</color></b>",
                        content: "Tải lại danh sách biến thể sản phẩm từ hệ thống."
                    );
                }

                if (NewBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        NewBarButtonItem,
                        title: "<b><color=Green>➕ Mới</color></b>",
                        content: "Thêm mới biến thể sản phẩm vào hệ thống."
                    );
                }

                if (EditBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        EditBarButtonItem,
                        title: "<b><color=Orange>✏️ Điều chỉnh</color></b>",
                        content: "Chỉnh sửa thông tin biến thể sản phẩm đã chọn."
                    );
                }

                if (DeleteBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        DeleteBarButtonItem,
                        title: "<b><color=Red>🗑️ Xóa</color></b>",
                        content: "Xóa các biến thể sản phẩm đã chọn khỏi hệ thống."
                    );
                }

                if (CountVariantAndImageBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        CountVariantAndImageBarButtonItem,
                        title: "<b><color=Purple>📊 Thống kê</color></b>",
                        content: "Thống kê số lượng hình ảnh và trạng thái cho các biến thể được chọn."
                    );
                }

                if (ExportBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        ExportBarButtonItem,
                        title: "<b><color=Purple>📤 Xuất</color></b>",
                        content: "Xuất danh sách biến thể sản phẩm ra file Excel."
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
        /// Thực hiện operation async với WaitingForm1 hiển thị.
        /// </summary>
        /// <param name="operation">Operation async cần thực hiện</param>
        private async Task ExecuteWithWaitingFormAsync(Func<Task> operation)
        {
            // Kiểm tra splash screen đã hiển thị chưa
            if (_isSplashVisible)
            {
                // Nếu đã hiển thị, chỉ thực hiện operation mà không hiển thị splash
                await operation();
                return;
            }

            try
            {
                // Đánh dấu splash screen đang hiển thị
                _isSplashVisible = true;
                
                // Hiển thị WaitingForm1
                SplashScreenManager.ShowForm(typeof(WaitForm1));

                // Thực hiện operation
                await operation();
            }
            finally
            {
                // Đóng WaitingForm1
                SplashScreenManager.CloseForm();
                
                // Đánh dấu splash screen đã đóng
                _isSplashVisible = false;
            }
        }

        /// <summary>
        /// Cập nhật trạng thái các nút
        /// </summary>
        private void UpdateButtonStates()
        {
            try
            {
                var selectedCount = _selectedVariantIds?.Count ?? 0;
                
                // Edit: chỉ khi chọn đúng 1 dòng
                if (EditBarButtonItem != null)
                    EditBarButtonItem.Enabled = selectedCount == 1;
                    
                // Delete: khi chọn >= 1 dòng
                if (DeleteBarButtonItem != null)
                    DeleteBarButtonItem.Enabled = selectedCount >= 1;
                    
                // Count: chỉ khi chọn >= 1 dòng
                if (CountVariantAndImageBarButtonItem != null)
                    CountVariantAndImageBarButtonItem.Enabled = selectedCount >= 1;
                    
                // Export: luôn enable (có thể xuất tất cả dữ liệu)
                if (ExportBarButtonItem != null)
                    ExportBarButtonItem.Enabled = true;
            }
            catch
            {
                // ignore
            }
        }

        /// <summary>
        /// Cập nhật status bar
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
        /// Cập nhật thông tin số dòng đang được chọn.
        /// </summary>
        private void UpdateSelectedRowStatus()
        {
            try
            {
                if (SelectedRowBarStaticItem == null) return;

                var selectedCount = _selectedVariantIds?.Count ?? 0;
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
        /// Cập nhật thông tin tổng kết dữ liệu.
        /// </summary>
        private void UpdateDataSummaryStatus()
        {
            try
            {
                if (DataSummaryBarStaticItem == null) return;

                var currentData = productVariantListDtoBindingSource.DataSource as List<ProductVariantListDto>;
                if (currentData == null || !currentData.Any())
                {
                    DataSummaryBarStaticItem.Caption = @"Chưa có dữ liệu";
                    return;
                }

                var variantCount = currentData.Count;
                var activeVariantCount = currentData.Count(x => x.IsActive);
                var inactiveVariantCount = currentData.Count(x => !x.IsActive);
                var totalImageCount = currentData.Sum(x => x.ImageCount);

                var summary = $"<b>Biến thể: {variantCount}</b> | " +
                             $"<color=green>Hoạt động: {activeVariantCount}</color> | " +
                             $"<color=red>Không hoạt động: {inactiveVariantCount}</color> | " +
                             $"<b>Hình ảnh: {totalImageCount}</b>";

                DataSummaryBarStaticItem.Caption = summary;
            }
            catch
            {
                // ignore
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

        #endregion
    }
}
