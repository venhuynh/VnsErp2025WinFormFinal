using System;
using System.Runtime.Serialization;

namespace Dal.Exceptions
{
    /// <summary>
    /// Base exception cho tất cả các lỗi liên quan đến Data Access
    /// </summary>
    [Serializable]
    public class DataAccessException : Exception
    {
        #region thuocTinhDonGian

        /// <summary>
        /// Thời gian xảy ra lỗi
        /// </summary>
        public DateTime ThoiGianLoi { get; }

        /// <summary>
        /// Mô tả chi tiết lỗi
        /// </summary>
        public string MoTaChiTiet { get; set; }

        /// <summary>
        /// Context của lỗi (tên method, class...)
        /// </summary>
        public string Context { get; set; }

        #endregion

        #region phuongThuc

        /// <summary>
        /// Constructor mặc định
        /// </summary>
        public DataAccessException() : base()
        {
            ThoiGianLoi = DateTime.Now;
        }

        /// <summary>
        /// Constructor với message
        /// </summary>
        /// <param name="message">Thông điệp lỗi</param>
        public DataAccessException(string message) : base(message)
        {
            ThoiGianLoi = DateTime.Now;
        }

        /// <summary>
        /// Constructor với message và inner exception
        /// </summary>
        /// <param name="message">Thông điệp lỗi</param>
        /// <param name="innerException">Inner exception</param>
        public DataAccessException(string message, Exception innerException) 
            : base(message, innerException)
        {
            ThoiGianLoi = DateTime.Now;
        }

        /// <summary>
        /// Constructor với message, inner exception và context
        /// </summary>
        /// <param name="message">Thông điệp lỗi</param>
        /// <param name="innerException">Inner exception</param>
        /// <param name="context">Context của lỗi</param>
        public DataAccessException(string message, Exception innerException, string context) 
            : base(message, innerException)
        {
            Context = context;
            ThoiGianLoi = DateTime.Now;
        }

        /// <summary>
        /// Constructor cho serialization
        /// </summary>
        /// <param name="info">Serialization info</param>
        /// <param name="context">Streaming context</param>
        protected DataAccessException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
            ThoiGianLoi = info.GetDateTime(nameof(ThoiGianLoi));
            MoTaChiTiet = info.GetString(nameof(MoTaChiTiet));
            Context = info.GetString(nameof(Context));
        }

        /// <summary>
        /// Lấy thông tin cho serialization
        /// </summary>
        /// <param name="info">Serialization info</param>
        /// <param name="context">Streaming context</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(ThoiGianLoi), ThoiGianLoi);
            info.AddValue(nameof(MoTaChiTiet), MoTaChiTiet);
            info.AddValue(nameof(Context), Context);
        }

        #endregion
    }
}
