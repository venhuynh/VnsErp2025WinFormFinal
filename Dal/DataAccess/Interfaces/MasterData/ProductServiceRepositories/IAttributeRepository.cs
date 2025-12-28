using System;
using System.Collections.Generic;
using DTO.MasterData.ProductService;

namespace Dal.DataAccess.Interfaces.MasterData.ProductServiceRepositories;

/// <summary>
/// Interface cho Data Access Layer của Attribute
/// </summary>
public interface IAttributeRepository
{
    #region ========== CREATE OPERATIONS ==========

    /// <summary>
    /// Lưu hoặc cập nhật Attribute
    /// </summary>
    /// <param name="dto">AttributeDto</param>
    void SaveOrUpdate(AttributeDto dto);

    #endregion

    #region ========== READ OPERATIONS ==========

    /// <summary>
    /// Lấy Attribute theo Id
    /// </summary>
    /// <param name="id">Id của Attribute</param>
    /// <returns>AttributeDto hoặc null nếu không tìm thấy</returns>
    AttributeDto GetById(Guid id);

    /// <summary>
    /// Lấy tất cả Attribute
    /// </summary>
    /// <returns>Danh sách tất cả AttributeDto</returns>
    List<AttributeDto> GetAll();

    #endregion

    #region ========== UPDATE OPERATIONS ==========
    // Update operations are handled by SaveOrUpdate method
    #endregion

    #region ========== DELETE OPERATIONS ==========

    /// <summary>
    /// Xóa Attribute theo Id (kèm xóa AttributeValue/VariantAttribute liên quan)
    /// </summary>
    /// <param name="id">Id của Attribute cần xóa</param>
    void Delete(Guid id);

    #endregion

    #region ========== VALIDATION & EXISTS CHECKS ==========

    /// <summary>
    /// Kiểm tra tên thuộc tính đã tồn tại chưa (loại trừ Id khi cập nhật)
    /// </summary>
    /// <param name="name">Tên cần kiểm tra</param>
    /// <param name="excludeId">Id cần loại trừ khỏi kiểm tra (dùng khi update)</param>
    /// <returns>True nếu tên đã tồn tại</returns>
    bool IsNameExists(string name, Guid? excludeId = null);

    /// <summary>
    /// Kiểm tra thuộc tính có phụ thuộc hay không (AttributeValue/VariantAttribute)
    /// </summary>
    /// <param name="id">Id của Attribute</param>
    /// <returns>True nếu có phụ thuộc</returns>
    bool HasDependencies(Guid id);

    #endregion
}
