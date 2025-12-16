# Hướng Dẫn Sử Dụng - Chi Tiết Biến Thể Sản Phẩm

## Mục Đích

Màn hình **"Chi tiết biến thể sản phẩm"** giúp bạn:

- Thêm mới biến thể sản phẩm vào hệ thống
- Chỉnh sửa thông tin biến thể sản phẩm đã có
- Quản lý các thuộc tính của biến thể (ví dụ: màu sắc, kích thước, dung lượng, v.v.)
- Thiết lập trạng thái hoạt động/ngừng hoạt động
- Tự động tạo mã biến thể dựa trên sản phẩm và đơn vị tính

**Lưu ý quan trọng**: 
- Sản phẩm/dịch vụ, mã biến thể và đơn vị tính là **bắt buộc**
- Mã biến thể sẽ được **tự động tạo** khi bạn chọn sản phẩm và đơn vị tính (chế độ thêm mới)
- Mỗi biến thể phải có **ít nhất một thuộc tính**
- Không thể chọn trùng thuộc tính trong cùng một biến thể

---

## Các Bước Thao Tác

### 1. Mở Màn Hình Chi Tiết

**Cách 1: Thêm mới biến thể sản phẩm**

**Bước 1**: Từ màn hình quản lý biến thể sản phẩm hoặc màn hình quản lý sản phẩm/dịch vụ, nhấn nút **"Thêm mới biến thể"** hoặc tương tự

**Bước 2**: Màn hình chi tiết sẽ hiển thị với tiêu đề **"Thêm mới biến thể sản phẩm"**

**Bước 3**: Tất cả các trường sẽ trống, sẵn sàng để bạn nhập thông tin

---

**Cách 2: Chỉnh sửa biến thể sản phẩm**

**Bước 1**: Từ màn hình quản lý biến thể sản phẩm, chọn một biến thể và nhấn nút **"Điều chỉnh"** hoặc nhấn đúp chuột

**Bước 2**: Màn hình chi tiết sẽ hiển thị với tiêu đề **"Chỉnh sửa biến thể sản phẩm - [Mã biến thể]"**

**Bước 3**: Tất cả các trường sẽ được điền sẵn với thông tin hiện tại

**Lưu ý**: 
- Khi chỉnh sửa, các trường **Sản phẩm/dịch vụ** và **Mã biến thể** sẽ bị khóa (không thể thay đổi)
- Trường **Đơn vị tính** vẫn có thể thay đổi

---

### 2. Nhập Thông Tin Cơ Bản

#### 2.1. Chọn Sản Phẩm/Dịch Vụ (Bắt buộc)

**Bước 1**: Nhấn vào ô **"Sản phẩm dịch vụ"**

**Bước 2**: Một danh sách các sản phẩm/dịch vụ đang hoạt động sẽ hiển thị

**Bước 3**: Chọn sản phẩm hoặc dịch vụ gốc từ danh sách

**Lưu ý**: 
- Trường này là **bắt buộc** (không được để trống)
- Chỉ hiển thị các sản phẩm/dịch vụ **đang hoạt động**
- Khi thêm mới, nếu bạn chọn sản phẩm và đơn vị tính, **mã biến thể sẽ được tự động tạo**
- Khi chỉnh sửa, trường này sẽ bị khóa (không thể thay đổi)

---

#### 2.2. Chọn Đơn Vị Tính (Bắt buộc)

**Bước 1**: Nhấn vào ô **"Đơn vị tính"**

**Bước 2**: Một danh sách các đơn vị tính đang hoạt động sẽ hiển thị

**Bước 3**: Chọn đơn vị tính phù hợp từ danh sách

**Lưu ý**: 
- Trường này là **bắt buộc** (không được để trống)
- Chỉ hiển thị các đơn vị tính **đang hoạt động**
- Khi thêm mới, nếu bạn chọn sản phẩm và đơn vị tính, **mã biến thể sẽ được tự động tạo**
- Khi chỉnh sửa, trường này vẫn có thể thay đổi

---

#### 2.3. Mã Biến Thể (Bắt buộc)

**Bước 1**: Nhấn vào ô **"Mã biến thể"**

**Bước 2**: 
- **Nếu thêm mới và đã chọn sản phẩm + đơn vị tính**: Mã sẽ được tự động điền theo format: [Mã sản phẩm]_[Mã đơn vị]_[Số thứ tự 4 chữ số]
- **Nếu thêm mới và chưa chọn đủ**: Nhập mã thủ công
- **Nếu chỉnh sửa**: Mã hiện tại sẽ được hiển thị nhưng không thể thay đổi (bị khóa)

