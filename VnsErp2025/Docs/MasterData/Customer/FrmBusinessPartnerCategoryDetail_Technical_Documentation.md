# FrmBusinessPartnerCategoryDetail - Tài Liệu Kỹ Thuật

## 1. Mục Đích Của Class

`FrmBusinessPartnerCategoryDetail` là một Windows Forms form (kế thừa từ `XtraForm` của DevExpress) được thiết kế để quản lý thông tin chi tiết của danh mục đối tác (Business Partner Category).

### Chức Năng Chính:
- **Thêm mới**: Tạo danh mục đối tác mới với đầy đủ thông tin
- **Chỉnh sửa**: Cập nhật thông tin của danh mục đối tác đã tồn tại
- **Validation**: Kiểm tra tính hợp lệ của dữ liệu đầu vào
- **Quản lý phân cấp**: Hỗ trợ chọn danh mục cha để tạo cấu trúc phân cấp
- **Xử lý lỗi**: Hiển thị thông báo lỗi và validation errors

### Đặc Điểm:
- Form modal (dialog) - chặn tương tác với form cha
- Hỗ trợ 2 chế độ: **Thêm mới** (`Guid.Empty`) và **Chỉnh sửa** (có `categoryId`)
- Validation real-time với `DXErrorProvider`
- Hỗ trợ phím tắt (Ctrl+S để lưu, Escape để hủy)

---

## 2. Vai Trò Trong Kiến Trúc

### **Vị Trí: UI Layer (Presentation Layer)**

Form này nằm ở tầng **UI (User Interface)** trong kiến trúc 3-layer của ứng dụng:

```
┌─────────────────────────────────────────┐
│  UI Layer (Presentation)                │
│  ┌───────────────────────────────────┐ │
│  │ FrmBusinessPartnerCategoryDetail  │ │ ← Class này
│  │ - XtraForm (DevExpress)          │ │
│  │ - Data Entry Form                │ │
│  │ - Validation Logic               │ │
│  └───────────────────────────────────┘ │
└─────────────────────────────────────────┘
              │
              │ Gọi methods
              ▼
┌─────────────────────────────────────────┐
│  BLL Layer (Business Logic)             │
│  ┌───────────────────────────────────┐ │
│  │ BusinessPartnerCategoryBll       │ │
│  │ - GetById()                      │ │
│  │ - GetCategoriesWithCounts()       │ │
│  │ - IsCategoryNameExists()         │ │
│  │ - Insert()                       │ │
│  │ - Update()                       │ │
│  └───────────────────────────────────┘ │
└─────────────────────────────────────────┘
              │
              │ Sử dụng
              ▼
┌─────────────────────────────────────────┐
│  DAL Layer (Data Access)                │
│  ┌───────────────────────────────────┐ │
│  │ Repository / DataContext           │ │
│  │ - Database Operations             │ │
│  └───────────────────────────────────┘ │
└─────────────────────────────────────────┘
```

### **Dependencies:**
- **BLL Layer**: `BusinessPartnerCategoryBll` - Xử lý business logic
- **DTO Layer**: `BusinessPartnerCategoryDto` - Data Transfer Object
- **Common Utilities**: 
  - `RequiredFieldHelper` - Đánh dấu các trường bắt buộc
  - `SuperToolTipHelper` - Tooltip hỗ trợ
  - `MsgBox` - Hiển thị thông báo
- **UI Framework**: DevExpress WinForms controls
  - `DXErrorProvider` - Validation error display
  - `DataLayoutControl` - Layout management
  - `SearchLookUpEdit` - Dropdown với search

### **Không Trực Tiếp Truy Cập:**
- ❌ Database (không gọi DAL trực tiếp)
- ❌ Domain entities (chỉ làm việc với DTO)

---

## 3. Giải Thích Các Method Chính

### 3.1. Constructor & Initialization

#### `FrmBusinessPartnerCategoryDetail(Guid categoryId)`
```csharp
public FrmBusinessPartnerCategoryDetail(Guid categoryId)
```
**Mục đích**: Khởi tạo form với chế độ thêm mới hoặc chỉnh sửa.

**Tham số**:
- `categoryId`: 
  - `Guid.Empty` → Chế độ **Thêm mới**
  - Có giá trị → Chế độ **Chỉnh sửa**

**Luồng xử lý**:
1. Gọi `InitializeComponent()` (Designer-generated)
2. Lưu `categoryId` vào `_categoryId`
3. Gọi `InitializeForm()` để setup form

