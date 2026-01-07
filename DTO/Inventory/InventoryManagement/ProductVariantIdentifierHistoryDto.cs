using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DTO.Inventory.InventoryManagement
{
    /// <summary>
    /// Enum định nghĩa các loại thay đổi trong lịch sử định danh sản phẩm
    /// </summary>
    public enum ProductVariantIdentifierHistoryChangeTypeEnum
    {
        /// <summary>
        /// Nhập
        /// </summary>
        [Description("Nhập")]
        Nhap = 1,

        /// <summary>
        /// Xuất
        /// </summary>
        [Description("Xuất")]
        Xuat = 2,

        /// <summary>
        /// Hỗ trợ kỹ thuật
        /// </summary>
        [Description("Hỗ trợ kỹ thuật")]
        HoTroKyThuat = 3,

        /// <summary>
        /// Thay đổi thông số
        /// </summary>
        [Description("Thay đổi thông số")]
        ThayDoiThongSo = 4,

        /// <summary>
        /// Thay đổi người dùng
        /// </summary>
        [Description("Thay đổi người dùng")]
        ThayDoiNguoiDung = 5,

        /// <summary>
        /// Khác
        /// </summary>
        [Description("Khác")]
        Khac = 99
    }

    /// <summary>
    /// Data Transfer Object cho ProductVariantIdentifierHistory entity
    /// Lưu trữ lịch sử thay đổi của ProductVariantIdentifier
    /// </summary>
    public class ProductVariantIdentifierHistoryDto
    {
        #region Properties - Thông tin cơ bản

        /// <summary>
        /// ID duy nhất của bản ghi lịch sử
        /// </summary>
        [DisplayName("ID")]
        [Display(Order = -1)]
        public Guid Id { get; set; }

        /// <summary>
        /// ID định danh sản phẩm (ProductVariantIdentifier)
        /// </summary>
        [DisplayName("ID Định danh sản phẩm")]
        [Display(Order = 0)]
        [Required(ErrorMessage = "ID định danh sản phẩm không được để trống")]
        public Guid ProductVariantIdentifierId { get; set; }

        /// <summary>
        /// ID biến thể sản phẩm (ProductVariant)
        /// </summary>
        [DisplayName("ID Biến thể sản phẩm")]
        [Display(Order = 1)]
        [Required(ErrorMessage = "ID biến thể sản phẩm không được để trống")]
        public Guid ProductVariantId { get; set; }

        /// <summary>
        /// Tên biến thể sản phẩm đầy đủ (từ ProductVariant.VariantFullName)
        /// </summary>
        [DisplayName("Tên biến thể sản phẩm")]
        [Display(Order = 2)]
        [StringLength(500, ErrorMessage = "Tên biến thể sản phẩm không được vượt quá 500 ký tự")]
        public string ProductVariantFullName { get; set; }

        #endregion

        #region Properties - Thông tin thay đổi

        /// <summary>
        /// Loại thay đổi: 1=Nhập, 2=Xuất, 3=Hỗ trợ kỹ thuật, 4=Thay đổi thông số, 99=Khác
        /// </summary>
        [DisplayName("Loại thay đổi")]
        [Display(Order = 10)]
        [Required(ErrorMessage = "Loại thay đổi không được để trống")]
        public int ChangeType { get; set; }

        /// <summary>
        /// Loại thay đổi dưới dạng enum
        /// </summary>
        [DisplayName("Loại thay đổi")]
        [Display(Order = 11)]
        [Description("Loại thay đổi dưới dạng enum")]
        public ProductVariantIdentifierHistoryChangeTypeEnum ChangeTypeEnum
        {
            get
            {
                return ChangeType switch
                {
                    1 => ProductVariantIdentifierHistoryChangeTypeEnum.Nhap,
                    2 => ProductVariantIdentifierHistoryChangeTypeEnum.Xuat,
                    3 => ProductVariantIdentifierHistoryChangeTypeEnum.HoTroKyThuat,
                    4 => ProductVariantIdentifierHistoryChangeTypeEnum.ThayDoiThongSo,
                    5 => ProductVariantIdentifierHistoryChangeTypeEnum.ThayDoiNguoiDung,
                    99 => ProductVariantIdentifierHistoryChangeTypeEnum.Khac,
                    _ => ProductVariantIdentifierHistoryChangeTypeEnum.Khac
                };
            }
            set
            {
                ChangeType = value switch
                {
                    ProductVariantIdentifierHistoryChangeTypeEnum.Nhap => 1,
                    ProductVariantIdentifierHistoryChangeTypeEnum.Xuat => 2,
                    ProductVariantIdentifierHistoryChangeTypeEnum.HoTroKyThuat => 3,
                    ProductVariantIdentifierHistoryChangeTypeEnum.ThayDoiThongSo => 4,
                    ProductVariantIdentifierHistoryChangeTypeEnum.ThayDoiNguoiDung => 5,
                    ProductVariantIdentifierHistoryChangeTypeEnum.Khac => 99,
                    _ => 99
                };
            }
        }

        /// <summary>
        /// Ngày thay đổi
        /// </summary>
        [DisplayName("Ngày thay đổi")]
        [Display(Order = 12)]
        [Required(ErrorMessage = "Ngày thay đổi không được để trống")]
        public DateTime ChangeDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Người thay đổi (EmployeeId hoặc ApplicationUserId)
        /// </summary>
        [DisplayName("Người thay đổi")]
        [Display(Order = 13)]
        public Guid? ChangedBy { get; set; }

        /// <summary>
        /// Giá trị thay đổi (thay thế cho OldValue/NewValue/FieldName/Description)
        /// </summary>
        [DisplayName("Giá trị thay đổi")]
        [Display(Order = 14)]
        [StringLength(1000, ErrorMessage = "Giá trị thay đổi không được vượt quá 1000 ký tự")]
        public string Value { get; set; }

        /// <summary>
        /// Ghi chú về thay đổi
        /// </summary>
        [DisplayName("Ghi chú")]
        [Display(Order = 15)]
        [StringLength(1000, ErrorMessage = "Ghi chú không được vượt quá 1000 ký tự")]
        public string Notes { get; set; }

        #endregion

        #region Properties - HTML Display

        /// <summary>
        /// Loại thay đổi dưới dạng HTML theo format DevExpress
        /// Sử dụng các tag HTML chuẩn của DevExpress: &lt;b&gt;, &lt;i&gt;, &lt;color&gt;
        /// Tham khảo: https://docs.devexpress.com/WindowsForms/4874/common-features/html-text-formatting
        /// </summary>
        [DisplayName("Loại thay đổi HTML")]
        [Display(Order = 20)]
        [Description("Loại thay đổi dưới dạng HTML")]
        public string ChangeTypeHtml
        {
            get
            {
                var changeTypeDescription = GetChangeTypeDescription();
                if (string.IsNullOrWhiteSpace(changeTypeDescription))
                    return string.Empty;

                // Màu sắc phù hợp với từng loại thay đổi
                string color;
                switch (ChangeTypeEnum)
                {
                    case ProductVariantIdentifierHistoryChangeTypeEnum.Nhap:
                        color = "#4CAF50"; // Xanh lá (nhập)
                        break;
                    case ProductVariantIdentifierHistoryChangeTypeEnum.Xuat:
                        color = "#F44336"; // Đỏ (xuất)
                        break;
                    case ProductVariantIdentifierHistoryChangeTypeEnum.HoTroKyThuat:
                        color = "#2196F3"; // Xanh dương (hỗ trợ)
                        break;
                    case ProductVariantIdentifierHistoryChangeTypeEnum.ThayDoiThongSo:
                        color = "#FF9800"; // Cam (thay đổi)
                        break;
                    case ProductVariantIdentifierHistoryChangeTypeEnum.ThayDoiNguoiDung:
                        color = "#9C27B0"; // Tím (người dùng)
                        break;
                    default:
                        color = "#757575"; // Xám (khác)
                        break;
                }

                return $"<b><color='{color}'>{changeTypeDescription}</color></b>";
            }
        }

        /// <summary>
        /// Giá trị thay đổi dưới dạng HTML theo format DevExpress
        /// Sử dụng các tag HTML chuẩn của DevExpress: &lt;b&gt;, &lt;i&gt;, &lt;color&gt;
        /// </summary>
        [DisplayName("Giá trị thay đổi HTML")]
        [Display(Order = 21)]
        [Description("Giá trị thay đổi dưới dạng HTML")]
        public string ValueHtml
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Value))
                    return string.Empty;

                // Format giá trị với màu xám cho label, đen cho value
                return $"<color='#757575'>Giá trị:</color> <color='#212121'><b>{Value}</b></color>";
            }
        }

        /// <summary>
        /// Thông tin lịch sử thay đổi dưới dạng HTML theo format DevExpress
        /// Sử dụng các tag HTML chuẩn của DevExpress: &lt;b&gt;, &lt;i&gt;, &lt;color&gt;
        /// Tham khảo: https://docs.devexpress.com/WindowsForms/4874/common-features/html-text-formatting
        /// Format tương tự BusinessPartnerListDto.ThongTinHtml để đảm bảo consistency
        /// </summary>
        [DisplayName("Thông tin HTML")]
        [Display(Order = 22)]
        [Description("Thông tin lịch sử thay đổi dưới dạng HTML")]
        public string ThongTinHtml
        {
            get
            {
                var htmlParts = new List<string>();

                // Loại thay đổi (primary info)
                var changeTypeDescription = GetChangeTypeDescription();
                if (!string.IsNullOrWhiteSpace(changeTypeDescription))
                {
                    string color;
                    switch (ChangeTypeEnum)
                    {
                        case ProductVariantIdentifierHistoryChangeTypeEnum.Nhap:
                            color = "#4CAF50";
                            break;
                        case ProductVariantIdentifierHistoryChangeTypeEnum.Xuat:
                            color = "#F44336";
                            break;
                        case ProductVariantIdentifierHistoryChangeTypeEnum.HoTroKyThuat:
                            color = "#2196F3";
                            break;
                        case ProductVariantIdentifierHistoryChangeTypeEnum.ThayDoiThongSo:
                            color = "#FF9800";
                            break;
                        case ProductVariantIdentifierHistoryChangeTypeEnum.ThayDoiNguoiDung:
                            color = "#9C27B0";
                            break;
                        default:
                            color = "#757575";
                            break;
                    }
                    htmlParts.Add($"<b><color='{color}'>{changeTypeDescription}</color></b>");
                }

                // Ngày thay đổi
                if (ChangeDate != default(DateTime))
                {
                    htmlParts.Add($"<br><color='#757575'>Ngày:</color> <color='#212121'><b>{ChangeDate:dd/MM/yyyy HH:mm}</b></color>");
                }

                // Giá trị thay đổi (nếu có)
                if (!string.IsNullOrWhiteSpace(Value))
                {
                    htmlParts.Add($"<br><color='#757575'>Giá trị:</color> <color='#212121'><b>{Value}</b></color>");
                }

                // Ghi chú (nếu có)
                if (!string.IsNullOrWhiteSpace(Notes))
                {
                    htmlParts.Add($"<br><color='#757575'>Ghi chú:</color> <color='#212121'><i>{Notes}</i></color>");
                }

                return string.Join(string.Empty, htmlParts);
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Lấy mô tả loại thay đổi
        /// </summary>
        /// <returns>Mô tả loại thay đổi</returns>
        public string GetChangeTypeDescription()
        {
            var field = ChangeTypeEnum.GetType().GetField(ChangeTypeEnum.ToString());
            var attribute = field?.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
            return attribute?.Length > 0 ? attribute[0].Description : ChangeTypeEnum.ToString();
        }

        /// <summary>
        /// Kiểm tra xem có giá trị thay đổi không
        /// </summary>
        /// <returns>True nếu có giá trị thay đổi</returns>
        public bool HasValue()
        {
            return !string.IsNullOrWhiteSpace(Value);
        }

        /// <summary>
        /// Kiểm tra xem có ghi chú không
        /// </summary>
        /// <returns>True nếu có ghi chú</returns>
        public bool HasNotes()
        {
            return !string.IsNullOrWhiteSpace(Notes);
        }

        #endregion
    }
}
