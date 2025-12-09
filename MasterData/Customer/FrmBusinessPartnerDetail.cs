using Bll.MasterData.CustomerBll;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraSplashScreen;
using DTO.MasterData.CustomerPartner;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common.Common;
using Common.Utils;
using Dal.DataContext;

namespace MasterData.Customer
{
    /// <summary>
    /// Form chi tiết đối tác - thêm mới và chỉnh sửa.
    /// Cung cấp giao diện nhập liệu cho thông tin đối tác với validation, upload logo và xử lý lỗi.
    /// </summary>
    public partial class FrmBusinessPartnerDetail : XtraForm
    {
        #region Fields

        /// <summary>
        /// Business Logic Layer cho đối tác
        /// </summary>
        private readonly BusinessPartnerBll _bll = new BusinessPartnerBll();

        /// <summary>
        /// ID của đối tác đang chỉnh sửa (Guid.Empty nếu thêm mới)
        /// </summary>
        private readonly Guid _businessPartnerId;

        /// <summary>
        /// Trạng thái chỉnh sửa (true nếu đang chỉnh sửa, false nếu thêm mới)
        /// </summary>
        private bool IsEditMode => _businessPartnerId != Guid.Empty;

        /// <summary>
        /// Flag đánh dấu đang load logo (để tránh trigger event ImageChanged khi load)
        /// </summary>
        private bool _isLoadingLogo;

        /// <summary>
        /// Kích thước tối đa cho hình ảnh logo (10MB)
        /// </summary>
        private const int MaxLogoSizeInBytes = 10 * 1024 * 1024;

        /// <summary>
        /// Kích thước tối đa cho thumbnail logo trong form detail (300px)
        /// </summary>
        private const int ThumbnailMaxDimension = 300;

        #endregion

        #region Constructor

        /// <summary>
        /// Khởi tạo form chi tiết đối tác
        /// </summary>
        /// <param name="businessPartnerId">ID đối tác (Guid.Empty nếu thêm mới)</param>
        public FrmBusinessPartnerDetail(Guid businessPartnerId)
        {
            _businessPartnerId = businessPartnerId;

            InitializeComponent();
            Shown += FrmBusinessPartnerDetail_Shown;
        }

        #endregion

        #region Form Events

        /// <summary>
        /// Xử lý sự kiện khi form được hiển thị: setup UI, load dữ liệu nếu đang chỉnh sửa
        /// </summary>
        /// <param name="sender">Nguồn sự kiện</param>
        /// <param name="e">Thông tin sự kiện</param>
        private void FrmBusinessPartnerDetail_Shown(object sender, EventArgs e)
        {
            try
            {
                // Đánh dấu các trường bắt buộc dựa trên DataAnnotations của DTO
                RequiredFieldHelper.MarkRequiredFields(this, typeof(BusinessPartnerListDto));

                // Thiết lập SuperToolTip cho các controls để cải thiện UX
                SetupSuperToolTips();

                // Cấu hình combobox Loại đối tác
                SetupPartnerTypeComboBox();

                // Nếu đang chỉnh sửa thì nạp dữ liệu chi tiết
                if (IsEditMode)
                {
                    _ = LoadDetailAsync(_businessPartnerId);
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Khởi tạo form");
            }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Xử lý sự kiện click button Lưu: Validate → Build DTO → Lưu qua BLL → Thông báo kết quả
        /// </summary>
        /// <param name="sender">Nguồn sự kiện</param>
        /// <param name="e">Thông tin sự kiện</param>
        private async void SaveBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                // Validate dữ liệu đầu vào
                if (!ValidateInput())
                {
                    return;
                }

                // Lưu dữ liệu với waiting form
                await ExecuteWithWaitingFormAsync(async () =>
                {
                    await SaveBusinessPartnerAsync();
                });

                // Thông báo thành công và đóng form
                MsgBox.ShowSuccess("Lưu dữ liệu đối tác thành công");
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lưu dữ liệu đối tác");
            }
        }

