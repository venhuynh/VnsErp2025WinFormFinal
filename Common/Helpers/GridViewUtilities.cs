using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using VNTA_NET_2025._02_Common.Logger;
using VNTA_NET_2025._02_Common.Logger.Configuration;
using VNTA_NET_2025._02_Common.Logger.Interfaces;

namespace VNTA_NET_2025._02_Common.Helpers;

/// <summary>
/// Utility class để xử lý các tác vụ tổng quát với GridView như filter, export, memo edit, v.v.
/// Tách biệt khỏi column formatting để dễ bảo trì.
/// </summary>
public static class GridViewUtilities
{
    #region ========== KHAI BÁO BIẾN ==========

    /// <summary>
    /// Logger để ghi log các hoạt động
    /// </summary>
    private static readonly ILogger _logger = LoggerFactory.CreateLogger(LogCategory.UI);

    #endregion

    #region ========== MEMO EDIT & WORD WRAP ==========

    /// <summary>
    /// Tạo RepositoryItemMemoEdit cho word wrap
    /// </summary>
    /// <returns>RepositoryItemMemoEdit được cấu hình</returns>
    public static RepositoryItemMemoEdit CreateMemoEditor()
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
    /// Áp dụng RepositoryItemMemoEdit cho cột GridView
    /// </summary>
    /// <param name="gridView">GridView</param>
    /// <param name="fieldName">Tên field của cột</param>
    /// <param name="memoEditor">RepositoryItemMemoEdit (optional, sẽ tạo mới nếu null)</param>
    public static void ApplyMemoEditorToColumn(GridView gridView, string fieldName, RepositoryItemMemoEdit memoEditor = null)
    {
        try
        {
            memoEditor ??= CreateMemoEditor();
            
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
    /// Áp dụng RepositoryItemMemoEdit cho cột BandedGridView
    /// </summary>
    /// <param name="gridView">BandedGridView</param>
    /// <param name="fieldName">Tên field của cột</param>
    /// <param name="memoEditor">RepositoryItemMemoEdit (optional, sẽ tạo mới nếu null)</param>
    public static void ApplyMemoEditorToBandedColumn(BandedGridView gridView, string fieldName, RepositoryItemMemoEdit memoEditor = null)
    {
        try
        {
            memoEditor ??= CreateMemoEditor();
            
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
    /// Áp dụng memo editor cho nhiều cột cùng lúc
    /// </summary>
    /// <param name="gridView">GridView</param>
    /// <param name="fieldNames">Danh sách tên field</param>
    public static void ApplyMemoEditorToColumns(GridView gridView, params string[] fieldNames)
    {
        var memoEditor = CreateMemoEditor();
        foreach (var fieldName in fieldNames)
        {
            ApplyMemoEditorToColumn(gridView, fieldName, memoEditor);
        }
    }

    #endregion

    #region ========== FILTER CONFIGURATION ==========

    /// <summary>
    /// Cấu hình filter cho GridView
    /// </summary>
    /// <param name="gridView">GridView</param>
    /// <param name="enableAutoFilter">Có bật auto filter row không</param>
    public static void ConfigureGridViewFilter(GridView gridView, bool enableAutoFilter = true)
    {
        try
        {
            if (enableAutoFilter)
            {
                gridView.OptionsView.ShowAutoFilterRow = true;
            }

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
    /// Cấu hình filter cho BandedGridView
    /// </summary>
    /// <param name="gridView">BandedGridView</param>
    /// <param name="enableAutoFilter">Có bật auto filter row không</param>
    public static void ConfigureBandedGridViewFilter(BandedGridView gridView, bool enableAutoFilter = true)
    {
        try
        {
            if (enableAutoFilter)
            {
                gridView.OptionsView.ShowAutoFilterRow = true;
            }

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
    /// Cấu hình filter cho một cột dựa trên kiểu dữ liệu
    /// </summary>
    /// <param name="column">Cột cần cấu hình</param>
    private static void ConfigureColumnFilter(GridColumn column)
    {
        try
        {
            if (column == null) return;

            var fieldType = column.ColumnType;

            // Cấu hình filter dựa trên kiểu dữ liệu
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

            _logger.Debug($"Đã cấu hình filter cho cột {column.FieldName} (Type: {fieldType?.Name})");
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

    #endregion

    #region ========== ADVANCED EXPORT ==========

    /// <summary>
    /// Xuất GridView ra file Excel với cấu hình nâng cao
    /// </summary>
    /// <param name="gridView">GridView cần xuất</param>
    /// <param name="fileName">Tên file mặc định</param>
    /// <param name="showDialog">Có hiển thị dialog lưu file không</param>
    /// <param name="autoOpen">Có tự động mở file sau khi xuất không</param>
    /// <returns>Đường dẫn file đã xuất (nếu thành công)</returns>
    public static string ExportGridToExcel(GridView gridView, string fileName = "Export", bool showDialog = true, bool autoOpen = true)
    {
        try
        {
            string filePath;

            if (showDialog)
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

                if (saveFileDialog.ShowDialog() != DialogResult.OK) return null;
                filePath = saveFileDialog.FileName;
            }
            else
            {
                filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), 
                    $"{fileName}_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
            }

            // Cấu hình export
            gridView.OptionsPrint.AutoWidth = false;

            // Export với màu sắc và định dạng
            var options = new DevExpress.XtraPrinting.XlsxExportOptionsEx
            {
                ExportType = DevExpress.Export.ExportType.WYSIWYG,
                ShowGridLines = true
            };

            gridView.ExportToXlsx(filePath, options);

            _logger.Info($"Đã xuất GridView '{gridView.Name}' ra file {filePath}");

            // Tự động mở file nếu yêu cầu
            if (autoOpen && showDialog)
            {
                var result = MessageBox.Show(
                    "Xuất file thành công. Bạn có muốn mở file này không?",
                    "Thông báo",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    Process.Start(filePath);
                }
            }
            else if (autoOpen && !showDialog)
            {
                Process.Start(filePath);
            }

            return filePath;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi xuất GridView '{gridView?.Name}' ra file Excel", ex);
            MessageBox.Show($"Lỗi khi xuất file: {ex.Message}", "Lỗi", 
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return null;
        }
    }

    #endregion

    #region ========== ENTITY TYPE CONFIGURATION ==========

    /// <summary>
    /// Lấy danh sách cột văn bản mặc định cho từng loại entity
    /// </summary>
    /// <param name="dtoType">Loại DTo</param>
    /// <returns>Danh sách tên cột</returns>
    public static List<string> GetDefaultTextColumns(string dtoType)
    {
        var textColumns = new List<string>();

        switch (dtoType?.ToLower())
        {
            //TODO:
            
            case "device":
            case "maychamcong":
                textColumns.AddRange(new[] { "DevName", "DevSN", "DevModel", "DevNote", "Description", "Remarks" });
                break;
            case "biodata":
            case "userinfo":
                textColumns.AddRange(new[] { "UserName", "TenPhongBan", "TenChucVu", "Passwd" });
                break;
            case "nhanvien":
            case "employee":
                textColumns.AddRange(new[] { "HoTen", "TenPhongBan", "TenChucVu", "DiaChi", "GhiChu" });
                break;
            case "phongban":
            case "department":
                textColumns.AddRange(new[] { "TenPhongBan", "MoTa", "GhiChu" });
                break;
            case "chucvu":
            case "position":
                textColumns.AddRange(new[] { "TenChucVu", "MoTa", "GhiChu" });
                break;
            case "calamviec":
            case "workshift":
                textColumns.AddRange(new[] { "TenCa", "MoTa", "GhiChu" });
                break;
            case "dulieuchamcongaudit":
            case "attlogaudit":
                textColumns.AddRange(new[] { "Pin", "UserName", "TenPhongBan", "TenChucVu", "DeviceName", "AttTimeStr" });
                break;
            case "lenhmaychamcong":
                textColumns.AddRange(new[] { "DeviceSn", "Content", "Description", "ReturnValue", "ResponseTime", "TransTime", "CommitTime" });
                break;
            case "chitietnhanvien":
            case "chitietnhanviendto":
            case "employeedetail":
                textColumns.AddRange(new[] {
                    "HoTen", "TenTiengHoa", "BoPhan", "ChucVu", "SoSoBHXH", "SoCCCD",
                    "NoiCapCCCD", "DanToc", "TonGiao", "QueQuan", "HKTT", "DiaChiCuTru",
                    "SoDT", "LyDoThoiViec"
                });
                break;
            default:
                // Mặc định cho các entity khác
                textColumns.AddRange(new[] { "Name", "Ten", "MoTa", "GhiChu", "Description" });
                break;
        }

        return textColumns;
    }

    #endregion
}