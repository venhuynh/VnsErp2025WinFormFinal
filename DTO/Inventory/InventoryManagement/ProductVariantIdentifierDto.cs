using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DTO.Inventory.InventoryManagement
{
    /// <summary>
    /// Enum định nghĩa các loại định danh sản phẩm
    /// </summary>
    public enum ProductVariantIdentifierEnum
    {
        /// <summary>
        /// Số Serial
        /// </summary>
        [Description("Serial Number")]
        SerialNumber = 0,

        /// <summary>
        /// Part Number
        /// </summary>
        [Description("Part Number")]
        PartNumber = 1,

        /// <summary>
        /// Mã QR
        /// </summary>
        [Description("QR Code")]
        QRCode = 2,

        /// <summary>
        /// Stock Keeping Unit
        /// </summary>
        [Description("SKU")]
        SKU = 3,

        /// <summary>
        /// Radio Frequency Identification
        /// </summary>
        [Description("RFID")]
        RFID = 4,

        /// <summary>
        /// Địa chỉ MAC
        /// </summary>
        [Description("MAC Address")]
        MACAddress = 5,

        /// <summary>
        /// Số IMEI
        /// </summary>
        [Description("IMEI")]
        IMEI = 6,

        /// <summary>
        /// Asset Tag
        /// </summary>
        [Description("Asset Tag")]
        AssetTag = 7,

        /// <summary>
        /// License Key
        /// </summary>
        [Description("License Key")]
        LicenseKey = 8,

        /// <summary>
        /// Universal Product Code
        /// </summary>
        [Description("UPC")]
        UPC = 9,

        /// <summary>
        /// European Article Number
        /// </summary>
        [Description("EAN")]
        EAN = 10,

        /// <summary>
        /// ID
        /// </summary>
        [Description("ID")]
        ID = 11,

        /// <summary>
        /// Loại định danh khác
        /// </summary>
        [Description("Other")]
        OtherIdentifier = 12
    }

    /// <summary>
    /// Enum định nghĩa tình trạng hàng hóa/sản phẩm
    /// </summary>
    public enum ProductVariantIdentifierStatusEnum
    {
        /// <summary>
        /// Tại kho VNS
        /// </summary>
        [Description("Tại kho VNS")]
        AtVnsWarehouse = 0,

        /// <summary>
        /// Đã xuất cho KH
        /// </summary>
        [Description("Đã xuất cho KH")]
        ExportedToCustomer = 1,

        /// <summary>
        /// Đang lắp đặt tại site KH
        /// </summary>
        [Description("Đang lắp đặt tại site KH")]
        InstallingAtCustomerSite = 2,

        /// <summary>
        /// Đang gửi Bảo hành NCC
        /// </summary>
        [Description("Đang gửi Bảo hành NCC")]
        UnderWarrantyAtSupplier = 3,

        /// <summary>
        /// Đã hư hỏng (Tại kho VNS)
        /// </summary>
        [Description("Đã hư hỏng (Tại kho VNS)")]
        DamagedAtVnsWarehouse = 4,

        /// <summary>
        /// Đã thanh lý
        /// </summary>
        [Description("Đã thanh lý")]
        Disposed = 5
    }

    /// <summary>
    /// Data Transfer Object cho ProductVariantIdentifier entity
    /// Quản lý các loại định danh cho ProductVariant: SerialNumber, PartNumber, QRCode, SKU, RFID, MAC, IMEI, AssetTag, LicenseKey, UPC, EAN, ISBN, v.v.
    /// </summary>
    public class ProductVariantIdentifierDto
    {
        #region Properties - Thông tin cơ bản

        /// <summary>
        /// ID duy nhất của định danh
        /// </summary>
        [DisplayName("ID")]
        [Display(Order = -1)]
        public Guid Id { get; set; }

        /// <summary>
        /// ID biến thể sản phẩm (ProductVariant)
        /// </summary>
        [DisplayName("ID Biến thể sản phẩm")]
        [Display(Order = 0)]
        [Required(ErrorMessage = "ID biến thể sản phẩm không được để trống")]
        public Guid ProductVariantId { get; set; }

        /// <summary>
        /// Tên biến thể sản phẩm đầy đủ (từ ProductVariant.VariantFullName)
        /// </summary>
        [DisplayName("Tên biến thể sản phẩm")]
        [Display(Order = 1)]
        [StringLength(500, ErrorMessage = "Tên biến thể sản phẩm không được vượt quá 500 ký tự")]
        public string ProductVariantFullName { get; set; }

        /// <summary>
        /// Phân loại khách hàng
        /// </summary>
        [DisplayName("Phân loại khách hàng")]
        [Display(Order = 2)]
        [StringLength(255, ErrorMessage = "Phân loại khách hàng không được vượt quá 255 ký tự")]
        public string CustomerCategory { get; set; }

        #endregion

        #region Properties - Định danh sản phẩm

        /// <summary>
        /// Số Serial Number
        /// </summary>
        [DisplayName("Serial Number")]
        [Display(Order = 10)]
        [StringLength(100, ErrorMessage = "Serial Number không được vượt quá 100 ký tự")]
        public string SerialNumber { get; set; }

        /// <summary>
        /// Mã vạch
        /// </summary>
        [DisplayName("Part Number")]
        [Display(Order = 11)]
        [StringLength(255, ErrorMessage = "PartNumber không được vượt quá 255 ký tự")]
        public string PartNumber { get; set; }

        /// <summary>
        /// Mã QR
        /// </summary>
        [DisplayName("QR Code")]
        [Display(Order = 12)]
        [StringLength(500, ErrorMessage = "QR Code không được vượt quá 500 ký tự")]
        public string QRCode { get; set; }

        /// <summary>
        /// Stock Keeping Unit - Mã sản phẩm
        /// </summary>
        [DisplayName("SKU")]
        [Display(Order = 13)]
        [StringLength(100, ErrorMessage = "SKU không được vượt quá 100 ký tự")]
        public string SKU { get; set; }

        /// <summary>
        /// Radio Frequency Identification
        /// </summary>
        [DisplayName("RFID")]
        [Display(Order = 14)]
        [StringLength(100, ErrorMessage = "RFID không được vượt quá 100 ký tự")]
        public string RFID { get; set; }

        /// <summary>
        /// Media Access Control Address
        /// </summary>
        [DisplayName("MAC Address")]
        [Display(Order = 15)]
        [StringLength(50, ErrorMessage = "MAC Address không được vượt quá 50 ký tự")]
        public string MACAddress { get; set; }

        /// <summary>
        /// International Mobile Equipment Identity
        /// </summary>
        [DisplayName("IMEI")]
        [Display(Order = 16)]
        [StringLength(50, ErrorMessage = "IMEI không được vượt quá 50 ký tự")]
        public string IMEI { get; set; }

        /// <summary>
        /// Mã tài sản nội bộ
        /// </summary>
        [DisplayName("Asset Tag")]
        [Display(Order = 17)]
        [StringLength(50, ErrorMessage = "Asset Tag không được vượt quá 50 ký tự")]
        public string AssetTag { get; set; }

        /// <summary>
        /// Khóa bản quyền (cho phần mềm)
        /// </summary>
        [DisplayName("License Key")]
        [Display(Order = 18)]
        [StringLength(255, ErrorMessage = "License Key không được vượt quá 255 ký tự")]
        public string LicenseKey { get; set; }

        /// <summary>
        /// Universal Product Code
        /// </summary>
        [DisplayName("UPC")]
        [Display(Order = 19)]
        [StringLength(50, ErrorMessage = "UPC không được vượt quá 50 ký tự")]
        public string UPC { get; set; }

        /// <summary>
        /// European Article Number
        /// </summary>
        [DisplayName("EAN")]
        [Display(Order = 20)]
        [StringLength(50, ErrorMessage = "EAN không được vượt quá 50 ký tự")]
        public string EAN { get; set; }

        /// <summary>
        /// International Standard Book Number
        /// </summary>
        [DisplayName("ID")]
        [Display(Order = 21)]
        [StringLength(500, ErrorMessage = "ID không được vượt quá 500 ký tự")]
        public string ID { get; set; }

        /// <summary>
        /// Loại định danh khác
        /// </summary>
        [DisplayName("Định danh khác")]
        [Display(Order = 22)]
        [StringLength(255, ErrorMessage = "Định danh khác không được vượt quá 255 ký tự")]
        public string OtherIdentifier { get; set; }

        /// <summary>
        /// Thông tin định danh hiển thị dưới dạng HTML theo chuẩn DevExpress (chỉ đọc)
        /// Sử dụng các tag HTML chuẩn của DevExpress: &lt;b&gt;, &lt;i&gt;, &lt;color&gt;
        /// Tham khảo: https://docs.devexpress.com/WindowsForms/4874/common-features/html-text-formatting
        /// Hiển thị tất cả các định danh có giá trị dưới dạng HTML formatted
        /// </summary>
        [DisplayName("Thông tin định danh (HTML)")]
        [Display(Order = 23)]
        [Description("Thông tin định danh sản phẩm hiển thị dưới dạng HTML theo chuẩn DevExpress")]
        public string IdentifiersHtml
        {
            get
            {
                var html = string.Empty;

                // Danh sách các định danh theo thứ tự ưu tiên
                var identifiers = new[]
                {
                    (ProductVariantIdentifierEnum.SerialNumber, SerialNumber, "Serial Number"),
                    (ProductVariantIdentifierEnum.PartNumber, PartNumber, "Part Number"),
                    (ProductVariantIdentifierEnum.QRCode, QRCode, "QR Code"),
                    (ProductVariantIdentifierEnum.SKU, SKU, "SKU"),
                    (ProductVariantIdentifierEnum.RFID, RFID, "RFID"),
                    (ProductVariantIdentifierEnum.MACAddress, MACAddress, "MAC Address"),
                    (ProductVariantIdentifierEnum.IMEI, IMEI, "IMEI"),
                    (ProductVariantIdentifierEnum.AssetTag, AssetTag, "Asset Tag"),
                    (ProductVariantIdentifierEnum.LicenseKey, LicenseKey, "License Key"),
                    (ProductVariantIdentifierEnum.UPC, UPC, "UPC"),
                    (ProductVariantIdentifierEnum.EAN, EAN, "EAN"),
                    (ProductVariantIdentifierEnum.ID, ID, "ID"),
                    (ProductVariantIdentifierEnum.OtherIdentifier, OtherIdentifier, "Other")
                };

                var hasAnyIdentifier = false;
                var identifierCount = 0;

                foreach (var (identifierType, value, label) in identifiers)
                {
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        hasAnyIdentifier = true;
                        identifierCount++;

                        // Escape HTML special characters để an toàn
                        var escapedValue = value.Replace("&", "&amp;")
                                               .Replace("<", "&lt;")
                                               .Replace(">", "&gt;")
                                               .Replace("\"", "&quot;")
                                               .Replace("'", "&#39;");

                        // Format theo chuẩn DevExpress:
                        // - Label: màu xám (#757575)
                        // - Value: màu đen đậm (#212121) hoặc màu xanh (blue) cho các định danh quan trọng
                        // - Sử dụng <b> cho bold, <color> cho màu sắc
                        
                        // Xác định màu cho value dựa trên loại định danh
                        string valueColor = "#212121"; // Mặc định màu đen
                        if (identifierType == ProductVariantIdentifierEnum.SerialNumber || 
                            identifierType == ProductVariantIdentifierEnum.SKU ||
                            identifierType == ProductVariantIdentifierEnum.PartNumber)
                        {
                            valueColor = "blue"; // Màu xanh cho các định danh quan trọng
                        }

                        // Thêm separator nếu không phải định danh đầu tiên
                        if (identifierCount > 1)
                        {
                            html += "<br>";
                        }

                        // Format: Label (màu xám) | Value (màu đen/xanh, bold)
                        html += $"<color='#757575'>{label}:</color> <color='{valueColor}'><b>{escapedValue}</b></color>";
                    }
                }

                if (!hasAnyIdentifier)
                {
                    html = "<color='#757575'><i>Chưa có thông tin định danh</i></color>";
                }

                return html;
            }
        }

        #endregion

        #region Properties - Quản lý hình ảnh QR code

        /// <summary>
        /// Đường dẫn tương đối hình ảnh QR code (relative path trên NAS)
        /// </summary>
        [DisplayName("Đường dẫn hình ảnh QR")]
        [Display(Order = 30)]
        [StringLength(500, ErrorMessage = "Đường dẫn hình ảnh QR không được vượt quá 500 ký tự")]
        public string QRCodeImagePath { get; set; }

        /// <summary>
        /// Đường dẫn đầy đủ hình ảnh QR code (full UNC path)
        /// </summary>
        [DisplayName("Đường dẫn đầy đủ hình ảnh QR")]
        [Display(Order = 31)]
        [StringLength(1000, ErrorMessage = "Đường dẫn đầy đủ hình ảnh QR không được vượt quá 1000 ký tự")]
        public string QRCodeImageFullPath { get; set; }

        /// <summary>
        /// Tên file hình ảnh QR code
        /// </summary>
        [DisplayName("Tên file hình ảnh QR")]
        [Display(Order = 32)]
        [StringLength(255, ErrorMessage = "Tên file hình ảnh QR không được vượt quá 255 ký tự")]
        public string QRCodeImageFileName { get; set; }

        /// <summary>
        /// Loại lưu trữ: NAS, Local, Cloud
        /// </summary>
        [DisplayName("Loại lưu trữ")]
        [Display(Order = 33)]
        [StringLength(20, ErrorMessage = "Loại lưu trữ không được vượt quá 20 ký tự")]
        public string QRCodeImageStorageType { get; set; }

        /// <summary>
        /// Khóa hình ảnh QR code (không cho chỉnh sửa/xóa khi = true)
        /// </summary>
        [DisplayName("Khóa hình ảnh QR")]
        [Display(Order = 34)]
        public bool QRCodeImageLocked { get; set; }

        /// <summary>
        /// Ngày khóa hình ảnh QR code
        /// </summary>
        [DisplayName("Ngày khóa hình ảnh QR")]
        [Display(Order = 35)]
        public DateTime? QRCodeImageLockedDate { get; set; }

        /// <summary>
        /// Người khóa hình ảnh QR code (EmployeeId hoặc ApplicationUserId)
        /// </summary>
        [DisplayName("Người khóa hình ảnh QR")]
        [Display(Order = 36)]
        public Guid? QRCodeImageLockedBy { get; set; }

        #endregion

        #region Properties - Tình trạng hàng hóa/sản phẩm

        /// <summary>
        /// Tình trạng hàng hóa/sản phẩm
        /// </summary>
        [DisplayName("Tình trạng")]
        [Display(Order = 40)]
        [Required(ErrorMessage = "Tình trạng không được để trống")]
        public ProductVariantIdentifierStatusEnum Status { get; set; } = ProductVariantIdentifierStatusEnum.AtVnsWarehouse;

        /// <summary>
        /// Ngày thay đổi trạng thái
        /// </summary>
        [DisplayName("Ngày thay đổi trạng thái")]
        [Display(Order = 41)]
        public DateTime? StatusDate { get; set; }

        /// <summary>
        /// Người thay đổi trạng thái (EmployeeId hoặc ApplicationUserId)
        /// </summary>
        [DisplayName("Người thay đổi trạng thái")]
        [Display(Order = 42)]
        public Guid? StatusChangedBy { get; set; }

        /// <summary>
        /// Ghi chú về trạng thái
        /// </summary>
        [DisplayName("Ghi chú trạng thái")]
        [Display(Order = 43)]
        [StringLength(1000, ErrorMessage = "Ghi chú trạng thái không được vượt quá 1000 ký tự")]
        public string StatusNotes { get; set; }

        #endregion

        #region Properties - Thông tin khác

        /// <summary>
        /// Còn sử dụng không
        /// </summary>
        [DisplayName("Đang hoạt động")]
        [Display(Order = 50)]
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Nguồn: 0=Manual, 1=Import, 2=AutoGenerate, 3=Scanner, 4=Other
        /// </summary>
        [DisplayName("Nguồn")]
        [Display(Order = 51)]
        public int? SourceType { get; set; }

        /// <summary>
        /// Tham chiếu nguồn (ví dụ: file import, device scanner, v.v.)
        /// </summary>
        [DisplayName("Tham chiếu nguồn")]
        [Display(Order = 52)]
        [StringLength(255, ErrorMessage = "Tham chiếu nguồn không được vượt quá 255 ký tự")]
        public string SourceReference { get; set; }

        /// <summary>
        /// Ngày bắt đầu có hiệu lực
        /// </summary>
        [DisplayName("Ngày bắt đầu hiệu lực")]
        [Display(Order = 53)]
        public DateTime? ValidFrom { get; set; }

        /// <summary>
        /// Ngày hết hiệu lực
        /// </summary>
        [DisplayName("Ngày hết hiệu lực")]
        [Display(Order = 54)]
        public DateTime? ValidTo { get; set; }

        /// <summary>
        /// Ghi chú bổ sung
        /// </summary>
        [DisplayName("Ghi chú")]
        [Display(Order = 55)]
        [StringLength(1000, ErrorMessage = "Ghi chú không được vượt quá 1000 ký tự")]
        public string Notes { get; set; }

        #endregion

        #region Properties - Audit fields

        /// <summary>
        /// Ngày tạo
        /// </summary>
        [DisplayName("Ngày tạo")]
        [Display(Order = 100)]
        [Required(ErrorMessage = "Ngày tạo không được để trống")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Ngày cập nhật
        /// </summary>
        [DisplayName("Ngày cập nhật")]
        [Display(Order = 101)]
        public DateTime? UpdatedDate { get; set; }

        /// <summary>
        /// Người tạo (EmployeeId hoặc ApplicationUserId)
        /// </summary>
        [DisplayName("Người tạo")]
        [Display(Order = 102)]
        public Guid? CreatedBy { get; set; }

        /// <summary>
        /// Người cập nhật (EmployeeId hoặc ApplicationUserId)
        /// </summary>
        [DisplayName("Người cập nhật")]
        [Display(Order = 103)]
        public Guid? UpdatedBy { get; set; }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Lấy giá trị định danh theo ProductVariantIdentifierEnum
        /// </summary>
        /// <param name="identifierType">Loại định danh</param>
        /// <returns>Giá trị định danh hoặc null</returns>
        public string GetIdentifierValue(ProductVariantIdentifierEnum identifierType)
        {
            return identifierType switch
            {
                ProductVariantIdentifierEnum.SerialNumber => SerialNumber,
                ProductVariantIdentifierEnum.PartNumber => PartNumber,
                ProductVariantIdentifierEnum.QRCode => QRCode,
                ProductVariantIdentifierEnum.SKU => SKU,
                ProductVariantIdentifierEnum.RFID => RFID,
                ProductVariantIdentifierEnum.MACAddress => MACAddress,
                ProductVariantIdentifierEnum.IMEI => IMEI,
                ProductVariantIdentifierEnum.AssetTag => AssetTag,
                ProductVariantIdentifierEnum.LicenseKey => LicenseKey,
                ProductVariantIdentifierEnum.UPC => UPC,
                ProductVariantIdentifierEnum.EAN => EAN,
                ProductVariantIdentifierEnum.ID => ID,
                ProductVariantIdentifierEnum.OtherIdentifier => OtherIdentifier,
                _ => null
            };
        }

        /// <summary>
        /// Đặt giá trị định danh theo ProductVariantIdentifierEnum
        /// </summary>
        /// <param name="identifierType">Loại định danh</param>
        /// <param name="value">Giá trị cần đặt</param>
        public void SetIdentifierValue(ProductVariantIdentifierEnum identifierType, string value)
        {
            switch (identifierType)
            {
                case ProductVariantIdentifierEnum.SerialNumber:
                    SerialNumber = value;
                    break;
                case ProductVariantIdentifierEnum.PartNumber:
                    PartNumber = value;
                    break;
                case ProductVariantIdentifierEnum.QRCode:
                    QRCode = value;
                    break;
                case ProductVariantIdentifierEnum.SKU:
                    SKU = value;
                    break;
                case ProductVariantIdentifierEnum.RFID:
                    RFID = value;
                    break;
                case ProductVariantIdentifierEnum.MACAddress:
                    MACAddress = value;
                    break;
                case ProductVariantIdentifierEnum.IMEI:
                    IMEI = value;
                    break;
                case ProductVariantIdentifierEnum.AssetTag:
                    AssetTag = value;
                    break;
                case ProductVariantIdentifierEnum.LicenseKey:
                    LicenseKey = value;
                    break;
                case ProductVariantIdentifierEnum.UPC:
                    UPC = value;
                    break;
                case ProductVariantIdentifierEnum.EAN:
                    EAN = value;
                    break;
                case ProductVariantIdentifierEnum.ID:
                    ID = value;
                    break;
                case ProductVariantIdentifierEnum.OtherIdentifier:
                    OtherIdentifier = value;
                    break;
            }
        }

        /// <summary>
        /// Lấy danh sách tất cả các định danh có giá trị
        /// </summary>
        /// <returns>Dictionary chứa loại định danh và giá trị</returns>
        public Dictionary<ProductVariantIdentifierEnum, string> GetAllIdentifiers()
        {
            var identifiers = new Dictionary<ProductVariantIdentifierEnum, string>();

            if (!string.IsNullOrWhiteSpace(SerialNumber))
                identifiers[ProductVariantIdentifierEnum.SerialNumber] = SerialNumber;
            if (!string.IsNullOrWhiteSpace(PartNumber))
                identifiers[ProductVariantIdentifierEnum.PartNumber] = PartNumber;
            if (!string.IsNullOrWhiteSpace(QRCode))
                identifiers[ProductVariantIdentifierEnum.QRCode] = QRCode;
            if (!string.IsNullOrWhiteSpace(SKU))
                identifiers[ProductVariantIdentifierEnum.SKU] = SKU;
            if (!string.IsNullOrWhiteSpace(RFID))
                identifiers[ProductVariantIdentifierEnum.RFID] = RFID;
            if (!string.IsNullOrWhiteSpace(MACAddress))
                identifiers[ProductVariantIdentifierEnum.MACAddress] = MACAddress;
            if (!string.IsNullOrWhiteSpace(IMEI))
                identifiers[ProductVariantIdentifierEnum.IMEI] = IMEI;
            if (!string.IsNullOrWhiteSpace(AssetTag))
                identifiers[ProductVariantIdentifierEnum.AssetTag] = AssetTag;
            if (!string.IsNullOrWhiteSpace(LicenseKey))
                identifiers[ProductVariantIdentifierEnum.LicenseKey] = LicenseKey;
            if (!string.IsNullOrWhiteSpace(UPC))
                identifiers[ProductVariantIdentifierEnum.UPC] = UPC;
            if (!string.IsNullOrWhiteSpace(EAN))
                identifiers[ProductVariantIdentifierEnum.EAN] = EAN;
            if (!string.IsNullOrWhiteSpace(ID))
                identifiers[ProductVariantIdentifierEnum.ID] = ID;
            if (!string.IsNullOrWhiteSpace(OtherIdentifier))
                identifiers[ProductVariantIdentifierEnum.OtherIdentifier] = OtherIdentifier;

            return identifiers;
        }

        /// <summary>
        /// Kiểm tra xem có định danh nào được nhập không
        /// </summary>
        /// <returns>True nếu có ít nhất một định danh được nhập</returns>
        public bool HasAnyIdentifier()
        {
            return !string.IsNullOrWhiteSpace(SerialNumber) ||
                   !string.IsNullOrWhiteSpace(PartNumber) ||
                   !string.IsNullOrWhiteSpace(QRCode) ||
                   !string.IsNullOrWhiteSpace(SKU) ||
                   !string.IsNullOrWhiteSpace(RFID) ||
                   !string.IsNullOrWhiteSpace(MACAddress) ||
                   !string.IsNullOrWhiteSpace(IMEI) ||
                   !string.IsNullOrWhiteSpace(AssetTag) ||
                   !string.IsNullOrWhiteSpace(LicenseKey) ||
                   !string.IsNullOrWhiteSpace(UPC) ||
                   !string.IsNullOrWhiteSpace(EAN) ||
                   !string.IsNullOrWhiteSpace(ID) ||
                   !string.IsNullOrWhiteSpace(OtherIdentifier);
        }

        /// <summary>
        /// Kiểm tra xem hình ảnh QR code có bị khóa không
        /// </summary>
        /// <returns>True nếu hình ảnh bị khóa</returns>
        public bool IsQRCodeImageLocked()
        {
            return QRCodeImageLocked;
        }

        /// <summary>
        /// Kiểm tra xem có hình ảnh QR code không
        /// </summary>
        /// <returns>True nếu có hình ảnh QR code</returns>
        public bool HasQRCodeImage()
        {
            return !string.IsNullOrWhiteSpace(QRCodeImagePath) ||
                   !string.IsNullOrWhiteSpace(QRCodeImageFullPath) ||
                   !string.IsNullOrWhiteSpace(QRCodeImageFileName);
        }

        /// <summary>
        /// Lấy mô tả tình trạng
        /// </summary>
        /// <returns>Mô tả tình trạng</returns>
        public string GetStatusDescription()
        {
            var field = Status.GetType().GetField(Status.ToString());
            var attribute = field?.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
            return attribute?.Length > 0 ? attribute[0].Description : Status.ToString();
        }

        /// <summary>
        /// Cập nhật tình trạng với thông tin audit
        /// </summary>
        /// <param name="newStatus">Tình trạng mới</param>
        /// <param name="changedBy">Người thay đổi</param>
        /// <param name="notes">Ghi chú (tùy chọn)</param>
        public void UpdateStatus(ProductVariantIdentifierStatusEnum newStatus, Guid? changedBy, string notes = null)
        {
            Status = newStatus;
            StatusDate = DateTime.Now;
            StatusChangedBy = changedBy;
            if (!string.IsNullOrWhiteSpace(notes))
            {
                StatusNotes = notes;
            }
            UpdatedDate = DateTime.Now;
            UpdatedBy = changedBy;
        }

        #endregion
    }
}
