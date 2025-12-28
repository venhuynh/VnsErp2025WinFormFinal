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

        /// <summary>
        /// RowHandle đang được edit (để lấy ContactId khi upload avatar)
        /// </summary>
        private int _editingRowHandle = GridControl.InvalidRowHandle;

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
            BusinessPartnerContactGridCardView.ShownEditor += BusinessPartnerContactGridCardView_ShownEditor;
            BusinessPartnerContactGridCardView.HiddenEditor += BusinessPartnerContactGridCardView_HiddenEditor;

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
                // GetAll() already returns List<BusinessPartnerContactDto>
                var contactDtos = _contactBll.GetAll();

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
                            // SaveOrUpdate expects BusinessPartnerContactDto, not Entity
                            _contactBll.SaveOrUpdate(dto);

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

        /// <summary>
        /// Xử lý sự kiện khi editor được hiển thị (lưu rowHandle đang edit)
        /// </summary>
        private void BusinessPartnerContactGridCardView_ShownEditor(object sender, EventArgs e)
        {
            try
            {
                if (sender is not CardView view) return;
                _editingRowHandle = view.FocusedRowHandle;
            }
            catch (Exception)
            {
                // ignore
            }
        }

        /// <summary>
        /// Xử lý sự kiện khi editor bị ẩn (clear rowHandle)
        /// </summary>
        private void BusinessPartnerContactGridCardView_HiddenEditor(object sender, EventArgs e)
        {
            try
            {
                _editingRowHandle = GridControl.InvalidRowHandle;
            }
            catch (Exception)
            {
                // ignore
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
        /// Xử lý sự kiện ImageChanged của RepositoryItemPictureEdit để cập nhật avatar liên hệ
        /// </summary>
        private async void ContactAvatarPictureEdit_ImageChanged(object sender, EventArgs e)
        {
            if (_isLoading) return;

            try
            {
                if (sender is not PictureEdit pictureEdit) return;

                // Lấy row đang được edit
                if (_editingRowHandle < 0 || _editingRowHandle == GridControl.InvalidRowHandle)
                {
                    // Fallback: lấy từ focused row
                    _editingRowHandle = BusinessPartnerContactGridCardView.FocusedRowHandle;
                }

                if (_editingRowHandle < 0 || _editingRowHandle == GridControl.InvalidRowHandle)
                {
                    return; // Không có row nào đang được edit
                }

                // Lấy DTO từ row
                if (BusinessPartnerContactGridCardView.GetRow(_editingRowHandle) is not BusinessPartnerContactDto contactDto)
                {
                    return;
                }

                var contactId = contactDto.Id;

                // Xử lý upload avatar
                await ExecuteWithWaitingFormAsync(async () =>
                {
                    if (pictureEdit.Image != null)
                    {
                        // Trường hợp có hình ảnh mới - UPLOAD
                        var imageBytes = ImageToByteArray(pictureEdit.Image);

                        // Kiểm tra kích thước hình ảnh (tối đa 10MB)
                        const int maxSizeInBytes = 10 * 1024 * 1024; // 10MB
                        if (imageBytes.Length > maxSizeInBytes)
                        {
                            MsgBox.ShowWarning("Hình ảnh quá lớn! Vui lòng chọn hình ảnh nhỏ hơn 10MB.");
                            return;
                        }

                        // Kiểm tra format hình ảnh
                        if (!IsValidImageFormat(imageBytes))
                        {
                            MsgBox.ShowWarning(
                                "Định dạng hình ảnh không được hỗ trợ! Vui lòng chọn file JPG, PNG hoặc GIF.");
                            return;
                        }

                        // Cập nhật avatar (lưu thumbnail trong database)
                        // Lưu ý: BusinessPartnerContact chỉ lưu thumbnail, không có logic upload file gốc lên NAS như BusinessPartner
                        _contactBll.UpdateAvatarOnly(contactId, imageBytes);

                        ShowInfo("Đã cập nhật avatar liên hệ thành công!");

                        // Reload data để cập nhật avatar mới
                        await LoadDataAsyncWithoutSplash();
                    }
                    else
                    {
                        // Trường hợp hình ảnh bị xóa - có thể xóa avatar nếu cần
                        // Hiện tại không xóa, chỉ bỏ qua
                        System.Diagnostics.Debug.WriteLine($"Avatar đã bị xóa cho liên hệ {contactId}");
                    }
                });
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi cập nhật avatar liên hệ");
            }
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

                    // Kiểm tra kích thước hình ảnh (tối đa 10MB)
                    const int maxSizeInBytes = 10 * 1024 * 1024; // 10MB
                    if (imageBytes.Length > maxSizeInBytes)
                    {
                        MsgBox.ShowWarning("Hình ảnh quá lớn! Vui lòng chọn hình ảnh nhỏ hơn 10MB.");
                        return;
                    }

                    // Kiểm tra format hình ảnh
                    if (!IsValidImageFormat(imageBytes))
                    {
                        MsgBox.ShowWarning(
                            "Định dạng hình ảnh không được hỗ trợ! Vui lòng chọn file JPG, PNG hoặc GIF.");
                        return;
                    }

                    await ExecuteWithWaitingFormAsync(async () =>
                    {
                        _contactBll.UpdateAvatarOnly(contactDto.Id, imageBytes);
                        // Reload data để cập nhật avatar mới
                        await LoadDataAsyncWithoutSplash();
                    });

                    ShowInfo("Đã cập nhật avatar thành công!");
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khi load avatar");
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

                    // Kiểm tra kích thước hình ảnh (tối đa 10MB)
                    const int maxSizeInBytes = 10 * 1024 * 1024; // 10MB
                    if (imageBytes.Length > maxSizeInBytes)
                    {
                        MsgBox.ShowWarning("Hình ảnh quá lớn! Vui lòng chọn hình ảnh nhỏ hơn 10MB.");
                        return;
                    }

                    // Kiểm tra format hình ảnh
                    if (!IsValidImageFormat(imageBytes))
                    {
                        MsgBox.ShowWarning(
                            "Định dạng hình ảnh không được hỗ trợ! Vui lòng chọn file JPG, PNG hoặc GIF.");
                        return;
                    }

                    await ExecuteWithWaitingFormAsync(async () =>
                    {
                        _contactBll.UpdateAvatarOnly(contactDto.Id, imageBytes);
                        // Reload data để cập nhật avatar mới
                        await LoadDataAsyncWithoutSplash();
                    });

                    ShowInfo("Đã paste avatar thành công!");
                }
                else
                {
                    MsgBox.ShowWarning("Clipboard không chứa hình ảnh.");
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khi paste avatar");
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
        /// Kiểm tra định dạng hình ảnh có hợp lệ không (JPG, PNG, GIF)
        /// </summary>
        /// <param name="imageBytes">Byte array của hình ảnh</param>
        /// <returns>True nếu format hợp lệ</returns>
        private bool IsValidImageFormat(byte[] imageBytes)
        {
            if (imageBytes == null || imageBytes.Length < 4) return false;

            // Kiểm tra magic bytes
            // JPEG: FF D8 FF
            if (imageBytes[0] == 0xFF && imageBytes[1] == 0xD8 && imageBytes[2] == 0xFF)
                return true;

            // PNG: 89 50 4E 47
            if (imageBytes[0] == 0x89 && imageBytes[1] == 0x50 && imageBytes[2] == 0x4E && imageBytes[3] == 0x47)
                return true;

            // GIF: 47 49 46 38 (GIF8)
            if (imageBytes[0] == 0x47 && imageBytes[1] == 0x49 && imageBytes[2] == 0x46 && imageBytes[3] == 0x38)
                return true;

            return false;
        }

        /// <summary>
        /// Hiển thị thông tin.
        /// </summary>
        private void ShowInfo(string message)
        {
            MsgBox.ShowSuccess(message);
        }

        /// <summary>
        /// Hiển thị lỗi với thông tin ngữ cảnh.
        /// </summary>
        private void ShowError(Exception ex, string context = null)
        {
            if (string.IsNullOrWhiteSpace(context))
                MsgBox.ShowException(ex);
            else
                MsgBox.ShowException(new Exception(context + ": " + ex.Message, ex));
        }

        #endregion
    }
}