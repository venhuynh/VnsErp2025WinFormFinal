using System;
using System.Collections.Generic;
using System.Linq;
using Dal.DataContext;
using MasterData.ProductService.Dto;

namespace MasterData.ProductService.Converters
{
    public static class ProductServiceCategoryConverters
    {
        public static ProductServiceCategoryDto ToDto(this ProductServiceCategory entity)
        {
            if (entity == null) return null;
            return new ProductServiceCategoryDto
            {
                Id = entity.Id,
                CategoryCode = entity.CategoryCode,
                CategoryName = entity.CategoryName,
                Description = entity.Description,
                ParentId = entity.ParentId,
                ParentCategoryName = null, // Sẽ được cập nhật bởi method có đầy đủ thông tin
                Level = 0, // Sẽ được tính toán
                HasChildren = false, // Sẽ được cập nhật
                FullPath = entity.CategoryName, // Sẽ được cập nhật với đường dẫn đầy đủ
                ProductCount = 0 // Sẽ được cập nhật bởi method có đếm
            };
        }

        public static ProductServiceCategoryDto ToDtoWithCount(this ProductServiceCategory entity, int productCount)
        {
            if (entity == null) return null;
            return new ProductServiceCategoryDto
            {
                Id = entity.Id,
                CategoryCode = entity.CategoryCode,
                CategoryName = entity.CategoryName,
                Description = entity.Description,
                ParentId = entity.ParentId,
                ParentCategoryName = null, // Sẽ được cập nhật bởi method có đầy đủ thông tin
                Level = 0, // Sẽ được tính toán
                HasChildren = false, // Sẽ được cập nhật
                FullPath = entity.CategoryName, // Sẽ được cập nhật với đường dẫn đầy đủ
                ProductCount = productCount
            };
        }

        public static IEnumerable<ProductServiceCategoryDto> ToDtos(this IEnumerable<ProductServiceCategory> entities)
        {
            if (entities == null) return Enumerable.Empty<ProductServiceCategoryDto>();
            return entities.Select(e => e.ToDto());
        }

        public static IEnumerable<ProductServiceCategoryDto> ToDtosWithCount(this IEnumerable<ProductServiceCategory> entities, 
            Func<Guid, int> productCountResolver)
        {
            if (entities == null) return Enumerable.Empty<ProductServiceCategoryDto>();
            return entities.Select(e => e.ToDtoWithCount(productCountResolver?.Invoke(e.Id) ?? 0));
        }

        /// <summary>
        /// Chuyển đổi ProductServiceCategory sang DTO với đếm số lượng sản phẩm/dịch vụ từ mapping table.
        /// </summary>
        /// <param name="entity">Entity ProductServiceCategory</param>
        /// <param name="mappingCounts">Dictionary chứa số lượng sản phẩm/dịch vụ theo CategoryId</param>
        /// <returns>ProductServiceCategoryDto với ProductCount</returns>
        public static ProductServiceCategoryDto ToDtoWithMappingCount(this ProductServiceCategory entity, 
            Dictionary<Guid, int> mappingCounts)
        {
            if (entity == null) return null;
            var productCount = mappingCounts?.ContainsKey(entity.Id) == true ? mappingCounts[entity.Id] : 0;
            
            // Debug logging
            System.Diagnostics.Debug.WriteLine($"ToDtoWithMappingCount - Category: {entity.CategoryName}, ID: {entity.Id}, ProductCount: {productCount}");
            
            return entity.ToDtoWithCount(productCount);
        }

        /// <summary>
        /// Chuyển đổi danh sách ProductServiceCategory sang DTO với đếm số lượng sản phẩm/dịch vụ.
        /// </summary>
        /// <param name="entities">Danh sách entities</param>
        /// <param name="mappingCounts">Dictionary chứa số lượng sản phẩm/dịch vụ theo CategoryId</param>
        /// <returns>Danh sách ProductServiceCategoryDto với ProductCount</returns>
        public static IEnumerable<ProductServiceCategoryDto> ToDtosWithMappingCount(this IEnumerable<ProductServiceCategory> entities, 
            Dictionary<Guid, int> mappingCounts)
        {
            if (entities == null) return Enumerable.Empty<ProductServiceCategoryDto>();
            return entities.Select(e => e.ToDtoWithMappingCount(mappingCounts));
        }

