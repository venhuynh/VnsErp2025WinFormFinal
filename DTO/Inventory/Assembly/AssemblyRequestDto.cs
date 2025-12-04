using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DTO.Inventory.Assembly;

/// <summary>
/// Data Transfer Object cho yêu cầu lắp ráp sản phẩm
/// Dùng để truyền dữ liệu từ UI đến BLL khi thực hiện lắp ráp
/// </summary>
public class AssemblyRequestDto
{
    #region Properties

    /// <summary>
    /// ID sản phẩm cần lắp ráp
    /// </summary>
    [DisplayName("ID Sản phẩm")]
    [Required(ErrorMessage = "Sản phẩm lắp ráp không được để trống")]
    public Guid ProductVariantId { get; set; }

    /// <summary>
    /// Số lượng sản phẩm cần lắp ráp
    /// </summary>
    [DisplayName("Số lượng")]
    [Required(ErrorMessage = "Số lượng không được để trống")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// ID kho thực hiện lắp ráp
    /// </summary>
    [DisplayName("ID Kho")]
    [Required(ErrorMessage = "Kho không được để trống")]
    public Guid WarehouseId { get; set; }

    /// <summary>
    /// Ngày lắp ráp
    /// </summary>
    [DisplayName("Ngày lắp ráp")]
    [Required(ErrorMessage = "Ngày lắp ráp không được để trống")]
    public DateTime AssemblyDate { get; set; }

    /// <summary>
    /// Ghi chú
    /// </summary>
    [DisplayName("Ghi chú")]
    [StringLength(1000, ErrorMessage = "Ghi chú không được vượt quá 1000 ký tự")]
    public string Notes { get; set; }

    /// <summary>
    /// Danh sách BOM components (tùy chọn - nếu không có sẽ load từ ProductBOM)
    /// </summary>
    [DisplayName("Danh sách linh kiện")]
    public List<ProductBOMDto> Components { get; set; }

    #endregion

    #region Constructors

    public AssemblyRequestDto()
    {
        AssemblyDate = DateTime.Now;
        Components = new List<ProductBOMDto>();
    }

    #endregion

    #region Validation

    /// <summary>
    /// Kiểm tra dữ liệu có hợp lệ không
    /// </summary>
    public bool IsValid()
    {
        return ProductVariantId != Guid.Empty
            && Quantity > 0
            && WarehouseId != Guid.Empty
            && AssemblyDate != default(DateTime);
    }

    #endregion
}

