# ProductService View DTOs - Master-Detail Pattern

## Tổng quan
Bộ DTO được thiết kế theo mô hình Master-Detail để thể hiện view của các entity ProductService và ProductVariant từ VnsErp2025.designer.cs.

## Cấu trúc DTO

### 1. ProductServiceViewDto.cs
**Mục đích**: DTO chính cho ProductService (Master)
**Sử dụng**: Hiển thị thông tin sản phẩm chính với danh sách biến thể

**Các thuộc tính chính**:
- Thông tin cơ bản: Id, Code, Name, Description
- Thông tin danh mục: CategoryId, CategoryName, CategoryCode
- Trạng thái: IsService, IsActive
- Hình ảnh: ThumbnailImage
- Thống kê: VariantCount
- Danh sách: Variants (List<ProductVariantViewDto>)

### 2. ProductVariantViewDto.cs
**Mục đích**: DTO cho ProductVariant (Detail) và các DTO liên quan
**Sử dụng**: Hiển thị thông tin biến thể sản phẩm

**Các DTO con**:
- `ProductVariantViewDto`: DTO chính cho biến thể
- `VariantAttributeViewDto`: DTO cho thuộc tính biến thể
- `ProductImageViewDto`: DTO cho hình ảnh sản phẩm

### 3. ProductServiceMasterDetailViewDto.cs
**Mục đích**: DTO tổng hợp Master-Detail
**Sử dụng**: Hiển thị đầy đủ thông tin sản phẩm và tất cả biến thể

**Đặc điểm**:
- Kết hợp thông tin Master (ProductService) và Detail (ProductVariant)
- Bao gồm thống kê tổng hợp
- Danh sách đầy đủ các biến thể với thuộc tính và hình ảnh

### 4. ProductServiceSummaryViewDto.cs
**Mục đích**: DTO tóm tắt và tìm kiếm
**Sử dụng**: Danh sách, dropdown, combo, tìm kiếm

**Các DTO**:
- `ProductServiceSummaryViewDto`: Tóm tắt sản phẩm
- `ProductVariantSummaryViewDto`: Tóm tắt biến thể
- `ProductServiceSearchViewDto`: Điều kiện tìm kiếm sản phẩm
- `ProductVariantSearchViewDto`: Điều kiện tìm kiếm biến thể

## Mô hình Master-Detail

```
ProductService (Master)
├── Thông tin cơ bản sản phẩm
├── Thông tin danh mục
├── Hình ảnh sản phẩm
└── ProductVariant[] (Detail)
    ├── Thông tin biến thể
    ├── Thuộc tính biến thể
    └── Hình ảnh biến thể
```

## Các trường hợp sử dụng

### 1. Hiển thị danh sách sản phẩm
- Sử dụng: `ProductServiceSummaryViewDto`
- Hiển thị: Mã, tên, danh mục, trạng thái, số biến thể

### 2. Chi tiết sản phẩm với biến thể
- Sử dụng: `ProductServiceMasterDetailViewDto`
- Hiển thị: Thông tin đầy đủ sản phẩm + tất cả biến thể

### 3. Quản lý biến thể
- Sử dụng: `ProductVariantViewDto`
- Hiển thị: Thông tin chi tiết biến thể với thuộc tính và hình ảnh

### 4. Tìm kiếm và lọc
- Sử dụng: `ProductServiceSearchViewDto`, `ProductVariantSearchViewDto`
- Chức năng: Lọc theo điều kiện khác nhau

## Validation Attributes

Tất cả DTO đều sử dụng:
- `[Display]`: Hiển thị tên tiếng Việt
- `[Required]`: Bắt buộc nhập
- `[StringLength]`: Giới hạn độ dài
- `[ErrorMessage]`: Thông báo lỗi tiếng Việt

## Lưu ý

1. **Chỉ là View DTO**: Không chứa logic nghiệp vụ
2. **Tối ưu hiển thị**: Chỉ chứa các trường cần thiết cho UI
3. **Hỗ trợ đa ngôn ngữ**: Sử dụng Display attributes tiếng Việt
4. **Mở rộng dễ dàng**: Có thể thêm thuộc tính mới mà không ảnh hưởng entity
5. **Tách biệt rõ ràng**: Master-Detail pattern rõ ràng, dễ hiểu

