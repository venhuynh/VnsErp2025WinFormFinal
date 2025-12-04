# Database Schema: Asset Management (Quản lý Tài sản)

## 📋 Tổng quan

Bảng `Asset` được thiết kế để quản lý tài sản cố định trong hệ thống ERP, bao gồm:
- Tài sản cố định (máy móc, thiết bị, phương tiện, nhà xưởng, v.v.)
- Tài sản lưu động (máy tính, điện thoại, v.v.)
- Tài sản vô hình (bản quyền, thương hiệu, v.v.)

## 🗄️ Database Schema

### 1. Asset Table

```sql
USE [VnsErp2025Final]
GO

/****** Object: Table [dbo].[Asset] Script Date: 12/04/2025 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Asset](
    -- Primary Key
    [Id] [uniqueidentifier] NOT NULL,
    
    -- Thông tin cơ bản
    [AssetCode] [nvarchar](50) NOT NULL,                    -- Mã tài sản (unique)
    [AssetName] [nvarchar](255) NOT NULL,                    -- Tên tài sản
    [AssetType] [int] NOT NULL,                               -- Loại tài sản (0: Cố định, 1: Lưu động, 2: Vô hình)
    [AssetCategory] [int] NOT NULL,                          -- Danh mục tài sản (0: Máy móc, 1: Thiết bị, 2: Phương tiện, 3: Nhà xưởng, 4: Khác)
    [Description] [nvarchar](1000) NULL,                     -- Mô tả
    
    -- Thông tin sản phẩm (nếu tài sản là sản phẩm trong hệ thống)
    [ProductVariantId] [uniqueidentifier] NULL,              -- Liên kết với ProductVariant (nếu có)
    
    -- Thông tin định vị
    [CompanyId] [uniqueidentifier] NOT NULL,                 -- Công ty
    [BranchId] [uniqueidentifier] NULL,                       -- Chi nhánh
    [DepartmentId] [uniqueidentifier] NULL,                  -- Phòng ban
    [AssignedEmployeeId] [uniqueidentifier] NULL,            -- Nhân viên phụ trách
    [Location] [nvarchar](500) NULL,                         -- Vị trí cụ thể
    
    -- Thông tin tài chính
    [PurchasePrice] [decimal](18, 2) NOT NULL,              -- Giá mua
    [PurchaseDate] [datetime] NULL,                          -- Ngày mua
    [SupplierName] [nvarchar](255) NULL,                     -- Nhà cung cấp
    [InvoiceNumber] [nvarchar](100) NULL,                    -- Số hóa đơn
    [InvoiceDate] [datetime] NULL,                          -- Ngày hóa đơn
    
    -- Thông tin khấu hao
    [DepreciationMethod] [int] NOT NULL DEFAULT 0,          -- Phương pháp khấu hao (0: Đường thẳng, 1: Số dư giảm dần, 2: Không khấu hao)
    [DepreciationRate] [decimal](5, 2) NULL,                 -- Tỷ lệ khấu hao (%/năm)
    [UsefulLife] [int] NULL,                                 -- Thời gian sử dụng (tháng)
    [DepreciationStartDate] [datetime] NULL,                -- Ngày bắt đầu khấu hao
    [AccumulatedDepreciation] [decimal](18, 2) NOT NULL DEFAULT 0, -- Khấu hao lũy kế
    [CurrentValue] [decimal](18, 2) NULL,                    -- Giá trị hiện tại (tự động tính: PurchasePrice - AccumulatedDepreciation)
    
    -- Thông tin trạng thái
    [Status] [int] NOT NULL DEFAULT 0,                      -- Trạng thái (0: Mới, 1: Đang sử dụng, 2: Bảo trì, 3: Ngừng sử dụng, 4: Thanh lý)
    [Condition] [int] NOT NULL DEFAULT 0,                   -- Tình trạng (0: Tốt, 1: Khá, 2: Trung bình, 3: Kém, 4: Hỏng)
    [IsActive] [bit] NOT NULL DEFAULT 1,                    -- Đang hoạt động
    [IsDeleted] [bit] NOT NULL DEFAULT 0,                   -- Đã xóa
    
    -- Thông tin bảo hành
    [WarrantyId] [uniqueidentifier] NULL,                    -- Liên kết với Warranty (nếu có)
    [WarrantyExpiryDate] [datetime] NULL,                    -- Ngày hết hạn bảo hành
    
    -- Thông tin bổ sung
    [SerialNumber] [nvarchar](100) NULL,                     -- Số seri
    [Manufacturer] [nvarchar](255) NULL,                    -- Nhà sản xuất
    [Model] [nvarchar](255) NULL,                            -- Model
    [Specifications] [nvarchar](2000) NULL,                  -- Thông số kỹ thuật
    [Notes] [nvarchar](1000) NULL,                           -- Ghi chú
    
    -- Audit Fields
    [CreateDate] [datetime] NOT NULL DEFAULT GETDATE(),     -- Ngày tạo
    [CreateBy] [uniqueidentifier] NULL,                     -- Người tạo
    [ModifiedDate] [datetime] NULL,                          -- Ngày sửa
    [ModifiedBy] [uniqueidentifier] NULL,                    -- Người sửa
    [DeletedDate] [datetime] NULL,                          -- Ngày xóa
    [DeletedBy] [uniqueidentifier] NULL,                    -- Người xóa
    
    -- Constraints
    CONSTRAINT [PK_Asset] PRIMARY KEY CLUSTERED ([Id] ASC)
        WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, 
              IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, 
              ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
    
    CONSTRAINT [UQ_Asset_AssetCode] UNIQUE NONCLUSTERED ([AssetCode] ASC, [IsDeleted] ASC)
        WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, 
              IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, 
              ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

-- Default Constraints
ALTER TABLE [dbo].[Asset] ADD DEFAULT (NEWID()) FOR [Id]
GO

ALTER TABLE [dbo].[Asset] ADD DEFAULT (0) FOR [AssetType]
GO

ALTER TABLE [dbo].[Asset] ADD DEFAULT (0) FOR [AssetCategory]
GO

ALTER TABLE [dbo].[Asset] ADD DEFAULT (0) FOR [DepreciationMethod]
GO

ALTER TABLE [dbo].[Asset] ADD DEFAULT (0) FOR [AccumulatedDepreciation]
GO

ALTER TABLE [dbo].[Asset] ADD DEFAULT (0) FOR [Status]
GO

ALTER TABLE [dbo].[Asset] ADD DEFAULT (0) FOR [Condition]
GO

ALTER TABLE [dbo].[Asset] ADD DEFAULT (1) FOR [IsActive]
GO

ALTER TABLE [dbo].[Asset] ADD DEFAULT (0) FOR [IsDeleted]
GO

ALTER TABLE [dbo].[Asset] ADD DEFAULT (GETDATE()) FOR [CreateDate]
GO

-- Foreign Key Constraints
ALTER TABLE [dbo].[Asset] WITH CHECK 
    ADD CONSTRAINT [FK_Asset_Company] 
    FOREIGN KEY([CompanyId]) REFERENCES [dbo].[Company] ([Id])
GO

ALTER TABLE [dbo].[Asset] CHECK CONSTRAINT [FK_Asset_Company]
GO

ALTER TABLE [dbo].[Asset] WITH CHECK 
    ADD CONSTRAINT [FK_Asset_CompanyBranch] 
    FOREIGN KEY([BranchId]) REFERENCES [dbo].[CompanyBranch] ([Id])
GO

ALTER TABLE [dbo].[Asset] CHECK CONSTRAINT [FK_Asset_CompanyBranch]
GO

ALTER TABLE [dbo].[Asset] WITH CHECK 
    ADD CONSTRAINT [FK_Asset_Department] 
    FOREIGN KEY([DepartmentId]) REFERENCES [dbo].[Department] ([Id])
GO

ALTER TABLE [dbo].[Asset] CHECK CONSTRAINT [FK_Asset_Department]
GO

ALTER TABLE [dbo].[Asset] WITH CHECK 
    ADD CONSTRAINT [FK_Asset_Employee] 
    FOREIGN KEY([AssignedEmployeeId]) REFERENCES [dbo].[Employee] ([Id])
GO

ALTER TABLE [dbo].[Asset] CHECK CONSTRAINT [FK_Asset_Employee]
GO

ALTER TABLE [dbo].[Asset] WITH CHECK 
    ADD CONSTRAINT [FK_Asset_ProductVariant] 
    FOREIGN KEY([ProductVariantId]) REFERENCES [dbo].[ProductVariant] ([Id])
GO

ALTER TABLE [dbo].[Asset] CHECK CONSTRAINT [FK_Asset_ProductVariant]
GO

ALTER TABLE [dbo].[Asset] WITH CHECK 
    ADD CONSTRAINT [FK_Asset_Warranty] 
    FOREIGN KEY([WarrantyId]) REFERENCES [dbo].[Warranty] ([Id])
GO

ALTER TABLE [dbo].[Asset] CHECK CONSTRAINT [FK_Asset_Warranty]
GO

ALTER TABLE [dbo].[Asset] WITH CHECK 
    ADD CONSTRAINT [FK_Asset_CreateBy] 
    FOREIGN KEY([CreateBy]) REFERENCES [dbo].[ApplicationUser] ([Id])
GO

ALTER TABLE [dbo].[Asset] CHECK CONSTRAINT [FK_Asset_CreateBy]
GO

ALTER TABLE [dbo].[Asset] WITH CHECK 
    ADD CONSTRAINT [FK_Asset_ModifiedBy] 
    FOREIGN KEY([ModifiedBy]) REFERENCES [dbo].[ApplicationUser] ([Id])
GO

ALTER TABLE [dbo].[Asset] CHECK CONSTRAINT [FK_Asset_ModifiedBy]
GO

ALTER TABLE [dbo].[Asset] WITH CHECK 
    ADD CONSTRAINT [FK_Asset_DeletedBy] 
    FOREIGN KEY([DeletedBy]) REFERENCES [dbo].[ApplicationUser] ([Id])
GO

ALTER TABLE [dbo].[Asset] CHECK CONSTRAINT [FK_Asset_DeletedBy]
GO

-- Check Constraints
ALTER TABLE [dbo].[Asset] WITH CHECK 
    ADD CONSTRAINT [CHK_Asset_AssetType] 
    CHECK (([AssetType] >= 0 AND [AssetType] <= 2))
GO

ALTER TABLE [dbo].[Asset] CHECK CONSTRAINT [CHK_Asset_AssetType]
GO

ALTER TABLE [dbo].[Asset] WITH CHECK 
    ADD CONSTRAINT [CHK_Asset_AssetCategory] 
    CHECK (([AssetCategory] >= 0 AND [AssetCategory] <= 4))
GO

ALTER TABLE [dbo].[Asset] CHECK CONSTRAINT [CHK_Asset_AssetCategory]
GO

ALTER TABLE [dbo].[Asset] WITH CHECK 
    ADD CONSTRAINT [CHK_Asset_DepreciationMethod] 
    CHECK (([DepreciationMethod] >= 0 AND [DepreciationMethod] <= 2))
GO

ALTER TABLE [dbo].[Asset] CHECK CONSTRAINT [CHK_Asset_DepreciationMethod]
GO

ALTER TABLE [dbo].[Asset] WITH CHECK 
    ADD CONSTRAINT [CHK_Asset_Status] 
    CHECK (([Status] >= 0 AND [Status] <= 4))
GO

ALTER TABLE [dbo].[Asset] CHECK CONSTRAINT [CHK_Asset_Status]
GO

ALTER TABLE [dbo].[Asset] WITH CHECK 
    ADD CONSTRAINT [CHK_Asset_Condition] 
    CHECK (([Condition] >= 0 AND [Condition] <= 4))
GO

ALTER TABLE [dbo].[Asset] CHECK CONSTRAINT [CHK_Asset_Condition]
GO

ALTER TABLE [dbo].[Asset] WITH CHECK 
    ADD CONSTRAINT [CHK_Asset_PurchasePrice] 
    CHECK (([PurchasePrice] >= 0))
GO

ALTER TABLE [dbo].[Asset] CHECK CONSTRAINT [CHK_Asset_PurchasePrice]
GO

ALTER TABLE [dbo].[Asset] WITH CHECK 
    ADD CONSTRAINT [CHK_Asset_AccumulatedDepreciation] 
    CHECK (([AccumulatedDepreciation] >= 0))
GO

ALTER TABLE [dbo].[Asset] CHECK CONSTRAINT [CHK_Asset_AccumulatedDepreciation]
GO

ALTER TABLE [dbo].[Asset] WITH CHECK 
    ADD CONSTRAINT [CHK_Asset_CurrentValue] 
    CHECK (([CurrentValue] IS NULL OR [CurrentValue] >= 0))
GO

ALTER TABLE [dbo].[Asset] CHECK CONSTRAINT [CHK_Asset_CurrentValue]
GO

-- Indexes
CREATE NONCLUSTERED INDEX [IX_Asset_AssetCode] 
    ON [dbo].[Asset] ([AssetCode] ASC)
    WHERE [IsDeleted] = 0
GO

CREATE NONCLUSTERED INDEX [IX_Asset_CompanyId] 
    ON [dbo].[Asset] ([CompanyId] ASC)
    WHERE [IsDeleted] = 0
GO

CREATE NONCLUSTERED INDEX [IX_Asset_BranchId] 
    ON [dbo].[Asset] ([BranchId] ASC)
    WHERE [IsDeleted] = 0
GO

CREATE NONCLUSTERED INDEX [IX_Asset_DepartmentId] 
    ON [dbo].[Asset] ([DepartmentId] ASC)
    WHERE [IsDeleted] = 0
GO

CREATE NONCLUSTERED INDEX [IX_Asset_AssignedEmployeeId] 
    ON [dbo].[Asset] ([AssignedEmployeeId] ASC)
    WHERE [IsDeleted] = 0
GO

CREATE NONCLUSTERED INDEX [IX_Asset_Status] 
    ON [dbo].[Asset] ([Status] ASC)
    WHERE [IsDeleted] = 0 AND [IsActive] = 1
GO

CREATE NONCLUSTERED INDEX [IX_Asset_ProductVariantId] 
    ON [dbo].[Asset] ([ProductVariantId] ASC)
    WHERE [ProductVariantId] IS NOT NULL AND [IsDeleted] = 0
GO
```

