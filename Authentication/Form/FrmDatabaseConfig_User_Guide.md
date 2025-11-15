# Hướng dẫn sử dụng - Cài đặt máy chủ cơ sở dữ liệu

## 1. Giới thiệu

### Chức năng của form

Form **Cài đặt máy chủ cơ sở dữ liệu** cho phép bạn cấu hình thông tin kết nối đến cơ sở dữ liệu SQL Server của hệ thống VNS ERP 2025. Form này giúp bạn:

- ✅ Thiết lập thông tin kết nối cơ sở dữ liệu
- ✅ Kiểm tra kết nối trước khi lưu
- ✅ Lưu cấu hình để sử dụng cho các lần sau
- ✅ Bảo mật mật khẩu bằng mã hóa

### Mục đích sử dụng

Form này thường được sử dụng khi:
- 🆕 Lần đầu cài đặt hệ thống
- 🔄 Thay đổi máy chủ cơ sở dữ liệu
- 🔧 Khắc phục sự cố kết nối
- 📦 Di chuyển hệ thống sang máy chủ mới

### Tóm tắt workflow

```
1. Mở form → Hệ thống tự động tải cấu hình hiện tại
2. Nhập/Chỉnh sửa thông tin kết nối
3. Nhấn "Cập nhật" → Hệ thống kiểm tra kết nối
4. Nếu kết nối thành công → Lưu cấu hình và đóng form
5. Nếu kết nối thất bại → Hiển thị lỗi, bạn có thể sửa lại
```

---

## 2. Hướng dẫn sử dụng form

### Bước 1: Mở form

Form sẽ tự động hiển thị khi:
- Lần đầu khởi động ứng dụng (nếu chưa có cấu hình)
- Được gọi từ menu cài đặt hệ thống

### Bước 2: Xem thông tin hiện tại

Khi form mở, các trường sẽ tự động hiển thị thông tin cấu hình hiện tại (nếu có):
- **IP/Tên máy chủ**: Tên hoặc địa chỉ IP của SQL Server
- **Tên CSDL**: Tên database cần kết nối
- **Tên đăng nhập**: Username để đăng nhập SQL Server
- **Mật khẩu**: Password (sẽ hiển thị dưới dạng dấu sao `*`)

### Bước 3: Nhập/Chỉnh sửa thông tin

#### 📍 **IP/Tên máy chủ** (ServerNameTextEdit)

- **Mô tả**: Địa chỉ hoặc tên của máy chủ SQL Server
- **Ví dụ hợp lệ**:
  - `localhost` - Máy chủ trên cùng máy tính
  - `192.168.1.100` - Địa chỉ IP
  - `SERVER01` - Tên máy chủ trong mạng
  - `SERVER01\SQLEXPRESS` - Named instance
- **Yêu cầu**: Không được để trống
- **Tab Index**: 0 (trường đầu tiên)

#### 📍 **Tên CSDL** (DatabaseNameTextEdit)

- **Mô tả**: Tên của database cần kết nối
- **Ví dụ hợp lệ**:
  - `VnsErp2025`
  - `VnsErp2025Final`
  - `MyDatabase`
- **Yêu cầu**: Không được để trống
- **Tab Index**: 2

#### 📍 **Tên đăng nhập** (UserIdTextEdit)

- **Mô tả**: Username để đăng nhập vào SQL Server
- **Ví dụ hợp lệ**:
  - `sa` - System Administrator
  - `dbuser` - User tùy chỉnh
- **Yêu cầu**: Không được để trống
- **Lưu ý**: Hệ thống luôn sử dụng SQL Authentication (không dùng Windows Authentication)
- **Tab Index**: 3

#### 📍 **Mật khẩu** (PasswordTextEdit)

- **Mô tả**: Password để đăng nhập vào SQL Server
- **Yêu cầu**: Không được để trống
- **Bảo mật**: 
  - Mật khẩu được ẩn khi nhập (hiển thị dấu `*`)
  - Mật khẩu được mã hóa trước khi lưu vào cấu hình
- **Tab Index**: 4

### Bước 4: Kiểm tra và lưu cấu hình

1. **Nhấn nút "Cập nhật"** (OKSmpleButton)
   - Hệ thống sẽ:
     - ✅ Kiểm tra tất cả trường không được để trống
     - ✅ Thử kết nối đến database với thông tin bạn nhập
     - ✅ Nếu kết nối thành công → Lưu cấu hình và đóng form
     - ❌ Nếu kết nối thất bại → Hiển thị thông báo lỗi

2. **Nhấn nút "Hủy"** (CancelSimpleButton)
   - Đóng form mà không lưu thay đổi
   - Cấu hình cũ vẫn được giữ nguyên

---

## 3. Bảng phím tắt

| Phím | Chức năng |
|------|-----------|
| **Tab** | Chuyển sang trường tiếp theo |
| **Shift + Tab** | Quay lại trường trước |
| **Enter** | Không có chức năng đặc biệt (chỉ chuyển focus) |

