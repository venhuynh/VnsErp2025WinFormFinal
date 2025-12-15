using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dal.Connection;
using Dal.DataAccess.Implementations.MasterData.CompanyRepository;
using Dal.DataAccess.Interfaces.MasterData.CompanyRepository;
using Dal.DataContext;
using Logger;
using Logger.Configuration;

namespace Bll.MasterData.CompanyBll
{
    /// <summary>
    /// Business Logic Layer cho quản lý chức vụ.
    /// Cung cấp các phương thức xử lý nghiệp vụ cho chức vụ.
    /// </summary>
    public class PositionBll
{
    #region Fields

    private IPositionRepository _dataAccess;
    private readonly object _lockObject = new object();

    #endregion

    #region Constructors

    /// <summary>
    /// Constructor mặc định
    /// </summary>
    public PositionBll()
    {
        
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Lấy hoặc khởi tạo Repository (lazy initialization)
    /// </summary>
    private IPositionRepository GetDataAccess()
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

                        _dataAccess = new PositionRepository(globalConnectionString);
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


    #region ========== QUẢN LÝ DỮ LIỆU ==========

    /// <summary>
    /// Lấy tất cả chức vụ (Async).
    /// </summary>
    /// <returns>Danh sách tất cả chức vụ</returns>
    public async Task<List<Position>> GetAllAsync()
    {
        try
        {
            return await GetDataAccess().GetAllAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Lỗi lấy danh sách chức vụ: " + ex.Message, ex);
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
            return GetDataAccess().GetAll();
        }
        catch (Exception ex)
        {
            throw new Exception("Lỗi lấy danh sách chức vụ: " + ex.Message, ex);
        }
    }

    /// <summary>
    /// Lấy chức vụ theo ID.
    /// </summary>
    /// <param name="id">ID của chức vụ</param>
    /// <returns>Chức vụ hoặc null nếu không tìm thấy</returns>
    public Position GetById(Guid id)
    {
        try
        {
            return GetDataAccess().GetById(id);
        }
        catch (Exception ex)
        {
            throw new Exception("Lỗi lấy thông tin chức vụ: " + ex.Message, ex);
        }
    }

    /// <summary>
    /// Lấy chức vụ theo mã chức vụ (trong Company duy nhất).
    /// </summary>
    /// <param name="positionCode">Mã chức vụ</param>
    /// <returns>Chức vụ hoặc null nếu không tìm thấy</returns>
    public Position GetByPositionCode(string positionCode)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(positionCode))
                return null;

