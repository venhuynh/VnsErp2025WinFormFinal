# 📊 Hướng dẫn chạy Seed Data cho BusinessPartner System

## 📋 Tổng quan

Thư mục này chứa các script SQL để tạo dữ liệu mẫu (seed data) cho hệ thống BusinessPartner trong VnsErp2025. Dữ liệu được thiết kế để phù hợp với mối quan hệ entity trong ERD và cung cấp dữ liệu test thực tế.

## 🗂️ Danh sách file Seed Data

| File | Mô tả | Số lượng bản ghi |
|------|-------|------------------|
| `Seed_BusinessPartnerCategory_TestData.sql` | Tạo danh mục đối tác | ~13 danh mục |
| `Seed_BusinessPartner_TestData.sql` | Tạo đối tác chính | 103 đối tác |
| `Seed_BusinessPartnerContact_TestData.sql` | Tạo liên hệ đối tác | ~120 liên hệ |
| `Seed_BusinessPartnerCategoryMapping_TestData.sql` | Tạo mapping đối tác-danh mục | ~150 mapping |
| `Seed_All_BusinessPartner_TestData.sql` | **File tổng hợp** | Tất cả |

## 🚀 Cách chạy Seed Data

### ⚡ Phương pháp 1: Chạy tất cả (Khuyến nghị)

```sql
-- Mở SQL Server Management Studio
-- Kết nối đến database VnsErp2025Final
-- Chạy file tổng hợp
:r "Seed_All_BusinessPartner_TestData.sql"
```

**Ưu điểm:**
- ✅ Chạy tất cả script theo đúng thứ tự
- ✅ Có thống kê và kiểm tra tính toàn vẹn
- ✅ Hiển thị progress và kết quả chi tiết
- ✅ An toàn với transaction rollback

### 🔧 Phương pháp 2: Chạy từng file riêng lẻ

Nếu bạn muốn kiểm soát chi tiết hoặc chỉ cần một phần dữ liệu:

```sql
-- 1. Tạo danh mục đối tác trước
:r "Seed_BusinessPartnerCategory_TestData.sql"

-- 2. Tạo đối tác chính
:r "Seed_BusinessPartner_TestData.sql"

-- 3. Tạo liên hệ đối tác
:r "Seed_BusinessPartnerContact_TestData.sql"

-- 4. Tạo mapping đối tác-danh mục
:r "Seed_BusinessPartnerCategoryMapping_TestData.sql"
```

### 📝 Phương pháp 3: Copy-paste từng script

Nếu `:r` command không hoạt động:

1. Mở từng file `.sql` trong text editor
2. Copy toàn bộ nội dung
3. Paste vào SQL Server Management Studio
4. Chạy từng script theo thứ tự

## 📊 Cấu trúc dữ liệu sẽ được tạo

### 🏷️ BusinessPartnerCategory (Danh mục đối tác)
```
- Khách hàng cá nhân
- Khách hàng doanh nghiệp  
- Nhà cung cấp nguyên vật liệu
- Nhà cung cấp dịch vụ
- Nhà cung cấp thiết bị
- Đại lý phân phối
- Nhà bán lẻ
- Nhà bán buôn
- Đối tác chiến lược
- Khách hàng tiềm năng
- Nhà cung cấp công nghệ
- Đối tác tài chính
- Nhà cung cấp marketing
- Đối tác logistics
- Nhà cung cấp năng lượng
```

### 👥 BusinessPartner (Đối tác)
```
- CUS001: Công ty TNHH ABC (Khách hàng)
- VEN001: Công ty TNHH XYZ (Nhà cung cấp)
- BOTH001: Công ty TNHH Song Phương (Cả hai)
- TEST001-TEST100: 100 đối tác test với dữ liệu đa dạng
```

### 📞 BusinessPartnerContact (Liên hệ)
```
- Mỗi đối tác có 1-3 liên hệ
- Phân biệt liên hệ chính (IsPrimary = 1) và phụ
- Thông tin: Họ tên, chức vụ, điện thoại, email
```

### 🔗 BusinessPartner_BusinessPartnerCategory (Mapping)
```
- PartnerType = 1 → Khách hàng cá nhân/doanh nghiệp
- PartnerType = 2 → Nhà cung cấp (dịch vụ/nguyên vật liệu/thiết bị)
- PartnerType = 3 → Cả khách hàng và nhà cung cấp
```

## ⚠️ Lưu ý quan trọng

