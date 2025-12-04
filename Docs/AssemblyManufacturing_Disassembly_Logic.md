# Logic Nghiệp Vụ Disassembly (Phân Rã) - Module Assembly/Manufacturing

## 📋 Tổng Quan

Nghiệp vụ **Disassembly (Phân rã)** là quy trình ngược lại với **Assembly (Lắp ráp)**:

- **Assembly:** Linh kiện → Thành phẩm (Bộ máy tính)
- **Disassembly:** Thành phẩm (Bộ máy tính cũ) → Linh kiện

---

## 🔄 QUY TRÌNH DISASSEMBLY

### BƯỚC 1: NHẬP THÀNH PHẨM CŨ

**Mục đích:**
- Nhập nguyên bộ máy tính/máy chủ cũ từ khách hàng hoặc nhà cung cấp
- Có thể là hàng mua lại, hàng bảo hành trả về, hàng thanh lý...

**Loại nhập kho:**
- `LoaiNhapXuatKhoEnum.NhapHangThuongMai = 1` (Nhập hàng thương mại)
- Hoặc các loại nhập khác tùy theo nguồn gốc

**Quy trình:**
1. Tạo phiếu nhập kho
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

### BƯỚC 2: PHÂN RÃ THÀNH PHẨM → LINH KIỆN

**Mục đích:**
- Xuất thành phẩm (bộ máy tính cũ) từ kho
- Phân rã ra thành từng linh kiện
- Nhập các linh kiện vào kho

**Loại xuất/nhập:**
- **Xuất:** `LoaiNhapXuatKhoEnum.XuatThanhPhamPhanRa = 17` (Xuất thành phẩm phân rã)
- **Nhập:** `LoaiNhapXuatKhoEnum.NhapLinhKienPhanRa = 7` (Nhập linh kiện phân rã)

**Quy trình tự động (trong 1 transaction):**

#### 2.1. User thực hiện phân rã:
1. Chọn thành phẩm cần phân rã (ví dụ: "Máy tính PC-001")
2. Nhập số lượng (ví dụ: 5 bộ)
3. Chọn kho thực hiện phân rã
4. Hệ thống tự động:
   - Load BOM của thành phẩm → Hiển thị danh sách linh kiện sẽ thu được
   - Kiểm tra tồn kho thành phẩm → Validate đủ thành phẩm
   - Tính giá trị linh kiện → Dựa trên giá thành phẩm hoặc giá thị trường

#### 2.2. Hệ thống tự động tạo phiếu (trong transaction):

**A. Tạo Phiếu Xuất Thành Phẩm:**
```
Số phiếu: PXK-0115-17001 (tự động sinh)
Loại: XuatThanhPhamPhanRa (17)
Ngày: 2025-01-15
Kho: Kho chính
Chi tiết:
  - Máy tính PC-001 cũ: 5 bộ (xuất)
  - Giá mua: 3,000,000 VNĐ/bộ
Tổng: 5 bộ
```

**B. Tạo Phiếu Nhập Linh Kiện:**
```
Số phiếu: PNK-0115-07001 (tự động sinh)
Loại: NhapLinhKienPhanRa (7)
Ngày: 2025-01-15
Kho: Kho chính
Chi tiết:
  - CPU Intel i5: 5 cái (nhập)
  - RAM 8GB DDR4: 10 thanh (nhập)
  - Ổ cứng SSD 256GB: 5 cái (nhập)
  - Mainboard H310: 5 cái (nhập)
  - Windows 10 Pro: 5 license (nhập)
Tổng: 30 linh kiện
```

**C. Lưu DisassemblyTransaction:**
```
- DisassemblyDate: 2025-01-15
- ProductVariantId: PC-001 (thành phẩm)
- Quantity: 5
- StockOutMasterId: [ID phiếu xuất thành phẩm]
- StockInMasterId: [ID phiếu nhập linh kiện]
- TotalValue: [Tổng giá trị linh kiện thu được]
- UnitValue: [Giá trị trung bình 1 linh kiện]
```

