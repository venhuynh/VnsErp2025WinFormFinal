using MasterData.Company.Dto;

namespace MasterData.Company.Converters
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
        /// <returns>PositionDto</returns>
        public static PositionDto ToDto(this Dal.DataContext.Position entity)
        {
            if (entity == null)
                return null;

            return new PositionDto
            {
                Id = entity.Id,
                CompanyId = entity.CompanyId,
                PositionCode = entity.PositionCode,
                PositionName = entity.PositionName,
                Description = entity.Description,
                IsManagerLevel = entity.IsManagerLevel,
                IsActive = entity.IsActive,
                CompanyName = entity.Company?.CompanyName // Lấy tên công ty từ navigation property
            };
        }

        /// <summary>
        /// Chuyển đổi PositionDto sang Position Entity
        /// </summary>
        /// <param name="dto">PositionDto</param>
        /// <returns>Position entity</returns>
        public static Dal.DataContext.Position ToEntity(this PositionDto dto)
        {
            if (dto == null)
                return null;

            return new Dal.DataContext.Position
            {
                Id = dto.Id,
                CompanyId = dto.CompanyId,
                PositionCode = dto.PositionCode,
                PositionName = dto.PositionName,
                Description = dto.Description,
                IsManagerLevel = dto.IsManagerLevel,
                IsActive = dto.IsActive
            };
        }
    }
}
