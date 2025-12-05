using Dal.Connection;
using Dal.DataAccess.Implementations.Inventory.InventoryManagement;
using Dal.DataAccess.Implementations.Inventory.StockIn;
using Dal.DataAccess.Interfaces.Inventory.InventoryManagement;
using Dal.DataContext;
using DTO.Inventory.StockIn;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using DTO.Inventory.InventoryManagement;

namespace Bll.Inventory.InventoryManagement
{
    /// <summary>
    /// Business Logic Layer cho StockInOutMaster (Phiếu nhập xuất kho)
    /// </summary>
    public class StockInOutMasterBll
{
    #region Fields

    private IStockInOutMasterRepository _dataAccess;
    private Dal.DataAccess.Interfaces.Inventory.StockIn.IStockInRepository _stockInRepository;
    private readonly ILogger _logger;
    private readonly object _lockObject = new();
    private readonly object _stockInRepositoryLock = new();

    #endregion

    #region Constructors

    /// <summary>
    /// Constructor mặc định
    /// </summary>
    public StockInOutMasterBll()
    {
        _logger = LoggerFactory.CreateLogger(LogCategory.BLL);
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Lấy hoặc khởi tạo Repository (lazy initialization)
    /// </summary>
    private IStockInOutMasterRepository GetDataAccess()
    {
        if (_dataAccess == null)
        {
            lock (_lockObject)
            {
                if (_dataAccess == null)
                {
                    try
                    {
                        // Sử dụng global connection string từ ApplicationStartupManager
                        var globalConnectionString = ApplicationStartupManager.Instance.GetGlobalConnectionString();
                        if (string.IsNullOrEmpty(globalConnectionString))
                        {
                            throw new InvalidOperationException(
                                "Không có global connection string. Ứng dụng chưa được khởi tạo hoặc chưa sẵn sàng.");
                        }

                        _dataAccess = new StockInOutMasterRepository(globalConnectionString);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"Lỗi khởi tạo StockInOutMasterRepository: {ex.Message}", ex);
                        throw;
                    }
                }
            }
        }

        return _dataAccess;
    }

    /// <summary>
    /// Lấy hoặc khởi tạo StockInRepository (lazy initialization)
    /// Sử dụng để query history vì method QueryHistory nằm trong StockInRepository
    /// </summary>
    private Dal.DataAccess.Interfaces.Inventory.StockIn.IStockInRepository GetStockInRepository()
    {
        if (_stockInRepository == null)
        {
            lock (_stockInRepositoryLock)
            {
                if (_stockInRepository == null)
                {
                    try
                    {
                        // Sử dụng global connection string từ ApplicationStartupManager
                        var globalConnectionString = ApplicationStartupManager.Instance.GetGlobalConnectionString();
                        if (string.IsNullOrEmpty(globalConnectionString))
                        {
                            return null;
                            //throw new InvalidOperationException(
                            //    "Không có global connection string. Ứng dụng chưa được khởi tạo hoặc chưa sẵn sàng.");
                        }

                        _stockInRepository = new StockInRepository(globalConnectionString);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"Lỗi khởi tạo StockInRepository: {ex.Message}", ex);
                        throw;
                    }
                }
            }
        }

        return _stockInRepository;
    }

    #endregion

    #region Query Operations

