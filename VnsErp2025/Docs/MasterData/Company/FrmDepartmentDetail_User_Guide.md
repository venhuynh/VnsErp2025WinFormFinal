# Hướng Dẫn Sử Dụng - Quản Lý Phòng Ban

## Mục Đích

Màn hình **"Quản lý phòng ban"** giúp bạn:

- Thêm mới phòng ban vào hệ thống
- Chỉnh sửa thông tin phòng ban đã có
- Thiết lập cấu trúc phân cấp phòng ban (phòng ban cha - phòng ban con)
- Quản lý trạng thái hoạt động của phòng ban

**Lưu ý quan trọng**: Màn hình này có hai chế độ:
- **Thêm mới**: Tạo phòng ban mới
- **Chỉnh sửa**: Sửa thông tin phòng ban đã có (một số trường sẽ bị khóa)

---

## Các Bước Thao Tác

### 1. Mở Màn Hình Quản Lý Phòng Ban

#### 1.1. Thêm Mới Phòng Ban

**Bước 1**: Từ màn hình danh sách phòng ban, nhấn nút **"Thêm mới"** hoặc **"Mới"**

**Bước 2**: Màn hình **"Thêm mới phòng ban"** sẽ mở ra

**Bước 3**: Bạn sẽ thấy các trường thông tin trống để nhập liệu

#### 1.2. Chỉnh Sửa Phòng Ban

**Bước 1**: Từ màn hình danh sách phòng ban, chọn phòng ban bạn muốn chỉnh sửa

**Bước 2**: Nhấn nút **"Sửa"** hoặc **"Chỉnh sửa"** hoặc nhấn đúp chuột vào phòng ban

**Bước 3**: Màn hình **"Chỉnh sửa phòng ban"** sẽ mở ra với thông tin đã được điền sẵn

**Lưu ý**: Khi ở chế độ chỉnh sửa, một số trường sẽ bị khóa và không thể thay đổi:
- Mã phòng ban
- Chi nhánh
- Phòng ban cha

---

### 2. Điền Thông Tin Phòng Ban

#### 2.1. Chọn Chi Nhánh (Bắt Buộc) ⭐

**Bước 1**: Nhấn vào ô **"Chọn chi nhánh"** (có dấu sao đỏ *)

**Bước 2**: Một danh sách dropdown sẽ hiển thị các chi nhánh có sẵn

**Bước 3**: Chọn chi nhánh mà phòng ban này thuộc về

**Bước 4**: Tên chi nhánh sẽ hiển thị trong ô sau khi chọn

**Lưu ý**: 
- Trường này **bắt buộc phải chọn** (có dấu sao đỏ *)
- Danh sách chỉ hiển thị các chi nhánh đang hoạt động
- Khi ở chế độ chỉnh sửa, trường này bị khóa và không thể thay đổi

---

#### 2.2. Chọn Phòng Ban Cha (Tùy Chọn)

**Bước 1**: Nhấn vào ô **"Phòng ban cha"**

**Bước 2**: Một danh sách dropdown sẽ hiển thị các phòng ban có sẵn

**Bước 3**: Chọn phòng ban cha nếu phòng ban này thuộc về một phòng ban khác

**Bước 4**: Đường dẫn phòng ban cha sẽ hiển thị trong ô sau khi chọn (ví dụ: "Chi nhánh A > Phòng Kinh doanh")

**Lưu ý**: 
- Trường này **không bắt buộc** - có thể để trống
- Nếu để trống, phòng ban này sẽ là phòng ban cấp cao nhất (không có phòng ban cha)
- Khi ở chế độ chỉnh sửa, trường này bị khóa và không thể thay đổi
- Bạn không thể chọn chính phòng ban đang chỉnh sửa làm phòng ban cha (để tránh lỗi vòng lặp)

---

#### 2.3. Nhập Mã Phòng Ban (Bắt Buộc Khi Tạo Mới) ⭐

**Bước 1**: Nhấn vào ô **"Mã phòng ban"** (có dấu sao đỏ *)

**Bước 2**: Gõ mã phòng ban (ví dụ: PB01, PB02, DEPT001)

