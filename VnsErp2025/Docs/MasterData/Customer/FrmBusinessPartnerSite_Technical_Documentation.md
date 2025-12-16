# FrmBusinessPartnerSite - Tài Liệu Kỹ Thuật

## 1. Mục Đích Của Class

`FrmBusinessPartnerSite` là một Windows Forms form (kế thừa từ `XtraForm` của DevExpress) được thiết kế để quản lý danh sách chi nhánh đối tác (Business Partner Site).

### Chức Năng Chính:
- **Hiển thị danh sách**: Hiển thị tất cả chi nhánh đối tác trong hệ thống với định dạng HTML
- **Thêm mới**: Mở form chi tiết để thêm chi nhánh mới
- **Chỉnh sửa**: Mở form chi tiết để chỉnh sửa chi nhánh đã chọn
- **Xóa**: Xóa chi nhánh đã chọn khỏi hệ thống
- **Xuất Excel**: Xuất danh sách chi nhánh ra file Excel
- **Tìm kiếm/Filter**: Hỗ trợ auto-filter row để tìm kiếm nhanh
- **Cập nhật thông minh**: Cập nhật single row thay vì reload toàn bộ để cải thiện UX
- **Row styling**: Tô màu dòng theo trạng thái (hoạt động/ngừng hoạt động)
- **Multi-line display**: Hiển thị dữ liệu xuống dòng (word wrap) cho các cột văn bản dài

### Đặc Điểm:
- Form standalone - có thể mở độc lập
- Event-driven architecture - sử dụng event `SiteSaved` để cập nhật datasource
- HTML rendering - Hiển thị thông tin chi nhánh dưới dạng HTML trong một cột duy nhất
- Multi-select support - Hỗ trợ chọn nhiều dòng
- Auto-height rows - Tự động điều chỉnh chiều cao dòng để hiển thị đầy đủ nội dung
- Status bar - Hiển thị tổng số và số đang hoạt động, thông tin dòng được chọn

---

## 2. Vai Trò Trong Kiến Trúc

### **Vị Trí: UI Layer (Presentation Layer)**

Form này nằm ở tầng **UI (User Interface)** trong kiến trúc 3-layer của ứng dụng:

```
┌─────────────────────────────────────────┐
│  UI Layer (Presentation)                │
│  ┌───────────────────────────────────┐ │
│  │ FrmBusinessPartnerSite             │ │ ← Class này
│  │ - XtraForm (DevExpress)            │ │
│  │ - List/Grid Form                   │ │
│  │ - CRUD Operations                   │ │
│  │ - Export Functionality             │ │
│  └───────────────────────────────────┘ │
└─────────────────────────────────────────┘
              │
              │ Gọi methods
              ▼
┌─────────────────────────────────────────┐
│  BLL Layer (Business Logic)            │
│  ┌───────────────────────────────────┐ │
│  │ BusinessPartnerSiteBll            │ │
│  │ - GetAll()                        │ │
│  │ - DeleteSite()                    │ │
│  └───────────────────────────────────┘ │
└─────────────────────────────────────────┘
              │
              │ Sử dụng
              ▼
┌─────────────────────────────────────────┐
│  DAL Layer (Data Access)                │
│  ┌───────────────────────────────────┐ │
│  │ BusinessPartnerSiteRepository     │ │
│  │ - Database Operations              │ │
│  └───────────────────────────────────┘ │
└─────────────────────────────────────────┘
              │
              │ Mở form detail
              ▼
┌─────────────────────────────────────────┐
│  UI Layer (Detail Form)                 │
│  ┌───────────────────────────────────┐ │
│  │ FrmBusinessPartnerSiteDetail       │ │
│  │ - Add/Edit Form                   │ │
│  │ - Event: SiteSaved                │ │
│  └───────────────────────────────────┘ │
└─────────────────────────────────────────┘
```

### **Dependencies:**
- **BLL Layer**: `BusinessPartnerSiteBll` - Xử lý business logic
- **DTO Layer**: `BusinessPartnerSiteListDto` - DTO để hiển thị danh sách
- **Domain Layer**: `BusinessPartnerSite` - Entity từ database (thông qua BLL)
- **Detail Form**: `FrmBusinessPartnerSiteDetail` - Form chi tiết để thêm/sửa
- **Common Utilities**: 
  - `GridViewHelper` - Helper cho GridView operations
  - `SuperToolTipHelper` - Tooltip hỗ trợ
  - `MsgBox` - Hiển thị thông báo
  - `OverlayManager` - Hiển thị overlay khi mở form modal
- **UI Framework**: DevExpress WinForms controls
  - `GridControl` - Hiển thị danh sách
  - `GridView` - View cho GridControl
  - `RepositoryItemHypertextLabel` - Hiển thị HTML
  - `RepositoryItemMemoEdit` - Word wrap cho text
  - `BarManager` - Toolbar và status bar

### **Không Trực Tiếp Truy Cập:**
- ❌ Database (không gọi DAL trực tiếp)
- ❌ Repository (chỉ làm việc qua BLL)

---

## 3. Giải Thích Các Method Chính

### 3.1. Constructor & Initialization

#### `FrmBusinessPartnerSite()`
```csharp
public FrmBusinessPartnerSite()
```
**Mục đích**: Khởi tạo form quản lý chi nhánh đối tác.

