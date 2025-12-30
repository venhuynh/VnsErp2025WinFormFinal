using DTO.MasterData.ProductService;
using System;
using System.Collections.Generic;
using System.Linq;
using ProductImage = Dal.DataContext.ProductImage;

namespace Dal.DtoConverter
{
    /// <summary>
    /// Converter giữa ProductImage Entity và ProductImageDto
    /// Cung cấp các phương thức chuyển đổi qua lại giữa Entity và DTO
    /// </summary>
    public static class ProductImageDtoConverter
    {
        #region ProductImage Entity -> ProductImageDto

        /// <summary>
        /// Chuyển đổi ProductImage Entity sang ProductImageDto
        /// </summary>
        /// <param name="entity">ProductImage Entity cần chuyển đổi</param>
        /// <param name="productCode">Mã sản phẩm (từ navigation property hoặc parameter)</param>
        /// <param name="productName">Tên sản phẩm (từ navigation property hoặc parameter)</param>
        /// <param name="variantCode">Mã biến thể (từ navigation property hoặc parameter)</param>
        /// <param name="variantFullName">Tên biến thể đầy đủ (từ navigation property hoặc parameter)</param>
        /// <param name="unitName">Tên đơn vị tính (từ navigation property hoặc parameter)</param>
        /// <param name="imageSequenceNumber">Số thứ tự hình ảnh (tính toán từ danh sách)</param>
        /// <returns>ProductImageDto hoặc null nếu entity là null</returns>
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
                // Thông tin cơ bản
                Id = entity.Id,
                ProductId = entity.ProductId,

                // Thông tin sản phẩm (từ parameters hoặc navigation properties)
                ProductCode = productCode,
                ProductName = productName,
                VariantCode = variantCode,
                VariantFullName = variantFullName,
                UnitName = unitName,
                ImageSequenceNumber = imageSequenceNumber,

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

        #region ProductImageDto -> ProductImage Entity

        /// <summary>
        /// Chuyển đổi ProductImageDto sang ProductImage Entity
        /// </summary>
        /// <param name="dto">ProductImageDto cần chuyển đổi</param>
        /// <param name="existingEntity">Entity hiện tại (cho trường hợp update). Nếu null thì tạo mới</param>
        /// <returns>ProductImage Entity hoặc null nếu dto là null</returns>
        public static ProductImage ToEntity(this ProductImageDto dto, ProductImage existingEntity = null)
        {
            if (dto == null) return null;

            if (existingEntity == null)
            {
                // Tạo mới entity
                var entity = new ProductImage
                {
                    // ID: Sử dụng dto.Id nếu không rỗng, ngược lại tạo mới
                    Id = dto.Id != Guid.Empty ? dto.Id : Guid.NewGuid(),

                    // Thông tin cơ bản
                    ProductId = dto.ProductId,

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
                // existingEntity.Id = dto.Id; // Không thay đổi ID

                // Cập nhật thông tin cơ bản
                existingEntity.ProductId = dto.ProductId;

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
                if (dto.ImageData != null && dto.ImageData.Length > 0)
                {
                    existingEntity.ImageData = new System.Data.Linq.Binary(dto.ImageData);
                }
                // Nếu dto.ImageData là null hoặc rỗng, giữ nguyên giá trị hiện tại (không xóa)

                return existingEntity;
            }
        }

        /// <summary>
        /// Chuyển đổi danh sách ProductImageDto sang danh sách ProductImage Entity
        /// </summary>
        /// <param name="dtos">Danh sách ProductImageDto</param>
        /// <returns>Danh sách ProductImage Entity</returns>
        public static List<ProductImage> ToEntityList(this IEnumerable<ProductImageDto> dtos)
        {
            if (dtos == null) return new List<ProductImage>();
            return dtos.Select(d => d.ToEntity()).Where(e => e != null).ToList();
        }

        /// <summary>
        /// Chuyển đổi danh sách ProductImageDto sang danh sách ProductImage Entity (alias cho ToEntityList)
        /// </summary>
        /// <param name="dtos">Danh sách ProductImageDto</param>
        /// <returns>Danh sách ProductImage Entity</returns>
        public static List<ProductImage> ToEntities(this IEnumerable<ProductImageDto> dtos)
        {
            if (dtos == null) return new List<ProductImage>();
            return dtos.Select(d => d.ToEntity()).Where(e => e != null).ToList();
        }

        #endregion
    }
}
