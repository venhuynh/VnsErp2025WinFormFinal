# TODO: Hoàn Thành Triển Khai ReloadDataSourceBarButtonItem

## ✅ Đã Hoàn Thành

### Màn Hình Nhập Kho
1. **FrmNhapBaoHanh** - ✅ HOÀN THÀNH
   - Designer.cs: Đã thêm button
   - .cs: Đã thêm event handler với Task.WhenAll
   - UcNhapBaoHanhDetail.cs: Đã thêm method `ReloadProductVariantDataSourceAsync()`

2. **FrmNhapLapRap** - ✅ HOÀN THÀNH  
   - Designer.cs: Đã thêm button
   - .cs: Đã thêm event handler với Task.WhenAll
   - UcNhapLapRapLapRapDetailDto.cs: Đã thêm method `ReloadProductVariantDataSourceAsync()`

### Màn Hình Xuất Kho
3. **FrmXuatBaoHanh** - ⚠️ CẦN CẬP NHẬT LOGIC
   - Designer.cs: ✅ Đã thêm button
   - .cs: ⚠️ Đang dùng reflection, cần đổi sang Task.WhenAll
   - UcXuatBaoHanhDetail.cs: ❌ Cần thêm method `ReloadProductVariantDataSourceAsync()`

4. **FrmXuatThietBiChoThueMuon** - ⚠️ CẦN CẬP NHẬT LOGIC
   - Designer.cs: ✅ Đã thêm button
   - .cs: ⚠️ Đang dùng reflection, cần đổi sang Task.WhenAll
   - UcXuatThietBiChoThueMuonDetailDto.cs: ❌ Cần thêm method `ReloadProductVariantDataSourceAsync()`

5. **FrmXuatKhoThuongMai** - ⚠️ CẦN CẬP NHẬT LOGIC
   - Designer.cs: ✅ Đã thêm button
   - .cs: ⚠️ Đang dùng reflection, cần đổi sang Task.WhenAll
   - UcXuatHangThuongMaiDetailDto.cs: ❌ Cần thêm method `ReloadProductVariantDataSourceAsync()`

---

## 📋 Các Bước Còn Lại

### Bước 1: Thêm Method ReloadProductVariantDataSourceAsync() cho UserControl Detail

Thêm method sau vào các UserControl Detail còn lại:

```csharp
/// <summary>
/// Reload ProductVariant datasource (public method để gọi từ Form)
/// </summary>
public async Task ReloadProductVariantDataSourceAsync()
{
    try
    {
        await LoadProductVariantsAsync(forceRefresh: true);
    }
    catch (Exception ex)
    {
        _logger.Error("ReloadProductVariantDataSourceAsync: Exception occurred", ex);
        throw;
    }
}
```

**Files cần cập nhật:**
- `Inventory/StockOut/XuatBaoHanh/UcXuatBaoHanhDetail.cs`
- `Inventory/StockOut/XuatChoThueMuon/UcXuatThietBiChoThueMuonDetailDto.cs`
- `Inventory/StockOut/XuatHangThuongMai/UcXuatHangThuongMaiDetailDto.cs`

### Bước 2: Cập Nhật Logic Reload trong Form

Thay đổi từ reflection sang Task.WhenAll trong các Form:

**Thay thế code cũ (reflection):**
```csharp
// Reload datasource cho Master UserControl
await ucXXXMaster1.LoadLookupDataAsync();

// Reload datasource cho Detail UserControl nếu có method
var detailType = ucXXXDetail1.GetType();
var reloadMethod = detailType.GetMethod("ReloadProductVariantDataSourceAsync");
if (reloadMethod != null)
{
    var task = reloadMethod.Invoke(ucXXXDetail1, null) as Task;
    if (task != null)
    {
        await task;
    }
}
```

**Bằng code mới (Task.WhenAll):**
```csharp
// Reload datasource cho cả 2 UserControl
await Task.WhenAll(
    ucXXXMaster1.LoadLookupDataAsync(),
    ucXXXDetail1.ReloadProductVariantDataSourceAsync()
);
```

**Files cần cập nhật:**
- `Inventory/StockOut/XuatBaoHanh/FrmXuatBaoHanh.cs`
- `Inventory/StockOut/XuatChoThueMuon/FrmXuatThietBiChoThueMuon.cs`
- `Inventory/StockOut/XuatHangThuongMai/FrmXuatKhoThuongMai.cs`

---

## ⚠️ Lưu Ý Quan Trọng

1. **C# Version Issues**: Một số file đang sử dụng C# 9.0+ features (target-typed object creation, using declarations, file-scoped namespace) nhưng project đang dùng C# 7.3. Cần:
   - Thay `new()` bằng `new ClassName()`
   - Thay `using var` bằng `using (var ...)`
   - Thay file-scoped namespace bằng block namespace

2. **Method LoadProductVariantsAsync**: Đảm bảo tất cả UserControl Detail đều có private method `LoadProductVariantsAsync(bool forceRefresh = false)` để public method `ReloadProductVariantDataSourceAsync()` có thể gọi.

3. **Testing**: Sau khi hoàn thành, cần test:
   - Button xuất hiện đúng vị trí trên toolbar
   - Click button reload thành công cả Master và Detail
   - Thông báo success hiển thị
   - Không có lỗi compile hoặc runtime

---

## 🎯 Kết Quả Mong Đợi

Sau khi hoàn thành tất cả các bước:
- 5 màn hình (2 nhập + 3 xuất) đều có nút "Làm mới dữ liệu"
- Nút reload cả Master và Detail UserControl đồng thời
- Logic nhất quán giống FrmNhapKhoThuongMai (sử dụng Task.WhenAll)
- Không có lỗi compile
- SuperToolTip hiển thị đầy đủ thông tin