**Property**: `IsEditMode` (computed property)
```csharp
private bool IsEditMode => _categoryId != Guid.Empty;
```

---

#### `InitializeForm()`
```csharp
private void InitializeForm()
```
**Mục đích**: Khởi tạo form và load dữ liệu nếu cần.

**Luồng xử lý**:
1. **Thiết lập tiêu đề**: 
   - Edit mode: "Điều chỉnh danh mục đối tác"
   - New mode: "Thêm mới danh mục đối tác"
2. **Đánh dấu trường bắt buộc**: 
   - Sử dụng `RequiredFieldHelper.MarkRequiredFields()` với `DataAnnotations` từ DTO
3. **Load danh sách danh mục cha**: `LoadParentCategories()`
4. **Setup tooltips**: `SetupSuperToolTips()`
5. **Load dữ liệu nếu edit mode**: `LoadCategoryData()`
6. **Set focus**: Focus vào `CategoryCodeTextEdit`

**Lưu ý**: 
- `RequiredFieldHelper` tự động đọc `[Required]` attributes từ DTO
- Focus được set vào control đầu tiên để cải thiện UX

---

### 3.2. Data Loading

#### `LoadParentCategories()`
```csharp
private void LoadParentCategories()
```
**Mục đích**: Load danh sách danh mục cha vào `SearchLookUpEdit` để người dùng chọn.

**Luồng xử lý**:
1. **Lấy dữ liệu từ BLL**:
   ```csharp
   var (categories, counts) = _businessPartnerCategoryBll.GetCategoriesWithCounts();
   ```
2. **Chuyển đổi sang DTO**:
   - Convert entities sang DTOs với `ToDtoWithCount()`
   - Tính toán `Level` (độ sâu trong cây phân cấp)
   - Tính toán `FullPath` (đường dẫn đầy đủ: "Root > Parent > Child")
   - Lấy `ParentCategoryName`
3. **Loại bỏ circular reference**:
   - Nếu đang edit mode, loại bỏ category hiện tại khỏi danh sách parent
   - Tránh chọn chính nó làm parent
4. **Bind vào BindingSource**:
   ```csharp
   businessPartnerCategoryDtoBindingSource.DataSource = dtoList;
   ```
5. **Cấu hình SearchLookUpEdit**:
   - `ValueMember = "Id"` (lưu Guid)
   - `DisplayMember = "FullPathHtml"` (hiển thị đường dẫn HTML)
   - `PopupView = parentCategoryGridView` (GridView trong popup)
6. **Cấu hình GridView**:
   - Tắt group panel, indicator
   - Sắp xếp theo `FullPathHtml` (Ascending)
7. **Đăng ký event**: `EditValueChanged` để xử lý khi giá trị thay đổi

**Thuật toán tính Level**:
```csharp
int level = 0;
var current = entity;
while (current.ParentId.HasValue && entityDict.ContainsKey(current.ParentId.Value))
{
    level++;
    current = entityDict[current.ParentId.Value];
    if (level > 10) break; // Guard tránh infinite loop
}
```

**Thuật toán tính FullPath**:
```csharp
var pathParts = new List<string> { entity.CategoryName };
current = entity;
while (current.ParentId.HasValue && entityDict.ContainsKey(current.ParentId.Value))
{
    current = entityDict[current.ParentId.Value];
    pathParts.Insert(0, current.CategoryName);
    if (pathParts.Count > 10) break; // Guard
}
dto.FullPath = string.Join(" > ", pathParts);
```

**Lưu ý**: 
- Có guard để tránh infinite loop (max 10 levels)
- Loại bỏ category hiện tại để tránh circular reference

---

#### `LoadCategoryData()`
```csharp
private void LoadCategoryData()
```
**Mục đích**: Load dữ liệu danh mục để chỉnh sửa (chỉ gọi trong edit mode).

**Luồng xử lý**:
1. **Lấy category từ BLL**:
   ```csharp
   var category = _businessPartnerCategoryBll.GetById(_categoryId);
   ```
2. **Validation**: Kiểm tra category có tồn tại không
   - Nếu null → Hiển thị lỗi và đóng form với `DialogResult.Cancel`
3. **Convert sang DTO**: `category.ToDto()`
4. **Bind vào controls**: `BindDataToControls(dto)`

