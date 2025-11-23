using Dal.DataContext;
using System;
using System.Collections.Generic;

namespace Dal.DataAccess.Interfaces.Inventory.InventoryManagement;

/// <summary>
/// Interface cho Repository quản lý StockInOutDetail
/// </summary>
public interface IStockInOutDetailRepository
{
    /// <summary>
    /// Query lịch sử sản phẩm nhập xuất kho với filter
    /// Tìm kiếm trong StockInOutDetail và các bảng liên quan (ProductVariant, ProductService)
    /// </summary>
    /// <param name="fromDate">Từ ngày</param>
    /// <param name="toDate">Đến ngày</param>
    /// <param name="keyword">Từ khóa tìm kiếm (tìm trong mã hàng, tên hàng, số phiếu)</param>
    /// <returns>Danh sách StockInOutDetail entities</returns>
    List<StockInOutDetail> QueryProductHistory(DateTime fromDate, DateTime toDate, string keyword = null);
}