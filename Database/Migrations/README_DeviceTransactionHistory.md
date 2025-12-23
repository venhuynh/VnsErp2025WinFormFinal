# Migration: Tạo bảng DeviceTransactionHistory

## Mô tả
Script migration này tạo bảng `DeviceTransactionHistory` để ghi lại lịch sử các thao tác với thiết bị như nhập, xuất, cấp phát, thu hồi, chuyển giao, bảo trì, v.v.

## Ngày tạo
2025

## Mục đích
Bảng này được thiết kế để:
- Ghi lại toàn bộ lịch sử các thao tác với thiết bị
- Lưu trữ thông tin chi tiết dưới 2 dạng: text và HTML
- Hỗ trợ truy vết và audit trail đầy đủ
- Liên kết với các bảng khác thông qua ReferenceId và ReferenceType

## Cấu trúc bảng

### Primary Key & Foreign Keys
- `Id` [uniqueidentifier] NOT NULL - Primary Key
- `DeviceId` [uniqueidentifier] NOT NULL - Liên kết với bảng Device (ON DELETE CASCADE)

### Thông tin thao tác
- `OperationType` [int] NOT NULL - Loại thao tác:
  - 0 = Import/Nhập kho
  - 1 = Export/Xuất kho
  - 2 = Allocation/Cấp phát
  - 3 = Recovery/Thu hồi
  - 4 = Transfer/Chuyển giao
  - 5 = Maintenance/Bảo trì
  - 6 = StatusChange/Đổi trạng thái
  - 7 = Other/Khác
- `OperationDate` [datetime] NOT NULL - Ngày giờ thực hiện thao tác

### Thông tin tham chiếu
- `ReferenceId` [uniqueidentifier] NULL - ID của bảng tham chiếu (ví dụ: StockInOutDetailId, DeviceTransferId)
- `ReferenceType` [int] NULL - Loại tham chiếu:
  - 0 = StockInOutDetail (Phiếu nhập/xuất chi tiết)
  - 1 = DeviceTransfer (Chuyển giao thiết bị)
  - 2 = Warranty (Bảo hành)
  - 3 = Other (Khác)

### Thông tin chi tiết (2 cột theo yêu cầu)
- `Information` [nvarchar](max) NULL - **Thông tin dạng text** (mô tả chi tiết thao tác)
  - Ví dụ: "Thiết bị được nhập kho từ phiếu nhập số PN-2025-001, số lượng: 1, giá: 15,000,000 VNĐ"
- `HtmlInformation` [nvarchar](max) NULL - **Thông tin dạng HTML** (để hiển thị đẹp trên UI)
  - Ví dụ: "<div><strong>Thiết bị được nhập kho</strong><br/>Phiếu nhập: <a href='#'>PN-2025-001</a><br/>Số lượng: <span class='highlight'>1</span><br/>Giá: <span class='price'>15,000,000 VNĐ</span></div>"

### Thông tin người thực hiện
- `PerformedBy` [uniqueidentifier] NULL - Người thực hiện thao tác (có thể là EmployeeId hoặc ApplicationUserId)

### Ghi chú
- `Notes` [nvarchar](1000) NULL - Ghi chú bổ sung

### Audit Fields
- `CreatedDate` [datetime] NOT NULL DEFAULT(GETDATE()) - Ngày tạo bản ghi
- `CreatedBy` [uniqueidentifier] NULL - Người tạo bản ghi

## Indexes

1. **IX_DeviceTransactionHistory_DeviceId_OperationDate**
   - Cột: `DeviceId`, `OperationDate` (DESC)
   - Mục đích: Query lịch sử theo thiết bị, sắp xếp theo thời gian mới nhất

2. **IX_DeviceTransactionHistory_OperationType**
   - Cột: `OperationType`
   - Mục đích: Filter theo loại thao tác

3. **IX_DeviceTransactionHistory_Reference**
   - Cột: `ReferenceId`, `ReferenceType`
   - Mục đích: Query theo tham chiếu (ví dụ: tìm tất cả thao tác liên quan đến một phiếu nhập)

4. **IX_DeviceTransactionHistory_PerformedBy**
   - Cột: `PerformedBy`
   - Mục đích: Query theo người thực hiện

## Các trường hợp sử dụng

### 1. Nhập thiết bị (Import)
```sql
INSERT INTO DeviceTransactionHistory (
    Id, DeviceId, OperationType, OperationDate, 
    ReferenceId, ReferenceType, 
    Information, HtmlInformation, 
    PerformedBy, CreatedDate, CreatedBy
)
VALUES (
    NEWID(), @DeviceId, 0, GETDATE(),
    @StockInOutDetailId, 0,
    N'Thiết bị được nhập kho từ phiếu nhập số ' + @DocumentNumber,
    N'<div><strong>Nhập kho</strong><br/>Phiếu: ' + @DocumentNumber + '</div>',
    @UserId, GETDATE(), @UserId
)
```

