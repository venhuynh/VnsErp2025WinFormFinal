# Tài Liệu Kỹ Thuật - UcBusinessPartnerCategory (User Control Quản Lý Danh Mục Đối Tác)

## Mục Lục

1. [Overview](#overview)
2. [Architecture](#architecture)
3. [Detailed Technical Breakdown](#detailed-technical-breakdown)
4. [Validation System](#validation-system)
5. [Business Logic Flow](#business-logic-flow)
6. [Error Handling](#error-handling)
7. [Security & Best Practices](#security--best-practices)
8. [Extensibility Guide](#extensibility-guide)
9. [Test Checklist](#test-checklist)
10. [Changelog Template](#changelog-template)

---

## 1. Overview

### 1.1. Vai Trò Trong Module

**UcBusinessPartnerCategory** là User Control thuộc module **MasterData.Customer**, có vai trò:

- **Quản lý danh sách danh mục đối tác** với cấu trúc phân cấp (hierarchical)
- **Hiển thị dạng cây** (TreeList) với thông tin số lượng đối tác
- **CRUD operations**: Create, Read, Update, Delete
- **Export dữ liệu** ra Excel
- **Multi-select support** với checkbox và regular selection

### 1.2. File Structure

```
MasterData/Customer/
├── UcBusinessPartnerCategory.cs                    # Main code-behind file
├── UcBusinessPartnerCategory.Designer.cs           # Designer-generated code
└── UcBusinessPartnerCategory.resx                  # Resources file

MasterData/Customer/Dto/
└── BusinessPartnerCategoryDto.cs                   # Data Transfer Object

MasterData/Customer/Converters/
└── BusinessPartnerCategoryConverters.cs           # Entity ↔ DTO converter

MasterData/Customer/
└── FrmBusinessPartnerCategoryDetail.cs             # Detail form (Add/Edit)

Bll/MasterData/Customer/
└── BusinessPartnerCategoryBll.cs                   # Business Logic Layer

Dal/DataAccess/MasterData/Partner/
└── BusinessPartnerCategoryDataAccess.cs            # Data Access Layer

Bll/Utils/
└── SuperToolTipHelper.cs                           # Helper tạo SuperToolTip
```

### 1.3. Dependencies

**DevExpress Controls:**
- `XtraUserControl` - Base class
- `TreeList` - Hierarchical data display
- `BarManager`, `BarButtonItem` - Toolbar
- `BindingSource` - Data binding
- `LayoutControl` - Layout container
- `SplashScreenManager` - Loading indicator
- `OverlayManager` - Overlay for modal forms
- `SuperToolTip` - Rich tooltips

**Internal Dependencies:**
- `Bll.MasterData.Customer.BusinessPartnerCategoryBll` - Business logic
- `Dal.DataAccess.MasterData.Partner.BusinessPartnerCategoryDataAccess` - Data access
- `MasterData.Customer.Dto.BusinessPartnerCategoryDto` - DTO
- `MasterData.Customer.Converters.BusinessPartnerCategoryConverters` - Converter
- `Bll.Utils.SuperToolTipHelper` - Tooltip helper
- `Bll.Common.MsgBox` - Message box helper
- `Bll.Common.OverlayManager` - Overlay manager

---

## 2. Architecture

### 2.1. Layer Architecture

```
┌─────────────────────────────────────────────────────────┐
│                    UI Layer (UcBusinessPartnerCategory)│
│  ┌───────────────────────────────────────────────────┐  │
│  │  - XtraUserControl                                │  │
│  │  - TreeList (Hierarchical display)                │  │
│  │  - BarManager (Toolbar)                          │  │
│  │  - Event Handlers                                 │  │
│  │  - Selection Management                           │  │
│  └───────────────────────────────────────────────────┘  │
└───────────────────────┬─────────────────────────────────┘
                        │
                        │ Uses
                        ▼
┌─────────────────────────────────────────────────────────┐
│      Business Logic Layer (BusinessPartnerCategoryBll)  │
│  ┌───────────────────────────────────────────────────┐  │
│  │  - GetCategoriesWithCountsAsync()                 │  │
│  │  - GetAllAsync()                                  │  │
│  │  - Delete(id)                                     │  │
│  │  - GetPartnerCountByCategoryAsync()               │  │
│  └───────────────────────────────────────────────────┘  │
└───────────────────────┬─────────────────────────────────┘
                        │
                        │ Uses
                        ▼
┌─────────────────────────────────────────────────────────┐
│    Data Access Layer (BusinessPartnerCategoryDataAccess)│
│  ┌───────────────────────────────────────────────────┐  │
│  │  - GetAllAsync()                                 │  │
│  │  - GetPartnerCountByCategoryAsync()               │  │
│  │  - DeleteCategory(id)                             │  │
│  └───────────────────────────────────────────────────┘  │
└───────────────────────┬─────────────────────────────────┘
                        │
                        │ Uses
                        ▼
┌─────────────────────────────────────────────────────────┐
│              Database (LINQ to SQL)                     │
│  ┌───────────────────────────────────────────────────┐  │
│  │  - BusinessPartnerCategory Table                 │  │
│  │  - BusinessPartnerCategoryMapping Table          │  │
│  └───────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────┘
```

### 2.2. Data Flow Diagram

```
┌──────────────┐
│   Database   │
│   (Tables)   │
└──────┬───────┘
       │
       │ Read
       ▼
┌──────────────────┐
│ BusinessPartner  │
│ CategoryDataAccess│
│  GetAllAsync()    │
│  GetPartnerCount  │
│  ByCategoryAsync()│
└──────┬───────────┘
       │
       │ Returns Entities + Counts
       ▼
┌──────────────────┐
│ BusinessPartner  │
│ CategoryBll      │
│  GetCategories   │
│  WithCountsAsync()│
└──────┬───────────┘
       │
       │ Returns (Categories, Counts)
       ▼
┌──────────────────┐
│  UcBusinessPartner│
│  Category         │
│  LoadDataAsync()  │
└──────┬───────────┘
       │
       │ Converts to DTOs
       ▼
┌──────────────────┐
│ BusinessPartner  │
│ CategoryConverters│
│  ToDtosWith      │
│  Hierarchy()     │
└──────┬───────────┘
       │
       │ Returns Hierarchical DTOs
       ▼
┌──────────────────┐
│   TreeList       │
│   (UI Display)   │
└──────────────────┘
```

### 2.3. Component Diagram (ASCII)

```
┌─────────────────────────────────────────────────────────────┐
│                  UcBusinessPartnerCategory                   │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│  ┌────────────────────────────────────────────────────┐    │
│  │              BarManager (Toolbar)                   │    │
│  │  ┌──────────────────────────────────────────────┐ │    │
│  │  │  ListDataBarButtonItem (Refresh)            │ │    │
│  │  │  NewBarButtonItem (Add)                     │ │    │
│  │  │  EditBarButtonItem (Edit - 1 row only)      │ │    │
│  │  │  DeleteBarButtonItem (Delete - multi)       │ │    │
│  │  │  ExportBarButtonItem (Export Excel)         │ │    │
│  │  └──────────────────────────────────────────────┘ │    │
│  └────────────────────────────────────────────────────┘    │
│                                                              │
│  ┌────────────────────────────────────────────────────┐    │
│  │          LayoutControl (Layout Container)          │    │
│  │                                                       │    │
│  │  ┌──────────────────────────────────────────────┐  │    │
│  │  │  TreeList (Hierarchical Data Display)       │  │    │
│  │  │  ┌──────────────────────────────────────┐ │  │    │
│  │  │  │  Columns:                            │ │  │    │
│  │  │  │  - colCategoryName                   │ │  │    │
│  │  │  │  - colDescription                    │ │  │    │
│  │  │  │  - colPartnerCount                   │ │  │    │
│  │  │  └──────────────────────────────────────┘ │  │    │
│  │  │  Features:                                │  │    │
│  │  │  - Checkbox selection                    │  │    │
│  │  │  - Multi-select                          │  │    │
│  │  │  - Custom draw (colors, row numbers)     │  │    │
│  │  │  - Hierarchical structure                │  │    │
│  │  └──────────────────────────────────────────────┘  │    │
│  └────────────────────────────────────────────────────┘    │
│                                                              │
│  ┌────────────────────────────────────────────────────┐    │
│  │      BindingSource (Data Binding)                   │    │
│  │  businessPartnerCategoryDtoBindingSource            │    │
│  └────────────────────────────────────────────────────┘    │
│                                                              │
│  ┌────────────────────────────────────────────────────┐    │
│  │          BusinessPartnerCategoryBll                 │    │
│  └────────────────────────────────────────────────────┘    │
│                                                              │
└─────────────────────────────────────────────────────────────┘
```

---

## 3. Detailed Technical Breakdown

### 3.1. Controls trong Designer

#### 3.1.1. BarManager & Toolbar

```csharp
private BarManager barManager1;
private Bar bar2;
private BarButtonItem ListDataBarButtonItem;
private BarButtonItem NewBarButtonItem;
private BarButtonItem EditBarButtonItem;
private BarButtonItem DeleteBarButtonItem;
private BarButtonItem ExportBarButtonItem;
```

**Cấu hình:**
- `bar2`: Main menu bar (Dock = Top)
- `ListDataBarButtonItem`: Caption = "Danh sách", Image = `list_16x16`
- `NewBarButtonItem`: Caption = "Mới", Image = `addnewdatasource_16x16`
- `EditBarButtonItem`: Caption = "Điều chỉnh", Image = `edittask_16x16`
- `DeleteBarButtonItem`: Caption = "Xóa", Image = `deletelist_16x16`
- `ExportBarButtonItem`: Caption = "Xuất", Image = `exporttoxls_16x16`

#### 3.1.2. TreeList

```csharp
private TreeList treeList1;
```

**Cấu hình:**
- `KeyFieldName`: "Id"
- `ParentFieldName`: "ParentId"
- `DataSource`: `businessPartnerCategoryDtoBindingSource`
- `OptionsBehavior.AllowRecursiveNodeChecking`: true
- `OptionsSelection.MultiSelect`: true
- `OptionsView.CheckBoxStyle`: Check
- `OptionsView.ShowCaption`: true
- `Caption`: "BẢNG QUẢN LÝ PHÂN LOẠI ĐỔI TÁC"

**Columns:**
- `colCategoryName`: FieldName = "CategoryName"
- `colDescription`: FieldName = "Description"
- `colPartnerCount`: FieldName = "PartnerCount", Caption = "Số lượng"

#### 3.1.3. BindingSource

```csharp
private BindingSource businessPartnerCategoryDtoBindingSource;
```

**Cấu hình:**
- `DataSource`: typeof(BusinessPartnerCategoryDto)

#### 3.1.4. LayoutControl

```csharp
private LayoutControl layoutControl1;
private LayoutControlGroup Root;
private LayoutControlItem layoutControlItem1;
```

**Cấu hình:**
- `Dock`: Fill
- Chứa `treeList1`

### 3.2. Event Handlers

#### 3.2.1. ListDataBarButtonItem_ItemClick

```csharp
private async void ListDataBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
```

**Chức năng:**
- Tải lại dữ liệu từ database
- Hiển thị WaitForm trong khi tải
- Refresh TreeList sau khi tải xong

**Flow:**
```
Click "Danh sách"
  ├─> LoadDataAsync()
  │   ├─> ExecuteWithWaitingFormAsync()
  │   │   ├─> SplashScreenManager.ShowForm()
  │   │   ├─> LoadDataAsyncWithoutSplash()
  │   │   │   ├─> GetCategoriesWithCountsAsync()
  │   │   │   ├─> ToDtosWithHierarchy()
  │   │   │   └─> BindGrid()
  │   │   └─> SplashScreenManager.CloseForm()
  │   └─> UpdateButtonStates()
```

#### 3.2.2. NewBarButtonItem_ItemClick

```csharp
private async void NewBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
```

**Chức năng:**
- Mở form `FrmBusinessPartnerCategoryDetail` với `Guid.Empty` (thêm mới)
- Hiển thị overlay
- Refresh danh sách sau khi đóng form

**Flow:**
```
Click "Mới"
  ├─> OverlayManager.ShowScope()
  ├─> new FrmBusinessPartnerCategoryDetail(Guid.Empty)
  ├─> form.ShowDialog()
  ├─> LoadDataAsync() (after dialog closes)
  └─> UpdateButtonStates()
```

#### 3.2.3. EditBarButtonItem_ItemClick

```csharp
private async void EditBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
```

**Chức năng:**
- Kiểm tra selection (phải chọn đúng 1 dòng)
- Lấy DTO từ selection
- Mở form `FrmBusinessPartnerCategoryDetail` với category ID
- Refresh danh sách sau khi đóng form

**Flow:**
```
Click "Điều chỉnh"
  ├─> Check selection count (must be 1)
  ├─> Get selected category ID
  ├─> Get DTO from BindingSource
  ├─> OverlayManager.ShowScope()
  ├─> new FrmBusinessPartnerCategoryDetail(categoryId)
  ├─> form.ShowDialog()
  ├─> LoadDataAsync() (after dialog closes)
  └─> UpdateButtonStates()
```

#### 3.2.4. DeleteBarButtonItem_ItemClick

```csharp
private async void DeleteBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
```

**Chức năng:**
- Kiểm tra selection (phải chọn ít nhất 1 dòng)
- Hiển thị hộp thoại xác nhận
- Xóa các danh mục theo thứ tự (con trước, cha sau)
- Refresh danh sách sau khi xóa

**Flow:**
```
Click "Xóa"
  ├─> Check selection count (must be >= 1)
  ├─> MsgBox.ShowYesNo(confirmMessage)
  ├─> ExecuteWithWaitingFormAsync()
  │   ├─> DeleteCategoriesInOrder(selectedIds)
  │   │   ├─> GetAllAsync() (to get all categories)
  │   │   ├─> CalculateCategoryLevel() (for each)
  │   │   ├─> Sort by level (descending)
  │   │   └─> Delete each category
  │   └─> LoadDataAsync() (refresh)
```

#### 3.2.5. ExportBarButtonItem_ItemClick

```csharp
private void ExportBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
```

**Chức năng:**
- Kiểm tra có dữ liệu hiển thị không
- Mở SaveFileDialog
- Xuất TreeList ra Excel

**Flow:**
```
Click "Xuất"
  ├─> Check rowCount > 0
  ├─> SaveFileDialog.ShowDialog()
  ├─> treeList1.ExportToXlsx(fileName)
  └─> ShowInfo("Xuất dữ liệu thành công!")
```

#### 3.2.6. TreeList1_SelectionChanged

```csharp
private void TreeList1_SelectionChanged(object sender, EventArgs e)
```

**Chức năng:**
- Cập nhật danh sách selected category IDs
- Cập nhật trạng thái các nút toolbar

**Flow:**
```
Selection Changed
  ├─> UpdateSelectedCategoryIds()
  │   ├─> Clear _selectedCategoryIds
  │   ├─> CheckNodeRecursive() (for each root node)
  │   └─> Add IDs from checked/selected nodes
  └─> UpdateButtonStates()
```

#### 3.2.7. TreeList1_AfterCheckNode

```csharp
private void TreeList1_AfterCheckNode(object sender, NodeEventArgs e)
```

**Chức năng:**
- Cập nhật danh sách selected category IDs khi checkbox thay đổi
- Cập nhật trạng thái các nút toolbar

**Flow:**
```
Checkbox Changed
  ├─> UpdateSelectedCategoryIds()
  └─> UpdateButtonStates()
```

#### 3.2.8. TreeList1_CustomDrawNodeIndicator

```csharp
private void TreeList1_CustomDrawNodeIndicator(object sender, CustomDrawNodeIndicatorEventArgs e)
```

**Chức năng:**
- Vẽ số thứ tự dòng vào indicator column

**Code:**
```csharp
var index = treeList1.GetVisibleIndexByNode(e.Node);
if (index >= 0)
{
    e.Cache.DrawString((index + 1).ToString(), e.Appearance.Font,
        e.Appearance.GetForeBrush(e.Cache), e.Bounds,
        StringFormat.GenericDefault);
    e.Handled = true;
}
```

#### 3.2.9. TreeList1_CustomDrawNodeCell

```csharp
private void TreeList1_CustomDrawNodeCell(object sender, CustomDrawNodeCellEventArgs e)
```

**Chức năng:**
- Tô màu cell dựa trên số lượng đối tác và level

**Logic:**
```csharp
// Root categories (Level 0)
if (row.Level == 0)
{
    backColor = row.PartnerCount == 0 ? Color.LightGray : Color.LightBlue;
}
// Sub-categories (Level > 0)
else
{
    if (row.PartnerCount == 0)
        backColor = Color.FromArgb(245, 245, 245);
    else if (row.PartnerCount <= 5)
        backColor = Color.LightYellow;
    else if (row.PartnerCount <= 20)
        backColor = Color.LightGreen;
    else
        backColor = Color.LightCyan;
}
```

### 3.3. Methods

#### 3.3.1. Public Methods

**Không có public methods** (chỉ có constructor)

#### 3.3.2. Private Methods

##### LoadDataAsync()

```csharp
private async Task LoadDataAsync()
```

**Chức năng:**
- Tải dữ liệu với WaitForm hiển thị
- Guard tránh re-entrancy (`_isLoading` flag)

**Code Flow:**
```csharp
if (_isLoading) return;
_isLoading = true;
try
{
    await ExecuteWithWaitingFormAsync(async () => 
    { 
        await LoadDataAsyncWithoutSplash(); 
    });
}
finally
{
    _isLoading = false;
}
```

##### LoadDataAsyncWithoutSplash()

```csharp
private async Task LoadDataAsyncWithoutSplash()
```

**Chức năng:**
- Tải dữ liệu không hiển thị WaitForm
- Lấy categories và counts từ BLL
- Convert sang DTOs với hierarchy
- Bind vào TreeList

**Code Flow:**
```csharp
var (categories, counts) = await _businessPartnerCategoryBll.GetCategoriesWithCountsAsync();
var dtoList = categories.ToDtosWithHierarchy(counts).ToList();
BindGrid(dtoList);
```

##### BindGrid()

```csharp
private void BindGrid(List<BusinessPartnerCategoryDto> data)
```

**Chức năng:**
- Clear selection trước khi bind
- Bind data vào BindingSource
- Auto-fit columns
- Cấu hình multi-line display
- Clear selection sau khi bind

##### DeleteCategoriesInOrder()

```csharp
private async Task DeleteCategoriesInOrder(List<Guid> categoryIds)
```

**Chức năng:**
- Xóa các danh mục theo thứ tự: con trước, cha sau
- Tính level cho mỗi category
- Sắp xếp theo level (descending)
- Xóa từng category

**Code Flow:**
```csharp
var allCategories = await _businessPartnerCategoryBll.GetAllAsync();
var categoryDict = allCategories.ToDictionary(c => c.Id);

var categoriesToDelete = categoryIds.Select(id =>
{
    var category = categoryDict.TryGetValue(id, out var value) ? value : null;
    if (category == null) return null;
    var level = CalculateCategoryLevel(category, categoryDict);
    return new { Category = category, Level = level };
}).Where(x => x != null).OrderByDescending(x => x.Level).ToList();

foreach (var item in categoriesToDelete)
{
    _businessPartnerCategoryBll.Delete(item.Category.Id);
}
```

##### CalculateCategoryLevel()

```csharp
private int CalculateCategoryLevel(BusinessPartnerCategory category,
    Dictionary<Guid, BusinessPartnerCategory> categoryDict)
```

**Chức năng:**
- Tính level của category trong cây phân cấp
- Đếm số cấp từ root đến category hiện tại

##### UpdateSelectedCategoryIds()

```csharp
private void UpdateSelectedCategoryIds()
```

**Chức năng:**
- Cập nhật danh sách `_selectedCategoryIds`
- Quét tất cả nodes (checked hoặc selected)
- Thêm IDs vào danh sách

**Code Flow:**
```csharp
_selectedCategoryIds.Clear();
foreach (TreeListNode node in treeList1.Nodes)
{
    CheckNodeRecursive(node);
}
```

##### CheckNodeRecursive()

```csharp
private void CheckNodeRecursive(TreeListNode node)
```

**Chức năng:**
- Kiểm tra node và các child nodes một cách đệ quy
- Thêm ID vào `_selectedCategoryIds` nếu node được checked hoặc selected

##### UpdateButtonStates()

```csharp
private void UpdateButtonStates()
```

**Chức năng:**
- Cập nhật trạng thái enabled/disabled của các nút toolbar
- Edit: enabled khi chọn đúng 1 dòng
- Delete: enabled khi chọn >= 1 dòng
- Export: enabled khi có dữ liệu hiển thị

##### ClearSelectionState()

```csharp
private void ClearSelectionState()
```

**Chức năng:**
- Xóa tất cả selection (checkbox và regular selection)
- Clear `_selectedCategoryIds`
- Update button states

##### ConfigureMultiLineGridView()

```csharp
private void ConfigureMultiLineGridView()
```

**Chức năng:**
- Cấu hình TreeList để hiển thị text xuống dòng (word wrap)
- Áp dụng RepositoryItemMemoEdit cho các cột dài
- Căn giữa tiêu đề

##### SetupSuperToolTips()

```csharp
private void SetupSuperToolTips()
```

**Chức năng:**
- Thiết lập SuperToolTip cho tất cả các nút toolbar
- Sử dụng `SuperToolTipHelper.SetBarButtonSuperTip()`

### 3.4. Data Flow: Input → Validation → Business Logic → Output

```
┌─────────────────────────────────────────────────────────────┐
│                        INPUT                                 │
│  ┌──────────────────────────────────────────────────────┐   │
│  │  User clicks toolbar buttons                        │   │
│  │  User selects rows (checkbox/click)                 │   │
│  │  User exports to Excel                              │   │
│  └──────────────────────────────────────────────────────┘   │
└───────────────────────┬─────────────────────────────────────┘
                        │
                        ▼
┌─────────────────────────────────────────────────────────────┐
│                    VALIDATION                                │
│  ┌──────────────────────────────────────────────────────┐   │
│  │  Selection Validation:                               │   │
│  │  - Edit: must select exactly 1 row                  │   │
│  │  - Delete: must select >= 1 row                     │   │
│  │  - Export: must have data displayed                 │   │
│  │                                                      │   │
│  │  Business Validation (in Detail Form):             │   │
│  │  - CategoryName: Required, MaxLength(100)           │   │
│  │  - CategoryName: Unique check                       │   │
│  │  - Description: MaxLength(255)                     │   │
│  └──────────────────────────────────────────────────────┘   │
└───────────────────────┬─────────────────────────────────────┘
                        │
                        │ Valid?
                        ▼
┌─────────────────────────────────────────────────────────────┐
│                  BUSINESS LOGIC                              │
│  ┌──────────────────────────────────────────────────────┐   │
│  │  BusinessPartnerCategoryBll:                        │   │
│  │  - GetCategoriesWithCountsAsync()                   │   │
│  │  - GetAllAsync()                                    │   │
│  │  - Delete(id)                                      │   │
│  │                                                      │   │
│  │  BusinessPartnerCategoryConverters:                 │   │
│  │  - ToDtosWithHierarchy()                           │   │
│  │  - CalculateLevel()                                │   │
│  │  - CalculateFullPath()                             │   │
│  │  - SortHierarchical()                              │   │
│  └──────────────────────────────────────────────────────┘   │
└───────────────────────┬─────────────────────────────────────┘
                        │
                        ▼
┌─────────────────────────────────────────────────────────────┐
│                        OUTPUT                                │
│  ┌──────────────────────────────────────────────────────┐   │
│  │  Display hierarchical data on TreeList              │   │
│  │  Show success/error messages                         │   │
│  │  Export to Excel file                               │   │
│  │  Update button states                               │   │
│  └──────────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────────┘
```

---

## 4. Validation System

### 4.1. Selection Validation

| Operation | Validation Rule | Error Message |
|-----------|----------------|---------------|
| **Edit** | Must select exactly 1 row | "Vui lòng chọn một dòng để chỉnh sửa." hoặc "Chỉ cho phép chỉnh sửa 1 dòng. Vui lòng bỏ chọn bớt." |
| **Delete** | Must select >= 1 row | "Vui lòng chọn ít nhất một dòng để xóa." |
| **Export** | Must have data displayed | "Không có dữ liệu để xuất." |

### 4.2. Business Validation (in FrmBusinessPartnerCategoryDetail)

**File**: `MasterData/Customer/FrmBusinessPartnerCategoryDetail.cs`

| Field | Required | MaxLength | Unique | Other |
|-------|----------|-----------|--------|-------|
| `CategoryName` | ✅ Yes | 100 | ✅ Yes | - |
| `Description` | ❌ No | 255 | ❌ No | - |

**Validation Logic:**
```csharp
// CategoryName required
if (string.IsNullOrWhiteSpace(CategoryNameTextEdit?.Text))
{
    dxErrorProvider1.SetError(CategoryNameTextEdit, 
        "Tên phân loại không được để trống", ErrorType.Critical);
    return false;
}

// CategoryName max length
if (CategoryNameTextEdit.Text.Trim().Length > 100)
{
    dxErrorProvider1.SetError(CategoryNameTextEdit, 
        "Tên phân loại không được vượt quá 100 ký tự", ErrorType.Critical);
    return false;
}

// CategoryName unique check
if (_businessPartnerCategoryBll.IsCategoryNameExists(categoryName, _categoryId))
{
    dxErrorProvider1.SetError(CategoryNameTextEdit, 
        "Tên phân loại đã tồn tại trong hệ thống", ErrorType.Critical);
    return false;
}

// Description max length
if (!string.IsNullOrWhiteSpace(DescriptionMemoEdit?.Text) && 
    DescriptionMemoEdit.Text.Trim().Length > 255)
{
    dxErrorProvider1.SetError(DescriptionMemoEdit, 
        "Mô tả không được vượt quá 255 ký tự", ErrorType.Critical);
    return false;
}
```

### 4.3. DataAnnotations trong BusinessPartnerCategoryDto

| Property | Required | StringLength | Other |
|----------|----------|--------------|-------|
| `Id` | - | - | - |
| `CategoryName` | ✅ Yes | 100 | - |
| `Description` | ❌ No | 255 | - |
| `ParentId` | ❌ No | - | - |
| `PartnerCount` | - | - | - |
| `Level` | - | - | - |

### 4.4. Custom Validation

**Hiện tại không có custom validation logic** trong UcBusinessPartnerCategory. Tất cả validation được thực hiện trong `FrmBusinessPartnerCategoryDetail`.

---

## 5. Business Logic Flow

### 5.1. Load Data Flow (Sequence Diagram)

```
User                    UcBusinessPartnerCategory    BusinessPartnerCategoryBll    BusinessPartnerCategoryDataAccess    Database
 │                          │                               │                               │                          │
 │  ──Click "Danh sách"──>     │                               │                               │                          │
 │                          │                               │                               │                          │
 │                          │──LoadDataAsync()─────────────>│                               │                          │
 │                          │                               │                               │                          │
 │                          │──ExecuteWithWaitingFormAsync()│                               │                          │
 │                          │  └─> Show WaitForm           │                               │                          │
 │                          │                               │                               │                          │
 │                          │──GetCategoriesWithCountsAsync()─>│                               │                          │
 │                          │                               │──GetAllAsync()───────────────>│                          │
 │                          │                               │                               │──Query─────────────────>│
 │                          │                               │                               │<─Categories─────────────│
 │                          │                               │<──Categories──────────────────│                          │
 │                          │                               │                               │                          │
 │                          │                               │──GetPartnerCountByCategoryAsync()─>│                          │
 │                          │                               │                               │──Query─────────────────>│
 │                          │                               │                               │<─Counts─────────────────│
 │                          │                               │<──Counts──────────────────────│                          │
 │                          │<──(Categories, Counts)────────│                               │                          │
 │                          │                               │                               │                          │
 │                          │──ToDtosWithHierarchy()────────│                               │                          │
 │                          │  (Converter)                  │                               │                          │
 │                          │                               │                               │                          │
 │                          │──BindGrid()───────────────────│                               │                          │
 │                          │  └─> Update TreeList         │                               │                          │
 │                          │                               │                               │                          │
 │                          │──Close WaitForm───────────────│                               │                          │
 │                          │                               │                               │                          │
 │<───Data Displayed─────────│                               │                               │                          │
```

### 5.2. Delete Flow (Sequence Diagram)

```
User                    UcBusinessPartnerCategory    BusinessPartnerCategoryBll    BusinessPartnerCategoryDataAccess    Database
 │                          │                               │                               │                          │
 │  ──Select rows─────────> │                               │                               │                          │
 │  ──Click "Xóa"─────────> │                               │                               │                          │
 │                          │                               │                               │                          │
 │                          │──Check selection─────────────│                               │                          │
 │                          │──MsgBox.ShowYesNo()──────────│                               │                          │
 │<───Confirm Dialog─────────│                               │                               │                          │
 │  ──Click "Yes"─────────> │                               │                               │                          │
 │                          │                               │                               │                          │
 │                          │──ExecuteWithWaitingFormAsync()│                               │                          │
 │                          │  └─> Show WaitForm           │                               │                          │
 │                          │                               │                               │                          │
 │                          │──DeleteCategoriesInOrder()───│                               │                          │
 │                          │                               │──GetAllAsync()───────────────>│                          │
 │                          │                               │                               │──Query─────────────────>│
 │                          │                               │                               │<─All Categories─────────│
 │                          │                               │<──All Categories──────────────│                          │
 │                          │                               │                               │                          │
 │                          │──CalculateCategoryLevel()─────│                               │                          │
 │                          │──Sort by level (desc)─────────│                               │                          │
 │                          │                               │                               │                          │
 │                          │──Delete (for each)───────────>│                               │                          │
 │                          │                               │──DeleteCategory(id)──────────>│                          │
 │                          │                               │                               │──Delete─────────────────>│
 │                          │                               │                               │<──OK─────────────────────│
 │                          │                               │<──OK─────────────────────────│                          │
 │                          │<──OK──────────────────────────│                               │                          │
 │                          │                               │                               │                          │
 │                          │──LoadDataAsync()──────────────│                               │                          │
 │                          │  └─> Refresh TreeList        │                               │                          │
 │                          │                               │                               │                          │
 │                          │──Close WaitForm───────────────│                               │                          │
 │                          │                               │                               │                          │
 │<───Data Refreshed────────│                               │                               │                          │
```

### 5.3. Add/Edit Flow

```
User                    UcBusinessPartnerCategory    FrmBusinessPartnerCategoryDetail    BusinessPartnerCategoryBll    Database
 │                          │                               │                               │                          │
 │  ──Click "Mới"/"Sửa"──> │                               │                               │                          │
 │                          │                               │                               │                          │
 │                          │──OverlayManager.ShowScope()──│                               │                          │
 │                          │──new FrmBusinessPartner      │                               │                          │
 │                          │   CategoryDetail(id)────────>│                               │                          │
 │                          │                               │                               │                          │
 │                          │                               │──InitializeForm()─────────────│                          │
 │                          │                               │  └─> Load data if edit       │                          │
 │                          │                               │                               │                          │
 │                          │                               │<──User enters data───────────│                          │
 │                          │                               │──Validate()──────────────────│                          │
 │                          │                               │──Save()──────────────────────│                          │
 │                          │                               │                               │──SaveOrUpdate()─────────>│
 │                          │                               │                               │<──OK─────────────────────│
 │                          │                               │<──OK─────────────────────────│                          │
 │                          │                               │──DialogResult = OK───────────│                          │
 │                          │<──DialogResult = OK───────────│                               │                          │
 │                          │                               │                               │                          │
 │                          │──LoadDataAsync()──────────────│                               │                          │
 │                          │  └─> Refresh TreeList        │                               │                          │
 │                          │                               │                               │                          │
 │<───Data Refreshed─────────│                               │                               │                          │
```

---

## 6. Error Handling

### 6.1. Try-Catch Blocks

#### 6.1.1. LoadDataAsync

```csharp
try
{
    await ExecuteWithWaitingFormAsync(async () => 
    { 
        await LoadDataAsyncWithoutSplash(); 
    });
}
catch (Exception ex)
{
    ShowError(ex, "Lỗi tải dữ liệu");
}
finally
{
    _isLoading = false;
}
```

#### 6.1.2. NewBarButtonItem_ItemClick

```csharp
try
{
    using (OverlayManager.ShowScope(this))
    {
        using (var form = new FrmBusinessPartnerCategoryDetail(Guid.Empty))
        {
            form.ShowDialog(this);
            await LoadDataAsync();
            UpdateButtonStates();
        }
    }
}
catch (Exception ex)
{
    ShowError(ex, "Lỗi hiển thị màn hình thêm mới");
}
```

#### 6.1.3. EditBarButtonItem_ItemClick

```csharp
try
{
    // ... validation and form opening logic
}
catch (Exception ex)
{
    ShowError(ex, "Lỗi hiển thị màn hình điều chỉnh");
}
catch (Exception ex)
{
    MsgBox.ShowException(ex);
}
```

#### 6.1.4. DeleteBarButtonItem_ItemClick

```csharp
try
{
    // ... validation and deletion logic
}
catch (Exception ex)
{
    ShowError(ex, "Lỗi xóa dữ liệu");
}
catch (Exception ex)
{
    MsgBox.ShowException(ex);
}
```

#### 6.1.5. ExportBarButtonItem_ItemClick

```csharp
try
{
    // ... export logic
}
catch (Exception ex)
{
    ShowError(ex, "Lỗi xuất dữ liệu");
}
```

### 6.2. Error Display Methods

#### ShowInfo()

```csharp
private void ShowInfo(string message)
{
    MsgBox.ShowSuccess(message);
}
```

#### ShowError()

```csharp
private void ShowError(Exception ex, string context = null)
{
    MsgBox.ShowException(string.IsNullOrWhiteSpace(context)
        ? ex
        : new Exception(context + ": " + ex.Message, ex));
}
```

### 6.3. Error Handling Best Practices

✅ **Đã áp dụng:**
- Try-catch cho tất cả operations
- User-friendly error messages
- Context information trong error messages
- Guard flags để tránh re-entrancy (`_isLoading`)

---

## 7. Security & Best Practices

### 7.1. Security

**Hiện tại không có thông tin nhạy cảm** được xử lý trong UcBusinessPartnerCategory:

- Không có authentication/authorization logic
- Quyền truy cập được quản lý bởi hệ thống phân quyền của ERP
- Dữ liệu được lưu trữ trong database với các ràng buộc an toàn

### 7.2. Best Practices

#### 7.2.1. Async/Await

✅ **Đã áp dụng:**
- Sử dụng async/await cho tất cả database operations
- WaitForm hiển thị trong khi tải dữ liệu
- Guard flag (`_isLoading`) để tránh re-entrancy

#### 7.2.2. Error Handling

✅ **Đã áp dụng:**
- Try-catch cho tất cả operations
- User-friendly error messages
- Context information trong error messages

#### 7.2.3. Separation of Concerns

✅ **Đã áp dụng:**
- UI Layer (UcBusinessPartnerCategory) → BLL Layer (BusinessPartnerCategoryBll) → DAL Layer (BusinessPartnerCategoryDataAccess)
- DTO pattern (BusinessPartnerCategoryDto)
- Converter pattern (BusinessPartnerCategoryConverters)

#### 7.2.4. Code Organization

✅ **Đã áp dụng:**
- Regions để tổ chức code
- XML documentation comments
- Meaningful method names
- Guard clauses để tránh invalid operations

### 7.3. Gợi Ý Cải Thiện

#### 7.3.1. Search/Filter Functionality

**Gợi ý thêm search box:**
```csharp
private void SearchTextEdit_TextChanged(object sender, EventArgs e)
{
    var searchText = SearchTextEdit.Text?.ToLower();
    if (string.IsNullOrWhiteSpace(searchText))
    {
        // Show all
        businessPartnerCategoryDtoBindingSource.Filter = null;
    }
    else
    {
        businessPartnerCategoryDtoBindingSource.Filter = 
            $"CategoryName LIKE '%{searchText}%' OR Description LIKE '%{searchText}%'";
    }
}
```

#### 7.3.2. Batch Operations

**Gợi ý thêm batch operations:**
```csharp
private async void BatchUpdateBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
{
    if (_selectedCategoryIds.Count == 0)
    {
        ShowInfo("Vui lòng chọn ít nhất một dòng để cập nhật hàng loạt.");
        return;
    }
    
    // Open batch update form
    using (var form = new FrmBatchUpdateCategory(_selectedCategoryIds))
    {
        form.ShowDialog(this);
        await LoadDataAsync();
    }
}
```

#### 7.3.3. Import from Excel

**Gợi ý thêm import functionality:**
```csharp
private async void ImportBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
{
    using (var openDialog = new OpenFileDialog())
    {
        openDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
        if (openDialog.ShowDialog() == DialogResult.OK)
        {
            try
            {
                await ImportFromExcel(openDialog.FileName);
                await LoadDataAsync();
                ShowInfo("Import dữ liệu thành công!");
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi import dữ liệu");
            }
        }
    }
}
```

---

## 8. Extensibility Guide

### 8.1. Cách Mở Rộng Form

#### 8.1.1. Thêm Cột Mới

**Bước 1**: Thêm property vào `BusinessPartnerCategoryDto.cs`
```csharp
[DisplayName("Ghi chú")]
[StringLength(500)]
public string Notes { get; set; }
```

**Bước 2**: Thêm column vào `UcBusinessPartnerCategory.Designer.cs`
```csharp
private TreeListColumn colNotes;
```

**Bước 3**: Cấu hình column trong `InitializeComponent()`
```csharp
this.colNotes.FieldName = "Notes";
this.colNotes.Name = "colNotes";
this.colNotes.Visible = true;
this.colNotes.VisibleIndex = 3;
```

**Bước 4**: Cập nhật converter nếu cần
```csharp
// In BusinessPartnerCategoryConverters.cs
dto.Notes = entity.Notes;
```

#### 8.1.2. Thêm Chức Năng Tìm Kiếm

**Bước 1**: Thêm SearchTextEdit vào Designer
```csharp
private TextEdit SearchTextEdit;
```

**Bước 2**: Thêm event handler
```csharp
SearchTextEdit.TextChanged += SearchTextEdit_TextChanged;
```

**Bước 3**: Implement filter logic
```csharp
private void SearchTextEdit_TextChanged(object sender, EventArgs e)
{
    var searchText = SearchTextEdit.Text?.ToLower();
    if (string.IsNullOrWhiteSpace(searchText))
    {
        businessPartnerCategoryDtoBindingSource.Filter = null;
    }
    else
    {
        businessPartnerCategoryDtoBindingSource.Filter = 
            $"CategoryName LIKE '%{searchText}%' OR Description LIKE '%{searchText}%'";
    }
}
```

#### 8.1.3. Thêm Chức Năng Sắp Xếp

**Bước 1**: Enable sorting trong TreeList
```csharp
treeList1.OptionsView.ShowAutoFilterRow = true;
treeList1.OptionsBehavior.EnableFiltering = true;
```

**Bước 2**: Configure columns for sorting
```csharp
colCategoryName.SortOrder = SortOrder.Ascending;
colPartnerCount.SortOrder = SortOrder.Descending;
```

### 8.2. Clean Code Patterns

#### 8.2.1. Repository Pattern

**Hiện tại**: Sử dụng trực tiếp `BusinessPartnerCategoryDataAccess`

**Gợi ý**: Tạo interface `IBusinessPartnerCategoryRepository`:
```csharp
public interface IBusinessPartnerCategoryRepository
{
    Task<List<BusinessPartnerCategory>> GetAllAsync();
    Task<Dictionary<Guid, int>> GetPartnerCountByCategoryAsync();
    void DeleteCategory(Guid id);
}
```

#### 8.2.2. Dependency Injection

**Gợi ý**: Sử dụng DI container:
```csharp
public UcBusinessPartnerCategory(IBusinessPartnerCategoryBll categoryBll)
{
    InitializeComponent();
    _businessPartnerCategoryBll = categoryBll;
    // ... rest of initialization
}
```

#### 8.2.3. Command Pattern

**Gợi ý**: Tách logic thành commands:
```csharp
public interface ICommand
{
    Task ExecuteAsync();
    bool CanExecute();
}

public class DeleteCategoryCommand : ICommand
{
    private readonly UcBusinessPartnerCategory _uc;
    private readonly List<Guid> _categoryIds;
    
    public async Task ExecuteAsync()
    {
        await _uc.DeleteCategoriesInOrder(_categoryIds);
    }
}
```

---

## 9. Test Checklist

### 9.1. Unit Test Cases

#### 9.1.1. Load Tests

- [ ] Test `LoadDataAsync` với dữ liệu tồn tại
- [ ] Test `LoadDataAsync` với dữ liệu rỗng
- [ ] Test `LoadDataAsync` với lỗi database
- [ ] Test `LoadDataAsync` re-entrancy guard (`_isLoading`)
- [ ] Test `BindGrid` với dữ liệu hợp lệ
- [ ] Test `BindGrid` clear selection

#### 9.1.2. Selection Tests

- [ ] Test `UpdateSelectedCategoryIds` với checkbox selection
- [ ] Test `UpdateSelectedCategoryIds` với regular selection
- [ ] Test `UpdateSelectedCategoryIds` với mixed selection
- [ ] Test `CheckNodeRecursive` với nested nodes
- [ ] Test `ClearSelectionState` clear tất cả selection

#### 9.1.3. Delete Tests

- [ ] Test `DeleteCategoriesInOrder` với 1 category
- [ ] Test `DeleteCategoriesInOrder` với nhiều categories (parent-child)
- [ ] Test `DeleteCategoriesInOrder` với thứ tự xóa đúng (con trước, cha sau)
- [ ] Test `CalculateCategoryLevel` với các level khác nhau
- [ ] Test `DeleteCategoriesInOrder` với lỗi xóa một category

#### 9.1.4. Export Tests

- [ ] Test `ExportBarButtonItem_ItemClick` với dữ liệu tồn tại
- [ ] Test `ExportBarButtonItem_ItemClick` với dữ liệu rỗng
- [ ] Test `ExportBarButtonItem_ItemClick` với lỗi ghi file

#### 9.1.5. Button State Tests

- [ ] Test `UpdateButtonStates` với 0 selection
- [ ] Test `UpdateButtonStates` với 1 selection
- [ ] Test `UpdateButtonStates` với nhiều selections
- [ ] Test `UpdateButtonStates` với dữ liệu rỗng

### 9.2. Manual Testing Scenarios

#### 9.2.1. Scenario 1: Load Data

**Steps:**
1. Mở form UcBusinessPartnerCategory
2. Click nút "Danh sách"
3. Kiểm tra WaitForm hiển thị
4. Kiểm tra dữ liệu load thành công
5. Kiểm tra TreeList hiển thị đúng cấu trúc cây

**Expected:**
- WaitForm hiển thị trong khi tải
- Dữ liệu hiển thị đúng
- Cấu trúc cây đúng (parent-child)
- Màu sắc phân biệt đúng

#### 9.2.2. Scenario 2: Add New Category

**Steps:**
1. Click nút "Mới"
2. Nhập thông tin danh mục
3. Click "Lưu"
4. Kiểm tra form đóng
5. Kiểm tra danh sách refresh

**Expected:**
- Form mở với overlay
- Form đóng sau khi lưu
- Danh sách tự động refresh
- Danh mục mới xuất hiện trong danh sách

#### 9.2.3. Scenario 3: Edit Category

**Steps:**
1. Chọn 1 dòng danh mục
2. Click nút "Điều chỉnh"
3. Chỉnh sửa thông tin
4. Click "Lưu"
5. Kiểm tra danh sách refresh

**Expected:**
- Form mở với dữ liệu đã có
- Form đóng sau khi lưu
- Danh sách tự động refresh
- Thông tin được cập nhật

#### 9.2.4. Scenario 4: Delete Single Category

**Steps:**
1. Chọn 1 dòng danh mục
2. Click nút "Xóa"
3. Xác nhận xóa
4. Kiểm tra danh sách refresh

**Expected:**
- Hộp thoại xác nhận hiển thị
- Danh mục bị xóa sau khi xác nhận
- Danh sách tự động refresh

#### 9.2.5. Scenario 5: Delete Multiple Categories

**Steps:**
1. Chọn nhiều dòng danh mục (checkbox)
2. Click nút "Xóa"
3. Xác nhận xóa
4. Kiểm tra danh sách refresh

**Expected:**
- Hộp thoại xác nhận hiển thị số lượng
- Tất cả danh mục được xóa
- Xóa theo thứ tự đúng (con trước, cha sau)
- Danh sách tự động refresh

#### 9.2.6. Scenario 6: Export to Excel

**Steps:**
1. Đảm bảo có dữ liệu hiển thị
2. Click nút "Xuất"
3. Chọn vị trí lưu file
4. Click "Lưu"
5. Kiểm tra file Excel

**Expected:**
- SaveFileDialog mở ra
- File Excel được tạo
- File chứa đúng dữ liệu
- Thông báo thành công hiển thị

#### 9.2.7. Scenario 7: Selection Validation

**Steps:**
1. Không chọn dòng nào, click "Điều chỉnh"
2. Chọn nhiều dòng, click "Điều chỉnh"
3. Không chọn dòng nào, click "Xóa"
4. Không có dữ liệu, click "Xuất"

**Expected:**
- Thông báo lỗi phù hợp cho mỗi trường hợp
- Các nút không thực hiện action khi không hợp lệ

---

## 10. Changelog Template

### 10.1. Format

```markdown
## [Version] - YYYY-MM-DD

### Added
- Tính năng mới 1
- Tính năng mới 2

### Changed
- Thay đổi 1
- Thay đổi 2

### Fixed
- Sửa lỗi 1
- Sửa lỗi 2

### Removed
- Xóa tính năng 1
- Xóa tính năng 2
```

### 10.2. Example

```markdown
## [1.0.0] - 2025-01-XX

### Added
- User Control UcBusinessPartnerCategory để quản lý danh mục đối tác
- Hiển thị dạng cây phân cấp (hierarchical TreeList)
- Chức năng thêm mới, sửa, xóa danh mục
- Chức năng xuất dữ liệu ra Excel
- Multi-select support với checkbox
- Màu sắc phân biệt theo số lượng đối tác và level
- Custom draw row numbers và colors
- Async operations với splash screen
- Tooltips hướng dẫn
- Tự động refresh sau khi thay đổi

### Changed
- (Chưa có)

### Fixed
- (Chưa có)

### Removed
- (Chưa có)

### Known Issues
- Chưa có chức năng tìm kiếm/filter trực tiếp trên màn hình
- Chưa có chức năng import từ Excel
```

---

## 11. Additional Notes

### 11.1. Missing Features

⚠️ **Chưa có chức năng tìm kiếm/filter:**
- Không có search box để tìm kiếm danh mục
- Không có filter options

⚠️ **Chưa có chức năng import:**
- Không có chức năng import từ Excel

### 11.2. Future Enhancements

- [ ] Add search/filter functionality
- [ ] Add import from Excel
- [ ] Add batch operations (batch update, batch delete)
- [ ] Add drag & drop để sắp xếp lại danh mục
- [ ] Add context menu cho TreeList
- [ ] Add print functionality
- [ ] Add export to PDF
- [ ] Add column customization (show/hide columns)

---

**Tài liệu này được tạo tự động từ source code. Cập nhật lần cuối: 2025-01-XX**

