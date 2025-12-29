using DTO.Inventory.InventoryManagement;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DTO.Inventory
{
    public class StockInOutMasterForUIDto
    {
        #region Properties - Thông tin cơ bản

        /// <summary>
        /// ID duy nhất của phiếu xuất kho
        /// </summary>
        [DisplayName("ID")]
        [Display(Order = -1)]
        public Guid Id { get; set; }

        /// <summary>
        /// Số phiếu xuất kho
        /// </summary>
        [DisplayName("Số phiếu xuất")]
        [Display(Order = 1)]
        [Required(ErrorMessage = "Số phiếu xuất không được để trống")]
        [StringLength(50, ErrorMessage = "Số phiếu xuất không được vượt quá 50 ký tự")]
        public string StockOutNumber { get; set; }

        /// <summary>
        /// Ngày xuất kho
        /// </summary>
        [DisplayName("Ngày xuất")]
        [Display(Order = 2)]
        [Required(ErrorMessage = "Ngày xuất không được để trống")]
        public DateTime StockOutDate { get; set; }

        /// <summary>
        /// Loại xuất kho
        /// </summary>
        [DisplayName("Loại xuất")]
        [Display(Order = 3)]
        [Required(ErrorMessage = "Loại xuất kho không được để trống")]
        public LoaiNhapXuatKhoEnum LoaiNhapXuatKho { get; set; }

        /// <summary>
        /// Trạng thái phiếu xuất
        /// </summary>
        [DisplayName("Trạng thái")]
        [Display(Order = 4)]
        public TrangThaiPhieuNhapEnum TrangThai { get; set; }

        #endregion

        #region Properties - Thông tin liên kết

        /// <summary>
        /// ID kho xuất hàng
        /// </summary>
        [DisplayName("ID Kho")]
        [Display(Order = 10)]
        [Required(ErrorMessage = "Kho xuất không được để trống")]
        public Guid WarehouseId { get; set; }

        /// <summary>
        /// Mã kho xuất hàng (để hiển thị)
        /// </summary>
        [DisplayName("Mã kho")]
        [Display(Order = 11)]
        public string WarehouseCode { get; set; }

        /// <summary>
        /// Tên kho xuất hàng (để hiển thị)
        /// </summary>
        [DisplayName("Tên kho")]
        [Display(Order = 12)]
        public string WarehouseName { get; set; }

        /// <summary>
        /// ID đơn bán hàng (SalesOrder) - có thể dùng PurchaseOrderId trong DB để lưu
        /// </summary>
        [DisplayName("ID Đơn bán")]
        [Display(Order = 13)]
        public Guid? SalesOrderId { get; set; }

        /// <summary>
        /// Số đơn bán hàng (để hiển thị)
        /// </summary>
        [DisplayName("Số đơn bán")]
        [Display(Order = 14)]
        public string SalesOrderNumber { get; set; }

        /// <summary>
        /// ID khách hàng (PartnerSiteId)
        /// </summary>
        [DisplayName("ID Khách hàng")]
        [Display(Order = 15)]
        public Guid? CustomerId { get; set; }

        /// <summary>
        /// Tên khách hàng (để hiển thị)
        /// </summary>
        [DisplayName("Tên khách hàng")]
        [Display(Order = 16)]
        [Required(ErrorMessage = "Khách hàng không được để trống")]
        public string CustomerName { get; set; }

        #endregion

        #region Properties - Thông tin bổ sung

        /// <summary>
        /// Ghi chú
        /// </summary>
        [DisplayName("Ghi chú")]
        [Display(Order = 20)]
        [StringLength(500, ErrorMessage = "Ghi chú không được vượt quá 500 ký tự")]
        public string Notes { get; set; }

        /// <summary>
        /// Người nhận hàng
        /// </summary>
        [DisplayName("Người nhận hàng")]
        [Display(Order = 21)]
        [StringLength(500, ErrorMessage = "Người nhận hàng không được vượt quá 500 ký tự")]
        public string NguoiNhanHang { get; set; }

        /// <summary>
        /// Người giao hàng
        /// </summary>
        [DisplayName("Người giao hàng")]
        [Display(Order = 22)]
        [StringLength(500, ErrorMessage = "Người giao hàng không được vượt quá 500 ký tự")]
        public string NguoiGiaoHang { get; set; }

        #endregion

        #region Private Fields - Tổng hợp

        private decimal _totalQuantity;
        private decimal _totalAmount;
        private decimal _totalVat;
        private decimal _totalAmountIncludedVat;

        #endregion

        #region Properties - Tổng hợp (Computed)

        /// <summary>
        /// Tổng số lượng xuất - Computed property
        /// Tính toán từ tổng StockOutQty của tất cả các dòng detail
        /// Map với: StockInOutMaster.TotalQuantity (lưu vào DB khi save)
        /// </summary>
        [DisplayName("Tổng SL")]
        [Display(Order = 30)]
        public decimal TotalQuantity => _totalQuantity;

        /// <summary>
        /// Tổng giá trị xuất (chưa VAT) - Computed property
        /// Tính toán từ tổng TotalAmount của tất cả các dòng detail
        /// Map với: StockInOutMaster.TotalAmount (lưu vào DB khi save)
        /// </summary>
        [DisplayName("Tổng tiền chưa VAT")]
        [Display(Order = 31)]
        public decimal TotalAmount => _totalAmount;

        /// <summary>
        /// Tổng VAT - Computed property
        /// Tính toán từ tổng VatAmount của tất cả các dòng detail
        /// Map với: StockInOutMaster.TotalVat (lưu vào DB khi save)
        /// </summary>
        [DisplayName("VAT")]
        [Display(Order = 32)]
        public decimal TotalVat => _totalVat;

        /// <summary>
        /// Tổng tiền bao gồm VAT - Computed property
        /// Tính toán từ tổng TotalAmountIncludedVat của tất cả các dòng detail
        /// Map với: StockInOutMaster.TotalAmountIncludedVat (lưu vào DB khi save)
        /// </summary>
        [DisplayName("Tổng tiền bao gồm VAT")]
        [Display(Order = 33)]
        public decimal TotalAmountIncludedVat => _totalAmountIncludedVat;

        #endregion

        #region Public Methods - Cập nhật tổng hợp

        /// <summary>
        /// Cập nhật các giá trị tổng hợp từ detail
        /// Method này được gọi từ UcStockOutMaster khi có thay đổi trong Detail
        /// </summary>
        /// <param name="totalQuantity">Tổng số lượng</param>
        /// <param name="totalAmount">Tổng tiền chưa VAT</param>
        /// <param name="totalVat">Tổng VAT</param>
        /// <param name="totalAmountIncludedVat">Tổng tiền bao gồm VAT</param>
        public void SetTotals(decimal totalQuantity, decimal totalAmount, decimal totalVat,
            decimal totalAmountIncludedVat)
        {
            _totalQuantity = totalQuantity;
            _totalAmount = totalAmount;
            _totalVat = totalVat;
            _totalAmountIncludedVat = totalAmountIncludedVat;
        }

        #endregion
    }
}