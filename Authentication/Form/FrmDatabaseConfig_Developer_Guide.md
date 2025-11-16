# FrmDatabaseConfig - Hướng dẫn cho Lập trình viên

## 1. Tổng quan

### Vai trò của form trong module

`FrmDatabaseConfig` là form cấu hình kết nối cơ sở dữ liệu cho hệ thống VNS ERP 2025. Form này đóng vai trò quan trọng trong việc:

- 🔧 **Cấu hình ban đầu:** Cho phép người dùng thiết lập thông tin kết nối database lần đầu
- 🔄 **Thay đổi cấu hình:** Cho phép cập nhật thông tin kết nối khi cần thiết
- ✅ **Kiểm tra kết nối:** Test connection trước khi lưu để đảm bảo cấu hình hợp lệ
- 💾 **Lưu trữ an toàn:** Mã hóa và lưu thông tin nhạy cảm (password) vào User Settings

**File:** `Authentication/Form/FrmDatabaseConfig.cs`  
**Namespace:** `Authentication.Form`  
**Base Class:** `DevExpress.XtraEditors.XtraForm`

### File structure

Form bao gồm các file sau:

```
Authentication/Form/
├── FrmDatabaseConfig.cs              # Main form code (295 lines)
├── FrmDatabaseConfig.Designer.cs     # Designer-generated code (409 lines)
└── FrmDatabaseConfig.resx            # Form resources (icons, strings)
```

**Dependencies:**
- `Dal.Connection.DatabaseConfig` - Configuration class (singleton)
- `Dal.Connection.ConnectionManager` - Connection management
- `Dal.Connection.ConnectionStringHelper` - Connection string utilities
- `Bll.Utils.MsgBox` - Message box helper
- `DevExpress.XtraEditors` - DevExpress controls
- `DevExpress.XtraDataLayout` - Data layout control
- `DevExpress.XtraEditors.DXErrorProvider` - Error provider

---

## 2. Kiến trúc

### 2.1. Architecture Diagram

```
┌─────────────────────────────────────────────────────────┐
│                    UI Layer                              │
│  ┌───────────────────────────────────────────────────┐  │
│  │  FrmDatabaseConfig                                │  │
│  │  - TextEdit controls (Server, DB, User, Pass)   │  │
│  │  - DataLayoutControl (layout)                     │  │
│  │  - DXErrorProvider (validation)                   │  │
│  │  - BindingSource (data binding)                   │  │
│  └───────────────────────────────────────────────────┘  │
└───────────────────────┬─────────────────────────────────┘
                        │
                        ▼
┌─────────────────────────────────────────────────────────┐
│              Configuration Layer                        │
│  ┌──────────────────┐  ┌──────────────────────────┐   │
│  │ DatabaseConfig   │  │ Properties.Settings       │   │
│  │ (Singleton)       │  │ (User Settings)          │   │
│  └──────────────────┘  └──────────────────────────┘   │
└───────────────────────┬─────────────────────────────────┘
                        │
                        ▼
┌─────────────────────────────────────────────────────────┐
│              Connection Layer                            │
│  ┌──────────────────┐  ┌──────────────────────────┐   │
│  │ ConnectionManager│  │ ConnectionStringHelper  │   │
│  │ - TestConnection │  │ - BuildConnectionString │   │
│  │ - SetConnection   │  │ - Encode/Decode         │   │
│  └──────────────────┘  └──────────────────────────┘   │
└───────────────────────┬─────────────────────────────────┘
                        │
                        ▼
┌─────────────────────────────────────────────────────────┐
│                    DAL Layer                             │
│  ┌───────────────────────────────────────────────────┐  │
│  │  SQL Server Database                              │  │
│  └───────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────┘
```

### 2.2. Dependencies

#### DevExpress Components:
- `DevExpress.XtraEditors.XtraForm` - Base form
- `DevExpress.XtraDataLayout.DataLayoutControl` - Layout control
- `DevExpress.XtraEditors.TextEdit` - Text input controls
- `DevExpress.XtraEditors.SimpleButton` - Buttons
- `DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider` - Error provider
- `DevExpress.XtraLayout` - Layout items

#### .NET Framework:
- `System.Windows.Forms.BindingSource` - Data binding
- `System.Configuration` - Configuration management
- `Properties.Settings` - User settings

