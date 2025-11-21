using System;
using System.Collections.Generic;
using System.Linq;
using Dal.DataContext;

namespace DTO.Inventory.StockIn;

/// <summary>
/// Converter giữa StockInOutMaster entity và StockInListDto
/// </summary>
public static class StockInListDtoConverter
{
    #region Entity to DTO

    /// <summary>
    /// Chuyển đổi StockInOutMaster entity thành StockInListDto
    /// </summary>
    /// <param name="entity">StockInOutMaster entity</param>
    /// <returns>StockInListDto</returns>
    public static StockInListDto ToDto(this StockInOutMaster entity)
    {
        if (entity == null) return null;

        var dto = new StockInListDto
        {
            Id = entity.Id,
            StockInNumber = entity.VocherNumber ?? string.Empty,
            StockInDate = entity.StockInOutDate,
            LoaiNhapKho = (LoaiNhapKhoEnum)entity.StockInOutType,
            TrangThai = (TrangThaiPhieuNhapEnum)entity.TrangThaiPhieuNhap,
            TotalQuantity = entity.TotalQuantity,
            TotalAmount = entity.TotalAmount,
            CreatedDate = entity.CreatedDate
        };

        // Map tên loại nhập kho
        dto.LoaiNhapKhoName = GetLoaiNhapKhoName(dto.LoaiNhapKho);

        // Map tên trạng thái
        dto.TrangThaiName = GetTrangThaiName(dto.TrangThai);

        // Map thông tin kho
        if (entity.CompanyBranch != null)
        {
            dto.WarehouseCode = entity.CompanyBranch.BranchCode;
            dto.WarehouseName = entity.CompanyBranch.BranchName;
        }

        // Map thông tin nhà cung cấp
        if (entity.BusinessPartnerSite != null)
        {
            if (entity.BusinessPartnerSite.BusinessPartner != null)
            {
                dto.SupplierCode = entity.BusinessPartnerSite.BusinessPartner.PartnerCode;
                dto.SupplierName = entity.BusinessPartnerSite.BusinessPartner.PartnerName;
            }
            else
            {
                dto.SupplierCode = entity.BusinessPartnerSite.SiteCode;
                dto.SupplierName = entity.BusinessPartnerSite.SiteName;
            }
        }

        // TODO: Map PurchaseOrderNumber nếu có PurchaseOrder navigation property
        // TODO: Map SalesOrderNumber nếu có SalesOrder navigation property
        // TODO: Map PhuongThucNhapKho nếu có trong entity
        // TODO: Map CreatedByName nếu có User navigation property

        return dto;
    }

    /// <summary>
    /// Chuyển đổi danh sách StockInOutMaster entities thành danh sách StockInListDto
    /// </summary>
    /// <param name="entities">Danh sách StockInOutMaster entities</param>
    /// <returns>Danh sách StockInListDto</returns>
    public static List<StockInListDto> ToDtoList(this IEnumerable<StockInOutMaster> entities)
    {
        if (entities == null) return new List<StockInListDto>();

        return entities.Select(entity => entity.ToDto()).ToList();
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Lấy tên loại nhập kho từ enum
    /// </summary>
    private static string GetLoaiNhapKhoName(LoaiNhapKhoEnum loaiNhapKho)
    {
        return loaiNhapKho switch
        {
            LoaiNhapKhoEnum.ThuongMai => "Thương mại",
            LoaiNhapKhoEnum.Khac => "Khác",
            _ => "Không xác định"
        };
    }

    /// <summary>
    /// Lấy tên trạng thái từ enum
    /// </summary>
    private static string GetTrangThaiName(TrangThaiPhieuNhapEnum trangThai)
    {
        return trangThai switch
        {
            TrangThaiPhieuNhapEnum.TaoMoi => "Tạo mới",
            TrangThaiPhieuNhapEnum.ChoDuyet => "Chờ duyệt",
            TrangThaiPhieuNhapEnum.DaDuyet => "Đã duyệt",
            TrangThaiPhieuNhapEnum.DangNhapKho => "Đang nhập kho",
            TrangThaiPhieuNhapEnum.DaNhapKho => "Đã nhập kho",
            TrangThaiPhieuNhapEnum.DaHuy => "Đã hủy",
            _ => "Không xác định"
        };
    }

    #endregion
}

