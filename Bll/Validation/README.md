# Validation Helper - BLL Layer

Thư viện validation helper tái sử dụng cho các form trong hệ thống VNS ERP 2025.

## Tổng quan

ValidationHelper được thiết kế theo nguyên tắc:
- **BLL**: Chứa các logic validation có thể tái sử dụng
- **Form**: Khai báo các validation rules cụ thể sử dụng BLL helpers
- **Separation of Concerns**: Tách biệt logic validation và UI validation rules

## ValidationHelper Class

### Các phương thức validation có thể tái sử dụng:

#### 1. String Validation Methods
```csharp
// Kiểm tra chuỗi trống
bool IsNullOrWhiteSpace(string value)

// Kiểm tra độ dài trong khoảng
bool IsValidLength(string value, int minLength, int maxLength)

// Kiểm tra độ dài tối thiểu
bool IsMinLength(string value, int minLength)
```

#### 2. Email Validation Methods
```csharp
// Kiểm tra định dạng email đầy đủ
bool IsValidEmail(string email)

// Kiểm tra email có chứa @ (kiểm tra cơ bản)
bool ContainsAtSymbol(string email)
```

#### 3. Password Validation Methods
```csharp
// Kiểm tra độ phức tạp mật khẩu
bool IsPasswordComplex(string password)

// Kiểm tra từng thành phần
bool HasUpperCase(string password)
bool HasLowerCase(string password)
bool HasDigit(string password)
bool HasSpecialCharacter(string password)
```

#### 4. Number Validation Methods
```csharp
// Kiểm tra số trong khoảng
bool IsInRange(int value, int minValue, int maxValue)

// Kiểm tra số >= giá trị tối thiểu
bool IsGreaterOrEqual(int value, int minValue)
```

#### 5. Utility Methods
```csharp
// Chuẩn hóa chuỗi
string NormalizeString(string value)

// Kiểm tra ký tự không mong muốn
bool ContainsForbiddenCharacters(string value, char[] forbiddenChars)
```

## Cách sử dụng trong Form

### Ví dụ: FrmLogin.cs

```csharp
using BllValidation = Bll.Validation;

public partial class FrmLogin : XtraForm
{
    private void SetupUserNameValidation()
    {
        // Sử dụng CustomValidationRule với logic từ BLL ValidationHelper
        var userNameValidationRule = new CustomUserNameValidationRule();
        dxValidationProvider1.SetValidationRule(UserNameTextEdit, userNameValidationRule);
    }

    private void SetupPasswordValidation()
    {
        // Sử dụng CustomValidationRule với logic từ BLL ValidationHelper
        var passwordValidationRule = new CustomPasswordValidationRule();
        dxValidationProvider1.SetValidationRule(PasswordTextEdit, passwordValidationRule);
    }
}

// Custom Validation Rules - Form Specific
public class CustomUserNameValidationRule : ValidationRule
{
    public override bool Validate(Control control, object value)
    {
        string username = BllValidation.ValidationHelper.NormalizeString(value?.ToString());

        // Rule 1: Kiểm tra không được để trống - sử dụng BLL ValidationHelper
        if (BllValidation.ValidationHelper.IsNullOrWhiteSpace(username))
        {
            ErrorText = "Vui lòng nhập tên đăng nhập";
            ErrorType = ErrorType.Critical;
            return false;
        }

        // Rule 2: Kiểm tra độ dài - sử dụng BLL ValidationHelper
        if (!BllValidation.ValidationHelper.IsValidLength(username, 3, 50))
        {
            ErrorText = "Tên đăng nhập phải có từ 3 đến 50 ký tự";
            ErrorType = ErrorType.Warning;
            return false;
        }

        // Rule 3: Kiểm tra định dạng email - sử dụng BLL ValidationHelper
        if (!BllValidation.ValidationHelper.IsValidEmail(username))
        {
            ErrorText = "Tên đăng nhập phải có định dạng email hợp lệ";
            ErrorType = ErrorType.Warning;
            return false;
        }

        return true;
    }
}

public class CustomPasswordValidationRule : ValidationRule
{
    public override bool Validate(Control control, object value)
    {
        string password = value?.ToString() ?? string.Empty;

        // Rule 1: Kiểm tra không được để trống - sử dụng BLL ValidationHelper
        if (BllValidation.ValidationHelper.IsNullOrWhiteSpace(password))
        {
            ErrorText = "Vui lòng nhập mật khẩu";
            ErrorType = ErrorType.Critical;
            return false;
        }

        // Rule 2: Kiểm tra độ dài tối thiểu - sử dụng BLL ValidationHelper
        if (!BllValidation.ValidationHelper.IsMinLength(password, 3))
        {
            ErrorText = "Mật khẩu phải có ít nhất 3 ký tự";
            ErrorType = ErrorType.Warning;
            return false;
        }

        // Rule 3: Kiểm tra độ phức tạp (tùy chọn) - sử dụng BLL ValidationHelper
        bool requireComplexity = false;
        if (requireComplexity && !BllValidation.ValidationHelper.IsPasswordComplex(password))
        {
            ErrorText = "Mật khẩu phải chứa chữ hoa, chữ thường, số và ký tự đặc biệt";
            ErrorType = ErrorType.Warning;
            return false;
        }

        return true;
    }
}
```

## Lợi ích của Approach này

### 1. **Separation of Concerns**
- **BLL**: Chứa logic validation thuần túy
- **Form**: Chứa validation rules cụ thể cho UI

### 2. **Reusability**
- ValidationHelper có thể sử dụng trong nhiều form khác nhau
- Logic validation được tái sử dụng

### 3. **Maintainability**
- Thay đổi logic validation chỉ cần sửa ở BLL
- Form chỉ cần sửa validation rules cụ thể

### 4. **Testability**
- Có thể unit test ValidationHelper độc lập
- Có thể test validation rules riêng biệt

### 5. **Flexibility**
- Form có thể tùy chỉnh validation rules theo nhu cầu
- BLL cung cấp các building blocks linh hoạt

## Mở rộng

### Thêm validation method mới vào BLL:
```csharp
// Trong ValidationHelper.cs
public static bool IsValidPhoneNumber(string phoneNumber)
{
    // Logic validation phone number
    return Regex.IsMatch(phoneNumber, @"^\+?[\d\s\-\(\)]+$");
}
```

### Sử dụng trong Form:
```csharp
// Trong CustomValidationRule
if (!BllValidation.ValidationHelper.IsValidPhoneNumber(phoneNumber))
{
    ErrorText = "Số điện thoại không hợp lệ";
    ErrorType = ErrorType.Warning;
    return false;
}
```

## Lưu ý

- ValidationHelper là static class, không cần khởi tạo
- Tất cả methods đều thread-safe
- Sử dụng constants cho các giá trị mặc định
- Error messages được định nghĩa ở Form level
- Logic validation được định nghĩa ở BLL level
