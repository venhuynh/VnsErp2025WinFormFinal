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
/// Repository implementation cho Asset
/// Quản lý các thao tác CRUD với bảng Asset (Tài sản)
/// </summary>
public class AssetRepository : IAssetRepository
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
    /// Khởi tạo một instance mới của class AssetRepository.
    /// </summary>
    /// <param name="connectionString">Chuỗi kết nối cơ sở dữ liệu</param>
    /// <exception cref="ArgumentNullException">Ném ra khi connectionString là null</exception>
    public AssetRepository(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _logger = LoggerFactory.CreateLogger(LogCategory.DAL);
        _logger.Info("AssetRepository được khởi tạo với connection string");
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
        loadOptions.LoadWith<Asset>(a => a.Company);
        loadOptions.LoadWith<Asset>(a => a.CompanyBranch);
        loadOptions.LoadWith<Asset>(a => a.Department);
        loadOptions.LoadWith<Asset>(a => a.Employee);
        loadOptions.LoadWith<Asset>(a => a.ProductVariant);
        loadOptions.LoadWith<ProductVariant>(v => v.ProductService);
        loadOptions.LoadWith<ProductVariant>(v => v.UnitOfMeasure);
        loadOptions.LoadWith<Asset>(a => a.Warranty);
        loadOptions.LoadWith<Asset>(a => a.ApplicationUser); // CreateBy
        loadOptions.LoadWith<Asset>(a => a.ApplicationUser1); // DeletedBy
        loadOptions.LoadWith<Asset>(a => a.ApplicationUser2); // ModifiedBy
        context.LoadOptions = loadOptions;

        return context;
    }

    #endregion

    #region Save Operations

    /// <summary>
    /// Lưu hoặc cập nhật tài sản
    /// </summary>
    /// <param name="asset">Entity tài sản cần lưu</param>
    public void SaveOrUpdate(Asset asset)
    {
        if (asset == null)
            throw new ArgumentNullException(nameof(asset));

        using var context = CreateNewContext();
        try
        {
            _logger.Debug("SaveOrUpdate: Bắt đầu lưu tài sản, Id={0}, AssetCode={1}, AssetName={2}", 
                asset.Id, asset.AssetCode, asset.AssetName);

            var existing = asset.Id != Guid.Empty ? 
                context.Assets.FirstOrDefault(x => x.Id == asset.Id) : null;

            if (existing == null)
            {
                // Thêm mới
                if (asset.Id == Guid.Empty)
                    asset.Id = Guid.NewGuid();
                
                // Thiết lập giá trị mặc định
                if (asset.CreateDate == default(DateTime))
                    asset.CreateDate = DateTime.Now;

                context.Assets.InsertOnSubmit(asset);
                context.SubmitChanges();
                
                _logger.Info("SaveOrUpdate: Đã thêm mới tài sản, Id={0}, AssetCode={1}", asset.Id, asset.AssetCode);
            }
            else
            {
                // Cập nhật
                existing.AssetCode = asset.AssetCode;
                existing.AssetName = asset.AssetName;
                existing.AssetType = asset.AssetType;
                existing.AssetCategory = asset.AssetCategory;
                existing.Description = asset.Description;
                existing.ProductVariantId = asset.ProductVariantId;
                existing.CompanyId = asset.CompanyId;
                existing.BranchId = asset.BranchId;
                existing.DepartmentId = asset.DepartmentId;
                existing.AssignedEmployeeId = asset.AssignedEmployeeId;
                existing.Location = asset.Location;
                existing.PurchasePrice = asset.PurchasePrice;
                existing.PurchaseDate = asset.PurchaseDate;
                existing.SupplierName = asset.SupplierName;
                existing.InvoiceNumber = asset.InvoiceNumber;
                existing.InvoiceDate = asset.InvoiceDate;
                existing.DepreciationMethod = asset.DepreciationMethod;
                existing.DepreciationRate = asset.DepreciationRate;
                existing.UsefulLife = asset.UsefulLife;
                existing.DepreciationStartDate = asset.DepreciationStartDate;
                existing.AccumulatedDepreciation = asset.AccumulatedDepreciation;
                existing.CurrentValue = asset.CurrentValue;
                existing.Status = asset.Status;
                existing.Condition = asset.Condition;
                existing.IsActive = asset.IsActive;
                existing.WarrantyId = asset.WarrantyId;
                existing.WarrantyExpiryDate = asset.WarrantyExpiryDate;
                existing.SerialNumber = asset.SerialNumber;
                existing.Manufacturer = asset.Manufacturer;
                existing.Model = asset.Model;
                existing.Specifications = asset.Specifications;
                existing.Notes = asset.Notes;
                existing.ModifiedDate = DateTime.Now;
                existing.ModifiedBy = asset.ModifiedBy;
                
                context.SubmitChanges();
                
                _logger.Info("SaveOrUpdate: Đã cập nhật tài sản, Id={0}, AssetCode={1}", existing.Id, existing.AssetCode);
            }
        }
        catch (Exception ex)
        {
            _logger.Error($"SaveOrUpdate: Lỗi lưu tài sản: {ex.Message}", ex);
            throw;
        }
    }

    #endregion

    #region Query Operations

    /// <summary>
    /// Lấy tài sản theo ID
    /// </summary>
    /// <param name="id">ID tài sản</param>
    /// <returns>Asset hoặc null</returns>
    public Asset GetById(Guid id)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetById: Lấy tài sản, Id={0}", id);

            var asset = context.Assets.FirstOrDefault(x => x.Id == id && !x.IsDeleted);

            if (asset == null)
            {
                _logger.Warning("GetById: Không tìm thấy tài sản với Id={0}", id);
            }
            else
            {
                // Load navigation properties
                LoadNavigationProperties(context, new List<Asset> { asset });
                
                _logger.Info("GetById: Đã lấy tài sản, Id={0}, AssetCode={1}, AssetName={2}", 
                    id, asset.AssetCode, asset.AssetName);
            }

            return asset;
        }
        catch (Exception ex)
        {
            _logger.Error($"GetById: Lỗi lấy tài sản: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Lấy tài sản theo mã tài sản
    /// </summary>
    /// <param name="assetCode">Mã tài sản</param>
    /// <returns>Asset hoặc null</returns>
    public Asset GetByAssetCode(string assetCode)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetByAssetCode: Lấy tài sản, AssetCode={0}", assetCode);

            if (string.IsNullOrWhiteSpace(assetCode))
            {
                _logger.Warning("GetByAssetCode: AssetCode không được để trống");
                return null;
            }

            var asset = context.Assets
                .FirstOrDefault(x => x.AssetCode == assetCode && !x.IsDeleted);

            if (asset == null)
            {
                _logger.Warning("GetByAssetCode: Không tìm thấy tài sản với AssetCode={0}", assetCode);
            }
            else
            {
                // Load navigation properties
                LoadNavigationProperties(context, new List<Asset> { asset });
                
                _logger.Info("GetByAssetCode: Đã lấy tài sản, Id={0}, AssetCode={1}", asset.Id, asset.AssetCode);
            }

            return asset;
        }
        catch (Exception ex)
        {
            _logger.Error($"GetByAssetCode: Lỗi lấy tài sản: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Lấy danh sách tài sản theo công ty
    /// </summary>
    /// <param name="companyId">ID công ty</param>
    /// <returns>Danh sách tài sản</returns>
    public List<Asset> GetByCompanyId(Guid companyId)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetByCompanyId: Lấy danh sách tài sản, CompanyId={0}", companyId);

            var assets = context.Assets
                .Where(x => x.CompanyId == companyId && !x.IsDeleted)
                .OrderBy(x => x.AssetCode)
                .ThenBy(x => x.CreateDate)
                .ToList();

            // Load navigation properties
            LoadNavigationProperties(context, assets);

            _logger.Info("GetByCompanyId: Lấy được {0} tài sản", assets.Count);
            return assets;
        }
        catch (Exception ex)
        {
            _logger.Error($"GetByCompanyId: Lỗi lấy danh sách tài sản: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Lấy danh sách tài sản theo chi nhánh
    /// </summary>
    /// <param name="branchId">ID chi nhánh</param>
    /// <returns>Danh sách tài sản</returns>
    public List<Asset> GetByBranchId(Guid branchId)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetByBranchId: Lấy danh sách tài sản, BranchId={0}", branchId);

            var assets = context.Assets
                .Where(x => x.BranchId == branchId && !x.IsDeleted)
                .OrderBy(x => x.AssetCode)
                .ThenBy(x => x.CreateDate)
                .ToList();

            // Load navigation properties
            LoadNavigationProperties(context, assets);

            _logger.Info("GetByBranchId: Lấy được {0} tài sản", assets.Count);
            return assets;
        }
        catch (Exception ex)
        {
            _logger.Error($"GetByBranchId: Lỗi lấy danh sách tài sản: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Lấy danh sách tài sản theo phòng ban
    /// </summary>
    /// <param name="departmentId">ID phòng ban</param>
    /// <returns>Danh sách tài sản</returns>
    public List<Asset> GetByDepartmentId(Guid departmentId)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetByDepartmentId: Lấy danh sách tài sản, DepartmentId={0}", departmentId);

            var assets = context.Assets
                .Where(x => x.DepartmentId == departmentId && !x.IsDeleted)
                .OrderBy(x => x.AssetCode)
                .ThenBy(x => x.CreateDate)
                .ToList();

            // Load navigation properties
            LoadNavigationProperties(context, assets);

            _logger.Info("GetByDepartmentId: Lấy được {0} tài sản", assets.Count);
            return assets;
        }
        catch (Exception ex)
        {
            _logger.Error($"GetByDepartmentId: Lỗi lấy danh sách tài sản: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Lấy danh sách tài sản theo nhân viên phụ trách
    /// </summary>
    /// <param name="employeeId">ID nhân viên</param>
    /// <returns>Danh sách tài sản</returns>
    public List<Asset> GetByEmployeeId(Guid employeeId)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetByEmployeeId: Lấy danh sách tài sản, EmployeeId={0}", employeeId);

            var assets = context.Assets
                .Where(x => x.AssignedEmployeeId == employeeId && !x.IsDeleted)
                .OrderBy(x => x.AssetCode)
                .ThenBy(x => x.CreateDate)
                .ToList();

            // Load navigation properties
            LoadNavigationProperties(context, assets);

            _logger.Info("GetByEmployeeId: Lấy được {0} tài sản", assets.Count);
            return assets;
        }
        catch (Exception ex)
        {
            _logger.Error($"GetByEmployeeId: Lỗi lấy danh sách tài sản: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Lấy danh sách tài sản theo sản phẩm
    /// </summary>
    /// <param name="productVariantId">ID biến thể sản phẩm</param>
    /// <returns>Danh sách tài sản</returns>
    public List<Asset> GetByProductVariantId(Guid productVariantId)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetByProductVariantId: Lấy danh sách tài sản, ProductVariantId={0}", productVariantId);

            var assets = context.Assets
                .Where(x => x.ProductVariantId == productVariantId && !x.IsDeleted)
                .OrderBy(x => x.AssetCode)
                .ThenBy(x => x.CreateDate)
                .ToList();

            // Load navigation properties
            LoadNavigationProperties(context, assets);

            _logger.Info("GetByProductVariantId: Lấy được {0} tài sản", assets.Count);
            return assets;
        }
        catch (Exception ex)
        {
            _logger.Error($"GetByProductVariantId: Lỗi lấy danh sách tài sản: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Query tài sản theo nhiều tiêu chí
    /// </summary>
    public List<Asset> QueryAssets(
        Guid? companyId = null,
        Guid? branchId = null,
        Guid? departmentId = null,
        Guid? employeeId = null,
        Guid? productVariantId = null,
        int? assetType = null,
        int? assetCategory = null,
        int? status = null,
        int? condition = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        bool? isActive = null)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("QueryAssets: Bắt đầu query tài sản, CompanyId={0}, BranchId={1}, DepartmentId={2}, EmployeeId={3}, AssetType={4}, Status={5}", 
                companyId, branchId, departmentId, employeeId, assetType, status);

            // Bắt đầu query từ Asset
            var queryable = context.Assets.AsQueryable();

            // Filter theo IsDeleted
            queryable = queryable.Where(x => !x.IsDeleted);

            // Filter theo công ty
            if (companyId.HasValue)
            {
                queryable = queryable.Where(x => x.CompanyId == companyId.Value);
            }

            // Filter theo chi nhánh
            if (branchId.HasValue)
            {
                queryable = queryable.Where(x => x.BranchId == branchId.Value);
            }

            // Filter theo phòng ban
            if (departmentId.HasValue)
            {
                queryable = queryable.Where(x => x.DepartmentId == departmentId.Value);
            }

            // Filter theo nhân viên
            if (employeeId.HasValue)
            {
                queryable = queryable.Where(x => x.AssignedEmployeeId == employeeId.Value);
            }

            // Filter theo sản phẩm
            if (productVariantId.HasValue)
            {
                queryable = queryable.Where(x => x.ProductVariantId == productVariantId.Value);
            }

            // Filter theo loại tài sản
            if (assetType.HasValue)
            {
                queryable = queryable.Where(x => x.AssetType == assetType.Value);
            }

            // Filter theo danh mục tài sản
            if (assetCategory.HasValue)
            {
                queryable = queryable.Where(x => x.AssetCategory == assetCategory.Value);
            }

            // Filter theo trạng thái
            if (status.HasValue)
            {
                queryable = queryable.Where(x => x.Status == status.Value);
            }

            // Filter theo tình trạng
            if (condition.HasValue)
            {
                queryable = queryable.Where(x => x.Condition == condition.Value);
            }

            // Filter theo thời gian (CreateDate)
            if (fromDate.HasValue && toDate.HasValue)
            {
                queryable = queryable.Where(x => x.CreateDate >= fromDate.Value.Date && 
                                                x.CreateDate <= toDate.Value.Date.AddDays(1).AddTicks(-1));
            }

            // Filter theo IsActive
            if (isActive.HasValue)
            {
                queryable = queryable.Where(x => x.IsActive == isActive.Value);
            }

            // Sắp xếp theo mã tài sản và ngày tạo
            var result = queryable
                .OrderBy(x => x.AssetCode)
                .ThenBy(x => x.CreateDate)
                .ToList();

            // Load navigation properties
            LoadNavigationProperties(context, result);

            _logger.Info("QueryAssets: Query thành công, ResultCount={0}", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.Error($"QueryAssets: Lỗi query tài sản: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Load navigation properties cho danh sách tài sản
    /// </summary>
    private void LoadNavigationProperties(VnsErp2025DataContext context, List<Asset> assets)
    {
        if (assets == null || !assets.Any())
            return;

        // Load Company
        var companyIds = assets.Select(x => x.CompanyId).Distinct().ToList();
        if (companyIds.Any())
        {
            var companies = context.Companies
                .Where(c => companyIds.Contains(c.Id))
                .ToDictionary(c => c.Id);

            foreach (var asset in assets)
            {
                if (asset.Company == null && companies.TryGetValue(asset.CompanyId, out var company))
                {
                    asset.Company = company;
                }
            }
        }

        // Load CompanyBranch
        var branchIds = assets
            .Where(x => x.BranchId.HasValue)
            .Select(x => x.BranchId.Value)
            .Distinct()
            .ToList();
        if (branchIds.Any())
        {
            var branches = context.CompanyBranches
                .Where(b => branchIds.Contains(b.Id))
                .ToDictionary(b => b.Id);

            foreach (var asset in assets)
            {
                if (asset.BranchId.HasValue && asset.CompanyBranch == null && 
                    branches.TryGetValue(asset.BranchId.Value, out var branch))
                {
                    asset.CompanyBranch = branch;
                }
            }
        }

        // Load Department
        var departmentIds = assets
            .Where(x => x.DepartmentId.HasValue)
            .Select(x => x.DepartmentId.Value)
            .Distinct()
            .ToList();
        if (departmentIds.Any())
        {
            var departments = context.Departments
                .Where(d => departmentIds.Contains(d.Id))
                .ToDictionary(d => d.Id);

            foreach (var asset in assets)
            {
                if (asset.DepartmentId.HasValue && asset.Department == null && 
                    departments.TryGetValue(asset.DepartmentId.Value, out var department))
                {
                    asset.Department = department;
                }
            }
        }

        // Load Employee
        var employeeIds = assets
            .Where(x => x.AssignedEmployeeId.HasValue)
            .Select(x => x.AssignedEmployeeId.Value)
            .Distinct()
            .ToList();
        if (employeeIds.Any())
        {
            var employees = context.Employees
                .Where(e => employeeIds.Contains(e.Id))
                .ToDictionary(e => e.Id);

            foreach (var asset in assets)
            {
                if (asset.AssignedEmployeeId.HasValue && asset.Employee == null && 
                    employees.TryGetValue(asset.AssignedEmployeeId.Value, out var employee))
                {
                    asset.Employee = employee;
                }
            }
        }

        // Load ProductVariant với ProductService và UnitOfMeasure
        var productVariantIds = assets
            .Where(x => x.ProductVariantId.HasValue)
            .Select(x => x.ProductVariantId.Value)
            .Distinct()
            .ToList();
        if (productVariantIds.Any())
        {
            // Load ProductVariants với navigation properties
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

            // Gán ProductVariant vào Asset và materialize navigation properties
            var productVariantsDict = productVariants.ToDictionary(pv => pv.Id);
            foreach (var asset in assets)
            {
                if (asset.ProductVariantId.HasValue && asset.ProductVariant == null && 
                    productVariantsDict.TryGetValue(asset.ProductVariantId.Value, out var productVariant))
                {
                    asset.ProductVariant = productVariant;
                    
                    // Materialize navigation properties để tránh lazy loading sau khi context dispose
                    _ = productVariant.ProductService?.Name; // Materialize ProductService
                    _ = productVariant.UnitOfMeasure?.Name; // Materialize UnitOfMeasure
                }
            }
        }

        // Load Warranty
        var warrantyIds = assets
            .Where(x => x.WarrantyId.HasValue)
            .Select(x => x.WarrantyId.Value)
            .Distinct()
            .ToList();
        if (warrantyIds.Any())
        {
            var warranties = context.Warranties
                .Where(w => warrantyIds.Contains(w.Id))
                .ToDictionary(w => w.Id);

            foreach (var asset in assets)
            {
                if (asset.WarrantyId.HasValue && asset.Warranty == null && 
                    warranties.TryGetValue(asset.WarrantyId.Value, out var warranty))
                {
                    asset.Warranty = warranty;
                }
            }
        }

        // Load ApplicationUser (CreateBy, ModifiedBy, DeletedBy)
        var userIds = assets
            .SelectMany(x => new[] { 
                x.CreateBy, 
                x.ModifiedBy, 
                x.DeletedBy
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

            foreach (var asset in assets)
            {
                // CreateBy (Guid?, nullable)
                if (asset.CreateBy.HasValue && asset.ApplicationUser == null && 
                    users.TryGetValue(asset.CreateBy.Value, out var createUser))
                {
                    asset.ApplicationUser = createUser;
                }

                // ModifiedBy (Guid?, nullable)
                if (asset.ModifiedBy.HasValue && asset.ApplicationUser2 == null && 
                    users.TryGetValue(asset.ModifiedBy.Value, out var modifiedUser))
                {
                    asset.ApplicationUser2 = modifiedUser;
                }

                // DeletedBy (Guid?, nullable)
                if (asset.DeletedBy.HasValue && asset.ApplicationUser1 == null && 
                    users.TryGetValue(asset.DeletedBy.Value, out var deletedUser))
                {
                    asset.ApplicationUser1 = deletedUser;
                }
            }
        }
    }

    #endregion

    #region Delete Operations

    /// <summary>
    /// Xóa tài sản theo ID
    /// </summary>
    /// <param name="id">ID tài sản cần xóa</param>
    /// <param name="deletedBy">ID người xóa (optional, để tương thích với BLL)</param>
    public void Delete(Guid id, Guid deletedBy = default)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("Delete: Bắt đầu xóa tài sản, Id={0}", id);

            var asset = context.Assets.FirstOrDefault(x => x.Id == id);
            if (asset == null)
            {
                _logger.Warning("Delete: Không tìm thấy tài sản với Id={0}", id);
                throw new InvalidOperationException($"Không tìm thấy tài sản với ID: {id}");
            }

            context.Assets.DeleteOnSubmit(asset);
            context.SubmitChanges();

            _logger.Info("Delete: Đã xóa tài sản, Id={0}", id);
        }
        catch (Exception ex)
        {
            _logger.Error($"Delete: Lỗi xóa tài sản: {ex.Message}", ex);
            throw;
        }
    }

    #endregion
}

