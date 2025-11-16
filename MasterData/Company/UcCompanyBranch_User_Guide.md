# Hướng Dẫn Sử Dụng - Quản Lý Chi Nhánh Công Ty

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

**UcCompanyBranch** (User Control Quản Lý Chi Nhánh Công Ty) là màn hình cho phép bạn:

- **Xem danh sách** tất cả chi nhánh công ty trong hệ thống
- **Tìm kiếm và lọc** chi nhánh theo nhiều tiêu chí
- **Thêm mới** chi nhánh công ty
- **Chỉnh sửa** thông tin chi nhánh đã có
- **Xóa** một hoặc nhiều chi nhánh (với ràng buộc business rules)
- **Xuất dữ liệu** ra file Excel

### 1.2. Mục Đích Sử Dụng

Màn hình này được sử dụng để:

- Quản lý danh sách chi nhánh của công ty
- Theo dõi thông tin liên hệ của từng chi nhánh
- Quản lý trạng thái hoạt động của các chi nhánh
- Xuất báo cáo danh sách chi nhánh

### 1.3. Workflow Sử Dụng

```
1. Mở màn hình → Tự động hiển thị danh sách chi nhánh (nếu đã có dữ liệu)
2. Click "Danh sách" để tải lại dữ liệu từ database
3. Chọn một hoặc nhiều chi nhánh trong danh sách
4. Thực hiện các thao tác:
   - Thêm mới: Click "Mới" → Nhập thông tin → Lưu
   - Sửa: Chọn 1 dòng → Click "Điều chỉnh" → Sửa thông tin → Lưu
   - Xóa: Chọn 1 hoặc nhiều dòng → Click "Xóa" → Xác nhận
   - Xuất Excel: Click "Xuất" → Chọn vị trí lưu file
5. Xem thống kê ở thanh trạng thái phía dưới
```

---

## 2. Hướng Dẫn Sử Dụng

### 2.1. Thanh Công Cụ (Toolbar)

Thanh công cụ nằm ở phía trên màn hình, bao gồm các nút:

#### 2.1.1. 🔄 Danh Sách (ListDataBarButtonItem)

- **Vị trí**: Nút đầu tiên bên trái
- **Biểu tượng**: 📋
- **Chức năng**: 
  - Tải lại toàn bộ danh sách chi nhánh từ database
  - Hiển thị WaitForm trong quá trình tải
  - Tự động cập nhật GridView với dữ liệu mới nhất
  - Xóa selection hiện tại sau khi tải

**Cách sử dụng:**
1. Click nút **"Danh sách"**
2. Đợi WaitForm hiển thị và tải dữ liệu
3. Danh sách sẽ được cập nhật

---

#### 2.1.2. ➕ Mới (NewBarButtonItem)

- **Vị trí**: Nút thứ hai
- **Biểu tượng**: ➕
- **Chức năng**: 
  - Mở form thêm mới chi nhánh công ty
  - Hiển thị overlay trên UserControl
  - Tự động tải lại dữ liệu sau khi đóng form

**Cách sử dụng:**
1. Click nút **"Mới"**
2. Form thêm mới sẽ hiển thị
3. Nhập đầy đủ thông tin bắt buộc
4. Click **"Lưu"** để lưu hoặc **"Đóng"** để hủy
5. Danh sách sẽ tự động tải lại sau khi đóng form

**Lưu ý:**
- Form sẽ tự động lấy CompanyId từ database (vì chỉ có 1 công ty)
- Bạn chỉ cần nhập thông tin chi nhánh

---

#### 2.1.3. ✏️ Điều Chỉnh (EditBarButtonItem)

- **Vị trí**: Nút thứ ba
- **Biểu tượng**: ✏️
- **Chức năng**: 
  - Mở form chỉnh sửa chi nhánh đã chọn
  - Chỉ hoạt động khi chọn đúng **1 dòng**

