using DeviceAssetManagement.Management.DeviceMangement;

namespace DeviceAssetManagement.Management.DeviceWarranty
{
    partial class FrmDeviceWarranty
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
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar3 = new DevExpress.XtraBars.Bar();
            this.XemBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.ThemMoiBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.XoaBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.ExportFileBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.dockManager1 = new DevExpress.XtraBars.Docking.DockManager(this.components);
            this.barHeaderItem1 = new DevExpress.XtraBars.BarHeaderItem();
            this.DataSummaryBarStaticItem = new DevExpress.XtraBars.BarStaticItem();
            this.SelectedRowBarStaticItem = new DevExpress.XtraBars.BarStaticItem();
            this.barSubItem2 = new DevExpress.XtraBars.BarSubItem();
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.DeviceWarrantyGridViewGridControl = new DevExpress.XtraGrid.GridControl();
            this.warrantyDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.advBandedGridView1 = new DevExpress.XtraGrid.Views.BandedGrid.AdvBandedGridView();
            this.gridBand1 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.colWarrantyInfoHtml = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.HtmlRepositoryItemHypertextLabel = new DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel();
            this.colWarrantyVnsToKhachHangHtml = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gridBand2 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.colWarrantyNccToVnsFrom = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.repositoryItemDateEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.colWarrantyNccToVnsMonthOfWarranty = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colWarrantyNccToVnsUntil = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colWarrantyVnsToKhachHangStatusText = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gridBand3 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.colWarrantyVnsToKhachHangFrom = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colWarrantyVnsToKhachHangMonthOfWarranty = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colWarrantyVnsToKhachHangUntil = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colWarrantyNccToVnsStatusText = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colWarrantyNccToVnsHtml = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.NotesMemoEdit = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
            this.StatusComboBox = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DeviceWarrantyGridViewGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.warrantyDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.advBandedGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HtmlRepositoryItemHypertextLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NotesMemoEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StatusComboBox)).BeginInit();
            this.SuspendLayout();
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar3});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.DockManager = this.dockManager1;
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.XemBarButtonItem,
            this.ThemMoiBarButtonItem,
            this.XoaBarButtonItem,
            this.ExportFileBarButtonItem,
            this.barHeaderItem1,
            this.DataSummaryBarStaticItem,
            this.SelectedRowBarStaticItem,
            this.barSubItem2});
            this.barManager1.MaxItemId = 15;
            // 
            // bar3
            // 
            this.bar3.BarName = "Custom 2";
            this.bar3.DockCol = 0;
            this.bar3.DockRow = 0;
            this.bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar3.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.XemBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.ThemMoiBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.XoaBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.ExportFileBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar3.Text = "Custom 2";
            // 
            // XemBarButtonItem
            // 
            this.XemBarButtonItem.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True;
            this.XemBarButtonItem.Caption = "Xem";
            this.XemBarButtonItem.Id = 1;
            this.XemBarButtonItem.ImageOptions.Image = global::DeviceAssetManagement.Properties.Resources.listnumbers_16x16;
            this.XemBarButtonItem.ImageOptions.LargeImage = global::DeviceAssetManagement.Properties.Resources.listnumbers_32x32;
            this.XemBarButtonItem.Name = "XemBarButtonItem";
            // 
            // ThemMoiBarButtonItem
            // 
            this.ThemMoiBarButtonItem.Caption = "Thêm mới";
            this.ThemMoiBarButtonItem.Id = 2;
            this.ThemMoiBarButtonItem.ImageOptions.Image = global::DeviceAssetManagement.Properties.Resources.add_16x16;
            this.ThemMoiBarButtonItem.ImageOptions.LargeImage = global::DeviceAssetManagement.Properties.Resources.add_32x32;
            this.ThemMoiBarButtonItem.Name = "ThemMoiBarButtonItem";
            // 
            // XoaBarButtonItem
            // 
            this.XoaBarButtonItem.Caption = "Xóa";
            this.XoaBarButtonItem.Id = 4;
            this.XoaBarButtonItem.ImageOptions.Image = global::DeviceAssetManagement.Properties.Resources.clear_16x16;
            this.XoaBarButtonItem.ImageOptions.LargeImage = global::DeviceAssetManagement.Properties.Resources.clear_32x32;
            this.XoaBarButtonItem.Name = "XoaBarButtonItem";
            // 
            // ExportFileBarButtonItem
            // 
            this.ExportFileBarButtonItem.Caption = "Xuất file";
            this.ExportFileBarButtonItem.Id = 5;
            this.ExportFileBarButtonItem.ImageOptions.Image = global::DeviceAssetManagement.Properties.Resources.exporttoxls_16x16;
            this.ExportFileBarButtonItem.ImageOptions.LargeImage = global::DeviceAssetManagement.Properties.Resources.exporttoxls_32x32;
            this.ExportFileBarButtonItem.Name = "ExportFileBarButtonItem";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager1;
            this.barDockControlTop.Size = new System.Drawing.Size(1030, 39);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 503);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(1030, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 39);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 464);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1030, 39);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 464);
            // 
            // dockManager1
            // 
            this.dockManager1.Form = this;
            this.dockManager1.MenuManager = this.barManager1;
            this.dockManager1.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "DevExpress.XtraBars.StandaloneBarDockControl",
            "System.Windows.Forms.MenuStrip",
            "System.Windows.Forms.StatusStrip",
            "System.Windows.Forms.StatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonStatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonControl",
            "DevExpress.XtraBars.Navigation.OfficeNavigationBar",
            "DevExpress.XtraBars.Navigation.TileNavPane",
            "DevExpress.XtraBars.TabFormControl",
            "DevExpress.XtraBars.FluentDesignSystem.FluentDesignFormControl",
            "DevExpress.XtraBars.ToolbarForm.ToolbarFormControl"});
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
            // barSubItem2
            // 
            this.barSubItem2.Caption = "Điều chỉnh";
            this.barSubItem2.Id = 14;
            this.barSubItem2.Name = "barSubItem2";
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
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.XoaBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.ExportFileBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem2)});
            this.bar2.OptionsBar.MultiLine = true;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Main menu";
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
            // DeviceWarrantyGridViewGridControl
            // 
            this.DeviceWarrantyGridViewGridControl.DataSource = this.warrantyDtoBindingSource;
            this.DeviceWarrantyGridViewGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DeviceWarrantyGridViewGridControl.Location = new System.Drawing.Point(0, 39);
            this.DeviceWarrantyGridViewGridControl.MainView = this.advBandedGridView1;
            this.DeviceWarrantyGridViewGridControl.MenuManager = this.barManager1;
            this.DeviceWarrantyGridViewGridControl.Name = "DeviceWarrantyGridViewGridControl";
            this.DeviceWarrantyGridViewGridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.HtmlRepositoryItemHypertextLabel,
            this.NotesMemoEdit,
            this.StatusComboBox,
            this.repositoryItemDateEdit1});
            this.DeviceWarrantyGridViewGridControl.Size = new System.Drawing.Size(1030, 464);
            this.DeviceWarrantyGridViewGridControl.TabIndex = 5;
            this.DeviceWarrantyGridViewGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.advBandedGridView1});
            // 
            // warrantyDtoBindingSource
            // 
            this.warrantyDtoBindingSource.DataSource = typeof(DTO.DeviceAssetManagement.WarrantyDto);
            // 
            // advBandedGridView1
            // 
            this.advBandedGridView1.Appearance.ViewCaption.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.advBandedGridView1.Appearance.ViewCaption.ForeColor = DevExpress.LookAndFeel.DXSkinColors.ForeColors.Information;
            this.advBandedGridView1.Appearance.ViewCaption.Options.UseFont = true;
            this.advBandedGridView1.Appearance.ViewCaption.Options.UseForeColor = true;
            this.advBandedGridView1.Bands.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.gridBand1,
            this.gridBand2,
            this.gridBand3});
            this.advBandedGridView1.Columns.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn[] {
            this.colWarrantyInfoHtml,
            this.colWarrantyVnsToKhachHangFrom,
            this.colWarrantyVnsToKhachHangHtml,
            this.colWarrantyVnsToKhachHangMonthOfWarranty,
            this.colWarrantyVnsToKhachHangUntil,
            this.colWarrantyNccToVnsFrom,
            this.colWarrantyNccToVnsHtml,
            this.colWarrantyNccToVnsMonthOfWarranty,
            this.colWarrantyNccToVnsUntil,
            this.colWarrantyNccToVnsStatusText,
            this.colWarrantyVnsToKhachHangStatusText});
            this.advBandedGridView1.GridControl = this.DeviceWarrantyGridViewGridControl;
            this.advBandedGridView1.Name = "advBandedGridView1";
            this.advBandedGridView1.OptionsSelection.MultiSelect = true;
            this.advBandedGridView1.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            this.advBandedGridView1.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            this.advBandedGridView1.OptionsView.ShowGroupPanel = false;
            this.advBandedGridView1.OptionsView.ShowViewCaption = true;
            this.advBandedGridView1.ViewCaption = "QUẢN LÝ TÀI SẢN - THIẾT BỊ";
            // 
            // gridBand1
            // 
            this.gridBand1.AppearanceHeader.BackColor = System.Drawing.Color.SteelBlue;
            this.gridBand1.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.gridBand1.AppearanceHeader.ForeColor = System.Drawing.Color.White;
            this.gridBand1.AppearanceHeader.Options.UseBackColor = true;
            this.gridBand1.AppearanceHeader.Options.UseFont = true;
            this.gridBand1.AppearanceHeader.Options.UseForeColor = true;
            this.gridBand1.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBand1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridBand1.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridBand1.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.gridBand1.Caption = "THÔNG TIN THIẾT BỊ";
            this.gridBand1.Columns.Add(this.colWarrantyInfoHtml);
            this.gridBand1.Columns.Add(this.colWarrantyVnsToKhachHangHtml);
            this.gridBand1.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            this.gridBand1.Name = "gridBand1";
            this.gridBand1.VisibleIndex = 0;
            this.gridBand1.Width = 400;
            // 
            // colWarrantyInfoHtml
            // 
            this.colWarrantyInfoHtml.AppearanceCell.Options.UseTextOptions = true;
            this.colWarrantyInfoHtml.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colWarrantyInfoHtml.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colWarrantyInfoHtml.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colWarrantyInfoHtml.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colWarrantyInfoHtml.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colWarrantyInfoHtml.AppearanceHeader.Options.UseBackColor = true;
            this.colWarrantyInfoHtml.AppearanceHeader.Options.UseFont = true;
            this.colWarrantyInfoHtml.AppearanceHeader.Options.UseForeColor = true;
            this.colWarrantyInfoHtml.AppearanceHeader.Options.UseTextOptions = true;
            this.colWarrantyInfoHtml.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colWarrantyInfoHtml.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colWarrantyInfoHtml.Caption = "Thông tin bảo hành";
            this.colWarrantyInfoHtml.ColumnEdit = this.HtmlRepositoryItemHypertextLabel;
            this.colWarrantyInfoHtml.FieldName = "ProductVariantName";
            this.colWarrantyInfoHtml.Name = "colWarrantyInfoHtml";
            this.colWarrantyInfoHtml.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
            this.colWarrantyInfoHtml.OptionsEditForm.RowSpan = 3;
            this.colWarrantyInfoHtml.RowCount = 4;
            this.colWarrantyInfoHtml.Visible = true;
            this.colWarrantyInfoHtml.Width = 400;
            // 
            // HtmlRepositoryItemHypertextLabel
            // 
            this.HtmlRepositoryItemHypertextLabel.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.HtmlRepositoryItemHypertextLabel.Name = "HtmlRepositoryItemHypertextLabel";
            // 
            // colWarrantyVnsToKhachHangHtml
            // 
            this.colWarrantyVnsToKhachHangHtml.AppearanceCell.Options.UseTextOptions = true;
            this.colWarrantyVnsToKhachHangHtml.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colWarrantyVnsToKhachHangHtml.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colWarrantyVnsToKhachHangHtml.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colWarrantyVnsToKhachHangHtml.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colWarrantyVnsToKhachHangHtml.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colWarrantyVnsToKhachHangHtml.AppearanceHeader.Options.UseBackColor = true;
            this.colWarrantyVnsToKhachHangHtml.AppearanceHeader.Options.UseFont = true;
            this.colWarrantyVnsToKhachHangHtml.AppearanceHeader.Options.UseForeColor = true;
            this.colWarrantyVnsToKhachHangHtml.AppearanceHeader.Options.UseTextOptions = true;
            this.colWarrantyVnsToKhachHangHtml.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colWarrantyVnsToKhachHangHtml.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colWarrantyVnsToKhachHangHtml.Caption = "Thông tin BH";
            this.colWarrantyVnsToKhachHangHtml.ColumnEdit = this.HtmlRepositoryItemHypertextLabel;
            this.colWarrantyVnsToKhachHangHtml.FieldName = "WarrantyVnsToKhachHangHtml";
            this.colWarrantyVnsToKhachHangHtml.Name = "colWarrantyVnsToKhachHangHtml";
            this.colWarrantyVnsToKhachHangHtml.RowIndex = 1;
            this.colWarrantyVnsToKhachHangHtml.Width = 300;
            // 
            // gridBand2
            // 
            this.gridBand2.AppearanceHeader.BackColor = System.Drawing.Color.MediumPurple;
            this.gridBand2.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.gridBand2.AppearanceHeader.ForeColor = System.Drawing.Color.White;
            this.gridBand2.AppearanceHeader.Options.UseBackColor = true;
            this.gridBand2.AppearanceHeader.Options.UseFont = true;
            this.gridBand2.AppearanceHeader.Options.UseForeColor = true;
            this.gridBand2.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBand2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridBand2.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridBand2.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.gridBand2.Caption = "BẢO HÀNH NCC → VNS";
            this.gridBand2.Columns.Add(this.colWarrantyNccToVnsFrom);
            this.gridBand2.Columns.Add(this.colWarrantyNccToVnsMonthOfWarranty);
            this.gridBand2.Columns.Add(this.colWarrantyNccToVnsUntil);
            this.gridBand2.Columns.Add(this.colWarrantyNccToVnsStatusText);
            this.gridBand2.Name = "gridBand2";
            this.gridBand2.VisibleIndex = 1;
            this.gridBand2.Width = 229;
            // 
            // colWarrantyNccToVnsFrom
            // 
            this.colWarrantyNccToVnsFrom.AppearanceCell.Options.UseTextOptions = true;
            this.colWarrantyNccToVnsFrom.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colWarrantyNccToVnsFrom.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colWarrantyNccToVnsFrom.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colWarrantyNccToVnsFrom.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colWarrantyNccToVnsFrom.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colWarrantyNccToVnsFrom.AppearanceHeader.Options.UseBackColor = true;
            this.colWarrantyNccToVnsFrom.AppearanceHeader.Options.UseFont = true;
            this.colWarrantyNccToVnsFrom.AppearanceHeader.Options.UseForeColor = true;
            this.colWarrantyNccToVnsFrom.AppearanceHeader.Options.UseTextOptions = true;
            this.colWarrantyNccToVnsFrom.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colWarrantyNccToVnsFrom.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colWarrantyNccToVnsFrom.Caption = "Ngày bắt đầu";
            this.colWarrantyNccToVnsFrom.ColumnEdit = this.repositoryItemDateEdit1;
            this.colWarrantyNccToVnsFrom.FieldName = "WarrantyNccToVnsFrom";
            this.colWarrantyNccToVnsFrom.Name = "colWarrantyNccToVnsFrom";
            this.colWarrantyNccToVnsFrom.Visible = true;
            this.colWarrantyNccToVnsFrom.Width = 229;
            // 
            // repositoryItemDateEdit1
            // 
            this.repositoryItemDateEdit1.AutoHeight = false;
            this.repositoryItemDateEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit1.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit1.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.repositoryItemDateEdit1.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.repositoryItemDateEdit1.EditFormat.FormatString = "dd/MM/yyyy";
            this.repositoryItemDateEdit1.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.repositoryItemDateEdit1.MaskSettings.Set("mask", "dd/MM/yyyy");
            this.repositoryItemDateEdit1.Name = "repositoryItemDateEdit1";
            // 
            // colWarrantyNccToVnsMonthOfWarranty
            // 
            this.colWarrantyNccToVnsMonthOfWarranty.AppearanceCell.Options.UseTextOptions = true;
            this.colWarrantyNccToVnsMonthOfWarranty.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colWarrantyNccToVnsMonthOfWarranty.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colWarrantyNccToVnsMonthOfWarranty.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colWarrantyNccToVnsMonthOfWarranty.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colWarrantyNccToVnsMonthOfWarranty.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colWarrantyNccToVnsMonthOfWarranty.AppearanceHeader.Options.UseBackColor = true;
            this.colWarrantyNccToVnsMonthOfWarranty.AppearanceHeader.Options.UseFont = true;
            this.colWarrantyNccToVnsMonthOfWarranty.AppearanceHeader.Options.UseForeColor = true;
            this.colWarrantyNccToVnsMonthOfWarranty.AppearanceHeader.Options.UseTextOptions = true;
            this.colWarrantyNccToVnsMonthOfWarranty.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colWarrantyNccToVnsMonthOfWarranty.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colWarrantyNccToVnsMonthOfWarranty.Caption = "Tháng BH";
            this.colWarrantyNccToVnsMonthOfWarranty.FieldName = "WarrantyNccToVnsMonthOfWarranty";
            this.colWarrantyNccToVnsMonthOfWarranty.Name = "colWarrantyNccToVnsMonthOfWarranty";
            this.colWarrantyNccToVnsMonthOfWarranty.RowIndex = 1;
            this.colWarrantyNccToVnsMonthOfWarranty.Visible = true;
            this.colWarrantyNccToVnsMonthOfWarranty.Width = 229;
            // 
            // colWarrantyNccToVnsUntil
            // 
            this.colWarrantyNccToVnsUntil.AppearanceCell.Options.UseTextOptions = true;
            this.colWarrantyNccToVnsUntil.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colWarrantyNccToVnsUntil.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colWarrantyNccToVnsUntil.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colWarrantyNccToVnsUntil.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colWarrantyNccToVnsUntil.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colWarrantyNccToVnsUntil.AppearanceHeader.Options.UseBackColor = true;
            this.colWarrantyNccToVnsUntil.AppearanceHeader.Options.UseFont = true;
            this.colWarrantyNccToVnsUntil.AppearanceHeader.Options.UseForeColor = true;
            this.colWarrantyNccToVnsUntil.AppearanceHeader.Options.UseTextOptions = true;
            this.colWarrantyNccToVnsUntil.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colWarrantyNccToVnsUntil.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colWarrantyNccToVnsUntil.Caption = "Ngày hết hạn";
            this.colWarrantyNccToVnsUntil.ColumnEdit = this.repositoryItemDateEdit1;
            this.colWarrantyNccToVnsUntil.FieldName = "WarrantyNccToVnsUntil";
            this.colWarrantyNccToVnsUntil.Name = "colWarrantyNccToVnsUntil";
            this.colWarrantyNccToVnsUntil.RowIndex = 2;
            this.colWarrantyNccToVnsUntil.Visible = true;
            this.colWarrantyNccToVnsUntil.Width = 229;
            // 
            // colWarrantyVnsToKhachHangStatusText
            // 
            this.colWarrantyVnsToKhachHangStatusText.AppearanceCell.Options.UseTextOptions = true;
            this.colWarrantyVnsToKhachHangStatusText.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colWarrantyVnsToKhachHangStatusText.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colWarrantyVnsToKhachHangStatusText.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colWarrantyVnsToKhachHangStatusText.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colWarrantyVnsToKhachHangStatusText.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colWarrantyVnsToKhachHangStatusText.AppearanceHeader.Options.UseBackColor = true;
            this.colWarrantyVnsToKhachHangStatusText.AppearanceHeader.Options.UseFont = true;
            this.colWarrantyVnsToKhachHangStatusText.AppearanceHeader.Options.UseForeColor = true;
            this.colWarrantyVnsToKhachHangStatusText.AppearanceHeader.Options.UseTextOptions = true;
            this.colWarrantyVnsToKhachHangStatusText.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colWarrantyVnsToKhachHangStatusText.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colWarrantyVnsToKhachHangStatusText.Caption = "Tình trạng";
            this.colWarrantyVnsToKhachHangStatusText.ColumnEdit = this.HtmlRepositoryItemHypertextLabel;
            this.colWarrantyVnsToKhachHangStatusText.FieldName = "WarrantyVnsToKhachHangStatusText";
            this.colWarrantyVnsToKhachHangStatusText.Name = "colWarrantyVnsToKhachHangStatusText";
            this.colWarrantyVnsToKhachHangStatusText.RowIndex = 3;
            this.colWarrantyVnsToKhachHangStatusText.Visible = true;
            this.colWarrantyVnsToKhachHangStatusText.Width = 237;
            // 
            // gridBand3
            // 
            this.gridBand3.AppearanceHeader.BackColor = System.Drawing.Color.DarkCyan;
            this.gridBand3.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.gridBand3.AppearanceHeader.ForeColor = System.Drawing.Color.White;
            this.gridBand3.AppearanceHeader.Options.UseBackColor = true;
            this.gridBand3.AppearanceHeader.Options.UseFont = true;
            this.gridBand3.AppearanceHeader.Options.UseForeColor = true;
            this.gridBand3.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBand3.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridBand3.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridBand3.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.gridBand3.Caption = "BẢO HÀNH VNS → KHÁCH HÀNG";
            this.gridBand3.Columns.Add(this.colWarrantyVnsToKhachHangFrom);
            this.gridBand3.Columns.Add(this.colWarrantyVnsToKhachHangMonthOfWarranty);
            this.gridBand3.Columns.Add(this.colWarrantyVnsToKhachHangUntil);
            this.gridBand3.Columns.Add(this.colWarrantyVnsToKhachHangStatusText);
            this.gridBand3.Name = "gridBand3";
            this.gridBand3.VisibleIndex = 2;
            this.gridBand3.Width = 237;
            // 
            // colWarrantyVnsToKhachHangFrom
            // 
            this.colWarrantyVnsToKhachHangFrom.AppearanceCell.Options.UseTextOptions = true;
            this.colWarrantyVnsToKhachHangFrom.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colWarrantyVnsToKhachHangFrom.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colWarrantyVnsToKhachHangFrom.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colWarrantyVnsToKhachHangFrom.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colWarrantyVnsToKhachHangFrom.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colWarrantyVnsToKhachHangFrom.AppearanceHeader.Options.UseBackColor = true;
            this.colWarrantyVnsToKhachHangFrom.AppearanceHeader.Options.UseFont = true;
            this.colWarrantyVnsToKhachHangFrom.AppearanceHeader.Options.UseForeColor = true;
            this.colWarrantyVnsToKhachHangFrom.AppearanceHeader.Options.UseTextOptions = true;
            this.colWarrantyVnsToKhachHangFrom.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colWarrantyVnsToKhachHangFrom.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colWarrantyVnsToKhachHangFrom.Caption = "Ngày bắt đầu";
            this.colWarrantyVnsToKhachHangFrom.ColumnEdit = this.repositoryItemDateEdit1;
            this.colWarrantyVnsToKhachHangFrom.FieldName = "WarrantyVnsToKhachHangFrom";
            this.colWarrantyVnsToKhachHangFrom.Name = "colWarrantyVnsToKhachHangFrom";
            this.colWarrantyVnsToKhachHangFrom.Visible = true;
            this.colWarrantyVnsToKhachHangFrom.Width = 237;
            // 
            // colWarrantyVnsToKhachHangMonthOfWarranty
            // 
            this.colWarrantyVnsToKhachHangMonthOfWarranty.AppearanceCell.Options.UseTextOptions = true;
            this.colWarrantyVnsToKhachHangMonthOfWarranty.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colWarrantyVnsToKhachHangMonthOfWarranty.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colWarrantyVnsToKhachHangMonthOfWarranty.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colWarrantyVnsToKhachHangMonthOfWarranty.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colWarrantyVnsToKhachHangMonthOfWarranty.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colWarrantyVnsToKhachHangMonthOfWarranty.AppearanceHeader.Options.UseBackColor = true;
            this.colWarrantyVnsToKhachHangMonthOfWarranty.AppearanceHeader.Options.UseFont = true;
            this.colWarrantyVnsToKhachHangMonthOfWarranty.AppearanceHeader.Options.UseForeColor = true;
            this.colWarrantyVnsToKhachHangMonthOfWarranty.AppearanceHeader.Options.UseTextOptions = true;
            this.colWarrantyVnsToKhachHangMonthOfWarranty.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colWarrantyVnsToKhachHangMonthOfWarranty.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colWarrantyVnsToKhachHangMonthOfWarranty.Caption = "Tháng BH";
            this.colWarrantyVnsToKhachHangMonthOfWarranty.FieldName = "WarrantyVnsToKhachHangMonthOfWarranty";
            this.colWarrantyVnsToKhachHangMonthOfWarranty.Name = "colWarrantyVnsToKhachHangMonthOfWarranty";
            this.colWarrantyVnsToKhachHangMonthOfWarranty.RowIndex = 1;
            this.colWarrantyVnsToKhachHangMonthOfWarranty.Visible = true;
            this.colWarrantyVnsToKhachHangMonthOfWarranty.Width = 237;
            // 
            // colWarrantyVnsToKhachHangUntil
            // 
            this.colWarrantyVnsToKhachHangUntil.AppearanceCell.Options.UseTextOptions = true;
            this.colWarrantyVnsToKhachHangUntil.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colWarrantyVnsToKhachHangUntil.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colWarrantyVnsToKhachHangUntil.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colWarrantyVnsToKhachHangUntil.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colWarrantyVnsToKhachHangUntil.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colWarrantyVnsToKhachHangUntil.AppearanceHeader.Options.UseBackColor = true;
            this.colWarrantyVnsToKhachHangUntil.AppearanceHeader.Options.UseFont = true;
            this.colWarrantyVnsToKhachHangUntil.AppearanceHeader.Options.UseForeColor = true;
            this.colWarrantyVnsToKhachHangUntil.AppearanceHeader.Options.UseTextOptions = true;
            this.colWarrantyVnsToKhachHangUntil.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colWarrantyVnsToKhachHangUntil.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colWarrantyVnsToKhachHangUntil.Caption = "Ngày hết hạn";
            this.colWarrantyVnsToKhachHangUntil.ColumnEdit = this.repositoryItemDateEdit1;
            this.colWarrantyVnsToKhachHangUntil.FieldName = "WarrantyVnsToKhachHangUntil";
            this.colWarrantyVnsToKhachHangUntil.Name = "colWarrantyVnsToKhachHangUntil";
            this.colWarrantyVnsToKhachHangUntil.RowIndex = 2;
            this.colWarrantyVnsToKhachHangUntil.Visible = true;
            this.colWarrantyVnsToKhachHangUntil.Width = 237;
            // 
            // colWarrantyNccToVnsStatusText
            // 
            this.colWarrantyNccToVnsStatusText.AppearanceCell.Options.UseTextOptions = true;
            this.colWarrantyNccToVnsStatusText.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colWarrantyNccToVnsStatusText.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colWarrantyNccToVnsStatusText.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colWarrantyNccToVnsStatusText.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colWarrantyNccToVnsStatusText.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colWarrantyNccToVnsStatusText.AppearanceHeader.Options.UseBackColor = true;
            this.colWarrantyNccToVnsStatusText.AppearanceHeader.Options.UseFont = true;
            this.colWarrantyNccToVnsStatusText.AppearanceHeader.Options.UseForeColor = true;
            this.colWarrantyNccToVnsStatusText.AppearanceHeader.Options.UseTextOptions = true;
            this.colWarrantyNccToVnsStatusText.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colWarrantyNccToVnsStatusText.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colWarrantyNccToVnsStatusText.Caption = "Tình trạng";
            this.colWarrantyNccToVnsStatusText.ColumnEdit = this.HtmlRepositoryItemHypertextLabel;
            this.colWarrantyNccToVnsStatusText.FieldName = "WarrantyNccToVnsStatusText";
            this.colWarrantyNccToVnsStatusText.Name = "colWarrantyNccToVnsStatusText";
            this.colWarrantyNccToVnsStatusText.RowIndex = 3;
            this.colWarrantyNccToVnsStatusText.Visible = true;
            this.colWarrantyNccToVnsStatusText.Width = 229;
            // 
            // colWarrantyNccToVnsHtml
            // 
            this.colWarrantyNccToVnsHtml.AppearanceCell.Options.UseTextOptions = true;
            this.colWarrantyNccToVnsHtml.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colWarrantyNccToVnsHtml.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colWarrantyNccToVnsHtml.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colWarrantyNccToVnsHtml.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colWarrantyNccToVnsHtml.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colWarrantyNccToVnsHtml.AppearanceHeader.Options.UseBackColor = true;
            this.colWarrantyNccToVnsHtml.AppearanceHeader.Options.UseFont = true;
            this.colWarrantyNccToVnsHtml.AppearanceHeader.Options.UseForeColor = true;
            this.colWarrantyNccToVnsHtml.AppearanceHeader.Options.UseTextOptions = true;
            this.colWarrantyNccToVnsHtml.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colWarrantyNccToVnsHtml.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colWarrantyNccToVnsHtml.Caption = "Thông tin BH";
            this.colWarrantyNccToVnsHtml.ColumnEdit = this.HtmlRepositoryItemHypertextLabel;
            this.colWarrantyNccToVnsHtml.FieldName = "WarrantyNccToVnsHtml";
            this.colWarrantyNccToVnsHtml.Name = "colWarrantyNccToVnsHtml";
            this.colWarrantyNccToVnsHtml.Visible = true;
            this.colWarrantyNccToVnsHtml.Width = 300;
            // 
            // NotesMemoEdit
            // 
            this.NotesMemoEdit.Name = "NotesMemoEdit";
            // 
            // StatusComboBox
            // 
            this.StatusComboBox.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.StatusComboBox.AutoHeight = false;
            this.StatusComboBox.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.StatusComboBox.ContextButtonOptions.AllowHtmlText = true;
            this.StatusComboBox.Items.AddRange(new object[] {
            "<color=\'red\'>Test</color>"});
            this.StatusComboBox.Name = "StatusComboBox";
            this.StatusComboBox.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            // 
            // FrmDeviceWarranty
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1030, 503);
            this.Controls.Add(this.DeviceWarrantyGridViewGridControl);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "FrmDeviceWarranty";
            this.Text = "QUẢN LÝ BẢO HÀNH";
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DeviceWarrantyGridViewGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.warrantyDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.advBandedGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HtmlRepositoryItemHypertextLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NotesMemoEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StatusComboBox)).EndInit();
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
        private DevExpress.XtraBars.BarButtonItem XemBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem ThemMoiBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem XoaBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem ExportFileBarButtonItem;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarHeaderItem barHeaderItem1;
        private DevExpress.XtraBars.BarStaticItem DataSummaryBarStaticItem;
        private DevExpress.XtraBars.BarStaticItem SelectedRowBarStaticItem;
        private DevExpress.XtraGrid.GridControl DeviceWarrantyGridViewGridControl;
        private DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel HtmlRepositoryItemHypertextLabel;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox StatusComboBox;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit NotesMemoEdit;
        private DevExpress.XtraBars.Docking.DockManager dockManager1;
        private DevExpress.XtraBars.BarSubItem barSubItem2;
        private DevExpress.XtraBars.Bar bar3;
        private System.Windows.Forms.BindingSource warrantyDtoBindingSource;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEdit1;
        private DevExpress.XtraGrid.Views.BandedGrid.AdvBandedGridView advBandedGridView1;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colWarrantyInfoHtml;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colWarrantyNccToVnsFrom;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colWarrantyNccToVnsMonthOfWarranty;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colWarrantyNccToVnsUntil;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colWarrantyVnsToKhachHangFrom;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colWarrantyVnsToKhachHangMonthOfWarranty;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colWarrantyVnsToKhachHangUntil;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colWarrantyVnsToKhachHangHtml;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colWarrantyNccToVnsHtml;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand1;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand2;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colWarrantyVnsToKhachHangStatusText;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand3;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colWarrantyNccToVnsStatusText;
    }
}