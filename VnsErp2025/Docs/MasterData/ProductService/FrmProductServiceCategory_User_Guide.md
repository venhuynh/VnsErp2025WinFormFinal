# Hướng Dẫn Sử Dụng - Quản Lý Phân Loại Hàng Hóa Dịch Vụ

## Mục Đích

Màn hình **"Phân loại hàng hóa dịch vụ"** giúp bạn:

- Xem danh sách tất cả phân loại hàng hóa/dịch vụ trong hệ thống theo cấu trúc cây (cha-con)
- Thêm mới phân loại hàng hóa/dịch vụ
- Chỉnh sửa thông tin phân loại hàng hóa/dịch vụ
- Xóa một hoặc nhiều phân loại hàng hóa/dịch vụ
- Xuất danh sách phân loại ra file Excel
- Xem thông tin tổng hợp (tổng số, số đã chọn, số sản phẩm/dịch vụ)

**Lưu ý quan trọng**: Khi xóa một phân loại, tất cả sản phẩm/dịch vụ thuộc phân loại đó sẽ được tự động chuyển sang **"Phân loại chưa đặt tên"**.

---

## Các Bước Thao Tác

### 1. Mở Màn Hình Quản Lý Phân Loại Hàng Hóa Dịch Vụ

**Bước 1**: Từ menu chính của hệ thống, tìm và chọn mục **"Phân loại hàng hóa dịch vụ"** hoặc **"Danh mục sản phẩm/dịch vụ"**

**Bước 2**: Màn hình sẽ mở ra với bảng danh sách phân loại

**Bước 3**: Lần đầu mở màn hình, bạn cần nhấn nút **"Danh sách"** để tải dữ liệu

---

### 2. Tải Danh Sách Phân Loại

**Bước 1**: Nhấn nút **"Danh sách"** (biểu tượng 🔄) trên thanh công cụ phía trên

**Bước 2**: Hệ thống sẽ hiển thị màn hình chờ trong vài giây để tải dữ liệu từ máy chủ

**Bước 3**: Sau khi tải xong, bạn sẽ thấy:
- Danh sách phân loại được hiển thị trong bảng theo cấu trúc cây (cha-con)
- Mỗi dòng hiển thị thông tin của một phân loại
- Ở phía dưới màn hình (thanh trạng thái), bạn sẽ thấy:
  - **"Tổng kết"**: Tổng số phân loại, số đang hoạt động, số không hoạt động, tổng số sản phẩm/dịch vụ
  - **"Đang chọn"**: Thông tin về dòng đang được chọn (tên, mã, trạng thái, số sản phẩm/dịch vụ) hoặc "Chưa chọn dòng nào"

**Lưu ý**: 
- Lần đầu mở màn hình, bạn cần nhấn nút "Danh sách" để tải dữ liệu
- Bạn có thể cuộn chuột để xem thêm các dòng khác
- Danh sách được sắp xếp theo thứ tự ưu tiên (SortOrder) và tên phân loại
- Các phân loại được hiển thị theo cấu trúc phân cấp (cha-con)

---

### 3. Thêm Mới Phân Loại

**Bước 1**: Nhấn nút **"Mới"** (biểu tượng ➕) trên thanh công cụ

**Bước 2**: Màn hình chi tiết sẽ hiển thị để bạn nhập thông tin phân loại mới

**Bước 3**: Điền đầy đủ thông tin theo yêu cầu (xem hướng dẫn trong màn hình chi tiết)

**Bước 4**: Nhấn nút **"Lưu"** trong màn hình chi tiết

**Bước 5**: Sau khi lưu thành công:
- Màn hình chi tiết sẽ tự động đóng
- Danh sách sẽ tự động tải lại và phân loại mới sẽ xuất hiện

**Lưu ý**: 
- Nếu bạn muốn hủy, nhấn nút **"Đóng"** trong màn hình chi tiết
- Màn hình chi tiết sẽ hiển thị ở chế độ modal, bạn phải đóng nó trước khi có thể thao tác với danh sách

---

### 4. Chỉnh Sửa Phân Loại

**Bước 1**: Chọn một dòng phân loại bạn muốn chỉnh sửa bằng cách:
- Nhấn vào dòng để chọn (dòng được chọn sẽ có màu nền khác)
- Hoặc đánh dấu checkbox ở đầu dòng

**Bước 2**: Đảm bảo chỉ chọn **một dòng duy nhất**

**Bước 3**: Nhấn nút **"Điều chỉnh"** (biểu tượng ✏️) trên thanh công cụ

