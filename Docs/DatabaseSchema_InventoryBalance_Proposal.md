# Database Schema Proposal: InventoryBalance (Tồn kho theo tháng)

## 📋 Tổng quan

Bảng `InventoryBalance` được thiết kế để quản lý số lượng tồn kho theo tháng cho từng sản phẩm/biến thể tại từng kho. Bảng này hỗ trợ:
- Quản lý tồn đầu kỳ, tổng nhập, tổng xuất, tồn cuối kỳ
- Khóa dữ liệu để tránh chỉnh sửa sau khi đã xác nhận
- Bảo mật với xác thực và kiểm tra tính toàn vẹn dữ liệu
- Audit trail đầy đủ

---

## 🗄️ Database Schema

### Table: InventoryBalance

```sql
CREATE TABLE [dbo].[InventoryBalance]
(
    -- Primary Key
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
    
    -- Foreign Keys
    [WarehouseId] UNIQUEIDENTIFIER NOT NULL,  -- FK -> CompanyBranch.Id
    [ProductVariantId] UNIQUEIDENTIFIER NOT NULL,  -- FK -> ProductVariant.Id
    
    -- Period Information (Kỳ báo cáo)
    [PeriodYear] INT NOT NULL,  -- Năm (ví dụ: 2025)
    [PeriodMonth] INT NOT NULL,  -- Tháng (1-12)
    
    -- Balance Information (Thông tin tồn kho)
    [OpeningBalance] DECIMAL(18, 2) NOT NULL DEFAULT 0,  -- Tồn đầu kỳ
    [TotalInQty] DECIMAL(18, 2) NOT NULL DEFAULT 0,  -- Tổng nhập trong kỳ
    [TotalOutQty] DECIMAL(18, 2) NOT NULL DEFAULT 0,  -- Tổng xuất trong kỳ
    [ClosingBalance] DECIMAL(18, 2) NOT NULL DEFAULT 0,  -- Tồn cuối kỳ (tính toán: OpeningBalance + TotalInQty - TotalOutQty)
    
    -- Value Information (Thông tin giá trị - optional)
    [OpeningValue] DECIMAL(18, 2) NULL DEFAULT 0,  -- Giá trị tồn đầu kỳ
    [TotalInValue] DECIMAL(18, 2) NULL DEFAULT 0,  -- Tổng giá trị nhập (chưa VAT)
    [TotalOutValue] DECIMAL(18, 2) NULL DEFAULT 0,  -- Tổng giá trị xuất (chưa VAT)
    [ClosingValue] DECIMAL(18, 2) NULL DEFAULT 0,  -- Giá trị tồn cuối kỳ
    
    -- VAT Information (Thông tin VAT)
    [TotalInVatAmount] DECIMAL(18, 2) NULL DEFAULT 0,  -- Tổng tiền VAT nhập
    [TotalOutVatAmount] DECIMAL(18, 2) NULL DEFAULT 0,  -- Tổng tiền VAT xuất
    [TotalInAmountIncludedVat] DECIMAL(18, 2) NULL DEFAULT 0,  -- Tổng tiền nhập (có VAT) = TotalInValue + TotalInVatAmount
    [TotalOutAmountIncludedVat] DECIMAL(18, 2) NULL DEFAULT 0,  -- Tổng tiền xuất (có VAT) = TotalOutValue + TotalOutVatAmount
    
    -- Lock & Security (Khóa và bảo mật)
    [IsLocked] BIT NOT NULL DEFAULT 0,  -- Đã khóa chưa (không cho phép chỉnh sửa)
    [LockedDate] DATETIME NULL,  -- Ngày khóa
    [LockedBy] UNIQUEIDENTIFIER NULL,  -- FK -> ApplicationUser.Id (Người khóa)
    [LockReason] NVARCHAR(500) NULL,  -- Lý do khóa
    
    [IsVerified] BIT NOT NULL DEFAULT 0,  -- Đã xác thực chưa
    [VerifiedDate] DATETIME NULL,  -- Ngày xác thực
    [VerifiedBy] UNIQUEIDENTIFIER NULL,  -- FK -> ApplicationUser.Id (Người xác thực)
    [VerificationNotes] NVARCHAR(1000) NULL,  -- Ghi chú xác thực
    
    [IsApproved] BIT NOT NULL DEFAULT 0,  -- Đã phê duyệt chưa
    [ApprovedDate] DATETIME NULL,  -- Ngày phê duyệt
    [ApprovedBy] UNIQUEIDENTIFIER NULL,  -- FK -> ApplicationUser.Id (Người phê duyệt)
    [ApprovalNotes] NVARCHAR(1000) NULL,  -- Ghi chú phê duyệt
    
    -- Status & Audit
    [Status] INT NOT NULL DEFAULT 0,  -- 0: Draft, 1: Locked, 2: Verified, 3: Approved, 4: Rejected
    [Notes] NVARCHAR(1000) NULL,  -- Ghi chú chung
    
    [IsActive] BIT NOT NULL DEFAULT 1,
    [IsDeleted] BIT NOT NULL DEFAULT 0,
    [CreateDate] DATETIME NOT NULL DEFAULT GETDATE(),
    [CreateBy] UNIQUEIDENTIFIER NOT NULL,  -- FK -> ApplicationUser.Id
    [ModifiedDate] DATETIME NULL,
    [ModifiedBy] UNIQUEIDENTIFIER NULL,  -- FK -> ApplicationUser.Id
    [DeletedDate] DATETIME NULL,
    [DeletedBy] UNIQUEIDENTIFIER NULL,  -- FK -> ApplicationUser.Id
    
    -- Constraints
    CONSTRAINT [FK_InventoryBalance_CompanyBranch] 
        FOREIGN KEY ([WarehouseId]) REFERENCES [dbo].[CompanyBranch]([Id]),
    CONSTRAINT [FK_InventoryBalance_ProductVariant] 
        FOREIGN KEY ([ProductVariantId]) REFERENCES [dbo].[ProductVariant]([Id]),
    CONSTRAINT [FK_InventoryBalance_LockedBy] 
        FOREIGN KEY ([LockedBy]) REFERENCES [dbo].[ApplicationUser]([Id]),
    CONSTRAINT [FK_InventoryBalance_VerifiedBy] 
        FOREIGN KEY ([VerifiedBy]) REFERENCES [dbo].[ApplicationUser]([Id]),
    CONSTRAINT [FK_InventoryBalance_ApprovedBy] 
        FOREIGN KEY ([ApprovedBy]) REFERENCES [dbo].[ApplicationUser]([Id]),
    CONSTRAINT [FK_InventoryBalance_CreateBy] 
        FOREIGN KEY ([CreateBy]) REFERENCES [dbo].[ApplicationUser]([Id]),
    CONSTRAINT [FK_InventoryBalance_ModifiedBy] 
        FOREIGN KEY ([ModifiedBy]) REFERENCES [dbo].[ApplicationUser]([Id]),
    CONSTRAINT [FK_InventoryBalance_DeletedBy] 
        FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[ApplicationUser]([Id]),
    
    -- Check Constraints
    CONSTRAINT [CHK_InventoryBalance_PeriodMonth] 
        CHECK ([PeriodMonth] >= 1 AND [PeriodMonth] <= 12),
    CONSTRAINT [CHK_InventoryBalance_PeriodYear] 
        CHECK ([PeriodYear] >= 2000 AND [PeriodYear] <= 9999),
    CONSTRAINT [CHK_InventoryBalance_ClosingBalance] 
        CHECK ([ClosingBalance] = [OpeningBalance] + [TotalInQty] - [TotalOutQty]),
    CONSTRAINT [CHK_InventoryBalance_TotalInAmountIncludedVat] 
        CHECK ([TotalInAmountIncludedVat] = [TotalInValue] + [TotalInVatAmount] OR ([TotalInValue] IS NULL AND [TotalInVatAmount] IS NULL)),
    CONSTRAINT [CHK_InventoryBalance_TotalOutAmountIncludedVat] 
        CHECK ([TotalOutAmountIncludedVat] = [TotalOutValue] + [TotalOutVatAmount] OR ([TotalOutValue] IS NULL AND [TotalOutVatAmount] IS NULL)),
    CONSTRAINT [CHK_InventoryBalance_Status] 
        CHECK ([Status] IN (0, 1, 2, 3, 4)),  -- 0: Draft, 1: Locked, 2: Verified, 3: Approved, 4: Rejected
    
    -- Unique Constraint: Một bản ghi duy nhất cho mỗi kho + sản phẩm + kỳ
    CONSTRAINT [UQ_InventoryBalance_Warehouse_Product_Period] 
        UNIQUE ([WarehouseId], [ProductVariantId], [PeriodYear], [PeriodMonth], [IsDeleted])
);

-- Indexes
CREATE INDEX [IX_InventoryBalance_WarehouseId] 
    ON [dbo].[InventoryBalance]([WarehouseId]);
CREATE INDEX [IX_InventoryBalance_ProductVariantId] 
    ON [dbo].[InventoryBalance]([ProductVariantId]);
CREATE INDEX [IX_InventoryBalance_Period] 
    ON [dbo].[InventoryBalance]([PeriodYear], [PeriodMonth]);
CREATE INDEX [IX_InventoryBalance_Status] 
    ON [dbo].[InventoryBalance]([Status], [IsLocked]);
CREATE INDEX [IX_InventoryBalance_IsActive_IsDeleted] 
    ON [dbo].[InventoryBalance]([IsActive], [IsDeleted]);
CREATE INDEX [IX_InventoryBalance_CreateDate] 
    ON [dbo].[InventoryBalance]([CreateDate]);
```

