using System;
using System.Collections.Generic;
using System.Linq;
using Dal.DataContext;
using DTO.MasterData.ProductService;

namespace MasterData.ProductService
{
    /// <summary>
    /// Extension methods cho ProductServiceCategory entities và DTOs.
    /// Cung cấp conversion, transformation, và utility methods.
    /// </summary>
    public static class ProductServiceCategoryExtensions
    {
        #region ========== CONVERSION METHODS ==========

        /// <summary>
        /// Convert entity to DTO with product count.
        /// </summary>
        /// <param name="entity">ProductServiceCategory entity</param>
        /// <param name="productCount">Số lượng sản phẩm/dịch vụ (mặc định 0)</param>
        /// <returns>ProductServiceCategoryDto</returns>
        public static ProductServiceCategoryDto ToDtoWithCount(
            this ProductServiceCategory entity, 
            int productCount = 0)
        {
            if (entity == null) return null;

            return new ProductServiceCategoryDto
            {
                Id = entity.Id,
                CategoryCode = entity.CategoryCode,
                CategoryName = entity.CategoryName,
                Description = entity.Description,
                ParentId = entity.ParentId,
                ProductCount = productCount,
                IsActive = entity.IsActive,
                SortOrder = entity.SortOrder,
                CreatedDate = entity.CreatedDate,
                CreatedBy = entity.CreatedBy,
                ModifiedDate = entity.ModifiedDate,
                ModifiedBy = entity.ModifiedBy
            };
        }

        /// <summary>
        /// Convert entity to DTO (without product count).
        /// </summary>
        /// <param name="entity">ProductServiceCategory entity</param>
        /// <returns>ProductServiceCategoryDto</returns>
        public static ProductServiceCategoryDto ToDto(this ProductServiceCategory entity)
        {
            return entity.ToDtoWithCount(0);
        }

        #endregion

        #region ========== BATCH CONVERSION METHODS ==========

        /// <summary>
        /// Convert entities collection to DTOs with hierarchical structure.
        /// </summary>
        /// <param name="entities">Collection of ProductServiceCategory entities</param>
        /// <param name="counts">Dictionary of product counts by category ID</param>
        /// <returns>IEnumerable of ProductServiceCategoryDto</returns>
        public static IEnumerable<ProductServiceCategoryDto> ToDtosWithHierarchy(
            this IEnumerable<ProductServiceCategory> entities,
            Dictionary<Guid, int> counts = null)
        {
            if (entities == null) return Enumerable.Empty<ProductServiceCategoryDto>();

            counts = counts ?? new Dictionary<Guid, int>();

            return entities
                .Select(e => e.ToDtoWithCount(
                    counts.ContainsKey(e.Id) ? counts[e.Id] : 0))
                .OrderBy(d => d.SortOrder ?? int.MaxValue)
                .ThenBy(d => d.CategoryName);
        }

        /// <summary>
        /// Convert entities collection to DTOs (without product count).
        /// </summary>
        /// <param name="entities">Collection of ProductServiceCategory entities</param>
        /// <returns>IEnumerable of ProductServiceCategoryDto</returns>
        public static IEnumerable<ProductServiceCategoryDto> ToDtos(
            this IEnumerable<ProductServiceCategory> entities)
        {
            return entities.ToDtosWithHierarchy();
        }

        #endregion

        #region ========== FILTERING EXTENSIONS ==========

        /// <summary>
        /// Filter categories to only active ones (IsActive = true).
        /// </summary>
        /// <param name="categories">Source categories</param>
        /// <returns>Filtered active categories</returns>
        public static IEnumerable<ProductServiceCategoryDto> WhereActive(
            this IEnumerable<ProductServiceCategoryDto> categories)
        {
            return categories?.Where(c => c != null && c.IsActive) ?? Enumerable.Empty<ProductServiceCategoryDto>();
        }

        /// <summary>
        /// Filter categories to only root categories (ParentId = null).
        /// </summary>
        /// <param name="categories">Source categories</param>
        /// <returns>Root categories</returns>
        public static IEnumerable<ProductServiceCategoryDto> WhereRoot(
            this IEnumerable<ProductServiceCategoryDto> categories)
        {
            return categories?.Where(c => c != null && c.ParentId == null) ?? Enumerable.Empty<ProductServiceCategoryDto>();
        }

