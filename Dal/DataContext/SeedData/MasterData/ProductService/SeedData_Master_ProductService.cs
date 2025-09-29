using System;
using System.Collections.Generic;
using System.Linq;
using Dal.DataContext;
using Dal.Configuration;
using Dal.Logging;

namespace Dal.DataContext.SeedData.MasterData.ProductService
{
    public static class SeedData_Master_ProductService
    {
        #region Private Fields

        /// <summary>
        /// Database settings
        /// </summary>
        private static readonly DatabaseSettings _settings = ConfigurationManager.DatabaseSettings;

        /// <summary>
        /// Connection string
        /// </summary>
        private static readonly string _connStr = _settings.ConnectionString;

        #endregion

        #region Private Methods

        /// <summary>
        /// Create DataContext với settings
        /// </summary>
        private static VnsErp2025DataContext CreateContext()
        {
            var context = new VnsErp2025DataContext(_connStr);
            context.CommandTimeout = _settings.CommandTimeout;
            return context;
        }

        #endregion

        /// <summary>
        /// Tạo mã phân loại từ tên danh mục
        /// </summary>
        /// <param name="categoryName">Tên danh mục</param>
        /// <returns>Mã phân loại</returns>
        private static string GenerateCategoryCode(string categoryName)
        {
            if (string.IsNullOrWhiteSpace(categoryName)) return "CAT";
            
            // Lấy chữ cái đầu của mỗi từ trong tên danh mục
            var words = categoryName.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var code = string.Empty;
            
            foreach (var word in words)
            {
                if (!string.IsNullOrWhiteSpace(word))
                {
                    var firstChar = word.Trim().FirstOrDefault();
                    if (char.IsLetter(firstChar))
                    {
                        code += char.ToUpper(firstChar);
                    }
                }
            }
            
            // Đảm bảo mã có ít nhất 2 ký tự
            return code.Length >= 2 ? code : "CAT";
        }

