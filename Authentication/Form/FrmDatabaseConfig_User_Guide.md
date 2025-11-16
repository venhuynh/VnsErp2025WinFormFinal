# Hướng dẫn sử dụng - Cài đặt máy chủ cơ sở dữ liệu

## 1. Giới thiệu

### Chức năng của form

Form **Cài đặt máy chủ cơ sở dữ liệu** cho phép bạn cấu hình thông tin kết nối đến cơ sở dữ liệu SQL Server của hệ thống VNS ERP 2025. Form này giúp bạn:

- ✅ Nhập thông tin kết nối cơ sở dữ liệu (server, database, username, password)
- ✅ Kiểm tra kết nối trước khi lưu cấu hình
- ✅ Lưu cấu hình để sử dụng cho các lần sau
- ✅ Tự động tải lại cấu hình đã lưu khi mở form

### Mục đích sử dụng

Form này thường được sử dụng khi:
- 🆕 Lần đầu cài đặt hệ thống
- 🔄 Thay đổi máy chủ cơ sở dữ liệu
- 🔧 Khắc phục sự cố kết nối
- 📦 Di chuyển hệ thống sang máy chủ mới

### Tóm tắt workflow

```
1. Mở form → Tự động tải cấu hình hiện tại (nếu có)
2. Nhập thông tin kết nối:
   - IP/Tên máy chủ
   - Tên cơ sở dữ liệu
   - Tên đăng nhập
   - Mật khẩu
3. Nhấn "Cập nhật" → Hệ thống kiểm tra kết nối
4. Nếu thành công → Lưu cấu hình và đóng form
5. Nếu thất bại → Hiển thị lỗi, giữ form mở để sửa
```

---

## 2. Hướng dẫn sử dụng form

### Bước 1: Mở form

Form sẽ tự động hiển thị khi:
- Lần đầu khởi động ứng dụng (nếu chưa có cấu hình)
- Người dùng chọn chức năng "Cài đặt cơ sở dữ liệu" từ menu

**Giao diện form:**
- Tiêu đề: **"CÀI ĐẶT MÁY CHỦ CƠ SỞ DỮ LIỆU"**
- Form hiển thị ở giữa màn hình
- Form luôn ở trên cùng (TopMost)

### Bước 2: Nhập thông tin kết nối

Form có 4 trường thông tin cần nhập:

#### 2.1. IP/Tên máy chủ (Server Name)

**Vị trí:** Ô nhập đầu tiên  
**Mô tả:** Nhập địa chỉ IP hoặc tên máy chủ SQL Server

**Ví dụ:**
- `localhost` - Nếu SQL Server trên cùng máy
- `192.168.1.100` - Địa chỉ IP máy chủ
- `SQLSERVER01` - Tên máy chủ trong mạng
- `SQLSERVER01\SQLEXPRESS` - Tên instance SQL Server Express

**Lưu ý:**
- ⚠️ Không được để trống
- ✅ Có thể nhập IP hoặc tên máy chủ
- ✅ Nếu dùng named instance, nhập theo định dạng: `TênMáyChủ\TênInstance`

#### 2.2. Tên CSDL (Database Name)

**Vị trí:** Ô nhập thứ hai  
**Mô tả:** Nhập tên cơ sở dữ liệu cần kết nối

**Ví dụ:**
- `VnsErp2025`
- `VnsErp2025Final`
- `VNS_ERP_DB`

**Lưu ý:**
- ⚠️ Không được để trống
- ✅ Tên database phải tồn tại trên SQL Server
- ✅ Phân biệt chữ hoa/thường (tùy cấu hình SQL Server)

#### 2.3. Tên đăng nhập (User Id)

**Vị trí:** Ô nhập thứ ba  
**Mô tả:** Nhập tên đăng nhập SQL Server (SQL Authentication)

**Ví dụ:**
- `sa` - Tài khoản quản trị mặc định
- `vns_user` - Tài khoản người dùng tùy chỉnh

