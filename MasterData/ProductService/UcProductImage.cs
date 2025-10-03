using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bll.MasterData.ProductServiceBll;
using MasterData.ProductService.Dto;
using Bll.Common;

namespace MasterData.ProductService
{
    public partial class UcProductImage : DevExpress.XtraEditors.XtraUserControl
    {
        #region Fields

        private ProductImageBll _productImageBll;
        private Guid? _currentProductId;
        private List<ProductImageDto> _imageList;

        #endregion

        #region Properties

        /// <summary>
        /// ID sản phẩm hiện tại
        /// </summary>
        public Guid? ProductId
        {
            get { return _currentProductId; }
            set
            {
                _currentProductId = value;
                LoadImages();
            }
        }

        #endregion

        #region Constructor

        public UcProductImage()
        {
            InitializeComponent();
            InitializeBll();
            InitializeEvents();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Khởi tạo BLL
        /// </summary>
        private void InitializeBll()
        {
            _productImageBll = new ProductImageBll();
            _imageList = new List<ProductImageDto>();
        }

        /// <summary>
        /// Khởi tạo events
        /// </summary>
        private void InitializeEvents()
        {
            // Event cho nút Add Image
            btnAddImage.Click += BtnAddImage_Click;
        }

        /// <summary>
        /// Load danh sách hình ảnh
        /// </summary>
        private void LoadImages()
        {
            //try
            //{
            //    if (!_currentProductId.HasValue)
            //    {
            //        _imageList.Clear();
            //        gridControl1.DataSource = null;
            //        return;
            //    }

            //    // Lấy danh sách hình ảnh từ BLL
            //    var images = _productImageBll.GetByProductId(_currentProductId.Value);
                
            //    // Convert sang DTO
            //    _imageList = images.Select(img => new ProductImageDto
            //    {
            //        Id = img.Id,
            //        ProductId = img.ProductId,
            //        VariantId = img.VariantId,
            //        ImagePath = img.ImagePath,
            //        SortOrder = img.SortOrder,
            //        IsPrimary = img.IsPrimary,
            //        ImageData = img.ImageData,
            //        ImageType = img.ImageType,
            //        ImageSize = img.ImageSize,
            //        ImageWidth = img.ImageWidth,
            //        ImageHeight = img.ImageHeight,
            //        Caption = img.Caption,
            //        AltText = img.AltText,
            //        IsActive = img.IsActive,
            //        CreatedDate = img.CreatedDate,
            //        ModifiedDate = img.ModifiedDate
            //    }).ToList();

            //    // Cập nhật display properties
            //    foreach (var dto in _imageList)
            //    {
            //        dto.UpdateDisplayProperties();
            //    }

            //    // Bind data
            //    gridControl1.DataSource = _imageList;
            //}
            //catch (Exception ex)
            //{
            //    XtraMessageBox.Show($"Lỗi khi tải danh sách hình ảnh: {ex.Message}", "Lỗi", 
            //        MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }

        /// <summary>
        /// Hiển thị form thêm hình ảnh
        /// </summary>
        private void ShowAddImageForm()
        {
            try
            {
                // Sử dụng OverlayManager.ShowScope để auto-close overlay
                using (OverlayManager.ShowScope(this))
                {
                    using (var addImageForm = new FrmAddProductImage())
                    {
                        // Cấu hình form
                        addImageForm.Text = "Thêm hình ảnh sản phẩm";
                        addImageForm.StartPosition = FormStartPosition.CenterParent;
                        
                        // Hiển thị form dạng dialog
                        addImageForm.ShowDialog(this);
                        
                        // Reload danh sách hình ảnh sau khi đóng form
                        LoadImages();
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Lỗi khi mở form thêm hình ảnh: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Xử lý sự kiện click nút Add Image
        /// </summary>
        private void BtnAddImage_Click(object sender, EventArgs e)
        {
            ShowAddImageForm();
        }

        #endregion
    }
}
