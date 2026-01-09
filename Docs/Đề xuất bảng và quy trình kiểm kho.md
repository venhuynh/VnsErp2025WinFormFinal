# Đề xuất Bảng và Quy trình Kiểm kho (Stocktaking)

## 1. Phân tích cấu trúc hiện tại

### 1.1. Các bảng liên quan đến nhập xuất và tồn kho

#### StockInOutMaster (Bảng chính nhập xuất)
- **Id**: UniqueIdentifier (PK)
- **StockInOutDate**: DateTime - Ngày nhập/xuất
- **VocherNumber**: NVarChar(50) - Số phiếu
- **StockInOutType**: Int - Loại nhập/xuất
- **VoucherStatus**: Int - Trạng thái phiếu
- **WarehouseId**: UniqueIdentifier - Kho
- **PurchaseOrderId**: UniqueIdentifier (nullable) - Đơn hàng mua
- **PartnerSiteId**: UniqueIdentifier (nullable) - Địa điểm đối tác
- **Notes**: NVarChar(500) - Ghi chú
- **TotalQuantity**: Decimal(18,2) - Tổng số lượng
- **TotalAmount**: Decimal(18,2) - Tổng tiền
- **TotalVat**: Decimal(18,2) - Tổng VAT
- **TotalAmountIncludedVat**: Decimal(18,2) - Tổng tiền bao gồm VAT
- **NguoiNhanHang**: NVarChar(500) - Người nhận hàng
- **NguoiGiaoHang**: NVarChar(500) - Người giao hàng
- **DiscountAmount**: Decimal(18,2) (nullable) - Giảm giá
- **TotalAmountAfterDiscount**: Decimal(18,2) (nullable) - Tổng tiền sau giảm giá
- **CreatedBy**: UniqueIdentifier (nullable)
- **CreatedDate**: DateTime (nullable)
- **UpdatedBy**: UniqueIdentifier (nullable)
- **UpdatedDate**: DateTime (nullable)

#### StockInOutDetail (Chi tiết nhập xuất)
- **Id**: UniqueIdentifier (PK)
- **StockInOutMasterId**: UniqueIdentifier (FK) - Liên kết với StockInOutMaster
- **ProductVariantId**: UniqueIdentifier (FK) - Sản phẩm
- **StockInQty**: Decimal(18,2) - Số lượng nhập
- **StockOutQty**: Decimal(18,2) - Số lượng xuất
- **UnitPrice**: Decimal(18,2) - Đơn giá
- **Vat**: Decimal(18,2) - VAT
- **VatAmount**: Decimal(18,2) - Tiền VAT
- **TotalAmount**: Decimal(18,2) - Tổng tiền
- **TotalAmountIncludedVat**: Decimal(18,2) - Tổng tiền bao gồm VAT
- **GhiChu**: NVarChar(1000) - Ghi chú
- **DiscountAmount**: Decimal(18,2) (nullable)
- **TotalAmountAfterDiscount**: Decimal(18,2) (nullable)
- **DiscountPercentage**: Decimal(18,2) (nullable)

#### InventoryBalance (Tồn kho theo kỳ)
- **Id**: UniqueIdentifier (PK)
- **WarehouseId**: UniqueIdentifier - Kho
- **ProductVariantId**: UniqueIdentifier - Sản phẩm
- **PeriodYear**: Int - Năm
- **PeriodMonth**: Int - Tháng
- **OpeningBalance**: Decimal(18,2) - Tồn đầu kỳ
- **TotalInQty**: Decimal(18,2) - Tổng nhập
- **TotalOutQty**: Decimal(18,2) - Tổng xuất
- **ClosingBalance**: Decimal(18,2) - Tồn cuối kỳ
- **OpeningValue**: Decimal(18,2) (nullable) - Giá trị đầu kỳ
- **TotalInValue**: Decimal(18,2) (nullable) - Giá trị nhập
- **TotalOutValue**: Decimal(18,2) (nullable) - Giá trị xuất
- **ClosingValue**: Decimal(18,2) (nullable) - Giá trị cuối kỳ
- **IsLocked**: Bit - Đã khóa
- **LockedDate**: DateTime (nullable)
- **LockedBy**: UniqueIdentifier (nullable)
- **IsVerified**: Bit - Đã xác minh
- **VerifiedDate**: DateTime (nullable)
- **VerifiedBy**: UniqueIdentifier (nullable)
- **IsApproved**: Bit - Đã duyệt
- **ApprovedDate**: DateTime (nullable)
- **ApprovedBy**: UniqueIdentifier (nullable)
- **Status**: Int - Trạng thái
- **Notes**: NVarChar - Ghi chú
- **IsActive**: Bit
- **IsDeleted**: Bit
- **CreateDate**: DateTime
- **CreateBy**: UniqueIdentifier
- **ModifiedDate**: DateTime (nullable)
- **ModifiedBy**: UniqueIdentifier (nullable)

