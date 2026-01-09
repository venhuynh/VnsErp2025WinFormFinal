using System;
using Dal.DataContext;
using DTO.MasterData.Company;

namespace Dal.DtoConverter.MasterData.Company
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
        /// Map tất cả các thuộc tính từ CompanyBranch entity sang CompanyBranchDto.
        /// </summary>
        /// <param name="entity">Entity chi nhánh công ty</param>
        /// <returns>DTO chi nhánh công ty</returns>
        public static CompanyBranchDto ToDto(this CompanyBranch entity)
        {
            if (entity == null)
                return null;

            return new CompanyBranchDto
            {
                // Primary key và foreign key
                Id = entity.Id,
                CompanyId = entity.CompanyId,

                // Thông tin cơ bản
                BranchCode = entity.BranchCode ?? string.Empty,
                BranchName = entity.BranchName ?? string.Empty,
                Address = entity.Address ?? string.Empty,
                Phone = entity.Phone ?? string.Empty,
                Email = entity.Email ?? string.Empty,
                ManagerName = entity.ManagerName ?? string.Empty,
                IsActive = entity.IsActive,

                // Date fields - Entity không có CreatedDate/ModifiedDate trong database schema
                // Sử dụng giá trị mặc định (DateTime.Now) khi convert từ entity
                // Lưu ý: Nếu cần lấy từ database, cần thêm các cột này vào bảng CompanyBranch
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now

                // Computed properties (không cần map, tự động tính toán):
                // - FullAddress: computed từ Address, Phone, Email
                // - ThongTinHtml: computed từ các thuộc tính khác
            };
        }

        /// <summary>
        /// Chuyển đổi từ DTO sang Entity.
        /// Map tất cả các thuộc tính từ CompanyBranchDto sang CompanyBranch entity.
        /// </summary>
        /// <param name="dto">DTO chi nhánh công ty</param>
        /// <returns>Entity chi nhánh công ty</returns>
        public static CompanyBranch ToEntity(this CompanyBranchDto dto)
        {
            if (dto == null)
                return null;

            return new CompanyBranch
            {
                // Primary key và foreign key
                Id = dto.Id,
                CompanyId = dto.CompanyId,

                // Thông tin cơ bản
                BranchCode = dto.BranchCode ?? string.Empty,
                BranchName = dto.BranchName ?? string.Empty,
                Address = dto.Address ?? string.Empty,
                Phone = dto.Phone ?? string.Empty,
                Email = dto.Email ?? string.Empty,
                ManagerName = dto.ManagerName ?? string.Empty,
                IsActive = dto.IsActive

                // Lưu ý: Entity không có CreatedDate/ModifiedDate properties trong database schema
                // Các thuộc tính này chỉ có trong DTO để tracking, không lưu vào database
                // Nếu cần lưu CreatedDate/ModifiedDate, cần thêm các cột này vào bảng CompanyBranch
            };
        }

        /// <summary>
        /// Chuyển đổi từ DTO sang Entity với thông tin cập nhật.
        /// Map tất cả các thuộc tính từ CompanyBranchDto sang CompanyBranch entity hiện có.
        /// Phương thức này dùng cho update operation.
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
            // Lưu ý: Không cập nhật Id (primary key không thay đổi)
            existingEntity.CompanyId = dto.CompanyId;
            existingEntity.BranchCode = dto.BranchCode ?? string.Empty;
            existingEntity.BranchName = dto.BranchName ?? string.Empty;
            existingEntity.Address = dto.Address ?? string.Empty;
            existingEntity.Phone = dto.Phone ?? string.Empty;
            existingEntity.Email = dto.Email ?? string.Empty;
            existingEntity.ManagerName = dto.ManagerName ?? string.Empty;
            existingEntity.IsActive = dto.IsActive;

            // Lưu ý: Entity không có CreatedDate/ModifiedDate properties trong database schema
            // Các thuộc tính này chỉ có trong DTO để tracking, không lưu vào database

            return existingEntity;
        }

        #endregion
    }
}