**Bước 3**: Nếu mã tự động, bạn có thể chỉnh sửa nếu cần (nhưng phải đảm bảo mã duy nhất)

**Lưu ý**: 
- Trường này là **bắt buộc** (không được để trống)
- Mã biến thể phải **duy nhất** trong hệ thống
- Format mã tự động: `[Mã sản phẩm]_[Mã đơn vị]_[0001-9999]`
- Ví dụ: Nếu sản phẩm có mã "SP001" và đơn vị có mã "KG", mã biến thể sẽ là "SP001_KG_0001", "SP001_KG_0002", v.v.

---

#### 2.4. Thiết Lập Trạng Thái

**Bước 1**: Tìm công tắc **"Trạng thái"** trên màn hình

**Bước 2**: 
- **Bật (ON)**: Biến thể **đang sử dụng** (màu xanh)
- **Tắt (OFF)**: Biến thể **không sử dụng** (màu đỏ)

**Lưu ý**: 
- Mặc định là **Đang sử dụng** (công tắc ở vị trí BẬT)
- Biến thể không sử dụng sẽ không hiển thị trong một số danh sách lựa chọn

---

### 3. Quản Lý Thuộc Tính Biến Thể

#### 3.1. Thêm Thuộc Tính Mới

**Cách 1: Sử dụng nút "+" trên EmbeddedNavigator**

**Bước 1**: Tìm thanh điều hướng (EmbeddedNavigator) ở phía dưới bảng thuộc tính

**Bước 2**: Nhấn nút **"+"** (dấu cộng) để thêm dòng mới

**Bước 3**: Một dòng trống sẽ xuất hiện ở đầu bảng

**Bước 4**: 
- Chọn **Tên thuộc tính** từ danh sách thả xuống (ví dụ: Màu sắc, Kích thước, Dung lượng, v.v.)
- Nhập **Giá trị** tương ứng (ví dụ: Đỏ, XL, 500ml, v.v.)

**Bước 5**: Nhấn Enter hoặc di chuyển sang dòng khác để lưu

**Cách 2: Nhấn vào dòng "Thêm mới" ở đầu bảng**

**Bước 1**: Tìm dòng **"Thêm mới"** ở đầu bảng thuộc tính

**Bước 2**: Nhấn vào dòng này

**Bước 3**: Thực hiện các bước tương tự như Cách 1 (từ Bước 4)

**Lưu ý**: 
- Bạn có thể thêm nhiều thuộc tính cho một biến thể
- Mỗi thuộc tính chỉ được chọn **một lần** trong cùng một biến thể
- Nếu tất cả thuộc tính đã được sử dụng, hệ thống sẽ thông báo: "Tất cả thuộc tính đã được sử dụng. Không thể thêm dòng mới."

---

#### 3.2. Chọn Tên Thuộc Tính

**Bước 1**: Nhấn vào ô **"Tên thuộc tính"** trong dòng mới

**Bước 2**: Một danh sách các thuộc tính sẽ hiển thị

**Bước 3**: Chọn thuộc tính phù hợp từ danh sách

**Lưu ý**: 
- Chỉ hiển thị các thuộc tính **chưa được sử dụng** trong biến thể này
- Nếu bạn chọn thuộc tính đã được sử dụng, hệ thống sẽ cảnh báo và yêu cầu chọn thuộc tính khác
- Mỗi thuộc tính chỉ được chọn một lần trong cùng một biến thể

---

#### 3.3. Nhập Giá Trị Thuộc Tính

**Bước 1**: Sau khi chọn tên thuộc tính, nhấn vào ô **"Giá trị"**

**Bước 2**: Nhập giá trị tương ứng với thuộc tính đã chọn

**Bước 3**: Hệ thống sẽ tự động kiểm tra:
- Giá trị không được để trống
- Giá trị không được vượt quá 255 ký tự
- Giá trị phải phù hợp với kiểu dữ liệu của thuộc tính:
  - **Số nguyên** (int, integer): Chỉ chấp nhận số nguyên (ví dụ: 1, 100, -50)
  - **Số thực** (number, decimal, float, money, currency): Chấp nhận số thực (ví dụ: 1.5, 100.99, -50.5)
  - **Ngày** (date): Chấp nhận định dạng ngày (ví dụ: 01/01/2025)
  - **Ngày giờ** (datetime): Chấp nhận định dạng ngày giờ (ví dụ: 01/01/2025 10:30)
  - **Đúng/Sai** (bool, boolean): Chấp nhận true/false, 1/0, có/không, yes/no
  - **Text** (mặc định): Chấp nhận bất kỳ văn bản nào

