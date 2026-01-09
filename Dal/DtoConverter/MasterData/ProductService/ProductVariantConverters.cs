using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using Dal.DataContext;
using DTO.MasterData.ProductService;

namespace Dal.DtoConverter.MasterData.ProductService
{
    /// <summary>
    /// Converter giữa ProductVariant entity và ProductVariantDto
    /// </summary>
    public static class ProductVariantConverters
    {
        #region Entity to DTO

        public static ProductVariantSimpleDto ToProductVariantSimpleDto(this ProductVariant entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            return new ProductVariantSimpleDto
            {
                Id = entity.Id,
                ProductName = entity.ProductService?.Name,
                VariantFullName = entity.VariantFullName,
                VariantFullNamePlain = entity.VariantNameForReport,
                UnitName = entity.UnitOfMeasure?.Name,
                IsActive = entity.IsActive,
                ThumbnailImage = entity.ThumbnailImage?.ToArray()
            };
        }

        /// <summary>
        /// Chuyển đổi ProductVariant entity thành ProductVariantDto
        /// Chỉ map từ ProductVariant entity, không lấy dữ liệu từ entity khác
        /// </summary>
        /// <param name="entity">ProductVariant entity</param>
        /// <param name="productCode">Mã sản phẩm (để tránh DataContext disposed errors)</param>
        /// <param name="productName">Tên sản phẩm (để tránh DataContext disposed errors)</param>
        /// <param name="unitCode">Mã đơn vị (để tránh DataContext disposed errors)</param>
        /// <param name="unitName">Tên đơn vị (để tránh DataContext disposed errors)</param>
        /// <returns>ProductVariantDto</returns>
        public static ProductVariantDto ToDto(
            this ProductVariant entity,
            string productCode = null,
            string productName = null,
            string unitCode = null,
            string unitName = null)
        {
            if (entity == null) return null;

            var dto = new ProductVariantDto
            {
                // Các thuộc tính cơ bản
                Id = entity.Id,
                ProductId = entity.ProductId,
                ProductCode = productCode, // Có thể null nếu không có giá trị
                ProductName = productName, // Có thể null nếu không có giá trị
                VariantCode = entity.VariantCode, // Có thể null nếu không có giá trị
                UnitId = entity.UnitId,
                UnitCode = unitCode, // Có thể null nếu không có giá trị
                UnitName = unitName, // Có thể null nếu không có giá trị
                IsActive = entity.IsActive,

                // Các thuộc tính về hình ảnh
                ThumbnailImage = entity.ThumbnailImage?.ToArray(), // Chỉ lấy từ ProductVariant entity
                ProductThumbnailImage = null, // Không lấy từ entity khác, chỉ map từ entity chính
                ThumbnailFileName = entity.ThumbnailFileName,
                ThumbnailRelativePath = entity.ThumbnailRelativePath,
                ThumbnailFullPath = entity.ThumbnailFullPath,
                ThumbnailStorageType = entity.ThumbnailStorageType,
                ThumbnailFileSize = entity.ThumbnailFileSize,
                ThumbnailChecksum = entity.ThumbnailChecksum,

                // Các thuộc tính về ngày tháng
                CreatedDate = entity.CreatedDate,
                ModifiedDate = entity.ModifiedDate,

                // Các thuộc tính về tên
                VariantFullName = entity.VariantFullName,
                VariantNameForReport = entity.VariantNameForReport
            };

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
            Dictionary<Guid, DataContext.ProductService> productDict = null,
            Dictionary<Guid, UnitOfMeasure> unitDict = null)
        {
            if (entities == null) return new List<ProductVariantDto>();

            return entities.Select(entity =>
            {
                string productCode = null;
                string productName = null;
                if (productDict != null && productDict.TryGetValue(entity.ProductId, out var product))
                {
                    productCode = product.Code;
                    productName = product.Name;
                    // Không lấy ProductThumbnailImage từ ProductService, chỉ map từ ProductVariant entity
                }

                string unitCode = null;
                string unitName = null;
                if (unitDict != null && unitDict.TryGetValue(entity.UnitId, out var unit))
                {
                    unitCode = unit.Code;
                    unitName = unit.Name;
                }

                return entity.ToDto(productCode, productName, unitCode, unitName);
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
                    ThumbnailImage = dto.ThumbnailImage != null ? new Binary(dto.ThumbnailImage) : null,
                    CreatedDate = dto.CreatedDate != default(DateTime) ? dto.CreatedDate : DateTime.Now,
                    ModifiedDate = dto.ModifiedDate != default(DateTime) ? dto.ModifiedDate : DateTime.Now,
                    VariantFullName = dto.VariantFullName,
                    VariantNameForReport = dto.VariantNameForReport,
                    ThumbnailFileName = dto.ThumbnailFileName,
                    ThumbnailRelativePath = dto.ThumbnailRelativePath,
                    ThumbnailFullPath = dto.ThumbnailFullPath,
                    ThumbnailStorageType = dto.ThumbnailStorageType,
                    ThumbnailFileSize = dto.ThumbnailFileSize,
                    ThumbnailChecksum = dto.ThumbnailChecksum
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
                destination.ModifiedDate = DateTime.Now; // Cập nhật ngày sửa
                destination.VariantFullName = dto.VariantFullName;
                destination.VariantNameForReport = dto.VariantNameForReport;
                destination.ThumbnailFileName = dto.ThumbnailFileName;
                destination.ThumbnailRelativePath = dto.ThumbnailRelativePath;
                destination.ThumbnailFullPath = dto.ThumbnailFullPath;
                destination.ThumbnailStorageType = dto.ThumbnailStorageType;
                destination.ThumbnailFileSize = dto.ThumbnailFileSize;
                destination.ThumbnailChecksum = dto.ThumbnailChecksum;
            }

            return destination;
        }

        #endregion
    }
}
