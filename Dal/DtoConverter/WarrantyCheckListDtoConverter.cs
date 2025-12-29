using Common.Enums;
using Dal.DataContext;
using DTO.DeviceAssetManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Dal.DtoConverter
{

    /// <summary>
    /// Converter giữa Warranty entity và WarrantyCheckListDto
    /// </summary>
    public static class WarrantyCheckListDtoConverter
    {
        #region Entity to DTO

        /// <summary>
        /// Chuyển đổi Warranty entity thành WarrantyDto
        /// </summary>
        /// <param name="entity">Warranty entity</param>
        /// <returns>WarrantyDto</returns>
        public static WarrantyCheckListDto ToWarrantyCheckListDto(this Warranty entity)
        {
            if (entity == null) return null;

            var dto = new WarrantyCheckListDto
            {
                Id = entity.Id,
                DeviceId = entity.DeviceId,
                WarrantyFrom = entity.WarrantyFrom,
                MonthOfWarranty = entity.MonthOfWarranty,
                WarrantyUntil = entity.WarrantyUntil,
                Notes = entity.Notes
            };

            // Lấy thông tin Device (SerialNumber, IMEI, MACAddress, AssetTag, LicenseKey)
            if (entity.Device != null)
            {
                var deviceInfoParts = new List<string>();
                if (!string.IsNullOrWhiteSpace(entity.Device.SerialNumber))
                    deviceInfoParts.Add($"S/N: {entity.Device.SerialNumber}");
                if (!string.IsNullOrWhiteSpace(entity.Device.IMEI))
                    deviceInfoParts.Add($"IMEI: {entity.Device.IMEI}");
                if (!string.IsNullOrWhiteSpace(entity.Device.MACAddress))
                    deviceInfoParts.Add($"MAC: {entity.Device.MACAddress}");
                if (!string.IsNullOrWhiteSpace(entity.Device.AssetTag))
                    deviceInfoParts.Add($"Asset: {entity.Device.AssetTag}");
                if (!string.IsNullOrWhiteSpace(entity.Device.LicenseKey))
                    deviceInfoParts.Add($"License: {entity.Device.LicenseKey}");

                dto.DeviceInfo = string.Join(" | ", deviceInfoParts);
            }

            // Lấy thông tin sản phẩm từ ProductVariant thông qua Device
            if (entity.Device != null && entity.Device.ProductVariant != null)
            {
                var productVariant = entity.Device.ProductVariant;

                // Lấy thông tin từ ProductService (sản phẩm/dịch vụ gốc)
                if (productVariant.ProductService != null)
                {
                    dto.ProductCode = productVariant.ProductService.Code ?? string.Empty;
                    dto.ProductName = productVariant.ProductService.Name ?? string.Empty;
                }

                // Lấy tên biến thể - ưu tiên VariantFullName, nếu không có thì lấy từ ProductService.Name
                if (!string.IsNullOrWhiteSpace(productVariant.VariantFullName))
                {
                    dto.ProductVariantName = productVariant.VariantFullName;
                }
                else if (productVariant.ProductService != null && !string.IsNullOrWhiteSpace(productVariant.ProductService.Name))
                {
                    dto.ProductVariantName = productVariant.ProductService.Name;
                }
            }

            // Lấy thông tin khách hàng từ StockInOutMaster thông qua Device.StockInOutDetail
            if (entity.Device != null && entity.Device.StockInOutDetail != null && entity.Device.StockInOutDetail.StockInOutMaster != null)
            {
                var master = entity.Device.StockInOutDetail.StockInOutMaster;
                dto.CustomerName = master.BusinessPartnerSite?.BusinessPartner?.PartnerName ??
                                  master.BusinessPartnerSite?.SiteName;
            }

            // Chuyển đổi WarrantyType từ int sang enum
            if (Enum.IsDefined(typeof(LoaiBaoHanhEnum), entity.WarrantyType))
            {
                dto.WarrantyType = (LoaiBaoHanhEnum)entity.WarrantyType;
            }
            else
            {
                // Nếu giá trị không hợp lệ, mặc định là NCCToVNS
                dto.WarrantyType = LoaiBaoHanhEnum.NCCToVNS;
            }

            // Lấy tên kiểu bảo hành từ Description attribute
            dto.WarrantyTypeName = GetEnumDescription(dto.WarrantyType);

            // Chuyển đổi WarrantyStatus từ int sang enum
            if (Enum.IsDefined(typeof(TrangThaiBaoHanhEnum), entity.WarrantyStatus))
            {
                dto.WarrantyStatus = (TrangThaiBaoHanhEnum)entity.WarrantyStatus;
            }
            else
            {
                // Nếu giá trị không hợp lệ, mặc định là ChoXuLy
                dto.WarrantyStatus = TrangThaiBaoHanhEnum.ChoXuLy;
            }

            // Lấy tên trạng thái từ Description attribute
            dto.WarrantyStatusName = GetEnumDescription(dto.WarrantyStatus);

            return dto;
        }

        /// <summary>
        /// Chuyển đổi danh sách Warranty entities thành danh sách WarrantyCheckListDto
        /// </summary>
        /// <param name="entities">Danh sách Warranty entities</param>
        /// <returns>Danh sách WarrantyCheckListDto</returns>
        public static List<WarrantyCheckListDto> ToDtoList(this IEnumerable<Warranty> entities)
        {
            if (entities == null) return new List<WarrantyCheckListDto>();

            return entities.Select(entity => entity.ToWarrantyCheckListDto()).ToList();
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Lấy Description từ enum value (generic method)
        /// </summary>
        /// <typeparam name="T">Kiểu enum</typeparam>
        /// <param name="enumValue">Giá trị enum</param>
        /// <returns>Description hoặc tên enum nếu không có Description</returns>
        private static string GetEnumDescription<T>(T enumValue)
        {
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
            if (fieldInfo == null) return enumValue.ToString();

            var descriptionAttribute = fieldInfo.GetCustomAttribute<DescriptionAttribute>();
            return descriptionAttribute?.Description ?? enumValue.ToString();
        }

        #endregion
    }
}
