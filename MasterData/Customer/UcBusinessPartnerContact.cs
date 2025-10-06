using Bll.Common;
using Bll.MasterData.Customer;
using Bll.Utils;
using DevExpress.Data;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Card;
using DevExpress.XtraSplashScreen;
using MasterData.Customer.Converters;
using MasterData.Customer.Dto;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MasterData.Customer
{
    /// <summary>
    /// UserControl quản lý danh sách liên hệ đối tác dạng CardView.
    /// Cung cấp chức năng CRUD đầy đủ với CardView, tìm kiếm toàn diện và giao diện thân thiện.
    /// </summary>
    public partial class UcBusinessPartnerContact : XtraUserControl
    {
        #region ========== KHAI BÁO BIẾN ==========

        /// <summary>
        /// Business Logic Layer cho liên hệ đối tác
        /// </summary>
        private readonly BusinessPartnerContactBll _contactBll = new BusinessPartnerContactBll();

        /// <summary>
        /// Danh sách ID của các liên hệ được chọn
        /// </summary>
        private List<Guid> _selectedContactIds = new List<Guid>();

        /// <summary>
        /// Guard tránh gọi LoadDataAsync song song
        /// </summary>
        private bool _isLoading;

        #endregion

        #region ========== CONSTRUCTOR & PUBLIC METHODS ==========

        /// <summary>
        /// Khởi tạo control, đăng ký event UI
        /// </summary>
        public UcBusinessPartnerContact()
        {
            InitializeComponent();

            // Toolbar events
            ListDataBarButtonItem.ItemClick += ListDataBarButtonItem_ItemClick;
            NewBarButtonItem.ItemClick += NewBarButtonItem_ItemClick;
            EditBarButtonItem.ItemClick += EditBarButtonItem_ItemClick;
            DeleteBarButtonItem.ItemClick += DeleteBarButtonItem_ItemClick;
            ExportBarButtonItem.ItemClick += ExportBarButtonItem_ItemClick;

            // CardView events
            BusinessPartnerContactGridCardView.SelectionChanged += BusinessPartnerContactGridCardView_SelectionChanged;
            BusinessPartnerContactGridCardView.CustomDrawCardCaption += BusinessPartnerContactGridCardView_CustomDrawCardCaption;
            BusinessPartnerContactGridCardView.KeyDown += BusinessPartnerContactGridCardView_KeyDown;
            BusinessPartnerContactGridCardView.CellValueChanged += BusinessPartnerContactGridCardView_CellValueChanged;
            BusinessPartnerContactGridCardView.ValidatingEditor += BusinessPartnerContactGridCardView_ValidatingEditor;
            BusinessPartnerContactGridCardView.ValidateRow += BusinessPartnerContactGridCardView_ValidateRow;


            // Set custom caption format
            BusinessPartnerContactGridCardView.CardCaptionFormat = @"Liên hệ thứ {0}";

            // Cấu hình RepositoryItemPictureEdit cho colAvatar
            ConfigureAvatarPictureEdit();

            UpdateButtonStates();
            UpdateStatusBar();
        }

        /// <summary>
        /// Thực hiện operation async với WaitingForm1 hiển thị
        /// </summary>
        /// <param name="operation">Operation async cần thực hiện</param>
        private async Task ExecuteWithWaitingFormAsync(Func<Task> operation)
        {
            bool splashShown = false;
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

        #region ========== SỰ KIỆN FORM ==========

        #region ========== SỰ KIỆN TOOLBAR ==========

        /// <summary>
        /// Người dùng bấm "Danh sách" để tải dữ liệu
        /// </summary>
        private async void ListDataBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// Người dùng bấm "Mới"
        /// </summary>
        private async void NewBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                using (OverlayManager.ShowScope(this))
                {
                    using (var form = new FrmBusinessPartnerContactDetail(Guid.Empty))
                    {
                        form.StartPosition = FormStartPosition.CenterParent;
                        form.ShowDialog(this);

                        await LoadDataAsyncWithoutSplash();
                        UpdateButtonStates();
            UpdateStatusBar();
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        #endregion

        #region ========== SỰ KIỆN CARDVIEW ==========

        /// <summary>
        /// CardView selection thay đổi -> cập nhật danh sách Id đã chọn và trạng thái nút
        /// </summary>
        private void BusinessPartnerContactGridCardView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                // Lấy danh sách các row được chọn từ CardView
                if (sender is CardView cardView)
                {
                    _selectedContactIds.Clear();

                    // Lấy tất cả các row được chọn
                    var selectedRowHandles = cardView.GetSelectedRows();

                    foreach (var rowHandle in selectedRowHandles)
                    {
                        if (rowHandle < 0) continue;
                        if (cardView.GetRow(rowHandle) is BusinessPartnerContactDto dto)
                        {
                            _selectedContactIds.Add(dto.Id);
                        }
                    }
                }

                UpdateButtonStates();
            UpdateStatusBar();
                UpdateStatusBar();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// Custom draw card caption để hiển thị màu sắc theo trạng thái liên hệ
        /// </summary>
        private void BusinessPartnerContactGridCardView_CustomDrawCardCaption(object sender, CardCaptionCustomDrawEventArgs e)
        {
            try
            {
                // Lấy dữ liệu từ CardView
                if (!(sender is CardView cardView) || e.RowHandle < 0) return;

                var dto = (BusinessPartnerContactDto)cardView.GetRow(e.RowHandle);
                if (dto == null) return;

                // Tùy chỉnh màu sắc theo trạng thái hoạt động
                if (!dto.IsActive)
                {
                    // Liên hệ không còn sử dụng - màu đỏ
                    e.Appearance.BackColor = Color.LightPink;
                    e.Appearance.ForeColor = Color.DarkRed;
                    e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Strikeout);
                }
                else if (dto.IsPrimary)
                {
                    // Liên hệ chính - màu xanh
                    e.Appearance.BackColor = Color.LightCyan;
                    e.Appearance.ForeColor = Color.DarkBlue;
                }
                else
                {
                    // Liên hệ thường - màu vàng
                    e.Appearance.BackColor = Color.LightYellow;
                    e.Appearance.ForeColor = Color.DarkGreen;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// Xử lý phím tắt cho CardView
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
                        // F2: Chỉnh sửa item được chọn
                        if (_selectedContactIds.Count == 1)
                        {
                            EditBarButtonItem_ItemClick(null, null);
                            e.Handled = true;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// Cập nhật trạng thái các nút toolbar dựa trên selection
        /// </summary>
        private void UpdateButtonStates()
        {
            try
            {
                var selectedCount = _selectedContactIds?.Count ?? 0;
                if (EditBarButtonItem != null)
                    EditBarButtonItem.Enabled = selectedCount == 1;
                if (DeleteBarButtonItem != null)
                    DeleteBarButtonItem.Enabled = selectedCount >= 1;
                var rowCount = BusinessPartnerContactGridCardView.RowCount;
                if (ExportBarButtonItem != null)
                    ExportBarButtonItem.Enabled = rowCount > 0;
            }
            catch
            {
                // ignore
            }
        }

        /// <summary>
        /// Cập nhật status bar với thông tin selection và data summary
        /// </summary>
        private void UpdateStatusBar()
        {
            try
            {
                UpdateSelectedRowStatus();
                UpdateDataSummaryStatus();
            }
            catch
            {
                // ignore
            }
        }

        /// <summary>
        /// Cập nhật thông tin số dòng đang được chọn
        /// </summary>
        private void UpdateSelectedRowStatus()
        {
            try
            {
                if (CurrentSelectBarStaticItem == null) return;

                var selectedCount = _selectedContactIds?.Count ?? 0;
                if (selectedCount == 0)
                {
                    CurrentSelectBarStaticItem.Caption = @"Chưa chọn dòng nào";
                }
                else if (selectedCount == 1)
                {
                    CurrentSelectBarStaticItem.Caption = @"Đang chọn 1 dòng";
                }
                else
                {
                    CurrentSelectBarStaticItem.Caption = $@"Đang chọn {selectedCount} dòng";
                }
            }
            catch
            {
                // ignore
            }
        }

        /// <summary>
        /// Cập nhật thông tin tổng kết dữ liệu với HTML formatting
        /// </summary>
        private void UpdateDataSummaryStatus()
        {
            try
            {
                if (DataSummaryBarStaticItem == null) return;

                var currentData = businessPartnerContactDtoBindingSource.DataSource as List<BusinessPartnerContactDto>;
                if (currentData == null || !currentData.Any())
                {
                    DataSummaryBarStaticItem.Caption = @"Chưa có dữ liệu";
                    return;
                }

                var currentPageCount = currentData.Count;
                var primaryCount = currentData.Count(x => x.IsPrimary);
                var activeCount = currentData.Count(x => x.IsActive);
                var inactiveCount = currentData.Count(x => !x.IsActive);

                // Tạo HTML content với màu sắc
                var summary = $"<b>Hiển thị: {currentPageCount}</b> | " +
                              $"<color=blue>Liên hệ chính: {primaryCount}</color> | " +
                              $"<color=green>Hoạt động: {activeCount}</color> | " +
                              $"<color=red>Không hoạt động: {inactiveCount}</color>";

                DataSummaryBarStaticItem.Caption = summary;
            }
            catch
            {
                // ignore
            }
        }

        /// <summary>
        /// Cấu hình CardView cho hiển thị tối ưu
        /// </summary>
        private void ConfigureCardView()
        {
            try
            {
                // CardView không cần cấu hình đặc biệt như GridView
                // Các cấu hình đã được thiết lập trong Designer
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        #endregion

        #endregion

        #region ========== SỰ KIỆN CRUD ==========

        /// <summary>
        /// Người dùng bấm "Điều chỉnh"
        /// </summary>
        private async void EditBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                // Chỉ cho phép chỉnh sửa 1 dòng dữ liệu
                if (_selectedContactIds == null || _selectedContactIds.Count == 0)
                {
                    MsgBox.ShowInfo("Vui lòng chọn một dòng để chỉnh sửa.");
                    return;
                }

                if (_selectedContactIds.Count > 1)
                {
                    MsgBox.ShowInfo("Chỉ cho phép chỉnh sửa 1 dòng. Vui lòng bỏ chọn bớt.");
                    return;
                }

                var id = _selectedContactIds[0];
                var selectedDtos = GetSelectedContactDtos();
                var dto = selectedDtos.FirstOrDefault(x => x.Id == id);

                if (dto == null)
                {
                    // Fallback: tìm trong datasource
                    if (businessPartnerContactDtoBindingSource.DataSource is IEnumerable<BusinessPartnerContactDto> list)
                    {
                        dto = list.FirstOrDefault(x => x.Id == id);
                    }
                }

                if (dto == null)
                {
                    MsgBox.ShowInfo("Không thể xác định dòng được chọn để chỉnh sửa.");
                    return;
                }

                using (OverlayManager.ShowScope(this))
                {
                    using (var form = new FrmBusinessPartnerContactDetail(dto.Id))
                    {
                        form.StartPosition = FormStartPosition.CenterParent;
                        form.ShowDialog(this);

                        await LoadDataAsyncWithoutSplash();
                        UpdateButtonStates();
            UpdateStatusBar();
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// Người dùng bấm "Xóa"
        /// </summary>
        private async void DeleteBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (_selectedContactIds == null || _selectedContactIds.Count == 0)
                {
                    MsgBox.ShowInfo("Vui lòng chọn ít nhất một dòng để xóa.");
                    return;
                }

                var confirmMessage = _selectedContactIds.Count == 1
                    ? "Bạn có chắc muốn xóa dòng dữ liệu đã chọn?"
                    : $"Bạn có chắc muốn xóa {_selectedContactIds.Count} dòng dữ liệu đã chọn?";

                if (!MsgBox.GetConfirmFromYesNoDialog(confirmMessage)) return;

                await ExecuteWithWaitingFormAsync(async () =>
                {
                    foreach (var id in _selectedContactIds.ToList())
                    {
                        _contactBll.Delete(id);
                    }

                    ClearSelectionState();
                    await LoadDataAsyncWithoutSplash();
                });
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(new Exception("Lỗi xóa dữ liệu: " + ex.Message, ex));
            }
        }

        /// <summary>
        /// Người dùng bấm "Xuất"
        /// </summary>
        private void ExportBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                var rowCount = BusinessPartnerContactGridCardView.RowCount;
                if (rowCount <= 0)
                {
                    MsgBox.ShowInfo("Không có dữ liệu để xuất.");
                    return;
                }

                // Tạo đường dẫn file với timestamp
                var fileName = $"BusinessPartnerContacts_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName);

                // Xuất dữ liệu ra file Excel
                BusinessPartnerContactGridControl.ExportToXlsx(path);

                // Mở file Excel với ứng dụng mặc định
                System.Diagnostics.Process.Start(path);

                MsgBox.ShowInfo($"Đã xuất dữ liệu thành công!\nFile: {fileName}\nVị trí: Desktop");
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        #endregion

        #region ========== QUẢN LÝ DỮ LIỆU ==========

        /// <summary>
        /// Tải dữ liệu và bind vào Grid (Async, hiển thị WaitForm)
        /// </summary>
        private async Task LoadDataAsync()
        {
            if (_isLoading) return; // tránh re-entrancy
            _isLoading = true;
            try
            {
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

        /// <summary>
        /// Tải dữ liệu và bind vào Grid (Async, không hiển thị WaitForm)
        /// </summary>
        private Task LoadDataAsyncWithoutSplash()
        {
            try
            {
                var entities = _contactBll.GetAll();
                // BusinessPartnerSite đã được include trong DAL, không cần truyền siteName
                var dtoList = entities.Select(e => e.ToDto()).ToList();

                BindGrid(dtoList);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(new Exception("Lỗi tải dữ liệu: " + ex.Message, ex));
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Bind danh sách DTO vào Grid và cấu hình hiển thị
        /// </summary>
        private void BindGrid(List<BusinessPartnerContactDto> data)
        {
            businessPartnerContactDtoBindingSource.DataSource = data;
            // CardView doesn't have BestFitColumns method
            ConfigureCardView();
            UpdateButtonStates();
            UpdateStatusBar();
        }

        /// <summary>
        /// Xóa trạng thái chọn hiện tại trên Grid
        /// </summary>
        private void ClearSelectionState()
        {
            _selectedContactIds.Clear();
            BusinessPartnerContactGridCardView.ClearSelection();
            BusinessPartnerContactGridCardView.FocusedRowHandle = GridControl.InvalidRowHandle;
            UpdateButtonStates();
            UpdateStatusBar();
        }

        /// <summary>
        /// Lấy danh sách các DTO được chọn
        /// </summary>
        /// <returns>Danh sách BusinessPartnerContactDto được chọn</returns>
        private List<BusinessPartnerContactDto> GetSelectedContactDtos()
        {
            var selectedDtos = new List<BusinessPartnerContactDto>();

            try
            {
                var selectedRowHandles = BusinessPartnerContactGridCardView.GetSelectedRows();

                selectedDtos.AddRange((from rowHandle in selectedRowHandles
                    where rowHandle >= 0
                    select BusinessPartnerContactGridCardView.GetRow(rowHandle)).OfType<BusinessPartnerContactDto>());
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }

            return selectedDtos;
        }

        /// <summary>
        /// Chọn tất cả các card trong CardView
        /// </summary>
        private void SelectAllCards()
        {
            try
            {
                BusinessPartnerContactGridCardView.SelectAll();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// Bỏ chọn tất cả các card
        /// </summary>
        private void DeselectAllCards()
        {
            try
            {
                BusinessPartnerContactGridCardView.ClearSelection();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        #endregion

        #region ========== XỬ LÝ HÌNH ẢNH ==========

        /// <summary>
        /// Cấu hình RepositoryItemPictureEdit cho colAvatar trong CardView
        /// Sử dụng PictureChanged event để xử lý thay đổi ảnh
        /// </summary>
        private void ConfigureAvatarPictureEdit()
        {
            try
            {
                // Lấy RepositoryItemPictureEdit từ colAvatar
                if (colAvatar?.ColumnEdit is DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit pictureEdit)
                {
                    // Giữ nguyên context menu mặc định (không thay đổi)
                    // pictureEdit.ShowMenu = true; // Mặc định
                    
                    // Cấu hình kích thước
                    pictureEdit.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom;
                    pictureEdit.ZoomPercent = 100;
                    
                    
                    
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }



        /// <summary>
        /// Kiểm tra định dạng hình ảnh có hợp lệ không
        /// </summary>
        /// <param name="imageBytes">Byte array của hình ảnh</param>
        /// <returns>True nếu định dạng hợp lệ</returns>
        private bool IsValidImageFormat(byte[] imageBytes)
        {
            try
            {
                if (imageBytes == null || imageBytes.Length < 4) return false;

                // Kiểm tra magic bytes của các format phổ biến
                // JPEG: FF D8 FF
                if (imageBytes[0] == 0xFF && imageBytes[1] == 0xD8 && imageBytes[2] == 0xFF)
                    return true;

                // PNG: 89 50 4E 47
                if (imageBytes[0] == 0x89 && imageBytes[1] == 0x50 && imageBytes[2] == 0x4E && imageBytes[3] == 0x47)
                    return true;

                // GIF: 47 49 46 38
                if (imageBytes[0] == 0x47 && imageBytes[1] == 0x49 && imageBytes[2] == 0x46 && imageBytes[3] == 0x38)
                    return true;

                // BMP: 42 4D
                if (imageBytes[0] == 0x42 && imageBytes[1] == 0x4D)
                    return true;

                return false;
            }
            catch
            {
                return false;
            }
        }


        #endregion


        /// <summary>
        /// Convert Image to byte array
        /// </summary>
        private byte[] ImageToByteArray(Image image)
        {
            using var ms = new MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms.ToArray();
        }

        /// <summary>
        /// Xử lý sự kiện CellValueChanged của CardView
        /// Được kích hoạt khi giá trị cell thay đổi
        /// </summary>
        private void BusinessPartnerContactGridCardView_CellValueChanged(
            object sender,
            DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                if (e.Column.FieldName == "Avatar")
                {
                    if (BusinessPartnerContactGridCardView.GetRow(e.RowHandle) is BusinessPartnerContactDto dto && dto.Avatar != null)
                    {
                        // Lưu avatar xuống DB
                        _contactBll.UpdateAvatarOnly(dto.Id, dto.Avatar);
                        MsgBox.ShowInfo("Đã cập nhật avatar thành công!");
                    }
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
                            _contactBll.UpdateEntityWithoutAvatar(entity);
                            
                            // Refresh data để đảm bảo đồng bộ
                            await LoadDataAsync();
                            UpdateButtonStates();
                            UpdateStatusBar();
                        });
                    }

                    //Hiển thị cập nhật thành công
                    MsgBox.ShowInfo("Đã cập nhật thông tin thành công!");
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(new Exception("Lỗi khi cập nhật dữ liệu: " + ex.Message, ex));
            }
        }
         

        private void ContactAvatarPictureEdit_ImageChanged(object sender, EventArgs e)
        {
            if (_isLoading) return;

            var pictureEdit = sender as PictureEdit;
            if (pictureEdit == null) return;

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
                            MsgBox.ShowWarning("Định dạng hình ảnh không được hỗ trợ! Vui lòng chọn file JPG, PNG hoặc GIF.");
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
                MsgBox.ShowInfo("Đã cập nhật avatar thành công!");
            });
        }

        /// <summary>
        /// Xử lý sự kiện ValidatingEditor của CardView
        /// Validate dữ liệu trước khi commit
        /// </summary>
        private void BusinessPartnerContactGridCardView_ValidatingEditor(object sender,
            DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            try
            {
                // Lấy thông tin column và row
                var view = sender as CardView;
                if (view == null) return;

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
                        var emailRegex = new System.Text.RegularExpressions.Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
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
                        var phoneRegex = new System.Text.RegularExpressions.Regex(@"^[\d\s\+\-\(\)]+$");
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
            DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
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
                    var emailRegex = new System.Text.RegularExpressions.Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
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
                    var phoneRegex = new System.Text.RegularExpressions.Regex(@"^[\d\s\+\-\(\)]+$");
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

    }
}
