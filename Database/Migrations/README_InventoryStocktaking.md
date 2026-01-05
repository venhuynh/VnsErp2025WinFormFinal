# Bảng Quản Lý Kiểm Kho Hàng Tháng

## Tổng quan

Hệ thống quản lý kiểm kho hàng tháng được thiết kế để:
- Quản lý quy trình kiểm kho định kỳ hàng tháng
- So sánh số lượng sổ sách với số lượng thực tế
- Xử lý chênh lệch và điều chỉnh tồn kho
- Hỗ trợ sử dụng mã SKU để định danh sản phẩm

## Cấu trúc bảng

### 1. InventoryStocktaking (Phiếu kiểm kho)

Bảng chính quản lý phiếu kiểm kho theo tháng.

#### Các trường chính:

| Trường | Kiểu | Mô tả |
|--------|------|-------|
| `Id` | uniqueidentifier | ID duy nhất |
| `StocktakingCode` | nvarchar(50) | Mã phiếu kiểm kho (VD: KK202501001) |
| `StocktakingName` | nvarchar(255) | Tên phiếu kiểm kho |
| `WarehouseId` | uniqueidentifier | ID kho (FK -> CompanyBranch) |
| `PeriodYear` | int | Năm kiểm kho (VD: 2025) |
| `PeriodMonth` | int | Tháng kiểm kho (1-12) |
| `StocktakingDate` | datetime | Ngày kiểm kho |
| `StartDate` | datetime | Ngày bắt đầu kiểm kho |
| `EndDate` | datetime | Ngày kết thúc kiểm kho |
| `Status` | int | Trạng thái: 0=Nháp, 1=Đang kiểm, 2=Hoàn thành, 3=Đã xác nhận, 4=Đã hủy |
| `IsLocked` | bit | Đã khóa chưa |
| `LockedDate` | datetime | Ngày khóa |
| `LockedBy` | uniqueidentifier | ID người khóa |
| `CreatedBy` | uniqueidentifier | ID người tạo |
| `CreateDate` | datetime | Ngày tạo |
| `ApprovedBy` | uniqueidentifier | ID người phê duyệt |
| `ApprovedDate` | datetime | Ngày phê duyệt |

#### Quy tắc mã phiếu kiểm kho:

- Format: `KK{YYYYMM}{XXX}`
- Ví dụ: `KK202501001` (Kiểm kho tháng 01/2025, phiếu số 001)
- Tự động tăng số thứ tự theo tháng

### 2. InventoryStocktakingDetail (Chi tiết kiểm kho)

Bảng chi tiết quản lý từng sản phẩm trong phiếu kiểm kho.

#### Các trường chính:

| Trường | Kiểu | Mô tả |
|--------|------|-------|
| `Id` | uniqueidentifier | ID duy nhất |
| `StocktakingId` | uniqueidentifier | ID phiếu kiểm kho (FK) |
| `ProductVariantId` | uniqueidentifier | ID biến thể sản phẩm (FK) |
| `ProductCode` | nvarchar(50) | **Mã sản phẩm (có thể dùng làm SKU)** |
| `ProductVariantCode` | nvarchar(50) | **Mã biến thể (có thể dùng làm SKU)** |
| `ProductName` | nvarchar(500) | Tên sản phẩm |
| `UnitOfMeasureId` | uniqueidentifier | ID đơn vị tính |
| `BookQuantity` | decimal(18,4) | Số lượng sổ sách (từ InventoryBalance) |
| `PhysicalQuantity` | decimal(18,4) | Số lượng thực tế (đếm được) |
| `DifferenceQuantity` | decimal(18,4) | Chênh lệch = PhysicalQuantity - BookQuantity |
| `UnitPrice` | decimal(18,4) | Đơn giá |
| `BookValue` | decimal(18,4) | Giá trị sổ sách |
| `PhysicalValue` | decimal(18,4) | Giá trị thực tế |
| `DifferenceValue` | decimal(18,4) | Chênh lệch giá trị |
| `CountedBy` | uniqueidentifier | ID người đếm |
| `CountedDate` | datetime | Ngày đếm |
| `CountedTimes` | int | Số lần đếm |
| `IsReCount` | bit | Có phải đếm lại không |
| `AdjustmentType` | int | Loại điều chỉnh: 0=Không, 1=Tăng, 2=Giảm |
| `AdjustmentQuantity` | decimal(18,4) | Số lượng điều chỉnh |
| `AdjustmentReason` | nvarchar(500) | Lý do điều chỉnh |
| `IsAdjusted` | bit | Đã điều chỉnh chưa |

