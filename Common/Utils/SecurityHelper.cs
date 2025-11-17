using System;
using System.Security.Cryptography;
using System.Text;

namespace Bll.Utils
{
    /// <summary>
    /// Utility class cho các thao tác bảo mật như hash password, verify password
    /// </summary>
    public static class SecurityHelper
    {
        #region thuocTinhDonGian

        #endregion

        #region phuongThuc

        /// <summary>
        /// Hash password sử dụng SHA256 với salt
        /// </summary>
        /// <param name="password">Mật khẩu gốc</param>
        /// <param name="salt">Salt (optional, sẽ tự tạo nếu null)</param>
        /// <returns>Hash password với format: salt$hash</returns>
        public static string HashPassword(string password, string salt = null)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException(@"Password không được rỗng", nameof(password));

            // Tạo salt nếu chưa có
            if (string.IsNullOrEmpty(salt))
            {
                salt = GenerateSalt();
            }

            // Kết hợp password với salt và hash
            string saltedPassword = password + salt;
            string hash = ComputeSha256Hash(saltedPassword);

            // Trả về format: salt$hash
            return $"{salt}${hash}";
        }

        /// <summary>
        /// Verify password với hash đã lưu
        /// </summary>
        /// <param name="password">Mật khẩu cần verify</param>
        /// <param name="hashedPassword">Hash password đã lưu (format: salt$hash)</param>
        /// <returns>True nếu password đúng</returns>
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(hashedPassword))
                return false;

            try
            {
                // Tách salt và hash từ hashedPassword
                var parts = hashedPassword.Split('$');
                if (parts.Length != 2)
                    return false;

                string salt = parts[0];
                string storedHash = parts[1];

                // Hash password với salt và so sánh
                string computedHash = ComputeSha256Hash(password + salt);
                return string.Equals(computedHash, storedHash, StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Tạo salt ngẫu nhiên
        /// </summary>
        /// <returns>Salt string</returns>
        private static string GenerateSalt()
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] saltBytes = new byte[16];
                rng.GetBytes(saltBytes);
                return Convert.ToBase64String(saltBytes);
            }
        }

        /// <summary>
        /// Compute SHA256 hash
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns>Hash string</returns>
        private static string ComputeSha256Hash(string input)
        {
            using var sha256 = SHA256.Create();
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = sha256.ComputeHash(inputBytes);
            return Convert.ToBase64String(hashBytes);
        }

        #endregion
    }
}