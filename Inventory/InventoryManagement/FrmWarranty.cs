using Bll.Inventory.InventoryManagement;
using Bll.Inventory.StockIn;
using Common.Common;
using Common.Utils;
using Dal.DataContext;
using DevExpress.XtraEditors;
using DTO.Inventory.InventoryManagement;
using DTO.Inventory.StockIn;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventory.InventoryManagement
{
    /// <summary>
    /// Form nhập bảo hành cho phiếu nhập/xuất kho.
    /// Cung cấp chức năng nhập thông tin bảo hành cho các sản phẩm trong phiếu nhập/xuất kho.
    /// </summary>
    public partial class FrmWarranty : XtraForm
    {
        #region ========== FIELDS & PROPERTIES ==========

        /// <summary>
        /// ID phiếu nhập/xuất kho
        /// </summary>
        public Guid StockInOutMasterId { get; private set; }

        /// <summary>
        /// Business Logic Layer cho bảo hành
        /// </summary>
        private readonly WarrantyBll _warrantyBll = new WarrantyBll();

        /// <summary>
        /// Business Logic Layer cho phiếu nhập kho
        /// </summary>
        private readonly StockInBll _stockInBll = new StockInBll();

        /// <summary>
        /// Logger để ghi log các sự kiện
        /// </summary>
        private readonly ILogger _logger = LoggerFactory.CreateLogger(LogCategory.UI);

        /// <summary>
        /// Trạng thái có thay đổi dữ liệu chưa lưu
        /// </summary>
        private bool _hasUnsavedChanges;

        #endregion

        #region ========== CONSTRUCTOR ==========

        /// <summary>
        /// Constructor với StockInOutMasterId
        /// </summary>
        /// <param name="stockInOutMasterId">ID phiếu nhập/xuất kho</param>
        public FrmWarranty(Guid stockInOutMasterId)
        {
            InitializeComponent();
            StockInOutMasterId = stockInOutMasterId;
            Load += FrmWarranty_Load;
        }

        #endregion

        #region ========== INITIALIZATION ==========

        /// <summary>
        /// Event handler khi form được load
        /// </summary>
        private async void FrmWarranty_Load(object sender, EventArgs e)
        {
            try
            {
                _logger.Debug("FrmWarranty_Load: Form loading, StockInOutMasterId={0}", StockInOutMasterId);

                // Setup events
                SetupEvents();

                // Load datasource với SplashScreen
                await LoadDataSourcesAsync();

                // Load danh sách bảo hành hiện có
                await LoadWarrantiesAsync();

                // Reset form về trạng thái ban đầu
                ResetForm();

                _logger.Info("FrmWarranty_Load: Form loaded successfully");
            }
            catch (Exception ex)
            {
                _logger.Error("FrmWarranty_Load: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi khởi tạo form: {ex.Message}");
            }
        }

        /// <summary>
        /// Load tất cả datasource cho các controls với SplashScreen
        /// </summary>
        private async Task LoadDataSourcesAsync()
        {
            try
            {
                _logger.Debug("LoadDataSourcesAsync: Starting to load datasources");

                // Hiển thị SplashScreen
                SplashScreenHelper.ShowWaitingSplashScreen();

                try
                {
                    // Load danh sách StockInDetail để hiển thị trong SearchLookUpEdit
                    await Task.Run(() =>
                    {
                        var details = _stockInBll.GetDetailsByMasterId(StockInOutMasterId);
                        
                        // Sử dụng converter trong DTO
                        var detailDtos = details.ToDtoList();
                        
                        // Update UI thread
                        BeginInvoke(new Action(() =>
                        {
                            stockInDetailDtoBindingSource.DataSource = detailDtos;
                            stockInDetailDtoBindingSource.ResetBindings(false);
                        }));
                    });

                    _logger.Info("LoadDataSourcesAsync: All datasources loaded successfully");
                }
                finally
                {
                    // Đóng SplashScreen
                    SplashScreenHelper.CloseSplashScreen();
                }
            }
            catch (Exception ex)
            {
                _logger.Error("LoadDataSourcesAsync: Exception occurred", ex);
                SplashScreenHelper.CloseSplashScreen();
                MsgBox.ShowError($"Lỗi tải dữ liệu: {ex.Message}");
            }
        }

        /// <summary>
        /// Load danh sách bảo hành hiện có
        /// </summary>
        private async Task LoadWarrantiesAsync()
        {
            try
            {
                _logger.Debug("LoadWarrantiesAsync: Loading warranties, StockInOutMasterId={0}", StockInOutMasterId);

                await Task.Run(() =>
                {
                    var warranties = _warrantyBll.GetByStockInOutMasterId(StockInOutMasterId);
                    var warrantyDtos = warranties.Select(w => w.ToDto()).ToList();

                    // Update UI thread
                    BeginInvoke(new Action(() =>
                    {
                        warrantyDtoBindingSource.DataSource = warrantyDtos;
                        warrantyDtoBindingSource.ResetBindings(false);
                    }));
                });

                _logger.Info("LoadWarrantiesAsync: Loaded warranties successfully");
            }
            catch (Exception ex)
            {
                _logger.Error("LoadWarrantiesAsync: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi tải danh sách bảo hành: {ex.Message}");
            }
        }


        /// <summary>
        /// Setup các event handlers
        /// </summary>
        private void SetupEvents()
        {
            try
            {
                // Bar button events
                SaveBarButtonItem.ItemClick += SaveBarButtonItem_ItemClick;
                CloseBarButtonItem.ItemClick += CloseBarButtonItem_ItemClick;

                // Hyperlink events
                ThemVaoHyperlinkLabelControl.Click += ThemVaoHyperlinkLabelControl_Click;
                BoRaHyperlinkLabelControl.Click += BoRaHyperlinkLabelControl_Click;

                // Grid events
                WarrantyDtoGridView.RowClick += WarrantyDtoGridView_RowClick;

                // Date and numeric edit events
                MonthOfWarrantyTextEdit.EditValueChanged += MonthOfWarrantyTextEdit_EditValueChanged;
                WarrantyFromDateEdit.EditValueChanged += WarrantyFromDateEdit_EditValueChanged;

                // Text edit events - ENTER key để thêm vào grid
                UniqueProductInfoTextEdit.KeyDown += UniqueProductInfoTextEdit_KeyDown;

                // Setup phím tắt
                SetupKeyboardShortcuts();
                UpdateHotKeyBarStaticItem();

                _logger.Debug("SetupEvents: Events setup completed");
            }
            catch (Exception ex)
            {
                _logger.Error("SetupEvents: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi setup events: {ex.Message}");
            }
        }

        /// <summary>
        /// Thiết lập phím tắt cho các nút chức năng
        /// </summary>
        private void SetupKeyboardShortcuts()
        {
            try
            {
                KeyDown += FrmWarranty_KeyDown;
                KeyPreview = true;

                _logger.Debug("SetupKeyboardShortcuts: Keyboard shortcuts configured");
            }
            catch (Exception ex)
            {
                _logger.Error("SetupKeyboardShortcuts: Exception occurred", ex);
            }
        }

        /// <summary>
        /// Cập nhật nội dung HotKeyBarStaticItem để hiển thị hướng dẫn phím tắt
        /// </summary>
        private void UpdateHotKeyBarStaticItem()
        {
            try
            {
                if (FormHotKeyBarStaticItem == null) return;

                var hotKeyText = @"<color=Gray>Phím tắt:</color> " +
                    @"<b><color=Blue>F2</color></b> Lưu | " +
                    @"<b><color=Blue>ESC</color></b> Đóng";

                FormHotKeyBarStaticItem.Caption = hotKeyText;
                FormHotKeyBarStaticItem.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;

                _logger.Debug("UpdateHotKeyBarStaticItem: Hot key bar updated");
            }
            catch (Exception ex)
            {
                _logger.Error("UpdateHotKeyBarStaticItem: Exception occurred", ex);
            }
        }

        /// <summary>
        /// Reset form về trạng thái ban đầu
        /// </summary>
        private void ResetForm()
        {
            try
            {
                _logger.Debug("ResetForm: Resetting form to initial state");

                // Clear input controls
                StockInOutDetailIdSearchLookUpEdit.EditValue = null;
                WarrantyFromDateEdit.EditValue = DateTime.Now; // Set ngày hiện tại khi khởi tạo
                MonthOfWarrantyTextEdit.EditValue = null;
                WarrantyUntilDateEdit.EditValue = null;
                UniqueProductInfoTextEdit.EditValue = null;

                // Reset state
                MarkAsSaved();

                _logger.Info("ResetForm: Form reset successfully");
            }
            catch (Exception ex)
            {
                _logger.Error("ResetForm: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi reset form: {ex.Message}");
            }
        }

        /// <summary>
        /// Đánh dấu form có thay đổi chưa lưu
        /// </summary>
        private void MarkAsChanged()
        {
            _hasUnsavedChanges = true;
            _logger.Debug("MarkAsChanged: Form marked as having unsaved changes");
        }

        /// <summary>
        /// Đánh dấu form đã lưu (không còn thay đổi)
        /// </summary>
        private void MarkAsSaved()
        {
            _hasUnsavedChanges = false;
            _logger.Debug("MarkAsSaved: Form marked as saved");
        }

        #endregion

        #region ========== EVENT HANDLERS ==========

        /// <summary>
        /// Event handler cho nút Lưu
        /// </summary>
        private async void SaveBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                _logger.Debug("SaveBarButtonItem_ItemClick: Save button clicked");

                // Disable button để tránh double-click
                SaveBarButtonItem.Enabled = false;

                try
                {
                    // Validate và lưu dữ liệu
                    var success = await SaveDataAsync();

                    if (success)
                    {
                        MsgBox.ShowSuccess("Lưu bảo hành thành công!", "Thành công", this);
                        MarkAsSaved();
                        await LoadWarrantiesAsync(); // Reload danh sách
                        ResetForm(); // Reset form
                        _logger.Info("SaveBarButtonItem_ItemClick: Data saved successfully");
                    }
                }
                finally
                {
                    // Re-enable button
                    SaveBarButtonItem.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                _logger.Error("SaveBarButtonItem_ItemClick: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi lưu bảo hành: {ex.Message}");
            }
        }

        /// <summary>
        /// Event handler cho nút Đóng
        /// </summary>
        private void CloseBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                _logger.Debug("CloseBarButtonItem_ItemClick: Close button clicked");

                // Kiểm tra có thay đổi chưa lưu không
                if (_hasUnsavedChanges)
                {
                    var result = MsgBox.ShowYesNoCancel(
                        "Bạn có thay đổi chưa lưu. Bạn muốn làm gì?",
                        "Xác nhận đóng",
                        this,
                        yesButtonText: "Lưu và đóng",
                        noButtonText: "Đóng không lưu",
                        cancelButtonText: "Hủy");

                    if (result == DialogResult.Yes)
                    {
                        // Lưu và đóng
                        SaveBarButtonItem_ItemClick(null, null);
                        Close();
                    }
                    else if (result == DialogResult.No)
                    {
                        // Đóng không lưu
                        Close();
                    }
                    // Cancel: giữ form mở
                }
                else
                {
                    Close();
                }
            }
            catch (Exception ex)
            {
                _logger.Error("CloseBarButtonItem_ItemClick: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi đóng form: {ex.Message}");
            }
        }

        /// <summary>
        /// Event handler cho nút Thêm vào
        /// </summary>
        private void ThemVaoHyperlinkLabelControl_Click(object sender, EventArgs e)
        {
            try
            {
                _logger.Debug("ThemVaoHyperlinkLabelControl_Click: Add button clicked");

                // Validate input
                if (!ValidateInput())
                {
                    return;
                }

                // Lấy giá trị từ controls
                var stockInOutDetailId = StockInOutDetailIdSearchLookUpEdit.EditValue as Guid?;
                if (!stockInOutDetailId.HasValue || stockInOutDetailId.Value == Guid.Empty)
                {
                    MsgBox.ShowWarning("Vui lòng chọn sản phẩm bảo hành", "Cảnh báo", this);
                    return;
                }

                var warrantyFrom = WarrantyFromDateEdit.EditValue as DateTime?;
                var monthOfWarranty = Convert.ToInt32(MonthOfWarrantyTextEdit.EditValue ?? 0);
                var warrantyUntil = WarrantyUntilDateEdit.EditValue as DateTime?;
                var uniqueProductInfo = UniqueProductInfoTextEdit.EditValue?.ToString() ?? string.Empty;

                // Tạo WarrantyDto mới
                var warrantyDto = new WarrantyDto
                {
                    Id = Guid.NewGuid(),
                    StockInOutDetailId = stockInOutDetailId.Value,
                    WarrantyType = LoaiBaoHanhEnum.NCCToVNS, // Mặc định
                    WarrantyFrom = warrantyFrom,
                    MonthOfWarranty = monthOfWarranty,
                    WarrantyUntil = warrantyUntil,
                    WarrantyStatus = TrangThaiBaoHanhEnum.ChoXuLy, // Mặc định
                    UniqueProductInfo = uniqueProductInfo
                };

                // Tính toán WarrantyUntil nếu có WarrantyFrom và MonthOfWarranty
                if (warrantyFrom.HasValue && monthOfWarranty > 0 && !warrantyUntil.HasValue)
                {
                    warrantyDto.WarrantyUntil = warrantyFrom.Value.AddMonths(monthOfWarranty);
                }

                // Lấy tên sản phẩm từ selected detail
                if (stockInDetailDtoBindingSource.Current is StockInDetailDto selectedDetail)
                {
                    warrantyDto.ProductVariantName = selectedDetail.ProductVariantName;
                }

                // Thêm vào grid
                if (warrantyDtoBindingSource.DataSource is not List<WarrantyDto> currentList)
                {
                    currentList = new List<WarrantyDto>();
                    warrantyDtoBindingSource.DataSource = currentList;
                }

                currentList.Add(warrantyDto);
                warrantyDtoBindingSource.ResetBindings(false);

                // Đánh dấu có thay đổi
                MarkAsChanged();

                // Clear input controls
                UniqueProductInfoTextEdit.Text = string.Empty;

                _logger.Info("ThemVaoHyperlinkLabelControl_Click: Warranty added to grid, Id={0}", warrantyDto.Id);
            }
            catch (Exception ex)
            {
                _logger.Error("ThemVaoHyperlinkLabelControl_Click: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi thêm bảo hành: {ex.Message}");
            }
        }

        /// <summary>
        /// Event handler cho nút Bỏ ra
        /// </summary>
        private void BoRaHyperlinkLabelControl_Click(object sender, EventArgs e)
        {
            try
            {
                _logger.Debug("BoRaHyperlinkLabelControl_Click: Remove button clicked");

                var selectedRow = WarrantyDtoGridView.GetFocusedRow() as WarrantyDto;
                if (selectedRow == null)
                {
                    MsgBox.ShowWarning("Vui lòng chọn dòng cần xóa", "Cảnh báo", this);
                    return;
                }

                var result = MsgBox.ShowYesNo(
                    "Bạn có chắc chắn muốn xóa bảo hành này?",
                    "Xác nhận xóa",
                    this);

                if (!result) return;

                if (warrantyDtoBindingSource.DataSource is not List<WarrantyDto> currentList) return;
                
                currentList.Remove(selectedRow);
                warrantyDtoBindingSource.ResetBindings(false);
                MarkAsChanged();
                _logger.Info("BoRaHyperlinkLabelControl_Click: Warranty removed from grid, Id={0}", selectedRow.Id);
            }
            catch (Exception ex)
            {
                _logger.Error("BoRaHyperlinkLabelControl_Click: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi xóa bảo hành: {ex.Message}");
            }
        }

        /// <summary>
        /// Event handler khi click vào row trong grid
        /// </summary>
        private void WarrantyDtoGridView_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            try
            {
                if (WarrantyDtoGridView.GetRow(e.RowHandle) is not WarrantyDto selectedRow) return;
                
                // Load thông tin vào input controls
                StockInOutDetailIdSearchLookUpEdit.EditValue = selectedRow.StockInOutDetailId;
                WarrantyFromDateEdit.EditValue = selectedRow.WarrantyFrom ?? DateTime.Now; // Nếu null thì set ngày hiện tại
                MonthOfWarrantyTextEdit.EditValue = selectedRow.MonthOfWarranty;
                WarrantyUntilDateEdit.EditValue = selectedRow.WarrantyUntil;
                UniqueProductInfoTextEdit.EditValue = selectedRow.UniqueProductInfo;
            }
            catch (Exception ex)
            {
                _logger.Error("WarrantyDtoGridView_RowClick: Exception occurred", ex);
            }
        }

        /// <summary>
        /// Event handler khi thay đổi số tháng bảo hành - tự động cập nhật ngày kết thúc bảo hành
        /// </summary>
        private void MonthOfWarrantyTextEdit_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                CalculateWarrantyUntil();
            }
            catch (Exception ex)
            {
                _logger.Error("MonthOfWarrantyTextEdit_EditValueChanged: Exception occurred", ex);
            }
        }

        /// <summary>
        /// Event handler khi thay đổi ngày bắt đầu bảo hành - tự động cập nhật ngày kết thúc bảo hành
        /// </summary>
        private void WarrantyFromDateEdit_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                CalculateWarrantyUntil();
            }
            catch (Exception ex)
            {
                _logger.Error("WarrantyFromDateEdit_EditValueChanged: Exception occurred", ex);
            }
        }

        /// <summary>
        /// Tính toán và cập nhật ngày kết thúc bảo hành dựa trên ngày bắt đầu và số tháng bảo hành
        /// </summary>
        private void CalculateWarrantyUntil()
        {
            try
            {
                var warrantyFrom = WarrantyFromDateEdit.EditValue as DateTime?;
                var monthOfWarranty = Convert.ToInt32(MonthOfWarrantyTextEdit.EditValue ?? 0);

                // Chỉ tính toán nếu có đầy đủ thông tin
                if (warrantyFrom.HasValue && monthOfWarranty > 0)
                {
                    var warrantyUntil = warrantyFrom.Value.AddMonths(monthOfWarranty);
                    WarrantyUntilDateEdit.EditValue = warrantyUntil;
                    
                    _logger.Debug("CalculateWarrantyUntil: Calculated WarrantyUntil={0} from WarrantyFrom={1} and MonthOfWarranty={2}", 
                        warrantyUntil, warrantyFrom.Value, monthOfWarranty);
                }
                else if (!warrantyFrom.HasValue || monthOfWarranty <= 0)
                {
                    // Nếu thiếu thông tin, clear WarrantyUntil
                    WarrantyUntilDateEdit.EditValue = null;
                }
            }
            catch (Exception ex)
            {
                _logger.Error("CalculateWarrantyUntil: Exception occurred", ex);
            }
        }

        /// <summary>
        /// Event handler khi nhấn phím trong UniqueProductInfoTextEdit - ENTER để thêm vào grid
        /// </summary>
        private void UniqueProductInfoTextEdit_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                // Khi nhấn ENTER, gọi ThemVaoHyperlinkLabelControl_Click
               if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    e.SuppressKeyPress = true; // Ngăn không cho phát ra tiếng beep
                    
                    _logger.Debug("UniqueProductInfoTextEdit_KeyDown: Enter key pressed, calling ThemVaoHyperlinkLabelControl_Click");
                    ThemVaoHyperlinkLabelControl_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("UniqueProductInfoTextEdit_KeyDown: Exception occurred", ex);
            }
        }

        /// <summary>
        /// Event handler xử lý phím tắt
        /// </summary>
        private void FrmWarranty_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                switch (e.KeyCode)
                {
                    case Keys.F2:
                        // F2: Lưu
                        e.Handled = true;
                        SaveBarButtonItem_ItemClick(null, null);
                        break;

                    case Keys.Escape:
                        // ESC: Đóng form
                        if (!(ActiveControl is BaseEdit { IsEditorActive: true }))
                        {
                            e.Handled = true;
                            CloseBarButtonItem_ItemClick(null, null);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.Error("FrmWarranty_KeyDown: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi xử lý phím tắt: {ex.Message}");
            }
        }

        #endregion

        #region ========== DATA OPERATIONS ==========

        /// <summary>
        /// Validate input trước khi thêm vào grid
        /// </summary>
        private bool ValidateInput()
        {
            try
            {
                // Validate StockInOutDetailId
                if (StockInOutDetailIdSearchLookUpEdit.EditValue is not Guid stockInOutDetailId || stockInOutDetailId == Guid.Empty)
                {
                    dxErrorProvider1.SetError(StockInOutDetailIdSearchLookUpEdit, "Vui lòng chọn sản phẩm bảo hành");
                    return false;
                }
                dxErrorProvider1.SetError(StockInOutDetailIdSearchLookUpEdit, string.Empty);

                // Validate UniqueProductInfo - đảm bảo duy nhất không được trùng lặp
                var uniqueProductInfo = UniqueProductInfoTextEdit.EditValue?.ToString()?.Trim() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(uniqueProductInfo))
                {
                    dxErrorProvider1.SetError(UniqueProductInfoTextEdit, "Vui lòng nhập thông tin sản phẩm duy nhất");
                    return false;
                }

                // Kiểm tra trùng lặp trong grid hiện tại (case-insensitive)
                // Nếu đang edit một dòng (có dòng được chọn trong grid), bỏ qua dòng đó khi kiểm tra trùng lặp
                if (warrantyDtoBindingSource.DataSource is List<WarrantyDto> currentList && currentList.Any())
                {
                    // Lấy dòng đang được chọn trong grid (nếu có)
                    WarrantyDto selectedRow = null;
                    var focusedRowHandle = WarrantyDtoGridView.FocusedRowHandle;
                    if (focusedRowHandle >= 0)
                    {
                        selectedRow = WarrantyDtoGridView.GetRow(focusedRowHandle) as WarrantyDto;
                    }

                    // Kiểm tra trùng lặp, bỏ qua dòng đang được chọn (nếu đang edit)
                    var isDuplicate = currentList.Any(w => 
                        w != selectedRow && // Bỏ qua dòng đang được chọn nếu đang edit
                        !string.IsNullOrWhiteSpace(w.UniqueProductInfo) &&
                        string.Equals(w.UniqueProductInfo.Trim(), uniqueProductInfo, StringComparison.OrdinalIgnoreCase));
                    
                    if (isDuplicate)
                    {
                        dxErrorProvider1.SetError(UniqueProductInfoTextEdit, "Thông tin sản phẩm duy nhất đã tồn tại trong danh sách. Vui lòng nhập giá trị khác.");
                        _logger.Warning("ValidateInput: Duplicate UniqueProductInfo detected, value={0}", uniqueProductInfo);
                        return false;
                    }
                }

                dxErrorProvider1.SetError(UniqueProductInfoTextEdit, string.Empty);

                // Validate MonthOfWarranty
                var monthOfWarranty = Convert.ToInt32(MonthOfWarrantyTextEdit.EditValue ?? 0);
                if (monthOfWarranty <= 0)
                {
                    dxErrorProvider1.SetError(MonthOfWarrantyTextEdit, "Số tháng bảo hành phải lớn hơn 0");
                    return false;
                }
                dxErrorProvider1.SetError(MonthOfWarrantyTextEdit, string.Empty);

                return true;
            }
            catch (Exception ex)
            {
                _logger.Error("ValidateInput: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi validation: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Lưu dữ liệu async
        /// </summary>
        private async Task<bool> SaveDataAsync()
        {
            try
            {
                _logger.Debug("SaveDataAsync: Starting save operation");

                // Lấy danh sách WarrantyDto từ grid
                var warrantyDtos = warrantyDtoBindingSource.Cast<WarrantyDto>().ToList();
                if (warrantyDtos.Count == 0)
                {
                    MsgBox.ShowWarning("Không có bảo hành nào để lưu", "Cảnh báo", this);
                    return false;
                }

                // Validate tất cả các dòng
                var validationErrors = new List<string>();
                for (int i = 0; i < warrantyDtos.Count; i++)
                {
                    var warranty = warrantyDtos[i];
                    if (warranty.StockInOutDetailId == Guid.Empty)
                    {
                        validationErrors.Add($"Dòng {i + 1}: Vui lòng chọn sản phẩm bảo hành");
                    }
                    if (string.IsNullOrWhiteSpace(warranty.UniqueProductInfo))
                    {
                        validationErrors.Add($"Dòng {i + 1}: Vui lòng nhập thông tin sản phẩm duy nhất");
                    }
                    if (warranty.MonthOfWarranty <= 0)
                    {
                        validationErrors.Add($"Dòng {i + 1}: Số tháng bảo hành phải lớn hơn 0");
                    }
                }

                // Kiểm tra trùng lặp UniqueProductInfo (case-insensitive)
                var duplicateGroups = warrantyDtos
                    .Where(w => !string.IsNullOrWhiteSpace(w.UniqueProductInfo))
                    .GroupBy(w => w.UniqueProductInfo.Trim(), StringComparer.OrdinalIgnoreCase)
                    .Where(g => g.Count() > 1)
                    .ToList();

                if (duplicateGroups.Any())
                {
                    foreach (var group in duplicateGroups)
                    {
                        var duplicateIndices = warrantyDtos
                            .Select((w, index) => new { w, index })
                            .Where(x => string.Equals(x.w.UniqueProductInfo?.Trim(), group.Key, StringComparison.OrdinalIgnoreCase))
                            .Select(x => x.index + 1)
                            .ToList();
                        
                        validationErrors.Add($"Thông tin sản phẩm duy nhất '{group.Key}' bị trùng lặp ở các dòng: {string.Join(", ", duplicateIndices)}");
                    }
                    
                    _logger.Warning("SaveDataAsync: Duplicate UniqueProductInfo detected, count={0}", duplicateGroups.Count);
                }

                if (validationErrors.Any())
                {
                    MsgBox.ShowError($"Có lỗi trong dữ liệu:\n\n{string.Join("\n", validationErrors)}", "Lỗi validation", this);
                    return false;
                }

                // Lưu từng bảo hành
                await Task.Run(() =>
                {
                    foreach (var warrantyDto in warrantyDtos)
                    {
                        var warranty = warrantyDto.ToEntity();
                        _warrantyBll.SaveOrUpdate(warranty);
                    }
                });

                _logger.Info("SaveDataAsync: Save operation completed successfully");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error("SaveDataAsync: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi lưu dữ liệu: {ex.Message}");
                return false;
            }
        }

        #endregion
    }
}
