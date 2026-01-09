using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using Common.Utils;
using Dal.DataContext;
using DTO.Inventory.InventoryManagement;

namespace Dal.DtoConverter.Inventory.Management
{
    /// <summary>
    /// Converter giữa StockInOutMaster entity và StockInOutMasterHistoryDto
    /// </summary>
    public static class StockInOutMasterHistoryDtoConverter
    {

        /// <summary>
        /// Chuyển đổi StockInOutMaster entity thành StockInOutMasterHistoryDto
        /// </summary>
        /// <param name="entity">StockInOutMaster entity</param>
        /// <returns>StockInOutMasterHistoryDto</returns>
        public static StockInOutMasterHistoryDto ToStockInOutMasterHistoryDto(this StockInOutMaster entity)
        {
            if (entity == null) return null;

            // Chuyển đổi StockInOutType (int) sang LoaiNhapXuatKhoEnum
            var loaiNhapXuatKho = ApplicationEnumUtils.GetEnumValue<LoaiNhapXuatKhoEnum>(entity.StockInOutType);
            var loaiNhapXuatKhoName = ApplicationEnumUtils.GetDescription(loaiNhapXuatKho);

            var dto = new StockInOutMasterHistoryDto
            {
                // Thông tin cơ bản
                Id = entity.Id,
                StockInOutDate = entity.StockInOutDate,
                VocherNumber = entity.VocherNumber ?? string.Empty,
                StockInOutType = entity.StockInOutType,
                LoaiNhapXuatKho = loaiNhapXuatKho,
                LoaiNhapXuatKhoName = loaiNhapXuatKhoName,

                // Thông tin liên kết
                WarehouseId = entity.WarehouseId,
                PurchaseOrderId = entity.PurchaseOrderId,
                PartnerSiteId = entity.PartnerSiteId,

                // Thông tin bổ sung
                Notes = entity.Notes ?? string.Empty,
                NguoiNhanHang = entity.NguoiNhanHang ?? string.Empty,
                NguoiGiaoHang = entity.NguoiGiaoHang ?? string.Empty,

                // Tổng hợp
                TotalQuantity = entity.TotalQuantity,
                TotalAmount = entity.TotalAmount,
                TotalVat = entity.TotalVat,
                TotalAmountIncludedVat = entity.TotalAmountIncludedVat,

                // Thông tin hệ thống
                CreatedBy = entity.CreatedBy,
                CreatedDate = entity.CreatedDate,
                UpdatedBy = entity.UpdatedBy,
                UpdatedDate = entity.UpdatedDate,

                // Hiển thị
                DetailsSummary = null // Có thể được tính toán từ StockInOutDetails nếu cần
            };

            // Map navigation properties nếu đã được load trong entity
            // Sử dụng try-catch để tránh DataContext disposed errors
            try
            {
                // Warehouse (CompanyBranch)
                if (entity.CompanyBranch != null)
                {
                    dto.WarehouseName = entity.CompanyBranch.BranchName ?? string.Empty;
                }

                // Customer (BusinessPartnerSite)
                if (entity.BusinessPartnerSite != null)
                {
                    dto.CustomerName = entity.BusinessPartnerSite.SiteName ?? string.Empty;
                }

                // Tính DetailsSummary từ StockInOutDetails nếu đã được load
                if (entity.StockInOutDetails != null && entity.StockInOutDetails.Any())
                {
                    dto.DetailsSummary = ComputeDetailsSummary(entity.StockInOutDetails);
                }
            }
            catch
            {
                // Nếu không thể load navigation properties (DataContext disposed), để giá trị mặc định
                // Các giá trị đã được set ở trên sẽ được giữ nguyên
            }

            return dto;
        }

        /// <summary>
        /// Chuyển đổi danh sách StockInOutMaster entities thành danh sách StockInOutMasterHistoryDto
        /// </summary>
        /// <param name="entities">Danh sách StockInOutMaster entities</param>
        /// <returns>Danh sách StockInOutMasterHistoryDto</returns>
        public static List<StockInOutMasterHistoryDto> ToStockInOutMasterHistoryDtos(this IEnumerable<StockInOutMaster> entities)
        {
            if (entities == null) return new List<StockInOutMasterHistoryDto>();

            return entities.Select(entity => entity.ToStockInOutMasterHistoryDto())
                .Where(dto => dto != null)
                .ToList();
        }


        #region Helper Methods

        /// <summary>
        /// Tính toán DetailsSummary từ danh sách StockInOutDetails
        /// Ví dụ: "3 sản phẩm: iPhone 15 Pro (2), MacBook Pro (1)"
        /// </summary>
        /// <param name="details">Danh sách StockInOutDetail</param>
        /// <returns>Chuỗi mô tả tóm tắt</returns>
        private static string ComputeDetailsSummary(EntitySet<StockInOutDetail> details)
        {
            if (details == null || !details.Any())
                return null;

            try
            {
                // Nhóm theo ProductVariant và đếm số lượng
                var grouped = details
                    .Where(d => d.ProductVariant != null)
                    .GroupBy(d => new
                    {
                        ProductName = d.ProductVariant?.ProductService?.Name ?? d.ProductVariant?.VariantCode ?? "Không xác định",
                        VariantCode = d.ProductVariant?.VariantCode ?? string.Empty
                    })
                    .Select(g => new
                    {
                        ProductName = g.Key.ProductName,
                        VariantCode = g.Key.VariantCode,
                        TotalQty = g.Sum(d => d.StockInQty + d.StockOutQty)
                    })
                    .ToList();

                if (!grouped.Any())
                    return null;

                var totalItems = grouped.Sum(g => (int)g.TotalQty);
                var itemDescriptions = grouped
                    .Select(g => $"{g.ProductName}{(string.IsNullOrWhiteSpace(g.VariantCode) ? "" : $" ({g.VariantCode})")} ({(int)g.TotalQty})")
                    .Take(5) // Chỉ lấy 5 sản phẩm đầu tiên
                    .ToList();

                var summary = $"{totalItems} sản phẩm";
                if (itemDescriptions.Any())
                {
                    summary += ": " + string.Join(", ", itemDescriptions);
                    if (grouped.Count > 5)
                    {
                        summary += $", ... (+{grouped.Count - 5} sản phẩm khác)";
                    }
                }

                return summary;
            }
            catch
            {
                // Nếu có lỗi khi tính toán (DataContext disposed, v.v.), trả về null
                return null;
            }
        }

        #endregion
    }
}