**Lưu ý**: 
- Trường này **bắt buộc phải nhập** khi tạo mới (có dấu sao đỏ *)
- Tối đa 50 ký tự
- Khi ở chế độ chỉnh sửa, trường này bị khóa và không thể thay đổi
- Mã phòng ban nên ngắn gọn, dễ nhớ

---

#### 2.4. Nhập Tên Phòng Ban (Bắt Buộc) ⭐

**Bước 1**: Nhấn vào ô **"Tên phòng ban"** (có dấu sao đỏ *)

**Bước 2**: Gõ tên phòng ban đầy đủ (ví dụ: Phòng Kinh doanh, Phòng Kỹ thuật, Phòng Hành chính)

**Lưu ý**: 
- Trường này **bắt buộc phải nhập** (có dấu sao đỏ *)
- Tối đa 255 ký tự
- Không được chỉ có khoảng trắng
- Có thể chỉnh sửa ở cả chế độ thêm mới và chỉnh sửa

---

#### 2.5. Nhập Mô Tả (Tùy Chọn)

**Bước 1**: Nhấn vào ô **"Mô tả"**

**Bước 2**: Gõ mô tả chi tiết về phòng ban (nếu cần)

**Lưu ý**: 
- Trường này **không bắt buộc** - có thể để trống
- Tối đa 255 ký tự
- Có thể nhập nhiều dòng
- Có thể chỉnh sửa ở cả chế độ thêm mới và chỉnh sửa

---

#### 2.6. Thiết Lập Trạng Thái

**Bước 1**: Nhìn vào công tắc **"Trạng thái"** (Toggle Switch)

**Bước 2**: 
- **Bật** (màu xanh): Phòng ban đang hoạt động
- **Tắt** (màu đỏ): Phòng ban không hoạt động

**Lưu ý**: 
- Mặc định khi tạo mới, trạng thái sẽ là **"Đang hoạt động"**
- Có thể thay đổi trạng thái ở cả chế độ thêm mới và chỉnh sửa
- Phòng ban không hoạt động sẽ không hiển thị trong một số danh sách

---

### 3. Lưu Thông Tin

**Bước 1**: Kiểm tra lại tất cả thông tin đã nhập

**Bước 2**: Đảm bảo các trường bắt buộc (có dấu sao đỏ) đã được điền đầy đủ:
- ✅ Chi nhánh đã được chọn
- ✅ Mã phòng ban đã được nhập (khi tạo mới)
- ✅ Tên phòng ban đã được nhập

**Bước 3**: Nhấn nút **"Lưu"** (biểu tượng 💾) trên thanh công cụ phía trên

**Bước 4**: Hệ thống sẽ kiểm tra thông tin:
- Nếu hợp lệ: 
  - Hiển thị thông báo "Tạo mới phòng ban thành công!" (khi thêm mới)
  - Hoặc "Cập nhật phòng ban thành công!" (khi chỉnh sửa)
  - Màn hình sẽ tự động đóng
- Nếu có lỗi: Hiển thị thông báo lỗi và yêu cầu bạn sửa lại

**Lưu ý**: 
- Nếu có lỗi validation, màn hình sẽ không đóng và bạn có thể sửa lại
- Sau khi lưu thành công, màn hình sẽ tự động đóng và quay về danh sách phòng ban

---

### 4. Đóng Màn Hình

**Bước 1**: Nhấn nút **"Đóng"** (biểu tượng ❌) trên thanh công cụ phía trên

**Bước 2**: Màn hình sẽ đóng ngay lập tức

**Lưu ý**: 
- Tất cả dữ liệu đã nhập sẽ **không được lưu** khi đóng màn hình
- Nếu muốn lưu, hãy nhấn nút "Lưu" trước khi đóng
- Bạn cũng có thể nhấn phím **Escape** để đóng màn hình

---

## Lưu Ý

### 1. Về Các Trường Bắt Buộc

Các trường có dấu **sao đỏ (*)** là bắt buộc phải có thông tin:
- **Chi nhánh**: Bắt buộc phải chọn một chi nhánh
- **Mã phòng ban**: Bắt buộc khi tạo mới (không thể sửa khi chỉnh sửa)
- **Tên phòng ban**: Bắt buộc phải nhập

Bạn không thể lưu thông tin nếu các trường bắt buộc chưa được điền đầy đủ.

