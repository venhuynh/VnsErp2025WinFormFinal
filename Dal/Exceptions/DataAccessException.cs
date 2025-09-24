using System;
using System.Runtime.Serialization;

namespace Dal.Exceptions
{
    /// <summary>
    /// Ngoại lệ cơ sở cho tất cả lỗi liên quan đến Data Access (DAL).
    /// - Chuẩn hóa thông tin lỗi: thời gian, mô tả chi tiết, ngữ cảnh, mã lỗi SQL.
    /// - Dùng làm lớp nền cho các exception cụ thể (ví dụ: ConnectionException).
    /// </summary>
    [Serializable]
    public class DataAccessException : Exception
    {
        #region Fields & Properties

        /// <summary>
        /// Thời gian xảy ra lỗi (tiếng Việt - tương thích cũ).
        /// </summary>
        public DateTime ThoiGianLoi { get; set; }

        /// <summary>
        /// Thời gian xảy ra lỗi (API mới, tiếng Anh; proxy sang <see cref="ThoiGianLoi"/>).
        /// </summary>
        public DateTime ErrorTime
        {
            get => ThoiGianLoi;
            set => ThoiGianLoi = value;
        }

        /// <summary>
        /// Mô tả chi tiết lỗi (tiếng Việt - tương thích cũ).
        /// </summary>
        public string MoTaChiTiet { get; set; }

        /// <summary>
        /// Mô tả chi tiết lỗi (API mới, tiếng Anh; proxy sang <see cref="MoTaChiTiet"/>).
        /// </summary>
        public string DetailedDescription
        {
            get => MoTaChiTiet;
            set => MoTaChiTiet = value;
        }

        /// <summary>
        /// Ngữ cảnh lỗi (tên method/class...).
        /// </summary>
        public string Context { get; set; }

        /// <summary>
        /// Ngữ cảnh lỗi (API mới, tiếng Anh; proxy sang <see cref="Context"/>).
        /// </summary>
        public string ErrorContext
        {
            get => Context;
            set => Context = value;
        }

        /// <summary>
        /// SQL Error Number (nếu có).
        /// </summary>
        public int? SqlErrorNumber { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Khởi tạo mặc định.
        /// </summary>
        public DataAccessException() : base()
        {
            ThoiGianLoi = DateTime.Now;
        }

        /// <summary>
        /// Khởi tạo với thông điệp lỗi.
        /// </summary>
        /// <param name="message">Thông điệp lỗi</param>
        public DataAccessException(string message) : base(message)
        {
            ThoiGianLoi = DateTime.Now;
        }

        /// <summary>
        /// Khởi tạo với thông điệp lỗi và inner exception.
        /// </summary>
        /// <param name="message">Thông điệp lỗi</param>
        /// <param name="innerException">Inner exception</param>
        public DataAccessException(string message, Exception innerException)
            : base(message, innerException)
        {
            ThoiGianLoi = DateTime.Now;
        }

        /// <summary>
        /// Khởi tạo với thông điệp, inner exception và ngữ cảnh lỗi.
        /// </summary>
        /// <param name="message">Thông điệp lỗi</param>
        /// <param name="innerException">Inner exception</param>
        /// <param name="context">Ngữ cảnh lỗi</param>
        public DataAccessException(string message, Exception innerException, string context)
            : base(message, innerException)
        {
            Context = context;
            ThoiGianLoi = DateTime.Now;
        }

        /// <summary>
        /// Khởi tạo cho serialization.
        /// </summary>
        /// <param name="info">Serialization info</param>
        /// <param name="context">Streaming context</param>
        protected DataAccessException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            ThoiGianLoi = info.GetDateTime(nameof(ThoiGianLoi));
            MoTaChiTiet = info.GetString(nameof(MoTaChiTiet));
            Context = info.GetString(nameof(Context));
            SqlErrorNumber = info.GetInt32(nameof(SqlErrorNumber));
        }

        #endregion

        #region Error Handling

        /// <summary>
        /// Ghi thông tin phục vụ serialization.
        /// </summary>
        /// <param name="info">Serialization info</param>
        /// <param name="context">Streaming context</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(ThoiGianLoi), ThoiGianLoi);
            info.AddValue(nameof(MoTaChiTiet), MoTaChiTiet);
            info.AddValue(nameof(Context), Context);
            info.AddValue(nameof(SqlErrorNumber), SqlErrorNumber);
        }

        #endregion
    }
}
