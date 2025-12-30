using DTO.Inventory.InventoryManagement;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DTO.Inventory.StockIn.NhapLapRap
{
    /// <summary>
    /// DTO cho danh sách phiếu nhập lắp ráp (dùng cho lookup)
    /// Wrapper cho StockInOutMasterForUIDto với namespace riêng
    /// </summary>
    public class NhapLapRapMasterListDto
    {
        #region Properties - Thông tin cơ bản

        /// <summary>
        /// ID duy nhất của phiếu nhập kho
        /// </summary>
        [DisplayName("ID")]
        [Display(Order = -1)]
        public Guid Id { get; set; }

        /// <summary>
        /// Số phiếu nhập kho
        /// </summary>
        [DisplayName("Số phiếu")]
        [Display(Order = 1)]
        [Required(ErrorMessage = "Số phiếu nhập không được để trống")]
        [StringLength(50, ErrorMessage = "Số phiếu nhập không được vượt quá 50 ký tự")]
        public string VoucherNumber { get; set; }

        /// <summary>
        /// Ngày nhập kho
        /// </summary>
        [DisplayName("Ngày nhập")]
        [Display(Order = 2)]
        [Required(ErrorMessage = "Ngày nhập không được để trống")]
        public DateTime StockInDate { get; set; }

        /// <summary>
        /// Loại nhập kho
        /// </summary>
        [DisplayName("Loại nhập")]
        [Display(Order = 3)]
        [Required(ErrorMessage = "Loại nhập kho không được để trống")]
        public LoaiNhapXuatKhoEnum LoaiNhapXuatKho { get; set; }

        /// <summary>
        /// Trạng thái phiếu nhập
        /// </summary>
        [DisplayName("Trạng thái")]
        [Display(Order = 4)]
        public TrangThaiPhieuNhapEnum TrangThai { get; set; }

        #endregion

        #region Properties - Thông tin liên kết

        /// <summary>
        /// ID kho nhập hàng
        /// </summary>
        [DisplayName("ID Kho")]
        [Display(Order = 10)]
        [Required(ErrorMessage = "Kho nhập không được để trống")]
        public Guid WarehouseId { get; set; }

        /// <summary>
        /// Mã kho nhập hàng (để hiển thị)
        /// </summary>
        [DisplayName("Mã kho")]
        [Display(Order = 11)]
        public string WarehouseCode { get; set; }

        /// <summary>
        /// Tên kho nhập hàng (để hiển thị)
        /// </summary>
        [DisplayName("Tên kho")]
        [Display(Order = 12)]
        public string WarehouseName { get; set; }

        /// <summary>
        /// ID nhà cung cấp
        /// </summary>
        [DisplayName("ID Nhà cung cấp")]
        [Display(Order = 13)]
        public Guid? SupplierId { get; set; }

        /// <summary>
        /// Tên nhà cung cấp (để hiển thị)
        /// </summary>
        [DisplayName("Tên nhà cung cấp")]
        [Display(Order = 14)]
        public string SupplierName { get; set; }

        #endregion

        #region Properties - Thông tin bổ sung

        /// <summary>
        /// Ghi chú
        /// </summary>
        [DisplayName("Ghi chú")]
        [Display(Order = 20)]
        [StringLength(500, ErrorMessage = "Ghi chú không được vượt quá 500 ký tự")]
        public string Notes { get; set; }

        #endregion

        #region Properties - Tổng hợp

        /// <summary>
        /// Tổng số lượng nhập
        /// </summary>
        [DisplayName("Tổng SL")]
        [Display(Order = 30)]
        public decimal TotalQuantity { get; set; }

        /// <summary>
        /// Tổng giá trị nhập (chưa VAT)
        /// </summary>
        [DisplayName("Tổng tiền chưa VAT")]
        [Display(Order = 31)]
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Tổng VAT
        /// </summary>
        [DisplayName("VAT")]
        [Display(Order = 32)]
        public decimal TotalVat { get; set; }

        /// <summary>
        /// Tổng tiền bao gồm VAT
        /// </summary>
        [DisplayName("Tổng tiền bao gồm VAT")]
        [Display(Order = 33)]
        public decimal TotalAmountIncludedVat { get; set; }

        #endregion
    }
}
