using Bll.Inventory.InventoryManagement;
using Common.Utils;
using DTO.Inventory.Report;
using System;
using System.Collections.Generic;

namespace Inventory.Report
{
    public partial class RpPhieuGiaoHang02 : DevExpress.XtraReports.UI.XtraReport
    {
        #region Private Fields

        private readonly StockInOutReportBll _stockInOutReportBll;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor mặc định (private để tránh khởi tạo trực tiếp)
        /// </summary>
        private RpPhieuGiaoHang02()
        {
            InitializeComponent();
            _stockInOutReportBll = new StockInOutReportBll();
        }

        /// <summary>
        /// Constructor với VoucherId để tự động load dữ liệu
        /// </summary>
        /// <param name="voucherId">ID phiếu nhập xuất kho (StockInOutMaster.Id)</param>
        public RpPhieuGiaoHang02(Guid voucherId) : this()
        {
            LoadData(voucherId);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Load dữ liệu và bind vào report
        /// </summary>
        /// <param name="voucherId">ID phiếu nhập xuất kho (StockInOutMaster.Id)</param>
        private void LoadData(Guid voucherId)
        {
            try
            {

                // Set parameter
                VoucherId.Value = voucherId;

                // Lấy dữ liệu từ BLL
                var reportData = _stockInOutReportBll.GetReportDtoById(voucherId);

                if (reportData == null)
                {
                    MsgBox.ShowWarning($"Không tìm thấy dữ liệu cho phiếu nhập xuất với ID: {voucherId}");
                    return;
                }

                // Bind data source thông qua bindingSource1
                // Wrap reportData trong List để đáp ứng yêu cầu của DevExpress
                bindingSource1.DataSource = new List<StockInOutReportDto> { reportData };

            }
            catch (Exception ex)
            {
                MsgBox.ShowError($"Lỗi tải dữ liệu cho báo cáo: {ex.Message}");
                throw;
            }
        }

        #endregion

    }
}