**Lưu ý:**
- ⚠️ Không được để trống
- ✅ Hệ thống sử dụng SQL Authentication (không dùng Windows Authentication)
- ✅ Tài khoản phải có quyền truy cập database đã chọn

#### 2.4. Mật khẩu (Password)

**Vị trí:** Ô nhập thứ tư  
**Mô tả:** Nhập mật khẩu của tài khoản SQL Server

**Lưu ý:**
- ⚠️ Không được để trống
- 🔒 Mật khẩu được ẩn khi nhập (hiển thị dấu `*`)
- ✅ Phân biệt chữ hoa/thường
- ⚠️ Kiểm tra Caps Lock khi nhập

### Bước 3: Kiểm tra và lưu cấu hình

#### 3.1. Nhấn nút "Cập nhật"

Sau khi nhập đầy đủ thông tin:

1. **Nhấn nút "Cập nhật"** (hoặc phím Enter)
2. Hệ thống sẽ:
   - ✅ Kiểm tra các trường không được để trống
   - ✅ Kiểm tra kết nối đến cơ sở dữ liệu
   - ✅ Nếu thành công: Lưu cấu hình và đóng form
   - ❌ Nếu thất bại: Hiển thị lỗi, giữ form mở

#### 3.2. Thông báo kết quả

**Thành công:**
```
"Kết nối cơ sở dữ liệu thành công!
Cấu hình đã được lưu."
```

**Thất bại:**
```
"Không thể kết nối đến cơ sở dữ liệu.
Vui lòng kiểm tra lại thông tin kết nối."
```

### Bước 4: Hủy thao tác (nếu cần)

- Nhấn nút **"Hủy"** để đóng form mà không lưu thay đổi
- Cấu hình cũ sẽ được giữ nguyên

---

## 3. Giải thích các control trên form

### 3.1. TextBox - IP/Tên máy chủ

| Thuộc tính | Giá trị |
|------------|---------|
| **Tên control** | `ServerNameTextEdit` |
| **Loại** | DevExpress TextEdit |
| **Binding** | `DatabaseConfig.ServerName` |
| **TabIndex** | 0 |
| **Validation** | Không được để trống |

**Cách sử dụng:**
- Click vào ô và nhập tên máy chủ hoặc IP
- Hệ thống tự động trim khoảng trắng đầu/cuối

### 3.2. TextBox - Tên CSDL

| Thuộc tính | Giá trị |
|------------|---------|
| **Tên control** | `DatabaseNameTextEdit` |
| **Loại** | DevExpress TextEdit |
| **Binding** | `DatabaseConfig.DatabaseName` |
| **TabIndex** | 2 |
| **Validation** | Không được để trống |

**Cách sử dụng:**
- Nhập tên database chính xác như trên SQL Server
- Kiểm tra tên database có tồn tại trước khi nhập

### 3.3. TextBox - Tên đăng nhập

| Thuộc tính | Giá trị |
|------------|---------|
| **Tên control** | `UserIdTextEdit` |
| **Loại** | DevExpress TextEdit |
| **Binding** | `DatabaseConfig.UserId` |
| **TabIndex** | 3 |
| **Validation** | Không được để trống |

**Cách sử dụng:**
- Nhập tên đăng nhập SQL Server
- Đảm bảo tài khoản có quyền truy cập database

### 3.4. TextBox - Mật khẩu

| Thuộc tính | Giá trị |
|------------|---------|
| **Tên control** | `PasswordTextEdit` |
| **Loại** | DevExpress TextEdit (Password) |
| **Binding** | `DatabaseConfig.Password` |
| **TabIndex** | 4 |
| **PasswordChar** | `*` |
| **UseSystemPasswordChar** | `true` |
| **Validation** | Không được để trống |

**Cách sử dụng:**
- Nhập mật khẩu (sẽ hiển thị dấu `*`)
- Kiểm tra Caps Lock trước khi nhập

