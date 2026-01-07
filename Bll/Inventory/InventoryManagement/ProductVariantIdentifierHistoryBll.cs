using Dal.Connection;
using Dal.DataAccess.Implementations.Inventory.InventoryManagement;
using Dal.DataAccess.Interfaces.Inventory.InventoryManagement;
using DTO.Inventory.InventoryManagement;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;
using System;
using System.Collections.Generic;

namespace Bll.Inventory.InventoryManagement
{
    /// <summary>
    /// Business Logic Layer cho ProductVariantIdentifierHistory
    /// Quản lý các thao tác business logic với ProductVariantIdentifierHistory (Lịch sử thay đổi định danh biến thể sản phẩm)
    /// </summary>
    public class ProductVariantIdentifierHistoryBll
    {
        #region Fields

        private IProductVariantIdentifierHistoryRepository _productVariantIdentifierHistoryRepository;
        private readonly ILogger _logger;
        private readonly object Lock = new object();

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor mặc định
        /// </summary>
        public ProductVariantIdentifierHistoryBll()
        {
            _logger = LoggerFactory.CreateLogger(LogCategory.BLL);
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Lấy hoặc khởi tạo ProductVariantIdentifierHistoryRepository (lazy initialization)
        /// </summary>
        private IProductVariantIdentifierHistoryRepository GetProductVariantIdentifierHistoryRepository()
        {
            if (_productVariantIdentifierHistoryRepository == null)
            {
                lock (Lock)
                {
                    if (_productVariantIdentifierHistoryRepository == null)
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

                            _productVariantIdentifierHistoryRepository = new ProductVariantIdentifierHistoryRepository(globalConnectionString);
                        }
                        catch (Exception ex)
                        {
                            _logger.Error($"Lỗi khởi tạo ProductVariantIdentifierHistoryRepository: {ex.Message}", ex);
                            throw;
                        }
                    }
                }
            }

            return _productVariantIdentifierHistoryRepository;
        }

        #endregion

        #region ========== READ OPERATIONS ==========

