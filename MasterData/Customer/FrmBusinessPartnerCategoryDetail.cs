using Bll.MasterData.Customer;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using DTO.MasterData.CustomerPartner;
using System;
using System.Windows.Forms;

namespace MasterData.Customer
{
    /// <summary>
    /// Form chi tiết danh mục đối tác - thêm mới và chỉnh sửa.
    /// Cung cấp giao diện nhập liệu cho thông tin danh mục đối tác với validation và xử lý lỗi.
    /// </summary>
    public partial class FrmBusinessPartnerCategoryDetail : XtraForm
    {
        #region ========== KHAI BÁO BIẾN ==========

        /// <summary>
        /// Business Logic Layer cho danh mục đối tác
        /// </summary>
        private readonly BusinessPartnerCategoryBll _businessPartnerCategoryBll = new();

        /// <summary>
        /// ID của danh mục đang chỉnh sửa (Guid.Empty nếu thêm mới)
        /// </summary>
        private readonly Guid _categoryId;

        /// <summary>
        /// Trạng thái chỉnh sửa (true nếu đang chỉnh sửa, false nếu thêm mới)
        /// </summary>
        private bool IsEditMode => _categoryId != Guid.Empty;

        #endregion

        #region ========== CONSTRUCTOR & PUBLIC METHODS ==========

        /// <summary>
        /// Khởi tạo form cho thêm mới danh mục.
        /// </summary>
        public FrmBusinessPartnerCategoryDetail() : this(Guid.Empty)
        {
        }

        /// <summary>
        /// Khởi tạo form cho chỉnh sửa danh mục.
        /// </summary>
        /// <param name="categoryId">ID của danh mục cần chỉnh sửa</param>
        public FrmBusinessPartnerCategoryDetail(Guid categoryId)
        {
            InitializeComponent();
            _categoryId = categoryId;
            InitializeForm();
        }

        #endregion

        #region ========== KHỞI TẠO FORM ==========

        /// <summary>
        /// Khởi tạo form và load dữ liệu nếu cần.
        /// </summary>
        private void InitializeForm()
        {
            try
            {
                // Thiết lập tiêu đề form
                Text = IsEditMode ? "Điều chỉnh danh mục đối tác" : "Thêm mới danh mục đối tác";

                // Đánh dấu các trường bắt buộc theo DataAnnotations của DTO
                RequiredFieldHelper.MarkRequiredFields(this, typeof(BusinessPartnerCategoryDto));

                // Thiết lập SuperToolTip cho các controls
                SetupSuperToolTips();

                // Load dữ liệu nếu đang chỉnh sửa
                if (IsEditMode)
                {
                    LoadCategoryData();
                }

                // Thiết lập focus cho control đầu tiên
                CategoryNameTextEdit.Focus();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khởi tạo form");
            }
        }

        #endregion

        #region ========== QUẢN LÝ DỮ LIỆU ==========

        /// <summary>
        /// Load dữ liệu danh mục để chỉnh sửa.
        /// </summary>
        private void LoadCategoryData()
        {
            try
            {
                var category = _businessPartnerCategoryBll.GetById(_categoryId);
                if (category == null)
                {
                    ShowError("Không tìm thấy danh mục đối tác");
                    DialogResult = DialogResult.Cancel;
                    return;
                }

                var dto = category.ToDto();
                BindDataToControls(dto);
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi tải dữ liệu danh mục");
            }
        }

        /// <summary>
        /// Bind dữ liệu DTO vào các control.
        /// </summary>
        /// <param name="dto">DTO chứa dữ liệu</param>
        private void BindDataToControls(BusinessPartnerCategoryDto dto)
        {
            CategoryNameTextEdit.Text = dto.CategoryName;
            DescriptionMemoEdit.Text = dto.Description;
        }

        /// <summary>
        /// Lấy dữ liệu từ các control và tạo DTO.
        /// </summary>
        /// <returns>DTO chứa dữ liệu từ form</returns>
        private BusinessPartnerCategoryDto GetDataFromControls()
        {
            return new BusinessPartnerCategoryDto
            {
                Id = _categoryId,
                CategoryName = CategoryNameTextEdit?.Text?.Trim(),
                Description = DescriptionMemoEdit?.Text?.Trim()
            };
        }

        /// <summary>
        /// Lưu dữ liệu danh mục.
        /// </summary>
        private void SaveCategory()
        {
            try
            {
                var dto = GetDataFromControls();
                var entity = dto.ToEntity();

                if (IsEditMode)
                {
                    // Cập nhật danh mục hiện có
                    _businessPartnerCategoryBll.Update(entity);
                    ShowInfo("Cập nhật danh mục đối tác thành công!");
                }
                else
                {
                    // Thêm danh mục mới
                    _businessPartnerCategoryBll.Insert(entity);
                    ShowInfo("Thêm mới danh mục đối tác thành công!");
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi lưu dữ liệu danh mục");
            }
        }

        #endregion

        #region ========== SỰ KIỆN FORM ==========

        /// <summary>
        /// Xử lý sự kiện click button Lưu
        /// </summary>
        private void SaveBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (ValidateInput())
            {
                SaveCategory();
            }
        }