**Luồng xử lý**:
1. Gọi `InitializeComponent()` (Designer-generated)
2. Khởi tạo `BusinessPartnerSiteBll`
3. Khởi tạo `_dataList` (danh sách rỗng)
4. Đăng ký events: `InitializeEvents()`
5. Cấu hình GridView: `ConfigureMultiLineGridView()`
6. Cập nhật trạng thái nút: `UpdateButtonStates()`
7. Setup SuperToolTips: `SetupSuperToolTips()`

**Lưu ý**: 
- Form không tự động load dữ liệu khi khởi tạo
- User phải nhấn nút "Danh sách" để load dữ liệu

---

#### `InitializeEvents()`
```csharp
private void InitializeEvents()
```
**Mục đích**: Khởi tạo các sự kiện cho form.

**Events được đăng ký**:
1. **Bar button events**:
   - `ListDataBarButtonItem.ItemClick` → `ListDataBarButtonItem_ItemClick()`
   - `NewBarButtonItem.ItemClick` → `NewBarButtonItem_ItemClick()`
   - `EditBarButtonItem.ItemClick` → `EditBarButtonItem_ItemClick()`
   - `DeleteBarButtonItem.ItemClick` → `DeleteBarButtonItem_ItemClick()`
   - `ExportBarButtonItem.ItemClick` → `ExportBarButtonItem_ItemClick()`

2. **Grid events**:
   - `BusinessPartnerSiteListDtoGridView.SelectionChanged` → `BusinessPartnerSiteListDtoGridView_SelectionChanged()`
   - `BusinessPartnerSiteListDtoGridView.DoubleClick` → `BusinessPartnerSiteListDtoGridView_DoubleClick()`
   - `BusinessPartnerSiteListDtoGridView.CustomDrawRowIndicator` → `BusinessPartnerSiteListDtoGridView_CustomDrawRowIndicator()`
   - `BusinessPartnerSiteListDtoGridView.RowCellStyle` → `BusinessPartnerSiteListDtoGridView_RowCellStyle()`

3. **HTML rendering**:
   - Enable HTML rendering cho `HtmlHypertextLabel`

---

### 3.2. Data Loading

#### `LoadDataAsync()`
```csharp
private async Task LoadDataAsync()
```
**Mục đích**: Tải dữ liệu và bind vào Grid (Async, hiển thị WaitForm).

**Luồng xử lý**:
1. **Guard check**: Kiểm tra `_isLoading` để tránh re-entrancy
   ```csharp
   if (_isLoading) return;
   _isLoading = true;
   ```
2. **Hiển thị waiting form**: `ExecuteWithWaitingFormAsync()`
3. **Load dữ liệu**: `LoadDataAsyncWithoutSplash()`
4. **Reset flag**: `_isLoading = false` (trong finally block)

**Lưu ý**: 
- Guard `_isLoading` ngăn gọi song song khi user nhấn nhiều lần
- Sử dụng waiting form để cải thiện UX

---

#### `LoadDataAsyncWithoutSplash()`
```csharp
private async Task LoadDataAsyncWithoutSplash()
```
**Mục đích**: Tải dữ liệu và bind vào Grid (Async, không hiển thị WaitForm).

**Luồng xử lý**:
1. **Lấy entities từ BLL**:
   ```csharp
   var entities = await Task.Run(() => _businessPartnerSiteBll.GetAll());
   ```
   - Sử dụng `Task.Run()` để chạy synchronous method trong background thread
2. **Convert entities sang DTOs**:
   ```csharp
   _dataList = entities.ToSiteListDtos().ToList();
   ```
   - Extension method `ToSiteListDtos()` convert entities sang DTOs
3. **Bind vào Grid**: `BindGrid(_dataList)`

**Sử dụng**: 
- Được gọi từ `LoadDataAsync()` (với waiting form)
- Được gọi sau khi xóa thành công (không cần waiting form vì đã có)

---

#### `BindGrid(List<BusinessPartnerSiteListDto> data)`
```csharp
private void BindGrid(List<BusinessPartnerSiteListDto> data)
```
**Mục đích**: Bind danh sách DTO vào Grid và cấu hình hiển thị.

**Luồng xử lý**:
1. **Bind datasource**:
   ```csharp
   businessPartnerSiteListDtoBindingSource.DataSource = data;
   ```
2. **Auto-fit columns**: `BusinessPartnerSiteListDtoGridView.BestFitColumns()`
3. **Cấu hình multi-line**: `ConfigureMultiLineGridView()`
4. **Cập nhật summary**: `UpdateDataSummary()`
5. **Cập nhật button states**: `UpdateButtonStates()`

**Lưu ý**: 
- `BestFitColumns()` tự động điều chỉnh độ rộng cột để vừa nội dung
- `ConfigureMultiLineGridView()` được gọi lại để đảm bảo cấu hình đúng

---

#### `UpdateDataSummary()`
```csharp
private void UpdateDataSummary()
```
**Mục đích**: Cập nhật thông tin tổng hợp dữ liệu trên status bar.

**Luồng xử lý**:
1. **Tính tổng số**: `totalCount = _dataList?.Count ?? 0`
2. **Tính số đang hoạt động**: `activeCount = _dataList?.Count(x => x.IsActive) ?? 0`
3. **Cập nhật caption**:
   ```csharp
   DataSummaryBarStaticItem.Caption = $"Tổng: {totalCount} | Hoạt động: {activeCount}";
   ```

**Hiển thị**: Status bar hiển thị "Tổng: X | Hoạt động: Y"

---

### 3.3. Single Row Update (Optimization)

