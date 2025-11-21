using Dal.Connection;
using Dal.DataAccess.Implementations.Inventory.StockIn;
using Dal.DataAccess.Interfaces.Inventory.StockIn;
using Dal.DataContext;
using DTO.Inventory.StockIn;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static DTO.Inventory.StockIn.StockInListDtoConverter;

namespace Bll.Inventory.StockIn
{
    /// <summary>
    /// Business Logic Layer cho StockIn (Phiếu nhập kho)
    /// </summary>
    public class StockInBll
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
        public StockInBll()
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
        /// </summary>
        /// <param name="masterDto">DTO phiếu nhập kho (master)</param>
        /// <param name="detailDtos">Danh sách DTO chi tiết phiếu nhập kho</param>
        /// <returns>ID phiếu nhập kho đã lưu</returns>
        public async Task<Guid> SaveAsync(StockInMasterDto masterDto, List<StockInDetailDto> detailDtos)
        {
            if (masterDto == null)
                throw new ArgumentNullException(nameof(masterDto));

            if (detailDtos == null || detailDtos.Count == 0)
                throw new ArgumentException("Danh sách chi tiết không được để trống", nameof(detailDtos));

            try
            {
                _logger.Debug("SaveAsync: Bắt đầu lưu phiếu nhập kho, MasterId={0}, DetailCount={1}", 
                    masterDto.Id, detailDtos.Count);

                // 0. Validate dữ liệu trước khi lưu
                ValidateBeforeSave(masterDto, detailDtos);

                // 1. Map DTOs sang Entities
                var masterEntity = MapMasterDtoToEntity(masterDto);
                var detailEntities = detailDtos.Select(MapDetailDtoToEntity).ToList();

                // 2. Lưu qua Repository
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
        /// </summary>
        private StockInOutMaster MapMasterDtoToEntity(StockInMasterDto dto)
        {
            return new StockInOutMaster
            {
                Id = dto.Id,
                StockInOutDate = dto.StockInDate,
                VocherNumber = dto.StockInNumber,
                StockInOutType = (int)dto.LoaiNhapKho, // Map enum to int
                TrangThaiPhieuNhap = (int)dto.TrangThai, // Map enum to int
                WarehouseId = dto.WarehouseId,
                PurchaseOrderId = dto.PurchaseOrderId,
                PartnerSiteId = dto.SupplierId,
                Notes = dto.Notes ?? string.Empty,
                TotalQuantity = dto.TotalQuantity,
                TotalAmount = dto.TotalAmount,
                TotalVat = dto.TotalVat,
                TotalAmountIncludedVat = dto.TotalAmountIncludedVat
            };
        }

        /// <summary>
        /// Map StockInDetailDto sang StockInOutDetail entity
        /// </summary>
        private StockInOutDetail MapDetailDtoToEntity(StockInDetailDto dto)
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
                TotalAmountIncludedVat = dto.TotalAmountIncludedVat // Computed property value
            };
        }

        /// <summary>
        /// Validate dữ liệu trước khi lưu
        /// </summary>
        private void ValidateBeforeSave(StockInMasterDto masterDto, List<StockInDetailDto> detailDtos)
        {
            // Validate WarehouseId - không được để trống hoặc Guid.Empty
            if (masterDto.WarehouseId == Guid.Empty)
            {
                throw new ArgumentException("Vui lòng chọn kho nhập. WarehouseId không được để trống.");
            }

            // Validate các detail
            foreach (var detail in detailDtos)
            {
                if (detail.ProductVariantId == Guid.Empty)
                {
                    throw new ArgumentException($"Dòng {detail.LineNumber}: Vui lòng chọn hàng hóa. ProductVariantId không được để trống.");
                }

                if (detail.StockInQty <= 0)
                {
                    throw new ArgumentException($"Dòng {detail.LineNumber}: Số lượng nhập phải lớn hơn 0.");
                }
            }

            _logger.Debug("ValidateBeforeSave: Validation passed");
        }

        #endregion

        #region Query Operations

