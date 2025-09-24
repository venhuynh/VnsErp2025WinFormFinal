using System;
using System.Runtime.Serialization;

namespace Dal.Exceptions
{
    /// <summary>
    /// Ngoại lệ cho các lỗi liên quan đến kết nối database.
    /// Vai trò: Chuẩn hóa thông tin lỗi (mã lỗi SQL, loại lỗi, chuỗi kết nối, thời điểm),
    /// và cung cấp helper để tạo exception từ SqlException.
    /// </summary>
    [Serializable]
    public class ConnectionException : DataAccessException
    {
        #region Fields & Properties

        /// <summary>
        /// Connection string gây ra lỗi.
        /// </summary>
        public string ConnectionString { get; }

        /// <summary>
        /// Loại lỗi kết nối (tiếng Việt - tương thích cũ).
        /// </summary>
        public ConnectionErrorType LoaiLoi { get; }

        /// <summary>
        /// Mã lỗi SQL Server (nếu có) - shadow thuộc tính từ lớp cha để lưu số lỗi SQL.
        /// </summary>
        public new int? SqlErrorNumber { get; set; }

        /// <summary>
        /// Thời gian xảy ra lỗi (tiếng Việt - tương thích cũ).
        /// </summary>
        public new DateTime ErrorTime { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Khởi tạo mặc định.
        /// </summary>
        public ConnectionException() : base()
        {
            ErrorTime = DateTime.Now;
        }

        /// <summary>
        /// Khởi tạo với thông điệp lỗi.
        /// </summary>
        public ConnectionException(string message) : base(message)
        {
            ErrorTime = DateTime.Now;
        }

        /// <summary>
        /// Khởi tạo với thông điệp lỗi và inner exception.
        /// </summary>
        public ConnectionException(string message, Exception innerException)
            : base(message, innerException)
        {
            ErrorTime = DateTime.Now;
        }

        /// <summary>
        /// Khởi tạo đầy đủ: thông điệp, inner, connection string.
        /// </summary>
        public ConnectionException(string message, Exception innerException, string connectionString)
            : base(message, innerException)
        {
            ConnectionString = connectionString;
            ErrorTime = DateTime.Now;
        }

        /// <summary>
        /// Khởi tạo đầy đủ: thông điệp, inner, connection string và loại lỗi.
        /// </summary>
        public ConnectionException(string message, Exception innerException, string connectionString, ConnectionErrorType loaiLoi)
            : base(message, innerException)
        {
            ConnectionString = connectionString;
            LoaiLoi = loaiLoi;
            ErrorTime = DateTime.Now;
        }

        /// <summary>
        /// Khởi tạo cho serialization.
        /// </summary>
        protected ConnectionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            ConnectionString = info.GetString(nameof(ConnectionString));
            LoaiLoi = (ConnectionErrorType)info.GetValue(nameof(LoaiLoi), typeof(ConnectionErrorType));
            SqlErrorNumber = (int?)info.GetValue(nameof(SqlErrorNumber), typeof(int?));
            ErrorTime = info.GetDateTime(nameof(ErrorTime));
        }

        #endregion

        #region Factory Methods

        /// <summary>
        /// Tạo ConnectionException từ SqlException (API mới).
        /// </summary>
        public static ConnectionException FromSqlException(System.Data.SqlClient.SqlException sqlException, string connectionString = null)
        {
            var loaiLoi = DetermineErrorType(sqlException);
            var message = BuildErrorMessage(sqlException, loaiLoi);

            return new ConnectionException(message, sqlException, connectionString, loaiLoi)
            {
                SqlErrorNumber = sqlException.Number
            };
        }

        /// <summary>
        /// Tạo ConnectionException từ SqlException (tương thích cũ).
        /// </summary>
        [Obsolete("Use FromSqlException(SqlException,string) instead")]
        public static ConnectionException TaoTuSqlException(System.Data.SqlClient.SqlException sqlException, string connectionString = null)
        {
            return FromSqlException(sqlException, connectionString);
        }

        #endregion

        #region Validation & Utilities

        /// <summary>
        /// Xác định loại lỗi từ SqlException (API mới).
        /// </summary>
        private static ConnectionErrorType DetermineErrorType(System.Data.SqlClient.SqlException sqlException)
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
        /// Xác định loại lỗi (tương thích cũ).
        /// </summary>
        [Obsolete("Use DetermineErrorType(SqlException) instead")]
        private static ConnectionErrorType XacDinhLoaiLoi(System.Data.SqlClient.SqlException sqlException)
        {
            return DetermineErrorType(sqlException);
        }

        /// <summary>
        /// Tạo thông điệp lỗi thân thiện (API mới).
        /// </summary>
        private static string BuildErrorMessage(System.Data.SqlClient.SqlException sqlException, ConnectionErrorType loaiLoi)
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
        /// Tạo thông điệp lỗi (tương thích cũ).
        /// </summary>
        [Obsolete("Use BuildErrorMessage(SqlException,ConnectionErrorType) instead")]
        private static string TaoThongDiepLoi(System.Data.SqlClient.SqlException sqlException, ConnectionErrorType loaiLoi)
        {
            return BuildErrorMessage(sqlException, loaiLoi);
        }

        /// <summary>
        /// Kiểm tra lỗi có thể retry không (API mới).
        /// </summary>
        public bool CanRetry()
        {
            return LoaiLoi == ConnectionErrorType.Timeout ||
                   LoaiLoi == ConnectionErrorType.NetworkError ||
                   LoaiLoi == ConnectionErrorType.ServerTooBusy ||
                   LoaiLoi == ConnectionErrorType.ServerUnavailable;
        }

        /// <summary>
        /// Kiểm tra lỗi có thể retry không (tương thích cũ).
        /// </summary>
        [Obsolete("Use CanRetry() instead")]
        public bool CoTheRetry()
        {
            return CanRetry();
        }

        #endregion

        #region Error Handling

        /// <summary>
        /// Ghi thông tin phục vụ serialization.
        /// </summary>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(ConnectionString), ConnectionString);
            info.AddValue(nameof(LoaiLoi), LoaiLoi);
            info.AddValue(nameof(SqlErrorNumber), SqlErrorNumber);
            info.AddValue(nameof(ErrorTime), ErrorTime);
        }

        /// <summary>
        /// Chuỗi mô tả chi tiết lỗi.
        /// </summary>
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
            
            result += $"\nThời gian: {ErrorTime:yyyy-MM-dd HH:mm:ss}";
            
            return result;
        }

        #endregion
    }

    /// <summary>
    /// Enum các loại lỗi kết nối.
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
