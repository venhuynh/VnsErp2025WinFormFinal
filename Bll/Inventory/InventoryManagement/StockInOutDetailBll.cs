using Dal.DataAccess.Interfaces.Inventory.InventoryManagement;
using Dal.DataContext;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;
using System;
using System.Collections.Generic;
using Dal.Connection;
using Dal.DataAccess.Implementations.Inventory.InventoryManagement;

namespace Bll.Inventory.InventoryManagement
{
    /// <summary>
    /// Business Logic Layer cho StockInOutProductHistory (Lịch sử sản phẩm nhập xuất kho)
    /// </summary>
    public class StockInOutDetailBll
    {
        #region Fields

        private IStockInOutDetailRepository _stockInOutDetailRepository;
        private readonly ILogger _logger;
        private readonly object _stockInOutDetailRepositoryLock = new object();

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor mặc định
        /// </summary>
        public StockInOutDetailBll()
        {
            _logger = LoggerFactory.CreateLogger(LogCategory.BLL);
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Lấy hoặc khởi tạo StockInOutDetailRepository (lazy initialization)
        /// </summary>
        private IStockInOutDetailRepository GetStockInOutDetailRepository()
        {
            if (_stockInOutDetailRepository == null)
            {
                lock (_stockInOutDetailRepositoryLock)
                {
                    if (_stockInOutDetailRepository == null)
                    {
                        try
                        {
                            // Sử dụng global connection string từ ApplicationStartupManager
                            var globalConnectionString = ApplicationStartupManager.Instance.GetGlobalConnectionString();
                            if (string.IsNullOrEmpty(globalConnectionString))
                            {
                                throw new InvalidOperationException(
                                    "Không có global connection string. Ứng dụng chưa được khởi tạo hoặc chưa sẵn sàng.");
                            }

                            _stockInOutDetailRepository = new StockInOutDetailRepository(globalConnectionString);
                        }
                        catch (Exception ex)
                        {
                            _logger.Error($"Lỗi khởi tạo StockInOutDetailRepository: {ex.Message}", ex);
                            throw;
                        }
                    }
                }
            }

            return _stockInOutDetailRepository;
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
            try
            {
                _logger.Debug(
                    "QueryProductHistory: Bắt đầu query lịch sử sản phẩm, FromDate={0}, ToDate={1}, Keyword={2}",
                    fromDate, toDate, keyword ?? "null");

                // Gọi Repository để thực hiện query
                var result = GetStockInOutDetailRepository().QueryProductHistory(fromDate, toDate, keyword);

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
        /// </summary>
        /// <param name="id">ID của StockInOutDetail cần xóa</param>
        /// <returns>ID của StockInOutMaster (để kiểm tra xem có còn detail nào không)</returns>
        public Guid Delete(Guid id)
        {
            try
            {
                _logger.Debug("Delete: Bắt đầu xóa StockInOutDetail, Id={0}", id);

                var masterId = GetStockInOutDetailRepository().Delete(id);

                _logger.Info("Delete: Xóa StockInOutDetail thành công, Id={0}, MasterId={1}", id, masterId);
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
            try
            {
                _logger.Debug("HasRemainingDetails: Kiểm tra MasterId={0}", stockInOutMasterId);

                var hasDetails = GetStockInOutDetailRepository().HasRemainingDetails(stockInOutMasterId);

                _logger.Info("HasRemainingDetails: MasterId={0}, HasDetails={1}", stockInOutMasterId, hasDetails);
                return hasDetails;
            }
            catch (Exception ex)
            {
                _logger.Error($"HasRemainingDetails: Lỗi kiểm tra detail: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Query StockInOutDetail theo danh sách ProductVariantId
        /// </summary>
        /// <param name="productVariantIds">Danh sách ProductVariantId</param>
        /// <returns>Danh sách StockInOutDetail entities</returns>
        public List<StockInOutDetail> QueryByProductVariantIds(List<Guid> productVariantIds)
        {
            try
            {
                _logger.Debug("QueryByProductVariantIds: Bắt đầu query, ProductVariantIds count={0}", 
                    productVariantIds?.Count ?? 0);

                var result = GetStockInOutDetailRepository().QueryByProductVariantIds(productVariantIds);

                _logger.Info("QueryByProductVariantIds: Query thành công, ResultCount={0}", result.Count);
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error($"QueryByProductVariantIds: Lỗi query: {ex.Message}", ex);
                throw;
            }
        }

        #endregion
    }
}