#### `UpdateSingleRowInDataSource(BusinessPartnerSiteListDto updatedDto)`
```csharp
private void UpdateSingleRowInDataSource(BusinessPartnerSiteListDto updatedDto)
```
**Mục đích**: Cập nhật một dòng trong datasource thay vì reload toàn bộ (cải thiện UX).

**Luồng xử lý**:
1. **Validation**: Kiểm tra `updatedDto` và `bindingSource.DataSource` không null
2. **Tìm dòng cần update**:
   ```csharp
   var index = dataList.FindIndex(d => d.Id == updatedDto.Id);
   ```
3. **Update dòng hiện có** (nếu tìm thấy):
   ```csharp
   if (index >= 0)
   {
       dataList[index] = updatedDto;
       businessPartnerSiteListDtoBindingSource.ResetBindings(false);
       BusinessPartnerSiteListDtoGridView.RefreshRow(rowHandle);
   }
   ```
4. **Thêm dòng mới** (nếu không tìm thấy - trường hợp thêm mới):
   ```csharp
   else
   {
       dataList.Insert(0, updatedDto);
       businessPartnerSiteListDtoBindingSource.ResetBindings(false);
   }
   ```
5. **Cập nhật summary**: `UpdateDataSummary()`
6. **Fallback**: Nếu có lỗi, reload toàn bộ: `LoadDataAsync()`

**Lợi ích**: 
- Không cần reload toàn bộ → Nhanh hơn
- Giữ nguyên selection và scroll position
- Cải thiện UX đáng kể

**Lưu ý**: 
- Chỉ update khi form detail trigger event `SiteSaved`
- Fallback về reload toàn bộ nếu có lỗi

---

### 3.4. Event Handlers - Bar Buttons

#### `ListDataBarButtonItem_ItemClick()`
```csharp
private async void ListDataBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
```
**Mục đích**: Xử lý sự kiện click button "Danh sách" - Tải lại dữ liệu.

**Luồng xử lý**:
1. Gọi `LoadDataAsync()` để tải lại dữ liệu
2. Hiển thị waiting form trong quá trình load

---

#### `NewBarButtonItem_ItemClick()`
```csharp
private void NewBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
```
**Mục đích**: Xử lý sự kiện click button "Mới" - Mở form thêm mới.

**Luồng xử lý**:
1. **Hiển thị overlay**: `OverlayManager.ShowScope(this)` - Làm mờ form cha
2. **Tạo form detail**: `new FrmBusinessPartnerSiteDetail(Guid.Empty)`
3. **Đăng ký event**:
   ```csharp
   form.SiteSaved += (updatedDto) =>
   {
       UpdateSingleRowInDataSource(updatedDto);
   };
   ```
4. **Hiển thị form modal**: `form.ShowDialog(this)`
5. **Cập nhật button states**: `UpdateButtonStates()` (nếu DialogResult.OK)

**Lưu ý**: 
- Sử dụng `OverlayManager` để cải thiện UX khi mở form modal
- Event `SiteSaved` được trigger từ form detail sau khi lưu thành công
- Single row update thay vì reload toàn bộ

---

#### `EditBarButtonItem_ItemClick()`
```csharp
private async void EditBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
```
**Mục đích**: Xử lý sự kiện click button "Điều chỉnh" - Mở form chỉnh sửa.

**Luồng xử lý**:
1. **Validation**: Kiểm tra `_selectedItem != null`
   - Nếu null → Hiển thị thông báo và return
2. **Hiển thị overlay**: `OverlayManager.ShowScope(this)`
3. **Tạo form detail**: `new FrmBusinessPartnerSiteDetail(_selectedItem.Id)`
4. **Đăng ký event**: `form.SiteSaved += (updatedDto) => UpdateSingleRowInDataSource(updatedDto)`
5. **Hiển thị form modal**: `form.ShowDialog(this)`
6. **Cập nhật button states**: `UpdateButtonStates()` (nếu DialogResult.OK)

**Lưu ý**: 
- Chỉ enable khi có selection
- Sử dụng `_selectedItem.Id` để load dữ liệu trong form detail

---

#### `DeleteBarButtonItem_ItemClick()`
```csharp
private async void DeleteBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
```
**Mục đích**: Xử lý sự kiện click button "Xóa" - Xóa chi nhánh đã chọn.

**Luồng xử lý**:
1. **Validation**: Kiểm tra `_selectedItem != null`
   - Nếu null → Hiển thị thông báo và return
2. **Xác nhận xóa**: `MsgBox.ShowYesNo(confirmMessage)`
   - Nếu không xác nhận → Return
3. **Xóa với waiting form**: `ExecuteWithWaitingFormAsync()`
   ```csharp
   var success = await Task.Run(() => _businessPartnerSiteBll.DeleteSite(_selectedItem.Id));
   ```
4. **Xử lý kết quả**:
   - Nếu thành công → Thông báo và reload dữ liệu (`LoadDataAsyncWithoutSplash()`)
   - Nếu thất bại → Hiển thị lỗi

**Lưu ý**: 
- Có xác nhận trước khi xóa
- Reload dữ liệu sau khi xóa thành công (không dùng single row update vì đã xóa)

---

#### `ExportBarButtonItem_ItemClick()`
```csharp
private void ExportBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
```
**Mục đích**: Xử lý sự kiện click button "Xuất" - Xuất danh sách ra Excel.