**Lưu ý**: 
- Chỉ gọi khi `IsEditMode == true`
- Nếu không tìm thấy category, form sẽ tự động đóng

---

#### `BindDataToControls(BusinessPartnerCategoryDto dto)`
```csharp
private void BindDataToControls(BusinessPartnerCategoryDto dto)
```
**Mục đích**: Bind dữ liệu từ DTO vào các controls trên form.

**Mapping**:
- `CategoryCodeTextEdit.Text` ← `dto.CategoryCode`
- `CategoryNameTextEdit.Text` ← `dto.CategoryName`
- `DescriptionMemoEdit.Text` ← `dto.Description`
- `IsActiveToogleSwitch.IsOn` ← `dto.IsActive`
- `ParentCategorySearchLookUpEdit.EditValue` ← `dto.ParentId` (nếu có)

**Lưu ý**: 
- `ParentId` có thể null → Set `EditValue = null`
- `EditValue` của `SearchLookUpEdit` phải là `Guid` (ValueMember)

---

### 3.3. Data Retrieval

#### `GetDataFromControls()`
```csharp
private BusinessPartnerCategoryDto GetDataFromControls()
```
**Mục đích**: Lấy dữ liệu từ các controls và tạo DTO.

**Luồng xử lý**:
1. **Tạo DTO mới** với các giá trị từ controls:
   - `Id = _categoryId` (Guid.Empty nếu thêm mới)
   - `CategoryCode = CategoryCodeTextEdit.Text.Trim()`
   - `CategoryName = CategoryNameTextEdit.Text.Trim()`
   - `Description = DescriptionMemoEdit.Text.Trim()`
   - `IsActive = IsActiveToogleSwitch.IsOn`
2. **Xử lý ParentId** (phức tạp hơn):
   - Lấy `EditValue` từ `SearchLookUpEdit`
   - Xử lý nhiều kiểu dữ liệu:
     - `Guid` → Sử dụng trực tiếp
     - `string` → Parse thành `Guid`
     - Object khác → Convert hoặc lấy từ selected row
   - Nếu không parse được → Lấy từ `GetFocusedRow()` trong GridView
   - Nếu vẫn không có → `ParentId = null`

**Lưu ý quan trọng**: 
- `EditValue` của `SearchLookUpEdit` có thể trả về nhiều kiểu dữ liệu
- Cần xử lý cẩn thận để tránh lỗi type conversion
- Có fallback logic để lấy từ selected row nếu cần

**Code pattern**:
```csharp
var editValue = ParentCategorySearchLookUpEdit.EditValue;
if (editValue != null && editValue != DBNull.Value)
{
    if (editValue is Guid guidValue)
        dto.ParentId = guidValue;
    else if (editValue is string stringValue && Guid.TryParse(stringValue, out var parsedGuid))
        dto.ParentId = parsedGuid;
    else
    {
        // Fallback: Lấy từ selected row
        var selectedRow = parentCategoryGridView.GetFocusedRow() as BusinessPartnerCategoryDto;
        dto.ParentId = selectedRow?.Id;
    }
}
```

---

### 3.4. Data Saving

#### `SaveCategory()`
```csharp
private void SaveCategory()
```
**Mục đích**: Lưu dữ liệu danh mục vào database.

**Luồng xử lý**:
1. **Lấy dữ liệu từ controls**: `GetDataFromControls()`
2. **Convert DTO sang Entity**: `dto.ToEntity()`
3. **Lưu vào database**:
   - **Edit mode**: `_businessPartnerCategoryBll.Update(entity)`
   - **New mode**: `_businessPartnerCategoryBll.Insert(entity)`
4. **Hiển thị thông báo thành công**
5. **Đóng form**: 
   - `DialogResult = DialogResult.OK`
   - `Close()`

**Lưu ý**: 
- Form cha sẽ nhận `DialogResult.OK` và reload dữ liệu
- Nếu có lỗi, sẽ hiển thị exception và không đóng form

---

### 3.5. Validation

#### `ValidateInput()`
```csharp
private bool ValidateInput()
```
**Mục đích**: Validate dữ liệu đầu vào trước khi lưu.

**Các validation rules**:

1. **CategoryName bắt buộc**:
   ```csharp
   if (string.IsNullOrWhiteSpace(CategoryNameTextEdit?.Text))
   {
       dxErrorProvider1.SetError(CategoryNameTextEdit, "Tên phân loại không được để trống", ErrorType.Critical);
       return false;
   }
   ```

