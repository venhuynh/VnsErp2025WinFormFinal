using Dal.DataAccess.Interfaces.MasterData.ProductService;
using Dal.DataContext;
using Dal.Exceptions;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Threading.Tasks;
using Attribute = Dal.DataContext.Attribute;
using CustomLogger = Logger.Interfaces.ILogger;

namespace Dal.DataAccess.Implementations.MasterData.ProductService;

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
        loadOptions.LoadWith<Attribute>(a => a.AttributeValues);
        context.LoadOptions = loadOptions;

        return context;
    }

    #endregion

    #region Read

    /// <summary>
    /// Lấy Attribute theo Id.
    /// </summary>
    public Attribute GetById(Guid id)
    {
        try
        {
            using var context = CreateNewContext();
            var attribute = context.Attributes.FirstOrDefault(x => x.Id == id);
            
            if (attribute != null)
            {
                _logger.Debug($"Đã lấy Attribute theo ID: {id} - {attribute.Name}");
            }
            
            return attribute;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy Attribute theo Id {id}: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy Attribute theo Id {id}: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Lấy tất cả Attribute.
    /// </summary>
    public List<Attribute> GetAll()
    {
        try
        {
            using var context = CreateNewContext();
            var attributes = context.Attributes
                .OrderBy(x => x.Name)
                .ToList();
            
            _logger.Debug($"Đã lấy {attributes.Count} Attribute");
            return attributes;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy tất cả Attribute: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy tất cả Attribute: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Lấy tất cả Attribute (Async).
    /// </summary>
    public async Task<List<Attribute>> GetAllAsync()
    {
        try
        {
            using var context = CreateNewContext();
            var attributes = await Task.Run(() => context.Attributes
                .OrderBy(x => x.Name)
                .ToList());
            
            _logger.Debug($"Đã lấy {attributes.Count} Attribute (async)");
            return attributes;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy tất cả Attribute: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy tất cả Attribute: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Lấy Attribute theo tên.
    /// </summary>
    public Attribute GetByName(string name)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(name)) return null;
            
            using var context = CreateNewContext();
            var attribute = context.Attributes.FirstOrDefault(x => x.Name == name.Trim());
            
            if (attribute != null)
            {
                _logger.Debug($"Đã lấy Attribute theo tên: {name} - {attribute.Name}");
            }
            
            return attribute;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy Attribute theo tên '{name}': {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy Attribute theo tên '{name}': {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Tìm kiếm Attribute theo từ khóa (Name/DataType/Description).
    /// </summary>
    public List<Attribute> Search(string keyword)
    {
        try
        {
            using var context = CreateNewContext();
            if (string.IsNullOrWhiteSpace(keyword))
            {
                var all = context.Attributes.OrderBy(x => x.Name).ToList();
                _logger.Debug($"Đã tìm kiếm Attribute (không có keyword): {all.Count} kết quả");
                return all;
            }

            var term = keyword.Trim().ToLower();
            var results = context.Attributes
                .Where(x =>
                    x.Name.ToLower().Contains(term) ||
                    x.DataType.ToLower().Contains(term) ||
                    (x.Description != null && x.Description.ToLower().Contains(term)))
                .OrderBy(x => x.Name)
                .ToList();
            
            _logger.Debug($"Đã tìm kiếm Attribute theo keyword '{keyword}': {results.Count} kết quả");
            return results;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi tìm kiếm Attribute với từ khóa '{keyword}': {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi tìm kiếm Attribute với từ khóa '{keyword}': {ex.Message}", ex);
        }
    }

    #endregion

    #region Create/Update

    /// <summary>
    /// Lưu hoặc cập nhật Attribute.
    /// </summary>
    public void SaveOrUpdate(Attribute entity)
    {
        try
        {
            if (entity == null) 
                throw new ArgumentNullException(nameof(entity));

            using var context = CreateNewContext();
            var existing = entity.Id != Guid.Empty 
                ? context.Attributes.FirstOrDefault(x => x.Id == entity.Id) 
                : null;

            if (existing == null)
            {
                // Thêm mới
                if (entity.Id == Guid.Empty)
                    entity.Id = Guid.NewGuid();
                    
                context.Attributes.InsertOnSubmit(entity);
                context.SubmitChanges();
                
                _logger.Info($"Đã thêm mới Attribute: {entity.Name}");
            }
            else
            {
                // Cập nhật
                existing.Name = entity.Name;
                existing.DataType = entity.DataType;
                existing.Description = entity.Description;
                
                context.SubmitChanges();
                
                _logger.Info($"Đã cập nhật Attribute: {existing.Name}");
            }
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lưu/cập nhật Attribute '{entity?.Name}': {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lưu/cập nhật Attribute '{entity?.Name}': {ex.Message}", ex);
        }
    }

    #endregion

    #region Delete

    /// <summary>
    /// Xóa Attribute theo Id (kèm xóa AttributeValue/VariantAttribute liên quan).
    /// </summary>
    public bool DeleteAttribute(Guid id)
    {
        try
        {
            using var context = CreateNewContext();
            var attr = context.Attributes.FirstOrDefault(x => x.Id == id);
            if (attr == null)
            {
                _logger.Warning($"Không tìm thấy Attribute để xóa: {id}");
                return false;
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
            return true;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi xóa Attribute {id}: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi xóa Attribute {id}: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Xóa Attribute theo Id (alias method).
    /// </summary>
    public void Delete(Guid id)
    {
        DeleteAttribute(id);
    }

    #endregion

    #region Validation & Utilities

    /// <summary>
    /// Kiểm tra tên thuộc tính đã tồn tại chưa (loại trừ Id khi cập nhật).
    /// </summary>
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

    /// <summary>
    /// Lấy danh sách tên Attribute (unique) - Async.
    /// </summary>
    public async Task<List<object>> GetUniqueNamesAsync()
    {
        try
        {
            using var context = CreateNewContext();
            var names = context.Attributes
                .Where(x => !string.IsNullOrEmpty(x.Name))
                .Select(x => x.Name)
                .Distinct()
                .OrderBy(x => x)
                .ToList();
            
            var result = await Task.FromResult(names.Cast<object>().ToList());
            _logger.Debug($"Đã lấy {result.Count} tên Attribute unique (async)");
            return result;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy danh sách tên Attribute unique: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy danh sách tên Attribute unique: {ex.Message}", ex);
        }
    }

    #endregion
}
