using Dal.DataContext;
using DTO.MasterData.Company;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dal.DtoConverter
{
    /// <summary>
    /// Converter giữa Department Entity, DepartmentDto và DepartmentLookupDto
    /// Gộp chung các converter để tránh trùng lặp code
    /// </summary>
    public static class DepartmentConverters
    {
        #region ToDto (DepartmentDto)

        /// <summary>
        /// Chuyển đổi từ Department Entity sang DepartmentDto
        /// </summary>
        /// <param name="entity">Department Entity</param>
        /// <param name="companyName">Tên công ty (tùy chọn, nếu đã có sẵn)</param>
        /// <param name="branchName">Tên chi nhánh (tùy chọn, nếu đã có sẵn)</param>
        /// <param name="parentDepartmentName">Tên phòng ban cha (tùy chọn, nếu đã có sẵn)</param>
        /// <param name="departmentDict">Dictionary chứa tất cả Department entities để tính FullPath (optional, chỉ cần khi tính full path)</param>
        /// <returns>DepartmentDto</returns>
        public static DepartmentDto ToDto(this Department entity, string companyName = null, string branchName = null, string parentDepartmentName = null, Dictionary<Guid, Department> departmentDict = null)
        {
            if (entity == null)
                return null;

            // QUAN TRỌNG: KHÔNG truy cập navigation properties (entity.CompanyBranch, entity.Department1)
            // vì DataContext đã bị dispose sau khi repository method kết thúc
            // Chỉ sử dụng tham số truyền vào hoặc departmentDict

            // BranchName: chỉ dùng tham số truyền vào, không truy cập entity.CompanyBranch
            var finalBranchName = branchName;

            // ParentDepartmentName: ưu tiên tham số, sau đó dùng departmentDict, cuối cùng mới dùng default
            string finalParentDepartmentName = parentDepartmentName;
            if (string.IsNullOrEmpty(finalParentDepartmentName) && entity.ParentId.HasValue)
            {
                // Thử lấy từ departmentDict nếu có
                if (departmentDict != null && departmentDict.ContainsKey(entity.ParentId.Value))
                {
                    finalParentDepartmentName = departmentDict[entity.ParentId.Value].DepartmentName;
                }
                else
                {
                    finalParentDepartmentName = "Không xác định";
                }
            }
            else if (string.IsNullOrEmpty(finalParentDepartmentName))
            {
                finalParentDepartmentName = null; // Không có parent
            }

            var dto = new DepartmentDto
            {
                Id = entity.Id,
                CompanyId = entity.CompanyId,
                BranchId = entity.BranchId,
                DepartmentCode = entity.DepartmentCode,
                DepartmentName = entity.DepartmentName,
                ParentId = entity.ParentId,
                Description = entity.Description,
                IsActive = entity.IsActive,

                // Navigation properties - sử dụng giá trị mặc định để tránh DataContext disposed
                BranchName = finalBranchName,
                ParentDepartmentName = finalParentDepartmentName,
                EmployeeCount = 0, // Sẽ được tính toán riêng nếu cần
                SubDepartmentCount = 0 // Sẽ được tính toán riêng nếu cần
            };

            // Tính FullPath: chỉ dùng departmentDict nếu có, nếu không chỉ dùng DepartmentName
            // KHÔNG tính từ navigation properties vì sẽ gây lỗi "Cannot access a disposed object"
            if (departmentDict != null && departmentDict.ContainsKey(entity.Id))
            {
                dto.FullPath = CalculateDepartmentFullPath(entity, departmentDict);
            }
            else
            {
                // Không có departmentDict, chỉ dùng DepartmentName
                // KHÔNG cố tính từ navigation properties vì DataContext đã bị dispose
                dto.FullPath = entity.DepartmentName;
            }

            return dto;
        }

        /// <summary>
        /// Chuyển đổi danh sách Department Entity sang danh sách DepartmentDto
        /// </summary>
        /// <param name="entities">Danh sách Department Entity</param>
        /// <param name="departmentDict">Dictionary chứa tất cả Department entities để tính FullPath (optional)</param>
        /// <returns>Danh sách DepartmentDto</returns>
        public static IEnumerable<DepartmentDto> ToDepartmentDtos(
            this IEnumerable<Department> entities,
            Dictionary<Guid, Department> departmentDict = null)
        {
            if (entities == null) return [];

            return entities.Select(entity => entity.ToDto(departmentDict: departmentDict));
        }

        #endregion

        #region ToLookupDto (DepartmentLookupDto)

        /// <summary>
        /// Chuyển đổi Department Entity sang DepartmentLookupDto
        /// DTO tối giản chỉ chứa thông tin cần thiết cho SearchLookUpEdit
        /// </summary>
        /// <param name="entity">Department Entity</param>
        /// <param name="departmentDict">Dictionary chứa tất cả Department entities để tính FullPath (optional)</param>
        /// <returns>DepartmentLookupDto</returns>
        public static DepartmentLookupDto ToLookupDto(this Department entity, Dictionary<Guid, Department> departmentDict = null)
        {
            if (entity == null) return null;

            var dto = new DepartmentLookupDto
            {
                Id = entity.Id,
                CompanyId = entity.CompanyId,
                BranchId = entity.BranchId,
                DepartmentCode = entity.DepartmentCode,
                DepartmentName = entity.DepartmentName,
                ParentId = entity.ParentId,
                IsActive = entity.IsActive
            };

            // Tính FullPath nếu có dictionary
            if (departmentDict != null && departmentDict.ContainsKey(entity.Id))
            {
                dto.FullPath = CalculateDepartmentFullPath(entity, departmentDict);
            }
            else
            {
                // Không có dictionary, chỉ dùng DepartmentName
                dto.FullPath = entity.DepartmentName;
            }

            return dto;
        }

        /// <summary>
        /// Chuyển đổi danh sách Department Entity sang danh sách DepartmentLookupDto
        /// </summary>
        /// <param name="entities">Danh sách Department Entity</param>
        /// <param name="departmentDict">Dictionary chứa tất cả Department entities để tính FullPath (optional)</param>
        /// <returns>Danh sách DepartmentLookupDto</returns>
        public static IEnumerable<DepartmentLookupDto> ToLookupDtos(
            this IEnumerable<Department> entities,
            Dictionary<Guid, Department> departmentDict = null)
        {
            if (entities == null) return [];

            return entities.Select(entity => entity.ToLookupDto(departmentDict));
        }

        #endregion

        #region ToEntity

        /// <summary>
        /// Chuyển đổi từ DepartmentDto sang Department Entity
        /// </summary>
        /// <param name="dto">DepartmentDto</param>
        /// <param name="destination">Department Entity đích (tùy chọn, cho update)</param>
        /// <returns>Department Entity</returns>
        public static Department ToEntity(this DepartmentDto dto, Department destination = null)
        {
            if (dto == null) return null;
            var entity = destination ?? new Department();
            if (dto.Id != Guid.Empty) entity.Id = dto.Id;
            entity.CompanyId = dto.CompanyId;
            entity.BranchId = dto.BranchId;
            entity.DepartmentCode = dto.DepartmentCode;
            entity.DepartmentName = dto.DepartmentName;
            entity.ParentId = dto.ParentId;
            entity.Description = dto.Description;
            entity.IsActive = dto.IsActive;
            return entity;
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Tính toán đường dẫn đầy đủ từ gốc đến department sử dụng departmentDict
        /// Phương thức dùng chung cho cả ToDto và ToLookupDto
        /// </summary>
        /// <param name="department">Department entity</param>
        /// <param name="departmentDict">Dictionary chứa tất cả Department entities</param>
        /// <returns>Đường dẫn đầy đủ (ví dụ: "Phòng ban A > Phòng ban A1")</returns>
        private static string CalculateDepartmentFullPath(Department department,
            Dictionary<Guid, Department> departmentDict)
        {
            if (department == null) return string.Empty;

            var pathParts = new List<string> { department.DepartmentName };
            var current = department;

            while (current.ParentId.HasValue && departmentDict.ContainsKey(current.ParentId.Value))
            {
                current = departmentDict[current.ParentId.Value];
                pathParts.Insert(0, current.DepartmentName);
                if (pathParts.Count > 10) break; // Tránh infinite loop
            }

            return string.Join(" > ", pathParts);
        }

        #endregion
    }
}