---

## 2. Đề xuất cấu trúc bảng Kiểm kho

### 2.1. StocktakingMaster (Bảng chính kiểm kho)

```sql
CREATE TABLE [dbo].[StocktakingMaster] (
    [Id] [uniqueidentifier] NOT NULL PRIMARY KEY,
    [StocktakingDate] [datetime] NOT NULL,                    -- Ngày kiểm kho
    [VoucherNumber] [nvarchar](50) NOT NULL,                  -- Số phiếu kiểm kho
    [StocktakingType] [int] NOT NULL,                         -- Loại kiểm kho (0: Định kỳ, 1: Đột xuất, 2: Toàn bộ, 3: Theo sản phẩm)
    [StocktakingStatus] [int] NOT NULL,                       -- Trạng thái (0: Nháp, 1: Đang kiểm, 2: Hoàn thành, 3: Đã duyệt, 4: Đã hủy)
    [WarehouseId] [uniqueidentifier] NOT NULL,                -- Kho kiểm
    [CompanyBranchId] [uniqueidentifier] NULL,                 -- Chi nhánh
    [StartDate] [datetime] NULL,                              -- Ngày bắt đầu kiểm
    [EndDate] [datetime] NULL,                                -- Ngày kết thúc kiểm
    [CountedBy] [uniqueidentifier] NULL,                       -- Người kiểm
    [CountedDate] [datetime] NULL,                            -- Ngày kiểm
    [ReviewedBy] [uniqueidentifier] NULL,                     -- Người rà soát
    [ReviewedDate] [datetime] NULL,                           -- Ngày rà soát
    [ApprovedBy] [uniqueidentifier] NULL,                     -- Người duyệt
    [ApprovedDate] [datetime] NULL,                           -- Ngày duyệt
    [Notes] [nvarchar](1000) NULL,                            -- Ghi chú
    [Reason] [nvarchar](500) NULL,                            -- Lý do kiểm kho
    [IsLocked] [bit] NOT NULL DEFAULT(0),                     -- Đã khóa
    [LockedDate] [datetime] NULL,
    [LockedBy] [uniqueidentifier] NULL,
    [IsActive] [bit] NOT NULL DEFAULT(1),
    [IsDeleted] [bit] NOT NULL DEFAULT(0),
    [CreatedBy] [uniqueidentifier] NULL,
    [CreatedDate] [datetime] NULL,
    [UpdatedBy] [uniqueidentifier] NULL,
    [UpdatedDate] [datetime] NULL,
    [DeletedBy] [uniqueidentifier] NULL,
    [DeletedDate] [datetime] NULL,
    
    CONSTRAINT [FK_StocktakingMaster_Warehouse] 
        FOREIGN KEY ([WarehouseId]) REFERENCES [dbo].[CompanyBranch]([Id]),
    CONSTRAINT [FK_StocktakingMaster_CompanyBranch] 
        FOREIGN KEY ([CompanyBranchId]) REFERENCES [dbo].[CompanyBranch]([Id]),
    CONSTRAINT [FK_StocktakingMaster_CountedBy] 
        FOREIGN KEY ([CountedBy]) REFERENCES [dbo].[ApplicationUser]([Id]),
    CONSTRAINT [FK_StocktakingMaster_ReviewedBy] 
        FOREIGN KEY ([ReviewedBy]) REFERENCES [dbo].[ApplicationUser]([Id]),
    CONSTRAINT [FK_StocktakingMaster_ApprovedBy] 
        FOREIGN KEY ([ApprovedBy]) REFERENCES [dbo].[ApplicationUser]([Id])
);
```

**Giải thích các trường:**
- **StocktakingType**: 
  - 0: Kiểm kho định kỳ (theo lịch)
  - 1: Kiểm kho đột xuất
  - 2: Kiểm kho toàn bộ kho
  - 3: Kiểm kho theo danh sách sản phẩm cụ thể
