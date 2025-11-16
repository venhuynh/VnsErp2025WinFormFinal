# Tài Liệu Kỹ Thuật - UcCompanyBranch (User Control Quản Lý Chi Nhánh Công Ty)

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

**UcCompanyBranch** là User Control thuộc module **MasterData.Company**, có vai trò:

- **Quản lý danh sách chi nhánh công ty** trong hệ thống ERP
- **Hiển thị danh sách** chi nhánh trong GridView với đầy đủ thông tin
- **CRUD operations**: Create (thêm mới), Read (xem danh sách), Update (chỉnh sửa), Delete (xóa)
- **Business rules enforcement**: Đảm bảo công ty luôn có ít nhất một chi nhánh
- **Data export**: Xuất dữ liệu ra file Excel

### 1.2. File Structure

```
MasterData/Company/
├── UcCompanyBranch.cs                    # Main code-behind file
├── UcCompanyBranch.Designer.cs           # Designer-generated code
└── UcCompanyBranch.resx                 # Resources file (nếu có)

MasterData/Company/Dto/
└── CompanyBranchDto.cs                  # Data Transfer Object

MasterData/Company/Converters/
└── CompanyBranchConverters.cs            # Entity ↔ DTO converter

MasterData/Company/
└── FrmCompanyBranchDetail.cs            # Form chi tiết chi nhánh

Bll/MasterData/Company/
└── CompanyBranchBll.cs                  # Business Logic Layer

Dal/DataAccess/MasterData/CompanyDal/
└── CompanyBranchDataAccess.cs           # Data Access Layer

Bll/Utils/
└── SuperToolTipHelper.cs                # Helper tạo SuperToolTip

Bll/Common/
├── MsgBox.cs                            # Message box helper
└── OverlayManager.cs                    # Overlay manager helper
```

### 1.3. Dependencies

**DevExpress Controls:**
- `XtraUserControl` - Base class
- `GridControl`, `GridView` - Grid display
- `BarManager`, `BarButtonItem`, `BarStaticItem` - Toolbar và status bar
- `LayoutControl` - Layout container
- `BindingSource` - Data binding
- `SplashScreenManager` - WaitForm
- `OverlayManager` - Overlay khi mở form

**Internal Dependencies:**
- `Bll.MasterData.Company.CompanyBranchBll` - Business logic
- `Dal.DataAccess.MasterData.CompanyDal.CompanyBranchDataAccess` - Data access
- `MasterData.Company.Dto.CompanyBranchDto` - DTO
- `MasterData.Company.Converters.CompanyBranchConverters` - Converter
- `Bll.Utils.SuperToolTipHelper` - Tooltip helper
- `Bll.Common.MsgBox` - Message box helper
- `Bll.Common.OverlayManager` - Overlay helper

---

## 2. Architecture

### 2.1. Layer Architecture

```
┌─────────────────────────────────────────────────────────┐
│                    UI Layer (UcCompanyBranch)           │
│  ┌───────────────────────────────────────────────────┐   │
│  │  - XtraUserControl                               │   │
│  │  - GridControl/GridView (Display)                │   │
│  │  - BarManager (Toolbar)                          │   │
│  │  - Event Handlers                                │   │
│  │  - Selection Management                          │   │
│  └───────────────────────────────────────────────────┘   │
└───────────────────────┬─────────────────────────────────┘
                        │
                        │ Uses
                        ▼
┌─────────────────────────────────────────────────────────┐
│        Business Logic Layer (CompanyBranchBll)          │
│  ┌───────────────────────────────────────────────────┐   │
│  │  - GetAllAsync()                                  │   │
│  │  - Delete(Guid id)                                │   │
│  └───────────────────────────────────────────────────┘   │
└───────────────────────┬─────────────────────────────────┘
                        │
                        │ Uses
                        ▼
┌─────────────────────────────────────────────────────────┐
│    Data Access Layer (CompanyBranchDataAccess)          │
│  ┌───────────────────────────────────────────────────┐   │
│  │  - GetAllAsync()                                  │   │
│  │  - Delete(CompanyBranch entity)                   │   │
│  └───────────────────────────────────────────────────┘   │
└───────────────────────┬─────────────────────────────────┘
                        │
                        │ Uses
                        ▼
┌─────────────────────────────────────────────────────────┐
│              Database (LINQ to SQL)                    │
│  ┌───────────────────────────────────────────────────┐   │
│  │  - CompanyBranch Table                           │   │
│  └───────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────┘
```

### 2.2. Data Flow Diagram

```
┌──────────────┐
│   Database   │
│ (CompanyBranch)│
└──────┬───────┘
       │
       │ Read All
       ▼
┌──────────────────┐
│CompanyBranchDataAccess│
│  GetAllAsync()   │
└──────┬───────────┘
       │
       │ Returns List<CompanyBranch>
       ▼
┌──────────────────┐
│   CompanyBranchBll│
│  GetAllAsync()   │
└──────┬───────────┘
       │
       │ Returns List<CompanyBranch>
       ▼
┌──────────────────┐
│  UcCompanyBranch │
│  LoadDataAsync() │
└──────┬───────────┘
       │
       │ Converts Entity → DTO
       ▼
┌──────────────────┐
│CompanyBranchConverters│
│    ToDto()       │
└──────┬───────────┘
       │
       │ Displays List<CompanyBranchDto> on GridView
       ▼
┌──────────────────┐
│   GridView       │
│  (Data Binding)  │
└──────────────────┘
```

### 2.3. Component Diagram (ASCII)

