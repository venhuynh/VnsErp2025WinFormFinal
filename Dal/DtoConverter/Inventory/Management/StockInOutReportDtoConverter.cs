using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Dal.DataContext;
using DTO.Inventory.InventoryManagement;
using DTO.Inventory.Report;

namespace Dal.DtoConverter.Inventory.Management
{
    /// <summary>
    /// Converter giữa StockInOutMaster, StockInOutDetail entities và StockInOutReportDto
    /// </summary>
    internal static class StockInOutReportDtoConverter
    {
        #region Entity to DTO

        /// <summary>
        /// Chuyển đổi StockInOutMaster entity (có kèm StockInOutDetails) thành StockInOutReportDto
        /// </summary>
        /// <param name="master">StockInOutMaster entity với navigation properties đã được load</param>
        /// <returns>StockInOutReportDto</returns>
        public static StockInOutReportDto ToReportDto(this StockInOutMaster master)
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
                VocherNumber = master.VocherNumber ?? string.Empty,
                LoaiNhapXuatKhoName = loaiNhapXuatKhoName,

                // Thông tin kho
                WarehouseName = master.CompanyBranch?.BranchName ?? string.Empty,

                // Thông tin đối tác
                CustomerName = master.BusinessPartnerSite?.SiteName ??
                               master.BusinessPartnerSite?.BusinessPartner?.PartnerName ?? string.Empty,

                // Thông tin địa chỉ đối tác (từ BusinessPartnerSite)
                CustomerAddress = master.BusinessPartnerSite?.Address ?? string.Empty,
                

                // Thông tin bổ sung
                Notes = master.Notes ?? string.Empty,
                NguoiNhanHang = master.NguoiNhanHang ?? string.Empty,
                NguoiGiaoHang = master.NguoiGiaoHang ?? string.Empty,

                // Thông tin đơn hàng (PurchaseOrderNumber có thể cần được load từ navigation property khác)
                PurchaseOrderNumber = string.Empty, // TODO: Map từ navigation property nếu có

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
                    var detailDto = detail.ToReportDetailDto(lineNumber);
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
        /// <param name="detail">StockInOutDetail entity với navigation properties đã được load</param>
        /// <param name="lineNumber">Số thứ tự dòng</param>
        /// <returns>StockInOutReportDetailDto</returns>
        public static StockInOutReportDetailDto ToReportDetailDto(this StockInOutDetail detail, int lineNumber)
        {
            if (detail == null) return null;

            // Lấy thông tin ProductVariant
            var productVariant = detail.ProductVariant;
            var productVariantName = productVariant?.VariantNameForReport ??
                                    productVariant?.ProductService?.Name ??
                                    string.Empty;

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

        /// <summary>
        /// Chuyển đổi danh sách StockInOutMaster entities thành danh sách StockInOutReportDto
        /// </summary>
        /// <param name="masters">Danh sách StockInOutMaster entities</param>
        /// <returns>Danh sách StockInOutReportDto</returns>
        public static List<StockInOutReportDto> ToReportDtoList(this IEnumerable<StockInOutMaster> masters)
        {
            if (masters == null) return new List<StockInOutReportDto>();

            return masters.Select(master => master.ToReportDto()).Where(dto => dto != null).ToList();
        }

        #endregion

        #region Helper Methods

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

            var descriptionAttribute = fieldInfo.GetCustomAttribute<DescriptionAttribute>();
            return descriptionAttribute?.Description ?? enumValue.ToString();
        }

        #endregion
    }
}