#### Internal Dependencies:
- `Dal.Connection.DatabaseConfig` - Configuration singleton
- `Dal.Connection.ConnectionManager` - Connection management
- `Dal.Connection.ConnectionStringHelper` - Connection string utilities
- `Bll.Utils.MsgBox` - Message display

---

## 3. Chi tiết kỹ thuật

### 3.1. Controls trong Designer

#### DataLayoutControl

```csharp
private DevExpress.XtraDataLayout.DataLayoutControl dataLayoutControl1;
```

- **Chức năng:** Container chính cho tất cả controls
- **Dock:** Fill
- **DataSource:** `databaseConfigBindingSource`
- **Layout Mode:** Table layout với 4 cột, 6 hàng

#### TextEdit Controls

| Control | Property | Binding | TabIndex |
|---------|----------|---------|----------|
| `ServerNameTextEdit` | EditValue | `DatabaseConfig.ServerName` | 0 |
| `DatabaseNameTextEdit` | EditValue | `DatabaseConfig.DatabaseName` | 2 |
| `UserIdTextEdit` | EditValue | `DatabaseConfig.UserId` | 3 |
| `PasswordTextEdit` | EditValue | `DatabaseConfig.Password` | 4 |
| | PasswordChar | `*` | |
| | UseSystemPasswordChar | `true` | |

#### Buttons

| Control | Text | Icon | TabIndex | Event Handler |
|---------|------|------|----------|---------------|
| `OKSmpleButton` | "Cập nhật" | `apply_16x16` | 5 | `OKSmpleButton_Click` |
| `CancelSimpleButton` | "Hủy" | `cancel_16x16` | 6 | `CancelSimpleButton_Click` |

#### Error Provider

```csharp
private DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider dxErrorProvider1;
```

- **ContainerControl:** Form
- **Chức năng:** Hiển thị lỗi validation cho từng control

#### BindingSource

```csharp
private System.Windows.Forms.BindingSource databaseConfigBindingSource;
```

- **DataSource Type:** `Dal.Connection.DatabaseConfig`
- **Chức năng:** Binding data giữa form và DatabaseConfig object

### 3.2. Event Handlers

#### OKSmpleButton_Click

```csharp
private void OKSmpleButton_Click(object sender, EventArgs e)
```

**Luồng xử lý:**
1. Validate dữ liệu bằng `KiemTraDuLieuHopLeBangValidationProvider()`
2. Cập nhật thông tin từ form vào `DatabaseConfig` bằng `CapNhatThongTinTuForm()`
3. Kiểm tra kết nối bằng `KiemTraKetNoi()`
4. Nếu thành công:
   - Lưu cấu hình bằng `LuuCauHinh()`
   - Hiển thị thông báo thành công
   - Đóng form với `DialogResult.OK`
5. Nếu thất bại:
   - Hiển thị thông báo lỗi
   - Giữ form mở

#### CancelSimpleButton_Click

```csharp
private void CancelSimpleButton_Click(object sender, EventArgs e)
```

**Luồng xử lý:**
1. Đóng form với `DialogResult.Cancel`
2. Không lưu thay đổi

### 3.3. Methods

#### Public Methods

Không có public methods. Form chỉ được sử dụng như dialog.

#### Private Methods

##### KhoiTaoForm()

```csharp
private void KhoiTaoForm()
```

**Chức năng:** Khởi tạo form và load dữ liệu

**Luồng:**
1. Khởi tạo `DatabaseConfig.Instance` (singleton)
2. Tải dữ liệu từ Settings bằng `TaiDuLieuTuSettings()`
3. Hiển thị thông tin hiện tại bằng `HienThiThongTinHienTai()`
4. Khởi tạo `ConnectionManager`

##### TaiDuLieuTuSettings()

```csharp
private void TaiDuLieuTuSettings()
```

**Chức năng:** Tải cấu hình từ User Settings

**Luồng:**
1. Lấy `Properties.Settings.Default`
2. Cập nhật `DatabaseConfig` từ Settings:
   - `ServerName` ← `DatabaseServer` (default: "localhost")
   - `DatabaseName` ← `DatabaseName` (default: "VnsErp2025")
   - `UserId` ← `DatabaseUserId` (default: empty)
   - `Password` ← `DatabasePassword` (decoded từ Base64)
   - `UseIntegratedSecurity` ← `false` (luôn SQL Auth)