**Lưu ý**: 
- Giá trị là **bắt buộc** (không được để trống)
- Giá trị phải phù hợp với kiểu dữ liệu của thuộc tính
- Nếu giá trị không hợp lệ, hệ thống sẽ hiển thị lỗi và không cho phép lưu

---

#### 3.4. Xóa Thuộc Tính

**Bước 1**: Chọn dòng thuộc tính bạn muốn xóa

**Bước 2**: Tìm thanh điều hướng (EmbeddedNavigator) ở phía dưới bảng

**Bước 3**: Nhấn nút **"X"** (dấu trừ) để xóa dòng

**Bước 4**: Hệ thống sẽ hiển thị hộp thoại xác nhận: "Bạn có chắc chắn muốn xóa dòng này?"

**Bước 5**: 
- Nhấn **"Có"** (Yes) nếu bạn chắc chắn muốn xóa
- Nhấn **"Không"** (No) nếu bạn muốn hủy

**Bước 6**: Nếu bạn chọn "Có", dòng sẽ được xóa khỏi bảng

**Lưu ý đặc biệt**: 
- Nếu đang chỉnh sửa và đây là **thuộc tính cuối cùng**, hệ thống sẽ hỏi: "Đây là thuộc tính cuối cùng của biến thể sản phẩm '[Mã]'. Bạn có muốn xóa toàn bộ biến thể sản phẩm này không?"
- Nếu bạn chọn "Có", **toàn bộ biến thể sản phẩm sẽ bị xóa** và màn hình sẽ đóng
- Nếu bạn chọn "Không", thao tác xóa sẽ bị hủy

---

### 4. Lưu Dữ Liệu

**Bước 1**: Kiểm tra lại tất cả thông tin đã nhập:
- Đã chọn sản phẩm/dịch vụ
- Đã có mã biến thể
- Đã chọn đơn vị tính
- Đã có ít nhất một thuộc tính với giá trị hợp lệ

**Bước 2**: Nhấn nút **"Lưu"** (biểu tượng 💾) trên thanh công cụ

**Bước 3**: Hệ thống sẽ:
- Kiểm tra tính hợp lệ của dữ liệu:
  - Sản phẩm/dịch vụ không được để trống
  - Mã biến thể không được để trống
  - Đơn vị tính không được để trống
  - Phải có ít nhất một thuộc tính
  - Tất cả thuộc tính phải có tên và giá trị
  - Giá trị phải phù hợp với kiểu dữ liệu của thuộc tính
- Nếu có lỗi, hệ thống sẽ hiển thị thông báo lỗi và yêu cầu sửa
- Nếu hợp lệ, hệ thống sẽ:
  - Hiển thị màn hình chờ trong vài giây
  - Lưu dữ liệu vào hệ thống
  - Tự động tính toán tên đầy đủ của biến thể (VariantFullName) từ các thuộc tính
  - Hiển thị thông báo: "Lưu dữ liệu thành công!"
  - Tự động đóng màn hình

**Lưu ý**: 
- Bạn phải có ít nhất một thuộc tính với giá trị hợp lệ trước khi có thể lưu
- Tên đầy đủ của biến thể sẽ được tự động tạo từ các thuộc tính (ví dụ: "Màu sắc : Đỏ, Kích thước : XL")

---

### 5. Hủy và Đóng Màn Hình

**Bước 1**: Nhấn nút **"Đóng"** (biểu tượng ❌) trên thanh công cụ

**Bước 2**: Nếu có thay đổi chưa lưu, hệ thống sẽ hiển thị hộp thoại xác nhận: "Có thay đổi chưa được lưu. Bạn có chắc chắn muốn đóng?"

**Bước 3**: 
- Nhấn **"Có"** (Yes) nếu bạn muốn đóng mà không lưu
- Nhấn **"Không"** (No) nếu bạn muốn quay lại để lưu

**Bước 4**: Màn hình sẽ đóng

