using Common.Utils;
using DTO.DeviceAssetManagement;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dal.DtoConverter
{

    /// <summary>
    /// Helper class cho DeviceTransactionHistoryDto
    /// </summary>
    public static class DeviceTransactionHistoryConverter
    {
        /// <summary>
        /// Lấy màu sắc tương ứng với loại thao tác (tên màu)
        /// </summary>
        /// <param name="operationType">Loại thao tác (int value của DeviceOperationTypeEnum)</param>
        /// <returns>Tên màu (green, blue, red, orange, purple, grey, black, v.v.)</returns>
        public static string GetOperationTypeColor(int operationType)
        {
            if (!Enum.IsDefined(typeof(DeviceOperationTypeEnum), operationType))
                return "black";

            var enumValue = (DeviceOperationTypeEnum)operationType;
            return GetOperationTypeColor(enumValue);
        }

        /// <summary>
        /// Lấy màu sắc tương ứng với loại thao tác (tên màu)
        /// </summary>
        /// <param name="operationType">Loại thao tác (DeviceOperationTypeEnum)</param>
        /// <returns>Tên màu (green, blue, red, orange, purple, grey, black, v.v.)</returns>
        public static string GetOperationTypeColor(DeviceOperationTypeEnum operationType)
        {
            return operationType switch
            {
                DeviceOperationTypeEnum.Import => "green",        // Green - Nhập kho
                DeviceOperationTypeEnum.Export => "red",          // Red - Xuất kho
                DeviceOperationTypeEnum.Allocation => "blue",     // Blue - Cấp phát
                DeviceOperationTypeEnum.Recovery => "orange",      // Orange - Thu hồi
                DeviceOperationTypeEnum.Transfer => "purple",     // Purple - Chuyển giao
                DeviceOperationTypeEnum.Maintenance => "darkorange", // Dark Orange - Bảo trì
                DeviceOperationTypeEnum.StatusChange => "darkslategray", // Dark Slate Gray - Đổi trạng thái
                DeviceOperationTypeEnum.Other => "grey",          // Grey - Khác
                _ => "black"                                      // Default - Black
            };
        }

        /// <summary>
        /// Lấy tên loại thao tác từ enum
        /// </summary>
        /// <param name="operationType">Loại thao tác (int value của DeviceOperationTypeEnum)</param>
        /// <returns>Tên loại thao tác</returns>
        public static string GetOperationTypeName(int operationType)
        {
            if (!Enum.IsDefined(typeof(DeviceOperationTypeEnum), operationType))
                return "Không xác định";

            var enumValue = (DeviceOperationTypeEnum)operationType;
            return ApplicationEnumUtils.GetDescription(enumValue);
        }

        /// <summary>
        /// Lấy tên loại thao tác từ enum
        /// </summary>
        /// <param name="operationType">Loại thao tác (DeviceOperationTypeEnum)</param>
        /// <returns>Tên loại thao tác</returns>
        public static string GetOperationTypeName(DeviceOperationTypeEnum operationType)
        {
            return ApplicationEnumUtils.GetDescription(operationType);
        }
    }

    /// <summary>
    /// Converter giữa DeviceTransactionHistory entity và DeviceTransactionHistoryDto
    /// </summary>
    public static class DeviceTransactionHistoryDtoConverter
    {
        #region Entity to DTO

        /// <summary>
        /// Chuyển đổi DeviceTransactionHistory entity thành DeviceTransactionHistoryDto
        /// </summary>
        /// <param name="entity">DeviceTransactionHistory entity</param>
        /// <returns>DeviceTransactionHistoryDto</returns>
        public static DeviceTransactionHistoryDto ToDto(this Dal.DataContext.DeviceTransactionHistory entity)
        {
            if (entity == null) return null;

            var dto = new DeviceTransactionHistoryDto
            {
                Id = entity.Id,
                DeviceId = entity.DeviceId,
                OperationType = Enum.IsDefined(typeof(DeviceOperationTypeEnum), entity.OperationType)
                    ? (DeviceOperationTypeEnum)entity.OperationType
                    : DeviceOperationTypeEnum.Other, // Default to Other if invalid
                OperationDate = entity.OperationDate,
                ReferenceId = entity.ReferenceId,
                ReferenceType = entity.ReferenceType,
                Information = entity.Information,
                HtmlInformation = entity.HtmlInformation,
                PerformedBy = entity.PerformedBy,
                Notes = entity.Notes,
                CreatedDate = entity.CreatedDate,
                CreatedBy = entity.CreatedBy
            };

            // Lấy tên loại tham chiếu từ enum
            if (entity.ReferenceType.HasValue && Enum.IsDefined(typeof(DeviceReferenceTypeEnum), entity.ReferenceType.Value))
            {
                var enumValue = (DeviceReferenceTypeEnum)entity.ReferenceType.Value;
                dto.ReferenceTypeName = ApplicationEnumUtils.GetDescription(enumValue);
            }

            // Lấy thông tin thiết bị nếu có
            if (entity.Device != null)
            {
                // Lấy tên và mã sản phẩm từ ProductVariant
                if (entity.Device.ProductVariant != null)
                {
                    dto.DeviceName = entity.Device.ProductVariant.VariantFullName;
                    dto.DeviceCode = entity.Device.ProductVariant.VariantCode;
                }

                // Tạo thông tin định danh thiết bị
                var deviceInfoParts = new List<string>();
                if (!string.IsNullOrWhiteSpace(entity.Device.SerialNumber))
                    deviceInfoParts.Add($"Serial: {entity.Device.SerialNumber}");
                if (!string.IsNullOrWhiteSpace(entity.Device.IMEI))
                    deviceInfoParts.Add($"IMEI: {entity.Device.IMEI}");
                if (!string.IsNullOrWhiteSpace(entity.Device.MACAddress))
                    deviceInfoParts.Add($"MAC: {entity.Device.MACAddress}");
                if (!string.IsNullOrWhiteSpace(entity.Device.AssetTag))
                    deviceInfoParts.Add($"AssetTag: {entity.Device.AssetTag}");
                if (!string.IsNullOrWhiteSpace(entity.Device.LicenseKey))
                    deviceInfoParts.Add($"License: {entity.Device.LicenseKey}");

                if (deviceInfoParts.Any())
                {
                    dto.DeviceInfo = string.Join(" | ", deviceInfoParts);
                }
            }

            // TODO: Lấy tên người thực hiện và người tạo từ ApplicationUser hoặc Employee nếu có

            return dto;
        }

        /// <summary>
        /// Chuyển đổi danh sách DeviceTransactionHistory entities thành danh sách DeviceTransactionHistoryDto
        /// </summary>
        /// <param name="entities">Danh sách DeviceTransactionHistory entities</param>
        /// <returns>Danh sách DeviceTransactionHistoryDto</returns>
        public static List<DeviceTransactionHistoryDto> ToDtoList(this IEnumerable<Dal.DataContext.DeviceTransactionHistory> entities)
        {
            if (entities == null) return new List<DeviceTransactionHistoryDto>();
            return entities.Select(entity => entity.ToDto()).Where(dto => dto != null).ToList();
        }

        #endregion

        #region DTO to Entity

        /// <summary>
        /// Chuyển đổi DeviceTransactionHistoryDto thành DeviceTransactionHistory entity
        /// </summary>
        /// <param name="dto">DeviceTransactionHistoryDto</param>
        /// <returns>DeviceTransactionHistory entity</returns>
        public static Dal.DataContext.DeviceTransactionHistory ToEntity(this DeviceTransactionHistoryDto dto)
        {
            if (dto == null) return null;

            var entity = new Dal.DataContext.DeviceTransactionHistory
            {
                Id = dto.Id == Guid.Empty ? Guid.NewGuid() : dto.Id,
                DeviceId = dto.DeviceId,
                OperationType = (int)dto.OperationType, // Convert enum to int for entity
                OperationDate = dto.OperationDate == default(DateTime) ? DateTime.Now : dto.OperationDate,
                ReferenceId = dto.ReferenceId,
                ReferenceType = dto.ReferenceType,
                Information = dto.Information,
                HtmlInformation = dto.HtmlInformation,
                PerformedBy = dto.PerformedBy,
                Notes = dto.Notes,
                CreatedDate = dto.CreatedDate == default(DateTime) ? DateTime.Now : dto.CreatedDate,
                CreatedBy = dto.CreatedBy
            };

            return entity;
        }

        #endregion
    }

}
