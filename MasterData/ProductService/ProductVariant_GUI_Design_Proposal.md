# 🖥️ ProductVariant GUI Design Proposal

## 🎯 **Tổng Quan Màn Hình**

**ProductVariant Management** là hệ thống quản lý các biến thể sản phẩm/dịch vụ, cho phép người dùng tạo, chỉnh sửa, xóa và quản lý các biến thể của sản phẩm một cách hiệu quả.

---

## 🏗️ **Kiến Trúc Màn Hình**

### **📋 Danh Sách Màn Hình Đề Xuất**

#### **1. UcProductVariantList.cs** - Danh sách biến thể
#### **2. FrmProductVariantDetail.cs** - Chi tiết biến thể
#### **3. FrmProductVariantBulkCreate.cs** - Tạo hàng loạt biến thể
#### **4. FrmProductVariantPriceManagement.cs** - Quản lý giá biến thể

---

## 📊 **1. UcProductVariantList - Danh Sách Biến Thể**

### **🎨 Layout Design**
```
┌─────────────────────────────────────────────────────────────────┐
│ 🔧 Toolbar                                                      │
├─────────────────────────────────────────────────────────────────┤
│ 📋 Grid View (AdvBandedGridView)                                │
│ ┌─────────┬─────────┬─────────┬─────────┬─────────┬─────────┐   │
│ │ Chọn    │ Mã SP   │ Tên SP  │ Mã VT   │ Đơn Vị  │ Giá Mua │   │
│ ├─────────┼─────────┼─────────┼─────────┼─────────┼─────────┤   │
│ │ ☑       │ SP001   │ Áo thun │ VT001   │ Cái     │ 100,000 │   │
│ │ ☑       │ SP001   │ Áo thun │ VT002   │ Cái     │ 120,000 │   │
│ └─────────┴─────────┴─────────┴─────────┴─────────┴─────────┘   │
├─────────────────────────────────────────────────────────────────┤
│ 📄 Status Bar                                                   │
└─────────────────────────────────────────────────────────────────┘
```

### **🔧 Toolbar Controls**
- **📋 Danh Sách**: Tải lại dữ liệu
- **➕ Mới**: Tạo biến thể mới
- **✏️ Điều Chỉnh**: Chỉnh sửa biến thể đã chọn
- **🗑️ Xóa**: Xóa biến thể đã chọn
- **📊 Tạo Hàng Loạt**: Tạo nhiều biến thể cùng lúc
- **💰 Quản Lý Giá**: Quản lý giá cho nhiều biến thể
- **📤 Xuất**: Xuất dữ liệu ra Excel
- **🔍 Lọc Dữ Liệu**: Tìm kiếm toàn diện

### **📊 Grid Columns**
| Cột | Tên | Mô tả | Width | Type |
|-----|-----|-------|-------|------|
| 1 | Chọn | Checkbox để chọn | 50px | CheckBox |
| 2 | Mã SP | Mã sản phẩm gốc | 80px | Text |
| 3 | Tên SP | Tên sản phẩm gốc | 200px | Text |
| 4 | Mã VT | Mã biến thể | 80px | Text |
| 5 | Đơn Vị | Đơn vị tính | 80px | Text |
| 6 | Giá Mua | Giá mua vào | 100px | Currency |
| 7 | Giá Bán | Giá bán ra | 100px | Currency |
| 8 | Trạng Thái | Hoạt động/Không hoạt động | 80px | Status |
| 9 | Ảnh | Thumbnail image | 60px | Image |
| 10 | Số Thuộc Tính | Số lượng thuộc tính | 80px | Number |
| 11 | Số Hình Ảnh | Số lượng hình ảnh | 80px | Number |

### **🎨 Visual Features**
- **Màu sắc phân biệt**: 
  - 🟢 Xanh lá: Biến thể hoạt động
  - 🔴 Đỏ: Biến thể không hoạt động
- **Highlight tìm kiếm**: Từ khóa được tô màu đỏ và bold
- **Responsive design**: Tự động điều chỉnh chiều cao dòng

---

## 📝 **2. FrmProductVariantDetail - Chi Tiết Biến Thể**

