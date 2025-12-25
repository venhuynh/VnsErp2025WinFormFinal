using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Bll.Inventory.InventoryManagement;
using Common.Common;
using Common.Enums;
using Common.Utils;
using DTO.DeviceAssetManagement;
using DTO.Inventory.InventoryManagement;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;

namespace DeviceAssetManagement.Management.DeviceMangement
{
    public partial class UcDeviceWarranty : DevExpress.XtraEditors.XtraUserControl
    {
        #region ========== FIELDS & PROPERTIES ==========

        /// <summary>
        /// Business Logic Layer cho Warranty
        /// </summary>
        private WarrantyBll _warrantyBll;

        /// <summary>
        /// Business Logic Layer cho Device
        /// </summary>
        private DeviceBll _deviceBll;

        /// <summary>
        /// Logger để ghi log các sự kiện
        /// </summary>
        private ILogger _logger;

        /// <summary>
        /// Danh sách thiết bị đã chọn
        /// </summary>
        private List<DeviceDto> _selectedDevices;

        #endregion

        #region ========== CONSTRUCTOR ==========

        public UcDeviceWarranty()
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
                // Khởi tạo logger
                _logger = LoggerFactory.CreateLogger(LogCategory.UI);

                // Khởi tạo BLL
                _warrantyBll = new WarrantyBll();
                _deviceBll = new DeviceBll();

                // Khởi tạo danh sách thiết bị đã chọn
                _selectedDevices = [];

                // Gán nguồn dữ liệu cho gridControl
                warrantyDtoBindingSource.DataSource = typeof(WarrantyDto);

                // Load LoaiBaoHanhComboBoxEdit với các giá trị enum
                LoadLoaiBaoHanhComboBox();

                // Setup events
                InitializeEvents();