```
┌─────────────────────────────────────────────────────────────┐
│                    UcCompanyBranch                         │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│  ┌────────────────────────────────────────────────────┐     │
│  │          BarManager (Toolbar)                      │     │
│  │  ┌──────────────────────────────────────────────┐ │     │
│  │  │  ListDataBarButtonItem (Tải dữ liệu)          │ │     │
│  │  │  NewBarButtonItem (Thêm mới)                  │ │     │
│  │  │  EditBarButtonItem (Sửa)                       │ │     │
│  │  │  DeleteBarButtonItem (Xóa)                     │ │     │
│  │  │  ExportBarButtonItem (Xuất Excel)              │ │     │
│  │  └──────────────────────────────────────────────┘ │     │
│  └────────────────────────────────────────────────────┘     │
│                                                              │
│  ┌────────────────────────────────────────────────────┐     │
│  │          Status Bar (bar1)                          │     │
│  │  ┌──────────────────────────────────────────────┐ │     │
│  │  │  DataSummaryBarStaticItem (Tổng kết)         │ │     │
│  │  │  SelectedRowBarStaticItem (Đang chọn)        │ │     │
│  │  └──────────────────────────────────────────────┘ │     │
│  └────────────────────────────────────────────────────┘     │
│                                                              │
│  ┌────────────────────────────────────────────────────┐     │
│  │          GridControl/GridView                       │     │
│  │  ┌──────────────────────────────────────────────┐ │     │
│  │  │  colBranchCode                                │ │     │
│  │  │  colBranchName                                │ │     │
│  │  │  colAddress                                   │ │     │
│  │  │  colPhone                                     │ │     │
│  │  │  colEmail                                     │ │     │
│  │  │  colManagerName                               │ │     │
│  │  │  colIsActive                                  │ │     │
│  │  └──────────────────────────────────────────────┘ │     │
│  │  - Multi-select (Checkbox)                        │     │
│  │  - Auto Filter Row                                │     │
│  │  - Find Panel                                     │     │
│  │  - Row Styling                                    │     │
│  └────────────────────────────────────────────────────┘     │
│                                                              │
│  ┌────────────────────────────────────────────────────┐     │
│  │          BindingSource                             │     │
│  │  - companyBranchDtoBindingSource                   │     │
│  └────────────────────────────────────────────────────┘     │
│                                                              │
│  ┌────────────────────────────────────────────────────┐     │
│  │          CompanyBranchBll                          │     │
│  └────────────────────────────────────────────────────┘     │
│                                                              │
└─────────────────────────────────────────────────────────────┘
```

---

## 3. Detailed Technical Breakdown

### 3.1. Controls trong Designer

#### 3.1.1. LayoutControl

```csharp
private LayoutControl layoutControl1;
```

- **Vị trí**: Dock = Fill
- **Chức năng**: Container chứa GridControl
- **Root**: `Root` (LayoutControlGroup)

#### 3.1.2. GridControl & GridView

```csharp
private GridControl CompanyBranchGridControl;
private GridView CompanyBranchGridView;
```

**Cấu hình GridView:**
- `OptionsSelection.MultiSelect = true` - Cho phép chọn nhiều dòng
- `OptionsSelection.MultiSelectMode = CheckBoxRowSelect` - Chọn bằng checkbox
- `OptionsFind.AlwaysVisible = true` - Find panel luôn hiển thị
- `OptionsView.ShowAutoFilterRow = true` - Hiển thị dòng lọc tự động
- `OptionsView.ShowGroupPanel = false` - Tắt group panel
- `OptionsView.ShowViewCaption = true` - Hiển thị caption
- `ViewCaption = "BẢNG DANH SÁCH CÁC CHI NHÁNH"` - Caption của view
- `IndicatorWidth = 40` - Độ rộng cột số thứ tự

**Columns:**
| Column Name | Field Name | Visible | VisibleIndex |
|-------------|------------|---------|--------------|
| `colBranchCode` | BranchCode | true | 1 |
| `colBranchName` | BranchName | true | 2 |
| `colAddress` | Address | true | 3 |
| `colPhone` | Phone | true | 4 |
| `colEmail` | Email | true | 5 |
| `colManagerName` | ManagerName | true | 6 |
| `colIsActive` | IsActive | true | 7 |

#### 3.1.3. BindingSource

```csharp
private BindingSource companyBranchDtoBindingSource;
```

- **DataSource Type**: `CompanyBranchDto`
- **Chức năng**: Bind dữ liệu từ List<CompanyBranchDto> vào GridView

#### 3.1.4. BarManager & Toolbar

**BarManager:**
```csharp
private BarManager barManager1;
```

**Main Menu Bar (bar2):**
- `ListDataBarButtonItem` - Tải dữ liệu
- `NewBarButtonItem` - Thêm mới
- `EditBarButtonItem` - Điều chỉnh
- `DeleteBarButtonItem` - Xóa
- `ExportBarButtonItem` - Xuất

**Status Bar (bar1):**
- `DataSummaryBarStaticItem` - Tổng kết
- `SelectedRowBarStaticItem` - Đang chọn

### 3.2. Event Handlers

#### 3.2.1. ListDataBarButtonItem_ItemClick

```csharp
private async void ListDataBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
```

**Chức năng:**
- Tải lại dữ liệu từ database
- Hiển thị WaitForm trong quá trình tải
- Cập nhật GridView với dữ liệu mới nhất

**Flow:**
```
Click "Danh sách"
  ├─> LoadDataAsync()
  │   ├─> Check _isLoading guard
  │   ├─> ExecuteWithWaitingFormAsync()
  │   │   ├─> Show WaitForm1
  │   │   ├─> LoadDataAsyncWithoutSplash()
  │   │   │   ├─> CompanyBranchBll.GetAllAsync()
  │   │   │   ├─> Convert Entity → DTO
  │   │   │   ├─> BindGrid(dtoList)
  │   │   │   └─> UpdateDataSummary()
  │   │   └─> Close WaitForm1
  └─> UpdateButtonStates()
```

#### 3.2.2. NewBarButtonItem_ItemClick

```csharp
private async void NewBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
```

**Chức năng:**
- Mở form thêm mới chi nhánh (FrmCompanyBranchDetail với Guid.Empty)
- Hiển thị overlay trên UserControl
- Tự động tải lại dữ liệu sau khi đóng form