    /// <summary>
    /// Query lịch sử nhập xuất kho với filter
    /// </summary>
    /// <param name="query">Query criteria</param>
    /// <returns>Danh sách StockInOutMaster entities</returns>
    public List<StockInOutMaster> QueryHistory(StockInHistoryQueryCriteria query)
    {
        try
        {
            _logger.Debug("QueryHistory: Bắt đầu query lịch sử, FromDate={0}, ToDate={1}", query.FromDate, query.ToDate);

            var entities = GetStockInRepository().QueryHistory(query);

            _logger.Info("QueryHistory: Query thành công, ResultCount={0}", entities.Count);
            return entities;
        }
        catch (Exception ex)
        {
            _logger.Error($"QueryHistory: Lỗi query lịch sử: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Đếm số lượng bản ghi theo query (không phân trang)
    /// </summary>
    /// <param name="query">Query criteria</param>
    /// <returns>Tổng số bản ghi</returns>
    public int CountHistory(StockInHistoryQueryCriteria query)
    {
        try
        {
            _logger.Debug("CountHistory: Bắt đầu đếm, FromDate={0}, ToDate={1}", query.FromDate, query.ToDate);

            var count = GetStockInRepository().CountHistory(query);

            _logger.Info("CountHistory: Đếm thành công, Count={0}", count);
            return count;
        }
        catch (Exception ex)
        {
            _logger.Error($"CountHistory: Lỗi đếm: {ex.Message}", ex);
            throw;
        }
    }

    #endregion

    #region Voucher Number Generation

    /// <summary>
    /// Tạo số phiếu nhập/xuất kho tự động theo format: PNK-MMYY-NNXXX hoặc PXK-MMYY-NNXXX
    /// Tự động xác định PNK (Phiếu nhập kho) hoặc PXK (Phiếu xuất kho) dựa trên LoaiNhapXuatKhoEnum
    /// </summary>
    /// <param name="stockInOutDate">Ngày nhập/xuất kho</param>
    /// <param name="loaiNhapXuatKho">Loại nhập/xuất kho</param>
    /// <returns>Số phiếu nhập/xuất kho theo format: PNK-MMYY-NNXXX hoặc PXK-MMYY-NNXXX</returns>
    /// <remarks>
    /// Format:
    /// - PNK/PXK: Phiếu nhập kho / Phiếu xuất kho (tự động xác định)
    /// - MM: Tháng (2 ký tự)
    /// - YY: Năm (2 ký tự cuối)
    /// - NN: Index của LoaiNhapXuatKhoEnum (2 ký tự)
    /// - XXX: Số thứ tự phiếu (3 ký tự từ 001 đến 999)
    /// 
    /// Logic xác định PNK hay PXK:
    /// - Nếu LoaiNhapXuatKhoEnum < 10: PNK (Nhập kho)
    /// - Nếu LoaiNhapXuatKhoEnum >= 10: PXK (Xuất kho)
    /// </remarks>
    public string GenerateVoucherNumber(DateTime stockInOutDate, LoaiNhapXuatKhoEnum loaiNhapXuatKho)
    {
        try
        {
            _logger.Debug("GenerateVoucherNumber: Date={0}, LoaiNhapXuatKho={1}", stockInOutDate, loaiNhapXuatKho);

            // Xác định prefix: PNK (Nhập kho) hoặc PXK (Xuất kho)
            // Logic: LoaiNhapXuatKhoEnum < 10 = Nhập kho, >= 10 = Xuất kho
            var isNhapKho = (int)loaiNhapXuatKho < 10;
            var prefix = isNhapKho ? "PNK" : "PXK";

            // Lấy thông tin từ ngày
            var month = stockInOutDate.Month.ToString("D2"); // MM
            var year = stockInOutDate.Year.ToString().Substring(2); // YY (2 ký tự cuối)
            var loaiNhapXuatKhoIndex = ((int)loaiNhapXuatKho).ToString("D2"); // NN (2 ký tự)

            // Lấy số thứ tự tiếp theo từ StockInRepository
            var loaiNhapKhoInt = (int)loaiNhapXuatKho;
            var nextSequence = GetStockInRepository().GetNextSequenceNumber(stockInOutDate, loaiNhapKhoInt);

            // Tạo số phiếu: PNK-MMYY-NNXXX hoặc PXK-MMYY-NNXXX
            var voucherNumber = $"{prefix}-{month}{year}-{loaiNhapXuatKhoIndex}{nextSequence:D3}";

            _logger.Debug("GenerateVoucherNumber: Generated={0}", voucherNumber);
            return voucherNumber;
        }
        catch (Exception ex)
        {
            _logger.Error($"GenerateVoucherNumber: Lỗi tạo số phiếu: {ex.Message}", ex);
            throw;
        }
    }

    #endregion

    #region Delete Operations

    /// <summary>
    /// Xóa phiếu nhập xuất kho theo ID
    /// </summary>
    /// <param name="id">ID phiếu nhập xuất kho cần xóa</param>
    public void Delete(Guid id)
    {
        try
        {
            _logger.Debug("Delete: Bắt đầu xóa phiếu nhập xuất kho, Id={0}", id);

            using var context = new VnsErp2025DataContext(ApplicationStartupManager.Instance.GetGlobalConnectionString());

            // Tìm master entity
            var master = context.StockInOutMasters.FirstOrDefault(m => m.Id == id);
            if (master == null)
            {
                _logger.Warning("Delete: Không tìm thấy phiếu nhập xuất kho với Id={0}", id);
                throw new InvalidOperationException($"Không tìm thấy phiếu nhập xuất kho với ID: {id}");
            }

            // Xóa tất cả details trước
            var details = context.StockInOutDetails.Where(d => d.StockInOutMasterId == id).ToList();
            if (details.Any())
            {
                context.StockInOutDetails.DeleteAllOnSubmit(details);
                _logger.Debug("Delete: Đã xóa {0} detail", details.Count);
            }

            // Xóa master
            context.StockInOutMasters.DeleteOnSubmit(master);
            context.SubmitChanges();

            _logger.Info("Delete: Xóa phiếu nhập xuất kho thành công, Id={0}", id);
        }
        catch (Exception ex)
        {
            _logger.Error($"Delete: Lỗi xóa phiếu nhập xuất kho: {ex.Message}", ex);
            throw;
        }
    }

    #endregion
}
}