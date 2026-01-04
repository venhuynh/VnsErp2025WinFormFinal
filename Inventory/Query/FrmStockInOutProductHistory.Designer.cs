namespace Inventory.Query
{
    partial class FrmStockInOutProductHistory
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.StockInOutProductHistoryDtoGridControl = new DevExpress.XtraGrid.GridControl();
            this.stockInOutProductHistoryDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.StockInOutProductHistoryDtoGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colStockInOutDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colWarehouseNameHtml = new DevExpress.XtraGrid.Columns.GridColumn();
            this.HtmlRepositoryItemHypertextLabel = new DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel();
            this.colCustomerNameHtml = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colProductVariantFullNameHtml = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colStockInQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colStockOutQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUnitPrice = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colVatAmount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTotalAmount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTotalAmountIncludedVat = new DevExpress.XtraGrid.Columns.GridColumn();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.KeyWordBarEditItem = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.TuNgayBarEditItem = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemDateEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.DenNgayBarEditItem = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemDateEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.XemBaoCaoBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.ChiTietPhieuNhapXuatBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.InPhieuBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.NhapBaoHanhBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.NhapDinhDanhSPBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.ThemHinhAnhBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.XoaPhieuBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.barHeaderItem1 = new DevExpress.XtraBars.BarHeaderItem();
            this.DataSummaryBarStaticItem = new DevExpress.XtraBars.BarStaticItem();
            this.SelectedRowBarStaticItem = new DevExpress.XtraBars.BarStaticItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.StockInOutProductHistoryDtoGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stockInOutProductHistoryDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StockInOutProductHistoryDtoGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HtmlRepositoryItemHypertextLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit2.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.StockInOutProductHistoryDtoGridControl);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 24);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.Root;
            this.layoutControl1.Size = new System.Drawing.Size(1056, 371);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // StockInOutProductHistoryDtoGridControl
            // 
            this.StockInOutProductHistoryDtoGridControl.DataSource = this.stockInOutProductHistoryDtoBindingSource;
            this.StockInOutProductHistoryDtoGridControl.Location = new System.Drawing.Point(12, 12);
            this.StockInOutProductHistoryDtoGridControl.MainView = this.StockInOutProductHistoryDtoGridView;
            this.StockInOutProductHistoryDtoGridControl.MenuManager = this.barManager1;
            this.StockInOutProductHistoryDtoGridControl.Name = "StockInOutProductHistoryDtoGridControl";
            this.StockInOutProductHistoryDtoGridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.HtmlRepositoryItemHypertextLabel});
            this.StockInOutProductHistoryDtoGridControl.Size = new System.Drawing.Size(1032, 347);
            this.StockInOutProductHistoryDtoGridControl.TabIndex = 4;
            this.StockInOutProductHistoryDtoGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.StockInOutProductHistoryDtoGridView});
            // 
            // stockInOutProductHistoryDtoBindingSource
            // 
            this.stockInOutProductHistoryDtoBindingSource.DataSource = typeof(DTO.Inventory.InventoryManagement.StockInOutProductHistoryDto);
            // 
            // StockInOutProductHistoryDtoGridView
            // 
            this.StockInOutProductHistoryDtoGridView.Appearance.ViewCaption.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.StockInOutProductHistoryDtoGridView.Appearance.ViewCaption.ForeColor = DevExpress.LookAndFeel.DXSkinColors.ForeColors.Information;
            this.StockInOutProductHistoryDtoGridView.Appearance.ViewCaption.Options.UseFont = true;
            this.StockInOutProductHistoryDtoGridView.Appearance.ViewCaption.Options.UseForeColor = true;
            this.StockInOutProductHistoryDtoGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colStockInOutDate,
            this.colWarehouseNameHtml,
            this.colCustomerNameHtml,
            this.colProductVariantFullNameHtml,
            this.colStockInQty,
            this.colStockOutQty,
            this.colUnitPrice,
            this.colVatAmount,
            this.colTotalAmount,
            this.colTotalAmountIncludedVat});
            this.StockInOutProductHistoryDtoGridView.GridControl = this.StockInOutProductHistoryDtoGridControl;
            this.StockInOutProductHistoryDtoGridView.IndicatorWidth = 50;
            this.StockInOutProductHistoryDtoGridView.Name = "StockInOutProductHistoryDtoGridView";
            this.StockInOutProductHistoryDtoGridView.OptionsFind.AlwaysVisible = true;
            this.StockInOutProductHistoryDtoGridView.OptionsSelection.MultiSelect = true;
            this.StockInOutProductHistoryDtoGridView.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            this.StockInOutProductHistoryDtoGridView.OptionsView.RowAutoHeight = true;
            this.StockInOutProductHistoryDtoGridView.OptionsView.ShowFooter = true;
            this.StockInOutProductHistoryDtoGridView.OptionsView.ShowGroupPanel = false;
            this.StockInOutProductHistoryDtoGridView.OptionsView.ShowViewCaption = true;
            this.StockInOutProductHistoryDtoGridView.ViewCaption = "LỊCH SỬ SẢN PHẨM NHẬP XUẤT KHO";
            // 
            // colStockInOutDate
            // 
            this.colStockInOutDate.AppearanceCell.Options.UseTextOptions = true;
            this.colStockInOutDate.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colStockInOutDate.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colStockInOutDate.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colStockInOutDate.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colStockInOutDate.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colStockInOutDate.AppearanceHeader.Options.UseBackColor = true;
            this.colStockInOutDate.AppearanceHeader.Options.UseFont = true;
            this.colStockInOutDate.AppearanceHeader.Options.UseForeColor = true;
            this.colStockInOutDate.AppearanceHeader.Options.UseTextOptions = true;
            this.colStockInOutDate.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colStockInOutDate.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colStockInOutDate.Caption = "Ngày nhập xuất";
            this.colStockInOutDate.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.colStockInOutDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.colStockInOutDate.FieldName = "StockInOutDate";
            this.colStockInOutDate.Name = "colStockInOutDate";
            this.colStockInOutDate.OptionsColumn.AllowEdit = false;
            this.colStockInOutDate.OptionsColumn.ReadOnly = true;
            this.colStockInOutDate.Visible = true;
            this.colStockInOutDate.VisibleIndex = 1;
            this.colStockInOutDate.Width = 120;
            // 
            // colWarehouseNameHtml
            // 
            this.colWarehouseNameHtml.AppearanceCell.Options.UseTextOptions = true;
            this.colWarehouseNameHtml.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colWarehouseNameHtml.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colWarehouseNameHtml.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colWarehouseNameHtml.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colWarehouseNameHtml.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colWarehouseNameHtml.AppearanceHeader.Options.UseBackColor = true;
            this.colWarehouseNameHtml.AppearanceHeader.Options.UseFont = true;
            this.colWarehouseNameHtml.AppearanceHeader.Options.UseForeColor = true;
            this.colWarehouseNameHtml.AppearanceHeader.Options.UseTextOptions = true;
            this.colWarehouseNameHtml.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colWarehouseNameHtml.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colWarehouseNameHtml.Caption = "Kho";
            this.colWarehouseNameHtml.ColumnEdit = this.HtmlRepositoryItemHypertextLabel;
            this.colWarehouseNameHtml.FieldName = "WarehouseNameHtml";
            this.colWarehouseNameHtml.Name = "colWarehouseNameHtml";
            this.colWarehouseNameHtml.OptionsColumn.AllowEdit = false;
            this.colWarehouseNameHtml.OptionsColumn.ReadOnly = true;
            this.colWarehouseNameHtml.Visible = true;
            this.colWarehouseNameHtml.VisibleIndex = 2;
            this.colWarehouseNameHtml.Width = 150;
            // 
            // HtmlRepositoryItemHypertextLabel
            // 
            this.HtmlRepositoryItemHypertextLabel.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.HtmlRepositoryItemHypertextLabel.Name = "HtmlRepositoryItemHypertextLabel";
            // 
            // colCustomerNameHtml
            // 
            this.colCustomerNameHtml.AppearanceCell.Options.UseTextOptions = true;
            this.colCustomerNameHtml.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colCustomerNameHtml.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colCustomerNameHtml.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colCustomerNameHtml.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colCustomerNameHtml.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colCustomerNameHtml.AppearanceHeader.Options.UseBackColor = true;
            this.colCustomerNameHtml.AppearanceHeader.Options.UseFont = true;
            this.colCustomerNameHtml.AppearanceHeader.Options.UseForeColor = true;
            this.colCustomerNameHtml.AppearanceHeader.Options.UseTextOptions = true;
            this.colCustomerNameHtml.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colCustomerNameHtml.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colCustomerNameHtml.Caption = "Khách hàng - Nhà Cung cấp";
            this.colCustomerNameHtml.ColumnEdit = this.HtmlRepositoryItemHypertextLabel;
            this.colCustomerNameHtml.FieldName = "CustomerNameHtml";
            this.colCustomerNameHtml.Name = "colCustomerNameHtml";
            this.colCustomerNameHtml.OptionsColumn.AllowEdit = false;
            this.colCustomerNameHtml.OptionsColumn.ReadOnly = true;
            this.colCustomerNameHtml.Visible = true;
            this.colCustomerNameHtml.VisibleIndex = 3;
            this.colCustomerNameHtml.Width = 200;
            // 
            // colProductVariantFullNameHtml
            // 
            this.colProductVariantFullNameHtml.AppearanceCell.Options.UseTextOptions = true;
            this.colProductVariantFullNameHtml.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colProductVariantFullNameHtml.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colProductVariantFullNameHtml.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colProductVariantFullNameHtml.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colProductVariantFullNameHtml.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colProductVariantFullNameHtml.AppearanceHeader.Options.UseBackColor = true;
            this.colProductVariantFullNameHtml.AppearanceHeader.Options.UseFont = true;
            this.colProductVariantFullNameHtml.AppearanceHeader.Options.UseForeColor = true;
            this.colProductVariantFullNameHtml.AppearanceHeader.Options.UseTextOptions = true;
            this.colProductVariantFullNameHtml.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colProductVariantFullNameHtml.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colProductVariantFullNameHtml.Caption = "Sản phẩm";
            this.colProductVariantFullNameHtml.ColumnEdit = this.HtmlRepositoryItemHypertextLabel;
            this.colProductVariantFullNameHtml.FieldName = "ProductVariantFullName";
            this.colProductVariantFullNameHtml.Name = "colProductVariantFullNameHtml";
            this.colProductVariantFullNameHtml.OptionsColumn.AllowEdit = false;
            this.colProductVariantFullNameHtml.OptionsColumn.ReadOnly = true;
            this.colProductVariantFullNameHtml.Visible = true;
            this.colProductVariantFullNameHtml.VisibleIndex = 4;
            this.colProductVariantFullNameHtml.Width = 300;
            // 
            // colStockInQty
            // 
            this.colStockInQty.AppearanceCell.ForeColor = System.Drawing.Color.Green;
            this.colStockInQty.AppearanceCell.Options.UseForeColor = true;
            this.colStockInQty.AppearanceCell.Options.UseTextOptions = true;
            this.colStockInQty.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.colStockInQty.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colStockInQty.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colStockInQty.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colStockInQty.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colStockInQty.AppearanceHeader.Options.UseBackColor = true;
            this.colStockInQty.AppearanceHeader.Options.UseFont = true;
            this.colStockInQty.AppearanceHeader.Options.UseForeColor = true;
            this.colStockInQty.AppearanceHeader.Options.UseTextOptions = true;
            this.colStockInQty.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colStockInQty.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colStockInQty.Caption = "SL nhập";
            this.colStockInQty.DisplayFormat.FormatString = "N2";
            this.colStockInQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colStockInQty.FieldName = "StockInQty";
            this.colStockInQty.Name = "colStockInQty";
            this.colStockInQty.OptionsColumn.AllowEdit = false;
            this.colStockInQty.OptionsColumn.ReadOnly = true;
            this.colStockInQty.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "StockInQty", "Tổng: {0:N2}")});
            this.colStockInQty.Visible = true;
            this.colStockInQty.VisibleIndex = 5;
            this.colStockInQty.Width = 118;
            // 
            // colStockOutQty
            // 
            this.colStockOutQty.AppearanceCell.ForeColor = System.Drawing.Color.Red;
            this.colStockOutQty.AppearanceCell.Options.UseForeColor = true;
            this.colStockOutQty.AppearanceCell.Options.UseTextOptions = true;
            this.colStockOutQty.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.colStockOutQty.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colStockOutQty.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colStockOutQty.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colStockOutQty.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colStockOutQty.AppearanceHeader.Options.UseBackColor = true;
            this.colStockOutQty.AppearanceHeader.Options.UseFont = true;
            this.colStockOutQty.AppearanceHeader.Options.UseForeColor = true;
            this.colStockOutQty.AppearanceHeader.Options.UseTextOptions = true;
            this.colStockOutQty.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colStockOutQty.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colStockOutQty.Caption = "SL xuất";
            this.colStockOutQty.DisplayFormat.FormatString = "N2";
            this.colStockOutQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colStockOutQty.FieldName = "StockOutQty";
            this.colStockOutQty.Name = "colStockOutQty";
            this.colStockOutQty.OptionsColumn.AllowEdit = false;
            this.colStockOutQty.OptionsColumn.ReadOnly = true;
            this.colStockOutQty.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "StockOutQty", "Tổng: {0:N2}")});
            this.colStockOutQty.Visible = true;
            this.colStockOutQty.VisibleIndex = 6;
            this.colStockOutQty.Width = 118;
            // 
            // colUnitPrice
            // 
            this.colUnitPrice.AppearanceCell.Options.UseTextOptions = true;
            this.colUnitPrice.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.colUnitPrice.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colUnitPrice.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colUnitPrice.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colUnitPrice.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colUnitPrice.AppearanceHeader.Options.UseBackColor = true;
            this.colUnitPrice.AppearanceHeader.Options.UseFont = true;
            this.colUnitPrice.AppearanceHeader.Options.UseForeColor = true;
            this.colUnitPrice.AppearanceHeader.Options.UseTextOptions = true;
            this.colUnitPrice.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colUnitPrice.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colUnitPrice.Caption = "Đơn giá";
            this.colUnitPrice.DisplayFormat.FormatString = "N0";
            this.colUnitPrice.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colUnitPrice.FieldName = "UnitPrice";
            this.colUnitPrice.Name = "colUnitPrice";
            this.colUnitPrice.OptionsColumn.AllowEdit = false;
            this.colUnitPrice.OptionsColumn.ReadOnly = true;
            this.colUnitPrice.Visible = true;
            this.colUnitPrice.VisibleIndex = 7;
            this.colUnitPrice.Width = 127;
            // 
            // colVatAmount
            // 
            this.colVatAmount.AppearanceCell.Options.UseTextOptions = true;
            this.colVatAmount.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.colVatAmount.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colVatAmount.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colVatAmount.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colVatAmount.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colVatAmount.AppearanceHeader.Options.UseBackColor = true;
            this.colVatAmount.AppearanceHeader.Options.UseFont = true;
            this.colVatAmount.AppearanceHeader.Options.UseForeColor = true;
            this.colVatAmount.AppearanceHeader.Options.UseTextOptions = true;
            this.colVatAmount.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colVatAmount.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colVatAmount.Caption = "Tiền VAT";
            this.colVatAmount.DisplayFormat.FormatString = "N0";
            this.colVatAmount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colVatAmount.FieldName = "VatAmount";
            this.colVatAmount.Name = "colVatAmount";
            this.colVatAmount.OptionsColumn.AllowEdit = false;
            this.colVatAmount.OptionsColumn.ReadOnly = true;
            this.colVatAmount.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "VatAmount", "Tổng: {0:N0}")});
            this.colVatAmount.Visible = true;
            this.colVatAmount.VisibleIndex = 8;
            this.colVatAmount.Width = 127;
            // 
            // colTotalAmount
            // 
            this.colTotalAmount.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colTotalAmount.AppearanceCell.ForeColor = System.Drawing.Color.DarkGreen;
            this.colTotalAmount.AppearanceCell.Options.UseFont = true;
            this.colTotalAmount.AppearanceCell.Options.UseForeColor = true;
            this.colTotalAmount.AppearanceCell.Options.UseTextOptions = true;
            this.colTotalAmount.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.colTotalAmount.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colTotalAmount.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colTotalAmount.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colTotalAmount.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colTotalAmount.AppearanceHeader.Options.UseBackColor = true;
            this.colTotalAmount.AppearanceHeader.Options.UseFont = true;
            this.colTotalAmount.AppearanceHeader.Options.UseForeColor = true;
            this.colTotalAmount.AppearanceHeader.Options.UseTextOptions = true;
            this.colTotalAmount.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colTotalAmount.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colTotalAmount.Caption = "Thành tiền";
            this.colTotalAmount.DisplayFormat.FormatString = "N0";
            this.colTotalAmount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colTotalAmount.FieldName = "TotalAmount";
            this.colTotalAmount.Name = "colTotalAmount";
            this.colTotalAmount.OptionsColumn.AllowEdit = false;
            this.colTotalAmount.OptionsColumn.ReadOnly = true;
            this.colTotalAmount.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "TotalAmount", "Tổng: {0:N0}")});
            this.colTotalAmount.Visible = true;
            this.colTotalAmount.VisibleIndex = 9;
            this.colTotalAmount.Width = 137;
            // 
            // colTotalAmountIncludedVat
            // 
            this.colTotalAmountIncludedVat.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colTotalAmountIncludedVat.AppearanceCell.ForeColor = System.Drawing.Color.DarkRed;
            this.colTotalAmountIncludedVat.AppearanceCell.Options.UseFont = true;
            this.colTotalAmountIncludedVat.AppearanceCell.Options.UseForeColor = true;
            this.colTotalAmountIncludedVat.AppearanceCell.Options.UseTextOptions = true;
            this.colTotalAmountIncludedVat.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.colTotalAmountIncludedVat.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colTotalAmountIncludedVat.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colTotalAmountIncludedVat.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colTotalAmountIncludedVat.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colTotalAmountIncludedVat.AppearanceHeader.Options.UseBackColor = true;
            this.colTotalAmountIncludedVat.AppearanceHeader.Options.UseFont = true;
            this.colTotalAmountIncludedVat.AppearanceHeader.Options.UseForeColor = true;
            this.colTotalAmountIncludedVat.AppearanceHeader.Options.UseTextOptions = true;
            this.colTotalAmountIncludedVat.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colTotalAmountIncludedVat.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colTotalAmountIncludedVat.Caption = "Tổng tiền (gồm VAT)";
            this.colTotalAmountIncludedVat.DisplayFormat.FormatString = "N0";
            this.colTotalAmountIncludedVat.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colTotalAmountIncludedVat.FieldName = "TotalAmountIncludedVat";
            this.colTotalAmountIncludedVat.Name = "colTotalAmountIncludedVat";
            this.colTotalAmountIncludedVat.OptionsColumn.AllowEdit = false;
            this.colTotalAmountIncludedVat.OptionsColumn.ReadOnly = true;
            this.colTotalAmountIncludedVat.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "TotalAmountIncludedVat", "Tổng: {0:N0}")});
            this.colTotalAmountIncludedVat.Visible = true;
            this.colTotalAmountIncludedVat.VisibleIndex = 10;
            this.colTotalAmountIncludedVat.Width = 156;
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar2,
            this.bar1});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.InPhieuBarButtonItem,
            this.NhapBaoHanhBarButtonItem,
            this.ThemHinhAnhBarButtonItem,
            this.TuNgayBarEditItem,
            this.DenNgayBarEditItem,
            this.XemBaoCaoBarButtonItem,
            this.barHeaderItem1,
            this.DataSummaryBarStaticItem,
            this.SelectedRowBarStaticItem,
            this.ChiTietPhieuNhapXuatBarButtonItem,
            this.XoaPhieuBarButtonItem,
            this.KeyWordBarEditItem,
            this.NhapDinhDanhSPBarButtonItem});
            this.barManager1.MainMenu = this.bar2;
            this.barManager1.MaxItemId = 18;
            this.barManager1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemDateEdit1,
            this.repositoryItemDateEdit2,
            this.repositoryItemTextEdit1});
            this.barManager1.StatusBar = this.bar1;
            // 
            // bar2
            // 
            this.bar2.BarName = "Main menu";
            this.bar2.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Top;
            this.bar2.DockCol = 0;
            this.bar2.DockRow = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(((DevExpress.XtraBars.BarLinkUserDefines)((DevExpress.XtraBars.BarLinkUserDefines.PaintStyle | DevExpress.XtraBars.BarLinkUserDefines.Width))), this.KeyWordBarEditItem, "", false, true, true, 147, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(((DevExpress.XtraBars.BarLinkUserDefines)((DevExpress.XtraBars.BarLinkUserDefines.PaintStyle | DevExpress.XtraBars.BarLinkUserDefines.Width))), this.TuNgayBarEditItem, "", false, true, true, 117, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(((DevExpress.XtraBars.BarLinkUserDefines)((DevExpress.XtraBars.BarLinkUserDefines.PaintStyle | DevExpress.XtraBars.BarLinkUserDefines.Width))), this.DenNgayBarEditItem, "", false, true, true, 125, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.XemBaoCaoBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.ChiTietPhieuNhapXuatBarButtonItem, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar2.OptionsBar.MultiLine = true;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Main menu";
            // 
            // KeyWordBarEditItem
            // 
            this.KeyWordBarEditItem.Caption = "Từ khóa";
            this.KeyWordBarEditItem.Edit = this.repositoryItemTextEdit1;
            this.KeyWordBarEditItem.Id = 15;
            this.KeyWordBarEditItem.Name = "KeyWordBarEditItem";
            // 
            // repositoryItemTextEdit1
            // 
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // TuNgayBarEditItem
            // 
            this.TuNgayBarEditItem.Caption = "Từ ngày";
            this.TuNgayBarEditItem.Edit = this.repositoryItemDateEdit1;
            this.TuNgayBarEditItem.Id = 7;
            this.TuNgayBarEditItem.Name = "TuNgayBarEditItem";
            // 
            // repositoryItemDateEdit1
            // 
            this.repositoryItemDateEdit1.AutoHeight = false;
            this.repositoryItemDateEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit1.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit1.Name = "repositoryItemDateEdit1";
            // 
            // DenNgayBarEditItem
            // 
            this.DenNgayBarEditItem.Caption = "Đến ngày";
            this.DenNgayBarEditItem.Edit = this.repositoryItemDateEdit2;
            this.DenNgayBarEditItem.Id = 8;
            this.DenNgayBarEditItem.Name = "DenNgayBarEditItem";
            // 
            // repositoryItemDateEdit2
            // 
            this.repositoryItemDateEdit2.AutoHeight = false;
            this.repositoryItemDateEdit2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit2.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit2.Name = "repositoryItemDateEdit2";
            // 
            // XemBaoCaoBarButtonItem
            // 
            this.XemBaoCaoBarButtonItem.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True;
            this.XemBaoCaoBarButtonItem.Caption = "Xem";
            this.XemBaoCaoBarButtonItem.Id = 9;
            this.XemBaoCaoBarButtonItem.ImageOptions.Image = global::Inventory.Properties.Resources.filterbyseries_pie_16x16;
            this.XemBaoCaoBarButtonItem.ImageOptions.LargeImage = global::Inventory.Properties.Resources.filterbyseries_pie_32x32;
            this.XemBaoCaoBarButtonItem.Name = "XemBaoCaoBarButtonItem";
            // 
            // ChiTietPhieuNhapXuatBarButtonItem
            // 
            this.ChiTietPhieuNhapXuatBarButtonItem.Caption = "Chi tiết";
            this.ChiTietPhieuNhapXuatBarButtonItem.Id = 13;
            this.ChiTietPhieuNhapXuatBarButtonItem.ImageOptions.Image = global::Inventory.Properties.Resources.hideproduct_16x16;
            this.ChiTietPhieuNhapXuatBarButtonItem.ImageOptions.LargeImage = global::Inventory.Properties.Resources.hideproduct_32x32;
            this.ChiTietPhieuNhapXuatBarButtonItem.Name = "ChiTietPhieuNhapXuatBarButtonItem";
            // 
            // InPhieuBarButtonItem
            // 
            this.InPhieuBarButtonItem.Caption = "In phiếu";
            this.InPhieuBarButtonItem.Id = 2;
            this.InPhieuBarButtonItem.ImageOptions.Image = global::Inventory.Properties.Resources.print_16x16;
            this.InPhieuBarButtonItem.ImageOptions.LargeImage = global::Inventory.Properties.Resources.print_32x32;
            this.InPhieuBarButtonItem.Name = "InPhieuBarButtonItem";
            // 
            // NhapBaoHanhBarButtonItem
            // 
            this.NhapBaoHanhBarButtonItem.Caption = "Nhập bảo hành";
            this.NhapBaoHanhBarButtonItem.Id = 4;
            this.NhapBaoHanhBarButtonItem.ImageOptions.Image = global::Inventory.Properties.Resources.showtestreport_16x16;
            this.NhapBaoHanhBarButtonItem.ImageOptions.LargeImage = global::Inventory.Properties.Resources.showtestreport_32x32;
            this.NhapBaoHanhBarButtonItem.Name = "NhapBaoHanhBarButtonItem";
            // 
            // NhapDinhDanhSPBarButtonItem
            // 
            this.NhapDinhDanhSPBarButtonItem.Caption = "Nhập định danh";
            this.NhapDinhDanhSPBarButtonItem.Id = 17;
            this.NhapDinhDanhSPBarButtonItem.ImageOptions.Image = global::Inventory.Properties.Resources.barcode_16x16;
            this.NhapDinhDanhSPBarButtonItem.Name = "NhapDinhDanhSPBarButtonItem";
            // 
            // ThemHinhAnhBarButtonItem
            // 
            this.ThemHinhAnhBarButtonItem.Caption = "Thêm hình ảnh";
            this.ThemHinhAnhBarButtonItem.Id = 5;
            this.ThemHinhAnhBarButtonItem.ImageOptions.Image = global::Inventory.Properties.Resources.insertimage_16x16;
            this.ThemHinhAnhBarButtonItem.ImageOptions.LargeImage = global::Inventory.Properties.Resources.insertimage_32x32;
            this.ThemHinhAnhBarButtonItem.Name = "ThemHinhAnhBarButtonItem";
            // 
            // XoaPhieuBarButtonItem
            // 
            this.XoaPhieuBarButtonItem.Caption = "Xóa";
            this.XoaPhieuBarButtonItem.Id = 14;
            this.XoaPhieuBarButtonItem.ImageOptions.Image = global::Inventory.Properties.Resources.clear_16x16;
            this.XoaPhieuBarButtonItem.Name = "XoaPhieuBarButtonItem";
            // 
            // bar1
            // 
            this.bar1.BarName = "Custom 3";
            this.bar1.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barHeaderItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.DataSummaryBarStaticItem),
            new DevExpress.XtraBars.LinkPersistInfo(this.SelectedRowBarStaticItem)});
            this.bar1.OptionsBar.AllowQuickCustomization = false;
            this.bar1.OptionsBar.DrawDragBorder = false;
            this.bar1.OptionsBar.UseWholeRow = true;
            this.bar1.Text = "Custom 3";
            // 
            // barHeaderItem1
            // 
            this.barHeaderItem1.Caption = "Tổng kết";
            this.barHeaderItem1.Id = 10;
            this.barHeaderItem1.Name = "barHeaderItem1";
            // 
            // DataSummaryBarStaticItem
            // 
            this.DataSummaryBarStaticItem.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True;
            this.DataSummaryBarStaticItem.Caption = "Chưa có dữ liệu";
            this.DataSummaryBarStaticItem.Id = 11;
            this.DataSummaryBarStaticItem.Name = "DataSummaryBarStaticItem";
            // 
            // SelectedRowBarStaticItem
            // 
            this.SelectedRowBarStaticItem.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True;
            this.SelectedRowBarStaticItem.Caption = "Chưa chọn dòng nào";
            this.SelectedRowBarStaticItem.Id = 12;
            this.SelectedRowBarStaticItem.Name = "SelectedRowBarStaticItem";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager1;
            this.barDockControlTop.Size = new System.Drawing.Size(1056, 24);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 395);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(1056, 22);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 24);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 371);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1056, 24);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 371);
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(1056, 371);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.StockInOutProductHistoryDtoGridControl;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(1036, 351);
            this.layoutControlItem1.TextVisible = false;
            // 
            // FrmStockInOutProductHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1056, 417);
            this.Controls.Add(this.layoutControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "FrmStockInOutProductHistory";
            this.Text = "LỊCH SỬ NHẬP XUẤT KHO";
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.StockInOutProductHistoryDtoGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stockInOutProductHistoryDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StockInOutProductHistoryDtoGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HtmlRepositoryItemHypertextLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit2.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraBars.BarButtonItem InPhieuBarButtonItem;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem NhapBaoHanhBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem ThemHinhAnhBarButtonItem;
        private DevExpress.XtraGrid.GridControl StockInOutProductHistoryDtoGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView StockInOutProductHistoryDtoGridView;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraBars.BarEditItem TuNgayBarEditItem;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEdit1;
        private DevExpress.XtraBars.BarEditItem DenNgayBarEditItem;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEdit2;
        private DevExpress.XtraBars.BarButtonItem XemBaoCaoBarButtonItem;
        private DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel HtmlRepositoryItemHypertextLabel;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarHeaderItem barHeaderItem1;
        private DevExpress.XtraBars.BarStaticItem DataSummaryBarStaticItem;
        private DevExpress.XtraBars.BarStaticItem SelectedRowBarStaticItem;
        private DevExpress.XtraBars.BarButtonItem ChiTietPhieuNhapXuatBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem XoaPhieuBarButtonItem;
        private DevExpress.XtraBars.BarEditItem KeyWordBarEditItem;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn colStockInOutDate;
        private DevExpress.XtraGrid.Columns.GridColumn colWarehouseNameHtml;
        private DevExpress.XtraGrid.Columns.GridColumn colCustomerNameHtml;
        private DevExpress.XtraGrid.Columns.GridColumn colProductVariantFullNameHtml;
        private DevExpress.XtraGrid.Columns.GridColumn colStockInQty;
        private DevExpress.XtraGrid.Columns.GridColumn colStockOutQty;
        private DevExpress.XtraGrid.Columns.GridColumn colUnitPrice;
        private DevExpress.XtraGrid.Columns.GridColumn colVatAmount;
        private DevExpress.XtraGrid.Columns.GridColumn colTotalAmount;
        private DevExpress.XtraGrid.Columns.GridColumn colTotalAmountIncludedVat;
        private DevExpress.XtraBars.BarButtonItem NhapDinhDanhSPBarButtonItem;
        private System.Windows.Forms.BindingSource stockInOutProductHistoryDtoBindingSource;
    }
}