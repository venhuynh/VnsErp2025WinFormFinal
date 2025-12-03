using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DTO.Inventory.Query
{
    /// <summary>
    /// Data Transfer Object cho hình ảnh nhập/xuất kho
    /// Dùng cho Query và truyền dữ liệu giữa Service ↔ WinForms
    /// Map với bảng StockInOutImage trong database
    /// </summary>
    public class StockInOutImageDto
    {
        #region Properties - Thông tin cơ bản

        /// <summary>
        /// ID duy nhất của hình ảnh
        /// Map với: StockInOutImage.Id
        /// </summary>
        [DisplayName("ID")]
        [Display(Order = -1)]
        public Guid Id { get; set; }

        /// <summary>
        /// ID phiếu nhập/xuất kho (Master)
        /// Map với: StockInOutImage.StockInOutMasterId
        /// </summary>
        [DisplayName("ID Phiếu nhập/xuất")]
        [Display(Order = 1)]
        [Required(ErrorMessage = "ID phiếu nhập/xuất không được để trống")]
        public Guid StockInOutMasterId { get; set; }

        /// <summary>
        /// Dữ liệu hình ảnh (byte array)
        /// Map với: StockInOutImage.ImageData
        /// </summary>
        [DisplayName("Dữ liệu hình ảnh")]
        [Display(Order = 2)]
        public byte[] ImageData { get; set; }

        #endregion

        #region Properties - Thông tin file

        /// <summary>
        /// Tên file hình ảnh
        /// Map với: StockInOutImage.FileName
        /// </summary>
        [DisplayName("Tên file")]
        [Display(Order = 3)]
        [StringLength(255, ErrorMessage = "Tên file không được vượt quá 255 ký tự")]
        public string FileName { get; set; }

        /// <summary>
        /// Đường dẫn tương đối của file
        /// Map với: StockInOutImage.RelativePath
        /// </summary>
        [DisplayName("Đường dẫn tương đối")]
        [Display(Order = 4)]
        [StringLength(500, ErrorMessage = "Đường dẫn tương đối không được vượt quá 500 ký tự")]
        public string RelativePath { get; set; }

        /// <summary>
        /// Đường dẫn đầy đủ của file
        /// Map với: StockInOutImage.FullPath
        /// </summary>
        [DisplayName("Đường dẫn đầy đủ")]
        [Display(Order = 5)]
        [StringLength(1000, ErrorMessage = "Đường dẫn đầy đủ không được vượt quá 1000 ký tự")]
        public string FullPath { get; set; }

        /// <summary>
        /// Loại lưu trữ (Database, FileSystem, NAS, Cloud, etc.)
        /// Map với: StockInOutImage.StorageType
        /// </summary>
        [DisplayName("Loại lưu trữ")]
        [Display(Order = 6)]
        [StringLength(20, ErrorMessage = "Loại lưu trữ không được vượt quá 20 ký tự")]
        public string StorageType { get; set; }

        /// <summary>
        /// Kích thước file (bytes)
        /// Map với: StockInOutImage.FileSize
        /// </summary>
        [DisplayName("Kích thước file (bytes)")]
        [Display(Order = 7)]
        public long? FileSize { get; set; }

        /// <summary>
        /// Phần mở rộng của file (.jpg, .png, .pdf, etc.)
        /// Map với: StockInOutImage.FileExtension
        /// </summary>
        [DisplayName("Phần mở rộng")]
        [Display(Order = 8)]
        [StringLength(10, ErrorMessage = "Phần mở rộng không được vượt quá 10 ký tự")]
        public string FileExtension { get; set; }

        /// <summary>
        /// MIME type của file (image/jpeg, image/png, application/pdf, etc.)
        /// Map với: StockInOutImage.MimeType
        /// </summary>
        [DisplayName("MIME Type")]
        [Display(Order = 9)]
        [StringLength(100, ErrorMessage = "MIME type không được vượt quá 100 ký tự")]
        public string MimeType { get; set; }

        /// <summary>
        /// Checksum/MD5 hash của file để kiểm tra tính toàn vẹn
        /// Map với: StockInOutImage.Checksum
        /// </summary>
        [DisplayName("Checksum")]
        [Display(Order = 10)]
        [StringLength(64, ErrorMessage = "Checksum không được vượt quá 64 ký tự")]
        public string Checksum { get; set; }

        /// <summary>
        /// Trạng thái file có tồn tại hay không
        /// Map với: StockInOutImage.FileExists
        /// </summary>
        [DisplayName("File tồn tại")]
        [Display(Order = 11)]
        public bool? FileExists { get; set; }

        /// <summary>
        /// Ngày giờ lần cuối kiểm tra file
        /// Map với: StockInOutImage.LastVerified
        /// </summary>
        [DisplayName("Ngày kiểm tra cuối")]
        [Display(Order = 12)]
        public DateTime? LastVerified { get; set; }

        /// <summary>
        /// Trạng thái migration (Pending, Completed, Failed, etc.)
        /// Map với: StockInOutImage.MigrationStatus
        /// </summary>
        [DisplayName("Trạng thái migration")]
        [Display(Order = 13)]
        [StringLength(20, ErrorMessage = "Trạng thái migration không được vượt quá 20 ký tự")]
        public string MigrationStatus { get; set; }

        #endregion

        #region Properties - Thông tin hệ thống

        /// <summary>
        /// Ngày tạo
        /// Map với: StockInOutImage.CreateDate
        /// </summary>
        [DisplayName("Ngày tạo")]
        [Display(Order = 20)]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// ID người tạo
        /// Map với: StockInOutImage.CreateBy
        /// </summary>
        [DisplayName("ID Người tạo")]
        [Display(Order = 21)]
        public Guid CreateBy { get; set; }

        /// <summary>
        /// Ngày sửa đổi
        /// Map với: StockInOutImage.ModifiedDate
        /// </summary>
        [DisplayName("Ngày sửa")]
        [Display(Order = 22)]
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// ID người sửa đổi
        /// Map với: StockInOutImage.ModifiedBy
        /// </summary>
        [DisplayName("ID Người sửa")]
        [Display(Order = 23)]
        public Guid ModifiedBy { get; set; }

        #endregion

        #region Helper Properties

        /// <summary>
        /// Hiển thị kích thước file dạng đọc được (KB, MB, GB)
        /// </summary>
        [DisplayName("Kích thước")]
        [Display(Order = 7)]
        public string FileSizeDisplay
        {
            get
            {
                if (!FileSize.HasValue || FileSize.Value == 0)
                    return "0 B";

                string[] sizes = ["B", "KB", "MB", "GB", "TB"];
                double len = FileSize.Value;
                int order = 0;
                while (len >= 1024 && order < sizes.Length - 1)
                {
                    order++;
                    len /= 1024;
                }

                return $"{len:0.##} {sizes[order]}";
            }
        }

        /// <summary>
        /// Kiểm tra xem file có tồn tại hay không (trả về true nếu FileExists = true)
        /// </summary>
        [DisplayName("Có file")]
        [Display(Order = 11)]
        public bool HasFile => FileExists == true;

        #endregion
    }
}