**Lưu ý**: 
- Tất cả thay đổi chưa lưu sẽ bị mất
- Nếu bạn đã lưu, màn hình sẽ tự động đóng sau khi lưu thành công

---

## Hiểu Về Giao Diện

### Thanh Công Cụ (Phía Trên)

Các nút chức năng chính:
- **💾 Lưu**: Lưu thông tin biến thể sản phẩm vào hệ thống
- **❌ Đóng**: Đóng màn hình mà không lưu thay đổi

### Các Trường Thông Tin

**1. Sản phẩm dịch vụ**
- **Loại**: SearchLookupEdit (tìm kiếm và chọn)
- **Bắt buộc**: Có (có dấu * đỏ)
- **Mô tả**: Chọn sản phẩm hoặc dịch vụ gốc từ danh sách
- **Khi chỉnh sửa**: Bị khóa (không thể thay đổi)

**2. Đơn vị tính**
- **Loại**: SearchLookUpEdit (tìm kiếm và chọn)
- **Bắt buộc**: Có (có dấu * đỏ)
- **Mô tả**: Chọn đơn vị tính cho biến thể
- **Khi chỉnh sửa**: Vẫn có thể thay đổi

**3. Mã biến thể**
- **Loại**: TextEdit (nhập text)
- **Bắt buộc**: Có (có dấu * đỏ)
- **Mô tả**: Mã duy nhất để nhận diện biến thể
- **Tự động tạo**: Có (khi chọn sản phẩm và đơn vị tính trong chế độ thêm mới)
- **Format**: [Mã sản phẩm]_[Mã đơn vị]_[Số thứ tự 4 chữ số]
- **Khi chỉnh sửa**: Bị khóa (không thể thay đổi)

**4. Trạng thái**
- **Loại**: ToggleSwitch (công tắc bật/tắt)
- **Bắt buộc**: Không
- **Mặc định**: Bật (Đang sử dụng)
- **Mô tả**: Trạng thái hoạt động của biến thể

### Bảng Thuộc Tính (Phía Dưới)

**Tiêu đề**: "DANH SÁCH CÁC THUỘC TÍNH BIẾN THỂ"

**Mỗi dòng hiển thị**:
- **Tên thuộc tính**: Chọn từ danh sách thả xuống (ví dụ: Màu sắc, Kích thước, Dung lượng)
- **Giá trị**: Nhập giá trị tương ứng (ví dụ: Đỏ, XL, 500ml)

**Tính năng**:
- **Thêm dòng mới**: Nhấn nút "+" trên EmbeddedNavigator hoặc nhấn vào dòng "Thêm mới" ở đầu bảng
- **Xóa dòng**: Chọn dòng và nhấn nút "X" trên EmbeddedNavigator
- **Chỉnh sửa**: Nhấn vào ô để chỉnh sửa trực tiếp
- **Validation**: Hệ thống tự động kiểm tra giá trị khi bạn nhập

**Thanh điều hướng (EmbeddedNavigator)**:
- Nút **"+"**: Thêm dòng mới
- Nút **"X"**: Xóa dòng đã chọn
- Các nút khác: Điều hướng trong bảng

### Thông Báo Lỗi

Khi có lỗi validation, hệ thống sẽ:
- Hiển thị thông báo lỗi trong ô bị lỗi
- Hiển thị hộp thoại cảnh báo với danh sách lỗi
- Không cho phép lưu cho đến khi sửa hết lỗi

---

## Lưu Ý Quan Trọng

### 1. Về Sản Phẩm/Dịch Vụ

- **Bắt buộc**: Phải chọn sản phẩm hoặc dịch vụ gốc
- **Khi chỉnh sửa**: Không thể thay đổi (bị khóa)
- **Chỉ hiển thị**: Các sản phẩm/dịch vụ đang hoạt động

### 2. Về Mã Biến Thể

- **Bắt buộc**: Mã không được để trống
- **Duy nhất**: Mã phải duy nhất trong hệ thống
- **Tự động tạo**: Khi thêm mới và chọn sản phẩm + đơn vị tính, mã sẽ được tự động tạo
- **Format**: [Mã sản phẩm]_[Mã đơn vị]_[0001-9999]
- **Khi chỉnh sửa**: Không thể thay đổi (bị khóa)

### 3. Về Đơn Vị Tính