2. **CategoryName độ dài tối đa**: 100 ký tự

3. **CategoryName không trùng lặp**:
   ```csharp
   if (_businessPartnerCategoryBll.IsCategoryNameExists(categoryName, _categoryId))
   {
       // Error: "Tên phân loại đã tồn tại trong hệ thống"
   }
   ```
   - `_categoryId` được truyền để exclude bản ghi hiện tại (khi edit)

4. **CategoryCode độ dài tối đa**: 50 ký tự (nếu có)

5. **Description độ dài tối đa**: 255 ký tự (nếu có)

6. **Circular Reference Check** (chỉ trong edit mode):
   - Không cho phép chọn chính nó làm parent
   - Không cho phép chọn con của nó làm parent
   - Thuật toán: Duyệt từ selected parent lên root, kiểm tra có gặp `_categoryId` không

**Thuật toán kiểm tra circular reference**:
```csharp
var allCategories = _businessPartnerCategoryBll.GetAll();
var categoryDict = allCategories.ToDictionary(c => c.Id);
var current = allCategories.FirstOrDefault(c => c.Id == selectedParentId);

while (current != null && current.ParentId.HasValue)
{
    if (current.ParentId.Value == _categoryId)
    {
        // Error: "Không thể chọn danh mục con của danh mục này làm danh mục cha"
        return false;
    }
    current = categoryDict.ContainsKey(current.ParentId.Value) 
        ? categoryDict[current.ParentId.Value] 
        : null;
}
```

**DXErrorProvider**:
- Sử dụng `dxErrorProvider1.SetError()` để hiển thị lỗi
- `ErrorType.Critical` → Hiển thị icon đỏ
- Tự động focus vào control có lỗi
- Clear errors trước khi validate: `dxErrorProvider1.ClearErrors()`

**Return**: 
- `true` → Dữ liệu hợp lệ
- `false` → Có lỗi, hiển thị error provider

---

### 3.6. Event Handlers

#### `SaveBarButtonItem_ItemClick()`
```csharp
private void SaveBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
```
**Mục đích**: Xử lý sự kiện click button "Lưu".

**Luồng xử lý**:
1. Validate input: `ValidateInput()`
2. Nếu hợp lệ → `SaveCategory()`

**Lưu ý**: Chỉ lưu khi validation pass

---

#### `CancelBarButtonItem_ItemClick()`
```csharp
private void CancelBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
```
**Mục đích**: Xử lý sự kiện click button "Đóng" hoặc "Hủy".

**Luồng xử lý**:
1. Set `DialogResult = DialogResult.Cancel`
2. `Close()` form

**Lưu ý**: Form cha sẽ nhận `DialogResult.Cancel` và không reload dữ liệu

---

#### `ProcessCmdKey()`
```csharp
protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
```
**Mục đích**: Xử lý phím tắt cho form.

**Phím tắt hỗ trợ**:
- **Ctrl+S**: Lưu (gọi `SaveBarButtonItem_ItemClick()`)
- **Escape**: Hủy (gọi `CancelBarButtonItem_ItemClick()`)

**Return**: 
- `true` → Đã xử lý phím tắt
- `false` → Không xử lý, gọi base method

---

#### `ParentCategorySearchLookUpEdit_EditValueChanged()`
```csharp
private void ParentCategorySearchLookUpEdit_EditValueChanged(object sender, EventArgs e)
```
**Mục đích**: Xử lý sự kiện thay đổi giá trị của `SearchLookUpEdit`.

**Luồng xử lý**:
1. Lấy `EditValue` từ control
2. Parse thành `Guid?` (tương tự logic trong `GetDataFromControls()`)
3. Nếu không parse được → Lấy từ selected row và set lại `EditValue`
4. Debug log để kiểm tra

**Lưu ý**: 
- Có try-catch để không làm gián đoạn user
- Chỉ log debug, không throw exception
- Đảm bảo `EditValue` luôn là `Guid` hoặc `null`

---

### 3.7. Utility Methods

#### `SetupSuperToolTips()`
```csharp
private void SetupSuperToolTips()
```
**Mục đích**: Thiết lập SuperToolTip cho các controls.

**Tooltips được thiết lập**:
- 🔖 Mã danh mục
- 📋 Tên phân loại (bắt buộc)
- 🌳 Danh mục cha
- 📝 Mô tả
- ✅ Trạng thái hoạt động
- 💾 Lưu
- ❌ Đóng

