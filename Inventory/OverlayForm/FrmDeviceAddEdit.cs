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


        /// <summary>
        /// Biến để biết ProductVariantListDto hiện tại được chọn là gì
        /// </summary>
        private ProductVariantListDto _selectedCurrentProductVariant = null;

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
            // ProductVariant SearchLookUpEdit event
            ProductVariantSearchLookUpEdit.EditValueChanged += ProductVariantSearchLookUpEdit_EditValueChanged;

            // Device Identifier events
            DeviceIdentifierComboBoxEdit.EditValueChanged += DeviceIdentifierComboBoxEdit_EditValueChanged;
            DeviceIdentifierTextEdit.EditValueChanged += DeviceIdentifierTextEdit_EditValueChanged;
            DeviceIdentifierTextEdit.KeyDown += DeviceIdentifierTextEdit_KeyDown;

            // AddValue HyperlinkLabelControl event
            AddValueHyperlinkLabelControl.Click += AddValueHyperlinkLabelControl_Click;

            // Bar button events
            NewDeviceBarButtonItem.ItemClick += NewDeviceBarButtonItem_ItemClick;
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

                // Khởi tạo deviceDtoBindingSource với danh sách rỗng
                // Trong Designer, DataSource được set là typeof(DeviceDto), cần chuyển sang List<DeviceDto>
                if (deviceDtoBindingSource.DataSource is not List<DeviceDto>)
                {
                    deviceDtoBindingSource.DataSource = new List<DeviceDto>();
                }

                // Load datasource cho ProductVariant
                await LoadProductVariantsAsync();

                // Load danh sách DeviceIdentifier vào ComboBox
                LoadDeviceIdentifierComboBox();

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

        #region ========== EVENT HANDLERS ==========

        /// <summary>
        /// Event handler khi ProductVariantSearchLookUpEdit thay đổi
        /// Cập nhật giá trị của _selectedCurrentProductVariant
        /// </summary>
        private void ProductVariantSearchLookUpEdit_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                //Reset _selectedCurrentProductVariant
                _selectedCurrentProductVariant = null;

                var editValue = ProductVariantSearchLookUpEdit.EditValue;
                if (editValue == null || !(editValue is Guid productVariantId) || productVariantId == Guid.Empty)
                {
                    _logger.Debug("ProductVariantSearchLookUpEdit_EditValueChanged: EditValue is null or invalid");
                    return;
                }

                _logger.Debug($"ProductVariantSearchLookUpEdit_EditValueChanged: ProductVariantId={productVariantId}");

                // Lấy ProductVariantListDto từ binding source
                _selectedCurrentProductVariant = productVariantListDtoBindingSource.Cast<ProductVariantListDto>()
                    .FirstOrDefault(v => v.Id == productVariantId);

            }
            catch (Exception ex)
            {
                _logger.Error($"ProductVariantSearchLookUpEdit_EditValueChanged: Lỗi xử lý sự kiện: {ex.Message}", ex);
                MsgBox.ShowError($"Lỗi cập nhật thông tin sản phẩm: {ex.Message}");
            }
        }


        /// <summary>
        /// Event handler khi DeviceIdentifierComboBoxEdit thay đổi
        /// </summary>
        private void DeviceIdentifierComboBoxEdit_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                // Khi loại định danh thay đổi, có thể cần clear giá trị cũ
                // Hoặc giữ nguyên giá trị nếu đã có
                _logger.Debug("DeviceIdentifierComboBoxEdit_EditValueChanged: Device identifier type changed");
            }
            catch (Exception ex)
            {
                _logger.Error($"DeviceIdentifierComboBoxEdit_EditValueChanged: Lỗi xử lý sự kiện: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Event handler khi click vào AddValueHyperlinkLabelControl
        /// Logic tìm DeviceDto để thêm identifier theo thứ tự ưu tiên:
        /// 1. DeviceDto được chọn trong GridView (nếu có cùng ProductVariantId)
        /// 2. DeviceDto có cùng ProductVariantId và chưa có identifier này
        /// 3. DeviceDto có cùng ProductVariantId và chưa có identifier nào (rỗng)
        /// 4. DeviceDto mới nhất có cùng ProductVariantId
        /// 5. Tạo DeviceDto mới nếu không tìm thấy
        /// </summary>
        private void AddValueHyperlinkLabelControl_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate ProductVariant đã được chọn
                if (_selectedCurrentProductVariant == null)
                {
                    MsgBox.ShowWarning("Vui lòng chọn sản phẩm - dịch vụ - hàng hóa");
                    ProductVariantSearchLookUpEdit.Focus();
                    return;
                }

                // Validate giá trị định danh
                var identifierValue = DeviceIdentifierTextEdit.EditValue?.ToString()?.Trim() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(identifierValue))
                {
                    MsgBox.ShowWarning("Vui lòng nhập giá trị định danh");
                    DeviceIdentifierTextEdit.Focus();
                    return;
                }

                // Validate loại định danh
                var identifierType = GetDeviceIdentifierType();
                if (!identifierType.HasValue)
                {
                    MsgBox.ShowWarning("Vui lòng chọn loại định danh");
                    DeviceIdentifierComboBoxEdit.Focus();
                    return;
                }

                // Đảm bảo DataSource là List<DeviceDto>
                if (deviceDtoBindingSource.DataSource is not List<DeviceDto> deviceList)
                {
                    deviceList = new List<DeviceDto>();
                    deviceDtoBindingSource.DataSource = deviceList;
                }

                // Lọc các DeviceDto có cùng ProductVariantId
                var sameProductVariantDevices = deviceList
                    .Where(d => d.ProductVariantId == _selectedCurrentProductVariant.Id)
                    .ToList();

                DeviceDto targetDeviceDto = null;

                // Ưu tiên 1: DeviceDto được chọn trong GridView (nếu có cùng ProductVariantId)
                var focusedDeviceDto = DeviceDtoGridView.GetFocusedRow() as DeviceDto;
                if (focusedDeviceDto != null && 
                    focusedDeviceDto.ProductVariantId == _selectedCurrentProductVariant.Id)
                {
                    targetDeviceDto = focusedDeviceDto;
                    _logger.Debug($"AddValueHyperlinkLabelControl_Click: Sử dụng DeviceDto được chọn trong GridView, Id={targetDeviceDto.Id}");
                }

                // Ưu tiên 2: DeviceDto có cùng ProductVariantId và chưa có identifier này
                if (targetDeviceDto == null)
                {
                    targetDeviceDto = sameProductVariantDevices
                        .FirstOrDefault(d => string.IsNullOrWhiteSpace(d.GetIdentifierValue(identifierType.Value)));
                    
                    if (targetDeviceDto != null)
                    {
                        _logger.Debug($"AddValueHyperlinkLabelControl_Click: Tìm thấy DeviceDto chưa có {identifierType.Value}, Id={targetDeviceDto.Id}");
                    }
                }

                // Ưu tiên 3: DeviceDto có cùng ProductVariantId và chưa có identifier nào (rỗng)
                if (targetDeviceDto == null)
                {
                    targetDeviceDto = sameProductVariantDevices
                        .FirstOrDefault(d => d.GetAllIdentifiers().Count == 0);
                    
                    if (targetDeviceDto != null)
                    {
                        _logger.Debug($"AddValueHyperlinkLabelControl_Click: Tìm thấy DeviceDto chưa có identifier nào, Id={targetDeviceDto.Id}");
                    }
                }

                // Ưu tiên 4: DeviceDto mới nhất có cùng ProductVariantId
                if (targetDeviceDto == null)
                {
                    targetDeviceDto = sameProductVariantDevices
                        .OrderByDescending(d => d.CreatedDate)
                        .FirstOrDefault();
                    
                    if (targetDeviceDto != null)
                    {
                        _logger.Debug($"AddValueHyperlinkLabelControl_Click: Sử dụng DeviceDto mới nhất, Id={targetDeviceDto.Id}");
                    }
                }

                // Ưu tiên 5: Tạo DeviceDto mới nếu không tìm thấy
                if (targetDeviceDto == null)
                {
                    // Status mặc định là Available (0)
                    int status = 0;
                    var statusName = GetStatusName(status);

                    targetDeviceDto = new DeviceDto
                    {
                        Id = Guid.NewGuid(), // Set ID mới để phân biệt với các DeviceDto khác
                        ProductVariantId = _selectedCurrentProductVariant.Id,
                        ProductVariantName = _selectedCurrentProductVariant.VariantFullName,
                        ProductVariantCode = _selectedCurrentProductVariant.VariantCode,
                        Status = status,
                        StatusName = statusName,
                        DeviceType = 0, // Hardware mặc định
                        DeviceTypeName = "Hardware",
                        IsActive = true,
                        CreatedDate = DateTime.Now
                    };

                    // Thêm DeviceDto mới vào danh sách
                    deviceList.Add(targetDeviceDto);
                    _logger.Info($"AddValueHyperlinkLabelControl_Click: Đã tạo DeviceDto mới với Id={targetDeviceDto.Id}, ProductVariantId={_selectedCurrentProductVariant.Id}");
                }
                else
                {
                    // Đảm bảo thông tin ProductVariant được đồng bộ
                    targetDeviceDto.ProductVariantId = _selectedCurrentProductVariant.Id;
                    targetDeviceDto.ProductVariantName = _selectedCurrentProductVariant.VariantFullName;
                    targetDeviceDto.ProductVariantCode = _selectedCurrentProductVariant.VariantCode;
                }

                // Kiểm tra xem identifier type này đã có giá trị chưa
                var currentValue = targetDeviceDto.GetIdentifierValue(identifierType.Value);
                
                if (!string.IsNullOrWhiteSpace(currentValue))
                {
                    // Đã có giá trị, kiểm tra xem có khác không
                    if (!currentValue.Equals(identifierValue, StringComparison.OrdinalIgnoreCase))
                    {
                        // Giá trị khác, cập nhật giá trị mới
                        targetDeviceDto.SetIdentifierValue(identifierType.Value, identifierValue);
                        _logger.Info($"AddValueHyperlinkLabelControl_Click: Đã cập nhật {identifierType.Value} từ '{currentValue}' thành '{identifierValue}' cho DeviceDto Id={targetDeviceDto.Id}");
                    }
                    else
                    {
                        // Giá trị giống nhau, không cần cập nhật
                        _logger.Debug($"AddValueHyperlinkLabelControl_Click: Giá trị {identifierType.Value}='{identifierValue}' đã tồn tại và giống nhau cho DeviceDto Id={targetDeviceDto.Id}");
                    }
                }
                else
                {
                    // Chưa có giá trị, thêm identifier mới
                    targetDeviceDto.SetIdentifierValue(identifierType.Value, identifierValue);
                    _logger.Info($"AddValueHyperlinkLabelControl_Click: Đã thêm {identifierType.Value}='{identifierValue}' vào DeviceDto Id={targetDeviceDto.Id}");
                }

                // Reset bindings và refresh gridview
                deviceDtoBindingSource.ResetBindings(false);
                RefreshDeviceDtoGridView();

                // Focus vào DeviceDto vừa được cập nhật trong GridView
                // Tìm index của DeviceDto trong list để focus vào row tương ứng
                var deviceIndex = deviceList.IndexOf(targetDeviceDto);
                if (deviceIndex >= 0 && deviceIndex < DeviceDtoGridView.RowCount)
                {
                    var rowHandle = DeviceDtoGridView.GetRowHandle(deviceIndex);
                    DeviceDtoGridView.FocusedRowHandle = rowHandle;
                    DeviceDtoGridView.MakeRowVisible(rowHandle);
                }

                // Clear input controls
                DeviceIdentifierTextEdit.EditValue = null;
                DeviceIdentifierTextEdit.Focus();
            }
            catch (Exception ex)
            {
                _logger.Error($"AddValueHyperlinkLabelControl_Click: Lỗi cập nhật/thêm identifier: {ex.Message}", ex);
                MsgBox.ShowError($"Lỗi cập nhật/thêm identifier: {ex.Message}");
            }
        }

        /// <summary>
        /// Event handler khi DeviceIdentifierTextEdit thay đổi
        /// Cập nhật giá trị định danh vào deviceDtoBindingSource
        /// </summary>
        private void DeviceIdentifierTextEdit_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                var identifierValue = DeviceIdentifierTextEdit.EditValue?.ToString()?.Trim() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(identifierValue))
                {
                    return;
                }

                // Lấy loại định danh từ ComboBox
                var identifierType = GetDeviceIdentifierType();
                if (!identifierType.HasValue)
                {
                    return;
                }

                _logger.Debug($"DeviceIdentifierTextEdit_EditValueChanged: IdentifierType={identifierType.Value}, Value={identifierValue}");

                // Cập nhật giá trị định danh vào tất cả các DeviceDto trong binding source
                UpdateDeviceIdentifier(identifierType.Value, identifierValue);

                // Refresh gridview để hiển thị
                RefreshDeviceDtoGridView();
            }
            catch (Exception ex)
            {
                _logger.Error($"DeviceIdentifierTextEdit_EditValueChanged: Lỗi xử lý sự kiện: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Event handler khi nhấn phím trong DeviceIdentifierTextEdit
        /// Enter key để thêm thiết bị vào danh sách
        /// </summary>
        private void DeviceIdentifierTextEdit_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    AddDeviceToList();
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"DeviceIdentifierTextEdit_KeyDown: Lỗi xử lý sự kiện: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Event handler khi click vào NewDeviceBarButtonItem
        /// - Xóa hết các thông tin đã có trong các control nhập liệu
        /// - Duyệt các phần tử của DeviceDto, nếu chưa có Id thì tạo Id mới
        /// </summary>
        private void NewDeviceBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                _logger.Debug("NewDeviceBarButtonItem_ItemClick: New device button clicked");

                // 1. Xóa hết các thông tin đã có trong các control nhập liệu
                ProductVariantSearchLookUpEdit.EditValue = null;
                DeviceIdentifierComboBoxEdit.EditValue = DeviceIdentifierEnum.SerialNumber; // Reset về giá trị mặc định
                DeviceIdentifierTextEdit.EditValue = null;
                
                // Reset _selectedCurrentProductVariant
                _selectedCurrentProductVariant = null;

                // 2. Duyệt các phần tử của DeviceDto, nếu chưa có Id thì tạo Id mới
                if (deviceDtoBindingSource.DataSource is List<DeviceDto> deviceList)
                {
                    int countNewIds = 0;
                    foreach (var deviceDto in deviceList)
                    {
                        if (deviceDto.Id == Guid.Empty)
                        {
                            deviceDto.Id = Guid.NewGuid();
                            countNewIds++;
                            _logger.Debug($"NewDeviceBarButtonItem_ItemClick: Đã tạo Id mới cho DeviceDto, Id={deviceDto.Id}");
                        }
                    }

                    if (countNewIds > 0)
                    {
                        deviceDtoBindingSource.ResetBindings(false);
                        RefreshDeviceDtoGridView();
                        _logger.Info($"NewDeviceBarButtonItem_ItemClick: Đã tạo {countNewIds} Id mới cho các DeviceDto chưa có Id");
                    }
                }

                // Focus vào control đầu tiên
                ProductVariantSearchLookUpEdit.Focus();

                _logger.Info("NewDeviceBarButtonItem_ItemClick: Đã clear các control và set Id cho các DeviceDto chưa có Id");
            }
            catch (Exception ex)
            {
                _logger.Error($"NewDeviceBarButtonItem_ItemClick: Lỗi xử lý: {ex.Message}", ex);
                MsgBox.ShowError($"Lỗi xử lý: {ex.Message}");
            }
        }

        #endregion

        #region ========== UPDATE METHODS ==========

        /// <summary>
        /// Cập nhật thông tin ProductVariant vào tất cả các DeviceDto trong binding source
        /// </summary>
        private void UpdateProductVariantInfo(Guid productVariantId, ProductVariantListDto variantListDto)
        {
            try
            {
                // Đảm bảo DataSource là List<DeviceDto>
                if (deviceDtoBindingSource.DataSource is not List<DeviceDto> deviceList)
                {
                    // Nếu DataSource chưa phải là List, khởi tạo mới
                    deviceList = new List<DeviceDto>();
                    deviceDtoBindingSource.DataSource = deviceList;
                }

                // Nếu chưa có DeviceDto nào trong list, tạo một DeviceDto mới với ProductVariant này
                if (deviceList.Count == 0)
                {
                    // Status mặc định là Available (0)
                    int status = 0;
                    var statusName = GetStatusName(status);

                    // Tạo DeviceDto mới với ProductVariant và các giá trị mặc định
                    var newDeviceDto = new DeviceDto
                    {
                        Id = Guid.NewGuid(),
                        ProductVariantId = productVariantId,
                        ProductVariantName = variantListDto.VariantFullName,
                        ProductVariantCode = variantListDto.VariantCode,
                        Status = status,
                        StatusName = statusName,
                        DeviceType = 0, // Hardware mặc định
                        DeviceTypeName = "Hardware",
                        IsActive = true,
                        CreatedDate = DateTime.Now
                    };

                    deviceList.Add(newDeviceDto);
                    _logger.Debug($"UpdateProductVariantInfo: Đã tạo DeviceDto mới với ProductVariantId={productVariantId}");
                }

                // Cập nhật thông tin ProductVariant cho tất cả các DeviceDto trong list
                foreach (var device in deviceList)
                {
                    device.ProductVariantId = productVariantId;
                    device.ProductVariantName = variantListDto.VariantFullName;
                    device.ProductVariantCode = variantListDto.VariantCode;
                }

                deviceDtoBindingSource.ResetBindings(false);
                _logger.Debug($"UpdateProductVariantInfo: Đã cập nhật thông tin ProductVariant cho {deviceList.Count} thiết bị");
            }
            catch (Exception ex)
            {
                _logger.Error($"UpdateProductVariantInfo: Lỗi cập nhật thông tin ProductVariant: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Cập nhật Status và StatusName vào tất cả các DeviceDto trong binding source
        /// </summary>
        private void UpdateStatusInfo(int status, string statusName)
        {
            try
            {
                // Đảm bảo DataSource là List<DeviceDto>
                if (deviceDtoBindingSource.DataSource is not List<DeviceDto> deviceList)
                {
                    // Nếu DataSource chưa phải là List, khởi tạo mới
                    deviceList = new List<DeviceDto>();
                    deviceDtoBindingSource.DataSource = deviceList;
                }

                foreach (var device in deviceList)
                {
                    device.Status = status;
                    device.StatusName = statusName;
                }

                deviceDtoBindingSource.ResetBindings(false);
                _logger.Debug($"UpdateStatusInfo: Đã cập nhật Status cho {deviceList.Count} thiết bị");
            }
            catch (Exception ex)
            {
                _logger.Error($"UpdateStatusInfo: Lỗi cập nhật Status: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Cập nhật giá trị định danh vào tất cả các DeviceDto trong binding source
        /// </summary>
        private void UpdateDeviceIdentifier(DeviceIdentifierEnum identifierType, string value)
        {
            try
            {
                // Đảm bảo DataSource là List<DeviceDto>
                if (deviceDtoBindingSource.DataSource is not List<DeviceDto> deviceList)
                {
                    // Nếu DataSource chưa phải là List, khởi tạo mới
                    deviceList = new List<DeviceDto>();
                    deviceDtoBindingSource.DataSource = deviceList;
                }

                foreach (var device in deviceList)
                {
                    device.SetIdentifierValue(identifierType, value);
                }

                deviceDtoBindingSource.ResetBindings(false);
                _logger.Debug($"UpdateDeviceIdentifier: Đã cập nhật {identifierType} cho {deviceList.Count} thiết bị");
            }
            catch (Exception ex)
            {
                _logger.Error($"UpdateDeviceIdentifier: Lỗi cập nhật định danh: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Thêm thiết bị mới vào danh sách
        /// </summary>
        private void AddDeviceToList()
        {
            try
            {
                // Validate các trường bắt buộc
                if (ProductVariantSearchLookUpEdit.EditValue == null || 
                    !(ProductVariantSearchLookUpEdit.EditValue is Guid productVariantId) || 
                    productVariantId == Guid.Empty)
                {
                    MsgBox.ShowWarning("Vui lòng chọn sản phẩm - dịch vụ - hàng hóa");
                    ProductVariantSearchLookUpEdit.Focus();
                    return;
                }

                var identifierValue = DeviceIdentifierTextEdit.EditValue?.ToString()?.Trim() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(identifierValue))
                {
                    MsgBox.ShowWarning("Vui lòng nhập giá trị định danh");
                    DeviceIdentifierTextEdit.Focus();
                    return;
                }

                var identifierType = GetDeviceIdentifierType();
                if (!identifierType.HasValue)
                {
                    MsgBox.ShowWarning("Vui lòng chọn loại định danh");
                    DeviceIdentifierComboBoxEdit.Focus();
                    return;
                }

                // Lấy thông tin ProductVariant
                var variantListDto = productVariantListDtoBindingSource.Cast<ProductVariantListDto>()
                    .FirstOrDefault(v => v.Id == productVariantId);

                if (variantListDto == null)
                {
                    MsgBox.ShowError("Không tìm thấy thông tin sản phẩm");
                    return;
                }

                // Status mặc định là Available (0)
                int status = 0;
                var statusName = GetStatusName(status);

                // Tạo DeviceDto mới
                var deviceDto = new DeviceDto
                {
                    Id = Guid.NewGuid(),
                    ProductVariantId = productVariantId,
                    ProductVariantName = variantListDto.VariantFullName,
                    ProductVariantCode = variantListDto.VariantCode,
                    Status = status,
                    StatusName = statusName,
                    DeviceType = 0, // Hardware mặc định
                    DeviceTypeName = "Hardware",
                    IsActive = true,
                    CreatedDate = DateTime.Now
                };

                // Set giá trị định danh
                deviceDto.SetIdentifierValue(identifierType.Value, identifierValue);

                // Thêm vào binding source
                if (deviceDtoBindingSource.DataSource is not List<DeviceDto> deviceList)
                {
                    deviceList = new List<DeviceDto>();
                    deviceDtoBindingSource.DataSource = deviceList;
                }

                deviceList.Add(deviceDto);
                deviceDtoBindingSource.ResetBindings(false);

                // Clear input controls
                DeviceIdentifierTextEdit.EditValue = null;
                DeviceIdentifierTextEdit.Focus();

                // Refresh gridview
                RefreshDeviceDtoGridView();

                _logger.Info($"AddDeviceToList: Đã thêm thiết bị mới, ProductVariantId={productVariantId}, IdentifierType={identifierType.Value}, IdentifierValue={identifierValue}");
            }
            catch (Exception ex)
            {
                _logger.Error($"AddDeviceToList: Lỗi thêm thiết bị: {ex.Message}", ex);
                MsgBox.ShowError($"Lỗi thêm thiết bị: {ex.Message}");
            }
        }

        /// <summary>
        /// Refresh DeviceDtoGridView để hiển thị dữ liệu mới
        /// </summary>
        private void RefreshDeviceDtoGridView()
        {
            try
            {
                DeviceDtoGridView.RefreshData();
            }
            catch (Exception ex)
            {
                _logger.Error($"RefreshDeviceDtoGridView: Lỗi refresh gridview: {ex.Message}", ex);
            }
        }

        #endregion

        #region ========== HELPER METHODS ==========


        /// <summary>
        /// Load danh sách DeviceIdentifier vào ComboBox
        /// </summary>
        private void LoadDeviceIdentifierComboBox()
        {
            try
            {
                
                DeviceIdentifierComboBoxEdit.Properties.Items.Clear();
                DeviceIdentifierComboBoxEdit.Properties.Items.Add(new { Value = DeviceIdentifierEnum.SerialNumber, Display = "Serial Number" });
                DeviceIdentifierComboBoxEdit.Properties.Items.Add(new { Value = DeviceIdentifierEnum.MAC, Display = "MAC" });
                DeviceIdentifierComboBoxEdit.Properties.Items.Add(new { Value = DeviceIdentifierEnum.IMEI, Display = "IMEI" });
                DeviceIdentifierComboBoxEdit.Properties.Items.Add(new { Value = DeviceIdentifierEnum.AssetTag, Display = "AssetTag" });
                DeviceIdentifierComboBoxEdit.Properties.Items.Add(new { Value = DeviceIdentifierEnum.LicenseKey, Display = "License Key" });

                // Set mặc định là SerialNumber
                DeviceIdentifierComboBoxEdit.EditValue = DeviceIdentifierEnum.SerialNumber;
            }
            catch (Exception ex)
            {
                _logger.Error($"LoadDeviceIdentifierComboBox: Lỗi load danh sách DeviceIdentifier: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy StatusName từ status value
        /// </summary>
        private string GetStatusName(int status)
        {
            return status switch
            {
                0 => "Available",
                1 => "InUse",
                2 => "Maintenance",
                3 => "Broken",
                4 => "Disposed",
                5 => "Reserved",
                _ => "Unknown"
            };
        }

        /// <summary>
        /// Lấy DeviceIdentifierType từ ComboBox
        /// </summary>
        private DeviceIdentifierEnum? GetDeviceIdentifierType()
        {
            try
            {
                var editValue = DeviceIdentifierComboBoxEdit.EditValue;
                if (editValue == null)
                {
                    return null;
                }

                // Nếu editValue là DeviceIdentifierEnum trực tiếp
                if (editValue is DeviceIdentifierEnum enumValue)
                {
                    return enumValue;
                }

                // Nếu editValue là object có property Value
                var valueProperty = editValue.GetType().GetProperty("Value");
                if (valueProperty != null)
                {
                    var value = valueProperty.GetValue(editValue);
                    if (value is DeviceIdentifierEnum identifierEnum)
                    {
                        return identifierEnum;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.Error($"GetDeviceIdentifierType: Lỗi lấy DeviceIdentifierType: {ex.Message}", ex);
                return null;
            }
        }

        #endregion
    }
}