        /// <summary>
        /// Lấy số thứ tự tiếp theo cho phiếu nhập kho
        /// </summary>
        /// <param name="stockInDate">Ngày nhập kho</param>
        /// <param name="loaiNhapKho">Loại nhập kho</param>
        /// <returns>Số thứ tự tiếp theo (1-999)</returns>
        public int GetNextSequenceNumber(DateTime stockInDate, LoaiNhapKhoEnum loaiNhapKho)
        {
            try
            {
                _logger.Debug("GetNextSequenceNumber: Date={0}, LoaiNhapKho={1}", stockInDate, loaiNhapKho);
                
                var loaiNhapKhoInt = (int)loaiNhapKho;
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
                var master = GetMasterById(voucherId);
                if (master == null)
                {
                    throw new Exception($"Không tìm thấy phiếu nhập kho với ID: {voucherId}");
                }

                // 2. Lấy detail data
                var details = GetDetailsByMasterId(voucherId);

                // 3. Map sang DTO
                var reportDto = new StockInReportDto
                {
                    SoPhieu = master.VocherNumber ?? string.Empty,
                    NgayThang = master.StockInOutDate,
                    NhanHangTu = new NguoiGiaoHangDto
                    {
                        // TODO: Lấy thông tin người giao hàng từ master hoặc related entity
                        // Tạm thời lấy từ BusinessPartnerSite nếu có
                        FullName = master.BusinessPartnerSite?.BusinessPartner?.PartnerName ?? 
                                   master.BusinessPartnerSite?.SiteName ?? 
                                   string.Empty
                    },
                    NguoiNhapXuat = new NguoiNhapXuatDto
                    {
                        // TODO: Lấy thông tin người nhập từ CreatedBy hoặc related entity
                        // Tạm thời để trống, sẽ cần bổ sung sau khi có authentication
                        FullName = string.Empty
                    },
                    KhoNhap = new KhoNhapDto
                    {
                        FullProductNameName = master.CompanyBranch?.BranchName ?? string.Empty
                    },
                    ChiTietNhapHangNoiBos = details.Select(d => new ChiTietNhapHangNoiBoDto
                    {
                        SanPham = new SanPhamDto
                        {
                            ProductName = d.ProductVariant?.ProductService?.Name ?? 
                                         d.ProductVariant?.VariantFullName ?? 
                                         d.ProductVariant?.VariantCode ?? 
                                         string.Empty
                        },
                        DonViTinh = d.ProductVariant?.UnitOfMeasure?.Name ?? string.Empty,
                        SoLuong = d.StockInQty,
                        TinhTrangSanPham = "Bình thường" // TODO: Lấy từ trường tương ứng nếu có
                    }).ToList()
                };

                _logger.Info("GetReportData: Lấy dữ liệu report thành công, VoucherId={0}, DetailCount={1}", 
                    voucherId, reportDto.ChiTietNhapHangNoiBos.Count);
                
                return reportDto;
            }
            catch (Exception ex)
            {
                _logger.Error($"GetReportData: Lỗi lấy dữ liệu report: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Query lịch sử nhập xuất kho với filter
        /// </summary>
        /// <param name="query">Query criteria</param>
        /// <returns>StockInHistoryResultDto chứa danh sách và pagination info</returns>
        public async Task<StockInHistoryResultDto> QueryHistoryAsync(StockInHistoryQueryDto query)
        {
            try
            {
                _logger.Debug("QueryHistoryAsync: Bắt đầu query lịch sử, FromDate={0}, ToDate={1}", 
                    query.FromDate, query.ToDate);

                // Validate query
                if (!query.Validate(out var errorMessage))
                {
                    _logger.Warning("QueryHistoryAsync: Query validation failed: {0}", errorMessage);
                    throw new ArgumentException(errorMessage);
                }

                // Convert DTO sang Criteria (tránh circular dependency)
                var criteria = ConvertDtoToCriteria(query);

                // Query từ repository
                var entities = await Task.Run(() => GetDataAccess().QueryHistory(criteria));
                
                // Map sang DTO - sử dụng extension method từ StockInListDtoConverter
                var items = entities.ToDtoList();
                
                // Đếm tổng số bản ghi (nếu có pagination)
                int totalCount;
                if (query.UsePagination)
                {
                    totalCount = await Task.Run(() => GetDataAccess().CountHistory(criteria));
                }
                else
                {
                    totalCount = items.Count;
                }

                // Tạo result DTO
                var result = new StockInHistoryResultDto
                {
                    Items = items,
                    TotalCount = totalCount,
                    PageIndex = query.PageIndex,
                    PageSize = query.PageSize
                };

                _logger.Info("QueryHistoryAsync: Query thành công, TotalCount={0}, ItemCount={1}", 
                    totalCount, items.Count);
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error($"QueryHistoryAsync: Lỗi query lịch sử: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Convert StockInHistoryQueryDto sang StockInHistoryQueryCriteria
        /// </summary>
        private StockInHistoryQueryCriteria ConvertDtoToCriteria(StockInHistoryQueryDto dto)
        {
            return new StockInHistoryQueryCriteria
            {
                FromDate = dto.FromDate,
                ToDate = dto.ToDate,
                WarehouseId = dto.WarehouseId,
                WarehouseCode = dto.WarehouseCode,
                LoaiNhapKho = dto.LoaiNhapKho.HasValue ? (int?)dto.LoaiNhapKho.Value : null,
                TrangThai = dto.TrangThai.HasValue ? (int?)dto.TrangThai.Value : null,
                TrangThaiList = dto.TrangThaiList?.Select(s => (int)s).ToArray(),
                SupplierId = dto.SupplierId,
                SupplierCode = dto.SupplierCode,
                PurchaseOrderId = dto.PurchaseOrderId,
                PurchaseOrderNumber = dto.PurchaseOrderNumber,
                SearchText = dto.SearchText,
                StockInNumber = dto.StockInNumber,
                OrderBy = dto.OrderBy,
                OrderDirection = dto.OrderDirection,
                PageIndex = dto.PageIndex,
                PageSize = dto.PageSize
            };
        }

        #endregion
    }
}
