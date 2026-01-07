namespace Inventory.Query;

/// <summary>
/// Form kiểm tra bảo hành
/// Cung cấp chức năng tìm kiếm theo từ khóa và khoảng thời gian
/// </summary>
public partial class FrmWarrantyCheck : DevExpress.XtraEditors.XtraForm
{
    //#region ========== FIELDS & PROPERTIES ==========

    ///// <summary>
    ///// Business Logic Layer cho bảo hành
    ///// </summary>
    //private readonly WarrantyBll _warrantyBll = new();

    ///// <summary>
    ///// Logger để ghi log các sự kiện
    ///// </summary>
    //private readonly ILogger _logger = LoggerFactory.CreateLogger(LogCategory.UI);

    ///// <summary>
    ///// ID bảo hành được chọn
    ///// </summary>
    //private Guid? _selectedWarrantyId;

    //#endregion

    //#region ========== CONSTRUCTOR ==========

    //public FrmWarrantyCheck()
    //{
    //    InitializeComponent();
    //    InitializeForm();
    //}

    //#endregion

    //#region ========== INITIALIZATION ==========

    ///// <summary>
    ///// Khởi tạo form
    ///// </summary>
    //private void InitializeForm()
    //{
    //    try
    //    {
    //        // Setup events
    //        SetupEvents();

    //        // Khởi tạo giá trị mặc định cho date pickers
    //        InitializeDateFilters();
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.Error("InitializeForm: Exception occurred", ex);
    //        MsgBox.ShowError($"Lỗi khởi tạo form: {ex.Message}");
    //    }
    //}

    ///// <summary>
    ///// Khởi tạo giá trị mặc định cho date filters
    ///// </summary>
    //private void InitializeDateFilters()
    //{
    //    try
    //    {
    //        // Từ ngày: đầu tháng hiện tại
    //        var fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
    //        TuNgayBarEditItem.EditValue = fromDate;

    //        // Đến ngày: ngày hiện tại
    //        DenNgayBarEditItem.EditValue = DateTime.Now;
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.Error("InitializeDateFilters: Exception occurred", ex);
    //    }
    //}

    ///// <summary>
    ///// Setup các event handlers
    ///// </summary>
    //private void SetupEvents()
    //{
    //    try
    //    {
    //        // Bar button events
    //        XemBaoCaoBarButtonItem.ItemClick += XemBaoCaoBarButtonItem_ItemClick;
    //        XoaPhieuBarButtonItem.ItemClick += XoaPhieuBarButtonItem_ItemClick;
    //        XuatFileBarButtonItem.ItemClick += XuatFileBarButtonItem_ItemClick;

    //        // GridView events
    //        WarrantyCheckListDtoGridView.DoubleClick += WarrantyCheckListDtoGridView_DoubleClick;
    //        WarrantyCheckListDtoGridView.FocusedRowChanged += WarrantyCheckListDtoGridView_FocusedRowChanged;
    //        WarrantyCheckListDtoGridView.SelectionChanged += WarrantyCheckListDtoGridView_SelectionChanged;
    //        WarrantyCheckListDtoGridView.CustomDrawRowIndicator += WarrantyCheckListDtoGridView_CustomDrawRowIndicator;
    //        WarrantyCheckListDtoGridView.RowCellStyle += WarrantyCheckListDtoGridView_RowCellStyle;
    //        WarrantyCheckListDtoGridView.CustomDrawGroupRow += WarrantyCheckListDtoGridView_CustomDrawGroupRow;
    //        WarrantyCheckListDtoGridView.CellValueChanged += WarrantyCheckListDtoGridView_CellValueChanged;

    //        // Form events
    //        Load += FrmWarrantyCheck_Load;
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.Error("SetupEvents: Exception occurred", ex);
    //    }
    //}

    //#endregion

    //#region ========== EVENT HANDLERS ==========

    ///// <summary>
    ///// Event handler khi form được load
    ///// </summary>
    //private void FrmWarrantyCheck_Load(object sender, EventArgs e)
    //{
            
    //}

