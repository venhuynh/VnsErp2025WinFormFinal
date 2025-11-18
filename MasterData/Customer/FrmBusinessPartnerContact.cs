using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Card;
using DevExpress.XtraSplashScreen;
using DTO.MasterData.CustomerPartner;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bll.MasterData.CustomerBll;
using Common.Common;
using Common.Utils;

namespace MasterData.Customer
{
    /// <summary>
    /// UserControl quản lý danh sách liên hệ đối tác dạng CardView.
    /// Cung cấp chức năng CRUD đầy đủ với CardView, tìm kiếm toàn diện và giao diện thân thiện.
    /// </summary>
    public partial class FrmBusinessPartnerContact : XtraForm
    {
        #region ========== KHAI BÁO BIẾN ==========

        /// <summary>
        /// Business Logic Layer cho liên hệ đối tác
        /// </summary>
        private readonly BusinessPartnerContactBll _contactBll = new();

        /// <summary>
        /// Danh sách ID của các liên hệ được chọn
        /// </summary>
        private readonly List<Guid> _selectedContactIds = [];

        /// <summary>
        /// Guard tránh gọi LoadDataAsync song song
        /// </summary>
        private bool _isLoading;

        #endregion

        #region ========== CONSTRUCTOR & PUBLIC METHODS ==========

        /// <summary>
        /// Khởi tạo control, đăng ký event UI
        /// </summary>
        public FrmBusinessPartnerContact()
        {
            InitializeComponent();
            RegisterEvents();

            UpdateButtonStates();

            // Setup SuperToolTips
            SetupSuperToolTips();
        }

        /// <summary>
        /// Tải dữ liệu liên hệ đối tác
        /// </summary>
        private async Task LoadDataAsync()
        {
            if (_isLoading) return;

            try
            {
                _isLoading = true;
                await ExecuteWithWaitingFormAsync(async () => { await LoadDataAsyncWithoutSplash(); });
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(new Exception("Lỗi tải dữ liệu: " + ex.Message, ex));
            }
            finally
            {
                _isLoading = false;
            }
        }

        #endregion

        #region ========== KHỞI TẠO FORM ==========

        /// <summary>
        /// Đăng ký các sự kiện UI
        /// </summary>
        private void RegisterEvents()
        {
            // Bar button events
            ListDataBarButtonItem.ItemClick += ListDataBarButtonItem_ItemClick;
            NewBarButtonItem.ItemClick += NewBarButtonItem_ItemClick;
            DeleteBarButtonItem.ItemClick += DeleteBarButtonItem_ItemClick;
            ExportBarButtonItem.ItemClick += ExportBarButtonItem_ItemClick;

            // CardView events
            BusinessPartnerContactGridCardView.SelectionChanged += BusinessPartnerContactGridCardView_SelectionChanged;
            BusinessPartnerContactGridCardView.KeyDown += BusinessPartnerContactGridCardView_KeyDown;
            BusinessPartnerContactGridCardView.CellValueChanged += BusinessPartnerContactGridCardView_CellValueChanged;
            BusinessPartnerContactGridCardView.ValidatingEditor += BusinessPartnerContactGridCardView_ValidatingEditor;
            BusinessPartnerContactGridCardView.ValidateRow += BusinessPartnerContactGridCardView_ValidateRow;

            // PictureEdit events - đăng ký ContextButtonClick cho RepositoryItemPictureEdit
            if (colAvatar?.ColumnEdit is RepositoryItemPictureEdit pictureEdit)
            {
                pictureEdit.ContextButtonClick += ContactAvatarPictureEdit_ContextButtonClick;
            }
        }

