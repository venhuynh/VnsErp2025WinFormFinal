# Tài Liệu Kỹ Thuật - UcCompany (User Control Quản Lý Thông Tin Công Ty)

## Mục Lục

1. [Overview](#overview)
2. [Architecture](#architecture)
3. [Detailed Technical Breakdown](#detailed-technical-breakdown)
4. [Validation System](#validation-system)
5. [Business Logic Flow](#business-logic-flow)
6. [Error Handling](#error-handling)
7. [Security & Best Practices](#security--best-practices)
8. [Extensibility Guide](#extensibility-guide)
9. [Test Checklist](#test-checklist)
10. [Changelog Template](#changelog-template)

---

## 1. Overview

### 1.1. Vai Trò Trong Module

**UcCompany** là User Control thuộc module **MasterData.Company**, có vai trò:

- **Quản lý thông tin công ty duy nhất** trong hệ thống ERP
- **Hiển thị và chỉnh sửa** thông tin công ty từ database
- **Quản lý logo** công ty với các chức năng: Load, Delete, Drag & Drop
- **Đảm bảo tính nhất quán**: Tự động đảm bảo chỉ có 1 công ty trong database

### 1.2. File Structure

```
MasterData/Company/
├── UcCompany.cs                    # Main code-behind file
├── UcCompany.Designer.cs           # Designer-generated code
└── UcCompany.resx                  # Resources file

MasterData/Company/Dto/
└── CompanyDto.cs                   # Data Transfer Object

MasterData/Company/Converters/
└── CompanyConverter.cs         # Entity ↔ DTO converter

Bll/MasterData/Company/
└── CompanyBll.cs                   # Business Logic Layer

Dal/DataAccess/MasterData/CompanyDal/
└── CompanyDataAccess.cs            # Data Access Layer

Bll/Utils/
├── RequiredFieldHelper.cs          # Helper đánh dấu trường bắt buộc
└── SuperToolTipHelper.cs           # Helper tạo SuperToolTip
```

### 1.3. Dependencies

**DevExpress Controls:**
- `XtraUserControl` - Base class
- `DataLayoutControl` - Layout container
- `TextEdit` - Text input controls
- `DateEdit` - Date picker
- `PictureEdit` - Image display/management
- `BarManager`, `BarButtonItem` - Toolbar
- `DXErrorProvider` - Error display
- `SuperToolTip` - Rich tooltips

**Internal Dependencies:**
- `Bll.MasterData.Company.CompanyBll` - Business logic
- `Dal.DataAccess.MasterData.CompanyDal.CompanyDataAccess` - Data access
- `MasterData.Company.Dto.CompanyDto` - DTO
- `MasterData.Company.Converters.CompanyConverter` - Converter
- `Bll.Utils.RequiredFieldHelper` - Required field marker
- `Bll.Utils.SuperToolTipHelper` - Tooltip helper
- `Dal.Logging.ILogger` - Logging interface

---

## 2. Architecture

### 2.1. Layer Architecture

```
┌─────────────────────────────────────────────────────────┐
│                    UI Layer (UcCompany)                 │
│  ┌───────────────────────────────────────────────────┐  │
│  │  - XtraUserControl                                │  │
│  │  - DataLayoutControl (Layout)                     │  │
│  │  - TextEdit, DateEdit, PictureEdit (Controls)    │  │
│  │  - Event Handlers                                 │  │
│  │  - Validation UI (DXErrorProvider)                │  │
│  └───────────────────────────────────────────────────┘  │
└───────────────────────┬─────────────────────────────────┘
                        │
                        │ Uses
                        ▼
┌─────────────────────────────────────────────────────────┐
│              Business Logic Layer (CompanyBll)          │
│  ┌───────────────────────────────────────────────────┐  │
│  │  - EnsureSingleCompany()                          │  │
│  │  - GetCompany()                                   │  │
│  │  - UpdateCompany()                                │  │
│  └───────────────────────────────────────────────────┘  │
└───────────────────────┬─────────────────────────────────┘
                        │
                        │ Uses
                        ▼
┌─────────────────────────────────────────────────────────┐
│          Data Access Layer (CompanyDataAccess)          │
│  ┌───────────────────────────────────────────────────┐  │
│  │  - EnsureDefaultCompany()                         │  │
│  │  - GetCompany()                                  │  │
│  │  - UpdateCompany()                               │  │
│  └───────────────────────────────────────────────────┘  │
└───────────────────────┬─────────────────────────────────┘
                        │
                        │ Uses
                        ▼
┌─────────────────────────────────────────────────────────┐
│              Database (LINQ to SQL)                     │
│  ┌───────────────────────────────────────────────────┐  │
│  │  - Company Table                                 │  │
│  └───────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────┘
```

### 2.2. Data Flow Diagram

```
┌──────────────┐
│   Database   │
│   (Company)  │
└──────┬───────┘
       │
       │ Read
       ▼
┌──────────────────┐
│ CompanyDataAccess│
│  GetCompany()    │
└──────┬───────────┘
       │
       │ Returns Entity
       ▼
┌──────────────────┐
│   CompanyBll     │
│  GetCompany()    │
└──────┬───────────┘
       │
       │ Returns Entity
       ▼
┌──────────────────┐
│  UcCompany       │
│  DisplayCompany  │
│      Info()      │
└──────┬───────────┘
       │
       │ Converts Entity → DTO
       ▼
┌──────────────────┐
│ CompanyConverter │
│    ToDto()       │
└──────┬───────────┘
       │
       │ Displays DTO on UI
       ▼
┌──────────────────┐
│   UI Controls    │
│  (TextEdit, etc.)│
└──────────────────┘
```

### 2.3. Component Diagram (ASCII)

```
┌─────────────────────────────────────────────────────────────┐
│                      UcCompany                              │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│  ┌────────────────────────────────────────────────────┐    │
│  │              BarManager (Toolbar)                   │    │
│  │  ┌──────────────────────────────────────────────┐ │    │
│  │  │  SaveBarButtonItem (⚠️ No event handler)      │ │    │
│  │  └──────────────────────────────────────────────┘ │    │
│  └────────────────────────────────────────────────────┘    │
│                                                              │
│  ┌────────────────────────────────────────────────────┐    │
│  │          DataLayoutControl (Layout Container)       │    │
│  │                                                       │    │
│  │  ┌──────────────────────────────────────────────┐  │    │
│  │  │  CompanyCodeTextEdit (Required)               │  │    │
│  │  │  CompanyNameTextEdit (Required)               │  │    │
│  │  │  TaxCodeTextEdit (Optional)                  │  │    │
│  │  │  PhoneTextEdit (Optional)                    │  │    │
│  │  │  EmailTextEdit (Optional, Email validation)  │  │    │
│  │  │  WebsiteTextEdit (Optional)                   │  │    │
│  │  │  AddressTextEdit (Optional)                   │  │    │
│  │  │  CountryTextEdit (Optional)                  │  │    │
│  │  │  CreatedDateDateEdit (Required, Read-only)   │  │    │
│  │  │  LogoPictureEdit (Optional, Image manager)   │  │    │
│  │  └──────────────────────────────────────────────┘  │    │
│  └────────────────────────────────────────────────────┘    │
│                                                              │
│  ┌────────────────────────────────────────────────────┐    │
│  │          DXErrorProvider (Error Display)           │    │
│  └────────────────────────────────────────────────────┘    │
│                                                              │
│  ┌────────────────────────────────────────────────────┐    │
│  │          CompanyBll (Business Logic)                │    │
│  └────────────────────────────────────────────────────┘    │
│                                                              │
└─────────────────────────────────────────────────────────────┘
```

---

## 3. Detailed Technical Breakdown

### 3.1. Controls trong Designer

#### 3.1.1. DataLayoutControl

```csharp
private DataLayoutControl dataLayoutControl1;
```

- **Vị trí**: Dock = Fill
- **Chức năng**: Container chứa tất cả các controls, tự động sắp xếp layout
- **Root**: `Root` (LayoutControlGroup)

#### 3.1.2. TextEdit Controls

| Control Name | Property | Required | Max Length | Validation |
|--------------|----------|----------|------------|------------|
| `CompanyCodeTextEdit` | CompanyCode | ✅ Yes | 50 | Required, StringLength |
| `CompanyNameTextEdit` | CompanyName | ✅ Yes | 255 | Required, StringLength |
| `TaxCodeTextEdit` | TaxCode | ❌ No | 50 | StringLength |
| `PhoneTextEdit` | Phone | ❌ No | 50 | StringLength |
| `EmailTextEdit` | Email | ❌ No | 100 | StringLength, EmailAddress |
| `WebsiteTextEdit` | Website | ❌ No | 100 | StringLength |
| `AddressTextEdit` | Address | ❌ No | 255 | StringLength |
| `CountryTextEdit` | Country | ❌ No | 100 | StringLength |

**Cấu hình chung:**
- `AllowNullInput = DefaultBoolean.False` (cho các trường required)
- `MenuManager = barManager1`

#### 3.1.3. DateEdit Control

```csharp
private DateEdit CreatedDateDateEdit;
```

- **Property**: `CreatedDate`
- **Required**: ✅ Yes
- **Read-only**: ✅ Yes (không cho phép chỉnh sửa)
- **Chức năng**: Hiển thị ngày tạo công ty

#### 3.1.4. PictureEdit Control

```csharp
private PictureEdit LogoPictureEdit;
```

- **Property**: `Logo`
- **Required**: ❌ No
- **SizeMode**: `Squeeze` (trong Designer), `Zoom` (trong code)
- **Chức năng**: 
  - Hiển thị logo công ty
  - Context menu: Load, Delete
  - Drag & Drop support

#### 3.1.5. BarManager & Toolbar

```csharp
private BarManager barManager1;
private Bar bar2;
private BarButtonItem SaveBarButtonItem;
```

- **Bar2**: Main menu bar (Dock = Top)
- **SaveBarButtonItem**: 
  - Caption: "Lưu"
  - Image: `save_16x16`, `save_32x32`
  - ⚠️ **Lưu ý**: Chưa có event handler `ItemClick`

#### 3.1.6. DXErrorProvider

```csharp
private DXErrorProvider dxErrorProvider1;
```

- **ContainerControl**: `this` (UcCompany)
- **Chức năng**: Hiển thị lỗi validation bên cạnh các controls

### 3.2. Event Handlers

#### 3.2.1. UcCompany_Load

```csharp
private void UcCompany_Load(object sender, EventArgs e)
```

**Chức năng:**
- Đảm bảo chỉ có 1 công ty trong database (`EnsureSingleCompany()`)
- Đánh dấu các trường bắt buộc (`MarkRequiredFields()`)
- Cấu hình LogoPictureEdit (`ConfigureLogoPictureEdit()`)
- Hiển thị thông tin công ty (`DisplayCompanyInfo()`)
- Setup SuperToolTips (`SetupSuperTips()`)

**Flow:**
```
Load Event
  ├─> EnsureSingleCompany()
  ├─> MarkRequiredFields(typeof(CompanyDto))
  ├─> ConfigureLogoPictureEdit()
  ├─> DisplayCompanyInfo()
  └─> SetupSuperTips()
```

#### 3.2.2. LoadLogo_Click

```csharp
private void LoadLogo_Click(object sender, EventArgs e)
```

**Chức năng:**
- Mở OpenFileDialog để chọn file hình ảnh
- Load hình ảnh vào `LogoPictureEdit`
- Lưu logo vào database (`SaveLogoToDatabase()`)

**Flow:**
```
Click "Load..."
  ├─> OpenFileDialog.ShowDialog()
  ├─> File.ReadAllBytes(imagePath)
  ├─> LogoPictureEdit.Image = Image.FromFile(imagePath)
  └─> SaveLogoToDatabase(imageBytes)
```

#### 3.2.3. DeleteLogo_Click

```csharp
private void DeleteLogo_Click(object sender, EventArgs e)
```

**Chức năng:**
- Xác nhận xóa logo
- Xóa logo khỏi `LogoPictureEdit`
- Xóa logo khỏi database (`DeleteLogoFromDatabase()`)

**Flow:**
```
Click "Delete"
  ├─> XtraMessageBox.ShowYesNo("Xác nhận xóa?")
  ├─> LogoPictureEdit.Image = null
  └─> DeleteLogoFromDatabase()
```

#### 3.2.4. LogoPictureEdit_DragEnter

```csharp
private void LogoPictureEdit_DragEnter(object sender, DragEventArgs e)
```

**Chức năng:**
- Kiểm tra xem có phải file drop không
- Set `DragDropEffects.Copy` nếu hợp lệ

#### 3.2.5. LogoPictureEdit_DragDrop

```csharp
private void LogoPictureEdit_DragDrop(object sender, DragEventArgs e)
```

**Chức năng:**
- Lấy file từ drag & drop
- Kiểm tra định dạng file (JPG, PNG, BMP, GIF)
- Load hình ảnh vào `LogoPictureEdit`
- Lưu logo vào database

**Flow:**
```
Drag & Drop File
  ├─> Get file from DragEventArgs
  ├─> Check extension (.jpg, .png, .bmp, .gif)
  ├─> LogoPictureEdit.Image = Image.FromFile(filePath)
  └─> SaveLogoToDatabase(imageBytes)
```

### 3.3. Methods

#### 3.3.1. Public Methods

**Không có public methods** (chỉ có constructors)

#### 3.3.2. Private Methods

##### DisplayCompanyInfo()

```csharp
private void DisplayCompanyInfo()
```

**Chức năng:**
- Lấy thông tin công ty từ database qua `CompanyBll.GetCompany()`
- Convert Entity → DTO qua `CompanyConverter.ToDto()`
- Hiển thị DTO lên các controls
- Load logo từ byte array

**Code Flow:**
```csharp
var company = _companyBll.GetCompany() as Company;
var companyDto = company.ToDto();
CompanyCodeTextEdit.Text = companyDto.CompanyCode ?? "";
// ... (các trường khác)
if (companyDto.Logo != null && companyDto.Logo.Length > 0)
{
    using (var ms = new MemoryStream(companyDto.Logo))
    {
        LogoPictureEdit.Image = Image.FromStream(ms);
    }
}
```

##### MarkRequiredFields()

```csharp
private void MarkRequiredFields(Type dtoType)
```

**Chức năng:**
- Gọi `RequiredFieldHelper.MarkRequiredFields()` để đánh dấu các trường bắt buộc
- Thêm dấu * đỏ vào caption của LayoutControlItem
- Set `NullValuePrompt` cho các BaseEdit controls

##### ConfigureLogoPictureEdit()

```csharp
private void ConfigureLogoPictureEdit()
```

**Chức năng:**
- Tắt menu mặc định (`ShowMenu = false`)
- Tạo ContextMenuStrip tùy chỉnh với "Load..." và "Delete"
- Cấu hình SizeMode = Zoom
- Enable Drag & Drop

##### SaveLogoToDatabase()

```csharp
private void SaveLogoToDatabase(byte[] logoBytes)
```

**Chức năng:**
- Lấy thông tin công ty hiện tại
- Cập nhật `company.Logo` và `company.UpdatedDate`
- Gọi `CompanyBll.UpdateCompany()` để lưu vào database

##### DeleteLogoFromDatabase()

```csharp
private void DeleteLogoFromDatabase()
```

**Chức năng:**
- Lấy thông tin công ty hiện tại
- Set `company.Logo = null` và `company.UpdatedDate = DateTime.Now`
- Gọi `CompanyBll.UpdateCompany()` để cập nhật database

##### SetupSuperTips()

```csharp
private void SetupSuperTips()
```

**Chức năng:**
- Gọi các method setup tooltip:
  - `SetupTextEditSuperTips()`
  - `SetupDateEditSuperTips()`
  - `SetupPictureEditSuperTips()`

##### SetupTextEditSuperTips()

```csharp
private void SetupTextEditSuperTips()
```

**Chức năng:**
- Tạo SuperToolTip cho từng TextEdit control
- Sử dụng `SuperToolTipHelper.SetTextEditSuperTip()`
- Mỗi tooltip có title (HTML) và content (HTML) mô tả chi tiết

##### SetupDateEditSuperTips()

```csharp
private void SetupDateEditSuperTips()
```

**Chức năng:**
- Tạo SuperToolTip cho `CreatedDateDateEdit`
- Sử dụng `SuperToolTipHelper.SetBaseEditSuperTip()`

##### SetupPictureEditSuperTips()

```csharp
private void SetupPictureEditSuperTips()
```

**Chức năng:**
- Tạo SuperToolTip cho `LogoPictureEdit`
- Mô tả cách sử dụng: Load, Delete, Drag & Drop

### 3.4. Data Flow: Input → Validation → Business Logic → Output

```
┌─────────────────────────────────────────────────────────────┐
│                        INPUT                                 │
│  ┌──────────────────────────────────────────────────────┐   │
│  │  User enters data in TextEdit controls               │   │
│  │  User loads/deletes logo via PictureEdit             │   │
│  └──────────────────────────────────────────────────────┘   │
└───────────────────────┬─────────────────────────────────────┘
                        │
                        ▼
┌─────────────────────────────────────────────────────────────┐
│                    VALIDATION                                │
│  ┌──────────────────────────────────────────────────────┐   │
│  │  DataAnnotations (CompanyDto):                      │   │
│  │  - [Required] → Check empty                         │   │
│  │  - [StringLength] → Check max length                │   │
│  │  - [EmailAddress] → Check email format              │   │
│  │                                                      │   │
│  │  DXErrorProvider:                                    │   │
│  │  - Display errors next to controls                  │   │
│  │                                                      │   │
│  │  RequiredFieldHelper:                                │   │
│  │  - Mark required fields with red *                  │   │
│  │  - Set NullValuePrompt                               │   │
│  └──────────────────────────────────────────────────────┘   │
└───────────────────────┬─────────────────────────────────────┘
                        │
                        │ Valid?
                        ▼
┌─────────────────────────────────────────────────────────────┐
│                  BUSINESS LOGIC                              │
│  ┌──────────────────────────────────────────────────────┐   │
│  │  CompanyBll:                                         │   │
│  │  - EnsureSingleCompany()                             │   │
│  │  - GetCompany() → Returns Entity                      │   │
│  │  - UpdateCompany() → Saves to DB                     │   │
│  │                                                      │   │
│  │  CompanyConverter:                                   │   │
│  │  - ToDto() → Entity → DTO                            │   │
│  │  - ToEntity() → DTO → Entity                         │   │
│  └──────────────────────────────────────────────────────┘   │
└───────────────────────┬─────────────────────────────────────┘
                        │
                        ▼
┌─────────────────────────────────────────────────────────────┐
│                        OUTPUT                                │
│  ┌──────────────────────────────────────────────────────┐   │
│  │  Display data on UI controls                         │   │
│  │  Show success/error messages                         │   │
│  │  Update logo display                                 │   │
│  └──────────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────────┘
```

---

## 4. Validation System

### 4.1. DataAnnotations trong CompanyDto

| Property | Required | StringLength | EmailAddress | Other |
|----------|----------|--------------|--------------|-------|
| `Id` | ✅ | - | - | - |
| `CompanyCode` | ✅ | 50 | - | - |
| `CompanyName` | ✅ | 255 | - | - |
| `TaxCode` | ❌ | 50 | - | - |
| `Phone` | ❌ | 50 | - | - |
| `Email` | ❌ | 100 | ✅ | - |
| `Website` | ❌ | 100 | - | - |
| `Address` | ❌ | 255 | - | - |
| `Country` | ❌ | 100 | - | - |
| `CreatedDate` | ✅ | - | - | - |
| `Logo` | ❌ | - | - | - |

### 4.2. RequiredFieldHelper

**File**: `Bll/Utils/RequiredFieldHelper.cs`

**Chức năng:**
- Quét các properties có `[Required]` trong DTO
- Tìm LayoutControlItem tương ứng
- Thêm dấu * đỏ vào caption: `"Mã công ty <color=red>*</color>"`
- Set `NullValuePrompt = "Bắt buộc nhập"` cho BaseEdit controls

**Sử dụng trong UcCompany:**
```csharp
RequiredFieldHelper.MarkRequiredFields(
    this, 
    typeof(CompanyDto),
    logger: (msg, ex) => _logger?.LogError(msg, ex)
);
```

**Kết quả:**
- `CompanyCodeTextEdit` → Caption: "Mã công ty <color=red>*</color>"
- `CompanyNameTextEdit` → Caption: "Tên công ty <color=red>*</color>"
- `CreatedDateDateEdit` → Caption: "Ngày tạo <color=red>*</color>"

### 4.3. DXErrorProvider

**Chức năng:**
- Hiển thị biểu tượng cảnh báo màu đỏ bên cạnh controls có lỗi
- Tooltip hiển thị thông báo lỗi khi di chuột qua

**Cấu hình:**
```csharp
dxErrorProvider1.ContainerControl = this;
```

### 4.4. Validation Flow

```
User Input
  │
  ├─> DataAnnotations Check (DTO level)
  │   ├─> [Required] → Check empty
  │   ├─> [StringLength] → Check max length
  │   └─> [EmailAddress] → Check email format
  │
  ├─> DXErrorProvider Display
  │   └─> Show error icon + tooltip
  │
  └─> RequiredFieldHelper
      └─> Mark required fields with red *
```

### 4.5. Custom Validation

**Hiện tại không có custom validation logic** trong UcCompany. Tất cả validation dựa trên DataAnnotations trong CompanyDto.

---

## 5. Business Logic Flow

### 5.1. Load Flow (Sequence Diagram)

```
User                    UcCompany              CompanyBll          CompanyDataAccess    Database
 │                          │                       │                       │              │
 │  ──Load Event────────>     │                       │                       │              │
 │                          │                       │                       │              │
 │                          │──EnsureSingleCompany()─>│                       │              │
 │                          │                       │──EnsureDefaultCompany()─>│            │
 │                          │                       │                       │──Query──────>│
 │                          │                       │                       │<─Company─────│
 │                          │                       │<──OK───────────────────│              │
 │                          │<──OK──────────────────│                       │              │
 │                          │                       │                       │              │
 │                          │──GetCompany()────────>│                       │              │
 │                          │                       │──GetCompany()────────>│              │
 │                          │                       │                       │──Query──────>│
 │                          │                       │                       │<─Company─────│
 │                          │                       │<──Company──────────────│              │
 │                          │<──Company─────────────│                       │              │
 │                          │                       │                       │              │
 │                          │──ToDto()──────────────│                       │              │
 │                          │  (CompanyConverter)    │                       │              │
 │                          │                       │                       │              │
 │                          │──DisplayCompanyInfo() │                       │              │
 │                          │  (Fill UI controls)   │                       │              │
 │                          │                       │                       │              │
 │<───UI Displayed──────────│                       │                       │              │
```

### 5.2. Logo Load Flow

```
User                    UcCompany              CompanyBll          CompanyDataAccess    Database
 │                          │                       │                       │              │
 │  ──Right Click────────> │                       │                       │              │
 │  ──Select "Load..."     │                       │                       │              │
 │                          │                       │                       │              │
 │                          │──OpenFileDialog()─────>│                       │              │
 │<───File Dialog───────────│                       │                       │              │
 │  ──Select Image────────>│                       │                       │              │
 │                          │                       │                       │              │
 │                          │──File.ReadAllBytes()  │                       │              │
 │                          │──Image.FromFile()     │                       │              │
 │                          │──LogoPictureEdit.Image│                       │              │
 │                          │                       │                       │              │
 │                          │──SaveLogoToDatabase()─>│                       │              │
 │                          │                       │──GetCompany()────────>│              │
 │                          │                       │                       │──Query──────>│
 │                          │                       │                       │<─Company─────│
 │                          │                       │<──Company──────────────│              │
 │                          │                       │                       │              │
 │                          │                       │──UpdateCompany()──────>│              │
 │                          │                       │                       │──Update──────>│
 │                          │                       │                       │<──OK──────────│
 │                          │                       │<──OK───────────────────│              │
 │                          │<──OK──────────────────│                       │              │
 │                          │                       │                       │              │
 │<───Success Message───────│                       │                       │              │
```

### 5.3. Logo Delete Flow

```
User                    UcCompany              CompanyBll          CompanyDataAccess    Database
 │                          │                       │                       │              │
 │  ──Right Click────────> │                       │                       │              │
 │  ──Select "Delete"      │                       │                       │              │
 │                          │                       │                       │              │
 │                          │──XtraMessageBox.Show()│                       │              │
 │<───Confirm Dialog────────│                       │                       │              │
 │  ──Click "Yes"─────────>│                       │                       │              │
 │                          │                       │                       │              │
 │                          │──LogoPictureEdit.Image│                       │              │
 │                          │    = null             │                       │              │
 │                          │                       │                       │              │
 │                          │──DeleteLogoFromDatabase()─>│                       │              │
 │                          │                       │──GetCompany()────────>│              │
 │                          │                       │                       │──Query──────>│
 │                          │                       │                       │<─Company─────│
 │                          │                       │<──Company──────────────│              │
 │                          │                       │                       │              │
 │                          │                       │──UpdateCompany()──────>│              │
 │                          │                       │                       │──Update──────>│
 │                          │                       │                       │<──OK──────────│
 │                          │                       │<──OK───────────────────│              │
 │                          │<──OK──────────────────│                       │              │
 │                          │                       │                       │              │
 │<───Success Message───────│                       │                       │              │
```

### 5.4. EnsureSingleCompany Flow

```
UcCompany              CompanyBll          CompanyDataAccess    Database
 │                       │                       │              │
 │──EnsureSingleCompany()─>│                       │              │
 │                       │──EnsureDefaultCompany()─>│            │
 │                       │                       │              │
 │                       │                       │──Count()────>│
 │                       │                       │<──Count──────│
 │                       │                       │              │
 │                       │  If count == 0:       │              │
 │                       │    └─> Create default │              │
 │                       │        company        │              │
 │                       │                       │──Insert─────>│
 │                       │                       │<──OK──────────│
 │                       │                       │              │
 │                       │  If count > 1:        │              │
 │                       │    └─> Keep first,    │              │
 │                       │        delete others  │              │
 │                       │                       │──Delete─────>│
 │                       │                       │<──OK──────────│
 │                       │                       │              │
 │<───OK──────────────────│                       │              │
```

---

## 6. Error Handling

### 6.1. Try-Catch Blocks

#### 6.1.1. UcCompany_Load

```csharp
try
{
    _companyBll.EnsureSingleCompany();
    MarkRequiredFields(typeof(CompanyDto));
    ConfigureLogoPictureEdit();
    DisplayCompanyInfo();
    SetupSuperTips();
}
catch (Exception ex)
{
    _logger?.LogError($"Lỗi khi load UcCompany: {ex.Message}", ex);
    XtraMessageBox.Show($"Lỗi khi khởi tạo dữ liệu công ty: {ex.Message}", 
        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
}
```

#### 6.1.2. DisplayCompanyInfo

```csharp
try
{
    // ... display logic
}
catch (Exception ex)
{
    _logger?.LogError($"Lỗi khi hiển thị thông tin công ty: {ex.Message}", ex);
    XtraMessageBox.Show($"Lỗi khi hiển thị thông tin công ty: {ex.Message}", 
        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
}
```

#### 6.1.3. LoadLogo_Click

```csharp
try
{
    // ... load logo logic
}
catch (Exception ex)
{
    _logger?.LogError($"Lỗi khi load logo: {ex.Message}", ex);
    XtraMessageBox.Show($"Lỗi khi load logo: {ex.Message}", 
        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
}
```

#### 6.1.4. DeleteLogo_Click

```csharp
try
{
    // ... delete logo logic
}
catch (Exception ex)
{
    _logger?.LogError($"Lỗi khi xóa logo: {ex.Message}", ex);
    XtraMessageBox.Show($"Lỗi khi xóa logo: {ex.Message}", 
        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
}
```

#### 6.1.5. LogoPictureEdit_DragDrop

```csharp
try
{
    // ... drag & drop logic
}
catch (Exception ex)
{
    _logger?.LogError($"Lỗi khi drag & drop logo: {ex.Message}", ex);
    XtraMessageBox.Show($"Lỗi khi load logo: {ex.Message}", 
        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
}
```

### 6.2. Logging

**Logger Interface**: `Dal.Logging.ILogger`

**Logger Implementation**: `ConsoleLogger` (default)

**Log Levels:**
- `LogInfo()` - Thông tin thường
- `LogWarning()` - Cảnh báo
- `LogError()` - Lỗi (với exception)

**Ví dụ:**
```csharp
_logger?.LogInfo("UcCompany đang load - đảm bảo chỉ có 1 công ty trong database");
_logger?.LogError($"Lỗi khi load UcCompany: {ex.Message}", ex);
```

### 6.3. User-Friendly Error Messages

**XtraMessageBox được sử dụng để hiển thị lỗi:**

| Loại | Icon | Ví dụ |
|------|------|-------|
| Error | `MessageBoxIcon.Error` | "Lỗi khi load logo: {ex.Message}" |
| Warning | `MessageBoxIcon.Warning` | "Không tìm thấy thông tin công ty trong database" |
| Information | `MessageBoxIcon.Information` | "Đã load logo thành công!" |
| Question | `MessageBoxIcon.Question` | "Bạn có chắc chắn muốn xóa logo?" |

---

## 7. Security & Best Practices

### 7.1. Security

**Hiện tại không có thông tin nhạy cảm** được xử lý trong UcCompany:

- Không có password
- Không có Remember Me
- Không có authentication/authorization logic
- Logo được lưu dưới dạng binary trong database

### 7.2. Best Practices

#### 7.2.1. Logging

✅ **Đã áp dụng:**
- Sử dụng ILogger interface
- Log tất cả exceptions
- Log các thao tác quan trọng (load, save, delete)

#### 7.2.2. Error Handling

✅ **Đã áp dụng:**
- Try-catch cho tất cả operations
- User-friendly error messages
- Logging errors

#### 7.2.3. Separation of Concerns

✅ **Đã áp dụng:**
- UI Layer (UcCompany) → BLL Layer (CompanyBll) → DAL Layer (CompanyDataAccess)
- DTO pattern (CompanyDto)
- Converter pattern (CompanyConverter)

#### 7.2.4. Code Organization

✅ **Đã áp dụng:**
- Regions để tổ chức code
- XML documentation comments
- Meaningful method names

### 7.3. Gợi Ý Cải Thiện

#### 7.3.1. Save Button Implementation

⚠️ **Vấn đề**: `SaveBarButtonItem` chưa có event handler

**Gợi ý:**
```csharp
private void SaveBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
{
    try
    {
        // Validate all fields
        if (!ValidateAllFields())
            return;

        // Get current company
        var company = _companyBll.GetCompany() as Company;
        if (company == null)
        {
            XtraMessageBox.Show("Không tìm thấy thông tin công ty", 
                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        // Update company from UI
        company.CompanyCode = CompanyCodeTextEdit.Text?.Trim();
        company.CompanyName = CompanyNameTextEdit.Text?.Trim();
        company.TaxCode = TaxCodeTextEdit.Text?.Trim();
        company.Phone = PhoneTextEdit.Text?.Trim();
        company.Email = EmailTextEdit.Text?.Trim();
        company.Website = WebsiteTextEdit.Text?.Trim();
        company.Address = AddressTextEdit.Text?.Trim();
        company.Country = CountryTextEdit.Text?.Trim();
        company.UpdatedDate = DateTime.Now;

        // Save to database
        _companyBll.UpdateCompany(company);

        XtraMessageBox.Show("Đã lưu thông tin công ty thành công!", 
            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
    catch (Exception ex)
    {
        _logger?.LogError($"Lỗi khi lưu thông tin công ty: {ex.Message}", ex);
        XtraMessageBox.Show($"Lỗi khi lưu thông tin công ty: {ex.Message}", 
            "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
```

#### 7.3.2. Validation Helper

**Gợi ý tạo ValidationHelper:**
```csharp
private bool ValidateAllFields()
{
    var errors = new List<string>();

    // Validate required fields
    if (string.IsNullOrWhiteSpace(CompanyCodeTextEdit.Text))
        errors.Add("Mã công ty không được để trống");

    if (string.IsNullOrWhiteSpace(CompanyNameTextEdit.Text))
        errors.Add("Tên công ty không được để trống");

    // Validate email format
    if (!string.IsNullOrWhiteSpace(EmailTextEdit.Text))
    {
        if (!IsValidEmail(EmailTextEdit.Text))
            errors.Add("Email không đúng định dạng");
    }

    // Display errors
    if (errors.Any())
    {
        XtraMessageBox.Show(string.Join("\n", errors), 
            "Lỗi Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return false;
    }

    return true;
}
```

#### 7.3.3. Async/Await

**Gợi ý sử dụng async cho các operations:**
```csharp
private async Task LoadCompanyInfoAsync()
{
    try
    {
        await Task.Run(() => _companyBll.EnsureSingleCompany());
        var company = await Task.Run(() => _companyBll.GetCompany() as Company);
        // ... display logic
    }
    catch (Exception ex)
    {
        // ... error handling
    }
}
```

---

## 8. Extensibility Guide

### 8.1. Cách Mở Rộng Form

#### 8.1.1. Thêm Trường Mới

**Bước 1**: Thêm property vào `CompanyDto.cs`
```csharp
[DisplayName("Fax")]
[StringLength(50, ErrorMessage = "Fax không được vượt quá 50 ký tự")]
public string Fax { get; set; }
```

**Bước 2**: Thêm control vào `UcCompany.Designer.cs`
```csharp
private TextEdit FaxTextEdit;
```

**Bước 3**: Thêm vào `DisplayCompanyInfo()`
```csharp
FaxTextEdit.Text = companyDto.Fax ?? "";
```

**Bước 4**: Thêm vào `SaveBarButtonItem_ItemClick()` (khi implement)
```csharp
company.Fax = FaxTextEdit.Text?.Trim();
```

**Bước 5**: Thêm SuperToolTip trong `SetupTextEditSuperTips()`
```csharp
SuperToolTipHelper.SetTextEditSuperTip(
    FaxTextEdit,
    title: @"<b><color=DarkBlue>📠 Fax</color></b>",
    content: @"Nhập số fax của công ty (tùy chọn)..."
);
```

#### 8.1.2. Thêm Validation Rule Mới

**Ví dụ**: Validate phone number format

```csharp
private bool ValidatePhoneNumber(string phone)
{
    if (string.IsNullOrWhiteSpace(phone))
        return true; // Optional field

    // Vietnamese phone number format
    var pattern = @"^(0|\+84)[0-9]{9,10}$";
    return System.Text.RegularExpressions.Regex.IsMatch(phone, pattern);
}
```

#### 8.1.3. Thêm Chức Năng Export Logo

```csharp
private void ExportLogo_Click(object sender, EventArgs e)
{
    try
    {
        if (LogoPictureEdit.Image == null)
        {
            XtraMessageBox.Show("Không có logo để export", 
                "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        using (var saveDialog = new SaveFileDialog())
        {
            saveDialog.Filter = "Image Files|*.jpg;*.png;*.bmp|All Files|*.*";
            saveDialog.Title = "Export Logo";
            
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                LogoPictureEdit.Image.Save(saveDialog.FileName);
                XtraMessageBox.Show("Đã export logo thành công!", 
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
    catch (Exception ex)
    {
        _logger?.LogError($"Lỗi khi export logo: {ex.Message}", ex);
        XtraMessageBox.Show($"Lỗi khi export logo: {ex.Message}", 
            "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
```

### 8.2. Clean Code Patterns

#### 8.2.1. Repository Pattern

**Hiện tại**: Sử dụng trực tiếp `CompanyDataAccess`

**Gợi ý**: Tạo interface `ICompanyRepository`:
```csharp
public interface ICompanyRepository
{
    Company GetCompany();
    void UpdateCompany(Company company);
    void EnsureDefaultCompany();
}
```

#### 8.2.2. Dependency Injection

**Gợi ý**: Sử dụng DI container (ví dụ: Autofac, Unity):
```csharp
public UcCompany(ICompanyBll companyBll, ILogger logger)
{
    InitializeComponent();
    _companyBll = companyBll;
    _logger = logger;
    Load += UcCompany_Load;
}
```

#### 8.2.3. Command Pattern

**Gợi ý**: Tách logic thành commands:
```csharp
public interface ICommand
{
    void Execute();
    bool CanExecute();
}

public class SaveCompanyCommand : ICommand
{
    private readonly UcCompany _ucCompany;
    
    public void Execute()
    {
        // Save logic
    }
}
```

### 8.3. Async Patterns

**Gợi ý**: Sử dụng async/await cho các operations:
```csharp
private async Task LoadCompanyInfoAsync()
{
    try
    {
        await _companyBll.EnsureSingleCompanyAsync();
        var company = await _companyBll.GetCompanyAsync();
        // ... display logic
    }
    catch (Exception ex)
    {
        // ... error handling
    }
}
```

---

## 9. Test Checklist

### 9.1. Unit Test Cases

#### 9.1.1. Load Tests

- [ ] Test `UcCompany_Load` với company tồn tại
- [ ] Test `UcCompany_Load` với company không tồn tại (tự động tạo)
- [ ] Test `UcCompany_Load` với nhiều companies (tự động xóa bớt)
- [ ] Test `DisplayCompanyInfo` với company có đầy đủ thông tin
- [ ] Test `DisplayCompanyInfo` với company có logo
- [ ] Test `DisplayCompanyInfo` với company không có logo

#### 9.1.2. Validation Tests

- [ ] Test `CompanyCodeTextEdit` với giá trị rỗng (required)
- [ ] Test `CompanyCodeTextEdit` với giá trị > 50 ký tự
- [ ] Test `CompanyNameTextEdit` với giá trị rỗng (required)
- [ ] Test `CompanyNameTextEdit` với giá trị > 255 ký tự
- [ ] Test `EmailTextEdit` với email không đúng định dạng
- [ ] Test `EmailTextEdit` với email > 100 ký tự
- [ ] Test `MarkRequiredFields` đánh dấu đúng các trường required

#### 9.1.3. Logo Tests

- [ ] Test `LoadLogo_Click` với file hợp lệ (JPG, PNG, BMP, GIF)
- [ ] Test `LoadLogo_Click` với file không hợp lệ
- [ ] Test `DeleteLogo_Click` với logo tồn tại
- [ ] Test `DeleteLogo_Click` với logo không tồn tại
- [ ] Test `LogoPictureEdit_DragDrop` với file hợp lệ
- [ ] Test `LogoPictureEdit_DragDrop` với file không hợp lệ
- [ ] Test `SaveLogoToDatabase` lưu thành công
- [ ] Test `DeleteLogoFromDatabase` xóa thành công

#### 9.1.4. Error Handling Tests

- [ ] Test exception trong `UcCompany_Load`
- [ ] Test exception trong `DisplayCompanyInfo`
- [ ] Test exception trong `LoadLogo_Click`
- [ ] Test exception trong `DeleteLogo_Click`
- [ ] Test exception trong `SaveLogoToDatabase`
- [ ] Test exception trong `DeleteLogoFromDatabase`

### 9.2. Manual Testing Scenarios

#### 9.2.1. Scenario 1: Load Form Lần Đầu

**Steps:**
1. Mở form UcCompany
2. Kiểm tra form load thành công
3. Kiểm tra các trường required có dấu * đỏ
4. Kiểm tra thông tin công ty được hiển thị (nếu có)

**Expected:**
- Form load không lỗi
- Các trường required có dấu * đỏ
- Thông tin công ty hiển thị đúng

#### 9.2.2. Scenario 2: Load Logo

**Steps:**
1. Click chuột phải vào vùng logo
2. Chọn "Load..."
3. Chọn file hình ảnh (JPG)
4. Kiểm tra logo hiển thị
5. Refresh form, kiểm tra logo vẫn còn

**Expected:**
- Logo hiển thị đúng
- Logo được lưu vào database
- Logo vẫn còn sau khi refresh

#### 9.2.3. Scenario 3: Delete Logo

**Steps:**
1. Load logo (nếu chưa có)
2. Click chuột phải vào vùng logo
3. Chọn "Delete"
4. Xác nhận xóa
5. Kiểm tra logo bị xóa
6. Refresh form, kiểm tra logo vẫn bị xóa

**Expected:**
- Logo bị xóa khỏi UI
- Logo bị xóa khỏi database
- Logo vẫn bị xóa sau khi refresh

#### 9.2.4. Scenario 4: Drag & Drop Logo

**Steps:**
1. Kéo thả file hình ảnh (PNG) vào vùng logo
2. Kiểm tra logo hiển thị
3. Refresh form, kiểm tra logo vẫn còn

**Expected:**
- Logo hiển thị đúng
- Logo được lưu vào database
- Logo vẫn còn sau khi refresh

#### 9.2.5. Scenario 5: Validation Errors

**Steps:**
1. Xóa nội dung trường "Mã công ty"
2. Rời khỏi trường
3. Kiểm tra hiển thị lỗi
4. Nhập email không đúng định dạng
5. Rời khỏi trường
6. Kiểm tra hiển thị lỗi

**Expected:**
- Lỗi hiển thị qua DXErrorProvider
- Tooltip hiển thị thông báo lỗi

---

## 10. Changelog Template

### 10.1. Format

```markdown
## [Version] - YYYY-MM-DD

### Added
- Tính năng mới 1
- Tính năng mới 2

### Changed
- Thay đổi 1
- Thay đổi 2

### Fixed
- Sửa lỗi 1
- Sửa lỗi 2

### Removed
- Xóa tính năng 1
- Xóa tính năng 2
```

### 10.2. Example

```markdown
## [1.0.0] - 2025-01-XX

### Added
- User Control UcCompany để quản lý thông tin công ty
- Chức năng load/delete logo qua context menu
- Chức năng drag & drop logo
- Đánh dấu trường bắt buộc với dấu * đỏ
- SuperToolTip cho tất cả controls
- Validation dựa trên DataAnnotations
- Tự động đảm bảo chỉ có 1 công ty trong database

### Changed
- (Chưa có)

### Fixed
- (Chưa có)

### Removed
- (Chưa có)

### Known Issues
- Nút "Lưu" chưa có event handler để lưu các trường thông tin (ngoài logo)
```

---

## 11. Additional Notes

### 11.1. Missing Features

⚠️ **Nút "Lưu" chưa có chức năng:**
- `SaveBarButtonItem` tồn tại trong Designer nhưng chưa có event handler `ItemClick`
- Logo được lưu tự động khi load/delete, nhưng các trường thông tin khác chưa có chức năng lưu

### 11.2. Future Enhancements

- [ ] Implement Save button functionality
- [ ] Add validation helper methods
- [ ] Add async/await support
- [ ] Add export logo functionality
- [ ] Add image preview/zoom functionality
- [ ] Add undo/redo support
- [ ] Add change tracking (dirty flag)

---

**Tài liệu này được tạo tự động từ source code. Cập nhật lần cuối: 2025-01-XX**

