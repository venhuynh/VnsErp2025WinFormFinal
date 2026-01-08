namespace Inventory.OverlayForm
{
    partial class FrmGetIdentifierForStockOut
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
            this.AddHyperlinkLabelControl = new DevExpress.XtraEditors.HyperlinkLabelControl();
            this.RemoveHyperlinkLabelControl = new DevExpress.XtraEditors.HyperlinkLabelControl();
            this.StockInOutDetailForUIDtoGridControl = new DevExpress.XtraGrid.GridControl();
            this.stockInOutDetailForUIDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.StockInOutDetailForUIDtoGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colFullNameHtml = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ProductNameHypertextLabel = new DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel();
            this.colStockOutQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.StockOutQtyTextEdit = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.colGhiChu = new DevExpress.XtraGrid.Columns.GridColumn();
            this.NoteMemoEdit = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
            this.IdentifierValueTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.simpleLabelItem1 = new DevExpress.XtraLayout.SimpleLabelItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.FinishedHyperlinkLabelControl = new DevExpress.XtraEditors.HyperlinkLabelControl();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.StockInOutDetailForUIDtoGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stockInOutDetailForUIDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StockInOutDetailForUIDtoGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductNameHypertextLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StockOutQtyTextEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NoteMemoEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.IdentifierValueTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.FinishedHyperlinkLabelControl);
            this.layoutControl1.Controls.Add(this.AddHyperlinkLabelControl);
            this.layoutControl1.Controls.Add(this.RemoveHyperlinkLabelControl);
            this.layoutControl1.Controls.Add(this.StockInOutDetailForUIDtoGridControl);
            this.layoutControl1.Controls.Add(this.IdentifierValueTextEdit);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.Root;
            this.layoutControl1.Size = new System.Drawing.Size(860, 513);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // AddHyperlinkLabelControl
            // 
            this.AddHyperlinkLabelControl.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.AddHyperlinkLabelControl.ImageOptions.Image = global::Inventory.Properties.Resources.add_16x16;
            this.AddHyperlinkLabelControl.Location = new System.Drawing.Point(605, 71);
            this.AddHyperlinkLabelControl.Name = "AddHyperlinkLabelControl";
            this.AddHyperlinkLabelControl.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.AddHyperlinkLabelControl.Size = new System.Drawing.Size(88, 20);
            this.AddHyperlinkLabelControl.StyleController = this.layoutControl1;
            this.AddHyperlinkLabelControl.TabIndex = 7;
            this.AddHyperlinkLabelControl.Text = "Thêm vào";
            // 
            // RemoveHyperlinkLabelControl
            // 
            this.RemoveHyperlinkLabelControl.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.RemoveHyperlinkLabelControl.ImageOptions.Image = global::Inventory.Properties.Resources.clear_16x16;
            this.RemoveHyperlinkLabelControl.Location = new System.Drawing.Point(697, 71);
            this.RemoveHyperlinkLabelControl.Name = "RemoveHyperlinkLabelControl";
            this.RemoveHyperlinkLabelControl.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.RemoveHyperlinkLabelControl.Size = new System.Drawing.Size(66, 20);
            this.RemoveHyperlinkLabelControl.StyleController = this.layoutControl1;
            this.RemoveHyperlinkLabelControl.TabIndex = 6;
            this.RemoveHyperlinkLabelControl.Text = "Bỏ ra";
            // 
            // StockInOutDetailForUIDtoGridControl
            // 
            this.StockInOutDetailForUIDtoGridControl.DataSource = this.stockInOutDetailForUIDtoBindingSource;
            this.StockInOutDetailForUIDtoGridControl.Location = new System.Drawing.Point(12, 95);
            this.StockInOutDetailForUIDtoGridControl.MainView = this.StockInOutDetailForUIDtoGridView;
            this.StockInOutDetailForUIDtoGridControl.Name = "StockInOutDetailForUIDtoGridControl";
            this.StockInOutDetailForUIDtoGridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.ProductNameHypertextLabel,
            this.StockOutQtyTextEdit,
            this.NoteMemoEdit});
            this.StockInOutDetailForUIDtoGridControl.Size = new System.Drawing.Size(836, 406);
            this.StockInOutDetailForUIDtoGridControl.TabIndex = 5;
            this.StockInOutDetailForUIDtoGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.StockInOutDetailForUIDtoGridView});
            // 
            // stockInOutDetailForUIDtoBindingSource
            // 
            this.stockInOutDetailForUIDtoBindingSource.DataSource = typeof(DTO.Inventory.StockInOutDetailForUIDto);
            // 
            // StockInOutDetailForUIDtoGridView
            // 
            this.StockInOutDetailForUIDtoGridView.Appearance.ViewCaption.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.StockInOutDetailForUIDtoGridView.Appearance.ViewCaption.ForeColor = DevExpress.LookAndFeel.DXSkinColors.ForeColors.Information;
            this.StockInOutDetailForUIDtoGridView.Appearance.ViewCaption.Options.UseFont = true;
            this.StockInOutDetailForUIDtoGridView.Appearance.ViewCaption.Options.UseForeColor = true;
            this.StockInOutDetailForUIDtoGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colFullNameHtml,
            this.colStockOutQty,
            this.colGhiChu});
            this.StockInOutDetailForUIDtoGridView.GridControl = this.StockInOutDetailForUIDtoGridControl;
            this.StockInOutDetailForUIDtoGridView.IndicatorWidth = 50;
            this.StockInOutDetailForUIDtoGridView.Name = "StockInOutDetailForUIDtoGridView";
            this.StockInOutDetailForUIDtoGridView.OptionsSelection.MultiSelect = true;
            this.StockInOutDetailForUIDtoGridView.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            this.StockInOutDetailForUIDtoGridView.OptionsView.RowAutoHeight = true;
            this.StockInOutDetailForUIDtoGridView.OptionsView.ShowFooter = true;
            this.StockInOutDetailForUIDtoGridView.OptionsView.ShowGroupPanel = false;
            this.StockInOutDetailForUIDtoGridView.OptionsView.ShowViewCaption = true;
            this.StockInOutDetailForUIDtoGridView.ViewCaption = "DANH SÁCH HÀNG HÓA";
            // 
            // colFullNameHtml
            // 
            this.colFullNameHtml.AppearanceCell.Options.UseTextOptions = true;
            this.colFullNameHtml.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colFullNameHtml.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colFullNameHtml.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colFullNameHtml.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colFullNameHtml.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colFullNameHtml.AppearanceHeader.Options.UseBackColor = true;
            this.colFullNameHtml.AppearanceHeader.Options.UseFont = true;
            this.colFullNameHtml.AppearanceHeader.Options.UseForeColor = true;
            this.colFullNameHtml.AppearanceHeader.Options.UseTextOptions = true;
            this.colFullNameHtml.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colFullNameHtml.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colFullNameHtml.Caption = "Tên hàng hóa";
            this.colFullNameHtml.ColumnEdit = this.ProductNameHypertextLabel;
            this.colFullNameHtml.FieldName = "ProductVariantName";
            this.colFullNameHtml.Name = "colFullNameHtml";
            this.colFullNameHtml.Visible = true;
            this.colFullNameHtml.VisibleIndex = 1;
            this.colFullNameHtml.Width = 450;
            // 
            // ProductNameHypertextLabel
            // 
            this.ProductNameHypertextLabel.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.ProductNameHypertextLabel.Name = "ProductNameHypertextLabel";
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
            this.colStockOutQty.VisibleIndex = 2;
            this.colStockOutQty.Width = 120;
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
            this.colGhiChu.Caption = "Ghi chú";
            this.colGhiChu.ColumnEdit = this.NoteMemoEdit;
            this.colGhiChu.FieldName = "GhiChu";
            this.colGhiChu.Name = "colGhiChu";
            this.colGhiChu.Visible = true;
            this.colGhiChu.VisibleIndex = 3;
            this.colGhiChu.Width = 250;
            // 
            // NoteMemoEdit
            // 
            this.NoteMemoEdit.Name = "NoteMemoEdit";
            this.NoteMemoEdit.ScrollBars = System.Windows.Forms.ScrollBars.None;
            // 
            // IdentifierValueTextEdit
            // 
            this.IdentifierValueTextEdit.Location = new System.Drawing.Point(95, 71);
            this.IdentifierValueTextEdit.Name = "IdentifierValueTextEdit";
            this.IdentifierValueTextEdit.Size = new System.Drawing.Size(506, 20);
            this.IdentifierValueTextEdit.StyleController = this.layoutControl1;
            this.IdentifierValueTextEdit.TabIndex = 4;
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.simpleLabelItem1,
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.layoutControlItem4,
            this.layoutControlItem5});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(860, 513);
            this.Root.TextVisible = false;
            // 
            // simpleLabelItem1
            // 
            this.simpleLabelItem1.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.simpleLabelItem1.AppearanceItemCaption.ForeColor = DevExpress.LookAndFeel.DXSkinColors.ForeColors.Information;
            this.simpleLabelItem1.AppearanceItemCaption.Options.UseFont = true;
            this.simpleLabelItem1.AppearanceItemCaption.Options.UseForeColor = true;
            this.simpleLabelItem1.AppearanceItemCaption.Options.UseTextOptions = true;
            this.simpleLabelItem1.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.simpleLabelItem1.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.simpleLabelItem1.Location = new System.Drawing.Point(0, 0);
            this.simpleLabelItem1.Name = "simpleLabelItem1";
            this.simpleLabelItem1.Padding = new DevExpress.XtraLayout.Utils.Padding(20, 20, 20, 20);
            this.simpleLabelItem1.Size = new System.Drawing.Size(840, 59);
            this.simpleLabelItem1.Text = "ĐỌC ĐỊNH DANH SẢN PHẨM XUẤT KHO";
            this.simpleLabelItem1.TextSize = new System.Drawing.Size(320, 19);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.IdentifierValueTextEdit;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 59);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(593, 24);
            this.layoutControlItem1.Text = "Giá trị định danh";
            this.layoutControlItem1.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem1.TextSize = new System.Drawing.Size(78, 13);
            this.layoutControlItem1.TextToControlDistance = 5;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.StockInOutDetailForUIDtoGridControl;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 83);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(840, 410);
            this.layoutControlItem2.TextVisible = false;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlItem3.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlItem3.Control = this.RemoveHyperlinkLabelControl;
            this.layoutControlItem3.Location = new System.Drawing.Point(685, 59);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(70, 24);
            this.layoutControlItem3.TextVisible = false;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlItem4.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlItem4.Control = this.AddHyperlinkLabelControl;
            this.layoutControlItem4.Location = new System.Drawing.Point(593, 59);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(92, 24);
            this.layoutControlItem4.TextVisible = false;
            // 
            // FinishedHyperlinkLabelControl
            // 
            this.FinishedHyperlinkLabelControl.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.FinishedHyperlinkLabelControl.ImageOptions.Image = global::Inventory.Properties.Resources.apply_16x16;
            this.FinishedHyperlinkLabelControl.Location = new System.Drawing.Point(767, 71);
            this.FinishedHyperlinkLabelControl.Name = "FinishedHyperlinkLabelControl";
            this.FinishedHyperlinkLabelControl.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.FinishedHyperlinkLabelControl.Size = new System.Drawing.Size(81, 20);
            this.FinishedHyperlinkLabelControl.StyleController = this.layoutControl1;
            this.FinishedHyperlinkLabelControl.TabIndex = 8;
            this.FinishedHyperlinkLabelControl.Text = "Kết thúc";
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.FinishedHyperlinkLabelControl;
            this.layoutControlItem5.Location = new System.Drawing.Point(755, 59);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(85, 24);
            this.layoutControlItem5.TextVisible = false;
            // 
            // FrmGetIdentifierForStockOut
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(860, 513);
            this.Controls.Add(this.layoutControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmGetIdentifierForStockOut";
            this.Text = "Đọc định danh sản phẩm hàng hóa";
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.StockInOutDetailForUIDtoGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stockInOutDetailForUIDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StockInOutDetailForUIDtoGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductNameHypertextLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StockOutQtyTextEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NoteMemoEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.IdentifierValueTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraGrid.GridControl StockInOutDetailForUIDtoGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView StockInOutDetailForUIDtoGridView;
        private DevExpress.XtraEditors.TextEdit IdentifierValueTextEdit;
        private DevExpress.XtraLayout.SimpleLabelItem simpleLabelItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private System.Windows.Forms.BindingSource stockInOutDetailForUIDtoBindingSource;
        private DevExpress.XtraGrid.Columns.GridColumn colFullNameHtml;
        private DevExpress.XtraGrid.Columns.GridColumn colStockOutQty;
        private DevExpress.XtraGrid.Columns.GridColumn colGhiChu;
        private DevExpress.XtraEditors.HyperlinkLabelControl AddHyperlinkLabelControl;
        private DevExpress.XtraEditors.HyperlinkLabelControl RemoveHyperlinkLabelControl;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel ProductNameHypertextLabel;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit StockOutQtyTextEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit NoteMemoEdit;
        private DevExpress.XtraEditors.HyperlinkLabelControl FinishedHyperlinkLabelControl;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
    }
}