# FrmDepartmentDetail - Developer Guide

## Mục Lục

1. [Overview](#1-overview)
2. [Architecture](#2-architecture)
3. [Detailed Technical Breakdown](#3-detailed-technical-breakdown)
4. [Validation System](#4-validation-system)
5. [Business Logic Flow](#5-business-logic-flow)
6. [Error Handling](#6-error-handling)
7. [Security](#7-security)
8. [Extensibility Guide](#8-extensibility-guide)
9. [Test Checklist](#9-test-checklist)
10. [Changelog Template](#10-changelog-template)

---

## 1. Overview

### 1.1. Vai Trò Trong Module

`FrmDepartmentDetail` là form chi tiết dùng để **tạo mới** và **chỉnh sửa** phòng ban trong module **MasterData.Company**. Form này là một phần của hệ thống quản lý cấu trúc tổ chức công ty, cho phép:

- Tạo mới phòng ban với đầy đủ thông tin (mã, tên, mô tả, chi nhánh, phòng ban cha)
- Chỉnh sửa thông tin phòng ban đã tồn tại (trừ mã phòng ban)
- Quản lý cấu trúc phân cấp phòng ban (parent-child relationship)
- Gán phòng ban vào chi nhánh cụ thể

### 1.2. File Structure

Form này bao gồm các file sau:

```
MasterData/Company/
├── FrmDepartmentDetail.cs              # Main form class (639 lines)
├── FrmDepartmentDetail.Designer.cs     # UI designer code (484 lines)
└── FrmDepartmentDetail.resx            # Resources (nếu có)
```

**Dependencies:**

- `MasterData.Company.Dto.DepartmentDto` - DTO cho phòng ban
- `MasterData.Company.Dto.CompanyBranchDto` - DTO cho chi nhánh
- `Bll.MasterData.Company.DepartmentBll` - Business Logic Layer
- `Bll.MasterData.Company.CompanyBranchBll` - BLL cho chi nhánh
- `Bll.MasterData.Company.CompanyBll` - BLL cho công ty
- `Bll.Utils.RequiredFieldHelper` - Helper đánh dấu trường bắt buộc
- `Bll.Utils.SuperToolTipHelper` - Helper tạo SuperToolTip
- `Dal.DataContext.Department` - Entity class
- `MasterData.Company.Converters.DepartmentConverters` - Converter Entity ↔ DTO

---

## 2. Architecture

### 2.1. Layer Architecture

Form này tuân theo kiến trúc **3-layer**:

```
┌─────────────────────────────────────────────────────────┐
│                    UI Layer                              │
│  ┌───────────────────────────────────────────────────┐  │
│  │  FrmDepartmentDetail (WinForm)                    │  │
│  │  - DataLayoutControl                              │  │
│  │  - DXErrorProvider                                │  │
│  │  - DevExpress Controls                            │  │
│  └───────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────┘
                          │
                          ▼
┌─────────────────────────────────────────────────────────┐
│                 Business Logic Layer (BLL)              │
│  ┌───────────────────────────────────────────────────┐  │
│  │  DepartmentBll                                     │  │
│  │  - CreateAsync()                                  │  │
│  │  - UpdateAsync()                                  │  │
│  │  - GetById()                                      │  │
│  │  - GetAll()                                       │  │
│  │  - ValidateDepartmentAsync()                     │  │
│  └───────────────────────────────────────────────────┘  │
│  ┌───────────────────────────────────────────────────┐  │
│  │  CompanyBranchBll                                 │  │
│  │  - GetActiveBranches()                            │  │
│  └───────────────────────────────────────────────────┘  │
│  ┌───────────────────────────────────────────────────┐  │
│  │  CompanyBll                                       │  │
│  │  - GetCompany()                                   │  │
│  └───────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────┘
                          │
                          ▼
┌─────────────────────────────────────────────────────────┐
│              Data Access Layer (DAL)                    │
│  ┌───────────────────────────────────────────────────┐  │
│  │  DepartmentDataAccess                             │  │
│  │  - CreateAsync()                                  │  │
│  │  - UpdateDepartmentAsync()                        │  │
│  │  - GetById()                                      │  │
│  │  - GetAll()                                       │  │
│  └───────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────┘
                          │
                          ▼
┌─────────────────────────────────────────────────────────┐
│                    Database                              │
│  ┌───────────────────────────────────────────────────┐  │
│  │  Department Table                                 │  │
│  │  CompanyBranch Table                              │  │
│  │  Company Table                                    │  │
│  └───────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────┘
```

### 2.2. Dependencies

**DevExpress Components:**
- `DevExpress.XtraBars` - Toolbar và menu
- `DevExpress.XtraEditors` - TextEdit, ToggleSwitch, SearchLookUpEdit, TreeListLookUpEdit
- `DevExpress.XtraLayout` - DataLayoutControl
- `DevExpress.XtraEditors.DXErrorProvider` - Error provider
- `DevExpress.XtraGrid` - GridView cho SearchLookUpEdit
- `DevExpress.XtraTreeList` - TreeList cho TreeListLookUpEdit

**Internal Dependencies:**
- `Bll.Utils.RequiredFieldHelper` - Đánh dấu trường bắt buộc
- `Bll.Utils.SuperToolTipHelper` - Tạo SuperToolTip
- `MasterData.Company.Converters.DepartmentConverters` - Converter Entity ↔ DTO

### 2.3. Data Flow Diagram

```
User Input
  │
  ├─> Controls (TextEdit, SearchLookUpEdit, TreeListLookUpEdit, ToggleSwitch)
  │   │
  │   ├─> EditValueChanged Events
  │   │   └─> Update _branchId, _parentId variables
  │   │
  │   └─> Validation (DXErrorProvider)
  │       └─> Display error icons
  │
  ├─> SaveBarButtonItem_ItemClick
  │   │
  │   ├─> ValidateForm()
  │   │   ├─> Check DepartmentCode (if new)
  │   │   ├─> Check DepartmentName
  │   │   └─> Check BranchId
  │   │
  │   ├─> GetDepartmentFromControls()
  │   │   ├─> GetCompanyIdFromDatabase()
  │   │   ├─> Create Department Entity
  │   │   └─> Set BranchId, ParentId from variables
  │   │
  │   ├─> DepartmentBll.CreateAsync() or UpdateAsync()
  │   │   ├─> ValidateDepartmentAsync()
  │   │   └─> DepartmentDataAccess.CreateAsync() or UpdateDepartmentAsync()
  │   │
  │   └─> Show success message → Close form
  │
  └─> CloseBarButtonItem_ItemClick
      └─> Close form (no save)
```

---

## 3. Detailed Technical Breakdown

### 3.1. Controls trong Designer

#### 3.1.1. Toolbar Controls

| Control | Type | Name | Chức Năng |
|---------|------|------|-----------|
| BarManager | `BarManager` | `barManager1` | Quản lý toolbar |
| Bar | `Bar` | `bar2` | Thanh toolbar chính |
| BarButtonItem | `BarButtonItem` | `SaveBarButtonItem` | Nút Lưu |
| BarButtonItem | `BarButtonItem` | `CloseBarButtonItem` | Nút Đóng |

#### 3.1.2. Layout Controls

| Control | Type | Name | Chức Năng |
|---------|------|------|-----------|
| DataLayoutControl | `DataLayoutControl` | `dataLayoutControl1` | Container chính cho form layout |
| LayoutControlGroup | `LayoutControlGroup` | `Root` | Root group |
| LayoutControlGroup | `LayoutControlGroup` | `layoutControlGroup1` | Group chứa các items |

#### 3.1.3. Input Controls

| Control | Type | Name | Binding Source | Chức Năng |
|---------|------|------|----------------|-----------|
| TextEdit | `TextEdit` | `DepartmentCodeTextEdit` | - | Nhập mã phòng ban |
| TextEdit | `TextEdit` | `DepartmentNameTextEdit` | - | Nhập tên phòng ban |
| TextEdit | `TextEdit` | `DescriptionTextEdit` | - | Nhập mô tả |
| SearchLookUpEdit | `SearchLookUpEdit` | `BranchNameSearchLookupedit` | `companyBranchDtoBindingSource` | Chọn chi nhánh |
| TreeListLookUpEdit | `TreeListLookUpEdit` | `ParentDepartmentNameTextEdit` | `departmentDtoBindingSource` | Chọn phòng ban cha |
| ToggleSwitch | `ToggleSwitch` | `IsActiveToogleSwitch` | - | Bật/tắt trạng thái |

#### 3.1.4. Binding Sources

| Binding Source | Type | DataSource Type | Chức Năng |
|----------------|------|-----------------|-----------|
| `companyBranchDtoBindingSource` | `BindingSource` | `CompanyBranchDto` | Data source cho chi nhánh |
| `departmentDtoBindingSource` | `BindingSource` | `DepartmentDto` | Data source cho phòng ban |

#### 3.1.5. Grid/TreeList Views

| View | Type | Name | Columns | Chức Năng |
|------|------|------|---------|-----------|
| GridView | `GridView` | `searchLookUpEdit1View` | `colBranchName`, `colFullAddress` | Hiển thị danh sách chi nhánh |
| TreeList | `TreeList` | `treeListLookUpEdit1TreeList` | `colParentDepartmentName`, `colDepartmentName` | Hiển thị cây phòng ban |

#### 3.1.6. Error Provider

| Control | Type | Name | Chức Năng |
|---------|------|------|-----------|
| DXErrorProvider | `DXErrorProvider` | `dxErrorProvider1` | Hiển thị lỗi validation |

### 3.2. Event Handlers

#### 3.2.1. SaveBarButtonItem_ItemClick

```csharp
private async void SaveBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
```

**Chức năng:**
- Validate form data
- Lưu phòng ban vào database (Create hoặc Update)
- Hiển thị thông báo thành công/thất bại
- Đóng form sau khi lưu thành công

**Flow:**
1. Gọi `ValidateForm()` → Nếu fail, return
2. Gọi `SaveDepartment()` (async)
3. Trong `SaveDepartment()`:
   - Gọi `GetDepartmentFromControls()` để tạo Entity
   - Nếu `_isEditMode`: Gọi `_departmentBll.UpdateAsync()`
   - Nếu không: Gọi `_departmentBll.CreateAsync()`
   - Hiển thị thông báo thành công
   - Set `DialogResult = DialogResult.OK`
   - Đóng form

#### 3.2.2. CloseBarButtonItem_ItemClick

```csharp
private void CloseBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
```

**Chức năng:**
- Đóng form ngay lập tức
- Không lưu dữ liệu

#### 3.2.3. BranchNameSearchLookupedit_EditValueChanged

```csharp
private void BranchNameSearchLookupedit_EditValueChanged(object sender, EventArgs e)
```

**Chức năng:**
- Cập nhật biến `_branchId` khi người dùng chọn chi nhánh
- Parse `EditValue` thành `Guid` và lưu vào `_branchId`
- Nếu `EditValue` là null, set `_branchId = null`

**Lưu ý:** Biến `_branchId` được sử dụng trong `GetDepartmentFromControls()` để set `Department.BranchId`.

#### 3.2.4. ParentDepartmentNameTextEdit_EditValueChanged

```csharp
private void ParentDepartmentNameTextEdit_EditValueChanged(object sender, EventArgs e)
```

**Chức năng:**
- Cập nhật biến `_parentId` khi người dùng chọn phòng ban cha
- Parse `EditValue` thành `Guid` và lưu vào `_parentId`
- Nếu `EditValue` là null, set `_parentId = null`

**Lưu ý:** Biến `_parentId` được sử dụng trong `GetDepartmentFromControls()` để set `Department.ParentId`.

### 3.3. Methods

#### 3.3.1. InitializeForm()

```csharp
private void InitializeForm()
```

**Chức năng:**
- Khởi tạo form: Load datasources, setup form mode, event handlers, validation, SuperToolTips

**Flow:**
1. `LoadDataSources()` - Load chi nhánh và phòng ban
2. Nếu `_isEditMode`: `LoadDepartmentData()` và set title "Chỉnh sửa phòng ban"
3. Nếu không: `SetDefaultValues()` và set title "Thêm mới phòng ban"
4. `SetupEventHandlers()` - Đăng ký event handlers
5. `SetupAdvancedValidation()` - Setup validation với DataAnnotations
6. `SetupSuperTips()` - Setup SuperToolTips

#### 3.3.2. LoadDataSources()

```csharp
private void LoadDataSources()
```

**Chức năng:**
- Load các datasource cho form: chi nhánh và phòng ban

**Flow:**
1. `LoadCompanyBranches()` - Load danh sách chi nhánh đang hoạt động
2. `LoadDepartments()` - Load danh sách phòng ban

#### 3.3.3. LoadCompanyBranches()

```csharp
private void LoadCompanyBranches()
```

**Chức năng:**
- Load danh sách chi nhánh đang hoạt động từ `CompanyBranchBll.GetActiveBranches()`
- Convert sang DTO và bind vào `companyBranchDtoBindingSource`

#### 3.3.4. LoadDepartments()

```csharp
private void LoadDepartments()
```

**Chức năng:**
- Load danh sách phòng ban từ `DepartmentBll.GetAll()`
- Convert sang DTO và bind vào `departmentDtoBindingSource`

#### 3.3.5. LoadDepartmentData()

```csharp
private void LoadDepartmentData()
```

**Chức năng:**
- Load dữ liệu phòng ban khi ở chế độ edit
- Chỉ chạy khi `_isEditMode == true` và `_departmentId != Guid.Empty`

**Flow:**
1. Gọi `_departmentBll.GetById(_departmentId)`
2. Nếu không tìm thấy: Hiển thị lỗi và đóng form
3. Convert Entity sang DTO: `department.ToDto()`
4. Lưu vào `_currentDepartment`
5. Gọi `BindDepartmentToControls()` để bind dữ liệu vào controls

#### 3.3.6. BindDepartmentToControls()

```csharp
private void BindDepartmentToControls()
```

**Chức năng:**
- Bind dữ liệu từ `_currentDepartment` vào các controls

**Flow:**
1. Set `DepartmentCodeTextEdit.EditValue = _currentDepartment.DepartmentCode`
2. Set `DepartmentNameTextEdit.EditValue = _currentDepartment.DepartmentName`
3. Set `DescriptionTextEdit.EditValue = _currentDepartment.Description`
4. Set `IsActiveToogleSwitch.EditValue = _currentDepartment.IsActive`
5. **Khóa mã phòng ban**: `DepartmentCodeTextEdit.Properties.ReadOnly = true`, `Enabled = false`
6. Set `BranchNameSearchLookupedit.EditValue = _currentDepartment.BranchId` và cập nhật `_branchId`
7. Set `ParentDepartmentNameTextEdit.EditValue = _currentDepartment.ParentId` và cập nhật `_parentId`

#### 3.3.7. SetDefaultValues()

```csharp
private void SetDefaultValues()
```

**Chức năng:**
- Set giá trị mặc định cho form mới

**Flow:**
1. Clear tất cả text fields
2. Set `IsActiveToogleSwitch.EditValue = true` (mặc định: Đang hoạt động)
3. **Enable mã phòng ban**: `DepartmentCodeTextEdit.Properties.ReadOnly = false`, `Enabled = true`
4. Set `_branchId = null`, `_parentId = null`

#### 3.3.8. SetupEventHandlers()

```csharp
private void SetupEventHandlers()
```

**Chức năng:**
- Đăng ký các event handlers

**Events đăng ký:**
- `SaveBarButtonItem.ItemClick += SaveBarButtonItem_ItemClick`
- `CloseBarButtonItem.ItemClick += CloseBarButtonItem_ItemClick`
- `BranchNameSearchLookupedit.EditValueChanged += BranchNameSearchLookupedit_EditValueChanged`
- `ParentDepartmentNameTextEdit.EditValueChanged += ParentDepartmentNameTextEdit_EditValueChanged`

#### 3.3.9. ValidateForm()

```csharp
private bool ValidateForm()
```

**Chức năng:**
- Validate dữ liệu form trước khi lưu

**Validation Rules:**
1. **DepartmentCode**: Chỉ validate khi tạo mới (`!_isEditMode`), không được để trống
2. **DepartmentName**: Bắt buộc, không được để trống
3. **BranchId**: Bắt buộc, `_branchId` không được null

**Return:**
- `true` nếu hợp lệ
- `false` nếu có lỗi (hiển thị lỗi qua `dxErrorProvider1`)

#### 3.3.10. SaveDepartment()

```csharp
private async Task SaveDepartment()
```

**Chức năng:**
- Lưu phòng ban vào database (Create hoặc Update)

**Flow:**
1. Gọi `GetDepartmentFromControls()` để tạo Entity
2. Nếu `_isEditMode`:
   - Gọi `_departmentBll.UpdateAsync(department)`
   - Hiển thị thông báo "Cập nhật phòng ban thành công"
3. Nếu không:
   - Gọi `_departmentBll.CreateAsync(department)`
   - Hiển thị thông báo "Tạo mới phòng ban thành công"
4. Set `DialogResult = DialogResult.OK`
5. Đóng form

#### 3.3.11. GetDepartmentFromControls()

```csharp
private Department GetDepartmentFromControls()
```

**Chức năng:**
- Tạo `Department` Entity từ dữ liệu trong controls

**Flow:**
1. Gọi `GetCompanyIdFromDatabase()` để lấy `CompanyId`
2. Tạo `Department` Entity:
   - `DepartmentCode = DepartmentCodeTextEdit.Text.Trim()`
   - `DepartmentName = DepartmentNameTextEdit.Text.Trim()`
   - `Description = DescriptionTextEdit.Text?.Trim()`
   - `IsActive = (bool)IsActiveToogleSwitch.EditValue`
   - `CompanyId = companyId` (từ database)
   - `BranchId = _branchId` (từ biến đã lưu)
   - `ParentId = _parentId` (từ biến đã lưu, có thể null)
3. Nếu `_isEditMode`: Set `Id = _departmentId`
4. Nếu không: Set `Id = Guid.NewGuid()`
5. Return Entity

**Lưu ý quan trọng:** `BranchId` và `ParentId` được lấy từ biến `_branchId` và `_parentId` (được cập nhật qua event handlers), không phải trực tiếp từ `EditValue` của controls.

#### 3.3.12. GetCompanyIdFromDatabase()

```csharp
private Guid GetCompanyIdFromDatabase()
```

**Chức năng:**
- Lấy `CompanyId` từ database (vì chỉ có 1 Company duy nhất)

**Flow:**
1. Tạo `CompanyBll` instance
2. Gọi `companyBll.GetCompany()` để lấy Company
3. Cast về `Dal.DataContext.Company` và return `Id`
4. Nếu không tìm thấy: Return `Guid.Empty`

#### 3.3.13. SetupAdvancedValidation()

```csharp
private void SetupAdvancedValidation()
```

**Chức năng:**
- Setup validation với DataAnnotations reflection

**Flow:**
1. Gọi `RequiredFieldHelper.MarkRequiredFields(this, typeof(DepartmentDto), logger)`
2. Helper sẽ:
   - Quét các properties có `[Required]` trong `DepartmentDto`
   - Tìm LayoutControlItem tương ứng
   - Thêm dấu * đỏ vào caption: `"Mã phòng ban <color=red>*</color>"`
   - Set `NullValuePrompt = "Bắt buộc nhập"` cho BaseEdit controls

#### 3.3.14. SetupSuperTips()

```csharp
private void SetupSuperTips()
```

**Chức năng:**
- Thiết lập SuperToolTip cho tất cả controls

**Flow:**
1. `SetupTextEditSuperTips()` - Setup cho TextEdit controls
2. `SetupSearchLookupSuperTips()` - Setup cho SearchLookUpEdit và TreeListLookUpEdit
3. `SetupBarButtonSuperTips()` - Setup cho BarButtonItem

#### 3.3.15. SetupTextEditSuperTips()

```csharp
private void SetupTextEditSuperTips()
```

**Chức năng:**
- Setup SuperToolTip cho các TextEdit: `DepartmentCodeTextEdit`, `DepartmentNameTextEdit`, `DescriptionTextEdit`

**Sử dụng:**
- `SuperToolTipHelper.SetTextEditSuperTip(textEdit, title, content)`

#### 3.3.16. SetupSearchLookupSuperTips()

```csharp
private void SetupSearchLookupSuperTips()
```

**Chức năng:**
- Setup SuperToolTip cho `BranchNameSearchLookupedit` và `ParentDepartmentNameTextEdit`

**Sử dụng:**
- `SuperToolTipHelper.SetBaseEditSuperTip(baseEdit, title, content)`

#### 3.3.17. SetupBarButtonSuperTips()

```csharp
private void SetupBarButtonSuperTips()
```

**Chức năng:**
- Setup SuperToolTip cho `SaveBarButtonItem` và `CloseBarButtonItem`

**Sử dụng:**
- `SuperToolTipHelper.SetBarButtonSuperTip(barButtonItem, title, content)`

### 3.4. Fields

| Field | Type | Mô Tả |
|-------|------|-------|
| `_departmentId` | `Guid` | ID phòng ban (Guid.Empty nếu tạo mới) |
| `_departmentBll` | `DepartmentBll` | BLL instance cho phòng ban |
| `_companyBranchBll` | `CompanyBranchBll` | BLL instance cho chi nhánh |
| `_currentDepartment` | `DepartmentDto` | DTO phòng ban hiện tại (khi edit) |
| `_isEditMode` | `bool` | True nếu đang ở chế độ edit |
| `_branchId` | `Guid?` | ID chi nhánh đã chọn (được cập nhật qua event handler) |
| `_parentId` | `Guid?` | ID phòng ban cha đã chọn (được cập nhật qua event handler, có thể null) |

**Lưu ý quan trọng:** `_branchId` và `_parentId` được cập nhật qua event handlers (`EditValueChanged`), không phải trực tiếp từ controls. Điều này đảm bảo giá trị luôn được đồng bộ khi người dùng thay đổi selection.

### 3.5. Constructors

#### 3.5.1. FrmDepartmentDetail() - Tạo Mới

```csharp
public FrmDepartmentDetail()
```

**Chức năng:**
- Constructor cho chế độ tạo mới

**Flow:**
1. `InitializeComponent()`
2. Set `_departmentId = Guid.Empty`
3. Khởi tạo BLL instances
4. Set `_isEditMode = false`
5. Gọi `InitializeForm()`

#### 3.5.2. FrmDepartmentDetail(Guid departmentId) - Chỉnh Sửa

```csharp
public FrmDepartmentDetail(Guid departmentId)
```

**Chức năng:**
- Constructor cho chế độ chỉnh sửa

**Flow:**
1. `InitializeComponent()`
2. Set `_departmentId = departmentId`
3. Khởi tạo BLL instances
4. Set `_isEditMode = (departmentId != Guid.Empty)`
5. Gọi `InitializeForm()`

---

## 4. Validation System

### 4.1. DXErrorProvider

**File**: `FrmDepartmentDetail.Designer.cs`

**Cấu hình:**
```csharp
this.dxErrorProvider1.ContainerControl = this;
```

**Chức năng:**
- Hiển thị biểu tượng cảnh báo màu đỏ bên cạnh controls có lỗi
- Tooltip hiển thị thông báo lỗi khi di chuột qua

**Sử dụng:**
```csharp
dxErrorProvider1.SetError(control, "Thông báo lỗi");
dxErrorProvider1.ClearErrors(); // Clear tất cả lỗi
```

### 4.2. RequiredFieldHelper

**File**: `Bll/Utils/RequiredFieldHelper.cs`

**Chức năng:**
- Quét các properties có `[Required]` trong `DepartmentDto`
- Tìm LayoutControlItem tương ứng
- Thêm dấu * đỏ vào caption: `"Mã phòng ban <color=red>*</color>"`
- Set `NullValuePrompt = "Bắt buộc nhập"` cho BaseEdit controls

**Sử dụng trong FrmDepartmentDetail:**
```csharp
RequiredFieldHelper.MarkRequiredFields(
    this, 
    typeof(DepartmentDto),
    logger: (msg, ex) => Debug.WriteLine($"{msg}: {ex?.Message}")
);
```

**Kết quả:**
- `DepartmentCodeTextEdit` → Caption: "Mã phòng ban <color=red>*</color>" (chỉ khi tạo mới)
- `DepartmentNameTextEdit` → Caption: "Tên phòng ban <color=red>*</color>"
- `BranchNameSearchLookupedit` → Không có dấu * trong DTO nhưng được validate trong form

### 4.3. DataAnnotations trong DepartmentDto

**File**: `MasterData/Company/Dto/DepartmentDto.cs`

| Property | Attributes | Validation Rule |
|----------|------------|-----------------|
| `Id` | `[Required]` | Không được để trống |
| `CompanyId` | `[Required]` | Không được để trống |
| `BranchId` | - | Không có `[Required]` (nullable) |
| `DepartmentCode` | `[Required]`, `[StringLength(50)]` | Bắt buộc, tối đa 50 ký tự |
| `DepartmentName` | `[Required]`, `[StringLength(255)]` | Bắt buộc, tối đa 255 ký tự |
| `ParentId` | - | Không có `[Required]` (nullable) |
| `Description` | `[StringLength(255)]` | Tối đa 255 ký tự (không bắt buộc) |
| `IsActive` | - | Không có validation |

### 4.4. Custom Validation trong Form

**Method**: `ValidateForm()`

**Validation Rules:**

1. **DepartmentCode** (chỉ khi tạo mới):
   ```csharp
   if (!_isEditMode && string.IsNullOrWhiteSpace(DepartmentCodeTextEdit.Text))
   {
       dxErrorProvider1.SetError(DepartmentCodeTextEdit, "Mã phòng ban không được để trống");
       isValid = false;
   }
   ```

2. **DepartmentName**:
   ```csharp
   if (string.IsNullOrWhiteSpace(DepartmentNameTextEdit.Text))
   {
       dxErrorProvider1.SetError(DepartmentNameTextEdit, "Tên phòng ban không được để trống");
       isValid = false;
   }
   ```

3. **BranchId** (custom validation, không có trong DTO):
   ```csharp
   if (_branchId == null)
   {
       dxErrorProvider1.SetError(BranchNameSearchLookupedit, "Vui lòng chọn chi nhánh");
       isValid = false;
   }
   ```

**Lưu ý:** `BranchId` không có `[Required]` trong DTO nhưng được validate trong form vì yêu cầu nghiệp vụ.

### 4.5. Validation Flow

```
User Input
  │
  ├─> DataAnnotations Check (DTO level)
  │   ├─> [Required] → Check empty
  │   ├─> [StringLength] → Check max length
  │   └─> RequiredFieldHelper → Mark required fields with red *
  │
  ├─> Custom Validation (Form level)
  │   ├─> ValidateForm() → Check DepartmentCode, DepartmentName, BranchId
  │   └─> DXErrorProvider → Display error icons
  │
  ├─> Business Logic Validation (BLL level)
  │   └─> DepartmentBll.ValidateDepartmentAsync()
  │       ├─> Check DepartmentCode uniqueness
  │       └─> Check other business rules
  │
  └─> Database Constraints
      └─> Foreign key constraints, unique constraints, etc.
```

### 4.6. Validation trong BLL

**File**: `Bll/MasterData/Company/DepartmentBll.cs`

**Method**: `ValidateDepartmentAsync()`

**Validation Rules:**
- Department không được null
- DepartmentCode không được để trống
- DepartmentName không được để trống
- (Có thể có thêm validation về uniqueness, v.v.)

---

## 5. Business Logic Flow

### 5.1. Load Flow (Sequence Diagram)

```
User                    FrmDepartmentDetail    CompanyBranchBll    DepartmentBll    Database
  │                              │                    │                  │            │
  │──Open Form──────────────────>│                    │                  │            │
  │                              │                    │                  │            │
  │                    InitializeForm()              │                  │            │
  │                              │                    │                  │            │
  │                    LoadDataSources()             │                  │            │
  │                              │                    │                  │            │
  │                    LoadCompanyBranches()         │                  │            │
  │                              │──GetActiveBranches()─────────────────>│            │
  │                              │                    │                  │            │
  │                              │<──List<CompanyBranch>─────────────────│            │
  │                              │                    │                  │            │
  │                    LoadDepartments()            │                  │            │
  │                              │                    │──GetAll()────────────────────>│
  │                              │                    │                  │            │
  │                              │                    │<──List<Department>────────────│
  │                              │                    │                  │            │
  │                    (If Edit Mode)               │                  │            │
  │                    LoadDepartmentData()         │                  │            │
  │                              │                    │──GetById(id)─────────────────>│
  │                              │                    │                  │            │
  │                              │<──Department──────────────────────────│            │
  │                              │                    │                  │            │
  │                    BindDepartmentToControls()    │                  │            │
  │                              │                    │                  │            │
  │<──Form Displayed─────────────│                    │                  │            │
```

### 5.2. Save Flow (Sequence Diagram)

```
User                    FrmDepartmentDetail    DepartmentBll    DepartmentDataAccess    Database
  │                              │                  │                      │              │
  │──Click Save──────────────────>│                  │                      │              │
  │                              │                    │                      │              │
  │                    ValidateForm()                │                      │              │
  │                              │                    │                      │              │
  │                    (If Valid)                   │                      │              │
  │                    GetDepartmentFromControls()   │                      │              │
  │                              │                    │                      │              │
  │                    GetCompanyIdFromDatabase()   │                      │              │
  │                              │                    │                      │              │
  │                    (If Edit Mode)               │                      │              │
  │                              │──UpdateAsync(dept)─>│                      │              │
  │                              │                  │                      │              │
  │                              │                  │──ValidateDepartmentAsync()──────────>│
  │                              │                  │                      │              │
  │                              │                  │──UpdateDepartmentAsync(dept)────────>│
  │                              │                  │                      │              │
  │                              │                  │<──Department──────────────────────│
  │                              │                  │                      │              │
  │                              │<──Department──────│                      │              │
  │                              │                    │                      │              │
  │                    (If Create Mode)              │                      │              │
  │                              │──CreateAsync(dept)─>│                      │              │
  │                              │                  │                      │              │
  │                              │                  │──ValidateDepartmentAsync()──────────>│
  │                              │                  │                      │              │
  │                              │                  │──CreateAsync(dept)─────────────────>│
  │                              │                  │                      │              │
  │                              │                  │<──Department──────────────────────│
  │                              │                  │                      │              │
  │                              │<──Department──────│                      │              │
  │                              │                    │                      │              │
  │                    Show Success Message          │                      │              │
  │                              │                    │                      │              │
  │                    Close Form                    │                      │              │
  │<──Form Closed─────────────────│                    │                      │              │
```

### 5.3. EditValueChanged Flow

```
User                    FrmDepartmentDetail    Controls
  │                              │                  │
  │──Select Branch───────────────>│                  │
  │                              │                    │
  │                    BranchNameSearchLookupedit_EditValueChanged()
  │                              │                    │
  │                    Parse EditValue to Guid       │
  │                              │                    │
  │                    Update _branchId variable    │
  │                              │                    │
  │                    (Similar for ParentDepartment)
  │                              │                    │
  │                    _branchId, _parentId stored  │
  │                    (Used later in GetDepartmentFromControls())
```

**Lưu ý:** `_branchId` và `_parentId` được lưu trong biến và sử dụng trong `GetDepartmentFromControls()`, không phải trực tiếp từ `EditValue` của controls. Điều này đảm bảo giá trị luôn được đồng bộ.

---

## 6. Error Handling

### 6.1. Try-Catch Blocks

Form sử dụng try-catch blocks ở các method quan trọng:

#### 6.1.1. InitializeForm()

```csharp
try
{
    LoadDataSources();
    // ... setup form
}
catch (Exception ex)
{
    XtraMessageBox.Show($"Lỗi khởi tạo form: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
}
```

#### 6.1.2. LoadDataSources()

```csharp
try
{
    LoadCompanyBranches();
    LoadDepartments();
}
catch (Exception ex)
{
    XtraMessageBox.Show($"Lỗi load datasource: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
}
```

#### 6.1.3. LoadDepartmentData()

```csharp
try
{
    var department = _departmentBll.GetById(_departmentId);
    if (department == null)
    {
        XtraMessageBox.Show("Không tìm thấy phòng ban", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        Close();
        return;
    }
    // ... bind data
}
catch (Exception ex)
{
    XtraMessageBox.Show($"Lỗi load dữ liệu phòng ban: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
}
```

#### 6.1.4. SaveBarButtonItem_ItemClick()

```csharp
try
{
    if (ValidateForm())
    {
        await SaveDepartment();
    }
}
catch (Exception ex)
{
    XtraMessageBox.Show($"Lỗi lưu phòng ban: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
}
```

#### 6.1.5. SaveDepartment()

```csharp
try
{
    var department = GetDepartmentFromControls();
    if (_isEditMode)
    {
        await _departmentBll.UpdateAsync(department);
        XtraMessageBox.Show("Cập nhật phòng ban thành công", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
    else
    {
        await _departmentBll.CreateAsync(department);
        XtraMessageBox.Show("Tạo mới phòng ban thành công", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
    DialogResult = DialogResult.OK;
    Close();
}
catch (Exception ex)
{
    throw new Exception($"Lỗi lưu phòng ban: {ex.Message}", ex);
}
```

### 6.2. Error Messages

**User-Friendly Messages:**
- "Lỗi khởi tạo form: {ex.Message}"
- "Lỗi load datasource: {ex.Message}"
- "Lỗi load dữ liệu phòng ban: {ex.Message}"
- "Không tìm thấy phòng ban"
- "Lỗi lưu phòng ban: {ex.Message}"
- "Cập nhật phòng ban thành công"
- "Tạo mới phòng ban thành công"

### 6.3. Logging

**Debug Logging:**
- Sử dụng `Debug.WriteLine()` để log các thông tin debug:
  - `Debug.WriteLine($"BranchId updated to: {branchId}")`
  - `Debug.WriteLine($"GetDepartmentFromControls - _branchId: {_branchId}")`
  - `Debug.WriteLine($"Before UpdateAsync - Department.BranchId: {department.BranchId}")`

**Logger trong RequiredFieldHelper:**
```csharp
RequiredFieldHelper.MarkRequiredFields(
    this, 
    typeof(DepartmentDto),
    logger: (msg, ex) => Debug.WriteLine($"{msg}: {ex?.Message}")
);
```

### 6.4. Exception Propagation

**Pattern:**
- Catch exception ở method level
- Wrap exception với thông báo rõ ràng
- Throw lại để UI layer xử lý (hiển thị MessageBox)

**Ví dụ:**
```csharp
catch (Exception ex)
{
    throw new Exception($"Lỗi lưu phòng ban: {ex.Message}", ex);
}
```

---

## 7. Security

### 7.1. Không Có Chức Năng Bảo Mật Đặc Biệt

Form này **không có** các chức năng bảo mật như:
- Remember Me
- Password encryption
- Session management
- Authentication/Authorization

### 7.2. Quyền Truy Cập

Quyền truy cập được quản lý ở **cấp hệ thống** (không phải trong form này).

### 7.3. Data Validation

Form có validation để đảm bảo dữ liệu hợp lệ:
- Required fields validation
- String length validation
- Business rule validation (trong BLL)

### 7.4. SQL Injection Prevention

SQL injection được ngăn chặn ở **DAL layer** (sử dụng parameterized queries, Entity Framework, v.v.), không phải trong form này.

---

## 8. Extensibility Guide

### 8.1. Thêm Trường Mới

**Bước 1:** Thêm property vào `DepartmentDto`:
```csharp
[DisplayName("Trường mới")]
[StringLength(100)]
public string NewField { get; set; }
```

**Bước 2:** Thêm control vào Designer:
- Thêm `TextEdit` vào `dataLayoutControl1`
- Thêm `LayoutControlItem` tương ứng

**Bước 3:** Cập nhật code:
- `BindDepartmentToControls()`: Bind dữ liệu vào control mới
- `SetDefaultValues()`: Set giá trị mặc định
- `GetDepartmentFromControls()`: Lấy giá trị từ control và set vào Entity
- `SetupTextEditSuperTips()`: Thêm SuperToolTip cho control mới

### 8.2. Thêm Validation Mới

**Bước 1:** Thêm validation rule vào `ValidateForm()`:
```csharp
if (string.IsNullOrWhiteSpace(NewFieldTextEdit.Text))
{
    dxErrorProvider1.SetError(NewFieldTextEdit, "Trường mới không được để trống");
    isValid = false;
}
```

**Bước 2:** (Tùy chọn) Thêm DataAnnotation vào `DepartmentDto`:
```csharp
[Required(ErrorMessage = "Trường mới không được để trống")]
public string NewField { get; set; }
```

### 8.3. Thêm Event Handler Mới

**Bước 1:** Tạo method event handler:
```csharp
private void NewControl_EditValueChanged(object sender, EventArgs e)
{
    try
    {
        // Xử lý logic
    }
    catch (Exception ex)
    {
        Debug.WriteLine($"Lỗi: {ex.Message}");
    }
}
```

**Bước 2:** Đăng ký event trong `SetupEventHandlers()`:
```csharp
NewControl.EditValueChanged += NewControl_EditValueChanged;
```

### 8.4. Async/Await Best Practices

**Hiện tại:**
- `SaveDepartment()` là async method
- `SaveBarButtonItem_ItemClick()` là async void (acceptable cho event handler)

**Cải thiện:**
- Có thể thêm loading indicator khi save
- Có thể disable controls khi đang save để tránh double-click

**Ví dụ:**
```csharp
private async Task SaveDepartment()
{
    try
    {
        // Disable controls
        SaveBarButtonItem.Enabled = false;
        dataLayoutControl1.Enabled = false;
        
        // Show loading indicator
        // ...
        
        // Save
        await _departmentBll.CreateAsync(department);
        
        // Enable controls
        SaveBarButtonItem.Enabled = true;
        dataLayoutControl1.Enabled = true;
    }
    catch (Exception ex)
    {
        // Re-enable controls on error
        SaveBarButtonItem.Enabled = true;
        dataLayoutControl1.Enabled = true;
        throw;
    }
}
```

### 8.5. Clean Code Patterns

**Hiện tại form đã áp dụng:**
- ✅ Separation of concerns (UI, BLL, DAL)
- ✅ Single Responsibility Principle (mỗi method có một nhiệm vụ)
- ✅ DRY (Don't Repeat Yourself) - Sử dụng helper classes
- ✅ Error handling với try-catch
- ✅ Validation ở nhiều layer

**Có thể cải thiện:**
- ⚠️ Có thể tách logic validation thành class riêng
- ⚠️ Có thể sử dụng Command Pattern cho Save/Close actions
- ⚠️ Có thể sử dụng MVVM pattern (nếu cần)

### 8.6. Dependency Injection

**Hiện tại:**
- BLL instances được tạo trực tiếp trong constructor:
  ```csharp
  _departmentBll = new DepartmentBll();
  _companyBranchBll = new CompanyBranchBll();
  ```

**Cải thiện:**
- Có thể sử dụng Dependency Injection:
  ```csharp
  public FrmDepartmentDetail(DepartmentBll departmentBll, CompanyBranchBll companyBranchBll)
  {
      _departmentBll = departmentBll;
      _companyBranchBll = companyBranchBll;
  }
  ```

---

## 9. Test Checklist

### 9.1. Unit Test Cases

#### 9.1.1. Constructor Tests

- [ ] Test constructor không tham số (tạo mới):
  - `_departmentId` phải là `Guid.Empty`
  - `_isEditMode` phải là `false`
  - Form title phải là "Thêm mới phòng ban"

- [ ] Test constructor có tham số (chỉnh sửa):
  - `_departmentId` phải bằng tham số
  - `_isEditMode` phải là `true` nếu `departmentId != Guid.Empty`
  - Form title phải là "Chỉnh sửa phòng ban"

#### 9.1.2. LoadDataSources Tests

- [ ] Test `LoadCompanyBranches()`:
  - Phải load danh sách chi nhánh đang hoạt động
  - `companyBranchDtoBindingSource.DataSource` phải không null

- [ ] Test `LoadDepartments()`:
  - Phải load danh sách phòng ban
  - `departmentDtoBindingSource.DataSource` phải không null

#### 9.1.3. LoadDepartmentData Tests

- [ ] Test load thành công:
  - Phải gọi `_departmentBll.GetById()`
  - Phải bind dữ liệu vào controls
  - Mã phòng ban phải bị khóa

- [ ] Test không tìm thấy phòng ban:
  - Phải hiển thị thông báo lỗi
  - Form phải đóng

#### 9.1.4. ValidateForm Tests

- [ ] Test validation thành công:
  - Tất cả trường bắt buộc đã nhập
  - Return `true`

- [ ] Test validation fail - thiếu mã phòng ban (tạo mới):
  - Return `false`
  - `dxErrorProvider1` phải có lỗi cho `DepartmentCodeTextEdit`

- [ ] Test validation fail - thiếu tên phòng ban:
  - Return `false`
  - `dxErrorProvider1` phải có lỗi cho `DepartmentNameTextEdit`

- [ ] Test validation fail - thiếu chi nhánh:
  - Return `false`
  - `dxErrorProvider1` phải có lỗi cho `BranchNameSearchLookupedit`

#### 9.1.5. GetDepartmentFromControls Tests

- [ ] Test tạo Entity từ controls:
  - Entity phải có đầy đủ thông tin từ controls
  - `BranchId` phải bằng `_branchId`
  - `ParentId` phải bằng `_parentId` (có thể null)
  - `CompanyId` phải được lấy từ database

- [ ] Test khi tạo mới:
  - `Id` phải là `Guid.NewGuid()`

- [ ] Test khi chỉnh sửa:
  - `Id` phải bằng `_departmentId`

#### 9.1.6. SaveDepartment Tests

- [ ] Test save thành công (tạo mới):
  - Phải gọi `_departmentBll.CreateAsync()`
  - Phải hiển thị thông báo "Tạo mới phòng ban thành công"
  - `DialogResult` phải là `DialogResult.OK`
  - Form phải đóng

- [ ] Test save thành công (chỉnh sửa):
  - Phải gọi `_departmentBll.UpdateAsync()`
  - Phải hiển thị thông báo "Cập nhật phòng ban thành công"
  - `DialogResult` phải là `DialogResult.OK`
  - Form phải đóng

- [ ] Test save fail:
  - Phải hiển thị thông báo lỗi
  - Form không đóng

#### 9.1.7. Event Handler Tests

- [ ] Test `BranchNameSearchLookupedit_EditValueChanged`:
  - Khi chọn chi nhánh: `_branchId` phải được cập nhật
  - Khi clear: `_branchId` phải là `null`

- [ ] Test `ParentDepartmentNameTextEdit_EditValueChanged`:
  - Khi chọn phòng ban cha: `_parentId` phải được cập nhật
  - Khi clear: `_parentId` phải là `null`

### 9.2. Manual Testing Scenarios

#### 9.2.1. Tạo Mới Phòng Ban

1. [ ] Mở form ở chế độ tạo mới
2. [ ] Kiểm tra form title: "Thêm mới phòng ban"
3. [ ] Kiểm tra mã phòng ban có thể nhập (không bị khóa)
4. [ ] Nhập đầy đủ thông tin bắt buộc:
   - Mã phòng ban: "PB01"
   - Tên phòng ban: "Phòng Kinh doanh"
   - Chi nhánh: Chọn một chi nhánh
5. [ ] Click nút Lưu
6. [ ] Kiểm tra thông báo thành công
7. [ ] Kiểm tra form đóng
8. [ ] Kiểm tra dữ liệu đã được lưu vào database

#### 9.2.2. Chỉnh Sửa Phòng Ban

1. [ ] Mở form với ID phòng ban hợp lệ
2. [ ] Kiểm tra form title: "Chỉnh sửa phòng ban"
3. [ ] Kiểm tra mã phòng ban bị khóa (không thể chỉnh sửa)
4. [ ] Kiểm tra dữ liệu hiện tại được load đúng
5. [ ] Chỉnh sửa tên phòng ban
6. [ ] Click nút Lưu
7. [ ] Kiểm tra thông báo thành công
8. [ ] Kiểm tra form đóng
9. [ ] Kiểm tra dữ liệu đã được cập nhật trong database

#### 9.2.3. Validation Tests

1. [ ] Test không nhập mã phòng ban (tạo mới):
   - Click Lưu
   - Kiểm tra hiển thị lỗi "Mã phòng ban không được để trống"

2. [ ] Test không nhập tên phòng ban:
   - Click Lưu
   - Kiểm tra hiển thị lỗi "Tên phòng ban không được để trống"

3. [ ] Test không chọn chi nhánh:
   - Click Lưu
   - Kiểm tra hiển thị lỗi "Vui lòng chọn chi nhánh"

4. [ ] Test nhập mã phòng ban quá dài (>50 ký tự):
   - Click Lưu
   - Kiểm tra validation (có thể ở BLL level)

5. [ ] Test nhập tên phòng ban quá dài (>255 ký tự):
   - Click Lưu
   - Kiểm tra validation (có thể ở BLL level)

#### 9.2.4. UI Tests

1. [ ] Kiểm tra SuperToolTip hiển thị đúng khi di chuột qua controls
2. [ ] Kiểm tra dấu * đỏ hiển thị đúng ở các trường bắt buộc
3. [ ] Kiểm tra ToggleSwitch hoạt động đúng (bật/tắt trạng thái)
4. [ ] Kiểm tra SearchLookUpEdit hiển thị danh sách chi nhánh
5. [ ] Kiểm tra TreeListLookUpEdit hiển thị cây phòng ban
6. [ ] Kiểm tra form không cho phép resize (FixedToolWindow)

#### 9.2.5. Error Handling Tests

1. [ ] Test lỗi khi load datasource:
   - Simulate lỗi database
   - Kiểm tra hiển thị thông báo lỗi

2. [ ] Test lỗi khi load phòng ban không tồn tại:
   - Mở form với ID không tồn tại
   - Kiểm tra hiển thị thông báo "Không tìm thấy phòng ban"
   - Kiểm tra form đóng

3. [ ] Test lỗi khi save:
   - Simulate lỗi database khi save
   - Kiểm tra hiển thị thông báo lỗi
   - Kiểm tra form không đóng

#### 9.2.6. Integration Tests

1. [ ] Test tạo phòng ban và kiểm tra hiển thị trong form danh sách
2. [ ] Test chỉnh sửa phòng ban và kiểm tra cập nhật trong form danh sách
3. [ ] Test chọn phòng ban cha và kiểm tra cấu trúc phân cấp
4. [ ] Test chọn chi nhánh và kiểm tra phòng ban thuộc đúng chi nhánh

---

## 10. Changelog Template

### Version 1.0 (2025-01-15)

**Initial Release**

#### Added
- Form `FrmDepartmentDetail` để tạo mới và chỉnh sửa phòng ban
- Validation với DXErrorProvider và RequiredFieldHelper
- SuperToolTip cho tất cả controls
- Support cho cấu trúc phân cấp phòng ban (parent-child)
- Support cho gán phòng ban vào chi nhánh
- Async/await cho save operations
- Error handling với try-catch và user-friendly messages

#### Features
- Tạo mới phòng ban với đầy đủ thông tin
- Chỉnh sửa phòng ban (trừ mã phòng ban)
- Validation đầy đủ (form level, DTO level, BLL level)
- Hiển thị danh sách chi nhánh đang hoạt động
- Hiển thị cây phòng ban để chọn phòng ban cha
- ToggleSwitch để bật/tắt trạng thái hoạt động

#### Known Issues
- (Không có)

#### Future Improvements
- [ ] Thêm loading indicator khi save
- [ ] Thêm confirmation dialog trước khi đóng form nếu có thay đổi chưa lưu
- [ ] Thêm validation để ngăn chặn chọn chính phòng ban đó làm phòng ban cha
- [ ] Thêm filter/search cho danh sách chi nhánh và phòng ban
- [ ] Thêm support cho Dependency Injection
- [ ] Thêm unit tests

---

**📝 Lưu ý**: Tài liệu này được tạo tự động dựa trên source code. Nếu bạn phát hiện thông tin không chính xác hoặc cần cập nhật, vui lòng chỉnh sửa và cập nhật version number.

