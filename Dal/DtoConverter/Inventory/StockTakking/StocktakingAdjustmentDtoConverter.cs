using DTO.Inventory.StockTakking;
using System;
using System.Collections.Generic;
using System.Linq;
using StocktakingAdjustment = Dal.DataContext.StocktakingAdjustment;

namespace Dal.DtoConverter.Inventory.StockTakking
{
    /// <summary>
    /// Converter giữa StocktakingAdjustment entity và StocktakingAdjustmentDto
    /// </summary>
    public static class StocktakingAdjustmentDtoConverter
    {
        #region Entity to DTO

        /// <summary>
        /// Chuyển đổi StocktakingAdjustment entity thành StocktakingAdjustmentDto
        /// </summary>
        /// <param name="entity">StocktakingAdjustment entity</param>
        /// <param name="productVariantName">Tên biến thể sản phẩm (tùy chọn, để tránh DataContext disposed errors)</param>
        /// <param name="productVariantCode">Mã sản phẩm (tùy chọn)</param>
        /// <returns>StocktakingAdjustmentDto</returns>
        public static StocktakingAdjustmentDto ToDto(this StocktakingAdjustment entity,
            string productVariantName = null,
            string productVariantCode = null)
        {
            if (entity == null) return null;

            // QUAN TRỌNG: KHÔNG truy cập navigation properties (entity.ProductVariant, entity.StocktakingMaster, etc.)
            // vì DataContext đã bị dispose sau khi repository method kết thúc
            // Chỉ sử dụng tham số truyền vào

            var dto = new StocktakingAdjustmentDto
            {
                // Thông tin cơ bản
                Id = entity.Id,
                StocktakingMasterId = entity.StocktakingMasterId,
                StocktakingDetailId = entity.StocktakingDetailId,
                StockInOutMasterId = entity.StockInOutMasterId,
                ProductVariantId = entity.ProductVariantId,
                ProductVariantName = productVariantName,
                ProductVariantCode = productVariantCode,

                // Thông tin điều chỉnh
                AdjustmentQuantity = entity.AdjustmentQuantity,
                AdjustmentValue = entity.AdjustmentValue,
                UnitPrice = entity.UnitPrice,
                AdjustmentType = entity.AdjustmentType,
                AdjustmentReason = entity.AdjustmentReason,
                AdjustmentDate = entity.AdjustmentDate,
                AdjustedBy = entity.AdjustedBy,

                // Thông tin bổ sung
                Notes = entity.Notes,

                // Trạng thái áp dụng
                IsApplied = entity.IsApplied,
                AppliedDate = entity.AppliedDate,

                // Trạng thái
                IsActive = entity.IsActive,
                IsDeleted = entity.IsDeleted,

                // Audit fields
                CreatedBy = entity.CreatedBy,
                CreatedDate = entity.CreatedDate,
                UpdatedBy = entity.UpdatedBy,
                UpdatedDate = entity.UpdatedDate
            };

            return dto;
        }

        /// <summary>
        /// Chuyển đổi danh sách StocktakingAdjustment entities thành danh sách StocktakingAdjustmentDto
        /// </summary>
        /// <param name="entities">Danh sách StocktakingAdjustment entities</param>
        /// <param name="productVariantDict">Dictionary chứa thông tin ProductVariant (key: ProductVariantId, value: (ProductVariantName, ProductVariantCode))</param>
        /// <returns>Danh sách StocktakingAdjustmentDto</returns>
        public static List<StocktakingAdjustmentDto> ToDtoList(this IEnumerable<StocktakingAdjustment> entities,
            Dictionary<Guid, (string ProductVariantName, string ProductVariantCode)> productVariantDict = null)
        {
            if (entities == null) return new List<StocktakingAdjustmentDto>();

            return entities.Select(entity =>
            {
                string productVariantName = null;
                string productVariantCode = null;

                if (productVariantDict != null && productVariantDict.TryGetValue(entity.ProductVariantId, out var productInfo))
                {
                    productVariantName = productInfo.ProductVariantName;
                    productVariantCode = productInfo.ProductVariantCode;
                }

                return entity.ToDto(productVariantName, productVariantCode);
            }).ToList();
        }