        public static ProductServiceCategory ToEntity(this ProductServiceCategoryDto dto, ProductServiceCategory destination = null)
        {
            if (dto == null) return null;
            var entity = destination ?? new ProductServiceCategory();

            // Set ID: tạo mới nếu Guid.Empty (thêm mới), dùng ID hiện tại nếu edit
            entity.Id = dto.Id == Guid.Empty ? Guid.NewGuid() : dto.Id;
            entity.CategoryCode = dto.CategoryCode;
            entity.CategoryName = dto.CategoryName;
            entity.Description = dto.Description;
            entity.ParentId = dto.ParentId;

            return entity;
        }

        /// <summary>
        /// Chuyển đổi danh sách categories thành hierarchical DTOs với thông tin đầy đủ
        /// </summary>
        /// <param name="entities">Danh sách entities</param>
        /// <param name="mappingCounts">Dictionary chứa số lượng sản phẩm/dịch vụ theo CategoryId</param>
        /// <returns>Danh sách DTOs với thông tin hierarchical, sắp xếp theo cấu trúc cây</returns>
        public static IEnumerable<ProductServiceCategoryDto> ToDtosWithHierarchy(this IEnumerable<ProductServiceCategory> entities, 
            Dictionary<Guid, int> mappingCounts)
        {
            if (entities == null) return Enumerable.Empty<ProductServiceCategoryDto>();
            
            var entityList = entities.ToList();
            var entityDict = entityList.ToDictionary(e => e.Id);
            
            // Tạo DTOs với số lượng ban đầu (chỉ sản phẩm/dịch vụ trực tiếp)
            var dtoList = entityList.Select(entity => 
            {
                var dto = entity.ToDtoWithMappingCount(mappingCounts);
                
                // Tính toán Level
                dto.Level = CalculateLevel(entity, entityDict);
                
                // Tính toán HasChildren (chỉ sub-categories)
                var hasSubCategories = entityList.Any(e => e.ParentId == entity.Id);
                dto.HasChildren = hasSubCategories;
                
                // Tính toán FullPath
                dto.FullPath = CalculateFullPath(entity, entityDict);
                
                // Lấy tên parent category
                if (entity.ParentId.HasValue && entityDict.ContainsKey(entity.ParentId.Value))
                {
                    dto.ParentCategoryName = entityDict[entity.ParentId.Value].CategoryName;
                }
                
                return dto;
            }).ToList();
            
            // Cộng tổng số lượng từ các danh mục con
            CalculateTotalProductCounts(dtoList, entityDict);
            
            // Sắp xếp theo cấu trúc cây: parent trước, children sau
            return SortHierarchical(dtoList);
        }

        /// <summary>
        /// Sắp xếp danh sách DTO theo cấu trúc cây
        /// </summary>
        private static IEnumerable<ProductServiceCategoryDto> SortHierarchical(List<ProductServiceCategoryDto> dtoList)
        {
            var result = new List<ProductServiceCategoryDto>();
            var dtoDict = dtoList.ToDictionary(d => d.Id);
            
            // Thêm các root categories trước (ParentId = null)
            var rootCategories = dtoList.Where(d => !d.ParentId.HasValue).OrderBy(d => d.CategoryName);
            
            foreach (var root in rootCategories)
            {
                result.Add(root);
                AddChildrenRecursive(root, dtoDict, result);
            }
            
            return result;
        }

        /// <summary>
        /// Thêm children một cách đệ quy
        /// </summary>
        private static void AddChildrenRecursive(ProductServiceCategoryDto parent, 
            Dictionary<Guid, ProductServiceCategoryDto> dtoDict, List<ProductServiceCategoryDto> result)
        {
            var children = dtoDict.Values
                .Where(d => d.ParentId == parent.Id)
                .OrderBy(d => d.CategoryName);
                
            foreach (var child in children)
            {
                result.Add(child);
                AddChildrenRecursive(child, dtoDict, result);
            }
        }