**Bước 4**: Màn hình chi tiết sẽ hiển thị với thông tin đã được điền sẵn

**Bước 5**: Chỉnh sửa thông tin cần thiết

**Bước 6**: Nhấn nút **"Lưu"** trong màn hình chi tiết

**Bước 7**: Sau khi lưu thành công:
- Màn hình chi tiết sẽ tự động đóng
- Danh sách sẽ tự động tải lại với thông tin đã cập nhật

**Lưu ý**: 
- Bạn chỉ có thể chỉnh sửa **một phân loại** tại một thời điểm
- Nếu bạn chọn nhiều hơn 1 dòng, hệ thống sẽ thông báo: "Chỉ cho phép chỉnh sửa 1 dòng. Vui lòng bỏ chọn bớt."
- Nếu bạn không chọn dòng nào, hệ thống sẽ thông báo: "Vui lòng chọn một dòng để chỉnh sửa."
- Nút "Điều chỉnh" chỉ được kích hoạt khi bạn chọn đúng 1 dòng

---

### 5. Xóa Phân Loại

**Bước 1**: Chọn một hoặc nhiều dòng phân loại bạn muốn xóa bằng cách:
- Nhấn vào dòng để chọn
- Hoặc đánh dấu checkbox ở đầu dòng
- Bạn có thể chọn nhiều dòng bằng cách giữ phím **Ctrl** và nhấn vào các dòng khác
- Hoặc đánh dấu checkbox của nhiều dòng

**Bước 2**: Nhấn nút **"Xóa"** (biểu tượng 🗑️) trên thanh công cụ

**Bước 3**: Hệ thống sẽ hiển thị hộp thoại xác nhận:
- Nếu chọn 1 dòng: "Bạn có chắc muốn xóa dòng dữ liệu đã chọn? (Sản phẩm/dịch vụ sẽ được chuyển sang 'Phân loại chưa đặt tên')"
- Nếu chọn nhiều dòng: "Bạn có chắc muốn xóa X dòng dữ liệu đã chọn? (Sản phẩm/dịch vụ sẽ được chuyển sang 'Phân loại chưa đặt tên')"

**Bước 4**: 
- Nhấn **"Có"** (Yes) nếu bạn chắc chắn muốn xóa
- Nhấn **"Không"** (No) nếu bạn muốn hủy thao tác

**Bước 5**: Nếu bạn chọn "Có", hệ thống sẽ:
- Hiển thị màn hình chờ trong vài giây
- Xóa các phân loại đã chọn (xóa con trước, cha sau để tránh lỗi)
- Tự động chuyển tất cả sản phẩm/dịch vụ thuộc các phân loại đã xóa sang "Phân loại chưa đặt tên"
- Tải lại danh sách

**Lưu ý quan trọng**:
- **Thao tác xóa không thể hoàn tác**, vì vậy hãy cẩn thận khi xóa
- Bạn có thể xóa nhiều phân loại cùng lúc
- Tất cả sản phẩm/dịch vụ thuộc phân loại đã xóa sẽ được tự động chuyển sang "Phân loại chưa đặt tên"
- Nút "Xóa" chỉ được kích hoạt khi bạn chọn ít nhất 1 dòng

---

### 6. Xuất Danh Sách Ra Excel

**Bước 1**: Đảm bảo bạn đã tải danh sách (có dữ liệu hiển thị)

**Bước 2**: Nhấn nút **"Xuất"** (biểu tượng 📊) trên thanh công cụ

**Lưu ý**: Nếu chưa có dữ liệu, hệ thống sẽ thông báo "Không có dữ liệu để xuất"

**Bước 3**: Hộp thoại lưu file sẽ hiển thị:
- Chọn vị trí bạn muốn lưu file Excel
- Tên file mặc định: "ProductServiceCategories.xlsx" (bạn có thể thay đổi)
- Nhấn nút **"Lưu"** (Save)

**Bước 4**: Hệ thống sẽ tự động tạo file Excel và hiển thị thông báo: "Xuất dữ liệu thành công!"

**Bước 5**: Mở file Excel tại vị trí đã chọn để xem kết quả

**Lưu ý**: 
- File Excel sẽ chứa tất cả dữ liệu đang hiển thị trong bảng
- Bạn có thể mở file bằng Microsoft Excel, LibreOffice Calc hoặc các phần mềm tương thích
- Nút "Xuất" chỉ được kích hoạt khi có dữ liệu hiển thị

---

