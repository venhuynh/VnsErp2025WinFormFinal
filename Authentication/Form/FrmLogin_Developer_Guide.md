# FrmLogin - Hướng dẫn cho Lập trình viên

## Tổng quan

`FrmLogin` là form đăng nhập hệ thống với hệ thống validation tự động sử dụng DevExpress DXValidationProvider. Form này xử lý toàn bộ quy trình đăng nhập, lưu trữ thông tin đăng nhập (Remember Me), và quản lý tài khoản người dùng.

**File:** `Authentication/Form/FrmLogin.cs`  
**Namespace:** `Authentication.Form`  
**Base Class:** `DevExpress.XtraEditors.XtraForm`

---

## Kiến trúc và Thiết kế

### 1. Kiến trúc phân lớp

Form tuân theo kiến trúc 3 lớp (3-tier architecture):

```
┌─────────────────────────────────┐
│   UI Layer (FrmLogin.cs)       │  ← Form, Validation Rules
├─────────────────────────────────┤
│   BLL Layer (LoginBll)          │  ← Business Logic
├─────────────────────────────────┤
│   DAL Layer (DataAccess)         │  ← Data Access
└─────────────────────────────────┘
```

### 2. Dependencies

#### BLL Dependencies:
- `Bll.Authentication.LoginBll` - Xử lý logic đăng nhập và xác thực
- `Bll.Validation.ValidationHelper` - Các phương thức validation tái sử dụng
- `Bll.Utils.ApplicationSystemUtils` - Quản lý thông tin user hiện tại
- `Bll.Utils.MsgBox` - Hiển thị thông báo

#### DevExpress Components:
- `DXValidationProvider` - Hệ thống validation tự động
- `XtraForm` - Base form class

#### .NET Framework:
- `Properties.Settings` - Lưu trữ thông tin Remember Me

---

## Luồng xử lý chính

### 1. Khởi tạo Form (Constructor)

```csharp
public FrmLogin()
{
    InitializeComponent();           // Khởi tạo UI controls
    InitializeValidation();          // Thiết lập validation rules
    SetupEventHandlers();            // Đăng ký event handlers
    LoadSavedCredentials();          // Tải thông tin đăng nhập đã lưu
    TaoTaiKhoanAdminPublic();        // Tạo tài khoản admin mặc định (nếu chưa có)
}
```

**Thứ tự quan trọng:**
1. `InitializeComponent()` phải được gọi đầu tiên
2. Validation setup trước event handlers
3. Load saved credentials sau khi controls đã sẵn sàng

### 2. Luồng đăng nhập (OkButton_Click)

```
┌─────────────────────────────────────┐
│  User nhấn nút "Đăng nhập"         │
└──────────────┬──────────────────────┘
               │
               ▼
┌─────────────────────────────────────┐
│  ValidateFormInput()                 │
│  - DXValidationProvider.Validate()  │
│  - PerformAdditionalValidation()    │
└──────────────┬──────────────────────┘
               │
        ┌──────┴──────┐
        │ Valid?      │
        └──┬──────┬───┘
      No   │      │   Yes
           │      │
           ▼      ▼
    ┌─────────┐  ┌──────────────────────┐
    │ Return  │  │ GetLoginCredentials() │
    └─────────┘  └──────────┬────────────┘
                            │
                            ▼
                 ┌──────────────────────────┐
                 │ LoginBll.XacThucDangNhap │
                 └──────────┬───────────────┘
                            │
                    ┌───────┴────────┐
                    │                │
              Success            Failed
                    │                │
                    ▼                ▼
         ┌──────────────────┐  ┌──────────────┐
         │ SetCurrentUser() │  │ Show Error   │
         │ HandleRememberMe │  │ Message      │
         │ ShowSuccess()    │  └──────────────┘
         │ CloseForm()      │
         └──────────────────┘
```

### 3. Validation Flow

Form sử dụng **DXValidationProvider** với chế độ **Manual**:

```csharp
dxValidationProvider1.ValidationMode = ValidationMode.Manual;
```

**Validation được kích hoạt khi:**
- User nhấn nút "Đăng nhập" → `ValidateFormInput()` → `dxValidationProvider1.Validate()`
- User rời khỏi control → `Validating` event (chỉ để clear error tự động)

**Validation Rules:**

