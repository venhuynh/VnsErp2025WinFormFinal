using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Dal.DataContext;
using MasterData.ProductService.Dto;

namespace MasterData.ProductService.Dto
{
    /// <summary>
    /// DTO view tổng hợp Master-Detail cho ProductService và ProductVariant
    /// Sử dụng cho các màn hình hiển thị thông tin đầy đủ sản phẩm và các biến thể
    /// </summary>
    public class ProductServiceMasterDetailViewDto
    {
        // === THÔNG TIN MASTER (ProductService) ===
        [Display(Name = "ID sản phẩm")]
        public Guid ProductId { get; set; }

        [Display(Name = "Tên sản phẩm")]
        public string ProductName { get; set; }

        [Display(Name = "Trạng thái sản phẩm")]
        public bool ProductIsActive { get; set; }

        [Display(Name = "Hình đại diện sản phẩm")]
        public byte[] ProductThumbnailImage { get; set; }


        // === DANH SÁCH BIẾN THỂ (DETAIL) ===
        [Display(Name = "Danh sách biến thể")]
        public List<ProductVariantDetailViewDto> Variants { get; set; } = new List<ProductVariantDetailViewDto>();
        
    }

    /// <summary>
    /// DTO chi tiết cho biến thể sản phẩm trong view Master-Detail
    /// </summary>
    public class ProductVariantDetailViewDto
    {
        [Display(Name = "ID biến thể")]
        public Guid VariantId { get; set; }

        [Display(Name = "Mã biến thể")]
        public string VariantCode { get; set; }

        [Display(Name = "Tên đầy đủ")]
        public string VariantName { get; set; }

        [Display(Name = "Đơn vị tính")]
        public string UnitName { get; set; }

    }

    /// <summary>
    /// Helper class chứa các phương thức convert Entity sang DTO
    /// Tuân thủ nguyên tắc separation of concerns
    /// </summary>
    public static class ProductVariantConverter
    {
        /// <summary>
        /// Convert danh sách ProductVariant entities sang ProductVariantDto
        /// Tuân thủ quy tắc: Entity -> DTO (chỉ trong DTO layer)
        /// </summary>
        /// <param name="variants">Danh sách ProductVariant entities</param>
        /// <returns>Danh sách ProductVariantDto</returns>
        public static List<ProductVariantDto> ConvertEntitiesToDtos(List<ProductVariant> variants)
        {
            try
            {
                var dtoList = new List<ProductVariantDto>();
                
                foreach (var variant in variants)
                {
                    var dto = new ProductVariantDto
                    {
                        Id = variant.Id,
                        ProductId = variant.ProductId,
                        VariantCode = variant.VariantCode,
                        UnitId = variant.UnitId,
                        IsActive = variant.IsActive,
                        ThumbnailImage = variant.ThumbnailImage?.ToArray()
                    };
                    
                    // Lấy thông tin từ navigation properties (đã được load từ BLL)
                    if (variant.ProductService != null)
                    {
                        dto.ProductCode = variant.ProductService.Code;
                        dto.ProductName = variant.ProductService.Name;
                        dto.ProductThumbnailImage = variant.ProductService.ThumbnailImage?.ToArray();
                    }
                    
                    if (variant.UnitOfMeasure != null)
                    {
                        dto.UnitCode = variant.UnitOfMeasure.Code;
                        dto.UnitName = variant.UnitOfMeasure.Name;
                    }
                    
                    // TODO: Load thông tin thuộc tính và hình ảnh nếu cần
                    dto.AttributeCount = 0; // Tạm thời
                    dto.ImageCount = 0; // Tạm thời
                    dto.Attributes = new List<ProductVariantAttributeDto>();
                    dto.Images = new List<ProductVariantImageDto>();
                    
                    dtoList.Add(dto);
                }
                
                return dtoList;
            }
            catch (Exception ex)
            {
                // Log error nếu cần
                throw new Exception($"Lỗi convert Entity sang DTO: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Convert danh sách ProductVariant entities sang ProductServiceMasterDetailViewDto
        /// Tạo cấu trúc Master-Detail view
        /// </summary>
        /// <param name="variants">Danh sách ProductVariant entities</param>
        /// <returns>Danh sách ProductServiceMasterDetailViewDto</returns>
        public static List<ProductServiceMasterDetailViewDto> ConvertToMasterDetailView(List<ProductVariant> variants)
        {
            try
            {
                var masterDetailList = new List<ProductServiceMasterDetailViewDto>();
                
                // Nhóm variants theo ProductId
                var groupedVariants = variants.GroupBy(v => v.ProductId);
                
                foreach (var group in groupedVariants)
                {
                    var firstVariant = group.First();
                    var masterDto = new ProductServiceMasterDetailViewDto
                    {
                        ProductId = firstVariant.ProductId,
                        ProductName = firstVariant.ProductService?.Name ?? "",
                        ProductIsActive = firstVariant.ProductService?.IsActive ?? false,
                        ProductThumbnailImage = firstVariant.ProductService?.ThumbnailImage?.ToArray()
                    };
                    
                    // Convert các variants thành detail DTOs
                    masterDto.Variants = group.Select(v => new ProductVariantDetailViewDto
                    {
                        VariantId = v.Id,
                        VariantCode = v.VariantCode,
                        VariantName = $"{v.VariantCode} - {v.UnitOfMeasure?.Name ?? ""}",
                        UnitName = v.UnitOfMeasure?.Name ?? ""
                    }).ToList();
                    
                    masterDetailList.Add(masterDto);
                }
                
                return masterDetailList;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi convert sang Master-Detail view: {ex.Message}", ex);
            }
        }
    }

}
