using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using Dal.DataAccess.Interfaces.MasterData.ProductServiceRepositories;
using Dal.DataContext;
using Dal.DtoConverter;
using Dal.Exceptions;
using DTO.MasterData.ProductService;
using Logger;
using Logger.Configuration;
using Attribute = Dal.DataContext.Attribute;
using CustomLogger = Logger.Interfaces.ILogger;

namespace Dal.DataAccess.Implementations.MasterData.ProductServiceRepositories;

/// <summary>
/// Data Access cho thực thể Attribute (LINQ to SQL trên VnsErp2025DataContext).
/// Cung cấp CRUD, tìm kiếm, kiểm tra unique và phiên bản đồng bộ/bất đồng bộ.
/// </summary>
public class AttributeRepository : IAttributeRepository
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
    public AttributeRepository(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _logger = LoggerFactory.CreateLogger(LogCategory.DAL);
        _logger.Info("AttributeRepository được khởi tạo với connection string");
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
        loadOptions.LoadWith<Attribute>(a => a.AttributeValues);
        context.LoadOptions = loadOptions;

        return context;
    }

    #endregion

    #region ========== CREATE OPERATIONS ==========

    /// <summary>
    /// Lưu hoặc cập nhật Attribute
    /// </summary>
    /// <param name="dto">AttributeDto</param>
    public void SaveOrUpdate(AttributeDto dto)
    {
        try
        {
            if (dto == null) 
                throw new ArgumentNullException(nameof(dto));

            using var context = CreateNewContext();
            var existing = dto.Id != Guid.Empty 
                ? context.Attributes.FirstOrDefault(x => x.Id == dto.Id) 
                : null;

            if (existing == null)
            {
                // Thêm mới
                var entity = dto.ToEntity();
                if (entity.Id == Guid.Empty)
                    entity.Id = Guid.NewGuid();
                    
                context.Attributes.InsertOnSubmit(entity);
                context.SubmitChanges();
                
                _logger.Info($"Đã thêm mới Attribute: {entity.Name}");
            }
            else
            {
                // Cập nhật - Sử dụng converter để cập nhật entity từ DTO
                dto.ToEntity(existing);
                
                context.SubmitChanges();
                
                _logger.Info($"Đã cập nhật Attribute: {existing.Name}");
            }
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lưu/cập nhật Attribute '{dto?.Name}': {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lưu/cập nhật Attribute '{dto?.Name}': {ex.Message}", ex);
        }
    }

    #endregion

    #region ========== READ OPERATIONS ==========

    /// <summary>
    /// Lấy Attribute theo Id
    /// </summary>
    /// <param name="id">Id của Attribute</param>
    /// <returns>AttributeDto hoặc null</returns>
    public AttributeDto GetById(Guid id)
    {
        try
        {
            using var context = CreateNewContext();
            var attribute = context.Attributes.FirstOrDefault(x => x.Id == id);
            
            if (attribute != null)
            {
                _logger.Debug($"Đã lấy Attribute theo ID: {id} - {attribute.Name}");
                return attribute.ToDto();
            }
            
            return null;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy Attribute theo Id {id}: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy Attribute theo Id {id}: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Lấy tất cả Attribute
    /// </summary>
    /// <returns>Danh sách AttributeDto</returns>
    public List<AttributeDto> GetAll()
    {
        try
        {
            using var context = CreateNewContext();
            var attributes = context.Attributes
                .OrderBy(x => x.Name)
                .ToList();
            
            var dtos = attributes.Select(a => a.ToDto()).ToList();
            
            _logger.Debug($"Đã lấy {dtos.Count} Attribute");
            return dtos;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy tất cả Attribute: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy tất cả Attribute: {ex.Message}", ex);
        }
    }

    #endregion

    #region ========== UPDATE OPERATIONS ==========
    // Update operations are handled by SaveOrUpdate method
    #endregion

    #region ========== DELETE OPERATIONS ==========

    /// <summary>
    /// Xóa Attribute theo Id (kèm xóa AttributeValue/VariantAttribute liên quan)
    /// </summary>
    /// <param name="id">Id của Attribute cần xóa</param>
    public void Delete(Guid id)
    {
        try
        {
            using var context = CreateNewContext();
            var attr = context.Attributes.FirstOrDefault(x => x.Id == id);
            if (attr == null)
            {
                _logger.Warning($"Không tìm thấy Attribute để xóa: {id}");
                return;
            }

            // Nếu còn phụ thuộc thì ngăn không cho xóa (an toàn theo yêu cầu)
            var hasVariantLinks = context.VariantAttributes.Any(x => x.AttributeId == id);
            var hasValues = context.AttributeValues.Any(x => x.AttributeId == id);
            if (hasVariantLinks || hasValues)
            {
                _logger.Warning($"Không thể xóa Attribute {id} - {attr.Name} vì còn dữ liệu phụ thuộc");
                throw new DataAccessException("Không thể xóa thuộc tính vì còn dữ liệu phụ thuộc (AttributeValue hoặc VariantAttribute).", null);
            }

            context.Attributes.DeleteOnSubmit(attr);
            context.SubmitChanges();
            
            _logger.Info($"Đã xóa Attribute: {id} - {attr.Name}");
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi xóa Attribute {id}: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi xóa Attribute {id}: {ex.Message}", ex);
        }
    }

    #endregion

    #region ========== VALIDATION & EXISTS CHECKS ==========

    /// <summary>
    /// Kiểm tra tên thuộc tính đã tồn tại chưa (loại trừ Id khi cập nhật)
    /// </summary>
    /// <param name="name">Tên cần kiểm tra</param>
    /// <param name="excludeId">Id cần loại trừ khỏi kiểm tra (dùng khi update)</param>
    /// <returns>True nếu tên đã tồn tại</returns>
    public bool IsNameExists(string name, Guid? excludeId = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(name)) return false;
            
            using var context = CreateNewContext();
            var query = context.Attributes.Where(x => x.Name == name.Trim());
            if (excludeId.HasValue)
                query = query.Where(x => x.Id != excludeId.Value);
                
            var result = query.Any();
            _logger.Debug($"IsNameExists: Name='{name}', ExcludeId={excludeId}, Result={result}");
            return result;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi kiểm tra tên Attribute '{name}': {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi kiểm tra tên Attribute '{name}': {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Kiểm tra thuộc tính có phụ thuộc hay không (AttributeValue/VariantAttribute)
    /// </summary>
    /// <param name="id">Id của Attribute</param>
    /// <returns>True nếu có phụ thuộc</returns>
    public bool HasDependencies(Guid id)
    {
        try
        {
            using var context = CreateNewContext();
            var result = context.AttributeValues.Any(x => x.AttributeId == id)
                       || context.VariantAttributes.Any(x => x.AttributeId == id);
            
            _logger.Debug($"HasDependencies check cho Attribute {id}: {result}");
            return result;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi kiểm tra phụ thuộc của Attribute {id}: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi kiểm tra phụ thuộc của Attribute {id}: {ex.Message}", ex);
        }
    }

    #endregion
}