**Luồng xử lý**:
1. **Validation**: Kiểm tra có dữ liệu hiển thị không
   ```csharp
   var rowCount = GridViewHelper.GetDisplayRowCount(BusinessPartnerSiteListDtoGridView) ?? 0;
   if (rowCount <= 0)
   {
       ShowInfo("Không có dữ liệu để xuất.");
       return;
   }
   ```
2. **Xuất Excel**: `GridViewHelper.ExportGridControl(BusinessPartnerSiteListDtoGridView, "BusinessPartnerSites.xlsx")`

**Lưu ý**: 
- Chỉ xuất dữ liệu đang hiển thị (sau filter)
- Sử dụng `GridViewHelper` để xuất Excel

---

### 3.5. Event Handlers - Grid

#### `BusinessPartnerSiteListDtoGridView_SelectionChanged()`
```csharp
private void BusinessPartnerSiteListDtoGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
```
**Mục đích**: Xử lý sự kiện thay đổi selection trên GridView.

**Luồng xử lý**:
1. **Lấy dòng được focus**:
   ```csharp
   if (sender is GridView view && view.FocusedRowHandle >= 0)
   {
       _selectedItem = view.GetFocusedRow() as BusinessPartnerSiteListDto;
       UpdateSelectedRowInfo();
   }
   ```
2. **Clear selection** (nếu không có dòng nào được chọn):
   ```csharp
   else
   {
       _selectedItem = null;
       SelectedRowBarStaticItem.Caption = "Chưa chọn dòng nào";
   }
   ```
3. **Cập nhật button states**: `UpdateButtonStates()`

**Lưu ý**: 
- Cập nhật `_selectedItem` mỗi khi selection thay đổi
- Cập nhật status bar và button states

---

#### `BusinessPartnerSiteListDtoGridView_DoubleClick()`
```csharp
private async void BusinessPartnerSiteListDtoGridView_DoubleClick(object sender, EventArgs e)
```
**Mục đích**: Xử lý sự kiện double click trên GridView - Mở form chỉnh sửa.

**Luồng xử lý**:
1. **Validation**: Kiểm tra `_selectedItem != null`
2. **Hiển thị overlay**: `OverlayManager.ShowScope(this)`
3. **Tạo form detail**: `new FrmBusinessPartnerSiteDetail(_selectedItem.Id)`
4. **Đăng ký event**: `form.SiteSaved += (updatedDto) => UpdateSingleRowInDataSource(updatedDto)`
5. **Hiển thị form modal**: `form.ShowDialog(this)`
6. **Cập nhật button states**: `UpdateButtonStates()` (nếu DialogResult.OK)

**Lưu ý**: 
- Tương tự `EditBarButtonItem_ItemClick()` nhưng trigger bằng double click
- Cải thiện UX - user có thể double click để sửa nhanh

---

#### `BusinessPartnerSiteListDtoGridView_CustomDrawRowIndicator()`
```csharp
private void BusinessPartnerSiteListDtoGridView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
```
**Mục đích**: Xử lý sự kiện vẽ số thứ tự dòng.

**Luồng xử lý**:
```csharp
GridViewHelper.CustomDrawRowIndicator(BusinessPartnerSiteListDtoGridView, e);
```

**Lưu ý**: 
- Sử dụng helper chung để vẽ số thứ tự dòng
- Đảm bảo consistency với các grid khác

---

#### `BusinessPartnerSiteListDtoGridView_RowCellStyle()`
```csharp
private void BusinessPartnerSiteListDtoGridView_RowCellStyle(object sender, RowCellStyleEventArgs e)
```
**Mục đích**: Xử lý sự kiện tô màu dòng theo trạng thái.

**Luồng xử lý**:
1. **Validation**: Kiểm tra sender là GridView, rowHandle hợp lệ, row là DTO
2. **Bỏ qua nếu đang chọn**: `if (view.IsRowSelected(e.RowHandle)) return;`
   - Giữ màu chọn mặc định của DevExpress
3. **Tô màu nếu không hoạt động**:
   ```csharp
   if (!row.IsActive)
   {
       e.Appearance.BackColor = Color.FromArgb(255, 205, 210); // Light Red
       e.Appearance.ForeColor = Color.DarkRed;
       e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Strikeout);
   }
   ```

**Hiển thị**: 
- Dòng không hoạt động: Nền đỏ nhạt, chữ đỏ đậm, gạch ngang
- Dòng đang hoạt động: Màu mặc định

**Lưu ý**: 
- Không ghi đè màu khi đang chọn để giữ màu chọn mặc định
- Có try-catch để ignore style errors

---

### 3.6. Grid Configuration

#### `ConfigureMultiLineGridView()`
```csharp
private void ConfigureMultiLineGridView()
```
**Mục đích**: Cấu hình GridView để hiển thị dữ liệu xuống dòng (word wrap) cho các cột văn bản dài.

**Luồng xử lý**:
1. **Bật auto-height**: `BusinessPartnerSiteListDtoGridView.OptionsView.RowAutoHeight = true;`
   - Tự động điều chỉnh chiều cao dòng để hiển thị đầy đủ nội dung
2. **Tạo RepositoryItemMemoEdit**:
   ```csharp
   var memo = new RepositoryItemMemoEdit
   {
       WordWrap = true,
       AutoHeight = false
   };
   memo.Appearance.TextOptions.WordWrap = WordWrap.Wrap;
   ```
3. **Áp dụng cho các cột**:
   - `ApplyMemoEditorToColumn("SiteName", memo)`
   - `ApplyMemoEditorToColumn("SiteFullAddress", memo)`
   - `ApplyMemoEditorToColumn("Address", memo)`
   - `ApplyMemoEditorToColumn("Notes", memo)`
