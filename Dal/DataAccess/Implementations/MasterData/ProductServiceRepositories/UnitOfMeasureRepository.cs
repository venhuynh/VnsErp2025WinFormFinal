using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Threading.Tasks;
using Dal.DataAccess.Interfaces.MasterData.ProductServiceRepositories;
using Dal.DataContext;
using Dal.DtoConverter;
using Dal.Exceptions;
using DTO.MasterData.ProductService;
using Logger;
using Logger.Configuration;
using CustomLogger = Logger.Interfaces.ILogger;

namespace Dal.DataAccess.Implementations.MasterData.ProductServiceRepositories;

/// <summary>
/// Data Access cho thực thể UnitOfMeasure (LINQ to SQL trên VnsErp2025DataContext).
/// Cung cấp CRUD, tìm kiếm, kiểm tra unique và phiên bản đồng bộ/bất đồng bộ.
/// </summary>
public class UnitOfMeasureRepository : IUnitOfMeasureRepository
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
    /// Khởi tạo một instance mới của class AttributeRepository.
    /// </summary>
    /// <param name="connectionString">Chuỗi kết nối cơ sở dữ liệu</param>
    /// <exception cref="ArgumentNullException">Ném ra khi connectionString là null</exception>
    public UnitOfMeasureRepository(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _logger = LoggerFactory.CreateLogger(LogCategory.DAL);
        _logger.Info("UnitOfMeasureRepository được khởi tạo với connection string");
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

        // Configure eager loading cho navigation properties (không cần vì dùng dictionary)
        // var loadOptions = new DataLoadOptions();
        // loadOptions.LoadWith<UnitOfMeasure>(b => b.ProductVariants);
        // context.LoadOptions = loadOptions;

        return context;
    }

    /// <summary>
    /// Lấy dictionary chứa số lượng ProductVariant theo UnitId
    /// </summary>
    /// <param name="context">DataContext</param>
    /// <param name="unitIds">Danh sách UnitId cần đếm</param>
    /// <returns>Dictionary với key = UnitId, value = Count</returns>
    private Dictionary<Guid, int> GetVariantCountDict(VnsErp2025DataContext context, List<Guid> unitIds)
    {
        if (unitIds == null || !unitIds.Any()) return new Dictionary<Guid, int>();

        var counts = context.ProductVariants
            .Where(pv => unitIds.Contains(pv.UnitId))
            .GroupBy(pv => pv.UnitId)
            .Select(g => new { UnitId = g.Key, Count = g.Count() })
            .ToList();

        return counts.ToDictionary(c => c.UnitId, c => c.Count);
    }

    #endregion

    #region ========== READ OPERATIONS ==========

    /// <summary>
    /// Lấy UnitOfMeasure theo Id.
    /// </summary>
    public UnitOfMeasureDto GetById(Guid id)
    {
        try
        {
            using var context = CreateNewContext();
            var entity = context.UnitOfMeasures.FirstOrDefault(x => x.Id == id);
            if (entity == null) return null;

            // Đếm số lượng ProductVariant
            var variantCount = context.ProductVariants.Count(pv => pv.UnitId == id);

            return entity.ToDto(variantCount);
        }
        catch (Exception ex)
        {
            throw new DataAccessException($"Lỗi khi lấy UnitOfMeasure theo Id {id}: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Lấy tất cả UnitOfMeasure.
    /// </summary>
    public List<UnitOfMeasureDto> GetAll()
    {
        try
        {
            using var context = CreateNewContext();
            var entities = context.UnitOfMeasures.OrderBy(x => x.Code).ToList();

            // Tạo dictionary chứa số lượng ProductVariant
            var unitIds = entities.Select(e => e.Id).ToList();
            var variantCountDict = GetVariantCountDict(context, unitIds);

            return entities.ToDtos(variantCountDict);
        }
        catch (Exception ex)
        {
            throw new DataAccessException($"Lỗi khi lấy tất cả UnitOfMeasure: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Lấy UnitOfMeasure theo mã.
    /// </summary>
    public UnitOfMeasureDto GetByCode(string code)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(code)) return null;
            using var context = CreateNewContext();
            var entity = context.UnitOfMeasures.FirstOrDefault(x => x.Code == code.Trim());
            if (entity == null) return null;

            // Đếm số lượng ProductVariant
            var variantCount = context.ProductVariants.Count(pv => pv.UnitId == entity.Id);

            return entity.ToDto(variantCount);
        }
        catch (Exception ex)
        {
            throw new DataAccessException($"Lỗi khi lấy UnitOfMeasure theo mã '{code}': {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Lấy UnitOfMeasure theo tên.
    /// </summary>
    public UnitOfMeasureDto GetByName(string name)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(name)) return null;
            using var context = CreateNewContext();
            var entity = context.UnitOfMeasures.FirstOrDefault(x => x.Name == name.Trim());
            if (entity == null) return null;

            // Đếm số lượng ProductVariant
            var variantCount = context.ProductVariants.Count(pv => pv.UnitId == entity.Id);

            return entity.ToDto(variantCount);
        }
        catch (Exception ex)
        {
            throw new DataAccessException($"Lỗi khi lấy UnitOfMeasure theo tên '{name}': {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Tìm kiếm UnitOfMeasure theo từ khóa (Code/Name/Description).
    /// </summary>
    public List<UnitOfMeasureDto> Search(string keyword)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(keyword)) return GetAll();
            using var context = CreateNewContext();
            var lowerKeyword = keyword.ToLower();
            var entities = context.UnitOfMeasures
                .Where(x => x.Code.ToLower().Contains(lowerKeyword) ||
                            x.Name.ToLower().Contains(lowerKeyword) ||
                            (x.Description != null && x.Description.ToLower().Contains(lowerKeyword)))
                .OrderBy(x => x.Code)
                .ToList();

            // Tạo dictionary chứa số lượng ProductVariant
            var unitIds = entities.Select(e => e.Id).ToList();
            var variantCountDict = GetVariantCountDict(context, unitIds);

            return entities.ToDtos(variantCountDict);
        }
        catch (Exception ex)
        {
            throw new DataAccessException($"Lỗi khi tìm kiếm UnitOfMeasure với từ khóa '{keyword}': {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Lấy UnitOfMeasure theo trạng thái hoạt động.
    /// </summary>
    public List<UnitOfMeasureDto> GetByStatus(bool isActive)
    {
        try
        {
            using var context = CreateNewContext();
            var entities = context.UnitOfMeasures
                .Where(x => x.IsActive == isActive)
                .OrderBy(x => x.Code)
                .ToList();

            // Tạo dictionary chứa số lượng ProductVariant
            var unitIds = entities.Select(e => e.Id).ToList();
            var variantCountDict = GetVariantCountDict(context, unitIds);

            return entities.ToDtos(variantCountDict);
        }
        catch (Exception ex)
        {
            throw new DataAccessException($"Lỗi khi lấy UnitOfMeasure theo trạng thái {isActive}: {ex.Message}", ex);
        }
    }

    #endregion

    #region ========== UPDATE OPERATIONS ==========

    /// <summary>
    /// Lưu hoặc cập nhật UnitOfMeasure.
    /// </summary>
    public void SaveOrUpdate(UnitOfMeasureDto dto)
    {
        try
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            using var context = CreateNewContext();
            var existing = dto.Id != Guid.Empty ? context.UnitOfMeasures.FirstOrDefault(x => x.Id == dto.Id) : null;

            // Convert DTO to Entity
            var entity = dto.ToEntity(existing);

            if (existing == null)
            {
                // Tạo mới
                if (entity.Id == Guid.Empty)
                    entity.Id = Guid.NewGuid();
                context.UnitOfMeasures.InsertOnSubmit(entity);
            }

            context.SubmitChanges();
        }
        catch (Exception ex)
        {
            throw new DataAccessException($"Lỗi khi lưu/cập nhật UnitOfMeasure '{dto?.Code}': {ex.Message}", ex);
        }
    }

    #endregion

    #region ========== DELETE OPERATIONS ==========

    /// <summary>
    /// Xóa UnitOfMeasure theo Id (kèm kiểm tra phụ thuộc).
    /// </summary>
    public bool DeleteUnitOfMeasure(Guid id)
    {
        try
        {
            using var context = CreateNewContext();
            var entity = context.UnitOfMeasures.FirstOrDefault(x => x.Id == id);
            if (entity == null) return false;

            // Kiểm tra phụ thuộc
            if (HasDependencies(id))
                throw new DataAccessException($"Không thể xóa UnitOfMeasure '{entity.Code}' vì đang được sử dụng trong ProductVariant");

            context.UnitOfMeasures.DeleteOnSubmit(entity);
            context.SubmitChanges();
            return true;
        }
        catch (Exception ex)
        {
            throw new DataAccessException($"Lỗi khi xóa UnitOfMeasure {id}: {ex.Message}", ex);
        }
    }

    #endregion

    #region ========== VALIDATION & EXISTS CHECKS ==========

    /// <summary>
    /// Kiểm tra mã UnitOfMeasure có tồn tại không.
    /// </summary>
    public bool IsCodeExists(string code, Guid? excludeId = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(code)) return false;
            using var context = CreateNewContext();
            var query = context.UnitOfMeasures.Where(x => x.Code == code.Trim());
            if (excludeId.HasValue)
                query = query.Where(x => x.Id != excludeId.Value);
            return query.Any();
        }
        catch (Exception ex)
        {
            throw new DataAccessException($"Lỗi khi kiểm tra mã UnitOfMeasure '{code}': {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Kiểm tra tên UnitOfMeasure có tồn tại không.
    /// </summary>
    public bool IsNameExists(string name, Guid? excludeId = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(name)) return false;
            using var context = CreateNewContext();
            var query = context.UnitOfMeasures.Where(x => x.Name == name.Trim());
            if (excludeId.HasValue)
                query = query.Where(x => x.Id != excludeId.Value);
            return query.Any();
        }
        catch (Exception ex)
        {
            throw new DataAccessException($"Lỗi khi kiểm tra tên UnitOfMeasure '{name}': {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Kiểm tra UnitOfMeasure có phụ thuộc hay không (ProductVariant).
    /// </summary>
    public bool HasDependencies(Guid id)
    {
        try
        {
            using var context = CreateNewContext();
            return context.ProductVariants.Any(x => x.UnitId == id);
        }
        catch (Exception ex)
        {
            throw new DataAccessException($"Lỗi khi kiểm tra phụ thuộc của UnitOfMeasure {id}: {ex.Message}", ex);
        }
    }

    #endregion

    #region ========== HELPER METHODS ==========

    /// <summary>
    /// Đếm số lượng UnitOfMeasure.
    /// </summary>
    public int GetCount()
    {
        try
        {
            using var context = CreateNewContext();
            return context.UnitOfMeasures.Count();
        }
        catch (Exception ex)
        {
            throw new DataAccessException($"Lỗi khi đếm số lượng UnitOfMeasure: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Đếm số lượng UnitOfMeasure theo trạng thái.
    /// </summary>
    public int GetCountByStatus(bool isActive)
    {
        try
        {
            using var context = CreateNewContext();
            return context.UnitOfMeasures.Count(x => x.IsActive == isActive);
        }
        catch (Exception ex)
        {
            throw new DataAccessException($"Lỗi khi đếm số lượng UnitOfMeasure theo trạng thái {isActive}: {ex.Message}", ex);
        }
    }

    #endregion
}