        /// <summary>
        /// Tính toán cấp độ của category trong cây phân cấp
        /// </summary>
        private static int CalculateLevel(ProductServiceCategory entity, Dictionary<Guid, ProductServiceCategory> entityDict)
        {
            int level = 0;
            var current = entity;
            while (current.ParentId.HasValue && entityDict.ContainsKey(current.ParentId.Value))
            {
                level++;
                current = entityDict[current.ParentId.Value];
                if (level > 10) break; // Tránh infinite loop
            }
            return level;
        }

        /// <summary>
        /// Tính toán đường dẫn đầy đủ từ gốc đến category
        /// </summary>
        private static string CalculateFullPath(ProductServiceCategory entity, Dictionary<Guid, ProductServiceCategory> entityDict)
        {
            var pathParts = new List<string> { entity.CategoryName };
            var current = entity;
            
            while (current.ParentId.HasValue && entityDict.ContainsKey(current.ParentId.Value))
            {
                current = entityDict[current.ParentId.Value];
                pathParts.Insert(0, current.CategoryName);
                if (pathParts.Count > 10) break; // Tránh infinite loop
            }
            
            return string.Join(" > ", pathParts);
        }

        /// <summary>
        /// Tính toán tổng số lượng sản phẩm/dịch vụ bao gồm cả các danh mục con
        /// </summary>
        /// <param name="dtoList">Danh sách DTOs</param>
        /// <param name="entityDict">Dictionary entities để xác định quan hệ parent-child</param>
        private static void CalculateTotalProductCounts(List<ProductServiceCategoryDto> dtoList, Dictionary<Guid, ProductServiceCategory> entityDict)
        {
            var dtoDict = dtoList.ToDictionary(d => d.Id);
            
            // Lưu trữ số lượng trực tiếp để tránh bị ghi đè
            var directCounts = dtoList.ToDictionary(d => d.Id, d => d.ProductCount);
            
            // Tính tổng số lượng cho mỗi danh mục (bao gồm cả con)
            foreach (var dto in dtoList)
            {
                var totalCount = CalculateTotalCountForCategory(dto.Id, dtoDict, entityDict, directCounts);
                dto.ProductCount = totalCount;
                
                // Debug logging
                System.Diagnostics.Debug.WriteLine($"CalculateTotalProductCounts - Category: {dto.CategoryName}, Direct: {directCounts[dto.Id]}, Total: {totalCount}");
            }
        }

        /// <summary>
        /// Tính tổng số lượng sản phẩm/dịch vụ cho một danh mục và tất cả các danh mục con
        /// </summary>
        /// <param name="categoryId">ID của danh mục</param>
        /// <param name="dtoDict">Dictionary DTOs</param>
        /// <param name="entityDict">Dictionary entities</param>
        /// <param name="directCounts">Dictionary chứa số lượng trực tiếp của mỗi danh mục</param>
        /// <returns>Tổng số lượng sản phẩm/dịch vụ</returns>
        private static int CalculateTotalCountForCategory(Guid categoryId, Dictionary<Guid, ProductServiceCategoryDto> dtoDict, 
            Dictionary<Guid, ProductServiceCategory> entityDict, Dictionary<Guid, int> directCounts)
        {
            if (!dtoDict.ContainsKey(categoryId))
                return 0;

            var directCount = directCounts.ContainsKey(categoryId) ? directCounts[categoryId] : 0;
            
            // Tìm tất cả danh mục con
            var childCategories = entityDict.Values.Where(e => e.ParentId == categoryId);
            
            var childCount = 0;
            foreach (var child in childCategories)
            {
                childCount += CalculateTotalCountForCategory(child.Id, dtoDict, entityDict, directCounts);
            }
            
            var totalCount = directCount + childCount;
            
            // Debug logging
            var dto = dtoDict[categoryId];
            System.Diagnostics.Debug.WriteLine($"  CalculateTotalCountForCategory - {dto.CategoryName}: Direct={directCount}, Children={childCount}, Total={totalCount}");
            
            return totalCount;
        }
    }
}
