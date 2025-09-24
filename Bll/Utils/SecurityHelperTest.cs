using System;

namespace Bll.Utils
{
    /// <summary>
    /// Test class cho SecurityHelper - có thể xóa sau khi test xong
    /// </summary>
    public static class SecurityHelperTest
    {
        /// <summary>
        /// Test method để kiểm tra SecurityHelper hoạt động đúng
        /// </summary>
        public static void TestSecurityHelper()
        {
            try
            {
                Console.WriteLine("=== Testing SecurityHelper ===");
                
                // Test 1: Hash password
                string password = "test123";
                string hashedPassword = SecurityHelper.HashPassword(password);
                Console.WriteLine($"Original password: {password}");
                Console.WriteLine($"Hashed password: {hashedPassword}");
                
                // Test 2: Verify correct password
                bool isValid = SecurityHelper.VerifyPassword(password, hashedPassword);
                Console.WriteLine($"Verify correct password: {isValid}");
                
                // Test 3: Verify wrong password
                bool isInvalid = SecurityHelper.VerifyPassword("wrongpassword", hashedPassword);
                Console.WriteLine($"Verify wrong password: {isInvalid}");
                
                // Test 4: Test with custom salt
                string customSalt = "mysalt123";
                string hashedWithCustomSalt = SecurityHelper.HashPassword(password, customSalt);
                Console.WriteLine($"Hashed with custom salt: {hashedWithCustomSalt}");
                
                bool isValidWithCustomSalt = SecurityHelper.VerifyPassword(password, hashedWithCustomSalt);
                Console.WriteLine($"Verify with custom salt: {isValidWithCustomSalt}");
                
                Console.WriteLine("=== SecurityHelper Test Completed Successfully ===");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SecurityHelper Test Failed: {ex.Message}");
            }
        }
    }
}
