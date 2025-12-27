using Dal.Connection;
using Dal.DataAccess.Implementations.Inventory.InventoryManagement;
using Dal.DataAccess.Interfaces.Inventory.InventoryManagement;
using Dal.DataContext;
using DTO.Inventory.InventoryManagement;
using DTO.Inventory.Report;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Bll.Inventory.InventoryManagement;

/// <summary>
/// Business Logic Layer cho StockInOutReport (Báo cáo phiếu nhập xuất kho)
/// </summary>
public class StockInOutReportBll
{
    #region Fields

    private IStockInOutMasterRepository _dataAccess;
    private readonly ILogger _logger;
    private readonly object _lockObject = new();

    #endregion

    #region Constructors

    /// <summary>
    /// Constructor mặc định
    /// </summary>
    public StockInOutReportBll()
    {
        _logger = LoggerFactory.CreateLogger(LogCategory.BLL);
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Lấy hoặc khởi tạo Repository (lazy initialization)
    /// </summary>
    private IStockInOutMasterRepository GetDataAccess()
    {
        if (_dataAccess == null)
        {
            lock (_lockObject)
            {
                if (_dataAccess == null)
                {
                    try
                    {
                        // Sử dụng global connection string từ ApplicationStartupManager
                        var globalConnectionString = ApplicationStartupManager.Instance.GetGlobalConnectionString();
                        if (string.IsNullOrEmpty(globalConnectionString))
                        {
                            throw new InvalidOperationException(
                                "Không có global connection string. Ứng dụng chưa được khởi tạo hoặc chưa sẵn sàng.");
                        }

                        _dataAccess = new StockInOutMasterRepository(globalConnectionString);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"Lỗi khởi tạo StockInOutMasterRepository: {ex.Message}", ex);
                        throw;
                    }
                }
            }
        }

        return _dataAccess;
    }

    /// <summary>
    /// Lấy Description từ enum value
    /// </summary>
    /// <typeparam name="T">Kiểu enum</typeparam>
    /// <param name="enumValue">Giá trị enum</param>
    /// <returns>Description hoặc tên enum nếu không có Description</returns>
    private static string GetEnumDescription<T>(T enumValue) where T : Enum
    {
        var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
        if (fieldInfo == null) return enumValue.ToString();

        var descriptionAttribute = fieldInfo.GetCustomAttribute<System.ComponentModel.DescriptionAttribute>();
        return descriptionAttribute?.Description ?? enumValue.ToString();
    }

    #endregion

    #region Query Operations

    /// <summary>
    /// Lấy thông tin đầy đủ của StockInOutReportDto dựa vào ID của StockInOutMaster
    /// Bao gồm cả master data và detail data với tất cả navigation properties
    /// </summary>
    /// <param name="masterId">ID của StockInOutMaster</param>
    /// <returns>StockInOutReportDto đầy đủ thông tin hoặc null nếu không tìm thấy</returns>
    public StockInOutReportDto GetReportDtoById(Guid masterId)
    {
        try
        {
            _logger.Debug("GetReportDtoById: Bắt đầu lấy thông tin report, MasterId={0}", masterId);

            // Lấy StockInOutMaster với tất cả navigation properties từ repository
            var master = GetDataAccess().GetMasterByIdWithDetails(masterId);
            if (master == null)
            {
                _logger.Warning("GetReportDtoById: Không tìm thấy StockInOutMaster với Id={0}", masterId);
                return null;
            }

            // Convert entity sang DTO
            var reportDto = ConvertToReportDto(master);

            _logger.Info("GetReportDtoById: Lấy thông tin report thành công, MasterId={0}, DetailCount={1}", 
                masterId, reportDto.ChiTietNhapXuatKhos?.Count ?? 0);
            
            return reportDto;
        }
        catch (Exception ex)
        {
            _logger.Error($"GetReportDtoById: Lỗi lấy thông tin report: {ex.Message}", ex);
            throw;
        }
    }

    #endregion

    #region Converter Methods

