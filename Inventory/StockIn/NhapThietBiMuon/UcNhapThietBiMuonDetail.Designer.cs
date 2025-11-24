namespace Inventory.StockIn.NhapThietBiMuon
{
    partial class UcNhapThietBiMuonDetail
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


        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.StockInDetailDtoGridControl = new DevExpress.XtraGrid.GridControl();
            this.stockInDetailDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.StockInDetailDtoGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colProductVariantCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ProductVariantSearchLookUpEdit = new DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit();
            this.productVariantListDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ProductVariantDtoSearchLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colFullName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ProductFullNameHypertextLabel = new DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel();
            this.colProductVariantName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.StockInDetailProductNameHtmlHypertextLabel = new DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel();
            this.colUnitOfMeasureName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colStockInQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.StockInQtyTextEdit = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.colUnitPrice = new DevExpress.XtraGrid.Columns.GridColumn();
            this.UnitPriceTextEdit = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.colVat = new DevExpress.XtraGrid.Columns.GridColumn();
            this.VatTextEdit = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.colVatAmount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTotalAmount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTotalAmountIncludedVat = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.StockInDetailDtoGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stockInDetailDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StockInDetailDtoGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductVariantSearchLookUpEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.productVariantListDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductVariantDtoSearchLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductFullNameHypertextLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StockInDetailProductNameHtmlHypertextLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StockInQtyTextEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UnitPriceTextEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VatTextEdit)).BeginInit();
            this.SuspendLayout();
            // 
            // StockInDetailDtoGridControl
            // 
            this.StockInDetailDtoGridControl.DataSource = this.stockInDetailDtoBindingSource;
            this.StockInDetailDtoGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StockInDetailDtoGridControl.Location = new System.Drawing.Point(0, 0);
            this.StockInDetailDtoGridControl.MainView = this.StockInDetailDtoGridView;
            this.StockInDetailDtoGridControl.Name = "StockInDetailDtoGridControl";
            this.StockInDetailDtoGridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.ProductVariantSearchLookUpEdit,
            this.StockInDetailProductNameHtmlHypertextLabel,
            this.StockInQtyTextEdit,
            this.UnitPriceTextEdit,
            this.VatTextEdit});
            this.StockInDetailDtoGridControl.Size = new System.Drawing.Size(1267, 704);
            this.StockInDetailDtoGridControl.TabIndex = 0;
            this.StockInDetailDtoGridControl.UseEmbeddedNavigator = true;
            this.StockInDetailDtoGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.StockInDetailDtoGridView});
            // 
            // stockInDetailDtoBindingSource
            // 
            this.stockInDetailDtoBindingSource.DataSource = typeof(DTO.Inventory.StockIn.StockInDetailDto);
            // 
            // StockInDetailDtoGridView
            // 
            this.StockInDetailDtoGridView.Appearance.ViewCaption.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.StockInDetailDtoGridView.Appearance.ViewCaption.ForeColor = DevExpress.LookAndFeel.DXSkinColors.ForeColors.Information;
            this.StockInDetailDtoGridView.Appearance.ViewCaption.Options.UseFont = true;
            this.StockInDetailDtoGridView.Appearance.ViewCaption.Options.UseForeColor = true;
            this.StockInDetailDtoGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colProductVariantCode,
            this.colProductVariantName,
            this.colUnitOfMeasureName,
            this.colStockInQty,
            this.colUnitPrice,
            this.colVat,
            this.colVatAmount,
            this.colTotalAmount,
            this.colTotalAmountIncludedVat});
            this.StockInDetailDtoGridView.GridControl = this.StockInDetailDtoGridControl;
            this.StockInDetailDtoGridView.IndicatorWidth = 50;
            this.StockInDetailDtoGridView.Name = "StockInDetailDtoGridView";
            this.StockInDetailDtoGridView.OptionsNavigation.AutoFocusNewRow = true;
            this.StockInDetailDtoGridView.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top;
            this.StockInDetailDtoGridView.OptionsView.RowAutoHeight = true;
            this.StockInDetailDtoGridView.OptionsView.ShowFooter = true;
            this.StockInDetailDtoGridView.OptionsView.ShowGroupPanel = false;
            this.StockInDetailDtoGridView.OptionsView.ShowViewCaption = true;
            this.StockInDetailDtoGridView.ViewCaption = "DANH SÁCH HÀNG HÓA DỊCH VỤ NHẬP KHO";
            // 
            // colProductVariantCode
            // 
            this.colProductVariantCode.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colProductVariantCode.AppearanceCell.ForeColor = System.Drawing.Color.DarkBlue;
            this.colProductVariantCode.AppearanceCell.Options.UseFont = true;
            this.colProductVariantCode.AppearanceCell.Options.UseForeColor = true;
            this.colProductVariantCode.AppearanceCell.Options.UseTextOptions = true;
            this.colProductVariantCode.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colProductVariantCode.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colProductVariantCode.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colProductVariantCode.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colProductVariantCode.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colProductVariantCode.AppearanceHeader.Options.UseBackColor = true;
            this.colProductVariantCode.AppearanceHeader.Options.UseFont = true;
            this.colProductVariantCode.AppearanceHeader.Options.UseForeColor = true;
            this.colProductVariantCode.AppearanceHeader.Options.UseTextOptions = true;
            this.colProductVariantCode.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colProductVariantCode.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colProductVariantCode.Caption = "Mã hàng";
            this.colProductVariantCode.ColumnEdit = this.ProductVariantSearchLookUpEdit;
            this.colProductVariantCode.FieldName = "ProductVariantId";
            this.colProductVariantCode.Name = "colProductVariantCode";
            this.colProductVariantCode.Visible = true;
            this.colProductVariantCode.VisibleIndex = 0;
            this.colProductVariantCode.Width = 120;
            // 
            // ProductVariantSearchLookUpEdit
            // 
            this.ProductVariantSearchLookUpEdit.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.ProductVariantSearchLookUpEdit.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ProductVariantSearchLookUpEdit.DataSource = this.productVariantListDtoBindingSource;
            this.ProductVariantSearchLookUpEdit.DisplayMember = "VariantCode";
            this.ProductVariantSearchLookUpEdit.KeyMember = "Id";
            this.ProductVariantSearchLookUpEdit.Name = "ProductVariantSearchLookUpEdit";
            this.ProductVariantSearchLookUpEdit.NullText = "";
            this.ProductVariantSearchLookUpEdit.PopupView = this.ProductVariantDtoSearchLookUpEdit1View;
            this.ProductVariantSearchLookUpEdit.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.ProductFullNameHypertextLabel});
            this.ProductVariantSearchLookUpEdit.ValueMember = "Id";
            // 
            // productVariantListDtoBindingSource
            // 
            this.productVariantListDtoBindingSource.DataSource = typeof(DTO.MasterData.ProductService.ProductVariantListDto);
            // 
            // ProductVariantDtoSearchLookUpEdit1View
            // 
            this.ProductVariantDtoSearchLookUpEdit1View.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colFullName});
            this.ProductVariantDtoSearchLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.ProductVariantDtoSearchLookUpEdit1View.Name = "ProductVariantDtoSearchLookUpEdit1View";
            this.ProductVariantDtoSearchLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.ProductVariantDtoSearchLookUpEdit1View.OptionsView.RowAutoHeight = true;
            this.ProductVariantDtoSearchLookUpEdit1View.OptionsView.ShowAutoFilterRow = true;
            this.ProductVariantDtoSearchLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            this.ProductVariantDtoSearchLookUpEdit1View.OptionsView.ShowIndicator = false;
            // 
            // colFullName
            // 
            this.colFullName.AppearanceCell.Options.UseTextOptions = true;
            this.colFullName.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colFullName.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colFullName.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colFullName.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colFullName.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colFullName.AppearanceHeader.Options.UseBackColor = true;
            this.colFullName.AppearanceHeader.Options.UseFont = true;
            this.colFullName.AppearanceHeader.Options.UseForeColor = true;
            this.colFullName.AppearanceHeader.Options.UseTextOptions = true;
            this.colFullName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colFullName.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colFullName.Caption = "Thông tin sản phẩm";
            this.colFullName.ColumnEdit = this.ProductFullNameHypertextLabel;
            this.colFullName.FieldName = "FullNameHtml";
            this.colFullName.Name = "colFullName";
            this.colFullName.Visible = true;
            this.colFullName.VisibleIndex = 0;
            this.colFullName.Width = 450;
            // 
            // ProductFullNameHypertextLabel
            // 
            this.ProductFullNameHypertextLabel.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.ProductFullNameHypertextLabel.Name = "ProductFullNameHypertextLabel";
            // 
            // colProductVariantName
            // 
            this.colProductVariantName.AppearanceCell.Options.UseTextOptions = true;
            this.colProductVariantName.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colProductVariantName.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colProductVariantName.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colProductVariantName.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colProductVariantName.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colProductVariantName.AppearanceHeader.Options.UseBackColor = true;
            this.colProductVariantName.AppearanceHeader.Options.UseFont = true;
            this.colProductVariantName.AppearanceHeader.Options.UseForeColor = true;
            this.colProductVariantName.AppearanceHeader.Options.UseTextOptions = true;
            this.colProductVariantName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colProductVariantName.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colProductVariantName.Caption = "Tên hàng hóa";
            this.colProductVariantName.ColumnEdit = this.StockInDetailProductNameHtmlHypertextLabel;
            this.colProductVariantName.FieldName = "ProductVariantName";
            this.colProductVariantName.Name = "colProductVariantName";
            this.colProductVariantName.OptionsColumn.AllowEdit = false;
            this.colProductVariantName.OptionsColumn.ReadOnly = true;
            this.colProductVariantName.Visible = true;
            this.colProductVariantName.VisibleIndex = 1;
            this.colProductVariantName.Width = 245;
            // 
            // StockInDetailProductNameHtmlHypertextLabel
            // 
            this.StockInDetailProductNameHtmlHypertextLabel.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.StockInDetailProductNameHtmlHypertextLabel.Name = "StockInDetailProductNameHtmlHypertextLabel";
            // 
            // colUnitOfMeasureName
            // 
            this.colUnitOfMeasureName.AppearanceCell.Options.UseTextOptions = true;
            this.colUnitOfMeasureName.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colUnitOfMeasureName.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colUnitOfMeasureName.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colUnitOfMeasureName.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colUnitOfMeasureName.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colUnitOfMeasureName.AppearanceHeader.Options.UseBackColor = true;
            this.colUnitOfMeasureName.AppearanceHeader.Options.UseFont = true;
            this.colUnitOfMeasureName.AppearanceHeader.Options.UseForeColor = true;
            this.colUnitOfMeasureName.AppearanceHeader.Options.UseTextOptions = true;
            this.colUnitOfMeasureName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colUnitOfMeasureName.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colUnitOfMeasureName.Caption = "Đơn vị tính";
            this.colUnitOfMeasureName.FieldName = "UnitOfMeasureName";
            this.colUnitOfMeasureName.Name = "colUnitOfMeasureName";
            this.colUnitOfMeasureName.OptionsColumn.AllowEdit = false;
            this.colUnitOfMeasureName.OptionsColumn.ReadOnly = true;
            this.colUnitOfMeasureName.Visible = true;
            this.colUnitOfMeasureName.VisibleIndex = 2;
            this.colUnitOfMeasureName.Width = 97;
            // 
            // colStockInQty
            // 
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
            this.colStockInQty.Caption = "Số lượng nhập";
            this.colStockInQty.ColumnEdit = this.StockInQtyTextEdit;
            this.colStockInQty.DisplayFormat.FormatString = "N2";
            this.colStockInQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colStockInQty.FieldName = "StockInQty";
            this.colStockInQty.Name = "colStockInQty";
            this.colStockInQty.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "StockInQty", "Tổng: {0:N2}")});
            this.colStockInQty.Visible = true;
            this.colStockInQty.VisibleIndex = 3;
            this.colStockInQty.Width = 118;
            // 
            // StockInQtyTextEdit
            // 
            this.StockInQtyTextEdit.AutoHeight = false;
            this.StockInQtyTextEdit.DisplayFormat.FormatString = "N2";
            this.StockInQtyTextEdit.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.StockInQtyTextEdit.EditFormat.FormatString = "N2";
            this.StockInQtyTextEdit.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.StockInQtyTextEdit.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.StockInQtyTextEdit.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.NumericMaskManager));
            this.StockInQtyTextEdit.Name = "StockInQtyTextEdit";
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
            this.colUnitPrice.ColumnEdit = this.UnitPriceTextEdit;
            this.colUnitPrice.DisplayFormat.FormatString = "N0";
            this.colUnitPrice.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colUnitPrice.FieldName = "UnitPrice";
            this.colUnitPrice.Name = "colUnitPrice";
            this.colUnitPrice.Visible = true;
            this.colUnitPrice.VisibleIndex = 4;
            this.colUnitPrice.Width = 127;
            // 
            // UnitPriceTextEdit
            // 
            this.UnitPriceTextEdit.AutoHeight = false;
            this.UnitPriceTextEdit.DisplayFormat.FormatString = "N0";
            this.UnitPriceTextEdit.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.UnitPriceTextEdit.EditFormat.FormatString = "N0";
            this.UnitPriceTextEdit.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.UnitPriceTextEdit.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.UnitPriceTextEdit.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.NumericMaskManager));
            this.UnitPriceTextEdit.Name = "UnitPriceTextEdit";
            // 
            // colVat
            // 
            this.colVat.AppearanceCell.Options.UseTextOptions = true;
            this.colVat.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colVat.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colVat.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colVat.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colVat.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colVat.AppearanceHeader.Options.UseBackColor = true;
            this.colVat.AppearanceHeader.Options.UseFont = true;
            this.colVat.AppearanceHeader.Options.UseForeColor = true;
            this.colVat.AppearanceHeader.Options.UseTextOptions = true;
            this.colVat.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colVat.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colVat.Caption = "VAT (%)";
            this.colVat.ColumnEdit = this.VatTextEdit;
            this.colVat.DisplayFormat.FormatString = "N2";
            this.colVat.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colVat.FieldName = "Vat";
            this.colVat.Name = "colVat";
            this.colVat.Visible = true;
            this.colVat.VisibleIndex = 5;
            this.colVat.Width = 88;
            // 
            // VatTextEdit
            // 
            this.VatTextEdit.AutoHeight = false;
            this.VatTextEdit.DisplayFormat.FormatString = "N2";
            this.VatTextEdit.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.VatTextEdit.EditFormat.FormatString = "N2";
            this.VatTextEdit.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.VatTextEdit.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.VatTextEdit.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.NumericMaskManager));
            this.VatTextEdit.Name = "VatTextEdit";
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
            this.colVatAmount.VisibleIndex = 6;
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
            this.colTotalAmount.VisibleIndex = 7;
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
            this.colTotalAmountIncludedVat.VisibleIndex = 8;
            this.colTotalAmountIncludedVat.Width = 156;
            // 
            // UcStockInDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.StockInDetailDtoGridControl);
            this.Name = "UcStockInDetail";
            this.Size = new System.Drawing.Size(1267, 704);
            ((System.ComponentModel.ISupportInitialize)(this.StockInDetailDtoGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stockInDetailDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StockInDetailDtoGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductVariantSearchLookUpEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.productVariantListDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductVariantDtoSearchLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductFullNameHypertextLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StockInDetailProductNameHtmlHypertextLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StockInQtyTextEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UnitPriceTextEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VatTextEdit)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl StockInDetailDtoGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView StockInDetailDtoGridView;

        private System.Windows.Forms.BindingSource stockInDetailDtoBindingSource;
        private DevExpress.XtraGrid.Columns.GridColumn colProductVariantCode;
        private DevExpress.XtraGrid.Columns.GridColumn colProductVariantName;
        private DevExpress.XtraGrid.Columns.GridColumn colUnitOfMeasureName;
        private DevExpress.XtraGrid.Columns.GridColumn colStockInQty;
        private DevExpress.XtraGrid.Columns.GridColumn colUnitPrice;
        private DevExpress.XtraGrid.Columns.GridColumn colVat;
        private DevExpress.XtraGrid.Columns.GridColumn colVatAmount;
        private DevExpress.XtraGrid.Columns.GridColumn colTotalAmount;
        private DevExpress.XtraGrid.Columns.GridColumn colTotalAmountIncludedVat;
        private DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit ProductVariantSearchLookUpEdit;
        private DevExpress.XtraGrid.Views.Grid.GridView ProductVariantDtoSearchLookUpEdit1View;
        private DevExpress.XtraGrid.Columns.GridColumn colFullName;
        private DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel ProductFullNameHypertextLabel;

        private System.Windows.Forms.BindingSource productVariantListDtoBindingSource;
        private DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel StockInDetailProductNameHtmlHypertextLabel;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit StockInQtyTextEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit UnitPriceTextEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit VatTextEdit;
    }
}
