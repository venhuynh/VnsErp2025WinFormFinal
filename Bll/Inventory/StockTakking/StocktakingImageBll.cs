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
    /// Business Logic Layer cho StocktakingImage
    /// Quản lý các thao tác business logic với StocktakingImage (Hình ảnh kiểm kho)
    /// </summary>
    public class StocktakingImageBll
    {
        #region Fields

        private IStocktakingImageRepository _stocktakingImageRepository;
        private readonly ILogger _logger;
        private readonly object _lockObject = new object();

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor mặc định
        /// </summary>
        public StocktakingImageBll()
        {
            _logger = LoggerFactory.CreateLogger(LogCategory.BLL);
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Lấy hoặc khởi tạo StocktakingImageRepository (lazy initialization)
        /// </summary>
        private IStocktakingImageRepository GetStocktakingImageRepository()
        {
            if (_stocktakingImageRepository == null)
            {
                lock (_lockObject)
                {
                    if (_stocktakingImageRepository == null)
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

                            _stocktakingImageRepository = new StocktakingImageRepository(globalConnectionString);
                        }
                        catch (Exception ex)
                        {
                            _logger.Error($"Lỗi khởi tạo StocktakingImageRepository: {ex.Message}", ex);
                            throw;
                        }
                    }
                }
            }

            return _stocktakingImageRepository;
        }

        #endregion

        #region ========== READ OPERATIONS ==========

        /// <summary>
        /// Lấy tất cả StocktakingImage
        /// </summary>
        /// <returns>Danh sách tất cả StocktakingImageDto</returns>
        public List<StocktakingImageDto> GetAll()
        {
            try
            {
                _logger.Debug("GetAll: Lấy tất cả hình ảnh kiểm kho");

                var dtos = GetStocktakingImageRepository().GetAll();

                _logger.Info("GetAll: Lấy được {0} hình ảnh kiểm kho", dtos.Count);
                return dtos;
            }
            catch (Exception ex)
            {
                _logger.Error($"GetAll: Lỗi lấy danh sách hình ảnh kiểm kho: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Lấy StocktakingImage theo ID
        /// </summary>
        /// <param name="id">ID của StocktakingImage</param>
        /// <returns>StocktakingImageDto hoặc null</returns>
        public StocktakingImageDto GetById(Guid id)
        {
            try
            {
                _logger.Debug("GetById: Lấy hình ảnh kiểm kho, Id={0}", id);

                var dto = GetStocktakingImageRepository().GetById(id);

                if (dto == null)
                {
                    _logger.Warning("GetById: Không tìm thấy hình ảnh kiểm kho, Id={0}", id);
                }
                else
                {
                    _logger.Info("GetById: Lấy hình ảnh kiểm kho thành công, Id={0}", id);
                }

                return dto;
            }
            catch (Exception ex)
            {
                _logger.Error($"GetById: Lỗi lấy hình ảnh kiểm kho: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Lấy danh sách StocktakingImage theo StocktakingMasterId
        /// </summary>
        /// <param name="stocktakingMasterId">ID phiếu kiểm kho</param>
        /// <returns>Danh sách StocktakingImageDto</returns>
        public List<StocktakingImageDto> GetByStocktakingMasterId(Guid? stocktakingMasterId)
        {
            try
            {
                _logger.Debug("GetByStocktakingMasterId: Lấy danh sách hình ảnh, StocktakingMasterId={0}", stocktakingMasterId);

                var dtos = GetStocktakingImageRepository().GetByStocktakingMasterId(stocktakingMasterId);

                _logger.Info("GetByStocktakingMasterId: Lấy được {0} hình ảnh", dtos.Count);
                return dtos;
            }
            catch (Exception ex)
            {
                _logger.Error($"GetByStocktakingMasterId: Lỗi lấy danh sách hình ảnh: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Lấy danh sách StocktakingImage theo StocktakingDetailId
        /// </summary>
        /// <param name="stocktakingDetailId">ID chi tiết kiểm kho</param>
        /// <returns>Danh sách StocktakingImageDto</returns>
        public List<StocktakingImageDto> GetByStocktakingDetailId(Guid? stocktakingDetailId)
        {
            try
            {
                _logger.Debug("GetByStocktakingDetailId: Lấy danh sách hình ảnh, StocktakingDetailId={0}", stocktakingDetailId);

                var dtos = GetStocktakingImageRepository().GetByStocktakingDetailId(stocktakingDetailId);

                _logger.Info("GetByStocktakingDetailId: Lấy được {0} hình ảnh", dtos.Count);
                return dtos;
            }
            catch (Exception ex)
            {
                _logger.Error($"GetByStocktakingDetailId: Lỗi lấy danh sách hình ảnh: {ex.Message}", ex);
                throw;
            }
        }

        #endregion

        #region ========== CREATE/UPDATE OPERATIONS ==========

        /// <summary>
        /// Lưu hoặc cập nhật StocktakingImage
        /// </summary>
        /// <param name="dto">StocktakingImageDto cần lưu</param>
        /// <returns>StocktakingImageDto đã được lưu</returns>
        public StocktakingImageDto SaveOrUpdate(StocktakingImageDto dto)
        {
            try
            {
                if (dto == null)
                    throw new ArgumentNullException(nameof(dto));

                _logger.Debug("SaveOrUpdate: Bắt đầu lưu hình ảnh kiểm kho, Id={0}", dto.Id);

                // Business logic validation
                ValidateBeforeSave(dto);

                // Tính toán FileSize nếu có ImageData
                if (dto.ImageData != null && dto.ImageData.Length > 0)
                {
                    dto.FileSize = dto.ImageData.Length;
                }

                var result = GetStocktakingImageRepository().SaveOrUpdate(dto);

                _logger.Info("SaveOrUpdate: Lưu hình ảnh kiểm kho thành công, Id={0}", result?.Id ?? dto.Id);
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error($"SaveOrUpdate: Lỗi lưu hình ảnh kiểm kho: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Validate dữ liệu trước khi lưu
        /// </summary>
        /// <param name="dto">StocktakingImageDto cần validate</param>
        private void ValidateBeforeSave(StocktakingImageDto dto)
        {
            // Kiểm tra ít nhất một trong StocktakingMasterId hoặc StocktakingDetailId phải có giá trị
            if (!dto.StocktakingMasterId.HasValue && !dto.StocktakingDetailId.HasValue)
            {
                throw new ArgumentException("Phải có ít nhất một trong StocktakingMasterId hoặc StocktakingDetailId", nameof(dto));
            }

            // Kiểm tra CreateBy không được rỗng
            if (dto.CreateBy == Guid.Empty)
            {
                throw new ArgumentException("CreateBy không được để trống", nameof(dto));
            }

            // Kiểm tra ModifiedBy không được rỗng
            if (dto.ModifiedBy == Guid.Empty)
            {
                throw new ArgumentException("ModifiedBy không được để trống", nameof(dto));
            }
        }

        #endregion

        #region ========== DELETE OPERATIONS ==========

        /// <summary>
        /// Xóa StocktakingImage theo ID (hard delete)
        /// </summary>
        /// <param name="id">ID của StocktakingImage cần xóa</param>
        /// <returns>True nếu xóa thành công, False nếu không tìm thấy</returns>
        public bool Delete(Guid id)
        {
            try
            {
                _logger.Debug("Delete: Xóa hình ảnh kiểm kho, Id={0}", id);

                var result = GetStocktakingImageRepository().Delete(id);

                if (result)
                {
                    _logger.Info("Delete: Xóa hình ảnh kiểm kho thành công, Id={0}", id);
                }
                else
                {
                    _logger.Warning("Delete: Không tìm thấy hình ảnh kiểm kho để xóa, Id={0}", id);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.Error($"Delete: Lỗi xóa hình ảnh kiểm kho: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Xóa tất cả StocktakingImage theo StocktakingMasterId (hard delete)
        /// </summary>
        /// <param name="stocktakingMasterId">ID phiếu kiểm kho</param>
        /// <returns>Số lượng bản ghi đã xóa</returns>
        public int DeleteByStocktakingMasterId(Guid stocktakingMasterId)
        {
            try
            {
                _logger.Debug("DeleteByStocktakingMasterId: Xóa hình ảnh, StocktakingMasterId={0}", stocktakingMasterId);

                var count = GetStocktakingImageRepository().DeleteByStocktakingMasterId(stocktakingMasterId);

                _logger.Info("DeleteByStocktakingMasterId: Xóa {0} hình ảnh thành công", count);
                return count;
            }
            catch (Exception ex)
            {
                _logger.Error($"DeleteByStocktakingMasterId: Lỗi xóa hình ảnh: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Xóa tất cả StocktakingImage theo StocktakingDetailId (hard delete)
        /// </summary>
        /// <param name="stocktakingDetailId">ID chi tiết kiểm kho</param>
        /// <returns>Số lượng bản ghi đã xóa</returns>
        public int DeleteByStocktakingDetailId(Guid stocktakingDetailId)
        {
            try
            {
                _logger.Debug("DeleteByStocktakingDetailId: Xóa hình ảnh, StocktakingDetailId={0}", stocktakingDetailId);

                var count = GetStocktakingImageRepository().DeleteByStocktakingDetailId(stocktakingDetailId);

                _logger.Info("DeleteByStocktakingDetailId: Xóa {0} hình ảnh thành công", count);
                return count;
            }
            catch (Exception ex)
            {
                _logger.Error($"DeleteByStocktakingDetailId: Lỗi xóa hình ảnh: {ex.Message}", ex);
                throw;
            }
        }

        #endregion
    }
}
