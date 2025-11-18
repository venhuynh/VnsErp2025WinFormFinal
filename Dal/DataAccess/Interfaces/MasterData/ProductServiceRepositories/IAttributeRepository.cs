using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Attribute = Dal.DataContext.Attribute;

namespace Dal.DataAccess.Interfaces.MasterData.ProductServiceRepositories;

/// <summary>
/// Data Access cho thực thể Attribute (LINQ to SQL trên VnsErp2025DataContext).
/// Cung cấp CRUD, tìm kiếm, kiểm tra unique và phiên bản đồng bộ/bất đồng bộ.
/// </summary>
public interface IAttributeRepository
{
    #region Read

    /// <summary>
    /// Lấy Attribute theo Id.
    /// </summary>
    /// <param name="id">Id của Attribute</param>
    /// <returns>Attribute hoặc null nếu không tìm thấy</returns>
    Attribute GetById(Guid id);

    /// <summary>
    /// Lấy tất cả Attribute.
    /// </summary>
    /// <returns>Danh sách tất cả Attribute</returns>
    List<Attribute> GetAll();

    /// <summary>
    /// Lấy tất cả Attribute (Async).
    /// </summary>
    /// <returns>Danh sách tất cả Attribute</returns>
    Task<List<Attribute>> GetAllAsync();

    /// <summary>
    /// Lấy Attribute theo tên.
    /// </summary>
    /// <param name="name">Tên Attribute</param>
    /// <returns>Attribute hoặc null nếu không tìm thấy</returns>
    Attribute GetByName(string name);

    /// <summary>
    /// Tìm kiếm Attribute theo từ khóa (Name/DataType/Description).
    /// </summary>
    /// <param name="keyword">Từ khóa tìm kiếm</param>
    /// <returns>Danh sách Attribute phù hợp</returns>
    List<Attribute> Search(string keyword);

    #endregion

    #region Create/Update

    /// <summary>
    /// Lưu hoặc cập nhật Attribute.
    /// </summary>
    /// <param name="entity">Attribute cần lưu hoặc cập nhật</param>
    void SaveOrUpdate(Attribute entity);

    #endregion

    #region Delete

    /// <summary>
    /// Xóa Attribute theo Id (kèm xóa AttributeValue/VariantAttribute liên quan).
    /// </summary>
    /// <param name="id">Id của Attribute cần xóa</param>
    /// <returns>True nếu xóa thành công, False nếu không tìm thấy</returns>
    bool DeleteAttribute(Guid id);

    /// <summary>
    /// Xóa Attribute theo Id (alias method).
    /// </summary>
    /// <param name="id">Id của Attribute cần xóa</param>
    void Delete(Guid id);

    #endregion

    #region Validation & Utilities

    /// <summary>
    /// Kiểm tra tên thuộc tính đã tồn tại chưa (loại trừ Id khi cập nhật).
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

    /// <summary>
    /// Lấy danh sách tên Attribute (unique) - Async.
    /// </summary>
    /// <returns>Danh sách tên unique</returns>
    Task<List<object>> GetUniqueNamesAsync();

    #endregion
}