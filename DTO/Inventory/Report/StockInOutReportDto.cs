using Common.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace DTO.Inventory.Report;

/// <summary>
/// DTO cho report in phiếu nhập xuất kho
/// Chứa master data và detail data với cấu trúc phù hợp cho DevExpress XtraReport
/// Dựa trên StockInOutMaster và StockInOutDetail entities
/// </summary>
public class StockInOutReportDto
{
    #region Properties - Master Data (từ StockInOutMaster)

    /// <summary>
    /// ID duy nhất của phiếu nhập xuất kho
    /// </summary>
    [DisplayName("ID")]
    public Guid Id { get; set; }

    /// <summary>
    /// Ngày nhập xuất kho
    /// </summary>
    [DisplayName("Ngày nhập xuất")]
    public DateTime StockInOutDate { get; set; }

    /// <summary>
    /// Số phiếu nhập xuất kho
    /// </summary>
    [DisplayName("Số phiếu")]
    public string VocherNumber { get; set; }

    /// <summary>
    /// Tên loại nhập xuất kho (hiển thị)
    /// </summary>
    [DisplayName("Loại nhập xuất")]
    public string LoaiNhapXuatKhoName { get; set; }

    /// <summary>
    /// Tên kho nhập xuất
    /// </summary>
    [DisplayName("Tên kho")]
    public string WarehouseName { get; set; }

    /// <summary>
    /// Số đơn mua hàng
    /// </summary>
    [DisplayName("Số PO")]
    public string PurchaseOrderNumber { get; set; }

    /// <summary>
    /// Tên khách hàng (hoặc nhà cung cấp)
    /// </summary>
    [DisplayName("Tên khách hàng")]
    public string CustomerName { get; set; }

    /// <summary>
    /// Địa chỉ (từ BusinessPartnerSite)
    /// </summary>
    [DisplayName("Địa chỉ")]
    public string CustomerAddress { get; set; }

    /// <summary>
    /// Địa chỉ đầy đủ của khách hàng (tính từ các thành phần địa chỉ)
    /// </summary>
    [DisplayName("Địa chỉ đầy đủ")]
    public string CustomerFullAddress
    {
        get
        {
            var addressParts = new List<string>();
            if (!string.IsNullOrWhiteSpace(CustomerAddress))
                addressParts.Add(CustomerAddress);
            
            return string.Join(", ", addressParts);
        }
    }

    /// <summary>
    /// Thông tin đầy đủ của khách hàng (kết hợp tên và địa chỉ)
    /// Dùng cho report binding: [CustomerFullInfo]
    /// </summary>
    [DisplayName("Thông tin khách hàng đầy đủ")]
    public string CustomerFullInfo
    {
        get
        {
            var infoParts = new List<string>();
            if (!string.IsNullOrWhiteSpace(CustomerName))
                infoParts.Add(CustomerName);
            if (!string.IsNullOrWhiteSpace(CustomerAddress))
                infoParts.Add(CustomerAddress);
            
            return string.Join(" - ", infoParts);
        }
    }

    /// <summary>
    /// Ghi chú
    /// </summary>
    [DisplayName("Ghi chú")]
    public string Notes { get; set; }

    /// <summary>
    /// Người nhận hàng
    /// </summary>
    [DisplayName("Người nhận hàng")]
    public string NguoiNhanHang { get; set; }

    /// <summary>
    /// Người giao hàng
    /// </summary>
    [DisplayName("Người giao hàng")]
    public string NguoiGiaoHang { get; set; }

    /// <summary>
    /// Tổng số lượng
    /// </summary>
    [DisplayName("Tổng số lượng")]
    public decimal TotalQuantity { get; set; }

    /// <summary>
    /// Tổng giá trị (chưa VAT)
    /// </summary>
    [DisplayName("Tổng tiền chưa VAT")]
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Tổng VAT
    /// </summary>
    [DisplayName("Tổng VAT")]
    public decimal TotalVat { get; set; }

