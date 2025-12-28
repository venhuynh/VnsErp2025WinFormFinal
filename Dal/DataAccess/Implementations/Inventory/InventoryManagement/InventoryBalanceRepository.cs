using Dal.DataAccess.Interfaces.Inventory.InventoryManagement;
using Dal.DataContext;
using Dal.DtoConverter;
using Dal.Exceptions;
using DTO.Inventory.InventoryManagement;
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
        loadOptions.LoadWith<InventoryBalance>(b => b.CompanyBranch);
        loadOptions.LoadWith<InventoryBalance>(b => b.ProductVariant);
        loadOptions.LoadWith<ProductVariant>(v => v.ProductService);
        loadOptions.LoadWith<ProductVariant>(v => v.UnitOfMeasure);
        loadOptions.LoadWith<InventoryBalance>(b => b.ApplicationUser); // ApprovedBy
        loadOptions.LoadWith<InventoryBalance>(b => b.ApplicationUser1); // CreateBy
        loadOptions.LoadWith<InventoryBalance>(b => b.ApplicationUser2); // DeletedBy
        loadOptions.LoadWith<InventoryBalance>(b => b.ApplicationUser3); // LockedBy
        loadOptions.LoadWith<InventoryBalance>(b => b.ApplicationUser4); // ModifiedBy, VerifiedBy
        context.LoadOptions = loadOptions;

        return context;
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

        // Load ProductVariant với ProductService và UnitOfMeasure
        var productVariantIds = balances.Select(x => x.ProductVariantId).Distinct().ToList();
        if (productVariantIds.Any())
        {
            var productVariants = context.ProductVariants
                .Where(pv => productVariantIds.Contains(pv.Id))
                .ToList();

            // Load ProductService cho các ProductVariants
            var productIds = productVariants
                .Where(pv => pv.ProductId != Guid.Empty)
                .Select(pv => pv.ProductId)
                .Distinct()
                .ToList();
            
            if (productIds.Any())
            {
                var productServices = context.ProductServices
                    .Where(ps => productIds.Contains(ps.Id))
                    .ToDictionary(ps => ps.Id);

                foreach (var productVariant in productVariants)
                {
                    if (productVariant.ProductService == null && 
                        productVariant.ProductId != Guid.Empty &&
                        productServices.TryGetValue(productVariant.ProductId, out var productService))
                    {
                        productVariant.ProductService = productService;
                    }
                }
            }

            // Load UnitOfMeasure cho các ProductVariants
            var unitIds = productVariants
                .Where(pv => pv.UnitId != Guid.Empty)
                .Select(pv => pv.UnitId)
                .Distinct()
                .ToList();

            if (unitIds.Any())
            {
                var unitOfMeasures = context.UnitOfMeasures
                    .Where(u => unitIds.Contains(u.Id))
                    .ToDictionary(u => u.Id);

                foreach (var productVariant in productVariants)
                {
                    if (productVariant.UnitOfMeasure == null && 
                        productVariant.UnitId != Guid.Empty &&
                        unitOfMeasures.TryGetValue(productVariant.UnitId, out var unitOfMeasure))
                    {
                        productVariant.UnitOfMeasure = unitOfMeasure;
                    }
                }
            }

            // Gán ProductVariant vào InventoryBalance và materialize navigation properties
            var productVariantsDict = productVariants.ToDictionary(pv => pv.Id);
            foreach (var balance in balances)
            {
                if (balance.ProductVariant == null && productVariantsDict.TryGetValue(balance.ProductVariantId, out var productVariant))
                {
                    balance.ProductVariant = productVariant;
                    _ = productVariant.ProductService?.Name;
                    _ = productVariant.UnitOfMeasure?.Name;
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
                if (balance.CreateBy != Guid.Empty && balance.ApplicationUser1 == null && 
                    users.TryGetValue(balance.CreateBy, out var createUser))
                {
                    balance.ApplicationUser1 = createUser;
                }

                if (balance.ModifiedBy.HasValue && balance.ApplicationUser4 == null && 
                    users.TryGetValue(balance.ModifiedBy.Value, out var modifiedUser))
                {
                    balance.ApplicationUser4 = modifiedUser;
                }

                if (balance.DeletedBy.HasValue && balance.ApplicationUser2 == null && 
                    users.TryGetValue(balance.DeletedBy.Value, out var deletedUser))
                {
                    balance.ApplicationUser2 = deletedUser;
                }

                if (balance.LockedBy.HasValue && balance.ApplicationUser3 == null && 
                    users.TryGetValue(balance.LockedBy.Value, out var lockedUser))
                {
                    balance.ApplicationUser3 = lockedUser;
                }

                if (balance.VerifiedBy.HasValue && balance.ApplicationUser4 == null && 
                    users.TryGetValue(balance.VerifiedBy.Value, out var verifiedUser))
                {
                    balance.ApplicationUser4 = verifiedUser;
                }

                if (balance.ApprovedBy.HasValue && balance.ApplicationUser == null && 
                    users.TryGetValue(balance.ApprovedBy.Value, out var approvedUser))
                {
                    balance.ApplicationUser = approvedUser;
                }
            }
        }
    }

    #endregion

    #region Create

    /// <summary>
    /// Thêm mới tồn kho
    /// </summary>
    /// <param name="inventoryBalance">DTO tồn kho cần thêm</param>
    /// <returns>ID của tồn kho vừa thêm</returns>
    public Guid Insert(InventoryBalanceDto inventoryBalance)
    {
        if (inventoryBalance == null)
            throw new ArgumentNullException(nameof(inventoryBalance));

        using var context = CreateNewContext();
        try
        {
            _logger.Debug("Insert: Bắt đầu thêm mới tồn kho, WarehouseId={0}, ProductVariantId={1}, Period={2}/{3}", 
                inventoryBalance.WarehouseId, inventoryBalance.ProductVariantId, 
                inventoryBalance.PeriodYear, inventoryBalance.PeriodMonth);

            // Chuyển đổi DTO sang Entity
            var entity = inventoryBalance.ToEntity();

            // Thiết lập giá trị mặc định
            if (entity.Id == Guid.Empty)
                entity.Id = Guid.NewGuid();
            
            if (entity.CreateDate == default(DateTime))
                entity.CreateDate = DateTime.Now;

            context.InventoryBalances.InsertOnSubmit(entity);
            context.SubmitChanges();

            _logger.Info("Insert: Đã thêm mới tồn kho, Id={0}", entity.Id);
            return entity.Id;
        }
        catch (Exception ex)
        {
            _logger.Error($"Insert: Lỗi thêm mới tồn kho: {ex.Message}", ex);
            throw new DataAccessException("Lỗi thêm mới tồn kho", ex);
        }
    }

    #endregion

    #region Retrieve

    /// <summary>
    /// Lấy tồn kho theo ID
    /// </summary>
    /// <param name="id">ID tồn kho</param>
    /// <returns>InventoryBalanceDto hoặc null</returns>
    public InventoryBalanceDto GetById(Guid id)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetById: Lấy tồn kho, Id={0}", id);

            var balance = context.InventoryBalances.FirstOrDefault(x => x.Id == id);

            if (balance == null)
            {
                _logger.Warning("GetById: Không tìm thấy tồn kho với Id={0}", id);
                return null;
            }

            _logger.Info("GetById: Đã lấy tồn kho, Id={0}, WarehouseId={1}, ProductVariantId={2}, Period={3}/{4}", 
                id, balance.WarehouseId, balance.ProductVariantId, balance.PeriodYear, balance.PeriodMonth);

            return balance.ToDto();
        }
        catch (Exception ex)
        {
            _logger.Error($"GetById: Lỗi lấy tồn kho: {ex.Message}", ex);
            throw new DataAccessException("Lỗi lấy tồn kho", ex);
        }
    }

    /// <summary>
    /// Lấy tồn kho theo kho, sản phẩm và kỳ
    /// </summary>
    /// <param name="warehouseId">ID kho</param>
    /// <param name="productVariantId">ID biến thể sản phẩm</param>
    /// <param name="periodYear">Năm</param>
    /// <param name="periodMonth">Tháng (1-12)</param>
    /// <returns>InventoryBalanceDto hoặc null</returns>
    public InventoryBalanceDto GetByPeriod(Guid warehouseId, Guid productVariantId, int periodYear, int periodMonth)
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
                return null;
            }

            _logger.Info("GetByPeriod: Đã lấy tồn kho, Id={0}", balance.Id);
            return balance.ToDto();
        }
        catch (Exception ex)
        {
            _logger.Error($"GetByPeriod: Lỗi lấy tồn kho: {ex.Message}", ex);
            throw new DataAccessException("Lỗi lấy tồn kho", ex);
        }
    }

    /// <summary>
    /// Lấy danh sách tồn kho theo kho
    /// </summary>
    /// <param name="warehouseId">ID kho</param>
    /// <returns>Danh sách tồn kho</returns>
    public List<InventoryBalanceDto> GetByWarehouseId(Guid warehouseId)
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

            LoadNavigationProperties(context, balances);

            _logger.Info("GetByWarehouseId: Lấy được {0} tồn kho", balances.Count);
            return balances.Select(x => x.ToDto()).ToList();
        }
        catch (Exception ex)
        {
            _logger.Error($"GetByWarehouseId: Lỗi lấy danh sách tồn kho: {ex.Message}", ex);
            throw new DataAccessException("Lỗi lấy danh sách tồn kho", ex);
        }
    }

    /// <summary>
    /// Lấy danh sách tồn kho theo sản phẩm
    /// </summary>
    /// <param name="productVariantId">ID biến thể sản phẩm</param>
    /// <returns>Danh sách tồn kho</returns>
    public List<InventoryBalanceDto> GetByProductVariantId(Guid productVariantId)
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

            LoadNavigationProperties(context, balances);

            _logger.Info("GetByProductVariantId: Lấy được {0} tồn kho", balances.Count);
            return balances.Select(x => x.ToDto()).ToList();
        }
        catch (Exception ex)
        {
            _logger.Error($"GetByProductVariantId: Lỗi lấy danh sách tồn kho: {ex.Message}", ex);
            throw new DataAccessException("Lỗi lấy danh sách tồn kho", ex);
        }
    }

    /// <summary>
    /// Lấy danh sách tồn kho theo kỳ
    /// </summary>
    /// <param name="periodYear">Năm</param>
    /// <param name="periodMonth">Tháng (1-12)</param>
    /// <returns>Danh sách tồn kho</returns>
    public List<InventoryBalanceDto> GetByPeriod(int periodYear, int periodMonth)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetByPeriod: Lấy danh sách tồn kho, Period={0}/{1}", periodYear, periodMonth);

            var balances = context.InventoryBalances
                .Where(x => x.PeriodYear == periodYear && x.PeriodMonth == periodMonth)
                .OrderBy(x => x.CreateDate)
                .ToList();

            LoadNavigationProperties(context, balances);

            _logger.Info("GetByPeriod: Lấy được {0} tồn kho", balances.Count);
            return balances.Select(x => x.ToDto()).ToList();
        }
        catch (Exception ex)
        {
            _logger.Error($"GetByPeriod: Lỗi lấy danh sách tồn kho: {ex.Message}", ex);
            throw new DataAccessException("Lỗi lấy danh sách tồn kho", ex);
        }
    }

    /// <summary>
    /// Query tồn kho theo nhiều tiêu chí
    /// </summary>
    public List<InventoryBalanceDto> QueryBalances(
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

            var queryable = context.InventoryBalances.AsQueryable();

            if (warehouseId.HasValue)
                queryable = queryable.Where(x => x.WarehouseId == warehouseId.Value);

            if (productVariantId.HasValue)
                queryable = queryable.Where(x => x.ProductVariantId == productVariantId.Value);

            if (periodYear.HasValue)
                queryable = queryable.Where(x => x.PeriodYear == periodYear.Value);

            if (periodMonth.HasValue)
                queryable = queryable.Where(x => x.PeriodMonth == periodMonth.Value);

            if (fromDate.HasValue && toDate.HasValue)
            {
                queryable = queryable.Where(x => x.CreateDate >= fromDate.Value.Date && 
                                                x.CreateDate <= toDate.Value.Date.AddDays(1).AddTicks(-1));
            }

            if (isLocked.HasValue)
                queryable = queryable.Where(x => x.IsLocked == isLocked.Value);

            if (isVerified.HasValue)
                queryable = queryable.Where(x => x.IsVerified == isVerified.Value);

            if (isApproved.HasValue)
                queryable = queryable.Where(x => x.IsApproved == isApproved.Value);

            if (status.HasValue)
                queryable = queryable.Where(x => x.Status == status.Value);

            var result = queryable
                .OrderBy(x => x.PeriodYear)
                .ThenBy(x => x.PeriodMonth)
                .ThenBy(x => x.CreateDate)
                .ToList();

            LoadNavigationProperties(context, result);

            _logger.Info("QueryBalances: Query thành công, ResultCount={0}", result.Count);
            return result.Select(x => x.ToDto()).ToList();
        }
        catch (Exception ex)
        {
            _logger.Error($"QueryBalances: Lỗi query tồn kho: {ex.Message}", ex);
            throw new DataAccessException("Lỗi query tồn kho", ex);
        }
    }

    /// <summary>
    /// Lấy danh sách tồn kho cần xác thực (IsVerified = false)
    /// </summary>
    /// <returns>Danh sách tồn kho cần xác thực</returns>
    public List<InventoryBalanceDto> GetUnverifiedBalances()
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

            LoadNavigationProperties(context, balances);

            _logger.Info("GetUnverifiedBalances: Lấy được {0} tồn kho cần xác thực", balances.Count);
            return balances.Select(x => x.ToDto()).ToList();
        }
        catch (Exception ex)
        {
            _logger.Error($"GetUnverifiedBalances: Lỗi lấy danh sách tồn kho cần xác thực: {ex.Message}", ex);
            throw new DataAccessException("Lỗi lấy danh sách tồn kho cần xác thực", ex);
        }
    }

    /// <summary>
    /// Lấy danh sách tồn kho cần phê duyệt (IsVerified = true, IsApproved = false)
    /// </summary>
    /// <returns>Danh sách tồn kho cần phê duyệt</returns>
    public List<InventoryBalanceDto> GetUnapprovedBalances()
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

            LoadNavigationProperties(context, balances);

            _logger.Info("GetUnapprovedBalances: Lấy được {0} tồn kho cần phê duyệt", balances.Count);
            return balances.Select(x => x.ToDto()).ToList();
        }
        catch (Exception ex)
        {
            _logger.Error($"GetUnapprovedBalances: Lỗi lấy danh sách tồn kho cần phê duyệt: {ex.Message}", ex);
            throw new DataAccessException("Lỗi lấy danh sách tồn kho cần phê duyệt", ex);
        }
    }

    #endregion

    #region Update

    /// <summary>
    /// Cập nhật tồn kho
    /// </summary>
    /// <param name="inventoryBalance">DTO tồn kho cần cập nhật</param>
    public void Update(InventoryBalanceDto inventoryBalance)
    {
        if (inventoryBalance == null)
            throw new ArgumentNullException(nameof(inventoryBalance));

        using var context = CreateNewContext();
        try
        {
            _logger.Debug("Update: Bắt đầu cập nhật tồn kho, Id={0}", inventoryBalance.Id);

            var existing = context.InventoryBalances.FirstOrDefault(x => x.Id == inventoryBalance.Id);
            if (existing == null)
            {
                _logger.Warning("Update: Không tìm thấy tồn kho với Id={0}", inventoryBalance.Id);
                throw new DataAccessException("Không tìm thấy tồn kho để cập nhật");
            }

            // Sử dụng converter để cập nhật entity từ DTO
            inventoryBalance.ToEntity(existing);
            existing.ModifiedDate = DateTime.Now;

            context.SubmitChanges();

            _logger.Info("Update: Đã cập nhật tồn kho, Id={0}", existing.Id);
        }
        catch (DataAccessException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.Error($"Update: Lỗi cập nhật tồn kho: {ex.Message}", ex);
            throw new DataAccessException("Lỗi cập nhật tồn kho", ex);
        }
    }

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
                throw new DataAccessException($"Không tìm thấy tồn kho với ID: {id}");
            }

            balance.IsLocked = isLocked;
            balance.LockedBy = lockedBy;
            balance.LockedDate = isLocked ? DateTime.Now : (DateTime?)null;
            balance.LockReason = lockReason;
            balance.Status = isLocked ? 1 : 0;
            balance.ModifiedDate = DateTime.Now;
            balance.ModifiedBy = lockedBy;

            context.SubmitChanges();

            _logger.Info("UpdateLockStatus: Đã cập nhật trạng thái khóa, Id={0}, IsLocked={1}", id, isLocked);
        }
        catch (DataAccessException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.Error($"UpdateLockStatus: Lỗi cập nhật trạng thái khóa: {ex.Message}", ex);
            throw new DataAccessException("Lỗi cập nhật trạng thái khóa", ex);
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
                throw new DataAccessException($"Không tìm thấy tồn kho với ID: {id}");
            }

            balance.IsVerified = isVerified;
            balance.VerifiedBy = verifiedBy;
            balance.VerifiedDate = isVerified ? DateTime.Now : (DateTime?)null;
            balance.VerificationNotes = verificationNotes;
            balance.Status = isVerified ? 2 : (balance.IsLocked ? 1 : 0);
            balance.ModifiedDate = DateTime.Now;
            balance.ModifiedBy = verifiedBy;

            context.SubmitChanges();

            _logger.Info("UpdateVerificationStatus: Đã cập nhật trạng thái xác thực, Id={0}, IsVerified={1}", id, isVerified);
        }
        catch (DataAccessException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.Error($"UpdateVerificationStatus: Lỗi cập nhật trạng thái xác thực: {ex.Message}", ex);
            throw new DataAccessException("Lỗi cập nhật trạng thái xác thực", ex);
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
                throw new DataAccessException($"Không tìm thấy tồn kho với ID: {id}");
            }

            balance.IsApproved = isApproved;
            balance.ApprovedBy = approvedBy;
            balance.ApprovedDate = isApproved ? DateTime.Now : (DateTime?)null;
            balance.ApprovalNotes = approvalNotes;
            balance.Status = isApproved ? 3 : (balance.IsVerified ? 2 : (balance.IsLocked ? 1 : 0));
            balance.ModifiedDate = DateTime.Now;
            balance.ModifiedBy = approvedBy;

            context.SubmitChanges();

            _logger.Info("UpdateApprovalStatus: Đã cập nhật trạng thái phê duyệt, Id={0}, IsApproved={1}", id, isApproved);
        }
        catch (DataAccessException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.Error($"UpdateApprovalStatus: Lỗi cập nhật trạng thái phê duyệt: {ex.Message}", ex);
            throw new DataAccessException("Lỗi cập nhật trạng thái phê duyệt", ex);
        }
    }

    #endregion

    #region Delete

    /// <summary>
    /// Xóa tồn kho theo ID
    /// </summary>
    /// <param name="id">ID tồn kho cần xóa</param>
    public void Delete(Guid id)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("Delete: Bắt đầu xóa tồn kho, Id={0}", id);

            var balance = context.InventoryBalances.FirstOrDefault(x => x.Id == id);
            if (balance == null)
            {
                _logger.Warning("Delete: Không tìm thấy tồn kho với Id={0}", id);
                throw new DataAccessException($"Không tìm thấy tồn kho với ID: {id}");
            }

            context.InventoryBalances.DeleteOnSubmit(balance);
            context.SubmitChanges();

            _logger.Info("Delete: Đã xóa tồn kho, Id={0}", id);
        }
        catch (DataAccessException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.Error($"Delete: Lỗi xóa tồn kho: {ex.Message}", ex);
            throw new DataAccessException("Lỗi xóa tồn kho", ex);
        }
    }

    #endregion

    #region Business Operations

    /// <summary>
    /// Tính lại tổng kết tồn kho cho kỳ được chỉ định
    /// Tính toán từ StockInOutDetail và cập nhật lại InventoryBalance
    /// Đảm bảo các mặt hàng của tháng trước nếu không có nhập xuất thì vẫn tồn tại trong tháng này
    /// </summary>
    /// <param name="periodYear">Năm kỳ</param>
    /// <param name="periodMonth">Tháng kỳ (1-12)</param>
    /// <returns>Số lượng tồn kho đã được cập nhật</returns>
    public int RecalculateSummary(int periodYear, int periodMonth)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Info("RecalculateSummary: Bắt đầu tính lại tổng kết cho kỳ {0}/{1:D2}", periodYear, periodMonth);

            if (periodYear < 2000 || periodYear > 9999)
                throw new ArgumentException($"PeriodYear phải trong khoảng 2000-9999, giá trị hiện tại: {periodYear}");
            
            if (periodMonth < 1 || periodMonth > 12)
                throw new ArgumentException($"PeriodMonth phải trong khoảng 1-12, giá trị hiện tại: {periodMonth}");

            var previousPeriodMonth = periodMonth == 1 ? 12 : periodMonth - 1;
            var previousPeriodYear = periodMonth == 1 ? periodYear - 1 : periodYear;

            var periodStartDate = new DateTime(periodYear, periodMonth, 1);
            var periodEndDate = periodStartDate.AddMonths(1).AddDays(-1);

            // Kiểm tra và xóa dữ liệu cũ
            var currentBalances = context.InventoryBalances
                .Where(b => b.PeriodYear == periodYear && b.PeriodMonth == periodMonth && !b.IsDeleted)
                .ToList();

            var lockedBalances = currentBalances.Where(b => b.IsLocked).ToList();
            if (lockedBalances.Any())
            {
                var lockedCount = lockedBalances.Count;
                _logger.Warning("RecalculateSummary: Có {0} tồn kho đã bị khóa, không thể tính lại tổng kết", lockedCount);
                throw new InvalidOperationException(
                    $"Không thể tính lại tổng kết vì có {lockedCount} tồn kho đã bị khóa trong kỳ {periodYear}/{periodMonth:D2}. " +
                    "Vui lòng mở khóa các tồn kho này trước khi thực hiện tổng kết.");
            }

            foreach (var balance in currentBalances)
            {
                context.InventoryBalances.DeleteOnSubmit(balance);
            }
            context.SubmitChanges();

            // Lấy danh sách các mặt hàng trong kỳ trước
            var previousBalances = context.InventoryBalances
                .Where(b => b.PeriodYear == previousPeriodYear 
                         && b.PeriodMonth == previousPeriodMonth 
                         && !b.IsDeleted)
                .Select(b => new { b.WarehouseId, b.ProductVariantId, b.ClosingBalance, b.ClosingValue })
                .ToList();

            // Lấy danh sách các mặt hàng phát sinh nhập xuất trong kỳ này
            var stockInOutItems = context.StockInOutDetails
                .Where(d => d.StockInOutMaster.StockInOutDate >= periodStartDate
                         && d.StockInOutMaster.StockInOutDate <= periodEndDate)
                .Select(d => new { WarehouseId = d.StockInOutMaster.WarehouseId, ProductVariantId = d.ProductVariantId })
                .Distinct()
                .ToList();

            // Gộp lại thành tổng danh sách các mặt hàng để tính tổng kết
            var allProductKeys = previousBalances
                .Select(b => new { b.WarehouseId, b.ProductVariantId })
                .Union(stockInOutItems)
                .Distinct()
                .ToList();

            int createdCount = 0;

            // Tính tổng kết cho từng mặt hàng
            foreach (var productKey in allProductKeys)
            {
                try
                {
                    var previousBalance = previousBalances
                        .FirstOrDefault(b => b.WarehouseId == productKey.WarehouseId 
                                          && b.ProductVariantId == productKey.ProductVariantId);

                    decimal openingBalance = previousBalance?.ClosingBalance ?? 0;
                    decimal? openingValue = previousBalance?.ClosingValue;

                    var stockInOutDetails = context.StockInOutDetails
                        .Where(d => d.StockInOutMaster.StockInOutDate >= periodStartDate
                                 && d.StockInOutMaster.StockInOutDate <= periodEndDate
                                 && d.StockInOutMaster.WarehouseId == productKey.WarehouseId
                                 && d.ProductVariantId == productKey.ProductVariantId)
                        .ToList();

                    var totalInQty = stockInOutDetails.Sum(d => d.StockInQty);
                    var totalOutQty = stockInOutDetails.Sum(d => d.StockOutQty);

                    var totalInValue = stockInOutDetails
                        .Sum(d => (decimal?)(d.UnitPrice * d.StockInQty));

                    var totalOutValue = stockInOutDetails
                        .Sum(d => (decimal?)(d.UnitPrice * d.StockOutQty));

                    var totalInVatAmount = stockInOutDetails
                        .Where(d => d.StockInQty > 0)
                        .Sum(d => (decimal?)d.VatAmount);

                    var totalOutVatAmount = stockInOutDetails
                        .Where(d => d.StockOutQty > 0)
                        .Sum(d => (decimal?)d.VatAmount);

                    var totalInAmountIncludedVat = totalInValue.HasValue && totalInVatAmount.HasValue
                        ? totalInValue.Value + totalInVatAmount.Value
                        : (decimal?)null;

                    var totalOutAmountIncludedVat = totalOutValue.HasValue && totalOutVatAmount.HasValue
                        ? totalOutValue.Value + totalOutVatAmount.Value
                        : (decimal?)null;

                    var closingBalance = openingBalance + totalInQty - totalOutQty;
                    var closingValue = (openingValue ?? 0) + (totalInValue ?? 0) - (totalOutValue ?? 0);

                    if (openingBalance == 0 && totalInQty == 0 && totalOutQty == 0)
                        continue;

                    var newBalance = new InventoryBalance
                    {
                        Id = Guid.NewGuid(),
                        WarehouseId = productKey.WarehouseId,
                        ProductVariantId = productKey.ProductVariantId,
                        PeriodYear = periodYear,
                        PeriodMonth = periodMonth,
                        OpeningBalance = openingBalance,
                        OpeningValue = openingValue,
                        TotalInQty = totalInQty,
                        TotalOutQty = totalOutQty,
                        ClosingBalance = closingBalance,
                        TotalInValue = totalInValue,
                        TotalOutValue = totalOutValue,
                        ClosingValue = closingValue,
                        TotalInVatAmount = totalInVatAmount,
                        TotalOutVatAmount = totalOutVatAmount,
                        TotalInAmountIncludedVat = totalInAmountIncludedVat,
                        TotalOutAmountIncludedVat = totalOutAmountIncludedVat,
                        CreateDate = DateTime.Now,
                        IsActive = true,
                        IsDeleted = false,
                        Status = 0
                    };

                    context.InventoryBalances.InsertOnSubmit(newBalance);
                    createdCount++;
                }
                catch (Exception ex)
                {
                    _logger.Error($"RecalculateSummary: Lỗi khi tính lại tổng kết cho tồn kho WarehouseId={productKey.WarehouseId}, ProductVariantId={productKey.ProductVariantId}: {ex.Message}", ex);
                }
            }

            context.SubmitChanges();

            _logger.Info("RecalculateSummary: Đã tính lại tổng kết cho {0} tồn kho trong kỳ {1}/{2:D2}", createdCount, periodYear, periodMonth);
            return createdCount;
        }
        catch (Exception ex)
        {
            _logger.Error($"RecalculateSummary: Lỗi tính lại tổng kết cho kỳ {periodYear}/{periodMonth:D2}: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Kết chuyển dữ liệu tồn kho từ kỳ hiện tại sang kỳ tiếp theo
    /// Tạo tồn kho mới cho kỳ tiếp theo với OpeningBalance = ClosingBalance của kỳ hiện tại
    /// </summary>
    /// <param name="fromPeriodYear">Năm kỳ nguồn</param>
    /// <param name="fromPeriodMonth">Tháng kỳ nguồn (1-12)</param>
    /// <param name="overwriteExisting">Nếu true, ghi đè dữ liệu đã tồn tại ở kỳ đích. Nếu false, báo lỗi nếu đã có dữ liệu</param>
    /// <returns>Số lượng tồn kho đã được kết chuyển</returns>
    public int ForwardBalance(int fromPeriodYear, int fromPeriodMonth, bool overwriteExisting = false)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Info("ForwardBalance: Bắt đầu kết chuyển tồn kho từ kỳ {0}/{1:D2}", fromPeriodYear, fromPeriodMonth);

            if (fromPeriodYear < 2000 || fromPeriodYear > 9999)
                throw new ArgumentException($"PeriodYear phải trong khoảng 2000-9999, giá trị hiện tại: {fromPeriodYear}");
            
            if (fromPeriodMonth < 1 || fromPeriodMonth > 12)
                throw new ArgumentException($"PeriodMonth phải trong khoảng 1-12, giá trị hiện tại: {fromPeriodMonth}");

            var toPeriodMonth = fromPeriodMonth == 12 ? 1 : fromPeriodMonth + 1;
            var toPeriodYear = fromPeriodMonth == 12 ? fromPeriodYear + 1 : fromPeriodYear;

            var sourceBalances = context.InventoryBalances
                .Where(b => b.PeriodYear == fromPeriodYear 
                         && b.PeriodMonth == fromPeriodMonth 
                         && !b.IsDeleted)
                .ToList();

            if (!sourceBalances.Any())
            {
                _logger.Warning("ForwardBalance: Không có dữ liệu tồn kho trong kỳ {0}/{1:D2} để kết chuyển", fromPeriodYear, fromPeriodMonth);
                throw new InvalidOperationException(
                    $"Không có dữ liệu tồn kho trong kỳ {fromPeriodYear}/{fromPeriodMonth:D2} để kết chuyển.");
            }

            var unlockedBalances = sourceBalances.Where(b => !b.IsLocked).ToList();
            if (unlockedBalances.Any())
            {
                var unlockedCount = unlockedBalances.Count;
                _logger.Warning("ForwardBalance: Có {0} tồn kho chưa được khóa trong kỳ {1}/{2:D2}", unlockedCount, fromPeriodYear, fromPeriodMonth);
                throw new InvalidOperationException(
                    $"Không thể kết chuyển vì có {unlockedCount} tồn kho chưa được khóa trong kỳ {fromPeriodYear}/{fromPeriodMonth:D2}. " +
                    "Vui lòng khóa tất cả tồn kho trước khi thực hiện kết chuyển.");
            }

            var existingTargetBalances = context.InventoryBalances
                .Where(b => b.PeriodYear == toPeriodYear 
                         && b.PeriodMonth == toPeriodMonth 
                         && !b.IsDeleted)
                .ToList();

            if (existingTargetBalances.Any())
            {
                if (!overwriteExisting)
                {
                    var existingCount = existingTargetBalances.Count;
                    _logger.Warning("ForwardBalance: Kỳ đích {0}/{1:D2} đã có {2} tồn kho", toPeriodYear, toPeriodMonth, existingCount);
                    throw new InvalidOperationException(
                        $"Kỳ đích {toPeriodYear}/{toPeriodMonth:D2} đã có {existingCount} tồn kho. " +
                        "Vui lòng xóa dữ liệu cũ hoặc chọn ghi đè.");
                }
                else
                {
                    foreach (var existing in existingTargetBalances)
                    {
                        context.InventoryBalances.DeleteOnSubmit(existing);
                    }
                    context.SubmitChanges();
                    _logger.Info("ForwardBalance: Đã xóa {0} tồn kho cũ trong kỳ {1}/{2:D2}", existingTargetBalances.Count, toPeriodYear, toPeriodMonth);
                }
            }

            int forwardedCount = 0;

            foreach (var sourceBalance in sourceBalances)
            {
                try
                {
                    if (sourceBalance.ClosingBalance == 0 && 
                        (!sourceBalance.ClosingValue.HasValue || sourceBalance.ClosingValue.Value == 0))
                    {
                        continue;
                    }

                    var newBalance = new InventoryBalance
                    {
                        Id = Guid.NewGuid(),
                        WarehouseId = sourceBalance.WarehouseId,
                        ProductVariantId = sourceBalance.ProductVariantId,
                        PeriodYear = toPeriodYear,
                        PeriodMonth = toPeriodMonth,
                        OpeningBalance = sourceBalance.ClosingBalance,
                        OpeningValue = sourceBalance.ClosingValue,
                        TotalInQty = 0,
                        TotalOutQty = 0,
                        TotalInValue = null,
                        TotalOutValue = null,
                        TotalInVatAmount = null,
                        TotalOutVatAmount = null,
                        TotalInAmountIncludedVat = null,
                        TotalOutAmountIncludedVat = null,
                        ClosingBalance = sourceBalance.ClosingBalance,
                        ClosingValue = sourceBalance.ClosingValue,
                        IsLocked = false,
                        LockedDate = null,
                        LockedBy = null,
                        LockReason = null,
                        IsVerified = false,
                        VerifiedDate = null,
                        VerifiedBy = null,
                        VerificationNotes = null,
                        IsApproved = false,
                        ApprovedDate = null,
                        ApprovedBy = null,
                        ApprovalNotes = null,
                        Status = 0,
                        Notes = $"Kết chuyển từ kỳ {fromPeriodYear}/{fromPeriodMonth:D2}",
                        IsActive = true,
                        IsDeleted = false,
                        CreateDate = DateTime.Now,
                        ModifiedDate = null,
                        ModifiedBy = null,
                        DeletedDate = null,
                        DeletedBy = null
                    };

                    context.InventoryBalances.InsertOnSubmit(newBalance);
                    forwardedCount++;
                }
                catch (Exception ex)
                {
                    _logger.Error($"ForwardBalance: Lỗi khi kết chuyển tồn kho WarehouseId={sourceBalance.WarehouseId}, ProductVariantId={sourceBalance.ProductVariantId}: {ex.Message}", ex);
                }
            }

            context.SubmitChanges();
            _logger.Info("ForwardBalance: Đã kết chuyển {0} tồn kho từ kỳ {1}/{2:D2} sang kỳ {3}/{4:D2}", 
                forwardedCount, fromPeriodYear, fromPeriodMonth, toPeriodYear, toPeriodMonth);
            
            return forwardedCount;
        }
        catch (Exception ex)
        {
            _logger.Error($"ForwardBalance: Lỗi kết chuyển tồn kho từ kỳ {fromPeriodYear}/{fromPeriodMonth:D2}: {ex.Message}", ex);
            throw;
        }
    }

    #endregion
}