### 2. Về Chế Độ Chỉnh Sửa

Khi ở chế độ chỉnh sửa, các trường sau sẽ bị khóa và không thể thay đổi:
- **Mã phòng ban**: Không thể thay đổi sau khi đã tạo
- **Chi nhánh**: Không thể thay đổi sau khi đã tạo
- **Phòng ban cha**: Không thể thay đổi sau khi đã tạo

Các trường có thể chỉnh sửa:
- **Tên phòng ban**: Có thể thay đổi
- **Mô tả**: Có thể thay đổi
- **Trạng thái**: Có thể thay đổi

### 3. Về Cấu Trúc Phân Cấp Phòng Ban

- Phòng ban có thể có phòng ban cha để tạo cấu trúc phân cấp
- Ví dụ: "Phòng Kinh doanh" có thể là phòng ban cha của "Nhóm Bán hàng" và "Nhóm Marketing"
- Bạn không thể chọn chính phòng ban đang chỉnh sửa làm phòng ban cha (để tránh lỗi vòng lặp)
- Nếu không chọn phòng ban cha, phòng ban này sẽ là phòng ban cấp cao nhất

### 4. Về Trạng Thái Hoạt Động

- **Đang hoạt động**: Phòng ban đang hoạt động bình thường
- **Không hoạt động**: Phòng ban đã ngừng hoạt động (có thể tạm thời hoặc vĩnh viễn)
- Phòng ban không hoạt động có thể không hiển thị trong một số danh sách hoặc báo cáo
- Mặc định khi tạo mới, trạng thái là "Đang hoạt động"

### 5. Về Tooltip (Gợi Ý)

Khi bạn di chuột qua các trường thông tin, bạn sẽ thấy một hộp gợi ý hiển thị thông tin chi tiết về trường đó, bao gồm:
- Mô tả chức năng
- Các quy tắc và giới hạn
- Hướng dẫn sử dụng

Hãy đọc các gợi ý này để hiểu rõ hơn về từng trường.

### 6. Về Danh Sách Dropdown

Khi chọn chi nhánh hoặc phòng ban cha:
- Danh sách sẽ hiển thị các mục có sẵn trong hệ thống
- Bạn có thể gõ để tìm kiếm trong danh sách
- Danh sách chi nhánh chỉ hiển thị các chi nhánh đang hoạt động
- Danh sách phòng ban cha sẽ không hiển thị chính phòng ban đang chỉnh sửa (khi ở chế độ edit)

---

## Lỗi Thường Gặp

### 1. "Vui lòng chọn chi nhánh"

**Nguyên nhân**: Bạn chưa chọn chi nhánh, trong khi đây là trường bắt buộc.

**Cách xử lý**:
- Nhấn vào ô "Chọn chi nhánh"
- Chọn một chi nhánh từ danh sách dropdown
- Đảm bảo chi nhánh đã được chọn trước khi nhấn nút "Lưu"

---

### 2. "Mã phòng ban không được để trống"

**Nguyên nhân**: Bạn chưa nhập mã phòng ban khi tạo mới, trong khi đây là trường bắt buộc.

**Cách xử lý**:
- Nhập mã phòng ban vào ô "Mã phòng ban"
- Mã phòng ban phải có ít nhất 1 ký tự
- Tối đa 50 ký tự

**Lưu ý**: Lỗi này chỉ xuất hiện khi tạo mới. Khi chỉnh sửa, trường này bị khóa nên không cần nhập.

---

### 3. "Tên phòng ban không được để trống"

**Nguyên nhân**: Bạn chưa nhập tên phòng ban, trong khi đây là trường bắt buộc.

**Cách xử lý**:
- Nhập tên phòng ban vào ô "Tên phòng ban"
- Tên phòng ban phải có ít nhất 1 ký tự (không phải chỉ khoảng trắng)
- Tối đa 255 ký tự

---

### 4. "Mã phòng ban không được vượt quá 50 ký tự"

**Nguyên nhân**: Bạn đã nhập mã phòng ban dài hơn 50 ký tự.

**Cách xử lý**:
- Rút ngắn mã phòng ban xuống còn tối đa 50 ký tự
- Xóa các ký tự thừa

---