4. **Cấu hình header**: Căn giữa tiêu đề

**Lưu ý**: 
- `RowAutoHeight = true` cho phép dòng tự động mở rộng để hiển thị đầy đủ nội dung
- `WordWrap = true` cho phép text xuống dòng khi quá dài

---

#### `ApplyMemoEditorToColumn(string fieldName, RepositoryItemMemoEdit memo)`
```csharp
private void ApplyMemoEditorToColumn(string fieldName, RepositoryItemMemoEdit memo)
```
**Mục đích**: Áp dụng RepositoryItemMemoEdit cho cột cụ thể.

**Luồng xử lý**:
1. **Tìm cột**: `var col = BusinessPartnerSiteListDtoGridView.Columns[fieldName];`
2. **Thêm repository vào GridControl** (nếu chưa có):
   ```csharp
   if (!BusinessPartnerSiteListDtoGridControl.RepositoryItems.Contains(memo))
   {
       BusinessPartnerSiteListDtoGridControl.RepositoryItems.Add(memo);
   }
   ```
3. **Gán repository cho cột**: `col.ColumnEdit = memo;`

**Lưu ý**: 
- Repository phải được thêm vào GridControl trước khi gán cho cột
- Mỗi repository chỉ cần thêm một lần

---

### 3.7. Utility Methods

#### `ExecuteWithWaitingFormAsync(Func<Task> operation)`
```csharp
private async Task ExecuteWithWaitingFormAsync(Func<Task> operation)
```
**Mục đích**: Thực hiện operation async với WaitingForm1 hiển thị.

**Luồng xử lý**:
1. **Hiển thị waiting form**: `SplashScreenManager.ShowForm(typeof(WaitForm1));`
2. **Thực hiện operation**: `await operation();`
3. **Đóng waiting form**: `SplashScreenManager.CloseForm();` (trong finally block)

**Lưu ý**: 
- Đảm bảo waiting form luôn được đóng kể cả khi có exception
- Cải thiện UX khi thực hiện operations mất thời gian

---

#### `UpdateButtonStates()`
```csharp
private void UpdateButtonStates()
```
**Mục đích**: Cập nhật trạng thái các nút toolbar dựa trên selection.

**Luồng xử lý**:
1. **Kiểm tra selection**: `var hasSelection = _selectedItem != null;`
2. **Cập nhật Edit button**: `EditBarButtonItem.Enabled = hasSelection;`
   - Chỉ enable khi chọn đúng 1 dòng
3. **Cập nhật Delete button**: `DeleteBarButtonItem.Enabled = hasSelection;`
   - Enable khi chọn >= 1 dòng
4. **Cập nhật Export button**:
   ```csharp
   var rowCount = GridViewHelper.GetDisplayRowCount(BusinessPartnerSiteListDtoGridView) ?? 0;
   ExportBarButtonItem.Enabled = rowCount > 0;
   ```
   - Enable khi có dữ liệu hiển thị

**Lưu ý**: 
- Được gọi mỗi khi selection thay đổi
- Được gọi sau khi load dữ liệu
- Có try-catch để ignore errors

---

#### `UpdateSelectedRowInfo()`
```csharp
private void UpdateSelectedRowInfo()
```
**Mục đích**: Cập nhật thông tin dòng được chọn trên status bar.

**Luồng xử lý**:
```csharp
if (_selectedItem != null)
{
    SelectedRowBarStaticItem.Caption = $"Đang chọn: {_selectedItem.SiteName}";
}
else
{
    SelectedRowBarStaticItem.Caption = "Chưa chọn dòng nào";
}
```

**Hiển thị**: Status bar hiển thị "Đang chọn: [Tên chi nhánh]" hoặc "Chưa chọn dòng nào"

---

#### `SetupSuperToolTips()`
```csharp
private void SetupSuperToolTips()
```
**Mục đích**: Thiết lập SuperToolTip cho các controls trong form.

**Tooltips được thiết lập**:
- 🔄 Tải dữ liệu: "Tải lại danh sách chi nhánh đối tác từ hệ thống."
- ➕ Thêm mới: "Thêm mới chi nhánh đối tác vào hệ thống."
- ✏️ Sửa: "Chỉnh sửa thông tin chi nhánh đối tác đã chọn."
- 🗑️ Xóa: "Xóa chi nhánh đối tác đã chọn khỏi hệ thống."
- 📊 Xuất Excel: "Xuất danh sách chi nhánh đối tác ra file Excel."

**Lưu ý**: 
- Có try-catch để không chặn form nếu setup tooltip lỗi
- Sử dụng `SuperToolTipHelper` để setup tooltip

---

#### `ShowInfo(string message)`, `ShowError()`
```csharp
private void ShowInfo(string message)
private void ShowError(Exception ex, string context = null)
private void ShowError(string message)
```
**Mục đích**: Helper methods để hiển thị thông báo.

**Sử dụng**:
- `ShowInfo()` → Thông báo thành công/info
- `ShowError(Exception, string)` → Thông báo lỗi với exception và context
- `ShowError(string)` → Thông báo lỗi đơn giản

---

## 4. Luồng Xử Lý Dữ Liệu

### 4.1. Luồng Load Dữ Liệu

