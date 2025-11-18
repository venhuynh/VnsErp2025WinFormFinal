using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dal.DataContext;

namespace Dal.DataAccess.Interfaces.MasterData.CompanyRepository
{
    /// <summary>
    /// Data Access Layer cho quản lý chức vụ.
    /// Cung cấp các phương thức truy cập dữ liệu cho chức vụ.
    /// </summary>
    public interface IPositionRepository
    {
        #region ========== QUẢN LÝ DỮ LIỆU ==========

        /// <summary>
        /// Lấy chức vụ theo ID.
        /// </summary>
        /// <param name="id">ID của chức vụ</param>
        /// <returns>Chức vụ hoặc null nếu không tìm thấy</returns>
        Position GetById(Guid id);

        /// <summary>
        /// Lấy chức vụ theo mã chức vụ.
        /// </summary>
        /// <param name="positionCode">Mã chức vụ</param>
        /// <returns>Chức vụ hoặc null nếu không tìm thấy</returns>
        Position GetByPositionCode(string positionCode);

        /// <summary>
        /// Lấy chức vụ theo mã chức vụ và CompanyId.
        /// </summary>
        /// <param name="positionCode">Mã chức vụ</param>
        /// <param name="companyId">ID công ty (nếu Guid.Empty thì lấy từ Company duy nhất)</param>
        /// <returns>Chức vụ hoặc null nếu không tìm thấy</returns>
        Position GetByPositionCodeAndCompany(string positionCode, Guid companyId);

        /// <summary>
        /// Lấy tất cả chức vụ (Async).
        /// </summary>
        /// <returns>Danh sách tất cả chức vụ</returns>
        Task<List<Position>> GetAllAsync();

        /// <summary>
        /// Lấy tất cả chức vụ (Sync).
        /// </summary>
        /// <returns>Danh sách tất cả chức vụ</returns>
        List<Position> GetAll();

        /// <summary>
        /// Lấy danh sách chức vụ đang hoạt động (Async).
        /// </summary>
        /// <returns>Danh sách chức vụ đang hoạt động</returns>
        Task<List<Position>> GetActivePositionsAsync();

        /// <summary>
        /// Lấy danh sách chức vụ đang hoạt động (Sync).
        /// </summary>
        /// <returns>Danh sách chức vụ đang hoạt động</returns>
        List<Position> GetActivePositions();

        /// <summary>
        /// Lấy tất cả chức vụ của một công ty.
        /// </summary>
        /// <param name="companyId">ID công ty (nếu Guid.Empty thì lấy từ Company duy nhất)</param>
        /// <returns>Danh sách chức vụ của công ty</returns>
        List<Position> GetByCompanyId(Guid companyId);

        #endregion

        #region ========== XỬ LÝ DỮ LIỆU ==========

        /// <summary>
        /// Thêm mới chức vụ.
        /// </summary>
        /// <param name="position">Chức vụ cần thêm</param>
        /// <returns>ID của chức vụ vừa thêm</returns>
        Guid Insert(Position position);

        /// <summary>
        /// Cập nhật chức vụ.
        /// </summary>
        /// <param name="position">Chức vụ cần cập nhật</param>
        void Update(Position position);

        /// <summary>
        /// Xóa chức vụ.
        /// </summary>
        /// <param name="position">Chức vụ cần xóa</param>
        void Delete(Position position);

        #endregion

        #region ========== KIỂM TRA DỮ LIỆU ==========

        /// <summary>
        /// Kiểm tra mã chức vụ có tồn tại không.
        /// </summary>
        /// <param name="positionCode">Mã chức vụ cần kiểm tra</param>
        /// <returns>True nếu tồn tại, False nếu không</returns>
        bool IsPositionCodeExists(string positionCode);

        /// <summary>
        /// Kiểm tra mã chức vụ có tồn tại không (trừ bản ghi hiện tại).
        /// </summary>
        /// <param name="positionCode">Mã chức vụ cần kiểm tra</param>
        /// <param name="excludeId">ID của bản ghi cần loại trừ</param>
        /// <returns>True nếu tồn tại, False nếu không</returns>
        bool IsPositionCodeExists(string positionCode, Guid excludeId);

        /// <summary>
        /// Kiểm tra tên chức vụ có tồn tại không.
        /// </summary>
        /// <param name="positionName">Tên chức vụ cần kiểm tra</param>
        /// <returns>True nếu tồn tại, False nếu không</returns>
        bool IsPositionNameExists(string positionName);

        /// <summary>
        /// Kiểm tra tên chức vụ có tồn tại không (trừ bản ghi hiện tại).
        /// </summary>
        /// <param name="positionName">Tên chức vụ cần kiểm tra</param>
        /// <param name="excludeId">ID của bản ghi cần loại trừ</param>
        /// <returns>True nếu tồn tại, False nếu không</returns>
        bool IsPositionNameExists(string positionName, Guid excludeId);

        #endregion
    }
}