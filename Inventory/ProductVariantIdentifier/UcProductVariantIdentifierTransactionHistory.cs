using Bll.Inventory.InventoryManagement;
using Common.Utils;
using DevExpress.XtraEditors;
using DTO.Inventory;
using DTO.Inventory.InventoryManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Inventory.ProductVariantIdentifier
{
    public partial class UcProductVariantIdentifierTransactionHistory : XtraUserControl
    {
        #region Fields

        /// <summary>
        /// Business Logic Layer cho ProductVariantIdentifierHistory
        /// </summary>
        private readonly ProductVariantIdentifierHistoryBll _productVariantIdentifierHistoryBll = new ProductVariantIdentifierHistoryBll();

        /// <summary>
        /// Business Logic Layer cho StockInOutDetail
        /// </summary>
        private readonly StockInOutDetailBll _stockInOutDetailBll = new StockInOutDetailBll();

        /// <summary>
        /// Business Logic Layer cho StockInOutMaster
        /// </summary>
        private readonly StockInOutMasterBll _stockInOutMasterBll = new StockInOutMasterBll();

        /// <summary>
        /// ProductVariantIdentifierDto hiện tại
        /// </summary>
        private ProductVariantIdentifierDto _currentDto;

        #endregion

        public UcProductVariantIdentifierTransactionHistory()
        {
            InitializeComponent();
            InitializeChangeTypeComboBox();
        }

        /// <summary>
        /// Khởi tạo ComboBox cho loại thay đổi
        /// </summary>
        private void InitializeChangeTypeComboBox()
        {
            try
            {
                LoadProductVariantIdentifierHistoryChangeTypeEnumComboBox();
                
                // Đăng ký event handler khi giá trị thay đổi
                ProductVariantIdentifierHistoryChangeTypeComboBoxEdit.EditValueChanged += ProductVariantIdentifierHistoryChangeTypeComboBoxEdit_EditValueChanged;
                
                // Khởi tạo trạng thái ban đầu (ẩn cả hai)
                UpdateLayoutVisibility(null);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"InitializeChangeTypeComboBox: Exception occurred - {ex.Message}");
            }
        }

        /// <summary>
        /// Load danh sách ProductVariantIdentifierHistoryChangeTypeEnum vào ComboBoxEdit
        /// </summary>
        private void LoadProductVariantIdentifierHistoryChangeTypeEnumComboBox()
        {
            try
            {
                ProductVariantIdentifierHistoryChangeTypeComboBoxEdit.Properties.Items.Clear();

                // Load tất cả các giá trị enum với màu sắc HTML
                foreach (ProductVariantIdentifierHistoryChangeTypeEnum value in Enum.GetValues(typeof(ProductVariantIdentifierHistoryChangeTypeEnum)))
                {
                    int index = (int)value; // Sử dụng giá trị enum làm index (1, 2, 3, 4, 5, 99)
                    string description = ApplicationEnumUtils.GetDescription(value);
                    string colorHex = GetChangeTypeColor(value);

                    // Tạo HTML với màu sắc theo chuẩn DevExpress
                    string itemName = $"<color='{colorHex}'>{description}</color>";

                    // Insert vào ComboBox với index để sắp xếp đúng thứ tự
                    // Nếu index lớn hơn số items hiện tại, thêm vào cuối
                    if (index >= ProductVariantIdentifierHistoryChangeTypeComboBoxEdit.Properties.Items.Count)
                    {
                        ProductVariantIdentifierHistoryChangeTypeComboBoxEdit.Properties.Items.Add(itemName);
                    }
                    else
                    {
                        ProductVariantIdentifierHistoryChangeTypeComboBoxEdit.Properties.Items.Insert(index, itemName);
                    }
                }

                // Cấu hình ComboBox
                ProductVariantIdentifierHistoryChangeTypeComboBoxEdit.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                ProductVariantIdentifierHistoryChangeTypeComboBoxEdit.Properties.ShowDropDown = DevExpress.XtraEditors.Controls.ShowDropDown.SingleClick;

                // Sử dụng CustomDisplayText để hiển thị Description với màu sắc
                ProductVariantIdentifierHistoryChangeTypeComboBoxEdit.Properties.CustomDisplayText += ProductVariantIdentifierHistoryChangeTypeComboBoxEdit_CustomDisplayText;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LoadProductVariantIdentifierHistoryChangeTypeEnumComboBox: Exception occurred - {ex.Message}");
            }
        }

        /// <summary>
        /// Lấy màu sắc tương ứng với loại thay đổi ProductVariantIdentifierHistoryChangeTypeEnum
        /// </summary>
        /// <param name="changeType">Loại thay đổi</param>
        /// <returns>Tên màu (color name) theo chuẩn DevExpress</returns>
        private string GetChangeTypeColor(ProductVariantIdentifierHistoryChangeTypeEnum changeType)
        {
            return changeType switch
            {
                ProductVariantIdentifierHistoryChangeTypeEnum.Nhap => "green",           // Green - Nhập
                ProductVariantIdentifierHistoryChangeTypeEnum.Xuat => "red",            // Red - Xuất
                ProductVariantIdentifierHistoryChangeTypeEnum.HoTroKyThuat => "blue",   // Blue - Hỗ trợ kỹ thuật
                ProductVariantIdentifierHistoryChangeTypeEnum.ThayDoiThongSo => "orange", // Orange - Thay đổi thông số
                ProductVariantIdentifierHistoryChangeTypeEnum.ThayDoiNguoiDung => "purple", // Purple - Thay đổi người dùng
                _ => "gray"                                                             // Gray - Khác
            };
        }

        /// <summary>
        /// Event handler để hiển thị Description với màu sắc trong ComboBoxEdit
        /// </summary>
        private void ProductVariantIdentifierHistoryChangeTypeComboBoxEdit_CustomDisplayText(object sender, DevExpress.XtraEditors.Controls.CustomDisplayTextEventArgs e)
        {
            try
            {
                if (e.Value == null) return;

                ProductVariantIdentifierHistoryChangeTypeEnum changeTypeValue;

                // Nếu giá trị là string (Description với HTML), convert về enum
                if (e.Value is string stringValue)
                {
                    var changeTypeEnum = GetChangeTypeEnumFromDescription(stringValue);
                    if (!changeTypeEnum.HasValue)
                    {
                        e.DisplayText = stringValue; // Giữ nguyên nếu không convert được
                        return;
                    }
                    changeTypeValue = changeTypeEnum.Value;
                }
                else if (e.Value is ProductVariantIdentifierHistoryChangeTypeEnum enumValue)
                {
                    changeTypeValue = enumValue;
                }
                else if (e.Value is int intValue && Enum.IsDefined(typeof(ProductVariantIdentifierHistoryChangeTypeEnum), intValue))
                {
                    changeTypeValue = (ProductVariantIdentifierHistoryChangeTypeEnum)intValue;
                }
                else
                {
                    return;
                }

                // Lấy Description và màu sắc
                var description = ApplicationEnumUtils.GetDescription(changeTypeValue);
                var colorHex = GetChangeTypeColor(changeTypeValue);

                // Tạo HTML với màu sắc theo chuẩn DevExpress
                e.DisplayText = $"<color='{colorHex}'>{description}</color>";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ProductVariantIdentifierHistoryChangeTypeComboBoxEdit_CustomDisplayText: Exception occurred - {ex.Message}");
                // Nếu có lỗi, hiển thị giá trị mặc định
                e.DisplayText = e.Value?.ToString() ?? string.Empty;
            }
        }

        /// <summary>
        /// Convert Description (có thể chứa HTML) về ProductVariantIdentifierHistoryChangeTypeEnum
        /// </summary>
        /// <param name="description">Description string (có thể chứa HTML color tags)</param>
        /// <returns>Enum value hoặc null nếu không tìm thấy</returns>
        private ProductVariantIdentifierHistoryChangeTypeEnum? GetChangeTypeEnumFromDescription(string description)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(description))
                    return null;

                // Loại bỏ HTML tags để lấy description thuần
                string cleanDescription = description;
                if (description.Contains("<color"))
                {
                    // Extract text giữa các tags: <color='...'>text</color>
                    var match = Regex.Match(description, @"<color[^>]*>([^<]+)</color>");
                    if (match.Success)
                    {
                        cleanDescription = match.Groups[1].Value;
                    }
                }

                // Tìm enum value có description khớp
                foreach (ProductVariantIdentifierHistoryChangeTypeEnum value in Enum.GetValues(typeof(ProductVariantIdentifierHistoryChangeTypeEnum)))
                {
                    var enumDescription = ApplicationEnumUtils.GetDescription(value);
                    if (enumDescription.Equals(cleanDescription, StringComparison.OrdinalIgnoreCase))
                    {
                        return value;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetChangeTypeEnumFromDescription: Exception occurred - {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Event handler khi giá trị ComboBox thay đổi
        /// </summary>
        private void ProductVariantIdentifierHistoryChangeTypeComboBoxEdit_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                var comboBoxEdit = sender as ComboBoxEdit;
                if (comboBoxEdit == null) return;

                UpdateLayoutVisibility(comboBoxEdit.EditValue);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ProductVariantIdentifierHistoryChangeTypeComboBoxEdit_EditValueChanged: Exception occurred - {ex.Message}");
            }
        }

        /// <summary>
        /// Cập nhật hiển thị/ẩn layout control dựa trên loại thay đổi
        /// </summary>
        /// <param name="editValue">Giá trị từ ComboBox (có thể là string HTML hoặc enum)</param>
        private void UpdateLayoutVisibility(object editValue)
        {
            try
            {
                ProductVariantIdentifierHistoryChangeTypeEnum? changeTypeEnum = null;

                // Convert editValue về enum
                if (editValue != null)
                {
                    if (editValue is string stringValue)
                    {
                        changeTypeEnum = GetChangeTypeEnumFromDescription(stringValue);
                    }
                    else if (editValue is ProductVariantIdentifierHistoryChangeTypeEnum enumValue)
                    {
                        changeTypeEnum = enumValue;
                    }
                    else if (editValue is int intValue && Enum.IsDefined(typeof(ProductVariantIdentifierHistoryChangeTypeEnum), intValue))
                    {
                        changeTypeEnum = (ProductVariantIdentifierHistoryChangeTypeEnum)intValue;
                    }
                }

                // Xác định có phải nhập xuất không
                bool isStockInOut = changeTypeEnum.HasValue && 
                    (changeTypeEnum.Value == ProductVariantIdentifierHistoryChangeTypeEnum.Nhap || 
                     changeTypeEnum.Value == ProductVariantIdentifierHistoryChangeTypeEnum.Xuat);

                // Cập nhật visibility
                if (isStockInOut)
                {
                    // Hiển thị LichSuNhapXuatLayoutControlItem, ẩn NotStockInOutChangeLayoutControlGroup
                    LichSuNhapXuatLayoutControlItem.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    NotStockInOutChangeLayoutControlGroup.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;

                    // Load datasource cho stockInOutMasterHistoryDtoBindingSource dựa vào _currentDto
                    LoadStockInOutMasterHistory();
                }
                else
                {
                    // Ẩn LichSuNhapXuatLayoutControlItem, hiển thị NotStockInOutChangeLayoutControlGroup
                    LichSuNhapXuatLayoutControlItem.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    NotStockInOutChangeLayoutControlGroup.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"UpdateLayoutVisibility: Exception occurred - {ex.Message}");
            }
        }

        /// <summary>
        /// Load dữ liệu lịch sử thay đổi cho ProductVariantIdentifier
        /// </summary>
        /// <param name="dto">ProductVariantIdentifierDto cần load lịch sử</param>
        public void LoadHistory(ProductVariantIdentifierDto dto)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"LoadHistory: Bắt đầu load, dto={(dto == null ? "null" : $"Id={dto.Id}, ProductVariantId={dto.ProductVariantId}")}");

                if (dto == null)
                {
                    System.Diagnostics.Debug.WriteLine("LoadHistory: DTO is null");
                    productVariantIdentifierHistoryDtoBindingSource.DataSource = new List<ProductVariantIdentifierHistoryDto>();
                    _currentDto = null;
                    return;
                }

                if (dto.Id == Guid.Empty)
                {
                    System.Diagnostics.Debug.WriteLine($"LoadHistory: DTO.Id is empty");
                    productVariantIdentifierHistoryDtoBindingSource.DataSource = new List<ProductVariantIdentifierHistoryDto>();
                    _currentDto = null;
                    return;
                }

                // Lưu DTO vào field
                _currentDto = dto;
                System.Diagnostics.Debug.WriteLine($"LoadHistory: Đã set _currentDto, ProductVariantId={_currentDto?.ProductVariantId}");

                // Load lịch sử thay đổi theo ProductVariantIdentifierId
                var histories = _productVariantIdentifierHistoryBll.GetByProductVariantIdentifierId(dto.Id);

                // Bind vào grid
                productVariantIdentifierHistoryDtoBindingSource.DataSource = histories ?? new List<ProductVariantIdentifierHistoryDto>();
                productVariantIdentifierHistoryDtoBindingSource.ResetBindings(false);

                System.Diagnostics.Debug.WriteLine($"LoadHistory: Đã load {histories?.Count ?? 0} bản ghi lịch sử cho ProductVariantIdentifierId={dto.Id}");
                System.Diagnostics.Debug.WriteLine($"LoadHistory: _currentDto sau khi load={( _currentDto == null ? "null" : $"Id={_currentDto.Id}, ProductVariantId={_currentDto.ProductVariantId}")}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LoadHistory: Exception occurred - {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"LoadHistory: StackTrace - {ex.StackTrace}");
                productVariantIdentifierHistoryDtoBindingSource.DataSource = new List<ProductVariantIdentifierHistoryDto>();
                _currentDto = null;
            }
        }

        /// <summary>
        /// Load danh sách StockInOutMasterHistoryDto dựa trên ProductVariantId của _currentDto
        /// </summary>
        private void LoadStockInOutMasterHistory()
        {
            try
            {
                if (_currentDto == null || _currentDto.ProductVariantId == Guid.Empty)
                {
                    System.Diagnostics.Debug.WriteLine("LoadStockInOutMasterHistory: _currentDto is null or ProductVariantId is empty");
                    stockInOutMasterHistoryDtoBindingSource.DataSource = new List<StockInOutMasterHistoryDto>();
                    stockInOutMasterHistoryDtoBindingSource.ResetBindings(false);
                    return;
                }

                // 1. Query StockInOutDetail theo ProductVariantId
                var productVariantIds = new List<Guid> { _currentDto.ProductVariantId };
                var stockInOutDetails = _stockInOutDetailBll.QueryByProductVariantIds(productVariantIds);

                if (stockInOutDetails == null || stockInOutDetails.Count == 0)
                {
                    System.Diagnostics.Debug.WriteLine($"LoadStockInOutMasterHistory: Không tìm thấy StockInOutDetail cho ProductVariantId={_currentDto.ProductVariantId}");
                    stockInOutMasterHistoryDtoBindingSource.DataSource = new List<StockInOutMasterHistoryDto>();
                    stockInOutMasterHistoryDtoBindingSource.ResetBindings(false);
                    return;
                }

                // 2. Lấy danh sách StockInOutMasterId (distinct)
                var masterIds = stockInOutDetails
                    .Where(d => d.StockInOutMasterId != Guid.Empty)
                    .Select(d => d.StockInOutMasterId)
                    .Distinct()
                    .ToList();

                if (masterIds.Count == 0)
                {
                    System.Diagnostics.Debug.WriteLine("LoadStockInOutMasterHistory: Không tìm thấy StockInOutMasterId");
                    stockInOutMasterHistoryDtoBindingSource.DataSource = new List<StockInOutMasterHistoryDto>();
                    stockInOutMasterHistoryDtoBindingSource.ResetBindings(false);
                    return;
                }

                // 3. Load StockInOutMasterForUIDto từ các master ID
                var masters = _stockInOutMasterBll.GetMastersByIds(masterIds);

                if (masters == null || masters.Count == 0)
                {
                    System.Diagnostics.Debug.WriteLine("LoadStockInOutMasterHistory: Không tìm thấy StockInOutMaster");
                    stockInOutMasterHistoryDtoBindingSource.DataSource = new List<StockInOutMasterHistoryDto>();
                    stockInOutMasterHistoryDtoBindingSource.ResetBindings(false);
                    return;
                }

                // 4. Convert StockInOutMasterForUIDto sang StockInOutMasterHistoryDto
                var historyDtos = masters.Select(m =>
                {
                    // Lấy tên loại nhập xuất từ enum
                    string loaiNhapXuatKhoName = ApplicationEnumUtils.GetDescription(m.LoaiNhapXuatKho);
                    
                    // Xác định StockInOutType dựa trên LoaiNhapXuatKho
                    int stockInOutType = (int)m.LoaiNhapXuatKho;

                    return new StockInOutMasterHistoryDto
                    {
                        Id = m.Id,
                        StockInOutDate = m.StockOutDate,
                        VocherNumber = m.VoucherNumber,
                        StockInOutType = stockInOutType,
                        LoaiNhapXuatKho = m.LoaiNhapXuatKho,
                        LoaiNhapXuatKhoName = loaiNhapXuatKhoName,
                        WarehouseId = m.WarehouseId,
                        WarehouseName = m.WarehouseName,
                        PurchaseOrderId = m.SalesOrderId, // Map SalesOrderId sang PurchaseOrderId
                        PartnerSiteId = m.CustomerId, // Map CustomerId sang PartnerSiteId
                        CustomerName = m.CustomerName,
                        Notes = m.Notes,
                        NguoiNhanHang = m.NguoiNhanHang,
                        NguoiGiaoHang = m.NguoiGiaoHang,
                        TotalQuantity = m.TotalQuantity,
                        TotalAmount = m.TotalAmount,
                        TotalVat = m.TotalVat,
                        TotalAmountIncludedVat = m.TotalAmountIncludedVat,
                        CreatedBy = null, // StockInOutMasterForUIDto không có
                        CreatedDate = null, // StockInOutMasterForUIDto không có
                        UpdatedBy = null, // StockInOutMasterForUIDto không có
                        UpdatedDate = null, // StockInOutMasterForUIDto không có
                        DetailsSummary = null // StockInOutMasterForUIDto không có, có thể tính toán sau nếu cần
                    };
                }).ToList();

                // 5. Bind vào binding source
                stockInOutMasterHistoryDtoBindingSource.DataSource = historyDtos;
                stockInOutMasterHistoryDtoBindingSource.ResetBindings(false);

                System.Diagnostics.Debug.WriteLine($"LoadStockInOutMasterHistory: Đã load {historyDtos.Count} phiếu nhập/xuất cho ProductVariantId={_currentDto.ProductVariantId}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LoadStockInOutMasterHistory: Exception occurred - {ex.Message}");
                stockInOutMasterHistoryDtoBindingSource.DataSource = new List<StockInOutMasterHistoryDto>();
                stockInOutMasterHistoryDtoBindingSource.ResetBindings(false);
            }
        }
    }
}
