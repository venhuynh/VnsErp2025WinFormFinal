using DTO.Inventory.InventoryManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DTO.Inventory.StockOut.XuatLapRap
{

    /// <summary>
    /// Data Transfer Object cho danh sách phiếu xuất lắp ráp
    /// Dùng cho GridControl (danh sách)
    /// </summary>
    public class XuatLapRapMasterListDto
    {
        #region Properties - Thông tin cơ bản

        /// <summary>
        /// ID duy nhất của phiếu xuất kho
        /// </summary>
        [DisplayName("ID")]
        [Display(Order = -1)]
        public Guid Id { get; set; }

        /// <summary>
        /// Số phiếu xuất kho
        /// </summary>
        [DisplayName("Số phiếu")]
        [Display(Order = 1)]
        public string StockOutNumber { get; set; }

        /// <summary>
        /// Ngày xuất kho
        /// </summary>
        [DisplayName("Ngày xuất")]
        [Display(Order = 2)]
        public DateTime StockOutDate { get; set; }

        /// <summary>
        /// Loại xuất kho
        /// </summary>
        [DisplayName("Loại xuất")]
        [Display(Order = 3)]
        public LoaiNhapXuatKhoEnum LoaiNhapXuatKho { get; set; }

        /// <summary>
        /// Trạng thái phiếu xuất
        /// </summary>
        [DisplayName("Trạng thái")]
        [Display(Order = 4)]
        public TrangThaiPhieuNhapEnum TrangThai { get; set; }

        #endregion

        #region Properties - Thông tin liên kết

        /// <summary>
        /// Mã kho xuất hàng
        /// </summary>
        [DisplayName("Mã kho")]
        [Display(Order = 10)]
        public string WarehouseCode { get; set; }

        /// <summary>
        /// Tên kho xuất hàng
        /// </summary>
        [DisplayName("Tên kho")]
        [Display(Order = 11)]
        public string WarehouseName { get; set; }

        #endregion

        #region Properties - Tổng hợp

        /// <summary>
        /// Tổng số lượng xuất
        /// </summary>
        [DisplayName("Tổng SL")]
        [Display(Order = 20)]
        public decimal TotalQuantity { get; set; }

        #endregion

        #region Properties - Thông tin hệ thống

        /// <summary>
        /// Tên người tạo
        /// </summary>
        [DisplayName("Người tạo")]
        [Display(Order = 30)]
        public string CreatedByName { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        [DisplayName("Ngày tạo")]
        [Display(Order = 31)]
        public DateTime? CreatedDate { get; set; }

        #endregion

        #region Properties - HTML Display

        /// <summary>
        /// Thông tin phiếu xuất dưới dạng HTML theo format DevExpress
        /// Sử dụng các tag HTML chuẩn của DevExpress: &lt;b&gt;, &lt;i&gt;, &lt;color&gt;, &lt;size&gt;
        /// Tham khảo: https://docs.devexpress.com/WindowsForms/4874/common-features/html-text-formatting
        /// Dùng cho SearchLookUpEdit để hiển thị thông tin đầy đủ của phiếu xuất
        /// </summary>
        [DisplayName("Thông tin phiếu xuất HTML")]
        [Description("Thông tin phiếu xuất dưới dạng HTML")]
        public string StockOutInfoHtml
        {
            get
            {
                var stockOutNumber = StockOutNumber ?? string.Empty;
                var stockOutDate = StockOutDate.ToString("dd/MM/yyyy");
                var warehouseName = WarehouseName ?? string.Empty;
                var warehouseCode = WarehouseCode ?? string.Empty;
                var totalQuantity = TotalQuantity.ToString("N2");
                var createdByName = CreatedByName ?? string.Empty;
                var statusText = TrangThai switch
                {
                    TrangThaiPhieuNhapEnum.TaoMoi => "Tạo mới",
                    TrangThaiPhieuNhapEnum.ChoDuyet => "Chờ duyệt",
                    TrangThaiPhieuNhapEnum.DaDuyet => "Đã duyệt",
                    TrangThaiPhieuNhapEnum.DangNhapKho => "Đang nhập kho",
                    TrangThaiPhieuNhapEnum.DaNhapKho => "Đã nhập kho",
                    _ => "Không xác định"
                };
                var statusColor = TrangThai switch
                {
                    TrangThaiPhieuNhapEnum.TaoMoi => "#FF9800",      // Orange - Tạo mới
                    TrangThaiPhieuNhapEnum.ChoDuyet => "#2196F3",   // Blue - Chờ duyệt
                    TrangThaiPhieuNhapEnum.DaDuyet => "#4CAF50",     // Green - Đã duyệt
                    TrangThaiPhieuNhapEnum.DangNhapKho => "#9C27B0", // Purple - Đang nhập kho
                    TrangThaiPhieuNhapEnum.DaNhapKho => "#4CAF50",   // Green - Đã nhập kho
                    _ => "#757575"                                    // Gray - Khác
                };

                // Format chuyên nghiệp với visual hierarchy rõ ràng
                // - Số phiếu: font lớn, bold, màu xanh đậm (primary)
                // - Ngày xuất: font nhỏ hơn, màu xám
                // - Kho xuất: hiển thị tên và mã kho
                // - Tổng số lượng: highlight
                // - Trạng thái: highlight với màu tương ứng
                // - Người tạo: thông tin bổ sung

                var html = $"<b><color='blue'>{stockOutNumber}</color></b>";

                html += $" <color='#757575'>({stockOutDate})</color>";

                html += "<br>";

                var infoParts = new List<string>();

                if (!string.IsNullOrWhiteSpace(warehouseName))
                {
                    var warehouseInfo = warehouseName;
                    if (!string.IsNullOrWhiteSpace(warehouseCode))
                    {
                        warehouseInfo += $" ({warehouseCode})";
                    }
                    infoParts.Add($"<color='#757575'>Kho:</color> <b>{warehouseInfo}</b>");
                }

                if (TotalQuantity > 0)
                {
                    infoParts.Add($"<color='#757575'>SL:</color> <b><color='#2196F3'>{totalQuantity}</color></b>");
                }

                if (infoParts.Any())
                {
                    html += string.Join(" | ", infoParts) + "<br>";
                }

                html += $"<color='#757575'>Trạng thái:</color> <color='{statusColor}'><b>{statusText}</b></color>";

                if (!string.IsNullOrWhiteSpace(createdByName))
                {
                    html += $" | <color='#757575'>Người tạo:</color> <b>{createdByName}</b>";
                }

                return html;
            }
        }

        #endregion
    }

    /// <summary>
    /// Data Transfer Object cho phiếu xuất lắp ráp (chi tiết)
    /// Dùng cho form nhập/sửa phiếu xuất kho
    /// </summary>
    public class XuatLapRapMasterDto
    {
        #region Properties - Thông tin cơ bản

        /// <summary>
        /// ID duy nhất của phiếu xuất kho
        /// </summary>
        [DisplayName("ID")]
        [Display(Order = -1)]
        public Guid Id { get; set; }

        /// <summary>
        /// Số phiếu xuất kho
        /// </summary>
        [DisplayName("Số phiếu xuất")]
        [Display(Order = 1)]
        [Required(ErrorMessage = "Số phiếu xuất không được để trống")]
        [StringLength(50, ErrorMessage = "Số phiếu xuất không được vượt quá 50 ký tự")]
        public string StockOutNumber { get; set; }

        /// <summary>
        /// Ngày xuất kho
        /// </summary>
        [DisplayName("Ngày xuất")]
        [Display(Order = 2)]
        [Required(ErrorMessage = "Ngày xuất không được để trống")]
        public DateTime StockOutDate { get; set; }

        /// <summary>
        /// Loại xuất kho
        /// </summary>
        [DisplayName("Loại xuất")]
        [Display(Order = 3)]
        [Required(ErrorMessage = "Loại xuất kho không được để trống")]
        public LoaiNhapXuatKhoEnum LoaiNhapXuatKho { get; set; }

        /// <summary>
        /// Trạng thái phiếu xuất
        /// </summary>
        [DisplayName("Trạng thái")]
        [Display(Order = 4)]
        public TrangThaiPhieuNhapEnum TrangThai { get; set; }

        #endregion

        #region Properties - Thông tin liên kết

        /// <summary>
        /// ID kho xuất hàng
        /// </summary>
        [DisplayName("ID Kho")]
        [Display(Order = 10)]
        [Required(ErrorMessage = "Kho xuất không được để trống")]
        public Guid WarehouseId { get; set; }

        /// <summary>
        /// Mã kho xuất hàng (để hiển thị)
        /// </summary>
        [DisplayName("Mã kho")]
        [Display(Order = 11)]
        public string WarehouseCode { get; set; }

        /// <summary>
        /// Tên kho xuất hàng (để hiển thị)
        /// </summary>
        [DisplayName("Tên kho")]
        [Display(Order = 12)]
        public string WarehouseName { get; set; }


        #endregion

        #region Properties - Thông tin bổ sung

        /// <summary>
        /// Ghi chú
        /// </summary>
        [DisplayName("Ghi chú")]
        [Display(Order = 20)]
        [StringLength(500, ErrorMessage = "Ghi chú không được vượt quá 500 ký tự")]
        public string Notes { get; set; }

        /// <summary>
        /// Người giao hàng
        /// </summary>
        [DisplayName("Người giao hàng")]
        [Display(Order = 21)]
        [StringLength(500, ErrorMessage = "Người giao hàng không được vượt quá 500 ký tự")]
        [Required(ErrorMessage = "Người giao hàng không được để trống")]
        public string NguoiGiaoHang { get; set; }

        /// <summary>
        /// Người nhận hàng
        /// </summary>
        [DisplayName("Người nhận hàng")]
        [Display(Order = 22)]
        [StringLength(500, ErrorMessage = "Người nhận hàng không được vượt quá 500 ký tự")]
        [Required(ErrorMessage = "Người nhận hàng không được để trống")]
        public string NguoiNhanHang { get; set; }

        #endregion

        #region Private Fields - Tổng hợp

        private decimal _totalQuantity;

        #endregion

        #region Properties - Tổng hợp (Computed)

        /// <summary>
        /// Tổng số lượng xuất - Computed property
        /// Tính toán từ tổng StockOutQty của tất cả các dòng detail
        /// Map với: StockInOutMaster.TotalQuantity (lưu vào DB khi save)
        /// </summary>
        [DisplayName("Tổng SL")]
        [Display(Order = 30)]
        public decimal TotalQuantity => _totalQuantity;

        #endregion

        #region Public Methods - Cập nhật tổng hợp

        /// <summary>
        /// Cập nhật các giá trị tổng hợp từ detail
        /// Method này được gọi từ UcStockOutMaster khi có thay đổi trong Detail
        /// </summary>
        /// <param name="totalQuantity">Tổng số lượng</param>
        public void SetTotals(decimal totalQuantity)
        {
            _totalQuantity = totalQuantity;
        }

        #endregion
    }
}