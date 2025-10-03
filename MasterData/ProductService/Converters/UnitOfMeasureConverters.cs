using System;
using System.Collections.Generic;
using System.Linq;
using Dal.DataContext;
using MasterData.ProductService.Dto;

namespace MasterData.ProductService.Converters
{
    /// <summary>
    /// Converter giữa UnitOfMeasure entity và UnitOfMeasureDto
    /// </summary>
    public static class UnitOfMeasureConverters
    {
        #region Entity to DTO

        /// <summary>
        /// Chuyển đổi UnitOfMeasure entity thành UnitOfMeasureDto
        /// </summary>
        /// <param name="entity">UnitOfMeasure entity</param>
        /// <returns>UnitOfMeasureDto</returns>
        public static UnitOfMeasureDto ToDto(this UnitOfMeasure entity)
        {
            if (entity == null) return null;

            return new UnitOfMeasureDto
            {
                Id = entity.Id,
                Code = entity.Code,
                Name = entity.Name,
                Description = entity.Description,
                IsActive = entity.IsActive
            };
        }

        /// <summary>
        /// Chuyển đổi danh sách UnitOfMeasure entities thành danh sách UnitOfMeasureDto
        /// </summary>
        /// <param name="entities">Danh sách UnitOfMeasure entities</param>
        /// <returns>Danh sách UnitOfMeasureDto</returns>
        public static List<UnitOfMeasureDto> ToDtoList(this IEnumerable<UnitOfMeasure> entities)
        {
            if (entities == null) return new List<UnitOfMeasureDto>();

            return entities.Select(entity => entity.ToDto()).ToList();
        }

        #endregion

        #region DTO to Entity

        /// <summary>
        /// Chuyển đổi UnitOfMeasureDto thành UnitOfMeasure entity
        /// </summary>
        /// <param name="dto">UnitOfMeasureDto</param>
        /// <returns>UnitOfMeasure entity</returns>
        public static UnitOfMeasure ToEntity(this UnitOfMeasureDto dto)
        {
            if (dto == null) return null;

            return new UnitOfMeasure
            {
                Id = dto.Id,
                Code = dto.Code,
                Name = dto.Name,
                Description = dto.Description,
                IsActive = dto.IsActive
            };
        }

        /// <summary>
        /// Chuyển đổi danh sách UnitOfMeasureDto thành danh sách UnitOfMeasure entities
        /// </summary>
        /// <param name="dtos">Danh sách UnitOfMeasureDto</param>
        /// <returns>Danh sách UnitOfMeasure entities</returns>
        public static List<UnitOfMeasure> ToEntityList(this IEnumerable<UnitOfMeasureDto> dtos)
        {
            if (dtos == null) return new List<UnitOfMeasure>();

            return dtos.Select(dto => dto.ToEntity()).ToList();
        }

        #endregion

        #region Update Entity from DTO

