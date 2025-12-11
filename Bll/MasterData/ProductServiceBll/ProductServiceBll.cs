using Dal.Connection;
using Dal.DataAccess.Implementations.MasterData.ProductServiceRepositories;
using Dal.DataAccess.Interfaces.MasterData.ProductServiceRepositories;
using Dal.DataContext;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bll.Common.ImageStorage;
using Bll.Common.ImageService;
using Bll.Common;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;

namespace Bll.MasterData.ProductServiceBll
{
    /// <summary>
    /// Business Logic Layer cho ProductService.
    /// Cung cấp các phương thức xử lý nghiệp vụ cho sản phẩm/dịch vụ.
    /// </summary>
    public class ProductServiceBll
    {
        #region Fields

        private IProductServiceRepository _dataAccess;
        private IProductServiceCategoryRepository _categoryDataAccess;
        private readonly object _lockObject = new object();
        private readonly object _categoryLockObject = new object();
        private readonly IImageStorageService _imageStorage;
        private readonly ImageCompressionService _imageCompression;
        private readonly ILogger _logger;

        #endregion

        #region Constructor

        /// <summary>
        /// Khởi tạo ProductServiceBll
        /// </summary>
        public ProductServiceBll()
        {
            new ProductImageBll();
            _logger = LoggerFactory.CreateLogger(LogCategory.BLL);
            _imageStorage = ImageStorageFactory.CreateFromConfig(_logger);
            _imageCompression = new ImageCompressionService();
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Lấy hoặc khởi tạo Repository (lazy initialization)
        /// </summary>
        private IProductServiceRepository GetDataAccess()
        {
            if (_dataAccess == null)
            {
                lock (_lockObject)
                {
                    if (_dataAccess == null)
                    {
                        try
                        {
                            var globalConnectionString = ApplicationStartupManager.Instance.GetGlobalConnectionString();
                            if (string.IsNullOrEmpty(globalConnectionString))
                            {
                                throw new InvalidOperationException(
                                    "Không có global connection string. Ứng dụng chưa được khởi tạo hoặc chưa sẵn sàng.");
                            }

                            _dataAccess = new ProductServiceRepository(globalConnectionString);
                        }
                        catch (Exception ex)
                        {
                            throw new InvalidOperationException(
                                "Không thể khởi tạo kết nối database. Vui lòng kiểm tra cấu hình database.", ex);
                        }
                    }
                }
            }

            return _dataAccess;
        }

        /// <summary>
        /// Lấy hoặc khởi tạo Category Repository (lazy initialization)
        /// </summary>
        private IProductServiceCategoryRepository GetCategoryDataAccess()
        {
            if (_categoryDataAccess == null)
            {
                lock (_categoryLockObject)
                {
                    if (_categoryDataAccess == null)
                    {
                        try
                        {
                            var globalConnectionString = ApplicationStartupManager.Instance.GetGlobalConnectionString();
                            if (string.IsNullOrEmpty(globalConnectionString))
                            {
                                throw new InvalidOperationException(
                                    "Không có global connection string. Ứng dụng chưa được khởi tạo hoặc chưa sẵn sàng.");
                            }

                            _categoryDataAccess = new ProductServiceCategoryRepository(globalConnectionString);
                        }
                        catch (Exception ex)
                        {
                            throw new InvalidOperationException(
                                "Không thể khởi tạo kết nối database. Vui lòng kiểm tra cấu hình database.", ex);
                        }
                    }
                }
            }

            return _categoryDataAccess;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Lấy tất cả sản phẩm/dịch vụ (Sync)
        /// </summary>
        /// <returns>Danh sách ProductService entities</returns>
        public List<ProductService> GetAll()
        {
            try
            {
                return GetDataAccess().GetAll();
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi lấy danh sách sản phẩm/dịch vụ: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy tất cả sản phẩm/dịch vụ (Async)
        /// </summary>
        /// <returns>Danh sách ProductService entities</returns>
        public async Task<List<ProductService>> GetAllAsync()
        {
            try
            {
                return await GetDataAccess().GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi lấy danh sách sản phẩm/dịch vụ: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy sản phẩm/dịch vụ theo ID
        /// </summary>
        /// <param name="id">ID sản phẩm/dịch vụ</param>
        /// <returns>ProductService entity</returns>
        public ProductService GetById(Guid id)
        {
            try
            {
                return GetDataAccess().GetById(id);
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi lấy sản phẩm/dịch vụ theo ID {id}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lưu sản phẩm/dịch vụ (thêm mới hoặc cập nhật)
        /// </summary>
        /// <param name="productService">ProductService entity cần lưu</param>
        public void Save(ProductService productService)
        {
            try
            {
                if (productService == null)
                    throw new ArgumentNullException(nameof(productService));

                GetDataAccess().SaveOrUpdate(productService);
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi lưu sản phẩm/dịch vụ: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lưu hoặc cập nhật sản phẩm/dịch vụ
        /// </summary>
        /// <param name="productService">ProductService entity cần lưu hoặc cập nhật</param>
        public void SaveOrUpdate(ProductService productService)
        {
            try
            {
                if (productService == null)
                    throw new ArgumentNullException(nameof(productService));

                GetDataAccess().SaveOrUpdate(productService);
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi lưu hoặc cập nhật sản phẩm/dịch vụ: {ex.Message}", ex);
            }
        }

        // Phương thức SaveOrUpdateWithImage đã được loại bỏ vì không còn cần thiết
        // Thay vào đó sử dụng SaveOrUpdate thông thường với ThumbnailImage

        /// <summary>
        /// Xóa sản phẩm/dịch vụ theo ID
        /// </summary>
        /// <param name="id">ID sản phẩm/dịch vụ cần xóa</param>
        public void Delete(Guid id)
        {
            try
            {
                var result = GetDataAccess().DeleteProductService(id);
                if (!result)
                {
                    throw new BusinessLogicException($"Không thể xóa sản phẩm/dịch vụ với ID {id}");
                }
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi xóa sản phẩm/dịch vụ với ID {id}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy tên danh mục theo ID
        /// </summary>
        /// <param name="categoryId">ID danh mục</param>
        /// <returns>Tên danh mục hoặc null nếu không tìm thấy</returns>
        public string GetCategoryName(Guid? categoryId)
        {
            try
            {
                if (categoryId == null || categoryId == Guid.Empty)
                    return null;

                var category = GetCategoryDataAccess().GetById(categoryId.Value);
                return category?.CategoryName;
            }
            catch (Exception)
            {
                // Log lỗi nhưng không throw để không ảnh hưởng đến việc hiển thị
                // Có thể sử dụng logger ở đây nếu cần
                return null;
            }
        }

        /// <summary>
        /// Lấy dictionary chứa tất cả categories để tối ưu hiệu suất khi convert DTO
        /// </summary>
        /// <returns>Dictionary với key là CategoryId và value là ProductServiceCategory entity</returns>
        public async Task<Dictionary<Guid, ProductServiceCategory>> GetCategoryDictAsync()
        {
            try
            {
                var categories = await GetCategoryDataAccess().GetAllAsync();
                return categories.ToDictionary(c => c.Id);
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi lấy danh sách categories: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Tính toán đường dẫn đầy đủ của category từ dictionary
        /// </summary>
        /// <param name="categoryId">ID danh mục</param>
        /// <param name="categoryDict">Dictionary chứa tất cả categories</param>
        /// <returns>Đường dẫn đầy đủ hoặc null nếu không tìm thấy</returns>
        private string CalculateCategoryFullPath(Guid? categoryId, Dictionary<Guid, ProductServiceCategory> categoryDict)
        {
            try
            {
                if (categoryId == null || categoryId == Guid.Empty || categoryDict == null)
                    return null;

                if (!categoryDict.TryGetValue(categoryId.Value, out var category))
                    return null;

                var pathParts = new List<string> { category.CategoryName };
                var current = category;

                // Đi ngược lên parent categories
                while (current.ParentId.HasValue)
                {
                    if (!categoryDict.TryGetValue(current.ParentId.Value, out current))
                        break;
                    pathParts.Insert(0, current.CategoryName);
                    
                    // Tránh infinite loop
                    if (pathParts.Count > 10)
                        break;
                }

                return string.Join(" > ", pathParts);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Lấy đường dẫn đầy đủ của danh mục từ gốc đến danh mục này
        /// </summary>
        /// <param name="categoryId">ID danh mục</param>
        /// <returns>Đường dẫn đầy đủ (ví dụ: "Danh mục cha > Danh mục con") hoặc null nếu không tìm thấy</returns>
        public string GetCategoryFullPath(Guid? categoryId)
        {
            try
            {
                if (categoryId == null || categoryId == Guid.Empty)
                    return null;

                var category = GetCategoryDataAccess().GetById(categoryId.Value);
                if (category == null)
                    return null;

                var pathParts = new List<string> { category.CategoryName };
                var current = category;

                // Đi ngược lên parent categories
                while (current.ParentId.HasValue)
                {
                    current = GetCategoryDataAccess().GetById(current.ParentId.Value);
                    if (current == null)
                        break;
                    pathParts.Insert(0, current.CategoryName);
                    
                    // Tránh infinite loop
                    if (pathParts.Count > 10)
                        break;
                }

                return string.Join(" > ", pathParts);
            }
            catch (Exception)
            {
                // Log lỗi nhưng không throw để không ảnh hưởng đến việc hiển thị
                // Có thể sử dụng logger ở đây nếu cần
                return null;
            }
        }

        /// <summary>
        /// Đếm số lượng biến thể của sản phẩm/dịch vụ
        /// </summary>
        /// <param name="productServiceId">ID sản phẩm/dịch vụ</param>
        /// <returns>Số lượng biến thể</returns>
        public int GetVariantCount(Guid productServiceId)
        {
            try
            {
                return GetDataAccess().GetVariantCount(productServiceId);
            }
            catch (Exception)
            {
                // Trả về 0 nếu có lỗi để không ảnh hưởng đến việc hiển thị
                return 0;
            }
        }

        /// <summary>
        /// Đếm số lượng hình ảnh của sản phẩm/dịch vụ
        /// </summary>
        /// <param name="productServiceId">ID sản phẩm/dịch vụ</param>
        /// <returns>Số lượng hình ảnh</returns>
        public int GetImageCount(Guid productServiceId)
        {
            try
            {
                return GetDataAccess().GetImageCount(productServiceId);
            }
            catch (Exception)
            {
                // Trả về 0 nếu có lỗi để không ảnh hưởng đến việc hiển thị
                return 0;
            }
        }

        /// <summary>
        /// Đếm số lượng biến thể và hình ảnh cho nhiều sản phẩm/dịch vụ
        /// </summary>
        /// <param name="productServiceIds">Danh sách ID sản phẩm/dịch vụ</param>
        /// <returns>Dictionary với key là ProductServiceId và value là (VariantCount, ImageCount)</returns>
        public Dictionary<Guid, (int VariantCount, int ImageCount)> GetCountsForProducts(List<Guid> productServiceIds)
        {
            try
            {
                return GetDataAccess().GetCountsForProducts(productServiceIds);
            }
            catch (Exception)
            {
                // Trả về dictionary rỗng nếu có lỗi
                return new Dictionary<Guid, (int, int)>();
            }
        }

        /// <summary>
        /// Tự động tạo mã sản phẩm dựa trên danh mục.
        /// Format: [CategoryCode] + [4 số từ 0001-9999]
        /// </summary>
        /// <param name="categoryId">ID của danh mục</param>
        /// <returns>Mã sản phẩm được tạo</returns>
        public string GenerateProductCode(Guid categoryId)
        {
            try
            {
                // Lấy mã danh mục
                var categoryCode = GetCategoryCode(categoryId);
                if (string.IsNullOrWhiteSpace(categoryCode))
                {
                    return string.Empty;
                }

                // Tìm số tiếp theo trong danh mục này
                var nextNumber = GetDataAccess().GetNextProductNumber(categoryId, categoryCode);

                // Tạo mã hoàn chỉnh
                return $"{categoryCode}{nextNumber:D4}";
            }
            catch (Exception)
            {
                // Trả về chuỗi rỗng nếu có lỗi
                return string.Empty;
            }
        }

        /// <summary>
        /// Lấy mã danh mục từ ID danh mục.
        /// </summary>
        /// <param name="categoryId">ID của danh mục</param>
        /// <returns>Mã danh mục</returns>
        private string GetCategoryCode(Guid categoryId)
        {
            try
            {
                return GetDataAccess().GetCategoryCode(categoryId);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Kiểm tra mã sản phẩm/dịch vụ có tồn tại không.
        /// </summary>
        /// <param name="code">Mã sản phẩm/dịch vụ cần kiểm tra</param>
        /// <param name="excludeId">ID sản phẩm/dịch vụ cần loại trừ (khi cập nhật)</param>
        /// <returns>True nếu tồn tại, False nếu không</returns>
        public bool IsCodeExists(string code, Guid? excludeId = null)
        {
            return GetDataAccess().IsCodeExists(code, excludeId);
        }

        /// <summary>
        /// Kiểm tra tên sản phẩm/dịch vụ có tồn tại không.
        /// </summary>
        /// <param name="name">Tên sản phẩm/dịch vụ cần kiểm tra</param>
        /// <param name="excludeId">ID sản phẩm/dịch vụ cần loại trừ (khi cập nhật)</param>
        /// <returns>True nếu tồn tại, False nếu không</returns>
        public bool IsNameExists(string name, Guid? excludeId = null)
        {
            return GetDataAccess().IsNameExists(name, excludeId);
        }

        #endregion

        #region Optimization Methods

        /// <summary>
        /// Lấy số lượng tổng cộng với filter
        /// </summary>
        /// <param name="searchText">Từ khóa tìm kiếm</param>
        /// <param name="categoryId">Filter theo category</param>
        /// <param name="isService">Filter theo loại</param>
        /// <param name="isActive">Filter theo trạng thái</param>
        /// <returns>Số lượng tổng cộng</returns>
        public async Task<int> GetCountAsync(
            string searchText = null,
            Guid? categoryId = null,
            bool? isService = null,
            bool? isActive = null)
        {
            try
            {
                return await GetDataAccess().GetCountAsync(searchText, categoryId, isService, isActive);
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi đếm số lượng sản phẩm/dịch vụ: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy dữ liệu phân trang
        /// </summary>
        /// <param name="pageIndex">Trang hiện tại (0-based)</param>
        /// <param name="pageSize">Số dòng mỗi trang</param>
        /// <param name="searchText">Từ khóa tìm kiếm</param>
        /// <param name="categoryId">Filter theo category</param>
        /// <param name="isService">Filter theo loại</param>
        /// <param name="isActive">Filter theo trạng thái</param>
        /// <returns>Danh sách entities</returns>
        public async Task<List<ProductService>> GetPagedAsync(
            int pageIndex,
            int pageSize,
            string searchText = null,
            Guid? categoryId = null,
            bool? isService = null,
            bool? isActive = null)
        {
            try
            {
                return await GetDataAccess().GetPagedAsync(pageIndex, pageSize, searchText, categoryId, isService, isActive);
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi lấy dữ liệu phân trang: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy dữ liệu thumbnail image cho lazy loading
        /// </summary>
        /// <param name="productId">ID sản phẩm</param>
        /// <returns>Dữ liệu ảnh thumbnail</returns>
        public byte[] GetThumbnailImageData(Guid productId)
        {
            try
            {
                return GetDataAccess().GetThumbnailImageData(productId);
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi lấy dữ liệu ảnh thumbnail: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Cập nhật thumbnail image cho sản phẩm/dịch vụ từ byte array
        /// Lưu ảnh gốc lên NAS và thumbnail đã resize vào database
        /// </summary>
        /// <param name="productId">ID sản phẩm/dịch vụ</param>
        /// <param name="imageBytes">Byte array của hình ảnh gốc (null để xóa)</param>
        /// <param name="thumbnailMaxDimension">Kích thước tối đa của thumbnail (mặc định 120px)</param>
        /// <returns>ProductService entity đã được cập nhật</returns>
        public async Task<ProductService> UpdateThumbnailImageAsync(Guid productId, byte[] imageBytes, int thumbnailMaxDimension = 120)
        {
            try
            {
                var productService = GetById(productId);
                if (productService == null)
                {
                    throw new BusinessLogicException($"Không tìm thấy sản phẩm/dịch vụ với ID {productId}");
                }

                // Xử lý xóa thumbnail
                if (imageBytes == null || imageBytes.Length == 0)
                {
                    // Xóa file trên NAS nếu có
                    if (!string.IsNullOrWhiteSpace(productService.ThumbnailRelativePath))
                    {
                        try
                        {
                            await _imageStorage.DeleteImageAsync(productService.ThumbnailRelativePath);
                            _logger.Info($"Đã xóa file thumbnail từ storage, RelativePath={productService.ThumbnailRelativePath}");
                        }
                        catch (Exception ex)
                        {
                            _logger.Warning($"Không thể xóa file thumbnail từ storage: {ex.Message}");
                        }
                    }

                    // Xóa thông tin trong database
                    productService.ThumbnailImage = null;
                    productService.ThumbnailFileName = null;
                    productService.ThumbnailRelativePath = null;
                    productService.ThumbnailFullPath = null;
                    productService.ThumbnailStorageType = null;
                    productService.ThumbnailFileSize = null;
                    productService.ThumbnailChecksum = null;
                }
                else
                {
                    // Xử lý upload thumbnail mới
                    if (thumbnailMaxDimension <= 0)
                    {
                        thumbnailMaxDimension = 120; // Default 120px cho cột thumbnail
                    }

                    // 1. Tạo tên file
                    var fileExtension = ".jpg"; // Mặc định JPEG
                    try
                    {
                        using (var ms = new MemoryStream(imageBytes))
                        using (var img = Image.FromStream(ms))
                        {
                            if (img.RawFormat.Equals(ImageFormat.Png))
                                fileExtension = ".png";
                            else if (img.RawFormat.Equals(ImageFormat.Gif))
                                fileExtension = ".gif";
                        }
                    }
                    catch
                    {
                        // Nếu không detect được, dùng .jpg mặc định
                    }

                    var fileName = $"PS_{productId}_{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid():N}{fileExtension}";

                    // 2. Lưu ảnh gốc lên NAS
                    var storageResult = await _imageStorage.SaveImageAsync(
                        imageData: imageBytes,
                        fileName: fileName,
                        category: ImageCategory.Product, // Sử dụng Product category
                        entityId: productId,
                        generateThumbnail: false // Không tạo thumbnail trên NAS, sẽ tạo và lưu vào database
                    );

                    if (!storageResult.Success)
                    {
                        throw new InvalidOperationException(
                            $"Không thể lưu hình ảnh vào storage: {storageResult.ErrorMessage}");
                    }

                    // 3. Tạo thumbnail từ ảnh gốc với kích thước tùy chỉnh
                    byte[] thumbnailData = null;
                    try
                    {
                        thumbnailData = _imageCompression.CompressImage(
                            imageData: imageBytes,
                            targetSize: 50000, // Target size: ~50KB
                            maxDimension: thumbnailMaxDimension, // Sử dụng kích thước tùy chỉnh (120px cho cột thumbnail)
                            format: ImageFormat.Jpeg
                        );
                    }
                    catch (Exception ex)
                    {
                        _logger.Warning($"Không thể tạo thumbnail cho sản phẩm/dịch vụ {productId}: {ex.Message}");
                        // Tiếp tục lưu ảnh dù không tạo được thumbnail
                    }

                    // 4. Xóa file cũ trên NAS nếu có
                    if (!string.IsNullOrWhiteSpace(productService.ThumbnailRelativePath) && 
                        productService.ThumbnailRelativePath != storageResult.RelativePath)
                    {
                        try
                        {
                            await _imageStorage.DeleteImageAsync(productService.ThumbnailRelativePath);
                            _logger.Info($"Đã xóa file thumbnail cũ từ storage, RelativePath={productService.ThumbnailRelativePath}");
                        }
                        catch (Exception ex)
                        {
                            _logger.Warning($"Không thể xóa file thumbnail cũ từ storage: {ex.Message}");
                        }
                    }

                    // 5. Cập nhật thông tin trong entity
                    productService.ThumbnailFileName = storageResult.FileName;
                    productService.ThumbnailRelativePath = storageResult.RelativePath;
                    productService.ThumbnailFullPath = storageResult.FullPath;
                    productService.ThumbnailStorageType = "NAS";
                    productService.ThumbnailFileSize = storageResult.FileSize;
                    productService.ThumbnailChecksum = storageResult.Checksum;

                    // Lưu thumbnail đã resize vào database
                    if (thumbnailData != null && thumbnailData.Length > 0)
                    {
                        productService.ThumbnailImage = new Binary(thumbnailData);
                    }
                    else
                    {
                        // Nếu không tạo được thumbnail, lưu ảnh gốc đã resize nhỏ
                        productService.ThumbnailImage = new Binary(imageBytes);
                    }
                }

                // 6. Lưu vào database
                SaveOrUpdate(productService);

                return productService;
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi khi cập nhật thumbnail image cho sản phẩm/dịch vụ {productId}: {ex.Message}", ex);
                throw new BusinessLogicException($"Lỗi khi cập nhật thumbnail image: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy danh sách sản phẩm với search và filter (optimized)
        /// </summary>
        /// <param name="searchText">Từ khóa tìm kiếm</param>
        /// <param name="categoryId">Filter theo category</param>
        /// <param name="isService">Filter theo loại</param>
        /// <param name="isActive">Filter theo trạng thái</param>
        /// <param name="orderBy">Sắp xếp theo</param>
        /// <param name="orderDirection">Hướng sắp xếp</param>
        /// <returns>Danh sách entities</returns>
        public async Task<List<ProductService>> GetFilteredAsync(
            string searchText = null,
            Guid? categoryId = null,
            bool? isService = null,
            bool? isActive = null,
            string orderBy = "Name",
            string orderDirection = "ASC")
        {
            try
            {
                return await GetDataAccess().GetFilteredAsync(searchText, categoryId, isService, isActive, orderBy, orderDirection);
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi lấy dữ liệu với filter: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy counts cho nhiều sản phẩm cùng lúc (optimized async version)
        /// </summary>
        /// <param name="productIds">Danh sách ID sản phẩm</param>
        /// <returns>Dictionary chứa counts</returns>
        public async Task<Dictionary<Guid, (int VariantCount, int ImageCount)>> GetCountsForProductsAsync(List<Guid> productIds)
        {
            try
            {
                if (productIds == null || !productIds.Any())
                    return new Dictionary<Guid, (int, int)>();

                return await GetDataAccess().GetCountsForProductsAsync(productIds);
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi đếm số lượng cho nhiều sản phẩm: {ex.Message}", ex);
            }
        }

        #endregion

        #region Search and Filter Methods

        /// <summary>
        /// Tìm kiếm sản phẩm/dịch vụ trong toàn bộ database (Sync)
        /// </summary>
        /// <param name="searchText">Text tìm kiếm</param>
        /// <returns>Danh sách kết quả tìm kiếm</returns>
        public List<ProductService> Search(string searchText)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchText))
                    return new List<ProductService>();

                return GetDataAccess().Search(searchText);
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi tìm kiếm sản phẩm/dịch vụ: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Tìm kiếm sản phẩm/dịch vụ trong toàn bộ database (Async)
        /// </summary>
        /// <param name="searchText">Text tìm kiếm</param>
        /// <returns>Danh sách kết quả tìm kiếm</returns>
        public async Task<List<ProductService>> SearchAsync(string searchText)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchText))
                    return new List<ProductService>();

                return await GetDataAccess().SearchAsync(searchText);
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi tìm kiếm sản phẩm/dịch vụ: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy danh sách mã code unique
        /// </summary>
        /// <returns>Danh sách mã code unique</returns>
        public async Task<List<object>> GetUniqueCodesAsync()
        {
            try
            {
                return await GetDataAccess().GetUniqueCodesAsync();
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi lấy danh sách mã code unique: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy danh sách tên unique
        /// </summary>
        /// <returns>Danh sách tên unique</returns>
        public async Task<List<object>> GetUniqueNamesAsync()
        {
            try
            {
                return await GetDataAccess().GetUniqueNamesAsync();
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi lấy danh sách tên unique: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy danh sách tên danh mục unique
        /// </summary>
        /// <returns>Danh sách tên danh mục unique</returns>
        public async Task<List<object>> GetUniqueCategoryNamesAsync()
        {
            try
            {
                return await GetDataAccess().GetUniqueCategoryNamesAsync();
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi lấy danh sách tên danh mục unique: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy danh sách loại hiển thị unique
        /// </summary>
        /// <returns>Danh sách loại hiển thị unique</returns>
        public async Task<List<object>> GetUniqueTypeDisplaysAsync()
        {
            try
            {
                return await GetDataAccess().GetUniqueTypeDisplaysAsync();
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi lấy danh sách loại hiển thị unique: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy danh sách trạng thái hiển thị unique
        /// </summary>
        /// <returns>Danh sách trạng thái hiển thị unique</returns>
        public async Task<List<object>> GetUniqueStatusDisplaysAsync()
        {
            try
            {
                return await GetDataAccess().GetUniqueStatusDisplaysAsync();
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi lấy danh sách trạng thái hiển thị unique: {ex.Message}", ex);
            }
        }

        #endregion
    }

    /// <summary>
    /// Exception cho Business Logic Layer
    /// </summary>
    public class BusinessLogicException : Exception
    {
        public BusinessLogicException(string message) : base(message) { }
        public BusinessLogicException(string message, Exception innerException) : base(message, innerException) { }
    }
}