**Cách sử dụng:**
1. **Chọn 1 dòng** trong danh sách (click vào checkbox hoặc dòng)
2. Click nút **"Điều chỉnh"**
3. Form chỉnh sửa sẽ hiển thị với thông tin đã có
4. Sửa thông tin cần thiết
5. Click **"Lưu"** để lưu hoặc **"Đóng"** để hủy
6. Danh sách sẽ tự động tải lại sau khi đóng form

**Lưu ý:**
- ⚠️ Phải chọn đúng **1 dòng**. Nếu chọn nhiều hơn 1 dòng, hệ thống sẽ yêu cầu bỏ chọn bớt
- Nếu không chọn dòng nào, hệ thống sẽ yêu cầu chọn dòng

---

#### 2.1.4. 🗑️ Xóa (DeleteBarButtonItem)

- **Vị trí**: Nút thứ tư
- **Biểu tượng**: 🗑️
- **Chức năng**: 
  - Xóa một hoặc nhiều chi nhánh đã chọn
  - Validate business rules trước khi xóa
  - Hiển thị dialog xác nhận

**Cách sử dụng:**
1. **Chọn 1 hoặc nhiều dòng** trong danh sách
2. Click nút **"Xóa"**
3. Hệ thống sẽ kiểm tra business rules:
   - ⚠️ **Không cho phép xóa** nếu sẽ không còn chi nhánh nào
   - ⚠️ **Không cho phép xóa** chi nhánh cuối cùng
4. Nếu pass validation, hiển thị dialog xác nhận:
   - **"Bạn có chắc muốn xóa dòng dữ liệu đã chọn?"** (nếu chọn 1 dòng)
   - **"Bạn có chắc muốn xóa X dòng dữ liệu đã chọn?"** (nếu chọn nhiều dòng)
5. Click **"Yes"** để xác nhận hoặc **"No"** để hủy
6. Nếu xác nhận, hệ thống sẽ xóa và tải lại danh sách

**Business Rules:**
- ⚠️ **Công ty phải có ít nhất một chi nhánh**
- ⚠️ **Không thể xóa tất cả chi nhánh**
- ⚠️ **Không thể xóa chi nhánh cuối cùng**

---

#### 2.1.5. 📊 Xuất (ExportBarButtonItem)

- **Vị trí**: Nút cuối cùng
- **Biểu tượng**: 📊
- **Chức năng**: 
  - Xuất toàn bộ dữ liệu trong GridView ra file Excel
  - Hiển thị SaveFileDialog để chọn vị trí lưu

**Cách sử dụng:**
1. Đảm bảo có dữ liệu trong danh sách
2. Click nút **"Xuất"**
3. Hộp thoại **"Lưu file"** sẽ hiển thị
4. Chọn vị trí lưu file (mặc định: `CompanyBranches.xlsx`)
5. Click **"Lưu"**
6. Hệ thống sẽ xuất file Excel và hiển thị thông báo thành công

**Lưu ý:**
- File Excel sẽ chứa toàn bộ dữ liệu đang hiển thị trong GridView
- Bao gồm tất cả các cột đã được cấu hình

---

### 2.2. Bảng Danh Sách (GridView)

#### 2.2.1. Cấu Trúc Bảng

Bảng hiển thị các cột sau:

| Cột | Mô Tả | Ví Dụ |
|-----|-------|-------|
| **Mã chi nhánh** (BranchCode) | Mã định danh chi nhánh | `CN01`, `CN_HCM` |
| **Tên chi nhánh** (BranchName) | Tên đầy đủ chi nhánh | `Chi nhánh TP.HCM`, `Chi nhánh Hà Nội` |
| **Địa chỉ** (Address) | Địa chỉ chi nhánh | `123 Đường ABC, Quận XYZ` |
| **Số điện thoại** (Phone) | Số điện thoại liên hệ | `02812345678` |
| **Email** | Email liên hệ | `hcm@company.com` |
| **Tên người quản lý** (ManagerName) | Tên người quản lý chi nhánh | `Nguyễn Văn A` |
| **Trạng thái hoạt động** (IsActive) | Trạng thái hoạt động | `True` (hoạt động) / `False` (không hoạt động) |