```
User clicks "Danh sách" button
         │
         ▼
ListDataBarButtonItem_ItemClick()
         │
         ├─> LoadDataAsync()
         │   │
         │   ├─> Check _isLoading (guard)
         │   │   └─> If loading → Return
         │   │
         │   ├─> Set _isLoading = true
         │   │
         │   ├─> ExecuteWithWaitingFormAsync()
         │   │   │
         │   │   ├─> Show waiting form
         │   │   │
         │   │   ├─> LoadDataAsyncWithoutSplash()
         │   │   │   │
         │   │   │   ├─> BLL.GetAll()
         │   │   │   │   │
         │   │   │   │   └─> [BLL] → [DAL] → Database SELECT
         │   │   │   │
         │   │   │   ├─> entities.ToSiteListDtos()
         │   │   │   │   │
         │   │   │   │   └─> Convert Entity → DTO
         │   │   │   │
         │   │   │   ├─> _dataList = dtoList
         │   │   │   │
         │   │   │   └─> BindGrid(_dataList)
         │   │   │       │
         │   │   │       ├─> businessPartnerSiteListDtoBindingSource.DataSource = data
         │   │   │       ├─> BestFitColumns()
         │   │   │       ├─> ConfigureMultiLineGridView()
         │   │   │       ├─> UpdateDataSummary()
         │   │   │       └─> UpdateButtonStates()
         │   │   │
         │   │   └─> Close waiting form
         │   │
         │   └─> Set _isLoading = false (finally)
         │
         ▼
Grid displays data
```

### 4.2. Luồng Thêm Mới

```
User clicks "Mới" button
         │
         ▼
NewBarButtonItem_ItemClick()
         │
         ├─> OverlayManager.ShowScope(this)
         │
         ├─> new FrmBusinessPartnerSiteDetail(Guid.Empty)
         │
         ├─> form.SiteSaved += (updatedDto) => UpdateSingleRowInDataSource(updatedDto)
         │
         ├─> form.ShowDialog(this)
         │   │
         │   └─> [User enters data and saves]
         │       │
         │       └─> Form detail triggers SiteSaved event
         │           │
         │           └─> UpdateSingleRowInDataSource(updatedDto)
         │               │
         │               ├─> Find index by Id
         │               ├─> If found → Update row
         │               │   ├─> dataList[index] = updatedDto
         │               │   ├─> ResetBindings(false)
         │               │   └─> RefreshRow(rowHandle)
         │               │
         │               └─> If not found → Insert at top
         │                   ├─> dataList.Insert(0, updatedDto)
         │                   └─> ResetBindings(false)
         │
         ├─> UpdateButtonStates()
         │
         └─> OverlayManager closes
         │
         ▼
Grid updated with new row (no full reload)
```

### 4.3. Luồng Chỉnh Sửa

```
User clicks "Điều chỉnh" button (or double clicks row)
         │
         ▼
EditBarButtonItem_ItemClick() or BusinessPartnerSiteListDtoGridView_DoubleClick()
         │
         ├─> Check _selectedItem != null
         │   └─> If null → Show info and return
         │
         ├─> OverlayManager.ShowScope(this)
         │
         ├─> new FrmBusinessPartnerSiteDetail(_selectedItem.Id)
         │   │
         │   └─> [Form detail loads existing data]
         │
         ├─> form.SiteSaved += (updatedDto) => UpdateSingleRowInDataSource(updatedDto)
         │
         ├─> form.ShowDialog(this)
         │   │
         │   └─> [User modifies data and saves]
         │       │
         │       └─> Form detail triggers SiteSaved event
         │           │
         │           └─> UpdateSingleRowInDataSource(updatedDto)
         │               │
         │               ├─> Find index by Id
         │               ├─> Update row: dataList[index] = updatedDto
         │               ├─> ResetBindings(false)
         │               └─> RefreshRow(rowHandle)
         │
         ├─> UpdateButtonStates()
         │
         └─> OverlayManager closes
         │
         ▼
Grid updated with modified row (no full reload)
```

### 4.4. Luồng Xóa

```
User clicks "Xóa" button
         │
         ▼
DeleteBarButtonItem_ItemClick()
         │
         ├─> Check _selectedItem != null
         │   └─> If null → Show info and return
         │
         ├─> MsgBox.ShowYesNo("Bạn có chắc muốn xóa...?")
         │   └─> If No → Return
         │
         ├─> ExecuteWithWaitingFormAsync()
         │   │
         │   ├─> Show waiting form
         │   │
         │   ├─> BLL.DeleteSite(_selectedItem.Id)
         │   │   │
         │   │   └─> [BLL] → [DAL] → Database DELETE
         │   │
         │   ├─> If success
         │   │   ├─> ShowInfo("Xóa chi nhánh thành công!")
         │   │   └─> LoadDataAsyncWithoutSplash()
         │   │       │
         │   │       ├─> BLL.GetAll()
         │   │       ├─> Convert to DTOs
         │   │       └─> BindGrid()
         │   │
         │   └─> If failed
         │       └─> ShowError("Không thể xóa...")
         │
         └─> Close waiting form
         │
         ▼
Grid reloaded (row removed)
```

### 4.5. Luồng Selection Changed