## Hiểu Về Giao Diện

### Thanh Công Cụ (Phía Trên)

Các nút chức năng chính:
- **🔄 Danh sách**: Tải lại danh sách từ máy chủ
- **➕ Mới**: Thêm mới phân loại
- **✏️ Điều chỉnh**: Chỉnh sửa phân loại đã chọn (chỉ kích hoạt khi chọn đúng 1 dòng)
- **🗑️ Xóa**: Xóa phân loại đã chọn (chỉ kích hoạt khi chọn ít nhất 1 dòng)
- **📊 Xuất**: Xuất danh sách ra Excel (chỉ kích hoạt khi có dữ liệu)

**Trạng thái nút tự động**:
- Nút "Điều chỉnh" chỉ bật khi bạn chọn đúng 1 dòng
- Nút "Xóa" chỉ bật khi bạn chọn ít nhất 1 dòng
- Nút "Xuất" chỉ bật khi có dữ liệu hiển thị
- Nút "Mới" và "Danh sách" luôn hoạt động

### Bảng Danh Sách (GridView)

**Mỗi dòng hiển thị**:
- **Số thứ tự**: Hiển thị ở cột đầu tiên (1, 2, 3, ...)
- **Checkbox**: Ở đầu dòng để chọn dòng
- **Đường dẫn**: Hiển thị đường dẫn phân cấp của phân loại (ví dụ: "Danh mục cha > Danh mục con")
- **Thông tin danh mục**: Hiển thị đầy đủ thông tin của phân loại (tên, mã, trạng thái, số sản phẩm/dịch vụ)
- **Mô tả**: Mô tả chi tiết của phân loại (nếu có)

**Màu sắc**:
- **Phân loại không hoạt động**: Hiển thị màu chữ đỏ để dễ nhận biết
- **Dòng được chọn**: Có màu nền khác biệt
- **Dòng chẵn/lẻ**: Có màu nền xen kẽ để dễ đọc

**Tính năng**:
- **Chọn nhiều dòng**: Giữ phím Ctrl và nhấn vào các dòng khác, hoặc đánh dấu checkbox của nhiều dòng
- **Cuộn**: Có thể cuộn để xem thêm các dòng khác
- **Tự động điều chỉnh chiều cao dòng**: Dòng sẽ tự động mở rộng để hiển thị đầy đủ nội dung

### Thanh Trạng Thái (Phía Dưới)

Hiển thị thông tin:

**Tổng kết**:
- **Tổng số**: Tổng số phân loại trong danh sách
- **Hoạt động**: Số phân loại đang hoạt động (màu xanh)
- **Không hoạt động**: Số phân loại không hoạt động (màu đỏ)
- **Sản phẩm/DV**: Tổng số sản phẩm/dịch vụ trong tất cả phân loại (màu cam)

**Đang chọn**:
- Khi chưa chọn: "Chưa chọn dòng nào" (màu xám)
- Khi chọn 1 dòng: Hiển thị chi tiết:
  - Số dòng đã chọn (màu xanh)
  - Tên phân loại (màu xanh, in đậm)
  - Mã phân loại (màu xám, trong ngoặc đơn)
  - Trạng thái: "Hoạt động" (màu xanh) hoặc "Ngừng" (màu đỏ)
  - Số sản phẩm/DV: Số lượng sản phẩm/dịch vụ thuộc phân loại này (màu cam)
- Khi chọn nhiều dòng: "Đang chọn X dòng" (màu xanh, in đậm)

---

## Lưu Ý Quan Trọng

### 1. Về Tải Dữ Liệu

- Luôn nhấn nút "Danh sách" để cập nhật dữ liệu mới nhất từ máy chủ
- Hệ thống sẽ tự động tải lại sau các thao tác thêm, sửa, xóa
- Nếu dữ liệu không cập nhật, hãy thử nhấn nút "Danh sách" lại

### 2. Về Chọn Dòng

- **Chọn một dòng**: Nhấn vào dòng hoặc đánh dấu checkbox
- **Chọn nhiều dòng**: Giữ phím Ctrl và nhấn vào các dòng khác, hoặc đánh dấu checkbox của nhiều dòng
- Dòng được chọn sẽ có màu nền khác biệt
- Thông tin chi tiết về dòng đã chọn hiển thị ở thanh trạng thái phía dưới

### 3. Về Xóa Phân Loại

