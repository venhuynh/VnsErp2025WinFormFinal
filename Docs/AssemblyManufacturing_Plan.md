# PLAN: Module Assembly/Manufacturing (Lắp Ráp Sản Phẩm)

## 📋 Tổng Quan

Module này cho phép:
- Định nghĩa BOM (Bill of Materials) - cấu trúc sản phẩm lắp ráp
- Tự động xuất linh kiện và nhập sản phẩm hoàn chỉnh trong 1 giao dịch
- Tính giá thành sản phẩm từ giá linh kiện
- Theo dõi lịch sử lắp ráp

## 🎯 Mục Tiêu

1. Tạo bảng `ProductBOM` để lưu cấu trúc sản phẩm
2. Thêm enum mới cho loại nhập/xuất lắp ráp
3. Tạo form quản lý BOM (CRUD)
4. Tạo form lắp ráp sản phẩm
5. Logic tự động tạo phiếu xuất linh kiện và nhập sản phẩm

## 📊 Database Schema

### Bảng ProductBOM

```sql
CREATE TABLE [dbo].[ProductBOM](
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
    [ProductVariantId] UNIQUEIDENTIFIER NOT NULL,  -- Sản phẩm hoàn chỉnh
    [ComponentVariantId] UNIQUEIDENTIFIER NOT NULL, -- Linh kiện
    [Quantity] DECIMAL(18, 2) NOT NULL,            -- Số lượng linh kiện cần
    [UnitId] UNIQUEIDENTIFIER NOT NULL,            -- Đơn vị tính
    [IsActive] BIT NOT NULL DEFAULT 1,
    [CreatedDate] DATETIME2 NOT NULL DEFAULT GETDATE(),
    [ModifiedDate] DATETIME2 NOT NULL DEFAULT GETDATE(),
    [Notes] NVARCHAR(1000) NULL,
    
    CONSTRAINT [FK_ProductBOM_ProductVariant] 
        FOREIGN KEY([ProductVariantId]) REFERENCES [dbo].[ProductVariant]([Id]),
    CONSTRAINT [FK_ProductBOM_ComponentVariant] 
        FOREIGN KEY([ComponentVariantId]) REFERENCES [dbo].[ProductVariant]([Id]),
    CONSTRAINT [FK_ProductBOM_Unit] 
        FOREIGN KEY([UnitId]) REFERENCES [dbo].[UnitOfMeasure]([Id])
)
```

### Bảng AssemblyTransaction (Lịch sử lắp ráp)

```sql
CREATE TABLE [dbo].[AssemblyTransaction](
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
    [AssemblyDate] DATETIME NOT NULL,
    [ProductVariantId] UNIQUEIDENTIFIER NOT NULL,  -- Sản phẩm lắp ráp
    [Quantity] DECIMAL(18, 2) NOT NULL,            -- Số lượng lắp ráp
    [StockOutMasterId] UNIQUEIDENTIFIER NOT NULL,  -- Phiếu xuất linh kiện
    [StockInMasterId] UNIQUEIDENTIFIER NOT NULL,   -- Phiếu nhập sản phẩm
    [WarehouseId] UNIQUEIDENTIFIER NOT NULL,
    [TotalCost] DECIMAL(18, 2) NOT NULL,          -- Tổng giá thành
    [CreatedBy] UNIQUEIDENTIFIER NULL,
    [CreatedDate] DATETIME NOT NULL DEFAULT GETDATE(),
    [Notes] NVARCHAR(1000) NULL,
    
    CONSTRAINT [FK_AssemblyTransaction_ProductVariant] 
        FOREIGN KEY([ProductVariantId]) REFERENCES [dbo].[ProductVariant]([Id]),
    CONSTRAINT [FK_AssemblyTransaction_StockOutMaster] 
        FOREIGN KEY([StockOutMasterId]) REFERENCES [dbo].[StockInOutMaster]([Id]),
    CONSTRAINT [FK_AssemblyTransaction_StockInMaster] 
        FOREIGN KEY([StockInMasterId]) REFERENCES [dbo].[StockInOutMaster]([Id]),
    CONSTRAINT [FK_AssemblyTransaction_Warehouse] 
        FOREIGN KEY([WarehouseId]) REFERENCES [dbo].[CompanyBranch]([Id])
)
```

## 🔧 Thay Đổi Code

### 1. Enum mới

Thêm vào `LoaiNhapXuatKhoEnum`:
- `XuatLinhKienLapRap = 16`
- `NhapSanPhamLapRap = 6`

### 2. Layers cần tạo

**DAL:**
- `IProductBOMRepository.cs`
- `ProductBOMRepository.cs`
- `IAssemblyTransactionRepository.cs`
- `AssemblyTransactionRepository.cs`

**BLL:**
- `ProductBOMBll.cs`
- `AssemblyBll.cs`

**DTO:**
- `ProductBOMDto.cs`
- `AssemblyTransactionDto.cs`
- `AssemblyRequestDto.cs`

**UI:**
- `FrmProductBOM.cs` - Quản lý BOM
- `FrmAssembly.cs` - Form lắp ráp sản phẩm

## ⚠️ Rủi Ro & Rollback

**Rủi ro:**
- Thay đổi database schema - cần backup trước
- Logic phức tạp - cần test kỹ
- Ảnh hưởng đến tồn kho - cần validate đủ linh kiện

**Rollback:**
- Xóa các bảng mới
- Revert enum changes
- Xóa code mới

## ✅ Checklist Triển Khai

- [ ] Tạo SQL script cho ProductBOM và AssemblyTransaction
- [ ] Update dbml file
- [ ] Thêm enum mới
- [ ] Tạo DAL layer
- [ ] Tạo BLL layer
- [ ] Tạo DTO
- [ ] Tạo form quản lý BOM
- [ ] Tạo form lắp ráp
- [ ] Test end-to-end
- [ ] Update documentation

