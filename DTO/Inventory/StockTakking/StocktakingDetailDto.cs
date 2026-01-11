using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DTO.Inventory.StockTakking
{
    /// <summary>
    /// Data Transfer Object cho StocktakingDetail entity
    /// Quản lý chi tiết kiểm kho cho từng sản phẩm
    /// </summary>
    public class StocktakingDetailDto
    {
        #region Properties - Thông tin cơ bản

        /// <summary>
        /// ID duy nhất của chi tiết kiểm kho
        /// </summary>
        [DisplayName("ID")]
        [Display(Order = -1)]
        public Guid Id { get; set; }

        /// <summary>
        /// ID phiếu kiểm kho
        /// </summary>
        [DisplayName("ID Phiếu kiểm kho")]
        [Display(Order = 0)]
        [Required(ErrorMessage = "ID phiếu kiểm kho không được để trống")]
        public Guid StocktakingMasterId { get; set; }

        /// <summary>
        /// ID biến thể sản phẩm
        /// </summary>
        [DisplayName("ID Biến thể sản phẩm")]
        [Display(Order = 1)]
        [Required(ErrorMessage = "ID biến thể sản phẩm không được để trống")]
        public Guid ProductVariantId { get; set; }

        /// <summary>
        /// Tên biến thể sản phẩm (để hiển thị)
        /// </summary>
        [DisplayName("Tên sản phẩm")]
        [Display(Order = 2)]
        public string ProductVariantName { get; set; }

        /// <summary>
        /// Mã sản phẩm (để hiển thị)
        /// </summary>
        [DisplayName("Mã sản phẩm")]
        [Display(Order = 3)]
        public string ProductVariantCode { get; set; }

        /// <summary>
        /// Đơn vị tính của biến thể sản phẩm (để hiển thị)
        /// </summary>
        [DisplayName("Đơn vị tính")]
        [Display(Order = 4)]
        public string ProductVariantUnitName { get; set; }

        #endregion

        #region Properties - Số lượng

        /// <summary>
        /// Số lượng theo hệ thống
        /// </summary>
        [DisplayName("SL Hệ thống")]
        [Display(Order = 10)]
        [Required(ErrorMessage = "Số lượng hệ thống không được để trống")]
        public decimal SystemQuantity { get; set; }

        /// <summary>
        /// Số lượng đã kiểm đếm
        /// </summary>
        [DisplayName("SL Đã kiểm")]
        [Display(Order = 11)]
        public decimal? CountedQuantity { get; set; }

        /// <summary>
        /// Số lượng chênh lệch
        /// </summary>
        [DisplayName("SL Chênh lệch")]
        [Display(Order = 12)]
        [Required(ErrorMessage = "Số lượng chênh lệch không được để trống")]
        public decimal DifferenceQuantity { get; set; }

        #endregion

        #region Properties - Giá trị

        /// <summary>
        /// Giá trị theo hệ thống
        /// </summary>
        [DisplayName("Giá trị Hệ thống")]
        [Display(Order = 20)]
        public decimal? SystemValue { get; set; }

        /// <summary>
        /// Giá trị đã kiểm đếm
        /// </summary>
        [DisplayName("Giá trị Đã kiểm")]
        [Display(Order = 21)]
        public decimal? CountedValue { get; set; }

        /// <summary>
        /// Giá trị chênh lệch
        /// </summary>
        [DisplayName("Giá trị Chênh lệch")]
        [Display(Order = 22)]
        public decimal? DifferenceValue { get; set; }

        /// <summary>
        /// Đơn giá
        /// </summary>
        [DisplayName("Đơn giá")]
        [Display(Order = 23)]
        public decimal? UnitPrice { get; set; }

        #endregion

        #region Properties - Điều chỉnh

        /// <summary>
        /// Loại điều chỉnh
        /// </summary>
        [DisplayName("Loại điều chỉnh")]
        [Display(Order = 30)]
        public AdjustmentTypeEnum? AdjustmentType { get; set; }

        /// <summary>
        /// Loại điều chỉnh dưới dạng int (để lưu vào database)
        /// </summary>
        [DisplayName("Loại điều chỉnh (Int)")]
        [Display(Order = 30)]
        [Browsable(false)]
        public int? AdjustmentTypeInt
        {
            get => AdjustmentType.ToInt();
            set => AdjustmentType = value.ToAdjustmentTypeEnum();
        }

        /// <summary>
        /// Lý do điều chỉnh
        /// </summary>
        [DisplayName("Lý do điều chỉnh")]
        [Display(Order = 31)]
        [StringLength(500, ErrorMessage = "Lý do điều chỉnh không được vượt quá 500 ký tự")]
        public string AdjustmentReason { get; set; }

        #endregion

        #region Properties - Quy trình phê duyệt

        /// <summary>
        /// Đã kiểm đếm
        /// </summary>
        [DisplayName("Đã kiểm đếm")]
        [Display(Order = 40)]
        public bool IsCounted { get; set; }

        /// <summary>
        /// Người kiểm đếm
        /// </summary>
        [DisplayName("Người kiểm đếm")]
        [Display(Order = 41)]
        public Guid? CountedBy { get; set; }

        /// <summary>
        /// Ngày kiểm đếm
        /// </summary>
        [DisplayName("Ngày kiểm đếm")]
        [Display(Order = 42)]
        public DateTime? CountedDate { get; set; }

        /// <summary>
        /// Đã rà soát
        /// </summary>
        [DisplayName("Đã rà soát")]
        [Display(Order = 43)]
        public bool IsReviewed { get; set; }

        /// <summary>
        /// Người rà soát
        /// </summary>
        [DisplayName("Người rà soát")]
        [Display(Order = 44)]
        public Guid? ReviewedBy { get; set; }

        /// <summary>
        /// Ngày rà soát
        /// </summary>
        [DisplayName("Ngày rà soát")]
        [Display(Order = 45)]
        public DateTime? ReviewedDate { get; set; }

        /// <summary>
        /// Ghi chú rà soát
        /// </summary>
        [DisplayName("Ghi chú rà soát")]
        [Display(Order = 46)]
        [StringLength(1000, ErrorMessage = "Ghi chú rà soát không được vượt quá 1000 ký tự")]
        public string ReviewNotes { get; set; }

        /// <summary>
        /// Đã phê duyệt
        /// </summary>
        [DisplayName("Đã phê duyệt")]
        [Display(Order = 47)]
        public bool IsApproved { get; set; }

        /// <summary>
        /// Người phê duyệt
        /// </summary>
        [DisplayName("Người phê duyệt")]
        [Display(Order = 48)]
        public Guid? ApprovedBy { get; set; }

        /// <summary>
        /// Ngày phê duyệt
        /// </summary>
        [DisplayName("Ngày phê duyệt")]
        [Display(Order = 49)]
        public DateTime? ApprovedDate { get; set; }

        #endregion

        #region Properties - Thông tin bổ sung

        /// <summary>
        /// Ghi chú
        /// </summary>
        [DisplayName("Ghi chú")]
        [Display(Order = 50)]
        [StringLength(1000, ErrorMessage = "Ghi chú không được vượt quá 1000 ký tự")]
        public string Notes { get; set; }

        #endregion

        #region Properties - Trạng thái

        /// <summary>
        /// Đang hoạt động
        /// </summary>
        [DisplayName("Đang hoạt động")]
        [Display(Order = 60)]
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Đã xóa
        /// </summary>
        [DisplayName("Đã xóa")]
        [Display(Order = 61)]
        public bool IsDeleted { get; set; } = false;

        #endregion

        #region Properties - Audit fields

        /// <summary>
        /// Người tạo
        /// </summary>
        [DisplayName("Người tạo")]
        [Display(Order = 100)]
        public Guid? CreatedBy { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        [DisplayName("Ngày tạo")]
        [Display(Order = 101)]
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Người cập nhật
        /// </summary>
        [DisplayName("Người cập nhật")]
        [Display(Order = 102)]
        public Guid? UpdatedBy { get; set; }

        /// <summary>
        /// Ngày cập nhật
        /// </summary>
        [DisplayName("Ngày cập nhật")]
        [Display(Order = 103)]
        public DateTime? UpdatedDate { get; set; }

        #endregion

        #region Properties - HTML Display

        /// <summary>
        /// Thông tin sản phẩm dưới dạng HTML theo format DevExpress
        /// Sử dụng các tag HTML chuẩn của DevExpress: &lt;b&gt;, &lt;i&gt;, &lt;color&gt;
        /// Tham khảo: https://docs.devexpress.com/WindowsForms/4874/common-features/html-text-formatting
        /// </summary>
        [DisplayName("Thông tin sản phẩm HTML")]
        [Display(Order = 200)]
        [Description("Thông tin sản phẩm dưới dạng HTML")]
        public string ThongTinSanPhamHtml
        {
            get
            {
                if (string.IsNullOrWhiteSpace(ProductVariantName))
                    return "<color='#757575'><i>Chưa có thông tin sản phẩm</i></color>";

                var html = $"<b><color='blue'>{EscapeHtml(ProductVariantName)}</color></b>";
                if (!string.IsNullOrWhiteSpace(ProductVariantCode))
                {
                    html += $" <color='gray'>({EscapeHtml(ProductVariantCode)})</color>";
                }
                if (!string.IsNullOrWhiteSpace(ProductVariantUnitName))
                {
                    html += $" <color='#757575'>- {EscapeHtml(ProductVariantUnitName)}</color>";
                }
                return html;
            }
        }

        /// <summary>
        /// Thông tin số lượng dưới dạng HTML theo format DevExpress
        /// </summary>
        [DisplayName("Số lượng HTML")]
        [Display(Order = 201)]
        [Description("Thông tin số lượng dưới dạng HTML")]
        public string SoLuongHtml
        {
            get
            {
                var htmlParts = new List<string>();
                var unitDisplay = !string.IsNullOrWhiteSpace(ProductVariantUnitName) ? $" {EscapeHtml(ProductVariantUnitName)}" : "";

                // Số lượng hệ thống
                htmlParts.Add($"<color='#757575'>Hệ thống:</color> <color='#212121'><b>{SystemQuantity:N2}{unitDisplay}</b></color>");

                // Số lượng đã kiểm
                if (CountedQuantity.HasValue)
                {
                    htmlParts.Add("<br>");
                    htmlParts.Add($"<color='#757575'>Đã kiểm:</color> <color='#212121'><b>{CountedQuantity.Value:N2}{unitDisplay}</b></color>");
                }

                // Số lượng chênh lệch
                if (DifferenceQuantity != 0)
                {
                    htmlParts.Add("<br>");
                    var diffColor = DifferenceQuantity > 0 ? "#4CAF50" : "#F44336"; // Xanh nếu dương, đỏ nếu âm
                    var diffSign = DifferenceQuantity > 0 ? "+" : "";
                    htmlParts.Add($"<color='#757575'>Chênh lệch:</color> <color='{diffColor}'><b>{diffSign}{DifferenceQuantity:N2}{unitDisplay}</b></color>");
                }
                else
                {
                    htmlParts.Add("<br>");
                    htmlParts.Add($"<color='#757575'>Chênh lệch:</color> <color='#212121'><b>{DifferenceQuantity:N2}{unitDisplay}</b></color>");
                }

                return string.Join(string.Empty, htmlParts);
            }
        }

        /// <summary>
        /// Thông tin quy trình phê duyệt dưới dạng HTML
        /// </summary>
        [DisplayName("Quy trình HTML")]
        [Display(Order = 202)]
        [Description("Thông tin quy trình phê duyệt dưới dạng HTML")]
        public string QuyTrinhHtml
        {
            get
            {
                var htmlParts = new List<string>();

                // Kiểm đếm
                if (IsCounted)
                {
                    htmlParts.Add($"<color='#4CAF50'><b>✓ Đã kiểm đếm</b></color>");
                    if (CountedDate.HasValue)
                    {
                        htmlParts.Add($" <color='#757575'>({CountedDate.Value:dd/MM/yyyy})</color>");
                    }
                }
                else
                {
                    htmlParts.Add($"<color='#757575'><i>Chưa kiểm đếm</i></color>");
                }

                // Rà soát
                if (IsReviewed)
                {
                    htmlParts.Add("<br>");
                    htmlParts.Add($"<color='#2196F3'><b>✓ Đã rà soát</b></color>");
                    if (ReviewedDate.HasValue)
                    {
                        htmlParts.Add($" <color='#757575'>({ReviewedDate.Value:dd/MM/yyyy})</color>");
                    }
                    if (!string.IsNullOrWhiteSpace(ReviewNotes))
                    {
                        htmlParts.Add($"<br><color='#757575'>Ghi chú:</color> <color='#212121'><i>{EscapeHtml(ReviewNotes)}</i></color>");
                    }
                }

                // Phê duyệt
                if (IsApproved)
                {
                    htmlParts.Add("<br>");
                    htmlParts.Add($"<color='#4CAF50'><b>✓ Đã phê duyệt</b></color>");
                    if (ApprovedDate.HasValue)
                    {
                        htmlParts.Add($" <color='#757575'>({ApprovedDate.Value:dd/MM/yyyy})</color>");
                    }
                }

                return string.Join(string.Empty, htmlParts);
            }
        }

        /// <summary>
        /// Thông tin tổng hợp dưới dạng HTML
        /// </summary>
        [DisplayName("Thông tin HTML")]
        [Display(Order = 203)]
        [Description("Thông tin tổng hợp dưới dạng HTML")]
        public string ThongTinHtml
        {
            get
            {
                var htmlParts = new List<string>();

                // Thông tin sản phẩm
                htmlParts.Add(ThongTinSanPhamHtml);

                // Số lượng
                htmlParts.Add("<br>");
                htmlParts.Add(SoLuongHtml);

                // Giá trị (nếu có)
                if (SystemValue.HasValue || CountedValue.HasValue || DifferenceValue.HasValue)
                {
                    htmlParts.Add("<br>");
                    htmlParts.Add("<color='#757575'>Giá trị:</color>");
                    if (SystemValue.HasValue)
                    {
                        htmlParts.Add($" <color='#212121'><b>{SystemValue.Value:N0}</b></color>");
                    }
                    if (CountedValue.HasValue)
                    {
                        htmlParts.Add($" <color='#757575'>(Đã kiểm: {CountedValue.Value:N0})</color>");
                    }
                    if (DifferenceValue.HasValue && DifferenceValue.Value != 0)
                    {
                        var diffColor = DifferenceValue.Value > 0 ? "#4CAF50" : "#F44336";
                        var diffSign = DifferenceValue.Value > 0 ? "+" : "";
                        htmlParts.Add($" <color='{diffColor}'>(Chênh lệch: {diffSign}{DifferenceValue.Value:N0})</color>");
                    }
                }

                // Loại điều chỉnh (nếu có)
                if (AdjustmentType.HasValue)
                {
                    htmlParts.Add("<br>");
                    var adjustmentColor = AdjustmentType.GetColor();
                    var adjustmentDesc = AdjustmentType.GetDescription();
                    htmlParts.Add($"<color='#757575'>Loại điều chỉnh:</color> <color='{adjustmentColor}'><b>{EscapeHtml(adjustmentDesc)}</b></color>");
                }

                // Lý do điều chỉnh (nếu có)
                if (!string.IsNullOrWhiteSpace(AdjustmentReason))
                {
                    htmlParts.Add("<br>");
                    htmlParts.Add($"<color='#757575'>Lý do:</color> <color='#212121'><i>{EscapeHtml(AdjustmentReason)}</i></color>");
                }

                // Ghi chú (nếu có)
                if (!string.IsNullOrWhiteSpace(Notes))
                {
                    htmlParts.Add("<br>");
                    htmlParts.Add($"<color='#757575'>Ghi chú:</color> <color='#212121'><i>{EscapeHtml(Notes)}</i></color>");
                }

                return string.Join(string.Empty, htmlParts);
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Escape HTML special characters
        /// </summary>
        private string EscapeHtml(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            return value.Replace("&", "&amp;")
                       .Replace("<", "&lt;")
                       .Replace(">", "&gt;")
                       .Replace("\"", "&quot;")
                       .Replace("'", "&#39;");
        }

        #endregion
    }
}
