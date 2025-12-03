namespace Bll.Common.ImageStorage
{
    /// <summary>
    /// Danh mục file để phân loại và tổ chức thư mục
    /// Mở rộng từ ImageCategory để hỗ trợ nhiều loại file khác nhau
    /// </summary>
    public enum FileCategory
    {
        /// <summary>
        /// Hình ảnh sản phẩm
        /// </summary>
        Product,

        /// <summary>
        /// Hình ảnh biến thể sản phẩm
        /// </summary>
        ProductVariant,

        /// <summary>
        /// Hình ảnh phiếu nhập/xuất kho
        /// </summary>
        StockInOut,

        /// <summary>
        /// Logo công ty
        /// </summary>
        Company,

        /// <summary>
        /// Avatar người dùng/đối tác
        /// </summary>
        Avatar,

        /// <summary>
        /// Thư mục tạm cho upload staging
        /// </summary>
        Temp,

        /// <summary>
        /// Chứng từ nhập/xuất kho (PDF, DOCX, XLSX, etc.)
        /// </summary>
        StockInOutDocument,

        /// <summary>
        /// Chứng từ đối tác (Hợp đồng, Hóa đơn, etc.)
        /// </summary>
        BusinessPartnerDocument,

        /// <summary>
        /// Tài liệu chung (không phân loại cụ thể)
        /// </summary>
        Document,

        /// <summary>
        /// Báo cáo
        /// </summary>
        Report
    }
}

