using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Common.Utils;

namespace DTO.DeviceAssetManagement;

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
    /// Loại thao tác (Import, Export, Allocation, Recovery, Transfer, Maintenance, StatusChange, Other)
    /// </summary>
    [DisplayName("Loại thao tác")]
    [Display(Order = 10)]
    [Required(ErrorMessage = "Loại thao tác không được để trống")]
    public DeviceOperationTypeEnum OperationType { get; set; }

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

    #region Properties - Thông tin hiển thị (Display - chỉ đọc, không lưu vào database)

    /// <summary>
    /// Tên thiết bị (lấy từ Device.ProductVariant)
    /// </summary>
    [DisplayName("Tên thiết bị")]
    [Display(Order = 200)]
    public string DeviceName { get; set; }

    /// <summary>
    /// Mã thiết bị (lấy từ Device.ProductVariant)
    /// </summary>
    [DisplayName("Mã thiết bị")]
    [Display(Order = 201)]
    public string DeviceCode { get; set; }

    /// <summary>
    /// Thông tin thiết bị (Serial Number, IMEI, MAC, v.v.)
    /// </summary>
    [DisplayName("Thông tin thiết bị")]
    [Display(Order = 202)]
    public string DeviceInfo { get; set; }

    /// <summary>
    /// Nội dung tổng quát lịch sử giao dịch theo định dạng HTML theo chuẩn DevExpress
    /// Sử dụng các tag HTML chuẩn của DevExpress: &lt;b&gt;, &lt;i&gt;, &lt;color&gt;, &lt;size&gt;
    /// Tham khảo: https://docs.devexpress.com/WindowsForms/4874/common-features/html-text-formatting
    /// </summary>
    [DisplayName("Nội dung HTML")]
    [Display(Order = 250)]
    [Description("Nội dung tổng quát lịch sử giao dịch dưới dạng HTML")]
    public string FullContentHtml
    {
        get
        {
            var html = string.Empty;

            // Ngày thực hiện
            if (OperationDate != default(DateTime))
            {
                if (!string.IsNullOrWhiteSpace(html))
                    html += " ";
                html += $"<color='#757575'>({OperationDate:dd/MM/yyyy HH:mm})</color>";
            }

            if (!string.IsNullOrWhiteSpace(html))
                html += "<br>";

            // Thông tin thiết bị
            if (!string.IsNullOrWhiteSpace(DeviceName))
            {
                html += $"<color='blue'><b>{DeviceName}</b></color>";
                if (!string.IsNullOrWhiteSpace(DeviceCode))
                {
                    html += $" <color='#757575'>({DeviceCode})</color>";
                }
                html += "<br>";
            }

            // Thông tin định danh thiết bị
            if (!string.IsNullOrWhiteSpace(DeviceInfo))
            {
                html += $"<color='#757575'>Thiết bị:</color> <color='#212121'>{DeviceInfo}</color><br>";
            }

            // Thông tin chi tiết (ưu tiên HtmlInformation, fallback về Information)
            if (!string.IsNullOrWhiteSpace(HtmlInformation))
            {
                html += $"{HtmlInformation}<br>";
            }
            else if (!string.IsNullOrWhiteSpace(Information))
            {
                html += $"<color='#212121'>{Information}</color><br>";
            }

            // Thông tin tham chiếu
            if (ReferenceId.HasValue)
            {
                var referenceInfo = string.Empty;
                if (!string.IsNullOrWhiteSpace(ReferenceTypeName))
                {
                    referenceInfo = ReferenceTypeName;
                }
                else if (ReferenceType.HasValue && Enum.IsDefined(typeof(DeviceReferenceTypeEnum), ReferenceType.Value))
                {
                    var enumValue = (DeviceReferenceTypeEnum)ReferenceType.Value;
                    referenceInfo = ApplicationEnumUtils.GetDescription(enumValue);
                }
                else
                {
                    referenceInfo = "Tham chiếu";
                }

                html += $"<color='#757575'>Tham chiếu:</color> <color='#212121'><b>{referenceInfo}</b></color>";
                html += $" <color='#757575'>({ReferenceId.Value})</color><br>";
            }

            // Người thực hiện
            if (!string.IsNullOrWhiteSpace(PerformedByName))
            {
                html += $"<color='#757575'>Người thực hiện:</color> <color='#212121'><b>{PerformedByName}</b></color><br>";
            }

            // Ghi chú
            if (!string.IsNullOrWhiteSpace(Notes))
            {
                html += $"<color='#757575'><i>{Notes}</i></color><br>";
            }

            // Thông tin audit
            var auditParts = new List<string>();
            if (CreatedDate != default(DateTime))
            {
                auditParts.Add($"Tạo: {CreatedDate:dd/MM/yyyy HH:mm}");
            }
            if (!string.IsNullOrWhiteSpace(CreatedByName))
            {
                auditParts.Add($"Bởi: {CreatedByName}");
            }

            if (auditParts.Any())
            {
                html += $"<color='#9E9E9E'>{string.Join(" | ", auditParts)}</color>";
            }

            return html;
        }
    }

    #endregion

    #region Helper Methods


    /// <summary>
    /// Lấy tên loại tham chiếu từ enum
    /// </summary>
    public void SetReferenceTypeName()
    {
        if (ReferenceType.HasValue && Enum.IsDefined(typeof(DeviceReferenceTypeEnum), ReferenceType.Value))
        {
            var enumValue = (DeviceReferenceTypeEnum)ReferenceType.Value;
            ReferenceTypeName = ApplicationEnumUtils.GetDescription(enumValue);
        }
        else
        {
            ReferenceTypeName = null;
        }
    }

    #endregion
}
