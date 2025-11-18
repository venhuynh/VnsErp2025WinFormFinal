using Dal.BaseDataAccess;
using Dal.DataAccess.Interfaces.MasterData.Company;
using Dal.DataContext;
using Dal.Exceptions;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Threading.Tasks;
using CustomLogger = Logger.Interfaces.ILogger;

namespace Dal.DataAccess.Implementations.MasterData.Company
{
    /// <summary>
    /// Data Access Layer cho quản lý chức vụ.
    /// Cung cấp các phương thức truy cập dữ liệu cho chức vụ.
    /// </summary>
    public class PositionRepository : IPositionRepository
    {
        #region Private Fields

        /// <summary>
        /// Chuỗi kết nối cơ sở dữ liệu để tạo DataContext mới cho mỗi operation.
        /// </summary>
        private readonly string _connectionString;

        /// <summary>
        /// Instance logger để theo dõi các thao tác và lỗi
        /// </summary>
        private readonly CustomLogger _logger;

        #endregion

        #region Constructor

        /// <summary>
        /// Khởi tạo một instance mới của class UserInfoRepository.
        /// </summary>
        /// <param name="connectionString">Chuỗi kết nối cơ sở dữ liệu</param>
        /// <exception cref="ArgumentNullException">Ném ra khi connectionString là null</exception>
        public PositionRepository(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            _logger = LoggerFactory.CreateLogger(LogCategory.DAL);
            _logger.Info("UserInfoRepository được khởi tạo với connection string");
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Tạo DataContext mới cho mỗi operation để tránh cache issue
        /// </summary>
        /// <returns>DataContext mới</returns>
        private VnsErp2025DataContext CreateNewContext()
        {
            var context = new VnsErp2025DataContext(_connectionString);

            // Configure eager loading cho navigation properties
            var loadOptions = new DataLoadOptions();
            loadOptions.LoadWith<Position>(u => u.Company);
            context.LoadOptions = loadOptions;

            return context;
        }

        #endregion

        #region ========== QUẢN LÝ DỮ LIỆU ==========

        /// <summary>
        /// Lấy chức vụ theo ID.
        /// </summary>
        /// <param name="id">ID của chức vụ</param>
        /// <returns>Chức vụ hoặc null nếu không tìm thấy</returns>
        public Position GetById(Guid id)
        {
            try
            {
                using var context = CreateNewContext();

                // Sử dụng DataLoadOptions để include relationships (tránh circular reference)
                var loadOptions = new DataLoadOptions();
                
                loadOptions.LoadWith<Position>(d => d.Company);

                context.LoadOptions = loadOptions;

                return context.Positions.FirstOrDefault(x => x.Id == id);
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Lỗi lấy thông tin chức vụ", ex);
            }
        }

        /// <summary>
        /// Lấy chức vụ theo mã chức vụ.
        /// </summary>
        /// <param name="positionCode">Mã chức vụ</param>
        /// <returns>Chức vụ hoặc null nếu không tìm thấy</returns>
        public Position GetByPositionCode(string positionCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(positionCode))
                    return null;

                using var context = CreateContext();

                // Sử dụng DataLoadOptions để include relationships (tránh circular reference)
                var loadOptions = new DataLoadOptions();
                
                loadOptions.LoadWith<Position>(d => d.Company);

                context.LoadOptions = loadOptions;

                return context.Positions.FirstOrDefault(x => x.PositionCode == positionCode.Trim());
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Lỗi lấy thông tin chức vụ theo mã", ex);
            }
        }

        /// <summary>
        /// Lấy chức vụ theo mã chức vụ và CompanyId.
        /// </summary>
        /// <param name="positionCode">Mã chức vụ</param>
        /// <param name="companyId">ID công ty (nếu Guid.Empty thì lấy từ Company duy nhất)</param>
        /// <returns>Chức vụ hoặc null nếu không tìm thấy</returns>
        public Position GetByPositionCodeAndCompany(string positionCode, Guid companyId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(positionCode))
                    return null;

                using var context = CreateContext();

                // Sử dụng DataLoadOptions để include relationships (tránh circular reference)
                var loadOptions = new DataLoadOptions();
                
                loadOptions.LoadWith<Position>(d => d.Company);

                context.LoadOptions = loadOptions;
                
