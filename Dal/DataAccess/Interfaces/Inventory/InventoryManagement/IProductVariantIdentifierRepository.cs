using System;
using System.Collections.Generic;
using DTO.Inventory.InventoryManagement;

namespace Dal.DataAccess.Interfaces.Inventory.InventoryManagement;

/// <summary>
/// Repository interface cho ProductVariantIdentifier
/// Quản lý các thao tác CRUD với bảng ProductVariantIdentifier (Định danh biến thể sản phẩm)
/// </summary>
public interface IProductVariantIdentifierRepository
{
    #region ========== READ OPERATIONS ==========

    /// <summary>
    /// Lấy tất cả ProductVariantIdentifier
    /// </summary>
    /// <returns>Danh sách tất cả ProductVariantIdentifierDto</returns>
    List<ProductVariantIdentifierDto> GetAll();

    /// <summary>
    /// Lấy ProductVariantIdentifier theo ID
    /// </summary>
    /// <param name="id">ID của ProductVariantIdentifier</param>
    /// <returns>ProductVariantIdentifierDto hoặc null</returns>
    ProductVariantIdentifierDto GetById(Guid id);

    /// <summary>
    /// Lấy danh sách ProductVariantIdentifier theo ProductVariantId
    /// </summary>
    /// <param name="productVariantId">ID biến thể sản phẩm</param>
    /// <returns>Danh sách ProductVariantIdentifierDto</returns>
    List<ProductVariantIdentifierDto> GetByProductVariantId(Guid productVariantId);

    /// <summary>
    /// Tìm ProductVariantIdentifier theo giá trị định danh (SerialNumber, Barcode, QRCode, SKU, RFID, MACAddress, IMEI, AssetTag, LicenseKey, UPC, EAN, ISBN)
    /// </summary>
    /// <param name="identifierValue">Giá trị định danh cần tìm</param>
    /// <returns>ProductVariantIdentifierDto nếu tìm thấy, null nếu không tìm thấy</returns>
    ProductVariantIdentifierDto FindByIdentifier(string identifierValue);

    /// <summary>
    /// Tìm ProductVariantIdentifier theo SerialNumber
    /// </summary>
    /// <param name="serialNumber">Số serial cần tìm</param>
    /// <returns>ProductVariantIdentifierDto nếu tìm thấy, null nếu không tìm thấy</returns>
    ProductVariantIdentifierDto FindBySerialNumber(string serialNumber);

    /// <summary>
    /// Tìm ProductVariantIdentifier theo Barcode
    /// </summary>
    /// <param name="barcode">Mã vạch cần tìm</param>
    /// <returns>ProductVariantIdentifierDto nếu tìm thấy, null nếu không tìm thấy</returns>
    ProductVariantIdentifierDto FindByBarcode(string barcode);

    /// <summary>
    /// Tìm ProductVariantIdentifier theo SKU
    /// </summary>
    /// <param name="sku">Mã SKU cần tìm</param>
    /// <returns>ProductVariantIdentifierDto nếu tìm thấy, null nếu không tìm thấy</returns>
    ProductVariantIdentifierDto FindBySKU(string sku);

    /// <summary>
    /// Lấy danh sách ProductVariantIdentifier theo tình trạng (Status)
    /// </summary>
    /// <param name="status">Tình trạng cần tìm</param>
    /// <returns>Danh sách ProductVariantIdentifierDto</returns>
    List<ProductVariantIdentifierDto> GetByStatus(ProductVariantIdentifierStatusEnum status);

    #endregion

    #region ========== CREATE/UPDATE OPERATIONS ==========

    /// <summary>
    /// Lưu hoặc cập nhật ProductVariantIdentifier
    /// </summary>
    /// <param name="dto">ProductVariantIdentifierDto cần lưu</param>
    /// <returns>ProductVariantIdentifierDto đã được lưu</returns>
    ProductVariantIdentifierDto SaveOrUpdate(ProductVariantIdentifierDto dto);

    #endregion

    #region ========== DELETE OPERATIONS ==========

    /// <summary>
    /// Xóa ProductVariantIdentifier theo ID
    /// </summary>
    /// <param name="id">ID của ProductVariantIdentifier cần xóa</param>
    /// <returns>True nếu xóa thành công, False nếu không tìm thấy</returns>
    bool Delete(Guid id);

    #endregion
}
