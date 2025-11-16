using Bll.MasterData.Company;
using Bll.Utils;
using Dal.Logging;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraLayout;
using MasterData.Company.Converters;
using MasterData.Company.Dto;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using DevExpress.Utils;

namespace MasterData.Company
{
    /// <summary>
    /// User Control quản lý thông tin công ty.
    /// Cung cấp chức năng hiển thị, cập nhật thông tin công ty và quản lý logo với validation và giao diện thân thiện.
    /// </summary>
    public partial class UcCompany : XtraUserControl
    {
        #region ========== KHAI BÁO BIẾN ==========

        /// <summary>
        /// Business Logic Layer cho công ty
        /// </summary>
        private readonly CompanyBll _companyBll;

        /// <summary>
        /// Logger để ghi log
        /// </summary>
        private readonly ILogger _logger;

        #endregion

        #region ========== CONSTRUCTOR & PUBLIC METHODS ==========

        /// <summary>
        /// Constructor mặc định
        /// </summary>
        public UcCompany()
        {
            InitializeComponent();
            _logger = new ConsoleLogger();
            _companyBll = new CompanyBll(_logger);
            
            // Đảm bảo chỉ có 1 công ty khi màn hình load lên
            Load += UcCompany_Load;
        }

        /// <summary>
        /// Constructor với connection string
        /// </summary>
        /// <param name="connectionString">Chuỗi kết nối database</param>
        public UcCompany(string connectionString) : this()
        {
            _companyBll?.Dispose();
            _companyBll = new CompanyBll(connectionString, _logger);
        }

        #endregion

        #region ========== KHỞI TẠO FORM ==========

        /// <summary>
        /// Khởi tạo BLL (đã được thực hiện trong constructor)
        /// </summary>
        private void InitializeBll()
        {
            // BLL đã được khởi tạo trong constructor
        }

        /// <summary>
        /// Khởi tạo events (đã được thực hiện trong constructor)
        /// </summary>
        private void InitializeEvents()
        {
            // Events đã được khởi tạo trong constructor
        }

        #endregion

        #region ========== QUẢN LÝ DỮ LIỆU ==========