        /// <summary>
        /// Tạo dữ liệu mẫu cho ProductServiceCategory với 3 cấp phân cấp
        /// </summary>
        /// <param name="context">DataContext để lưu dữ liệu</param>
        public static void SeedProductServiceCategories(VnsErp2025DataContext context)
        {
            var categories = new List<ProductServiceCategory>();
            var random = new Random();

            // Cấp 1: 6 danh mục chính
            var level1Categories = new[]
            {
                new { Name = "Thiết bị điện tử", Description = "Các thiết bị điện tử, công nghệ thông tin" },
                new { Name = "Văn phòng phẩm", Description = "Đồ dùng văn phòng, dụng cụ học tập" },
                new { Name = "Nội thất", Description = "Bàn ghế, tủ kệ, đồ nội thất văn phòng" },
                new { Name = "Dịch vụ IT", Description = "Các dịch vụ công nghệ thông tin" },
                new { Name = "Dịch vụ tư vấn", Description = "Dịch vụ tư vấn kinh doanh, pháp lý" },
                new { Name = "Dịch vụ bảo trì", Description = "Dịch vụ bảo trì, sửa chữa thiết bị" }
            };

            // Cấp 2: 18 danh mục con
            var level2Categories = new[]
            {
                // Thiết bị điện tử
                new { Name = "Máy tính", Description = "Máy tính để bàn, laptop, máy chủ", ParentName = "Thiết bị điện tử" },
                new { Name = "Thiết bị mạng", Description = "Router, switch, modem, thiết bị mạng", ParentName = "Thiết bị điện tử" },
                new { Name = "Thiết bị in ấn", Description = "Máy in, máy photocopy, máy scan", ParentName = "Thiết bị điện tử" },
                new { Name = "Thiết bị âm thanh", Description = "Loa, micro, tai nghe, thiết bị âm thanh", ParentName = "Thiết bị điện tử" },

                // Văn phòng phẩm
                new { Name = "Giấy tờ", Description = "Giấy A4, giấy in, sổ sách", ParentName = "Văn phòng phẩm" },
                new { Name = "Bút viết", Description = "Bút bi, bút chì, bút dạ quang", ParentName = "Văn phòng phẩm" },
                new { Name = "Dụng cụ văn phòng", Description = "Kéo, bấm ghim, băng dính, keo dán", ParentName = "Văn phòng phẩm" },
                new { Name = "Túi đựng", Description = "Túi nhựa, túi giấy, hộp đựng", ParentName = "Văn phòng phẩm" },

                // Nội thất
                new { Name = "Bàn làm việc", Description = "Bàn văn phòng, bàn họp, bàn làm việc", ParentName = "Nội thất" },
                new { Name = "Ghế ngồi", Description = "Ghế văn phòng, ghế họp, ghế xoay", ParentName = "Nội thất" },
                new { Name = "Tủ lưu trữ", Description = "Tủ hồ sơ, tủ tài liệu, kệ sách", ParentName = "Nội thất" },
                new { Name = "Thiết bị chiếu sáng", Description = "Đèn bàn, đèn trần, đèn LED", ParentName = "Nội thất" },

                // Dịch vụ IT
                new { Name = "Phát triển phần mềm", Description = "Lập trình, phát triển ứng dụng", ParentName = "Dịch vụ IT" },
                new { Name = "Bảo trì hệ thống", Description = "Bảo trì máy tính, mạng, server", ParentName = "Dịch vụ IT" },
                new { Name = "Tư vấn IT", Description = "Tư vấn giải pháp công nghệ", ParentName = "Dịch vụ IT" },

                // Dịch vụ tư vấn
                new { Name = "Tư vấn kinh doanh", Description = "Tư vấn chiến lược, quản lý", ParentName = "Dịch vụ tư vấn" },
                new { Name = "Tư vấn pháp lý", Description = "Tư vấn luật, hợp đồng", ParentName = "Dịch vụ tư vấn" },
                new { Name = "Tư vấn tài chính", Description = "Tư vấn đầu tư, kế toán", ParentName = "Dịch vụ tư vấn" },

                // Dịch vụ bảo trì
                new { Name = "Bảo trì thiết bị", Description = "Bảo trì máy móc, thiết bị", ParentName = "Dịch vụ bảo trì" }
            };

            // Cấp 3: 12 danh mục con của cấp 2
            var level3Categories = new[]
            {
                // Máy tính
                new { Name = "Laptop", Description = "Máy tính xách tay", ParentName = "Máy tính" },
                new { Name = "Desktop", Description = "Máy tính để bàn", ParentName = "Máy tính" },
                new { Name = "Server", Description = "Máy chủ", ParentName = "Máy tính" },

                // Thiết bị in ấn
                new { Name = "Máy in laser", Description = "Máy in laser đen trắng, màu", ParentName = "Thiết bị in ấn" },
                new { Name = "Máy in phun", Description = "Máy in phun màu", ParentName = "Thiết bị in ấn" },

                // Giấy tờ
                new { Name = "Giấy A4", Description = "Giấy A4 70g, 80g", ParentName = "Giấy tờ" },
                new { Name = "Sổ sách", Description = "Sổ tay, sổ ghi chép", ParentName = "Giấy tờ" },

                // Bàn làm việc
                new { Name = "Bàn văn phòng", Description = "Bàn làm việc văn phòng", ParentName = "Bàn làm việc" },
                new { Name = "Bàn họp", Description = "Bàn họp, bàn hội nghị", ParentName = "Bàn làm việc" },

                // Phát triển phần mềm
                new { Name = "Web development", Description = "Phát triển website, ứng dụng web", ParentName = "Phát triển phần mềm" },
                new { Name = "Mobile app", Description = "Phát triển ứng dụng di động", ParentName = "Phát triển phần mềm" },

                // Tư vấn kinh doanh
                new { Name = "Chiến lược", Description = "Tư vấn chiến lược kinh doanh", ParentName = "Tư vấn kinh doanh" }
            };

            // Tạo danh mục cấp 1 (ParentId = null)
            var categoryDict = new Dictionary<string, Guid>();
            
            foreach (var cat in level1Categories)
            {
                var category = new ProductServiceCategory
                {
                    Id = Guid.NewGuid(),
                    CategoryName = cat.Name,
                    CategoryCode = GenerateCategoryCode(cat.Name),
                    Description = cat.Description,
                    ParentId = null
                };
                categories.Add(category);
                categoryDict[cat.Name] = category.Id;
            }

            // Tạo danh mục cấp 2
            foreach (var cat in level2Categories)
            {
                if (categoryDict.ContainsKey(cat.ParentName))
                {
                    var category = new ProductServiceCategory
                    {
                        Id = Guid.NewGuid(),
                        CategoryName = cat.Name,
                        CategoryCode = GenerateCategoryCode(cat.Name),
                        Description = cat.Description,
                        ParentId = categoryDict[cat.ParentName]
                    };
                    categories.Add(category);
                    categoryDict[cat.Name] = category.Id;
                }
            }

            // Tạo danh mục cấp 3
            foreach (var cat in level3Categories)
            {
                if (categoryDict.ContainsKey(cat.ParentName))
                {
                    var category = new ProductServiceCategory
                    {
                        Id = Guid.NewGuid(),
                        CategoryName = cat.Name,
                        CategoryCode = GenerateCategoryCode(cat.Name),
                        Description = cat.Description,
                        ParentId = categoryDict[cat.ParentName]
                    };
                    categories.Add(category);
                    categoryDict[cat.Name] = category.Id;
                }
            }

            // Thêm danh mục "Chưa phân loại" (mặc định)
            var uncategorizedCategory = new ProductServiceCategory
            {
                Id = Guid.NewGuid(),
                CategoryName = "Chưa phân loại",
                CategoryCode = "CPL",
                Description = "Danh mục mặc định cho các sản phẩm/dịch vụ chưa được phân loại",
                ParentId = null
            };
            categories.Insert(0, uncategorizedCategory);

            // Thêm vào context
            context.ProductServiceCategories.InsertAllOnSubmit(categories);
        }

