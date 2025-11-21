# Hướng dẫn sử dụng StockInHistoryQueryDto

## Tổng quan

`StockInHistoryQueryDto` là DTO chuyên nghiệp để truy vấn lịch sử nhập xuất kho với đầy đủ các tiêu chí filter, sorting và pagination.

## Cấu trúc DTO

### StockInHistoryQueryDto - Query Criteria

DTO này chứa các tiêu chí để filter và query dữ liệu:

#### 1. Filter theo thời gian (Bắt buộc)
- `FromDate` (DateTime, Required) - Từ ngày
- `ToDate` (DateTime, Required) - Đến ngày

#### 2. Filter theo kho (Optional)
- `WarehouseId` (Guid?) - ID kho cụ thể
- `WarehouseCode` (string) - Mã kho để search nhanh

#### 3. Filter theo loại và trạng thái (Optional)
- `LoaiNhapKho` (LoaiNhapKhoEnum?) - Loại nhập kho
- `TrangThai` (TrangThaiPhieuNhapEnum?) - Trạng thái đơn
- `TrangThaiList` (TrangThaiPhieuNhapEnum[]) - Danh sách trạng thái (ưu tiên hơn TrangThai)

#### 4. Filter theo đối tác (Optional)
- `SupplierId` (Guid?) - ID nhà cung cấp/khách hàng
- `SupplierCode` (string) - Mã đối tác để search

#### 5. Filter theo đơn hàng (Optional)
- `PurchaseOrderId` (Guid?) - ID đơn mua hàng (PO)
- `PurchaseOrderNumber` (string) - Số PO để search

#### 6. Search text (Optional)
- `SearchText` (string) - Từ khóa tìm kiếm (tìm trong số phiếu, ghi chú)
- `StockInNumber` (string) - Số phiếu cụ thể

#### 7. Sorting
- `OrderBy` (string, default: "StockInDate") - Cột sắp xếp
  - Các giá trị hợp lệ: "StockInDate", "StockInNumber", "TotalAmount", "TotalQuantity", "CreatedDate"
- `OrderDirection` (string, default: "DESC") - Hướng sắp xếp: "ASC" hoặc "DESC"

#### 8. Pagination (Optional)
- `PageIndex` (int?, default: null) - Số trang (bắt đầu từ 1)
- `PageSize` (int?, default: null) - Số bản ghi mỗi trang (1-1000)

### StockInHistoryResultDto - Query Result

DTO này chứa kết quả truy vấn:
- `Items` (List<StockInListDto>) - Danh sách phiếu nhập kho
- `TotalCount` (int) - Tổng số bản ghi (không phân trang)
- `PageIndex` (int?) - Số trang hiện tại
- `PageSize` (int?) - Số bản ghi mỗi trang
- `TotalPages` (int?) - Tổng số trang (tính toán tự động)
- `HasNextPage` (bool) - Có trang tiếp theo không
- `HasPreviousPage` (bool) - Có trang trước đó không

## Cách sử dụng

### Ví dụ 1: Query đơn giản - Lấy tất cả phiếu nhập trong khoảng thời gian

```csharp
var query = new StockInHistoryQueryDto
{
    FromDate = new DateTime(2025, 1, 1),
    ToDate = new DateTime(2025, 1, 31)
};

var result = await _stockInBll.QueryHistoryAsync(query);
```

### Ví dụ 2: Query với filter theo kho và loại nhập

```csharp
var query = new StockInHistoryQueryDto
{
    FromDate = new DateTime(2025, 1, 1),
    ToDate = new DateTime(2025, 1, 31),
    WarehouseId = warehouseId,
    LoaiNhapKho = LoaiNhapKhoEnum.ThuongMai
};

var result = await _stockInBll.QueryHistoryAsync(query);
```

### Ví dụ 3: Query với filter theo trạng thái và search text

```csharp
var query = new StockInHistoryQueryDto
{
    FromDate = new DateTime(2025, 1, 1),
    ToDate = new DateTime(2025, 1, 31),
    TrangThai = TrangThaiPhieuNhapEnum.DaNhapKho,
    SearchText = "PNK-0125"
};

var result = await _stockInBll.QueryHistoryAsync(query);
```

### Ví dụ 4: Query với pagination

```csharp
var query = new StockInHistoryQueryDto
{
    FromDate = new DateTime(2025, 1, 1),
    ToDate = new DateTime(2025, 1, 31),
    PageIndex = 1,
    PageSize = 50,
    OrderBy = "StockInDate",
    OrderDirection = "DESC"
};

var result = await _stockInBll.QueryHistoryAsync(query);

// Kiểm tra pagination
if (result.HasNextPage)
{
    // Có trang tiếp theo
}

if (result.HasPreviousPage)
{
    // Có trang trước
}
```

