# Logic Nghiệp Vụ Module Assembly/Manufacturing

## 📋 Tổng Quan Quy Trình

Module Assembly/Manufacturing hỗ trợ **2 quy trình chính**:

### 🔧 QUY TRÌNH 1: ASSEMBLY (Lắp Ráp) - Sản Xuất Mới
```
1. NHẬP THƯƠNG MẠI (Linh kiện)
   ↓
2. XUẤT LẮP RÁP → NHẬP THÀNH PHẨM (Bộ máy tính)
   ↓
3. XUẤT BÁN (Bộ máy tính cho khách hàng)
```

### 🔨 QUY TRÌNH 2: DISASSEMBLY (Phân Rã) - Tái Chế
```
1. NHẬP THÀNH PHẨM CŨ (Bộ máy tính cũ)
   ↓
2. XUẤT PHÂN RÃ → NHẬP LINH KIỆN (Từ thành phẩm phân rã ra)
   ↓
3. XUẤT BÁN (Linh kiện riêng lẻ cho khách hàng)
```

---

## 🔄 CHI TIẾT 3 BƯỚC NGHIỆP VỤ

### BƯỚC 1: NHẬP THƯƠNG MẠI - Nhập Linh Kiện Từ Nhà Cung Cấp

**Mục đích:**
- Nhập các linh kiện (CPU, RAM, Ổ cứng, Mainboard, Windows...) từ nhà cung cấp vào kho
- Tăng tồn kho linh kiện để phục vụ lắp ráp

**Loại nhập kho:**
- `LoaiNhapXuatKhoEnum.NhapHangThuongMai = 1` (Nhập hàng thương mại)
- `LoaiNhapXuatKhoEnum.NhapHangBaoHanh = 5` (Nhập hàng bảo hành - nếu có)

**Quy trình:**
1. Tạo phiếu nhập kho thương mại
2. Chọn nhà cung cấp
3. Nhập danh sách linh kiện:
   - CPU Intel i5: 20 cái
   - RAM 8GB DDR4: 40 thanh
   - Ổ cứng SSD 256GB: 15 cái
   - Mainboard H310: 20 cái
   - Windows 10 Pro: 20 license
4. Lưu phiếu → Tồn kho linh kiện tăng

**Kết quả:**
- Tồn kho linh kiện trong `InventoryBalance` tăng
- Có thể xem trong "Tồn kho" theo từng linh kiện

**Màn hình sử dụng:**
- `Inventory/StockIn/NhapHangThuongMai` (đã có sẵn trong hệ thống)

---

### BƯỚC 2: XUẤT LẮP RÁP → NHẬP THÀNH PHẨM

**Mục đích:**
- Xuất các linh kiện từ kho để lắp ráp thành bộ máy tính hoàn chỉnh
- Nhập bộ máy tính hoàn chỉnh vào kho

**Loại xuất/nhập:**
- **Xuất:** `LoaiNhapXuatKhoEnum.XuatLinhKienLapRap = 16` (Xuất linh kiện lắp ráp)
- **Nhập:** `LoaiNhapXuatKhoEnum.NhapSanPhamLapRap = 6` (Nhập sản phẩm lắp ráp)

**Quy trình tự động (trong 1 transaction):**

#### 2.1. User thực hiện lắp ráp:
1. Chọn sản phẩm cần lắp ráp (ví dụ: "Máy tính PC-001")
2. Nhập số lượng (ví dụ: 10 bộ)
3. Chọn kho thực hiện lắp ráp
4. Hệ thống tự động:
   - Load BOM của sản phẩm → Hiển thị danh sách linh kiện cần thiết
   - Kiểm tra tồn kho → Validate đủ linh kiện
   - Tính giá thành → Tổng giá linh kiện

#### 2.2. Hệ thống tự động tạo phiếu (trong transaction):

**A. Tạo Phiếu Xuất Linh Kiện:**
```
Số phiếu: PXK-0115-16001 (tự động sinh)
Loại: XuatLinhKienLapRap (16)
Ngày: 2025-01-15
Kho: Kho chính
Chi tiết:
  - CPU Intel i5: 10 cái (xuất)
  - RAM 8GB DDR4: 20 thanh (xuất)
  - Ổ cứng SSD 256GB: 10 cái (xuất)
  - Mainboard H310: 10 cái (xuất)
  - Windows 10 Pro: 10 license (xuất)
Tổng: 60 linh kiện
```