### 2. Xuất thiết bị (Export)
```sql
INSERT INTO DeviceTransactionHistory (
    Id, DeviceId, OperationType, OperationDate,
    ReferenceId, ReferenceType,
    Information, HtmlInformation,
    PerformedBy, CreatedDate, CreatedBy
)
VALUES (
    NEWID(), @DeviceId, 1, GETDATE(),
    @StockInOutDetailId, 0,
    N'Thiết bị được xuất kho theo phiếu xuất số ' + @DocumentNumber,
    N'<div><strong>Xuất kho</strong><br/>Phiếu: ' + @DocumentNumber + '</div>',
    @UserId, GETDATE(), @UserId
)
```

### 3. Cấp phát thiết bị (Allocation)
```sql
INSERT INTO DeviceTransactionHistory (
    Id, DeviceId, OperationType, OperationDate,
    Information, HtmlInformation,
    PerformedBy, CreatedDate, CreatedBy
)
VALUES (
    NEWID(), @DeviceId, 2, GETDATE(),
    N'Thiết bị được cấp phát cho nhân viên: ' + @EmployeeName + ', Phòng ban: ' + @DepartmentName,
    N'<div><strong>Cấp phát</strong><br/>Nhân viên: <strong>' + @EmployeeName + '</strong><br/>Phòng ban: ' + @DepartmentName + '</div>',
    @UserId, GETDATE(), @UserId
)
```

### 4. Thu hồi thiết bị (Recovery)
```sql
INSERT INTO DeviceTransactionHistory (
    Id, DeviceId, OperationType, OperationDate,
    Information, HtmlInformation,
    PerformedBy, Notes, CreatedDate, CreatedBy
)
VALUES (
    NEWID(), @DeviceId, 3, GETDATE(),
    N'Thiết bị được thu hồi từ nhân viên: ' + @EmployeeName,
    N'<div><strong>Thu hồi</strong><br/>Từ nhân viên: ' + @EmployeeName + '</div>',
    @UserId, @RecoveryReason, GETDATE(), @UserId
)
```

## Lưu ý quan trọng

1. **Cascade Delete**: Khi xóa thiết bị, tất cả lịch sử giao dịch sẽ tự động bị xóa (ON DELETE CASCADE)

2. **Information vs HtmlInformation**:
   - `Information`: Dùng cho logging, search, export dữ liệu
   - `HtmlInformation`: Dùng để hiển thị đẹp trên UI, có thể chứa HTML tags, CSS classes, links

3. **ReferenceId và ReferenceType**: 
   - Cho phép liên kết với các bảng khác mà không cần tạo nhiều Foreign Key
   - Linh hoạt hơn khi có nhiều loại tham chiếu khác nhau

4. **Performance**: 
   - Indexes được tối ưu cho các query phổ biến
   - Sử dụng filtered indexes cho các cột nullable để tiết kiệm không gian

## Cách thực hiện

1. Backup database trước khi chạy migration
2. Chạy script `Create_DeviceTransactionHistory_Table.sql`
3. Kiểm tra kết quả và đảm bảo không có lỗi
4. Cập nhật LINQ to SQL model (VnsErp2025.dbml) nếu cần
5. Rebuild project để cập nhật DataContext

## Kiểm tra sau migration

Sau khi chạy migration, hãy kiểm tra:
- Bảng DeviceTransactionHistory đã được tạo thành công
- Tất cả Foreign Key constraints đã được tạo đúng
- Tất cả Indexes đã được tạo đúng
- Có thể insert và query dữ liệu thành công

## Ví dụ query

### Lấy lịch sử của một thiết bị
```sql
SELECT 
    dth.OperationDate,
    dth.OperationType,
    dth.Information,
    dth.HtmlInformation,
    dth.PerformedBy
FROM DeviceTransactionHistory dth
WHERE dth.DeviceId = @DeviceId
ORDER BY dth.OperationDate DESC
```

### Lấy tất cả thao tác nhập/xuất trong khoảng thời gian
```sql
SELECT 
    d.SerialNumber,
    dth.OperationDate,
    dth.OperationType,
    dth.Information
FROM DeviceTransactionHistory dth
INNER JOIN Device d ON d.Id = dth.DeviceId
WHERE dth.OperationType IN (0, 1) -- Import hoặc Export
    AND dth.OperationDate BETWEEN @StartDate AND @EndDate
ORDER BY dth.OperationDate DESC
```

### Lấy lịch sử theo người thực hiện
```sql
SELECT 
    d.SerialNumber,
    dth.OperationDate,
    dth.OperationType,
    dth.Information
FROM DeviceTransactionHistory dth
INNER JOIN Device d ON d.Id = dth.DeviceId
WHERE dth.PerformedBy = @UserId
ORDER BY dth.OperationDate DESC
```
