using DTO.MasterData.ProductService;
using System;
using System.Collections.Generic;
using System.Linq;
using ProductImage = Dal.DataContext.ProductImage;

namespace Dal.DtoConverter
{
    /// <summary>
    /// Converter giữa ProductImage Entity và ProductImageDto
    /// </summary>
    public static class ProductImageConverters
    {
        #region ProductImage -> ProductImageDto

        /// <summary>
        /// Chuyển đổi ProductImage Entity sang ProductImageDto
        /// </summary>
        /// <param name="entity">ProductImage Entity</param>
        /// <param name="productCode">Mã sản phẩm (từ eager loading hoặc parameter)</param>
        /// <param name="productName">Tên sản phẩm (từ eager loading hoặc parameter)</param>
        /// <param name="variantCode">Mã biến thể (từ eager loading hoặc parameter)</param>
        /// <param name="variantFullName">Tên biến thể đầy đủ (từ eager loading hoặc parameter)</param>
        /// <param name="unitName">Tên đơn vị tính (từ eager loading hoặc parameter)</param>
        /// <param name="imageSequenceNumber">Số thứ tự hình ảnh (tính toán từ danh sách)</param>
        /// <returns>ProductImageDto</returns>
        public static ProductImageDto ToDto(
            this ProductImage entity,
            string productCode = null,
            string productName = null,
            string variantCode = null,
            string variantFullName = null,
            string unitName = null,
            int imageSequenceNumber = 0)
        {
            if (entity == null) return null;

            var dto = new ProductImageDto
            {
                Id = entity.Id,
                ProductId = entity.ProductId,
                FileName = entity.FileName,
                RelativePath = entity.RelativePath,
                FullPath = entity.FullPath,
                StorageType = entity.StorageType,
                FileSize = entity.FileSize,
                FileExtension = entity.FileExtension,
                MimeType = entity.MimeType,
                Checksum = entity.Checksum,
                FileExists = entity.FileExists,
                LastVerified = entity.LastVerified,
                MigrationStatus = entity.MigrationStatus,
                CreateDate = entity.CreateDate,
                CreateBy = entity.CreateBy,
                ModifiedDate = entity.ModifiedDate,
                ModifiedBy = entity.ModifiedBy,
                ProductCode = productCode,
                ProductName = productName,
                VariantCode = variantCode,
                VariantFullName = variantFullName,
                UnitName = unitName,
                ImageSequenceNumber = imageSequenceNumber
            };

            // Convert ImageData từ Binary sang byte[]
            if (entity.ImageData != null)
            {
                dto.ImageData = entity.ImageData.ToArray();
            }

            return dto;
        }

        /// <summary>
        /// Chuyển đổi danh sách ProductImage Entity sang danh sách ProductImageDto
        /// </summary>
        /// <param name="entities">Danh sách ProductImage Entity</param>
        /// <returns>Danh sách ProductImageDto</returns>
        public static List<ProductImageDto> ToDtoList(this IEnumerable<ProductImage> entities)
        {
            if (entities == null) return new List<ProductImageDto>();
            return entities.Select(e => e.ToDto()).ToList();
        }

        /// <summary>
        /// Chuyển đổi danh sách ProductImage Entity sang danh sách ProductImageDto (alias cho ToDtoList)
        /// </summary>
        /// <param name="entities">Danh sách ProductImage Entity</param>
        /// <returns>Danh sách ProductImageDto</returns>
        public static List<ProductImageDto> ToDtos(this IEnumerable<ProductImage> entities)
        {
            if (entities == null) return new List<ProductImageDto>();
            return entities.Select(e => e.ToDto()).ToList();
        }

        #endregion

        #region ProductImageDto -> ProductImage

        /// <summary>
        /// Chuyển đổi ProductImageDto sang ProductImage Entity
        /// </summary>
        /// <param name="dto">ProductImageDto</param>
        /// <param name="existingEntity">Entity hiện tại (cho update)</param>
        /// <returns>ProductImage Entity</returns>
        public static ProductImage ToEntity(this ProductImageDto dto, ProductImage existingEntity = null)
        {
            if (dto == null) return null;

            if (existingEntity == null)
            {
                // Tạo mới
                var entity = new ProductImage
                {
                    Id = dto.Id != Guid.Empty ? dto.Id : Guid.NewGuid(),
                    ProductId = dto.ProductId,
                    FileName = dto.FileName,
                    RelativePath = dto.RelativePath,
                    FullPath = dto.FullPath,
                    StorageType = dto.StorageType,
                    FileSize = dto.FileSize,
                    FileExtension = dto.FileExtension,
                    MimeType = dto.MimeType,
                    Checksum = dto.Checksum,
                    FileExists = dto.FileExists,
                    LastVerified = dto.LastVerified,
                    MigrationStatus = dto.MigrationStatus,
                    CreateDate = dto.CreateDate != default(DateTime) ? dto.CreateDate : DateTime.Now,
                    CreateBy = dto.CreateBy != Guid.Empty ? dto.CreateBy : Guid.Empty,
                    ModifiedDate = dto.ModifiedDate,
                    ModifiedBy = dto.ModifiedBy != Guid.Empty ? dto.ModifiedBy : Guid.Empty
                };

                // Convert ImageData từ byte[] sang Binary
                if (dto.ImageData != null)
                {
                    entity.ImageData = new System.Data.Linq.Binary(dto.ImageData);
                }
                else
                {
                    entity.ImageData = null;
                }

                return entity;
            }
            else
            {
                // Cập nhật entity hiện có
                existingEntity.ProductId = dto.ProductId;
                existingEntity.FileName = dto.FileName;
                existingEntity.RelativePath = dto.RelativePath;
                existingEntity.FullPath = dto.FullPath;
                existingEntity.StorageType = dto.StorageType;
                existingEntity.FileSize = dto.FileSize;
                existingEntity.FileExtension = dto.FileExtension;
                existingEntity.MimeType = dto.MimeType;
                existingEntity.Checksum = dto.Checksum;
                existingEntity.FileExists = dto.FileExists;
                existingEntity.LastVerified = dto.LastVerified;
                existingEntity.MigrationStatus = dto.MigrationStatus;
                existingEntity.ModifiedDate = DateTime.Now;
                existingEntity.ModifiedBy = dto.ModifiedBy != Guid.Empty ? dto.ModifiedBy : existingEntity.ModifiedBy;

                // Convert ImageData từ byte[] sang Binary (chỉ update nếu có giá trị mới)
                if (dto.ImageData != null)
                {
                    existingEntity.ImageData = new System.Data.Linq.Binary(dto.ImageData);
                }

                return existingEntity;
            }
        }

        #endregion
    }
}