        /// <summary>
        /// Load thông tin công ty khi form khởi tạo
        /// </summary>
        private void UcCompany_Load(object sender, EventArgs e)
        {
            try
            {
                _logger?.LogInfo("UcCompany đang load - đảm bảo chỉ có 1 công ty trong database");
                _companyBll.EnsureSingleCompany();
                
                // Đánh dấu các trường bắt buộc theo DataAnnotations của DTO
                MarkRequiredFields(typeof(CompanyDto));
                
                // Cấu hình LogoPictureEdit
                ConfigureLogoPictureEdit();
                
                // Hiển thị thông tin công ty lên các control có sẵn
                DisplayCompanyInfo();

                // Setup SuperToolTips
                SetupSuperTips();
                
                _logger?.LogInfo("UcCompany load hoàn thành - đã đảm bảo chỉ có 1 công ty");
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Lỗi khi load UcCompany: {ex.Message}", ex);
                XtraMessageBox.Show($"Lỗi khi khởi tạo dữ liệu công ty: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Hiển thị thông tin công ty lên các control
        /// </summary>
        private void DisplayCompanyInfo()
        {
            try
            {
                // Lấy thông tin công ty Entity từ database thông qua BLL

                if (_companyBll.GetCompany() is Dal.DataContext.Company company)
                {
                    // Convert Entity sang DTO trong UI layer
                    var companyDto = company.ToDto();
                    
                    // Hiển thị thông tin công ty DTO lên các control
                    CompanyCodeTextEdit.Text = companyDto.CompanyCode ?? "";
                    CompanyNameTextEdit.Text = companyDto.CompanyName ?? "";
                    TaxCodeTextEdit.Text = companyDto.TaxCode ?? "";
                    PhoneTextEdit.Text = companyDto.Phone ?? "";
                    EmailTextEdit.Text = companyDto.Email ?? "";
                    WebsiteTextEdit.Text = companyDto.Website ?? "";
                    AddressTextEdit.Text = companyDto.Address ?? "";
                    CountryTextEdit.Text = companyDto.Country ?? "";
                    CreatedDateDateEdit.EditValue = companyDto.CreatedDate;
                    
                    // Hiển thị logo nếu có
                    if (companyDto.Logo != null && companyDto.Logo.Length > 0)
                    {
                        using (var ms = new MemoryStream(companyDto.Logo))
                        {
                            LogoPictureEdit.Image = System.Drawing.Image.FromStream(ms);
                        }
                    }
                    else
                    {
                        LogoPictureEdit.Image = null;
                    }
                    
                    _logger?.LogInfo("Đã hiển thị thông tin công ty DTO từ database lên giao diện");
                }
                else
                {
                    _logger?.LogWarning("Không tìm thấy thông tin công ty trong database");
                    XtraMessageBox.Show("Không tìm thấy thông tin công ty trong database", "Cảnh báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Lỗi khi hiển thị thông tin công ty: {ex.Message}", ex);
                XtraMessageBox.Show($"Lỗi khi hiển thị thông tin công ty: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region ========== CẤU HÌNH GIAO DIỆN ==========

        /// <summary>
        /// Cấu hình LogoPictureEdit để chỉ hiển thị nút Delete và Load
        /// Sử dụng ContextMenuStrip tùy chỉnh để đảm bảo tính ổn định
        /// </summary>
        private void ConfigureLogoPictureEdit()
        {
            try
            {
                if (LogoPictureEdit != null)
                {
                    // Tắt menu mặc định
                    LogoPictureEdit.Properties.ShowMenu = false;
                    
                    // Tạo context menu tùy chỉnh
                    var contextMenu = new ContextMenuStrip();
                    
                    // Thêm menu Load
                    var loadItem = new ToolStripMenuItem("Load...");
                    loadItem.Click += LoadLogo_Click;
                    contextMenu.Items.Add(loadItem);
                    
                    // Thêm menu Delete
                    var deleteItem = new ToolStripMenuItem("Delete");
                    deleteItem.Click += DeleteLogo_Click;
                    contextMenu.Items.Add(deleteItem);
                    
                    // Gán context menu tùy chỉnh
                    LogoPictureEdit.ContextMenuStrip = contextMenu;
                    
                    // Cấu hình kích thước
                    LogoPictureEdit.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom;
                    LogoPictureEdit.Properties.ZoomPercent = 100;
                    
                    // Cho phép drag & drop
                    LogoPictureEdit.AllowDrop = true;
                    LogoPictureEdit.DragEnter += LogoPictureEdit_DragEnter;
                    LogoPictureEdit.DragDrop += LogoPictureEdit_DragDrop;
                    
                    _logger?.LogInfo("Đã cấu hình LogoPictureEdit - chỉ hiển thị nút Delete và Load");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Lỗi khi cấu hình LogoPictureEdit: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Đánh dấu các layout item tương ứng với thuộc tính có [Required] bằng dấu * đỏ.
        /// Quy ước mapping control theo tên thuộc tính (từ editor được gán vào LayoutControlItem.Control):
        /// - Editor: "txt" + PropertyName, PropertyName + "TextEdit", hoặc chính PropertyName (BaseEdit)
        /// </summary>
        private void MarkRequiredFields(Type dtoType)
        {
            try
            {
                RequiredFieldHelper.MarkRequiredFields(
                    this, 
                    dtoType,
                    logger: (msg, ex) => _logger?.LogError(msg, ex)
                );
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Lỗi đánh dấu trường bắt buộc: {ex.Message}", ex);
            }
        }

        #endregion

        #region ========== XỬ LÝ LOGO ==========

        /// <summary>
        /// Lưu logo vào database
        /// </summary>
        private void SaveLogoToDatabase(byte[] logoBytes)
        {
            try
            {
                _logger?.LogInfo("Bắt đầu lưu logo vào database");
                
                // Lấy thông tin công ty hiện tại
                if (_companyBll.GetCompany() is Dal.DataContext.Company company)
                {
                    // Cập nhật logo
                    company.Logo = logoBytes != null ? new System.Data.Linq.Binary(logoBytes) : null;
                    company.UpdatedDate = DateTime.Now;
                    
                    // Lưu vào database thông qua BLL
                    _companyBll.UpdateCompany(company);
                    
                    _logger?.LogInfo("Đã lưu logo vào database thành công");
                }
                else
                {
                    _logger?.LogWarning("Không tìm thấy thông tin công ty để cập nhật logo");
                    XtraMessageBox.Show("Không tìm thấy thông tin công ty để cập nhật logo", "Cảnh báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Lỗi khi lưu logo vào database: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Xóa logo khỏi database
        /// </summary>
        private void DeleteLogoFromDatabase()
        {
            try
            {
                _logger?.LogInfo("Bắt đầu xóa logo khỏi database");
                
                // Lấy thông tin công ty hiện tại
                if (_companyBll.GetCompany() is Dal.DataContext.Company company)
                {
                    // Xóa logo
                    company.Logo = null;
                    company.UpdatedDate = DateTime.Now;
                    
                    // Lưu vào database thông qua BLL
                    _companyBll.UpdateCompany(company);
                    
                    _logger?.LogInfo("Đã xóa logo khỏi database thành công");
                }
                else
                {
                    _logger?.LogWarning("Không tìm thấy thông tin công ty để xóa logo");
                    XtraMessageBox.Show("Không tìm thấy thông tin công ty để xóa logo", "Cảnh báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Lỗi khi xóa logo khỏi database: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Xử lý sự kiện Load logo
        /// </summary>
        private void LoadLogo_Click(object sender, EventArgs e)
        {
            try
            {
                using (var openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = @"Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif|All Files|*.*";
                    openFileDialog.Title = @"Chọn logo công ty";
                    
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        var imagePath = openFileDialog.FileName;
                        var imageBytes = File.ReadAllBytes(imagePath);
                        
                        // Hiển thị hình ảnh
                        LogoPictureEdit.Image = System.Drawing.Image.FromFile(imagePath);
                        
                        // Lưu vào database
                        SaveLogoToDatabase(imageBytes);
                        
                        _logger?.LogInfo($"Đã load logo từ file: {imagePath}");
                        XtraMessageBox.Show("Đã load logo thành công!", "Thông báo", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Lỗi khi load logo: {ex.Message}", ex);
                XtraMessageBox.Show($"Lỗi khi load logo: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Xử lý sự kiện Delete logo
        /// </summary>
        private void DeleteLogo_Click(object sender, EventArgs e)
        {
            try
            {
                if (XtraMessageBox.Show("Bạn có chắc chắn muốn xóa logo?", "Xác nhận", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // Xóa hình ảnh khỏi control
                    LogoPictureEdit.Image = null;
                    
                    // Xóa khỏi database
                    DeleteLogoFromDatabase();
                    
                    _logger?.LogInfo("Đã xóa logo");
                    XtraMessageBox.Show("Đã xóa logo thành công!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Lỗi khi xóa logo: {ex.Message}", ex);
                XtraMessageBox.Show($"Lỗi khi xóa logo: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Xử lý sự kiện DragEnter cho LogoPictureEdit
        /// </summary>
        private void LogoPictureEdit_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        /// <summary>
        /// Xử lý sự kiện DragDrop cho LogoPictureEdit
        /// </summary>
        private void LogoPictureEdit_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                    if (files.Length > 0)
                    {
                        var filePath = files[0];
                        var extension = Path.GetExtension(filePath).ToLower();
                        
                        if (new[] { ".jpg", ".jpeg", ".png", ".bmp", ".gif" }.Contains(extension))
                        {
                            // Hiển thị hình ảnh
                            LogoPictureEdit.Image = System.Drawing.Image.FromFile(filePath);
                            
                            // Lưu vào database
                            var imageBytes = File.ReadAllBytes(filePath);
                            SaveLogoToDatabase(imageBytes);
                            
                            _logger?.LogInfo($"Đã load logo từ drag & drop: {filePath}");
                            XtraMessageBox.Show("Đã load logo thành công!", "Thông báo", 
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            XtraMessageBox.Show("Vui lòng chọn file hình ảnh hợp lệ!", "Cảnh báo", 
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Lỗi khi drag & drop logo: {ex.Message}", ex);
                XtraMessageBox.Show($"Lỗi khi load logo: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region ========== SUPERTOOLTIP ==========

        /// <summary>
        /// Thiết lập SuperToolTip cho tất cả các controls trong UserControl
        /// </summary>
        private void SetupSuperTips()
        {
            try
            {
                SetupTextEditSuperTips();
                SetupDateEditSuperTips();
                SetupPictureEditSuperTips();
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Lỗi khi setup SuperToolTip: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Thiết lập SuperToolTip cho các TextEdit controls
        /// </summary>
        private void SetupTextEditSuperTips()
        {
            // SuperTip cho Mã công ty
            SuperToolTipHelper.SetTextEditSuperTip(
                CompanyCodeTextEdit,
                title: @"<b><color=DarkBlue>🏷️ Mã công ty</color></b>",
                content: @"Nhập <b>mã công ty</b> trong hệ thống.<br/><br/><b>Chức năng:</b><br/>• Nhập mã công ty (ví dụ: CT01, CT02, v.v.)<br/>• Hiển thị mã công ty khi load dữ liệu<br/>• Validation tự động khi rời khỏi control<br/><br/><b>Ràng buộc:</b><br/>• <b>Bắt buộc nhập</b> (có dấu * đỏ)<br/>• Không được để trống<br/>• Tối đa 50 ký tự<br/>• Tự động trim khoảng trắng đầu/cuối<br/><br/><b>Validation:</b><br/>• Kiểm tra rỗng khi validating<br/>• Kiểm tra độ dài tối đa (50 ký tự)<br/>• Hiển thị lỗi qua ErrorProvider nếu không hợp lệ<br/><br/><b>DataAnnotations:</b><br/>• Có attribute [Required] trong DTO<br/>• Có attribute [StringLength(50)]<br/>• Tự động đánh dấu * đỏ trong layout<br/>• Hiển thị prompt 'Bắt buộc nhập' khi rỗng<br/><br/><color=Gray>Lưu ý:</color> Mã công ty sẽ được lưu vào database khi cập nhật thông tin công ty. Hệ thống chỉ cho phép có một công ty duy nhất."
            );

            // SuperTip cho Tên công ty
            SuperToolTipHelper.SetTextEditSuperTip(
                CompanyNameTextEdit,
                title: @"<b><color=DarkBlue>🏢 Tên công ty</color></b>",
                content: @"Nhập <b>tên công ty</b> trong hệ thống.<br/><br/><b>Chức năng:</b><br/>• Nhập tên công ty đầy đủ<br/>• Hiển thị tên công ty khi load dữ liệu<br/>• Validation tự động khi rời khỏi control<br/><br/><b>Ràng buộc:</b><br/>• <b>Bắt buộc nhập</b> (có dấu * đỏ)<br/>• Không được để trống<br/>• Tối đa 255 ký tự<br/>• Không được chứa chỉ khoảng trắng<br/>• Tự động trim khoảng trắng đầu/cuối<br/><br/><b>Validation:</b><br/>• Kiểm tra rỗng khi validating<br/>• Kiểm tra độ dài tối đa (255 ký tự)<br/>• Hiển thị lỗi qua ErrorProvider nếu không hợp lệ<br/><br/><b>DataAnnotations:</b><br/>• Có attribute [Required] trong DTO<br/>• Có attribute [StringLength(255)]<br/>• Tự động đánh dấu * đỏ trong layout<br/>• Hiển thị prompt 'Bắt buộc nhập' khi rỗng<br/><br/><color=Gray>Lưu ý:</color> Tên công ty sẽ được lưu vào database khi cập nhật thông tin công ty."
            );

            // SuperTip cho Mã số thuế
            SuperToolTipHelper.SetTextEditSuperTip(
                TaxCodeTextEdit,
                title: @"<b><color=DarkBlue>📋 Mã số thuế</color></b>",
                content: @"Nhập <b>mã số thuế</b> của công ty (tùy chọn).<br/><br/><b>Chức năng:</b><br/>• Nhập mã số thuế của công ty<br/>• Hiển thị mã số thuế khi load dữ liệu<br/>• Validation tự động khi rời khỏi control<br/><br/><b>Ràng buộc:</b><br/>• <b>Không bắt buộc nhập</b> (có thể để trống)<br/>• Tối đa 50 ký tự nếu có nhập<br/>• Tự động trim khoảng trắng đầu/cuối<br/><br/><b>Validation:</b><br/>• Chỉ kiểm tra độ dài tối đa (50 ký tự) nếu có nhập<br/>• Hiển thị lỗi qua ErrorProvider nếu vượt quá độ dài<br/>• Không bắt buộc nhập<br/><br/><b>DataAnnotations:</b><br/>• Không có attribute [Required] trong DTO<br/>• Có attribute [StringLength(50)]<br/>• Có thể để trống<br/><br/><color=Gray>Lưu ý:</color> Mã số thuế sẽ được lưu vào database khi cập nhật thông tin công ty. Có thể để trống nếu không cần thiết."
            );

            // SuperTip cho Số điện thoại
            SuperToolTipHelper.SetTextEditSuperTip(
                PhoneTextEdit,
                title: @"<b><color=DarkBlue>📞 Số điện thoại</color></b>",
                content: @"Nhập <b>số điện thoại</b> của công ty (tùy chọn).<br/><br/><b>Chức năng:</b><br/>• Nhập số điện thoại công ty (ví dụ: 02812345678, 0912345678)<br/>• Hiển thị số điện thoại khi load dữ liệu<br/>• Validation tự động khi rời khỏi control<br/><br/><b>Ràng buộc:</b><br/>• <b>Không bắt buộc nhập</b> (có thể để trống)<br/>• Tối đa 50 ký tự nếu có nhập<br/>• Tự động trim khoảng trắng đầu/cuối<br/><br/><b>Validation:</b><br/>• Chỉ kiểm tra độ dài tối đa (50 ký tự) nếu có nhập<br/>• Hiển thị lỗi qua ErrorProvider nếu vượt quá độ dài<br/>• Không bắt buộc nhập<br/><br/><b>DataAnnotations:</b><br/>• Không có attribute [Required] trong DTO<br/>• Có attribute [StringLength(50)]<br/>• Có thể để trống<br/><br/><color=Gray>Lưu ý:</color> Số điện thoại sẽ được lưu vào database khi cập nhật thông tin công ty. Có thể để trống nếu không cần thiết."
            );

            // SuperTip cho Email
            SuperToolTipHelper.SetTextEditSuperTip(
                EmailTextEdit,
                title: @"<b><color=DarkBlue>📧 Email</color></b>",
                content: @"Nhập <b>địa chỉ email</b> của công ty (tùy chọn).<br/><br/><b>Chức năng:</b><br/>• Nhập địa chỉ email công ty (ví dụ: info@company.com)<br/>• Hiển thị email khi load dữ liệu<br/>• Validation tự động khi rời khỏi control<br/><br/><b>Ràng buộc:</b><br/>• <b>Không bắt buộc nhập</b> (có thể để trống)<br/>• Tối đa 100 ký tự nếu có nhập<br/>• Phải đúng định dạng email nếu có nhập<br/>• Tự động trim khoảng trắng đầu/cuối<br/><br/><b>Validation:</b><br/>• Kiểm tra định dạng email bằng EmailAddress attribute nếu có nhập<br/>• Kiểm tra độ dài tối đa (100 ký tự) nếu có nhập<br/>• Hiển thị lỗi qua ErrorProvider nếu không hợp lệ<br/>• Không bắt buộc nhập<br/><br/><b>DataAnnotations:</b><br/>• Không có attribute [Required] trong DTO<br/>• Có attribute [StringLength(100)]<br/>• Có attribute [EmailAddress]<br/>• Có thể để trống<br/><br/><color=Gray>Lưu ý:</color> Email sẽ được lưu vào database khi cập nhật thông tin công ty. Nếu có nhập, email phải đúng định dạng (ví dụ: user@domain.com)."
            );

            // SuperTip cho Website
            SuperToolTipHelper.SetTextEditSuperTip(
                WebsiteTextEdit,
                title: @"<b><color=DarkBlue>🌐 Website</color></b>",
                content: @"Nhập <b>địa chỉ website</b> của công ty (tùy chọn).<br/><br/><b>Chức năng:</b><br/>• Nhập địa chỉ website công ty (ví dụ: www.company.com)<br/>• Hiển thị website khi load dữ liệu<br/>• Validation tự động khi rời khỏi control<br/><br/><b>Ràng buộc:</b><br/>• <b>Không bắt buộc nhập</b> (có thể để trống)<br/>• Tối đa 100 ký tự nếu có nhập<br/>• Tự động trim khoảng trắng đầu/cuối<br/><br/><b>Validation:</b><br/>• Chỉ kiểm tra độ dài tối đa (100 ký tự) nếu có nhập<br/>• Hiển thị lỗi qua ErrorProvider nếu vượt quá độ dài<br/>• Không bắt buộc nhập<br/><br/><b>DataAnnotations:</b><br/>• Không có attribute [Required] trong DTO<br/>• Có attribute [StringLength(100)]<br/>• Có thể để trống<br/><br/><color=Gray>Lưu ý:</color> Website sẽ được lưu vào database khi cập nhật thông tin công ty. Có thể để trống nếu không cần thiết."
            );

            // SuperTip cho Địa chỉ
            SuperToolTipHelper.SetTextEditSuperTip(
                AddressTextEdit,
                title: @"<b><color=DarkBlue>📍 Địa chỉ</color></b>",
                content: @"Nhập <b>địa chỉ</b> của công ty (tùy chọn).<br/><br/><b>Chức năng:</b><br/>• Nhập địa chỉ công ty (ví dụ: 123 Đường ABC, Quận XYZ, TP.HCM)<br/>• Hiển thị địa chỉ khi load dữ liệu<br/>• Validation tự động khi rời khỏi control<br/><br/><b>Ràng buộc:</b><br/>• <b>Không bắt buộc nhập</b> (có thể để trống)<br/>• Tối đa 255 ký tự nếu có nhập<br/>• Tự động trim khoảng trắng đầu/cuối<br/><br/><b>Validation:</b><br/>• Chỉ kiểm tra độ dài tối đa (255 ký tự) nếu có nhập<br/>• Hiển thị lỗi qua ErrorProvider nếu vượt quá độ dài<br/>• Không bắt buộc nhập<br/><br/><b>DataAnnotations:</b><br/>• Không có attribute [Required] trong DTO<br/>• Có attribute [StringLength(255)]<br/>• Có thể để trống<br/><br/><color=Gray>Lưu ý:</color> Địa chỉ sẽ được lưu vào database khi cập nhật thông tin công ty. Có thể để trống nếu không cần thiết."
            );

            // SuperTip cho Quốc gia
            SuperToolTipHelper.SetTextEditSuperTip(
                CountryTextEdit,
                title: @"<b><color=DarkBlue>🌍 Quốc gia</color></b>",
                content: @"Nhập <b>quốc gia</b> của công ty (tùy chọn).<br/><br/><b>Chức năng:</b><br/>• Nhập quốc gia của công ty (ví dụ: Việt Nam, USA, v.v.)<br/>• Hiển thị quốc gia khi load dữ liệu<br/>• Validation tự động khi rời khỏi control<br/><br/><b>Ràng buộc:</b><br/>• <b>Không bắt buộc nhập</b> (có thể để trống)<br/>• Tối đa 100 ký tự nếu có nhập<br/>• Tự động trim khoảng trắng đầu/cuối<br/><br/><b>Validation:</b><br/>• Chỉ kiểm tra độ dài tối đa (100 ký tự) nếu có nhập<br/>• Hiển thị lỗi qua ErrorProvider nếu vượt quá độ dài<br/>• Không bắt buộc nhập<br/><br/><b>DataAnnotations:</b><br/>• Không có attribute [Required] trong DTO<br/>• Có attribute [StringLength(100)]<br/>• Có thể để trống<br/><br/><color=Gray>Lưu ý:</color> Quốc gia sẽ được lưu vào database khi cập nhật thông tin công ty. Có thể để trống nếu không cần thiết."
            );
        }

        /// <summary>
        /// Thiết lập SuperToolTip cho DateEdit controls
        /// </summary>
        private void SetupDateEditSuperTips()
        {
            // SuperTip cho Ngày tạo
            SuperToolTipHelper.SetBaseEditSuperTip(
                CreatedDateDateEdit,
                title: @"<b><color=DarkBlue>📅 Ngày tạo</color></b>",
                content: @"Hiển thị <b>ngày tạo</b> của công ty.<br/><br/><b>Chức năng:</b><br/>• Hiển thị ngày tạo công ty trong hệ thống<br/>• Tự động được set khi tạo mới công ty<br/>• Chỉ đọc (read-only)<br/><br/><b>Ràng buộc:</b><br/>• <b>Bắt buộc</b> (có dấu * đỏ)<br/>• Không được để trống<br/>• Tự động được set bởi hệ thống<br/><br/><b>Validation:</b><br/>• Kiểm tra rỗng khi validating<br/>• Hiển thị lỗi qua ErrorProvider nếu không hợp lệ<br/><br/><b>DataAnnotations:</b><br/>• Có attribute [Required] trong DTO<br/>• Tự động đánh dấu * đỏ trong layout<br/><br/><color=Gray>Lưu ý:</color> Ngày tạo được tự động set bởi hệ thống khi tạo mới công ty. Người dùng không thể chỉnh sửa trường này."
            );
        }

        /// <summary>
        /// Thiết lập SuperToolTip cho PictureEdit controls
        /// </summary>
        private void SetupPictureEditSuperTips()
        {
            // SuperTip cho Logo
            SuperToolTipHelper.SetBaseEditSuperTip(
                LogoPictureEdit,
                title: @"<b><color=DarkBlue>🖼️ Logo công ty</color></b>",
                content: @"Quản lý <b>logo công ty</b>.<br/><br/><b>Chức năng:</b><br/>• Hiển thị logo công ty<br/>• Load logo từ file (click chuột phải → Load...)<br/>• Xóa logo (click chuột phải → Delete)<br/>• Drag & drop file hình ảnh vào control<br/>• Tự động lưu vào database khi thay đổi<br/><br/><b>Định dạng hỗ trợ:</b><br/>• JPG, JPEG<br/>• PNG<br/>• BMP<br/>• GIF<br/><br/><b>Cách sử dụng:</b><br/>• <b>Load logo:</b> Click chuột phải → Load... → Chọn file hình ảnh<br/>• <b>Xóa logo:</b> Click chuột phải → Delete → Xác nhận<br/>• <b>Drag & Drop:</b> Kéo thả file hình ảnh vào control<br/><br/><b>Ràng buộc:</b><br/>• <b>Không bắt buộc</b> (có thể để trống)<br/>• Chỉ chấp nhận file hình ảnh<br/>• Tự động lưu vào database khi thay đổi<br/><br/><b>DataAnnotations:</b><br/>• Không có attribute [Required] trong DTO<br/>• Có thể để trống<br/><br/><color=Gray>Lưu ý:</color> Logo sẽ được lưu vào database ngay khi load hoặc xóa. Logo được hiển thị với chế độ Zoom để phù hợp với kích thước control."
            );
        }

        #endregion
    }
}
