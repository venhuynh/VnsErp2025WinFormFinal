using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dal.Exceptions;

namespace Dal.BaseDataAccess
{
    /// <summary>
    /// Helper chung cho Data Access: cung cấp tiện ích về khóa chính, kiểm tra dữ liệu, clone và so sánh.
    /// Dùng để giảm lặp code và tăng tính nhất quán ở các lớp DAL.
    /// </summary>
    public static class DataAccessHelper
    {
        #region Fields & Properties

        /// <summary>
        /// Bộ nhớ đệm PropertyInfo của khóa chính theo kiểu entity.
        /// </summary>
        private static readonly Dictionary<Type, PropertyInfo> _primaryKeyCache = new Dictionary<Type, PropertyInfo>();

        #endregion

        #region Validation & Utilities (New API)

        /// <summary>
        /// Lấy Property khóa chính của kiểu thực thể.
        /// </summary>
        /// <param name="entityType">Kiểu thực thể</param>
        /// <returns>PropertyInfo là khóa chính; có thể null nếu không tìm thấy</returns>
        /// <exception cref="ArgumentNullException">Khi <paramref name="entityType"/> null</exception>
        public static PropertyInfo GetPrimaryKeyProperty(Type entityType)
        {
            if (entityType == null)
                throw new ArgumentNullException(nameof(entityType));

            if (_primaryKeyCache.ContainsKey(entityType))
                return _primaryKeyCache[entityType];

            var properties = entityType.GetProperties();
            var primaryKeyProperty = properties.FirstOrDefault(p =>
                p.GetCustomAttributes(typeof(System.Data.Linq.Mapping.ColumnAttribute), false)
                 .Cast<System.Data.Linq.Mapping.ColumnAttribute>()
                 .Any(attr => attr.IsPrimaryKey));

            _primaryKeyCache[entityType] = primaryKeyProperty;
            return primaryKeyProperty;
        }

        /// <summary>
        /// Lấy giá trị khóa chính của một thực thể.
        /// </summary>
        /// <param name="entity">Thực thể cần đọc</param>
        /// <returns>Giá trị khóa chính</returns>
        /// <exception cref="ArgumentNullException">Khi <paramref name="entity"/> null</exception>
        /// <exception cref="DataAccessException">Khi không tìm thấy khóa chính</exception>
        public static object GetPrimaryKeyValue(object entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var primaryKeyProperty = GetPrimaryKeyProperty(entity.GetType());
            if (primaryKeyProperty == null)
                throw new DataAccessException($"Không tìm thấy primary key cho {entity.GetType().Name}");

            return primaryKeyProperty.GetValue(entity);
        }