        /// <summary>
        /// Xử lý sự kiện click button Đóng: Đóng form mà không lưu thay đổi
        /// </summary>
        /// <param name="sender">Nguồn sự kiện</param>
        /// <param name="e">Thông tin sự kiện</param>
        private void CloseBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Xử lý sự kiện ImageChanged của LogoThumbnailDataPictureEdit: Upload logo đối tác
        /// </summary>
        /// <param name="sender">Nguồn sự kiện (PictureEdit)</param>
        /// <param name="e">Thông tin sự kiện</param>
        private async void LogoThumbnailDataPictureEdit_ImageChanged(object sender, EventArgs e)
        {
            try
            {
                // Bỏ qua nếu đang load logo (tránh trigger khi load từ database)
                if (_isLoadingLogo)
                {
                    return;
                }

                // Chỉ xử lý khi sender là PictureEdit
                if (sender is not PictureEdit pictureEdit)
                {
                    return;
                }

                // Chỉ upload khi đã có PartnerId (đang chỉnh sửa)
                // Nếu chưa có PartnerId (thêm mới), logo sẽ được upload sau khi lưu thành công
                if (!IsEditMode)
                {
                    return;
                }

                // Xử lý upload logo với waiting form
                await ExecuteWithWaitingFormAsync(async () =>
                {
                    await HandleLogoUploadAsync(pictureEdit);
                });
            }
            catch (Exception ex)
            {
                ShowError(ex, "Cập nhật logo đối tác");
                // Reload logo về trạng thái cũ khi có lỗi
                await ReloadLogoAsync();
            }
        }

        #endregion

        #region Private Methods

        #region UI Setup

        /// <summary>
        /// Thiết lập SuperToolTip cho tất cả các controls trong form để cải thiện UX
        /// </summary>
        private void SetupSuperToolTips()
        {
            try
            {
                // Thông tin cơ bản
                SetupSuperToolTip(PartnerCodeTextEdit, "🔖 Mã đối tác",
                    "Nhập mã đối tác duy nhất trong hệ thống. Trường này là bắt buộc.");
                SetupSuperToolTip(PartnerNameTextEdit, "🏢 Tên đối tác",
                    "Nhập tên đầy đủ của đối tác. Trường này là bắt buộc.");
                SetupSuperToolTip(PartnerTypeNameComboBoxEdit, "📂 Loại đối tác",
                    "Chọn loại đối tác: Khách hàng, Nhà cung cấp, hoặc cả hai.");

                // Thông tin liên hệ
                SetupSuperToolTip(TaxCodeTextEdit, "📋 Mã số thuế",
                    "Nhập mã số thuế của đối tác.");
                SetupSuperToolTip(PhoneTextEdit, "📞 Số điện thoại",
                    "Nhập số điện thoại liên hệ của đối tác.");
                SetupSuperToolTip(EmailTextEdit, "📧 Email",
                    "Nhập địa chỉ email liên hệ của đối tác.");
                SetupSuperToolTip(WebsiteTextEdit, "🌐 Website",
                    "Nhập địa chỉ website của đối tác (ví dụ: https://example.com).");

                // Thông tin địa chỉ
                SetupSuperToolTip(AddressTextEdit, "📍 Địa chỉ",
                    "Nhập địa chỉ đầy đủ của đối tác.");
                SetupSuperToolTip(CityTextEdit, "🏙️ Thành phố",
                    "Nhập tên thành phố của đối tác.");
                SetupSuperToolTip(CountryTextEdit, "🌍 Quốc gia",
                    "Nhập tên quốc gia của đối tác.");

                // Trạng thái và Logo
                SetupSuperToolTip(IsActiveToggleSwitch, "⚡ Trạng thái",
                    "Bật/tắt trạng thái hoạt động của đối tác. Đối tác không hoạt động sẽ không được sử dụng trong các giao dịch.");
                SetupSuperToolTip(LogoThumbnailDataPictureEdit, "🖼️ Logo đối tác",
                    "Click để chọn hoặc thay đổi logo đối tác. Hỗ trợ định dạng JPG, PNG, GIF. Kích thước tối đa 10MB. Logo gốc sẽ được lưu trên NAS và thumbnail sẽ được lưu trong database.");

                // Các nút hành động
                SetupBarButtonSuperToolTip(SaveBarButtonItem, "💾 Lưu",
                    "Lưu thông tin đối tác vào hệ thống.");
                SetupBarButtonSuperToolTip(CloseBarButtonItem, "❌ Đóng",
                    "Đóng form mà không lưu thay đổi.");
            }
            catch (Exception ex)
            {
                // Log lỗi nhưng không chặn form
                System.Diagnostics.Debug.WriteLine($"Lỗi setup SuperToolTip: {ex.Message}");
            }
        }

