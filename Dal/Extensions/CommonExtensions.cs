using System;
using System.Collections.Generic;
using System.Linq;

namespace Dal.Extensions
{
    /// <summary>
    /// Bộ tiện ích mở rộng (extension methods) dùng chung cho entities và kiểu cơ bản.
    /// Mục tiêu: giảm lặp mã kiểm tra null/empty, chuẩn hóa xử lý chuỗi, ngày giờ, collection.
    /// </summary>
    public static class CommonExtensions
    {
        #region Fields & Properties

        // Không có trường/thuộc tính tĩnh cần khai báo

        #endregion

        #region Validation & Utilities

        /// <summary>
        /// Kiểm tra string có null hoặc empty không.
        /// </summary>
        /// <param name="value">Chuỗi cần kiểm tra</param>
        /// <returns>true nếu null hoặc empty</returns>
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// Kiểm tra string có null, empty hoặc chỉ chứa khoảng trắng không.
        /// </summary>
        /// <param name="value">Chuỗi cần kiểm tra</param>
        /// <returns>true nếu null, empty hoặc whitespace</returns>
        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// Trả về chuỗi an toàn (null thành chuỗi rỗng).
        /// </summary>
        /// <param name="value">Chuỗi cần chuyển</param>
        /// <returns>Chuỗi không null</returns>
        public static string ToSafeString(this string value)
        {
            return value ?? string.Empty;
        }

        /// <summary>
        /// Trim chuỗi và chuyển null thành chuỗi rỗng.
        /// </summary>
        /// <param name="value">Chuỗi cần xử lý</param>
        /// <returns>Chuỗi đã trim, không null</returns>
        public static string TrimSafe(this string value)
        {
            return value?.Trim() ?? string.Empty;
        }

        /// <summary>
        /// Kiểm tra Guid có bằng Guid.Empty không.
        /// </summary>
        /// <param name="guid">Giá trị Guid</param>
        /// <returns>true nếu empty</returns>
        public static bool IsEmpty(this Guid guid)
        {
            return guid == Guid.Empty;
        }

        /// <summary>
        /// Kiểm tra Guid nullable có null hoặc Guid.Empty không.
        /// </summary>
        /// <param name="guid">Giá trị Guid?</param>
        /// <returns>true nếu null hoặc empty</returns>
        public static bool IsNullOrEmpty(this Guid? guid)
        {
            return guid == null || guid == Guid.Empty;
        }

        /// <summary>
        /// Lấy giá trị hoặc mặc định cho kiểu giá trị (struct).
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu</typeparam>
        /// <param name="value">Giá trị nullable</param>
        /// <param name="defaultValue">Giá trị mặc định</param>
        /// <returns>Giá trị thực tế hoặc mặc định</returns>
        public static T ValueOr<T>(this T? value, T defaultValue) where T : struct
        {
            return value ?? defaultValue;
        }

        /// <summary>
        /// Lấy giá trị hoặc mặc định cho reference type.
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu</typeparam>
        /// <param name="value">Giá trị</param>
        /// <param name="defaultValue">Giá trị mặc định</param>
        /// <returns>Giá trị thực tế hoặc mặc định</returns>
        public static T ValueOr<T>(this T value, T defaultValue) where T : class
        {
            return value ?? defaultValue;
        }

        /// <summary>
        /// Kiểm tra collection có null hoặc không có phần tử không.
        /// </summary>
        /// <typeparam name="T">Kiểu phần tử</typeparam>
        /// <param name="collection">Tập hợp cần kiểm tra</param>
        /// <returns>true nếu null hoặc empty</returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
        {
            return collection == null || !collection.Any();
        }

        /// <summary>
        /// Lấy phần tử đầu tiên hoặc trả về giá trị mặc định chỉ định.
        /// </summary>
        /// <typeparam name="T">Kiểu phần tử</typeparam>
        /// <param name="collection">Tập hợp nguồn</param>
        /// <param name="defaultValue">Giá trị mặc định nếu không có phần tử</param>
        /// <returns>Phần tử đầu tiên hoặc defaultValue</returns>
        public static T FirstOrDefault<T>(this IEnumerable<T> collection, T defaultValue)
        {
            if (collection == null)
                return defaultValue;

            var first = collection.FirstOrDefault();
            return EqualityComparer<T>.Default.Equals(first, default(T)) ? defaultValue : first;
        }

        /// <summary>
        /// Định dạng DateTime thành chuỗi theo format chỉ định.
        /// </summary>
        /// <param name="date">Giá trị ngày</param>
        /// <param name="format">Định dạng (mặc định: dd/MM/yyyy)</param>
        /// <returns>Chuỗi đã định dạng</returns>
        public static string ToDateString(this DateTime date, string format = "dd/MM/yyyy")
        {
            return date.ToString(format);
        }

        /// <summary>
        /// Định dạng DateTime? thành chuỗi theo format; null trả về giá trị mặc định.
        /// </summary>
        /// <param name="date">Giá trị ngày (nullable)</param>
        /// <param name="format">Định dạng (mặc định: dd/MM/yyyy)</param>
        /// <param name="nullValue">Giá trị khi null (mặc định: "N/A")</param>
        /// <returns>Chuỗi đã định dạng</returns>
        public static string ToDateString(this DateTime? date, string format = "dd/MM/yyyy", string nullValue = "N/A")
        {
            return date?.ToString(format) ?? nullValue;
        }

        /// <summary>
        /// Kiểm tra DateTime có nằm trong khoảng [startDate, endDate] không.
        /// </summary>
        /// <param name="date">Ngày cần kiểm tra</param>
        /// <param name="startDate">Ngày bắt đầu</param>
        /// <param name="endDate">Ngày kết thúc</param>
        /// <returns>true nếu nằm trong khoảng</returns>
        public static bool IsBetween(this DateTime date, DateTime startDate, DateTime endDate)
        {
            return date >= startDate && date <= endDate;
        }

        /// <summary>
        /// Kiểm tra DateTime? có nằm trong khoảng [startDate, endDate] không.
        /// </summary>
        /// <param name="date">Ngày cần kiểm tra (nullable)</param>
        /// <param name="startDate">Ngày bắt đầu</param>
        /// <param name="endDate">Ngày kết thúc</param>
        /// <returns>true nếu nằm trong khoảng</returns>
        public static bool IsBetween(this DateTime? date, DateTime startDate, DateTime endDate)
        {
            return date.HasValue && date.Value.IsBetween(startDate, endDate);
        }

        #endregion
    }
}