    ///// <summary>
    ///// Event handler cho nút Xem báo cáo
    ///// </summary>
    //private async void XemBaoCaoBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    //{
    //    try
    //    {
    //        await LoadDataAsync();
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.Error("XemBaoCaoBarButtonItem_ItemClick: Exception occurred", ex);
    //        MsgBox.ShowError($"Lỗi tải dữ liệu: {ex.Message}");
    //    }
    //}

    ///// <summary>
    ///// Event handler cho nút Xóa bảo hành
    ///// Cho phép xóa nhiều dòng cùng lúc
    ///// </summary>
    //private async void XoaPhieuBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    //{
    //    try
    //    {
    //        // Kiểm tra số lượng bảo hành được chọn
    //        var selectedCount = WarrantyCheckListDtoGridView.SelectedRowsCount;
    //        if (selectedCount == 0)
    //        {
    //            MsgBox.ShowWarning("Vui lòng chọn ít nhất một bảo hành để xóa.");
    //            return;
    //        }

    //        // Lấy tất cả các dòng được chọn
    //        var selectedRowHandles = WarrantyCheckListDtoGridView.GetSelectedRows();
    //        var selectedDtos = selectedRowHandles
    //            .Select(handle => WarrantyCheckListDtoGridView.GetRow(handle) as WarrantyCheckListDto)
    //            .Where(dto => dto != null && dto.Id != Guid.Empty)
    //            .ToList();

    //        if (selectedDtos.Count == 0)
    //        {
    //            MsgBox.ShowWarning("Không có bảo hành hợp lệ để xóa.");
    //            return;
    //        }

    //        // Hiển thị confirmation dialog
    //        var confirmMessage = selectedDtos.Count == 1
    //            ? $"Bạn có chắc muốn xóa bảo hành:\n<b>{selectedDtos[0].DeviceInfo ?? "N/A"}</b>\n" +
    //              $"Sản phẩm: <b>{selectedDtos[0].ProductVariantName ?? "N/A"}</b>?\n\n" +
    //              "Hành động này không thể hoàn tác!"
    //            : $"Bạn có chắc muốn xóa <b>{selectedDtos.Count}</b> bảo hành?\n\n" +
    //              "Hành động này không thể hoàn tác!";
                
    //        if (!MsgBox.ShowYesNo(confirmMessage, "Xác nhận xóa"))
    //        {
    //            return;
    //        }

    //        // Thực hiện xóa với OverlayManager
    //        var deletedCount = 0;
                
    //        using (OverlayManager.ShowScope(this))
    //        {
    //            await Task.Run(() =>
    //            {
    //                try
    //                {
    //                    foreach (var dto in selectedDtos)
    //                    {
    //                        try
    //                        {
    //                            // Xóa bảo hành
    //                            _warrantyBll.Delete(dto.Id);
    //                            _logger.Info("XoaPhieuBarButtonItem_ItemClick: Đã xóa bảo hành, WarrantyId={0}", dto.Id);
                                    
    //                            deletedCount++;
    //                        }
    //                        catch (Exception ex)
    //                        {
    //                            _logger.Error($"XoaPhieuBarButtonItem_ItemClick: Lỗi xóa bảo hành {dto.Id}: {ex.Message}", ex);
    //                            // Tiếp tục xóa các bảo hành khác
    //                        }
    //                    }
    //                }
    //                catch (Exception ex)
    //                {
    //                    _logger.Error("XoaPhieuBarButtonItem_ItemClick: Exception during delete operation", ex);
    //                    BeginInvoke(new Action(() =>
    //                    {
    //                        MsgBox.ShowError($"Lỗi xóa bảo hành: {ex.Message}");
    //                    }));
    //                    throw;
    //                }
    //            });
    //        }

    //        // Reload data sau khi xóa thành công
    //        await LoadDataAsync();

    //        if (deletedCount == selectedDtos.Count)
    //        {
    //            MsgBox.ShowSuccess($"Đã xóa thành công {deletedCount} bảo hành.");
    //        }
    //        else
    //        {
    //            MsgBox.ShowWarning($"Đã xóa {deletedCount}/{selectedDtos.Count} bảo hành. Vui lòng kiểm tra lại.");
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.Error("XoaPhieuBarButtonItem_ItemClick: Exception occurred", ex);
    //        MsgBox.ShowError($"Lỗi xóa bảo hành: {ex.Message}");
    //    }
    //}

