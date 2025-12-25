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
            this.DeviceWarrantyDtoGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colWarrantyInfoHtml = new DevExpress.XtraGrid.Columns.GridColumn();
            this.HtmlRepositoryItemHypertextLabel = new DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel();
            this.colWarrantyInfoHtml1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.NotesMemoEdit = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
            this.StatusComboBox = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.repositoryItemDateEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DeviceWarrantyGridViewGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.warrantyDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DeviceWarrantyDtoGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HtmlRepositoryItemHypertextLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NotesMemoEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StatusComboBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1.CalendarTimeProperties)).BeginInit();
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
            this.DeviceWarrantyGridViewGridControl.MainView = this.DeviceWarrantyDtoGridView;
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
            this.colWarrantyInfoHtml1});
            this.DeviceWarrantyDtoGridView.GridControl = this.DeviceWarrantyGridViewGridControl;
            this.DeviceWarrantyDtoGridView.Name = "DeviceWarrantyDtoGridView";
            this.DeviceWarrantyDtoGridView.OptionsSelection.MultiSelect = true;
            this.DeviceWarrantyDtoGridView.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            this.DeviceWarrantyDtoGridView.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            this.DeviceWarrantyDtoGridView.OptionsView.RowAutoHeight = true;
            this.DeviceWarrantyDtoGridView.OptionsView.ShowGroupPanel = false;
            this.DeviceWarrantyDtoGridView.OptionsView.ShowViewCaption = true;
            this.DeviceWarrantyDtoGridView.ViewCaption = "QUẢN LÝ BẢO HÀNH TÀI SẢN - THIẾT BỊ";
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
            this.colWarrantyInfoHtml.Caption = "Thông tin thiết bị";
            this.colWarrantyInfoHtml.ColumnEdit = this.HtmlRepositoryItemHypertextLabel;
            this.colWarrantyInfoHtml.FieldName = "ProductVariantName";
            this.colWarrantyInfoHtml.Name = "colWarrantyInfoHtml";
            this.colWarrantyInfoHtml.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
            this.colWarrantyInfoHtml.Visible = true;
            this.colWarrantyInfoHtml.VisibleIndex = 1;
            this.colWarrantyInfoHtml.Width = 355;
            // 
            // HtmlRepositoryItemHypertextLabel
            // 
            this.HtmlRepositoryItemHypertextLabel.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.HtmlRepositoryItemHypertextLabel.Name = "HtmlRepositoryItemHypertextLabel";
            // 
            // colWarrantyInfoHtml1
            // 
            this.colWarrantyInfoHtml1.AppearanceCell.Options.UseTextOptions = true;
            this.colWarrantyInfoHtml1.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colWarrantyInfoHtml1.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colWarrantyInfoHtml1.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colWarrantyInfoHtml1.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colWarrantyInfoHtml1.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colWarrantyInfoHtml1.AppearanceHeader.Options.UseBackColor = true;
            this.colWarrantyInfoHtml1.AppearanceHeader.Options.UseFont = true;
            this.colWarrantyInfoHtml1.AppearanceHeader.Options.UseForeColor = true;
            this.colWarrantyInfoHtml1.AppearanceHeader.Options.UseTextOptions = true;
            this.colWarrantyInfoHtml1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colWarrantyInfoHtml1.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colWarrantyInfoHtml1.Caption = "Thông tin bảo hành";
            this.colWarrantyInfoHtml1.ColumnEdit = this.HtmlRepositoryItemHypertextLabel;
            this.colWarrantyInfoHtml1.FieldName = "WarrantyInfoHtml";
            this.colWarrantyInfoHtml1.Name = "colWarrantyInfoHtml1";
            this.colWarrantyInfoHtml1.Visible = true;
            this.colWarrantyInfoHtml1.VisibleIndex = 2;
            this.colWarrantyInfoHtml1.Width = 568;
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
            ((System.ComponentModel.ISupportInitialize)(this.DeviceWarrantyDtoGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HtmlRepositoryItemHypertextLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NotesMemoEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StatusComboBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1)).EndInit();
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
        private DevExpress.XtraGrid.Views.Grid.GridView DeviceWarrantyDtoGridView;
        private DevExpress.XtraGrid.Columns.GridColumn colWarrantyInfoHtml;
        private DevExpress.XtraGrid.Columns.GridColumn colWarrantyInfoHtml1;
    }
}