    /// <summary>
    /// Tổng tiền bao gồm VAT
    /// </summary>
    [DisplayName("Tổng tiền bao gồm VAT")]
    public decimal TotalAmountIncludedVat { get; set; }

    /// <summary>
    /// Số tiền giảm giá
    /// </summary>
    [DisplayName("Số tiền giảm giá")]
    public decimal? DiscountAmount { get; set; }

    /// <summary>
    /// Tổng tiền sau giảm giá
    /// </summary>
    [DisplayName("Tổng tiền sau giảm giá")]
    public decimal? TotalAmountAfterDiscount { get; set; }

    #endregion

    #region Properties - Detail Data (từ StockInOutDetail)

    /// <summary>
    /// Danh sách chi tiết nhập xuất hàng
    /// </summary>
    [DisplayName("Chi tiết nhập xuất")]
    public List<StockInOutReportDetailDto> ChiTietNhapXuatKhos { get; set; } = [];

    #endregion

    #region Properties - Computed Properties for Report Binding

    /// <summary>
    /// Tổng tiền hàng hóa (chưa VAT) - alias cho TotalAmount
    /// Dùng cho report binding: [TongTienHangHoa]
    /// </summary>
    [DisplayName("Tổng tiền hàng hóa")]
    public decimal TongTienHangHoa => TotalAmount;

    /// <summary>
    /// Tổng tiền thuế VAT - alias cho TotalVat
    /// Dùng cho report binding: [TongTienThueVAT]
    /// </summary>
    [DisplayName("Tổng tiền thuế VAT")]
    public decimal TongTienThueVAT => TotalVat;

    /// <summary>
    /// Tổng tiền bao gồm thuế VAT - alias cho TotalAmountIncludedVat
    /// Dùng cho report binding: [TongTienBaoGomThueVAT]
    /// </summary>
    [DisplayName("Tổng tiền bao gồm thuế VAT")]
    public decimal TongTienBaoGomThueVAT => TotalAmountIncludedVat;

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

    /// <summary>
    /// Tổng số dòng chi tiết
    /// Dùng cho report binding: [TongSoDong]
    /// </summary>
    [DisplayName("Tổng số dòng")]
    public int TongSoDong => ChiTietNhapXuatKhos?.Count ?? 0;

    #endregion

    #region Properties - String Format Properties for Report Binding

    /// <summary>
    /// Tổng số lượng (dạng chuỗi)
    /// Dùng cho report binding: [TotalQuantityString]
    /// </summary>
    [DisplayName("Tổng số lượng (chuỗi)")]
    public string TotalQuantityString => FormatDecimal(TotalQuantity);

    /// <summary>
    /// Tổng tiền chưa VAT (dạng chuỗi)
    /// Dùng cho report binding: [TotalAmountString]
    /// </summary>
    [DisplayName("Tổng tiền chưa VAT (chuỗi)")]
    public string TotalAmountString => FormatDecimal(TotalAmount);

    /// <summary>
    /// Tổng VAT (dạng chuỗi)
    /// Dùng cho report binding: [TotalVatString]
    /// </summary>
    [DisplayName("Tổng VAT (chuỗi)")]
    public string TotalVatString => FormatDecimal(TotalVat);

    /// <summary>
    /// Tổng tiền bao gồm VAT (dạng chuỗi)
    /// Dùng cho report binding: [TotalAmountIncludedVatString]
    /// </summary>
    [DisplayName("Tổng tiền bao gồm VAT (chuỗi)")]
    public string TotalAmountIncludedVatString => FormatDecimal(TotalAmountIncludedVat);

    /// <summary>
    /// Số tiền giảm giá (dạng chuỗi)
    /// Dùng cho report binding: [DiscountAmountString]
    /// </summary>
    [DisplayName("Số tiền giảm giá (chuỗi)")]
    public string DiscountAmountString => FormatDecimal(DiscountAmount);