### 5. "Tên phòng ban không được vượt quá 255 ký tự"

**Nguyên nhân**: Bạn đã nhập tên phòng ban dài hơn 255 ký tự.

**Cách xử lý**:
- Rút ngắn tên phòng ban xuống còn tối đa 255 ký tự
- Xóa các ký tự thừa

---

### 6. "Mô tả không được vượt quá 255 ký tự"

**Nguyên nhân**: Bạn đã nhập mô tả dài hơn 255 ký tự.

**Cách xử lý**:
- Rút ngắn mô tả xuống còn tối đa 255 ký tự
- Xóa các ký tự thừa

---

### 7. "Không tìm thấy phòng ban"

**Nguyên nhân**: Hệ thống không tìm thấy phòng ban bạn đang cố gắng chỉnh sửa (có thể đã bị xóa).

**Cách xử lý**:
- Đóng màn hình và quay về danh sách phòng ban
- Kiểm tra xem phòng ban có còn tồn tại không
- Nếu phòng ban đã bị xóa, bạn cần tạo mới

---

### 8. "Lỗi lưu phòng ban"

**Nguyên nhân**: Có lỗi kỹ thuật khi lưu phòng ban vào hệ thống.

**Cách xử lý**:
- Kiểm tra kết nối mạng/internet
- Kiểm tra lại tất cả thông tin đã nhập
- Thử lại thao tác sau vài phút
- Nếu vẫn lỗi, liên hệ bộ phận IT

---

### 9. "Lỗi load dữ liệu phòng ban"

**Nguyên nhân**: Có lỗi khi hệ thống tải thông tin phòng ban để chỉnh sửa.

**Cách xử lý**:
- Đóng màn hình và mở lại
- Kiểm tra kết nối mạng/internet
- Nếu vẫn lỗi, liên hệ bộ phận IT

---

### 10. "Lỗi load datasource"

**Nguyên nhân**: Có lỗi khi hệ thống tải danh sách chi nhánh hoặc phòng ban.

**Cách xử lý**:
- Đóng màn hình và mở lại
- Kiểm tra kết nối mạng/internet
- Nếu vẫn lỗi, liên hệ bộ phận IT

---

### 11. "Không tìm thấy thông tin công ty trong hệ thống"

**Nguyên nhân**: Hệ thống không tìm thấy thông tin công ty (cần thiết để tạo phòng ban).

**Cách xử lý**:
- Liên hệ quản trị viên hệ thống
- Đảm bảo thông tin công ty đã được thiết lập trong hệ thống

---

### 12. Biểu Tượng Cảnh Báo Màu Đỏ Bên Cạnh Trường

**Nguyên nhân**: Trường đó có lỗi (để trống khi bắt buộc, vượt quá độ dài, v.v.).

**Cách xử lý**:
- Di chuột qua biểu tượng cảnh báo để xem thông báo lỗi chi tiết
- Sửa lại thông tin theo yêu cầu
- Biểu tượng sẽ biến mất khi thông tin đã đúng

---

## Câu Hỏi Thường Gặp

### Q1: Tại sao tôi không thể thay đổi mã phòng ban khi chỉnh sửa?

**Trả lời**: Mã phòng ban được thiết kế để không thể thay đổi sau khi đã tạo. Điều này đảm bảo tính nhất quán của dữ liệu và tránh nhầm lẫn. Nếu cần thay đổi mã, bạn có thể tạo phòng ban mới với mã mới.

---

### Q2: Tại sao tôi không thể thay đổi chi nhánh khi chỉnh sửa?

**Trả lời**: Chi nhánh được thiết kế để không thể thay đổi sau khi đã tạo. Điều này đảm bảo tính nhất quán của dữ liệu. Nếu cần thay đổi chi nhánh, bạn có thể tạo phòng ban mới với chi nhánh mới.

---

### Q3: Tại sao tôi không thể thay đổi phòng ban cha khi chỉnh sửa?

**Trả lời**: Phòng ban cha được thiết kế để không thể thay đổi sau khi đã tạo. Điều này đảm bảo tính nhất quán của cấu trúc phân cấp. Nếu cần thay đổi, bạn có thể tạo phòng ban mới với phòng ban cha mới.

---

