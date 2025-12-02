using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.WinExplorer;
using DevExpress.Utils;
using DTO.Inventory.Query;
using Bll.Inventory.InventoryManagement;
using Dal.DataContext;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventory.Query
{
    /// <summary>
    /// Form hiển thị danh sách hình ảnh nhập/xuất kho sử dụng WinExplorerView
    /// </summary>
    public partial class FrmStockInOutImageLookup : XtraForm
    {
        private Guid? _stockInOutMasterId;
        private List<StockInOutImageDto> _dataSource;
        private StockInOutImageBll _stockInOutImageBll;

        /// <summary>
        /// Constructor mặc định - hiển thị tất cả hình ảnh
        /// </summary>
        public FrmStockInOutImageLookup()
        {
            InitializeComponent();
            InitializeBll();
            InitializeWinExplorerView();
        }

        /// <summary>
        /// Constructor với StockInOutMasterId - chỉ hiển thị hình ảnh của phiếu nhập/xuất cụ thể
        /// </summary>
        /// <param name="stockInOutMasterId">ID phiếu nhập/xuất kho</param>
        public FrmStockInOutImageLookup(Guid stockInOutMasterId) : this()
        {
            _stockInOutMasterId = stockInOutMasterId;
        }

        /// <summary>
        /// Khởi tạo BLL
        /// </summary>
        private void InitializeBll()
        {
            try
            {
                _stockInOutImageBll = new StockInOutImageBll();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Lỗi khởi tạo dịch vụ hình ảnh: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _stockInOutImageBll = null;
            }
        }

        /// <summary>
        /// Khởi tạo cấu hình WinExplorerView
        /// </summary>
        private void InitializeWinExplorerView()
        {
            // Cấu hình ColumnSet
            winExplorerView.ColumnSet.TextColumn = colFileName;
            winExplorerView.ColumnSet.DescriptionColumn = colFileSizeDisplay;
            winExplorerView.ColumnSet.ExtraLargeImageColumn = colImageData;
            winExplorerView.ColumnSet.LargeImageColumn = colImageData;
            winExplorerView.ColumnSet.MediumImageColumn = colImageData;
            winExplorerView.ColumnSet.SmallImageColumn = colImageData;

            // Đăng ký event để xử lý thumbnail
            winExplorerView.GetThumbnailImage += WinExplorerView_GetThumbnailImage;

            // Cấu hình mặc định
            winExplorerView.OptionsView.Style = WinExplorerViewStyle.ExtraLarge;
            winExplorerView.OptionsView.ShowCheckBoxes = true;
            winExplorerView.OptionsView.ShowCheckBoxInGroupCaption = true;
            winExplorerView.OptionsView.ShowExpandCollapseButtons = true;
            winExplorerView.OptionsSelection.ItemSelectionMode = DevExpress.XtraGrid.Views.WinExplorer.IconItemSelectionMode.CheckBox;
            winExplorerView.OptionsImageLoad.AnimationType = DevExpress.Utils.ImageContentAnimationType.Expand;
        }

        /// <summary>
        /// Xử lý event GetThumbnailImage để convert ImageData (byte[]) thành Image
        /// Load từ storage nếu ImageData chưa có trong DTO
        /// </summary>
        private async void WinExplorerView_GetThumbnailImage(object sender, ThumbnailImageEventArgs e)
        {
            try
            {
                // Sử dụng DataSourceIndex để lấy dữ liệu từ DataSource
                if (_dataSource == null || e.DataSourceIndex < 0 || e.DataSourceIndex >= _dataSource.Count)
                    return;

                var dto = _dataSource[e.DataSourceIndex];
                if (dto == null)
                    return;

                byte[] imageData = dto.ImageData;

                // Nếu ImageData chưa có, load từ storage
                if ((imageData == null || imageData.Length == 0) && 
                    _stockInOutImageBll != null && 
                    !string.IsNullOrEmpty(dto.RelativePath))
                {
                    try
                    {
                        imageData = await _stockInOutImageBll.GetImageDataAsync(dto.Id);
                        // Cache lại vào DTO để không phải load lại lần sau
                        if (imageData != null)
                        {
                            dto.ImageData = imageData;
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Lỗi load image từ storage: {ex.Message}");
                    }
                }

                // Convert byte[] thành Image
                if (imageData != null && imageData.Length > 0)
                {
                    using (var ms = new MemoryStream(imageData))
                    {
                        var image = Image.FromStream(ms);
                        // Sử dụng CreateThumbnailImage để tạo thumbnail với kích thước mong muốn
                        e.ThumbnailImage = e.CreateThumbnailImage(image, e.DesiredThumbnailSize);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error nếu cần
                System.Diagnostics.Debug.WriteLine($"Lỗi load thumbnail: {ex.Message}");
            }
        }

        /// <summary>
        /// Load dữ liệu hình ảnh
        /// </summary>
        public void LoadData(List<StockInOutImageDto> images)
        {
            if (images == null)
            {
                _dataSource = null;
                gridControl.DataSource = null;
                return;
            }

            // Filter theo StockInOutMasterId nếu có
            if (_stockInOutMasterId.HasValue)
            {
                images = images.Where(x => x.StockInOutMasterId == _stockInOutMasterId.Value).ToList();
            }

            // Lưu reference để sử dụng trong GetThumbnailImage event
            _dataSource = images;
            gridControl.DataSource = images;
            winExplorerView.RefreshData();
        }

        /// <summary>
        /// Load dữ liệu từ database
        /// </summary>
        public async Task LoadDataAsync()
        {
            try
            {
                if (_stockInOutImageBll == null)
                {
                    XtraMessageBox.Show("Dịch vụ hình ảnh chưa được khởi tạo.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LoadData(new List<StockInOutImageDto>());
                    return;
                }

                List<StockInOutImageDto> images;

                if (_stockInOutMasterId.HasValue)
                {
                    // Load hình ảnh theo StockInOutMasterId
                    var entities = _stockInOutImageBll.GetByStockInOutMasterId(_stockInOutMasterId.Value);
                    images = MapEntitiesToDtos(entities);
                }
                else
                {
                    // Load tất cả hình ảnh (nếu cần)
                    // TODO: Implement GetAll method trong BLL nếu cần
                    images = new List<StockInOutImageDto>();
                }

                LoadData(images);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoadData(new List<StockInOutImageDto>());
            }
        }

        /// <summary>
        /// Map entities sang DTOs
        /// </summary>
        private List<StockInOutImageDto> MapEntitiesToDtos(List<StockInOutImage> entities)
        {
            if (entities == null)
                return new List<StockInOutImageDto>();

            return entities.Select(entity => new StockInOutImageDto
            {
                Id = entity.Id,
                StockInOutMasterId = entity.StockInOutMasterId,
                // ImageData sẽ được load từ storage trong GetThumbnailImage event nếu cần
                ImageData = null, // Không load ImageData ngay để tránh load quá nhiều dữ liệu
                FileName = entity.FileName,
                RelativePath = entity.RelativePath,
                FullPath = entity.FullPath,
                StorageType = entity.StorageType,
                FileSize = entity.FileSize,
                FileExtension = entity.FileExtension,
                MimeType = entity.MimeType,
                Checksum = entity.Checksum,
                FileExists = entity.FileExists,
                LastVerified = entity.LastVerified,
                MigrationStatus = entity.MigrationStatus,
                CreateDate = entity.CreateDate,
                CreateBy = entity.CreateBy,
                ModifiedDate = entity.ModifiedDate,
                ModifiedBy = entity.ModifiedBy
            }).ToList();
        }


        /// <summary>
        /// Lấy danh sách hình ảnh được chọn
        /// </summary>
        public List<StockInOutImageDto> GetSelectedImages()
        {
            var selectedImages = new List<StockInOutImageDto>();
            var selectedRows = winExplorerView.GetSelectedRows();

            foreach (int rowHandle in selectedRows)
            {
                var dto = winExplorerView.GetRow(rowHandle) as StockInOutImageDto;
                if (dto != null)
                {
                    selectedImages.Add(dto);
                }
            }

            return selectedImages;
        }

        /// <summary>
        /// Load form
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LoadDataAsync();
        }
    }
}
