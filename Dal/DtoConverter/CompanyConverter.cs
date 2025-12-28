using DTO.MasterData.Company;
using System;
using System.Data.Linq;

namespace Dal.DtoConverter
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
                Logo = entity.Logo?.ToArray(), // Convert Binary sang byte[]
                                               // Logo metadata fields
                LogoFileName = entity.LogoFileName,
                LogoRelativePath = entity.LogoRelativePath,
                LogoFullPath = entity.LogoFullPath,
                LogoStorageType = entity.LogoStorageType,
                LogoFileSize = entity.LogoFileSize,
                LogoChecksum = entity.LogoChecksum
            };
        }

        /// <summary>
        /// Chuyển đổi CompanyDto sang Company Entity
        /// </summary>
        /// <param name="dto">CompanyDto</param>
        /// <param name="existingEntity">Entity hiện tại (cho update)</param>
        /// <returns>Company entity</returns>
        public static Dal.DataContext.Company ToEntity(this CompanyDto dto, Dal.DataContext.Company existingEntity = null)
        {
            if (dto == null)
                return null;

            var entity = existingEntity ?? new Dal.DataContext.Company();

            // Chỉ set ID nếu là entity mới
            if (existingEntity == null && dto.Id != Guid.Empty)
            {
                entity.Id = dto.Id;
            }

            entity.CompanyCode = dto.CompanyCode;
            entity.CompanyName = dto.CompanyName;
            entity.TaxCode = dto.TaxCode;
            entity.Phone = dto.Phone;
            entity.Email = dto.Email;
            entity.Website = dto.Website;
            entity.Address = dto.Address;
            entity.Country = dto.Country;
            entity.CreatedDate = dto.CreatedDate;
            entity.UpdatedDate = dto.UpdatedDate;

            // Copy Logo (binary)
            if (dto.Logo != null && dto.Logo.Length > 0)
            {
                entity.Logo = new Binary(dto.Logo);
            }
            else
            {
                entity.Logo = null;
            }

            // Copy Logo fields (metadata only)
            entity.LogoFileName = dto.LogoFileName;
            entity.LogoRelativePath = dto.LogoRelativePath;
            entity.LogoFullPath = dto.LogoFullPath;
            entity.LogoStorageType = dto.LogoStorageType;
            entity.LogoFileSize = dto.LogoFileSize;
            entity.LogoChecksum = dto.LogoChecksum;

            return entity;
        }
    }
}