### **🎨 Layout Design**
```
┌─────────────────────────────────────────────────────────────────┐
│ 📋 Tab Control                                                  │
├─────────────────────────────────────────────────────────────────┤
│ 📊 Tab 1: Thông Tin Cơ Bản                                     │
│ ┌─────────────────┬─────────────────────────────────────────┐   │
│ │ Sản phẩm gốc    │ [ComboBox: Chọn sản phẩm]              │   │
│ │ Mã biến thể     │ [TextEdit: VT001]                      │   │
│ │ Đơn vị tính     │ [ComboBox: Chọn đơn vị]                │   │
│ │ Giá mua vào     │ [SpinEdit: 100,000]                    │   │
│ │ Giá bán ra      │ [SpinEdit: 150,000]                    │   │
│ │ Trạng thái      │ [CheckEdit: ☑ Hoạt động]               │   │
│ └─────────────────┴─────────────────────────────────────────┘   │
├─────────────────────────────────────────────────────────────────┤
│ 🎨 Tab 2: Thuộc Tính                                           │
│ ┌─────────────────────────────────────────────────────────────┐   │
│ │ [DataGrid: Danh sách thuộc tính]                            │   │
│ │ ┌─────────┬─────────┬─────────┬─────────┬─────────┐         │   │
│ │ │ Thuộc   │ Giá Trị │ Mô Tả   │ Thứ Tự  │ Hành   │         │   │
│ │ │ Tính    │         │         │         │ Động   │         │   │
│ │ ├─────────┼─────────┼─────────┼─────────┼─────────┤         │   │
│ │ │ Màu sắc │ Đỏ      │ Màu đỏ  │ 1       │ [Sửa]  │         │   │
│ │ │ Kích    │ L       │ Size L  │ 2       │ [Sửa]  │         │   │
│ │ │ thước   │         │         │         │         │         │   │
│ │ └─────────┴─────────┴─────────┴─────────┴─────────┘         │   │
│ │ [Button: + Thêm thuộc tính]                                 │   │
│ └─────────────────────────────────────────────────────────────┘   │
├─────────────────────────────────────────────────────────────────┤
│ 🖼️ Tab 3: Hình Ảnh                                             │
│ ┌─────────────────────────────────────────────────────────────┐   │
│ │ [Image Gallery: Hiển thị hình ảnh]                          │   │
│ │ [Button: + Thêm hình ảnh] [Button: - Xóa hình ảnh]         │   │
│ └─────────────────────────────────────────────────────────────┘   │
├─────────────────────────────────────────────────────────────────┤
│ 🔧 Action Buttons                                               │
│ [Lưu] [Lưu & Đóng] [Đóng] [Hủy]                               │
└─────────────────────────────────────────────────────────────────┘
```

### **📊 Tab 1: Thông Tin Cơ Bản**
| Field | Control | Validation | Description |
|-------|---------|------------|-------------|
| Sản phẩm gốc | ComboBox | Required | Chọn sản phẩm từ danh sách |
| Mã biến thể | TextEdit | Required, Unique | Mã duy nhất cho biến thể |
| Đơn vị tính | ComboBox | Required | Chọn đơn vị từ UnitOfMeasure |
| Giá mua vào | SpinEdit | >= 0 | Giá mua vào (có thể null) |
| Giá bán ra | SpinEdit | >= 0 | Giá bán ra (có thể null) |
| Trạng thái | CheckEdit | - | Hoạt động/Không hoạt động |

### **🎨 Tab 2: Thuộc Tính**
- **DataGrid**: Hiển thị danh sách thuộc tính
- **Add Button**: Thêm thuộc tính mới
- **Edit/Delete**: Sửa/xóa thuộc tính
- **Drag & Drop**: Sắp xếp thứ tự thuộc tính

### **🖼️ Tab 3: Hình Ảnh**
- **Image Gallery**: Hiển thị hình ảnh dạng thumbnail
- **Upload**: Thêm hình ảnh mới
- **Delete**: Xóa hình ảnh
- **Set Primary**: Đặt ảnh chính
- **Preview**: Xem ảnh full size

---

## 📦 **3. FrmProductVariantBulkCreate - Tạo Hàng Loạt**

