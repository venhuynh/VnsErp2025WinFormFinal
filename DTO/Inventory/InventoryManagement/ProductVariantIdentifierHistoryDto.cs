using System;
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
