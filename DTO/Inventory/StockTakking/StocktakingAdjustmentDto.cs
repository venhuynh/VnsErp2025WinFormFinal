using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DTO.Inventory.StockTakking
{
    /// <summary>
    /// Data Transfer Object cho StocktakingAdjustment entity
    /// Quản lý thông tin điều chỉnh kho sau khi kiểm kho
    /// </summary>
    public class StocktakingAdjustmentDto
    {
        #region Properties - Thông tin cơ bản

        /// <summary>
        /// ID duy nhất của điều chỉnh
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
        /// ID chi tiết kiểm kho
        /// </summary>
        [DisplayName("ID Chi tiết kiểm kho")]
        [Display(Order = 1)]
        [Required(ErrorMessage = "ID chi tiết kiểm kho không được để trống")]
        public Guid StocktakingDetailId { get; set; }

        /// <summary>
        /// ID phiếu nhập/xuất kho (nếu có)
        /// </summary>
        [DisplayName("ID Phiếu nhập/xuất")]
        [Display(Order = 2)]
        public Guid? StockInOutMasterId { get; set; }

        /// <summary>
        /// ID biến thể sản phẩm
        /// </summary>
        [DisplayName("ID Biến thể sản phẩm")]
        [Display(Order = 3)]
        [Required(ErrorMessage = "ID biến thể sản phẩm không được để trống")]
        public Guid ProductVariantId { get; set; }

        /// <summary>
        /// Tên biến thể sản phẩm (để hiển thị)
        /// </summary>
        [DisplayName("Tên sản phẩm")]
        [Display(Order = 4)]
        public string ProductVariantName { get; set; }

        /// <summary>
        /// Mã sản phẩm (để hiển thị)
        /// </summary>
        [DisplayName("Mã sản phẩm")]
        [Display(Order = 5)]
        public string ProductVariantCode { get; set; }

        #endregion

        #region Properties - Thông tin điều chỉnh

        /// <summary>
        /// Số lượng điều chỉnh
        /// </summary>
        [DisplayName("SL Điều chỉnh")]
        [Display(Order = 10)]
        [Required(ErrorMessage = "Số lượng điều chỉnh không được để trống")]
        public decimal AdjustmentQuantity { get; set; }

        /// <summary>
        /// Giá trị điều chỉnh
        /// </summary>
        [DisplayName("Giá trị Điều chỉnh")]
        [Display(Order = 11)]
        public decimal? AdjustmentValue { get; set; }

        /// <summary>
        /// Đơn giá
        /// </summary>
        [DisplayName("Đơn giá")]
        [Display(Order = 12)]
        public decimal? UnitPrice { get; set; }

        /// <summary>
        /// Loại điều chỉnh
        /// </summary>
        [DisplayName("Loại điều chỉnh")]
        [Display(Order = 13)]
        [Required(ErrorMessage = "Loại điều chỉnh không được để trống")]
        public int AdjustmentType { get; set; }

        /// <summary>
        /// Lý do điều chỉnh
        /// </summary>
        [DisplayName("Lý do điều chỉnh")]
        [Display(Order = 14)]
        [StringLength(500, ErrorMessage = "Lý do điều chỉnh không được vượt quá 500 ký tự")]
        public string AdjustmentReason { get; set; }

        /// <summary>
        /// Ngày điều chỉnh
        /// </summary>
        [DisplayName("Ngày điều chỉnh")]
        [Display(Order = 15)]
        [Required(ErrorMessage = "Ngày điều chỉnh không được để trống")]
        public DateTime AdjustmentDate { get; set; }

        /// <summary>
        /// Người điều chỉnh
        /// </summary>
        [DisplayName("Người điều chỉnh")]
        [Display(Order = 16)]
        public Guid? AdjustedBy { get; set; }

        #endregion

        #region Properties - Thông tin bổ sung

        /// <summary>
        /// Ghi chú
        /// </summary>
        [DisplayName("Ghi chú")]
        [Display(Order = 20)]
        [StringLength(1000, ErrorMessage = "Ghi chú không được vượt quá 1000 ký tự")]
        public string Notes { get; set; }

        #endregion

        #region Properties - Trạng thái áp dụng

        /// <summary>
        /// Đã áp dụng
        /// </summary>
        [DisplayName("Đã áp dụng")]
        [Display(Order = 30)]
        public bool IsApplied { get; set; }

        /// <summary>
        /// Ngày áp dụng
        /// </summary>
        [DisplayName("Ngày áp dụng")]
        [Display(Order = 31)]
        public DateTime? AppliedDate { get; set; }

        #endregion

        #region Properties - Trạng thái

        /// <summary>
        /// Đang hoạt động
        /// </summary>
        [DisplayName("Đang hoạt động")]
        [Display(Order = 40)]
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Đã xóa
        /// </summary>
        [DisplayName("Đã xóa")]
        [Display(Order = 41)]
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
        /// Thông tin điều chỉnh dưới dạng HTML theo format DevExpress
        /// Sử dụng các tag HTML chuẩn của DevExpress: &lt;b&gt;, &lt;i&gt;, &lt;color&gt;
        /// Tham khảo: https://docs.devexpress.com/WindowsForms/4874/common-features/html-text-formatting
        /// </summary>
        [DisplayName("Thông tin HTML")]
        [Display(Order = 200)]
        [Description("Thông tin điều chỉnh dưới dạng HTML")]
        public string ThongTinHtml
        {
            get
            {
                var htmlParts = new List<string>();

                // Sản phẩm
                if (!string.IsNullOrWhiteSpace(ProductVariantName))
                {
                    htmlParts.Add($"<b><color='blue'>{EscapeHtml(ProductVariantName)}</color></b>");
                    if (!string.IsNullOrWhiteSpace(ProductVariantCode))
                    {
                        htmlParts.Add($" <color='gray'>({EscapeHtml(ProductVariantCode)})</color>");
                    }
                }

                // Số lượng điều chỉnh
                htmlParts.Add("<br>");
                var quantityColor = AdjustmentQuantity > 0 ? "#4CAF50" : "#F44336"; // Xanh nếu dương, đỏ nếu âm
                var quantitySign = AdjustmentQuantity > 0 ? "+" : "";
                htmlParts.Add($"<color='#757575'>Số lượng:</color> <color='{quantityColor}'><b>{quantitySign}{AdjustmentQuantity:N2}</b></color>");

                // Giá trị điều chỉnh (nếu có)
                if (AdjustmentValue.HasValue)
                {
                    htmlParts.Add("<br>");
                    var valueColor = AdjustmentValue.Value > 0 ? "#4CAF50" : "#F44336";
                    var valueSign = AdjustmentValue.Value > 0 ? "+" : "";
                    htmlParts.Add($"<color='#757575'>Giá trị:</color> <color='{valueColor}'><b>{valueSign}{AdjustmentValue.Value:N0}</b></color>");
                }

                // Loại điều chỉnh
                if (AdjustmentType != 0)
                {
                    htmlParts.Add("<br>");
                    htmlParts.Add($"<color='#757575'>Loại:</color> <color='#212121'><b>{AdjustmentType}</b></color>");
                }

                // Ngày điều chỉnh
                if (AdjustmentDate != default(DateTime))
                {
                    htmlParts.Add("<br>");
                    htmlParts.Add($"<color='#757575'>Ngày:</color> <color='#212121'><b>{AdjustmentDate:dd/MM/yyyy}</b></color>");
                }

                // Lý do (nếu có)
                if (!string.IsNullOrWhiteSpace(AdjustmentReason))
                {
                    htmlParts.Add("<br>");
                    htmlParts.Add($"<color='#757575'>Lý do:</color> <color='#212121'><i>{EscapeHtml(AdjustmentReason)}</i></color>");
                }

                // Trạng thái áp dụng
                htmlParts.Add("<br>");
                if (IsApplied)
                {
                    htmlParts.Add($"<color='#4CAF50'><b>✓ Đã áp dụng</b></color>");
                    if (AppliedDate.HasValue)
                    {
                        htmlParts.Add($" <color='#757575'>({AppliedDate.Value:dd/MM/yyyy})</color>");
                    }
                }
                else
                {
                    htmlParts.Add($"<color='#FF9800'><b>⏳ Chưa áp dụng</b></color>");
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
