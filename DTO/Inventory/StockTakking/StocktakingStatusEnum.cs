using System.ComponentModel;

namespace DTO.Inventory.StockTakking
{
    /// <summary>
    /// Enum định nghĩa các trạng thái kiểm kho
    /// </summary>
    public enum StocktakingStatusEnum
    {
        /// <summary>
        /// Nháp (Draft)
        /// </summary>
        [Description("Nháp")]
        Draft = 0,

        /// <summary>
        /// Đang kiểm (In Progress)
        /// </summary>
        [Description("Đang kiểm")]
        InProgress = 1,

        /// <summary>
        /// Hoàn thành (Completed)
        /// </summary>
        [Description("Hoàn thành")]
        Completed = 2,

        /// <summary>
        /// Đã duyệt (Approved)
        /// </summary>
        [Description("Đã duyệt")]
        Approved = 3,

        /// <summary>
        /// Đã hủy (Cancelled)
        /// </summary>
        [Description("Đã hủy")]
        Cancelled = 4
    }
}
