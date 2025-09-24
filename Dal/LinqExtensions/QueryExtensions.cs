using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Dal.LinqExtensions
{
    /// <summary>
    /// Extension methods cho LINQ queries
    /// </summary>
    public static class QueryExtensions
    {
        #region thuocTinhDonGian

        #endregion

        #region phuongThuc

        /// <summary>
        /// Thêm điều kiện WHERE nếu condition là true
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="query">Query</param>
        /// <param name="condition">Điều kiện</param>
        /// <param name="predicate">Predicate function</param>
        /// <returns>Query với điều kiện WHERE</returns>
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> predicate)
        {
            return condition ? query.Where(predicate) : query;
        }

        /// <summary>
        /// Thêm điều kiện WHERE nếu value không null
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="query">Query</param>
        /// <param name="value">Giá trị kiểm tra</param>
        /// <param name="predicate">Predicate function</param>
        /// <returns>Query với điều kiện WHERE</returns>
        public static IQueryable<T> WhereIfNotNull<T>(this IQueryable<T> query, object value, Expression<Func<T, bool>> predicate)
        {
            return value != null ? query.Where(predicate) : query;
        }

        /// <summary>
        /// Thêm điều kiện WHERE nếu string không null hoặc empty
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="query">Query</param>
        /// <param name="value">String kiểm tra</param>
        /// <param name="predicate">Predicate function</param>
        /// <returns>Query với điều kiện WHERE</returns>
        public static IQueryable<T> WhereIfNotEmpty<T>(this IQueryable<T> query, string value, Expression<Func<T, bool>> predicate)
        {
            return !string.IsNullOrWhiteSpace(value) ? query.Where(predicate) : query;
        }

        /// <summary>
        /// Thêm điều kiện WHERE nếu Guid không empty
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="query">Query</param>
        /// <param name="value">Guid kiểm tra</param>
        /// <param name="predicate">Predicate function</param>
        /// <returns>Query với điều kiện WHERE</returns>
        public static IQueryable<T> WhereIfNotEmpty<T>(this IQueryable<T> query, Guid value, Expression<Func<T, bool>> predicate)
        {
            return value != Guid.Empty ? query.Where(predicate) : query;
        }

        /// <summary>
        /// Thêm điều kiện WHERE nếu DateTime không null
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="query">Query</param>
        /// <param name="value">DateTime nullable kiểm tra</param>
        /// <param name="predicate">Predicate function</param>
        /// <returns>Query với điều kiện WHERE</returns>
        public static IQueryable<T> WhereIfNotNull<T>(this IQueryable<T> query, DateTime? value, Expression<Func<T, bool>> predicate)
        {
            return value.HasValue ? query.Where(predicate) : query;
        }

        /// <summary>
        /// Sắp xếp theo property động
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="query">Query</param>
        /// <param name="propertyName">Tên property</param>
        /// <param name="ascending">Sắp xếp tăng dần (true) hay giảm dần (false)</param>
        /// <returns>Query đã sắp xếp</returns>
        public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> query, string propertyName, bool ascending = true)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                return query;

            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, propertyName);
            var lambda = Expression.Lambda(property, parameter);

            string methodName = ascending ? "OrderBy" : "OrderByDescending";
            var methodCall = Expression.Call(
                typeof(Queryable),
                methodName,
                new Type[] { typeof(T), property.Type },
                query.Expression,
                lambda);

            return query.Provider.CreateQuery<T>(methodCall);
        }

        /// <summary>
        /// Thêm ThenBy sau OrderBy
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="query">Query</param>
        /// <param name="propertyName">Tên property</param>
        /// <param name="ascending">Sắp xếp tăng dần (true) hay giảm dần (false)</param>
        /// <returns>Query đã sắp xếp thêm</returns>
        public static IQueryable<T> ThenByDynamic<T>(this IQueryable<T> query, string propertyName, bool ascending = true)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                return query;

            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, propertyName);
            var lambda = Expression.Lambda(property, parameter);

            string methodName = ascending ? "ThenBy" : "ThenByDescending";
            var methodCall = Expression.Call(
                typeof(Queryable),
                methodName,
                new Type[] { typeof(T), property.Type },
                query.Expression,
                lambda);

            return query.Provider.CreateQuery<T>(methodCall);
        }

        /// <summary>
        /// Pagination cho query
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="query">Query</param>
        /// <param name="pageNumber">Số trang (bắt đầu từ 1)</param>
        /// <param name="pageSize">Kích thước trang</param>
        /// <returns>Query đã phân trang</returns>
        public static IQueryable<T> Paginate<T>(this IQueryable<T> query, int pageNumber, int pageSize)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            return query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

        /// <summary>
        /// Lấy tổng số trang
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="query">Query</param>
        /// <param name="pageSize">Kích thước trang</param>
        /// <returns>Tổng số trang</returns>
        public static int GetTotalPages<T>(this IQueryable<T> query, int pageSize)
        {
            if (pageSize < 1) pageSize = 10;

            var totalCount = query.Count();
            return (int)Math.Ceiling((double)totalCount / pageSize);
        }

        /// <summary>
        /// Thêm điều kiện tìm kiếm theo từ khóa trong nhiều fields
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="query">Query</param>
        /// <param name="searchTerm">Từ khóa tìm kiếm</param>
        /// <param name="propertyNames">Danh sách tên properties</param>
        /// <returns>Query với điều kiện tìm kiếm</returns>
        public static IQueryable<T> SearchIn<T>(this IQueryable<T> query, string searchTerm, params string[] propertyNames)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || propertyNames == null || propertyNames.Length == 0)
                return query;

            var parameter = Expression.Parameter(typeof(T), "x");
            var searchLower = searchTerm.ToLower();

            Expression combinedExpression = null;

            foreach (var propertyName in propertyNames)
            {
                var property = Expression.Property(parameter, propertyName);
                var toString = Expression.Call(property, "ToString", null);
                var toLower = Expression.Call(toString, "ToLower", null);
                var contains = Expression.Call(toLower, "Contains", null, Expression.Constant(searchLower));

                if (combinedExpression == null)
                    combinedExpression = contains;
                else
                    combinedExpression = Expression.OrElse(combinedExpression, contains);
            }

            var lambda = Expression.Lambda<Func<T, bool>>(combinedExpression, parameter);
            return query.Where(lambda);
        }

        /// <summary>
        /// Lấy query với include relationships
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="query">Query</param>
        /// <param name="includes">Danh sách includes</param>
        /// <returns>Query với includes</returns>
        public static IQueryable<T> Include<T>(this IQueryable<T> query, params string[] includes)
        {
            // LINQ to SQL không hỗ trợ Include như EF Core
            // Method này chỉ để tương thích, không thực hiện gì
            return query;
        }

        #endregion
    }
}