- **StocktakingStatus**:
  - 0: Nháp (Draft)
  - 1: Đang kiểm (In Progress)
  - 2: Hoàn thành (Completed)
  - 3: Đã duyệt (Approved)
  - 4: Đã hủy (Cancelled)

### 2.2. StocktakingDetail (Chi tiết kiểm kho)

```sql
CREATE TABLE [dbo].[StocktakingDetail] (
    [Id] [uniqueidentifier] NOT NULL PRIMARY KEY,
    [StocktakingMasterId] [uniqueidentifier] NOT NULL,        -- Liên kết với StocktakingMaster
    [ProductVariantId] [uniqueidentifier] NOT NULL,            -- Sản phẩm
    [SystemQuantity] [decimal](18,2) NOT NULL DEFAULT(0),     -- Số lượng hệ thống (tồn kho theo sổ sách)
    [CountedQuantity] [decimal](18,2) NULL,                   -- Số lượng thực tế đếm được
    [DifferenceQuantity] [decimal](18,2) NOT NULL DEFAULT(0), -- Chênh lệch (CountedQuantity - SystemQuantity)
    [SystemValue] [decimal](18,2) NULL,                       -- Giá trị theo sổ sách
    [CountedValue] [decimal](18,2) NULL,                      -- Giá trị thực tế
    [DifferenceValue] [decimal](18,2) NULL,                   -- Chênh lệch giá trị
    [UnitPrice] [decimal](18,2) NULL,                         -- Đơn giá (để tính giá trị)
    [AdjustmentType] [int] NULL,                              -- Loại điều chỉnh (0: Tăng, 1: Giảm, 2: Không điều chỉnh)
    [AdjustmentReason] [nvarchar](500) NULL,                 -- Lý do điều chỉnh
    [IsCounted] [bit] NOT NULL DEFAULT(0),                    -- Đã đếm chưa
    [CountedBy] [uniqueidentifier] NULL,                       -- Người đếm
    [CountedDate] [datetime] NULL,                            -- Ngày đếm
    [IsReviewed] [bit] NOT NULL DEFAULT(0),                   -- Đã rà soát
    [ReviewedBy] [uniqueidentifier] NULL,
    [ReviewedDate] [datetime] NULL,
    [ReviewNotes] [nvarchar](1000) NULL,                     -- Ghi chú rà soát
    [IsApproved] [bit] NOT NULL DEFAULT(0),                   -- Đã duyệt
    [ApprovedBy] [uniqueidentifier] NULL,
    [ApprovedDate] [datetime] NULL,
    [Notes] [nvarchar](1000) NULL,                           -- Ghi chú
    [IsActive] [bit] NOT NULL DEFAULT(1),
    [IsDeleted] [bit] NOT NULL DEFAULT(0),
    [CreatedBy] [uniqueidentifier] NULL,
    [CreatedDate] [datetime] NULL,
    [UpdatedBy] [uniqueidentifier] NULL,
    [UpdatedDate] [datetime] NULL,
    
    CONSTRAINT [FK_StocktakingDetail_StocktakingMaster] 
        FOREIGN KEY ([StocktakingMasterId]) REFERENCES [dbo].[StocktakingMaster]([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_StocktakingDetail_ProductVariant] 
        FOREIGN KEY ([ProductVariantId]) REFERENCES [dbo].[ProductVariant]([Id]),
    CONSTRAINT [FK_StocktakingDetail_CountedBy] 
        FOREIGN KEY ([CountedBy]) REFERENCES [dbo].[ApplicationUser]([Id])
);
```

**Giải thích các trường:**
- **SystemQuantity**: Lấy từ InventoryBalance hoặc tính từ StockInOutMaster đến thời điểm kiểm kho
- **CountedQuantity**: Số lượng thực tế đếm được tại kho
- **DifferenceQuantity**: Chênh lệch = CountedQuantity - SystemQuantity
  - Dương: Thừa hàng
  - Âm: Thiếu hàng
- **AdjustmentType**:
  - 0: Tăng (thừa hàng)
  - 1: Giảm (thiếu hàng)
  - 2: Không điều chỉnh (nếu chênh lệch trong phạm vi cho phép)

### 2.3. StocktakingAdjustment (Điều chỉnh tồn kho sau kiểm)