---

## 📊 Mô tả các trường

### 1. Thông tin cơ bản
- **Id**: Khóa chính (GUID)
- **WarehouseId**: ID kho (tham chiếu CompanyBranch)
- **ProductVariantId**: ID biến thể sản phẩm
- **PeriodYear**: Năm của kỳ báo cáo
- **PeriodMonth**: Tháng của kỳ báo cáo (1-12)

### 2. Thông tin tồn kho
- **OpeningBalance**: Tồn đầu kỳ (số lượng)
- **TotalInQty**: Tổng số lượng nhập trong kỳ
- **TotalOutQty**: Tổng số lượng xuất trong kỳ
- **ClosingBalance**: Tồn cuối kỳ (tự động tính: OpeningBalance + TotalInQty - TotalOutQty)

### 3. Thông tin giá trị (tùy chọn)
- **OpeningValue**: Giá trị tồn đầu kỳ
- **TotalInValue**: Tổng giá trị nhập (chưa VAT)
- **TotalOutValue**: Tổng giá trị xuất (chưa VAT)
- **ClosingValue**: Giá trị tồn cuối kỳ

### 4. Thông tin VAT
- **TotalInVatAmount**: Tổng tiền VAT nhập
- **TotalOutVatAmount**: Tổng tiền VAT xuất
- **TotalInAmountIncludedVat**: Tổng tiền nhập (có VAT) = TotalInValue + TotalInVatAmount
- **TotalOutAmountIncludedVat**: Tổng tiền xuất (có VAT) = TotalOutValue + TotalOutVatAmount

