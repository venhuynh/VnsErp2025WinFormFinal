using Common.Appconfig;
using Dal.DataAccess.Implementations.MasterData.CompanyRepository;
using Dal.DataAccess.Interfaces.MasterData.CompanyRepository;
using Dal.DataContext;
using Logger;
using Logger.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dal.DataAccess.Interfaces.MasterData.PartnerRepository;

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
        public CompanyBll()
        {
            // Khởi tạo logger trước để đảm bảo không null trong khối catch
            _logger = LoggerFactory.CreateLogger(LogCategory.BLL);
            // Repository sẽ được khởi tạo lazy khi cần sử dụng
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Lấy hoặc khởi tạo Repository (lazy initialization)
        /// </summary>
        private ICompanyRepository GetDataAccess()
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

                            _dataAccess = new CompanyRepository(globalConnectionString);
                        }
                        catch (Exception ex)
                        {
                            _logger.Error("Lỗi khi khởi tạo CompanyRepository: {0}", ex, ex.Message);
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
        public Task<List<BusinessPartner>> GetAllAsync()
        {
            return _businessPartnerDataAccess.GetAllAsync();
        }

        /// <summary>
        /// Lấy toàn bộ đối tác (entities) - Sync.
        /// </summary>
        public List<BusinessPartner> GetAll()
        {
            return _businessPartnerDataAccess.GetAll();
        }

        /// <summary>
        /// Tồn tại mã?
        /// </summary>
        public bool IsCodeExists(string code)
        {
            return _businessPartnerDataAccess.IsPartnerCodeExists(code);
        }

        /// <summary>
        /// Xóa đối tác theo Id.
        /// </summary>
        public void Delete(Guid id)
        {
            _businessPartnerDataAccess.DeletePartner(id);
        }

        /// <summary>
        /// Lấy đối tác theo Id (entity).
        /// </summary>
        public BusinessPartner GetById(Guid id)
        {
            return _businessPartnerDataAccess.GetById(id);
        }

        /// <summary>
        /// Lấy đối tác theo Id (Async, entity).
        /// </summary>
        public Task<BusinessPartner> GetByIdAsync(Guid id)
        {
            return _businessPartnerDataAccess.GetByIdAsync(id);
        }

        /// <summary>
        /// Lưu/cập nhật đầy đủ thông tin đối tác.
        /// </summary>
        public void SaveOrUpdate(BusinessPartner entity)
        {
            _businessPartnerDataAccess.SaveOrUpdate(entity);
        }
    }
}
