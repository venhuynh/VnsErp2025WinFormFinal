# Migration: Add VariantNameForReport Column to ProductVariant Table

## Mô tả

Migration này thêm cột `VariantNameForReport` vào bảng `ProductVariant` để lưu trữ tên biến thể sản phẩm đã được xử lý (loại bỏ HTML tags), phục vụ cho việc hiển thị trong các report.

## Lý do

- Cột `VariantFullName` hiện tại có thể chứa HTML tags (ví dụ: `<b>`, `<color=...>`, `<size=...>`) để hiển thị trong UI
- Khi xuất report, cần hiển thị text thuần không có HTML tags
- Việc loại bỏ HTML tags mỗi lần render report có thể ảnh hưởng đến performance
- Lưu trữ sẵn giá trị đã xử lý giúp tối ưu hiệu suất và đảm bảo tính nhất quán

## Thay đổi

### Bảng: `ProductVariant`

**Thêm cột mới:**
- `VariantNameForReport` (NVARCHAR(MAX), NULL)
  - Lưu trữ tên biến thể sản phẩm đã loại bỏ HTML tags
  - Cho phép NULL để tương thích với dữ liệu hiện có
  - Có thể được populate từ `VariantFullName` sau khi loại bỏ HTML tags

## Cách sử dụng

### 1. Chạy Migration Script

```sql
-- Chạy script migration
-- File: Add_VariantNameForReport_To_ProductVariant.sql
```

### 2. Cập nhật Entity trong Designer

Sau khi chạy migration, cần:
1. Cập nhật LINQ to SQL designer để thêm property `VariantNameForReport` vào entity `ProductVariant`
2. Hoặc regenerate designer từ database

### 3. Populate Dữ liệu

Có thể populate dữ liệu bằng cách:

**Option 1: Qua Application Code**
- Sử dụng method `StripHtmlTags()` trong `StockInOutReportBll` để loại bỏ HTML tags
- Cập nhật `VariantNameForReport` khi tạo mới hoặc cập nhật `VariantFullName`

**Option 2: Qua SQL Script (nếu cần)**
```sql
-- Cập nhật VariantNameForReport từ VariantFullName (loại bỏ HTML tags)
-- Lưu ý: SQL Server không có hàm built-in để strip HTML tags
-- Nên thực hiện qua application code
```

### 4. Sử dụng trong Report

Trong `StockInOutReportBll.ConvertToReportDetailDto()`:
```csharp
// Ưu tiên sử dụng VariantNameForReport nếu có
var productVariantName = productVariant?.VariantNameForReport ?? 
                         StripHtmlTags(productVariant?.VariantFullName ?? string.Empty);
```

## Rollback (nếu cần)

```sql
-- Xóa cột nếu cần rollback
ALTER TABLE [dbo].[ProductVariant]
DROP COLUMN [VariantNameForReport];
GO
```

## Lưu ý

1. **Dữ liệu hiện có**: Cột mới được thêm với giá trị NULL, cần populate dữ liệu sau
2. **Đồng bộ dữ liệu**: Cần đảm bảo `VariantNameForReport` được cập nhật khi `VariantFullName` thay đổi
3. **Performance**: Có thể tạo index nếu cần query theo cột này (tuy nhiên NVARCHAR(MAX) không thể index trực tiếp)

## Tác động

- ✅ Không ảnh hưởng đến dữ liệu hiện có (cột mới cho phép NULL)
- ✅ Không ảnh hưởng đến các chức năng hiện tại
- ✅ Cải thiện performance khi render report (không cần xử lý HTML tags mỗi lần)
- ✅ Đảm bảo tính nhất quán dữ liệu trong report

## Liên quan

- File migration: `Add_VariantNameForReport_To_ProductVariant.sql`
- Entity: `ProductVariant` trong `VnsErp2025.designer.cs`
- BLL: `StockInOutReportBll.ConvertToReportDetailDto()`
- DTO: `StockInOutReportDetailDto.ProductVariantName`

