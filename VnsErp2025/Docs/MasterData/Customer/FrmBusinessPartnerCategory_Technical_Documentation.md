# FrmBusinessPartnerCategory - Tài Liệu Kỹ Thuật

## 1. Mục Đích Của Class

`FrmBusinessPartnerCategory` là một Windows Forms form (kế thừa từ `XtraForm` của DevExpress) được thiết kế để quản lý danh sách danh mục đối tác (Business Partner Category) trong hệ thống ERP.

### Chức Năng Chính:
- **Hiển thị danh sách**: Hiển thị danh sách danh mục đối tác dưới dạng bảng với cấu trúc phân cấp (hierarchical)
- **Tìm kiếm**: Hỗ trợ tìm kiếm và lọc dữ liệu thông qua GridView của DevExpress
- **Thêm mới**: Mở form chi tiết để thêm mới danh mục đối tác
- **Sửa đổi**: Chỉnh sửa thông tin danh mục đối tác đã chọn
- **Xóa**: Xóa một hoặc nhiều danh mục đối tác (có xử lý quan hệ cha-con)
- **Xuất dữ liệu**: Xuất danh sách ra file Excel (.xlsx)

---

## 2. Vai Trò Trong Kiến Trúc

### **Vị Trí: UI Layer (Presentation Layer)**

Form này nằm ở tầng **UI (User Interface)** trong kiến trúc 3-layer của ứng dụng:

```
┌─────────────────────────────────────────┐
│  UI Layer (Presentation)               │
│  ┌───────────────────────────────────┐ │
│  │ FrmBusinessPartnerCategory        │ │ ← Class này
│  │ - XtraForm (DevExpress)          │ │
│  │ - Event Handlers                 │ │
│  │ - UI State Management            │ │
│  └───────────────────────────────────┘ │
└─────────────────────────────────────────┘
              │
              │ Gọi methods
              ▼
┌─────────────────────────────────────────┐
│  BLL Layer (Business Logic)             │
│  ┌───────────────────────────────────┐ │
│  │ BusinessPartnerCategoryBll        │ │
│  │ - GetCategoriesWithCountsAsync()  │ │
│  │ - GetAllAsync()                   │ │
│  │ - Delete()                        │ │
│  └───────────────────────────────────┘ │
└─────────────────────────────────────────┘
              │
              │ Sử dụng
              ▼
┌─────────────────────────────────────────┐
│  DAL Layer (Data Access)                │
│  ┌───────────────────────────────────┐ │
│  │ Repository / DataContext          │ │
│  │ - Database Operations             │ │
│  └───────────────────────────────────┘ │
└─────────────────────────────────────────┘
```

### **Dependencies:**
- **BLL Layer**: `BusinessPartnerCategoryBll` - Xử lý business logic
- **DTO Layer**: `BusinessPartnerCategoryDto` - Data Transfer Object
- **Common Utilities**: 
  - `Logger` - Ghi log
  - `MsgBox` - Hiển thị thông báo
  - `SuperToolTipHelper` - Tooltip hỗ trợ
- **UI Framework**: DevExpress WinForms controls

### **Không Trực Tiếp Truy Cập:**
- ❌ Database (không gọi DAL trực tiếp)
- ❌ Domain entities (chỉ làm việc với DTO)

---

## 3. Giải Thích Các Method Chính

### 3.1. Quản Lý Dữ Liệu

#### `LoadDataAsync()`
```csharp
private async Task LoadDataAsync()
```
**Mục đích**: Tải dữ liệu danh mục từ database và hiển thị lên GridView với splash screen.

**Luồng xử lý**:
1. Kiểm tra `_isLoading` để tránh gọi song song (re-entrancy guard)
2. Hiển thị `WaitForm1` (splash screen)
3. Gọi `LoadDataAsyncWithoutSplash()` để tải dữ liệu
4. Xử lý exception và đóng splash screen

**Lưu ý**: 
- Sử dụng guard pattern để tránh multiple concurrent loads
- Tự động đóng splash screen trong `finally` block

---

#### `LoadDataAsyncWithoutSplash()`
```csharp
private async Task LoadDataAsyncWithoutSplash()
```
**Mục đích**: Tải dữ liệu thực tế từ BLL và bind vào GridView (không hiển thị splash screen).

