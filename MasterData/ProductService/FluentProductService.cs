using System;
using System.Windows.Forms;
using Bll.Common;
using Dal.DataContext.SeedData.MasterData.ProductService;
using DevExpress.XtraBars.FluentDesignSystem;

namespace MasterData.ProductService
{
    /// <summary>
    /// Form chính quản lý sản phẩm/dịch vụ sử dụng Fluent Design System.
    /// Cung cấp giao diện hiện đại với navigation menu và quản lý các UserControl con.
    /// </summary>
    public partial class FluentProductService : FluentDesignForm
    {
        #region ========== KHAI BÁO BIẾN ==========

        /// <summary>
        /// Manager quản lý các UserControl con
        /// </summary>
        private UserControlManager _userControlManager;

        /// <summary>
        /// UserControl danh sách sản phẩm/dịch vụ dạng CardView
        /// </summary>
        private UcProductServiceListCardView _productServiceListControl;

        /// <summary>
        /// UserControl danh mục sản phẩm/dịch vụ
        /// </summary>
        private UcProductServiceCategory _productServiceCategoryControl;

        /// <summary>
        /// UserControl biến thể sản phẩm
        /// </summary>
        private UcProductVariant _productVariantControl;

        /// <summary>
        /// UserControl hình ảnh sản phẩm
        /// </summary>
        private UcProductImage _productImageControl;

        #endregion

        #region ========== CONSTRUCTOR & PUBLIC METHODS ==========

        /// <summary>
        /// Khởi tạo form chính
        /// </summary>
        public FluentProductService()
        {
            InitializeComponent();
            
            ProductServiceCategoryBtn.Click += ProductServiceCategoryBtn_Click;
            ProductServiceListBtn.Click += ProductServiceListBtn_Click;
            ProductVariantBtn.Click += ProductVariantBtn_Click;
            ProductServiceImagesBtn.Click += ProductServiceImagesBtn_Click;

            InitializeUserControls();
            
            //Xóa hết dữ liệu test
            //DeleteAllData();
            
            //Tạo dữ liệu test
            //SeedMasterData();
        }

        #endregion

        #region ========== KHỞI TẠO ==========

