# FrmBusinessPartnerDetail - Tài Liệu Kỹ Thuật

## 1. Mục Đích Của Class

`FrmBusinessPartnerDetail` là một Windows Forms form (kế thừa từ `XtraForm` của DevExpress) được thiết kế để quản lý thông tin chi tiết của đối tác (Business Partner).

### Chức Năng Chính:
- **Thêm mới**: Tạo đối tác mới với đầy đủ thông tin (mã, tên, loại, liên hệ, địa chỉ, logo)
- **Chỉnh sửa**: Cập nhật thông tin của đối tác đã tồn tại
- **Validation**: Kiểm tra tính hợp lệ của dữ liệu đầu vào (mã bắt buộc, không trùng lặp, loại đối tác)
- **Quản lý logo**: Upload và hiển thị logo đối tác (hỗ trợ JPG, PNG, GIF, tối đa 10MB)
- **Xử lý lỗi**: Hiển thị thông báo lỗi và validation errors với DXErrorProvider
- **Event-driven**: Trigger event `PartnerSaved` để form cha có thể cập nhật datasource

### Đặc Điểm:
- Form modal (dialog) - chặn tương tác với form cha
- Hỗ trợ 2 chế độ: **Thêm mới** (`Guid.Empty`) và **Chỉnh sửa** (có `businessPartnerId`)
- Validation real-time với `DXErrorProvider`
- Upload logo tự động khi thay đổi (chỉ trong edit mode)
- Logo được lưu trên NAS (file gốc) và thumbnail trong database
- Hỗ trợ SuperToolTip cho tất cả controls để cải thiện UX

---

## 2. Vai Trò Trong Kiến Trúc

### **Vị Trí: UI Layer (Presentation Layer)**

Form này nằm ở tầng **UI (User Interface)** trong kiến trúc 3-layer của ứng dụng:

```
┌─────────────────────────────────────────┐
│  UI Layer (Presentation)                │
│  ┌───────────────────────────────────┐ │
│  │ FrmBusinessPartnerDetail          │ │ ← Class này
│  │ - XtraForm (DevExpress)           │ │
│  │ - Data Entry Form                 │ │
│  │ - Validation Logic                │ │
│  │ - Logo Upload Handler             │ │
│  └───────────────────────────────────┘ │
└─────────────────────────────────────────┘
              │
              │ Gọi methods
              ▼
┌─────────────────────────────────────────┐
│  BLL Layer (Business Logic)             │
│  ┌───────────────────────────────────┐ │
│  │ BusinessPartnerBll                 │ │
│  │ - GetByIdAsync()                   │ │
│  │ - GetById()                        │ │
│  │ - IsCodeExists()                   │ │
│  │ - SaveOrUpdate()                   │ │
│  │ - UploadLogoFromBytesAsync()       │ │
│  │ - GetCategoryDictAsync()           │ │
│  └───────────────────────────────────┘ │
└─────────────────────────────────────────┘
              │
              │ Sử dụng
              ▼
┌─────────────────────────────────────────┐
│  DAL Layer (Data Access)                │
│  ┌───────────────────────────────────┐ │
│  │ BusinessPartnerRepository         │ │
│  │ - Database Operations              │ │
│  │ - Image Storage (NAS)              │ │
│  └───────────────────────────────────┘ │
└─────────────────────────────────────────┘
```

### **Dependencies:**
- **BLL Layer**: `BusinessPartnerBll` - Xử lý business logic và upload logo
- **DTO Layer**: 
  - `BusinessPartnerDetailDto` - DTO để lưu dữ liệu
  - `BusinessPartnerListDto` - DTO để trả về cho form cha
- **Domain Layer**: `BusinessPartner` - Entity từ database
- **Common Utilities**: 
  - `RequiredFieldHelper` - Đánh dấu các trường bắt buộc
  - `SuperToolTipHelper` - Tooltip hỗ trợ
  - `MsgBox` - Hiển thị thông báo
  - `ExecuteWithWaitingFormAsync` - Hiển thị splash screen
- **UI Framework**: DevExpress WinForms controls
  - `DXErrorProvider` - Validation error display
  - `DataLayoutControl` - Layout management
  - `ComboBoxEdit` - Dropdown cho loại đối tác
  - `PictureEdit` - Hiển thị và upload logo

### **Không Trực Tiếp Truy Cập:**
- ❌ Database (không gọi DAL trực tiếp)
- ❌ Repository (chỉ làm việc qua BLL)

---

## 3. Giải Thích Các Method Chính

### 3.1. Constructor & Initialization

#### `FrmBusinessPartnerDetail(Guid businessPartnerId)`
```csharp
public FrmBusinessPartnerDetail(Guid businessPartnerId)
```
**Mục đích**: Khởi tạo form với chế độ thêm mới hoặc chỉnh sửa.

**Tham số**:
- `businessPartnerId`: 
  - `Guid.Empty` → Chế độ **Thêm mới**
  - Có giá trị → Chế độ **Chỉnh sửa**

**Luồng xử lý**:
1. Lưu `businessPartnerId` vào `_businessPartnerId`
2. Gọi `InitializeComponent()` (Designer-generated)
3. Đăng ký event `Shown` để load dữ liệu khi form hiển thị

**Property**: `IsEditMode` (computed property)
```csharp
private bool IsEditMode => _businessPartnerId != Guid.Empty;
```

---

#### `FrmBusinessPartnerDetail_Shown()`
```csharp
private void FrmBusinessPartnerDetail_Shown(object sender, EventArgs e)
```
**Mục đích**: Khởi tạo form và load dữ liệu nếu cần (được gọi khi form được hiển thị).

**Luồng xử lý**:
1. **Đánh dấu trường bắt buộc**: 
   - Sử dụng `RequiredFieldHelper.MarkRequiredFields()` với `DataAnnotations` từ `BusinessPartnerListDto`
2. **Thiết lập SuperToolTip**: `SetupSuperToolTips()` - Cải thiện UX với tooltips cho tất cả controls
3. **Cấu hình ComboBox loại đối tác**: `SetupPartnerTypeComboBox()` - Setup 3 lựa chọn chuẩn
4. **Load dữ liệu nếu edit mode**: `LoadDetailAsync(_businessPartnerId)` - Chỉ gọi khi `IsEditMode == true`

**Lưu ý**: 
- `RequiredFieldHelper` tự động đọc `[Required]` attributes từ DTO
- Load dữ liệu được thực hiện async để không block UI thread

