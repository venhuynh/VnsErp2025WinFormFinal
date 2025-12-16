# Hướng Dẫn Sử Dụng - Quản Lý Đơn Vị Tính

## Mục Đích

Màn hình **"Danh sách các đơn vị tính"** giúp bạn:

- Xem danh sách tất cả đơn vị tính trong hệ thống
- Thêm mới đơn vị tính
- Chỉnh sửa thông tin đơn vị tính
- Xóa một hoặc nhiều đơn vị tính
- Tìm kiếm và lọc đơn vị tính
- Thiết lập trạng thái hoạt động/ngừng hoạt động

**Lưu ý quan trọng**: 
- Mã và tên đơn vị tính là **bắt buộc** và **không được trùng lặp**
- Mã đơn vị tính **không được chứa khoảng trắng**
- Không thể chỉnh sửa hoặc xóa đơn vị tính đang có dữ liệu phụ thuộc

---

## Các Bước Thao Tác

### 1. Mở Màn Hình Quản Lý Đơn Vị Tính

**Bước 1**: Từ màn hình quản lý sản phẩm/dịch vụ hoặc các màn hình khác, tìm và chọn chức năng **"Quản lý đơn vị tính"** hoặc **"Đơn vị tính"**

**Bước 2**: Màn hình quản lý đơn vị tính sẽ hiển thị dạng popup (cửa sổ nhỏ)

**Bước 3**: Màn hình sẽ tự động tải danh sách đơn vị tính từ hệ thống

---

### 2. Xem Danh Sách Đơn Vị Tính

**Bước 1**: Danh sách đơn vị tính được hiển thị trong bảng ở phía dưới màn hình

**Bước 2**: Mỗi dòng hiển thị thông tin của một đơn vị tính:
- **Số thứ tự**: Hiển thị ở cột đầu tiên (1, 2, 3, ...)
- **Thông tin đơn vị tính**: Mã, tên, mô tả, trạng thái

**Bước 3**: Bạn có thể:
- Cuộn chuột để xem thêm các dòng khác
- Sử dụng dòng lọc ở đầu cột để tìm kiếm đơn vị tính

**Lưu ý**: 
- Danh sách được tự động tải khi mở màn hình
- Bạn có thể nhấn nút **"Làm mới"** để tải lại danh sách từ hệ thống

---

### 3. Tìm Kiếm và Lọc Đơn Vị Tính

**Bước 1**: Ở đầu cột **"Thông tin đơn vị tính"** trong bảng, bạn sẽ thấy một dòng trống để nhập điều kiện lọc

**Bước 2**: Nhập từ khóa bạn muốn tìm (ví dụ: mã, tên, mô tả)

**Bước 3**: Hệ thống sẽ tự động lọc và chỉ hiển thị các dòng thỏa mãn điều kiện

**Bước 4**: Để xóa bộ lọc, xóa nội dung trong dòng lọc

**Lưu ý**: 
- Bộ lọc hoạt động ngay khi bạn nhập, không cần nhấn nút nào
- Bạn có thể tìm kiếm theo bất kỳ phần nào của thông tin đơn vị tính

---

### 4. Thêm Mới Đơn Vị Tính

**Bước 1**: Nhấn nút **"Thêm mới"** (biểu tượng ➕) trên thanh công cụ

**Bước 2**: Hệ thống sẽ:
- Tạo một dòng mới trong bảng
- Xóa tất cả dữ liệu trong các ô nhập phía trên
- Thiết lập trạng thái "Hoạt động" mặc định (đã đánh dấu)
- Tự động focus vào ô **"Mã ĐVT"**

**Bước 3**: Nhập thông tin đơn vị tính:
- **Mã ĐVT**: Nhập mã đơn vị tính (bắt buộc, tối đa 20 ký tự, không có khoảng trắng)
- **Tên ĐVT**: Nhập tên đơn vị tính (bắt buộc, tối đa 100 ký tự)
- **Mô tả**: Nhập mô tả (tùy chọn, tối đa 255 ký tự)
- **Trạng thái**: Đánh dấu nếu đơn vị tính đang hoạt động (mặc định là đã đánh dấu)

**Bước 4**: Nhấn nút **"Lưu"** (biểu tượng 💾) để lưu đơn vị tính mới

