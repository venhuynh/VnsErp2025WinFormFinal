using Dal.Connection;
using Dal.DataAccess.Implementations.Inventory.Assembly;
using Dal.DataAccess.Implementations.Inventory.InventoryManagement;
using Dal.DataAccess.Implementations.Inventory.StockIn;
using Dal.DataAccess.Interfaces.Inventory.Assembly;
using Dal.DataAccess.Interfaces.Inventory.InventoryManagement;
using Dal.DataAccess.Interfaces.Inventory.StockIn;
using Dal.DataContext;
using DTO.Inventory.Assembly;
using DTO.Inventory.StockIn;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bll.Inventory.Assembly;

/// <summary>
/// Business Logic Layer cho Assembly (Lắp ráp sản phẩm)
/// </summary>
public class AssemblyBll
{
    #region Fields

    private IProductBOMRepository _bomRepository;
    private IAssemblyTransactionRepository _assemblyTransactionRepository;
    private IStockInRepository _stockInRepository;
    private IInventoryBalanceRepository _inventoryBalanceRepository;
    private readonly ILogger _logger;
    private readonly object _lockObject = new object();

    #endregion

    #region Constructors

    public AssemblyBll()
    {
        _logger = LoggerFactory.CreateLogger(LogCategory.BLL);
    }

    #endregion

    #region Helper Methods

