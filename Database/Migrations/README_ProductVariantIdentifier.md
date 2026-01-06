# Bảng Quản Lý Định Danh ProductVariant

## Tổng quan

Hệ thống quản lý định danh cho ProductVariant được thiết kế để:
- Quản lý các loại định danh cho ProductVariant (tương tự như bảng Device nhưng quản lý rộng hơn)
- Hỗ trợ nhiều loại định danh: SerialNumber, Barcode, QRCode, SKU, RFID, MAC, IMEI, AssetTag, LicenseKey, UPC, EAN, ISBN, v.v.
- Không chỉ giới hạn cho thiết bị mà có thể quản lý định danh cho mọi loại sản phẩm
- Lưu trữ lịch sử thay đổi định danh

## So sánh với bảng Device

| Đặc điểm | Device | ProductVariantIdentifier |
|----------|--------|-------------------------|
| **Phạm vi** | Chỉ quản lý thiết bị | Quản lý rộng hơn, cho mọi loại sản phẩm |
| **Cấu trúc** | Các cột cố định (SerialNumber, MACAddress, IMEI, v.v.) | Bảng linh hoạt, mỗi định danh là một dòng |
| **Nhiều định danh** | Giới hạn bởi số cột | Không giới hạn, có thể thêm nhiều định danh |
| **Loại định danh** | Cố định | Linh hoạt, dễ mở rộng |

## Cấu trúc bảng

### 1. ProductVariantIdentifier (Định danh ProductVariant)

Bảng chính quản lý các định danh cho ProductVariant.

#### Các trường chính:

| Trường | Kiểu | Mô tả |
|--------|------|-------|
| `Id` | uniqueidentifier | ID duy nhất |
| `ProductVariantId` | uniqueidentifier | ID biến thể sản phẩm (FK -> ProductVariant) |
| `IdentifierType` | int | Loại định danh (xem bảng dưới) |
| `IdentifierValue` | nvarchar(255) | Giá trị định danh (bắt buộc) |
| `IdentifierLabel` | nvarchar(100) | Nhãn mô tả (tùy chọn) |
| `IsPrimary` | bit | Định danh chính (mỗi ProductVariant chỉ nên có 1 primary) |
| `IsUnique` | bit | Giá trị có phải duy nhất không (mặc định = 1) |
| `IsActive` | bit | Còn sử dụng không |
| `SourceType` | int | Nguồn: 0=Manual, 1=Import, 2=AutoGenerate, 3=Scanner, 4=Other |
| `SourceReference` | nvarchar(255) | Tham chiếu nguồn |
| `ValidFrom` | datetime | Ngày bắt đầu có hiệu lực |
| `ValidTo` | datetime | Ngày hết hiệu lực |
| `Notes` | nvarchar(1000) | Ghi chú |
| `CreatedDate` | datetime | Ngày tạo |
| `UpdatedDate` | datetime | Ngày cập nhật |
| `CreatedBy` | uniqueidentifier | Người tạo |
| `UpdatedBy` | uniqueidentifier | Người cập nhật |

#### Các loại định danh (IdentifierType):

| Giá trị | Tên | Mô tả |
|---------|-----|-------|
| 0 | SerialNumber | Số serial |
| 1 | Barcode | Mã vạch |
| 2 | QRCode | Mã QR |
| 3 | SKU | Stock Keeping Unit (mã sản phẩm) |
| 4 | RFID | Radio Frequency Identification |
| 5 | MAC | Media Access Control Address |
| 6 | IMEI | International Mobile Equipment Identity |
| 7 | AssetTag | Mã tài sản nội bộ |
| 8 | LicenseKey | Khóa bản quyền (cho phần mềm) |
| 9 | UPC | Universal Product Code |
| 10 | EAN | European Article Number |
| 11 | ISBN | International Standard Book Number |
| 12 | Other | Loại khác |

#### Ràng buộc và Index:

1. **Primary Key**: `Id`
2. **Foreign Key**: 
   - `ProductVariantId` -> `ProductVariant.Id` (ON DELETE CASCADE)
