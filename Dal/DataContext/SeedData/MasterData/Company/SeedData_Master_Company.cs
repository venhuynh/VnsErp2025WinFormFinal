using System;
using System.Collections.Generic;
using System.Linq;
using Dal.DataContext;

namespace Dal.DataContext.SeedData.MasterData.Company
{
    /// <summary>
    /// Class quản lý seed data cho Company entities
    /// Bao gồm: Company, CompanyBranch, Department, Position, Employee
    /// </summary>
    public static class SeedData_Master_Company
    {
        #region ========== PUBLIC METHODS ==========

        /// <summary>
        /// Tạo tất cả seed data cho Company entities
        /// </summary>
        public static void CreateAllCompanyData()
        {
            using (var context = CreateContext())
            {
                CreateAllCompanyData(context);
            }
        }

        /// <summary>
        /// Tạo tất cả seed data cho Company entities với connection string
        /// </summary>
        /// <param name="connectionString">Connection string để kết nối database</param>
        public static void CreateAllCompanyData(string connectionString)
        {
            using (var context = new VnsErp2025DataContext(connectionString))
            {
                CreateAllCompanyData(context);
            }
        }

        /// <summary>
        /// Tạo tất cả seed data cho Company entities với context
        /// </summary>
        /// <param name="context">DataContext để tạo dữ liệu</param>
        public static void CreateAllCompanyData(VnsErp2025DataContext context)
        {
            try
            {
                Console.WriteLine("🏢 Bắt đầu tạo seed data cho Company entities...");

                // 1. Tạo Companies
                CreateCompanies(context);

                // 2. Tạo CompanyBranches
                CreateCompanyBranches(context);

                // 3. Tạo Positions
                CreatePositions(context);

                // 4. Tạo Departments
                CreateDepartments(context);

                // 5. Tạo Employees
                CreateEmployees(context);

                Console.WriteLine("🎉 Hoàn thành tạo seed data cho Company entities!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Lỗi khi tạo seed data: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Xóa tất cả seed data cho Company entities
        /// </summary>
        public static void DeleteAllCompanyData()
        {
            using (var context = CreateContext())
            {
                DeleteAllCompanyData(context);
            }
        }

        /// <summary>
        /// Xóa tất cả seed data cho Company entities với connection string
        /// </summary>
        /// <param name="connectionString">Connection string để kết nối database</param>
        public static void DeleteAllCompanyData(string connectionString)
        {
            using (var context = new VnsErp2025DataContext(connectionString))
            {
                DeleteAllCompanyData(context);
            }
        }

        /// <summary>
        /// Xóa tất cả seed data cho Company entities với context
        /// </summary>
        /// <param name="context">DataContext để xóa dữ liệu</param>
        public static void DeleteAllCompanyData(VnsErp2025DataContext context)
        {
            try
            {
                Console.WriteLine("🗑️ Bắt đầu xóa seed data cho Company entities...");

                // Xóa theo thứ tự ngược lại để tránh foreign key constraint
                
                // 1. Xóa Employees
                var employeeCount = context.Employees.Count();
                if (employeeCount > 0)
                {
                    context.Employees.DeleteAllOnSubmit(context.Employees);
                    context.SubmitChanges();
                    Console.WriteLine($"✅ Đã xóa {employeeCount} Employees");
                }

                // 2. Xóa Departments
                var departmentCount = context.Departments.Count();
                if (departmentCount > 0)
                {
                    context.Departments.DeleteAllOnSubmit(context.Departments);
                    context.SubmitChanges();
                    Console.WriteLine($"✅ Đã xóa {departmentCount} Departments");
                }

                // 3. Xóa Positions
                var positionCount = context.Positions.Count();
                if (positionCount > 0)
                {
                    context.Positions.DeleteAllOnSubmit(context.Positions);
                    context.SubmitChanges();
                    Console.WriteLine($"✅ Đã xóa {positionCount} Positions");
                }

                // 4. Xóa CompanyBranches
                var branchCount = context.CompanyBranches.Count();
                if (branchCount > 0)
                {
                    context.CompanyBranches.DeleteAllOnSubmit(context.CompanyBranches);
                    context.SubmitChanges();
                    Console.WriteLine($"✅ Đã xóa {branchCount} CompanyBranches");
                }

                // 5. Xóa Companies
                var companyCount = context.Companies.Count();
                if (companyCount > 0)
                {
                    context.Companies.DeleteAllOnSubmit(context.Companies);
                    context.SubmitChanges();
                    Console.WriteLine($"✅ Đã xóa {companyCount} Companies");
                }

                Console.WriteLine("🎉 Hoàn thành xóa seed data cho Company entities!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Lỗi khi xóa seed data: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region ========== PRIVATE METHODS ==========

        /// <summary>
        /// Tạo Companies
        /// </summary>
        private static void CreateCompanies(VnsErp2025DataContext context)
        {
            var companies = new List<DataContext.Company>
            {
                new DataContext.Company
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    CompanyCode = "VNS001",
                    CompanyName = "Công ty TNHH VNS Technology",
                    TaxCode = "0123456789",
                    Phone = "028-1234-5678",
                    Email = "info@vnstech.com",
                    Website = "www.vnstech.com",
                    Address = "123 Nguyễn Huệ, Quận 1, TP.HCM",
                    Country = "Việt Nam",
                    CreatedDate = DateTime.Now.AddDays(-365),
                    UpdatedDate = DateTime.Now.AddDays(-30)
                },
                new DataContext.Company
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    CompanyCode = "VNS002",
                    CompanyName = "Công ty TNHH VNS Solutions",
                    TaxCode = "0987654321",
                    Phone = "028-8765-4321",
                    Email = "contact@vnssolutions.com",
                    Website = "www.vnssolutions.com",
                    Address = "456 Lê Lợi, Quận 3, TP.HCM",
                    Country = "Việt Nam",
                    CreatedDate = DateTime.Now.AddDays(-300),
                    UpdatedDate = DateTime.Now.AddDays(-15)
                }
            };

            context.Companies.InsertAllOnSubmit(companies);
            context.SubmitChanges();
            Console.WriteLine($"✅ Đã tạo {companies.Count} Companies");
        }

        /// <summary>
        /// Tạo CompanyBranches
        /// </summary>
        private static void CreateCompanyBranches(VnsErp2025DataContext context)
        {
            var companyId1 = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var companyId2 = Guid.Parse("22222222-2222-2222-2222-222222222222");

            var branches = new List<CompanyBranch>
            {
                // Chi nhánh của VNS Technology
                new CompanyBranch
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    CompanyId = companyId1,
                    BranchCode = "VNS001-HN",
                    BranchName = "Chi nhánh Hà Nội",
                    Address = "789 Tràng Tiền, Hoàn Kiếm, Hà Nội",
                    Phone = "024-1234-5678",
                    Email = "hanoi@vnstech.com",
                    ManagerName = "Nguyễn Văn A",
                    IsActive = true,
                },
                new CompanyBranch
                {
                    Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    CompanyId = companyId1,
                    BranchCode = "VNS001-DN",
                    BranchName = "Chi nhánh Đà Nẵng",
                    Address = "321 Lê Duẩn, Hải Châu, Đà Nẵng",
                    Phone = "0236-1234-5678",
                    Email = "danang@vnstech.com",
                    ManagerName = "Trần Thị B",
                    IsActive = true,
                },
                // Chi nhánh của VNS Solutions
                new CompanyBranch
                {
                    Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                    CompanyId = companyId2,
                    BranchCode = "VNS002-HN",
                    BranchName = "Chi nhánh Hà Nội",
                    Address = "654 Ba Đình, Ba Đình, Hà Nội",
                    Phone = "024-8765-4321",
                    Email = "hanoi@vnssolutions.com",
                    ManagerName = "Lê Văn C",
                    IsActive = true,
                }
            };

            context.CompanyBranches.InsertAllOnSubmit(branches);
            context.SubmitChanges();
            Console.WriteLine($"✅ Đã tạo {branches.Count} CompanyBranches");
        }

        /// <summary>
        /// Tạo Positions
        /// </summary>
        private static void CreatePositions(VnsErp2025DataContext context)
        {
            var companyId1 = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var companyId2 = Guid.Parse("22222222-2222-2222-2222-222222222222");

            var positions = new List<Position>
            {
                // VNS Technology positions
                new Position
                {
                    Id = Guid.Parse("66666666-6666-6666-6666-666666666666"),
                    CompanyId = companyId1,
                    PositionCode = "CEO",
                    PositionName = "Tổng Giám Đốc",
                    Description = "Lãnh đạo cao nhất của công ty",
                    IsManagerLevel = true,
                    IsActive = true,
                },
                new Position
                {
                    Id = Guid.Parse("77777777-7777-7777-7777-777777777777"),
                    CompanyId = companyId1,
                    PositionCode = "CTO",
                    PositionName = "Giám Đốc Công Nghệ",
                    Description = "Quản lý công nghệ và phát triển sản phẩm",
                    IsManagerLevel = true,
                    IsActive = true,
                },
                new Position
                {
                    Id = Guid.Parse("88888888-8888-8888-8888-888888888888"),
                    CompanyId = companyId1,
                    PositionCode = "DEV",
                    PositionName = "Lập Trình Viên",
                    Description = "Phát triển phần mềm và ứng dụng",
                    IsManagerLevel = false,
                    IsActive = true,
                },
                new Position
                {
                    Id = Guid.Parse("99999999-9999-9999-9999-999999999999"),
                    CompanyId = companyId1,
                    PositionCode = "HR",
                    PositionName = "Chuyên Viên Nhân Sự",
                    Description = "Quản lý nhân sự và tuyển dụng",
                    IsManagerLevel = false,
                    IsActive = true,
                },
                // VNS Solutions positions
                new Position
                {
                    Id = Guid.Parse("AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA"),
                    CompanyId = companyId2,
                    PositionCode = "CEO",
                    PositionName = "Tổng Giám Đốc",
                    Description = "Lãnh đạo cao nhất của công ty",
                    IsManagerLevel = true,
                    IsActive = true,
                },
                new Position
                {
                    Id = Guid.Parse("BBBBBBBB-BBBB-BBBB-BBBB-BBBBBBBBBBBB"),
                    CompanyId = companyId2,
                    PositionCode = "SALES",
                    PositionName = "Nhân Viên Kinh Doanh",
                    Description = "Bán hàng và phát triển khách hàng",
                    IsManagerLevel = false,
                    IsActive = true,
                }
            };

            context.Positions.InsertAllOnSubmit(positions);
            context.SubmitChanges();
            Console.WriteLine($"✅ Đã tạo {positions.Count} Positions");
        }

        /// <summary>
        /// Tạo Departments
        /// </summary>
        private static void CreateDepartments(VnsErp2025DataContext context)
        {
            var companyId1 = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var companyId2 = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var branchId1 = Guid.Parse("33333333-3333-3333-3333-333333333333");
            var branchId2 = Guid.Parse("44444444-4444-4444-4444-444444444444");

            var departments = new List<Department>
            {
                // VNS Technology - Cấp 1
                new Department
                {
                    Id = Guid.Parse("CCCCCCCC-CCCC-CCCC-CCCC-CCCCCCCCCCCC"),
                    CompanyId = companyId1,
                    BranchId = null, // Trụ sở chính
                    DepartmentCode = "CEO",
                    DepartmentName = "Ban Giám Đốc",
                    ParentId = null,
                    Description = "Ban lãnh đạo cao nhất của công ty",
                    IsActive = true,
                },
                new Department
                {
                    Id = Guid.Parse("DDDDDDDD-DDDD-DDDD-DDDD-DDDDDDDDDDDD"),
                    CompanyId = companyId1,
                    BranchId = null,
                    DepartmentCode = "IT",
                    DepartmentName = "Phòng Công Nghệ Thông Tin",
                    ParentId = null,
                    Description = "Phát triển và bảo trì hệ thống công nghệ",
                    IsActive = true,
                },
                new Department
                {
                    Id = Guid.Parse("EEEEEEEE-EEEE-EEEE-EEEE-EEEEEEEEEEEE"),
                    CompanyId = companyId1,
                    BranchId = null,
                    DepartmentCode = "HR",
                    DepartmentName = "Phòng Nhân Sự",
                    ParentId = null,
                    Description = "Quản lý nhân sự và tuyển dụng",
                    IsActive = true,
                },
                // VNS Technology - Cấp 2 (Phòng ban con)
                new Department
                {
                    Id = Guid.Parse("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF"),
                    CompanyId = companyId1,
                    BranchId = null,
                    DepartmentCode = "IT-DEV",
                    DepartmentName = "Bộ phận Phát Triển",
                    ParentId = Guid.Parse("DDDDDDDD-DDDD-DDDD-DDDD-DDDDDDDDDDDD"),
                    Description = "Phát triển phần mềm và ứng dụng",
                    IsActive = true,
                },
                new Department
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111112"),
                    CompanyId = companyId1,
                    BranchId = null,
                    DepartmentCode = "IT-SUPPORT",
                    DepartmentName = "Bộ phận Hỗ Trợ",
                    ParentId = Guid.Parse("DDDDDDDD-DDDD-DDDD-DDDD-DDDDDDDDDDDD"),
                    Description = "Hỗ trợ kỹ thuật và bảo trì hệ thống",
                    IsActive = true,
                },
                new Department
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222223"),
                    CompanyId = companyId1,
                    BranchId = null,
                    DepartmentCode = "HR-REC",
                    DepartmentName = "Bộ phận Tuyển Dụng",
                    ParentId = Guid.Parse("EEEEEEEE-EEEE-EEEE-EEEE-EEEEEEEEEEEE"),
                    Description = "Tuyển dụng và phỏng vấn nhân viên",
                    IsActive = true,
                },
                // VNS Solutions - Cấp 1
                new Department
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333334"),
                    CompanyId = companyId2,
                    BranchId = null,
                    DepartmentCode = "CEO",
                    DepartmentName = "Ban Giám Đốc",
                    ParentId = null,
                    Description = "Ban lãnh đạo cao nhất của công ty",
                    IsActive = true,
                },
                new Department
                {
                    Id = Guid.Parse("44444444-4444-4444-4444-444444444445"),
                    CompanyId = companyId2,
                    BranchId = null,
                    DepartmentCode = "SALES",
                    DepartmentName = "Phòng Kinh Doanh",
                    ParentId = null,
                    Description = "Bán hàng và phát triển khách hàng",
                    IsActive = true,
                }
            };

            context.Departments.InsertAllOnSubmit(departments);
            context.SubmitChanges();
            Console.WriteLine($"✅ Đã tạo {departments.Count} Departments");
        }

