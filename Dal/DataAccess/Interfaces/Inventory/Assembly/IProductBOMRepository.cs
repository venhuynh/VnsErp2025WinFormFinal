using Dal.DataContext;
using System;
using System.Collections.Generic;

namespace Dal.DataAccess.Interfaces.Inventory.Assembly;

/// <summary>
/// Interface cho Repository xử lý dữ liệu ProductBOM (Bill of Materials)
/// </summary>
public interface IProductBOMRepository
{
    /// <summary>
    /// Lưu hoặc cập nhật BOM
    /// </summary>
    /// <param name="bom">Entity ProductBOM cần lưu</param>
    void SaveOrUpdate(ProductBOM bom);

    /// <summary>
    /// Xóa BOM
    /// </summary>
    /// <param name="id">ID của BOM cần xóa</param>
    void Delete(Guid id);

    /// <summary>
    /// Lấy BOM theo ID
    /// </summary>
    /// <param name="id">ID của BOM</param>
    /// <returns>ProductBOM entity hoặc null nếu không tìm thấy</returns>
    ProductBOM GetById(Guid id);

    /// <summary>
    /// Lấy danh sách BOM theo ProductVariantId (sản phẩm hoàn chỉnh)
    /// </summary>
    /// <param name="productVariantId">ID sản phẩm hoàn chỉnh</param>
    /// <returns>Danh sách ProductBOM entities</returns>
    List<ProductBOM> GetByProductVariantId(Guid productVariantId);

    /// <summary>
    /// Lấy tất cả BOM đang hoạt động
    /// </summary>
    /// <returns>Danh sách ProductBOM entities</returns>
    List<ProductBOM> GetAllActive();

    /// <summary>
    /// Kiểm tra BOM đã tồn tại chưa (theo ProductVariantId và ComponentVariantId)
    /// </summary>
    /// <param name="productVariantId">ID sản phẩm hoàn chỉnh</param>
    /// <param name="componentVariantId">ID linh kiện</param>
    /// <param name="excludeId">ID cần loại trừ (khi update)</param>
    /// <returns>True nếu đã tồn tại</returns>
    bool Exists(Guid productVariantId, Guid componentVariantId, Guid? excludeId = null);
}

