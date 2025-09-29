# 📋 Hướng Dẫn Sử Dụng - Danh Sách Sản Phẩm/Dịch Vụ

## 🎯 **Tổng Quan**

**Danh Sách Sản Phẩm/Dịch Vụ** là màn hình chính để quản lý và xem danh sách tất cả sản phẩm và dịch vụ trong hệ thống ERP. Màn hình này cung cấp các tính năng tìm kiếm, lọc, phân trang và quản lý dữ liệu một cách hiệu quả.

---

## 🖥️ **Giao Diện Chính**

### **📊 Bảng Dữ Liệu**
- **Cột Mã**: Mã sản phẩm/dịch vụ
- **Cột Tên**: Tên sản phẩm/dịch vụ
- **Cột Loại**: Sản phẩm hoặc Dịch vụ
- **Cột Mô Tả**: Mô tả chi tiết
- **Cột Danh Mục**: Tên danh mục
- **Cột Trạng Thái**: Trạng thái hoạt động
- **Cột Số Biến Thể**: Số lượng biến thể
- **Cột Số Hình Ảnh**: Số lượng hình ảnh
- **Cột Ảnh Đại Diện**: Hình ảnh thumbnail

### **🎨 Màu Sắc Phân Biệt**
- **🟡 Màu Vàng Nhạt**: Sản phẩm
- **🔵 Màu Xanh Nhạt**: Dịch vụ
- **🔴 Chữ Đỏ Gạch Ngang**: Không hoạt động

---

## 🔧 **Thanh Công Cụ (Toolbar)**

### **📋 Nút "Danh Sách"**
- **Chức năng**: Tải lại dữ liệu từ database
- **Cách sử dụng**: Click vào nút "Danh Sách" để refresh dữ liệu
- **Khi nào dùng**: Khi muốn cập nhật dữ liệu mới nhất

### **🔍 Nút "Lọc Dữ Liệu"**
- **Chức năng**: Tìm kiếm toàn diện trong tất cả các cột
- **Cách sử dụng**: 
  1. Click vào nút "Lọc Dữ Liệu"
  2. Chọn loại tìm kiếm:
     - **Tìm kiếm đơn giản**: Nhập 1 từ khóa
     - **Tìm kiếm nâng cao**: Nhập nhiều từ khóa (mỗi dòng 1 từ khóa)
  3. Nhập từ khóa tìm kiếm
  4. Click "OK"
- **Kết quả**: Từ khóa được tô màu đỏ và bold trong kết quả

### **➕ Nút "Mới"**
- **Chức năng**: Thêm sản phẩm/dịch vụ mới
- **Cách sử dụng**: Click vào nút "Mới" để mở form thêm mới
- **Lưu ý**: Form sẽ mở ở chế độ thêm mới

### **✏️ Nút "Điều Chỉnh"**
- **Chức năng**: Chỉnh sửa sản phẩm/dịch vụ đã chọn
- **Cách sử dụng**: 
  1. Chọn 1 dòng dữ liệu (click vào checkbox)
  2. Click vào nút "Điều Chỉnh"
- **Lưu ý**: Chỉ cho phép chọn 1 dòng để chỉnh sửa

### **🗑️ Nút "Xóa"**
- **Chức năng**: Xóa sản phẩm/dịch vụ đã chọn
- **Cách sử dụng**: 
  1. Chọn 1 hoặc nhiều dòng dữ liệu
  2. Click vào nút "Xóa"
  3. Xác nhận xóa trong dialog
- **Lưu ý**: Có thể chọn nhiều dòng để xóa cùng lúc

### **📊 Nút "Đếm Số Lượng"**
- **Chức năng**: Đếm số lượng biến thể và hình ảnh cho sản phẩm/dịch vụ đã chọn
- **Cách sử dụng**: 
  1. Chọn 1 hoặc nhiều dòng dữ liệu
  2. Click vào nút "Đếm Số Lượng"
- **Kết quả**: Cột "Số Biến Thể" và "Số Hình Ảnh" sẽ được cập nhật

### **📤 Nút "Xuất"**
- **Chức năng**: Xuất dữ liệu ra file Excel
- **Cách sử dụng**: Click vào nút "Xuất" khi có dữ liệu hiển thị
- **Kết quả**: File Excel được tạo với tên "ProductServices.xlsx"

