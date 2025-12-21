using Bll.MasterData.ProductServiceBll;
using Common.Common;
using Common.Utils;
using Dal.DataContext;
using DevExpress.XtraEditors;
using DTO.Inventory.InventoryManagement;
using DTO.MasterData.ProductService;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventory.Management.DeviceMangement
{
    public partial class UcDeviceDtoAddEdit : DevExpress.XtraEditors.XtraUserControl
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

        public UcDeviceDtoAddEdit()
        {
            InitializeComponent();
            InitializeControl();
        }

        #endregion

        #region ========== INITIALIZATION ==========

        /// <summary>
        /// Khởi tạo control
        /// </summary>
        private void InitializeControl()
        {
            try
            {
                // Setup events
                InitializeEvents();
            }
            catch (Exception ex)
            {
                _logger.Error("InitializeControl: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi khởi tạo control: {ex.Message}");
            }
        }

        /// <summary>
        /// Khởi tạo các event handlers
        /// </summary>
        private void InitializeEvents()
        {
            // Event khi SearchLookUpEdit popup - load dữ liệu nếu chưa load
            ProductVariantSearchLookUpEdit.Properties.Popup += SearchLookUpEdit1_Popup;

            // Load DeviceIdentifierEnum vào ComboBox
            LoadDeviceIdentifierEnumComboBox();
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

        /// <summary>
        /// Event handler khi SearchLookUpEdit popup
        /// Chỉ load dữ liệu nếu chưa load hoặc datasource rỗng
        /// </summary>
        private async void SearchLookUpEdit1_Popup(object sender, EventArgs e)
        {
            try
            {
                // Chỉ load nếu chưa load hoặc datasource rỗng
                if (!_isProductVariantDataSourceLoaded ||
                    productVariantListDtoBindingSource.DataSource == null ||
                    (productVariantListDtoBindingSource.DataSource is List<ProductVariantListDto> list && list.Count == 0))
                {
                    await LoadProductVariantsAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.Error("SearchLookUpEdit1_Popup: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi tải dữ liệu biến thể sản phẩm: {ex.Message}");
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

        #endregion

        #region ========== DEVICE IDENTIFIER ENUM MANAGEMENT ==========

        /// <summary>
        /// Load danh sách DeviceIdentifierEnum vào ComboBox
        /// Sử dụng Description attribute để hiển thị text
        /// </summary>
        private void LoadDeviceIdentifierEnumComboBox()
        {
            try
            {
                DeviceIdentifierEnumComboBox.Items.Clear();

                // Load tất cả các giá trị enum trực tiếp
                foreach (DeviceIdentifierEnum identifierType in Enum.GetValues(typeof(DeviceIdentifierEnum)))
                {
                    DeviceIdentifierEnumComboBox.Items.Add(identifierType);
                }

                // Cấu hình ComboBox
                DeviceIdentifierEnumComboBox.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                DeviceIdentifierEnumComboBox.ShowDropDown = DevExpress.XtraEditors.Controls.ShowDropDown.SingleClick;

                // Sử dụng CustomDisplayText để hiển thị Description thay vì tên enum
                DeviceIdentifierEnumComboBox.CustomDisplayText += DeviceIdentifierEnumComboBox_CustomDisplayText;
            }
            catch (Exception ex)
            {
                _logger.Error("LoadDeviceIdentifierEnumComboBox: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi load danh sách kiểu định danh: {ex.Message}");
            }
        }

        /// <summary>
        /// Event handler để hiển thị Description thay vì tên enum trong ComboBox
        /// </summary>
        private void DeviceIdentifierEnumComboBox_CustomDisplayText(object sender, DevExpress.XtraEditors.Controls.CustomDisplayTextEventArgs e)
        {
            try
            {
                if (e.Value is DeviceIdentifierEnum enumValue)
                {
                    // Lấy Description từ attribute
                    e.DisplayText = GetEnumDescription(enumValue);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("DeviceIdentifierEnumComboBox_CustomDisplayText: Exception occurred", ex);
                // Nếu có lỗi, hiển thị tên enum mặc định
                if (e.Value is DeviceIdentifierEnum enumValue)
                {
                    e.DisplayText = enumValue.ToString();
                }
            }
        }

        /// <summary>
        /// Lấy Description từ enum value
        /// </summary>
        /// <param name="enumValue">Giá trị enum</param>
        /// <returns>Description hoặc tên enum nếu không có Description</returns>
        private string GetEnumDescription(DeviceIdentifierEnum enumValue)
        {
            try
            {
                var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
                if (fieldInfo == null) return enumValue.ToString();

                var descriptionAttribute = fieldInfo.GetCustomAttribute<DescriptionAttribute>();
                return descriptionAttribute?.Description ?? enumValue.ToString();
            }
            catch (Exception ex)
            {
                _logger.Error($"GetEnumDescription: Exception occurred for {enumValue}", ex);
                return enumValue.ToString();
            }
        }

        #endregion
    }
}