## Sử dụng mã SKU

### Có thể dùng mã SKU không?

**Có, hoàn toàn có thể!** Hệ thống hỗ trợ 2 cách sử dụng mã SKU:

1. **ProductCode** (Mã sản phẩm chính)
   - Lấy từ `ProductService.Code`
   - Dùng khi SKU là mã sản phẩm chung

2. **ProductVariantCode** (Mã biến thể)
   - Lấy từ `ProductVariant.VariantCode`
   - Dùng khi SKU là mã biến thể cụ thể

### Index cho tìm kiếm SKU

Cả hai trường đều đã được index để tìm kiếm nhanh:
- `IX_InventoryStocktakingDetail_ProductCode`
- `IX_InventoryStocktakingDetail_ProductVariantCode`

### Ví dụ query theo SKU:

```sql
-- Tìm kiểm kho theo ProductCode (SKU)
SELECT * FROM InventoryStocktakingDetail
WHERE ProductCode = 'SKU001'

-- Tìm kiểm kho theo ProductVariantCode (SKU)
SELECT * FROM InventoryStocktakingDetail
WHERE ProductVariantCode = 'SKU-VAR-001'

-- Tìm kiểm kho theo cả hai (linh hoạt)
SELECT * FROM InventoryStocktakingDetail
WHERE ProductCode = 'SKU001' OR ProductVariantCode = 'SKU001'
```

## Quy trình kiểm kho

### Bước 1: Tạo phiếu kiểm kho

Sử dụng stored procedure `sp_CreateStocktakingFromBalance`:

```sql
DECLARE @Code nvarchar(50), @Id uniqueidentifier
EXEC sp_CreateStocktakingFromBalance
    @WarehouseId = '...',
    @PeriodYear = 2025,
    @PeriodMonth = 1,
    @CreatedBy = '...',
    @StocktakingCode = @Code OUTPUT,
    @StocktakingId = @Id OUTPUT
```

Hoặc tạo thủ công:

```sql
INSERT INTO InventoryStocktaking (...)
VALUES (...)
```

### Bước 2: Nhập số lượng thực tế

Cập nhật `PhysicalQuantity` cho từng sản phẩm:

```sql
UPDATE InventoryStocktakingDetail
SET PhysicalQuantity = 100,
    CountedBy = @UserId,
    CountedDate = GETDATE()
WHERE Id = @DetailId
```

**Lưu ý:** Trigger sẽ tự động tính `DifferenceQuantity`, `PhysicalValue`, và `DifferenceValue`.

### Bước 3: Xử lý chênh lệch

Nếu có chênh lệch, cập nhật thông tin điều chỉnh:

```sql
UPDATE InventoryStocktakingDetail
SET AdjustmentType = 1, -- 1=Tăng, 2=Giảm
    AdjustmentQuantity = ABS(DifferenceQuantity),
    AdjustmentReason = N'Lý do điều chỉnh',
    IsAdjusted = 1
WHERE Id = @DetailId
    AND DifferenceQuantity <> 0
```

### Bước 4: Hoàn thành và phê duyệt

```sql
-- Cập nhật trạng thái
UPDATE InventoryStocktaking
SET Status = 2, -- Hoàn thành
    EndDate = GETDATE()
WHERE Id = @StocktakingId

-- Phê duyệt
UPDATE InventoryStocktaking
SET Status = 3, -- Đã xác nhận
    ApprovedBy = @UserId,
    ApprovedDate = GETDATE(),
    ApprovalNotes = N'Ghi chú phê duyệt'
WHERE Id = @StocktakingId
```

## View tổng hợp

### vw_InventoryStocktakingSummary

View này cung cấp thông tin tổng hợp của phiếu kiểm kho:

```sql
SELECT * FROM vw_InventoryStocktakingSummary
WHERE PeriodYear = 2025 AND PeriodMonth = 1
```

Các trường tổng hợp:
- `TotalItems`: Tổng số sản phẩm
- `TotalBookQuantity`: Tổng số lượng sổ sách
- `TotalPhysicalQuantity`: Tổng số lượng thực tế
- `TotalDifferenceQuantity`: Tổng chênh lệch số lượng
- `TotalBookValue`: Tổng giá trị sổ sách
- `TotalPhysicalValue`: Tổng giá trị thực tế
- `TotalDifferenceValue`: Tổng chênh lệch giá trị
- `UncountedItems`: Số sản phẩm chưa đếm
- `ItemsWithDifference`: Số sản phẩm có chênh lệch