### 🔒 Script Idempotent
- Tất cả script đều sử dụng `IF NOT EXISTS` để kiểm tra trước khi insert
- **An toàn chạy nhiều lần** mà không bị trùng lặp dữ liệu
- Có thể chạy lại nếu cần thiết

### 🔄 Transaction Safety
- Mỗi script đều có `BEGIN TRY...CATCH` với transaction
- Tự động rollback nếu có lỗi
- Đảm bảo tính toàn vẹn dữ liệu

### 📍 Thứ tự thực hiện
**QUAN TRỌNG**: Phải chạy theo đúng thứ tự vì có foreign key constraints:
1. **BusinessPartnerCategory** (không phụ thuộc)
2. **BusinessPartner** (không phụ thuộc)
3. **BusinessPartnerContact** (phụ thuộc BusinessPartner)
4. **BusinessPartner_BusinessPartnerCategory** (phụ thuộc cả hai)

## 🛠️ Xử lý sự cố

### ❌ Lỗi thường gặp

#### 1. Lỗi `:r` command không hoạt động
```sql
-- Thay vì dùng :r, copy-paste nội dung file
-- Hoặc sử dụng File → Open → File trong SSMS
```

#### 2. Lỗi foreign key constraint
```
Nguyên nhân: Chạy script không đúng thứ tự
Giải pháp: Chạy lại từ đầu theo đúng thứ tự
```

#### 3. Lỗi connection string
```
Nguyên nhân: Database không tồn tại hoặc không kết nối được
Giải pháp: Kiểm tra connection string và tạo database trước
```

### 🔍 Kiểm tra dữ liệu sau khi chạy

```sql
-- Kiểm tra tổng quan
SELECT 'BusinessPartnerCategory' as TableName, COUNT(*) as Count FROM dbo.BusinessPartnerCategory
UNION ALL
SELECT 'BusinessPartner', COUNT(*) FROM dbo.BusinessPartner
UNION ALL
SELECT 'BusinessPartnerContact', COUNT(*) FROM dbo.BusinessPartnerContact
UNION ALL
SELECT 'BusinessPartner_BusinessPartnerCategory', COUNT(*) FROM dbo.BusinessPartner_BusinessPartnerCategory;

-- Kiểm tra mapping
SELECT 
    p.PartnerCode,
    p.PartnerName,
    c.CategoryName
FROM dbo.BusinessPartner p
JOIN dbo.BusinessPartner_BusinessPartnerCategory m ON m.PartnerId = p.Id
JOIN dbo.BusinessPartnerCategory c ON c.Id = m.CategoryId
ORDER BY p.PartnerCode;
```

## 🧹 Xóa dữ liệu test (nếu cần)

```sql
-- ⚠️ CẢNH BÁO: Chỉ chạy khi muốn xóa toàn bộ dữ liệu test
-- Chạy theo thứ tự ngược để tránh foreign key constraint

DELETE FROM dbo.BusinessPartner_BusinessPartnerCategory;
DELETE FROM dbo.BusinessPartnerContact;
DELETE FROM dbo.BusinessPartner;
DELETE FROM dbo.BusinessPartnerCategory;
```

## 📈 Kết quả mong đợi

Sau khi chạy thành công, bạn sẽ có:

- ✅ **~17 danh mục đối tác** đa dạng
- ✅ **103 đối tác** với thông tin đầy đủ
- ✅ **~120 liên hệ** với phân cấp chính/phụ
- ✅ **~150 mapping** đối tác-danh mục
- ✅ **Dữ liệu thực tế** với tên tiếng Việt
- ✅ **Mối quan hệ hoàn chỉnh** giữa các bảng

## 🎯 Sử dụng dữ liệu test

Sau khi tạo dữ liệu, bạn có thể:

1. **Mở form BusinessPartner** để xem danh sách đối tác
2. **Thêm/sửa/xóa** đối tác để test chức năng
3. **Kiểm tra validation** PartnerCode trùng lặp
4. **Test WaitingForm1** khi load/xóa dữ liệu
5. **Kiểm tra mapping** đối tác-danh mục

## 📞 Hỗ trợ

Nếu gặp vấn đề khi chạy seed data:

1. Kiểm tra log lỗi trong SQL Server Management Studio
2. Đảm bảo database `VnsErp2025Final` đã được tạo
3. Kiểm tra quyền truy cập database
4. Chạy từng script riêng lẻ để xác định script gây lỗi

---

**📅 Cập nhật lần cuối**: 2025  
**👨‍💻 Tạo bởi**: AI Assistant  
**📝 Phiên bản**: 1.0
