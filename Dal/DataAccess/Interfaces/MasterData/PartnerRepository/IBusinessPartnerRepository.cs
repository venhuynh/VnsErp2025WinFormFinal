using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DTO.MasterData.CustomerPartner;

namespace Dal.DataAccess.Interfaces.MasterData.PartnerRepository;

/// <summary>
/// Data Access cho thực thể BusinessPartner (LINQ to SQL trên VnsErp2025DataContext).
/// Cung cấp CRUD đầy đủ, phương thức đặc thù và phiên bản đồng bộ/bất đồng bộ.
/// </summary>
public interface IBusinessPartnerRepository
{
    #region ========== CREATE OPERATIONS ==========

    /// <summary>
    /// Thêm đối tác mới với validation cơ bản (mã/code duy nhất, tên bắt buộc).
    /// </summary>
    /// <param name="code">Mã đối tác</param>
    /// <param name="name">Tên đối tác</param>
    /// <param name="partnerType">Loại đối tác</param>
    /// <param name="isActive">Trạng thái</param>
    /// <param name="createdBy">ID người tạo (optional)</param>
    /// <returns>BusinessPartnerDetailDto đã tạo</returns>
    BusinessPartnerDetailDto AddNewPartner(string code, string name, int partnerType, bool isActive = true, Guid? createdBy = null);

    /// <summary>
    /// Thêm đối tác mới (Async).
    /// </summary>
    /// <param name="code">Mã đối tác</param>
    /// <param name="name">Tên đối tác</param>
    /// <param name="partnerType">Loại đối tác</param>
    /// <param name="isActive">Trạng thái</param>
    /// <param name="createdBy">ID người tạo (optional)</param>
    Task<BusinessPartnerDetailDto> AddNewPartnerAsync(string code, string name, int partnerType, bool isActive = true, Guid? createdBy = null);

    /// <summary>
    /// Thêm mới hoặc cập nhật đầy đủ thông tin đối tác.
    /// Nếu Id tồn tại -> cập nhật tất cả trường theo DTO truyền vào.
    /// Nếu không tồn tại -> thêm mới.
    /// </summary>
    /// <param name="dto">BusinessPartnerDetailDto</param>
    /// <param name="userId">ID người dùng thực hiện (optional, dùng cho audit fields)</param>
    void SaveOrUpdate(BusinessPartnerDetailDto dto, Guid? userId = null);

    #endregion

    #region ========== READ OPERATIONS ==========

    /// <summary>
    /// Lấy đối tác theo Id.
    /// </summary>
    BusinessPartnerDetailDto GetById(Guid id);

    /// <summary>
    /// Lấy đối tác theo Id (Async).
    /// </summary>
    Task<BusinessPartnerDetailDto> GetByIdAsync(Guid id);

    /// <summary>
    /// Lấy đối tác theo mã.
    /// </summary>
    BusinessPartnerDetailDto GetByCode(string code);

    /// <summary>
    /// Lấy đối tác theo mã (Async).
    /// </summary>
    Task<BusinessPartnerDetailDto> GetByCodeAsync(string code);

    /// <summary>
    /// Tìm kiếm đối tác theo tên (contains, case-insensitive).
    /// </summary>
    List<BusinessPartnerListDto> SearchByName(string keyword);

    /// <summary>
    /// Lấy danh sách đối tác đang hoạt động.
    /// </summary>
    List<BusinessPartnerListDto> GetActivePartners();

    /// <summary>
    /// Lấy danh sách đối tác đang hoạt động (Async).
    /// </summary>
    Task<List<BusinessPartnerListDto>> GetActivePartnersAsync();

    /// <summary>
    /// Lấy danh sách đối tác đang hoạt động cho Lookup (chỉ load các trường cần thiết: Id, PartnerCode, PartnerType, PartnerName, LogoThumbnailData).
    /// Không load navigation properties để tối ưu hiệu năng.
    /// </summary>
    List<BusinessPartnerLookupDto> GetActivePartnersForLookup();

    /// <summary>
    /// Lấy danh sách đối tác đang hoạt động cho Lookup (Async) - chỉ load các trường cần thiết.
    /// </summary>
    Task<List<BusinessPartnerLookupDto>> GetActivePartnersForLookupAsync();

    #endregion

    #region ========== UPDATE OPERATIONS ==========

    /// <summary>
    /// Cập nhật thông tin liên hệ (điện thoại, email) cho đối tác.
    /// </summary>
    /// <param name="id">ID đối tác</param>
    /// <param name="phone">Số điện thoại</param>
    /// <param name="email">Email</param>
    /// <param name="modifiedBy">ID người cập nhật (optional)</param>
    void UpdateContactInfo(Guid id, string phone, string email, Guid? modifiedBy = null);

    /// <summary>
    /// Kích hoạt/Vô hiệu hóa đối tác.
    /// </summary>
    /// <param name="id">ID đối tác</param>
    /// <param name="isActive">Trạng thái hoạt động</param>
    /// <param name="modifiedBy">ID người cập nhật (optional)</param>
    void SetActive(Guid id, bool isActive, Guid? modifiedBy = null);

    #endregion

    #region ========== DELETE OPERATIONS ==========

    /// <summary>
    /// Xóa đối tác theo Id (Soft Delete - đánh dấu IsDeleted = true).
    /// </summary>
    /// <param name="id">ID đối tác</param>
    /// <param name="deletedBy">ID người xóa (optional)</param>
    void DeletePartner(Guid id, Guid? deletedBy = null);

    /// <summary>
    /// Xóa đối tác theo Id (Async) - Soft Delete.
    /// </summary>
    /// <param name="id">ID đối tác</param>
    /// <param name="deletedBy">ID người xóa (optional)</param>
    Task DeletePartnerAsync(Guid id, Guid? deletedBy = null);

    #endregion

    #region ========== VALIDATION & EXISTS CHECKS ==========

    /// <summary>
    /// Kiểm tra tồn tại theo Id.
    /// </summary>
    bool Exists(Guid id);

    /// <summary>
    /// Kiểm tra tồn tại mã đối tác.
    /// </summary>
    bool IsPartnerCodeExists(string code);

    /// <summary>
    /// Kiểm tra tồn tại mã đối tác (Async).
    /// </summary>
    Task<bool> IsPartnerCodeExistsAsync(string code);

    #endregion
}