**Bước 5**: Hệ thống sẽ:
- Kiểm tra tính hợp lệ của dữ liệu
- Kiểm tra mã và tên không trùng lặp
- Lưu vào hệ thống
- Hiển thị thông báo: "Đã lưu đơn vị tính"
- Tự động làm mới danh sách

**Lưu ý**: 
- Bạn có thể sử dụng phím tắt **Ctrl + N** để thêm mới nhanh
- Sau khi lưu, bạn có thể tiếp tục thêm đơn vị tính khác

---

### 5. Chỉnh Sửa Đơn Vị Tính

**Cách 1: Sử dụng nút "Điều chỉnh"**

**Bước 1**: Chọn một dòng đơn vị tính bạn muốn chỉnh sửa bằng cách:
- Nhấn vào dòng để chọn (dòng được chọn sẽ có màu nền khác)
- Hoặc đánh dấu checkbox ở đầu dòng

**Bước 2**: Đảm bảo chỉ chọn **một dòng duy nhất**

**Bước 3**: Nhấn nút **"Điều chỉnh"** (biểu tượng ✏️) trên thanh công cụ

**Bước 4**: Hệ thống sẽ:
- Kiểm tra xem đơn vị tính có dữ liệu phụ thuộc không
- Nếu có dữ liệu phụ thuộc, hiển thị cảnh báo và không cho phép chỉnh sửa
- Nếu không có dữ liệu phụ thuộc, hiển thị thông tin trong các ô nhập phía trên
- Tự động focus vào ô **"Mã ĐVT"** và chọn toàn bộ text

**Bước 5**: Chỉnh sửa thông tin cần thiết

**Bước 6**: Nhấn nút **"Lưu"** để lưu thay đổi

**Bước 7**: Hệ thống sẽ:
- Kiểm tra tính hợp lệ của dữ liệu
- Kiểm tra mã và tên không trùng lặp (nếu đã thay đổi)
- Lưu vào hệ thống
- Hiển thị thông báo: "Đã lưu đơn vị tính"
- Tự động làm mới danh sách

**Cách 2: Nhấn đúp chuột**

**Bước 1**: Nhấn đúp chuột vào dòng đơn vị tính bạn muốn chỉnh sửa

**Bước 2**: Hệ thống sẽ thực hiện tương tự như Cách 1 (từ Bước 4)

**Lưu ý**: 
- Bạn chỉ có thể chỉnh sửa **một đơn vị tính** tại một thời điểm
- Nếu bạn chọn nhiều hơn 1 dòng, hệ thống sẽ thông báo: "Chỉ cho phép chỉnh sửa 1 dòng. Vui lòng bỏ chọn bớt."
- Nếu bạn không chọn dòng nào, hệ thống sẽ thông báo: "Vui lòng chọn một dòng để chỉnh sửa."
- Nếu đơn vị tính có dữ liệu phụ thuộc, bạn không thể chỉnh sửa
- Nút "Điều chỉnh" chỉ được kích hoạt khi bạn chọn đúng 1 dòng và không đang chỉnh sửa

---

### 6. Hủy Chỉnh Sửa

**Bước 1**: Khi đang chỉnh sửa, nhấn phím **Escape**

**Bước 2**: Hệ thống sẽ:
- Hủy bỏ tất cả thay đổi chưa lưu
- Xóa dữ liệu trong các ô nhập
- Thoát khỏi chế độ chỉnh sửa
- Hiển thị thông báo: "Đã hủy chỉnh sửa"

**Lưu ý**: 
- Tất cả thay đổi chưa lưu sẽ bị mất
- Bạn có thể hủy chỉnh sửa bất cứ lúc nào khi đang ở chế độ chỉnh sửa

---

### 7. Xóa Đơn Vị Tính

**Bước 1**: Chọn một hoặc nhiều dòng đơn vị tính bạn muốn xóa bằng cách:
- Nhấn vào dòng để chọn
- Hoặc đánh dấu checkbox ở đầu dòng
- Bạn có thể chọn nhiều dòng bằng cách giữ phím **Ctrl** và nhấn vào các dòng khác
- Hoặc đánh dấu checkbox của nhiều dòng

**Bước 2**: Nhấn nút **"Xóa"** (biểu tượng 🗑️) trên thanh công cụ