**B. Tạo Phiếu Nhập Sản Phẩm:**
```
Số phiếu: PNK-0115-06001 (tự động sinh)
Loại: NhapSanPhamLapRap (6)
Ngày: 2025-01-15
Kho: Kho chính
Chi tiết:
  - Máy tính PC-001: 10 bộ (nhập)
  - Giá thành: 15,000,000 VNĐ (tổng giá linh kiện)
  - Giá đơn vị: 1,500,000 VNĐ/bộ
Tổng: 10 bộ
```

**C. Lưu AssemblyTransaction:**
```
- AssemblyDate: 2025-01-15
- ProductVariantId: PC-001
- Quantity: 10
- StockOutMasterId: [ID phiếu xuất linh kiện]
- StockInMasterId: [ID phiếu nhập sản phẩm]
- TotalCost: 15,000,000
- UnitCost: 1,500,000
```

**Kết quả:**
- ✅ Tồn kho linh kiện giảm (đã xuất)
- ✅ Tồn kho sản phẩm hoàn chỉnh tăng (đã nhập)
- ✅ Có lịch sử lắp ráp trong `AssemblyTransaction`
- ✅ Có thể trace ngược: Từ sản phẩm → Xem linh kiện đã dùng

**Màn hình sử dụng:**
- `Inventory/Assembly/AssemblyProduct` (cần tạo mới)

---

### BƯỚC 3: XUẤT BÁN - Xuất Bộ Máy Tính Cho Khách Hàng

**Mục đích:**
- Xuất bộ máy tính hoàn chỉnh từ kho để bán cho khách hàng
- Ghi nhận doanh thu, giảm tồn kho thành phẩm

**Loại xuất kho:**
- `LoaiNhapXuatKhoEnum.XuatHangThuongMai = 11` (Xuất hàng thương mại)

**Quy trình:**
1. Tạo phiếu xuất kho thương mại
2. Chọn khách hàng (BusinessPartner)
3. Chọn đơn bán hàng (SalesOrder) - nếu có
4. Nhập danh sách sản phẩm:
   - Máy tính PC-001: 5 bộ
   - Giá bán: 2,000,000 VNĐ/bộ
5. Lưu phiếu → Tồn kho thành phẩm giảm

**Kết quả:**
- Tồn kho thành phẩm trong `InventoryBalance` giảm
- Có thể liên kết với Sales Order
- Ghi nhận doanh thu

**Màn hình sử dụng:**
- `Inventory/StockOut/XuatHangThuongMai` (đã có sẵn trong hệ thống)

---

## 📊 SƠ ĐỒ LUỒNG DỮ LIỆU

```
┌─────────────────────────────────────────────────────────────┐
│  BƯỚC 1: NHẬP THƯƠNG MẠI                                     │
│  ┌───────────────────────────────────────────────────────┐  │
│  │ Nhà cung cấp → Nhập linh kiện vào kho                 │  │
│  │ Loại: NhapHangThuongMai (1)                           │  │
│  │                                                         │  │
│  │ Kết quả: Tồn kho linh kiện ↑                           │  │
│  │ - CPU: 20 cái                                          │  │
│  │ - RAM: 40 thanh                                        │  │
│  │ - Ổ cứng: 15 cái                                      │  │
│  │ - Mainboard: 20 cái                                    │  │
│  │ - Windows: 20 license                                 │  │
│  └───────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────┘
                          ↓
┌─────────────────────────────────────────────────────────────┐
│  BƯỚC 2: LẮP RÁP (Tự động trong 1 transaction)              │
│  ┌───────────────────────────────────────────────────────┐  │
│  │ A. XUẤT LINH KIỆN                                     │  │
│  │    Loại: XuatLinhKienLapRap (16)                      │  │
│  │    - CPU: 10 cái (xuất)                               │  │
│  │    - RAM: 20 thanh (xuất)                             │  │
│  │    - Ổ cứng: 10 cái (xuất)                            │  │
│  │    - Mainboard: 10 cái (xuất)                         │  │
│  │    - Windows: 10 license (xuất)                        │  │
│  │    → Tồn kho linh kiện ↓                               │  │
│  │                                                         │  │
│  │ B. NHẬP THÀNH PHẨM                                     │  │
│  │    Loại: NhapSanPhamLapRap (6)                        │  │
│  │    - Máy tính PC-001: 10 bộ (nhập)                    │  │
│  │    - Giá thành: 15,000,000 VNĐ                        │  │
│  │    → Tồn kho thành phẩm ↑                              │  │
│  │                                                         │  │
│  │ C. GHI NHẬN                                            │  │
│  │    AssemblyTransaction:                                │  │
│  │    - Liên kết phiếu xuất + phiếu nhập                 │  │
│  │    - Lưu giá thành, số lượng                           │  │
│  └───────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────┘
                          ↓
┌─────────────────────────────────────────────────────────────┐
│  BƯỚC 3: XUẤT BÁN                                           │
│  ┌───────────────────────────────────────────────────────┐  │
│  │ Khách hàng ← Xuất bộ máy tính từ kho                  │  │
│  │ Loại: XuatHangThuongMai (11)                          │  │
│  │                                                         │  │
│  │ - Máy tính PC-001: 5 bộ (xuất)                        │  │
│  │ - Giá bán: 2,000,000 VNĐ/bộ                           │  │
│  │                                                         │  │
│  │ Kết quả: Tồn kho thành phẩm ↓                          │  │
│  │ - Còn lại: 5 bộ                                        │  │
│  └───────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────┘
```

