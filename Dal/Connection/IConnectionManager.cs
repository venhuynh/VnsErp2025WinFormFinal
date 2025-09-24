using System;
using System.Data;
using System.Data.SqlClient;

namespace Dal.Connection
{
    /// <summary>
    /// Interface quản lý kết nối cơ sở dữ liệu
    /// </summary>
    public interface IConnectionManager : IDisposable
    {
        #region thuocTinhDonGian

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

        #region phuongThuc

        /// <summary>
        /// Mở kết nối database
        /// </summary>
        /// <returns>True nếu mở thành công</returns>
        bool MoKetNoi();

        /// <summary>
        /// Đóng kết nối database
        /// </summary>
        void DongKetNoi();

        /// <summary>
        /// Lấy SqlConnection object
        /// </summary>
        /// <returns>SqlConnection object</returns>
        SqlConnection LayKetNoi();

        /// <summary>
        /// Kiểm tra kết nối có đang mở không
        /// </summary>
        /// <returns>True nếu kết nối đang mở</returns>
        bool KiemTraKetNoi();

        /// <summary>
        /// Kiểm tra kết nối có hoạt động không
        /// </summary>
        /// <returns>True nếu kết nối hoạt động bình thường</returns>
        bool KiemTraHoatDong();

        /// <summary>
        /// Tạo SqlCommand với connection hiện tại
        /// </summary>
        /// <param name="sql">Câu lệnh SQL</param>
        /// <returns>SqlCommand object</returns>
        SqlCommand TaoCommand(string sql);

        /// <summary>
        /// Tạo SqlCommand với stored procedure
        /// </summary>
        /// <param name="storedProcedureName">Tên stored procedure</param>
        /// <returns>SqlCommand object</returns>
        SqlCommand TaoStoredProcedureCommand(string storedProcedureName);

        /// <summary>
        /// Thực hiện test kết nối
        /// </summary>
        /// <returns>True nếu kết nối thành công</returns>
        bool TestKetNoi();

        /// <summary>
        /// Reset kết nối
        /// </summary>
        void ResetKetNoi();

        /// <summary>
        /// Thiết lập connection string mới
        /// </summary>
        /// <param name="connectionString">Chuỗi kết nối mới</param>
        void ThietLapConnectionString(string connectionString);

        #endregion

        #region suKien

        /// <summary>
        /// Sự kiện khi kết nối được mở
        /// </summary>
        event EventHandler<ConnectionEventArgs> KetNoiMo;

        /// <summary>
        /// Sự kiện khi kết nối bị đóng
        /// </summary>
        event EventHandler<ConnectionEventArgs> KetNoiDong;

        /// <summary>
        /// Sự kiện khi có lỗi kết nối
        /// </summary>
        event EventHandler<ConnectionErrorEventArgs> LoiKetNoi;

        #endregion
    }

    /// <summary>
    /// Event arguments cho connection events
    /// </summary>
    public class ConnectionEventArgs : EventArgs
    {
        #region thuocTinhDonGian

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

        #region phuongThuc

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
        #region thuocTinhDonGian

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

        #region phuongThuc

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