```
User selects/deselects row in Grid
         │
         ▼
BusinessPartnerSiteListDtoGridView_SelectionChanged()
         │
         ├─> Check view.FocusedRowHandle >= 0
         │   │
         │   ├─> If valid
         │   │   ├─> _selectedItem = view.GetFocusedRow() as DTO
         │   │   └─> UpdateSelectedRowInfo()
         │   │       │
         │   │       └─> SelectedRowBarStaticItem.Caption = "Đang chọn: {SiteName}"
         │   │
         │   └─> If invalid
         │       ├─> _selectedItem = null
         │       └─> SelectedRowBarStaticItem.Caption = "Chưa chọn dòng nào"
         │
         └─> UpdateButtonStates()
             │
             ├─> EditBarButtonItem.Enabled = hasSelection
             ├─> DeleteBarButtonItem.Enabled = hasSelection
             └─> ExportBarButtonItem.Enabled = rowCount > 0
         │
         ▼
Status bar and buttons updated
```

### 4.6. Luồng Row Styling

```
Grid renders row
         │
         ▼
BusinessPartnerSiteListDtoGridView_RowCellStyle()
         │
         ├─> Check sender is GridView
         ├─> Check rowHandle >= 0
         ├─> Get row as BusinessPartnerSiteListDto
         ├─> Check if row is selected
         │   └─> If selected → Return (keep default selection color)
         │
         ├─> Check row.IsActive
         │   │
         │   ├─> If active → Return (keep default color)
         │   │
         │   └─> If not active
         │       ├─> e.Appearance.BackColor = Light Red
         │       ├─> e.Appearance.ForeColor = Dark Red
         │       └─> e.Appearance.Font = Strikeout
         │
         ▼
Row styled (inactive rows highlighted)
```

---

## 5. Lưu Ý Khi Mở Rộng Hoặc Sửa Đổi

### 5.1. Guard _isLoading

⚠️ **Quan trọng**: Flag `_isLoading` ngăn gọi `LoadDataAsync()` song song.

**Khi nào set flag**:
- `_isLoading = true` trước khi load dữ liệu
- `_isLoading = false` sau khi load xong (trong finally block)

**Khi nào check flag**:
- Trong `LoadDataAsync()`:
  ```csharp
  if (_isLoading) return; // Skip if already loading
  ```

**Lưu ý**: 
- Nếu không có guard, user nhấn nhiều lần sẽ gọi load song song → Lỗi hoặc chậm
- Luôn reset flag trong finally block để đảm bảo flag luôn được reset

---

### 5.2. Single Row Update vs Full Reload

✅ **Pattern hiện tại**: Sử dụng `UpdateSingleRowInDataSource()` để cập nhật single row thay vì reload toàn bộ.

**Khi nào dùng single row update**:
- Sau khi thêm mới thành công (event `SiteSaved`)
- Sau khi chỉnh sửa thành công (event `SiteSaved`)

**Khi nào dùng full reload**:
- Sau khi xóa thành công (không thể update vì đã xóa)
- Khi có lỗi trong single row update (fallback)
- Khi user nhấn "Danh sách" để reload

**Lợi ích của single row update**:
- Nhanh hơn (không cần query database lại)
- Giữ nguyên selection và scroll position
- Cải thiện UX đáng kể

**Khi thêm logic mới**:
- Nếu chỉ cập nhật một dòng → Dùng `UpdateSingleRowInDataSource()`
- Nếu cần reload toàn bộ → Dùng `LoadDataAsync()` hoặc `LoadDataAsyncWithoutSplash()`

---

### 5.3. Event SiteSaved

✅ **Pattern hiện tại**: Sử dụng event để communicate giữa form detail và form list.

**Cách sử dụng**:
```csharp
form.SiteSaved += (updatedDto) =>
{
    UpdateSingleRowInDataSource(updatedDto);
};
```

**Lưu ý**: 
- Event được trigger từ form detail sau khi lưu thành công
- `updatedDto` chứa đầy đủ thông tin để update grid
- Form list có thể update datasource mà không cần reload toàn bộ

**Khi thêm logic mới**:
- Nếu cần thông báo form list về thay đổi → Sử dụng event pattern tương tự
- Đảm bảo DTO chứa đầy đủ thông tin cần thiết

---

### 5.4. OverlayManager

✅ **Pattern hiện tại**: Sử dụng `OverlayManager` khi mở form modal.

**Cách sử dụng**:
```csharp
using (OverlayManager.ShowScope(this))
{
    using (var form = new FrmBusinessPartnerSiteDetail(id))
    {
        form.ShowDialog(this);
    }
}
```

**Lợi ích**: 
- Làm mờ form cha khi mở form modal
- Cải thiện UX - user biết form cha đang bị block

**Lưu ý**: 
- Sử dụng `using` để đảm bảo overlay được đóng đúng cách
- Chỉ dùng khi mở form modal

---

### 5.5. Grid Configuration

⚠️ **Lưu ý**: Grid được cấu hình với nhiều tính năng đặc biệt.

**Các cấu hình quan trọng**:
1. **RowAutoHeight = true**: Tự động điều chỉnh chiều cao dòng
2. **WordWrap**: Text xuống dòng khi quá dài
3. **RepositoryItemMemoEdit**: Cho các cột văn bản dài
4. **RepositoryItemHypertextLabel**: Cho cột HTML
5. **Multi-select**: Hỗ trợ chọn nhiều dòng
6. **Auto-filter row**: Hỗ trợ tìm kiếm nhanh

**Khi thêm cột mới**:
- Nếu cột là văn bản dài → Áp dụng `RepositoryItemMemoEdit` với word wrap
- Nếu cột là HTML → Áp dụng `RepositoryItemHypertextLabel`
- Đảm bảo `RowAutoHeight = true` để hiển thị đầy đủ

---

### 5.6. Row Styling

⚠️ **Lưu ý**: Row styling được áp dụng để highlight dòng không hoạt động.

