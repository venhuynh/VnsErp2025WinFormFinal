using Dal.DataAccess.MasterData;
using Dal.DataContext;
using Dal.Logging;
using System;
using System.Collections.Generic;

namespace Bll.MasterData.Customer
{
    /// <summary>
    /// Business Logic Layer cho BusinessPartnerContact
    /// </summary>
    public class BusinessPartnerContactBll
    {
        private readonly BusinessPartnerContactDataAccess _dataAccess;
        private readonly ILogger _logger;

        public BusinessPartnerContactBll()
        {
            _logger = new ConsoleLogger();
            _dataAccess = new BusinessPartnerContactDataAccess();
        }

        public BusinessPartnerContactBll(string connectionString, ILogger logger = null)
        {
            _logger = logger ?? new ConsoleLogger();
            _dataAccess = new BusinessPartnerContactDataAccess(connectionString);
        }

        /// <summary>
        /// Lấy tất cả BusinessPartnerContact
        /// </summary>
        /// <returns>Danh sách BusinessPartnerContact</returns>
        public List<BusinessPartnerContact> GetAll()
        {
            try
            {
                _logger?.LogInfo("Bắt đầu lấy danh sách BusinessPartnerContact");
                var result = _dataAccess.GetAll();
                _logger?.LogInfo($"Hoàn thành lấy danh sách BusinessPartnerContact: {result.Count} records");
                return result;
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Lỗi khi lấy danh sách BusinessPartnerContact: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Lấy BusinessPartnerContact theo ID
        /// </summary>
        /// <param name="id">ID của BusinessPartnerContact</param>
        /// <returns>BusinessPartnerContact hoặc null</returns>
        public BusinessPartnerContact GetById(Guid id)
        {
            try
            {
                _logger?.LogInfo($"Bắt đầu lấy BusinessPartnerContact với ID: {id}");
                var result = _dataAccess.GetById(id);
                _logger?.LogInfo(result != null ? "Tìm thấy BusinessPartnerContact" : "Không tìm thấy BusinessPartnerContact");
                return result;
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Lỗi khi lấy BusinessPartnerContact theo ID: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Lưu hoặc cập nhật BusinessPartnerContact
        /// </summary>
        /// <param name="entity">BusinessPartnerContact entity</param>
        /// <returns>ID của entity đã lưu</returns>
        public Guid SaveOrUpdate(BusinessPartnerContact entity)
        {
            try
            {
                _logger?.LogInfo($"Bắt đầu lưu BusinessPartnerContact: {entity.FullName}");
                if (entity.Id == Guid.Empty)
                {
                    _dataAccess.Add(entity);
                }
                else
                {
                    _dataAccess.Update(entity);
                }
                _logger?.LogInfo($"Hoàn thành lưu BusinessPartnerContact với ID: {entity.Id}");
                return entity.Id;
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Lỗi khi lưu BusinessPartnerContact: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Xóa BusinessPartnerContact theo ID
        /// </summary>
        /// <param name="id">ID của BusinessPartnerContact</param>
        /// <returns>True nếu xóa thành công</returns>
        public bool Delete(Guid id)
        {
            try
            {
                _logger?.LogInfo($"Bắt đầu xóa BusinessPartnerContact với ID: {id}");
                var entity = _dataAccess.GetById(id);
                if (entity != null)
                {
                    _dataAccess.Delete(entity);
                    _logger?.LogInfo("Xóa BusinessPartnerContact thành công");
                    return true;
                }
                _logger?.LogInfo("Không tìm thấy BusinessPartnerContact để xóa");
                return false;
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Lỗi khi xóa BusinessPartnerContact: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Kiểm tra Phone có tồn tại không
        /// </summary>
        /// <param name="phone">Phone cần kiểm tra</param>
        /// <param name="excludeId">ID cần loại trừ (cho trường hợp update)</param>
        /// <returns>True nếu đã tồn tại</returns>
        public bool IsPhoneExists(string phone, Guid? excludeId = null)
        {
            try
            {
                _logger?.LogInfo($"Kiểm tra Phone: {phone}");
                var result = _dataAccess.IsPhoneExists(phone, excludeId);
                _logger?.LogInfo(result ? "Phone đã tồn tại" : "Phone chưa tồn tại");
                return result;
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Lỗi khi kiểm tra Phone: {ex.Message}", ex);
                throw;
            }
        }


    }
}