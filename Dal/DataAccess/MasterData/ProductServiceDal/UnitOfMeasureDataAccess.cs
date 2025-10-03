using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Linq;
using Dal.BaseDataAccess;
using Dal.DataContext;
using Dal.Exceptions;
using Dal.Logging;

namespace Dal.DataAccess.MasterData.ProductServiceDal
{
    /// <summary>
    /// Data Access cho thực thể UnitOfMeasure (LINQ to SQL trên VnsErp2025DataContext).
    /// Cung cấp CRUD, tìm kiếm, kiểm tra unique và phiên bản đồng bộ/bất đồng bộ.
    /// </summary>
    public class UnitOfMeasureDataAccess : BaseDataAccess<UnitOfMeasure>
    {
        #region Constructors

        public UnitOfMeasureDataAccess(ILogger logger = null) : base(logger)
        {
        }

        public UnitOfMeasureDataAccess(string connectionString, ILogger logger = null) : base(connectionString, logger)
        {
        }

        #endregion

        #region Read

        /// <summary>
        /// Lấy UnitOfMeasure theo Id.
        /// </summary>
        public UnitOfMeasure GetById(Guid id)
        {
            try
            {
                using var context = CreateContext();
                return context.UnitOfMeasures.FirstOrDefault(x => x.Id == id);
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi lấy UnitOfMeasure theo Id {id}: {ex.Message}", ex);
            }
        }

        public override UnitOfMeasure GetById(object id)
        {
            if (id is Guid guid) return GetById(guid);
            return null;
        }

        /// <summary>
        /// Lấy tất cả UnitOfMeasure.
        /// </summary>
        public override List<UnitOfMeasure> GetAll()
        {
            try
            {
                using var context = CreateContext();
                return context.UnitOfMeasures
                    .OrderBy(x => x.Code)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi lấy tất cả UnitOfMeasure: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy tất cả UnitOfMeasure (Async).
        /// </summary>
        public override async Task<List<UnitOfMeasure>> GetAllAsync()
        {
            try
            {
                using var context = CreateContext();
                return await Task.FromResult(
                    context.UnitOfMeasures
                        .OrderBy(x => x.Code)
                        .ToList()
                );
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi lấy tất cả UnitOfMeasure: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy UnitOfMeasure theo mã.
        /// </summary>
        public UnitOfMeasure GetByCode(string code)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(code)) return null;
                using var context = CreateContext();
                return context.UnitOfMeasures.FirstOrDefault(x => x.Code == code.Trim());
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi lấy UnitOfMeasure theo mã '{code}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy UnitOfMeasure theo tên.
        /// </summary>
        public UnitOfMeasure GetByName(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name)) return null;
                using var context = CreateContext();
                return context.UnitOfMeasures.FirstOrDefault(x => x.Name == name.Trim());
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi lấy UnitOfMeasure theo tên '{name}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Tìm kiếm UnitOfMeasure theo từ khóa (Code/Name/Description).
        /// </summary>
        public List<UnitOfMeasure> Search(string keyword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(keyword)) return GetAll();
                using var context = CreateContext();
                var lowerKeyword = keyword.ToLower();
                return context.UnitOfMeasures
                    .Where(x => x.Code.ToLower().Contains(lowerKeyword) ||
                               x.Name.ToLower().Contains(lowerKeyword) ||
                               (x.Description != null && x.Description.ToLower().Contains(lowerKeyword)))
                    .OrderBy(x => x.Code)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi tìm kiếm UnitOfMeasure với từ khóa '{keyword}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy UnitOfMeasure theo trạng thái hoạt động.
        /// </summary>
        public List<UnitOfMeasure> GetByStatus(bool isActive)
        {
            try
            {
                using var context = CreateContext();
                return context.UnitOfMeasures
                    .Where(x => x.IsActive == isActive)
                    .OrderBy(x => x.Code)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi lấy UnitOfMeasure theo trạng thái {isActive}: {ex.Message}", ex);
            }
        }

        #endregion

        #region Create/Update

        /// <summary>
        /// Lưu hoặc cập nhật UnitOfMeasure.
        /// </summary>
        public void SaveOrUpdate(UnitOfMeasure entity)
        {
            try
            {
                if (entity == null) throw new ArgumentNullException(nameof(entity));

                using var context = CreateContext();
                var existing = entity.Id != Guid.Empty ? context.UnitOfMeasures.FirstOrDefault(x => x.Id == entity.Id) : null;

                if (existing == null)
                {
                    if (entity.Id == Guid.Empty)
                        entity.Id = Guid.NewGuid();
                    context.UnitOfMeasures.InsertOnSubmit(entity);
                }
                else
                {
                    existing.Code = entity.Code;
                    existing.Name = entity.Name;
                    existing.Description = entity.Description;
                    existing.IsActive = entity.IsActive;
                }

                context.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi lưu/cập nhật UnitOfMeasure '{entity?.Code}': {ex.Message}", ex);
            }
        }

        #endregion

        #region Delete

        /// <summary>
        /// Xóa UnitOfMeasure theo Id (kèm kiểm tra phụ thuộc).
        /// </summary>
        public bool DeleteUnitOfMeasure(Guid id)
        {
            try
            {
                using var context = CreateContext();
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

        /// <summary>
        /// Xóa UnitOfMeasure theo Id (Async).
        /// </summary>
        public async Task<bool> DeleteUnitOfMeasureAsync(Guid id)
        {
            return await Task.FromResult(DeleteUnitOfMeasure(id));
        }

        #endregion

        #region Validation

        /// <summary>
        /// Kiểm tra mã UnitOfMeasure có tồn tại không.
        /// </summary>
        public bool IsCodeExists(string code, Guid? excludeId = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(code)) return false;
                using var context = CreateContext();
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
                using var context = CreateContext();
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
                using var context = CreateContext();
                return context.ProductVariants.Any(x => x.UnitId == id);
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi kiểm tra phụ thuộc của UnitOfMeasure {id}: {ex.Message}", ex);
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Lấy danh sách mã UnitOfMeasure (unique) - Async.
        /// </summary>
        public async Task<List<object>> GetUniqueCodesAsync()
        {
            try
            {
                using var context = CreateContext();
                var codes = context.UnitOfMeasures
                    .Where(x => !string.IsNullOrEmpty(x.Code))
                    .Select(x => x.Code)
                    .Distinct()
                    .OrderBy(x => x)
                    .ToList();
                return await Task.FromResult(codes.Cast<object>().ToList());
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi lấy danh sách mã UnitOfMeasure unique: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy danh sách tên UnitOfMeasure (unique) - Async.
        /// </summary>
        public async Task<List<object>> GetUniqueNamesAsync()
        {
            try
            {
                using var context = CreateContext();
                var names = context.UnitOfMeasures
                    .Where(x => !string.IsNullOrEmpty(x.Name))
                    .Select(x => x.Name)
                    .Distinct()
                    .OrderBy(x => x)
                    .ToList();
                return await Task.FromResult(names.Cast<object>().ToList());
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi lấy danh sách tên UnitOfMeasure unique: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Đếm số lượng UnitOfMeasure.
        /// </summary>
        public int GetCount()
        {
            try
            {
                using var context = CreateContext();
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
                using var context = CreateContext();
                return context.UnitOfMeasures.Count(x => x.IsActive == isActive);
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi đếm số lượng UnitOfMeasure theo trạng thái {isActive}: {ex.Message}", ex);
            }
        }

        #endregion
    }
}