    /// <summary>
    /// Tổng tiền sau giảm giá (dạng chuỗi)
    /// Dùng cho report binding: [TotalAmountAfterDiscountString]
    /// </summary>
    [DisplayName("Tổng tiền sau giảm giá (chuỗi)")]
    public string TotalAmountAfterDiscountString => FormatDecimal(TotalAmountAfterDiscount);

    /// <summary>
    /// Tổng tiền hàng hóa (dạng chuỗi)
    /// Dùng cho report binding: [TongTienHangHoaString]
    /// </summary>
    [DisplayName("Tổng tiền hàng hóa (chuỗi)")]
    public string TongTienHangHoaString => FormatDecimal(TongTienHangHoa);

    /// <summary>
    /// Tổng tiền thuế VAT (dạng chuỗi)
    /// Dùng cho report binding: [TongTienThueVATString]
    /// </summary>
    [DisplayName("Tổng tiền thuế VAT (chuỗi)")]
    public string TongTienThueVATString => FormatDecimal(TongTienThueVAT);

    /// <summary>
    /// Tổng tiền bao gồm thuế VAT (dạng chuỗi)
    /// Dùng cho report binding: [TongTienBaoGomThueVATString]
    /// </summary>
    [DisplayName("Tổng tiền bao gồm thuế VAT (chuỗi)")]
    public string TongTienBaoGomThueVATString => FormatDecimal(TongTienBaoGomThueVAT);

    /// <summary>
    /// Tổng số dòng (dạng chuỗi)
    /// Dùng cho report binding: [TongSoDongString]
    /// </summary>
    [DisplayName("Tổng số dòng (chuỗi)")]
    public string TongSoDongString => TongSoDong.ToString("N0", CultureInfo.GetCultureInfo("vi-VN"));

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

        var descriptionAttribute = fieldInfo.GetCustomAttribute<DescriptionAttribute>();
        return descriptionAttribute?.Description ?? enumValue.ToString();
    }

    /// <summary>
    /// Format số decimal thành chuỗi: nếu là số nguyên thì không hiển thị phần thập phân, ngược lại hiển thị 2 chữ số thập phân
    /// </summary>
    /// <param name="value">Giá trị số</param>
    /// <returns>Chuỗi đã format theo chuẩn Việt Nam</returns>
    private static string FormatDecimal(decimal value)
    {
        var culture = CultureInfo.GetCultureInfo("vi-VN");
        // Kiểm tra nếu số là số nguyên (không có phần thập phân)
        if (value == Math.Truncate(value))
        {
            return value.ToString("N0", culture);
        }
        return value.ToString("N2", culture);
    }

    /// <summary>
    /// Format số decimal nullable thành chuỗi: nếu là số nguyên thì không hiển thị phần thập phân, ngược lại hiển thị 2 chữ số thập phân
    /// </summary>
    /// <param name="value">Giá trị số nullable</param>
    /// <returns>Chuỗi đã format theo chuẩn Việt Nam hoặc chuỗi rỗng nếu null</returns>
    private static string FormatDecimal(decimal? value)
    {
        if (!value.HasValue) return string.Empty;
        return FormatDecimal(value.Value);
    }

    #endregion
}

/// <summary>
/// DTO cho chi tiết phiếu nhập xuất kho trong report
/// Dựa trên StockInOutDetail entity
/// </summary>
public class StockInOutReportDetailDto
{
    #region Properties - Thông tin cơ bản (từ StockInOutDetail)

    /// <summary>
    /// Thứ tự dòng (dùng cho UI)
    /// </summary>
    [DisplayName("STT")]
    public int LineNumber { get; set; }

    #endregion

    #region Properties - Thông tin hàng hóa (từ ProductVariant)

    /// <summary>
    /// Tên biến thể sản phẩm
    /// </summary>
    [DisplayName("Tên hàng")]
    public string ProductVariantName { get; set; }