**Flow:**
```
Click "Mới"
  ├─> Show OverlayManager
  ├─> new FrmCompanyBranchDetail(Guid.Empty)
  ├─> form.ShowDialog()
  ├─> LoadDataAsync() (after dialog closes)
  ├─> UpdateButtonStates()
  └─> Close OverlayManager
```

#### 3.2.3. EditBarButtonItem_ItemClick

```csharp
private async void EditBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
```

**Chức năng:**
- Mở form chỉnh sửa chi nhánh đã chọn
- Validate: Phải chọn đúng 1 dòng
- Hiển thị overlay trên UserControl
- Tự động tải lại dữ liệu sau khi đóng form

**Flow:**
```
Click "Điều chỉnh"
  ├─> Check selection: Must be exactly 1 row
  ├─> Get selected ID from _selectedBranchIds[0]
  ├─> Show OverlayManager
  ├─> new FrmCompanyBranchDetail(id)
  ├─> form.ShowDialog()
  ├─> LoadDataAsync() (after dialog closes)
  ├─> UpdateButtonStates()
  └─> Close OverlayManager
```

#### 3.2.4. DeleteBarButtonItem_ItemClick

```csharp
private async void DeleteBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
```

**Chức năng:**
- Xóa một hoặc nhiều chi nhánh đã chọn
- Validate business rules trước khi xóa
- Hiển thị dialog xác nhận
- Tự động tải lại dữ liệu sau khi xóa

**Flow:**
```
Click "Xóa"
  ├─> Check selection: Must be at least 1 row
  ├─> ValidateDeleteBusinessRules()
  │   ├─> Check: totalBranches - selectedBranches > 0
  │   └─> Check: Not deleting last branch
  ├─> Show confirmation dialog (Yes/No)
  ├─> If Yes:
  │   ├─> Show WaitForm1
  │   ├─> For each selected ID:
  │   │   └─> CompanyBranchBll.Delete(id)
  │   ├─> LoadDataAsync()
  │   └─> Close WaitForm1
  └─> UpdateButtonStates()
```

#### 3.2.5. ExportBarButtonItem_ItemClick

```csharp
private void ExportBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
```

**Chức năng:**
- Xuất toàn bộ dữ liệu trong GridView ra file Excel
- Hiển thị SaveFileDialog để chọn vị trí lưu

**Flow:**
```
Click "Xuất"
  ├─> Check rowCount > 0
  ├─> Show SaveFileDialog
  │   ├─> Filter: "Excel Files (*.xlsx)|*.xlsx"
  │   └─> DefaultFileName: "CompanyBranches.xlsx"
  ├─> If user selects location:
  │   ├─> GridView.ExportToXlsx(fileName)
  │   └─> Show success message
  └─> UpdateButtonStates()
```

#### 3.2.6. CompanyBranchGridView_SelectionChanged

```csharp
private void CompanyBranchGridView_SelectionChanged(object sender, EventArgs e)
```

**Chức năng:**
- Cập nhật danh sách selected branch IDs
- Cập nhật trạng thái các nút toolbar
- Cập nhật thống kê

**Flow:**
```
Selection Changed
  ├─> UpdateSelectedBranchIds()
  │   ├─> Clear _selectedBranchIds
  │   ├─> Get selected rows from GridView
  │   └─> Add IDs to _selectedBranchIds
  ├─> UpdateButtonStates()
  └─> UpdateDataSummary()
```

#### 3.2.7. CompanyBranchGridView_DoubleClick

```csharp
private async void CompanyBranchGridView_DoubleClick(object sender, EventArgs e)
```

**Chức năng:**
- Mở form chi tiết khi double-click vào dòng
- Validate: Phải chọn đúng 1 dòng
- Tương tự như EditBarButtonItem_ItemClick

#### 3.2.8. CompanyBranchGridView_CustomDrawRowIndicator

```csharp
private void CompanyBranchGridView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
```

**Chức năng:**
- Vẽ số thứ tự dòng (1, 2, 3, ...) trong cột indicator

**Code:**
```csharp
if (e.Info.IsRowIndicator && e.RowHandle >= 0)
{
    e.Info.DisplayText = (e.RowHandle + 1).ToString();
}
```

#### 3.2.9. CompanyBranchGridView_RowCellStyle

```csharp
private void CompanyBranchGridView_RowCellStyle(object sender, RowCellStyleEventArgs e)
```

**Chức năng:**
- Tô màu dòng có IsActive = False với màu đỏ

**Code:**
```csharp
var row = gridView.GetRow(e.RowHandle) as CompanyBranchDto;
if (row != null && !row.IsActive)
{
    e.Appearance.ForeColor = Color.Red;
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
- Guard tránh re-entrancy (_isLoading)

**Code Flow:**
```csharp
if (_isLoading) return; // Guard
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
- Gọi CompanyBranchBll.GetAllAsync()
- Convert Entity → DTO
- Bind vào GridView

##### BindGrid()

```csharp
private void BindGrid(List<CompanyBranchDto> data)
```

**Chức năng:**
- Clear selection trước khi bind
- Bind dữ liệu vào BindingSource
- Auto-fit columns
- Clear selection sau khi bind

##### UpdateSelectedBranchIds()

```csharp
private void UpdateSelectedBranchIds()
```

**Chức năng:**
- Cập nhật danh sách _selectedBranchIds từ GridView selection
- Lấy IDs từ các dòng đã chọn

##### ClearSelectionState()

```csharp
private void ClearSelectionState()
```

**Chức năng:**
- Xóa selection trên GridView
- Clear _selectedBranchIds
- Reset focused row

##### ValidateDeleteBusinessRules()

```csharp
private async Task<bool> ValidateDeleteBusinessRules()
```

**Chức năng:**
- Validate business rules trước khi xóa
- Kiểm tra: Không cho phép xóa nếu sẽ không còn chi nhánh nào
- Kiểm tra: Không cho phép xóa chi nhánh cuối cùng

**Business Rules:**
```csharp
// Rule 1: totalBranches - selectedBranches > 0
if (totalBranches - selectedBranches <= 0)
    return false;

// Rule 2: Not deleting last branch
if (totalBranches == 1 && selectedBranches == 1)
    return false;
```