---

## 🔍 CHI TIẾT LOGIC XỬ LÝ

### 1. Tính Giá Thành Sản Phẩm Lắp Ráp

**Công thức:**
```
Giá thành = Tổng (Số lượng linh kiện × Đơn giá linh kiện)

Ví dụ:
- CPU: 1 × 5,000,000 = 5,000,000
- RAM: 2 × 500,000 = 1,000,000
- Ổ cứng: 1 × 3,000,000 = 3,000,000
- Mainboard: 1 × 2,000,000 = 2,000,000
- Windows: 1 × 1,000,000 = 1,000,000
─────────────────────────────────────
Tổng: 12,000,000 VNĐ/bộ
```

**Nguồn đơn giá linh kiện:**
1. **Ưu tiên 1:** Lấy từ `InventoryBalance.ClosingValue / ClosingBalance` (giá trung bình)
2. **Ưu tiên 2:** Lấy từ `StockInOutDetail` gần nhất có `StockInQty > 0` và `UnitPrice > 0`
3. **Nếu không có:** Trả về 0 (có thể cần nhập giá thủ công)

### 2. Kiểm Tra Tồn Kho Linh Kiện

**Logic:**
```csharp
foreach (linh kiện trong BOM) {
    Số lượng cần = BOM.Quantity × Số lượng lắp ráp
    Tồn kho = InventoryBalance.GetByPeriod(warehouseId, componentId, year, month).ClosingBalance
    
    if (Tồn kho < Số lượng cần) {
        throw "Không đủ linh kiện"
    }
}
```

**Ví dụ:**
- Cần lắp 10 bộ máy tính
- Mỗi bộ cần 2 thanh RAM
- → Cần 20 thanh RAM
- Tồn kho hiện tại: 25 thanh → ✅ Đủ
- Nếu tồn kho: 15 thanh → ❌ Thiếu 5 thanh

### 3. Tự Động Tạo Phiếu Xuất/Nhập

**Trong 1 transaction (đảm bảo tính nhất quán):**

```csharp
BeginTransaction();
try {
    // 1. Tạo phiếu xuất linh kiện
    StockOutMaster outMaster = new {
        StockInOutType = XuatLinhKienLapRap (16),
        VocherNumber = "PXK-0115-16001",
        WarehouseId = warehouseId,
        ...
    };
    StockOutDetails = [danh sách linh kiện với StockOutQty]
    Save(outMaster, StockOutDetails);
    
    // 2. Tạo phiếu nhập sản phẩm
    StockInMaster inMaster = new {
        StockInOutType = NhapSanPhamLapRap (6),
        VocherNumber = "PNK-0115-06001",
        WarehouseId = warehouseId,
        TotalAmount = totalCost,
        ...
    };
    StockInDetails = [sản phẩm với StockInQty, UnitPrice = unitCost]
    Save(inMaster, StockInDetails);
    
    // 3. Lưu AssemblyTransaction
    AssemblyTransaction = new {
        StockOutMasterId = outMaster.Id,
        StockInMasterId = inMaster.Id,
        TotalCost = totalCost,
        ...
    };
    Save(AssemblyTransaction);
    
    Commit();
} catch {
    Rollback();
}
```

**Lợi ích:**
- Đảm bảo tính nhất quán: Hoặc tất cả thành công, hoặc tất cả rollback
- Không bị mất dữ liệu: Không có trường hợp xuất linh kiện nhưng không nhập sản phẩm

---

## 📋 MAPPING VỚI ENUM HỆ THỐNG

