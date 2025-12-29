using Dal.DataAccess.Interfaces.Inventory.StockIn;
using Dal.DataContext;
using Dal.DtoConverter.Inventory;
using Dal.Exceptions;
using DTO.Inventory;
using Logger;
using Logger.Configuration;
using System;
using System.Data.Linq;
using System.Linq;
using CustomLogger = Logger.Interfaces.ILogger;

namespace Dal.DataAccess.Implementations.Inventory.StockIn;

public class StockInRepository : IStockInRepository
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
    public StockInRepository(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _logger = LoggerFactory.CreateLogger(LogCategory.DAL);
        _logger.Info("StockInRepository được khởi tạo với connection string");
    }

    #endregion

    #region ========== HELPER METHODS ==========

    /// <summary>
    /// Tạo DataContext mới cho mỗi operation để tránh cache issue
    /// </summary>
    /// <returns>DataContext mới</returns>
    private VnsErp2025DataContext CreateNewContext()
    {
        var context = new VnsErp2025DataContext(_connectionString);

        // Configure eager loading cho navigation properties
        var loadOptions = new DataLoadOptions();
        loadOptions.LoadWith<StockInOutMaster>(m => m.CompanyBranch);
        loadOptions.LoadWith<StockInOutMaster>(m => m.BusinessPartnerSite);
        loadOptions.LoadWith<BusinessPartnerSite>(s => s.BusinessPartner);
        loadOptions.LoadWith<StockInOutMaster>(m => m.StockInOutDetails);
        loadOptions.LoadWith<StockInOutDetail>(d => d.ProductVariant);
        loadOptions.LoadWith<ProductVariant>(v => v.ProductService);
        context.LoadOptions = loadOptions;

        return context;
    }

    #endregion

    #region Create



    #endregion

    #region Retreive

    /// <summary>
    /// Lấy thông tin master phiếu nhập/xuất kho theo ID và chuyển đổi sang DTO
    /// </summary>
    /// <param name="stockInOutMasterId">ID phiếu nhập/xuất kho</param>
    /// <returns>StockInOutMasterForUIDto nếu tìm thấy, null nếu không tìm thấy</returns>
    public StockInOutMasterForUIDto GetStockInOutMasterForUIDtoById(Guid stockInOutMasterId)
    {
        try
        {
            if (stockInOutMasterId == Guid.Empty)
            {
                _logger.Warning("GetStockInOutMasterForUIDtoById: stockInOutMasterId là Guid.Empty");
                return null;
            }

            using var context = CreateNewContext();

            // Truy vấn entity từ database
            var masterEntity = context.StockInOutMasters
                .FirstOrDefault(m => m.Id == stockInOutMasterId);

            if (masterEntity == null)
            {
                _logger.Warning("GetStockInOutMasterForUIDtoById: Không tìm thấy master với Id={0}", stockInOutMasterId);
                return null;
            }

            // Force load tất cả navigation properties trước khi dispose DataContext
            // (để đảm bảo dữ liệu được load đầy đủ trước khi convert sang DTO)
            if (masterEntity.CompanyBranch != null)
            {
                var _ = masterEntity.CompanyBranch.BranchName;
                var __ = masterEntity.CompanyBranch.BranchCode;
            }

            if (masterEntity.BusinessPartnerSite != null)
            {
                var _ = masterEntity.BusinessPartnerSite.SiteName;
                if (masterEntity.BusinessPartnerSite.BusinessPartner != null)
                {
                    var __ = masterEntity.BusinessPartnerSite.BusinessPartner.PartnerName;
                }
            }

            // Chuyển đổi entity sang DTO sử dụng converter
            var dto = masterEntity.ToDto();

            _logger.Info("GetStockInOutMasterForUIDtoById: Lấy được master thành công, Id={0}", stockInOutMasterId);
            
            return dto;
        }
        catch (Exception ex)
        {
            _logger.Error($"GetStockInOutMasterForUIDtoById: Lỗi lấy thông tin master: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy thông tin master phiếu nhập/xuất kho: {ex.Message}", ex);
        }
    }

    #endregion

}