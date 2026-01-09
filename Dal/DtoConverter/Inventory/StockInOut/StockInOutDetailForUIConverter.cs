using System.Collections.Generic;
using System.Linq;
using Dal.DataContext;
using DTO.DeviceAssetManagement;
using DTO.Inventory;

namespace Dal.DtoConverter.Inventory.StockInOut
{
    /// <summary>
    /// Converter giữa StockInOutDetail entity và StockInOutDetailForUIDto
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public static class StockInOutDetailForUIConverter
    {
        #region Entity to DTO

        /// <summary>
        /// Chuyển đổi StockInOutDetail entity thành StockInOutDetailForUIDto
        /// </summary>
        /// <param name="entity">StockInOutDetail entity</param>
        /// <param name="lineNumber">Thứ tự dòng (dùng cho UI)</param>
        /// <returns>StockInOutDetailForUIDto</returns>
        public static StockInOutDetailForUIDto ToDto(this StockInOutDetail entity, int lineNumber = 0)
        {
            if (entity == null) return null;

            var dto = new StockInOutDetailForUIDto
            {
                // Thông tin cơ bản
                Id = entity.Id,
                StockInOutMasterId = entity.StockInOutMasterId,
                ProductVariantId = entity.ProductVariantId,
                LineNumber = lineNumber,

                // Số lượng và giá
                StockOutQty = entity.StockOutQty,
                StockInQty = entity.StockInQty,
                UnitPrice = entity.UnitPrice,
                Vat = entity.Vat,

                // Tình trạng sản phẩm (mặc định)
                GhiChu = "Bình thường",

            };

            // Map thông tin từ ProductVariant nếu đã được load
            if (entity.ProductVariant != null)
            {
                var productVariant = entity.ProductVariant;

                // Mã và tên biến thể sản phẩm
                dto.ProductVariantCode = productVariant.VariantCode ?? string.Empty;
                
                // Ưu tiên VariantFullName, nếu không có thì lấy từ ProductService.Name
                if (!string.IsNullOrWhiteSpace(productVariant.VariantFullName))
                {
                    dto.ProductVariantName = productVariant.VariantFullName;
                }
                else if (productVariant.ProductService != null)
                {
                    dto.ProductVariantName = productVariant.ProductService.Name ?? string.Empty;
                }

                // Đơn vị tính
                dto.UnitOfMeasureId = productVariant.UnitId;

                if (productVariant.UnitOfMeasure != null)
                {
                    dto.UnitOfMeasureCode = productVariant.UnitOfMeasure.Code ?? string.Empty;
                    dto.UnitOfMeasureName = productVariant.UnitOfMeasure.Name ?? string.Empty;
                }
            }

            // Lưu ý: TotalAmount, VatAmount, TotalAmountIncludedVat là computed properties trong DTO
            // Chúng sẽ tự động tính toán từ StockOutQty, UnitPrice, Vat
            // Không cần map từ entity vì entity đã lưu các giá trị này, nhưng DTO tính lại để đảm bảo tính nhất quán

            return dto;
        }

        /// <summary>
        /// Chuyển đổi danh sách StockInOutDetail entities thành danh sách StockInOutDetailForUIDto
        /// </summary>
        /// <param name="entities">Danh sách StockInOutDetail entities</param>
        /// <returns>Danh sách StockInOutDetailForUIDto</returns>
        public static List<StockInOutDetailForUIDto> ToDtoList(this IEnumerable<StockInOutDetail> entities)
        {
            if (entities == null) return new List<StockInOutDetailForUIDto>();

            return entities.Select((entity, index) => entity.ToDto(index + 1)).ToList();
        }

        /// <summary>
        /// Chuyển đổi StockInOutDetail entity thành StockInOutDetailForUIDto với Devices và Warranties
        /// </summary>
        /// <param name="entity">StockInOutDetail entity</param>
        /// <param name="lineNumber">Thứ tự dòng</param>
        /// <param name="devices">Danh sách DeviceDto (tùy chọn)</param>
        /// <param name="warranties">Danh sách WarrantyDto (tùy chọn)</param>
        /// <returns>StockInOutDetailForUIDto</returns>
        public static StockInOutDetailForUIDto ToDtoWithRelatedData(
            this StockInOutDetail entity,
            int lineNumber = 0,
            List<DeviceDto> devices = null,
            List<WarrantyDto> warranties = null)
        {
            var dto = entity.ToDto(lineNumber);

            if (dto == null) return null;
             

            return dto;
        }

        #endregion

        #region DTO to Entity

        /// <summary>
        /// Chuyển đổi StockInOutDetailForUIDto thành StockInOutDetail entity
        /// </summary>
        /// <param name="dto">StockInOutDetailForUIDto</param>
        /// <returns>StockInOutDetail entity</returns>
        public static StockInOutDetail ToEntity(this StockInOutDetailForUIDto dto)
        {
            if (dto == null) return null;

            // Tính toán các giá trị từ computed properties trong DTO
            // DTO có computed properties: TotalAmount, VatAmount, TotalAmountIncludedVat
            // Nhưng khi save, ta cần lưu các giá trị đã tính toán vào entity
            var totalAmount = dto.TotalAmount; // Computed: StockOutQty * UnitPrice
            var vatAmount = dto.VatAmount; // Computed: TotalAmount * (Vat / 100)
            var totalAmountIncludedVat = dto.TotalAmountIncludedVat; // Computed: TotalAmount + VatAmount

            var entity = new StockInOutDetail
            {
                Id = dto.Id,
                StockInOutMasterId = dto.StockInOutMasterId,
                ProductVariantId = dto.ProductVariantId,
                StockInQty = dto.StockInQty,
                StockOutQty = dto.StockOutQty,
                UnitPrice = dto.UnitPrice,
                Vat = dto.Vat,
                VatAmount = vatAmount,
                TotalAmount = totalAmount,
                TotalAmountIncludedVat = totalAmountIncludedVat
            };

            return entity;
        }

        /// <summary>
        /// Chuyển đổi danh sách StockInOutDetailForUIDto thành danh sách StockInOutDetail entities
        /// </summary>
        /// <param name="dtos">Danh sách StockInOutDetailForUIDto</param>
        /// <returns>Danh sách StockInOutDetail entities</returns>
        public static List<StockInOutDetail> ToEntityList(this IEnumerable<StockInOutDetailForUIDto> dtos)
        {
            if (dtos == null) return new List<StockInOutDetail>();

            return dtos.Select(dto => dto.ToEntity()).ToList();
        }

        #endregion
    }
}
