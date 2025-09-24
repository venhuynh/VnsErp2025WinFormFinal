using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dal.DataContext;
using Dal.Exceptions;
using Dal.Connection;
using Dal.Configuration;
using Dal.Logging;
using Dal.Helpers;

namespace Dal.BaseDataAccess
{
    /// <summary>
    /// Base class cho tất cả DataAccess classes (Enhanced with Configuration, Logging, Retry)
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public abstract class BaseDataAccess<T> where T : class
    {
        #region thuocTinhDonGian

        /// <summary>
        /// Database settings
        /// </summary>
        protected readonly DatabaseSettings _settings;

        /// <summary>
        /// Logger instance
        /// </summary>
        protected readonly ILogger _logger;

        /// <summary>
        /// Connection string (backward compatibility)
        /// </summary>
        protected readonly string _connStr;

        #endregion

        #region phuongThuc

        /// <summary>
        /// Constructor mặc định
        /// </summary>
        protected BaseDataAccess(ILogger logger = null)
        {
            _settings = ConfigurationManager.DatabaseSettings;
            _logger = logger ?? new NullLogger();
            _connStr = _settings.ConnectionString;
        }

        /// <summary>
        /// Constructor với connection string
        /// </summary>
        /// <param name="connectionString">Connection string</param>
        /// <param name="logger">Logger instance</param>
        protected BaseDataAccess(string connectionString, ILogger logger = null)
        {
            _connStr = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            _logger = logger ?? new NullLogger();
            _settings = ConfigurationManager.DatabaseSettings;
        }

        /// <summary>
        /// Lấy tất cả records
        /// </summary>
        /// <returns>Danh sách tất cả records</returns>
        public virtual List<T> LayTatCa()
        {
            var operationName = $"LayTatCa_{typeof(T).Name}";
            _logger.LogDebug("Starting {0}", operationName);

            if (_settings.EnableRetryOnFailure)
            {
                return RetryHelper.ExecuteWithRetry(
                    () =>
                    {
                        using var context = CreateContext();
                        var result = context.GetTable<T>().ToList();
                        _logger.LogDebug("{0} completed successfully", operationName);
                        return result;
                    },
                    _settings.MaxRetryCount,
                    _settings.RetryDelayMs,
                    RetryHelper.ShouldRetrySqlException,
                    _logger
                );
            }
            else
            {
                return ExecuteWithErrorHandling(
                    () =>
                    {
                        using var context = CreateContext();
                        return context.GetTable<T>().ToList();
                    },
                    operationName
                );
            }
        }

        /// <summary>
        /// Lấy tất cả records (Async)
        /// </summary>
        /// <returns>Danh sách tất cả records</returns>
        public virtual async Task<List<T>> LayTatCaAsync()
        {
            try
            {
                using var context = new VnsErp2025DataContext(_connStr);
                // LINQ to SQL không có async methods, sử dụng Task.Run
                return await Task.Run(() => context.GetTable<T>().ToList());
            }
            catch (System.Data.SqlClient.SqlException sqlEx)
            {
                throw new DataAccessException($"Lỗi SQL khi lấy tất cả {typeof(T).Name}: {sqlEx.Message}", sqlEx)
                {
                    SqlErrorNumber = sqlEx.Number,
                    ThoiGianLoi = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi lấy tất cả {typeof(T).Name}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy record theo ID
        /// </summary>
        /// <param name="id">ID của record</param>
        /// <returns>Record tìm được hoặc null</returns>
        public virtual T LayTheoId(object id)
        {
            try
            {
                if (id == null)
                    return null;

                using var context = new VnsErp2025DataContext(_connStr);
                var table = context.GetTable<T>();
                var primaryKeyProperty = LayPrimaryKeyProperty();
                
                if (primaryKeyProperty == null)
                    throw new DataAccessException($"Không tìm thấy primary key cho {typeof(T).Name}");

                // Sử dụng reflection để tìm theo primary key
                foreach (var item in table)
                {
                    var value = primaryKeyProperty.GetValue(item);
                    if (Equals(value, id))
                        return item;
                }
                
                return null;
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi lấy {typeof(T).Name} theo ID {id}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Thêm record mới
        /// </summary>
        /// <param name="entity">Entity cần thêm</param>
        public virtual void Them(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));

                using var context = new VnsErp2025DataContext(_connStr);
                context.GetTable<T>().InsertOnSubmit(entity);
                context.SubmitChanges();
            }
            catch (System.Data.SqlClient.SqlException sqlEx) when (sqlEx.Number == 2627) // Duplicate key
            {
                throw new DataAccessException($"Lỗi: {typeof(T).Name} đã tồn tại trong hệ thống", sqlEx)
                {
                    SqlErrorNumber = sqlEx.Number,
                    ThoiGianLoi = DateTime.Now
                };
            }
            catch (System.Data.SqlClient.SqlException sqlEx) when (sqlEx.Number == 515) // Cannot insert null
            {
                throw new DataAccessException($"Lỗi: Không thể thêm {typeof(T).Name} - thiếu dữ liệu bắt buộc", sqlEx)
                {
                    SqlErrorNumber = sqlEx.Number,
                    ThoiGianLoi = DateTime.Now
                };
            }
            catch (System.Data.SqlClient.SqlException sqlEx)
            {
                throw new DataAccessException($"Lỗi SQL khi thêm {typeof(T).Name}: {sqlEx.Message}", sqlEx)
                {
                    SqlErrorNumber = sqlEx.Number,
                    ThoiGianLoi = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi thêm {typeof(T).Name}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Thêm record mới (Async)
        /// </summary>
        /// <param name="entity">Entity cần thêm</param>
        public virtual async Task ThemAsync(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));

                using var context = new VnsErp2025DataContext(_connStr);
                context.GetTable<T>().InsertOnSubmit(entity);
                await Task.Run(() => context.SubmitChanges());
            }
            catch (System.Data.SqlClient.SqlException sqlEx) when (sqlEx.Number == 2627) // Duplicate key
            {
                throw new DataAccessException($"Lỗi: {typeof(T).Name} đã tồn tại trong hệ thống", sqlEx)
                {
                    SqlErrorNumber = sqlEx.Number,
                    ThoiGianLoi = DateTime.Now
                };
            }
            catch (System.Data.SqlClient.SqlException sqlEx) when (sqlEx.Number == 515) // Cannot insert null
            {
                throw new DataAccessException($"Lỗi: Không thể thêm {typeof(T).Name} - thiếu dữ liệu bắt buộc", sqlEx)
                {
                    SqlErrorNumber = sqlEx.Number,
                    ThoiGianLoi = DateTime.Now
                };
            }
            catch (System.Data.SqlClient.SqlException sqlEx)
            {
                throw new DataAccessException($"Lỗi SQL khi thêm {typeof(T).Name}: {sqlEx.Message}", sqlEx)
                {
                    SqlErrorNumber = sqlEx.Number,
                    ThoiGianLoi = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi thêm {typeof(T).Name}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Cập nhật record
        /// </summary>
        /// <param name="entity">Entity cần cập nhật</param>
        public virtual void CapNhat(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));

                using var context = new VnsErp2025DataContext(_connStr);
                // LINQ to SQL tự động track changes khi entity đã được attach
                context.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi cập nhật {typeof(T).Name}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Xóa record theo ID
        /// </summary>
        /// <param name="id">ID của record cần xóa</param>
        public virtual void Xoa(object id)
        {
            try
            {
                var entity = LayTheoId(id);
                if (entity != null)
                {
                    Xoa(entity);
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi xóa {typeof(T).Name} theo ID {id}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Xóa entity
        /// </summary>
        /// <param name="entity">Entity cần xóa</param>
        public virtual void Xoa(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));

                using var context = new VnsErp2025DataContext(_connStr);
                context.GetTable<T>().DeleteOnSubmit(entity);
                context.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi xóa {typeof(T).Name}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Kiểm tra entity có tồn tại không
        /// </summary>
        /// <param name="id">ID của entity</param>
        /// <returns>True nếu tồn tại</returns>
        public virtual bool TonTai(object id)
        {
            try
            {
                return LayTheoId(id) != null;
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi kiểm tra tồn tại {typeof(T).Name} với ID {id}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Đếm số lượng records
        /// </summary>
        /// <returns>Số lượng records</returns>
        public virtual int DemSoLuong()
        {
            try
            {
                using var context = new VnsErp2025DataContext(_connStr);
                return context.GetTable<T>().Count();
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi đếm số lượng {typeof(T).Name}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy primary key property của entity
        /// </summary>
        /// <returns>Primary key property</returns>
        protected virtual System.Reflection.PropertyInfo LayPrimaryKeyProperty()
        {
            var properties = typeof(T).GetProperties();
            return properties.FirstOrDefault(p => 
                p.GetCustomAttributes(typeof(System.Data.Linq.Mapping.ColumnAttribute), false)
                 .Cast<System.Data.Linq.Mapping.ColumnAttribute>()
                 .Any(attr => attr.IsPrimaryKey));
        }

        /// <summary>
        /// Create DataContext với settings
        /// </summary>
        protected virtual VnsErp2025DataContext CreateContext()
        {
            var context = new VnsErp2025DataContext(_connStr);
            context.CommandTimeout = _settings.CommandTimeout;
            return context;
        }

        /// <summary>
        /// Execute operation với error handling
        /// </summary>
        protected virtual TResult ExecuteWithErrorHandling<TResult>(Func<TResult> operation, string operationName)
        {
            try
            {
                return operation();
            }
            catch (System.Data.SqlClient.SqlException sqlEx)
            {
                _logger.LogError("SQL error in {0}: {1} (Error: {2})", sqlEx, operationName, sqlEx.Message, sqlEx.Number);
                throw new DataAccessException($"Lỗi SQL khi thực hiện {operationName}: {sqlEx.Message}", sqlEx)
                {
                    SqlErrorNumber = sqlEx.Number,
                    ThoiGianLoi = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in {0}: {1}", ex, operationName, ex.Message);
                throw new DataAccessException($"Lỗi khi thực hiện {operationName}: {ex.Message}", ex);
            }
        }

        #endregion
    }
}