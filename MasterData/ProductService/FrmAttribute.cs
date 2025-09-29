using Bll.MasterData.ProductServiceBll;
using Bll.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraGrid.Views.Grid;
using MasterData.ProductService.Converters;
using MasterData.ProductService.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace MasterData.ProductService
{
	/// <summary>
	/// Form quản lý Thuộc Tính (Attribute) dạng popup.
	/// - Tổ chức vùng code rõ ràng theo mẫu FrmProductServiceDetail
	/// - Chỉ chứa event handler và gọi helper; xử lý chính tách thành phương thức riêng
	/// </summary>
    public partial class FrmAttribute : DevExpress.XtraEditors.XtraForm
    {
		#region Fields
        private readonly BindingList<AttributeDto> _attributes = new BindingList<AttributeDto>();
        private AttributeDto _current;
        private List<Guid> _selectedAttributeIds = new List<Guid>();

        private IList<AttributeDto> ResultAttributes => _attributes.ToList();
        private readonly AttributeBll _AttributeBll = new AttributeBll();
        #endregion

        #region Constructor
        public FrmAttribute()
        {
            InitializeComponent();
            InitializeBehavior();
        }

        public FrmAttribute(IEnumerable<AttributeDto> initialAttributes) : this()
        {
            if (initialAttributes != null)
            {
                foreach (var a in initialAttributes)
                {
                    _attributes.Add(a);
                }
            }
        }

        public static IList<AttributeDto> ShowManage(IWin32Window owner, IEnumerable<AttributeDto> initialAttributes)
        {
            using (var frm = new FrmAttribute(initialAttributes))
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog(owner);
                return frm.ResultAttributes;
            }
        }
		#endregion

		#region Initialization
        private void InitializeBehavior()
        {
            // Binding source to list
            attributeDtoBindingSource.DataSource = _attributes;

            // Grid selection change (checkbox selection)
			AttributeGridView.SelectionChanged += AttributeGridView_SelectionChanged;
			AttributeGridView.CustomDrawRowIndicator += AttributeGridView_CustomDrawRowIndicator;

            // Buttons
            SaveBarButtonItem.ItemClick += SaveBarButtonItem_ItemClick;
            EditBarButtonItem.ItemClick += EditBarButtonItem_ItemClick;
            DeleteBarButtonItem.ItemClick += DeleteBarButtonItem_ItemClick;
            RefreshBarButtonItem.ItemClick += RefreshBarButtonItem_ItemClick;

            // Double click to edit
            AttributeGridView.DoubleClick += (s, e) => BeginEditSelected();

			// Keyboard shortcuts
			KeyPreview = true;
			KeyDown += FrmAttribute_KeyDown;

            // Create default current if empty
            if (_attributes.Count == 0)
            {
                _attributes.Add(new AttributeDto());
            }

            // Checkbox selection: không auto bind editors theo selection

			// Grid configure and button states
			ConfigureGrid();
			UpdateButtonStates();

            // Gọi làm mới (clear selection + clear editors + load data) khi form hiển thị
            RefreshData();
        }

        private void BindEditors()
        {
            NameTextEdit.DataBindings.Clear();
            DataTypeTextEdit.DataBindings.Clear();
            DescriptionTextEdit.DataBindings.Clear();

            if (_current == null) return;

            // Bind trực tiếp vào đối tượng _current để tránh lệ thuộc Position của BindingSource
            NameTextEdit.DataBindings.Add("EditValue", _current, nameof(AttributeDto.Name), true, DataSourceUpdateMode.OnPropertyChanged);
            DataTypeTextEdit.DataBindings.Add("EditValue", _current, nameof(AttributeDto.DataType), true, DataSourceUpdateMode.OnPropertyChanged);
            DescriptionTextEdit.DataBindings.Add("EditValue", _current, nameof(AttributeDto.Description), true, DataSourceUpdateMode.OnPropertyChanged);
        }
		#endregion

		#region Grid Events & Helpers
        private void UpdateCurrentFromGrid()
        {
            // Checkbox mode: không tự cập nhật _current để tránh tự đổ dữ liệu vào editors
        }

        private void AttributeGridView_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            try
            {
                _selectedAttributeIds = GridViewHelper.GetSelectedRowColumnValues<Guid>(sender, nameof(AttributeDto.Id));
                
                UpdateButtonStates();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

		private void AttributeGridView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
		{
			try
			{
				GridViewHelper.CustomDrawRowIndicator(AttributeGridView, e);
			}
			catch
			{
				// ignore
			}
		}

		private void ConfigureGrid()
		{
			try
			{
				AttributeGridView.IndicatorWidth = 50;
				AttributeGridView.OptionsView.ShowAutoFilterRow = true;
				AttributeGridView.OptionsView.ShowGroupPanel = false;
				AttributeGridView.BestFitColumns();
			}
			catch
			{
				// ignore
			}
		}
		#endregion

		#region Toolbar Event Handlers
        private void SaveBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            SaveCurrent();
        }

        private void EditBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            BeginEditSelected();
        }

        private void DeleteBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DeleteSelected();
        }

		private void FrmAttribute_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control && e.KeyCode == Keys.S)
			{
				SaveCurrent();
				e.Handled = true;
			}
			else if (e.KeyCode == Keys.Delete)
			{
				DeleteSelected();
				e.Handled = true;
			}
		}
		#endregion

		#region Actions & Validation
        private void BeginEditSelected()
        {
            if (_selectedAttributeIds == null || _selectedAttributeIds.Count == 0)
            {
                ShowInfo("Vui lòng chọn một dòng để chỉnh sửa.");
                return;
            }
            if (_selectedAttributeIds.Count > 1)
            {
                ShowInfo("Chỉ cho phép chỉnh sửa 1 dòng. Vui lòng bỏ chọn bớt.");
                return;
            }

            var id = _selectedAttributeIds[0];
            AttributeDto dto = null;
            // Ưu tiên lấy từ FocusedRow nếu khớp Id
            if (AttributeGridView.GetFocusedRow() is AttributeDto focused && focused.Id == id)
            {
                dto = focused;
            }
            else
            {
                // Tìm đúng DTO trong datasource theo Id
                if (attributeDtoBindingSource.DataSource is IEnumerable<AttributeDto> list)
                {
                    foreach (var item in list)
                    {
                        if (item.Id == id)
                        {
                            dto = item;
                            break;
                        }
                    }
                }
            }

            if (dto == null)
            {
                ShowInfo("Không thể xác định dòng được chọn để chỉnh sửa.");
                return;
            }

            _current = dto;
            // Đồng bộ BindingSource.Current về đúng phần tử đang edit (để các thao tác khác nếu dùng BindingSource không bị lệch)
            var index = _attributes.IndexOf(_current);
            if (index >= 0) attributeDtoBindingSource.Position = index;
            BindEditors();
            NameTextEdit.Focus();
            NameTextEdit.SelectAll();
        }

        private int? GetSingleSelectedRowHandle()
        {
            try
            {
                AttributeGridView.PostEditor();
                AttributeGridView.UpdateCurrentRow();

                var selected = AttributeGridView.GetSelectedRows();
                if (selected == null) return null;

                var valid = selected
                    .Where(h => h >= 0 && AttributeGridView.IsRowSelected(h))
                    .ToArray();

                if (valid.Length == 1)
                    return valid[0];

                // Nếu nhiều dòng còn selected, ưu tiên dòng đang focus nếu nó đang được chọn
                var focused = AttributeGridView.FocusedRowHandle;
                if (focused >= 0 && AttributeGridView.IsRowSelected(focused))
                    return focused;

                return null;
            }
            catch
            {
                return null;
            }
        }

        private void SaveCurrent()
        {
            ClearValidation();
            if (!ValidateCurrent()) return;

			if (_current == null)
			{
				ShowInfo("Vui lòng chọn 1 dòng và bấm Điều chỉnh trước khi lưu.");
				return;
			}

			attributeDtoBindingSource.EndEdit();
			try
			{
                // Kiểm tra trùng tên (unique)
                if (_AttributeBll.IsNameExists(_current.Name?.Trim(), _current.Id == Guid.Empty ? null : (Guid?)_current.Id))
                {
                    ShowInfo("Tên thuộc tính đã tồn tại. Vui lòng chọn tên khác.");
                    return;
                }

				// Lưu xuống DB qua BLL
				var entity = _current.ToEntity();
				_AttributeBll.SaveOrUpdate(entity);
				// Cập nhật lại Id nếu là bản ghi mới
				_current.Id = entity.Id;
				AttributeGridView.RefreshData();
				ShowInfo("Đã lưu thuộc tính");
			}
			catch (Exception ex)
			{
				ShowError(ex, "Lỗi lưu thuộc tính");
			}
        }

        private bool ValidateCurrent()
        {
            if (_current == null)
                return false;

            var errors = _current.GetValidationErrors();
            if (errors.Count == 0) return true;

            // Map common errors to editors
            foreach (var err in errors)
            {
                if (err.Contains("Tên thuộc tính"))
                    dxErrorProvider1.SetError(NameTextEdit, err);
                else if (err.Contains("Kiểu dữ liệu"))
                    dxErrorProvider1.SetError(DataTypeTextEdit, err);
                else if (err.Contains("Mô tả"))
                    dxErrorProvider1.SetError(DescriptionTextEdit, err);
            }

			ShowInfo(string.Join(Environment.NewLine, errors));
            return false;
        }

        private void ClearValidation()
        {
            dxErrorProvider1.SetError(NameTextEdit, string.Empty);
            dxErrorProvider1.SetError(DataTypeTextEdit, string.Empty);
            dxErrorProvider1.SetError(DescriptionTextEdit, string.Empty);
        }

        private void DeleteSelected()
        {
            var count = _selectedAttributeIds?.Count ?? 0;
            if (count == 0)
            {
                ShowInfo("Vui lòng chọn ít nhất 1 dòng để xóa.");
                return;
            }

			var confirmMsg = count == 1 ? "Bạn có chắc muốn xóa thuộc tính đã chọn?" : $"Bạn có chắc muốn xóa {count} thuộc tính đã chọn?";
			if (!MsgBox.GetConfirmFromYesNoDialog(confirmMsg)) return;

			try
			{
				// Thu thập DTO được chọn theo Id đã lưu
				var toDelete = new List<AttributeDto>();
				if (attributeDtoBindingSource.DataSource is IEnumerable<AttributeDto> list)
				{
					foreach (var item in list)
					{
						if (_selectedAttributeIds.Contains(item.Id))
							toDelete.Add(item);
					}
				}

				foreach (var dto in toDelete)
				{
                    // Chặn xóa khi còn phụ thuộc
                    if (_AttributeBll.HasDependencies(dto.Id))
                    {
                        ShowInfo($"Không thể xóa '{dto.Name}' vì còn dữ liệu phụ thuộc.");
                        continue;
                    }
					_AttributeBll.Delete(dto.Id);
					_attributes.Remove(dto);
				}
			}
			catch (Exception ex)
			{
				ShowError(ex, "Lỗi xóa thuộc tính");
				return;
			}

            if (_attributes.Count == 0)
            {
                _attributes.Add(new AttributeDto());
            }

            ClearSelectionState();
            AttributeGridView.FocusedRowHandle = Math.Max(0, _attributes.Count - 1);
            AttributeGridView.RefreshData();
        }

        // API: Add new attribute programmatically (used by caller screens)
        public AttributeDto AddNew(string name = "", string dataType = "", string description = "")
        {
            var dto = new AttributeDto
            {
                Name = name,
                DataType = dataType,
                Description = description
            };
            _attributes.Add(dto);
            attributeDtoBindingSource.Position = _attributes.IndexOf(dto);
            AttributeGridView.FocusedRowHandle = attributeDtoBindingSource.Position;
            _current = dto;
            BindEditors();
            return dto;
        }

        // API: Set/Replace data from caller
        private void SetData(IEnumerable<AttributeDto> attributes)
        {
			_attributes.Clear();
			if (attributes != null)
			{
				foreach (var a in attributes)
				{
					_attributes.Add(a);
				}
			}
            if (_attributes.Count == 0) _attributes.Add(new AttributeDto());
            attributeDtoBindingSource.ResetBindings(false);
            // Checkbox mode: không auto bind editors theo selection
        }

        private void ClearSelectionState()
        {
            _selectedAttributeIds.Clear();
            AttributeGridView.ClearSelection();
        }

		/// <summary>
		/// Tải dữ liệu từ DB và bind vào lưới.
		/// </summary>
		public void LoadData()
		{
			try
			{
				var entities = _AttributeBll.GetAll();
				var dtos = entities.ToDtoList();
				SetData(dtos);
			}
			catch (Exception ex)
			{
				ShowError(ex, "Lỗi tải danh sách thuộc tính");
			}
		}
		#endregion

        #region Refresh Logic
        private void RefreshBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            RefreshData();
        }

        private void RefreshData()
        {
            try
            {
                // Clear selections
                ClearSelectionState();

                // Clear current and editors
                _current = null;
                ClearValidation();
                NameTextEdit.DataBindings.Clear();
                DataTypeTextEdit.DataBindings.Clear();
                DescriptionTextEdit.DataBindings.Clear();
                NameTextEdit.EditValue = null;
                DataTypeTextEdit.EditValue = null;
                DescriptionTextEdit.EditValue = null;

                // Reload data
                LoadData();
                UpdateButtonStates();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi làm mới dữ liệu");
            }
        }
        #endregion

		#region Button States
		private void UpdateButtonStates()
		{
            try
            {
                var selectedCount = _selectedAttributeIds?.Count ?? 0;
                if (EditBarButtonItem != null) EditBarButtonItem.Enabled = selectedCount == 1;
                if (DeleteBarButtonItem != null) DeleteBarButtonItem.Enabled = selectedCount >= 1;
				if (SaveBarButtonItem != null) SaveBarButtonItem.Enabled = true;
			}
			catch
			{
				// ignore
			}
		}
		#endregion

		#region Utility Methods
		private void ShowInfo(string message)
		{
			MsgBox.ShowInfo(message);
		}

		private void ShowError(Exception ex, string context = null)
		{
			if (string.IsNullOrWhiteSpace(context))
				MsgBox.ShowException(ex);
			else
				MsgBox.ShowException(new Exception(context + ": " + ex.Message, ex));
		}
		#endregion
    }
}
