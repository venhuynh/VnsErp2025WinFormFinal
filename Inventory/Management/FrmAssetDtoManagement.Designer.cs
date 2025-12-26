using DTO.DeviceAssetManagement;

namespace Inventory.Management
{
    partial class FrmAssetDtoManagement
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
            this.assetDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.XemBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.ThemMoiBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.SuaBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.XoaBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.ExportFileBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.barHeaderItem1 = new DevExpress.XtraBars.BarHeaderItem();
            this.DataSummaryBarStaticItem = new DevExpress.XtraBars.BarStaticItem();
            this.SelectedRowBarStaticItem = new DevExpress.XtraBars.BarStaticItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.AssetDtoGridViewGridControl = new DevExpress.XtraGrid.GridControl();
            this.AssetDtoGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colAssetInfoHtml = new DevExpress.XtraGrid.Columns.GridColumn();
            this.HtmlRepositoryItemHypertextLabel = new DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel();
            this.colLocationHtml = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPurchasePriceHtml = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPurchaseDateHtml = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.assetDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AssetDtoGridViewGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AssetDtoGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HtmlRepositoryItemHypertextLabel)).BeginInit();
            this.SuspendLayout();
            // 
            // assetDtoBindingSource
            // 
            this.assetDtoBindingSource.DataSource = typeof(AssetDto);
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
            this.XemBarButtonItem,
            this.ThemMoiBarButtonItem,
            this.SuaBarButtonItem,
            this.XoaBarButtonItem,
            this.ExportFileBarButtonItem,
            this.barHeaderItem1,
            this.DataSummaryBarStaticItem,
            this.SelectedRowBarStaticItem});
            this.barManager1.MainMenu = this.bar2;
            this.barManager1.MaxItemId = 9;
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
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.XemBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.ThemMoiBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.SuaBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.XoaBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.ExportFileBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar2.OptionsBar.MultiLine = true;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Main menu";
            // 
            // XemBarButtonItem
            // 
            this.XemBarButtonItem.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True;
            this.XemBarButtonItem.Caption = "Xem";
            this.XemBarButtonItem.Id = 1;
            this.XemBarButtonItem.ImageOptions.Image = global::Inventory.Properties.Resources.listnumbers_16x16;
            this.XemBarButtonItem.ImageOptions.LargeImage = global::Inventory.Properties.Resources.listnumbers_32x32;
            this.XemBarButtonItem.Name = "XemBarButtonItem";
            // 
            // ThemMoiBarButtonItem
            // 
            this.ThemMoiBarButtonItem.Caption = "Thêm mới";
            this.ThemMoiBarButtonItem.Id = 2;
            this.ThemMoiBarButtonItem.ImageOptions.Image = global::Inventory.Properties.Resources.add_16x16;
            this.ThemMoiBarButtonItem.Name = "ThemMoiBarButtonItem";
            // 
            // SuaBarButtonItem
            // 
            this.SuaBarButtonItem.Caption = "Sửa";
            this.SuaBarButtonItem.Id = 3;
            this.SuaBarButtonItem.ImageOptions.Image = global::Inventory.Properties.Resources.edit_16x16;
            this.SuaBarButtonItem.ImageOptions.LargeImage = global::Inventory.Properties.Resources.edit_32x32;
            this.SuaBarButtonItem.Name = "SuaBarButtonItem";
            // 
            // XoaBarButtonItem
            // 
            this.XoaBarButtonItem.Caption = "Xóa";
            this.XoaBarButtonItem.Id = 4;
            this.XoaBarButtonItem.ImageOptions.Image = global::Inventory.Properties.Resources.clear_16x16;
            this.XoaBarButtonItem.Name = "XoaBarButtonItem";
            // 
            // ExportFileBarButtonItem
            // 
            this.ExportFileBarButtonItem.Caption = "Xuất file";
            this.ExportFileBarButtonItem.Id = 5;
            this.ExportFileBarButtonItem.ImageOptions.Image = global::Inventory.Properties.Resources.exporttoxps_16x16;
            this.ExportFileBarButtonItem.Name = "ExportFileBarButtonItem";
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
            this.barHeaderItem1.Id = 6;
            this.barHeaderItem1.Name = "barHeaderItem1";
            // 
            // DataSummaryBarStaticItem
            // 
            this.DataSummaryBarStaticItem.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True;
            this.DataSummaryBarStaticItem.Caption = "Chưa có dữ liệu";
            this.DataSummaryBarStaticItem.Id = 7;
            this.DataSummaryBarStaticItem.Name = "DataSummaryBarStaticItem";
            // 
            // SelectedRowBarStaticItem
            // 
            this.SelectedRowBarStaticItem.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True;
            this.SelectedRowBarStaticItem.Caption = "Chưa chọn dòng nào";
            this.SelectedRowBarStaticItem.Id = 8;
            this.SelectedRowBarStaticItem.Name = "SelectedRowBarStaticItem";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager1;
            this.barDockControlTop.Size = new System.Drawing.Size(1525, 24);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 510);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(1525, 22);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 24);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 486);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1525, 24);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 486);
            // 
            // AssetDtoGridViewGridControl
            // 
            this.AssetDtoGridViewGridControl.DataSource = this.assetDtoBindingSource;
            this.AssetDtoGridViewGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AssetDtoGridViewGridControl.Location = new System.Drawing.Point(0, 24);
            this.AssetDtoGridViewGridControl.MainView = this.AssetDtoGridView;
            this.AssetDtoGridViewGridControl.MenuManager = this.barManager1;
            this.AssetDtoGridViewGridControl.Name = "AssetDtoGridViewGridControl";
            this.AssetDtoGridViewGridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.HtmlRepositoryItemHypertextLabel});
            this.AssetDtoGridViewGridControl.Size = new System.Drawing.Size(1525, 486);
            this.AssetDtoGridViewGridControl.TabIndex = 5;
            this.AssetDtoGridViewGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.AssetDtoGridView});
            // 
            // AssetDtoGridView
            // 
            this.AssetDtoGridView.Appearance.ViewCaption.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.AssetDtoGridView.Appearance.ViewCaption.ForeColor = DevExpress.LookAndFeel.DXSkinColors.ForeColors.Information;
            this.AssetDtoGridView.Appearance.ViewCaption.Options.UseFont = true;
            this.AssetDtoGridView.Appearance.ViewCaption.Options.UseForeColor = true;
            this.AssetDtoGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colAssetInfoHtml,
            this.colLocationHtml,
            this.colPurchasePriceHtml,
            this.colPurchaseDateHtml});
            this.AssetDtoGridView.GridControl = this.AssetDtoGridViewGridControl;
            this.AssetDtoGridView.Name = "AssetDtoGridView";
            this.AssetDtoGridView.OptionsBehavior.Editable = false;
            this.AssetDtoGridView.OptionsSelection.MultiSelect = true;
            this.AssetDtoGridView.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            this.AssetDtoGridView.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            this.AssetDtoGridView.OptionsView.RowAutoHeight = true;
            this.AssetDtoGridView.OptionsView.ShowGroupPanel = false;
            this.AssetDtoGridView.OptionsView.ShowViewCaption = true;
            this.AssetDtoGridView.ViewCaption = "QUẢN LÝ TÀI SẢN";
            // 
            // colAssetInfoHtml
            // 
            this.colAssetInfoHtml.AppearanceCell.Options.UseTextOptions = true;
            this.colAssetInfoHtml.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colAssetInfoHtml.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colAssetInfoHtml.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colAssetInfoHtml.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colAssetInfoHtml.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colAssetInfoHtml.AppearanceHeader.Options.UseBackColor = true;
            this.colAssetInfoHtml.AppearanceHeader.Options.UseFont = true;
            this.colAssetInfoHtml.AppearanceHeader.Options.UseForeColor = true;
            this.colAssetInfoHtml.AppearanceHeader.Options.UseTextOptions = true;
            this.colAssetInfoHtml.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colAssetInfoHtml.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colAssetInfoHtml.Caption = "Thông tin tài sản";
            this.colAssetInfoHtml.ColumnEdit = this.HtmlRepositoryItemHypertextLabel;
            this.colAssetInfoHtml.FieldName = "AssetInfoHtml";
            this.colAssetInfoHtml.Name = "colAssetInfoHtml";
            this.colAssetInfoHtml.OptionsColumn.AllowEdit = false;
            this.colAssetInfoHtml.OptionsColumn.ReadOnly = true;
            this.colAssetInfoHtml.Visible = true;
            this.colAssetInfoHtml.VisibleIndex = 1;
            this.colAssetInfoHtml.Width = 300;
            // 
            // HtmlRepositoryItemHypertextLabel
            // 
            this.HtmlRepositoryItemHypertextLabel.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.HtmlRepositoryItemHypertextLabel.Name = "HtmlRepositoryItemHypertextLabel";
            // 
            // colLocationHtml
            // 
            this.colLocationHtml.AppearanceCell.Options.UseTextOptions = true;
            this.colLocationHtml.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colLocationHtml.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colLocationHtml.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colLocationHtml.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colLocationHtml.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colLocationHtml.AppearanceHeader.Options.UseBackColor = true;
            this.colLocationHtml.AppearanceHeader.Options.UseFont = true;
            this.colLocationHtml.AppearanceHeader.Options.UseForeColor = true;
            this.colLocationHtml.AppearanceHeader.Options.UseTextOptions = true;
            this.colLocationHtml.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colLocationHtml.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colLocationHtml.Caption = "Vị trí";
            this.colLocationHtml.ColumnEdit = this.HtmlRepositoryItemHypertextLabel;
            this.colLocationHtml.FieldName = "LocationHtml";
            this.colLocationHtml.Name = "colLocationHtml";
            this.colLocationHtml.OptionsColumn.AllowEdit = false;
            this.colLocationHtml.OptionsColumn.ReadOnly = true;
            this.colLocationHtml.Visible = true;
            this.colLocationHtml.VisibleIndex = 2;
            this.colLocationHtml.Width = 200;
            // 
            // colPurchasePriceHtml
            // 
            this.colPurchasePriceHtml.AppearanceCell.Options.UseTextOptions = true;
            this.colPurchasePriceHtml.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colPurchasePriceHtml.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colPurchasePriceHtml.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colPurchasePriceHtml.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colPurchasePriceHtml.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colPurchasePriceHtml.AppearanceHeader.Options.UseBackColor = true;
            this.colPurchasePriceHtml.AppearanceHeader.Options.UseFont = true;
            this.colPurchasePriceHtml.AppearanceHeader.Options.UseForeColor = true;
            this.colPurchasePriceHtml.AppearanceHeader.Options.UseTextOptions = true;
            this.colPurchasePriceHtml.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colPurchasePriceHtml.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colPurchasePriceHtml.Caption = "Giá mua";
            this.colPurchasePriceHtml.ColumnEdit = this.HtmlRepositoryItemHypertextLabel;
            this.colPurchasePriceHtml.FieldName = "PurchasePriceHtml";
            this.colPurchasePriceHtml.Name = "colPurchasePriceHtml";
            this.colPurchasePriceHtml.OptionsColumn.AllowEdit = false;
            this.colPurchasePriceHtml.OptionsColumn.ReadOnly = true;
            this.colPurchasePriceHtml.Visible = true;
            this.colPurchasePriceHtml.VisibleIndex = 3;
            this.colPurchasePriceHtml.Width = 150;
            // 
            // colPurchaseDateHtml
            // 
            this.colPurchaseDateHtml.AppearanceCell.Options.UseTextOptions = true;
            this.colPurchaseDateHtml.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colPurchaseDateHtml.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colPurchaseDateHtml.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colPurchaseDateHtml.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colPurchaseDateHtml.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colPurchaseDateHtml.AppearanceHeader.Options.UseBackColor = true;
            this.colPurchaseDateHtml.AppearanceHeader.Options.UseFont = true;
            this.colPurchaseDateHtml.AppearanceHeader.Options.UseForeColor = true;
            this.colPurchaseDateHtml.AppearanceHeader.Options.UseTextOptions = true;
            this.colPurchaseDateHtml.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colPurchaseDateHtml.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colPurchaseDateHtml.Caption = "Ngày mua";
            this.colPurchaseDateHtml.ColumnEdit = this.HtmlRepositoryItemHypertextLabel;
            this.colPurchaseDateHtml.FieldName = "PurchaseDateHtml";
            this.colPurchaseDateHtml.Name = "colPurchaseDateHtml";
            this.colPurchaseDateHtml.OptionsColumn.AllowEdit = false;
            this.colPurchaseDateHtml.OptionsColumn.ReadOnly = true;
            this.colPurchaseDateHtml.Visible = true;
            this.colPurchaseDateHtml.VisibleIndex = 4;
            this.colPurchaseDateHtml.Width = 150;
            // 
            // FrmAssetDtoManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1525, 532);
            this.Controls.Add(this.AssetDtoGridViewGridControl);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "FrmAssetDtoManagement";
            this.Text = "Quản lý tài sản";
            ((System.ComponentModel.ISupportInitialize)(this.assetDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AssetDtoGridViewGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AssetDtoGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HtmlRepositoryItemHypertextLabel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private System.Windows.Forms.BindingSource assetDtoBindingSource;
        private DevExpress.XtraBars.BarButtonItem XemBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem ThemMoiBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem SuaBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem XoaBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem ExportFileBarButtonItem;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarHeaderItem barHeaderItem1;
        private DevExpress.XtraBars.BarStaticItem DataSummaryBarStaticItem;
        private DevExpress.XtraBars.BarStaticItem SelectedRowBarStaticItem;
        private DevExpress.XtraGrid.GridControl AssetDtoGridViewGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView AssetDtoGridView;
        private DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel HtmlRepositoryItemHypertextLabel;
        private DevExpress.XtraGrid.Columns.GridColumn colAssetInfoHtml;
        private DevExpress.XtraGrid.Columns.GridColumn colLocationHtml;
        private DevExpress.XtraGrid.Columns.GridColumn colPurchaseDateHtml;
        private DevExpress.XtraGrid.Columns.GridColumn colPurchasePriceHtml;
    }
}
