using Dal.DataContext;
using System;
using Dal.DataAccess.Interfaces.MasterData.Company;

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

    #region Company

    /// <summary>
    /// Lấy repository để quản lý dữ liệu chi nhánh công ty.
    /// </summary>
    /// <returns>
    /// Một thể hiện của <see cref="Dal.DataAccess.Interfaces.MasterData.Company.ICompanyBranchRepository"/> 
    /// cung cấp các phương thức truy cập dữ liệu cho chi nhánh công ty.
    /// </returns>
    ICompanyBranchRepository GetCompanyBranchRepository();

    /// <summary>
    /// Lấy đối tượng truy cập dữ liệu cho chức vụ.
    /// </summary>
    /// <returns>
    /// Một thể hiện của <see cref="Dal.DataAccess.Interfaces.MasterData.Company.IPositionDataAccess"/> 
    /// cung cấp các phương thức truy cập dữ liệu cho chức vụ.
    /// </returns>
    IPositionRepository GetPositionReposiroty();

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    ICompanyRepository GetCompanyRepository();

    
    IDepartmentRepository GetDepartmentRepository();

    #endregion

    #endregion
}