**Lưu ý**: Form không hỗ trợ phím tắt để lưu nhanh. Bạn cần click nút "Cập nhật" để lưu.

---

## 4. Validation - Xử lý lỗi thường gặp

### ❌ Lỗi: "Tên máy chủ không được để trống"

**Nguyên nhân**: Bạn chưa nhập thông tin vào trường "IP/Tên máy chủ"

**Cách khắc phục**:
1. Click vào trường "IP/Tên máy chủ"
2. Nhập tên hoặc địa chỉ IP của máy chủ SQL Server
3. Ví dụ: `localhost`, `192.168.1.100`, hoặc `SERVER01`

---

### ❌ Lỗi: "Tên cơ sở dữ liệu không được để trống"

**Nguyên nhân**: Bạn chưa nhập tên database

**Cách khắc phục**:
1. Click vào trường "Tên CSDL"
2. Nhập tên database cần kết nối
3. Ví dụ: `VnsErp2025`, `VnsErp2025Final`

**Lưu ý**: Tên database phải tồn tại trên SQL Server. Nếu chưa có, bạn cần tạo database trước.

---

### ❌ Lỗi: "Tên đăng nhập không được để trống"

**Nguyên nhân**: Bạn chưa nhập username

**Cách khắc phục**:
1. Click vào trường "Tên đăng nhập"
2. Nhập username có quyền truy cập database
3. Ví dụ: `sa` (System Administrator) hoặc username khác

---

### ❌ Lỗi: "Mật khẩu không được để trống"

**Nguyên nhân**: Bạn chưa nhập mật khẩu

**Cách khắc phục**:
1. Click vào trường "Mật khẩu"
2. Nhập mật khẩu tương ứng với username đã nhập

---

### ❌ Lỗi: "Không thể kết nối đến cơ sở dữ liệu. Vui lòng kiểm tra lại thông tin kết nối."

**Nguyên nhân**: Có nhiều nguyên nhân có thể gây ra lỗi này:

1. **Máy chủ SQL Server không chạy**
   - Kiểm tra SQL Server Service có đang chạy không
   - Khởi động SQL Server Service nếu cần

2. **Tên máy chủ hoặc IP không đúng**
   - Kiểm tra lại tên máy chủ hoặc địa chỉ IP
   - Thử ping đến máy chủ để kiểm tra kết nối mạng

3. **Tên database không tồn tại**
   - Kiểm tra database đã được tạo chưa
   - Tạo database mới nếu cần

4. **Username hoặc Password sai**
   - Kiểm tra lại username và password
   - Đảm bảo Caps Lock không được bật
   - Thử đăng nhập bằng SQL Server Management Studio với thông tin tương tự

5. **SQL Server không cho phép kết nối từ xa**
   - Kiểm tra SQL Server có cho phép Remote Connections không
   - Kiểm tra Firewall có chặn port 1433 không

6. **Named Instance không đúng**
   - Nếu dùng named instance, đảm bảo format đúng: `SERVERNAME\INSTANCENAME`
   - Ví dụ: `SERVER01\SQLEXPRESS`

**Cách khắc phục từng bước**:
1. ✅ Kiểm tra SQL Server Service đang chạy
2. ✅ Kiểm tra tên máy chủ/IP đúng
3. ✅ Kiểm tra database đã tồn tại
4. ✅ Kiểm tra username/password đúng
5. ✅ Kiểm tra kết nối mạng và firewall
6. ✅ Thử kết nối bằng SQL Server Management Studio

---

### ❌ Lỗi: "Lỗi khởi tạo form"

**Nguyên nhân**: Có lỗi khi tải cấu hình hiện tại

**Cách khắc phục**:
1. Đóng và mở lại form
2. Nếu vẫn lỗi, liên hệ bộ phận IT

---

### ❌ Lỗi: "Lỗi tải dữ liệu từ Settings"

**Nguyên nhân**: Không thể đọc cấu hình đã lưu

**Cách khắc phục**:
1. Nhập lại thông tin từ đầu
2. Nếu vẫn lỗi, liên hệ bộ phận IT

---

### ❌ Lỗi: "Lỗi lưu cấu hình"

**Nguyên nhân**: Không thể ghi cấu hình vào file

**Cách khắc phục**:
1. Kiểm tra quyền ghi file của ứng dụng
2. Chạy ứng dụng với quyền Administrator
3. Liên hệ bộ phận IT nếu vẫn lỗi

---

## 5. Câu hỏi thường gặp (FAQ)

### Q1: Tôi có thể dùng Windows Authentication không?

**A:** Không. Hiện tại form chỉ hỗ trợ SQL Authentication (username/password). Hệ thống sẽ tự động đặt `UseIntegratedSecurity = false`.

---

### Q2: Mật khẩu của tôi có an toàn không?

**A:** Có. Mật khẩu được mã hóa bằng Base64 trước khi lưu vào cấu hình. Tuy nhiên, bạn vẫn nên:
- ✅ Sử dụng mật khẩu mạnh
- ✅ Không chia sẻ thông tin đăng nhập
- ✅ Thay đổi mật khẩu định kỳ

