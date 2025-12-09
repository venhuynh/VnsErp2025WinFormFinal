# Migration: Xóa các cột Liên hệ, Ngân hàng, Thanh toán khỏi BusinessPartner

## Mô tả

Migration script này xóa các cột liên hệ, ngân hàng, và thanh toán khỏi bảng `BusinessPartner` vì các thông tin này sẽ được tách ra làm bảng riêng.

## Các cột sẽ bị XÓA

| Tên cột | Kiểu dữ liệu | Mô tả |
|---------|--------------|-------|
| `ContactPerson` | `NVarChar(100)` | Người liên hệ |
| `ContactPosition` | `NVarChar(100)` | Chức vụ người liên hệ |
| `BankAccount` | `NVarChar(50)` | Số tài khoản ngân hàng |
| `BankName` | `NVarChar(100)` | Tên ngân hàng |
| `CreditLimit` | `Decimal(18,2)` | Hạn mức tín dụng |
| `PaymentTerm` | `NVarChar(50)` | Điều khoản thanh toán |

## ⚠️ CẢNH BÁO QUAN TRỌNG

**DỮ LIỆU SẼ BỊ MẤT VĨNH VIỄN!**

- Script này sẽ **XÓA VĨNH VIỄN** các cột và dữ liệu trong các cột đó
- **BẮT BUỘC** phải backup database trước khi chạy
- Nếu có constraint liên quan (như CHECK constraint cho CreditLimit), script sẽ tự động xóa constraint trước khi xóa cột

## Cách chạy

1. **BACKUP DATABASE** trước khi chạy migration
2. Mở SQL Server Management Studio
3. Kết nối đến database `VnsErp2025Final`
4. Mở file `Migration_BusinessPartner_RemoveContactBankFields.sql`
5. Review script và đảm bảo đã backup
6. Execute script

## Rollback

**KHÔNG THỂ ROLLBACK TỰ ĐỘNG!**

Nếu cần rollback, bạn phải:
1. Restore từ backup
2. Hoặc tạo lại các cột và import dữ liệu từ backup

## Lưu ý

- Script sẽ tự động xóa các constraint liên quan (như CHECK constraint cho CreditLimit)
- Script có thể chạy lại nhiều lần (idempotent) - sẽ bỏ qua nếu cột đã không tồn tại
- Sau khi chạy migration, cần:
  1. Refresh DataContext trong Visual Studio
  2. Cập nhật Entity trong `VnsErp2025.designer.cs` (hoặc regenerate từ DBML)
  3. Cập nhật DTO để bỏ các properties đã xóa
  4. Cập nhật Repository và BLL để bỏ các fields đã xóa

## Tham khảo

- Các thông tin liên hệ sẽ được tách ra bảng `BusinessPartnerContact` (đã có sẵn)
- Các thông tin ngân hàng và thanh toán sẽ được tách ra bảng riêng (cần tạo mới)