**Luồng xử lý**:
1. Gọi `_businessPartnerCategoryBll.GetCategoriesWithCountsAsync()` để lấy:
   - Danh sách categories
   - Dictionary số lượng đối tác theo CategoryId
2. Chuyển đổi sang DTO với cấu trúc hierarchical bằng extension method `ToDtosWithHierarchy()`
3. Gọi `BindGrid()` để bind dữ liệu vào GridView
4. Log debug information để hỗ trợ troubleshooting

**Extension Method**: `ToDtosWithHierarchy()` tính toán:
- `Level`: Mức độ phân cấp (0 = root, 1 = child, ...)
- `FullPath`: Đường dẫn đầy đủ từ root đến node hiện tại
- `HasChildren`: Có danh mục con hay không
- `PartnerCount`: Số lượng đối tác thuộc danh mục

---

#### `BindGrid(List<BusinessPartnerCategoryDto> data)`
```csharp
private void BindGrid(List<BusinessPartnerCategoryDto> data)
```
**Mục đích**: Bind danh sách DTO vào GridView và cấu hình hiển thị.

**Luồng xử lý**:
1. Clear selection state trước khi bind
2. Gán `data` vào `businessPartnerCategoryDtoBindingSource.DataSource`
3. Tự động điều chỉnh độ rộng cột (`BestFitColumns()`)
4. Cấu hình sắp xếp mặc định (`ConfigureMultiLineGridView()`)
5. Clear selection lại sau khi bind
6. Cập nhật summary và selection info

**Lưu ý**: Clear selection được gọi 2 lần để đảm bảo không có selection cũ còn sót lại.

---

### 3.2. Event Handlers

#### `NewBarButtonItem_ItemClick()`
```csharp
private async void NewBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
```
**Mục đích**: Xử lý sự kiện click button "Thêm mới".

**Luồng xử lý**:
1. Hiển thị overlay để disable form hiện tại
2. Mở `FrmBusinessPartnerCategoryDetail` với `Guid.Empty` (thêm mới)
3. Sau khi đóng form detail, tự động reload dữ liệu
4. Cập nhật trạng thái buttons

---

#### `EditBarButtonItem_ItemClick()`
```csharp
private async void EditBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
```
**Mục đích**: Xử lý sự kiện click button "Điều chỉnh".

**Luồng xử lý**:
1. **Validation**: 
   - Kiểm tra có selection hay không
   - Chỉ cho phép chỉnh sửa 1 dòng (không cho phép multi-select)
2. Lấy `Id` từ `_selectedCategoryIds[0]`
3. Tìm DTO tương ứng từ GridView hoặc BindingSource
4. Mở `FrmBusinessPartnerCategoryDetail` với `Id` của category
5. Reload dữ liệu sau khi đóng form

**Lưu ý**: Có logic fallback để tìm DTO nếu `FocusedRow` không khớp với selection.

---

#### `DeleteBarButtonItem_ItemClick()`
```csharp
private async void DeleteBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
```
**Mục đích**: Xử lý sự kiện click button "Xóa".

**Luồng xử lý**:
1. **Validation**: Kiểm tra có selection hay không
2. Hiển thị confirmation dialog (Yes/No)
3. Gọi `DeleteCategoriesInOrder()` để xóa theo thứ tự hierarchical
4. Hiển thị splash screen trong quá trình xóa
5. Tự động reload dữ liệu sau khi xóa thành công

**Lưu ý**: Hỗ trợ xóa nhiều dòng cùng lúc.

---

#### `DeleteCategoriesInOrder(List<Guid> categoryIds)`
```csharp
private async Task DeleteCategoriesInOrder(List<Guid> categoryIds)
```
**Mục đích**: Xóa các danh mục theo thứ tự hierarchical (con trước, cha sau) để tránh lỗi foreign key constraint.

**Luồng xử lý**:
1. Lấy tất cả categories để xây dựng dictionary
2. Tính toán `Level` cho mỗi category cần xóa
3. Kiểm tra số lượng đối tác (`PartnerCount`) của mỗi category
4. **Thông báo người dùng**: Nếu có category chứa đối tác, hiển thị danh sách và xác nhận
5. Sắp xếp categories theo `Level` giảm dần (level cao = xóa trước)
6. Xóa từng category:
   - Repository tự động di chuyển đối tác sang "Chưa phân loại" nếu có
   - Log thông tin xóa
7. Hiển thị thông báo kết quả (số lượng đã xóa, số đối tác đã di chuyển)

