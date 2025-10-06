using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bll.Common;
using Bll.MasterData.Customer;
using Bll.Utils;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Card;
using DevExpress.XtraSplashScreen;
using MasterData.Customer.Converters;
using MasterData.Customer.Dto;

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

            // Set custom caption format
            BusinessPartnerContactGridCardView.CardCaptionFormat = @"Liên hệ thứ {0}";

            UpdateButtonStates();
        }

        /// <summary>
        /// Thực hiện operation async với WaitingForm1 hiển thị
        /// </summary>
        /// <param name="operation">Operation async cần thực hiện</param>
        private async Task ExecuteWithWaitingFormAsync(Func<Task> operation)
        {
            try
            {
                // Hiển thị WaitingForm1
                SplashScreenManager.ShowForm(typeof(WaitForm1));

                // Thực hiện operation
                await operation();
            }
            finally
            {
                // Đóng WaitingForm1
                SplashScreenManager.CloseForm();
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
                var partnerBll = new BusinessPartnerBll();
                var partners = partnerBll.GetAll();
                var partnerNameById = partners.ToDictionary(p => p.Id, p => p.PartnerName);
                var dtoList = entities.Select(e => e.ToDto(partnerNameById.TryGetValue(e.SiteId, out var value) ? value : null)).ToList();

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

        
    }
}
