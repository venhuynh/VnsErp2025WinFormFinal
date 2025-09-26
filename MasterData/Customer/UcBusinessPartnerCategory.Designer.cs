namespace MasterData.Customer
{
    partial class UcBusinessPartnerCategory
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
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.ListDataBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.NewBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.EditBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.DeleteBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.ExportBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.dockManager1 = new DevExpress.XtraBars.Docking.DockManager(this.components);
            this.dockPanel1 = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel1_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.dataLayoutControl1 = new DevExpress.XtraDataLayout.DataLayoutControl();
            this.CategoryNameTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.DescriptionMemoEdit = new DevExpress.XtraEditors.MemoEdit();
            this.businessPartnerCategoryDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.ItemForCategoryName = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForDescription = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.BusinessPartnerCategoryGridControl = new DevExpress.XtraGrid.GridControl();
            this.BusinessPartnerCategoryGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colCategoryName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDescription = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).BeginInit();
            this.dockPanel1.SuspendLayout();
            this.dockPanel1_Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).BeginInit();
            this.dataLayoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CategoryNameTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DescriptionMemoEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.businessPartnerCategoryDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForCategoryName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForDescription)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BusinessPartnerCategoryGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BusinessPartnerCategoryGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            this.SuspendLayout();
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar2});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.DockManager = this.dockManager1;
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.NewBarButtonItem,
            this.ListDataBarButtonItem,
            this.EditBarButtonItem,
            this.DeleteBarButtonItem,
            this.ExportBarButtonItem,
            this.barButtonItem1});
            this.barManager1.MainMenu = this.bar2;
            this.barManager1.MaxItemId = 6;
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
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.ExportBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItem1, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
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
            // ExportBarButtonItem
            // 
            this.ExportBarButtonItem.Caption = "Xuất";
            this.ExportBarButtonItem.Id = 4;
            this.ExportBarButtonItem.ImageOptions.Image = global::MasterData.Properties.Resources.exporttoxls_16x16;
            this.ExportBarButtonItem.ImageOptions.LargeImage = global::MasterData.Properties.Resources.exporttoxls_32x32;
            this.ExportBarButtonItem.Name = "ExportBarButtonItem";
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "Lưu";
            this.barButtonItem1.Id = 5;
            this.barButtonItem1.ImageOptions.Image = global::MasterData.Properties.Resources.save_16x16;
            this.barButtonItem1.Name = "barButtonItem1";
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
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 661);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(1077, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 39);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 622);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1077, 39);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 622);
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
            this.dockPanel1.ID = new System.Guid("6cc8fca0-bd27-46af-b0c4-4dd2cc1e6c70");
            this.dockPanel1.Location = new System.Drawing.Point(791, 39);
            this.dockPanel1.Name = "dockPanel1";
            this.dockPanel1.Options.ShowCloseButton = false;
            this.dockPanel1.OriginalSize = new System.Drawing.Size(286, 200);
            this.dockPanel1.Size = new System.Drawing.Size(286, 622);
            this.dockPanel1.Text = "dockPanel1";
            // 
            // dockPanel1_Container
            // 
            this.dockPanel1_Container.Controls.Add(this.dataLayoutControl1);
            this.dockPanel1_Container.Location = new System.Drawing.Point(4, 38);
            this.dockPanel1_Container.Name = "dockPanel1_Container";
            this.dockPanel1_Container.Size = new System.Drawing.Size(279, 581);
            this.dockPanel1_Container.TabIndex = 0;
            // 
            // dataLayoutControl1
            // 
            this.dataLayoutControl1.Controls.Add(this.CategoryNameTextEdit);
            this.dataLayoutControl1.Controls.Add(this.DescriptionMemoEdit);
            this.dataLayoutControl1.DataSource = this.businessPartnerCategoryDtoBindingSource;
            this.dataLayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataLayoutControl1.Location = new System.Drawing.Point(0, 0);
            this.dataLayoutControl1.Name = "dataLayoutControl1";
            this.dataLayoutControl1.Root = this.layoutControlGroup1;
            this.dataLayoutControl1.Size = new System.Drawing.Size(279, 581);
            this.dataLayoutControl1.TabIndex = 0;
            this.dataLayoutControl1.Text = "dataLayoutControl1";
            // 
            // CategoryNameTextEdit
            // 
            this.CategoryNameTextEdit.Location = new System.Drawing.Point(96, 16);
            this.CategoryNameTextEdit.MenuManager = this.barManager1;
            this.CategoryNameTextEdit.Name = "CategoryNameTextEdit";
            this.CategoryNameTextEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.CategoryNameTextEdit.Size = new System.Drawing.Size(167, 28);
            this.CategoryNameTextEdit.StyleController = this.dataLayoutControl1;
            this.CategoryNameTextEdit.TabIndex = 4;
            // 
            // DescriptionMemoEdit
            // 
            this.DescriptionMemoEdit.Location = new System.Drawing.Point(96, 50);
            this.DescriptionMemoEdit.MenuManager = this.barManager1;
            this.DescriptionMemoEdit.Name = "DescriptionMemoEdit";
            this.DescriptionMemoEdit.Size = new System.Drawing.Size(167, 515);
            this.DescriptionMemoEdit.StyleController = this.dataLayoutControl1;
            this.DescriptionMemoEdit.TabIndex = 5;
            // 
            // businessPartnerCategoryDtoBindingSource
            // 
            this.businessPartnerCategoryDtoBindingSource.DataSource = typeof(MasterData.Dto.BusinessPartnerCategoryDto);
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup2});
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Size = new System.Drawing.Size(279, 581);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlGroup2
            // 
            this.layoutControlGroup2.AllowDrawBackground = false;
            this.layoutControlGroup2.GroupBordersVisible = false;
            this.layoutControlGroup2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.ItemForCategoryName,
            this.ItemForDescription});
            this.layoutControlGroup2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup2.Name = "autoGeneratedGroup0";
            this.layoutControlGroup2.Size = new System.Drawing.Size(253, 555);
            // 
            // ItemForCategoryName
            // 
            this.ItemForCategoryName.Control = this.CategoryNameTextEdit;
            this.ItemForCategoryName.Location = new System.Drawing.Point(0, 0);
            this.ItemForCategoryName.Name = "ItemForCategoryName";
            this.ItemForCategoryName.Size = new System.Drawing.Size(253, 34);
            this.ItemForCategoryName.Text = "Tên phân loại";
            this.ItemForCategoryName.TextSize = new System.Drawing.Size(64, 13);
            // 
            // ItemForDescription
            // 
            this.ItemForDescription.Control = this.DescriptionMemoEdit;
            this.ItemForDescription.Location = new System.Drawing.Point(0, 34);
            this.ItemForDescription.Name = "ItemForDescription";
            this.ItemForDescription.Size = new System.Drawing.Size(253, 521);
            this.ItemForDescription.StartNewLine = true;
            this.ItemForDescription.Text = "Mô tả";
            this.ItemForDescription.TextSize = new System.Drawing.Size(64, 13);
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.BusinessPartnerCategoryGridControl);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 39);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.Root;
            this.layoutControl1.Size = new System.Drawing.Size(791, 622);
            this.layoutControl1.TabIndex = 4;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // BusinessPartnerCategoryGridControl
            // 
            this.BusinessPartnerCategoryGridControl.DataSource = this.businessPartnerCategoryDtoBindingSource;
            this.BusinessPartnerCategoryGridControl.Location = new System.Drawing.Point(16, 16);
            this.BusinessPartnerCategoryGridControl.MainView = this.BusinessPartnerCategoryGridView;
            this.BusinessPartnerCategoryGridControl.MenuManager = this.barManager1;
            this.BusinessPartnerCategoryGridControl.Name = "BusinessPartnerCategoryGridControl";
            this.BusinessPartnerCategoryGridControl.Size = new System.Drawing.Size(759, 590);
            this.BusinessPartnerCategoryGridControl.TabIndex = 5;
            this.BusinessPartnerCategoryGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.BusinessPartnerCategoryGridView});
            // 
            // BusinessPartnerCategoryGridView
            // 
            this.BusinessPartnerCategoryGridView.Appearance.ViewCaption.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.BusinessPartnerCategoryGridView.Appearance.ViewCaption.ForeColor = System.Drawing.Color.Blue;
            this.BusinessPartnerCategoryGridView.Appearance.ViewCaption.Options.UseFont = true;
            this.BusinessPartnerCategoryGridView.Appearance.ViewCaption.Options.UseForeColor = true;
            this.BusinessPartnerCategoryGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colCategoryName,
            this.colDescription});
            this.BusinessPartnerCategoryGridView.GridControl = this.BusinessPartnerCategoryGridControl;
            this.BusinessPartnerCategoryGridView.IndicatorWidth = 40;
            this.BusinessPartnerCategoryGridView.Name = "BusinessPartnerCategoryGridView";
            this.BusinessPartnerCategoryGridView.OptionsSelection.MultiSelect = true;
            this.BusinessPartnerCategoryGridView.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            this.BusinessPartnerCategoryGridView.OptionsView.ColumnAutoWidth = false;
            this.BusinessPartnerCategoryGridView.OptionsView.RowAutoHeight = true;
            this.BusinessPartnerCategoryGridView.OptionsView.ShowAutoFilterRow = true;
            this.BusinessPartnerCategoryGridView.OptionsView.ShowFooter = true;
            this.BusinessPartnerCategoryGridView.OptionsView.ShowGroupPanel = false;
            this.BusinessPartnerCategoryGridView.OptionsView.ShowViewCaption = true;
            this.BusinessPartnerCategoryGridView.ViewCaption = "BẢNG DỮ LIỆU ĐỐI TÁC";
            // 
            // colCategoryName
            // 
            this.colCategoryName.FieldName = "CategoryName";
            this.colCategoryName.Name = "colCategoryName";
            this.colCategoryName.Visible = true;
            this.colCategoryName.VisibleIndex = 1;
            // 
            // colDescription
            // 
            this.colDescription.FieldName = "Description";
            this.colDescription.Name = "colDescription";
            this.colDescription.Visible = true;
            this.colDescription.VisibleIndex = 2;
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(791, 622);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.BusinessPartnerCategoryGridControl;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(765, 596);
            this.layoutControlItem1.TextVisible = false;
            // 
            // UcBusinessPartnerCategory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutControl1);
            this.Controls.Add(this.dockPanel1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "UcBusinessPartnerCategory";
            this.Size = new System.Drawing.Size(1077, 661);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).EndInit();
            this.dockPanel1.ResumeLayout(false);
            this.dockPanel1_Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).EndInit();
            this.dataLayoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CategoryNameTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DescriptionMemoEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.businessPartnerCategoryDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForCategoryName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForDescription)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.BusinessPartnerCategoryGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BusinessPartnerCategoryGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraGrid.GridControl BusinessPartnerCategoryGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView BusinessPartnerCategoryGridView;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraBars.BarButtonItem ListDataBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem NewBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem EditBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem DeleteBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem ExportBarButtonItem;
        private DevExpress.XtraBars.Docking.DockManager dockManager1;
        private DevExpress.XtraBars.Docking.DockPanel dockPanel1;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel1_Container;
        private DevExpress.XtraDataLayout.DataLayoutControl dataLayoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private System.Windows.Forms.BindingSource businessPartnerCategoryDtoBindingSource;
        private DevExpress.XtraGrid.Columns.GridColumn colCategoryName;
        private DevExpress.XtraGrid.Columns.GridColumn colDescription;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraEditors.TextEdit CategoryNameTextEdit;
        private DevExpress.XtraEditors.MemoEdit DescriptionMemoEdit;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup2;
        private DevExpress.XtraLayout.LayoutControlItem ItemForCategoryName;
        private DevExpress.XtraLayout.LayoutControlItem ItemForDescription;
    }
}