    /// <summary>
    /// Chuyển đổi StockInOutMaster entity thành StockInOutReportDto
    /// Bao gồm cả master data và detail data
    /// </summary>
    /// <param name="master">StockInOutMaster entity</param>
    /// <returns>StockInOutReportDto</returns>
    private StockInOutReportDto ConvertToReportDto(StockInOutMaster master)
    {
        if (master == null) return null;

        // Chuyển đổi StockInOutType từ int sang LoaiNhapXuatKhoEnum
        LoaiNhapXuatKhoEnum loaiNhapXuatKho;
        if (Enum.IsDefined(typeof(LoaiNhapXuatKhoEnum), master.StockInOutType))
        {
            loaiNhapXuatKho = (LoaiNhapXuatKhoEnum)master.StockInOutType;
        }
        else
        {
            loaiNhapXuatKho = LoaiNhapXuatKhoEnum.Khac;
        }

        // Lấy tên loại nhập xuất từ Description attribute
        var loaiNhapXuatKhoName = GetEnumDescription(loaiNhapXuatKho);

        // Tạo DTO master
        var reportDto = new StockInOutReportDto
        {
            // Thông tin cơ bản
            Id = master.Id,
            StockInOutDate = master.StockInOutDate,
            VocherNumber = master.VocherNumber,
            LoaiNhapXuatKhoName = loaiNhapXuatKhoName,

            // Thông tin kho
            WarehouseName = master.CompanyBranch?.BranchName ?? string.Empty,

            // Thông tin đối tác
            CustomerName = master.BusinessPartnerSite?.BusinessPartner?.PartnerName ?? 
                          master.BusinessPartnerSite?.SiteName ?? string.Empty,

            // Thông tin bổ sung
            Notes = master.Notes ?? string.Empty,
            NguoiNhanHang = master.NguoiNhanHang ?? string.Empty,
            NguoiGiaoHang = master.NguoiGiaoHang ?? string.Empty,

            // Tổng hợp
            TotalQuantity = master.TotalQuantity,
            TotalAmount = master.TotalAmount,
            TotalVat = master.TotalVat,
            TotalAmountIncludedVat = master.TotalAmountIncludedVat,
            DiscountAmount = master.DiscountAmount,
            TotalAmountAfterDiscount = master.TotalAmountAfterDiscount,

            // Chi tiết
            ChiTietNhapXuatKhos = new List<StockInOutReportDetailDto>()
        };

        // Convert details nếu có
        if (master.StockInOutDetails != null && master.StockInOutDetails.Any())
        {
            var details = master.StockInOutDetails
                .OrderBy(d => d.Id) // Sắp xếp theo ID để đảm bảo thứ tự nhất quán
                .ToList();

            int lineNumber = 1;
            foreach (var detail in details)
            {
                var detailDto = ConvertToReportDetailDto(detail, lineNumber);
                if (detailDto != null)
                {
                    reportDto.ChiTietNhapXuatKhos.Add(detailDto);
                    lineNumber++;
                }
            }
        }

        return reportDto;
    }

    /// <summary>
    /// Chuyển đổi StockInOutDetail entity thành StockInOutReportDetailDto
    /// </summary>
    /// <param name="detail">StockInOutDetail entity</param>
    /// <param name="lineNumber">Số thứ tự dòng</param>
    /// <returns>StockInOutReportDetailDto</returns>
    private StockInOutReportDetailDto ConvertToReportDetailDto(StockInOutDetail detail, int lineNumber)
    {
        if (detail == null) return null;

        // Lấy thông tin ProductVariant
        var productVariant = detail.ProductVariant;
        var productVariantName = productVariant?.VariantNameForReport ?? 
                                productVariant?.ProductService?.Name ?? 
                                string.Empty;

        // Loại bỏ tất cả các tag HTML trong productVariantName nếu có
        //productVariantName = StripHtmlTags(productVariantName);

        // Lấy thông tin UnitOfMeasure
        var unitOfMeasureName = productVariant?.UnitOfMeasure?.Name ?? string.Empty;

        // Tạo DTO detail
        var detailDto = new StockInOutReportDetailDto
        {
            // Thông tin cơ bản
            LineNumber = lineNumber,

            // Thông tin hàng hóa
            ProductVariantName = productVariantName,
            UnitOfMeasureName = unitOfMeasureName,

            // Số lượng và giá
            StockInQty = detail.StockInQty,
            StockOutQty = detail.StockOutQty,
            UnitPrice = detail.UnitPrice,
            Vat = detail.Vat,
            VatAmount = detail.VatAmount,
            TotalAmount = detail.TotalAmount,
            TotalAmountIncludedVat = detail.TotalAmountIncludedVat,
            DiscountAmount = detail.DiscountAmount,
            DiscountPercentage = detail.DiscountPercentage,
            TotalAmountAfterDiscount = detail.TotalAmountAfterDiscount,
            GhiChu = detail.GhiChu ?? string.Empty
        };

        return detailDto;
    }

    #endregion
}
