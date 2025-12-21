using Bll.Inventory.InventoryManagement;
using Common.Common;
using Common.Utils;
using DTO.Inventory.InventoryManagement;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraBars.Docking;

namespace Inventory.Management.DeviceMangement
{
    public partial class FrmDeviceDtoMangement : DevExpress.XtraEditors.XtraForm
    {
        #region ========== FIELDS & PROPERTIES ==========

        /// <summary>
        /// Business Logic Layer cho thiết bị
        /// </summary>
        private readonly DeviceBll _deviceBll = new DeviceBll();

        /// <summary>
        /// Logger để ghi log các sự kiện
        /// </summary>
        private readonly ILogger _logger = LoggerFactory.CreateLogger(LogCategory.UI);

        /// <summary>
        /// Trạng thái đang tải dữ liệu
        /// </summary>
        private bool _isLoading;

        #endregion

        public FrmDeviceDtoMangement()
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
                deviceDtoBindingSource.DataSource = new List<DeviceDto>();

                // Load StatusComboBox với các giá trị status
                LoadStatusComboBox();

                // Cấu hình GridView để cho phép edit colStatus và colNotes
                ConfigureGridViewEditing();

                // Setup events
                InitializeEvents();

                // Load dữ liệu ban đầu
                //LoadDeviceDataAsync();
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
            // Event khi click nút Thêm mới
            ThemMoiBarButtonItem.ItemClick += ThemMoiBarButtonItem_ItemClick;

            // Event khi click nút Xem (reload dữ liệu)
            XemBarButtonItem.ItemClick += XemBarButtonItem_ItemClick;

            // Đăng ký event DeviceSaved từ ucDeviceDtoAddEdit1
            ucDeviceDtoAddEdit1.DeviceSaved += UcDeviceDtoAddEdit1_DeviceSaved;

            // Event khi selection thay đổi trong GridView
            DeviceDtoGridView.SelectionChanged += DeviceDtoGridView_SelectionChanged;

            // Event khi giá trị cell thay đổi (để lưu vào database)
            DeviceDtoGridView.CellValueChanged += DeviceDtoGridView_CellValueChanged;
        }

