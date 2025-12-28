using Dal.DataContext;
using DTO.MasterData.ProductService;
using System;
using System.Collections.Generic;

namespace Dal.DtoConverter;


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
    /// <param name="productVariantCount">Số lượng ProductVariant sử dụng đơn vị này (để tránh DataContext disposed errors)</param>
    /// <returns>UnitOfMeasureDto</returns>
    public static UnitOfMeasureDto ToDto(this UnitOfMeasure entity, int productVariantCount = 0)
    {
        if (entity == null) return null;

        return new UnitOfMeasureDto
        {
            Id = entity.Id,
            Code = entity.Code,
            Name = entity.Name,
            Description = entity.Description,
            IsActive = entity.IsActive,
            ProductVariantCount = productVariantCount
        };
    }

    /// <summary>
    /// Chuyển đổi UnitOfMeasure entity thành UnitOfMeasureDto với đếm số lượng ProductVariant
    /// </summary>
    /// <param name="entity">UnitOfMeasure entity</param>
    /// <param name="context">DataContext để đếm ProductVariant</param>
    /// <returns>UnitOfMeasureDto</returns>
    public static UnitOfMeasureDto ToDto(this UnitOfMeasure entity, VnsErp2025DataContext context)
    {
        if (entity == null) return null;

        var dto = new UnitOfMeasureDto
        {
            Id = entity.Id,
            Code = entity.Code,
            Name = entity.Name,
            Description = entity.Description,
            IsActive = entity.IsActive
        };

        // Đếm số lượng ProductVariant sử dụng đơn vị tính này
        try
        {
            if (context != null && entity.Id != Guid.Empty)
            {
                dto.ProductVariantCount = context.ProductVariants.Count(x => x.UnitId == entity.Id);
            }
            else
            {
                dto.ProductVariantCount = 0;
            }
        }
        catch
        {
            // Nếu có lỗi khi đếm, set về 0
            dto.ProductVariantCount = 0;
        }

        return dto;
    }

    /// <summary>
    /// Chuyển đổi danh sách UnitOfMeasure entities thành danh sách UnitOfMeasureDto
    /// </summary>
    /// <param name="entities">Danh sách UnitOfMeasure entities</param>
    /// <param name="variantCountDict">Dictionary chứa số lượng ProductVariant theo UnitId (key = UnitId, value = Count)</param>
    /// <returns>Danh sách UnitOfMeasureDto</returns>
    public static List<UnitOfMeasureDto> ToDtos(this IEnumerable<UnitOfMeasure> entities, Dictionary<Guid, int> variantCountDict = null)
    {
        if (entities == null) return new List<UnitOfMeasureDto>();

        return entities.Select(entity =>
        {
            var count = variantCountDict != null && variantCountDict.ContainsKey(entity.Id)
                ? variantCountDict[entity.Id]
                : 0;
            return entity.ToDto(count);
        }).ToList();
    }

    /// <summary>
    /// Chuyển đổi danh sách UnitOfMeasure entities thành danh sách UnitOfMeasureDto với đếm số lượng ProductVariant
    /// Tối ưu: Đếm tất cả trong một query thay vì đếm từng entity
    /// </summary>
    /// <param name="entities">Danh sách UnitOfMeasure entities</param>
    /// <param name="context">DataContext để đếm ProductVariant</param>
    /// <returns>Danh sách UnitOfMeasureDto</returns>
    public static List<UnitOfMeasureDto> ToDtoList(this IEnumerable<UnitOfMeasure> entities, VnsErp2025DataContext context)
    {
        if (entities == null) return new List<UnitOfMeasureDto>();

        var entityList = entities.ToList();
        if (entityList.Count == 0) return new List<UnitOfMeasureDto>();

        // Tạo dictionary để lưu số lượng ProductVariant theo UnitOfMeasureId
        var variantCountDict = new Dictionary<Guid, int>();

        try
        {
            if (context != null)
            {
                // Lấy danh sách UnitOfMeasureId
                var unitIds = entityList.Where(e => e.Id != Guid.Empty).Select(e => e.Id).ToList();

                if (unitIds.Any())
                {
                    // Đếm tất cả ProductVariant theo UnitId trong một query
                    var counts = context.ProductVariants
                        .Where(pv => unitIds.Contains(pv.UnitId))
                        .GroupBy(pv => pv.UnitId)
                        .Select(g => new { UnitId = g.Key, Count = g.Count() })
                        .ToList();

                    // Tạo dictionary để lookup nhanh
                    foreach (var count in counts)
                    {
                        variantCountDict[count.UnitId] = count.Count;
                    }
                }
            }
        }
        catch
        {
            // Nếu có lỗi, tất cả sẽ có count = 0
        }

        // Convert entities sang DTOs với số lượng đã đếm
        return entityList.Select(entity =>
        {
            var dto = new UnitOfMeasureDto
            {
                Id = entity.Id,
                Code = entity.Code,
                Name = entity.Name,
                Description = entity.Description,
                IsActive = entity.IsActive,
                ProductVariantCount = variantCountDict.ContainsKey(entity.Id) ? variantCountDict[entity.Id] : 0
            };
            return dto;
        }).ToList();
    }

    #endregion

    #region DTO to Entity

    /// <summary>
    /// Chuyển đổi UnitOfMeasureDto thành UnitOfMeasure entity
    /// </summary>
    /// <param name="dto">UnitOfMeasureDto</param>
    /// <param name="destination">Entity đích để cập nhật (nếu null thì tạo mới)</param>
    /// <returns>UnitOfMeasure entity</returns>
    public static UnitOfMeasure ToEntity(this UnitOfMeasureDto dto, UnitOfMeasure destination = null)
    {
        if (dto == null) return null;

        if (destination == null)
        {
            // Tạo mới
            return new UnitOfMeasure
            {
                Id = dto.Id == Guid.Empty ? Guid.NewGuid() : dto.Id,
                Code = dto.Code,
                Name = dto.Name,
                Description = dto.Description,
                IsActive = dto.IsActive
            };
        }
        else
        {
            // Cập nhật
            destination.Code = dto.Code;
            destination.Name = dto.Name;
            destination.Description = dto.Description;
            destination.IsActive = dto.IsActive;
            return destination;
        }
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
            unit.description
        )).ToList();
    }

    /// <summary>
    /// Tạo UnitOfMeasureDto từ UnitOfMeasure entity với thông tin đầy đủ (bao gồm đếm ProductVariant)
    /// </summary>
    /// <param name="entity">UnitOfMeasure entity</param>
    /// <param name="context">DataContext để đếm ProductVariant (optional)</param>
    /// <returns>UnitOfMeasureDto với thông tin đầy đủ</returns>
    public static UnitOfMeasureDto ToFullDto(this UnitOfMeasure entity, VnsErp2025DataContext context = null)
    {
        if (entity == null) return null;

        if (context != null)
        {
            return entity.ToDto(context);
        }

        return new UnitOfMeasureDto
        {
            Id = entity.Id,
            Code = entity.Code,
            Name = entity.Name,
            Description = entity.Description,
            IsActive = entity.IsActive,
            ProductVariantCount = 0 // Mặc định = 0 nếu không có context
        };
    }

    /// <summary>
    /// Tạo danh sách UnitOfMeasureDto từ danh sách UnitOfMeasure entities với thông tin đầy đủ (bao gồm đếm ProductVariant)
    /// </summary>
    /// <param name="entities">Danh sách UnitOfMeasure entities</param>
    /// <param name="context">DataContext để đếm ProductVariant (optional)</param>
    /// <returns>Danh sách UnitOfMeasureDto với thông tin đầy đủ</returns>
    public static List<UnitOfMeasureDto> ToFullDtoList(this IEnumerable<UnitOfMeasure> entities, VnsErp2025DataContext context = null)
    {
        if (entities == null) return new List<UnitOfMeasureDto>();

        if (context != null)
        {
            return entities.ToDtoList(context);
        }

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