**Lưu ý:** Password được decode từ Base64 trước khi sử dụng.

##### HienThiThongTinHienTai()

```csharp
private void HienThiThongTinHienTai()
```

**Chức năng:** Hiển thị thông tin từ DatabaseConfig lên form

**Luồng:**
1. Gán `DatabaseConfig` vào `BindingSource.DataSource`
2. Gán giá trị vào các TextEdit controls:
   - `ServerNameTextEdit.EditValue = DatabaseConfig.ServerName`
   - `DatabaseNameTextEdit.EditValue = DatabaseConfig.DatabaseName`
   - `UserIdTextEdit.EditValue = DatabaseConfig.UserId`
   - `PasswordTextEdit.EditValue = DatabaseConfig.Password`

##### KiemTraDuLieuHopLeBangValidationProvider()

```csharp
private bool KiemTraDuLieuHopLeBangValidationProvider()
```

**Chức năng:** Validate dữ liệu đầu vào bằng DXErrorProvider

**Validation Rules:**
1. `ServerNameTextEdit` - Không được để trống
2. `DatabaseNameTextEdit` - Không được để trống
3. `UserIdTextEdit` - Không được để trống
4. `PasswordTextEdit` - Không được để trống

**Luồng:**
1. Clear errors cũ: `dxErrorProvider1.ClearErrors()`
2. Kiểm tra từng field:
   - Nếu rỗng → Set error và focus vào control
   - Return `false`
3. Nếu tất cả hợp lệ → Return `true`

**Lưu ý:** Không sử dụng DXValidationProvider mà dùng DXErrorProvider thủ công.

##### CapNhatThongTinTuForm()

```csharp
private void CapNhatThongTinTuForm()
```

**Chức năng:** Cập nhật DatabaseConfig từ form inputs

**Luồng:**
1. Trim và gán giá trị:
   - `DatabaseConfig.ServerName = ServerNameTextEdit.Text.Trim()`
   - `DatabaseConfig.DatabaseName = DatabaseNameTextEdit.Text.Trim()`
   - `DatabaseConfig.UserId = UserIdTextEdit.Text.Trim()`
   - `DatabaseConfig.Password = PasswordTextEdit.Text` (không trim)
2. Set `UseIntegratedSecurity = false` (luôn SQL Auth)

**Lưu ý:** Password không được trim để giữ khoảng trắng hợp lệ.

##### KiemTraKetNoi()

```csharp
private bool KiemTraKetNoi()
```

**Chức năng:** Test kết nối đến database

**Luồng:**
1. Lấy thông tin từ form (trim nếu cần)
2. Build connection string bằng `ConnectionStringHelper.BuildDetailedConnectionString()`:
   - Server, Database, UserId, Password
   - `integratedSecurity = false`
   - `timeout = 15`, `commandTimeout = 30`
   - `pooling = true`, `minPoolSize = 1`, `maxPoolSize = 100`
3. Set connection string vào `ConnectionManager`
4. Test connection bằng `ConnectionManager.TestConnection()`
5. Return kết quả

**Test Connection Logic:**
- `ConnectionManager.TestConnection()` thực hiện: `SELECT GETDATE()`
- Timeout: 10 giây cho test query

##### LuuCauHinh()

```csharp
private void LuuCauHinh()
```

**Chức năng:** Lưu cấu hình vào Settings

**Luồng:**
1. Gọi `CapNhatAppConfig()` để lưu vào Settings
2. Có thể mở rộng để lưu vào file config khác

##### CapNhatAppConfig()

```csharp
private void CapNhatAppConfig()
```

**Chức năng:** Cập nhật User Settings với cấu hình mới

**Luồng:**
1. Lấy `Properties.Settings.Default`
2. Cập nhật các giá trị:
   - `DatabaseServer = DatabaseConfig.ServerName`
   - `DatabaseName = DatabaseConfig.DatabaseName`
   - `DatabaseUserId = DatabaseConfig.UserId`
   - `DatabasePassword = EncodeConnectionString(DatabaseConfig.Password)`
   - `UseIntegratedSecurity = DatabaseConfig.UseIntegratedSecurity`
3. Lưu Settings: `settings.Save()`

**Lưu ý:** Password được encode bằng Base64 trước khi lưu.

### 3.4. Data Flow Diagram