    ///// <summary>
    ///// Event handler cho nút Xuất file
    ///// </summary>
    //private void XuatFileBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    //{
    //    try
    //    {
    //        var rowCount = WarrantyCheckListDtoGridView.RowCount;
    //        if (rowCount <= 0)
    //        {
    //            MsgBox.ShowWarning("Không có dữ liệu để xuất.");
    //            return;
    //        }

    //        // Sử dụng helper từ Common để xuất file Excel
    //        var fileName = $"BaoHanh_{DateTime.Now:yyyyMMdd_HHmmss}";
    //        GridViewHelper.ExportGridControl(WarrantyCheckListDtoGridView, fileName);
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.Error("XuatFileBarButtonItem_ItemClick: Exception occurred", ex);
    //        MsgBox.ShowError($"Lỗi xuất file: {ex.Message}");
    //    }
    //}

    ///// <summary>
    ///// Event handler khi double click trên GridView
    ///// </summary>
    //private void WarrantyCheckListDtoGridView_DoubleClick(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        // Có thể mở form chi tiết bảo hành nếu cần
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.Error("WarrantyCheckListDtoGridView_DoubleClick: Exception occurred", ex);
    //        MsgBox.ShowError($"Lỗi: {ex.Message}");
    //    }
    //}

    ///// <summary>
    ///// Event handler khi row được chọn thay đổi
    ///// </summary>
    //private void WarrantyCheckListDtoGridView_FocusedRowChanged(object sender, 
    //    FocusedRowChangedEventArgs e)
    //{
    //    try
    //    {
    //        UpdateSelectedItem();
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.Error("WarrantyCheckListDtoGridView_FocusedRowChanged: Exception occurred", ex);
    //    }
    //}

    ///// <summary>
    ///// Event handler khi selection thay đổi trên GridView
    ///// </summary>
    //private void WarrantyCheckListDtoGridView_SelectionChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        UpdateSelectedItem();
    //        UpdateDataSummary();
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.Error("WarrantyCheckListDtoGridView_SelectionChanged: Exception occurred", ex);
    //    }
    //}

    ///// <summary>
    ///// Xử lý sự kiện vẽ số thứ tự dòng cho GridView
    ///// Sử dụng helper từ Common để hiển thị số thứ tự (1, 2, 3...)
    ///// </summary>
    //private void WarrantyCheckListDtoGridView_CustomDrawRowIndicator(object sender, 
    //    DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
    //{
    //    try
    //    {
    //        GridViewHelper.CustomDrawRowIndicator(WarrantyCheckListDtoGridView, e);
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.Error("WarrantyCheckListDtoGridView_CustomDrawRowIndicator: Exception occurred", ex);
    //    }
    //}

    ///// <summary>
    ///// Xử lý sự kiện tô màu cell theo trạng thái hoặc điều kiện
    ///// Tô màu nền dòng theo trạng thái bảo hành
    ///// </summary>
    //private void WarrantyCheckListDtoGridView_RowCellStyle(object sender, 
    //    DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
    //{
    //    try
    //    {
    //        var gridView = sender as DevExpress.XtraGrid.Views.Grid.GridView;
    //        if (gridView == null) return;
                
    //        // Bỏ qua các dòng không hợp lệ (header, footer, group row)
    //        if (e.RowHandle < 0) return;

    //        // Không tô màu khi dòng đang được chọn để giữ màu chọn mặc định của DevExpress
    //        if (gridView.IsRowSelected(e.RowHandle)) return;

    //        var dto = gridView.GetRow(e.RowHandle) as WarrantyCheckListDto;
    //        if (dto == null) return;

    //        // Xác định màu nền dựa trên IsWarrantyExpired

    //        var backColor =
    //            // Ưu tiên kiểm tra IsWarrantyExpired - nếu hết hạn thì tô màu đỏ/cam
    //            // Hết hạn bảo hành: màu đỏ nhạt để cảnh báo
    //            dto.IsWarrantyExpired ? System.Drawing.Color.Red : System.Drawing.Color.Blue;

