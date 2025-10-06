using System;
using System.Linq;
using Dal.DataContext;

namespace Dal.DataContext.SeedData
{
    /// <summary>
    /// Class quản lý seed data cho tất cả các module
    /// </summary>
    public static class SeedData_All
    {
        /// <summary>
        /// Xóa tất cả dữ liệu trong database (tất cả các bảng)
        /// Sử dụng connection string từ ConfigurationManager (recommended)
        /// </summary>
        public static void DeleteAllData()
        {
            using (var context = CreateContext())
            {
                DeleteAllData(context);
            }
        }

        /// <summary>
        /// Xóa tất cả dữ liệu trong database (tất cả các bảng)
        /// Sử dụng connection string được truyền vào (cho trường hợp đặc biệt)
        /// </summary>
        /// <param name="connectionString">Connection string để kết nối database</param>
        public static void DeleteAllData(string connectionString)
        {
            using (var context = new VnsErp2025DataContext(connectionString))
            {
                DeleteAllData(context);
            }
        }

        /// <summary>
        /// Xóa tất cả dữ liệu trong database (tất cả các bảng)
        /// Sử dụng context có sẵn (cho backward compatibility)
        /// </summary>
        /// <param name="context">DataContext để xóa dữ liệu</param>
        public static void DeleteAllData(VnsErp2025DataContext context)
        {
            try
            {
                Console.WriteLine("🔄 Bắt đầu xóa tất cả dữ liệu...");

                // Xóa dữ liệu theo thứ tự ngược lại để tránh foreign key constraint
                
                // 1-5. Xóa tất cả dữ liệu Partner (BusinessPartner, BusinessPartnerCategory, Mappings, Sites, Contacts)
                var contactCount = context.BusinessPartnerContacts.Count();
                var siteCount = context.BusinessPartnerSites.Count();
                var mappingCount = context.BusinessPartner_BusinessPartnerCategories.Count();
                var partnerCount = context.BusinessPartners.Count();
                var categoryCount = context.BusinessPartnerCategories.Count();

                if (contactCount > 0 || siteCount > 0 || mappingCount > 0 || partnerCount > 0 || categoryCount > 0)
                {
                    // Xóa theo thứ tự đúng để tránh foreign key constraint
                    if (contactCount > 0)
                    {
                        context.BusinessPartnerContacts.DeleteAllOnSubmit(context.BusinessPartnerContacts);
                        context.SubmitChanges();
                        Console.WriteLine($"✅ Đã xóa {contactCount} BusinessPartnerContacts");
                    }

                    if (siteCount > 0)
                    {
                        context.BusinessPartnerSites.DeleteAllOnSubmit(context.BusinessPartnerSites);
                        context.SubmitChanges();
                        Console.WriteLine($"✅ Đã xóa {siteCount} BusinessPartnerSites");
                    }

                    if (mappingCount > 0)
                    {
                        context.BusinessPartner_BusinessPartnerCategories.DeleteAllOnSubmit(context.BusinessPartner_BusinessPartnerCategories);
                        context.SubmitChanges();
                        Console.WriteLine($"✅ Đã xóa {mappingCount} BusinessPartner_BusinessPartnerCategories");
                    }

                    if (partnerCount > 0)
                    {
                        context.BusinessPartners.DeleteAllOnSubmit(context.BusinessPartners);
                        context.SubmitChanges();
                        Console.WriteLine($"✅ Đã xóa {partnerCount} BusinessPartners");
                    }

                    if (categoryCount > 0)
                    {
                        context.BusinessPartnerCategories.DeleteAllOnSubmit(context.BusinessPartnerCategories);
                        context.SubmitChanges();
                        Console.WriteLine($"✅ Đã xóa {categoryCount} BusinessPartnerCategories");
                    }
                }

                // 6. Xóa Companies (Company Management)
                var companyCount = context.Companies.Count();
                if (companyCount > 0)
                {
                    context.Companies.DeleteAllOnSubmit(context.Companies);
                    context.SubmitChanges();
                    Console.WriteLine($"✅ Đã xóa {companyCount} Companies");
                }

                // 7. Xóa CompanyBranches (Company Management)
                var branchCount = context.CompanyBranches.Count();
                if (branchCount > 0)
                {
                    context.CompanyBranches.DeleteAllOnSubmit(context.CompanyBranches);
                    context.SubmitChanges();
                    Console.WriteLine($"✅ Đã xóa {branchCount} CompanyBranches");
                }

                // 8. Xóa Departments (Company Management)
                var departmentCount = context.Departments.Count();
                if (departmentCount > 0)
                {
                    context.Departments.DeleteAllOnSubmit(context.Departments);
                    context.SubmitChanges();
                    Console.WriteLine($"✅ Đã xóa {departmentCount} Departments");
                }

                // 9. Xóa Employees (Company Management)
                var employeeCount = context.Employees.Count();
                if (employeeCount > 0)
                {
                    context.Employees.DeleteAllOnSubmit(context.Employees);
                    context.SubmitChanges();
                    Console.WriteLine($"✅ Đã xóa {employeeCount} Employees");
                }

                // 10. Xóa Positions (Company Management)
                var positionCount = context.Positions.Count();
                if (positionCount > 0)
                {
                    context.Positions.DeleteAllOnSubmit(context.Positions);
                    context.SubmitChanges();
                    Console.WriteLine($"✅ Đã xóa {positionCount} Positions");
                }

                // 11. Xóa VariantAttributes (Product Management)
                var variantAttributeCount = context.VariantAttributes.Count();
                if (variantAttributeCount > 0)
                {
                    context.VariantAttributes.DeleteAllOnSubmit(context.VariantAttributes);
                    context.SubmitChanges();
                    Console.WriteLine($"✅ Đã xóa {variantAttributeCount} VariantAttributes");
                }

                // 12. Xóa ProductImages (Product Management)
                var productImageCount = context.ProductImages.Count();
                if (productImageCount > 0)
                {
                    context.ProductImages.DeleteAllOnSubmit(context.ProductImages);
                    context.SubmitChanges();
                    Console.WriteLine($"✅ Đã xóa {productImageCount} ProductImages");
                }

                // 13. Xóa ProductVariants (Product Management)
                var variantCount = context.ProductVariants.Count();
                if (variantCount > 0)
                {
                    context.ProductVariants.DeleteAllOnSubmit(context.ProductVariants);
                    context.SubmitChanges();
                    Console.WriteLine($"✅ Đã xóa {variantCount} ProductVariants");
                }

                // 14. Xóa ProductServices (Product Management)
                var productCount = context.ProductServices.Count();
                if (productCount > 0)
                {
                    context.ProductServices.DeleteAllOnSubmit(context.ProductServices);
                    context.SubmitChanges();
                    Console.WriteLine($"✅ Đã xóa {productCount} ProductServices");
                }

                // 15. Xóa ProductServiceCategories (Product Management)
                var productCategoryCount = context.ProductServiceCategories.Count();
                if (productCategoryCount > 0)
                {
                    context.ProductServiceCategories.DeleteAllOnSubmit(context.ProductServiceCategories);
                    context.SubmitChanges();
                    Console.WriteLine($"✅ Đã xóa {productCategoryCount} ProductServiceCategories");
                }

                // 16. Xóa UnitOfMeasures (Product Management)
                var unitCount = context.UnitOfMeasures.Count();
                if (unitCount > 0)
                {
                    context.UnitOfMeasures.DeleteAllOnSubmit(context.UnitOfMeasures);
                    context.SubmitChanges();
                    Console.WriteLine($"✅ Đã xóa {unitCount} UnitOfMeasures");
                }

                // 17. Xóa AttributeValues (Product Management)
                var attributeValueCount = context.AttributeValues.Count();
                if (attributeValueCount > 0)
                {
                    context.AttributeValues.DeleteAllOnSubmit(context.AttributeValues);
                    context.SubmitChanges();
                    Console.WriteLine($"✅ Đã xóa {attributeValueCount} AttributeValues");
                }

                // 18. Xóa Attributes (Product Management)
                var attributeCount = context.Attributes.Count();
                if (attributeCount > 0)
                {
                    context.Attributes.DeleteAllOnSubmit(context.Attributes);
                    context.SubmitChanges();
                    Console.WriteLine($"✅ Đã xóa {attributeCount} Attributes");
                }

                // 19. Xóa ApplicationUsers (User Management)
                var userCount = context.ApplicationUsers.Count();
                if (userCount > 0)
                {
                    context.ApplicationUsers.DeleteAllOnSubmit(context.ApplicationUsers);
                    context.SubmitChanges();
                    Console.WriteLine($"✅ Đã xóa {userCount} ApplicationUsers");
                }

                Console.WriteLine("🎉 Hoàn thành xóa tất cả dữ liệu trong database!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Lỗi khi xóa dữ liệu: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Tạo context mới từ connection string
        /// </summary>
        /// <returns>VnsErp2025DataContext</returns>
        private static VnsErp2025DataContext CreateContext()
        {
            return new VnsErp2025DataContext();
        }
    }
}
