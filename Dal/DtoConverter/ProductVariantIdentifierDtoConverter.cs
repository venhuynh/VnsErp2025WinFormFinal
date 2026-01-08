using DTO.Inventory.InventoryManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using ProductVariantIdentifier = Dal.DataContext.ProductVariantIdentifier;

namespace Dal.DtoConverter
{
    /// <summary>
    /// Converter giữa ProductVariantIdentifier entity và ProductVariantIdentifierDto
    /// </summary>
    public static class ProductVariantIdentifierDtoConverter
    {
        #region Entity to DTO

        /// <summary>
        /// Chuyển đổi ProductVariantIdentifier entity thành ProductVariantIdentifierDto
        /// </summary>
        /// <param name="entity">ProductVariantIdentifier entity</param>
        /// <param name="productVariantFullName">Tên biến thể sản phẩm đầy đủ (tùy chọn, để tránh DataContext disposed errors)</param>
        /// <param name="customerCategory">Phân loại khách hàng (tùy chọn)</param>
        /// <returns>ProductVariantIdentifierDto</returns>
        public static ProductVariantIdentifierDto ToDto(this ProductVariantIdentifier entity,
            string productVariantFullName = null,
            string customerCategory = null)
        {
            if (entity == null) return null;

            // QUAN TRỌNG: KHÔNG truy cập navigation properties (entity.ProductVariant)
            // vì DataContext đã bị dispose sau khi repository method kết thúc
            // Chỉ sử dụng tham số truyền vào

            var dto = new ProductVariantIdentifierDto
            {
                // Thông tin cơ bản
                Id = entity.Id,
                ProductVariantId = entity.ProductVariantId,
                ProductVariantFullName = productVariantFullName,
                CustomerCategory = customerCategory,

                // Định danh sản phẩm
                SerialNumber = entity.SerialNumber,
                PartNumber = entity.Barcode,
                QRCode = entity.QRCode,
                SKU = entity.SKU,
                RFID = entity.RFID,
                MACAddress = entity.MACAddress,
                IMEI = entity.IMEI,
                AssetTag = entity.AssetTag,
                LicenseKey = entity.LicenseKey,
                UPC = entity.UPC,
                EAN = entity.EAN,
                ID = entity.ISBN, // Map to ISBN column, not entity.Id
                OtherIdentifier = entity.OtherIdentifier,

                // Quản lý hình ảnh QR code
                QRCodeImagePath = entity.QRCodeImagePath,
                QRCodeImageFullPath = entity.QRCodeImageFullPath,
                QRCodeImageFileName = entity.QRCodeImageFileName,
                QRCodeImageStorageType = entity.QRCodeImageStorageType,
                QRCodeImageLocked = entity.QRCodeImageLocked,
                QRCodeImageLockedDate = entity.QRCodeImageLockedDate,
                QRCodeImageLockedBy = entity.QRCodeImageLockedBy,
                QRCodeImage = entity.QRCodeImage?.ToArray(), // Convert Binary sang byte[]

                // Tình trạng hàng hóa/sản phẩm
                Status = (ProductVariantIdentifierStatusEnum)entity.Status, // Convert int to enum
                StatusDate = entity.StatusDate,
                StatusChangedBy = entity.StatusChangedBy,
                StatusNotes = entity.StatusNotes,

                // Thông tin khác
                IsActive = entity.IsActive,
                SourceType = entity.SourceType,
                SourceReference = entity.SourceReference,
                ValidFrom = entity.ValidFrom,
                ValidTo = entity.ValidTo,
                Notes = entity.Notes,

                // Audit fields
                CreatedDate = entity.CreatedDate,
                UpdatedDate = entity.UpdatedDate,
                CreatedBy = entity.CreatedBy,
                UpdatedBy = entity.UpdatedBy
            };

            return dto;
        }

        /// <summary>
        /// Chuyển đổi danh sách ProductVariantIdentifier entities thành danh sách ProductVariantIdentifierDto
        /// </summary>
        /// <param name="entities">Danh sách ProductVariantIdentifier entities</param>
        /// <param name="productVariantDict">Dictionary chứa thông tin ProductVariant (key: ProductVariantId, value: (ProductVariantFullName, CustomerCategory))</param>
        /// <returns>Danh sách ProductVariantIdentifierDto</returns>
        public static List<ProductVariantIdentifierDto> ToDtoList(this IEnumerable<ProductVariantIdentifier> entities,
            Dictionary<Guid, (string ProductVariantFullName, string CustomerCategory)> productVariantDict = null)
        {
            if (entities == null) return new List<ProductVariantIdentifierDto>();

            return entities.Select(entity =>
            {
                string productVariantFullName = null;
                string customerCategory = null;

                if (productVariantDict != null && productVariantDict.TryGetValue(entity.ProductVariantId, out var productInfo))
                {
                    productVariantFullName = productInfo.ProductVariantFullName;
                    customerCategory = productInfo.CustomerCategory;
                }

                return entity.ToDto(productVariantFullName, customerCategory);
            }).ToList();
        }

        /// <summary>
        /// Chuyển đổi danh sách ProductVariantIdentifier entities thành danh sách ProductVariantIdentifierDto (alias)
        /// </summary>
        /// <param name="entities">Danh sách ProductVariantIdentifier entities</param>
        /// <returns>Danh sách ProductVariantIdentifierDto</returns>
        public static List<ProductVariantIdentifierDto> ToDtos(this IEnumerable<ProductVariantIdentifier> entities)
        {
            if (entities == null) return new List<ProductVariantIdentifierDto>();
            return entities.Select(e => e.ToDto(null, null)).ToList();
        }