#### 2.2.2. Tính Năng Bảng

**1. Multi-Select (Chọn nhiều dòng):**
- Click vào **checkbox** ở đầu mỗi dòng để chọn
- Có thể chọn nhiều dòng cùng lúc
- Selection được hiển thị ở thanh trạng thái phía dưới

**2. Auto Filter Row (Dòng lọc tự động):**
- Dòng đầu tiên của bảng là dòng lọc
- Nhập giá trị vào ô để lọc dữ liệu
- Lọc theo từng cột riêng biệt

**3. Find Panel (Tìm kiếm):**
- Thanh tìm kiếm luôn hiển thị ở phía trên bảng
- Nhập từ khóa để tìm kiếm trong toàn bộ dữ liệu
- Tìm kiếm theo tất cả các cột

**4. Row Styling (Tô màu dòng):**
- Dòng có **IsActive = False** (không hoạt động) sẽ được tô màu **đỏ**
- Giúp dễ dàng nhận biết chi nhánh không hoạt động

**5. Row Indicator (Số thứ tự):**
- Cột đầu tiên hiển thị số thứ tự dòng (1, 2, 3, ...)
- Tự động cập nhật khi lọc hoặc sắp xếp

**6. Double-Click (Mở form chi tiết):**
- Double-click vào một dòng để mở form chi tiết
- Chỉ hoạt động khi chọn đúng **1 dòng**

---

### 2.3. Thanh Trạng Thái (Status Bar)

Thanh trạng thái nằm ở phía dưới màn hình, hiển thị:

#### 2.3.1. Tổng Kết (DataSummaryBarStaticItem)

- **Nhãn**: "Tổng kết:"
- **Nội dung**: 
  - `"Tổng: X chi nhánh"` (nếu có dữ liệu)
  - `"Chưa có dữ liệu"` (nếu không có dữ liệu)
- **Cập nhật**: Tự động cập nhật khi tải dữ liệu

#### 2.3.2. Đang Chọn (SelectedRowBarStaticItem)

- **Nhãn**: "Đang chọn:"
- **Nội dung**: 
  - `"Đã chọn: X dòng"` (nếu có chọn dòng)
  - `"Chưa chọn dòng nào"` (nếu không chọn dòng nào)
- **Cập nhật**: Tự động cập nhật khi thay đổi selection

---

### 2.4. Phím Tắt

Hiện tại màn hình **không có phím tắt** được cấu hình. Tất cả thao tác được thực hiện bằng chuột.

---

### 2.5. Tooltips (Gợi Ý)

Khi di chuột qua các nút trên thanh công cụ, bạn sẽ thấy tooltip hiển thị:

- **Tiêu đề**: Tên nút với biểu tượng
- **Nội dung**: Hướng dẫn chi tiết về chức năng của nút, bao gồm:
  - Chức năng
  - Quy trình thực hiện
  - Yêu cầu
  - Kết quả
  - Lưu ý

---

## 3. Validation và Xử Lý Lỗi

### 3.1. Danh Sách Lỗi Thường Gặp

#### 3.1.1. Lỗi: "Vui lòng chọn một dòng để chỉnh sửa."

**Nguyên nhân**:
- Bạn đã click nút **"Điều chỉnh"** nhưng chưa chọn dòng nào
- Hoặc đã bỏ chọn tất cả các dòng

**Cách khắc phục**:
1. Click vào **checkbox** hoặc **dòng** trong bảng để chọn
2. Đảm bảo chỉ chọn **1 dòng** (không chọn nhiều hơn)
3. Click lại nút **"Điều chỉnh"**

---

#### 3.1.2. Lỗi: "Chỉ cho phép chỉnh sửa 1 dòng. Vui lòng bỏ chọn bớt."

**Nguyên nhân**:
- Bạn đã chọn **nhiều hơn 1 dòng** và click nút **"Điều chỉnh"**

**Cách khắc phục**:
1. Click vào **checkbox** của các dòng không cần chỉnh sửa để bỏ chọn
2. Chỉ giữ lại **1 dòng** được chọn
3. Click lại nút **"Điều chỉnh"**

