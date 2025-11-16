# Hướng Dẫn Sử Dụng - Quản Lý Danh Mục Đối Tác

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

**UcBusinessPartnerCategory** (User Control Quản Lý Danh Mục Đối Tác) là màn hình cho phép bạn:

- **Xem danh sách** danh mục đối tác dạng cây phân cấp (hierarchical tree)
- **Thêm mới** danh mục đối tác
- **Chỉnh sửa** thông tin danh mục đối tác
- **Xóa** một hoặc nhiều danh mục đối tác
- **Xuất dữ liệu** ra file Excel
- **Tìm kiếm và lọc** danh mục (nếu có)

### 1.2. Mục Đích Sử Dụng

Màn hình này được sử dụng để:

- Quản lý các phân loại đối tác trong hệ thống ERP
- Tổ chức đối tác theo cấu trúc phân cấp (danh mục cha - danh mục con)
- Theo dõi số lượng đối tác thuộc từng danh mục
- Hỗ trợ phân loại và tìm kiếm đối tác hiệu quả

### 1.3. Workflow Sử Dụng

```
1. Màn hình hiển thị danh sách danh mục dạng cây
2. Người dùng chọn danh mục (checkbox hoặc click)
3. Thực hiện thao tác: Thêm mới / Sửa / Xóa / Xuất Excel
4. Hệ thống xử lý và cập nhật dữ liệu
5. Tự động refresh danh sách sau khi thay đổi
```

---

## 2. Hướng Dẫn Sử Dụng

### 2.1. Giao Diện Chính

Màn hình được chia thành 2 phần chính:

#### 2.1.1. Thanh Công Cụ (Toolbar)

Thanh công cụ nằm ở phía trên màn hình, chứa các nút chức năng:

| Nút | Tên | Chức Năng | Khi Nào Sử Dụng |
|-----|-----|-----------|-----------------|
| 🔄 **Danh sách** | `ListDataBarButtonItem` | Tải lại dữ liệu từ hệ thống | Khi muốn refresh danh sách |
| ➕ **Mới** | `NewBarButtonItem` | Thêm mới danh mục | Luôn có thể sử dụng |
| ✏️ **Điều chỉnh** | `EditBarButtonItem` | Chỉnh sửa danh mục | Chỉ khi chọn đúng **1 dòng** |
| 🗑️ **Xóa** | `DeleteBarButtonItem` | Xóa danh mục | Khi chọn **ít nhất 1 dòng** |
| 📊 **Xuất** | `ExportBarButtonItem` | Xuất ra Excel | Khi có dữ liệu hiển thị |

#### 2.1.2. Bảng Dữ Liệu (TreeList)

Bảng hiển thị danh sách danh mục đối tác dạng **cây phân cấp** với các cột:

| Cột | Mô Tả | Ví Dụ |
|-----|-------|-------|
| **Tên phân loại** | Tên danh mục đối tác | "Khách hàng", "Nhà cung cấp" |
| **Mô tả** | Mô tả chi tiết về danh mục | "Danh mục khách hàng nội địa" |
| **Số lượng** | Số lượng đối tác thuộc danh mục | 15, 0, 23 |

**Đặc điểm:**
- Hiển thị dạng **cây phân cấp** (parent-child)
- Có **checkbox** để chọn nhiều dòng
- **Màu sắc** thay đổi theo số lượng đối tác và cấp độ
- Có **số thứ tự** ở cột đầu tiên

### 2.2. Các Thao Tác Cơ Bản

#### 2.2.1. Tải Lại Dữ Liệu

**Cách thực hiện:**
1. Click nút **🔄 Danh sách** trên thanh công cụ
2. Hệ thống sẽ hiển thị màn hình chờ (WaitForm) trong khi tải dữ liệu
3. Danh sách được cập nhật sau khi tải xong

**Khi nào cần:**
- Sau khi thêm/sửa/xóa danh mục
- Khi dữ liệu có thể đã thay đổi từ nơi khác
- Khi muốn refresh toàn bộ danh sách

#### 2.2.2. Thêm Mới Danh Mục

