# Hướng Dẫn Sử Dụng - Quản Lý Thông Tin Công Ty

## Mục Lục

1. [Giới Thiệu](#giới-thiệu)
2. [Hướng Dẫn Sử Dụng](#hướng-dẫn-sử-dụng)
3. [Validation và Xử Lý Lỗi](#validation-và-xử-lý-lỗi)
4. [Câu Hỏi Thường Gặp (FAQs)](#câu-hỏi-thường-gặp-faqs)
5. [Lưu Ý và Bảo Mật](#lưu-ý-và-bảo-mật)
6. [Thông Tin Phiên Bản](#thông-tin-phiên-bản)

---

## 1. Giới Thiệu

### 1.1. Chức Năng

**UcCompany** (User Control Quản Lý Thông Tin Công Ty) là màn hình cho phép bạn:

- **Xem và chỉnh sửa** thông tin công ty trong hệ thống
- **Quản lý logo** công ty (thêm, xóa, thay đổi)
- **Đảm bảo** hệ thống chỉ có **một công ty duy nhất**

### 1.2. Mục Đích Sử Dụng

Màn hình này được sử dụng để:

- Cấu hình thông tin công ty ban đầu khi thiết lập hệ thống
- Cập nhật thông tin công ty khi có thay đổi
- Quản lý logo công ty để hiển thị trong các báo cáo và tài liệu

### 1.3. Workflow Sử Dụng

```
1. Màn hình tự động load thông tin công ty từ database
2. Hệ thống đảm bảo chỉ có 1 công ty trong database
3. Người dùng xem/chỉnh sửa thông tin
4. Người dùng quản lý logo (nếu cần)
5. Thông tin được lưu tự động khi thay đổi logo
```

---

## 2. Hướng Dẫn Sử Dụng

### 2.1. Các Trường Thông Tin

#### 2.1.1. Mã Công Ty (CompanyCode) ⭐ **BẮT BUỘC**

- **Mô tả**: Mã định danh duy nhất của công ty
- **Ví dụ**: `CT01`, `COMPANY001`
- **Ràng buộc**:
  - ⚠️ **Bắt buộc nhập** (có dấu * đỏ)
  - Tối đa **50 ký tự**
  - Không được để trống

#### 2.1.2. Tên Công Ty (CompanyName) ⭐ **BẮT BUỘC**

- **Mô tả**: Tên đầy đủ của công ty
- **Ví dụ**: `Công ty TNHH ABC`, `ABC Company Limited`
- **Ràng buộc**:
  - ⚠️ **Bắt buộc nhập** (có dấu * đỏ)
  - Tối đa **255 ký tự**
  - Không được để trống
  - Không được chứa chỉ khoảng trắng

#### 2.1.3. Mã Số Thuế (TaxCode) - Tùy chọn

- **Mô tả**: Mã số thuế của công ty
- **Ví dụ**: `0123456789`
- **Ràng buộc**:
  - Không bắt buộc
  - Tối đa **50 ký tự** (nếu có nhập)

#### 2.1.4. Số Điện Thoại (Phone) - Tùy chọn

- **Mô tả**: Số điện thoại liên hệ của công ty
- **Ví dụ**: `02812345678`, `0912345678`
- **Ràng buộc**:
  - Không bắt buộc
  - Tối đa **50 ký tự** (nếu có nhập)

#### 2.1.5. Email - Tùy chọn

- **Mô tả**: Địa chỉ email của công ty
- **Ví dụ**: `info@company.com`, `contact@abc.vn`
- **Ràng buộc**:
  - Không bắt buộc
  - Tối đa **100 ký tự** (nếu có nhập)
  - ⚠️ **Phải đúng định dạng email** nếu có nhập (ví dụ: `user@domain.com`)

#### 2.1.6. Website - Tùy chọn

- **Mô tả**: Địa chỉ website của công ty
- **Ví dụ**: `www.company.com`, `https://company.vn`
- **Ràng buộc**:
  - Không bắt buộc
  - Tối đa **100 ký tự** (nếu có nhập)

#### 2.1.7. Địa Chỉ (Address) - Tùy chọn

- **Mô tả**: Địa chỉ trụ sở chính của công ty
- **Ví dụ**: `123 Đường ABC, Quận XYZ, TP.HCM`
- **Ràng buộc**:
  - Không bắt buộc
  - Tối đa **255 ký tự** (nếu có nhập)

#### 2.1.8. Quốc Gia (Country) - Tùy chọn

- **Mô tả**: Quốc gia của công ty
- **Ví dụ**: `Việt Nam`, `USA`, `Singapore`
- **Ràng buộc**:
  - Không bắt buộc
  - Tối đa **100 ký tự** (nếu có nhập)

#### 2.1.9. Ngày Tạo (CreatedDate) ⭐ **BẮT BUỘC**

- **Mô tả**: Ngày tạo thông tin công ty trong hệ thống
- **Ràng buộc**:
  - ⚠️ **Bắt buộc** (có dấu * đỏ)
  - Tự động được set bởi hệ thống
  - **Chỉ đọc** (read-only) - người dùng không thể chỉnh sửa

#### 2.1.10. Logo - Tùy chọn

- **Mô tả**: Logo của công ty
- **Định dạng hỗ trợ**: JPG, JPEG, PNG, BMP, GIF
- **Ràng buộc**:
  - Không bắt buộc
  - Chỉ chấp nhận file hình ảnh

### 2.2. Quản Lý Logo

#### 2.2.1. Thêm/Tải Logo

**Cách 1: Sử dụng Menu Chuột Phải**

1. Click chuột phải vào vùng hiển thị logo
2. Chọn **"Load..."** từ menu
3. Chọn file hình ảnh từ hộp thoại
4. Logo sẽ được hiển thị và **tự động lưu vào database**

**Cách 2: Drag & Drop**

1. Kéo thả file hình ảnh từ Windows Explorer vào vùng logo
2. Logo sẽ được hiển thị và **tự động lưu vào database**

#### 2.2.2. Xóa Logo

1. Click chuột phải vào vùng hiển thị logo
2. Chọn **"Delete"** từ menu
3. Xác nhận xóa trong hộp thoại
4. Logo sẽ bị xóa và **tự động cập nhật trong database**

### 2.3. Thanh Công Cụ (Toolbar)

#### 2.3.1. Nút Lưu (SaveBarButtonItem)

- **Vị trí**: Thanh công cụ phía trên
- **Biểu tượng**: 💾
- **Chức năng**: 
  - ⚠️ **Lưu ý**: Hiện tại nút Lưu chưa có chức năng lưu dữ liệu. Logo được lưu tự động khi thêm/xóa.

### 2.4. Tooltips (Gợi Ý)

Khi di chuột qua các trường, bạn sẽ thấy tooltip hiển thị:

- **Tiêu đề**: Tên trường với biểu tượng
- **Nội dung**: Hướng dẫn chi tiết về trường đó, bao gồm:
  - Chức năng
  - Ràng buộc
  - Validation
  - DataAnnotations

---

## 3. Validation và Xử Lý Lỗi

### 3.1. Danh Sách Lỗi Thường Gặp

#### 3.1.1. Lỗi: "Mã công ty không được để trống"

**Nguyên nhân**:
- Trường **Mã công ty** là bắt buộc nhưng bạn chưa nhập

**Cách khắc phục**:
1. Nhập mã công ty vào trường **Mã công ty**
2. Mã công ty phải có ít nhất 1 ký tự
3. Tối đa 50 ký tự

---

#### 3.1.2. Lỗi: "Tên công ty không được để trống"

**Nguyên nhân**:
- Trường **Tên công ty** là bắt buộc nhưng bạn chưa nhập

**Cách khắc phục**:
1. Nhập tên công ty vào trường **Tên công ty**
2. Tên công ty phải có ít nhất 1 ký tự (không phải chỉ khoảng trắng)
3. Tối đa 255 ký tự

---

#### 3.1.3. Lỗi: "Mã công ty không được vượt quá 50 ký tự"

**Nguyên nhân**:
- Bạn đã nhập mã công ty dài hơn 50 ký tự

**Cách khắc phục**:
1. Rút ngắn mã công ty xuống còn tối đa 50 ký tự
2. Xóa các ký tự thừa

---

#### 3.1.4. Lỗi: "Tên công ty không được vượt quá 255 ký tự"

**Nguyên nhân**:
- Bạn đã nhập tên công ty dài hơn 255 ký tự

**Cách khắc phục**:
1. Rút ngắn tên công ty xuống còn tối đa 255 ký tự
2. Xóa các ký tự thừa

---

#### 3.1.5. Lỗi: "Email không đúng định dạng"

**Nguyên nhân**:
- Bạn đã nhập email nhưng định dạng không đúng

**Cách khắc phục**:
1. Kiểm tra lại định dạng email
2. Email phải có dạng: `username@domain.com`
3. Ví dụ đúng: `info@company.com`, `contact@abc.vn`
4. Ví dụ sai: `info@`, `@company.com`, `info company.com`

---

#### 3.1.6. Lỗi: "Email không được vượt quá 100 ký tự"

**Nguyên nhân**:
- Bạn đã nhập email dài hơn 100 ký tự

**Cách khắc phục**:
1. Rút ngắn email xuống còn tối đa 100 ký tự
2. Hoặc sử dụng email ngắn hơn

---

#### 3.1.7. Lỗi: "Vui lòng chọn file hình ảnh hợp lệ!"

**Nguyên nhân**:
- Bạn đã kéo thả file không phải là hình ảnh vào vùng logo

**Cách khắc phục**:
1. Chỉ chọn các file có định dạng: **JPG, JPEG, PNG, BMP, GIF**
2. Không chọn file văn bản, PDF, hoặc các định dạng khác

---

#### 3.1.8. Lỗi: "Không tìm thấy thông tin công ty trong database"

**Nguyên nhân**:
- Hệ thống không tìm thấy thông tin công ty trong database

**Cách khắc phục**:
1. Kiểm tra kết nối database
2. Liên hệ quản trị viên hệ thống
3. Hệ thống sẽ tự động tạo công ty mặc định nếu chưa có

---

### 3.2. Hiển Thị Lỗi

- Lỗi được hiển thị qua **DXErrorProvider** (biểu tượng cảnh báo màu đỏ bên cạnh trường)
- Tooltip hiển thị thông báo lỗi chi tiết khi di chuột qua biểu tượng lỗi
- Các trường bắt buộc có dấu **<color=red>*</color>** màu đỏ

---

## 4. Câu Hỏi Thường Gặp (FAQs)

### 4.1. Tại sao hệ thống chỉ cho phép có 1 công ty?

**Trả lời**: Đây là thiết kế của hệ thống ERP. Mỗi hệ thống chỉ quản lý thông tin của một công ty duy nhất. Khi màn hình load, hệ thống tự động đảm bảo chỉ có 1 công ty trong database.

---

### 4.2. Làm thế nào để thay đổi logo công ty?

**Trả lời**: 
- **Cách 1**: Click chuột phải vào vùng logo → Chọn **"Load..."** → Chọn file hình ảnh mới
- **Cách 2**: Kéo thả file hình ảnh vào vùng logo
- Logo sẽ được lưu tự động vào database ngay sau khi thay đổi

---

### 4.3. Logo có bắt buộc không?

**Trả lời**: Không. Logo là trường tùy chọn. Bạn có thể để trống nếu không cần thiết.

---

### 4.4. Tại sao không thể chỉnh sửa trường "Ngày tạo"?

**Trả lời**: Trường "Ngày tạo" được tự động set bởi hệ thống khi tạo mới công ty. Người dùng không thể chỉnh sửa để đảm bảo tính nhất quán của dữ liệu.

---

### 4.5. Làm thế nào để xóa logo?

**Trả lời**: 
1. Click chuột phải vào vùng logo
2. Chọn **"Delete"**
3. Xác nhận xóa trong hộp thoại
4. Logo sẽ bị xóa và cập nhật trong database

---

### 4.6. Email có bắt buộc nhập không?

**Trả lời**: Không. Email là trường tùy chọn. Tuy nhiên, nếu bạn nhập email, nó phải đúng định dạng (ví dụ: `user@domain.com`).

---

### 4.7. Có thể nhập bao nhiêu ký tự cho tên công ty?

**Trả lời**: Tối đa **255 ký tự**. Nếu vượt quá, hệ thống sẽ hiển thị lỗi.

---

### 4.8. Logo hỗ trợ những định dạng nào?

**Trả lời**: Logo hỗ trợ các định dạng:
- **JPG, JPEG**
- **PNG**
- **BMP**
- **GIF**

---

### 4.9. Tại sao nút "Lưu" không hoạt động?

**Trả lời**: Hiện tại, nút "Lưu" chưa có chức năng lưu dữ liệu. Logo được lưu tự động khi thêm/xóa. Các trường thông tin khác sẽ được lưu trong phiên bản tương lai.

---

### 4.10. Làm thế nào để biết trường nào là bắt buộc?

**Trả lời**: Các trường bắt buộc có dấu **<color=red>*</color>** màu đỏ bên cạnh tên trường. Các trường bắt buộc trong màn hình này:
- **Mã công ty** ⭐
- **Tên công ty** ⭐
- **Ngày tạo** ⭐

---

## 5. Lưu Ý và Bảo Mật

### 5.1. Lưu Ý Chung

- ⚠️ **Hệ thống chỉ cho phép có 1 công ty duy nhất**. Khi màn hình load, hệ thống tự động đảm bảo điều này.
- ⚠️ **Logo được lưu tự động** khi thêm hoặc xóa. Không cần click nút "Lưu".
- ⚠️ **Nút "Lưu" hiện tại chưa có chức năng** lưu các trường thông tin khác (ngoài logo).
- ⚠️ **Trường "Ngày tạo"** là chỉ đọc, không thể chỉnh sửa.

### 5.2. Bảo Mật

- Thông tin công ty được lưu trữ trong database
- Logo được lưu dưới dạng binary (byte array) trong database
- Không có thông tin nhạy cảm nào được lưu trữ ở đây

### 5.3. Best Practices

- **Mã công ty**: Nên sử dụng mã ngắn gọn, dễ nhớ (ví dụ: `CT01`)
- **Tên công ty**: Nên nhập tên đầy đủ, chính xác
- **Email**: Nên nhập email chính thức của công ty
- **Logo**: Nên sử dụng logo có độ phân giải phù hợp (không quá lớn để tránh làm chậm hệ thống)

---

## 6. Thông Tin Phiên Bản

### 6.1. Phiên Bản Hiện Tại

- **Tên màn hình**: UcCompany (User Control Quản Lý Thông Tin Công Ty)
- **Module**: MasterData.Company
- **Framework**: DevExpress WinForms
- **Ngôn ngữ**: C#

### 6.2. Tính Năng Hiện Tại

✅ Xem thông tin công ty  
✅ Chỉnh sửa thông tin công ty (giao diện)  
✅ Quản lý logo (thêm, xóa, drag & drop)  
✅ Tự động đảm bảo chỉ có 1 công ty  
✅ Đánh dấu trường bắt buộc  
✅ Validation tự động  
✅ Tooltips hướng dẫn  
✅ Hiển thị lỗi qua ErrorProvider  

### 6.3. Hạn Chế

⚠️ Nút "Lưu" chưa có chức năng lưu các trường thông tin (ngoài logo)  
⚠️ Chưa có chức năng tạo mới công ty (chỉ quản lý 1 công ty duy nhất)  

### 6.4. Lịch Sử Cập Nhật

- **Phiên bản hiện tại**: Chưa có thông tin
- **Cập nhật gần nhất**: Chưa có thông tin

---

**Tài liệu này được tạo tự động từ source code. Nếu có thắc mắc, vui lòng liên hệ đội phát triển.**

