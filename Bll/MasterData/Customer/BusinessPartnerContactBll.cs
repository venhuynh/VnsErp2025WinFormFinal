using Dal.DataAccess.MasterData;
using Dal.DataContext;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bll.MasterData.Customer
{
    /// <summary>
    /// BLL cho BusinessPartnerContact.
    /// </summary>
    public class BusinessPartnerContactBll
    {
        private readonly BusinessPartnerContactDataAccess _dataAccess = new();

        public List<BusinessPartnerContact> GetAll()
        {
            return _dataAccess.GetAllContacts();
        }

        public Task<List<BusinessPartnerContact>> GetAllAsync()
        {
            return _dataAccess.GetAllContactsAsync();
        }

        public BusinessPartnerContact GetById(Guid id)
        {
            return _dataAccess.GetById(id);
        }

        public Task<BusinessPartnerContact> GetByIdAsync(Guid id)
        {
            return _dataAccess.GetByIdAsync(id);
        }

        public void SaveOrUpdate(BusinessPartnerContact entity)
        {
            _dataAccess.SaveOrUpdate(entity);
        }

        public bool IsPhoneExists(string phone, Guid excludeId)
        {
            return _dataAccess.IsPhoneExists(phone, excludeId);
        }

        public void Delete(Guid id)
        {
            _dataAccess.DeleteById(id);
        }
    }
}


