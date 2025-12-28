using DTO.MasterData.ProductService;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;

namespace Dal.DtoConverter
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
        /// <param name="categoryFullPathResolver">Hàm resolve đường dẫn đầy đủ của danh mục từ CategoryId</param>
        /// <returns>ProductServiceDto</returns>
        public static ProductServiceDto ToDto(this Dal.DataContext.ProductService entity,
            Func<Guid?, string> categoryNameResolver,
            Func<Guid, int> variantCountResolver = null,
            Func<Guid, int> imageCountResolver = null,
            Func<Guid?, string> categoryFullPathResolver = null)
        {
            if (entity == null) return null;

            return new ProductServiceDto
            {
                Id = entity.Id,
                Code = entity.Code,
                Name = entity.Name,
                CategoryId = entity.CategoryId,
                CategoryName = categoryNameResolver?.Invoke(entity.CategoryId),
                CategoryFullPath = categoryFullPathResolver?.Invoke(entity.CategoryId),
                IsService = entity.IsService,
                Description = entity.Description,
                IsActive = entity.IsActive,
                ThumbnailImage = entity.ThumbnailImage?.ToArray(),
                VariantCount = variantCountResolver?.Invoke(entity.Id) ?? 0,
                ImageCount = imageCountResolver?.Invoke(entity.Id) ?? 0,
            };
        }

        /// <summary>
        /// Chuyển đổi ProductService entity sang ProductServiceDto với category dictionary để tối ưu hiệu suất
        /// </summary>
        /// <param name="entity">ProductService entity</param>
        /// <param name="categoryDict">Dictionary chứa tất cả ProductServiceCategory entities (key = CategoryId)</param>
        /// <param name="variantCountResolver">Hàm resolve số lượng biến thể từ ProductServiceId</param>
        /// <param name="imageCountResolver">Hàm resolve số lượng hình ảnh từ ProductServiceId</param>
        /// <returns>ProductServiceDto</returns>
        public static ProductServiceDto ToDto(this Dal.DataContext.ProductService entity,
            Dictionary<Guid, Dal.DataContext.ProductServiceCategory> categoryDict = null,
            Func<Guid, int> variantCountResolver = null,
            Func<Guid, int> imageCountResolver = null)
        {
            if (entity == null) return null;

            string categoryName = null;
            string categoryFullPath = null;

            if (entity.CategoryId.HasValue && categoryDict != null)
            {
                if (categoryDict.TryGetValue(entity.CategoryId.Value, out var category))
                {
                    categoryName = category.CategoryName;
                    categoryFullPath = CalculateCategoryFullPath(category, categoryDict);
                }
            }

            return new ProductServiceDto
            {
                Id = entity.Id,
                Code = entity.Code,
                Name = entity.Name,
                CategoryId = entity.CategoryId,
                CategoryName = categoryName,
                CategoryFullPath = categoryFullPath,
                IsService = entity.IsService,
                Description = entity.Description,
                IsActive = entity.IsActive,
                ThumbnailImage = entity.ThumbnailImage?.ToArray(),
                VariantCount = variantCountResolver?.Invoke(entity.Id) ?? 0,
                ImageCount = imageCountResolver?.Invoke(entity.Id) ?? 0,
            };
        }

        /// <summary>
        /// Tính toán đường dẫn đầy đủ của category từ dictionary
        /// </summary>
        private static string CalculateCategoryFullPath(Dal.DataContext.ProductServiceCategory category,
            Dictionary<Guid, Dal.DataContext.ProductServiceCategory> categoryDict)
        {
            if (category == null || categoryDict == null) return null;

            try
            {
                var pathParts = new List<string> { category.CategoryName };
                var current = category;

                // Đi ngược lên parent categories
                while (current.ParentId.HasValue)
                {
                    if (!categoryDict.TryGetValue(current.ParentId.Value, out current))
                        break;
                    pathParts.Insert(0, current.CategoryName);

                    // Tránh infinite loop
                    if (pathParts.Count > 10)
                        break;
                }

                return string.Join(" > ", pathParts);
            }
            catch
            {
                return category.CategoryName;
            }
        }

        /// <summary>
        /// Chuyển đổi danh sách ProductService entities sang danh sách ProductServiceDto
        /// </summary>
        /// <param name="entities">Danh sách ProductService entities</param>
        /// <param name="categoryNameResolver">Hàm resolve tên danh mục từ CategoryId</param>
        /// <param name="variantCountResolver">Hàm resolve số lượng biến thể từ ProductServiceId</param>
        /// <param name="imageCountResolver">Hàm resolve số lượng hình ảnh từ ProductServiceId</param>
        /// <param name="categoryFullPathResolver">Hàm resolve đường dẫn đầy đủ của danh mục từ CategoryId</param>
        /// <returns>Danh sách ProductServiceDto</returns>
        public static List<ProductServiceDto> ToDtoList(this IEnumerable<Dal.DataContext.ProductService> entities,
            Func<Guid?, string> categoryNameResolver = null,
            Func<Guid, int> variantCountResolver = null,
            Func<Guid, int> imageCountResolver = null,
            Func<Guid?, string> categoryFullPathResolver = null)
        {
            if (entities == null) return new List<ProductServiceDto>();

            return entities.Select(x => x.ToDto(categoryNameResolver, variantCountResolver, imageCountResolver, categoryFullPathResolver)).ToList();
        }

        /// <summary>
        /// Chuyển đổi danh sách ProductService entities sang danh sách ProductServiceDto với category dictionary để tối ưu hiệu suất
        /// </summary>
        /// <param name="entities">Danh sách ProductService entities</param>
        /// <param name="categoryDict">Dictionary chứa tất cả ProductServiceCategory entities (key = CategoryId)</param>
        /// <param name="variantCountResolver">Hàm resolve số lượng biến thể từ ProductServiceId</param>
        /// <param name="imageCountResolver">Hàm resolve số lượng hình ảnh từ ProductServiceId</param>
        /// <returns>Danh sách ProductServiceDto</returns>
        public static List<ProductServiceDto> ToDtoList(this IEnumerable<Dal.DataContext.ProductService> entities,
            Dictionary<Guid, Dal.DataContext.ProductServiceCategory> categoryDict = null,
            Func<Guid, int> variantCountResolver = null,
            Func<Guid, int> imageCountResolver = null)
        {
            if (entities == null) return new List<ProductServiceDto>();

            return entities.Select(x => x.ToDto(categoryDict, variantCountResolver, imageCountResolver)).ToList();
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
