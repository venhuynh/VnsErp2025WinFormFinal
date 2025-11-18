using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bll.MasterData.ProductServiceBll;
using Common.Utils;
using DevExpress.XtraEditors;
using DTO.MasterData.ProductService;

namespace MasterData.ProductService
{
    /// <summary>
    /// Form thêm hình ảnh sản phẩm.
    /// Cung cấp chức năng chọn và upload nhiều hình ảnh cho sản phẩm với validation và giao diện thân thiện.
    /// </summary>
    public partial class FrmProductImageAdd : XtraForm
    {
        #region ========== KHAI BÁO BIẾN ==========

        /// <summary>
        /// Business Logic Layer cho hình ảnh sản phẩm
        /// </summary>
        private ProductImageBll _productImageBll;

        /// <summary>
        /// Business Logic Layer cho sản phẩm/dịch vụ
        /// </summary>
        private ProductServiceBll _productServiceBll;

        /// <summary>
        /// ID sản phẩm để thêm hình ảnh
        /// </summary>
        private Guid ProductId { get; set; }

        #endregion

        #region ========== CONSTRUCTOR & PUBLIC METHODS ==========

        /// <summary>
        /// Constructor mặc định
        /// </summary>
        public FrmProductImageAdd()
        {
            InitializeComponent();
            InitializeBll();
            InitializeEvents();
            
            // Thiết lập SuperToolTip cho các controls
            SetupSuperToolTips();
            
            LoadProductList();
        }

        #endregion

        #region ========== KHỞI TẠO FORM ==========

        /// <summary>
        /// Khởi tạo BLL
        /// </summary>
        private void InitializeBll()
        {
            _productImageBll = new ProductImageBll();
            _productServiceBll = new ProductServiceBll();
        }

        /// <summary>
        /// Khởi tạo events
        /// </summary>
        private void InitializeEvents()
        {
            // Event cho nút chọn hình ảnh
            OpenSelectImageHyperlinkLabelControl.Click += OpenSelectImageHyperlinkLabelControl_Click;
            
            // Event cho SearchLookupEdit chọn sản phẩm
            ProductServiceSearchLookupEdit.EditValueChanged += ProductServiceSearchLookupEdit_EditValueChanged;
        }

        #endregion

        #region ========== QUẢN LÝ DỮ LIỆU ==========

        /// <summary>
        /// Thực hiện operation async với WaitingForm1 hiển thị
        /// </summary>
        /// <param name="operation">Operation async cần thực hiện</param>
        private async Task ExecuteWithWaitingFormAsync(Func<Task> operation)
        {
            try
            {
                // Thực hiện operation
                await operation();
            }
            catch (Exception e)
            {
                MsgBox.ShowException(e);
            }
           
        }

        /// <summary>
        /// Load danh sách sản phẩm/dịch vụ
        /// </summary>
        private async void LoadProductList()
        {
            await ExecuteWithWaitingFormAsync(async () =>
            {
                await LoadProductListAsync();
            });
        }