**Bước 3**: Hệ thống sẽ hiển thị hộp thoại xác nhận:
- Nếu chọn 1 dòng: "Bạn có chắc muốn xóa đơn vị tính đã chọn?"
- Nếu chọn nhiều dòng: "Bạn có chắc muốn xóa X đơn vị tính đã chọn?"

**Bước 4**: 
- Nhấn **"Có"** (Yes) nếu bạn chắc chắn muốn xóa
- Nhấn **"Không"** (No) nếu bạn muốn hủy thao tác

**Bước 5**: Nếu bạn chọn "Có", hệ thống sẽ:
- Kiểm tra từng đơn vị tính có dữ liệu phụ thuộc không
- Xóa các đơn vị tính không có dữ liệu phụ thuộc
- Hiển thị cảnh báo cho các đơn vị tính có dữ liệu phụ thuộc (không xóa)
- Tự động làm mới danh sách

**Lưu ý quan trọng**:
- **Thao tác xóa không thể hoàn tác**, vì vậy hãy cẩn thận khi xóa
- Bạn có thể xóa nhiều đơn vị tính cùng lúc
- Nếu đơn vị tính có dữ liệu phụ thuộc, bạn không thể xóa
- Nếu đang chỉnh sửa đơn vị tính bị xóa, hệ thống sẽ tự động hủy chỉnh sửa
- Nút "Xóa" chỉ được kích hoạt khi bạn chọn ít nhất 1 dòng và không đang chỉnh sửa
- Bạn có thể sử dụng phím tắt **Delete** để xóa nhanh

---

### 8. Làm Mới Danh Sách

**Bước 1**: Nhấn nút **"Làm mới"** (biểu tượng 🔄) trên thanh công cụ

**Bước 2**: Hệ thống sẽ:
- Tải lại danh sách đơn vị tính từ hệ thống
- Xóa tất cả bộ lọc
- Xóa selection hiện tại
- Hủy chế độ chỉnh sửa (nếu có)
- Xóa dữ liệu trong các ô nhập

**Lưu ý**: 
- Sử dụng chức năng này khi bạn muốn cập nhật danh sách với dữ liệu mới nhất từ hệ thống
- Tất cả thay đổi chưa lưu sẽ bị mất

---

## Hiểu Về Giao Diện

### Thanh Công Cụ (Phía Trên)

Các nút chức năng chính:
- **➕ Thêm mới**: Tạo đơn vị tính mới (chỉ kích hoạt khi không đang chỉnh sửa)
- **💾 Lưu**: Lưu thông tin đơn vị tính (chỉ kích hoạt khi đang chỉnh sửa)
- **✏️ Điều chỉnh**: Chỉnh sửa đơn vị tính đã chọn (chỉ kích hoạt khi chọn đúng 1 dòng và không đang chỉnh sửa)
- **🗑️ Xóa**: Xóa đơn vị tính đã chọn (chỉ kích hoạt khi chọn ít nhất 1 dòng và không đang chỉnh sửa)
- **🔄 Làm mới**: Tải lại danh sách từ hệ thống

**Phím tắt**:
- **Ctrl + S**: Lưu nhanh
- **Ctrl + N**: Thêm mới nhanh
- **Escape**: Hủy chỉnh sửa (khi đang chỉnh sửa)
- **Delete**: Xóa đơn vị tính đã chọn

**Trạng thái nút tự động**:
- Nút "Thêm mới" chỉ bật khi không đang chỉnh sửa
- Nút "Lưu" chỉ bật khi đang chỉnh sửa và có dữ liệu để lưu
- Nút "Điều chỉnh" chỉ bật khi bạn chọn đúng 1 dòng và không đang chỉnh sửa
- Nút "Xóa" chỉ bật khi bạn chọn ít nhất 1 dòng và không đang chỉnh sửa
- Nút "Làm mới" luôn hoạt động

### Phần Nhập Liệu (Phía Trên)

Các trường thông tin:
- **Mã ĐVT**: Nhập mã đơn vị tính (bắt buộc, tối đa 20 ký tự, không có khoảng trắng)
- **Tên ĐVT**: Nhập tên đơn vị tính (bắt buộc, tối đa 100 ký tự)
- **Mô tả**: Nhập mô tả (tùy chọn, tối đa 255 ký tự)
- **Trạng thái**: Đánh dấu nếu đơn vị tính đang hoạt động