**Thuật toán tính Level**:
```csharp
private int CalculateCategoryLevel(BusinessPartnerCategory category, 
    Dictionary<Guid, BusinessPartnerCategory> categoryDict)
```
- Đếm số lượng parent từ category hiện tại lên root
- Có guard để tránh infinite loop (max 10 levels)

---

#### `ExportBarButtonItem_ItemClick()`
```csharp
private void ExportBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
```
**Mục đích**: Xuất dữ liệu GridView ra file Excel.

**Luồng xử lý**:
1. Kiểm tra có dữ liệu hay không
2. Hiển thị `SaveFileDialog` với filter `.xlsx`
3. Gọi `BusinessPartnerCategoryDtoGridView.ExportToXlsx()` (DevExpress built-in method)
4. Hiển thị thông báo thành công

---

#### `BusinessPartnerCategoryDtoGridView_SelectionChanged()`
```csharp
private void BusinessPartnerCategoryDtoGridView_SelectionChanged(object sender, EventArgs e)
```
**Mục đích**: Xử lý sự kiện thay đổi selection trên GridView.

**Luồng xử lý**:
1. Cập nhật `_selectedCategoryIds` từ GridView selection
2. Cập nhật trạng thái buttons (Enable/Disable)
3. Cập nhật thông tin selection hiện tại trên status bar

**Lưu ý**: GridView được cấu hình với `MultiSelect = true` và `MultiSelectMode = CheckBoxRowSelect`.

---

### 3.3. Utility Methods

#### `UpdateButtonStates()`
```csharp
private void UpdateButtonStates()
```
**Mục đích**: Cập nhật trạng thái Enable/Disable của các buttons dựa trên selection và dữ liệu.

**Logic**:
- **Edit**: Chỉ enable khi chọn đúng 1 dòng
- **Delete**: Enable khi chọn >= 1 dòng
- **Export**: Enable khi có dữ liệu hiển thị (rowCount > 0)

---

#### `UpdateDataSummary()`
```csharp
private void UpdateDataSummary()
```
**Mục đích**: Cập nhật thông tin tổng kết dữ liệu trên status bar với format HTML.

**Thông tin hiển thị**:
- Tổng số danh mục
- Số lượng danh mục hoạt động (màu xanh)
- Số lượng danh mục ngừng (màu đỏ)
- Tổng số đối tác (màu cam)

**Format**: Sử dụng HTML với DevExpress color tags (`<color='#1976D2'>`, `<b>`, etc.)

---

#### `UpdateCurrentSelection()`
```csharp
private void UpdateCurrentSelection()
```
**Mục đích**: Cập nhật thông tin selection hiện tại trên status bar.

**Thông tin hiển thị**:
- Khi chọn 0 dòng: "Chưa chọn dòng nào"
- Khi chọn 1 dòng: Hiển thị chi tiết (tên, mã, trạng thái, số đối tác)
- Khi chọn nhiều dòng: "X dòng được chọn"

---

#### `UpdateSelectedCategoryIds()`
```csharp
private void UpdateSelectedCategoryIds()
```
**Mục đích**: Cập nhật danh sách `_selectedCategoryIds` từ GridView selection.

**Luồng xử lý**:
1. Clear `_selectedCategoryIds`
2. Lấy tất cả selected rows từ GridView
3. Extract `Id` từ mỗi DTO và thêm vào list
4. Log debug information

---

#### `ClearSelectionState()`
```csharp
private void ClearSelectionState()
```
**Mục đích**: Xóa toàn bộ selection trên GridView và reset state.

**Luồng xử lý**:
1. Clear `_selectedCategoryIds`
2. Clear GridView selection
3. Reset `FocusedRowHandle` về `InvalidRowHandle`
4. Cập nhật buttons và selection info

---

#### `ConfigureMultiLineGridView()`
```csharp
private void ConfigureMultiLineGridView()
```
**Mục đích**: Cấu hình GridView với sắp xếp mặc định.

**Logic**:
- Ưu tiên sắp xếp theo `SortOrder` (nếu có column)
- Fallback: Sắp xếp theo `CategoryName` (Ascending)

---

#### `ExecuteWithWaitingFormAsync(Func<Task> operation)`
```csharp
private async Task ExecuteWithWaitingFormAsync(Func<Task> operation)
```
**Mục đích**: Wrapper method để thực hiện async operation với splash screen.

