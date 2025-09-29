# Cách Test ImageService Integration

## 🚀 **Các Cách Test ImageService**

### **1. Test Trong FrmProductServiceDetail**

#### **A. Sử dụng Phím Tắt**
1. Mở form `FrmProductServiceDetail`
2. Nhấn `Ctrl + T`
3. Xem kết quả test trong message box

#### **B. Test Khi Chọn Ảnh**
1. Mở form `FrmProductServiceDetail`
2. Click nút "Browse" để chọn ảnh
3. ImageService sẽ tự động:
   - Validate file ảnh
   - Compress ảnh
   - Hiển thị kết quả

### **2. Test Standalone**

#### **A. Console Test**
```csharp
// Chạy trong console application
TestImageServiceIntegration.Main(new string[0]);
```

#### **B. WinForms Test**
```csharp
// Chạy trong WinForms application
TestImageServiceIntegration.TestInWinForms();
```

#### **C. Programmatic Test**
```csharp
// Test trong code
TestImageServiceIntegration.TestBasicFunctionality();
```

## 🧪 **Các Test Cases**

### **1. Configuration Test**
- ✅ ImageServiceConfiguration creation
- ✅ CdnBaseUrl, UseCdn, LocalImagePath
- ✅ MemoryCacheSize, DefaultImageQuality
- ✅ MaxImageWidth, MaxImageHeight

### **2. Service Creation Test**
- ✅ ImageService creation
- ✅ ImageValidationService creation
- ✅ ImageCompressionService creation

### **3. Format Validation Test**
- ✅ JPG format allowed
- ✅ PNG format allowed
- ✅ TXT format rejected

### **4. Image Validation Test**
- ✅ Invalid file detection
- ✅ Error message generation
- ✅ Validation result structure

## 📊 **Expected Results**

### **✅ Success Output**
```
✅ ImageServiceConfiguration created successfully
   - CdnBaseUrl: 
   - UseCdn: False
   - LocalImagePath: ~/Images/

✅ ImageValidationService created successfully
✅ ImageCompressionService created successfully
✅ ImageService created successfully

✅ Format validation working:
   - JPG allowed: True
   - PNG allowed: True
   - TXT allowed: False

✅ Image validation working:
   - IsValid: False
   - Errors count: 1

🎉 All ImageService integration tests passed!
```

### **❌ Error Output**
```
❌ ImageService integration test failed: [Error Message]
Stack trace: [Stack Trace]
```

## 🔧 **Troubleshooting**

### **1. Lỗi "Type not found"**
```
Error: The type or namespace name 'ImageValidationService' could not be found
```
**Solution:**
- Kiểm tra Bll project có được build thành công không
- Kiểm tra MasterData project có reference đến Bll project không
- Rebuild solution

### **2. Lỗi "Assembly not found"**
```
Error: Could not load file or assembly 'Bll'
```
**Solution:**
- Build Bll project trước
- Kiểm tra output path của Bll project
- Copy Bll.dll đến MasterData bin folder

### **3. Lỗi "Missing reference"**
```
Error: The type or namespace name 'System.Security.Cryptography' could not be found
```
**Solution:**
- Thêm reference `System.Security.Cryptography.Algorithms` vào Bll project
- Rebuild Bll project

## 🎯 **Test Scenarios**

### **1. Happy Path**
1. Mở FrmProductServiceDetail
2. Nhấn Ctrl+T
3. Thấy message "✅ ImageService test completed successfully!"

### **2. Image Upload Test**
1. Mở FrmProductServiceDetail
2. Click Browse button
3. Chọn file ảnh hợp lệ (.jpg, .png)
4. Thấy message "Đã chọn và nén ảnh thành công"

### **3. Invalid File Test**
1. Mở FrmProductServiceDetail
2. Click Browse button
3. Chọn file không hợp lệ (.txt, .exe)
4. Thấy error message "File ảnh không hợp lệ"

### **4. Large File Test**
1. Mở FrmProductServiceDetail
2. Click Browse button
3. Chọn file ảnh > 10MB
4. Thấy error message "Kích thước file quá lớn"

## 📈 **Performance Test**

### **1. Load Time Test**
- Measure time để load ảnh thumbnail
- Expected: < 1 second cho ảnh < 1MB

### **2. Compression Test**
- Measure compression ratio
- Expected: 50-70% size reduction

### **3. Memory Test**
- Monitor memory usage khi load ảnh
- Expected: < 100MB cho ảnh 5MB

## 🛡️ **Security Test**

### **1. File Signature Test**
- Test với file có signature giả mạo
- Expected: Rejected

### **2. Malicious Content Test**
- Test với file chứa malicious code
- Expected: Detected và blocked

### **3. Metadata Test**
- Test với file có metadata nhạy cảm
- Expected: Sanitized

## 🎉 **Success Criteria**

### **✅ All Tests Pass**
- Configuration loads correctly
- Services create successfully
- Validation works properly
- Compression functions correctly
- Error handling works
- Performance meets requirements
- Security checks pass

### **📊 Metrics**
- **Load Time:** < 1 second
- **Compression Ratio:** 50-70%
- **Memory Usage:** < 100MB
- **Error Rate:** 0%
- **Security Score:** 100%

---

**Khi tất cả tests pass, ImageService integration đã sẵn sàng cho production!** 🚀