- **Bắt buộc**: Phải chọn đơn vị tính
- **Khi chỉnh sửa**: Vẫn có thể thay đổi
- **Chỉ hiển thị**: Các đơn vị tính đang hoạt động

### 4. Về Thuộc Tính

- **Bắt buộc**: Phải có ít nhất một thuộc tính
- **Không trùng lặp**: Mỗi thuộc tính chỉ được chọn một lần trong cùng một biến thể
- **Giá trị bắt buộc**: Mỗi thuộc tính phải có giá trị
- **Kiểu dữ liệu**: Giá trị phải phù hợp với kiểu dữ liệu của thuộc tính
- **Giới hạn**: Giá trị tối đa 255 ký tự

### 5. Về Tự Động Tạo Mã

- **Chỉ khi thêm mới**: Mã chỉ được tự động tạo khi bạn đang thêm mới (không phải chỉnh sửa)
- **Khi chọn sản phẩm + đơn vị tính**: Mã sẽ được tự động tạo ngay khi bạn chọn cả hai
- **Format**: [Mã sản phẩm]_[Mã đơn vị]_[Số thứ tự 4 chữ số]
- **Số thứ tự**: Tự động tăng từ 0001 đến 9999
- **Có thể chỉnh sửa**: Bạn có thể chỉnh sửa mã tự động nếu cần (nhưng phải đảm bảo mã duy nhất)

### 6. Về Validation (Kiểm Tra Dữ Liệu)

- **Kiểm tra khi lưu**: Hệ thống sẽ kiểm tra tất cả dữ liệu khi bạn nhấn "Lưu"
- **Kiểm tra khi nhập**: Hệ thống sẽ kiểm tra giá trị thuộc tính ngay khi bạn nhập
- **Hiển thị lỗi**: Nếu có lỗi, hệ thống sẽ hiển thị thông báo và không cho phép lưu
- **Phải sửa lỗi**: Bạn phải sửa tất cả lỗi trước khi có thể lưu thành công

### 7. Về Trạng Thái

- **Mặc định**: Đang sử dụng (bật)
- **Không sử dụng**: Biến thể không sử dụng sẽ không hiển thị trong một số danh sách lựa chọn
- **Có thể thay đổi**: Bạn có thể thay đổi trạng thái bất cứ lúc nào

### 8. Về Xóa Thuộc Tính Cuối Cùng

- **Cảnh báo đặc biệt**: Nếu bạn xóa thuộc tính cuối cùng khi đang chỉnh sửa, hệ thống sẽ hỏi có muốn xóa toàn bộ biến thể không
- **Xóa biến thể**: Nếu bạn chọn "Có", toàn bộ biến thể sản phẩm sẽ bị xóa và màn hình sẽ đóng
- **Hủy xóa**: Nếu bạn chọn "Không", thao tác xóa sẽ bị hủy

### 9. Về Tên Đầy Đủ Biến Thể (VariantFullName)

- **Tự động tạo**: Tên đầy đủ sẽ được tự động tạo từ các thuộc tính khi lưu
- **Format**: "Tên thuộc tính 1 : Giá trị 1, Tên thuộc tính 2 : Giá trị 2, ..."
- **Ví dụ**: "Màu sắc : Đỏ, Kích thước : XL, Dung lượng : 500ml"

---

## Lỗi Thường Gặp

### 1. "Vui lòng chọn sản phẩm/dịch vụ."

**Nguyên nhân**: Bạn chưa chọn sản phẩm/dịch vụ

**Cách xử lý**:
- Nhấn vào ô "Sản phẩm dịch vụ" và chọn một sản phẩm/dịch vụ từ danh sách

---

### 2. "Vui lòng nhập mã biến thể."

**Nguyên nhân**: Bạn chưa nhập mã biến thể

**Cách xử lý**:
- Nhập mã biến thể vào ô "Mã biến thể"
- Hoặc chọn sản phẩm và đơn vị tính để mã được tự động tạo (chế độ thêm mới)

---

### 3. "Vui lòng chọn đầy đủ thuộc tính cho tất cả dòng."

**Nguyên nhân**: Có dòng thuộc tính chưa chọn tên thuộc tính

**Cách xử lý**:
- Chọn tên thuộc tính cho tất cả các dòng trong bảng
- Hoặc xóa các dòng không cần thiết

---

### 4. "Vui lòng nhập đầy đủ giá trị cho tất cả thuộc tính."

**Nguyên nhân**: Có dòng thuộc tính chưa nhập giá trị

