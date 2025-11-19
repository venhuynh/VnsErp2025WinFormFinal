using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dal.Connection;
using Dal.DataAccess.Implementations.Inventory.StockIn;
using Dal.DataAccess.Interfaces.Inventory.StockIn;
using Dal.DataContext;
using DTO.Inventory.StockIn;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;

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

        #endregion
    }
}
