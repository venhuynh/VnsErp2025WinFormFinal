using Dal.DataContext;
using System;
using System.Collections.Generic;

namespace Dal.DataAccess.Interfaces.Inventory.InventoryManagement;

/// <summary>
/// Repository interface cho Asset
/// Quản lý các thao tác CRUD với bảng Asset (Tài sản)
/// </summary>
public interface IAssetRepository
{
    #region ========== READ OPERATIONS ==========

    /// <summary>
    /// Lấy tài sản theo ID
    /// </summary>
    /// <param name="id">ID tài sản</param>
    /// <returns>Asset hoặc null</returns>
    Asset GetById(Guid id);

    /// <summary>
    /// Lấy tài sản theo mã tài sản
    /// </summary>
    /// <param name="assetCode">Mã tài sản</param>
    /// <returns>Asset hoặc null</returns>
    Asset GetByAssetCode(string assetCode);

    /// <summary>
    /// Lấy danh sách tài sản theo công ty
    /// </summary>
    /// <param name="companyId">ID công ty</param>
    /// <returns>Danh sách tài sản</returns>
    List<Asset> GetByCompanyId(Guid companyId);

    /// <summary>
    /// Lấy danh sách tài sản theo chi nhánh
    /// </summary>
    /// <param name="branchId">ID chi nhánh</param>
    /// <returns>Danh sách tài sản</returns>
    List<Asset> GetByBranchId(Guid branchId);

    /// <summary>
    /// Lấy danh sách tài sản theo phòng ban
    /// </summary>
    /// <param name="departmentId">ID phòng ban</param>
    /// <returns>Danh sách tài sản</returns>
    List<Asset> GetByDepartmentId(Guid departmentId);

    /// <summary>
    /// Lấy danh sách tài sản theo nhân viên phụ trách
    /// </summary>
    /// <param name="employeeId">ID nhân viên</param>
    /// <returns>Danh sách tài sản</returns>
    List<Asset> GetByEmployeeId(Guid employeeId);

    /// <summary>
    /// Lấy danh sách tài sản theo sản phẩm
    /// </summary>
    /// <param name="productVariantId">ID biến thể sản phẩm</param>
    /// <returns>Danh sách tài sản</returns>
    List<Asset> GetByProductVariantId(Guid productVariantId);

    /// <summary>
    /// Query tài sản theo nhiều tiêu chí
    /// </summary>
    /// <param name="companyId">ID công ty (null = tất cả)</param>
    /// <param name="branchId">ID chi nhánh (null = tất cả)</param>
    /// <param name="departmentId">ID phòng ban (null = tất cả)</param>
    /// <param name="employeeId">ID nhân viên (null = tất cả)</param>
    /// <param name="productVariantId">ID biến thể sản phẩm (null = tất cả)</param>
    /// <param name="assetType">Loại tài sản (null = tất cả)</param>
    /// <param name="assetCategory">Danh mục tài sản (null = tất cả)</param>
    /// <param name="status">Trạng thái (null = tất cả)</param>
    /// <param name="condition">Tình trạng (null = tất cả)</param>
    /// <param name="fromDate">Từ ngày (CreateDate) (null = không filter)</param>
    /// <param name="toDate">Đến ngày (CreateDate) (null = không filter)</param>
    /// <param name="isActive">Đang hoạt động (null = tất cả)</param>
    /// <returns>Danh sách tài sản</returns>
    List<Asset> QueryAssets(
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
        bool? isActive = null);

    #endregion

    #region ========== CREATE/UPDATE OPERATIONS ==========

    /// <summary>
    /// Lưu hoặc cập nhật tài sản
    /// </summary>
    /// <param name="asset">Entity tài sản cần lưu</param>
    void SaveOrUpdate(Asset asset);

    #endregion

    #region ========== DELETE OPERATIONS ==========

    /// <summary>
    /// Xóa tài sản theo ID
    /// </summary>
    /// <param name="id">ID tài sản cần xóa</param>
    /// <param name="deletedBy">ID người xóa (optional, để tương thích với BLL)</param>
    void Delete(Guid id, Guid deletedBy = default);

    #endregion
}
