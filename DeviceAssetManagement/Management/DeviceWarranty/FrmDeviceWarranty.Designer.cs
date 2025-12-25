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
            this.dockPanel1 = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel1_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.ucDeviceWarranty1 = new DeviceAssetManagement.Management.DeviceMangement.UcDeviceWarranty();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.barHeaderItem1 = new DevExpress.XtraBars.BarHeaderItem();
            this.DataSummaryBarStaticItem = new DevExpress.XtraBars.BarStaticItem();
            this.SelectedRowBarStaticItem = new DevExpress.XtraBars.BarStaticItem();
            this.barSubItem2 = new DevExpress.XtraBars.BarSubItem();
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.DeviceWarrantyGridViewGridControl = new DevExpress.XtraGrid.GridControl();
            this.warrantyDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.DeviceWarrantyDtoGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colWarrantyInfoHtml = new DevExpress.XtraGrid.Columns.GridColumn();
            this.HtmlRepositoryItemHypertextLabel = new DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel();
            this.colDeviceHtmlInfo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.NotesMemoEdit = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
            this.StatusComboBox = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).BeginInit();
            this.dockPanel1.SuspendLayout();
            this.dockPanel1_Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DeviceWarrantyGridViewGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.warrantyDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DeviceWarrantyDtoGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HtmlRepositoryItemHypertextLabel)).BeginInit();
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
            this.dockManager1.RootPanels.AddRange(new DevExpress.XtraBars.Docking.DockPanel[] {
            this.dockPanel1});
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
            // dockPanel1
            // 
            this.dockPanel1.Controls.Add(this.dockPanel1_Container);
            this.dockPanel1.Dock = DevExpress.XtraBars.Docking.DockingStyle.Right;
            this.dockPanel1.ID = new System.Guid("0c476c5c-6d27-493e-8a8e-c8d2ad65a997");
            this.dockPanel1.Location = new System.Drawing.Point(587, 39);
            this.dockPanel1.Name = "dockPanel1";
            this.dockPanel1.OriginalSize = new System.Drawing.Size(443, 200);
            this.dockPanel1.Size = new System.Drawing.Size(443, 464);
            this.dockPanel1.Text = "dockPanel1";
            // 
            // dockPanel1_Container
            // 
            this.dockPanel1_Container.Controls.Add(this.layoutControl1);
            this.dockPanel1_Container.Location = new System.Drawing.Point(4, 38);
            this.dockPanel1_Container.Name = "dockPanel1_Container";
            this.dockPanel1_Container.Size = new System.Drawing.Size(436, 423);
            this.dockPanel1_Container.TabIndex = 0;
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.ucDeviceWarranty1);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.Root;
            this.layoutControl1.Size = new System.Drawing.Size(436, 423);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // ucDeviceWarranty1
            // 
            this.ucDeviceWarranty1.Location = new System.Drawing.Point(16, 16);
            this.ucDeviceWarranty1.Name = "ucDeviceWarranty1";
            this.ucDeviceWarranty1.Size = new System.Drawing.Size(404, 391);
            this.ucDeviceWarranty1.TabIndex = 7;
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem4});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(436, 423);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.ucDeviceWarranty1;
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(410, 397);
            this.layoutControlItem4.TextVisible = false;
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
            this.DeviceWarrantyGridViewGridControl.MainView = this.DeviceWarrantyDtoGridView;
            this.DeviceWarrantyGridViewGridControl.MenuManager = this.barManager1;
            this.DeviceWarrantyGridViewGridControl.Name = "DeviceWarrantyGridViewGridControl";
            this.DeviceWarrantyGridViewGridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.HtmlRepositoryItemHypertextLabel,
            this.NotesMemoEdit,
            this.StatusComboBox});
            this.DeviceWarrantyGridViewGridControl.Size = new System.Drawing.Size(587, 464);
            this.DeviceWarrantyGridViewGridControl.TabIndex = 5;
            this.DeviceWarrantyGridViewGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.DeviceWarrantyDtoGridView});
            // 
            // warrantyDtoBindingSource
            // 
            this.warrantyDtoBindingSource.DataSource = typeof(DTO.DeviceAssetManagement.WarrantyDto);
            // 
            // DeviceWarrantyDtoGridView
            // 
            this.DeviceWarrantyDtoGridView.Appearance.ViewCaption.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.DeviceWarrantyDtoGridView.Appearance.ViewCaption.ForeColor = DevExpress.LookAndFeel.DXSkinColors.ForeColors.Information;
            this.DeviceWarrantyDtoGridView.Appearance.ViewCaption.Options.UseFont = true;
            this.DeviceWarrantyDtoGridView.Appearance.ViewCaption.Options.UseForeColor = true;
            this.DeviceWarrantyDtoGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colWarrantyInfoHtml,
            this.colDeviceHtmlInfo});
            this.DeviceWarrantyDtoGridView.GridControl = this.DeviceWarrantyGridViewGridControl;
            this.DeviceWarrantyDtoGridView.Name = "DeviceWarrantyDtoGridView";
            this.DeviceWarrantyDtoGridView.OptionsSelection.MultiSelect = true;
            this.DeviceWarrantyDtoGridView.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            this.DeviceWarrantyDtoGridView.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            this.DeviceWarrantyDtoGridView.OptionsView.RowAutoHeight = true;
            this.DeviceWarrantyDtoGridView.OptionsView.ShowGroupPanel = false;
            this.DeviceWarrantyDtoGridView.OptionsView.ShowViewCaption = true;
            this.DeviceWarrantyDtoGridView.ViewCaption = "QUẢN LÝ TÀI SẢN - THIẾT BỊ";
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
            this.colWarrantyInfoHtml.FieldName = "WarrantyInfoHtml";
            this.colWarrantyInfoHtml.Name = "colWarrantyInfoHtml";
            this.colWarrantyInfoHtml.Visible = true;
            this.colWarrantyInfoHtml.VisibleIndex = 2;
            this.colWarrantyInfoHtml.Width = 400;
            // 
            // HtmlRepositoryItemHypertextLabel
            // 
            this.HtmlRepositoryItemHypertextLabel.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.HtmlRepositoryItemHypertextLabel.Name = "HtmlRepositoryItemHypertextLabel";
            // 
            // colDeviceHtmlInfo
            // 
            this.colDeviceHtmlInfo.ColumnEdit = this.HtmlRepositoryItemHypertextLabel;
            this.colDeviceHtmlInfo.FieldName = "ProductVariantName";
            this.colDeviceHtmlInfo.Name = "colDeviceHtmlInfo";
            this.colDeviceHtmlInfo.Visible = true;
            this.colDeviceHtmlInfo.VisibleIndex = 1;
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
            this.Controls.Add(this.dockPanel1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "FrmDeviceWarranty";
            this.Text = "QUẢN LÝ BẢO HÀNH";
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).EndInit();
            this.dockPanel1.ResumeLayout(false);
            this.dockPanel1_Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DeviceWarrantyGridViewGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.warrantyDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DeviceWarrantyDtoGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HtmlRepositoryItemHypertextLabel)).EndInit();
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
        private DevExpress.XtraGrid.Views.Grid.GridView DeviceWarrantyDtoGridView;
        private DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel HtmlRepositoryItemHypertextLabel;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox StatusComboBox;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit NotesMemoEdit;
        private DevExpress.XtraBars.Docking.DockManager dockManager1;
        private DevExpress.XtraBars.Docking.DockPanel dockPanel1;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel1_Container;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private UcDeviceWarranty ucDeviceWarranty1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraBars.BarSubItem barSubItem2;
        private DevExpress.XtraBars.Bar bar3;
        private System.Windows.Forms.BindingSource warrantyDtoBindingSource;
        private DevExpress.XtraGrid.Columns.GridColumn colWarrantyInfoHtml;
        private DevExpress.XtraGrid.Columns.GridColumn colDeviceHtmlInfo;
    }
}