using Bll.Inventory.InventoryManagement;
using Bll.Inventory.StockIn;
using Bll.MasterData.CompanyBll;
using Bll.MasterData.CustomerBll;
using Common;
using Common.Utils;
using Dal.DataContext;
using DevExpress.XtraEditors;
using DTO.Inventory.StockIn;
using DTO.Inventory.StockOut.XuatThietBiChoThueMuon;
using DTO.MasterData.Company;
using DTO.MasterData.CustomerPartner;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventory.StockOut.XuatChoThueMuon;

public partial class UcXuatThietBiChoThueMuonMasterDto : DevExpress.XtraEditors.XtraUserControl
{
    public UcXuatThietBiChoThueMuonMasterDto()
    {
        InitializeComponent();
    }
}