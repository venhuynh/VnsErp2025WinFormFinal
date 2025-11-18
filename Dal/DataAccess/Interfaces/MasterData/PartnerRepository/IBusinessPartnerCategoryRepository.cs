using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dal.DataContext;

namespace Dal.DataAccess.Interfaces.MasterData.PartnerRepository;

/// <summary>
/// Data Access cho thực thể BusinessPartnerCategory (LINQ to SQL trên VnsErp2025DataContext).
/// Cung cấp CRUD đầy đủ, phương thức đặc thù và phiên bản đồng bộ/bất đồng bộ.
/// </summary>
public interface IBusinessPartnerCategoryRepository
{
    #region Create

    /// <summary>
    /// Thêm danh mục đối tác mới với validation cơ bản.
    /// </summary>
    /// <param name="categoryName">Tên danh mục</param>
    /// <param name="description">Mô tả</param>
    /// <returns>Danh mục đã tạo</returns>
    BusinessPartnerCategory AddNewCategory(string categoryName, string description = null);

    /// <summary>
    /// Thêm danh mục đối tác mới (Async).
    /// </summary>
    Task<BusinessPartnerCategory> AddNewCategoryAsync(string categoryName, string description = null);

    #endregion

    #region Read

    /// <summary>
    /// Lấy danh mục theo Id.
    /// </summary>
    BusinessPartnerCategory GetById(Guid id);

    /// <summary>
    /// Lấy tất cả danh mục.
    /// </summary>
    List<BusinessPartnerCategory> GetAll();

    /// <summary>
    /// Lấy tất cả danh mục (Async).
    /// </summary>
    Task<List<BusinessPartnerCategory>> GetAllAsync();

    /// <summary>
    /// Tìm kiếm danh mục theo tên (contains, case-insensitive).
    /// </summary>
    List<BusinessPartnerCategory> SearchByName(string keyword);

    #endregion

    #region Update

    /// <summary>
    /// Cập nhật thông tin danh mục.
    /// </summary>
    void UpdateCategory(Guid id, string categoryName, string description = null);

    /// <summary>
    /// Cập nhật thông tin danh mục (Async).
    /// </summary>
    Task UpdateCategoryAsync(Guid id, string categoryName, string description = null);

    #endregion

    #region Delete

    /// <summary>
    /// Xóa danh mục theo Id. Nếu có partners, chuyển sang category "Chưa phân loại".
    /// </summary>
    void DeleteCategory(Guid id);

    /// <summary>
    /// Xóa danh mục theo Id (Async). Nếu có partners, chuyển sang category "Chưa phân loại".
    /// </summary>
    Task DeleteCategoryAsync(Guid id);

    #endregion

    #region Exists Checks

    /// <summary>
    /// Kiểm tra tồn tại theo Id.
    /// </summary>
    bool Exists(Guid id);

    /// <summary>
    /// Kiểm tra tồn tại tên danh mục.
    /// </summary>
    bool IsCategoryNameExists(string categoryName, Guid excludeId = default(Guid));

    /// <summary>
    /// Kiểm tra tồn tại tên danh mục (Async).
    /// </summary>
    Task<bool> IsCategoryNameExistsAsync(string categoryName, Guid excludeId = default(Guid));

    #endregion

    #region Partner Count Methods

    /// <summary>
    /// Kiểm tra xem danh mục có đối tác nào không.
    /// </summary>
    bool HasPartners(Guid categoryId);

    /// <summary>
    /// Kiểm tra xem danh mục có đối tác nào không (Async).
    /// </summary>
    Task<bool> HasPartnersAsync(Guid categoryId);

    /// <summary>
    /// Lấy số lượng đối tác của một danh mục cụ thể.
    /// </summary>
    int GetPartnerCount(Guid categoryId);

    /// <summary>
    /// Lấy số lượng đối tác của một danh mục cụ thể (Async).
    /// </summary>
    Task<int> GetPartnerCountAsync(Guid categoryId);

    /// <summary>
    /// Đếm số lượng đối tác theo từng danh mục (bao gồm cả đối tác của sub-categories).
    /// </summary>
    Dictionary<Guid, int> GetPartnerCountByCategory();

    /// <summary>
    /// Đếm số lượng đối tác theo từng danh mục (Async).
    /// </summary>
    Task<Dictionary<Guid, int>> GetPartnerCountByCategoryAsync();

    /// <summary>
    /// Lấy tên các đối tác theo từng danh mục (bao gồm cả đối tác của sub-categories).
    /// </summary>
    Dictionary<Guid, string> GetPartnerNamesByCategory();

    /// <summary>
    /// Lấy mã các đối tác theo từng danh mục (bao gồm cả đối tác của sub-categories).
    /// </summary>
    Dictionary<Guid, string> GetPartnerCodesByCategory();

    #endregion

    #region Save/Update Full Entity

    /// <summary>
    /// Lưu/cập nhật đầy đủ thông tin danh mục.
    /// </summary>
    void SaveOrUpdate(BusinessPartnerCategory entity);

    /// <summary>
    /// Lưu/cập nhật đầy đủ thông tin danh mục (Async).
    /// </summary>
    Task SaveOrUpdateAsync(BusinessPartnerCategory entity);

    #endregion
}