---

### 3.2. UI Setup

#### `SetupSuperToolTips()`
```csharp
private void SetupSuperToolTips()
```
**Mục đích**: Thiết lập SuperToolTip cho tất cả các controls trong form để cải thiện UX.

**Tooltips được thiết lập**:
- 🔖 Mã đối tác (bắt buộc)
- 🏢 Tên đối tác (bắt buộc)
- 📂 Loại đối tác
- 📋 Mã số thuế
- 📞 Số điện thoại
- 📧 Email
- 🌐 Website
- 📍 Địa chỉ
- 🏙️ Thành phố
- 🌍 Quốc gia
- ⚡ Trạng thái hoạt động
- 🖼️ Logo đối tác
- 💾 Lưu
- ❌ Đóng

**Lưu ý**: Có try-catch để không chặn form nếu setup tooltip lỗi

---

#### `SetupPartnerTypeComboBox()`
```csharp
private void SetupPartnerTypeComboBox()
```
**Mục đích**: Cấu hình ComboBoxEdit cho loại đối tác với 3 lựa chọn chuẩn.

**Các lựa chọn**:
1. "Khách hàng" → `PartnerType = 1`
2. "Nhà cung cấp" → `PartnerType = 2`
3. "Khách hàng & Nhà cung cấp" → `PartnerType = 3`

**Cấu hình**:
- `TextEditStyle = DisableTextEditor` - Chỉ cho phép chọn từ danh sách, không nhập text tự do

---

### 3.3. Data Loading

#### `LoadDetailAsync(Guid id)`
```csharp
private async Task LoadDetailAsync(Guid id)
```
**Mục đích**: Nạp dữ liệu chi tiết đối tác theo Id vào các controls (asynchronous).

**Luồng xử lý**:
1. **Lấy entity từ BLL**:
   ```csharp
   var entity = await _bll.GetByIdAsync(id);
   ```
2. **Validation**: Kiểm tra entity có tồn tại không
   - Nếu null → Return (không load gì)
3. **Map dữ liệu vào controls**:
   - `LoadBasicInformation(entity)` - Load thông tin cơ bản
   - `LoadPartnerType(entity)` - Load loại đối tác
   - `LoadLogo(entity)` - Load logo thumbnail

**Lưu ý**: 
- Chỉ gọi khi `IsEditMode == true`
- Sử dụng async/await để không block UI thread

---

#### `LoadBasicInformation(BusinessPartner entity)`
```csharp
private void LoadBasicInformation(BusinessPartner entity)
```
**Mục đích**: Load thông tin cơ bản vào các controls.

**Mapping**:
- `PartnerCodeTextEdit.Text` ← `entity.PartnerCode`
- `PartnerNameTextEdit.Text` ← `entity.PartnerName`
- `TaxCodeTextEdit.Text` ← `entity.TaxCode`
- `PhoneTextEdit.Text` ← `entity.Phone`
- `EmailTextEdit.Text` ← `entity.Email`
- `WebsiteTextEdit.Text` ← `entity.Website`
- `AddressTextEdit.Text` ← `entity.Address`
- `CityTextEdit.Text` ← `entity.City`
- `CountryTextEdit.Text` ← `entity.Country`
- `IsActiveToggleSwitch.IsOn` ← `entity.IsActive`

---

#### `LoadPartnerType(BusinessPartner entity)`
```csharp
private void LoadPartnerType(BusinessPartner entity)
```
**Mục đích**: Load loại đối tác vào ComboBox.

**Mapping**:
- `PartnerType = 1` → `SelectedIndex = 0` (Khách hàng)
- `PartnerType = 2` → `SelectedIndex = 1` (Nhà cung cấp)
- `PartnerType = 3` → `SelectedIndex = 2` (Khách hàng & Nhà cung cấp)
- Default → `SelectedIndex = -1` (Chưa chọn)

---

#### `LoadLogo(BusinessPartner entity)`
```csharp
private void LoadLogo(BusinessPartner entity)
```
**Mục đích**: Load logo thumbnail vào PictureEdit.

**Luồng xử lý**:
1. **Đánh dấu đang load logo**: `_isLoadingLogo = true` - Tránh trigger event `ImageChanged` khi load
2. **Chuyển đổi Binary sang byte array**: `entity.LogoThumbnailData.ToArray()`
3. **Load ảnh từ byte array**:
   ```csharp
   using (var ms = new MemoryStream(thumbnailBytes))
   {
       loadedImage = Image.FromStream(ms);
   }
   ```
4. **Clone Image**: `CloneImage(loadedImage)` - Tạo bản copy độc lập để tránh lỗi GDI+ khi stream bị dispose
5. **Set vào PictureEdit**: `LogoThumbnailDataPictureEdit.Image = clonedImage`
6. **Reset flag**: `_isLoadingLogo = false`

**Lưu ý quan trọng**: 
- Phải clone Image để tránh lỗi "A generic error occurred in GDI+" khi stream đã bị dispose
- Flag `_isLoadingLogo` ngăn trigger event `ImageChanged` khi load từ database

---

#### `ReloadLogoAsync()`
```csharp
private async Task ReloadLogoAsync()
```
**Mục đích**: Reload logo từ database (sử dụng khi upload thất bại để rollback).

**Luồng xử lý**:
1. Kiểm tra `IsEditMode` và control có tồn tại không
2. Đánh dấu `_isLoadingLogo = true`
3. Lấy lại entity từ database: `await _bll.GetByIdAsync(_businessPartnerId)`
4. Load logo tương tự `LoadLogo()` hoặc xóa ảnh nếu không có logo

**Sử dụng**: Được gọi trong catch block của `LogoThumbnailDataPictureEdit_ImageChanged()` để rollback khi upload thất bại

---

### 3.4. Data Saving

#### `SaveBusinessPartnerAsync()`
```csharp
private async Task SaveBusinessPartnerAsync()
```
**Mục đích**: Lưu dữ liệu đối tác và upload logo nếu có.

**Luồng xử lý**:
1. **Thu thập dữ liệu từ form**: `BuildDetailDtoFromForm()` - Tạo DTO từ controls
2. **Convert DTO → Entity**:
   ```csharp
   var existing = IsEditMode ? _bll.GetById(detailDto.Id) : null;
   var entity = detailDto.ToEntity(existing);
   ```
   - Nếu edit mode: Lấy entity hiện tại để merge
   - Nếu new mode: `existing = null` → Tạo entity mới
