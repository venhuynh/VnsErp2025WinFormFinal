using Bll.Inventory.InventoryManagement;
using Bll.Inventory.StockInOut;
using Bll.MasterData.ProductServiceBll;
using Common.Common;
using Dal.DataAccess.Implementations.Inventory.StockIn;
using Dal.DataContext;
using DevExpress.XtraEditors;
using DTO.Inventory.InventoryManagement;
using DTO.Inventory.StockIn.NhapLapRap;
using DTO.MasterData.ProductService;
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
        /// Business Logic Layer cho sản phẩm/dịch vụ
        /// </summary>
        private readonly ProductServiceBll _productServiceBll = new ProductServiceBll();

        /// <summary>
        /// Business Logic Layer cho chi tiết nhập xuất kho
        /// </summary>
        private readonly StockInOutBll _stockInOutBll = new StockInOutBll();

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
            PhieuXuatLapRapLookupEdit.EditValueChanged += PhieuXuatLapRapLookupEdit_EditValueChanged;
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
                // Hiển thị SplashScreen một lần cho cả 2 operations
                SplashScreenHelper.ShowWaitingSplashScreen();
                try
                {
                    // Load cả 2 datasource song song
                    await Task.WhenAll(
                        LoadNhapLapRapListAsyncWithoutSplash(),
                        LoadProductServiceListAsyncWithoutSplash()
                    );
                }
                finally
                {
                    SplashScreenHelper.CloseSplashScreen();
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"FrmTaoThietBiNhapLapRap_Load: Lỗi load dữ liệu: {ex.Message}", ex);
                SplashScreenHelper.CloseSplashScreen();
                MsgBox.ShowError($"Lỗi load dữ liệu: {ex.Message}");
            }
        }

        #endregion

        #region ========== DATA LOADING ==========

        /// <summary>
        /// Load danh sách các phiếu nhập lắp ráp theo thứ tự ngày nhập mới nhất (không hiển thị SplashScreen)
        /// </summary>
        private async Task LoadNhapLapRapListAsyncWithoutSplash()
        {
            try
            {
                _logger.Debug("LoadNhapLapRapListAsyncWithoutSplash: Bắt đầu load danh sách phiếu nhập lắp ráp");

                await Task.Run(() =>
                {
                    try
                    {
                        // Lấy dữ liệu từ BLL
                        var entities = _stockInOutMasterBll.GetPhieuNhapLapRap();

                        // Map entities sang DTOs sử dụng converter
                        var dtos = entities.ToNhapLapRapMasterListDtoList();

                        // Update UI thread
                        BeginInvoke(new Action(() =>
                        {
                            xuatLapRapMasterListDtoBindingSource.DataSource = dtos;
                            xuatLapRapMasterListDtoBindingSource.ResetBindings(false);
                        }));

                        _logger.Info($"LoadNhapLapRapListAsyncWithoutSplash: Đã load {dtos.Count} phiếu nhập lắp ráp");
                    }
                    catch (Exception ex)
                    {
                        _logger.Error("LoadNhapLapRapListAsyncWithoutSplash: Exception occurred", ex);
                        BeginInvoke(new Action(() =>
                        {
                            MsgBox.ShowError($"Lỗi tải danh sách phiếu nhập lắp ráp: {ex.Message}");
                        }));
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.Error($"LoadNhapLapRapListAsyncWithoutSplash: Lỗi load danh sách phiếu nhập lắp ráp: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Load danh sách sản phẩm/dịch vụ tương tự như FrmProductServiceList (không hiển thị SplashScreen)
        /// </summary>
        private async Task LoadProductServiceListAsyncWithoutSplash()
        {
            try
            {
                _logger.Debug("LoadProductServiceListAsyncWithoutSplash: Bắt đầu load danh sách sản phẩm/dịch vụ");

                // Get all data
                var entities = await _productServiceBll.GetAllAsync();
                _logger.Debug($"LoadProductServiceListAsyncWithoutSplash: Đã nhận được {entities?.Count ?? 0} entities từ BLL");

                // Load tất cả categories một lần vào dictionary để tối ưu hiệu suất
                var categoryDict = await _productServiceBll.GetCategoryDictAsync();
                _logger.Debug($"LoadProductServiceListAsyncWithoutSplash: Đã lấy được {categoryDict?.Count ?? 0} categories");

                // Convert to DTOs với categoryDict (tối ưu hơn resolver functions)
                var dtoList = entities.ToDtoList(categoryDict).ToList();
                _logger.Debug($"LoadProductServiceListAsyncWithoutSplash: Đã convert được {dtoList.Count} DTOs");

                // Update UI thread
                BeginInvoke(new Action(() =>
                {
                    productServiceDtoBindingSource.DataSource = dtoList;
                    productServiceDtoBindingSource.ResetBindings(false);
                }));

                _logger.Info($"LoadProductServiceListAsyncWithoutSplash: Đã load {dtoList.Count} sản phẩm/dịch vụ");
            }
            catch (Exception ex)
            {
                _logger.Error($"LoadProductServiceListAsyncWithoutSplash: Lỗi load danh sách sản phẩm/dịch vụ: {ex.Message}", ex);
                BeginInvoke(new Action(() =>
                {
                    MsgBox.ShowError($"Lỗi tải danh sách sản phẩm/dịch vụ: {ex.Message}");
                }));
                throw;
            }
        }

        /// <summary>
        /// Event handler khi giá trị của PhieuXuatLapRapLookupEdit thay đổi
        /// Load chi tiết phiếu nhập lắp ráp từ master id và hiển thị vào XuatLapRapDetailDtoGridControl
        /// </summary>
        private async void PhieuXuatLapRapLookupEdit_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                // Lấy master id từ lookup edit
                if (PhieuXuatLapRapLookupEdit.EditValue == null || 
                    !(PhieuXuatLapRapLookupEdit.EditValue is Guid masterId) || 
                    masterId == Guid.Empty)
                {
                    // Clear datasource nếu không có giá trị
                    xuatLapRapDetailDtoBindingSource.DataSource = new List<NhapLapRapDetailDto>();
                    xuatLapRapDetailDtoBindingSource.ResetBindings(false);
                    return;
                }

                _logger.Debug($"PhieuXuatLapRapLookupEdit_EditValueChanged: Bắt đầu load detail cho MasterId={masterId}");

                // Hiển thị SplashScreen
                SplashScreenHelper.ShowWaitingSplashScreen();

                try
                {
                    await Task.Run(() =>
                    {
                        try
                        {
                            // Lấy detail entities từ BLL
                            var detailEntities = _stockInOutBll.GetDetailsByMasterId(masterId);
                            _logger.Debug($"PhieuXuatLapRapLookupEdit_EditValueChanged: Đã lấy được {detailEntities.Count} detail entities");

                            // Convert entities sang NhapLapRapDetailDto
                            var detailDtos = detailEntities.ToNhapLapRapDetailDtoList();
                            _logger.Debug($"PhieuXuatLapRapLookupEdit_EditValueChanged: Đã convert được {detailDtos.Count} detail DTOs");

                            // Update UI thread
                            BeginInvoke(new Action(() =>
                            {
                                xuatLapRapDetailDtoBindingSource.DataSource = detailDtos;
                                xuatLapRapDetailDtoBindingSource.ResetBindings(false);
                                XuatLapRapDetailDtoGridControl.RefreshDataSource();
                            }));

                            _logger.Info($"PhieuXuatLapRapLookupEdit_EditValueChanged: Đã load {detailDtos.Count} chi tiết phiếu nhập lắp ráp cho MasterId={masterId}");
                        }
                        catch (Exception ex)
                        {
                            _logger.Error($"PhieuXuatLapRapLookupEdit_EditValueChanged: Exception occurred", ex);
                            BeginInvoke(new Action(() =>
                            {
                                MsgBox.ShowError($"Lỗi tải chi tiết phiếu nhập lắp ráp: {ex.Message}");
                            }));
                        }
                    });
                }
                finally
                {
                    SplashScreenHelper.CloseSplashScreen();
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"PhieuXuatLapRapLookupEdit_EditValueChanged: Lỗi xử lý sự kiện: {ex.Message}", ex);
                SplashScreenHelper.CloseSplashScreen();
                MsgBox.ShowError($"Lỗi xử lý sự kiện: {ex.Message}");
            }
        }

        #endregion
    }
}