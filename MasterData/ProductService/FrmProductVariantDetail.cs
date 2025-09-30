using Bll.Common;
using Bll.MasterData.ProductServiceBll;
using DevExpress.XtraSplashScreen;
using MasterData.ProductService.Converters;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

namespace MasterData.ProductService
{
    public partial class FrmProductVariantDetail : DevExpress.XtraEditors.XtraForm
    {
        private readonly Guid _productVariantId;
        private readonly ProductServiceBll _productServiceBll = new ProductServiceBll();
		private readonly UnitOfMeasureBll _unitOfMeasureBll = new UnitOfMeasureBll();
		private readonly AttributeBll _attributeBll = new AttributeBll();
		private bool _isLoadingDataSources;

        public FrmProductVariantDetail(Guid productVariantId)
        {
            _productVariantId = productVariantId;
            InitializeComponent();
			this.Load += FrmProductVariantDetail_Load;
			AttributeSearchLookUpEdit.EditValueChanged += AttributeSearchLookUpEdit_EditValueChanged;
			AttributeValueGridView.RowUpdated += AttributeValueGridView_RowUpdated;
			AttributeValueGridView.ValidatingEditor += AttributeValueGridView_ValidatingEditor;
        }

		private void AttributeValueGridView_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
		{
			try
			{
				var view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
				if (view == null) return;

				var column = view.FocusedColumn;
				if (column == null) return;

				// Validate required and type for Value (string) based on selected Attribute's DataType
				if (column.FieldName == "Value")
				{
					var str = e.Value?.ToString()?.Trim();
					if (string.IsNullOrEmpty(str))
					{
						e.Valid = false;
						e.ErrorText = "Giá trị thuộc tính không được để trống";
						return;
					}
					if (str.Length > 255)
					{
						e.Valid = false;
						e.ErrorText = "Giá trị thuộc tính không được vượt quá 255 ký tự";
						return;
					}

					// Get current row's attribute name
					var row = view.GetFocusedRow() as MasterData.ProductService.Dto.AttributeValueDto;
					var attributeName = row?.AttributeName?.Trim();
					if (string.IsNullOrWhiteSpace(attributeName))
					{
						e.Valid = false;
						e.ErrorText = "Vui lòng chọn 'Tên thuộc tính' trước khi nhập giá trị";
						return;
					}

					// Resolve attribute data type by name
					string dataType = null;
					if (attributeDtoBindingSource?.DataSource is System.Collections.IEnumerable list)
					{
						foreach (var item in list)
						{
							if (item is MasterData.ProductService.Dto.AttributeDto dto && string.Equals(dto.Name, attributeName, StringComparison.Ordinal))
							{
								dataType = dto.DataType;
								break;
							}
						}
					}

					if (!string.IsNullOrWhiteSpace(dataType))
					{
						if (!ValidateValueAgainstDataType(dataType, str, out var error))
						{
							e.Valid = false;
							e.ErrorText = error ?? $"Giá trị không phù hợp với kiểu '{dataType}'";
							return;
						}
					}
				}
			}
			catch
			{
				// ignore validation exceptions to avoid editor crash
			}
		}

		private bool ValidateValueAgainstDataType(string dataType, string value, out string error)
		{
			error = null;
			var type = dataType?.Trim()?.ToLowerInvariant();
			switch (type)
			{
				case "int":
				case "integer":
					{
						if (!long.TryParse(value, NumberStyles.Integer, CultureInfo.CurrentCulture, out _))
						{
							error = "Giá trị phải là số nguyên";
							return false;
						}
						return true;
					}
				case "number":
				case "numeric":
				case "decimal":
				case "float":
				case "double":
				case "money":
				case "currency":
					{
						if (!decimal.TryParse(value, NumberStyles.Number, CultureInfo.CurrentCulture, out _))
						{
							error = "Giá trị phải là số";
							return false;
						}
						return true;
					}
				case "date":
					{
						if (!DateTime.TryParse(value, CultureInfo.CurrentCulture, DateTimeStyles.None, out _))
						{
							error = "Giá trị phải là ngày hợp lệ";
							return false;
						}
						return true;
					}
				case "datetime":
					{
						if (!DateTime.TryParse(value, CultureInfo.CurrentCulture, DateTimeStyles.AllowWhiteSpaces, out _))
						{
							error = "Giá trị phải là ngày giờ hợp lệ";
							return false;
						}
						return true;
					}
				case "bool":
				case "boolean":
					{
						if (bool.TryParse(value, out _)) return true;
						// Chấp nhận 0/1, có/không
						var v = value.Trim().ToLowerInvariant();
						if (v == "1" || v == "0" || v == "có" || v == "không" || v == "yes" || v == "no") return true;
						error = "Giá trị phải là kiểu đúng/sai (true/false, 1/0, có/không)";
						return false;
					}
				case "string":
				case "text":
				default:
					return true; // đã kiểm tra rỗng và độ dài phía trên
			}
		}

