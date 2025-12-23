# Migration: Device Table Column Refactoring

## Mô tả
Script migration này điều chỉnh bảng `Device` bằng cách xóa các cột không cần thiết và chỉ giữ lại các cột theo yêu cầu mới.

## Ngày thực hiện
2025

## Mục đích
Đơn giản hóa cấu trúc bảng `Device` bằng cách loại bỏ các cột không còn được sử dụng, giúp:
- Giảm độ phức tạp của bảng
- Cải thiện hiệu suất
- Dễ dàng bảo trì hơn

## Các cột bị xóa

### Foreign Key Columns (và constraints liên quan)
- `CompanyId` - Liên kết với bảng Company
- `BranchId` - Liên kết với bảng CompanyBranch
- `DepartmentId` - Liên kết với bảng Department
- `AssignedEmployeeId` - Liên kết với bảng Employee

### Thông tin vị trí
- `Location` - Vị trí cụ thể (phòng, tầng, v.v.)

### Thông tin ngày tháng
- `PurchaseDate` - Ngày mua
- `InstallationDate` - Ngày lắp đặt/cài đặt
- `LastMaintenanceDate` - Ngày bảo trì cuối
- `NextMaintenanceDate` - Ngày bảo trì tiếp theo

### Thông tin bổ sung
- `Manufacturer` - Nhà sản xuất
- `Model` - Model cụ thể
- `Specifications` - Thông số kỹ thuật chi tiết
- `Configuration` - Cấu hình (cho phần mềm)

## Các cột được giữ lại

### Primary Key & Foreign Keys
- `Id` [uniqueidentifier] NOT NULL - Primary Key
- `ProductVariantId` [uniqueidentifier] NOT NULL - Loại thiết bị
- `StockInOutDetailId` [uniqueidentifier] NULL - Phiếu nhập kho
- `WarrantyId` [uniqueidentifier] NULL - Thông tin bảo hành

### Thông tin định danh duy nhất
- `SerialNumber` [nvarchar](100) NULL - Số serial
- `MACAddress` [nvarchar](50) NULL - Địa chỉ MAC
- `IMEI` [nvarchar](50) NULL - IMEI
- `AssetTag` [nvarchar](50) NULL - Mã tài sản
- `LicenseKey` [nvarchar](255) NULL - License key
- `HostName` [nvarchar](100) NULL - Tên máy/hostname
- `IPAddress` [nvarchar](50) NULL - Địa chỉ IP

### Thông tin trạng thái
- `Status` [int] NOT NULL - Trạng thái thiết bị
- `DeviceType` [int] NOT NULL - Loại thiết bị

### Ghi chú
- `Notes` [nvarchar](1000) NULL - Ghi chú

### Audit Fields
- `IsActive` [bit] NOT NULL - Trạng thái hoạt động
- `CreatedDate` [datetime] NOT NULL - Ngày tạo
- `UpdatedDate` [datetime] NULL - Ngày cập nhật
- `CreatedBy` [uniqueidentifier] NULL - Người tạo
- `UpdatedBy` [uniqueidentifier] NULL - Người cập nhật

## Các thay đổi được thực hiện

1. **Xóa Foreign Key Constraints:**
   - `FK_Device_Company`
   - `FK_Device_CompanyBranch`
   - `FK_Device_Department`
   - `FK_Device_Employee`

2. **Xóa Indexes:**
   - `IX_Device_AssignedEmployeeId`
   - `IX_Device_DepartmentId`

3. **Xóa các cột:**
   - Tất cả các cột được liệt kê ở trên

## Lưu ý quan trọng

⚠️ **CẢNH BÁO:** Script này sẽ xóa vĩnh viễn dữ liệu trong các cột bị xóa. Hãy đảm bảo:
- Đã backup database trước khi chạy migration
- Đã kiểm tra và xác nhận không còn code nào sử dụng các cột này
- Đã test trên môi trường development trước

## Cách thực hiện

1. Backup database trước khi chạy migration
2. Chạy script `Migration_Device_Refactor_Columns.sql`
3. Kiểm tra kết quả và đảm bảo không có lỗi
4. Cập nhật LINQ to SQL model (VnsErp2025.dbml) nếu cần
5. Rebuild project để cập nhật DataContext

## Rollback

Nếu cần rollback, bạn sẽ cần:
1. Khôi phục từ backup
2. Hoặc tạo lại các cột và constraints đã xóa (không khôi phục được dữ liệu)

## Ảnh hưởng đến các bảng liên quan

- Bảng `DeviceHistory` - Không ảnh hưởng (vẫn lưu lịch sử thay đổi)
- Bảng `DeviceTransfer` - Không ảnh hưởng (vẫn quản lý chuyển giao thiết bị)
- Các bảng khác - Không ảnh hưởng

## Kiểm tra sau migration

Sau khi chạy migration, hãy kiểm tra:
- Bảng Device chỉ còn các cột được liệt kê ở trên
- Tất cả Foreign Key constraints đã được xóa đúng
- Tất cả Indexes đã được xóa đúng
- Không có lỗi trong application khi truy cập bảng Device
