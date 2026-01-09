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

namespace Inventory.StockTakking
{
    public partial class FrmFrmStocktakingMasterAddEdit : DevExpress.XtraEditors.XtraForm
    {
        #region ========== FIELDS & PROPERTIES ==========

        /// <summary>
        /// ID của StocktakingMaster (dùng khi edit)
        /// Guid.Empty nếu là thêm mới
        /// </summary>
        private Guid _stocktakingMasterId = Guid.Empty;

        #endregion

        #region ========== CONSTRUCTOR ==========

        /// <summary>
        /// Constructor với tham số ID
        /// </summary>
        /// <param name="stocktakingMasterId">ID của StocktakingMaster. Guid.Empty nếu là thêm mới</param>
        public FrmFrmStocktakingMasterAddEdit(Guid stocktakingMasterId)
        {
            _stocktakingMasterId = stocktakingMasterId;
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
                // TODO: Thêm logic khởi tạo form
                // - Load dữ liệu nếu _stocktakingMasterId != Guid.Empty (chế độ edit)
                // - Setup events, controls, etc.
            }
            catch (Exception ex)
            {
                // TODO: Thêm logger và xử lý lỗi
                MessageBox.Show($"Lỗi khởi tạo form: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion
    }
}