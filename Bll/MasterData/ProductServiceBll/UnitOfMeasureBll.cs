using System;
using System.Collections.Generic;
using System.Linq;
using Dal.Connection;
using Dal.DataAccess.Implementations.MasterData.ProductServiceRepositories;
using Dal.DataAccess.Interfaces.MasterData.ProductServiceRepositories;
using UnitOfMeasure = Dal.DataContext.UnitOfMeasure;

namespace Bll.MasterData.ProductServiceBll
{
    /// <summary>
    /// Business Logic Layer cho UnitOfMeasure
    /// Cung cấp các chức năng nghiệp vụ cho quản lý đơn vị tính
    /// </summary>
    public class UnitOfMeasureBll
    {
        #region Private Fields

        private IUnitOfMeasureRepository _dataAccess;
        private readonly object _lockObject = new object();

        #endregion

        #region Helper Methods

        /// <summary>
        /// Lấy hoặc khởi tạo Repository (lazy initialization)
        /// </summary>
        private IUnitOfMeasureRepository GetDataAccess()
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

                            _dataAccess = new UnitOfMeasureRepository(globalConnectionString);
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

        #region Public Methods

        /// <summary>
        /// Lấy tất cả đơn vị tính
        /// </summary>
        /// <returns>Danh sách UnitOfMeasure entities</returns>
        public List<UnitOfMeasure> GetAll()
        {
            try
            {
                return GetDataAccess().GetAll();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách đơn vị tính: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy đơn vị tính theo ID
        /// </summary>
        /// <param name="id">ID đơn vị tính</param>
        /// <returns>UnitOfMeasure entity hoặc null</returns>
        public UnitOfMeasure GetById(Guid id)
        {
            try
            {
                return GetDataAccess().GetById(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy đơn vị tính theo ID {id}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy đơn vị tính theo mã
        /// </summary>
        /// <param name="code">Mã đơn vị tính</param>
        /// <returns>UnitOfMeasure entity hoặc null</returns>
        public UnitOfMeasure GetByCode(string code)
        {
            try
            {
                return GetDataAccess().GetByCode(code);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy đơn vị tính theo mã '{code}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy đơn vị tính theo tên
        /// </summary>
        /// <param name="name">Tên đơn vị tính</param>
        /// <returns>UnitOfMeasure entity hoặc null</returns>
        public UnitOfMeasure GetByName(string name)
        {
            try
            {
                return GetDataAccess().GetByName(name);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy đơn vị tính theo tên '{name}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Tìm kiếm đơn vị tính theo từ khóa
        /// </summary>
        /// <param name="keyword">Từ khóa tìm kiếm</param>
        /// <returns>Danh sách UnitOfMeasure entities</returns>
        public List<UnitOfMeasure> Search(string keyword)
        {
            try
            {
                return GetDataAccess().Search(keyword);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tìm kiếm đơn vị tính với từ khóa '{keyword}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy đơn vị tính theo trạng thái hoạt động
        /// </summary>
        /// <param name="isActive">Trạng thái hoạt động</param>
        /// <returns>Danh sách UnitOfMeasure entities</returns>
        public List<UnitOfMeasure> GetByStatus(bool isActive)
        {
            try
            {
                return GetDataAccess().GetByStatus(isActive);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy đơn vị tính theo trạng thái {isActive}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lưu hoặc cập nhật đơn vị tính
        /// </summary>
        /// <param name="entity">UnitOfMeasure entity cần lưu</param>
        public void SaveOrUpdate(UnitOfMeasure entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));

                GetDataAccess().SaveOrUpdate(entity);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lưu đơn vị tính '{entity?.Code}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Xóa đơn vị tính theo ID
        /// </summary>
        /// <param name="id">ID đơn vị tính</param>
        /// <returns>True nếu thành công</returns>
        public bool Delete(Guid id)
        {
            try
            {
                return GetDataAccess().DeleteUnitOfMeasure(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa đơn vị tính {id}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Xóa đơn vị tính theo mã
        /// </summary>
        /// <param name="code">Mã đơn vị tính</param>
        /// <returns>True nếu thành công</returns>
        public bool DeleteByCode(string code)
        {
            try
            {
                var entity = GetDataAccess().GetByCode(code);
                if (entity == null)
                    return false;

                return GetDataAccess().DeleteUnitOfMeasure(entity.Id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa đơn vị tính theo mã '{code}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Kiểm tra mã đơn vị có tồn tại không
        /// </summary>
        /// <param name="code">Mã đơn vị</param>
        /// <param name="excludeId">ID loại trừ (khi cập nhật)</param>
        /// <returns>True nếu tồn tại</returns>
        public bool IsCodeExists(string code, Guid? excludeId = null)
        {
            try
            {
                return GetDataAccess().IsCodeExists(code, excludeId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi kiểm tra mã đơn vị '{code}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Kiểm tra tên đơn vị có tồn tại không
        /// </summary>
        /// <param name="name">Tên đơn vị</param>
        /// <param name="excludeId">ID loại trừ (khi cập nhật)</param>
        /// <returns>True nếu tồn tại</returns>
        public bool IsNameExists(string name, Guid? excludeId = null)
        {
            try
            {
                return GetDataAccess().IsNameExists(name, excludeId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi kiểm tra tên đơn vị '{name}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Kiểm tra đơn vị có đang được sử dụng không
        /// </summary>
        /// <param name="id">ID đơn vị</param>
        /// <returns>True nếu đang được sử dụng</returns>
        public bool HasDependencies(Guid id)
        {
            try
            {
                return GetDataAccess().HasDependencies(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi kiểm tra phụ thuộc của đơn vị {id}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Đếm số lượng đơn vị tính
        /// </summary>
        /// <returns>Số lượng</returns>
        public int GetCount()
        {
            try
            {
                return GetDataAccess().GetCount();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi đếm số lượng đơn vị tính: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Đếm số lượng đơn vị tính theo trạng thái
        /// </summary>
        /// <param name="isActive">Trạng thái hoạt động</param>
        /// <returns>Số lượng</returns>
        public int GetCountByStatus(bool isActive)
        {
            try
            {
                return GetDataAccess().GetCountByStatus(isActive);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi đếm số lượng đơn vị tính theo trạng thái {isActive}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy danh sách mã đơn vị unique
        /// </summary>
        /// <returns>Danh sách mã</returns>
        public List<string> GetUniqueCodes()
        {
            try
            {
                var entities = GetAll();
                return entities.Select(entity => entity.Code)
                              .Where(code => !string.IsNullOrWhiteSpace(code))
                              .Distinct()
                              .OrderBy(code => code)
                              .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách mã đơn vị: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy danh sách tên đơn vị unique
        /// </summary>
        /// <returns>Danh sách tên</returns>
        public List<string> GetUniqueNames()
        {
            try
            {
                var entities = GetAll();
                return entities.Select(entity => entity.Name)
                              .Where(name => !string.IsNullOrWhiteSpace(name))
                              .Distinct()
                              .OrderBy(name => name)
                              .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách tên đơn vị: {ex.Message}", ex);
            }
        }

        #endregion
    }

}
