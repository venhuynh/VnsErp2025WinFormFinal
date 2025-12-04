using Dal.DataAccess.Interfaces.Inventory.InventoryManagement;
using Dal.DataAccess.Interfaces.Inventory.StockIn;
using Dal.DataAccess.Interfaces.MasterData.CompanyRepository;
using Dal.DataAccess.Interfaces.MasterData.PartnerRepository;
using Dal.DataAccess.Interfaces.MasterData.ProductServiceRepositories;
using Dal.DataContext;
using System;

namespace Dal.DataAccess.Interfaces;

/// <summary>
/// Interface cho Unit of Work pattern
/// Quản lý transaction và context lifecycle
/// </summary>
public interface IUnitOfWork : IDisposable
{
    #region Properties

    /// <summary>
    /// DataContext instance
    /// </summary>
    VnsErp2025DataContext Context { get; }

    /// <summary>
    /// Trạng thái transaction
    /// </summary>
    bool IsInTransaction { get; }

    #endregion

    #region Transaction Management

    /// <summary>
    /// Bắt đầu transaction
    /// </summary>
    void BeginTransaction();

    /// <summary>
    /// Commit transaction
    /// </summary>
    void Commit();

    /// <summary>
    /// Rollback transaction
    /// </summary>
    void Rollback();

    /// <summary>
    /// Save changes
    /// </summary>
    void SaveChanges();

    #endregion

    #region MasterData

    IApplicationUserRepository GetApplicationUserDataAccess();

    #region Company

    /// <summary>
    /// Lấy repository để quản lý dữ liệu chi nhánh công ty.
    /// </summary>
    /// <returns>
    /// Một thể hiện của <see cref="ICompanyBranchRepository"/> 
    /// cung cấp các phương thức truy cập dữ liệu cho chi nhánh công ty.
    /// </returns>
    ICompanyBranchRepository GetCompanyBranchRepository();

    /// <summary>
    /// Lấy đối tượng truy cập dữ liệu cho chức vụ.
    /// </summary>
    /// <returns>
    /// Một thể hiện của <see cref="Company.IPositionDataAccess"/> 
    /// cung cấp các phương thức truy cập dữ liệu cho chức vụ.
    /// </returns>
    IPositionRepository GetPositionReposiroty();

    /// <summary>
    /// Lấy đối tượng truy cập dữ liệu cho thực thể công ty.
    /// </summary>
    /// <returns>
    /// Một thể hiện của <see cref="ICompanyRepository"/> 
    /// cung cấp các phương thức truy cập dữ liệu cho thực thể công ty.
    /// </returns>
    ICompanyRepository GetCompanyRepository();

    /// <summary>
    /// Lấy đối tượng truy cập dữ liệu để quản lý dữ liệu phòng ban.
    /// </summary>
    /// <returns>
    /// Một thể hiện của <see cref="IDepartmentRepository"/> 
    /// cung cấp các phương thức để truy cập dữ liệu phòng ban.
    /// </returns>
    IDepartmentRepository GetDepartmentRepository();

    /// <summary>
    /// Lấy đối tượng truy cập dữ liệu cho nhân viên.
    /// </summary>
    /// <returns>
    /// Một thể hiện của <see cref="Dal.DataAccess.Interfaces.MasterData.CompanyRepository.IEmployeeRepository"/> 
    /// cung cấp các phương thức truy cập dữ liệu cho nhân viên.
    /// </returns>
    IEmployeeRepository GetEmployeeRepository();
    #endregion

    #region Partner
    /// <summary>
    /// Lấy đối tượng truy cập dữ liệu để quản lý thực thể đối tác kinh doanh.
    /// </summary>
    /// <returns>
    /// Một thể hiện của <see cref="IBusinessPartnerRepository"/> 
    /// cung cấp các phương thức để truy cập và quản lý dữ liệu đối tác kinh doanh.
    /// </returns>
    IBusinessPartnerRepository GetBusinessPartnerRepository();

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IBusinessPartnerCategoryRepository GetBusinessPartnerCategoryRepository();

    /// <summary>
    /// Lấy đối tượng truy cập dữ liệu để quản lý dữ liệu liên hệ của đối tác kinh doanh.
    /// </summary>
    /// <returns>
    /// Một thể hiện của <see cref="IBusinessPartnerContactRepository"/> 
    /// cung cấp các phương thức để truy cập và quản lý dữ liệu liên hệ của đối tác kinh doanh.
    /// </returns>
    IBusinessPartnerContactRepository GetBusinessPartnerContactRepository();

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IBusinessPartnerSiteRepository GetBusinessPartnerSiteRepository();

    #endregion

    #region ProductService

    IAttributeRepository GetAttributeRepository();

    IProductImageRepository GetProductImageRepository();

    IProductServiceCategoryRepository GetProductServiceCategoryRepository();

    IProductServiceRepository GetProductServiceRepository();

    IProductVariantRepository GetProductVariantRepository();

    IUnitOfMeasureRepository GetUnitOfMeasureRepository();

    #endregion

    #endregion

    #region Inventory

    #region StockIn

    IStockInRepository GetStockInRepository();

    #endregion

    #region InventoryManagement

    IStockInOutImageRepository GetStockInOutImageRepository();

    IWarrantyRepository GetWarrantyRepository();

    IStockInOutMasterRepository GetStockInOutMasterRepository();

    IStockInOutDetailRepository GetStockInOutDetailRepository();

    IDeviceRepository GetDeviceRepository();

    IStockInOutDocumentRepository GetStockInOutDocumentRepository();

    IInventoryBalanceRepository GetInventoryBalanceRepository();

    IAssetRepository GetAssetRepository();

    #endregion

    #endregion
}