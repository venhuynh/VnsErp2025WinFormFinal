namespace Inventory.ProductVariantIdentifier
{
    partial class FrmProductVariantIdentifier
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
            this.productVariantIdentifierDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ProductVariantIdentifierDtoGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colQRCodeImage = new DevExpress.XtraGrid.Columns.GridColumn();
            this.QRCodeImagePictureEdit = new DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit();
            this.colProductVariantFullName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.HtmlRepositoryItemHypertextLabel = new DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel();
            this.colIdentifiersHtml = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colStatus = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUpdatedDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colIsActive = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNotes = new DevExpress.XtraGrid.Columns.GridColumn();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.LocToanBoBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.ExportFileBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem3 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem4 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem5 = new DevExpress.XtraBars.BarButtonItem();
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
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.ProductVariantIdentifierStatusEnumComboBox = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ProductVariantIdentifierDtoGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.productVariantIdentifierDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductVariantIdentifierDtoGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.QRCodeImagePictureEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HtmlRepositoryItemHypertextLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NamRepositoryItemSpinEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ThangRepositoryItemSpinEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductVariantIdentifierStatusEnumComboBox)).BeginInit();
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
            this.ProductVariantIdentifierDtoGridControl.DataSource = this.productVariantIdentifierDtoBindingSource;
            this.ProductVariantIdentifierDtoGridControl.Location = new System.Drawing.Point(12, 12);
            this.ProductVariantIdentifierDtoGridControl.MainView = this.ProductVariantIdentifierDtoGridView;
            this.ProductVariantIdentifierDtoGridControl.MenuManager = this.barManager1;
            this.ProductVariantIdentifierDtoGridControl.Name = "ProductVariantIdentifierDtoGridControl";
            this.ProductVariantIdentifierDtoGridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.HtmlRepositoryItemHypertextLabel,
            this.QRCodeImagePictureEdit,
            this.ProductVariantIdentifierStatusEnumComboBox});
            this.ProductVariantIdentifierDtoGridControl.Size = new System.Drawing.Size(1333, 462);
            this.ProductVariantIdentifierDtoGridControl.TabIndex = 4;
            this.ProductVariantIdentifierDtoGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.ProductVariantIdentifierDtoGridView});
            // 
            // productVariantIdentifierDtoBindingSource
            // 
            this.productVariantIdentifierDtoBindingSource.DataSource = typeof(DTO.Inventory.InventoryManagement.ProductVariantIdentifierDto);
            // 
            // ProductVariantIdentifierDtoGridView
            // 
            this.ProductVariantIdentifierDtoGridView.Appearance.ViewCaption.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.ProductVariantIdentifierDtoGridView.Appearance.ViewCaption.ForeColor = DevExpress.LookAndFeel.DXSkinColors.ForeColors.Information;
            this.ProductVariantIdentifierDtoGridView.Appearance.ViewCaption.Options.UseFont = true;
            this.ProductVariantIdentifierDtoGridView.Appearance.ViewCaption.Options.UseForeColor = true;
            this.ProductVariantIdentifierDtoGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colQRCodeImage,
            this.colProductVariantFullName,
            this.colIdentifiersHtml,
            this.colStatus,
            this.colUpdatedDate,
            this.colIsActive,
            this.colNotes});
            this.ProductVariantIdentifierDtoGridView.GridControl = this.ProductVariantIdentifierDtoGridControl;
            this.ProductVariantIdentifierDtoGridView.IndicatorWidth = 50;
            this.ProductVariantIdentifierDtoGridView.Name = "ProductVariantIdentifierDtoGridView";
            this.ProductVariantIdentifierDtoGridView.OptionsBehavior.Editable = false;
            this.ProductVariantIdentifierDtoGridView.OptionsFind.AlwaysVisible = true;
            this.ProductVariantIdentifierDtoGridView.OptionsSelection.MultiSelect = true;
            this.ProductVariantIdentifierDtoGridView.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            this.ProductVariantIdentifierDtoGridView.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            this.ProductVariantIdentifierDtoGridView.OptionsView.RowAutoHeight = true;
            this.ProductVariantIdentifierDtoGridView.OptionsView.ShowFooter = true;
            this.ProductVariantIdentifierDtoGridView.OptionsView.ShowGroupPanel = false;
            this.ProductVariantIdentifierDtoGridView.OptionsView.ShowViewCaption = true;
            this.ProductVariantIdentifierDtoGridView.ViewCaption = "BẢNG ĐỊNH DANH SẢN PHẨM HÀNG HÓA";
            // 
            // colQRCodeImage
            // 
            this.colQRCodeImage.AppearanceCell.Options.UseTextOptions = true;
            this.colQRCodeImage.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colQRCodeImage.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colQRCodeImage.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colQRCodeImage.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colQRCodeImage.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colQRCodeImage.AppearanceHeader.Options.UseBackColor = true;
            this.colQRCodeImage.AppearanceHeader.Options.UseFont = true;
            this.colQRCodeImage.AppearanceHeader.Options.UseForeColor = true;
            this.colQRCodeImage.AppearanceHeader.Options.UseTextOptions = true;
            this.colQRCodeImage.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colQRCodeImage.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colQRCodeImage.Caption = "Ảnh QR";
            this.colQRCodeImage.ColumnEdit = this.QRCodeImagePictureEdit;
            this.colQRCodeImage.FieldName = "QRCodeImage";
            this.colQRCodeImage.MaxWidth = 120;
            this.colQRCodeImage.MinWidth = 120;
            this.colQRCodeImage.Name = "colQRCodeImage";
            this.colQRCodeImage.OptionsColumn.FixedWidth = true;
            this.colQRCodeImage.Visible = true;
            this.colQRCodeImage.VisibleIndex = 1;
            this.colQRCodeImage.Width = 120;
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
            // colProductVariantFullName
            // 
            this.colProductVariantFullName.AppearanceCell.Options.UseTextOptions = true;
            this.colProductVariantFullName.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colProductVariantFullName.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colProductVariantFullName.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colProductVariantFullName.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colProductVariantFullName.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colProductVariantFullName.AppearanceHeader.Options.UseBackColor = true;
            this.colProductVariantFullName.AppearanceHeader.Options.UseFont = true;
            this.colProductVariantFullName.AppearanceHeader.Options.UseForeColor = true;
            this.colProductVariantFullName.AppearanceHeader.Options.UseTextOptions = true;
            this.colProductVariantFullName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colProductVariantFullName.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colProductVariantFullName.Caption = "Sản phẩm";
            this.colProductVariantFullName.ColumnEdit = this.HtmlRepositoryItemHypertextLabel;
            this.colProductVariantFullName.FieldName = "ProductVariantFullName";
            this.colProductVariantFullName.Name = "colProductVariantFullName";
            this.colProductVariantFullName.OptionsColumn.AllowEdit = false;
            this.colProductVariantFullName.OptionsColumn.ReadOnly = true;
            this.colProductVariantFullName.Visible = true;
            this.colProductVariantFullName.VisibleIndex = 2;
            this.colProductVariantFullName.Width = 300;
            // 
            // HtmlRepositoryItemHypertextLabel
            // 
            this.HtmlRepositoryItemHypertextLabel.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.HtmlRepositoryItemHypertextLabel.Name = "HtmlRepositoryItemHypertextLabel";
            // 
            // colIdentifiersHtml
            // 
            this.colIdentifiersHtml.AppearanceCell.Options.UseTextOptions = true;
            this.colIdentifiersHtml.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colIdentifiersHtml.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colIdentifiersHtml.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colIdentifiersHtml.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colIdentifiersHtml.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colIdentifiersHtml.AppearanceHeader.Options.UseBackColor = true;
            this.colIdentifiersHtml.AppearanceHeader.Options.UseFont = true;
            this.colIdentifiersHtml.AppearanceHeader.Options.UseForeColor = true;
            this.colIdentifiersHtml.AppearanceHeader.Options.UseTextOptions = true;
            this.colIdentifiersHtml.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colIdentifiersHtml.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colIdentifiersHtml.Caption = "Định danh";
            this.colIdentifiersHtml.ColumnEdit = this.HtmlRepositoryItemHypertextLabel;
            this.colIdentifiersHtml.FieldName = "IdentifiersHtml";
            this.colIdentifiersHtml.Name = "colIdentifiersHtml";
            this.colIdentifiersHtml.OptionsColumn.AllowEdit = false;
            this.colIdentifiersHtml.OptionsColumn.ReadOnly = true;
            this.colIdentifiersHtml.Visible = true;
            this.colIdentifiersHtml.VisibleIndex = 3;
            this.colIdentifiersHtml.Width = 400;
            // 
            // colStatus
            // 
            this.colStatus.AppearanceCell.Options.UseTextOptions = true;
            this.colStatus.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colStatus.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colStatus.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colStatus.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colStatus.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colStatus.AppearanceHeader.Options.UseBackColor = true;
            this.colStatus.AppearanceHeader.Options.UseFont = true;
            this.colStatus.AppearanceHeader.Options.UseForeColor = true;
            this.colStatus.AppearanceHeader.Options.UseTextOptions = true;
            this.colStatus.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colStatus.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colStatus.Caption = "Tình trạng";
            this.colStatus.ColumnEdit = this.ProductVariantIdentifierStatusEnumComboBox;
            this.colStatus.FieldName = "Status";
            this.colStatus.Name = "colStatus";
            this.colStatus.OptionsColumn.ReadOnly = false;
            this.colStatus.OptionsColumn.AllowEdit = true;
            this.colStatus.Visible = true;
            this.colStatus.VisibleIndex = 4;
            this.colStatus.Width = 150;
            // 
            // colUpdatedDate
            // 
            this.colUpdatedDate.AppearanceCell.Options.UseTextOptions = true;
            this.colUpdatedDate.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colUpdatedDate.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colUpdatedDate.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colUpdatedDate.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colUpdatedDate.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colUpdatedDate.AppearanceHeader.Options.UseBackColor = true;
            this.colUpdatedDate.AppearanceHeader.Options.UseFont = true;
            this.colUpdatedDate.AppearanceHeader.Options.UseForeColor = true;
            this.colUpdatedDate.AppearanceHeader.Options.UseTextOptions = true;
            this.colUpdatedDate.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colUpdatedDate.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colUpdatedDate.Caption = "Ngày cập nhật";
            this.colUpdatedDate.DisplayFormat.FormatString = "dd/MM/yyyy HH:mm";
            this.colUpdatedDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.colUpdatedDate.FieldName = "UpdatedDate";
            this.colUpdatedDate.Name = "colUpdatedDate";
            this.colUpdatedDate.OptionsColumn.AllowEdit = false;
            this.colUpdatedDate.OptionsColumn.ReadOnly = true;
            this.colUpdatedDate.Visible = true;
            this.colUpdatedDate.VisibleIndex = 5;
            this.colUpdatedDate.Width = 150;
            // 
            // colIsActive
            // 
            this.colIsActive.AppearanceCell.Options.UseTextOptions = true;
            this.colIsActive.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colIsActive.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colIsActive.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colIsActive.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colIsActive.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colIsActive.AppearanceHeader.Options.UseBackColor = true;
            this.colIsActive.AppearanceHeader.Options.UseFont = true;
            this.colIsActive.AppearanceHeader.Options.UseForeColor = true;
            this.colIsActive.AppearanceHeader.Options.UseTextOptions = true;
            this.colIsActive.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colIsActive.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colIsActive.Caption = "Đang hoạt động";
            this.colIsActive.FieldName = "IsActive";
            this.colIsActive.Name = "colIsActive";
            this.colIsActive.OptionsColumn.AllowEdit = false;
            this.colIsActive.OptionsColumn.ReadOnly = true;
            this.colIsActive.Visible = true;
            this.colIsActive.VisibleIndex = 6;
            this.colIsActive.Width = 120;
            // 
            // colNotes
            // 
            this.colNotes.AppearanceCell.Options.UseTextOptions = true;
            this.colNotes.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colNotes.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colNotes.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colNotes.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colNotes.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colNotes.AppearanceHeader.Options.UseBackColor = true;
            this.colNotes.AppearanceHeader.Options.UseFont = true;
            this.colNotes.AppearanceHeader.Options.UseForeColor = true;
            this.colNotes.AppearanceHeader.Options.UseTextOptions = true;
            this.colNotes.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colNotes.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colNotes.Caption = "Ghi chú";
            this.colNotes.FieldName = "Notes";
            this.colNotes.Name = "colNotes";
            this.colNotes.OptionsColumn.AllowEdit = false;
            this.colNotes.OptionsColumn.ReadOnly = true;
            this.colNotes.Visible = true;
            this.colNotes.VisibleIndex = 7;
            this.colNotes.Width = 250;
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
            this.LocToanBoBarButtonItem,
            this.barHeaderItem1,
            this.DataSummaryBarStaticItem,
            this.SelectedRowBarStaticItem,
            this.ExportFileBarButtonItem,
            this.barButtonItem1,
            this.barButtonItem2,
            this.barButtonItem3,
            this.barButtonItem4,
            this.barButtonItem5});
            this.barManager1.MainMenu = this.bar2;
            this.barManager1.MaxItemId = 27;
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
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.LocToanBoBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.ExportFileBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem2),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem3),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem4),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem5)});
            this.bar2.OptionsBar.MultiLine = true;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Main menu";
            // 
            // LocToanBoBarButtonItem
            // 
            this.LocToanBoBarButtonItem.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True;
            this.LocToanBoBarButtonItem.Caption = "Toàn bộ";
            this.LocToanBoBarButtonItem.Id = 9;
            this.LocToanBoBarButtonItem.ImageOptions.Image = global::Inventory.Properties.Resources.filterbyseries_pie_16x16;
            this.LocToanBoBarButtonItem.ImageOptions.LargeImage = global::Inventory.Properties.Resources.filterbyseries_pie_32x32;
            this.LocToanBoBarButtonItem.Name = "LocToanBoBarButtonItem";
            // 
            // ExportFileBarButtonItem
            // 
            this.ExportFileBarButtonItem.Caption = "Xuất file";
            this.ExportFileBarButtonItem.Id = 18;
            this.ExportFileBarButtonItem.ImageOptions.Image = global::Inventory.Properties.Resources.exporttoxps_16x16;
            this.ExportFileBarButtonItem.Name = "ExportFileBarButtonItem";
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "LS Nhập/Xuất";
            this.barButtonItem1.Id = 22;
            this.barButtonItem1.Name = "barButtonItem1";
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.Caption = "Thêm mới";
            this.barButtonItem2.Id = 23;
            this.barButtonItem2.Name = "barButtonItem2";
            // 
            // barButtonItem3
            // 
            this.barButtonItem3.Caption = "Điều chỉnh";
            this.barButtonItem3.Id = 24;
            this.barButtonItem3.Name = "barButtonItem3";
            // 
            // barButtonItem4
            // 
            this.barButtonItem4.Caption = "Xóa";
            this.barButtonItem4.Id = 25;
            this.barButtonItem4.Name = "barButtonItem4";
            // 
            // barButtonItem5
            // 
            this.barButtonItem5.Caption = "In tem";
            this.barButtonItem5.Id = 26;
            this.barButtonItem5.Name = "barButtonItem5";
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
            // ProductVariantIdentifierStatusEnumComboBox
            // 
            this.ProductVariantIdentifierStatusEnumComboBox.AutoHeight = false;
            this.ProductVariantIdentifierStatusEnumComboBox.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ProductVariantIdentifierStatusEnumComboBox.Name = "ProductVariantIdentifierStatusEnumComboBox";
            // 
            // FrmProductVariantIdentifier
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1357, 532);
            this.Controls.Add(this.layoutControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "FrmProductVariantIdentifier";
            this.Text = "Bảng định danh SPHH";
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ProductVariantIdentifierDtoGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.productVariantIdentifierDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductVariantIdentifierDtoGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.QRCodeImagePictureEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HtmlRepositoryItemHypertextLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NamRepositoryItemSpinEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ThangRepositoryItemSpinEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductVariantIdentifierStatusEnumComboBox)).EndInit();
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
        private DevExpress.XtraBars.BarButtonItem LocToanBoBarButtonItem;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarHeaderItem barHeaderItem1;
        private DevExpress.XtraBars.BarStaticItem DataSummaryBarStaticItem;
        private DevExpress.XtraBars.BarStaticItem SelectedRowBarStaticItem;
        private DevExpress.XtraBars.BarButtonItem ExportFileBarButtonItem;
        private DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel HtmlRepositoryItemHypertextLabel;
        private System.Windows.Forms.BindingSource productVariantIdentifierDtoBindingSource;
        private DevExpress.XtraGrid.Views.Grid.GridView ProductVariantIdentifierDtoGridView;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraBars.BarButtonItem barButtonItem3;
        private DevExpress.XtraBars.BarButtonItem barButtonItem4;
        private DevExpress.XtraBars.BarButtonItem barButtonItem5;
        private DevExpress.XtraGrid.Columns.GridColumn colProductVariantFullName;
        private DevExpress.XtraGrid.Columns.GridColumn colIdentifiersHtml;
        private DevExpress.XtraGrid.Columns.GridColumn colStatus;
        private DevExpress.XtraGrid.Columns.GridColumn colUpdatedDate;
        private DevExpress.XtraGrid.Columns.GridColumn colIsActive;
        private DevExpress.XtraGrid.Columns.GridColumn colNotes;
        private DevExpress.XtraGrid.Columns.GridColumn colQRCodeImage;
        private DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit QRCodeImagePictureEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox ProductVariantIdentifierStatusEnumComboBox;
    }
}