### 3.5. Nút "Cập nhật"

| Thuộc tính | Giá trị |
|------------|---------|
| **Tên control** | `OKSmpleButton` |
| **Text** | "Cập nhật" |
| **Icon** | `apply_16x16` |
| **TabIndex** | 5 |
| **Chức năng** | Validate → Test Connection → Save Config |

**Cách sử dụng:**
- Click để lưu cấu hình
- Hoặc nhấn Enter khi đang ở ô mật khẩu

### 3.6. Nút "Hủy"

| Thuộc tính | Giá trị |
|------------|---------|
| **Tên control** | `CancelSimpleButton` |
| **Text** | "Hủy" |
| **Icon** | `cancel_16x16` |
| **TabIndex** | 6 |
| **Chức năng** | Đóng form, không lưu |

**Cách sử dụng:**
- Click để hủy và đóng form
- Hoặc nhấn ESC (nếu được hỗ trợ)

---

## 4. Phím tắt

Form hỗ trợ các phím tắt sau:

| Phím | Chức năng |
|------|-----------|
| **Tab** | Chuyển sang ô nhập tiếp theo |
| **Shift + Tab** | Chuyển về ô nhập trước |
| **Enter** (ở ô Mật khẩu) | Thực hiện "Cập nhật" (tương đương click nút) |
| **ESC** | Đóng form (nếu được hỗ trợ) |

**Thứ tự Tab:**
1. IP/Tên máy chủ
2. Tên CSDL
3. Tên đăng nhập
4. Mật khẩu
5. Nút "Cập nhật"
6. Nút "Hủy"

---

## 5. Validation - Xử lý lỗi thường gặp

### 5.1. Lỗi: "Tên máy chủ không được để trống"

**Nguyên nhân:**
- Ô "IP/Tên máy chủ" đang để trống

**Cách khắc phục:**
1. Click vào ô "IP/Tên máy chủ"
2. Nhập tên máy chủ hoặc địa chỉ IP
3. Ví dụ: `localhost`, `192.168.1.100`, `SQLSERVER01`

---

### 5.2. Lỗi: "Tên cơ sở dữ liệu không được để trống"

**Nguyên nhân:**
- Ô "Tên CSDL" đang để trống

**Cách khắc phục:**
1. Click vào ô "Tên CSDL"
2. Nhập tên database chính xác
3. Kiểm tra tên database có tồn tại trên SQL Server

---

### 5.3. Lỗi: "Tên đăng nhập không được để trống"

**Nguyên nhân:**
- Ô "Tên đăng nhập" đang để trống

**Cách khắc phục:**
1. Click vào ô "Tên đăng nhập"
2. Nhập tên đăng nhập SQL Server
3. Đảm bảo tài khoản có quyền truy cập

---

### 5.4. Lỗi: "Mật khẩu không được để trống"

**Nguyên nhân:**
- Ô "Mật khẩu" đang để trống

**Cách khắc phục:**
1. Click vào ô "Mật khẩu"
2. Nhập mật khẩu của tài khoản SQL Server
3. Kiểm tra Caps Lock

---

### 5.5. Lỗi: "Không thể kết nối đến cơ sở dữ liệu"

**Nguyên nhân có thể:**
1. ❌ Tên máy chủ hoặc IP không đúng
2. ❌ SQL Server không chạy hoặc không khả dụng
3. ❌ Tên database không tồn tại
4. ❌ Tên đăng nhập hoặc mật khẩu sai
5. ❌ Tài khoản không có quyền truy cập database
6. ❌ Firewall chặn kết nối
7. ❌ SQL Server không cho phép kết nối từ xa
8. ❌ Port SQL Server bị chặn (mặc định 1433)

**Cách khắc phục:**

**Bước 1: Kiểm tra SQL Server**
- ✅ SQL Server đang chạy
- ✅ SQL Server Browser đang chạy (nếu dùng named instance)
- ✅ SQL Server cho phép kết nối từ xa

