using System.ComponentModel;

namespace Common.Enums
{
    /// <summary>
    /// Enum định nghĩa các kiểu bảo hành
    /// </summary>
    public enum LoaiBaoHanhEnum
    {
        /// <summary>
        /// Bảo hành từ Nhà cung cấp -> VNS
        /// </summary>
        [Description("NCC -> VNS")]
        NCCToVNS = 0,

        /// <summary>
        /// Bảo hành từ VNS -> Khách hàng
        /// </summary>
        [Description("VNS -> Khách hàng")]
        VNSToKhachHang = 1
    }
}