```
┌─────────────────────────────────────────────────────────────┐
│                    User Input                                │
│  - ServerNameTextEdit.Text                                  │
│  - DatabaseNameTextEdit.Text                                │
│  - UserIdTextEdit.Text                                      │
│  - PasswordTextEdit.Text                                    │
└───────────────────────┬─────────────────────────────────────┘
                        │
                        ▼
┌─────────────────────────────────────────────────────────────┐
│              Validation Layer                                │
│  KiemTraDuLieuHopLeBangValidationProvider()                 │
│  - Check empty fields                                       │
│  - DXErrorProvider.SetError() if invalid                    │
└───────────────────────┬─────────────────────────────────────┘
                        │
                  ┌─────┴─────┐
                  │ Valid?    │
                  └──┬────┬───┘
                No   │    │   Yes
                     │    │
                     ▼    ▼
            ┌─────────┐  ┌──────────────────────────────┐
            │ Return  │  │ CapNhatThongTinTuForm()      │
            │         │  │ - Update DatabaseConfig      │
            └─────────┘  └──────────┬───────────────────┘
                                    │
                                    ▼
                    ┌───────────────────────────────┐
                    │  KiemTraKetNoi()               │
                    │  - Build connection string     │
                    │  - ConnectionManager.Test()   │
                    └──────────┬──────────────────────┘
                               │
                       ┌───────┴────────┐
                       │               │
                  Success          Failed
                       │               │
                       ▼               ▼
            ┌──────────────────┐  ┌──────────────┐
            │ LuuCauHinh()     │  │ Show Error   │
            │ - CapNhatAppConfig│  │ Message     │
            │ - Encode password │  └──────────────┘
            │ - Save Settings   │
            └──────────┬─────────┘
                       │
                       ▼
            ┌──────────────────┐
            │ Show Success     │
            │ Close Form (OK)  │
            └──────────────────┘
```

---

## 4. Validation System

### 4.1. DXErrorProvider

Form sử dụng **DXErrorProvider** (không phải DXValidationProvider) để hiển thị lỗi validation.

**Cách hoạt động:**
- Validation được thực hiện thủ công trong `KiemTraDuLieuHopLeBangValidationProvider()`
- Sử dụng `dxErrorProvider1.SetError()` để hiển thị lỗi
- Sử dụng `dxErrorProvider1.ClearErrors()` để xóa lỗi cũ

### 4.2. Validation Rules

| Control | Rule | Error Message | Error Type |
|---------|------|---------------|------------|
| `ServerNameTextEdit` | Không được để trống | "Tên máy chủ không được để trống" | - |
| `DatabaseNameTextEdit` | Không được để trống | "Tên cơ sở dữ liệu không được để trống" | - |
| `UserIdTextEdit` | Không được để trống | "Tên đăng nhập không được để trống" | - |
| `PasswordTextEdit` | Không được để trống | "Mật khẩu không được để trống" | - |

**Lưu ý:** 
- Validation chỉ kiểm tra empty, không kiểm tra format
- Không có validation cho độ dài, ký tự đặc biệt, etc.
- Có thể cải thiện bằng cách thêm validation rules phức tạp hơn

### 4.3. Validation Flow

```
User clicks "Cập nhật"
    │
    ▼
KiemTraDuLieuHopLeBangValidationProvider()
    │
    ├─► Clear old errors
    │
    ├─► Check ServerName → Empty? → SetError + Focus + Return false
    │
    ├─► Check DatabaseName → Empty? → SetError + Focus + Return false
    │
    ├─► Check UserId → Empty? → SetError + Focus + Return false
    │
    ├─► Check Password → Empty? → SetError + Focus + Return false
    │
    └─► All valid → Return true
```

### 4.4. Gợi ý cải thiện Validation

**Có thể thêm:**
- ✅ Kiểm tra độ dài tối thiểu/tối đa
- ✅ Kiểm tra format (IP address, server name)
- ✅ Kiểm tra ký tự không hợp lệ
- ✅ Sử dụng DXValidationProvider với custom rules
- ✅ Real-time validation (Validating event)

---

## 5. Business Logic Flow

### 5.1. Authentication / Connection Flow

Form không xử lý authentication của user, mà xử lý **cấu hình kết nối database**.

**Sequence Diagram:**

