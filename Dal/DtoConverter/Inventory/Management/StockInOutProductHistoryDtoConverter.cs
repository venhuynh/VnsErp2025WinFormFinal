using System;
using System.Collections.Generic;
using System.Linq;
using Common.Utils;
using Dal.DataContext;
using DTO.Inventory.InventoryManagement;

namespace Dal.DtoConverter.Inventory.Management
{
    /// <summary>
    /// Converter giữa StockInOutDetail entity và StockInOutProductHistoryDto
    /// </summary>
    public static class StockInOutProductHistoryDtoConverter
    {
        /// <summary>
        /// Chuyển đổi StockInOutDetail entity thành StockInOutProductHistoryDto
        /// </summary>
        /// <param name="entity">StockInOutDetail entity</param>
        /// <returns>StockInOutProductHistoryDto</returns>
        public static StockInOutProductHistoryDto ToDto(this StockInOutDetail entity)
        {
            if (entity == null) return null;

            // Chuyển đổi StockInOutType (int) sang LoaiNhapXuatKhoEnum từ Master
            LoaiNhapXuatKhoEnum loaiNhapXuatKho = LoaiNhapXuatKhoEnum.NhapHangThuongMai;
            string loaiNhapXuatKhoName = string.Empty;
            string vocherNumber = string.Empty;
            DateTime stockInOutDate = DateTime.Now;
            Guid warehouseId = Guid.Empty;
            string warehouseName = string.Empty;
            Guid? partnerSiteId = null;
            string customerName = string.Empty;

            // Lấy thông tin từ StockInOutMaster nếu đã được load
            try
            {
                if (entity.StockInOutMaster != null)
                {
                    var master = entity.StockInOutMaster;
                    loaiNhapXuatKho = ApplicationEnumUtils.GetEnumValue<LoaiNhapXuatKhoEnum>(master.StockInOutType);
                    loaiNhapXuatKhoName = ApplicationEnumUtils.GetDescription(loaiNhapXuatKho);
                    vocherNumber = master.VocherNumber ?? string.Empty;
                    stockInOutDate = master.StockInOutDate;
                    warehouseId = master.WarehouseId;
                    partnerSiteId = master.PartnerSiteId;

                    // Warehouse (CompanyBranch)
                    if (master.CompanyBranch != null)
                    {
                        warehouseName = master.CompanyBranch.BranchName ?? string.Empty;
                    }

                    // Customer (BusinessPartnerSite)
                    if (master.BusinessPartnerSite != null)
                    {
                        customerName = master.BusinessPartnerSite.SiteName ?? string.Empty;
                    }
                }
            }
            catch
            {
                // Nếu không thể load navigation properties (DataContext disposed), để giá trị mặc định
            }

            // Lấy thông tin từ ProductVariant nếu đã được load
            string productCode = string.Empty;
            string productName = string.Empty;
            string productVariantCode = string.Empty;
            string productVariantFullName = string.Empty;
            Guid? unitOfMeasureId = null;
            string unitOfMeasureCode = string.Empty;
            string unitOfMeasureName = string.Empty;

            try
            {
                if (entity.ProductVariant != null)
                {
                    var variant = entity.ProductVariant;
                    productVariantCode = variant.VariantCode ?? string.Empty;
                    productVariantFullName = variant.VariantFullName ?? string.Empty;
                    unitOfMeasureId = variant.UnitId;

                    // ProductService
                    if (variant.ProductService != null)
                    {
                        productCode = variant.ProductService.Code ?? string.Empty;
                        productName = variant.ProductService.Name ?? string.Empty;
                    }

                    // UnitOfMeasure
                    if (variant.UnitOfMeasure != null)
                    {
                        unitOfMeasureCode = variant.UnitOfMeasure.Code ?? string.Empty;
                        unitOfMeasureName = variant.UnitOfMeasure.Name ?? string.Empty;
                    }
                }
            }
            catch
            {
                // Nếu không thể load navigation properties (DataContext disposed), để giá trị mặc định
            }

            var dto = new StockInOutProductHistoryDto
            {
                // Thông tin cơ bản
                Id = entity.Id,
                StockInOutMasterId = entity.StockInOutMasterId,
                VocherNumber = vocherNumber,
                StockInOutDate = stockInOutDate,
                LoaiNhapXuatKho = loaiNhapXuatKho,
                LoaiNhapXuatKhoName = loaiNhapXuatKhoName,

                // Thông tin sản phẩm
                ProductVariantId = entity.ProductVariantId,
                ProductCode = productCode,
                ProductName = productName,
                ProductVariantCode = productVariantCode,
                ProductVariantFullName = productVariantFullName,
                UnitOfMeasureId = unitOfMeasureId,
                UnitOfMeasureCode = unitOfMeasureCode,
                UnitOfMeasureName = unitOfMeasureName,

                // Số lượng và giá
                StockInQty = entity.StockInQty,
                StockOutQty = entity.StockOutQty,
                UnitPrice = entity.UnitPrice,
                Vat = entity.Vat,
                VatAmount = entity.VatAmount,
                TotalAmount = entity.TotalAmount,
                TotalAmountIncludedVat = entity.TotalAmountIncludedVat,

                // Thông tin liên kết
                WarehouseId = warehouseId,
                WarehouseName = warehouseName,
                CustomerName = customerName
            };

            return dto;
        }

        /// <summary>
        /// Chuyển đổi danh sách StockInOutDetail entities thành danh sách StockInOutProductHistoryDto
        /// </summary>
        /// <param name="entities">Danh sách StockInOutDetail entities</param>
        /// <returns>Danh sách StockInOutProductHistoryDto</returns>
        public static List<StockInOutProductHistoryDto> ToDtos(this IEnumerable<StockInOutDetail> entities)
        {
            if (entities == null) return new List<StockInOutProductHistoryDto>();

            return entities.Select(entity => entity.ToDto())
                .Where(dto => dto != null)
                .ToList();
        }
    }
}