        /// <summary>
        /// Cập nhật UnitOfMeasure entity từ UnitOfMeasureDto
        /// </summary>
        /// <param name="entity">UnitOfMeasure entity cần cập nhật</param>
        /// <param name="dto">UnitOfMeasureDto chứa dữ liệu mới</param>
        public static void UpdateFromDto(this UnitOfMeasure entity, UnitOfMeasureDto dto)
        {
            if (entity == null || dto == null) return;

            entity.Code = dto.Code;
            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.IsActive = dto.IsActive;
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Tạo UnitOfMeasureDto mới với thông tin cơ bản
        /// </summary>
        /// <param name="code">Mã đơn vị</param>
        /// <param name="name">Tên đơn vị</param>
        /// <param name="description">Mô tả</param>
        /// <param name="isActive">Trạng thái hoạt động</param>
        /// <returns>UnitOfMeasureDto mới</returns>
        public static UnitOfMeasureDto CreateNew(
            string code,
            string name,
            string description = null,
            bool isActive = true)
        {
            return new UnitOfMeasureDto(code, name, description, isActive);
        }

        /// <summary>
        /// Tạo danh sách UnitOfMeasureDto từ template
        /// </summary>
        /// <param name="units">Danh sách thông tin đơn vị (Code, Name, Description)</param>
        /// <returns>Danh sách UnitOfMeasureDto</returns>
        public static List<UnitOfMeasureDto> CreateBulk(
            IEnumerable<(string code, string name, string description)> units)
        {
            if (units == null) return new List<UnitOfMeasureDto>();

            return units.Select(unit => new UnitOfMeasureDto(
                unit.code,
                unit.name,
                unit.description,
                true
            )).ToList();
        }

        /// <summary>
        /// Tạo UnitOfMeasureDto từ UnitOfMeasure entity với thông tin đầy đủ
        /// </summary>
        /// <param name="entity">UnitOfMeasure entity</param>
        /// <returns>UnitOfMeasureDto với thông tin đầy đủ</returns>
        public static UnitOfMeasureDto ToFullDto(this UnitOfMeasure entity)
        {
            if (entity == null) return null;

            return new UnitOfMeasureDto
            {
                Id = entity.Id,
                Code = entity.Code,
                Name = entity.Name,
                Description = entity.Description,
                IsActive = entity.IsActive
            };
        }

        /// <summary>
        /// Tạo danh sách UnitOfMeasureDto từ danh sách UnitOfMeasure entities với thông tin đầy đủ
        /// </summary>
        /// <param name="entities">Danh sách UnitOfMeasure entities</param>
        /// <returns>Danh sách UnitOfMeasureDto với thông tin đầy đủ</returns>
        public static List<UnitOfMeasureDto> ToFullDtoList(this IEnumerable<UnitOfMeasure> entities)
        {
            if (entities == null) return new List<UnitOfMeasureDto>();

            return entities.Select(entity => entity.ToFullDto()).ToList();
        }

        #endregion

        #region Validation Helpers

        /// <summary>
        /// Kiểm tra tính hợp lệ của UnitOfMeasureDto
        /// </summary>
        /// <param name="dto">UnitOfMeasureDto cần kiểm tra</param>
        /// <returns>True nếu hợp lệ</returns>
        public static bool IsValidDto(this UnitOfMeasureDto dto)
        {
            return dto?.IsValid() == true;
        }

        /// <summary>
        /// Lấy danh sách lỗi validation của UnitOfMeasureDto
        /// </summary>
        /// <param name="dto">UnitOfMeasureDto cần kiểm tra</param>
        /// <returns>Danh sách lỗi</returns>
        public static List<string> GetValidationErrors(this UnitOfMeasureDto dto)
        {
            return dto?.GetValidationErrors() ?? new List<string> { "DTO không được null" };
        }

        #endregion

        #region Search and Filter Helpers

        /// <summary>
        /// Tìm UnitOfMeasureDto theo mã
        /// </summary>
        /// <param name="dtos">Danh sách UnitOfMeasureDto</param>
        /// <param name="code">Mã cần tìm</param>
        /// <returns>UnitOfMeasureDto tìm được hoặc null</returns>
        public static UnitOfMeasureDto FindByCode(this IEnumerable<UnitOfMeasureDto> dtos, string code)
        {
            if (dtos == null || string.IsNullOrWhiteSpace(code)) return null;

            return dtos.FirstOrDefault(dto => 
                string.Equals(dto.Code, code, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Tìm UnitOfMeasureDto theo tên
        /// </summary>
        /// <param name="dtos">Danh sách UnitOfMeasureDto</param>
        /// <param name="name">Tên cần tìm</param>
        /// <returns>UnitOfMeasureDto tìm được hoặc null</returns>
        public static UnitOfMeasureDto FindByName(this IEnumerable<UnitOfMeasureDto> dtos, string name)
        {
            if (dtos == null || string.IsNullOrWhiteSpace(name)) return null;

            return dtos.FirstOrDefault(dto => 
                string.Equals(dto.Name, name, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Lọc UnitOfMeasureDto theo trạng thái hoạt động
        /// </summary>
        /// <param name="dtos">Danh sách UnitOfMeasureDto</param>
        /// <param name="isActive">Trạng thái cần lọc</param>
        /// <returns>Danh sách UnitOfMeasureDto đã lọc</returns>
        public static List<UnitOfMeasureDto> FilterByStatus(this IEnumerable<UnitOfMeasureDto> dtos, bool isActive)
        {
            if (dtos == null) return new List<UnitOfMeasureDto>();

            return dtos.Where(dto => dto.IsActive == isActive).ToList();
        }

        /// <summary>
        /// Tìm kiếm UnitOfMeasureDto theo từ khóa
        /// </summary>
        /// <param name="dtos">Danh sách UnitOfMeasureDto</param>
        /// <param name="keyword">Từ khóa tìm kiếm</param>
        /// <returns>Danh sách UnitOfMeasureDto tìm được</returns>
        public static List<UnitOfMeasureDto> Search(this IEnumerable<UnitOfMeasureDto> dtos, string keyword)
        {
            if (dtos == null) return new List<UnitOfMeasureDto>();
            if (string.IsNullOrWhiteSpace(keyword)) return dtos.ToList();

            var lowerKeyword = keyword.ToLower();
            return dtos.Where(dto => 
                (dto.Code?.ToLower().Contains(lowerKeyword) == true) ||
                (dto.Name?.ToLower().Contains(lowerKeyword) == true) ||
                (dto.Description?.ToLower().Contains(lowerKeyword) == true)
            ).ToList();
        }

        #endregion
    }
}
