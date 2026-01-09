using System;
using System.Collections.Generic;
using System.Linq;
using Dal.DataContext;
using DTO.MasterData.Company;

namespace Dal.DtoConverter.MasterData.Company
{

    /// <summary>
    /// Converter giữa Employee Entity và EmployeeDto
    /// </summary>
    public static class EmployeeConverters
    {
        /// <summary>
        /// Chuyển đổi từ Employee Entity sang EmployeeDto
        /// </summary>
        /// <param name="entity">Employee Entity</param>
        /// <param name="companyName">Tên công ty (tùy chọn, nếu đã có sẵn)</param>
        /// <param name="branchName">Tên chi nhánh (tùy chọn, nếu đã có sẵn)</param>
        /// <param name="departmentName">Tên phòng ban (tùy chọn, nếu đã có sẵn)</param>
        /// <param name="positionName">Tên chức vụ (tùy chọn, nếu đã có sẵn)</param>
        /// <returns>EmployeeDto</returns>
        public static EmployeeDto ToDto(this Employee entity, string companyName = null, string branchName = null, string departmentName = null, string positionName = null)
        {
            if (entity == null)
                return null;

            // QUAN TRỌNG: KHÔNG truy cập navigation properties (entity.Company, entity.CompanyBranch, entity.Department, entity.Position)
            // vì DataContext đã bị dispose sau khi repository method kết thúc
            // Chỉ sử dụng tham số truyền vào

            return new EmployeeDto
            {
                Id = entity.Id,
                CompanyId = entity.CompanyId,
                BranchId = entity.BranchId,
                DepartmentId = entity.DepartmentId,
                PositionId = entity.PositionId,
                EmployeeCode = entity.EmployeeCode,
                FullName = entity.FullName,
                Gender = entity.Gender,
                BirthDate = entity.BirthDate,
                Phone = entity.Phone,
                Email = entity.Email,
                HireDate = entity.HireDate,
                ResignDate = entity.ResignDate,
                AvatarPath = entity.AvatarPath,
                IsActive = entity.IsActive,
                Mobile = entity.Mobile,
                Fax = entity.Fax,
                LinkedIn = entity.LinkedIn,
                Skype = entity.Skype,
                WeChat = entity.WeChat,
                Notes = entity.Notes,
                CreatedDate = entity.CreatedDate,
                ModifiedDate = entity.ModifiedDate,
                AvatarFileName = entity.AvatarFileName,
                AvatarRelativePath = entity.AvatarRelativePath,
                AvatarFullPath = entity.AvatarFullPath,
                AvatarStorageType = entity.AvatarStorageType,
                AvatarFileSize = entity.AvatarFileSize,
                AvatarChecksum = entity.AvatarChecksum,
                AvatarThumbnailData = entity.AvatarThumbnailData?.ToArray(),

                // Navigation properties - sử dụng giá trị từ tham số truyền vào
                CompanyName = companyName,
                BranchName = branchName,
                DepartmentName = departmentName,
                PositionName = positionName
            };
        }

        /// <summary>
        /// Chuyển đổi danh sách Employee Entity sang danh sách EmployeeDto
        /// </summary>
        /// <param name="entities">Danh sách Employee Entity</param>
        /// <returns>Danh sách EmployeeDto</returns>
        public static IEnumerable<EmployeeDto> ToEmployeeDtos(this IEnumerable<Employee> entities)
        {
            if (entities == null) return [];

            return entities.Select(entity => entity.ToDto());
        }

        /// <summary>
        /// Chuyển đổi từ EmployeeDto sang Employee Entity
        /// </summary>
        /// <param name="dto">EmployeeDto</param>
        /// <param name="destination">Employee Entity đích (tùy chọn, cho update)</param>
        /// <returns>Employee Entity</returns>
        public static Employee ToEntity(this EmployeeDto dto, Employee destination = null)
        {
            if (dto == null) return null;
            var entity = destination ?? new Employee();
            if (dto.Id != Guid.Empty) entity.Id = dto.Id;
            entity.CompanyId = dto.CompanyId;
            entity.BranchId = dto.BranchId;
            entity.DepartmentId = dto.DepartmentId;
            entity.PositionId = dto.PositionId;
            entity.EmployeeCode = dto.EmployeeCode;
            entity.FullName = dto.FullName;
            entity.Gender = dto.Gender;
            entity.BirthDate = dto.BirthDate;
            entity.Phone = dto.Phone;
            entity.Email = dto.Email;
            entity.HireDate = dto.HireDate;
            entity.ResignDate = dto.ResignDate;
            entity.AvatarPath = dto.AvatarPath;
            entity.IsActive = dto.IsActive;
            entity.Mobile = dto.Mobile;
            entity.Fax = dto.Fax;
            entity.LinkedIn = dto.LinkedIn;
            entity.Skype = dto.Skype;
            entity.WeChat = dto.WeChat;
            entity.Notes = dto.Notes;
            entity.CreatedDate = dto.CreatedDate;
            entity.ModifiedDate = dto.ModifiedDate;
            entity.AvatarFileName = dto.AvatarFileName;
            entity.AvatarRelativePath = dto.AvatarRelativePath;
            entity.AvatarFullPath = dto.AvatarFullPath;
            entity.AvatarStorageType = dto.AvatarStorageType;
            entity.AvatarFileSize = dto.AvatarFileSize;
            entity.AvatarChecksum = dto.AvatarChecksum;
            if (dto.AvatarThumbnailData != null)
            {
                entity.AvatarThumbnailData = new System.Data.Linq.Binary(dto.AvatarThumbnailData);
            }
            return entity;
        }
    }

}