---

### Q3: Tôi có thể kết nối đến SQL Server trên máy khác không?

**A:** Có, miễn là:
- ✅ Máy chủ SQL Server cho phép kết nối từ xa
- ✅ Firewall không chặn port 1433 (hoặc port SQL Server đang dùng)
- ✅ Bạn có thông tin đăng nhập hợp lệ
- ✅ Máy tính của bạn có thể kết nối mạng đến máy chủ

---

### Q4: Tôi nhập đúng thông tin nhưng vẫn báo lỗi kết nối?

**A:** Hãy kiểm tra:
1. SQL Server Service có đang chạy không
2. Tên máy chủ/IP có đúng không (thử ping)
3. Database có tồn tại không
4. Username/password có đúng không (thử đăng nhập bằng SQL Server Management Studio)
5. Firewall có chặn không
6. SQL Server có cho phép Remote Connections không

---

### Q5: Tôi có thể dùng tên instance không?

**A:** Có. Bạn có thể nhập tên instance trong trường "IP/Tên máy chủ" với format:
- `SERVERNAME\INSTANCENAME`
- Ví dụ: `SERVER01\SQLEXPRESS`

---

### Q6: Form có tự động lưu không?

**A:** Không. Bạn phải nhấn nút "Cập nhật" để lưu cấu hình. Nếu nhấn "Hủy", tất cả thay đổi sẽ bị hủy.

---

### Q7: Tôi có thể xem lại mật khẩu đã lưu không?

**A:** Không. Mật khẩu được ẩn và mã hóa. Nếu quên mật khẩu, bạn cần:
- Liên hệ quản trị viên database
- Hoặc nhập lại mật khẩu mới trong form này

---

### Q8: Cấu hình được lưu ở đâu?

**A:** Cấu hình được lưu trong:
- **User Settings** của ứng dụng (Properties.Settings)
- File cấu hình: `%LocalAppData%\YourApp\user.config`
- Các thông tin được lưu:
  - `DatabaseServer` - Tên máy chủ
  - `DatabaseName` - Tên database
  - `DatabaseUserId` - Username
  - `DatabasePassword` - Password (đã mã hóa)
  - `UseIntegratedSecurity` - Luôn là `false`

---

### Q9: Tôi có thể dùng nhiều database khác nhau không?

**A:** Form chỉ cho phép cấu hình một database tại một thời điểm. Nếu cần đổi database, bạn mở lại form và nhập thông tin mới.

---

### Q10: Form có kiểm tra kết nối trước khi lưu không?

**A:** Có. Khi bạn nhấn "Cập nhật", hệ thống sẽ:
1. Kiểm tra tất cả trường không rỗng
2. Thử kết nối đến database với thông tin bạn nhập
3. Chỉ lưu nếu kết nối thành công
4. Hiển thị lỗi nếu kết nối thất bại

---

## 6. Lưu ý bảo mật

### 🔒 Bảo mật mật khẩu

- ✅ Mật khẩu được mã hóa (Base64) trước khi lưu
- ✅ Mật khẩu không hiển thị dạng văn bản thô
- ⚠️ Tuy nhiên, Base64 không phải mã hóa mạnh, chỉ là encoding
- 💡 **Khuyến nghị**: Sử dụng mật khẩu mạnh và không chia sẻ thông tin đăng nhập

### 🔒 Bảo mật cấu hình

- ✅ Cấu hình được lưu trong User Settings (an toàn hơn Registry)
- ✅ Chỉ user hiện tại có thể truy cập cấu hình của mình
- ⚠️ Không lưu cấu hình trên máy tính dùng chung

### 🔒 Best Practices

1. ✅ **Sử dụng mật khẩu mạnh**: Tối thiểu 8 ký tự, có chữ hoa, chữ thường, số
2. ✅ **Không chia sẻ thông tin đăng nhập**: Mỗi user nên có tài khoản riêng
3. ✅ **Thay đổi mật khẩu định kỳ**: Đặc biệt cho tài khoản `sa`
4. ✅ **Kiểm tra quyền truy cập**: Chỉ cấp quyền cần thiết cho user
5. ✅ **Sử dụng tài khoản riêng**: Không dùng `sa` cho production

---

## 7. Thông tin phiên bản

- **Phiên bản**: 1.0
- **Cập nhật lần cuối**: 2025
- **Hệ thống**: VNS ERP 2025
- **Form**: FrmDatabaseConfig

---

## Hỗ trợ

Nếu bạn gặp vấn đề khi sử dụng form này:

1. ✅ Kiểm tra lại các bước trong hướng dẫn
2. ✅ Xem phần "Validation - Xử lý lỗi thường gặp"
3. ✅ Liên hệ **bộ phận IT** hoặc **quản trị viên hệ thống**

---

**Chúc bạn cấu hình thành công!** 🎉

