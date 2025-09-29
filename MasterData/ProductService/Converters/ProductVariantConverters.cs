using System;
using System.Collections.Generic;
using System.Linq;
using Dal.DataContext;
using MasterData.ProductService.Dto;

namespace MasterData.ProductService.Converters
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
        /// <param name="getProductName">Function để lấy tên sản phẩm</param>
        /// <param name="getProductCode">Function để lấy mã sản phẩm</param>
        /// <param name="getUnitName">Function để lấy tên đơn vị</param>
        /// <param name="getUnitCode">Function để lấy mã đơn vị</param>
        /// <returns>ProductVariantDto</returns>
        public static ProductVariantDto ToDto(
            this ProductVariant entity,
            Func<Guid, string> getProductName = null,
            Func<Guid, string> getProductCode = null,
            Func<Guid, string> getUnitName = null,
            Func<Guid, string> getUnitCode = null)
        {
            if (entity == null) return null;

            var dto = new ProductVariantDto
            {
                Id = entity.Id,
                ProductId = entity.ProductId,
                ProductCode = getProductCode?.Invoke(entity.ProductId),
                ProductName = getProductName?.Invoke(entity.ProductId),
                VariantCode = entity.VariantCode,
                UnitId = entity.UnitId,
                UnitCode = getUnitCode?.Invoke(entity.UnitId),
                UnitName = getUnitName?.Invoke(entity.UnitId),
                IsActive = entity.IsActive,
                ThumbnailImage = entity.ThumbnailImage?.ToArray(),
                AttributeCount = entity.VariantAttributes?.Count ?? 0,
                ImageCount = entity.ProductImages?.Count ?? 0
            };

            // Convert attributes
            if (entity.VariantAttributes != null && entity.VariantAttributes.Any())
            {
                dto.Attributes = entity.VariantAttributes.Select(va => new ProductVariantAttributeDto
                {
                    AttributeId = va.AttributeId,
                    AttributeName = va.Attribute?.Name,
                    AttributeValueId = va.AttributeValueId,
                    AttributeValue = va.AttributeValue?.Value,
                    Description = va.Attribute?.Description,
                    SortOrder = 0 // Default sort order
                }).ToList();
            }

            // Convert images
            if (entity.ProductImages != null && entity.ProductImages.Any())
            {
                dto.Images = entity.ProductImages.Select(pi => new ProductVariantImageDto
                {
                    ImageId = pi.Id,
                    ImagePath = pi.ImagePath,
                    ImageData = pi.ImageData?.ToArray(),
                    SortOrder = pi.SortOrder ?? 0,
                    IsPrimary = pi.IsPrimary ?? false,
                    ImageType = pi.ImageType,
                    ImageSize = pi.ImageSize,
                    ImageWidth = pi.ImageWidth,
                    ImageHeight = pi.ImageHeight,
                    Caption = pi.Caption,
                    AltText = pi.AltText,
                    IsActive = pi.IsActive ?? true
                }).ToList();
            }

            return dto;
        }

        /// <summary>
        /// Chuyển đổi danh sách ProductVariant entities thành danh sách ProductVariantDto
        /// </summary>
        /// <param name="entities">Danh sách ProductVariant entities</param>
        /// <param name="getProductName">Function để lấy tên sản phẩm</param>
        /// <param name="getProductCode">Function để lấy mã sản phẩm</param>
        /// <param name="getUnitName">Function để lấy tên đơn vị</param>
        /// <param name="getUnitCode">Function để lấy mã đơn vị</param>
        /// <returns>Danh sách ProductVariantDto</returns>
        public static List<ProductVariantDto> ToDtoList(
            this IEnumerable<ProductVariant> entities,
            Func<Guid, string> getProductName = null,
            Func<Guid, string> getProductCode = null,
            Func<Guid, string> getUnitName = null,
            Func<Guid, string> getUnitCode = null)
        {
            if (entities == null) return new List<ProductVariantDto>();

            return entities.Select(entity => entity.ToDto(
                getProductName,
                getProductCode,
                getUnitName,
                getUnitCode
            )).ToList();
        }

        #endregion

        #region DTO to Entity

        /// <summary>
        /// Chuyển đổi ProductVariantDto thành ProductVariant entity
        /// </summary>
        /// <param name="dto">ProductVariantDto</param>
        /// <returns>ProductVariant entity</returns>
        public static ProductVariant ToEntity(this ProductVariantDto dto)
        {
            if (dto == null) return null;

            var entity = new ProductVariant
            {
                Id = dto.Id,
                ProductId = dto.ProductId,
                VariantCode = dto.VariantCode,
                UnitId = dto.UnitId,
                IsActive = dto.IsActive,
                ThumbnailImage = dto.ThumbnailImage
            };

            return entity;
        }

        /// <summary>
        /// Chuyển đổi danh sách ProductVariantDto thành danh sách ProductVariant entities
        /// </summary>
        /// <param name="dtos">Danh sách ProductVariantDto</param>
        /// <returns>Danh sách ProductVariant entities</returns>
        public static List<ProductVariant> ToEntityList(this IEnumerable<ProductVariantDto> dtos)
        {
            if (dtos == null) return new List<ProductVariant>();

            return dtos.Select(dto => dto.ToEntity()).ToList();
        }

        #endregion

        #region Update Entity from DTO

        /// <summary>
        /// Cập nhật ProductVariant entity từ ProductVariantDto
        /// </summary>
        /// <param name="entity">ProductVariant entity cần cập nhật</param>
        /// <param name="dto">ProductVariantDto chứa dữ liệu mới</param>
        public static void UpdateFromDto(this ProductVariant entity, ProductVariantDto dto)
        {
            if (entity == null || dto == null) return;

            entity.ProductId = dto.ProductId;
            entity.VariantCode = dto.VariantCode;
            entity.UnitId = dto.UnitId;
            entity.IsActive = dto.IsActive;
            entity.ThumbnailImage = dto.ThumbnailImage;
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Tạo ProductVariantDto mới với thông tin cơ bản
        /// </summary>
        /// <param name="productId">ID sản phẩm gốc</param>
        /// <param name="variantCode">Mã biến thể</param>
        /// <param name="unitId">ID đơn vị tính</param>
        /// <param name="getProductName">Function để lấy tên sản phẩm</param>
        /// <param name="getProductCode">Function để lấy mã sản phẩm</param>
        /// <param name="getUnitName">Function để lấy tên đơn vị</param>
        /// <param name="getUnitCode">Function để lấy mã đơn vị</param>
        /// <returns>ProductVariantDto mới</returns>
        public static ProductVariantDto CreateNew(
            Guid productId,
            string variantCode,
            Guid unitId,
            Func<Guid, string> getProductName = null,
            Func<Guid, string> getProductCode = null,
            Func<Guid, string> getUnitName = null,
            Func<Guid, string> getUnitCode = null)
        {
            var dto = new ProductVariantDto(productId, variantCode, unitId)
            {
                ProductCode = getProductCode?.Invoke(productId),
                ProductName = getProductName?.Invoke(productId),
                UnitCode = getUnitCode?.Invoke(unitId),
                UnitName = getUnitName?.Invoke(unitId)
            };

            return dto;
        }

        /// <summary>
        /// Tạo danh sách ProductVariantDto từ template
        /// </summary>
        /// <param name="productId">ID sản phẩm gốc</param>
        /// <param name="unitId">ID đơn vị tính</param>
        /// <param name="variantCodes">Danh sách mã biến thể</param>
        /// <param name="getProductName">Function để lấy tên sản phẩm</param>
        /// <param name="getProductCode">Function để lấy mã sản phẩm</param>
        /// <param name="getUnitName">Function để lấy tên đơn vị</param>
        /// <param name="getUnitCode">Function để lấy mã đơn vị</param>
        /// <returns>Danh sách ProductVariantDto</returns>
        public static List<ProductVariantDto> CreateBulk(
            Guid productId,
            Guid unitId,
            IEnumerable<string> variantCodes,
            Func<Guid, string> getProductName = null,
            Func<Guid, string> getProductCode = null,
            Func<Guid, string> getUnitName = null,
            Func<Guid, string> getUnitCode = null)
        {
            if (variantCodes == null) return new List<ProductVariantDto>();

            return variantCodes.Select(variantCode => CreateNew(
                productId,
                variantCode,
                unitId,
                getProductName,
                getProductCode,
                getUnitName,
                getUnitCode
            )).ToList();
        }

        /// <summary>
        /// Tạo ProductVariantDto từ ProductVariant entity với thông tin đầy đủ
        /// </summary>
        /// <param name="entity">ProductVariant entity</param>
        /// <returns>ProductVariantDto với thông tin đầy đủ</returns>
        public static ProductVariantDto ToFullDto(this ProductVariant entity)
        {
            if (entity == null) return null;

            var dto = new ProductVariantDto
            {
                Id = entity.Id,
                ProductId = entity.ProductId,
                VariantCode = entity.VariantCode,
                UnitId = entity.UnitId,
                IsActive = entity.IsActive,
                ThumbnailImage = entity.ThumbnailImage?.ToArray()
            };

            // Load product information
            if (entity.ProductService != null)
            {
                dto.ProductCode = entity.ProductService.Code;
                dto.ProductName = entity.ProductService.Name;
            }

            // Load unit information
            if (entity.UnitOfMeasure != null)
            {
                dto.UnitCode = entity.UnitOfMeasure.Code;
                dto.UnitName = entity.UnitOfMeasure.Name;
            }

            // Convert attributes
            if (entity.VariantAttributes != null && entity.VariantAttributes.Any())
            {
                dto.Attributes = entity.VariantAttributes.Select(va => new ProductVariantAttributeDto
                {
                    AttributeId = va.AttributeId,
                    AttributeName = va.Attribute?.Name,
                    AttributeValueId = va.AttributeValueId,
                    AttributeValue = va.AttributeValue?.Value,
                    Description = va.Attribute?.Description,
                    SortOrder = 0
                }).ToList();
                dto.AttributeCount = dto.Attributes.Count;
            }

            // Convert images
            if (entity.ProductImages != null && entity.ProductImages.Any())
            {
                dto.Images = entity.ProductImages.Select(pi => new ProductVariantImageDto
                {
                    ImageId = pi.Id,
                    ImagePath = pi.ImagePath,
                    ImageData = pi.ImageData?.ToArray(),
                    SortOrder = pi.SortOrder ?? 0,
                    IsPrimary = pi.IsPrimary ?? false,
                    ImageType = pi.ImageType,
                    ImageSize = pi.ImageSize,
                    ImageWidth = pi.ImageWidth,
                    ImageHeight = pi.ImageHeight,
                    Caption = pi.Caption,
                    AltText = pi.AltText,
                    IsActive = pi.IsActive ?? true
                }).ToList();
                dto.ImageCount = dto.Images.Count;
            }

            return dto;
        }

        /// <summary>
        /// Tạo danh sách ProductVariantDto từ danh sách ProductVariant entities với thông tin đầy đủ
        /// </summary>
        /// <param name="entities">Danh sách ProductVariant entities</param>
        /// <returns>Danh sách ProductVariantDto với thông tin đầy đủ</returns>
        public static List<ProductVariantDto> ToFullDtoList(this IEnumerable<ProductVariant> entities)
        {
            if (entities == null) return new List<ProductVariantDto>();

            return entities.Select(entity => entity.ToFullDto()).ToList();
        }

        #endregion

        #region Validation Helpers

        /// <summary>
        /// Kiểm tra tính hợp lệ của ProductVariantDto
        /// </summary>
        /// <param name="dto">ProductVariantDto cần kiểm tra</param>
        /// <returns>True nếu hợp lệ</returns>
        public static bool IsValidDto(this ProductVariantDto dto)
        {
            return dto?.IsValid() == true;
        }

        /// <summary>
        /// Lấy danh sách lỗi validation của ProductVariantDto
        /// </summary>
        /// <param name="dto">ProductVariantDto cần kiểm tra</param>
        /// <returns>Danh sách lỗi</returns>
        public static List<string> GetValidationErrors(this ProductVariantDto dto)
        {
            return dto?.GetValidationErrors() ?? new List<string> { "DTO không được null" };
        }

        #endregion
    }
}