**Luồng xử lý**:
1. Hiển thị `WaitForm1` (SplashScreenManager)
2. Thực hiện operation
3. Đóng splash screen trong `finally` block

**Pattern**: Sử dụng try-finally để đảm bảo splash screen luôn được đóng.

---

#### `SetupSuperToolTips()`
```csharp
private void SetupSuperToolTips()
```
**Mục đích**: Thiết lập SuperToolTip cho các buttons trên toolbar.

**Tooltips được thiết lập**:
- 🔄 Tải dữ liệu
- ➕ Thêm mới
- ✏️ Sửa
- 🗑️ Xóa
- 📊 Xuất Excel

---

## 4. Luồng Xử Lý Dữ Liệu

### 4.1. Luồng Tải Dữ Liệu

```
User clicks "Danh sách" button
         │
         ▼
LoadDataAsync()
         │
         ├─> Check _isLoading guard
         ├─> Show WaitForm1 (SplashScreen)
         │
         ▼
LoadDataAsyncWithoutSplash()
         │
         ├─> BusinessPartnerCategoryBll.GetCategoriesWithCountsAsync()
         │   │
         │   ├─> Returns: (List<BusinessPartnerCategory>, Dictionary<Guid, int>)
         │   │
         │   └─> [BLL Layer] → [DAL Layer] → Database
         │
         ├─> categories.ToDtosWithHierarchy(counts)
         │   │
         │   ├─> Calculate Level (hierarchical depth)
         │   ├─> Calculate FullPath (breadcrumb path)
         │   ├─> Calculate HasChildren
         │   ├─> Add PartnerCount from dictionary
         │   └─> Sort hierarchical (parent → children)
         │
         ▼
BindGrid(dtoList)
         │
         ├─> ClearSelectionState()
         ├─> businessPartnerCategoryDtoBindingSource.DataSource = dtoList
         ├─> BestFitColumns()
         ├─> ConfigureMultiLineGridView()
         ├─> UpdateDataSummary()
         └─> UpdateCurrentSelection()
         │
         ▼
Close WaitForm1
         │
         ▼
GridView displays data
```

### 4.2. Luồng Thêm Mới

```
User clicks "Mới" button
         │
         ▼
NewBarButtonItem_ItemClick()
         │
         ├─> Show OverlayManager (disable current form)
         │
         ├─> new FrmBusinessPartnerCategoryDetail(Guid.Empty)
         │   │
         │   └─> [User enters data and saves]
         │
         ├─> form.ShowDialog(this)
         │
         ├─> await LoadDataAsync() [Reload data]
         │
         └─> UpdateButtonStates()
```

### 4.3. Luồng Sửa Đổi

```
User clicks "Điều chỉnh" button
         │
         ▼
EditBarButtonItem_ItemClick()
         │
         ├─> Validate: _selectedCategoryIds.Count == 1
         │
         ├─> Get selectedId = _selectedCategoryIds[0]
         │
         ├─> Find DTO from GridView or BindingSource
         │
         ├─> Show OverlayManager
         │
         ├─> new FrmBusinessPartnerCategoryDetail(selectedId)
         │   │
         │   └─> [User modifies and saves]
         │
         ├─> form.ShowDialog(this)
         │
         ├─> await LoadDataAsync() [Reload data]
         │
         └─> UpdateButtonStates()
```

### 4.4. Luồng Xóa

```
User clicks "Xóa" button
         │
         ▼
DeleteBarButtonItem_ItemClick()
         │
         ├─> Validate: _selectedCategoryIds.Count > 0
         │
         ├─> Show confirmation dialog (Yes/No)
         │
         ├─> Show WaitForm1
         │
         ▼
DeleteCategoriesInOrder(categoryIds)
         │
         ├─> Get all categories from BLL
         │
         ├─> Calculate Level for each category to delete
         │
         ├─> Check PartnerCount for each category
         │
         ├─> If categories have partners:
         │   └─> Show warning with list of categories
         │   └─> User confirms → Continue
         │
         ├─> Sort by Level DESC (children first, parents last)
         │
         ├─> For each category:
         │   ├─> If PartnerCount > 0:
         │   │   └─> Repository auto-moves partners to "Chưa phân loại"
         │   │
         │   └─> BusinessPartnerCategoryBll.Delete(categoryId)
         │       │
         │       └─> [BLL] → [DAL] → Database DELETE
         │
         ├─> Show success message (with moved partners count)
         │
         └─> Close WaitForm1
         │
         ▼
Auto reload: ListDataBarButtonItem.PerformClick()
```

