using Bll.MasterData.Company;
using Dal.Logging;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraLayout;
using MasterData.Company.Converters;
using MasterData.Company.Dto;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using DevExpress.Utils;

namespace MasterData.Company
{
    public partial class UcCompany : XtraUserControl
    {
        private readonly CompanyBll _companyBll;
        private readonly ILogger _logger;

        public UcCompany()
        {
            InitializeComponent();
            _logger = new ConsoleLogger();
            _companyBll = new CompanyBll(_logger);
            
            // Đảm bảo chỉ có 1 công ty khi màn hình load lên
            Load += UcCompany_Load;
        }

        public UcCompany(string connectionString) : this()
        {
            _companyBll?.Dispose();
            _companyBll = new CompanyBll(connectionString, _logger);
        }

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
                
                _logger?.LogInfo("UcCompany load hoàn thành - đã đảm bảo chỉ có 1 công ty");
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Lỗi khi load UcCompany: {ex.Message}", ex);
                XtraMessageBox.Show($"Lỗi khi khởi tạo dữ liệu công ty: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

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

        /// <summary>
        /// Đánh dấu các layout item tương ứng với thuộc tính có [Required] bằng dấu * đỏ.
        /// Quy ước mapping control theo tên thuộc tính (từ editor được gán vào LayoutControlItem.Control):
        /// - Editor: "txt" + PropertyName, PropertyName + "TextEdit", hoặc chính PropertyName (BaseEdit)
        /// </summary>
        private void MarkRequiredFields(Type dtoType)
        {
            try
            {
                var requiredProps = dtoType
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.GetCustomAttributes(typeof(RequiredAttribute), true).Any())
                    .ToList();

                var allLayoutItems = GetAllLayoutControlItems(this);

                foreach (var it in allLayoutItems)
                {
                    it.AllowHtmlStringInCaption = true;
                }

                foreach (var prop in requiredProps)
                {
                    var propName = prop.Name;
                    var item = allLayoutItems.FirstOrDefault(it => IsEditorMatchProperty(it.Control, propName));
                    if (item == null) continue;

                    if (!(item.Text?.Contains("*") ?? false))
                    {
                        var baseCaption = string.IsNullOrWhiteSpace(item.Text) ? propName : item.Text;
                        item.Text = baseCaption + @" <color=red>*</color>";
                    }

                    if (item.Control is BaseEdit be && be.Properties is RepositoryItemTextEdit txtProps)
                    {
                        txtProps.NullValuePrompt = @"Bắt buộc nhập";
                        txtProps.NullValuePromptShowForEmptyValue = true;
                    }
                }
            }
            catch
            {
                // ignore marking errors
            }
        }

        private static bool IsEditorMatchProperty(Control editor, string propName)
        {
            if (editor == null) return false;
            var name = editor.Name ?? string.Empty;
            string[] candidates = new[]
            {
                name,
                name.Replace("txt", string.Empty),
                name.Replace("TextEdit", string.Empty)
            };
            return candidates.Any(c => string.Equals(c, propName, StringComparison.OrdinalIgnoreCase));
        }

        private static System.Collections.Generic.List<LayoutControlItem> GetAllLayoutControlItems(Control root)
        {
            var result = new System.Collections.Generic.List<LayoutControlItem>();
            if (root == null) return result;
            var layoutControls = root.Controls.OfType<LayoutControl>().ToList();
            var nested = root.Controls.Cast<Control>().SelectMany(c => GetAllLayoutControlItems(c)).ToList();
            foreach (var lc in layoutControls)
            {
                if (lc.Root != null)
                {
                    CollectLayoutItems(lc.Root, result);
                }
            }
            result.AddRange(nested);
            return result;
        }

        private static void CollectLayoutItems(BaseLayoutItem baseItem, System.Collections.Generic.List<LayoutControlItem> collector)
        {
            if (baseItem == null) return;
            if (baseItem is LayoutControlItem lci)
            {
                collector.Add(lci);
            }
            if (baseItem is LayoutControlGroup group)
            {
                foreach (BaseLayoutItem child in group.Items)
                {
                    CollectLayoutItems(child, collector);
                }
            }
        }

    }
}
