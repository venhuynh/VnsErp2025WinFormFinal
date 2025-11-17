using System.Drawing;
using System.Linq;
using DevExpress.Utils;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;

namespace Common.Helpers
{
    /// <summary>
    /// Helper class để chuẩn hóa việc format cột thông tin nhân viên giữa các màn hình.
    /// Đảm bảo tính nhất quán về màu sắc, font, alignment và kích thước cột.
    /// </summary>
    public static class EmployeeInfoColumnFormatter
    {
        #region ========== CẤU HÌNH CỘT THÔNG TIN NHÂN VIÊN ==========

        /// <summary>
        /// Cấu hình cột thông tin nhân viên cơ bản (Mã nhân viên, Họ tên, Phòng ban, Chức vụ)
        /// </summary>
        /// <param name="gridView">GridView cần cấu hình</param>
        /// <param name="columnNames">Danh sách tên cột cần cấu hình</param>
        public static void ConfigureEmployeeInfoColumns(GridView gridView, string[] columnNames)
        {
            foreach (var columnName in columnNames)
            {
                var column = gridView.Columns[columnName];
                if (column != null)
                {
                    ConfigureSingleEmployeeInfoColumn(column, columnName);
                }
            }
        }

        /// <summary>
        /// Cấu hình một cột thông tin nhân viên cụ thể
        /// </summary>
        /// <param name="column">Cột cần cấu hình</param>
        /// <param name="columnName">Tên cột</param>
        private static void ConfigureSingleEmployeeInfoColumn(GridColumn column, string columnName)
        {

            // Cấu hình hiển thị cell
            column.AppearanceCell.Options.UseTextOptions = true;
            column.AppearanceCell.TextOptions.WordWrap = WordWrap.Wrap;
            column.AppearanceCell.TextOptions.VAlignment = VertAlignment.Center;

            // Cấu hình header
            column.AppearanceHeader.Options.UseTextOptions = true;
            column.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
            column.AppearanceHeader.TextOptions.VAlignment = VertAlignment.Center;
            column.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;

            // Thiết lập màu sắc và font cho header
            column.AppearanceHeader.BackColor = Color.LightSteelBlue;
            column.AppearanceHeader.Options.UseBackColor = true;
            column.AppearanceHeader.ForeColor = Color.DarkBlue;
            column.AppearanceHeader.Font = new Font(column.AppearanceHeader.Font, FontStyle.Bold);

            // Thiết lập chiều rộng cột theo loại
            SetColumnWidth(column, columnName);

        }

        /// <summary>
        /// Thiết lập chiều rộng cột theo loại cột
        /// </summary>
        /// <param name="column">Cột cần thiết lập</param>
        /// <param name="columnName">Tên cột</param>
        private static void SetColumnWidth(GridColumn column, string columnName)
        {
            switch (columnName.ToLower())
            {
                case "manhanvien":
                case "pin":
                    column.Width = 100; // Cột mã nhân viên
                    break;
                case "hoten":
                case "username":
                    column.Width = 150; // Cột họ tên
                    break;
                case "phongban":
                case "tenphongban":
                    column.Width = 120; // Cột phòng ban
                    break;
                case "chucvu":
                case "tenchucvu":
                    column.Width = 100; // Cột chức vụ
                    break;
                case "ngaychamcong":
                    column.Width = 100; // Cột ngày chấm công
                    break;
                default:
                    column.Width = 120; // Mặc định
                    break;
            }
        }

        #endregion

        #region ========== CẤU HÌNH CỘT ĐỘNG ==========

        /// <summary>
        /// Cấu hình các cột động (như cột ngày, giờ chấm công)
        /// </summary>
        /// <param name="gridView">GridView cần cấu hình</param>
        /// <param name="columnPrefix">Tiền tố của cột động (VD: "Ngay_", "GioChamCong_")</param>
        /// <param name="headerBackColor">Màu nền header</param>
        /// <param name="headerForeColor">Màu chữ header</param>
        /// <param name="columnWidth">Chiều rộng cột</param>
        private static void ConfigureDynamicColumns(GridView gridView, string columnPrefix,
            Color headerBackColor, Color headerForeColor, int columnWidth = 80)
        {
            var dynamicColumns = gridView.Columns
                .Where(c => c.FieldName.StartsWith(columnPrefix))
                .ToList();

            foreach (var column in dynamicColumns)
            {
                ConfigureSingleDynamicColumn(column, headerBackColor, headerForeColor, columnWidth);
            }

        }

        /// <summary>
        /// Cấu hình một cột động cụ thể
        /// </summary>
        /// <param name="column">Cột cần cấu hình</param>
        /// <param name="headerBackColor">Màu nền header</param>
        /// <param name="headerForeColor">Màu chữ header</param>
        /// <param name="columnWidth">Chiều rộng cột</param>
        private static void ConfigureSingleDynamicColumn(GridColumn column, Color headerBackColor,
            Color headerForeColor, int columnWidth)
        {

            // Cấu hình hiển thị cell
            column.AppearanceCell.Options.UseTextOptions = true;
            column.AppearanceCell.TextOptions.WordWrap = WordWrap.Wrap;
            column.AppearanceCell.TextOptions.VAlignment = VertAlignment.Center;
            column.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;

            // Cấu hình header
            column.AppearanceHeader.Options.UseTextOptions = true;
            column.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
            column.AppearanceHeader.TextOptions.VAlignment = VertAlignment.Center;
            column.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;

            // Thiết lập màu sắc và font cho header
            column.AppearanceHeader.BackColor = headerBackColor;
            column.AppearanceHeader.Options.UseBackColor = true;
            column.AppearanceHeader.ForeColor = headerForeColor;
            column.AppearanceHeader.Font = new Font(column.AppearanceHeader.Font, FontStyle.Bold);

            // Thiết lập chiều rộng cột
            column.Width = columnWidth;

        }