**Kết quả:**
- ✅ Tồn kho thành phẩm giảm (đã xuất)
- ✅ Tồn kho linh kiện tăng (đã nhập)
- ✅ Có lịch sử phân rã trong `DisassemblyTransaction`
- ✅ Có thể trace ngược: Từ linh kiện → Xem thành phẩm đã phân rã

---

### BƯỚC 3: XUẤT BÁN LINH KIỆN

**Mục đích:**
- Xuất từng linh kiện riêng lẻ từ kho để bán cho khách hàng
- Ghi nhận doanh thu, giảm tồn kho linh kiện

**Loại xuất kho:**
- `LoaiNhapXuatKhoEnum.XuatHangThuongMai = 11` (Xuất hàng thương mại)

**Quy trình:**
1. Tạo phiếu xuất kho thương mại
2. Chọn khách hàng (BusinessPartner)
3. Chọn đơn bán hàng (SalesOrder) - nếu có
4. Nhập danh sách linh kiện:
   - CPU Intel i5: 3 cái
   - RAM 8GB DDR4: 5 thanh
   - Ổ cứng SSD 256GB: 2 cái
5. Lưu phiếu → Tồn kho linh kiện giảm

**Kết quả:**
- Tồn kho linh kiện trong `InventoryBalance` giảm
- Có thể liên kết với Sales Order
- Ghi nhận doanh thu

**Màn hình sử dụng:**
- `Inventory/StockOut/XuatHangThuongMai` (đã có sẵn)

---

## 📊 SƠ ĐỒ LUỒNG DỮ LIỆU DISASSEMBLY

```
┌─────────────────────────────────────────────────────────────┐
│  BƯỚC 1: NHẬP THÀNH PHẨM CŨ                                │
│  ┌───────────────────────────────────────────────────────┐  │
│  │ Khách hàng/NCC → Nhập bộ máy tính cũ vào kho          │  │
│  │ Loại: NhapHangThuongMai (1)                           │  │
│  │                                                         │  │
│  │ Kết quả: Tồn kho thành phẩm ↑                           │  │
│  │ - Máy tính PC cũ: 5 bộ                                 │  │
│  │ - Máy chủ Server cũ: 2 bộ                             │  │
│  └───────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────┘
                          ↓
┌─────────────────────────────────────────────────────────────┐
│  BƯỚC 2: PHÂN RÃ (Tự động trong 1 transaction)              │
│  ┌───────────────────────────────────────────────────────┐  │
│  │ A. XUẤT THÀNH PHẨM                                     │  │
│  │    Loại: XuatThanhPhamPhanRa (17)                      │  │
│  │    - Máy tính PC cũ: 5 bộ (xuất)                      │  │
│  │    → Tồn kho thành phẩm ↓                              │  │
│  │                                                         │  │
│  │ B. NHẬP LINH KIỆN                                      │  │
│  │    Loại: NhapLinhKienPhanRa (7)                       │  │
│  │    - CPU Intel i5: 5 cái (nhập)                       │  │
│  │    - RAM 8GB DDR4: 10 thanh (nhập)                    │  │
│  │    - Ổ cứng SSD 256GB: 5 cái (nhập)                   │  │
│  │    - Mainboard H310: 5 cái (nhập)                     │  │
│  │    - Windows 10 Pro: 5 license (nhập)                  │  │
│  │    → Tồn kho linh kiện ↑                                │  │
│  │                                                         │  │
│  │ C. GHI NHẬN                                            │  │
│  │    DisassemblyTransaction:                             │  │
│  │    - Liên kết phiếu xuất + phiếu nhập                 │  │
│  │    - Lưu giá trị, số lượng                             │  │
│  └───────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────┘
                          ↓
┌─────────────────────────────────────────────────────────────┐
│  BƯỚC 3: XUẤT BÁN LINH KIỆN                                  │
│  ┌───────────────────────────────────────────────────────┐  │
│  │ Khách hàng ← Xuất từng linh kiện từ kho               │  │
│  │ Loại: XuatHangThuongMai (11)                          │  │
│  │                                                         │  │
│  │ - CPU Intel i5: 3 cái (xuất)                          │  │
│  │ - RAM 8GB DDR4: 5 thanh (xuất)                         │  │
│  │ - Ổ cứng SSD 256GB: 2 cái (xuất)                       │  │
│  │                                                         │  │
│  │ Kết quả: Tồn kho linh kiện ↓                           │  │
│  │ - CPU còn lại: 2 cái                                   │  │
│  │ - RAM còn lại: 5 thanh                                 │  │
│  │ - Ổ cứng còn lại: 3 cái                                │  │
│  └───────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────┘
```