**Lưu ý**: Có try-catch để không chặn form nếu setup tooltip lỗi

---

#### `ShowInfo()`, `ShowError()`
```csharp
private void ShowInfo(string message)
private void ShowError(string message)
private void ShowError(Exception ex, string context = null)
```
**Mục đích**: Helper methods để hiển thị thông báo.

**Sử dụng**:
- `ShowInfo()` → Thông báo thành công
- `ShowError(string)` → Thông báo lỗi đơn giản
- `ShowError(Exception, string)` → Thông báo lỗi với exception và context

---

## 4. Luồng Xử Lý Dữ Liệu

### 4.1. Luồng Thêm Mới

```
User clicks "Mới" button in parent form
         │
         ▼
new FrmBusinessPartnerCategoryDetail(Guid.Empty)
         │
         ├─> Constructor
         │   ├─> InitializeComponent()
         │   ├─> _categoryId = Guid.Empty
         │   └─> InitializeForm()
         │
         ├─> InitializeForm()
         │   ├─> Set title: "Thêm mới danh mục đối tác"
         │   ├─> MarkRequiredFields() [DataAnnotations]
         │   ├─> LoadParentCategories()
         │   │   │
         │   │   ├─> BLL.GetCategoriesWithCounts()
         │   │   ├─> Convert to DTOs with Level/FullPath
         │   │   ├─> Bind to BindingSource
         │   │   └─> Configure SearchLookUpEdit
         │   │
         │   ├─> SetupSuperToolTips()
         │   └─> Focus CategoryCodeTextEdit
         │
         ▼
Form displays (empty fields)
         │
         ▼
User enters data
         │
         ▼
User clicks "Lưu" (or Ctrl+S)
         │
         ▼
SaveBarButtonItem_ItemClick()
         │
         ├─> ValidateInput()
         │   │
         │   ├─> Check CategoryName required
         │   ├─> Check CategoryName length (max 100)
         │   ├─> Check CategoryName unique
         │   ├─> Check CategoryCode length (max 50)
         │   ├─> Check Description length (max 255)
         │   └─> Return true/false
         │
         ├─> If valid → SaveCategory()
         │   │
         │   ├─> GetDataFromControls()
         │   │   │
         │   │   ├─> Read values from controls
         │   │   ├─> Parse ParentId from SearchLookUpEdit
         │   │   └─> Return DTO
         │   │
         │   ├─> dto.ToEntity()
         │   │
         │   ├─> BLL.Insert(entity)
         │   │   │
         │   │   └─> [BLL] → [DAL] → Database INSERT
         │   │
         │   ├─> ShowInfo("Thêm mới thành công!")
         │   ├─> DialogResult = DialogResult.OK
         │   └─> Close()
         │
         └─> If invalid → Show errors via DXErrorProvider
         │
         ▼
Parent form receives DialogResult.OK
         │
         └─> Reload data
```

### 4.2. Luồng Chỉnh Sửa

```
User clicks "Điều chỉnh" button in parent form
         │
         ▼
new FrmBusinessPartnerCategoryDetail(categoryId)
         │
         ├─> Constructor
         │   ├─> InitializeComponent()
         │   ├─> _categoryId = categoryId (not Empty)
         │   └─> InitializeForm()
         │
         ├─> InitializeForm()
         │   ├─> Set title: "Điều chỉnh danh mục đối tác"
         │   ├─> MarkRequiredFields()
         │   ├─> LoadParentCategories()
         │   │   │
         │   │   ├─> BLL.GetCategoriesWithCounts()
         │   │   ├─> Convert to DTOs
         │   │   ├─> Remove current category from list (avoid circular)
         │   │   └─> Bind to SearchLookUpEdit
         │   │
         │   ├─> SetupSuperToolTips()
         │   ├─> LoadCategoryData() [IsEditMode = true]
         │   │   │
         │   │   ├─> BLL.GetById(_categoryId)
         │   │   ├─> category.ToDto()
         │   │   └─> BindDataToControls(dto)
         │   │       │
         │   │       ├─> Set CategoryCode, CategoryName, Description
         │   │       ├─> Set IsActive
         │   │       └─> Set ParentId (if has)
         │   │
         │   └─> Focus CategoryCodeTextEdit
         │
         ▼
Form displays (with existing data)
         │
         ▼
User modifies data
         │
         ▼
User clicks "Lưu" (or Ctrl+S)
         │
         ▼
SaveBarButtonItem_ItemClick()
         │
         ├─> ValidateInput()
         │   │
         │   ├─> Check CategoryName required
         │   ├─> Check CategoryName length
         │   ├─> Check CategoryName unique (exclude _categoryId)
         │   ├─> Check other fields
         │   ├─> Check circular reference
         │   │   │
         │   │   ├─> Cannot select itself as parent
         │   │   └─> Cannot select its child as parent
         │   │
         │   └─> Return true/false
         │
         ├─> If valid → SaveCategory()
         │   │
         │   ├─> GetDataFromControls()
         │   │   ├─> Read values (including _categoryId)
         │   │   └─> Return DTO
         │   │
         │   ├─> dto.ToEntity()
         │   │
         │   ├─> BLL.Update(entity)
         │   │   │
         │   │   └─> [BLL] → [DAL] → Database UPDATE
         │   │
         │   ├─> ShowInfo("Cập nhật thành công!")
         │   ├─> DialogResult = DialogResult.OK
         │   └─> Close()
         │
         └─> If invalid → Show errors
         │
         ▼
Parent form receives DialogResult.OK
         │
         └─> Reload data
```

