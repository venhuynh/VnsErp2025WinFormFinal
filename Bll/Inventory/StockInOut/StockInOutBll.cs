using Dal.Connection;
using Dal.DataAccess.Implementations.Inventory.StockIn;
using Dal.DataAccess.Interfaces.Inventory.StockIn;
using DTO.Inventory;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bll.Inventory.StockInOut
{
    /// <summary>
    /// Business Logic Layer cho StockIn/StockOut (Phiếu nhập/xuất kho)
    /// Xử lý các nghiệp vụ liên quan đến phiếu nhập kho và phiếu xuất kho
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

        #region UpSert

        /// <summary>
        /// Lưu phiếu nhập/xuất kho mới (Insert)
        /// </summary>
        /// <param name="masterDto">Thông tin master của phiếu nhập/xuất kho (DTO)</param>
        /// <param name="detailDtos">Danh sách chi tiết của phiếu nhập/xuất kho (DTOs)</param>
        /// <returns>ID của master đã được lưu thành công</returns>
        /// <exception cref="ArgumentNullException">Khi masterDto hoặc detailDtos là null</exception>
        /// <exception cref="ArgumentException">Khi detailDtos rỗng hoặc WarehouseId không hợp lệ</exception>
        public async Task<Guid> SaveAsync(StockInOutMasterForUIDto masterDto, List<StockInOutDetailForUIDto> detailDtos)
        {
            try
            {
                if (masterDto == null)
                {
                    _logger?.Warning("SaveAsync: masterDto is null");
                    throw new ArgumentNullException(nameof(masterDto), "Master DTO không được null");
                }

                if (detailDtos == null || detailDtos.Count == 0)
                {
                    _logger?.Warning("SaveAsync: detailDtos is null or empty");
                    throw new ArgumentException("Danh sách chi tiết không được rỗng", nameof(detailDtos));
                }

                // Validate business rules
                if (masterDto.WarehouseId == Guid.Empty)
                {
                    _logger?.Warning("SaveAsync: WarehouseId is Empty");
                    throw new ArgumentException("Vui lòng chọn kho", nameof(masterDto));
                }

                // Gọi repository để lưu
                var savedMasterId = await GetDataAccess().SaveAsync(masterDto, detailDtos);

                _logger?.Info("SaveAsync: Đã lưu phiếu nhập/xuất kho thành công, MasterId={0}", savedMasterId);

                return savedMasterId;
            }
            catch (ArgumentException)
            {
                // Re-throw ArgumentException để form có thể xử lý
                throw;
            }
            catch (Exception ex)
            {
                _logger?.Error($"SaveAsync: Lỗi lưu phiếu nhập/xuất kho: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Cập nhật phiếu nhập/xuất kho (Update)
        /// </summary>
        /// <param name="masterDto">Thông tin master của phiếu nhập/xuất kho (DTO) - phải có Id hợp lệ</param>
        /// <param name="detailDtos">Danh sách chi tiết của phiếu nhập/xuất kho (DTOs)</param>
        /// <returns>ID của master đã được cập nhật thành công</returns>
        /// <exception cref="ArgumentNullException">Khi masterDto hoặc detailDtos là null</exception>
        /// <exception cref="ArgumentException">Khi masterDto.Id rỗng, detailDtos rỗng hoặc WarehouseId không hợp lệ</exception>
        public async Task<Guid> UpdateAsync(StockInOutMasterForUIDto masterDto, List<StockInOutDetailForUIDto> detailDtos)
        {
            try
            {
                if (masterDto == null)
                {
                    _logger?.Warning("UpdateAsync: masterDto is null");
                    throw new ArgumentNullException(nameof(masterDto), "Master DTO không được null");
                }

                if (masterDto.Id == Guid.Empty)
                {
                    _logger?.Warning("UpdateAsync: masterDto.Id is Empty");
                    throw new ArgumentException("Master ID không được rỗng khi cập nhật", nameof(masterDto));
                }

                if (detailDtos == null || detailDtos.Count == 0)
                {
                    _logger?.Warning("UpdateAsync: detailDtos is null or empty");
                    throw new ArgumentException("Danh sách chi tiết không được rỗng", nameof(detailDtos));
                }

                // Validate business rules
                if (masterDto.WarehouseId == Guid.Empty)
                {
                    _logger?.Warning("UpdateAsync: WarehouseId is Empty");
                    throw new ArgumentException("Vui lòng chọn kho", nameof(masterDto));
                }

                // Gọi repository để cập nhật
                var updatedMasterId = await GetDataAccess().UpdateAsync(masterDto, detailDtos);

                _logger?.Info("UpdateAsync: Đã cập nhật phiếu nhập/xuất kho thành công, MasterId={0}", updatedMasterId);

                return updatedMasterId;
            }
            catch (ArgumentException)
            {
                // Re-throw ArgumentException để form có thể xử lý
                throw;
            }
            catch (Exception ex)
            {
                _logger?.Error($"UpdateAsync: Lỗi cập nhật phiếu nhập/xuất kho: {ex.Message}", ex);
                throw;
            }
        }

        #endregion

        #region Retrieve

        /// <summary>
        /// Lấy thông tin master phiếu nhập/xuất kho theo ID và chuyển đổi sang DTO
        /// </summary>
        /// <param name="stockInOutMasterId">ID phiếu nhập/xuất kho</param>
        /// <returns>StockInOutMasterForUIDto nếu tìm thấy, null nếu không tìm thấy</returns>
        public StockInOutMasterForUIDto GetStockInOutMasterForUIDtoById(Guid stockInOutMasterId)
        {
            try
            {
                if (stockInOutMasterId == Guid.Empty)
                {
                    _logger?.Warning("GetMasterById: stockInOutMasterId là Guid.Empty");
                    return null;
                }

                // Lấy entity từ repository
                return GetDataAccess().GetStockInOutMasterForUIDtoById(stockInOutMasterId);
                
            }
            catch (Exception ex)
            {
                _logger?.Error($"GetMasterById: Lỗi lấy thông tin master: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Lấy danh sách chi tiết của phiếu nhập/xuất kho theo ID master
        /// </summary>
        /// <param name="stockInOutMasterId">ID phiếu nhập/xuất kho</param>
        /// <returns>Danh sách StockInOutDetailForUIDto</returns>
        public List<StockInOutDetailForUIDto> GetStockInOutDetailsByMasterId(Guid stockInOutMasterId)
        {
            try
            {
                if (stockInOutMasterId == Guid.Empty)
                {
                    _logger?.Warning("GetStockInOutDetailsByMasterId: stockInOutMasterId là Guid.Empty");
                    return new List<StockInOutDetailForUIDto>();
                }

                // Lấy details từ repository
                return GetDataAccess().GetStockInOutDetailsByMasterId(stockInOutMasterId);
                
            }
            catch (Exception ex)
            {
                _logger?.Error($"GetStockInOutDetailsByMasterId: Lỗi lấy thông tin details: {ex.Message}", ex);
                throw;
            }
        }

        #endregion


        
    }
}
