using System;
using System.Data;
using System.Data.SqlClient;

namespace Dal.Connection
{
    /// <summary>
    /// Interface quản lý kết nối cơ sở dữ liệu cho DAL (thread-safe lifecycle, command factory, health checks).
    /// Khớp với API của ConnectionManager.
    /// </summary>
    public interface IConnectionManager : IDisposable
    {
        #region Fields & Properties

        /// <summary>
        /// Chuỗi kết nối hiện tại
        /// </summary>
        string ConnectionString { get; }

        /// <summary>
        /// Trạng thái kết nối
        /// </summary>
        ConnectionState State { get; }

        /// <summary>
        /// Thời gian timeout cho command (giây)
        /// </summary>
        int CommandTimeout { get; set; }

        /// <summary>
        /// Thời gian timeout cho connection (giây)
        /// </summary>
        int ConnectionTimeout { get; set; }

        #endregion

        #region Connection Lifecycle / Factory Methods

        /// <summary>
        /// Mở kết nối database
        /// </summary>
        /// <returns>True nếu mở thành công</returns>
        bool OpenConnection();

        /// <summary>
        /// Đóng kết nối database
        /// </summary>
        void CloseConnection();

        /// <summary>
        /// Lấy SqlConnection object (đảm bảo đã mở)
        /// </summary>
        /// <returns>SqlConnection object</returns>
        SqlConnection GetConnection();

        /// <summary>
        /// Kiểm tra kết nối có đang mở không
        /// </summary>
        /// <returns>True nếu kết nối đang mở</returns>
        bool IsOpen();

        /// <summary>
        /// Kiểm tra kết nối có hoạt động không (truy vấn nhẹ)
        /// </summary>
        /// <returns>True nếu kết nối hoạt động bình thường</returns>
        bool IsHealthy();

        /// <summary>
        /// Tạo SqlCommand với connection hiện tại
        /// </summary>
        /// <param name="sql">Câu lệnh SQL</param>
        /// <returns>SqlCommand object</returns>
        SqlCommand CreateCommand(string sql);

        /// <summary>
        /// Tạo SqlCommand cho stored procedure
        /// </summary>
        /// <param name="storedProcedureName">Tên stored procedure</param>
        /// <returns>SqlCommand object</returns>
        SqlCommand CreateStoredProcedureCommand(string storedProcedureName);

        /// <summary>
        /// Thực hiện test kết nối (truy vấn GETDATE())
        /// </summary>
        /// <returns>True nếu kết nối thành công</returns>
        bool TestConnection();

        /// <summary>
        /// Reset kết nối (đóng và khởi tạo lại)
        /// </summary>
        void ResetConnection();

        /// <summary>
        /// Thiết lập connection string mới và khởi tạo lại kết nối
        /// </summary>
        /// <param name="connectionString">Chuỗi kết nối mới</param>
        void SetConnectionString(string connectionString);

        #endregion

        #region Events

        /// <summary>
        /// Sự kiện khi kết nối được mở
        /// </summary>
        event EventHandler<ConnectionEventArgs> ConnectionOpened;

        /// <summary>
        /// Sự kiện khi kết nối bị đóng
        /// </summary>
        event EventHandler<ConnectionEventArgs> ConnectionClosed;

        /// <summary>
        /// Sự kiện khi có lỗi kết nối
        /// </summary>
        event EventHandler<ConnectionErrorEventArgs> ConnectionError;

        #endregion
    }

    /// <summary>
    /// Event arguments cho connection events
    /// </summary>
    public class ConnectionEventArgs : EventArgs
    {
        #region Fields & Properties

        /// <summary>
        /// Thời gian xảy ra sự kiện
        /// </summary>
        public DateTime ThoiGian { get; set; }

        /// <summary>
        /// Connection string được sử dụng
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Thông tin bổ sung
        /// </summary>
        public string ThongTinBoSung { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor mặc định
        /// </summary>
        public ConnectionEventArgs()
        {
            ThoiGian = DateTime.Now;
        }

        /// <summary>
        /// Constructor với tham số
        /// </summary>
        /// <param name="connectionString">Connection string</param>
        /// <param name="thongTinBoSung">Thông tin bổ sung</param>
        public ConnectionEventArgs(string connectionString, string thongTinBoSung = null)
        {
            ThoiGian = DateTime.Now;
            ConnectionString = connectionString;
            ThongTinBoSung = thongTinBoSung;
        }

        #endregion
    }

    /// <summary>
    /// Event arguments cho connection error events
    /// </summary>
    public class ConnectionErrorEventArgs : ConnectionEventArgs
    {
        #region Fields & Properties

        /// <summary>
        /// Exception xảy ra
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// Mô tả lỗi
        /// </summary>
        public string MoTaLoi { get; set; }

        /// <summary>
        /// Mức độ nghiêm trọng của lỗi
        /// </summary>
        public ErrorSeverity MucDoNghiemTrong { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor mặc định
        /// </summary>
        public ConnectionErrorEventArgs()
        {
        }

        /// <summary>
        /// Constructor với tham số
        /// </summary>
        /// <param name="exception">Exception xảy ra</param>
        /// <param name="connectionString">Connection string</param>
        /// <param name="mucDoNghiemTrong">Mức độ nghiêm trọng</param>
        public ConnectionErrorEventArgs(Exception exception, string connectionString, ErrorSeverity mucDoNghiemTrong = ErrorSeverity.TrungBinh)
            : base(connectionString)
        {
            Exception = exception;
            MoTaLoi = exception?.Message;
            MucDoNghiemTrong = mucDoNghiemTrong;
        }

        #endregion
    }

    /// <summary>
    /// Enum mức độ nghiêm trọng của lỗi
    /// </summary>
    public enum ErrorSeverity
    {
        /// <summary>
        /// Lỗi nhẹ
        /// </summary>
        Nhe = 1,

        /// <summary>
        /// Lỗi trung bình
        /// </summary>
        TrungBinh = 2,

        /// <summary>
        /// Lỗi nghiêm trọng
        /// </summary>
        NghiemTrong = 3,

        /// <summary>
        /// Lỗi cực kỳ nghiêm trọng
        /// </summary>
        CucKyNghiemTrong = 4
    }
}