    /// <summary>
    /// Tên đơn vị tính
    /// </summary>
    [DisplayName("Đơn vị tính")]
    public string UnitOfMeasureName { get; set; }

    #endregion

    #region Properties - Số lượng và giá (từ StockInOutDetail)

    /// <summary>
    /// Số lượng nhập
    /// </summary>
    [DisplayName("Số lượng nhập")]
    public decimal StockInQty { get; set; }

    /// <summary>
    /// Số lượng xuất
    /// </summary>
    [DisplayName("Số lượng xuất")]
    public decimal StockOutQty { get; set; }

    /// <summary>
    /// Số lượng (tổng hợp - nhập hoặc xuất tùy loại phiếu)
    /// </summary>
    [DisplayName("Số lượng")]
    public decimal Quantity => StockInQty > 0 ? StockInQty : StockOutQty;

    /// <summary>
    /// Đơn giá
    /// </summary>
    [DisplayName("Đơn giá")]
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Thuế suất VAT (%)
    /// </summary>
    [DisplayName("Thuế suất VAT (%)")]
    public decimal Vat { get; set; }

    /// <summary>
    /// Số tiền VAT
    /// </summary>
    [DisplayName("Số tiền VAT")]
    public decimal VatAmount { get; set; }

    /// <summary>
    /// Thành tiền (chưa VAT)
    /// </summary>
    [DisplayName("Thành tiền")]
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Thành tiền bao gồm VAT
    /// </summary>
    [DisplayName("Thành tiền bao gồm VAT")]
    public decimal TotalAmountIncludedVat { get; set; }

    /// <summary>
    /// Số tiền giảm giá
    /// </summary>
    [DisplayName("Số tiền giảm giá")]
    public decimal? DiscountAmount { get; set; }

    /// <summary>
    /// Phần trăm giảm giá
    /// </summary>
    [DisplayName("Phần trăm giảm giá")]
    public decimal? DiscountPercentage { get; set; }

    /// <summary>
    /// Thành tiền sau giảm giá
    /// </summary>
    [DisplayName("Thành tiền sau giảm giá")]
    public decimal? TotalAmountAfterDiscount { get; set; }

    /// <summary>
    /// Ghi chú chi tiết
    /// </summary>
    [DisplayName("Ghi chú")]
    public string GhiChu { get; set; }

    #endregion

    #region Properties - Computed Properties for Report Binding

    // Lưu ý: Các computed properties (alias) đã được loại bỏ để tránh trùng lặp trong Field List của DevExpress Report Designer
    // Sử dụng trực tiếp các property gốc với tên property (ví dụ: [LineNumber], [ProductVariantName], [UnitPrice], v.v.)
    // hoặc sử dụng DisplayName trong report binding (ví dụ: [STT], [Tên hàng], [Đơn giá], v.v.)

    #endregion

    #region Properties - String Format Properties for Report Binding

    /// <summary>
    /// Thứ tự dòng (dạng chuỗi)
    /// Dùng cho report binding: [LineNumberString]
    /// </summary>
    [DisplayName("STT (chuỗi)")]
    public string LineNumberString => LineNumber.ToString("N0", CultureInfo.GetCultureInfo("vi-VN"));

    /// <summary>
    /// Số lượng nhập (dạng chuỗi)
    /// Dùng cho report binding: [StockInQtyString]
    /// </summary>
    [DisplayName("Số lượng nhập (chuỗi)")]
    public string StockInQtyString => FormatDecimal(StockInQty);

    /// <summary>
    /// Số lượng xuất (dạng chuỗi)
    /// Dùng cho report binding: [StockOutQtyString]
    /// </summary>
    [DisplayName("Số lượng xuất (chuỗi)")]
    public string StockOutQtyString => FormatDecimal(StockOutQty);