        #endregion

        #region ========== ÁP DỤNG STYLING ĐẶC BIỆT ==========

        /// <summary>
        /// Áp dụng định dạng đặc biệt cho các cột thông tin nhân viên
        /// </summary>
        /// <param name="e">RowCellStyleEventArgs</param>
        /// <param name="columnName">Tên cột</param>
        public static void ApplyEmployeeInfoStyling(RowCellStyleEventArgs e, string columnName)
        {
            var lowerColumnName = columnName?.ToLower();

            switch (lowerColumnName)
            {
                case "manhanvien":
                case "pin":
                    // Cột mã nhân viên - màu xanh lá và đậm
                    e.Appearance.ForeColor = Color.DarkGreen;
                    e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                    break;

                case "hoten":
                case "username":
                    // Cột họ tên - màu xanh đậm cho dễ đọc
                    e.Appearance.ForeColor = Color.DarkBlue;
                    e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                    break;

                case "phongban":
                case "tenphongban":
                    // Cột phòng ban - màu tím
                    e.Appearance.ForeColor = Color.Purple;
                    e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                    break;

                case "chucvu":
                case "tenchucvu":
                    // Cột chức vụ - màu cam
                    e.Appearance.ForeColor = Color.DarkOrange;
                    e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                    break;

                case "ngaychamcong":
                    // Cột ngày chấm công - màu đỏ
                    e.Appearance.ForeColor = Color.DarkRed;
                    e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                    break;

                default:
                    // Các cột khác - màu đen bình thường
                    e.Appearance.ForeColor = Color.Black;
                    e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Regular);
                    break;
            }

        }

        /// <summary>
        /// Áp dụng định dạng đặc biệt cho các cột động
        /// </summary>
        /// <param name="e">RowCellStyleEventArgs</param>
        /// <param name="columnName">Tên cột</param>
        /// <param name="columnPrefix">Tiền tố cột động</param>
        /// <param name="foreColor">Màu chữ</param>
        public static void ApplyDynamicColumnStyling(RowCellStyleEventArgs e, string columnName,
            string columnPrefix, Color foreColor)
        {

            if (columnName?.StartsWith(columnPrefix) == true)
            {
                e.Appearance.ForeColor = foreColor;
                e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
            }

        }

        #endregion

        #region ========== CẤU HÌNH ROW HEIGHT ==========

        /// <summary>
        /// Cấu hình chiều cao dòng cho GridView
        /// </summary>
        /// <param name="gridView">GridView cần cấu hình</param>
        public static void ConfigureRowHeight(GridView gridView)
        {

            // Cấu hình chiều cao dòng cơ bản
            gridView.OptionsView.RowAutoHeight = true;

            // Cấu hình hiển thị header
            gridView.OptionsView.ColumnHeaderAutoHeight = DefaultBoolean.True;

            // Cấu hình word wrap cho header
            gridView.Appearance.HeaderPanel.Options.UseTextOptions = true;
            gridView.Appearance.HeaderPanel.TextOptions.WordWrap = WordWrap.Wrap;

            // Cấu hình hiển thị cell với word wrap
            gridView.Appearance.Row.Options.UseTextOptions = true;
            gridView.Appearance.Row.TextOptions.WordWrap = WordWrap.Wrap;

            // Cấu hình hiển thị cell được chọn
            gridView.Appearance.FocusedRow.Options.UseTextOptions = true;
            gridView.Appearance.FocusedRow.TextOptions.WordWrap = WordWrap.Wrap;

            // Cấu hình hiển thị cell được highlight
            gridView.Appearance.SelectedRow.Options.UseTextOptions = true;
            gridView.Appearance.SelectedRow.TextOptions.WordWrap = WordWrap.Wrap;

        }

        #endregion

        #region ========== CẤU HÌNH TỔNG HỢP ==========

        /// <summary>
        /// Cấu hình tổng hợp cho GridView với thông tin nhân viên
        /// </summary>
        /// <param name="gridView">GridView cần cấu hình</param>
        /// <param name="employeeInfoColumns">Danh sách cột thông tin nhân viên</param>
        /// <param name="dynamicColumnPrefix">Tiền tố cột động (optional)</param>
        /// <param name="dynamicColumnBackColor">Màu nền cột động (optional)</param>
        /// <param name="dynamicColumnForeColor">Màu chữ cột động (optional)</param>
        /// <param name="dynamicColumnWidth">Chiều rộng cột động (optional)</param>
        public static void ConfigureGridViewForEmployeeInfo(GridView gridView, string[] employeeInfoColumns,
            string dynamicColumnPrefix = null, Color? dynamicColumnBackColor = null,
            Color? dynamicColumnForeColor = null, int? dynamicColumnWidth = null)
        {

            // Cấu hình chiều cao dòng
            ConfigureRowHeight(gridView);

            // Cấu hình cột thông tin nhân viên
            ConfigureEmployeeInfoColumns(gridView, employeeInfoColumns);

            // Cấu hình cột động nếu có
            if (!string.IsNullOrEmpty(dynamicColumnPrefix))
            {
                var backColor = dynamicColumnBackColor ?? Color.LightGreen;
                var foreColor = dynamicColumnForeColor ?? Color.DarkGreen;
                var width = dynamicColumnWidth ?? 80;

                ConfigureDynamicColumns(gridView, dynamicColumnPrefix, backColor, foreColor, width);
            }
        }

        #endregion
    }
}