**Cách xử lý**:
- Nhập giá trị cho tất cả các dòng trong bảng
- Hoặc xóa các dòng không cần thiết

---

### 5. "Giá trị thuộc tính không được để trống"

**Nguyên nhân**: Bạn chưa nhập giá trị cho thuộc tính

**Cách xử lý**:
- Nhập giá trị vào ô "Giá trị" của dòng thuộc tính

---

### 6. "Giá trị thuộc tính không được vượt quá 255 ký tự"

**Nguyên nhân**: Giá trị bạn nhập quá dài (hơn 255 ký tự)

**Cách xử lý**:
- Rút ngắn giá trị xuống còn tối đa 255 ký tự

---

### 7. "Giá trị phải là số nguyên"

**Nguyên nhân**: Thuộc tính có kiểu dữ liệu là số nguyên, nhưng bạn nhập giá trị không phải số nguyên

**Cách xử lý**:
- Nhập một số nguyên (ví dụ: 1, 100, -50)
- Không được nhập số thực (ví dụ: 1.5, 100.99)

---

### 8. "Giá trị phải là số"

**Nguyên nhân**: Thuộc tính có kiểu dữ liệu là số, nhưng bạn nhập giá trị không phải số

**Cách xử lý**:
- Nhập một số (có thể là số nguyên hoặc số thực)
- Ví dụ: 1, 100, 1.5, 100.99, -50.5

---

### 9. "Giá trị phải là ngày hợp lệ"

**Nguyên nhân**: Thuộc tính có kiểu dữ liệu là ngày, nhưng bạn nhập giá trị không phải ngày

**Cách xử lý**:
- Nhập một ngày hợp lệ theo định dạng ngày của hệ thống
- Ví dụ: 01/01/2025

---

### 10. "Giá trị phải là ngày giờ hợp lệ"

**Nguyên nhân**: Thuộc tính có kiểu dữ liệu là ngày giờ, nhưng bạn nhập giá trị không phải ngày giờ

**Cách xử lý**:
- Nhập một ngày giờ hợp lệ theo định dạng ngày giờ của hệ thống
- Ví dụ: 01/01/2025 10:30

---

### 11. "Giá trị phải là kiểu đúng/sai (true/false, 1/0, có/không)"

**Nguyên nhân**: Thuộc tính có kiểu dữ liệu là đúng/sai, nhưng bạn nhập giá trị không phù hợp

**Cách xử lý**:
- Nhập một trong các giá trị: true, false, 1, 0, có, không, yes, no

---

### 12. "Vui lòng chọn 'Tên thuộc tính' trước khi nhập giá trị"

**Nguyên nhân**: Bạn đang cố nhập giá trị nhưng chưa chọn tên thuộc tính

**Cách xử lý**:
- Chọn tên thuộc tính trước
- Sau đó mới nhập giá trị

---

### 13. "Thuộc tính '[Tên]' đã được sử dụng. Vui lòng chọn thuộc tính khác."

**Nguyên nhân**: Bạn đang cố chọn thuộc tính đã được sử dụng trong biến thể này

**Cách xử lý**:
- Chọn một thuộc tính khác chưa được sử dụng
- Hoặc xóa dòng đang sử dụng thuộc tính đó trước

---

### 14. "Tất cả thuộc tính đã được sử dụng. Không thể thêm dòng mới."

**Nguyên nhân**: Bạn đã sử dụng tất cả thuộc tính có sẵn trong hệ thống

**Cách xử lý**:
- Không thể thêm thuộc tính mới nữa
- Nếu cần thêm thuộc tính mới, hãy tạo thuộc tính mới trong hệ thống trước

---

### 15. "Lỗi tải dữ liệu"

**Nguyên nhân**: Có lỗi kỹ thuật khi tải dữ liệu từ máy chủ

**Cách xử lý**:
- Kiểm tra kết nối mạng/internet
- Thử lại sau vài phút
- Nếu vẫn lỗi, liên hệ bộ phận IT

---

### 16. "Lỗi lưu dữ liệu"

**Nguyên nhân**: Có lỗi kỹ thuật khi lưu dữ liệu

**Cách xử lý**:
- Kiểm tra kết nối mạng/internet
- Kiểm tra lại tất cả thông tin đã nhập
- Thử lại sau vài phút
- Nếu vẫn lỗi, liên hệ bộ phận IT

---