        /// <summary>
        /// Tạo dữ liệu mẫu cho UnitOfMeasure
        /// </summary>
        /// <param name="context">DataContext để lưu dữ liệu</param>
        public static void SeedUnitOfMeasures(VnsErp2025DataContext context)
        {
            var units = new List<UnitOfMeasure>();

            var unitData = new[]
            {
                new { Code = "PCS", Name = "Cái", Description = "Đơn vị cái" },
                new { Code = "SET", Name = "Bộ", Description = "Đơn vị bộ" },
                new { Code = "BOX", Name = "Hộp", Description = "Đơn vị hộp" },
                new { Code = "PACK", Name = "Gói", Description = "Đơn vị gói" },
                new { Code = "ROLL", Name = "Cuộn", Description = "Đơn vị cuộn" },
                new { Code = "REAM", Name = "Ram", Description = "Đơn vị ram giấy (500 tờ)" },
                new { Code = "KG", Name = "Kilogram", Description = "Đơn vị khối lượng" },
                new { Code = "G", Name = "Gram", Description = "Đơn vị gram" },
                new { Code = "L", Name = "Lít", Description = "Đơn vị thể tích" },
                new { Code = "ML", Name = "Mililit", Description = "Đơn vị mililit" },
                new { Code = "M", Name = "Mét", Description = "Đơn vị chiều dài" },
                new { Code = "CM", Name = "Centimet", Description = "Đơn vị centimet" },
                new { Code = "M2", Name = "Mét vuông", Description = "Đơn vị diện tích" },
                new { Code = "M3", Name = "Mét khối", Description = "Đơn vị thể tích" },
                new { Code = "HOUR", Name = "Giờ", Description = "Đơn vị thời gian" },
                new { Code = "DAY", Name = "Ngày", Description = "Đơn vị ngày" },
                new { Code = "MONTH", Name = "Tháng", Description = "Đơn vị tháng" },
                new { Code = "YEAR", Name = "Năm", Description = "Đơn vị năm" },
                new { Code = "SERVICE", Name = "Dịch vụ", Description = "Đơn vị dịch vụ" },
                new { Code = "LICENSE", Name = "Giấy phép", Description = "Đơn vị giấy phép" }
            };

            foreach (var unit in unitData)
            {
                var unitOfMeasure = new UnitOfMeasure
                {
                    Id = Guid.NewGuid(),
                    Code = unit.Code,
                    Name = unit.Name,
                    Description = unit.Description,
                    IsActive = true
                };
                units.Add(unitOfMeasure);
            }

            // Thêm vào context
            context.UnitOfMeasures.InsertAllOnSubmit(units);
        }

