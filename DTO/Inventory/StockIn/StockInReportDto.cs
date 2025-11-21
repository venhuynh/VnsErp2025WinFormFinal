using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DTO.Inventory.StockIn;

/// <summary>
/// DTO cho report in phiếu nhập kho
/// Chứa master data và detail data với cấu trúc phù hợp cho DevExpress XtraReport
/// </summary>
public class StockInReportDto
{
    #region Properties - Master Data

    /// <summary>
    /// Số phiếu nhập kho
    /// </summary>
    [DisplayName("Số phiếu")]
    public string SoPhieu { get; set; }

    /// <summary>
    /// Ngày nhập kho
    /// </summary>
    [DisplayName("Ngày nhập")]
    public DateTime NgayThang { get; set; }

    /// <summary>
    /// Thông tin người giao hàng
    /// </summary>
    public NguoiGiaoHangDto NhanHangTu { get; set; }

    /// <summary>
    /// Thông tin người nhập kho
    /// </summary>
    public NguoiNhapXuatDto NguoiNhapXuat { get; set; }

    /// <summary>
    /// Thông tin kho nhập
    /// </summary>
    public KhoNhapDto KhoNhap { get; set; }

    #endregion

    #region Properties - Detail Data

    /// <summary>
    /// Danh sách chi tiết nhập hàng nội bộ
    /// </summary>
    [DisplayName("Chi tiết nhập hàng")]
    public List<ChiTietNhapHangNoiBoDto> ChiTietNhapHangNoiBos { get; set; } = new List<ChiTietNhapHangNoiBoDto>();

    #endregion
}

/// <summary>
/// DTO cho thông tin người giao hàng
/// </summary>
public class NguoiGiaoHangDto
{
    [DisplayName("Họ tên")]
    public string FullName { get; set; }
}

/// <summary>
/// DTO cho thông tin người nhập/xuất kho
/// </summary>
public class NguoiNhapXuatDto
{
    [DisplayName("Họ tên")]
    public string FullName { get; set; }
}

/// <summary>
/// DTO cho thông tin kho nhập
/// </summary>
public class KhoNhapDto
{
    [DisplayName("Tên kho")]
    public string FullProductNameName { get; set; }
}

/// <summary>
/// DTO cho chi tiết nhập hàng nội bộ
/// </summary>
public class ChiTietNhapHangNoiBoDto
{
    /// <summary>
    /// Thông tin sản phẩm
    /// </summary>
    public SanPhamDto SanPham { get; set; }

    /// <summary>
    /// Đơn vị tính
    /// </summary>
    [DisplayName("Đơn vị tính")]
    public string DonViTinh { get; set; }

    /// <summary>
    /// Số lượng
    /// </summary>
    [DisplayName("Số lượng")]
    public decimal SoLuong { get; set; }

    /// <summary>
    /// Tình trạng sản phẩm
    /// </summary>
    [DisplayName("Tình trạng")]
    public string TinhTrangSanPham { get; set; }
}

/// <summary>
/// DTO cho thông tin sản phẩm
/// </summary>
public class SanPhamDto
{
    [DisplayName("Tên sản phẩm")]
    public string ProductName { get; set; }
}

