using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DTO.Inventory.InventoryManagement
{
    /// <summary>
    /// Data Transfer Object cho hình ảnh nhập/xuất kho
    /// </summary>
    public class StockInOutImageDto
    {
        #region Properties - Thông tin cơ bản

        /// <summary>
        /// ID duy nhất của hình ảnh
        /// </summary>
        [DisplayName("ID")]
        [Display(Order = -1)]
        public Guid Id { get; set; }

        /// <summary>
        /// ID phiếu nhập/xuất kho
        /// </summary>
        [DisplayName("ID Phiếu nhập/xuất")]
        [Display(Order = 1)]
        [Required(ErrorMessage = "ID phiếu nhập/xuất không được để trống")]
        public Guid StockInOutMasterId { get; set; }

        /// <summary>
        /// Dữ liệu hình ảnh (byte array)
        /// </summary>
        [DisplayName("Dữ liệu hình ảnh")]
        [Display(Order = 2)]
        public byte[] ImageData { get; set; }

        #endregion

        #region Properties - Thông tin hệ thống

        /// <summary>
        /// Ngày tạo
        /// </summary>
        [DisplayName("Ngày tạo")]
        [Display(Order = 10)]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// ID người tạo
        /// </summary>
        [DisplayName("ID Người tạo")]
        [Display(Order = 11)]
        public Guid CreateBy { get; set; }

        /// <summary>
        /// Ngày sửa đổi
        /// </summary>
        [DisplayName("Ngày sửa")]
        [Display(Order = 12)]
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// ID người sửa đổi
        /// </summary>
        [DisplayName("ID Người sửa")]
        [Display(Order = 13)]
        public Guid ModifiedBy { get; set; }

        #endregion
    }
}