```
User                    Form                    DatabaseConfig          ConnectionManager        SQL Server
 │                       │                            │                          │                    │
 │───Open Form──────────►│                            │                          │                    │
 │                       │───Get Instance───────────►│                          │                    │
 │                       │                            │                          │                    │
 │                       │───Load Settings───────────►│                          │                    │
 │                       │                            │                          │                    │
 │                       │───Display Data────────────►│                          │                    │
 │                       │                            │                          │                    │
 │◄───Show Form──────────│                            │                          │                    │
 │                       │                            │                          │                    │
 │───Enter Info─────────►│                            │                          │                    │
 │                       │                            │                          │                    │
 │───Click OK───────────►│                            │                          │                    │
 │                       │───Validate─────────────────►│                          │                    │
 │                       │◄───Valid───────────────────│                          │                    │
 │                       │                            │                          │                    │
 │                       │───Update Config────────────►│                          │                    │
 │                       │                            │                          │                    │
 │                       │───Build Connection String───│                          │                    │
 │                       │                            │                          │                    │
 │                       │                            │───Set Connection String─►│                    │
 │                       │                            │                          │                    │
 │                       │                            │───Test Connection─────────┼──────────────────►│
 │                       │                            │                          │◄───SELECT GETDATE()│
 │                       │                            │                          │                    │
 │                       │                            │◄───Success───────────────│                    │
 │                       │◄───Connection OK───────────│                          │                    │
 │                       │                            │                          │                    │
 │                       │───Save Config──────────────►│                          │                    │
 │                       │                            │                          │                    │
 │                       │───Encode Password──────────│                          │                    │
 │                       │                            │                          │                    │
 │                       │───Save Settings────────────►│                          │                    │
 │                       │                            │                          │                    │
 │◄───Success Message────│                            │                          │                    │
 │                       │                            │                          │                    │
 │◄───Close Form─────────│                            │                          │                    │
```

### 5.2. Connection String Building

**Connection String được build với các tham số:**

```csharp
ConnectionStringHelper.BuildDetailedConnectionString(
    server: ServerNameTextEdit.Text.Trim(),
    database: DatabaseNameTextEdit.Text.Trim(),
    integratedSecurity: false,  // Luôn SQL Auth
    userId: UserIdTextEdit.Text.Trim(),
    password: PasswordTextEdit.Text,  // Không trim
    timeout: 15,
    commandTimeout: 30,
    pooling: true,
    minPoolSize: 1,
    maxPoolSize: 100
);
```

**Kết quả:** Connection string dạng:
```
Data Source=ServerName;Initial Catalog=DatabaseName;User ID=UserId;Password=Password;Connect Timeout=15;Pooling=True;Min Pool Size=1;Max Pool Size=100;Enlist=True
```

### 5.3. Test Connection Logic

**ConnectionManager.TestConnection()** thực hiện:
1. Lấy connection từ `GetConnection()`
2. Tạo command: `SELECT GETDATE()`
3. Set timeout: 10 giây
4. Execute scalar
5. Return `true` nếu có kết quả, `false` nếu exception

---

## 6. Error Handling

### 6.1. Try-Catch Blocks

Form có try-catch ở các điểm quan trọng:

| Method | Exception Handling |
|--------|-------------------|
| `KhoiTaoForm()` | Catch all → `MsgBox.ShowException()` |
| `TaiDuLieuTuSettings()` | Catch all → `MsgBox.ShowException()` |
| `HienThiThongTinHienTai()` | Catch all → `MsgBox.ShowException()` |
| `OKSmpleButton_Click()` | Catch all → `MsgBox.ShowException()` |
| `KiemTraKetNoi()` | Catch all → `MsgBox.ShowException()` + Return false |
| `LuuCauHinh()` | Catch all → `MsgBox.ShowException()` + Throw |
| `CapNhatAppConfig()` | Catch all → Wrap và throw new Exception |

### 6.2. Error Messages

**User-facing messages:**
- "Lỗi khởi tạo form"
- "Lỗi tải dữ liệu từ Settings"
- "Lỗi hiển thị thông tin"
- "Lỗi xử lý"
- "Lỗi kiểm tra kết nối"
- "Lỗi lưu cấu hình"
- "Không thể kết nối đến cơ sở dữ liệu.\nVui lòng kiểm tra lại thông tin kết nối."
- "Kết nối cơ sở dữ liệu thành công!\nCấu hình đã được lưu."

### 6.3. Gợi ý cải thiện Error Handling

