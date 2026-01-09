using Dal.DataAccess.Interfaces.Inventory.InventoryManagement;
using Dal.DataContext;
using Dal.DtoConverter.Inventory.StockTakking;
using DTO.Inventory.StockTakking;
using Logger;
using Logger.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using CustomLogger = Logger.Interfaces.ILogger;

namespace Dal.DataAccess.Implementations.Inventory.InventoryManagement;

/// <summary>
/// Repository implementation cho StocktakingMaster
/// Quản lý các thao tác CRUD với bảng StocktakingMaster (Phiếu kiểm kho)
/// </summary>
public class StocktakingMasterRepository : IStocktakingMasterRepository
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
    /// Khởi tạo một instance mới của class StocktakingMasterRepository.
    /// </summary>
    /// <param name="connectionString">Chuỗi kết nối cơ sở dữ liệu</param>
    /// <exception cref="ArgumentNullException">Ném ra khi connectionString là null</exception>
    public StocktakingMasterRepository(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _logger = LoggerFactory.CreateLogger(LogCategory.DAL);
        _logger.Info("StocktakingMasterRepository được khởi tạo với connection string");
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Tạo DataContext mới cho mỗi operation để tránh cache issue
    /// </summary>
    /// <returns>DataContext mới</returns>
    private VnsErp2025DataContext CreateNewContext()
    {
        return new VnsErp2025DataContext(_connectionString);
    }

    /// <summary>
    /// Lấy dictionary thông tin Warehouse cho danh sách StocktakingMaster
    /// Bao gồm: WarehouseName và WarehouseCode
    /// </summary>
    private Dictionary<Guid, (string WarehouseName, string WarehouseCode)> GetWarehouseDict(
        VnsErp2025DataContext context,
        IEnumerable<StocktakingMaster> masters)
    {
        var warehouseIds = masters
            .Where(m => m.WarehouseId != Guid.Empty)
            .Select(m => m.WarehouseId)
            .Distinct()
            .ToList();

        if (!warehouseIds.Any())
            return new Dictionary<Guid, (string, string)>();

        // Query Warehouse từ bảng Warehouse (nếu có) hoặc từ CompanyBranch
        // Tạm thời để null, có thể mở rộng sau khi biết cấu trúc bảng Warehouse
        var warehouses = context.CompanyBranches
            .Where(cb => warehouseIds.Contains(cb.Id))
            .Select(cb => new
            {
                cb.Id,
                WarehouseName = cb.BranchName ?? cb.BranchCode,
                WarehouseCode = cb.BranchCode
            })
            .ToList();

        return warehouses.ToDictionary(
            w => w.Id,
            w => (w.WarehouseName, w.WarehouseCode)
        );
    }

    /// <summary>
    /// Lấy thông tin Warehouse cho một StocktakingMaster
    /// </summary>
    private (string WarehouseName, string WarehouseCode) GetWarehouseInfo(
        VnsErp2025DataContext context,
        Guid warehouseId)
    {
        if (warehouseId == Guid.Empty)
            return (null, null);

        // Query Warehouse từ bảng Warehouse (nếu có) hoặc từ CompanyBranch
        // Tạm thời query từ CompanyBranch, có thể mở rộng sau
        var warehouse = context.CompanyBranches.FirstOrDefault(cb => cb.Id == warehouseId);
        if (warehouse == null)
            return (null, null);

        return (warehouse.BranchName ?? warehouse.BranchCode, warehouse.BranchCode);
    }

    #endregion

    #region ========== READ OPERATIONS ==========

    /// <summary>
    /// Lấy tất cả StocktakingMaster
    /// </summary>
    /// <returns>Danh sách tất cả StocktakingMasterDto</returns>
    public List<StocktakingMasterDto> GetAll()
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetAll: Lấy tất cả phiếu kiểm kho");

            var entities = context.StocktakingMasters
                .Where(m => !m.IsDeleted)
                .ToList();

            // Fetch related data trước khi convert để tránh DataContext disposed errors
            var warehouseDict = GetWarehouseDict(context, entities);

            _logger.Info("GetAll: Lấy được {0} phiếu kiểm kho", entities.Count);
            return entities.ToDtoList(warehouseDict);
        }
        catch (Exception ex)
        {
            _logger.Error($"GetAll: Lỗi lấy danh sách phiếu kiểm kho: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Lấy StocktakingMaster theo ID
    /// </summary>
    /// <param name="id">ID của StocktakingMaster</param>
    /// <returns>StocktakingMasterDto hoặc null</returns>
    public StocktakingMasterDto GetById(Guid id)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetById: Lấy phiếu kiểm kho, Id={0}", id);

            var entity = context.StocktakingMasters.FirstOrDefault(m => m.Id == id && !m.IsDeleted);

            if (entity == null)
            {
                _logger.Warning("GetById: Không tìm thấy phiếu kiểm kho, Id={0}", id);
                return null;
            }

            // Fetch related data trước khi convert để tránh DataContext disposed errors
            var (warehouseName, warehouseCode) = GetWarehouseInfo(context, entity.WarehouseId);

            _logger.Info("GetById: Lấy phiếu kiểm kho thành công, Id={0}", id);
            return entity.ToDto(warehouseName, warehouseCode);
        }
        catch (Exception ex)
        {
            _logger.Error($"GetById: Lỗi lấy phiếu kiểm kho: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Lấy danh sách StocktakingMaster theo WarehouseId
    /// </summary>
    /// <param name="warehouseId">ID kho</param>
    /// <returns>Danh sách StocktakingMasterDto</returns>
    public List<StocktakingMasterDto> GetByWarehouseId(Guid warehouseId)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetByWarehouseId: Lấy danh sách phiếu kiểm kho, WarehouseId={0}", warehouseId);

            var entities = context.StocktakingMasters
                .Where(m => m.WarehouseId == warehouseId && !m.IsDeleted)
                .ToList();

            // Fetch related data trước khi convert để tránh DataContext disposed errors
            var warehouseDict = GetWarehouseDict(context, entities);

            _logger.Info("GetByWarehouseId: Lấy được {0} phiếu kiểm kho", entities.Count);
            return entities.ToDtoList(warehouseDict);
        }
        catch (Exception ex)
        {
            _logger.Error($"GetByWarehouseId: Lỗi lấy danh sách phiếu kiểm kho: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Lấy danh sách StocktakingMaster theo trạng thái
    /// </summary>
    /// <param name="stocktakingStatus">Trạng thái kiểm kho</param>
    /// <returns>Danh sách StocktakingMasterDto</returns>
    public List<StocktakingMasterDto> GetByStatus(int stocktakingStatus)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetByStatus: Lấy danh sách phiếu kiểm kho, Status={0}", stocktakingStatus);

            var entities = context.StocktakingMasters
                .Where(m => m.StocktakingStatus == stocktakingStatus && !m.IsDeleted)
                .ToList();

            // Fetch related data trước khi convert để tránh DataContext disposed errors
            var warehouseDict = GetWarehouseDict(context, entities);

            _logger.Info("GetByStatus: Lấy được {0} phiếu kiểm kho với Status={1}", entities.Count, stocktakingStatus);
            return entities.ToDtoList(warehouseDict);
        }
        catch (Exception ex)
        {
            _logger.Error($"GetByStatus: Lỗi lấy danh sách phiếu kiểm kho: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Lấy danh sách StocktakingMaster theo loại
    /// </summary>
    /// <param name="stocktakingType">Loại kiểm kho</param>
    /// <returns>Danh sách StocktakingMasterDto</returns>
    public List<StocktakingMasterDto> GetByType(int stocktakingType)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetByType: Lấy danh sách phiếu kiểm kho, Type={0}", stocktakingType);

            var entities = context.StocktakingMasters
                .Where(m => m.StocktakingType == stocktakingType && !m.IsDeleted)
                .ToList();

            // Fetch related data trước khi convert để tránh DataContext disposed errors
            var warehouseDict = GetWarehouseDict(context, entities);

            _logger.Info("GetByType: Lấy được {0} phiếu kiểm kho với Type={1}", entities.Count, stocktakingType);
            return entities.ToDtoList(warehouseDict);
        }
        catch (Exception ex)
        {
            _logger.Error($"GetByType: Lỗi lấy danh sách phiếu kiểm kho: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Tìm StocktakingMaster theo số phiếu
    /// </summary>
    /// <param name="voucherNumber">Số phiếu kiểm kho</param>
    /// <returns>StocktakingMasterDto nếu tìm thấy, null nếu không tìm thấy</returns>
    public StocktakingMasterDto FindByVoucherNumber(string voucherNumber)
    {
        using var context = CreateNewContext();
        try
        {
            if (string.IsNullOrWhiteSpace(voucherNumber))
            {
                _logger.Warning("FindByVoucherNumber: VoucherNumber is null or empty");
                return null;
            }

            _logger.Debug("FindByVoucherNumber: Tìm phiếu kiểm kho, VoucherNumber={0}", voucherNumber);

            var entity = context.StocktakingMasters.FirstOrDefault(m =>
                !m.IsDeleted &&
                m.VoucherNumber != null &&
                m.VoucherNumber.Trim().ToLower() == voucherNumber.Trim().ToLower());

            if (entity == null)
            {
                _logger.Warning("FindByVoucherNumber: Không tìm thấy phiếu kiểm kho với VoucherNumber, VoucherNumber={0}", voucherNumber);
                return null;
            }

            // Fetch related data trước khi convert để tránh DataContext disposed errors
            var (warehouseName, warehouseCode) = GetWarehouseInfo(context, entity.WarehouseId);

            _logger.Info("FindByVoucherNumber: Tìm thấy phiếu kiểm kho, Id={0}, VoucherNumber={1}", entity.Id, voucherNumber);
            return entity.ToDto(warehouseName, warehouseCode);
        }
        catch (Exception ex)
        {
            _logger.Error($"FindByVoucherNumber: Lỗi tìm phiếu kiểm kho: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Lấy số thứ tự tiếp theo cho số phiếu kiểm kho
    /// Dựa trên năm để tìm số thứ tự cao nhất và trả về số tiếp theo
    /// Format số phiếu: KK-YYYY-XXX (KK = Kiểm kho, YYYY = Năm 4 ký tự, XXX = số thứ tự 3 ký tự)
    /// </summary>
    /// <param name="stocktakingDate">Ngày của phiếu kiểm kho</param>
    /// <returns>Số thứ tự tiếp theo (bắt đầu từ 1 nếu chưa có phiếu nào trong năm đó)</returns>
    public int GetNextSequenceNumber(DateTime stocktakingDate)
    {
        using var context = CreateNewContext();
        try
        {
            // Lấy năm từ ngày
            var year = stocktakingDate.Year;

            // Tìm tất cả các phiếu trong cùng năm
            var existingMasters = context.StocktakingMasters
                .Where(m => m.StocktakingDate.Year == year && !m.IsDeleted)
                .ToList();

            if (existingMasters.Count == 0)
            {
                _logger.Info("GetNextSequenceNumber: Không có phiếu nào trong năm {0}. Trả về số 1", year);
                return 1;
            }

            // Parse sequence numbers từ các voucher numbers
            // Format: KK-YYYY-XXX, trong đó XXX là sequence number (3 chữ số cuối)
            var sequenceNumbers = new List<int>();

            foreach (var master in existingMasters)
            {
                if (string.IsNullOrEmpty(master.VoucherNumber))
                    continue;

                // Tách voucher number theo dấu '-'
                var parts = master.VoucherNumber.Split('-');
                if (parts.Length >= 3)
                {
                    // Phần cuối cùng chứa sequence number (3 chữ số)
                    var lastPart = parts[2];
                    if (lastPart.Length >= 3)
                    {
                        // Lấy 3 chữ số cuối làm sequence number
                        var sequenceStr = lastPart.Substring(lastPart.Length - 3);
                        if (int.TryParse(sequenceStr, out int seqNum))
                        {
                            sequenceNumbers.Add(seqNum);
                        }
                    }
                }
            }

            if (sequenceNumbers.Count == 0)
            {
                _logger.Info("GetNextSequenceNumber: Không thể parse sequence number từ các voucher numbers. Trả về số 1");
                return 1;
            }

            // Tìm số thứ tự cao nhất
            var maxSequence = sequenceNumbers.Max();
            var nextSequence = maxSequence + 1;

            // Giới hạn tối đa 999 phiếu mỗi năm
            if (nextSequence > 999)
            {
                _logger.Warning("GetNextSequenceNumber: Số thứ tự vượt quá 999 trong năm {0}. Trả về số 999", year);
                return 999;
            }

            _logger.Info("GetNextSequenceNumber: Năm {0}, max sequence={1}, next sequence={2}",
                year, maxSequence, nextSequence);

            return nextSequence;
        }
        catch (Exception ex)
        {
            _logger.Error($"GetNextSequenceNumber: Lỗi lấy số thứ tự tiếp theo: {ex.Message}", ex);
            throw;
        }
    }

    #endregion

    #region ========== CREATE/UPDATE OPERATIONS ==========

    /// <summary>
    /// Lưu hoặc cập nhật StocktakingMaster
    /// </summary>
    /// <param name="dto">StocktakingMasterDto cần lưu</param>
    /// <returns>StocktakingMasterDto đã được lưu</returns>
    public StocktakingMasterDto SaveOrUpdate(StocktakingMasterDto dto)
    {
        var context = new VnsErp2025DataContext(_connectionString);
        try
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            _logger.Debug("SaveOrUpdate: Bắt đầu lưu phiếu kiểm kho, Id={0}, VoucherNumber={1}",
                dto.Id, dto.VoucherNumber);

            var existingEntity = dto.Id != Guid.Empty ?
                context.StocktakingMasters.FirstOrDefault(m => m.Id == dto.Id) : null;

            StocktakingMaster entity;
            if (existingEntity == null)
            {
                // Thêm mới
                entity = dto.ToEntity();
                if (entity.Id == Guid.Empty)
                {
                    entity.Id = Guid.NewGuid();
                    dto.Id = entity.Id;
                }
                if (entity.CreatedDate == null)
                {
                    entity.CreatedDate = DateTime.Now;
                    dto.CreatedDate = entity.CreatedDate;
                }
                context.StocktakingMasters.InsertOnSubmit(entity);
                _logger.Info("SaveOrUpdate: Thêm mới phiếu kiểm kho, Id={0}", entity.Id);
            }
            else
            {
                // Cập nhật
                dto.ToEntity(existingEntity);
                existingEntity.UpdatedDate = DateTime.Now;
                entity = existingEntity;
                _logger.Info("SaveOrUpdate: Cập nhật phiếu kiểm kho, Id={0}", entity.Id);
            }

            context.SubmitChanges();
            _logger.Info("SaveOrUpdate: Lưu phiếu kiểm kho thành công, Id={0}", entity.Id);

            // Load lại entity và fetch related data để convert sang DTO
            var savedEntity = context.StocktakingMasters.FirstOrDefault(m => m.Id == entity.Id);
            if (savedEntity == null)
                return null;

            // Fetch related data trước khi convert để tránh DataContext disposed errors
            var (warehouseName, warehouseCode) = GetWarehouseInfo(context, savedEntity.WarehouseId);

            return savedEntity.ToDto(warehouseName, warehouseCode);
        }
        catch (Exception ex)
        {
            _logger.Error($"SaveOrUpdate: Lỗi lưu phiếu kiểm kho: {ex.Message}", ex);
            throw;
        }
        finally
        {
            context.Dispose();
        }
    }

    #endregion

    #region ========== DELETE OPERATIONS ==========

    /// <summary>
    /// Xóa StocktakingMaster theo ID (soft delete)
    /// </summary>
    /// <param name="id">ID của StocktakingMaster cần xóa</param>
    /// <returns>True nếu xóa thành công, False nếu không tìm thấy</returns>
    public bool Delete(Guid id)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("Delete: Xóa phiếu kiểm kho, Id={0}", id);

            var entity = context.StocktakingMasters.FirstOrDefault(m => m.Id == id);

            if (entity == null)
            {
                _logger.Warning("Delete: Không tìm thấy phiếu kiểm kho để xóa, Id={0}", id);
                return false;
            }

            // Soft delete: Set IsDeleted = true thay vì xóa thực sự
            entity.IsDeleted = true;
            entity.DeletedDate = DateTime.Now;
            entity.UpdatedDate = DateTime.Now;

            context.SubmitChanges();
            _logger.Info("Delete: Xóa phiếu kiểm kho thành công (soft delete), Id={0}", id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.Error($"Delete: Lỗi xóa phiếu kiểm kho: {ex.Message}", ex);
            throw;
        }
    }

    #endregion
}
