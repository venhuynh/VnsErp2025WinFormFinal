using System;
using System.Collections.Generic;
using System.Linq;
using Dal.DataContext;
using DTO.Inventory;
using DTO.Inventory.InventoryManagement;

namespace Dal.DtoConverter.Inventory.StockInOut
{
    /// <summary>
    /// Converter giữa StockInOutMaster entity và StockInOutMasterForUIDto
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public static class StockInOutMasterForUIConverter
    {
        #region Entity to DTO

        /// <summary>
        /// Chuyển đổi StockInOutMaster entity thành StockInOutMasterForUIDto
        /// </summary>
        /// <param name="entity">StockInOutMaster entity</param>
        /// <returns>StockInOutMasterForUIDto</returns>
        public static StockInOutMasterForUIDto ToDto(this StockInOutMaster entity)
        {
            if (entity == null) return null;

            var dto = new StockInOutMasterForUIDto
            {
                // Thông tin cơ bản
                Id = entity.Id,
                VoucherNumber = entity.VocherNumber ?? string.Empty,
                StockOutDate = entity.StockInOutDate,

                // Loại và trạng thái - chuyển đổi từ int sang enum
                LoaiNhapXuatKho = ConvertToLoaiNhapXuatKhoEnum(entity.StockInOutType),
                TrangThai = ConvertToTrangThaiPhieuNhapEnum(entity.VoucherStatus),

                // Thông tin liên kết
                WarehouseId = entity.WarehouseId,
                SalesOrderId = entity.PurchaseOrderId, // PurchaseOrderId trong DB được dùng để lưu SalesOrderId
                CustomerId = entity.PartnerSiteId,

                // Thông tin bổ sung
                Notes = entity.Notes ?? string.Empty,
                NguoiNhanHang = entity.NguoiNhanHang ?? string.Empty,
                NguoiGiaoHang = entity.NguoiGiaoHang ?? string.Empty
            };

            // Map navigation properties nếu đã được load trong entity
            // Warehouse (CompanyBranch)
            if (entity.CompanyBranch != null)
            {
                dto.WarehouseCode = entity.CompanyBranch.BranchCode ?? string.Empty;
                dto.WarehouseName = entity.CompanyBranch.BranchName ?? string.Empty;
            }

            // Customer (BusinessPartnerSite)
            if (entity.BusinessPartnerSite != null)
            {
                dto.CustomerName = entity.BusinessPartnerSite.SiteName ?? string.Empty;

                // Lấy SalesOrderNumber từ BusinessPartnerSite nếu có (hoặc từ navigation property khác)
                // Note: SalesOrderNumber có thể cần được load từ SalesOrder entity nếu có
                // Hiện tại chỉ map CustomerName
            }

            // Cập nhật tổng hợp từ entity
            dto.SetTotals(
                entity.TotalQuantity,
                entity.TotalAmount,
                entity.TotalVat,
                entity.TotalAmountIncludedVat
            );

            return dto;
        }

        /// <summary>
        /// Chuyển đổi danh sách StockInOutMaster entities thành danh sách StockInOutMasterForUIDto
        /// </summary>
        /// <param name="entities">Danh sách StockInOutMaster entities</param>
        /// <returns>Danh sách StockInOutMasterForUIDto</returns>
        public static List<StockInOutMasterForUIDto> ToDtoList(this IEnumerable<StockInOutMaster> entities)
        {
            if (entities == null) return new List<StockInOutMasterForUIDto>();

            return entities.Select(entity => entity.ToDto()).ToList();
        }

        #endregion

        #region DTO to Entity

        /// <summary>
        /// Chuyển đổi StockInOutMasterForUIDto thành StockInOutMaster entity
        /// </summary>
        /// <param name="dto">StockInOutMasterForUIDto</param>
        /// <returns>StockInOutMaster entity</returns>
        public static StockInOutMaster ToEntity(this StockInOutMasterForUIDto dto)
        {
            if (dto == null) return null;

            var entity = new StockInOutMaster
            {
                Id = dto.Id,
                VocherNumber = dto.VoucherNumber ?? string.Empty,
                StockInOutDate = dto.StockOutDate,
                StockInOutType = (int)dto.LoaiNhapXuatKho, // Chuyển đổi enum sang int
                VoucherStatus = (int)dto.TrangThai, // Chuyển đổi enum sang int
                WarehouseId = dto.WarehouseId,
                PurchaseOrderId = dto.SalesOrderId, // PurchaseOrderId trong DB được dùng để lưu SalesOrderId
                PartnerSiteId = dto.CustomerId,
                Notes = dto.Notes ?? string.Empty,
                NguoiNhanHang = dto.NguoiNhanHang ?? string.Empty,
                NguoiGiaoHang = dto.NguoiGiaoHang ?? string.Empty,

                // Tổng hợp - lấy từ computed properties trong DTO
                TotalQuantity = dto.TotalQuantity,
                TotalAmount = dto.TotalAmount,
                TotalVat = dto.TotalVat,
                TotalAmountIncludedVat = dto.TotalAmountIncludedVat
            };

            return entity;
        }

        /// <summary>
        /// Chuyển đổi danh sách StockInOutMasterForUIDto thành danh sách StockInOutMaster entities
        /// </summary>
        /// <param name="dtos">Danh sách StockInOutMasterForUIDto</param>
        /// <returns>Danh sách StockInOutMaster entities</returns>
        public static List<StockInOutMaster> ToEntityList(this IEnumerable<StockInOutMasterForUIDto> dtos)
        {
            if (dtos == null) return new List<StockInOutMaster>();

            return dtos.Select(dto => dto.ToEntity()).ToList();
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Chuyển đổi StockInOutType (int) sang LoaiNhapXuatKhoEnum
        /// </summary>
        /// <param name="stockInOutType">Giá trị int từ StockInOutType</param>
        /// <returns>LoaiNhapXuatKhoEnum</returns>
        private static LoaiNhapXuatKhoEnum ConvertToLoaiNhapXuatKhoEnum(int stockInOutType)
        {
            if (Enum.IsDefined(typeof(LoaiNhapXuatKhoEnum), stockInOutType))
            {
                return (LoaiNhapXuatKhoEnum)stockInOutType;
            }

            // Nếu giá trị không hợp lệ, mặc định là Khac
            return LoaiNhapXuatKhoEnum.Khac;
        }

        /// <summary>
        /// Chuyển đổi VoucherStatus (int) sang TrangThaiPhieuNhapEnum
        /// </summary>
        /// <param name="voucherStatus">Giá trị int từ VoucherStatus</param>
        /// <returns>TrangThaiPhieuNhapEnum</returns>
        private static TrangThaiPhieuNhapEnum ConvertToTrangThaiPhieuNhapEnum(int voucherStatus)
        {
            if (Enum.IsDefined(typeof(TrangThaiPhieuNhapEnum), voucherStatus))
            {
                return (TrangThaiPhieuNhapEnum)voucherStatus;
            }

            // Nếu giá trị không hợp lệ, mặc định là TaoMoi
            return TrangThaiPhieuNhapEnum.TaoMoi;
        }

        #endregion
    }
}