**Lưu ý**: 
- Các trường bắt buộc sẽ có dấu * đỏ
- Khi chỉnh sửa, dữ liệu sẽ được hiển thị trong các ô này
- Khi thêm mới, các ô sẽ trống (trừ trạng thái mặc định là đã đánh dấu)

### Bảng Danh Sách (Phía Dưới)

**Mỗi dòng hiển thị**:
- **Số thứ tự**: Hiển thị ở cột đầu tiên (1, 2, 3, ...)
- **Checkbox**: Ở đầu dòng để chọn dòng
- **Thông tin đơn vị tính**: Mã, tên, mô tả, trạng thái (hiển thị dạng HTML với màu sắc)

**Màu sắc**:
- **Đơn vị tính không hoạt động**: Có thể hiển thị màu đỏ hoặc gạch ngang
- **Dòng được chọn**: Có màu nền khác biệt
- **Dòng chẵn/lẻ**: Có màu nền xen kẽ để dễ đọc

**Tính năng**:
- **Chọn nhiều dòng**: Giữ phím Ctrl và nhấn vào các dòng khác, hoặc đánh dấu checkbox của nhiều dòng
- **Tìm kiếm/Lọc**: Nhập điều kiện vào dòng lọc ở đầu cột
- **Nhấn đúp chuột**: Mở chế độ chỉnh sửa
- **Cuộn**: Có thể cuộn để xem thêm các dòng khác

### Thông Báo Lỗi

Khi có lỗi validation, hệ thống sẽ:
- Hiển thị biểu tượng cảnh báo (⚠️) bên cạnh trường bị lỗi
- Hiển thị thông báo lỗi khi bạn di chuột vào biểu tượng
- Hiển thị hộp thoại cảnh báo với danh sách lỗi
- Tự động focus vào trường đầu tiên bị lỗi

---

## Lưu Ý Quan Trọng

### 1. Về Mã Đơn Vị Tính

- **Bắt buộc**: Mã không được để trống
- **Duy nhất**: Mã phải duy nhất trong hệ thống (không trùng với mã khác)
- **Không có khoảng trắng**: Mã không được chứa khoảng trắng (ví dụ: "KG" đúng, "K G" sai)
- **Giới hạn**: Tối đa 20 ký tự

### 2. Về Tên Đơn Vị Tính

- **Bắt buộc**: Tên không được để trống
- **Duy nhất**: Tên phải duy nhất trong hệ thống (không trùng với tên khác)
- **Giới hạn**: Tối đa 100 ký tự

### 3. Về Mô Tả

- **Tùy chọn**: Có thể để trống
- **Giới hạn**: Tối đa 255 ký tự

### 4. Về Trạng Thái

- **Mặc định**: Hoạt động (đã đánh dấu khi thêm mới)
- **Ngừng hoạt động**: Đơn vị tính ngừng hoạt động sẽ không hiển thị trong một số danh sách lựa chọn
- **Có thể thay đổi**: Bạn có thể thay đổi trạng thái bất cứ lúc nào

### 5. Về Dữ Liệu Phụ Thuộc

- **Không thể chỉnh sửa**: Nếu đơn vị tính đang được sử dụng trong các giao dịch khác, bạn không thể chỉnh sửa
- **Không thể xóa**: Nếu đơn vị tính đang được sử dụng trong các giao dịch khác, bạn không thể xóa
- **Kiểm tra tự động**: Hệ thống sẽ tự động kiểm tra trước khi cho phép chỉnh sửa hoặc xóa

### 6. Về Validation (Kiểm Tra Dữ Liệu)

- **Kiểm tra khi lưu**: Hệ thống sẽ kiểm tra tất cả dữ liệu khi bạn nhấn "Lưu"
- **Hiển thị lỗi**: Nếu có lỗi, hệ thống sẽ hiển thị thông báo và đánh dấu trường bị lỗi
- **Phải sửa lỗi**: Bạn phải sửa tất cả lỗi trước khi có thể lưu thành công

### 7. Về Chế Độ Chỉnh Sửa

