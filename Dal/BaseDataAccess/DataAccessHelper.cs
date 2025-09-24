using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dal.Exceptions;

namespace Dal.BaseDataAccess
{
    /// <summary>
    /// Helper methods chung cho Data Access
    /// </summary>
    public static class DataAccessHelper
    {
        #region thuocTinhDonGian

        private static readonly Dictionary<Type, PropertyInfo> PrimaryKeyCache = new Dictionary<Type, PropertyInfo>();

        #endregion

        #region phuongThuc

        /// <summary>
        /// Lấy primary key property của entity type
        /// </summary>
        /// <param name="entityType">Entity type</param>
        /// <returns>Primary key property</returns>
        public static PropertyInfo LayPrimaryKeyProperty(Type entityType)
        {
            if (entityType == null)
                throw new ArgumentNullException(nameof(entityType));

            if (PrimaryKeyCache.ContainsKey(entityType))
                return PrimaryKeyCache[entityType];

            var properties = entityType.GetProperties();
            var primaryKeyProperty = properties.FirstOrDefault(p => 
                p.GetCustomAttributes(typeof(System.Data.Linq.Mapping.ColumnAttribute), false)
                 .Cast<System.Data.Linq.Mapping.ColumnAttribute>()
                 .Any(attr => attr.IsPrimaryKey));

            PrimaryKeyCache[entityType] = primaryKeyProperty;
            return primaryKeyProperty;
        }

        /// <summary>
        /// Lấy giá trị primary key của entity
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Giá trị primary key</returns>
        public static object LayGiaTriPrimaryKey(object entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var primaryKeyProperty = LayPrimaryKeyProperty(entity.GetType());
            if (primaryKeyProperty == null)
                throw new DataAccessException($"Không tìm thấy primary key cho {entity.GetType().Name}");

            return primaryKeyProperty.GetValue(entity);
        }

        /// <summary>
        /// Kiểm tra entity có null hay không
        /// </summary>
        /// <param name="entity">Entity cần kiểm tra</param>
        /// <param name="entityName">Tên entity (cho error message)</param>
        public static void KiemTraEntityNull(object entity, string entityName)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), $"{entityName} không được null");
        }

        /// <summary>
        /// Kiểm tra ID có hợp lệ hay không
        /// </summary>
        /// <param name="id">ID cần kiểm tra</param>
        /// <param name="entityName">Tên entity (cho error message)</param>
        public static void KiemTraIdHopLe(object id, string entityName)
        {
            if (id == null)
                throw new ArgumentException($"{entityName} ID không được null", nameof(id));
        }

        /// <summary>
        /// Tạo predicate để tìm entity theo primary key
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="id">Primary key value</param>
        /// <returns>Predicate function</returns>
        public static System.Linq.Expressions.Expression<Func<T, bool>> TaoPredicateTimTheoId<T>(object id)
        {
            var entityType = typeof(T);
            var primaryKeyProperty = LayPrimaryKeyProperty(entityType);
            
            if (primaryKeyProperty == null)
                throw new DataAccessException($"Không tìm thấy primary key cho {entityType.Name}");

            var parameter = System.Linq.Expressions.Expression.Parameter(entityType, "x");
            var property = System.Linq.Expressions.Expression.Property(parameter, primaryKeyProperty);
            var constant = System.Linq.Expressions.Expression.Constant(id);
            var equality = System.Linq.Expressions.Expression.Equal(property, constant);
            
            return System.Linq.Expressions.Expression.Lambda<Func<T, bool>>(equality, parameter);
        }

        /// <summary>
        /// Clone entity (shallow copy)
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="source">Source entity</param>
        /// <returns>Cloned entity</returns>
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
        /// So sánh hai entities có cùng primary key hay không
        /// </summary>
        /// <param name="entity1">Entity 1</param>
        /// <param name="entity2">Entity 2</param>
        /// <returns>True nếu cùng primary key</returns>
        public static bool CungPrimaryKey(object entity1, object entity2)
        {
            if (entity1 == null || entity2 == null)
                return false;

            if (entity1.GetType() != entity2.GetType())
                return false;

            var primaryKeyValue1 = LayGiaTriPrimaryKey(entity1);
            var primaryKeyValue2 = LayGiaTriPrimaryKey(entity2);

            return Equals(primaryKeyValue1, primaryKeyValue2);
        }

        /// <summary>
        /// Kiểm tra entity có thay đổi hay không
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="original">Entity gốc</param>
        /// <param name="modified">Entity đã sửa</param>
        /// <returns>True nếu có thay đổi</returns>
        public static bool CoThayDoi<T>(T original, T modified)
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
    }
}
