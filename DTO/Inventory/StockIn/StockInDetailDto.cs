using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DTO.Inventory.StockIn
{
    /// <summary>
    /// Data Transfer Object cho chi tiết phiếu nhập kho
    /// Dùng cho GridControl và truyền dữ liệu giữa Service ↔ WinForms
    /// </summary>
    public class StockInDetailDto
    {
        #region Properties - Thông tin cơ bản

        /// <summary>
        /// ID duy nhất của chi tiết phiếu nhập
        /// </summary>
        [DisplayName("ID")]
        [Display(Order = -1)]
        public Guid Id { get; set; }

        /// <summary>
        /// ID phiếu nhập kho (Master)
        /// </summary>
        [DisplayName("ID Phiếu nhập")]
        [Display(Order = 0)]
        [Required(ErrorMessage = "ID phiếu nhập không được để trống")]
        public Guid StockInMasterId { get; set; }

        /// <summary>
        /// Thứ tự dòng
        /// </summary>
        [DisplayName("STT")]
        [Display(Order = 1)]
        public int LineNumber { get; set; }

        #endregion

        #region Properties - Thông tin hàng hóa

        /// <summary>
        /// ID hàng hóa (Item/Product)
        /// </summary>
        [DisplayName("Hàng hóa")]
        [Display(Order = 10)]
        [Required(ErrorMessage = "Hàng hóa không được để trống")]
        public Guid ItemId { get; set; }

        /// <summary>
        /// Mã hàng hóa
        /// </summary>
        [DisplayName("Mã hàng")]
        [Display(Order = 11)]
        public string ItemCode { get; set; }

        /// <summary>
        /// Tên hàng hóa
        /// </summary>
        [DisplayName("Tên hàng")]
        [Display(Order = 12)]
        public string ItemName { get; set; }

        /// <summary>
        /// ID đơn vị tính
        /// </summary>
        [DisplayName("Đơn vị tính")]
        [Display(Order = 13)]
        [Required(ErrorMessage = "Đơn vị tính không được để trống")]
        public Guid UnitOfMeasureId { get; set; }

        /// <summary>
        /// Mã đơn vị tính
        /// </summary>
        [DisplayName("Mã ĐVT")]
        [Display(Order = 14)]
        public string UnitOfMeasureCode { get; set; }

        /// <summary>
        /// Tên đơn vị tính
        /// </summary>
        [DisplayName("Đơn vị tính")]
        [Display(Order = 15)]
        public string UnitOfMeasureName { get; set; }

        #endregion

        #region Properties - Số lượng và giá

        /// <summary>
        /// Số lượng đặt hàng (từ PO) - Nếu có
        /// </summary>
        [DisplayName("SL đặt")]
        [Display(Order = 20)]
        [Range(0, double.MaxValue, ErrorMessage = "Số lượng đặt phải lớn hơn hoặc bằng 0")]
        public decimal? OrderedQuantity { get; set; }

        /// <summary>
        /// Số lượng đã nhận trước đó (từ PO) - Nếu có
        /// </summary>
        [DisplayName("SL đã nhận")]
        [Display(Order = 21)]
        [Range(0, double.MaxValue, ErrorMessage = "Số lượng đã nhận phải lớn hơn hoặc bằng 0")]
        public decimal? ReceivedQuantity { get; set; }

        /// <summary>
        /// Số lượng nhập (thực tế)
        /// </summary>
        [DisplayName("SL nhập")]
        [Display(Order = 22)]
        [Required(ErrorMessage = "Số lượng nhập không được để trống")]
        [Range(0.0001, double.MaxValue, ErrorMessage = "Số lượng nhập phải lớn hơn 0")]
        public decimal Quantity { get; set; }

        /// <summary>
        /// Đơn giá nhập
        /// </summary>
        [DisplayName("Đơn giá")]
        [Display(Order = 23)]
        [Range(0, double.MaxValue, ErrorMessage = "Đơn giá phải lớn hơn hoặc bằng 0")]
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Thành tiền (Quantity * UnitPrice)
        /// </summary>
        [DisplayName("Thành tiền")]
        [Display(Order = 24)]
        public decimal LineAmount { get; set; }

        #endregion

        #region Properties - Quản lý lô và vị trí

        /// <summary>
        /// Số lô (Batch Number) - Nếu quản lý theo lô
        /// </summary>
        [DisplayName("Số lô")]
        [Display(Order = 30)]
        [StringLength(50, ErrorMessage = "Số lô không được vượt quá 50 ký tự")]
        public string BatchNumber { get; set; }

        /// <summary>
        /// Hạn sử dụng - Nếu quản lý theo lô
        /// </summary>
        [DisplayName("Hạn sử dụng")]
        [Display(Order = 31)]
        public DateTime? ExpiryDate { get; set; }

        /// <summary>
        /// ID vị trí kho (Location) - Tùy chọn
        /// </summary>
        [DisplayName("Vị trí kho")]
        [Display(Order = 32)]
        public Guid? LocationId { get; set; }

        /// <summary>
        /// Mã vị trí kho
        /// </summary>
        [DisplayName("Mã vị trí")]
        [Display(Order = 33)]
        public string LocationCode { get; set; }

        /// <summary>
        /// Tên vị trí kho
        /// </summary>
        [DisplayName("Vị trí kho")]
        [Display(Order = 34)]
        public string LocationName { get; set; }

        #endregion

        #region Properties - Trạng thái và chất lượng

        /// <summary>
        /// Trạng thái hàng hóa
        /// </summary>
        [DisplayName("Trạng thái hàng")]
        [Display(Order = 40)]
        [Required(ErrorMessage = "Trạng thái hàng không được để trống")]
        public TrangThaiHangEnum TrangThaiHang { get; set; }

        /// <summary>
        /// Tên trạng thái hàng (hiển thị)
        /// </summary>
        [DisplayName("Trạng thái hàng")]
        [Display(Order = 41)]
        public string TrangThaiHangName { get; set; }

        #endregion

        #region Properties - Thông tin bổ sung

        /// <summary>
        /// Ghi chú dòng
        /// </summary>
        [DisplayName("Ghi chú")]
        [Display(Order = 50)]
        [StringLength(255, ErrorMessage = "Ghi chú không được vượt quá 255 ký tự")]
        public string Notes { get; set; }

        /// <summary>
        /// ID chi tiết PO (nếu nhập theo PO)
        /// </summary>
        [DisplayName("ID Chi tiết PO")]
        [Display(Order = 51)]
        public Guid? PurchaseOrderDetailId { get; set; }

        #endregion
    }
}