    /// <summary>
    /// Số lượng (dạng chuỗi)
    /// Dùng cho report binding: [QuantityString]
    /// </summary>
    [DisplayName("Số lượng (chuỗi)")]
    public string QuantityString => FormatDecimal(Quantity);

    /// <summary>
    /// Đơn giá (dạng chuỗi)
    /// Dùng cho report binding: [UnitPriceString]
    /// </summary>
    [DisplayName("Đơn giá (chuỗi)")]
    public string UnitPriceString => FormatDecimal(UnitPrice);

    /// <summary>
    /// Thuế suất VAT (dạng chuỗi)
    /// Dùng cho report binding: [VatString]
    /// </summary>
    [DisplayName("Thuế suất VAT (chuỗi)")]
    public string VatString => FormatDecimal(Vat);

    /// <summary>
    /// Số tiền VAT (dạng chuỗi)
    /// Dùng cho report binding: [VatAmountString]
    /// </summary>
    [DisplayName("Số tiền VAT (chuỗi)")]
    public string VatAmountString => FormatDecimal(VatAmount);

    /// <summary>
    /// Thành tiền (dạng chuỗi)
    /// Dùng cho report binding: [TotalAmountString]
    /// </summary>
    [DisplayName("Thành tiền (chuỗi)")]
    public string TotalAmountString => FormatDecimal(TotalAmount);

    /// <summary>
    /// Thành tiền bao gồm VAT (dạng chuỗi)
    /// Dùng cho report binding: [TotalAmountIncludedVatString]
    /// </summary>
    [DisplayName("Thành tiền bao gồm VAT (chuỗi)")]
    public string TotalAmountIncludedVatString => FormatDecimal(TotalAmountIncludedVat);

    /// <summary>
    /// Số tiền giảm giá (dạng chuỗi)
    /// Dùng cho report binding: [DiscountAmountString]
    /// </summary>
    [DisplayName("Số tiền giảm giá (chuỗi)")]
    public string DiscountAmountString => FormatDecimal(DiscountAmount);

    /// <summary>
    /// Phần trăm giảm giá (dạng chuỗi)
    /// Dùng cho report binding: [DiscountPercentageString]
    /// </summary>
    [DisplayName("Phần trăm giảm giá (chuỗi)")]
    public string DiscountPercentageString => FormatDecimal(DiscountPercentage);

    /// <summary>
    /// Thành tiền sau giảm giá (dạng chuỗi)
    /// Dùng cho report binding: [TotalAmountAfterDiscountString]
    /// </summary>
    [DisplayName("Thành tiền sau giảm giá (chuỗi)")]
    public string TotalAmountAfterDiscountString => FormatDecimal(TotalAmountAfterDiscount);

    #endregion

    #region Helper Methods

    /// <summary>
    /// Format số decimal thành chuỗi: nếu là số nguyên thì không hiển thị phần thập phân, ngược lại hiển thị 2 chữ số thập phân
    /// </summary>
    /// <param name="value">Giá trị số</param>
    /// <returns>Chuỗi đã format theo chuẩn Việt Nam</returns>
    private static string FormatDecimal(decimal value)
    {
        var culture = CultureInfo.GetCultureInfo("vi-VN");
        // Kiểm tra nếu số là số nguyên (không có phần thập phân)
        if (value == Math.Truncate(value))
        {
            return value.ToString("N0", culture);
        }
        return value.ToString("N2", culture);
    }

    /// <summary>
    /// Format số decimal nullable thành chuỗi: nếu là số nguyên thì không hiển thị phần thập phân, ngược lại hiển thị 2 chữ số thập phân
    /// </summary>
    /// <param name="value">Giá trị số nullable</param>
    /// <returns>Chuỗi đã format theo chuẩn Việt Nam hoặc chuỗi rỗng nếu null</returns>
    private static string FormatDecimal(decimal? value)
    {
        if (!value.HasValue) return string.Empty;
        return FormatDecimal(value.Value);
    }

    #endregion
}