- **Thao tác xóa không thể hoàn tác**, vì vậy hãy cẩn thận khi xóa
- Bạn có thể xóa nhiều phân loại cùng lúc
- **Tất cả sản phẩm/dịch vụ** thuộc phân loại đã xóa sẽ được **tự động chuyển sang "Phân loại chưa đặt tên"**
- Hệ thống sẽ tự động xóa theo thứ tự: con trước, cha sau để tránh lỗi
- Nếu một số phân loại không thể xóa (do đang được sử dụng), hệ thống sẽ thông báo

### 4. Về Xuất Excel

- Xuất tất cả dữ liệu đang hiển thị trong bảng
- File được lưu tại vị trí bạn chọn với tên file bạn đặt
- File Excel có thể chỉnh sửa, in ấn, chia sẻ

### 5. Về Chỉnh Sửa

- Bạn chỉ có thể chỉnh sửa **một phân loại** tại một thời điểm
- Nếu chọn nhiều hơn 1 dòng, hệ thống sẽ yêu cầu bỏ chọn bớt

### 6. Về Trạng Thái Phân Loại

- Phân loại không hoạt động sẽ hiển thị màu chữ đỏ trong danh sách
- Trạng thái được quản lý trong màn hình chi tiết

### 7. Về Cấu Trúc Phân Cấp

- Danh sách hiển thị theo cấu trúc cây (cha-con)
- Đường dẫn phân loại hiển thị đầy đủ cấp độ phân cấp
- Danh sách được sắp xếp theo thứ tự ưu tiên (SortOrder) và tên phân loại

### 8. Về Sản Phẩm/Dịch Vụ

- Mỗi phân loại có thể chứa nhiều sản phẩm/dịch vụ
- Số lượng sản phẩm/dịch vụ hiển thị trong cột "Thông tin danh mục"
- Khi xóa phân loại, tất cả sản phẩm/dịch vụ sẽ được chuyển sang "Phân loại chưa đặt tên"

---

## Lỗi Thường Gặp

### 1. "Vui lòng chọn một dòng để chỉnh sửa"

**Nguyên nhân**: Bạn chưa chọn dòng nào khi nhấn nút "Điều chỉnh"

**Cách xử lý**:
- Chọn một dòng bằng cách nhấn vào dòng hoặc đánh dấu checkbox
- Sau đó nhấn lại nút "Điều chỉnh"

---

### 2. "Chỉ cho phép chỉnh sửa 1 dòng. Vui lòng bỏ chọn bớt."

**Nguyên nhân**: Bạn đã chọn nhiều hơn 1 dòng khi nhấn nút "Điều chỉnh"

**Cách xử lý**:
- Bỏ chọn các dòng khác, chỉ giữ lại 1 dòng được chọn
- Sau đó nhấn lại nút "Điều chỉnh"

---

### 3. "Vui lòng chọn ít nhất một dòng để xóa"

**Nguyên nhân**: Bạn chưa chọn dòng nào khi nhấn nút "Xóa"

**Cách xử lý**:
- Chọn ít nhất 1 dòng bằng cách nhấn vào dòng hoặc đánh dấu checkbox
- Sau đó nhấn lại nút "Xóa"

---

### 4. "Không có dữ liệu để xuất"

**Nguyên nhân**: Bạn chưa tải danh sách hoặc danh sách đang trống

**Cách xử lý**:
- Nhấn nút "Danh sách" để tải dữ liệu
- Đợi hệ thống tải xong
- Sau đó thử lại nút "Xuất"

---

### 5. "Lỗi tải dữ liệu"

**Nguyên nhân**: Có lỗi kỹ thuật khi tải dữ liệu từ máy chủ

**Cách xử lý**:
- Kiểm tra kết nối mạng/internet
- Thử lại thao tác sau vài phút
- Nếu vẫn lỗi, liên hệ bộ phận IT

---

### 6. "Lỗi xóa dữ liệu"

**Nguyên nhân**: Có lỗi kỹ thuật khi xóa phân loại

**Cách xử lý**:
- Kiểm tra kết nối mạng/internet
- Ghi lại thông báo lỗi chi tiết (nếu có)
- Thử lại thao tác sau vài phút
- Nếu vẫn lỗi, liên hệ bộ phận IT

---

### 7. "Không thể xóa phân loại: [Thông báo lỗi chi tiết]"

**Nguyên nhân**: Phân loại đang được sử dụng trong các giao dịch khác hoặc có ràng buộc dữ liệu

**Cách xử lý**:
- Đọc thông báo lỗi chi tiết để biết lý do
- Kiểm tra xem phân loại có đang được sử dụng ở đâu không
- Nếu cần, liên hệ quản trị viên hoặc bộ phận IT