| Control | Rule Class | Validation Logic |
|---------|-----------|------------------|
| `UserNameTextEdit` | `CustomUserNameValidationRule` | - Không rỗng<br>- Độ dài 3-50 ký tự |
| `PasswordTextEdit` | `CustomPasswordValidationRule` | - Không rỗng<br>- Độ dài tối thiểu 3 ký tự<br>- Password complexity (tùy chọn) |

---

## Các thành phần chính

### 1. Validation System

#### Custom Validation Rules

Form định nghĩa 2 custom validation rules:

**CustomUserNameValidationRule:**
```csharp
public class CustomUserNameValidationRule : ValidationRule
{
    public override bool Validate(Control control, object value)
    {
        // Sử dụng BLL ValidationHelper
        string username = ValidationHelper.NormalizeString(value?.ToString());
        
        // Rule 1: Không rỗng
        if (ValidationHelper.IsNullOrWhiteSpace(username))
        {
            ErrorText = "Vui lòng nhập tên đăng nhập";
            ErrorType = ErrorType.Critical;
            return false;
        }
        
        // Rule 2: Độ dài 3-50
        if (!ValidationHelper.IsValidLength(username, 3, 50))
        {
            ErrorText = "Tên đăng nhập phải có từ 3 đến 50 ký tự";
            ErrorType = ErrorType.Warning;
            return false;
        }
        
        return true;
    }
}
```

**CustomPasswordValidationRule:**
- Tương tự nhưng kiểm tra độ dài tối thiểu 3 ký tự
- Có option kiểm tra password complexity (hiện tại `requireComplexity = false`)

#### Validation Helper từ BLL

Form sử dụng `Bll.Validation.ValidationHelper` để:
- Tái sử dụng logic validation
- Đảm bảo consistency across forms
- Dễ dàng maintain và update

**Các phương thức được sử dụng:**
- `NormalizeString()` - Chuẩn hóa chuỗi
- `IsNullOrWhiteSpace()` - Kiểm tra rỗng
- `IsValidLength()` - Kiểm tra độ dài
- `IsMinLength()` - Kiểm tra độ dài tối thiểu
- `IsPasswordComplex()` - Kiểm tra độ phức tạp (tùy chọn)

### 2. Remember Me Feature

#### Lưu trữ thông tin

Sử dụng `Properties.Settings` để lưu:
- `RememberMe` (bool) - Trạng thái checkbox
- `SavedUsername` (string) - Tên đăng nhập
- `SavedPassword` (string) - Mật khẩu (đã được mã hóa từ LoginBll)

**Lưu ý bảo mật:**
- Password được mã hóa trước khi lưu (từ `LoginBll`)
- Lưu trong User Settings (an toàn hơn Registry)
- Xóa ngay khi user bỏ check Remember Me

#### Luồng Remember Me

```
┌─────────────────────────────┐
│ User check Remember Me      │
└──────────┬──────────────────┘
           │
           ▼
┌─────────────────────────────┐
│ Đăng nhập thành công        │
└──────────┬──────────────────┘
           │
           ▼
┌─────────────────────────────┐
│ HandleRememberMe()          │
│ - SaveCredentials()         │
│   → Properties.Settings      │
└─────────────────────────────┘
```

**Khi load form:**
```
┌─────────────────────────────┐
│ LoadSavedCredentials()      │
└──────────┬──────────────────┘
           │
     ┌─────┴─────┐
     │           │
  Has Saved   No Saved
     │           │
     ▼           ▼
┌─────────┐  ┌──────────┐
│ Load    │  │ Focus    │
│ Username│  │ Username │
│ & Pass  │  │ Field    │
└─────────┘  └──────────┘
```

### 3. Authentication Flow

#### LoginBll Integration

Form sử dụng `LoginBll.XacThucDangNhap()` để xác thực:

```csharp
var loginResult = _loginBll.XacThucDangNhap(
    loginCredentials.Username, 
    loginCredentials.Password
);
```

**LoginResult Structure:**
```csharp
public class LoginResult
{
    public bool ThanhCong { get; set; }
    public string ThongBaoLoi { get; set; }
    public LoginErrorCode MaLoi { get; set; }
    public UserInfo User { get; set; }
}
```