        /// <summary>
        /// Chuyển đổi danh sách StocktakingAdjustment entities thành danh sách StocktakingAdjustmentDto (alias)
        /// </summary>
        /// <param name="entities">Danh sách StocktakingAdjustment entities</param>
        /// <returns>Danh sách StocktakingAdjustmentDto</returns>
        public static List<StocktakingAdjustmentDto> ToDtos(this IEnumerable<StocktakingAdjustment> entities)
        {
            if (entities == null) return new List<StocktakingAdjustmentDto>();
            return entities.Select(e => e.ToDto(null, null)).ToList();
        }

        #endregion

        #region DTO to Entity

        /// <summary>
        /// Chuyển đổi StocktakingAdjustmentDto thành StocktakingAdjustment entity
        /// </summary>
        /// <param name="dto">StocktakingAdjustmentDto</param>
        /// <param name="existingEntity">Existing entity to update (optional, for edit mode)</param>
        /// <returns>StocktakingAdjustment entity</returns>
        public static StocktakingAdjustment ToEntity(this StocktakingAdjustmentDto dto, StocktakingAdjustment existingEntity = null)
        {
            if (dto == null) return null;

            StocktakingAdjustment entity;
            if (existingEntity != null)
            {
                // Update existing entity
                entity = existingEntity;
            }
            else
            {
                // Create new entity
                entity = new StocktakingAdjustment();
                if (dto.Id != Guid.Empty)
                {
                    entity.Id = dto.Id;
                }
                else
                {
                    entity.Id = Guid.NewGuid();
                }
            }

            // Map properties - Thông tin cơ bản
            entity.StocktakingMasterId = dto.StocktakingMasterId;
            entity.StocktakingDetailId = dto.StocktakingDetailId;
            entity.StockInOutMasterId = dto.StockInOutMasterId;
            entity.ProductVariantId = dto.ProductVariantId;

            // Map properties - Thông tin điều chỉnh
            entity.AdjustmentQuantity = dto.AdjustmentQuantity;
            entity.AdjustmentValue = dto.AdjustmentValue;
            entity.UnitPrice = dto.UnitPrice;
            entity.AdjustmentType = dto.AdjustmentType;
            entity.AdjustmentReason = dto.AdjustmentReason;
            entity.AdjustmentDate = dto.AdjustmentDate;
            entity.AdjustedBy = dto.AdjustedBy;

            // Map properties - Thông tin bổ sung
            entity.Notes = dto.Notes;

            // Map properties - Trạng thái áp dụng
            entity.IsApplied = dto.IsApplied;
            entity.AppliedDate = dto.AppliedDate;

            // Map properties - Trạng thái
            entity.IsActive = dto.IsActive;
            entity.IsDeleted = dto.IsDeleted;

            // Map properties - Audit fields
            entity.UpdatedDate = dto.UpdatedDate ?? DateTime.Now;
            entity.UpdatedBy = dto.UpdatedBy;

            if (existingEntity == null)
            {
                // Chỉ set CreatedDate và CreatedBy khi tạo mới
                entity.CreatedDate = dto.CreatedDate ?? DateTime.Now;
                entity.CreatedBy = dto.CreatedBy;
            }

            return entity;
        }

        /// <summary>
        /// Chuyển đổi danh sách StocktakingAdjustmentDto thành danh sách StocktakingAdjustment entities
        /// </summary>
        /// <param name="dtos">Danh sách StocktakingAdjustmentDto</param>
        /// <returns>Danh sách StocktakingAdjustment entities</returns>
        public static List<StocktakingAdjustment> ToEntityList(this IEnumerable<StocktakingAdjustmentDto> dtos)
        {
            if (dtos == null) return new List<StocktakingAdjustment>();
            return dtos.Select(d => d.ToEntity()).Where(e => e != null).ToList();
        }

        /// <summary>
        /// Chuyển đổi danh sách StocktakingAdjustmentDto thành danh sách StocktakingAdjustment entities (alias)
        /// </summary>
        /// <param name="dtos">Danh sách StocktakingAdjustmentDto</param>
        /// <returns>Danh sách StocktakingAdjustment entities</returns>
        public static List<StocktakingAdjustment> ToEntities(this IEnumerable<StocktakingAdjustmentDto> dtos)
        {
            if (dtos == null) return new List<StocktakingAdjustment>();
            return dtos.Select(d => d.ToEntity()).Where(e => e != null).ToList();
        }

        #endregion
    }
}
