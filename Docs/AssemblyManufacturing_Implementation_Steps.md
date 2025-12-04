# Hướng Dẫn Triển Khai Module Assembly/Manufacturing

## ✅ Đã Hoàn Thành

1. ✅ Tạo Plan chi tiết (`Docs/AssemblyManufacturing_Plan.md`)
2. ✅ Tạo SQL scripts:
   - `Dal/Scripts/CreateProductBOMTable.sql`
   - `Dal/Scripts/CreateAssemblyTransactionTable.sql`
3. ✅ Thêm enum mới vào `LoaiNhapXuatKhoEnum`:
   - `NhapSanPhamLapRap = 6`
   - `XuatLinhKienLapRap = 16`
4. ✅ Tạo DTO:
   - `DTO/Inventory/Assembly/ProductBOMDto.cs`
   - `DTO/Inventory/Assembly/AssemblyTransactionDto.cs`
   - `DTO/Inventory/Assembly/AssemblyRequestDto.cs`
5. ✅ Tạo Interface:
   - `Dal/DataAccess/Interfaces/Inventory/Assembly/IProductBOMRepository.cs`
   - `Dal/DataAccess/Interfaces/Inventory/Assembly/IAssemblyTransactionRepository.cs`
6. ✅ Tạo BLL:
   - `Bll/Inventory/Assembly/ProductBOMBll.cs`

## 🔨 Các Bước Tiếp Theo

### Bước 1: Chạy SQL Scripts

1. Mở SQL Server Management Studio
2. Kết nối đến database `VnsErp2025Final`
3. Chạy lần lượt:
   - `Dal/Scripts/CreateProductBOMTable.sql`
   - `Dal/Scripts/CreateAssemblyTransactionTable.sql`

### Bước 2: Update DBML File

1. Mở `Dal/DataContext/VnsErp2025.dbml` trong Visual Studio
2. Right-click → "Add Table" → Chọn 2 bảng mới:
   - `ProductBOM`
   - `AssemblyTransaction`
3. Save và rebuild project để generate entities

### Bước 3: Tạo Repository Implementation

Tạo file `Dal/DataAccess/Implementations/Inventory/Assembly/ProductBOMRepository.cs`:

```csharp
// Tham khảo pattern từ AssetRepository.cs
// Implement các method trong IProductBOMRepository
// Sử dụng DataContext để CRUD với ProductBOM table
```

Tạo file `Dal/DataAccess/Implementations/Inventory/Assembly/AssemblyTransactionRepository.cs`:

```csharp
// Implement các method trong IAssemblyTransactionRepository
// Sử dụng DataContext để CRUD với AssemblyTransaction table
```

### Bước 4: Tạo AssemblyBll

Tạo file `Bll/Inventory/Assembly/AssemblyBll.cs` với logic chính:

1. **Method `AssembleProductAsync(AssemblyRequestDto request)`:**
   - Load BOM từ ProductVariantId
   - Validate đủ linh kiện trong kho
   - Tạo phiếu xuất linh kiện (StockOutMaster với type = XuatLinhKienLapRap)
   - Tạo phiếu nhập sản phẩm (StockInMaster với type = NhapSanPhamLapRap)
   - Tính giá thành từ giá linh kiện
   - Lưu AssemblyTransaction
   - Tất cả trong 1 transaction

2. **Method `CalculateAssemblyCost(Guid productVariantId, decimal quantity)`:**
   - Load BOM
   - Lấy giá linh kiện từ tồn kho (InventoryBalance hoặc giá mua gần nhất)
   - Tính tổng giá thành = sum(Quantity * UnitPrice) cho mỗi linh kiện

### Bước 5: Tạo Form Quản Lý BOM

Tạo `Inventory/Assembly/FrmProductBOM.cs`:
- Grid hiển thị danh sách BOM
- Form thêm/sửa BOM
- Chọn ProductVariant (sản phẩm hoàn chỉnh)
- Chọn ComponentVariant (linh kiện)
- Nhập số lượng và đơn vị

### Bước 6: Tạo Form Lắp Ráp

Tạo `Inventory/Assembly/FrmAssembly.cs`:
- Chọn sản phẩm cần lắp (ProductVariant)
- Nhập số lượng
- Chọn kho
- Hiển thị danh sách linh kiện cần (từ BOM)
- Hiển thị tổng giá thành
- Button "Lắp ráp" → gọi AssemblyBll.AssembleProductAsync()

### Bước 7: Update Form History

Cập nhật các form history để hỗ trợ loại nhập/xuất mới:
- `Inventory/Query/FrmStockInOutMasterHistory.cs`
- `Inventory/Query/FrmStockInOutProductHistory.cs`

Thêm case cho:
- `LoaiNhapXuatKhoEnum.NhapSanPhamLapRap`
- `LoaiNhapXuatKhoEnum.XuatLinhKienLapRap`

### Bước 8: Test

1. Test tạo BOM cho 1 sản phẩm
2. Test lắp ráp sản phẩm:
   - Kiểm tra đủ linh kiện
   - Kiểm tra tạo phiếu xuất/nhập đúng
   - Kiểm tra tính giá thành
   - Kiểm tra lưu AssemblyTransaction

## 📝 Lưu Ý

1. **Transaction:** Tất cả các bước trong `AssembleProductAsync` phải trong 1 transaction để đảm bảo data consistency
2. **Validation:** 
   - Kiểm tra đủ linh kiện trong kho trước khi lắp ráp
   - Kiểm tra BOM đã được định nghĩa
   - Kiểm tra ProductVariant không thể là ComponentVariant của chính nó
3. **Giá thành:** Có thể lấy từ:
   - Giá mua gần nhất (từ StockInOutDetail)
   - Giá trung bình (từ InventoryBalance)
   - Giá chuẩn (từ ProductVariant nếu có)

## 🔗 Files Cần Tạo

- [ ] `Dal/DataAccess/Implementations/Inventory/Assembly/ProductBOMRepository.cs`
- [ ] `Dal/DataAccess/Implementations/Inventory/Assembly/AssemblyTransactionRepository.cs`
- [ ] `Bll/Inventory/Assembly/AssemblyBll.cs`
- [ ] `Inventory/Assembly/FrmProductBOM.cs`
- [ ] `Inventory/Assembly/FrmAssembly.cs`

## 📚 Tham Khảo

- Pattern từ `AssetRepository.cs` cho Repository
- Pattern từ `StockInBll.cs` cho BLL
- Pattern từ `FrmNhapNoiBo.cs` cho Form