        /// <summary>
        /// Xử lý sự kiện click button Hủy
        /// </summary>
        private void CancelBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        /// <summary>
        /// Xử lý phím tắt cho form
        /// </summary>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.S))
            {
                SaveBarButtonItem_ItemClick(null, null);
                return true;
            }

            if (keyData == Keys.Escape)
            {
                CancelBarButtonItem_ItemClick(null, null);
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion

        #region ========== XỬ LÝ DỮ LIỆU ==========

        /// <summary>
        /// Validate dữ liệu đầu vào.
        /// </summary>
        /// <returns>True nếu hợp lệ, False nếu không</returns>
        private bool ValidateInput()
        {
            dxErrorProvider1.ClearErrors();

            // CategoryName bắt buộc
            if (string.IsNullOrWhiteSpace(CategoryNameTextEdit?.Text))
            {
                dxErrorProvider1.SetError(CategoryNameTextEdit, "Tên phân loại không được để trống",
                    ErrorType.Critical);
                CategoryNameTextEdit?.Focus();
                return false;
            }

            // Kiểm tra độ dài CategoryName
            if (CategoryNameTextEdit.Text.Trim().Length > 100)
            {
                dxErrorProvider1.SetError(CategoryNameTextEdit, "Tên phân loại không được vượt quá 100 ký tự",
                    ErrorType.Critical);
                CategoryNameTextEdit?.Focus();
                return false;
            }

            // Kiểm tra trùng lặp tên phân loại (không tính bản ghi đang chỉnh sửa)
            var categoryName = CategoryNameTextEdit.Text.Trim();
            if (_businessPartnerCategoryBll.IsCategoryNameExists(categoryName, _categoryId))
            {
                dxErrorProvider1.SetError(CategoryNameTextEdit, "Tên phân loại đã tồn tại trong hệ thống",
                    ErrorType.Critical);
                CategoryNameTextEdit?.Focus();
                return false;
            }

            // Kiểm tra độ dài Description
            if (!string.IsNullOrWhiteSpace(DescriptionMemoEdit?.Text) && DescriptionMemoEdit.Text.Trim().Length > 255)
            {
                dxErrorProvider1.SetError(DescriptionMemoEdit, "Mô tả không được vượt quá 255 ký tự",
                    ErrorType.Critical);
                DescriptionMemoEdit?.Focus();
                return false;
            }

            return true;
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
                if (CategoryNameTextEdit != null)
                {
                    SuperToolTipHelper.SetTextEditSuperTip(
                        CategoryNameTextEdit,
                        title: "<b><color=DarkBlue>📋 Tên phân loại</color></b>",
                        content: "Nhập tên phân loại đối tác. Trường này là bắt buộc."
                    );
                }

                if (DescriptionMemoEdit != null)
                {
                    SuperToolTipHelper.SetBaseEditSuperTip(
                        DescriptionMemoEdit,
                        title: "<b><color=DarkBlue>📝 Mô tả</color></b>",
                        content: "Nhập mô tả chi tiết về phân loại đối tác (tối đa 255 ký tự)."
                    );
                }

                if (SaveBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        SaveBarButtonItem,
                        title: "<b><color=Blue>💾 Lưu</color></b>",
                        content: "Lưu thông tin danh mục đối tác vào hệ thống."
                    );
                }

                if (CloseBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        CloseBarButtonItem,
                        title: "<b><color=Red>❌ Đóng</color></b>",
                        content: "Đóng form mà không lưu thay đổi."
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
        /// Hiển thị thông tin.
        /// </summary>
        /// <param name="message">Thông báo</param>
        private void ShowInfo(string message)
        {
            MsgBox.ShowSuccess(message);
        }

        /// <summary>
        /// Hiển thị lỗi.
        /// </summary>
        /// <param name="message">Thông báo lỗi</param>
        private void ShowError(string message)
        {
            MsgBox.ShowError(message);
        }

        /// <summary>
        /// Hiển thị lỗi với thông tin ngữ cảnh.
        /// </summary>
        /// <param name="ex">Exception</param>
        /// <param name="context">Ngữ cảnh lỗi</param>
        private void ShowError(Exception ex, string context = null)
        {
            MsgBox.ShowException(string.IsNullOrWhiteSpace(context)
                ? ex
                : new Exception(context + ": " + ex.Message, ex));
        }

        #endregion
    }
}