##### ExecuteWithWaitingFormAsync()

```csharp
private async Task ExecuteWithWaitingFormAsync(Func<Task> operation)
```

**Chức năng:**
- Thực hiện operation async với WaitForm hiển thị
- Show WaitForm1 trước khi thực hiện
- Close WaitForm1 sau khi hoàn thành

##### UpdateButtonStates()

```csharp
private void UpdateButtonStates()
```

**Chức năng:**
- Cập nhật trạng thái enabled/disabled của các nút toolbar
- Edit: Enabled khi selectedCount == 1
- Delete: Enabled khi selectedCount >= 1
- Export: Enabled khi rowCount > 0

##### UpdateDataSummary()

```csharp
private void UpdateDataSummary()
```

**Chức năng:**
- Cập nhật thông tin tổng kết ở status bar
- DataSummaryBarStaticItem: "Tổng: X chi nhánh"
- SelectedRowBarStaticItem: "Đã chọn: X dòng" hoặc "Chưa chọn dòng nào"

##### SetupSuperTips()

```csharp
private void SetupSuperTips()
```

**Chức năng:**
- Setup SuperToolTips cho tất cả các controls
- Gọi SetupBarButtonSuperTips()

##### SetupBarButtonSuperTips()

```csharp
private void SetupBarButtonSuperTips()
```

**Chức năng:**
- Tạo SuperToolTip cho từng BarButtonItem
- Sử dụng SuperToolTipHelper.SetBarButtonSuperTip()
- Mỗi tooltip có title (HTML) và content (HTML) mô tả chi tiết

##### ShowInfo()

```csharp
private void ShowInfo(string message)
```

**Chức năng:**
- Hiển thị thông báo thông tin
- Sử dụng MsgBox.ShowSuccess()

##### ShowError()

```csharp
private void ShowError(Exception ex, string context = null)
```

**Chức năng:**
- Hiển thị thông báo lỗi
- Sử dụng MsgBox.ShowException()

### 3.4. Data Flow: Input → Validation → Business Logic → Output

```
┌─────────────────────────────────────────────────────────────┐
│                        INPUT                                 │
│  ┌──────────────────────────────────────────────────────┐   │
│  │  User clicks toolbar buttons                          │   │
│  │  User selects rows in GridView                        │   │
│  │  User double-clicks row                               │   │
│  └──────────────────────────────────────────────────────┘   │
└───────────────────────┬─────────────────────────────────────┘
                        │
                        ▼
┌─────────────────────────────────────────────────────────────┐
│                    VALIDATION                               │
│  ┌──────────────────────────────────────────────────────┐   │
│  │  Selection Validation:                               │   │
│  │  - Edit: Must select exactly 1 row                  │   │
│  │  - Delete: Must select at least 1 row                │   │
│  │                                                      │   │
│  │  Business Rules Validation:                        │   │
│  │  - Delete: Cannot delete all branches                │   │
│  │  - Delete: Cannot delete last branch                 │   │
│  │                                                      │   │
│  │  Data Validation:                                    │   │
│  │  - Export: Must have at least 1 row                  │   │
│  └──────────────────────────────────────────────────────┘   │
└───────────────────────┬─────────────────────────────────────┘
                        │
                        │ Valid?
                        ▼
┌─────────────────────────────────────────────────────────────┐
│                  BUSINESS LOGIC                              │
│  ┌──────────────────────────────────────────────────────┐   │
│  │  CompanyBranchBll:                                   │   │
│  │  - GetAllAsync() → Returns List<CompanyBranch>       │   │
│  │  - Delete(Guid id) → Deletes branch                  │   │
│  │                                                      │   │
│  │  CompanyBranchConverters:                            │   │
│  │  - ToDto() → Entity → DTO                            │   │
│  └──────────────────────────────────────────────────────┘   │
└───────────────────────┬─────────────────────────────────────┘
                        │
                        ▼
┌─────────────────────────────────────────────────────────────┐
│                        OUTPUT                                │
│  ┌──────────────────────────────────────────────────────┐   │
│  │  Display data on GridView                            │   │
│  │  Show success/error messages                         │   │
│  │  Update button states                                │   │
│  │  Update status bar                                   │   │
│  │  Export Excel file                                   │   │
│  └──────────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────────┘
```

---

## 4. Validation System

### 4.1. Selection Validation

| Operation | Validation Rule | Error Message |
|-----------|----------------|---------------|
| **Edit** | Must select exactly 1 row | "Vui lòng chọn một dòng để chỉnh sửa." hoặc "Chỉ cho phép chỉnh sửa 1 dòng. Vui lòng bỏ chọn bớt." |
| **Delete** | Must select at least 1 row | "Vui lòng chọn ít nhất một dòng để xóa." |
| **Double-Click** | Must select exactly 1 row | "Vui lòng chọn một dòng để xem chi tiết." hoặc "Chỉ cho phép xem chi tiết 1 dòng. Vui lòng bỏ chọn bớt." |
| **Export** | Must have at least 1 row in GridView | "Không có dữ liệu để xuất." |

### 4.2. Business Rules Validation

**Rule 1: Cannot delete all branches**
```csharp
if (totalBranches - selectedBranches <= 0)
{
    ShowInfo("Không thể xóa tất cả chi nhánh. Công ty phải có ít nhất một chi nhánh.");
    return false;
}
```

**Rule 2: Cannot delete last branch**
```csharp
if (totalBranches == 1 && selectedBranches == 1)
{
    ShowInfo("Không thể xóa chi nhánh cuối cùng. Công ty phải có ít nhất một chi nhánh.");
    return false;
}
```

### 4.3. Data Validation

**Export Validation:**
```csharp
var rowCount = CompanyBranchGridView.RowCount;
if (rowCount <= 0)
{
    ShowInfo("Không có dữ liệu để xuất.");
    return;
}
```

### 4.4. Validation Flow

