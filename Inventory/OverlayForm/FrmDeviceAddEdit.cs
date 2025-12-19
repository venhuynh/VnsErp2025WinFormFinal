using Bll.MasterData.ProductServiceBll;
using Common.Common;
using Common.Utils;
using Dal.DataContext;
using DevExpress.XtraEditors;
using DTO.MasterData.ProductService;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventory.OverlayForm
{
    public partial class FrmDeviceAddEdit : DevExpress.XtraEditors.XtraForm
    {
        #region ========== FIELDS & PROPERTIES ==========

        /// <summary>
        /// Business Logic Layer cho biến thể sản phẩm
        /// </summary>
        private readonly ProductVariantBll _productVariantBll = new ProductVariantBll();

        /// <summary>
        /// Logger để ghi log các sự kiện
        /// </summary>
        private readonly ILogger _logger = LoggerFactory.CreateLogger(LogCategory.UI);

        /// <summary>
        /// Trạng thái đang tải dữ liệu (guard tránh gọi LoadProductVariantsAsync song song)
        /// </summary>
        private bool _isLoadingProductVariants;

        /// <summary>
        /// Flag đánh dấu ProductVariant datasource đã được load chưa
        /// </summary>
        private bool _isProductVariantDataSourceLoaded;

        #endregion

        #region ========== CONSTRUCTOR ==========

        public FrmDeviceAddEdit()
        {
            InitializeComponent();
            Load += FrmDeviceAddEdit_Load;
            SetupEvents();
        }

        /// <summary>
        /// Setup các event handlers
        /// </summary>
        private void SetupEvents()
        {
        }

        #endregion

        #region ========== INITIALIZATION ==========

        /// <summary>
        /// Event handler khi form được load
        /// </summary>
        private async void FrmDeviceAddEdit_Load(object sender, EventArgs e)
        {
            try
            {
                _logger.Debug("FrmDeviceAddEdit_Load: Form loading");

                // Load datasource cho ProductVariant
                await LoadProductVariantsAsync();

                _logger.Info("FrmDeviceAddEdit_Load: Form loaded successfully");
            }
            catch (Exception ex)
            {
                _logger.Error("FrmDeviceAddEdit_Load: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi khởi tạo form: {ex.Message}");
            }
        }

        #endregion

        #region ========== PRODUCT VARIANT MANAGEMENT ==========

        /// <summary>
        /// Load danh sách biến thể sản phẩm vào SearchLookUpEdit
        /// Method này được gọi từ form khi FormLoad
        /// Sử dụng SplashScreen để tăng trải nghiệm người dùng và ngăn người dùng thao tác khi đang load data
        /// </summary>
        /// <param name="forceRefresh">Nếu true, sẽ load lại từ database ngay cả khi đã load trước đó</param>
        private async Task LoadProductVariantsAsync(bool forceRefresh = false)
        {
            if (_isLoadingProductVariants) return;
            _isLoadingProductVariants = true;

            try
            {
                // Nếu đã load và không force refresh, không load lại
                if (_isProductVariantDataSourceLoaded && !forceRefresh &&
                    productVariantListDtoBindingSource.DataSource != null &&
                    productVariantListDtoBindingSource.DataSource is List<ProductVariantListDto> existingList &&
                    existingList.Count > 0)
                {
                    return;
                }

                // Hiển thị SplashScreen để thông báo đang load dữ liệu
                SplashScreenHelper.ShowWaitingSplashScreen();

                try
                {
                    // Lấy dữ liệu Entity từ BLL với thông tin đầy đủ
                    var variants = await _productVariantBll.GetAllInUseWithDetailsAsync();

                    // Convert Entity sang ProductVariantListDto
                    var variantListDtos = await ConvertToVariantListDtosAsync(variants);

                    // Bind dữ liệu vào BindingSource
                    productVariantListDtoBindingSource.DataSource = variantListDtos;
                    productVariantListDtoBindingSource.ResetBindings(false);

                    _isProductVariantDataSourceLoaded = true;
                }
                finally
                {
                    // Đóng SplashScreen sau khi hoàn thành hoặc có lỗi
                    SplashScreenHelper.CloseSplashScreen();
                }
            }
            catch (Exception ex)
            {
                _logger.Error("LoadProductVariantsAsync: Exception occurred", ex);
                SplashScreenHelper.CloseSplashScreen(); // Đảm bảo đóng SplashScreen khi có lỗi
                MsgBox.ShowError($"Lỗi tải danh sách biến thể sản phẩm: {ex.Message}");
            }
            finally
            {
                _isLoadingProductVariants = false;
            }
        }

        /// <summary>
        /// Convert Entity sang ProductVariantListDto (Async)
        /// Sử dụng extension method ToListDto() có sẵn trong DTO và bổ sung các field còn thiếu
        /// </summary>
        private Task<List<ProductVariantListDto>> ConvertToVariantListDtosAsync(List<ProductVariant> variants)
        {
            try
            {
                var result = new List<ProductVariantListDto>();

                foreach (var variant in variants)
                {
                    // Sử dụng extension method ToListDto() có sẵn trong DTO
                    var dto = variant.ToListDto();
                    if (dto == null) continue;

                    result.Add(dto);
                }

                return Task.FromResult(result);
            }
            catch (Exception ex)
            {
                _logger.Error("ConvertToVariantListDtosAsync: Exception occurred", ex);
                throw new Exception($"Lỗi convert sang ProductVariantListDto: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Reload ProductVariant datasource (public method để gọi từ form)
        /// </summary>
        public async Task ReloadProductVariantDataSourceAsync()
        {
            try
            {
                await LoadProductVariantsAsync(forceRefresh: true);
            }
            catch (Exception ex)
            {
                _logger.Error("ReloadProductVariantDataSourceAsync: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi reload datasource biến thể sản phẩm: {ex.Message}");
            }
        }

        #endregion
    }
}