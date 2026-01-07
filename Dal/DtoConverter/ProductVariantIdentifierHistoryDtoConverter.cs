using DTO.Inventory.InventoryManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using ProductVariantIdentifierHistory = Dal.DataContext.ProductVariantIdentifierHistory;

namespace Dal.DtoConverter
{
    /// <summary>
    /// Converter giữa ProductVariantIdentifierHistory entity và ProductVariantIdentifierHistoryDto
    /// </summary>
    public static class ProductVariantIdentifierHistoryDtoConverter
    {
        #region Entity to DTO

        /// <summary>
        /// Chuyển đổi ProductVariantIdentifierHistory entity thành ProductVariantIdentifierHistoryDto
        /// </summary>
        /// <param name="entity">ProductVariantIdentifierHistory entity</param>
        /// <param name="productVariantFullName">Tên biến thể sản phẩm đầy đủ (tùy chọn, để tránh DataContext disposed errors)</param>
        /// <returns>ProductVariantIdentifierHistoryDto</returns>
        public static ProductVariantIdentifierHistoryDto ToDto(this ProductVariantIdentifierHistory entity,
            string productVariantFullName = null)
        {
            if (entity == null) return null;

            // QUAN TRỌNG: KHÔNG truy cập navigation properties (entity.ProductVariant, entity.ProductVariantIdentifier)
            // vì DataContext đã bị dispose sau khi repository method kết thúc
            // Chỉ sử dụng tham số truyền vào

            var dto = new ProductVariantIdentifierHistoryDto
            {
                // Thông tin cơ bản
                Id = entity.Id,
                ProductVariantIdentifierId = entity.ProductVariantIdentifierId,
                ProductVariantId = entity.ProductVariantId,
                ProductVariantFullName = productVariantFullName,

                // Thông tin thay đổi
                ChangeType = entity.ChangeType,
                ChangeDate = entity.ChangeDate,
                ChangedBy = entity.ChangedBy,
                Value = entity.Value,
                Notes = entity.Notes
            };

            return dto;
        }

        /// <summary>
        /// Chuyển đổi danh sách ProductVariantIdentifierHistory entities thành danh sách ProductVariantIdentifierHistoryDto
        /// </summary>
        /// <param name="entities">Danh sách ProductVariantIdentifierHistory entities</param>
        /// <param name="productVariantDict">Dictionary chứa thông tin ProductVariant (key: ProductVariantId, value: ProductVariantFullName)</param>
        /// <returns>Danh sách ProductVariantIdentifierHistoryDto</returns>
        public static List<ProductVariantIdentifierHistoryDto> ToDtoList(this IEnumerable<ProductVariantIdentifierHistory> entities,
            Dictionary<Guid, string> productVariantDict = null)
        {
            if (entities == null) return new List<ProductVariantIdentifierHistoryDto>();

            return entities.Select(entity =>
            {
                string productVariantFullName = null;

                if (productVariantDict != null && productVariantDict.TryGetValue(entity.ProductVariantId, out var fullName))
                {
                    productVariantFullName = fullName;
                }

                return entity.ToDto(productVariantFullName);
            }).ToList();
        }

        /// <summary>
        /// Chuyển đổi danh sách ProductVariantIdentifierHistory entities thành danh sách ProductVariantIdentifierHistoryDto (alias)
        /// </summary>
        /// <param name="entities">Danh sách ProductVariantIdentifierHistory entities</param>
        /// <returns>Danh sách ProductVariantIdentifierHistoryDto</returns>
        public static List<ProductVariantIdentifierHistoryDto> ToDtos(this IEnumerable<ProductVariantIdentifierHistory> entities)
        {
            if (entities == null) return new List<ProductVariantIdentifierHistoryDto>();
            return entities.Select(e => e.ToDto(null)).ToList();
        }

        #endregion

        #region DTO to Entity

        /// <summary>
        /// Chuyển đổi ProductVariantIdentifierHistoryDto thành ProductVariantIdentifierHistory entity
        /// </summary>
        /// <param name="dto">ProductVariantIdentifierHistoryDto</param>
        /// <param name="existingEntity">Existing entity to update (optional, for edit mode)</param>
        /// <returns>ProductVariantIdentifierHistory entity</returns>
        public static ProductVariantIdentifierHistory ToEntity(this ProductVariantIdentifierHistoryDto dto, ProductVariantIdentifierHistory existingEntity = null)
        {
            if (dto == null) return null;

            ProductVariantIdentifierHistory entity;
            if (existingEntity != null)
            {
                // Update existing entity
                entity = existingEntity;
            }
            else
            {
                // Create new entity
                entity = new ProductVariantIdentifierHistory();
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
            entity.ProductVariantIdentifierId = dto.ProductVariantIdentifierId;
            entity.ProductVariantId = dto.ProductVariantId;

            // Map properties - Thông tin thay đổi
            entity.ChangeType = dto.ChangeType;
            entity.ChangeDate = dto.ChangeDate;
            entity.ChangedBy = dto.ChangedBy;
            entity.Value = dto.Value;
            entity.Notes = dto.Notes;

            return entity;
        }

        /// <summary>
        /// Chuyển đổi danh sách ProductVariantIdentifierHistoryDto thành danh sách ProductVariantIdentifierHistory entities
        /// </summary>
        /// <param name="dtos">Danh sách ProductVariantIdentifierHistoryDto</param>
        /// <returns>Danh sách ProductVariantIdentifierHistory entities</returns>
        public static List<ProductVariantIdentifierHistory> ToEntityList(this IEnumerable<ProductVariantIdentifierHistoryDto> dtos)
        {
            if (dtos == null) return new List<ProductVariantIdentifierHistory>();
            return dtos.Select(d => d.ToEntity()).Where(e => e != null).ToList();
        }

        /// <summary>
        /// Chuyển đổi danh sách ProductVariantIdentifierHistoryDto thành danh sách ProductVariantIdentifierHistory entities (alias)
        /// </summary>
        /// <param name="dtos">Danh sách ProductVariantIdentifierHistoryDto</param>
        /// <returns>Danh sách ProductVariantIdentifierHistory entities</returns>
        public static List<ProductVariantIdentifierHistory> ToEntities(this IEnumerable<ProductVariantIdentifierHistoryDto> dtos)
        {
            if (dtos == null) return new List<ProductVariantIdentifierHistory>();
            return dtos.Select(d => d.ToEntity()).Where(e => e != null).ToList();
        }

        #endregion
    }
}