---

## 📄 **Thanh Trạng Thái (Status Bar)**

### **📊 Thông Tin Tổng Kết**
- **Tất cả dữ liệu**: Hiển thị khi chọn "Tất cả" trong số dòng
- **Trang X/Y**: Hiển thị trang hiện tại và tổng số trang
- **Hiển thị**: Số dòng hiện tại / Tổng số dòng
- **Sản phẩm**: Số lượng sản phẩm
- **Dịch vụ**: Số lượng dịch vụ
- **Hoạt động**: Số lượng đang hoạt động
- **Không hoạt động**: Số lượng không hoạt động

### **🎯 Thông Tin Lựa Chọn**
- **Chưa chọn dòng nào**: Khi chưa chọn dòng nào
- **Đang chọn 1 dòng**: Khi chọn 1 dòng
- **Đang chọn X dòng**: Khi chọn nhiều dòng

---

## 📑 **Phân Trang (Pagination)**

### **🔢 Chọn Số Dòng Trên Trang**
- **Tùy chọn**: 20, 50, 100, Tất cả
- **Cách sử dụng**: Click vào dropdown "Số dòng" và chọn số lượng
- **Mặc định**: 50 dòng/trang

### **📄 Chuyển Trang**
- **Cách sử dụng**: Click vào dropdown "Trang" và chọn số trang
- **Hiển thị**: Danh sách các trang có sẵn

---

## 🔍 **Tìm Kiếm và Lọc**

### **🔍 Tìm Kiếm Nhanh (Find)**
- **Vị trí**: Thanh tìm kiếm ở dưới cùng của grid
- **Cách sử dụng**: Nhập từ khóa và nhấn Enter
- **Tìm kiếm**: Trong tất cả dữ liệu hiện tại

### **🔍 Tìm Kiếm Toàn Diện**
- **Cách sử dụng**: Click nút "Lọc Dữ Liệu"
- **Tìm kiếm đơn giản**: 
  - Nhập 1 từ khóa
  - Tìm kiếm trong tất cả các cột
- **Tìm kiếm nâng cao**: 
  - Nhập nhiều từ khóa (mỗi dòng 1 từ khóa)
  - Tất cả từ khóa phải có trong cùng 1 dòng
- **Kết quả**: Từ khóa được highlight màu đỏ và bold

### **📋 Lọc Theo Cột**
- **Cách sử dụng**: Click vào icon filter ở header cột
- **Tùy chọn**: 
  - Text filter: Cho cột văn bản
  - List filter: Cho cột có giá trị cố định
  - Checkbox filter: Cho cột boolean

---

## 🎨 **Tính Năng Đặc Biệt**

### **🌈 Highlight Từ Khóa Tìm Kiếm**
- **Màu sắc**: Đỏ và bold
- **Áp dụng**: Khi sử dụng tìm kiếm toàn diện
- **Hiển thị**: Trong các cột Name, Description, CategoryName

### **📱 Responsive Design**
- **Tự động điều chỉnh**: Chiều cao dòng theo nội dung
- **Word wrap**: Text tự động xuống dòng
- **Best fit**: Cột tự động điều chỉnh độ rộng

### **⚡ Performance Optimization**
- **Pagination**: Chỉ tải dữ liệu cần thiết
- **Lazy loading**: Tải ảnh thumbnail khi cần
- **Async operations**: Tất cả thao tác đều bất đồng bộ

---

## 🚀 **Hướng Dẫn Sử Dụng Chi Tiết**

### **📋 Scenario 1: Xem Danh Sách Sản Phẩm**
1. Mở màn hình "Danh Sách Sản Phẩm/Dịch Vụ"
2. Click nút "Danh Sách" để tải dữ liệu
3. Sử dụng phân trang để xem nhiều dữ liệu
4. Quan sát màu sắc để phân biệt sản phẩm/dịch vụ

### **🔍 Scenario 2: Tìm Kiếm Sản Phẩm**
1. Click nút "Lọc Dữ Liệu"
2. Chọn "Tìm kiếm đơn giản"
3. Nhập từ khóa (ví dụ: "Bàn")
4. Click "OK"
5. Quan sát kết quả với từ khóa được highlight

