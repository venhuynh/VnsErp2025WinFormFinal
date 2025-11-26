using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bll.Inventory.StockIn;
using Common.Common;
using Common.Utils;
using Dal.DataContext;
using DevExpress.XtraReports.UI;
using Inventory.StockIn.InPhieu;
using Inventory.StockIn.NhapThietBiMuon;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;

namespace Inventory.StockIn.NhapNoiBo;

    public partial class FrmNhapNoiBo : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// Constructor mặc định (tạo phiếu mới)
        /// </summary>
        public FrmNhapNoiBo()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor với ID phiếu nhập kho (mở để xem/sửa)
        /// </summary>
        /// <param name="stockInId">ID phiếu nhập kho</param>
        public FrmNhapNoiBo(Guid stockInId)
        {
            InitializeComponent();
            // TODO: Load data với stockInId
        }
    }