### 4.3. Luồng Load Parent Categories

```
LoadParentCategories()
         │
         ├─> BLL.GetCategoriesWithCounts()
         │   │
         │   └─> Returns: (List<Category>, Dictionary<Guid, int>)
         │
         ├─> Convert to DTOs
         │   │
         │   ├─> For each category:
         │   │   ├─> ToDtoWithCount(count)
         │   │   ├─> Calculate Level (traverse up to root)
         │   │   ├─> Calculate FullPath (build breadcrumb)
         │   │   └─> Get ParentCategoryName
         │   │
         │   └─> Result: List<BusinessPartnerCategoryDto>
         │
         ├─> Filter (if edit mode)
         │   │
         │   └─> Remove current category (d.Id != _categoryId)
         │
         ├─> Bind to BindingSource
         │   │
         │   └─> businessPartnerCategoryDtoBindingSource.DataSource = dtoList
         │
         ├─> Configure SearchLookUpEdit
         │   │
         │   ├─> DataSource = BindingSource
         │   ├─> ValueMember = "Id"
         │   ├─> DisplayMember = "FullPathHtml"
         │   └─> PopupView = parentCategoryGridView
         │
         ├─> Configure GridView
         │   │
         │   ├─> Sort by FullPathHtml
         │   ├─> Disable group panel, indicator
         │   └─> Set focus style
         │
         └─> Register EditValueChanged event
```

### 4.4. Luồng Validation

```
ValidateInput()
         │
         ├─> dxErrorProvider1.ClearErrors()
         │
         ├─> Validate CategoryName
         │   │
         │   ├─> Required check
         │   ├─> Length check (max 100)
         │   └─> Uniqueness check (BLL.IsCategoryNameExists())
         │
         ├─> Validate CategoryCode
         │   │
         │   └─> Length check (max 50, if not empty)
         │
         ├─> Validate Description
         │   │
         │   └─> Length check (max 255, if not empty)
         │
         ├─> Validate Circular Reference (if edit mode)
         │   │
         │   ├─> Check: selectedParentId != _categoryId
         │   │
         │   └─> Check: selectedParentId is not child of _categoryId
         │       │
         │       ├─> Get all categories
         │       ├─> Traverse from selectedParentId up to root
         │       └─> If encounter _categoryId → Error
         │
         └─> Return true (valid) or false (invalid)
            │
            └─> If false → Errors displayed via DXErrorProvider
```

---

## 5. Lưu Ý Khi Mở Rộng Hoặc Sửa Đổi

### 5.1. Edit Mode vs New Mode

⚠️ **Quan trọng**: Form hoạt động ở 2 chế độ khác nhau:
- **New Mode** (`_categoryId == Guid.Empty`):
  - Không load dữ liệu
  - Không loại bỏ category khỏi parent list
  - Không kiểm tra circular reference
- **Edit Mode** (`_categoryId != Guid.Empty`):
  - Load dữ liệu từ database
  - Loại bỏ category hiện tại khỏi parent list
  - Kiểm tra circular reference