        /// <summary>
        /// Tạo dữ liệu mẫu cho Attribute và AttributeValue
        /// </summary>
        /// <param name="context">DataContext để lưu dữ liệu</param>
        public static void SeedAttributesAndValues(VnsErp2025DataContext context)
        {
            var attributes = new List<Attribute>();
            var attributeValues = new List<AttributeValue>();
            var random = new Random();

            // Định nghĩa các thuộc tính và giá trị
            var attributeData = new[]
            {
                new
                {
                    Name = "Màu sắc",
                    DataType = "text",
                    Description = "Màu sắc của sản phẩm",
                    Values = new[] { "Đen", "Trắng", "Xám", "Xanh dương", "Đỏ", "Vàng", "Xanh lá", "Tím", "Cam", "Hồng" }
                },
                new
                {
                    Name = "Kích thước",
                    DataType = "text",
                    Description = "Kích thước của sản phẩm",
                    Values = new[] { "XS", "S", "M", "L", "XL", "XXL", "XXXL" }
                },
                new
                {
                    Name = "Chất liệu",
                    DataType = "text",
                    Description = "Chất liệu làm sản phẩm",
                    Values = new[] { "Nhựa", "Kim loại", "Gỗ", "Vải", "Da", "Cao su", "Thủy tinh", "Gốm sứ" }
                },
                new
                {
                    Name = "Công suất",
                    DataType = "number",
                    Description = "Công suất thiết bị (Watt)",
                    Values = new[] { "100W", "200W", "500W", "1000W", "1500W", "2000W", "3000W" }
                },
                new
                {
                    Name = "Dung lượng",
                    DataType = "text",
                    Description = "Dung lượng lưu trữ",
                    Values = new[] { "16GB", "32GB", "64GB", "128GB", "256GB", "512GB", "1TB", "2TB" }
                },
                new
                {
                    Name = "Tốc độ",
                    DataType = "text",
                    Description = "Tốc độ xử lý",
                    Values = new[] { "Chậm", "Trung bình", "Nhanh", "Rất nhanh", "Cực nhanh" }
                },
                new
                {
                    Name = "Loại kết nối",
                    DataType = "text",
                    Description = "Loại kết nối",
                    Values = new[] { "USB", "USB-C", "HDMI", "VGA", "Ethernet", "WiFi", "Bluetooth", "Thunderbolt" }
                },
                new
                {
                    Name = "Hệ điều hành",
                    DataType = "text",
                    Description = "Hệ điều hành",
                    Values = new[] { "Windows", "macOS", "Linux", "Android", "iOS", "Chrome OS" }
                },
                new
                {
                    Name = "Thương hiệu",
                    DataType = "text",
                    Description = "Thương hiệu sản phẩm",
                    Values = new[] { "Dell", "HP", "Lenovo", "Asus", "Acer", "Apple", "Samsung", "Sony", "Canon", "Epson" }
                },
                new
                {
                    Name = "Xuất xứ",
                    DataType = "text",
                    Description = "Nước sản xuất",
                    Values = new[] { "Việt Nam", "Trung Quốc", "Hàn Quốc", "Nhật Bản", "Mỹ", "Đức", "Thái Lan", "Malaysia" }
                }
            };

            foreach (var attrData in attributeData)
            {
                var attribute = new Attribute
                {
                    Id = Guid.NewGuid(),
                    Name = attrData.Name,
                    DataType = attrData.DataType,
                    Description = attrData.Description
                };
                attributes.Add(attribute);

                // Tạo các giá trị cho thuộc tính
                foreach (var value in attrData.Values)
                {
                    var attributeValue = new AttributeValue
                    {
                        Id = Guid.NewGuid(),
                        AttributeId = attribute.Id,
                        Value = value
                    };
                    attributeValues.Add(attributeValue);
                }
            }

            // Thêm vào context
            context.Attributes.InsertAllOnSubmit(attributes);
            context.AttributeValues.InsertAllOnSubmit(attributeValues);
        }

