using Bll.Inventory.StockIn;
using Common.Utils;
using DTO.Inventory.StockIn;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;
using System;
using System.Collections.Generic;
using DTO.Inventory.StockIn.NhapHangThuongMai;

namespace Inventory.StockIn.InPhieu
{
    public partial class InPhieuNhapKho : DevExpress.XtraReports.UI.XtraReport
    {
        #region Private Fields

        private readonly StockInBll _stockInBll;
        private readonly ILogger _logger;

        #endregion

        #region Constructor

        public InPhieuNhapKho()
        {
            InitializeComponent();
            _stockInBll = new StockInBll();
            _logger = LoggerFactory.CreateLogger(LogCategory.UI);
        }

        /// <summary>
        /// Constructor với VoucherId để tự động load dữ liệu
        /// </summary>
        /// <param name="voucherId">ID phiếu nhập kho</param>
        public InPhieuNhapKho(Guid voucherId) : this()
        {
            LoadData(voucherId);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Load dữ liệu và bind vào report
        /// </summary>
        /// <param name="voucherId">ID phiếu nhập kho</param>
        public void LoadData(Guid voucherId)
        {
            try
            {
                _logger.Debug("LoadData: Bắt đầu load dữ liệu cho report, VoucherId={0}", voucherId);

                // Set parameter
                VoucherId.Value = voucherId;

                // Lấy dữ liệu từ BLL
                var reportData = _stockInBll.GetReportData(voucherId);

                if (reportData == null)
                {
                    _logger.Warning("LoadData: Không tìm thấy dữ liệu cho VoucherId={0}", voucherId);
                    return;
                }

                // Bind data source thông qua bindingSource1
                // Wrap reportData trong List để đáp ứng yêu cầu của DevExpress
                bindingSource1.DataSource = new List<StockInReportDto> { reportData };
                
                // Bind detail report data source - DetailReport sẽ tự động lấy từ ChiTietNhapHangNoiBos
                // Nếu cần set trực tiếp, có thể dùng: DetailReport.DataSource = reportData.ChiTietNhapHangNoiBos

                _logger.Info("LoadData: Load dữ liệu thành công, VoucherId={0}, DetailCount={1}", 
                    voucherId, reportData.ChiTietNhapHangNoiBos?.Count ?? 0);
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
