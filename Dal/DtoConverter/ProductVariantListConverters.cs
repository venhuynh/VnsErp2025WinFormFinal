using Dal.DataContext;
using DTO.MasterData.ProductService;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Dal.DtoConverter
{

    /// <summary>
    /// Converter giữa ProductVariant entity và ProductVariantListDto
    /// </summary>
    public static class ProductVariantListConverters
    {
        /// <summary>
        /// Chuyển đổi ProductVariant entity thành ProductVariantListDto
        /// </summary>
        /// <param name="entity">ProductVariant entity</param>
        /// <returns>ProductVariantListDto</returns>
        public static ProductVariantListDto ToListDto(this ProductVariant entity)
        {
            if (entity == null) return null;

            var dto = new ProductVariantListDto
            {
                Id = entity.Id,
                ProductName = entity.ProductService.Name,
                VariantCode = entity.VariantCode ?? string.Empty,
                VariantFullName = entity.VariantFullName ?? string.Empty,
                IsActive = entity.IsActive,
                //ImageCount = entity.ProductImages?.Count ?? 0,
                ThumbnailImage = entity.ThumbnailImage?.ToArray()
            };

            // Lấy UnitName từ entity.VariantFullName nếu có format (UnitName) ở cuối
            // Format có thể là: "Attribute1: Value1, Attribute2: Value2 (UnitName)"
            // Hoặc fallback về navigation property nếu không tìm thấy trong VariantFullName
            if (!string.IsNullOrWhiteSpace(entity.VariantFullName))
            {
                // Tìm pattern (UnitName) ở cuối string
                var match = Regex.Match(
                    entity.VariantFullName,
                    @"\s*\(([^)]+)\)\s*$"
                );

                if (match.Success && match.Groups.Count > 1)
                {
                    dto.UnitName = match.Groups[1].Value.Trim();
                }
            }

            // Fallback: Lấy từ navigation property nếu không tìm thấy trong VariantFullName
            if (string.IsNullOrWhiteSpace(dto.UnitName) && entity.UnitOfMeasure != null)
            {
                dto.UnitName = entity.UnitOfMeasure.Name ?? string.Empty;
            }

            return dto;
        }

        /// <summary>
        /// Chuyển đổi danh sách ProductVariant entities thành danh sách ProductVariantListDto
        /// </summary>
        /// <param name="entities">Danh sách ProductVariant entities</param>
        /// <returns>Danh sách ProductVariantListDto</returns>
        public static List<ProductVariantListDto> ToListDtoList(this IEnumerable<ProductVariant> entities)
        {
            if (entities == null) return new List<ProductVariantListDto>();

            return entities.Select(entity => entity.ToListDto()).ToList();
        }
    }
}