            // Sử dụng GetByPositionCodeAndCompany với Guid.Empty để lấy từ Company duy nhất
            return GetDataAccess().GetByPositionCodeAndCompany(positionCode.Trim(), Guid.Empty);
        }
        catch (Exception ex)
        {
            throw new Exception("Lỗi lấy thông tin chức vụ theo mã: " + ex.Message, ex);
        }
    }

    /// <summary>
    /// Lấy danh sách chức vụ đang hoạt động.
    /// </summary>
    /// <returns>Danh sách chức vụ đang hoạt động</returns>
    public async Task<List<Position>> GetActivePositionsAsync()
    {
        try
        {
            return await GetDataAccess().GetActivePositionsAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Lỗi lấy danh sách chức vụ đang hoạt động: " + ex.Message, ex);
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
            return GetDataAccess().GetActivePositions();
        }
        catch (Exception ex)
        {
            throw new Exception("Lỗi lấy danh sách chức vụ đang hoạt động: " + ex.Message, ex);
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
            return GetDataAccess().GetByCompanyId(companyId);
        }
        catch (Exception ex)
        {
            throw new Exception("Lỗi lấy danh sách chức vụ theo công ty: " + ex.Message, ex);
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
            // Validate dữ liệu đầu vào
            ValidatePosition(position);

            // Kiểm tra trùng lặp mã chức vụ
            if (IsPositionCodeExists(position.PositionCode))
            {
                throw new Exception($"Mã chức vụ '{position.PositionCode}' đã tồn tại trong hệ thống");
            }

            // Thiết lập thông tin mặc định
            position.Id = Guid.NewGuid();

            return GetDataAccess().Insert(position);
        }
        catch (Exception ex)
        {
            throw new Exception("Lỗi thêm mới chức vụ: " + ex.Message, ex);
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
            // Validate dữ liệu đầu vào
            ValidatePosition(position);

            // Kiểm tra chức vụ có tồn tại không
            var existingPosition = GetById(position.Id);
            if (existingPosition == null)
            {
                throw new Exception("Không tìm thấy chức vụ để cập nhật");
            }

            // Kiểm tra trùng lặp mã chức vụ (trừ bản ghi hiện tại)
            if (IsPositionCodeExists(position.PositionCode, position.Id))
            {
                throw new Exception($"Mã chức vụ '{position.PositionCode}' đã tồn tại trong hệ thống");
            }

            GetDataAccess().Update(position);
        }
        catch (Exception ex)
        {
            throw new Exception("Lỗi cập nhật chức vụ: " + ex.Message, ex);
        }
    }

    /// <summary>
    /// Xóa chức vụ.
    /// </summary>
    /// <param name="id">ID của chức vụ cần xóa</param>
    public void Delete(Guid id)
    {
        // Kiểm tra chức vụ có tồn tại không
        var existingPosition = GetById(id);
        if (existingPosition == null)
        {
            throw new Exception("Không tìm thấy chức vụ để xóa");
        }

        // Kiểm tra ràng buộc dữ liệu (nếu có)
        if (HasDataConstraints(id))
        {
            throw new Exception("Không thể xóa chức vụ do có dữ liệu liên quan");
        }

        GetDataAccess().Delete(existingPosition);
    }

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

            return GetDataAccess().IsPositionCodeExists(positionCode.Trim());
        }
        catch (Exception ex)
        {
            throw new Exception("Lỗi kiểm tra mã chức vụ: " + ex.Message, ex);
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

            return GetDataAccess().IsPositionCodeExists(positionCode.Trim(), excludeId);
        }
        catch (Exception ex)
        {
            throw new Exception("Lỗi kiểm tra mã chức vụ: " + ex.Message, ex);
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

            return GetDataAccess().IsPositionNameExists(positionName.Trim());
        }
        catch (Exception ex)
        {
            throw new Exception("Lỗi kiểm tra tên chức vụ: " + ex.Message, ex);
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

            return GetDataAccess().IsPositionNameExists(positionName.Trim(), excludeId);
        }
        catch (Exception ex)
        {
            throw new Exception("Lỗi kiểm tra tên chức vụ: " + ex.Message, ex);
        }
    }

    #endregion

    #region ========== TIỆN ÍCH ==========

    /// <summary>
    /// Validate dữ liệu chức vụ.
    /// </summary>
    /// <param name="position">Chức vụ cần validate</param>
    private void ValidatePosition(Position position)
    {
        if (position == null)
            throw new Exception("Thông tin chức vụ không được để trống");

        if (string.IsNullOrWhiteSpace(position.PositionCode))
            throw new Exception("Mã chức vụ không được để trống");

        if (string.IsNullOrWhiteSpace(position.PositionName))
            throw new Exception("Tên chức vụ không được để trống");

        if (position.PositionCode.Trim().Length > 50)
            throw new Exception("Mã chức vụ không được vượt quá 50 ký tự");

        if (position.PositionName.Trim().Length > 255)
            throw new Exception("Tên chức vụ không được vượt quá 255 ký tự");

        if (!string.IsNullOrWhiteSpace(position.Description) && position.Description.Trim().Length > 255)
            throw new Exception("Mô tả không được vượt quá 255 ký tự");
    }

    /// <summary>
    /// Kiểm tra chức vụ có ràng buộc dữ liệu không.
    /// </summary>
    /// <param name="id">ID của chức vụ</param>
    /// <returns>True nếu có ràng buộc, False nếu không</returns>
    private bool HasDataConstraints(Guid id)
    {
        try
        {
            // TODO: Kiểm tra các ràng buộc dữ liệu
            // Ví dụ: Kiểm tra có nhân viên nào thuộc chức vụ này không
                
            return false; // Tạm thời return false, cần implement theo yêu cầu cụ thể
        }
        catch (Exception ex)
        {
            throw new Exception("Lỗi kiểm tra ràng buộc dữ liệu: " + ex.Message, ex);
        }
    }

    #endregion
}
}