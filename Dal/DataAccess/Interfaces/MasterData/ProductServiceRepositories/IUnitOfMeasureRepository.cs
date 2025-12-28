using System;
using System.Collections.Generic;
using DTO.MasterData.ProductService;

namespace Dal.DataAccess.Interfaces.MasterData.ProductServiceRepositories;

/// <summary>
/// Data Access cho thực thể UnitOfMeasure (LINQ to SQL trên VnsErp2025DataContext).
/// Cung cấp CRUD, tìm kiếm, kiểm tra unique và phiên bản đồng bộ/bất đồng bộ.
/// </summary>
public interface IUnitOfMeasureRepository
{
    #region ========== READ OPERATIONS ==========

    /// <summary>
    /// Lấy UnitOfMeasure theo Id.
    /// </summary>
    UnitOfMeasureDto GetById(Guid id);

    /// <summary>
    /// Lấy tất cả UnitOfMeasure.
    /// </summary>
    List<UnitOfMeasureDto> GetAll();

    /// <summary>
    /// Lấy UnitOfMeasure theo mã.
    /// </summary>
    UnitOfMeasureDto GetByCode(string code);

    /// <summary>
    /// Lấy UnitOfMeasure theo tên.
    /// </summary>
    UnitOfMeasureDto GetByName(string name);

    /// <summary>
    /// Tìm kiếm UnitOfMeasure theo từ khóa (Code/Name/Description).
    /// </summary>
    List<UnitOfMeasureDto> Search(string keyword);

    /// <summary>
    /// Lấy UnitOfMeasure theo trạng thái hoạt động.
    /// </summary>
    List<UnitOfMeasureDto> GetByStatus(bool isActive);

    #endregion

    #region ========== CREATE OPERATIONS ==========

    #endregion

    #region ========== UPDATE OPERATIONS ==========

    /// <summary>
    /// Lưu hoặc cập nhật UnitOfMeasure.
    /// </summary>
    void SaveOrUpdate(UnitOfMeasureDto dto);

    #endregion

    #region ========== DELETE OPERATIONS ==========

    /// <summary>
    /// Xóa UnitOfMeasure theo Id (kèm kiểm tra phụ thuộc).
    /// </summary>
    bool DeleteUnitOfMeasure(Guid id);

    #endregion

    #region ========== VALIDATION & EXISTS CHECKS ==========

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

    #region ========== HELPER METHODS ==========

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
