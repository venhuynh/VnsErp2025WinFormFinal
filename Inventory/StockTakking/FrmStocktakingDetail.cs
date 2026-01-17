using Bll.Inventory.StockTakking;
using Common.Common;
using Common.Helpers;
using Common.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DTO.Inventory.StockTakking;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Inventory.StockTakking
{
    public partial class FrmStocktakingDetail : XtraForm
    {
        #region ========== FIELDS & PROPERTIES ==========

        /// <summary>
        /// Business Logic Layer cho StocktakingMaster
        /// </summary>
        private readonly StocktakingMasterBll _stocktakingMasterBll = new StocktakingMasterBll();

        /// <summary>
        /// Logger để ghi log các sự kiện
        /// </summary>
        private readonly ILogger _logger = LoggerFactory.CreateLogger(LogCategory.UI);

        #endregion

        #region ========== CONSTRUCTOR ==========

        public FrmStocktakingDetail()
        {
            InitializeComponent();
            InitializeForm();
        }

        #endregion

        #region ========== INITIALIZATION ==========

        /// <summary>
        /// Khởi tạo form
        /// </summary>
        private void InitializeForm()
        {
            try
            {
                // Setup events
                SetupEvents();
            }
            catch (Exception ex)
            {
                _logger.Error("InitializeForm: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi khởi tạo form: {ex.Message}");
            }
        }

        /// <summary>
        /// Thiết lập các event handlers
        /// </summary>
        private void SetupEvents()
        {
            try
            {
                // Event khi popup SearchLookUpEdit mở
                if (StockTakingMasterSearchLookUpEdit != null)
                {
                    StockTakingMasterSearchLookUpEdit.QueryPopUp += StockTakingMasterSearchLookUpEdit_QueryPopUp;
                }

                if (AddNewBarButtonItem != null)
                {
                    AddNewBarButtonItem.ItemClick += AddNewBarButtonItem_ItemClick;
                }

                if (EditBarButtonItem != null)
                {
                    EditBarButtonItem.ItemClick += EditBarButtonItem_ItemClick;
                }
            }
            catch (Exception ex)
            {
                _logger.Error("SetupEvents: Exception occurred", ex);
            }
        }

        #endregion

        #region ========== EVENT HANDLERS ==========

        /// <summary>
        /// Event handler khi popup SearchLookUpEdit mở - Load data cho stocktakingMasterDtoBindingSource
        /// </summary>
        private void StockTakingMasterSearchLookUpEdit_QueryPopUp(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                _logger.Debug("StockTakingMasterSearchLookUpEdit_QueryPopUp: Bắt đầu load data cho SearchLookUpEdit");

                // Load dữ liệu từ BLL
                var allData = _stocktakingMasterBll.GetAll() ?? new List<StocktakingMasterDto>();

                // Bind dữ liệu vào binding source
                stocktakingMasterDtoBindingSource.DataSource = allData;
                stocktakingMasterDtoBindingSource.ResetBindings(false);

                _logger.Info("StockTakingMasterSearchLookUpEdit_QueryPopUp: Đã load {0} phiếu kiểm kho", allData.Count);
            }
            catch (Exception ex)
            {
                _logger.Error("StockTakingMasterSearchLookUpEdit_QueryPopUp: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi tải danh sách phiếu kiểm kho: {ex.Message}");
            }
        }

        private void AddNewBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                using (var form = new FrmStocktakingDetailAddEdit())
                {
                    form.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("AddNewBarButtonItem_ItemClick: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi mở form thêm mới: {ex.Message}");
            }
        }

        private void EditBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                var selectedDto = StocktakingDetailDtoGandedGridView.GetFocusedRow() as StocktakingDetailDto;
                if (selectedDto == null || selectedDto.ProductVariantId == Guid.Empty)
                {
                    MsgBox.ShowWarning("Vui lòng chọn một dòng hợp lệ để chỉnh sửa.");
                    return;
                }

                using (var form = new FrmStocktakingDetailAddEdit(selectedDto.ProductVariantId))
                {
                    form.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("EditBarButtonItem_ItemClick: Exception occurred", ex);
                MsgBox.ShowError($"Lỗi mở form chỉnh sửa: {ex.Message}");
            }
        }

        #endregion
    }
}