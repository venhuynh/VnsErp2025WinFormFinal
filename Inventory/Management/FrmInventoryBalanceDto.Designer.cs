namespace Inventory.Management
{
    partial class FrmInventoryBalanceDto
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
            this.InventoryBalanceDtoGridControl = new DevExpress.XtraGrid.GridControl();
            this.inventoryBalanceDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.InventoryBalanceDtoGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colPeriodDisplay = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colWarehouseName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colProductName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colProductCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colOpeningBalance = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTotalInQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTotalOutQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colClosingBalance = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colStatusText = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colGroupCaption = new DevExpress.XtraGrid.Columns.GridColumn();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.NamBarEditItem = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemSpinEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.ThangBarEditItem = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemSpinEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.XemBaoCaoBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
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
            this.ExportFileBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.InventoryBalanceDtoGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inventoryBalanceDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.InventoryBalanceDtoGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSpinEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSpinEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.InventoryBalanceDtoGridControl);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 24);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.Root;
            this.layoutControl1.Size = new System.Drawing.Size(1035, 443);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // InventoryBalanceDtoGridControl
            // 
            this.InventoryBalanceDtoGridControl.DataSource = this.inventoryBalanceDtoBindingSource;
            this.InventoryBalanceDtoGridControl.Location = new System.Drawing.Point(12, 12);
            this.InventoryBalanceDtoGridControl.MainView = this.InventoryBalanceDtoGridView;
            this.InventoryBalanceDtoGridControl.MenuManager = this.barManager1;
            this.InventoryBalanceDtoGridControl.Name = "InventoryBalanceDtoGridControl";
            this.InventoryBalanceDtoGridControl.Size = new System.Drawing.Size(1011, 419);
            this.InventoryBalanceDtoGridControl.TabIndex = 4;
            this.InventoryBalanceDtoGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.InventoryBalanceDtoGridView});
            // 
            // inventoryBalanceDtoBindingSource
            // 
            this.inventoryBalanceDtoBindingSource.DataSource = typeof(DTO.Inventory.InventoryManagement.InventoryBalanceDto);
            // 
            // InventoryBalanceDtoGridView
            // 
            this.InventoryBalanceDtoGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colPeriodDisplay,
            this.colWarehouseName,
            this.colProductName,
            this.colProductCode,
            this.colOpeningBalance,
            this.colTotalInQty,
            this.colTotalOutQty,
            this.colClosingBalance,
            this.colStatusText,
            this.colGroupCaption});
            this.InventoryBalanceDtoGridView.GridControl = this.InventoryBalanceDtoGridControl;
            this.InventoryBalanceDtoGridView.Name = "InventoryBalanceDtoGridView";
            this.InventoryBalanceDtoGridView.OptionsBehavior.Editable = false;
            this.InventoryBalanceDtoGridView.OptionsSelection.MultiSelect = true;
            this.InventoryBalanceDtoGridView.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            this.InventoryBalanceDtoGridView.OptionsView.ShowGroupPanel = false;
            // 
            // colPeriodDisplay
            // 
            this.colPeriodDisplay.Caption = "Kỳ";
            this.colPeriodDisplay.FieldName = "PeriodDisplay";
            this.colPeriodDisplay.Name = "colPeriodDisplay";
            this.colPeriodDisplay.Visible = true;
            this.colPeriodDisplay.VisibleIndex = 1;
            this.colPeriodDisplay.Width = 100;
            // 
            // colWarehouseName
            // 
            this.colWarehouseName.Caption = "Kho";
            this.colWarehouseName.FieldName = "WarehouseName";
            this.colWarehouseName.Name = "colWarehouseName";
            this.colWarehouseName.Visible = true;
            this.colWarehouseName.VisibleIndex = 2;
            this.colWarehouseName.Width = 150;
            // 
            // colProductName
            // 
            this.colProductName.Caption = "Tên sản phẩm";
            this.colProductName.FieldName = "ProductName";
            this.colProductName.Name = "colProductName";
            this.colProductName.Visible = true;
            this.colProductName.VisibleIndex = 3;
            this.colProductName.Width = 200;
            // 
            // colProductCode
            // 
            this.colProductCode.Caption = "Mã sản phẩm";
            this.colProductCode.FieldName = "ProductCode";
            this.colProductCode.Name = "colProductCode";
            this.colProductCode.Visible = true;
            this.colProductCode.VisibleIndex = 4;
            this.colProductCode.Width = 120;
            // 
            // colOpeningBalance
            // 
            this.colOpeningBalance.Caption = "Tồn đầu kỳ";
            this.colOpeningBalance.FieldName = "OpeningBalance";
            this.colOpeningBalance.Name = "colOpeningBalance";
            this.colOpeningBalance.Visible = true;
            this.colOpeningBalance.VisibleIndex = 5;
            this.colOpeningBalance.Width = 120;
            // 
            // colTotalInQty
            // 
            this.colTotalInQty.Caption = "Tổng nhập";
            this.colTotalInQty.FieldName = "TotalInQty";
            this.colTotalInQty.Name = "colTotalInQty";
            this.colTotalInQty.Visible = true;
            this.colTotalInQty.VisibleIndex = 6;
            this.colTotalInQty.Width = 120;
            // 
            // colTotalOutQty
            // 
            this.colTotalOutQty.Caption = "Tổng xuất";
            this.colTotalOutQty.FieldName = "TotalOutQty";
            this.colTotalOutQty.Name = "colTotalOutQty";
            this.colTotalOutQty.Visible = true;
            this.colTotalOutQty.VisibleIndex = 7;
            this.colTotalOutQty.Width = 120;
            // 
            // colClosingBalance
            // 
            this.colClosingBalance.Caption = "Tồn cuối kỳ";
            this.colClosingBalance.FieldName = "ClosingBalance";
            this.colClosingBalance.Name = "colClosingBalance";
            this.colClosingBalance.Visible = true;
            this.colClosingBalance.VisibleIndex = 8;
            this.colClosingBalance.Width = 120;
            // 
            // colStatusText
            // 
            this.colStatusText.Caption = "Trạng thái";
            this.colStatusText.FieldName = "StatusText";
            this.colStatusText.Name = "colStatusText";
            this.colStatusText.Visible = true;
            this.colStatusText.VisibleIndex = 9;
            this.colStatusText.Width = 120;
            // 
            // colGroupCaption
            // 
            this.colGroupCaption.Caption = "Group Caption";
            this.colGroupCaption.FieldName = "GroupCaption";
            this.colGroupCaption.Name = "colGroupCaption";
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
            this.NamBarEditItem,
            this.ThangBarEditItem,
            this.XemBaoCaoBarButtonItem,
            this.barHeaderItem1,
            this.DataSummaryBarStaticItem,
            this.SelectedRowBarStaticItem,
            this.ExportFileBarButtonItem});
            this.barManager1.MainMenu = this.bar2;
            this.barManager1.MaxItemId = 19;
            this.barManager1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemSpinEdit1,
            this.repositoryItemSpinEdit2});
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
            new DevExpress.XtraBars.LinkPersistInfo(((DevExpress.XtraBars.BarLinkUserDefines)((DevExpress.XtraBars.BarLinkUserDefines.PaintStyle | DevExpress.XtraBars.BarLinkUserDefines.Width))), this.NamBarEditItem, "", false, true, true, 100, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(((DevExpress.XtraBars.BarLinkUserDefines)((DevExpress.XtraBars.BarLinkUserDefines.PaintStyle | DevExpress.XtraBars.BarLinkUserDefines.Width))), this.ThangBarEditItem, "", false, true, true, 100, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.XemBaoCaoBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(this.ExportFileBarButtonItem)});
            this.bar2.OptionsBar.MultiLine = true;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Main menu";
            // 
            // NamBarEditItem
            // 
            this.NamBarEditItem.Caption = "Năm";
            this.NamBarEditItem.Edit = this.repositoryItemSpinEdit1;
            this.NamBarEditItem.Id = 7;
            this.NamBarEditItem.Name = "NamBarEditItem";
            // 
            // repositoryItemSpinEdit1
            // 
            this.repositoryItemSpinEdit1.AutoHeight = false;
            this.repositoryItemSpinEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemSpinEdit1.MaxValue = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.repositoryItemSpinEdit1.MinValue = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.repositoryItemSpinEdit1.Name = "repositoryItemSpinEdit1";
            // 
            // ThangBarEditItem
            // 
            this.ThangBarEditItem.Caption = "Tháng";
            this.ThangBarEditItem.Edit = this.repositoryItemSpinEdit2;
            this.ThangBarEditItem.Id = 8;
            this.ThangBarEditItem.Name = "ThangBarEditItem";
            // 
            // repositoryItemSpinEdit2
            // 
            this.repositoryItemSpinEdit2.AutoHeight = false;
            this.repositoryItemSpinEdit2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemSpinEdit2.MaxValue = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.repositoryItemSpinEdit2.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.repositoryItemSpinEdit2.Name = "repositoryItemSpinEdit2";
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
            this.layoutControlItem1.Control = this.InventoryBalanceDtoGridControl;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(1015, 423);
            this.layoutControlItem1.TextVisible = false;
            // 
            // ExportFileBarButtonItem
            // 
            this.ExportFileBarButtonItem.Caption = "Xuất file";
            this.ExportFileBarButtonItem.Id = 18;
            this.ExportFileBarButtonItem.ImageOptions.Image = global::Inventory.Properties.Resources.exporttoxps_16x16;
            this.ExportFileBarButtonItem.Name = "ExportFileBarButtonItem";
            // 
            // FrmInventoryBalanceDto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1035, 489);
            this.Controls.Add(this.layoutControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "FrmInventoryBalanceDto";
            this.Text = "Tồn kho theo tháng";
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.InventoryBalanceDtoGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inventoryBalanceDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.InventoryBalanceDtoGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSpinEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSpinEdit2)).EndInit();
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
        private DevExpress.XtraGrid.GridControl InventoryBalanceDtoGridControl;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraBars.BarEditItem NamBarEditItem;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit repositoryItemSpinEdit1;
        private DevExpress.XtraBars.BarEditItem ThangBarEditItem;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit repositoryItemSpinEdit2;
        private DevExpress.XtraBars.BarButtonItem XemBaoCaoBarButtonItem;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarHeaderItem barHeaderItem1;
        private DevExpress.XtraBars.BarStaticItem DataSummaryBarStaticItem;
        private DevExpress.XtraBars.BarStaticItem SelectedRowBarStaticItem;
        private DevExpress.XtraGrid.Views.Grid.GridView InventoryBalanceDtoGridView;
        private DevExpress.XtraGrid.Columns.GridColumn colPeriodDisplay;
        private DevExpress.XtraGrid.Columns.GridColumn colWarehouseName;
        private DevExpress.XtraGrid.Columns.GridColumn colProductName;
        private DevExpress.XtraGrid.Columns.GridColumn colProductCode;
        private DevExpress.XtraGrid.Columns.GridColumn colOpeningBalance;
        private DevExpress.XtraGrid.Columns.GridColumn colTotalInQty;
        private DevExpress.XtraGrid.Columns.GridColumn colTotalOutQty;
        private DevExpress.XtraGrid.Columns.GridColumn colClosingBalance;
        private DevExpress.XtraGrid.Columns.GridColumn colStatusText;
        private DevExpress.XtraGrid.Columns.GridColumn colGroupCaption;
        private System.Windows.Forms.BindingSource inventoryBalanceDtoBindingSource;
        private DevExpress.XtraBars.BarButtonItem ExportFileBarButtonItem;
    }
}