    //        e.Appearance.ForeColor = backColor;
    //        e.Appearance.Options.UseBackColor = true;
    //        e.Appearance.Options.UseForeColor = true;
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.Error("WarrantyCheckListDtoGridView_RowCellStyle: Exception occurred", ex);
    //        // Ignore style errors để không ảnh hưởng đến hiển thị
    //    }
    //}

    ///// <summary>
    ///// Xử lý sự kiện vẽ group row để hiển thị HTML
    ///// </summary>
    //private void WarrantyCheckListDtoGridView_CustomDrawGroupRow(object sender, 
    //    RowObjectCustomDrawEventArgs e)
    //{
    //    try
    //    {
    //        var gridView = sender as DevExpress.XtraGrid.Views.Grid.GridView;
    //        if (gridView == null) return;

    //        var groupInfo = e.Info as DevExpress.XtraGrid.Views.Grid.ViewInfo.GridGroupRowInfo;
    //        if (groupInfo == null) return;

    //        // Lấy giá trị group (UniqueProductInfoHtml) - đã là HTML format
    //        var groupValue = groupInfo.GroupValueText;
                
    //        // Nếu giá trị là HTML, format GroupText để hiển thị HTML
    //        if (!string.IsNullOrWhiteSpace(groupValue) && 
    //            (groupValue.Contains("<color") || groupValue.Contains("<size") || groupValue.Contains("<b>")))
    //        {
    //            // Format GroupText với HTML - DevExpress sẽ tự động render HTML nếu AllowHtmlDrawGroups = True
    //            groupInfo.GroupText = $"{groupInfo.Column.Caption}: {groupValue}";
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.Error("WarrantyCheckListDtoGridView_CustomDrawGroupRow: Exception occurred", ex);
    //        // Ignore errors để không ảnh hưởng đến hiển thị
    //    }
    //}

    ///// <summary>
    ///// Xử lý sự kiện khi giá trị cell thay đổi trong GridView
    ///// Cập nhật WarrantyFrom hoặc MonthOfWarranty và tính lại WarrantyUntil
    ///// </summary>
    //private async void WarrantyCheckListDtoGridView_CellValueChanged(object sender, 
    //    DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
    //{
    //    try
    //    {
    //        var fieldName = e.Column?.FieldName;
    //        var rowHandle = e.RowHandle;

    //        if (rowHandle < 0) return; // Bỏ qua new row

    //        // Chỉ xử lý các cột được phép chỉnh sửa
    //        if (fieldName != "WarrantyFrom" && fieldName != "MonthOfWarranty")
    //            return;

    //        // Lấy DTO từ row
    //        if (!(WarrantyCheckListDtoGridView.GetRow(rowHandle) is WarrantyCheckListDto dto))
    //        {
    //            _logger.Warning("CellValueChanged: Row data is null, RowHandle={0}", rowHandle);
    //            return;
    //        }

    //        // Kiểm tra ID hợp lệ
    //        if (dto.Id == Guid.Empty)
    //        {
    //            _logger.Warning("CellValueChanged: WarrantyId is Empty");
    //            return;
    //        }

    //        // Tính lại WarrantyUntil dựa trên WarrantyFrom và MonthOfWarranty
    //        if (dto.WarrantyFrom.HasValue && dto.MonthOfWarranty > 0)
    //        {
    //            dto.WarrantyUntil = dto.WarrantyFrom.Value.AddMonths(dto.MonthOfWarranty);
    //        }
    //        else
    //        {
    //            dto.WarrantyUntil = null;
    //        }

    //        // Cập nhật database
    //        await Task.Run(() =>
    //        {
    //            try
    //            {
    //                // Lấy entity từ database bằng cách query theo Id
    //                var entities = _warrantyBll.Query(null, null, null);
    //                var entity = entities.FirstOrDefault(w => w.Id == dto.Id);

    //                if (entity == null)
    //                {
    //                    _logger.Warning("CellValueChanged: Warranty entity not found, Id={0}", dto.Id);
    //                    return;
    //                }