### 5. Khóa dữ liệu (Lock)
- **IsLocked**: Đã khóa chưa (khi khóa, không cho phép chỉnh sửa)
- **LockedDate**: Ngày khóa
- **LockedBy**: Người khóa
- **LockReason**: Lý do khóa

### 6. Xác thực (Verification)
- **IsVerified**: Đã xác thực chưa
- **VerifiedDate**: Ngày xác thực
- **VerifiedBy**: Người xác thực
- **VerificationNotes**: Ghi chú xác thực

### 7. Phê duyệt (Approval)
- **IsApproved**: Đã phê duyệt chưa
- **ApprovedDate**: Ngày phê duyệt
- **ApprovedBy**: Người phê duyệt
- **ApprovalNotes**: Ghi chú phê duyệt

### 8. Trạng thái và Audit
- **Status**: Trạng thái (0: Draft, 1: Locked, 2: Verified, 3: Approved, 4: Rejected)
- **Notes**: Ghi chú chung
- **IsActive**: Đang hoạt động
- **IsDeleted**: Đã xóa (soft delete)
- **CreateDate, CreateBy**: Ngày tạo và người tạo
- **ModifiedDate, ModifiedBy**: Ngày sửa và người sửa
- **DeletedDate, DeletedBy**: Ngày xóa và người xóa

---

## 🔒 Quy trình khóa và bảo mật