```
User Action
  │
  ├─> Selection Validation
  │   ├─> Check selection count
  │   └─> Show error if invalid
  │
  ├─> Business Rules Validation (for Delete)
  │   ├─> Check total branches
  │   ├─> Check selected branches
  │   └─> Show error if violates rules
  │
  ├─> Data Validation (for Export)
  │   ├─> Check row count
  │   └─> Show error if no data
  │
  └─> Proceed if all valid
```

---

## 5. Business Logic Flow

### 5.1. Load Data Flow (Sequence Diagram)

```
User                    UcCompanyBranch        CompanyBranchBll    CompanyBranchDataAccess    Database
 │                          │                         │                       │              │
 │  ──Click "Danh sách"──> │                         │                       │              │
 │                          │                         │                       │              │
 │                          │──LoadDataAsync()───────>│                       │              │
 │                          │                         │                       │              │
 │                          │                         │──GetAllAsync()───────>│              │
 │                          │                         │                       │──Query──────>│
 │                          │                         │                       │<─List───────│
 │                          │                         │<──List<CompanyBranch>──│              │
 │                          │<──List<CompanyBranch>──│                       │              │
 │                          │                         │                       │              │
 │                          │──ToDto()───────────────│                       │              │
 │                          │  (CompanyBranchConverters)                      │              │
 │                          │                         │                       │              │
 │                          │──BindGrid(dtoList)─────│                       │              │
 │                          │  (Update GridView)     │                       │              │
 │                          │                         │                       │              │
 │                          │──UpdateDataSummary()───│                       │              │
 │                          │  (Update status bar)    │                       │              │
 │                          │                         │                       │              │
 │<───Data Displayed────────│                         │                       │              │
```

### 5.2. Add New Branch Flow

```
User                    UcCompanyBranch        FrmCompanyBranchDetail    CompanyBranchBll    Database
 │                          │                         │                         │              │
 │  ──Click "Mới"─────────> │                         │                         │              │
 │                          │                         │                         │              │
 │                          │──Show OverlayManager───>│                         │              │
 │                          │                         │                         │              │
 │                          │──new FrmCompanyBranchDetail(Guid.Empty)──────────>│              │
 │                          │                         │                         │              │
 │                          │                         │──ShowDialog()───────────│              │
 │<───Form Displayed────────│<────────────────────────│                         │              │
 │  ──Enter data───────────>│                         │                         │              │
 │  ──Click "Lưu"──────────>│                         │                         │              │
 │                          │                         │──Save()─────────────────>│              │
 │                          │                         │                         │──Insert─────>│
 │                          │                         │                         │<──OK──────────│
 │                          │                         │<──OK────────────────────│              │
 │                          │                         │──Close()────────────────│              │
 │<───Form Closed───────────│<────────────────────────│                         │              │
 │                          │                         │                         │              │
 │                          │──LoadDataAsync()───────>│                         │              │
 │                          │  (Reload list)          │                         │              │
 │                          │                         │                         │              │
 │                          │──Close OverlayManager───│                         │              │
 │                          │                         │                         │              │
 │<───List Updated──────────│                         │                         │              │
```

### 5.3. Edit Branch Flow

```
User                    UcCompanyBranch        FrmCompanyBranchDetail    CompanyBranchBll    Database
 │                          │                         │                         │              │
 │  ──Select 1 row─────────> │                         │                         │              │
 │  ──Click "Điều chỉnh"───> │                         │                         │              │
 │                          │                         │                         │              │
 │                          │──Validate: 1 row selected                         │              │
 │                          │                         │                         │              │
 │                          │──Show OverlayManager───>│                         │              │
 │                          │                         │                         │              │
 │                          │──new FrmCompanyBranchDetail(id)──────────────────>│              │
 │                          │                         │                         │              │
 │                          │                         │──LoadData(id)───────────>│              │
 │                          │                         │                         │──Query──────>│
 │                          │                         │                         │<─Branch─────│
 │                          │                         │<──Branch────────────────│              │
 │                          │                         │                         │              │
 │                          │                         │──ShowDialog()───────────│              │
 │<───Form Displayed────────│<────────────────────────│                         │              │
 │  ──Edit data────────────>│                         │                         │              │
 │  ──Click "Lưu"──────────>│                         │                         │              │
 │                          │                         │──Save()─────────────────>│              │
 │                          │                         │                         │──Update────>│
 │                          │                         │                         │<──OK──────────│
 │                          │                         │<──OK────────────────────│              │
 │                          │                         │──Close()────────────────│              │
 │<───Form Closed───────────│<────────────────────────│                         │              │
 │                          │                         │                         │              │
 │                          │──LoadDataAsync()───────>│                         │              │
 │                          │  (Reload list)          │                         │              │
 │                          │                         │                         │              │
 │                          │──Close OverlayManager───│                         │              │
 │                          │                         │                         │              │
 │<───List Updated──────────│                         │                         │              │
```

### 5.4. Delete Branch Flow

```
User                    UcCompanyBranch        CompanyBranchBll    Database
 │                          │                         │              │
 │  ──Select rows──────────> │                         │              │
 │  ──Click "Xóa"──────────> │                         │              │
 │                          │                         │              │
 │                          │──Validate: >= 1 row selected          │              │
 │                          │                         │              │
 │                          │──ValidateDeleteBusinessRules()───────>│              │
 │                          │  (Check total branches)  │              │
 │                          │                         │              │
 │                          │──Show confirmation dialog             │              │
 │<───Confirm Dialog────────│                         │              │
 │  ──Click "Yes"──────────>│                         │              │
 │                          │                         │              │
 │                          │──Show WaitForm1─────────│              │
 │                          │                         │              │
 │                          │──For each selected ID:  │              │
 │                          │  └─> Delete(id)────────>│              │
 │                          │                         │──Delete─────>│
 │                          │                         │<──OK──────────│
 │                          │                         │              │
 │                          │──LoadDataAsync()───────>│              │
 │                          │  (Reload list)          │              │
 │                          │                         │              │
 │                          │──Close WaitForm1───────│              │
 │                          │                         │              │
 │<───List Updated──────────│                         │              │
```

