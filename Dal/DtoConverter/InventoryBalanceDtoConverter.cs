using Dal.DataContext;
using DTO.Inventory.InventoryManagement;
using System;
using System.Collections.Generic;

namespace Dal.DtoConverter
{

    /// <summary>
    /// Converter giữa InventoryBalance entity và InventoryBalanceDto
    /// </summary>
    public static class InventoryBalanceDtoConverter
    {
        #region Entity to DTO

        /// <summary>
        /// Chuyển đổi InventoryBalance entity thành InventoryBalanceDto
        /// Tự động map navigation properties nếu đã được load trong entity
        /// </summary>
        /// <param name="entity">InventoryBalance entity</param>
        /// <returns>InventoryBalanceDto</returns>
        public static InventoryBalanceDto ToDto(this InventoryBalance entity)
        {
            if (entity == null) return null;

            var dto = new InventoryBalanceDto
            {
                Id = entity.Id,
                WarehouseId = entity.WarehouseId,
                ProductVariantId = entity.ProductVariantId,
                PeriodYear = entity.PeriodYear,
                PeriodMonth = entity.PeriodMonth,
                OpeningBalance = entity.OpeningBalance,
                TotalInQty = entity.TotalInQty,
                TotalOutQty = entity.TotalOutQty,
                ClosingBalance = entity.ClosingBalance,
                OpeningValue = entity.OpeningValue,
                TotalInValue = entity.TotalInValue,
                TotalOutValue = entity.TotalOutValue,
                ClosingValue = entity.ClosingValue,
                TotalInVatAmount = entity.TotalInVatAmount,
                TotalOutVatAmount = entity.TotalOutVatAmount,
                TotalInAmountIncludedVat = entity.TotalInAmountIncludedVat,
                TotalOutAmountIncludedVat = entity.TotalOutAmountIncludedVat,
                IsLocked = entity.IsLocked,
                LockedDate = entity.LockedDate,
                LockedBy = entity.LockedBy,
                LockReason = entity.LockReason,
                IsVerified = entity.IsVerified,
                VerifiedDate = entity.VerifiedDate,
                VerifiedBy = entity.VerifiedBy,
                VerificationNotes = entity.VerificationNotes,
                IsApproved = entity.IsApproved,
                ApprovedDate = entity.ApprovedDate,
                ApprovedBy = entity.ApprovedBy,
                ApprovalNotes = entity.ApprovalNotes,
                Status = entity.Status,
                Notes = entity.Notes,
                IsActive = entity.IsActive,
                IsDeleted = entity.IsDeleted,
                CreateDate = entity.CreateDate,
                CreateBy = entity.CreateBy,
                ModifiedDate = entity.ModifiedDate,
                ModifiedBy = entity.ModifiedBy,
                DeletedDate = entity.DeletedDate,
                DeletedBy = entity.DeletedBy
            };

            // Map navigation properties nếu đã được load trong entity
            // Warehouse
            if (entity.CompanyBranch != null)
            {
                dto.WarehouseName = entity.CompanyBranch.BranchName;
                dto.WarehouseCode = entity.CompanyBranch.BranchCode;
            }

            // Product
            if (entity.ProductVariant != null)
            {
                dto.ProductVariantCode = entity.ProductVariant.VariantCode;
                dto.ProductVariantFullName = entity.ProductVariant.VariantFullName ?? string.Empty;

                dto.UnitOfMeasureId = entity.ProductVariant.UnitId;

                // Lấy thông tin đơn vị tính nếu có
                if (entity.ProductVariant.UnitOfMeasure != null)
                {
                    dto.UnitOfMeasureCode = entity.ProductVariant.UnitOfMeasure.Code;
                    dto.UnitOfMeasureName = entity.ProductVariant.UnitOfMeasure.Name;
                }

                // Lấy thông tin từ ProductService (sản phẩm/dịch vụ gốc)
                if (entity.ProductVariant.ProductService != null)
                {
                    dto.ProductCode = entity.ProductVariant.ProductService.Code ?? string.Empty;
                    dto.ProductName = entity.ProductVariant.ProductService.Name ?? string.Empty;
                }
            }

            // User names
            if (entity.ApplicationUser1 != null) // CreateBy
            {
                dto.CreatedByName = entity.ApplicationUser1.UserName;
            }

            if (entity.ApplicationUser4 != null) // ModifiedBy
            {
                dto.ModifiedByName = entity.ApplicationUser4.UserName;
            }

            if (entity.ApplicationUser2 != null) // DeletedBy
            {
                dto.DeletedByName = entity.ApplicationUser2.UserName;
            }

            if (entity.ApplicationUser3 != null) // LockedBy
            {
                dto.LockedByName = entity.ApplicationUser3.UserName;
            }

            if (entity.ApplicationUser4 != null) // VerifiedBy
            {
                dto.VerifiedByName = entity.ApplicationUser4.UserName;
            }

            if (entity.ApplicationUser != null) // ApprovedBy
            {
                dto.ApprovedByName = entity.ApplicationUser.UserName;
            }

            return dto;
        }

        /// <summary>
        /// Chuyển đổi danh sách InventoryBalance entities thành danh sách InventoryBalanceDto
        /// </summary>
        /// <param name="entities">Danh sách InventoryBalance entities</param>
        /// <returns>Danh sách InventoryBalanceDto</returns>
        public static List<InventoryBalanceDto> ToDtoList(this IEnumerable<InventoryBalance> entities)
        {
            if (entities == null) return [];

            List<InventoryBalanceDto> list = [];
            foreach (var entity in entities)
            {
                var dto = entity.ToDto();
                if (dto != null) list.Add(dto);
            }

            return list;
        }

        #endregion

        #region DTO to Entity