- **Một lúc một**: Bạn chỉ có thể chỉnh sửa một đơn vị tính tại một thời điểm
- **Hủy chỉnh sửa**: Bạn có thể nhấn Escape để hủy chỉnh sửa bất cứ lúc nào
- **Tự động focus**: Khi bắt đầu chỉnh sửa, hệ thống sẽ tự động focus vào ô "Mã ĐVT"

### 8. Về Làm Mới Danh Sách

- **Tải lại dữ liệu**: Làm mới sẽ tải lại dữ liệu từ hệ thống
- **Xóa thay đổi**: Tất cả thay đổi chưa lưu sẽ bị mất
- **Xóa bộ lọc**: Tất cả bộ lọc sẽ bị xóa

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

### 3. "Vui lòng chọn ít nhất 1 dòng để xóa"

**Nguyên nhân**: Bạn chưa chọn dòng nào khi nhấn nút "Xóa"

**Cách xử lý**:
- Chọn ít nhất 1 dòng bằng cách nhấn vào dòng hoặc đánh dấu checkbox
- Sau đó nhấn lại nút "Xóa"

---

### 4. "Mã đơn vị tính không được để trống"

**Nguyên nhân**: Bạn chưa nhập mã đơn vị tính

**Cách xử lý**:
- Nhập mã đơn vị tính vào ô "Mã ĐVT"
- Đảm bảo mã không có khoảng trắng

---

### 5. "Mã đơn vị tính không được chứa khoảng trắng"

**Nguyên nhân**: Mã bạn nhập có chứa khoảng trắng

**Cách xử lý**:
- Xóa tất cả khoảng trắng trong mã
- Ví dụ: "KG" đúng, "K G" sai

---

### 6. "Mã đơn vị tính không được vượt quá 20 ký tự"

**Nguyên nhân**: Mã bạn nhập quá dài (hơn 20 ký tự)

**Cách xử lý**:
- Rút ngắn mã xuống còn tối đa 20 ký tự

---

### 7. "Mã đơn vị tính đã tồn tại"

**Nguyên nhân**: Mã bạn nhập đã được sử dụng bởi đơn vị tính khác

**Cách xử lý**:
- Nhập một mã khác, duy nhất
- Kiểm tra xem có đơn vị tính nào đã sử dụng mã này chưa

---

### 8. "Tên đơn vị tính không được để trống"

**Nguyên nhân**: Bạn chưa nhập tên đơn vị tính

**Cách xử lý**:
- Nhập tên đơn vị tính vào ô "Tên ĐVT"

---

### 9. "Tên đơn vị tính không được vượt quá 100 ký tự"

**Nguyên nhân**: Tên bạn nhập quá dài (hơn 100 ký tự)

**Cách xử lý**:
- Rút ngắn tên xuống còn tối đa 100 ký tự

---

### 10. "Tên đơn vị tính đã tồn tại"

**Nguyên nhân**: Tên bạn nhập đã được sử dụng bởi đơn vị tính khác

**Cách xử lý**:
- Nhập một tên khác, duy nhất
- Kiểm tra xem có đơn vị tính nào đã sử dụng tên này chưa

---

### 11. "Mô tả không được vượt quá 255 ký tự"

**Nguyên nhân**: Mô tả bạn nhập quá dài (hơn 255 ký tự)

**Cách xử lý**:
- Rút ngắn mô tả xuống còn tối đa 255 ký tự

---

### 12. "Không thể chỉnh sửa '[Tên]' vì còn dữ liệu phụ thuộc. Việc sửa đổi có thể ảnh hưởng đến tính toàn vẹn dữ liệu."

**Nguyên nhân**: Đơn vị tính bạn muốn chỉnh sửa đang được sử dụng trong các giao dịch khác

**Cách xử lý**:
- Bạn không thể chỉnh sửa đơn vị tính này
- Nếu cần thay đổi, hãy tạo đơn vị tính mới và cập nhật các giao dịch liên quan

---

### 13. "Không thể xóa '[Tên]' vì còn dữ liệu phụ thuộc."

**Nguyên nhân**: Đơn vị tính bạn muốn xóa đang được sử dụng trong các giao dịch khác

**Cách xử lý**:
- Bạn không thể xóa đơn vị tính này
- Nếu không còn sử dụng, hãy đánh dấu "Ngừng hoạt động" thay vì xóa

---

### 14. "Vui lòng chọn 1 dòng và bấm Điều chỉnh trước khi lưu."