---

#### 3.1.3. Lỗi: "Vui lòng chọn ít nhất một dòng để xóa."

**Nguyên nhân**:
- Bạn đã click nút **"Xóa"** nhưng chưa chọn dòng nào

**Cách khắc phục**:
1. Click vào **checkbox** hoặc **dòng** trong bảng để chọn
2. Có thể chọn **1 hoặc nhiều dòng**
3. Click lại nút **"Xóa"**

---

#### 3.1.4. Lỗi: "Không thể xóa tất cả chi nhánh. Công ty phải có ít nhất một chi nhánh."

**Nguyên nhân**:
- Bạn đang cố gắng xóa tất cả chi nhánh trong hệ thống
- Hoặc xóa chi nhánh cuối cùng

**Cách khắc phục**:
1. ⚠️ **Không thể xóa** - Đây là business rule của hệ thống
2. Công ty **phải có ít nhất một chi nhánh**
3. Nếu muốn xóa, bạn phải:
   - Thêm chi nhánh mới trước
   - Sau đó mới xóa chi nhánh cũ

---

#### 3.1.5. Lỗi: "Không thể xóa chi nhánh cuối cùng. Công ty phải có ít nhất một chi nhánh."

**Nguyên nhân**:
- Bạn đang cố gắng xóa chi nhánh cuối cùng trong hệ thống
- Hệ thống chỉ còn 1 chi nhánh và bạn đang chọn xóa chi nhánh đó

**Cách khắc phục**:
1. ⚠️ **Không thể xóa** - Đây là business rule của hệ thống
2. Công ty **phải có ít nhất một chi nhánh**
3. Nếu muốn xóa, bạn phải:
   - Thêm chi nhánh mới trước
   - Sau đó mới xóa chi nhánh cũ

---

#### 3.1.6. Lỗi: "Không có dữ liệu để xuất."

**Nguyên nhân**:
- Bạn đã click nút **"Xuất"** nhưng bảng không có dữ liệu
- Hoặc tất cả dữ liệu đã bị lọc hết

**Cách khắc phục**:
1. Click nút **"Danh sách"** để tải lại dữ liệu
2. Xóa các bộ lọc trong Auto Filter Row (nếu có)
3. Xóa từ khóa trong Find Panel (nếu có)
4. Đảm bảo có ít nhất 1 dòng dữ liệu hiển thị
5. Click lại nút **"Xuất"**

---

#### 3.1.7. Lỗi: "Lỗi tải dữ liệu"

**Nguyên nhân**:
- Lỗi kết nối database
- Lỗi trong quá trình xử lý dữ liệu
- Lỗi network (nếu database ở xa)

**Cách khắc phục**:
1. Kiểm tra kết nối database
2. Kiểm tra network connection
3. Thử lại bằng cách click nút **"Danh sách"**
4. Nếu vẫn lỗi, liên hệ quản trị viên hệ thống

---

#### 3.1.8. Lỗi: "Lỗi xóa dữ liệu"

**Nguyên nhân**:
- Lỗi kết nối database khi xóa
- Chi nhánh đang được sử dụng ở nơi khác (foreign key constraint)
- Lỗi trong quá trình xử lý

**Cách khắc phục**:
1. Kiểm tra xem chi nhánh có đang được sử dụng không
2. Kiểm tra kết nối database
3. Thử lại
4. Nếu vẫn lỗi, liên hệ quản trị viên hệ thống

---

#### 3.1.9. Lỗi: "Lỗi xuất dữ liệu"

**Nguyên nhân**:
- Không có quyền ghi file vào thư mục đã chọn
- Ổ đĩa đầy
- File đang được mở bởi ứng dụng khác

**Cách khắc phục**:
1. Chọn thư mục khác có quyền ghi
2. Kiểm tra dung lượng ổ đĩa
3. Đóng file Excel nếu đang mở
4. Thử lại

---

### 3.2. Hiển Thị Lỗi