        /// <summary>
        /// Chuyển đổi InventoryBalanceDto thành InventoryBalance entity
        /// </summary>
        /// <param name="dto">InventoryBalanceDto</param>
        /// <param name="existingEntity">Entity hiện có (nếu đang update), null nếu tạo mới</param>
        /// <returns>InventoryBalance entity</returns>
        public static InventoryBalance ToEntity(this InventoryBalanceDto dto, InventoryBalance existingEntity = null)
        {
            if (dto == null) return null;

            var entity = existingEntity ?? new InventoryBalance();

            // Chỉ map các properties có thể chỉnh sửa, không map navigation properties
            entity.WarehouseId = dto.WarehouseId;
            entity.ProductVariantId = dto.ProductVariantId;
            entity.PeriodYear = dto.PeriodYear;
            entity.PeriodMonth = dto.PeriodMonth;
            entity.OpeningBalance = dto.OpeningBalance;
            entity.TotalInQty = dto.TotalInQty;
            entity.TotalOutQty = dto.TotalOutQty;
            entity.ClosingBalance = dto.ClosingBalance;
            entity.OpeningValue = dto.OpeningValue;
            entity.TotalInValue = dto.TotalInValue;
            entity.TotalOutValue = dto.TotalOutValue;
            entity.ClosingValue = dto.ClosingValue;
            entity.TotalInVatAmount = dto.TotalInVatAmount;
            entity.TotalOutVatAmount = dto.TotalOutVatAmount;
            entity.TotalInAmountIncludedVat = dto.TotalInAmountIncludedVat;
            entity.TotalOutAmountIncludedVat = dto.TotalOutAmountIncludedVat;
            entity.IsLocked = dto.IsLocked;
            entity.LockedDate = dto.LockedDate;
            entity.LockedBy = dto.LockedBy;
            entity.LockReason = dto.LockReason;
            entity.IsVerified = dto.IsVerified;
            entity.VerifiedDate = dto.VerifiedDate;
            entity.VerifiedBy = dto.VerifiedBy;
            entity.VerificationNotes = dto.VerificationNotes;
            entity.IsApproved = dto.IsApproved;
            entity.ApprovedDate = dto.ApprovedDate;
            entity.ApprovedBy = dto.ApprovedBy;
            entity.ApprovalNotes = dto.ApprovalNotes;
            entity.Status = dto.Status;
            entity.Notes = dto.Notes;
            entity.IsActive = dto.IsActive;
            entity.IsDeleted = dto.IsDeleted;

            // Chỉ set ID và audit fields nếu là entity mới
            if (existingEntity == null)
            {
                entity.Id = dto.Id != Guid.Empty ? dto.Id : Guid.NewGuid();
                entity.CreateDate = DateTime.Now;
                entity.CreateBy = dto.CreateBy;
            }
            else
            {
                // Update: chỉ cập nhật ModifiedDate và ModifiedBy (sẽ được BLL set)
                // Không thay đổi CreateDate và CreateBy
            }

            // ModifiedDate và ModifiedBy sẽ được BLL set khi save
            entity.ModifiedDate = dto.ModifiedDate;
            entity.ModifiedBy = dto.ModifiedBy;
            entity.DeletedDate = dto.DeletedDate;
            entity.DeletedBy = dto.DeletedBy;

            return entity;
        }

        /// <summary>
        /// Chuyển đổi danh sách InventoryBalanceDto thành danh sách InventoryBalance entities
        /// </summary>
        /// <param name="dtos">Danh sách InventoryBalanceDto</param>
        /// <returns>Danh sách InventoryBalance entities</returns>
        public static List<InventoryBalance> ToEntityList(this IEnumerable<InventoryBalanceDto> dtos)
        {
            if (dtos == null) return [];

            List<InventoryBalance> list = [];
            foreach (var dto in dtos)
            {
                var entity = dto.ToEntity();
                if (entity != null) list.Add(entity);
            }

            return list;
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Map navigation properties từ entity sang DTO (warehouse, product, users)
        /// Method này được gọi từ BLL sau khi đã load navigation properties
        /// </summary>
        /// <param name="dto">DTO cần map navigation properties</param>
        /// <param name="warehouseName">Tên kho</param>
        /// <param name="warehouseCode">Mã kho</param>
        /// <param name="productName">Tên sản phẩm</param>
        /// <param name="productCode">Mã sản phẩm</param>
        /// <param name="createdByName">Tên người tạo</param>
        /// <param name="modifiedByName">Tên người sửa</param>
        /// <param name="deletedByName">Tên người xóa</param>
        /// <param name="lockedByName">Tên người khóa</param>
        /// <param name="verifiedByName">Tên người xác thực</param>
        /// <param name="approvedByName">Tên người phê duyệt</param>
        /// <returns>DTO đã được map navigation properties</returns>
        public static InventoryBalanceDto MapNavigationProperties(
            this InventoryBalanceDto dto,
            string warehouseName = null,
            string warehouseCode = null,
            string productName = null,
            string productCode = null,
            string createdByName = null,
            string modifiedByName = null,
            string deletedByName = null,
            string lockedByName = null,
            string verifiedByName = null,
            string approvedByName = null)
        {
            if (dto == null) return null;

            dto.WarehouseName = warehouseName;
            dto.WarehouseCode = warehouseCode;
            dto.ProductName = productName;
            dto.ProductCode = productCode;
            dto.CreatedByName = createdByName;
            dto.ModifiedByName = modifiedByName;
            dto.DeletedByName = deletedByName;
            dto.LockedByName = lockedByName;
            dto.VerifiedByName = verifiedByName;
            dto.ApprovedByName = approvedByName;

            return dto;
        }

        #endregion
    }


}