**Bước 2: Kiểm tra thông tin kết nối**
- ✅ Tên máy chủ/IP chính xác
- ✅ Tên database tồn tại
- ✅ Tên đăng nhập và mật khẩu đúng

**Bước 3: Kiểm tra quyền truy cập**
- ✅ Tài khoản có quyền `db_datareader` và `db_datawriter` trên database
- ✅ Hoặc có quyền `db_owner` trên database

**Bước 4: Kiểm tra Firewall và Network**
- ✅ Port 1433 (hoặc port SQL Server) mở
- ✅ Windows Firewall cho phép SQL Server
- ✅ Network có thể truy cập máy chủ

**Bước 5: Kiểm tra SQL Server Configuration**
- ✅ SQL Server cho phép SQL Authentication
- ✅ Mixed Mode Authentication được bật

**Công cụ kiểm tra:**
- Sử dụng SQL Server Management Studio (SSMS) để test kết nối
- Sử dụng `sqlcmd` để test từ command line:
  ```
  sqlcmd -S TênMáyChủ -U TênĐăngNhập -P MậtKhẩu -d TênDatabase
  ```

---

### 5.6. Lỗi: "Lỗi khởi tạo form"

**Nguyên nhân:**
- Lỗi khi tải cấu hình từ Settings
- Lỗi khi giải mã mật khẩu đã lưu

**Cách khắc phục:**
1. Đóng form và mở lại
2. Nếu vẫn lỗi, liên hệ quản trị viên
3. Có thể cần xóa cấu hình cũ và nhập lại

---

### 5.7. Lỗi: "Lỗi lưu cấu hình"

**Nguyên nhân:**
- Không có quyền ghi vào file Settings
- File Settings bị khóa
- Ổ đĩa đầy

**Cách khắc phục:**
1. Chạy ứng dụng với quyền Administrator
2. Kiểm tra dung lượng ổ đĩa
3. Đóng các ứng dụng khác có thể đang sử dụng Settings
4. Thử lại

---

## 6. Câu hỏi thường gặp (FAQ)

### Q1: Tôi có thể dùng Windows Authentication không?

**A:** Không. Hiện tại hệ thống chỉ hỗ trợ **SQL Authentication** (tên đăng nhập và mật khẩu). Windows Authentication không được hỗ trợ trong form này.

---

### Q2: Mật khẩu có được lưu an toàn không?

**A:** Có. Mật khẩu được **mã hóa bằng Base64** trước khi lưu vào Settings. Tuy nhiên, đây không phải mã hóa mạnh, nên:
- ✅ An toàn cho môi trường development
- ⚠️ Nên cẩn thận trong môi trường production
- 🔒 Không chia sẻ file Settings với người khác

---

### Q3: Tôi có thể kết nối đến SQL Server trên máy khác không?

**A:** Có, nếu:
- ✅ SQL Server cho phép kết nối từ xa
- ✅ Firewall cho phép kết nối
- ✅ Network có thể truy cập máy chủ
- ✅ Bạn có tên đăng nhập và mật khẩu hợp lệ

**Cách nhập:**
- Nhập IP hoặc tên máy chủ vào ô "IP/Tên máy chủ"
- Ví dụ: `192.168.1.100` hoặc `SQLSERVER01`

---

### Q4: Tôi có thể kết nối đến SQL Server Express không?

**A:** Có. Nhập tên máy chủ kèm instance:
- Ví dụ: `localhost\SQLEXPRESS`
- Hoặc: `TênMáyChủ\SQLEXPRESS`

---

### Q5: Form có tự động kiểm tra kết nối không?

**A:** Có. Khi bạn nhấn "Cập nhật", hệ thống sẽ:
1. Kiểm tra các trường không được để trống
2. Tạo connection string
3. Thử kết nối đến database
4. Thực hiện truy vấn test (`SELECT GETDATE()`)
5. Chỉ lưu cấu hình nếu kết nối thành công