### Q4: Tôi có thể để trống phòng ban cha không?

**Trả lời**: Có, phòng ban cha là trường tùy chọn. Nếu để trống, phòng ban này sẽ là phòng ban cấp cao nhất (không có phòng ban cha).

---

### Q5: Tại sao tôi không thấy phòng ban của mình trong danh sách phòng ban cha?

**Trả lời**: Khi ở chế độ chỉnh sửa, phòng ban đang chỉnh sửa sẽ không hiển thị trong danh sách phòng ban cha. Điều này để tránh lỗi vòng lặp (chọn chính nó làm phòng ban cha).

---

### Q6: Tôi có thể tạo nhiều phòng ban cùng mã không?

**Trả lời**: Không, mỗi phòng ban phải có mã duy nhất. Hệ thống sẽ kiểm tra và không cho phép tạo phòng ban trùng mã.

---

### Q7: Trạng thái "Không hoạt động" có nghĩa là gì?

**Trả lời**: Trạng thái "Không hoạt động" có nghĩa là phòng ban đã ngừng hoạt động (có thể tạm thời hoặc vĩnh viễn). Phòng ban không hoạt động có thể không hiển thị trong một số danh sách hoặc báo cáo.

---

### Q8: Tôi có thể xóa phòng ban từ màn hình này không?

**Trả lời**: Không, màn hình này chỉ dùng để thêm mới hoặc chỉnh sửa phòng ban. Để xóa phòng ban, bạn cần quay về màn hình danh sách phòng ban và sử dụng chức năng xóa ở đó.

---

### Q9: Tôi có thể chỉnh sửa nhiều trường cùng lúc không?

**Trả lời**: Có, bạn có thể chỉnh sửa nhiều trường cùng lúc. Sau khi chỉnh sửa xong tất cả, nhấn nút "Lưu" một lần để lưu tất cả các thay đổi.

---

### Q10: Làm sao để biết thông tin đã được lưu thành công?

**Trả lời**: Sau khi nhấn nút "Lưu", hệ thống sẽ hiển thị thông báo:
- "Tạo mới phòng ban thành công!" - nếu thêm mới thành công
- "Cập nhật phòng ban thành công!" - nếu chỉnh sửa thành công
- Màn hình sẽ tự động đóng sau khi lưu thành công

---

### Q11: Tôi có thể hủy các thay đổi chưa lưu không?

**Trả lời**: Có, bạn có thể:
- Nhấn nút "Đóng" - các thay đổi sẽ không được lưu
- Hoặc nhấn phím **Escape** để đóng màn hình

---

### Q12: Tại sao tôi thấy biểu tượng cảnh báo màu đỏ bên cạnh một trường?

**Trả lời**: Biểu tượng cảnh báo màu đỏ xuất hiện khi trường đó có lỗi:
- Để trống khi bắt buộc
- Vượt quá độ dài cho phép

Di chuột qua biểu tượng để xem thông báo lỗi chi tiết và cách sửa.

---

### Q13: Tôi có thể sử dụng phím tắt không?

**Trả lời**: 
- **Tab**: Chuyển sang trường tiếp theo
- **Enter**: Hoàn tất chỉnh sửa trường hiện tại (tùy cấu hình)
- **Escape**: Đóng màn hình (không lưu)
- Các phím tắt khác tùy theo cấu hình của hệ thống

---

### Q14: Tại sao danh sách chi nhánh chỉ hiển thị một số chi nhánh?

**Trả lời**: Danh sách chi nhánh chỉ hiển thị các chi nhánh đang hoạt động. Các chi nhánh không hoạt động sẽ không hiển thị trong danh sách (trừ khi bạn đang chỉnh sửa phòng ban thuộc chi nhánh đó).

---

### Q15: Tôi có thể tìm kiếm trong danh sách dropdown không?

**Trả lời**: Có, khi mở danh sách dropdown (chi nhánh hoặc phòng ban cha), bạn có thể gõ để tìm kiếm. Hệ thống sẽ tự động lọc danh sách theo từ khóa bạn gõ.

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
- Tên phòng ban đang thao tác (nếu có)

---

**Tài liệu này được cập nhật lần cuối: 2025-01-XX**

**Phiên bản: 1.0**
