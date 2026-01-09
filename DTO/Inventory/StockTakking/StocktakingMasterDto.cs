using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DTO.Inventory.StockTakking
{
    /// <summary>
    /// Data Transfer Object cho StocktakingMaster entity
    /// Quản lý thông tin phiếu kiểm kho
    /// </summary>
    public class StocktakingMasterDto
    {
        #region Properties - Thông tin cơ bản

        /// <summary>
        /// ID duy nhất của phiếu kiểm kho
        /// </summary>
        [DisplayName("ID")]
        [Display(Order = -1)]
        public Guid Id { get; set; }

        /// <summary>
        /// Ngày kiểm kho
        /// </summary>
        [DisplayName("Ngày kiểm kho")]
        [Display(Order = 1)]
        [Required(ErrorMessage = "Ngày kiểm kho không được để trống")]
        public DateTime StocktakingDate { get; set; }

        /// <summary>
        /// Số phiếu kiểm kho
        /// </summary>
        [DisplayName("Số phiếu")]
        [Display(Order = 2)]
        [Required(ErrorMessage = "Số phiếu kiểm kho không được để trống")]
        [StringLength(50, ErrorMessage = "Số phiếu kiểm kho không được vượt quá 50 ký tự")]
        public string VoucherNumber { get; set; }

        /// <summary>
        /// Loại kiểm kho
        /// </summary>
        [DisplayName("Loại kiểm kho")]
        [Display(Order = 3)]
        [Required(ErrorMessage = "Loại kiểm kho không được để trống")]
        public StocktakingTypeEnum StocktakingType { get; set; }

        /// <summary>
        /// Trạng thái kiểm kho
        /// </summary>
        [DisplayName("Trạng thái")]
        [Display(Order = 4)]
        [Required(ErrorMessage = "Trạng thái kiểm kho không được để trống")]
        public StocktakingStatusEnum StocktakingStatus { get; set; }

        /// <summary>
        /// ID kho kiểm
        /// </summary>
        [DisplayName("ID Kho")]
        [Display(Order = 5)]
        [Required(ErrorMessage = "Kho kiểm không được để trống")]
        public Guid WarehouseId { get; set; }

        /// <summary>
        /// ID chi nhánh công ty
        /// </summary>
        [DisplayName("ID Chi nhánh")]
        [Display(Order = 6)]
        public Guid? CompanyBranchId { get; set; }

        /// <summary>
        /// Tên kho (để hiển thị)
        /// </summary>
        [DisplayName("Tên kho")]
        [Display(Order = 7)]
        public string WarehouseName { get; set; }

        /// <summary>
        /// Mã kho (để hiển thị)
        /// </summary>
        [DisplayName("Mã kho")]
        [Display(Order = 8)]
        public string WarehouseCode { get; set; }

        #endregion

        #region Properties - Thời gian kiểm kho

        /// <summary>
        /// Ngày bắt đầu kiểm kho
        /// </summary>
        [DisplayName("Ngày bắt đầu")]
        [Display(Order = 10)]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Ngày kết thúc kiểm kho
        /// </summary>
        [DisplayName("Ngày kết thúc")]
        [Display(Order = 11)]
        public DateTime? EndDate { get; set; }

        #endregion

        #region Properties - Quy trình phê duyệt

        /// <summary>
        /// Người kiểm đếm
        /// </summary>
        [DisplayName("Người kiểm đếm")]
        [Display(Order = 20)]
        public Guid? CountedBy { get; set; }

        /// <summary>
        /// Ngày kiểm đếm
        /// </summary>
        [DisplayName("Ngày kiểm đếm")]
        [Display(Order = 21)]
        public DateTime? CountedDate { get; set; }

        /// <summary>
        /// Người rà soát
        /// </summary>
        [DisplayName("Người rà soát")]
        [Display(Order = 22)]
        public Guid? ReviewedBy { get; set; }

        /// <summary>
        /// Ngày rà soát
        /// </summary>
        [DisplayName("Ngày rà soát")]
        [Display(Order = 23)]
        public DateTime? ReviewedDate { get; set; }

        /// <summary>
        /// Người phê duyệt
        /// </summary>
        [DisplayName("Người phê duyệt")]
        [Display(Order = 24)]
        public Guid? ApprovedBy { get; set; }

        /// <summary>
        /// Ngày phê duyệt
        /// </summary>
        [DisplayName("Ngày phê duyệt")]
        [Display(Order = 25)]
        public DateTime? ApprovedDate { get; set; }

        #endregion

        #region Properties - Thông tin bổ sung

        /// <summary>
        /// Ghi chú
        /// </summary>
        [DisplayName("Ghi chú")]
        [Display(Order = 30)]
        [StringLength(1000, ErrorMessage = "Ghi chú không được vượt quá 1000 ký tự")]
        public string Notes { get; set; }

        /// <summary>
        /// Lý do kiểm kho
        /// </summary>
        [DisplayName("Lý do")]
        [Display(Order = 31)]
        [StringLength(500, ErrorMessage = "Lý do không được vượt quá 500 ký tự")]
        public string Reason { get; set; }

        #endregion

        #region Properties - Khóa phiếu

        /// <summary>
        /// Đã khóa phiếu
        /// </summary>
        [DisplayName("Đã khóa")]
        [Display(Order = 40)]
        public bool IsLocked { get; set; }

        /// <summary>
        /// Ngày khóa phiếu
        /// </summary>
        [DisplayName("Ngày khóa")]
        [Display(Order = 41)]
        public DateTime? LockedDate { get; set; }

        /// <summary>
        /// Người khóa phiếu
        /// </summary>
        [DisplayName("Người khóa")]
        [Display(Order = 42)]
        public Guid? LockedBy { get; set; }

        #endregion

        #region Properties - Trạng thái

        /// <summary>
        /// Đang hoạt động
        /// </summary>
        [DisplayName("Đang hoạt động")]
        [Display(Order = 50)]
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Đã xóa
        /// </summary>
        [DisplayName("Đã xóa")]
        [Display(Order = 51)]
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

        /// <summary>
        /// Người xóa
        /// </summary>
        [DisplayName("Người xóa")]
        [Display(Order = 104)]
        public Guid? DeletedBy { get; set; }

        /// <summary>
        /// Ngày xóa
        /// </summary>
        [DisplayName("Ngày xóa")]
        [Display(Order = 105)]
        public DateTime? DeletedDate { get; set; }

        #endregion

        #region Properties - HTML Display

        /// <summary>
        /// Thông tin phiếu kiểm kho dưới dạng HTML theo format DevExpress
        /// Sử dụng các tag HTML chuẩn của DevExpress: &lt;b&gt;, &lt;i&gt;, &lt;color&gt;
        /// Tham khảo: https://docs.devexpress.com/WindowsForms/4874/common-features/html-text-formatting
        /// </summary>
        [DisplayName("Thông tin HTML")]
        [Display(Order = 200)]
        [Description("Thông tin phiếu kiểm kho dưới dạng HTML")]
        public string ThongTinHtml
        {
            get
            {
                var htmlParts = new List<string>();
                var voucherNumber = VoucherNumber ?? string.Empty;

                // Số phiếu (nổi bật nhất) - màu xanh dương
                if (!string.IsNullOrWhiteSpace(voucherNumber))
                {
                    htmlParts.Add($"<b><color='blue'>{EscapeHtml(voucherNumber)}</color></b>");
                }

                // Ngày kiểm kho
                if (StocktakingDate != default(DateTime))
                {
                    if (!string.IsNullOrWhiteSpace(voucherNumber))
                    {
                        htmlParts.Add($" <color='blue'>({StocktakingDate:dd/MM/yyyy})</color>");
                    }
                    else
                    {
                        htmlParts.Add($"<b><color='blue'>{StocktakingDate:dd/MM/yyyy}</color></b>");
                    }
                }

                // Kho kiểm
                if (!string.IsNullOrWhiteSpace(WarehouseName))
                {
                    htmlParts.Add("<br>");
                    htmlParts.Add($"<color='#757575'>Kho:</color> <color='#212121'><b>{EscapeHtml(WarehouseName)}</b></color>");
                    if (!string.IsNullOrWhiteSpace(WarehouseCode))
                    {
                        htmlParts.Add($" <color='gray'>({EscapeHtml(WarehouseCode)})</color>");
                    }
                }

                // Loại kiểm kho và trạng thái
                if (StocktakingType != StocktakingTypeEnum.Periodic || StocktakingStatus != StocktakingStatusEnum.Draft)
                {
                    htmlParts.Add("<br>");
                    if (StocktakingType != StocktakingTypeEnum.Periodic)
                    {
                        var stocktakingTypeDescription = Common.Utils.ApplicationEnumUtils.GetDescription(StocktakingType);
                        htmlParts.Add($"<color='#757575'>Loại:</color> <color='#212121'><b>{EscapeHtml(stocktakingTypeDescription)}</b></color>");
                    }
                    if (StocktakingStatus != StocktakingStatusEnum.Draft)
                    {
                        if (StocktakingType != StocktakingTypeEnum.Periodic)
                        {
                            htmlParts.Add(" | ");
                        }
                        var stocktakingStatusDescription = Common.Utils.ApplicationEnumUtils.GetDescription(StocktakingStatus);
                        htmlParts.Add($"<color='#757575'>Trạng thái:</color> <color='#212121'><b>{EscapeHtml(stocktakingStatusDescription)}</b></color>");
                    }
                }

                // Thời gian kiểm kho
                if (StartDate.HasValue || EndDate.HasValue)
                {
                    htmlParts.Add("<br>");
                    if (StartDate.HasValue && EndDate.HasValue)
                    {
                        htmlParts.Add($"<color='#757575'>Thời gian:</color> <color='#212121'><b>{StartDate.Value:dd/MM/yyyy}</b></color> <color='#757575'>-</color> <color='#212121'><b>{EndDate.Value:dd/MM/yyyy}</b></color>");
                    }
                    else if (StartDate.HasValue)
                    {
                        htmlParts.Add($"<color='#757575'>Bắt đầu:</color> <color='#212121'><b>{StartDate.Value:dd/MM/yyyy}</b></color>");
                    }
                    else if (EndDate.HasValue)
                    {
                        htmlParts.Add($"<color='#757575'>Kết thúc:</color> <color='#212121'><b>{EndDate.Value:dd/MM/yyyy}</b></color>");
                    }
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

        /// <summary>
        /// Thông tin quy trình phê duyệt dưới dạng HTML
        /// </summary>
        [DisplayName("Quy trình HTML")]
        [Display(Order = 201)]
        [Description("Thông tin quy trình phê duyệt dưới dạng HTML")]
        public string QuyTrinhHtml
        {
            get
            {
                var htmlParts = new List<string>();

                // Kiểm đếm
                if (CountedDate.HasValue || CountedBy.HasValue)
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
                if (ReviewedDate.HasValue)
                {
                    htmlParts.Add("<br>");
                    htmlParts.Add($"<color='#2196F3'><b>✓ Đã rà soát</b></color>");
                    htmlParts.Add($" <color='#757575'>({ReviewedDate.Value:dd/MM/yyyy})</color>");
                }

                // Phê duyệt
                if (ApprovedDate.HasValue)
                {
                    htmlParts.Add("<br>");
                    htmlParts.Add($"<color='#4CAF50'><b>✓ Đã phê duyệt</b></color>");
                    htmlParts.Add($" <color='#757575'>({ApprovedDate.Value:dd/MM/yyyy})</color>");
                }

                // Khóa phiếu
                if (IsLocked)
                {
                    htmlParts.Add("<br>");
                    htmlParts.Add($"<color='#F44336'><b>🔒 Đã khóa</b></color>");
                    if (LockedDate.HasValue)
                    {
                        htmlParts.Add($" <color='#757575'>({LockedDate.Value:dd/MM/yyyy})</color>");
                    }
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