## 📊 Mô tả các trường

### Thông tin cơ bản
- **AssetCode**: Mã tài sản (unique, không trùng lặp)
- **AssetName**: Tên tài sản
- **AssetType**: Loại tài sản
  - `0`: Tài sản cố định
  - `1`: Tài sản lưu động
  - `2`: Tài sản vô hình
- **AssetCategory**: Danh mục tài sản
  - `0`: Máy móc
  - `1`: Thiết bị
  - `2`: Phương tiện
  - `3`: Nhà xưởng
  - `4`: Khác

### Thông tin định vị
- **CompanyId**: Công ty sở hữu (required)
- **BranchId**: Chi nhánh (optional)
- **DepartmentId**: Phòng ban (optional)
- **AssignedEmployeeId**: Nhân viên phụ trách (optional)
- **Location**: Vị trí cụ thể (optional)

### Thông tin tài chính
- **PurchasePrice**: Giá mua (required, >= 0)
- **PurchaseDate**: Ngày mua (optional)
- **SupplierName**: Nhà cung cấp (optional)
- **InvoiceNumber**: Số hóa đơn (optional)
- **InvoiceDate**: Ngày hóa đơn (optional)

### Thông tin khấu hao
- **DepreciationMethod**: Phương pháp khấu hao
  - `0`: Đường thẳng (Straight-line)
  - `1`: Số dư giảm dần (Declining balance)
  - `2`: Không khấu hao
