using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using DevExpress.Utils;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using VNTA_NET_2025._02_Common.Helpers;

namespace Common.Helpers;

/// <summary>
/// Helper class để chuẩn hóa việc format cột trong GridView giữa các màn hình.
/// Đảm bảo tính nhất quán về màu sắc, font, alignment và kích thước cột.
/// </summary>
public static class GridViewColumnHelper
{


    #region ========== CẤU HÌNH CỘT THÔNG TIN CƠ BẢN ==========

    /// <summary>
    /// Cấu hình cột thông tin cơ bản (ID, Tên, Mô tả, Trạng thái, v.v.)
    /// </summary>
    /// <param name="gridView">GridView cần cấu hình</param>
    /// <param name="columnNames">Danh sách tên cột cần cấu hình</param>
    public static void ConfigureBasicInfoColumns(GridView gridView, string[] columnNames)
    {
        try
        {
            foreach (var columnName in columnNames)
            {
                var column = gridView.Columns[columnName];
                if (column != null)
                {
                    ConfigureSingleBasicColumn(column, columnName);
                }
            }

        }
        catch (Exception ex)
        {
            throw;
        }
    }

    /// <summary>
    /// Cấu hình một cột thông tin cơ bản cụ thể
    /// </summary>
    /// <param name="column">Cột cần cấu hình</param>
    /// <param name="columnName">Tên cột</param>
    private static void ConfigureSingleBasicColumn(GridColumn column, string columnName)
    {
        try
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
            SetBasicColumnWidth(column, columnName);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    /// <summary>
    /// Thiết lập chiều rộng cột theo loại cột
    /// </summary>
    /// <param name="column">Cột cần thiết lập</param>
    /// <param name="columnName">Tên cột</param>
    private static void SetBasicColumnWidth(GridColumn column, string columnName)
    {
        var lowerColumnName = columnName.ToLower();

        switch (lowerColumnName)
        {
            // Cột ID và mã
            case "id":
            case "deviceid":
            case "madevice":
            case "devsn":
            case "devicesn":
            case "serialnumber":
                column.Width = 100;
                break;

            // Cột tên
            case "name":
            case "devname":
            case "devicename":
            case "ten":
            case "tenthietbi":
                column.Width = 180;
                break;

            // Cột vị trí
            case "location":
            case "devlocation":
            case "vitri":
            case "diachi":
                column.Width = 150;
                break;

            // Cột trạng thái
            case "status":
            case "trangthai":
            case "isinuse":
            case "isactive":
            case "isactivated":
            case "hoatdong":
            case "trangthaisudungtext":
            case "trangthaikichhoattext":
                column.Width = 120;
                break;

            // Cột IP và thông tin mạng
            case "ip":
            case "devip":
            case "ipaddress":
            case "port":
            case "cong":
            case "connectiontimeout":
                column.Width = 120;
                break;

            // Cột MAC Address
            case "mac":
            case "devmac":
            case "macaddress":
                column.Width = 140;
                break;

            // Cột model và device info
            case "_devicemodel":
            case "devicemodel":
            case "modelmay":
            case "model":
                column.Width = 150;
                break;

            // Cột mô tả
            case "description":
            case "mota":
            case "ghichu":
            case "note":
                column.Width = 200;
                break;

            // Cột ngày tháng
            case "createdate":
            case "updatedate":
            case "ngaytao":
            case "ngaycapnhat":
            case "lastconnection":
            case "lastrequest":
                column.Width = 130;
                break;

            // Cột version và firmware
            case "version":
            case "firmwareversion":
            case "phienban":
                column.Width = 110;
                break;

            default:
                column.Width = 120; // Mặc định
                break;
        }
    }

    #endregion



    #region ========== CẤU HÌNH CỘT ĐỘNG ==========

    /// <summary>
    /// Cấu hình các cột động (như cột ngày, giờ, dữ liệu biến đổi)
    /// </summary>
    /// <param name="gridView">GridView cần cấu hình</param>
    /// <param name="columnPrefix">Tiền tố của cột động (VD: "Ngay_", "GioChamCong_")</param>
    /// <param name="headerBackColor">Màu nền header</param>
    /// <param name="headerForeColor">Màu chữ header</param>
    /// <param name="columnWidth">Chiều rộng cột</param>
    public static void ConfigureDynamicColumns(GridView gridView, string columnPrefix,
        Color headerBackColor, Color headerForeColor, int columnWidth = 80)
    {
        try
        {
            var dynamicColumns = gridView.Columns
                .Cast<GridColumn>()
                .Where(c => c.FieldName.StartsWith(columnPrefix))
                .ToList();

            foreach (var column in dynamicColumns)
            {
                ConfigureSingleDynamicColumn(column, headerBackColor, headerForeColor, columnWidth);
            }


        }
        catch (Exception ex)
        {
            throw;
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
        try
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
        catch (Exception ex)
        {
            throw;
        }
    }

    #endregion

    #region ========== ÁP DỤNG STYLING ĐẶC BIỆT ==========

    /// <summary>
    /// Áp dụng định dạng đặc biệt cho các cột thông tin cơ bản
    /// </summary>
    /// <param name="e">RowCellStyleEventArgs</param>
    /// <param name="columnName">Tên cột</param>
    public static void ApplyBasicInfoStyling(RowCellStyleEventArgs e, string columnName)
    {
        try
        {
            var lowerColumnName = columnName?.ToLower();

            switch (lowerColumnName)
            {
                case "id":
                case "deviceid":
                case "devsn":
                    // Cột ID và Serial - màu xanh lá và đậm
                    e.Appearance.ForeColor = Color.DarkGreen;
                    e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                    break;

                case "devname":
                case "name":
                case "devicename":
                    // Cột tên - màu xanh đậm
                    e.Appearance.ForeColor = Color.DarkBlue;
                    e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                    break;

                case "devlocation":
                case "location":
                    // Cột vị trí - màu tím
                    e.Appearance.ForeColor = Color.Purple;
                    break;

                case "isinuse":
                case "isactive":
                case "isactivated":
                case "trangthai":
                case "trangthaisudungtext":
                case "trangthaikichhoattext":
                    // Cột trạng thái - màu theo trạng thái
                    ApplyStatusStyling(e);
                    break;

                case "devip":
                case "ipaddress":
                    // Cột IP - màu cam
                    e.Appearance.ForeColor = Color.DarkOrange;
                    break;

                case "devmac":
                case "macaddress":
                    // Cột MAC - màu nâu
                    e.Appearance.ForeColor = Color.Brown;
                    break;

                case "_devicemodel":
                case "devicemodel":
                case "modelmay":
                case "vendorname":
                    // Cột model và vendor - màu xanh navy
                    e.Appearance.ForeColor = Color.Navy;
                    e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                    break;

                default:
                    // Các cột khác - màu đen bình thường
                    e.Appearance.ForeColor = Color.Black;
                    break;
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    /// <summary>
    /// Áp dụng styling cho cột trạng thái
    /// </summary>
    /// <param name="e">RowCellStyleEventArgs</param>
    private static void ApplyStatusStyling(RowCellStyleEventArgs e)
    {
        try
        {
            var cellValue = e.CellValue?.ToString()?.ToLower();

            switch (cellValue)
            {
                case "true":
                case "1":
                case "active":
                case "hoạt động":
                case "đang sử dụng":
                case "đang hoạt động":
                case "đã kích hoạt":
                case "kích hoạt":
                    e.Appearance.ForeColor = Color.Green;
                    e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                    break;

                case "false":
                case "0":
                case "inactive":
                case "không hoạt động":
                case "không sử dụng":
                case "chưa kích hoạt":
                case "không kích hoạt":
                case "tạm dừng":
                    e.Appearance.ForeColor = Color.Red;
                    e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                    break;

                default:
                    e.Appearance.ForeColor = Color.Gray;
                    break;
            }
        }
        catch (Exception ex)
        {
            throw;
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
        try
        {
            if (columnName?.StartsWith(columnPrefix) == true)
            {
                e.Appearance.ForeColor = foreColor;
                e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
            }
        }
        catch (Exception ex)
        {
            throw;
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
        try
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
        catch (Exception ex)
        {
            throw;
        }
    }

    #endregion

    #region ========== UTILITY METHODS ==========

    /// <summary>
    /// Tự động resize tất cả các cột theo nội dung
    /// </summary>
    /// <param name="gridView">GridView cần resize</param>
    public static void AutoResizeColumns(GridView gridView)
    {
        try
        {
            gridView.BestFitColumns();

        }
        catch (Exception ex)
        {
            throw;
        }
    }

    /// <summary>
    /// Ẩn một hoặc nhiều cột
    /// </summary>
    /// <param name="gridView">GridView</param>
    /// <param name="columnNames">Danh sách tên cột cần ẩn</param>
    public static void HideColumns(GridView gridView, params string[] columnNames)
    {
        try
        {
            foreach (var columnName in columnNames)
            {
                var column = gridView.Columns[columnName];
                if (column != null)
                {
                    column.Visible = false;
                }
            }


        }
        catch (Exception ex)
        {
            throw;
        }
    }

    /// <summary>
    /// Hiển thị một hoặc nhiều cột
    /// </summary>
    /// <param name="gridView">GridView</param>
    /// <param name="columnNames">Danh sách tên cột cần hiển thị</param>
    public static void ShowColumns(GridView gridView, params string[] columnNames)
    {
        try
        {
            foreach (var columnName in columnNames)
            {
                var column = gridView.Columns[columnName];
                if (column != null)
                {
                    column.Visible = true;
                }
            }


        }
        catch (Exception ex)
        {
            throw;
        }
    }

    #endregion

    #region ========== CẤU HÌNH MULTILINE GRIDVIEW ==========

    /// <summary>
    /// Cấu hình GridView để hiển thị dữ liệu xuống dòng (word wrap) cho các cột văn bản dài.
    /// Đồng thời bật tự động tính chiều cao dòng để hiển thị đầy đủ nội dung.
    /// </summary>
    /// <param name="gridView">GridView cần cấu hình</param>
    /// <param name="dtoType">Loại entity để áp dụng cấu hình phù hợp</param>
    public static void ConfigureMultiLineGridView(GridView gridView, string dtoType = "Default")
    {
        try
        {
            if (gridView == null)
            {

                return;
            }

            // Cấu hình cơ bản cho word wrap
            gridView.OptionsView.RowAutoHeight = true;
            gridView.OptionsView.ColumnAutoWidth = false;
            gridView.OptionsView.AllowCellMerge = false;
            gridView.OptionsView.AllowHtmlDrawHeaders = true;

            // Cấu hình appearance cho text wrapping
            gridView.Appearance.Row.TextOptions.WordWrap = WordWrap.Wrap;
            gridView.Appearance.Row.Options.UseTextOptions = true;

            // Cấu hình header appearance
            gridView.Appearance.HeaderPanel.TextOptions.WordWrap = WordWrap.Wrap;
            gridView.Appearance.HeaderPanel.Options.UseTextOptions = true;

            // Cấu hình cột theo entity type với utilities helper
            var textColumns = GridViewUtilities.GetDefaultTextColumns(dtoType);
            foreach (var columnName in textColumns)
            {
                GridViewUtilities.ApplyMemoEditorToColumn(gridView, columnName);
            }

            // Cấu hình cột theo entity type
            ConfigureColumnWordWrap(gridView, dtoType);

            // Đặt height tối thiểu cho row
            gridView.RowHeight = 25;

            // Cấu hình filter row với utilities
            GridViewUtilities.ConfigureGridViewFilter(gridView, true);


        }
        catch (Exception ex)
        {
            throw;
        }
    }

    /// <summary>
    /// Cấu hình word wrap cho các cột theo loại entity
    /// </summary>
    /// <param name="gridView">GridView</param>
    /// <param name="dto">Loại entity</param>
    private static void ConfigureColumnWordWrap(GridView gridView, string dto)
    {
        try
        {
            // Lấy danh sách cột text từ DTO type hoặc sử dụng danh sách mặc định
            string[] textColumns = GetTextColumnsForDto(dto);

            foreach (GridColumn column in gridView.Columns)
            {
                if (textColumns.Any(tc => column.FieldName.IndexOf(tc, StringComparison.OrdinalIgnoreCase) >= 0))
                {
                    // Cấu hình word wrap cho cột text
                    column.AppearanceCell.TextOptions.WordWrap = WordWrap.Wrap;
                    column.AppearanceCell.Options.UseTextOptions = true;

                    // Đặt width phù hợp cho cột text
                    if (column.Width < 100)
                        column.Width = 150;
                    else if (column.Width > 300)
                        column.Width = 250;
                }
                else if (IsNumericColumn(column))
                {
                    // Cấu hình cho cột số
                    column.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Far;
                    column.AppearanceCell.Options.UseTextOptions = true;
                    column.Width = Math.Max(column.Width, 80);
                }
                else if (IsDateColumn(column))
                {
                    // Cấu hình cho cột ngày
                    column.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                    column.AppearanceCell.Options.UseTextOptions = true;
                    column.Width = Math.Max(column.Width, 120);
                }
            }


        }
        catch (Exception ex)
        {
            throw;
        }
    }

    /// <summary>
    /// Lấy danh sách cột text cần word wrap theo DTO type
    /// </summary>
    /// <param name="dtoType">Loại DTO</param>
    /// <returns>Danh sách tên cột cần word wrap</returns>
    private static string[] GetTextColumnsForDto(string dtoType)
    {
        try
        {
            // Nếu có thể lấy từ reflection, ưu tiên sử dụng
            var dynamicColumns = GetTextColumnsFromReflection(dtoType);
            if (dynamicColumns.Length > 0)
            {
                return dynamicColumns;
            }

            // Nếu không thì sử dụng danh sách được định nghĩa sẵn
            return dtoType switch
            {
                "Device" or "MayChamCongAddEditDto" =>
                    new[]
                    {
                        "DevName", "DevSN", "DevModel", "DevNote", "Description", "Remarks", "DevLocation", "VendorName"
                    },
                "DanhSachMayChamCongDto" =>
                    new[]
                    {
                        "DevName", "DevSn", "DevLocation", "DevIp", "DevMac", "_DeviceModel", "GhiChu", "ModelMay",
                        "NguoiTao", "NguoiCapNhat"
                    },
                "CaLamViec" =>
                    new[] { "TenCa", "MoTa", "GhiChu", "Description", "Note" },
                "Employee" =>
                    new[] { "HoTen", "ChucVu", "PhongBan", "DiaChi", "GhiChu", "Email", "SoDienThoai" },
                "NhanSu" =>
                    new[] { "HoTen", "ChucVu", "PhongBan", "DiaChi", "GhiChu", "Email", "SoDienThoai" },
                "BaoCao" =>
                    new[] { "TenBaoCao", "MoTa", "NoiDung", "GhiChu", "Description" },
                _ =>
                    new[] { "Name", "Description", "Note", "Remarks", "Comment", "MoTa", "GhiChu" }
            };
        }
        catch (Exception ex)
        {

            return new[] { "Name", "Description", "Note", "Remarks", "Comment" };
        }
    }

    /// <summary>
    /// Lấy danh sách cột text từ DTO type thông qua reflection
    /// </summary>
    /// <param name="dtoTypeName">Tên loại DTO</param>
    /// <returns>Danh sách tên cột text</returns>
    private static string[] GetTextColumnsFromReflection(string dtoTypeName)
    {
        try
        {
            // Tìm DTO type trong các assembly đã load
            var assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
            Type dtoType = null;

            foreach (var assembly in assemblies)
            {
                dtoType = assembly.GetTypes()
                    .FirstOrDefault(t => t.Name.Equals(dtoTypeName, StringComparison.OrdinalIgnoreCase) ||
                                         t.Name.EndsWith(dtoTypeName, StringComparison.OrdinalIgnoreCase));
                if (dtoType != null) break;
            }

            if (dtoType == null)
            {

                return new string[0];
            }

            // Lấy các property có kiểu string và tên chứa các từ khóa text
            var textProperties = dtoType.GetProperties()
                .Where(p => p.PropertyType == typeof(string) && IsTextProperty(p.Name))
                .Select(p => p.Name)
                .ToArray();

            return textProperties;
        }
        catch (Exception ex)
        {
            return new string[0];
        }
    }

    /// <summary>
    /// Kiểm tra xem property có phải là text property không
    /// </summary>
    /// <param name="propertyName">Tên property</param>
    /// <returns>True nếu là text property</returns>
    private static bool IsTextProperty(string propertyName)
    {
        var lowerName = propertyName.ToLower();

        // Các từ khóa cho text property
        var textKeywords = new[]
        {
            "name", "description", "note", "remark", "comment", "mota", "ghichu", "ten", "diachi",
            "email", "chucvu", "phongban", "vitri", "location", "address", "title", "content",
            "noidung", "thongtin", "info", "detail", "chitiet", "model", "version", "vendor"
        };

        return textKeywords.Any(keyword => lowerName.Contains(keyword));
    }

    /// <summary>
    /// Kiểm tra xem cột có phải là cột số không
    /// </summary>
    private static bool IsNumericColumn(GridColumn column)
    {
        var fieldName = column.FieldName.ToLower();
        return fieldName.Contains("id") || fieldName.Contains("count") ||
               fieldName.Contains("number") || fieldName.Contains("amount") ||
               fieldName.Contains("price") || fieldName.Contains("quantity") ||
               column.ColumnType == typeof(int) || column.ColumnType == typeof(decimal) ||
               column.ColumnType == typeof(double) || column.ColumnType == typeof(float);
    }

    /// <summary>
    /// Kiểm tra xem cột có phải là cột ngày không
    /// </summary>
    private static bool IsDateColumn(GridColumn column)
    {
        var fieldName = column.FieldName.ToLower();
        return fieldName.Contains("date") || fieldName.Contains("time") ||
               fieldName.Contains("created") || fieldName.Contains("updated") ||
               column.ColumnType == typeof(DateTime) || column.ColumnType == typeof(DateTime?);
    }

    #endregion

    #region ========== ROW & DISPLAY UTILITIES ==========

    /// <summary>
    /// Custom draw row indicator để hiển thị số thứ tự (1, 2, 3...)
    /// </summary>
    /// <param name="gridView">GridView</param>
    /// <param name="e">RowIndicatorCustomDrawEventArgs</param>
    public static void CustomDrawRowIndicator(GridView gridView,
        DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
    {
        if (e.RowHandle >= 0)
            e.Info.DisplayText = (e.RowHandle + 1).ToString();
    }

    /// <summary>
    /// Lấy số lượng dòng hiển thị trong GridView
    /// </summary>
    /// <param name="gridView">GridView</param>
    /// <returns>Số lượng dòng</returns>
    public static int GetDisplayRowCount(GridView gridView)
    {
        return gridView?.DataRowCount ?? 0;
    }

    /// <summary>
    /// Lấy danh sách giá trị của một cột từ các dòng được chọn
    /// </summary>
    /// <typeparam name="T">Kiểu dữ liệu</typeparam>
    /// <param name="gridView">GridView</param>
    /// <param name="columnName">Tên cột</param>
    /// <returns>Danh sách giá trị</returns>
    public static List<T> GetSelectedRowColumnValues<T>(GridView gridView, string columnName)
    {
        var selectedValues = new List<T>();

        try
        {
            if (gridView == null || string.IsNullOrEmpty(columnName))
                return selectedValues;

            var selectedIndex = gridView.GetSelectedRows();

            foreach (var rowHandle in selectedIndex)
            {
                var selectedValue = gridView.GetRowCellValue(rowHandle, columnName);

                if (selectedValue == null || selectedValue == DBNull.Value) continue;

                if (selectedValue is T typedValue)
                {
                    selectedValues.Add(typedValue);
                }
            }


        }
        catch (Exception ex)
        {
            throw;
        }

        return selectedValues;
    }

    #endregion

    #region ========== DTO-SPECIFIC CONFIGURATION ==========

    /// <summary>
    /// Cấu hình GridView cho DanhSachMayChamCongDto với thiết lập tối ưu
    /// </summary>
    /// <param name="gridView">GridView cần cấu hình</param>
    public static void ConfigureDanhSachMayChamCongColumns(GridView gridView)
    {
        try
        {
            if (gridView == null)
            {

                return;
            }

            // Cấu hình cột thông tin cơ bản cho DanhSachMayChamCongDto - dựa trên properties thực tế
            var basicColumns = new[]
            {
                "Id", "DevSn", "DevName", "DevLocation",
                "DevIp", "DevMac", "_DeviceModel", "IsInUse", "IsActivated",
                "Port", "ConnectionTimeout", "TrangThaiSuDungText", "TrangThaiKichHoatText"
            };

            ConfigureBasicInfoColumns(gridView, basicColumns);

            // Cấu hình multi-line cho DanhSachMayChamCong
            ConfigureMultiLineGridView(gridView, "DanhSachMayChamCongDto");

            // Cấu hình row height
            ConfigureRowHeight(gridView);

            // Ẩn các cột không cần thiết trong danh sách
            HideColumns(gridView, "NgayTao", "NgayCapNhat", "NguoiTao", "NguoiCapNhat",
                "GhiChu", "ConnectionTimeout", "ModelMay", "TrangThaiSuDung", "TrangThaiKichHoat");

            // Hiển thị các cột quan trọng cho danh sách
            ShowColumns(gridView, "Id", "DevSn", "DevName", "DevLocation", "DevIp",
                "IsInUse", "IsActivated", "_DeviceModel", "TrangThaiSuDungText");


        }
        catch (Exception ex)
        {
            throw;
        }
    }

    /// <summary>
    /// Cấu hình GridView cho Device/MayChamCong với thiết lập tối ưu
    /// </summary>
    /// <param name="gridView">GridView cần cấu hình</param>
    public static void ConfigureDeviceColumns(GridView gridView)
    {
        try
        {
            if (gridView == null)
            {

                return;
            }

            // Cấu hình cột thông tin cơ bản cho Device
            var basicColumns = new[]
            {
                "ID", "DeviceID", "DevSN", "DevName", "DevLocation",
                "DevIP", "DevMAC", "VendorName", "IsInUse"
            };

            ConfigureBasicInfoColumns(gridView, basicColumns);

            // Cấu hình multi-line cho Device
            ConfigureMultiLineGridView(gridView, "Device");

            // Cấu hình row height
            ConfigureRowHeight(gridView);

            // Ẩn các cột không cần thiết
            HideColumns(gridView, "CreateDate", "UpdateDate", "CreatedBy", "UpdatedBy");


        }
        catch (Exception ex)
        {
            throw;
        }
    }

    /// <summary>
    /// Cấu hình GridView theo DTO type tự động
    /// </summary>
    /// <param name="gridView">GridView cần cấu hình</param>
    /// <param name="dtoType">Loại DTO</param>
    public static void ConfigureColumnsByDtoType(GridView gridView, string dtoType)
    {
        try
        {
            if (gridView == null || string.IsNullOrEmpty(dtoType))
            {

                return;
            }

            switch (dtoType.ToLower())
            {
                case "danhsachmaychamcongdto":
                case "danhsachmaychamcong":
                    ConfigureDanhSachMayChamCongColumns(gridView);
                    break;

                case "device":
                case "maychamcongaddedtdto":
                case "maychamcong":
                    ConfigureDeviceColumns(gridView);
                    break;

                default:
                    // Cấu hình mặc định
                    ConfigureMultiLineGridView(gridView, dtoType);
                    ConfigureRowHeight(gridView);
                    AutoResizeColumns(gridView);
                    break;
            }


        }
        catch (Exception ex)
        {
            throw;
        }
    }

    #endregion

    #region ========== EXPORT UTILITIES ==========

    /// <summary>
    /// Xuất GridView ra file Excel (Wrapper cho GridViewUtilities)
    /// </summary>
    /// <param name="gridView">GridView cần xuất</param>
    /// <param name="fileName">Tên file mặc định</param>
    public static void ExportToExcel(GridView gridView, string fileName = "Export")
    {
        GridViewUtilities.ExportGridToExcel(gridView, fileName, true, true);
    }

    #endregion
}