using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bll.Common;
using Bll.MasterData.Customer;
using Bll.Utils;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSplashScreen;
using MasterData.Customer.Converters;
using MasterData.Customer.Dto;

namespace MasterData.Customer
{
    public partial class UcBusinessPartnerContact : XtraUserControl
    {
        #region Fields

        private readonly BusinessPartnerContactBll _contactBll = new BusinessPartnerContactBll();
        private List<Guid> _selectedContactIds = new List<Guid>();
        private bool _isLoading; // guard tránh gọi LoadDataAsync song song

        #endregion

        #region Constructor

        public UcBusinessPartnerContact()
        {
            InitializeComponent();

            // Toolbar events
            ListDataBarButtonItem.ItemClick += ListDataBarButtonItem_ItemClick;
            NewBarButtonItem.ItemClick += NewBarButtonItem_ItemClick;
            EditBarButtonItem.ItemClick += EditBarButtonItem_ItemClick;
            DeleteBarButtonItem.ItemClick += DeleteBarButtonItem_ItemClick;
            ExportBarButtonItem.ItemClick += ExportBarButtonItem_ItemClick;

            // Grid events
            BusinessPartnerContactGridView.SelectionChanged += BusinessPartnerContactGridView_SelectionChanged;
            BusinessPartnerContactGridView.CustomDrawRowIndicator += BusinessPartnerContactGridView_CustomDrawRowIndicator;
            BusinessPartnerContactGridView.RowCellStyle += BusinessPartnerContactGridView_RowCellStyle;

            UpdateButtonStates();
        }

        #endregion

        #region Private Helper Methods

        private async Task ExecuteWithWaitingFormAsync(Func<Task> operation)
        {
            try
            {
                SplashScreenManager.ShowForm(typeof(WaitForm1));
                await operation();
            }
            finally
            {
                SplashScreenManager.CloseForm();
            }
        }

        #endregion

        #region Event Handlers

        private async void ListDataBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            await LoadDataAsync();
        }