        /// <summary>
        /// Load danh sách sản phẩm/dịch vụ (async, không hiển thị WaitForm)
        /// </summary>
        private async Task LoadProductListAsync()
        {
            try
            {

                // Get all data
                var entities = await _productServiceBll.GetAllAsync();

                // Convert to DTOs (without counting to improve performance)
                var dtoList = entities.ToDtoList(
                    categoryId => _productServiceBll.GetCategoryName(categoryId)
                ).ToList();
                
                // Bind trực tiếp vào productServiceDtoBindingSource
                productServiceDtoBindingSource.DataSource = dtoList;

                // Nếu có ProductId được set, tự động chọn sản phẩm đó
                if (ProductId != Guid.Empty)
                {
                    SelectProduct(ProductId);
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khi tải danh sách sản phẩm/dịch vụ");
            }
        }

        /// <summary>
        /// Chọn sản phẩm theo ID
        /// </summary>
        /// <param name="productId">ID sản phẩm</param>
        private void SelectProduct(Guid productId)
        {
            try
            {
                // Tìm sản phẩm trong binding source
                for (int i = 0; i < productServiceDtoBindingSource.Count; i++)
                {
                    if (productServiceDtoBindingSource[i] is ProductServiceDto product && product.Id == productId)
                    {
                        productServiceDtoBindingSource.Position = i;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khi chọn sản phẩm");
            }
        }

        #endregion

        #region ========== SỰ KIỆN FORM ==========

        /// <summary>
        /// Xử lý sự kiện thay đổi giá trị SearchLookupEdit
        /// </summary>
        private void ProductServiceSearchLookupEdit_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                // EditValue trả về Guid (ID của sản phẩm)
                if (ProductServiceSearchLookupEdit.EditValue is Guid productId)
                {
                    // Cập nhật ProductId
                    ProductId = productId;
                    
                }
                else
                {
                    // Reset ProductId nếu không có sản phẩm nào được chọn
                    ProductId = Guid.Empty;
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khi chọn sản phẩm");
            }
        }

        /// <summary>
        /// Xử lý sự kiện click nút chọn hình ảnh
        /// </summary>
        private async void OpenSelectImageHyperlinkLabelControl_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra đã chọn sản phẩm chưa
                if (ProductId == Guid.Empty)
                {
                    ShowInfo("Vui lòng chọn sản phẩm trước khi thêm hình ảnh.");
                    return;
                }

                // Cấu hình OpenFileDialog để chọn nhiều hình ảnh
                xtraOpenFileDialog1.Filter = @"Hình ảnh|*.jpg;*.jpeg;*.png;*.gif;*.bmp|Tất cả files|*.*";
                xtraOpenFileDialog1.Multiselect = true;
                xtraOpenFileDialog1.Title = @"Chọn hình ảnh cho sản phẩm";

                // Hiển thị dialog chọn file
                if (xtraOpenFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    var selectedFiles = xtraOpenFileDialog1.FileNames;
                    if (selectedFiles.Length > 0)
                    {
                        await ProcessSelectedImagesAsync(selectedFiles);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khi chọn hình ảnh");
            }
        }

        #endregion

        #region ========== XỬ LÝ HÌNH ẢNH ==========

        /// <summary>
        /// Xử lý các hình ảnh đã chọn
        /// </summary>
        /// <param name="imagePaths">Danh sách đường dẫn hình ảnh</param>
        private async Task ProcessSelectedImagesAsync(string[] imagePaths)
        {
            try
            {
                await ExecuteWithWaitingFormAsync(async () =>
                {
                    await ProcessSelectedImagesWithoutSplashAsync(imagePaths);
                });
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khi xử lý hình ảnh");
            }
        }

        /// <summary>
        /// Xử lý các hình ảnh đã chọn (không hiển thị WaitingForm)
        /// </summary>
        /// <param name="imagePaths">Danh sách đường dẫn hình ảnh</param>
        private Task ProcessSelectedImagesWithoutSplashAsync(string[] imagePaths)
        {
            var successCount = 0;
            var errorCount = 0;
            var errorMessages = new List<string>();

            foreach (var imagePath in imagePaths)
            {
                try
                {
                    // Sử dụng BLL để lưu hình ảnh
                    var productImage = _productImageBll.SaveImageFromFile(ProductId, imagePath);
                    
                    if (productImage != null)
                    {
                        successCount++;
                    }
                    else
                    {
                        errorCount++;
                        errorMessages.Add($"{Path.GetFileName(imagePath)}: Không thể lưu hình ảnh");
                    }
                }
                catch (Exception ex)
                {
                    errorCount++;
                    errorMessages.Add($"{Path.GetFileName(imagePath)}: {ex.Message}");
                }
            }

            // Hiển thị kết quả
            ShowImageProcessingResult(successCount, errorCount, errorMessages);
            
            //Đóng màn hình 
            Close();
            return Task.CompletedTask;
        }

        /// <summary>
        /// Hiển thị kết quả xử lý hình ảnh
        /// </summary>
        /// <param name="successCount">Số hình ảnh thành công</param>
        /// <param name="errorCount">Số hình ảnh lỗi</param>
        /// <param name="errorMessages">Danh sách lỗi</param>
        private void ShowImageProcessingResult(int successCount, int errorCount, List<string> errorMessages)
        {
            var message = "Kết quả xử lý hình ảnh:\n\n";
            message += $"✅ Thành công: {successCount} hình ảnh\n";
            message += $"❌ Lỗi: {errorCount} hình ảnh\n\n";

            if (errorCount > 0 && errorMessages.Any())
            {
                message += "Chi tiết lỗi:\n";
                foreach (var error in errorMessages.Take(5)) // Chỉ hiển thị 5 lỗi đầu tiên
                {
                    message += $"• {error}\n";
                }
                if (errorMessages.Count > 5)
                {
                    message += $"• ... và {errorMessages.Count - 5} lỗi khác\n";
                }
            }

            if (successCount > 0)
            {
                message += "\n🎉 Hình ảnh đã được lưu thành công!";
            }

            ShowInfo(message);
        }

        #endregion

        #region ========== TIỆN ÍCH ==========

        /// <summary>
        /// Thiết lập SuperToolTip cho các controls trong form
        /// </summary>
        private void SetupSuperToolTips()
        {
            try
            {
                if (ProductServiceSearchLookupEdit != null)
                {
                    SuperToolTipHelper.SetBaseEditSuperTip(
                        ProductServiceSearchLookupEdit,
                        title: "<b><color=DarkBlue>📦 Sản phẩm/Dịch vụ</color></b>",
                        content: "Chọn sản phẩm hoặc dịch vụ để thêm hình ảnh. Trường này là bắt buộc."
                    );
                }

                if (OpenSelectImageHyperlinkLabelControl != null)
                {
                    var superTip = SuperToolTipHelper.CreateSuperToolTip(
                        title: "<b><color=Green>🖼️ Chọn hình ảnh</color></b>",
                        content: "Chọn một hoặc nhiều hình ảnh để thêm vào sản phẩm/dịch vụ."
                    );
                    OpenSelectImageHyperlinkLabelControl.SuperTip = superTip;
                }
            }
            catch (Exception ex)
            {
                // Ignore lỗi setup SuperToolTip để không chặn form
                System.Diagnostics.Debug.WriteLine($"Lỗi setup SuperToolTip: {ex.Message}");
            }
        }

        /// <summary>
        /// Hiển thị thông tin
        /// </summary>
        private void ShowInfo(string message)
        {
            MsgBox.ShowSuccess(message);
        }

        /// <summary>
        /// Hiển thị lỗi với thông tin ngữ cảnh
        /// </summary>
        private void ShowError(Exception ex, string context = null)
        {
            MsgBox.ShowException(
                string.IsNullOrWhiteSpace(context) ? ex : new Exception($"{context}: {ex.Message}", ex));
        }

        #endregion
    }
}