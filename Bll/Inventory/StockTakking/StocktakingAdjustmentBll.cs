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
    /// Business Logic Layer cho StocktakingAdjustment
    /// Quản lý các thao tác business logic với StocktakingAdjustment (Điều chỉnh kho sau kiểm kho)
    /// </summary>
    public class StocktakingAdjustmentBll
    {
        #region Fields

        private IStocktakingAdjustmentRepository _stocktakingAdjustmentRepository;
        private readonly ILogger _logger;
        private readonly object _lockObject = new object();

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor mặc định
        /// </summary>
        public StocktakingAdjustmentBll()
        {
            _logger = LoggerFactory.CreateLogger(LogCategory.BLL);
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Lấy hoặc khởi tạo StocktakingAdjustmentRepository (lazy initialization)
        /// </summary>
        private IStocktakingAdjustmentRepository GetStocktakingAdjustmentRepository()
        {
            if (_stocktakingAdjustmentRepository == null)
            {
                lock (_lockObject)
                {
                    if (_stocktakingAdjustmentRepository == null)
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

                            _stocktakingAdjustmentRepository = new StocktakingAdjustmentRepository(globalConnectionString);
                        }
                        catch (Exception ex)
                        {
                            _logger.Error($"Lỗi khởi tạo StocktakingAdjustmentRepository: {ex.Message}", ex);
                            throw;
                        }
                    }
                }
            }

            return _stocktakingAdjustmentRepository;
        }

        #endregion

        #region ========== READ OPERATIONS ==========

        /// <summary>
        /// Lấy tất cả StocktakingAdjustment
        /// </summary>
        /// <returns>Danh sách tất cả StocktakingAdjustmentDto</returns>
        public List<StocktakingAdjustmentDto> GetAll()
        {
            try
            {
                _logger.Debug("GetAll: Lấy tất cả điều chỉnh kiểm kho");

                var dtos = GetStocktakingAdjustmentRepository().GetAll();

                _logger.Info("GetAll: Lấy được {0} điều chỉnh kiểm kho", dtos.Count);
                return dtos;
            }
            catch (Exception ex)
            {
                _logger.Error($"GetAll: Lỗi lấy danh sách điều chỉnh kiểm kho: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Lấy StocktakingAdjustment theo ID
        /// </summary>
        /// <param name="id">ID của StocktakingAdjustment</param>
        /// <returns>StocktakingAdjustmentDto hoặc null</returns>
        public StocktakingAdjustmentDto GetById(Guid id)
        {
            try
            {
                _logger.Debug("GetById: Lấy điều chỉnh kiểm kho, Id={0}", id);

                var dto = GetStocktakingAdjustmentRepository().GetById(id);

                if (dto == null)
                {
                    _logger.Warning("GetById: Không tìm thấy điều chỉnh kiểm kho, Id={0}", id);
                }
                else
                {
                    _logger.Info("GetById: Lấy điều chỉnh kiểm kho thành công, Id={0}", id);
                }

                return dto;
            }
            catch (Exception ex)
            {
                _logger.Error($"GetById: Lỗi lấy điều chỉnh kiểm kho: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Lấy danh sách StocktakingAdjustment theo StocktakingMasterId
        /// </summary>
        /// <param name="stocktakingMasterId">ID phiếu kiểm kho</param>
        /// <returns>Danh sách StocktakingAdjustmentDto</returns>
        public List<StocktakingAdjustmentDto> GetByStocktakingMasterId(Guid stocktakingMasterId)
        {
            try
            {
                _logger.Debug("GetByStocktakingMasterId: Lấy danh sách điều chỉnh, StocktakingMasterId={0}", stocktakingMasterId);

                var dtos = GetStocktakingAdjustmentRepository().GetByStocktakingMasterId(stocktakingMasterId);

                _logger.Info("GetByStocktakingMasterId: Lấy được {0} điều chỉnh", dtos.Count);
                return dtos;
            }
            catch (Exception ex)
            {
                _logger.Error($"GetByStocktakingMasterId: Lỗi lấy danh sách điều chỉnh: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Lấy danh sách StocktakingAdjustment theo StocktakingDetailId
        /// </summary>
        /// <param name="stocktakingDetailId">ID chi tiết kiểm kho</param>
        /// <returns>Danh sách StocktakingAdjustmentDto</returns>
        public List<StocktakingAdjustmentDto> GetByStocktakingDetailId(Guid stocktakingDetailId)
        {
            try
            {
                _logger.Debug("GetByStocktakingDetailId: Lấy danh sách điều chỉnh, StocktakingDetailId={0}", stocktakingDetailId);

                var dtos = GetStocktakingAdjustmentRepository().GetByStocktakingDetailId(stocktakingDetailId);

                _logger.Info("GetByStocktakingDetailId: Lấy được {0} điều chỉnh", dtos.Count);
                return dtos;
            }
            catch (Exception ex)
            {
                _logger.Error($"GetByStocktakingDetailId: Lỗi lấy danh sách điều chỉnh: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Lấy danh sách StocktakingAdjustment chưa được áp dụng
        /// </summary>
        /// <param name="stocktakingMasterId">ID phiếu kiểm kho (tùy chọn)</param>
        /// <returns>Danh sách StocktakingAdjustmentDto</returns>
        public List<StocktakingAdjustmentDto> GetUnapplied(Guid? stocktakingMasterId = null)
        {
            try
            {
                _logger.Debug("GetUnapplied: Lấy danh sách điều chỉnh chưa áp dụng, StocktakingMasterId={0}", stocktakingMasterId);

                var dtos = GetStocktakingAdjustmentRepository().GetUnapplied(stocktakingMasterId);

                _logger.Info("GetUnapplied: Lấy được {0} điều chỉnh chưa áp dụng", dtos.Count);
                return dtos;
            }
            catch (Exception ex)
            {
                _logger.Error($"GetUnapplied: Lỗi lấy danh sách điều chỉnh: {ex.Message}", ex);
                throw;
            }
        }

        #endregion

        #region ========== CREATE/UPDATE OPERATIONS ==========

        /// <summary>
        /// Lưu hoặc cập nhật StocktakingAdjustment
        /// </summary>
        /// <param name="dto">StocktakingAdjustmentDto cần lưu</param>
        /// <returns>StocktakingAdjustmentDto đã được lưu</returns>
        public StocktakingAdjustmentDto SaveOrUpdate(StocktakingAdjustmentDto dto)
        {
            try
            {
                if (dto == null)
                    throw new ArgumentNullException(nameof(dto));

                _logger.Debug("SaveOrUpdate: Bắt đầu lưu điều chỉnh kiểm kho, Id={0}", dto.Id);

                // Business logic validation
                ValidateBeforeSave(dto);

                // Tính toán AdjustmentValue nếu có UnitPrice
                if (dto.UnitPrice.HasValue)
                {
                    dto.AdjustmentValue = dto.AdjustmentQuantity * dto.UnitPrice.Value;
                }

                var result = GetStocktakingAdjustmentRepository().SaveOrUpdate(dto);

                _logger.Info("SaveOrUpdate: Lưu điều chỉnh kiểm kho thành công, Id={0}", result?.Id ?? dto.Id);
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error($"SaveOrUpdate: Lỗi lưu điều chỉnh kiểm kho: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Validate dữ liệu trước khi lưu
        /// </summary>
        /// <param name="dto">StocktakingAdjustmentDto cần validate</param>
        private void ValidateBeforeSave(StocktakingAdjustmentDto dto)
        {
            // Kiểm tra StocktakingMasterId không được rỗng
            if (dto.StocktakingMasterId == Guid.Empty)
            {
                throw new ArgumentException("StocktakingMasterId không được để trống", nameof(dto));
            }

            // Kiểm tra StocktakingDetailId không được rỗng
            if (dto.StocktakingDetailId == Guid.Empty)
            {
                throw new ArgumentException("StocktakingDetailId không được để trống", nameof(dto));
            }

            // Kiểm tra ProductVariantId không được rỗng
            if (dto.ProductVariantId == Guid.Empty)
            {
                throw new ArgumentException("ProductVariantId không được để trống", nameof(dto));
            }

            // Kiểm tra AdjustmentQuantity không được bằng 0
            if (dto.AdjustmentQuantity == 0)
            {
                throw new ArgumentException("Số lượng điều chỉnh không được bằng 0", nameof(dto));
            }
        }

        #endregion

        #region ========== DELETE OPERATIONS ==========

        /// <summary>
        /// Xóa StocktakingAdjustment theo ID (soft delete)
        /// </summary>
        /// <param name="id">ID của StocktakingAdjustment cần xóa</param>
        /// <returns>True nếu xóa thành công, False nếu không tìm thấy</returns>
        public bool Delete(Guid id)
        {
            try
            {
                _logger.Debug("Delete: Xóa điều chỉnh kiểm kho, Id={0}", id);

                var result = GetStocktakingAdjustmentRepository().Delete(id);

                if (result)
                {
                    _logger.Info("Delete: Xóa điều chỉnh kiểm kho thành công (soft delete), Id={0}", id);
                }
                else
                {
                    _logger.Warning("Delete: Không tìm thấy điều chỉnh kiểm kho để xóa, Id={0}", id);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.Error($"Delete: Lỗi xóa điều chỉnh kiểm kho: {ex.Message}", ex);
                throw;
            }
        }

        #endregion
    }
}