3. **Unique Index**: 
   - `IX_ProductVariantIdentifier_IdentifierValue_Unique`: Đảm bảo (IdentifierType, IdentifierValue) là duy nhất khi `IsUnique = 1` và `IsActive = 1`
4. **Non-clustered Indexes**:
   - `IX_ProductVariantIdentifier_ProductVariantId`: Tìm kiếm theo ProductVariant
   - `IX_ProductVariantIdentifier_IdentifierType`: Tìm kiếm theo loại định danh
   - `IX_ProductVariantIdentifier_IdentifierValue`: Tìm kiếm theo giá trị định danh
   - `IX_ProductVariantIdentifier_IsPrimary`: Tìm định danh chính nhanh
   - `IX_ProductVariantIdentifier_ValidDate`: Query theo thời gian hiệu lực

### 2. ProductVariantIdentifierHistory (Lịch sử thay đổi)

Bảng lưu trữ lịch sử thay đổi của các định danh.

#### Các trường chính:

| Trường | Kiểu | Mô tả |
|--------|------|-------|
| `Id` | uniqueidentifier | ID duy nhất |
| `ProductVariantIdentifierId` | uniqueidentifier | ID định danh (FK) |
| `ProductVariantId` | uniqueidentifier | ID ProductVariant (để query nhanh) |
| `ChangeType` | int | Loại thay đổi (xem bảng dưới) |
| `ChangeDate` | datetime | Ngày thay đổi |
| `ChangedBy` | uniqueidentifier | Người thay đổi |
| `OldValue` | nvarchar(500) | Giá trị cũ |
| `NewValue` | nvarchar(500) | Giá trị mới |
| `FieldName` | nvarchar(100) | Tên trường thay đổi |
| `Description` | nvarchar(1000) | Mô tả thay đổi |
| `Notes` | nvarchar(1000) | Ghi chú |

#### Các loại thay đổi (ChangeType):

| Giá trị | Tên | Mô tả |
|---------|-----|-------|
| 0 | Created | Tạo mới |
| 1 | Updated | Cập nhật |
| 2 | Activated | Kích hoạt |
| 3 | Deactivated | Vô hiệu hóa |
| 4 | Deleted | Xóa |
| 5 | PrimaryChanged | Thay đổi định danh chính |

## Sử dụng

### Ví dụ: Thêm định danh cho ProductVariant

```sql
-- Thêm SerialNumber
INSERT INTO ProductVariantIdentifier 
    (Id, ProductVariantId, IdentifierType, IdentifierValue, IsPrimary, IsUnique, IsActive, CreatedDate)
VALUES 
    (NEWID(), @ProductVariantId, 0, 'SN123456789', 1, 1, 1, GETDATE())

-- Thêm Barcode
INSERT INTO ProductVariantIdentifier 
    (Id, ProductVariantId, IdentifierType, IdentifierValue, IsPrimary, IsUnique, IsActive, CreatedDate)
VALUES 
    (NEWID(), @ProductVariantId, 1, '1234567890123', 0, 1, 1, GETDATE())

-- Thêm SKU
INSERT INTO ProductVariantIdentifier 
    (Id, ProductVariantId, IdentifierType, IdentifierValue, IsPrimary, IsUnique, IsActive, CreatedDate)
VALUES 
    (NEWID(), @ProductVariantId, 3, 'SKU-ABC-001', 0, 1, 1, GETDATE())
```

### Ví dụ: Tìm ProductVariant theo định danh

```sql
-- Tìm theo SerialNumber
SELECT pv.* 
FROM ProductVariant pv
INNER JOIN ProductVariantIdentifier pvi ON pv.Id = pvi.ProductVariantId
WHERE pvi.IdentifierType = 0 -- SerialNumber
  AND pvi.IdentifierValue = 'SN123456789'
  AND pvi.IsActive = 1

-- Tìm theo Barcode
SELECT pv.* 
FROM ProductVariant pv
INNER JOIN ProductVariantIdentifier pvi ON pv.Id = pvi.ProductVariantId
WHERE pvi.IdentifierType = 1 -- Barcode
  AND pvi.IdentifierValue = '1234567890123'
  AND pvi.IsActive = 1

-- Tìm theo bất kỳ định danh nào
SELECT pv.* 
FROM ProductVariant pv
INNER JOIN ProductVariantIdentifier pvi ON pv.Id = pvi.ProductVariantId
WHERE pvi.IdentifierValue = @SearchValue
  AND pvi.IsActive = 1
```

