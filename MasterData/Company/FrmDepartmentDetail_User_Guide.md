# Hướng Dẫn Sử Dụng Form Chi Tiết Phòng Ban

## Mục Lục

1. [Giới Thiệu](#1-giới-thiệu)
2. [Cách Sử Dụng Form](#2-cách-sử-dụng-form)
3. [Validation](#3-validation)
4. [Câu Hỏi Thường Gặp (FAQs)](#4-câu-hỏi-thường-gặp-faqs)
5. [Ghi Chú](#5-ghi-chú)
6. [Thông Tin Phiên Bản](#6-thông-tin-phiên-bản)

---

## 1. Giới Thiệu

### 1.1. Chức Năng

Form **Chi Tiết Phòng Ban** (`FrmDepartmentDetail`) cho phép bạn:

- **Tạo mới** phòng ban trong hệ thống
- **Chỉnh sửa** thông tin phòng ban đã có
- Quản lý thông tin chi tiết về phòng ban như: mã phòng ban, tên phòng ban, mô tả, chi nhánh, phòng ban cha, trạng thái hoạt động

### 1.2. Mục Đích Sử Dụng

Form này được sử dụng để:

- Thiết lập cấu trúc tổ chức của công ty
- Tạo cây phân cấp phòng ban (phòng ban cha - phòng ban con)
- Gán phòng ban vào các chi nhánh cụ thể
- Quản lý trạng thái hoạt động của phòng ban

### 1.3. Workflow Sử Dụng

**Khi tạo mới phòng ban:**

1. Mở form → Form hiển thị ở chế độ "Thêm mới phòng ban"
2. Nhập thông tin bắt buộc: **Mã phòng ban**, **Tên phòng ban**, **Chi nhánh**
3. (Tùy chọn) Chọn **Phòng ban cha** nếu phòng ban này thuộc một phòng ban khác
4. (Tùy chọn) Nhập **Mô tả** chi tiết
5. Điều chỉnh **Trạng thái** (mặc định: Đang hoạt động)
6. Click nút **Lưu** để lưu vào database
7. Form tự động đóng sau khi lưu thành công

**Khi chỉnh sửa phòng ban:**

1. Mở form với ID phòng ban → Form hiển thị ở chế độ "Chỉnh sửa phòng ban"
2. Dữ liệu hiện tại được tự động load và hiển thị
3. **Mã phòng ban** bị khóa, không thể thay đổi
4. Chỉnh sửa các trường khác: Tên phòng ban, Chi nhánh, Phòng ban cha, Mô tả, Trạng thái
5. Click nút **Lưu** để cập nhật
6. Form tự động đóng sau khi lưu thành công

---

## 2. Cách Sử Dụng Form

### 2.1. Giao Diện Form

Form được chia thành các phần chính:

- **Thanh công cụ (Toolbar)**: Chứa 2 nút: **Lưu** và **Đóng**
- **Khu vực nhập liệu**: Chứa các trường thông tin được sắp xếp theo layout dọc

### 2.2. Các Trường Thông Tin

#### 2.2.1. Tên Chi Nhánh (Bắt Buộc) ⭐

- **Control**: Dropdown danh sách chi nhánh
- **Vị trí**: Dòng đầu tiên
- **Chức năng**: 
  - Chọn chi nhánh mà phòng ban thuộc về
  - Hiển thị danh sách các chi nhánh đang hoạt động
  - Hiển thị tên chi nhánh và địa chỉ đầy đủ
- **Ràng buộc**: 
  - ⚠️ **Bắt buộc phải chọn** (có dấu * đỏ)
  - Không được để trống
- **Cách sử dụng**:
  1. Click vào ô dropdown
  2. Chọn chi nhánh từ danh sách
  3. Hoặc gõ tên để tìm kiếm

#### 2.2.2. Phòng Ban Cha (Tùy Chọn)

- **Control**: Dropdown dạng cây (TreeList)
- **Vị trí**: Dòng thứ hai
- **Chức năng**:
  - Chọn phòng ban cha để tạo cấu trúc phân cấp
  - Hiển thị danh sách phòng ban dạng cây (có thể có nhiều cấp)
  - Nếu để trống, phòng ban này sẽ là phòng ban cấp cao nhất
- **Ràng buộc**: 
  - ✅ **Không bắt buộc** (có thể để trống)
- **Cách sử dụng**:
  1. Click vào ô dropdown
  2. Chọn phòng ban cha từ cây phân cấp
  3. Để trống nếu không có phòng ban cha

#### 2.2.3. Mã Phòng Ban (Bắt Buộc khi tạo mới) ⭐

- **Control**: Ô nhập text
- **Vị trí**: Dòng thứ ba, bên trái
- **Chức năng**:
  - Nhập mã định danh của phòng ban (ví dụ: PB01, PB02, v.v.)
  - Mã này dùng để phân biệt các phòng ban
- **Ràng buộc**:
  - ⚠️ **Bắt buộc nhập** khi tạo mới (có dấu * đỏ)
  - Tối đa 50 ký tự
  - **Không thể chỉnh sửa** khi đang ở chế độ edit (bị khóa)
- **Cách sử dụng**:
  - Khi tạo mới: Nhập mã phòng ban
  - Khi chỉnh sửa: Mã phòng ban bị khóa, không thể thay đổi

#### 2.2.4. Trạng Thái (Tùy Chọn)

- **Control**: Toggle Switch (công tắc bật/tắt)
- **Vị trí**: Dòng thứ ba, bên phải
- **Chức năng**:
  - Bật/tắt trạng thái hoạt động của phòng ban
  - Hiển thị: "Đang hoạt động" (màu xanh) hoặc "Không hoạt động" (màu đỏ)
- **Giá trị mặc định**: Đang hoạt động (bật)
- **Cách sử dụng**:
  - Click vào công tắc để chuyển đổi trạng thái
  - Màu xanh = Đang hoạt động
  - Màu đỏ = Không hoạt động

#### 2.2.5. Tên Phòng Ban (Bắt Buộc) ⭐

- **Control**: Ô nhập text
- **Vị trí**: Dòng thứ tư
- **Chức năng**:
  - Nhập tên đầy đủ của phòng ban (ví dụ: Phòng Kinh doanh, Phòng Kỹ thuật, v.v.)
- **Ràng buộc**:
  - ⚠️ **Bắt buộc nhập** (có dấu * đỏ)
  - Không được để trống
  - Tối đa 255 ký tự
  - Không được chứa chỉ khoảng trắng
- **Cách sử dụng**:
  - Nhập tên phòng ban vào ô text
  - Hệ thống tự động loại bỏ khoảng trắng đầu/cuối

#### 2.2.6. Mô Tả (Tùy Chọn)

- **Control**: Ô nhập text (có thể nhiều dòng)
- **Vị trí**: Dòng cuối cùng
- **Chức năng**:
  - Nhập mô tả chi tiết về phòng ban
  - Có thể để trống nếu không cần thiết
- **Ràng buộc**:
  - ✅ **Không bắt buộc** (có thể để trống)
  - Tối đa 255 ký tự nếu có nhập
- **Cách sử dụng**:
  - Nhập mô tả vào ô text
  - Có thể nhập nhiều dòng

### 2.3. Các Nút Chức Năng

#### 2.3.1. Nút Lưu 💾

- **Vị trí**: Thanh công cụ, bên trái
- **Icon**: Biểu tượng lưu (save_16x16 / save_32x32)
- **Chức năng**:
  - Validate tất cả dữ liệu đầu vào
  - Lưu thông tin phòng ban vào database
  - Hiển thị thông báo thành công/thất bại
  - Đóng form sau khi lưu thành công
- **Quy trình**:
  1. Kiểm tra validation (Mã phòng ban, Tên phòng ban, Chi nhánh)
  2. Nếu có lỗi: Hiển thị biểu tượng cảnh báo đỏ bên cạnh trường lỗi
  3. Nếu hợp lệ: Lưu vào database
  4. Hiển thị thông báo "Tạo mới phòng ban thành công" hoặc "Cập nhật phòng ban thành công"
  5. Đóng form

#### 2.3.2. Nút Đóng ❌

- **Vị trí**: Thanh công cụ, bên phải
- **Icon**: Biểu tượng hủy (cancel_16x16 / cancel_32x32)
- **Chức năng**:
  - Đóng form ngay lập tức
  - Không lưu dữ liệu đã nhập
  - Không ảnh hưởng đến database
- **Lưu ý**: Tất cả dữ liệu đã nhập sẽ bị mất khi đóng form. Nếu muốn lưu, hãy click nút **Lưu** trước.

### 2.4. Phím Tắt

Hiện tại form không có phím tắt được định nghĩa. Bạn có thể sử dụng:

- **Tab**: Di chuyển giữa các trường nhập liệu
- **Enter**: (Không có chức năng mặc định)
- **Escape**: Đóng form (chức năng mặc định của Windows Form)

### 2.5. SuperToolTip (Tooltip Hướng Dẫn)

Khi di chuột qua các control, bạn sẽ thấy tooltip hiển thị thông tin chi tiết:

- **Mã phòng ban**: Hướng dẫn về ràng buộc, validation, và cách sử dụng
- **Tên phòng ban**: Hướng dẫn về ràng buộc và validation
- **Mô tả**: Hướng dẫn về độ dài tối đa
- **Chi nhánh**: Hướng dẫn về cách chọn chi nhánh
- **Phòng ban cha**: Hướng dẫn về cấu trúc phân cấp
- **Nút Lưu**: Hướng dẫn về quy trình lưu dữ liệu
- **Nút Đóng**: Hướng dẫn về chức năng đóng form

---

## 3. Validation

### 3.1. Các Lỗi Thường Gặp và Cách Khắc Phục

#### ❌ Lỗi 1: "Mã phòng ban không được để trống"

- **Nguyên nhân**: 
  - Bạn đang ở chế độ tạo mới và chưa nhập mã phòng ban
- **Cách khắc phục**:
  1. Nhập mã phòng ban vào ô "Mã phòng ban"
  2. Mã phòng ban phải có ít nhất 1 ký tự (không được để trống)
  3. Click nút **Lưu** lại

#### ❌ Lỗi 2: "Tên phòng ban không được để trống"

- **Nguyên nhân**: 
  - Bạn chưa nhập tên phòng ban hoặc chỉ nhập khoảng trắng
- **Cách khắc phục**:
  1. Nhập tên phòng ban vào ô "Tên phòng ban"
  2. Đảm bảo tên phòng ban không chỉ chứa khoảng trắng
  3. Tên phòng ban không được vượt quá 255 ký tự
  4. Click nút **Lưu** lại

#### ❌ Lỗi 3: "Vui lòng chọn chi nhánh"

- **Nguyên nhân**: 
  - Bạn chưa chọn chi nhánh cho phòng ban
- **Cách khắc phục**:
  1. Click vào ô "Tên chi nhánh"
  2. Chọn một chi nhánh từ danh sách dropdown
  3. Đảm bảo chi nhánh đã được chọn (hiển thị tên chi nhánh trong ô)
  4. Click nút **Lưu** lại

#### ❌ Lỗi 4: "Mã phòng ban không được vượt quá 50 ký tự"

- **Nguyên nhân**: 
  - Bạn đã nhập mã phòng ban quá dài (hơn 50 ký tự)
- **Cách khắc phục**:
  1. Rút ngắn mã phòng ban xuống còn tối đa 50 ký tự
  2. Click nút **Lưu** lại

#### ❌ Lỗi 5: "Tên phòng ban không được vượt quá 255 ký tự"

- **Nguyên nhân**: 
  - Bạn đã nhập tên phòng ban quá dài (hơn 255 ký tự)
- **Cách khắc phục**:
  1. Rút ngắn tên phòng ban xuống còn tối đa 255 ký tự
  2. Click nút **Lưu** lại

#### ❌ Lỗi 6: "Mô tả không được vượt quá 255 ký tự"

- **Nguyên nhân**: 
  - Bạn đã nhập mô tả quá dài (hơn 255 ký tự)
- **Cách khắc phục**:
  1. Rút ngắn mô tả xuống còn tối đa 255 ký tự
  2. Click nút **Lưu** lại

#### ❌ Lỗi 7: "Không tìm thấy phòng ban"

- **Nguyên nhân**: 
  - Phòng ban bạn đang cố chỉnh sửa không còn tồn tại trong database
- **Cách khắc phục**:
  1. Đóng form
  2. Kiểm tra lại danh sách phòng ban
  3. Mở lại form với phòng ban hợp lệ

#### ❌ Lỗi 8: "Không tìm thấy thông tin công ty trong hệ thống"

- **Nguyên nhân**: 
  - Hệ thống không tìm thấy thông tin công ty (chỉ có 1 công ty duy nhất)
- **Cách khắc phục**:
  1. Liên hệ quản trị viên hệ thống
  2. Đảm bảo thông tin công ty đã được thiết lập trong hệ thống

### 3.2. Biểu Tượng Cảnh Báo

Khi có lỗi validation, bạn sẽ thấy:

- **Biểu tượng cảnh báo đỏ** (⚠️) xuất hiện bên cạnh trường có lỗi
- **Tooltip hiển thị thông báo lỗi** khi di chuột qua biểu tượng cảnh báo
- Form sẽ **không đóng** và **không lưu** dữ liệu cho đến khi tất cả lỗi được sửa

### 3.3. Dấu * Đỏ

Các trường có dấu **<color=red>*</color>** là các trường bắt buộc:

- ⭐ **Tên chi nhánh** (bắt buộc)
- ⭐ **Mã phòng ban** (bắt buộc khi tạo mới)
- ⭐ **Tên phòng ban** (bắt buộc)

Các trường không có dấu * là tùy chọn:

- ✅ **Phòng ban cha** (tùy chọn)
- ✅ **Mô tả** (tùy chọn)
- ✅ **Trạng thái** (tùy chọn, mặc định: Đang hoạt động)

---

## 4. Câu Hỏi Thường Gặp (FAQs)

### ❓ Câu Hỏi 1: Tôi có thể thay đổi mã phòng ban sau khi đã tạo không?

**Trả lời**: Không. Mã phòng ban **không thể thay đổi** sau khi đã tạo. Khi bạn mở form ở chế độ chỉnh sửa, trường "Mã phòng ban" sẽ bị khóa và không thể chỉnh sửa. Đây là thiết kế để đảm bảo tính nhất quán của dữ liệu.

### ❓ Câu Hỏi 2: Tôi có thể tạo phòng ban mà không chọn chi nhánh không?

**Trả lời**: Không. Chi nhánh là trường **bắt buộc**. Mỗi phòng ban phải thuộc về một chi nhánh cụ thể. Nếu bạn không chọn chi nhánh, hệ thống sẽ hiển thị lỗi "Vui lòng chọn chi nhánh" và không cho phép lưu.

### ❓ Câu Hỏi 3: Phòng ban cha là gì? Tôi có bắt buộc phải chọn không?

**Trả lời**: Phòng ban cha dùng để tạo **cấu trúc phân cấp** trong tổ chức. Ví dụ: Phòng Kinh doanh có thể có các phòng ban con như Phòng Bán hàng, Phòng Marketing. **Bạn không bắt buộc phải chọn** phòng ban cha. Nếu để trống, phòng ban này sẽ là phòng ban cấp cao nhất.

### ❓ Câu Hỏi 4: Tôi có thể tạo nhiều phòng ban với cùng một mã không?

**Trả lời**: Không. Mã phòng ban phải là **duy nhất** trong hệ thống. Nếu bạn cố gắng tạo phòng ban với mã đã tồn tại, hệ thống sẽ báo lỗi và không cho phép lưu.

### ❓ Câu Hỏi 5: Trạng thái "Không hoạt động" có nghĩa là gì?

**Trả lời**: Khi bạn chuyển trạng thái sang "Không hoạt động", phòng ban này sẽ **không còn hoạt động** trong hệ thống. Có thể dùng để ẩn các phòng ban đã ngừng hoạt động mà không cần xóa khỏi database. Bạn có thể bật lại trạng thái "Đang hoạt động" bất cứ lúc nào.

### ❓ Câu Hỏi 6: Tôi có thể xóa phòng ban từ form này không?

**Trả lời**: Không. Form này chỉ dùng để **tạo mới** và **chỉnh sửa** phòng ban. Để xóa phòng ban, bạn cần sử dụng form danh sách phòng ban (FrmDepartment).

### ❓ Câu Hỏi 7: Tại sao danh sách chi nhánh không hiển thị đầy đủ?

**Trả lời**: Form chỉ hiển thị các chi nhánh **đang hoạt động** (IsActive = true). Nếu bạn không thấy chi nhánh mong muốn, có thể chi nhánh đó đang ở trạng thái "Không hoạt động". Hãy kiểm tra và kích hoạt lại chi nhánh đó.

### ❓ Câu Hỏi 8: Tôi có thể chọn chính phòng ban đó làm phòng ban cha không?

**Trả lời**: Không. Hệ thống sẽ ngăn chặn việc chọn chính phòng ban đó làm phòng ban cha để tránh vòng lặp vô hạn trong cấu trúc phân cấp.

### ❓ Câu Hỏi 9: Form có tự động lưu không?

**Trả lời**: Không. Form **không tự động lưu**. Bạn phải click nút **Lưu** để lưu dữ liệu vào database. Nếu bạn đóng form mà không lưu, tất cả thay đổi sẽ bị mất.

### ❓ Câu Hỏi 10: Tôi nhận được thông báo "Lỗi lưu phòng ban" - Làm thế nào?

**Trả lời**: Có nhiều nguyên nhân có thể gây ra lỗi này:
- Kết nối database bị gián đoạn
- Dữ liệu không hợp lệ (mã phòng ban trùng, v.v.)
- Quyền truy cập database không đủ

**Cách khắc phục**:
1. Kiểm tra kết nối mạng/database
2. Kiểm tra lại dữ liệu đã nhập (đặc biệt là mã phòng ban)
3. Liên hệ quản trị viên hệ thống nếu vấn đề vẫn tiếp tục

---

## 5. Ghi Chú

### 5.1. Lưu Ý Quan Trọng

- ⚠️ **Mã phòng ban không thể thay đổi** sau khi đã tạo. Hãy cẩn thận khi nhập mã phòng ban.
- ⚠️ **Chi nhánh là bắt buộc**. Mỗi phòng ban phải thuộc về một chi nhánh.
- ⚠️ **Form không tự động lưu**. Bạn phải click nút **Lưu** để lưu dữ liệu.
- ⚠️ **Dữ liệu sẽ bị mất** nếu bạn đóng form mà không lưu.

### 5.2. Best Practices

- ✅ **Đặt tên mã phòng ban ngắn gọn, dễ nhớ** (ví dụ: PB01, PB02, v.v.)
- ✅ **Sử dụng cấu trúc phân cấp** để tổ chức phòng ban một cách logic
- ✅ **Nhập mô tả đầy đủ** để dễ dàng quản lý sau này
- ✅ **Kiểm tra kỹ dữ liệu** trước khi click nút Lưu

### 5.3. Bảo Mật

Form này không có chức năng bảo mật đặc biệt (không có Remember Me, password, v.v.). Quyền truy cập được quản lý ở cấp hệ thống.

---

## 6. Thông Tin Phiên Bản

- **Tên Form**: `FrmDepartmentDetail`
- **Namespace**: `MasterData.Company`
- **Phiên bản tài liệu**: 1.0
- **Ngày cập nhật**: 2025-01-15
- **Framework**: .NET Framework 4.8
- **UI Framework**: DevExpress WinForms v25.1

---

**📝 Lưu ý**: Tài liệu này được tạo tự động dựa trên source code. Nếu bạn phát hiện thông tin không chính xác hoặc cần hỗ trợ thêm, vui lòng liên hệ đội phát triển.