**Nguyên nhân**: Bạn nhấn "Lưu" nhưng chưa chọn dòng để chỉnh sửa hoặc chưa nhấn "Điều chỉnh"

**Cách xử lý**:
- Chọn một dòng và nhấn nút "Điều chỉnh" trước
- Hoặc nhấn nút "Thêm mới" để tạo đơn vị tính mới

---

### 15. "Lỗi lưu đơn vị tính"

**Nguyên nhân**: Có lỗi kỹ thuật khi lưu dữ liệu

**Cách xử lý**:
- Kiểm tra kết nối mạng/internet
- Kiểm tra lại tất cả thông tin đã nhập
- Thử lại sau vài phút
- Nếu vẫn lỗi, liên hệ bộ phận IT

---

### 16. "Lỗi xóa đơn vị tính"

**Nguyên nhân**: Có lỗi kỹ thuật khi xóa dữ liệu

**Cách xử lý**:
- Kiểm tra kết nối mạng/internet
- Thử lại sau vài phút
- Nếu vẫn lỗi, liên hệ bộ phận IT

---

### 17. "Lỗi tải danh sách đơn vị tính"

**Nguyên nhân**: Có lỗi khi tải dữ liệu từ máy chủ

**Cách xử lý**:
- Kiểm tra kết nối mạng/internet
- Thử lại sau vài phút
- Nếu vẫn lỗi, liên hệ bộ phận IT

---

### 18. "Lỗi làm mới dữ liệu"

**Nguyên nhân**: Có lỗi khi làm mới dữ liệu

**Cách xử lý**:
- Kiểm tra kết nối mạng/internet
- Thử lại sau vài phút
- Nếu vẫn lỗi, liên hệ bộ phận IT

---

## Câu Hỏi Thường Gặp

### Q1: Tại sao mã đơn vị tính không được chứa khoảng trắng?

**Trả lời**: Đây là quy tắc của hệ thống để đảm bảo tính nhất quán và dễ xử lý. Mã đơn vị tính phải là một chuỗi liền không có khoảng trắng (ví dụ: "KG", "LIT", "M2").

---

### Q2: Tôi có thể để trống mã đơn vị tính không?

**Trả lời**: Không, mã đơn vị tính là trường bắt buộc. Bạn phải nhập mã.

---

### Q3: Tôi có thể để trống tên đơn vị tính không?

**Trả lời**: Không, tên đơn vị tính là trường bắt buộc. Bạn phải nhập tên.

---

### Q4: Tại sao tôi không thể lưu khi mã/tên đã tồn tại?

**Trả lời**: Hệ thống yêu cầu mã và tên phải duy nhất để tránh nhầm lẫn. Nếu mã hoặc tên đã được sử dụng, bạn cần nhập một mã/tên khác.

---

### Q5: Tôi có thể thay đổi mã đơn vị tính khi chỉnh sửa không?

**Trả lời**: Có, bạn có thể thay đổi mã khi chỉnh sửa, nhưng mã mới phải duy nhất (không trùng với mã khác) và không được có dữ liệu phụ thuộc.

---

### Q6: Tôi có thể thay đổi tên đơn vị tính khi chỉnh sửa không?

**Trả lời**: Có, bạn có thể thay đổi tên khi chỉnh sửa, nhưng tên mới phải duy nhất (không trùng với tên khác) và không được có dữ liệu phụ thuộc.

---

### Q7: Tại sao tôi không thể chỉnh sửa một số đơn vị tính?

**Trả lời**: Nếu đơn vị tính đang được sử dụng trong các giao dịch khác (có dữ liệu phụ thuộc), bạn không thể chỉnh sửa để đảm bảo tính toàn vẹn dữ liệu.

---

### Q8: Tại sao tôi không thể xóa một số đơn vị tính?

**Trả lời**: Nếu đơn vị tính đang được sử dụng trong các giao dịch khác (có dữ liệu phụ thuộc), bạn không thể xóa để đảm bảo tính toàn vẹn dữ liệu. Thay vào đó, hãy đánh dấu "Ngừng hoạt động".

---

### Q9: Tôi có thể sử dụng phím tắt không?

