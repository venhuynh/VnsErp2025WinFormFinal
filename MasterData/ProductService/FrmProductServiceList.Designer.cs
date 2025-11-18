using System.ComponentModel;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraLayout;
using DTO.MasterData.ProductService;

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
            this.DataFilterBtn = new DevExpress.XtraBars.BarButtonItem();
            this.NewBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.EditBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.DeleteBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.CountVariantAndImageBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.ExportBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.barHeaderItem1 = new DevExpress.XtraBars.BarHeaderItem();
            this.DataSummaryBarStaticItem = new DevExpress.XtraBars.BarStaticItem();
            this.PageBarEditItem = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.RecordNumberBarEditItem = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemComboBox2 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.barHeaderItem2 = new DevExpress.XtraBars.BarHeaderItem();
            this.SelectedRowBarStaticItem = new DevExpress.XtraBars.BarStaticItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.ProductServiceListGridControl = new DevExpress.XtraGrid.GridControl();
            this.productServiceDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ProductServiceAdvBandedGridView = new DevExpress.XtraGrid.Views.BandedGrid.AdvBandedGridView();
            this.gridBand1 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.colCode = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colName = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.repositoryItemHypertextLabel1 = new DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel();
            this.colTypeDisplay = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colDescription = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gridBand3 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.colCategoryName = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colStatusDisplay = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colVariantCount = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colImageCount = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colIsActive = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gridBand2 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.colThumbnail = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.repositoryItemPictureEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit();
            this.colId = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ProductServiceListGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.productServiceDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductServiceAdvBandedGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemHypertextLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemPictureEdit1)).BeginInit();
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
            this.SelectedRowBarStaticItem,
            this.PageBarEditItem,
            this.RecordNumberBarEditItem,
            this.DataFilterBtn});
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
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.DataFilterBtn, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
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
            // DataFilterBtn
            // 
            this.DataFilterBtn.Caption = "Lọc dữ liệu";
            this.DataFilterBtn.Id = 12;
            this.DataFilterBtn.ImageOptions.Image = global::MasterData.Properties.Resources.crossdatasourcefiltering__16x16;
            this.DataFilterBtn.Name = "DataFilterBtn";
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
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.PageBarEditItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.RecordNumberBarEditItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
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
            // PageBarEditItem
            // 
            this.PageBarEditItem.Caption = "Trang";
            this.PageBarEditItem.Edit = this.repositoryItemComboBox1;
            this.PageBarEditItem.Id = 10;
            this.PageBarEditItem.Name = "PageBarEditItem";
            // 
            // repositoryItemComboBox1
            // 
            this.repositoryItemComboBox1.AutoHeight = false;
            this.repositoryItemComboBox1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox1.Name = "repositoryItemComboBox1";
            // 
            // RecordNumberBarEditItem
            // 
            this.RecordNumberBarEditItem.Caption = "Số dòng";
            this.RecordNumberBarEditItem.Edit = this.repositoryItemComboBox2;
            this.RecordNumberBarEditItem.Id = 11;
            this.RecordNumberBarEditItem.Name = "RecordNumberBarEditItem";
            // 
            // repositoryItemComboBox2
            // 
            this.repositoryItemComboBox2.AutoHeight = false;
            this.repositoryItemComboBox2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox2.Name = "repositoryItemComboBox2";
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
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 590);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(1075, 37);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 39);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 551);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1075, 39);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 551);
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.ProductServiceListGridControl);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 39);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.Root;
            this.layoutControl1.Size = new System.Drawing.Size(1075, 551);
            this.layoutControl1.TabIndex = 4;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // ProductServiceListGridControl
            // 
            this.ProductServiceListGridControl.DataSource = this.productServiceDtoBindingSource;
            this.ProductServiceListGridControl.EmbeddedNavigator.AllowHtmlTextInToolTip = DevExpress.Utils.DefaultBoolean.True;
            this.ProductServiceListGridControl.Location = new System.Drawing.Point(16, 16);
            this.ProductServiceListGridControl.MainView = this.ProductServiceAdvBandedGridView;
            this.ProductServiceListGridControl.MenuManager = this.barManager1;
            this.ProductServiceListGridControl.Name = "ProductServiceListGridControl";
            this.ProductServiceListGridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemPictureEdit1,
            this.repositoryItemHypertextLabel1});
            this.ProductServiceListGridControl.Size = new System.Drawing.Size(1043, 519);
            this.ProductServiceListGridControl.TabIndex = 5;
            this.ProductServiceListGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.ProductServiceAdvBandedGridView});
            // 
            // productServiceDtoBindingSource
            // 
            this.productServiceDtoBindingSource.DataSource = typeof(ProductServiceDto);
            // 
            // ProductServiceAdvBandedGridView
            // 
            this.ProductServiceAdvBandedGridView.Appearance.ViewCaption.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.ProductServiceAdvBandedGridView.Appearance.ViewCaption.ForeColor = System.Drawing.Color.Blue;
            this.ProductServiceAdvBandedGridView.Appearance.ViewCaption.Options.UseFont = true;
            this.ProductServiceAdvBandedGridView.Appearance.ViewCaption.Options.UseForeColor = true;
            this.ProductServiceAdvBandedGridView.Bands.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.gridBand1,
            this.gridBand3,
            this.gridBand2});
            this.ProductServiceAdvBandedGridView.Columns.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn[] {
            this.colId,
            this.colThumbnail,
            this.colCode,
            this.colName,
            this.colCategoryName,
            this.colTypeDisplay,
            this.colDescription,
            this.colIsActive,
            this.colStatusDisplay,
            this.colVariantCount,
            this.colImageCount});
            this.ProductServiceAdvBandedGridView.GridControl = this.ProductServiceListGridControl;
            this.ProductServiceAdvBandedGridView.IndicatorWidth = 40;
            this.ProductServiceAdvBandedGridView.Name = "ProductServiceAdvBandedGridView";
            this.ProductServiceAdvBandedGridView.OptionsClipboard.AllowHtmlFormat = DevExpress.Utils.DefaultBoolean.True;
            this.ProductServiceAdvBandedGridView.OptionsFind.AlwaysVisible = true;
            this.ProductServiceAdvBandedGridView.OptionsPrint.EnableAppearanceEvenRow = true;
            this.ProductServiceAdvBandedGridView.OptionsSelection.MultiSelect = true;
            this.ProductServiceAdvBandedGridView.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            this.ProductServiceAdvBandedGridView.OptionsView.AllowHtmlDrawDetailTabs = true;
            this.ProductServiceAdvBandedGridView.OptionsView.AllowHtmlDrawHeaders = true;
            this.ProductServiceAdvBandedGridView.OptionsView.EnableAppearanceEvenRow = true;
            this.ProductServiceAdvBandedGridView.OptionsView.ShowAutoFilterRow = true;
            this.ProductServiceAdvBandedGridView.OptionsView.ShowGroupPanel = false;
            this.ProductServiceAdvBandedGridView.OptionsView.ShowViewCaption = true;
            this.ProductServiceAdvBandedGridView.ViewCaption = "BẢNG DỮ LIỆU SẢN PHẨM DỊCH VỤ";
            // 
            // gridBand1
            // 
            this.gridBand1.Caption = "Thông tin cơ bản";
            this.gridBand1.Columns.Add(this.colCode);
            this.gridBand1.Columns.Add(this.colName);
            this.gridBand1.Columns.Add(this.colTypeDisplay);
            this.gridBand1.Columns.Add(this.colDescription);
            this.gridBand1.Name = "gridBand1";
            this.gridBand1.VisibleIndex = 0;
            this.gridBand1.Width = 225;
            // 
            // colCode
            // 
            this.colCode.FieldName = "Code";
            this.colCode.Name = "colCode";
            this.colCode.Visible = true;
            // 
            // colName
            // 
            this.colName.ColumnEdit = this.repositoryItemHypertextLabel1;
            this.colName.FieldName = "Name";
            this.colName.Name = "colName";
            this.colName.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "Name", "Có {0} dòng")});
            this.colName.Visible = true;
            // 
            // repositoryItemHypertextLabel1
            // 
            this.repositoryItemHypertextLabel1.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.repositoryItemHypertextLabel1.Name = "repositoryItemHypertextLabel1";
            // 
            // colTypeDisplay
            // 
            this.colTypeDisplay.ColumnEdit = this.repositoryItemHypertextLabel1;
            this.colTypeDisplay.FieldName = "TypeDisplay";
            this.colTypeDisplay.Name = "colTypeDisplay";
            this.colTypeDisplay.Visible = true;
            // 
            // colDescription
            // 
            this.colDescription.ColumnEdit = this.repositoryItemHypertextLabel1;
            this.colDescription.FieldName = "Description";
            this.colDescription.Name = "colDescription";
            this.colDescription.RowCount = 2;
            this.colDescription.RowIndex = 1;
            this.colDescription.Visible = true;
            // 
            // gridBand3
            // 
            this.gridBand3.Caption = "Phân loại & Trạng thái";
            this.gridBand3.Columns.Add(this.colCategoryName);
            this.gridBand3.Columns.Add(this.colStatusDisplay);
            this.gridBand3.Columns.Add(this.colVariantCount);
            this.gridBand3.Columns.Add(this.colImageCount);
            this.gridBand3.Columns.Add(this.colIsActive);
            this.gridBand3.Name = "gridBand3";
            this.gridBand3.VisibleIndex = 1;
            this.gridBand3.Width = 150;
            // 
            // colCategoryName
            // 
            this.colCategoryName.ColumnEdit = this.repositoryItemHypertextLabel1;
            this.colCategoryName.FieldName = "CategoryName";
            this.colCategoryName.Name = "colCategoryName";
            this.colCategoryName.Visible = true;
            // 
            // colStatusDisplay
            // 
            this.colStatusDisplay.FieldName = "StatusDisplay";
            this.colStatusDisplay.Name = "colStatusDisplay";
            this.colStatusDisplay.Visible = true;
            // 
            // colVariantCount
            // 
            this.colVariantCount.FieldName = "VariantCount";
            this.colVariantCount.Name = "colVariantCount";
            this.colVariantCount.RowIndex = 1;
            this.colVariantCount.Visible = true;
            // 
            // colImageCount
            // 
            this.colImageCount.FieldName = "ImageCount";
            this.colImageCount.Name = "colImageCount";
            this.colImageCount.RowIndex = 1;
            this.colImageCount.Visible = true;
            // 
            // colIsActive
            // 
            this.colIsActive.FieldName = "IsActive";
            this.colIsActive.Name = "colIsActive";
            this.colIsActive.RowIndex = 2;
            this.colIsActive.Visible = true;
            // 
            // gridBand2
            // 
            this.gridBand2.Caption = "Ảnh & Thời gian";
            this.gridBand2.Columns.Add(this.colThumbnail);
            this.gridBand2.Name = "gridBand2";
            this.gridBand2.VisibleIndex = 2;
            this.gridBand2.Width = 80;
            // 
            // colThumbnail
            // 
            this.colThumbnail.ColumnEdit = this.repositoryItemPictureEdit1;
            this.colThumbnail.FieldName = "ThumbnailImage";
            this.colThumbnail.Name = "colThumbnail";
            this.colThumbnail.OptionsEditForm.RowSpan = 3;
            this.colThumbnail.RowCount = 3;
            this.colThumbnail.Visible = true;
            this.colThumbnail.Width = 80;
            // 
            // repositoryItemPictureEdit1
            // 
            this.repositoryItemPictureEdit1.Name = "repositoryItemPictureEdit1";
            // 
            // colId
            // 
            this.colId.FieldName = "Id";
            this.colId.Name = "colId";
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(1075, 551);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.ProductServiceListGridControl;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(1049, 525);
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
            ((System.ComponentModel.ISupportInitialize)(this.ProductServiceAdvBandedGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemHypertextLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemPictureEdit1)).EndInit();
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
        private AdvBandedGridView ProductServiceAdvBandedGridView;
        private BandedGridColumn colId;
        private BandedGridColumn colThumbnail;
        private RepositoryItemPictureEdit repositoryItemPictureEdit1;
        private BandedGridColumn colCode;
        private BandedGridColumn colName;
        private BandedGridColumn colCategoryName;
        private BandedGridColumn colTypeDisplay;
        private BandedGridColumn colDescription;
        private BandedGridColumn colIsActive;
        private BandedGridColumn colStatusDisplay;
        private BandedGridColumn colVariantCount;
        private BandedGridColumn colImageCount;
        private GridBand gridBand1;
        private GridBand gridBand3;
        private GridBand gridBand2;
        private Bar bar1;
        private BarHeaderItem barHeaderItem1;
        private BarStaticItem DataSummaryBarStaticItem;
        private BarHeaderItem barHeaderItem2;
        private BarStaticItem SelectedRowBarStaticItem;
        private BarEditItem PageBarEditItem;
        private RepositoryItemComboBox repositoryItemComboBox1;
        private BarEditItem RecordNumberBarEditItem;
        private RepositoryItemComboBox repositoryItemComboBox2;
        private BarButtonItem DataFilterBtn;
        private RepositoryItemHypertextLabel repositoryItemHypertextLabel1;
    }
}
