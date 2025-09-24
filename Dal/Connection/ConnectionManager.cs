using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using Dal.Exceptions;

namespace Dal.Connection
{
    /// <summary>
    /// Quản lý vòng đời kết nối SQL Server an toàn (thread-safe) cho DAL.
    /// - Khởi tạo từ Connection String mặc định (ưu tiên User Settings), hoặc chuỗi truyền vào.
    /// - Cung cấp API mở/đóng, tạo lệnh SQL/stored procedure, test kết nối.
    /// - Phát sự kiện khi kết nối mở/đóng/lỗi để lớp cao hơn theo dõi.
    /// </summary>
    public class ConnectionManager : IConnectionManager
    {
        #region Fields & Properties

        private SqlConnection _connection; // Kết nối hiện hành (quản lý nội bộ)
        private readonly object _lockObject = new object(); // Khóa đồng bộ khi mở/đóng/reset
        private bool _disposed; // Đánh dấu đã giải phóng tài nguyên

        /// <summary>
        /// Chuỗi kết nối hiện tại
        /// </summary>
        public string ConnectionString { get; private set; }

        /// <summary>
        /// Trạng thái kết nối
        /// </summary>
        public ConnectionState State => _connection?.State ?? ConnectionState.Closed;

        /// <summary>
        /// Thời gian timeout cho command (giây)
        /// </summary>
        public int CommandTimeout { get; set; } = 30;

        /// <summary>
        /// Thời gian timeout cho connection (giây)
        /// </summary>
        public int ConnectionTimeout { get; set; } = 15;

        #endregion

        #region Events

        /// <summary>
        /// Sự kiện khi kết nối được mở (mới)
        /// </summary>
        public event EventHandler<ConnectionEventArgs> ConnectionOpened;

        /// <summary>
        /// Sự kiện khi kết nối bị đóng (mới)
        /// </summary>
        public event EventHandler<ConnectionEventArgs> ConnectionClosed;

        /// <summary>
        /// Sự kiện khi có lỗi kết nối (mới)
        /// </summary>
        public event EventHandler<ConnectionErrorEventArgs> ConnectionError;

        [Obsolete("Use ConnectionOpened instead")]
        public event EventHandler<ConnectionEventArgs> KetNoiMo;

        [Obsolete("Use ConnectionClosed instead")]
        public event EventHandler<ConnectionEventArgs> KetNoiDong;

        [Obsolete("Use ConnectionError instead")]
        public event EventHandler<ConnectionErrorEventArgs> LoiKetNoi;

        #endregion

        #region Constructors

        /// <summary>
        /// Khởi tạo với Connection String mặc định (ưu tiên Settings/UI, sau đó config).
        /// </summary>
        public ConnectionManager()
        {
            InitializeConnection();
        }

        /// <summary>
        /// Khởi tạo với Connection String chỉ định.
        /// </summary>
        /// <param name="connectionString">Chuỗi kết nối</param>
        public ConnectionManager(string connectionString)
        {
            ConnectionString = connectionString;
            InitializeConnection();
        }

        #endregion

        #region Connection Lifecycle / Factory Methods

        /// <summary>
        /// Tạo instance SqlConnection và gắn xử lý sự kiện trạng thái.
        /// </summary>
        private void InitializeConnection()
        {
            try
            {
                if (string.IsNullOrEmpty(ConnectionString))
                {
                    ConnectionString = ConnectionStringHelper.LayConnectionStringMacDinh();
                }

                _connection = new SqlConnection(ConnectionString);
                _connection.StateChange += OnConnectionStateChange;
            }
            catch (Exception ex)
            {
                OnConnectionError(ex);
                throw new ConnectionException("Không thể khởi tạo kết nối database", ex);
            }
        }

        /// <summary>
        /// Mở kết nối (thread-safe). Nếu đang Closed sẽ thực sự mở và phát sự kiện.
        /// </summary>
        /// <returns>True nếu sau khi gọi, kết nối đang ở trạng thái Open</returns>
        public bool OpenConnection()
        {
            lock (_lockObject)
            {
                try
                {
                    if (_connection == null)
                    {
                        InitializeConnection();
                    }

                    if (_connection is { State: ConnectionState.Closed })
                    {
                        _connection.Open();
                        OnConnectionOpened();
                        return true;
                    }

                    return _connection is { State: ConnectionState.Open };
                }
                catch (Exception ex)
                {
                    OnConnectionError(ex);
                    throw new ConnectionException("Không thể mở kết nối database", ex);
                }
            }
        }

