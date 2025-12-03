using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DTO.Inventory.StockIn;

namespace DTO.Inventory.InventoryManagement;

/// <summary>
/// Data Transfer Object cho chứng từ nhập/xuất kho
/// Dùng cho Query và truyền dữ liệu giữa Service ↔ WinForms
/// Map với bảng StockInOutDocument trong database
/// </summary>
public class StockInOutDocumentDto
{
    #region Properties - Thông tin cơ bản

    /// <summary>
    /// ID duy nhất của chứng từ
    /// Map với: StockInOutDocument.Id
    /// </summary>
    [DisplayName("ID")]
    [Display(Order = -1)]
    public Guid Id { get; set; }

    /// <summary>
    /// ID phiếu nhập/xuất kho (Master)
    /// Map với: StockInOutDocument.StockInOutMasterId
    /// </summary>
    [DisplayName("ID Phiếu nhập/xuất")]
    [Display(Order = 1)]
    public Guid? StockInOutMasterId { get; set; }

    /// <summary>
    /// ID đối tác
    /// Map với: StockInOutDocument.BusinessPartnerId
    /// </summary>
    [DisplayName("ID Đối tác")]
    [Display(Order = 2)]
    public Guid? BusinessPartnerId { get; set; }

    #endregion

    #region Properties - Thông tin phiếu nhập/xuất (từ StockInOutMaster)

    /// <summary>
    /// Số phiếu nhập/xuất kho
    /// Map với: StockInOutMaster.VocherNumber
    /// </summary>
    [DisplayName("Số phiếu")]
    [Display(Order = 3)]
    [StringLength(50, ErrorMessage = "Số phiếu không được vượt quá 50 ký tự")]
    public string VocherNumber { get; set; }

    /// <summary>
    /// Ngày tháng phiếu nhập/xuất kho
    /// Map với: StockInOutMaster.StockInOutDate
    /// </summary>
    [DisplayName("Ngày tháng")]
    [Display(Order = 4)]
    public DateTime? StockInOutDate { get; set; }

    /// <summary>
    /// Loại nhập/xuất kho
    /// Map với: StockInOutMaster.StockInOutType
    /// </summary>
    [DisplayName("Loại nhập/xuất")]
    [Display(Order = 5)]
    public LoaiNhapXuatKhoEnum? LoaiNhapXuatKho { get; set; }

    /// <summary>
    /// Tên loại nhập/xuất kho (hiển thị)
    /// </summary>
    [DisplayName("Loại nhập/xuất (text)")]
    [Display(Order = 6)]
    public string LoaiNhapXuatKhoText { get; set; }

    /// <summary>
    /// Thông tin khách hàng/đối tác (nếu có)
    /// </summary>
    [DisplayName("Khách hàng/Đối tác")]
    [Display(Order = 7)]
    [StringLength(255, ErrorMessage = "Thông tin khách hàng không được vượt quá 255 ký tự")]
    public string CustomerInfo { get; set; }

    #endregion

    #region Properties - Thông tin chứng từ

    /// <summary>
    /// Loại chứng từ
    /// Map với: StockInOutDocument.DocumentType
    /// </summary>
    [DisplayName("Loại chứng từ")]
    [Display(Order = 10)]
    public int DocumentType { get; set; }

    /// <summary>
    /// Tên loại chứng từ (hiển thị)
    /// </summary>
    [DisplayName("Loại chứng từ (text)")]
    [Display(Order = 11)]
    public string DocumentTypeText { get; set; }

    /// <summary>
    /// Danh mục chứng từ
    /// Map với: StockInOutDocument.DocumentCategory
    /// </summary>
    [DisplayName("Danh mục")]
    [Display(Order = 12)]
    public int? DocumentCategory { get; set; }

    /// <summary>
    /// Tên danh mục chứng từ (hiển thị)
    /// </summary>
    [DisplayName("Danh mục (text)")]
    [Display(Order = 13)]
    public string DocumentCategoryText { get; set; }

    /// <summary>
    /// Số chứng từ
    /// Map với: StockInOutDocument.DocumentNumber
    /// </summary>
    [DisplayName("Số chứng từ")]
    [Display(Order = 14)]
    [StringLength(100, ErrorMessage = "Số chứng từ không được vượt quá 100 ký tự")]
    public string DocumentNumber { get; set; }

    /// <summary>
    /// Ngày chứng từ
    /// Map với: StockInOutDocument.DocumentDate
    /// </summary>
    [DisplayName("Ngày chứng từ")]
    [Display(Order = 15)]
    public DateTime? DocumentDate { get; set; }