| Bước | Loại Nhập/Xuất | Enum Value | Mô tả |
|------|----------------|------------|-------|
| **Bước 1** | Nhập thương mại | `NhapHangThuongMai = 1` | Nhập linh kiện từ NCC |
| **Bước 1** | Nhập bảo hành | `NhapHangBaoHanh = 5` | Nhập linh kiện bảo hành (nếu có) |
| **Bước 2** | Xuất lắp ráp | `XuatLinhKienLapRap = 16` | Xuất linh kiện để lắp ráp |
| **Bước 2** | Nhập thành phẩm | `NhapSanPhamLapRap = 6` | Nhập sản phẩm lắp ráp |
| **Bước 3** | Xuất bán | `XuatHangThuongMai = 11` | Xuất thành phẩm bán cho khách |

---

## ✅ VALIDATION & BUSINESS RULES

### Bước 1: Nhập Thương Mại
- ✅ Nhà cung cấp phải tồn tại
- ✅ Linh kiện phải tồn tại trong ProductVariant
- ✅ Số lượng > 0
- ✅ Đơn giá >= 0

### Bước 2: Lắp Ráp
- ✅ Sản phẩm phải có BOM (ít nhất 1 linh kiện)
- ✅ Tất cả linh kiện phải đủ tồn kho
- ✅ Số lượng lắp ráp > 0
- ✅ Kho phải tồn tại và đang hoạt động
- ✅ Giá thành được tính tự động từ giá linh kiện

### Bước 3: Xuất Bán
- ✅ Khách hàng phải tồn tại (nếu có)
- ✅ Sản phẩm phải đủ tồn kho
- ✅ Số lượng xuất > 0
- ✅ Giá bán >= 0

---

## 🔗 LIÊN KẾT DỮ LIỆU

### Trace Ngược (Từ Sản Phẩm → Linh Kiện)
```
Sản phẩm PC-001 (AssemblyTransaction)
  ↓
Phiếu nhập: PNK-0115-06001 (NhapSanPhamLapRap)
  ↓
Phiếu xuất: PXK-0115-16001 (XuatLinhKienLapRap)
  ↓
Chi tiết linh kiện:
  - CPU Intel i5: 10 cái
  - RAM 8GB: 20 thanh
  - ...
```

### Trace Xuôi (Từ Linh Kiện → Sản Phẩm)
```
Linh kiện CPU Intel i5
  ↓
Phiếu xuất: PXK-0115-16001 (XuatLinhKienLapRap)
  ↓
AssemblyTransaction
  ↓
Sản phẩm: PC-001 (10 bộ)
```

---

## 📝 GHI CHÚ QUAN TRỌNG

1. **Tất cả thao tác lắp ráp đều được ghi nhận:**
   - Trong `AssemblyTransaction` (lịch sử lắp ráp)
   - Trong `StockInOutMaster/Detail` (phiếu xuất/nhập)
   - Trong `InventoryBalance` (tồn kho)

2. **Giá thành được tính tự động:**
   - Không cần nhập thủ công
   - Dựa trên giá linh kiện trong kho
   - Có thể xem chi tiết giá từng linh kiện

3. **Đảm bảo tính nhất quán:**
   - Tất cả trong 1 transaction
   - Không có trường hợp xuất linh kiện nhưng không nhập sản phẩm

4. **Có thể mở rộng:**
   - Lắp ráp theo lô (nhiều sản phẩm cùng lúc)
   - Lắp ráp từ nhiều kho
   - Tính chi phí nhân công, chi phí khác

---

---

## 🔨 QUY TRÌNH 2: DISASSEMBLY (PHÂN RÃ)

### BƯỚC 1: NHẬP THÀNH PHẨM CŨ

**Mục đích:**
- Nhập nguyên bộ máy tính/máy chủ cũ từ khách hàng hoặc nhà cung cấp
- Có thể là hàng mua lại, hàng bảo hành trả về, hàng thanh lý...

**Loại nhập kho:**
- `LoaiNhapXuatKhoEnum.NhapHangThuongMai = 1` (Nhập hàng thương mại)
- Hoặc các loại nhập khác tùy theo nguồn gốc

**Quy trình:**
1. Tạo phiếu nhập kho thương mại
2. Chọn nhà cung cấp/khách hàng (nếu có)
3. Nhập thành phẩm:
   - Máy tính PC cũ: 5 bộ
   - Máy chủ Server cũ: 2 bộ
4. Lưu phiếu → Tồn kho thành phẩm tăng

**Kết quả:**
- Tồn kho thành phẩm trong `InventoryBalance` tăng
- Sẵn sàng để phân rã

