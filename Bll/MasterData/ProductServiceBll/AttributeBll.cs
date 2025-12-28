using Dal.Connection;
using Dal.DataAccess.Implementations.MasterData.ProductServiceRepositories;
using Dal.DataAccess.Interfaces.MasterData.ProductServiceRepositories;
using DTO.MasterData.ProductService;
using System;
using System.Collections.Generic;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;

namespace Bll.MasterData.ProductServiceBll
{
    /// <summary>
    /// Business Logic Layer cho Attribute
    /// </summary>
    public class AttributeBll
    {
        #region Fields

        private IAttributeRepository _dataAccess;
        private readonly object _lockObject = new object();
        private readonly ILogger _logger;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor mặc định
        /// </summary>
        public AttributeBll()
        {
            _logger = LoggerFactory.CreateLogger(LogCategory.BLL);
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Lấy hoặc khởi tạo Repository (lazy initialization)
        /// </summary>
        private IAttributeRepository GetDataAccess()
        {
            if (_dataAccess == null)
            {
                lock (_lockObject)
                {
                    if (_dataAccess == null)
                    {
                        try
                        {
                            var globalConnectionString = ApplicationStartupManager.Instance.GetGlobalConnectionString();
                            if (string.IsNullOrEmpty(globalConnectionString))
                            {
                                throw new InvalidOperationException(
                                    "Không có global connection string. Ứng dụng chưa được khởi tạo hoặc chưa sẵn sàng.");
                            }

                            _dataAccess = new AttributeRepository(globalConnectionString);
                        }
                        catch (Exception ex)
                        {
                            throw new InvalidOperationException(
                                "Không thể khởi tạo kết nối database. Vui lòng kiểm tra cấu hình database.", ex);
                        }
                    }
                }
            }

            return _dataAccess;
        }

        #endregion

        #region ========== CREATE OPERATIONS ==========

        /// <summary>
        /// Lưu hoặc cập nhật Attribute
        /// </summary>
        /// <param name="dto">AttributeDto</param>
        public void SaveOrUpdate(AttributeDto dto)
        {
            try
            {
                if (dto == null)
                {
                    throw new ArgumentNullException(nameof(dto));
                }

                GetDataAccess().SaveOrUpdate(dto);
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi khi lưu/cập nhật Attribute: {ex.Message}", ex);
                throw new Exception($"Lỗi khi lưu/cập nhật Attribute: {ex.Message}", ex);
            }
        }

        #endregion

        #region ========== READ OPERATIONS ==========

        /// <summary>
        /// Lấy tất cả Attribute
        /// </summary>
        /// <returns>Danh sách AttributeDto</returns>
        public List<AttributeDto> GetAll()
        {
            try
            {
                return GetDataAccess().GetAll();
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi khi lấy tất cả Attribute: {ex.Message}", ex);
                throw new Exception($"Lỗi khi lấy tất cả Attribute: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy Attribute theo Id
        /// </summary>
        /// <param name="id">Id của Attribute</param>
        /// <returns>AttributeDto hoặc null</returns>
        public AttributeDto GetById(Guid id)
        {
            try
            {
                return GetDataAccess().GetById(id);
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi khi lấy Attribute theo Id: {ex.Message}", ex);
                throw new Exception($"Lỗi khi lấy Attribute theo Id: {ex.Message}", ex);
            }
        }

        #endregion

        #region ========== UPDATE OPERATIONS ==========
        // Update operations are handled by SaveOrUpdate method
        #endregion

        #region ========== DELETE OPERATIONS ==========

        /// <summary>
        /// Xóa Attribute theo Id
        /// </summary>
        /// <param name="id">Id của Attribute cần xóa</param>
        public void Delete(Guid id)
        {
            try
            {
                GetDataAccess().Delete(id);
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi khi xóa Attribute: {ex.Message}", ex);
                throw new Exception($"Lỗi khi xóa Attribute: {ex.Message}", ex);
            }
        }

        #endregion

        #region ========== VALIDATION & EXISTS CHECKS ==========

        /// <summary>
        /// Kiểm tra tên thuộc tính đã tồn tại chưa (loại trừ Id khi cập nhật)
        /// </summary>
        /// <param name="name">Tên cần kiểm tra</param>
        /// <param name="excludeId">Id cần loại trừ khỏi kiểm tra (dùng khi update)</param>
        /// <returns>True nếu tên đã tồn tại</returns>
        public bool IsNameExists(string name, Guid? excludeId = null)
        {
            try
            {
                return GetDataAccess().IsNameExists(name, excludeId);
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi khi kiểm tra tên Attribute: {ex.Message}", ex);
                throw new Exception($"Lỗi khi kiểm tra tên Attribute: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Kiểm tra thuộc tính có phụ thuộc hay không (AttributeValue/VariantAttribute)
        /// </summary>
        /// <param name="id">Id của Attribute</param>
        /// <returns>True nếu có phụ thuộc</returns>
        public bool HasDependencies(Guid id)
        {
            try
            {
                return GetDataAccess().HasDependencies(id);
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi khi kiểm tra phụ thuộc của Attribute: {ex.Message}", ex);
                throw new Exception($"Lỗi khi kiểm tra phụ thuộc của Attribute: {ex.Message}", ex);
            }
        }

        #endregion
    }
}