- **DepreciationRate**: Tỷ lệ khấu hao (%/năm, optional)
- **UsefulLife**: Thời gian sử dụng (tháng, optional)
- **DepreciationStartDate**: Ngày bắt đầu khấu hao (optional)
- **AccumulatedDepreciation**: Khấu hao lũy kế (default: 0)
- **CurrentValue**: Giá trị hiện tại (tự động tính: PurchasePrice - AccumulatedDepreciation)

### Thông tin trạng thái
- **Status**: Trạng thái sử dụng
  - `0`: Mới
  - `1`: Đang sử dụng
  - `2`: Bảo trì
  - `3`: Ngừng sử dụng
  - `4`: Thanh lý
- **Condition**: Tình trạng
  - `0`: Tốt
  - `1`: Khá
  - `2`: Trung bình
  - `3`: Kém
  - `4`: Hỏng

### Thông tin bảo hành
- **WarrantyId**: Liên kết với bảng Warranty (optional)
- **WarrantyExpiryDate**: Ngày hết hạn bảo hành (optional)

### Thông tin bổ sung
- **SerialNumber**: Số seri (optional)
- **Manufacturer**: Nhà sản xuất (optional)
- **Model**: Model (optional)
- **Specifications**: Thông số kỹ thuật (optional)
- **Notes**: Ghi chú (optional)

