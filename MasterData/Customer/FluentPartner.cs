using Bll.Common;
using System;
using System.Windows.Forms;
using DevExpress.XtraBars.FluentDesignSystem;

namespace MasterData.Customer
{
    public partial class FluentPartner : DevExpress.XtraBars.FluentDesignSystem.FluentDesignForm
    {
        #region Fields

        private UserControlManager _userControlManager;
        private UcBusinessPartnerList _businessPartnerListControl;
        private UcBusinessPartnerCategory _businessPartnerCategoryControl;

        #endregion

        #region Constructor

        public FluentPartner()
        {
            InitializeComponent();
            InitializeUserControls();
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Khởi tạo các UserControl và UserControlManager
        /// </summary>
        private void InitializeUserControls()
        {
            try
            {
                // Khởi tạo UserControlManager với container
                _userControlManager = new UserControlManager(this.fluentDesignFormContainer1);

                // Khởi tạo các UserControl
                _businessPartnerListControl = new UcBusinessPartnerList();
                _businessPartnerCategoryControl = new UcBusinessPartnerCategory();

                // Thiết lập form properties
                this.Text = "VnsErp2025 - Quản lý Đối tác";
                this.WindowState = FormWindowState.Maximized;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khởi tạo form: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Xử lý sự kiện Load form
        /// </summary>
        private void FluentPartner_Load(object sender, EventArgs e)
        {
            try
            {
                // Hiển thị màn hình mặc định (Danh sách đối tác)
                ShowBusinessPartnerList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load form: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Xử lý click nút Danh mục đối tác
        /// </summary>
        private void BusinessPartnerCategoryBtn_Click(object sender, EventArgs e)
        {
            try
            {
                ShowBusinessPartnerCategory();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi hiển thị danh mục đối tác: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Xử lý click nút Danh sách đối tác
        /// </summary>
        private void BusinessPartnerListBtn_Click(object sender, EventArgs e)
        {
            try
            {
                ShowBusinessPartnerList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi hiển thị danh sách đối tác: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Navigation Methods

        /// <summary>
        /// Hiển thị màn hình danh sách đối tác
        /// </summary>
        private void ShowBusinessPartnerList()
        {
            try
            {
                if (_businessPartnerListControl == null)
                {
                    _businessPartnerListControl = new UcBusinessPartnerList();
                }

                _userControlManager?.ShowControl(_businessPartnerListControl, "Danh sách đối tác");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi hiển thị danh sách đối tác: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Hiển thị màn hình danh mục đối tác
        /// </summary>
        private void ShowBusinessPartnerCategory()
        {
            try
            {
                if (_businessPartnerCategoryControl == null)
                {
                    _businessPartnerCategoryControl = new UcBusinessPartnerCategory();
                }

                _userControlManager?.ShowControl(_businessPartnerCategoryControl, "Danh mục đối tác");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi hiển thị danh mục đối tác: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Form Management

        /// <summary>
        /// Xử lý sự kiện đóng form
        /// </summary>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            try
            {
                // Cleanup resources
                _businessPartnerListControl?.Dispose();
                _businessPartnerCategoryControl?.Dispose();
                _userControlManager = null;

                base.OnFormClosing(e);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi đóng form: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Xử lý sự kiện đóng form (FormClosed)
        /// </summary>
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            try
            {
                base.OnFormClosed(e);
            }
            catch (Exception ex)
            {
                // Silent error handling
            }
        }

        #endregion
    }
}
