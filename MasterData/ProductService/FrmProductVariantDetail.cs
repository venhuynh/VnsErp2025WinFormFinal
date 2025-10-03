using Bll.Common;
using Bll.MasterData.ProductServiceBll;
using Bll.Utils;
using Dal.DataContext;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSplashScreen;
using MasterData.ProductService.Converters;
using MasterData.ProductService.Dto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MasterData.ProductService
{
    /// <summary>
    /// Form chi tiết biến thể sản phẩm - thêm mới và chỉnh sửa.
    /// Cung cấp chức năng CRUD đầy đủ với validation nghiệp vụ và giao diện thân thiện.
    /// </summary>
    public partial class FrmProductVariantDetail : XtraForm
    {
        #region ========== KHAI BÁO BIẾN ==========

        /// <summary>
        /// ID của biến thể sản phẩm đang chỉnh sửa (Guid.Empty nếu thêm mới)
        /// </summary>
        private readonly Guid _productVariantId;

        /// <summary>
        /// Business Logic Layer cho sản phẩm/dịch vụ
        /// </summary>
        private readonly ProductServiceBll _productServiceBll = new ProductServiceBll();

        /// <summary>
        /// Business Logic Layer cho đơn vị tính
        /// </summary>
        private readonly UnitOfMeasureBll _unitOfMeasureBll = new UnitOfMeasureBll();

        /// <summary>
        /// Business Logic Layer cho thuộc tính
        /// </summary>
        private readonly AttributeBll _attributeBll = new AttributeBll();

        /// <summary>
        /// Business Logic Layer cho biến thể sản phẩm
        /// </summary>
        private readonly ProductVariantBll _productVariantBll = new ProductVariantBll();

        /// <summary>
        /// Trạng thái đang tải dữ liệu nguồn
        /// </summary>
        private bool _isLoadingDataSources;

        /// <summary>
        /// Cache danh sách giá trị thuộc tính để quản lý thêm/xóa dòng
        /// </summary>
        private readonly List<AttributeValueDto> _attributeValuesCache = new List<AttributeValueDto>();

        #endregion

        #region ========== CONSTRUCTOR & PUBLIC METHODS ==========

        /// <summary>
        /// Khởi tạo form cho thêm mới hoặc chỉnh sửa biến thể sản phẩm.
        /// </summary>
        /// <param name="productVariantId">ID của biến thể sản phẩm cần chỉnh sửa (Guid.Empty nếu thêm mới)</param>
        public FrmProductVariantDetail(Guid productVariantId)
        {
            _productVariantId = productVariantId;
            InitializeComponent();
			
			// Thiết lập tiêu đề form dựa trên chế độ (tạo mới hoặc edit)
			if (_productVariantId == Guid.Empty)
			{
				Text = @"Thêm mới biến thể sản phẩm";
			}
			else
			{
				Text = @"Chỉnh sửa biến thể sản phẩm";
			}

			Load += FrmProductVariantDetail_Load;
			ProductNameSearchLookupEdit.EditValueChanged += ProductNameSearchLookupEdit_EditValueChanged;
			ProductNameSearchLookupEdit.Popup += ProductNameSearchLookupEdit_Popup;
			UnitNameSearchLookupEdit.EditValueChanged += UnitNameSearchLookupEdit_EditValueChanged;
			UnitNameSearchLookupEdit.Popup += UnitNameSearchLookupEdit_Popup;
			AttributeSearchLookUpEdit.EditValueChanged += AttributeSearchLookUpEdit_EditValueChanged;
			AttributeValueGridView.RowUpdated += AttributeValueGridView_RowUpdated;
			AttributeValueGridView.ValidatingEditor += AttributeValueGridView_ValidatingEditor;
			AttributeValueGridView.InitNewRow += AttributeValueGridView_InitNewRow;
			
			// Đăng ký event handler cho EmbeddedNavigator
			AttributeValueGridControl.EmbeddedNavigator.ButtonClick += EmbeddedNavigator_ButtonClick;
			
			SaveBarButtonItem.ItemClick += SaveBarButtonItem_ItemClick;
			CloseBarButtonItem.ItemClick += CloseBarButtonItem_ItemClick;
        }

        [Localizable(false)]
        public sealed override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        #endregion

        #region ========== KHỞI TẠO FORM ==========

        /// <summary>
        /// Form load event.
        /// </summary>
        private async void FrmProductVariantDetail_Load(object sender, EventArgs e)
		{
			await LoadDataSourcesAsync();
		}

        #endregion

        #region ========== QUẢN LÝ DỮ LIỆU ==========

		/// <summary>
		/// Load tất cả các nguồn dữ liệu cần thiết cho form
		/// </summary>
		private async Task LoadDataSourcesAsync()
		{
			if (_isLoadingDataSources) return;
			_isLoadingDataSources = true;
			try
			{
				await ExecuteWithWaitingFormAsync(async () =>
				{
					if (_productVariantId == Guid.Empty)
					{
						// Chế độ thêm mới: Load toàn bộ datasource
						await LoadDataSourcesForNewModeAsync();
					}
					else
					{
						// Chế độ edit: Chỉ load dữ liệu cần thiết
						await LoadDataSourcesForEditModeAsync();
					}
				});
			}
			catch (Exception ex)
			{
				MsgBox.ShowError($"Lỗi tải dữ liệu: {ex.Message}");
			}
			finally
			{
				_isLoadingDataSources = false;
			}
		}

		/// <summary>
		/// Load datasource cho chế độ thêm mới (cần load toàn bộ danh sách)
		/// </summary>
		private async Task LoadDataSourcesForNewModeAsync()
		{
			await LoadProductServicesDataSourceAsync();
			await LoadUnitOfMeasureDataSourceAsync();
			await LoadAttributesDataSourceAsync();
			await LoadAttributeValuesAsync();
		}

		/// <summary>
		/// Load dữ liệu cho chế độ edit (chỉ load dữ liệu cần thiết)
		/// </summary>
		private async Task LoadDataSourcesForEditModeAsync()
		{
			// Load danh sách thuộc tính trước (cần cho grid) - ưu tiên cao nhất
			await LoadAttributesDataSourceAsync();
			
			// Load dữ liệu biến thể hiện có
			await LoadExistingVariantDataAsync();
			
			// Load giá trị thuộc tính của biến thể (sau khi đã load biến thể)
			await LoadAttributeValuesAsync();
			
			// ProductNameSearchLookupEdit và UnitNameSearchLookupEdit sẽ được load khi popup
		}

		/// <summary>
		/// Load danh sách sản phẩm/dịch vụ vào binding source
		/// </summary>
		private async Task LoadProductServicesDataSourceAsync()
		{
			var entities = await _productServiceBll.GetFilteredAsync(
				isActive: true,
				orderBy: "Name",
				orderDirection: "ASC");

			var dtos = entities.ToDtoList(categoryId => _productServiceBll.GetCategoryName(categoryId));
			productServiceDtoBindingSource.DataSource = dtos;
		}

		/// <summary>
		/// Load danh sách đơn vị tính vào binding source
		/// </summary>
		private async Task LoadUnitOfMeasureDataSourceAsync()
		{
			var units = await Task.Run(() => _unitOfMeasureBll.GetByStatus(true));
			var unitDtos = units.ToDtoList()
				.OrderBy(u => u.Name)
				.ToList();
			unitOfMeasureDtoBindingSource.DataSource = unitDtos;
		}

		/// <summary>
		/// Load danh sách thuộc tính vào binding source
		/// </summary>
		private async Task LoadAttributesDataSourceAsync()
		{
			var attributes = await Task.Run(() => _attributeBll.GetAll());
			var attributeDtos = attributes.ToDtoList()
				.OrderBy(a => a.Name)
				.ToList();
			attributeDtoBindingSource.DataSource = attributeDtos;
		}

		/// <summary>
		/// Load giá trị thuộc tính hiện có (nếu đang edit)
		/// </summary>
		private Task LoadAttributeValuesAsync()
		{
			try
			{
				// Initialize cache
				_attributeValuesCache.Clear();

				// If editing existing variant, load its attribute values
				if (_productVariantId != Guid.Empty)
				{
					var existingValues = _productVariantBll.GetAttributeValues(_productVariantId);
					
					// Convert từ BLL result sang DTO
					foreach (var (attributeId, attributeName, value) in existingValues)
					{
						var dto = new AttributeValueDto
						{
							Id = Guid.NewGuid(), // Generate new ID for DTO
							AttributeId = attributeId,
							AttributeName = attributeName,
							Value = value
						};
						_attributeValuesCache.Add(dto);
					}
				}

				// Bind to grid
				attributeValueDtoBindingSource.DataSource = new List<AttributeValueDto>(_attributeValuesCache);
				attributeValueDtoBindingSource.ResetBindings(false);
			}
			catch (Exception ex)
			{
				MsgBox.ShowError($"Lỗi tải giá trị thuộc tính: {ex.Message}");
			}
			
			return Task.CompletedTask;
		}

		/// <summary>
		/// Load dữ liệu biến thể hiện có khi edit
		/// </summary>
		private async Task LoadExistingVariantDataAsync()
        {
            try
            {
				if (_productVariantId == Guid.Empty) return;

				var variant = _productVariantBll.GetById(_productVariantId);
				if (variant == null) return;

				// Load dữ liệu sản phẩm cụ thể thay vì toàn bộ datasource
				await LoadSpecificProductDataAsync(variant.ProductId);
				
				// Load dữ liệu đơn vị tính cụ thể thay vì toàn bộ datasource
				await LoadSpecificUnitDataAsync(variant.UnitId);

				// Load dữ liệu vào form
				ProductNameSearchLookupEdit.EditValue = variant.ProductId;
				VariantCodeTextEdit.Text = variant.VariantCode;
				UnitNameSearchLookupEdit.EditValue = variant.UnitId;
				IsActiveToggleSwitch.IsOn = variant.IsActive;

				// Disable các control quan trọng khi edit để tránh thay đổi dữ liệu cốt lõi
				SetEditModeControls();
            }
            catch (Exception ex)
            {
				MsgBox.ShowError($"Lỗi tải dữ liệu biến thể: {ex.Message}");
			}
		}

		/// <summary>
		/// Load dữ liệu sản phẩm cụ thể cho chế độ edit
		/// </summary>
		private Task LoadSpecificProductDataAsync(Guid productId)
		{
			try
			{
				// Lấy thông tin sản phẩm cụ thể
				var product = _productServiceBll.GetById(productId);
				if (product != null)
				{
					// Tạo DTO từ entity
					var productDto = new ProductServiceDto
					{
						Id = product.Id,
						Code = product.Code,
						Name = product.Name,
						CategoryId = product.CategoryId,
						CategoryName = _productServiceBll.GetCategoryName(product.CategoryId),
						IsActive = product.IsActive,
					};

					// Bind vào datasource (chỉ có 1 item cho edit mode)
					productServiceDtoBindingSource.DataSource = new List<ProductServiceDto> { productDto };
				}
			}
			catch (Exception ex)
			{
				MsgBox.ShowError($"Lỗi tải dữ liệu sản phẩm: {ex.Message}");
			}
			
			return Task.CompletedTask;
		}

		/// <summary>
		/// Load dữ liệu đơn vị tính cụ thể cho chế độ edit
		/// </summary>
		private Task LoadSpecificUnitDataAsync(Guid unitId)
		{
			try
			{
				// Lấy thông tin đơn vị tính cụ thể
				var unit = _unitOfMeasureBll.GetById(unitId);
				if (unit != null)
				{
					// Tạo DTO từ entity
					var unitDto = new UnitOfMeasureDto
					{
						Id = unit.Id,
						Code = unit.Code,
						Name = unit.Name,
						IsActive = unit.IsActive,
					};

					// Bind vào datasource (chỉ có 1 item cho edit mode)
					unitOfMeasureDtoBindingSource.DataSource = new List<UnitOfMeasureDto> { unitDto };
				}
			}
			catch (Exception ex)
			{
				MsgBox.ShowError($"Lỗi tải dữ liệu đơn vị tính: {ex.Message}");
			}
			
			return Task.CompletedTask;
		}

        #endregion

        #region ========== VALIDATION ==========

		/// <summary>
		/// Validate giá trị thuộc tính theo kiểu dữ liệu
		/// </summary>
		/// <param name="dataType">Kiểu dữ liệu</param>
		/// <param name="value">Giá trị cần validate</param>
		/// <param name="error">Thông báo lỗi nếu có</param>
		/// <returns>True nếu hợp lệ</returns>
		private bool ValidateValueAgainstDataType(string dataType, string value, out string error)
		{
			error = null;
			var type = dataType?.Trim().ToLowerInvariant();
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
                default:
					return true; // đã kiểm tra rỗng và độ dài phía trên
			}
		}

		/// <summary>
		/// Validate dữ liệu form trước khi lưu
		/// </summary>
		/// <returns>True nếu hợp lệ</returns>
		private bool ValidateFormData()
		{
			try
			{
				// Validate các field bắt buộc
				if (string.IsNullOrWhiteSpace(ProductNameSearchLookupEdit.Text))
				{
					MsgBox.ShowWarning("Vui lòng chọn sản phẩm/dịch vụ.");
					ProductNameSearchLookupEdit.Focus();
					return false;
				}

				if (string.IsNullOrWhiteSpace(VariantCodeTextEdit.Text))
				{
					MsgBox.ShowWarning("Vui lòng nhập mã biến thể.");
					VariantCodeTextEdit.Focus();
					return false;
				}

				// Validate attribute values
				var attributeValues = GetChangedAttributeValues();
				foreach (var attrValue in attributeValues)
				{
					if (attrValue.AttributeId == Guid.Empty)
					{
						MsgBox.ShowWarning("Vui lòng chọn đầy đủ thuộc tính cho tất cả dòng.");
						return false;
					}

					if (string.IsNullOrWhiteSpace(attrValue.Value))
					{
						MsgBox.ShowWarning("Vui lòng nhập đầy đủ giá trị cho tất cả thuộc tính.");
						return false;
					}
				}

				return true;
			}
			catch
			{
				return false;
			}
		}

        #endregion

        #region ========== SỰ KIỆN GRID ==========

		/// <summary>
		/// Xử lý khi click nút trên EmbeddedNavigator
		/// </summary>
		private void EmbeddedNavigator_ButtonClick(object sender, NavigatorButtonClickEventArgs e)
		{
			try
			{
				// Xử lý nút xóa
				if (e.Button.ButtonType == NavigatorButtonType.Remove)
				{
					// Kiểm tra nếu đang ở chế độ edit và chỉ còn 1 dòng thuộc tính
					if (_productVariantId != Guid.Empty && _attributeValuesCache.Count == 1)
					{
						// Xác nhận xóa toàn bộ biến thể sản phẩm
						var variantCode = VariantCodeTextEdit.Text?.Trim();
						var message = string.IsNullOrEmpty(variantCode) 
							? "Đây là thuộc tính cuối cùng của biến thể sản phẩm. Bạn có muốn xóa toàn bộ biến thể sản phẩm này không?"
							: $"Đây là thuộc tính cuối cùng của biến thể sản phẩm '{variantCode}'. Bạn có muốn xóa toàn bộ biến thể sản phẩm này không?";
						
						if (MsgBox.GetConfirmFromYesNoDialog(message, "Xác nhận xóa biến thể sản phẩm"))
						{
							// Xóa toàn bộ biến thể sản phẩm
							DeleteProductVariant();
							e.Handled = true;
							return;
						}
						else
						{
							// User không muốn xóa, hủy thao tác
							e.Handled = true;
							return;
						}
					}
					
					RemoveCurrentAttributeValueRow();
					e.Handled = true; // Đánh dấu đã xử lý để tránh xử lý mặc định
				}
				// Xử lý nút thêm mới
				else if (e.Button.ButtonType == NavigatorButtonType.Append)
				{
					AddNewAttributeValueRow();
					e.Handled = true; // Đánh dấu đã xử lý để tránh xử lý mặc định
				}
			}
			catch (Exception ex)
			{
				MsgBox.ShowError($"Lỗi xử lý EmbeddedNavigator: {ex.Message}");
			}
		}

		/// <summary>
		/// Xử lý khi validate editor trong grid
		/// </summary>
		private void AttributeValueGridView_ValidatingEditor(object sender, BaseContainerValidateEditorEventArgs e)
		{
			try
			{
				var view = sender as GridView;
				if (view == null) return;

				var column = view.FocusedColumn;
				if (column == null) return;

				// Validate required and type for Value (string) based on selected Attribute's DataType
				if (column.FieldName == "Value")
				{
					var str = e.Value?.ToString().Trim();
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
					var row = view.GetFocusedRow() as AttributeValueDto;
					var attributeName = row?.AttributeName?.Trim();
					if (string.IsNullOrWhiteSpace(attributeName))
					{
						e.Valid = false;
						e.ErrorText = "Vui lòng chọn 'Tên thuộc tính' trước khi nhập giá trị";
						return;
					}

					// Resolve attribute data type by name
					string dataType = null;
					if (attributeDtoBindingSource?.DataSource is IEnumerable list)
					{
						foreach (var item in list)
						{
							if (item is AttributeDto dto && string.Equals(dto.Name, attributeName, StringComparison.Ordinal))
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
                        }
					}
				}
			}
			catch
			{
				// ignore validation exceptions to avoid editor crash
			}
		}

		/// <summary>
		/// Xử lý khi cập nhật dòng trong grid
		/// </summary>
		private void AttributeValueGridView_RowUpdated(object sender, RowObjectEventArgs e)
		{
			try
			{
				// Commit edits to the binding source after a row is updated
				attributeValueDtoBindingSource.EndEdit();
				// Refresh cache to keep it in sync
				RefreshAttributeValuesCache();
			}
            catch
            {
                // ignored
            }
        }

		/// <summary>
		/// Xử lý khi thay đổi thuộc tính trong grid
		/// </summary>
		private void AttributeSearchLookUpEdit_EditValueChanged(object sender, EventArgs e)
		{
			try
			{
				// Repository editor returns Name (string) per designer ValueMember="Name"
				var editor = sender as SearchLookUpEdit;
				if (editor == null) return;

				var selectedName = editor.EditValue as string;
				if (string.IsNullOrWhiteSpace(selectedName)) return;

				// Resolve selected DTO by Name
				AttributeDto selectedDto = null;
				if (attributeDtoBindingSource?.DataSource is IEnumerable list)
				{
					foreach (var item in list)
					{
						if (item is AttributeDto dto && string.Equals(dto.Name, selectedName, StringComparison.Ordinal))
						{
							selectedDto = dto;
							break;
						}
					}
				}

				var focused = AttributeValueGridView.GetFocusedRow() as AttributeValueDto;
				if (focused != null && selectedDto != null)
				{
					// Kiểm tra xem thuộc tính đã được sử dụng chưa (trừ dòng hiện tại)
					if (IsAttributeAlreadyUsed(selectedDto.Id, focused))
					{
						MsgBox.ShowWarning($"Thuộc tính '{selectedName}' đã được sử dụng. Vui lòng chọn thuộc tính khác.");
						
						// Reset selection
						editor.EditValue = focused.AttributeName;
						return;
					}

					focused.AttributeName = selectedName;
					focused.AttributeId = selectedDto.Id;
					// Do not touch Value here to avoid type conversion issues
					AttributeValueGridView.SetFocusedRowCellValue("AttributeName", focused.AttributeName);
				}
			}
            catch
            {
                // ignored
            }
        }

		/// <summary>
		/// Xử lý khi khởi tạo dòng mới trong grid
		/// </summary>
		private void AttributeValueGridView_InitNewRow(object sender, InitNewRowEventArgs e)
		{
			try
			{
				// Kiểm tra xem còn thuộc tính nào chưa được sử dụng không
				var availableAttributes = GetAvailableAttributes();
				if (availableAttributes.Count == 0)
				{
					MsgBox.ShowInfo("Tất cả thuộc tính đã được sử dụng. Không thể thêm dòng mới.");
					return;
				}

				// Tạo dòng mới và thêm vào cache
				var newRow = new AttributeValueDto
				{
					Id = Guid.NewGuid(),
					AttributeId = Guid.Empty,
					AttributeName = string.Empty,
					Value = string.Empty
				};

				// Thêm dòng mới vào vị trí hiện tại thay vì cuối danh sách
				var currentRowHandle = AttributeValueGridView.FocusedRowHandle;
				if (currentRowHandle >= 0 && currentRowHandle < _attributeValuesCache.Count)
				{
					// Chèn vào vị trí hiện tại + 1
					_attributeValuesCache.Insert(currentRowHandle + 1, newRow);
				}
				else
				{
					// Nếu không có vị trí hợp lệ, thêm vào đầu danh sách
					_attributeValuesCache.Insert(0, newRow);
				}

				// Cập nhật binding source
				attributeValueDtoBindingSource.DataSource = new List<AttributeValueDto>(_attributeValuesCache);
				attributeValueDtoBindingSource.ResetBindings(false);

				// Focus vào dòng mới và cột đầu tiên
				var newRowHandle = currentRowHandle >= 0 ? currentRowHandle + 1 : 0;
				AttributeValueGridView.FocusedRowHandle = newRowHandle;
				AttributeValueGridView.FocusedColumn = AttributeValueGridView.Columns["AttributeName"];
				
				// Đảm bảo dòng mới hiển thị trong viewport
				AttributeValueGridView.MakeRowVisible(newRowHandle);
			}
			catch (Exception ex)
			{
				MsgBox.ShowError($"Lỗi khởi tạo dòng mới: {ex.Message}");
			}
		}

        #endregion

        #region ========== QUẢN LÝ THUỘC TÍNH ==========

		/// <summary>
		/// Thêm dòng mới vào cache và grid
		/// </summary>
		private void AddNewAttributeValueRow()
		{
			try
			{
				// Kiểm tra xem còn thuộc tính nào chưa được sử dụng không
				var availableAttributes = GetAvailableAttributes();
				if (availableAttributes.Count == 0)
				{
					MsgBox.ShowInfo("Tất cả thuộc tính đã được sử dụng. Không thể thêm dòng mới.");
					return;
				}

				var newRow = new AttributeValueDto
				{
					Id = Guid.NewGuid(),
					AttributeId = Guid.Empty,
					AttributeName = string.Empty,
					Value = string.Empty
				};

				// Thêm dòng mới vào vị trí hiện tại thay vì cuối danh sách
				var currentRowHandle = AttributeValueGridView.FocusedRowHandle;
				if (currentRowHandle >= 0 && currentRowHandle < _attributeValuesCache.Count)
				{
					// Chèn vào vị trí hiện tại + 1
					_attributeValuesCache.Insert(currentRowHandle + 1, newRow);
				}
				else
				{
					// Nếu không có vị trí hợp lệ, thêm vào đầu danh sách
					_attributeValuesCache.Insert(0, newRow);
				}

				// Cập nhật binding source
				attributeValueDtoBindingSource.DataSource = new List<AttributeValueDto>(_attributeValuesCache);
				attributeValueDtoBindingSource.ResetBindings(false);

				// Focus vào dòng mới và cột đầu tiên
				var newRowHandle = currentRowHandle >= 0 ? currentRowHandle + 1 : 0;
				AttributeValueGridView.FocusedRowHandle = newRowHandle;
				AttributeValueGridView.FocusedColumn = AttributeValueGridView.Columns["AttributeName"];
				
				// Đảm bảo dòng mới hiển thị trong viewport
				AttributeValueGridView.MakeRowVisible(newRowHandle);
			}
			catch (Exception ex)
			{
				MsgBox.ShowError($"Lỗi thêm dòng mới: {ex.Message}");
			}
		}

		/// <summary>
		/// Xóa dòng hiện tại khỏi cache và grid
		/// </summary>
		private void RemoveCurrentAttributeValueRow()
		{
			try
			{
				var focusedRow = AttributeValueGridView.GetFocusedRow() as AttributeValueDto;
				if (focusedRow == null) return;

				// Confirm deletion
				if (!MsgBox.GetConfirmFromYesNoDialog("Bạn có chắc chắn muốn xóa dòng này?", "Xác nhận xóa")) return;

				// Lưu vị trí hiện tại để điều chỉnh focus sau khi xóa
				var currentRowHandle = AttributeValueGridView.FocusedRowHandle;
				
				// Remove from cache
				_attributeValuesCache.Remove(focusedRow);
				
				// Cập nhật binding source với cache mới
				attributeValueDtoBindingSource.DataSource = new List<AttributeValueDto>(_attributeValuesCache);
				attributeValueDtoBindingSource.ResetBindings(false);
				
				// Đảm bảo grid được refresh
				AttributeValueGridView.RefreshData();

				// Kiểm tra nếu không còn thuộc tính nào và đang ở chế độ edit
				if (_attributeValuesCache.Count == 0 && _productVariantId != Guid.Empty)
				{
					// Xác nhận xóa toàn bộ biến thể sản phẩm
					var variantCode = VariantCodeTextEdit.Text?.Trim();
					var message = string.IsNullOrEmpty(variantCode) 
						? "Biến thể sản phẩm này không còn thuộc tính nào. Bạn có muốn xóa toàn bộ biến thể sản phẩm này không?"
						: $"Biến thể sản phẩm '{variantCode}' không còn thuộc tính nào. Bạn có muốn xóa toàn bộ biến thể sản phẩm này không?";
					
					if (MsgBox.GetConfirmFromYesNoDialog(message, "Xác nhận xóa biến thể sản phẩm"))
					{
						// Xóa toàn bộ biến thể sản phẩm
						DeleteProductVariant();
						return;
					}
				}

				// Điều chỉnh focus thông minh
				if (_attributeValuesCache.Count > 0)
				{
					// Nếu xóa dòng cuối cùng, focus vào dòng trước đó
					if (currentRowHandle >= _attributeValuesCache.Count)
					{
						AttributeValueGridView.FocusedRowHandle = _attributeValuesCache.Count - 1;
					}
					// Nếu xóa dòng ở giữa, focus vào dòng cùng vị trí
					else
					{
						AttributeValueGridView.FocusedRowHandle = currentRowHandle;
					}
					
					// Focus vào cột AttributeName để dễ dàng chỉnh sửa
					AttributeValueGridView.FocusedColumn = AttributeValueGridView.Columns["AttributeName"];
					
					// Đảm bảo dòng được focus hiển thị trong viewport
					AttributeValueGridView.MakeRowVisible(AttributeValueGridView.FocusedRowHandle);
				}
			}
			catch (Exception ex)
			{
				MsgBox.ShowError($"Lỗi xóa dòng: {ex.Message}");
			}
		}

		/// <summary>
		/// Làm mới cache từ binding source
		/// </summary>
		private void RefreshAttributeValuesCache()
        {
            try
            {
				attributeValueDtoBindingSource.EndEdit();
				_attributeValuesCache.Clear();

				if (attributeValueDtoBindingSource.DataSource is IEnumerable<AttributeValueDto> data)
				{
					_attributeValuesCache.AddRange(data);
				}
            }
            catch (Exception ex)
            {
				MsgBox.ShowError($"Lỗi làm mới cache: {ex.Message}");
			}
		}

		/// <summary>
		/// Lấy danh sách các giá trị thuộc tính đã thay đổi
		/// </summary>
		private List<AttributeValueDto> GetChangedAttributeValues()
		{
			try
			{
				attributeValueDtoBindingSource.EndEdit();
				return new List<AttributeValueDto>(_attributeValuesCache);
			}
			catch
			{
				return new List<AttributeValueDto>();
			}
		}

		/// <summary>
		/// Lấy danh sách các thuộc tính chưa được sử dụng
		/// </summary>
		private List<AttributeDto> GetAvailableAttributes()
		{
			try
			{
				var allAttributes = new List<AttributeDto>();
				if (attributeDtoBindingSource?.DataSource is IEnumerable<AttributeDto> data)
				{
					allAttributes.AddRange(data);
				}

				var usedAttributeIds = _attributeValuesCache
					.Where(av => av.AttributeId != Guid.Empty)
					.Select(av => av.AttributeId)
					.ToHashSet();

				return allAttributes
					.Where(attr => !usedAttributeIds.Contains(attr.Id))
					.ToList();
			}
			catch
			{
				return new List<AttributeDto>();
			}
		}

		/// <summary>
		/// Kiểm tra xem thuộc tính đã được sử dụng chưa
		/// </summary>
		private bool IsAttributeAlreadyUsed(Guid attributeId, AttributeValueDto excludeRow = null)
		{
			try
			{
				return _attributeValuesCache
                    .Any(av => !Equals(av, excludeRow) && av.AttributeId == attributeId);
			}
			catch
			{
				return false;
			}
		}

        #endregion

        #region ========== CHỨC NĂNG XÓA DỮ LIỆU ==========

		/// <summary>
		/// Xóa toàn bộ biến thể sản phẩm
		/// </summary>
		private async void DeleteProductVariant()
		{
			try
			{
				await ExecuteWithWaitingFormAsync(async () =>
				{
					// Xóa biến thể sản phẩm
					await _productVariantBll.DeleteAsync(_productVariantId);
					
					MsgBox.ShowInfo("Đã xóa biến thể sản phẩm thành công!");
					
					// Đóng form
					Close();
				});
			}
			catch (Exception ex)
			{
				MsgBox.ShowError($"Lỗi xóa biến thể sản phẩm: {ex.Message}");
			}
		}

        #endregion

        #region ========== CHỨC NĂNG LƯU DỮ LIỆU ==========

		/// <summary>
		/// Xử lý khi click nút Lưu
		/// </summary>
		private async void SaveBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
		{
			try
			{
				await ExecuteWithWaitingFormAsync(async () =>
				{
					// Validate dữ liệu trước khi lưu
					if (!ValidateFormData())
					{
						MsgBox.ShowWarning("Vui lòng kiểm tra lại dữ liệu trước khi lưu.");
						return;
					}

					// Tạo ProductVariant entity từ form data
					var variant = CreateVariantEntityFromForm();
					var attributeValues = GetAttributeValuesForSave();

					// Lưu biến thể
					var savedId = await _productVariantBll.SaveAsync(variant, attributeValues);

					// Cập nhật _productVariantId nếu đang tạo mới
					if (_productVariantId == Guid.Empty)
					{
						// Update the private field using reflection or create a property
						var field = typeof(FrmProductVariantDetail).GetField("_productVariantId", 
							System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
						field?.SetValue(this, savedId);
					}

					MsgBox.ShowInfo("Lưu dữ liệu thành công!");
					
					//Đóng màn hình này
                    Close();

                });
			}
			catch (Exception ex)
			{
				MsgBox.ShowError($"Lỗi lưu dữ liệu: {ex.Message}");
			}
		}

		/// <summary>
		/// Xử lý khi click nút Đóng
		/// </summary>
		private void CloseBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
				// Kiểm tra xem có thay đổi chưa lưu không
				if (HasUnsavedChanges())
				{
					if (!MsgBox.GetConfirmFromYesNoDialog("Có thay đổi chưa được lưu. Bạn có chắc chắn muốn đóng?")) return;
				}

				Close();
            }
            catch (Exception ex)
            {
				MsgBox.ShowError($"Lỗi đóng form: {ex.Message}");
			}
		}

		/// <summary>
		/// Tạo ProductVariant entity từ dữ liệu form
		/// </summary>
		private ProductVariant CreateVariantEntityFromForm()
		{
			try
			{
				// Lấy ProductId từ ProductNameSearchLookupEdit
				var selectedProduct = ProductNameSearchLookupEdit.EditValue;
				Guid productId = Guid.Empty;
				if (selectedProduct != null)
				{
					// Assuming the EditValue contains the ProductId
					productId = (Guid)selectedProduct;
				}

				// Lấy UnitId từ UnitNameSearchLookupEdit
				var selectedUnit = UnitNameSearchLookupEdit.EditValue;
				Guid unitId = Guid.Empty;
				if (selectedUnit != null)
				{
					// Assuming the EditValue contains the UnitId
					unitId = (Guid)selectedUnit;
				}

				// Tính toán VariantFullName từ các thuộc tính đã chọn
				var variantFullName = BuildVariantFullNameFromForm();

				var variant = new ProductVariant
				{
					Id = _productVariantId,
					ProductId = productId,
					VariantCode = VariantCodeTextEdit.Text?.Trim(),
					UnitId = unitId,
					IsActive = IsActiveToggleSwitch.IsOn,
					VariantFullName = variantFullName
				};

				return variant;
			}
			catch (Exception ex)
			{
				throw new Exception($"Lỗi tạo dữ liệu biến thể: {ex.Message}", ex);
			}
		}

		/// <summary>
		/// Lấy danh sách giá trị thuộc tính để lưu (convert từ DTO sang tuple)
		/// </summary>
		private List<(Guid AttributeId, string Value)> GetAttributeValuesForSave()
        {
            try
            {
				var attributeValues = GetChangedAttributeValues();
				return attributeValues
					.Where(av => av.AttributeId != Guid.Empty && !string.IsNullOrWhiteSpace(av.Value))
					.Select(av => (av.AttributeId, av.Value))
					.ToList();
            }
            catch (Exception ex)
            {
				throw new Exception($"Lỗi lấy giá trị thuộc tính: {ex.Message}", ex);
            }
        }

        #endregion

        #region ========== SỰ KIỆN POPUP ==========

		/// <summary>
		/// Xử lý khi popup ProductNameSearchLookupEdit
		/// </summary>
		private async void ProductNameSearchLookupEdit_Popup(object sender, EventArgs e)
		{
			try
			{
				// Chỉ load khi chưa có dữ liệu hoặc đang ở chế độ thêm mới
				if (productServiceDtoBindingSource.DataSource == null || _productVariantId == Guid.Empty)
				{
					await LoadProductServicesDataSourceAsync();
				}
			}
			catch (Exception ex)
			{
				MsgBox.ShowError($"Lỗi tải danh sách sản phẩm: {ex.Message}");
			}
		}

		/// <summary>
		/// Xử lý khi popup UnitNameSearchLookupEdit
		/// </summary>
		private async void UnitNameSearchLookupEdit_Popup(object sender, EventArgs e)
		{
			try
			{
				// Chỉ load khi chưa có dữ liệu hoặc đang ở chế độ thêm mới
				if (unitOfMeasureDtoBindingSource.DataSource == null || _productVariantId == Guid.Empty)
				{
					await LoadUnitOfMeasureDataSourceAsync();
				}
			}
			catch (Exception ex)
			{
				MsgBox.ShowError($"Lỗi tải danh sách đơn vị tính: {ex.Message}");
			}
		}

        #endregion

        #region ========== SỰ KIỆN FORM ==========

		/// <summary>
		/// Xử lý khi thay đổi sản phẩm/dịch vụ
		/// </summary>
		private void ProductNameSearchLookupEdit_EditValueChanged(object sender, EventArgs e)
		{
			try
			{
				// Chỉ tự động sinh mã khi đang thêm mới (không phải edit mode)
				if (_productVariantId != Guid.Empty)
					return;

				GenerateVariantCode();
			}
			catch (Exception ex)
			{
				MsgBox.ShowError($"Lỗi khi thay đổi sản phẩm/dịch vụ: {ex.Message}");
			}
		}

		/// <summary>
		/// Xử lý khi thay đổi đơn vị tính
		/// </summary>
		private void UnitNameSearchLookupEdit_EditValueChanged(object sender, EventArgs e)
		{
			try
			{
				// Chỉ tự động sinh mã khi đang thêm mới (không phải edit mode)
				if (_productVariantId != Guid.Empty)
					return;

				GenerateVariantCode();
			}
			catch (Exception ex)
			{
				MsgBox.ShowError($"Lỗi khi thay đổi đơn vị tính: {ex.Message}");
			}
		}

        #endregion

        #region ========== TÍNH TOÁN VARIANT FULL NAME ==========

		/// <summary>
		/// Xây dựng VariantFullName từ dữ liệu form hiện tại
		/// </summary>
		private string BuildVariantFullNameFromForm()
		{
			try
			{
				var attributeValues = GetChangedAttributeValues();
				if (attributeValues == null || !attributeValues.Any())
				{
					return string.Empty;
				}

				var fullNameParts = new List<string>();
				foreach (var attrValue in attributeValues)
				{
					if (attrValue.AttributeId != Guid.Empty && !string.IsNullOrWhiteSpace(attrValue.Value))
					{
						// Format: AttributeName : Value
						fullNameParts.Add($"{attrValue.AttributeName} : {attrValue.Value}");
					}
				}

				return string.Join(", ", fullNameParts);
			}
			catch (Exception ex)
			{
				// Log error but don't throw to avoid breaking the save process
				System.Diagnostics.Debug.WriteLine($"Lỗi tính toán VariantFullName: {ex.Message}");
				return string.Empty;
			}
		}

        #endregion

        #region ========== TỰ ĐỘNG SINH MÃ ==========

		/// <summary>
		/// Tự động sinh mã biến thể theo format: [Mã sản phẩm] + [Đơn vị tính] + [Số thứ tự]
		/// </summary>
		private void GenerateVariantCode()
        {
            try
            {
				// Lấy thông tin sản phẩm/dịch vụ
				var selectedProductId = ProductNameSearchLookupEdit.EditValue as Guid?;
				if (selectedProductId == null || selectedProductId == Guid.Empty)
				{
					VariantCodeTextEdit.Text = string.Empty;
					return;
				}

				// Lấy thông tin đơn vị tính
				var selectedUnitId = UnitNameSearchLookupEdit.EditValue as Guid?;
				if (selectedUnitId == null || selectedUnitId == Guid.Empty)
				{
					VariantCodeTextEdit.Text = string.Empty;
					return;
				}

				// Lấy thông tin sản phẩm để lấy mã
				var product = _productServiceBll.GetById(selectedProductId.Value);
				if (product == null)
				{
					VariantCodeTextEdit.Text = string.Empty;
					return;
				}

				// Lấy thông tin đơn vị tính để lấy mã
				var unit = _unitOfMeasureBll.GetById(selectedUnitId.Value);
				if (unit == null)
				{
					VariantCodeTextEdit.Text = string.Empty;
					return;
				}

				// Tạo prefix: [Mã sản phẩm] + [Mã đơn vị]
				var prefix = $"{product.Code}_{unit.Code}";

				// Tìm số thứ tự tiếp theo cho biến thể của sản phẩm này
				var nextNumber = GetNextVariantNumber(selectedProductId.Value, prefix);

				// Tạo mã biến thể: prefix + số thứ tự (4 chữ số)
				var variantCode = $"{prefix}_{nextNumber:D4}";

				VariantCodeTextEdit.Text = variantCode;
            }
            catch (Exception ex)
            {
				MsgBox.ShowError($"Lỗi sinh mã biến thể: {ex.Message}");
			}
		}

		/// <summary>
		/// Lấy số thứ tự tiếp theo cho biến thể của sản phẩm
		/// </summary>
		/// <param name="productId">ID sản phẩm</param>
		/// <param name="prefix">Prefix mã biến thể</param>
		/// <returns>Số thứ tự tiếp theo (1-9999)</returns>
		private int GetNextVariantNumber(Guid productId, string prefix)
		{
			try
			{
				// Lấy danh sách biến thể hiện có của sản phẩm
				var existingVariants = _productVariantBll.GetByProductId(productId);
				
				// Tìm các mã biến thể có cùng prefix
				var existingCodes = existingVariants
					.Where(v => v.VariantCode != null && v.VariantCode.StartsWith(prefix + "_"))
					.Select(v => v.VariantCode)
					.ToList();

				if (!existingCodes.Any())
					return 1;

				// Tìm số lớn nhất trong các mã hiện có
				var maxNumber = 0;
				foreach (var code in existingCodes)
				{
					// Lấy phần số cuối (4 chữ số sau dấu _ cuối cùng)
					var lastUnderscoreIndex = code.LastIndexOf('_');
					if (lastUnderscoreIndex >= 0 && code.Length >= lastUnderscoreIndex + 5)
					{
						var numberPart = code.Substring(lastUnderscoreIndex + 1, 4);
						if (int.TryParse(numberPart, out var number))
						{
							maxNumber = Math.Max(maxNumber, number);
						}
					}
				}

				// Trả về số tiếp theo, nhưng không vượt quá 9999
				return Math.Min(maxNumber + 1, 9999);
			}
			catch (Exception ex)
			{
				MsgBox.ShowError($"Lỗi lấy số thứ tự biến thể: {ex.Message}");
				return 1;
			}
		}

        #endregion

        #region ========== TIỆN ÍCH ==========

		/// <summary>
		/// Thực thi operation với splash screen
		/// </summary>
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

		/// <summary>
		/// Kiểm tra xem có thay đổi chưa lưu không
		/// </summary>
		private bool HasUnsavedChanges()
		{
			try
			{
				// TODO: Implement logic to check for unsaved changes
				// For now, always return false
				return false;
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// Thiết lập các control ở chế độ edit (disable các field quan trọng)
		/// </summary>
		private void SetEditModeControls()
		{
			try
			{
				// Disable các control quan trọng khi edit để tránh thay đổi dữ liệu cốt lõi
				ProductNameSearchLookupEdit.Enabled = false;
				VariantCodeTextEdit.Enabled = false;
				UnitNameSearchLookupEdit.Enabled = false;

				// Thay đổi màu nền để người dùng biết các field này không thể chỉnh sửa
				ProductNameSearchLookupEdit.BackColor = System.Drawing.SystemColors.Control;
				VariantCodeTextEdit.BackColor = System.Drawing.SystemColors.Control;
				UnitNameSearchLookupEdit.BackColor = System.Drawing.SystemColors.Control;

				// Cập nhật tiêu đề form để hiển thị chế độ edit với mã biến thể
				var variantCode = VariantCodeTextEdit.Text?.Trim();
				if (!string.IsNullOrEmpty(variantCode))
				{
					Text = $@"Chỉnh sửa biến thể sản phẩm - {variantCode}";
				}
				else
				{
					Text = "Chỉnh sửa biến thể sản phẩm";
				}
			}
			catch (Exception ex)
			{
				MsgBox.ShowError($"Lỗi thiết lập chế độ edit: {ex.Message}");
			}
		}

        #endregion
    }
}