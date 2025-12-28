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
        /// <param name="deviceName">Tên thiết bị (tùy chọn, để tránh DataContext disposed errors)</param>
        /// <param name="deviceCode">Mã thiết bị (tùy chọn, để tránh DataContext disposed errors)</param>
        /// <param name="deviceInfo">Thông tin thiết bị (tùy chọn, để tránh DataContext disposed errors)</param>
        /// <param name="performedByName">Tên người thực hiện (tùy chọn, để tránh DataContext disposed errors)</param>
        /// <param name="createdByName">Tên người tạo (tùy chọn, để tránh DataContext disposed errors)</param>
        /// <returns>DeviceTransactionHistoryDto</returns>
        public static DeviceTransactionHistoryDto ToDto(this Dal.DataContext.DeviceTransactionHistory entity,
            string deviceName = null,
            string deviceCode = null,
            string deviceInfo = null,
            string performedByName = null,
            string createdByName = null)
        {
            if (entity == null) return null;

            // QUAN TRỌNG: KHÔNG truy cập navigation properties (entity.Device, entity.Device.ProductVariant)
            // vì DataContext đã bị dispose sau khi repository method kết thúc
            // Chỉ sử dụng tham số truyền vào

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
                CreatedBy = entity.CreatedBy,
                // Navigation properties - sử dụng giá trị từ tham số truyền vào
                DeviceName = deviceName,
                DeviceCode = deviceCode,
                DeviceInfo = deviceInfo,
                PerformedByName = performedByName,
                CreatedByName = createdByName
            };

            // Lấy tên loại tham chiếu từ enum
            if (entity.ReferenceType.HasValue && Enum.IsDefined(typeof(DeviceReferenceTypeEnum), entity.ReferenceType.Value))
            {
                var enumValue = (DeviceReferenceTypeEnum)entity.ReferenceType.Value;
                dto.ReferenceTypeName = ApplicationEnumUtils.GetDescription(enumValue);
            }

            return dto;
        }

        /// <summary>
        /// Chuyển đổi danh sách DeviceTransactionHistory entities thành danh sách DeviceTransactionHistoryDto
        /// </summary>
        /// <param name="entities">Danh sách DeviceTransactionHistory entities</param>
        /// <param name="deviceDict">Dictionary chứa thông tin Device (key: DeviceId, value: (DeviceName, DeviceCode, DeviceInfo))</param>
        /// <param name="performedByDict">Dictionary chứa tên người thực hiện (key: PerformedBy, value: PerformedByName)</param>
        /// <param name="createdByDict">Dictionary chứa tên người tạo (key: CreatedBy, value: CreatedByName)</param>
        /// <returns>Danh sách DeviceTransactionHistoryDto</returns>
        public static List<DeviceTransactionHistoryDto> ToDtoList(this IEnumerable<Dal.DataContext.DeviceTransactionHistory> entities,
            Dictionary<Guid, (string DeviceName, string DeviceCode, string DeviceInfo)> deviceDict = null,
            Dictionary<Guid, string> performedByDict = null,
            Dictionary<Guid, string> createdByDict = null)
        {
            if (entities == null) return new List<DeviceTransactionHistoryDto>();

            return entities.Select(entity =>
            {
                string deviceName = null;
                string deviceCode = null;
                string deviceInfo = null;
                string performedByName = null;
                string createdByName = null;

                if (deviceDict != null && deviceDict.TryGetValue(entity.DeviceId, out var deviceInfoTuple))
                {
                    deviceName = deviceInfoTuple.DeviceName;
                    deviceCode = deviceInfoTuple.DeviceCode;
                    deviceInfo = deviceInfoTuple.DeviceInfo;
                }

                if (performedByDict != null && entity.PerformedBy.HasValue && 
                    performedByDict.TryGetValue(entity.PerformedBy.Value, out var performedName))
                {
                    performedByName = performedName;
                }

                if (createdByDict != null && entity.CreatedBy.HasValue && 
                    createdByDict.TryGetValue(entity.CreatedBy.Value, out var createdName))
                {
                    createdByName = createdName;
                }

                return entity.ToDto(deviceName, deviceCode, deviceInfo, performedByName, createdByName);
            }).Where(dto => dto != null).ToList();
        }

        #endregion

        #region DTO to Entity

        /// <summary>
        /// Chuyển đổi DeviceTransactionHistoryDto thành DeviceTransactionHistory entity
        /// </summary>
        /// <param name="dto">DeviceTransactionHistoryDto</param>
        /// <param name="existingEntity">Existing entity to update (optional, for edit mode)</param>
        /// <returns>DeviceTransactionHistory entity</returns>
        public static Dal.DataContext.DeviceTransactionHistory ToEntity(this DeviceTransactionHistoryDto dto, Dal.DataContext.DeviceTransactionHistory existingEntity = null)
        {
            if (dto == null) return null;

            Dal.DataContext.DeviceTransactionHistory entity;
            if (existingEntity != null)
            {
                // Update existing entity
                entity = existingEntity;
            }
            else
            {
                // Create new entity
                entity = new Dal.DataContext.DeviceTransactionHistory();
                if (dto.Id != Guid.Empty)
                {
                    entity.Id = dto.Id;
                }
            }

            // Map properties
            entity.DeviceId = dto.DeviceId;
            entity.OperationType = (int)dto.OperationType; // Convert enum to int for entity
            entity.OperationDate = dto.OperationDate == default(DateTime) ? DateTime.Now : dto.OperationDate;
            entity.ReferenceId = dto.ReferenceId;
            entity.ReferenceType = dto.ReferenceType;
            entity.Information = dto.Information;
            entity.HtmlInformation = dto.HtmlInformation;
            entity.PerformedBy = dto.PerformedBy;
            entity.Notes = dto.Notes;

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
