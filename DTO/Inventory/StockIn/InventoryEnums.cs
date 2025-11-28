using System.ComponentModel;

namespace DTO.Inventory.StockIn
{
    /// <summary>
    /// Enum định nghĩa các loại nhập xuất kho
    /// </summary>
    public enum LoaiNhapXuatKhoEnum
    {
        /// <summary>
        /// Nhập hàng thương mại
        /// </summary>
        [Description("Nhập hàng thương mại")]
        NhapHangThuongMai = 1,

        /// <summary>
        /// Nhập thiết bị mượn/thuê
        /// </summary>
        [Description("Nhập thiết bị mượn/thuê")]
        NhapThietBiMuonThue = 2,

        /// <summary>
        /// Nhập thiết bị nội bộ
        /// </summary>
        [Description("Nhập thiết bị nội bộ")]
        NhapNoiBo = 3,

        /// <summary>
        /// Nhập lưu chuyển kho
        /// </summary>
        [Description("Nhập lưu chuyển kho")]
        NhapLuuChuyenKho = 4,

        /// <summary>
        /// Nhập hàng bảo hành  
        /// </summary>
        [Description("Nhập hàng bảo hành")]
        NhapHangBaoHanh = 5,

        /// <summary>
        /// Khác
        /// </summary>
        [Description("Khác")]
        Khac = 99
    }

    /// <summary>
    /// Enum định nghĩa các trạng thái phiếu nhập kho
    /// </summary>
    public enum TrangThaiPhieuNhapEnum
    {
        /// <summary>
        /// Tạo mới - Phiếu nhập kho vừa được tạo, chưa được xử lý
        /// </summary>
        [Description("Tạo mới")]
        TaoMoi = 1,

        /// <summary>
        /// Chờ duyệt - Phiếu nhập kho đang chờ được duyệt
        /// </summary>
        [Description("Chờ duyệt")]
        ChoDuyet = 2,

        /// <summary>
        /// Đã duyệt - Phiếu nhập kho đã được duyệt, sẵn sàng để nhập kho
        /// </summary>
        [Description("Đã duyệt")]
        DaDuyet = 3,

        /// <summary>
        /// Đang nhập kho - Phiếu nhập kho đang trong quá trình nhập kho
        /// </summary>
        [Description("Đang nhập kho")]
        DangNhapKho = 4,

        /// <summary>
        /// Đã nhập kho - Phiếu nhập kho đã hoàn tất việc nhập kho
        /// </summary>
        [Description("Đã nhập kho")]
        DaNhapKho = 5
    }
}