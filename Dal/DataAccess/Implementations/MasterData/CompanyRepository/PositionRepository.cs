using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Threading.Tasks;
using Dal.DataAccess.Interfaces.MasterData.CompanyRepository;
using Dal.DataContext;
using Dal.Exceptions;
using Logger;
using Logger.Configuration;
using CustomLogger = Logger.Interfaces.ILogger;

namespace Dal.DataAccess.Implementations.MasterData.CompanyRepository;

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
    /// Khởi tạo một instance mới của class PositionRepository.
    /// </summary>
    /// <param name="connectionString">Chuỗi kết nối cơ sở dữ liệu</param>
    /// <exception cref="ArgumentNullException">Ném ra khi connectionString là null</exception>
    public PositionRepository(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _logger = LoggerFactory.CreateLogger(LogCategory.DAL);
        _logger.Info("PositionRepository được khởi tạo với connection string");
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
        loadOptions.LoadWith<Position>(p => p.Company);
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
            var position = context.Positions.FirstOrDefault(x => x.Id == id);
            
            if (position != null)
            {
                _logger.Debug($"Đã lấy chức vụ theo ID: {id} - {position.PositionName}");
            }
            
            return position;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi lấy thông tin chức vụ: {ex.Message}", ex);
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

            using var context = CreateNewContext();
            var position = context.Positions.FirstOrDefault(x => x.PositionCode == positionCode.Trim());
            
            if (position != null)
            {
                _logger.Debug($"Đã lấy chức vụ theo mã: {positionCode} - {position.PositionName}");
            }
            
            return position;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi lấy thông tin chức vụ theo mã: {ex.Message}", ex);
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

            using var context = CreateNewContext();
            
            // Nếu companyId là Guid.Empty, lấy từ Company duy nhất
            if (companyId == Guid.Empty)
            {
                var company = context.Companies.FirstOrDefault();
                if (company == null)
                    return null;
                companyId = company.Id;
            }

            var position = context.Positions.FirstOrDefault(x => x.PositionCode == positionCode.Trim() && x.CompanyId == companyId);
            
            if (position != null)
            {
                _logger.Debug($"Đã lấy chức vụ theo mã và công ty: {positionCode} - {position.PositionName}");
            }
            
            return position;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi lấy thông tin chức vụ theo mã và công ty: {ex.Message}", ex);
            throw new DataAccessException("Lỗi lấy thông tin chức vụ theo mã và công ty", ex);
        }
    }

    /// <summary>
    /// Lấy tất cả chức vụ (Async).
    /// </summary>
    /// <returns>Danh sách tất cả chức vụ</returns>
    public async Task<List<Position>> GetAllAsync()
    {
        try
        {
            using var context = CreateNewContext();

            var positions = await Task.Run(() => context.Positions
                .OrderBy(p => p.PositionName)
                .ToList());
            
            _logger.Debug($"Đã lấy {positions.Count} chức vụ (async)");
            return positions;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi lấy danh sách chức vụ: {ex.Message}", ex);
            throw new DataAccessException("Lỗi lấy danh sách chức vụ", ex);
        }
    }

    /// <summary>
    /// Lấy tất cả chức vụ (Sync).
    /// </summary>
    /// <returns>Danh sách tất cả chức vụ</returns>
    public List<Position> GetAll()
    {
        try
        {
            using var context = CreateNewContext();

            var positions = context.Positions
                .OrderBy(p => p.PositionName)
                .ToList();
            
            _logger.Debug($"Đã lấy {positions.Count} chức vụ");
            return positions;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi lấy danh sách chức vụ: {ex.Message}", ex);
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
            using var context = CreateNewContext();

            var positions = await Task.Run(() => context.Positions
                .Where(x => x.IsActive)
                .OrderBy(p => p.PositionName)
                .ToList());
            
            _logger.Debug($"Đã lấy {positions.Count} chức vụ đang hoạt động (async)");
            return positions;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi lấy danh sách chức vụ đang hoạt động: {ex.Message}", ex);
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
            using var context = CreateNewContext();

            var positions = context.Positions
                .Where(x => x.IsActive)
                .OrderBy(p => p.PositionName)
                .ToList();
            
            _logger.Debug($"Đã lấy {positions.Count} chức vụ đang hoạt động");
            return positions;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi lấy danh sách chức vụ đang hoạt động: {ex.Message}", ex);
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
            using var context = CreateNewContext();
            
            // Nếu companyId là Guid.Empty, lấy từ Company duy nhất
            if (companyId == Guid.Empty)
            {
                var company = context.Companies.FirstOrDefault();
                if (company == null)
                    return new List<Position>();
                companyId = company.Id;
            }

            var positions = context.Positions
                .Where(x => x.CompanyId == companyId)
                .OrderBy(p => p.PositionName)
                .ToList();
            
            _logger.Debug($"Đã lấy {positions.Count} chức vụ theo công ty: {companyId}");
            return positions;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi lấy danh sách chức vụ theo công ty: {ex.Message}", ex);
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
            if (position == null)
                throw new ArgumentNullException(nameof(position));

            using var context = CreateNewContext();
            context.Positions.InsertOnSubmit(position);
            context.SubmitChanges();
            
            _logger.Info($"Đã thêm mới chức vụ: {position.PositionCode} - {position.PositionName}");
            return position.Id;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi thêm mới chức vụ: {ex.Message}", ex);
            throw new DataAccessException("Lỗi thêm mới chức vụ", ex);
        }
    }

    /// <summary>
    /// Cập nhật chức vụ.
    /// </summary>
    /// <param name="position">Chức vụ cần cập nhật</param>
    public void Update(Position position)
    {
        try
        {
            if (position == null)
                throw new ArgumentNullException(nameof(position));

            using var context = CreateNewContext();
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
            
            _logger.Info($"Đã cập nhật chức vụ: {existingPosition.PositionCode} - {existingPosition.PositionName}");
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi cập nhật chức vụ: {ex.Message}", ex);
            throw new DataAccessException("Lỗi cập nhật chức vụ", ex);
        }
    }

    /// <summary>
    /// Xóa chức vụ.
    /// </summary>
    /// <param name="position">Chức vụ cần xóa</param>
    public void Delete(Position position)
    {
        try
        {
            if (position == null)
                throw new ArgumentNullException(nameof(position));

            using var context = CreateNewContext();
            var existingPosition = context.Positions.FirstOrDefault(x => x.Id == position.Id);
            
            if (existingPosition == null)
            {
                throw new DataAccessException("Không tìm thấy chức vụ để xóa");
            }

            context.Positions.DeleteOnSubmit(existingPosition);
            context.SubmitChanges();
            
            _logger.Info($"Đã xóa chức vụ: {existingPosition.PositionCode} - {existingPosition.PositionName}");
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi xóa chức vụ: {ex.Message}", ex);
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

            using var context = CreateNewContext();
            var result = context.Positions.Any(x => x.PositionCode == positionCode.Trim());
            
            _logger.Debug($"IsPositionCodeExists: Code='{positionCode}', Result={result}");
            return result;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi kiểm tra mã chức vụ: {ex.Message}", ex);
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

            using var context = CreateNewContext();
            var result = context.Positions.Any(x => x.PositionCode == positionCode.Trim() && x.Id != excludeId);
            
            _logger.Debug($"IsPositionCodeExists: Code='{positionCode}', ExcludeId={excludeId}, Result={result}");
            return result;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi kiểm tra mã chức vụ: {ex.Message}", ex);
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

            using var context = CreateNewContext();
            var result = context.Positions.Any(x => x.PositionName == positionName.Trim());
            
            _logger.Debug($"IsPositionNameExists: Name='{positionName}', Result={result}");
            return result;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi kiểm tra tên chức vụ: {ex.Message}", ex);
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

            using var context = CreateNewContext();
            var result = context.Positions.Any(x => x.PositionName == positionName.Trim() && x.Id != excludeId);
            
            _logger.Debug($"IsPositionNameExists: Name='{positionName}', ExcludeId={excludeId}, Result={result}");
            return result;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi kiểm tra tên chức vụ: {ex.Message}", ex);
            throw new DataAccessException("Lỗi kiểm tra tên chức vụ", ex);
        }
    }

    #endregion
}
