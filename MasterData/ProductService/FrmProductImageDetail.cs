using Bll.MasterData.ProductServiceBll;
using Common.Common;
using Common.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraSplashScreen;
using MasterData.ProductService.Dto;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MasterData.ProductService
{
    /// <summary>
    /// Form chi tiết hình ảnh sản phẩm.
    /// Cung cấp chức năng xem, zoom, tải xuống, đặt làm ảnh chính và xóa hình ảnh với giao diện thân thiện.
    /// </summary>
    public partial class FrmProductImageDetail : XtraForm
    {
        #region ========== KHAI BÁO BIẾN ==========

        /// <summary>
        /// ID hình ảnh sản phẩm
        /// </summary>
        private Guid _productImageId;

        /// <summary>
        /// Business Logic Layer cho hình ảnh sản phẩm
        /// </summary>
        private ProductImageBll _productImageBll;

        /// <summary>
        /// DTO hình ảnh hiện tại
        /// </summary>
        private ProductImageDto _currentImageDto;

        /// <summary>
        /// Property để track việc xóa hình ảnh
        /// </summary>
        public bool WasImageDeleted { get; private set; }

        #endregion

        #region ========== CONSTRUCTOR & PUBLIC METHODS ==========

        /// <summary>
        /// Constructor với ID hình ảnh
        /// </summary>
        /// <param name="productImageId">ID hình ảnh sản phẩm</param>
        public FrmProductImageDetail(Guid productImageId)
        {
            _productImageId = productImageId;
            InitializeComponent();
            InitializeBll();
            InitializeEvents();
            
            // Thiết lập SuperToolTip cho các controls
            SetupSuperToolTips();
            
            LoadImageData();
        }

        #endregion

        #region ========== KHỞI TẠO FORM ==========

        /// <summary>
        /// Khởi tạo BLL
        /// </summary>
        private void InitializeBll()
        {
            _productImageBll = new ProductImageBll();
        }

        /// <summary>
        /// Khởi tạo events
        /// </summary>
        private void InitializeEvents()
        {
            // Event cho zoom track bar
            zoomTrackBarControl1.EditValueChanged += ZoomTrackBarControl1_EditValueChanged;
            
            // Event cho các buttons
            SetPrimaryImageSimpleButton.Click += SetPrimaryImageSimpleButton_Click;
            DownLoadImageSimpleButton.Click += DownLoadImageSimpleButton_Click;
            DeleteImageSimpleButton.Click += DeleteImageSimpleButton_Click;
        }

        #endregion

        #region ========== QUẢN LÝ DỮ LIỆU ==========

        /// <summary>
        /// Load dữ liệu hình ảnh
        /// </summary>
        private void LoadImageData()
        {
            try
            {
                // Lấy thông tin hình ảnh từ BLL
                var imageEntity = _productImageBll.GetById(_productImageId);
                if (imageEntity == null)
                {
                    MsgBox.ShowError("Không tìm thấy hình ảnh với ID đã cho.");
                    Close();
                    return;
                }

                // Convert sang DTO
                _currentImageDto = new ProductImageDto
                {
                    Id = imageEntity.Id,
                    ProductId = imageEntity.ProductId ?? Guid.Empty,
                    VariantId = imageEntity.VariantId,
                    ImagePath = imageEntity.ImagePath,
                    SortOrder = imageEntity.SortOrder ?? 0,
                    IsPrimary = imageEntity.IsPrimary ?? false,
                    ImageData = imageEntity.ImageData?.ToArray(),
                    ImageType = imageEntity.ImageType,
                    ImageSize = imageEntity.ImageSize ?? 0,
                    ImageWidth = imageEntity.ImageWidth ?? 0,
                    ImageHeight = imageEntity.ImageHeight ?? 0,
                    Caption = imageEntity.Caption,
                    AltText = imageEntity.AltText,
                    IsActive = imageEntity.IsActive ?? false,
                    CreatedDate = imageEntity.CreatedDate ?? DateTime.Now,
                    ModifiedDate = imageEntity.ModifiedDate
                };

                // Load hình ảnh vào PictureEdit
                LoadImageToPictureEdit();

                // Hiển thị thông tin chi tiết
                DisplayImageDetail();

                // Cấu hình zoom track bar
                ConfigureZoomTrackBar();
            }
            catch (Exception ex)
            {
                MsgBox.ShowError($"Lỗi khi load dữ liệu hình ảnh: {ex.Message}");
            }
        }

        /// <summary>
        /// Load hình ảnh vào PictureEdit
        /// </summary>
        private void LoadImageToPictureEdit()
        {
            try
            {
                if (_currentImageDto?.ImageData != null && _currentImageDto.ImageData.Length > 0)
                {
                    // Sử dụng ImageData từ database
                    using (var ms = new MemoryStream(_currentImageDto.ImageData))
                    {
                        ProductImagePictureEdit.Image = Image.FromStream(ms);
                    }
                }
                else if (!string.IsNullOrEmpty(_currentImageDto?.ImagePath) && File.Exists(_currentImageDto.ImagePath))
                {
                    // Sử dụng ImagePath nếu có
                    ProductImagePictureEdit.Image = Image.FromFile(_currentImageDto.ImagePath);
                }
                else
                {
                    // Hiển thị placeholder nếu không có hình ảnh
                    ProductImagePictureEdit.Image = CreatePlaceholderImage();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Lỗi khi load hình ảnh: {ex.Message}");
                ProductImagePictureEdit.Image = CreatePlaceholderImage();
            }
        }

        /// <summary>
        /// Tạo placeholder image khi không có hình ảnh
        /// </summary>
        private Image CreatePlaceholderImage()
        {
            try
            {
                var bitmap = new Bitmap(400, 300);
                using (var g = Graphics.FromImage(bitmap))
                {
                    g.Clear(Color.LightGray);
                    g.DrawString("Không có hình ảnh", 
                                new Font("Arial", 16, FontStyle.Bold), 
                                Brushes.DarkGray, 
                                new PointF(150, 140));
                }
                return bitmap;
            }
            catch
            {
                return new Bitmap(400, 300);
            }
        }

        /// <summary>
        /// Hiển thị thông tin chi tiết hình ảnh
        /// </summary>
        private void DisplayImageDetail()
        {
            try
            {
                if (_currentImageDto == null) return;

                var detailText = $"🖼️ THÔNG TIN CHI TIẾT HÌNH ẢNH{Environment.NewLine}{Environment.NewLine}" +
                               $"📝 Tên: {_currentImageDto.Caption ?? "Không có"}{Environment.NewLine}" +
                               $"📄 Mô tả: {_currentImageDto.AltText ?? "Không có"}{Environment.NewLine}" +
                               $"🆔 ID: {_currentImageDto.Id}{Environment.NewLine}" +
                               $"⭐ Ảnh chính: {(_currentImageDto.IsPrimary ? "Có" : "Không")}{Environment.NewLine}" +
                               $"📏 Kích thước: {FormatImageDimensions(_currentImageDto.ImageWidth, _currentImageDto.ImageHeight)}{Environment.NewLine}" +
                               $"💾 Dung lượng: {FormatFileSize(_currentImageDto.ImageSize)}{Environment.NewLine}" +
                               $"📁 Loại file: {_currentImageDto.ImageType?.ToUpper() ?? "Không xác định"}{Environment.NewLine}" +
                               $"🔢 Thứ tự: {_currentImageDto.SortOrder}{Environment.NewLine}" +
                               $"📅 Ngày tạo: {_currentImageDto.CreatedDate:dd/MM/yyyy HH:mm:ss}{Environment.NewLine}";

                if (_currentImageDto.ModifiedDate.HasValue)
                {
                    detailText += $"📅 Ngày sửa: {_currentImageDto.ModifiedDate.Value:dd/MM/yyyy HH:mm:ss}{Environment.NewLine}";
                }

                if (!string.IsNullOrEmpty(_currentImageDto.ImagePath))
                {
                    detailText += $"📂 Đường dẫn: {_currentImageDto.ImagePath}{Environment.NewLine}";
                }

                detailText += $"{Environment.NewLine}📊 THỐNG KÊ:{Environment.NewLine}" +
                            $"• Trạng thái: {(_currentImageDto.IsActive ? "Hoạt động" : "Không hoạt động")}{Environment.NewLine}" +
                            $"• Có dữ liệu ảnh: {(_currentImageDto.ImageData != null && _currentImageDto.ImageData.Length > 0 ? "Có" : "Không")}{Environment.NewLine}";

                if (_currentImageDto.ImageData != null && _currentImageDto.ImageData.Length > 0)
                {
                    detailText += $"• Kích thước dữ liệu: {FormatFileSize(_currentImageDto.ImageData.Length)}{Environment.NewLine}";
                }

                detailText += $"{Environment.NewLine}⏰ Thời gian xem: {DateTime.Now:HH:mm:ss dd/MM/yyyy}";

                ImageDetailMemoEdit.Text = detailText;
            }
            catch (Exception ex)
            {
                ImageDetailMemoEdit.Text = $@"Lỗi khi hiển thị thông tin chi tiết: {ex.Message}";
            }
        }

        /// <summary>
        /// Cấu hình zoom track bar theo DevExpress demo
        /// </summary>
        private void ConfigureZoomTrackBar()
        {
            try
            {
                // Cấu hình zoom track bar theo DevExpress demo
                zoomTrackBarControl1.Properties.Minimum = 10;   // 10% zoom tối thiểu
                zoomTrackBarControl1.Properties.Maximum = 500;  // 500% zoom tối đa
                zoomTrackBarControl1.Value = 100; // 100% zoom mặc định
                
                // Cấu hình step và tick frequency
                zoomTrackBarControl1.Properties.SmallChange = 10;  // Bước nhảy nhỏ
                zoomTrackBarControl1.Properties.LargeChange = 50;  // Bước nhảy lớn
                zoomTrackBarControl1.Properties.TickFrequency = 50; // Tần suất tick
                
                // Cấu hình hiển thị
                zoomTrackBarControl1.Properties.ShowValueToolTip = true;
                zoomTrackBarControl1.Properties.ShowLabels = true;
                
                // Cấu hình PictureEdit để zoom hoạt động đúng
                ProductImagePictureEdit.Properties.SizeMode = PictureSizeMode.Clip;
                ProductImagePictureEdit.Properties.ZoomPercent = 100;
                
                Debug.WriteLine("Đã cấu hình zoom track bar: 10%-500%");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Lỗi khi cấu hình zoom track bar: {ex.Message}");
            }
        }

        #endregion

        #region ========== SỰ KIỆN FORM ==========

        /// <summary>
        /// Xử lý sự kiện thay đổi zoom track bar
        /// </summary>
        private void ZoomTrackBarControl1_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                var zoomValue = zoomTrackBarControl1.Value;
                
                // Cập nhật zoom theo cách DevExpress demo
                if (ProductImagePictureEdit.Image != null)
                {
                    // Set SizeMode to Clip để zoom hoạt động đúng
                    ProductImagePictureEdit.Properties.SizeMode = PictureSizeMode.Clip;
                    
                    // Cập nhật ZoomPercent - đây là cách chính để zoom
                    ProductImagePictureEdit.Properties.ZoomPercent = zoomValue;
                    
                    // Refresh để hiển thị thay đổi
                    ProductImagePictureEdit.Refresh();
                    
                    Debug.WriteLine($"Zoom: {zoomValue}%");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Lỗi khi thay đổi zoom: {ex.Message}");
            }
        }

        /// <summary>
        /// Xử lý sự kiện click nút "Đặt làm ảnh chính"
        /// </summary>
        private void SetPrimaryImageSimpleButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (_currentImageDto == null) return;

                if (MsgBox.ShowYesNo("Bạn có chắc chắn muốn đặt hình ảnh này làm ảnh chính?"))
                {
                    // Set primary image trong database
                    _productImageBll.SetAsPrimary(_currentImageDto.Id);
                    
                    MsgBox.ShowSuccess($"Đã đặt hình ảnh '{_currentImageDto.Caption}' làm ảnh chính");
                    
                    // Cập nhật trạng thái local
                    _currentImageDto.IsPrimary = true;
                    DisplayImageDetail(); // Refresh thông tin
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowError($"Lỗi khi đặt ảnh chính: {ex.Message}");
            }
        }

        /// <summary>
        /// Xử lý sự kiện click nút "Tải ảnh"
        /// </summary>
        private void DownLoadImageSimpleButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (_currentImageDto == null) return;

                // Mở SaveFileDialog để chọn nơi lưu
                using (var saveDialog = new SaveFileDialog())
                {
                    saveDialog.Filter = @"Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif|All Files|*.*";
                    
                    // Sử dụng tên sản phẩm làm tên file mặc định
                    var productName = GetProductName(_currentImageDto.ProductId);
                    var imageCaption = _currentImageDto.Caption ?? "image";
                    var fileName = $"{productName}_{imageCaption}.jpg";
                    
                    // Làm sạch tên file (loại bỏ ký tự không hợp lệ)
                    fileName = CleanFileName(fileName);
                    saveDialog.FileName = fileName;
                    
                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Lưu hình ảnh
                        if (_currentImageDto.ImageData != null && _currentImageDto.ImageData.Length > 0)
                        {
                            File.WriteAllBytes(saveDialog.FileName, _currentImageDto.ImageData);
                            MsgBox.ShowSuccess($"Đã tải hình ảnh thành công: {saveDialog.FileName}");
                        }
                        else
                        {
                            MsgBox.ShowWarning("Không có dữ liệu hình ảnh để tải xuống.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowError($"Lỗi khi tải ảnh: {ex.Message}");
            }
        }

        /// <summary>
        /// Xử lý sự kiện click nút "Xóa Ảnh"
        /// </summary>
        private void DeleteImageSimpleButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (_currentImageDto == null) return;

                // Kiểm tra xem có phải ảnh chính không
                var isPrimary = _currentImageDto.IsPrimary;
                var warningMessage = isPrimary 
                    ? "Bạn có chắc chắn muốn xóa hình ảnh chính này? Hành động này sẽ:\n• Xóa ảnh khỏi database\n• Xóa file ảnh khỏi thư mục lưu trữ\n• Cập nhật thông tin sản phẩm"
                    : "Bạn có chắc chắn muốn xóa hình ảnh này? Hành động này sẽ:\n• Xóa ảnh khỏi database\n• Xóa file ảnh khỏi thư mục lưu trữ";

                if (MsgBox.ShowYesNo(warningMessage))
                {
                    // Hiển thị WaitingForm thay vì message box
                    try
                    {
                        SplashScreenManager.ShowForm(typeof(WaitForm1));
                        
                        // Xóa hình ảnh hoàn chỉnh (database + file + cập nhật ProductService)
                        _productImageBll.DeleteImageComplete(_currentImageDto.Id);
                        
                        // Set flag để báo hiệu đã xóa hình ảnh
                        WasImageDeleted = true;
                        
                        // Đóng form sau khi xóa thành công
                        Close();
                    }
                    catch (Exception ex)
                    {
                        MsgBox.ShowError($"Lỗi khi xóa hình ảnh: {ex.Message}");
                    }
                    finally
                    {
                        SplashScreenManager.CloseForm();
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowError($"Lỗi khi xóa ảnh: {ex.Message}");
            }
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
                if (SetPrimaryImageSimpleButton != null)
                {
                    var superTip = SuperToolTipHelper.CreateSuperToolTip(
                        title: "<b><color=Blue>⭐ Đặt làm ảnh chính</color></b>",
                        content: "Đặt hình ảnh này làm ảnh chính của sản phẩm/dịch vụ."
                    );
                    SetPrimaryImageSimpleButton.SuperTip = superTip;
                }

                if (DownLoadImageSimpleButton != null)
                {
                    var superTip = SuperToolTipHelper.CreateSuperToolTip(
                        title: "<b><color=Green>💾 Tải ảnh</color></b>",
                        content: "Tải hình ảnh xuống máy tính."
                    );
                    DownLoadImageSimpleButton.SuperTip = superTip;
                }

                if (DeleteImageSimpleButton != null)
                {
                    var superTip = SuperToolTipHelper.CreateSuperToolTip(
                        title: "<b><color=Red>🗑️ Xóa Ảnh</color></b>",
                        content: "Xóa hình ảnh khỏi hệ thống."
                    );
                    DeleteImageSimpleButton.SuperTip = superTip;
                }

                if (zoomTrackBarControl1 != null)
                {
                    SuperToolTipHelper.SetBaseEditSuperTip(
                        zoomTrackBarControl1,
                        title: "<b><color=Purple>🔍 Zoom</color></b>",
                        content: "Điều chỉnh mức độ phóng to/thu nhỏ hình ảnh (10% - 500%)."
                    );
                }
            }
            catch (Exception ex)
            {
                // Ignore lỗi setup SuperToolTip để không chặn form
                System.Diagnostics.Debug.WriteLine($"Lỗi setup SuperToolTip: {ex.Message}");
            }
        }

        /// <summary>
        /// Lấy tên sản phẩm theo ProductId
        /// </summary>
        private string GetProductName(Guid productId)
        {
            try
            {
                
                // Có thể sử dụng ProductServiceBll để lấy tên sản phẩm
                return "Product"; // Tạm thời return giá trị mặc định
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Lỗi khi lấy tên sản phẩm: {ex.Message}");
                return "Product";
            }
        }

        /// <summary>
        /// Format kích thước hình ảnh một cách chuyên nghiệp
        /// </summary>
        private string FormatImageDimensions(int width, int height)
        {
            try
            {
                if (width <= 0 || height <= 0)
                    return "Không xác định";

                // Tính tỷ lệ khung hình
                var aspectRatio = (double)width / height;
                string ratioText = "";
                
                if (Math.Abs(aspectRatio - 16.0/9.0) < 0.01)
                    ratioText = " (16:9)";
                else if (Math.Abs(aspectRatio - 4.0/3.0) < 0.01)
                    ratioText = " (4:3)";
                else if (Math.Abs(aspectRatio - 1.0) < 0.01)
                    ratioText = " (1:1)";
                else if (Math.Abs(aspectRatio - 3.0/2.0) < 0.01)
                    ratioText = " (3:2)";

                // Xác định độ phân giải
                string resolutionText = "";
                var totalPixels = width * height;
                
                if (totalPixels >= 2073600) // 1920x1080
                    resolutionText = " (HD)";
                else if (totalPixels >= 921600) // 1280x720
                    resolutionText = " (HD Ready)";
                else if (totalPixels >= 230400) // 640x360
                    resolutionText = " (SD)";

                return $"{width:N0} × {height:N0} pixels{ratioText}{resolutionText}";
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Lỗi khi format kích thước hình ảnh: {ex.Message}");
                return $"{width} × {height} pixels";
            }
        }

        /// <summary>
        /// Format dung lượng file một cách chuyên nghiệp
        /// </summary>
        private string FormatFileSize(long bytes)
        {
            try
            {
                if (bytes <= 0)
                    return "0 B";

                string[] sizes = { "B", "KB", "MB", "GB", "TB" };
                double len = bytes;
                int order = 0;
                
                while (len >= 1024 && order < sizes.Length - 1)
                {
                    order++;
                    len = len / 1024;
                }

                // Format với số thập phân phù hợp
                string format = order == 0 ? "0" : "0.#";
                return $"{len.ToString(format)} {sizes[order]}";
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Lỗi khi format dung lượng file: {ex.Message}");
                return $"{bytes:N0} bytes";
            }
        }

        /// <summary>
        /// Làm sạch tên file (loại bỏ ký tự không hợp lệ)
        /// </summary>
        private string CleanFileName(string fileName)
        {
            try
            {
                var invalidChars = Path.GetInvalidFileNameChars();
                var cleanFileName = fileName;
                
                foreach (var invalidChar in invalidChars)
                {
                    cleanFileName = cleanFileName.Replace(invalidChar, '_');
                }
                
                // Loại bỏ khoảng trắng thừa và ký tự đặc biệt
                cleanFileName = cleanFileName.Trim();
                cleanFileName = cleanFileName.Replace(" ", "_");
                cleanFileName = cleanFileName.Replace("__", "_");
                
                return cleanFileName;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Lỗi khi làm sạch tên file: {ex.Message}");
                return "image";
            }
        }

        #endregion
    }
}