---

### 8. "Lỗi xuất dữ liệu"

**Nguyên nhân**: Có lỗi kỹ thuật khi xuất file Excel

**Cách xử lý**:
- Kiểm tra quyền ghi file tại vị trí bạn chọn
- Đảm bảo có đủ dung lượng ổ đĩa
- Thử lại thao tác sau vài phút
- Nếu vẫn lỗi, liên hệ bộ phận IT

---

### 9. "Lỗi hiển thị màn hình thêm mới" / "Lỗi hiển thị màn hình điều chỉnh"

**Nguyên nhân**: Có lỗi khi mở màn hình chi tiết

**Cách xử lý**:
- Đóng màn hình và thử lại
- Kiểm tra kết nối mạng/internet
- Nếu vẫn lỗi, liên hệ bộ phận IT

---

### 10. "Không thể xác định dòng được chọn để chỉnh sửa"

**Nguyên nhân**: Có lỗi khi xác định dòng đã chọn

**Cách xử lý**:
- Bỏ chọn và chọn lại dòng
- Nhấn nút "Danh sách" để tải lại dữ liệu
- Sau đó thử lại thao tác chỉnh sửa

---

### 11. Màn Hình Bị Đơ Hoặc Không Phản Hồi

**Nguyên nhân**: Hệ thống đang xử lý dữ liệu (tải, xóa, v.v.)

**Cách xử lý**:
- Đợi vài giây, hệ thống sẽ tự động hoàn tất
- Nếu màn hình chờ hiển thị, đừng đóng nó
- Nếu sau 1-2 phút vẫn không phản hồi, liên hệ bộ phận IT

---

## Câu Hỏi Thường Gặp

### Q1: Tại sao khi xóa phân loại, sản phẩm/dịch vụ lại được chuyển sang "Phân loại chưa đặt tên"?

**Trả lời**: Đây là tính năng bảo vệ dữ liệu của hệ thống. Khi bạn xóa một phân loại, tất cả sản phẩm/dịch vụ thuộc phân loại đó sẽ được tự động chuyển sang "Phân loại chưa đặt tên" để đảm bảo không mất dữ liệu. Sau đó, bạn có thể phân loại lại các sản phẩm/dịch vụ này vào phân loại phù hợp.

---

### Q2: Tôi có thể xóa nhiều phân loại cùng lúc không?

**Trả lời**: Có, bạn có thể chọn nhiều dòng (bằng cách giữ Ctrl và nhấn vào các dòng khác, hoặc đánh dấu checkbox) và nhấn nút "Xóa". Hệ thống sẽ xóa tất cả các phân loại đã chọn sau khi bạn xác nhận. Tất cả sản phẩm/dịch vụ thuộc các phân loại đã xóa sẽ được chuyển sang "Phân loại chưa đặt tên".

---

### Q3: Tại sao tôi không thể chỉnh sửa khi chọn nhiều dòng?

**Trả lời**: Hệ thống chỉ cho phép chỉnh sửa một phân loại tại một thời điểm. Nếu bạn chọn nhiều hơn 1 dòng, hãy bỏ chọn bớt và chỉ giữ lại 1 dòng.

---

### Q4: Tôi có thể hoàn tác thao tác xóa không?

**Trả lời**: Không, thao tác xóa không thể hoàn tác. Vì vậy hãy cẩn thận và xác nhận kỹ trước khi xóa. Nếu xóa nhầm, bạn cần thêm mới phân loại đó lại và phân loại lại các sản phẩm/dịch vụ từ "Phân loại chưa đặt tên".

---

### Q5: Tại sao một số phân loại hiển thị màu đỏ?

**Trả lời**: Các phân loại hiển thị màu chữ đỏ là các phân loại **không hoạt động**. Màu đỏ giúp bạn dễ nhận biết các phân loại đã ngừng hoạt động.

---

### Q6: Tôi có thể tìm kiếm phân loại trong danh sách không?

**Trả lời**: Bạn có thể sử dụng tính năng tìm kiếm của GridView (nếu được cấu hình). Thông thường, bạn có thể gõ vào thanh tìm kiếm hoặc sử dụng bộ lọc của GridView.

---

### Q7: File Excel xuất ra có chứa tất cả dữ liệu không?

**Trả lời**: File Excel sẽ chứa tất cả dữ liệu đang hiển thị trong bảng tại thời điểm bạn nhấn nút "Xuất", bao gồm tất cả các cột thông tin.

