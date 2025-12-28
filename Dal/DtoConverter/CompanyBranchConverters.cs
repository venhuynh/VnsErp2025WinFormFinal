using Dal.DataContext;
using DTO.MasterData.Company;
using System;

namespace Dal.DtoConverter
{

    /// <summary>
    /// Converters giữa Entity và DTO cho chi nhánh công ty.
    /// Cung cấp các phương thức chuyển đổi dữ liệu giữa các layer.
    /// </summary>
    public static class CompanyBranchConverters
    {
        #region ========== CONVERTERS ==========

        /// <summary>
        /// Chuyển đổi từ Entity sang DTO.
        /// </summary>
        /// <param name="entity">Entity chi nhánh công ty</param>
        /// <returns>DTO chi nhánh công ty</returns>
        public static CompanyBranchDto ToDto(this CompanyBranch entity)
        {
            if (entity == null)
                return null;

            return new CompanyBranchDto
            {
                Id = entity.Id,
                CompanyId = entity.CompanyId,
                BranchCode = entity.BranchCode,
                BranchName = entity.BranchName,
                Address = entity.Address,
                Phone = entity.Phone,
                Email = entity.Email,
                ManagerName = entity.ManagerName,
                IsActive = entity.IsActive,
                // Entity không có CreatedDate/ModifiedDate, sử dụng giá trị mặc định
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
        }

        /// <summary>
        /// Chuyển đổi từ DTO sang Entity.
        /// </summary>
        /// <param name="dto">DTO chi nhánh công ty</param>
        /// <returns>Entity chi nhánh công ty</returns>
        public static CompanyBranch ToEntity(this CompanyBranchDto dto)
        {
            if (dto == null)
                return null;

            return new CompanyBranch
            {
                Id = dto.Id,
                CompanyId = dto.CompanyId,
                BranchCode = dto.BranchCode,
                BranchName = dto.BranchName,
                Address = dto.Address,
                Phone = dto.Phone,
                Email = dto.Email,
                ManagerName = dto.ManagerName,
                IsActive = dto.IsActive
                // Entity không có CreatedDate/ModifiedDate properties
            };
        }

        /// <summary>
        /// Chuyển đổi từ DTO sang Entity với thông tin cập nhật.
        /// </summary>
        /// <param name="dto">DTO chi nhánh công ty</param>
        /// <param name="existingEntity">Entity hiện tại (để giữ nguyên thông tin cũ)</param>
        /// <returns>Entity chi nhánh công ty đã cập nhật</returns>
        public static CompanyBranch ToEntity(this CompanyBranchDto dto, CompanyBranch existingEntity)
        {
            if (dto == null)
                return null;

            if (existingEntity == null)
                return dto.ToEntity();

            // Cập nhật thông tin từ DTO
            existingEntity.CompanyId = dto.CompanyId;
            existingEntity.BranchCode = dto.BranchCode;
            existingEntity.BranchName = dto.BranchName;
            existingEntity.Address = dto.Address;
            existingEntity.Phone = dto.Phone;
            existingEntity.Email = dto.Email;
            existingEntity.ManagerName = dto.ManagerName;
            existingEntity.IsActive = dto.IsActive;
            // Entity không có ModifiedDate property

            return existingEntity;
        }

        #endregion
    }
}