        /// <summary>
        /// Tạo 200 dòng dữ liệu mẫu cho ProductService
        /// </summary>
        /// <param name="context">DataContext để lưu dữ liệu</param>
        public static void SeedProductServices(VnsErp2025DataContext context)
        {
            var products = new List<DataContext.ProductService>();
            var random = new Random();

            // Lấy danh sách categories
            var categories = context.ProductServiceCategories.ToList();
            var leafCategories = categories.Where(c => !categories.Any(child => child.ParentId == c.Id)).ToList();

            if (!leafCategories.Any())
            {
                return; // Không có categories nào để tạo products
            }

            // Danh sách tên sản phẩm mẫu
            var productNames = new[]
            {
                "Máy tính xách tay Dell Inspiron", "Máy in laser HP LaserJet", "Bàn làm việc gỗ cao cấp",
                "Ghế văn phòng ergonomic", "Router WiFi 6", "Màn hình LED 24 inch", "Bàn phím cơ gaming",
                "Chuột không dây", "Webcam HD", "Microphone USB", "Loa Bluetooth", "Tai nghe chống ồn",
                "Máy photocopy đa chức năng", "Máy scan A4", "Máy fax", "Máy tính bảng", "Điện thoại thông minh",
                "Đồng hồ thông minh", "Máy ảnh kỹ thuật số", "Máy quay phim", "Ổ cứng ngoài", "USB flash drive",
                "Thẻ nhớ SD", "Cáp USB-C", "Cáp HDMI", "Adapter nguồn", "Pin dự phòng", "Sạc không dây",
                "Giấy A4 80g", "Bút bi xanh", "Bút chì 2B", "Tẩy", "Thước kẻ", "Compa", "Kéo văn phòng",
                "Bấm ghim", "Băng dính", "Keo dán", "Túi nhựa", "Hộp đựng tài liệu", "Sổ tay", "Lịch để bàn",
                "Bảng trắng", "Bút dạ bảng", "Bảng ghim", "Kẹp tài liệu", "Túi đựng hồ sơ", "Nhãn dán",
                "Tủ hồ sơ 2 ngăn", "Kệ sách 3 tầng", "Đèn bàn LED", "Đèn trần", "Quạt máy", "Máy lọc không khí",
                "Dịch vụ phát triển website", "Dịch vụ bảo trì máy tính", "Dịch vụ tư vấn IT", "Dịch vụ thiết kế logo",
                "Dịch vụ marketing online", "Dịch vụ kế toán", "Dịch vụ tư vấn pháp lý", "Dịch vụ nhân sự",
                "Dịch vụ đào tạo", "Dịch vụ dịch thuật", "Dịch vụ in ấn", "Dịch vụ vận chuyển", "Dịch vụ lưu trữ",
                "Dịch vụ bảo mật", "Dịch vụ backup dữ liệu", "Dịch vụ hosting", "Dịch vụ domain", "Dịch vụ email",
                "Dịch vụ CRM", "Dịch vụ ERP", "Dịch vụ POS", "Dịch vụ thanh toán", "Dịch vụ logistics"
            };

            for (int i = 1; i <= 200; i++)
            {
                var selectedCategory = leafCategories[random.Next(leafCategories.Count)];
                var isService = selectedCategory.CategoryName.Contains("Dịch vụ") || 
                               selectedCategory.CategoryName.Contains("Tư vấn") ||
                               selectedCategory.CategoryName.Contains("Phát triển") ||
                               selectedCategory.CategoryName.Contains("Bảo trì");

                // Tạo mã sản phẩm dựa trên CategoryCode
                var categoryCode = selectedCategory.CategoryCode ?? "SP";
                var productCode = isService ? $"SV{i:D6}" : $"{categoryCode}{i:D4}";

                var product = new DataContext.ProductService
                {
                    Id = Guid.NewGuid(),
                    Code = productCode,
                    Name = productNames[random.Next(productNames.Length)] + $" {i}",
                    CategoryId = selectedCategory.Id,
                    IsService = isService,
                    Description = GenerateProductDescription(isService, random),
                    IsActive = random.Next(0, 10) < 9, // 90% active
                };

                products.Add(product);
            }

            // Thêm vào context
            context.ProductServices.InsertAllOnSubmit(products);
        }

