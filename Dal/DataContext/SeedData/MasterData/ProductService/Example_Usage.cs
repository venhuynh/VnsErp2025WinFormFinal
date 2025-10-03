using System;
using System.Collections.Generic;
using Dal.DataContext.SeedData.MasterData.ProductService;

namespace Dal.DataContext.SeedData.MasterData.ProductService.Examples
{
    /// <summary>
    /// Ví dụ về cách sử dụng các phương thức trong SeedData_Master_ProductService
    /// </summary>
    public static class ExampleUsage
    {
        /// <summary>
        /// Ví dụ 1: Xóa tất cả dữ liệu ProductService
        /// </summary>
        public static void Example1_DeleteAllData()
        {
            try
            {
                Console.WriteLine("=== Ví dụ 1: Xóa tất cả dữ liệu ProductService ===");
                
                // Xóa tất cả dữ liệu và lấy thông tin số lượng đã xóa
                var deletedCounts = SeedData_Master_ProductService.DeleteAllProductServiceData();
                
                Console.WriteLine("Đã xóa thành công các dữ liệu sau:");
                foreach (var item in deletedCounts)
                {
                    Console.WriteLine($"  {item.Key}: {item.Value} records");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi: {ex.Message}");
            }
        }

        /// <summary>
        /// Ví dụ 2: Kiểm tra số lượng dữ liệu hiện tại
        /// </summary>
        public static void Example2_CheckDataCounts()
        {
            try
            {
                Console.WriteLine("\n=== Ví dụ 2: Kiểm tra số lượng dữ liệu hiện tại ===");
                
                // Lấy thông tin số lượng dữ liệu hiện tại
                var currentCounts = SeedData_Master_ProductService.GetProductServiceDataCounts();
                
                Console.WriteLine("Số lượng dữ liệu hiện tại:");
                foreach (var item in currentCounts)
                {
                    Console.WriteLine($"  {item.Key}: {item.Value} records");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi: {ex.Message}");
            }
        }

        /// <summary>
        /// Ví dụ 3: Xóa dữ liệu với connection string tùy chỉnh
        /// </summary>
        public static void Example3_DeleteWithCustomConnectionString()
        {
            try
            {
                Console.WriteLine("\n=== Ví dụ 3: Xóa dữ liệu với connection string tùy chỉnh ===");
                
                string customConnectionString = "Data Source=localhost;Initial Catalog=VnsErp2025;Integrated Security=True";
                
                // Xóa dữ liệu với connection string tùy chỉnh
                var deletedCounts = SeedData_Master_ProductService.DeleteAllProductServiceData(customConnectionString);
                
                Console.WriteLine("Đã xóa thành công các dữ liệu sau:");
                foreach (var item in deletedCounts)
                {
                    Console.WriteLine($"  {item.Key}: {item.Value} records");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi: {ex.Message}");
            }
        }

        /// <summary>
        /// Ví dụ 4: Tạo lại dữ liệu mẫu sau khi xóa
        /// </summary>
        public static void Example4_DeleteAndReseed()
        {
            try
            {
                Console.WriteLine("\n=== Ví dụ 4: Xóa và tạo lại dữ liệu mẫu ===");
                
                // Bước 1: Kiểm tra số lượng dữ liệu trước khi xóa
                Console.WriteLine("Số lượng dữ liệu trước khi xóa:");
                var countsBefore = SeedData_Master_ProductService.GetProductServiceDataCounts();
                foreach (var item in countsBefore)
                {
                    Console.WriteLine($"  {item.Key}: {item.Value} records");
                }
                
                // Bước 2: Xóa tất cả dữ liệu
                Console.WriteLine("\nĐang xóa dữ liệu...");
                var deletedCounts = SeedData_Master_ProductService.DeleteAllProductServiceData();
                Console.WriteLine("Đã xóa thành công!");
                
                // Bước 3: Tạo lại dữ liệu mẫu
                Console.WriteLine("\nĐang tạo dữ liệu mẫu...");
                SeedData_Master_ProductService.SeedAllData();
                Console.WriteLine("Đã tạo dữ liệu mẫu thành công!");
                
                // Bước 4: Kiểm tra số lượng dữ liệu sau khi tạo
                Console.WriteLine("\nSố lượng dữ liệu sau khi tạo:");
                var countsAfter = SeedData_Master_ProductService.GetProductServiceDataCounts();
                foreach (var item in countsAfter)
                {
                    Console.WriteLine($"  {item.Key}: {item.Value} records");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi: {ex.Message}");
            }
        }

        /// <summary>
        /// Ví dụ 5: Sử dụng trong WinForms application
        /// </summary>
        public static void Example5_WinFormsUsage()
        {
            try
            {
                Console.WriteLine("\n=== Ví dụ 5: Sử dụng trong WinForms application ===");
                
                // Giả sử đây là code trong một button click event
                // private void btnDeleteAllData_Click(object sender, EventArgs e)
                // {
                //     try
                //     {
                //         // Hiển thị dialog xác nhận
                //         var result = MessageBox.Show(
                //             "Bạn có chắc muốn xóa tất cả dữ liệu ProductService?",
                //             "Xác nhận xóa dữ liệu",
                //             MessageBoxButtons.YesNo,
                //             MessageBoxIcon.Warning);
                //         
                //         if (result == DialogResult.Yes)
                //         {
                //             // Hiển thị progress bar hoặc waiting form
                //             using (var waitForm = new WaitForm())
                //             {
                //                 waitForm.Show();
                //                 Application.DoEvents();
                //                 
                //                 // Xóa dữ liệu
                //                 var deletedCounts = SeedData_Master_ProductService.DeleteAllProductServiceData();
                //                 
                //                 // Hiển thị kết quả
                //                 var message = "Đã xóa thành công:\n";
                //                 foreach (var item in deletedCounts)
                //                 {
                //                     message += $"{item.Key}: {item.Value} records\n";
                //                 }
                //                 
                //                 MessageBox.Show(message, "Kết quả", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //             }
                //         }
                //     }
                //     catch (Exception ex)
                //     {
                //         MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //     }
                // }
                
                Console.WriteLine("Xem code trong comment để biết cách sử dụng trong WinForms");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi: {ex.Message}");
            }
        }

        /// <summary>
        /// Chạy tất cả ví dụ
        /// </summary>
        public static void RunAllExamples()
        {
            Console.WriteLine("=== CHẠY TẤT CẢ VÍ DỤ ===");
            
            Example1_DeleteAllData();
            Example2_CheckDataCounts();
            Example3_DeleteWithCustomConnectionString();
            Example4_DeleteAndReseed();
            Example5_WinFormsUsage();
            
            Console.WriteLine("\n=== HOÀN THÀNH TẤT CẢ VÍ DỤ ===");
        }
    }
}
