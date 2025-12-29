using Dal.DataContext;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dal.DataAccess.Implementations.Inventory.StockIn;
using DTO.Inventory;

namespace Dal.DataAccess.Interfaces.Inventory.StockIn;

/// <summary>
/// Interface cho Repository xử lý dữ liệu StockIn (Phiếu nhập kho)
/// </summary>
public interface IStockInRepository
{
    
    StockInOutMasterForUIDto GetStockInOutMasterForUIDtoById(Guid stockInOutMasterId);
}