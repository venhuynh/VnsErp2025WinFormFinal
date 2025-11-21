# Hướng dẫn sử dụng chức năng In Phiếu Nhập Kho

## Tổng quan

Chức năng in phiếu nhập kho được triển khai với các thành phần sau:

1. **DTO**: `StockInReportDto` - Chứa cấu trúc dữ liệu cho report
2. **Repository**: Method `GetMasterById` - Lấy master data với navigation properties
3. **BLL**: Method `GetReportData` - Lấy và map dữ liệu cho report
4. **Report**: `InPhieuNhapKho` - DevExpress XtraReport với logic load data
5. **Helper**: `StockInReportHelper` - Các method tiện ích để in/xuất report

## Cách sử dụng

### 1. In phiếu nhập kho với preview

```csharp
using Inventory.StockIn.InPhieu;

// In với preview (người dùng có thể xem trước khi in)
StockInReportHelper.PrintStockInVoucher(voucherId);
```

### 2. In trực tiếp (không preview)

```csharp
// In trực tiếp ra máy in mặc định
StockInReportHelper.PrintStockInVoucherDirect(voucherId);
```

### 3. Xuất ra file PDF

```csharp
// Xuất ra file PDF
string filePath = @"C:\Reports\PhieuNhapKho.pdf";
StockInReportHelper.ExportStockInVoucherToPdf(voucherId, filePath);
```

### 4. Sử dụng trực tiếp từ Report class

```csharp
using Inventory.StockIn.InPhieu;
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting;

// Tạo report với VoucherId (tự động load data)
var report = new InPhieuNhapKho(voucherId);

// Hoặc tạo report rồi load data sau
var report = new InPhieuNhapKho();
report.LoadData(voucherId);

// Hiển thị preview bằng ReportPrintTool
using (var printTool = new ReportPrintTool(report))
{
    printTool.ShowPreviewDialog();
}

// Hoặc in trực tiếp
report.Print();
```

## Cấu trúc dữ liệu

### StockInReportDto

Report sử dụng cấu trúc dữ liệu như sau:

```csharp
StockInReportDto
├── SoPhieu (string) - Số phiếu nhập kho
├── NgayThang (DateTime) - Ngày nhập kho
├── NhanHangTu
│   └── FullName (string) - Tên người giao hàng
├── NguoiNhapXuat
│   └── FullName (string) - Tên người nhập kho
├── KhoNhap
│   └── FullProductNameName (string) - Tên kho nhập
└── ChiTietNhapHangNoiBos (List<ChiTietNhapHangNoiBoDto>)
    ├── SanPham.ProductName (string) - Tên sản phẩm
    ├── DonViTinh (string) - Đơn vị tính
    ├── SoLuong (decimal) - Số lượng
    └── TinhTrangSanPham (string) - Tình trạng sản phẩm
```

## Expression Binding trong Report

Report sử dụng các expression binding sau (đã được cấu hình trong Designer):

- `[SoPhieu]` - Số phiếu
- `[NhanHangTu].[FullName]` - Tên người giao hàng
- `[NguoiNhapXuat].[FullName]` - Tên người nhập
- `[NgayThang]` - Ngày nhập (format: dd-MMM-yy)
- `[KhoNhap].[FullProductNameName]` - Tên kho nhập
- `[SanPham].[ProductName]` - Tên sản phẩm (trong detail)
- `[DonViTinh]` - Đơn vị tính (trong detail)
- `[SoLuong]` - Số lượng (trong detail, format: N2)
- `[TinhTrangSanPham]` - Tình trạng (trong detail, format: N0)

## Lưu ý

1. **Người nhập kho**: Hiện tại field `NguoiNhapXuat.FullName` đang để trống. Cần bổ sung logic lấy thông tin người nhập từ `CreatedBy` hoặc related entity sau khi có authentication.

2. **Tình trạng sản phẩm**: Hiện tại field `TinhTrangSanPham` đang hardcode là "Bình thường". Cần bổ sung logic lấy từ database nếu có.

3. **Người giao hàng**: Hiện tại lấy từ `PartnerSite.BusinessPartner.Name` hoặc `PartnerSite.Name`. Có thể cần điều chỉnh logic này tùy theo yêu cầu nghiệp vụ.

## Ví dụ tích hợp vào Form

```csharp
// Trong form quản lý nhập kho
private void btnPrint_Click(object sender, EventArgs e)
{
    if (_currentStockInId == Guid.Empty)
    {
        MsgBox.ShowWarning("Vui lòng chọn phiếu nhập kho để in.");
        return;
    }

    // In với preview
    StockInReportHelper.PrintStockInVoucher(_currentStockInId);
}
```

## Xử lý lỗi

Tất cả các method đều có xử lý lỗi và logging. Khi có lỗi:
- Log được ghi vào logger với level Error
- Thông báo lỗi hiển thị cho người dùng qua `MsgBox.ShowError()`

## Logging

Tất cả các thao tác đều được log:
- Debug: Các thao tác bắt đầu
- Info: Các thao tác thành công
- Warning: Các cảnh báo (ví dụ: không tìm thấy dữ liệu)
- Error: Các lỗi xảy ra