**LoginErrorCode Enum:**
- `ThanhCong = 0`
- `UserNameRong = 1`
- `MatKhauRong = 2`
- `UserKhongTonTai = 3`
- `UserBiVoHieuHoa = 4`
- `MatKhauSai = 5`
- `LoiHeThong = 99`

#### Xử lý kết quả đăng nhập

**Thành công:**
1. Lưu user vào `ApplicationSystemUtils.SetCurrentUser()`
2. Xử lý Remember Me
3. Hiển thị thông báo thành công
4. Đóng form với `DialogResult.OK`

**Thất bại:**
- Hiển thị thông báo lỗi cụ thể từ `LoginResult.ThongBaoLoi`
- Giữ form mở để user thử lại

### 4. Keyboard Navigation

Form hỗ trợ navigation bằng phím Enter:

```csharp
private void TextEdit_KeyDown(object sender, KeyEventArgs e)
{
    if (e.KeyCode == Keys.Enter)
    {
        if (sender == UserNameTextEdit)
            PasswordTextEdit.Focus();
        else if (sender == PasswordTextEdit)
            OkButton.PerformClick();
    }
}
```

**Flow:**
- Enter ở Username → Focus Password
- Enter ở Password → Click OK button

### 5. User Management (Development/Testing)

Form có các phương thức public để tạo tài khoản:

**Tạo Admin mặc định:**
```csharp
public void TaoTaiKhoanAdminPublic()
```
- Tự động gọi trong constructor
- Tạo tài khoản "admin/admin" nếu chưa tồn tại
- Dùng cho testing và development

**Tạo User tùy chỉnh:**
```csharp
public void TaoTaiKhoanUserPublic(string userName, string password, bool active = true)
```
- Có thể gọi từ bên ngoài form
- Tạo user với thông tin tùy chỉnh

---

## Constants và Configuration

### Constants

```csharp
private const int MIN_PASSWORD_LENGTH = 3;
```

**Lưu ý:** Constant này được sử dụng trong `PerformAdditionalValidation()` nhưng validation chính đã được xử lý bởi `CustomPasswordValidationRule`. Có thể xem xét loại bỏ để tránh duplicate logic.

---

## Error Handling

### Exception Handling Strategy

Form sử dụng try-catch blocks ở các điểm quan trọng:

1. **Remember Me Operations:**
   - `SaveCredentials()` - Lưu thông tin
   - `ClearSavedCredentials()` - Xóa thông tin
   - `LoadSavedCredentials()` - Tải thông tin

2. **User Management:**
   - `TaoTaiKhoanAdmin()` - Tạo admin
   - `TaoTaiKhoanUser()` - Tạo user

3. **Main Login Flow:**
   - `OkButton_Click()` - Xử lý đăng nhập

**Error Display:**
- Sử dụng `MsgBox.ShowException()` để hiển thị lỗi
- Hiển thị thông báo cụ thể cho từng loại lỗi

---

## Debugging Guide

### 1. Debug Points

**Breakpoints quan trọng:**
- `OkButton_Click()` - Điểm vào chính
- `ValidateFormInput()` - Kiểm tra validation
- `LoginBll.XacThucDangNhap()` - Xác thực
- `HandleRememberMe()` - Xử lý Remember Me

### 2. Common Issues

#### Issue 1: Validation không hoạt động

**Triệu chứng:** Validation rules không được kích hoạt

**Kiểm tra:**
1. `dxValidationProvider1.ValidationMode` phải là `ValidationMode.Manual`
2. Validation rules đã được set: `dxValidationProvider1.SetValidationRule()`
3. `ValidateFormInput()` được gọi trước khi xử lý

**Debug:**
```csharp
// Thêm breakpoint và kiểm tra
bool isValid = dxValidationProvider1.Validate();
```

#### Issue 2: Remember Me không lưu

**Triệu chứng:** Thông tin đăng nhập không được lưu

**Kiểm tra:**
1. `Properties.Settings.Default.RememberMe` có được set không
2. `Properties.Settings.Default.Save()` có được gọi không
3. Kiểm tra file settings: `%LocalAppData%\YourApp\user.config`

**Debug:**
```csharp
// Kiểm tra giá trị
var rememberMe = Properties.Settings.Default.RememberMe;
var username = Properties.Settings.Default.SavedUsername;
```