**Khi thêm logic mới**:
- Luôn kiểm tra `IsEditMode` trước khi thực hiện logic chỉ dành cho edit mode
- Sử dụng pattern:
  ```csharp
  if (IsEditMode)
  {
      // Edit-specific logic
  }
  ```

### 5.2. SearchLookUpEdit EditValue Handling

⚠️ **Phức tạp**: `EditValue` của `SearchLookUpEdit` có thể trả về nhiều kiểu:
- `Guid` (mong đợi)
- `string` (cần parse)
- `DBNull` (null value)
- Object khác (cần convert)

**Best Practice**:
```csharp
var editValue = ParentCategorySearchLookUpEdit.EditValue;
Guid? parentId = null;

if (editValue != null && editValue != DBNull.Value)
{
    if (editValue is Guid guidValue)
        parentId = guidValue;
    else if (editValue is string stringValue && Guid.TryParse(stringValue, out var parsedGuid))
        parentId = parsedGuid;
    else
    {
        // Fallback: Lấy từ selected row
        var selectedRow = parentCategoryGridView.GetFocusedRow() as BusinessPartnerCategoryDto;
        parentId = selectedRow?.Id;
    }
}
```

**Lưu ý**: 
- Luôn có fallback logic
- Không assume `EditValue` luôn là `Guid`
- Test với nhiều scenario (chọn, clear, programmatic set)

### 5.3. Circular Reference Prevention

⚠️ **Quan trọng**: Phải ngăn chặn circular reference trong hierarchical data.

**Các trường hợp cần kiểm tra**:
1. **Chọn chính nó làm parent**: 
   ```csharp
   if (selectedParentId == _categoryId) // Error
   ```
2. **Chọn con của nó làm parent**: 
   - Duyệt từ selected parent lên root
   - Nếu gặp `_categoryId` → Error

**Thuật toán**:
```csharp
var allCategories = _businessPartnerCategoryBll.GetAll();
var categoryDict = allCategories.ToDictionary(c => c.Id);
var current = allCategories.FirstOrDefault(c => c.Id == selectedParentId);

while (current != null && current.ParentId.HasValue)
{
    if (current.ParentId.Value == _categoryId)
    {
        // Circular reference detected!
        return false;
    }
    current = categoryDict.ContainsKey(current.ParentId.Value) 
        ? categoryDict[current.ParentId.Value] 
        : null;
}
```

**Lưu ý**: 
- Chỉ kiểm tra trong edit mode
- Cần guard để tránh infinite loop (mặc dù đã filter trong `LoadParentCategories()`)

### 5.4. Validation Logic

✅ **Pattern hiện tại**: Validation được thực hiện trong `ValidateInput()` với `DXErrorProvider`.

**Khi thêm validation mới**:
1. Clear errors trước: `dxErrorProvider1.ClearErrors()`
2. Kiểm tra điều kiện
3. Nếu lỗi: 
   ```csharp
   dxErrorProvider1.SetError(control, "Error message", ErrorType.Critical);
   control.Focus();
   return false;
   ```
4. Return `true` nếu tất cả đều hợp lệ

**Lưu ý**: 
- Validation được thực hiện khi user click "Lưu"
- Có thể thêm real-time validation trong `TextChanged` events nếu cần
- Sử dụng `ErrorType.Critical` để hiển thị icon đỏ

### 5.5. Required Fields

✅ **Pattern hiện tại**: Sử dụng `RequiredFieldHelper.MarkRequiredFields()` với DataAnnotations.

**Cách hoạt động**:
- `RequiredFieldHelper` đọc `[Required]` attributes từ DTO
- Tự động đánh dấu các controls tương ứng

**Khi thêm field mới**:
- Thêm `[Required]` attribute vào property trong DTO
- `RequiredFieldHelper` sẽ tự động đánh dấu control

**Lưu ý**: 
- Không cần manually mark required fields
- Đảm bảo control name match với property name (convention)

### 5.6. Data Binding

⚠️ **Lưu ý**: Form sử dụng manual binding (không dùng data binding tự động).

**Pattern hiện tại**:
- **Load**: `BindDataToControls(dto)` - DTO → Controls
- **Save**: `GetDataFromControls()` - Controls → DTO

**Khi thêm field mới**:
1. Thêm control vào Designer
2. Update `BindDataToControls()`: `Control.Text = dto.Property`
3. Update `GetDataFromControls()`: `dto.Property = Control.Text`

