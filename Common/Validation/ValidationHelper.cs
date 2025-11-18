using System.Linq;
using System.Text.RegularExpressions;

namespace Common.Validation
{
    /// <summary>
    /// Helper class chứa các phương thức validation có thể tái sử dụng
    /// </summary>
    public static class ValidationHelper
    {
        #region Constants
        /// <summary>
        /// Regex pattern cho email validation
        /// </summary>
        private static readonly Regex EmailRegex = new Regex(
            @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);
        #endregion

        #region String Validation Methods
        /// <summary>
        /// Kiểm tra chuỗi có trống hoặc null không
        /// </summary>
        /// <param name="value">Giá trị cần kiểm tra</param>
        /// <returns>True nếu chuỗi trống hoặc null</returns>
        public static bool IsNullOrWhiteSpace(string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// Kiểm tra độ dài chuỗi có trong khoảng cho phép không
        /// </summary>
        /// <param name="value">Giá trị cần kiểm tra</param>
        /// <param name="minLength">Độ dài tối thiểu</param>
        /// <param name="maxLength">Độ dài tối đa</param>
        /// <returns>True nếu độ dài hợp lệ</returns>
        public static bool IsValidLength(string value, int minLength, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return false;
            return value.Length >= minLength && value.Length <= maxLength;
        }

        /// <summary>
        /// Kiểm tra độ dài tối thiểu
        /// </summary>
        /// <param name="value">Giá trị cần kiểm tra</param>
        /// <param name="minLength">Độ dài tối thiểu</param>
        /// <returns>True nếu đủ độ dài tối thiểu</returns>
        public static bool IsMinLength(string value, int minLength)
        {
            return !string.IsNullOrEmpty(value) && value.Length >= minLength;
        }
        #endregion

        #region Email Validation Methods
        /// <summary>
        /// Kiểm tra định dạng email có hợp lệ không
        /// </summary>
        /// <param name="email">Email cần kiểm tra</param>
        /// <returns>True nếu email hợp lệ</returns>
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            
            try
            {
                return EmailRegex.IsMatch(email.Trim());
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Kiểm tra email có chứa ký tự @ không (kiểm tra cơ bản)
        /// </summary>
        /// <param name="email">Email cần kiểm tra</param>
        /// <returns>True nếu chứa @</returns>
        public static bool ContainsAtSymbol(string email)
        {
            return !string.IsNullOrEmpty(email) && email.Contains("@");
        }
        #endregion

        #region Password Validation Methods
        /// <summary>
        /// Kiểm tra độ phức tạp của mật khẩu
        /// </summary>
        /// <param name="password">Mật khẩu cần kiểm tra</param>
        /// <returns>True nếu mật khẩu đủ phức tạp</returns>
        public static bool IsPasswordComplex(string password)
        {
            if (string.IsNullOrEmpty(password)) return false;

            bool hasUpperCase = false;
            bool hasLowerCase = false;
            bool hasDigit = false;
            bool hasSpecialChar = false;

            foreach (char c in password)
            {
                if (char.IsUpper(c)) hasUpperCase = true;
                else if (char.IsLower(c)) hasLowerCase = true;
                else if (char.IsDigit(c)) hasDigit = true;
                else if (char.IsPunctuation(c) || char.IsSymbol(c)) hasSpecialChar = true;
            }

            return hasUpperCase && hasLowerCase && hasDigit && hasSpecialChar;
        }

        /// <summary>
        /// Kiểm tra mật khẩu có chứa ít nhất một chữ hoa không
        /// </summary>
        /// <param name="password">Mật khẩu cần kiểm tra</param>
        /// <returns>True nếu có chữ hoa</returns>
        public static bool HasUpperCase(string password)
        {
            if (string.IsNullOrEmpty(password)) return false;
            return password.Any(char.IsUpper);
        }

        /// <summary>
        /// Kiểm tra mật khẩu có chứa ít nhất một chữ thường không
        /// </summary>
        /// <param name="password">Mật khẩu cần kiểm tra</param>
        /// <returns>True nếu có chữ thường</returns>
        public static bool HasLowerCase(string password)
        {
            if (string.IsNullOrEmpty(password)) return false;
            return password.Any(char.IsLower);
        }

        /// <summary>
        /// Kiểm tra mật khẩu có chứa ít nhất một số không
        /// </summary>
        /// <param name="password">Mật khẩu cần kiểm tra</param>
        /// <returns>True nếu có số</returns>
        public static bool HasDigit(string password)
        {
            if (string.IsNullOrEmpty(password)) return false;
            return password.Any(char.IsDigit);
        }

        /// <summary>
        /// Kiểm tra mật khẩu có chứa ít nhất một ký tự đặc biệt không
        /// </summary>
        /// <param name="password">Mật khẩu cần kiểm tra</param>
        /// <returns>True nếu có ký tự đặc biệt</returns>
        public static bool HasSpecialCharacter(string password)
        {
            if (string.IsNullOrEmpty(password)) return false;
            return password.Any(c => char.IsPunctuation(c) || char.IsSymbol(c));
        }
        #endregion

        #region Number Validation Methods
        /// <summary>
        /// Kiểm tra số có trong khoảng cho phép không
        /// </summary>
        /// <param name="value">Giá trị cần kiểm tra</param>
        /// <param name="minValue">Giá trị tối thiểu</param>
        /// <param name="maxValue">Giá trị tối đa</param>
        /// <returns>True nếu số trong khoảng hợp lệ</returns>
        public static bool IsInRange(int value, int minValue, int maxValue)
        {
            return value >= minValue && value <= maxValue;
        }

        /// <summary>
        /// Kiểm tra số có lớn hơn hoặc bằng giá trị tối thiểu không
        /// </summary>
        /// <param name="value">Giá trị cần kiểm tra</param>
        /// <param name="minValue">Giá trị tối thiểu</param>
        /// <returns>True nếu số hợp lệ</returns>
        public static bool IsGreaterOrEqual(int value, int minValue)
        {
            return value >= minValue;
        }
        #endregion

        #region Utility Methods
        /// <summary>
        /// Trim và chuẩn hóa chuỗi
        /// </summary>
        /// <param name="value">Chuỗi cần chuẩn hóa</param>
        /// <returns>Chuỗi đã được chuẩn hóa</returns>
        public static string NormalizeString(string value)
        {
            return value?.Trim() ?? string.Empty;
        }

        /// <summary>
        /// Kiểm tra chuỗi có chứa ký tự không mong muốn không
        /// </summary>
        /// <param name="value">Chuỗi cần kiểm tra</param>
        /// <param name="forbiddenChars">Danh sách ký tự không được phép</param>
        /// <returns>True nếu chứa ký tự không mong muốn</returns>
        public static bool ContainsForbiddenCharacters(string value, char[] forbiddenChars)
        {
            if (string.IsNullOrEmpty(value) || forbiddenChars == null) return false;
            return value.Any(c => forbiddenChars.Contains(c));
        }
        #endregion
    }
}