**Cách thực hiện:**
1. Click nút **➕ Mới** trên thanh công cụ
2. Màn hình **FrmBusinessPartnerCategoryDetail** sẽ mở ra
3. Nhập thông tin danh mục:
   - **Tên phân loại** ⭐ (bắt buộc, tối đa 100 ký tự)
   - **Mô tả** (tùy chọn, tối đa 255 ký tự)
   - **Danh mục cha** (tùy chọn - để tạo danh mục con)
4. Click **Lưu** để lưu dữ liệu
5. Màn hình tự động đóng và refresh danh sách

**Lưu ý:**
- Tên phân loại không được trùng với danh mục khác
- Có thể tạo danh mục gốc (không có danh mục cha) hoặc danh mục con

#### 2.2.3. Chỉnh Sửa Danh Mục

**Cách thực hiện:**
1. **Chọn đúng 1 dòng** danh mục cần sửa (bằng checkbox hoặc click)
2. Click nút **✏️ Điều chỉnh** trên thanh công cụ
3. Màn hình **FrmBusinessPartnerCategoryDetail** sẽ mở ra với dữ liệu đã có
4. Chỉnh sửa thông tin cần thiết
5. Click **Lưu** để cập nhật
6. Màn hình tự động đóng và refresh danh sách

**Lưu ý:**
- ⚠️ Chỉ có thể sửa **1 dòng** tại một thời điểm
- Nếu chọn nhiều hơn 1 dòng, hệ thống sẽ thông báo: "Chỉ cho phép chỉnh sửa 1 dòng. Vui lòng bỏ chọn bớt."
- Nếu không chọn dòng nào, hệ thống sẽ thông báo: "Vui lòng chọn một dòng để chỉnh sửa."

#### 2.2.4. Xóa Danh Mục

**Cách thực hiện:**
1. **Chọn 1 hoặc nhiều dòng** danh mục cần xóa (bằng checkbox)
2. Click nút **🗑️ Xóa** trên thanh công cụ
3. Hệ thống hiển thị hộp thoại xác nhận:
   - Nếu chọn 1 dòng: "Bạn có chắc muốn xóa dòng dữ liệu đã chọn?"
   - Nếu chọn nhiều dòng: "Bạn có chắc muốn xóa {số lượng} dòng dữ liệu đã chọn?"
4. Click **Có** để xác nhận xóa
5. Hệ thống sẽ xóa theo thứ tự: **con trước, cha sau** (để tránh lỗi foreign key)
6. Tự động refresh danh sách sau khi xóa

**Lưu ý:**
- ⚠️ Có thể xóa **nhiều dòng** cùng lúc
- Hệ thống tự động xử lý thứ tự xóa để tránh lỗi
- Nếu không chọn dòng nào, hệ thống sẽ thông báo: "Vui lòng chọn ít nhất một dòng để xóa."

#### 2.2.5. Xuất Dữ Liệu Ra Excel

**Cách thực hiện:**
1. Đảm bảo có dữ liệu hiển thị trên bảng
2. Click nút **📊 Xuất** trên thanh công cụ
3. Hộp thoại **SaveFileDialog** sẽ mở ra
4. Chọn vị trí lưu file và đặt tên file (mặc định: `BusinessPartnerCategories.xlsx`)
5. Click **Lưu** để xuất
6. Hệ thống thông báo: "Xuất dữ liệu thành công!"

**Lưu ý:**
- ⚠️ Chỉ xuất được khi có dữ liệu hiển thị
- Nếu không có dữ liệu, hệ thống sẽ thông báo: "Không có dữ liệu để xuất."
- File Excel sẽ chứa tất cả các cột hiển thị trên TreeList

### 2.3. Chọn Dữ Liệu

#### 2.3.1. Chọn Bằng Checkbox

- Click vào **checkbox** ở đầu mỗi dòng để chọn/bỏ chọn
- Có thể chọn **nhiều dòng** cùng lúc
- Checkbox hỗ trợ **recursive checking** (chọn cha sẽ tự động chọn con)

#### 2.3.2. Chọn Bằng Click

