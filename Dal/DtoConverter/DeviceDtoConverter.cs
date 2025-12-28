using Common.Enums;
using DTO.DeviceAssetManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Dal.DtoConverter
{

    /// <summary>
    /// Converter giữa Device entity và DeviceDto
    /// </summary>
    public static class DeviceDtoConverter
    {
        #region Entity to DTO

        /// <summary>
        /// Chuyển đổi Device entity thành DeviceDto
        /// </summary>
        /// <param name="entity">Device entity</param>
        /// <param name="productVariantName">Tên sản phẩm (tùy chọn, để tránh DataContext disposed errors)</param>
        /// <param name="productVariantCode">Mã sản phẩm (tùy chọn, để tránh DataContext disposed errors)</param>
        /// <param name="unitName">Tên đơn vị tính (tùy chọn, để tránh DataContext disposed errors)</param>
        /// <param name="warrantyFrom">Ngày bắt đầu bảo hành (tùy chọn, để tránh DataContext disposed errors)</param>
        /// <param name="warrantyUntil">Ngày kết thúc bảo hành (tùy chọn, để tránh DataContext disposed errors)</param>
        /// <param name="warrantyType">Loại bảo hành (tùy chọn, để tránh DataContext disposed errors)</param>
        /// <returns>DeviceDto</returns>
        public static DeviceDto ToDto(this Dal.DataContext.Device entity, 
            string productVariantName = null, 
            string productVariantCode = null, 
            string unitName = null,
            DateTime? warrantyFrom = null,
            DateTime? warrantyUntil = null,
            int? warrantyType = null)
        {
            if (entity == null) return null;

            // QUAN TRỌNG: KHÔNG truy cập navigation properties (entity.ProductVariant, entity.Warranties)
            // vì DataContext đã bị dispose sau khi repository method kết thúc
            // Chỉ sử dụng tham số truyền vào

            var dto = new DeviceDto
            {
                Id = entity.Id,
                ProductVariantId = entity.ProductVariantId,
                StockInOutDetailId = entity.StockInOutDetailId,
                SerialNumber = entity.SerialNumber,
                MACAddress = entity.MACAddress,
                IMEI = entity.IMEI,
                AssetTag = entity.AssetTag,
                LicenseKey = entity.LicenseKey,
                HostName = entity.HostName,
                IPAddress = entity.IPAddress,
                Status = (DeviceStatusEnum)entity.Status, // Convert int to enum
                DeviceType = entity.DeviceType,
                Notes = entity.Notes,
                IsActive = entity.IsActive,
                CreatedDate = entity.CreatedDate,
                UpdatedDate = entity.UpdatedDate,
                CreatedBy = entity.CreatedBy,
                UpdatedBy = entity.UpdatedBy,
                // Navigation properties - sử dụng giá trị từ tham số truyền vào
                ProductVariantName = productVariantName,
                ProductVariantCode = productVariantCode,
                UnitName = unitName,
                WarrantyFrom = warrantyFrom,
                WarrantyUntil = warrantyUntil
            };

            // Xác định trạng thái bảo hành dựa vào WarrantyUntil và thêm màu sắc
            if (warrantyUntil.HasValue)
            {
                var today = DateTime.Today;
                var warrantyUntilDate = warrantyUntil.Value.Date;
                string statusText;
                string statusColor;

                if (warrantyUntilDate > today)
                {
                    // Còn bảo hành
                    statusText = "Còn bảo hành";
                    statusColor = "green";
                }
                else if (warrantyUntilDate == today)
                {
                    // Hết bảo hành hôm nay
                    statusText = "Hết bảo hành";
                    statusColor = "red";
                }
                else
                {
                    // Đã hết bảo hành
                    statusText = "Hết bảo hành";
                    statusColor = "red";
                }

                // Thêm màu sắc vào WarrantyStatusName với format HTML
                dto.WarrantyStatusName = $"<color='{statusColor}'><b>{statusText}</b></color>";
            }
            else if (warrantyFrom.HasValue || warrantyType.HasValue)
            {
                // Chưa có ngày kết thúc bảo hành nhưng có thông tin bảo hành
                dto.WarrantyStatusName = "<color='gray'><b>Chưa xác định</b></color>";
            }

            // Chuyển đổi WarrantyType từ int sang enum và lấy tên
            if (warrantyType.HasValue && Enum.IsDefined(typeof(LoaiBaoHanhEnum), warrantyType.Value))
            {
                var warrantyTypeEnum = (LoaiBaoHanhEnum)warrantyType.Value;
                var warrantyTypeField = warrantyTypeEnum.GetType().GetField(warrantyTypeEnum.ToString());
                if (warrantyTypeField != null)
                {
                    var descriptionAttr = warrantyTypeField.GetCustomAttribute<DescriptionAttribute>();
                    dto.WarrantyTypeName = descriptionAttr?.Description ?? warrantyTypeEnum.ToString();
                }
            }

            return dto;
        }

        /// <summary>
        /// Chuyển đổi danh sách Device entities thành danh sách DeviceDto
        /// </summary>
        /// <param name="entities">Danh sách Device entities</param>
        /// <param name="productVariantDict">Dictionary chứa thông tin ProductVariant (key: ProductVariantId, value: (ProductVariantName, ProductVariantCode, UnitName))</param>
        /// <param name="warrantyDict">Dictionary chứa thông tin Warranty (key: DeviceId, value: (WarrantyFrom, WarrantyUntil, WarrantyType))</param>
        /// <returns>Danh sách DeviceDto</returns>
        public static List<DeviceDto> ToDtoList(this IEnumerable<Dal.DataContext.Device> entities,
            Dictionary<Guid, (string ProductVariantName, string ProductVariantCode, string UnitName)> productVariantDict = null,
            Dictionary<Guid, (DateTime? WarrantyFrom, DateTime? WarrantyUntil, int? WarrantyType)> warrantyDict = null)
        {
            if (entities == null) return new List<DeviceDto>();

            return entities.Select(entity =>
            {
                string productVariantName = null;
                string productVariantCode = null;
                string unitName = null;
                DateTime? warrantyFrom = null;
                DateTime? warrantyUntil = null;
                int? warrantyType = null;

                if (productVariantDict != null && productVariantDict.TryGetValue(entity.ProductVariantId, out var productInfo))
                {
                    productVariantName = productInfo.ProductVariantName;
                    productVariantCode = productInfo.ProductVariantCode;
                    unitName = productInfo.UnitName;
                }

                if (warrantyDict != null && warrantyDict.TryGetValue(entity.Id, out var warrantyInfo))
                {
                    warrantyFrom = warrantyInfo.WarrantyFrom;
                    warrantyUntil = warrantyInfo.WarrantyUntil;
                    warrantyType = warrantyInfo.WarrantyType;
                }

                return entity.ToDto(productVariantName, productVariantCode, unitName, warrantyFrom, warrantyUntil, warrantyType);
            }).ToList();
        }


        #endregion

        #region DTO to Entity

        /// <summary>
        /// Chuyển đổi DeviceDto thành Device entity
        /// </summary>
        /// <param name="dto">DeviceDto</param>
        /// <param name="existingEntity">Existing entity to update (optional, for edit mode)</param>
        /// <returns>Device entity</returns>
        public static Dal.DataContext.Device ToEntity(this DeviceDto dto, Dal.DataContext.Device existingEntity = null)
        {
            if (dto == null) return null;

            Dal.DataContext.Device entity;
            if (existingEntity != null)
            {
                // Update existing entity
                entity = existingEntity;
            }
            else
            {
                // Create new entity
                entity = new Dal.DataContext.Device();
                if (dto.Id != Guid.Empty)
                {
                    entity.Id = dto.Id;
                }
            }

            // Map properties
            entity.ProductVariantId = dto.ProductVariantId;
            entity.StockInOutDetailId = dto.StockInOutDetailId;
            entity.SerialNumber = dto.SerialNumber;
            entity.MACAddress = dto.MACAddress;
            entity.IMEI = dto.IMEI;
            entity.AssetTag = dto.AssetTag;
            entity.LicenseKey = dto.LicenseKey;
            entity.HostName = dto.HostName;
            entity.IPAddress = dto.IPAddress;
            entity.Status = (int)dto.Status; // Convert enum to int
            entity.DeviceType = dto.DeviceType;
            entity.Notes = dto.Notes;
            entity.IsActive = dto.IsActive;
            entity.UpdatedDate = dto.UpdatedDate;
            entity.UpdatedBy = dto.UpdatedBy;

            if (existingEntity == null)
            {
                // Chỉ set CreatedDate và CreatedBy khi tạo mới
                entity.CreatedDate = dto.CreatedDate == default(DateTime) ? DateTime.Now : dto.CreatedDate;
                entity.CreatedBy = dto.CreatedBy;
            }

            return entity;
        }

        #endregion
    }

}
