using Dal.DataAccess.Interfaces.Inventory.InventoryManagement;
using Dal.DataContext;
using Logger;
using Logger.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using CustomLogger = Logger.Interfaces.ILogger;

namespace Dal.DataAccess.Implementations.Inventory.InventoryManagement;

/// <summary>
/// Repository implementation cho InventoryBalance
/// Quản lý các thao tác CRUD với bảng InventoryBalance (Tồn kho theo tháng)
/// </summary>
public class InventoryBalanceRepository : IInventoryBalanceRepository
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
    /// Khởi tạo một instance mới của class InventoryBalanceRepository.
    /// </summary>
    /// <param name="connectionString">Chuỗi kết nối cơ sở dữ liệu</param>
    /// <exception cref="ArgumentNullException">Ném ra khi connectionString là null</exception>
    public InventoryBalanceRepository(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _logger = LoggerFactory.CreateLogger(LogCategory.DAL);
        _logger.Info("InventoryBalanceRepository được khởi tạo với connection string");
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
    /// Lưu hoặc cập nhật tồn kho
    /// </summary>
    /// <param name="inventoryBalance">Entity tồn kho cần lưu</param>
    public void SaveOrUpdate(InventoryBalance inventoryBalance)
    {
        if (inventoryBalance == null)
            throw new ArgumentNullException(nameof(inventoryBalance));

        using var context = CreateNewContext();
        try
        {
            _logger.Debug("SaveOrUpdate: Bắt đầu lưu tồn kho, Id={0}, WarehouseId={1}, ProductVariantId={2}, Period={3}/{4}", 
                inventoryBalance.Id, inventoryBalance.WarehouseId, inventoryBalance.ProductVariantId, 
                inventoryBalance.PeriodYear, inventoryBalance.PeriodMonth);

            var existing = inventoryBalance.Id != Guid.Empty ? 
                context.InventoryBalances.FirstOrDefault(x => x.Id == inventoryBalance.Id) : null;

            if (existing == null)
            {
                // Thêm mới
                if (inventoryBalance.Id == Guid.Empty)
                    inventoryBalance.Id = Guid.NewGuid();
                
                // Thiết lập giá trị mặc định
                if (inventoryBalance.CreateDate == default(DateTime))
                    inventoryBalance.CreateDate = DateTime.Now;

                context.InventoryBalances.InsertOnSubmit(inventoryBalance);
                context.SubmitChanges();
                
                _logger.Info("SaveOrUpdate: Đã thêm mới tồn kho, Id={0}", inventoryBalance.Id);
            }
            else
            {
                // Cập nhật
                existing.WarehouseId = inventoryBalance.WarehouseId;
                existing.ProductVariantId = inventoryBalance.ProductVariantId;
                existing.PeriodYear = inventoryBalance.PeriodYear;
                existing.PeriodMonth = inventoryBalance.PeriodMonth;
                existing.OpeningBalance = inventoryBalance.OpeningBalance;
                existing.TotalInQty = inventoryBalance.TotalInQty;
                existing.TotalOutQty = inventoryBalance.TotalOutQty;
                existing.ClosingBalance = inventoryBalance.ClosingBalance;
                existing.OpeningValue = inventoryBalance.OpeningValue;
                existing.TotalInValue = inventoryBalance.TotalInValue;
                existing.TotalOutValue = inventoryBalance.TotalOutValue;
                existing.ClosingValue = inventoryBalance.ClosingValue;
                existing.TotalInVatAmount = inventoryBalance.TotalInVatAmount;
                existing.TotalOutVatAmount = inventoryBalance.TotalOutVatAmount;
                existing.TotalInAmountIncludedVat = inventoryBalance.TotalInAmountIncludedVat;
                existing.TotalOutAmountIncludedVat = inventoryBalance.TotalOutAmountIncludedVat;
                existing.IsLocked = inventoryBalance.IsLocked;
                existing.LockedDate = inventoryBalance.LockedDate;
                existing.LockedBy = inventoryBalance.LockedBy;
                existing.LockReason = inventoryBalance.LockReason;
                existing.IsVerified = inventoryBalance.IsVerified;
                existing.VerifiedDate = inventoryBalance.VerifiedDate;
                existing.VerifiedBy = inventoryBalance.VerifiedBy;
                existing.VerificationNotes = inventoryBalance.VerificationNotes;
                existing.IsApproved = inventoryBalance.IsApproved;
                existing.ApprovedDate = inventoryBalance.ApprovedDate;
                existing.ApprovedBy = inventoryBalance.ApprovedBy;
                existing.ApprovalNotes = inventoryBalance.ApprovalNotes;
                existing.Status = inventoryBalance.Status;
                existing.Notes = inventoryBalance.Notes;
                existing.ModifiedDate = DateTime.Now;
                existing.ModifiedBy = inventoryBalance.ModifiedBy;
                existing.IsActive = inventoryBalance.IsActive;
                
                context.SubmitChanges();
                
                _logger.Info("SaveOrUpdate: Đã cập nhật tồn kho, Id={0}", existing.Id);
            }
        }
        catch (Exception ex)
        {
            _logger.Error($"SaveOrUpdate: Lỗi lưu tồn kho: {ex.Message}", ex);
            throw;
        }
    }

    #endregion

    #region Query Operations

    /// <summary>
    /// Lấy tồn kho theo ID
    /// </summary>
    /// <param name="id">ID tồn kho</param>
    /// <returns>InventoryBalance hoặc null</returns>
    public InventoryBalance GetById(Guid id)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetById: Lấy tồn kho, Id={0}", id);

            var balance = context.InventoryBalances.FirstOrDefault(x => x.Id == id);

            if (balance == null)
            {
                _logger.Warning("GetById: Không tìm thấy tồn kho với Id={0}", id);
            }
            else
            {
                _logger.Info("GetById: Đã lấy tồn kho, Id={0}, WarehouseId={1}, ProductVariantId={2}, Period={3}/{4}", 
                    id, balance.WarehouseId, balance.ProductVariantId, balance.PeriodYear, balance.PeriodMonth);
            }

            return balance;
        }
        catch (Exception ex)
        {
            _logger.Error($"GetById: Lỗi lấy tồn kho: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Lấy tồn kho theo kho, sản phẩm và kỳ
    /// </summary>
    /// <param name="warehouseId">ID kho</param>
    /// <param name="productVariantId">ID biến thể sản phẩm</param>
    /// <param name="periodYear">Năm</param>
    /// <param name="periodMonth">Tháng (1-12)</param>
    /// <returns>InventoryBalance hoặc null</returns>
    public InventoryBalance GetByPeriod(Guid warehouseId, Guid productVariantId, int periodYear, int periodMonth)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetByPeriod: Lấy tồn kho, WarehouseId={0}, ProductVariantId={1}, Period={2}/{3}", 
                warehouseId, productVariantId, periodYear, periodMonth);

            var balance = context.InventoryBalances
                .FirstOrDefault(x => x.WarehouseId == warehouseId &&
                                   x.ProductVariantId == productVariantId &&
                                   x.PeriodYear == periodYear &&
                                   x.PeriodMonth == periodMonth);

            if (balance == null)
            {
                _logger.Warning("GetByPeriod: Không tìm thấy tồn kho với WarehouseId={0}, ProductVariantId={1}, Period={2}/{3}", 
                    warehouseId, productVariantId, periodYear, periodMonth);
            }
            else
            {
                _logger.Info("GetByPeriod: Đã lấy tồn kho, Id={0}", balance.Id);
            }

            return balance;
        }
        catch (Exception ex)
        {
            _logger.Error($"GetByPeriod: Lỗi lấy tồn kho: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Lấy danh sách tồn kho theo kho
    /// </summary>
    /// <param name="warehouseId">ID kho</param>
    /// <returns>Danh sách tồn kho</returns>
    public List<InventoryBalance> GetByWarehouseId(Guid warehouseId)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetByWarehouseId: Lấy danh sách tồn kho, WarehouseId={0}", warehouseId);

            var balances = context.InventoryBalances
                .Where(x => x.WarehouseId == warehouseId)
                .OrderBy(x => x.PeriodYear)
                .ThenBy(x => x.PeriodMonth)
                .ThenBy(x => x.CreateDate)
                .ToList();

            // Load navigation properties
            LoadNavigationProperties(context, balances);

            _logger.Info("GetByWarehouseId: Lấy được {0} tồn kho", balances.Count);
            return balances;
        }
        catch (Exception ex)
        {
            _logger.Error($"GetByWarehouseId: Lỗi lấy danh sách tồn kho: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Lấy danh sách tồn kho theo sản phẩm
    /// </summary>
    /// <param name="productVariantId">ID biến thể sản phẩm</param>
    /// <returns>Danh sách tồn kho</returns>
    public List<InventoryBalance> GetByProductVariantId(Guid productVariantId)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetByProductVariantId: Lấy danh sách tồn kho, ProductVariantId={0}", productVariantId);

            var balances = context.InventoryBalances
                .Where(x => x.ProductVariantId == productVariantId)
                .OrderBy(x => x.PeriodYear)
                .ThenBy(x => x.PeriodMonth)
                .ThenBy(x => x.CreateDate)
                .ToList();

            // Load navigation properties
            LoadNavigationProperties(context, balances);

            _logger.Info("GetByProductVariantId: Lấy được {0} tồn kho", balances.Count);
            return balances;
        }
        catch (Exception ex)
        {
            _logger.Error($"GetByProductVariantId: Lỗi lấy danh sách tồn kho: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Lấy danh sách tồn kho theo kỳ
    /// </summary>
    /// <param name="periodYear">Năm</param>
    /// <param name="periodMonth">Tháng (1-12)</param>
    /// <returns>Danh sách tồn kho</returns>
    public List<InventoryBalance> GetByPeriod(int periodYear, int periodMonth)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetByPeriod: Lấy danh sách tồn kho, Period={0}/{1}", periodYear, periodMonth);

            var balances = context.InventoryBalances
                .Where(x => x.PeriodYear == periodYear && x.PeriodMonth == periodMonth)
                .OrderBy(x => x.CreateDate)
                .ToList();

            // Load navigation properties
            LoadNavigationProperties(context, balances);

            _logger.Info("GetByPeriod: Lấy được {0} tồn kho", balances.Count);
            return balances;
        }
        catch (Exception ex)
        {
            _logger.Error($"GetByPeriod: Lỗi lấy danh sách tồn kho: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Query tồn kho theo nhiều tiêu chí
    /// </summary>
    public List<InventoryBalance> QueryBalances(
        Guid? warehouseId = null,
        Guid? productVariantId = null,
        int? periodYear = null,
        int? periodMonth = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        bool? isLocked = null,
        bool? isVerified = null,
        bool? isApproved = null,
        int? status = null)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("QueryBalances: Bắt đầu query tồn kho, WarehouseId={0}, ProductVariantId={1}, Period={2}/{3}, IsLocked={4}", 
                warehouseId, productVariantId, periodYear, periodMonth, isLocked);

            // Bắt đầu query từ InventoryBalance
            var queryable = context.InventoryBalances.AsQueryable();

            // Filter theo kho
            if (warehouseId.HasValue)
            {
                queryable = queryable.Where(x => x.WarehouseId == warehouseId.Value);
            }

            // Filter theo sản phẩm
            if (productVariantId.HasValue)
            {
                queryable = queryable.Where(x => x.ProductVariantId == productVariantId.Value);
            }

            // Filter theo kỳ
            if (periodYear.HasValue)
            {
                queryable = queryable.Where(x => x.PeriodYear == periodYear.Value);
            }

            if (periodMonth.HasValue)
            {
                queryable = queryable.Where(x => x.PeriodMonth == periodMonth.Value);
            }

            // Filter theo thời gian (CreateDate)
            if (fromDate.HasValue && toDate.HasValue)
            {
                queryable = queryable.Where(x => x.CreateDate >= fromDate.Value.Date && 
                                                x.CreateDate <= toDate.Value.Date.AddDays(1).AddTicks(-1));
            }

            // Filter theo trạng thái khóa
            if (isLocked.HasValue)
            {
                queryable = queryable.Where(x => x.IsLocked == isLocked.Value);
            }

            // Filter theo trạng thái xác thực
            if (isVerified.HasValue)
            {
                queryable = queryable.Where(x => x.IsVerified == isVerified.Value);
            }

            // Filter theo trạng thái phê duyệt
            if (isApproved.HasValue)
            {
                queryable = queryable.Where(x => x.IsApproved == isApproved.Value);
            }

            // Filter theo status
            if (status.HasValue)
            {
                queryable = queryable.Where(x => x.Status == status.Value);
            }

            // Sắp xếp theo kỳ và ngày tạo
            var result = queryable
                .OrderBy(x => x.PeriodYear)
                .ThenBy(x => x.PeriodMonth)
                .ThenBy(x => x.CreateDate)
                .ToList();

            // Load navigation properties
            LoadNavigationProperties(context, result);

            _logger.Info("QueryBalances: Query thành công, ResultCount={0}", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.Error($"QueryBalances: Lỗi query tồn kho: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Lấy danh sách tồn kho cần xác thực (IsVerified = false)
    /// </summary>
    /// <returns>Danh sách tồn kho cần xác thực</returns>
    public List<InventoryBalance> GetUnverifiedBalances()
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetUnverifiedBalances: Lấy danh sách tồn kho cần xác thực");

            var balances = context.InventoryBalances
                .Where(x => x.IsVerified == false)
                .OrderBy(x => x.PeriodYear)
                .ThenBy(x => x.PeriodMonth)
                .ThenBy(x => x.CreateDate)
                .ToList();

            // Load navigation properties
            LoadNavigationProperties(context, balances);

            _logger.Info("GetUnverifiedBalances: Lấy được {0} tồn kho cần xác thực", balances.Count);
            return balances;
        }
        catch (Exception ex)
        {
            _logger.Error($"GetUnverifiedBalances: Lỗi lấy danh sách tồn kho cần xác thực: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Lấy danh sách tồn kho cần phê duyệt (IsVerified = true, IsApproved = false)
    /// </summary>
    /// <returns>Danh sách tồn kho cần phê duyệt</returns>
    public List<InventoryBalance> GetUnapprovedBalances()
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetUnapprovedBalances: Lấy danh sách tồn kho cần phê duyệt");

            var balances = context.InventoryBalances
                .Where(x => x.IsVerified == true && x.IsApproved == false)
                .OrderBy(x => x.PeriodYear)
                .ThenBy(x => x.PeriodMonth)
                .ThenBy(x => x.CreateDate)
                .ToList();

            // Load navigation properties
            LoadNavigationProperties(context, balances);

            _logger.Info("GetUnapprovedBalances: Lấy được {0} tồn kho cần phê duyệt", balances.Count);
            return balances;
        }
        catch (Exception ex)
        {
            _logger.Error($"GetUnapprovedBalances: Lỗi lấy danh sách tồn kho cần phê duyệt: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Load navigation properties cho danh sách tồn kho
    /// </summary>
    private void LoadNavigationProperties(VnsErp2025DataContext context, List<InventoryBalance> balances)
    {
        if (balances == null || !balances.Any())
            return;

        // Load CompanyBranch (Warehouse)
        var warehouseIds = balances.Select(x => x.WarehouseId).Distinct().ToList();
        if (warehouseIds.Any())
        {
            var warehouses = context.CompanyBranches
                .Where(w => warehouseIds.Contains(w.Id))
                .ToDictionary(w => w.Id);

            foreach (var balance in balances)
            {
                if (balance.CompanyBranch == null && warehouses.TryGetValue(balance.WarehouseId, out var warehouse))
                {
                    balance.CompanyBranch = warehouse;
                }
            }
        }

        // Load ProductVariant
        var productVariantIds = balances.Select(x => x.ProductVariantId).Distinct().ToList();
        if (productVariantIds.Any())
        {
            var productVariants = context.ProductVariants
                .Where(pv => productVariantIds.Contains(pv.Id))
                .ToDictionary(pv => pv.Id);

            foreach (var balance in balances)
            {
                if (balance.ProductVariant == null && productVariants.TryGetValue(balance.ProductVariantId, out var productVariant))
                {
                    balance.ProductVariant = productVariant;
                }
            }
        }

        // Load ApplicationUser (CreateBy, ModifiedBy, DeletedBy, LockedBy, VerifiedBy, ApprovedBy)
        var userIds = balances
            .SelectMany(x => new[] { 
                (Guid?)x.CreateBy, 
                x.ModifiedBy, 
                x.DeletedBy, 
                x.LockedBy, 
                x.VerifiedBy, 
                x.ApprovedBy 
            })
            .Where(id => id.HasValue)
            .Select(id => id.Value)
            .Distinct()
            .ToList();

        if (userIds.Any())
        {
            var users = context.ApplicationUsers
                .Where(u => userIds.Contains(u.Id))
                .ToDictionary(u => u.Id);

            foreach (var balance in balances)
            {
                // CreateBy (Guid, không nullable)
                if (balance.CreateBy != Guid.Empty && balance.ApplicationUser1 == null && 
                    users.TryGetValue(balance.CreateBy, out var createUser))
                {
                    balance.ApplicationUser1 = createUser;
                }

                // ModifiedBy (Guid?, nullable)
                if (balance.ModifiedBy.HasValue && balance.ApplicationUser4 == null && 
                    users.TryGetValue(balance.ModifiedBy.Value, out var modifiedUser))
                {
                    balance.ApplicationUser4 = modifiedUser;
                }

                // DeletedBy (Guid?, nullable)
                if (balance.DeletedBy.HasValue && balance.ApplicationUser2 == null && 
                    users.TryGetValue(balance.DeletedBy.Value, out var deletedUser))
                {
                    balance.ApplicationUser2 = deletedUser;
                }

                // LockedBy (Guid?, nullable)
                if (balance.LockedBy.HasValue && balance.ApplicationUser3 == null && 
                    users.TryGetValue(balance.LockedBy.Value, out var lockedUser))
                {
                    balance.ApplicationUser3 = lockedUser;
                }

                // VerifiedBy (Guid?, nullable)
                if (balance.VerifiedBy.HasValue && balance.ApplicationUser5 == null && 
                    users.TryGetValue(balance.VerifiedBy.Value, out var verifiedUser))
                {
                    balance.ApplicationUser5 = verifiedUser;
                }

                // ApprovedBy (Guid?, nullable)
                if (balance.ApprovedBy.HasValue && balance.ApplicationUser == null && 
                    users.TryGetValue(balance.ApprovedBy.Value, out var approvedUser))
                {
                    balance.ApplicationUser = approvedUser;
                }
            }
        }
    }

    #endregion

    #region Update Status Operations

    /// <summary>
    /// Cập nhật trạng thái khóa của tồn kho
    /// </summary>
    /// <param name="id">ID tồn kho</param>
    /// <param name="isLocked">Đã khóa</param>
    /// <param name="lockedBy">ID người khóa</param>
    /// <param name="lockReason">Lý do khóa</param>
    public void UpdateLockStatus(Guid id, bool isLocked, Guid lockedBy, string lockReason = null)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("UpdateLockStatus: Cập nhật trạng thái khóa, Id={0}, IsLocked={1}", id, isLocked);

            var balance = context.InventoryBalances.FirstOrDefault(x => x.Id == id);
            if (balance == null)
            {
                _logger.Warning("UpdateLockStatus: Không tìm thấy tồn kho với Id={0}", id);
                throw new InvalidOperationException($"Không tìm thấy tồn kho với ID: {id}");
            }

            balance.IsLocked = isLocked;
            balance.LockedBy = lockedBy;
            balance.LockedDate = isLocked ? DateTime.Now : (DateTime?)null;
            balance.LockReason = lockReason;
            balance.Status = isLocked ? 1 : 0; // 1: Locked, 0: Draft
            balance.ModifiedDate = DateTime.Now;
            balance.ModifiedBy = lockedBy;

            context.SubmitChanges();

            _logger.Info("UpdateLockStatus: Đã cập nhật trạng thái khóa, Id={0}, IsLocked={1}", id, isLocked);
        }
        catch (Exception ex)
        {
            _logger.Error($"UpdateLockStatus: Lỗi cập nhật trạng thái khóa: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Cập nhật trạng thái xác thực của tồn kho
    /// </summary>
    /// <param name="id">ID tồn kho</param>
    /// <param name="isVerified">Đã xác thực</param>
    /// <param name="verifiedBy">ID người xác thực</param>
    /// <param name="verificationNotes">Ghi chú xác thực</param>
    public void UpdateVerificationStatus(Guid id, bool isVerified, Guid verifiedBy, string verificationNotes = null)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("UpdateVerificationStatus: Cập nhật trạng thái xác thực, Id={0}, IsVerified={1}", id, isVerified);

            var balance = context.InventoryBalances.FirstOrDefault(x => x.Id == id);
            if (balance == null)
            {
                _logger.Warning("UpdateVerificationStatus: Không tìm thấy tồn kho với Id={0}", id);
                throw new InvalidOperationException($"Không tìm thấy tồn kho với ID: {id}");
            }

            balance.IsVerified = isVerified;
            balance.VerifiedBy = verifiedBy;
            balance.VerifiedDate = isVerified ? DateTime.Now : (DateTime?)null;
            balance.VerificationNotes = verificationNotes;
            balance.Status = isVerified ? 2 : (balance.IsLocked ? 1 : 0); // 2: Verified, 1: Locked, 0: Draft
            balance.ModifiedDate = DateTime.Now;
            balance.ModifiedBy = verifiedBy;

            context.SubmitChanges();

            _logger.Info("UpdateVerificationStatus: Đã cập nhật trạng thái xác thực, Id={0}, IsVerified={1}", id, isVerified);
        }
        catch (Exception ex)
        {
            _logger.Error($"UpdateVerificationStatus: Lỗi cập nhật trạng thái xác thực: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Cập nhật trạng thái phê duyệt của tồn kho
    /// </summary>
    /// <param name="id">ID tồn kho</param>
    /// <param name="isApproved">Đã phê duyệt</param>
    /// <param name="approvedBy">ID người phê duyệt</param>
    /// <param name="approvalNotes">Ghi chú phê duyệt</param>
    public void UpdateApprovalStatus(Guid id, bool isApproved, Guid approvedBy, string approvalNotes = null)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("UpdateApprovalStatus: Cập nhật trạng thái phê duyệt, Id={0}, IsApproved={1}", id, isApproved);

            var balance = context.InventoryBalances.FirstOrDefault(x => x.Id == id);
            if (balance == null)
            {
                _logger.Warning("UpdateApprovalStatus: Không tìm thấy tồn kho với Id={0}", id);
                throw new InvalidOperationException($"Không tìm thấy tồn kho với ID: {id}");
            }

            balance.IsApproved = isApproved;
            balance.ApprovedBy = approvedBy;
            balance.ApprovedDate = isApproved ? DateTime.Now : (DateTime?)null;
            balance.ApprovalNotes = approvalNotes;
            balance.Status = isApproved ? 3 : (balance.IsVerified ? 2 : (balance.IsLocked ? 1 : 0)); // 3: Approved, 2: Verified, 1: Locked, 0: Draft
            balance.ModifiedDate = DateTime.Now;
            balance.ModifiedBy = approvedBy;

            context.SubmitChanges();

            _logger.Info("UpdateApprovalStatus: Đã cập nhật trạng thái phê duyệt, Id={0}, IsApproved={1}", id, isApproved);
        }
        catch (Exception ex)
        {
            _logger.Error($"UpdateApprovalStatus: Lỗi cập nhật trạng thái phê duyệt: {ex.Message}", ex);
            throw;
        }
    }

    #endregion

    #region Delete Operations

    /// <summary>
    /// Xóa tồn kho theo ID
    /// </summary>
    /// <param name="id">ID tồn kho cần xóa</param>
    /// <param name="deletedBy">ID người xóa (optional, để tương thích với BLL)</param>
    public void Delete(Guid id, Guid deletedBy = default)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("Delete: Bắt đầu xóa tồn kho, Id={0}", id);

            var balance = context.InventoryBalances.FirstOrDefault(x => x.Id == id);
            if (balance == null)
            {
                _logger.Warning("Delete: Không tìm thấy tồn kho với Id={0}", id);
                throw new InvalidOperationException($"Không tìm thấy tồn kho với ID: {id}");
            }

            context.InventoryBalances.DeleteOnSubmit(balance);
            context.SubmitChanges();

            _logger.Info("Delete: Đã xóa tồn kho, Id={0}", id);
        }
        catch (Exception ex)
        {
            _logger.Error($"Delete: Lỗi xóa tồn kho: {ex.Message}", ex);
            throw;
        }
    }

    #endregion
}