        private static string GenerateProductDescription(bool isService, Random random)
        {
            if (isService)
            {
                var serviceDescriptions = new[]
                {
                    "Dịch vụ chuyên nghiệp với đội ngũ giàu kinh nghiệm",
                    "Giải pháp tối ưu cho nhu cầu kinh doanh",
                    "Hỗ trợ 24/7 với cam kết chất lượng cao",
                    "Dịch vụ uy tín, giá cả hợp lý",
                    "Tư vấn miễn phí và hỗ trợ tận tình"
                };
                return serviceDescriptions[random.Next(serviceDescriptions.Length)];
            }
            else
            {
                var productDescriptions = new[]
                {
                    "Sản phẩm chất lượng cao, bền bỉ",
                    "Thiết kế hiện đại, tiện dụng",
                    "Giá cả hợp lý, phù hợp mọi nhu cầu",
                    "Sản phẩm chính hãng, bảo hành đầy đủ",
                    "Chất lượng tốt, giá thành cạnh tranh"
                };
                return productDescriptions[random.Next(productDescriptions.Length)];
            }
        }

        /// <summary>
        /// Tạo 500 dòng dữ liệu mẫu cho ProductVariant
        /// </summary>
        /// <param name="context">DataContext để lưu dữ liệu</param>
        public static void SeedProductVariants(VnsErp2025DataContext context)
        {
            var variants = new List<ProductVariant>();
            var random = new Random();

            // Lấy danh sách products và units
            var products = context.ProductServices.ToList();
            var units = context.UnitOfMeasures.ToList();

            if (!products.Any() || !units.Any())
            {
                return;
            }

            // Tạo variants cho mỗi product (1-3 variants per product)
            foreach (var product in products)
            {
                var variantCount = random.Next(1, 4); // 1-3 variants
                
                for (int i = 1; i <= variantCount; i++)
                {
                    var unit = units[random.Next(units.Count)];
                    var isService = product.IsService;

                    var variant = new ProductVariant
                    {
                        Id = Guid.NewGuid(),
                        ProductId = product.Id,
                        VariantCode = $"{product.Code}-V{i}",
                        UnitId = unit.Id,
                        PurchasePrice = isService ? null : (decimal?)random.Next(100000, 10000000),
                        SalePrice = isService ? (decimal?)random.Next(500000, 50000000) : (decimal?)random.Next(200000, 20000000),
                        IsActive = random.Next(0, 10) < 9, // 90% active
                    };

                    variants.Add(variant);
                }
            }

            // Thêm vào context
            context.ProductVariants.InsertAllOnSubmit(variants);
        }

        /// <summary>
        /// Tạo dữ liệu mẫu cho VariantAttribute (gắn thuộc tính cho variants)
        /// </summary>
        /// <param name="context">DataContext để lưu dữ liệu</param>
        public static void SeedVariantAttributes(VnsErp2025DataContext context)
        {
            var variantAttributes = new List<VariantAttribute>();
            var random = new Random();

            // Lấy danh sách variants, attributes và attribute values
            var variants = context.ProductVariants.ToList();
            var attributes = context.Attributes.ToList();
            var attributeValues = context.AttributeValues.ToList();

            if (!variants.Any() || !attributes.Any() || !attributeValues.Any())
            {
                return;
            }

            // Gắn thuộc tính cho mỗi variant (2-5 thuộc tính per variant)
            foreach (var variant in variants)
            {
                var attributeCount = random.Next(2, 6); // 2-5 attributes
                var selectedAttributes = attributes.OrderBy(x => random.Next()).Take(attributeCount).ToList();

                foreach (var attribute in selectedAttributes)
                {
                    // Lấy giá trị ngẫu nhiên cho thuộc tính này
                    var availableValues = attributeValues.Where(av => av.AttributeId == attribute.Id).ToList();
                    if (availableValues.Any())
                    {
                        var selectedValue = availableValues[random.Next(availableValues.Count)];

                        var variantAttribute = new VariantAttribute
                        {
                            VariantId = variant.Id,
                            AttributeId = attribute.Id,
                            AttributeValueId = selectedValue.Id
                        };

                        variantAttributes.Add(variantAttribute);
                    }
                }
            }

            // Thêm vào context
            context.VariantAttributes.InsertAllOnSubmit(variantAttributes);
        }

