using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Common.Utils
{
    /// <summary>
    /// Utility class for working with Enums - Enhanced version
    /// </summary>
    public static class ApplicationEnumUtils
    {
        /// <summary>
        /// Đưa vào một int trả về giá trị tương ứng
        /// </summary>
        /// <typeparam name="TEnum">Enum type</typeparam>
        /// <param name="intValue">Integer value</param>
        /// <returns>Enum value</returns>
        public static TEnum GetEnumValue<TEnum>(int intValue) where TEnum : Enum
        {
            return (TEnum)Enum.ToObject(typeof(TEnum), intValue);
        }

        /// <summary>
        /// Lấy giá trị enum từ index
        /// </summary>
        /// <param name="enumObj">Enum object</param>
        /// <param name="index">Index</param>
        /// <returns>Enum value</returns>
        public static object GetEnumValueFromIndex(Enum enumObj, int index)
        {
            Array enumValues = Enum.GetValues(enumObj.GetType());

            if (index < 0 || index >= enumValues.Length)
            {
                return null;
            }

            return enumValues.GetValue(index);
        }

        /// <summary>
        /// Parse string to enum value
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="value">String value</param>
        /// <returns>Enum value</returns>
        public static T Parse<T>(string value) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException($"Lỗi không thể chuyển đổi {value} sang kiểu {typeof(T)}");
            }

            try
            {
                return (T)Enum.Parse(typeof(T), value);
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// Lấy Description attribute của enum value
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="value">Enum value</param>
        /// <returns>Description string</returns>
        public static string GetDescription<T>(T value) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            var field = value.GetType().GetField(value.ToString());
            if (field == null) return value.ToString();

            var attribute = field.GetCustomAttributes(typeof(DescriptionAttribute), false)
                .FirstOrDefault() as DescriptionAttribute;

            return attribute?.Description ?? value.ToString();
        }

        /// <summary>
        /// Lấy DisplayName attribute của enum value
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="value">Enum value</param>
        /// <returns>DisplayName string</returns>
        public static string GetDisplayName<T>(T value) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            var field = value.GetType().GetField(value.ToString());
            if (field == null) return value.ToString();

            var attribute = field.GetCustomAttributes(typeof(DisplayNameAttribute), false)
                .FirstOrDefault() as DisplayNameAttribute;

            return attribute?.DisplayName ?? value.ToString();
        }

        /// <summary>
        /// Lấy giá trị integer của enum
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="value">Enum value</param>
        /// <returns>Integer value</returns>
        public static int GetValue<T>(T value) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            return (int)(IConvertible)value;
        }

        /// <summary>
        /// Lấy tất cả enum values với descriptions
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <returns>Dictionary of enum values and descriptions</returns>
        public static Dictionary<T, string> GetAllEnumDescriptions<T>() where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            var result = new Dictionary<T, string>();
            foreach (T value in Enum.GetValues(typeof(T)))
            {
                result[value] = GetDescription(value);
            }
            return result;
        }

        /// <summary>
        /// Lấy tất cả enum values với display names
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <returns>Dictionary of enum values and display names</returns>
        public static Dictionary<T, string> GetAllEnumDisplayNames<T>() where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            var result = new Dictionary<T, string>();
            foreach (T value in Enum.GetValues(typeof(T)))
            {
                result[value] = GetDisplayName(value);
            }
            return result;
        }

        /// <summary>
        /// Kiểm tra xem enum value có hợp lệ không
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="value">Value to check</param>
        /// <returns>True if valid</returns>
        public static bool IsValidEnumValue<T>(object value) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                return false;
            }

            return Enum.IsDefined(typeof(T), value);
        }

        /// <summary>
        /// Lấy enum value từ string name
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="name">Enum name</param>
        /// <returns>Enum value or null if not found</returns>
        public static T? GetEnumFromName<T>(string name) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                return null;
            }

            if (Enum.TryParse<T>(name, out T result))
            {
                return result;
            }
            return null;
        }
    }
}
