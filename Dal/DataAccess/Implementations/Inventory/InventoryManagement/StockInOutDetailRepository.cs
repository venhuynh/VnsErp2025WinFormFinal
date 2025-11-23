using Dal.DataAccess.Interfaces.Inventory.InventoryManagement;
using Dal.DataContext;
using Logger;
using Logger.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using CustomLogger = Logger.Interfaces.ILogger;

namespace Dal.DataAccess.Implementations.Inventory.InventoryManagement
{
    public class StockInOutDetailRepository: IStockInOutDetailRepository
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
        /// Khởi tạo một instance mới của class StockInOutDetailRepository.
        /// </summary>
        /// <param name="connectionString">Chuỗi kết nối cơ sở dữ liệu</param>
        /// <exception cref="ArgumentNullException">Ném ra khi connectionString là null</exception>
        public StockInOutDetailRepository(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            _logger = LoggerFactory.CreateLogger(LogCategory.DAL);
            _logger.Info("StockInOutDetailRepository được khởi tạo với connection string");
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
            loadOptions.LoadWith<StockInOutDetail>(d => d.StockInOutMaster);
            loadOptions.LoadWith<StockInOutMaster>(m => m.CompanyBranch);
            loadOptions.LoadWith<StockInOutMaster>(m => m.BusinessPartnerSite);
            loadOptions.LoadWith<BusinessPartnerSite>(s => s.BusinessPartner);
            loadOptions.LoadWith<StockInOutDetail>(d => d.ProductVariant);
            loadOptions.LoadWith<ProductVariant>(v => v.ProductService);
            loadOptions.LoadWith<ProductVariant>(v => v.UnitOfMeasure);
            context.LoadOptions = loadOptions;

            return context;
        }

        #endregion

        #region Query Operations

