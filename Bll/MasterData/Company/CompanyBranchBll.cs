using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dal.DataAccess.Implementations.MasterData.Company;
using Dal.DataContext;
using Dal.Exceptions;

namespace Bll.MasterData.Company
{
    /// <summary>
    /// Business Logic Layer cho quản lý chi nhánh công ty.
    /// Cung cấp các phương thức xử lý nghiệp vụ cho chi nhánh công ty.
    /// </summary>
    public class CompanyBranchBll
    {
        #region ========== KHAI BÁO BIẾN ==========

        /// <summary>
        /// Data Access Layer cho chi nhánh công ty
        /// </summary>
        private readonly CompanyBranchDataAccess _companyBranchDataAccess = new CompanyBranchDataAccess();

        #endregion

        #region ========== CONSTRUCTOR & PUBLIC METHODS ==========

        /// <summary>
        /// Khởi tạo Business Logic Layer cho chi nhánh công ty.
        /// </summary>
        public CompanyBranchBll()
        {
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
                return await _companyBranchDataAccess.GetAllAsync();
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
                return _companyBranchDataAccess.GetAll();
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
                return _companyBranchDataAccess.GetById(id);
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
                return _companyBranchDataAccess.GetByBranchCodeAndCompany(branchCode.Trim(), Guid.Empty);
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
                return await _companyBranchDataAccess.GetActiveBranchesAsync();
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
                return _companyBranchDataAccess.GetActiveBranches();
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
                return _companyBranchDataAccess.GetByCompanyId(companyId);
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

                return _companyBranchDataAccess.Insert(companyBranch);
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


                _companyBranchDataAccess.Update(companyBranch);
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
                if (HasDataConstraints(id))
                {
                    throw new Exception("Không thể xóa chi nhánh công ty do có dữ liệu liên quan");
                }

                _companyBranchDataAccess.Delete(existingBranch);
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

                return _companyBranchDataAccess.IsBranchCodeExists(branchCode.Trim());
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

                return _companyBranchDataAccess.IsBranchCodeExists(branchCode.Trim(), excludeId);
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

                return _companyBranchDataAccess.IsBranchNameExists(branchName.Trim());
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

                return _companyBranchDataAccess.IsBranchNameExists(branchName.Trim(), excludeId);
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
        /// <param name="id">ID của chi nhánh</param>
        /// <returns>True nếu có ràng buộc, False nếu không</returns>
        private bool HasDataConstraints(Guid id)
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