### 17. "Lỗi sinh mã biến thể"

**Nguyên nhân**: Có lỗi khi hệ thống tự động tạo mã

**Cách xử lý**:
- Nhập mã thủ công thay vì để hệ thống tự động tạo
- Nếu vẫn lỗi, liên hệ bộ phận IT

---

### 18. "Lỗi tải danh sách sản phẩm" / "Lỗi tải danh sách đơn vị tính"

**Nguyên nhân**: Có lỗi khi tải danh sách từ máy chủ

**Cách xử lý**:
- Kiểm tra kết nối mạng/internet
- Thử lại sau vài phút
- Nếu vẫn lỗi, liên hệ bộ phận IT

---

## Câu Hỏi Thường Gặp

### Q1: Tại sao mã biến thể được tự động tạo khi tôi chọn sản phẩm và đơn vị tính?

**Trả lời**: Đây là tính năng tiện ích của hệ thống. Khi bạn chọn sản phẩm và đơn vị tính trong chế độ thêm mới, hệ thống sẽ tự động tạo mã biến thể theo format: [Mã sản phẩm]_[Mã đơn vị]_[Số thứ tự] để đảm bảo tính nhất quán và dễ quản lý. Bạn vẫn có thể chỉnh sửa mã tự động nếu cần.

---

### Q2: Tôi có thể để trống mã biến thể không?

**Trả lời**: Không, mã biến thể là trường bắt buộc. Bạn phải nhập mã hoặc chọn sản phẩm và đơn vị tính để mã được tự động tạo.

---

### Q3: Tại sao khi chỉnh sửa, tôi không thể thay đổi sản phẩm/dịch vụ và mã biến thể?

**Trả lời**: Đây là quy tắc của hệ thống để đảm bảo tính toàn vẹn dữ liệu. Khi chỉnh sửa, sản phẩm/dịch vụ và mã biến thể không thể thay đổi vì chúng là dữ liệu cốt lõi của biến thể. Bạn vẫn có thể thay đổi đơn vị tính và các thuộc tính.

---

### Q4: Tôi có thể thêm bao nhiêu thuộc tính cho một biến thể?

**Trả lời**: Bạn có thể thêm nhiều thuộc tính cho một biến thể, nhưng mỗi thuộc tính chỉ được chọn một lần. Số lượng thuộc tính tối đa phụ thuộc vào số lượng thuộc tính có sẵn trong hệ thống.

---

### Q5: Tại sao tôi không thể chọn thuộc tính đã được sử dụng?

**Trả lời**: Hệ thống yêu cầu mỗi thuộc tính chỉ được chọn một lần trong cùng một biến thể để tránh trùng lặp. Nếu bạn cần sử dụng lại thuộc tính, hãy xóa dòng đang sử dụng thuộc tính đó trước.

---

### Q6: Tại sao khi tôi nhập giá trị, hệ thống báo lỗi?

**Trả lời**: Có thể do:
- Giá trị không phù hợp với kiểu dữ liệu của thuộc tính (ví dụ: nhập chữ cho thuộc tính số)
- Giá trị quá dài (hơn 255 ký tự)
- Giá trị để trống
- Hãy kiểm tra kiểu dữ liệu của thuộc tính và nhập giá trị phù hợp

---

### Q7: Kiểu dữ liệu của thuộc tính là gì?

**Trả lời**: Kiểu dữ liệu xác định loại giá trị bạn có thể nhập:
- **Số nguyên**: Chỉ chấp nhận số nguyên (1, 100, -50)
- **Số thực**: Chấp nhận số thực (1.5, 100.99, -50.5)
- **Ngày**: Chấp nhận định dạng ngày (01/01/2025)
- **Ngày giờ**: Chấp nhận định dạng ngày giờ (01/01/2025 10:30)
- **Đúng/Sai**: Chấp nhận true/false, 1/0, có/không, yes/no
- **Text**: Chấp nhận bất kỳ văn bản nào

---

### Q8: Tại sao khi tôi xóa thuộc tính cuối cùng, hệ thống hỏi có muốn xóa toàn bộ biến thể không?

**Trả lời**: Một biến thể sản phẩm phải có ít nhất một thuộc tính. Nếu bạn xóa thuộc tính cuối cùng, biến thể sẽ không còn thuộc tính nào, do đó hệ thống hỏi bạn có muốn xóa toàn bộ biến thể không. Nếu bạn chọn "Có", toàn bộ biến thể sẽ bị xóa.

