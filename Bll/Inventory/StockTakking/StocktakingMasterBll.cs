using Dal.Connection;
using Dal.DataAccess.Implementations.Inventory.InventoryManagement;
using Dal.DataAccess.Interfaces.Inventory.InventoryManagement;
using DTO.Inventory.StockTakking;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;
using System;
using System.Collections.Generic;

namespace Bll.Inventory.StockTakking
{
    /// <summary>
    /// Business Logic Layer cho StocktakingMaster
    /// Quản lý các thao tác business logic với StocktakingMaster (Phiếu kiểm kho)
    /// </summary>
    public class StocktakingMasterBll
    {
        #region Fields

        private IStocktakingMasterRepository _stocktakingMasterRepository;
        private readonly ILogger _logger;
        private readonly object _lockObject = new object();

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor mặc định
        /// </summary>
        public StocktakingMasterBll()
        {
            _logger = LoggerFactory.CreateLogger(LogCategory.BLL);
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Lấy hoặc khởi tạo StocktakingMasterRepository (lazy initialization)
        /// </summary>
        private IStocktakingMasterRepository GetStocktakingMasterRepository()
        {
            if (_stocktakingMasterRepository == null)
            {
                lock (_lockObject)
                {
                    if (_stocktakingMasterRepository == null)
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

                            _stocktakingMasterRepository = new StocktakingMasterRepository(globalConnectionString);
                        }
                        catch (Exception ex)
                        {
                            _logger.Error($"Lỗi khởi tạo StocktakingMasterRepository: {ex.Message}", ex);
                            throw;
                        }
                    }
                }
            }

            return _stocktakingMasterRepository;
        }

        #endregion

        #region ========== READ OPERATIONS ==========