        private async void NewBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                using (OverlayManager.ShowScope(this))
                {
                    using (var form = new FrmBusinessPartnerContactDetail(Guid.Empty))
                    {
                        form.StartPosition = FormStartPosition.CenterParent;
                        form.ShowDialog(this);

                        await LoadDataAsync();
                        
                        UpdateButtonStates();
                    }
                }
                
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void BusinessPartnerContactGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                _selectedContactIds = GridViewHelper.GetSelectedRowColumnValues<Guid>(sender, nameof(BusinessPartnerContactDto.Id));
                UpdateButtonStates();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void BusinessPartnerContactGridView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            GridViewHelper.CustomDrawRowIndicator(BusinessPartnerContactGridView, e);
        }

        private void UpdateButtonStates()
        {
            try
            {
                var selectedCount = _selectedContactIds?.Count ?? 0;
                if (EditBarButtonItem != null)
                    EditBarButtonItem.Enabled = selectedCount == 1;
                if (DeleteBarButtonItem != null)
                    DeleteBarButtonItem.Enabled = selectedCount >= 1;
                var rowCount = GridViewHelper.GetDisplayRowCount(BusinessPartnerContactGridView) ?? 0;
                if (ExportBarButtonItem != null)
                    ExportBarButtonItem.Enabled = rowCount > 0;
            }
            catch
            {
                // ignore
            }
        }

        private void ConfigureMultiLineGridView()
        {
            try
            {
                BusinessPartnerContactGridView.OptionsView.RowAutoHeight = true;

                var memo = new RepositoryItemMemoEdit
                {
                    WordWrap = true,
                    AutoHeight = false
                };
                memo.Appearance.TextOptions.WordWrap = WordWrap.Wrap;

                ApplyMemoEditorToColumn("FullName", memo);
                ApplyMemoEditorToColumn("Email", memo);
                ApplyMemoEditorToColumn("Position", memo);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void ApplyMemoEditorToColumn(string fieldName, RepositoryItemMemoEdit memo)
        {
            var col = BusinessPartnerContactGridView.Columns[fieldName];
            if (col == null) return;
            if (!BusinessPartnerContactGridControl.RepositoryItems.Contains(memo))
            {
                BusinessPartnerContactGridControl.RepositoryItems.Add(memo);
            }
            col.ColumnEdit = memo;
        }

        private void BusinessPartnerContactGridView_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            try
            {
                var view = sender as GridView;
                if (view == null) return;
                if (e.RowHandle < 0) return;
                var row = view.GetRow(e.RowHandle) as BusinessPartnerContactDto;
                if (row == null) return;

                if (view.IsRowSelected(e.RowHandle)) return;

                // Tô màu nhẹ cho liên hệ chính
                if (row.IsPrimary)
                {
                    e.Appearance.BackColor = Color.LightGoldenrodYellow;
                    e.Appearance.Options.UseBackColor = true;
                }
            }
            catch
            {
                // ignore
            }
        }

        private async void EditBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (_selectedContactIds == null || _selectedContactIds.Count == 0)
            {
                MsgBox.ShowInfo("Vui lòng chọn một dòng để chỉnh sửa.");
                return;
            }
            if (_selectedContactIds.Count > 1)
            {
                MsgBox.ShowInfo("Chỉ cho phép chỉnh sửa 1 dòng. Vui lòng bỏ chọn bớt.");
                return;
            }

            var id = _selectedContactIds[0];
            var dto = BusinessPartnerContactGridView.GetFocusedRow() as BusinessPartnerContactDto;
            if (dto == null || dto.Id != id)
            {
                if (businessPartnerContactDtoBindingSource.DataSource is IEnumerable list)
                {
                    foreach (var item in list)
                    {
                        if (item is BusinessPartnerContactDto x && x.Id == id)
                        {
                            dto = x;
                            break;
                        }
                    }
                }
            }

            if (dto == null)
            {
                MsgBox.ShowInfo("Không thể xác định dòng được chọn để chỉnh sửa.");
                return;
            }

            try
            {
                using (OverlayManager.ShowScope(this))
                {
                    using (var form = new FrmBusinessPartnerContactDetail(dto.Id))
                    {
                        form.StartPosition = FormStartPosition.CenterParent;
                        form.ShowDialog(this);

                        await LoadDataAsync();
                        UpdateButtonStates();
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private async void DeleteBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (_selectedContactIds == null || _selectedContactIds.Count == 0)
            {
                MsgBox.ShowInfo("Vui lòng chọn ít nhất một dòng để xóa.");
                return;
            }

            var confirmMessage = _selectedContactIds.Count == 1
                ? "Bạn có chắc muốn xóa dòng dữ liệu đã chọn?"
                : $"Bạn có chắc muốn xóa {_selectedContactIds.Count} dòng dữ liệu đã chọn?";

            if (!MsgBox.GetConfirmFromYesNoDialog(confirmMessage)) return;

            try
            {
                await ExecuteWithWaitingFormAsync(() =>
                {
                    foreach (var id in _selectedContactIds.ToList())
                    {
                        _contactBll.Delete(id);
                    }

                    return Task.CompletedTask;
                });

                ClearSelectionState();
                ListDataBarButtonItem.PerformClick();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(new Exception("Lỗi xóa dữ liệu: " + ex.Message, ex));
            }
        }

        private void ExportBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            var rowCount = GridViewHelper.GetDisplayRowCount(BusinessPartnerContactGridView) ?? 0;
            if (rowCount <= 0)
            {
                MsgBox.ShowInfo("Không có dữ liệu để xuất.");
                return;
            }

            GridViewHelper.ExportGridControl(BusinessPartnerContactGridView, "BusinessPartnerContacts.xlsx");
        }

        #endregion

        #region Load/Bind helpers

        private async Task LoadDataAsync()
        {
            if (_isLoading) return;
            _isLoading = true;
            try
            {
                await ExecuteWithWaitingFormAsync(async () =>
                {
                    await LoadDataAsyncWithoutSplash();
                });
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(new Exception("Lỗi tải dữ liệu: " + ex.Message, ex));
            }
            finally
            {
                _isLoading = false;
            }
        }

        private async Task LoadDataAsyncWithoutSplash()
        {
            try
            {
                var entities = await _contactBll.GetAllAsync();
                var partnerBll = new BusinessPartnerBll();
                var partners = await partnerBll.GetAllAsync();
                var partnerNameById = partners.ToDictionary(p => p.Id, p => p.PartnerName);
                var dtoList = entities.Select(e => e.ToDto(partnerNameById.TryGetValue(e.PartnerId, out var value) ? value : null)).ToList();

                BindGrid(dtoList);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(new Exception("Lỗi tải dữ liệu: " + ex.Message, ex));
            }
        }

        private void BindGrid(List<BusinessPartnerContactDto> data)
        {
            businessPartnerContactDtoBindingSource.DataSource = data;
            BusinessPartnerContactGridView.BestFitColumns();
            ConfigureMultiLineGridView();
            UpdateButtonStates();
        }

        private void ClearSelectionState()
        {
            _selectedContactIds.Clear();
            BusinessPartnerContactGridView.ClearSelection();
            BusinessPartnerContactGridView.FocusedRowHandle = GridControl.InvalidRowHandle;
            UpdateButtonStates();
        }

        #endregion
    }
}
