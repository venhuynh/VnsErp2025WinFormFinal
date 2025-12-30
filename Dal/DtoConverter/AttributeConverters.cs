using DTO.MasterData.ProductService;
using System;
using System.Collections.Generic;
using System.Linq;
using Attribute = Dal.DataContext.Attribute;

namespace Dal.DtoConverter
{
    /// <summary>
    /// Converter giữa Attribute/AttributeValue/VariantAttribute (Dal) và DTO tương ứng
    /// </summary>
    public static class AttributeConverters
    {
        

        #region AttributeDto -> Attribute

        /// <summary>
        /// Chuyển đổi AttributeDto sang Attribute Entity
        /// </summary>
        /// <param name="dto">AttributeDto</param>
        /// <param name="existingEntity">Entity hiện tại (cho update)</param>
        /// <returns>Attribute Entity</returns>
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
                entity.Id = Guid.Empty;
            }

            entity.Name = dto.Name;
            entity.DataType = dto.DataType;
            entity.Description = dto.Description;

            // Note: AttributeValues không được set ở đây vì chúng được quản lý riêng
            // thông qua AttributeValueRepository

            return entity;
        }

        #endregion
    }
}
