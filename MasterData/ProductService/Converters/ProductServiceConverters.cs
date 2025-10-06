using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using MasterData.ProductService.Dto;

namespace MasterData.ProductService.Converters
{
    /// <summary>
    /// Converter để chuyển đổi giữa ProductService Entity và ProductServiceDto
    /// </summary>
    public static class ProductServiceConverters
    {
        /// <summary>
        /// Chuyển đổi ProductService entity sang ProductServiceDto
        /// </summary>
        /// <param name="entity">ProductService entity</param>
        /// <returns>ProductServiceDto</returns>
        public static ProductServiceDto ToDto(this Dal.DataContext.ProductService entity)
        {
            if (entity == null) return null;

            return new ProductServiceDto
            {
                Id = entity.Id,
                Code = entity.Code,
                Name = entity.Name,
                CategoryId = entity.CategoryId,
                CategoryName = null, // Sẽ được resolve riêng nếu cần
                IsService = entity.IsService,
                Description = entity.Description,
                IsActive = entity.IsActive,
                ThumbnailImage = entity.ThumbnailImage?.ToArray(),
                VariantCount = 0, // Sẽ được tính toán riêng nếu cần
                ImageCount = 0,   // Sẽ được tính toán riêng nếu cần
            };
        }

        /// <summary>
        /// Chuyển đổi ProductService entity sang ProductServiceDto với resolver cho tên danh mục
        /// </summary>
        /// <param name="entity">ProductService entity</param>
        /// <param name="categoryNameResolver">Hàm resolve tên danh mục từ CategoryId</param>
        /// <param name="variantCountResolver">Hàm resolve số lượng biến thể từ ProductServiceId</param>
        /// <param name="imageCountResolver">Hàm resolve số lượng hình ảnh từ ProductServiceId</param>
        /// <returns>ProductServiceDto</returns>
        public static ProductServiceDto ToDto(this Dal.DataContext.ProductService entity, 
            Func<Guid?, string> categoryNameResolver,
            Func<Guid, int> variantCountResolver = null,
            Func<Guid, int> imageCountResolver = null)
        {
            if (entity == null) return null;

            return new ProductServiceDto
            {
                Id = entity.Id,
                Code = entity.Code,
                Name = entity.Name,
                CategoryId = entity.CategoryId,
                CategoryName = categoryNameResolver?.Invoke(entity.CategoryId),
                IsService = entity.IsService,
                Description = entity.Description,
                IsActive = entity.IsActive,
                ThumbnailImage = entity.ThumbnailImage?.ToArray(),
                VariantCount = variantCountResolver?.Invoke(entity.Id) ?? 0,
                ImageCount = imageCountResolver?.Invoke(entity.Id) ?? 0,
            };
        }

        /// <summary>
        /// Chuyển đổi danh sách ProductService entities sang danh sách ProductServiceDto
        /// </summary>
        /// <param name="entities">Danh sách ProductService entities</param>
        /// <param name="categoryNameResolver">Hàm resolve tên danh mục từ CategoryId</param>
        /// <param name="variantCountResolver">Hàm resolve số lượng biến thể từ ProductServiceId</param>
        /// <param name="imageCountResolver">Hàm resolve số lượng hình ảnh từ ProductServiceId</param>
        /// <returns>Danh sách ProductServiceDto</returns>
        public static List<ProductServiceDto> ToDtoList(this IEnumerable<Dal.DataContext.ProductService> entities, 
            Func<Guid?, string> categoryNameResolver = null,
            Func<Guid, int> variantCountResolver = null,
            Func<Guid, int> imageCountResolver = null)
        {
            if (entities == null) return new List<ProductServiceDto>();

            return entities.Select(x => x.ToDto(categoryNameResolver, variantCountResolver, imageCountResolver)).ToList();
        }

        /// <summary>
        /// Chuyển đổi ProductServiceDto sang ProductService entity
        /// </summary>
        /// <param name="dto">ProductServiceDto</param>
        /// <returns>ProductService entity</returns>
        public static Dal.DataContext.ProductService ToEntity(this ProductServiceDto dto)
        {
            if (dto == null) return null;

            return new Dal.DataContext.ProductService
            {
                Id = dto.Id,
                Code = dto.Code,
                Name = dto.Name,
                CategoryId = dto.CategoryId,
                IsService = dto.IsService,
                Description = dto.Description,
                IsActive = dto.IsActive,
                ThumbnailImage = dto.ThumbnailImage != null ? new Binary(dto.ThumbnailImage) : null,
            };
        }
    }
}