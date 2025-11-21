using System;
using System.Windows.Forms;
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting;
using Common.Utils;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;

namespace Inventory.StockIn.InPhieu
{
    /// <summary>
    /// Helper class để in phiếu nhập kho
    /// </summary>
    public static class StockInReportHelper
    {
        private static readonly ILogger _logger = LoggerFactory.CreateLogger(LogCategory.UI);

        /// <summary>
        /// In phiếu nhập kho với preview
        /// </summary>
        /// <param name="voucherId">ID phiếu nhập kho</param>
        public static void PrintStockInVoucher(Guid voucherId)
        {
            try
            {
                _logger.Debug("PrintStockInVoucher: Bắt đầu in phiếu nhập kho, VoucherId={0}", voucherId);

                if (voucherId == Guid.Empty)
                {
                    MsgBox.ShowWarning("Vui lòng chọn phiếu nhập kho để in.");
                    return;
                }

                // Tạo và load report
                var report = new InPhieuNhapKho(voucherId);

                // Hiển thị preview bằng ReportPrintTool
                using (var printTool = new ReportPrintTool(report))
                {
                    printTool.ShowPreviewDialog();
                }

                _logger.Info("PrintStockInVoucher: In phiếu nhập kho thành công, VoucherId={0}", voucherId);
            }
            catch (Exception ex)
            {
                _logger.Error($"PrintStockInVoucher: Lỗi in phiếu nhập kho: {ex.Message}", ex);
                MsgBox.ShowError($"Lỗi in phiếu nhập kho: {ex.Message}");
            }
        }

        /// <summary>
        /// In phiếu nhập kho trực tiếp (không preview)
        /// </summary>
        /// <param name="voucherId">ID phiếu nhập kho</param>
        public static void PrintStockInVoucherDirect(Guid voucherId)
        {
            try
            {
                _logger.Debug("PrintStockInVoucherDirect: Bắt đầu in trực tiếp, VoucherId={0}", voucherId);

                if (voucherId == Guid.Empty)
                {
                    MsgBox.ShowWarning("Vui lòng chọn phiếu nhập kho để in.");
                    return;
                }

                // Tạo và load report
                var report = new InPhieuNhapKho(voucherId);

                // In trực tiếp
                report.Print();

                _logger.Info("PrintStockInVoucherDirect: In trực tiếp thành công, VoucherId={0}", voucherId);
            }
            catch (Exception ex)
            {
                _logger.Error($"PrintStockInVoucherDirect: Lỗi in trực tiếp: {ex.Message}", ex);
                MsgBox.ShowError($"Lỗi in phiếu nhập kho: {ex.Message}");
            }
        }

        /// <summary>
        /// Xuất phiếu nhập kho ra file PDF
        /// </summary>
        /// <param name="voucherId">ID phiếu nhập kho</param>
        /// <param name="filePath">Đường dẫn file PDF để lưu</param>
        public static void ExportStockInVoucherToPdf(Guid voucherId, string filePath)
        {
            try
            {
                _logger.Debug("ExportStockInVoucherToPdf: Bắt đầu xuất PDF, VoucherId={0}, FilePath={1}", voucherId, filePath);

                if (voucherId == Guid.Empty)
                {
                    MsgBox.ShowWarning("Vui lòng chọn phiếu nhập kho để xuất.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(filePath))
                {
                    MsgBox.ShowWarning("Vui lòng chọn đường dẫn file để lưu.");
                    return;
                }

                // Tạo và load report
                var report = new InPhieuNhapKho(voucherId);

                // Xuất ra PDF
                report.ExportToPdf(filePath);

                _logger.Info("ExportStockInVoucherToPdf: Xuất PDF thành công, VoucherId={0}, FilePath={1}", voucherId, filePath);
                MsgBox.ShowSuccess($"Xuất phiếu nhập kho ra PDF thành công:\n{filePath}");
            }
            catch (Exception ex)
            {
                _logger.Error($"ExportStockInVoucherToPdf: Lỗi xuất PDF: {ex.Message}", ex);
                MsgBox.ShowError($"Lỗi xuất phiếu nhập kho ra PDF: {ex.Message}");
            }
        }
    }
}

