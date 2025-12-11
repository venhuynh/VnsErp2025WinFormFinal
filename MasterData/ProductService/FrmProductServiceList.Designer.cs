using DevExpress.XtraBars;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout;
using System.ComponentModel;
using System.Windows.Forms;
using DevExpress.XtraEditors.Repository;

namespace MasterData.ProductService
{
    partial class FrmProductServiceList
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.ListDataBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.NewBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.EditBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.DeleteBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.CountVariantAndImageBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.ExportBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.barHeaderItem1 = new DevExpress.XtraBars.BarHeaderItem();
            this.DataSummaryBarStaticItem = new DevExpress.XtraBars.BarStaticItem();
            this.barHeaderItem2 = new DevExpress.XtraBars.BarHeaderItem();
            this.SelectedRowBarStaticItem = new DevExpress.XtraBars.BarStaticItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.repositoryItemComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.repositoryItemComboBox2 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.ProductServiceListGridControl = new DevExpress.XtraGrid.GridControl();
            this.productServiceDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ProductServiceGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colThongTinHtml = new DevExpress.XtraGrid.Columns.GridColumn();
            this.HtmlHypertextLabel = new DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel();
            this.colThumbnailImage = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ProductThumbnailRepositoryItemPictureEdit = new DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit();
            this.colCategoryFullPathHtml = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ProductServiceListGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.productServiceDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductServiceGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HtmlHypertextLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductThumbnailRepositoryItemPictureEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            this.SuspendLayout();
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
            this.NewBarButtonItem,
            this.ListDataBarButtonItem,
            this.EditBarButtonItem,
            this.DeleteBarButtonItem,
            this.ExportBarButtonItem,
            this.CountVariantAndImageBarButtonItem,
            this.barHeaderItem1,
            this.DataSummaryBarStaticItem,
            this.barHeaderItem2,
            this.SelectedRowBarStaticItem});
            this.barManager1.MainMenu = this.bar2;
            this.barManager1.MaxItemId = 13;
            this.barManager1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemComboBox1,
            this.repositoryItemComboBox2});
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
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.ListDataBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.NewBarButtonItem, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.EditBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.DeleteBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.CountVariantAndImageBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.ExportBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar2.OptionsBar.MultiLine = true;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Main menu";
            // 
            // ListDataBarButtonItem
            // 
            this.ListDataBarButtonItem.Caption = "Danh sách";
            this.ListDataBarButtonItem.Id = 1;
            this.ListDataBarButtonItem.ImageOptions.Image = global::MasterData.Properties.Resources.list_16x16;
            this.ListDataBarButtonItem.ImageOptions.LargeImage = global::MasterData.Properties.Resources.list_32x32;
            this.ListDataBarButtonItem.Name = "ListDataBarButtonItem";
            // 
            // NewBarButtonItem
            // 
            this.NewBarButtonItem.Caption = "Mới";
            this.NewBarButtonItem.Id = 0;
            this.NewBarButtonItem.ImageOptions.Image = global::MasterData.Properties.Resources.addnewdatasource_16x16;
            this.NewBarButtonItem.ImageOptions.LargeImage = global::MasterData.Properties.Resources.addnewdatasource_32x32;
            this.NewBarButtonItem.Name = "NewBarButtonItem";
            // 
            // EditBarButtonItem
            // 
            this.EditBarButtonItem.Caption = "Điều chỉnh";
            this.EditBarButtonItem.Id = 2;
            this.EditBarButtonItem.ImageOptions.Image = global::MasterData.Properties.Resources.edittask_16x16;
            this.EditBarButtonItem.ImageOptions.LargeImage = global::MasterData.Properties.Resources.edittask_32x32;
            this.EditBarButtonItem.Name = "EditBarButtonItem";
            // 
            // DeleteBarButtonItem
            // 
            this.DeleteBarButtonItem.Caption = "Xóa";
            this.DeleteBarButtonItem.Id = 3;
            this.DeleteBarButtonItem.ImageOptions.Image = global::MasterData.Properties.Resources.deletelist_16x16;
            this.DeleteBarButtonItem.ImageOptions.LargeImage = global::MasterData.Properties.Resources.deletelist_32x32;
            this.DeleteBarButtonItem.Name = "DeleteBarButtonItem";
            // 
            // CountVariantAndImageBarButtonItem
            // 
            this.CountVariantAndImageBarButtonItem.Caption = "Đếm số lượng";
            this.CountVariantAndImageBarButtonItem.Id = 5;
            this.CountVariantAndImageBarButtonItem.ImageOptions.Image = global::MasterData.Properties.Resources.list_16x16;
            this.CountVariantAndImageBarButtonItem.ImageOptions.LargeImage = global::MasterData.Properties.Resources.list_32x32;
            this.CountVariantAndImageBarButtonItem.Name = "CountVariantAndImageBarButtonItem";
            // 
            // ExportBarButtonItem
            // 
            this.ExportBarButtonItem.Caption = "Xuất";
            this.ExportBarButtonItem.Id = 4;
            this.ExportBarButtonItem.ImageOptions.Image = global::MasterData.Properties.Resources.exporttoxls_16x16;
            this.ExportBarButtonItem.ImageOptions.LargeImage = global::MasterData.Properties.Resources.exporttoxls_32x32;
            this.ExportBarButtonItem.Name = "ExportBarButtonItem";
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
            new DevExpress.XtraBars.LinkPersistInfo(this.barHeaderItem2),
            new DevExpress.XtraBars.LinkPersistInfo(this.SelectedRowBarStaticItem)});
            this.bar1.OptionsBar.AllowQuickCustomization = false;
            this.bar1.OptionsBar.DrawDragBorder = false;
            this.bar1.OptionsBar.UseWholeRow = true;
            this.bar1.Text = "Custom 3";
            // 
            // barHeaderItem1
            // 
            this.barHeaderItem1.Caption = "Tổng kết:";
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
            // barHeaderItem2
            // 
            this.barHeaderItem2.Caption = "Đang chọn:";
            this.barHeaderItem2.Id = 8;
            this.barHeaderItem2.Name = "barHeaderItem2";
            // 
            // SelectedRowBarStaticItem
            // 
            this.SelectedRowBarStaticItem.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True;
            this.SelectedRowBarStaticItem.Caption = "Chưa chọn dòng nào";
            this.SelectedRowBarStaticItem.Id = 9;
            this.SelectedRowBarStaticItem.Name = "SelectedRowBarStaticItem";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager1;
            this.barDockControlTop.Size = new System.Drawing.Size(1075, 39);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 592);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(1075, 35);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 39);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 553);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1075, 39);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 553);
            // 
            // repositoryItemComboBox1
            // 
            this.repositoryItemComboBox1.AutoHeight = false;
            this.repositoryItemComboBox1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox1.Name = "repositoryItemComboBox1";
            // 
            // repositoryItemComboBox2
            // 
            this.repositoryItemComboBox2.AutoHeight = false;
            this.repositoryItemComboBox2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox2.Name = "repositoryItemComboBox2";
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.ProductServiceListGridControl);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 39);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.Root;
            this.layoutControl1.Size = new System.Drawing.Size(1075, 553);
            this.layoutControl1.TabIndex = 4;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // ProductServiceListGridControl
            // 
            this.ProductServiceListGridControl.DataSource = this.productServiceDtoBindingSource;
            this.ProductServiceListGridControl.Location = new System.Drawing.Point(16, 16);
            this.ProductServiceListGridControl.MainView = this.ProductServiceGridView;
            this.ProductServiceListGridControl.MenuManager = this.barManager1;
            this.ProductServiceListGridControl.Name = "ProductServiceListGridControl";
            this.ProductServiceListGridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.HtmlHypertextLabel,
            this.ProductThumbnailRepositoryItemPictureEdit});
            this.ProductServiceListGridControl.Size = new System.Drawing.Size(1043, 521);
            this.ProductServiceListGridControl.TabIndex = 5;
            this.ProductServiceListGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.ProductServiceGridView});
            // 
            // productServiceDtoBindingSource
            // 
            this.productServiceDtoBindingSource.DataSource = typeof(DTO.MasterData.ProductService.ProductServiceDto);
            // 
            // ProductServiceGridView
            // 
            this.ProductServiceGridView.Appearance.ViewCaption.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.ProductServiceGridView.Appearance.ViewCaption.ForeColor = DevExpress.LookAndFeel.DXSkinColors.ForeColors.Information;
            this.ProductServiceGridView.Appearance.ViewCaption.Options.UseFont = true;
            this.ProductServiceGridView.Appearance.ViewCaption.Options.UseForeColor = true;
            this.ProductServiceGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colThongTinHtml,
            this.colThumbnailImage,
            this.colCategoryFullPathHtml});
            this.ProductServiceGridView.GridControl = this.ProductServiceListGridControl;
            this.ProductServiceGridView.IndicatorWidth = 40;
            this.ProductServiceGridView.Name = "ProductServiceGridView";
            this.ProductServiceGridView.OptionsSelection.MultiSelect = true;
            this.ProductServiceGridView.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            this.ProductServiceGridView.OptionsView.ColumnAutoWidth = false;
            this.ProductServiceGridView.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            this.ProductServiceGridView.OptionsView.EnableAppearanceEvenRow = true;
            this.ProductServiceGridView.OptionsView.EnableAppearanceOddRow = true;
            this.ProductServiceGridView.OptionsView.RowAutoHeight = true;
            this.ProductServiceGridView.OptionsView.ShowAutoFilterRow = true;
            this.ProductServiceGridView.OptionsView.ShowFooter = true;
            this.ProductServiceGridView.OptionsView.ShowGroupPanel = false;
            this.ProductServiceGridView.OptionsView.ShowViewCaption = true;
            this.ProductServiceGridView.ViewCaption = "BẢNG DỮ LIỆU SẢN PHẨM DỊCH VỤ";
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
            this.colThongTinHtml.Caption = "Thông tin đầy đủ";
            this.colThongTinHtml.ColumnEdit = this.HtmlHypertextLabel;
            this.colThongTinHtml.FieldName = "ThongTinHtml";
            this.colThongTinHtml.MinWidth = 300;
            this.colThongTinHtml.Name = "colThongTinHtml";
            this.colThongTinHtml.Visible = true;
            this.colThongTinHtml.VisibleIndex = 3;
            this.colThongTinHtml.Width = 500;
            // 
            // HtmlHypertextLabel
            // 
            this.HtmlHypertextLabel.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.HtmlHypertextLabel.Name = "HtmlHypertextLabel";
            // 
            // colThumbnailImage
            // 
            this.colThumbnailImage.AppearanceCell.Options.UseTextOptions = true;
            this.colThumbnailImage.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colThumbnailImage.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colThumbnailImage.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colThumbnailImage.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colThumbnailImage.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colThumbnailImage.AppearanceHeader.Options.UseBackColor = true;
            this.colThumbnailImage.AppearanceHeader.Options.UseFont = true;
            this.colThumbnailImage.AppearanceHeader.Options.UseForeColor = true;
            this.colThumbnailImage.AppearanceHeader.Options.UseTextOptions = true;
            this.colThumbnailImage.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colThumbnailImage.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colThumbnailImage.Caption = "Ảnh đại diện";
            this.colThumbnailImage.ColumnEdit = this.ProductThumbnailRepositoryItemPictureEdit;
            this.colThumbnailImage.FieldName = "ThumbnailImage";
            this.colThumbnailImage.MaxWidth = 120;
            this.colThumbnailImage.MinWidth = 120;
            this.colThumbnailImage.Name = "colThumbnailImage";
            this.colThumbnailImage.OptionsColumn.FixedWidth = true;
            this.colThumbnailImage.Visible = true;
            this.colThumbnailImage.VisibleIndex = 1;
            this.colThumbnailImage.Width = 120;
            // 
            // ProductThumbnailRepositoryItemPictureEdit
            // 
            this.ProductThumbnailRepositoryItemPictureEdit.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.ProductThumbnailRepositoryItemPictureEdit.Name = "ProductThumbnailRepositoryItemPictureEdit";
            this.ProductThumbnailRepositoryItemPictureEdit.PictureInterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            this.ProductThumbnailRepositoryItemPictureEdit.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            // 
            // colCategoryFullPathHtml
            // 
            this.colCategoryFullPathHtml.AppearanceCell.Options.UseTextOptions = true;
            this.colCategoryFullPathHtml.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colCategoryFullPathHtml.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colCategoryFullPathHtml.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colCategoryFullPathHtml.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colCategoryFullPathHtml.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colCategoryFullPathHtml.AppearanceHeader.Options.UseBackColor = true;
            this.colCategoryFullPathHtml.AppearanceHeader.Options.UseFont = true;
            this.colCategoryFullPathHtml.AppearanceHeader.Options.UseForeColor = true;
            this.colCategoryFullPathHtml.AppearanceHeader.Options.UseTextOptions = true;
            this.colCategoryFullPathHtml.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colCategoryFullPathHtml.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colCategoryFullPathHtml.Caption = "Phân loại";
            this.colCategoryFullPathHtml.ColumnEdit = this.HtmlHypertextLabel;
            this.colCategoryFullPathHtml.FieldName = "CategoryFullPathHtml";
            this.colCategoryFullPathHtml.MinWidth = 200;
            this.colCategoryFullPathHtml.Name = "colCategoryFullPathHtml";
            this.colCategoryFullPathHtml.Visible = true;
            this.colCategoryFullPathHtml.VisibleIndex = 2;
            this.colCategoryFullPathHtml.Width = 280;
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(1075, 553);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.ProductServiceListGridControl;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(1049, 527);
            this.layoutControlItem1.TextVisible = false;
            // 
            // FrmProductServiceList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1075, 627);
            this.Controls.Add(this.layoutControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "FrmProductServiceList";
            this.Text = "SẢN PHẨM DỊCH VỤ";
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ProductServiceListGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.productServiceDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductServiceGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HtmlHypertextLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductThumbnailRepositoryItemPictureEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BarManager barManager1;
        private Bar bar2;
        private BarDockControl barDockControlTop;
        private BarDockControl barDockControlBottom;
        private BarDockControl barDockControlLeft;
        private BarDockControl barDockControlRight;
        private LayoutControl layoutControl1;
        private GridControl ProductServiceListGridControl;
        private LayoutControlGroup Root;
        private LayoutControlItem layoutControlItem1;
        private BarButtonItem ListDataBarButtonItem;
        private BarButtonItem NewBarButtonItem;
        private BarButtonItem EditBarButtonItem;
        private BarButtonItem DeleteBarButtonItem;
        private BarButtonItem ExportBarButtonItem;
        private BarButtonItem CountVariantAndImageBarButtonItem;
        
        private BindingSource productServiceDtoBindingSource;
        private RepositoryItemPictureEdit repositoryItemPictureEdit1;
        private Bar bar1;
        private BarHeaderItem barHeaderItem1;
        private BarStaticItem DataSummaryBarStaticItem;
        private BarHeaderItem barHeaderItem2;
        private BarStaticItem SelectedRowBarStaticItem;
        private RepositoryItemComboBox repositoryItemComboBox1;
        private RepositoryItemComboBox repositoryItemComboBox2;
        private GridView ProductServiceGridView;
        private GridColumn colThongTinHtml;
        private GridColumn colThumbnailImage;
        private DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel HtmlHypertextLabel;
        private GridColumn colCategoryFullPathHtml;
        private DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit ProductThumbnailRepositoryItemPictureEdit;
    }
}
