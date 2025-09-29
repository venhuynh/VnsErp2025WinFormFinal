using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace MasterData.ProductService.Dto
{
    /// <summary>
    /// Data Transfer Object cho UnitOfMeasure entity
    /// Chứa thông tin đơn vị tính
    /// Tối ưu cho hiển thị trong GridView và ComboBox
    /// </summary>
    public class UnitOfMeasureDto : INotifyPropertyChanged
    {
        #region Private Fields
        private string _code;
        private string _name;
        private string _description;
        private bool _isActive;
        #endregion

        #region Properties

        /// <summary>
        /// ID duy nhất của đơn vị tính
        /// </summary>
        [DisplayName("ID")]
        [Display(Order = -1)]
        public Guid Id { get; set; }

        /// <summary>
        /// Mã đơn vị tính (từ UnitOfMeasure.Code)
        /// </summary>
        [DisplayName("Mã đơn vị")]
        [Display(Order = 1)]
        [Required(ErrorMessage = "Mã đơn vị không được để trống")]
        [StringLength(20, ErrorMessage = "Mã đơn vị không được vượt quá 20 ký tự")]
        public string Code 
        { 
            get => _code;
            set => SetProperty(ref _code, value);
        }

        /// <summary>
        /// Tên đơn vị tính (từ UnitOfMeasure.Name)
        /// </summary>
        [DisplayName("Tên đơn vị")]
        [Display(Order = 2)]
        [Required(ErrorMessage = "Tên đơn vị không được để trống")]
        [StringLength(100, ErrorMessage = "Tên đơn vị không được vượt quá 100 ký tự")]
        public string Name 
        { 
            get => _name;
            set => SetProperty(ref _name, value);
        }

        /// <summary>
        /// Mô tả đơn vị tính (từ UnitOfMeasure.Description)
        /// </summary>
        [DisplayName("Mô tả")]
        [Display(Order = 3)]
        [StringLength(255, ErrorMessage = "Mô tả không được vượt quá 255 ký tự")]
        public string Description 
        { 
            get => _description;
            set => SetProperty(ref _description, value);
        }

        /// <summary>
        /// Trạng thái hoạt động (từ UnitOfMeasure.IsActive)
        /// </summary>
        [DisplayName("Trạng thái")]
        [Display(Order = 4)]
        [Description("Trạng thái hoạt động của đơn vị tính")]
        public bool IsActive 
        { 
            get => _isActive;
            set => SetProperty(ref _isActive, value);
        }

        /// <summary>
        /// Số lượng biến thể sản phẩm sử dụng đơn vị này
        /// </summary>
        [DisplayName("Số biến thể")]
        [Description("Số lượng biến thể sản phẩm sử dụng đơn vị này")]
        public int ProductVariantCount { get; set; }

        /// <summary>
        /// Danh sách biến thể sản phẩm sử dụng đơn vị này
        /// </summary>
        [DisplayName("Biến thể sản phẩm")]
        [Description("Danh sách biến thể sản phẩm sử dụng đơn vị này")]
        public List<ProductVariantDto> ProductVariants { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Khởi tạo DTO với giá trị mặc định
        /// </summary>
        public UnitOfMeasureDto()
        {
            Id = Guid.NewGuid();
            ProductVariants = new List<ProductVariantDto>();
            IsActive = true;
        }

        /// <summary>
        /// Khởi tạo DTO với thông tin cơ bản
        /// </summary>
        /// <param name="code">Mã đơn vị</param>
        /// <param name="name">Tên đơn vị</param>
        public UnitOfMeasureDto(string code, string name)
            : this()
        {
            Code = code;
            Name = name;
        }

        /// <summary>
        /// Khởi tạo DTO với đầy đủ thông tin
        /// </summary>
        /// <param name="code">Mã đơn vị</param>
        /// <param name="name">Tên đơn vị</param>
        /// <param name="description">Mô tả</param>
        /// <param name="isActive">Trạng thái hoạt động</param>
        public UnitOfMeasureDto(string code, string name, string description, bool isActive = true)
            : this(code, name)
        {
            Description = description;
            IsActive = isActive;
        }

        #endregion

        #region Computed Properties

        /// <summary>
        /// Hiển thị trạng thái dạng text
        /// </summary>
        [DisplayName("Trạng thái")]
        [Description("Trạng thái hiển thị của đơn vị tính")]
        public string StatusDisplay => IsActive ? "Hoạt động" : "Không hoạt động";

        /// <summary>
        /// Hiển thị tên đầy đủ của đơn vị (Code - Name)
        /// </summary>
        [DisplayName("Tên đầy đủ")]
        [Description("Tên đầy đủ của đơn vị tính")]
        public string FullName => $"{Code} - {Name}";

        /// <summary>
        /// Hiển thị thông tin đơn vị với mô tả (Code - Name - Description)
        /// </summary>
        [DisplayName("Thông tin đầy đủ")]
        [Description("Thông tin đầy đủ của đơn vị tính")]
        public string FullInfo
        {
            get
            {
                var info = $"{Code} - {Name}";
                if (!string.IsNullOrWhiteSpace(Description))
                {
                    info += $" ({Description})";
                }
                return info;
            }
        }

        /// <summary>
        /// Hiển thị số lượng biến thể sản phẩm
        /// </summary>
        [DisplayName("Số biến thể")]
        [Description("Số lượng biến thể sản phẩm sử dụng đơn vị này")]
        public string ProductVariantCountDisplay
        {
            get
            {
                if (ProductVariantCount == 0)
                    return "Không có";
                
                return $"{ProductVariantCount} biến thể";
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Kiểm tra xem đơn vị có hợp lệ không
        /// </summary>
        /// <returns>True nếu hợp lệ</returns>
        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Code) &&
                   !string.IsNullOrWhiteSpace(Name);
        }

        /// <summary>
        /// Lấy danh sách lỗi validation
        /// </summary>
        /// <returns>Danh sách lỗi</returns>
        public List<string> GetValidationErrors()
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(Code))
                errors.Add("Mã đơn vị không được để trống");

            if (string.IsNullOrWhiteSpace(Name))
                errors.Add("Tên đơn vị không được để trống");

            if (Code != null && Code.Length > 20)
                errors.Add("Mã đơn vị không được vượt quá 20 ký tự");

            if (Name != null && Name.Length > 100)
                errors.Add("Tên đơn vị không được vượt quá 100 ký tự");

            if (Description != null && Description.Length > 255)
                errors.Add("Mô tả không được vượt quá 255 ký tự");

            return errors;
        }

        /// <summary>
        /// Kiểm tra xem đơn vị có đang được sử dụng không
        /// </summary>
        /// <returns>True nếu đang được sử dụng</returns>
        public bool IsInUse()
        {
            return ProductVariantCount > 0;
        }

        /// <summary>
        /// Tạo bản sao của DTO
        /// </summary>
        /// <returns>Bản sao mới</returns>
        public UnitOfMeasureDto Clone()
        {
            return new UnitOfMeasureDto
            {
                Id = this.Id,
                Code = this.Code,
                Name = this.Name,
                Description = this.Description,
                IsActive = this.IsActive,
                ProductVariantCount = this.ProductVariantCount,
                ProductVariants = this.ProductVariants?.ConvertAll(pv => pv.Clone())
            };
        }

        /// <summary>
        /// Cập nhật thông tin từ DTO khác
        /// </summary>
        /// <param name="other">DTO nguồn</param>
        public void UpdateFrom(UnitOfMeasureDto other)
        {
            if (other == null) return;

            Code = other.Code;
            Name = other.Name;
            Description = other.Description;
            IsActive = other.IsActive;
            ProductVariantCount = other.ProductVariantCount;
        }

        /// <summary>
        /// Reset về trạng thái ban đầu
        /// </summary>
        public void Reset()
        {
            Code = string.Empty;
            Name = string.Empty;
            Description = string.Empty;
            IsActive = true;
            ProductVariantCount = 0;
            ProductVariants?.Clear();
        }

        /// <summary>
        /// Tạo đơn vị mặc định
        /// </summary>
        /// <returns>Đơn vị mặc định</returns>
        public static UnitOfMeasureDto CreateDefault()
        {
            return new UnitOfMeasureDto("PCS", "Cái", "Đơn vị tính mặc định", true);
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
            if (obj is UnitOfMeasureDto other)
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
            return FullName;
        }

        #endregion
    }
}