        /// <summary>
        /// Event handler khi click nút Thêm mới
        /// Mở dockPanel1 với độ rộng = 2/3 độ rộng màn hình
        /// </summary>
        private void ThemMoiBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
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
                _logger.Error("ThemMoiBarButtonItem_ItemClick: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi mở panel thêm mới: {ex.Message}");
            }
        }

        /// <summary>
        /// Event handler khi click nút Xem (reload dữ liệu)
        /// </summary>
        private void XemBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                LoadDeviceDataAsync();
            }
            catch (Exception ex)
            {
                _logger.Error("XemBarButtonItem_ItemClick: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi tải dữ liệu: {ex.Message}");
            }
        }

        /// <summary>
        /// Event handler khi thiết bị được lưu thành công từ ucDeviceDtoAddEdit1
        /// Refresh lại dữ liệu trong grid
        /// </summary>
        private void UcDeviceDtoAddEdit1_DeviceSaved(object sender, EventArgs e)
        {
            try
            {
                _logger.Debug("UcDeviceDtoAddEdit1_DeviceSaved: Device saved, refreshing data");
                
                // Refresh lại dữ liệu
                LoadDeviceDataAsync();

                // Đóng dockPanel1 sau khi lưu thành công
                dockPanel1.Visibility = DockVisibility.Hidden;
            }
            catch (Exception ex)
            {
                _logger.Error("UcDeviceDtoAddEdit1_DeviceSaved: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi refresh dữ liệu: {ex.Message}");
            }
        }


        #region ========== STATUS COMBOBOX ==========

        /// <summary>
        /// Load StatusComboBox với các giá trị status enum
        /// </summary>
        private void LoadStatusComboBox()
        {
            try
            {
                StatusComboBox.Items.Clear();

                // Xóa các items cũ
                StatusComboBox.Items.Clear();

                // Thêm các tùy chọn xác thực sử dụng ApplicationEnumUtils
                foreach (DeviceStatusEnum value in Enum.GetValues(typeof(DeviceStatusEnum)))
                {
                    int index = ApplicationEnumUtils.GetValue(value);
                    string itemName = ApplicationEnumUtils.GetDescription(value);

                    StatusComboBox.Items.Insert(index, itemName);
                }

                
                // Sử dụng CustomDisplayText để hiển thị text tương ứng
                StatusComboBox.CustomDisplayText += StatusComboBox_CustomDisplayText;
            }
            catch (Exception ex)
            {
                _logger.Error("LoadStatusComboBox: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi load StatusComboBox: {ex.Message}");
            }
        }

        /// <summary>
        /// Event handler để hiển thị Description thay vì tên enum trong StatusComboBox
        /// </summary>
        private void StatusComboBox_CustomDisplayText(object sender, DevExpress.XtraEditors.Controls.CustomDisplayTextEventArgs e)
        {
            try
            {
                if (e.Value == null) return;

                // Nếu giá trị là string (Description), giữ nguyên
                if (e.Value is string stringValue)
                {
                    e.DisplayText = stringValue;
                    return;
                }

                DeviceStatusEnum statusValue;
                if (e.Value is DeviceStatusEnum enumValue)
                {
                    statusValue = enumValue;
                }
                else if (e.Value is int intValue && Enum.IsDefined(typeof(DeviceStatusEnum), intValue))
                {
                    statusValue = (DeviceStatusEnum)intValue;
                }
                else
                {
                    return;
                }

                // Lấy Description từ attribute
                e.DisplayText = GetStatusDescription(statusValue);
            }
            catch (Exception ex)
            {
                _logger.Error("StatusComboBox_CustomDisplayText: Exception occurred", ex);
                // Nếu có lỗi, hiển thị giá trị mặc định
                e.DisplayText = e.Value?.ToString() ?? string.Empty;
            }
        }

        /// <summary>
        /// Lấy Description từ enum value
        /// </summary>
        /// <param name="status">Giá trị enum</param>
        /// <returns>Description hoặc tên enum nếu không có Description</returns>
        private string GetStatusDescription(DeviceStatusEnum status)
        {
            try
            {
                return ApplicationEnumUtils.GetDescription(status);
            }
            catch (Exception ex)
            {
                _logger.Error($"GetStatusDescription: Exception occurred for {status}", ex);
                return status.ToString();
            }
        }

        /// <summary>
        /// Lấy enum value từ Description string
        /// </summary>
        /// <param name="description">Description string</param>
        /// <returns>Enum value hoặc null nếu không tìm thấy</returns>
        private DeviceStatusEnum? GetStatusEnumFromDescription(string description)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(description))
                    return null;

                // Duyệt qua tất cả các giá trị enum để tìm Description khớp
                foreach (DeviceStatusEnum enumValue in Enum.GetValues(typeof(DeviceStatusEnum)))
                {
                    var enumDescription = ApplicationEnumUtils.GetDescription(enumValue);
                    if (string.Equals(enumDescription, description, StringComparison.OrdinalIgnoreCase))
                    {
                        return enumValue;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.Error($"GetStatusEnumFromDescription: Exception occurred for '{description}'", ex);
                return null;
            }
        }

        /// <summary>
        /// Cấu hình GridView để cho phép edit colStatus và colNotes
        /// </summary>
        private void ConfigureGridViewEditing()
        {
            try
            {
                // Cho phép edit colStatus và colNotes
                colStatus.OptionsColumn.AllowEdit = true;
                colNotes.OptionsColumn.AllowEdit = true;

                // Các cột khác không cho phép edit
                colHtmlInfo.OptionsColumn.AllowEdit = false;
            }
            catch (Exception ex)
            {
                _logger.Error("ConfigureGridViewEditing: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi cấu hình GridView: {ex.Message}");
            }
        }

        #endregion

        #region ========== GRIDVIEW EVENTS ==========

        /// <summary>
        /// Event handler khi giá trị cell thay đổi trong GridView
        /// Lưu thay đổi vào database
        /// </summary>
        private void DeviceDtoGridView_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                // Chỉ xử lý khi thay đổi colStatus hoặc colNotes
                if (e.Column != colStatus && e.Column != colNotes)
                    return;

                // Lấy DeviceDto từ row hiện tại
                if (e.RowHandle < 0) return;

                var deviceDto = DeviceDtoGridView.GetRow(e.RowHandle) as DeviceDto;
                if (deviceDto == null) return;

                // Nếu thay đổi colStatus, cần convert string (Description) về enum
                if (e.Column == colStatus && e.Value is string statusDescription)
                {
                    var statusEnum = GetStatusEnumFromDescription(statusDescription);
                    if (statusEnum.HasValue)
                    {
                        deviceDto.Status = statusEnum.Value;
                    }
                    else
                    {
                        _logger.Warning($"DeviceDtoGridView_CellValueChanged: Cannot convert status description '{statusDescription}' to enum");
                        MsgBox.ShowWarning($"Không thể chuyển đổi trạng thái '{statusDescription}'");
                        return;
                    }
                }

                _logger.Debug($"DeviceDtoGridView_CellValueChanged: Updating device {deviceDto.Id}, Column: {e.Column.FieldName}");

                // Lưu thay đổi vào database
                SaveDeviceChangesAsync(deviceDto);
            }
            catch (Exception ex)
            {
                _logger.Error("DeviceDtoGridView_CellValueChanged: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi lưu thay đổi: {ex.Message}");
            }
        }

        /// <summary>
        /// Lưu thay đổi của Device vào database
        /// </summary>
        private async void SaveDeviceChangesAsync(DeviceDto deviceDto)
        {
            try
            {
                await Task.Run(() =>
                {
                    try
                    {
                        // Lấy Device entity từ database
                        var device = _deviceBll.GetById(deviceDto.Id);
                        if (device == null)
                        {
                            _logger.Warning($"SaveDeviceChangesAsync: Device not found, Id={deviceDto.Id}");
                            return;
                        }

                        // Cập nhật Status và Notes (convert enum to int)
                        device.Status = (int)deviceDto.Status;
                        device.Notes = deviceDto.Notes;
                        device.UpdatedDate = DateTime.Now;

                        // Lưu vào database
                        _deviceBll.SaveOrUpdate(device);

                        _logger.Info($"SaveDeviceChangesAsync: Device updated successfully, Id={deviceDto.Id}");
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"SaveDeviceChangesAsync: Exception occurred, DeviceId={deviceDto.Id}", ex);
                        throw;
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.Error("SaveDeviceChangesAsync: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi lưu thay đổi thiết bị: {ex.Message}");
            }
        }

        #endregion

        #region ========== DATA LOADING ==========

        /// <summary>
        /// Load dữ liệu thiết bị từ database và bind vào grid
        /// </summary>
        private async void LoadDeviceDataAsync()
        {
            if (_isLoading) return;
            _isLoading = true;

            try
            {
                SplashScreenHelper.ShowWaitingSplashScreen();

                try
                {
                    await Task.Run(() =>
                    {
                        // Lấy tất cả Device entities từ BLL
                        var devices = _deviceBll.GetAll();

                        // Convert Entity sang DeviceDto
                        var deviceDtos = devices
                            .Where(d => d != null)
                            .Select(entity => entity.ToDto())
                            .Where(dto => dto != null)
                            .ToList();

                        // Update UI thread
                        BeginInvoke(new Action(() =>
                        {
                            deviceDtoBindingSource.DataSource = deviceDtos;
                            deviceDtoBindingSource.ResetBindings(false);
                            DeviceDtoGridView.RefreshData();
                            UpdateStatusBar();
                        }));
                    });
                }
                finally
                {
                    SplashScreenHelper.CloseSplashScreen();
                }
            }
            catch (Exception ex)
            {
                _logger.Error("LoadDeviceDataAsync: Exception occurred", ex);
                SplashScreenHelper.CloseSplashScreen();
                MsgBox.ShowError($"Lỗi tải dữ liệu thiết bị: {ex.Message}");
            }
            finally
            {
                _isLoading = false;
            }
        }

        /// <summary>
        /// Event handler khi selection thay đổi trong GridView
        /// </summary>
        private void DeviceDtoGridView_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            try
            {
                UpdateStatusBar();
            }
            catch (Exception ex)
            {
                _logger.Error("DeviceDtoGridView_SelectionChanged: Exception occurred", ex);
            }
        }

        /// <summary>
        /// Cập nhật status bar với thông tin tổng hợp
        /// </summary>
        private void UpdateStatusBar()
        {
            try
            {
                var totalCount = deviceDtoBindingSource.Count;
                var selectedCount = DeviceDtoGridView.SelectedRowsCount;

                DataSummaryBarStaticItem.Caption = $"Tổng số: <b>{totalCount}</b> thiết bị";
                SelectedRowBarStaticItem.Caption = selectedCount > 0 
                    ? $"Đã chọn: <b>{selectedCount}</b> dòng" 
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