        /// <summary>
        /// Đóng kết nối nếu đang mở (thread-safe) và phát sự kiện đóng.
        /// </summary>
        public void CloseConnection()
        {
            lock (_lockObject)
            {
                try
                {
                    if (_connection != null && _connection.State != ConnectionState.Closed)
                    {
                        _connection.Close();
                        OnConnectionClosed();
                    }
                }
                catch (Exception ex)
                {
                    OnConnectionError(ex);
                    throw new ConnectionException("Không thể đóng kết nối database", ex);
                }
            }
        }

        /// <summary>
        /// Lấy SqlConnection đã sẵn sàng (đảm bảo mở trước khi trả về).
        /// </summary>
        /// <returns>Đối tượng SqlConnection đã Open</returns>
        public SqlConnection GetConnection()
        {
            if (_connection == null)
            {
                InitializeConnection();
            }

            if (_connection is not { State: ConnectionState.Open })
            {
                OpenConnection();
            }

            return _connection;
        }

        #endregion

        #region Command Factory

        /// <summary>
        /// Tạo SqlCommand (text) gắn với kết nối hiện tại.
        /// </summary>
        /// <param name="sql">Câu lệnh SQL dạng text</param>
        /// <returns>Đối tượng SqlCommand đã thiết lập CommandTimeout</returns>
        public SqlCommand CreateCommand(string sql)
        {
            var connection = GetConnection();
            var command = new SqlCommand(sql, connection)
            {
                CommandTimeout = CommandTimeout
            };
            return command;
        }

        /// <summary>
        /// Tạo SqlCommand cho Stored Procedure.
        /// </summary>
        /// <param name="storedProcedureName">Tên Stored Procedure</param>
        /// <returns>Đối tượng SqlCommand đã thiết lập CommandType và CommandTimeout</returns>
        public SqlCommand CreateStoredProcedureCommand(string storedProcedureName)
        {
            var connection = GetConnection();
            var command = new SqlCommand(storedProcedureName, connection)
            {
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = CommandTimeout
            };
            return command;
        }

        #endregion

        #region Health Checks

        /// <summary>
        /// Kiểm tra trạng thái kết nối có đang mở không.
        /// </summary>
        /// <returns>True nếu kết nối đang ở trạng thái Open</returns>
        public bool IsOpen()
        {
            return _connection?.State == ConnectionState.Open;
        }

