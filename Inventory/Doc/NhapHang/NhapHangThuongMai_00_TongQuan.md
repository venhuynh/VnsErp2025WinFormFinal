# TỔNG QUAN – 3 QUY TRÌNH NHẬP KHO THƯƠNG MẠI CỦA VNS

**Version 1.0** – Dùng cho VNS-ERP 2025 & Cursor AI

---

## 1. Giới thiệu

Tài liệu này mô tả tổng quan ba quy trình nhập kho thương mại được sử dụng trong hệ thống VNS-ERP 2025. Ba quy trình này phản ánh đầy đủ các tình huống nhập kho thực tế của doanh nghiệp thương mại – dịch vụ và được thiết kế để Cursor AI có thể hiểu, phân tích và tự động sinh mã theo đúng ngữ cảnh.

## 2. Danh sách 3 quy trình nhập kho

1. **Nhập kho stock đợi bán** (Stock For Sale)
2. **Nhập kho theo PO Nhà Cung Cấp** (PO NCC)
3. **Nhập kho theo đơn đặt hàng khách** (Customer PO / Sales Order)

## 3. So sánh tổng quan

| Quy trình | Liên quan khách | Liên quan PO NCC | Mục đích | Ghi chú |
|-----------|----------------|------------------|----------|---------|
| 1. Nhập stock đợi bán | ❌ | Tùy chọn | Tăng tồn kho để bán | Không gắn đơn khách |
| 2. Nhập theo PO NCC | ❌ | ✔ Bắt buộc | Nhập theo kế hoạch mua | Quy trình nhập chuẩn |
| 3. Nhập theo PO khách | ✔ Bắt buộc | Tùy chọn | Nhập để đáp ứng đơn hàng | Có liên kết SalesOrder |

## 4. Mối liên hệ 3 quy trình trong luồng vận hành

```
Đơn đặt hàng khách (SO) ──▶ (nếu cần mua) ──▶ PO NCC
                                         ▼
                        NCC giao hàng ──▶ Nhập kho ──▶ Xuất kho cho khách
```

### Giải thích:

- **Quy trình 1** xảy ra độc lập.
- **Quy trình 2** là mua hàng tiêu chuẩn.
- **Quy trình 3** là nhập kho để phục vụ đơn khách — có thể qua PO NCC hoặc nhập trực tiếp.

## 5. Quy trình 1 – Nhập kho stock đợi bán

### Mục tiêu

Nhập hàng về kho trước khi có đơn bán, dùng cho dự trữ, xoay vòng hàng hóa hoặc tận dụng giá tốt.

### Đặc điểm

- Không gắn với khách hàng.
- Có thể có hoặc không có PO với nhà cung cấp.
- Tồn kho nhập vào là **On-hand Inventory**.

### Phương thức nhập kho

- `PhuongThucNhapKhoEnum.NhapTrucTiep`
- `LoaiNhapKhoEnum.ThuongMai`

## 6. Quy trình 2 – Nhập kho theo PO Nhà Cung Cấp (PO NCC)

### Mục tiêu

Quy trình chuẩn của hoạt động mua hàng. Nhập kho dựa trên đơn mua (PO) đã được duyệt.

### Đặc điểm

- Bắt buộc phải có **PO NCC**.
- Khi NCC giao hàng → tạo phiếu nhập theo PO.
- Theo dõi số lượng: đặt – nhận – còn lại.
- Cập nhật trạng thái PO: **Partial** / **Completed**.

### Phương thức nhập kho

- `PhuongThucNhapKhoEnum.NhapTuDonHang`
- `LoaiNhapKhoEnum.ThuongMai`

## 7. Quy trình 3 – Nhập kho theo đơn đặt hàng khách (Customer PO / Sales Order)

### Mục tiêu

Nhập hàng để phục vụ trực tiếp đơn đặt hàng của khách, không nhập dư nếu không có phê duyệt.

### Đặc điểm

- Bắt buộc phải gắn **SalesOrderId**.
- Có thể nhập hàng từ PO NCC hoặc nhập trực tiếp.
- Tồn kho nhập vào có thể là:
  - **Allocated stock** (dành riêng cho đơn khách)
  - Hoặc **on-hand** tùy cấu hình doanh nghiệp.
- Thường dùng trong nghiệp vụ **"Buy-to-Order"**.

### Phương thức nhập kho

- `PhuongThucNhapKhoEnum.NhapTheoDonKhachHang` (đề xuất thêm cho hệ thống)
- `LoaiNhapKhoEnum.ThuongMai`

## 8. Sự liên kết giữa 3 quy trình

### Luồng logic trong thực tế doanh nghiệp

1. **Có đơn khách** → kiểm tra tồn.
2. **Nếu tồn đủ** → xuất trực tiếp.
3. **Nếu tồn thiếu** → hệ thống đề xuất tạo PO NCC.

**Khi hàng về:**

- Nếu là stock đợi bán → nhập dạng **Quy trình 1**.
- Nếu theo PO NCC → nhập **Quy trình 2**.
- Nếu để phục vụ đơn khách → nhập **Quy trình 3**.

## 9. Đề xuất bổ sung Enum (nếu muốn hoàn chỉnh)

```csharp
public enum PhuongThucNhapKhoEnum
{
    NhapTrucTiep = 1,
    NhapTuDonHang = 2,
    NhapTuNhaCungCap = 3,
    NhapTheoDonKhachHang = 4 // Đề xuất mới
}
```

Điều này giúp Cursor AI phân biệt rõ 3 loại quy trình khi sinh mã.

## 10. Kết luận

Ba quy trình nhập kho thương mại trong VNS được chuẩn hóa rõ ràng, liên kết chặt chẽ theo nghiệp vụ thực tế. Việc tách biệt và mô tả chi tiết 3 quy trình giúp Cursor AI hiểu đúng bối cảnh, từ đó tạo ra code chính xác, đúng domain và đạt chuẩn Clean Architecture.

---

*End of document*
