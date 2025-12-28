using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dal.Connection;
using Dal.DataAccess.Implementations.MasterData.CompanyRepository;
using Dal.DataAccess.Interfaces.MasterData.CompanyRepository;
using DTO.MasterData.Company;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;

namespace Bll.MasterData.CompanyBll
{
    /// <summary>
    /// Business Logic Layer cho quản lý nhân viên.
    /// Cung cấp các phương thức xử lý nghiệp vụ cho nhân viên.
    /// </summary>
    public class EmployeeBll
    {
        #region Fields

        private IEmployeeRepository _dataAccess;
        private readonly ILogger _logger;
        private readonly object _lockObject = new object();

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor mặc định
        /// </summary>
        public EmployeeBll()
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
        private IEmployeeRepository GetDataAccess()
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

                            _dataAccess = new EmployeeRepository(globalConnectionString);
                        }
                        catch (Exception ex)
                        {
                            _logger.Error("Lỗi khi khởi tạo EmployeeRepository: {0}", ex, ex.Message);
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
        /// Lấy tất cả nhân viên (Async).
        /// </summary>
        /// <returns>Danh sách tất cả nhân viên</returns>
        public async Task<List<EmployeeDto>> GetAllAsync()
        {
            try
            {
                return await GetDataAccess().GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi lấy danh sách nhân viên: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Lấy tất cả nhân viên (Sync).
        /// </summary>
        /// <returns>Danh sách tất cả nhân viên</returns>
        public List<EmployeeDto> GetAll()
        {
            try
            {
                return GetDataAccess().GetAll();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi lấy danh sách nhân viên: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Lấy nhân viên theo ID.
        /// </summary>
        /// <param name="id">ID của nhân viên</param>
        /// <returns>EmployeeDto hoặc null nếu không tìm thấy</returns>
        public EmployeeDto GetById(Guid id)
        {
            try
            {
                return GetDataAccess().GetById(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi lấy thông tin nhân viên: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Lấy nhân viên theo mã nhân viên.
        /// </summary>
        /// <param name="employeeCode">Mã nhân viên</param>
        /// <returns>EmployeeDto hoặc null nếu không tìm thấy</returns>
        public EmployeeDto GetByEmployeeCode(string employeeCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(employeeCode))
                    return null;

                return GetDataAccess().GetByEmployeeCode(employeeCode.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi lấy thông tin nhân viên theo mã: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Lấy danh sách nhân viên đang hoạt động.
        /// </summary>
        /// <returns>Danh sách nhân viên đang hoạt động</returns>
        public async Task<List<EmployeeDto>> GetActiveEmployeesAsync()
        {
            try
            {
                return await GetDataAccess().GetActiveEmployeesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi lấy danh sách nhân viên đang hoạt động: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Lấy danh sách nhân viên đang hoạt động (Sync).
        /// </summary>
        /// <returns>Danh sách nhân viên đang hoạt động</returns>
        public List<EmployeeDto> GetActiveEmployees()
        {
            try
            {
                return GetDataAccess().GetActiveEmployees();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi lấy danh sách nhân viên đang hoạt động: " + ex.Message, ex);
            }
        }

        #endregion

        #region ========== XỬ LÝ DỮ LIỆU ==========

        /// <summary>
        /// Thêm mới nhân viên.
        /// </summary>
        /// <param name="employee">EmployeeDto cần thêm</param>
        /// <returns>ID của nhân viên vừa thêm</returns>
        public Guid Insert(EmployeeDto employee)
        {
            try
            {
                // Validate dữ liệu đầu vào
                ValidateEmployee(employee);

                // Kiểm tra trùng lặp mã nhân viên
                if (IsEmployeeCodeExists(employee.EmployeeCode))
                {
                    throw new Exception($"Mã nhân viên '{employee.EmployeeCode}' đã tồn tại trong hệ thống");
                }

                // Thiết lập thông tin mặc định
                if (employee.Id == Guid.Empty)
                {
                    employee.Id = Guid.NewGuid();
                }

                if (employee.CreatedDate == default(DateTime))
                {
                    employee.CreatedDate = DateTime.Now;
                }

                return GetDataAccess().Insert(employee);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi thêm mới nhân viên: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Cập nhật nhân viên.
        /// </summary>
        /// <param name="employee">EmployeeDto cần cập nhật</param>
        public void Update(EmployeeDto employee)
        {
            try
            {
                // Validate dữ liệu đầu vào
                ValidateEmployee(employee);

                // Kiểm tra nhân viên có tồn tại không
                var existingEmployee = GetById(employee.Id);
                if (existingEmployee == null)
                {
                    throw new Exception("Không tìm thấy nhân viên để cập nhật");
                }

                // Kiểm tra trùng lặp mã nhân viên (trừ bản ghi hiện tại)
                if (IsEmployeeCodeExists(employee.EmployeeCode, employee.Id))
                {
                    throw new Exception($"Mã nhân viên '{employee.EmployeeCode}' đã tồn tại trong hệ thống");
                }

                // Cập nhật ModifiedDate
                employee.ModifiedDate = DateTime.Now;

                GetDataAccess().Update(employee);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi cập nhật nhân viên: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Lưu hoặc cập nhật nhân viên.
        /// </summary>
        /// <param name="employee">EmployeeDto cần lưu</param>
        /// <returns>ID của nhân viên đã lưu</returns>
        public Guid SaveOrUpdate(EmployeeDto employee)
        {
            try
            {
                if (employee.Id == Guid.Empty || GetById(employee.Id) == null)
                {
                    return Insert(employee);
                }
                else
                {
                    Update(employee);
                    return employee.Id;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi lưu nhân viên: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Xóa nhân viên.
        /// </summary>
        /// <param name="id">ID của nhân viên cần xóa</param>
        public void Delete(Guid id)
        {
            // Kiểm tra nhân viên có tồn tại không
            var existingEmployee = GetById(id);
            if (existingEmployee == null)
            {
                throw new Exception("Không tìm thấy nhân viên để xóa");
            }

            GetDataAccess().Delete(existingEmployee);
        }

        /// <summary>
        /// Kiểm tra mã nhân viên có tồn tại không.
        /// </summary>
        /// <param name="employeeCode">Mã nhân viên cần kiểm tra</param>
        /// <returns>True nếu tồn tại, False nếu không</returns>
        public bool IsEmployeeCodeExists(string employeeCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(employeeCode))
                    return false;

                return GetDataAccess().IsEmployeeCodeExists(employeeCode.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi kiểm tra mã nhân viên: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Kiểm tra mã nhân viên có tồn tại không (trừ bản ghi hiện tại).
        /// </summary>
        /// <param name="employeeCode">Mã nhân viên cần kiểm tra</param>
        /// <param name="excludeId">ID của bản ghi cần loại trừ</param>
        /// <returns>True nếu tồn tại, False nếu không</returns>
        public bool IsEmployeeCodeExists(string employeeCode, Guid excludeId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(employeeCode))
                    return false;

                return GetDataAccess().IsEmployeeCodeExists(employeeCode.Trim(), excludeId);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi kiểm tra mã nhân viên: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Cập nhật chỉ avatar thumbnail của nhân viên
        /// </summary>
        /// <param name="employeeId">ID của nhân viên</param>
        /// <param name="avatarThumbnailBytes">Dữ liệu hình ảnh thumbnail</param>
        public void UpdateAvatarOnly(Guid employeeId, byte[] avatarThumbnailBytes)
        {
            try
            {
                var employee = GetById(employeeId);
                if (employee == null)
                {
                    throw new Exception("Không tìm thấy nhân viên để cập nhật avatar");
                }

                employee.AvatarThumbnailData = avatarThumbnailBytes;
                employee.ModifiedDate = DateTime.Now;

                GetDataAccess().Update(employee);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật avatar thumbnail cho nhân viên với ID {employeeId}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Xóa avatar của nhân viên
        /// </summary>
        /// <param name="employeeId">ID của nhân viên</param>
        public void DeleteAvatarOnly(Guid employeeId)
        {
            try
            {
                UpdateAvatarOnly(employeeId, null);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa avatar cho nhân viên với ID {employeeId}: {ex.Message}", ex);
            }
        }

        #endregion

        #region ========== TIỆN ÍCH ==========

        /// <summary>
        /// Validate dữ liệu nhân viên.
        /// </summary>
        /// <param name="employee">EmployeeDto cần validate</param>
        private void ValidateEmployee(EmployeeDto employee)
        {
            if (employee == null)
                throw new Exception("Thông tin nhân viên không được để trống");

            if (string.IsNullOrWhiteSpace(employee.EmployeeCode))
                throw new Exception("Mã nhân viên không được để trống");

            if (string.IsNullOrWhiteSpace(employee.FullName))
                throw new Exception("Họ và tên không được để trống");

            if (employee.EmployeeCode.Trim().Length > 50)
                throw new Exception("Mã nhân viên không được vượt quá 50 ký tự");

            if (employee.FullName.Trim().Length > 100)
                throw new Exception("Họ và tên không được vượt quá 100 ký tự");

            if (!string.IsNullOrWhiteSpace(employee.Phone) && employee.Phone.Trim().Length > 50)
                throw new Exception("Số điện thoại không được vượt quá 50 ký tự");

            if (!string.IsNullOrWhiteSpace(employee.Email) && employee.Email.Trim().Length > 100)
                throw new Exception("Email không được vượt quá 100 ký tự");
        }

        #endregion
    }
}
