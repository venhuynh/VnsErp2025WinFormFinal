using System;
using System.Runtime.Serialization;

namespace Dal.Exceptions
{
    /// <summary>
    /// Exception cho các lỗi liên quan đến kết nối database
    /// </summary>
    [Serializable]
    public class ConnectionException : DataAccessException
    {
        #region thuocTinhDonGian

        /// <summary>
        /// Connection string gây ra lỗi
        /// </summary>
        public string ConnectionString { get; }

        /// <summary>
        /// Loại lỗi kết nối
        /// </summary>
        public ConnectionErrorType LoaiLoi { get; }

        /// <summary>
        /// Mã lỗi SQL Server (nếu có)
        /// </summary>
        public new int? SqlErrorNumber { get; set; }

        /// <summary>
        /// Thời gian xảy ra lỗi
        /// </summary>
        public new DateTime ThoiGianLoi { get; }

        #endregion

        #region phuongThuc

        /// <summary>
        /// Constructor mặc định
        /// </summary>
        public ConnectionException() : base()
        {
            ThoiGianLoi = DateTime.Now;
        }

        /// <summary>
        /// Constructor với message
        /// </summary>
        /// <param name="message">Thông điệp lỗi</param>
        public ConnectionException(string message) : base(message)
        {
            ThoiGianLoi = DateTime.Now;
        }

        /// <summary>
        /// Constructor với message và inner exception
        /// </summary>
        /// <param name="message">Thông điệp lỗi</param>
        /// <param name="innerException">Inner exception</param>
        public ConnectionException(string message, Exception innerException) 
            : base(message, innerException)
        {
            ThoiGianLoi = DateTime.Now;
        }

        /// <summary>
        /// Constructor với message, inner exception và connection string
        /// </summary>
        /// <param name="message">Thông điệp lỗi</param>
        /// <param name="innerException">Inner exception</param>
        /// <param name="connectionString">Connection string</param>
        public ConnectionException(string message, Exception innerException, string connectionString) 
            : base(message, innerException)
        {
            ConnectionString = connectionString;
            ThoiGianLoi = DateTime.Now;
        }

        /// <summary>
        /// Constructor với message, inner exception, connection string và loại lỗi
        /// </summary>
        /// <param name="message">Thông điệp lỗi</param>
        /// <param name="innerException">Inner exception</param>
        /// <param name="connectionString">Connection string</param>
        /// <param name="loaiLoi">Loại lỗi kết nối</param>
        public ConnectionException(string message, Exception innerException, string connectionString, ConnectionErrorType loaiLoi) 
            : base(message, innerException)
        {
            ConnectionString = connectionString;
            LoaiLoi = loaiLoi;
            ThoiGianLoi = DateTime.Now;
        }

        /// <summary>
        /// Constructor cho serialization
        /// </summary>
        /// <param name="info">Serialization info</param>
        /// <param name="context">Streaming context</param>
        protected ConnectionException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
            ConnectionString = info.GetString(nameof(ConnectionString));
            LoaiLoi = (ConnectionErrorType)info.GetValue(nameof(LoaiLoi), typeof(ConnectionErrorType));
            SqlErrorNumber = (int?)info.GetValue(nameof(SqlErrorNumber), typeof(int?));
            ThoiGianLoi = info.GetDateTime(nameof(ThoiGianLoi));
        }