        /// <summary>
        /// Khởi tạo các UserControl và UserControlManager
        /// </summary>
        private void InitializeUserControls()
        {
            try
            {
                // Khởi tạo UserControlManager với container
                _userControlManager = new UserControlManager(fluentDesignFormContainer1);

                // Thiết lập form properties
                Text = @"VnsErp2025 - Quản lý Sản phẩm/Dịch vụ";
                WindowState = FormWindowState.Maximized;
            }
            catch (Exception ex)
            {
                MessageBox.Show($@"Lỗi khởi tạo form: {ex.Message}", @"Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region ========== SỰ KIỆN FORM ==========


        /// <summary>
        /// Xóa tất cả dữ liệu ProductService (dùng cho test)
        /// </summary>
        public static void DeleteAllData()
        {
            try
            {
                Console.WriteLine("=== Ví dụ 1: Xóa tất cả dữ liệu ProductService ===");

                // Xóa tất cả dữ liệu và lấy thông tin số lượng đã xóa
                var deletedCounts = SeedData_Master_ProductService.DeleteAllProductServiceData();

                Console.WriteLine("Đã xóa thành công các dữ liệu sau:");
                foreach (var item in deletedCounts)
                {
                    Console.WriteLine($"  {item.Key}: {item.Value} records");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Gọi hàm SeedData_Master_ProductService để tạo dữ liệu mẫu
        /// Sử dụng ConnectionManager để lấy connection string, không tạo context ở GUI
        /// </summary>
        private void SeedMasterData()
        {
            try
            {
                // Gọi hàm tạo tất cả dữ liệu mẫu - sử dụng ConnectionManager
                SeedData_Master_ProductService.SeedAllData();
                
                MessageBox.Show("Đã tạo dữ liệu mẫu thành công!\n\n" +
                              "• 37 ProductServiceCategory (3 cấp phân cấp)\n" +
                              "• 20 UnitOfMeasure\n" +
                              "• 10 Attribute với 70+ AttributeValue\n" +
                              "• 200 ProductService (60% sản phẩm, 40% dịch vụ)\n" +
                              "• 500 ProductVariant\n" +
                              "• 1000+ VariantAttribute mappings\n" +
                              "• 1000 ProductImage", 
                              "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tạo dữ liệu mẫu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Xử lý click nút Danh mục sản phẩm/dịch vụ
        /// </summary>
        private void ProductServiceCategoryBtn_Click(object sender, EventArgs e)
        {
            try
            {
                ShowProductServiceCategory();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi hiển thị danh mục sản phẩm/dịch vụ: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Xử lý click nút Danh sách sản phẩm/dịch vụ
        /// </summary>
        private void ProductServiceListBtn_Click(object sender, EventArgs e)
        {
            try
            {
                ShowProductServiceList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi hiển thị danh sách sản phẩm/dịch vụ: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Xử lý click nút Biến thể sản phẩm
        /// </summary>
        private void ProductVariantBtn_Click(object sender, EventArgs e)
        {
            try
            {
                ShowProductVariant();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi hiển thị biến thể sản phẩm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Xử lý click nút Hình ảnh sản phẩm
        /// </summary>
        private void ProductServiceImagesBtn_Click(object sender, EventArgs e)
        {
            try
            {
                ShowProductImage();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi hiển thị hình ảnh sản phẩm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region ========== ĐIỀU HƯỚNG ==========

        /// <summary>
        /// Hiển thị màn hình danh sách sản phẩm/dịch vụ
        /// </summary>
        private void ShowProductServiceList()
        {
            try
            {
                if (_productServiceListControl == null)
                {
                    _productServiceListControl = new UcProductServiceListCardView();
                }

                _userControlManager?.ShowControl(_productServiceListControl, "Danh sách sản phẩm/dịch vụ");
            }
            catch (Exception ex)
            {
                MessageBox.Show($@"Lỗi khi hiển thị danh sách sản phẩm/dịch vụ: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Hiển thị màn hình danh mục sản phẩm/dịch vụ
        /// </summary>
        private void ShowProductServiceCategory()
        {
            try
            {
                if (_productServiceCategoryControl == null)
                {
                    _productServiceCategoryControl = new UcProductServiceCategory();
                }

                _userControlManager?.ShowControl(_productServiceCategoryControl, "Danh mục sản phẩm/dịch vụ");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi hiển thị danh mục sản phẩm/dịch vụ: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Hiển thị màn hình biến thể sản phẩm
        /// </summary>
        private void ShowProductVariant()
        {
            try
            {
                if (_productVariantControl == null)
                {
                    _productVariantControl = new UcProductVariant();
                }

                _userControlManager?.ShowControl(_productVariantControl, "Biến thể sản phẩm");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi hiển thị biến thể sản phẩm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Hiển thị màn hình hình ảnh sản phẩm
        /// </summary>
        private void ShowProductImage()
        {
            try
            {
                if (_productImageControl == null)
                {
                    _productImageControl = new UcProductImage();
                }

                _userControlManager?.ShowControl(_productImageControl, "Hình ảnh sản phẩm");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi hiển thị hình ảnh sản phẩm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region ========== QUẢN LÝ FORM ==========

        /// <summary>
        /// Xử lý sự kiện đóng form
        /// </summary>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            try
            {
                // Cleanup resources
                _productServiceListControl?.Dispose();
                _productServiceCategoryControl?.Dispose();
                _productVariantControl?.Dispose();
                _productImageControl?.Dispose();
                _userControlManager = null;

                base.OnFormClosing(e);
            }
            catch (Exception ex)
            {
                MessageBox.Show($@"Lỗi khi đóng form: {ex.Message}", @"Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Xử lý sự kiện đóng form (FormClosed)
        /// </summary>
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            try
            {
                base.OnFormClosed(e);
            }
            catch (Exception)
            {
                // Silent error handling
            }
        }

        #endregion
    }
}
