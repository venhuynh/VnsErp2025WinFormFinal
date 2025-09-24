using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using Dal.Exceptions;
using Dal.Helpers;

namespace Dal.Connection
{
    /// <summary>
    /// Class quản lý kết nối cơ sở dữ liệu
    /// </summary>
    public class ConnectionManager : IConnectionManager
    {
        #region thuocTinhDonGian

        private SqlConnection _connection;
        private readonly object _lockObject = new object();
        private bool _disposed = false;

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

        #region suKien

        /// <summary>
        /// Sự kiện khi kết nối được mở
        /// </summary>
        public event EventHandler<ConnectionEventArgs> KetNoiMo;

        /// <summary>
        /// Sự kiện khi kết nối bị đóng
        /// </summary>
        public event EventHandler<ConnectionEventArgs> KetNoiDong;

        /// <summary>
        /// Sự kiện khi có lỗi kết nối
        /// </summary>
        public event EventHandler<ConnectionErrorEventArgs> LoiKetNoi;

        #endregion

        #region phuongThuc

        /// <summary>
        /// Constructor mặc định
        /// </summary>
        public ConnectionManager()
        {
            KhoiTaoKetNoi();
        }

        /// <summary>
        /// Constructor với connection string
        /// </summary>
        /// <param name="connectionString">Chuỗi kết nối</param>
        public ConnectionManager(string connectionString)
        {
            ConnectionString = connectionString;
            KhoiTaoKetNoi();
        }