    //                // Cập nhật các field
    //                entity.WarrantyFrom = dto.WarrantyFrom;
    //                entity.MonthOfWarranty = dto.MonthOfWarranty;
    //                entity.WarrantyUntil = dto.WarrantyUntil;

    //                // Lưu vào database
    //                _warrantyBll.SaveOrUpdate(entity);

    //                _logger.Info("CellValueChanged: Đã cập nhật bảo hành, Id={0}, WarrantyFrom={1}, MonthOfWarranty={2}", 
    //                    dto.Id, dto.WarrantyFrom, dto.MonthOfWarranty);
    //            }
    //            catch (Exception ex)
    //            {
    //                _logger.Error($"CellValueChanged: Lỗi cập nhật database: {ex.Message}", ex);
    //                BeginInvoke(new Action(() =>
    //                {
    //                    MsgBox.ShowError($"Lỗi cập nhật bảo hành: {ex.Message}");
    //                }));
    //                throw;
    //            }
    //        });

    //        // Refresh row để hiển thị thay đổi
    //        WarrantyCheckListDtoGridView.RefreshRow(rowHandle);
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.Error("CellValueChanged: Exception occurred", ex);
    //        MsgBox.ShowError($"Lỗi xử lý thay đổi cell: {ex.Message}");
    //    }
    //}

    //#endregion

    //#region ========== DATA LOADING ==========

    ///// <summary>
    ///// Load dữ liệu bảo hành
    ///// </summary>
    //private async Task LoadDataAsync()
    //{
    //    try
    //    {
    //        // Hiển thị SplashScreen
    //        SplashScreenHelper.ShowWaitingSplashScreen();

    //        try
    //        {
    //            await LoadDataWithoutSplashAsync();
    //        }
    //        finally
    //        {
    //            // Đóng SplashScreen
    //            SplashScreenHelper.CloseSplashScreen();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.Error("LoadDataAsync: Exception occurred", ex);
    //        SplashScreenHelper.CloseSplashScreen();
    //        MsgBox.ShowError($"Lỗi tải dữ liệu: {ex.Message}");
    //    }
    //}

    ///// <summary>
    ///// Load dữ liệu không hiển thị SplashScreen (dùng cho refresh)
    ///// </summary>
    //private async Task LoadDataWithoutSplashAsync()
    //{
    //    await Task.Run(() =>
    //    {
    //        try
    //        {
    //            // Lấy giá trị từ date pickers
    //            var fromDate = TuNgayBarEditItem.EditValue as DateTime? ?? 
    //                           new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
    //            var toDate = DenNgayBarEditItem.EditValue as DateTime? ?? DateTime.Now;

    //            // Validate date range
    //            if (fromDate > toDate)
    //            {
    //                BeginInvoke(new Action(() =>
    //                {
    //                    MsgBox.ShowWarning("Từ ngày không được lớn hơn đến ngày.");
    //                }));
    //                return;
    //            }

    //            // Lấy từ khóa tìm kiếm
    //            var keyword = KeyWordBarEditItem.EditValue as string;
    //            if (string.IsNullOrWhiteSpace(keyword))
    //            {
    //                keyword = null;
    //            }

    //            // Lấy dữ liệu từ BLL
    //            var entities = _warrantyBll.Query(
    //                fromDate.Date, 
    //                toDate.Date.AddDays(1).AddTicks(-1), // Đến cuối ngày
    //                keyword);

    //            // Convert sang DTO
    //            var dtos = entities.Select(e => e.ToWarrantyCheckListDto()).ToList();

    //            // Update UI thread
    //            BeginInvoke(new Action(() =>
    //            {
    //                warrantyCheckListDtoBindingSource.DataSource = dtos;
    //                warrantyCheckListDtoBindingSource.ResetBindings(false);

    //                // Reset selection
    //                _selectedWarrantyId = null;
    //                UpdateButtonStates();
    //                UpdateDataSummary();
    //            }));
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.Error("LoadDataWithoutSplashAsync: Exception occurred", ex);
    //            BeginInvoke(new Action(() =>
    //            {
    //                MsgBox.ShowError($"Lỗi tải dữ liệu: {ex.Message}");
    //            }));
    //        }
    //    });
    //}