3. **Lưu entity qua BLL**: `_bll.SaveOrUpdate(entity)`
   - Repository sẽ tự động set `Id` nếu là tạo mới
4. **Upload logo nếu có**:
   ```csharp
   if (LogoThumbnailDataPictureEdit?.Image != null && entity.Id != Guid.Empty)
   {
       await UploadLogoIfValidAsync(entity.Id);
   }
   ```
   - Chỉ upload khi đã có `Id` (sau khi lưu thành công)
   - Logo sẽ được upload sau khi có Id (đặc biệt quan trọng cho new mode)
5. **Lấy lại entity đã lưu và trigger event**:
   ```csharp
   var savedEntity = await _bll.GetByIdAsync(entity.Id);
   var categoryDict = await _bll.GetCategoryDictAsync();
   var listDto = savedEntity.ToListDto(categoryDict);
   PartnerSaved?.Invoke(listDto);
   ```
   - Trigger event `PartnerSaved` để form cha có thể cập nhật datasource

**Lưu ý**: 
- Logo được upload sau khi lưu entity để đảm bảo có `Id`
- Event `PartnerSaved` giúp form cha refresh grid mà không cần reload toàn bộ

---

#### `BuildDetailDtoFromForm()`
```csharp
private BusinessPartnerDetailDto BuildDetailDtoFromForm()
```
**Mục đích**: Thu thập dữ liệu từ Form thành `BusinessPartnerDetailDto`.

**Mapping**:
- `Id = _businessPartnerId` (Guid.Empty nếu thêm mới)
- `PartnerCode = PartnerCodeTextEdit?.EditValue?.ToString()`
- `PartnerName = PartnerNameTextEdit?.EditValue?.ToString()`
- `PartnerType = GetPartnerTypeFromComboBox()` - Convert SelectedIndex → PartnerType
- `TaxCode = TaxCodeTextEdit?.EditValue?.ToString()`
- `Phone = PhoneTextEdit?.EditValue?.ToString()`
- `Email = EmailTextEdit?.EditValue?.ToString()`
- `Website = WebsiteTextEdit?.EditValue?.ToString()`
- `Address = AddressTextEdit?.EditValue?.ToString()`
- `City = CityTextEdit?.EditValue?.ToString()`
- `Country = CountryTextEdit?.EditValue?.ToString()`
- `IsActive = (IsActiveToggleSwitch?.EditValue as bool?) ?? true`
- `CreatedDate = DateTime.Now`
- `UpdatedDate = DateTime.Now`

---

#### `GetPartnerTypeFromComboBox()`
```csharp
private int GetPartnerTypeFromComboBox()
```
**Mục đích**: Lấy giá trị PartnerType từ ComboBox selection.

**Mapping**:
- `SelectedIndex = 0` → `PartnerType = 1` (Khách hàng)
- `SelectedIndex = 1` → `PartnerType = 2` (Nhà cung cấp)
- `SelectedIndex = 2` → `PartnerType = 3` (Khách hàng & Nhà cung cấp)
- Default → `PartnerType = 0` (Chưa chọn)

---

### 3.5. Logo Upload

#### `LogoThumbnailDataPictureEdit_ImageChanged()`
```csharp
private async void LogoThumbnailDataPictureEdit_ImageChanged(object sender, EventArgs e)
```
**Mục đích**: Xử lý sự kiện ImageChanged của PictureEdit - Upload logo đối tác tự động.

**Luồng xử lý**:
1. **Bỏ qua nếu đang load logo**: 
   ```csharp
   if (_isLoadingLogo) return;
   ```
   - Tránh trigger khi load logo từ database
2. **Kiểm tra sender**: Chỉ xử lý khi sender là `PictureEdit`
3. **Kiểm tra edit mode**: 
   ```csharp
   if (!IsEditMode) return;
   ```
   - Chỉ upload khi đang chỉnh sửa (đã có Id)
   - Nếu thêm mới, logo sẽ được upload sau khi lưu thành công
4. **Upload logo**: `HandleLogoUploadAsync(pictureEdit)` với waiting form
5. **Xử lý lỗi**: Nếu có lỗi, reload logo về trạng thái cũ

**Lưu ý**: 
- Upload tự động khi user thay đổi logo (chỉ trong edit mode)
- Có rollback mechanism khi upload thất bại

---

#### `HandleLogoUploadAsync(PictureEdit pictureEdit)`
```csharp
private async Task HandleLogoUploadAsync(PictureEdit pictureEdit)
```
**Mục đích**: Xử lý upload logo từ PictureEdit.

**Luồng xử lý**:
1. **Chuyển đổi Image sang byte array**: `ImageToByteArray(pictureEdit.Image)`
2. **Kiểm tra kích thước**: Tối đa 10MB (`MaxLogoSizeInBytes`)
   - Nếu vượt quá → Warning và reload logo cũ
3. **Kiểm tra format**: `IsValidImageFormat(imageBytes)` - JPG, PNG, GIF
   - Nếu không hợp lệ → Warning và reload logo cũ
4. **Upload qua BLL**: `await _bll.UploadLogoFromBytesAsync(_businessPartnerId, imageBytes, ThumbnailMaxDimension)`
   - File gốc lưu trên NAS
   - Thumbnail (300px) lưu trong database
5. **Thông báo thành công**: `MsgBox.ShowSuccess()`
6. **Reload logo**: `ReloadLogoAsync()` - Hiển thị thumbnail mới từ database

---

#### `UploadLogoIfValidAsync(Guid partnerId)`
```csharp
private async Task UploadLogoIfValidAsync(Guid partnerId)
```
**Mục đích**: Upload logo nếu hợp lệ (kiểm tra kích thước và format) - Sử dụng khi lưu form.

**Luồng xử lý**:
1. **Chuyển đổi Image sang byte array**: `ImageToByteArray(LogoThumbnailDataPictureEdit.Image)`
2. **Kiểm tra kích thước**: Tối đa 10MB
3. **Kiểm tra format**: JPG, PNG, GIF
4. **Upload qua BLL**: `await _bll.UploadLogoFromBytesAsync(partnerId, imageBytes, ThumbnailMaxDimension)`

**Khác biệt với `HandleLogoUploadAsync()`**:
- Không reload logo sau khi upload (vì form sẽ đóng)
- Không hiển thị thông báo thành công (vì đã có thông báo lưu thành công)