---

### Q6: Tôi có thể thay đổi port SQL Server không?

**A:** Hiện tại form không hỗ trợ nhập port riêng. Hệ thống sử dụng port mặc định (1433) hoặc port của named instance.

Nếu SQL Server dùng port khác, bạn có thể:
- Nhập theo định dạng: `TênMáyChủ,Port`
- Ví dụ: `192.168.1.100,1434`

---

### Q7: Cấu hình được lưu ở đâu?

**A:** Cấu hình được lưu trong **User Settings** của ứng dụng:
- **Vị trí:** `%LocalAppData%\YourApp\user.config`
- **Các giá trị được lưu:**
  - `DatabaseServer` - Tên máy chủ
  - `DatabaseName` - Tên database
  - `DatabaseUserId` - Tên đăng nhập
  - `DatabasePassword` - Mật khẩu (đã mã hóa)
  - `UseIntegratedSecurity` - Luôn là `false`

---

### Q8: Tôi có thể xem lại mật khẩu đã lưu không?

**A:** Không. Mật khẩu được ẩn trong form và không thể xem lại. Nếu quên, bạn cần:
- Nhập lại mật khẩu mới
- Hoặc liên hệ quản trị viên SQL Server

---

### Q9: Form có hỗ trợ nhiều cấu hình database không?

**A:** Không. Form chỉ hỗ trợ một cấu hình database duy nhất. Nếu cần thay đổi, bạn phải:
1. Mở form
2. Nhập thông tin mới
3. Nhấn "Cập nhật"
4. Cấu hình cũ sẽ bị thay thế

---

### Q10: Tôi gặp lỗi "Timeout" khi kết nối, làm thế nào?

**A:** Lỗi timeout có thể do:
- ⏱️ SQL Server phản hồi chậm
- 🌐 Network chậm hoặc không ổn định
- 🔥 Firewall chặn kết nối

**Cách khắc phục:**
1. Kiểm tra SQL Server có đang chạy không
2. Kiểm tra network connection
3. Kiểm tra firewall
4. Thử ping đến máy chủ SQL Server
5. Liên hệ quản trị viên nếu vẫn không được

**Lưu ý:** Form sử dụng timeout mặc định 15 giây cho connection và 30 giây cho command.

---

## 7. Lưu ý bảo mật

### 7.1. Bảo vệ thông tin đăng nhập

⚠️ **Quan trọng:**
- ✅ Chỉ nhập thông tin trên máy tính an toàn
- ❌ Không chia sẻ file Settings với người khác
- 🔒 Mật khẩu được mã hóa nhưng không phải mã hóa mạnh
- 🚫 Không lưu mật khẩu trên máy tính công cộng

### 7.2. Quyền truy cập database

- ✅ Chỉ cấp quyền cần thiết cho tài khoản
- ✅ Không dùng tài khoản `sa` trong production
- ✅ Tạo tài khoản riêng với quyền hạn chế
- ✅ Đổi mật khẩu định kỳ

### 7.3. Kết nối mạng

- ✅ Sử dụng kết nối an toàn (VPN) nếu kết nối từ xa
- ✅ Bật firewall và chỉ mở port cần thiết
- ✅ Sử dụng SSL/TLS nếu có thể

---

## 8. Thông tin phiên bản

- **Phiên bản:** 1.0
- **Cập nhật lần cuối:** 2025
- **Hệ thống:** VNS ERP 2025
- **Form:** FrmDatabaseConfig

---

## Hỗ trợ

Nếu bạn gặp vấn đề khi sử dụng form này:

1. ✅ Kiểm tra lại các bước trong hướng dẫn
2. ✅ Xem phần "Xử lý lỗi thường gặp"
3. ✅ Liên hệ quản trị viên hệ thống hoặc bộ phận IT

**Chúc bạn sử dụng hệ thống hiệu quả!** 🎉