### **🎨 Layout Design**
```
┌─────────────────────────────────────────────────────────────────┐
│ 📋 Tạo Biến Thể Hàng Loạt                                      │
├─────────────────────────────────────────────────────────────────┤
│ 📊 Bước 1: Chọn Sản Phẩm                                       │
│ ┌─────────────────────────────────────────────────────────────┐   │
│ │ Sản phẩm: [ComboBox: Chọn sản phẩm]                        │   │
│ │ Đơn vị mặc định: [ComboBox: Chọn đơn vị]                   │   │
│ └─────────────────────────────────────────────────────────────┘   │
├─────────────────────────────────────────────────────────────────┤
│ 📊 Bước 2: Cấu Hình Thuộc Tính                                 │
│ ┌─────────────────────────────────────────────────────────────┐   │
│ │ [DataGrid: Danh sách thuộc tính]                            │   │
│ │ ┌─────────┬─────────┬─────────┬─────────┬─────────┐         │   │
│ │ │ Thuộc   │ Giá Trị │ Mô Tả   │ Thứ Tự  │ Hành   │         │   │
│ │ │ Tính    │         │         │         │ Động   │         │   │
│ │ ├─────────┼─────────┼─────────┼─────────┼─────────┤         │   │
│ │ │ Màu sắc │ Đỏ,Xanh │ Màu sắc │ 1       │ [Sửa]  │         │   │
│ │ │ Kích    │ S,M,L   │ Kích    │ 2       │ [Sửa]  │         │   │
│ │ │ thước   │         │ thước   │         │         │         │   │
│ │ └─────────┴─────────┴─────────┴─────────┴─────────┘         │   │
│ │ [Button: + Thêm thuộc tính]                                 │   │
│ └─────────────────────────────────────────────────────────────┘   │
├─────────────────────────────────────────────────────────────────┤
│ 📊 Bước 3: Xem Trước Kết Quả                                   │
│ ┌─────────────────────────────────────────────────────────────┐   │
│ │ Sẽ tạo 6 biến thể:                                          │   │
│ │ • VT001: Màu Đỏ - Size S                                    │   │
│ │ • VT002: Màu Đỏ - Size M                                    │   │
│ │ • VT003: Màu Đỏ - Size L                                    │   │
│ │ • VT004: Màu Xanh - Size S                                  │   │
│ │ • VT005: Màu Xanh - Size M                                  │   │
│ │ • VT006: Màu Xanh - Size L                                  │   │
│ └─────────────────────────────────────────────────────────────┘   │
├─────────────────────────────────────────────────────────────────┤
│ 🔧 Action Buttons                                               │
│ [Tạo Biến Thể] [Hủy]                                           │
└─────────────────────────────────────────────────────────────────┘
```

### **🔄 Workflow**
1. **Chọn sản phẩm**: Chọn sản phẩm gốc
2. **Cấu hình thuộc tính**: Định nghĩa các thuộc tính và giá trị
3. **Xem trước**: Hiển thị danh sách biến thể sẽ được tạo
4. **Tạo biến thể**: Thực hiện tạo hàng loạt

---

## 💰 **4. FrmProductVariantPriceManagement - Quản Lý Giá**

### **🎨 Layout Design**
```
┌─────────────────────────────────────────────────────────────────┐
│ 💰 Quản Lý Giá Biến Thể                                        │
├─────────────────────────────────────────────────────────────────┤
│ 📊 Bộ Lọc                                                       │
│ ┌─────────────────────────────────────────────────────────────┐   │
│ │ Sản phẩm: [ComboBox] | Danh mục: [ComboBox] | Trạng thái: [ComboBox] │
│ └─────────────────────────────────────────────────────────────┘   │
├─────────────────────────────────────────────────────────────────┤
│ 📊 Danh Sách Biến Thể                                           │
│ ┌─────────────────────────────────────────────────────────────┐   │
│ │ [DataGrid: Danh sách biến thể với giá]                      │   │
│ │ ┌─────────┬─────────┬─────────┬─────────┬─────────┐         │   │
│ │ │ Chọn    │ Mã SP   │ Mã VT   │ Giá Mua │ Giá Bán │         │   │
│ │ ├─────────┼─────────┼─────────┼─────────┼─────────┤         │   │
│ │ │ ☑       │ SP001   │ VT001   │ 100,000 │ 150,000 │         │   │
│ │ │ ☑       │ SP001   │ VT002   │ 120,000 │ 180,000 │         │   │
│ │ └─────────┴─────────┴─────────┴─────────┴─────────┘         │   │
│ └─────────────────────────────────────────────────────────────┘   │
├─────────────────────────────────────────────────────────────────┤
│ 💰 Cập Nhật Giá Hàng Loạt                                      │
│ ┌─────────────────────────────────────────────────────────────┐   │
│ │ Loại cập nhật: [RadioButton: Tăng %, Tăng số, Đặt giá mới] │   │
│ │ Giá trị: [SpinEdit: 10] [Label: % hoặc VND]                │   │
│ │ Áp dụng cho: [CheckBox: Giá mua] [CheckBox: Giá bán]       │   │
│ │ [Button: Cập Nhật Giá]                                      │   │
│ └─────────────────────────────────────────────────────────────┘   │
├─────────────────────────────────────────────────────────────────┤
│ 🔧 Action Buttons                                               │
│ [Lưu] [Hủy] [Xuất Excel]                                       │
└─────────────────────────────────────────────────────────────────┘
```

