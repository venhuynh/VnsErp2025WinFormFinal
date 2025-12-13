using Bll.Inventory.InventoryManagement;
using Bll.Inventory.StockIn;
using Common.Common;
using Dal.DataAccess.Implementations.Inventory.StockIn;
using Dal.DataContext;
using DevExpress.XtraEditors;
using DTO.Inventory.InventoryManagement;
using DTO.Inventory.StockIn.NhapLapRap;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common.Utils;
using StockInHistoryQueryCriteria = Dal.DataAccess.Implementations.Inventory.StockIn.StockInHistoryQueryCriteria;

namespace Inventory.OverlayForm
{
    public partial class FrmTaoThietBiNhapLapRap : DevExpress.XtraEditors.XtraForm
    {
        #region ========== FIELDS & PROPERTIES ==========

        /// <summary>
        /// Business Logic Layer cho phiếu nhập xuất kho
        /// </summary>
        private readonly StockInOutMasterBll _stockInOutMasterBll = new StockInOutMasterBll();

        /// <summary>
        /// Logger để ghi log các sự kiện
        /// </summary>
        private readonly ILogger _logger = LoggerFactory.CreateLogger(LogCategory.UI);

        #endregion

        #region ========== CONSTRUCTOR ==========

        public FrmTaoThietBiNhapLapRap()
        {
            InitializeComponent();
            Load += FrmTaoThietBiNhapLapRap_Load;
        }

        #endregion

        #region ========== EVENT HANDLERS ==========

        /// <summary>
        /// Event handler khi form được load
        /// </summary>
        private async void FrmTaoThietBiNhapLapRap_Load(object sender, EventArgs e)
        {
            try
            {
                await LoadNhapLapRapListAsync();
            }
            catch (Exception ex)
            {
                _logger.Error($"FrmTaoThietBiNhapLapRap_Load: Lỗi load dữ liệu: {ex.Message}", ex);
                MsgBox.ShowError($"Lỗi load danh sách phiếu nhập lắp ráp: {ex.Message}");
            }
        }

        #endregion

        #region ========== DATA LOADING ==========

        /// <summary>
        /// Load danh sách các phiếu nhập lắp ráp theo thứ tự ngày nhập mới nhất
        /// </summary>
        private async Task LoadNhapLapRapListAsync()
        {
            try
            {
                _logger.Debug("LoadNhapLapRapListAsync: Bắt đầu load danh sách phiếu nhập lắp ráp");

                // Hiển thị SplashScreen để thông báo đang load dữ liệu
                SplashScreenHelper.ShowWaitingSplashScreen();

                try
                {
                    await Task.Run(() =>
                    {
                        try
                        {
                            // Query dữ liệu từ database
                            var query = new StockInHistoryQueryCriteria
                            {
                                FromDate = DateTime.Now.AddYears(-10), // Lấy dữ liệu 10 năm gần đây
                                ToDate = DateTime.Now,
                                LoaiNhapKho = (int)LoaiNhapXuatKhoEnum.NhapSanPhamLapRap,
                                OrderBy = "StockInDate",
                                OrderDirection = "DESC" // Sắp xếp theo ngày nhập mới nhất
                            };

                            // Lấy dữ liệu từ BLL
                            var entities = _stockInOutMasterBll.QueryHistory(query);

                            // Map entities sang DTOs
                            var dtos = entities.Select(entity => new NhapLapRapMasterListDto
                            {
                                Id = entity.Id,
                                StockInNumber = entity.VocherNumber ?? string.Empty,
                                StockInDate = entity.StockInOutDate,
                                LoaiNhapXuatKho = (LoaiNhapXuatKhoEnum)entity.StockInOutType,
                                TrangThai = (TrangThaiPhieuNhapEnum)entity.VoucherStatus,
                                WarehouseCode = entity.CompanyBranch?.BranchCode ?? string.Empty,
                                WarehouseName = entity.CompanyBranch?.BranchName ?? string.Empty,
                                TotalQuantity = entity.TotalQuantity,
                                CreatedDate = entity.CreatedDate
                            }).ToList();

                            // Update UI thread
                            BeginInvoke(new Action(() =>
                            {
                                xuatLapRapMasterListDtoBindingSource.DataSource = dtos;
                                xuatLapRapMasterListDtoBindingSource.ResetBindings(false);
                            }));

                            _logger.Info($"LoadNhapLapRapListAsync: Đã load {dtos.Count} phiếu nhập lắp ráp");
                        }
                        catch (Exception ex)
                        {
                            _logger.Error("LoadNhapLapRapListAsync: Exception occurred", ex);
                            BeginInvoke(new Action(() =>
                            {
                                MsgBox.ShowError($"Lỗi tải dữ liệu: {ex.Message}");
                            }));
                        }
                    });
                }
                finally
                {
                    // Đóng SplashScreen sau khi hoàn thành hoặc có lỗi
                    SplashScreenHelper.CloseSplashScreen();
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"LoadNhapLapRapListAsync: Lỗi load danh sách phiếu nhập lắp ráp: {ex.Message}", ex);
                SplashScreenHelper.CloseSplashScreen(); // Đảm bảo đóng SplashScreen khi có lỗi
                MsgBox.ShowError($"Lỗi load danh sách phiếu nhập lắp ráp: {ex.Message}");
            }
        }

        #endregion
    }
}