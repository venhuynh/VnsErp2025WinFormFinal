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
    /// Business Logic Layer cho StocktakingDetail
    /// Quản lý các thao tác business logic với StocktakingDetail (Chi tiết kiểm kho)
    /// </summary>
    public class StocktakingDetailBll
    {
        #region Fields

        private IStocktakingDetailRepository _stocktakingDetailRepository;
        private readonly ILogger _logger;
        private readonly object _lockObject = new object();

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor mặc định
        /// </summary>
        public StocktakingDetailBll()
        {
            _logger = LoggerFactory.CreateLogger(LogCategory.BLL);
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Lấy hoặc khởi tạo StocktakingDetailRepository (lazy initialization)
        /// </summary>
        private IStocktakingDetailRepository GetStocktakingDetailRepository()
        {
            if (_stocktakingDetailRepository == null)
            {
                lock (_lockObject)
                {
                    if (_stocktakingDetailRepository == null)
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

                            _stocktakingDetailRepository = new StocktakingDetailRepository(globalConnectionString);
                        }
                        catch (Exception ex)
                        {
                            _logger.Error($"Lỗi khởi tạo StocktakingDetailRepository: {ex.Message}", ex);
                            throw;
                        }
                    }
                }
            }

            return _stocktakingDetailRepository;
        }

        #endregion

        #region ========== READ OPERATIONS ==========

        /// <summary>
        /// Lấy tất cả StocktakingDetail
        /// </summary>
        /// <returns>Danh sách tất cả StocktakingDetailDto</returns>
        public List<StocktakingDetailDto> GetAll()
        {
            try
            {
                _logger.Debug("GetAll: Lấy tất cả chi tiết kiểm kho");

                var dtos = GetStocktakingDetailRepository().GetAll();

                _logger.Info("GetAll: Lấy được {0} chi tiết kiểm kho", dtos.Count);
                return dtos;
            }
            catch (Exception ex)
            {
                _logger.Error($"GetAll: Lỗi lấy danh sách chi tiết kiểm kho: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Lấy StocktakingDetail theo ID
        /// </summary>
        /// <param name="id">ID của StocktakingDetail</param>
        /// <returns>StocktakingDetailDto hoặc null</returns>
        public StocktakingDetailDto GetById(Guid id)
        {
            try
            {
                _logger.Debug("GetById: Lấy chi tiết kiểm kho, Id={0}", id);

                var dto = GetStocktakingDetailRepository().GetById(id);

                if (dto == null)
                {
                    _logger.Warning("GetById: Không tìm thấy chi tiết kiểm kho, Id={0}", id);
                }
                else
                {
                    _logger.Info("GetById: Lấy chi tiết kiểm kho thành công, Id={0}", id);
                }

                return dto;
            }
            catch (Exception ex)
            {
                _logger.Error($"GetById: Lỗi lấy chi tiết kiểm kho: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Lấy danh sách StocktakingDetail theo StocktakingMasterId
        /// </summary>
        /// <param name="stocktakingMasterId">ID phiếu kiểm kho</param>
        /// <returns>Danh sách StocktakingDetailDto</returns>
        public List<StocktakingDetailDto> GetByStocktakingMasterId(Guid stocktakingMasterId)
        {
            try
            {
                _logger.Debug("GetByStocktakingMasterId: Lấy danh sách chi tiết kiểm kho, StocktakingMasterId={0}", stocktakingMasterId);

                var dtos = GetStocktakingDetailRepository().GetByStocktakingMasterId(stocktakingMasterId);

                _logger.Info("GetByStocktakingMasterId: Lấy được {0} chi tiết kiểm kho", dtos.Count);
                return dtos;
            }
            catch (Exception ex)
            {
                _logger.Error($"GetByStocktakingMasterId: Lỗi lấy danh sách chi tiết kiểm kho: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Lấy danh sách StocktakingDetail theo ProductVariantId
        /// </summary>
        /// <param name="productVariantId">ID biến thể sản phẩm</param>
        /// <returns>Danh sách StocktakingDetailDto</returns>
        public List<StocktakingDetailDto> GetByProductVariantId(Guid productVariantId)
        {
            try
            {
                _logger.Debug("GetByProductVariantId: Lấy danh sách chi tiết kiểm kho, ProductVariantId={0}", productVariantId);

                var dtos = GetStocktakingDetailRepository().GetByProductVariantId(productVariantId);

                _logger.Info("GetByProductVariantId: Lấy được {0} chi tiết kiểm kho", dtos.Count);
                return dtos;
            }
            catch (Exception ex)
            {
                _logger.Error($"GetByProductVariantId: Lỗi lấy danh sách chi tiết kiểm kho: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Lấy danh sách StocktakingDetail chưa được kiểm đếm
        /// </summary>
        /// <param name="stocktakingMasterId">ID phiếu kiểm kho</param>
        /// <returns>Danh sách StocktakingDetailDto</returns>
        public List<StocktakingDetailDto> GetUncountedByStocktakingMasterId(Guid stocktakingMasterId)
        {
            try
            {
                _logger.Debug("GetUncountedByStocktakingMasterId: Lấy danh sách chi tiết chưa kiểm đếm, StocktakingMasterId={0}", stocktakingMasterId);

                var dtos = GetStocktakingDetailRepository().GetUncountedByStocktakingMasterId(stocktakingMasterId);

                _logger.Info("GetUncountedByStocktakingMasterId: Lấy được {0} chi tiết chưa kiểm đếm", dtos.Count);
                return dtos;
            }
            catch (Exception ex)
            {
                _logger.Error($"GetUncountedByStocktakingMasterId: Lỗi lấy danh sách chi tiết kiểm kho: {ex.Message}", ex);
                throw;
            }
        }

        #endregion

        #region ========== CREATE/UPDATE OPERATIONS ==========

        /// <summary>
        /// Lưu hoặc cập nhật StocktakingDetail
        /// </summary>
        /// <param name="dto">StocktakingDetailDto cần lưu</param>
        /// <returns>StocktakingDetailDto đã được lưu</returns>
        public StocktakingDetailDto SaveOrUpdate(StocktakingDetailDto dto)
        {
            try
            {
                if (dto == null)
                    throw new ArgumentNullException(nameof(dto));

                _logger.Debug("SaveOrUpdate: Bắt đầu lưu chi tiết kiểm kho, Id={0}", dto.Id);

                // Business logic validation
                ValidateBeforeSave(dto);

                // Tính toán DifferenceQuantity nếu chưa có
                if (dto.CountedQuantity.HasValue)
                {
                    dto.DifferenceQuantity = dto.CountedQuantity.Value - dto.SystemQuantity;
                }

                // Tính toán DifferenceValue nếu có UnitPrice
                if (dto.UnitPrice.HasValue && dto.CountedQuantity.HasValue)
                {
                    dto.CountedValue = dto.CountedQuantity.Value * dto.UnitPrice.Value;
                    if (dto.SystemValue.HasValue)
                    {
                        dto.DifferenceValue = dto.CountedValue.Value - dto.SystemValue.Value;
                    }
                }

                var result = GetStocktakingDetailRepository().SaveOrUpdate(dto);

                _logger.Info("SaveOrUpdate: Lưu chi tiết kiểm kho thành công, Id={0}", result?.Id ?? dto.Id);
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error($"SaveOrUpdate: Lỗi lưu chi tiết kiểm kho: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Lưu hoặc cập nhật danh sách StocktakingDetail
        /// </summary>
        /// <param name="dtos">Danh sách StocktakingDetailDto cần lưu</param>
        /// <returns>Danh sách StocktakingDetailDto đã được lưu</returns>
        public List<StocktakingDetailDto> SaveOrUpdateList(List<StocktakingDetailDto> dtos)
        {
            try
            {
                if (dtos == null || dtos.Count == 0)
                {
                    _logger.Warning("SaveOrUpdateList: Danh sách rỗng");
                    return new List<StocktakingDetailDto>();
                }

                _logger.Debug("SaveOrUpdateList: Bắt đầu lưu {0} chi tiết kiểm kho", dtos.Count);

                // Validate và tính toán cho từng dto
                foreach (var dto in dtos)
                {
                    ValidateBeforeSave(dto);

                    // Tính toán DifferenceQuantity nếu chưa có
                    if (dto.CountedQuantity.HasValue)
                    {
                        dto.DifferenceQuantity = dto.CountedQuantity.Value - dto.SystemQuantity;
                    }

                    // Tính toán DifferenceValue nếu có UnitPrice
                    if (dto.UnitPrice.HasValue && dto.CountedQuantity.HasValue)
                    {
                        dto.CountedValue = dto.CountedQuantity.Value * dto.UnitPrice.Value;
                        if (dto.SystemValue.HasValue)
                        {
                            dto.DifferenceValue = dto.CountedValue.Value - dto.SystemValue.Value;
                        }
                    }
                }

                var result = GetStocktakingDetailRepository().SaveOrUpdateList(dtos);

                _logger.Info("SaveOrUpdateList: Lưu {0} chi tiết kiểm kho thành công", result.Count);
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error($"SaveOrUpdateList: Lỗi lưu danh sách chi tiết kiểm kho: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Validate dữ liệu trước khi lưu
        /// </summary>
        /// <param name="dto">StocktakingDetailDto cần validate</param>
        private void ValidateBeforeSave(StocktakingDetailDto dto)
        {
            // Kiểm tra StocktakingMasterId không được rỗng
            if (dto.StocktakingMasterId == Guid.Empty)
            {
                throw new ArgumentException("StocktakingMasterId không được để trống", nameof(dto));
            }

            // Kiểm tra ProductVariantId không được rỗng
            if (dto.ProductVariantId == Guid.Empty)
            {
                throw new ArgumentException("ProductVariantId không được để trống", nameof(dto));
            }

            // Kiểm tra SystemQuantity phải >= 0
            if (dto.SystemQuantity < 0)
            {
                throw new ArgumentException("Số lượng hệ thống không được âm", nameof(dto));
            }

            // Kiểm tra CountedQuantity phải >= 0 nếu có
            if (dto.CountedQuantity.HasValue && dto.CountedQuantity.Value < 0)
            {
                throw new ArgumentException("Số lượng đã kiểm không được âm", nameof(dto));
            }
        }

        #endregion

        #region ========== DELETE OPERATIONS ==========

        /// <summary>
        /// Xóa StocktakingDetail theo ID (soft delete)
        /// </summary>
        /// <param name="id">ID của StocktakingDetail cần xóa</param>
        /// <returns>True nếu xóa thành công, False nếu không tìm thấy</returns>
        public bool Delete(Guid id)
        {
            try
            {
                _logger.Debug("Delete: Xóa chi tiết kiểm kho, Id={0}", id);

                var result = GetStocktakingDetailRepository().Delete(id);

                if (result)
                {
                    _logger.Info("Delete: Xóa chi tiết kiểm kho thành công (soft delete), Id={0}", id);
                }
                else
                {
                    _logger.Warning("Delete: Không tìm thấy chi tiết kiểm kho để xóa, Id={0}", id);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.Error($"Delete: Lỗi xóa chi tiết kiểm kho: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Xóa tất cả StocktakingDetail theo StocktakingMasterId (soft delete)
        /// </summary>
        /// <param name="stocktakingMasterId">ID phiếu kiểm kho</param>
        /// <returns>Số lượng bản ghi đã xóa</returns>
        public int DeleteByStocktakingMasterId(Guid stocktakingMasterId)
        {
            try
            {
                _logger.Debug("DeleteByStocktakingMasterId: Xóa chi tiết kiểm kho, StocktakingMasterId={0}", stocktakingMasterId);

                var count = GetStocktakingDetailRepository().DeleteByStocktakingMasterId(stocktakingMasterId);

                _logger.Info("DeleteByStocktakingMasterId: Xóa {0} chi tiết kiểm kho thành công", count);
                return count;
            }
            catch (Exception ex)
            {
                _logger.Error($"DeleteByStocktakingMasterId: Lỗi xóa chi tiết kiểm kho: {ex.Message}", ex);
                throw;
            }
        }

        #endregion
    }
}