        /// <summary>
        /// Lấy tất cả StocktakingMaster
        /// </summary>
        /// <returns>Danh sách tất cả StocktakingMasterDto</returns>
        public List<StocktakingMasterDto> GetAll()
        {
            try
            {
                _logger.Debug("GetAll: Lấy tất cả phiếu kiểm kho");

                var dtos = GetStocktakingMasterRepository().GetAll();

                _logger.Info("GetAll: Lấy được {0} phiếu kiểm kho", dtos.Count);
                return dtos;
            }
            catch (Exception ex)
            {
                _logger.Error($"GetAll: Lỗi lấy danh sách phiếu kiểm kho: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Lấy StocktakingMaster theo ID
        /// </summary>
        /// <param name="id">ID của StocktakingMaster</param>
        /// <returns>StocktakingMasterDto hoặc null</returns>
        public StocktakingMasterDto GetById(Guid id)
        {
            try
            {
                _logger.Debug("GetById: Lấy phiếu kiểm kho, Id={0}", id);

                var dto = GetStocktakingMasterRepository().GetById(id);

                if (dto == null)
                {
                    _logger.Warning("GetById: Không tìm thấy phiếu kiểm kho, Id={0}", id);
                }
                else
                {
                    _logger.Info("GetById: Lấy phiếu kiểm kho thành công, Id={0}", id);
                }

                return dto;
            }
            catch (Exception ex)
            {
                _logger.Error($"GetById: Lỗi lấy phiếu kiểm kho: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Lấy danh sách StocktakingMaster theo WarehouseId
        /// </summary>
        /// <param name="warehouseId">ID kho</param>
        /// <returns>Danh sách StocktakingMasterDto</returns>
        public List<StocktakingMasterDto> GetByWarehouseId(Guid warehouseId)
        {
            try
            {
                _logger.Debug("GetByWarehouseId: Lấy danh sách phiếu kiểm kho, WarehouseId={0}", warehouseId);

                var dtos = GetStocktakingMasterRepository().GetByWarehouseId(warehouseId);

                _logger.Info("GetByWarehouseId: Lấy được {0} phiếu kiểm kho", dtos.Count);
                return dtos;
            }
            catch (Exception ex)
            {
                _logger.Error($"GetByWarehouseId: Lỗi lấy danh sách phiếu kiểm kho: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Lấy danh sách StocktakingMaster theo trạng thái
        /// </summary>
        /// <param name="stocktakingStatus">Trạng thái kiểm kho</param>
        /// <returns>Danh sách StocktakingMasterDto</returns>
        public List<StocktakingMasterDto> GetByStatus(int stocktakingStatus)
        {
            try
            {
                _logger.Debug("GetByStatus: Lấy danh sách phiếu kiểm kho, Status={0}", stocktakingStatus);

                var dtos = GetStocktakingMasterRepository().GetByStatus(stocktakingStatus);

                _logger.Info("GetByStatus: Lấy được {0} phiếu kiểm kho với Status={1}", dtos.Count, stocktakingStatus);
                return dtos;
            }
            catch (Exception ex)
            {
                _logger.Error($"GetByStatus: Lỗi lấy danh sách phiếu kiểm kho: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Lấy danh sách StocktakingMaster theo loại
        /// </summary>
        /// <param name="stocktakingType">Loại kiểm kho</param>
        /// <returns>Danh sách StocktakingMasterDto</returns>
        public List<StocktakingMasterDto> GetByType(int stocktakingType)
        {
            try
            {
                _logger.Debug("GetByType: Lấy danh sách phiếu kiểm kho, Type={0}", stocktakingType);

                var dtos = GetStocktakingMasterRepository().GetByType(stocktakingType);

                _logger.Info("GetByType: Lấy được {0} phiếu kiểm kho với Type={1}", dtos.Count, stocktakingType);
                return dtos;
            }
            catch (Exception ex)
            {
                _logger.Error($"GetByType: Lỗi lấy danh sách phiếu kiểm kho: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Tìm StocktakingMaster theo số phiếu
        /// </summary>
        /// <param name="voucherNumber">Số phiếu kiểm kho</param>
        /// <returns>StocktakingMasterDto nếu tìm thấy, null nếu không tìm thấy</returns>
        public StocktakingMasterDto FindByVoucherNumber(string voucherNumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(voucherNumber))
                {
                    _logger.Warning("FindByVoucherNumber: VoucherNumber is null or empty");
                    return null;
                }

                _logger.Debug("FindByVoucherNumber: Tìm phiếu kiểm kho, VoucherNumber={0}", voucherNumber);

                var dto = GetStocktakingMasterRepository().FindByVoucherNumber(voucherNumber);

                if (dto == null)
                {
                    _logger.Warning("FindByVoucherNumber: Không tìm thấy phiếu kiểm kho với VoucherNumber, VoucherNumber={0}", voucherNumber);
                }
                else
                {
                    _logger.Info("FindByVoucherNumber: Tìm thấy phiếu kiểm kho, Id={0}, VoucherNumber={1}", dto.Id, voucherNumber);
                }

                return dto;
            }
            catch (Exception ex)
            {
                _logger.Error($"FindByVoucherNumber: Lỗi tìm phiếu kiểm kho: {ex.Message}", ex);
                throw;
            }
        }

        #endregion

        #region ========== CREATE/UPDATE OPERATIONS ==========

        /// <summary>
        /// Lưu hoặc cập nhật StocktakingMaster
        /// </summary>
        /// <param name="dto">StocktakingMasterDto cần lưu</param>
        /// <returns>StocktakingMasterDto đã được lưu</returns>
        public StocktakingMasterDto SaveOrUpdate(StocktakingMasterDto dto)
        {
            try
            {
                if (dto == null)
                    throw new ArgumentNullException(nameof(dto));

                _logger.Debug("SaveOrUpdate: Bắt đầu lưu phiếu kiểm kho, Id={0}, VoucherNumber={1}",
                    dto.Id, dto.VoucherNumber);

                // Business logic validation
                ValidateBeforeSave(dto);

                var result = GetStocktakingMasterRepository().SaveOrUpdate(dto);

                _logger.Info("SaveOrUpdate: Lưu phiếu kiểm kho thành công, Id={0}", result?.Id ?? dto.Id);
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error($"SaveOrUpdate: Lỗi lưu phiếu kiểm kho: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Validate dữ liệu trước khi lưu
        /// </summary>
        /// <param name="dto">StocktakingMasterDto cần validate</param>
        private void ValidateBeforeSave(StocktakingMasterDto dto)
        {
            // Kiểm tra WarehouseId không được rỗng
            if (dto.WarehouseId == Guid.Empty)
            {
                throw new ArgumentException("WarehouseId không được để trống", nameof(dto));
            }

            // Kiểm tra VoucherNumber không được rỗng
            if (string.IsNullOrWhiteSpace(dto.VoucherNumber))
            {
                throw new ArgumentException("Số phiếu kiểm kho không được để trống", nameof(dto));
            }

            // Kiểm tra tính duy nhất của VoucherNumber (nếu tạo mới)
            if (dto.Id == Guid.Empty)
            {
                var existing = FindByVoucherNumber(dto.VoucherNumber);
                if (existing != null)
                {
                    throw new InvalidOperationException($"Số phiếu kiểm kho '{dto.VoucherNumber}' đã tồn tại");
                }
            }
        }

        #endregion

        #region ========== DELETE OPERATIONS ==========

        /// <summary>
        /// Xóa StocktakingMaster theo ID (soft delete)
        /// </summary>
        /// <param name="id">ID của StocktakingMaster cần xóa</param>
        /// <returns>True nếu xóa thành công, False nếu không tìm thấy</returns>
        public bool Delete(Guid id)
        {
            try
            {
                _logger.Debug("Delete: Xóa phiếu kiểm kho, Id={0}", id);

                var result = GetStocktakingMasterRepository().Delete(id);

                if (result)
                {
                    _logger.Info("Delete: Xóa phiếu kiểm kho thành công (soft delete), Id={0}", id);
                }
                else
                {
                    _logger.Warning("Delete: Không tìm thấy phiếu kiểm kho để xóa, Id={0}", id);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.Error($"Delete: Lỗi xóa phiếu kiểm kho: {ex.Message}", ex);
                throw;
            }
        }

        #endregion
    }
}
