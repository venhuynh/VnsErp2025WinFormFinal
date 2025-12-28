using Dal.DataContext;
using DTO.MasterData.ProductService;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;

namespace Dal.DtoConverter
{
    /// <summary>
    /// Converter giữa ProductVariant entity và ProductVariantDto
    /// </summary>
    public static class ProductVariantConverters
    {
        #region Entity to DTO

        /// <summary>
        /// Chuyển đổi ProductVariant entity thành ProductVariantDto
        /// </summary>
        /// <param name="entity">ProductVariant entity</param>
        /// <param name="productCode">Mã sản phẩm (để tránh DataContext disposed errors)</param>
        /// <param name="productName">Tên sản phẩm (để tránh DataContext disposed errors)</param>
        /// <param name="unitCode">Mã đơn vị (để tránh DataContext disposed errors)</param>
        /// <param name="unitName">Tên đơn vị (để tránh DataContext disposed errors)</param>
        /// <param name="productThumbnailImage">Ảnh thumbnail sản phẩm (để tránh DataContext disposed errors)</param>
        /// <returns>ProductVariantDto</returns>
        public static ProductVariantDto ToDto(
            this ProductVariant entity,
            string productCode = null,
            string productName = null,
            string unitCode = null,
            string unitName = null,
            byte[] productThumbnailImage = null)
        {
            if (entity == null) return null;

            var dto = new ProductVariantDto
            {
                Id = entity.Id,
                ProductId = entity.ProductId,
                ProductCode = productCode,
                ProductName = productName,
                VariantCode = entity.VariantCode,
                UnitId = entity.UnitId,
                UnitCode = unitCode,
                UnitName = unitName,
                IsActive = entity.IsActive,
                ThumbnailImage = entity.ThumbnailImage?.ToArray(),
                ProductThumbnailImage = productThumbnailImage,
                AttributeCount = 0,
                ImageCount = 0,
                Attributes = new List<ProductVariantAttributeDto>(),
                Images = new List<ProductImageDto>()
            };

            // Convert attributes nếu có eager loading
            if (entity.VariantAttributes != null && entity.VariantAttributes.Any())
            {
                dto.Attributes = entity.VariantAttributes.Select(va => new ProductVariantAttributeDto
                {
                    AttributeId = va.AttributeId,
                    AttributeName = va.Attribute?.Name,
                    AttributeValueId = va.AttributeValueId,
                    AttributeValue = va.AttributeValue?.Value,
                    Description = va.Attribute?.Description,
                }).ToList();
                dto.AttributeCount = dto.Attributes.Count;
            }

            return dto;
        }

        /// <summary>
        /// Chuyển đổi danh sách ProductVariant entities thành danh sách ProductVariantDto
        /// </summary>
        /// <param name="entities">Danh sách ProductVariant entities</param>
        /// <param name="productDict">Dictionary chứa ProductService entities (key = ProductId) để lấy productCode, productName</param>
        /// <param name="unitDict">Dictionary chứa UnitOfMeasure entities (key = UnitId) để lấy unitCode, unitName</param>
        /// <returns>Danh sách ProductVariantDto</returns>
        public static List<ProductVariantDto> ToDtos(
            this IEnumerable<ProductVariant> entities,
            Dictionary<Guid, ProductService> productDict = null,
            Dictionary<Guid, UnitOfMeasure> unitDict = null)
        {
            if (entities == null) return new List<ProductVariantDto>();

            return entities.Select(entity =>
            {
                string productCode = null;
                string productName = null;
                byte[] productThumbnailImage = null;
                if (productDict != null && productDict.TryGetValue(entity.ProductId, out var product))
                {
                    productCode = product.Code;
                    productName = product.Name;
                    productThumbnailImage = product.ThumbnailImage?.ToArray();
                }

                string unitCode = null;
                string unitName = null;
                if (unitDict != null && unitDict.TryGetValue(entity.UnitId, out var unit))
                {
                    unitCode = unit.Code;
                    unitName = unit.Name;
                }

                return entity.ToDto(productCode, productName, unitCode, unitName, productThumbnailImage);
            }).ToList();
        }

        #endregion

        #region DTO to Entity

        /// <summary>
        /// Chuyển đổi ProductVariantDto thành ProductVariant entity
        /// </summary>
        /// <param name="dto">ProductVariantDto</param>
        /// <param name="destination">Entity đích để cập nhật (nếu null thì tạo mới)</param>
        /// <returns>ProductVariant entity</returns>
        public static ProductVariant ToEntity(this ProductVariantDto dto, ProductVariant destination = null)
        {
            if (dto == null) return null;

            if (destination == null)
            {
                // Tạo mới
                destination = new ProductVariant
                {
                    Id = dto.Id == Guid.Empty ? Guid.NewGuid() : dto.Id,
                    ProductId = dto.ProductId,
                    VariantCode = dto.VariantCode,
                    UnitId = dto.UnitId,
                    IsActive = dto.IsActive,
                    ThumbnailImage = dto.ThumbnailImage != null ? new Binary(dto.ThumbnailImage) : null
                };
            }
            else
            {
                // Cập nhật
                destination.ProductId = dto.ProductId;
                destination.VariantCode = dto.VariantCode;
                destination.UnitId = dto.UnitId;
                destination.IsActive = dto.IsActive;
                if (dto.ThumbnailImage != null)
                {
                    destination.ThumbnailImage = new Binary(dto.ThumbnailImage);
                }
            }

            return destination;
        }

        #endregion
    }
}

