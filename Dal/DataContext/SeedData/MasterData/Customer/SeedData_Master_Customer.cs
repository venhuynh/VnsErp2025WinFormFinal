using Dal.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dal.DataContext.SeedData.MasterData.Customer
{
    public static class SeedData_Master_Customer
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
        /// Tạo 100 dòng dữ liệu mẫu cho BusinessPartner
        /// </summary>
        /// <param name="context">DataContext để lưu dữ liệu</param>
        public static void SeedBusinessPartners(VnsErp2025DataContext context)
        {
            var businessPartners = new List<BusinessPartner>();
            var random = new Random();

            // Danh sách tên công ty mẫu
            var companyNames = new[]
            {
                "Công ty TNHH", "Công ty Cổ phần", "Doanh nghiệp tư nhân", "Công ty liên doanh",
                "Tập đoàn", "Tổng công ty", "Công ty trách nhiệm hữu hạn một thành viên"
            };

            var businessTypes = new[]
            {
                "Thương mại", "Sản xuất", "Dịch vụ", "Xuất nhập khẩu", "Bất động sản",
                "Công nghệ thông tin", "Tài chính", "Vận tải", "Xây dựng", "Nông nghiệp"
            };

            var cities = new[]
            {
                "Hà Nội", "TP. Hồ Chí Minh", "Đà Nẵng", "Hải Phòng", "Cần Thơ",
                "An Giang", "Bà Rịa - Vũng Tàu", "Bắc Giang", "Bắc Kạn", "Bạc Liêu",
                "Bắc Ninh", "Bến Tre", "Bình Định", "Bình Dương", "Bình Phước",
                "Bình Thuận", "Cà Mau", "Cao Bằng", "Đắk Lắk", "Đắk Nông"
            };

            var countries = new[]
            {
                "Việt Nam", "Trung Quốc", "Hàn Quốc", "Nhật Bản", "Thái Lan",
                "Singapore", "Malaysia", "Philippines", "Indonesia", "Mỹ",
                "Đức", "Pháp", "Anh", "Ý", "Tây Ban Nha"
            };

            var bankNames = new[]
            {
                "Vietcombank", "BIDV", "VietinBank", "Agribank", "Techcombank",
                "MB Bank", "ACB", "VPBank", "TPBank", "HDBank",
                "Sacombank", "VIB", "MSB", "SeABank", "Eximbank"
            };

            var paymentTerms = new[]
            {
                "Thanh toán ngay", "Thanh toán 30 ngày", "Thanh toán 60 ngày", "Thanh toán 90 ngày",
                "Thanh toán trước", "Thanh toán sau", "Thanh toán theo tiến độ", "Thanh toán cuối tháng"
            };

            for (int i = 1; i <= 100; i++)
            {
                var partnerType = random.Next(1, 4); // 1: Khách hàng, 2: Nhà cung cấp, 3: Cả hai
                var hasTaxCode = partnerType == 1 && random.Next(0, 2) == 1; // Khách hàng có thể có hoặc không có mã số thuế
                
                var businessPartner = new BusinessPartner
                {
                    Id = Guid.NewGuid(),
                    PartnerCode = $"BP{i:D6}", // BP000001, BP000002, ...
                    PartnerName = GeneratePartnerName(companyNames, businessTypes, i, random),
                    PartnerType = partnerType,
                    TaxCode = hasTaxCode ? GenerateTaxCode(random) : null,
                    Phone = GeneratePhoneNumber(random),
                    Email = GenerateEmail(i, random),
                    Website = random.Next(0, 2) == 1 ? GenerateWebsite(i, random) : null,
                    Address = GenerateAddress(random),
                    City = cities[random.Next(cities.Length)],
                    Country = countries[random.Next(countries.Length)],
                    
                    IsActive = random.Next(0, 10) < 9, // 90% active
                    CreatedDate = DateTime.Now.AddDays(-random.Next(0, 365)),
                    UpdatedDate = random.Next(0, 2) == 1 ? DateTime.Now.AddDays(-random.Next(0, 30)) : (DateTime?)null
                };

                businessPartners.Add(businessPartner);
            }

            // Thêm vào context
            context.BusinessPartners.InsertAllOnSubmit(businessPartners);
        }

        private static string GeneratePartnerName(string[] companyTypes, string[] businessTypes, int index, Random random)
        {
            var companyType = companyTypes[random.Next(companyTypes.Length)];
            var businessType = businessTypes[random.Next(businessTypes.Length)];
            
            var namePrefixes = new[]
            {
                "Anh", "Bình", "Cao", "Đông", "Gia", "Hưng", "Khang", "Long", "Minh", "Nam",
                "Phúc", "Quang", "Sơn", "Thành", "Uy", "Việt", "Xuân", "Yên", "Zin", "Alpha"
            };

            var nameSuffixes = new[]
            {
                "Phát", "Thịnh", "Vượng", "Minh", "Hưng", "Thành", "Phúc", "An", "Bình", "Cường",
                "Đạt", "Giang", "Hòa", "Khánh", "Lộc", "Mạnh", "Nghĩa", "Oanh", "Phong", "Quý"
            };

            var prefix = namePrefixes[random.Next(namePrefixes.Length)];
            var suffix = nameSuffixes[random.Next(nameSuffixes.Length)];
            
            return $"{companyType} {prefix} {suffix} - {businessType}";
        }

        private static string GenerateTaxCode(Random random)
        {
            // Tạo mã số thuế 10 số
            var taxCode = "";
            for (int i = 0; i < 10; i++)
            {
                taxCode += random.Next(0, 10);
            }
            return taxCode;
        }

        private static string GeneratePhoneNumber(Random random)
        {
            var prefixes = new[] { "024", "028", "0236", "0234", "0232", "0231", "0292", "0296", "0297" };
            var prefix = prefixes[random.Next(prefixes.Length)];
            var number = random.Next(1000000, 9999999);
            return $"{prefix}{number}";
        }

        private static string GenerateEmail(int index, Random random)
        {
            var domains = new[] { "gmail.com", "yahoo.com", "hotmail.com", "company.com", "business.vn" };
            var domain = domains[random.Next(domains.Length)];
            return $"partner{index:D3}@{domain}";
        }

        private static string GenerateWebsite(int index, Random random)
        {
            var domains = new[] { "com", "com.vn", "vn", "net", "org" };
            var domain = domains[random.Next(domains.Length)];
            return $"www.partner{index:D3}.{domain}";
        }

        private static string GenerateAddress(Random random)
        {
            var streets = new[]
            {
                "Nguyễn Văn Linh", "Lê Văn Việt", "Trường Chinh", "Cộng Hòa", "Nguyễn Thị Minh Khai",
                "Hai Bà Trưng", "Lý Thường Kiệt", "Trần Hưng Đạo", "Lê Lợi", "Quang Trung",
                "Nguyễn Du", "Nguyễn Huệ", "Điện Biên Phủ", "Hoàng Văn Thụ", "Võ Văn Tần"
            };

            var street = streets[random.Next(streets.Length)];
            var number = random.Next(1, 999);
            return $"{number}, đường {street}";
        }

        private static string GenerateContactPerson(Random random)
        {
            var firstNames = new[]
            {
                "Nguyễn", "Trần", "Lê", "Phạm", "Hoàng", "Phan", "Vũ", "Võ", "Đặng", "Bùi",
                "Đỗ", "Hồ", "Ngô", "Dương", "Lý", "An", "Bình", "Cường", "Đức", "Giang"
            };

            var lastNames = new[]
            {
                "Văn An", "Thị Bình", "Văn Cường", "Thị Dung", "Văn Đức", "Thị Em", "Văn Giang",
                "Thị Hoa", "Văn Khang", "Thị Lan", "Văn Minh", "Thị Nga", "Văn Oanh", "Thị Phương",
                "Văn Quang", "Thị Rinh", "Văn Sơn", "Thị Tuyết", "Văn Uy", "Thị Vy"
            };

            var firstName = firstNames[random.Next(firstNames.Length)];
            var lastName = lastNames[random.Next(lastNames.Length)];
            return $"{firstName} {lastName}";
        }

        private static string GenerateContactPosition(Random random)
        {
            var positions = new[]
            {
                "Giám đốc", "Phó giám đốc", "Trưởng phòng", "Phó trưởng phòng", "Nhân viên",
                "Kế toán trưởng", "Trưởng phòng kinh doanh", "Trưởng phòng kỹ thuật", "Chuyên viên",
                "Thư ký", "Trợ lý", "Quản lý", "Điều hành", "Phụ trách", "Phụ trách bán hàng"
            };

            return positions[random.Next(positions.Length)];
        }

        private static string GenerateBankAccount(Random random)
        {
            // Tạo số tài khoản ngân hàng
            var account = "";
            for (int i = 0; i < 12; i++)
            {
                account += random.Next(0, 10);
            }
            return account;
        }

        /// <summary>
        /// Tạo 30 dòng dữ liệu mẫu cho BusinessPartnerCategory với 3 cấp phân cấp
        /// </summary>
        /// <param name="context">DataContext để lưu dữ liệu</param>
        public static void SeedBusinessPartnerCategories(VnsErp2025DataContext context)
        {
            var categories = new List<BusinessPartnerCategory>();
            var random = new Random();

            // Cấp 1: 5 danh mục chính
            var level1Categories = new[]
            {
                new { Name = "Khách hàng doanh nghiệp", Description = "Các doanh nghiệp, công ty mua hàng hóa/dịch vụ" },
                new { Name = "Nhà cung cấp", Description = "Các nhà cung cấp nguyên vật liệu, hàng hóa, dịch vụ" },
                new { Name = "Đối tác chiến lược", Description = "Các đối tác hợp tác lâu dài, liên doanh" },
                new { Name = "Khách hàng cá nhân", Description = "Khách hàng mua lẻ, cá nhân" },
                new { Name = "Đối tác tài chính", Description = "Ngân hàng, tổ chức tài chính, nhà đầu tư" }
            };

            // Cấp 2: 15 danh mục con
            var level2Categories = new[]
            {
                // Khách hàng doanh nghiệp
                new { Name = "Nhà bán lẻ", Description = "Các cửa hàng, siêu thị bán lẻ", ParentName = "Khách hàng doanh nghiệp" },
                new { Name = "Nhà bán buôn", Description = "Các nhà phân phối, bán buôn", ParentName = "Khách hàng doanh nghiệp" },
                new { Name = "Đại lý phân phối", Description = "Hệ thống đại lý phân phối sản phẩm", ParentName = "Khách hàng doanh nghiệp" },
                new { Name = "Khách hàng tiềm năng", Description = "Khách hàng có khả năng mua hàng cao", ParentName = "Khách hàng doanh nghiệp" },

                // Nhà cung cấp
                new { Name = "Nhà cung cấp nguyên vật liệu", Description = "Cung cấp nguyên liệu thô cho sản xuất", ParentName = "Nhà cung cấp" },
                new { Name = "Nhà cung cấp thiết bị", Description = "Cung cấp máy móc, thiết bị sản xuất", ParentName = "Nhà cung cấp" },
                new { Name = "Nhà cung cấp dịch vụ", Description = "Cung cấp các dịch vụ hỗ trợ kinh doanh", ParentName = "Nhà cung cấp" },
                new { Name = "Nhà cung cấp công nghệ", Description = "Cung cấp giải pháp công nghệ thông tin", ParentName = "Nhà cung cấp" },
                new { Name = "Nhà cung cấp năng lượng", Description = "Cung cấp điện, nước, gas, nhiên liệu", ParentName = "Nhà cung cấp" },

                // Đối tác chiến lược
                new { Name = "Đối tác logistics", Description = "Hợp tác vận chuyển, kho bãi", ParentName = "Đối tác chiến lược" },
                new { Name = "Đối tác marketing", Description = "Hợp tác quảng cáo, truyền thông", ParentName = "Đối tác chiến lược" },
                new { Name = "Đối tác nghiên cứu", Description = "Hợp tác nghiên cứu và phát triển", ParentName = "Đối tác chiến lược" },

                // Khách hàng cá nhân
                new { Name = "Khách hàng VIP", Description = "Khách hàng cá nhân có giá trị cao", ParentName = "Khách hàng cá nhân" },
                new { Name = "Khách hàng thường", Description = "Khách hàng cá nhân mua hàng thông thường", ParentName = "Khách hàng cá nhân" },

                // Đối tác tài chính
                new { Name = "Ngân hàng thương mại", Description = "Các ngân hàng cung cấp dịch vụ tài chính", ParentName = "Đối tác tài chính" }
            };

            // Cấp 3: 10 danh mục con của cấp 2
            var level3Categories = new[]
            {
                // Nhà bán lẻ
                new { Name = "Siêu thị", Description = "Các siêu thị lớn, chuỗi bán lẻ", ParentName = "Nhà bán lẻ" },
                new { Name = "Cửa hàng tiện lợi", Description = "Cửa hàng tiện lợi, mini mart", ParentName = "Nhà bán lẻ" },

                // Nhà cung cấp công nghệ
                new { Name = "Nhà cung cấp phần mềm", Description = "Cung cấp phần mềm, ứng dụng", ParentName = "Nhà cung cấp công nghệ" },
                new { Name = "Nhà cung cấp phần cứng", Description = "Cung cấp máy tính, thiết bị IT", ParentName = "Nhà cung cấp công nghệ" },

                // Nhà cung cấp dịch vụ
                new { Name = "Nhà cung cấp dịch vụ kế toán", Description = "Dịch vụ kế toán, kiểm toán", ParentName = "Nhà cung cấp dịch vụ" },
                new { Name = "Nhà cung cấp dịch vụ pháp lý", Description = "Dịch vụ tư vấn pháp luật", ParentName = "Nhà cung cấp dịch vụ" },

                // Đối tác logistics
                new { Name = "Nhà vận chuyển đường bộ", Description = "Vận chuyển bằng xe tải, container", ParentName = "Đối tác logistics" },
                new { Name = "Nhà vận chuyển đường biển", Description = "Vận chuyển bằng tàu biển", ParentName = "Đối tác logistics" },

                // Khách hàng VIP
                new { Name = "Khách hàng VIP Kim cương", Description = "Khách hàng VIP cấp cao nhất", ParentName = "Khách hàng VIP" },
                new { Name = "Khách hàng VIP Vàng", Description = "Khách hàng VIP cấp cao", ParentName = "Khách hàng VIP" }
            };

            // Tạo danh mục cấp 1 (ParentId = null)
            var categoryDict = new Dictionary<string, Guid>();
            
            foreach (var cat in level1Categories)
            {
                var category = new BusinessPartnerCategory
                {
                    Id = Guid.NewGuid(),
                    CategoryName = cat.Name,
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
                    var category = new BusinessPartnerCategory
                    {
                        Id = Guid.NewGuid(),
                        CategoryName = cat.Name,
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
                    var category = new BusinessPartnerCategory
                    {
                        Id = Guid.NewGuid(),
                        CategoryName = cat.Name,
                        Description = cat.Description,
                        ParentId = categoryDict[cat.ParentName]
                    };
                    categories.Add(category);
                    categoryDict[cat.Name] = category.Id;
                }
            }

            // Thêm danh mục "Chưa phân loại" (mặc định)
            var uncategorizedCategory = new BusinessPartnerCategory
            {
                Id = Guid.NewGuid(),
                CategoryName = "Chưa phân loại",
                Description = "Danh mục mặc định cho các đối tác chưa được phân loại",
                ParentId = null
            };
            categories.Insert(0, uncategorizedCategory); // Thêm vào đầu danh sách

            // Thêm vào context
            context.BusinessPartnerCategories.InsertAllOnSubmit(categories);
        }

        /// <summary>
        /// Tạo dữ liệu mapping giữa BusinessPartner và BusinessPartnerCategory
        /// BusinessPartner chỉ được mapping vào các node con cuối cùng (leaf nodes) và có thể thuộc nhiều node
        /// </summary>
        /// <param name="context">DataContext để lưu dữ liệu</param>
        public static void SeedBusinessPartnerMappings(VnsErp2025DataContext context)
        {
            var mappings = new List<BusinessPartner_BusinessPartnerCategory>();
            var random = new Random();

            // Lấy danh sách categories
            var categories = context.BusinessPartnerCategories.ToList();
            var categoryDict = categories.ToDictionary(c => c.CategoryName, c => c.Id);

            // Tìm các leaf nodes (node không có con)
            var leafNodes = categories.Where(c => !categories.Any(child => child.ParentId == c.Id)).ToList();
            var leafNodeDict = leafNodes.ToDictionary(c => c.CategoryName, c => c.Id);

            // Lấy danh sách partners
            var partners = context.BusinessPartners.ToList();

            // Mapping logic dựa trên PartnerType
            foreach (var partner in partners)
            {
                var partnerMappings = new List<Guid>(); // Một partner có thể thuộc nhiều category

                switch (partner.PartnerType)
                {
                    case 1: // Khách hàng
                        if (partner.TaxCode != null) // Có mã số thuế = doanh nghiệp
                        {
                            // 60% vào 1 category, 30% vào 2 categories, 10% vào 3 categories
                            var mappingCount = GetRandomMappingCount(random);
                            
                            var customerLeafNodes = new[]
                            {
                                "Siêu thị", "Cửa hàng tiện lợi", "Nhà bán buôn", "Đại lý phân phối", "Khách hàng tiềm năng"
                            }.Where(name => leafNodeDict.ContainsKey(name)).ToArray();

                            for (int i = 0; i < Math.Min(mappingCount, customerLeafNodes.Length); i++)
                            {
                                var selectedCategory = customerLeafNodes[random.Next(customerLeafNodes.Length)];
                                if (!partnerMappings.Contains(leafNodeDict[selectedCategory]))
                                {
                                    partnerMappings.Add(leafNodeDict[selectedCategory]);
                                }
                            }
                        }
                        else // Không có mã số thuế = cá nhân
                        {
                            // 70% vào 1 category, 20% vào 2 categories, 10% vào 3 categories
                            var mappingCount = GetRandomMappingCount(random);
                            
                            var individualLeafNodes = new[]
                            {
                                "Khách hàng VIP Kim cương", "Khách hàng VIP Vàng", "Khách hàng thường"
                            }.Where(name => leafNodeDict.ContainsKey(name)).ToArray();

                            for (int i = 0; i < Math.Min(mappingCount, individualLeafNodes.Length); i++)
                            {
                                var selectedCategory = individualLeafNodes[random.Next(individualLeafNodes.Length)];
                                if (!partnerMappings.Contains(leafNodeDict[selectedCategory]))
                                {
                                    partnerMappings.Add(leafNodeDict[selectedCategory]);
                                }
                            }
                        }
                        break;

                    case 2: // Nhà cung cấp
                        // 50% vào 1 category, 30% vào 2 categories, 20% vào 3 categories
                        var supplierMappingCount = GetRandomMappingCount(random);
                        
                        var supplierLeafNodes = new[]
                        {
                            "Nhà cung cấp phần mềm", "Nhà cung cấp phần cứng", "Nhà cung cấp dịch vụ kế toán",
                            "Nhà cung cấp dịch vụ pháp lý", "Nhà cung cấp nguyên vật liệu", "Nhà cung cấp thiết bị",
                            "Nhà cung cấp năng lượng"
                        }.Where(name => leafNodeDict.ContainsKey(name)).ToArray();

                        for (int i = 0; i < Math.Min(supplierMappingCount, supplierLeafNodes.Length); i++)
                        {
                            var selectedCategory = supplierLeafNodes[random.Next(supplierLeafNodes.Length)];
                            if (!partnerMappings.Contains(leafNodeDict[selectedCategory]))
                            {
                                partnerMappings.Add(leafNodeDict[selectedCategory]);
                            }
                        }
                        break;

                    case 3: // Cả hai
                        // 40% vào 1 category, 40% vào 2 categories, 20% vào 3 categories
                        var bothMappingCount = GetRandomMappingCount(random);
                        
                        var bothLeafNodes = new[]
                        {
                            "Nhà vận chuyển đường bộ", "Nhà vận chuyển đường biển", "Đối tác marketing",
                            "Đối tác nghiên cứu", "Ngân hàng thương mại"
                        }.Where(name => leafNodeDict.ContainsKey(name)).ToArray();

                        for (int i = 0; i < Math.Min(bothMappingCount, bothLeafNodes.Length); i++)
                        {
                            var selectedCategory = bothLeafNodes[random.Next(bothLeafNodes.Length)];
                            if (!partnerMappings.Contains(leafNodeDict[selectedCategory]))
                            {
                                partnerMappings.Add(leafNodeDict[selectedCategory]);
                            }
                        }
                        break;

                    default:
                        // Mặc định vào "Chưa phân loại" (nếu là leaf node)
                        if (leafNodeDict.ContainsKey("Chưa phân loại"))
                        {
                            partnerMappings.Add(leafNodeDict["Chưa phân loại"]);
                        }
                        break;
                }

                // Nếu không có mapping nào phù hợp, gán vào "Chưa phân loại"
                if (partnerMappings.Count == 0)
                {
                    if (leafNodeDict.ContainsKey("Chưa phân loại"))
                    {
                        partnerMappings.Add(leafNodeDict["Chưa phân loại"]);
                    }
                    else
                    {
                        // Nếu không có "Chưa phân loại", gán vào một leaf node bất kỳ
                        if (leafNodes.Any())
                        {
                            partnerMappings.Add(leafNodes[random.Next(leafNodes.Count)].Id);
                        }
                    }
                }

                // Tạo mappings cho partner
                foreach (var categoryId in partnerMappings)
                {
                    var mapping = new BusinessPartner_BusinessPartnerCategory
                    {
                        PartnerId = partner.Id,
                        CategoryId = categoryId
                    };
                    mappings.Add(mapping);
                }
            }

            // Thêm vào context
            context.BusinessPartner_BusinessPartnerCategories.InsertAllOnSubmit(mappings);
        }

        /// <summary>
        /// Tạo số lượng mapping ngẫu nhiên cho một partner
        /// </summary>
        /// <param name="random">Random generator</param>
        /// <returns>Số lượng mapping (1-3)</returns>
        private static int GetRandomMappingCount(Random random)
        {
            var rand = random.Next(0, 100);
            if (rand < 50) return 1;      // 50% - 1 mapping
            if (rand < 80) return 2;      // 30% - 2 mappings
            return 3;                     // 20% - 3 mappings
        }

        /// <summary>
        /// Tạo 200 dữ liệu mẫu cho BusinessPartnerSite
        /// Mối quan hệ 1-nhiều: Một BusinessPartner có thể có nhiều sites
        /// </summary>
        /// <param name="context">DataContext để lưu dữ liệu</param>
        public static void SeedBusinessPartnerSites(VnsErp2025DataContext context)
        {
            var sites = new List<BusinessPartnerSite>();
            var random = new Random();

            // Lấy danh sách partners
            var partners = context.BusinessPartners.ToList();
            
            if (!partners.Any())
            {
                return; // Không có partners nào để tạo sites
            }

            var provinces = new[]
            {
                "Hà Nội", "TP. Hồ Chí Minh", "Đà Nẵng", "Hải Phòng", "Cần Thơ",
                "An Giang", "Bà Rịa - Vũng Tàu", "Bắc Giang", "Bắc Kạn", "Bạc Liêu",
                "Bắc Ninh", "Bến Tre", "Bình Định", "Bình Dương", "Bình Phước",
                "Bình Thuận", "Cà Mau", "Cao Bằng", "Đắk Lắk", "Đắk Nông"
            };

            var countries = new[]
            {
                "Việt Nam", "Trung Quốc", "Hàn Quốc", "Nhật Bản", "Thái Lan",
                "Singapore", "Malaysia", "Philippines", "Indonesia", "Mỹ"
            };

            // Tạo 200 sites
            for (int i = 1; i <= 200; i++)
            {
                // Chọn partner ngẫu nhiên
                var selectedPartner = partners[random.Next(partners.Count)];
                
                // Kiểm tra xem partner này đã có bao nhiêu sites
                var existingSitesCount = context.BusinessPartnerSites.Count(s => s.PartnerId == selectedPartner.Id);
                
                // Tạo site mới
                var isDefault = existingSitesCount == 0; // Nếu là site đầu tiên của partner thì là default
                
                var site = new BusinessPartnerSite
                {
                    Id = Guid.NewGuid(),
                    PartnerId = selectedPartner.Id,
                    SiteCode = $"{selectedPartner.PartnerCode}S{i:D2}", // BP000001S01, BP000001S02, ...
                    SiteName = $"{selectedPartner.PartnerName} - Chi nhánh {i}",
                    Address = GenerateAddress(random),
                    City = provinces[random.Next(provinces.Length)],
                    Province = provinces[random.Next(provinces.Length)],
                    Country = countries[random.Next(countries.Length)],
                    ContactPerson = GenerateContactPerson(random),
                    Phone = GeneratePhoneNumber(random),
                    Email = GenerateContactEmail(selectedPartner.PartnerCode, i, random),
                    IsDefault = isDefault,
                    IsActive = random.Next(0, 10) < 9, // 90% active
                    CreatedDate = DateTime.Now.AddDays(-random.Next(0, 365)),
                    UpdatedDate = random.Next(0, 2) == 1 ? DateTime.Now.AddDays(-random.Next(0, 30)) : (DateTime?)null
                };
                
                sites.Add(site);
            }

            // Thêm vào context
            context.BusinessPartnerSites.InsertAllOnSubmit(sites);
        }

        /// <summary>
        /// Tạo 500 dữ liệu mẫu cho BusinessPartnerContact
        /// Mối quan hệ 1-nhiều: Một BusinessPartnerSite có thể có nhiều contacts
        /// </summary>
        /// <param name="context">DataContext để lưu dữ liệu</param>
        public static void SeedBusinessPartnerContacts(VnsErp2025DataContext context)
        {
            var contacts = new List<BusinessPartnerContact>();
            var random = new Random();

            // Lấy danh sách sites
            var sites = context.BusinessPartnerSites.ToList();
            
            if (!sites.Any())
            {
                return; // Không có sites nào để tạo contacts
            }

            // Tạo 500 contacts
            for (int i = 1; i <= 500; i++)
            {
                // Chọn site ngẫu nhiên
                var selectedSite = sites[random.Next(sites.Count)];
                
                // Kiểm tra xem site này đã có bao nhiêu contacts
                var existingContactsCount = context.BusinessPartnerContacts.Count(c => c.SiteId == selectedSite.Id);
                
                // Tạo contact mới
                var isPrimary = existingContactsCount == 0; // Nếu là contact đầu tiên của site thì là primary
                
                var contact = new BusinessPartnerContact
                {
                    Id = Guid.NewGuid(),
                    SiteId = selectedSite.Id,
                    FullName = GenerateContactPerson(random),
                    Position = GenerateContactPosition(random),
                    Phone = GeneratePhoneNumber(random),
                    Email = GenerateContactEmail(selectedSite.SiteCode, existingContactsCount + 1, random),
                    IsPrimary = isPrimary,
                    // Không có avatar mặc định (AvatarThumbnailData và các metadata fields sẽ là null)
                    IsActive = random.Next(0, 10) < 9 // 90% active
                };
                
                contacts.Add(contact);
            }

            // Thêm vào context
            context.BusinessPartnerContacts.InsertAllOnSubmit(contacts);
        }

        private static string GenerateContactEmail(string partnerCode, int contactIndex, Random random)
        {
            var domains = new[] { "gmail.com", "yahoo.com", "hotmail.com", "company.com", "business.vn" };
            var domain = domains[random.Next(domains.Length)];
            return $"contact{contactIndex}@{partnerCode.ToLower()}.{domain}";
        }

        /// <summary>
        /// Tạo tất cả dữ liệu mẫu (BusinessPartner, BusinessPartnerCategory, Mappings, Sites, Contacts)
        /// Sử dụng connection string từ ConfigurationManager (recommended)
        /// </summary>
        public static void SeedAllData()
        {
            using (var context = CreateContext())
            {
                // Xóa dữ liệu cũ (nếu có) theo thứ tự ngược lại để tránh foreign key constraint
                context.BusinessPartnerContacts.DeleteAllOnSubmit(context.BusinessPartnerContacts);
                context.BusinessPartnerSites.DeleteAllOnSubmit(context.BusinessPartnerSites);
                context.BusinessPartner_BusinessPartnerCategories.DeleteAllOnSubmit(context.BusinessPartner_BusinessPartnerCategories);
                context.BusinessPartners.DeleteAllOnSubmit(context.BusinessPartners);
                context.BusinessPartnerCategories.DeleteAllOnSubmit(context.BusinessPartnerCategories);
                context.SubmitChanges();

                // Tạo dữ liệu mới theo thứ tự đúng
                SeedBusinessPartnerCategories(context);
                context.SubmitChanges(); // Submit categories trước

                SeedBusinessPartners(context);
                context.SubmitChanges(); // Submit partners trước

                SeedBusinessPartnerMappings(context);
                context.SubmitChanges(); // Submit mappings

                SeedBusinessPartnerSites(context);
                context.SubmitChanges(); // Submit sites

                SeedBusinessPartnerContacts(context);
                context.SubmitChanges(); // Submit contacts cuối cùng
            }
        }

        /// <summary>
        /// Tạo tất cả dữ liệu mẫu (BusinessPartner, BusinessPartnerCategory, Mappings, Sites, Contacts)
        /// Sử dụng connection string được truyền vào (cho trường hợp đặc biệt)
        /// </summary>
        /// <param name="connectionString">Connection string để kết nối database</param>
        public static void SeedAllData(string connectionString)
        {
            using (var context = new VnsErp2025DataContext(connectionString))
            {
                // Xóa dữ liệu cũ (nếu có) theo thứ tự ngược lại để tránh foreign key constraint
                context.BusinessPartnerContacts.DeleteAllOnSubmit(context.BusinessPartnerContacts);
                context.BusinessPartnerSites.DeleteAllOnSubmit(context.BusinessPartnerSites);
                context.BusinessPartner_BusinessPartnerCategories.DeleteAllOnSubmit(context.BusinessPartner_BusinessPartnerCategories);
                context.BusinessPartners.DeleteAllOnSubmit(context.BusinessPartners);
                context.BusinessPartnerCategories.DeleteAllOnSubmit(context.BusinessPartnerCategories);
                context.SubmitChanges();

                // Tạo dữ liệu mới theo thứ tự đúng
                SeedBusinessPartnerCategories(context);
                context.SubmitChanges(); // Submit categories trước

                SeedBusinessPartners(context);
                context.SubmitChanges(); // Submit partners trước

                SeedBusinessPartnerMappings(context);
                context.SubmitChanges(); // Submit mappings

                SeedBusinessPartnerSites(context);
                context.SubmitChanges(); // Submit sites

                SeedBusinessPartnerContacts(context);
                context.SubmitChanges(); // Submit contacts cuối cùng
            }
        }

        /// <summary>
        /// Tạo tất cả dữ liệu mẫu (BusinessPartner, BusinessPartnerCategory, Mappings, Sites, Contacts)
        /// Sử dụng context có sẵn (cho backward compatibility)
        /// </summary>
        /// <param name="context">DataContext để lưu dữ liệu</param>
        public static void SeedAllData(VnsErp2025DataContext context)
        {
            // Xóa dữ liệu cũ (nếu có) theo thứ tự ngược lại để tránh foreign key constraint
            context.BusinessPartnerContacts.DeleteAllOnSubmit(context.BusinessPartnerContacts);
            context.BusinessPartnerSites.DeleteAllOnSubmit(context.BusinessPartnerSites);
            context.BusinessPartner_BusinessPartnerCategories.DeleteAllOnSubmit(context.BusinessPartner_BusinessPartnerCategories);
            context.BusinessPartners.DeleteAllOnSubmit(context.BusinessPartners);
            context.BusinessPartnerCategories.DeleteAllOnSubmit(context.BusinessPartnerCategories);
            context.SubmitChanges();

            // Tạo dữ liệu mới theo thứ tự đúng
            SeedBusinessPartnerCategories(context);
            context.SubmitChanges(); // Submit categories trước

            SeedBusinessPartners(context);
            context.SubmitChanges(); // Submit partners trước

            SeedBusinessPartnerMappings(context);
            context.SubmitChanges(); // Submit mappings

            SeedBusinessPartnerSites(context);
            context.SubmitChanges(); // Submit sites

            SeedBusinessPartnerContacts(context);
            context.SubmitChanges(); // Submit contacts cuối cùng
        }

        #region ========== XÓA DỮ LIỆU ==========

        /// <summary>
        /// Xóa tất cả dữ liệu Partner (BusinessPartner, BusinessPartnerCategory, Mappings, Sites, Contacts)
        /// Sử dụng connection string từ ConfigurationManager (recommended)
        /// </summary>
        public static void DeleteAllPartnerData()
        {
            using (var context = CreateContext())
            {
                DeleteAllPartnerData(context);
            }
        }

        /// <summary>
        /// Xóa tất cả dữ liệu Partner (BusinessPartner, BusinessPartnerCategory, Mappings, Sites, Contacts)
        /// Sử dụng connection string được truyền vào (cho trường hợp đặc biệt)
        /// </summary>
        /// <param name="connectionString">Connection string để kết nối database</param>
        public static void DeleteAllPartnerData(string connectionString)
        {
            using (var context = new VnsErp2025DataContext(connectionString))
            {
                DeleteAllPartnerData(context);
            }
        }

        /// <summary>
        /// Xóa tất cả dữ liệu Partner (BusinessPartner, BusinessPartnerCategory, Mappings, Sites, Contacts)
        /// Sử dụng context có sẵn (cho backward compatibility)
        /// </summary>
        /// <param name="context">DataContext để xóa dữ liệu</param>
        public static void DeleteAllPartnerData(VnsErp2025DataContext context)
        {
            try
            {
                Console.WriteLine("🔄 Bắt đầu xóa tất cả dữ liệu Partner...");

                // Xóa dữ liệu theo thứ tự ngược lại để tránh foreign key constraint
                // 1. Xóa BusinessPartnerContacts (có FK đến BusinessPartnerSite)
                var contactCount = context.BusinessPartnerContacts.Count();
                if (contactCount > 0)
                {
                    context.BusinessPartnerContacts.DeleteAllOnSubmit(context.BusinessPartnerContacts);
                    context.SubmitChanges();
                    Console.WriteLine($"✅ Đã xóa {contactCount} BusinessPartnerContacts");
                }

                // 2. Xóa BusinessPartnerSites (có FK đến BusinessPartner)
                var siteCount = context.BusinessPartnerSites.Count();
                if (siteCount > 0)
                {
                    context.BusinessPartnerSites.DeleteAllOnSubmit(context.BusinessPartnerSites);
                    context.SubmitChanges();
                    Console.WriteLine($"✅ Đã xóa {siteCount} BusinessPartnerSites");
                }

                // 3. Xóa BusinessPartner_BusinessPartnerCategories (mapping table)
                var mappingCount = context.BusinessPartner_BusinessPartnerCategories.Count();
                if (mappingCount > 0)
                {
                    context.BusinessPartner_BusinessPartnerCategories.DeleteAllOnSubmit(context.BusinessPartner_BusinessPartnerCategories);
                    context.SubmitChanges();
                    Console.WriteLine($"✅ Đã xóa {mappingCount} BusinessPartner_BusinessPartnerCategories");
                }

                // 4. Xóa BusinessPartners (parent table)
                var partnerCount = context.BusinessPartners.Count();
                if (partnerCount > 0)
                {
                    context.BusinessPartners.DeleteAllOnSubmit(context.BusinessPartners);
                    context.SubmitChanges();
                    Console.WriteLine($"✅ Đã xóa {partnerCount} BusinessPartners");
                }

                // 5. Xóa BusinessPartnerCategories (parent table)
                var categoryCount = context.BusinessPartnerCategories.Count();
                if (categoryCount > 0)
                {
                    context.BusinessPartnerCategories.DeleteAllOnSubmit(context.BusinessPartnerCategories);
                    context.SubmitChanges();
                    Console.WriteLine($"✅ Đã xóa {categoryCount} BusinessPartnerCategories");
                }

                Console.WriteLine("🎉 Hoàn thành xóa tất cả dữ liệu Partner!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Lỗi khi xóa dữ liệu Partner: {ex.Message}");
                throw;
            }
        }


        #endregion
    }
}
