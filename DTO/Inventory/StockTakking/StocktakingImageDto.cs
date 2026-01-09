using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DTO.Inventory.StockTakking
{
    /// <summary>
    /// Data Transfer Object cho StocktakingImage entity
    /// Quản lý hình ảnh liên quan đến kiểm kho
    /// </summary>
    public class StocktakingImageDto
    {
        #region Properties - Thông tin cơ bản

        /// <summary>
        /// ID duy nhất của hình ảnh
        /// </summary>
        [DisplayName("ID")]
        [Display(Order = -1)]
        public Guid Id { get; set; }

        /// <summary>
        /// ID phiếu kiểm kho
        /// </summary>
        [DisplayName("ID Phiếu kiểm kho")]
        [Display(Order = 0)]
        public Guid? StocktakingMasterId { get; set; }

        /// <summary>
        /// ID chi tiết kiểm kho
        /// </summary>
        [DisplayName("ID Chi tiết kiểm kho")]
        [Display(Order = 1)]
        public Guid? StocktakingDetailId { get; set; }

        #endregion

        #region Properties - Dữ liệu hình ảnh

        /// <summary>
        /// Dữ liệu hình ảnh (binary)
        /// </summary>
        [DisplayName("Dữ liệu hình ảnh")]
        [Display(Order = 10)]
        [Description("Dữ liệu binary của hình ảnh")]
        public byte[] ImageData { get; set; }

        #endregion

        #region Properties - Thông tin file

        /// <summary>
        /// Tên file
        /// </summary>
        [DisplayName("Tên file")]
        [Display(Order = 20)]
        [StringLength(255, ErrorMessage = "Tên file không được vượt quá 255 ký tự")]
        public string FileName { get; set; }

        /// <summary>
        /// Đường dẫn tương đối
        /// </summary>
        [DisplayName("Đường dẫn tương đối")]
        [Display(Order = 21)]
        [StringLength(500, ErrorMessage = "Đường dẫn tương đối không được vượt quá 500 ký tự")]
        public string RelativePath { get; set; }

        /// <summary>
        /// Đường dẫn đầy đủ
        /// </summary>
        [DisplayName("Đường dẫn đầy đủ")]
        [Display(Order = 22)]
        [StringLength(1000, ErrorMessage = "Đường dẫn đầy đủ không được vượt quá 1000 ký tự")]
        public string FullPath { get; set; }

        /// <summary>
        /// Loại lưu trữ (NAS, Local, Cloud)
        /// </summary>
        [DisplayName("Loại lưu trữ")]
        [Display(Order = 23)]
        [StringLength(50, ErrorMessage = "Loại lưu trữ không được vượt quá 50 ký tự")]
        public string StorageType { get; set; }

        /// <summary>
        /// Kích thước file (bytes)
        /// </summary>
        [DisplayName("Kích thước file")]
        [Display(Order = 24)]
        public long? FileSize { get; set; }

        /// <summary>
        /// Phần mở rộng file
        /// </summary>
        [DisplayName("Phần mở rộng")]
        [Display(Order = 25)]
        [StringLength(10, ErrorMessage = "Phần mở rộng file không được vượt quá 10 ký tự")]
        public string FileExtension { get; set; }

        /// <summary>
        /// MIME type
        /// </summary>
        [DisplayName("MIME Type")]
        [Display(Order = 26)]
        [StringLength(100, ErrorMessage = "MIME type không được vượt quá 100 ký tự")]
        public string MimeType { get; set; }

        /// <summary>
        /// Checksum (MD5, SHA256, v.v.)
        /// </summary>
        [DisplayName("Checksum")]
        [Display(Order = 27)]
        [StringLength(100, ErrorMessage = "Checksum không được vượt quá 100 ký tự")]
        public string Checksum { get; set; }

        #endregion

        #region Properties - Trạng thái file

        /// <summary>
        /// File có tồn tại không
        /// </summary>
        [DisplayName("File tồn tại")]
        [Display(Order = 30)]
        public bool? FileExists { get; set; }

        /// <summary>
        /// Ngày xác minh lần cuối
        /// </summary>
        [DisplayName("Ngày xác minh")]
        [Display(Order = 31)]
        public DateTime? LastVerified { get; set; }

        /// <summary>
        /// Trạng thái migration
        /// </summary>
        [DisplayName("Trạng thái migration")]
        [Display(Order = 32)]
        [StringLength(50, ErrorMessage = "Trạng thái migration không được vượt quá 50 ký tự")]
        public string MigrationStatus { get; set; }

        #endregion

        #region Properties - Audit fields

        /// <summary>
        /// Ngày tạo
        /// </summary>
        [DisplayName("Ngày tạo")]
        [Display(Order = 100)]
        [Required(ErrorMessage = "Ngày tạo không được để trống")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        [DisplayName("Người tạo")]
        [Display(Order = 101)]
        [Required(ErrorMessage = "Người tạo không được để trống")]
        public Guid CreateBy { get; set; }

        /// <summary>
        /// Ngày cập nhật
        /// </summary>
        [DisplayName("Ngày cập nhật")]
        [Display(Order = 102)]
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Người cập nhật
        /// </summary>
        [DisplayName("Người cập nhật")]
        [Display(Order = 103)]
        [Required(ErrorMessage = "Người cập nhật không được để trống")]
        public Guid ModifiedBy { get; set; }

        #endregion

        #region Properties - HTML Display

        /// <summary>
        /// Thông tin file dưới dạng HTML theo format DevExpress
        /// Sử dụng các tag HTML chuẩn của DevExpress: &lt;b&gt;, &lt;i&gt;, &lt;color&gt;
        /// Tham khảo: https://docs.devexpress.com/WindowsForms/4874/common-features/html-text-formatting
        /// </summary>
        [DisplayName("Thông tin file HTML")]
        [Display(Order = 200)]
        [Description("Thông tin file dưới dạng HTML")]
        public string FileInfoHtml
        {
            get
            {
                var htmlParts = new List<string>();

                // Tên file (nổi bật nhất)
                if (!string.IsNullOrWhiteSpace(FileName))
                {
                    htmlParts.Add($"<b><color='blue'>{EscapeHtml(FileName)}</color></b>");
                }
                else
                {
                    htmlParts.Add("<color='#757575'><i>Chưa có tên file</i></color>");
                }

                // Kích thước file
                if (FileSize.HasValue)
                {
                    htmlParts.Add("<br>");
                    var fileSizeStr = FormatFileSize(FileSize.Value);
                    htmlParts.Add($"<color='#757575'>Kích thước:</color> <color='#212121'><b>{fileSizeStr}</b></color>");
                }

                // Loại file
                if (!string.IsNullOrWhiteSpace(FileExtension))
                {
                    htmlParts.Add("<br>");
                    htmlParts.Add($"<color='#757575'>Loại:</color> <color='#212121'><b>{EscapeHtml(FileExtension)}</b></color>");
                }

                // MIME type
                if (!string.IsNullOrWhiteSpace(MimeType))
                {
                    htmlParts.Add("<br>");
                    htmlParts.Add($"<color='#757575'>MIME:</color> <color='#212121'><i>{EscapeHtml(MimeType)}</i></color>");
                }

                // Đường dẫn
                if (!string.IsNullOrWhiteSpace(RelativePath) || !string.IsNullOrWhiteSpace(FullPath))
                {
                    htmlParts.Add("<br>");
                    var path = !string.IsNullOrWhiteSpace(RelativePath) ? RelativePath : FullPath;
                    htmlParts.Add($"<color='#757575'>Đường dẫn:</color> <color='#212121'><i>{EscapeHtml(path)}</i></color>");
                }

                // Loại lưu trữ
                if (!string.IsNullOrWhiteSpace(StorageType))
                {
                    htmlParts.Add("<br>");
                    htmlParts.Add($"<color='#757575'>Lưu trữ:</color> <color='#212121'><b>{EscapeHtml(StorageType)}</b></color>");
                }

                // Trạng thái file
                if (FileExists.HasValue)
                {
                    htmlParts.Add("<br>");
                    if (FileExists.Value)
                    {
                        htmlParts.Add($"<color='#4CAF50'><b>✓ File tồn tại</b></color>");
                    }
                    else
                    {
                        htmlParts.Add($"<color='#F44336'><b>✗ File không tồn tại</b></color>");
                    }
                    if (LastVerified.HasValue)
                    {
                        htmlParts.Add($" <color='#757575'>(Xác minh: {LastVerified.Value:dd/MM/yyyy})</color>");
                    }
                }

                // Ngày tạo
                if (CreateDate != default(DateTime))
                {
                    htmlParts.Add("<br>");
                    htmlParts.Add($"<color='#757575'>Ngày tạo:</color> <color='#212121'><b>{CreateDate:dd/MM/yyyy HH:mm}</b></color>");
                }

                return string.Join(string.Empty, htmlParts);
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Escape HTML special characters
        /// </summary>
        private string EscapeHtml(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            return value.Replace("&", "&amp;")
                       .Replace("<", "&lt;")
                       .Replace(">", "&gt;")
                       .Replace("\"", "&quot;")
                       .Replace("'", "&#39;");
        }

        /// <summary>
        /// Format file size to human readable format
        /// </summary>
        private string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }

        #endregion
    }
}