### **✏️ Scenario 3: Chỉnh Sửa Sản Phẩm**
1. Chọn 1 dòng sản phẩm cần chỉnh sửa
2. Click nút "Điều Chỉnh"
3. Chỉnh sửa thông tin trong form
4. Lưu thay đổi
5. Dữ liệu được cập nhật tự động

### **📊 Scenario 4: Đếm Số Lượng**
1. Chọn 1 hoặc nhiều dòng sản phẩm
2. Click nút "Đếm Số Lượng"
3. Chờ hệ thống xử lý
4. Quan sát cột "Số Biến Thể" và "Số Hình Ảnh" được cập nhật

### **🔍 Scenario 5: Tìm Kiếm Nâng Cao**
1. Click nút "Lọc Dữ Liệu"
2. Chọn "Tìm kiếm nâng cao"
3. Nhập nhiều từ khóa (mỗi dòng 1 từ khóa):
   ```
   Bàn
   Gỗ
   Trắng
   ```
4. Click "OK"
5. Kết quả sẽ chứa tất cả 3 từ khóa trong cùng 1 dòng

---

## ⚠️ **Lưu Ý Quan Trọng**

### **🔒 Bảo Mật**
- Chỉ người dùng có quyền mới có thể chỉnh sửa/xóa
- Tất cả thao tác đều được ghi log

### **⚡ Hiệu Suất**
- Dữ liệu lớn được phân trang để tối ưu hiệu suất
- Sử dụng "Tất cả" chỉ khi thực sự cần thiết
- Tìm kiếm toàn diện có thể chậm với dữ liệu lớn

### **💾 Lưu Trữ**
- Dữ liệu được lưu trữ trong database
- Ảnh thumbnail được lưu dưới dạng binary
- Backup dữ liệu định kỳ

### **🔄 Đồng Bộ**
- Dữ liệu được cập nhật real-time
- Nhiều người dùng có thể làm việc đồng thời
- Xung đột dữ liệu được xử lý tự động

---

## 🆘 **Xử Lý Sự Cố**

### **❌ Lỗi Thường Gặp**

#### **"Không có dữ liệu để hiển thị"**
- **Nguyên nhân**: Database trống hoặc lỗi kết nối
- **Giải pháp**: Kiểm tra kết nối database, click "Danh sách" để tải lại

#### **"Vui lòng chọn một dòng để chỉnh sửa"**
- **Nguyên nhân**: Chưa chọn dòng nào hoặc chọn quá nhiều dòng
- **Giải pháp**: Chọn đúng 1 dòng dữ liệu

#### **"Không tìm thấy kết quả nào"**
- **Nguyên nhân**: Từ khóa tìm kiếm không khớp
- **Giải pháp**: Thử từ khóa khác, kiểm tra chính tả

#### **"Lỗi tải dữ liệu"**
- **Nguyên nhân**: Lỗi kết nối hoặc database
- **Giải pháp**: Kiểm tra kết nối mạng, liên hệ quản trị viên

### **🔧 Thao Tác Khắc Phục**
1. **Refresh dữ liệu**: Click nút "Danh sách"
2. **Kiểm tra kết nối**: Đảm bảo kết nối mạng ổn định
3. **Restart ứng dụng**: Đóng và mở lại ứng dụng
4. **Liên hệ hỗ trợ**: Nếu vấn đề vẫn tiếp tục

---

## 📞 **Hỗ Trợ**

### **📧 Liên Hệ**
- **Email**: support@vnserp.com
- **Hotline**: 1900-xxxx
- **Website**: www.vnserp.com

### **📚 Tài Liệu**
- **User Manual**: Hướng dẫn sử dụng chi tiết
- **Video Tutorial**: Video hướng dẫn
- **FAQ**: Câu hỏi thường gặp

### **🔄 Cập Nhật**
- **Version**: 2025.1
- **Last Updated**: 15/01/2025
- **Next Update**: 15/02/2025

---

**🎉 Chúc bạn sử dụng hiệu quả hệ thống Danh Sách Sản Phẩm/Dịch Vụ!**

*Nếu có bất kỳ thắc mắc nào, vui lòng liên hệ bộ phận hỗ trợ.*
