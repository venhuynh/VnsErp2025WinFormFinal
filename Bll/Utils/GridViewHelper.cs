using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace Bll.Utils
{
    public static class GridViewHelper
    {
        public static void CustomDrawRowIndicator(GridView gridView, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
        }


        public static int? GetDisplayRowCount(GridView gridView)
        {
            return gridView?.DataRowCount;
        }

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
                MsgBox.ShowException(ex);
            }

            return selectedValues;
        }

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

            if (saveFileDialog.ShowDialog() != DialogResult.OK) return;

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

                if (MsgBox.GetConfirmFromYesNoDialog("Xuất file thành công. Bạn có muốn mở file này không?"))
                {
                    Process.Start(saveFileDialog.FileName);
                }
            }
            catch (Exception e)
            {
                MsgBox.ShowException(e);
            }
        }
    }
}
