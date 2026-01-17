using Bll.Inventory.StockTakking;
using Common.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DTO.Inventory.StockTakking;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;
using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventory.StockTakking
{
    public partial class FrmStocktakingDetailAddEdit : XtraForm
    {
        #region ========== FIELDS & PROPERTIES ==========

        /// <summary>
        /// ID của StocktakingDetail (dùng khi edit)
        /// Guid.Empty nếu là thêm mới
        /// </summary>
        private readonly Guid _stocktakingDetailId;

        /// <summary>
        /// ID phiếu kiểm kho (bắt buộc)
        /// </summary>
        private readonly Guid _stocktakingMasterId;

        /// <summary>
        /// Business Logic Layer cho StocktakingDetail
        /// </summary>
        private readonly StocktakingDetailBll _stocktakingDetailBll = new StocktakingDetailBll();

        /// <summary>
        /// Business Logic Layer cho ProductVariant
        /// </summary>
        private readonly Bll.MasterData.ProductServiceBll.ProductVariantBll _productVariantBll =
            new Bll.MasterData.ProductServiceBll.ProductVariantBll();

        /// <summary>
        /// Logger để ghi log
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// ProductVariantId cần preselect khi mở form
        /// </summary>
        private readonly Guid _initialProductVariantId;

        #endregion

        #region ========== CONSTRUCTOR ==========

        public FrmStocktakingDetailAddEdit() : this(Guid.Empty, Guid.Empty, Guid.Empty)
        {
        }

        public FrmStocktakingDetailAddEdit(Guid productVariantId) : this(Guid.Empty, Guid.Empty, productVariantId)
        {
        }

        /// <summary>
        /// Constructor với tham số ID phiếu kiểm kho và chi tiết
        /// </summary>
        /// <param name="stocktakingMasterId">ID phiếu kiểm kho</param>
        /// <param name="stocktakingDetailId">ID chi tiết kiểm kho (Guid.Empty nếu thêm mới)</param>
        public FrmStocktakingDetailAddEdit(Guid stocktakingMasterId, Guid stocktakingDetailId)
            : this(stocktakingMasterId, stocktakingDetailId, Guid.Empty)
        {
        }

        private FrmStocktakingDetailAddEdit(Guid stocktakingMasterId, Guid stocktakingDetailId, Guid initialProductVariantId)
        {
            _stocktakingMasterId = stocktakingMasterId;
            _stocktakingDetailId = stocktakingDetailId;
            _initialProductVariantId = initialProductVariantId;
            _logger = LoggerFactory.CreateLogger(LogCategory.UI);
            InitializeComponent();
            InitializeForm();
        }

        #endregion

        #region ========== INITIALIZATION & SETUP ==========

        private void InitializeForm()
        {
            try
            {
                SetFormTitle();

                if (_stocktakingDetailId == Guid.Empty)
                {
                    InitializeForAddMode();
                    _ = LoadProductVariantDataSourceAsync();
                }
                else
                {
                    InitializeForEditMode();
                }

                ProductVariantNameSearchLookupEdit.Popup += ProductVariantNameSearchLookupEdit_Popup;
                SaveBarButtonItem.ItemClick += SaveBarButtonItem_ItemClick;
                CloseBarButtonItem.ItemClick += CloseBarButtonItem_ItemClick;
            }
            catch (Exception ex)
            {
                _logger.Error($"InitializeForm: Lỗi khởi tạo form: {ex.Message}", ex);
                ShowError(ex, "Lỗi khởi tạo form");
            }
        }

        private void SetFormTitle()
        {
            Text = _stocktakingDetailId == Guid.Empty
                ? @"Thêm mới chi tiết kiểm kho"
                : @"Điều chỉnh chi tiết kiểm kho";
        }

        private void InitializeForAddMode()
        {
            try
            {
                _logger.Debug("InitializeForAddMode: Khởi tạo form cho chế độ thêm mới");
                IsCountedToggleSwitch.EditValue = false;
                IsReviewedCheckEdit.EditValue = false;
                IsApprovedCheckEdit.EditValue = false;
            }
            catch (Exception ex)
            {
                _logger.Error($"InitializeForAddMode: Lỗi khởi tạo chế độ thêm mới: {ex.Message}", ex);
                throw;
            }
        }

        private void InitializeForEditMode()
        {
            try
            {
                _logger.Debug($"InitializeForEditMode: Khởi tạo form cho chế độ điều chỉnh, Id={_stocktakingDetailId}");
                LoadExistingData();
            }
            catch (Exception ex)
            {
                _logger.Error($"InitializeForEditMode: Lỗi khởi tạo chế độ điều chỉnh: {ex.Message}", ex);
                throw;
            }
        }

        #endregion

        #region ========== DATA LOADING ==========

        private async void LoadExistingData()
        {
            try
            {
                await LoadProductVariantDataSourceAsync();

                var dto = _stocktakingDetailBll.GetById(_stocktakingDetailId);
                if (dto == null)
                {
                    throw new InvalidOperationException($"Không tìm thấy chi tiết kiểm kho với ID: {_stocktakingDetailId}");
                }

                ProductVariantNameSearchLookupEdit.EditValue = dto.ProductVariantId;
                SystemQuantityMemoEdit.EditValue = dto.SystemQuantity;
                CountedQuantityMemoEdit.EditValue = dto.CountedQuantity;
                DifferenceQuantityMemoEdit.EditValue = dto.DifferenceQuantity;
                SystemValueMemoEdit.EditValue = dto.SystemValue;
                CountedValueMemoEdit.EditValue = dto.CountedValue;
                DifferenceValueMemoEdit.EditValue = dto.DifferenceValue;
                UnitPriceMemoEdit.EditValue = dto.UnitPrice;
                AdjustmentTypeImageComboBoxEdit.EditValue = dto.AdjustmentType;
                AdjustmentReasonMemoEdit.Text = dto.AdjustmentReason ?? string.Empty;

                IsCountedToggleSwitch.EditValue = dto.IsCounted;
                CountedByMemoEdit.Text = dto.CountedBy?.ToString() ?? string.Empty;
                CountedDateDateEdit.EditValue = dto.CountedDate;

                IsReviewedCheckEdit.EditValue = dto.IsReviewed;
                ReviewedByMemoEdit.Text = dto.ReviewedBy?.ToString() ?? string.Empty;
                ReviewedDateDateEdit.EditValue = dto.ReviewedDate;
                ReviewNotesMemoEdit.Text = dto.ReviewNotes ?? string.Empty;

                IsApprovedCheckEdit.EditValue = dto.IsApproved;
                ApprovedByMemoEdit.Text = dto.ApprovedBy?.ToString() ?? string.Empty;
                ApprovedDateDateEdit.EditValue = dto.ApprovedDate;

                NotesMemoEdit.Text = dto.Notes ?? string.Empty;
            }
            catch (Exception ex)
            {
                _logger.Error($"LoadExistingData: Lỗi load dữ liệu: {ex.Message}", ex);
                ShowError(ex, "Lỗi tải dữ liệu chi tiết kiểm kho");
                throw;
            }
        }

        private async Task LoadProductVariantDataSourceAsync()
        {
            try
            {
                var productVariants = await _productVariantBll.GetAllAsync();
                productVariantSimpleDtoBindingSource.DataSource = productVariants;
                productVariantSimpleDtoBindingSource.ResetBindings(false);

                if (_initialProductVariantId != Guid.Empty)
                {
                    ProductVariantNameSearchLookupEdit.EditValue = _initialProductVariantId;
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"LoadProductVariantDataSourceAsync: Lỗi tải danh sách sản phẩm: {ex.Message}", ex);
                ShowError(ex, "Lỗi tải danh sách sản phẩm");
            }
        }

        private async void ProductVariantNameSearchLookupEdit_Popup(object sender, EventArgs e)
        {
            try
            {
                await LoadProductVariantDataSourceAsync();
            }
            catch (Exception ex)
            {
                _logger.Error($"ProductVariantNameSearchLookupEdit_Popup: Lỗi tải danh sách sản phẩm: {ex.Message}", ex);
                ShowError(ex, "Lỗi tải danh sách sản phẩm");
            }
        }

        #endregion

        #region ========== EVENT HANDLERS ==========

        private async void SaveBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (!ValidateInput())
                {
                    return;
                }

                var dto = GetDto();
                if (dto == null)
                {
                    ShowError("Không thể lấy dữ liệu từ form. Vui lòng kiểm tra lại.");
                    return;
                }

                var savedDto = await Task.Run(() => _stocktakingDetailBll.SaveOrUpdate(dto));
                if (savedDto != null)
                {
                    var parentForm = FindForm();
                    MsgBox.ShowSuccess("Lưu chi tiết kiểm kho thành công!", "Thành công", parentForm);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    ShowError("Lưu chi tiết kiểm kho thất bại. Vui lòng thử lại.");
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"SaveBarButtonItem_ItemClick: Lỗi lưu chi tiết kiểm kho: {ex.Message}", ex);
                ShowError(ex, "Lỗi lưu chi tiết kiểm kho");
            }
        }

        private void CloseBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
            catch (Exception ex)
            {
                _logger.Error($"CloseBarButtonItem_ItemClick: Lỗi đóng form: {ex.Message}", ex);
                ShowError(ex, "Lỗi đóng form");
            }
        }

        #endregion

        #region ========== VALIDATION ==========

        private bool ValidateInput()
        {
            try
            {
                dxErrorProvider1.ClearErrors();

                if (_stocktakingMasterId == Guid.Empty)
                {
                    ShowError("Phiếu kiểm kho không hợp lệ. Vui lòng mở lại form từ phiếu kiểm kho.");
                    return false;
                }

                if (ProductVariantNameSearchLookupEdit.EditValue is not Guid productVariantId || productVariantId == Guid.Empty)
                {
                    dxErrorProvider1.SetError(ProductVariantNameSearchLookupEdit, "Sản phẩm không được để trống");
                    return false;
                }

                if (!TryGetDecimal(SystemQuantityMemoEdit, "Số lượng hệ thống", true, out _))
                {
                    return false;
                }

                var hasCountedQuantity = TryGetDecimal(CountedQuantityMemoEdit, "Số lượng đã kiểm", false, out var countedQuantity);
                if (!hasCountedQuantity)
                {
                    return false;
                }

                if (!TryGetDecimal(DifferenceQuantityMemoEdit, "Số lượng chênh lệch", false, out _))
                {
                    return false;
                }

                if (!countedQuantity.HasValue && string.IsNullOrWhiteSpace(DifferenceQuantityMemoEdit.Text))
                {
                    dxErrorProvider1.SetError(DifferenceQuantityMemoEdit, "Số lượng chênh lệch không được để trống");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.Error($"ValidateInput: Lỗi validate dữ liệu: {ex.Message}", ex);
                ShowError(ex, "Lỗi validate dữ liệu");
                return false;
            }
        }

        private bool TryGetDecimal(MemoEdit editor, string fieldName, bool required, out decimal? value)
        {
            value = null;
            var text = editor?.Text?.Trim();

            if (string.IsNullOrWhiteSpace(text))
            {
                if (required)
                {
                    dxErrorProvider1.SetError(editor, $"{fieldName} không được để trống");
                    return false;
                }
                return true;
            }

            if (!decimal.TryParse(text, NumberStyles.Number, CultureInfo.CurrentCulture, out var parsed))
            {
                dxErrorProvider1.SetError(editor, $"{fieldName} không hợp lệ");
                return false;
            }

            value = parsed;
            return true;
        }

        #endregion

        #region ========== DATA CONVERSION ==========

        private StocktakingDetailDto GetDto()
        {
            try
            {
                if (!ValidateInput())
                {
                    return null;
                }

                var systemQuantity = TryGetDecimal(SystemQuantityMemoEdit, "Số lượng hệ thống", true, out var systemValue)
                    ? systemValue.GetValueOrDefault()
                    : 0m;

                TryGetDecimal(CountedQuantityMemoEdit, "Số lượng đã kiểm", false, out var countedQuantity);
                TryGetDecimal(DifferenceQuantityMemoEdit, "Số lượng chênh lệch", false, out var differenceQuantity);
                TryGetDecimal(UnitPriceMemoEdit, "Đơn giá", false, out var unitPrice);
                TryGetDecimal(SystemValueMemoEdit, "Giá trị hệ thống", false, out var systemValueAmount);
                TryGetDecimal(CountedValueMemoEdit, "Giá trị đã kiểm", false, out var countedValueAmount);
                TryGetDecimal(DifferenceValueMemoEdit, "Giá trị chênh lệch", false, out var differenceValueAmount);

                if (!differenceQuantity.HasValue && countedQuantity.HasValue)
                {
                    differenceQuantity = countedQuantity.Value - systemQuantity;
                }

                var dto = new StocktakingDetailDto
                {
                    Id = _stocktakingDetailId,
                    StocktakingMasterId = _stocktakingMasterId,
                    ProductVariantId = ProductVariantNameSearchLookupEdit.EditValue is Guid pvId ? pvId : Guid.Empty,
                    ProductVariantName = ProductVariantNameSearchLookupEdit.Text?.Trim(),
                    SystemQuantity = systemQuantity,
                    CountedQuantity = countedQuantity,
                    DifferenceQuantity = differenceQuantity ?? 0m,
                    SystemValue = systemValueAmount,
                    CountedValue = countedValueAmount,
                    DifferenceValue = differenceValueAmount,
                    UnitPrice = unitPrice,
                    AdjustmentType = AdjustmentTypeImageComboBoxEdit.EditValue is AdjustmentTypeEnum adj ? adj : null,
                    AdjustmentReason = AdjustmentReasonMemoEdit.Text?.Trim(),
                    IsCounted = IsCountedToggleSwitch.EditValue is bool isCounted && isCounted,
                    CountedBy = TryParseGuid(CountedByMemoEdit.Text),
                    CountedDate = CountedDateDateEdit.EditValue as DateTime?,
                    IsReviewed = IsReviewedCheckEdit.EditValue is bool isReviewed && isReviewed,
                    ReviewedBy = TryParseGuid(ReviewedByMemoEdit.Text),
                    ReviewedDate = ReviewedDateDateEdit.EditValue as DateTime?,
                    ReviewNotes = ReviewNotesMemoEdit.Text?.Trim(),
                    IsApproved = IsApprovedCheckEdit.EditValue is bool isApproved && isApproved,
                    ApprovedBy = TryParseGuid(ApprovedByMemoEdit.Text),
                    ApprovedDate = ApprovedDateDateEdit.EditValue as DateTime?,
                    Notes = NotesMemoEdit.Text?.Trim(),
                    IsActive = true,
                    IsDeleted = false,
                    CreatedDate = _stocktakingDetailId == Guid.Empty ? DateTime.Now : null,
                    UpdatedDate = DateTime.Now
                };

                return dto;
            }
            catch (Exception ex)
            {
                _logger.Error($"GetDto: Lỗi lấy dữ liệu từ form: {ex.Message}", ex);
                ShowError(ex, "Lỗi lấy dữ liệu");
                return null;
            }
        }

        private Guid? TryParseGuid(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            return Guid.TryParse(value.Trim(), out var parsed) ? parsed : null;
        }

        #endregion

        #region ========== HELPER METHODS ==========

        private void ShowError(Exception ex, string message)
        {
            try
            {
                var parentForm = FindForm();
                if (ex != null)
                {
                    MsgBox.ShowException(ex, message, parentForm);
                }
                else
                {
                    MsgBox.ShowError(message, "Lỗi", parentForm);
                }
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi: {message}: {ex?.Message}");
            }
        }

        private void ShowError(string message)
        {
            try
            {
                var parentForm = FindForm();
                MsgBox.ShowError(message, "Lỗi", parentForm);
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi: {message}");
            }
        }

        #endregion
    }
}