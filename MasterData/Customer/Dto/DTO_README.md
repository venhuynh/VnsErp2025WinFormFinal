## Tài liệu DTO - MasterData

Tài liệu mô tả các DTO dùng cho binding UI (DevExpress GridControl, DataLayoutControl) và truyền dữ liệu giữa Service ↔ WinForms.

### Nguyên tắc chung
- **Namespace**: `MasterData.Dto`
- **POCO**: Chỉ chứa dữ liệu, không chứa logic nghiệp vụ.
- **DataAnnotations**: Sử dụng đầy đủ để phục vụ caption hiển thị và validate tiếng Việt.
- **Enum/Lookup**: Thuộc tính mã/giá trị (ví dụ `PartnerType`) luôn đi kèm thuộc tính text hiển thị (`PartnerTypeName`).

---

## BusinessPartnerListDto
Dùng cho GridControl (danh sách đối tác).

### Thuộc tính
- **Id** (`Guid`)
  - Display: "ID"
- **PartnerCode** (`string`)
  - Display: "Mã đối tác"
  - Required: "Mã đối tác không được để trống"
  - StringLength(20)
- **PartnerName** (`string`)
  - Display: "Tên đối tác"
  - Required: "Tên đối tác không được để trống"
  - StringLength(200)
- **PartnerType** (`int`)
  - Display: "Loại đối tác"
- **PartnerTypeName** (`string`)
  - Display: "Loại đối tác" (text hiển thị)
- **TaxCode** (`string`)
  - Display: "Mã số thuế"
  - StringLength(20)
- **Phone** (`string`)
  - Display: "Số điện thoại"
  - Phone
  - StringLength(20)
- **Email** (`string`)
  - Display: "Email"
  - EmailAddress
  - StringLength(100)
- **City** (`string`)
  - Display: "Thành phố"
  - StringLength(100)
- **IsActive** (`bool`)
  - Display: "Trạng thái"
- **CreatedDate** (`DateTime`)
  - Display: "Ngày tạo"
  - DataType(DateTime)

---

## BusinessPartnerDetailDto
Dùng cho DataLayoutControl (chi tiết đối tác).

### Thuộc tính chính
- **Id** (`Guid`) — Display: "ID"
- **PartnerCode** (`string`) — Display: "Mã đối tác", Required, StringLength(20)
- **PartnerName** (`string`) — Display: "Tên đối tác", Required, StringLength(200)
- **PartnerType** (`int`) — Display: "Loại đối tác", Required
- **PartnerTypeName** (`string`) — Display: "Loại đối tác" (text hiển thị)
- **TaxCode** (`string`) — Display: "Mã số thuế", StringLength(20)
- **Phone** (`string`) — Display: "Số điện thoại", Phone, StringLength(20)
- **Email** (`string`) — Display: "Email", EmailAddress, StringLength(100)
- **Website** (`string`) — Display: "Website", Url, StringLength(200)
- **Address** (`string`) — Display: "Địa chỉ", StringLength(500)
- **City** (`string`) — Display: "Thành phố", StringLength(100)
- **Country** (`string`) — Display: "Quốc gia", StringLength(100)
- **ContactPerson** (`string`) — Display: "Người liên hệ", StringLength(100)
- **ContactPosition** (`string`) — Display: "Chức vụ", StringLength(100)
- **BankAccount** (`string`) — Display: "Số tài khoản ngân hàng", StringLength(50)
- **BankName** (`string`) — Display: "Tên ngân hàng", StringLength(200)
- **CreditLimit** (`decimal?`) — Display: "Hạn mức tín dụng", DataType(Currency), Range(0, +∞)
- **PaymentTerm** (`string`) — Display: "Điều khoản thanh toán", StringLength(200)
- **IsActive** (`bool`) — Display: "Trạng thái"
- **CreatedDate** (`DateTime`) — Display: "Ngày tạo", DataType(DateTime)
- **UpdatedDate** (`DateTime?`) — Display: "Ngày cập nhật", DataType(DateTime)

---

## BusinessPartnerContactDto
Dùng cho tab Liên hệ của đối tác.

### Thuộc tính
- **Id** (`Guid`) — Display: "ID"
- **PartnerId** (`Guid`) — Display: "ID đối tác", Required
- **FullName** (`string`) — Display: "Họ và tên", Required, StringLength(100)
- **Position** (`string`) — Display: "Chức vụ", StringLength(100)
- **Phone** (`string`) — Display: "Số điện thoại", Phone, StringLength(20)
- **Email** (`string`) — Display: "Email", EmailAddress, StringLength(100)
- **IsPrimary** (`bool`) — Display: "Liên hệ chính"

---

## BusinessPartnerCategoryDto
Dùng cho quản lý phân loại đối tác.

### Thuộc tính
- **Id** (`Guid`) — Display: "ID"
- **CategoryName** (`string`) — Display: "Tên phân loại", Required, StringLength(100)
- **Description** (`string`) — Display: "Mô tả", StringLength(500)

---

## Ví dụ sử dụng nhanh

### Binding danh sách vào DevExpress GridControl
```csharp
// Giả sử lấy dữ liệu từ Service
IEnumerable<BusinessPartnerListDto> partners = await service.GetPartnersAsync();
gridControl.DataSource = partners.ToList();

// Lưu ý: GridView sẽ dùng [Display(Name)] làm caption cột
gridView.BestFitColumns();
```

### Binding chi tiết vào DataLayoutControl
```csharp
BusinessPartnerDetailDto model = await service.GetPartnerDetailAsync(id);
dataLayoutControl.DataSource = model;
```

### Validate bằng DataAnnotations (ví dụ WinForms)
```csharp
var context = new ValidationContext(model, serviceProvider: null, items: null);
var results = new List<ValidationResult>();
bool isValid = Validator.TryValidateObject(model, context, results, validateAllProperties: true);

if (!isValid)
{
    // results chứa các thông báo lỗi tiếng Việt từ ErrorMessage
    var message = string.Join("\n", results.Select(r => $"- {r.ErrorMessage}"));
    XtraMessageBox.Show(message, "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
}
```

### Gợi ý hiển thị PartnerType với lookup
```csharp
// Ví dụ: repository cung cấp danh sách loại đối tác
lookUpEditPartnerType.Properties.DataSource = partnerTypes; // List<{ Id:int, Name:string }>
lookUpEditPartnerType.Properties.ValueMember = "Id";
lookUpEditPartnerType.Properties.DisplayMember = "Name";

// Bind giá trị vào PartnerType và hiển thị PartnerTypeName
lookUpEditPartnerType.DataBindings.Add("EditValue", bindingSource, nameof(BusinessPartnerDetailDto.PartnerType));
textEditPartnerTypeName.DataBindings.Add("Text", bindingSource, nameof(BusinessPartnerDetailDto.PartnerTypeName));
```

---

## Ghi chú triển khai
- Khi thêm trường mới phục vụ hiển thị, luôn bổ sung `[Display(Name="...")]` với tiếng Việt.
- Trường nhập liệu cần có `[Required]` và giới hạn `[StringLength]` hợp lý để ValidationProvider hiển thị lỗi chính xác.
- Các trường số tiền dùng `[DataType(DataType.Currency)]` để định dạng.
- Ngày giờ dùng `[DataType(DataType.DateTime)]` hoặc `[DataType(DataType.Date)]` tùy ngữ cảnh.


