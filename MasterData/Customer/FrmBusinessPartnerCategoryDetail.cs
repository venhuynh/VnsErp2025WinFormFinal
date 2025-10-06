using System;
using System.Windows.Forms;
using Bll.MasterData.Customer;
using Bll.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using MasterData.Customer.Converters;
using MasterData.Customer.Dto;

namespace MasterData.Customer
{
    /// <summary>
    /// Form chi tiết danh mục đối tác - thêm mới và chỉnh sửa.
    /// </summary>
    public partial class FrmBusinessPartnerCategoryDetail : XtraForm
    {
        #region Fields

        private readonly BusinessPartnerCategoryBll _businessPartnerCategoryBll = new BusinessPartnerCategoryBll();
        private readonly Guid _categoryId;
        private bool _isEditMode => _categoryId != Guid.Empty;

        #endregion

        #region Constructor

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

        #region Private Methods

        /// <summary>
        /// Khởi tạo form và load dữ liệu nếu cần.
        /// </summary>
        private void InitializeForm()
        {
            try
            {
                // Thiết lập tiêu đề form
                Text = _isEditMode ? "Điều chỉnh danh mục đối tác" : "Thêm mới danh mục đối tác";

                // Load dữ liệu nếu đang chỉnh sửa
                if (_isEditMode)
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
        /// Validate dữ liệu đầu vào.
        /// </summary>
        /// <returns>True nếu hợp lệ, False nếu không</returns>
        private bool ValidateInput()
        {
            dxErrorProvider1.ClearErrors();

            // CategoryName bắt buộc
            if (string.IsNullOrWhiteSpace(CategoryNameTextEdit?.Text))
            {
                dxErrorProvider1.SetError(CategoryNameTextEdit, "Tên phân loại không được để trống", ErrorType.Critical);
                CategoryNameTextEdit?.Focus();
                return false;
            }

            // Kiểm tra độ dài CategoryName
            if (CategoryNameTextEdit.Text.Trim().Length > 100)
            {
                dxErrorProvider1.SetError(CategoryNameTextEdit, "Tên phân loại không được vượt quá 100 ký tự", ErrorType.Critical);
                CategoryNameTextEdit?.Focus();
                return false;
            }

            // Kiểm tra trùng lặp tên phân loại (không tính bản ghi đang chỉnh sửa)
            var categoryName = CategoryNameTextEdit.Text.Trim();
            if (_businessPartnerCategoryBll.IsCategoryNameExists(categoryName, _categoryId))
            {
                dxErrorProvider1.SetError(CategoryNameTextEdit, "Tên phân loại đã tồn tại trong hệ thống", ErrorType.Critical);
                CategoryNameTextEdit?.Focus();
                return false;
            }

            // Kiểm tra độ dài Description
            if (!string.IsNullOrWhiteSpace(DescriptionMemoEdit?.Text) && DescriptionMemoEdit.Text.Trim().Length > 255)
            {
                dxErrorProvider1.SetError(DescriptionMemoEdit, "Mô tả không được vượt quá 255 ký tự", ErrorType.Critical);
                DescriptionMemoEdit?.Focus();
                return false;
            }

            return true;
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

                if (_isEditMode)
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

        /// <summary>
        /// Hiển thị thông tin.
        /// </summary>
        /// <param name="message">Thông báo</param>
        private void ShowInfo(string message)
        {
            MsgBox.ShowInfo(message);
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
            if (string.IsNullOrWhiteSpace(context))
                MsgBox.ShowException(ex);
            else
                MsgBox.ShowException(new Exception(context + ": " + ex.Message, ex));
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Người dùng bấm nút Lưu.
        /// </summary>
        private void SaveBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (ValidateInput())
            {
                SaveCategory();
            }
        }

        /// <summary>
        /// Người dùng bấm nút Hủy.
        /// </summary>
        private void CancelBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        /// <summary>
        /// Xử lý phím tắt.
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
    }
}
