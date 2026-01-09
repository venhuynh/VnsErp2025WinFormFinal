using Dal.DataContext;
using DTO.MasterData.CustomerPartner;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dal.DtoConverter.MasterData.Customer
{

    /// <summary>
    /// Converter cho BusinessPartner Entity và DTO
    /// </summary>
    public static class BusinessPartnerConverters
    {
        /// <summary>
        /// Chuyển đổi BusinessPartner Entity sang BusinessPartnerListDto
        /// Sử dụng navigation properties đã được load trong repository
        /// </summary>
        /// <param name="entity">BusinessPartner Entity</param>
        /// <param name="categoryDict">Dictionary chứa tất cả BusinessPartnerCategory entities để tính FullPath (optional, chỉ cần khi tính full path của category)</param>
        /// <returns>BusinessPartnerListDto</returns>
        public static BusinessPartnerListDto ToListDto(this BusinessPartner entity,
            Dictionary<Guid, BusinessPartnerCategory> categoryDict = null)
        {
            if (entity == null) return null;

            System.Diagnostics.Debug.WriteLine(
                $"[ToListDto] Bắt đầu convert partner {entity.Id} ({entity.PartnerCode})");

            var dto = new BusinessPartnerListDto
            {
                Id = entity.Id,
                PartnerCode = entity.PartnerCode,
                PartnerName = entity.PartnerName,
                PartnerType = entity.PartnerType,
                PartnerTypeName = ResolvePartnerTypeName(entity.PartnerType),
                TaxCode = entity.TaxCode,
                Phone = entity.Phone,
                Email = entity.Email,
                Website = entity.Website,
                Address = entity.Address,
                City = entity.City,
                Country = entity.Country,
                IsActive = entity.IsActive,
                CreatedDate = entity.CreatedDate,
                UpdatedDate = entity.UpdatedDate,
                CreatedBy = entity.CreatedBy,
                ModifiedBy = entity.ModifiedBy,
                LogoFileName = entity.LogoFileName,
                LogoRelativePath = entity.LogoRelativePath,
                LogoFullPath = entity.LogoFullPath,
                LogoStorageType = entity.LogoStorageType,
                LogoFileSize = entity.LogoFileSize,
                LogoChecksum = entity.LogoChecksum,
                LogoThumbnailData = entity.LogoThumbnailData?.ToArray()
            };

            // Sử dụng navigation properties trực tiếp với try-catch chi tiết
            try
            {
                System.Diagnostics.Debug.WriteLine(
                    $"[ToListDto] Đang truy cập ApplicationUser cho partner {entity.Id}");
                var createdByUser = entity.ApplicationUser;
                dto.CreatedByName = createdByUser?.UserName;
                System.Diagnostics.Debug.WriteLine($"[ToListDto] Đã lấy CreatedByName: {dto.CreatedByName}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    $"[ToListDto] LỖI khi truy cập ApplicationUser cho partner {entity.Id}: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"[ToListDto] StackTrace: {ex.StackTrace}");
                dto.CreatedByName = null;
            }

            try
            {
                System.Diagnostics.Debug.WriteLine(
                    $"[ToListDto] Đang truy cập ApplicationUser2 cho partner {entity.Id}");
                var modifiedByUser = entity.ApplicationUser2;
                dto.ModifiedByName = modifiedByUser?.UserName;
                System.Diagnostics.Debug.WriteLine($"[ToListDto] Đã lấy ModifiedByName: {dto.ModifiedByName}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    $"[ToListDto] LỖI khi truy cập ApplicationUser2 cho partner {entity.Id}: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"[ToListDto] StackTrace: {ex.StackTrace}");
                dto.ModifiedByName = null;
            }

            // Tính CategoryNames và CategoryPaths từ navigation properties
            try
            {
                System.Diagnostics.Debug.WriteLine(
                    $"[ToListDto] Đang truy cập BusinessPartner_BusinessPartnerCategories cho partner {entity.Id}");
                var categories = entity.BusinessPartner_BusinessPartnerCategories;
                System.Diagnostics.Debug.WriteLine($"[ToListDto] Categories is null: {categories == null}");

                if (categories != null && categories.Count > 0)
                {
                    System.Diagnostics.Debug.WriteLine($"[ToListDto] Có {categories.Count} categories");
                    var categoryNames = new List<string>();
                    var categoryPaths = new List<string>();

                    int index = 0;
                    foreach (var mapping in categories)
                    {
                        try
                        {
                            System.Diagnostics.Debug.WriteLine($"[ToListDto] Đang xử lý category mapping {index}");
                            var category = mapping.BusinessPartnerCategory;
                            if (category == null)
                            {
                                System.Diagnostics.Debug.WriteLine($"[ToListDto] Category là null cho mapping {index}");
                                continue;
                            }

                            System.Diagnostics.Debug.WriteLine($"[ToListDto] Category name: {category.CategoryName}");
                            categoryNames.Add(category.CategoryName);

                            // Tính FullPath: chỉ dùng categoryDict nếu có, nếu không chỉ dùng CategoryName
                            // KHÔNG tính từ navigation properties vì sẽ gây lỗi "Cannot access a disposed object"
                            string fullPath;
                            if (categoryDict != null && categoryDict.ContainsKey(category.Id))
                            {
                                fullPath = CalculateCategoryFullPath(category, categoryDict);
                                System.Diagnostics.Debug.WriteLine(
                                    $"[ToListDto] Full path từ categoryDict: {fullPath}");
                            }
                            else
                            {
                                // Không có categoryDict, chỉ dùng CategoryName
                                // KHÔNG cố tính từ navigation properties vì DataContext đã bị dispose
                                fullPath = category.CategoryName;
                                System.Diagnostics.Debug.WriteLine(
                                    $"[ToListDto] Không có categoryDict, dùng CategoryName: {fullPath}");
                            }

                            categoryPaths.Add(fullPath);
                            index++;
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine(
                                $"[ToListDto] LỖI khi xử lý category mapping {index}: {ex.Message}");
                            System.Diagnostics.Debug.WriteLine($"[ToListDto] StackTrace: {ex.StackTrace}");
                        }
                    }

                    dto.CategoryNames = string.Join(", ", categoryNames);
                    dto.CategoryPaths = string.Join(", ", categoryPaths);
                    dto.FullPath = categoryPaths.FirstOrDefault();
                    System.Diagnostics.Debug.WriteLine($"[ToListDto] Đã xử lý xong categories cho partner {entity.Id}");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"[ToListDto] Không có categories cho partner {entity.Id}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    $"[ToListDto] LỖI TỔNG QUÁT khi xử lý categories cho partner {entity.Id}: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"[ToListDto] StackTrace: {ex.StackTrace}");
            }

            System.Diagnostics.Debug.WriteLine($"[ToListDto] Hoàn thành convert partner {entity.Id}");
            return dto;
        }

        /// <summary>
        /// Chuyển đổi danh sách BusinessPartner Entity sang danh sách BusinessPartnerListDto
        /// </summary>
        /// <param name="entities">Danh sách BusinessPartner Entity</param>
        /// <param name="categoryDict">Dictionary chứa tất cả BusinessPartnerCategory entities để tính FullPath (optional)</param>
        /// <returns>Danh sách BusinessPartnerListDto</returns>
        public static IEnumerable<BusinessPartnerListDto> ToBusinessPartnerListDtos(
            this IEnumerable<BusinessPartner> entities,
            Dictionary<Guid, BusinessPartnerCategory> categoryDict = null)
        {
            if (entities == null) return [];

            return entities.Select(entity => entity.ToListDto(categoryDict));
        }

        /// <summary>
        /// Chuyển đổi BusinessPartner Entity sang BusinessPartnerDetailDto
        /// </summary>
        /// <param name="entity">BusinessPartner Entity</param>
        /// <returns>BusinessPartnerDetailDto</returns>
        public static BusinessPartnerDetailDto ToDetailDto(this BusinessPartner entity)
        {
            if (entity == null) return null;

            return new BusinessPartnerDetailDto
            {
                Id = entity.Id,
                PartnerCode = entity.PartnerCode,
                PartnerName = entity.PartnerName,
                PartnerType = entity.PartnerType,
                TaxCode = entity.TaxCode,
                Phone = entity.Phone,
                Email = entity.Email,
                Website = entity.Website,
                Address = entity.Address,
                City = entity.City,
                Country = entity.Country,
                IsActive = entity.IsActive,
                CreatedDate = entity.CreatedDate,
                UpdatedDate = entity.UpdatedDate,
                LogoFileName = entity.LogoFileName,
                LogoRelativePath = entity.LogoRelativePath,
                LogoFullPath = entity.LogoFullPath,
                LogoStorageType = entity.LogoStorageType,
                LogoFileSize = entity.LogoFileSize,
                LogoChecksum = entity.LogoChecksum,
                LogoThumbnailData = entity.LogoThumbnailData?.ToArray()
            };
        }

        /// <summary>
        /// Chuyển đổi BusinessPartnerDetailDto sang BusinessPartner Entity
        /// </summary>
        /// <param name="dto">BusinessPartnerDetailDto</param>
        /// <param name="existingEntity">Entity hiện tại (cho update)</param>
        /// <returns>BusinessPartner Entity</returns>
        public static BusinessPartner ToEntity(this BusinessPartnerDetailDto dto, BusinessPartner existingEntity = null)
        {
            if (dto == null) return null;

            var entity = existingEntity ?? new BusinessPartner();

            // Chỉ set ID nếu là entity mới
            if (existingEntity == null && dto.Id != Guid.Empty)
            {
                entity.Id = dto.Id;
            }

            entity.PartnerCode = dto.PartnerCode;
            entity.PartnerName = dto.PartnerName;
            entity.PartnerType = dto.PartnerType;
            entity.TaxCode = dto.TaxCode;
            entity.Phone = dto.Phone;
            entity.Email = dto.Email;
            entity.Website = dto.Website;
            entity.Address = dto.Address;
            entity.City = dto.City;
            entity.Country = dto.Country;
            entity.IsActive = dto.IsActive;
            entity.CreatedDate = dto.CreatedDate;
            entity.UpdatedDate = dto.UpdatedDate;

            // Copy Logo fields (metadata only)
            entity.LogoFileName = dto.LogoFileName;
            entity.LogoRelativePath = dto.LogoRelativePath;
            entity.LogoFullPath = dto.LogoFullPath;
            entity.LogoStorageType = dto.LogoStorageType;
            entity.LogoFileSize = dto.LogoFileSize;
            entity.LogoChecksum = dto.LogoChecksum;

            // Copy LogoThumbnailData (binary)
            if (dto.LogoThumbnailData != null && dto.LogoThumbnailData.Length > 0)
            {
                entity.LogoThumbnailData = new System.Data.Linq.Binary(dto.LogoThumbnailData);
            }
            else
            {
                entity.LogoThumbnailData = null;
            }

            return entity;
        }

        /// <summary>
        /// Resolve tên loại đối tác từ PartnerType
        /// </summary>
        /// <param name="partnerType">PartnerType (int)</param>
        /// <returns>Tên loại đối tác</returns>
        private static string ResolvePartnerTypeName(int partnerType)
        {
            return partnerType switch
            {
                1 => "Khách hàng",
                2 => "Nhà cung cấp",
                3 => "Cả hai",
                _ => "Không xác định"
            };
        }

        /// <summary>
        /// Tính toán đường dẫn đầy đủ từ gốc đến category sử dụng categoryDict
        /// </summary>
        /// <param name="category">BusinessPartnerCategory entity</param>
        /// <param name="categoryDict">Dictionary chứa tất cả BusinessPartnerCategory entities</param>
        /// <returns>Đường dẫn đầy đủ (ví dụ: "Danh mục A > Danh mục A1")</returns>
        private static string CalculateCategoryFullPath(BusinessPartnerCategory category,
            Dictionary<Guid, BusinessPartnerCategory> categoryDict)
        {
            if (category == null) return string.Empty;

            var pathParts = new List<string> { category.CategoryName };
            var current = category;

            while (current.ParentId.HasValue && categoryDict.ContainsKey(current.ParentId.Value))
            {
                current = categoryDict[current.ParentId.Value];
                pathParts.Insert(0, current.CategoryName);
                if (pathParts.Count > 10) break; // Tránh infinite loop
            }

            return string.Join(" > ", pathParts);
        }
    }
}

/// <summary>
/// Converter cho BusinessPartner Entity và BusinessPartnerLookupDto
/// </summary>
public static class BusinessPartnerLookupConverters
{
    /// <summary>
    /// Chuyển đổi BusinessPartner Entity sang BusinessPartnerLookupDto
    /// DTO tối giản chỉ chứa thông tin cần thiết cho SearchLookUpEdit
    /// </summary>
    /// <param name="entity">BusinessPartner Entity</param>
    /// <returns>BusinessPartnerLookupDto</returns>
    public static BusinessPartnerLookupDto ToLookupDto(this BusinessPartner entity)
    {
        if (entity == null) return null;

        return new BusinessPartnerLookupDto
        {
            Id = entity.Id,
            PartnerCode = entity.PartnerCode,
            PartnerType = entity.PartnerType,
            PartnerName = entity.PartnerName,
            LogoThumbnailData = entity.LogoThumbnailData?.ToArray()
        };
    }
}