- Lỗi được hiển thị qua **MsgBox** (hộp thoại thông báo)
- Thông báo lỗi rõ ràng, dễ hiểu
- Có thể có thông tin chi tiết về nguyên nhân lỗi

---

## 4. Câu Hỏi Thường Gặp (FAQs)

### 4.1. Tại sao không thể xóa tất cả chi nhánh?

**Trả lời**: Đây là business rule của hệ thống. Công ty **phải có ít nhất một chi nhánh**. Nếu muốn xóa chi nhánh, bạn phải thêm chi nhánh mới trước.

---

### 4.2. Làm thế nào để thêm mới chi nhánh?

**Trả lời**: 
1. Click nút **"Mới"** trên thanh công cụ
2. Form thêm mới sẽ hiển thị
3. Nhập đầy đủ thông tin bắt buộc (Mã chi nhánh, Tên chi nhánh)
4. Nhập thông tin tùy chọn (Địa chỉ, Số điện thoại, Email, Tên người quản lý)
5. Chọn trạng thái hoạt động (mặc định: Hoạt động)
6. Click **"Lưu"** để lưu

---

### 4.3. Làm thế nào để chỉnh sửa chi nhánh?

**Trả lời**: 
1. **Chọn 1 dòng** trong bảng (click vào checkbox hoặc dòng)
2. Click nút **"Điều chỉnh"**
3. Form chỉnh sửa sẽ hiển thị với thông tin đã có
4. Sửa thông tin cần thiết
5. Click **"Lưu"** để lưu

**Lưu ý**: Phải chọn đúng **1 dòng**. Nếu chọn nhiều hơn 1 dòng, hệ thống sẽ yêu cầu bỏ chọn bớt.

---

### 4.4. Có thể xóa nhiều chi nhánh cùng lúc không?

**Trả lời**: Có. Bạn có thể:
1. Chọn **nhiều dòng** trong bảng (click vào checkbox của từng dòng)
2. Click nút **"Xóa"**
3. Xác nhận xóa trong dialog

**Lưu ý**: Hệ thống vẫn sẽ kiểm tra business rules. Bạn không thể xóa nếu sẽ không còn chi nhánh nào.

---

### 4.5. Làm thế nào để tìm kiếm chi nhánh?

**Trả lời**: Có 2 cách:

**Cách 1: Sử dụng Find Panel (Thanh tìm kiếm)**
- Thanh tìm kiếm nằm ở phía trên bảng
- Nhập từ khóa vào thanh tìm kiếm
- Hệ thống sẽ tự động lọc dữ liệu theo từ khóa

**Cách 2: Sử dụng Auto Filter Row (Dòng lọc)**
- Dòng đầu tiên của bảng là dòng lọc
- Nhập giá trị vào ô của cột cần lọc
- Hệ thống sẽ lọc dữ liệu theo cột đó

---

### 4.6. Tại sao một số dòng có màu đỏ?

**Trả lời**: Dòng có màu đỏ là dòng có **IsActive = False** (không hoạt động). Đây là cách hệ thống giúp bạn dễ dàng nhận biết chi nhánh không hoạt động.

---

### 4.7. Làm thế nào để xuất dữ liệu ra Excel?

**Trả lời**: 
1. Đảm bảo có dữ liệu trong bảng
2. Click nút **"Xuất"** trên thanh công cụ
3. Chọn vị trí lưu file trong hộp thoại "Lưu file"
4. Click **"Lưu"**
5. File Excel sẽ được tạo tại vị trí đã chọn

---

### 4.8. File Excel xuất ra chứa gì?

**Trả lời**: File Excel sẽ chứa **toàn bộ dữ liệu đang hiển thị** trong GridView, bao gồm:
- Tất cả các cột đã được cấu hình
- Tất cả các dòng đang hiển thị (sau khi lọc, nếu có)

---

### 4.9. Làm thế nào để xem chi tiết một chi nhánh?

**Trả lời**: Có 2 cách:

**Cách 1: Sử dụng nút "Điều chỉnh"**
1. Chọn 1 dòng
2. Click nút **"Điều chỉnh"**

