using Dal.DataContext;
using DTO.DeviceAssetManagement;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dal.DtoConverter
{
    /// <summary>
    /// Converter giữa Asset entity và AssetDto
    /// </summary>
    public static class AssetConverters
    {
        #region Entity to DTO

        /// <summary>
        /// Chuyển đổi Asset entity thành AssetDto
        /// </summary>
        /// <param name="entity">Asset entity</param>
        /// <param name="createdByName">Tên người tạo (tùy chọn, để tránh DataContext disposed errors)</param>
        /// <param name="modifiedByName">Tên người cập nhật (tùy chọn, để tránh DataContext disposed errors)</param>
        /// <param name="deletedByName">Tên người xóa (tùy chọn, để tránh DataContext disposed errors)</param>
        /// <returns>AssetDto</returns>
        public static AssetDto ToDto(this Asset entity, 
            string createdByName = null, 
            string modifiedByName = null,
            string deletedByName = null)
        {
            if (entity == null) return null;

            var dto = new AssetDto
            {
                // Thông tin cơ bản
                Id = entity.Id,
                AssetCode = entity.AssetCode,
                AssetName = entity.AssetName,
                AssetType = entity.AssetType,
                AssetCategory = entity.AssetCategory,
                Description = entity.Description,

                // Liên kết sản phẩm
                ProductVariantId = entity.ProductVariantId,
                ProductVariantCode = entity.ProductVariant?.VariantCode,
                ProductVariantFullName = entity.ProductVariant?.VariantFullName,

                // Thông tin công ty và địa điểm
                CompanyId = entity.CompanyId,
                CompanyName = entity.Company?.CompanyName,
                BranchId = entity.BranchId,
                BranchName = entity.CompanyBranch?.BranchName,
                BranchCode = entity.CompanyBranch?.BranchCode,
                DepartmentId = entity.DepartmentId,
                DepartmentName = entity.Department?.DepartmentName,
                AssignedEmployeeId = entity.AssignedEmployeeId,
                AssignedEmployeeName = entity.Employee?.FullName,
                Location = entity.Location,

                // Thông tin tài chính
                PurchasePrice = entity.PurchasePrice,
                PurchaseDate = entity.PurchaseDate,
                SupplierName = entity.SupplierName,
                InvoiceNumber = entity.InvoiceNumber,
                InvoiceDate = entity.InvoiceDate,

                // Khấu hao
                DepreciationMethod = entity.DepreciationMethod,
                DepreciationRate = entity.DepreciationRate,
                UsefulLife = entity.UsefulLife,
                DepreciationStartDate = entity.DepreciationStartDate,
                AccumulatedDepreciation = entity.AccumulatedDepreciation,
                CurrentValue = entity.CurrentValue,

                // Trạng thái và tình trạng
                Status = entity.Status,
                Condition = entity.Condition,

                // Bảo hành
                WarrantyId = entity.WarrantyId,
                
                WarrantyExpiryDate = entity.WarrantyExpiryDate,

                // Thông tin kỹ thuật
                SerialNumber = entity.SerialNumber,
                Manufacturer = entity.Manufacturer,
                Model = entity.Model,
                Specifications = entity.Specifications,
                Notes = entity.Notes,

                // Audit
                IsActive = entity.IsActive,
                IsDeleted = entity.IsDeleted,
                CreateDate = entity.CreateDate,
                CreateBy = entity.CreateBy,
                CreatedByName = createdByName,
                ModifiedDate = entity.ModifiedDate,
                ModifiedBy = entity.ModifiedBy,
                ModifiedByName = modifiedByName,
                DeletedDate = entity.DeletedDate,
                DeletedBy = entity.DeletedBy,
                DeletedByName = deletedByName
            };

            return dto;
        }

        /// <summary>
        /// Chuyển đổi danh sách Asset entities thành danh sách AssetDto
        /// </summary>
        /// <param name="entities">Danh sách Asset entities</param>
        /// <returns>Danh sách AssetDto</returns>
        public static List<AssetDto> ToDtoList(this IEnumerable<Asset> entities)
        {
            if (entities == null) return new List<AssetDto>();

            return entities.Select(entity => entity.ToDto()).ToList();
        }

        /// <summary>
        /// Chuyển đổi danh sách Asset entities thành danh sách AssetDto (alias cho ToDtoList)
        /// </summary>
        /// <param name="entities">Danh sách Asset entities</param>
        /// <returns>Danh sách AssetDto</returns>
        public static List<AssetDto> ToDtos(this IEnumerable<Asset> entities)
        {
            if (entities == null) return new List<AssetDto>();

            return entities.Select(entity => entity.ToDto()).ToList();
        }

        #endregion

        #region DTO to Entity

        /// <summary>
        /// Chuyển đổi AssetDto thành Asset entity
        /// </summary>
        /// <param name="dto">AssetDto</param>
        /// <param name="destination">Entity đích để cập nhật (nếu null thì tạo mới)</param>
        /// <returns>Asset entity</returns>
        public static Asset ToEntity(this AssetDto dto, Asset destination = null)
        {
            if (dto == null) return null;

            if (destination == null)
            {
                // Tạo mới
                return new Asset
                {
                    Id = dto.Id != Guid.Empty ? dto.Id : Guid.NewGuid(),
                    AssetCode = dto.AssetCode,
                    AssetName = dto.AssetName,
                    AssetType = dto.AssetType,
                    AssetCategory = dto.AssetCategory,
                    Description = dto.Description,
                    ProductVariantId = dto.ProductVariantId,
                    CompanyId = dto.CompanyId,
                    BranchId = dto.BranchId,
                    DepartmentId = dto.DepartmentId,
                    AssignedEmployeeId = dto.AssignedEmployeeId,
                    Location = dto.Location,
                    PurchasePrice = dto.PurchasePrice,
                    PurchaseDate = dto.PurchaseDate,
                    SupplierName = dto.SupplierName,
                    InvoiceNumber = dto.InvoiceNumber,
                    InvoiceDate = dto.InvoiceDate,
                    DepreciationMethod = dto.DepreciationMethod,
                    DepreciationRate = dto.DepreciationRate,
                    UsefulLife = dto.UsefulLife,
                    DepreciationStartDate = dto.DepreciationStartDate,
                    AccumulatedDepreciation = dto.AccumulatedDepreciation,
                    CurrentValue = dto.CurrentValue,
                    Status = dto.Status,
                    Condition = dto.Condition,
                    WarrantyId = dto.WarrantyId,
                    WarrantyExpiryDate = dto.WarrantyExpiryDate,
                    SerialNumber = dto.SerialNumber,
                    Manufacturer = dto.Manufacturer,
                    Model = dto.Model,
                    Specifications = dto.Specifications,
                    Notes = dto.Notes,
                    IsActive = dto.IsActive,
                    IsDeleted = dto.IsDeleted,
                    CreateDate = dto.CreateDate != default(DateTime) ? dto.CreateDate : DateTime.Now,
                    CreateBy = dto.CreateBy,
                    ModifiedDate = null, // Khi tạo mới, ModifiedDate nên là null
                    ModifiedBy = null,   // Khi tạo mới, ModifiedBy nên là null
                    DeletedDate = dto.DeletedDate,
                    DeletedBy = dto.DeletedBy
                };
            }
            else
            {
                // Cập nhật
                destination.AssetCode = dto.AssetCode;
                destination.AssetName = dto.AssetName;
                destination.AssetType = dto.AssetType;
                destination.AssetCategory = dto.AssetCategory;
                destination.Description = dto.Description;
                destination.ProductVariantId = dto.ProductVariantId;
                destination.CompanyId = dto.CompanyId;
                destination.BranchId = dto.BranchId;
                destination.DepartmentId = dto.DepartmentId;
                destination.AssignedEmployeeId = dto.AssignedEmployeeId;
                destination.Location = dto.Location;
                destination.PurchasePrice = dto.PurchasePrice;
                destination.PurchaseDate = dto.PurchaseDate;
                destination.SupplierName = dto.SupplierName;
                destination.InvoiceNumber = dto.InvoiceNumber;
                destination.InvoiceDate = dto.InvoiceDate;
                destination.DepreciationMethod = dto.DepreciationMethod;
                destination.DepreciationRate = dto.DepreciationRate;
                destination.UsefulLife = dto.UsefulLife;
                destination.DepreciationStartDate = dto.DepreciationStartDate;
                destination.AccumulatedDepreciation = dto.AccumulatedDepreciation;
                destination.CurrentValue = dto.CurrentValue;
                destination.Status = dto.Status;
                destination.Condition = dto.Condition;
                destination.WarrantyId = dto.WarrantyId;
                destination.WarrantyExpiryDate = dto.WarrantyExpiryDate;
                destination.SerialNumber = dto.SerialNumber;
                destination.Manufacturer = dto.Manufacturer;
                destination.Model = dto.Model;
                destination.Specifications = dto.Specifications;
                destination.Notes = dto.Notes;
                destination.IsActive = dto.IsActive;
                destination.IsDeleted = dto.IsDeleted;
                // ModifiedDate và ModifiedBy sẽ được set bởi repository layer
                // Không set ở đây để tránh duplicate
                destination.DeletedDate = dto.DeletedDate;
                destination.DeletedBy = dto.DeletedBy;
                
                return destination;
            }
        }

        #endregion
    }
}
