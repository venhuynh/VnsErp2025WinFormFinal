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
using Bll.Inventory.InventoryManagement;
using Common.Common;
using Common.Utils;
using DTO.DeviceAssetManagement;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;

namespace DeviceAssetManagement.Management.DeviceWarranty
{
    public partial class FrmDeviceWarranty : DevExpress.XtraEditors.XtraForm
    {
        #region ========== FIELDS & PROPERTIES ==========

        /// <summary>
        /// Business Logic Layer cho thiết bị
        /// </summary>
        private readonly DeviceBll _deviceBll = new DeviceBll();

        /// <summary>
        /// Business Logic Layer cho bảo hành
        /// </summary>
        private readonly WarrantyBll _warrantyBll = new WarrantyBll();

        /// <summary>
        /// Logger để ghi log các sự kiện
        /// </summary>
        private readonly ILogger _logger = LoggerFactory.CreateLogger(LogCategory.UI);

        /// <summary>
        /// Trạng thái đang tải dữ liệu
        /// </summary>
        private bool _isLoading;

        #endregion

        public FrmDeviceWarranty()
        {
            InitializeComponent();
            InitializeForm();
        }

        /// <summary>
        /// Khởi tạo form
        /// </summary>
        private void InitializeForm()
        {
            try
            {
                // Khởi tạo binding source với danh sách rỗng
                warrantyDtoBindingSource.DataSource = new List<WarrantyDto>();

                // Setup events
                InitializeEvents();
            }
            catch (Exception ex)
            {
                _logger.Error("InitializeForm: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi khởi tạo form: {ex.Message}");
            }
        }

        /// <summary>
        /// Khởi tạo các event handlers
        /// </summary>
        private void InitializeEvents()
        {
            // Event khi click nút Xem
            XemBarButtonItem.ItemClick += XemBarButtonItem_ItemClick;

            // GridView events
            if (advBandedGridView1 != null)
            {
                advBandedGridView1.SelectionChanged += DeviceWarrantyDtoGridView_SelectionChanged;
            }
        }


        #region ========== EVENT HANDLERS ==========

