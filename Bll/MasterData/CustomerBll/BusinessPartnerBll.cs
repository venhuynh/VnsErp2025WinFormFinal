using Dal.Connection;
using Dal.DataAccess.Implementations.MasterData.PartnerRepository;
using Dal.DataAccess.Interfaces.MasterData.PartnerRepository;
using Dal.DataContext;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bll.MasterData.CustomerBll
{
    /// <summary>
    /// Business logic layer cho BusinessPartner. Trả về entity để UI tự chuyển đổi sang DTO.
    /// Tránh phụ thuộc ngược sang project MasterData (DTO) để không tạo vòng tham chiếu.
    /// </summary>
    public class BusinessPartnerBll
    {
        #region Fields

        private IBusinessPartnerRepository _dataAccess;
        private readonly object _lockObject = new object();

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor mặc định
        /// </summary>
        public BusinessPartnerBll()
        {
            
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Lấy hoặc khởi tạo Repository (lazy initialization)
        /// </summary>
        private IBusinessPartnerRepository GetDataAccess()
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

                            _dataAccess = new BusinessPartnerRepository(globalConnectionString);
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


        /// <summary>
        /// Lấy toàn bộ đối tác (entities) - Async.
        /// </summary>
        public async Task<List<BusinessPartner>> GetAllAsync()
        {
            // TODO: IBusinessPartnerRepository không có GetAllAsync(), sử dụng GetActivePartners() tạm thời
            return await Task.Run(() => GetDataAccess().GetActivePartners());
        }

        /// <summary>
        /// Lấy toàn bộ đối tác (entities) - Sync.
        /// </summary>
        public List<BusinessPartner> GetAll()
        {
            // TODO: IBusinessPartnerRepository không có GetAll(), sử dụng GetActivePartners() tạm thời
            return GetDataAccess().GetActivePartners();
        }

        /// <summary>
        /// Tồn tại mã?
        /// </summary>
        public bool IsCodeExists(string code)
        {
            return GetDataAccess().IsPartnerCodeExists(code);
        }

        /// <summary>
        /// Xóa đối tác theo Id (Soft Delete).
        /// </summary>
        /// <param name="id">ID đối tác</param>
        /// <param name="deletedBy">ID người xóa (optional)</param>
        public void Delete(Guid id, Guid? deletedBy = null)
        {
            GetDataAccess().DeletePartner(id, deletedBy);
        }

        /// <summary>
        /// Lấy đối tác theo Id (entity).
        /// </summary>
        public BusinessPartner GetById(Guid id)
        {
            return GetDataAccess().GetById(id);
        }

        /// <summary>
        /// Lấy đối tác theo Id (Async, entity).
        /// </summary>
        public Task<BusinessPartner> GetByIdAsync(Guid id)
        {
            return GetDataAccess().GetByIdAsync(id);
        }

        /// <summary>
        /// Lưu/cập nhật đầy đủ thông tin đối tác.
        /// </summary>
        /// <param name="entity">Entity đối tác</param>
        /// <param name="userId">ID người dùng thực hiện (optional, dùng cho audit fields)</param>
        public void SaveOrUpdate(BusinessPartner entity, Guid? userId = null)
        {
            GetDataAccess().SaveOrUpdate(entity, userId);
        }
    }
}
