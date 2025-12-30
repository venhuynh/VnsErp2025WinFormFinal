using System.ComponentModel;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout;
using DTO.MasterData.ProductService;

namespace MasterData.ProductService
{
    partial class FrmProductVariant
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
            this.ProductVariantListGridControl = new DevExpress.XtraGrid.GridControl();
            this.productVariantSimpleDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ProductVariantListGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colVariantFullNameHtml = new DevExpress.XtraGrid.Columns.GridColumn();
            this.VariantFullNameHtmlHypertextLabel = new DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel();
            this.colThumbnailImage = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ThumbnailItemPictureEdit = new DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit();
            this.colProductName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.ListDataBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.UpdateProductVariantFullNameBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.NewBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.EditBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.DeleteBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
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
            this.CountVariantAndImageBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.repositoryItemSpinEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.repositoryItemComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.ProductVariantListGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.productVariantSimpleDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductVariantListGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VariantFullNameHtmlHypertextLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ThumbnailItemPictureEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSpinEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            this.SuspendLayout();
            // 
            // ProductVariantListGridControl
            // 
            this.ProductVariantListGridControl.DataSource = this.productVariantSimpleDtoBindingSource;
            this.ProductVariantListGridControl.Location = new System.Drawing.Point(12, 12);
            this.ProductVariantListGridControl.MainView = this.ProductVariantListGridView;
            this.ProductVariantListGridControl.MenuManager = this.barManager1;
            this.ProductVariantListGridControl.Name = "ProductVariantListGridControl";
            this.ProductVariantListGridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.VariantFullNameHtmlHypertextLabel,
            this.ThumbnailItemPictureEdit});
            this.ProductVariantListGridControl.Size = new System.Drawing.Size(963, 441);
            this.ProductVariantListGridControl.TabIndex = 4;
            this.ProductVariantListGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.ProductVariantListGridView});
            // 
            // productVariantSimpleDtoBindingSource
            // 
            this.productVariantSimpleDtoBindingSource.DataSource = typeof(DTO.MasterData.ProductService.ProductVariantSimpleDto);
            // 
            // ProductVariantListGridView
            // 
            this.ProductVariantListGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colVariantFullNameHtml,
            this.colThumbnailImage,
            this.colProductName});
            this.ProductVariantListGridView.GridControl = this.ProductVariantListGridControl;
            this.ProductVariantListGridView.GroupSummary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Count, "VariantCode", null, " [Có {0} biến thể]")});
            this.ProductVariantListGridView.IndicatorWidth = 50;
            this.ProductVariantListGridView.Name = "ProductVariantListGridView";
            this.ProductVariantListGridView.OptionsScrollAnnotations.ShowSelectedRows = DevExpress.Utils.DefaultBoolean.True;
            this.ProductVariantListGridView.OptionsSelection.MultiSelect = true;
            this.ProductVariantListGridView.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            this.ProductVariantListGridView.OptionsView.GroupDrawMode = DevExpress.XtraGrid.Views.Grid.GroupDrawMode.Office;
            this.ProductVariantListGridView.OptionsView.RowAutoHeight = true;
            this.ProductVariantListGridView.OptionsView.ShowChildrenInGroupPanel = true;
            this.ProductVariantListGridView.OptionsView.ShowGroupedColumns = true;
            this.ProductVariantListGridView.OptionsView.ShowGroupPanel = false;
            this.ProductVariantListGridView.OptionsView.ShowGroupPanelColumnsAsSingleRow = true;
            this.ProductVariantListGridView.RowHeight = 60;
            this.ProductVariantListGridView.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.colVariantFullNameHtml, DevExpress.Data.ColumnSortOrder.Ascending)});
            // 
            // colVariantFullNameHtml
            // 
            this.colVariantFullNameHtml.AppearanceCell.Options.UseTextOptions = true;
            this.colVariantFullNameHtml.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colVariantFullNameHtml.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colVariantFullNameHtml.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colVariantFullNameHtml.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colVariantFullNameHtml.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colVariantFullNameHtml.AppearanceHeader.Options.UseBackColor = true;
            this.colVariantFullNameHtml.AppearanceHeader.Options.UseFont = true;
            this.colVariantFullNameHtml.AppearanceHeader.Options.UseForeColor = true;
            this.colVariantFullNameHtml.AppearanceHeader.Options.UseTextOptions = true;
            this.colVariantFullNameHtml.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colVariantFullNameHtml.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colVariantFullNameHtml.Caption = "Thông tin đầy đủ";
            this.colVariantFullNameHtml.ColumnEdit = this.VariantFullNameHtmlHypertextLabel;
            this.colVariantFullNameHtml.FieldName = "VariantFullName";
            this.colVariantFullNameHtml.MinWidth = 300;
            this.colVariantFullNameHtml.Name = "colVariantFullNameHtml";
            this.colVariantFullNameHtml.Visible = true;
            this.colVariantFullNameHtml.VisibleIndex = 3;
            this.colVariantFullNameHtml.Width = 500;
            // 
            // VariantFullNameHtmlHypertextLabel
            // 
            this.VariantFullNameHtmlHypertextLabel.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.VariantFullNameHtmlHypertextLabel.Name = "VariantFullNameHtmlHypertextLabel";
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
            this.colThumbnailImage.ColumnEdit = this.ThumbnailItemPictureEdit;
            this.colThumbnailImage.FieldName = "ThumbnailImage";
            this.colThumbnailImage.MaxWidth = 120;
            this.colThumbnailImage.MinWidth = 120;
            this.colThumbnailImage.Name = "colThumbnailImage";
            this.colThumbnailImage.OptionsColumn.FixedWidth = true;
            this.colThumbnailImage.Visible = true;
            this.colThumbnailImage.VisibleIndex = 1;
            this.colThumbnailImage.Width = 120;
            // 
            // ThumbnailItemPictureEdit
            // 
            this.ThumbnailItemPictureEdit.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.ThumbnailItemPictureEdit.CustomHeight = 60;
            this.ThumbnailItemPictureEdit.Name = "ThumbnailItemPictureEdit";
            this.ThumbnailItemPictureEdit.PictureInterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            this.ThumbnailItemPictureEdit.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            // 
            // colProductName
            // 
            this.colProductName.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.colProductName.AppearanceCell.ForeColor = DevExpress.LookAndFeel.DXSkinColors.ForeColors.Information;
            this.colProductName.AppearanceCell.Options.UseFont = true;
            this.colProductName.AppearanceCell.Options.UseForeColor = true;
            this.colProductName.AppearanceCell.Options.UseTextOptions = true;
            this.colProductName.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colProductName.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colProductName.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colProductName.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colProductName.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colProductName.AppearanceHeader.Options.UseBackColor = true;
            this.colProductName.AppearanceHeader.Options.UseFont = true;
            this.colProductName.AppearanceHeader.Options.UseForeColor = true;
            this.colProductName.AppearanceHeader.Options.UseTextOptions = true;
            this.colProductName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colProductName.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colProductName.Caption = "Tên sản phẩm";
            this.colProductName.FieldName = "ProductName";
            this.colProductName.MinWidth = 200;
            this.colProductName.Name = "colProductName";
            this.colProductName.OptionsColumn.AllowEdit = false;
            this.colProductName.OptionsColumn.AllowFocus = false;
            this.colProductName.OptionsColumn.AllowMove = false;
            this.colProductName.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.colProductName.OptionsColumn.FixedWidth = true;
            this.colProductName.ToolTip = "Tên sản phẩm/dịch vụ";
            this.colProductName.Visible = true;
            this.colProductName.VisibleIndex = 2;
            this.colProductName.Width = 300;
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
            this.ListDataBarButtonItem,
            this.NewBarButtonItem,
            this.EditBarButtonItem,
            this.DeleteBarButtonItem,
            this.CountVariantAndImageBarButtonItem,
            this.ExportBarButtonItem,
            this.barHeaderItem1,
            this.DataSummaryBarStaticItem,
            this.barHeaderItem2,
            this.SelectedRowBarStaticItem,
            this.UpdateProductVariantFullNameBarButtonItem});
            this.barManager1.MainMenu = this.bar2;
            this.barManager1.MaxItemId = 16;
            this.barManager1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemSpinEdit1,
            this.repositoryItemComboBox1});
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
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.UpdateProductVariantFullNameBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.NewBarButtonItem, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.EditBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.DeleteBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
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
            // UpdateProductVariantFullNameBarButtonItem
            // 
            this.UpdateProductVariantFullNameBarButtonItem.Caption = "Cập nhật tên";
            this.UpdateProductVariantFullNameBarButtonItem.Id = 15;
            this.UpdateProductVariantFullNameBarButtonItem.ImageOptions.Image = global::MasterData.Properties.Resources.updatetableofcontents_16x16;
            this.UpdateProductVariantFullNameBarButtonItem.ImageOptions.LargeImage = global::MasterData.Properties.Resources.updatetableofcontents_32x32;
            this.UpdateProductVariantFullNameBarButtonItem.Name = "UpdateProductVariantFullNameBarButtonItem";
            // 
            // NewBarButtonItem
            // 
            this.NewBarButtonItem.Caption = "Mới";
            this.NewBarButtonItem.Id = 3;
            this.NewBarButtonItem.ImageOptions.Image = global::MasterData.Properties.Resources.addnewdatasource_16x16;
            this.NewBarButtonItem.ImageOptions.LargeImage = global::MasterData.Properties.Resources.addnewdatasource_32x32;
            this.NewBarButtonItem.Name = "NewBarButtonItem";
            // 
            // EditBarButtonItem
            // 
            this.EditBarButtonItem.Caption = "Điều chỉnh";
            this.EditBarButtonItem.Id = 4;
            this.EditBarButtonItem.ImageOptions.Image = global::MasterData.Properties.Resources.edittask_16x16;
            this.EditBarButtonItem.ImageOptions.LargeImage = global::MasterData.Properties.Resources.edittask_32x32;
            this.EditBarButtonItem.Name = "EditBarButtonItem";
            // 
            // DeleteBarButtonItem
            // 
            this.DeleteBarButtonItem.Caption = "Xóa";
            this.DeleteBarButtonItem.Id = 5;
            this.DeleteBarButtonItem.ImageOptions.Image = global::MasterData.Properties.Resources.deletelist_16x16;
            this.DeleteBarButtonItem.ImageOptions.LargeImage = global::MasterData.Properties.Resources.deletelist_32x32;
            this.DeleteBarButtonItem.Name = "DeleteBarButtonItem";
            // 
            // ExportBarButtonItem
            // 
            this.ExportBarButtonItem.Caption = "Xuất";
            this.ExportBarButtonItem.Id = 7;
            this.ExportBarButtonItem.ImageOptions.Image = global::MasterData.Properties.Resources.exporttoxls_16x16;
            this.ExportBarButtonItem.ImageOptions.LargeImage = global::MasterData.Properties.Resources.exporttoxls_32x32;
            this.ExportBarButtonItem.Name = "ExportBarButtonItem";
            // 
            // bar1
            // 
            this.bar1.BarName = "Status bar";
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
            this.bar1.Text = "Status bar";
            // 
            // barHeaderItem1
            // 
            this.barHeaderItem1.Caption = "Tổng kết:";
            this.barHeaderItem1.Id = 8;
            this.barHeaderItem1.Name = "barHeaderItem1";
            // 
            // DataSummaryBarStaticItem
            // 
            this.DataSummaryBarStaticItem.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True;
            this.DataSummaryBarStaticItem.Caption = "Chưa có dữ liệu";
            this.DataSummaryBarStaticItem.Id = 9;
            this.DataSummaryBarStaticItem.Name = "DataSummaryBarStaticItem";
            // 
            // barHeaderItem2
            // 
            this.barHeaderItem2.Caption = "Đang chọn:";
            this.barHeaderItem2.Id = 12;
            this.barHeaderItem2.Name = "barHeaderItem2";
            // 
            // SelectedRowBarStaticItem
            // 
            this.SelectedRowBarStaticItem.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True;
            this.SelectedRowBarStaticItem.Caption = "Chưa chọn dòng nào";
            this.SelectedRowBarStaticItem.Id = 13;
            this.SelectedRowBarStaticItem.Name = "SelectedRowBarStaticItem";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager1;
            this.barDockControlTop.Size = new System.Drawing.Size(987, 24);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 489);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(987, 22);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 24);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 465);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(987, 24);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 465);
            // 
            // CountVariantAndImageBarButtonItem
            // 
            this.CountVariantAndImageBarButtonItem.Caption = "Thống kê";
            this.CountVariantAndImageBarButtonItem.Id = 6;
            this.CountVariantAndImageBarButtonItem.Name = "CountVariantAndImageBarButtonItem";
            // 
            // repositoryItemSpinEdit1
            // 
            this.repositoryItemSpinEdit1.AutoHeight = false;
            this.repositoryItemSpinEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemSpinEdit1.MaxValue = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.repositoryItemSpinEdit1.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.repositoryItemSpinEdit1.Name = "repositoryItemSpinEdit1";
            // 
            // repositoryItemComboBox1
            // 
            this.repositoryItemComboBox1.AutoHeight = false;
            this.repositoryItemComboBox1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox1.Items.AddRange(new object[] {
            "20",
            "50",
            "100",
            "Tất cả"});
            this.repositoryItemComboBox1.Name = "repositoryItemComboBox1";
            this.repositoryItemComboBox1.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.ProductVariantListGridControl);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 24);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.Root;
            this.layoutControl1.Size = new System.Drawing.Size(987, 465);
            this.layoutControl1.TabIndex = 4;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(987, 465);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.ProductVariantListGridControl;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(967, 445);
            this.layoutControlItem1.TextVisible = false;
            // 
            // FrmProductVariant
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(987, 511);
            this.Controls.Add(this.layoutControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "FrmProductVariant";
            this.Text = "BIẾN THỂ SPDV";
            ((System.ComponentModel.ISupportInitialize)(this.ProductVariantListGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.productVariantSimpleDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductVariantListGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VariantFullNameHtmlHypertextLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ThumbnailItemPictureEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSpinEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BarManager barManager1;
        private Bar bar2;
        private BarButtonItem ListDataBarButtonItem;
        private BarButtonItem NewBarButtonItem;
        private BarButtonItem EditBarButtonItem;
        private BarButtonItem DeleteBarButtonItem;
        private BarButtonItem CountVariantAndImageBarButtonItem;
        private BarButtonItem ExportBarButtonItem;
        private Bar bar1;
        private BarHeaderItem barHeaderItem1;
        private BarStaticItem DataSummaryBarStaticItem;
        private RepositoryItemSpinEdit repositoryItemSpinEdit1;
        private RepositoryItemComboBox repositoryItemComboBox1;
        private BarHeaderItem barHeaderItem2;
        private BarStaticItem SelectedRowBarStaticItem;
        private BarDockControl barDockControlTop;
        private BarDockControl barDockControlBottom;
        private BarDockControl barDockControlLeft;
        private BarDockControl barDockControlRight;
        private LayoutControl layoutControl1;
        private LayoutControlGroup Root;
        private GridControl ProductVariantListGridControl;
        private GridView ProductVariantListGridView;
        private LayoutControlItem layoutControlItem1;
        private GridColumn colVariantFullNameHtml;
        private RepositoryItemHypertextLabel VariantFullNameHtmlHypertextLabel;
        private BarButtonItem UpdateProductVariantFullNameBarButtonItem;
        private GridColumn colThumbnailImage;
        private RepositoryItemPictureEdit ThumbnailItemPictureEdit;
        private GridColumn colProductName;
        private BindingSource productVariantSimpleDtoBindingSource;
    }
}
