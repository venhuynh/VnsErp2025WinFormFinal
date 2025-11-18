using Common.Appconfig;
using Dal.DataAccess.Implementations.MasterData.ProductServiceRepositories;
using Dal.DataAccess.Interfaces.MasterData.ProductServiceRepositories;
using System;
using System.Collections.Generic;
using Attribute = Dal.DataContext.Attribute;

namespace Bll.MasterData.ProductServiceBll
{
    public class AttributeBll
    {
        #region Fields

        private IAttributeRepository _dataAccess;
        private readonly object _lockObject = new object();

        #endregion

        #region Constructors

        public AttributeBll()
        {
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Lấy hoặc khởi tạo Repository (lazy initialization)
        /// </summary>
        private IAttributeRepository GetDataAccess()
        {
            if (_dataAccess == null)
            {
                lock (_lockObject)
                {
                    if (_dataAccess == null)
                    {
                        try
                        {
                            var globalConnectionString = ApplicationStartupManager.Instance.GetGlobalConnectionString();
                            if (string.IsNullOrEmpty(globalConnectionString))
                            {
                                throw new InvalidOperationException(
                                    "Không có global connection string. Ứng dụng chưa được khởi tạo hoặc chưa sẵn sàng.");
                            }

                            _dataAccess = new AttributeRepository(globalConnectionString);
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

        public List<Attribute> GetAll()
        {
            return GetDataAccess().GetAll();
        }

        public Attribute GetById(Guid id)
        {
            return GetDataAccess().GetById(id);
        }

        public void SaveOrUpdate(Attribute entity)
        {
            GetDataAccess().SaveOrUpdate(entity);
        }

        public void Delete(Guid id)
        {
            GetDataAccess().Delete(id);
        }

        public bool IsNameExists(string name, Guid? excludeId = null)
        {
            return GetDataAccess().IsNameExists(name, excludeId);
        }

        public bool HasDependencies(Guid id)
        {
            return GetDataAccess().HasDependencies(id);
        }
    }
}
