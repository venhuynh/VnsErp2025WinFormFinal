using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Linq;
using System.Threading.Tasks;
using Dal.DataAccess.Interfaces.Inventory.StockIn;
using Dal.DataContext;
using Logger;
using Logger.Configuration;
using CustomLogger = Logger.Interfaces.ILogger;

namespace Dal.DataAccess.Implementations.Inventory.StockIn;

public class StockInRepository : IStockInRepository
{
    #region Private Fields

    /// <summary>
    /// Chuỗi kết nối cơ sở dữ liệu để tạo DataContext mới cho mỗi operation.
    /// </summary>
    private readonly string _connectionString;

    /// <summary>
    /// Instance logger để theo dõi các thao tác và lỗi
    /// </summary>
    private readonly CustomLogger _logger;

    #endregion

    #region Constructor

    /// <summary>
    /// Khởi tạo một instance mới của class StockInRepository.
    /// </summary>
    /// <param name="connectionString">Chuỗi kết nối cơ sở dữ liệu</param>
    /// <exception cref="ArgumentNullException">Ném ra khi connectionString là null</exception>
    public StockInRepository(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _logger = LoggerFactory.CreateLogger(LogCategory.DAL);
        _logger.Info("StockInRepository được khởi tạo với connection string");
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Tạo DataContext mới cho mỗi operation để tránh cache issue
    /// </summary>
    /// <returns>DataContext mới</returns>
    private VnsErp2025DataContext CreateNewContext()
    {
        var context = new VnsErp2025DataContext(_connectionString);

        // Configure eager loading cho navigation properties
        var loadOptions = new DataLoadOptions();
        context.LoadOptions = loadOptions;

        return context;
    }

    #endregion

    #region Save Operations

    /// <summary>
    /// Lưu phiếu nhập kho (master và detail) với transaction
    /// </summary>
    /// <param name="master">Entity phiếu nhập kho (master)</param>
    /// <param name="details">Danh sách entity chi tiết phiếu nhập kho</param>
    /// <returns>ID phiếu nhập kho đã lưu</returns>
    public async Task<Guid> SaveAsync(StockInOutMaster master, List<StockInOutDetail> details)
    {
        if (master == null)
            throw new ArgumentNullException(nameof(master));

        if (details == null || details.Count == 0)
            throw new ArgumentException("Danh sách chi tiết không được để trống", nameof(details));

        return await Task.Run(() =>
        {
            using var context = CreateNewContext();
            try
            {
                _logger.Debug("SaveAsync: Bắt đầu lưu phiếu nhập kho, MasterId={0}, DetailCount={1}", 
                    master.Id, details.Count);

                // Bắt đầu transaction
                context.Connection.Open();
                using (var transaction = context.Connection.BeginTransaction())
                {
                    try
                    {
                        context.Transaction = transaction;

                        // 1. Lưu hoặc cập nhật Master
                        var savedMaster = SaveMaster(context, master);
                        var masterId = savedMaster.Id;

                        _logger.Debug("SaveAsync: Master đã được lưu, MasterId={0}", masterId);

                        // 2. Xóa các detail cũ nếu đang update
                        if (master.Id != Guid.Empty)
                        {
                            var existingDetails = context.StockInOutDetails
                                .Where(d => d.StockInOutMasterId == masterId)
                                .ToList();

                            if (existingDetails.Any())
                            {
                                context.StockInOutDetails.DeleteAllOnSubmit(existingDetails);
                                _logger.Debug("SaveAsync: Đã xóa {0} detail cũ", existingDetails.Count);
                            }
                        }

                        // 3. Lưu các Detail mới
                        SaveDetails(context, masterId, details);

                        _logger.Debug("SaveAsync: Đã lưu {0} detail", details.Count);

                        // 4. Commit transaction
                        transaction.Commit();
                        context.SubmitChanges();

                        _logger.Info("SaveAsync: Lưu phiếu nhập kho thành công, MasterId={0}", masterId);
                        return masterId;
                    }
                    catch (Exception ex)
                    {
                        _logger.Error("SaveAsync: Lỗi trong transaction, đang rollback", ex);
                        try
                        {
                            // Chỉ rollback nếu transaction chưa completed
                            if (transaction.Connection != null && transaction.Connection.State == ConnectionState.Open)
                            {
                                transaction.Rollback();
                            }
                        }
                        catch (Exception rollbackEx)
                        {
                            _logger.Error("SaveAsync: Lỗi khi rollback transaction", rollbackEx);
                            // Không throw lại lỗi rollback, chỉ log
                        }
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"SaveAsync: Lỗi lưu phiếu nhập kho: {ex.Message}", ex);
                throw;
            }
        });
    }

    /// <summary>
    /// Lưu hoặc cập nhật Master entity
    /// </summary>
    private StockInOutMaster SaveMaster(VnsErp2025DataContext context, StockInOutMaster master)
    {
        var currentTime = DateTime.Now;
        StockInOutMaster masterEntity;

        if (master.Id != Guid.Empty)
        {
            // Update existing
            masterEntity = context.StockInOutMasters.FirstOrDefault(m => m.Id == master.Id);
            if (masterEntity == null)
            {
                throw new Exception($"Không tìm thấy phiếu nhập kho với ID: {master.Id}");
            }

            // Cập nhật các trường
            masterEntity.StockInOutDate = master.StockInOutDate;
            masterEntity.VocherNumber = master.VocherNumber;
            masterEntity.StockInOutType = master.StockInOutType;
            masterEntity.TrangThaiPhieuNhap = master.TrangThaiPhieuNhap;
            masterEntity.WarehouseId = master.WarehouseId;
            masterEntity.PurchaseOrderId = master.PurchaseOrderId;
            masterEntity.PartnerSiteId = master.PartnerSiteId;
            masterEntity.Notes = master.Notes;
            masterEntity.TotalQuantity = master.TotalQuantity;
            masterEntity.TotalAmount = master.TotalAmount;
            masterEntity.TotalVat = master.TotalVat;
            masterEntity.TotalAmountIncludedVat = master.TotalAmountIncludedVat;
            masterEntity.UpdatedDate = currentTime;
            // UpdatedBy sẽ được set sau khi có authentication

            _logger.Debug("SaveMaster: Cập nhật master, Id={0}", masterEntity.Id);
        }
        else
        {
            // Create new
            master.Id = Guid.NewGuid();
            master.CreatedDate = currentTime;
            master.UpdatedDate = currentTime;
            // CreatedBy và UpdatedBy sẽ được set sau khi có authentication

            context.StockInOutMasters.InsertOnSubmit(master);
            masterEntity = master;
            _logger.Debug("SaveMaster: Tạo mới master, Id={0}", masterEntity.Id);
        }

        return masterEntity;
    }

    /// <summary>
    /// Lưu các Detail entities
    /// </summary>
    private void SaveDetails(VnsErp2025DataContext context, Guid masterId, List<StockInOutDetail> details)
    {
        var detailEntities = new List<StockInOutDetail>();

        foreach (var detail in details)
        {
            var detailEntity = new StockInOutDetail
            {
                Id = detail.Id == Guid.Empty ? Guid.NewGuid() : detail.Id,
                StockInOutMasterId = masterId,
                ProductVariantId = detail.ProductVariantId,
                StockInQty = detail.StockInQty,
                StockOutQty = detail.StockOutQty,
                UnitPrice = detail.UnitPrice,
                Vat = detail.Vat,
                VatAmount = detail.VatAmount,
                TotalAmount = detail.TotalAmount,
                TotalAmountIncludedVat = detail.TotalAmountIncludedVat
            };

            detailEntities.Add(detailEntity);
        }

        context.StockInOutDetails.InsertAllOnSubmit(detailEntities);
        _logger.Debug("SaveDetails: Đã chuẩn bị {0} detail entities để insert", detailEntities.Count);
    }

    /// <summary>
    /// Lấy số thứ tự tiếp theo cho phiếu nhập kho
    /// </summary>
    /// <param name="stockInDate">Ngày nhập kho</param>
    /// <param name="loaiNhapKho">Loại nhập kho (StockInOutType)</param>
    /// <returns>Số thứ tự tiếp theo (1-999)</returns>
    public int GetNextSequenceNumber(DateTime stockInDate, int loaiNhapKho)
    {
        try
        {
            using var context = CreateNewContext();

            // Tạo pattern để tìm: PNK-MMYY-NNXXX
            var month = stockInDate.Month.ToString("D2"); // MM
            var year = stockInDate.Year.ToString().Substring(2); // YY (2 ký tự cuối)
            var loaiNhapKhoIndex = loaiNhapKho.ToString("D2"); // NN (2 ký tự)
            var pattern = $"PNK-{month}{year}-{loaiNhapKhoIndex}%"; // PNK-MMYY-NN%

            _logger.Debug("GetNextSequenceNumber: Tìm pattern={0}, Date={1}, LoaiNhapKho={2}", 
                pattern, stockInDate, loaiNhapKho);

            // Tìm tất cả các phiếu nhập kho có pattern tương ứng trong cùng tháng/năm
            var matchingVouchers = context.StockInOutMasters
                .Where(m => m.VocherNumber.StartsWith($"PNK-{month}{year}-{loaiNhapKhoIndex}") &&
                           m.StockInOutDate.Year == stockInDate.Year &&
                           m.StockInOutDate.Month == stockInDate.Month &&
                           m.StockInOutType == loaiNhapKho)
                .Select(m => m.VocherNumber)
                .ToList();

            if (!matchingVouchers.Any())
            {
                _logger.Debug("GetNextSequenceNumber: Không tìm thấy phiếu nào với pattern={0}, trả về 1", pattern);
                return 1;
            }

            // Extract số thứ tự từ các mã phiếu (3 ký tự cuối)
            var maxSequence = 0;
            foreach (var voucherNumber in matchingVouchers)
            {
                // Format: PNK-MMYY-NNXXX
                // Lấy 3 ký tự cuối (XXX)
                if (voucherNumber.Length >= 3)
                {
                    var sequencePart = voucherNumber.Substring(voucherNumber.Length - 3);
                    if (int.TryParse(sequencePart, out var sequence))
                    {
                        maxSequence = Math.Max(maxSequence, sequence);
                    }
                }
            }

            // Trả về số tiếp theo, nhưng không vượt quá 999
            var nextSequence = Math.Min(maxSequence + 1, 999);
            _logger.Debug("GetNextSequenceNumber: Pattern={0}, MaxSequence={1}, NextSequence={2}", 
                pattern, maxSequence, nextSequence);
            
            return nextSequence;
        }
        catch (Exception ex)
        {
            _logger.Error($"GetNextSequenceNumber: Lỗi lấy số thứ tự tiếp theo: {ex.Message}", ex);
            // Fallback: trả về 1 nếu có lỗi
            return 1;
        }
    }

    #endregion
}