- Click vào **dòng dữ liệu** để chọn
- Có thể chọn **nhiều dòng** bằng cách giữ **Ctrl** và click
- Có thể chọn **một khoảng** bằng cách giữ **Shift** và click

### 2.4. Màu Sắc Hiển Thị

Hệ thống tự động tô màu các dòng dựa trên:

#### 2.4.1. Danh Mục Gốc (Level 0)

| Điều Kiện | Màu Nền |
|-----------|---------|
| Không có đối tác (PartnerCount = 0) | **LightGray** (Xám nhạt) |
| Có đối tác (PartnerCount > 0) | **LightBlue** (Xanh nhạt) |

#### 2.4.2. Danh Mục Con (Level > 0)

| Số Lượng Đối Tác | Màu Nền |
|------------------|---------|
| 0 đối tác | **Very Light Gray** (Xám rất nhạt) |
| 1-5 đối tác | **LightYellow** (Vàng nhạt) |
| 6-20 đối tác | **LightGreen** (Xanh lá nhạt) |
| > 20 đối tác | **LightCyan** (Xanh dương nhạt) |

**Lưu ý:** Màu sắc sẽ không hiển thị khi dòng đang được chọn (để giữ màu chọn mặc định của DevExpress).

### 2.5. Tooltips (Gợi Ý)

Khi di chuột qua các nút trên thanh công cụ, bạn sẽ thấy tooltip hiển thị:

- **🔄 Tải dữ liệu**: "Tải lại danh sách danh mục đối tác từ hệ thống."
- **➕ Thêm mới**: "Thêm mới danh mục đối tác vào hệ thống."
- **✏️ Sửa**: "Chỉnh sửa thông tin danh mục đối tác đã chọn."
- **🗑️ Xóa**: "Xóa các danh mục đối tác đã chọn khỏi hệ thống."
- **📊 Xuất Excel**: "Xuất danh sách danh mục đối tác ra file Excel."

---

## 3. Validation và Xử Lý Lỗi

### 3.1. Danh Sách Lỗi Thường Gặp

#### 3.1.1. Lỗi: "Vui lòng chọn một dòng để chỉnh sửa."

**Nguyên nhân:**
- Bạn đã click nút **✏️ Điều chỉnh** nhưng chưa chọn dòng nào

**Cách khắc phục:**
1. Chọn **1 dòng** danh mục cần sửa (bằng checkbox hoặc click)
2. Click lại nút **✏️ Điều chỉnh**

---

#### 3.1.2. Lỗi: "Chỉ cho phép chỉnh sửa 1 dòng. Vui lòng bỏ chọn bớt."

**Nguyên nhân:**
- Bạn đã chọn **nhiều hơn 1 dòng** và click nút **✏️ Điều chỉnh**

**Cách khắc phục:**
1. Bỏ chọn các dòng thừa, chỉ giữ lại **1 dòng**
2. Click lại nút **✏️ Điều chỉnh**

---

#### 3.1.3. Lỗi: "Vui lòng chọn ít nhất một dòng để xóa."

**Nguyên nhân:**
- Bạn đã click nút **🗑️ Xóa** nhưng chưa chọn dòng nào

**Cách khắc phục:**
1. Chọn **ít nhất 1 dòng** danh mục cần xóa (bằng checkbox)
2. Click lại nút **🗑️ Xóa**

---

#### 3.1.4. Lỗi: "Không có dữ liệu để xuất."

**Nguyên nhân:**
- Bạn đã click nút **📊 Xuất** nhưng bảng không có dữ liệu hiển thị

**Cách khắc phục:**
1. Click nút **🔄 Danh sách** để tải dữ liệu
2. Đảm bảo có dữ liệu hiển thị trên bảng
3. Click lại nút **📊 Xuất**

---

#### 3.1.5. Lỗi: "Không thể xác định dòng được chọn để chỉnh sửa."

**Nguyên nhân:**
- Hệ thống không tìm thấy dữ liệu tương ứng với dòng đã chọn (có thể do dữ liệu đã bị xóa hoặc thay đổi)

