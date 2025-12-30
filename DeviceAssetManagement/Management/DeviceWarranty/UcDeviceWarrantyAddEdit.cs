using Bll.Inventory.InventoryManagement;
using Common.Enums;
using Common.Utils;
using DTO.DeviceAssetManagement;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DeviceAssetManagement.Management.DeviceWarranty
{
    public partial class UcDeviceWarrantyAddEdit : DevExpress.XtraEditors.XtraUserControl
    {
        //#region ========== FIELDS & PROPERTIES ==========

        ///// <summary>
        ///// Business Logic Layer cho bảo hành
        ///// </summary>
        //private readonly WarrantyBll _warrantyBll = new WarrantyBll();

        ///// <summary>
        ///// Logger để ghi log các sự kiện
        ///// </summary>
        //private readonly ILogger _logger = LoggerFactory.CreateLogger(LogCategory.UI);

        ///// <summary>
        ///// Danh sách các DeviceId được chọn để thêm mới bảo hành
        ///// </summary>
        //private List<Guid> _selectedDeviceIds = new List<Guid>();

        ///// <summary>
        ///// Lấy danh sách DeviceId đã được chọn
        ///// </summary>
        //public List<Guid> SelectedDeviceIds => _selectedDeviceIds.ToList();

        ///// <summary>
        ///// Trạng thái đang lưu dữ liệu
        ///// </summary>
        //private bool _isSaving;

        //#endregion

        //#region ========== EVENTS ==========

        ///// <summary>
        ///// Event được raise khi bảo hành được lưu thành công
        ///// </summary>
        //public event EventHandler WarrantySaved;

        ///// <summary>
        ///// Event được raise khi nút Đóng được click
        ///// </summary>
        //public event EventHandler WarrantyClosed;

        //#endregion

        //public UcDeviceWarrantyAddEdit()
        //{
        //    InitializeComponent();
        //    InitializeControl();
        //}

        ///// <summary>
        ///// Khởi tạo control
        ///// </summary>
        //private void InitializeControl()
        //{
        //    try
        //    {
        //        // Load LoaiBaoHanhEnumComboBox với các giá trị enum
        //        LoadLoaiBaoHanhEnumComboBox();

        //        // Đăng ký các events cho WarrantyDtoGridView
        //        InitializeGridViewEvents();

        //        // Đăng ký các events cho các nút
        //        InitializeButtonEvents();
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log error nếu có logger, hoặc show message
        //        System.Diagnostics.Debug.WriteLine($"InitializeControl: Exception occurred - {ex.Message}");
        //    }
        //}

        ///// <summary>
        ///// Khởi tạo các event handlers cho các nút
        ///// </summary>
        //private void InitializeButtonEvents()
        //{
        //    try
        //    {
        //        // Event khi click nút Lưu
        //        SaveBarButtonItem.ItemClick += SaveBarButtonItem_ItemClick;

        //        // Event khi click nút Đóng
        //        CloseBarButtonItem.ItemClick += CloseBarButtonItem_ItemClick;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error("InitializeButtonEvents: Exception occurred", ex);
        //    }
        //}

        ///// <summary>
        ///// Khởi tạo các event handlers cho WarrantyDtoGridView
        ///// </summary>
        //private void InitializeGridViewEvents()
        //{
        //    try
        //    {
        //        // Event để validate và convert giá trị trước khi set vào property
        //        WarrantyDtoGridView.ValidatingEditor += WarrantyDtoGridView_ValidatingEditor;

        //        // Event để hiển thị HTML với màu sắc cho colWarrantyType
        //        WarrantyDtoGridView.CustomColumnDisplayText += WarrantyDtoGridView_CustomColumnDisplayText;

        //        // Event khi giá trị cell thay đổi (tính toán WarrantyUntil và lưu vào database)
        //        WarrantyDtoGridView.CellValueChanged += WarrantyDtoGridView_CellValueChanged;
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Diagnostics.Debug.WriteLine($"InitializeGridViewEvents: Exception occurred - {ex.Message}");
        //    }
        //}

        ///// <summary>
        ///// Load LoaiBaoHanhEnumComboBox với các giá trị enum và HTML tag
        ///// </summary>
        //private void LoadLoaiBaoHanhEnumComboBox()
        //{
        //    try
        //    {
        //        // Xóa các items cũ
        //        LoaiBaoHanhEnumComboBox.Items.Clear();

        //        // Thêm các giá trị enum với HTML tag để hiển thị màu sắc
        //        // Màu sắc được thiết lập trong CustomDisplayText event
        //        foreach (LoaiBaoHanhEnum value in Enum.GetValues(typeof(LoaiBaoHanhEnum)))
        //        {
        //            int index = ApplicationEnumUtils.GetValue(value);

        //            // Lấy Description và màu sắc
        //            var description = GetWarrantyTypeDescription(value);
        //            var colorHex = WarrantyDto.GetWarrantyTypeColor(value);

        //            // Tạo HTML với màu sắc
        //            string itemName = $"<color='{colorHex}'>{description}</color>";

        //            LoaiBaoHanhEnumComboBox.Items.Insert(index, itemName);
        //        }

        //        // Sử dụng CustomDisplayText để hiển thị text tương ứng
        //        LoaiBaoHanhEnumComboBox.CustomDisplayText += LoaiBaoHanhEnumComboBox_CustomDisplayText;
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Diagnostics.Debug.WriteLine($"LoadLoaiBaoHanhEnumComboBox: Exception occurred - {ex.Message}");
        //    }
        //}

        ///// <summary>
        ///// Event handler để hiển thị Description với màu sắc HTML trong LoaiBaoHanhEnumComboBox
        ///// </summary>
        //private void LoaiBaoHanhEnumComboBox_CustomDisplayText(object sender, DevExpress.XtraEditors.Controls.CustomDisplayTextEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Value == null) return;

        //        LoaiBaoHanhEnum warrantyTypeValue;

        //        // Nếu giá trị là string (Description), convert về enum
        //        if (e.Value is string stringValue)
        //        {
        //            var warrantyTypeEnum = GetWarrantyTypeEnumFromDescription(stringValue);
        //            if (!warrantyTypeEnum.HasValue)
        //            {
        //                e.DisplayText = stringValue;
        //                return;
        //            }
        //            warrantyTypeValue = warrantyTypeEnum.Value;
        //        }
        //        else if (e.Value is LoaiBaoHanhEnum enumValue)
        //        {
        //            warrantyTypeValue = enumValue;
        //        }
        //        else if (e.Value is int intValue && Enum.IsDefined(typeof(LoaiBaoHanhEnum), intValue))
        //        {
        //            warrantyTypeValue = (LoaiBaoHanhEnum)intValue;
        //        }
        //        else
        //        {
        //            return;
        //        }

        //        // Lấy Description và màu sắc
        //        var description = GetWarrantyTypeDescription(warrantyTypeValue);
        //        var colorHex = WarrantyDto.GetWarrantyTypeColor(warrantyTypeValue);

        //        // Tạo HTML với màu sắc
        //        e.DisplayText = $"<color='{colorHex}'>{description}</color>";
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Diagnostics.Debug.WriteLine($"LoaiBaoHanhEnumComboBox_CustomDisplayText: Exception occurred - {ex.Message}");
        //        // Nếu có lỗi, hiển thị giá trị mặc định
        //        e.DisplayText = e.Value?.ToString() ?? string.Empty;
        //    }
        //}

        ///// <summary>
        ///// Lấy Description từ enum value
        ///// </summary>
        ///// <param name="warrantyType">Giá trị enum</param>
        ///// <returns>Description hoặc tên enum nếu không có Description</returns>
        //private string GetWarrantyTypeDescription(LoaiBaoHanhEnum warrantyType)
        //{
        //    try
        //    {
        //        // Thử sử dụng ApplicationEnumUtils trước
        //        if (ApplicationEnumUtils.GetDescription(warrantyType) != null)
        //        {
        //            return ApplicationEnumUtils.GetDescription(warrantyType);
        //        }

        //        // Nếu không có, sử dụng DescriptionAttribute
        //        var fieldInfo = warrantyType.GetType().GetField(warrantyType.ToString());
        //        if (fieldInfo == null) return warrantyType.ToString();

        //        var descriptionAttribute = fieldInfo.GetCustomAttribute<DescriptionAttribute>();
        //        return descriptionAttribute?.Description ?? warrantyType.ToString();
        //    }
        //    catch
        //    {
        //        return warrantyType.ToString();
        //    }
        //}

        ///// <summary>
        ///// Lấy enum value từ Description string (có thể chứa HTML tags)
        ///// </summary>
        ///// <param name="description">Description string (có thể chứa HTML tags)</param>
        ///// <returns>Enum value hoặc null nếu không tìm thấy</returns>
        //private LoaiBaoHanhEnum? GetWarrantyTypeEnumFromDescription(string description)
        //{
        //    try
        //    {
        //        if (string.IsNullOrWhiteSpace(description))
        //            return null;

        //        // Strip HTML tags nếu có
        //        var cleanDescription = StripHtmlTags(description);

        //        // Duyệt qua tất cả các giá trị enum để tìm Description khớp
        //        foreach (LoaiBaoHanhEnum enumValue in Enum.GetValues(typeof(LoaiBaoHanhEnum)))
        //        {
        //            var enumDescription = GetWarrantyTypeDescription(enumValue);
        //            if (string.Equals(enumDescription, cleanDescription, StringComparison.OrdinalIgnoreCase))
        //            {
        //                return enumValue;
        //            }
        //        }

        //        return null;
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

        ///// <summary>
        ///// Loại bỏ HTML tags từ string
        ///// </summary>
        ///// <param name="htmlString">String chứa HTML tags</param>
        ///// <returns>String không có HTML tags</returns>
        //private string StripHtmlTags(string htmlString)
        //{
        //    try
        //    {
        //        if (string.IsNullOrWhiteSpace(htmlString))
        //            return htmlString;

        //        var result = htmlString;

        //        // Loại bỏ <color='...'> và </color>
        //        result = System.Text.RegularExpressions.Regex.Replace(result, @"<color=['""][^'""]*['""]>", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        //        result = System.Text.RegularExpressions.Regex.Replace(result, @"</color>", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        //        // Loại bỏ các tags khác nếu có
        //        result = System.Text.RegularExpressions.Regex.Replace(result, @"<[^>]+>", "");

        //        return result.Trim();
        //    }
        //    catch
        //    {
        //        return htmlString;
        //    }
        //}

        //#region ========== BUTTON EVENT HANDLERS ==========

        ///// <summary>
        ///// Event handler khi click nút Lưu
        ///// Lưu tất cả các thay đổi trong GridView và raise WarrantySaved event
        ///// </summary>
        //private void SaveBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    try
        //    {
        //        _logger.Debug("SaveBarButtonItem_ItemClick: Bắt đầu lưu bảo hành");

        //        // Lưu tất cả các thay đổi trong GridView
        //        WarrantyDtoGridView.PostEditor();
        //        WarrantyDtoGridView.UpdateCurrentRow();

        //        // Kiểm tra validation
        //        if (!ValidateWarrantyData())
        //        {
        //            AlertHelper.ShowWarning("Vui lòng kiểm tra lại thông tin bảo hành trước khi lưu.", "Dữ liệu không hợp lệ", this);
        //            return;
        //        }

        //        // Lưu tất cả các WarrantyDto trong GridView
        //        SaveAllWarranties();

        //        // Raise WarrantySaved event để thông báo cho form cha
        //        WarrantySaved?.Invoke(this, EventArgs.Empty);

        //        AlertHelper.ShowSuccess("Lưu bảo hành thành công!", "Thành công", this);
        //        _logger.Info("SaveBarButtonItem_ItemClick: Lưu bảo hành thành công");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error("SaveBarButtonItem_ItemClick: Exception occurred", ex);
        //        AlertHelper.ShowError($"Lỗi lưu bảo hành: {ex.Message}", "Lỗi", this);
        //    }
        //}

        ///// <summary>
        ///// Event handler khi click nút Đóng
        ///// Raise WarrantyClosed event để thông báo cho form cha
        ///// </summary>
        //private void CloseBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    try
        //    {
        //        _logger.Debug("CloseBarButtonItem_ItemClick: Đóng panel bảo hành");

        //        // Raise WarrantyClosed event để thông báo cho form cha
        //        WarrantyClosed?.Invoke(this, EventArgs.Empty);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error("CloseBarButtonItem_ItemClick: Exception occurred", ex);
        //        AlertHelper.ShowError($"Lỗi đóng panel: {ex.Message}", "Lỗi", this);
        //    }
        //}

        ///// <summary>
        ///// Validate dữ liệu bảo hành trong GridView
        ///// </summary>
        ///// <returns>True nếu dữ liệu hợp lệ, False nếu không hợp lệ</returns>
        //private bool ValidateWarrantyData()
        //{
        //    try
        //    {
        //        var warrantyDtos = warrantyDtoBindingSource.DataSource as List<WarrantyDto>;
        //        if (warrantyDtos == null || !warrantyDtos.Any())
        //        {
        //            return true; // Không có dữ liệu thì không cần validate
        //        }

        //        foreach (var warrantyDto in warrantyDtos)
        //        {
        //            if (warrantyDto.DeviceId == null || warrantyDto.DeviceId == Guid.Empty)
        //            {
        //                AlertHelper.ShowWarning("Có thiết bị không có DeviceId hợp lệ.", "Dữ liệu không hợp lệ", this);
        //                return false;
        //            }

        //            if (warrantyDto.MonthOfWarranty < 0)
        //            {
        //                AlertHelper.ShowWarning("Số tháng bảo hành phải lớn hơn hoặc bằng 0.", "Dữ liệu không hợp lệ", this);
        //                return false;
        //            }
        //        }

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error("ValidateWarrantyData: Exception occurred", ex);
        //        return false;
        //    }
        //}

        ///// <summary>
        ///// Lưu tất cả các WarrantyDto trong GridView vào database
        ///// </summary>
        //private async void SaveAllWarranties()
        //{
        //    try
        //    {
        //        var warrantyDtos = warrantyDtoBindingSource.DataSource as List<WarrantyDto>;
        //        if (warrantyDtos == null || !warrantyDtos.Any())
        //        {
        //            _logger.Debug("SaveAllWarranties: Không có dữ liệu để lưu");
        //            return;
        //        }

        //        _logger.Debug($"SaveAllWarranties: Bắt đầu lưu {warrantyDtos.Count} warranties");

        //        // Lưu từng WarrantyDto vào database
        //        await Task.Run(() =>
        //        {
        //            foreach (var warrantyDto in warrantyDtos)
        //            {
        //                if (warrantyDto.DeviceId == null || warrantyDto.DeviceId == Guid.Empty)
        //                    continue;

        //                try
        //                {
        //                    // Lưu từng WarrantyDto (sử dụng logic đồng bộ)
        //                    SaveWarrantyChangesSync(warrantyDto);
        //                }
        //                catch (Exception ex)
        //                {
        //                    _logger.Error($"SaveAllWarranties: Lỗi lưu warranty cho DeviceId={warrantyDto.DeviceId}", ex);
        //                    // Tiếp tục lưu các warranty khác
        //                }
        //            }
        //        });

        //        _logger.Info($"SaveAllWarranties: Đã lưu {warrantyDtos.Count} warranties");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error("SaveAllWarranties: Exception occurred", ex);
        //        throw;
        //    }
        //}

        ///// <summary>
        ///// Lưu thay đổi của Warranty vào database (synchronous version)
        ///// Đảm bảo ràng buộc: mỗi Device chỉ có 1 Warranty (duy nhất một loại LoaiBaoHanhEnum)
        ///// </summary>
        //private void SaveWarrantyChangesSync(WarrantyDto warrantyDto)
        //{
        //    try
        //    {
        //        if (warrantyDto.DeviceId == null || warrantyDto.DeviceId == Guid.Empty)
        //        {
        //            _logger.Warning("SaveWarrantyChangesSync: DeviceId is null or empty");
        //            return;
        //        }

        //        // Tìm Warranty hiện có cho Device này (mỗi Device chỉ có 1 Warranty)
        //        var existingWarranty = _warrantyBll.FindByDeviceId(warrantyDto.DeviceId.Value);

        //        // Nếu đã có Warranty cho Device này
        //        if (existingWarranty != null)
        //        {
        //            // Nếu đây là record mới (Id == Guid.Empty) hoặc Id khác với Warranty hiện có
        //            // => Cập nhật Warranty hiện có với thông tin mới
        //            if (warrantyDto.Id == Guid.Empty || warrantyDto.Id != existingWarranty.Id)
        //            {
        //                // Cập nhật Warranty hiện có với thông tin mới từ DTO
        //                existingWarranty.WarrantyType = (int)warrantyDto.WarrantyType;
        //                existingWarranty.WarrantyFrom = warrantyDto.WarrantyFrom;
        //                existingWarranty.MonthOfWarranty = warrantyDto.MonthOfWarranty;
        //                existingWarranty.WarrantyUntil = warrantyDto.WarrantyUntil;
        //                existingWarranty.WarrantyStatus = (int)warrantyDto.WarrantyStatus;
        //                existingWarranty.Notes = warrantyDto.Notes;
        //                existingWarranty.IsActive = warrantyDto.IsActive;
        //                existingWarranty.UpdatedDate = DateTime.Now;

        //                _warrantyBll.SaveOrUpdate(existingWarranty);

        //                _logger.Info($"SaveWarrantyChangesSync: Updated existing warranty, Id={existingWarranty.Id}, DeviceId={warrantyDto.DeviceId}, WarrantyType={warrantyDto.WarrantyType}");

        //                // Cập nhật lại warrantyDto.Id để đồng bộ với UI
        //                warrantyDto.Id = existingWarranty.Id;
        //                return;
        //            }
        //            else
        //            {
        //                // Đây là Warranty hiện có, chỉ cần cập nhật
        //                var warranty = warrantyDto.ToEntity();
        //                warranty.UpdatedDate = DateTime.Now;

        //                _warrantyBll.SaveOrUpdate(warranty);

        //                _logger.Info($"SaveWarrantyChangesSync: Updated warranty, Id={warranty.Id}, DeviceId={warrantyDto.DeviceId}");
        //                return;
        //            }
        //        }

        //        // Nếu chưa có Warranty cho Device này, tạo mới
        //        var newWarranty = warrantyDto.ToEntity();

        //        // Nếu là record mới (Id == Guid.Empty), tạo Id mới
        //        if (newWarranty.Id == Guid.Empty)
        //        {
        //            newWarranty.Id = Guid.NewGuid();
        //            newWarranty.CreatedDate = DateTime.Now;
        //        }
        //        else
        //        {
        //            newWarranty.UpdatedDate = DateTime.Now;
        //        }

        //        // Lưu vào database
        //        _warrantyBll.SaveOrUpdate(newWarranty);

        //        // Cập nhật lại warrantyDto.Id nếu là record mới
        //        if (warrantyDto.Id == Guid.Empty)
        //        {
        //            warrantyDto.Id = newWarranty.Id;
        //        }

        //        _logger.Info($"SaveWarrantyChangesSync: Created new warranty, Id={newWarranty.Id}, DeviceId={warrantyDto.DeviceId}, WarrantyType={warrantyDto.WarrantyType}");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error($"SaveWarrantyChangesSync: Exception occurred, DeviceId={warrantyDto.DeviceId}", ex);
        //        throw;
        //    }
        //}

        //#endregion

        //#region ========== PUBLIC METHODS ==========

        ///// <summary>
        ///// Load danh sách DeviceId được chọn vào UserControl
        ///// Không load datasource vào GridView, user sẽ tự thêm dòng mới
        ///// </summary>
        ///// <param name="selectedDeviceIds">Danh sách DeviceId của các thiết bị được chọn</param>
        //public void LoadSelectedDeviceIds(List<Guid> selectedDeviceIds)
        //{
        //    try
        //    {
        //        if (selectedDeviceIds == null || !selectedDeviceIds.Any())
        //        {
        //            _selectedDeviceIds = new List<Guid>();
        //            // Không load datasource, để grid trống
        //            warrantyDtoBindingSource.DataSource = new List<WarrantyDto>();
        //            return;
        //        }

        //        // Lưu danh sách DeviceId được chọn
        //        _selectedDeviceIds = selectedDeviceIds
        //            .Where(id => id != Guid.Empty)
        //            .Distinct()
        //            .ToList();

        //        _logger.Debug($"LoadSelectedDeviceIds: Loaded {_selectedDeviceIds.Count} device ID(s)");

        //        // Không load datasource vào GridView
        //        // User sẽ tự thêm dòng mới trong GridView và chọn DeviceId từ danh sách
        //        warrantyDtoBindingSource.DataSource = new List<WarrantyDto>();
        //        WarrantyDtoGridView.RefreshData();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error("LoadSelectedDeviceIds: Exception occurred", ex);
        //        AlertHelper.ShowError($"Lỗi load danh sách thiết bị: {ex.Message}", "Lỗi", this);
        //    }
        //}

        ///// <summary>
        ///// Load WarrantyDto vào GridView để điều chỉnh
        ///// Load datasource cho warrantyDtoBindingSource với WarrantyDto đã chọn
        ///// </summary>
        ///// <param name="warrantyDto">WarrantyDto cần điều chỉnh</param>
        //public void LoadWarrantyForEdit(WarrantyDto warrantyDto)
        //{
        //    try
        //    {
        //        if (warrantyDto == null)
        //        {
        //            _logger.Warning("LoadWarrantyForEdit: warrantyDto is null");
        //            warrantyDtoBindingSource.DataSource = new List<WarrantyDto>();
        //            return;
        //        }

        //        // Kiểm tra xem có DeviceId hợp lệ không
        //        if (warrantyDto.DeviceId == null || warrantyDto.DeviceId == Guid.Empty)
        //        {
        //            _logger.Warning("LoadWarrantyForEdit: DeviceId is null or empty");
        //            AlertHelper.ShowWarning("Thiết bị không có DeviceId hợp lệ.", "Lỗi", this);
        //            warrantyDtoBindingSource.DataSource = new List<WarrantyDto>();
        //            return;
        //        }

        //        _logger.Debug($"LoadWarrantyForEdit: Loading warranty for DeviceId={warrantyDto.DeviceId}, WarrantyId={warrantyDto.Id}");

        //        // Nếu WarrantyDto có Id = Guid.Empty, tìm Warranty hiện có trong database
        //        if (warrantyDto.Id == Guid.Empty)
        //        {
        //            var existingWarranty = _warrantyBll.FindByDeviceId(warrantyDto.DeviceId.Value);
        //            if (existingWarranty != null)
        //            {
        //                // Convert sang DTO
        //                warrantyDto = existingWarranty.ToDto();
        //                _logger.Debug($"LoadWarrantyForEdit: Found existing warranty, Id={warrantyDto.Id}");
        //            }
        //            else
        //            {
        //                // Nếu không tìm thấy Warranty, tạo WarrantyDto mới
        //                warrantyDto = new WarrantyDto
        //                {
        //                    Id = Guid.Empty,
        //                    DeviceId = warrantyDto.DeviceId,
        //                    WarrantyType = LoaiBaoHanhEnum.NCCToVNS,
        //                    WarrantyFrom = DateTime.Now,
        //                    MonthOfWarranty = 0,
        //                    WarrantyUntil = null,
        //                    WarrantyStatus = TrangThaiBaoHanhEnum.ChoXuLy,
        //                    Notes = string.Empty,
        //                    IsActive = true,
        //                    CreatedDate = DateTime.Now,
        //                    UpdatedDate = null
        //                };
        //                _logger.Debug("LoadWarrantyForEdit: Created new WarrantyDto");
        //            }
        //        }

        //        // Load datasource với WarrantyDto đã chọn
        //        var warrantyDtos = new List<WarrantyDto> { warrantyDto };
        //        warrantyDtoBindingSource.DataSource = warrantyDtos;
        //        WarrantyDtoGridView.RefreshData();

        //        // Lưu DeviceId vào danh sách selected
        //        _selectedDeviceIds = new List<Guid> { warrantyDto.DeviceId.Value };

        //        _logger.Info($"LoadWarrantyForEdit: Loaded warranty successfully, DeviceId={warrantyDto.DeviceId}");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error("LoadWarrantyForEdit: Exception occurred", ex);
        //        AlertHelper.ShowError($"Lỗi load thông tin bảo hành: {ex.Message}", "Lỗi", this);
        //    }
        //}

        //#endregion

        //#region ========== GRIDVIEW EVENTS ==========

        ///// <summary>
        ///// Event handler để validate và convert giá trị trước khi set vào property
        ///// Xử lý conversion từ string (HTML Description) sang enum
        ///// </summary>
        //private void WarrantyDtoGridView_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        //{
        //    try
        //    {
        //        var view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
        //        if (view == null) return;

        //        var focusedColumn = view.FocusedColumn;
        //        if (focusedColumn != colWarrantyType) return;

        //        // Nếu giá trị là string (Description với HTML), convert về enum
        //        if (e.Value is string warrantyTypeDescription)
        //        {
        //            var warrantyTypeEnum = GetWarrantyTypeEnumFromDescription(warrantyTypeDescription);
        //            if (warrantyTypeEnum.HasValue)
        //            {
        //                // Set lại giá trị là enum để DevExpress có thể bind đúng
        //                e.Value = warrantyTypeEnum.Value;
        //                e.Valid = true;
        //            }
        //            else
        //            {
        //                System.Diagnostics.Debug.WriteLine($"WarrantyDtoGridView_ValidatingEditor: Cannot convert warranty type description '{warrantyTypeDescription}' to enum");
        //                e.ErrorText = $"Không thể chuyển đổi loại bảo hành '{warrantyTypeDescription}'";
        //                e.Valid = false;
        //            }
        //        }
        //        // Nếu giá trị đã là enum, giữ nguyên
        //        else if (e.Value is LoaiBaoHanhEnum)
        //        {
        //            e.Valid = true;
        //        }
        //        // Nếu giá trị là int, convert về enum
        //        else if (e.Value is int intValue && Enum.IsDefined(typeof(LoaiBaoHanhEnum), intValue))
        //        {
        //            e.Value = (LoaiBaoHanhEnum)intValue;
        //            e.Valid = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Diagnostics.Debug.WriteLine($"WarrantyDtoGridView_ValidatingEditor: Exception occurred - {ex.Message}");
        //        e.ErrorText = $"Lỗi xử lý giá trị: {ex.Message}";
        //        e.Valid = false;
        //    }
        //}

        ///// <summary>
        ///// Event handler để hiển thị HTML với màu sắc cho colWarrantyType trong GridView
        ///// </summary>
        //private void WarrantyDtoGridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        //{
        //    try
        //    {
        //        // Chỉ xử lý cột WarrantyType
        //        if (e.Column != colWarrantyType || e.Value == null)
        //            return;

        //        LoaiBaoHanhEnum warrantyTypeValue;

        //        // Convert giá trị về enum
        //        if (e.Value is LoaiBaoHanhEnum enumValue)
        //        {
        //            warrantyTypeValue = enumValue;
        //        }
        //        else if (e.Value is int intValue && Enum.IsDefined(typeof(LoaiBaoHanhEnum), intValue))
        //        {
        //            warrantyTypeValue = (LoaiBaoHanhEnum)intValue;
        //        }
        //        else if (e.Value is string stringValue)
        //        {
        //            // Nếu là string, thử strip HTML và convert
        //            var cleanString = StripHtmlTags(stringValue);
        //            var warrantyTypeEnum = GetWarrantyTypeEnumFromDescription(cleanString);
        //            if (!warrantyTypeEnum.HasValue)
        //            {
        //                return; // Không thể convert, giữ nguyên giá trị
        //            }
        //            warrantyTypeValue = warrantyTypeEnum.Value;
        //        }
        //        else
        //        {
        //            return;
        //        }

        //        // Lấy Description và màu sắc
        //        var description = GetWarrantyTypeDescription(warrantyTypeValue);
        //        var colorHex = WarrantyDto.GetWarrantyTypeColor(warrantyTypeValue);

        //        // Tạo HTML với màu sắc
        //        e.DisplayText = $"<color='{colorHex}'>{description}</color>";
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Diagnostics.Debug.WriteLine($"WarrantyDtoGridView_CustomColumnDisplayText: Exception occurred - {ex.Message}");
        //        // Nếu có lỗi, giữ nguyên giá trị mặc định
        //    }
        //}

        ///// <summary>
        ///// Event handler khi giá trị cell thay đổi trong GridView
        ///// Tự động tính toán WarrantyUntil và lưu thay đổi vào database
        ///// </summary>
        //private void WarrantyDtoGridView_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        //{
        //    try
        //    {
        //        if (_isSaving) return; // Tránh recursive call

        //        // Xử lý tính toán WarrantyUntil trước (cho cả new row và existing row)
        //        if (e.Column == colWarrantyFrom || e.Column == colMonthOfWarranty)
        //        {
        //            CalculateAndUpdateWarrantyUntil(e.RowHandle);
        //        }

        //        // Lấy WarrantyDto từ row hiện tại (chỉ lưu nếu là existing row)
        //        if (e.RowHandle < 0) return; // New row chưa cần lưu vào database

        //        var warrantyDto = WarrantyDtoGridView.GetRow(e.RowHandle) as WarrantyDto;
        //        if (warrantyDto == null || warrantyDto.DeviceId == null || warrantyDto.DeviceId == Guid.Empty)
        //            return;

        //        _logger.Debug($"WarrantyDtoGridView_CellValueChanged: Updating warranty, DeviceId={warrantyDto.DeviceId}, Column: {e.Column.FieldName}");

        //        // Lưu thay đổi vào database
        //        SaveWarrantyChangesAsync(warrantyDto);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error("WarrantyDtoGridView_CellValueChanged: Exception occurred", ex);
        //        AlertHelper.ShowError($"Lỗi lưu thay đổi: {ex.Message}", "Lỗi", this);
        //    }
        //}

        ///// <summary>
        ///// Tính toán và cập nhật WarrantyUntil từ WarrantyFrom và MonthOfWarranty
        ///// Xử lý cả trường hợp new row (e.RowHandle < 0) và existing row
        ///// </summary>
        ///// <param name="rowHandle">Row handle của row cần tính toán</param>
        //private void CalculateAndUpdateWarrantyUntil(int rowHandle)
        //{
        //    try
        //    {
        //        // Đảm bảo giá trị đã được cập nhật vào DTO trước khi tính toán
        //        WarrantyDtoGridView.PostEditor();
        //        WarrantyDtoGridView.UpdateCurrentRow();

        //        WarrantyDto warrantyDto = null;

        //        // Xử lý cả trường hợp new row (rowHandle < 0) và existing row
        //        if (rowHandle >= 0)
        //        {
        //            warrantyDto = WarrantyDtoGridView.GetRow(rowHandle) as WarrantyDto;
        //        }
        //        else
        //        {
        //            // New row: lấy từ focused row
        //            var focusedRowHandle = WarrantyDtoGridView.FocusedRowHandle;
        //            if (focusedRowHandle >= 0)
        //            {
        //                warrantyDto = WarrantyDtoGridView.GetRow(focusedRowHandle) as WarrantyDto;
        //                rowHandle = focusedRowHandle; // Sử dụng focused row handle
        //            }
        //            else
        //            {
        //                // Lấy từ datasource nếu có
        //                var dataSource = warrantyDtoBindingSource.DataSource as List<WarrantyDto>;
        //                if (dataSource != null && dataSource.Any())
        //                {
        //                    warrantyDto = dataSource.LastOrDefault();
        //                    // Tìm row handle của warrantyDto này
        //                    for (int i = 0; i < WarrantyDtoGridView.RowCount; i++)
        //                    {
        //                        var row = WarrantyDtoGridView.GetRow(i) as WarrantyDto;
        //                        if (row == warrantyDto)
        //                        {
        //                            rowHandle = i;
        //                            break;
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        if (warrantyDto == null) return;

        //        DateTime? newWarrantyUntil = null;

        //        // Tính toán WarrantyUntil nếu có đủ thông tin
        //        if (warrantyDto.WarrantyFrom.HasValue && warrantyDto.MonthOfWarranty > 0)
        //        {
        //            newWarrantyUntil = warrantyDto.WarrantyFrom.Value.AddMonths(warrantyDto.MonthOfWarranty);
        //        }

        //        // Chỉ cập nhật nếu giá trị thay đổi
        //        if (warrantyDto.WarrantyUntil != newWarrantyUntil)
        //        {
        //            // Tạm thời set _isSaving = true để tránh trigger CellValueChanged khi SetRowCellValue
        //            var wasSaving = _isSaving;
        //            _isSaving = true;

        //            try
        //            {
        //                // Sử dụng SetRowCellValue để cập nhật giá trị trong GridView
        //                if (rowHandle >= 0)
        //                {
        //                    WarrantyDtoGridView.SetRowCellValue(rowHandle, colWarrantyUntil, newWarrantyUntil);
        //                }
        //                else
        //                {
        //                    // Nếu không có row handle hợp lệ, cập nhật trực tiếp vào DTO
        //                    warrantyDto.WarrantyUntil = newWarrantyUntil;
        //                }

        //                // Cập nhật property trong DTO để đồng bộ
        //                warrantyDto.WarrantyUntil = newWarrantyUntil;

        //                // Refresh row để hiển thị giá trị mới
        //                if (rowHandle >= 0)
        //                {
        //                    WarrantyDtoGridView.RefreshRow(rowHandle);
        //                }
        //            }
        //            finally
        //            {
        //                // Khôi phục lại trạng thái _isSaving
        //                _isSaving = wasSaving;
        //            }

        //            if (newWarrantyUntil.HasValue)
        //            {
        //                _logger.Debug($"CalculateAndUpdateWarrantyUntil: Updated WarrantyUntil to {newWarrantyUntil.Value:dd/MM/yyyy} for DeviceId={warrantyDto.DeviceId}");
        //            }
        //            else
        //            {
        //                _logger.Debug($"CalculateAndUpdateWarrantyUntil: Cleared WarrantyUntil for DeviceId={warrantyDto.DeviceId}");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error("CalculateAndUpdateWarrantyUntil: Exception occurred", ex);
        //    }
        //}


        ///// <summary>
        ///// Lưu thay đổi của Warranty vào database
        ///// Đảm bảo ràng buộc: mỗi Device chỉ có 1 Warranty (duy nhất một loại LoaiBaoHanhEnum)
        ///// </summary>
        //private async void SaveWarrantyChangesAsync(WarrantyDto warrantyDto)
        //{
        //    if (_isSaving) return;
        //    _isSaving = true;

        //    try
        //    {
        //        await Task.Run(() =>
        //        {
        //            try
        //            {
        //                if (warrantyDto.DeviceId == null || warrantyDto.DeviceId == Guid.Empty)
        //                {
        //                    _logger.Warning("SaveWarrantyChangesAsync: DeviceId is null or empty");
        //                    return;
        //                }

        //                // Tìm Warranty hiện có cho Device này (mỗi Device chỉ có 1 Warranty)
        //                var existingWarranty = _warrantyBll.FindByDeviceId(warrantyDto.DeviceId.Value);

        //                // Nếu đã có Warranty cho Device này
        //                if (existingWarranty != null)
        //                {
        //                    // Nếu đây là record mới (Id == Guid.Empty) hoặc Id khác với Warranty hiện có
        //                    // => Cập nhật Warranty hiện có với thông tin mới
        //                    if (warrantyDto.Id == Guid.Empty || warrantyDto.Id != existingWarranty.Id)
        //                    {
        //                        // Cập nhật Warranty hiện có với thông tin mới từ DTO
        //                        existingWarranty.WarrantyType = (int)warrantyDto.WarrantyType;
        //                        existingWarranty.WarrantyFrom = warrantyDto.WarrantyFrom;
        //                        existingWarranty.MonthOfWarranty = warrantyDto.MonthOfWarranty;
        //                        existingWarranty.WarrantyUntil = warrantyDto.WarrantyUntil;
        //                        existingWarranty.WarrantyStatus = (int)warrantyDto.WarrantyStatus;
        //                        existingWarranty.Notes = warrantyDto.Notes;
        //                        existingWarranty.IsActive = warrantyDto.IsActive;
        //                        existingWarranty.UpdatedDate = DateTime.Now;

        //                        _warrantyBll.SaveOrUpdate(existingWarranty);

        //                        _logger.Info($"SaveWarrantyChangesAsync: Updated existing warranty, Id={existingWarranty.Id}, DeviceId={warrantyDto.DeviceId}, WarrantyType={warrantyDto.WarrantyType}");

        //                        // Cập nhật lại warrantyDto.Id để đồng bộ với UI
        //                        warrantyDto.Id = existingWarranty.Id;

        //                        // Update UI thread để refresh grid
        //                        BeginInvoke(new Action(() =>
        //                        {
        //                            WarrantyDtoGridView.RefreshData();
        //                        }));

        //                        return;
        //                    }
        //                    else
        //                    {
        //                        // Đây là Warranty hiện có, chỉ cần cập nhật
        //                        var warranty = warrantyDto.ToEntity();
        //                        warranty.UpdatedDate = DateTime.Now;

        //                        _warrantyBll.SaveOrUpdate(warranty);

        //                        _logger.Info($"SaveWarrantyChangesAsync: Updated warranty, Id={warranty.Id}, DeviceId={warrantyDto.DeviceId}");

        //                        // Update UI thread để refresh grid
        //                        BeginInvoke(new Action(() =>
        //                        {
        //                            WarrantyDtoGridView.RefreshData();
        //                        }));

        //                        return;
        //                    }
        //                }

        //                // Nếu chưa có Warranty cho Device này, tạo mới
        //                var newWarranty = warrantyDto.ToEntity();

        //                // Nếu là record mới (Id == Guid.Empty), tạo Id mới
        //                if (newWarranty.Id == Guid.Empty)
        //                {
        //                    newWarranty.Id = Guid.NewGuid();
        //                    newWarranty.CreatedDate = DateTime.Now;
        //                }
        //                else
        //                {
        //                    newWarranty.UpdatedDate = DateTime.Now;
        //                }

        //                // Lưu vào database
        //                _warrantyBll.SaveOrUpdate(newWarranty);

        //                // Cập nhật lại warrantyDto.Id nếu là record mới
        //                if (warrantyDto.Id == Guid.Empty)
        //                {
        //                    warrantyDto.Id = newWarranty.Id;
        //                }

        //                _logger.Info($"SaveWarrantyChangesAsync: Created new warranty, Id={newWarranty.Id}, DeviceId={warrantyDto.DeviceId}, WarrantyType={warrantyDto.WarrantyType}");

        //                // Update UI thread để refresh grid
        //                BeginInvoke(new Action(() =>
        //                {
        //                    WarrantyDtoGridView.RefreshData();
        //                }));
        //            }
        //            catch (Exception ex)
        //            {
        //                _logger.Error($"SaveWarrantyChangesAsync: Exception occurred, DeviceId={warrantyDto.DeviceId}", ex);
        //                throw;
        //            }
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error("SaveWarrantyChangesAsync: Exception occurred", ex);
        //        AlertHelper.ShowError($"Lỗi lưu thay đổi bảo hành: {ex.Message}", "Lỗi", this);
        //    }
        //    finally
        //    {
        //        _isSaving = false;
        //    }
        //}

        //#endregion
    }
}
