using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Dal.DataContext;
using DTO.Inventory.StockIn;

namespace DTO.Inventory.InventoryManagement
{
    /// <summary>
    /// Data Transfer Object cho thông tin bảo hành
    /// </summary>
    public class WarrantyDto
    {
        #region Properties - Thông tin cơ bản

        /// <summary>
        /// ID duy nhất của bảo hành
        /// </summary>
        [DisplayName("ID")]
        [Display(Order = -1)]
        public Guid Id { get; set; }

        /// <summary>
        /// ID chi tiết phiếu nhập/xuất kho
        /// </summary>
        [DisplayName("ID Chi tiết phiếu")]
        [Display(Order = 1)]
        [Required(ErrorMessage = "ID chi tiết phiếu nhập/xuất kho không được để trống")]
        public Guid StockInOutDetailId { get; set; }

        /// <summary>
        /// Ngày bắt đầu bảo hành
        /// </summary>
        [DisplayName("Ngày bắt đầu BH")]
        [Display(Order = 2)]
        public DateTime? WarrantyFrom { get; set; }

        /// <summary>
        /// Số tháng bảo hành
        /// </summary>
        [DisplayName("Số tháng BH")]
        [Display(Order = 3)]
        [Required(ErrorMessage = "Số tháng bảo hành không được để trống")]
        [Range(0, int.MaxValue, ErrorMessage = "Số tháng bảo hành phải lớn hơn hoặc bằng 0")]
        public int MonthOfWarranty { get; set; }

        /// <summary>
        /// Ngày kết thúc bảo hành
        /// </summary>
        [DisplayName("Ngày kết thúc BH")]
        [Display(Order = 4)]
        public DateTime? WarrantyUntil { get; set; }

        /// <summary>
        /// Trạng thái bảo hành
        /// </summary>
        [DisplayName("Trạng thái BH")]
        [Display(Order = 5)]
        [Required(ErrorMessage = "Trạng thái bảo hành không được để trống")]
        public TrangThaiBaoHanhEnum WarrantyStatus { get; set; }

        /// <summary>
        /// Thông tin sản phẩm duy nhất (Serial Number, IMEI, v.v.)
        /// </summary>
        [DisplayName("Thông tin SP duy nhất")]
        [Display(Order = 6)]
        [Required(ErrorMessage = "Thông tin sản phẩm duy nhất không được để trống")]
        [StringLength(200, ErrorMessage = "Thông tin sản phẩm duy nhất không được vượt quá 200 ký tự")]
        public string UniqueProductInfo { get; set; }

        #endregion

        #region Properties - Thông tin hiển thị (Display)

        /// <summary>
        /// Tên trạng thái bảo hành (hiển thị)
        /// </summary>
        [DisplayName("Trạng thái")]
        [Display(Order = 10)]
        public string WarrantyStatusName { get; set; }

        /// <summary>
        /// Kiểm tra bảo hành đã hết hạn chưa (chỉ đọc)
        /// </summary>
        [DisplayName("Hết hạn BH")]
        [Display(Order = 11)]
        [Description("True nếu bảo hành đã hết hạn, False nếu còn bảo hành")]
        public bool IsWarrantyExpired
        {
            get
            {
                // Nếu không có ngày hết hạn, coi như chưa hết hạn
                if (!WarrantyUntil.HasValue)
                    return false;

                // So sánh với ngày hiện tại (chỉ so sánh ngày, không so sánh giờ)
                var today = DateTime.Now.Date;
                var warrantyUntilDate = WarrantyUntil.Value.Date;

                return warrantyUntilDate < today;
            }
        }

        /// <summary>
        /// Tình trạng bảo hành (chỉ đọc) - "Còn bảo hành" hoặc "Hết hạn bảo hành"
        /// </summary>
        [DisplayName("Tình trạng BH")]
        [Display(Order = 12)]
        [Description("Tình trạng bảo hành hiện tại")]
        public string WarrantyStatusText
        {
            get
            {
                if (IsWarrantyExpired)
                    return "Hết hạn bảo hành";
                
                // Nếu không có ngày hết hạn, không thể xác định
                if (!WarrantyUntil.HasValue)
                    return "Chưa xác định";

                return "Còn bảo hành";
            }
        }

        /// <summary>
        /// Tổng hợp thông tin bảo hành (chỉ đọc)
        /// Hiển thị đầy đủ thông tin: sản phẩm, trạng thái, thời gian bảo hành, tình trạng
        /// </summary>
        [DisplayName("Thông tin tổng hợp")]
        [Display(Order = 13)]
        [Description("Tổng hợp đầy đủ thông tin bảo hành")]
        public string FullInfo
        {
            get
            {
                var parts = new List<string>();

                // Thông tin sản phẩm
                if (!string.IsNullOrWhiteSpace(UniqueProductInfo))
                {
                    parts.Add($"SP: {UniqueProductInfo}");
                }

                // Trạng thái bảo hành
                if (!string.IsNullOrWhiteSpace(WarrantyStatusName))
                {
                    parts.Add($"Trạng thái: {WarrantyStatusName}");
                }

                // Thời gian bảo hành
                var timeInfo = new List<string>();
                if (WarrantyFrom.HasValue)
                {
                    timeInfo.Add($"Từ: {WarrantyFrom.Value:dd/MM/yyyy}");
                }
                if (WarrantyUntil.HasValue)
                {
                    timeInfo.Add($"Đến: {WarrantyUntil.Value:dd/MM/yyyy}");
                }
                if (MonthOfWarranty > 0)
                {
                    timeInfo.Add($"{MonthOfWarranty} tháng");
                }
                if (timeInfo.Any())
                {
                    parts.Add($"Thời gian: {string.Join(" - ", timeInfo)}");
                }

                // Tình trạng
                parts.Add($"Tình trạng: {WarrantyStatusText}");

                return string.Join(" | ", parts);
            }
        }

