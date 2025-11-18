using System;
using System.Collections.Generic;
using Attribute = Dal.DataContext.Attribute;

namespace Bll.MasterData.ProductServiceBll
{
    public class AttributeBll
    {
        private readonly AttributeDataAccess _dataAccess = new AttributeDataAccess();

        public List<Attribute> GetAll()
        {
            return _dataAccess.GetAll();
        }

        public Attribute GetById(Guid id)
        {
            return _dataAccess.GetById(id);
        }

        public void SaveOrUpdate(Attribute entity)
        {
            _dataAccess.SaveOrUpdate(entity);
        }

        public void Delete(Guid id)
        {
            _dataAccess.Delete(id);
        }

        public bool IsNameExists(string name, Guid? excludeId = null)
        {
            return _dataAccess.IsNameExists(name, excludeId);
        }

        public bool HasDependencies(Guid id)
        {
            return _dataAccess.HasDependencies(id);
        }
    }
}