---

### Q8: Tại sao sau khi thêm/sửa/xóa, tôi phải đợi màn hình chờ?

**Trả lời**: Hệ thống cần thời gian để lưu dữ liệu vào database và cập nhật. Màn hình chờ giúp bạn biết hệ thống đang xử lý. Thông thường chỉ mất vài giây.

---

### Q9: Tôi có thể sử dụng phím tắt không?

**Trả lời**: 
- **Ctrl + Click**: Chọn nhiều dòng
- **Delete**: Có thể được cấu hình để xóa (sau khi xác nhận)
- Các phím tắt khác tùy theo cấu hình của hệ thống

---

### Q10: Tại sao tôi không thấy dữ liệu khi mở màn hình?

**Trả lời**: Bạn cần nhấn nút **"Danh sách"** để tải dữ liệu. Màn hình không tự động tải dữ liệu khi mở.

---

### Q11: Làm sao để biết tôi đã chọn bao nhiêu dòng?

**Trả lời**: Thông tin về dòng đã chọn hiển thị ở thanh trạng thái phía dưới:
- Khi chọn 1 dòng: Hiển thị chi tiết tên, mã, trạng thái, số sản phẩm/DV
- Khi chọn nhiều dòng: "Đang chọn X dòng"
- Khi chưa chọn: "Chưa chọn dòng nào"

---

### Q12: Tại sao nút "Điều chỉnh" bị tắt (màu xám)?

**Trả lời**: Nút "Điều chỉnh" chỉ được kích hoạt khi bạn chọn đúng 1 dòng. Nếu bạn chưa chọn dòng nào hoặc chọn nhiều hơn 1 dòng, nút sẽ bị tắt.

---

### Q13: Tại sao nút "Xóa" bị tắt (màu xám)?

**Trả lời**: Nút "Xóa" chỉ được kích hoạt khi bạn chọn ít nhất 1 dòng. Nếu bạn chưa chọn dòng nào, nút sẽ bị tắt.

---

### Q14: Tại sao nút "Xuất" bị tắt (màu xám)?

**Trả lời**: Nút "Xuất" chỉ được kích hoạt khi có dữ liệu hiển thị trong bảng. Nếu chưa tải dữ liệu hoặc danh sách trống, nút sẽ bị tắt.

---

### Q15: Tôi có thể in danh sách phân loại không?

**Trả lời**: 
- Xuất ra Excel trước (nút "Xuất")
- Mở file Excel
- Sử dụng tính năng in của Excel (File > Print hoặc Ctrl+P)
- Điều chỉnh layout, orientation, margins theo nhu cầu

---

### Q16: "Phân loại chưa đặt tên" là gì?

**Trả lời**: "Phân loại chưa đặt tên" là phân loại mặc định của hệ thống. Khi bạn xóa một phân loại, tất cả sản phẩm/dịch vụ thuộc phân loại đó sẽ được tự động chuyển sang phân loại này. Bạn có thể phân loại lại các sản phẩm/dịch vụ này vào phân loại phù hợp.

---

### Q17: Tại sao danh sách được sắp xếp theo thứ tự này?

**Trả lời**: Danh sách được sắp xếp theo thứ tự ưu tiên (SortOrder) trước, sau đó theo tên phân loại. Điều này giúp bạn dễ dàng quản lý và tìm kiếm phân loại.

---

### Q18: Tôi có thể thay đổi thứ tự hiển thị của các phân loại không?

**Trả lời**: Thứ tự hiển thị được quản lý bởi trường "Thứ tự ưu tiên" (SortOrder) trong màn hình chi tiết. Bạn có thể chỉnh sửa thứ tự ưu tiên của từng phân loại để thay đổi thứ tự hiển thị.

---

## Liên Hệ Hỗ Trợ

Nếu bạn gặp vấn đề hoặc có câu hỏi không được giải đáp trong tài liệu này, vui lòng liên hệ:

- **Bộ phận IT**: [Thông tin liên hệ]
- **Email hỗ trợ**: [Email]
- **Hotline**: [Số điện thoại]

Khi liên hệ, vui lòng cung cấp:
- Mô tả vấn đề bạn gặp phải
- Các bước bạn đã thực hiện
- Thông báo lỗi (nếu có)
- Ảnh chụp màn hình (nếu có thể)
- Tên phân loại đang thao tác (nếu có)

---

**Tài liệu này được cập nhật lần cuối: 2025-01-XX**

**Phiên bản: 1.0**