        #endregion
    }


    /// <summary>
    /// Enum định nghĩa các trạng thái bảo hành
    /// </summary>
    public enum TrangThaiBaoHanhEnum
    {
        /// <summary>
        /// Chờ xử lý - Yêu cầu bảo hành đã được tạo, chờ xử lý
        /// </summary>
        [Description("Chờ xử lý")]
        ChoXuLy = 1,

        /// <summary>
        /// Đang bảo hành - Sản phẩm đang trong quá trình bảo hành
        /// </summary>
        [Description("Đang bảo hành")]
        DangBaoHanh = 2,

        /// <summary>
        /// Đã hoàn thành - Bảo hành đã hoàn tất
        /// </summary>
        [Description("Đã hoàn thành")]
        DaHoanThanh = 3,

        /// <summary>
        /// Đã từ chối - Yêu cầu bảo hành bị từ chối
        /// </summary>
        [Description("Đã từ chối")]
        DaTuChoi = 4,

        /// <summary>
        /// Đã hủy - Yêu cầu bảo hành đã bị hủy
        /// </summary>
        [Description("Đã hủy")]
        DaHuy = 99
    }

    /// <summary>
    /// Converter giữa Warranty entity và WarrantyDto
    /// </summary>
    public static class WarrantyDtoConverter
    {
        #region Entity to DTO

        /// <summary>
        /// Chuyển đổi Warranty entity thành WarrantyDto
        /// </summary>
        /// <param name="entity">Warranty entity</param>
        /// <returns>WarrantyDto</returns>
        public static WarrantyDto ToDto(this Warranty entity)
        {
            if (entity == null) return null;

            var dto = new WarrantyDto
            {
                Id = entity.Id,
                StockInOutDetailId = entity.StockInOutDetailId,
                WarrantyFrom = entity.WarrantyFrom,
                MonthOfWarranty = entity.MonthOfWarranty,
                WarrantyUntil = entity.WarrantyUntil,
                UniqueProductInfo = entity.UniqueProductInfo
            };

            // Chuyển đổi WarrantyStatus từ int sang enum
            if (Enum.IsDefined(typeof(TrangThaiBaoHanhEnum), entity.WarrantyStatus))
            {
                dto.WarrantyStatus = (TrangThaiBaoHanhEnum)entity.WarrantyStatus;
            }
            else
            {
                // Nếu giá trị không hợp lệ, mặc định là ChoXuLy
                dto.WarrantyStatus = TrangThaiBaoHanhEnum.ChoXuLy;
            }

            // Lấy tên trạng thái từ Description attribute
            dto.WarrantyStatusName = GetEnumDescription(dto.WarrantyStatus);

            return dto;
        }

        /// <summary>
        /// Chuyển đổi danh sách Warranty entities thành danh sách WarrantyDto
        /// </summary>
        /// <param name="entities">Danh sách Warranty entities</param>
        /// <returns>Danh sách WarrantyDto</returns>
        public static List<WarrantyDto> ToDtoList(this IEnumerable<Warranty> entities)
        {
            if (entities == null) return new List<WarrantyDto>();

            return entities.Select(entity => entity.ToDto()).ToList();
        }

        #endregion

        #region DTO to Entity

        /// <summary>
        /// Chuyển đổi WarrantyDto thành Warranty entity
        /// </summary>
        /// <param name="dto">WarrantyDto</param>
        /// <returns>Warranty entity</returns>
        public static Warranty ToEntity(this WarrantyDto dto)
        {
            if (dto == null) return null;

            return new Warranty
            {
                Id = dto.Id,
                StockInOutDetailId = dto.StockInOutDetailId,
                WarrantyFrom = dto.WarrantyFrom,
                MonthOfWarranty = dto.MonthOfWarranty,
                WarrantyUntil = dto.WarrantyUntil,
                WarrantyStatus = (int)dto.WarrantyStatus, // Chuyển đổi enum sang int
                UniqueProductInfo = dto.UniqueProductInfo
            };
        }

        /// <summary>
        /// Chuyển đổi danh sách WarrantyDto thành danh sách Warranty entities
        /// </summary>
        /// <param name="dtos">Danh sách WarrantyDto</param>
        /// <returns>Danh sách Warranty entities</returns>
        public static List<Warranty> ToEntityList(this IEnumerable<WarrantyDto> dtos)
        {
            if (dtos == null) return new List<Warranty>();

            return dtos.Select(dto => dto.ToEntity()).ToList();
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Lấy Description từ enum value
        /// </summary>
        /// <param name="enumValue">Giá trị enum</param>
        /// <returns>Description hoặc tên enum nếu không có Description</returns>
        private static string GetEnumDescription(TrangThaiBaoHanhEnum enumValue)
        {
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
            if (fieldInfo == null) return enumValue.ToString();

            var descriptionAttribute = fieldInfo.GetCustomAttribute<DescriptionAttribute>();
            return descriptionAttribute?.Description ?? enumValue.ToString();
        }

        #endregion
    }
}
