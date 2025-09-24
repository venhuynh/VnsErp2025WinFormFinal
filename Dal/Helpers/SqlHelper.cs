using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Dal.Helpers
{
    /// <summary>
    /// Helper class cho các thao tác SQL
    /// </summary>
    public static class SqlHelper
    {
        #region thuocTinhDonGian

        private const int DEFAULT_TIMEOUT = 30;

        #endregion

        #region phuongThuc

        /// <summary>
        /// Tạo SqlParameter với giá trị
        /// </summary>
        /// <param name="parameterName">Tên parameter</param>
        /// <param name="value">Giá trị</param>
        /// <returns>SqlParameter</returns>
        public static SqlParameter TaoParameter(string parameterName, object value)
        {
            return new SqlParameter(parameterName, value ?? DBNull.Value);
        }

        /// <summary>
        /// Tạo SqlParameter với kiểu dữ liệu cụ thể
        /// </summary>
        /// <param name="parameterName">Tên parameter</param>
        /// <param name="dbType">Kiểu dữ liệu SQL</param>
        /// <param name="value">Giá trị</param>
        /// <returns>SqlParameter</returns>
        public static SqlParameter TaoParameter(string parameterName, SqlDbType dbType, object value)
        {
            var parameter = new SqlParameter(parameterName, dbType)
            {
                Value = value ?? DBNull.Value
            };
            return parameter;
        }

        /// <summary>
        /// Tạo SqlParameter cho output
        /// </summary>
        /// <param name="parameterName">Tên parameter</param>
        /// <param name="dbType">Kiểu dữ liệu SQL</param>
        /// <param name="size">Kích thước</param>
        /// <returns>SqlParameter</returns>
        public static SqlParameter TaoOutputParameter(string parameterName, SqlDbType dbType, int size = 0)
        {
            var parameter = new SqlParameter(parameterName, dbType)
            {
                Direction = ParameterDirection.Output,
                Size = size > 0 ? size : 0
            };
            return parameter;
        }

        /// <summary>
        /// Tạo SqlParameter cho return value
        /// </summary>
        /// <param name="parameterName">Tên parameter</param>
        /// <param name="dbType">Kiểu dữ liệu SQL</param>
        /// <returns>SqlParameter</returns>
        public static SqlParameter TaoReturnValueParameter(string parameterName, SqlDbType dbType)
        {
            var parameter = new SqlParameter(parameterName, dbType)
            {
                Direction = ParameterDirection.ReturnValue
            };
            return parameter;
        }

        /// <summary>
        /// Escape SQL string để tránh injection
        /// </summary>
        /// <param name="value">Giá trị cần escape</param>
        /// <returns>Giá trị đã escape</returns>
        public static string EscapeSqlString(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            return value.Replace("'", "''");
        }

        /// <summary>
        /// Tạo câu lệnh SELECT với WHERE conditions
        /// </summary>
        /// <param name="tableName">Tên bảng</param>
        /// <param name="columns">Danh sách cột</param>
        /// <param name="whereConditions">Điều kiện WHERE</param>
        /// <returns>Câu lệnh SELECT</returns>
        public static string TaoSelectStatement(string tableName, string columns = "*", string whereConditions = null)
        {
            var sql = new StringBuilder();
            sql.Append($"SELECT {columns} FROM {tableName}");
            
            if (!string.IsNullOrEmpty(whereConditions))
            {
                sql.Append($" WHERE {whereConditions}");
            }
            
            return sql.ToString();
        }

        /// <summary>
        /// Tạo câu lệnh INSERT
        /// </summary>
        /// <param name="tableName">Tên bảng</param>
        /// <param name="columns">Danh sách cột</param>
        /// <param name="values">Danh sách giá trị</param>
        /// <returns>Câu lệnh INSERT</returns>
        public static string TaoInsertStatement(string tableName, string[] columns, string[] values)
        {
            if (columns == null || values == null || columns.Length != values.Length)
                throw new ArgumentException("Số lượng cột và giá trị phải bằng nhau");

            var sql = new StringBuilder();
            sql.Append($"INSERT INTO {tableName} (");
            sql.Append(string.Join(", ", columns));
            sql.Append(") VALUES (");
            sql.Append(string.Join(", ", values));
            sql.Append(")");

            return sql.ToString();
        }

        /// <summary>
        /// Tạo câu lệnh UPDATE
        /// </summary>
        /// <param name="tableName">Tên bảng</param>
        /// <param name="setClause">Mệnh đề SET</param>
        /// <param name="whereClause">Mệnh đề WHERE</param>
        /// <returns>Câu lệnh UPDATE</returns>
        public static string TaoUpdateStatement(string tableName, string setClause, string whereClause = null)
        {
            var sql = new StringBuilder();
            sql.Append($"UPDATE {tableName} SET {setClause}");
            
            if (!string.IsNullOrEmpty(whereClause))
            {
                sql.Append($" WHERE {whereClause}");
            }
            
            return sql.ToString();
        }

        /// <summary>
        /// Tạo câu lệnh DELETE
        /// </summary>
        /// <param name="tableName">Tên bảng</param>
        /// <param name="whereClause">Mệnh đề WHERE</param>
        /// <returns>Câu lệnh DELETE</returns>
        public static string TaoDeleteStatement(string tableName, string whereClause)
        {
            if (string.IsNullOrEmpty(whereClause))
                throw new ArgumentException("WHERE clause là bắt buộc cho DELETE statement");

            return $"DELETE FROM {tableName} WHERE {whereClause}";
        }

        /// <summary>
        /// Tạo câu lệnh COUNT
        /// </summary>
        /// <param name="tableName">Tên bảng</param>
        /// <param name="whereClause">Mệnh đề WHERE</param>
        /// <param name="distinctColumn">Cột để đếm distinct</param>
        /// <returns>Câu lệnh COUNT</returns>
        public static string TaoCountStatement(string tableName, string whereClause = null, string distinctColumn = null)
        {
            var sql = new StringBuilder();
            
            if (!string.IsNullOrEmpty(distinctColumn))
            {
                sql.Append($"SELECT COUNT(DISTINCT {distinctColumn}) FROM {tableName}");
            }
            else
            {
                sql.Append($"SELECT COUNT(*) FROM {tableName}");
            }
            
            if (!string.IsNullOrEmpty(whereClause))
            {
                sql.Append($" WHERE {whereClause}");
            }
            
            return sql.ToString();
        }

        /// <summary>
        /// Tạo câu lệnh EXISTS
        /// </summary>
        /// <param name="tableName">Tên bảng</param>
        /// <param name="whereClause">Mệnh đề WHERE</param>
        /// <returns>Câu lệnh EXISTS</returns>
        public static string TaoExistsStatement(string tableName, string whereClause)
        {
            return $"SELECT CASE WHEN EXISTS(SELECT 1 FROM {tableName} WHERE {whereClause}) THEN 1 ELSE 0 END";
        }

        /// <summary>
        /// Tạo câu lệnh TOP
        /// </summary>
        /// <param name="sql">Câu lệnh SQL gốc</param>
        /// <param name="topCount">Số lượng record</param>
        /// <returns>Câu lệnh SQL với TOP</returns>
        public static string ThemTopClause(string sql, int topCount)
        {
            if (topCount <= 0)
                return sql;

            var upperSql = sql.TrimStart().ToUpper();
            if (upperSql.StartsWith("SELECT"))
            {
                return sql.Substring(0, 6) + $"TOP {topCount} " + sql.Substring(6);
            }

            return sql;
        }

        /// <summary>
        /// Tạo câu lệnh ORDER BY
        /// </summary>
        /// <param name="sql">Câu lệnh SQL gốc</param>
        /// <param name="orderByClause">Mệnh đề ORDER BY</param>
        /// <returns>Câu lệnh SQL với ORDER BY</returns>
        public static string ThemOrderByClause(string sql, string orderByClause)
        {
            if (string.IsNullOrEmpty(orderByClause))
                return sql;

            return $"{sql} ORDER BY {orderByClause}";
        }

        /// <summary>
        /// Tạo câu lệnh pagination
        /// </summary>
        /// <param name="sql">Câu lệnh SQL gốc</param>
        /// <param name="pageNumber">Số trang (bắt đầu từ 1)</param>
        /// <param name="pageSize">Kích thước trang</param>
        /// <param name="orderByClause">Mệnh đề ORDER BY</param>
        /// <returns>Câu lệnh SQL với pagination</returns>
        public static string TaoPaginationSql(string sql, int pageNumber, int pageSize, string orderByClause)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            var offset = (pageNumber - 1) * pageSize;
            
            if (string.IsNullOrEmpty(orderByClause))
                orderByClause = "(SELECT NULL)";

            return $@"
                SELECT * FROM (
                    SELECT *, ROW_NUMBER() OVER (ORDER BY {orderByClause}) AS RowNum
                    FROM ({sql}) AS InnerQuery
                ) AS PagedQuery
                WHERE RowNum > {offset} AND RowNum <= {offset + pageSize}";
        }

        /// <summary>
        /// Kiểm tra tên bảng có hợp lệ không
        /// </summary>
        /// <param name="tableName">Tên bảng</param>
        /// <returns>True nếu hợp lệ</returns>
        public static bool KiemTraTenBangHopLe(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
                return false;

            // Kiểm tra ký tự đặc biệt
            var invalidChars = new[] { ' ', ';', '\'', '"', '[', ']', '(', ')' };
            var invalidStrings = new[] { "--", "/*", "*/" };
            
            foreach (var invalidChar in invalidChars)
            {
                if (tableName.Contains(invalidChar.ToString()))
                    return false;
            }
            
            foreach (var invalidString in invalidStrings)
            {
                if (tableName.Contains(invalidString))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Kiểm tra tên cột có hợp lệ không
        /// </summary>
        /// <param name="columnName">Tên cột</param>
        /// <returns>True nếu hợp lệ</returns>
        public static bool KiemTraTenCotHopLe(string columnName)
        {
            return KiemTraTenBangHopLe(columnName);
        }

        /// <summary>
        /// Tạo tên parameter từ tên cột
        /// </summary>
        /// <param name="columnName">Tên cột</param>
        /// <returns>Tên parameter</returns>
        public static string TaoTenParameter(string columnName)
        {
            if (string.IsNullOrEmpty(columnName))
                return string.Empty;

            return $"@{columnName}";
        }

        /// <summary>
        /// Tạo tên parameter từ tên cột với prefix
        /// </summary>
        /// <param name="columnName">Tên cột</param>
        /// <param name="prefix">Prefix</param>
        /// <returns>Tên parameter</returns>
        public static string TaoTenParameter(string columnName, string prefix)
        {
            if (string.IsNullOrEmpty(columnName))
                return string.Empty;

            var parameterName = string.IsNullOrEmpty(prefix) ? columnName : $"{prefix}_{columnName}";
            return $"@{parameterName}";
        }

        /// <summary>
        /// Chuyển đổi kiểu dữ liệu C# sang SqlDbType
        /// </summary>
        /// <param name="type">Kiểu dữ liệu C#</param>
        /// <returns>SqlDbType tương ứng</returns>
        public static SqlDbType ChuyenDoiKieuDuLieu(Type type)
        {
            if (type == null)
                return SqlDbType.VarChar;

            var underlyingType = Nullable.GetUnderlyingType(type) ?? type;

            if (underlyingType == typeof(int))
                return SqlDbType.Int;
            if (underlyingType == typeof(long))
                return SqlDbType.BigInt;
            if (underlyingType == typeof(short))
                return SqlDbType.SmallInt;
            if (underlyingType == typeof(byte))
                return SqlDbType.TinyInt;
            if (underlyingType == typeof(bool))
                return SqlDbType.Bit;
            if (underlyingType == typeof(decimal))
                return SqlDbType.Decimal;
            if (underlyingType == typeof(double))
                return SqlDbType.Float;
            if (underlyingType == typeof(float))
                return SqlDbType.Real;
            if (underlyingType == typeof(DateTime))
                return SqlDbType.DateTime;
            if (underlyingType == typeof(Guid))
                return SqlDbType.UniqueIdentifier;
            if (underlyingType == typeof(byte[]))
                return SqlDbType.VarBinary;
            
            return SqlDbType.VarChar;
        }

        #endregion
    }
}
