using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DTO.MasterData.CustomerPartner;

namespace Dal.DataAccess.Interfaces.MasterData.PartnerRepository;

/// <summary>
/// Data Access cho thực thể BusinessPartnerCategory (LINQ to SQL trên VnsErp2025DataContext).
/// Cung cấp CRUD đầy đủ, phương thức đặc thù và phiên bản đồng bộ/bất đồng bộ.
/// </summary>
public interface IBusinessPartnerCategoryRepository
{
    #region ========== CREATE OPERATIONS ==========

    /// <summary>
    /// Thêm danh mục đối tác mới với validation cơ bản.
    /// </summary>
    /// <param name="categoryName">Tên danh mục</param>
    /// <param name="description">Mô tả</param>
    /// <returns>BusinessPartnerCategoryDto đã tạo</returns>
    BusinessPartnerCategoryDto AddNewCategory(string categoryName, string description = null);

    /// <summary>
    /// Thêm danh mục đối tác mới (Async).
    /// </summary>
    Task<BusinessPartnerCategoryDto> AddNewCategoryAsync(string categoryName, string description = null);

    /// <summary>
    /// Lưu/cập nhật đầy đủ thông tin danh mục.
    /// </summary>
    void SaveOrUpdate(BusinessPartnerCategoryDto dto);

    /// <summary>
    /// Lưu/cập nhật đầy đủ thông tin danh mục (Async).
    /// </summary>
    Task SaveOrUpdateAsync(BusinessPartnerCategoryDto dto);

    #endregion

    #region ========== READ OPERATIONS ==========

    /// <summary>
    /// Lấy danh mục theo Id.
    /// </summary>
    BusinessPartnerCategoryDto GetById(Guid id);

    /// <summary>
    /// Lấy tất cả danh mục.
    /// </summary>
    List<BusinessPartnerCategoryDto> GetAll();

    /// <summary>
    /// Lấy tất cả danh mục (Async).
    /// </summary>
    Task<List<BusinessPartnerCategoryDto>> GetAllAsync();

    /// <summary>
    /// Tìm kiếm danh mục theo tên (contains, case-insensitive).
    /// </summary>
    List<BusinessPartnerCategoryDto> SearchByName(string keyword);

    #endregion

    #region ========== UPDATE OPERATIONS ==========

    /// <summary>
    /// Cập nhật thông tin danh mục.
    /// </summary>
    void UpdateCategory(Guid id, string categoryName, string description = null);

    /// <summary>
    /// Cập nhật thông tin danh mục (Async).
    /// </summary>
    Task UpdateCategoryAsync(Guid id, string categoryName, string description = null);

    #endregion

    #region ========== DELETE OPERATIONS ==========

    /// <summary>
    /// Xóa danh mục theo Id. Nếu có partners, chuyển sang category "Chưa phân loại".
    /// </summary>
    void DeleteCategory(Guid id);

    /// <summary>
    /// Xóa danh mục theo Id (Async). Nếu có partners, chuyển sang category "Chưa phân loại".
    /// </summary>
    Task DeleteCategoryAsync(Guid id);

    #endregion

    #region ========== VALIDATION & EXISTS CHECKS ==========

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

    #region ========== BUSINESS LOGIC METHODS ==========

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
}