    ///// <summary>
    ///// Cập nhật item được chọn từ GridView
    ///// </summary>
    //private void UpdateSelectedItem()
    //{
    //    try
    //    {
    //        var focusedRowHandle = WarrantyCheckListDtoGridView.FocusedRowHandle;
                
    //        if (focusedRowHandle >= 0)
    //        {
    //            if (WarrantyCheckListDtoGridView.GetRow(focusedRowHandle) is WarrantyCheckListDto dto)
    //            {
    //                _selectedWarrantyId = dto.Id;
    //            }
    //            else
    //            {
    //                _selectedWarrantyId = null;
    //            }
    //        }
    //        else
    //        {
    //            _selectedWarrantyId = null;
    //        }

    //        UpdateButtonStates();
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.Error("UpdateSelectedItem: Exception occurred", ex);
    //    }
    //}

    ///// <summary>
    ///// Cập nhật trạng thái các nút
    ///// </summary>
    //private void UpdateButtonStates()
    //{
    //    try
    //    {
    //        var hasSelection = _selectedWarrantyId.HasValue && 
    //                           _selectedWarrantyId.Value != Guid.Empty;

    //        // Lấy số lượng dòng được chọn
    //        var selectedCount = WarrantyCheckListDtoGridView.SelectedRowsCount;

    //        // Nút Xóa: cho phép xóa nhiều dòng (chỉ cần có selection)
    //        XoaPhieuBarButtonItem.Enabled = selectedCount > 0;
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.Error("UpdateButtonStates: Exception occurred", ex);
    //    }
    //}

    ///// <summary>
    ///// Cập nhật thông tin tổng kết dữ liệu với HTML formatting chuyên nghiệp
    ///// Sử dụng các tag HTML chuẩn của DevExpress: &lt;b&gt;, &lt;i&gt;, &lt;color&gt;, &lt;size&gt;
    ///// </summary>
    //private void UpdateDataSummary()
    //{
    //    try
    //    {
    //        var totalRows = WarrantyCheckListDtoGridView.RowCount;
    //        var selectedRows = WarrantyCheckListDtoGridView.SelectedRowsCount;

    //        // Cập nhật tổng số bảo hành với HTML formatting
    //        if (DataSummaryBarStaticItem != null)
    //        {
    //            if (totalRows == 0)
    //            {
    //                // Không có dữ liệu - hiển thị màu xám, italic
    //                DataSummaryBarStaticItem.Caption = @"<color=#757575><i>Chưa có dữ liệu</i></color>";
    //            }
    //            else
    //            {
    //                // Có dữ liệu - format chuyên nghiệp:
    //                // Label "Tổng:" màu xám, size nhỏ
    //                // Số lượng màu xanh đậm, bold
    //                // Text "bảo hành" màu xám
    //                DataSummaryBarStaticItem.Caption = 
    //                    $@"<size=9><color=#757575>Tổng:</color></size> " +
    //                    $@"<b><color=blue>{totalRows:N0}</color></b> " +
    //                    $@"<size=9><color=#757575>bảo hành</color></size>";
    //            }
    //        }

    //        // Cập nhật số dòng đã chọn với HTML formatting
    //        if (SelectedRowBarStaticItem != null)
    //        {
    //            if (selectedRows > 0)
    //            {
    //                // Có chọn dòng - format chuyên nghiệp:
    //                // Label "Đã chọn:" màu xám, size nhỏ
    //                // Số lượng màu xanh đậm, bold
    //                // Text "dòng" màu xám
    //                SelectedRowBarStaticItem.Caption = 
    //                    $@"<size=9><color=#757575>Đã chọn:</color></size> " +
    //                    $@"<b><color=blue>{selectedRows:N0}</color></b> " +
    //                    $@"<size=9><color=#757575>dòng</color></size>";
    //            }
    //            else
    //            {
    //                // Không chọn dòng - hiển thị màu xám, italic
    //                SelectedRowBarStaticItem.Caption = @"<color=#757575><i>Chưa chọn dòng nào</i></color>";
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.Error("UpdateDataSummary: Exception occurred", ex);
    //    }
    //}

    //#endregion
}