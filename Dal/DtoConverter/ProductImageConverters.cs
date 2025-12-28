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

            var entity = existingEntity ?? new ProductImage();

            // Chỉ set ID nếu là entity đã tồn tại (edit mode)
            // Khi tạo mới (existingEntity == null), không set Id từ dto để đảm bảo Id = Guid.Empty
            if (existingEntity != null)
            {
                // Edit mode: giữ nguyên Id của existing entity
                // Không cần set lại vì entity đã là existingEntity
            }
            else
            {
                // Create mode: đảm bảo Id = Guid.Empty (default của new ProductImage())
                entity.Id = Guid.Empty;
            }

            entity.ProductId = dto.ProductId;
            entity.FileName = dto.FileName;
            entity.RelativePath = dto.RelativePath;
            entity.FullPath = dto.FullPath;
            entity.StorageType = dto.StorageType;
            entity.FileSize = dto.FileSize;
            entity.FileExtension = dto.FileExtension;
            entity.MimeType = dto.MimeType;
            entity.Checksum = dto.Checksum;
            entity.FileExists = dto.FileExists;
            entity.LastVerified = dto.LastVerified;
            entity.MigrationStatus = dto.MigrationStatus;
            entity.CreateDate = dto.CreateDate;
            entity.CreateBy = dto.CreateBy;
            entity.ModifiedDate = dto.ModifiedDate;
            entity.ModifiedBy = dto.ModifiedBy;

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

        #endregion
    }
}

