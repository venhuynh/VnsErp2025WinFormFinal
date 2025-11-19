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

namespace Inventory.InventoryManagement
{
    /// <summary>
    /// Form nhập bảo hành cho phiếu nhập/xuất kho.
    /// Cung cấp chức năng nhập thông tin bảo hành cho các sản phẩm trong phiếu nhập/xuất kho.
    /// </summary>
    public partial class FrmWarranty : DevExpress.XtraEditors.XtraForm
    {
        #region ========== FIELDS & PROPERTIES ==========

        /// <summary>
        /// ID phiếu nhập/xuất kho
        /// </summary>
        public Guid StockInOutMasterId { get; private set; }

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
        }

        #endregion
    }
}