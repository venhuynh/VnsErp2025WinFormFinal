using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dal.DataAccess;
using Dal.DataContext;

namespace Bll.MasterData.Customer
{
    /// <summary>
    /// Business logic layer cho BusinessPartner. Trả về entity để UI tự chuyển đổi sang DTO.
    /// Tránh phụ thuộc ngược sang project MasterData (DTO) để không tạo vòng tham chiếu.
    /// </summary>
    public class BusinessPartnerBll
    {
        private readonly BusinessPartnerDataAccess _businessPartnerDataAccess = new();

        /// <summary>
        /// Lấy toàn bộ đối tác (entities).
        /// </summary>
        public List<BusinessPartner> GetAll()
        {
            return _businessPartnerDataAccess.GetAll();
        }

        /// <summary>
        /// Lấy toàn bộ đối tác (entities) - Async.
        /// </summary>
        public Task<List<BusinessPartner>> GetAllAsync()
        {
            return _businessPartnerDataAccess.GetAllAsync();
        }

        /// <summary>
        /// Lấy danh sách đối tác đang hoạt động (entities).
        /// </summary>
        public List<BusinessPartner> GetActivePartners()
        {
            return _businessPartnerDataAccess.GetActivePartners();
        }

        /// <summary>
        /// Tìm theo mã.
        /// </summary>
        public BusinessPartner GetByCode(string code)
        {
            return _businessPartnerDataAccess.GetByCode(code);
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
        /// Tạo mới đối tác cơ bản.
        /// </summary>
        public BusinessPartner AddNew(string code, string name, int partnerType, bool isActive)
        {
            return _businessPartnerDataAccess.AddNewPartner(code, name, partnerType, isActive);
        }

        /// <summary>
        /// Cập nhật thông tin liên hệ cơ bản.
        /// </summary>
        public void UpdateContact(Guid id, string phone, string email)
        {
            _businessPartnerDataAccess.UpdateContactInfo(id, phone, email);
        }

        /// <summary>
        /// Cập nhật trạng thái kích hoạt.
        /// </summary>
        public void SetActive(Guid id, bool isActive)
        {
            _businessPartnerDataAccess.SetActive(id, isActive);
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
