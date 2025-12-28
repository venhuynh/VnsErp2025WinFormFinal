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
    public static class AssetDtoConverter
    {
        #region Entity to DTO

        /// <summary>
        /// Chuyển đổi Asset entity thành AssetDto
        /// Tự động map navigation properties nếu đã được load trong entity
        /// </summary>
        /// <param name="entity">Asset entity</param>
        /// <returns>AssetDto</returns>
        public static AssetDto ToDto(this Asset entity)
        {
            if (entity == null) return null;

            var dto = new AssetDto
            {
                Id = entity.Id,
                AssetCode = entity.AssetCode,
                AssetName = entity.AssetName,
                AssetType = entity.AssetType,
                AssetCategory = entity.AssetCategory,
                Description = entity.Description,
                ProductVariantId = entity.ProductVariantId,
                CompanyId = entity.CompanyId,
                BranchId = entity.BranchId,
                DepartmentId = entity.DepartmentId,
                AssignedEmployeeId = entity.AssignedEmployeeId,
                Location = entity.Location,
                PurchasePrice = entity.PurchasePrice,
                PurchaseDate = entity.PurchaseDate,
                SupplierName = entity.SupplierName,
                InvoiceNumber = entity.InvoiceNumber,
                InvoiceDate = entity.InvoiceDate,
                DepreciationMethod = entity.DepreciationMethod,
                DepreciationRate = entity.DepreciationRate,
                UsefulLife = entity.UsefulLife,
                DepreciationStartDate = entity.DepreciationStartDate,
                AccumulatedDepreciation = entity.AccumulatedDepreciation,
                CurrentValue = entity.CurrentValue,
                Status = entity.Status,
                Condition = entity.Condition,
                WarrantyId = entity.WarrantyId,
                WarrantyExpiryDate = entity.WarrantyExpiryDate,
                SerialNumber = entity.SerialNumber,
                Manufacturer = entity.Manufacturer,
                Model = entity.Model,
                Specifications = entity.Specifications,
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
            // ProductVariant
            if (entity.ProductVariant != null)
            {
                dto.ProductVariantCode = entity.ProductVariant.VariantCode;
                dto.ProductVariantFullName = entity.ProductVariant.VariantFullName ?? string.Empty;
            }

            // Company
            if (entity.Company != null)
            {
                dto.CompanyName = entity.Company.CompanyName;
            }

            // Branch
            if (entity.CompanyBranch != null)
            {
                dto.BranchName = entity.CompanyBranch.BranchName;
                dto.BranchCode = entity.CompanyBranch.BranchCode;
            }

            // Department
            if (entity.Department != null)
            {
                dto.DepartmentName = entity.Department.DepartmentName;
            }

            // Employee
            if (entity.Employee != null)
            {
                dto.AssignedEmployeeName = entity.Employee.FullName;
            }

            // Warranty
            if (entity.Warranty != null)
            {
                // Warranty không có WarrantyName, sử dụng DeviceInfo từ Device hoặc tạo display string
                var warrantyDeviceInfoParts = new List<string>();
                if (entity.Warranty.Device != null)
                {
                    if (!string.IsNullOrWhiteSpace(entity.Warranty.Device.SerialNumber))
                        warrantyDeviceInfoParts.Add($"S/N: {entity.Warranty.Device.SerialNumber}");
                    if (!string.IsNullOrWhiteSpace(entity.Warranty.Device.IMEI))
                        warrantyDeviceInfoParts.Add($"IMEI: {entity.Warranty.Device.IMEI}");
                    if (!string.IsNullOrWhiteSpace(entity.Warranty.Device.MACAddress))
                        warrantyDeviceInfoParts.Add($"MAC: {entity.Warranty.Device.MACAddress}");
                    if (!string.IsNullOrWhiteSpace(entity.Warranty.Device.AssetTag))
                        warrantyDeviceInfoParts.Add($"Asset: {entity.Warranty.Device.AssetTag}");
                    if (!string.IsNullOrWhiteSpace(entity.Warranty.Device.LicenseKey))
                        warrantyDeviceInfoParts.Add($"License: {entity.Warranty.Device.LicenseKey}");
                }

                if (warrantyDeviceInfoParts.Any())
                {
                    dto.WarrantyName = string.Join(" | ", warrantyDeviceInfoParts);
                }
                else if (entity.Warranty.WarrantyUntil.HasValue)
                {
                    dto.WarrantyName = $"Bảo hành {entity.Warranty.WarrantyType} - {entity.Warranty.WarrantyUntil.Value:dd/MM/yyyy}";
                }
                else
                {
                    dto.WarrantyName = $"Bảo hành {entity.Warranty.WarrantyType}";
                }
            }

            // User names
            if (entity.ApplicationUser != null) // CreateBy
            {
                dto.CreatedByName = entity.ApplicationUser.UserName;
            }

            if (entity.ApplicationUser1 != null) // ModifiedBy
            {
                dto.ModifiedByName = entity.ApplicationUser1.UserName;
            }

            if (entity.ApplicationUser2 != null) // DeletedBy
            {
                dto.DeletedByName = entity.ApplicationUser2.UserName;
            }

            return dto;
        }

        /// <summary>
        /// Chuyển đổi danh sách Asset entities thành danh sách AssetDto
        /// </summary>
        /// <param name="entities">Danh sách Asset entities</param>
        /// <returns>Danh sách AssetDto</returns>
        public static List<AssetDto> ToDtoList(this IEnumerable<Asset> entities)
        {
            if (entities == null) return [];

            List<AssetDto> list = [];
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
        /// Chuyển đổi AssetDto thành Asset entity
        /// </summary>
        /// <param name="dto">AssetDto</param>
        /// <param name="existingEntity">Entity hiện có (nếu đang update), null nếu tạo mới</param>
        /// <returns>Asset entity</returns>
        public static Asset ToEntity(this AssetDto dto, Asset existingEntity = null)
        {
            if (dto == null) return null;

            var entity = existingEntity ?? new Asset();

            // Chỉ map các properties có thể chỉnh sửa, không map navigation properties
            entity.AssetCode = dto.AssetCode;
            entity.AssetName = dto.AssetName;
            entity.AssetType = dto.AssetType;
            entity.AssetCategory = dto.AssetCategory;
            entity.Description = dto.Description;
            entity.ProductVariantId = dto.ProductVariantId;
            entity.CompanyId = dto.CompanyId;
            entity.BranchId = dto.BranchId;
            entity.DepartmentId = dto.DepartmentId;
            entity.AssignedEmployeeId = dto.AssignedEmployeeId;
            entity.Location = dto.Location;
            entity.PurchasePrice = dto.PurchasePrice;
            entity.PurchaseDate = dto.PurchaseDate;
            entity.SupplierName = dto.SupplierName;
            entity.InvoiceNumber = dto.InvoiceNumber;
            entity.InvoiceDate = dto.InvoiceDate;
            entity.DepreciationMethod = dto.DepreciationMethod;
            entity.DepreciationRate = dto.DepreciationRate;
            entity.UsefulLife = dto.UsefulLife;
            entity.DepreciationStartDate = dto.DepreciationStartDate;
            entity.AccumulatedDepreciation = dto.AccumulatedDepreciation;
            entity.CurrentValue = dto.CurrentValue;
            entity.Status = dto.Status;
            entity.Condition = dto.Condition;
            entity.WarrantyId = dto.WarrantyId;
            entity.WarrantyExpiryDate = dto.WarrantyExpiryDate;
            entity.SerialNumber = dto.SerialNumber;
            entity.Manufacturer = dto.Manufacturer;
            entity.Model = dto.Model;
            entity.Specifications = dto.Specifications;
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
        /// Chuyển đổi danh sách AssetDto thành danh sách Asset entities
        /// </summary>
        /// <param name="dtos">Danh sách AssetDto</param>
        /// <returns>Danh sách Asset entities</returns>
        public static List<Asset> ToEntityList(this IEnumerable<AssetDto> dtos)
        {
            if (dtos == null) return [];

            List<Asset> list = [];
            foreach (var dto in dtos)
            {
                var entity = dto.ToEntity();
                if (entity != null) list.Add(entity);
            }

            return list;
        }

        #endregion
    }


}
