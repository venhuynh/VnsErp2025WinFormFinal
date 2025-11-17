using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraGrid.Views.Grid;
using DialogResult = DevExpress.Utils.CommonDialogs.Internal.DialogResult;

namespace Common.Helpers;

/// <summary>
/// Helper class để cấu hình GridView cho hiển thị nhiều dòng, filter thông minh và các tiện ích khác
/// </summary>
public static class GridViewHelper
{
    

    #region ========== ROW INDICATOR & DISPLAY ==========

    /// <summary>
    /// Custom draw row indicator để hiển thị số thứ tự (1, 2, 3...)
    /// </summary>
    /// <param name="gridView">GridView</param>
    /// <param name="e">RowIndicatorCustomDrawEventArgs</param>
    public static void CustomDrawRowIndicator(GridView gridView, RowIndicatorCustomDrawEventArgs e)
    {
        if (e.RowHandle >= 0)
            e.Info.DisplayText = (e.RowHandle + 1).ToString();
    }

    /// <summary>
    /// Lấy số lượng dòng hiển thị trong GridView
    /// </summary>
    /// <param name="gridView">GridView</param>
    /// <returns>Số lượng dòng</returns>
    public static int? GetDisplayRowCount(GridView gridView)
    {
        return gridView?.DataRowCount;
    }

    #endregion

    #region ========== SELECTION UTILITIES ==========

