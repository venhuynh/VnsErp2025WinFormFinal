using System;
using System.Collections.Generic;
using System.Linq;
using Dal.DataContext;
using Dal.DataAccess.MasterData;
using MasterData.Dto;

namespace MasterData.Converters
{
    public static class BusinessPartnerCategoryConverters
    {
        public static BusinessPartnerCategoryDto ToDto(this BusinessPartnerCategory entity)
        {
            if (entity == null) return null;
            return new BusinessPartnerCategoryDto
            {
                Id = entity.Id,
                CategoryName = entity.CategoryName,
                Description = entity.Description,
                ParentId = entity.ParentId,
                CategoryType = entity.ParentId == null ? "Category chính" : "Sub-category",
                PartnerCount = 0, // Sẽ được cập nhật bởi method có đếm
                Level = 0, // Sẽ được tính toán
                HasChildren = false, // Sẽ được cập nhật
                FullPath = entity.CategoryName // Sẽ được cập nhật với đường dẫn đầy đủ
            };
        }

        public static BusinessPartnerCategoryDto ToDtoWithCount(this BusinessPartnerCategory entity, int partnerCount)
        {
            if (entity == null) return null;
            return new BusinessPartnerCategoryDto
            {
                Id = entity.Id,
                CategoryName = entity.CategoryName,
                Description = entity.Description,
                ParentId = entity.ParentId,
                CategoryType = entity.ParentId == null ? "Category chính" : "Sub-category",
                PartnerCount = partnerCount,
                Level = 0, // Sẽ được tính toán
                HasChildren = false, // Sẽ được cập nhật
                FullPath = entity.CategoryName // Sẽ được cập nhật với đường dẫn đầy đủ
            };
        }

        public static IEnumerable<BusinessPartnerCategoryDto> ToDtos(this IEnumerable<BusinessPartnerCategory> entities)
        {
            if (entities == null) return Enumerable.Empty<BusinessPartnerCategoryDto>();
            return entities.Select(e => e.ToDto());
        }

        public static IEnumerable<BusinessPartnerCategoryDto> ToDtosWithCount(this IEnumerable<BusinessPartnerCategory> entities, 
            Func<Guid, int> partnerCountResolver)
        {
            if (entities == null) return Enumerable.Empty<BusinessPartnerCategoryDto>();
            return entities.Select(e => e.ToDtoWithCount(partnerCountResolver?.Invoke(e.Id) ?? 0));
        }

        /// <summary>
        /// Chuyển đổi BusinessPartnerCategory sang DTO với đếm số lượng đối tác từ mapping table.
        /// </summary>
        /// <param name="entity">Entity BusinessPartnerCategory</param>
        /// <param name="mappingCounts">Dictionary chứa số lượng đối tác theo CategoryId</param>
        /// <returns>BusinessPartnerCategoryDto với PartnerCount</returns>
        public static BusinessPartnerCategoryDto ToDtoWithMappingCount(this BusinessPartnerCategory entity, 
            Dictionary<Guid, int> mappingCounts)
        {
            if (entity == null) return null;
            var partnerCount = mappingCounts?.ContainsKey(entity.Id) == true ? mappingCounts[entity.Id] : 0;
            
            // Debug logging
            System.Diagnostics.Debug.WriteLine($"ToDtoWithMappingCount - Category: {entity.CategoryName}, ID: {entity.Id}, PartnerCount: {partnerCount}");
            
            return entity.ToDtoWithCount(partnerCount);
        }

        /// <summary>
        /// Chuyển đổi danh sách BusinessPartnerCategory sang DTO với đếm số lượng đối tác.
        /// </summary>
        /// <param name="entities">Danh sách entities</param>
        /// <param name="mappingCounts">Dictionary chứa số lượng đối tác theo CategoryId</param>
        /// <returns>Danh sách BusinessPartnerCategoryDto với PartnerCount</returns>
        public static IEnumerable<BusinessPartnerCategoryDto> ToDtosWithMappingCount(this IEnumerable<BusinessPartnerCategory> entities, 
            Dictionary<Guid, int> mappingCounts)
        {
            if (entities == null) return Enumerable.Empty<BusinessPartnerCategoryDto>();
            return entities.Select(e => e.ToDtoWithMappingCount(mappingCounts));
        }

        public static BusinessPartnerCategory ToEntity(this BusinessPartnerCategoryDto dto, BusinessPartnerCategory destination = null)
        {
            if (dto == null) return null;
            var entity = destination ?? new BusinessPartnerCategory();

            entity.Id = dto.Id == Guid.Empty ? entity.Id : dto.Id;
            entity.CategoryName = dto.CategoryName;
            entity.Description = dto.Description;
            entity.ParentId = dto.ParentId;

            return entity;
        }

        /// <summary>
        /// Chuyển đổi danh sách categories thành hierarchical DTOs với thông tin đầy đủ
        /// </summary>
        /// <param name="entities">Danh sách entities</param>
        /// <param name="mappingCounts">Dictionary chứa số lượng đối tác theo CategoryId</param>
        /// <returns>Danh sách DTOs với thông tin hierarchical, sắp xếp theo cấu trúc cây</returns>
        public static IEnumerable<BusinessPartnerCategoryDto> ToDtosWithHierarchy(this IEnumerable<BusinessPartnerCategory> entities, 
            Dictionary<Guid, int> mappingCounts)
        {
            if (entities == null) return Enumerable.Empty<BusinessPartnerCategoryDto>();
            
            var entityList = entities.ToList();
            var entityDict = entityList.ToDictionary(e => e.Id);
            
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
            
            // Sắp xếp theo cấu trúc cây: parent trước, children sau
            return SortHierarchical(dtoList);
        }


        /// <summary>
        /// Sắp xếp danh sách DTO theo cấu trúc cây
        /// </summary>
        private static IEnumerable<BusinessPartnerCategoryDto> SortHierarchical(List<BusinessPartnerCategoryDto> dtoList)
        {
            var result = new List<BusinessPartnerCategoryDto>();
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
        private static void AddChildrenRecursive(BusinessPartnerCategoryDto parent, 
            Dictionary<Guid, BusinessPartnerCategoryDto> dtoDict, List<BusinessPartnerCategoryDto> result)
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
        private static int CalculateLevel(BusinessPartnerCategory entity, Dictionary<Guid, BusinessPartnerCategory> entityDict)
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
        private static string CalculateFullPath(BusinessPartnerCategory entity, Dictionary<Guid, BusinessPartnerCategory> entityDict)
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
    }
}


