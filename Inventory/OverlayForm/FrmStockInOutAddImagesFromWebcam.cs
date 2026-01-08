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

namespace Inventory.OverlayForm
{
    public partial class FrmStockInOutAddImagesFromWebcam : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// ID phiếu nhập/xuất kho
        /// </summary>
        private Guid StockInOutMasterId { get; set; }

        /// <summary>
        /// Constructor với StockInOutMasterId
        /// </summary>
        /// <param name="stockInOutMasterId">ID phiếu nhập/xuất kho</param>
        public FrmStockInOutAddImagesFromWebcam(Guid stockInOutMasterId)
        {
            InitializeComponent();
            StockInOutMasterId = stockInOutMasterId;
        }
    }
}