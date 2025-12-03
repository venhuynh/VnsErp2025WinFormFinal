using Dal.DataAccess.Interfaces.Inventory.InventoryManagement;
using Dal.DataContext;
using Logger;
using Logger.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using CustomLogger = Logger.Interfaces.ILogger;
namespace Dal.DataAccess.Implementations.Inventory.InventoryManagement;

public class StockInOutImageRepository : IStockInOutImageRepository
{
    #region Private Fields

    /// <summary>
    /// Chuỗi kết nối cơ sở dữ liệu để tạo DataContext mới cho mỗi operation.
    /// </summary>
    private readonly string _connectionString;

    /// <summary>
    /// Instance logger để theo dõi các thao tác và lỗi
    /// </summary>
    private readonly CustomLogger _logger;

    #endregion

    #region Constructor

    /// <summary>
    /// Khởi tạo một instance mới của class StockInRepository.
    /// </summary>
    /// <param name="connectionString">Chuỗi kết nối cơ sở dữ liệu</param>
    /// <exception cref="ArgumentNullException">Ném ra khi connectionString là null</exception>
    public StockInOutImageRepository(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _logger = LoggerFactory.CreateLogger(LogCategory.DAL);
        _logger.Info("StockInOutImageRepository được khởi tạo với connection string");
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Tạo DataContext mới cho mỗi operation để tránh cache issue
    /// </summary>
    /// <returns>DataContext mới</returns>
    private VnsErp2025DataContext CreateNewContext()
    {
        var context = new VnsErp2025DataContext(_connectionString);

        // Configure eager loading cho navigation properties
        var loadOptions = new DataLoadOptions();
        context.LoadOptions = loadOptions;

        return context;
    }

    #endregion

    #region Save Operations

    /// <summary>
    /// Lưu hoặc cập nhật hình ảnh nhập/xuất kho
    /// </summary>
    /// <param name="stockInOutImage">Entity hình ảnh cần lưu</param>
    public void SaveOrUpdate(StockInOutImage stockInOutImage)
    {
        if (stockInOutImage == null)
            throw new ArgumentNullException(nameof(stockInOutImage));

        using var context = CreateNewContext();
        try
        {
            _logger.Debug("SaveOrUpdate: Bắt đầu lưu hình ảnh, Id={0}, StockInOutMasterId={1}", 
                stockInOutImage.Id, stockInOutImage.StockInOutMasterId);

            var existing = stockInOutImage.Id != Guid.Empty ? 
                context.StockInOutImages.FirstOrDefault(x => x.Id == stockInOutImage.Id) : null;

            if (existing == null)
            {
                // Thêm mới
                if (stockInOutImage.Id == Guid.Empty)
                    stockInOutImage.Id = Guid.NewGuid();
                
                // Thiết lập giá trị mặc định
                if (stockInOutImage.CreateDate == default(DateTime))
                    stockInOutImage.CreateDate = DateTime.Now;

                context.StockInOutImages.InsertOnSubmit(stockInOutImage);
                context.SubmitChanges();
                
                _logger.Info("SaveOrUpdate: Đã thêm mới hình ảnh, Id={0}", stockInOutImage.Id);
            }
            else
            {
                // Cập nhật
                existing.StockInOutMasterId = stockInOutImage.StockInOutMasterId;
                existing.ImageData = stockInOutImage.ImageData;
                existing.FileName = stockInOutImage.FileName;
                existing.RelativePath = stockInOutImage.RelativePath;
                existing.FullPath = stockInOutImage.FullPath;
                existing.StorageType = stockInOutImage.StorageType;
                existing.FileSize = stockInOutImage.FileSize;
                existing.FileExtension = stockInOutImage.FileExtension;
                existing.MimeType = stockInOutImage.MimeType;
                existing.Checksum = stockInOutImage.Checksum;
                existing.FileExists = stockInOutImage.FileExists;
                existing.ModifiedDate = DateTime.Now;
                existing.ModifiedBy = stockInOutImage.ModifiedBy;
                
                context.SubmitChanges();
                
                _logger.Info("SaveOrUpdate: Đã cập nhật hình ảnh, Id={0}", existing.Id);
            }
        }
        catch (Exception ex)
        {
            _logger.Error($"SaveOrUpdate: Lỗi lưu hình ảnh: {ex.Message}", ex);
            throw;
        }
    }

    #endregion

    #region Query Operations

    /// <summary>
    /// Lấy hình ảnh theo ID
    /// </summary>
    /// <param name="id">ID hình ảnh</param>
    /// <returns>StockInOutImage hoặc null</returns>
    public StockInOutImage GetById(Guid id)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetById: Lấy hình ảnh, Id={0}", id);

            var image = context.StockInOutImages.FirstOrDefault(x => x.Id == id);

            if (image == null)
            {
                _logger.Warning("GetById: Không tìm thấy hình ảnh với Id={0}", id);
            }
            else
            {
                _logger.Info("GetById: Đã lấy hình ảnh, Id={0}, StockInOutMasterId={1}", id, image.StockInOutMasterId);
            }

            return image;
        }
        catch (Exception ex)
        {
            _logger.Error($"GetById: Lỗi lấy hình ảnh: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Lấy danh sách hình ảnh theo StockInOutMasterId
    /// </summary>
    /// <param name="stockInOutMasterId">ID phiếu nhập/xuất kho</param>
    /// <returns>Danh sách hình ảnh</returns>
    public List<StockInOutImage> GetByStockInOutMasterId(Guid stockInOutMasterId)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetByStockInOutMasterId: Lấy danh sách hình ảnh, StockInOutMasterId={0}", stockInOutMasterId);

            var images = context.StockInOutImages
                .Where(x => x.StockInOutMasterId == stockInOutMasterId)
                .OrderBy(x => x.CreateDate)
                .ToList();
            
            // Load StockInOutMaster cho tất cả images để có thông tin phiếu
            foreach (var image in images)
            {
                if (image.StockInOutMaster == null)
                {
                    image.StockInOutMaster = context.StockInOutMasters
                        .FirstOrDefault(m => m.Id == image.StockInOutMasterId);
                }
            }

            _logger.Info("GetByStockInOutMasterId: Lấy được {0} hình ảnh", images.Count);
            return images;
        }
        catch (Exception ex)
        {
            _logger.Error($"GetByStockInOutMasterId: Lỗi lấy danh sách hình ảnh: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Query hình ảnh theo khoảng thời gian và từ khóa
    /// </summary>
    /// <param name="fromDate">Từ ngày</param>
    /// <param name="toDate">Đến ngày</param>
    /// <param name="keyword">Từ khóa tìm kiếm (tên file, đường dẫn)</param>
    /// <returns>Danh sách hình ảnh</returns>
    public List<StockInOutImage> QueryImages(DateTime fromDate, DateTime toDate, string keyword = null)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("QueryImages: Bắt đầu query hình ảnh, FromDate={0}, ToDate={1}, Keyword={2}", 
                fromDate, toDate, keyword ?? "null");

            // Bắt đầu query từ StockInOutImage và include StockInOutMaster để lấy thông tin phiếu
            var queryable = context.StockInOutImages.AsQueryable();

            // Filter theo thời gian (CreateDate)
            queryable = queryable.Where(x => x.CreateDate >= fromDate.Date && 
                                            x.CreateDate <= toDate.Date.AddDays(1).AddTicks(-1));

            // Filter theo từ khóa nếu có
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var searchText = keyword.Trim();
                queryable = queryable.Where(x =>
                    // Tìm trong tên file
                    (x.FileName != null && x.FileName.Contains(searchText)) ||
                    // Tìm trong đường dẫn tương đối
                    (x.RelativePath != null && x.RelativePath.Contains(searchText)) ||
                    // Tìm trong đường dẫn đầy đủ
                    (x.FullPath != null && x.FullPath.Contains(searchText))
                );
            }

            // Sắp xếp theo ngày tạo (mới nhất trước)
            var result = queryable
                .OrderByDescending(x => x.CreateDate)
                .ThenByDescending(x => x.FileName ?? string.Empty)
                .ToList();

            // Load StockInOutMaster cho tất cả images để có thông tin phiếu
            var masterIds = result.Select(x => x.StockInOutMasterId).Distinct().ToList();
            var masters = context.StockInOutMasters
                .Where(m => masterIds.Contains(m.Id))
                .ToDictionary(m => m.Id);

            foreach (var image in result)
            {
                if (image.StockInOutMaster == null && masters.TryGetValue(image.StockInOutMasterId, out var master))
                {
                    image.StockInOutMaster = master;
                }
            }

            _logger.Info("QueryImages: Query thành công, ResultCount={0}", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.Error($"QueryImages: Lỗi query hình ảnh: {ex.Message}", ex);
            throw;
        }
    }

    #endregion

    #region Delete Operations

    /// <summary>
    /// Xóa hình ảnh theo ID
    /// </summary>
    /// <param name="id">ID hình ảnh cần xóa</param>
    public void Delete(Guid id)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("Delete: Bắt đầu xóa hình ảnh, Id={0}", id);

            var image = context.StockInOutImages.FirstOrDefault(x => x.Id == id);
            if (image == null)
            {
                _logger.Warning("Delete: Không tìm thấy hình ảnh với Id={0}", id);
                throw new InvalidOperationException($"Không tìm thấy hình ảnh với ID: {id}");
            }

            context.StockInOutImages.DeleteOnSubmit(image);
            context.SubmitChanges();

            _logger.Info("Delete: Đã xóa hình ảnh, Id={0}", id);
        }
        catch (Exception ex)
        {
            _logger.Error($"Delete: Lỗi xóa hình ảnh: {ex.Message}", ex);
            throw;
        }
    }

    #endregion

}