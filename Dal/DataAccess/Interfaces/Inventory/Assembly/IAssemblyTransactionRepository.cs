using Dal.DataContext;
using System;
using System.Collections.Generic;

namespace Dal.DataAccess.Interfaces.Inventory.Assembly;

/// <summary>
/// Interface cho Repository xử lý dữ liệu AssemblyTransaction (Lịch sử lắp ráp)
/// </summary>
public interface IAssemblyTransactionRepository
{
    /// <summary>
    /// Lưu giao dịch lắp ráp
    /// </summary>
    /// <param name="transaction">Entity AssemblyTransaction cần lưu</param>
    void Save(AssemblyTransaction transaction);

    /// <summary>
    /// Lấy giao dịch lắp ráp theo ID
    /// </summary>
    /// <param name="id">ID của giao dịch</param>
    /// <returns>AssemblyTransaction entity hoặc null nếu không tìm thấy</returns>
    AssemblyTransaction GetById(Guid id);

    /// <summary>
    /// Lấy danh sách giao dịch lắp ráp theo ProductVariantId
    /// </summary>
    /// <param name="productVariantId">ID sản phẩm</param>
    /// <returns>Danh sách AssemblyTransaction entities</returns>
    List<AssemblyTransaction> GetByProductVariantId(Guid productVariantId);

    /// <summary>
    /// Lấy danh sách giao dịch lắp ráp theo khoảng thời gian
    /// </summary>
    /// <param name="fromDate">Từ ngày</param>
    /// <param name="toDate">Đến ngày</param>
    /// <returns>Danh sách AssemblyTransaction entities</returns>
    List<AssemblyTransaction> GetByDateRange(DateTime fromDate, DateTime toDate);
}

