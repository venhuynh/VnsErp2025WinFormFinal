using Common.Appconfig;
using Dal.DataAccess.Implementations.MasterData.Company;
using Dal.DataAccess.Implementations.MasterData.Partner;
using Dal.DataAccess.Interfaces;
using Dal.DataAccess.Interfaces.MasterData.Company;
using Dal.DataAccess.Interfaces.MasterData.Partner;
using Dal.DataContext;
using Logger;
using Logger.Configuration;
using System;
using System.Data;
using Dal.DataAccess.Implementations.MasterData.ProductServiceRepositories;
using Dal.DataAccess.Interfaces.MasterData.ProductServiceRepositories;
using CustomLogger = Logger.Interfaces.ILogger;

namespace Dal.DataAccess.Implementations
{
    /// <summary>
    /// Unit of Work implementation
    /// Quản lý transaction và context lifecycle
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        #region Fields & Properties

        private readonly IDataContextFactory _contextFactory;
        private readonly CustomLogger _logger;
        private VnsErp2025DataContext _context;
        private IDbTransaction _transaction;
        private bool _disposed;

        /// <summary>
        /// DataContext instance
        /// </summary>
        public VnsErp2025DataContext Context
        {
            get
            {
                if (_context != null) return _context;
                _context = _contextFactory.CreateContext();
                _logger.Debug("Tạo mới VntaDataContext trong UnitOfWork");
                return _context;
            }
        }

        /// <summary>
        /// Trạng thái transaction
        /// </summary>
        public bool IsInTransaction => _transaction != null;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor với Dependency Injection
        /// </summary>
        /// <param name="contextFactory">DataContext factory</param>
        public UnitOfWork(IDataContextFactory contextFactory)
        {
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
            _logger = LoggerFactory.CreateLogger(LogCategory.DAL);
            _logger.Info("UnitOfWork được khởi tạo");
        }

        #endregion
        #region Transaction Management