### Workflow trạng thái:
```
Draft (0) 
  → Locked (1) [Khóa dữ liệu]
    → Verified (2) [Xác thực]
      → Approved (3) [Phê duyệt]
        → [Hoàn tất]
```

### Quy tắc khóa dữ liệu:
1. **Khi IsLocked = true**:
   - Không cho phép chỉnh sửa OpeningBalance, TotalInQty, TotalOutQty, ClosingBalance
   - Chỉ cho phép chỉnh sửa Notes, VerificationNotes, ApprovalNotes
   - Chỉ người có quyền mới có thể unlock

2. **Khi IsVerified = true**:
   - Dữ liệu đã được xác thực bởi người có thẩm quyền
   - Không thể unlock trừ khi có quyền đặc biệt

3. **Khi IsApproved = true**:
   - Dữ liệu đã được phê duyệt cuối cùng
   - Không thể chỉnh sửa hoặc unlock

### Quyền truy cập:
- **View**: Tất cả user có quyền xem
- **Create/Edit**: User có quyền tạo/sửa (chỉ khi IsLocked = false)
- **Lock**: User có quyền khóa (Manager, Supervisor)
- **Verify**: User có quyền xác thực (Supervisor, Auditor)
- **Approve**: User có quyền phê duyệt (Director, Manager)
- **Unlock**: User có quyền unlock (Manager, Admin) - chỉ khi chưa Approved

---

## 🔄 Logic tính toán

### 1. Tính tồn cuối kỳ:
```sql
ClosingBalance = OpeningBalance + TotalInQty - TotalOutQty
```

### 2. Tính tồn đầu kỳ của tháng sau:
```sql
OpeningBalance (tháng N+1) = ClosingBalance (tháng N)
```

### 3. Tính tổng nhập/xuất từ StockInOutDetail:
```sql
-- Tổng nhập (số lượng)
SELECT SUM(StockInQty) 
FROM StockInOutDetail d
INNER JOIN StockInOutMaster m ON d.StockInOutMasterId = m.Id
WHERE m.WarehouseId = @WarehouseId
  AND d.ProductVariantId = @ProductVariantId
  AND YEAR(m.StockInOutDate) = @PeriodYear
  AND MONTH(m.StockInOutDate) = @PeriodMonth
  AND m.VoucherStatus = 1  -- Đã duyệt

-- Tổng xuất (số lượng)
SELECT SUM(StockOutQty) 
FROM StockInOutDetail d
INNER JOIN StockInOutMaster m ON d.StockInOutMasterId = m.Id
WHERE m.WarehouseId = @WarehouseId
  AND d.ProductVariantId = @ProductVariantId
  AND YEAR(m.StockInOutDate) = @PeriodYear
  AND MONTH(m.StockInOutDate) = @PeriodMonth
  AND m.VoucherStatus = 1  -- Đã duyệt

-- Tổng giá trị nhập (chưa VAT)
SELECT SUM(TotalAmount) 
FROM StockInOutDetail d
INNER JOIN StockInOutMaster m ON d.StockInOutMasterId = m.Id
WHERE m.WarehouseId = @WarehouseId
  AND d.ProductVariantId = @ProductVariantId
  AND YEAR(m.StockInOutDate) = @PeriodYear
  AND MONTH(m.StockInOutDate) = @PeriodMonth
  AND m.VoucherStatus = 1
  AND d.StockInQty > 0  -- Chỉ tính nhập

-- Tổng giá trị xuất (chưa VAT)
SELECT SUM(TotalAmount) 
FROM StockInOutDetail d
INNER JOIN StockInOutMaster m ON d.StockInOutMasterId = m.Id
WHERE m.WarehouseId = @WarehouseId
  AND d.ProductVariantId = @ProductVariantId
  AND YEAR(m.StockInOutDate) = @PeriodYear
  AND MONTH(m.StockInOutDate) = @PeriodMonth
  AND m.VoucherStatus = 1
  AND d.StockOutQty > 0  -- Chỉ tính xuất

-- Tổng VAT nhập
SELECT SUM(VatAmount) 
FROM StockInOutDetail d
INNER JOIN StockInOutMaster m ON d.StockInOutMasterId = m.Id
WHERE m.WarehouseId = @WarehouseId
  AND d.ProductVariantId = @ProductVariantId
  AND YEAR(m.StockInOutDate) = @PeriodYear
  AND MONTH(m.StockInOutDate) = @PeriodMonth
  AND m.VoucherStatus = 1
  AND d.StockInQty > 0  -- Chỉ tính nhập

-- Tổng VAT xuất
SELECT SUM(VatAmount) 
FROM StockInOutDetail d
INNER JOIN StockInOutMaster m ON d.StockInOutMasterId = m.Id
WHERE m.WarehouseId = @WarehouseId
  AND d.ProductVariantId = @ProductVariantId
  AND YEAR(m.StockInOutDate) = @PeriodYear
  AND MONTH(m.StockInOutDate) = @PeriodMonth
  AND m.VoucherStatus = 1
  AND d.StockOutQty > 0  -- Chỉ tính xuất
```

