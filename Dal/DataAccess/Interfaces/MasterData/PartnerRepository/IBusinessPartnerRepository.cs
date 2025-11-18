using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dal.DataContext;

namespace Dal.DataAccess.Interfaces.MasterData.PartnerRepository;

/// <summary>
/// Data Access cho thực thể BusinessPartner (LINQ to SQL trên VnsErp2025DataContext).
/// Cung cấp CRUD đầy đủ, phương thức đặc thù và phiên bản đồng bộ/bất đồng bộ.
/// </summary>
public interface IBusinessPartnerRepository
{
    #region Create

    /// <summary>
    /// Thêm đối tác mới với validation cơ bản (mã/code duy nhất, tên bắt buộc).
    /// </summary>
    /// <param name="code">Mã đối tác</param>
    /// <param name="name">Tên đối tác</param>
    /// <param name="partnerType">Loại đối tác</param>
    /// <param name="isActive">Trạng thái</param>
    /// <returns>Đối tác đã tạo</returns>
    BusinessPartner AddNewPartner(string code, string name, int partnerType, bool isActive = true);

    /// <summary>
    /// Thêm đối tác mới (Async).
    /// </summary>
    Task<BusinessPartner> AddNewPartnerAsync(string code, string name, int partnerType, bool isActive = true);

    #endregion

    #region Read

    /// <summary>
    /// Lấy đối tác theo Id.
    /// </summary>
    BusinessPartner GetById(Guid id);

    /// <summary>
    /// Lấy đối tác theo Id (Async).
    /// </summary>
    Task<BusinessPartner> GetByIdAsync(Guid id);

    /// <summary>
    /// Lấy đối tác theo mã.
    /// </summary>
    BusinessPartner GetByCode(string code);

    /// <summary>
    /// Lấy đối tác theo mã (Async).
    /// </summary>
    Task<BusinessPartner> GetByCodeAsync(string code);

    /// <summary>
    /// Tìm kiếm đối tác theo tên (contains, case-insensitive).
    /// </summary>
    List<BusinessPartner> SearchByName(string keyword);

    /// <summary>
    /// Lấy danh sách đối tác đang hoạt động.
    /// </summary>
    List<BusinessPartner> GetActivePartners();

    #endregion

    #region Update

    /// <summary>
    /// Cập nhật thông tin liên hệ (điện thoại, email) cho đối tác.
    /// </summary>
    void UpdateContactInfo(Guid id, string phone, string email);

    /// <summary>
    /// Kích hoạt/Vô hiệu hóa đối tác.
    /// </summary>
    void SetActive(Guid id, bool isActive);

    #endregion

    #region Delete

    /// <summary>
    /// Xóa đối tác theo Id.
    /// </summary>
    void DeletePartner(Guid id);

    /// <summary>
    /// Xóa đối tác theo Id (Async).
    /// </summary>
    Task DeletePartnerAsync(Guid id);

    #endregion

    #region Exists Checks

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

    #region Save/Update Full Entity

    /// <summary>
    /// Thêm mới hoặc cập nhật đầy đủ thông tin đối tác.
    /// Nếu Id tồn tại -> cập nhật tất cả trường theo entity truyền vào.
    /// Nếu không tồn tại -> thêm mới.
    /// </summary>
    void SaveOrUpdate(BusinessPartner source);

    #endregion
}