    /// <summary>
    /// Ngày phát hành
    /// Map với: StockInOutDocument.IssueDate
    /// </summary>
    [DisplayName("Ngày phát hành")]
    [Display(Order = 16)]
    public DateTime? IssueDate { get; set; }

    /// <summary>
    /// Ngày hết hạn
    /// Map với: StockInOutDocument.ExpiryDate
    /// </summary>
    [DisplayName("Ngày hết hạn")]
    [Display(Order = 17)]
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// Số tiền (nếu là chứng từ tài chính)
    /// Map với: StockInOutDocument.Amount
    /// </summary>
    [DisplayName("Số tiền")]
    [Display(Order = 18)]
    public decimal? Amount { get; set; }

    /// <summary>
    /// Loại tiền tệ
    /// Map với: StockInOutDocument.Currency
    /// </summary>
    [DisplayName("Loại tiền tệ")]
    [Display(Order = 19)]
    [StringLength(10, ErrorMessage = "Loại tiền tệ không được vượt quá 10 ký tự")]
    public string Currency { get; set; }

    #endregion

    #region Properties - Thông tin file

    /// <summary>
    /// Tên file gốc
    /// Map với: StockInOutDocument.FileName
    /// </summary>
    [DisplayName("Tên file")]
    [Display(Order = 20)]
    [StringLength(255, ErrorMessage = "Tên file không được vượt quá 255 ký tự")]
    public string FileName { get; set; }

    /// <summary>
    /// Tên hiển thị
    /// Map với: StockInOutDocument.DisplayName
    /// </summary>
    [DisplayName("Tên hiển thị")]
    [Display(Order = 21)]
    [StringLength(255, ErrorMessage = "Tên hiển thị không được vượt quá 255 ký tự")]
    public string DisplayName { get; set; }

    /// <summary>
    /// Mô tả
    /// Map với: StockInOutDocument.Description
    /// </summary>
    [DisplayName("Mô tả")]
    [Display(Order = 22)]
    [StringLength(1000, ErrorMessage = "Mô tả không được vượt quá 1000 ký tự")]
    public string Description { get; set; }

    /// <summary>
    /// Đường dẫn tương đối của file
    /// Map với: StockInOutDocument.RelativePath
    /// </summary>
    [DisplayName("Đường dẫn tương đối")]
    [Display(Order = 23)]
    [StringLength(500, ErrorMessage = "Đường dẫn tương đối không được vượt quá 500 ký tự")]
    public string RelativePath { get; set; }

    /// <summary>
    /// Đường dẫn đầy đủ của file
    /// Map với: StockInOutDocument.FullPath
    /// </summary>
    [DisplayName("Đường dẫn đầy đủ")]
    [Display(Order = 24)]
    [StringLength(1000, ErrorMessage = "Đường dẫn đầy đủ không được vượt quá 1000 ký tự")]
    public string FullPath { get; set; }

    /// <summary>
    /// Loại lưu trữ (NAS, Local, Cloud, etc.)
    /// Map với: StockInOutDocument.StorageType
    /// </summary>
    [DisplayName("Loại lưu trữ")]
    [Display(Order = 25)]
    [StringLength(20, ErrorMessage = "Loại lưu trữ không được vượt quá 20 ký tự")]
    public string StorageType { get; set; }

    /// <summary>
    /// Kích thước file (bytes)
    /// Map với: StockInOutDocument.FileSize
    /// </summary>
    [DisplayName("Kích thước file (bytes)")]
    [Display(Order = 26)]
    public long? FileSize { get; set; }

    /// <summary>
    /// Kích thước file (hiển thị dạng KB/MB)
    /// </summary>
    [DisplayName("Kích thước file")]
    [Display(Order = 27)]
    public string FileSizeDisplay
    {
        get
        {
            if (!FileSize.HasValue || FileSize.Value == 0)
                return "0 KB";

            var size = FileSize.Value;
            if (size < 1024)
                return $"{size} bytes";
            if (size < 1024 * 1024)
                return $"{size / 1024.0:F2} KB";
            return $"{size / (1024.0 * 1024.0):F2} MB";
        }
    }

    /// <summary>
    /// Phần mở rộng file
    /// Map với: StockInOutDocument.FileExtension
    /// </summary>
    [DisplayName("Phần mở rộng")]
    [Display(Order = 28)]
    [StringLength(10, ErrorMessage = "Phần mở rộng không được vượt quá 10 ký tự")]
    public string FileExtension { get; set; }

