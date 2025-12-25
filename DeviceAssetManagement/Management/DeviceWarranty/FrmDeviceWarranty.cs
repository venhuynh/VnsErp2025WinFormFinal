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
using DevExpress.XtraBars.Docking;
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

        /// <summary>
        /// Danh sách các thiết bị được chọn (WarrantyDto) để thêm mới bảo hành
        /// </summary>
        private List<WarrantyDto> _selectedWarrantyDtos = new List<WarrantyDto>();

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
                // Đóng dockPanel1 khi khởi tạo
                dockPanel1.Visibility = DockVisibility.Hidden;

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

            // Event khi click nút Thêm mới
            ThemMoiBarButtonItem.ItemClick += ThemMoiBarButtonItem_ItemClick;

            // GridView events
            if (DeviceWarrantyDtoGridView != null)
            {
                DeviceWarrantyDtoGridView.SelectionChanged += DeviceWarrantyDtoGridView_SelectionChanged;
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
        /// Event handler khi click nút Thêm mới
        /// Mở dockPanel1 với độ rộng = 2/3 độ rộng màn hình và hiển thị ucDeviceWarrantyAddEdit1
        /// Chỉ thực hiện khi có một hoặc nhiều thiết bị được chọn
        /// </summary>
        private void ThemMoiBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                // Kiểm tra xem có thiết bị nào được chọn không
                if (_selectedWarrantyDtos == null || _selectedWarrantyDtos.Count == 0)
                {
                    MsgBox.ShowWarning("Vui lòng chọn thiết bị để thêm mới bảo hành.", "Chưa chọn thiết bị");
                    return;
                }

                _logger.Debug($"ThemMoiBarButtonItem_ItemClick: Selected {_selectedWarrantyDtos.Count} device(s)");

                // Thiết lập độ rộng dock panel
                SetupDockPanelWidth();

                // Hiển thị UserControl thêm mới
                ShowUserControlInDockPanel();

                // Cập nhật tiêu đề theo ngữ cảnh + số lượng
                SetDockPanelTitle(@"Thêm mới bảo hành", _selectedWarrantyDtos.Count);

                // TODO: Load danh sách thiết bị đã chọn vào UserControl (nếu UserControl có method LoadSelectedDevices)
                // ucDeviceWarrantyAddEdit1.LoadSelectedDevices(_selectedWarrantyDtos);
            }
            catch (Exception ex)
            {
                _logger.Error("ThemMoiBarButtonItem_ItemClick: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi mở panel thêm mới: {ex.Message}");
            }
        }

        /// <summary>
        /// Event handler khi selection thay đổi trong GridView
        /// Cập nhật danh sách các thiết bị được chọn để thêm mới bảo hành
        /// </summary>
        private void DeviceWarrantyDtoGridView_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            try
            {
                // Lấy danh sách các dòng được chọn
                var selectedRows = DeviceWarrantyDtoGridView.GetSelectedRows();
                
                // Làm mới danh sách thiết bị được chọn
                _selectedWarrantyDtos.Clear();

                if (selectedRows != null && selectedRows.Length > 0)
                {
                    // Lấy tất cả các WarrantyDto được chọn (mỗi WarrantyDto đại diện cho một thiết bị)
                    foreach (var rowHandle in selectedRows)
                    {
                        if (rowHandle < 0) continue;

                        var warrantyDto = DeviceWarrantyDtoGridView.GetRow(rowHandle) as WarrantyDto;
                        if (warrantyDto != null && warrantyDto.DeviceId.HasValue)
                        {
                            _selectedWarrantyDtos.Add(warrantyDto);
                        }
                    }
                }

                _logger.Debug($"DeviceWarrantyDtoGridView_SelectionChanged: Selected {_selectedWarrantyDtos.Count} device(s)");

                // Cập nhật status bar
                UpdateStatusBar();
            }
            catch (Exception ex)
            {
                _logger.Error("DeviceWarrantyDtoGridView_SelectionChanged: Exception occurred", ex);
            }
        }

        #endregion

        #region ========== DOCK PANEL MANAGEMENT ==========

        /// <summary>
        /// Thiết lập độ rộng cho dockPanel1 = 2/3 độ rộng màn hình
        /// </summary>
        private void SetupDockPanelWidth()
        {
            try
            {
                // Tính độ rộng = 2/3 độ rộng màn hình
                int screenWidth = Screen.PrimaryScreen.WorkingArea.Width;
                int panelWidth = (int)(screenWidth * 2.0 / 3.0);

                // Lấy chiều cao hiện tại của panel hoặc tính từ form
                int panelHeight = dockPanel1.Size.Height > 0 
                    ? dockPanel1.Size.Height 
                    : this.ClientSize.Height - barDockControlTop.Height - barDockControlBottom.Height;

                // Hiển thị dockPanel1 trước
                dockPanel1.Visibility = DockVisibility.Visible;

                // Set OriginalSize để panel giữ kích thước khi dock
                dockPanel1.OriginalSize = new Size(panelWidth, 200);

                // Set Size cho dockPanel1 (width mới, giữ nguyên height)
                dockPanel1.Size = new Size(panelWidth, panelHeight);

                // Force update layout
                dockPanel1.Update();
                this.Update();
            }
            catch (Exception ex)
            {
                _logger.Error("SetupDockPanelWidth: Exception occurred", ex);
                throw;
            }
        }

        /// <summary>
        /// Hiển thị UserControl trong dockPanel1
        /// </summary>
        private void ShowUserControlInDockPanel()
        {
            try
            {
                // Hiển thị UserControl
                ucDeviceWarrantyAddEdit1.Visible = true;
                ucDeviceWarrantyAddEdit1.Dock = DockStyle.Fill;

                // Refresh layout
                dockPanel1_Container.Refresh();
            }
            catch (Exception ex)
            {
                _logger.Error("ShowUserControlInDockPanel: Exception occurred", ex);
                throw;
            }
        }

        /// <summary>
        /// Cập nhật tiêu đề dockPanel1 theo ngữ cảnh và số lượng thiết bị chọn
        /// </summary>
        /// <param name="baseTitle">Tiêu đề cơ bản</param>
        /// <param name="selectedCount">Số thiết bị được chọn (0 nếu không cần hiển thị)</param>
        private void SetDockPanelTitle(string baseTitle, int selectedCount = 0)
        {
            if (selectedCount > 0)
            {
                dockPanel1.Text = $@"{baseTitle} ({selectedCount} thiết bị)";
            }
            else
            {
                dockPanel1.Text = baseTitle;
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
                if (DeviceWarrantyDtoGridView != null)
                {
                    DeviceWarrantyDtoGridView.RefreshData();
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
                var selectedCount = DeviceWarrantyDtoGridView != null ? DeviceWarrantyDtoGridView.SelectedRowsCount : 0;

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