#### Issue 3: Login không thành công

**Triệu chứng:** Đăng nhập thất bại dù thông tin đúng

**Kiểm tra:**
1. `LoginResult.MaLoi` để xác định loại lỗi
2. Kiểm tra `LoginBll` có hoạt động đúng không
3. Kiểm tra database connection
4. Kiểm tra password hashing/verification

**Debug:**
```csharp
var loginResult = _loginBll.XacThucDangNhap(username, password);
// Kiểm tra loginResult.MaLoi
switch (loginResult.MaLoi)
{
    case LoginErrorCode.UserKhongTonTai:
        // User không tồn tại
        break;
    case LoginErrorCode.MatKhauSai:
        // Mật khẩu sai
        break;
    // ...
}
```

### 3. Logging (Nếu cần thêm)

Có thể thêm logging để debug:

```csharp
// Ví dụ thêm logging
private void OkButton_Click(object sender, EventArgs e)
{
    try
    {
        // Log attempt
        System.Diagnostics.Debug.WriteLine($"Login attempt: {UserNameTextEdit.Text}");
        
        // ... existing code ...
        
        if (loginResult.ThanhCong)
        {
            System.Diagnostics.Debug.WriteLine("Login successful");
        }
        else
        {
            System.Diagnostics.Debug.WriteLine($"Login failed: {loginResult.MaLoi}");
        }
    }
    catch (Exception ex)
    {
        System.Diagnostics.Debug.WriteLine($"Exception: {ex.Message}");
        throw;
    }
}
```

---

## Nâng cấp và Mở rộng

### 1. Thêm Validation Rules mới

**Ví dụ: Thêm email validation cho username**

```csharp
// Trong CustomUserNameValidationRule.Validate()
// Thêm sau rule kiểm tra độ dài:

// Rule 3: Kiểm tra định dạng email (nếu cần)
if (requireEmailFormat && !ValidationHelper.IsValidEmail(username))
{
    ErrorText = "Tên đăng nhập phải là email hợp lệ";
    ErrorType = ErrorType.Warning;
    return false;
}
```

### 2. Thêm Password Complexity

**Bật password complexity check:**

```csharp
// Trong CustomPasswordValidationRule.Validate()
// Thay đổi:
bool requireComplexity = true; // Thay vì false
```

### 3. Thêm Captcha/2FA

**Ví dụ thêm 2FA:**

```csharp
private void OkButton_Click(object sender, EventArgs e)
{
    // ... existing validation ...
    
    // Thêm 2FA step
    if (loginResult.ThanhCong && user.Requires2FA)
    {
        var twoFactorCode = Show2FADialog();
        if (!Verify2FA(twoFactorCode))
        {
            ShowAuthenticationFailedMessage("Mã xác thực 2FA không đúng");
            return;
        }
    }
    
    // ... continue ...
}
```

### 4. Thêm Rate Limiting

**Ngăn brute force attacks:**

```csharp
private int _failedAttempts = 0;
private DateTime? _lastAttemptTime;

private void OkButton_Click(object sender, EventArgs e)
{
    // Check rate limiting
    if (_failedAttempts >= 5)
    {
        var timeSinceLastAttempt = DateTime.Now - _lastAttemptTime.Value;
        if (timeSinceLastAttempt.TotalMinutes < 15)
        {
            ShowAuthenticationFailedMessage("Quá nhiều lần thử. Vui lòng thử lại sau 15 phút.");
            return;
        }
        else
        {
            _failedAttempts = 0; // Reset
        }
    }
    
    // ... existing login logic ...
    
    if (!loginResult.ThanhCong)
    {
        _failedAttempts++;
        _lastAttemptTime = DateTime.Now;
    }
    else
    {
        _failedAttempts = 0; // Reset on success
    }
}
```

### 5. Async/Await Support

**Chuyển sang async nếu cần:**

```csharp
private async void OkButton_Click(object sender, EventArgs e)
{
    try
    {
        // ... validation ...
        
        // Sử dụng async version
        var loginResult = await _loginBll.XacThucDangNhapAsync(
            loginCredentials.Username, 
            loginCredentials.Password
        );
        
        // ... rest of code ...
    }
    catch (Exception ex)
    {
        MsgBox.ShowException(ex, "Lỗi hệ thống");
    }
}
```

