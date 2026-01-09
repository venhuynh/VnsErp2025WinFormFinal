using System.ComponentModel;

namespace DTO.Inventory.StockTakking
{
    /// <summary>
    /// Enum định nghĩa các loại kiểm kho
    /// </summary>
    public enum StocktakingTypeEnum
    {
        /// <summary>
        /// Kiểm kho định kỳ (theo lịch)
        /// </summary>
        [Description("Kiểm kho định kỳ")]
        Periodic = 0,

        /// <summary>
        /// Kiểm kho đột xuất
        /// </summary>
        [Description("Kiểm kho đột xuất")]
        AdHoc = 1,

        /// <summary>
        /// Kiểm kho toàn bộ kho
        /// </summary>
        [Description("Kiểm kho toàn bộ kho")]
        FullWarehouse = 2,

        /// <summary>
        /// Kiểm kho theo danh sách sản phẩm cụ thể
        /// </summary>
        [Description("Kiểm kho theo danh sách sản phẩm")]
        ByProductList = 3
    }
}