        /// <summary>
        /// Bắt đầu transaction
        /// </summary>
        public void BeginTransaction()
        {
            try
            {
                if (_transaction != null)
                {
                    _logger.Warning("Transaction đã được bắt đầu trước đó");
                    return;
                }

                _transaction = Context.Connection.BeginTransaction();
                _logger.Info("Bắt đầu transaction mới");
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi khi bắt đầu transaction: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Commit transaction
        /// </summary>
        public void Commit()
        {
            try
            {
                if (_transaction == null)
                {
                    _logger.Warning("Không có transaction để commit");
                    return;
                }

                _transaction.Commit();
                _logger.Info("Commit transaction thành công");
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi khi commit transaction: {ex.Message}", ex);
                throw;
            }
            finally
            {
                if (_transaction != null)
                {
                    _transaction.Dispose();
                    _transaction = null;
                }
            }
        }

        /// <summary>
        /// Rollback transaction
        /// </summary>
        public void Rollback()
        {
            try
            {
                if (_transaction == null)
                {
                    _logger.Warning("Không có transaction để rollback");
                    return;
                }

                _transaction.Rollback();
                _logger.Info("Rollback transaction thành công");
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi khi rollback transaction: {ex.Message}", ex);
                throw;
            }
            finally
            {
                if (_transaction != null)
                {
                    _transaction.Dispose();
                    _transaction = null;
                }
            }
        }

        /// <summary>
        /// Save changes
        /// </summary>
        public void SaveChanges()
        {
            try
            {
                if (_context == null)
                {
                    _logger.Warning("Không có context để save changes");
                    return;
                }

                _context.SubmitChanges();
                _logger.Info("Save changes thành công");
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi khi save changes: {ex.Message}", ex);
                throw;
            }
        }


        #endregion
        

        #region IDisposable Implementation

        /// <summary>
        /// Dispose resources
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose resources
        /// </summary>
        /// <param name="disposing">True nếu đang dispose</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    try
                    {
                        // Rollback transaction nếu chưa commit
                        if (_transaction != null)
                        {
                            _transaction.Rollback();
                            _transaction.Dispose();
                            _logger.Info("Rollback transaction trong Dispose");
                        }

                        // Dispose context
                        if (_context != null)
                        {
                            _context.Dispose();
                            _logger.Info("Dispose VntaDataContext trong UnitOfWork");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"Lỗi khi dispose UnitOfWork: {ex.Message}", ex);
                    }
                }

                _disposed = true;
            }
        }

        #endregion

        #region Data Access Implementation

        #region MasterData

        #region Company

        public IApplicationUserRepository GetApplicationUserDataAccess()
        {
            // Sử dụng global connection string từ ApplicationStartupManager
            var globalConnectionString = ApplicationStartupManager.Instance.GetGlobalConnectionString();
            return string.IsNullOrEmpty(globalConnectionString)
                ? throw new InvalidOperationException("Global connection string is not configured.")
                : new ApplicationUserRepository(globalConnectionString);
        }

        public ICompanyBranchRepository GetCompanyBranchRepository()
        {
            // Sử dụng global connection string từ ApplicationStartupManager
            var globalConnectionString = ApplicationStartupManager.Instance.GetGlobalConnectionString();
            return string.IsNullOrEmpty(globalConnectionString) 
                ? throw new InvalidOperationException("Global connection string is not configured.") 
                : new CompanyBranchRepository(globalConnectionString);
        }

        public IPositionRepository GetPositionReposiroty()
        {
            // Sử dụng global connection string từ ApplicationStartupManager
            var globalConnectionString = ApplicationStartupManager.Instance.GetGlobalConnectionString();
            return string.IsNullOrEmpty(globalConnectionString)
                ? throw new InvalidOperationException("Global connection string is not configured.")
                : new PositionRepository(globalConnectionString);
        }

        public ICompanyRepository GetCompanyRepository()
        {
            // Sử dụng global connection string từ ApplicationStartupManager
            var globalConnectionString = ApplicationStartupManager.Instance.GetGlobalConnectionString();
            return string.IsNullOrEmpty(globalConnectionString)
                ? throw new InvalidOperationException("Global connection string is not configured.")
                : new CompanyRepository(globalConnectionString);
        }

        public IDepartmentRepository GetDepartmentRepository()
        {
            // Sử dụng global connection string từ ApplicationStartupManager
            var globalConnectionString = ApplicationStartupManager.Instance.GetGlobalConnectionString();
            return string.IsNullOrEmpty(globalConnectionString)
                ? throw new InvalidOperationException("Global connection string is not configured.")
                : new DepartmentRepository(globalConnectionString);
        }

        #endregion

        #region Partner

        public IBusinessPartnerRepository GetBusinessPartnerRepository()
        {
            // Sử dụng global connection string từ ApplicationStartupManager
            var globalConnectionString = ApplicationStartupManager.Instance.GetGlobalConnectionString();
            return string.IsNullOrEmpty(globalConnectionString)
                ? throw new InvalidOperationException("Global connection string is not configured.")
                : new BusinessPartnerRepository(globalConnectionString);
        }

        public IBusinessPartnerCategoryRepository GetBusinessPartnerCategoryRepository()
        {
            // Sử dụng global connection string từ ApplicationStartupManager
            var globalConnectionString = ApplicationStartupManager.Instance.GetGlobalConnectionString();
            return string.IsNullOrEmpty(globalConnectionString)
                ? throw new InvalidOperationException("Global connection string is not configured.")
                : new BusinessPartnerCategoryRepository(globalConnectionString);
        }

        public IBusinessPartnerContactRepository GetBusinessPartnerContactRepository()
        {
            // Sử dụng global connection string từ ApplicationStartupManager
            var globalConnectionString = ApplicationStartupManager.Instance.GetGlobalConnectionString();
            return string.IsNullOrEmpty(globalConnectionString)
                ? throw new InvalidOperationException("Global connection string is not configured.")
                : new BusinessPartnerContactRepository(globalConnectionString);
        }

        public IBusinessPartnerSiteRepository GetBusinessPartnerSiteRepository()
        {
            // Sử dụng global connection string từ ApplicationStartupManager
            var globalConnectionString = ApplicationStartupManager.Instance.GetGlobalConnectionString();
            return string.IsNullOrEmpty(globalConnectionString)
                ? throw new InvalidOperationException("Global connection string is not configured.")
                : new BusinessPartnerSiteRepository(globalConnectionString);
        }

        #endregion

        #region ProductService


        public IAttributeRepository GetAttributeRepository()
        {
            // Sử dụng global connection string từ AttributeRepository
            var globalConnectionString = ApplicationStartupManager.Instance.GetGlobalConnectionString();
            return string.IsNullOrEmpty(globalConnectionString)
                ? throw new InvalidOperationException("Global connection string is not configured.")
                : new AttributeRepository(globalConnectionString);
        }

        public IProductImageRepository GetProductImageRepository()
        {
            // Sử dụng global connection string từ ApplicationStartupManager
            var globalConnectionString = ApplicationStartupManager.Instance.GetGlobalConnectionString();
            return string.IsNullOrEmpty(globalConnectionString)
                ? throw new InvalidOperationException("Global connection string is not configured.")
                : new ProductImageRepository(globalConnectionString);
        }

        public IProductServiceCategoryRepository GetProductServiceCategoryRepository()
        {
            // Sử dụng global connection string từ ApplicationStartupManager
            var globalConnectionString = ApplicationStartupManager.Instance.GetGlobalConnectionString();
            return string.IsNullOrEmpty(globalConnectionString)
                ? throw new InvalidOperationException("Global connection string is not configured.")
                : new ProductServiceCategoryRepository(globalConnectionString);
        }

        public IProductServiceRepository GetProductServiceRepository()
        {
            // Sử dụng global connection string từ ApplicationStartupManager
            var globalConnectionString = ApplicationStartupManager.Instance.GetGlobalConnectionString();
            return string.IsNullOrEmpty(globalConnectionString)
                ? throw new InvalidOperationException("Global connection string is not configured.")
                : new ProductServiceRepository(globalConnectionString);
        }

        public IProductVariantRepository GetProductVariantRepository()
        {
            // Sử dụng global connection string từ ApplicationStartupManager
            var globalConnectionString = ApplicationStartupManager.Instance.GetGlobalConnectionString();
            return string.IsNullOrEmpty(globalConnectionString)
                ? throw new InvalidOperationException("Global connection string is not configured.")
                : new ProductVariantRepository(globalConnectionString);
        }

        public IUnitOfMeasureRepository GetUnitOfMeasureRepository()
        {
            // Sử dụng global connection string từ ApplicationStartupManager
            var globalConnectionString = ApplicationStartupManager.Instance.GetGlobalConnectionString();
            return string.IsNullOrEmpty(globalConnectionString)
                ? throw new InvalidOperationException("Global connection string is not configured.")
                : new UnitOfMeasureRepository(globalConnectionString);
        }


        #endregion

        #endregion

        #endregion
    }
}
