using Bll.Common;
using System;
using System.Windows.Forms;
using Dal.DataContext.SeedData.MasterData.ProductService;

namespace MasterData.ProductService
{
    public partial class FluentProductService : DevExpress.XtraBars.FluentDesignSystem.FluentDesignForm
    {
        #region Fields

        private UserControlManager _userControlManager;
        private UcProductServiceList _productServiceListControl;
        private UcProductServiceCategory _productServiceCategoryControl;
        private UcProductVariant _productVariantControl;
        private UcUnitOfMeasure _unitOfMeasureControl;
        private UcAttribute _attributeControl;

        #endregion

        #region Constructor

        public FluentProductService()
        {
            InitializeComponent();
            
            ProductServiceCategoryBtn.Click += ProductServiceCategoryBtn_Click;

            InitializeUserControls();
            
            //DeleteAllData();
            
            //SeedMasterData();
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Khởi tạo các UserControl và UserControlManager
        /// </summary>
        private void InitializeUserControls()
        {
            try
            {
                // Khởi tạo UserControlManager với container
                _userControlManager = new UserControlManager(this.fluentDesignFormContainer1);

                // Thiết lập form properties
                this.Text = @"VnsErp2025 - Quản lý Sản phẩm/Dịch vụ";
                this.WindowState = FormWindowState.Maximized;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khởi tạo form: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Xử lý sự kiện Load form
        /// </summary>
        private void FluentProductService_Load(object sender, EventArgs e)
        {
            try
            {
                // Gọi hàm SeedData_Master_ProductService để tạo dữ liệu mẫu
                //SeedMasterData();

                // Hiển thị màn hình mặc định (Danh sách sản phẩm/dịch vụ)
                //ShowProductServiceList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load form: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

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
        /// Xử lý click nút Đơn vị đo
        /// </summary>
        private void UnitOfMeasureBtn_Click(object sender, EventArgs e)
        {
            try
            {
                ShowUnitOfMeasure();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi hiển thị đơn vị đo: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Xử lý click nút Thuộc tính sản phẩm
        /// </summary>
        private void AttributeBtn_Click(object sender, EventArgs e)
        {
            try
            {
                ShowAttribute();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi hiển thị thuộc tính sản phẩm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Navigation Methods

        /// <summary>
        /// Hiển thị màn hình danh sách sản phẩm/dịch vụ
        /// </summary>
        private void ShowProductServiceList()
        {
            try
            {
                if (_productServiceListControl == null)
                {
                    _productServiceListControl = new UcProductServiceList();
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
        /// Hiển thị màn hình đơn vị đo
        /// </summary>
        private void ShowUnitOfMeasure()
        {
            try
            {
                if (_unitOfMeasureControl == null)
                {
                    _unitOfMeasureControl = new UcUnitOfMeasure();
                }

                _userControlManager?.ShowControl(_unitOfMeasureControl, "Đơn vị đo");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi hiển thị đơn vị đo: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Hiển thị màn hình thuộc tính sản phẩm
        /// </summary>
        private void ShowAttribute()
        {
            try
            {
                if (_attributeControl == null)
                {
                    _attributeControl = new UcAttribute();
                }

                _userControlManager?.ShowControl(_attributeControl, "Thuộc tính sản phẩm");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi hiển thị thuộc tính sản phẩm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Form Management

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
                _unitOfMeasureControl?.Dispose();
                _attributeControl?.Dispose();
                _userControlManager = null;

                base.OnFormClosing(e);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi đóng form: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
