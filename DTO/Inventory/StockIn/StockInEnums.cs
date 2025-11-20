namespace DTO.Inventory.StockIn
{
    #region Stock In - Nhập Kho

    /// <summary>
    /// Enum định nghĩa các loại nhập kho
    /// </summary>
    public enum LoaiNhapKhoEnum
    {
        /// <summary>
        /// Nhập hàng thương mại - Nhân viên nhập hàng vào kho sau đó xuất hàng mang bán cho khách
        /// </summary>
        ThuongMai = 0,

        /// <summary>
        /// Nhập hàng khác (có thể mở rộng sau)
        /// </summary>
        Khac = 99
    }

    /// <summary>
    /// Enum định nghĩa các trạng thái của phiếu nhập kho
    /// </summary>
    public enum TrangThaiPhieuNhapEnum
    {
        /// <summary>
        /// Tạo mới - Phiếu nhập vừa được tạo, chưa được xử lý
        /// </summary>
        TaoMoi = 1,

        /// <summary>
        /// Chờ duyệt - Đang chờ người có thẩm quyền duyệt
        /// </summary>
        ChoDuyet = 2,

        /// <summary>
        /// Đã duyệt - Đã được duyệt, sẵn sàng nhập kho
        /// </summary>
        DaDuyet = 3,

        /// <summary>
        /// Đang nhập kho - Đang trong quá trình nhập kho
        /// </summary>
        DangNhapKho = 4,

        /// <summary>
        /// Đã nhập kho - Đã hoàn tất nhập kho
        /// </summary>
        DaNhapKho = 5,

        /// <summary>
        /// Đã hủy - Phiếu nhập đã bị hủy
        /// </summary>
        DaHuy = 99
    }

    /// <summary>
    /// Enum định nghĩa các phương thức nhập kho
    /// </summary>
    public enum PhuongThucNhapKhoEnum
    {
        /// <summary>
        /// Nhập trực tiếp - Nhập hàng trực tiếp vào kho
        /// </summary>
        NhapTrucTiep = 1,

        /// <summary>
        /// Nhập từ đơn hàng - Nhập hàng từ đơn đặt hàng đã có
        /// </summary>
        NhapTuDonHang = 2,

        /// <summary>
        /// Nhập từ nhà cung cấp - Nhập hàng trực tiếp từ nhà cung cấp
        /// </summary>
        NhapTuNhaCungCap = 3,

        /// <summary>
        /// Nhập theo đơn khách hàng - Nhập hàng để phục vụ đơn đặt hàng của khách (Buy-to-Order)
        /// </summary>
        NhapTheoDonKhachHang = 4,

        /// <summary>
        /// Nhập khác - Các phương thức nhập khác
        /// </summary>
        NhapKhac = 99
    }

    /// <summary>
    /// Enum định nghĩa các trạng thái hàng hóa khi nhập kho
    /// </summary>
    public enum TrangThaiHangEnum
    {
        /// <summary>
        /// Bình thường - Hàng hóa đạt chất lượng, nhập vào kho bình thường
        /// </summary>
        BinhThuong = 1,

        /// <summary>
        /// Bị lỗi - Hàng hóa bị lỗi, cần xử lý riêng
        /// </summary>
        BiLoi = 2,

        /// <summary>
        /// Cách ly - Hàng hóa cần được cách ly để kiểm tra
        /// </summary>
        CachLy = 3,

        /// <summary>
        /// Chờ trả nhà cung cấp - Hàng hóa cần trả về nhà cung cấp
        /// </summary>
        ChoTraNhaCungCap = 4,

        /// <summary>
        /// Đã trả nhà cung cấp - Hàng hóa đã được trả về nhà cung cấp
        /// </summary>
        DaTraNhaCungCap = 5
    }

    /// <summary>
    /// Enum định nghĩa các lý do nhập thừa so với đơn mua hàng (PO)
    /// Sử dụng khi người dùng có quyền "Over-Receive" và nhập số lượng vượt quá PO
    /// </summary>
    public enum LyDoNhapThuaEnum
    {
        /// <summary>
        /// Nhà cung cấp giao thừa - Nhà cung cấp giao hàng vượt quá số lượng đặt hàng
        /// </summary>
        NhaCungCapGiaoThua = 1,

        /// <summary>
        /// Đã được phê duyệt - Đã được người có thẩm quyền phê duyệt nhập thừa
        /// </summary>
        DaDuocPheDuyet = 2,

        /// <summary>
        /// Bù trừ thiếu lần trước - Bù trừ cho lần nhập thiếu trước đó
        /// </summary>
        BuTruThieuLanTruoc = 3,

        /// <summary>
        /// Hàng khuyến mãi - Hàng khuyến mãi từ nhà cung cấp
        /// </summary>
        HangKhuyenMai = 4,

        /// <summary>
        /// Lý do khác - Các lý do khác cần ghi chú
        /// </summary>
        LyDoKhac = 99
    }

    #endregion

    #region Purchase Order - Đơn Mua Hàng

    /// <summary>
    /// Enum định nghĩa các trạng thái của đơn mua hàng (Purchase Order - PO)
    /// </summary>
    public enum TrangThaiPoEnum
    {
        /// <summary>
        /// Tạo mới - PO vừa được tạo, chưa được phê duyệt
        /// </summary>
        TaoMoi = 1,

        /// <summary>
        /// Chờ duyệt - Đang chờ người có thẩm quyền phê duyệt
        /// </summary>
        ChoDuyet = 2,

        /// <summary>
        /// Đã duyệt - PO đã được phê duyệt, sẵn sàng gửi cho nhà cung cấp
        /// </summary>
        DaDuyet = 3,

        /// <summary>
        /// Đã gửi - PO đã được gửi cho nhà cung cấp
        /// </summary>
        DaGui = 4,

        /// <summary>
        /// Đã nhận một phần - Đã nhận một phần hàng hóa từ nhà cung cấp
        /// </summary>
        DaNhanMotPhan = 5,

        /// <summary>
        /// Đã nhận đủ - Đã nhận đủ hàng hóa theo PO
        /// </summary>
        DaNhanDu = 6,

        /// <summary>
        /// Đã hoàn thành - PO đã hoàn tất toàn bộ quy trình
        /// </summary>
        DaHoanThanh = 7,

        /// <summary>
        /// Đã hủy - PO đã bị hủy
        /// </summary>
        DaHuy = 99
    }

    #endregion

    #region Inventory Transaction - Giao Dịch Tồn Kho

    /// <summary>
    /// Enum định nghĩa các loại giao dịch tồn kho (Inventory Transaction)
    /// </summary>
    public enum LoaiGiaoDichTonKhoEnum
    {
        /// <summary>
        /// Nhập kho - Giao dịch tăng tồn kho (Goods Receipt)
        /// </summary>
        NhapKho = 1,

        /// <summary>
        /// Xuất kho - Giao dịch giảm tồn kho (Goods Issue)
        /// </summary>
        XuatKho = 2,

        /// <summary>
        /// Chuyển kho - Chuyển hàng từ kho này sang kho khác
        /// </summary>
        ChuyenKho = 3,

        /// <summary>
        /// Điều chỉnh - Điều chỉnh số lượng tồn kho (Inventory Adjustment)
        /// </summary>
        DieuChinh = 4,

        /// <summary>
        /// Trả hàng nhập - Trả hàng về nhà cung cấp
        /// </summary>
        TraHangNhap = 5,

        /// <summary>
        /// Nhận hàng trả - Nhận hàng trả từ khách hàng
        /// </summary>
        NhanHangTra = 6,

        /// <summary>
        /// Kiểm kê - Giao dịch từ kiểm kê kho
        /// </summary>
        KiemKe = 7
    }

    #endregion

    #region Warranty - Bảo Hành

    #endregion
}

