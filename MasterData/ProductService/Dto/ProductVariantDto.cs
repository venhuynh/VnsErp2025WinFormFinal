using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace MasterData.ProductService.Dto
{
    /// <summary>
    /// Data Transfer Object cho ProductVariant entity
    /// Chứa thông tin biến thể sản phẩm/dịch vụ
    /// Tối ưu cho hiển thị trong AdvBandedGridView
    /// </summary>
    public class ProductVariantDto : INotifyPropertyChanged
    {
        #region Private Fields
        private string _productCode;
        private string _productName;
        private string _variantCode;
        private string _unitCode;
        private string _unitName;
        private bool _isActive;

        #endregion

        #region Properties

        /// <summary>
        /// ID duy nhất của biến thể
        /// </summary>
        [DisplayName("ID")]
        [Display(Order = -1)]
        public Guid Id { get; set; }

        /// <summary>
        /// ID của sản phẩm/dịch vụ gốc
        /// </summary>
        [DisplayName("ID Sản phẩm")]
        [Display(Order = -1)]
        [Required(ErrorMessage = "Sản phẩm gốc không được để trống")]
        public Guid ProductId { get; set; }

        /// <summary>
        /// Mã sản phẩm/dịch vụ gốc (từ ProductService.Code)
        /// </summary>
        [DisplayName("Mã sản phẩm")]
        [Display(Order = 1)]
        [StringLength(50, ErrorMessage = "Mã sản phẩm không được vượt quá 50 ký tự")]
        public string ProductCode 
        { 
            get => _productCode;
            set => SetProperty(ref _productCode, value);
        }

        /// <summary>
        /// Tên sản phẩm/dịch vụ gốc (từ ProductService.Name)
        /// </summary>
        [DisplayName("Tên sản phẩm")]
        [Display(Order = 2)]
        [StringLength(255, ErrorMessage = "Tên sản phẩm không được vượt quá 255 ký tự")]
        public string ProductName 
        { 
            get => _productName;
            set => SetProperty(ref _productName, value);
        }

        /// <summary>
        /// Mã biến thể (từ ProductVariant.VariantCode)
        /// </summary>
        [DisplayName("Mã biến thể")]
        [Display(Order = 3)]
        [Required(ErrorMessage = "Mã biến thể không được để trống")]
        [StringLength(50, ErrorMessage = "Mã biến thể không được vượt quá 50 ký tự")]
        public string VariantCode 
        { 
            get => _variantCode;
            set => SetProperty(ref _variantCode, value);
        }

        /// <summary>
        /// ID của đơn vị tính (từ ProductVariant.UnitId)
        /// </summary>
        [DisplayName("ID Đơn vị")]
        [Display(Order = -1)]
        [Required(ErrorMessage = "Đơn vị tính không được để trống")]
        public Guid UnitId { get; set; }

        /// <summary>
        /// Mã đơn vị tính (từ UnitOfMeasure.Code)
        /// </summary>
        [DisplayName("Mã đơn vị")]
        [Display(Order = 4)]
        [StringLength(20, ErrorMessage = "Mã đơn vị không được vượt quá 20 ký tự")]
        public string UnitCode 
        { 
            get => _unitCode;
            set => SetProperty(ref _unitCode, value);
        }

        /// <summary>
        /// Tên đơn vị tính (từ UnitOfMeasure.Name)
        /// </summary>
        [DisplayName("Tên đơn vị")]
        [Display(Order = 5)]
        [StringLength(100, ErrorMessage = "Tên đơn vị không được vượt quá 100 ký tự")]
        public string UnitName 
        { 
            get => _unitName;
            set => SetProperty(ref _unitName, value);
        }

        /// <summary>
        /// Trạng thái hoạt động (từ ProductVariant.IsActive)
        /// </summary>
        [DisplayName("Trạng thái")]
        [Display(Order = 6)]
        [Description("Trạng thái hoạt động của biến thể")]
        public bool IsActive 
        { 
            get => _isActive;
            set => SetProperty(ref _isActive, value);
        }

        /// <summary>
        /// Ảnh thumbnail của biến thể (từ ProductVariant.ThumbnailImage)
        /// </summary>
        [DisplayName("Ảnh thumbnail")]
        [Description("Ảnh thumbnail của biến thể")]
        public byte[] ThumbnailImage { get; set; }

        /// <summary>
        /// Số lượng thuộc tính của biến thể
        /// </summary>
        [DisplayName("Số thuộc tính")]
        [Description("Số lượng thuộc tính của biến thể")]
        public int AttributeCount { get; set; }

        /// <summary>
        /// Số lượng hình ảnh của biến thể
        /// </summary>
        [DisplayName("Số hình ảnh")]
        [Description("Số lượng hình ảnh của biến thể")]
        public int ImageCount { get; set; }

        /// <summary>
        /// Danh sách thuộc tính của biến thể (từ VariantAttributes)
        /// </summary>
        [DisplayName("Thuộc tính")]
        [Description("Danh sách thuộc tính của biến thể")]
        public List<ProductVariantAttributeDto> Attributes { get; set; }

        /// <summary>
        /// Danh sách hình ảnh của biến thể (từ ProductImages)
        /// </summary>
        [DisplayName("Hình ảnh")]
        [Description("Danh sách hình ảnh của biến thể")]
        public List<ProductVariantImageDto> Images { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Khởi tạo DTO với giá trị mặc định
        /// </summary>
        public ProductVariantDto()
        {
            Id = Guid.NewGuid();
            Attributes = new List<ProductVariantAttributeDto>();
            Images = new List<ProductVariantImageDto>();
            IsActive = true;
        }

        /// <summary>
        /// Khởi tạo DTO với thông tin cơ bản
        /// </summary>
        /// <param name="productId">ID sản phẩm gốc</param>
        /// <param name="variantCode">Mã biến thể</param>
        /// <param name="unitId">ID đơn vị tính</param>
        public ProductVariantDto(Guid productId, string variantCode, Guid unitId)
            : this()
        {
            ProductId = productId;
            VariantCode = variantCode;
            UnitId = unitId;
        }

        #endregion

        #region Computed Properties

        /// <summary>
        /// Hiển thị trạng thái dạng text
        /// </summary>
        [DisplayName("Trạng thái")]
        [Description("Trạng thái hiển thị của biến thể")]
        public string StatusDisplay => IsActive ? "Hoạt động" : "Không hoạt động";

        /// <summary>
        /// Hiển thị tên đầy đủ của biến thể (ProductName - VariantCode)
        /// </summary>
        [DisplayName("Tên đầy đủ")]
        [Description("Tên đầy đủ của biến thể")]
        public string FullName => $"{ProductName} - {VariantCode}";

        /// <summary>
        /// Hiển thị thông tin đơn vị (UnitCode - UnitName)
        /// </summary>
        [DisplayName("Đơn vị")]
        [Description("Thông tin đơn vị tính")]
        public string UnitDisplay => $"{UnitCode} - {UnitName}";

        /// <summary>
        /// Hiển thị danh sách thuộc tính dạng text (tối ưu cho GridView)
        /// </summary>
        [DisplayName("Thuộc tính")]
        [Description("Danh sách thuộc tính hiển thị")]
        public string AttributesDisplay
        {
            get
            {
                if (Attributes == null || Attributes.Count == 0)
                    return "Không có";

                var attributeTexts = new List<string>();
                foreach (var attr in Attributes)
                {
                    attributeTexts.Add($"{attr.AttributeName}: {attr.AttributeValue}");
                }

                return string.Join(", ", attributeTexts);
            }
        }

        /// <summary>
        /// Hiển thị danh sách hình ảnh dạng text (tối ưu cho GridView)
        /// </summary>
        [DisplayName("Hình ảnh")]
        [Description("Danh sách hình ảnh hiển thị")]
        public string ImagesDisplay
        {
            get
            {
                if (Images == null || Images.Count == 0)
                    return "Không có";

                return $"{ImageCount} hình ảnh";
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Kiểm tra xem biến thể có hợp lệ không
        /// </summary>
        /// <returns>True nếu hợp lệ</returns>
        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(VariantCode) &&
                   ProductId != Guid.Empty &&
                   UnitId != Guid.Empty;
        }

        /// <summary>
        /// Lấy danh sách lỗi validation
        /// </summary>
        /// <returns>Danh sách lỗi</returns>
        public List<string> GetValidationErrors()
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(VariantCode))
                errors.Add("Mã biến thể không được để trống");

            if (ProductId == Guid.Empty)
                errors.Add("Sản phẩm gốc không được để trống");

            if (UnitId == Guid.Empty)
                errors.Add("Đơn vị tính không được để trống");

            return errors;
        }

        /// <summary>
        /// Tạo bản sao của DTO
        /// </summary>
        /// <returns>Bản sao mới</returns>
        public ProductVariantDto Clone()
        {
            return new ProductVariantDto
            {
                Id = this.Id,
                ProductId = this.ProductId,
                ProductCode = this.ProductCode,
                ProductName = this.ProductName,
                VariantCode = this.VariantCode,
                UnitId = this.UnitId,
                UnitCode = this.UnitCode,
                UnitName = this.UnitName,
                IsActive = this.IsActive,
                ThumbnailImage = this.ThumbnailImage?.Clone() as byte[],
                AttributeCount = this.AttributeCount,
                ImageCount = this.ImageCount,
                Attributes = this.Attributes?.ConvertAll(a => a.Clone()),
                Images = this.Images?.ConvertAll(i => i.Clone())
            };
        }

        /// <summary>
        /// Cập nhật thông tin từ DTO khác
        /// </summary>
        /// <param name="other">DTO nguồn</param>
        public void UpdateFrom(ProductVariantDto other)
        {
            if (other == null) return;

            ProductId = other.ProductId;
            ProductCode = other.ProductCode;
            ProductName = other.ProductName;
            VariantCode = other.VariantCode;
            UnitId = other.UnitId;
            UnitCode = other.UnitCode;
            UnitName = other.UnitName;
            IsActive = other.IsActive;
            ThumbnailImage = other.ThumbnailImage?.Clone() as byte[];
            AttributeCount = other.AttributeCount;
            ImageCount = other.ImageCount;
        }

        #endregion

        #region INotifyPropertyChanged Implementation

        /// <summary>
        /// Event được trigger khi property thay đổi
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Helper method để set property và trigger PropertyChanged event
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu của property</typeparam>
        /// <param name="field">Private field</param>
        /// <param name="value">Giá trị mới</param>
        /// <param name="propertyName">Tên property (tự động lấy từ caller)</param>
        /// <returns>True nếu giá trị thay đổi</returns>
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// Trigger PropertyChanged event
        /// </summary>
        /// <param name="propertyName">Tên property thay đổi</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// So sánh hai DTO
        /// </summary>
        /// <param name="obj">Đối tượng so sánh</param>
        /// <returns>True nếu bằng nhau</returns>
        public override bool Equals(object obj)
        {
            if (obj is ProductVariantDto other)
            {
                return Id == other.Id;
            }

            return false;
        }

        /// <summary>
        /// Lấy hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        /// <summary>
        /// Chuyển đổi thành string
        /// </summary>
        /// <returns>String representation</returns>
        public override string ToString()
        {
            return $"{ProductName} - {VariantCode} ({UnitName})";
        }

        #endregion
    }

}