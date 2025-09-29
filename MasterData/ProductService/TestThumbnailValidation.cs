using System;
using System.Windows.Forms;
using Bll.Common.ImageService;

namespace MasterData.ProductService
{
    /// <summary>
    /// Test class để verify thumbnail validation logic
    /// </summary>
    public static class TestThumbnailValidation
    {
        /// <summary>
        /// Test thumbnail validation với các kích thước khác nhau
        /// </summary>
        public static void TestThumbnailValidation()
        {
            try
            {
                var validationService = new ImageValidationService();
                
                Console.WriteLine("🧪 Testing Thumbnail Validation Logic");
                Console.WriteLine("=====================================\n");

                // Test 1: Invalid file format
                TestInvalidFormat(validationService);
                
                // Test 2: Valid small image (should pass)
                TestValidSmallImage(validationService);
                
                // Test 3: Valid large image (should pass with warnings)
                TestValidLargeImage(validationService);
                
                // Test 4: Invalid data
                TestInvalidData(validationService);

                Console.WriteLine("\n🎉 All thumbnail validation tests completed!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Test failed: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }

        private static void TestInvalidFormat(ImageValidationService validationService)
        {
            Console.WriteLine("Test 1: Invalid file format (.txt)");
            var testData = new byte[] { 1, 2, 3, 4, 5 };
            var result = validationService.ValidateImageForThumbnail(testData, "test.txt");
            
            Console.WriteLine($"   - IsValid: {result.IsValid} (Expected: False)");
            Console.WriteLine($"   - Errors: {result.Errors.Count} (Expected: > 0)");
            Console.WriteLine($"   - Warnings: {result.Warnings.Count}");
            
            if (!result.IsValid && result.Errors.Count > 0)
                Console.WriteLine("   ✅ PASS: Invalid format correctly rejected");
            else
                Console.WriteLine("   ❌ FAIL: Invalid format should be rejected");
            Console.WriteLine();
        }

        private static void TestValidSmallImage(ImageValidationService validationService)
        {
            Console.WriteLine("Test 2: Valid small image (100x100)");
            // Tạo một ảnh JPEG nhỏ giả lập
            var smallImageData = CreateMockJpegData(100, 100);
            var result = validationService.ValidateImageForThumbnail(smallImageData, "small.jpg");
            
            Console.WriteLine($"   - IsValid: {result.IsValid} (Expected: True)");
            Console.WriteLine($"   - Errors: {result.Errors.Count} (Expected: 0)");
            Console.WriteLine($"   - Warnings: {result.Warnings.Count} (Expected: 0)");
            Console.WriteLine($"   - Width: {result.Width} (Expected: 100)");
            Console.WriteLine($"   - Height: {result.Height} (Expected: 100)");
            
            if (result.IsValid && result.Errors.Count == 0 && result.Warnings.Count == 0)
                Console.WriteLine("   ✅ PASS: Small image correctly accepted");
            else
                Console.WriteLine("   ❌ FAIL: Small image should be accepted without warnings");
            Console.WriteLine();
        }

        private static void TestValidLargeImage(ImageValidationService validationService)
        {
            Console.WriteLine("Test 3: Valid large image (8000x6000)");
            // Tạo một ảnh JPEG lớn giả lập
            var largeImageData = CreateMockJpegData(8000, 6000);
            var result = validationService.ValidateImageForThumbnail(largeImageData, "large.jpg");
            
            Console.WriteLine($"   - IsValid: {result.IsValid} (Expected: True)");
            Console.WriteLine($"   - Errors: {result.Errors.Count} (Expected: 0)");
            Console.WriteLine($"   - Warnings: {result.Warnings.Count} (Expected: > 0)");
            Console.WriteLine($"   - Width: {result.Width} (Expected: 8000)");
            Console.WriteLine($"   - Height: {result.Height} (Expected: 6000)");
            
            if (result.IsValid && result.Errors.Count == 0 && result.Warnings.Count > 0)
            {
                Console.WriteLine("   ✅ PASS: Large image correctly accepted with warnings");
                Console.WriteLine($"   - Warning: {string.Join(", ", result.Warnings)}");
            }
            else
                Console.WriteLine("   ❌ FAIL: Large image should be accepted with warnings");
            Console.WriteLine();
        }

        private static void TestInvalidData(ImageValidationService validationService)
        {
            Console.WriteLine("Test 4: Invalid data (null/empty)");
            var result1 = validationService.ValidateImageForThumbnail(null, "test.jpg");
            var result2 = validationService.ValidateImageForThumbnail(new byte[0], "test.jpg");
            
            Console.WriteLine($"   - Null data IsValid: {result1.IsValid} (Expected: False)");
            Console.WriteLine($"   - Empty data IsValid: {result2.IsValid} (Expected: False)");
            
            if (!result1.IsValid && !result2.IsValid)
                Console.WriteLine("   ✅ PASS: Invalid data correctly rejected");
            else
                Console.WriteLine("   ❌ FAIL: Invalid data should be rejected");
            Console.WriteLine();
        }

        /// <summary>
        /// Tạo mock JPEG data cho testing
        /// </summary>
        private static byte[] CreateMockJpegData(int width, int height)
        {
            try
            {
                // Tạo một bitmap đơn giản
                using (var bitmap = new System.Drawing.Bitmap(width, height))
                {
                    using (var graphics = System.Drawing.Graphics.FromImage(bitmap))
                    {
                        // Vẽ một hình chữ nhật đơn giản
                        graphics.FillRectangle(System.Drawing.Brushes.Blue, 0, 0, width, height);
                        graphics.DrawRectangle(System.Drawing.Pens.Red, 0, 0, width - 1, height - 1);
                    }

                    // Convert to JPEG
                    using (var ms = new System.IO.MemoryStream())
                    {
                        bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        return ms.ToArray();
                    }
                }
            }
            catch
            {
                // Fallback: return minimal JPEG header
                return new byte[] { 0xFF, 0xD8, 0xFF, 0xE0, 0x00, 0x10, 0x4A, 0x46, 0x49, 0x46, 0x00, 0x01 };
            }
        }

        /// <summary>
        /// Test trong WinForms context
        /// </summary>
        public static void TestInWinForms()
        {
            try
            {
                var testForm = new Form
                {
                    Text = "Thumbnail Validation Test",
                    Size = new System.Drawing.Size(500, 400),
                    StartPosition = FormStartPosition.CenterScreen
                };

                var textBox = new TextBox
                {
                    Multiline = true,
                    ScrollBars = ScrollBars.Vertical,
                    Dock = DockStyle.Fill,
                    ReadOnly = true,
                    Font = new System.Drawing.Font("Consolas", 9)
                };

                testForm.Controls.Add(textBox);

                // Capture console output
                var originalOut = Console.Out;
                using (var writer = new System.IO.StringWriter())
                {
                    Console.SetOut(writer);
                    
                    TestThumbnailValidation();
                    
                    var output = writer.ToString();
                    textBox.Text = output;
                }
                Console.SetOut(originalOut);

                testForm.KeyDown += (sender, e) =>
                {
                    if (e.KeyCode == Keys.Escape)
                        testForm.Close();
                };

                testForm.KeyPreview = true;
                testForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Test failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Main entry point cho console test
        /// </summary>
        public static void Main(string[] args)
        {
            Console.WriteLine("🚀 VNS ERP 2025 - Thumbnail Validation Test");
            Console.WriteLine("==========================================\n");

            TestThumbnailValidation();

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