**Có thể thêm:**
- ✅ Logging chi tiết (file log, event log)
- ✅ Phân loại exception (SQLException, ConfigurationException, etc.)
- ✅ Retry logic cho connection test
- ✅ Hiển thị exception details cho admin (debug mode)
- ✅ User-friendly error messages cụ thể hơn

---

## 7. Security - Mã hóa Password

### 7.1. Password Encoding/Decoding

**Encoding (khi lưu):**
```csharp
settings.DatabasePassword = ConnectionStringHelper.EncodeConnectionString(
    _databaseConfig.Password
);
```

**Decoding (khi load):**
```csharp
_databaseConfig.Password = ConnectionStringHelper.DecodeConnectionString(
    settings.DatabasePassword ?? string.Empty
);
```

**Implementation:**
- **Encode:** Base64 encoding (UTF-8 bytes)
- **Decode:** Base64 decoding → UTF-8 string

### 7.2. Storage Location

**User Settings location:**
- `%LocalAppData%\YourApp\user.config`
- Per-user, per-application

**Stored values:**
- `DatabaseServer` - Plain text
- `DatabaseName` - Plain text
- `DatabaseUserId` - Plain text
- `DatabasePassword` - Base64 encoded
- `UseIntegratedSecurity` - Boolean

### 7.3. Security Considerations

**Hiện tại:**
- ✅ Password được encode (Base64)
- ✅ Lưu trong User Settings (per-user)
- ⚠️ Base64 không phải mã hóa mạnh (dễ decode)
- ⚠️ Không có encryption key

**Gợi ý cải thiện:**
- ✅ Sử dụng DPAPI (Data Protection API) để mã hóa
- ✅ Sử dụng Windows Credential Manager
- ✅ Sử dụng encryption key từ machine/user
- ✅ Không lưu password, chỉ lưu connection string đã mã hóa

**Ví dụ cải thiện với DPAPI:**
```csharp
using System.Security.Cryptography;

private void SavePassword(string password)
{
    byte[] encrypted = ProtectedData.Protect(
        Encoding.UTF8.GetBytes(password),
        null,  // Optional entropy
        DataProtectionScope.CurrentUser
    );
    settings.DatabasePassword = Convert.ToBase64String(encrypted);
}

private string LoadPassword()
{
    byte[] encrypted = Convert.FromBase64String(settings.DatabasePassword);
    byte[] decrypted = ProtectedData.Unprotect(
        encrypted,
        null,
        DataProtectionScope.CurrentUser
    );
    return Encoding.UTF8.GetString(decrypted);
}
```

---

## 8. Hướng dẫn mở rộng

### 8.1. Thêm Validation Rules

**Ví dụ: Thêm validation cho Server Name format**

```csharp
private bool ValidateServerName(string serverName)
{
    // Kiểm tra format IP hoặc hostname
    if (string.IsNullOrEmpty(serverName))
        return false;
    
    // IP address pattern
    var ipPattern = @"^(\d{1,3}\.){3}\d{1,3}$";
    // Hostname pattern
    var hostnamePattern = @"^[a-zA-Z0-9][a-zA-Z0-9\-\.]*[a-zA-Z0-9]$";
    
    return Regex.IsMatch(serverName, ipPattern) || 
           Regex.IsMatch(serverName, hostnamePattern);
}
```

### 8.2. Thêm Windows Authentication Support

**Hiện tại:** Form chỉ hỗ trợ SQL Authentication.

**Cải thiện:**
```csharp
// Thêm checkbox
private CheckBox UseWindowsAuthCheckBox;

// Trong CapNhatThongTinTuForm()
_databaseConfig.UseIntegratedSecurity = UseWindowsAuthCheckBox.Checked;

// Trong validation
if (!UseWindowsAuthCheckBox.Checked)
{
    // Validate UserId và Password
}
else
{
    // Skip UserId và Password validation
}
```

### 8.3. Thêm Port Configuration

**Thêm TextEdit cho Port:**

```csharp
private TextEdit PortTextEdit;

// Trong BuildDetailedConnectionString
var server = ServerNameTextEdit.Text.Trim();
var port = PortTextEdit.Text.Trim();
if (!string.IsNullOrEmpty(port))
{
    server = $"{server},{port}";
}
```

### 8.4. Thêm Connection String Preview

**Thêm TextEdit read-only để hiển thị connection string (ẩn password):**

