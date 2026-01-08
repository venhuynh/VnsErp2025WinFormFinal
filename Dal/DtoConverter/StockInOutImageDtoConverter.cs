using DTO.Inventory.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using StockInOutImage = Dal.DataContext.StockInOutImage;

namespace Dal.DtoConverter
{
    /// <summary>
    /// Converter giữa StockInOutImage Entity và StockInOutImageDto
    /// Cung cấp các phương thức chuyển đổi qua lại giữa Entity và DTO
    /// </summary>
    public static class StockInOutImageDtoConverter
    {
        #region StockInOutImage Entity -> StockInOutImageDto

        /// <summary>
        /// Chuyển đổi StockInOutImage Entity sang StockInOutImageDto
        /// </summary>
        /// <param name="entity">StockInOutImage Entity cần chuyển đổi</param>
        /// <returns>StockInOutImageDto hoặc null nếu entity là null</returns>
        public static StockInOutImageDto ToDto(this StockInOutImage entity)
        {
            if (entity == null) return null;

            var dto = new StockInOutImageDto
            {
                // Thông tin cơ bản
                Id = entity.Id,
                StockInOutMasterId = entity.StockInOutMasterId,

                // Thông tin file
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

                // Thông tin hệ thống
                CreateDate = entity.CreateDate,
                CreateBy = entity.CreateBy,
                ModifiedDate = entity.ModifiedDate,
                ModifiedBy = entity.ModifiedBy
            };

            // Chuyển đổi ImageData từ System.Data.Linq.Binary sang byte[]
            if (entity.ImageData != null)
            {
                dto.ImageData = entity.ImageData.ToArray();
            }
            else
            {
                dto.ImageData = null;
            }

            return dto;
        }

        /// <summary>
        /// Chuyển đổi danh sách StockInOutImage Entity sang danh sách StockInOutImageDto
        /// </summary>
        /// <param name="entities">Danh sách StockInOutImage Entity</param>
        /// <returns>Danh sách StockInOutImageDto</returns>
        public static List<StockInOutImageDto> ToDtoList(this IEnumerable<StockInOutImage> entities)
        {
            if (entities == null) return new List<StockInOutImageDto>();
            return entities.Select(e => e.ToDto()).Where(d => d != null).ToList();
        }

        /// <summary>
        /// Chuyển đổi danh sách StockInOutImage Entity sang danh sách StockInOutImageDto (alias cho ToDtoList)
        /// </summary>
        /// <param name="entities">Danh sách StockInOutImage Entity</param>
        /// <returns>Danh sách StockInOutImageDto</returns>
        public static List<StockInOutImageDto> ToDtos(this IEnumerable<StockInOutImage> entities)
        {
            if (entities == null) return new List<StockInOutImageDto>();
            return entities.Select(e => e.ToDto()).Where(d => d != null).ToList();
        }

        #endregion

        #region StockInOutImageDto -> StockInOutImage Entity

        /// <summary>
        /// Chuyển đổi StockInOutImageDto sang StockInOutImage Entity
        /// </summary>
        /// <param name="dto">StockInOutImageDto cần chuyển đổi</param>
        /// <param name="existingEntity">Entity hiện tại (cho trường hợp update). Nếu null thì tạo mới</param>
        /// <returns>StockInOutImage Entity hoặc null nếu dto là null</returns>
        public static StockInOutImage ToEntity(this StockInOutImageDto dto, StockInOutImage existingEntity = null)
        {
            if (dto == null) return null;

            if (existingEntity == null)
            {
                // Tạo mới entity
                var entity = new StockInOutImage
                {
                    // ID: Sử dụng dto.Id nếu không rỗng, ngược lại tạo mới
                    Id = dto.Id != Guid.Empty ? dto.Id : Guid.NewGuid(),

                    // Thông tin cơ bản
                    StockInOutMasterId = dto.StockInOutMasterId,

                    // Thông tin file
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

                    // Thông tin hệ thống - Create
                    CreateDate = dto.CreateDate != default(DateTime) ? dto.CreateDate : DateTime.Now,
                    CreateBy = dto.CreateBy != Guid.Empty ? dto.CreateBy : Guid.Empty,

                    // Thông tin hệ thống - Modified (null cho entity mới)
                    ModifiedDate = null,
                    ModifiedBy = Guid.Empty
                };

                // Chuyển đổi ImageData từ byte[] sang System.Data.Linq.Binary
                // Lưu ý: Thông thường ImageData không được lưu vào database nữa (lưu vào storage)
                // Nhưng vẫn giữ logic này để tương thích
                if (dto.ImageData != null && dto.ImageData.Length > 0)
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
                // Không thay đổi Id và CreateDate, CreateBy (thông tin audit tạo)

                // Cập nhật thông tin cơ bản
                existingEntity.StockInOutMasterId = dto.StockInOutMasterId;

                // Cập nhật thông tin file
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

                // Cập nhật thông tin hệ thống - Modified
                existingEntity.ModifiedDate = DateTime.Now;
                existingEntity.ModifiedBy = dto.ModifiedBy != Guid.Empty ? dto.ModifiedBy : existingEntity.ModifiedBy;

                // Cập nhật ImageData nếu có giá trị mới
                // Lưu ý: Thông thường ImageData không được lưu vào database nữa
                if (dto.ImageData != null && dto.ImageData.Length > 0)
                {
                    existingEntity.ImageData = new System.Data.Linq.Binary(dto.ImageData);
                }
                // Nếu dto.ImageData là null hoặc rỗng, giữ nguyên giá trị hiện tại (không xóa)

                return existingEntity;
            }
        }

        /// <summary>
        /// Chuyển đổi danh sách StockInOutImageDto sang danh sách StockInOutImage Entity
        /// </summary>
        /// <param name="dtos">Danh sách StockInOutImageDto</param>
        /// <returns>Danh sách StockInOutImage Entity</returns>
        public static List<StockInOutImage> ToEntityList(this IEnumerable<StockInOutImageDto> dtos)
        {
            if (dtos == null) return new List<StockInOutImage>();
            return dtos.Select(d => d.ToEntity()).Where(e => e != null).ToList();
        }

        /// <summary>
        /// Chuyển đổi danh sách StockInOutImageDto sang danh sách StockInOutImage Entity (alias cho ToEntityList)
        /// </summary>
        /// <param name="dtos">Danh sách StockInOutImageDto</param>
        /// <returns>Danh sách StockInOutImage Entity</returns>
        public static List<StockInOutImage> ToEntities(this IEnumerable<StockInOutImageDto> dtos)
        {
            if (dtos == null) return new List<StockInOutImage>();
            return dtos.Select(d => d.ToEntity()).Where(e => e != null).ToList();
        }

        #endregion
    }
}