        /// <summary>
        /// Filter categories by parent ID.
        /// </summary>
        /// <param name="categories">Source categories</param>
        /// <param name="parentId">Parent category ID (null for root categories)</param>
        /// <returns>Children of specified parent</returns>
        public static IEnumerable<ProductServiceCategoryDto> WhereParent(
            this IEnumerable<ProductServiceCategoryDto> categories,
            Guid? parentId)
        {
            return categories?.Where(c => c != null && c.ParentId == parentId) ?? Enumerable.Empty<ProductServiceCategoryDto>();
        }

        /// <summary>
        /// Filter categories to only those with products.
        /// </summary>
        /// <param name="categories">Source categories</param>
        /// <returns>Categories with product count > 0</returns>
        public static IEnumerable<ProductServiceCategoryDto> WhereHasProducts(
            this IEnumerable<ProductServiceCategoryDto> categories)
        {
            return categories?.Where(c => c != null && c.ProductCount > 0) ?? Enumerable.Empty<ProductServiceCategoryDto>();
        }

        /// <summary>
        /// Filter categories to only those without products.
        /// </summary>
        /// <param name="categories">Source categories</param>
        /// <returns>Categories with product count = 0</returns>
        public static IEnumerable<ProductServiceCategoryDto> WhereNoProducts(
            this IEnumerable<ProductServiceCategoryDto> categories)
        {
            return categories?.Where(c => c != null && c.ProductCount == 0) ?? Enumerable.Empty<ProductServiceCategoryDto>();
        }

        #endregion

        #region ========== SORTING EXTENSIONS ==========

        /// <summary>
        /// Sort categories by SortOrder then CategoryName.
        /// </summary>
        /// <param name="categories">Source categories</param>
        /// <returns>Sorted categories</returns>
        public static IEnumerable<ProductServiceCategoryDto> OrderBySortOrder(
            this IEnumerable<ProductServiceCategoryDto> categories)
        {
            return categories?
                .OrderBy(c => c?.SortOrder ?? int.MaxValue)
                .ThenBy(c => c?.CategoryName ?? string.Empty) 
                ?? Enumerable.Empty<ProductServiceCategoryDto>();
        }

        /// <summary>
        /// Sort categories by CategoryName.
        /// </summary>
        /// <param name="categories">Source categories</param>
        /// <returns>Categories sorted by name</returns>
        public static IEnumerable<ProductServiceCategoryDto> OrderByName(
            this IEnumerable<ProductServiceCategoryDto> categories)
        {
            return categories?.OrderBy(c => c?.CategoryName ?? string.Empty) ?? Enumerable.Empty<ProductServiceCategoryDto>();
        }

        /// <summary>
        /// Sort categories by ProductCount descending.
        /// </summary>
        /// <param name="categories">Source categories</param>
        /// <returns>Categories sorted by product count</returns>
        public static IEnumerable<ProductServiceCategoryDto> OrderByProductCountDesc(
            this IEnumerable<ProductServiceCategoryDto> categories)
        {
            return categories?.OrderByDescending(c => c?.ProductCount ?? 0) ?? Enumerable.Empty<ProductServiceCategoryDto>();
        }

        #endregion

        #region ========== HIERARCHY EXTENSIONS ==========

        /// <summary>
        /// Get all children of a category (recursive).
        /// </summary>
        /// <param name="categories">All available categories</param>
        /// <param name="parentId">Parent category ID</param>
        /// <returns>All descendants of the parent</returns>
        public static List<ProductServiceCategoryDto> GetAllChildren(
            this IEnumerable<ProductServiceCategoryDto> categories,
            Guid? parentId)
        {
            var result = new List<ProductServiceCategoryDto>();
            if (categories == null) return result;

            var categoriesList = categories.ToList();
            var directChildren = categoriesList.Where(c => c.ParentId == parentId).ToList();

            foreach (var child in directChildren)
            {
                result.Add(child);
                result.AddRange(categoriesList.GetAllChildren(child.Id));
            }

            return result;
        }

        /// <summary>
        /// Build hierarchical tree from flat list.
        /// </summary>
        /// <param name="categories">Flat list of categories</param>
        /// <returns>Dictionary with root categories as keys and their children as values</returns>
        public static Dictionary<ProductServiceCategoryDto, List<ProductServiceCategoryDto>> BuildHierarchy(
            this IEnumerable<ProductServiceCategoryDto> categories)
        {
            var result = new Dictionary<ProductServiceCategoryDto, List<ProductServiceCategoryDto>>();
            if (categories == null) return result;

            var categoriesList = categories.ToList();
            var roots = categoriesList.WhereRoot().OrderBySortOrder().ToList();

            foreach (var root in roots)
            {
                var children = categoriesList.WhereParent(root.Id).OrderBySortOrder().ToList();
                result[root] = children;
            }

            return result;
        }