---

## 🔍 CHI TIẾT LOGIC XỬ LÝ DISASSEMBLY

### 1. Tính Giá Trị Linh Kiện Thu Được

**Công thức:**
```
Giá trị linh kiện = Giá thành phẩm / Số lượng linh kiện (theo BOM)

Hoặc:
Giá trị linh kiện = Giá thị trường linh kiện (nếu có)

Ví dụ:
- Giá mua thành phẩm: 3,000,000 VNĐ/bộ
- BOM: 1 bộ = 5 linh kiện
- → Giá trị trung bình 1 linh kiện: 600,000 VNĐ
```

**Nguồn giá trị:**
1. **Ưu tiên 1:** Lấy từ giá mua thành phẩm / Số lượng linh kiện trong BOM
2. **Ưu tiên 2:** Lấy giá thị trường linh kiện (nếu có cấu hình)
3. **Nếu không có:** Cho phép nhập giá thủ công

### 2. Kiểm Tra Tồn Kho Thành Phẩm

**Logic:**
```csharp
Số lượng cần phân rã = quantity
Tồn kho thành phẩm = InventoryBalance.GetByPeriod(warehouseId, productVariantId, year, month).ClosingBalance

if (Tồn kho < Số lượng cần) {
    throw "Không đủ thành phẩm để phân rã"
}
```

### 3. Sử Dụng BOM Ngược

**Logic:**
- Load BOM từ `ProductVariantId` (thành phẩm)
- Lấy danh sách `ComponentVariantId` (linh kiện)
- Số lượng linh kiện thu được = `BOM.Quantity × Số lượng thành phẩm phân rã`

**Ví dụ:**
- Phân rã 5 bộ máy tính PC-001
- BOM: 1 bộ = 1 CPU + 2 RAM + 1 Ổ cứng + 1 Mainboard + 1 Windows
- → Thu được:
  - CPU: 5 × 1 = 5 cái
  - RAM: 5 × 2 = 10 thanh
  - Ổ cứng: 5 × 1 = 5 cái
  - Mainboard: 5 × 1 = 5 cái
  - Windows: 5 × 1 = 5 license

---

## 📋 MAPPING VỚI ENUM HỆ THỐNG

| Bước | Loại Nhập/Xuất | Enum Value | Mô tả |
|------|----------------|------------|-------|
| **Bước 1** | Nhập thành phẩm cũ | `NhapHangThuongMai = 1` | Nhập bộ máy tính cũ |
| **Bước 2** | Xuất phân rã | `XuatThanhPhamPhanRa = 17` | Xuất thành phẩm để phân rã |
| **Bước 2** | Nhập linh kiện phân rã | `NhapLinhKienPhanRa = 7` | Nhập linh kiện từ phân rã |
| **Bước 3** | Xuất bán linh kiện | `XuatHangThuongMai = 11` | Xuất linh kiện bán cho khách |

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

## ✅ VALIDATION & BUSINESS RULES

### Disassembly Process:
1. ✅ Thành phẩm phải có BOM (để biết phân rã ra linh kiện gì)
2. ✅ Thành phẩm phải đủ tồn kho
3. ✅ Số lượng phân rã > 0
4. ✅ Kho phải tồn tại và đang hoạt động
5. ✅ Giá trị linh kiện được tính tự động hoặc cho phép nhập thủ công

---

## 🎯 KẾT LUẬN

Nghiệp vụ Disassembly bổ sung cho Assembly, tạo thành chu trình hoàn chỉnh:

```
Nhập linh kiện → Lắp ráp → Thành phẩm → Bán thành phẩm
                                    ↓
Nhập thành phẩm cũ → Phân rã → Linh kiện → Bán linh kiện
```

Cả 2 quy trình đều sử dụng chung BOM, đảm bảo tính nhất quán dữ liệu.