---

### 3.6. Image Processing Helpers

#### `ImageToByteArray(Image image)`
```csharp
private byte[] ImageToByteArray(Image image)
```
**Mục đích**: Chuyển đổi Image sang byte array (JPEG format để giảm kích thước).

**Luồng xử lý**:
1. **Clone Image**: `CloneImage(image)` - Tránh lỗi GDI+ khi Image đang bị lock
2. **Save với format JPEG**:
   ```csharp
   using (var ms = new MemoryStream())
   {
       clonedImage.Save(ms, ImageFormat.Jpeg);
       return ms.ToArray();
   }
   ```
3. **Fallback**: Nếu clone thất bại, thử save trực tiếp (có thể fail nếu Image bị lock)

**Lưu ý**: 
- Format JPEG để giảm kích thước file
- Clone Image để tránh lỗi khi Image đang được sử dụng bởi control khác

---

#### `CloneImage(Image image)`
```csharp
private Image CloneImage(Image image)
```
**Mục đích**: Clone Image để tạo bản copy độc lập, tránh lỗi GDI+ khi Image bị lock.

**Luồng xử lý**:
1. Tạo `Bitmap` mới với kích thước của Image gốc
2. Vẽ Image gốc lên Bitmap mới:
   ```csharp
   using (var graphics = Graphics.FromImage(bitmap))
   {
       graphics.DrawImage(image, 0, 0, image.Width, image.Height);
   }
   ```
3. Return Bitmap mới

**Lưu ý**: 
- Bitmap mới độc lập với Image gốc
- Có thể dispose Image gốc mà không ảnh hưởng Bitmap mới

---

#### `IsValidImageFormat(byte[] imageBytes)`
```csharp
private bool IsValidImageFormat(byte[] imageBytes)
```
**Mục đích**: Kiểm tra định dạng hình ảnh có hợp lệ không (JPG, PNG, GIF) bằng cách kiểm tra magic bytes.

**Magic bytes được kiểm tra**:
- **JPEG**: `FF D8 FF` (3 bytes đầu)
- **PNG**: `89 50 4E 47` (4 bytes đầu)
- **GIF**: `47 49 46 38` (4 bytes đầu - "GIF8")

**Lưu ý**: 
- Kiểm tra magic bytes an toàn hơn kiểm tra extension
- Hỗ trợ JPG, PNG, GIF

---

### 3.7. Validation

#### `ValidateInput()`
```csharp
private bool ValidateInput()
```
**Mục đích**: Validate input theo thứ tự, đặt lỗi và focus control không hợp lệ đầu tiên.

**Luồng xử lý**:
1. **Clear errors cũ**: `dxErrorProvider1.ClearErrors()`
2. **Validate mã đối tác**: `ValidatePartnerCode()`
   - Bắt buộc
   - Không trùng lặp (exclude bản ghi hiện tại nếu edit mode)
3. **Validate tên đối tác**: `ValidatePartnerName()`
   - Bắt buộc
4. **Validate loại đối tác**: `ValidatePartnerType()`
   - Khuyến nghị bắt buộc chọn

**Return**: 
- `true` → Dữ liệu hợp lệ
- `false` → Có lỗi, hiển thị error provider và focus vào control lỗi đầu tiên

---

#### `ValidatePartnerCode()`
```csharp
private bool ValidatePartnerCode()
```
**Mục đích**: Validate mã đối tác (bắt buộc và không trùng lặp).

**Validation rules**:
1. **Không được để trống**:
   ```csharp
   if (string.IsNullOrWhiteSpace(PartnerCodeTextEdit?.Text))
   {
       dxErrorProvider1.SetError(PartnerCodeTextEdit, "Mã đối tác không được để trống", ErrorType.Critical);
       PartnerCodeTextEdit?.Focus();
       return false;
   }
   ```
2. **Không trùng lặp**:
   - **Edit mode**: Chỉ kiểm tra trùng khi mã đã thay đổi
     ```csharp
     var existingPartner = _bll.GetById(_businessPartnerId);
     if (existingPartner != null && existingPartner.PartnerCode != partnerCode)
     {
         if (_bll.IsCodeExists(partnerCode))
         {
             // Error: "Mã đối tác đã tồn tại trong hệ thống"
         }
     }
     ```
   - **New mode**: Luôn kiểm tra trùng
     ```csharp
     if (_bll.IsCodeExists(partnerCode))
     {
         // Error: "Mã đối tác đã tồn tại trong hệ thống"
     }
     ```

---

#### `ValidatePartnerName()`
```csharp
private bool ValidatePartnerName()
```
**Mục đích**: Validate tên đối tác (bắt buộc).

**Validation rules**:
- Không được để trống
- Hiển thị error và focus vào control nếu lỗi

---

#### `ValidatePartnerType()`
```csharp
private bool ValidatePartnerType()
```
**Mục đích**: Validate loại đối tác (khuyến nghị bắt buộc chọn).

**Validation rules**:
- Phải chọn loại đối tác (`SelectedIndex >= 0`)
- Sử dụng `ErrorType.Warning` (không phải Critical) vì chỉ là khuyến nghị

---

### 3.8. Event Handlers

#### `SaveBarButtonItem_ItemClick()`
```csharp
private async void SaveBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
```
**Mục đích**: Xử lý sự kiện click button "Lưu".

**Luồng xử lý**:
1. **Validate input**: `ValidateInput()`
   - Nếu không hợp lệ → Return (không lưu)
2. **Lưu dữ liệu với waiting form**: 
   ```csharp
   await ExecuteWithWaitingFormAsync(async () =>
   {
       await SaveBusinessPartnerAsync();
   });
   ```
3. **Thông báo thành công**: `MsgBox.ShowSuccess()`
4. **Đóng form**: 
   - `DialogResult = DialogResult.OK`
   - `Close()`

**Lưu ý**: 
- Sử dụng waiting form để hiển thị splash screen khi lưu
- Form cha sẽ nhận `DialogResult.OK` và có thể reload dữ liệu

---

#### `CloseBarButtonItem_ItemClick()`
```csharp
private void CloseBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
```
**Mục đích**: Xử lý sự kiện click button "Đóng".

**Luồng xử lý**:
1. `Close()` form

**Lưu ý**: Form cha sẽ nhận `DialogResult.Cancel` (mặc định) và không reload dữ liệu

---

### 3.9. Helper Methods

