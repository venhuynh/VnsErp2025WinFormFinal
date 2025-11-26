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
using DTO.Inventory.StockIn.NhapNoiBo;
using Inventory.StockIn.InPhieu;
using Inventory.StockIn.NhapThietBiMuon;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;

namespace Inventory.StockIn.NhapLuuChuyenKho;

public partial class FrmNhapLuuChuyenKho : DevExpress.XtraEditors.XtraForm
{
    public FrmNhapLuuChuyenKho()
    {
        InitializeComponent();
    }
}