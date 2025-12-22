using Dal.DataContext;
using System;
using System.Collections.Generic;

namespace Dal.DataAccess.Interfaces.Inventory.InventoryManagement;

public interface IWarrantyRepository
{
    /// <summary>
    /// Lấy danh sách bảo hành theo StockInOutMasterId
    /// </summary>
    /// <param name="stockInOutMasterId">ID phiếu nhập/xuất kho</param>
    /// <returns>Danh sách Warranty entities</returns>
    List<Warranty> GetByStockInOutMasterId(Guid stockInOutMasterId);

    /// <summary>
    /// Query danh sách bảo hành với filter theo từ khóa và khoảng thời gian
    /// </summary>
    /// <param name="fromDate">Từ ngày (nullable)</param>
    /// <param name="toDate">Đến ngày (nullable)</param>
    /// <param name="keyword">Từ khóa tìm kiếm (tìm trong Device properties: SerialNumber, IMEI, MACAddress, AssetTag, LicenseKey, ProductVariantName, CustomerName, Notes)</param>
    /// <returns>Danh sách Warranty entities</returns>
    List<Warranty> Query(DateTime? fromDate, DateTime? toDate, string keyword);

    /// <summary>
    /// Lưu hoặc cập nhật bảo hành
    /// </summary>
    /// <param name="warranty">Entity bảo hành cần lưu</param>
    void SaveOrUpdate(Warranty warranty);

    /// <summary>
    /// Xóa bảo hành
    /// </summary>
    /// <param name="warrantyId">ID bảo hành cần xóa</param>
    void Delete(Guid warrantyId);

    /// <summary>
    /// Tìm Warranty theo DeviceId
    /// </summary>
    /// <param name="deviceId">ID của Device cần tìm</param>
    /// <returns>Warranty entity nếu tìm thấy, null nếu không tìm thấy</returns>
    Warranty FindByDeviceId(Guid deviceId);

    /// <summary>
    /// Tìm Warranty theo thông tin Device (SerialNumber, IMEI, MACAddress, AssetTag, LicenseKey)
    /// </summary>
    /// <param name="deviceInfo">Thông tin Device cần tìm (SerialNumber, IMEI, MACAddress, AssetTag, hoặc LicenseKey)</param>
    /// <returns>Warranty entity nếu tìm thấy, null nếu không tìm thấy</returns>
    Warranty FindByDeviceInfo(string deviceInfo);
}