        /// <summary>
        /// Thực hiện operation async với WaitingForm1 hiển thị
        /// </summary>
        /// <param name="operation">Operation async cần thực hiện</param>
        private static async Task ExecuteWithWaitingFormAsync(Func<Task> operation)
        {
            var splashShown = false;
            try
            {
                // Kiểm tra xem đã có splash screen chưa bằng cách thử đóng trước
                try
                {
                    SplashScreenManager.CloseForm();
                }
                catch
                {
                    // Nếu không có splash screen thì sẽ có exception, bỏ qua
                }

                // Hiển thị WaitingForm1
                SplashScreenManager.ShowForm(typeof(WaitForm1));
                splashShown = true;

                // Thực hiện operation
                await operation();
            }
            finally
            {
                // Đóng splash screen
                if (splashShown)
                {
                    try
                    {
                        SplashScreenManager.CloseForm();
                    }
                    catch
                    {
                        // Bỏ qua nếu có lỗi khi đóng
                    }
                }
            }
        }

        #endregion

        #region ========== QUẢN LÝ DỮ LIỆU ==========

        /// <summary>
        /// Tải dữ liệu không có splash screen
        /// </summary>
        private Task LoadDataAsyncWithoutSplash()
        {
            try
            {
                // Lấy dữ liệu từ BLL
                var contacts = _contactBll.GetAll();
                var contactDtos = contacts.Select(e => e.ToDto()).ToList();

                // Bind dữ liệu vào GridControl
                BindGrid(contactDtos);

                // Cập nhật trạng thái button và status bar
                UpdateButtonStates();
                UpdateStatusBar();

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(new Exception("Lỗi tải dữ liệu: " + ex.Message, ex));
                return Task.CompletedTask;
            }
        }

        /// <summary>
        /// Bind dữ liệu vào GridControl
        /// </summary>
        /// <param name="contactDtos">Danh sách DTO cần bind</param>
        private void BindGrid(List<BusinessPartnerContactDto> contactDtos)
        {
            try
            {
                BusinessPartnerContactGridControl.DataSource = contactDtos;
                ConfigureCardView();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(new Exception("Lỗi bind dữ liệu: " + ex.Message, ex));
            }
        }

        /// <summary>
        /// Cấu hình CardView
        /// </summary>
        private void ConfigureCardView()
        {
            try
            {
                // Cấu hình cơ bản cho CardView
                // Hầu hết cấu hình đã được thực hiện trong Designer
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(new Exception("Lỗi cấu hình CardView: " + ex.Message, ex));
            }
        }

        /// <summary>
        /// Cập nhật trạng thái các button
        /// </summary>
        private void UpdateButtonStates()
        {
            try
            {
                var rowCount = BusinessPartnerContactGridCardView.RowCount;
                var hasSelection = _selectedContactIds.Count > 0;

                // Cập nhật trạng thái button
                DeleteBarButtonItem.Enabled = hasSelection;
                ExportBarButtonItem.Enabled = rowCount > 0;
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(new Exception("Lỗi cập nhật trạng thái button: " + ex.Message, ex));
            }
        }

        /// <summary>
        /// Cập nhật status bar
        /// </summary>
        private void UpdateStatusBar()
        {
            try
            {
                UpdateDataSummaryStatus();
                UpdateSelectedRowStatus();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(new Exception("Lỗi cập nhật status bar: " + ex.Message, ex));
            }
        }

        /// <summary>
        /// Cập nhật thông tin tổng kết dữ liệu
        /// </summary>
        private void UpdateDataSummaryStatus()
        {
            try
            {
                var rowCount = BusinessPartnerContactGridCardView.RowCount;
                DataSummaryBarStaticItem.Caption = $@"Tổng số: {rowCount} liên hệ";
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(new Exception("Lỗi cập nhật data summary: " + ex.Message, ex));
            }
        }

        /// <summary>
        /// Cập nhật thông tin số dòng được chọn
        /// </summary>
        private void UpdateSelectedRowStatus()
        {
            try
            {
                var selectedCount = _selectedContactIds.Count;
                CurrentSelectBarStaticItem.Caption = $@"Đã chọn: {selectedCount} liên hệ";
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(new Exception("Lỗi cập nhật selected row status: " + ex.Message, ex));
            }
        }