    /// <summary>
    /// Lấy danh sách giá trị của một cột từ các dòng được chọn
    /// </summary>
    /// <typeparam name="T">Kiểu dữ liệu</typeparam>
    /// <param name="sender">GridView</param>
    /// <param name="columnName">Tên cột</param>
    /// <returns>Danh sách giá trị</returns>
    public static List<T> GetSelectedRowColumnValues<T>(object sender, string columnName)
    {
        List<T> selectedValues = [];

        try
        {
            if (sender is not GridView view || string.IsNullOrEmpty(columnName))
                return selectedValues;

            var selectedIndex = view.GetSelectedRows();

            foreach (var t in selectedIndex)
            {
                var selectedValue = view.GetRowCellValue(t, columnName);

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

    #region ========== EXPORT UTILITIES ==========

    /// <summary>
    /// Xuất GridView ra file Excel
    /// </summary>
    /// <param name="gridView">GridView cần xuất</param>
    /// <param name="fileName">Tên file mặc định</param>
    public static void ExportGridControl(GridView gridView, string fileName)
    {
        var saveFileDialog = new SaveFileDialog
        {
            CreatePrompt = true,
            OverwritePrompt = true,
            FileName = fileName,
            DefaultExt = "xlsx",
            Filter = @"Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*",
            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        };

        if (saveFileDialog.ShowDialog() != (System.Windows.Forms.DialogResult)DialogResult.OK) return;

        try
        {
            // Tắt AutoWidth để xuất toàn bộ cột
            gridView.OptionsPrint.AutoWidth = false;

            // Nếu là BandedGridView thì cast ra để xử lý thêm
            if (gridView is BandedGridView bandedGridView)
            {
                bandedGridView.OptionsView.ColumnAutoWidth = false;
            }

            // Export giữ nguyên định dạng và màu sắc
            var options = new DevExpress.XtraPrinting.XlsxExportOptionsEx
            {
                ExportType = DevExpress.Export.ExportType.WYSIWYG, // Giữ màu
                ShowGridLines = true
            };

            gridView.ExportToXlsx(saveFileDialog.FileName, options);

            

            if (MsgBox.ShowYesNo("Xuất file thành công. Bạn có muốn mở file này không?", "Xác nhận", null))
            {
                Process.Start(saveFileDialog.FileName);
            }
        }
        catch (Exception e)
        {
            _logger.Error($"Lỗi khi xuất GridView '{gridView?.Name}' ra file Excel", e);
            MsgBox.ShowException(e);
        }
    }

    #endregion

    #region ========== BANDED GRID VIEW CONFIGURATION ==========

    /// <summary>
    /// Cấu hình BandedGridView để hiển thị dữ liệu xuống dòng (word wrap) cho các cột văn bản dài.
    /// Đồng thời bật tự động tính chiều cao dòng để hiển thị đầy đủ nội dung.
    /// </summary>
    /// <param name="gridView">BandedGridView cần cấu hình</param>
    /// <param name="textColumns">Danh sách tên các cột văn bản cần wrap</param>
    /// <param name="enableAutoFilter">Có bật auto filter row không</param>
    /// <param name="centerHeaders">Có căn giữa header không</param>
    private static void ConfigureMultiLineBandedGridView(BandedGridView gridView,
        List<string> textColumns = null,
        bool enableAutoFilter = true,
        bool centerHeaders = true)
    {
        try
        {
            if (gridView == null)
            {
                _logger.Warning("GridView is null, không thể cấu hình");
                return;
            }

            // Bật tự động điều chỉnh chiều cao dòng để wrap nội dung
            gridView.OptionsView.RowAutoHeight = true;

            // RepositoryItemMemoEdit cho wrap text
            var memo = CreateMemoEditor();

            // Áp dụng cho các cột văn bản
            if (textColumns != null && textColumns.Any())
            {
                foreach (var columnName in textColumns)
                {
                    ApplyMemoEditorToBandedColumn(gridView, columnName, memo);
                }
            }

            // Cấu hình filter và search
            if (enableAutoFilter)
            {
                ConfigureBandedGridViewFilter(gridView);
            }

            // Tùy chọn hiển thị: căn giữa tiêu đề cho đẹp
            if (centerHeaders)
            {
                gridView.Appearance.HeaderPanel.TextOptions.HAlignment = HorzAlignment.Center;
                gridView.Appearance.HeaderPanel.Options.UseTextOptions = true;
            }

            _logger.Info($"Đã cấu hình BandedGridView '{gridView.Name}' cho hiển thị nhiều dòng");
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi cấu hình BandedGridView '{gridView?.Name}' cho hiển thị nhiều dòng", ex);
            throw;
        }
    }

    /// <summary>
    /// Cấu hình BandedGridView với cấu hình mặc định cho các cột thông dụng
    /// </summary>
    /// <param name="gridView">BandedGridView cần cấu hình</param>
    /// <param name="entityType">Loại entity để áp dụng cấu hình mặc định</param>
    public static void ConfigureMultiLineBandedGridView(BandedGridView gridView, string entityType)
    {
        try
        {
            var textColumns = GetDefaultTextColumns(entityType);
            ConfigureMultiLineBandedGridView(gridView, textColumns, true, true);

            // Nếu là BandedGridView thì cast ra để xử lý thêm
            if (gridView is { } bandedGridView)
            {
                bandedGridView.OptionsView.ColumnAutoWidth = false;
            }

            _logger.Info($"Đã cấu hình BandedGridView '{gridView.Name}' với cấu hình mặc định cho {entityType}");
        }
        catch (Exception ex)
        {
            _logger.Error(
                $"Lỗi khi cấu hình BandedGridView '{gridView?.Name}' với cấu hình mặc định cho {entityType}", ex);
            throw;
        }
    }

    #endregion

    #region ========== GRID VIEW CONFIGURATION ==========

    /// <summary>
    /// Cấu hình GridView để hiển thị dữ liệu xuống dòng (word wrap) cho các cột văn bản dài.
    /// Đồng thời bật tự động tính chiều cao dòng để hiển thị đầy đủ nội dung.
    /// </summary>
    /// <param name="gridView">GridView cần cấu hình</param>
    /// <param name="textColumns">Danh sách tên các cột văn bản cần wrap</param>
    /// <param name="enableAutoFilter">Có bật auto filter row không</param>
    /// <param name="centerHeaders">Có căn giữa header không</param>
    private static void ConfigureMultiLineGridView(GridView gridView,
        List<string> textColumns = null,
        bool enableAutoFilter = true,
        bool centerHeaders = true)
    {
        try
        {
            if (gridView == null)
            {
                _logger.Warning("GridView is null, không thể cấu hình");
                return;
            }

            // Bật tự động điều chỉnh chiều cao dòng để wrap nội dung
            gridView.OptionsView.RowAutoHeight = true;

            // RepositoryItemMemoEdit cho wrap text
            var memo = CreateMemoEditor();

            // Áp dụng cho các cột văn bản
            if (textColumns != null && textColumns.Any())
            {
                foreach (var columnName in textColumns)
                {
                    ApplyMemoEditorToColumn(gridView, columnName, memo);
                }
            }

            // Cấu hình filter và search
            if (enableAutoFilter)
            {
                ConfigureGridViewFilter(gridView);
            }

            // Tùy chọn hiển thị: căn giữa tiêu đề cho đẹp
            if (centerHeaders)
            {
                gridView.Appearance.HeaderPanel.TextOptions.HAlignment = HorzAlignment.Center;
                gridView.Appearance.HeaderPanel.Options.UseTextOptions = true;
            }

            _logger.Info($"Đã cấu hình GridView '{gridView.Name}' cho hiển thị nhiều dòng");
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi cấu hình GridView '{gridView?.Name}' cho hiển thị nhiều dòng", ex);
            throw;
        }
    }

    /// <summary>
    /// Cấu hình GridView với cấu hình mặc định cho các cột thông dụng
    /// </summary>
    /// <param name="gridView">GridView cần cấu hình</param>
    /// <param name="entityType">Loại entity để áp dụng cấu hình mặc định</param>
    public static void ConfigureMultiLineGridView(GridView gridView, string entityType)
    {
        try
        {
            var textColumns = GetDefaultTextColumns(entityType);
            ConfigureMultiLineGridView(gridView, textColumns, true, true);

            _logger.Info($"Đã cấu hình GridView '{gridView.Name}' với cấu hình mặc định cho {entityType}");
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi cấu hình GridView '{gridView?.Name}' với cấu hình mặc định cho {entityType}",
                ex);
            throw;
        }
    }

    #endregion

    #region ========== PRIVATE HELPER METHODS ==========

    /// <summary>
    /// Tạo RepositoryItemMemoEdit cho wrap text
    /// </summary>
    /// <returns>RepositoryItemMemoEdit</returns>
    private static RepositoryItemMemoEdit CreateMemoEditor()
    {
        var memo = new RepositoryItemMemoEdit
        {
            WordWrap = true,
            AutoHeight = false
        };
        memo.Appearance.TextOptions.WordWrap = WordWrap.Wrap;
        return memo;
    }

    /// <summary>
    /// Áp dụng RepositoryItemMemoEdit cho cột BandedGridView
    /// </summary>
    /// <param name="gridView">BandedGridView</param>
    /// <param name="fieldName">Tên field của cột</param>
    /// <param name="memoEditor">RepositoryItemMemoEdit</param>
    private static void ApplyMemoEditorToBandedColumn(BandedGridView gridView, string fieldName,
        RepositoryItemMemoEdit memoEditor)
    {
        try
        {
            var column = gridView.Columns[fieldName];
            if (column != null)
            {
                column.ColumnEdit = memoEditor;
                _logger.Debug($"Đã áp dụng memo editor cho cột {fieldName} trong BandedGridView");
            }
            else
            {
                _logger.Warning($"Không tìm thấy cột {fieldName} trong BandedGridView");
            }
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi áp dụng memo editor cho cột {fieldName} trong BandedGridView", ex);
        }
    }

    /// <summary>
    /// Áp dụng RepositoryItemMemoEdit cho cột GridView
    /// </summary>
    /// <param name="gridView">GridView</param>
    /// <param name="fieldName">Tên field của cột</param>
    /// <param name="memoEditor">RepositoryItemMemoEdit</param>
    private static void ApplyMemoEditorToColumn(GridView gridView, string fieldName,
        RepositoryItemMemoEdit memoEditor)
    {
        try
        {
            var column = gridView.Columns[fieldName];
            if (column != null)
            {
                column.ColumnEdit = memoEditor;
                _logger.Debug($"Đã áp dụng memo editor cho cột {fieldName} trong GridView");
            }
            else
            {
                _logger.Warning($"Không tìm thấy cột {fieldName} trong GridView");
            }
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi áp dụng memo editor cho cột {fieldName} trong GridView", ex);
        }
    }

    /// <summary>
    /// Cấu hình filter cho BandedGridView
    /// </summary>
    /// <param name="gridView">BandedGridView</param>
    private static void ConfigureBandedGridViewFilter(BandedGridView gridView)
    {
        try
        {
            // Bật auto filter row
            gridView.OptionsView.ShowAutoFilterRow = true;

            // Cấu hình filter cho các cột
            ConfigureBandedColumnFilters(gridView);

            _logger.Debug($"Đã cấu hình filter cho BandedGridView '{gridView.Name}'");
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi cấu hình filter cho BandedGridView '{gridView?.Name}'", ex);
        }
    }

    /// <summary>
    /// Cấu hình filter cho GridView
    /// </summary>
    /// <param name="gridView">GridView</param>
    private static void ConfigureGridViewFilter(GridView gridView)
    {
        try
        {
            // Bật auto filter row
            gridView.OptionsView.ShowAutoFilterRow = true;

            // Cấu hình filter cho các cột
            ConfigureColumnFilters(gridView);

            _logger.Debug($"Đã cấu hình filter cho GridView '{gridView.Name}'");
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi cấu hình filter cho GridView '{gridView?.Name}'", ex);
        }
    }

    /// <summary>
    /// Cấu hình filter cho từng cột BandedGridView
    /// </summary>
    /// <param name="gridView">BandedGridView</param>
    private static void ConfigureBandedColumnFilters(BandedGridView gridView)
    {
        try
        {
            foreach (BandedGridColumn column in gridView.Columns)
            {
                ConfigureColumnFilter(column);
            }

            _logger.Debug($"Đã cấu hình filter cho {gridView.Columns.Count} cột trong BandedGridView");
        }
        catch (Exception ex)
        {
            _logger.Error("Lỗi khi cấu hình filter cho các cột BandedGridView", ex);
        }
    }

    /// <summary>
    /// Cấu hình filter cho từng cột GridView
    /// </summary>
    /// <param name="gridView">GridView</param>
    private static void ConfigureColumnFilters(GridView gridView)
    {
        try
        {
            foreach (GridColumn column in gridView.Columns)
            {
                ConfigureColumnFilter(column);
            }

            _logger.Debug($"Đã cấu hình filter cho {gridView.Columns.Count} cột trong GridView");
        }
        catch (Exception ex)
        {
            _logger.Error("Lỗi khi cấu hình filter cho các cột GridView", ex);
        }
    }

    /// <summary>
    /// Cấu hình filter cho một cột dựa trên kiểu dữ liệu
    /// </summary>
    /// <param name="column">Cột cần cấu hình</param>
    private static void ConfigureColumnFilter(GridColumn column)
    {
        try
        {
            if (column == null) return;

            var fieldType = column.ColumnType;
            var fieldName = column.FieldName?.ToLower();

            // Cấu hình filter dựa trên kiểu dữ liệu và tên field
            if (fieldType == typeof(string))
            {
                // Text columns - sử dụng Contains
                column.OptionsFilter.AutoFilterCondition = AutoFilterCondition.Contains;
            }
            else if (fieldType == typeof(bool))
            {
                // Boolean columns - sử dụng Equals
                column.OptionsFilter.AutoFilterCondition = AutoFilterCondition.Equals;
            }
            else if (IsNumericType(fieldType))
            {
                // Numeric columns - sử dụng GreaterOrEqual
                column.OptionsFilter.AutoFilterCondition = AutoFilterCondition.GreaterOrEqual;
            }
            else if (fieldType == typeof(DateTime))
            {
                // DateTime columns - sử dụng Equals
                column.OptionsFilter.AutoFilterCondition = AutoFilterCondition.Equals;
            }
            else
            {
                // Default - sử dụng Contains
                column.OptionsFilter.AutoFilterCondition = AutoFilterCondition.Contains;
            }

            _logger.Debug($"Đã cấu hình filter cho cột {column.FieldName} (Type: {fieldType.Name})");
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi cấu hình filter cho cột {column?.FieldName}", ex);
        }
    }

    /// <summary>
    /// Kiểm tra xem kiểu dữ liệu có phải là numeric không
    /// </summary>
    /// <param name="type">Kiểu dữ liệu</param>
    /// <returns>True nếu là numeric</returns>
    private static bool IsNumericType(Type type)
    {
        return type == typeof(int) || type == typeof(long) || type == typeof(float) ||
               type == typeof(double) || type == typeof(decimal) || type == typeof(short) ||
               type == typeof(byte) || type == typeof(uint) || type == typeof(ulong) ||
               type == typeof(ushort) || type == typeof(sbyte);
    }

    /// <summary>
    /// Lấy danh sách cột văn bản mặc định cho từng loại entity
    /// </summary>
    /// <param name="entityType">Loại entity</param>
    /// <returns>Danh sách tên cột</returns>
    private static List<string> GetDefaultTextColumns(string entityType)
    {
        var textColumns = new List<string>();

        switch (entityType?.ToLower())
        {
            case "biodata":
            case "userinfo":
                textColumns.AddRange(["UserName", "TenPhongBan", "TenChucVu", "Passwd"]);
                break;
            case "nhanvien":
            case "employee":
                textColumns.AddRange(["HoTen", "TenPhongBan", "TenChucVu", "DiaChi", "GhiChu"]);
                break;
            case "phongban":
            case "department":
                textColumns.AddRange(["TenPhongBan", "MoTa", "GhiChu"]);
                break;
            case "chucvu":
            case "position":
                textColumns.AddRange(["TenChucVu", "MoTa", "GhiChu"]);
                break;
            case "calamviec":
            case "workshift":
                textColumns.AddRange(["TenCa", "MoTa", "GhiChu"]);
                break;
            case "dulieuchamcongaudit":
            case "attlogaudit":
                textColumns.AddRange(["Pin", "UserName", "TenPhongBan", "TenChucVu", "DeviceName", "AttTimeStr"]);
                break;

            //Lệnh máy chấm công
            case "lenhmaychamcong":
                textColumns.AddRange(["DeviceSn", "Content", "Description", "ReturnValue", "ResponseTime", "TransTime", "CommitTime"]);
                break;

            //Chi tiết nhân viên
            case "chitietnhanvien":
            case "chitietnhanviendto":
            case "employeedetail":
                textColumns.AddRange([
                    "HoTen", "TenTiengHoa", "BoPhan", "ChucVu", "SoSoBHXH", "SoCCCD",
                    "NoiCapCCCD", "DanToc", "TonGiao", "QueQuan", "HKTT", "DiaChiCuTru",
                    "SoDT", "LyDoThoiViec"
                ]);
                break;

            default:
                // Mặc định cho các entity khác
                textColumns.AddRange(["Name", "Ten", "MoTa", "GhiChu", "Description"]);
                break;
        }

        return textColumns;
    }

    #endregion
}