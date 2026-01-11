using DTO.Inventory.StockTakking;
using System;
using System.Collections.Generic;
using System.Linq;
using StocktakingDetail = Dal.DataContext.StocktakingDetail;

namespace Dal.DtoConverter.Inventory.StockTakking
{
    /// <summary>
    /// Converter giữa StocktakingDetail entity và StocktakingDetailDto
    /// </summary>
    public static class StocktakingDetailDtoConverter
    {
        #region Entity to DTO

        /// <summary>
        /// Chuyển đổi StocktakingDetail entity thành StocktakingDetailDto
        /// </summary>
        /// <param name="entity">StocktakingDetail entity</param>
        /// <param name="productVariantName">Tên biến thể sản phẩm (tùy chọn, để tránh DataContext disposed errors)</param>
        /// <param name="productVariantCode">Mã sản phẩm (tùy chọn)</param>
        /// <param name="productVariantUnitName">Đơn vị tính của biến thể sản phẩm (tùy chọn)</param>
        /// <returns>StocktakingDetailDto</returns>
        public static StocktakingDetailDto ToDto(this StocktakingDetail entity,
            string productVariantName = null,
            string productVariantCode = null,
            string productVariantUnitName = null)
        {
            if (entity == null) return null;

            // QUAN TRỌNG: KHÔNG truy cập navigation properties (entity.ProductVariant, entity.StocktakingMaster, etc.)
            // vì DataContext đã bị dispose sau khi repository method kết thúc
            // Chỉ sử dụng tham số truyền vào

            var dto = new StocktakingDetailDto
            {
                // Thông tin cơ bản
                Id = entity.Id,
                StocktakingMasterId = entity.StocktakingMasterId,
                ProductVariantId = entity.ProductVariantId,
                ProductVariantName = productVariantName,
                ProductVariantCode = productVariantCode,
                ProductVariantUnitName = productVariantUnitName,

                // Số lượng
                SystemQuantity = entity.SystemQuantity,
                CountedQuantity = entity.CountedQuantity,
                DifferenceQuantity = entity.DifferenceQuantity,

                // Giá trị
                SystemValue = entity.SystemValue,
                CountedValue = entity.CountedValue,
                DifferenceValue = entity.DifferenceValue,
                UnitPrice = entity.UnitPrice,

                // Điều chỉnh
                AdjustmentType = entity.AdjustmentType.ToAdjustmentTypeEnum(),
                AdjustmentReason = entity.AdjustmentReason,

                // Quy trình phê duyệt
                IsCounted = entity.IsCounted,
                CountedBy = entity.CountedBy,
                CountedDate = entity.CountedDate,
                IsReviewed = entity.IsReviewed,
                ReviewedBy = entity.ReviewedBy,
                ReviewedDate = entity.ReviewedDate,
                ReviewNotes = entity.ReviewNotes,
                IsApproved = entity.IsApproved,
                ApprovedBy = entity.ApprovedBy,
                ApprovedDate = entity.ApprovedDate,

                // Thông tin bổ sung
                Notes = entity.Notes,

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
        /// Chuyển đổi danh sách StocktakingDetail entities thành danh sách StocktakingDetailDto
        /// </summary>
        /// <param name="entities">Danh sách StocktakingDetail entities</param>
        /// <param name="productVariantDict">Dictionary chứa thông tin ProductVariant (key: ProductVariantId, value: (ProductVariantName, ProductVariantCode, ProductVariantUnitName))</param>
        /// <returns>Danh sách StocktakingDetailDto</returns>
        public static List<StocktakingDetailDto> ToDtoList(this IEnumerable<StocktakingDetail> entities,
            Dictionary<Guid, (string ProductVariantName, string ProductVariantCode, string ProductVariantUnitName)> productVariantDict = null)
        {
            if (entities == null) return new List<StocktakingDetailDto>();

            return entities.Select(entity =>
            {
                string productVariantName = null;
                string productVariantCode = null;
                string productVariantUnitName = null;

                if (productVariantDict != null && productVariantDict.TryGetValue(entity.ProductVariantId, out var productInfo))
                {
                    productVariantName = productInfo.ProductVariantName;
                    productVariantCode = productInfo.ProductVariantCode;
                    productVariantUnitName = productInfo.ProductVariantUnitName;
                }

                return entity.ToDto(productVariantName, productVariantCode, productVariantUnitName);
            }).ToList();
        }

        /// <summary>
        /// Chuyển đổi danh sách StocktakingDetail entities thành danh sách StocktakingDetailDto (alias)
        /// </summary>
        /// <param name="entities">Danh sách StocktakingDetail entities</param>
        /// <returns>Danh sách StocktakingDetailDto</returns>
        public static List<StocktakingDetailDto> ToDtos(this IEnumerable<StocktakingDetail> entities)
        {
            if (entities == null) return new List<StocktakingDetailDto>();
            return entities.Select(e => e.ToDto(null, null, null)).ToList();
        }

        #endregion

        #region DTO to Entity

        /// <summary>
        /// Chuyển đổi StocktakingDetailDto thành StocktakingDetail entity
        /// </summary>
        /// <param name="dto">StocktakingDetailDto</param>
        /// <param name="existingEntity">Existing entity to update (optional, for edit mode)</param>
        /// <returns>StocktakingDetail entity</returns>
        public static StocktakingDetail ToEntity(this StocktakingDetailDto dto, StocktakingDetail existingEntity = null)
        {
            if (dto == null) return null;

            StocktakingDetail entity;
            if (existingEntity != null)
            {
                // Update existing entity
                entity = existingEntity;
            }
            else
            {
                // Create new entity
                entity = new StocktakingDetail();
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
            entity.ProductVariantId = dto.ProductVariantId;

            // Map properties - Số lượng
            entity.SystemQuantity = dto.SystemQuantity;
            entity.CountedQuantity = dto.CountedQuantity;
            entity.DifferenceQuantity = dto.DifferenceQuantity;

            // Map properties - Giá trị
            entity.SystemValue = dto.SystemValue;
            entity.CountedValue = dto.CountedValue;
            entity.DifferenceValue = dto.DifferenceValue;
            entity.UnitPrice = dto.UnitPrice;

            // Map properties - Điều chỉnh
            entity.AdjustmentType = dto.AdjustmentType.ToInt();
            entity.AdjustmentReason = dto.AdjustmentReason;

            // Map properties - Quy trình phê duyệt
            entity.IsCounted = dto.IsCounted;
            entity.CountedBy = dto.CountedBy;
            entity.CountedDate = dto.CountedDate;
            entity.IsReviewed = dto.IsReviewed;
            entity.ReviewedBy = dto.ReviewedBy;
            entity.ReviewedDate = dto.ReviewedDate;
            entity.ReviewNotes = dto.ReviewNotes;
            entity.IsApproved = dto.IsApproved;
            entity.ApprovedBy = dto.ApprovedBy;
            entity.ApprovedDate = dto.ApprovedDate;

            // Map properties - Thông tin bổ sung
            entity.Notes = dto.Notes;

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
        /// Chuyển đổi danh sách StocktakingDetailDto thành danh sách StocktakingDetail entities
        /// </summary>
        /// <param name="dtos">Danh sách StocktakingDetailDto</param>
        /// <returns>Danh sách StocktakingDetail entities</returns>
        public static List<StocktakingDetail> ToEntityList(this IEnumerable<StocktakingDetailDto> dtos)
        {
            if (dtos == null) return new List<StocktakingDetail>();
            return dtos.Select(d => d.ToEntity()).Where(e => e != null).ToList();
        }

        /// <summary>
        /// Chuyển đổi danh sách StocktakingDetailDto thành danh sách StocktakingDetail entities (alias)
        /// </summary>
        /// <param name="dtos">Danh sách StocktakingDetailDto</param>
        /// <returns>Danh sách StocktakingDetail entities</returns>
        public static List<StocktakingDetail> ToEntities(this IEnumerable<StocktakingDetailDto> dtos)
        {
            if (dtos == null) return new List<StocktakingDetail>();
            return dtos.Select(d => d.ToEntity()).Where(e => e != null).ToList();
        }

        #endregion
    }
}