    /// <summary>
    /// MIME type
    /// Map với: StockInOutDocument.MimeType
    /// </summary>
    [DisplayName("MIME type")]
    [Display(Order = 29)]
    [StringLength(100, ErrorMessage = "MIME type không được vượt quá 100 ký tự")]
    public string MimeType { get; set; }

    #endregion

    #region Properties - Trạng thái và bảo mật

    /// <summary>
    /// File có tồn tại trên storage không
    /// Map với: StockInOutDocument.FileExists
    /// </summary>
    [DisplayName("File tồn tại")]
    [Display(Order = 30)]
    public bool? FileExists { get; set; }

    /// <summary>
    /// Đã được xác minh chưa
    /// Map với: StockInOutDocument.IsVerified
    /// </summary>
    [DisplayName("Đã xác minh")]
    [Display(Order = 31)]
    public bool? IsVerified { get; set; }

    /// <summary>
    /// Mức độ truy cập
    /// Map với: StockInOutDocument.AccessLevel
    /// </summary>
    [DisplayName("Mức độ truy cập")]
    [Display(Order = 32)]
    public int? AccessLevel { get; set; }

    /// <summary>
    /// Tên mức độ truy cập (hiển thị)
    /// </summary>
    [DisplayName("Mức độ truy cập (text)")]
    [Display(Order = 33)]
    public string AccessLevelText { get; set; }

    /// <summary>
    /// Có phải tài liệu mật không
    /// Map với: StockInOutDocument.IsConfidential
    /// </summary>
    [DisplayName("Tài liệu mật")]
    [Display(Order = 34)]
    public bool? IsConfidential { get; set; }

    #endregion

    #region Properties - Audit

    /// <summary>
    /// Ngày tạo
    /// Map với: StockInOutDocument.CreateDate
    /// </summary>
    [DisplayName("Ngày tạo")]
    [Display(Order = 40)]
    public DateTime CreateDate { get; set; }

    /// <summary>
    /// Người tạo
    /// Map với: StockInOutDocument.CreateBy
    /// </summary>
    [DisplayName("Người tạo")]
    [Display(Order = 41)]
    public Guid CreateBy { get; set; }

    /// <summary>
    /// Ngày sửa
    /// Map với: StockInOutDocument.ModifiedDate
    /// </summary>
    [DisplayName("Ngày sửa")]
    [Display(Order = 42)]
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// Trạng thái hoạt động
    /// Map với: StockInOutDocument.IsActive
    /// </summary>
    [DisplayName("Hoạt động")]
    [Display(Order = 43)]
    public bool IsActive { get; set; }

    #endregion

    #region Helper Properties

    /// <summary>
    /// Caption hiển thị trên UI: "Tên hiển thị (Số chứng từ)"
    /// </summary>
    [DisplayName("Caption")]
    [Display(Order = 50)]
    public string DisplayCaption
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(DisplayName))
            {
                if (!string.IsNullOrWhiteSpace(DocumentNumber))
                    return $"{DisplayName} ({DocumentNumber})";
                return DisplayName;
            }

            if (!string.IsNullOrWhiteSpace(FileName))
            {
                if (!string.IsNullOrWhiteSpace(DocumentNumber))
                    return $"{FileName} ({DocumentNumber})";
                return FileName;
            }

            return DocumentNumber ?? "Không có tên";
        }
    }

    /// <summary>
    /// Group Caption để nhóm các chứng từ: "DocumentDate - DocumentTypeText - DocumentNumber"
    /// </summary>
    [DisplayName("Group Caption")]
    [Display(Order = 51)]
    public string GroupCaption
    {
        get
        {
            var parts = new System.Collections.Generic.List<string>();

            // DocumentDate
            if (DocumentDate.HasValue)
            {
                parts.Add(DocumentDate.Value.ToString("dd/MM/yyyy"));
            }
            else if (StockInOutDate.HasValue)
            {
                parts.Add(StockInOutDate.Value.ToString("dd/MM/yyyy"));
            }

            // DocumentTypeText
            if (!string.IsNullOrWhiteSpace(DocumentTypeText))
            {
                parts.Add(DocumentTypeText);
            }

            // DocumentNumber
            if (!string.IsNullOrWhiteSpace(DocumentNumber))
            {
                parts.Add(DocumentNumber);
            }
            else if (!string.IsNullOrWhiteSpace(VocherNumber))
            {
                parts.Add(VocherNumber);
            }

            return parts.Count > 0 ? string.Join(" - ", parts) : "Không có thông tin";
        }
    }

    #endregion
}