        #endregion

        #region DTO to Entity

        /// <summary>
        /// Chuyển đổi ProductVariantIdentifierDto thành ProductVariantIdentifier entity
        /// </summary>
        /// <param name="dto">ProductVariantIdentifierDto</param>
        /// <param name="existingEntity">Existing entity to update (optional, for edit mode)</param>
        /// <returns>ProductVariantIdentifier entity</returns>
        public static ProductVariantIdentifier ToEntity(this ProductVariantIdentifierDto dto, ProductVariantIdentifier existingEntity = null)
        {
            if (dto == null) return null;

            ProductVariantIdentifier entity;
            if (existingEntity != null)
            {
                // Update existing entity
                entity = existingEntity;
            }
            else
            {
                // Create new entity
                entity = new ProductVariantIdentifier();
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
            entity.ProductVariantId = dto.ProductVariantId;

            // Map properties - Định danh sản phẩm
            entity.SerialNumber = dto.SerialNumber;
            entity.Barcode = dto.PartNumber;
            entity.QRCode = dto.QRCode;
            entity.SKU = dto.SKU;
            entity.RFID = dto.RFID;
            entity.MACAddress = dto.MACAddress;
            entity.IMEI = dto.IMEI;
            entity.AssetTag = dto.AssetTag;
            entity.LicenseKey = dto.LicenseKey;
            entity.UPC = dto.UPC;
            entity.EAN = dto.EAN;
            entity.ISBN = dto.ID; // Map from DTO.ID to entity.ISBN (not entity.Id)
            entity.OtherIdentifier = dto.OtherIdentifier;

            // Map properties - Quản lý hình ảnh QR code
            // Chỉ cập nhật nếu hình ảnh chưa bị khóa
            if (!entity.QRCodeImageLocked)
            {
                entity.QRCodeImagePath = dto.QRCodeImagePath;
                entity.QRCodeImageFullPath = dto.QRCodeImageFullPath;
                entity.QRCodeImageFileName = dto.QRCodeImageFileName;
                entity.QRCodeImageStorageType = dto.QRCodeImageStorageType;
                // Chuyển đổi QRCodeImage từ byte[] sang System.Data.Linq.Binary
                if (dto.QRCodeImage != null && dto.QRCodeImage.Length > 0)
                {
                    entity.QRCodeImage = new System.Data.Linq.Binary(dto.QRCodeImage);
                }
                else
                {
                    entity.QRCodeImage = null;
                }
            }
            // Cập nhật thông tin khóa (có thể được thay đổi bởi admin)
            entity.QRCodeImageLocked = dto.QRCodeImageLocked;
            entity.QRCodeImageLockedDate = dto.QRCodeImageLockedDate;
            entity.QRCodeImageLockedBy = dto.QRCodeImageLockedBy;

            // Map properties - Tình trạng hàng hóa/sản phẩm
            entity.Status = (int)dto.Status; // Convert enum to int
            entity.StatusDate = dto.StatusDate;
            entity.StatusChangedBy = dto.StatusChangedBy;
            entity.StatusNotes = dto.StatusNotes;

            // Map properties - Thông tin khác
            entity.IsActive = dto.IsActive;
            entity.SourceType = dto.SourceType;
            entity.SourceReference = dto.SourceReference;
            entity.ValidFrom = dto.ValidFrom;
            entity.ValidTo = dto.ValidTo;
            entity.Notes = dto.Notes;

            // Map properties - Audit fields
            entity.UpdatedDate = dto.UpdatedDate ?? DateTime.Now;
            entity.UpdatedBy = dto.UpdatedBy;

            if (existingEntity == null)
            {
                // Chỉ set CreatedDate và CreatedBy khi tạo mới
                entity.CreatedDate = dto.CreatedDate == default(DateTime) ? DateTime.Now : dto.CreatedDate;
                entity.CreatedBy = dto.CreatedBy;
            }

            return entity;
        }

        /// <summary>
        /// Chuyển đổi danh sách ProductVariantIdentifierDto thành danh sách ProductVariantIdentifier entities
        /// </summary>
        /// <param name="dtos">Danh sách ProductVariantIdentifierDto</param>
        /// <returns>Danh sách ProductVariantIdentifier entities</returns>
        public static List<ProductVariantIdentifier> ToEntityList(this IEnumerable<ProductVariantIdentifierDto> dtos)
        {
            if (dtos == null) return new List<ProductVariantIdentifier>();
            return dtos.Select(d => d.ToEntity()).Where(e => e != null).ToList();
        }

        /// <summary>
        /// Chuyển đổi danh sách ProductVariantIdentifierDto thành danh sách ProductVariantIdentifier entities (alias)
        /// </summary>
        /// <param name="dtos">Danh sách ProductVariantIdentifierDto</param>
        /// <returns>Danh sách ProductVariantIdentifier entities</returns>
        public static List<ProductVariantIdentifier> ToEntities(this IEnumerable<ProductVariantIdentifierDto> dtos)
        {
            if (dtos == null) return new List<ProductVariantIdentifier>();
            return dtos.Select(d => d.ToEntity()).Where(e => e != null).ToList();
        }

        #endregion
    }
}