        #endregion

        #region ========== SỰ KIỆN FORM ==========

        /// <summary>
        /// Xử lý sự kiện click button Tải dữ liệu
        /// </summary>
        private async void ListDataBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                await LoadDataAsync();
                UpdateStatusBar();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(new Exception("Lỗi tải dữ liệu: " + ex.Message, ex));
            }
        }

        /// <summary>
        /// Xử lý sự kiện click button Thêm mới
        /// </summary>
        private void NewBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                using var form = new FrmBusinessPartnerContactDetail();
                var result = form.ShowDialog();

                if (result == DialogResult.OK)
                {
                    ListDataBarButtonItem.PerformClick();

                    UpdateStatusBar();
                    MsgBox.ShowSuccess("Đã thêm mới liên hệ thành công!");
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(new Exception("Lỗi thêm mới liên hệ: " + ex.Message, ex));
            }
        }


        /// <summary>
        /// Xử lý sự kiện click button Xóa
        /// </summary>
        private async void DeleteBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (_selectedContactIds.Count == 0)
                {
                    MsgBox.ShowWarning("Vui lòng chọn liên hệ cần xóa.");
                    return;
                }

                
                if (!MsgBox.ShowYesNo($"Bạn có chắc chắn muốn xóa {_selectedContactIds.Count} liên hệ đã chọn?")) return;

                await ExecuteWithWaitingFormAsync(async () =>
                {
                    var deletedCount = 0;
                    var failedCount = 0;
                    var failedIds = new List<Guid>();

                    foreach (var id in _selectedContactIds.ToList())
                    {
                        try
                        {
                            var result = _contactBll.Delete(id);
                            if (result)
                            {
                                deletedCount++;
                            }
                            else
                            {
                                failedCount++;
                                failedIds.Add(id);
                            }
                        }
                        catch (Exception deleteEx)
                        {
                            failedCount++;
                            failedIds.Add(id);
                            Debug.WriteLine($"Lỗi khi xóa contact {id}: {deleteEx.Message}");
                        }
                    }

                    ClearSelectionState();
                    await LoadDataAsyncWithoutSplash();

                    // Hiển thị kết quả
                    if (failedCount == 0)
                    {
                        MsgBox.ShowSuccess($"Đã xóa thành công {deletedCount} liên hệ.");
                    }
                    else if (deletedCount > 0)
                    {
                        MsgBox.ShowWarning($"Đã xóa thành công {deletedCount} liên hệ. Không thể xóa {failedCount} liên hệ.");
                    }
                    else
                    {
                        MsgBox.ShowError($"Không thể xóa {failedCount} liên hệ. Vui lòng kiểm tra log để biết chi tiết.");
                    }
                });
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(new Exception("Lỗi xóa liên hệ: " + ex.Message, ex));
            }
        }

        /// <summary>
        /// Xử lý sự kiện click button Xuất Excel
        /// </summary>
        private void ExportBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                var rowCount = BusinessPartnerContactGridCardView.RowCount;
                if (rowCount == 0)
                {
                    MsgBox.ShowWarning("Không có dữ liệu để xuất.");
                    return;
                }

                // Tạo tên file với timestamp
                var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                var fileName = $"BusinessPartnerContacts_{timestamp}.xlsx";
                var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName);

                // Xuất dữ liệu
                BusinessPartnerContactGridControl.ExportToXlsx(path);

                // Mở file
                Process.Start(path);

                MsgBox.ShowSuccess($"Đã xuất {rowCount} liên hệ ra file: {fileName}");
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(new Exception("Lỗi xuất Excel: " + ex.Message, ex));
            }
        }

        #endregion

        #region ========== SỰ KIỆN CARDVIEW ==========

        /// <summary>
        /// Xử lý sự kiện thay đổi selection trong CardView
        /// </summary>
        private void BusinessPartnerContactGridCardView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                UpdateSelectedContactIds();
                UpdateButtonStates();
                UpdateSelectedRowStatus();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(new Exception("Lỗi cập nhật selection: " + ex.Message, ex));
            }
        }


        /// <summary>
        /// Xử lý sự kiện phím tắt trong CardView
        /// </summary>
        private void BusinessPartnerContactGridCardView_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                switch (e.KeyCode)
                {
                    case Keys.A when e.Control:
                        // Ctrl+A: Chọn tất cả
                        SelectAllCards();
                        e.Handled = true;
                        break;
                    case Keys.Escape:
                        // Escape: Bỏ chọn tất cả
                        DeselectAllCards();
                        e.Handled = true;
                        break;
                    case Keys.Delete:
                        // Delete: Xóa các item được chọn
                        if (_selectedContactIds.Count > 0)
                        {
                            DeleteBarButtonItem_ItemClick(null, null);
                            e.Handled = true;
                        }

                        break;
                    case Keys.F2:
                        // F2: Không còn chức năng edit riêng biệt, đã tích hợp vào CardView
                        e.Handled = true;
                        break;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(new Exception("Lỗi xử lý phím tắt: " + ex.Message, ex));
            }
        }

        /// <summary>
        /// Xử lý sự kiện thay đổi giá trị cell
        /// </summary>
        private void BusinessPartnerContactGridCardView_CellValueChanged(
            object sender,
            CellValueChangedEventArgs e)
        {
            try
            {
                if (e.Column.FieldName == "Avatar")
                {
                    if (BusinessPartnerContactGridCardView.GetRow(e.RowHandle) is not BusinessPartnerContactDto dto ||
                        dto.Avatar == null) return;

                    // Lưu avatar xuống DB
                    _contactBll.UpdateAvatarOnly(dto.Id, dto.Avatar);

                    MsgBox.ShowSuccess("Đã cập nhật avatar thành công!");
                }
                else if (e.Column.FieldName != "SiteName")
                {
                    // Cập nhật các trường khác (không phải SiteName và Avatar)
                    if (BusinessPartnerContactGridCardView.GetRow(e.RowHandle) is BusinessPartnerContactDto dto)
                    {
                        _ = ExecuteWithWaitingFormAsync(async () =>
                        {
                            // Convert DTO to Entity và cập nhật
                            var entity = dto.ToEntity();
                            _contactBll.SaveOrUpdate(entity);

                            // Refresh data để đảm bảo đồng bộ
                            await LoadDataAsync();
                            UpdateButtonStates();
                            UpdateStatusBar();
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(new Exception("Lỗi khi cập nhật dữ liệu: " + ex.Message, ex));
            }
        }

        /// <summary>
        /// Xử lý sự kiện ValidatingEditor của CardView
        /// Validate dữ liệu trước khi commit
        /// </summary>
        private void BusinessPartnerContactGridCardView_ValidatingEditor(object sender,
            BaseContainerValidateEditorEventArgs e)
        {
            try
            {
                // Lấy thông tin column và row
                if (sender is not CardView view) return;

                var column = view.FocusedColumn;
                if (column == null) return;

                // Bỏ qua SiteName và Avatar
                if (column.FieldName == "SiteName" || column.FieldName == "Avatar")
                {
                    e.Valid = true;
                    return;
                }

                // Validate các trường bắt buộc
                if (column.FieldName == "FullName")
                {
                    if (e.Value == null || string.IsNullOrWhiteSpace(e.Value.ToString()))
                    {
                        e.Valid = false;
                        e.ErrorText = "Họ tên không được để trống.";
                        return;
                    }
                }
                else if (column.FieldName == "Email")
                {
                    var email = e.Value?.ToString();
                    if (!string.IsNullOrWhiteSpace(email))
                    {
                        // Kiểm tra format email
                        var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
                        if (!emailRegex.IsMatch(email))
                        {
                            e.Valid = false;
                            e.ErrorText = "Định dạng email không hợp lệ.";
                            return;
                        }
                    }
                }
                else if (column.FieldName == "Phone")
                {
                    var phone = e.Value?.ToString();
                    if (!string.IsNullOrWhiteSpace(phone))
                    {
                        // Kiểm tra format số điện thoại (chỉ cho phép số, dấu +, dấu -, dấu cách, dấu ngoặc)
                        var phoneRegex = new Regex(@"^[\d\s\+\-\(\)]+$");
                        if (!phoneRegex.IsMatch(phone))
                        {
                            e.Valid = false;
                            e.ErrorText = "Định dạng số điện thoại không hợp lệ.";
                            return;
                        }
                    }
                }

                e.Valid = true;
            }
            catch (Exception ex)
            {
                e.Valid = false;
                e.ErrorText = $"Lỗi validate: {ex.Message}";
            }
        }

        /// <summary>
        /// Xử lý sự kiện ValidateRow của CardView
        /// Validate toàn bộ row trước khi commit
        /// </summary>
        private void BusinessPartnerContactGridCardView_ValidateRow(object sender,
            ValidateRowEventArgs e)
        {
            try
            {
                if (e.Row is not BusinessPartnerContactDto dto)
                {
                    e.Valid = false;
                    e.ErrorText = "Dữ liệu không hợp lệ.";
                    return;
                }

                // Validate FullName (bắt buộc)
                if (string.IsNullOrWhiteSpace(dto.FullName))
                {
                    e.Valid = false;
                    e.ErrorText = "Họ tên không được để trống.";
                    return;
                }

                // Validate Email format (nếu có)
                if (!string.IsNullOrWhiteSpace(dto.Email))
                {
                    var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
                    if (!emailRegex.IsMatch(dto.Email))
                    {
                        e.Valid = false;
                        e.ErrorText = "Định dạng email không hợp lệ.";
                        return;
                    }
                }

                // Validate Phone format (nếu có)
                if (!string.IsNullOrWhiteSpace(dto.Phone))
                {
                    var phoneRegex = new Regex(@"^[\d\s\+\-\(\)]+$");
                    if (!phoneRegex.IsMatch(dto.Phone))
                    {
                        e.Valid = false;
                        e.ErrorText = "Định dạng số điện thoại không hợp lệ.";
                        return;
                    }
                }

                e.Valid = true;
            }
            catch (Exception ex)
            {
                e.Valid = false;
                e.ErrorText = $"Lỗi validate row: {ex.Message}";
            }
        }

        #endregion

        #region ========== XỬ LÝ HÌNH ẢNH ==========

        /// <summary>
        /// Xử lý sự kiện ContextButtonClick của RepositoryItemPictureEdit
        /// </summary>
        private async void ContactAvatarPictureEdit_ContextButtonClick(object sender,
            ContextItemClickEventArgs e)
        {
            try
            {
                if (_isLoading) return;

                try
                {
                    var focusedRowHandle = BusinessPartnerContactGridCardView.FocusedRowHandle;
                    if (focusedRowHandle < 0) return;

                    if (BusinessPartnerContactGridCardView.GetRow(focusedRowHandle) is not BusinessPartnerContactDto contactDto) return;

                    switch (e.Item.Name)
                    {
                        case "Load":
                            await HandleLoadAvatar(contactDto);
                            break;
                        case "Delete":
                            await HandleDeleteAvatar(contactDto);
                            break;
                        case "Copy":
                            
                            break;
                        case "Paste":
                            await HandlePasteAvatar(contactDto);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    MsgBox.ShowException(new Exception("Lỗi xử lý context button: " + ex.Message, ex));
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// Xử lý sự kiện ImageChanged của RepositoryItemPictureEdit
        /// </summary>
        private void ContactAvatarPictureEdit_ImageChanged(object sender, EventArgs e)
        {
            if (_isLoading) return;

            if (sender is not PictureEdit pictureEdit) return;

            var selectedContactDtos = GetSelectedContactDtos();
            if (selectedContactDtos == null || selectedContactDtos.Count == 0) return;

            _ = ExecuteWithWaitingFormAsync(async () =>
            {
                foreach (var contactDto in selectedContactDtos)
                {
                    if (pictureEdit.Image != null)
                    {
                        // Trường hợp có hình ảnh mới - UPDATE
                        var imageBytes = ImageToByteArray(pictureEdit.Image);

                        // Kiểm tra kích thước hình ảnh (tối đa 5MB)
                        const int maxSizeInBytes = 5 * 1024 * 1024; // 5MB
                        if (imageBytes.Length > maxSizeInBytes)
                        {
                            MsgBox.ShowWarning("Hình ảnh quá lớn! Vui lòng chọn hình ảnh nhỏ hơn 5MB.");
                            return;
                        }

                        // Kiểm tra format hình ảnh
                        if (!IsValidImageFormat(imageBytes))
                        {
                            MsgBox.ShowWarning(
                                "Định dạng hình ảnh không được hỗ trợ! Vui lòng chọn file JPG, PNG hoặc GIF.");
                            return;
                        }

                        // Cập nhật avatar
                        _contactBll.UpdateAvatarOnly(contactDto.Id, imageBytes);
                        contactDto.Avatar = imageBytes; // Cập nhật DTO
                    }
                    else
                    {
                        // Trường hợp hình ảnh bị xóa - DELETE
                        // Chỉ xóa nếu DTO hiện tại có avatar
                        if (contactDto.Avatar != null)
                        {
                            _contactBll.DeleteAvatarOnly(contactDto.Id);
                            contactDto.Avatar = null; // Cập nhật DTO
                        }
                    }
                }

                await LoadDataAsync();
                UpdateButtonStates();
                MsgBox.ShowSuccess("Đã cập nhật avatar thành công!");
            });
        }

        /// <summary>
        /// Xử lý load avatar từ file
        /// </summary>
        private async Task HandleLoadAvatar(BusinessPartnerContactDto contactDto)
        {
            try
            {
                using var openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = @"Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp|All Files|*.*";
                openFileDialog.Title = @"Chọn hình ảnh avatar";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var imageBytes = File.ReadAllBytes(openFileDialog.FileName);

                    // Kiểm tra kích thước hình ảnh (tối đa 5MB)
                    const int maxSizeInBytes = 5 * 1024 * 1024; // 5MB
                    if (imageBytes.Length > maxSizeInBytes)
                    {
                        MsgBox.ShowWarning("Hình ảnh quá lớn! Vui lòng chọn hình ảnh nhỏ hơn 5MB.");
                        return;
                    }

                    // Kiểm tra format hình ảnh
                    if (!IsValidImageFormat(imageBytes))
                    {
                        MsgBox.ShowWarning(
                            "Định dạng hình ảnh không được hỗ trợ! Vui lòng chọn file JPG, PNG hoặc GIF.");
                        return;
                    }

                    await ExecuteWithWaitingFormAsync(() =>
                    {
                        _contactBll.UpdateAvatarOnly(contactDto.Id, imageBytes);
                        return Task.CompletedTask;
                    });

                    // Cập nhật DTO
                    contactDto.Avatar = imageBytes;

                    MsgBox.ShowSuccess("Đã cập nhật avatar thành công!");
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(new Exception("Lỗi khi load avatar: " + ex.Message, ex));
            }
        }

        /// <summary>
        /// Xử lý xóa avatar
        /// </summary>
        private async Task HandleDeleteAvatar(BusinessPartnerContactDto contactDto)
        {
            try
            {
                if (!MsgBox.ShowYesNo("Bạn có chắc chắn muốn xóa avatar này?")) return;

                await ExecuteWithWaitingFormAsync(() =>
                {
                    _contactBll.DeleteAvatarOnly(contactDto.Id);
                    return Task.CompletedTask;
                });

                // Cập nhật DTO
                contactDto.Avatar = null;

                MsgBox.ShowSuccess("Đã xóa avatar thành công!");
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(new Exception("Lỗi khi xóa avatar: " + ex.Message, ex));
            }
        }

        /// <summary>
        /// Xử lý paste avatar từ clipboard
        /// </summary>
        private async Task HandlePasteAvatar(BusinessPartnerContactDto contactDto)
        {
            try
            {
                if (Clipboard.ContainsImage())
                {
                    var image = Clipboard.GetImage();
                    var imageBytes = ImageToByteArray(image);

                    // Kiểm tra kích thước hình ảnh (tối đa 5MB)
                    const int maxSizeInBytes = 5 * 1024 * 1024; // 5MB
                    if (imageBytes.Length > maxSizeInBytes)
                    {
                        MsgBox.ShowWarning("Hình ảnh quá lớn! Vui lòng chọn hình ảnh nhỏ hơn 5MB.");
                        return;
                    }

                    // Kiểm tra format hình ảnh
                    if (!IsValidImageFormat(imageBytes))
                    {
                        MsgBox.ShowWarning(
                            "Định dạng hình ảnh không được hỗ trợ! Vui lòng chọn file JPG, PNG hoặc GIF.");
                        return;
                    }

                    await ExecuteWithWaitingFormAsync(() =>
                    {
                        _contactBll.UpdateAvatarOnly(contactDto.Id, imageBytes);
                        return Task.CompletedTask;
                    });

                    // Cập nhật DTO
                    contactDto.Avatar = imageBytes;

                    MsgBox.ShowSuccess("Đã paste avatar thành công!");
                }
                else
                {
                    MsgBox.ShowWarning("Clipboard không chứa hình ảnh.");
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(new Exception("Lỗi khi paste avatar: " + ex.Message, ex));
            }
        }

        #endregion

        #region ========== TIỆN ÍCH ==========

        /// <summary>
        /// Thiết lập SuperToolTip cho các controls trong UserControl
        /// </summary>
        private void SetupSuperToolTips()
        {
            try
            {
                if (ListDataBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        ListDataBarButtonItem,
                        title: "<b><color=Blue>🔄 Tải dữ liệu</color></b>",
                        content: "Tải lại danh sách liên hệ đối tác từ hệ thống."
                    );
                }

                if (NewBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        NewBarButtonItem,
                        title: "<b><color=Green>➕ Thêm mới</color></b>",
                        content: "Thêm mới liên hệ đối tác vào hệ thống."
                    );
                }

                if (DeleteBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        DeleteBarButtonItem,
                        title: "<b><color=Red>🗑️ Xóa</color></b>",
                        content: "Xóa các liên hệ đối tác đã chọn khỏi hệ thống."
                    );
                }

                if (ExportBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        ExportBarButtonItem,
                        title: "<b><color=Purple>📊 Xuất Excel</color></b>",
                        content: "Xuất danh sách liên hệ đối tác ra file Excel."
                    );
                }
            }
            catch (Exception ex)
            {
                // Ignore lỗi setup SuperToolTip để không chặn UserControl
                System.Diagnostics.Debug.WriteLine($"Lỗi setup SuperToolTip: {ex.Message}");
            }
        }

        /// <summary>
        /// Cập nhật danh sách ID liên hệ được chọn
        /// </summary>
        private void UpdateSelectedContactIds()
        {
            try
            {
                _selectedContactIds.Clear();
                var selectedRowHandles = BusinessPartnerContactGridCardView.GetSelectedRows();

                foreach (var rowHandle in selectedRowHandles)
                {
                    if (BusinessPartnerContactGridCardView.GetRow(rowHandle) is BusinessPartnerContactDto dto)
                    {
                        _selectedContactIds.Add(dto.Id);
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(new Exception("Lỗi cập nhật selected contact IDs: " + ex.Message, ex));
            }
        }

        /// <summary>
        /// Lấy danh sách DTO của các liên hệ được chọn
        /// </summary>
        /// <returns>Danh sách BusinessPartnerContactDto được chọn</returns>
        private List<BusinessPartnerContactDto> GetSelectedContactDtos()
        {
            try
            {
                var selectedDtos = new List<BusinessPartnerContactDto>();
                var selectedRowHandles = BusinessPartnerContactGridCardView.GetSelectedRows();

                foreach (var rowHandle in selectedRowHandles)
                {
                    if (BusinessPartnerContactGridCardView.GetRow(rowHandle) is BusinessPartnerContactDto dto)
                    {
                        selectedDtos.Add(dto);
                    }
                }

                return selectedDtos;
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(new Exception("Lỗi lấy selected contact DTOs: " + ex.Message, ex));
                return [];
            }
        }

        /// <summary>
        /// Chọn tất cả cards
        /// </summary>
        private void SelectAllCards()
        {
            try
            {
                BusinessPartnerContactGridCardView.SelectAll();
                UpdateSelectedContactIds();
                UpdateButtonStates();
                UpdateSelectedRowStatus();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(new Exception("Lỗi chọn tất cả cards: " + ex.Message, ex));
            }
        }

        /// <summary>
        /// Bỏ chọn tất cả cards
        /// </summary>
        private void DeselectAllCards()
        {
            try
            {
                BusinessPartnerContactGridCardView.ClearSelection();
                ClearSelectionState();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(new Exception("Lỗi bỏ chọn tất cả cards: " + ex.Message, ex));
            }
        }

        /// <summary>
        /// Xóa trạng thái selection
        /// </summary>
        private void ClearSelectionState()
        {
            try
            {
                _selectedContactIds.Clear();
                BusinessPartnerContactGridCardView.ClearSelection();
                BusinessPartnerContactGridCardView.FocusedRowHandle = GridControl.InvalidRowHandle;
                UpdateButtonStates();
                UpdateSelectedRowStatus();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(new Exception("Lỗi xóa selection state: " + ex.Message, ex));
            }
        }

        /// <summary>
        /// Convert Image to byte array
        /// </summary>
        /// <param name="image">Image cần convert</param>
        /// <returns>Byte array của image</returns>
        private byte[] ImageToByteArray(Image image)
        {
            try
            {
                using var ms = new MemoryStream();
                image.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(new Exception("Lỗi convert image to byte array: " + ex.Message, ex));
                return [];
            }
        }

        /// <summary>
        /// Kiểm tra format hình ảnh có hợp lệ không
        /// </summary>
        /// <param name="imageBytes">Byte array của hình ảnh</param>
        /// <returns>True nếu format hợp lệ</returns>
        private bool IsValidImageFormat(byte[] imageBytes)
        {
            try
            {
                if (imageBytes == null || imageBytes.Length < 4) return false;

                // Kiểm tra magic bytes của các format phổ biến
                var jpegSignature = new byte[] { 0xFF, 0xD8, 0xFF };
                var pngSignature = new byte[] { 0x89, 0x50, 0x4E, 0x47 };
                var gifSignature = new byte[] { 0x47, 0x49, 0x46 };
                var bmpSignature = new byte[] { 0x42, 0x4D };

                return imageBytes.Take(3).SequenceEqual(jpegSignature) ||
                       imageBytes.Take(4).SequenceEqual(pngSignature) ||
                       imageBytes.Take(3).SequenceEqual(gifSignature) ||
                       imageBytes.Take(2).SequenceEqual(bmpSignature);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(new Exception("Lỗi kiểm tra image format: " + ex.Message, ex));
                return false;
            }
        }

        #endregion
    }
}