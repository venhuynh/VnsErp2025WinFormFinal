using DTO.DeviceAssetManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DTO.Inventory
{
    public class StockInOutDetailForUIDto
    {
        #region Properties - Thông tin cơ bản (map với DB)

        /// <summary>
        /// ID duy nhất của chi tiết phiếu xuất
        /// Map với: StockInOutDetail.Id
        /// </summary>
        [DisplayName("ID")]
        [Display(Order = -1)]
        public Guid Id { get; set; }

        /// <summary>
        /// ID phiếu xuất kho (Master)
        /// Map với: StockInOutDetail.StockInOutMasterId
        /// </summary>
        [DisplayName("ID Phiếu xuất")]
        [Display(Order = 0)]
        [Required(ErrorMessage = "ID phiếu xuất không được để trống")]
        public Guid StockInOutMasterId { get; set; }

        /// <summary>
        /// ID biến thể sản phẩm (ProductVariant)
        /// Map với: StockInOutDetail.ProductVariantId
        /// </summary>
        [DisplayName("ID Biến thể")]
        [Display(Order = 1)]
        [Required(ErrorMessage = "Biến thể sản phẩm không được để trống")]
        public Guid ProductVariantId { get; set; }

        /// <summary>
        /// Thứ tự dòng (dùng cho UI, không có trong DB)
        /// </summary>
        [DisplayName("STT")]
        [Display(Order = 2)]
        public int LineNumber { get; set; }

        #endregion

        #region Properties - Thông tin hàng hóa (hiển thị, lấy từ ProductVariant)

        /// <summary>
        /// Mã biến thể sản phẩm (để hiển thị)
        /// Lấy từ ProductVariant.VariantCode
        /// </summary>
        [DisplayName("Mã hàng")]
        [Display(Order = 10)]
        public string ProductVariantCode { get; set; }

        /// <summary>
        /// Tên biến thể sản phẩm (để hiển thị)
        /// Lấy từ ProductVariant.VariantFullName hoặc ProductService.Name
        /// </summary>
        [DisplayName("Tên hàng")]
        [Display(Order = 11)]
        public string ProductVariantName { get; set; }

        /// <summary>
        /// ID đơn vị tính (để hiển thị)
        /// Lấy từ ProductVariant.UnitId
        /// </summary>
        [DisplayName("ID ĐVT")]
        [Display(Order = 12)]
        public Guid? UnitOfMeasureId { get; set; }

        /// <summary>
        /// Mã đơn vị tính (để hiển thị)
        /// Lấy từ UnitOfMeasure.Code
        /// </summary>
        [DisplayName("Mã ĐVT")]
        [Display(Order = 13)]
        public string UnitOfMeasureCode { get; set; }

        /// <summary>
        /// Tên đơn vị tính (để hiển thị)
        /// Lấy từ UnitOfMeasure.Name
        /// </summary>
        [DisplayName("Đơn vị tính")]
        [Display(Order = 14)]
        public string UnitOfMeasureName { get; set; }

        #endregion

        #region Properties - Số lượng và giá (map với DB)

        /// <summary>
        /// Số lượng xuất
        /// Map với: StockInOutDetail.StockOutQty
        /// </summary>
        [DisplayName("SL xuất")]
        [Display(Order = 20)]
        [Required(ErrorMessage = "Số lượng xuất không được để trống")]
        [Range(0, double.MaxValue, ErrorMessage = "Số lượng xuất phải lớn hơn hoặc bằng 0")]
        public decimal StockOutQty { get; set; }

        /// <summary>
        /// Số lượng nhập (dùng cho phiếu nhập kho, mặc định = 0 cho phiếu xuất)
        /// Map với: StockInOutDetail.StockInQty
        /// </summary>
        [DisplayName("SL nhập")]
        [Display(Order = 21)]
        [Range(0, double.MaxValue, ErrorMessage = "Số lượng nhập phải lớn hơn hoặc bằng 0")]
        public decimal StockInQty { get; set; }

        /// <summary>
        /// Đơn giá
        /// Map với: StockInOutDetail.UnitPrice
        /// </summary>
        [DisplayName("Đơn giá")]
        [Display(Order = 22)]
        [Required(ErrorMessage = "Đơn giá không được để trống")]
        [Range(0, double.MaxValue, ErrorMessage = "Đơn giá phải lớn hơn hoặc bằng 0")]
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Tỷ lệ VAT (%)
        /// Map với: StockInOutDetail.Vat
        /// </summary>
        [DisplayName("VAT (%)")]
        [Display(Order = 23)]
        [Range(0, 100, ErrorMessage = "VAT phải từ 0 đến 100")]
        public decimal Vat { get; set; } = 8;

        /// <summary>
        /// Tổng tiền (chưa VAT) - Computed property
        /// Tính toán: StockOutQty * UnitPrice
        /// Map với: StockInOutDetail.TotalAmount (lưu vào DB khi save)
        /// </summary>
        [DisplayName("Tổng tiền")]
        [Display(Order = 25)]
        [Range(0, double.MaxValue, ErrorMessage = "Tổng tiền phải lớn hơn hoặc bằng 0")]
        public decimal TotalAmount => StockOutQty * UnitPrice;

        /// <summary>
        /// Số tiền VAT - Computed property
        /// Tính toán: TotalAmount * (Vat / 100)
        /// Map với: StockInOutDetail.VatAmount (lưu vào DB khi save)
        /// </summary>
        [DisplayName("Số tiền VAT")]
        [Display(Order = 24)]
        [Range(0, double.MaxValue, ErrorMessage = "Số tiền VAT phải lớn hơn hoặc bằng 0")]
        public decimal VatAmount => TotalAmount * (Vat / 100);

        /// <summary>
        /// Tổng tiền bao gồm VAT - Computed property
        /// Tính toán: TotalAmount + VatAmount
        /// Map với: StockInOutDetail.TotalAmountIncludedVat (lưu vào DB khi save)
        /// </summary>
        [DisplayName("Tổng tiền gồm VAT")]
        [Display(Order = 26)]
        [Range(0, double.MaxValue, ErrorMessage = "Tổng tiền gồm VAT phải lớn hơn hoặc bằng 0")]
        public decimal TotalAmountIncludedVat => TotalAmount + VatAmount;

        /// <summary>
        /// Tình trạng sản phẩm (dùng cho report)
        /// </summary>
        [DisplayName("Tình trạng")]
        [Display(Order = 27)]
        public string TinhTrangSanPham { get; set; } = "Bình thường";

        /// <summary>
        /// Danh sách thông tin bảo hành cho sản phẩm này
        /// </summary>
        [DisplayName("Thông tin bảo hành")]
        [Display(Order = 28)]
        public List<WarrantyDto> Warranties { get; set; } = [];

        /// <summary>
        /// Danh sách thông tin định danh thiết bị (Device) cho sản phẩm này
        /// </summary>
        [DisplayName("Thông tin định danh thiết bị")]
        [Display(Order = 29)]
        public List<DeviceDto> Devices { get; set; } = [];

        /// <summary>
        /// Thông tin chi tiết phiếu xuất dưới dạng HTML theo format DevExpress
        /// Sử dụng các tag HTML chuẩn của DevExpress: &lt;b&gt;, &lt;i&gt;, &lt;color&gt;, &lt;size&gt;
        /// Tham khảo: https://docs.devexpress.com/WindowsForms/4874/common-features/html-text-formatting
        /// </summary>
        [DisplayName("Thông tin HTML")]
        [Display(Order = 30)]
        [Description("Thông tin chi tiết phiếu xuất dưới dạng HTML")]
        public string FullNameHtml
        {
            get
            {
                var productVariantName = ProductVariantName ?? string.Empty;
                var productVariantCode = ProductVariantCode ?? string.Empty;
                var unitName = UnitOfMeasureName ?? string.Empty;
                var unitCode = UnitOfMeasureCode ?? string.Empty;

                // Format chuyên nghiệp với visual hierarchy rõ ràng
                // - Tên sản phẩm: font lớn, bold, màu xanh đậm (primary)
                // - Mã sản phẩm: font nhỏ hơn, màu xám
                // - Đơn vị tính: font nhỏ hơn, màu xám cho label, đen cho value
                // - Số lượng và giá: font nhỏ hơn, màu xám cho label, đen cho value

                var html = string.Empty;

                // Tên sản phẩm (nổi bật nhất)
                if (!string.IsNullOrWhiteSpace(productVariantName))
                {
                    html += $"<size=12><b><color='blue'>{productVariantName}</color></b></size>";
                }

                // Mã sản phẩm (nếu có)
                if (!string.IsNullOrWhiteSpace(productVariantCode))
                {
                    if (!string.IsNullOrWhiteSpace(productVariantName))
                    {
                        html += $" <size=9><color='#757575'>({productVariantCode})</color></size>";
                    }
                    else
                    {
                        html += $"<size=12><b><color='blue'>{productVariantCode}</color></b></size>";
                    }
                }

                if (!string.IsNullOrWhiteSpace(productVariantName) || !string.IsNullOrWhiteSpace(productVariantCode))
                {
                    html += "<br>";
                }

                // Đơn vị tính
                if (!string.IsNullOrWhiteSpace(unitCode) || !string.IsNullOrWhiteSpace(unitName))
                {
                    var unitDisplay = string.IsNullOrWhiteSpace(unitCode)
                        ? unitName
                        : string.IsNullOrWhiteSpace(unitName)
                            ? unitCode
                            : $"{unitCode} - {unitName}";

                    html += $"<size=9><color='#757575'>Đơn vị tính:</color></size> <size=10><color='#212121'><b>{unitDisplay}</b></color></size><br>";
                }

                // Số lượng xuất
                if (StockOutQty > 0)
                {
                    html += $"<size=9><color='#757575'>Số lượng:</color></size> <size=10><color='#212121'><b>{StockOutQty:N2}</b></color></size>";
                }

                // Đơn giá
                if (UnitPrice > 0)
                {
                    if (StockOutQty > 0)
                        html += " | ";
                    html += $"<size=9><color='#757575'>Đơn giá:</color></size> <size=10><color='#212121'><b>{UnitPrice:N0}</b></color></size>";
                }

                // VAT
                if (Vat > 0)
                {
                    if (StockOutQty > 0 || UnitPrice > 0)
                        html += " | ";
                    html += $"<size=9><color='#757575'>VAT:</color></size> <size=10><color='#212121'><b>{Vat}%</b></color></size>";
                }

                if (StockOutQty > 0 || UnitPrice > 0 || Vat > 0)
                {
                    html += "<br>";
                }

                // Tổng tiền
                if (TotalAmountIncludedVat > 0)
                {
                    html += $"<size=9><color='#757575'>Tổng tiền:</color></size> <size=10><color='#2196F3'><b>{TotalAmountIncludedVat:N0}</b></color></size>";
                }

                return html;
            }
        }

        #endregion
    }
}