### Ví dụ: Lấy tất cả định danh của một ProductVariant

```sql
SELECT 
    pvi.IdentifierType,
    CASE pvi.IdentifierType
        WHEN 0 THEN 'SerialNumber'
        WHEN 1 THEN 'Barcode'
        WHEN 2 THEN 'QRCode'
        WHEN 3 THEN 'SKU'
        WHEN 4 THEN 'RFID'
        WHEN 5 THEN 'MAC'
        WHEN 6 THEN 'IMEI'
        WHEN 7 THEN 'AssetTag'
        WHEN 8 THEN 'LicenseKey'
        WHEN 9 THEN 'UPC'
        WHEN 10 THEN 'EAN'
        WHEN 11 THEN 'ISBN'
        ELSE 'Other'
    END AS IdentifierTypeName,
    pvi.IdentifierValue,
    pvi.IdentifierLabel,
    pvi.IsPrimary,
    pvi.IsUnique,
    pvi.ValidFrom,
    pvi.ValidTo
FROM ProductVariantIdentifier pvi
WHERE pvi.ProductVariantId = @ProductVariantId
  AND pvi.IsActive = 1
ORDER BY pvi.IsPrimary DESC, pvi.IdentifierType
```

### Ví dụ: Lấy định danh chính của ProductVariant

```sql
SELECT 
    pvi.IdentifierType,
    pvi.IdentifierValue,
    pvi.IdentifierLabel
FROM ProductVariantIdentifier pvi
WHERE pvi.ProductVariantId = @ProductVariantId
  AND pvi.IsPrimary = 1
  AND pvi.IsActive = 1
```

### Ví dụ: Kiểm tra tính duy nhất của định danh

```sql
-- Kiểm tra xem một giá trị định danh đã tồn tại chưa
SELECT COUNT(*) 
FROM ProductVariantIdentifier
WHERE IdentifierType = @IdentifierType
  AND IdentifierValue = @IdentifierValue
  AND IsUnique = 1
  AND IsActive = 1
  AND (@ProductVariantId IS NULL OR ProductVariantId != @ProductVariantId)
```

## Quy tắc và Best Practices

1. **Định danh chính (IsPrimary)**:
   - Mỗi ProductVariant chỉ nên có 1 định danh chính
   - Khi đặt một định danh là primary, nên bỏ primary của các định danh khác

2. **Tính duy nhất (IsUnique)**:
   - Các định danh như SerialNumber, IMEI, MAC thường là duy nhất → `IsUnique = 1`
   - Các định danh như Barcode, SKU có thể không duy nhất → `IsUnique = 0`

3. **Thời gian hiệu lực (ValidFrom/ValidTo)**:
   - Sử dụng khi định danh có thời hạn (ví dụ: LicenseKey có thời hạn)
   - Để NULL nếu định danh không có thời hạn

4. **Lịch sử thay đổi**:
   - Nên tự động ghi lại mọi thay đổi vào bảng History
   - Sử dụng trigger hoặc application logic để tự động insert vào History

## Migration

Để chạy migration, thực hiện:

```sql
-- Chạy script tạo bảng
EXEC sp_executesql N'$(SQLCMDPATH)\Database\Migrations\Create_ProductVariantIdentifier_Table.sql'
```

Hoặc chạy trực tiếp file SQL trong SQL Server Management Studio.

## Lưu ý

- Bảng này được thiết kế để linh hoạt hơn bảng Device
- Có thể mở rộng thêm các loại định danh mới bằng cách thêm giá trị vào enum IdentifierType
- Unique constraint chỉ áp dụng cho các định danh có `IsUnique = 1` và `IsActive = 1`
- Khi xóa ProductVariant, tất cả định danh sẽ tự động bị xóa (CASCADE)
