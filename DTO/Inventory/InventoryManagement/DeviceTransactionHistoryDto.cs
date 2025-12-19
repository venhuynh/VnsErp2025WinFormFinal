using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DTO.Inventory.InventoryManagement;

/// <summary>
/// Data Transfer Object cho DeviceTransactionHistory entity
/// Ghi lại lịch sử các thao tác với thiết bị: nhập, xuất, cấp phát, thu hồi, v.v.
/// </summary>
public class DeviceTransactionHistoryDto
{
    #region Properties - Thông tin cơ bản

    /// <summary>
    /// ID duy nhất của bản ghi lịch sử
    /// </summary>
    [DisplayName("ID")]
    [Display(Order = -1)]
    public Guid Id { get; set; }

    /// <summary>
    /// ID thiết bị được thao tác
    /// </summary>
    [DisplayName("ID Thiết bị")]
    [Display(Order = 0)]
    [Required(ErrorMessage = "ID thiết bị không được để trống")]
    public Guid DeviceId { get; set; }

    #endregion

    #region Properties - Thông tin thao tác

    /// <summary>
    /// Loại thao tác (0=Import, 1=Export, 2=Allocation, 3=Recovery, 4=Transfer, 5=Maintenance, 6=StatusChange, 7=Other)
    /// </summary>
    [DisplayName("Loại thao tác")]
    [Display(Order = 10)]
    [Required(ErrorMessage = "Loại thao tác không được để trống")]
    public int OperationType { get; set; }

    /// <summary>
    /// Tên loại thao tác (từ enum)
    /// </summary>
    [DisplayName("Tên loại thao tác")]
    [Display(Order = 11)]
    public string OperationTypeName { get; set; }

    /// <summary>
    /// Ngày giờ thực hiện thao tác
    /// </summary>
    [DisplayName("Ngày thực hiện")]
    [Display(Order = 12)]
    [Required(ErrorMessage = "Ngày thực hiện không được để trống")]
    public DateTime OperationDate { get; set; }

    #endregion

    #region Properties - Thông tin tham chiếu

    /// <summary>
    /// ID tham chiếu (ví dụ: StockInOutDetailId, DeviceTransferId)
    /// </summary>
    [DisplayName("ID Tham chiếu")]
    [Display(Order = 20)]
    public Guid? ReferenceId { get; set; }

    /// <summary>
    /// Loại tham chiếu (0=StockInOutDetail, 1=DeviceTransfer, 2=Warranty, 3=Other)
    /// </summary>
    [DisplayName("Loại tham chiếu")]
    [Display(Order = 21)]
    public int? ReferenceType { get; set; }

    /// <summary>
    /// Tên loại tham chiếu (từ enum)
    /// </summary>
    [DisplayName("Tên loại tham chiếu")]
    [Display(Order = 22)]
    public string ReferenceTypeName { get; set; }

    #endregion

    #region Properties - Thông tin chi tiết (2 cột theo yêu cầu)

    /// <summary>
    /// Thông tin dạng text (mô tả chi tiết thao tác)
    /// </summary>
    [DisplayName("Thông tin")]
    [Display(Order = 30)]
    public string Information { get; set; }

    /// <summary>
    /// Thông tin dạng HTML (để hiển thị đẹp trên UI)
    /// </summary>
    [DisplayName("Thông tin HTML")]
    [Display(Order = 31)]
    public string HtmlInformation { get; set; }

    #endregion

    #region Properties - Thông tin người thực hiện

    /// <summary>
    /// Người thực hiện thao tác (EmployeeId hoặc ApplicationUserId)
    /// </summary>
    [DisplayName("Người thực hiện")]
    [Display(Order = 40)]
    public Guid? PerformedBy { get; set; }

    /// <summary>
    /// Tên người thực hiện
    /// </summary>
    [DisplayName("Tên người thực hiện")]
    [Display(Order = 41)]
    public string PerformedByName { get; set; }

    #endregion

    #region Properties - Ghi chú

    /// <summary>
    /// Ghi chú bổ sung
    /// </summary>
    [DisplayName("Ghi chú")]
    [Display(Order = 50)]
    [StringLength(1000, ErrorMessage = "Ghi chú không được vượt quá 1000 ký tự")]
    public string Notes { get; set; }

    #endregion

    #region Properties - Audit Fields

    /// <summary>
    /// Ngày tạo bản ghi
    /// </summary>
    [DisplayName("Ngày tạo")]
    [Display(Order = 100)]
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Người tạo bản ghi
    /// </summary>
    [DisplayName("Người tạo")]
    [Display(Order = 101)]
    public Guid? CreatedBy { get; set; }

    /// <summary>
    /// Tên người tạo
    /// </summary>
    [DisplayName("Tên người tạo")]
    [Display(Order = 102)]
    public string CreatedByName { get; set; }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Lấy tên loại thao tác từ enum
    /// </summary>
    public void SetOperationTypeName()
    {
        if (Enum.IsDefined(typeof(DeviceOperationTypeEnum), OperationType))
        {
            var enumValue = (DeviceOperationTypeEnum)OperationType;
            OperationTypeName = GetEnumDescription(enumValue);
        }
        else
        {
            OperationTypeName = "Không xác định";
        }
    }

    /// <summary>
    /// Lấy tên loại tham chiếu từ enum
    /// </summary>
    public void SetReferenceTypeName()
    {
        if (ReferenceType.HasValue && Enum.IsDefined(typeof(DeviceReferenceTypeEnum), ReferenceType.Value))
        {
            var enumValue = (DeviceReferenceTypeEnum)ReferenceType.Value;
            ReferenceTypeName = GetEnumDescription(enumValue);
        }
        else
        {
            ReferenceTypeName = null;
        }
    }

    /// <summary>
    /// Lấy mô tả từ enum
    /// </summary>
    private string GetEnumDescription(Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        if (field == null) return value.ToString();

        var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
        return attribute?.Description ?? value.ToString();
    }

    #endregion
}
