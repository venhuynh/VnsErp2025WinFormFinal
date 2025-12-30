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

    

    

    
}