        /// <summary>
        /// Tạo 1000 dòng dữ liệu mẫu cho ProductImage
        /// </summary>
        /// <param name="context">DataContext để lưu dữ liệu</param>
        public static void SeedProductImages(VnsErp2025DataContext context)
        {
            var images = new List<ProductImage>();
            var random = new Random();

            // Lấy danh sách products và variants
            var products = context.ProductServices.ToList();
            var variants = context.ProductVariants.ToList();

            if (!products.Any())
            {
                return;
            }

            // Tạo images cho products và variants
            for (int i = 1; i <= 1000; i++)
            {
                var isVariantImage = random.Next(0, 2) == 1 && variants.Any();
                var product = products[random.Next(products.Count)];
                var variant = isVariantImage ? variants.Where(v => v.ProductId == product.Id).FirstOrDefault() : null;

                // Chỉ tạo image cho product nếu không phải service
                if (product.IsService && !isVariantImage)
                {
                    continue;
                }

                var image = new ProductImage
                {
                    Id = Guid.NewGuid(),
                    ProductId = isVariantImage ? null : product.Id,
                    VariantId = isVariantImage ? variant?.Id : null,
                    ImagePath = $"/images/products/{product.Code}_{i}.jpg",
                    SortOrder = random.Next(1, 10),
                    IsPrimary = random.Next(0, 10) < 2 // 20% là ảnh chính
                };

                images.Add(image);
            }

            // Thêm vào context
            context.ProductImages.InsertAllOnSubmit(images);
        }

        /// <summary>
        /// Tạo tất cả dữ liệu mẫu (ProductServiceCategory, UnitOfMeasure, Attribute, AttributeValue, ProductService, ProductVariant, VariantAttribute, ProductImage)
        /// Sử dụng connection string từ ConfigurationManager (recommended)
        /// </summary>
        public static void SeedAllData()
        {
            using (var context = CreateContext())
            {
                SeedAllData(context);
            }
        }

        /// <summary>
        /// Tạo tất cả dữ liệu mẫu sử dụng connection string được truyền vào
        /// </summary>
        /// <param name="connectionString">Connection string để kết nối database</param>
        public static void SeedAllData(string connectionString)
        {
            using (var context = new VnsErp2025DataContext(connectionString))
            {
                SeedAllData(context);
            }
        }