        /// <summary>
        /// Đảm bảo entity khác null.
        /// </summary>
        /// <param name="entity">Entity cần kiểm tra</param>
        /// <param name="entityName">Tên entity (phục vụ thông báo lỗi)</param>
        /// <exception cref="ArgumentNullException">Khi <paramref name="entity"/> null</exception>
        public static void EnsureEntityNotNull(object entity, string entityName)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), $"{entityName} không được null");
        }

        /// <summary>
        /// Đảm bảo ID hợp lệ (khác null).
        /// </summary>
        /// <param name="id">ID cần kiểm tra</param>
        /// <param name="entityName">Tên entity (phục vụ thông báo lỗi)</param>
        /// <exception cref="ArgumentException">Khi <paramref name="id"/> null</exception>
        public static void EnsureValidId(object id, string entityName)
        {
            if (id == null)
                throw new ArgumentException($"{entityName} ID không được null", nameof(id));
        }

        /// <summary>
        /// Tạo biểu thức predicate so sánh khóa chính bằng giá trị chỉ định.
        /// </summary>
        /// <typeparam name="T">Kiểu thực thể</typeparam>
        /// <param name="id">Giá trị khóa chính</param>
        /// <returns>Biểu thức predicate dạng x =&gt; x.Id == id</returns>
        /// <exception cref="DataAccessException">Khi không tìm thấy khóa chính</exception>
        public static System.Linq.Expressions.Expression<Func<T, bool>> BuildIdPredicate<T>(object id)
        {
            var entityType = typeof(T);
            var primaryKeyProperty = GetPrimaryKeyProperty(entityType);

            if (primaryKeyProperty == null)
                throw new DataAccessException($"Không tìm thấy primary key cho {entityType.Name}");

            var parameter = System.Linq.Expressions.Expression.Parameter(entityType, "x");
            var property = System.Linq.Expressions.Expression.Property(parameter, primaryKeyProperty);
            var constant = System.Linq.Expressions.Expression.Constant(id);
            var equality = System.Linq.Expressions.Expression.Equal(property, constant);

            return System.Linq.Expressions.Expression.Lambda<Func<T, bool>>(equality, parameter);
        }

        /// <summary>
        /// Tạo bản sao (shallow copy) của một thực thể.
        /// </summary>
        /// <typeparam name="T">Kiểu thực thể</typeparam>
        /// <param name="source">Thực thể nguồn</param>
        /// <returns>Thực thể sao chép</returns>
        public static T CloneEntity<T>(T source) where T : class, new()
        {
            if (source == null)
                return null;

            var target = new T();
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                     .Where(p => p.CanRead && p.CanWrite);

            foreach (var property in properties)
            {
                var value = property.GetValue(source);
                property.SetValue(target, value);
            }

            return target;
        }

        /// <summary>
        /// Kiểm tra hai thực thể có cùng khóa chính hay không.
        /// </summary>
        /// <param name="entity1">Thực thể 1</param>
        /// <param name="entity2">Thực thể 2</param>
        /// <returns>true nếu cùng khóa chính</returns>
        public static bool HasSamePrimaryKey(object entity1, object entity2)
        {
            if (entity1 == null || entity2 == null)
                return false;

            if (entity1.GetType() != entity2.GetType())
                return false;

            var primaryKeyValue1 = GetPrimaryKeyValue(entity1);
            var primaryKeyValue2 = GetPrimaryKeyValue(entity2);

            return Equals(primaryKeyValue1, primaryKeyValue2);
        }

        /// <summary>
        /// Kiểm tra hai thực thể có khác nhau (theo giá trị property) hay không.
        /// </summary>
        /// <typeparam name="T">Kiểu thực thể</typeparam>
        /// <param name="original">Thực thể gốc</param>
        /// <param name="modified">Thực thể đã sửa</param>
        /// <returns>true nếu có thay đổi</returns>
        public static bool HasChanges<T>(T original, T modified)
        {
            if (original == null || modified == null)
                return true;

            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                     .Where(p => p.CanRead);

            foreach (var property in properties)
            {
                var originalValue = property.GetValue(original);
                var modifiedValue = property.GetValue(modified);

                if (!Equals(originalValue, modifiedValue))
                    return true;
            }

            return false;
        }

        #endregion

        #region Obsolete Aliases (Backward Compatibility)

        [Obsolete("Use GetPrimaryKeyProperty(Type) instead")]
        public static PropertyInfo LayPrimaryKeyProperty(Type entityType) => GetPrimaryKeyProperty(entityType);

        [Obsolete("Use GetPrimaryKeyValue(object) instead")]
        public static object LayGiaTriPrimaryKey(object entity) => GetPrimaryKeyValue(entity);

        [Obsolete("Use EnsureEntityNotNull(object, string) instead")]
        public static void KiemTraEntityNull(object entity, string entityName) => EnsureEntityNotNull(entity, entityName);

        [Obsolete("Use EnsureValidId(object, string) instead")]
        public static void KiemTraIdHopLe(object id, string entityName) => EnsureValidId(id, entityName);

        [Obsolete("Use BuildIdPredicate<T>(object) instead")]
        public static System.Linq.Expressions.Expression<Func<T, bool>> TaoPredicateTimTheoId<T>(object id) => BuildIdPredicate<T>(id);

        [Obsolete("Use HasSamePrimaryKey(object, object) instead")]
        public static bool CungPrimaryKey(object entity1, object entity2) => HasSamePrimaryKey(entity1, entity2);

        [Obsolete("Use HasChanges<T>(T, T) instead")]
        public static bool CoThayDoi<T>(T original, T modified) => HasChanges(original, modified);

        #endregion
    }
}
