using System;
using System.Collections.Generic;
using System.Linq;

namespace Dal.Extensions
{
    /// <summary>
    /// Common extension methods cho entities
    /// </summary>
    public static class CommonExtensions
    {
        #region thuocTinhDonGian

        #endregion

        #region phuongThuc

        /// <summary>
        /// Kiểm tra string có null hoặc empty không
        /// </summary>
        /// <param name="value">String cần kiểm tra</param>
        /// <returns>True nếu null hoặc empty</returns>
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// Kiểm tra string có null, empty hoặc chỉ chứa whitespace không
        /// </summary>
        /// <param name="value">String cần kiểm tra</param>
        /// <returns>True nếu null, empty hoặc whitespace</returns>
        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// Chuyển string thành safe string (null thành empty)
        /// </summary>
        /// <param name="value">String cần chuyển</param>
        /// <returns>Safe string</returns>
        public static string ToSafeString(this string value)
        {
            return value ?? string.Empty;
        }

        /// <summary>
        /// Trim string và chuyển null thành empty
        /// </summary>
        /// <param name="value">String cần xử lý</param>
        /// <returns>String đã trim</returns>
        public static string TrimSafe(this string value)
        {
            return value?.Trim() ?? string.Empty;
        }

        /// <summary>
        /// Kiểm tra Guid có empty không
        /// </summary>
        /// <param name="guid">Guid cần kiểm tra</param>
        /// <returns>True nếu empty</returns>
        public static bool IsEmpty(this Guid guid)
        {
            return guid == Guid.Empty;
        }

        /// <summary>
        /// Kiểm tra Guid có empty không (nullable)
        /// </summary>
        /// <param name="guid">Guid nullable cần kiểm tra</param>
        /// <returns>True nếu null hoặc empty</returns>
        public static bool IsNullOrEmpty(this Guid? guid)
        {
            return guid == null || guid == Guid.Empty;
        }

        /// <summary>
        /// Lấy giá trị hoặc default
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu</typeparam>
        /// <param name="value">Giá trị</param>
        /// <param name="defaultValue">Giá trị mặc định</param>
        /// <returns>Giá trị hoặc default</returns>
        public static T ValueOr<T>(this T? value, T defaultValue) where T : struct
        {
            return value ?? defaultValue;
        }

        /// <summary>
        /// Lấy giá trị hoặc default cho reference type
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu</typeparam>
        /// <param name="value">Giá trị</param>
        /// <param name="defaultValue">Giá trị mặc định</param>
        /// <returns>Giá trị hoặc default</returns>
        public static T ValueOr<T>(this T value, T defaultValue) where T : class
        {
            return value ?? defaultValue;
        }

        /// <summary>
        /// Kiểm tra collection có null hoặc empty không
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu</typeparam>
        /// <param name="collection">Collection cần kiểm tra</param>
        /// <returns>True nếu null hoặc empty</returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
        {
            return collection == null || !collection.Any();
        }

        /// <summary>
        /// Lấy phần tử đầu tiên hoặc default
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu</typeparam>
        /// <param name="collection">Collection</param>
        /// <param name="defaultValue">Giá trị mặc định</param>
        /// <returns>Phần tử đầu tiên hoặc default</returns>
        public static T FirstOrDefault<T>(this IEnumerable<T> collection, T defaultValue)
        {
            if (collection == null)
                return defaultValue;

            var first = collection.FirstOrDefault();
            return EqualityComparer<T>.Default.Equals(first, default(T)) ? defaultValue : first;
        }

        /// <summary>
        /// Format date thành string với format mặc định
        /// </summary>
        /// <param name="date">Date cần format</param>
        /// <param name="format">Format (mặc định: dd/MM/yyyy)</param>
        /// <returns>String đã format</returns>
        public static string ToDateString(this DateTime date, string format = "dd/MM/yyyy")
        {
            return date.ToString(format);
        }

        /// <summary>
        /// Format date nullable thành string
        /// </summary>
        /// <param name="date">Date nullable cần format</param>
        /// <param name="format">Format (mặc định: dd/MM/yyyy)</param>
        /// <param name="nullValue">Giá trị hiển thị khi null (mặc định: "N/A")</param>
        /// <returns>String đã format</returns>
        public static string ToDateString(this DateTime? date, string format = "dd/MM/yyyy", string nullValue = "N/A")
        {
            return date?.ToString(format) ?? nullValue;
        }

        /// <summary>
        /// Kiểm tra date có trong khoảng thời gian không
        /// </summary>
        /// <param name="date">Date cần kiểm tra</param>
        /// <param name="startDate">Ngày bắt đầu</param>
        /// <param name="endDate">Ngày kết thúc</param>
        /// <returns>True nếu trong khoảng</returns>
        public static bool IsBetween(this DateTime date, DateTime startDate, DateTime endDate)
        {
            return date >= startDate && date <= endDate;
        }

        /// <summary>
        /// Kiểm tra date nullable có trong khoảng thời gian không
        /// </summary>
        /// <param name="date">Date nullable cần kiểm tra</param>
        /// <param name="startDate">Ngày bắt đầu</param>
        /// <param name="endDate">Ngày kết thúc</param>
        /// <returns>True nếu trong khoảng</returns>
        public static bool IsBetween(this DateTime? date, DateTime startDate, DateTime endDate)
        {
            return date.HasValue && date.Value.IsBetween(startDate, endDate);
        }

        #endregion
    }
}
