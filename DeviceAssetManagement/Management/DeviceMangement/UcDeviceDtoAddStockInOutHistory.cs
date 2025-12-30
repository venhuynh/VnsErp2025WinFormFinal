using Bll.Inventory.InventoryManagement;
using Common.Common;
using Common.Utils;
using DTO.DeviceAssetManagement;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace DeviceAssetManagement.Management.DeviceMangement
{
    public partial class UcDeviceDtoAddStockInOutHistory : DevExpress.XtraEditors.XtraUserControl
    {
        //#region ========== FIELDS & PROPERTIES ==========

        ///// <summary>
        ///// BindingSource cho lịch sử giao dịch thiết bị
        ///// </summary>
        //private BindingSource _lichSuNhapXuatDtoBindingSource;

        ///// <summary>
        ///// Business Logic Layer cho StockInOutDetail
        ///// </summary>
        //private StockInOutDetailBll _stockInOutDetailBll;

        ///// <summary>
        ///// Business Logic Layer cho StockInOutMaster
        ///// </summary>
        //private StockInOutMasterBll _stockInOutMasterBll;

        ///// <summary>
        ///// Business Logic Layer cho DeviceTransactionHistory
        ///// </summary>
        //private DeviceTransactionHistoryBll _deviceTransactionHistoryBll;

        ///// <summary>
        ///// Logger để ghi log các sự kiện
        ///// </summary>
        //private ILogger _logger;

        ///// <summary>
        ///// Danh sách thiết bị đã chọn
        ///// </summary>
        //private List<DeviceDto> _selectedDevices;

        //#endregion

        //#region ========== CONSTRUCTOR ==========

        //public UcDeviceDtoAddStockInOutHistory()
        //{
        //    InitializeComponent();
        //    InitializeControl();
        //}

        //#endregion

        //#region ========== INITIALIZATION ==========

        ///// <summary>
        ///// Khởi tạo control
        ///// </summary>
        //private void InitializeControl()
        //{
        //    try
        //    {
        //        // Khởi tạo logger
        //        _logger = LoggerFactory.CreateLogger(LogCategory.UI);

        //        // Khởi tạo BLL
        //        _stockInOutDetailBll = new StockInOutDetailBll();
        //        _stockInOutMasterBll = new StockInOutMasterBll();
        //        _deviceTransactionHistoryBll = new DeviceTransactionHistoryBll();

        //        // Khởi tạo danh sách thiết bị đã chọn
        //        _selectedDevices = new List<DeviceDto>();

        //        // Tạo BindingSource cho lịch sử giao dịch thiết bị
        //        _lichSuNhapXuatDtoBindingSource = new BindingSource();
        //        _lichSuNhapXuatDtoBindingSource.DataSource = typeof(DeviceTransactionHistoryDto);

        //        // Gán nguồn dữ liệu cho gridControlLichSuNhapXuat
        //        LichSuNhapXuatGridControl.DataSource = _lichSuNhapXuatDtoBindingSource;

        //        // Setup events
        //        InitializeEvents();

        //        // Setup SuperToolTips
        //        SetupSuperToolTips();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger?.Error($"InitializeControl error: {ex.Message}", ex);
        //        System.Diagnostics.Debug.WriteLine($"InitializeControl error: {ex.Message}");
        //    }
        //}

        ///// <summary>
        ///// Khởi tạo các event handlers
        ///// </summary>
        //private void InitializeEvents()
        //{
        //    // Đăng ký event handler cho nút Thêm vào
        //    ThemVaoHyperlinkLabelControl.Click += ThemVaoHyperlinkLabelControl_Click;

        //    // Đăng ký event handler cho nút Xóa
        //    XoaHyperlinkLabelControl.Click += XoaHyperlinkLabelControl_Click;
        //}

        //#endregion

        //#region ========== SUPERTOOLTIP ==========

        ///// <summary>
        ///// Thiết lập SuperToolTip cho tất cả các controls trong UserControl
        ///// </summary>
        //private void SetupSuperToolTips()
        //{
        //    try
        //    {
        //        SetupSearchLookUpEditSuperTips();
        //        SetupHyperlinkLabelControlSuperTips();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger?.Error($"SetupSuperToolTips error: {ex.Message}", ex);
        //    }
        //}

        ///// <summary>
        ///// Thiết lập SuperToolTip cho SearchLookUpEdit controls
        ///// </summary>
        //private void SetupSearchLookUpEditSuperTips()
        //{
        //    // SuperTip cho ProductVariantSearchLookUpEdit (chọn phiếu nhập xuất)
        //    if (ProductVariantSearchLookUpEdit != null)
        //    {
        //        SuperToolTipHelper.SetBaseEditSuperTip(
        //            ProductVariantSearchLookUpEdit,
        //            title: @"<b><color=DarkBlue>📋 Chọn phiếu nhập xuất</color></b>",
        //            content: @"Chọn <b>phiếu nhập xuất</b> để thêm vào lịch sử giao dịch thiết bị.<br/><br/><b>Chức năng:</b><br/>• Hiển thị danh sách phiếu nhập xuất liên quan đến các thiết bị đã chọn<br/>• Tự động load dữ liệu khi mở popup<br/>• Hiển thị thông tin đầy đủ của phiếu nhập xuất dưới dạng HTML<br/><br/><b>Logic load dữ liệu:</b><br/>• Lấy ProductVariantId từ thiết bị đã chọn<br/>• Tìm StockInOutDetail theo ProductVariantId<br/>• Lấy StockInOutMaster từ StockInOutDetail<br/>• Hiển thị danh sách phiếu nhập xuất<br/><br/><b>Hiển thị:</b><br/>• Số phiếu, loại nhập xuất, ngày thực hiện<br/>• Thông tin kho, đối tác, ghi chú<br/>• Format HTML với màu sắc và định dạng đẹp<br/><br/><color=Gray>Lưu ý:</color> Chỉ hiển thị các phiếu nhập xuất có liên quan đến thiết bị đã chọn."
        //        );
        //    }
        //}

        ///// <summary>
        ///// Thiết lập SuperToolTip cho HyperlinkLabelControl controls
        ///// </summary>
        //private void SetupHyperlinkLabelControlSuperTips()
        //{
        //    // SuperTip cho nút Thêm vào
        //    if (ThemVaoHyperlinkLabelControl != null)
        //    {
        //        ThemVaoHyperlinkLabelControl.SuperTip = SuperToolTipHelper.CreateSuperToolTip(
        //            title: @"<b><color=Green>➕ Thêm vào lịch sử</color></b>",
        //            content: @"Thêm <b>phiếu nhập xuất được chọn</b> vào lịch sử giao dịch thiết bị.<br/><br/><b>Chức năng:</b><br/>• Tạo bản ghi DeviceTransactionHistory cho mỗi thiết bị đã chọn<br/>• Xác định loại thao tác (Import/Export) dựa trên loại phiếu nhập xuất<br/>• Lưu thông tin tham chiếu đến phiếu nhập xuất<br/>• Tự động kiểm tra trùng lặp (không thêm nếu đã tồn tại)<br/><br/><b>Logic xử lý:</b><br/>• Nhập kho (1-7) → Import<br/>• Xuất kho (11-17) → Export<br/>• Khác (99) → Other<br/><br/><b>Thông tin lưu:</b><br/>• Loại thao tác (OperationType)<br/>• Ngày thực hiện (từ phiếu nhập xuất)<br/>• ID tham chiếu (StockInOutMasterId)<br/>• Loại tham chiếu (StockInOutDetail)<br/>• Thông tin chi tiết (số phiếu, loại nhập xuất)<br/><br/><b>Validation:</b><br/>• Kiểm tra đã chọn thiết bị<br/>• Kiểm tra đã chọn phiếu nhập xuất<br/>• Bỏ qua thiết bị đã có lịch sử cho phiếu này<br/><br/><color=Gray>Lưu ý:</color> Sau khi thêm thành công, lịch sử sẽ tự động được load lại."
        //        );
        //    }

        //    // SuperTip cho nút Xóa
        //    if (XoaHyperlinkLabelControl != null)
        //    {
        //        XoaHyperlinkLabelControl.SuperTip = SuperToolTipHelper.CreateSuperToolTip(
        //            title: @"<b><color=Red>🗑️ Xóa lịch sử</color></b>",
        //            content: @"Xóa <b>các dòng lịch sử giao dịch được chọn</b> khỏi hệ thống.<br/><br/><b>Chức năng:</b><br/>• Xóa các bản ghi DeviceTransactionHistory được chọn trong GridView<br/>• Hỗ trợ xóa nhiều dòng cùng lúc<br/>• Hiển thị xác nhận trước khi xóa<br/>• Tự động refresh dữ liệu sau khi xóa<br/><br/><b>Quy trình:</b><br/>1. Kiểm tra có dòng nào được chọn không<br/>2. Hiển thị dialog xác nhận với thông tin chi tiết<br/>3. Xóa từng bản ghi qua BLL<br/>4. Hiển thị kết quả (thành công/lỗi)<br/>5. Load lại lịch sử<br/><br/><b>Xác nhận:</b><br/>• Hiển thị thông tin chi tiết nếu xóa 1 dòng<br/>• Hiển thị số lượng nếu xóa nhiều dòng<br/>• Cảnh báo hành động không thể hoàn tác<br/><br/><b>Xử lý lỗi:</b><br/>• Xử lý từng bản ghi độc lập<br/>• Hiển thị danh sách lỗi chi tiết nếu có<br/>• Không dừng quá trình nếu một bản ghi lỗi<br/><br/><color=Red>⚠️ Cảnh báo:</color> Hành động này không thể hoàn tác!"
        //        );
        //    }
        //}

        //#endregion

        //#region ========== DEVICE MANAGEMENT ==========

        ///// <summary>
        ///// Load danh sách phiếu nhập xuất dựa trên các thiết bị đã chọn
        ///// Logic: Lấy ProductVariantId từ thiết bị -> Tìm StockInOutDetail -> Lấy StockInOutMaster -> Load vào SearchLookUpEdit
        ///// </summary>
        ///// <param name="selectedDevices">Danh sách thiết bị đã chọn</param>
        //public void LoadSelectedDevices(List<DeviceDto> selectedDevices)
        //{
        //    try
        //    {
        //        // Lưu danh sách thiết bị đã chọn
        //        _selectedDevices = selectedDevices ?? new List<DeviceDto>();

        //        if (_selectedDevices.Count == 0)
        //        {
        //            stockInOutMasterHistoryDtoBindingSource.DataSource = new List<StockInOutMasterHistoryDto>();
        //            stockInOutMasterHistoryDtoBindingSource.ResetBindings(false);
        //            // Load lịch sử rỗng
        //            LoadDeviceTransactionHistory();
        //            return;
        //        }

        //        _logger?.Debug($"LoadSelectedDevices: Bắt đầu load, SelectedDevices count={_selectedDevices.Count}");

        //        // Bước 1: Lấy danh sách ProductVariantId từ danh sách thiết bị (distinct)
        //        var productVariantIds = _selectedDevices
        //            .Where(d => d != null && d.ProductVariantId != Guid.Empty)
        //            .Select(d => d.ProductVariantId)
        //            .Distinct()
        //            .ToList();

        //        if (productVariantIds.Count == 0)
        //        {
        //            _logger?.Warning("LoadSelectedDevices: Không có ProductVariantId hợp lệ");
        //            stockInOutMasterHistoryDtoBindingSource.DataSource = new List<StockInOutMasterHistoryDto>();
        //            stockInOutMasterHistoryDtoBindingSource.ResetBindings(false);
        //            return;
        //        }

        //        _logger?.Debug($"LoadSelectedDevices: ProductVariantIds count={productVariantIds.Count}");

        //        // Bước 2: Query StockInOutDetail theo danh sách ProductVariantId
        //        var stockInOutDetails = _stockInOutDetailBll.QueryByProductVariantIds(productVariantIds);

        //        if (stockInOutDetails == null || stockInOutDetails.Count == 0)
        //        {
        //            _logger?.Info("LoadSelectedDevices: Không tìm thấy StockInOutDetail nào");
        //            stockInOutMasterHistoryDtoBindingSource.DataSource = new List<StockInOutMasterHistoryDto>();
        //            stockInOutMasterHistoryDtoBindingSource.ResetBindings(false);
        //            return;
        //        }

        //        _logger?.Debug($"LoadSelectedDevices: StockInOutDetails count={stockInOutDetails.Count}");

        //        // Bước 3: Lấy danh sách StockInOutMasterId (distinct)
        //        var stockInOutMasterIds = stockInOutDetails
        //            .Where(d => d.StockInOutMaster != null && d.StockInOutMasterId != Guid.Empty)
        //            .Select(d => d.StockInOutMasterId)
        //            .Distinct()
        //            .ToList();

        //        if (stockInOutMasterIds.Count == 0)
        //        {
        //            _logger?.Warning("LoadSelectedDevices: Không có StockInOutMasterId hợp lệ");
        //            stockInOutMasterHistoryDtoBindingSource.DataSource = new List<StockInOutMasterHistoryDto>();
        //            stockInOutMasterHistoryDtoBindingSource.ResetBindings(false);
        //            return;
        //        }

        //        _logger?.Debug($"LoadSelectedDevices: StockInOutMasterIds count={stockInOutMasterIds.Count}");

        //        // Bước 4: Query StockInOutMaster từ danh sách MasterId thông qua BLL
        //        var stockInOutMasters = _stockInOutMasterBll.GetMastersByIds(stockInOutMasterIds);

        //        if (stockInOutMasters.Count == 0)
        //        {
        //            _logger?.Warning("LoadSelectedDevices: Không tìm thấy StockInOutMaster nào");
        //            stockInOutMasterHistoryDtoBindingSource.DataSource = new List<StockInOutMasterHistoryDto>();
        //            stockInOutMasterHistoryDtoBindingSource.ResetBindings(false);
        //            return;
        //        }

        //        _logger?.Debug($"LoadSelectedDevices: StockInOutMasters count={stockInOutMasters.Count}");

        //        // Bước 5: Convert từ StockInOutMasterForUIDto sang StockInOutMasterHistoryDto
        //        var stockInOutMasterHistoryDtos = stockInOutMasters
        //            .Select(m => ConvertToHistoryDto(m))
        //            .Where(dto => dto != null)
        //            .ToList();

        //        _logger?.Info($"LoadSelectedDevices: Load thành công, DTOs count={stockInOutMasterHistoryDtos.Count}");

        //        // Bước 6: Load vào BindingSource
        //        stockInOutMasterHistoryDtoBindingSource.DataSource = stockInOutMasterHistoryDtos;
        //        stockInOutMasterHistoryDtoBindingSource.ResetBindings(false);

        //        // Refresh SearchLookUpEdit
        //        ProductVariantSearchLookUpEdit.Properties.PopupView.RefreshData();

        //        // Load lịch sử giao dịch của các thiết bị đã chọn
        //        LoadDeviceTransactionHistory();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger?.Error($"LoadSelectedDevices error: {ex.Message}", ex);
        //        System.Diagnostics.Debug.WriteLine($"LoadSelectedDevices error: {ex.Message}");
        //        throw;
        //    }
        //}

        ///// <summary>
        ///// Load lịch sử giao dịch thiết bị vào gridview
        ///// </summary>
        //private void LoadDeviceTransactionHistory()
        //{
        //    try
        //    {
        //        if (_selectedDevices == null || _selectedDevices.Count == 0)
        //        {
        //            _lichSuNhapXuatDtoBindingSource.DataSource = new List<DeviceTransactionHistoryDto>();
        //            _lichSuNhapXuatDtoBindingSource.ResetBindings(false);
        //            return;
        //        }

        //        _logger?.Debug($"LoadDeviceTransactionHistory: Bắt đầu load, SelectedDevices count={_selectedDevices.Count}");

        //        // Lấy danh sách DeviceId
        //        var deviceIds = _selectedDevices
        //            .Where(d => d != null && d.Id != Guid.Empty)
        //            .Select(d => d.Id)
        //            .Distinct()
        //            .ToList();

        //        if (deviceIds.Count == 0)
        //        {
        //            _logger?.Warning("LoadDeviceTransactionHistory: Không có DeviceId hợp lệ");
        //            _lichSuNhapXuatDtoBindingSource.DataSource = new List<DeviceTransactionHistoryDto>();
        //            _lichSuNhapXuatDtoBindingSource.ResetBindings(false);
        //            return;
        //        }

        //        // Query lịch sử giao dịch cho tất cả các thiết bị
        //        var allHistories = new List<DeviceTransactionHistoryDto>();
        //        foreach (var deviceId in deviceIds)
        //        {
        //            var histories = _deviceTransactionHistoryBll.GetByDeviceId(deviceId);
        //            // GetByDeviceId already returns List<DeviceTransactionHistoryDto>, no need to convert
        //            allHistories.AddRange(histories);
        //        }

        //        // Sắp xếp theo ngày mới nhất
        //        allHistories = allHistories
        //            .OrderByDescending(h => h.OperationDate)
        //            .ThenByDescending(h => h.CreatedDate)
        //            .ToList();

        //        _logger?.Info($"LoadDeviceTransactionHistory: Load thành công, Histories count={allHistories.Count}");

        //        // Load vào BindingSource
        //        _lichSuNhapXuatDtoBindingSource.DataSource = allHistories;
        //        _lichSuNhapXuatDtoBindingSource.ResetBindings(false);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger?.Error($"LoadDeviceTransactionHistory error: {ex.Message}", ex);
        //        System.Diagnostics.Debug.WriteLine($"LoadDeviceTransactionHistory error: {ex.Message}");
        //        throw;
        //    }
        //}

        //#endregion

        //#region ========== ADD TRANSACTION HISTORY ==========

        ///// <summary>
        ///// Event handler cho nút Thêm vào
        ///// Thêm phiếu nhập xuất được chọn vào bảng DeviceTransactionHistory
        ///// </summary>
        //private void ThemVaoHyperlinkLabelControl_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        // Kiểm tra có thiết bị nào được chọn không
        //        if (_selectedDevices == null || _selectedDevices.Count == 0)
        //        {
        //            MsgBox.ShowWarning("Vui lòng chọn thiết bị trước khi thêm lịch sử nhập xuất.", "Chưa chọn thiết bị");
        //            return;
        //        }

        //        // Kiểm tra có phiếu nhập xuất nào được chọn không
        //        if (ProductVariantSearchLookUpEdit.EditValue == null || 
        //            !(ProductVariantSearchLookUpEdit.EditValue is Guid stockInOutMasterId) ||
        //            stockInOutMasterId == Guid.Empty)
        //        {
        //            MsgBox.ShowWarning("Vui lòng chọn phiếu nhập xuất để thêm vào lịch sử.", "Chưa chọn phiếu nhập xuất");
        //            return;
        //        }

        //        // Lấy thông tin phiếu nhập xuất được chọn
        //        var selectedMaster = stockInOutMasterHistoryDtoBindingSource.Current as StockInOutMasterHistoryDto;
        //        if (selectedMaster == null)
        //        {
        //            // Thử lấy từ EditValue
        //            var allMasters = stockInOutMasterHistoryDtoBindingSource.DataSource as List<StockInOutMasterHistoryDto>;
        //            selectedMaster = allMasters?.FirstOrDefault(m => m.Id == stockInOutMasterId);
        //        }

        //        if (selectedMaster == null)
        //        {
        //            MsgBox.ShowError("Không thể lấy thông tin phiếu nhập xuất được chọn.", "Lỗi");
        //            return;
        //        }

        //        _logger?.Debug($"ThemVaoHyperlinkLabelControl_Click: Bắt đầu thêm lịch sử, StockInOutMasterId={stockInOutMasterId}, SelectedDevices count={_selectedDevices.Count}");

        //        // Xác định OperationType dựa trên LoaiNhapXuatKho
        //        var operationType = DetermineOperationType(selectedMaster.LoaiNhapXuatKho);

        //        // Hiển thị splash screen
        //        SplashScreenHelper.ShowWaitingSplashScreen();

        //        try
        //        {
        //            // Tạo DeviceTransactionHistory cho mỗi thiết bị
        //            var successCount = 0;
        //            var errorCount = 0;
        //            var errorMessages = new List<string>();

        //            foreach (var device in _selectedDevices)
        //            {
        //                if (device == null || device.Id == Guid.Empty)
        //                    continue;

        //                try
        //                {
        //                    // Kiểm tra xem đã có lịch sử cho phiếu nhập xuất này chưa
        //                    var existingHistories = _deviceTransactionHistoryBll.Query(
        //                        deviceId: device.Id,
        //                        referenceId: stockInOutMasterId,
        //                        referenceType: (int)DeviceReferenceTypeEnum.StockInOutDetail);

        //                    if (existingHistories.Any())
        //                    {
        //                        _logger?.Warning($"ThemVaoHyperlinkLabelControl_Click: Đã tồn tại lịch sử cho DeviceId={device.Id}, StockInOutMasterId={stockInOutMasterId}");
        //                        continue; // Bỏ qua thiết bị đã có lịch sử
        //                    }

        //                    // Tạo DeviceTransactionHistoryDto
        //                    var historyDto = new DeviceTransactionHistoryDto
        //                    {
        //                        Id = Guid.NewGuid(),
        //                        DeviceId = device.Id,
        //                        OperationType = operationType,
        //                        OperationDate = selectedMaster.StockInOutDate,
        //                        ReferenceId = stockInOutMasterId,
        //                        ReferenceType = (int)DeviceReferenceTypeEnum.StockInOutDetail,
        //                        Information = $"Phiếu {selectedMaster.LoaiNhapXuatKhoName}: {selectedMaster.VocherNumber}",
        //                        HtmlInformation = $"<b>Phiếu {selectedMaster.LoaiNhapXuatKhoName}:</b> {selectedMaster.VocherNumber}<br>" +
        //                                         $"<color='#757575'>Ngày: {selectedMaster.StockInOutDate:dd/MM/yyyy}</color>",
        //                        Notes = selectedMaster.Notes,
        //                        CreatedDate = DateTime.Now
        //                    };

        //                    // Set tên loại tham chiếu (OperationTypeName là computed property, tự động lấy từ OperationType)
        //                    historyDto.SetReferenceTypeName();

                            
        //                    _deviceTransactionHistoryBll.SaveOrUpdate(historyDto);

        //                    successCount++;
        //                    _logger?.Debug($"ThemVaoHyperlinkLabelControl_Click: Thêm lịch sử thành công cho DeviceId={device.Id}");
        //                }
        //                catch (Exception ex)
        //                {
        //                    errorCount++;
        //                    var errorMsg = $"Thiết bị {device.Id}: {ex.Message}";
        //                    errorMessages.Add(errorMsg);
        //                    _logger?.Error($"ThemVaoHyperlinkLabelControl_Click: Lỗi thêm lịch sử cho DeviceId={device.Id}, Error={ex.Message}", ex);
        //                }
        //            }

        //            // Đóng splash screen
        //            SplashScreenHelper.CloseSplashScreen();

        //            // Hiển thị kết quả
        //            if (successCount > 0)
        //            {
        //                var message = $"Đã thêm lịch sử nhập xuất thành công cho {successCount} thiết bị.";
        //                if (errorCount > 0)
        //                {
        //                    message += $"\nCó {errorCount} thiết bị gặp lỗi.";
        //                    if (errorMessages.Any())
        //                    {
        //                        message += "\n\nChi tiết lỗi:\n" + string.Join("\n", errorMessages);
        //                    }
        //                    MsgBox.ShowWarning(message, "Kết quả");
        //                }
        //                else
        //                {
        //                    MsgBox.ShowSuccess(message, "Thành công");
        //                }

        //                // Load lại lịch sử
        //                LoadDeviceTransactionHistory();

        //                // Trigger event để form cha refresh dữ liệu
        //                if (successCount > 0)
        //                {
        //                    OnHistorySaved();
        //                }
        //            }
        //            else
        //            {
        //                var message = "Không thể thêm lịch sử nhập xuất cho bất kỳ thiết bị nào.";
        //                if (errorMessages.Any())
        //                {
        //                    message += "\n\nChi tiết lỗi:\n" + string.Join("\n", errorMessages);
        //                }
        //                MsgBox.ShowError(message, "Lỗi");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            SplashScreenHelper.CloseSplashScreen();
        //            _logger?.Error($"ThemVaoHyperlinkLabelControl_Click: Lỗi tổng quát, Error={ex.Message}", ex);
        //            MsgBox.ShowError($"Lỗi khi thêm lịch sử nhập xuất: {ex.Message}", "Lỗi");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger?.Error($"ThemVaoHyperlinkLabelControl_Click: Exception, Error={ex.Message}", ex);
        //        MsgBox.ShowError($"Lỗi: {ex.Message}", "Lỗi");
        //    }
        //}

        ///// <summary>
        ///// Xác định OperationType dựa trên LoaiNhapXuatKhoEnum
        ///// </summary>
        ///// <param name="loaiNhapXuatKho">Loại nhập xuất kho</param>
        ///// <returns>DeviceOperationTypeEnum tương ứng</returns>
        //private DeviceOperationTypeEnum DetermineOperationType(LoaiNhapXuatKhoEnum loaiNhapXuatKho)
        //{
        //    // Nếu là loại nhập (1-7): Import
        //    // Nếu là loại xuất (11-17): Export
        //    // Nếu là Khác (99): Other
        //    return loaiNhapXuatKho switch
        //    {
        //        LoaiNhapXuatKhoEnum.NhapHangThuongMai or
        //        LoaiNhapXuatKhoEnum.NhapThietBiMuonThue or
        //        LoaiNhapXuatKhoEnum.NhapNoiBo or
        //        LoaiNhapXuatKhoEnum.NhapLuuChuyenKho or
        //        LoaiNhapXuatKhoEnum.NhapHangBaoHanh or
        //        LoaiNhapXuatKhoEnum.NhapSanPhamLapRap or
        //        LoaiNhapXuatKhoEnum.NhapLinhKienPhanRa => DeviceOperationTypeEnum.Import,

        //        LoaiNhapXuatKhoEnum.XuatHangThuongMai or
        //        LoaiNhapXuatKhoEnum.XuatThietBiMuonThue or
        //        LoaiNhapXuatKhoEnum.XuatNoiBo or
        //        LoaiNhapXuatKhoEnum.XuatLuuChuyenKho or
        //        LoaiNhapXuatKhoEnum.XuatHangBaoHanh or
        //        LoaiNhapXuatKhoEnum.XuatLinhKienLapRap or
        //        LoaiNhapXuatKhoEnum.XuatThanhPhamPhanRa => DeviceOperationTypeEnum.Export,

        //        _ => DeviceOperationTypeEnum.Other
        //    };
        //}

        //#endregion

        //#region ========== DELETE TRANSACTION HISTORY ==========

        ///// <summary>
        ///// Event handler cho nút Xóa
        ///// Xóa các dòng lịch sử nhập xuất được chọn
        ///// </summary>
        //private void XoaHyperlinkLabelControl_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        // Kiểm tra có dòng nào được chọn không
        //        var selectedRowHandles = LichSuNhapXuatGridView.GetSelectedRows();
        //        if (selectedRowHandles == null || selectedRowHandles.Length == 0)
        //        {
        //            MsgBox.ShowWarning("Vui lòng chọn ít nhất một dòng lịch sử để xóa.", "Chưa chọn dòng");
        //            return;
        //        }

        //        // Lấy danh sách DTO được chọn
        //        var selectedDtos = selectedRowHandles
        //            .Where(handle => handle >= 0)
        //            .Select(handle => LichSuNhapXuatGridView.GetRow(handle) as DeviceTransactionHistoryDto)
        //            .Where(dto => dto != null && dto.Id != Guid.Empty)
        //            .ToList();

        //        if (selectedDtos.Count == 0)
        //        {
        //            MsgBox.ShowWarning("Không có dòng lịch sử hợp lệ để xóa.", "Lỗi");
        //            return;
        //        }

        //        _logger?.Debug($"XoaHyperlinkLabelControl_Click: Bắt đầu xóa, Selected count={selectedDtos.Count}");

        //        // Hiển thị confirmation dialog
        //        var confirmMessage = selectedDtos.Count == 1
        //            ? $"Bạn có chắc muốn xóa lịch sử giao dịch này?\n\n" +
        //              $"<b>Loại thao tác:</b> {selectedDtos[0].OperationTypeName}\n" +
        //              $"<b>Ngày thực hiện:</b> {selectedDtos[0].OperationDate:dd/MM/yyyy HH:mm}\n\n" +
        //              "Hành động này không thể hoàn tác!"
        //            : $"Bạn có chắc muốn xóa <b>{selectedDtos.Count}</b> dòng lịch sử giao dịch?\n\n" +
        //              "Hành động này không thể hoàn tác!";

        //        if (!MsgBox.ShowYesNo(confirmMessage, "Xác nhận xóa"))
        //        {
        //            return;
        //        }

        //        // Hiển thị splash screen
        //        SplashScreenHelper.ShowWaitingSplashScreen();

        //        try
        //        {
        //            var successCount = 0;
        //            var errorCount = 0;
        //            var errorMessages = new List<string>();

        //            // Xóa từng bản ghi
        //            foreach (var dto in selectedDtos)
        //            {
        //                try
        //                {
        //                    _deviceTransactionHistoryBll.Delete(dto.Id);
        //                    successCount++;
        //                    _logger?.Debug($"XoaHyperlinkLabelControl_Click: Xóa thành công, Id={dto.Id}");
        //                }
        //                catch (Exception ex)
        //                {
        //                    errorCount++;
        //                    var errorMsg = $"Lịch sử {dto.OperationTypeName} ({dto.OperationDate:dd/MM/yyyy}): {ex.Message}";
        //                    errorMessages.Add(errorMsg);
        //                    _logger?.Error($"XoaHyperlinkLabelControl_Click: Lỗi xóa Id={dto.Id}, Error={ex.Message}", ex);
        //                }
        //            }

        //            // Đóng splash screen
        //            SplashScreenHelper.CloseSplashScreen();

        //            // Hiển thị kết quả
        //            if (successCount > 0)
        //            {
        //                var message = $"Đã xóa thành công {successCount} dòng lịch sử.";
        //                if (errorCount > 0)
        //                {
        //                    message += $"\nCó {errorCount} dòng gặp lỗi.";
        //                    if (errorMessages.Any())
        //                    {
        //                        message += "\n\nChi tiết lỗi:\n" + string.Join("\n", errorMessages);
        //                    }
        //                    MsgBox.ShowWarning(message, "Kết quả");
        //                }
        //                else
        //                {
        //                    MsgBox.ShowSuccess(message, "Thành công");
        //                }

        //                // Load lại lịch sử
        //                LoadDeviceTransactionHistory();

        //                // Trigger event để form cha refresh dữ liệu
        //                if (successCount > 0)
        //                {
        //                    OnHistorySaved();
        //                }
        //            }
        //            else
        //            {
        //                var message = "Không thể xóa bất kỳ dòng lịch sử nào.";
        //                if (errorMessages.Any())
        //                {
        //                    message += "\n\nChi tiết lỗi:\n" + string.Join("\n", errorMessages);
        //                }
        //                MsgBox.ShowError(message, "Lỗi");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            SplashScreenHelper.CloseSplashScreen();
        //            _logger?.Error($"XoaHyperlinkLabelControl_Click: Lỗi tổng quát, Error={ex.Message}", ex);
        //            MsgBox.ShowError($"Lỗi khi xóa lịch sử: {ex.Message}", "Lỗi");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger?.Error($"XoaHyperlinkLabelControl_Click: Exception, Error={ex.Message}", ex);
        //        MsgBox.ShowError($"Lỗi: {ex.Message}", "Lỗi");
        //    }
        //}

        ///// <summary>
        ///// Event được trigger khi lịch sử nhập xuất được lưu thành công (thêm hoặc xóa)
        ///// </summary>
        //public event EventHandler HistorySaved;

        ///// <summary>
        ///// Trigger event HistorySaved
        ///// </summary>
        //protected virtual void OnHistorySaved()
        //{
        //    HistorySaved?.Invoke(this, EventArgs.Empty);
        //}

        //#endregion
    }
}
