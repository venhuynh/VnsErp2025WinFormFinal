using Bll.Inventory.InventoryManagement;
using Common.Utils;
using DTO.Inventory.Report;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Linq;

namespace Inventory.Report
{
    public partial class RpPhieuGiaoHang02 : DevExpress.XtraReports.UI.XtraReport
    {
        #region Private Fields

        private readonly StockInOutReportBll _stockInOutReportBll;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor mặc định (private để tránh khởi tạo trực tiếp)
        /// </summary>
        private RpPhieuGiaoHang02()
        {
            InitializeComponent();
            _stockInOutReportBll = new StockInOutReportBll();
            LoadCompanyLogo();
        }

        

        /// <summary>
        /// Constructor với VoucherId để tự động load dữ liệu
        /// </summary>
        /// <param name="voucherId">ID phiếu nhập xuất kho (StockInOutMaster.Id)</param>
        public RpPhieuGiaoHang02(Guid voucherId) : this()
        {
            LoadData(voucherId);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Load dữ liệu và bind vào report
        /// </summary>
        /// <param name="voucherId">ID phiếu nhập xuất kho (StockInOutMaster.Id)</param>
        private void LoadData(Guid voucherId)
        {
            try
            {

                // Set parameter
                VoucherId.Value = voucherId;

                // Lấy dữ liệu từ BLL
                var reportData = _stockInOutReportBll.GetReportDtoById(voucherId);

                if (reportData == null)
                {
                    MsgBox.ShowWarning($"Không tìm thấy dữ liệu cho phiếu nhập xuất với ID: {voucherId}");
                    return;
                }

                // Bind data source thông qua bindingSource1
                // Wrap reportData trong List để đáp ứng yêu cầu của DevExpress
                bindingSource1.DataSource = new List<StockInOutReportDto> { reportData };

            }
            catch (Exception ex)
            {
                MsgBox.ShowError($"Lỗi tải dữ liệu cho báo cáo: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Load logo công ty từ đường dẫn tương đối
        /// </summary>
        private void LoadCompanyLogo()
        {
            try
            {
                //FIXME: Đã import file vào thư mục Resources của project này
                // Thử các đường dẫn có thể có của logo
                string[] possiblePaths = new string[]
                {
                    // Đường dẫn từ Application.StartupPath
                    Path.Combine(Application.StartupPath, "Images", "Ven - Solutions logo_1438x617_background_transfer.jpg"),
                    // Đường dẫn từ thư mục gốc của solution
                    Path.Combine(Application.StartupPath, "..", "..", "VnsErp2025.Blazor.Server", "Images", "Ven - Solutions logo_1438x617_background_transfer.jpg"),
                    // Đường dẫn tuyệt đối (fallback)
                    @"C:\Users\Admin\source\Workspaces\2025\VnsErp2025\VnsErp2025.Blazor.Server\Images\Ven - Solutions logo_1438x617_background_transfer.jpg"
                };

                foreach (string logoPath in possiblePaths)
                {
                    if (File.Exists(logoPath))
                    {
                        xrPictureBox1.ImageUrl = logoPath;
                        return;
                    }
                }

                // Nếu không tìm thấy, thử tìm file logo với tên khác trong thư mục Images
                string imagesFolder = Path.Combine(Application.StartupPath, "Images");
                if (Directory.Exists(imagesFolder))
                {
                    string[] logoFiles = Directory.GetFiles(imagesFolder, "*logo*.jpg", SearchOption.TopDirectoryOnly);
                    if (logoFiles.Length > 0)
                    {
                        xrPictureBox1.ImageUrl = logoFiles[0];
                        return;
                    }
                }

                // Nếu vẫn không tìm thấy, để trống (logo sẽ không hiển thị)
                xrPictureBox1.ImageUrl = null;
            }
            catch (Exception ex)
            {
                // Log lỗi nhưng không throw để report vẫn có thể hiển thị
                System.Diagnostics.Debug.WriteLine($"Không thể load logo công ty: {ex.Message}");
                xrPictureBox1.ImageUrl = null;
            }
        }


        #endregion

    }
}