        /// <summary>
        /// Lấy thông tin cho serialization
        /// </summary>
        /// <param name="info">Serialization info</param>
        /// <param name="context">Streaming context</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(ConnectionString), ConnectionString);
            info.AddValue(nameof(LoaiLoi), LoaiLoi);
            info.AddValue(nameof(SqlErrorNumber), SqlErrorNumber);
            info.AddValue(nameof(ThoiGianLoi), ThoiGianLoi);
        }

        /// <summary>
        /// Tạo ConnectionException từ SqlException
        /// </summary>
        /// <param name="sqlException">SqlException</param>
        /// <param name="connectionString">Connection string</param>
        /// <returns>ConnectionException</returns>
        public static ConnectionException TaoTuSqlException(System.Data.SqlClient.SqlException sqlException, string connectionString = null)
        {
            var loaiLoi = XacDinhLoaiLoi(sqlException);
            var message = TaoThongDiepLoi(sqlException, loaiLoi);
            
            return new ConnectionException(message, sqlException, connectionString, loaiLoi)
            {
                SqlErrorNumber = sqlException.Number
            };
        }

        /// <summary>
        /// Xác định loại lỗi từ SqlException
        /// </summary>
        /// <param name="sqlException">SqlException</param>
        /// <returns>Loại lỗi kết nối</returns>
        private static ConnectionErrorType XacDinhLoaiLoi(System.Data.SqlClient.SqlException sqlException)
        {
            switch (sqlException.Number)
            {
                case 2:
                case -2:
                    return ConnectionErrorType.Timeout;
                case 18456:
                    return ConnectionErrorType.AuthenticationFailed;
                case 53:
                case 233:
                    return ConnectionErrorType.NetworkError;
                case 10928:
                case 10929:
                    return ConnectionErrorType.ServerTooBusy;
                case 40197:
                case 40501:
                    return ConnectionErrorType.ServerUnavailable;
                case 18470:
                    return ConnectionErrorType.DatabaseUnavailable;
                default:
                    return ConnectionErrorType.Unknown;
            }
        }

        /// <summary>
        /// Tạo thông điệp lỗi thân thiện
        /// </summary>
        /// <param name="sqlException">SqlException</param>
        /// <param name="loaiLoi">Loại lỗi</param>
        /// <returns>Thông điệp lỗi</returns>
        private static string TaoThongDiepLoi(System.Data.SqlClient.SqlException sqlException, ConnectionErrorType loaiLoi)
        {
            switch (loaiLoi)
            {
                case ConnectionErrorType.Timeout:
                    return "Kết nối database bị timeout. Vui lòng thử lại sau.";
                case ConnectionErrorType.AuthenticationFailed:
                    return "Không thể xác thực với database. Vui lòng kiểm tra thông tin đăng nhập.";
                case ConnectionErrorType.NetworkError:
                    return "Không thể kết nối đến database. Vui lòng kiểm tra kết nối mạng.";
                case ConnectionErrorType.ServerTooBusy:
                    return "Server database đang quá tải. Vui lòng thử lại sau.";
                case ConnectionErrorType.ServerUnavailable:
                    return "Server database hiện không khả dụng. Vui lòng thử lại sau.";
                case ConnectionErrorType.DatabaseUnavailable:
                    return "Database hiện không khả dụng. Vui lòng thử lại sau.";
                default:
                    return $"Lỗi kết nối database: {sqlException.Message}";
            }
        }

        /// <summary>
        /// Kiểm tra lỗi có thể retry không
        /// </summary>
        /// <returns>True nếu có thể retry</returns>
        public bool CoTheRetry()
        {
            return LoaiLoi == ConnectionErrorType.Timeout ||
                   LoaiLoi == ConnectionErrorType.NetworkError ||
                   LoaiLoi == ConnectionErrorType.ServerTooBusy ||
                   LoaiLoi == ConnectionErrorType.ServerUnavailable;
        }

        /// <summary>
        /// Lấy thông tin chi tiết lỗi
        /// </summary>
        /// <returns>Thông tin chi tiết</returns>
        public override string ToString()
        {
            var result = base.ToString();
            
            if (!string.IsNullOrEmpty(ConnectionString))
            {
                result += $"\nConnection String: {ConnectionString}";
            }
            
            if (LoaiLoi != ConnectionErrorType.Unknown)
            {
                result += $"\nLoại lỗi: {LoaiLoi}";
            }
            
            if (SqlErrorNumber.HasValue)
            {
                result += $"\nMã lỗi SQL: {SqlErrorNumber.Value}";
            }
            
            result += $"\nThời gian: {ThoiGianLoi:yyyy-MM-dd HH:mm:ss}";
            
            return result;
        }

        #endregion
    }

    /// <summary>
    /// Enum các loại lỗi kết nối
    /// </summary>
    public enum ConnectionErrorType
    {
        /// <summary>
        /// Lỗi không xác định
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Lỗi timeout
        /// </summary>
        Timeout = 1,

        /// <summary>
        /// Lỗi xác thực
        /// </summary>
        AuthenticationFailed = 2,

        /// <summary>
        /// Lỗi mạng
        /// </summary>
        NetworkError = 3,

        /// <summary>
        /// Server quá tải
        /// </summary>
        ServerTooBusy = 4,

        /// <summary>
        /// Server không khả dụng
        /// </summary>
        ServerUnavailable = 5,

        /// <summary>
        /// Database không khả dụng
        /// </summary>
        DatabaseUnavailable = 6,

        /// <summary>
        /// Lỗi cấu hình
        /// </summary>
        ConfigurationError = 7,

        /// <summary>
        /// Lỗi connection string
        /// </summary>
        InvalidConnectionString = 8
    }
}
