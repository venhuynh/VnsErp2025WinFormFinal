using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dal.DataContext;

namespace Dal.DataAccess.Interfaces.MasterData.ProductServiceRepositories;

/// <summary>
/// Data Access cho thực thể UnitOfMeasure (LINQ to SQL trên VnsErp2025DataContext).
/// Cung cấp CRUD, tìm kiếm, kiểm tra unique và phiên bản đồng bộ/bất đồng bộ.
/// </summary>
public interface IUnitOfMeasureRepository
{
    #region Read Operations

    /// <summary>
    /// Lấy UnitOfMeasure theo Id.
    /// </summary>
    UnitOfMeasure GetById(Guid id);

    /// <summary>
    /// Lấy tất cả UnitOfMeasure.
    /// </summary>
    List<UnitOfMeasure> GetAll();

    /// <summary>
    /// Lấy tất cả UnitOfMeasure (Async).
    /// </summary>
    Task<List<UnitOfMeasure>> GetAllAsync();

    /// <summary>
    /// Lấy UnitOfMeasure theo mã.
    /// </summary>
    UnitOfMeasure GetByCode(string code);

    /// <summary>
    /// Lấy UnitOfMeasure theo tên.
    /// </summary>
    UnitOfMeasure GetByName(string name);

    /// <summary>
    /// Tìm kiếm UnitOfMeasure theo từ khóa (Code/Name/Description).
    /// </summary>
    List<UnitOfMeasure> Search(string keyword);

    /// <summary>
    /// Lấy UnitOfMeasure theo trạng thái hoạt động.
    /// </summary>
    List<UnitOfMeasure> GetByStatus(bool isActive);

    #endregion

    #region Create/Update Operations

    /// <summary>
    /// Lưu hoặc cập nhật UnitOfMeasure.
    /// </summary>
    void SaveOrUpdate(UnitOfMeasure entity);

    #endregion

    #region Delete Operations

    /// <summary>
    /// Xóa UnitOfMeasure theo Id (kèm kiểm tra phụ thuộc).
    /// </summary>
    bool DeleteUnitOfMeasure(Guid id);

    /// <summary>
    /// Xóa UnitOfMeasure theo Id (Async).
    /// </summary>
    Task<bool> DeleteUnitOfMeasureAsync(Guid id);

    #endregion

    #region Validation Operations

    /// <summary>
    /// Kiểm tra mã UnitOfMeasure có tồn tại không.
    /// </summary>
    bool IsCodeExists(string code, Guid? excludeId = null);

    /// <summary>
    /// Kiểm tra tên UnitOfMeasure có tồn tại không.
    /// </summary>
    bool IsNameExists(string name, Guid? excludeId = null);

    /// <summary>
    /// Kiểm tra UnitOfMeasure có phụ thuộc hay không (ProductVariant).
    /// </summary>
    bool HasDependencies(Guid id);

    #endregion

    #region Helper Methods

    /// <summary>
    /// Lấy danh sách mã UnitOfMeasure (unique) - Async.
    /// </summary>
    Task<List<object>> GetUniqueCodesAsync();

    /// <summary>
    /// Lấy danh sách tên UnitOfMeasure (unique) - Async.
    /// </summary>
    Task<List<object>> GetUniqueNamesAsync();

    /// <summary>
    /// Đếm số lượng UnitOfMeasure.
    /// </summary>
    int GetCount();

    /// <summary>
    /// Đếm số lượng UnitOfMeasure theo trạng thái.
    /// </summary>
    int GetCountByStatus(bool isActive);

    #endregion
}