**Cách khắc phục:**
1. Click nút **🔄 Danh sách** để tải lại dữ liệu
2. Chọn lại dòng cần sửa
3. Click lại nút **✏️ Điều chỉnh**

---

#### 3.1.6. Lỗi: "Lỗi tải dữ liệu"

**Nguyên nhân:**
- Có lỗi xảy ra khi tải dữ liệu từ database (kết nối database, lỗi query, v.v.)

**Cách khắc phục:**
1. Kiểm tra kết nối database
2. Thử lại bằng cách click nút **🔄 Danh sách**
3. Nếu vẫn lỗi, liên hệ quản trị viên hệ thống

---

#### 3.1.7. Lỗi: "Lỗi xóa dữ liệu"

**Nguyên nhân:**
- Có lỗi xảy ra khi xóa dữ liệu (foreign key constraint, lỗi database, v.v.)

**Cách khắc phục:**
1. Kiểm tra xem danh mục có đang được sử dụng ở nơi khác không
2. Thử xóa lại
3. Nếu vẫn lỗi, liên hệ quản trị viên hệ thống

---

#### 3.1.8. Lỗi: "Lỗi xuất dữ liệu"

**Nguyên nhân:**
- Có lỗi xảy ra khi xuất dữ liệu ra Excel (quyền ghi file, đường dẫn không hợp lệ, v.v.)

**Cách khắc phục:**
1. Kiểm tra quyền ghi file tại thư mục đích
2. Chọn đường dẫn khác để lưu file
3. Đảm bảo có đủ dung lượng ổ đĩa

---

### 3.2. Validation Trong Form Chi Tiết

Khi thêm mới hoặc chỉnh sửa danh mục, các validation sau sẽ được áp dụng:

#### 3.2.1. Tên Phân Loại (CategoryName)

- ⚠️ **Bắt buộc nhập** (có dấu * đỏ)
- Tối đa **100 ký tự**
- **Không được trùng** với tên danh mục khác (trừ bản ghi đang chỉnh sửa)

#### 3.2.2. Mô Tả (Description)

- Không bắt buộc
- Tối đa **255 ký tự** (nếu có nhập)

---

## 4. Câu Hỏi Thường Gặp (FAQs)

### 4.1. Tại sao danh mục hiển thị dạng cây phân cấp?

**Trả lời**: Hệ thống hỗ trợ cấu trúc phân cấp (parent-child) để tổ chức danh mục đối tác một cách logic. Danh mục gốc (không có danh mục cha) sẽ hiển thị ở cấp 0, danh mục con sẽ hiển thị lùi vào bên trong.

---

### 4.2. Làm thế nào để tạo danh mục con?

**Trả lời**: 
1. Click nút **➕ Mới** để thêm mới danh mục
2. Trong form chi tiết, chọn **Danh mục cha** từ dropdown
3. Nhập thông tin danh mục con
4. Click **Lưu**

---

### 4.3. Có thể xóa nhiều danh mục cùng lúc không?

**Trả lời**: Có. Bạn có thể chọn nhiều danh mục bằng checkbox và click nút **🗑️ Xóa**. Hệ thống sẽ tự động xóa theo thứ tự: con trước, cha sau để tránh lỗi.

---

### 4.4. Tại sao một số dòng có màu khác nhau?

**Trả lời**: Màu sắc được sử dụng để phân biệt:
- **Danh mục gốc**: Xám nhạt (không có đối tác) hoặc Xanh nhạt (có đối tác)
- **Danh mục con**: Màu thay đổi theo số lượng đối tác (Vàng nhạt, Xanh lá nhạt, Xanh dương nhạt)

---

### 4.5. Số lượng đối tác được tính như thế nào?

**Trả lời**: Số lượng đối tác được đếm từ bảng mapping giữa đối tác và danh mục. Mỗi danh mục sẽ hiển thị tổng số đối tác trực tiếp thuộc danh mục đó.

---

### 4.6. Có thể sửa nhiều danh mục cùng lúc không?

**Trả lời**: Không. Hệ thống chỉ cho phép sửa **1 danh mục** tại một thời điểm để đảm bảo tính chính xác và tránh nhầm lẫn.