        /// <summary>
        /// Event handler cho nút Xem
        /// </summary>
        private void XemBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                LoadWarrantyDataAsync();
            }
            catch (Exception ex)
            {
                _logger.Error("XemBarButtonItem_ItemClick: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi tải dữ liệu: {ex.Message}");
            }
        }

        /// <summary>
        /// Event handler khi selection thay đổi trong GridView
        /// </summary>
        private void DeviceWarrantyDtoGridView_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            try
            {
                UpdateStatusBar();
            }
            catch (Exception ex)
            {
                _logger.Error("DeviceWarrantyDtoGridView_SelectionChanged: Exception occurred", ex);
            }
        }

        #endregion

        #region ========== DATA LOADING ==========

        /// <summary>
        /// Load dữ liệu bảo hành từ database và bind vào grid
        /// Với mỗi device, tìm thông tin bảo hành nếu có thì thêm vào, không có thì hiển thị "chưa có thông tin bảo hành"
        /// </summary>
        private async void LoadWarrantyDataAsync()
        {
            if (_isLoading)
            {
                _logger.Debug("LoadWarrantyDataAsync: Đang tải dữ liệu, bỏ qua request mới");
                return;
            }

            _isLoading = true;

            try
            {
                SplashScreenHelper.ShowWaitingSplashScreen();
                _logger.Debug("LoadWarrantyDataAsync: Bắt đầu tải dữ liệu bảo hành");

                var warrantyDtos = await LoadWarrantyDataInternalAsync();

                // Update UI thread
                BeginInvoke(new Action(() =>
                {
                    BindWarrantyDataToGrid(warrantyDtos);
                }));
            }
            catch (Exception ex)
            {
                _logger.Error("LoadWarrantyDataAsync: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi tải dữ liệu bảo hành: {ex.Message}");
            }
            finally
            {
                SplashScreenHelper.CloseSplashScreen();
                _isLoading = false;
                _logger.Debug("LoadWarrantyDataAsync: Hoàn thành tải dữ liệu");
            }
        }

        /// <summary>
        /// Load dữ liệu bảo hành từ database (internal method, chạy trong background thread)
        /// </summary>
        /// <returns>Danh sách WarrantyDto</returns>
        private async Task<List<WarrantyDto>> LoadWarrantyDataInternalAsync()
        {
            return await Task.Run(() =>
            {
                try
                {
                    // Lấy tất cả các Device từ BLL
                    _logger.Debug("LoadWarrantyDataInternalAsync: Lấy danh sách devices");
                    var devices = _deviceBll.GetAll();
                    _logger.Info($"LoadWarrantyDataInternalAsync: Lấy được {devices?.Count ?? 0} devices");

                    if (devices == null || !devices.Any())
                    {
                        _logger.Warning("LoadWarrantyDataInternalAsync: Không có device nào");
                        return new List<WarrantyDto>();
                    }

                    // Tạo dictionary Warranty theo DeviceId để lookup nhanh
                    var warrantyDict = LoadWarrantiesByDeviceIds(devices);
                    _logger.Info($"LoadWarrantyDataInternalAsync: Load được {warrantyDict.Count} warranties");

                    // Sử dụng method trong DTO để tạo danh sách WarrantyDto
                    _logger.Debug("LoadWarrantyDataInternalAsync: Convert sang WarrantyDto");
                    var warrantyDtos = devices.ToWarrantyDtoListFromDevices(warrantyDict);
                    _logger.Info($"LoadWarrantyDataInternalAsync: Tạo được {warrantyDtos.Count} WarrantyDto");

                    return warrantyDtos;
                }
                catch (Exception ex)
                {
                    _logger.Error("LoadWarrantyDataInternalAsync: Exception occurred", ex);
                    throw;
                }
            });
        }

        /// <summary>
        /// Load warranties theo danh sách deviceIds và tạo dictionary để lookup nhanh
        /// </summary>
        /// <param name="devices">Danh sách devices</param>
        /// <returns>Dictionary chứa Warranty theo DeviceId</returns>
        private Dictionary<Guid, Dal.DataContext.Warranty> LoadWarrantiesByDeviceIds(IEnumerable<Dal.DataContext.Device> devices)
        {
            var warrantyDict = new Dictionary<Guid, Dal.DataContext.Warranty>();
            
            if (devices == null)
            {
                return warrantyDict;
            }

            var deviceIds = devices
                .Where(d => d != null && d.Id != Guid.Empty)
                .Select(d => d.Id)
                .Distinct()
                .ToList();

            if (!deviceIds.Any())
            {
                _logger.Debug("LoadWarrantiesByDeviceIds: Không có deviceId hợp lệ");
                return warrantyDict;
            }

            _logger.Debug($"LoadWarrantiesByDeviceIds: Bắt đầu load warranties cho {deviceIds.Count} devices");

            int successCount = 0;
            int errorCount = 0;

            // Lấy tất cả warranties cho các devices này
            foreach (var deviceId in deviceIds)
            {
                try
                {
                    var warranty = _warrantyBll.FindByDeviceId(deviceId);
                    if (warranty != null)
                    {
                        warrantyDict[deviceId] = warranty;
                        successCount++;
                    }
                }
                catch (Exception ex)
                {
                    errorCount++;
                    _logger.Warning($"LoadWarrantiesByDeviceIds: Không thể load warranty cho DeviceId={deviceId}: {ex.Message}");
                }
            }

            _logger.Info($"LoadWarrantiesByDeviceIds: Load thành công {successCount} warranties, lỗi {errorCount}");

            return warrantyDict;
        }

        /// <summary>
        /// Bind danh sách WarrantyDto vào GridView (chạy trên UI thread)
        /// </summary>
        /// <param name="warrantyDtos">Danh sách WarrantyDto</param>
        private void BindWarrantyDataToGrid(List<WarrantyDto> warrantyDtos)
        {
            try
            {
                if (warrantyDtos == null)
                {
                    warrantyDtos = new List<WarrantyDto>();
                }

                warrantyDtoBindingSource.DataSource = warrantyDtos;
                warrantyDtoBindingSource.ResetBindings(false);
                if (advBandedGridView1 != null)
                {
                    advBandedGridView1.RefreshData();
                }
                UpdateStatusBar();

                _logger.Debug($"BindWarrantyDataToGrid: Bind {warrantyDtos.Count} WarrantyDto vào grid");
            }
            catch (Exception ex)
            {
                _logger.Error("BindWarrantyDataToGrid: Exception occurred", ex);
                throw;
            }
        }


        /// <summary>
        /// Cập nhật status bar
        /// </summary>
        private void UpdateStatusBar()
        {
            try
            {
                var totalCount = warrantyDtoBindingSource.Count;
                var selectedCount = advBandedGridView1 != null ? advBandedGridView1.SelectedRowsCount : 0;

                DataSummaryBarStaticItem.Caption = $@"Tổng số: <b>{totalCount}</b> thiết bị";
                SelectedRowBarStaticItem.Caption = selectedCount > 0 
                    ? $@"Đã chọn: <b>{selectedCount}</b> dòng" 
                    : "Chưa chọn dòng nào";
            }
            catch (Exception ex)
            {
                _logger.Error("UpdateStatusBar: Exception occurred", ex);
            }
        }

        #endregion
    }
}