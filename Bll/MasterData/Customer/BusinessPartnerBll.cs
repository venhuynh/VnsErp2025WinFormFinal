using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dal.DataAccess;
using Dal.DataAccess.MasterData.Partner;
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
        /// Lấy toàn bộ đối tác (entities) - Async.
        /// </summary>
        public Task<List<BusinessPartner>> GetAllAsync()
        {
            return _businessPartnerDataAccess.GetAllAsync();
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