        /// <summary>
        /// Get category path from child to root (excluding root).
        /// </summary>
        /// <param name="category">Start category</param>
        /// <param name="allCategories">All available categories for lookup</param>
        /// <returns>Path of category names from child to parent</returns>
        public static string GetHierarchyPath(
            this ProductServiceCategoryDto category,
            IEnumerable<ProductServiceCategoryDto> allCategories)
        {
            if (category == null) return string.Empty;

            var path = new List<string> { category.CategoryName };
            var current = category;
            var categoriesDict = allCategories?.ToDictionary(c => c.Id) ?? new Dictionary<Guid, ProductServiceCategoryDto>();

            while (current.ParentId.HasValue && categoriesDict.ContainsKey(current.ParentId.Value))
            {
                current = categoriesDict[current.ParentId.Value];
                path.Insert(0, current.CategoryName);
            }

            return string.Join(" > ", path);
        }

        /// <summary>
        /// Get category depth in hierarchy (0 = root).
        /// </summary>
        /// <param name="category">Category to check</param>
        /// <param name="allCategories">All available categories for lookup</param>
        /// <returns>Depth level</returns>
        public static int GetHierarchyDepth(
            this ProductServiceCategoryDto category,
            IEnumerable<ProductServiceCategoryDto> allCategories)
        {
            if (category == null) return -1;

            int depth = 0;
            var current = category;
            var categoriesDict = allCategories?.ToDictionary(c => c.Id) ?? new Dictionary<Guid, ProductServiceCategoryDto>();

            while (current.ParentId.HasValue && categoriesDict.ContainsKey(current.ParentId.Value))
            {
                depth++;
                current = categoriesDict[current.ParentId.Value];
                if (depth > 100) break; // Prevent infinite loop
            }

            return depth;
        }

        #endregion

        #region ========== STATUS EXTENSIONS ==========

        /// <summary>
        /// Get status text for display.
        /// </summary>
        /// <param name="category">Category DTO</param>
        /// <returns>Status string (Active/Inactive)</returns>
        public static string GetStatusText(this ProductServiceCategoryDto category)
        {
            return category?.IsActive == true ? "Hoạt động" : "Vô hiệu hóa";
        }

        /// <summary>
        /// Check if category can be deactivated (has no active children with products).
        /// </summary>
        /// <param name="category">Category to check</param>
        /// <param name="allCategories">All categories for lookup</param>
        /// <returns>True if can be deactivated</returns>
        public static bool CanBeDeactivated(
            this ProductServiceCategoryDto category,
            IEnumerable<ProductServiceCategoryDto> allCategories)
        {
            if (category == null || !category.IsActive) return false;

            var children = allCategories.GetAllChildren(category.Id);
            return !children.Any(c => c.IsActive && c.ProductCount > 0);
        }

        #endregion

        #region ========== VALIDATION EXTENSIONS ==========

        /// <summary>
        /// Validate category data for business rules.
        /// </summary>
        /// <param name="category">Category DTO</param>
        /// <returns>List of validation errors (empty if valid)</returns>
        public static List<string> ValidateBusinessRules(this ProductServiceCategoryDto category)
        {
            var errors = new List<string>();

            if (category == null)
            {
                errors.Add("Danh mục không hợp lệ.");
                return errors;
            }

            if (string.IsNullOrWhiteSpace(category.CategoryCode))
                errors.Add("Mã danh mục không được để trống.");

            if (string.IsNullOrWhiteSpace(category.CategoryName))
                errors.Add("Tên danh mục không được để trống.");

            if (!string.IsNullOrWhiteSpace(category.CategoryCode) && category.CategoryCode.Length > 50)
                errors.Add("Mã danh mục không được vượt quá 50 ký tự.");

            if (!string.IsNullOrWhiteSpace(category.CategoryName) && category.CategoryName.Length > 200)
                errors.Add("Tên danh mục không được vượt quá 200 ký tự.");

            return errors;
        }

        #endregion
    }
}