### 4.5. Luồng Xuất Excel

```
User clicks "Xuất" button
         │
         ▼
ExportBarButtonItem_ItemClick()
         │
         ├─> Validate: rowCount > 0
         │
         ├─> Show SaveFileDialog
         │   └─> Filter: *.xlsx
         │
         ├─> User selects file path
         │
         ├─> BusinessPartnerCategoryDtoGridView.ExportToXlsx(filePath)
         │   │
         │   └─> [DevExpress built-in method]
         │
         └─> Show success message
```

### 4.6. Luồng Selection Changed

```
User selects/deselects rows in GridView
         │
         ▼
BusinessPartnerCategoryDtoGridView_SelectionChanged()
         │
         ├─> UpdateSelectedCategoryIds()
         │   │
         │   ├─> Clear _selectedCategoryIds
         │   ├─> Get selected rows from GridView
         │   ├─> Extract Id from each DTO
         │   └─> Add to _selectedCategoryIds
         │
         ├─> UpdateButtonStates()
         │   │
         │   ├─> Edit: Enable if selectedCount == 1
         │   ├─> Delete: Enable if selectedCount >= 1
         │   └─> Export: Enable if rowCount > 0
         │
         └─> UpdateCurrentSelection()
             │
             └─> Update status bar with selection info
```

---

## 5. Lưu Ý Khi Mở Rộng Hoặc Sửa Đổi

### 5.1. Async/Await Pattern

⚠️ **Quan trọng**: Class sử dụng async/await pattern. Khi thêm method mới gọi BLL:
- Sử dụng `async Task` cho methods
- Sử dụng `async void` chỉ cho event handlers
- Luôn `await` các async calls
- Sử dụng `ExecuteWithWaitingFormAsync()` nếu cần hiển thị splash screen

**Ví dụ đúng**:
```csharp
private async Task SomeNewMethodAsync()
{
    await ExecuteWithWaitingFormAsync(async () =>
    {
        var data = await _businessPartnerCategoryBll.SomeMethodAsync();
        // Process data...
    });
}
```

### 5.2. Re-entrancy Guard

⚠️ **Quan trọng**: `LoadDataAsync()` có guard `_isLoading` để tránh gọi song song. Nếu thêm method mới có thể gọi song song, cần implement guard tương tự.

**Pattern hiện tại**:
```csharp
private bool _isLoading;

private async Task LoadDataAsync()
{
    if (_isLoading) return; // Guard
    _isLoading = true;
    try
    {
        // Do work...
    }
    finally
    {
        _isLoading = false;
    }
}
```

### 5.3. Selection Management

⚠️ **Lưu ý**: Selection state được quản lý bởi `_selectedCategoryIds` và GridView selection. Khi bind data mới:
- **Luôn** gọi `ClearSelectionState()` trước và sau `BindGrid()`
- Không thay đổi selection trong quá trình async operation (có thể gây race condition)

**Best Practice**:
```csharp
private void BindGrid(List<BusinessPartnerCategoryDto> data)
{
    ClearSelectionState(); // Before bind
    businessPartnerCategoryDtoBindingSource.DataSource = data;
    // ... other config ...
    ClearSelectionState(); // After bind (safety)
}
```

### 5.4. Hierarchical Data

⚠️ **Lưu ý**: Dữ liệu có cấu trúc phân cấp (parent-child). Khi xóa:
- **Luôn** xóa theo thứ tự: children trước, parents sau
- Sử dụng `CalculateCategoryLevel()` để xác định thứ tự
- Có guard để tránh infinite loop (max 10 levels)

**Nếu thêm logic mới liên quan đến hierarchy**:
- Kiểm tra `ParentId.HasValue`
- Sử dụng `ToDtosWithHierarchy()` extension method
- Xem xét `Level`, `FullPath`, `HasChildren` properties

### 5.5. Error Handling

✅ **Pattern hiện tại**: Sử dụng try-catch với `ShowError()` helper method.

**Khi thêm error handling mới**:
```csharp
try
{
    // Operation...
}
catch (Exception ex)
{
    ShowError(ex, "Context message");
    // Hoặc
    MsgBox.ShowException(ex);
}
```