        /// <summary>
        /// Tạo Employees
        /// </summary>
        private static void CreateEmployees(VnsErp2025DataContext context)
        {
            var companyId1 = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var companyId2 = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var branchId1 = Guid.Parse("33333333-3333-3333-3333-333333333333");
            var branchId2 = Guid.Parse("44444444-4444-4444-4444-444444444444");

            var employees = new List<Employee>
            {
                // VNS Technology employees
                new Employee
                {
                    Id = Guid.Parse("55555555-5555-5555-5555-555555555556"),
                    CompanyId = companyId1,
                    BranchId = null,
                    DepartmentId = Guid.Parse("CCCCCCCC-CCCC-CCCC-CCCC-CCCCCCCCCCCC"),
                    PositionId = Guid.Parse("66666666-6666-6666-6666-666666666666"),
                    EmployeeCode = "VNS001-001",
                    FullName = "Nguyễn Văn Giám Đốc",
                    Gender = "Nam",
                    BirthDate = new DateTime(1975, 5, 15),
                    Phone = "0901-234-567",
                    Email = "ceo@vnstech.com",
                    HireDate = new DateTime(2020, 1, 1),
                    ResignDate = null,
                    AvatarPath = "/images/avatars/ceo.jpg",
                    IsActive = true,
                },
                new Employee
                {
                    Id = Guid.Parse("66666666-6666-6666-6666-666666666667"),
                    CompanyId = companyId1,
                    BranchId = null,
                    DepartmentId = Guid.Parse("DDDDDDDD-DDDD-DDDD-DDDD-DDDDDDDDDDDD"),
                    PositionId = Guid.Parse("77777777-7777-7777-7777-777777777777"),
                    EmployeeCode = "VNS001-002",
                    FullName = "Trần Thị Công Nghệ",
                    Gender = "Nữ",
                    BirthDate = new DateTime(1980, 8, 20),
                    Phone = "0902-345-678",
                    Email = "cto@vnstech.com",
                    HireDate = new DateTime(2020, 3, 1),
                    ResignDate = null,
                    AvatarPath = "/images/avatars/cto.jpg",
                    IsActive = true,
                },
                new Employee
                {
                    Id = Guid.Parse("77777777-7777-7777-7777-777777777778"),
                    CompanyId = companyId1,
                    BranchId = null,
                    DepartmentId = Guid.Parse("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF"),
                    PositionId = Guid.Parse("88888888-8888-8888-8888-888888888888"),
                    EmployeeCode = "VNS001-003",
                    FullName = "Lê Văn Lập Trình",
                    Gender = "Nam",
                    BirthDate = new DateTime(1990, 12, 10),
                    Phone = "0903-456-789",
                    Email = "dev@vnstech.com",
                    HireDate = new DateTime(2021, 6, 1),
                    ResignDate = null,
                    AvatarPath = "/images/avatars/dev.jpg",
                    IsActive = true,
                },
                new Employee
                {
                    Id = Guid.Parse("88888888-8888-8888-8888-888888888889"),
                    CompanyId = companyId1,
                    BranchId = null,
                    DepartmentId = Guid.Parse("22222222-2222-2222-2222-222222222223"),
                    PositionId = Guid.Parse("99999999-9999-9999-9999-999999999999"),
                    EmployeeCode = "VNS001-004",
                    FullName = "Phạm Thị Nhân Sự",
                    Gender = "Nữ",
                    BirthDate = new DateTime(1985, 3, 25),
                    Phone = "0904-567-890",
                    Email = "hr@vnstech.com",
                    HireDate = new DateTime(2021, 9, 1),
                    ResignDate = null,
                    AvatarPath = "/images/avatars/hr.jpg",
                    IsActive = true,
                },
                // VNS Solutions employees
                new Employee
                {
                    Id = Guid.Parse("99999999-9999-9999-9999-99999999999A"),
                    CompanyId = companyId2,
                    BranchId = null,
                    DepartmentId = Guid.Parse("33333333-3333-3333-3333-333333333334"),
                    PositionId = Guid.Parse("AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA"),
                    EmployeeCode = "VNS002-001",
                    FullName = "Hoàng Văn Giám Đốc",
                    Gender = "Nam",
                    BirthDate = new DateTime(1978, 7, 12),
                    Phone = "0905-678-901",
                    Email = "ceo@vnssolutions.com",
                    HireDate = new DateTime(2021, 1, 1),
                    ResignDate = null,
                    AvatarPath = "/images/avatars/ceo2.jpg",
                    IsActive = true,
                },
                new Employee
                {
                    Id = Guid.Parse("AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAB"),
                    CompanyId = companyId2,
                    BranchId = null,
                    DepartmentId = Guid.Parse("44444444-4444-4444-4444-444444444445"),
                    PositionId = Guid.Parse("BBBBBBBB-BBBB-BBBB-BBBB-BBBBBBBBBBBB"),
                    EmployeeCode = "VNS002-002",
                    FullName = "Võ Thị Kinh Doanh",
                    Gender = "Nữ",
                    BirthDate = new DateTime(1988, 11, 5),
                    Phone = "0906-789-012",
                    Email = "sales@vnssolutions.com",
                    HireDate = new DateTime(2021, 4, 1),
                    ResignDate = null,
                    AvatarPath = "/images/avatars/sales.jpg",
                    IsActive = true,
                }
            };

            context.Employees.InsertAllOnSubmit(employees);
            context.SubmitChanges();
            Console.WriteLine($"✅ Đã tạo {employees.Count} Employees");
        }

        /// <summary>
        /// Tạo context mới từ connection string
        /// </summary>
        /// <returns>VnsErp2025DataContext</returns>
        private static VnsErp2025DataContext CreateContext()
        {
            return new VnsErp2025DataContext();
        }

        #endregion
    }
}
