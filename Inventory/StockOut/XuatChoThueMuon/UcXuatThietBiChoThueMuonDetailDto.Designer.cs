namespace Inventory.StockOut.XuatChoThueMuon
{
    partial class UcXuatThietBiChoThueMuonDetailDto
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
            this.XuatThietBiChoThueMuonDetailDtoGridControl = new DevExpress.XtraGrid.GridControl();
            this.XuatThietBiChoThueMuonDetailDtoGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colProductVariantCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ProductVariantSearchLookUpEdit = new DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit();
            this.productVariantListDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ProductVariantDtoSearchLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colFullName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ProductFullNameHypertextLabel = new DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel();
            this.colProductVariantName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.StockInDetailProductNameHtmlHypertextLabel = new DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel();
            this.colUnitOfMeasureName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colStockOutQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.StockOutQtyTextEdit = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.colGhiChu = new DevExpress.XtraGrid.Columns.GridColumn();
            this.GhiChuMemoEdit = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
            this.UnitPriceTextEdit = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.VatTextEdit = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.stockInOutDetailForUIDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.XuatThietBiChoThueMuonDetailDtoGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.XuatThietBiChoThueMuonDetailDtoGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductVariantSearchLookUpEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.productVariantListDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductVariantDtoSearchLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductFullNameHypertextLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StockInDetailProductNameHtmlHypertextLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StockOutQtyTextEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GhiChuMemoEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UnitPriceTextEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VatTextEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stockInOutDetailForUIDtoBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // XuatThietBiChoThueMuonDetailDtoGridControl
            // 
            this.XuatThietBiChoThueMuonDetailDtoGridControl.DataSource = this.stockInOutDetailForUIDtoBindingSource;
            this.XuatThietBiChoThueMuonDetailDtoGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.XuatThietBiChoThueMuonDetailDtoGridControl.Location = new System.Drawing.Point(0, 0);
            this.XuatThietBiChoThueMuonDetailDtoGridControl.MainView = this.XuatThietBiChoThueMuonDetailDtoGridView;
            this.XuatThietBiChoThueMuonDetailDtoGridControl.Name = "XuatThietBiChoThueMuonDetailDtoGridControl";
            this.XuatThietBiChoThueMuonDetailDtoGridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.ProductVariantSearchLookUpEdit,
            this.StockInDetailProductNameHtmlHypertextLabel,
            this.StockOutQtyTextEdit,
            this.UnitPriceTextEdit,
            this.VatTextEdit,
            this.GhiChuMemoEdit});
            this.XuatThietBiChoThueMuonDetailDtoGridControl.Size = new System.Drawing.Size(1267, 704);
            this.XuatThietBiChoThueMuonDetailDtoGridControl.TabIndex = 0;
            this.XuatThietBiChoThueMuonDetailDtoGridControl.UseEmbeddedNavigator = true;
            this.XuatThietBiChoThueMuonDetailDtoGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.XuatThietBiChoThueMuonDetailDtoGridView});
            // 
            // XuatThietBiChoThueMuonDetailDtoGridView
            // 
            this.XuatThietBiChoThueMuonDetailDtoGridView.Appearance.ViewCaption.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.XuatThietBiChoThueMuonDetailDtoGridView.Appearance.ViewCaption.ForeColor = DevExpress.LookAndFeel.DXSkinColors.ForeColors.Information;
            this.XuatThietBiChoThueMuonDetailDtoGridView.Appearance.ViewCaption.Options.UseFont = true;
            this.XuatThietBiChoThueMuonDetailDtoGridView.Appearance.ViewCaption.Options.UseForeColor = true;
            this.XuatThietBiChoThueMuonDetailDtoGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colProductVariantCode,
            this.colProductVariantName,
            this.colUnitOfMeasureName,
            this.colStockOutQty,
            this.colGhiChu});
            this.XuatThietBiChoThueMuonDetailDtoGridView.GridControl = this.XuatThietBiChoThueMuonDetailDtoGridControl;
            this.XuatThietBiChoThueMuonDetailDtoGridView.IndicatorWidth = 50;
            this.XuatThietBiChoThueMuonDetailDtoGridView.Name = "XuatThietBiChoThueMuonDetailDtoGridView";
            this.XuatThietBiChoThueMuonDetailDtoGridView.OptionsNavigation.AutoFocusNewRow = true;
            this.XuatThietBiChoThueMuonDetailDtoGridView.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top;
            this.XuatThietBiChoThueMuonDetailDtoGridView.OptionsView.RowAutoHeight = true;
            this.XuatThietBiChoThueMuonDetailDtoGridView.OptionsView.ShowFooter = true;
            this.XuatThietBiChoThueMuonDetailDtoGridView.OptionsView.ShowGroupPanel = false;
            this.XuatThietBiChoThueMuonDetailDtoGridView.OptionsView.ShowViewCaption = true;
            this.XuatThietBiChoThueMuonDetailDtoGridView.ViewCaption = "DANH SÁCH HÀNG HÓA DỊCH VỤ XUẤT KHO";
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
            // colStockOutQty
            // 
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
            this.colStockOutQty.Caption = "Số lượng xuất";
            this.colStockOutQty.ColumnEdit = this.StockOutQtyTextEdit;
            this.colStockOutQty.DisplayFormat.FormatString = "N2";
            this.colStockOutQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colStockOutQty.FieldName = "StockOutQty";
            this.colStockOutQty.Name = "colStockOutQty";
            this.colStockOutQty.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "StockOutQty", "Tổng: {0:N2}")});
            this.colStockOutQty.Visible = true;
            this.colStockOutQty.VisibleIndex = 3;
            this.colStockOutQty.Width = 118;
            // 
            // StockOutQtyTextEdit
            // 
            this.StockOutQtyTextEdit.AutoHeight = false;
            this.StockOutQtyTextEdit.DisplayFormat.FormatString = "N2";
            this.StockOutQtyTextEdit.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.StockOutQtyTextEdit.EditFormat.FormatString = "N2";
            this.StockOutQtyTextEdit.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.StockOutQtyTextEdit.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.NumericMaskManager));
            this.StockOutQtyTextEdit.Name = "StockOutQtyTextEdit";
            // 
            // colGhiChu
            // 
            this.colGhiChu.AppearanceCell.Options.UseTextOptions = true;
            this.colGhiChu.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colGhiChu.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colGhiChu.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colGhiChu.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colGhiChu.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colGhiChu.AppearanceHeader.Options.UseBackColor = true;
            this.colGhiChu.AppearanceHeader.Options.UseFont = true;
            this.colGhiChu.AppearanceHeader.Options.UseForeColor = true;
            this.colGhiChu.AppearanceHeader.Options.UseTextOptions = true;
            this.colGhiChu.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colGhiChu.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colGhiChu.ColumnEdit = this.GhiChuMemoEdit;
            this.colGhiChu.FieldName = "GhiChu";
            this.colGhiChu.Name = "colGhiChu";
            this.colGhiChu.Visible = true;
            this.colGhiChu.VisibleIndex = 4;
            // 
            // GhiChuMemoEdit
            // 
            this.GhiChuMemoEdit.Name = "GhiChuMemoEdit";
            // 
            // UnitPriceTextEdit
            // 
            this.UnitPriceTextEdit.AutoHeight = false;
            this.UnitPriceTextEdit.DisplayFormat.FormatString = "N0";
            this.UnitPriceTextEdit.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.UnitPriceTextEdit.EditFormat.FormatString = "N0";
            this.UnitPriceTextEdit.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.UnitPriceTextEdit.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.NumericMaskManager));
            this.UnitPriceTextEdit.Name = "UnitPriceTextEdit";
            // 
            // VatTextEdit
            // 
            this.VatTextEdit.AutoHeight = false;
            this.VatTextEdit.DisplayFormat.FormatString = "N2";
            this.VatTextEdit.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.VatTextEdit.EditFormat.FormatString = "N2";
            this.VatTextEdit.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.VatTextEdit.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.NumericMaskManager));
            this.VatTextEdit.Name = "VatTextEdit";
            // 
            // stockInOutDetailForUIDtoBindingSource
            // 
            this.stockInOutDetailForUIDtoBindingSource.DataSource = typeof(DTO.Inventory.StockInOutDetailForUIDto);
            // 
            // UcXuatThietBiChoThueMuonDetailDto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.XuatThietBiChoThueMuonDetailDtoGridControl);
            this.Name = "UcXuatThietBiChoThueMuonDetailDto";
            this.Size = new System.Drawing.Size(1267, 704);
            ((System.ComponentModel.ISupportInitialize)(this.XuatThietBiChoThueMuonDetailDtoGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.XuatThietBiChoThueMuonDetailDtoGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductVariantSearchLookUpEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.productVariantListDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductVariantDtoSearchLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductFullNameHypertextLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StockInDetailProductNameHtmlHypertextLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StockOutQtyTextEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GhiChuMemoEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UnitPriceTextEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VatTextEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stockInOutDetailForUIDtoBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl XuatThietBiChoThueMuonDetailDtoGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView XuatThietBiChoThueMuonDetailDtoGridView;
        private DevExpress.XtraGrid.Columns.GridColumn colProductVariantCode;
        private DevExpress.XtraGrid.Columns.GridColumn colProductVariantName;
        private DevExpress.XtraGrid.Columns.GridColumn colUnitOfMeasureName;
        private DevExpress.XtraGrid.Columns.GridColumn colStockOutQty;
        private DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit ProductVariantSearchLookUpEdit;
        private DevExpress.XtraGrid.Views.Grid.GridView ProductVariantDtoSearchLookUpEdit1View;
        private DevExpress.XtraGrid.Columns.GridColumn colFullName;
        private DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel ProductFullNameHypertextLabel;

        private System.Windows.Forms.BindingSource productVariantListDtoBindingSource;
        private DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel StockInDetailProductNameHtmlHypertextLabel;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit StockOutQtyTextEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit UnitPriceTextEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit VatTextEdit;
        private DevExpress.XtraGrid.Columns.GridColumn colGhiChu;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit GhiChuMemoEdit;
        private System.Windows.Forms.BindingSource stockInOutDetailForUIDtoBindingSource;
    }
}