**Trả lời**: Có, bạn có thể sử dụng:
- **Ctrl + S**: Lưu nhanh
- **Ctrl + N**: Thêm mới nhanh
- **Escape**: Hủy chỉnh sửa (khi đang chỉnh sửa)
- **Delete**: Xóa đơn vị tính đã chọn

---

### Q10: Tại sao khi tôi nhấn "Lưu", hệ thống báo lỗi?

**Trả lời**: Có thể do:
- Bạn chưa nhập đầy đủ thông tin bắt buộc (mã, tên)
- Mã hoặc tên đã tồn tại
- Mã có chứa khoảng trắng
- Dữ liệu vượt quá giới hạn cho phép
- Hãy kiểm tra các thông báo lỗi và sửa các lỗi được đánh dấu

---

### Q11: Tôi có thể hủy chỉnh sửa không?

**Trả lời**: Có, bạn có thể nhấn phím **Escape** để hủy chỉnh sửa bất cứ lúc nào. Tất cả thay đổi chưa lưu sẽ bị mất.

---

### Q12: Tại sao nút "Điều chỉnh" bị tắt (màu xám)?

**Trả lời**: Nút "Điều chỉnh" chỉ được kích hoạt khi:
- Bạn chọn đúng 1 dòng
- Bạn không đang chỉnh sửa đơn vị tính khác

---

### Q13: Tại sao nút "Xóa" bị tắt (màu xám)?

**Trả lời**: Nút "Xóa" chỉ được kích hoạt khi:
- Bạn chọn ít nhất 1 dòng
- Bạn không đang chỉnh sửa đơn vị tính

---

### Q14: Tại sao nút "Lưu" bị tắt (màu xám)?

**Trả lời**: Nút "Lưu" chỉ được kích hoạt khi:
- Bạn đang ở chế độ chỉnh sửa (đã chọn dòng và nhấn "Điều chỉnh" hoặc nhấn "Thêm mới")
- Có dữ liệu để lưu

---

### Q15: Tại sao nút "Thêm mới" bị tắt (màu xám)?

**Trả lời**: Nút "Thêm mới" chỉ được kích hoạt khi bạn không đang chỉnh sửa đơn vị tính khác. Hãy hoàn tất hoặc hủy chỉnh sửa hiện tại trước.

---

### Q16: Tôi có thể tìm kiếm đơn vị tính trong danh sách không?

**Trả lời**: Có, bạn có thể sử dụng dòng lọc ở đầu cột "Thông tin đơn vị tính" để tìm kiếm. Bộ lọc hoạt động ngay khi bạn nhập, không cần nhấn nút nào.

---

### Q17: Tôi có thể xóa nhiều đơn vị tính cùng lúc không?

**Trả lời**: Có, bạn có thể chọn nhiều dòng (bằng cách giữ Ctrl và nhấn vào các dòng khác, hoặc đánh dấu checkbox) và nhấn nút "Xóa". Hệ thống sẽ xóa tất cả các đơn vị tính đã chọn (trừ những đơn vị tính có dữ liệu phụ thuộc).

---

### Q18: Tại sao sau khi lưu, danh sách tự động làm mới?

**Trả lời**: Đây là hành vi mặc định của hệ thống. Sau khi lưu thành công, hệ thống sẽ tự động làm mới danh sách để hiển thị dữ liệu mới nhất, bao gồm cả đơn vị tính vừa lưu.

---

### Q19: Tôi có thể chỉnh sửa nhiều đơn vị tính cùng lúc không?

**Trả lời**: Không, bạn chỉ có thể chỉnh sửa một đơn vị tính tại một thời điểm. Hãy hoàn tất hoặc hủy chỉnh sửa đơn vị tính hiện tại trước khi chỉnh sửa đơn vị tính khác.

---

### Q20: "Dữ liệu phụ thuộc" là gì?

**Trả lời**: Dữ liệu phụ thuộc là các giao dịch, sản phẩm, hoặc dữ liệu khác đang sử dụng đơn vị tính này. Nếu đơn vị tính có dữ liệu phụ thuộc, bạn không thể chỉnh sửa hoặc xóa để đảm bảo tính toàn vẹn dữ liệu.

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
- Mã hoặc tên đơn vị tính đang thao tác (nếu có)

---

**Tài liệu này được cập nhật lần cuối: 2025-01-XX**

**Phiên bản: 1.0**
