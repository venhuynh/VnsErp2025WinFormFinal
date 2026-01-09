using DTO.Inventory.StockTakking;
using System;
using System.Collections.Generic;
using System.Linq;
using StocktakingMaster = Dal.DataContext.StocktakingMaster;

namespace Dal.DtoConverter.Inventory.StockTakking
{
    /// <summary>
    /// Converter giữa StocktakingMaster entity và StocktakingMasterDto
    /// </summary>
    public static class StocktakingMasterDtoConverter
    {
        #region Entity to DTO

        /// <summary>
        /// Chuyển đổi StocktakingMaster entity thành StocktakingMasterDto
        /// </summary>
        /// <param name="entity">StocktakingMaster entity</param>
        /// <param name="warehouseName">Tên kho (tùy chọn, để tránh DataContext disposed errors)</param>
        /// <param name="warehouseCode">Mã kho (tùy chọn)</param>
        /// <returns>StocktakingMasterDto</returns>
        public static StocktakingMasterDto ToDto(this StocktakingMaster entity,
            string warehouseName = null,
            string warehouseCode = null)
        {
            if (entity == null) return null;

            // QUAN TRỌNG: KHÔNG truy cập navigation properties (entity.Warehouse, entity.CompanyBranch, etc.)
            // vì DataContext đã bị dispose sau khi repository method kết thúc
            // Chỉ sử dụng tham số truyền vào

            var dto = new StocktakingMasterDto
            {
                // Thông tin cơ bản
                Id = entity.Id,
                StocktakingDate = entity.StocktakingDate,
                VoucherNumber = entity.VoucherNumber,
                StocktakingType = entity.StocktakingType,
                StocktakingStatus = entity.StocktakingStatus,
                WarehouseId = entity.WarehouseId,
                CompanyBranchId = entity.CompanyBranchId,
                WarehouseName = warehouseName,
                WarehouseCode = warehouseCode,

                // Thời gian kiểm kho
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,

                // Quy trình phê duyệt
                CountedBy = entity.CountedBy,
                CountedDate = entity.CountedDate,
                ReviewedBy = entity.ReviewedBy,
                ReviewedDate = entity.ReviewedDate,
                ApprovedBy = entity.ApprovedBy,
                ApprovedDate = entity.ApprovedDate,

                // Thông tin bổ sung
                Notes = entity.Notes,
                Reason = entity.Reason,

                // Khóa phiếu
                IsLocked = entity.IsLocked,
                LockedDate = entity.LockedDate,
                LockedBy = entity.LockedBy,

                // Trạng thái
                IsActive = entity.IsActive,
                IsDeleted = entity.IsDeleted,

                // Audit fields
                CreatedBy = entity.CreatedBy,
                CreatedDate = entity.CreatedDate,
                UpdatedBy = entity.UpdatedBy,
                UpdatedDate = entity.UpdatedDate,
                DeletedBy = entity.DeletedBy,
                DeletedDate = entity.DeletedDate
            };

            return dto;
        }

        /// <summary>
        /// Chuyển đổi danh sách StocktakingMaster entities thành danh sách StocktakingMasterDto
        /// </summary>
        /// <param name="entities">Danh sách StocktakingMaster entities</param>
        /// <param name="warehouseDict">Dictionary chứa thông tin Warehouse (key: WarehouseId, value: (WarehouseName, WarehouseCode))</param>
        /// <returns>Danh sách StocktakingMasterDto</returns>
        public static List<StocktakingMasterDto> ToDtoList(this IEnumerable<StocktakingMaster> entities,
            Dictionary<Guid, (string WarehouseName, string WarehouseCode)> warehouseDict = null)
        {
            if (entities == null) return new List<StocktakingMasterDto>();

            return entities.Select(entity =>
            {
                string warehouseName = null;
                string warehouseCode = null;

                if (warehouseDict != null && warehouseDict.TryGetValue(entity.WarehouseId, out var warehouseInfo))
                {
                    warehouseName = warehouseInfo.WarehouseName;
                    warehouseCode = warehouseInfo.WarehouseCode;
                }

                return entity.ToDto(warehouseName, warehouseCode);
            }).ToList();
        }