**Màn hình sử dụng:**
- `Inventory/StockIn/NhapHangThuongMai` (đã có sẵn)

---

### BƯỚC 2: XUẤT PHÂN RÃ → NHẬP LINH KIỆN

**Mục đích:**
- Xuất thành phẩm (bộ máy tính cũ) từ kho
- Phân rã ra thành từng linh kiện
- Nhập các linh kiện vào kho

**Loại xuất/nhập:**
- **Xuất:** `LoaiNhapXuatKhoEnum.XuatThanhPhamPhanRa = 17` (Xuất thành phẩm phân rã)
- **Nhập:** `LoaiNhapXuatKhoEnum.NhapLinhKienPhanRa = 7` (Nhập linh kiện phân rã)

**Quy trình tự động (tương tự Assembly nhưng ngược lại):**

1. User chọn thành phẩm cần phân rã
2. Nhập số lượng
3. Hệ thống tự động:
   - Load BOM của thành phẩm → Hiển thị danh sách linh kiện sẽ thu được
   - Kiểm tra tồn kho thành phẩm → Validate đủ thành phẩm
   - Tính giá trị linh kiện → Dựa trên giá thành phẩm hoặc giá thị trường

4. **Trong 1 transaction:**
   - Tạo phiếu xuất thành phẩm (`XuatThanhPhamPhanRa`)
   - Tạo phiếu nhập linh kiện (`NhapLinhKienPhanRa`)
   - Lưu `DisassemblyTransaction` (cần tạo bảng mới)

**Kết quả:**
- ✅ Tồn kho thành phẩm giảm (đã xuất)
- ✅ Tồn kho linh kiện tăng (đã nhập)
- ✅ Có lịch sử phân rã

---

### BƯỚC 3: XUẤT BÁN LINH KIỆN

**Mục đích:**
- Xuất từng linh kiện riêng lẻ từ kho để bán cho khách hàng

**Loại xuất kho:**
- `LoaiNhapXuatKhoEnum.XuatHangThuongMai = 11` (Xuất hàng thương mại)

**Quy trình:**
1. Tạo phiếu xuất kho thương mại
2. Chọn khách hàng
3. Nhập danh sách linh kiện:
   - CPU Intel i5: 3 cái
   - RAM 8GB DDR4: 5 thanh
   - Ổ cứng SSD 256GB: 2 cái
4. Lưu phiếu → Tồn kho linh kiện giảm

**Màn hình sử dụng:**
- `Inventory/StockOut/XuatHangThuongMai` (đã có sẵn)

---

## 🔄 SO SÁNH ASSEMBLY vs DISASSEMBLY

| Tiêu chí | Assembly (Lắp ráp) | Disassembly (Phân rã) |
|----------|---------------------|----------------------|
| **Input** | Linh kiện | Thành phẩm |
| **Output** | Thành phẩm | Linh kiện |
| **Xuất** | `XuatLinhKienLapRap (16)` | `XuatThanhPhamPhanRa (17)` |
| **Nhập** | `NhapSanPhamLapRap (6)` | `NhapLinhKienPhanRa (7)` |
| **BOM** | ProductVariantId → ComponentVariantId | ProductVariantId → ComponentVariantId (dùng chung) |
| **Giá thành** | Tổng giá linh kiện | Giá thành phẩm / Số linh kiện |
| **Mục đích** | Sản xuất mới | Tái chế, bán linh kiện |

---

## 🎯 KẾT LUẬN

Hệ thống hỗ trợ **2 quy trình đầy đủ**:

### ✅ QUY TRÌNH 1: ASSEMBLY (Lắp Ráp)
- Bước 1: Nhập thương mại → Tồn kho linh kiện tăng
- Bước 2: Xuất lắp ráp → Nhập thành phẩm → Tồn kho linh kiện giảm, thành phẩm tăng
- Bước 3: Xuất bán → Tồn kho thành phẩm giảm

### ✅ QUY TRÌNH 2: DISASSEMBLY (Phân Rã) - **CẦN IMPLEMENT**
- Bước 1: Nhập thành phẩm cũ → Tồn kho thành phẩm tăng
- Bước 2: Xuất phân rã → Nhập linh kiện → Tồn kho thành phẩm giảm, linh kiện tăng
- Bước 3: Xuất bán linh kiện → Tồn kho linh kiện giảm

**Chu trình hoàn chỉnh:**
```
Nhập linh kiện → Lắp ráp → Thành phẩm → Bán thành phẩm
                                    ↓
Nhập thành phẩm cũ → Phân rã → Linh kiện → Bán linh kiện
```

