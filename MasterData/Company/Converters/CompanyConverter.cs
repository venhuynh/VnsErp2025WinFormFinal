using System.Data.Linq;
using MasterData.Company.Dto;

namespace MasterData.Company.Converters
{
    /// <summary>
    /// Converter để chuyển đổi giữa Company Entity và CompanyDto
    /// </summary>
    public static class CompanyConverter
    {
        /// <summary>
        /// Chuyển đổi Company Entity sang CompanyDto
        /// </summary>
        /// <param name="entity">Company entity</param>
        /// <returns>CompanyDto</returns>
        public static CompanyDto ToDto(this Dal.DataContext.Company entity)
        {
            if (entity == null)
                return null;

            return new CompanyDto
            {
                Id = entity.Id,
                CompanyCode = entity.CompanyCode,
                CompanyName = entity.CompanyName,
                TaxCode = entity.TaxCode,
                Phone = entity.Phone,
                Email = entity.Email,
                Website = entity.Website,
                Address = entity.Address,
                Country = entity.Country,
                CreatedDate = entity.CreatedDate,
                UpdatedDate = entity.UpdatedDate,
                Logo = entity.Logo?.ToArray() // Convert Binary sang byte[]
            };
        }

        /// <summary>
        /// Chuyển đổi CompanyDto sang Company Entity
        /// </summary>
        /// <param name="dto">CompanyDto</param>
        /// <returns>Company entity</returns>
        public static Dal.DataContext.Company ToEntity(this CompanyDto dto)
        {
            if (dto == null)
                return null;

            return new Dal.DataContext.Company
            {
                Id = dto.Id,
                CompanyCode = dto.CompanyCode,
                CompanyName = dto.CompanyName,
                TaxCode = dto.TaxCode,
                Phone = dto.Phone,
                Email = dto.Email,
                Website = dto.Website,
                Address = dto.Address,
                Country = dto.Country,
                CreatedDate = dto.CreatedDate,
                UpdatedDate = dto.UpdatedDate,
                Logo = dto.Logo != null ? new Binary(dto.Logo) : null
            };
        }
    }
}
