using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DTO.Inventory.Assembly;

/// <summary>
/// Data Transfer Object cho AssemblyTransaction (Lịch sử lắp ráp)
/// Lưu thông tin các giao dịch lắp ráp sản phẩm
/// </summary>
public class AssemblyTransactionDto
{
    #region Properties - Thông tin cơ bản (map với DB)

    /// <summary>
    /// ID duy nhất của giao dịch lắp ráp
    /// Map với: AssemblyTransaction.Id
    /// </summary>
    [DisplayName("ID")]
    [Display(Order = -1)]
    public Guid Id { get; set; }

    /// <summary>
    /// Ngày lắp ráp
    /// Map với: AssemblyTransaction.AssemblyDate
    /// </summary>
    [DisplayName("Ngày lắp ráp")]
    [Display(Order = 1)]
    [Required(ErrorMessage = "Ngày lắp ráp không được để trống")]
    public DateTime AssemblyDate { get; set; }

    /// <summary>
    /// ID sản phẩm lắp ráp
    /// Map với: AssemblyTransaction.ProductVariantId
    /// </summary>
    [DisplayName("ID Sản phẩm")]
    [Display(Order = 2)]
    [Required(ErrorMessage = "Sản phẩm lắp ráp không được để trống")]
    public Guid ProductVariantId { get; set; }

    /// <summary>
    /// Số lượng lắp ráp
    /// Map với: AssemblyTransaction.Quantity
    /// </summary>
    [DisplayName("Số lượng")]
    [Display(Order = 3)]
    [Required(ErrorMessage = "Số lượng không được để trống")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// ID phiếu xuất linh kiện
    /// Map với: AssemblyTransaction.StockOutMasterId
    /// </summary>
    [DisplayName("ID Phiếu xuất")]
    [Display(Order = 4)]
    [Required(ErrorMessage = "Phiếu xuất linh kiện không được để trống")]
    public Guid StockOutMasterId { get; set; }

    /// <summary>
    /// ID phiếu nhập sản phẩm
    /// Map với: AssemblyTransaction.StockInMasterId
    /// </summary>
    [DisplayName("ID Phiếu nhập")]
    [Display(Order = 5)]
    [Required(ErrorMessage = "Phiếu nhập sản phẩm không được để trống")]
    public Guid StockInMasterId { get; set; }

    /// <summary>
    /// ID kho thực hiện lắp ráp
    /// Map với: AssemblyTransaction.WarehouseId
    /// </summary>
    [DisplayName("ID Kho")]
    [Display(Order = 6)]
    [Required(ErrorMessage = "Kho không được để trống")]
    public Guid WarehouseId { get; set; }

    /// <summary>
    /// Tổng giá thành (từ linh kiện)
    /// Map với: AssemblyTransaction.TotalCost
    /// </summary>
    [DisplayName("Tổng giá thành")]
    [Display(Order = 7)]
    public decimal TotalCost { get; set; }

    /// <summary>
    /// Giá thành đơn vị
    /// Map với: AssemblyTransaction.UnitCost
    /// </summary>
    [DisplayName("Giá thành đơn vị")]
    [Display(Order = 8)]
    public decimal UnitCost { get; set; }

    /// <summary>
    /// Ghi chú
    /// Map với: AssemblyTransaction.Notes
    /// </summary>
    [DisplayName("Ghi chú")]
    [Display(Order = 9)]
    [StringLength(1000, ErrorMessage = "Ghi chú không được vượt quá 1000 ký tự")]
    public string Notes { get; set; }

    /// <summary>
    /// Người tạo
    /// Map với: AssemblyTransaction.CreatedBy
    /// </summary>
    [DisplayName("Người tạo")]
    [Display(Order = 10)]
    public Guid? CreatedBy { get; set; }

    /// <summary>
    /// Ngày tạo
    /// Map với: AssemblyTransaction.CreatedDate
    /// </summary>
    [DisplayName("Ngày tạo")]
    [Display(Order = 11)]
    public DateTime CreatedDate { get; set; }

    #endregion

    #region Properties - Thông tin hiển thị

    /// <summary>
    /// Mã sản phẩm lắp ráp (để hiển thị)
    /// </summary>
    [DisplayName("Mã sản phẩm")]
    [Display(Order = 20)]
    public string ProductVariantCode { get; set; }

    /// <summary>
    /// Tên sản phẩm lắp ráp (để hiển thị)
    /// </summary>
    [DisplayName("Tên sản phẩm")]
    [Display(Order = 21)]
    public string ProductVariantName { get; set; }

    /// <summary>
    /// Số phiếu xuất linh kiện (để hiển thị)
    /// </summary>
    [DisplayName("Số phiếu xuất")]
    [Display(Order = 22)]
    public string StockOutNumber { get; set; }

    /// <summary>
    /// Số phiếu nhập sản phẩm (để hiển thị)
    /// </summary>
    [DisplayName("Số phiếu nhập")]
    [Display(Order = 23)]
    public string StockInNumber { get; set; }

    /// <summary>
    /// Tên kho (để hiển thị)
    /// </summary>
    [DisplayName("Kho")]
    [Display(Order = 24)]
    public string WarehouseName { get; set; }

    #endregion

    #region Constructors

    public AssemblyTransactionDto()
    {
        Id = Guid.NewGuid();
        AssemblyDate = DateTime.Now;
        CreatedDate = DateTime.Now;
    }

    #endregion
}