#### `ExecuteWithWaitingFormAsync(Func<Task> operation)`
```csharp
private async Task ExecuteWithWaitingFormAsync(Func<Task> operation)
```
**Mục đích**: Thực thi async operation với waiting form (hiển thị splash screen).

**Luồng xử lý**:
1. **Hiển thị waiting form**: `SplashScreenManager.ShowForm(typeof(WaitForm1))`
2. **Thực hiện operation**: `await operation()`
3. **Đóng waiting form**: `SplashScreenManager.CloseForm()` (trong finally block)

**Lưu ý**: 
- Đảm bảo waiting form luôn được đóng kể cả khi có exception
- Cải thiện UX khi thực hiện operations mất thời gian

---

#### `ShowError(Exception ex, string action)`
```csharp
private void ShowError(Exception ex, string action)
```
**Mục đích**: Hiển thị lỗi qua XtraMessageBox với thông báo tiếng Việt.

**Sử dụng**: `MsgBox.ShowException(ex, $"Lỗi {action}")`

---

## 4. Luồng Xử Lý Dữ Liệu

### 4.1. Luồng Thêm Mới

```
User clicks "Mới" button in parent form
         │
         ▼
new FrmBusinessPartnerDetail(Guid.Empty)
         │
         ├─> Constructor
         │   ├─> InitializeComponent()
         │   ├─> _businessPartnerId = Guid.Empty
         │   └─> Shown += FrmBusinessPartnerDetail_Shown
         │
         ├─> Form.Shown event
         │   │
         │   ├─> FrmBusinessPartnerDetail_Shown()
         │   │   │
         │   │   ├─> RequiredFieldHelper.MarkRequiredFields()
         │   │   ├─> SetupSuperToolTips()
         │   │   ├─> SetupPartnerTypeComboBox()
         │   │   └─> [Skip LoadDetailAsync - IsEditMode = false]
         │   │
         │   └─> Form displays (empty fields)
         │
         ▼
User enters data
         │
         ▼
User clicks "Lưu" button
         │
         ▼
SaveBarButtonItem_ItemClick()
         │
         ├─> ValidateInput()
         │   │
         │   ├─> ValidatePartnerCode()
         │   │   ├─> Check required
         │   │   └─> Check unique (BLL.IsCodeExists())
         │   │
         │   ├─> ValidatePartnerName()
         │   │   └─> Check required
         │   │
         │   └─> ValidatePartnerType()
         │       └─> Check selected
         │
         ├─> If valid → ExecuteWithWaitingFormAsync()
         │   │
         │   ├─> Show waiting form
         │   │
         │   ├─> SaveBusinessPartnerAsync()
         │   │   │
         │   │   ├─> BuildDetailDtoFromForm()
         │   │   │   ├─> Read values from controls
         │   │   │   ├─> GetPartnerTypeFromComboBox()
         │   │   │   └─> Return DTO
         │   │   │
         │   │   ├─> Convert DTO → Entity
         │   │   │   ├─> existing = null (new mode)
         │   │   │   └─> entity = detailDto.ToEntity(null)
         │   │   │
         │   │   ├─> BLL.SaveOrUpdate(entity)
         │   │   │   │
         │   │   │   └─> [BLL] → [DAL] → Database INSERT
         │   │   │   └─> Repository sets entity.Id
         │   │   │
         │   │   ├─> UploadLogoIfValidAsync(entity.Id)
         │   │   │   │
         │   │   │   ├─> ImageToByteArray()
         │   │   │   ├─> Check size (max 10MB)
         │   │   │   ├─> Check format (JPG/PNG/GIF)
         │   │   │   └─> BLL.UploadLogoFromBytesAsync()
         │   │   │       │
         │   │   │       └─> [BLL] → Save original to NAS
         │   │   │       └─> [BLL] → Save thumbnail to DB
         │   │   │
         │   │   ├─> Get saved entity
         │   │   │   ├─> BLL.GetByIdAsync(entity.Id)
         │   │   │   └─> BLL.GetCategoryDictAsync()
         │   │   │
         │   │   ├─> Convert Entity → ListDto
         │   │   │   └─> savedEntity.ToListDto(categoryDict)
         │   │   │
         │   │   └─> Trigger event
         │   │       └─> PartnerSaved?.Invoke(listDto)
         │   │
         │   └─> Close waiting form
         │
         ├─> MsgBox.ShowSuccess()
         ├─> DialogResult = DialogResult.OK
         └─> Close()
         │
         ▼
Parent form receives DialogResult.OK
         │
         └─> Event PartnerSaved triggered
         └─> Update datasource (if subscribed)
```

### 4.2. Luồng Chỉnh Sửa

