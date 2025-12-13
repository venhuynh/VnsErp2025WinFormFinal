using Common.Utils;
using DTO.Inventory.StockIn.NhapHangThuongMai;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;
using System;
using System.Collections.Generic;
using Bll.Inventory.StockInOut;

namespace Inventory.StockIn.InPhieu;

public partial class InPhieuBaoHanh : DevExpress.XtraReports.UI.XtraReport
{
    #region Private Fields

    private readonly StockInOutBll _stockInBll;
    private readonly ILogger _logger;

    #endregion

    #region Constructor

    private InPhieuBaoHanh()
    {
        InitializeComponent();
        _stockInBll = new StockInOutBll();
        _logger = LoggerFactory.CreateLogger(LogCategory.UI);
    }

    /// <summary>
    /// Constructor với VoucherId để tự động load dữ liệu
    /// </summary>
    /// <param name="voucherId">ID phiếu nhập kho</param>
    public InPhieuBaoHanh(Guid voucherId) : this()
    {
        LoadData(voucherId);
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Load dữ liệu và bind vào report
    /// </summary>
    /// <param name="voucherId">ID phiếu nhập kho</param>
    private void LoadData(Guid voucherId)
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