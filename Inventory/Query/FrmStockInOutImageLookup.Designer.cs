namespace Inventory.Query
{
    partial class FrmStockInOutImageLookup
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
            this.StockInOutImageDtoGridControl = new DevExpress.XtraGrid.GridControl();
            this.stockInOutImageDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.StockInOutImageDtoWinExplorerView = new DevExpress.XtraGrid.Views.WinExplorer.WinExplorerView();
            this.colId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFileName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFileSizeDisplay = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colImageData = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCreateDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFullPath = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colStorageType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFileExtension = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDisplayCaption = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colVocherNumber = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colGroupCaption = new DevExpress.XtraGrid.Columns.GridColumn();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.TuNgayBarEditItem = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemDateEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.DenNgayBarEditItem = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemDateEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.XemBaoCaoBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.XoaPhieuBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.XuatFileBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.barHeaderItem1 = new DevExpress.XtraBars.BarHeaderItem();
            this.DataSummaryBarStaticItem = new DevExpress.XtraBars.BarStaticItem();
            this.SelectedRowBarStaticItem = new DevExpress.XtraBars.BarStaticItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.StockInOutImageDtoGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stockInOutImageDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StockInOutImageDtoWinExplorerView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit2.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.StockInOutImageDtoGridControl);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 24);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.Root;
            this.layoutControl1.Size = new System.Drawing.Size(1035, 443);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // StockInOutImageDtoGridControl
            // 
            this.StockInOutImageDtoGridControl.DataSource = this.stockInOutImageDtoBindingSource;
            this.StockInOutImageDtoGridControl.Location = new System.Drawing.Point(12, 12);
            this.StockInOutImageDtoGridControl.MainView = this.StockInOutImageDtoWinExplorerView;
            this.StockInOutImageDtoGridControl.MenuManager = this.barManager1;
            this.StockInOutImageDtoGridControl.Name = "StockInOutImageDtoGridControl";
            this.StockInOutImageDtoGridControl.Size = new System.Drawing.Size(1011, 419);
            this.StockInOutImageDtoGridControl.TabIndex = 4;
            this.StockInOutImageDtoGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.StockInOutImageDtoWinExplorerView});
            // 
            // stockInOutImageDtoBindingSource
            // 
            this.stockInOutImageDtoBindingSource.DataSource = typeof(DTO.Inventory.Query.StockInOutImageDto);
            // 
            // StockInOutImageDtoWinExplorerView
            // 
            this.StockInOutImageDtoWinExplorerView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colId,
            this.colFileName,
            this.colFileSizeDisplay,
            this.colImageData,
            this.colCreateDate,
            this.colFullPath,
            this.colStorageType,
            this.colFileExtension,
            this.colDisplayCaption,
            this.colVocherNumber,
            this.colGroupCaption});
            this.StockInOutImageDtoWinExplorerView.GridControl = this.StockInOutImageDtoGridControl;
            this.StockInOutImageDtoWinExplorerView.Name = "StockInOutImageDtoWinExplorerView";
            this.StockInOutImageDtoWinExplorerView.OptionsBehavior.Editable = false;
            this.StockInOutImageDtoWinExplorerView.OptionsHtmlTemplate.AllowContentSelection = DevExpress.Utils.DefaultBoolean.True;
            this.StockInOutImageDtoWinExplorerView.OptionsImageLoad.AnimationType = DevExpress.Utils.ImageContentAnimationType.Expand;
            this.StockInOutImageDtoWinExplorerView.OptionsImageLoad.AsyncLoad = true;
            this.StockInOutImageDtoWinExplorerView.OptionsSelection.AllowMarqueeSelection = true;
            this.StockInOutImageDtoWinExplorerView.OptionsSelection.ItemSelectionMode = DevExpress.XtraGrid.Views.WinExplorer.IconItemSelectionMode.Click;
            this.StockInOutImageDtoWinExplorerView.OptionsSelection.MultiSelect = true;
            this.StockInOutImageDtoWinExplorerView.OptionsView.ImageLayoutMode = DevExpress.Utils.Drawing.ImageLayoutMode.Stretch;
            this.StockInOutImageDtoWinExplorerView.OptionsView.ShowExpandCollapseButtons = true;
            this.StockInOutImageDtoWinExplorerView.OptionsView.Style = DevExpress.XtraGrid.Views.WinExplorer.WinExplorerViewStyle.ExtraLarge;
            this.StockInOutImageDtoWinExplorerView.OptionsViewStyles.ExtraLarge.ImageSize = new System.Drawing.Size(256, 256);
            this.StockInOutImageDtoWinExplorerView.OptionsViewStyles.Large.ImageSize = new System.Drawing.Size(96, 96);
            this.StockInOutImageDtoWinExplorerView.OptionsViewStyles.Medium.ImageSize = new System.Drawing.Size(48, 48);
            this.StockInOutImageDtoWinExplorerView.OptionsViewStyles.Small.ImageSize = new System.Drawing.Size(16, 16);
            // 
            // colId
            // 
            this.colId.FieldName = "Id";
            this.colId.Name = "colId";
            // 
            // colFileName
            // 
            this.colFileName.Caption = "Tên file";
            this.colFileName.FieldName = "FileName";
            this.colFileName.Name = "colFileName";
            this.colFileName.Visible = true;
            this.colFileName.VisibleIndex = 0;
            // 
            // colFileSizeDisplay
            // 
            this.colFileSizeDisplay.Caption = "Kích thước";
            this.colFileSizeDisplay.FieldName = "FileSizeDisplay";
            this.colFileSizeDisplay.Name = "colFileSizeDisplay";
            // 
            // colImageData
            // 
            this.colImageData.Caption = "Hình ảnh";
            this.colImageData.FieldName = "ImageData";
            this.colImageData.Name = "colImageData";
            // 
            // colCreateDate
            // 
            this.colCreateDate.Caption = "Ngày tạo";
            this.colCreateDate.FieldName = "CreateDate";
            this.colCreateDate.Name = "colCreateDate";
            // 
            // colFullPath
            // 
            this.colFullPath.Caption = "Đường dẫn";
            this.colFullPath.FieldName = "FullPath";
            this.colFullPath.Name = "colFullPath";
            // 
            // colStorageType
            // 
            this.colStorageType.Caption = "Loại lưu trữ";
            this.colStorageType.FieldName = "StorageType";
            this.colStorageType.Name = "colStorageType";
            // 
            // colFileExtension
            // 
            this.colFileExtension.Caption = "Phần mở rộng";
            this.colFileExtension.FieldName = "FileExtension";
            this.colFileExtension.Name = "colFileExtension";
            // 
            // colDisplayCaption
            // 
            this.colDisplayCaption.Caption = "Caption";
            this.colDisplayCaption.FieldName = "DisplayCaption";
            this.colDisplayCaption.Name = "colDisplayCaption";
            this.colDisplayCaption.Visible = true;
            this.colDisplayCaption.VisibleIndex = 1;
            // 
            // colVocherNumber
            // 
            this.colVocherNumber.Caption = "Số phiếu";
            this.colVocherNumber.FieldName = "VocherNumber";
            this.colVocherNumber.Name = "colVocherNumber";
            this.colVocherNumber.Visible = true;
            this.colVocherNumber.VisibleIndex = 2;
            // 
            // colGroupCaption
            // 
            this.colGroupCaption.Caption = "Group Caption";
            this.colGroupCaption.FieldName = "GroupCaption";
            this.colGroupCaption.Name = "colGroupCaption";
            this.colGroupCaption.Visible = true;
            this.colGroupCaption.VisibleIndex = 3;
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
            this.TuNgayBarEditItem,
            this.DenNgayBarEditItem,
            this.XemBaoCaoBarButtonItem,
            this.barHeaderItem1,
            this.DataSummaryBarStaticItem,
            this.SelectedRowBarStaticItem,
            this.XoaPhieuBarButtonItem,
            this.XuatFileBarButtonItem});
            this.barManager1.MainMenu = this.bar2;
            this.barManager1.MaxItemId = 18;
            this.barManager1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemDateEdit1,
            this.repositoryItemDateEdit2,
            this.repositoryItemTextEdit1});
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
            new DevExpress.XtraBars.LinkPersistInfo(((DevExpress.XtraBars.BarLinkUserDefines)((DevExpress.XtraBars.BarLinkUserDefines.PaintStyle | DevExpress.XtraBars.BarLinkUserDefines.Width))), this.TuNgayBarEditItem, "", false, true, true, 117, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(((DevExpress.XtraBars.BarLinkUserDefines)((DevExpress.XtraBars.BarLinkUserDefines.PaintStyle | DevExpress.XtraBars.BarLinkUserDefines.Width))), this.DenNgayBarEditItem, "", false, true, true, 125, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.XemBaoCaoBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.XoaPhieuBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.XuatFileBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar2.OptionsBar.MultiLine = true;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Main menu";
            // 
            // repositoryItemTextEdit1
            // 
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // TuNgayBarEditItem
            // 
            this.TuNgayBarEditItem.Caption = "Từ ngày";
            this.TuNgayBarEditItem.Edit = this.repositoryItemDateEdit1;
            this.TuNgayBarEditItem.Id = 7;
            this.TuNgayBarEditItem.Name = "TuNgayBarEditItem";
            // 
            // repositoryItemDateEdit1
            // 
            this.repositoryItemDateEdit1.AutoHeight = false;
            this.repositoryItemDateEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit1.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit1.Name = "repositoryItemDateEdit1";
            // 
            // DenNgayBarEditItem
            // 
            this.DenNgayBarEditItem.Caption = "Đến ngày";
            this.DenNgayBarEditItem.Edit = this.repositoryItemDateEdit2;
            this.DenNgayBarEditItem.Id = 8;
            this.DenNgayBarEditItem.Name = "DenNgayBarEditItem";
            // 
            // repositoryItemDateEdit2
            // 
            this.repositoryItemDateEdit2.AutoHeight = false;
            this.repositoryItemDateEdit2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit2.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit2.Name = "repositoryItemDateEdit2";
            // 
            // XemBaoCaoBarButtonItem
            // 
            this.XemBaoCaoBarButtonItem.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True;
            this.XemBaoCaoBarButtonItem.Caption = "Xem";
            this.XemBaoCaoBarButtonItem.Id = 9;
            this.XemBaoCaoBarButtonItem.ImageOptions.Image = global::Inventory.Properties.Resources.filterbyseries_pie_16x16;
            this.XemBaoCaoBarButtonItem.ImageOptions.LargeImage = global::Inventory.Properties.Resources.filterbyseries_pie_32x32;
            this.XemBaoCaoBarButtonItem.Name = "XemBaoCaoBarButtonItem";
            // 
            // XoaPhieuBarButtonItem
            // 
            this.XoaPhieuBarButtonItem.Caption = "Xóa";
            this.XoaPhieuBarButtonItem.Id = 14;
            this.XoaPhieuBarButtonItem.ImageOptions.Image = global::Inventory.Properties.Resources.clear_16x16;
            this.XoaPhieuBarButtonItem.Name = "XoaPhieuBarButtonItem";
            // 
            // XuatFileBarButtonItem
            // 
            this.XuatFileBarButtonItem.Caption = "Tải về";
            this.XuatFileBarButtonItem.Id = 16;
            this.XuatFileBarButtonItem.ImageOptions.Image = global::Inventory.Properties.Resources.save_16x16;
            this.XuatFileBarButtonItem.ImageOptions.LargeImage = global::Inventory.Properties.Resources.exporttoxps_32x32;
            this.XuatFileBarButtonItem.Name = "XuatFileBarButtonItem";
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
            this.barDockControlTop.Size = new System.Drawing.Size(1035, 24);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 467);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(1035, 22);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 24);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 443);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1035, 24);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 443);
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(1035, 443);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.StockInOutImageDtoGridControl;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(1015, 423);
            this.layoutControlItem1.TextVisible = false;
            // 
            // FrmStockInOutImageLookup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1035, 489);
            this.Controls.Add(this.layoutControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "FrmStockInOutImageLookup";
            this.Text = "Danh sách hình ảnh nhập/xuất kho";
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.StockInOutImageDtoGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stockInOutImageDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StockInOutImageDtoWinExplorerView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit2.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit2)).EndInit();
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
        private DevExpress.XtraGrid.GridControl StockInOutImageDtoGridControl;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraBars.BarEditItem TuNgayBarEditItem;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEdit1;
        private DevExpress.XtraBars.BarEditItem DenNgayBarEditItem;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEdit2;
        private DevExpress.XtraBars.BarButtonItem XemBaoCaoBarButtonItem;
        private DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel HtmlRepositoryItemHypertextLabel;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarHeaderItem barHeaderItem1;
        private DevExpress.XtraBars.BarStaticItem DataSummaryBarStaticItem;
        private DevExpress.XtraBars.BarStaticItem SelectedRowBarStaticItem;
        private DevExpress.XtraBars.BarButtonItem XoaPhieuBarButtonItem;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
        private DevExpress.XtraBars.BarButtonItem XuatFileBarButtonItem;
        private DevExpress.XtraGrid.Views.WinExplorer.WinExplorerView StockInOutImageDtoWinExplorerView;
        private DevExpress.XtraGrid.Columns.GridColumn colId;
        private DevExpress.XtraGrid.Columns.GridColumn colFileName;
        private DevExpress.XtraGrid.Columns.GridColumn colFileSizeDisplay;
        private DevExpress.XtraGrid.Columns.GridColumn colImageData;
        private DevExpress.XtraGrid.Columns.GridColumn colDisplayCaption;
        private DevExpress.XtraGrid.Columns.GridColumn colVocherNumber;
        private DevExpress.XtraGrid.Columns.GridColumn colGroupCaption;
        private DevExpress.XtraGrid.Columns.GridColumn colCreateDate;
        private DevExpress.XtraGrid.Columns.GridColumn colFullPath;
        private DevExpress.XtraGrid.Columns.GridColumn colStorageType;
        private DevExpress.XtraGrid.Columns.GridColumn colFileExtension;
        private System.Windows.Forms.BindingSource stockInOutImageDtoBindingSource;
    }
}