### Ví dụ 5: Query với nhiều filter kết hợp

```csharp
var query = new StockInHistoryQueryDto
{
    FromDate = new DateTime(2025, 1, 1),
    ToDate = new DateTime(2025, 1, 31),
    WarehouseId = warehouseId,
    SupplierId = supplierId,
    LoaiNhapKho = LoaiNhapKhoEnum.ThuongMai,
    TrangThaiList = new[] 
    { 
        TrangThaiPhieuNhapEnum.DaDuyet, 
        TrangThaiPhieuNhapEnum.DaNhapKho 
    },
    SearchText = "PNK",
    OrderBy = "TotalAmount",
    OrderDirection = "DESC",
    PageIndex = 1,
    PageSize = 100
};

// Validate trước khi query
if (query.Validate(out var errorMessage))
{
    var result = await _stockInBll.QueryHistoryAsync(query);
}
else
{
    MsgBox.ShowWarning(errorMessage);
}
```

## Validation

DTO có method `Validate()` để kiểm tra tính hợp lệ:

```csharp
if (query.Validate(out var errorMessage))
{
    // Query hợp lệ, có thể thực hiện query
    var result = await _stockInBll.QueryHistoryAsync(query);
}
else
{
    // Query không hợp lệ, hiển thị lỗi
    MsgBox.ShowWarning(errorMessage);
}
```

## Helper Properties

DTO cung cấp các helper properties để hỗ trợ pagination:

```csharp
// Kiểm tra có sử dụng pagination không
if (query.UsePagination)
{
    // Sử dụng pagination
    var skip = query.SkipCount;  // Số bản ghi bỏ qua
    var take = query.TakeCount;  // Số bản ghi lấy
}
```

## Đề xuất Implementation

### 1. Thêm method vào IStockInRepository

```csharp
/// <summary>
/// Query lịch sử nhập xuất kho với filter
/// </summary>
/// <param name="query">Query criteria</param>
/// <returns>Danh sách StockInOutMaster entities</returns>
List<StockInOutMaster> QueryHistory(StockInHistoryQueryDto query);

/// <summary>
/// Đếm số lượng bản ghi theo query (không phân trang)
/// </summary>
/// <param name="query">Query criteria</param>
/// <returns>Tổng số bản ghi</returns>
int CountHistory(StockInHistoryQueryDto query);
```

### 2. Thêm method vào StockInBll

```csharp
/// <summary>
/// Query lịch sử nhập xuất kho với filter
/// </summary>
/// <param name="query">Query criteria</param>
/// <returns>StockInHistoryResultDto chứa danh sách và pagination info</returns>
public async Task<StockInHistoryResultDto> QueryHistoryAsync(StockInHistoryQueryDto query)
{
    // Validate query
    if (!query.Validate(out var errorMessage))
    {
        throw new ArgumentException(errorMessage);
    }

    // Query từ repository
    var entities = await Task.Run(() => GetDataAccess().QueryHistory(query));
    
    // Map sang DTO
    var items = entities.Select(e => MapEntityToListDto(e)).ToList();
    
    // Đếm tổng số bản ghi (nếu có pagination)
    int totalCount = 0;
    if (query.UsePagination)
    {
        totalCount = GetDataAccess().CountHistory(query);
    }
    else
    {
        totalCount = items.Count;
    }

    // Tạo result DTO
    return new StockInHistoryResultDto
    {
        Items = items,
        TotalCount = totalCount,
        PageIndex = query.PageIndex,
        PageSize = query.PageSize
    };
}
```

## Best Practices

1. **Luôn validate query trước khi thực hiện**: Sử dụng `Validate()` method
2. **Sử dụng pagination cho dataset lớn**: Đặt `PageSize` hợp lý (50-100)
3. **Sử dụng OrderBy và OrderDirection**: Đảm bảo kết quả được sắp xếp đúng
4. **Filter theo khoảng thời gian hợp lý**: Tránh query quá rộng (ví dụ: > 1 năm)
5. **Sử dụng SearchText cho tìm kiếm nhanh**: Thay vì filter nhiều field riêng lẻ
6. **Sử dụng TrangThaiList thay vì TrangThai**: Khi cần filter nhiều trạng thái

## Performance Tips

1. **Index database**: Đảm bảo có index trên `StockInOutDate`, `WarehouseId`, `PartnerSiteId`
2. **Limit date range**: Không query quá rộng (khuyến nghị: < 3 tháng)
3. **Use pagination**: Luôn sử dụng pagination cho dataset lớn
4. **Select only needed fields**: Repository chỉ select các field cần thiết
5. **Cache lookup data**: Cache danh sách kho, đối tác để tránh query nhiều lần

