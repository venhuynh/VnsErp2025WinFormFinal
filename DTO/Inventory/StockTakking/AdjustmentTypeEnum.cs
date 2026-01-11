using System;
using System.ComponentModel;

namespace DTO.Inventory.StockTakking
{
    /// <summary>
    /// Enum định nghĩa các loại điều chỉnh tồn kho sau kiểm kho
    /// </summary>
    public enum AdjustmentTypeEnum
    {
        /// <summary>
        /// Tăng (thừa hàng) - Khi CountedQuantity > SystemQuantity
        /// </summary>
        [Description("Tăng")]
        Increase = 0,

        /// <summary>
        /// Giảm (thiếu hàng) - Khi CountedQuantity < SystemQuantity
        /// </summary>
        [Description("Giảm")]
        Decrease = 1,

        /// <summary>
        /// Không điều chỉnh (nếu chênh lệch trong phạm vi cho phép)
        /// </summary>
        [Description("Không điều chỉnh")]
        NoAdjustment = 2
    }

    /// <summary>
    /// Extension methods cho AdjustmentTypeEnum
    /// </summary>
    public static class AdjustmentTypeEnumExtensions
    {
        /// <summary>
        /// Lấy màu HTML cho loại điều chỉnh (theo format DevExpress)
        /// </summary>
        /// <param name="adjustmentType">Loại điều chỉnh</param>
        /// <returns>Tên màu HTML</returns>
        public static string GetColor(this AdjustmentTypeEnum? adjustmentType)
        {
            if (!adjustmentType.HasValue)
                return "gray"; // Màu xám cho giá trị null

            return adjustmentType.Value switch
            {
                AdjustmentTypeEnum.Increase => "green",      // Màu xanh lá (tăng/thừa hàng)
                AdjustmentTypeEnum.Decrease => "red",        // Màu đỏ (giảm/thiếu hàng)
                AdjustmentTypeEnum.NoAdjustment => "orange", // Màu cam (không điều chỉnh)
                _ => "gray"                                   // Màu xám mặc định
            };
        }

        /// <summary>
        /// Lấy mô tả của loại điều chỉnh
        /// </summary>
        /// <param name="adjustmentType">Loại điều chỉnh</param>
        /// <returns>Mô tả</returns>
        public static string GetDescription(this AdjustmentTypeEnum? adjustmentType)
        {
            if (!adjustmentType.HasValue)
                return "Chưa xác định";

            return adjustmentType.Value switch
            {
                AdjustmentTypeEnum.Increase => "Tăng",
                AdjustmentTypeEnum.Decrease => "Giảm",
                AdjustmentTypeEnum.NoAdjustment => "Không điều chỉnh",
                _ => "Không xác định"
            };
        }

        /// <summary>
        /// Chuyển đổi từ int? sang AdjustmentTypeEnum?
        /// </summary>
        /// <param name="value">Giá trị int</param>
        /// <returns>AdjustmentTypeEnum?</returns>
        public static AdjustmentTypeEnum? ToAdjustmentTypeEnum(this int? value)
        {
            if (!value.HasValue)
                return null;

            if (Enum.IsDefined(typeof(AdjustmentTypeEnum), value.Value))
                return (AdjustmentTypeEnum)value.Value;

            return null;
        }

        /// <summary>
        /// Chuyển đổi từ AdjustmentTypeEnum? sang int?
        /// </summary>
        /// <param name="adjustmentType">AdjustmentTypeEnum?</param>
        /// <returns>int?</returns>
        public static int? ToInt(this AdjustmentTypeEnum? adjustmentType)
        {
            if (!adjustmentType.HasValue)
                return null;

            return (int)adjustmentType.Value;
        }
    }
}
