using System;
using System.Windows.Forms;
using DevExpress.XtraBars.Docking;

namespace Inventory.Management.DeviceMangement
{
    public partial class FrmDeviceDtoMangement : DevExpress.XtraEditors.XtraForm
    {
        public FrmDeviceDtoMangement()
        {
            InitializeComponent();
            InitializeForm();
        }

        /// <summary>
        /// Khởi tạo form
        /// </summary>
        private void InitializeForm()
        {
            try
            {
                // Đóng dockPanel1 khi khởi tạo
                dockPanel1.Visibility = DockVisibility.Hidden;

                // Setup events
                InitializeEvents();
            }
            catch (Exception ex)
            {
                Common.Utils.MsgBox.ShowError($"Lỗi khởi tạo form: {ex.Message}");
            }
        }

        /// <summary>
        /// Khởi tạo các event handlers
        /// </summary>
        private void InitializeEvents()
        {
            // Event khi click nút Thêm mới
            ThemMoiBarButtonItem.ItemClick += ThemMoiBarButtonItem_ItemClick;
        }

        /// <summary>
        /// Event handler khi click nút Thêm mới
        /// Mở dockPanel1 với độ rộng = 2/3 độ rộng màn hình
        /// </summary>
        private void ThemMoiBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                // Tính độ rộng = 2/3 độ rộng màn hình
                int screenWidth = Screen.PrimaryScreen.WorkingArea.Width;
                int panelWidth = (int)(screenWidth * 2.0 / 3.0);

                // Set độ rộng cho dockPanel1
                dockPanel1.Width = panelWidth;

                // Hiển thị dockPanel1
                dockPanel1.Visibility = DockVisibility.Visible;
            }
            catch (Exception ex)
            {
                Common.Utils.MsgBox.ShowError($"Lỗi mở panel thêm mới: {ex.Message}");
            }
        }
    }
}