## 🔗 Relationships

### Foreign Keys
- `CompanyId` → `Company.Id`
- `BranchId` → `CompanyBranch.Id`
- `DepartmentId` → `Department.Id`
- `AssignedEmployeeId` → `Employee.Id`
- `ProductVariantId` → `ProductVariant.Id` (optional)
- `WarrantyId` → `Warranty.Id` (optional)
- `CreateBy` → `ApplicationUser.Id` (optional)
- `ModifiedBy` → `ApplicationUser.Id` (optional)
- `DeletedBy` → `ApplicationUser.Id` (optional)

## 📈 Indexes

1. **IX_Asset_AssetCode**: Index trên `AssetCode` (filtered: `IsDeleted = 0`)
2. **IX_Asset_CompanyId**: Index trên `CompanyId` (filtered: `IsDeleted = 0`)
3. **IX_Asset_BranchId**: Index trên `BranchId` (filtered: `IsDeleted = 0`)
4. **IX_Asset_DepartmentId**: Index trên `DepartmentId` (filtered: `IsDeleted = 0`)
5. **IX_Asset_AssignedEmployeeId**: Index trên `AssignedEmployeeId` (filtered: `IsDeleted = 0`)
6. **IX_Asset_Status**: Index trên `Status` (filtered: `IsDeleted = 0 AND IsActive = 1`)
7. **IX_Asset_ProductVariantId**: Index trên `ProductVariantId` (filtered: `ProductVariantId IS NOT NULL AND IsDeleted = 0`)