        /// <summary>
        /// Xóa tất cả dữ liệu trong các entity ProductService theo thứ tự ngược lại để tránh foreign key constraint
        /// </summary>
        /// <param name="context">DataContext để xóa dữ liệu</param>
        /// <returns>Dictionary chứa thông tin số lượng dữ liệu đã xóa</returns>
        public static Dictionary<string, int> DeleteAllProductServiceData(VnsErp2025DataContext context)
        {
            try
            {
                // Lấy số lượng dữ liệu trước khi xóa
                var countsBefore = GetProductServiceDataCounts(context);
                
                // Xóa dữ liệu theo thứ tự ngược lại để tránh foreign key constraint
                context.ProductImages.DeleteAllOnSubmit(context.ProductImages);
                context.VariantAttributes.DeleteAllOnSubmit(context.VariantAttributes);
                context.ProductVariants.DeleteAllOnSubmit(context.ProductVariants);
                context.ProductServices.DeleteAllOnSubmit(context.ProductServices);
                context.AttributeValues.DeleteAllOnSubmit(context.AttributeValues);
                context.Attributes.DeleteAllOnSubmit(context.Attributes);
                context.UnitOfMeasures.DeleteAllOnSubmit(context.UnitOfMeasures);
                context.ProductServiceCategories.DeleteAllOnSubmit(context.ProductServiceCategories);
                
                context.SubmitChanges();

                return countsBefore;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa dữ liệu ProductService: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Xóa tất cả dữ liệu trong các entity ProductService sử dụng connection string từ ConfigurationManager
        /// </summary>
        /// <returns>Dictionary chứa thông tin số lượng dữ liệu đã xóa</returns>
        public static Dictionary<string, int> DeleteAllProductServiceData()
        {
            using (var context = CreateContext())
            {
                return DeleteAllProductServiceData(context);
            }
        }

        /// <summary>
        /// Xóa tất cả dữ liệu trong các entity ProductService sử dụng connection string được truyền vào
        /// </summary>
        /// <param name="connectionString">Connection string để kết nối database</param>
        /// <returns>Dictionary chứa thông tin số lượng dữ liệu đã xóa</returns>
        public static Dictionary<string, int> DeleteAllProductServiceData(string connectionString)
        {
            using (var context = new VnsErp2025DataContext(connectionString))
            {
                return DeleteAllProductServiceData(context);
            }
        }

        /// <summary>
        /// Lấy thông tin số lượng dữ liệu trong các entity ProductService
        /// </summary>
        /// <param name="context">DataContext để đếm dữ liệu</param>
        /// <returns>Dictionary chứa tên entity và số lượng records</returns>
        public static Dictionary<string, int> GetProductServiceDataCounts(VnsErp2025DataContext context)
        {
            var counts = new Dictionary<string, int>();
            
            try
            {
                counts["ProductImages"] = context.ProductImages.Count();
                counts["VariantAttributes"] = context.VariantAttributes.Count();
                counts["ProductVariants"] = context.ProductVariants.Count();
                counts["ProductServices"] = context.ProductServices.Count();
                counts["AttributeValues"] = context.AttributeValues.Count();
                counts["Attributes"] = context.Attributes.Count();
                counts["UnitOfMeasures"] = context.UnitOfMeasures.Count();
                counts["ProductServiceCategories"] = context.ProductServiceCategories.Count();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi đếm dữ liệu ProductService: {ex.Message}", ex);
            }
            
            return counts;
        }

        /// <summary>
        /// Lấy thông tin số lượng dữ liệu trong các entity ProductService sử dụng connection string từ ConfigurationManager
        /// </summary>
        /// <returns>Dictionary chứa tên entity và số lượng records</returns>
        public static Dictionary<string, int> GetProductServiceDataCounts()
        {
            using (var context = CreateContext())
            {
                return GetProductServiceDataCounts(context);
            }
        }

        /// <summary>
        /// Lấy thông tin số lượng dữ liệu trong các entity ProductService sử dụng connection string được truyền vào
        /// </summary>
        /// <param name="connectionString">Connection string để kết nối database</param>
        /// <returns>Dictionary chứa tên entity và số lượng records</returns>
        public static Dictionary<string, int> GetProductServiceDataCounts(string connectionString)
        {
            using (var context = new VnsErp2025DataContext(connectionString))
            {
                return GetProductServiceDataCounts(context);
            }
        }

        /// <summary>
        /// Tạo tất cả dữ liệu mẫu sử dụng context có sẵn
        /// </summary>
        /// <param name="context">DataContext để lưu dữ liệu</param>
        public static void SeedAllData(VnsErp2025DataContext context)
        {
            // Xóa dữ liệu cũ (nếu có) theo thứ tự ngược lại để tránh foreign key constraint
            var deletedCounts = DeleteAllProductServiceData(context);

            // Tạo dữ liệu mới theo thứ tự đúng
            SeedProductServiceCategories(context);
            context.SubmitChanges();

            SeedUnitOfMeasures(context);
            context.SubmitChanges();

            SeedAttributesAndValues(context);
            context.SubmitChanges();

            SeedProductServices(context);
            context.SubmitChanges();

            SeedProductVariants(context);
            context.SubmitChanges();

            SeedVariantAttributes(context);
            context.SubmitChanges();

            SeedProductImages(context);
            context.SubmitChanges();
        }
    }
}