### **💰 Tính Năng Quản Lý Giá**
- **Cập nhật hàng loạt**: Tăng/giảm giá theo % hoặc số tiền
- **Import giá từ Excel**: Import giá từ file Excel
- **Lịch sử thay đổi giá**: Theo dõi lịch sử thay đổi
- **Báo cáo giá**: Xuất báo cáo giá ra Excel

---

## 🎨 **UI/UX Design Guidelines**

### **🎨 Color Scheme**
- **Primary**: #2E86AB (Blue)
- **Secondary**: #A23B72 (Purple)
- **Success**: #F18F01 (Orange)
- **Warning**: #C73E1D (Red)
- **Info**: #7209B7 (Purple)

### **📱 Responsive Design**
- **Grid**: Tự động điều chỉnh chiều cao dòng
- **Columns**: Tự động resize theo nội dung
- **Mobile**: Responsive cho tablet/mobile

### **♿ Accessibility**
- **Keyboard Navigation**: Hỗ trợ điều hướng bằng phím
- **Screen Reader**: Hỗ trợ đọc màn hình
- **High Contrast**: Chế độ tương phản cao

---

## 🔧 **Technical Implementation**

### **📊 Data Binding**
```csharp
// Binding source cho grid
private BindingSource productVariantBindingSource;

// Binding source cho combo box
private BindingSource productServiceBindingSource;
private BindingSource unitOfMeasureBindingSource;
```

### **🔄 Event Handlers**
```csharp
// Grid events
ProductVariantGridView.SelectionChanged += OnSelectionChanged;
ProductVariantGridView.CustomDrawRowIndicator += OnCustomDrawRowIndicator;

// Toolbar events
NewButton.Click += OnNewButtonClick;
EditButton.Click += OnEditButtonClick;
DeleteButton.Click += OnDeleteButtonClick;
```

### **⚡ Performance Optimization**
- **Pagination**: Phân trang cho dữ liệu lớn
- **Lazy Loading**: Tải dữ liệu khi cần
- **Caching**: Cache dữ liệu thường dùng
- **Async Operations**: Tất cả thao tác bất đồng bộ

---

## 🚀 **Advanced Features**

### **🔍 Search & Filter**
- **Quick Search**: Tìm kiếm nhanh trong grid
- **Advanced Filter**: Bộ lọc nâng cao
- **Saved Filters**: Lưu bộ lọc thường dùng

### **📊 Analytics**
- **Sales Analysis**: Phân tích bán hàng theo biến thể
- **Price Analysis**: Phân tích giá theo thời gian
- **Inventory Analysis**: Phân tích tồn kho

### **🔄 Integration**
- **Excel Import/Export**: Import/export dữ liệu
- **API Integration**: Tích hợp với hệ thống khác
- **Real-time Sync**: Đồng bộ real-time

---

## 📚 **Related Documentation**

- [ProductVariant Entity Overview](./ProductVariant_Entity_Overview.md)
- [ProductService GUI Design](./ProductService_GUI_Design.md)
- [UnitOfMeasure GUI Design](./UnitOfMeasure_GUI_Design.md)
- [VariantAttribute GUI Design](./VariantAttribute_GUI_Design.md)

---

**🎉 ProductVariant GUI Design đã sẵn sàng để triển khai!**

*Thiết kế này cung cấp giao diện người dùng trực quan và hiệu quả cho việc quản lý các biến thể sản phẩm, đáp ứng đầy đủ các yêu cầu nghiệp vụ và mang lại trải nghiệm người dùng tốt nhất.*
