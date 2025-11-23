namespace Bll.Common.ImageStorage
{
    /// <summary>
    /// Danh mục hình ảnh để phân loại và tổ chức thư mục
    /// </summary>
    public enum ImageCategory
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
        Temp
    }
}