### 5.5. Export Flow

```
User                    UcCompanyBranch        GridView
 │                          │                         │
 │  ──Click "Xuất"─────────> │                         │
 │                          │                         │
 │                          │──Check rowCount > 0     │
 │                          │                         │
 │                          │──Show SaveFileDialog    │
 │<───Save Dialog───────────│                         │
 │  ──Select location──────>│                         │
 │  ──Click "Save"──────────>│                         │
 │                          │                         │
 │                          │──GridView.ExportToXlsx()─────────────>│
 │                          │                         │
 │                          │──Show success message   │
 │<───Success Message───────│                         │
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
        using (var form = new FrmCompanyBranchDetail(Guid.Empty))
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
    using (OverlayManager.ShowScope(this))
    {
        using (var form = new FrmCompanyBranchDetail(id))
        {
            form.ShowDialog(this);
            await LoadDataAsync();
            UpdateButtonStates();
        }
    }
}
catch (Exception ex)
{
    ShowError(ex, "Lỗi hiển thị màn hình điều chỉnh");
}
```

#### 6.1.4. DeleteBarButtonItem_ItemClick

```csharp
try
{
    await ExecuteWithWaitingFormAsync(() =>
    {
        foreach (var id in _selectedBranchIds)
        {
            _companyBranchBll.Delete(id);
        }
        return Task.CompletedTask;
    });
    
    await LoadDataAsync();
}
catch (Exception ex)
{
    ShowError(ex, "Lỗi xóa dữ liệu");
}
```

#### 6.1.5. ExportBarButtonItem_ItemClick

```csharp
try
{
    var saveDialog = new SaveFileDialog { ... };
    if (saveDialog.ShowDialog() == DialogResult.OK)
    {
        CompanyBranchGridView.ExportToXlsx(saveDialog.FileName);
        ShowInfo("Xuất dữ liệu thành công!");
    }
}
catch (Exception ex)
{
    ShowError(ex, "Lỗi xuất dữ liệu");
}
```

#### 6.1.6. CompanyBranchGridView_SelectionChanged

```csharp
try
{
    UpdateSelectedBranchIds();
    UpdateButtonStates();
    UpdateDataSummary();
}
catch (Exception ex)
{
    ShowError(ex);
}
```

#### 6.1.7. CompanyBranchGridView_RowCellStyle

```csharp
try
{
    // ... styling logic
}
catch (Exception)
{
    // ignore style errors
}
```

### 6.2. Error Display

**MsgBox Helper:**
- `MsgBox.ShowSuccess()` - Thông báo thành công
- `MsgBox.ShowException()` - Hiển thị exception với stack trace
- `ShowInfo()` - Wrapper cho ShowSuccess
- `ShowError()` - Wrapper cho ShowException với context

**Error Messages:**
- "Lỗi tải dữ liệu"
- "Lỗi hiển thị màn hình thêm mới"
- "Lỗi hiển thị màn hình điều chỉnh"
- "Lỗi xóa dữ liệu"
- "Lỗi xuất dữ liệu"
- "Lỗi kiểm tra business rules"

---

## 7. Security & Best Practices

### 7.1. Security

**Hiện tại không có thông tin nhạy cảm** được xử lý trong UcCompanyBranch:

- Không có password
- Không có Remember Me
- Không có authentication/authorization logic
- Dữ liệu được lưu trữ trong database

### 7.2. Best Practices

#### 7.2.1. Async/Await

✅ **Đã áp dụng:**
- Tất cả data operations sử dụng async/await
- LoadDataAsync() với async pattern
- ExecuteWithWaitingFormAsync() để hiển thị WaitForm

#### 7.2.2. Error Handling

✅ **Đã áp dụng:**
- Try-catch cho tất cả operations
- User-friendly error messages
- Context-aware error messages

#### 7.2.3. Re-entrancy Guard

✅ **Đã áp dụng:**
- `_isLoading` flag để tránh gọi LoadDataAsync song song
- Check guard trước khi thực hiện operation

#### 7.2.4. Selection Management

✅ **Đã áp dụng:**
- `_selectedBranchIds` để lưu trữ IDs đã chọn
- Clear selection sau khi bind data mới
- Update selection khi GridView selection thay đổi

#### 7.2.5. UI State Management

✅ **Đã áp dụng:**
- UpdateButtonStates() để cập nhật trạng thái nút
- UpdateDataSummary() để cập nhật status bar
- Tự động cập nhật sau mỗi operation

### 7.3. Gợi Ý Cải Thiện

#### 7.3.1. Loading State Management

**Gợi ý cải thiện guard pattern:**
```csharp
private readonly SemaphoreSlim _loadSemaphore = new SemaphoreSlim(1, 1);

private async Task LoadDataAsync()
{
    if (!await _loadSemaphore.WaitAsync(0))
        return; // Already loading
    
    try
    {
        await ExecuteWithWaitingFormAsync(async () =>
        {
            await LoadDataAsyncWithoutSplash();
        });
    }
    finally
    {
        _loadSemaphore.Release();
    }
}
```

#### 7.3.2. Cancellation Token

**Gợi ý thêm cancellation support:**
```csharp
private CancellationTokenSource _cancellationTokenSource;

private async Task LoadDataAsync()
{
    _cancellationTokenSource?.Cancel();
    _cancellationTokenSource = new CancellationTokenSource();
    
    try
    {
        await LoadDataAsyncWithoutSplash(_cancellationTokenSource.Token);
    }
    catch (OperationCanceledException)
    {
        // Handle cancellation
    }
}
```

#### 7.3.3. Progress Reporting

**Gợi ý thêm progress reporting:**
```csharp
private async Task LoadDataAsync(IProgress<int> progress = null)
{
    await ExecuteWithWaitingFormAsync(async () =>
    {
        progress?.Report(0);
        var branches = await _companyBranchBll.GetAllAsync();
        progress?.Report(50);
        var dtoList = branches.Select(b => b.ToDto()).ToList();
        progress?.Report(100);
        BindGrid(dtoList);
    });
}
```

