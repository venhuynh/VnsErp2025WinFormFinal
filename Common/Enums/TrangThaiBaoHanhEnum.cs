using System.ComponentModel;

namespace Common.Enums
{
    /// <summary>
    /// Enum định nghĩa các trạng thái bảo hành
    /// </summary>
    public enum TrangThaiBaoHanhEnum
    {
        /// <summary>
        /// Chờ xử lý - Yêu cầu bảo hành đã được tạo, chờ xử lý
        /// </summary>
        [Description("Chờ xử lý")]
        ChoXuLy = 1,

        /// <summary>
        /// Đang bảo hành - Sản phẩm đang trong quá trình bảo hành
        /// </summary>
        [Description("Đang bảo hành")]
        DangBaoHanh = 2,

        /// <summary>
        /// Đã hoàn thành - Bảo hành đã hoàn tất
        /// </summary>
        [Description("Đã hoàn thành")]
        DaHoanThanh = 3,

        /// <summary>
        /// Đã từ chối - Yêu cầu bảo hành bị từ chối
        /// </summary>
        [Description("Đã từ chối")]
        DaTuChoi = 4,

        /// <summary>
        /// Đã hủy - Yêu cầu bảo hành đã bị hủy
        /// </summary>
        [Description("Đã hủy")]
        DaHuy = 99
    }
}