```csharp
private TextEdit ConnectionStringPreviewTextEdit;

// Trong CapNhatThongTinTuForm()
var connectionString = ConnectionStringHelper.BuildDetailedConnectionString(...);
var safeConnectionString = ConnectionStringHelper.GetSafeConnectionString(connectionString);
ConnectionStringPreviewTextEdit.Text = safeConnectionString;
```

### 8.5. Async/Await Support

**Chuyển TestConnection sang async:**

```csharp
private async Task<bool> KiemTraKetNoiAsync()
{
    try
    {
        var connectionString = BuildConnectionString();
        _connectionManager.SetConnectionString(connectionString);
        
        // Async test
        return await Task.Run(() => _connectionManager.TestConnection());
    }
    catch (Exception ex)
    {
        MsgBox.ShowException(ex, "Lỗi kiểm tra kết nối");
        return false;
    }
}

private async void OKSmpleButton_Click(object sender, EventArgs e)
{
    // ... validation ...
    
    // Show progress
    OKSmpleButton.Enabled = false;
    OKSmpleButton.Text = "Đang kiểm tra...";
    
    var isConnected = await KiemTraKetNoiAsync();
    
    OKSmpleButton.Enabled = true;
    OKSmpleButton.Text = "Cập nhật";
    
    if (isConnected)
    {
        // ... save config ...
    }
}
```

### 8.6. Thêm Multiple Database Support

**Lưu nhiều cấu hình:**

```csharp
// Thêm ComboBox để chọn profile
private ComboBoxEdit ProfileComboBox;

// Lưu profiles vào Settings
private void SaveProfile(string profileName, DatabaseConfig config)
{
    var profiles = LoadProfiles();
    profiles[profileName] = config;
    SaveProfiles(profiles);
}

// Load profile khi chọn
private void ProfileComboBox_SelectedIndexChanged(object sender, EventArgs e)
{
    var profileName = ProfileComboBox.Text;
    var config = LoadProfile(profileName);
    LoadConfigToForm(config);
}
```

### 8.7. Clean Code Patterns

**Sử dụng Repository Pattern cho Settings:**

```csharp
public interface ISettingsRepository
{
    DatabaseConfig Load();
    void Save(DatabaseConfig config);
}

public class SettingsRepository : ISettingsRepository
{
    public DatabaseConfig Load()
    {
        var settings = Properties.Settings.Default;
        return new DatabaseConfig
        {
            ServerName = settings.DatabaseServer ?? "localhost",
            // ...
        };
    }
    
    public void Save(DatabaseConfig config)
    {
        var settings = Properties.Settings.Default;
        settings.DatabaseServer = config.ServerName;
        // ...
        settings.Save();
    }
}
```

**Sử dụng trong Form:**
```csharp
private readonly ISettingsRepository _settingsRepository;

public FrmDatabaseConfig(ISettingsRepository settingsRepository)
{
    _settingsRepository = settingsRepository;
    InitializeComponent();
    KhoiTaoForm();
}
```

---

## 9. Test Checklist

### 9.1. Unit Test Cases

**Test Cases cần cover:**

#### Test KiemTraDuLieuHopLeBangValidationProvider()

- [ ] Test với tất cả fields empty → Return false
- [ ] Test với ServerName empty → Return false, error set
- [ ] Test với DatabaseName empty → Return false, error set
- [ ] Test với UserId empty → Return false, error set
- [ ] Test với Password empty → Return false, error set
- [ ] Test với tất cả fields filled → Return true

#### Test CapNhatThongTinTuForm()

- [ ] Test cập nhật ServerName
- [ ] Test cập nhật DatabaseName
- [ ] Test cập nhật UserId
- [ ] Test cập nhật Password (không trim)
- [ ] Test UseIntegratedSecurity = false

#### Test TaiDuLieuTuSettings()

- [ ] Test load từ Settings có giá trị
- [ ] Test load từ Settings null → dùng default
- [ ] Test decode password từ Base64
- [ ] Test với Settings không tồn tại

#### Test CapNhatAppConfig()

- [ ] Test lưu tất cả giá trị
- [ ] Test encode password trước khi lưu
- [ ] Test Settings.Save() được gọi
- [ ] Test exception handling

#### Test KiemTraKetNoi()