---

## 8. Extensibility Guide

### 8.1. Cách Mở Rộng Form

#### 8.1.1. Thêm Cột Mới vào GridView

**Bước 1**: Thêm property vào `CompanyBranchDto.cs`
```csharp
[DisplayName("Ghi chú")]
[StringLength(500)]
public string Notes { get; set; }
```

**Bước 2**: Thêm column vào `UcCompanyBranch.Designer.cs`
```csharp
private GridColumn colNotes;
```

**Bước 3**: Cấu hình column trong Designer
```csharp
this.colNotes.FieldName = "Notes";
this.colNotes.Name = "colNotes";
this.colNotes.Visible = true;
this.colNotes.VisibleIndex = 8;
```

**Bước 4**: Cập nhật converter nếu cần
```csharp
Notes = entity.Notes
```

#### 8.1.2. Thêm Nút Mới vào Toolbar

**Bước 1**: Thêm BarButtonItem vào Designer
```csharp
private BarButtonItem RefreshBarButtonItem;
```

**Bước 2**: Cấu hình trong InitializeComponent()
```csharp
this.RefreshBarButtonItem.Caption = "Làm mới";
this.RefreshBarButtonItem.Id = 5;
this.RefreshBarButtonItem.Name = "RefreshBarButtonItem";
```

**Bước 3**: Thêm event handler
```csharp
RefreshBarButtonItem.ItemClick += RefreshBarButtonItem_ItemClick;
```

**Bước 4**: Implement event handler
```csharp
private async void RefreshBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
{
    await LoadDataAsync();
}
```

#### 8.1.3. Thêm Chức Năng Import từ Excel

```csharp
private void ImportBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
{
    try
    {
        using (var openDialog = new OpenFileDialog())
        {
            openDialog.Filter = "Excel Files (*.xlsx)|*.xlsx|All Files (*.*)|*.*";
            openDialog.Title = "Chọn file Excel để import";
            
            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                // Read Excel file
                // Parse data
                // Validate data
                // Save to database
                // Reload grid
            }
        }
    }
    catch (Exception ex)
    {
        ShowError(ex, "Lỗi import dữ liệu");
    }
}
```

#### 8.1.4. Thêm Chức Năng In

```csharp
private void PrintBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
{
    try
    {
        var rowCount = CompanyBranchGridView.RowCount;
        if (rowCount <= 0)
        {
            ShowInfo("Không có dữ liệu để in.");
            return;
        }
        
        CompanyBranchGridView.ShowPrintPreview();
    }
    catch (Exception ex)
    {
        ShowError(ex, "Lỗi in dữ liệu");
    }
}
```

### 8.2. Clean Code Patterns

#### 8.2.1. Repository Pattern

**Hiện tại**: Sử dụng trực tiếp `CompanyBranchBll`

**Gợi ý**: Tạo interface `ICompanyBranchRepository`:
```csharp
public interface ICompanyBranchRepository
{
    Task<List<CompanyBranch>> GetAllAsync();
    Task<CompanyBranch> GetByIdAsync(Guid id);
    Task CreateAsync(CompanyBranch branch);
    Task UpdateAsync(CompanyBranch branch);
    Task DeleteAsync(Guid id);
}
```

#### 8.2.2. Dependency Injection

**Gợi ý**: Sử dụng DI container:
```csharp
public UcCompanyBranch(ICompanyBranchBll companyBranchBll)
{
    InitializeComponent();
    _companyBranchBll = companyBranchBll;
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

public class DeleteBranchCommand : ICommand
{
    private readonly UcCompanyBranch _ucCompanyBranch;
    private readonly List<Guid> _branchIds;
    
    public async Task ExecuteAsync()
    {
        // Delete logic
    }
}
```

### 8.3. Async Patterns

**Gợi ý**: Sử dụng async/await cho tất cả operations:
```csharp
private async Task<bool> ValidateDeleteBusinessRulesAsync()
{
    var allBranches = await _companyBranchBll.GetAllAsync();
    // ... validation logic
}
```

---

## 9. Test Checklist

### 9.1. Unit Test Cases

#### 9.1.1. Load Tests

- [ ] Test `LoadDataAsync` với dữ liệu tồn tại
- [ ] Test `LoadDataAsync` với không có dữ liệu
- [ ] Test `LoadDataAsync` với re-entrancy guard (_isLoading)
- [ ] Test `BindGrid` với empty list
- [ ] Test `BindGrid` với large list (1000+ items)
- [ ] Test `UpdateDataSummary` với các trường hợp khác nhau

#### 9.1.2. Selection Tests

- [ ] Test `UpdateSelectedBranchIds` với 0 selection
- [ ] Test `UpdateSelectedBranchIds` với 1 selection
- [ ] Test `UpdateSelectedBranchIds` với multiple selections
- [ ] Test `ClearSelectionState` xóa selection đúng cách
- [ ] Test `UpdateButtonStates` với các trạng thái selection khác nhau

#### 9.1.3. Business Rules Tests

- [ ] Test `ValidateDeleteBusinessRules` với totalBranches = 1, selectedBranches = 1 (should fail)
- [ ] Test `ValidateDeleteBusinessRules` với totalBranches = 2, selectedBranches = 2 (should fail)
- [ ] Test `ValidateDeleteBusinessRules` với totalBranches = 2, selectedBranches = 1 (should pass)
- [ ] Test `ValidateDeleteBusinessRules` với totalBranches = 10, selectedBranches = 5 (should pass)

#### 9.1.4. Error Handling Tests

- [ ] Test exception trong `LoadDataAsync`
- [ ] Test exception trong `NewBarButtonItem_ItemClick`
- [ ] Test exception trong `EditBarButtonItem_ItemClick`
- [ ] Test exception trong `DeleteBarButtonItem_ItemClick`
- [ ] Test exception trong `ExportBarButtonItem_ItemClick`

