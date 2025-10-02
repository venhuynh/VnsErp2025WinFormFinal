using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bll.MasterData.ProductServiceBll;
using Bll.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraSplashScreen;
using MasterData.ProductService.Dto;

namespace MasterData.ProductService
{
    public partial class UcProductVariant : DevExpress.XtraEditors.XtraUserControl
    {
        #region Fields

        private readonly ProductVariantBll _productVariantBll = new ProductVariantBll();
        private List<Guid> _selectedVariantIds = new List<Guid>();
        private bool _isLoading; // guard tránh gọi LoadDataAsync song song (Splash đã hiển thị)

        #endregion

        #region Constructor

        public UcProductVariant()
        {
            InitializeComponent();
            
            // Đăng ký event handlers
            ListDataBarButtonItem.ItemClick += ListDataBarButtonItem_ItemClick;
            NewBarButtonItem.ItemClick += NewBarButtonItem_ItemClick;
            EditBarButtonItem.ItemClick += EditBarButtonItem_ItemClick;
            DeleteBarButtonItem.ItemClick += DeleteBarButtonItem_ItemClick;
            CountVariantAndImageBarButtonItem.ItemClick += CountVariantAndImageBarButtonItem_ItemClick;
            ExportBarButtonItem.ItemClick += ExportBarButtonItem_ItemClick;
            DataFilterBtn.ItemClick += DataFilterBtn_ItemClick;

            // Grid events
            ProductServiceMasterDetailViewGridView.SelectionChanged += ProductServiceMasterDetailViewGridView_SelectionChanged;
            VariantGridView.SelectionChanged += VariantGridView_SelectionChanged;

            UpdateButtonStates();
        }

        #endregion

        #region Event Handlers

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
                // TODO: Implement new variant form
                ShowInfo("Chức năng thêm mới biến thể sẽ được triển khai sau.");
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
                // TODO: Implement edit variant form
                ShowInfo("Chức năng chỉnh sửa biến thể sẽ được triển khai sau.");
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi hiển thị màn hình điều chỉnh");
            }
        }

        /// <summary>
        /// Người dùng bấm "Xóa".
        /// </summary>
        private void DeleteBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                // TODO: Implement delete functionality
                ShowInfo("Chức năng xóa biến thể sẽ được triển khai sau.");
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi xóa dữ liệu");
            }
        }

        /// <summary>
        /// Người dùng bấm "Thống kê".
        /// </summary>
        private void CountVariantAndImageBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                // TODO: Implement count functionality
                ShowInfo("Chức năng thống kê sẽ được triển khai sau.");
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
                // TODO: Implement export functionality
                ShowInfo("Chức năng xuất dữ liệu sẽ được triển khai sau.");
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi xuất dữ liệu");
            }
        }

        /// <summary>
        /// Người dùng bấm "Tìm kiếm".
        /// </summary>
        private void DataFilterBtn_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                // TODO: Implement search functionality
                ShowInfo("Chức năng tìm kiếm sẽ được triển khai sau.");
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi thực hiện tìm kiếm");
            }
        }

        /// <summary>
        /// Grid selection thay đổi -> cập nhật danh sách Id đã chọn và trạng thái nút.
        /// </summary>
        private void ProductServiceMasterDetailViewGridView_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            try
            {
                // TODO: Implement selection handling for master grid
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
        private void VariantGridView_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            try
            {
                // TODO: Implement selection handling for variant grid
                UpdateButtonStates();
                UpdateStatusBar();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        #endregion

        #region Data Loading Methods

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
                // Lấy dữ liệu Entity từ BLL (tuân thủ Bll -> Entity)
                var variants = await _productVariantBll.GetAllWithDetailsAsync();
                
                // Convert Entity sang Master-Detail DTO trong GUI (tuân thủ Entity -> DTO)
                var masterDetailDtos = ProductVariantConverter.ConvertToMasterDetailView(variants);
                
                // Bind dữ liệu vào grid
                BindGrid(masterDetailDtos);
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi tải dữ liệu");
            }
        }

        /// <summary>
        /// Bind danh sách Master-Detail DTO vào Grid và cấu hình hiển thị.
        /// </summary>
        private void BindGrid(List<ProductServiceMasterDetailViewDto> data)
        {
            try
            {
                // Bind dữ liệu vào BindingSource
                productServiceMasterDetailViewDtoBindingSource.DataSource = data;
                
                // Cấu hình grid
                ProductServiceMasterDetailViewGridView.BestFitColumns();
                VariantGridView.BestFitColumns();
                
                // Cập nhật trạng thái
                UpdateButtonStates();
                UpdateStatusBar();
                
                // Hiển thị thông báo
                var totalVariants = data?.Sum(x => x.Variants?.Count ?? 0) ?? 0;
                ShowInfo($"Đã tải {data?.Count ?? 0} sản phẩm với {totalVariants} biến thể.");
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi bind dữ liệu vào grid");
            }
        }

        #endregion

        #region Helper Methods


        /// <summary>
        /// Thực hiện operation async với WaitingForm1 hiển thị.
        /// </summary>
        /// <param name="operation">Operation async cần thực hiện</param>
        private async Task ExecuteWithWaitingFormAsync(Func<Task> operation)
        {
            try
            {
                // Hiển thị WaitingForm1
                SplashScreenManager.ShowForm(typeof(Bll.Common.WaitForm1));

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

                var currentData = productServiceMasterDetailViewDtoBindingSource.DataSource as List<ProductServiceMasterDetailViewDto>;
                if (currentData == null || !currentData.Any())
                {
                    DataSummaryBarStaticItem.Caption = @"Chưa có dữ liệu";
                    return;
                }

                var productCount = currentData.Count;
                var activeProductCount = currentData.Count(x => x.ProductIsActive);
                var inactiveProductCount = currentData.Count(x => !x.ProductIsActive);
                var totalVariantCount = currentData.Sum(x => x.Variants?.Count ?? 0);

                var summary = $"<b>Sản phẩm: {productCount}</b> | " +
                             $"<b>Biến thể: {totalVariantCount}</b> | " +
                             $"<color=green>Hoạt động: {activeProductCount}</color> | " +
                             $"<color=red>Không hoạt động: {inactiveProductCount}</color>";

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