**Logic hiện tại**:
- Dòng không hoạt động: Nền đỏ nhạt, chữ đỏ đậm, gạch ngang
- Dòng đang hoạt động: Màu mặc định
- Dòng đang chọn: Giữ màu chọn mặc định (không ghi đè)

**Khi thêm styling mới**:
- Luôn check `view.IsRowSelected(e.RowHandle)` để không ghi đè màu chọn
- Có try-catch để ignore style errors
- Sử dụng màu sắc rõ ràng, dễ phân biệt

---

### 5.7. Button States Management

✅ **Pattern hiện tại**: Button states được cập nhật dựa trên selection và data.

**Logic**:
- **Edit**: Enable khi có selection (`_selectedItem != null`)
- **Delete**: Enable khi có selection (`_selectedItem != null`)
- **Export**: Enable khi có dữ liệu hiển thị (`rowCount > 0`)

**Khi thêm button mới**:
- Xác định điều kiện enable/disable
- Cập nhật trong `UpdateButtonStates()`
- Đảm bảo được gọi khi cần (selection changed, data loaded, v.v.)

---

### 5.8. Error Handling

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
- Log exception nếu cần
- Không "nuốt" exception mà không thông báo
- Single row update có fallback về full reload nếu có lỗi

---

### 5.9. Data Summary

✅ **Pattern hiện tại**: Status bar hiển thị tổng số và số đang hoạt động.

**Khi thêm thông tin mới**:
- Cập nhật `UpdateDataSummary()` để tính toán thêm
- Đảm bảo được gọi sau khi load dữ liệu và sau khi update single row

---

### 5.10. Export Functionality

⚠️ **Lưu ý**: Export chỉ xuất dữ liệu đang hiển thị (sau filter).

**Khi thêm tính năng export mới**:
- Kiểm tra có dữ liệu hiển thị không
- Sử dụng `GridViewHelper.ExportGridControl()` hoặc method tương tự
- Đảm bảo export đúng dữ liệu đang hiển thị (không phải tất cả)

---

### 5.11. HTML Rendering

⚠️ **Lưu ý**: Grid hiển thị thông tin chi nhánh dưới dạng HTML trong một cột duy nhất.

**Cấu hình**:
- Cột `colThongTinHtml` sử dụng `RepositoryItemHypertextLabel`
- `AllowHtmlDraw = true` để enable HTML rendering
- DTO có property `ThongTinHtml` chứa HTML string

**Khi thêm HTML rendering mới**:
- Đảm bảo DTO có property chứa HTML string
- Sử dụng `RepositoryItemHypertextLabel` cho cột
- Enable `AllowHtmlDraw = true`

---

### 5.12. Testing

✅ **Khi thêm/chỉnh sửa code**:
- Test load dữ liệu (có và không có dữ liệu)
- Test thêm mới (single row update)
- Test chỉnh sửa (single row update)
- Test xóa (full reload)
- Test export (có và không có dữ liệu)
- Test selection changed (button states)
- Test double click (mở form edit)
- Test row styling (active/inactive)
- Test với dữ liệu lớn (performance)
- Test guard `_isLoading` (nhấn nhiều lần)

---

### 5.13. Code Style

✅ **Tuân thủ**:
- Sử dụng regions để tổ chức code
- XML documentation comments cho public/protected methods
- Naming convention:
  - Private methods: `PascalCase`
  - Private fields: `_camelCase`
  - Events: `ObjectName_EventName`

---

### 5.14. Dependencies

⚠️ **Khi thay đổi dependencies**:
- `BusinessPartnerSiteBll`: Nếu thay đổi interface, cần update tất cả calls
- `BusinessPartnerSiteListDto`: Nếu thêm/sửa properties, cần update:
  - Grid columns
  - Row styling logic
  - Export functionality
- `FrmBusinessPartnerSiteDetail`: Nếu thay đổi event `SiteSaved`, cần update event handler
- `GridViewHelper`: Nếu thay đổi methods, cần update calls

---

## 6. Tóm Tắt

### Điểm Mạnh:
✅ Single row update thay vì full reload → Cải thiện UX  
✅ Event-driven architecture → Loose coupling  
✅ HTML rendering → Hiển thị thông tin đẹp  
✅ Multi-line display → Hiển thị đầy đủ nội dung  
✅ Row styling → Dễ phân biệt trạng thái  
✅ Guard `_isLoading` → Tránh re-entrancy  
✅ OverlayManager → Cải thiện UX khi mở form modal  
✅ Status bar → Hiển thị thông tin hữu ích  
✅ SuperToolTip → Hỗ trợ user  
✅ Multi-select support → Linh hoạt  

### Điểm Cần Lưu Ý:
⚠️ Guard `_isLoading` cần được quản lý cẩn thận  
⚠️ Single row update có fallback về full reload  
⚠️ HTML rendering phụ thuộc vào DTO property  
⚠️ Row styling không ghi đè màu chọn  
⚠️ Export chỉ xuất dữ liệu đang hiển thị  

### Khuyến Nghị:
💡 Cân nhắc thêm pagination cho dataset lớn  
💡 Cân nhắc thêm sorting/filtering nâng cao  
💡 Cân nhắc thêm undo/redo cho delete  
💡 Cân nhắc thêm batch operations (xóa nhiều dòng)  
💡 Cân nhắc thêm search box riêng (ngoài auto-filter)  

---

**Tài liệu này được tạo tự động dựa trên phân tích code. Cập nhật lần cuối: 2025-01-XX**
