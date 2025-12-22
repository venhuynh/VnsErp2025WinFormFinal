using Bll.Inventory.InventoryManagement;
using Common.Common;
using Common.Utils;
using DevExpress.XtraBars.Docking;
using DTO.Inventory.InventoryManagement;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

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

            // Event khi click nút Lịch sử nhập - xuất
            ThemLichSuNhapXuatThietBiBarButtonItem.ItemClick += ThemLichSuNhapXuatThietBiBarButtonItem_ItemClick;

            // Event khi click nút Thêm hình ảnh
            ThemHinhAnhBarButtonItem.ItemClick += ThemHinhAnhBarButtonItem_ItemClick;

            // Event khi click nút Xem (reload dữ liệu)
            XemBarButtonItem.ItemClick += XemBarButtonItem_ItemClick;

            // Đăng ký event DeviceSaved từ ucDeviceDtoAddEdit1
            ucDeviceDtoAddEdit1.DeviceSaved += UcDeviceDtoAddEdit1_DeviceSaved;

            // Event khi selection thay đổi trong GridView
            DeviceDtoGridView.SelectionChanged += DeviceDtoGridView_SelectionChanged;

            // Event khi giá trị cell thay đổi (để lưu vào database)
            DeviceDtoGridView.CellValueChanged += DeviceDtoGridView_CellValueChanged;

            // Event để validate và convert giá trị trước khi set vào property
            DeviceDtoGridView.ValidatingEditor += DeviceDtoGridView_ValidatingEditor;

            // Event để hiển thị HTML với màu sắc cho colStatus
            DeviceDtoGridView.CustomColumnDisplayText += DeviceDtoGridView_CustomColumnDisplayText;
        }

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
        /// Hiển thị UserControl phù hợp trong dockPanel1
        /// </summary>
        /// <param name="controlType">Loại UserControl: "AddEdit", "StockInOutHistory", hoặc "ImageAdd"</param>
        private void ShowUserControlInDockPanel(string controlType)
        {
            try
            {
                // Ẩn tất cả UserControls trước
                ucDeviceDtoAddEdit1.Visible = false;
                ucDeviceDtoAddStockInOutHistory1.Visible = false;
                ucDeviceImageAdd1.Visible = false;
                layoutControlItem1.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItem2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItem3.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;

                // Hiển thị UserControl tương ứng
                switch (controlType)
                {
                    case "AddEdit":
                        ucDeviceDtoAddEdit1.Visible = true;
                        layoutControlItem1.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        dockPanel1.Text = "Thêm mới thiết bị";
                        break;

                    case "StockInOutHistory":
                        ucDeviceDtoAddStockInOutHistory1.Visible = true;
                        layoutControlItem2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        dockPanel1.Text = "Lịch sử nhập - xuất thiết bị";
                        break;

                    case "ImageAdd":
                        ucDeviceImageAdd1.Visible = true;
                        layoutControlItem3.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        dockPanel1.Text = "Thêm hình ảnh thiết bị";
                        break;

                    default:
                        _logger.Warning($"ShowUserControlInDockPanel: Unknown controlType '{controlType}'");
                        break;
                }

                // Refresh layout
                layoutControl1.Refresh();
            }
            catch (Exception ex)
            {
                _logger.Error("ShowUserControlInDockPanel: Exception occurred", ex);
                throw;
            }
        }

        #endregion

        /// <summary>
        /// Event handler khi click nút Thêm mới
        /// Mở dockPanel1 với độ rộng = 2/3 độ rộng màn hình và hiển thị ucDeviceDtoAddEdit1
        /// </summary>
        private void ThemMoiBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                // Thiết lập độ rộng dock panel
                SetupDockPanelWidth();

                // Hiển thị UserControl thêm mới
                ShowUserControlInDockPanel("AddEdit");
            }
            catch (Exception ex)
            {
                _logger.Error("ThemMoiBarButtonItem_ItemClick: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi mở panel thêm mới: {ex.Message}");
            }
        }

        /// <summary>
        /// Event handler khi click nút Lịch sử nhập - xuất
        /// Mở dockPanel1 với độ rộng = 2/3 độ rộng màn hình và hiển thị ucDeviceDtoAddStockInOutHistory1
        /// Chỉ thực hiện khi có thiết bị được chọn
        /// </summary>
        private void ThemLichSuNhapXuatThietBiBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                // Kiểm tra xem có thiết bị nào được chọn không
                var selectedRows = DeviceDtoGridView.GetSelectedRows();
                if (selectedRows == null || selectedRows.Length == 0)
                {
                    MsgBox.ShowWarning("Vui lòng chọn thiết bị để xem lịch sử nhập - xuất.", "Chưa chọn thiết bị");
                    return;
                }

                // Lấy tất cả các thiết bị được chọn
                var selectedDevices = new List<DeviceDto>();
                foreach (var rowHandle in selectedRows)
                {
                    if (rowHandle < 0) continue;

                    var device = DeviceDtoGridView.GetRow(rowHandle) as DeviceDto;
                    if (device != null)
                    {
                        selectedDevices.Add(device);
                    }
                }

                if (selectedDevices.Count == 0)
                {
                    MsgBox.ShowWarning("Không thể lấy thông tin thiết bị được chọn.", "Lỗi");
                    return;
                }

                _logger.Debug($"ThemLichSuNhapXuatThietBiBarButtonItem_ItemClick: Selected {selectedDevices.Count} device(s)");

                // Thiết lập độ rộng dock panel
                SetupDockPanelWidth();

                // Hiển thị UserControl lịch sử nhập - xuất
                ShowUserControlInDockPanel("StockInOutHistory");

                // Load danh sách thiết bị đã chọn vào SearchLookUpEdit
                ucDeviceDtoAddStockInOutHistory1.LoadSelectedDevices(selectedDevices);
            }
            catch (Exception ex)
            {
                _logger.Error("ThemLichSuNhapXuatThietBiBarButtonItem_ItemClick: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi mở màn hình lịch sử nhập - xuất: {ex.Message}");
            }
        }

        /// <summary>
        /// Event handler khi click nút Thêm hình ảnh
        /// Mở dockPanel1 với độ rộng = 2/3 độ rộng màn hình và hiển thị ucDeviceImageAdd1
        /// Chỉ thực hiện khi có thiết bị được chọn
        /// </summary>
        private void ThemHinhAnhBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                // Kiểm tra xem có thiết bị nào được chọn không
                var selectedRows = DeviceDtoGridView.GetSelectedRows();
                if (selectedRows == null || selectedRows.Length == 0)
                {
                    MsgBox.ShowWarning("Vui lòng chọn thiết bị để thêm hình ảnh.", "Chưa chọn thiết bị");
                    return;
                }

                // Lấy tất cả các thiết bị được chọn
                var selectedDevices = new List<DeviceDto>();
                foreach (var rowHandle in selectedRows)
                {
                    if (rowHandle < 0) continue;

                    var device = DeviceDtoGridView.GetRow(rowHandle) as DeviceDto;
                    if (device != null)
                    {
                        selectedDevices.Add(device);
                    }
                }

                if (selectedDevices.Count == 0)
                {
                    MsgBox.ShowWarning("Không thể lấy thông tin thiết bị được chọn.", "Lỗi");
                    return;
                }

                _logger.Debug($"ThemHinhAnhBarButtonItem_ItemClick: Selected {selectedDevices.Count} device(s)");

                // Thiết lập độ rộng dock panel
                SetupDockPanelWidth();

                // Hiển thị UserControl thêm hình ảnh
                ShowUserControlInDockPanel("ImageAdd");

                // Load danh sách thiết bị đã chọn vào UserControl
                ucDeviceImageAdd1.LoadSelectedDevices(selectedDevices);
            }
            catch (Exception ex)
            {
                _logger.Error("ThemHinhAnhBarButtonItem_ItemClick: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi mở màn hình thêm hình ảnh: {ex.Message}");
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
                // Xóa các items cũ
                StatusComboBox.Items.Clear();

                // Thêm các tùy chọn xác thực sử dụng ApplicationEnumUtils
                // Màu sắc được thiết lập trong CustomDrawItem event
                foreach (DeviceStatusEnum value in Enum.GetValues(typeof(DeviceStatusEnum)))
                {
                    int index = ApplicationEnumUtils.GetValue(value);
                    //string itemName = ApplicationEnumUtils.GetDescription(value);

                    // Lấy Description và màu sắc
                    var description = GetStatusDescription(value);
                    var colorHex = GetStatusColor(value);

                    // Tạo HTML với màu sắc
                    string itemName = $"<color='{colorHex}'>{description}</color>";

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
        /// Event handler để hiển thị Description với màu sắc HTML trong StatusComboBox
        /// </summary>
        private void StatusComboBox_CustomDisplayText(object sender, DevExpress.XtraEditors.Controls.CustomDisplayTextEventArgs e)
        {
            try
            {
                if (e.Value == null) return;

                DeviceStatusEnum statusValue;

                // Nếu giá trị là string (Description), convert về enum
                if (e.Value is string stringValue)
                {
                    var statusEnum = GetStatusEnumFromDescription(stringValue);
                    if (!statusEnum.HasValue)
                    {
                        e.DisplayText = stringValue;
                        return;
                    }
                    statusValue = statusEnum.Value;
                }
                else if (e.Value is DeviceStatusEnum enumValue)
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

                // Lấy Description và màu sắc
                var description = GetStatusDescription(statusValue);
                var colorHex = GetStatusColor(statusValue);

                // Tạo HTML với màu sắc
                e.DisplayText = $"<color='{colorHex}'>{description}</color>";
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
        /// Lấy màu sắc tương ứng với trạng thái thiết bị
        /// </summary>
        /// <param name="status">Trạng thái thiết bị</param>
        /// <returns>Tên màu (color name)</returns>
        private string GetStatusColor(DeviceStatusEnum status)
        {
            return status switch
            {
                DeviceStatusEnum.Available => "green",         // Green - Đang trong kho VNS
                DeviceStatusEnum.InUse => "blue",              // Blue - Đang sử dụng tại VNS
                DeviceStatusEnum.Maintenance => "orange",      // Orange - Đang gửi bảo hành
                DeviceStatusEnum.Broken => "red",              // Red - Đã hỏng
                DeviceStatusEnum.Disposed => "gray",           // Grey - Đã thanh lý
                DeviceStatusEnum.Reserved => "purple",         // Purple - Đã giao cho khách hàng
                _ => "black"                                   // Default - Black
            };
        }

        /// <summary>
        /// Lấy enum value từ Description string (có thể chứa HTML tags)
        /// </summary>
        /// <param name="description">Description string (có thể chứa HTML tags)</param>
        /// <returns>Enum value hoặc null nếu không tìm thấy</returns>
        private DeviceStatusEnum? GetStatusEnumFromDescription(string description)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(description))
                    return null;

                // Strip HTML tags nếu có
                var cleanDescription = StripHtmlTags(description);

                // Duyệt qua tất cả các giá trị enum để tìm Description khớp
                foreach (DeviceStatusEnum enumValue in Enum.GetValues(typeof(DeviceStatusEnum)))
                {
                    var enumDescription = ApplicationEnumUtils.GetDescription(enumValue);
                    if (string.Equals(enumDescription, cleanDescription, StringComparison.OrdinalIgnoreCase))
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
        /// Loại bỏ HTML tags từ string
        /// </summary>
        /// <param name="htmlString">String chứa HTML tags</param>
        /// <returns>String không có HTML tags</returns>
        private string StripHtmlTags(string htmlString)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(htmlString))
                    return htmlString;

                // Loại bỏ các HTML tags phổ biến của DevExpress: <color>, <b>, <i>, <size>, etc.
                var result = htmlString;

                // Loại bỏ <color='...'> và </color>
                result = Regex.Replace(result, @"<color=['""][^'""]*['""]>", "", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"</color>", "", RegexOptions.IgnoreCase);

                // Loại bỏ các tags khác nếu có
                result = Regex.Replace(result, @"<[^>]+>", "");

                return result.Trim();
            }
            catch (Exception ex)
            {
                _logger.Error($"StripHtmlTags: Exception occurred for '{htmlString}'", ex);
                return htmlString;
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

                // Cho phép hiển thị HTML cho colStatus
                //colStatus.OptionsColumn.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;

                // Các cột khác không cho phép edit
                colHtmlInfo.OptionsColumn.AllowEdit = false;
            }
            catch (Exception ex)
            {
                _logger.Error("ConfigureGridViewEditing: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi cấu hình GridView: {ex.Message}");
            }
        }

        /// <summary>
        /// Event handler để hiển thị HTML với màu sắc cho colStatus trong GridView
        /// </summary>
        private void DeviceDtoGridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            try
            {
                // Chỉ xử lý cột Status
                if (e.Column != colStatus || e.Value == null)
                    return;

                DeviceStatusEnum statusValue;

                // Convert giá trị về enum
                if (e.Value is DeviceStatusEnum enumValue)
                {
                    statusValue = enumValue;
                }
                else if (e.Value is int intValue && Enum.IsDefined(typeof(DeviceStatusEnum), intValue))
                {
                    statusValue = (DeviceStatusEnum)intValue;
                }
                else if (e.Value is string stringValue)
                {
                    // Nếu là string, thử strip HTML và convert
                    var cleanString = StripHtmlTags(stringValue);
                    var statusEnum = GetStatusEnumFromDescription(cleanString);
                    if (!statusEnum.HasValue)
                    {
                        return; // Không thể convert, giữ nguyên giá trị
                    }
                    statusValue = statusEnum.Value;
                }
                else
                {
                    return;
                }

                // Lấy Description và màu sắc
                var description = GetStatusDescription(statusValue);
                var colorName = GetStatusColor(statusValue);

                // Tạo HTML với màu sắc
                e.DisplayText = $"<color='{colorName}'>{description}</color>";
            }
            catch (Exception ex)
            {
                _logger.Error("DeviceDtoGridView_CustomColumnDisplayText: Exception occurred", ex);
                // Nếu có lỗi, giữ nguyên giá trị mặc định
            }
        }

        #endregion

        #region ========== GRIDVIEW EVENTS ==========

        /// <summary>
        /// Event handler để validate và convert giá trị trước khi set vào property
        /// Xử lý conversion từ string (Description) sang enum
        /// </summary>
        private void DeviceDtoGridView_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            try
            {
                var view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
                if (view == null) return;

                var focusedColumn = view.FocusedColumn;
                if (focusedColumn != colStatus) return;

                // Nếu giá trị là string (Description), convert về enum
                if (e.Value is string statusDescription)
                {
                    var statusEnum = GetStatusEnumFromDescription(statusDescription);
                    if (statusEnum.HasValue)
                    {
                        // Set lại giá trị là enum để DevExpress có thể bind đúng
                        e.Value = statusEnum.Value;
                        e.Valid = true;
                    }
                    else
                    {
                        _logger.Warning($"DeviceDtoGridView_ValidatingEditor: Cannot convert status description '{statusDescription}' to enum");
                        e.ErrorText = $"Không thể chuyển đổi trạng thái '{statusDescription}'";
                        e.Valid = false;
                    }
                }
                // Nếu giá trị đã là enum, giữ nguyên
                else if (e.Value is DeviceStatusEnum)
                {
                    e.Valid = true;
                }
                // Nếu giá trị là int, convert về enum
                else if (e.Value is int intValue && Enum.IsDefined(typeof(DeviceStatusEnum), intValue))
                {
                    e.Value = (DeviceStatusEnum)intValue;
                    e.Valid = true;
                }
            }
            catch (Exception ex)
            {
                _logger.Error("DeviceDtoGridView_ValidatingEditor: Exception occurred", ex);
                e.ErrorText = $"Lỗi xử lý giá trị: {ex.Message}";
                e.Valid = false;
            }
        }

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