        /// <summary>
        /// Thiết lập SuperToolTip cho TextEdit
        /// </summary>
        /// <param name="textEdit">Control TextEdit cần thiết lập</param>
        /// <param name="title">Tiêu đề tooltip</param>
        /// <param name="content">Nội dung tooltip</param>
        private void SetupSuperToolTip(TextEdit textEdit, string title, string content)
        {
            if (textEdit != null)
            {
                SuperToolTipHelper.SetTextEditSuperTip(textEdit,
                    title: $"<b><color=DarkBlue>{title}</color></b>",
                    content: content);
            }
        }

        /// <summary>
        /// Thiết lập SuperToolTip cho BaseEdit (ComboBox, ToggleSwitch, PictureEdit)
        /// </summary>
        /// <param name="baseEdit">Control BaseEdit cần thiết lập</param>
        /// <param name="title">Tiêu đề tooltip</param>
        /// <param name="content">Nội dung tooltip</param>
        private void SetupSuperToolTip(BaseEdit baseEdit, string title, string content)
        {
            if (baseEdit != null)
            {
                SuperToolTipHelper.SetBaseEditSuperTip(baseEdit,
                    title: $"<b><color=DarkBlue>{title}</color></b>",
                    content: content);
            }
        }

        /// <summary>
        /// Thiết lập SuperToolTip cho BarButtonItem
        /// </summary>
        /// <param name="buttonItem">BarButtonItem cần thiết lập</param>
        /// <param name="title">Tiêu đề tooltip</param>
        /// <param name="content">Nội dung tooltip</param>
        private void SetupBarButtonSuperToolTip(BarButtonItem buttonItem, string title, string content)
        {
            if (buttonItem != null)
            {
                var color = title.Contains("Lưu") ? "Blue" : "Red";
                SuperToolTipHelper.SetBarButtonSuperTip(buttonItem,
                    title: $"<b><color={color}>{title}</color></b>",
                    content: content);
            }
        }

        /// <summary>
        /// Cấu hình dữ liệu cho ComboBoxEdit 'PartnerTypeNameComboBoxEdit' với 3 lựa chọn chuẩn
        /// </summary>
        private void SetupPartnerTypeComboBox()
        {
            if (PartnerTypeNameComboBoxEdit == null)
            {
                return;
            }

            // Xóa các item cũ và thêm mới
            PartnerTypeNameComboBoxEdit.Properties.Items.Clear();
            PartnerTypeNameComboBoxEdit.Properties.Items.AddRange(new[]
            {
                "Khách hàng",
                "Nhà cung cấp",
                "Khách hàng & Nhà cung cấp"
            });

            // Vô hiệu hóa text editor để chỉ cho phép chọn từ danh sách
            PartnerTypeNameComboBoxEdit.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
        }

        #endregion

        #region Data Loading