```sql
CREATE TABLE [dbo].[StocktakingAdjustment] (
    [Id] [uniqueidentifier] NOT NULL PRIMARY KEY,
    [StocktakingMasterId] [uniqueidentifier] NOT NULL,        -- Liên kết với phiếu kiểm kho
    [StocktakingDetailId] [uniqueidentifier] NOT NULL,       -- Chi tiết kiểm kho
    [StockInOutMasterId] [uniqueidentifier] NULL,            -- Liên kết với phiếu điều chỉnh (nếu tạo)
    [ProductVariantId] [uniqueidentifier] NOT NULL,          -- Sản phẩm
    [AdjustmentQuantity] [decimal](18,2) NOT NULL,          -- Số lượng điều chỉnh (có thể âm)
    [AdjustmentValue] [decimal](18,2) NULL,                  -- Giá trị điều chỉnh
    [UnitPrice] [decimal](18,2) NULL,                        -- Đơn giá điều chỉnh
    [AdjustmentType] [int] NOT NULL,                         -- Loại điều chỉnh (0: Tăng, 1: Giảm)
    [AdjustmentReason] [nvarchar](500) NULL,                -- Lý do điều chỉnh
    [AdjustmentDate] [datetime] NOT NULL,                    -- Ngày điều chỉnh
    [AdjustedBy] [uniqueidentifier] NULL,                     -- Người thực hiện điều chỉnh
    [Notes] [nvarchar](1000) NULL,
    [IsApplied] [bit] NOT NULL DEFAULT(0),                   -- Đã áp dụng vào tồn kho chưa
    [AppliedDate] [datetime] NULL,
    [IsActive] [bit] NOT NULL DEFAULT(1),
    [IsDeleted] [bit] NOT NULL DEFAULT(0),
    [CreatedBy] [uniqueidentifier] NULL,
    [CreatedDate] [datetime] NULL,
    [UpdatedBy] [uniqueidentifier] NULL,
    [UpdatedDate] [datetime] NULL,
    
    CONSTRAINT [FK_StocktakingAdjustment_StocktakingMaster] 
        FOREIGN KEY ([StocktakingMasterId]) REFERENCES [dbo].[StocktakingMaster]([Id]),
    CONSTRAINT [FK_StocktakingAdjustment_StocktakingDetail] 
        FOREIGN KEY ([StocktakingDetailId]) REFERENCES [dbo].[StocktakingDetail]([Id]),
    CONSTRAINT [FK_StocktakingAdjustment_StockInOutMaster] 
        FOREIGN KEY ([StockInOutMasterId]) REFERENCES [dbo].[StockInOutMaster]([Id]),
    CONSTRAINT [FK_StocktakingAdjustment_ProductVariant] 
        FOREIGN KEY ([ProductVariantId]) REFERENCES [dbo].[ProductVariant]([Id])
);
```

**Mục đích:** Bảng này lưu lại các điều chỉnh tồn kho được tạo từ kết quả kiểm kho. Có thể tự động tạo StockInOutMaster để điều chỉnh tồn kho.

### 2.4. StocktakingImage (Hình ảnh kiểm kho)

```sql
CREATE TABLE [dbo].[StocktakingImage] (
    [Id] [uniqueidentifier] NOT NULL PRIMARY KEY,
    [StocktakingMasterId] [uniqueidentifier] NULL,           -- Hình ảnh của phiếu kiểm kho
    [StocktakingDetailId] [uniqueidentifier] NULL,            -- Hình ảnh của chi tiết kiểm kho
    [ImageData] [varbinary](max) NULL,                         -- Dữ liệu hình ảnh
    [ImageFileName] [nvarchar](255) NULL,                     -- Tên file
    [ImageContentType] [nvarchar](100) NULL,                  -- Loại file (image/jpeg, image/png)
    [ImageSize] [bigint] NULL,                                 -- Kích thước file (bytes)
    [Description] [nvarchar](500) NULL,                        -- Mô tả
    [IsActive] [bit] NOT NULL DEFAULT(1),
    [IsDeleted] [bit] NOT NULL DEFAULT(0),
    [CreatedBy] [uniqueidentifier] NULL,
    [CreatedDate] [datetime] NULL,
    
    CONSTRAINT [FK_StocktakingImage_StocktakingMaster] 
        FOREIGN KEY ([StocktakingMasterId]) REFERENCES [dbo].[StocktakingMaster]([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_StocktakingImage_StocktakingDetail] 
        FOREIGN KEY ([StocktakingDetailId]) REFERENCES [dbo].[StocktakingDetail]([Id]) ON DELETE CASCADE
);
```

**Mục đích:** Lưu hình ảnh liên quan đến kiểm kho (ảnh kho, ảnh sản phẩm, ảnh chứng từ...)

