using Bll.Inventory.InventoryManagement;
using Common.Utils;
using DevExpress.XtraReports.UI;
using DTO.Inventory.Report;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;
using System;
using System.Collections.Generic;

namespace Inventory.Report.PhieuXuat
{
    /// <summary>
    /// Report phiếu giao hàng (phiếu nhập xuất kho)
    /// Sử dụng StockInOutReportDto để hiển thị thông tin đầy đủ của phiếu nhập xuất
    /// </summary>
    public partial class RpPhieuGiaoHang : DevExpress.XtraReports.UI.XtraReport
    {
        #region Private Fields

        private readonly StockInOutReportBll _stockInOutReportBll;
        private readonly ILogger _logger;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor mặc định (private để tránh khởi tạo trực tiếp)
        /// </summary>
        private RpPhieuGiaoHang()
        {
            InitializeComponent();
            _stockInOutReportBll = new StockInOutReportBll();
            _logger = LoggerFactory.CreateLogger(LogCategory.UI);
        }

        /// <summary>
        /// Constructor với VoucherId để tự động load dữ liệu
        /// </summary>
        /// <param name="voucherId">ID phiếu nhập xuất kho (StockInOutMaster.Id)</param>
        public RpPhieuGiaoHang(Guid voucherId) : this()
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
                _logger.Debug("LoadData: Bắt đầu load dữ liệu cho report, VoucherId={0}", voucherId);

                // Set parameter
                VoucherId.Value = voucherId;

                // Lấy dữ liệu từ BLL
                var reportData = _stockInOutReportBll.GetReportDtoById(voucherId);

                if (reportData == null)
                {
                    _logger.Warning("LoadData: Không tìm thấy dữ liệu cho VoucherId={0}", voucherId);
                    MsgBox.ShowWarning($"Không tìm thấy dữ liệu cho phiếu nhập xuất với ID: {voucherId}");
                    return;
                }

                // Bind data source thông qua bindingSource1
                // Wrap reportData trong List để đáp ứng yêu cầu của DevExpress
                bindingSource1.DataSource = new List<StockInOutReportDto> { reportData };

                // Bind detail report data source - DetailReport sẽ tự động lấy từ ChiTietNhapXuatKhos
                // Nếu cần set trực tiếp, có thể dùng: DetailReport.DataSource = reportData.ChiTietNhapXuatKhos

                _logger.Info("LoadData: Load dữ liệu thành công, VoucherId={0}, DetailCount={1}",
                    voucherId, reportData.ChiTietNhapXuatKhos?.Count ?? 0);
            }
            catch (Exception ex)
            {
                _logger.Error($"LoadData: Lỗi load dữ liệu cho report: {ex.Message}", ex);
                MsgBox.ShowError($"Lỗi tải dữ liệu cho báo cáo: {ex.Message}");
                throw;
            }
        }

        #endregion
    }
}