        /// <summary>
        /// Chuyển đổi danh sách StocktakingMaster entities thành danh sách StocktakingMasterDto (alias)
        /// </summary>
        /// <param name="entities">Danh sách StocktakingMaster entities</param>
        /// <returns>Danh sách StocktakingMasterDto</returns>
        public static List<StocktakingMasterDto> ToDtos(this IEnumerable<StocktakingMaster> entities)
        {
            if (entities == null) return new List<StocktakingMasterDto>();
            return entities.Select(e => e.ToDto(null, null)).ToList();
        }

        #endregion

        #region DTO to Entity

        /// <summary>
        /// Chuyển đổi StocktakingMasterDto thành StocktakingMaster entity
        /// </summary>
        /// <param name="dto">StocktakingMasterDto</param>
        /// <param name="existingEntity">Existing entity to update (optional, for edit mode)</param>
        /// <returns>StocktakingMaster entity</returns>
        public static StocktakingMaster ToEntity(this StocktakingMasterDto dto, StocktakingMaster existingEntity = null)
        {
            if (dto == null) return null;

            StocktakingMaster entity;
            if (existingEntity != null)
            {
                // Update existing entity
                entity = existingEntity;
            }
            else
            {
                // Create new entity
                entity = new StocktakingMaster();
                if (dto.Id != Guid.Empty)
                {
                    entity.Id = dto.Id;
                }
                else
                {
                    entity.Id = Guid.NewGuid();
                }
            }

            // Map properties - Thông tin cơ bản
            entity.StocktakingDate = dto.StocktakingDate;
            entity.VoucherNumber = dto.VoucherNumber;
            entity.StocktakingType = dto.StocktakingType;
            entity.StocktakingStatus = dto.StocktakingStatus;
            entity.WarehouseId = dto.WarehouseId;
            entity.CompanyBranchId = dto.CompanyBranchId;

            // Map properties - Thời gian kiểm kho
            entity.StartDate = dto.StartDate;
            entity.EndDate = dto.EndDate;

            // Map properties - Quy trình phê duyệt
            entity.CountedBy = dto.CountedBy;
            entity.CountedDate = dto.CountedDate;
            entity.ReviewedBy = dto.ReviewedBy;
            entity.ReviewedDate = dto.ReviewedDate;
            entity.ApprovedBy = dto.ApprovedBy;
            entity.ApprovedDate = dto.ApprovedDate;

            // Map properties - Thông tin bổ sung
            entity.Notes = dto.Notes;
            entity.Reason = dto.Reason;

            // Map properties - Khóa phiếu
            entity.IsLocked = dto.IsLocked;
            entity.LockedDate = dto.LockedDate;
            entity.LockedBy = dto.LockedBy;

            // Map properties - Trạng thái
            entity.IsActive = dto.IsActive;
            entity.IsDeleted = dto.IsDeleted;

            // Map properties - Audit fields
            entity.UpdatedDate = dto.UpdatedDate ?? DateTime.Now;
            entity.UpdatedBy = dto.UpdatedBy;

            if (existingEntity == null)
            {
                // Chỉ set CreatedDate và CreatedBy khi tạo mới
                entity.CreatedDate = dto.CreatedDate ?? DateTime.Now;
                entity.CreatedBy = dto.CreatedBy;
            }

            return entity;
        }

        /// <summary>
        /// Chuyển đổi danh sách StocktakingMasterDto thành danh sách StocktakingMaster entities
        /// </summary>
        /// <param name="dtos">Danh sách StocktakingMasterDto</param>
        /// <returns>Danh sách StocktakingMaster entities</returns>
        public static List<StocktakingMaster> ToEntityList(this IEnumerable<StocktakingMasterDto> dtos)
        {
            if (dtos == null) return new List<StocktakingMaster>();
            return dtos.Select(d => d.ToEntity()).Where(e => e != null).ToList();
        }

        /// <summary>
        /// Chuyển đổi danh sách StocktakingMasterDto thành danh sách StocktakingMaster entities (alias)
        /// </summary>
        /// <param name="dtos">Danh sách StocktakingMasterDto</param>
        /// <returns>Danh sách StocktakingMaster entities</returns>
        public static List<StocktakingMaster> ToEntities(this IEnumerable<StocktakingMasterDto> dtos)
        {
            if (dtos == null) return new List<StocktakingMaster>();
            return dtos.Select(d => d.ToEntity()).Where(e => e != null).ToList();
        }

        #endregion
    }
}
