using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Linq;
using Dal.BaseDataAccess;
using Dal.DataContext;
using Dal.Exceptions;
using Dal.Logging;
using Attribute = Dal.DataContext.Attribute;

namespace Dal.DataAccess.MasterData.ProductServiceDal
{
    /// <summary>
    /// Data Access cho thực thể Attribute (LINQ to SQL trên VnsErp2025DataContext).
    /// Cung cấp CRUD, tìm kiếm, kiểm tra unique và phiên bản đồng bộ/bất đồng bộ.
    /// </summary>
    public class AttributeDataAccess : BaseDataAccess<Attribute>
    {
        #region Constructors

        public AttributeDataAccess(ILogger logger = null) : base(logger)
        {
        }

        public AttributeDataAccess(string connectionString, ILogger logger = null) : base(connectionString, logger)
        {
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
                using var context = CreateContext();
                var load = new DataLoadOptions();
                load.LoadWith<Attribute>(a => a.AttributeValues);
                context.LoadOptions = load;
                return context.Attributes.FirstOrDefault(x => x.Id == id);
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi lấy Attribute theo Id {id}: {ex.Message}", ex);
            }
        }

        protected override Attribute GetById(object id)
        {
            if (id is Guid guid) return GetById(guid);
            return null;
        }

        /// <summary>
        /// Lấy tất cả Attribute.
        /// </summary>
        public override List<Attribute> GetAll()
        {
            try
            {
                using var context = CreateContext();
                var load = new DataLoadOptions();
                load.LoadWith<Attribute>(a => a.AttributeValues);
                context.LoadOptions = load;
                return context.Attributes
                    .OrderBy(x => x.Name)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi lấy tất cả Attribute: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy tất cả Attribute (Async).
        /// </summary>
        public override async Task<List<Attribute>> GetAllAsync()
        {
            try
            {
                using var context = CreateContext();
                var load = new DataLoadOptions();
                load.LoadWith<Attribute>(a => a.AttributeValues);
                context.LoadOptions = load;
                return await Task.FromResult(
                    context.Attributes
                        .OrderBy(x => x.Name)
                        .ToList()
                );
            }
            catch (Exception ex)
            {
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
                using var context = CreateContext();
                return context.Attributes.FirstOrDefault(x => x.Name == name.Trim());
            }
            catch (Exception ex)
            {
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
                using var context = CreateContext();
                if (string.IsNullOrWhiteSpace(keyword))
                    return context.Attributes.OrderBy(x => x.Name).ToList();

                var term = keyword.Trim().ToLower();
                return context.Attributes
                    .Where(x =>
                        x.Name.ToLower().Contains(term) ||
                        x.DataType.ToLower().Contains(term) ||
                        (x.Description != null && x.Description.ToLower().Contains(term)))
                    .OrderBy(x => x.Name)
                    .ToList();
            }
            catch (Exception ex)
            {
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
                if (entity == null) throw new ArgumentNullException(nameof(entity));

                using var context = CreateContext();
                var existing = entity.Id != Guid.Empty ? context.Attributes.FirstOrDefault(x => x.Id == entity.Id) : null;

                if (existing == null)
                {
                    if (entity.Id == Guid.Empty)
                        entity.Id = Guid.NewGuid();
                    context.Attributes.InsertOnSubmit(entity);
                }
                else
                {
                    existing.Name = entity.Name;
                    existing.DataType = entity.DataType;
                    existing.Description = entity.Description;
                }

                context.SubmitChanges();
            }
            catch (Exception ex)
            {
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
                using var context = CreateContext();
                var attr = context.Attributes.FirstOrDefault(x => x.Id == id);
                if (attr == null) return false;

                // Nếu còn phụ thuộc thì ngăn không cho xóa (an toàn theo yêu cầu)
                var hasVariantLinks = context.VariantAttributes.Any(x => x.AttributeId == id);
                var hasValues = context.AttributeValues.Any(x => x.AttributeId == id);
                if (hasVariantLinks || hasValues)
                {
                    throw new DataAccessException("Không thể xóa thuộc tính vì còn dữ liệu phụ thuộc (AttributeValue hoặc VariantAttribute).", null);
                }

                context.Attributes.DeleteOnSubmit(attr);
                context.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi xóa Attribute {id}: {ex.Message}", ex);
            }
        }

        public void Delete(Guid id)
        {
            DeleteAttribute(id);
        }

        public override bool DeleteById(Guid id)
        {
            return DeleteAttribute(id);
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
                using var context = CreateContext();
                var query = context.Attributes.Where(x => x.Name == name.Trim());
                if (excludeId.HasValue)
                    query = query.Where(x => x.Id != excludeId.Value);
                return query.Any();
            }
            catch (Exception ex)
            {
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
                using var context = CreateContext();
                return context.AttributeValues.Any(x => x.AttributeId == id)
                       || context.VariantAttributes.Any(x => x.AttributeId == id);
            }
            catch (Exception ex)
            {
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
                using var context = CreateContext();
                var names = context.Attributes
                    .Where(x => !string.IsNullOrEmpty(x.Name))
                    .Select(x => x.Name)
                    .Distinct()
                    .OrderBy(x => x)
                    .ToList();
                return await Task.FromResult(names.Cast<object>().ToList());
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi lấy danh sách tên Attribute unique: {ex.Message}", ex);
            }
        }

        #endregion
    }
}