    private IProductBOMRepository GetBOMRepository()
    {
        if (_bomRepository == null)
        {
            lock (_lockObject)
            {
                if (_bomRepository == null)
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

                        _bomRepository = new ProductBOMRepository(globalConnectionString);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"Lỗi khởi tạo ProductBOMRepository: {ex.Message}", ex);
                        throw;
                    }
                }
            }
        }
        return _bomRepository;
    }

    private IAssemblyTransactionRepository GetAssemblyTransactionRepository()
    {
        if (_assemblyTransactionRepository == null)
        {
            lock (_lockObject)
            {
                if (_assemblyTransactionRepository == null)
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

                        _assemblyTransactionRepository = new AssemblyTransactionRepository(globalConnectionString);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"Lỗi khởi tạo AssemblyTransactionRepository: {ex.Message}", ex);
                        throw;
                    }
                }
            }
        }
        return _assemblyTransactionRepository;
    }

    private IStockInRepository GetStockInRepository()
    {
        if (_stockInRepository == null)
        {
            lock (_lockObject)
            {
                if (_stockInRepository == null)
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

    private IInventoryBalanceRepository GetInventoryBalanceRepository()
    {
        if (_inventoryBalanceRepository == null)
        {
            lock (_lockObject)
            {
                if (_inventoryBalanceRepository == null)
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

                        _inventoryBalanceRepository = new InventoryBalanceRepository(globalConnectionString);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"Lỗi khởi tạo InventoryBalanceRepository: {ex.Message}", ex);
                        throw;
                    }
                }
            }
        }
        return _inventoryBalanceRepository;
    }

    #endregion

    #region Assembly Operations

    /// <summary>
    /// Thực hiện lắp ráp sản phẩm
    /// Tự động tạo phiếu xuất linh kiện và phiếu nhập sản phẩm hoàn chỉnh
    /// </summary>
    public async Task<AssemblyTransactionDto> AssembleProductAsync(AssemblyRequestDto request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        if (!request.IsValid())
            throw new ArgumentException("Dữ liệu yêu cầu lắp ráp không hợp lệ", nameof(request));

        try
        {
            _logger.Info("AssembleProductAsync: Bắt đầu lắp ráp sản phẩm, ProductVariantId={0}, Quantity={1}, WarehouseId={2}",
                request.ProductVariantId, request.Quantity, request.WarehouseId);

            // 1. Load BOM từ ProductVariantId
            var bomList = GetBOMRepository().GetByProductVariantId(request.ProductVariantId);
            if (bomList == null || bomList.Count == 0)
            {
                throw new InvalidOperationException(
                    $"Không tìm thấy BOM cho sản phẩm. Vui lòng định nghĩa BOM trước khi lắp ráp.");
            }

            // 2. Validate đủ linh kiện trong kho
            ValidateComponentAvailability(bomList, request.Quantity, request.WarehouseId);

            // 3. Tính giá thành từ linh kiện
            var costInfo = CalculateAssemblyCost(bomList, request.Quantity, request.WarehouseId);

            // 4. Tạo phiếu xuất linh kiện và phiếu nhập sản phẩm trong 1 transaction
            var transaction = await CreateAssemblyTransactionAsync(request, bomList, costInfo);

            _logger.Info("AssembleProductAsync: Lắp ráp thành công, TransactionId={0}", transaction.Id);
            return transaction;
        }
        catch (Exception ex)
        {
            _logger.Error($"AssembleProductAsync: Lỗi lắp ráp sản phẩm: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Tính giá thành lắp ráp từ linh kiện
    /// </summary>
    public AssemblyCostInfo CalculateAssemblyCost(
        List<ProductBOM> bomList, 
        decimal quantity, 
        Guid warehouseId)
    {
        if (bomList == null || bomList.Count == 0)
            throw new ArgumentException("Danh sách BOM không được để trống", nameof(bomList));

        if (quantity <= 0)
            throw new ArgumentException("Số lượng phải lớn hơn 0", nameof(quantity));

        try
        {
            _logger.Debug("CalculateAssemblyCost: Tính giá thành, BOMCount={0}, Quantity={1}, WarehouseId={2}",
                bomList.Count, quantity, warehouseId);

            decimal totalCost = 0;
            var componentCosts = new List<ComponentCostInfo>();

            // Lấy giá linh kiện từ tồn kho hoặc giá mua gần nhất
            foreach (var bom in bomList.Where(b => b.IsActive))
            {
                var componentQty = bom.Quantity * quantity;
                var unitPrice = GetComponentUnitPrice(bom.ComponentVariantId, warehouseId);
                var componentCost = unitPrice * componentQty;

                totalCost += componentCost;

                componentCosts.Add(new ComponentCostInfo
                {
                    ComponentVariantId = bom.ComponentVariantId,
                    ComponentName = bom.ProductVariant?.VariantFullName ?? bom.ProductVariant?.ProductService?.Name ?? "N/A",
                    Quantity = componentQty,
                    UnitPrice = unitPrice,
                    TotalCost = componentCost
                });
            }

            var unitCost = quantity > 0 ? totalCost / quantity : 0;

            _logger.Info("CalculateAssemblyCost: Tổng giá thành={0}, Giá đơn vị={1}", totalCost, unitCost);

            return new AssemblyCostInfo
            {
                TotalCost = totalCost,
                UnitCost = unitCost,
                ComponentCosts = componentCosts
            };
        }
        catch (Exception ex)
        {
            _logger.Error($"CalculateAssemblyCost: Lỗi tính giá thành: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Lấy đơn giá linh kiện từ tồn kho hoặc giá mua gần nhất
    /// </summary>
    private decimal GetComponentUnitPrice(Guid componentVariantId, Guid warehouseId)
    {
        try
        {
            // Ưu tiên 1: Lấy từ InventoryBalance (giá trung bình)
            var currentDate = DateTime.Now;
            var balance = GetInventoryBalanceRepository().GetByPeriod(
                warehouseId, componentVariantId, currentDate.Year, currentDate.Month);

            if (balance != null && balance.ClosingBalance > 0 && balance.ClosingValue.HasValue)
            {
                var avgPrice = balance.ClosingValue.Value / balance.ClosingBalance;
                _logger.Debug("GetComponentUnitPrice: Lấy giá từ InventoryBalance, ComponentVariantId={0}, Price={1}",
                    componentVariantId, avgPrice);
                return avgPrice;
            }

            // Ưu tiên 2: Lấy giá mua gần nhất từ StockInOutDetail
            var connectionString = ApplicationStartupManager.Instance.GetGlobalConnectionString();
            using var context = new VnsErp2025DataContext(connectionString);
            
            var recentStockIn = context.StockInOutDetails
                .Where(d => d.ProductVariantId == componentVariantId
                    && d.StockInOutMaster.WarehouseId == warehouseId
                    && d.StockInQty > 0
                    && d.UnitPrice > 0)
                .OrderByDescending(d => d.StockInOutMaster.StockInOutDate)
                .FirstOrDefault();

            if (recentStockIn != null)
            {
                _logger.Debug("GetComponentUnitPrice: Lấy giá từ StockInOutDetail gần nhất, ComponentVariantId={0}, Price={1}",
                    componentVariantId, recentStockIn.UnitPrice);
                return recentStockIn.UnitPrice;
            }

            // Nếu không tìm thấy, trả về 0 (có thể cần nhập giá thủ công)
            _logger.Warning("GetComponentUnitPrice: Không tìm thấy giá cho ComponentVariantId={0}, trả về 0", componentVariantId);
            return 0;
        }
        catch (Exception ex)
        {
            _logger.Error($"GetComponentUnitPrice: Lỗi lấy giá linh kiện: {ex.Message}", ex);
            return 0; // Trả về 0 nếu có lỗi, có thể cần nhập giá thủ công
        }
    }

    /// <summary>
    /// Validate đủ linh kiện trong kho
    /// </summary>
    private void ValidateComponentAvailability(
        List<ProductBOM> bomList, 
        decimal quantity, 
        Guid warehouseId)
    {
        var currentDate = DateTime.Now;
        var errors = new List<string>();

        foreach (var bom in bomList.Where(b => b.IsActive))
        {
            var requiredQty = bom.Quantity * quantity;

            // Lấy tồn kho hiện tại
            var balance = GetInventoryBalanceRepository().GetByPeriod(
                warehouseId, bom.ComponentVariantId, currentDate.Year, currentDate.Month);

            var availableQty = balance?.ClosingBalance ?? 0;

            if (availableQty < requiredQty)
            {
                var componentName = bom.ProductVariant?.VariantFullName 
                    ?? bom.ProductVariant?.ProductService?.Name 
                    ?? bom.ComponentVariantId.ToString();
                
                errors.Add($"Linh kiện '{componentName}': Cần {requiredQty}, Tồn kho: {availableQty}");
            }
        }

        if (errors.Any())
        {
            var errorMessage = "Không đủ linh kiện trong kho:\n" + string.Join("\n", errors);
            _logger.Warning("ValidateComponentAvailability: {0}", errorMessage);
            throw new InvalidOperationException(errorMessage);
        }
    }

    /// <summary>
    /// Tạo giao dịch lắp ráp (phiếu xuất linh kiện + phiếu nhập sản phẩm)
    /// </summary>
    private async Task<AssemblyTransactionDto> CreateAssemblyTransactionAsync(
        AssemblyRequestDto request,
        List<ProductBOM> bomList,
        AssemblyCostInfo costInfo)
    {
        var connectionString = ApplicationStartupManager.Instance.GetGlobalConnectionString();
        using var context = new VnsErp2025DataContext(connectionString);

        try
        {
            context.Connection.Open();
            using var transaction = context.Connection.BeginTransaction();
            context.Transaction = transaction;

            try
            {
                // 1. Tạo phiếu xuất linh kiện
                var stockOutMaster = CreateStockOutMaster(request);
                var stockOutDetails = CreateStockOutDetails(bomList, request.Quantity, stockOutMaster.Id, request.WarehouseId);
                
                // Tính tổng cho master
                stockOutMaster.TotalQuantity = stockOutDetails.Sum(d => d.StockOutQty);
                stockOutMaster.TotalAmount = stockOutDetails.Sum(d => d.TotalAmount);
                stockOutMaster.TotalAmountIncludedVat = stockOutMaster.TotalAmount;
                
                // Lưu phiếu xuất
                var stockOutMasterId = await GetStockInRepository().SaveAsync(stockOutMaster, stockOutDetails);
                stockOutMaster.Id = stockOutMasterId;

                // 2. Tạo phiếu nhập sản phẩm hoàn chỉnh
                var stockInMaster = CreateStockInMaster(request, costInfo);
                var stockInDetails = CreateStockInDetails(request, stockInMaster.Id, costInfo);
                
                // Lưu phiếu nhập
                var stockInMasterId = await GetStockInRepository().SaveAsync(stockInMaster, stockInDetails);
                stockInMaster.Id = stockInMasterId;

                // 3. Lưu AssemblyTransaction
                var assemblyTransaction = new AssemblyTransaction
                {
                    Id = Guid.NewGuid(),
                    AssemblyDate = request.AssemblyDate,
                    ProductVariantId = request.ProductVariantId,
                    Quantity = request.Quantity,
                    StockOutMasterId = stockOutMasterId,
                    StockInMasterId = stockInMasterId,
                    WarehouseId = request.WarehouseId,
                    TotalCost = costInfo.TotalCost,
                    UnitCost = costInfo.UnitCost,
                    Notes = request.Notes,
                    CreatedDate = DateTime.Now
                };

                context.AssemblyTransactions.InsertOnSubmit(assemblyTransaction);
                context.SubmitChanges();

                // 4. Commit transaction
                transaction.Commit();

                _logger.Info("CreateAssemblyTransactionAsync: Đã tạo giao dịch lắp ráp, TransactionId={0}, StockOutId={1}, StockInId={2}",
                    assemblyTransaction.Id, stockOutMasterId, stockInMasterId);

                return MapEntityToDto(assemblyTransaction);
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
        catch (Exception ex)
        {
            _logger.Error($"CreateAssemblyTransactionAsync: Lỗi tạo giao dịch lắp ráp: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Tạo phiếu xuất linh kiện
    /// </summary>
    private StockInOutMaster CreateStockOutMaster(AssemblyRequestDto request)
    {
        var stockOutMasterBll = new InventoryManagement.StockInOutMasterBll();
        var voucherNumber = stockOutMasterBll.GenerateVoucherNumber(
            request.AssemblyDate, 
            LoaiNhapXuatKhoEnum.XuatLinhKienLapRap);

        return new StockInOutMaster
        {
            Id = Guid.NewGuid(),
            StockInOutDate = request.AssemblyDate,
            VocherNumber = voucherNumber,
            StockInOutType = (int)LoaiNhapXuatKhoEnum.XuatLinhKienLapRap,
            VoucherStatus = (int)TrangThaiPhieuNhapEnum.DaNhapKho, // Đã nhập kho = đã xuất
            WarehouseId = request.WarehouseId,
            Notes = $"Xuất linh kiện để lắp ráp: {request.Notes ?? string.Empty}",
            TotalQuantity = 0, // Sẽ tính sau
            TotalAmount = 0,
            TotalVat = 0,
            TotalAmountIncludedVat = 0,
            CreatedDate = DateTime.Now,
            UpdatedDate = DateTime.Now
        };
    }

    /// <summary>
    /// Tạo chi tiết phiếu xuất linh kiện
    /// </summary>
    private List<StockInOutDetail> CreateStockOutDetails(
        List<ProductBOM> bomList, 
        decimal quantity, 
        Guid stockOutMasterId,
        Guid warehouseId)
    {
        var details = new List<StockInOutDetail>();
        decimal totalQty = 0;
        decimal totalAmount = 0;

        foreach (var bom in bomList.Where(b => b.IsActive))
        {
            var componentQty = bom.Quantity * quantity;
            var unitPrice = GetComponentUnitPrice(bom.ComponentVariantId, warehouseId);
            var totalPrice = unitPrice * componentQty;

            var detail = new StockInOutDetail
            {
                Id = Guid.NewGuid(),
                StockInOutMasterId = stockOutMasterId,
                ProductVariantId = bom.ComponentVariantId,
                StockInQty = 0,
                StockOutQty = componentQty,
                UnitPrice = unitPrice,
                Vat = 0, // Xuất linh kiện nội bộ không có VAT
                VatAmount = 0,
                TotalAmount = totalPrice,
                TotalAmountIncludedVat = totalPrice,
                GhiChu = $"Linh kiện lắp ráp: {bom.Notes ?? string.Empty}"
            };

            details.Add(detail);
            totalQty += componentQty;
            totalAmount += totalPrice;
        }

        return details;
    }

    /// <summary>
    /// Tạo phiếu nhập sản phẩm hoàn chỉnh
    /// </summary>
    private StockInOutMaster CreateStockInMaster(AssemblyRequestDto request, AssemblyCostInfo costInfo)
    {
        var stockInMasterBll = new InventoryManagement.StockInOutMasterBll();
        var voucherNumber = stockInMasterBll.GenerateVoucherNumber(
            request.AssemblyDate, 
            LoaiNhapXuatKhoEnum.NhapSanPhamLapRap);

        return new StockInOutMaster
        {
            Id = Guid.NewGuid(),
            StockInOutDate = request.AssemblyDate,
            VocherNumber = voucherNumber,
            StockInOutType = (int)LoaiNhapXuatKhoEnum.NhapSanPhamLapRap,
            VoucherStatus = (int)TrangThaiPhieuNhapEnum.DaNhapKho,
            WarehouseId = request.WarehouseId,
            Notes = $"Nhập sản phẩm lắp ráp: {request.Notes ?? string.Empty}",
            TotalQuantity = request.Quantity,
            TotalAmount = costInfo.TotalCost,
            TotalVat = 0, // Nhập nội bộ không có VAT
            TotalAmountIncludedVat = costInfo.TotalCost,
            CreatedDate = DateTime.Now,
            UpdatedDate = DateTime.Now
        };
    }

    /// <summary>
    /// Tạo chi tiết phiếu nhập sản phẩm
    /// </summary>
    private List<StockInOutDetail> CreateStockInDetails(
        AssemblyRequestDto request, 
        Guid stockInMasterId, 
        AssemblyCostInfo costInfo)
    {
        var detail = new StockInOutDetail
        {
            Id = Guid.NewGuid(),
            StockInOutMasterId = stockInMasterId,
            ProductVariantId = request.ProductVariantId,
            StockInQty = request.Quantity,
            StockOutQty = 0,
            UnitPrice = costInfo.UnitCost,
            Vat = 0, // Nhập nội bộ không có VAT
            VatAmount = 0,
            TotalAmount = costInfo.TotalCost,
            TotalAmountIncludedVat = costInfo.TotalCost,
            GhiChu = $"Sản phẩm lắp ráp: {request.Notes ?? string.Empty}"
        };

        return new List<StockInOutDetail> { detail };
    }

    /// <summary>
    /// Map entity sang DTO
    /// </summary>
    private AssemblyTransactionDto MapEntityToDto(AssemblyTransaction entity)
    {
        return new AssemblyTransactionDto
        {
            Id = entity.Id,
            AssemblyDate = entity.AssemblyDate,
            ProductVariantId = entity.ProductVariantId,
            Quantity = entity.Quantity,
            StockOutMasterId = entity.StockOutMasterId,
            StockInMasterId = entity.StockInMasterId,
            WarehouseId = entity.WarehouseId,
            TotalCost = entity.TotalCost,
            UnitCost = entity.UnitCost,
            Notes = entity.Notes,
            CreatedBy = entity.CreatedBy,
            CreatedDate = entity.CreatedDate,
            // Load navigation properties if available
            ProductVariantCode = entity.ProductVariant?.VariantCode ?? string.Empty,
            ProductVariantName = entity.ProductVariant?.VariantFullName ?? entity.ProductVariant?.ProductService?.Name ?? string.Empty,
            StockOutNumber = entity.StockInOutMaster1?.VocherNumber ?? string.Empty,
            StockInNumber = entity.StockInOutMaster?.VocherNumber ?? string.Empty,
            WarehouseName = entity.CompanyBranch?.BranchName ?? string.Empty
        };
    }

    #endregion
}

/// <summary>
/// Thông tin giá thành lắp ráp
/// </summary>
public class AssemblyCostInfo
{
    public decimal TotalCost { get; set; }
    public decimal UnitCost { get; set; }
    public List<ComponentCostInfo> ComponentCosts { get; set; } = new List<ComponentCostInfo>();
}

/// <summary>
/// Thông tin giá thành từng linh kiện
/// </summary>
public class ComponentCostInfo
{
    public Guid ComponentVariantId { get; set; }
    public string ComponentName { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalCost { get; set; }
}