---

## 3. Quy trình Kiểm kho

### 3.1. Quy trình tổng quan

```
1. Tạo phiếu kiểm kho (StocktakingMaster)
   └─ Status: Nháp (0)
   
2. Thêm danh sách sản phẩm cần kiểm (StocktakingDetail)
   └─ Lấy SystemQuantity từ InventoryBalance hoặc tính toán
   └─ IsCounted = false
   
3. Bắt đầu kiểm kho
   └─ Status: Đang kiểm (1)
   └─ StartDate = DateTime.Now
   
4. Thực hiện đếm (có thể nhiều lần)
   └─ Cập nhật CountedQuantity cho từng StocktakingDetail
   └─ Tính DifferenceQuantity tự động
   └─ IsCounted = true khi hoàn thành đếm
   
5. Hoàn thành kiểm kho
   └─ Status: Hoàn thành (2)
   └─ EndDate = DateTime.Now
   └─ Tính toán tổng hợp chênh lệch
   
6. Rà soát kết quả (tùy chọn)
   └─ ReviewedBy, ReviewedDate
   └─ IsReviewed = true cho các dòng cần rà soát
   
7. Duyệt phiếu kiểm kho
   └─ Status: Đã duyệt (3)
   └─ ApprovedBy, ApprovedDate
   
8. Tạo điều chỉnh tồn kho (nếu có chênh lệch)
   └─ Tạo StocktakingAdjustment
   └─ Tự động tạo StockInOutMaster (nếu cần)
   └─ Cập nhật InventoryBalance
```

### 3.2. Chi tiết các bước

#### Bước 1: Tạo phiếu kiểm kho

**Input:**
- WarehouseId: Kho cần kiểm
- StocktakingType: Loại kiểm kho
- Reason: Lý do kiểm kho
- Danh sách ProductVariantId (nếu StocktakingType = 3)

**Xử lý:**
- Tạo StocktakingMaster với Status = 0 (Nháp)
- Tự động sinh VoucherNumber (VD: KK-2025-001)
- Nếu StocktakingType = 2 (Toàn bộ), tự động lấy tất cả sản phẩm có tồn trong kho
- Nếu StocktakingType = 3 (Theo danh sách), chỉ tạo StocktakingDetail cho các sản phẩm được chọn

**Output:**
- StocktakingMaster mới với danh sách StocktakingDetail

#### Bước 2: Lấy số lượng hệ thống

**Xử lý:**
- Với mỗi StocktakingDetail, lấy SystemQuantity từ:
  - InventoryBalance.ClosingBalance (nếu có)
  - Hoặc tính từ StockInOutMaster đến thời điểm kiểm kho
- Lưu vào StocktakingDetail.SystemQuantity
- Tính SystemValue = SystemQuantity * UnitPrice (nếu có)

#### Bước 3: Thực hiện đếm

**Input:**
- StocktakingDetailId
- CountedQuantity: Số lượng thực tế đếm được

**Xử lý:**
- Cập nhật CountedQuantity
- Tính DifferenceQuantity = CountedQuantity - SystemQuantity
- Cập nhật IsCounted = true
- Cập nhật CountedBy, CountedDate
- Tính CountedValue = CountedQuantity * UnitPrice
- Tính DifferenceValue = CountedValue - SystemValue

**Validation:**
- CountedQuantity >= 0
- Có thể cho phép đếm nhiều lần và lấy lần đếm cuối cùng

#### Bước 4: Hoàn thành kiểm kho

**Xử lý:**
- Kiểm tra tất cả StocktakingDetail đã được đếm (IsCounted = true)
- Cập nhật StocktakingMaster.Status = 2 (Hoàn thành)
- Cập nhật EndDate
- Tính tổng hợp:
  - Tổng số sản phẩm kiểm
  - Tổng số sản phẩm có chênh lệch
  - Tổng chênh lệch số lượng
  - Tổng chênh lệch giá trị

#### Bước 5: Rà soát (tùy chọn)

**Input:**
- StocktakingDetailId (hoặc toàn bộ)
- ReviewNotes: Ghi chú rà soát

**Xử lý:**
- Cập nhật IsReviewed = true
- Cập nhật ReviewedBy, ReviewedDate
- Có thể điều chỉnh CountedQuantity nếu phát hiện sai sót

#### Bước 6: Duyệt phiếu kiểm kho

