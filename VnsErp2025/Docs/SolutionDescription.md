# VNS ERP 2025 - Mô Tả Solution

## 1. Tổng Quan Dự Án

**Tên dự án:** VNS ERP 2025  
**Loại ứng dụng:** Hệ thống quản lý doanh nghiệp (Enterprise Resource Planning)  
**Kiến trúc:** 3-Layer Architecture (DAL, BLL, GUI)  
**Ngôn ngữ lập trình:** C#  
**Framework:** .NET Framework 4.8  
**Cơ sở dữ liệu:** Microsoft SQL Server  

## 2. Kiến Trúc Hệ Thống

### 2.1 Mô Hình 3 Lớp (3-Layer Architecture)

```
┌─────────────────────────────────────┐
│            GUI Layer                │
│      (Presentation Layer)           │
│         VnsErp2025                  │
└─────────────────┬───────────────────┘
                  │
┌─────────────────┴───────────────────┐
│            BLL Layer                │
│       (Business Logic Layer)        │
│              Bll                    │
└─────────────────┬───────────────────┘
                  │
┌─────────────────┴───────────────────┐
│            DAL Layer                │
│       (Data Access Layer)           │
│              Dal                    │
└─────────────────┬───────────────────┘
                  │
┌─────────────────┴───────────────────┐
│         Database Layer              │
│       Microsoft SQL Server          │
└─────────────────────────────────────┘
```

### 2.2 Mô Tả Các Lớp

#### **GUI Layer (VnsErp2025)**
- **Mục đích:** Giao diện người dùng, xử lý tương tác với người dùng
- **Công nghệ:** Windows Forms với DevExpress Controls
- **Chức năng:**
  - Hiển thị dữ liệu cho người dùng
  - Thu thập input từ người dùng
  - Xử lý các sự kiện UI
  - Validation dữ liệu ở tầng presentation
- **Framework:** .NET Framework 4.8
- **UI Components:** DevExpress v25.1

#### **BLL Layer (Bll)**
- **Mục đích:** Xử lý logic nghiệp vụ, quy tắc kinh doanh
- **Chức năng:**
  - Xử lý logic nghiệp vụ phức tạp
  - Validation dữ liệu nghiệp vụ
  - Quản lý workflow và business rules
  - Chuyển đổi dữ liệu giữa DAL và GUI
- **Loại project:** Class Library (.dll)

#### **DAL Layer (Dal)**
- **Mục đích:** Truy cập và thao tác với cơ sở dữ liệu
- **Chức năng:**
  - Kết nối cơ sở dữ liệu
  - Thực hiện các thao tác CRUD (Create, Read, Update, Delete)
  - Quản lý connection string
  - Xử lý stored procedures và SQL queries
- **Loại project:** Class Library (.dll)

## 3. Cấu Trúc Solution

### 3.1 Danh Sách Projects

| Tên Project | Loại | Mô Tả |
|-------------|------|-------|
| `VnsErp2025` | Windows Application | GUI Layer - Ứng dụng Windows Forms chính |
| `Bll` | Class Library | Business Logic Layer - Xử lý logic nghiệp vụ |
| `Dal` | Class Library | Data Access Layer - Truy cập cơ sở dữ liệu |

### 3.2 Dependencies

```
VnsErp2025 (GUI)
    ├── Bll (Business Logic)
    │   └── Dal (Data Access)
    └── DevExpress Components
        ├── DevExpress.XtraEditors.v25.1
        ├── DevExpress.Data.v25.1
        ├── DevExpress.Utils.v25.1
        └── DevExpress.BonusSkins.v25.1
```

## 4. Công Nghệ Sử Dụng

### 4.1 Framework và Runtime
- **.NET Framework:** 4.8
- **Platform Target:** Any CPU
- **Language:** C#

### 4.2 UI Framework
- **DevExpress:** Version 25.1
- **Windows Forms:** .NET Framework built-in
- **Skin Support:** DevExpress BonusSkins

### 4.3 Cơ Sở Dữ Liệu
- **Database Engine:** Microsoft SQL Server
- **Connection:** ADO.NET hoặc Entity Framework (tùy theo implementation)

### 4.4 Development Environment
- **IDE:** Visual Studio 2022 Enterprise
- **Version Control:** Azure DevOps TFS
- **Code Quality:** ReSharper

## 5. Quy Ước Coding

### 5.1 Naming Convention
- **Class Names:** PascalCase (tiếng Việt không dấu)
- **Method Names:** PascalCase
- **Property Names:** PascalCase (tiếng Việt không dấu)
- **Variable Names:** camelCase
- **Database Columns:** snake_case (tiếng Việt không dấu)

### 5.2 Code Organization
- **Regions:** Sử dụng regions bằng tiếng Việt không dấu
- **Comments:** Tiếng Việt để dễ quản lý
- **Business Objects:** Tuân thủ XAF DevExpress standards

## 6. Cấu Hình Build

### 6.1 Build Configurations
- **Debug:** Development và testing
- **Release:** Production deployment

### 6.2 Output Types
- **VnsErp2025:** Windows Executable (.exe)
- **Bll:** Class Library (.dll)
- **Dal:** Class Library (.dll)

## 7. Mục Tiêu Phát Triển

### 7.1 Giai Đoạn Hiện Tại
- Thiết lập kiến trúc cơ bản 3 lớp
- Chuẩn bị infrastructure cho development

### 7.2 Kế Hoạch Phát Triển
- Implement các module nghiệp vụ cốt lõi
- Phát triển giao diện người dùng
- Tích hợp với cơ sở dữ liệu
- Testing và deployment

## 8. Tài Liệu Liên Quan

- **Data Dictionary:** Mỗi Business Object sẽ có file DataDictionary.md riêng
- **API Documentation:** Sẽ được tạo cho các public methods
- **Database Schema:** Sẽ được document riêng
- **User Manual:** Hướng dẫn sử dụng cho end users

---

**Ngày tạo:** $(Get-Date -Format "dd/MM/yyyy")  
**Phiên bản:** 1.0  
**Người tạo:** Project Manager  
**Trạng thái:** Draft
