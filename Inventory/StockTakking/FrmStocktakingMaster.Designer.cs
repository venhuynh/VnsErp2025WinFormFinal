namespace Inventory.StockTakking
{
    partial class FrmStocktakingMaster
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
            this.ProductVariantIdentifierDtoGridControl = new DevExpress.XtraGrid.GridControl();
            this.stocktakingMasterDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ProductVariantIdentifierDtoGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colIsLocked = new DevExpress.XtraGrid.Columns.GridColumn();
            this.IsActiveCheckEdit = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.colQuyTrinhHtml = new DevExpress.XtraGrid.Columns.GridColumn();
            this.HtmlRepositoryItemHypertextLabel = new DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel();
            this.colThongTinHtml = new DevExpress.XtraGrid.Columns.GridColumn();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.LoadDataBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.AddNewBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.EditBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem4 = new DevExpress.XtraBars.BarButtonItem();
            this.ExportFileBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.barHeaderItem1 = new DevExpress.XtraBars.BarHeaderItem();
            this.DataSummaryBarStaticItem = new DevExpress.XtraBars.BarStaticItem();
            this.SelectedRowBarStaticItem = new DevExpress.XtraBars.BarStaticItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.NamRepositoryItemSpinEdit = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.ThangRepositoryItemSpinEdit = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.QRCodeImagePictureEdit = new DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit();
            this.ProductVariantIdentifierStatusEnumComboBox = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.NotesMemoEdit = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ProductVariantIdentifierDtoGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stocktakingMasterDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductVariantIdentifierDtoGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.IsActiveCheckEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HtmlRepositoryItemHypertextLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NamRepositoryItemSpinEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ThangRepositoryItemSpinEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.QRCodeImagePictureEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductVariantIdentifierStatusEnumComboBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NotesMemoEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.ProductVariantIdentifierDtoGridControl);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 24);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.Root;
            this.layoutControl1.Size = new System.Drawing.Size(1357, 486);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // ProductVariantIdentifierDtoGridControl
            // 
            this.ProductVariantIdentifierDtoGridControl.DataSource = this.stocktakingMasterDtoBindingSource;
            this.ProductVariantIdentifierDtoGridControl.Location = new System.Drawing.Point(12, 12);
            this.ProductVariantIdentifierDtoGridControl.MainView = this.ProductVariantIdentifierDtoGridView;
            this.ProductVariantIdentifierDtoGridControl.MenuManager = this.barManager1;
            this.ProductVariantIdentifierDtoGridControl.Name = "ProductVariantIdentifierDtoGridControl";
            this.ProductVariantIdentifierDtoGridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.HtmlRepositoryItemHypertextLabel,
            this.QRCodeImagePictureEdit,
            this.ProductVariantIdentifierStatusEnumComboBox,
            this.NotesMemoEdit,
            this.IsActiveCheckEdit});
            this.ProductVariantIdentifierDtoGridControl.Size = new System.Drawing.Size(1333, 462);
            this.ProductVariantIdentifierDtoGridControl.TabIndex = 4;
            this.ProductVariantIdentifierDtoGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.ProductVariantIdentifierDtoGridView});
            // 
            // stocktakingMasterDtoBindingSource
            // 
            this.stocktakingMasterDtoBindingSource.DataSource = typeof(DTO.Inventory.StockTakking.StocktakingMasterDto);
            // 
            // ProductVariantIdentifierDtoGridView
            // 
            this.ProductVariantIdentifierDtoGridView.Appearance.ViewCaption.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.ProductVariantIdentifierDtoGridView.Appearance.ViewCaption.ForeColor = DevExpress.LookAndFeel.DXSkinColors.ForeColors.Information;
            this.ProductVariantIdentifierDtoGridView.Appearance.ViewCaption.Options.UseFont = true;
            this.ProductVariantIdentifierDtoGridView.Appearance.ViewCaption.Options.UseForeColor = true;
            this.ProductVariantIdentifierDtoGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colIsLocked,
            this.colQuyTrinhHtml,
            this.colThongTinHtml});
            this.ProductVariantIdentifierDtoGridView.GridControl = this.ProductVariantIdentifierDtoGridControl;
            this.ProductVariantIdentifierDtoGridView.IndicatorWidth = 50;
            this.ProductVariantIdentifierDtoGridView.Name = "ProductVariantIdentifierDtoGridView";
            this.ProductVariantIdentifierDtoGridView.OptionsFind.AlwaysVisible = true;
            this.ProductVariantIdentifierDtoGridView.OptionsSelection.MultiSelect = true;
            this.ProductVariantIdentifierDtoGridView.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            this.ProductVariantIdentifierDtoGridView.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            this.ProductVariantIdentifierDtoGridView.OptionsView.RowAutoHeight = true;
            this.ProductVariantIdentifierDtoGridView.OptionsView.ShowFooter = true;
            this.ProductVariantIdentifierDtoGridView.OptionsView.ShowGroupPanel = false;
            this.ProductVariantIdentifierDtoGridView.OptionsView.ShowViewCaption = true;
            this.ProductVariantIdentifierDtoGridView.ViewCaption = "BẢNG KIỂM KHO";
            // 
            // colIsLocked
            // 
            this.colIsLocked.AppearanceCell.Options.UseTextOptions = true;
            this.colIsLocked.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colIsLocked.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colIsLocked.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colIsLocked.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colIsLocked.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colIsLocked.AppearanceHeader.Options.UseBackColor = true;
            this.colIsLocked.AppearanceHeader.Options.UseFont = true;
            this.colIsLocked.AppearanceHeader.Options.UseForeColor = true;
            this.colIsLocked.AppearanceHeader.Options.UseTextOptions = true;
            this.colIsLocked.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colIsLocked.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colIsLocked.Caption = "Khóa";
            this.colIsLocked.ColumnEdit = this.IsActiveCheckEdit;
            this.colIsLocked.FieldName = "IsLocked";
            this.colIsLocked.Name = "colIsLocked";
            this.colIsLocked.Visible = true;
            this.colIsLocked.VisibleIndex = 3;
            this.colIsLocked.Width = 100;
            // 
            // IsActiveCheckEdit
            // 
            this.IsActiveCheckEdit.Name = "IsActiveCheckEdit";
            // 
            // colQuyTrinhHtml
            // 
            this.colQuyTrinhHtml.AppearanceCell.Options.UseTextOptions = true;
            this.colQuyTrinhHtml.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colQuyTrinhHtml.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colQuyTrinhHtml.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colQuyTrinhHtml.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colQuyTrinhHtml.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colQuyTrinhHtml.AppearanceHeader.Options.UseBackColor = true;
            this.colQuyTrinhHtml.AppearanceHeader.Options.UseFont = true;
            this.colQuyTrinhHtml.AppearanceHeader.Options.UseForeColor = true;
            this.colQuyTrinhHtml.AppearanceHeader.Options.UseTextOptions = true;
            this.colQuyTrinhHtml.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colQuyTrinhHtml.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colQuyTrinhHtml.Caption = "Quy trình";
            this.colQuyTrinhHtml.ColumnEdit = this.HtmlRepositoryItemHypertextLabel;
            this.colQuyTrinhHtml.FieldName = "QuyTrinhHtml";
            this.colQuyTrinhHtml.Name = "colQuyTrinhHtml";
            this.colQuyTrinhHtml.OptionsColumn.AllowEdit = false;
            this.colQuyTrinhHtml.OptionsColumn.ReadOnly = true;
            this.colQuyTrinhHtml.Visible = true;
            this.colQuyTrinhHtml.VisibleIndex = 2;
            this.colQuyTrinhHtml.Width = 400;
            // 
            // HtmlRepositoryItemHypertextLabel
            // 
            this.HtmlRepositoryItemHypertextLabel.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.HtmlRepositoryItemHypertextLabel.Name = "HtmlRepositoryItemHypertextLabel";
            // 
            // colThongTinHtml
            // 
            this.colThongTinHtml.AppearanceCell.Options.UseTextOptions = true;
            this.colThongTinHtml.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colThongTinHtml.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colThongTinHtml.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colThongTinHtml.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colThongTinHtml.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colThongTinHtml.AppearanceHeader.Options.UseBackColor = true;
            this.colThongTinHtml.AppearanceHeader.Options.UseFont = true;
            this.colThongTinHtml.AppearanceHeader.Options.UseForeColor = true;
            this.colThongTinHtml.AppearanceHeader.Options.UseTextOptions = true;
            this.colThongTinHtml.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colThongTinHtml.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colThongTinHtml.Caption = "Thông tin";
            this.colThongTinHtml.ColumnEdit = this.HtmlRepositoryItemHypertextLabel;
            this.colThongTinHtml.FieldName = "ThongTinHtml";
            this.colThongTinHtml.Name = "colThongTinHtml";
            this.colThongTinHtml.OptionsColumn.AllowEdit = false;
            this.colThongTinHtml.OptionsColumn.ReadOnly = true;
            this.colThongTinHtml.Visible = true;
            this.colThongTinHtml.VisibleIndex = 1;
            this.colThongTinHtml.Width = 400;
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
            this.barHeaderItem1,
            this.DataSummaryBarStaticItem,
            this.SelectedRowBarStaticItem,
            this.ExportFileBarButtonItem,
            this.AddNewBarButtonItem,
            this.EditBarButtonItem,
            this.barButtonItem4,
            this.LoadDataBarButtonItem});
            this.barManager1.MainMenu = this.bar2;
            this.barManager1.MaxItemId = 34;
            this.barManager1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.NamRepositoryItemSpinEdit,
            this.ThangRepositoryItemSpinEdit});
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
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.LoadDataBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.AddNewBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.EditBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItem4, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.ExportFileBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar2.OptionsBar.MultiLine = true;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Main menu";
            // 
            // LoadDataBarButtonItem
            // 
            this.LoadDataBarButtonItem.Caption = "Danh sách";
            this.LoadDataBarButtonItem.Id = 32;
            this.LoadDataBarButtonItem.ImageOptions.Image = global::Inventory.Properties.Resources.listnumbers_16x16;
            this.LoadDataBarButtonItem.Name = "LoadDataBarButtonItem";
            // 
            // AddNewBarButtonItem
            // 
            this.AddNewBarButtonItem.Caption = "Thêm mới";
            this.AddNewBarButtonItem.Id = 23;
            this.AddNewBarButtonItem.ImageOptions.Image = global::Inventory.Properties.Resources.add_16x16;
            this.AddNewBarButtonItem.Name = "AddNewBarButtonItem";
            // 
            // EditBarButtonItem
            // 
            this.EditBarButtonItem.Caption = "Điều chỉnh";
            this.EditBarButtonItem.Id = 24;
            this.EditBarButtonItem.ImageOptions.Image = global::Inventory.Properties.Resources.edit_16x16;
            this.EditBarButtonItem.Name = "EditBarButtonItem";
            // 
            // barButtonItem4
            // 
            this.barButtonItem4.Caption = "Xóa";
            this.barButtonItem4.Id = 25;
            this.barButtonItem4.ImageOptions.Image = global::Inventory.Properties.Resources.clear_16x16;
            this.barButtonItem4.Name = "barButtonItem4";
            // 
            // ExportFileBarButtonItem
            // 
            this.ExportFileBarButtonItem.Caption = "Xuất file";
            this.ExportFileBarButtonItem.Id = 18;
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
            this.barDockControlTop.Size = new System.Drawing.Size(1357, 24);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 510);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(1357, 22);
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
            this.barDockControlRight.Location = new System.Drawing.Point(1357, 24);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 486);
            // 
            // NamRepositoryItemSpinEdit
            // 
            this.NamRepositoryItemSpinEdit.AutoHeight = false;
            this.NamRepositoryItemSpinEdit.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.NamRepositoryItemSpinEdit.MaskSettings.Set("mask", "d");
            this.NamRepositoryItemSpinEdit.MaxValue = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.NamRepositoryItemSpinEdit.MinValue = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.NamRepositoryItemSpinEdit.Name = "NamRepositoryItemSpinEdit";
            // 
            // ThangRepositoryItemSpinEdit
            // 
            this.ThangRepositoryItemSpinEdit.AutoHeight = false;
            this.ThangRepositoryItemSpinEdit.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ThangRepositoryItemSpinEdit.MaskSettings.Set("mask", "d");
            this.ThangRepositoryItemSpinEdit.MaxValue = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.ThangRepositoryItemSpinEdit.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ThangRepositoryItemSpinEdit.Name = "ThangRepositoryItemSpinEdit";
            // 
            // QRCodeImagePictureEdit
            // 
            this.QRCodeImagePictureEdit.AllowFocused = false;
            this.QRCodeImagePictureEdit.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.QRCodeImagePictureEdit.CustomHeight = 60;
            this.QRCodeImagePictureEdit.Name = "QRCodeImagePictureEdit";
            this.QRCodeImagePictureEdit.NullText = " ";
            this.QRCodeImagePictureEdit.PictureInterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            this.QRCodeImagePictureEdit.ShowMenu = false;
            this.QRCodeImagePictureEdit.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            // 
            // ProductVariantIdentifierStatusEnumComboBox
            // 
            this.ProductVariantIdentifierStatusEnumComboBox.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.ProductVariantIdentifierStatusEnumComboBox.AutoHeight = false;
            this.ProductVariantIdentifierStatusEnumComboBox.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ProductVariantIdentifierStatusEnumComboBox.ContextButtonOptions.AllowHtmlText = true;
            this.ProductVariantIdentifierStatusEnumComboBox.Name = "ProductVariantIdentifierStatusEnumComboBox";
            this.ProductVariantIdentifierStatusEnumComboBox.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            // 
            // NotesMemoEdit
            // 
            this.NotesMemoEdit.Name = "NotesMemoEdit";
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(1357, 486);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.ProductVariantIdentifierDtoGridControl;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(1337, 466);
            this.layoutControlItem1.TextVisible = false;
            // 
            // FrmStocktakingMaster
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1357, 532);
            this.Controls.Add(this.layoutControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "FrmStocktakingMaster";
            this.Text = "Bảng kiểm kho";
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ProductVariantIdentifierDtoGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stocktakingMasterDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductVariantIdentifierDtoGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.IsActiveCheckEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HtmlRepositoryItemHypertextLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NamRepositoryItemSpinEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ThangRepositoryItemSpinEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.QRCodeImagePictureEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductVariantIdentifierStatusEnumComboBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NotesMemoEdit)).EndInit();
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
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraGrid.GridControl ProductVariantIdentifierDtoGridControl;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit NamRepositoryItemSpinEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit ThangRepositoryItemSpinEdit;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarHeaderItem barHeaderItem1;
        private DevExpress.XtraBars.BarStaticItem DataSummaryBarStaticItem;
        private DevExpress.XtraBars.BarStaticItem SelectedRowBarStaticItem;
        private DevExpress.XtraBars.BarButtonItem ExportFileBarButtonItem;
        private DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel HtmlRepositoryItemHypertextLabel;
        private DevExpress.XtraGrid.Views.Grid.GridView ProductVariantIdentifierDtoGridView;
        private DevExpress.XtraBars.BarButtonItem AddNewBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem EditBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem barButtonItem4;
        private DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit QRCodeImagePictureEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox ProductVariantIdentifierStatusEnumComboBox;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit NotesMemoEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit IsActiveCheckEdit;
        private DevExpress.XtraBars.BarButtonItem LoadDataBarButtonItem;
        private System.Windows.Forms.BindingSource stocktakingMasterDtoBindingSource;
        private DevExpress.XtraGrid.Columns.GridColumn colIsLocked;
        private DevExpress.XtraGrid.Columns.GridColumn colQuyTrinhHtml;
        private DevExpress.XtraGrid.Columns.GridColumn colThongTinHtml;
    }
}