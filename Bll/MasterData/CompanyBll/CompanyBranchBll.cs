using Dal.Connection;
using Dal.DataAccess.Implementations.MasterData.CompanyRepository;
using Dal.DataAccess.Interfaces.MasterData.CompanyRepository;
using Dal.DataContext;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bll.MasterData.CompanyBll
{
    /// <summary>
    /// Business Logic Layer cho quản lý chi nhánh công ty.
    /// Cung cấp các phương thức xử lý nghiệp vụ cho chi nhánh công ty.
    /// </summary>
    public class CompanyBranchBll
    {
        #region Fields

        private ICompanyBranchRepository _companyBranchDataAccess;
        private readonly object _lockObject = new object();

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor mặc định
        /// </summary>
        public CompanyBranchBll()
        {
            
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Lấy hoặc khởi tạo Repository (lazy initialization)
        /// </summary>
        private ICompanyBranchRepository GetDataAccess()
        {
            if (_companyBranchDataAccess != null) return _companyBranchDataAccess;
            lock (_lockObject)
            {
                if (_companyBranchDataAccess != null) return _companyBranchDataAccess;
                try
                {
                    // Sử dụng global connection string từ ApplicationStartupManager
                    var globalConnectionString = ApplicationStartupManager.Instance.GetGlobalConnectionString();
                    if (string.IsNullOrEmpty(globalConnectionString))
                    {
                        throw new InvalidOperationException(
                            "Không có global connection string. Ứng dụng chưa được khởi tạo hoặc chưa sẵn sàng.");
                    }

                    // ReSharper disable once PossibleMultipleWriteAccessInDoubleCheckLocking
                    _companyBranchDataAccess = new CompanyBranchRepository(globalConnectionString);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException(
                        "Không thể khởi tạo kết nối database. Vui lòng kiểm tra cấu hình database.", ex);
                }
            }

            return _companyBranchDataAccess;
        }

        #endregion

        #region ========== QUẢN LÝ DỮ LIỆU ==========

        /// <summary>
        /// Lấy tất cả chi nhánh công ty (Async).
        /// </summary>
        /// <returns>Danh sách tất cả chi nhánh công ty</returns>
        public async Task<List<CompanyBranch>> GetAllAsync()
        {
            try
            {
                // Lấy tất cả chi nhánh từ công ty duy nhất (Guid.Empty)
                return await Task.Run(() => GetDataAccess().GetByCompanyId(Guid.Empty));
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi lấy danh sách chi nhánh công ty: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Lấy tất cả chi nhánh công ty (Sync).
        /// </summary>
        /// <returns>Danh sách tất cả chi nhánh công ty</returns>
        public List<CompanyBranch> GetAll()
        {
            try
            {
                // Lấy tất cả chi nhánh từ công ty duy nhất (Guid.Empty)
                return GetDataAccess().GetByCompanyId(Guid.Empty);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi lấy danh sách chi nhánh công ty: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Lấy chi nhánh công ty theo ID.
        /// </summary>
        /// <param name="id">ID của chi nhánh công ty</param>
        /// <returns>Chi nhánh công ty hoặc null nếu không tìm thấy</returns>
        public CompanyBranch GetById(Guid id)
        {
            try
            {
                return GetDataAccess().GetById(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi lấy thông tin chi nhánh công ty: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Lấy chi nhánh công ty theo mã chi nhánh (trong Company duy nhất).
        /// </summary>
        /// <param name="branchCode">Mã chi nhánh</param>
        /// <returns>Chi nhánh công ty hoặc null nếu không tìm thấy</returns>
        public CompanyBranch GetByBranchCode(string branchCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(branchCode))
                    return null;

                // Sử dụng GetByBranchCodeAndCompany với Guid.Empty để lấy từ Company duy nhất
                return GetDataAccess().GetByBranchCodeAndCompany(branchCode.Trim(), Guid.Empty);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi lấy thông tin chi nhánh công ty theo mã: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Lấy danh sách chi nhánh công ty đang hoạt động.
        /// </summary>
        /// <returns>Danh sách chi nhánh công ty đang hoạt động</returns>
        public async Task<List<CompanyBranch>> GetActiveBranchesAsync()
        {
            try
            {
                return await GetDataAccess().GetActiveBranchesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi lấy danh sách chi nhánh công ty đang hoạt động: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Lấy danh sách chi nhánh công ty đang hoạt động (Sync).
        /// </summary>
        /// <returns>Danh sách chi nhánh công ty đang hoạt động</returns>
        public List<CompanyBranch> GetActiveBranches()
        {
            try
            {
                return GetDataAccess().GetActiveBranches();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi lấy danh sách chi nhánh công ty đang hoạt động: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Lấy tất cả chi nhánh của một công ty.
        /// </summary>
        /// <param name="companyId">ID công ty (nếu Guid.Empty thì lấy từ Company duy nhất)</param>
        /// <returns>Danh sách chi nhánh của công ty</returns>
        public List<CompanyBranch> GetByCompanyId(Guid companyId)
        {
            try
            {
                return GetDataAccess().GetByCompanyId(companyId);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi lấy danh sách chi nhánh theo công ty: " + ex.Message, ex);
            }
        }

        #endregion

        #region ========== XỬ LÝ DỮ LIỆU ==========

        /// <summary>
        /// Thêm mới chi nhánh công ty.
        /// </summary>
        /// <param name="companyBranch">Chi nhánh công ty cần thêm</param>
        /// <returns>ID của chi nhánh công ty vừa thêm</returns>
        public Guid Insert(CompanyBranch companyBranch)
        {
            try
            {
                // Validate dữ liệu đầu vào
                ValidateCompanyBranch(companyBranch);

                // Kiểm tra trùng lặp mã chi nhánh
                if (IsBranchCodeExists(companyBranch.BranchCode))
                {
                    throw new Exception($"Mã chi nhánh '{companyBranch.BranchCode}' đã tồn tại trong hệ thống");
                }

                // Thiết lập thông tin mặc định
                companyBranch.Id = Guid.NewGuid();

                return GetDataAccess().Insert(companyBranch);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi thêm mới chi nhánh công ty: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Cập nhật chi nhánh công ty.
        /// </summary>
        /// <param name="companyBranch">Chi nhánh công ty cần cập nhật</param>
        public void Update(CompanyBranch companyBranch)
        {
            try
            {
                // Validate dữ liệu đầu vào
                ValidateCompanyBranch(companyBranch);

                // Kiểm tra chi nhánh có tồn tại không
                var existingBranch = GetById(companyBranch.Id);
                if (existingBranch == null)
                {
                    throw new Exception("Không tìm thấy chi nhánh công ty để cập nhật");
                }

                // Kiểm tra trùng lặp mã chi nhánh (trừ bản ghi hiện tại)
                if (IsBranchCodeExists(companyBranch.BranchCode, companyBranch.Id))
                {
                    throw new Exception($"Mã chi nhánh '{companyBranch.BranchCode}' đã tồn tại trong hệ thống");
                }

                // TODO: Update method chưa được implement trong repository
                // Cần thêm Update method vào ICompanyBranchRepository và CompanyBranchRepository
                throw new NotImplementedException("Phương thức Update chưa được triển khai trong repository");
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi cập nhật chi nhánh công ty: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Xóa chi nhánh công ty.
        /// </summary>
        /// <param name="id">ID của chi nhánh công ty cần xóa</param>
        public void Delete(Guid id)
        {
            try
            {
                // Kiểm tra chi nhánh có tồn tại không
                var existingBranch = GetById(id);
                if (existingBranch == null)
                {
                    throw new Exception("Không tìm thấy chi nhánh công ty để xóa");
                }

                // Kiểm tra ràng buộc dữ liệu (nếu có)
                if (HasDataConstraints())
                {
                    throw new Exception("Không thể xóa chi nhánh công ty do có dữ liệu liên quan");
                }

                // TODO: Delete method chưa được implement trong repository
                // Cần thêm Delete method vào ICompanyBranchRepository và CompanyBranchRepository
                throw new NotImplementedException("Phương thức Delete chưa được triển khai trong repository");
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi xóa chi nhánh công ty: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Kiểm tra mã chi nhánh có tồn tại không.
        /// </summary>
        /// <param name="branchCode">Mã chi nhánh cần kiểm tra</param>
        /// <returns>True nếu tồn tại, False nếu không</returns>
        public bool IsBranchCodeExists(string branchCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(branchCode))
                    return false;

                return GetDataAccess().IsBranchCodeExists(branchCode.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi kiểm tra mã chi nhánh: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Kiểm tra mã chi nhánh có tồn tại không (trừ bản ghi hiện tại).
        /// </summary>
        /// <param name="branchCode">Mã chi nhánh cần kiểm tra</param>
        /// <param name="excludeId">ID của bản ghi cần loại trừ</param>
        /// <returns>True nếu tồn tại, False nếu không</returns>
        public bool IsBranchCodeExists(string branchCode, Guid excludeId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(branchCode))
                    return false;

                return GetDataAccess().IsBranchCodeExists(branchCode.Trim(), excludeId);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi kiểm tra mã chi nhánh: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Kiểm tra tên chi nhánh có tồn tại không.
        /// </summary>
        /// <param name="branchName">Tên chi nhánh cần kiểm tra</param>
        /// <returns>True nếu tồn tại, False nếu không</returns>
        public bool IsBranchNameExists(string branchName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(branchName))
                    return false;

                return GetDataAccess().IsBranchNameExists(branchName.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi kiểm tra tên chi nhánh: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Kiểm tra tên chi nhánh có tồn tại không (trừ bản ghi hiện tại).
        /// </summary>
        /// <param name="branchName">Tên chi nhánh cần kiểm tra</param>
        /// <param name="excludeId">ID của bản ghi cần loại trừ</param>
        /// <returns>True nếu tồn tại, False nếu không</returns>
        public bool IsBranchNameExists(string branchName, Guid excludeId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(branchName))
                    return false;

                return GetDataAccess().IsBranchNameExists(branchName.Trim(), excludeId);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi kiểm tra tên chi nhánh: " + ex.Message, ex);
            }
        }

        #endregion

        #region ========== TIỆN ÍCH ==========

        /// <summary>
        /// Validate dữ liệu chi nhánh công ty.
        /// </summary>
        /// <param name="companyBranch">Chi nhánh công ty cần validate</param>
        private void ValidateCompanyBranch(CompanyBranch companyBranch)
        {
            if (companyBranch == null)
                throw new Exception("Thông tin chi nhánh công ty không được để trống");

            if (string.IsNullOrWhiteSpace(companyBranch.BranchCode))
                throw new Exception("Mã chi nhánh không được để trống");

            if (string.IsNullOrWhiteSpace(companyBranch.BranchName))
                throw new Exception("Tên chi nhánh không được để trống");

            if (companyBranch.BranchCode.Trim().Length > 20)
                throw new Exception("Mã chi nhánh không được vượt quá 20 ký tự");

            if (companyBranch.BranchName.Trim().Length > 100)
                throw new Exception("Tên chi nhánh không được vượt quá 100 ký tự");

            if (!string.IsNullOrWhiteSpace(companyBranch.Phone) && companyBranch.Phone.Trim().Length > 20)
                throw new Exception("Số điện thoại không được vượt quá 20 ký tự");

            if (!string.IsNullOrWhiteSpace(companyBranch.Email) && companyBranch.Email.Trim().Length > 100)
                throw new Exception("Email không được vượt quá 100 ký tự");

            if (!string.IsNullOrWhiteSpace(companyBranch.Email) && !IsValidEmail(companyBranch.Email))
                throw new Exception("Email không đúng định dạng");

            if (!string.IsNullOrWhiteSpace(companyBranch.Address) && companyBranch.Address.Trim().Length > 255)
                throw new Exception("Địa chỉ không được vượt quá 255 ký tự");

            if (!string.IsNullOrWhiteSpace(companyBranch.ManagerName) && companyBranch.ManagerName.Trim().Length > 100)
                throw new Exception("Tên người quản lý không được vượt quá 100 ký tự");
        }

        /// <summary>
        /// Kiểm tra email có đúng định dạng không.
        /// </summary>
        /// <param name="email">Email cần kiểm tra</param>
        /// <returns>True nếu đúng định dạng, False nếu không</returns>
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Kiểm tra chi nhánh có ràng buộc dữ liệu không.
        /// </summary>
        /// <returns>True nếu có ràng buộc, False nếu không</returns>
        private bool HasDataConstraints()
        {
            try
            {
                // TODO: Kiểm tra các ràng buộc dữ liệu
                // Ví dụ: Kiểm tra có nhân viên nào thuộc chi nhánh này không
                // Ví dụ: Kiểm tra có đơn hàng nào liên quan đến chi nhánh này không
                
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
