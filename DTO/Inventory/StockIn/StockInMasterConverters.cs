using System;
using System.Collections.Generic;
using System.Linq;
using Dal.DataContext;
using DTO.Inventory.StockIn;

namespace DTO.Inventory.StockIn;

/// <summary>
/// Converters giữa Entity StockInOutMaster và DTO StockInMasterDto.
/// Cung cấp các phương thức chuyển đổi dữ liệu giữa các layer.
/// </summary>
public static class StockInMasterConverters
{
    #region ========== CONVERTERS ==========

    /// <summary>
    /// Chuyển đổi từ Entity StockInOutMaster sang DTO StockInMasterDto.
    /// </summary>
    /// <param name="entity">Entity StockInOutMaster</param>
    /// <returns>DTO StockInMasterDto</returns>
    public static StockInMasterDto ToDto(this StockInOutMaster entity)
    {
        if (entity == null)
            return null;

        return new StockInMasterDto
        {
            // Thông tin cơ bản
            Id = entity.Id,
            StockInNumber = entity.VocherNumber,
            StockInDate = entity.StockInOutDate,
            LoaiNhapKho = (LoaiNhapKhoEnum)entity.StockInOutType,
            TrangThai = (TrangThaiPhieuNhapEnum)entity.TrangThaiPhieuNhap,

            // Thông tin liên kết
            WarehouseId = entity.WarehouseId,
            PurchaseOrderId = entity.PurchaseOrderId,
            SupplierId = entity.PartnerSiteId, // PartnerSiteId trong DB map với SupplierId trong DTO

            // Thông tin bổ sung
            Notes = entity.Notes ?? string.Empty,
            TotalQuantity = entity.TotalQuantity,
            TotalAmount = entity.TotalAmount,
            TotalVat = entity.TotalVat,
            TotalAmountIncludedVat = entity.TotalAmountIncludedVat,

            // Thông tin hệ thống (nếu có trong entity)
            // CreatedBy, CreatedDate, UpdatedBy, UpdatedDate - có thể không có trong entity
            // Sẽ được set từ entity nếu có
        };
    }

    /// <summary>
    /// Chuyển đổi từ DTO StockInMasterDto sang Entity StockInOutMaster.
    /// </summary>
    /// <param name="dto">DTO StockInMasterDto</param>
    /// <returns>Entity StockInOutMaster</returns>
    public static StockInOutMaster ToEntity(this StockInMasterDto dto)
    {
        if (dto == null)
            return null;

        return new StockInOutMaster
        {
            // Thông tin cơ bản
            Id = dto.Id != Guid.Empty ? dto.Id : Guid.NewGuid(),
            VocherNumber = dto.StockInNumber,
            StockInOutDate = dto.StockInDate,
            StockInOutType = (int)dto.LoaiNhapKho,
            TrangThaiPhieuNhap = (int)dto.TrangThai,

            // Thông tin liên kết
            WarehouseId = dto.WarehouseId,
            PurchaseOrderId = dto.PurchaseOrderId,
            PartnerSiteId = dto.SupplierId, // SupplierId trong DTO map với PartnerSiteId trong DB

            // Thông tin bổ sung
            Notes = dto.Notes ?? string.Empty,
            TotalQuantity = dto.TotalQuantity,
            TotalAmount = dto.TotalAmount,
            TotalVat = dto.TotalVat,
            TotalAmountIncludedVat = dto.TotalAmountIncludedVat,
        };
    }

    /// <summary>
    /// Chuyển đổi từ DTO sang Entity với thông tin cập nhật.
    /// </summary>
    /// <param name="dto">DTO StockInMasterDto</param>
    /// <param name="existingEntity">Entity hiện tại (để giữ nguyên thông tin cũ)</param>
    /// <returns>Entity StockInOutMaster đã cập nhật</returns>
    public static StockInOutMaster ToEntity(this StockInMasterDto dto, StockInOutMaster existingEntity)
    {
        if (dto == null)
            return null;

        if (existingEntity == null)
            return dto.ToEntity();

        // Cập nhật thông tin từ DTO
        // Không cập nhật Id nếu đã có
        if (dto.Id != Guid.Empty)
        {
            existingEntity.Id = dto.Id;
        }

        existingEntity.VocherNumber = dto.StockInNumber;
        existingEntity.StockInOutDate = dto.StockInDate;
        existingEntity.StockInOutType = (int)dto.LoaiNhapKho;
        existingEntity.TrangThaiPhieuNhap = (int)dto.TrangThai;
        existingEntity.WarehouseId = dto.WarehouseId;
        existingEntity.PurchaseOrderId = dto.PurchaseOrderId;
        existingEntity.PartnerSiteId = dto.SupplierId;
        existingEntity.Notes = dto.Notes ?? string.Empty;
        existingEntity.TotalQuantity = dto.TotalQuantity;
        existingEntity.TotalAmount = dto.TotalAmount;
        existingEntity.TotalVat = dto.TotalVat;
        existingEntity.TotalAmountIncludedVat = dto.TotalAmountIncludedVat;

        return existingEntity;
    }

    /// <summary>
    /// Chuyển đổi danh sách Entity sang danh sách DTO.
    /// </summary>
    /// <param name="entities">Danh sách Entity StockInOutMaster</param>
    /// <returns>Danh sách DTO StockInMasterDto</returns>
    public static IEnumerable<StockInMasterDto> ToDtos(this IEnumerable<StockInOutMaster> entities)
    {
        if (entities == null)
            return Enumerable.Empty<StockInMasterDto>();

        return entities.Select(entity => entity.ToDto());
    }

    /// <summary>
    /// Chuyển đổi danh sách DTO sang danh sách Entity.
    /// </summary>
    /// <param name="dtos">Danh sách DTO StockInMasterDto</param>
    /// <returns>Danh sách Entity StockInOutMaster</returns>
    public static IEnumerable<StockInOutMaster> ToEntities(this IEnumerable<StockInMasterDto> dtos)
    {
        if (dtos == null)
            return Enumerable.Empty<StockInOutMaster>();

        return dtos.Select(dto => dto.ToEntity());
    }

    #endregion
}

