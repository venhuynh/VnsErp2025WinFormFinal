using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bll.Inventory.InventoryManagement;
using Dal.Connection;
using Dal.DataAccess.Implementations.Inventory.StockIn;
using Dal.DataAccess.Interfaces.Inventory.StockIn;
using Dal.DataContext;
using DTO.Inventory.InventoryManagement;
using DTO.Inventory.StockIn.NhapHangThuongMai;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;

namespace Bll.Inventory.StockInOut
{
    /// <summary>
    /// Business Logic Layer cho StockIn (Phiếu nhập kho)
    /// </summary>
    public class StockInOutBll
    {
        #region Fields

        private IStockInRepository _dataAccess;
        private readonly ILogger _logger;
        private readonly object _lockObject = new object();

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor mặc định
        /// </summary>
        public StockInOutBll()
        {
            _logger = LoggerFactory.CreateLogger(LogCategory.BLL);
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Lấy hoặc khởi tạo Repository (lazy initialization)
        /// </summary>
        private IStockInRepository GetDataAccess()
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

                            _dataAccess = new StockInRepository(globalConnectionString);
                        }
                        catch (Exception ex)
                        {
                            _logger.Error($"Lỗi khởi tạo StockInRepository: {ex.Message}", ex);
                            throw;
                        }
                    }
                }
            }

            return _dataAccess;
        }

        #endregion

        #region Save Operations

        /// <summary>
        /// Lưu phiếu nhập kho (master và detail) với transaction
        /// Đảm bảo validation đã được thực hiện trước khi gọi method này
        /// Method này nhận entities để có thể được sử dụng bởi nhiều màn hình khác nhau với các DTO khác nhau
        /// </summary>
        /// <param name="masterEntity">Entity phiếu nhập kho (master)</param>
        /// <param name="detailEntities">Danh sách entity chi tiết phiếu nhập kho</param>
        /// <returns>ID phiếu nhập kho đã lưu</returns>
        public async Task<Guid> SaveAsync(StockInOutMaster masterEntity, List<StockInOutDetail> detailEntities)
        {
            if (masterEntity == null)
                throw new ArgumentNullException(nameof(masterEntity));

            if (detailEntities == null || detailEntities.Count == 0)
                throw new ArgumentException("Danh sách chi tiết không được để trống", nameof(detailEntities));

            try
            {
                _logger.Debug("SaveAsync: Bắt đầu lưu phiếu nhập kho, MasterId={0}, DetailCount={1}", 
                    masterEntity.Id, detailEntities.Count);

                // 0. Validate dữ liệu trước khi lưu
                ValidateBeforeSave(masterEntity, detailEntities);

                // 1. Lưu qua Repository
                var savedMasterId = await GetDataAccess().SaveAsync(masterEntity, detailEntities);

                _logger.Info("SaveAsync: Lưu phiếu nhập kho thành công, MasterId={0}", savedMasterId);
                return savedMasterId;
            }
            catch (Exception ex)
            {
                _logger.Error($"SaveAsync: Lỗi lưu phiếu nhập kho: {ex.Message}", ex);
                throw new Exception($"Lỗi lưu phiếu nhập kho: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Map StockInMasterDto sang StockInOutMaster entity
        /// Helper method để convert DTO sang entity (dùng cho các màn hình sử dụng StockInMasterDto)
        /// </summary>
        public static StockInOutMaster MapMasterDtoToEntity(StockInMasterDto dto)
        {
            return new StockInOutMaster
            {
                Id = dto.Id,
                StockInOutDate = dto.StockInDate,
                VocherNumber = dto.StockInNumber,
                StockInOutType = (int)dto.LoaiNhapXuatKho, // Map enum to int
                VoucherStatus = (int)dto.TrangThai, // Map enum to int
                WarehouseId = dto.WarehouseId,
                PurchaseOrderId = dto.PurchaseOrderId,
                PartnerSiteId = dto.SupplierId,
                Notes = dto.Notes ?? string.Empty,
                TotalQuantity = dto.TotalQuantity,
                TotalAmount = dto.TotalAmount,
                TotalVat = dto.TotalVat,
                TotalAmountIncludedVat = dto.TotalAmountIncludedVat,
                NguoiNhanHang = dto.NguoiNhanHang,
                NguoiGiaoHang = dto.NguoiGiaoHang
            };
        }

        /// <summary>
        /// Map StockInDetailDto sang StockInOutDetail entity
        /// Helper method để convert DTO sang entity (dùng cho các màn hình sử dụng StockInDetailDto)
        /// </summary>
        public static StockInOutDetail MapDetailDtoToEntity(StockInDetailDto dto)
        {
            return new StockInOutDetail
            {
                Id = dto.Id,
                StockInOutMasterId = dto.StockInOutMasterId,
                ProductVariantId = dto.ProductVariantId,
                StockInQty = dto.StockInQty,
                StockOutQty = dto.StockOutQty,
                UnitPrice = dto.UnitPrice,
                Vat = dto.Vat,
                VatAmount = dto.VatAmount, // Computed property value
                TotalAmount = dto.TotalAmount, // Computed property value
                TotalAmountIncludedVat = dto.TotalAmountIncludedVat, // Computed property value
                
            };
        }

        /// <summary>
        /// Validate dữ liệu trước khi lưu
        /// </summary>
        private void ValidateBeforeSave(StockInOutMaster masterEntity, List<StockInOutDetail> detailEntities)
        {
            // Validate WarehouseId - không được để trống hoặc Guid.Empty
            if (masterEntity.WarehouseId == Guid.Empty)
            {
                throw new ArgumentException("Vui lòng chọn kho. WarehouseId không được để trống.");
            }

            // Xác định loại phiếu (nhập hay xuất) từ StockInOutType
            var loaiNhapXuatKho = (LoaiNhapXuatKhoEnum)masterEntity.StockInOutType;
            var isStockIn = IsStockInType(loaiNhapXuatKho);
            var isStockOut = IsStockOutType(loaiNhapXuatKho);

            // Validate các detail
            for (int i = 0; i < detailEntities.Count; i++)
            {
                var detail = detailEntities[i];
                var lineNumber = i + 1;

                if (detail.ProductVariantId == Guid.Empty)
                {
                    throw new ArgumentException($"Dòng {lineNumber}: Vui lòng chọn hàng hóa. ProductVariantId không được để trống.");
                }

                // Tùy theo phiếu nhập hoặc phiếu xuất thì kiểm tra xem số lượng nhập hoặc xuất
                if (isStockIn)
                {
                    // Phiếu nhập: kiểm tra StockInQty phải lớn hơn 0
                    if (detail.StockInQty <= 0)
                    {
                        throw new ArgumentException($"Dòng {lineNumber}: Số lượng nhập phải lớn hơn 0.");
                    }
                }
                else if (isStockOut)
                {
                    // Phiếu xuất: kiểm tra StockOutQty phải lớn hơn 0
                    if (detail.StockOutQty <= 0)
                    {
                        throw new ArgumentException($"Dòng {lineNumber}: Số lượng xuất phải lớn hơn 0.");
                    }
                }
                else
                {
                    // Trường hợp Khac hoặc không xác định: kiểm tra cả hai
                    if (detail.StockInQty <= 0 && detail.StockOutQty <= 0)
                    {
                        throw new ArgumentException($"Dòng {lineNumber}: Phải có số lượng nhập hoặc số lượng xuất lớn hơn 0.");
                    }
                }
            }

            _logger.Debug("ValidateBeforeSave: Validation passed");
        }

        /// <summary>
        /// Kiểm tra xem loại nhập xuất kho có phải là loại nhập không
        /// </summary>
        /// <param name="loaiNhapXuatKho">Loại nhập xuất kho</param>
        /// <returns>True nếu là loại nhập, False nếu không</returns>
        private static bool IsStockInType(LoaiNhapXuatKhoEnum loaiNhapXuatKho)
        {
            return loaiNhapXuatKho == LoaiNhapXuatKhoEnum.NhapHangThuongMai ||
                   loaiNhapXuatKho == LoaiNhapXuatKhoEnum.NhapThietBiMuonThue ||
                   loaiNhapXuatKho == LoaiNhapXuatKhoEnum.NhapNoiBo ||
                   loaiNhapXuatKho == LoaiNhapXuatKhoEnum.NhapLuuChuyenKho ||
                   loaiNhapXuatKho == LoaiNhapXuatKhoEnum.NhapHangBaoHanh ||
                   loaiNhapXuatKho == LoaiNhapXuatKhoEnum.NhapSanPhamLapRap ||
                   loaiNhapXuatKho == LoaiNhapXuatKhoEnum.NhapLinhKienPhanRa;
        }

        /// <summary>
        /// Kiểm tra xem loại nhập xuất kho có phải là loại xuất không
        /// </summary>
        /// <param name="loaiNhapXuatKho">Loại nhập xuất kho</param>
        /// <returns>True nếu là loại xuất, False nếu không</returns>
        private static bool IsStockOutType(LoaiNhapXuatKhoEnum loaiNhapXuatKho)
        {
            return loaiNhapXuatKho == LoaiNhapXuatKhoEnum.XuatHangThuongMai ||
                   loaiNhapXuatKho == LoaiNhapXuatKhoEnum.XuatThietBiMuonThue ||
                   loaiNhapXuatKho == LoaiNhapXuatKhoEnum.XuatNoiBo ||
                   loaiNhapXuatKho == LoaiNhapXuatKhoEnum.XuatLuuChuyenKho ||
                   loaiNhapXuatKho == LoaiNhapXuatKhoEnum.XuatHangBaoHanh ||
                   loaiNhapXuatKho == LoaiNhapXuatKhoEnum.XuatLinhKienLapRap ||
                   loaiNhapXuatKho == LoaiNhapXuatKhoEnum.XuatThanhPhamPhanRa;
        }

        #endregion

        #region Query Operations

        /// <summary>
        /// Lấy số thứ tự tiếp theo cho phiếu nhập kho
        /// </summary>
        /// <param name="stockInDate">Ngày nhập kho</param>
        /// <param name="loaiNhapXuatKho">Loại nhập kho</param>
        /// <returns>Số thứ tự tiếp theo (1-999)</returns>
        public int GetNextSequenceNumber(DateTime stockInDate, LoaiNhapXuatKhoEnum loaiNhapXuatKho)
        {
            try
            {
                _logger.Debug("GetNextSequenceNumber: Date={0}, LoaiNhapXuatKho={1}", stockInDate, loaiNhapXuatKho);
                
                var loaiNhapKhoInt = (int)loaiNhapXuatKho;
                var nextSequence = GetDataAccess().GetNextSequenceNumber(stockInDate, loaiNhapKhoInt);
                
                _logger.Debug("GetNextSequenceNumber: NextSequence={0}", nextSequence);
                return nextSequence;
            }
            catch (Exception ex)
            {
                _logger.Error($"GetNextSequenceNumber: Lỗi lấy số thứ tự tiếp theo: {ex.Message}", ex);
                // Fallback: trả về 1 nếu có lỗi
                return 1;
            }
        }


        /// <summary>
        /// Lấy danh sách chi tiết phiếu nhập/xuất kho theo MasterId
        /// </summary>
        /// <param name="stockInOutMasterId">ID phiếu nhập/xuất kho</param>
        /// <returns>Danh sách StockInOutDetail entities</returns>
        public List<StockInOutDetail> GetDetailsByMasterId(Guid stockInOutMasterId)
        {
            try
            {
                _logger.Debug("GetDetailsByMasterId: Lấy danh sách chi tiết, StockInOutMasterId={0}", stockInOutMasterId);
                
                var details = GetDataAccess().GetDetailsByMasterId(stockInOutMasterId);
                
                _logger.Info("GetDetailsByMasterId: Lấy được {0} chi tiết", details.Count);
                return details;
            }
            catch (Exception ex)
            {
                _logger.Error($"GetDetailsByMasterId: Lỗi lấy danh sách chi tiết: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Lấy thông tin master phiếu nhập/xuất kho theo ID
        /// </summary>
        /// <param name="stockInOutMasterId">ID phiếu nhập/xuất kho</param>
        /// <returns>StockInOutMaster entity với navigation properties đã load</returns>
        public StockInOutMaster GetMasterById(Guid stockInOutMasterId)
        {
            try
            {
                _logger.Debug("GetMasterById: Lấy thông tin master, StockInOutMasterId={0}", stockInOutMasterId);
                
                var master = GetDataAccess().GetMasterById(stockInOutMasterId);
                
                if (master != null)
                {
                    _logger.Info("GetMasterById: Lấy được master thành công, Id={0}", master.Id);
                }
                else
                {
                    _logger.Warning("GetMasterById: Không tìm thấy master với Id={0}", stockInOutMasterId);
                }
                
                return master;
            }
            catch (Exception ex)
            {
                _logger.Error($"GetMasterById: Lỗi lấy thông tin master: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Map StockInOutMaster entity sang StockInMasterDto
        /// </summary>
        private StockInMasterDto MapMasterEntityToDto(StockInOutMaster entity)
        {
            if (entity == null) return null;

            var dto = new StockInMasterDto
            {
                Id = entity.Id,
                StockInNumber = entity.VocherNumber ?? string.Empty,
                StockInDate = entity.StockInOutDate,
                LoaiNhapXuatKho = (LoaiNhapXuatKhoEnum)entity.StockInOutType,
                TrangThai = (TrangThaiPhieuNhapEnum)entity.VoucherStatus,
                WarehouseId = entity.WarehouseId,
                WarehouseCode = entity.CompanyBranch?.BranchCode ?? string.Empty,
                WarehouseName = entity.CompanyBranch?.BranchName ?? string.Empty,
                PurchaseOrderId = entity.PurchaseOrderId,
                PurchaseOrderNumber = string.Empty, // TODO: Lấy từ PurchaseOrder entity nếu cần
                SupplierId = entity.PartnerSiteId,
                SupplierName = entity.BusinessPartnerSite?.BusinessPartner?.PartnerName ?? 
                               entity.BusinessPartnerSite?.SiteName ?? 
                               string.Empty,
                Notes = entity.Notes ?? string.Empty,
                NguoiNhanHang = entity.NguoiNhanHang ?? string.Empty,
                NguoiGiaoHang = entity.NguoiGiaoHang ?? string.Empty
            };

            // Gán các giá trị tổng hợp từ entity
            dto.SetTotals(
                entity.TotalQuantity,
                entity.TotalAmount,
                entity.TotalVat,
                entity.TotalAmountIncludedVat
            );

            return dto;
        }

        /// <summary>
        /// Lấy dữ liệu cho report in phiếu nhập kho
        /// </summary>
        /// <param name="voucherId">ID phiếu nhập kho</param>
        /// <returns>StockInReportDto chứa master và detail data</returns>
        public StockInReportDto GetReportData(Guid voucherId)
        {
            try
            {
                _logger.Debug("GetReportData: Lấy dữ liệu cho report, VoucherId={0}", voucherId);

                // 1. Lấy master data
                var masterEntity = GetMasterById(voucherId);
                if (masterEntity == null)
                {
                    throw new Exception($"Không tìm thấy phiếu nhập kho với ID: {voucherId}");
                }

                // 2. Lấy detail data
                var detailEntities = GetDetailsByMasterId(voucherId);

                // 3. Lấy thông tin bảo hành
                var warrantyBll = new WarrantyBll();
                var warrantyEntities = warrantyBll.GetByStockInOutMasterId(voucherId);
                var warrantyDtos = warrantyEntities.Select(w => w.ToDto()).ToList();

                // 4. Lấy thông tin Device (định danh thiết bị)
                var deviceBll = new DeviceBll();
                var deviceEntities = deviceBll.GetByStockInOutMasterId(voucherId);
                var deviceDtos = deviceEntities.Select(d => d.ToDto()).ToList();

                // 5. Map sang DTO - sử dụng StockInMasterDto và StockInDetailDto
                var masterDto = MapMasterEntityToDto(masterEntity);
                var detailDtos = detailEntities.Select(d => 
                {
                    var detailDto = StockInDetailDtoConverter.ToDto(d);
                    // Gán thông tin bảo hành cho từng detail
                    detailDto.Warranties = warrantyDtos
                        .Where(w => w.StockInOutDetailId == d.Id)
                        .OrderBy(w => w.WarrantyFrom ?? DateTime.MinValue)
                        .ThenBy(w => w.WarrantyUntil ?? DateTime.MaxValue)
                        .ToList();
                    // Gán thông tin Device (định danh thiết bị) cho từng detail
                    detailDto.Devices = deviceDtos
                        .Where(dev => dev.StockInOutDetailId.HasValue && dev.StockInOutDetailId.Value == d.Id)
                        .ToList();
                    return detailDto;
                }).ToList();

                // 5. Tạo report DTO
                var reportDto = new StockInReportDto
                {
                    Master = masterDto,
                    ChiTietNhapHangNoiBos = detailDtos,
                    // 6. Nhóm thông tin bảo hành theo thời gian và gán vào GhiChu
                    GhiChu = BuildWarrantyInfo(warrantyDtos, masterDto.Notes),
                    
                    
                };

                //7. Nếu thông tin PO có thì thêm vào
                if (reportDto.GhiChu != null && masterEntity.PurchaseOrderId != null)
                {
                   // reportDto.GhiChu += $"\n=== THÔNG TIN ĐƠN HÀNG MUA ===\nSố PO: {masterEntity.PurchaseOrderId.VocherNumber}\nNgày PO: {masterEntity.PurchaseOrder.PurchaseOrderDate:dd/MM/yyyy}";
                }
                _logger.Info("GetReportData: Lấy dữ liệu report thành công, VoucherId={0}, DetailCount={1}, WarrantyCount={2}, DeviceCount={3}", 
                    voucherId, reportDto.ChiTietNhapHangNoiBos.Count, warrantyDtos.Count, deviceDtos.Count);
                
                return reportDto;
            }
            catch (Exception ex)
            {
                _logger.Error($"GetReportData: Lỗi lấy dữ liệu report: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Xây dựng thông tin bảo hành đã nhóm theo thời gian và kết hợp với ghi chú
        /// </summary>
        /// <param name="warrantyDtos">Danh sách bảo hành</param>
        /// <param name="notes">Ghi chú gốc từ master</param>
        /// <returns>Chuỗi thông tin bảo hành đã nhóm và ghi chú</returns>
        private string BuildWarrantyInfo(List<WarrantyDto> warrantyDtos, string notes)
        {
            var result = new List<string>();

            // Thêm ghi chú gốc nếu có
            if (!string.IsNullOrWhiteSpace(notes))
            {
                result.Add(notes);
            }

            // Nhóm bảo hành theo thời gian bảo hành (WarrantyFrom)
            var groupedWarranties = warrantyDtos
                .Where(w => w.WarrantyFrom.HasValue)
                .GroupBy(w => w.WarrantyFrom.Value.Date)
                .OrderBy(g => g.Key)
                .ToList();

            if (groupedWarranties.Any())
            {
                result.Add("=== THÔNG TIN BẢO HÀNH ===");

                foreach (var group in groupedWarranties)
                {
                    var warrantyDate = group.Key;
                    var warrantiesInGroup = group.OrderBy(w => w.WarrantyUntil ?? DateTime.MaxValue).ToList();

                    result.Add($"Ngày bắt đầu BH: {warrantyDate:dd/MM/yyyy}");

                    foreach (var warranty in warrantiesInGroup)
                    {
                        var warrantyInfo = new List<string>();

                        // Thông tin sản phẩm
                        //if (!string.IsNullOrWhiteSpace(warranty.ProductVariantName))
                        //{
                        //    warrantyInfo.Add($"SP: {warranty.ProductVariantName}");
                        //}

                        // Serial/IMEI
                        if (!string.IsNullOrWhiteSpace(warranty.UniqueProductInfo))
                        {
                            warrantyInfo.Add($"Serial/IMEI: {warranty.UniqueProductInfo}");
                        }

                        //// Kiểu bảo hành
                        //if (!string.IsNullOrWhiteSpace(warranty.WarrantyTypeName))
                        //{
                        //    warrantyInfo.Add($"Kiểu: {warranty.WarrantyTypeName}");
                        //}

                        // Thời gian bảo hành
                        var timeInfo = new List<string>();
                        if (warranty.WarrantyFrom.HasValue)
                        {
                            timeInfo.Add($"Từ: {warranty.WarrantyFrom.Value:dd/MM/yyyy}");
                        }
                        if (warranty.WarrantyUntil.HasValue)
                        {
                            timeInfo.Add($"Đến: {warranty.WarrantyUntil.Value:dd/MM/yyyy}");
                        }
                        if (warranty.MonthOfWarranty > 0)
                        {
                            timeInfo.Add($"{warranty.MonthOfWarranty} tháng");
                        }
                        if (timeInfo.Any())
                        {
                            warrantyInfo.Add($"Thời gian: {string.Join(" - ", timeInfo)}");
                        }

                        //// Trạng thái
                        //if (!string.IsNullOrWhiteSpace(warranty.WarrantyStatusName))
                        //{
                        //    warrantyInfo.Add($"Trạng thái: {warranty.WarrantyStatusName}");
                        //}

                        //// Tình trạng
                        //if (!string.IsNullOrWhiteSpace(warranty.WarrantyStatusText))
                        //{
                        //    warrantyInfo.Add($"Tình trạng: {warranty.WarrantyStatusText}");
                        //}

                        if (warrantyInfo.Any())
                        {
                            result.Add($"  • {string.Join(" | ", warrantyInfo)}");
                        }
                    }
                }
            }

            // Bảo hành không có ngày bắt đầu (nếu có)
            var warrantiesWithoutDate = warrantyDtos
                .Where(w => !w.WarrantyFrom.HasValue)
                .ToList();

            if (warrantiesWithoutDate.Any())
            {
                if (!groupedWarranties.Any())
                {
                    result.Add("=== THÔNG TIN BẢO HÀNH ===");
                }
                result.Add("\nBảo hành chưa có ngày bắt đầu:");
                foreach (var warranty in warrantiesWithoutDate)
                {
                    var warrantyInfo = new List<string>();
                    if (!string.IsNullOrWhiteSpace(warranty.ProductVariantName))
                    {
                        warrantyInfo.Add($"SP: {warranty.ProductVariantName}");
                    }
                    if (!string.IsNullOrWhiteSpace(warranty.UniqueProductInfo))
                    {
                        warrantyInfo.Add($"Serial/IMEI: {warranty.UniqueProductInfo}");
                    }
                    if (warrantyInfo.Any())
                    {
                        result.Add($"  • {string.Join(" | ", warrantyInfo)}");
                    }
                }
            }

            return string.Join("\n", result);
        }

        #endregion
    }
}
