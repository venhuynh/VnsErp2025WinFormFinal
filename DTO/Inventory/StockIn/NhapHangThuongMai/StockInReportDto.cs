using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Common.Helpers;

namespace DTO.Inventory.StockIn.NhapHangThuongMai;

/// <summary>
/// DTO cho report in phiếu nhập kho
/// Sử dụng StockInMasterDto và StockInDetailDto để tái sử dụng code
/// Chứa master data và detail data với cấu trúc phù hợp cho DevExpress XtraReport
/// </summary>
public class StockInReportDto
{
    #region Properties - Master Data

    /// <summary>
    /// Thông tin master phiếu nhập kho
    /// Sử dụng StockInMasterDto để tái sử dụng code
    /// </summary>
    public StockInMasterDto Master { get; set; }

    #endregion

    #region Properties - Detail Data

    /// <summary>
    /// Danh sách chi tiết nhập hàng
    /// Sử dụng StockInDetailDto để tái sử dụng code
    /// </summary>
    [DisplayName("Chi tiết nhập hàng")]
    public List<StockInDetailDto> ChiTietNhapHangNoiBos { get; set; } = new List<StockInDetailDto>();

    #endregion

    #region Properties - Computed Properties for Report Binding

    /// <summary>
    /// Số phiếu nhập kho (alias cho Master.StockInNumber)
    /// Dùng cho report binding: [SoPhieu]
    /// </summary>
    [DisplayName("Số phiếu")]
    public string SoPhieu => Master?.StockInNumber ?? string.Empty;

    /// <summary>
    /// Ngày nhập kho (alias cho Master.StockInDate)
    /// Dùng cho report binding: [NgayThang]
    /// </summary>
    [DisplayName("Ngày nhập")]
    public DateTime NgayThang => Master?.StockInDate ?? DateTime.MinValue;

    /// <summary>
    /// Loại nhập kho (alias cho Master.LoaiNhapXuatKho)
    /// Dùng cho report binding: [LoaiNhapXuatKho]
    /// </summary>
    [DisplayName("Loại nhập kho")]
    public LoaiNhapXuatKhoEnum LoaiNhapXuatKho => Master?.LoaiNhapXuatKho ?? default(LoaiNhapXuatKhoEnum);

    /// <summary>
    /// Tên loại nhập kho (hiển thị) - lấy từ Description attribute của enum
    /// Thêm "PHIẾU" phía trước và chuyển sang chữ in hoa
    /// Dùng cho report binding: [LoaiNhapXuatKhoName]
    /// </summary>
    [DisplayName("Tên loại nhập kho")]
    public string LoaiNhapXuatKhoName
    {
        get
        {
            var description = GetEnumDescription(Master?.LoaiNhapXuatKho ?? default(LoaiNhapXuatKhoEnum));
            if (string.IsNullOrWhiteSpace(description))
                return "PHIẾU";
            return "PHIẾU " + description.ToUpper();
        }
    }

    /// <summary>
    /// Tên kho nhập (alias cho Master.WarehouseName)
    /// Dùng cho report binding: [KhoNhap].[TenKho]
    /// </summary>
    [DisplayName("Tên kho")]
    public string KhoNhap_TenKho => Master?.WarehouseName ?? string.Empty;

    /// <summary>
    /// Tên người giao hàng (alias cho Master.NguoiGiaoHang)
    /// Dùng cho report binding: [NguoiGiaoHang].[FullName]
    /// </summary>
    [DisplayName("Tên khách hàng")]
    public string TenKhachHang_FullName => Master?.SupplierName ?? string.Empty;

    /// <summary>
    /// Tên người giao hàng (alias cho Master.NguoiGiaoHang)
    /// Dùng cho report binding: [NguoiGiaoHang].[FullName]
    /// </summary>
    [DisplayName("Người giao hàng")]
    public string NguoiGiaoHang_FullName => Master?.NguoiGiaoHang ?? string.Empty;

    /// <summary>
    /// Tên người nhận hàng (alias cho Master.NguoiNhanHang)
    /// Dùng cho report binding: [NguoiNhanHang].[FullName]
    /// </summary>
    [DisplayName("Người nhận hàng")]
    public string NguoiNhanHang_FullName => Master?.NguoiNhanHang ?? string.Empty;

    /// <summary>
    /// Tên người nhập kho (alias - cần bổ sung sau khi có authentication)
    /// Dùng cho report binding: [NguoiNhapXuat].[FullName]
    /// </summary>
    [DisplayName("Người nhập kho")]
    public string NguoiNhapXuat_FullName => string.Empty; // TODO: Lấy từ CreatedBy hoặc related entity

    /// <summary>
    /// Ghi chú - bao gồm ghi chú gốc và thông tin bảo hành đã nhóm theo thời gian
    /// Dùng cho report binding: [GhiChu]
    /// </summary>
    [DisplayName("Ghi chú")]
    public string GhiChu { get; set; } = string.Empty;

    /// <summary>
    /// Tổng tiền hàng hóa (chưa VAT) - alias cho Master.TotalAmount
    /// Dùng cho report binding: [TongTienHangHoa]
    /// </summary>
    [DisplayName("Tổng tiền hàng hóa")]
    public decimal TongTienHangHoa => Master?.TotalAmount ?? 0;

    /// <summary>
    /// Tổng tiền thuế VAT - alias cho Master.TotalVat
    /// Dùng cho report binding: [TongTienThueVAT]
    /// </summary>
    [DisplayName("Tổng tiền thuế VAT")]
    public decimal TongTienThueVAT => Master?.TotalVat ?? 0;

    /// <summary>
    /// Tổng tiền bao gồm thuế VAT - alias cho Master.TotalAmountIncludedVat
    /// Dùng cho report binding: [TongTienBaoGomThueVAT]
    /// </summary>
    [DisplayName("Tổng tiền bao gồm thuế VAT")]
    public decimal TongTienBaoGomThueVAT => Master?.TotalAmountIncludedVat ?? 0;

    /// <summary>
    /// Số tiền bằng chữ - chuyển đổi từ TongTienBaoGomThueVAT
    /// Thêm "Số tiền bằng chữ:" phía trước và in hoa chữ cái đầu tiên
    /// Dùng cho report binding: [SoTienBangChu]
    /// </summary>
    [DisplayName("Số tiền bằng chữ")]
    public string SoTienBangChu
    {
        get
        {
            var words = NumberToWordsHelper.ConvertToWords(TongTienBaoGomThueVAT);
            if (string.IsNullOrWhiteSpace(words))
                return "Số tiền bằng chữ: không đồng";
            
            // In hoa chữ cái đầu tiên
            if (words.Length > 0)
            {
                words = char.ToUpper(words[0]) + (words.Length > 1 ? words.Substring(1) : string.Empty);
            }
            
            return "Số tiền bằng chữ: " + words;
        }
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Lấy Description từ enum value
    /// </summary>
    /// <typeparam name="T">Kiểu enum</typeparam>
    /// <param name="enumValue">Giá trị enum</param>
    /// <returns>Description hoặc tên enum nếu không có Description</returns>
    private static string GetEnumDescription<T>(T enumValue) where T : Enum
    {
        var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
        if (fieldInfo == null) return enumValue.ToString();

        var descriptionAttribute = fieldInfo.GetCustomAttribute<System.ComponentModel.DescriptionAttribute>();
        return descriptionAttribute?.Description ?? enumValue.ToString();
    }

    #endregion
}