### 6. Cải thiện Remember Me Security

**Sử dụng Windows Credential Manager thay vì Settings:**

```csharp
using System.Security.Cryptography;

private void SaveCredentials(string username, string password)
{
    // Sử dụng Windows Credential Manager
    // Hoặc encrypt password với DPAPI
    byte[] encrypted = ProtectedData.Protect(
        Encoding.UTF8.GetBytes(password),
        null,
        DataProtectionScope.CurrentUser
    );
    
    Properties.Settings.Default.SavedPassword = Convert.ToBase64String(encrypted);
    Properties.Settings.Default.Save();
}
```

---

## Best Practices

### 1. Validation
- ✅ Sử dụng BLL ValidationHelper để tái sử dụng logic
- ✅ Validation rules tách riêng trong custom classes
- ✅ Validation mode Manual để kiểm soát khi nào validate
- ⚠️ Tránh duplicate validation logic (MIN_PASSWORD_LENGTH vs CustomPasswordValidationRule)

### 2. Error Handling
- ✅ Try-catch ở các điểm quan trọng
- ✅ Hiển thị thông báo lỗi cụ thể cho user
- ✅ Log exceptions để debug (nếu cần)

### 3. Security
- ✅ Password được hash trước khi lưu (từ LoginBll)
- ✅ Không lưu plain text password
- ⚠️ Có thể cải thiện: Sử dụng DPAPI hoặc Credential Manager

### 4. Code Organization
- ✅ Code được tổ chức theo regions
- ✅ Methods có XML documentation
- ✅ Separation of concerns (UI/BLL/DAL)

### 5. User Experience
- ✅ Keyboard navigation (Enter key)
- ✅ Auto-focus vào đúng field
- ✅ Remember Me feature
- ✅ Clear error messages

---

## Testing Checklist

### Unit Testing (Nếu có)

**Test Cases cần cover:**
- [ ] Validation rules hoạt động đúng
- [ ] Login thành công với credentials hợp lệ
- [ ] Login thất bại với credentials không hợp lệ
- [ ] Remember Me lưu và load đúng
- [ ] Keyboard navigation hoạt động
- [ ] Error messages hiển thị đúng

### Manual Testing

**Test Scenarios:**
1. **Happy Path:**
   - Nhập username và password hợp lệ
   - Click "Đăng nhập"
   - Kiểm tra đăng nhập thành công

2. **Validation:**
   - Để trống username → Kiểm tra error message
   - Username < 3 ký tự → Kiểm tra error message
   - Password < 3 ký tự → Kiểm tra error message

3. **Remember Me:**
   - Check Remember Me → Đăng nhập → Đóng form → Mở lại → Kiểm tra thông tin đã lưu
   - Uncheck Remember Me → Kiểm tra thông tin bị xóa

4. **Keyboard Navigation:**
   - Enter ở username → Focus chuyển sang password
   - Enter ở password → Đăng nhập được kích hoạt

5. **Error Cases:**
   - Sai username → Kiểm tra error message
   - Sai password → Kiểm tra error message
   - User bị vô hiệu hóa → Kiểm tra error message

---

## Tài liệu tham khảo

### Related Files
- `Bll/Authentication/LoginBll.cs` - Business logic layer
- `Bll/Validation/ValidationHelper.cs` - Validation helpers
- `Bll/Utils/ApplicationSystemUtils.cs` - User management
- `Bll/Utils/SecurityHelper.cs` - Password hashing

### DevExpress Documentation
- DXValidationProvider: https://docs.devexpress.com/WindowsForms/DevExpress.XtraEditors.DXErrorProvider.DXValidationProvider
- ValidationRule: https://docs.devexpress.com/WindowsForms/DevExpress.XtraEditors.DXErrorProvider.ValidationRule

---

## Changelog

### Version 1.0 (Current)
- ✅ Basic login functionality
- ✅ DXValidationProvider integration
- ✅ Remember Me feature
- ✅ Keyboard navigation
- ✅ User management methods
- ✅ Comprehensive error handling

---

## Liên hệ và Hỗ trợ

Nếu có câu hỏi hoặc cần hỗ trợ, vui lòng liên hệ:
- Team Lead hoặc Senior Developer
- Tạo issue trong hệ thống quản lý dự án