**Lưu ý**: 
- Không bắt exception và "nuốt" (swallow) mà không log
- Luôn hiển thị thông báo cho người dùng
- Log lỗi bằng `_logger` nếu cần debug

### 5.6. UI State Management

⚠️ **Quan trọng**: Các buttons được enable/disable dựa trên:
- Selection state (`_selectedCategoryIds`)
- Data availability (`rowCount`)

**Khi thêm button mới**:
- Thêm logic vào `UpdateButtonStates()`
- Gọi `UpdateButtonStates()` sau mỗi thay đổi selection hoặc data

### 5.7. Logging

✅ **Pattern hiện tại**: Sử dụng `ILogger` với các levels:
- `Debug`: Thông tin chi tiết cho troubleshooting
- `Info`: Thông tin quan trọng (xóa, thêm, sửa)
- `Warning`: Cảnh báo (không critical)
- `Error`: Lỗi cần xử lý

**Khi thêm logging**:
```csharp
_logger.Debug("Debug message with {0} parameter", value);
_logger.Info("Operation completed: {0}", result);
_logger.Warning("Warning message: {0}", issue);
_logger.Error("Error occurred: {0}", ex.Message);
```

### 5.8. DevExpress Controls

⚠️ **Lưu ý**: Class sử dụng nhiều DevExpress controls:
- `XtraForm` (base class)
- `GridControl` / `GridView`
- `BarManager` / `BarButtonItem`
- `SplashScreenManager`
- `OverlayManager`

**Khi sửa đổi UI**:
- Không sửa code trong `InitializeComponent()` (Designer file)
- Sửa trong `.Designer.cs` chỉ khi thực sự cần thiết
- Prefer code-behind trong `.cs` file

### 5.9. Dependencies

⚠️ **Khi thay đổi dependencies**:
- `BusinessPartnerCategoryBll`: Nếu thay đổi interface, cần update tất cả calls
- `BusinessPartnerCategoryDto`: Nếu thêm/sửa properties, cần update:
  - GridView columns (nếu cần hiển thị)
  - `UpdateDataSummary()` (nếu liên quan)
  - `UpdateCurrentSelection()` (nếu liên quan)

### 5.10. Performance

💡 **Tối ưu hóa**:
- `LoadDataAsync()` có guard để tránh multiple loads
- Sử dụng `BestFitColumns()` chỉ khi cần (có thể chậm với nhiều rows)
- `ToDtosWithHierarchy()` có thể tốn thời gian với dataset lớn (O(n²) trong worst case)

**Nếu dataset rất lớn (>1000 rows)**:
- Cân nhắc pagination
- Cân nhắc virtual mode cho GridView
- Cân nhắc lazy loading cho hierarchical data

### 5.11. Testing

✅ **Khi thêm/chỉnh sửa code**:
- Test với dataset nhỏ (< 10 rows)
- Test với dataset lớn (> 100 rows)
- Test với hierarchical data (nhiều levels)
- Test với selection (single, multiple, none)
- Test với xóa category có đối tác
- Test với xóa category có children

### 5.12. Code Style

✅ **Tuân thủ**:
- Sử dụng regions để tổ chức code (`#region ... #endregion`)
- XML documentation comments cho public/protected methods
- Naming convention: 
  - Private methods: `PascalCase`
  - Private fields: `_camelCase`
  - Events: `ObjectName_EventName`

---

## 6. Tóm Tắt

### Điểm Mạnh:
✅ Async/await pattern được sử dụng đúng cách  
✅ Có re-entrancy guard để tránh race conditions  
✅ Error handling đầy đủ với logging  
✅ UI state management nhất quán  
✅ Hỗ trợ hierarchical data với xử lý thông minh  
✅ User experience tốt (splash screen, tooltips, status bar)  

### Điểm Cần Lưu Ý:
⚠️ `ToDtosWithHierarchy()` có thể chậm với dataset rất lớn  
⚠️ Selection state có thể phức tạp khi có nhiều async operations  
⚠️ Phụ thuộc vào DevExpress (khó migrate sang framework khác)  

### Khuyến Nghị:
💡 Cân nhắc thêm unit tests cho business logic  
💡 Cân nhắc thêm integration tests cho UI flows  
💡 Cân nhắc caching nếu dataset không thay đổi thường xuyên  

---

**Tài liệu này được tạo tự động dựa trên phân tích code. Cập nhật lần cuối: 2025-01-XX**