**Lưu ý**: 
- Luôn `.Trim()` string values
- Xử lý null values cẩn thận
- `SearchLookUpEdit` cần xử lý đặc biệt (xem section 5.2)

### 5.7. Error Handling

✅ **Pattern hiện tại**: Try-catch trong các methods chính.

**Khi thêm error handling**:
```csharp
try
{
    // Operation
}
catch (Exception ex)
{
    ShowError(ex, "Context message");
    // Không throw lại để không crash form
}
```

**Lưu ý**: 
- Luôn hiển thị thông báo cho user
- Log exception nếu cần (hiện tại chỉ dùng `ShowError`)
- Không "nuốt" exception mà không thông báo

### 5.8. DialogResult Pattern

✅ **Pattern hiện tại**: Sử dụng `DialogResult` để communicate với form cha.

**Values**:
- `DialogResult.OK` → Lưu thành công, form cha reload data
- `DialogResult.Cancel` → Hủy, form cha không reload

**Khi sửa đổi**:
- Luôn set `DialogResult` trước khi `Close()`
- Form cha sẽ check `DialogResult` để quyết định có reload hay không

**Example**:
```csharp
// In parent form
using (var form = new FrmBusinessPartnerCategoryDetail(categoryId))
{
    if (form.ShowDialog() == DialogResult.OK)
    {
        await LoadDataAsync(); // Reload data
    }
}
```

### 5.9. Performance Considerations

💡 **Tối ưu hóa**:
- `LoadParentCategories()` load tất cả categories - có thể chậm với dataset lớn
- Tính toán `Level` và `FullPath` là O(n²) trong worst case

**Nếu dataset rất lớn**:
- Cân nhắc lazy loading cho parent categories
- Cân nhắc caching parent list
- Cân nhắc chỉ load parent categories khi user mở dropdown

### 5.10. Testing

✅ **Khi thêm/chỉnh sửa code**:
- Test với **new mode** (Guid.Empty)
- Test với **edit mode** (có categoryId)
- Test validation:
  - Required fields
  - Length limits
  - Uniqueness
  - Circular reference
- Test `SearchLookUpEdit`:
  - Chọn parent
  - Clear parent (null)
  - Chọn parent rồi đổi sang parent khác
- Test phím tắt (Ctrl+S, Escape)
- Test với dữ liệu edge cases:
  - CategoryName = ""
  - CategoryName = very long string
  - ParentId = null
  - ParentId = valid Guid

### 5.11. Code Style

✅ **Tuân thủ**:
- Sử dụng regions để tổ chức code
- XML documentation comments cho public/protected methods
- Naming convention:
  - Private methods: `PascalCase`
  - Private fields: `_camelCase`
  - Events: `ObjectName_EventName`

### 5.12. Dependencies

⚠️ **Khi thay đổi dependencies**:
- `BusinessPartnerCategoryBll`: Nếu thay đổi interface, cần update tất cả calls
- `BusinessPartnerCategoryDto`: Nếu thêm/sửa properties:
  - Update `BindDataToControls()`
  - Update `GetDataFromControls()`
  - Update validation nếu cần
- `RequiredFieldHelper`: Nếu thay đổi cách hoạt động, cần test lại required fields marking

---

## 6. Tóm Tắt

### Điểm Mạnh:
✅ Hỗ trợ cả thêm mới và chỉnh sửa trong cùng một form  
✅ Validation đầy đủ với DXErrorProvider  
✅ Ngăn chặn circular reference trong hierarchical data  
✅ Xử lý phức tạp cho SearchLookUpEdit EditValue  
✅ Hỗ trợ phím tắt (Ctrl+S, Escape)  
✅ Required fields tự động từ DataAnnotations  
✅ User experience tốt (tooltips, error messages)  

### Điểm Cần Lưu Ý:
⚠️ `EditValue` handling phức tạp và dễ lỗi  
⚠️ Circular reference check có thể chậm với dataset lớn  
⚠️ Load tất cả parent categories có thể chậm  
⚠️ Manual data binding (không tự động)  

### Khuyến Nghị:
💡 Cân nhắc thêm real-time validation (TextChanged events)  
💡 Cân nhắc lazy loading cho parent categories  
💡 Cân nhắc unit tests cho validation logic  
💡 Cân nhắc helper method để parse EditValue (reusable)  

---

**Tài liệu này được tạo tự động dựa trên phân tích code. Cập nhật lần cuối: 2025-01-XX**