        /// <summary>
        /// Kiểm tra nhanh tính sẵn sàng của kết nối bằng câu lệnh đơn giản.
        /// </summary>
        /// <returns>True nếu truy vấn thử thành công</returns>
        public bool IsHealthy()
        {
            try
            {
                if (!IsOpen())
                {
                    return false;
                }

                using (var command = new SqlCommand("SELECT 1", _connection))
                {
                    command.CommandTimeout = 5;
                    var result = command.ExecuteScalar();
                    return result != null && result.ToString() == "1";
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Thực hiện truy vấn test để xác nhận kết nối hoạt động.
        /// </summary>
        /// <returns>True nếu thực thi truy vấn thành công</returns>
        public bool TestConnection()
        {
            try
            {
                var connection = GetConnection();
                using (var command = new SqlCommand("SELECT GETDATE()", connection))
                {
                    command.CommandTimeout = 10;
                    var result = command.ExecuteScalar();
                    return result != null;
                }
            }
            catch (Exception ex)
            {
                OnConnectionError(ex);
                return false;
            }
        }

        /// <summary>
        /// Đóng và mở lại kết nối an toàn để làm mới trạng thái.
        /// </summary>
        public void ResetConnection()
        {
            lock (_lockObject)
            {
                try
                {
                    CloseConnection();
                    Thread.Sleep(100); // Chờ một chút trước khi tạo lại
                    InitializeConnection();
                }
                catch (Exception ex)
                {
                    OnConnectionError(ex);
                    throw new ConnectionException("Không thể reset kết nối", ex);
                }
            }
        }

        /// <summary>
        /// Thay đổi Connection String và tạo lại kết nối (thread-safe).
        /// </summary>
        /// <param name="connectionString">Chuỗi kết nối mới</param>
        public void SetConnectionString(string connectionString)
        {
            lock (_lockObject)
            {
                try
                {
                    CloseConnection();
                    ConnectionString = connectionString;
                    InitializeConnection();
                }
                catch (Exception ex)
                {
                    OnConnectionError(ex);
                    throw new ConnectionException("Không thể thiết lập connection string mới", ex);
                }
            }
        }

        #endregion

        #region Event Raisers & Error Handling

        /// <summary>
        /// Bắt sự kiện khi trạng thái kết nối thay đổi (có thể dùng để log/giám sát).
        /// </summary>
        /// <param name="sender">Nguồn phát sự kiện</param>
        /// <param name="e">Thông tin trạng thái cũ/mới</param>
        private void OnConnectionStateChange(object sender, StateChangeEventArgs e)
        {

        }

        /// <summary>
        /// Phát sự kiện thông báo kết nối đã mở.
        /// </summary>
        private void OnConnectionOpened()
        {
            ConnectionOpened?.Invoke(this, new ConnectionEventArgs(ConnectionString, "Kết nối database đã được mở"));
            KetNoiMo?.Invoke(this, new ConnectionEventArgs(ConnectionString, "Kết nối database đã được mở"));
        }

        /// <summary>
        /// Phát sự kiện thông báo kết nối đã đóng.
        /// </summary>
        private void OnConnectionClosed()
        {
            ConnectionClosed?.Invoke(this, new ConnectionEventArgs(ConnectionString, "Kết nối database đã được đóng"));
            KetNoiDong?.Invoke(this, new ConnectionEventArgs(ConnectionString, "Kết nối database đã được đóng"));
        }

        /// <summary>
        /// Phát sự kiện lỗi kết nối kèm mức độ nghiêm trọng.
        /// </summary>
        /// <param name="exception">Lỗi phát sinh</param>
        private void OnConnectionError(Exception exception)
        {
            var severity = DetermineErrorSeverity(exception);
            ConnectionError?.Invoke(this, new ConnectionErrorEventArgs(exception, ConnectionString, severity));
            LoiKetNoi?.Invoke(this, new ConnectionErrorEventArgs(exception, ConnectionString, severity));
        }

        /// <summary>
        /// Quy đổi Exception thành mức độ nghiêm trọng để cảnh báo phù hợp.
        /// </summary>
        /// <param name="exception">Ngoại lệ phát sinh</param>
        /// <returns>Mức độ nghiêm trọng của lỗi</returns>
        private ErrorSeverity DetermineErrorSeverity(Exception exception)
        {
            if (exception is SqlException sqlEx)
            {
                // Các lỗi SQL Server nghiêm trọng
                if (sqlEx.Number == -2 || sqlEx.Number == 2) // Timeout
                    return ErrorSeverity.TrungBinh;
                if (sqlEx.Number == 18456) // Login failed
                    return ErrorSeverity.NghiemTrong;
                if (sqlEx.Number >= 50000) // User defined errors
                    return ErrorSeverity.TrungBinh;
            }

            if (exception is TimeoutException)
                return ErrorSeverity.TrungBinh;

            if (exception is InvalidOperationException)
                return ErrorSeverity.NghiemTrong;

            return ErrorSeverity.Nhe;
        }

        #endregion

        #region Obsolete Aliases (Backward Compatibility)

        [Obsolete("Use OpenConnection() instead")]
        public bool MoKetNoi() => OpenConnection();

        [Obsolete("Use CloseConnection() instead")]
        public void DongKetNoi() => CloseConnection();

        [Obsolete("Use GetConnection() instead")]
        public SqlConnection LayKetNoi() => GetConnection();

        [Obsolete("Use IsOpen() instead")]
        public bool KiemTraKetNoi() => IsOpen();

        [Obsolete("Use IsHealthy() instead")]
        public bool KiemTraHoatDong() => IsHealthy();

        [Obsolete("Use CreateCommand(string) instead")]
        public SqlCommand TaoCommand(string sql) => CreateCommand(sql);

        [Obsolete("Use CreateStoredProcedureCommand(string) instead")]
        public SqlCommand TaoStoredProcedureCommand(string storedProcedureName) => CreateStoredProcedureCommand(storedProcedureName);

        [Obsolete("Use TestConnection() instead")]
        public bool TestKetNoi() => TestConnection();

        [Obsolete("Use ResetConnection() instead")]
        public void ResetKetNoi() => ResetConnection();

        [Obsolete("Use SetConnectionString(string) instead")]
        public void ThietLapConnectionString(string connectionString) => SetConnectionString(connectionString);

        #endregion

        #region Dispose Pattern

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
            if (!_disposed && disposing)
            {
                try
                {
                    if (_connection != null)
                    {
                        _connection.StateChange -= OnConnectionStateChange;
                        CloseConnection();
                        _connection.Dispose();
                        _connection = null;
                    }
                }
                catch (Exception ex)
                {
                    // Log error nhưng không throw exception trong Dispose
                    OnConnectionError(ex);
                }
                finally
                {
                    _disposed = true;
                }
            }
        }

        /// <summary>
        /// Finalizer
        /// </summary>
        ~ConnectionManager()
        {
            Dispose(false);
        }

        #endregion
    }
}