### 9.2. Manual Testing Scenarios

#### 9.2.1. Scenario 1: Load Data

**Steps:**
1. Mở UserControl
2. Click nút "Danh sách"
3. Kiểm tra WaitForm hiển thị
4. Kiểm tra dữ liệu được load vào GridView
5. Kiểm tra status bar cập nhật

**Expected:**
- WaitForm hiển thị và đóng đúng cách
- Dữ liệu hiển thị trong GridView
- Status bar hiển thị "Tổng: X chi nhánh"

#### 9.2.2. Scenario 2: Add New Branch

**Steps:**
1. Click nút "Mới"
2. Kiểm tra overlay hiển thị
3. Kiểm tra form FrmCompanyBranchDetail mở
4. Nhập thông tin và lưu
5. Kiểm tra danh sách được tải lại

**Expected:**
- Overlay hiển thị
- Form mở ở chế độ thêm mới
- Sau khi lưu, danh sách được tải lại với chi nhánh mới

#### 9.2.3. Scenario 3: Edit Branch

**Steps:**
1. Chọn 1 dòng trong GridView
2. Click nút "Điều chỉnh"
3. Kiểm tra form mở với dữ liệu đã có
4. Sửa thông tin và lưu
5. Kiểm tra danh sách được tải lại

**Expected:**
- Form mở ở chế độ chỉnh sửa
- Dữ liệu hiển thị đúng
- Sau khi lưu, danh sách được tải lại với dữ liệu đã cập nhật

#### 9.2.4. Scenario 4: Delete Branch (Single)

**Steps:**
1. Chọn 1 dòng
2. Click nút "Xóa"
3. Kiểm tra business rules validation
4. Xác nhận xóa
5. Kiểm tra dòng bị xóa

**Expected:**
- Business rules được kiểm tra
- Dialog xác nhận hiển thị
- Sau khi xác nhận, dòng bị xóa và danh sách được tải lại

#### 9.2.5. Scenario 5: Delete All Branches (Should Fail)

**Steps:**
1. Chọn tất cả các dòng
2. Click nút "Xóa"
3. Kiểm tra business rules validation

**Expected:**
- Business rules validation fail
- Hiển thị thông báo "Không thể xóa tất cả chi nhánh"
- Không có dialog xác nhận
- Dữ liệu không thay đổi

#### 9.2.6. Scenario 6: Export to Excel

**Steps:**
1. Đảm bảo có dữ liệu trong GridView
2. Click nút "Xuất"
3. Chọn vị trí lưu file
4. Kiểm tra file được tạo

**Expected:**
- SaveFileDialog hiển thị
- File Excel được tạo tại vị trí đã chọn
- File chứa toàn bộ dữ liệu trong GridView
- Hiển thị thông báo thành công

#### 9.2.7. Scenario 7: Multi-Select

**Steps:**
1. Chọn nhiều dòng bằng checkbox
2. Kiểm tra status bar cập nhật
3. Kiểm tra nút "Điều chỉnh" bị vô hiệu hóa
4. Kiểm tra nút "Xóa" được kích hoạt

**Expected:**
- Status bar hiển thị "Đã chọn: X dòng"
- Nút "Điều chỉnh" bị vô hiệu hóa (vì chọn nhiều hơn 1 dòng)
- Nút "Xóa" được kích hoạt

#### 9.2.8. Scenario 8: Double-Click Row

**Steps:**
1. Double-click vào một dòng
2. Kiểm tra form chi tiết mở

**Expected:**
- Form FrmCompanyBranchDetail mở ở chế độ chỉnh sửa
- Dữ liệu hiển thị đúng

#### 9.2.9. Scenario 9: Row Styling

**Steps:**
1. Tải dữ liệu có cả dòng IsActive = True và False
2. Kiểm tra dòng IsActive = False có màu đỏ

**Expected:**
- Dòng IsActive = False có màu chữ đỏ
- Dòng IsActive = True có màu chữ bình thường

#### 9.2.10. Scenario 10: Auto Filter

**Steps:**
1. Nhập giá trị vào Auto Filter Row
2. Kiểm tra dữ liệu được lọc

**Expected:**
- Dữ liệu được lọc theo giá trị đã nhập
- Chỉ hiển thị các dòng khớp với filter

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
- User Control UcCompanyBranch để quản lý danh sách chi nhánh công ty
- Chức năng tải dữ liệu với WaitForm
- Chức năng thêm mới chi nhánh
- Chức năng chỉnh sửa chi nhánh
- Chức năng xóa một hoặc nhiều chi nhánh với business rules validation
- Chức năng xuất dữ liệu ra Excel
- Multi-select với checkbox
- Auto filter row
- Find panel
- Row styling (màu đỏ cho dòng không hoạt động)
- Row indicator (số thứ tự)
- Double-click để mở form chi tiết
- Status bar hiển thị thống kê
- SuperToolTips cho tất cả các nút
- OverlayManager khi mở form detail
- Re-entrancy guard (_isLoading)

### Changed
- (Chưa có)

### Fixed
- (Chưa có)

### Removed
- (Chưa có)

### Known Issues
- Không có phím tắt được cấu hình
- Không có chức năng in trực tiếp
- Không có chức năng import từ Excel
```

---

## 11. Additional Notes

### 11.1. Missing Features

⚠️ **Không có phím tắt được cấu hình**  
⚠️ **Không có chức năng in trực tiếp** (phải xuất Excel rồi in)  
⚠️ **Không có chức năng import từ Excel**  

### 11.2. Future Enhancements

- [ ] Add keyboard shortcuts
- [ ] Add print functionality
- [ ] Add import from Excel
- [ ] Add bulk operations (activate/deactivate multiple branches)
- [ ] Add advanced filtering
- [ ] Add column customization
- [ ] Add export to PDF
- [ ] Add undo/redo support
- [ ] Add change tracking

---

**Tài liệu này được tạo tự động từ source code. Cập nhật lần cuối: 2025-01-XX**

