using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bll.Inventory.StockIn;
using Bll.MasterData.ProductServiceBll;
using Common.Common;
using Common.Helpers;
using Common.Utils;
using Dal.DataContext;
using DevExpress.Data;
using DTO.Inventory.StockIn.NhapNoiBo;
using DTO.Inventory.StockIn.NhapThietBiMuon;
using DTO.MasterData.ProductService;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;

namespace Inventory.StockIn.NhapNoiBo;

public partial class UcNhapNoiBoDetail : DevExpress.XtraEditors.XtraUserControl
{
    public UcNhapNoiBoDetail()
    {
        InitializeComponent();
        // Additional initialization logic can be added here if needed
    }
}