```
User clicks "Điều chỉnh" button in parent form
         │
         ▼
new FrmBusinessPartnerDetail(businessPartnerId)
         │
         ├─> Constructor
         │   ├─> InitializeComponent()
         │   ├─> _businessPartnerId = businessPartnerId
         │   └─> Shown += FrmBusinessPartnerDetail_Shown
         │
         ├─> Form.Shown event
         │   │
         │   ├─> FrmBusinessPartnerDetail_Shown()
         │   │   │
         │   │   ├─> RequiredFieldHelper.MarkRequiredFields()
         │   │   ├─> SetupSuperToolTips()
         │   │   ├─> SetupPartnerTypeComboBox()
         │   │   └─> LoadDetailAsync(_businessPartnerId) [IsEditMode = true]
         │   │       │
         │   │       ├─> BLL.GetByIdAsync(id)
         │   │       │
         │   │       ├─> LoadBasicInformation(entity)
         │   │       │   └─> Set all TextEdit values
         │   │       │
         │   │       ├─> LoadPartnerType(entity)
         │   │       │   └─> Set ComboBox SelectedIndex
         │   │       │
         │   │       └─> LoadLogo(entity)
         │   │           │
         │   │           ├─> _isLoadingLogo = true
         │   │           ├─> entity.LogoThumbnailData.ToArray()
         │   │           ├─> Image.FromStream()
         │   │           ├─> CloneImage()
         │   │           ├─> LogoThumbnailDataPictureEdit.Image = clonedImage
         │   │           └─> _isLoadingLogo = false
         │   │
         │   └─> Form displays (with existing data)
         │
         ▼
User modifies data
         │
         ▼
User changes logo (optional)
         │
         ▼
LogoThumbnailDataPictureEdit_ImageChanged()
         │
         ├─> Check _isLoadingLogo (skip if true)
         ├─> Check IsEditMode (skip if false)
         │
         ├─> ExecuteWithWaitingFormAsync()
         │   │
         │   ├─> HandleLogoUploadAsync()
         │   │   │
         │   │   ├─> ImageToByteArray()
         │   │   ├─> Check size (max 10MB)
         │   │   ├─> Check format (JPG/PNG/GIF)
         │   │   ├─> BLL.UploadLogoFromBytesAsync()
         │   │   │   │
         │   │   │   └─> [BLL] → Save original to NAS
         │   │   │   └─> [BLL] → Save thumbnail to DB
         │   │   │
         │   │   ├─> MsgBox.ShowSuccess()
         │   │   └─> ReloadLogoAsync()
         │   │       │
         │   │       ├─> BLL.GetByIdAsync()
         │   │       ├─> Load logo thumbnail
         │   │       └─> Update PictureEdit
         │   │
         │   └─> Close waiting form
         │
         └─> If error → ReloadLogoAsync() (rollback)
         │
         ▼
User clicks "Lưu" button
         │
         ▼
SaveBarButtonItem_ItemClick()
         │
         ├─> ValidateInput()
         │   │
         │   ├─> ValidatePartnerCode()
         │   │   ├─> Check required
         │   │   └─> Check unique (exclude current if code unchanged)
         │   │
         │   ├─> ValidatePartnerName()
         │   │   └─> Check required
         │   │
         │   └─> ValidatePartnerType()
         │       └─> Check selected
         │
         ├─> If valid → ExecuteWithWaitingFormAsync()
         │   │
         │   ├─> SaveBusinessPartnerAsync()
         │   │   │
         │   │   ├─> BuildDetailDtoFromForm()
         │   │   │   └─> Read values (including _businessPartnerId)
         │   │   │
         │   │   ├─> Convert DTO → Entity
         │   │   │   ├─> existing = BLL.GetById(detailDto.Id)
         │   │   │   └─> entity = detailDto.ToEntity(existing)
         │   │   │
         │   │   ├─> BLL.SaveOrUpdate(entity)
         │   │   │   │
         │   │   │   └─> [BLL] → [DAL] → Database UPDATE
         │   │   │
         │   │   ├─> UploadLogoIfValidAsync(entity.Id)
         │   │   │   └─> [Only if logo changed and not uploaded yet]
         │   │   │
         │   │   ├─> Get saved entity
         │   │   │   └─> BLL.GetByIdAsync(entity.Id)
         │   │   │
         │   │   ├─> Convert Entity → ListDto
         │   │   │   └─> savedEntity.ToListDto(categoryDict)
         │   │   │
         │   │   └─> Trigger event
         │   │       └─> PartnerSaved?.Invoke(listDto)
         │   │
         │   └─> Close waiting form
         │
         ├─> MsgBox.ShowSuccess()
         ├─> DialogResult = DialogResult.OK
         └─> Close()
         │
         ▼
Parent form receives DialogResult.OK
         │
         └─> Event PartnerSaved triggered
         └─> Update datasource (if subscribed)
```

### 4.3. Luồng Upload Logo (Edit Mode)

```
User changes logo in PictureEdit
         │
         ▼
LogoThumbnailDataPictureEdit_ImageChanged event
         │
         ├─> Check _isLoadingLogo (skip if loading)
         ├─> Check IsEditMode (skip if new mode)
         │
         ├─> ExecuteWithWaitingFormAsync()
         │   │
         │   ├─> HandleLogoUploadAsync()
         │   │   │
         │   │   ├─> ImageToByteArray(pictureEdit.Image)
         │   │   │   │
         │   │   │   ├─> CloneImage(image)
         │   │   │   ├─> Save to MemoryStream (JPEG)
         │   │   │   └─> Return byte[]
         │   │   │
         │   │   ├─> Check size (max 10MB)
         │   │   │   └─> If exceed → Warning + ReloadLogoAsync()
         │   │   │
         │   │   ├─> Check format (IsValidImageFormat)
         │   │   │   │
         │   │   │   ├─> Check JPEG magic bytes (FF D8 FF)
         │   │   │   ├─> Check PNG magic bytes (89 50 4E 47)
         │   │   │   └─> Check GIF magic bytes (47 49 46 38)
         │   │   │   │
         │   │   │   └─> If invalid → Warning + ReloadLogoAsync()
         │   │   │
         │   │   ├─> BLL.UploadLogoFromBytesAsync()
         │   │   │   │
         │   │   │   ├─> [BLL] Compress image
         │   │   │   ├─> [BLL] Save original to NAS
         │   │   │   ├─> [BLL] Create thumbnail (300px)
         │   │   │   └─> [BLL] Save thumbnail to DB
         │   │   │
         │   │   ├─> MsgBox.ShowSuccess()
         │   │   │
         │   │   └─> ReloadLogoAsync()
         │   │       │
         │   │       ├─> _isLoadingLogo = true
         │   │       ├─> BLL.GetByIdAsync(_businessPartnerId)
         │   │       ├─> Load thumbnail from DB
         │   │       ├─> CloneImage()
         │   │       ├─> Update PictureEdit
         │   │       └─> _isLoadingLogo = false
         │   │
         │   └─> Close waiting form
         │
         └─> If error → ReloadLogoAsync() (rollback to old logo)
```

### 4.4. Luồng Validation

```
ValidateInput()
         │
         ├─> dxErrorProvider1.ClearErrors()
         │
         ├─> ValidatePartnerCode()
         │   │
         │   ├─> Check required
         │   │   └─> If empty → SetError + Focus + Return false
         │   │
         │   └─> Check unique
         │       │
         │       ├─> If EditMode
         │       │   ├─> Get existing partner
         │       │   ├─> If code changed
         │       │   │   └─> Check BLL.IsCodeExists()
         │       │   └─> If exists → SetError + Focus + Return false
         │       │
         │       └─> If NewMode
         │           └─> Check BLL.IsCodeExists()
         │               └─> If exists → SetError + Focus + Return false
         │
         ├─> ValidatePartnerName()
         │   │
         │   └─> Check required
         │       └─> If empty → SetError + Focus + Return false
         │
         ├─> ValidatePartnerType()
         │   │
         │   └─> Check selected
         │       └─> If not selected → SetError + Focus + Return false
         │
         └─> Return true (all valid)
```

---

