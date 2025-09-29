using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bll.MasterData.ProductService;
using MasterData.ProductService.Dto;

namespace MasterData.ProductService
{
    /// <summary>
    /// Service xử lý pagination cho ProductService list
    /// </summary>
    public class ProductServicePaginationService
    {
        private readonly ProductServiceBll _productServiceBll;
        private const int DefaultPageSize = 50;
        private const int MaxPageSize = 200;

        public ProductServicePaginationService()
        {
            _productServiceBll = new ProductServiceBll();
        }

        /// <summary>
        /// Lấy dữ liệu với pagination
        /// </summary>
        /// <param name="pageIndex">Trang hiện tại (0-based)</param>
        /// <param name="pageSize">Số dòng mỗi trang</param>
        /// <param name="searchText">Từ khóa tìm kiếm</param>
        /// <param name="categoryId">Filter theo category</param>
        /// <param name="isService">Filter theo loại (null = all, true = service, false = product)</param>
        /// <param name="isActive">Filter theo trạng thái (null = all, true = active, false = inactive)</param>
        /// <returns>Paged result</returns>
        public async Task<PagedResult<ProductServiceDto>> GetPagedDataAsync(
            int pageIndex = 0, 
            int pageSize = DefaultPageSize,
            string searchText = null,
            Guid? categoryId = null,
            bool? isService = null,
            bool? isActive = null)
        {
            try
            {
                // Validate parameters
                pageSize = Math.Min(Math.Max(pageSize, 1), MaxPageSize);
                pageIndex = Math.Max(pageIndex, 0);

                // Get total count
                var totalCount = await _productServiceBll.GetCountAsync(searchText, categoryId, isService, isActive);

                // Get paged data
                var entities = await _productServiceBll.GetPagedAsync(
                    pageIndex, pageSize, searchText, categoryId, isService, isActive);

                // Convert to DTOs (without counting to improve performance)
                var dtoList = entities.ToDtoList(
                    categoryId => _productServiceBll.GetCategoryName(categoryId)
                ).ToList();

                return new PagedResult<ProductServiceDto>
                {
                    Data = dtoList,
                    TotalCount = totalCount,
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy dữ liệu phân trang: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy dữ liệu với counting (chậm hơn, dùng khi cần thiết)
        /// </summary>
        public async Task<PagedResult<ProductServiceDto>> GetPagedDataWithCountingAsync(
            int pageIndex = 0, 
            int pageSize = DefaultPageSize,
            string searchText = null,
            Guid? categoryId = null,
            bool? isService = null,
            bool? isActive = null)
        {
            try
            {
                var result = await GetPagedDataAsync(pageIndex, pageSize, searchText, categoryId, isService, isActive);
                
                // Count variants and images for current page
                var productIds = result.Data.Select(x => x.Id).ToList();
                var counts = await _productServiceBll.GetCountsForProductsAsync(productIds);

                // Update DTOs with counts
                foreach (var dto in result.Data)
                {
                    if (counts.ContainsKey(dto.Id))
                    {
                        dto.VariantCount = counts[dto.Id].VariantCount;
                        dto.ImageCount = counts[dto.Id].ImageCount;
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy dữ liệu phân trang với đếm: {ex.Message}", ex);
            }
        }
    }

    /// <summary>
    /// Kết quả phân trang
    /// </summary>
    /// <typeparam name="T">Kiểu dữ liệu</typeparam>
    public class PagedResult<T>
    {
        public List<T> Data { get; set; } = new List<T>();
        public int TotalCount { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage => PageIndex > 0;
        public bool HasNextPage => PageIndex < TotalPages - 1;
    }
}