**Cách 2: Double-click vào dòng**
1. Double-click vào dòng cần xem
2. Form chi tiết sẽ hiển thị

---

### 4.10. Tại sao nút "Điều chỉnh" bị vô hiệu hóa (disabled)?

**Trả lời**: Nút **"Điều chỉnh"** chỉ được kích hoạt khi bạn chọn đúng **1 dòng**. Nếu:
- Không chọn dòng nào → Nút bị vô hiệu hóa
- Chọn nhiều hơn 1 dòng → Nút bị vô hiệu hóa
- Chọn đúng 1 dòng → Nút được kích hoạt

---

## 5. Lưu Ý và Bảo Mật

### 5.1. Lưu Ý Chung

- ⚠️ **Công ty phải có ít nhất một chi nhánh**. Hệ thống không cho phép xóa tất cả chi nhánh.
- ⚠️ **Nút "Điều chỉnh" chỉ hoạt động khi chọn đúng 1 dòng**. Nếu chọn nhiều hơn 1 dòng, hệ thống sẽ yêu cầu bỏ chọn bớt.
- ⚠️ **Dữ liệu được tải từ database**. Nếu không thấy dữ liệu, click nút **"Danh sách"** để tải lại.
- ⚠️ **Selection sẽ bị xóa** sau khi tải lại dữ liệu. Nếu đang chọn dòng, hãy thực hiện thao tác trước khi tải lại.

### 5.2. Business Rules

- **Rule 1**: Công ty phải có ít nhất một chi nhánh
- **Rule 2**: Không thể xóa tất cả chi nhánh
- **Rule 3**: Không thể xóa chi nhánh cuối cùng

### 5.3. Bảo Mật

- Thông tin chi nhánh được lưu trữ trong database
- Không có thông tin nhạy cảm nào được xử lý ở đây
- Quyền truy cập được quản lý bởi hệ thống phân quyền (nếu có)

### 5.4. Best Practices

- **Thêm chi nhánh mới trước khi xóa chi nhánh cũ** (nếu muốn thay thế)
- **Kiểm tra trạng thái hoạt động** trước khi xóa (dòng màu đỏ = không hoạt động)
- **Sử dụng Auto Filter Row** để lọc dữ liệu nhanh chóng
- **Xuất Excel định kỳ** để backup dữ liệu

---

## 6. Thông Tin Phiên Bản

### 6.1. Phiên Bản Hiện Tại

- **Tên màn hình**: UcCompanyBranch (User Control Quản Lý Chi Nhánh Công Ty)
- **Module**: MasterData.Company
- **Framework**: DevExpress WinForms
- **Ngôn ngữ**: C#

### 6.2. Tính Năng Hiện Tại

✅ Xem danh sách chi nhánh  
✅ Tìm kiếm và lọc dữ liệu  
✅ Thêm mới chi nhánh  
✅ Chỉnh sửa chi nhánh  
✅ Xóa một hoặc nhiều chi nhánh (với business rules)  
✅ Xuất dữ liệu ra Excel  
✅ Multi-select với checkbox  
✅ Auto filter row  
✅ Find panel  
✅ Row styling (màu đỏ cho dòng không hoạt động)  
✅ Row indicator (số thứ tự)  
✅ Double-click để mở form chi tiết  
✅ Status bar hiển thị thống kê  
✅ SuperToolTips  
✅ WaitForm khi tải dữ liệu  
✅ OverlayManager khi mở form detail  

### 6.3. Hạn Chế

⚠️ Không có phím tắt được cấu hình  
⚠️ Không có chức năng in trực tiếp (phải xuất Excel rồi in)  
⚠️ Không có chức năng import từ Excel  

### 6.4. Lịch Sử Cập Nhật

- **Phiên bản hiện tại**: Chưa có thông tin
- **Cập nhật gần nhất**: Chưa có thông tin

---

**Tài liệu này được tạo tự động từ source code. Nếu có thắc mắc, vui lòng liên hệ đội phát triển.**

