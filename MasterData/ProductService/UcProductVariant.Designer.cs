namespace MasterData.ProductService
{
    partial class UcProductVariant
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


        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            DevExpress.XtraGrid.GridLevelNode gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
            this.VariantGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colVariantCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colVariantName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ProductServiceMasterDetailViewGridControl = new DevExpress.XtraGrid.GridControl();
            this.ProductServiceMasterDetailViewGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colProductName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colProductIsActive = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colProductThumbnailImage = new DevExpress.XtraGrid.Columns.GridColumn();
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
            this.barHeaderItem2 = new DevExpress.XtraBars.BarHeaderItem();
            this.SelectedRowBarStaticItem = new DevExpress.XtraBars.BarStaticItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.repositoryItemSpinEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.repositoryItemComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.productServiceMasterDetailViewDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.colUnitName = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.VariantGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductServiceMasterDetailViewGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductServiceMasterDetailViewGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSpinEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.productServiceMasterDetailViewDtoBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // VariantGridView
            // 
            this.VariantGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colVariantCode,
            this.colVariantName,
            this.colUnitName});
            this.VariantGridView.GridControl = this.ProductServiceMasterDetailViewGridControl;
            this.VariantGridView.Name = "VariantGridView";
            this.VariantGridView.OptionsSelection.MultiSelect = true;
            this.VariantGridView.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            this.VariantGridView.OptionsSelection.ShowCheckBoxSelectorInGroupRow = DevExpress.Utils.DefaultBoolean.True;
            this.VariantGridView.OptionsView.ShowGroupPanel = false;
            // 
            // colVariantCode
            // 
            this.colVariantCode.FieldName = "VariantCode";
            this.colVariantCode.Name = "colVariantCode";
            this.colVariantCode.Visible = true;
            this.colVariantCode.VisibleIndex = 1;
            // 
            // colVariantName
            // 
            this.colVariantName.FieldName = "VariantName";
            this.colVariantName.Name = "colVariantName";
            this.colVariantName.Visible = true;
            this.colVariantName.VisibleIndex = 2;
            // 
            // ProductServiceMasterDetailViewGridControl
            // 
            this.ProductServiceMasterDetailViewGridControl.DataSource = this.productServiceMasterDetailViewDtoBindingSource;
            gridLevelNode1.LevelTemplate = this.VariantGridView;
            gridLevelNode1.RelationName = "Variants";
            this.ProductServiceMasterDetailViewGridControl.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.ProductServiceMasterDetailViewGridControl.Location = new System.Drawing.Point(16, 16);
            this.ProductServiceMasterDetailViewGridControl.MainView = this.ProductServiceMasterDetailViewGridView;
            this.ProductServiceMasterDetailViewGridControl.MenuManager = this.barManager1;
            this.ProductServiceMasterDetailViewGridControl.Name = "ProductServiceMasterDetailViewGridControl";
            this.ProductServiceMasterDetailViewGridControl.Size = new System.Drawing.Size(1045, 555);
            this.ProductServiceMasterDetailViewGridControl.TabIndex = 4;
            this.ProductServiceMasterDetailViewGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.ProductServiceMasterDetailViewGridView,
            this.VariantGridView});
            // 
            // ProductServiceMasterDetailViewGridView
            // 
            this.ProductServiceMasterDetailViewGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colProductName,
            this.colProductIsActive,
            this.colProductThumbnailImage});
            this.ProductServiceMasterDetailViewGridView.GridControl = this.ProductServiceMasterDetailViewGridControl;
            this.ProductServiceMasterDetailViewGridView.IndicatorWidth = 50;
            this.ProductServiceMasterDetailViewGridView.Name = "ProductServiceMasterDetailViewGridView";
            this.ProductServiceMasterDetailViewGridView.OptionsView.ShowGroupPanel = false;
            // 
            // colProductName
            // 
            this.colProductName.FieldName = "ProductName";
            this.colProductName.Name = "colProductName";
            this.colProductName.Visible = true;
            this.colProductName.VisibleIndex = 0;
            // 
            // colProductIsActive
            // 
            this.colProductIsActive.FieldName = "ProductIsActive";
            this.colProductIsActive.Name = "colProductIsActive";
            this.colProductIsActive.Visible = true;
            this.colProductIsActive.VisibleIndex = 1;
            // 
            // colProductThumbnailImage
            // 
            this.colProductThumbnailImage.FieldName = "ProductThumbnailImage";
            this.colProductThumbnailImage.Name = "colProductThumbnailImage";
            this.colProductThumbnailImage.Visible = true;
            this.colProductThumbnailImage.VisibleIndex = 2;
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
            this.DataFilterBtn,
            this.NewBarButtonItem,
            this.EditBarButtonItem,
            this.DeleteBarButtonItem,
            this.CountVariantAndImageBarButtonItem,
            this.ExportBarButtonItem,
            this.barHeaderItem1,
            this.DataSummaryBarStaticItem,
            this.barHeaderItem2,
            this.SelectedRowBarStaticItem});
            this.barManager1.MainMenu = this.bar2;
            this.barManager1.MaxItemId = 15;
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
            this.DataFilterBtn.Caption = "Tìm kiếm";
            this.DataFilterBtn.Id = 2;
            this.DataFilterBtn.Name = "DataFilterBtn";
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
            // CountVariantAndImageBarButtonItem
            // 
            this.CountVariantAndImageBarButtonItem.Caption = "Thống kê";
            this.CountVariantAndImageBarButtonItem.Id = 6;
            this.CountVariantAndImageBarButtonItem.Name = "CountVariantAndImageBarButtonItem";
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
            this.barDockControlTop.Size = new System.Drawing.Size(1077, 39);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 626);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(1077, 35);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 39);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 587);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1077, 39);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 587);
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
            this.layoutControl1.Controls.Add(this.ProductServiceMasterDetailViewGridControl);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 39);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.Root;
            this.layoutControl1.Size = new System.Drawing.Size(1077, 587);
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
            this.Root.Size = new System.Drawing.Size(1077, 587);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.ProductServiceMasterDetailViewGridControl;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(1051, 561);
            this.layoutControlItem1.TextVisible = false;
            // 
            // productServiceMasterDetailViewDtoBindingSource
            // 
            this.productServiceMasterDetailViewDtoBindingSource.DataSource = typeof(MasterData.ProductService.Dto.ProductServiceMasterDetailViewDto);
            // 
            // colUnitName
            // 
            this.colUnitName.FieldName = "UnitName";
            this.colUnitName.Name = "colUnitName";
            this.colUnitName.Visible = true;
            this.colUnitName.VisibleIndex = 3;
            // 
            // UcProductVariant
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "UcProductVariant";
            this.Size = new System.Drawing.Size(1077, 661);
            ((System.ComponentModel.ISupportInitialize)(this.VariantGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductServiceMasterDetailViewGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductServiceMasterDetailViewGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSpinEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.productServiceMasterDetailViewDtoBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraBars.BarButtonItem ListDataBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem DataFilterBtn;
        private DevExpress.XtraBars.BarButtonItem NewBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem EditBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem DeleteBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem CountVariantAndImageBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem ExportBarButtonItem;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarHeaderItem barHeaderItem1;
        private DevExpress.XtraBars.BarStaticItem DataSummaryBarStaticItem;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit repositoryItemSpinEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox1;
        private DevExpress.XtraBars.BarHeaderItem barHeaderItem2;
        private DevExpress.XtraBars.BarStaticItem SelectedRowBarStaticItem;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraGrid.GridControl ProductServiceMasterDetailViewGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView ProductServiceMasterDetailViewGridView;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraGrid.Views.Grid.GridView VariantGridView;
        private DevExpress.XtraGrid.Columns.GridColumn colVariantCode;
        private DevExpress.XtraGrid.Columns.GridColumn colVariantName;
        private DevExpress.XtraGrid.Columns.GridColumn colProductName;
        private DevExpress.XtraGrid.Columns.GridColumn colProductIsActive;
        private DevExpress.XtraGrid.Columns.GridColumn colProductThumbnailImage;
        
        private DevExpress.XtraGrid.Columns.GridColumn colUnitName;
        private System.Windows.Forms.BindingSource productServiceMasterDetailViewDtoBindingSource;
    }
}
