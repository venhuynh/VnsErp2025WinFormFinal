using Dal.DataContext;
using DTO.MasterData.ProductService;
using System;
using System.Collections.Generic;
using System.Linq;
using Attribute = Dal.DataContext.Attribute;

namespace Dal.DtoConverter
{
    /// <summary>
    /// Converter giữa Attribute entity và AttributeDto
    /// </summary>
    public static class AttributeDtoConverter
    {
        #region Entity to DTO

        /// <summary>
        /// Chuyển đổi Attribute entity thành AttributeDto
        /// </summary>
        /// <param name="entity">Attribute entity</param>
        /// <returns>AttributeDto</returns>
        public static AttributeDto ToDto(this Attribute entity)
        {
            if (entity == null) return null;

            var dto = new AttributeDto
            {
                Id = entity.Id,
                Name = entity.Name,
                DataType = entity.DataType,
                Description = entity.Description,
                AttributeValues = new List<AttributeValueDto>()
            };

            // Convert AttributeValues nếu đã được load (eager loading)
            // Sử dụng try-catch để tránh DataContext disposed errors
            try
            {
                if (entity.AttributeValues != null && entity.AttributeValues.Any())
                {
                    dto.AttributeValues = entity.AttributeValues.Select(av => new AttributeValueDto
                    {
                        Id = av.Id,
                        AttributeId = av.AttributeId,
                        Value = av.Value,
                        AttributeName = entity.Name,
                        AttributeDataType = entity.DataType,
                        AttributeDescription = entity.Description
                    }).ToList();
                }
            }
            catch
            {
                // Nếu không thể load AttributeValues (DataContext disposed), để danh sách rỗng
                dto.AttributeValues = new List<AttributeValueDto>();
            }

            return dto;
        }

        /// <summary>
        /// Chuyển đổi danh sách Attribute entities thành danh sách AttributeDto
        /// </summary>
        /// <param name="entities">Danh sách Attribute entities</param>
        /// <returns>Danh sách AttributeDto</returns>
        public static List<AttributeDto> ToDtos(this IEnumerable<Attribute> entities)
        {
            if (entities == null) return new List<AttributeDto>();

            return entities.Select(entity => entity.ToDto()).Where(dto => dto != null).ToList();
        }

        /// <summary>
        /// Chuyển đổi danh sách Attribute entities thành danh sách AttributeDto (alias cho ToDtos)
        /// </summary>
        /// <param name="entities">Danh sách Attribute entities</param>
        /// <returns>Danh sách AttributeDto</returns>
        public static List<AttributeDto> ToDtoList(this IEnumerable<Attribute> entities)
        {
            if (entities == null) return new List<AttributeDto>();

            return entities.Select(entity => entity.ToDto()).Where(dto => dto != null).ToList();
        }

        #endregion

        #region DTO to Entity

        /// <summary>
        /// Chuyển đổi AttributeDto thành Attribute entity
        /// </summary>
        /// <param name="dto">AttributeDto</param>
        /// <param name="existingEntity">Entity hiện tại (cho update, nếu null thì tạo mới)</param>
        /// <returns>Attribute entity</returns>
        public static Attribute ToEntity(this AttributeDto dto, Attribute existingEntity = null)
        {
            if (dto == null) return null;

            var entity = existingEntity ?? new Attribute();

            // Chỉ set ID nếu là entity đã tồn tại (edit mode)
            // Khi tạo mới (existingEntity == null), không set Id từ dto để đảm bảo Id = Guid.Empty
            if (existingEntity != null)
            {
                // Edit mode: giữ nguyên Id của existing entity
                // Không cần set lại vì entity đã là existingEntity
            }
            else
            {
                // Create mode: đảm bảo Id = Guid.Empty (default của new Attribute())
                // Repository sẽ generate Id mới khi InsertOnSubmit
                entity.Id = Guid.Empty;
            }

            // Map properties
            entity.Name = dto.Name;
            entity.DataType = dto.DataType;
            entity.Description = dto.Description;

            // Note: AttributeValues không được set ở đây vì chúng được quản lý riêng
            // thông qua AttributeValueRepository để tránh vấn đề với EntitySet và DataContext

            return entity;
        }


        #endregion
    }
}