                // Setup SuperToolTips
                SetupSuperToolTips();
            }
            catch (Exception ex)
            {
                _logger?.Error($"InitializeControl error: {ex.Message}", ex);
                System.Diagnostics.Debug.WriteLine($"InitializeControl error: {ex.Message}");
            }
        }

        /// <summary>
        /// Khởi tạo các event handlers
        /// </summary>
        private void InitializeEvents()
        {
            // Đăng ký event handler cho nút Thêm vào
            ThemVaoHyperlinkLabelControl.Click += ThemVaoHyperlinkLabelControl_Click;

            // Đăng ký event handler cho nút Bỏ ra
            BoRaHyperlinkLabelControl.Click += BoRaHyperlinkLabelControl_Click;

            // Đăng ký event handler cho nút Lưu
            SaveBarButtonItem.ItemClick += SaveBarButtonItem_ItemClick;

            // Đăng ký event handler cho nút Đóng
            CloseBarButtonItem.ItemClick += CloseBarButtonItem_ItemClick;

            // Đăng ký event handler cho MonthOfWarrantyTextEdit để tự động tính ngày hết hạn
            MonthOfWarrantyTextEdit.EditValueChanged += MonthOfWarrantyTextEdit_EditValueChanged;

            // Đăng ký event handler cho WarrantyFromDateEdit để tự động tính ngày hết hạn
            WarrantyFromDateEdit.EditValueChanged += WarrantyFromDateEdit_EditValueChanged;
        }

        #endregion

        #region ========== SUPERTOOLTIP ==========

        /// <summary>
        /// Thiết lập SuperToolTip cho tất cả các controls trong UserControl
        /// </summary>
        private void SetupSuperToolTips()
        {
            try
            {
                SetupHyperlinkLabelControlSuperTips();
            }
            catch (Exception ex)
            {
                _logger?.Error($"SetupSuperToolTips error: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Thiết lập SuperToolTip cho HyperlinkLabelControl controls
        /// </summary>
        private void SetupHyperlinkLabelControlSuperTips()
        {
            // SuperTip cho nút Thêm vào
            if (ThemVaoHyperlinkLabelControl != null)
            {
                ThemVaoHyperlinkLabelControl.SuperTip = SuperToolTipHelper.CreateSuperToolTip(
                    title: @"<b><color=Green>➕ Thêm bảo hành</color></b>",
                    content: @"Thêm <b>bảo hành mới</b> vào danh sách từ thông tin đã nhập.<br/><br/><b>Chức năng:</b><br/>• Tạo WarrantyDto từ thông tin đã nhập<br/>• Thêm vào danh sách trong GridView<br/>• Chưa lưu vào database (chỉ lưu khi click Lưu)<br/><br/><b>Thông tin cần nhập:</b><br/>• Loại bảo hành (bắt buộc)<br/>• Từ ngày, số tháng bảo hành<br/>• Đến ngày (tự động tính nếu có từ ngày và số tháng)<br/><br/><b>Validation:</b><br/>• Kiểm tra đã chọn loại bảo hành<br/>• Kiểm tra đã nhập số tháng bảo hành<br/>• Tự động tính đến ngày nếu có từ ngày và số tháng<br/><br/><color=Gray>Lưu ý:</color> Cần chọn thiết bị trước khi thêm bảo hành."
                );
            }

            // SuperTip cho nút Bỏ ra
            if (BoRaHyperlinkLabelControl != null)
            {
                BoRaHyperlinkLabelControl.SuperTip = SuperToolTipHelper.CreateSuperToolTip(
                    title: @"<b><color=Red>🗑️ Xóa bảo hành</color></b>",
                    content: @"Xóa <b>các dòng bảo hành được chọn</b> khỏi danh sách.<br/><br/><b>Chức năng:</b><br/>• Xóa các WarrantyDto được chọn trong GridView<br/>• Hỗ trợ xóa nhiều dòng cùng lúc<br/>• Chỉ xóa khỏi danh sách, chưa xóa khỏi database<br/>• Xóa khỏi database khi click Lưu<br/><br/><b>Quy trình:</b><br/>1. Kiểm tra có dòng nào được chọn không<br/>2. Xóa khỏi BindingSource<br/>3. Refresh GridView<br/><br/><color=Gray>Lưu ý:</color> Cần click Lưu để xóa khỏi database."
                );
            }
        }

        #endregion

        #region ========== DEVICE MANAGEMENT ==========

        /// <summary>
        /// Load danh sách bảo hành dựa trên các thiết bị đã chọn
        /// </summary>
        /// <param name="selectedDevices">Danh sách thiết bị đã chọn</param>
        public void LoadSelectedDevices(List<DeviceDto> selectedDevices)
        {
            try
            {
                // Lưu danh sách thiết bị đã chọn
                _selectedDevices = selectedDevices ?? new List<DeviceDto>();

                if (_selectedDevices.Count == 0)
                {
                    warrantyDtoBindingSource.DataSource = new List<WarrantyDto>();
                    warrantyDtoBindingSource.ResetBindings(false);
                    return;
                }

                _logger?.Debug($"LoadSelectedDevices: Bắt đầu load, SelectedDevices count={_selectedDevices.Count}");

                // Lấy danh sách DeviceId
                var deviceIds = _selectedDevices
                    .Where(d => d != null && d.Id != Guid.Empty)
                    .Select(d => d.Id)
                    .Distinct()
                    .ToList();

                if (deviceIds.Count == 0)
                {
                    _logger?.Warning("LoadSelectedDevices: Không có DeviceId hợp lệ");
                    warrantyDtoBindingSource.DataSource = new List<WarrantyDto>();
                    warrantyDtoBindingSource.ResetBindings(false);
                    return;
                }

                // Query bảo hành cho tất cả các thiết bị
                var allWarranties = new List<WarrantyDto>();
                foreach (var deviceId in deviceIds)
                {
                    var warranty = _warrantyBll.FindByDeviceId(deviceId);
                    if (warranty != null)
                    {
                        var warrantyDto = warranty.ToDto();
                        if (warrantyDto != null)
                        {
                            // Enrich DeviceInfo từ selectedDevices nếu chưa có
                            if (string.IsNullOrWhiteSpace(warrantyDto.DeviceInfo))
                            {
                                var device = _selectedDevices.FirstOrDefault(d => d.Id == deviceId);
                                if (device != null)
                                {
                                    var deviceInfoParts = new List<string>();
                                    if (!string.IsNullOrWhiteSpace(device.SerialNumber))
                                        deviceInfoParts.Add($"S/N: {device.SerialNumber}");
                                    if (!string.IsNullOrWhiteSpace(device.IMEI))
                                        deviceInfoParts.Add($"IMEI: {device.IMEI}");
                                    if (!string.IsNullOrWhiteSpace(device.MACAddress))
                                        deviceInfoParts.Add($"MAC: {device.MACAddress}");
                                    if (!string.IsNullOrWhiteSpace(device.AssetTag))
                                        deviceInfoParts.Add($"Asset: {device.AssetTag}");
                                    if (!string.IsNullOrWhiteSpace(device.LicenseKey))
                                        deviceInfoParts.Add($"License: {device.LicenseKey}");
                                    
                                    warrantyDto.DeviceInfo = string.Join(" | ", deviceInfoParts);
                                }
                            }
                            
                            allWarranties.Add(warrantyDto);
                        }
                    }
                }

                // Sắp xếp theo ngày bắt đầu bảo hành
                allWarranties = allWarranties
                    .OrderByDescending(w => w.WarrantyFrom ?? DateTime.MinValue)
                    .ThenByDescending(w => w.CreatedDate)
                    .ToList();

                _logger?.Info($"LoadSelectedDevices: Load thành công, Warranties count={allWarranties.Count}");

                // Load vào BindingSource
                warrantyDtoBindingSource.DataSource = allWarranties;
                warrantyDtoBindingSource.ResetBindings(false);
            }
            catch (Exception ex)
            {
                _logger?.Error($"LoadSelectedDevices error: {ex.Message}", ex);
                System.Diagnostics.Debug.WriteLine($"LoadSelectedDevices error: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region ========== ADD WARRANTY ==========

        /// <summary>
        /// Event handler cho nút Thêm vào
        /// Thêm bảo hành mới vào danh sách từ thông tin đã nhập
        /// </summary>
        private void ThemVaoHyperlinkLabelControl_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra có thiết bị nào được chọn không
                if (_selectedDevices == null || _selectedDevices.Count == 0)
                {
                    MsgBox.ShowWarning("Vui lòng chọn thiết bị trước khi thêm bảo hành.", "Chưa chọn thiết bị");
                    return;
                }

                // Validate input
                if (!ValidateInput())
                {
                    return;
                }

                // Lấy thông tin từ form
                var warrantyType = GetSelectedWarrantyType();
                if (!warrantyType.HasValue)
                {
                    MsgBox.ShowWarning("Vui lòng chọn loại bảo hành.", "Chưa chọn loại bảo hành");
                    return;
                }

                var warrantyFrom = WarrantyFromDateEdit.EditValue as DateTime?;
                var monthOfWarranty = 0;
                if (int.TryParse(MonthOfWarrantyTextEdit.EditValue?.ToString(), out var monthValue))
                {
                    monthOfWarranty = monthValue;
                }

                // Tính đến ngày nếu có từ ngày và số tháng
                DateTime? warrantyUntil = null;
                if (warrantyFrom.HasValue && monthOfWarranty > 0)
                {
                    warrantyUntil = warrantyFrom.Value.AddMonths(monthOfWarranty);
                }
                else
                {
                    warrantyUntil = WarrantyUntilDateEdit.EditValue as DateTime?;
                }

                // Tạo WarrantyDto mới cho mỗi thiết bị
                var newWarranties = new List<WarrantyDto>();
                foreach (var device in _selectedDevices)
                {
                    if (device == null || device.Id == Guid.Empty)
                        continue;

                    // Kiểm tra xem đã có bảo hành cho thiết bị này chưa
                    var existingWarranties = warrantyDtoBindingSource.DataSource as List<WarrantyDto>;
                    if (existingWarranties != null && existingWarranties.Any(w => w.DeviceId == device.Id))
                    {
                        _logger?.Warning($"ThemVaoHyperlinkLabelControl_Click: Đã tồn tại bảo hành cho DeviceId={device.Id}");
                        continue; // Bỏ qua thiết bị đã có bảo hành
                    }

                    // Lấy thông tin định danh thiết bị
                    var deviceInfoParts = new List<string>();
                    if (!string.IsNullOrWhiteSpace(device.SerialNumber))
                        deviceInfoParts.Add($"S/N: {device.SerialNumber}");
                    if (!string.IsNullOrWhiteSpace(device.IMEI))
                        deviceInfoParts.Add($"IMEI: {device.IMEI}");
                    if (!string.IsNullOrWhiteSpace(device.MACAddress))
                        deviceInfoParts.Add($"MAC: {device.MACAddress}");
                    if (!string.IsNullOrWhiteSpace(device.AssetTag))
                        deviceInfoParts.Add($"Asset: {device.AssetTag}");
                    if (!string.IsNullOrWhiteSpace(device.LicenseKey))
                        deviceInfoParts.Add($"License: {device.LicenseKey}");

                    var warrantyDto = new WarrantyDto
                    {
                        Id = Guid.NewGuid(),
                        DeviceId = device.Id,
                        WarrantyType = warrantyType.Value,
                        WarrantyFrom = warrantyFrom,
                        MonthOfWarranty = monthOfWarranty,
                        WarrantyUntil = warrantyUntil,
                        WarrantyStatus = TrangThaiBaoHanhEnum.ChoXuLy,
                        Notes = string.Empty,
                        IsActive = true,
                        CreatedDate = DateTime.Now,
                        DeviceInfo = string.Join(" | ", deviceInfoParts)
                    };

                    newWarranties.Add(warrantyDto);
                }

                if (newWarranties.Count == 0)
                {
                    MsgBox.ShowWarning("Tất cả các thiết bị đã có bảo hành hoặc không hợp lệ.", "Thông báo");
                    return;
                }

                // Thêm vào BindingSource
                var currentWarranties = warrantyDtoBindingSource.DataSource as List<WarrantyDto> ?? new List<WarrantyDto>();
                currentWarranties.AddRange(newWarranties);
                warrantyDtoBindingSource.DataSource = currentWarranties;
                warrantyDtoBindingSource.ResetBindings(false);

                _logger?.Info($"ThemVaoHyperlinkLabelControl_Click: Đã thêm {newWarranties.Count} bảo hành vào danh sách");

                // Clear form
                ClearForm();
            }
            catch (Exception ex)
            {
                _logger?.Error($"ThemVaoHyperlinkLabelControl_Click: Exception, Error={ex.Message}", ex);
                MsgBox.ShowError($"Lỗi: {ex.Message}", "Lỗi");
            }
        }

        /// <summary>
        /// Validate input từ form
        /// </summary>
        private bool ValidateInput()
        {
            // Kiểm tra loại bảo hành
            var warrantyType = GetSelectedWarrantyType();
            if (!warrantyType.HasValue)
            {
                MsgBox.ShowWarning("Vui lòng chọn loại bảo hành.", "Chưa chọn loại bảo hành");
                return false;
            }

            // Kiểm tra số tháng bảo hành
            if (!int.TryParse(MonthOfWarrantyTextEdit.EditValue?.ToString(), out var monthValue) || monthValue <= 0)
            {
                MsgBox.ShowWarning("Vui lòng nhập số tháng bảo hành hợp lệ (lớn hơn 0).", "Số tháng bảo hành không hợp lệ");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Lấy loại bảo hành đã chọn từ ComboBox
        /// </summary>
        private LoaiBaoHanhEnum? GetSelectedWarrantyType()
        {
            try
            {
                var selectedValue = LoaiBaoHanhComboBoxEdit.EditValue;
                if (selectedValue == null) return null;

                if (selectedValue is LoaiBaoHanhEnum enumValue)
                {
                    return enumValue;
                }

                if (selectedValue is int intValue && Enum.IsDefined(typeof(LoaiBaoHanhEnum), intValue))
                {
                    return (LoaiBaoHanhEnum)intValue;
                }

                if (selectedValue is string stringValue)
                {
                    var cleanString = StripHtmlTags(stringValue);
                    return GetWarrantyTypeEnumFromDescription(cleanString);
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger?.Error($"GetSelectedWarrantyType: Exception, Error={ex.Message}", ex);
                return null;
            }
        }

        /// <summary>
        /// Clear form input
        /// </summary>
        private void ClearForm()
        {
            try
            {
                LoaiBaoHanhComboBoxEdit.EditValue = null;
                WarrantyFromDateEdit.EditValue = null;
                MonthOfWarrantyTextEdit.EditValue = null;
                WarrantyUntilDateEdit.EditValue = null;
            }
            catch (Exception ex)
            {
                _logger?.Error($"ClearForm: Exception, Error={ex.Message}", ex);
            }
        }

        #endregion

        #region ========== DELETE WARRANTY ==========

        /// <summary>
        /// Event handler cho nút Bỏ ra
        /// Xóa các dòng bảo hành được chọn khỏi danh sách
        /// </summary>
        private void BoRaHyperlinkLabelControl_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra có dòng nào được chọn không
                var selectedRowHandles = WarrantyDtoGridView.GetSelectedRows();
                if (selectedRowHandles == null || selectedRowHandles.Length == 0)
                {
                    MsgBox.ShowWarning("Vui lòng chọn ít nhất một dòng bảo hành để xóa.", "Chưa chọn dòng");
                    return;
                }

                // Lấy danh sách DTO được chọn
                var selectedDtos = selectedRowHandles
                    .Where(handle => handle >= 0)
                    .Select(handle => WarrantyDtoGridView.GetRow(handle) as WarrantyDto)
                    .Where(dto => dto != null && dto.Id != Guid.Empty)
                    .ToList();

                if (selectedDtos.Count == 0)
                {
                    MsgBox.ShowWarning("Không có dòng bảo hành hợp lệ để xóa.", "Lỗi");
                    return;
                }

                _logger?.Debug($"BoRaHyperlinkLabelControl_Click: Bắt đầu xóa, Selected count={selectedDtos.Count}");

                // Xóa khỏi BindingSource
                var currentWarranties = warrantyDtoBindingSource.DataSource as List<WarrantyDto>;
                if (currentWarranties != null)
                {
                    var idsToRemove = selectedDtos.Select(d => d.Id).ToHashSet();
                    currentWarranties.RemoveAll(w => idsToRemove.Contains(w.Id));
                    warrantyDtoBindingSource.DataSource = currentWarranties;
                    warrantyDtoBindingSource.ResetBindings(false);
                }

                _logger?.Info($"BoRaHyperlinkLabelControl_Click: Đã xóa {selectedDtos.Count} bảo hành khỏi danh sách");
            }
            catch (Exception ex)
            {
                _logger?.Error($"BoRaHyperlinkLabelControl_Click: Exception, Error={ex.Message}", ex);
                MsgBox.ShowError($"Lỗi: {ex.Message}", "Lỗi");
            }
        }

        #endregion

        #region ========== SAVE WARRANTY ==========

        /// <summary>
        /// Event handler cho nút Lưu
        /// Lưu tất cả bảo hành trong danh sách vào database
        /// </summary>
        private void SaveBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                var warranties = warrantyDtoBindingSource.DataSource as List<WarrantyDto>;
                if (warranties == null || warranties.Count == 0)
                {
                    MsgBox.ShowWarning("Không có bảo hành nào để lưu.", "Thông báo");
                    return;
                }

                _logger?.Debug($"SaveBarButtonItem_ItemClick: Bắt đầu lưu, Warranties count={warranties.Count}");

                // Hiển thị splash screen
                SplashScreenHelper.ShowWaitingSplashScreen();

                try
                {
                    var successCount = 0;
                    var errorCount = 0;
                    var errorMessages = new List<string>();

                    // Lưu từng bảo hành
                    foreach (var warrantyDto in warranties)
                    {
                        try
                        {
                            //Set lại trạng thái
                            warrantyDto.WarrantyStatus = TrangThaiBaoHanhEnum.DangBaoHanh;

                            // Convert DTO sang Entity
                            var warrantyEntity = warrantyDto.ToEntity();
                            
                            // Lưu vào database
                            _warrantyBll.SaveOrUpdate(warrantyEntity);
                            
                            successCount++;
                            _logger?.Debug($"SaveBarButtonItem_ItemClick: Lưu thành công, Id={warrantyDto.Id}");
                        }
                        catch (Exception ex)
                        {
                            errorCount++;
                            var errorMsg = $"Bảo hành {warrantyDto.Id}: {ex.Message}";
                            errorMessages.Add(errorMsg);
                            _logger?.Error($"SaveBarButtonItem_ItemClick: Lỗi lưu Id={warrantyDto.Id}, Error={ex.Message}", ex);
                        }
                    }

                    // Đóng splash screen
                    SplashScreenHelper.CloseSplashScreen();

                    // Hiển thị kết quả
                    if (successCount > 0)
                    {
                        var message = $"Đã lưu thành công {successCount} bảo hành.";
                        if (errorCount > 0)
                        {
                            message += $"\nCó {errorCount} bảo hành gặp lỗi.";
                            if (errorMessages.Any())
                            {
                                message += "\n\nChi tiết lỗi:\n" + string.Join("\n", errorMessages);
                            }
                            MsgBox.ShowWarning(message, "Kết quả");
                        }
                        else
                        {
                            MsgBox.ShowSuccess(message, "Thành công");
                        }

                        // Load lại danh sách từ database
                        LoadSelectedDevices(_selectedDevices);

                        // Trigger event để form cha refresh dữ liệu (khi có ít nhất một bản ghi thành công)
                        if (successCount > 0)
                        {
                            OnWarrantySaved();
                        }
                    }
                    else
                    {
                        var message = "Không thể lưu bất kỳ bảo hành nào.";
                        if (errorMessages.Any())
                        {
                            message += "\n\nChi tiết lỗi:\n" + string.Join("\n", errorMessages);
                        }
                        MsgBox.ShowError(message, "Lỗi");
                    }
                }
                catch (Exception ex)
                {
                    SplashScreenHelper.CloseSplashScreen();
                    _logger?.Error($"SaveBarButtonItem_ItemClick: Lỗi tổng quát, Error={ex.Message}", ex);
                    MsgBox.ShowError($"Lỗi khi lưu bảo hành: {ex.Message}", "Lỗi");
                }
            }
            catch (Exception ex)
            {
                _logger?.Error($"SaveBarButtonItem_ItemClick: Exception, Error={ex.Message}", ex);
                MsgBox.ShowError($"Lỗi: {ex.Message}", "Lỗi");
            }
        }

        /// <summary>
        /// Event handler cho nút Đóng
        /// </summary>
        private void CloseBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                // Trigger event để form cha xử lý đóng
                OnWarrantyClosed();
            }
            catch (Exception ex)
            {
                _logger?.Error($"CloseBarButtonItem_ItemClick: Exception, Error={ex.Message}", ex);
                MsgBox.ShowError($"Lỗi: {ex.Message}", "Lỗi");
            }
        }

        /// <summary>
        /// Event được trigger khi bảo hành được lưu thành công
        /// </summary>
        public event EventHandler WarrantySaved;

        /// <summary>
        /// Trigger event WarrantySaved
        /// </summary>
        protected virtual void OnWarrantySaved()
        {
            WarrantySaved?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Event được trigger khi nút Đóng được click
        /// </summary>
        public event EventHandler WarrantyClosed;

        /// <summary>
        /// Trigger event WarrantyClosed
        /// </summary>
        protected virtual void OnWarrantyClosed()
        {
            WarrantyClosed?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region ========== AUTO CALCULATE WARRANTY UNTIL ==========

        /// <summary>
        /// Event handler khi MonthOfWarrantyTextEdit thay đổi giá trị
        /// Tự động tính và set ngày hết hạn bảo hành nếu có ngày bắt đầu
        /// </summary>
        private void MonthOfWarrantyTextEdit_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                CalculateWarrantyUntil();
            }
            catch (Exception ex)
            {
                _logger?.Error($"MonthOfWarrantyTextEdit_EditValueChanged: Exception, Error={ex.Message}", ex);
            }
        }

        /// <summary>
        /// Event handler khi WarrantyFromDateEdit thay đổi giá trị
        /// Tự động tính và set ngày hết hạn bảo hành nếu có số tháng bảo hành
        /// </summary>
        private void WarrantyFromDateEdit_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                CalculateWarrantyUntil();
            }
            catch (Exception ex)
            {
                _logger?.Error($"WarrantyFromDateEdit_EditValueChanged: Exception, Error={ex.Message}", ex);
            }
        }

        /// <summary>
        /// Tính toán và set ngày hết hạn bảo hành dựa trên ngày bắt đầu và số tháng bảo hành
        /// </summary>
        private void CalculateWarrantyUntil()
        {
            try
            {
                // Lấy ngày bắt đầu bảo hành
                var warrantyFrom = WarrantyFromDateEdit.EditValue as DateTime?;
                if (!warrantyFrom.HasValue)
                {
                    return; // Không có ngày bắt đầu, không tính
                }

                // Lấy số tháng bảo hành
                var monthOfWarranty = 0;
                if (!int.TryParse(MonthOfWarrantyTextEdit.EditValue?.ToString(), out monthOfWarranty) || monthOfWarranty <= 0)
                {
                    return; // Không có số tháng hợp lệ, không tính
                }

                // Tính ngày hết hạn = ngày bắt đầu + số tháng
                var warrantyUntil = warrantyFrom.Value.AddMonths(monthOfWarranty);

                // Set giá trị vào WarrantyUntilDateEdit
                WarrantyUntilDateEdit.EditValue = warrantyUntil;

                _logger?.Debug($"CalculateWarrantyUntil: Đã tính ngày hết hạn, From={warrantyFrom.Value:dd/MM/yyyy}, Months={monthOfWarranty}, Until={warrantyUntil:dd/MM/yyyy}");
            }
            catch (Exception ex)
            {
                _logger?.Error($"CalculateWarrantyUntil: Exception, Error={ex.Message}", ex);
            }
        }

        #endregion

        #region ========== LOAI BAO HANH COMBOBOX ==========

        /// <summary>
        /// Load LoaiBaoHanhComboBoxEdit với các giá trị enum
        /// </summary>
        private void LoadLoaiBaoHanhComboBox()
        {
            try
            {
                // Xóa các items cũ
                LoaiBaoHanhComboBoxEdit.Properties.Items.Clear();

                // Thêm các tùy chọn sử dụng ApplicationEnumUtils
                // Màu sắc được thiết lập trong CustomDisplayText event
                foreach (LoaiBaoHanhEnum value in Enum.GetValues(typeof(LoaiBaoHanhEnum)))
                {
                    int index = ApplicationEnumUtils.GetValue(value);

                    // Lấy Description và màu sắc
                    var description = GetWarrantyTypeDescription(value);
                    var colorName = GetWarrantyTypeColor(value);

                    // Tạo HTML với màu sắc
                    string itemName = $"<color='{colorName}'>{description}</color>";

                    LoaiBaoHanhComboBoxEdit.Properties.Items.Insert(index, itemName);
                }

                // Sử dụng CustomDisplayText để hiển thị text tương ứng
                LoaiBaoHanhComboBoxEdit.CustomDisplayText += LoaiBaoHanhComboBoxEdit_CustomDisplayText;
            }
            catch (Exception ex)
            {
                _logger.Error("LoadLoaiBaoHanhComboBox: Exception occurred", ex);
            }
        }

        /// <summary>
        /// Event handler để hiển thị Description với màu sắc HTML trong LoaiBaoHanhComboBoxEdit
        /// </summary>
        private void LoaiBaoHanhComboBoxEdit_CustomDisplayText(object sender, DevExpress.XtraEditors.Controls.CustomDisplayTextEventArgs e)
        {
            try
            {
                if (e.Value == null) return;

                LoaiBaoHanhEnum warrantyTypeValue;

                // Nếu giá trị là string (Description), convert về enum
                if (e.Value is string stringValue)
                {
                    var warrantyTypeEnum = GetWarrantyTypeEnumFromDescription(stringValue);
                    if (!warrantyTypeEnum.HasValue)
                    {
                        e.DisplayText = stringValue;
                        return;
                    }
                    warrantyTypeValue = warrantyTypeEnum.Value;
                }
                else if (e.Value is LoaiBaoHanhEnum enumValue)
                {
                    warrantyTypeValue = enumValue;
                }
                else if (e.Value is int intValue && Enum.IsDefined(typeof(LoaiBaoHanhEnum), intValue))
                {
                    warrantyTypeValue = (LoaiBaoHanhEnum)intValue;
                }
                else
                {
                    return;
                }

                // Lấy Description và màu sắc
                var description = GetWarrantyTypeDescription(warrantyTypeValue);
                var colorName = GetWarrantyTypeColor(warrantyTypeValue);

                // Tạo HTML với màu sắc
                e.DisplayText = $"<color='{colorName}'>{description}</color>";
            }
            catch (Exception ex)
            {
                _logger.Error("LoaiBaoHanhComboBoxEdit_CustomDisplayText: Exception occurred", ex);
                // Nếu có lỗi, hiển thị giá trị mặc định
                e.DisplayText = e.Value?.ToString() ?? string.Empty;
            }
        }

        /// <summary>
        /// Lấy Description từ enum value
        /// </summary>
        /// <param name="warrantyType">Giá trị enum</param>
        /// <returns>Description hoặc tên enum nếu không có Description</returns>
        private string GetWarrantyTypeDescription(LoaiBaoHanhEnum warrantyType)
        {
            try
            {
                return ApplicationEnumUtils.GetDescription(warrantyType);
            }
            catch (Exception ex)
            {
                _logger.Error($"GetWarrantyTypeDescription: Exception occurred for {warrantyType}", ex);
                return warrantyType.ToString();
            }
        }

        /// <summary>
        /// Lấy màu sắc tương ứng với loại bảo hành
        /// Gọi từ WarrantyDto để đồng bộ với các màn hình khác
        /// </summary>
        /// <param name="warrantyType">Loại bảo hành</param>
        /// <returns>Tên màu (color name)</returns>
        private string GetWarrantyTypeColor(LoaiBaoHanhEnum warrantyType)
        {
            return WarrantyDto.GetWarrantyTypeColor(warrantyType);
        }

        /// <summary>
        /// Lấy enum value từ Description string (có thể chứa HTML tags)
        /// </summary>
        /// <param name="description">Description string (có thể chứa HTML tags)</param>
        /// <returns>Enum value hoặc null nếu không tìm thấy</returns>
        private LoaiBaoHanhEnum? GetWarrantyTypeEnumFromDescription(string description)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(description))
                    return null;

                // Strip HTML tags nếu có
                var cleanDescription = StripHtmlTags(description);

                // Duyệt qua tất cả các giá trị enum để tìm Description khớp
                foreach (LoaiBaoHanhEnum enumValue in Enum.GetValues(typeof(LoaiBaoHanhEnum)))
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
                _logger.Error($"GetWarrantyTypeEnumFromDescription: Exception occurred for '{description}'", ex);
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

        #endregion
    }
}