## 5. Lưu Ý Khi Mở Rộng Hoặc Sửa Đổi

### 5.1. Edit Mode vs New Mode

⚠️ **Quan trọng**: Form hoạt động ở 2 chế độ khác nhau:
- **New Mode** (`_businessPartnerId == Guid.Empty`):
  - Không load dữ liệu
  - Logo không upload tự động (chỉ upload sau khi lưu thành công)
  - Validation kiểm tra trùng mã đối tác
- **Edit Mode** (`_businessPartnerId != Guid.Empty`):
  - Load dữ liệu từ database
  - Logo upload tự động khi thay đổi
  - Validation kiểm tra trùng mã (chỉ khi mã đã thay đổi)

**Khi thêm logic mới**:
- Luôn kiểm tra `IsEditMode` trước khi thực hiện logic chỉ dành cho edit mode
- Sử dụng pattern:
  ```csharp
  if (IsEditMode)
  {
      // Edit-specific logic
  }
  ```

---

### 5.2. Logo Upload Timing

⚠️ **Quan trọng**: Logo được upload ở 2 thời điểm khác nhau:

1. **Tự động upload (Edit Mode)**: 
   - Khi user thay đổi logo trong `PictureEdit`
   - Event `ImageChanged` được trigger
   - Upload ngay lập tức (cần có `Id`)

2. **Upload khi lưu (New Mode hoặc chưa upload)**:
   - Sau khi lưu entity thành công (đã có `Id`)
   - Trong method `SaveBusinessPartnerAsync()`
   - Chỉ upload nếu logo chưa được upload

**Lưu ý**: 
- Trong new mode, logo không thể upload tự động vì chưa có `Id`
- Logo sẽ được upload sau khi lưu entity thành công
- Flag `_isLoadingLogo` ngăn trigger event khi load logo từ database

---

### 5.3. Image Handling và GDI+ Errors

⚠️ **Phức tạp**: Xử lý Image trong .NET có thể gặp lỗi GDI+ nếu không cẩn thận.

**Các vấn đề thường gặp**:
1. **"A generic error occurred in GDI+"**: 
   - Xảy ra khi Image được load từ stream đã bị dispose
   - **Giải pháp**: Clone Image trước khi dispose stream

2. **Image bị lock**:
   - Xảy ra khi Image đang được sử dụng bởi control
   - **Giải pháp**: Clone Image trước khi convert sang byte array

**Best Practice**:
```csharp
// Load Image
Image loadedImage = null;
try
{
    using (var ms = new MemoryStream(bytes))
    {
        loadedImage = Image.FromStream(ms);
    }
    
    // Clone để tạo bản copy độc lập
    var clonedImage = CloneImage(loadedImage);
    
    // Dispose Image gốc
    loadedImage?.Dispose();
    
    // Sử dụng clonedImage
    pictureEdit.Image = clonedImage;
}
catch
{
    loadedImage?.Dispose();
    throw;
}
```

---

### 5.4. Flag _isLoadingLogo

⚠️ **Quan trọng**: Flag `_isLoadingLogo` ngăn trigger event `ImageChanged` khi load logo từ database.

**Khi nào set flag**:
- `_isLoadingLogo = true` trước khi load logo từ database
- `_isLoadingLogo = false` sau khi load xong (trong finally block)

**Khi nào check flag**:
- Trong `LogoThumbnailDataPictureEdit_ImageChanged()`:
  ```csharp
  if (_isLoadingLogo) return; // Skip event handler
  ```

**Lưu ý**: 
- Nếu không có flag, việc load logo sẽ trigger event `ImageChanged` → Upload logo ngay → Infinite loop
- Luôn reset flag trong finally block để đảm bảo flag luôn được reset

---

### 5.5. Validation Logic

✅ **Pattern hiện tại**: Validation được thực hiện trong `ValidateInput()` với `DXErrorProvider`.

**Khi thêm validation mới**:
1. Clear errors trước: `dxErrorProvider1.ClearErrors()`
2. Kiểm tra điều kiện
3. Nếu lỗi: 
   ```csharp
   dxErrorProvider1.SetError(control, "Error message", ErrorType.Critical);
   control.Focus();
   return false;
   ```
4. Return `true` nếu tất cả đều hợp lệ

**Lưu ý**: 
- Validation được thực hiện khi user click "Lưu"
- Có thể thêm real-time validation trong `TextChanged` events nếu cần
- Sử dụng `ErrorType.Critical` cho lỗi bắt buộc, `ErrorType.Warning` cho khuyến nghị

---

### 5.6. Required Fields

✅ **Pattern hiện tại**: Sử dụng `RequiredFieldHelper.MarkRequiredFields()` với DataAnnotations.

**Cách hoạt động**:
- `RequiredFieldHelper` đọc `[Required]` attributes từ DTO
- Tự động đánh dấu các controls tương ứng

**Khi thêm field mới**:
- Thêm `[Required]` attribute vào property trong DTO
- `RequiredFieldHelper` sẽ tự động đánh dấu control

**Lưu ý**: 
- Không cần manually mark required fields
- Đảm bảo control name match với property name (convention)

---

### 5.7. Data Binding

⚠️ **Lưu ý**: Form sử dụng manual binding (không dùng data binding tự động).

**Pattern hiện tại**:
- **Load**: 
  - `LoadBasicInformation(entity)` - Entity → Controls
  - `LoadPartnerType(entity)` - Entity → ComboBox
  - `LoadLogo(entity)` - Entity → PictureEdit
- **Save**: 
  - `BuildDetailDtoFromForm()` - Controls → DTO
  - `GetPartnerTypeFromComboBox()` - ComboBox → PartnerType

**Khi thêm field mới**:
1. Thêm control vào Designer
2. Update `LoadBasicInformation()`: `Control.Text = entity.Property`
3. Update `BuildDetailDtoFromForm()`: `dto.Property = Control.EditValue?.ToString()`

**Lưu ý**: 
- Luôn `.Trim()` string values
- Xử lý null values cẩn thận
- ComboBox cần xử lý đặc biệt (SelectedIndex → PartnerType)

---

### 5.8. Event PartnerSaved

✅ **Pattern hiện tại**: Sử dụng event để communicate với form cha.

**Cách sử dụng**:
```csharp
// In parent form
using (var form = new FrmBusinessPartnerDetail(partnerId))
{
    form.PartnerSaved += (listDto) =>
    {
        // Update datasource với listDto mới
        UpdateDataSource(listDto);
    };
    
    if (form.ShowDialog() == DialogResult.OK)
    {
        // Event đã được trigger, datasource đã được update
    }
}
```