        /// <summary>
        /// Nạp dữ liệu chi tiết đối tác theo Id vào các control (asynchronous)
        /// </summary>
        /// <param name="id">ID đối tác cần load</param>
        private async Task LoadDetailAsync(Guid id)
        {
            try
            {
                // Gọi BLL để lấy entity từ database
                var entity = await _bll.GetByIdAsync(id);
                if (entity == null)
                {
                    return;
                }

                // Map dữ liệu vào các controls
                LoadBasicInformation(entity);
                LoadPartnerType(entity);
                LoadLogo(entity);
            }
            catch (Exception ex)
            {
                ShowError(ex, "Tải dữ liệu chi tiết đối tác");
            }
        }

        /// <summary>
        /// Load thông tin cơ bản vào các controls
        /// </summary>
        /// <param name="entity">Entity đối tác chứa dữ liệu</param>
        private void LoadBasicInformation(BusinessPartner entity)
        {
            if (PartnerCodeTextEdit != null) PartnerCodeTextEdit.EditValue = entity.PartnerCode;
            if (PartnerNameTextEdit != null) PartnerNameTextEdit.EditValue = entity.PartnerName;
            if (TaxCodeTextEdit != null) TaxCodeTextEdit.EditValue = entity.TaxCode;
            if (PhoneTextEdit != null) PhoneTextEdit.EditValue = entity.Phone;
            if (EmailTextEdit != null) EmailTextEdit.EditValue = entity.Email;
            if (WebsiteTextEdit != null) WebsiteTextEdit.EditValue = entity.Website;
            if (AddressTextEdit != null) AddressTextEdit.EditValue = entity.Address;
            if (CityTextEdit != null) CityTextEdit.EditValue = entity.City;
            if (CountryTextEdit != null) CountryTextEdit.EditValue = entity.Country;
            if (IsActiveToggleSwitch != null) IsActiveToggleSwitch.EditValue = entity.IsActive;
        }

        /// <summary>
        /// Load loại đối tác vào ComboBox
        /// </summary>
        /// <param name="entity">Entity đối tác chứa dữ liệu</param>
        private void LoadPartnerType(BusinessPartner entity)
        {
            if (PartnerTypeNameComboBoxEdit == null)
            {
                return;
            }

            // Map PartnerType (1, 2, 3) sang SelectedIndex (0, 1, 2)
            switch (entity.PartnerType)
            {
                case 1: PartnerTypeNameComboBoxEdit.SelectedIndex = 0; break; // Khách hàng
                case 2: PartnerTypeNameComboBoxEdit.SelectedIndex = 1; break; // Nhà cung cấp
                case 3: PartnerTypeNameComboBoxEdit.SelectedIndex = 2; break; // Khách hàng & Nhà cung cấp
                default: PartnerTypeNameComboBoxEdit.SelectedIndex = -1; break;
            }
        }