        /// <summary>
        /// Query lịch sử sản phẩm nhập xuất kho với filter
        /// Tìm kiếm trong StockInOutDetail và các bảng liên quan (ProductVariant, ProductService)
        /// </summary>
        /// <param name="fromDate">Từ ngày</param>
        /// <param name="toDate">Đến ngày</param>
        /// <param name="keyword">Từ khóa tìm kiếm (tìm trong mã hàng, tên hàng, số phiếu)</param>
        /// <returns>Danh sách StockInOutDetail entities</returns>
        public List<StockInOutDetail> QueryProductHistory(DateTime fromDate, DateTime toDate, string keyword = null)
        {
            using var context = CreateNewContext();
            try
            {
                _logger.Debug("QueryProductHistory: Bắt đầu query lịch sử sản phẩm, FromDate={0}, ToDate={1}, Keyword={2}", 
                    fromDate, toDate, keyword ?? "null");

                // Bắt đầu query từ StockInOutDetail
                var queryable = context.StockInOutDetails.AsQueryable();

                // Filter theo thời gian thông qua StockInOutMaster (chỉ lấy các record có Master)
                queryable = queryable.Where(d => d.StockInOutMaster != null &&
                                                 d.StockInOutMaster.StockInOutDate >= fromDate.Date &&
                                                 d.StockInOutMaster.StockInOutDate <= toDate.Date.AddDays(1).AddTicks(-1));

                // Filter theo từ khóa nếu có
                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    var searchText = keyword.Trim();
                    queryable = queryable.Where(d =>
                        // Tìm trong số phiếu (từ Master)
                        (d.StockInOutMaster != null && d.StockInOutMaster.VocherNumber != null && 
                         d.StockInOutMaster.VocherNumber.Contains(searchText)) ||
                        // Tìm trong mã biến thể sản phẩm
                        (d.ProductVariant != null && d.ProductVariant.VariantCode != null && 
                         d.ProductVariant.VariantCode.Contains(searchText)) ||
                        // Tìm trong tên biến thể sản phẩm
                        (d.ProductVariant != null && d.ProductVariant.VariantFullName != null && 
                         d.ProductVariant.VariantFullName.Contains(searchText)) ||
                        // Tìm trong tên sản phẩm/dịch vụ (từ ProductService)
                        (d.ProductVariant != null && d.ProductVariant.ProductService != null && 
                         d.ProductVariant.ProductService.Name != null && 
                         d.ProductVariant.ProductService.Name.Contains(searchText)) ||
                        // Tìm trong mã sản phẩm/dịch vụ (từ ProductService)
                        (d.ProductVariant != null && d.ProductVariant.ProductService != null && 
                         d.ProductVariant.ProductService.Code != null && 
                         d.ProductVariant.ProductService.Code.Contains(searchText))
                    );
                }

                // Sắp xếp theo ngày nhập xuất (mới nhất trước) và số phiếu
                // Vì đã filter chỉ lấy các record có StockInOutMaster != null, nên có thể an toàn truy cập
                var result = queryable
                    .OrderByDescending(d => d.StockInOutMaster.StockInOutDate)
                    .ThenByDescending(d => d.StockInOutMaster.VocherNumber ?? string.Empty)
                    .ToList();

                _logger.Info("QueryProductHistory: Query thành công, ResultCount={0}", result.Count);
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error($"QueryProductHistory: Lỗi query lịch sử sản phẩm: {ex.Message}", ex);
                throw;
            }
        }

        #endregion

        #region Delete Operations

        /// <summary>
        /// Xóa StockInOutDetail theo ID
        /// Xóa tất cả Warranty liên quan trước khi xóa detail
        /// </summary>
        /// <param name="id">ID của StockInOutDetail cần xóa</param>
        /// <returns>ID của StockInOutMaster (để kiểm tra xem có còn detail nào không)</returns>
        public Guid Delete(Guid id)
        {
            using var context = CreateNewContext();
            try
            {
                _logger.Debug("Delete: Bắt đầu xóa StockInOutDetail, Id={0}", id);

                // Tìm detail entity
                var detail = context.StockInOutDetails.FirstOrDefault(d => d.Id == id);
                if (detail == null)
                {
                    _logger.Warning("Delete: Không tìm thấy StockInOutDetail với Id={0}", id);
                    throw new InvalidOperationException($"Không tìm thấy chi tiết nhập xuất kho với ID: {id}");
                }

                // Lưu MasterId trước khi xóa
                var masterId = detail.StockInOutMasterId;

                // Xóa tất cả Warranty liên quan đến detail này trước
                var warranties = context.Warranties.Where(w => w.StockInOutDetailId == id).ToList();
                if (warranties.Any())
                {
                    context.Warranties.DeleteAllOnSubmit(warranties);
                    _logger.Debug("Delete: Đã xóa {0} Warranty liên quan", warranties.Count);
                }

                // Xóa detail
                context.StockInOutDetails.DeleteOnSubmit(detail);
                context.SubmitChanges();

                _logger.Info("Delete: Đã xóa StockInOutDetail, Id={0}, MasterId={1}", id, masterId);
                return masterId;
            }
            catch (Exception ex)
            {
                _logger.Error($"Delete: Lỗi xóa StockInOutDetail: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Kiểm tra xem StockInOutMaster còn detail nào không
        /// </summary>
        /// <param name="stockInOutMasterId">ID của StockInOutMaster</param>
        /// <returns>True nếu còn detail, False nếu không còn</returns>
        public bool HasRemainingDetails(Guid stockInOutMasterId)
        {
            using var context = CreateNewContext();
            try
            {
                var hasDetails = context.StockInOutDetails
                    .Any(d => d.StockInOutMasterId == stockInOutMasterId);

                _logger.Debug("HasRemainingDetails: MasterId={0}, HasDetails={1}", stockInOutMasterId, hasDetails);
                return hasDetails;
            }
            catch (Exception ex)
            {
                _logger.Error($"HasRemainingDetails: Lỗi kiểm tra detail: {ex.Message}", ex);
                throw;
            }
        }

        #endregion
    }
}