- [ ] Test với connection string hợp lệ → Return true
- [ ] Test với connection string không hợp lệ → Return false
- [ ] Test với SQL Server không khả dụng → Return false
- [ ] Test exception handling

### 9.2. Manual Testing Scenarios

#### Scenario 1: Happy Path

1. Mở form
2. Nhập thông tin hợp lệ:
   - Server: `localhost`
   - Database: `VnsErp2025`
   - User: `sa`
   - Password: `password123`
3. Click "Cập nhật"
4. **Expected:** Thông báo thành công, form đóng

#### Scenario 2: Validation - Empty Fields

1. Mở form
2. Để trống Server Name
3. Click "Cập nhật"
4. **Expected:** Error message, focus vào Server Name

#### Scenario 3: Invalid Connection

1. Mở form
2. Nhập thông tin không hợp lệ:
   - Server: `InvalidServer`
   - Database: `InvalidDB`
   - User: `InvalidUser`
   - Password: `InvalidPass`
3. Click "Cập nhật"
4. **Expected:** Error message "Không thể kết nối", form vẫn mở

#### Scenario 4: Load Saved Config

1. Lưu cấu hình hợp lệ
2. Đóng form
3. Mở lại form
4. **Expected:** Thông tin đã lưu được hiển thị

#### Scenario 5: Cancel

1. Mở form
2. Thay đổi thông tin
3. Click "Hủy"
4. **Expected:** Form đóng, không lưu thay đổi

#### Scenario 6: Password Encoding/Decoding

1. Nhập password: `MyPassword123!@#`
2. Lưu cấu hình
3. Đóng và mở lại form
4. **Expected:** Password được decode và hiển thị đúng

#### Scenario 7: SQL Server Express

1. Nhập server: `localhost\SQLEXPRESS`
2. Nhập database hợp lệ
3. Nhập credentials hợp lệ
4. Click "Cập nhật"
5. **Expected:** Kết nối thành công (nếu SQLEXPRESS đang chạy)

#### Scenario 8: Remote SQL Server

1. Nhập server: `192.168.1.100` (IP remote)
2. Nhập database hợp lệ
3. Nhập credentials hợp lệ
4. Click "Cập nhật"
5. **Expected:** Kết nối thành công (nếu network và firewall cho phép)

---

## 10. Changelog Template

### Version 1.0 (Current)

**Date:** 2025

**Features:**
- ✅ Form cấu hình database connection
- ✅ Validation bằng DXErrorProvider
- ✅ Test connection trước khi lưu
- ✅ Lưu cấu hình vào User Settings
- ✅ Mã hóa password bằng Base64
- ✅ Load cấu hình đã lưu khi mở form
- ✅ SQL Authentication support

**Known Issues:**
- ⚠️ Base64 encoding không phải mã hóa mạnh
- ⚠️ Không hỗ trợ Windows Authentication
- ⚠️ Không hỗ trợ port configuration
- ⚠️ Validation chỉ kiểm tra empty, không kiểm tra format

**Future Improvements:**
- 🔄 DPAPI encryption cho password
- 🔄 Windows Authentication support
- 🔄 Port configuration
- 🔄 Connection string preview
- 🔄 Multiple database profiles
- 🔄 Async connection test
- 🔄 Enhanced validation rules

---

## Tài liệu tham khảo

### Related Files
- `Dal/Connection/DatabaseConfig.cs` - Configuration singleton
- `Dal/Connection/ConnectionManager.cs` - Connection management
- `Dal/Connection/ConnectionStringHelper.cs` - Connection string utilities
- `Bll/Utils/MsgBox.cs` - Message box helper

### DevExpress Documentation
- DataLayoutControl: https://docs.devexpress.com/WindowsForms/DevExpress.XtraDataLayout.DataLayoutControl
- DXErrorProvider: https://docs.devexpress.com/WindowsForms/DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider
- TextEdit: https://docs.devexpress.com/WindowsForms/DevExpress.XtraEditors.TextEdit

### .NET Documentation
- Properties.Settings: https://docs.microsoft.com/en-us/dotnet/api/system.configuration.applicationsettingsbase
- Data Protection API: https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.protecteddata

---

## Liên hệ và Hỗ trợ

Nếu có câu hỏi hoặc cần hỗ trợ:
- Team Lead hoặc Senior Developer
- Tạo issue trong hệ thống quản lý dự án
- Tham khảo code comments trong source files