		private void AttributeValueGridView_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
		{
			try
			{
				// Commit edits to the binding source after a row is updated
				attributeValueDtoBindingSource.EndEdit();
			}
			catch
			{
			}
		}

		private void AttributeSearchLookUpEdit_EditValueChanged(object sender, EventArgs e)
		{
			try
			{
				// Repository editor returns Name (string) per designer ValueMember="Name"
				var editor = sender as DevExpress.XtraEditors.SearchLookUpEdit;
				if (editor == null) return;

				var selectedName = editor.EditValue as string;
				if (string.IsNullOrWhiteSpace(selectedName)) return;

				// Resolve selected DTO by Name
				MasterData.ProductService.Dto.AttributeDto selectedDto = null;
				if (attributeDtoBindingSource?.DataSource is System.Collections.IEnumerable list)
				{
					foreach (var item in list)
					{
						if (item is MasterData.ProductService.Dto.AttributeDto dto && string.Equals(dto.Name, selectedName, StringComparison.Ordinal))
						{
							selectedDto = dto;
							break;
						}
					}
				}

				var focused = AttributeValueGridView.GetFocusedRow() as MasterData.ProductService.Dto.AttributeValueDto;
				if (focused != null)
				{
					focused.AttributeName = selectedName;
					focused.AttributeId = selectedDto?.Id ?? Guid.Empty;
					// Do not touch Value here to avoid type conversion issues
					AttributeValueGridView.SetFocusedRowCellValue("AttributeName", focused.AttributeName);
				}
			}
			catch
			{
			}
		}

        private async void FrmProductVariantDetail_Load(object sender, EventArgs e)
		{
			await LoadDataSourcesAsync();
		}

		private async Task LoadDataSourcesAsync()
		{
			if (_isLoadingDataSources) return;
			_isLoadingDataSources = true;
			try
			{
				await ExecuteWithWaitingFormAsync(async () =>
				{
					await LoadProductServicesDataSourceAsync();
					await LoadUnitOfMeasureDataSourceAsync();
					await LoadAttributesDataSourceAsync();
				});
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Lỗi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			finally
			{
				_isLoadingDataSources = false;
			}
		}

		private async Task LoadProductServicesDataSourceAsync()
		{
			var entities = await _productServiceBll.GetFilteredAsync(
				isActive: true,
				orderBy: "Name",
				orderDirection: "ASC");

			var dtos = entities.ToDtoList(categoryId => _productServiceBll.GetCategoryName(categoryId));
			productServiceDtoBindingSource.DataSource = dtos;
		}

		private async Task LoadUnitOfMeasureDataSourceAsync()
		{
			var units = await Task.Run(() => _unitOfMeasureBll.GetByStatus(true));
			var unitDtos = units.ToDtoList()
				.OrderBy(u => u.Name)
				.ToList();
			unitOfMeasureDtoBindingSource.DataSource = unitDtos;
		}

		private async Task LoadAttributesDataSourceAsync()
		{
			var attributes = await Task.Run(() => _attributeBll.GetAll());
			var attributeDtos = attributes.ToDtoList()
				.OrderBy(a => a.Name)
				.ToList();
			attributeDtoBindingSource.DataSource = attributeDtos;
		}

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

    }
}