using System;
using System.Windows.Forms;
using System.Threading.Tasks;
using DevExpress.XtraSplashScreen;
using Bll.Common;
using Bll.MasterData.ProductServiceBll;
using MasterData.ProductService.Converters;

namespace MasterData.ProductService
{
    public partial class FrmProductVariantDetail : DevExpress.XtraEditors.XtraForm
    {
        private readonly Guid _productVariantId;
        private readonly ProductServiceBll _productServiceBll = new ProductServiceBll();
		private bool _isLoadingDataSources;

        public FrmProductVariantDetail(Guid productVariantId)
        {
            _productVariantId = productVariantId;
            InitializeComponent();
			this.Load += FrmProductVariantDetail_Load;
        }

		private async void FrmProductVariantDetail_Load(object sender, EventArgs e)
		{
			await LoadDataSourcesAsync();
		}

		private async Task LoadDataSourcesAsync()
		{
			if (_isLoadingDataSources) return;
			_isLoadingDataSources = true;
			try
			{
				await ExecuteWithWaitingFormAsync(async () =>
				{
					await LoadProductServicesDataSourceAsync();
				});
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Lỗi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			finally
			{
				_isLoadingDataSources = false;
			}
		}

		private async Task LoadProductServicesDataSourceAsync()
		{
			var entities = await _productServiceBll.GetFilteredAsync(
				isActive: true,
				orderBy: "Name",
				orderDirection: "ASC");

			var dtos = entities.ToDtoList(categoryId => _productServiceBll.GetCategoryName(categoryId));
			productServiceDtoBindingSource.DataSource = dtos;
		}

		private async Task ExecuteWithWaitingFormAsync(Func<Task> operation)
		{
			try
			{
				SplashScreenManager.ShowForm(typeof(WaitForm1));
				await operation();
			}
			finally
			{
				SplashScreenManager.CloseForm();
			}
		}

    }
}