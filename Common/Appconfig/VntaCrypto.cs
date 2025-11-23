using System;
using System.Security.Cryptography;
using System.Text;
// ReSharper disable InconsistentNaming

namespace Common.Appconfig
{
    /// <summary>
    /// Utility class cho mã hóa/giải mã dữ liệu sử dụng AES
    /// </summary>
    public static class VntaCrypto
    {
        private const string ENCRYPTION_KEY = "VNTA_NET_2025_KEY_32BYTES_LONG!!"; // 32 bytes key
        private const string ENCRYPTION_IV = "VNTA_IV_16BYTES!!"; // 16 bytes IV
        private const string ENCRYPTION_PREFIX = "VNTA_ENCRYPTED:";

        /// <summary>
        /// Mã hóa chuỗi
        /// </summary>
        /// <param name="plainText">Chuỗi cần mã hóa</param>
        /// <returns>Chuỗi đã mã hóa (Base64)</returns>
        public static string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return string.Empty;

            try
            {
                byte[] keyBytes = Encoding.UTF8.GetBytes(ENCRYPTION_KEY);
                byte[] ivBytes = Encoding.UTF8.GetBytes(ENCRYPTION_IV);

                using var aes = Aes.Create();
                aes.Key = keyBytes;
                aes.IV = ivBytes;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (var encryptor = aes.CreateEncryptor())
                {
                    byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                    byte[] encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
                    string encryptedBase64 = Convert.ToBase64String(encryptedBytes);
                    return ENCRYPTION_PREFIX + encryptedBase64;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi mã hóa: {ex.Message}");
                return plainText; // Fallback về plain text nếu lỗi
            }
        }

        /// <summary>
        /// Giải mã chuỗi
        /// </summary>
        /// <param name="encryptedText">Chuỗi đã mã hóa</param>
        /// <returns>Chuỗi đã giải mã</returns>
        public static string Decrypt(string encryptedText)
        {
            if (string.IsNullOrEmpty(encryptedText))
                return string.Empty;

            // Kiểm tra xem có prefix không
            if (!encryptedText.StartsWith(ENCRYPTION_PREFIX))
            {
                // Không phải định dạng VntaCrypto, trả về nguyên vẹn
                return encryptedText;
            }

            try
            {
                string encryptedBase64 = encryptedText.Substring(ENCRYPTION_PREFIX.Length);
                byte[] encryptedBytes = Convert.FromBase64String(encryptedBase64);

                byte[] keyBytes = Encoding.UTF8.GetBytes(ENCRYPTION_KEY);
                byte[] ivBytes = Encoding.UTF8.GetBytes(ENCRYPTION_IV);

                using var aes = Aes.Create();
                aes.Key = keyBytes;
                aes.IV = ivBytes;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using var decryptor = aes.CreateDecryptor();
                byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                return Encoding.UTF8.GetString(decryptedBytes);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi giải mã: {ex.Message}");
                return encryptedText; // Fallback về encrypted text nếu lỗi
            }
        }

        /// <summary>
        /// Kiểm tra xem chuỗi có phải đã được mã hóa bằng VntaCrypto không
        /// </summary>
        /// <param name="text">Chuỗi cần kiểm tra</param>
        /// <returns>True nếu đã được mã hóa</returns>
        public static bool IsEncrypted(string text)
        {
            if (string.IsNullOrEmpty(text))
                return false;

            return text.StartsWith(ENCRYPTION_PREFIX);
        }
    }
}