---

## 📝 Stored Procedures đề xuất

### 1. sp_CalculateInventoryBalance
Tính toán và cập nhật tồn kho cho một kỳ cụ thể

### 2. sp_LockInventoryBalance
Khóa tồn kho cho một kỳ (chỉ khi chưa khóa)

### 3. sp_VerifyInventoryBalance
Xác thực tồn kho (chỉ khi đã khóa)

### 4. sp_ApproveInventoryBalance
Phê duyệt tồn kho (chỉ khi đã xác thực)

### 5. sp_GetInventoryBalanceByPeriod
Lấy tồn kho theo khoảng thời gian

### 6. sp_GetInventoryBalanceSummary
Lấy tổng hợp tồn kho theo kho/sản phẩm

---

## 🎯 Use Cases

### 1. Tính toán tồn kho hàng tháng
- Tự động tính từ StockInOutDetail
- Cập nhật vào InventoryBalance
- Tính tồn đầu kỳ từ tháng trước

### 2. Khóa tồn kho
- Sau khi tính toán xong, Manager khóa dữ liệu
- Không cho phép chỉnh sửa sau khi khóa

### 3. Xác thực tồn kho
- Supervisor/Auditor xác thực tính chính xác
- Có thể thêm ghi chú nếu phát hiện sai sót

### 4. Phê duyệt tồn kho
- Director/Manager phê duyệt cuối cùng
- Sau khi phê duyệt, không thể thay đổi

### 5. Báo cáo tồn kho
- Xem tồn kho theo khoảng thời gian
- So sánh tồn kho giữa các kỳ
- Xuất báo cáo Excel/PDF

---

## ⚠️ Lưu ý

1. **Unique Constraint**: Một bản ghi duy nhất cho mỗi (WarehouseId, ProductVariantId, PeriodYear, PeriodMonth)
2. **Check Constraint**: Đảm bảo ClosingBalance = OpeningBalance + TotalInQty - TotalOutQty
3. **Period Validation**: PeriodMonth phải từ 1-12, PeriodYear hợp lệ
4. **Status Workflow**: Chỉ cho phép chuyển trạng thái theo đúng workflow
5. **Soft Delete**: Sử dụng IsDeleted thay vì xóa vật lý để giữ lịch sử

---

## 🔐 Security Best Practices

1. **Row-Level Security**: Có thể thêm RLS để giới hạn quyền xem theo kho
2. **Audit Logging**: Ghi log tất cả thay đổi quan trọng (lock, verify, approve)
3. **Encryption**: Có thể mã hóa các trường nhạy cảm nếu cần
4. **Backup**: Backup định kỳ để phục hồi khi cần
5. **Version Control**: Có thể thêm bảng InventoryBalanceHistory để lưu lịch sử thay đổi

---

## 📈 Performance Optimization

1. **Indexes**: Đã tạo indexes cho các trường thường query
2. **Partitioning**: Có thể partition theo PeriodYear nếu dữ liệu lớn
3. **Materialized Views**: Có thể tạo view tổng hợp cho báo cáo nhanh
4. **Caching**: Cache dữ liệu tồn kho thường xuyên truy cập

---

## 🚀 Migration Strategy

1. **Phase 1**: Tạo bảng và constraints
2. **Phase 2**: Tính toán và populate dữ liệu lịch sử (nếu có)
3. **Phase 3**: Tích hợp vào ứng dụng (BLL, DAL, UI)
4. **Phase 4**: Tạo stored procedures và functions
5. **Phase 5**: Testing và validation

