using DTO.Inventory.StockTakking;
using System;
using System.Collections.Generic;
using System.Linq;
using StocktakingImage = Dal.DataContext.StocktakingImage;

namespace Dal.DtoConverter.Inventory.StockTakking
{
    /// <summary>
    /// Converter giữa StocktakingImage entity và StocktakingImageDto
    /// </summary>
    public static class StocktakingImageDtoConverter
    {
        #region Entity to DTO

        /// <summary>
        /// Chuyển đổi StocktakingImage entity thành StocktakingImageDto
        /// </summary>
        /// <param name="entity">StocktakingImage entity</param>
        /// <returns>StocktakingImageDto</returns>
        public static StocktakingImageDto ToDto(this StocktakingImage entity)
        {
            if (entity == null) return null;

            // QUAN TRỌNG: KHÔNG truy cập navigation properties (entity.StocktakingMaster, etc.)
            // vì DataContext đã bị dispose sau khi repository method kết thúc

            var dto = new StocktakingImageDto
            {
                // Thông tin cơ bản
                Id = entity.Id,
                StocktakingMasterId = entity.StocktakingMasterId,
                StocktakingDetailId = entity.StocktakingDetailId,

                // Dữ liệu hình ảnh
                ImageData = entity.ImageData?.ToArray(), // Convert Binary sang byte[]

                // Thông tin file
                FileName = entity.FileName,
                RelativePath = entity.RelativePath,
                FullPath = entity.FullPath,
                StorageType = entity.StorageType,
                FileSize = entity.FileSize,
                FileExtension = entity.FileExtension,
                MimeType = entity.MimeType,
                Checksum = entity.Checksum,

                // Trạng thái file
                FileExists = entity.FileExists,
                LastVerified = entity.LastVerified,
                MigrationStatus = entity.MigrationStatus,

                // Audit fields
                CreateDate = entity.CreateDate,
                CreateBy = entity.CreateBy,
                ModifiedDate = entity.ModifiedDate,
                ModifiedBy = entity.ModifiedBy
            };

            return dto;
        }

        /// <summary>
        /// Chuyển đổi danh sách StocktakingImage entities thành danh sách StocktakingImageDto
        /// </summary>
        /// <param name="entities">Danh sách StocktakingImage entities</param>
        /// <returns>Danh sách StocktakingImageDto</returns>
        public static List<StocktakingImageDto> ToDtoList(this IEnumerable<StocktakingImage> entities)
        {
            if (entities == null) return new List<StocktakingImageDto>();
            return entities.Select(e => e.ToDto()).Where(d => d != null).ToList();
        }

        /// <summary>
        /// Chuyển đổi danh sách StocktakingImage entities thành danh sách StocktakingImageDto (alias)
        /// </summary>
        /// <param name="entities">Danh sách StocktakingImage entities</param>
        /// <returns>Danh sách StocktakingImageDto</returns>
        public static List<StocktakingImageDto> ToDtos(this IEnumerable<StocktakingImage> entities)
        {
            if (entities == null) return new List<StocktakingImageDto>();
            return entities.Select(e => e.ToDto()).Where(d => d != null).ToList();
        }

        #endregion

        #region DTO to Entity

        /// <summary>
        /// Chuyển đổi StocktakingImageDto thành StocktakingImage entity
        /// </summary>
        /// <param name="dto">StocktakingImageDto</param>
        /// <param name="existingEntity">Existing entity to update (optional, for edit mode)</param>
        /// <returns>StocktakingImage entity</returns>
        public static StocktakingImage ToEntity(this StocktakingImageDto dto, StocktakingImage existingEntity = null)
        {
            if (dto == null) return null;

            StocktakingImage entity;
            if (existingEntity != null)
            {
                // Update existing entity
                entity = existingEntity;
            }
            else
            {
                // Create new entity
                entity = new StocktakingImage();
                if (dto.Id != Guid.Empty)
                {
                    entity.Id = dto.Id;
                }
                else
                {
                    entity.Id = Guid.NewGuid();
                }
            }

            // Map properties - Thông tin cơ bản
            entity.StocktakingMasterId = dto.StocktakingMasterId;
            entity.StocktakingDetailId = dto.StocktakingDetailId;

            // Map properties - Dữ liệu hình ảnh
            // Chuyển đổi ImageData từ byte[] sang System.Data.Linq.Binary
            if (dto.ImageData != null && dto.ImageData.Length > 0)
            {
                entity.ImageData = new System.Data.Linq.Binary(dto.ImageData);
            }
            else
            {
                entity.ImageData = null;
            }

            // Map properties - Thông tin file
            entity.FileName = dto.FileName;
            entity.RelativePath = dto.RelativePath;
            entity.FullPath = dto.FullPath;
            entity.StorageType = dto.StorageType;
            entity.FileSize = dto.FileSize;
            entity.FileExtension = dto.FileExtension;
            entity.MimeType = dto.MimeType;
            entity.Checksum = dto.Checksum;

            // Map properties - Trạng thái file
            entity.FileExists = dto.FileExists;
            entity.LastVerified = dto.LastVerified;
            entity.MigrationStatus = dto.MigrationStatus;

            // Map properties - Audit fields
            entity.ModifiedDate = dto.ModifiedDate ?? DateTime.Now;
            entity.ModifiedBy = dto.ModifiedBy;

            if (existingEntity == null)
            {
                // Chỉ set CreateDate và CreateBy khi tạo mới
                entity.CreateDate = dto.CreateDate == default(DateTime) ? DateTime.Now : dto.CreateDate;
                entity.CreateBy = dto.CreateBy;
            }

            return entity;
        }

        /// <summary>
        /// Chuyển đổi danh sách StocktakingImageDto thành danh sách StocktakingImage entities
        /// </summary>
        /// <param name="dtos">Danh sách StocktakingImageDto</param>
        /// <returns>Danh sách StocktakingImage entities</returns>
        public static List<StocktakingImage> ToEntityList(this IEnumerable<StocktakingImageDto> dtos)
        {
            if (dtos == null) return new List<StocktakingImage>();
            return dtos.Select(d => d.ToEntity()).Where(e => e != null).ToList();
        }

        /// <summary>
        /// Chuyển đổi danh sách StocktakingImageDto thành danh sách StocktakingImage entities (alias)
        /// </summary>
        /// <param name="dtos">Danh sách StocktakingImageDto</param>
        /// <returns>Danh sách StocktakingImage entities</returns>
        public static List<StocktakingImage> ToEntities(this IEnumerable<StocktakingImageDto> dtos)
        {
            if (dtos == null) return new List<StocktakingImage>();
            return dtos.Select(d => d.ToEntity()).Where(e => e != null).ToList();
        }

        #endregion
    }
}