**Lưu ý**: 
- Event được trigger sau khi lưu thành công
- `listDto` chứa đầy đủ thông tin để update grid
- Form cha có thể update datasource mà không cần reload toàn bộ

---

### 5.9. Error Handling

✅ **Pattern hiện tại**: Try-catch trong các methods chính.

**Khi thêm error handling**:
```csharp
try
{
    // Operation
}
catch (Exception ex)
{
    ShowError(ex, "Context message");
    // Không throw lại để không crash form
}
```

**Lưu ý**: 
- Luôn hiển thị thông báo cho user
- Log exception nếu cần (hiện tại chỉ dùng `ShowError`)
- Không "nuốt" exception mà không thông báo
- Logo upload có rollback mechanism (reload logo cũ khi lỗi)

---

### 5.10. DialogResult Pattern

✅ **Pattern hiện tại**: Sử dụng `DialogResult` để communicate với form cha.

**Values**:
- `DialogResult.OK` → Lưu thành công, form cha có thể reload data
- `DialogResult.Cancel` → Hủy, form cha không reload (mặc định)

**Khi sửa đổi**:
- Luôn set `DialogResult` trước khi `Close()`
- Form cha sẽ check `DialogResult` để quyết định có reload hay không

---

### 5.11. Image Format Validation

⚠️ **Quan trọng**: Validation format bằng magic bytes, không phải extension.

**Magic bytes được kiểm tra**:
- **JPEG**: `FF D8 FF` (3 bytes đầu)
- **PNG**: `89 50 4E 47` (4 bytes đầu)
- **GIF**: `47 49 46 38` (4 bytes đầu - "GIF8")

**Lưu ý**: 
- Kiểm tra magic bytes an toàn hơn kiểm tra extension
- User có thể đổi extension nhưng magic bytes không thể fake
- Hỗ trợ JPG, PNG, GIF

**Khi thêm format mới**:
1. Thêm magic bytes check trong `IsValidImageFormat()`
2. Đảm bảo BLL hỗ trợ format mới
3. Update tooltip để thông báo format mới

---

### 5.12. Image Size Limit

⚠️ **Quan trọng**: Kích thước logo tối đa 10MB.

**Constants**:
- `MaxLogoSizeInBytes = 10 * 1024 * 1024` (10MB)
- `ThumbnailMaxDimension = 300` (300px)

**Lưu ý**: 
- File gốc lưu trên NAS (không giới hạn kích thước trong code, nhưng nên giới hạn)
- Thumbnail lưu trong database (300px để giảm kích thước)
- Validation kích thước trước khi upload

**Khi thay đổi giới hạn**:
- Update constant `MaxLogoSizeInBytes`
- Update thông báo warning
- Cân nhắc ảnh hưởng đến performance và storage

---

### 5.13. Performance Considerations

💡 **Tối ưu hóa**:
- Logo upload tự động có thể chậm với file lớn
- Sử dụng waiting form để cải thiện UX
- Thumbnail được tạo để giảm kích thước database

**Nếu cần tối ưu thêm**:
- Cân nhắc lazy loading cho logo (chỉ load khi cần)
- Cân nhắc caching logo thumbnail
- Cân nhắc compress image trước khi upload

---

### 5.14. Testing

✅ **Khi thêm/chỉnh sửa code**:
- Test với **new mode** (Guid.Empty)
- Test với **edit mode** (có businessPartnerId)
- Test validation:
  - Required fields
  - Code uniqueness
  - Partner type selection
- Test logo upload:
  - Upload trong edit mode (tự động)
  - Upload khi lưu (new mode)
  - Upload file lớn (>10MB) → Should fail
  - Upload file format không hợp lệ → Should fail
  - Upload file hợp lệ → Should succeed
- Test với dữ liệu edge cases:
  - PartnerCode = ""
  - PartnerCode = very long string
  - PartnerType = not selected
  - Logo = null
  - Logo = very large file

---

### 5.15. Code Style

✅ **Tuân thủ**:
- Sử dụng regions để tổ chức code
- XML documentation comments cho public/protected methods
- Naming convention:
  - Private methods: `PascalCase`
  - Private fields: `_camelCase`
  - Events: `ObjectName_EventName`
  - Constants: `PascalCase`

---

### 5.16. Dependencies

⚠️ **Khi thay đổi dependencies**:
- `BusinessPartnerBll`: Nếu thay đổi interface, cần update tất cả calls
- `BusinessPartnerDetailDto`: Nếu thêm/sửa properties:
  - Update `BuildDetailDtoFromForm()`
  - Update validation nếu cần
- `BusinessPartnerListDto`: Nếu thay đổi, cần update event `PartnerSaved`
- `RequiredFieldHelper`: Nếu thay đổi cách hoạt động, cần test lại required fields marking

---

## 6. Tóm Tắt

### Điểm Mạnh:
✅ Hỗ trợ cả thêm mới và chỉnh sửa trong cùng một form  
✅ Validation đầy đủ với DXErrorProvider  
✅ Upload logo tự động trong edit mode  
✅ Xử lý Image an toàn (clone để tránh GDI+ errors)  
✅ Hỗ trợ SuperToolTip cho tất cả controls  
✅ Event-driven architecture (PartnerSaved event)  
✅ Required fields tự động từ DataAnnotations  
✅ User experience tốt (waiting form, error messages, tooltips)  
✅ Rollback mechanism cho logo upload  

### Điểm Cần Lưu Ý:
⚠️ Image handling phức tạp và dễ lỗi GDI+  
⚠️ Flag `_isLoadingLogo` cần được quản lý cẩn thận  
⚠️ Logo upload timing khác nhau giữa new mode và edit mode  
⚠️ Manual data binding (không tự động)  
⚠️ Validation chỉ khi click "Lưu" (không real-time)  

### Khuyến Nghị:
💡 Cân nhắc thêm real-time validation (TextChanged events)  
💡 Cân nhắc thêm progress bar cho logo upload  
💡 Cân nhắc unit tests cho validation logic  
💡 Cân nhắc helper method để parse EditValue (reusable)  
💡 Cân nhắc thêm preview logo trước khi upload  

---

**Tài liệu này được tạo tự động dựa trên phân tích code. Cập nhật lần cuối: 2025-01-XX**
