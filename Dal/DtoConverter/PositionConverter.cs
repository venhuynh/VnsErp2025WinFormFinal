using DTO.MasterData.Company;

namespace Dal.DtoConverter
{
    /// <summary>
    /// Converter để chuyển đổi giữa Position Entity và PositionDto
    /// </summary>
    public static class PositionConverter
    {
        /// <summary>
        /// Chuyển đổi Position Entity sang PositionDto
        /// </summary>
        /// <param name="entity">Position entity</param>
        /// <param name="companyName">Tên công ty (tùy chọn, nếu đã có sẵn)</param>
        /// <returns>PositionDto</returns>
        public static PositionDto ToDto(this Dal.DataContext.Position entity, string companyName = null)
        {
            if (entity == null)
                return null;

            // QUAN TRỌNG: KHÔNG truy cập navigation properties (entity.Company)
            // vì DataContext đã bị dispose sau khi repository method kết thúc
            // Chỉ sử dụng tham số truyền vào

            return new PositionDto
            {
                Id = entity.Id,
                CompanyId = entity.CompanyId,
                PositionCode = entity.PositionCode,
                PositionName = entity.PositionName,
                Description = entity.Description,
                IsManagerLevel = entity.IsManagerLevel,
                IsActive = entity.IsActive,
                CompanyName = companyName // Sử dụng tham số truyền vào thay vì navigation property
            };
        }

        /// <summary>
        /// Chuyển đổi PositionDto sang Position Entity
        /// </summary>
        /// <param name="dto">PositionDto</param>
        /// <param name="destination">Position Entity đích (tùy chọn, cho update)</param>
        /// <returns>Position entity</returns>
        public static Dal.DataContext.Position ToEntity(this PositionDto dto, Dal.DataContext.Position destination = null)
        {
            if (dto == null)
                return null;

            var entity = destination ?? new Dal.DataContext.Position();
            
            // Chỉ set ID nếu là entity mới
            if (destination == null && dto.Id != System.Guid.Empty)
            {
                entity.Id = dto.Id;
            }

            entity.CompanyId = dto.CompanyId;
            entity.PositionCode = dto.PositionCode;
            entity.PositionName = dto.PositionName;
            entity.Description = dto.Description;
            entity.IsManagerLevel = dto.IsManagerLevel;
            entity.IsActive = dto.IsActive;

            return entity;
        }
    }
}