---

### Q9: Tôi có thể xóa toàn bộ biến thể bằng cách xóa tất cả thuộc tính không?

**Trả lời**: Có, khi bạn xóa thuộc tính cuối cùng, hệ thống sẽ hỏi có muốn xóa toàn bộ biến thể không. Nếu bạn chọn "Có", toàn bộ biến thể sẽ bị xóa.

---

### Q10: Tên đầy đủ biến thể (VariantFullName) là gì?

**Trả lời**: Tên đầy đủ biến thể là tên mô tả đầy đủ của biến thể, được tự động tạo từ các thuộc tính khi lưu. Ví dụ: "Màu sắc : Đỏ, Kích thước : XL, Dung lượng : 500ml". Tên này giúp bạn dễ dàng nhận biết biến thể trong danh sách.

---

### Q11: Tôi có thể sử dụng phím tắt không?

**Trả lời**: Bạn có thể sử dụng:
- **Enter**: Lưu dòng hiện tại trong bảng thuộc tính
- **Tab**: Di chuyển sang ô tiếp theo
- Các phím tắt khác tùy theo cấu hình của hệ thống

---

### Q12: Tại sao khi tôi chọn sản phẩm và đơn vị tính, mã không được tự động tạo?

**Trả lời**: Mã chỉ được tự động tạo khi bạn đang ở chế độ **thêm mới**. Nếu bạn đang chỉnh sửa biến thể đã có, mã sẽ không được tự động tạo.

---

### Q13: Tôi có thể thay đổi đơn vị tính khi chỉnh sửa không?

**Trả lời**: Có, bạn có thể thay đổi đơn vị tính khi chỉnh sửa. Trường này không bị khóa như sản phẩm/dịch vụ và mã biến thể.

---

### Q14: Tại sao tôi không thể lưu khi có lỗi validation?

**Trả lời**: Hệ thống yêu cầu tất cả dữ liệu phải hợp lệ trước khi lưu. Bạn cần sửa tất cả lỗi được hiển thị trước khi có thể lưu thành công.

---

### Q15: Tôi có thể hủy thay đổi và đóng màn hình không?

**Trả lời**: Có, bạn có thể nhấn nút "Đóng" để đóng màn hình. Nếu có thay đổi chưa lưu, hệ thống sẽ hỏi xác nhận. Tất cả thay đổi chưa lưu sẽ bị mất.

---

### Q16: Tại sao sau khi lưu, màn hình tự động đóng?

**Trả lời**: Đây là hành vi mặc định của hệ thống. Sau khi lưu thành công, màn hình sẽ tự động đóng để bạn quay lại danh sách và xem kết quả.

---

### Q17: Tôi có thể mở nhiều màn hình chi tiết cùng lúc không?

**Trả lời**: Có, bạn có thể mở nhiều màn hình chi tiết cùng lúc để thêm mới hoặc chỉnh sửa nhiều biến thể sản phẩm.

---

### Q18: Format mã biến thể tự động là gì?

**Trả lời**: Format mã biến thể tự động là: `[Mã sản phẩm]_[Mã đơn vị]_[Số thứ tự 4 chữ số]`

Ví dụ:
- Sản phẩm có mã "SP001", đơn vị có mã "KG" → Mã biến thể: "SP001_KG_0001", "SP001_KG_0002", v.v.
- Sản phẩm có mã "DV002", đơn vị có mã "GIO" → Mã biến thể: "DV002_GIO_0001", "DV002_GIO_0002", v.v.

---

### Q19: Số thứ tự trong mã biến thể được tính như thế nào?

**Trả lời**: Số thứ tự được tính tự động dựa trên các biến thể hiện có của cùng sản phẩm và đơn vị tính. Hệ thống sẽ tìm số lớn nhất và tăng thêm 1. Số thứ tự có thể từ 0001 đến 9999.

---

### Q20: Tôi có thể chỉnh sửa mã biến thể tự động không?

**Trả lời**: Có, bạn có thể chỉnh sửa mã tự động nếu cần, nhưng phải đảm bảo mã duy nhất (không trùng với mã khác trong hệ thống).

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
- Mã biến thể hoặc tên sản phẩm đang thao tác (nếu có)

---

**Tài liệu này được cập nhật lần cuối: 2025-01-XX**

**Phiên bản: 1.0**