        /// <summary>
        /// Load logo thumbnail vào PictureEdit
        /// </summary>
        /// <param name="entity">Entity đối tác chứa dữ liệu</param>
        private void LoadLogo(BusinessPartner entity)
        {
            if (LogoThumbnailDataPictureEdit == null || entity.LogoThumbnailData == null)
            {
                return;
            }

            try
            {
                // Đánh dấu đang load logo để tránh trigger event ImageChanged
                _isLoadingLogo = true;

                // Chuyển đổi Binary sang byte array
                var thumbnailBytes = entity.LogoThumbnailData.ToArray();
                if (thumbnailBytes != null && thumbnailBytes.Length > 0)
                {
                    // Load ảnh từ byte array và clone để tránh lỗi GDI+ khi stream bị dispose
                    Image loadedImage = null;
                    try
                    {
                        using (var ms = new MemoryStream(thumbnailBytes))
                        {
                            loadedImage = Image.FromStream(ms);
                        }

                        // Clone Image để tạo bản copy độc lập, tránh lỗi khi stream đã bị dispose
                        var clonedImage = CloneImage(loadedImage);
                        
                        // Dispose Image gốc nếu cần
                        loadedImage?.Dispose();

                        // Set cloned Image vào PictureEdit
                        LogoThumbnailDataPictureEdit.Image = clonedImage;
                    }
                    catch
                    {
                        // Dispose Image nếu có lỗi
                        loadedImage?.Dispose();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log lỗi nhưng không chặn form
                System.Diagnostics.Debug.WriteLine($"Lỗi load logo thumbnail: {ex.Message}");
            }
            finally
            {
                // Reset flag sau khi load xong
                _isLoadingLogo = false;
            }
        }

        /// <summary>
        /// Reload logo từ database (sử dụng khi upload thất bại để rollback)
        /// </summary>
        private async Task ReloadLogoAsync()
        {
            try
            {
                if (!IsEditMode || LogoThumbnailDataPictureEdit == null)
                {
                    return;
                }

                // Đánh dấu đang load logo để tránh trigger event ImageChanged
                _isLoadingLogo = true;

                // Lấy lại entity từ database
                var entity = await _bll.GetByIdAsync(_businessPartnerId);
                if (entity?.LogoThumbnailData != null)
                {
                    var thumbnailBytes = entity.LogoThumbnailData.ToArray();
                    if (thumbnailBytes != null && thumbnailBytes.Length > 0)
                    {
                        // Load ảnh từ byte array và clone để tránh lỗi GDI+ khi stream bị dispose
                        Image loadedImage = null;
                        try
                        {
                            using (var ms = new MemoryStream(thumbnailBytes))
                            {
                                loadedImage = Image.FromStream(ms);
                            }

                            // Clone Image để tạo bản copy độc lập, tránh lỗi khi stream đã bị dispose
                            var clonedImage = CloneImage(loadedImage);
                            
                            // Dispose Image gốc nếu cần
                            loadedImage?.Dispose();

                            // Set cloned Image vào PictureEdit
                            LogoThumbnailDataPictureEdit.Image = clonedImage;
                        }
                        catch
                        {
                            // Dispose Image nếu có lỗi
                            loadedImage?.Dispose();
                            throw;
                        }
                    }
                }
                else
                {
                    // Xóa ảnh nếu không có logo
                    LogoThumbnailDataPictureEdit.Image = null;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi reload logo: {ex.Message}");
            }
            finally
            {
                // Reset flag sau khi reload xong
                _isLoadingLogo = false;
            }
        }

        #endregion

        #region Data Saving

        /// <summary>
        /// Lưu dữ liệu đối tác và upload logo nếu có
        /// </summary>
        private async Task SaveBusinessPartnerAsync()
        {
            // Bước 1: Thu thập dữ liệu từ form và build DTO
            var detailDto = BuildDetailDtoFromForm();

            // Bước 2: Convert DTO -> Entity
            var existing = IsEditMode ? _bll.GetById(detailDto.Id) : null;
            var entity = detailDto.ToEntity(existing);

            // Bước 3: Lưu entity qua BLL (Repository sẽ tự động set Id nếu là tạo mới)
            _bll.SaveOrUpdate(entity);

            // Bước 4: Upload logo nếu có (khi tạo mới, logo sẽ được upload sau khi có Id)
            if (LogoThumbnailDataPictureEdit?.Image != null && entity.Id != Guid.Empty)
            {
                await UploadLogoIfValidAsync(entity.Id);
            }
        }

        /// <summary>
        /// Thu thập dữ liệu từ Form thành BusinessPartnerDetailDto (đủ trường để lưu)
        /// </summary>
        /// <returns>DTO chứa dữ liệu từ form</returns>
        private BusinessPartnerDetailDto BuildDetailDtoFromForm()
        {
            return new BusinessPartnerDetailDto
            {
                Id = _businessPartnerId,
                PartnerCode = PartnerCodeTextEdit?.EditValue?.ToString(),
                PartnerName = PartnerNameTextEdit?.EditValue?.ToString(),
                PartnerType = GetPartnerTypeFromComboBox(),
                TaxCode = TaxCodeTextEdit?.EditValue?.ToString(),
                Phone = PhoneTextEdit?.EditValue?.ToString(),
                Email = EmailTextEdit?.EditValue?.ToString(),
                Website = WebsiteTextEdit?.EditValue?.ToString(),
                Address = AddressTextEdit?.EditValue?.ToString(),
                City = CityTextEdit?.EditValue?.ToString(),
                Country = CountryTextEdit?.EditValue?.ToString(),
                IsActive = (IsActiveToggleSwitch?.EditValue as bool?) ?? true,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            };
        }

        /// <summary>
        /// Lấy giá trị PartnerType từ ComboBox selection
        /// </summary>
        /// <returns>Giá trị PartnerType (1: Khách hàng, 2: Nhà cung cấp, 3: Cả hai, 0: Chưa chọn)</returns>
        private int GetPartnerTypeFromComboBox()
        {
            if (PartnerTypeNameComboBoxEdit == null)
            {
                return 0;
            }

            // Map SelectedIndex (0, 1, 2) sang PartnerType (1, 2, 3)
            return PartnerTypeNameComboBoxEdit.SelectedIndex switch
            {
                0 => 1, // Khách hàng
                1 => 2, // Nhà cung cấp
                2 => 3, // Khách hàng & Nhà cung cấp
                _ => 0  // Chưa chọn
            };
        }

        /// <summary>
        /// Upload logo nếu hợp lệ (kiểm tra kích thước và format)
        /// </summary>
        /// <param name="partnerId">ID đối tác</param>
        private async Task UploadLogoIfValidAsync(Guid partnerId)
        {
            // Chuyển đổi Image sang byte array
            var imageBytes = ImageToByteArray(LogoThumbnailDataPictureEdit.Image);
            if (imageBytes == null || imageBytes.Length == 0)
            {
                return;
            }

            // Kiểm tra kích thước (tối đa 10MB)
            if (imageBytes.Length > MaxLogoSizeInBytes)
            {
                MsgBox.ShowWarning($"Hình ảnh quá lớn! Vui lòng chọn hình ảnh nhỏ hơn {MaxLogoSizeInBytes / (1024 * 1024)}MB.");
                return;
            }

            // Kiểm tra format (JPG, PNG, GIF)
            if (!IsValidImageFormat(imageBytes))
            {
                MsgBox.ShowWarning("Định dạng hình ảnh không được hỗ trợ! Vui lòng chọn file JPG, PNG hoặc GIF.");
                return;
            }

            // Upload logo qua BLL (lưu file gốc trên NAS và thumbnail trong database)
            await _bll.UploadLogoFromBytesAsync(partnerId, imageBytes, ThumbnailMaxDimension);
        }

        /// <summary>
        /// Xử lý upload logo từ PictureEdit
        /// </summary>
        /// <param name="pictureEdit">PictureEdit chứa hình ảnh</param>
        private async Task HandleLogoUploadAsync(PictureEdit pictureEdit)
        {
            if (pictureEdit.Image == null)
            {
                return;
            }

            // Chuyển đổi Image sang byte array
            var imageBytes = ImageToByteArray(pictureEdit.Image);
            if (imageBytes == null || imageBytes.Length == 0)
            {
                return;
            }

            // Kiểm tra kích thước
            if (imageBytes.Length > MaxLogoSizeInBytes)
            {
                MsgBox.ShowWarning($"Hình ảnh quá lớn! Vui lòng chọn hình ảnh nhỏ hơn {MaxLogoSizeInBytes / (1024 * 1024)}MB.");
                await ReloadLogoAsync();
                return;
            }

            // Kiểm tra format
            if (!IsValidImageFormat(imageBytes))
            {
                MsgBox.ShowWarning("Định dạng hình ảnh không được hỗ trợ! Vui lòng chọn file JPG, PNG hoặc GIF.");
                await ReloadLogoAsync();
                return;
            }

            // Upload logo qua BLL
            await _bll.UploadLogoFromBytesAsync(_businessPartnerId, imageBytes, ThumbnailMaxDimension);

            // Thông báo thành công
            MsgBox.ShowSuccess("Đã cập nhật logo đối tác thành công!");

            // Reload logo để hiển thị thumbnail mới
            await ReloadLogoAsync();
        }

        #endregion

        #region Validation

        /// <summary>
        /// Validate input theo thứ tự, đặt lỗi và focus control không hợp lệ đầu tiên
        /// </summary>
        /// <returns>True nếu hợp lệ, False nếu không hợp lệ</returns>
        private bool ValidateInput()
        {
            // Xóa tất cả lỗi cũ
            dxErrorProvider1.ClearErrors();

            // Validate mã đối tác (bắt buộc và không trùng lặp)
            if (!ValidatePartnerCode())
            {
                return false;
            }

            // Validate tên đối tác (bắt buộc)
            if (!ValidatePartnerName())
            {
                return false;
            }

            // Validate loại đối tác (khuyến nghị bắt buộc chọn)
            if (!ValidatePartnerType())
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validate mã đối tác (bắt buộc và không trùng lặp)
        /// </summary>
        /// <returns>True nếu hợp lệ, False nếu không hợp lệ</returns>
        private bool ValidatePartnerCode()
        {
            // Kiểm tra không được để trống
            if (string.IsNullOrWhiteSpace(PartnerCodeTextEdit?.Text))
            {
                dxErrorProvider1.SetError(PartnerCodeTextEdit, "Mã đối tác không được để trống", ErrorType.Critical);
                PartnerCodeTextEdit?.Focus();
                return false;
            }

            var partnerCode = PartnerCodeTextEdit.Text.Trim();

            // Kiểm tra trùng lặp mã đối tác
            if (IsEditMode)
            {
                // Nếu đang chỉnh sửa, chỉ kiểm tra trùng khi mã đã thay đổi
                var existingPartner = _bll.GetById(_businessPartnerId);
                if (existingPartner != null && existingPartner.PartnerCode != partnerCode)
                {
                    if (_bll.IsCodeExists(partnerCode))
                    {
                        dxErrorProvider1.SetError(PartnerCodeTextEdit, "Mã đối tác đã tồn tại trong hệ thống", ErrorType.Critical);
                        PartnerCodeTextEdit?.Focus();
                        return false;
                    }
                }
            }
            else
            {
                // Nếu thêm mới, luôn kiểm tra trùng
                if (_bll.IsCodeExists(partnerCode))
                {
                    dxErrorProvider1.SetError(PartnerCodeTextEdit, "Mã đối tác đã tồn tại trong hệ thống", ErrorType.Critical);
                    PartnerCodeTextEdit?.Focus();
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Validate tên đối tác (bắt buộc)
        /// </summary>
        /// <returns>True nếu hợp lệ, False nếu không hợp lệ</returns>
        private bool ValidatePartnerName()
        {
            // Kiểm tra không được để trống
            if (string.IsNullOrWhiteSpace(PartnerNameTextEdit?.Text))
            {
                dxErrorProvider1.SetError(PartnerNameTextEdit, "Tên đối tác không được để trống", ErrorType.Critical);
                PartnerNameTextEdit?.Focus();
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validate loại đối tác (khuyến nghị bắt buộc chọn)
        /// </summary>
        /// <returns>True nếu hợp lệ, False nếu không hợp lệ</returns>
        private bool ValidatePartnerType()
        {
            // Kiểm tra đã chọn loại đối tác
            if (PartnerTypeNameComboBoxEdit != null && PartnerTypeNameComboBoxEdit.SelectedIndex < 0)
            {
                dxErrorProvider1.SetError(PartnerTypeNameComboBoxEdit, "Vui lòng chọn loại đối tác", ErrorType.Warning);
                PartnerTypeNameComboBoxEdit.Focus();
                return false;
            }

            return true;
        }

        #endregion

        #endregion

        #region Helpers

        /// <summary>
        /// Thực thi async operation với waiting form (hiển thị splash screen)
        /// </summary>
        /// <param name="operation">Operation async cần thực thi</param>
        private async Task ExecuteWithWaitingFormAsync(Func<Task> operation)
        {
            try
            {
                // Hiển thị waiting form
                SplashScreenManager.ShowForm(typeof(WaitForm1));

                // Thực hiện operation
                await operation();
            }
            finally
            {
                // Đóng waiting form
                SplashScreenManager.CloseForm();
            }
        }

        /// <summary>
        /// Chuyển đổi Image sang byte array (JPEG format để giảm kích thước)
        /// </summary>
        /// <param name="image">Image cần chuyển đổi</param>
        /// <returns>Byte array chứa dữ liệu hình ảnh, null nếu image là null</returns>
        private byte[] ImageToByteArray(Image image)
        {
            if (image == null)
            {
                return null;
            }

            try
            {
                // Clone Image để tránh lỗi GDI+ khi Image đang bị lock hoặc stream gốc đã đóng
                using (var clonedImage = CloneImage(image))
                {
                    using (var ms = new MemoryStream())
                    {
                        // Lưu với format JPEG để giảm kích thước
                        clonedImage.Save(ms, ImageFormat.Jpeg);
                        return ms.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                // Log lỗi và thử cách khác nếu clone thất bại
                System.Diagnostics.Debug.WriteLine($"Lỗi ImageToByteArray: {ex.Message}");
                
                // Fallback: thử save trực tiếp (có thể fail nếu Image bị lock)
                try
                {
                    using (var ms = new MemoryStream())
                    {
                        image.Save(ms, ImageFormat.Jpeg);
                        return ms.ToArray();
                    }
                }
                catch
                {
                    throw new InvalidOperationException("Không thể chuyển đổi hình ảnh sang byte array. Vui lòng thử lại với hình ảnh khác.", ex);
                }
            }
        }

        /// <summary>
        /// Clone Image để tạo bản copy độc lập, tránh lỗi GDI+ khi Image bị lock
        /// </summary>
        /// <param name="image">Image cần clone</param>
        /// <returns>Image đã được clone</returns>
        private Image CloneImage(Image image)
        {
            if (image == null)
            {
                return null;
            }

            // Tạo bitmap mới từ Image gốc
            var bitmap = new Bitmap(image.Width, image.Height);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.DrawImage(image, 0, 0, image.Width, image.Height);
            }
            return bitmap;
        }

        /// <summary>
        /// Kiểm tra định dạng hình ảnh có hợp lệ không (JPG, PNG, GIF) bằng cách kiểm tra magic bytes
        /// </summary>
        /// <param name="imageBytes">Byte array chứa dữ liệu hình ảnh</param>
        /// <returns>True nếu hợp lệ, False nếu không hợp lệ</returns>
        private bool IsValidImageFormat(byte[] imageBytes)
        {
            if (imageBytes == null || imageBytes.Length < 4)
            {
                return false;
            }

            // Kiểm tra magic bytes của các định dạng được hỗ trợ
            // JPEG: FF D8 FF
            if (imageBytes[0] == 0xFF && imageBytes[1] == 0xD8 && imageBytes[2] == 0xFF)
            {
                return true;
            }

            // PNG: 89 50 4E 47
            if (imageBytes[0] == 0x89 && imageBytes[1] == 0x50 && imageBytes[2] == 0x4E && imageBytes[3] == 0x47)
            {
                return true;
            }

            // GIF: 47 49 46 38 (GIF8)
            if (imageBytes[0] == 0x47 && imageBytes[1] == 0x49 && imageBytes[2] == 0x46 && imageBytes[3] == 0x38)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Hiển thị lỗi qua XtraMessageBox với thông báo tiếng Việt
        /// </summary>
        /// <param name="ex">Exception cần hiển thị</param>
        /// <param name="action">Tên hành động đang thực hiện khi xảy ra lỗi</param>
        private void ShowError(Exception ex, string action)
        {
            MsgBox.ShowException(ex, $"Lỗi {action}");
        }

        #endregion
    }
}