## ✅ Constraints

### Check Constraints
- `CHK_Asset_AssetType`: AssetType phải trong khoảng 0-2
- `CHK_Asset_AssetCategory`: AssetCategory phải trong khoảng 0-4
- `CHK_Asset_DepreciationMethod`: DepreciationMethod phải trong khoảng 0-2
- `CHK_Asset_Status`: Status phải trong khoảng 0-4
- `CHK_Asset_Condition`: Condition phải trong khoảng 0-4
- `CHK_Asset_PurchasePrice`: PurchasePrice >= 0
- `CHK_Asset_AccumulatedDepreciation`: AccumulatedDepreciation >= 0
- `CHK_Asset_CurrentValue`: CurrentValue >= 0 (nếu có giá trị)

### Unique Constraints
- `UQ_Asset_AssetCode`: AssetCode phải unique (kết hợp với IsDeleted để cho phép soft delete)

## 🔄 Tính năng bổ sung có thể thêm

1. **AssetTransfer**: Bảng lịch sử chuyển giao tài sản (tương tự DeviceTransfer)
2. **AssetMaintenance**: Bảng lịch sử bảo trì tài sản
3. **AssetDepreciation**: Bảng chi tiết khấu hao theo tháng
4. **AssetImage**: Bảng hình ảnh tài sản (tương tự ProductImage)

## 📝 Notes

- Bảng này được thiết kế theo pattern của các bảng khác trong hệ thống (Device, InventoryBalance)
- Sử dụng soft delete (`IsDeleted`) để giữ lại lịch sử
- Có đầy đủ audit fields (CreateDate, CreateBy, ModifiedDate, ModifiedBy, DeletedDate, DeletedBy)
- Hỗ trợ liên kết với ProductVariant nếu tài sản là sản phẩm trong hệ thống
- Hỗ trợ liên kết với Warranty để quản lý bảo hành
- Có thể mở rộng thêm các bảng liên quan như AssetTransfer, AssetMaintenance, AssetDepreciation