                // Nếu companyId là Guid.Empty, lấy từ Company duy nhất
                if (companyId == Guid.Empty)
                {
                    var company = context.Companies.FirstOrDefault();
                    if (company == null)
                        return null;
                    companyId = company.Id;
                }

                return context.Positions.FirstOrDefault(x => x.PositionCode == positionCode.Trim() && x.CompanyId == companyId);
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Lỗi lấy thông tin chức vụ theo mã và công ty", ex);
            }
        }

        /// <summary>
        /// Lấy tất cả chức vụ (Async).
        /// </summary>
        /// <returns>Danh sách tất cả chức vụ</returns>
        public override async Task<List<Position>> GetAllAsync()
        {
            try
            {
                using var context = CreateContext();

                // Sử dụng DataLoadOptions để include relationships (tránh circular reference)
                var loadOptions = new DataLoadOptions();
                
                loadOptions.LoadWith<Position>(d => d.Company);

                context.LoadOptions = loadOptions;

                return await Task.Run(() => context.Positions
                    .OrderBy(d => d.PositionName)
                    .ToList());
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Lỗi lấy danh sách chức vụ", ex);
            }
        }

        /// <summary>
        /// Lấy tất cả chức vụ (Sync).
        /// </summary>
        /// <returns>Danh sách tất cả chức vụ</returns>
        public override List<Position> GetAll()
        {
            try
            {
                using var context = CreateContext();

                // Sử dụng DataLoadOptions để include relationships (tránh circular reference)
                var loadOptions = new DataLoadOptions();
                
                loadOptions.LoadWith<Position>(d => d.Company);


                context.LoadOptions = loadOptions;

                return context.Positions.
                    OrderBy(d=>d.PositionName)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Lỗi lấy danh sách chức vụ", ex);
            }
        }

        /// <summary>
        /// Lấy danh sách chức vụ đang hoạt động (Async).
        /// </summary>
        /// <returns>Danh sách chức vụ đang hoạt động</returns>
        public async Task<List<Position>> GetActivePositionsAsync()
        {
            try
            {
                using var context = CreateContext();

                // Sử dụng DataLoadOptions để include relationships (tránh circular reference)
                var loadOptions = new DataLoadOptions();
                
                loadOptions.LoadWith<Position>(d => d.Company);

                context.LoadOptions = loadOptions;

                return await Task.Run(() => context.Positions
                    .Where(x => x.IsActive)
                    .OrderBy(d => d.PositionName)
                    .ToList());
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Lỗi lấy danh sách chức vụ đang hoạt động", ex);
            }
        }

        /// <summary>
        /// Lấy danh sách chức vụ đang hoạt động (Sync).
        /// </summary>
        /// <returns>Danh sách chức vụ đang hoạt động</returns>
        public List<Position> GetActivePositions()
        {
            try
            {
                using var context = CreateContext();

                // Sử dụng DataLoadOptions để include relationships (tránh circular reference)
                var loadOptions = new DataLoadOptions();
                
                loadOptions.LoadWith<Position>(d => d.Company);

                context.LoadOptions = loadOptions;

                return context.Positions
                    .Where(x => x.IsActive)
                    .OrderBy(d => d.PositionName)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Lỗi lấy danh sách chức vụ đang hoạt động", ex);
            }
        }

        /// <summary>
        /// Lấy tất cả chức vụ của một công ty.
        /// </summary>
        /// <param name="companyId">ID công ty (nếu Guid.Empty thì lấy từ Company duy nhất)</param>
        /// <returns>Danh sách chức vụ của công ty</returns>
        public List<Position> GetByCompanyId(Guid companyId)
        {
            try
            {
                using var context = CreateContext();

                // Sử dụng DataLoadOptions để include relationships (tránh circular reference)
                var loadOptions = new DataLoadOptions();
                
                loadOptions.LoadWith<Position>(d => d.Company);

                context.LoadOptions = loadOptions;
                
                // Nếu companyId là Guid.Empty, lấy từ Company duy nhất
                if (companyId == Guid.Empty)
                {
                    var company = context.Companies.FirstOrDefault();
                    if (company == null)
                        return new List<Position>();
                    companyId = company.Id;
                }

                return context.Positions
                    .Where(x => x.CompanyId == companyId)
                    .OrderBy(d => d.PositionName)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Lỗi lấy danh sách chức vụ theo công ty", ex);
            }
        }

        #endregion

        #region ========== XỬ LÝ DỮ LIỆU ==========

        /// <summary>
        /// Thêm mới chức vụ.
        /// </summary>
        /// <param name="position">Chức vụ cần thêm</param>
        /// <returns>ID của chức vụ vừa thêm</returns>
        public Guid Insert(Position position)
        {
            try
            {
                using var context = CreateContext();
                context.Positions.InsertOnSubmit(position);
                context.SubmitChanges();
                return position.Id;
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Lỗi thêm mới chức vụ", ex);
            }
        }

        /// <summary>
        /// Cập nhật chức vụ.
        /// </summary>
        /// <param name="position">Chức vụ cần cập nhật</param>
        public override void Update(Position position)
        {
            try
            {
                using var context = CreateContext();
                var existingPosition = context.Positions.FirstOrDefault(x => x.Id == position.Id);
                if (existingPosition == null)
                {
                    throw new DataAccessException("Không tìm thấy chức vụ để cập nhật");
                }

                // Cập nhật các thuộc tính
                existingPosition.CompanyId = position.CompanyId;
                existingPosition.PositionCode = position.PositionCode;
                existingPosition.PositionName = position.PositionName;
                existingPosition.Description = position.Description;
                existingPosition.IsManagerLevel = position.IsManagerLevel;
                existingPosition.IsActive = position.IsActive;

                context.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Lỗi cập nhật chức vụ", ex);
            }
        }

        /// <summary>
        /// Xóa chức vụ.
        /// </summary>
        /// <param name="position">Chức vụ cần xóa</param>
        public override void Delete(Position position)
        {
            try
            {
                using var context = CreateContext();
                var existingPosition = context.Positions.FirstOrDefault(x => x.Id == position.Id);
                if (existingPosition == null)
                {
                    throw new DataAccessException("Không tìm thấy chức vụ để xóa");
                }

                context.Positions.DeleteOnSubmit(existingPosition);
                context.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Lỗi xóa chức vụ", ex);
            }
        }

        #endregion

        #region ========== KIỂM TRA DỮ LIỆU ==========

        /// <summary>
        /// Kiểm tra mã chức vụ có tồn tại không.
        /// </summary>
        /// <param name="positionCode">Mã chức vụ cần kiểm tra</param>
        /// <returns>True nếu tồn tại, False nếu không</returns>
        public bool IsPositionCodeExists(string positionCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(positionCode))
                    return false;

                using var context = CreateContext();
                return context.Positions.Any(x => x.PositionCode == positionCode.Trim());
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Lỗi kiểm tra mã chức vụ", ex);
            }
        }

        /// <summary>
        /// Kiểm tra mã chức vụ có tồn tại không (trừ bản ghi hiện tại).
        /// </summary>
        /// <param name="positionCode">Mã chức vụ cần kiểm tra</param>
        /// <param name="excludeId">ID của bản ghi cần loại trừ</param>
        /// <returns>True nếu tồn tại, False nếu không</returns>
        public bool IsPositionCodeExists(string positionCode, Guid excludeId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(positionCode))
                    return false;

                using var context = CreateContext();
                return context.Positions.Any(x => x.PositionCode == positionCode.Trim() && x.Id != excludeId);
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Lỗi kiểm tra mã chức vụ", ex);
            }
        }

        /// <summary>
        /// Kiểm tra tên chức vụ có tồn tại không.
        /// </summary>
        /// <param name="positionName">Tên chức vụ cần kiểm tra</param>
        /// <returns>True nếu tồn tại, False nếu không</returns>
        public bool IsPositionNameExists(string positionName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(positionName))
                    return false;

                using var context = CreateContext();
                return context.Positions.Any(x => x.PositionName == positionName.Trim());
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Lỗi kiểm tra tên chức vụ", ex);
            }
        }

        /// <summary>
        /// Kiểm tra tên chức vụ có tồn tại không (trừ bản ghi hiện tại).
        /// </summary>
        /// <param name="positionName">Tên chức vụ cần kiểm tra</param>
        /// <param name="excludeId">ID của bản ghi cần loại trừ</param>
        /// <returns>True nếu tồn tại, False nếu không</returns>
        public bool IsPositionNameExists(string positionName, Guid excludeId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(positionName))
                    return false;

                using var context = CreateContext();
                return context.Positions.Any(x => x.PositionName == positionName.Trim() && x.Id != excludeId);
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Lỗi kiểm tra tên chức vụ", ex);
            }
        }

        #endregion
    }
}
