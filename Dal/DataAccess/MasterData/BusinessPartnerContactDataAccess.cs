using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dal.BaseDataAccess;
using Dal.DataContext;
using Dal.Logging;

namespace Dal.DataAccess.MasterData
{
    /// <summary>
    /// DataAccess cho BusinessPartnerContact dựa trên BaseDataAccess.
    /// </summary>
    public class BusinessPartnerContactDataAccess : BaseDataAccess<BusinessPartnerContact>
    {
        #region Constructors

        public BusinessPartnerContactDataAccess(ILogger logger = null) : base(logger)
        {
        }

        public BusinessPartnerContactDataAccess(string connectionString, ILogger logger = null) : base(connectionString, logger)
        {
        }

        #endregion

        #region Convenience Methods

        public List<BusinessPartnerContact> GetAllContacts()
        {
            return GetAll();
        }

        public Task<List<BusinessPartnerContact>> GetAllContactsAsync()
        {
            return GetAllAsync();
        }

        public BusinessPartnerContact GetById(Guid id)
        {
            return base.GetById(id);
        }

        public async Task<BusinessPartnerContact> GetByIdAsync(Guid id)
        {
            try
            {
                using var context = CreateContext();
                return await Task.Run(() => context.BusinessPartnerContacts.FirstOrDefault(c => c.Id == id));
            }
            catch
            {
                return null;
            }
        }

        public void SaveOrUpdate(BusinessPartnerContact entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            using var context = CreateContext();
            var existing = entity.Id != Guid.Empty ? context.BusinessPartnerContacts.FirstOrDefault(x => x.Id == entity.Id) : null;
            if (existing == null)
            {
                if (entity.Id == Guid.Empty)
                    entity.Id = Guid.NewGuid();
                context.BusinessPartnerContacts.InsertOnSubmit(entity);
            }
            else
            {
                existing.PartnerId = entity.PartnerId;
                existing.FullName = entity.FullName;
                existing.Position = entity.Position;
                existing.Phone = entity.Phone;
                existing.Email = entity.Email;
                existing.IsPrimary = entity.IsPrimary;
            }
            context.SubmitChanges();
        }

        public void DeleteById(Guid id)
        {
            using var context = CreateContext();
            var entity = context.BusinessPartnerContacts.FirstOrDefault(x => x.Id == id);
            if (entity == null) return;
            context.BusinessPartnerContacts.DeleteOnSubmit(entity);
            context.SubmitChanges();
        }

        public bool IsPhoneExists(string phone, Guid excludeId)
        {
            if (string.IsNullOrWhiteSpace(phone)) return false;
            using var context = CreateContext();
            var normalized = phone.Trim();
            return context.BusinessPartnerContacts.Any(c => c.Phone == normalized && c.Id != excludeId);
        }

        #endregion
    }
}