        /// <summary>
        /// Lấy tất cả ProductVariantIdentifierHistory
        /// </summary>
        /// <returns>Danh sách tất cả ProductVariantIdentifierHistoryDto</returns>
        public List<ProductVariantIdentifierHistoryDto> GetAll()
        {
            try
            {
                _logger.Debug("GetAll: Lấy tất cả lịch sử thay đổi định danh sản phẩm");

                var dtos = GetProductVariantIdentifierHistoryRepository().GetAll();

                _logger.Info("GetAll: Lấy được {0} bản ghi lịch sử", dtos.Count);
                return dtos;
            }
            catch (Exception ex)
            {
                _logger.Error($"GetAll: Lỗi lấy danh sách lịch sử thay đổi: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Lấy ProductVariantIdentifierHistory theo ID
        /// </summary>
        /// <param name="id">ID của ProductVariantIdentifierHistory</param>
        /// <returns>ProductVariantIdentifierHistoryDto hoặc null</returns>
        public ProductVariantIdentifierHistoryDto GetById(Guid id)
        {
            try
            {
                _logger.Debug("GetById: Lấy lịch sử thay đổi, Id={0}", id);

                var dto = GetProductVariantIdentifierHistoryRepository().GetById(id);

                if (dto == null)
                {
                    _logger.Warning("GetById: Không tìm thấy lịch sử thay đổi, Id={0}", id);
                }
                else
                {
                    _logger.Info("GetById: Lấy lịch sử thay đổi thành công, Id={0}", id);
                }

                return dto;
            }
            catch (Exception ex)
            {
                _logger.Error($"GetById: Lỗi lấy lịch sử thay đổi: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Lấy danh sách ProductVariantIdentifierHistory theo ProductVariantIdentifierId
        /// </summary>
        /// <param name="productVariantIdentifierId">ID định danh sản phẩm</param>
        /// <returns>Danh sách ProductVariantIdentifierHistoryDto</returns>
        public List<ProductVariantIdentifierHistoryDto> GetByProductVariantIdentifierId(Guid productVariantIdentifierId)
        {
            try
            {
                _logger.Debug("GetByProductVariantIdentifierId: Lấy danh sách lịch sử thay đổi, ProductVariantIdentifierId={0}", productVariantIdentifierId);

                var dtos = GetProductVariantIdentifierHistoryRepository().GetByProductVariantIdentifierId(productVariantIdentifierId);

                _logger.Info("GetByProductVariantIdentifierId: Lấy được {0} bản ghi lịch sử", dtos.Count);
                return dtos;
            }
            catch (Exception ex)
            {
                _logger.Error($"GetByProductVariantIdentifierId: Lỗi lấy danh sách lịch sử thay đổi: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Lấy danh sách ProductVariantIdentifierHistory theo ProductVariantId
        /// </summary>
        /// <param name="productVariantId">ID biến thể sản phẩm</param>
        /// <returns>Danh sách ProductVariantIdentifierHistoryDto</returns>
        public List<ProductVariantIdentifierHistoryDto> GetByProductVariantId(Guid productVariantId)
        {
            try
            {
                _logger.Debug("GetByProductVariantId: Lấy danh sách lịch sử thay đổi, ProductVariantId={0}", productVariantId);

                var dtos = GetProductVariantIdentifierHistoryRepository().GetByProductVariantId(productVariantId);

                _logger.Info("GetByProductVariantId: Lấy được {0} bản ghi lịch sử", dtos.Count);
                return dtos;
            }
            catch (Exception ex)
            {
                _logger.Error($"GetByProductVariantId: Lỗi lấy danh sách lịch sử thay đổi: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Lấy danh sách ProductVariantIdentifierHistory theo loại thay đổi (ChangeType)
        /// </summary>
        /// <param name="changeType">Loại thay đổi</param>
        /// <returns>Danh sách ProductVariantIdentifierHistoryDto</returns>
        public List<ProductVariantIdentifierHistoryDto> GetByChangeType(int changeType)
        {
            try
            {
                _logger.Debug("GetByChangeType: Lấy danh sách lịch sử thay đổi, ChangeType={0}", changeType);

                var dtos = GetProductVariantIdentifierHistoryRepository().GetByChangeType(changeType);

                _logger.Info("GetByChangeType: Lấy được {0} bản ghi lịch sử với ChangeType={1}", dtos.Count, changeType);
                return dtos;
            }
            catch (Exception ex)
            {
                _logger.Error($"GetByChangeType: Lỗi lấy danh sách lịch sử thay đổi: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Lấy danh sách ProductVariantIdentifierHistory theo loại thay đổi (ChangeTypeEnum)
        /// </summary>
        /// <param name="changeTypeEnum">Loại thay đổi dưới dạng enum</param>
        /// <returns>Danh sách ProductVariantIdentifierHistoryDto</returns>
        public List<ProductVariantIdentifierHistoryDto> GetByChangeTypeEnum(ProductVariantIdentifierHistoryChangeTypeEnum changeTypeEnum)
        {
            try
            {
                _logger.Debug("GetByChangeTypeEnum: Lấy danh sách lịch sử thay đổi, ChangeTypeEnum={0}", changeTypeEnum);

                var dtos = GetProductVariantIdentifierHistoryRepository().GetByChangeTypeEnum(changeTypeEnum);

                _logger.Info("GetByChangeTypeEnum: Lấy được {0} bản ghi lịch sử với ChangeTypeEnum={1}", dtos.Count, changeTypeEnum);
                return dtos;
            }
            catch (Exception ex)
            {
                _logger.Error($"GetByChangeTypeEnum: Lỗi lấy danh sách lịch sử thay đổi: {ex.Message}", ex);
                throw;
            }
        }

        #endregion

        #region ========== CREATE/UPDATE OPERATIONS ==========

        /// <summary>
        /// Lưu hoặc cập nhật ProductVariantIdentifierHistory
        /// </summary>
        /// <param name="dto">ProductVariantIdentifierHistoryDto cần lưu</param>
        /// <returns>ProductVariantIdentifierHistoryDto đã được lưu</returns>
        public ProductVariantIdentifierHistoryDto SaveOrUpdate(ProductVariantIdentifierHistoryDto dto)
        {
            try
            {
                if (dto == null)
                    throw new ArgumentNullException(nameof(dto));

                _logger.Debug("SaveOrUpdate: Bắt đầu lưu lịch sử thay đổi, Id={0}, ProductVariantIdentifierId={1}",
                    dto.Id, dto.ProductVariantIdentifierId);

                // Business logic validation
                ValidateBeforeSave(dto);

                var result = GetProductVariantIdentifierHistoryRepository().SaveOrUpdate(dto);

                _logger.Info("SaveOrUpdate: Lưu lịch sử thay đổi thành công, Id={0}", result?.Id ?? dto.Id);
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error($"SaveOrUpdate: Lỗi lưu lịch sử thay đổi: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Tạo bản ghi lịch sử thay đổi mới
        /// </summary>
        /// <param name="productVariantIdentifierId">ID định danh sản phẩm</param>
        /// <param name="productVariantId">ID biến thể sản phẩm</param>
        /// <param name="changeTypeEnum">Loại thay đổi</param>
        /// <param name="value">Giá trị thay đổi</param>
        /// <param name="notes">Ghi chú</param>
        /// <param name="changedBy">Người thay đổi</param>
        /// <returns>ProductVariantIdentifierHistoryDto đã được tạo</returns>
        public ProductVariantIdentifierHistoryDto CreateHistory(
            Guid productVariantIdentifierId,
            Guid productVariantId,
            ProductVariantIdentifierHistoryChangeTypeEnum changeTypeEnum,
            string value = null,
            string notes = null,
            Guid? changedBy = null)
        {
            try
            {
                _logger.Debug("CreateHistory: Tạo bản ghi lịch sử thay đổi, ProductVariantIdentifierId={0}, ChangeType={1}",
                    productVariantIdentifierId, changeTypeEnum);

                var dto = new ProductVariantIdentifierHistoryDto
                {
                    Id = Guid.NewGuid(),
                    ProductVariantIdentifierId = productVariantIdentifierId,
                    ProductVariantId = productVariantId,
                    ChangeTypeEnum = changeTypeEnum,
                    ChangeDate = DateTime.Now,
                    Value = value,
                    Notes = notes,
                    ChangedBy = changedBy
                };

                var result = SaveOrUpdate(dto);

                _logger.Info("CreateHistory: Tạo bản ghi lịch sử thay đổi thành công, Id={0}", result.Id);
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error($"CreateHistory: Lỗi tạo bản ghi lịch sử thay đổi: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Validate dữ liệu trước khi lưu
        /// </summary>
        /// <param name="dto">ProductVariantIdentifierHistoryDto cần validate</param>
        private void ValidateBeforeSave(ProductVariantIdentifierHistoryDto dto)
        {
            // Kiểm tra ProductVariantIdentifierId không được rỗng
            if (dto.ProductVariantIdentifierId == Guid.Empty)
            {
                throw new ArgumentException("ProductVariantIdentifierId không được để trống", nameof(dto));
            }

            // Kiểm tra ProductVariantId không được rỗng
            if (dto.ProductVariantId == Guid.Empty)
            {
                throw new ArgumentException("ProductVariantId không được để trống", nameof(dto));
            }

            // Kiểm tra ChangeType phải hợp lệ
            if (!Enum.IsDefined(typeof(ProductVariantIdentifierHistoryChangeTypeEnum), dto.ChangeTypeEnum))
            {
                throw new ArgumentException($"ChangeType không hợp lệ: {dto.ChangeType}", nameof(dto));
            }
        }

        #endregion

        #region ========== DELETE OPERATIONS ==========

        /// <summary>
        /// Xóa ProductVariantIdentifierHistory theo ID
        /// </summary>
        /// <param name="id">ID của ProductVariantIdentifierHistory cần xóa</param>
        /// <returns>True nếu xóa thành công, False nếu không tìm thấy</returns>
        public bool Delete(Guid id)
        {
            try
            {
                _logger.Debug("Delete: Xóa lịch sử thay đổi, Id={0}", id);

                var result = GetProductVariantIdentifierHistoryRepository().Delete(id);

                if (result)
                {
                    _logger.Info("Delete: Xóa lịch sử thay đổi thành công, Id={0}", id);
                }
                else
                {
                    _logger.Warning("Delete: Không tìm thấy lịch sử thay đổi để xóa, Id={0}", id);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.Error($"Delete: Lỗi xóa lịch sử thay đổi: {ex.Message}", ex);
                throw;
            }
        }

        #endregion
    }
}
