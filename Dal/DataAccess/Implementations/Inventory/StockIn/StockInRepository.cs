using Dal.DataAccess.Interfaces.Inventory.StockIn;
using Dal.DataContext;
using Dal.DtoConverter.Inventory;
using Dal.Exceptions;
using DTO.Inventory;
using Logger;
using Logger.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Threading.Tasks;
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

    /// <summary>
    /// Lưu phiếu nhập/xuất kho mới (Insert)
    /// Tạo mới master và tất cả details trong database
    /// </summary>
    /// <param name="masterDto">Thông tin master của phiếu nhập/xuất kho (DTO)</param>
    /// <param name="detailDtos">Danh sách chi tiết của phiếu nhập/xuất kho (DTOs)</param>
    /// <returns>ID của master đã được lưu thành công</returns>
    /// <exception cref="ArgumentNullException">Khi masterDto hoặc detailDtos là null</exception>
    /// <exception cref="ArgumentException">Khi detailDtos rỗng</exception>
    /// <exception cref="DataAccessException">Khi có lỗi xảy ra trong quá trình lưu vào database</exception>
    public Task<Guid> SaveAsync(StockInOutMasterForUIDto masterDto, List<StockInOutDetailForUIDto> detailDtos)
    {
        try
        {
            if (masterDto == null)
            {
                throw new ArgumentNullException(nameof(masterDto), @"Master DTO không được null");
            }

            if (detailDtos == null || detailDtos.Count == 0)
            {
                throw new ArgumentException(@"Danh sách chi tiết không được rỗng", nameof(detailDtos));
            }

            using var context = CreateNewContext();

            // Convert DTO sang Entity
            var masterEntity = masterDto.ToEntity();
            
            // Tạo ID mới nếu chưa có
            if (masterEntity.Id == Guid.Empty)
            {
                masterEntity.Id = Guid.NewGuid();
            }

            // Convert detail DTOs sang entities
            var detailEntities = detailDtos.Select(dto =>
            {
                var detailEntity = dto.ToEntity();
                // Gán master ID cho detail
                detailEntity.StockInOutMasterId = masterEntity.Id;
                // Tạo ID mới nếu chưa có
                if (detailEntity.Id == Guid.Empty)
                {
                    detailEntity.Id = Guid.NewGuid();
                }
                return detailEntity;
            }).ToList();

            // Insert master và details vào database
            context.StockInOutMasters.InsertOnSubmit(masterEntity);
            context.StockInOutDetails.InsertAllOnSubmit(detailEntities);
            
            context.SubmitChanges();

            _logger.Info(@"SaveAsync: Đã lưu phiếu nhập/xuất kho mới, MasterId={0}, DetailCount={1}", 
                masterEntity.Id, detailEntities.Count);

            return Task.FromResult(masterEntity.Id);
        }
        catch (Exception ex)
        {
            _logger.Error($@"SaveAsync: Lỗi lưu phiếu nhập/xuất kho: {ex.Message}", ex);
            throw new DataAccessException($@"Lỗi khi lưu phiếu nhập/xuất kho: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Cập nhật phiếu nhập/xuất kho (Update)
    /// Cập nhật thông tin master và thay thế toàn bộ details (xóa cũ, thêm mới)
    /// </summary>
    /// <param name="masterDto">Thông tin master của phiếu nhập/xuất kho (DTO) - phải có Id hợp lệ</param>
    /// <param name="detailDtos">Danh sách chi tiết mới của phiếu nhập/xuất kho (DTOs)</param>
    /// <returns>ID của master đã được cập nhật thành công</returns>
    /// <exception cref="ArgumentNullException">Khi masterDto hoặc detailDtos là null</exception>
    /// <exception cref="ArgumentException">Khi masterDto.Id rỗng hoặc detailDtos rỗng</exception>
    /// <exception cref="DataAccessException">Khi không tìm thấy master với Id đã cho hoặc có lỗi xảy ra trong quá trình cập nhật</exception>
    public Task<Guid> UpdateAsync(StockInOutMasterForUIDto masterDto, List<StockInOutDetailForUIDto> detailDtos)
    {
        try
        {
            if (masterDto == null)
            {
                throw new ArgumentNullException(nameof(masterDto), @"Master DTO không được null");
            }

            if (masterDto.Id == Guid.Empty)
            {
                throw new ArgumentException(@"Master ID không được rỗng khi cập nhật", nameof(masterDto));
            }

            if (detailDtos == null || detailDtos.Count == 0)
            {
                throw new ArgumentException(@"Danh sách chi tiết không được rỗng", nameof(detailDtos));
            }

            using var context = CreateNewContext();

            // Lấy master entity hiện tại
            var existingMaster = context.StockInOutMasters.FirstOrDefault(m => m.Id == masterDto.Id);
            if (existingMaster == null)
            {
                throw new DataAccessException($@"Không tìm thấy phiếu nhập/xuất kho với ID: {masterDto.Id}");
            }

            // Cập nhật thông tin master từ DTO
            var masterEntity = masterDto.ToEntity();
            existingMaster.StockInOutDate = masterEntity.StockInOutDate;
            existingMaster.VocherNumber = masterEntity.VocherNumber;
            existingMaster.StockInOutType = masterEntity.StockInOutType;
            existingMaster.VoucherStatus = masterEntity.VoucherStatus;
            existingMaster.WarehouseId = masterEntity.WarehouseId;
            existingMaster.PurchaseOrderId = masterEntity.PurchaseOrderId;
            existingMaster.PartnerSiteId = masterEntity.PartnerSiteId;
            existingMaster.Notes = masterEntity.Notes;
            existingMaster.NguoiNhanHang = masterEntity.NguoiNhanHang;
            existingMaster.NguoiGiaoHang = masterEntity.NguoiGiaoHang;
            existingMaster.TotalQuantity = masterEntity.TotalQuantity;
            existingMaster.TotalAmount = masterEntity.TotalAmount;
            existingMaster.TotalVat = masterEntity.TotalVat;
            existingMaster.TotalAmountIncludedVat = masterEntity.TotalAmountIncludedVat;

            // Xóa tất cả details cũ
            var existingDetails = context.StockInOutDetails
                .Where(d => d.StockInOutMasterId == masterDto.Id)
                .ToList();
            context.StockInOutDetails.DeleteAllOnSubmit(existingDetails);

            // Thêm details mới
            var detailEntities = detailDtos.Select(dto =>
            {
                var detailEntity = dto.ToEntity();
                // Gán master ID cho detail
                detailEntity.StockInOutMasterId = masterDto.Id;
                // Tạo ID mới nếu chưa có
                if (detailEntity.Id == Guid.Empty)
                {
                    detailEntity.Id = Guid.NewGuid();
                }
                return detailEntity;
            }).ToList();

            context.StockInOutDetails.InsertAllOnSubmit(detailEntities);
            
            context.SubmitChanges();

            _logger.Info(@"UpdateAsync: Đã cập nhật phiếu nhập/xuất kho, MasterId={0}, DetailCount={1}", 
                masterDto.Id, detailEntities.Count);

            return Task.FromResult(masterDto.Id);
        }
        catch (Exception ex)
        {
            _logger.Error($@"UpdateAsync: Lỗi cập nhật phiếu nhập/xuất kho: {ex.Message}", ex);
            throw new DataAccessException($@"Lỗi khi cập nhật phiếu nhập/xuất kho: {ex.Message}", ex);
        }
    }

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

    /// <summary>
    /// Lấy danh sách chi tiết của phiếu nhập/xuất kho dựa trên ID master
    /// </summary>
    /// <param name="stockInOutMasterId">ID duy nhất của phiếu nhập/xuất kho</param>
    /// <returns>Danh sách StockInOutDetailForUIDto</returns>
    public List<StockInOutDetailForUIDto> GetStockInOutDetailsByMasterId(Guid stockInOutMasterId)
    {
        try
        {
            if (stockInOutMasterId == Guid.Empty)
            {
                _logger.Warning("GetStockInOutDetailsByMasterId: stockInOutMasterId là Guid.Empty");
                return new List<StockInOutDetailForUIDto>();
            }

            using var context = CreateNewContext();

            // Lấy danh sách detail entities từ database
            var detailEntities = context.StockInOutDetails
                .Where(d => d.StockInOutMasterId == stockInOutMasterId)
                .OrderBy(d => d.Id) // Sắp xếp để đảm bảo thứ tự nhất quán
                .ToList();

            if (detailEntities == null || detailEntities.Count == 0)
            {
                _logger.Info("GetStockInOutDetailsByMasterId: Không tìm thấy details, MasterId={0}", stockInOutMasterId);
                return new List<StockInOutDetailForUIDto>();
            }

            // Chuyển đổi entities sang DTOs sử dụng extension method
            var detailDtos = detailEntities
                .Where(e => e != null)
                .Select((entity, index) => entity.ToDto(index + 1)) // Extension method từ StockInOutDetailForUIConverter
                .Where(dto => dto != null)
                .ToList();

            // Set line numbers cho các detail DTOs
            for (int i = 0; i < detailDtos.Count; i++)
            {
                detailDtos[i].LineNumber = i + 1;
            }

            _logger.Info("GetStockInOutDetailsByMasterId: Lấy được {0} details thành công, MasterId={1}", detailDtos.Count, stockInOutMasterId);
            
            return detailDtos;
        }
        catch (Exception ex)
        {
            _logger.Error($"GetStockInOutDetailsByMasterId: Lỗi lấy thông tin details: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy thông tin chi tiết phiếu nhập/xuất kho: {ex.Message}", ex);
        }
    }

    #endregion

}