**Xử lý:**
- Kiểm tra quyền duyệt
- Cập nhật StocktakingMaster.Status = 3 (Đã duyệt)
- Cập nhật ApprovedBy, ApprovedDate
- Có thể khóa phiếu (IsLocked = true)

#### Bước 7: Tạo điều chỉnh tồn kho

**Xử lý:**
- Với mỗi StocktakingDetail có DifferenceQuantity != 0:
  - Tạo StocktakingAdjustment
  - AdjustmentQuantity = DifferenceQuantity
  - AdjustmentType:
    - 0 nếu DifferenceQuantity > 0 (thừa)
    - 1 nếu DifferenceQuantity < 0 (thiếu)
  
- Tự động tạo StockInOutMaster để điều chỉnh:
  - StockInOutType = 4 (Điều chỉnh từ kiểm kho) - cần thêm enum này
  - VoucherStatus = 1 (Đã duyệt)
  - Tạo StockInOutDetail:
    - Nếu thừa: StockInQty = DifferenceQuantity
    - Nếu thiếu: StockOutQty = Abs(DifferenceQuantity)
  
- Cập nhật InventoryBalance:
  - Cập nhật ClosingBalance
  - Cập nhật các giá trị liên quan

- Đánh dấu StocktakingAdjustment.IsApplied = true

---

## 4. Các enum và constant cần bổ sung

### 4.1. StocktakingType (Enum)
```csharp
public enum StocktakingType
{
    Periodic = 0,        // Định kỳ
    AdHoc = 1,           // Đột xuất
    FullWarehouse = 2,   // Toàn bộ kho
    ByProductList = 3    // Theo danh sách sản phẩm
}
```

### 4.2. StocktakingStatus (Enum)
```csharp
public enum StocktakingStatus
{
    Draft = 0,           // Nháp
    InProgress = 1,      // Đang kiểm
    Completed = 2,       // Hoàn thành
    Approved = 3,        // Đã duyệt
    Cancelled = 4        // Đã hủy
}
```

### 4.3. AdjustmentType (Enum)
```csharp
public enum AdjustmentType
{
    Increase = 0,        // Tăng
    Decrease = 1,        // Giảm
    NoAdjustment = 2     // Không điều chỉnh
}
```

### 4.4. Cần bổ sung vào StockInOutType
- Thêm giá trị mới: `StockAdjustmentFromStocktaking = 4` (Điều chỉnh từ kiểm kho)

---

## 5. Các báo cáo và truy vấn cần thiết

### 5.1. Báo cáo kết quả kiểm kho
- Danh sách phiếu kiểm kho theo kho, thời gian
- Chi tiết chênh lệch theo sản phẩm
- Tổng hợp chênh lệch theo kho
- Lịch sử kiểm kho

### 5.2. Truy vấn thường dùng
- Lấy danh sách sản phẩm cần kiểm (tồn > 0 hoặc theo điều kiện)
- Tính số lượng hệ thống tại thời điểm kiểm kho
- So sánh kết quả kiểm kho giữa các kỳ
- Theo dõi các sản phẩm thường xuyên có chênh lệch

---

## 6. Lưu ý và đề xuất bổ sung

### 6.1. Tối ưu hóa
- Index trên các cột thường query:
  - StocktakingMaster: WarehouseId, StocktakingDate, StocktakingStatus
  - StocktakingDetail: StocktakingMasterId, ProductVariantId, IsCounted
  - StocktakingAdjustment: StocktakingMasterId, IsApplied

### 6.2. Tính năng mở rộng (tùy chọn)
- **Kiểm kho theo vị trí**: Thêm LocationId vào StocktakingDetail nếu kho có chia vị trí
- **Kiểm kho theo lô/serial**: Liên kết với ProductVariantIdentifier để kiểm tra từng lô/serial
- **Kiểm kho định kỳ tự động**: Tạo job tự động tạo phiếu kiểm kho theo lịch
- **Cảnh báo chênh lệch lớn**: Thiết lập ngưỡng chênh lệch để cảnh báo
- **Xuất Excel**: Xuất danh sách kiểm kho ra Excel để đếm offline, sau đó import lại

### 6.3. Bảo mật và quyền
- Quyền tạo phiếu kiểm kho
- Quyền thực hiện đếm
- Quyền rà soát
- Quyền duyệt và tạo điều chỉnh
- Quyền xem báo cáo kiểm kho

---

## 7. Script SQL tạo bảng (tổng hợp)

Xem file SQL riêng: `Database/Create_Stocktaking_Tables.sql`