        /// <summary>
        /// Khởi tạo kết nối
        /// </summary>
        private void KhoiTaoKetNoi()
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
                OnConnectionError(ex, "Lỗi khởi tạo kết nối");
                throw new ConnectionException("Không thể khởi tạo kết nối database", ex);
            }
        }

        /// <summary>
        /// Mở kết nối database
        /// </summary>
        /// <returns>True nếu mở thành công</returns>
        public bool MoKetNoi()
        {
            lock (_lockObject)
            {
                try
                {
                    if (_connection == null)
                    {
                        KhoiTaoKetNoi();
                    }

                    if (_connection.State == ConnectionState.Closed)
                    {
                        _connection.Open();
                        OnConnectionOpened();
                        return true;
                    }

                    return _connection.State == ConnectionState.Open;
                }
                catch (Exception ex)
                {
                    OnConnectionError(ex, "Lỗi mở kết nối database");
                    throw new ConnectionException("Không thể mở kết nối database", ex);
                }
            }
        }

        /// <summary>
        /// Đóng kết nối database
        /// </summary>
        public void DongKetNoi()
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
                    OnConnectionError(ex, "Lỗi đóng kết nối database");
                    throw new ConnectionException("Không thể đóng kết nối database", ex);
                }
            }
        }

        /// <summary>
        /// Lấy SqlConnection object
        /// </summary>
        /// <returns>SqlConnection object</returns>
        public SqlConnection LayKetNoi()
        {
            if (_connection == null)
            {
                KhoiTaoKetNoi();
            }

            if (_connection.State != ConnectionState.Open)
            {
                MoKetNoi();
            }

            return _connection;
        }

        /// <summary>
        /// Kiểm tra kết nối có đang mở không
        /// </summary>
        /// <returns>True nếu kết nối đang mở</returns>
        public bool KiemTraKetNoi()
        {
            return _connection?.State == ConnectionState.Open;
        }

        /// <summary>
        /// Kiểm tra kết nối có hoạt động không
        /// </summary>
        /// <returns>True nếu kết nối hoạt động bình thường</returns>
        public bool KiemTraHoatDong()
        {
            try
            {
                if (!KiemTraKetNoi())
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
        /// Tạo SqlCommand với connection hiện tại
        /// </summary>
        /// <param name="sql">Câu lệnh SQL</param>
        /// <returns>SqlCommand object</returns>
        public SqlCommand TaoCommand(string sql)
        {
            var connection = LayKetNoi();
            var command = new SqlCommand(sql, connection)
            {
                CommandTimeout = CommandTimeout
            };
            return command;
        }

        /// <summary>
        /// Tạo SqlCommand với stored procedure
        /// </summary>
        /// <param name="storedProcedureName">Tên stored procedure</param>
        /// <returns>SqlCommand object</returns>
        public SqlCommand TaoStoredProcedureCommand(string storedProcedureName)
        {
            var connection = LayKetNoi();
            var command = new SqlCommand(storedProcedureName, connection)
            {
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = CommandTimeout
            };
            return command;
        }

        /// <summary>
        /// Thực hiện test kết nối
        /// </summary>
        /// <returns>True nếu kết nối thành công</returns>
        public bool TestKetNoi()
        {
            try
            {
                var connection = LayKetNoi();
                using (var command = new SqlCommand("SELECT GETDATE()", connection))
                {
                    command.CommandTimeout = 10;
                    var result = command.ExecuteScalar();
                    return result != null;
                }
            }
            catch (Exception ex)
            {
                OnConnectionError(ex, "Test kết nối thất bại");
                return false;
            }
        }

        /// <summary>
        /// Reset kết nối
        /// </summary>
        public void ResetKetNoi()
        {
            lock (_lockObject)
            {
                try
                {
                    DongKetNoi();
                    Thread.Sleep(100); // Chờ một chút trước khi tạo lại
                    KhoiTaoKetNoi();
                }
                catch (Exception ex)
                {
                    OnConnectionError(ex, "Lỗi reset kết nối");
                    throw new ConnectionException("Không thể reset kết nối", ex);
                }
            }
        }

        /// <summary>
        /// Thiết lập connection string mới
        /// </summary>
        /// <param name="connectionString">Chuỗi kết nối mới</param>
        public void ThietLapConnectionString(string connectionString)
        {
            lock (_lockObject)
            {
                try
                {
                    DongKetNoi();
                    ConnectionString = connectionString;
                    KhoiTaoKetNoi();
                }
                catch (Exception ex)
                {
                    OnConnectionError(ex, "Lỗi thiết lập connection string mới");
                    throw new ConnectionException("Không thể thiết lập connection string mới", ex);
                }
            }
        }

        /// <summary>
        /// Xử lý sự kiện thay đổi trạng thái connection
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">State change event args</param>
        private void OnConnectionStateChange(object sender, StateChangeEventArgs e)
        {
            // Có thể thêm logic xử lý khi trạng thái connection thay đổi
            var currentState = e.CurrentState.ToString();
            var previousState = e.OriginalState.ToString();
        }

        /// <summary>
        /// Xử lý sự kiện kết nối được mở
        /// </summary>
        private void OnConnectionOpened()
        {
            KetNoiMo?.Invoke(this, new ConnectionEventArgs(ConnectionString, "Kết nối database đã được mở"));
        }

        /// <summary>
        /// Xử lý sự kiện kết nối bị đóng
        /// </summary>
        private void OnConnectionClosed()
        {
            KetNoiDong?.Invoke(this, new ConnectionEventArgs(ConnectionString, "Kết nối database đã được đóng"));
        }

        /// <summary>
        /// Xử lý sự kiện lỗi kết nối
        /// </summary>
        /// <param name="exception">Exception xảy ra</param>
        /// <param name="message">Thông điệp lỗi</param>
        private void OnConnectionError(Exception exception, string message)
        {
            var severity = XacDinhMucDoLoi(exception);
            LoiKetNoi?.Invoke(this, new ConnectionErrorEventArgs(exception, ConnectionString, severity));
        }

        /// <summary>
        /// Xác định mức độ nghiêm trọng của lỗi
        /// </summary>
        /// <param name="exception">Exception</param>
        /// <returns>Mức độ nghiêm trọng</returns>
        private ErrorSeverity XacDinhMucDoLoi(Exception exception)
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

        #region giaiPhongTaiNguyen

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
                        DongKetNoi();
                        _connection.Dispose();
                        _connection = null;
                    }
                }
                catch (Exception ex)
                {
                    // Log error nhưng không throw exception trong Dispose
                    OnConnectionError(ex, "Lỗi trong quá trình dispose connection");
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