---

### 4.7. File Excel xuất ra có định dạng gì?

**Trả lời**: File Excel được xuất ra với định dạng **.xlsx** (Excel 2007 trở lên). File sẽ chứa tất cả các cột hiển thị trên TreeList, bao gồm: Tên phân loại, Mô tả, Số lượng.

---

### 4.8. Tại sao không thể xóa một số danh mục?

**Trả lời**: Có thể do:
- Danh mục đang được sử dụng bởi đối tác (foreign key constraint)
- Danh mục có danh mục con (cần xóa con trước)
- Lỗi kết nối database

Hệ thống sẽ tự động xử lý thứ tự xóa (con trước, cha sau) để tránh lỗi.

---

### 4.9. Làm thế nào để tìm kiếm danh mục?

**Trả lời**: Hiện tại màn hình chưa có chức năng tìm kiếm. Bạn có thể:
- Sử dụng tính năng tìm kiếm của TreeList (nếu có)
- Xuất ra Excel và tìm kiếm trong file Excel
- Cuộn và tìm thủ công trên bảng

---

### 4.10. Có giới hạn số cấp danh mục không?

**Trả lời**: Hệ thống hỗ trợ cấu trúc phân cấp không giới hạn về lý thuyết, nhưng để đảm bảo hiệu suất và dễ quản lý, nên giới hạn ở **3-4 cấp** là hợp lý.

---

## 5. Lưu Ý và Bảo Mật

### 5.1. Lưu Ý Chung

- ⚠️ **Tên phân loại không được trùng** với danh mục khác trong hệ thống
- ⚠️ Khi xóa danh mục, hệ thống sẽ **tự động xóa theo thứ tự** (con trước, cha sau)
- ⚠️ **Số lượng đối tác** được cập nhật tự động khi tải dữ liệu
- ⚠️ **Màu sắc** chỉ hiển thị khi dòng không được chọn

### 5.2. Best Practices

- **Đặt tên danh mục**: Nên đặt tên ngắn gọn, rõ ràng, dễ hiểu
- **Cấu trúc phân cấp**: Nên tổ chức theo logic nghiệp vụ, không quá sâu
- **Mô tả**: Nên điền mô tả để dễ quản lý và tìm kiếm sau này
- **Xóa danh mục**: Nên kiểm tra số lượng đối tác trước khi xóa

### 5.3. Bảo Mật

- Không có thông tin nhạy cảm nào được lưu trữ ở đây
- Quyền truy cập được quản lý bởi hệ thống phân quyền của ERP
- Dữ liệu được lưu trữ trong database với các ràng buộc an toàn

---

## 6. Thông Tin Phiên Bản

### 6.1. Phiên Bản Hiện Tại

- **Tên màn hình**: UcBusinessPartnerCategory (User Control Quản Lý Danh Mục Đối Tác)
- **Module**: MasterData.Customer
- **Framework**: DevExpress WinForms
- **Ngôn ngữ**: C#

### 6.2. Tính Năng Hiện Tại

✅ Hiển thị danh sách danh mục dạng cây phân cấp  
✅ Thêm mới danh mục  
✅ Chỉnh sửa danh mục  
✅ Xóa một hoặc nhiều danh mục  
✅ Xuất dữ liệu ra Excel  
✅ Chọn nhiều dòng bằng checkbox  
✅ Màu sắc phân biệt theo số lượng đối tác  
✅ Tooltips hướng thị  
✅ Async operations với splash screen  
✅ Tự động refresh sau khi thay đổi  

### 6.3. Hạn Chế

⚠️ Chưa có chức năng tìm kiếm/filter trực tiếp trên màn hình  
⚠️ Chưa có chức năng sắp xếp tùy chỉnh  
⚠️ Chưa có chức năng import từ Excel  

### 6.4. Lịch Sử Cập Nhật

- **Phiên bản hiện tại**: Chưa có thông tin
- **Cập nhật gần nhất**: Chưa có thông tin

---

**Tài liệu này được tạo tự động từ source code. Nếu có thắc mắc, vui lòng liên hệ đội phát triển.**