## Trigger tự động

### TR_InventoryStocktakingDetail_CalculateDifference

Trigger này tự động tính toán:
- `DifferenceQuantity` = `PhysicalQuantity` - `BookQuantity`
- `BookValue` = `BookQuantity` × `UnitPrice`
- `PhysicalValue` = `PhysicalQuantity` × `UnitPrice`
- `DifferenceValue` = `DifferenceQuantity` × `UnitPrice`

**Lưu ý:** Trigger chạy sau mỗi lần INSERT hoặc UPDATE.

## Best Practices

### 1. Quản lý mã SKU

- **Thống nhất:** Chọn một trong hai: `ProductCode` hoặc `ProductVariantCode` làm SKU chính
- **Unique:** Đảm bảo mã SKU là duy nhất trong hệ thống
- **Index:** Đã có index sẵn, không cần tạo thêm

### 2. Quy trình kiểm kho

- **Tạo phiếu:** Sử dụng stored procedure để tự động lấy dữ liệu từ InventoryBalance
- **Đếm thực tế:** Nhập số lượng thực tế cẩn thận, có thể đếm lại nhiều lần
- **Xử lý chênh lệch:** Ghi rõ lý do điều chỉnh
- **Phê duyệt:** Chỉ phê duyệt sau khi đã kiểm tra kỹ

### 3. Performance

- Sử dụng view `vw_InventoryStocktakingSummary` cho báo cáo tổng hợp
- Query theo index (ProductCode, ProductVariantCode, WarehouseId, Period)
- Tránh query trực tiếp bảng detail khi không cần thiết

## Ví dụ sử dụng

### Tìm kiểm kho theo SKU và kho:

```sql
SELECT 
    s.StocktakingCode,
    s.StocktakingName,
    w.BranchName AS WarehouseName,
    d.ProductCode AS SKU,
    d.ProductName,
    d.BookQuantity,
    d.PhysicalQuantity,
    d.DifferenceQuantity
FROM InventoryStocktaking s
INNER JOIN CompanyBranch w ON s.WarehouseId = w.Id
INNER JOIN InventoryStocktakingDetail d ON s.Id = d.StocktakingId
WHERE d.ProductCode = 'SKU001' -- Hoặc ProductVariantCode
    AND s.WarehouseId = @WarehouseId
    AND s.PeriodYear = 2025
    AND s.PeriodMonth = 1
    AND s.IsDeleted = 0
```

### Báo cáo chênh lệch:

```sql
SELECT 
    s.StocktakingCode,
    w.BranchName,
    COUNT(*) AS TotalItems,
    SUM(ABS(d.DifferenceQuantity)) AS TotalDifference,
    SUM(ABS(d.DifferenceValue)) AS TotalDifferenceValue
FROM InventoryStocktaking s
INNER JOIN CompanyBranch w ON s.WarehouseId = w.Id
INNER JOIN InventoryStocktakingDetail d ON s.Id = d.StocktakingId
WHERE s.PeriodYear = 2025
    AND s.PeriodMonth = 1
    AND d.DifferenceQuantity <> 0
    AND s.IsDeleted = 0
GROUP BY s.StocktakingCode, w.BranchName
ORDER BY TotalDifference DESC
```

## Migration

Để áp dụng script:

1. Backup database trước khi chạy
2. Chạy script `Create_InventoryStocktaking_Tables.sql`
3. Kiểm tra các bảng, index, trigger, view đã được tạo
4. Test với dữ liệu mẫu

## Lưu ý quan trọng

1. **Foreign Keys:** Đảm bảo các bảng tham chiếu đã tồn tại:
   - `CompanyBranch` (Warehouse)
   - `ProductVariant`
   - `UnitOfMeasure`
   - `ApplicationUser`

2. **Data Integrity:** 
   - Không xóa InventoryBalance khi đã có phiếu kiểm kho
   - Kiểm tra trước khi tạo phiếu mới cho cùng kỳ

3. **Permissions:**
   - Cần quyền CREATE TABLE, CREATE INDEX, CREATE TRIGGER, CREATE VIEW, CREATE PROCEDURE

## Hỗ trợ

Nếu có vấn đề, vui lòng kiểm tra:
- Log lỗi SQL Server
- Foreign key constraints
- Index performance
- Trigger execution
