using DevExpress.XtraBars;
using DevExpress.XtraDataLayout;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraLayout;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;
using DTO.Inventory.StockIn.NhapHangThuongMai;
using DTO.MasterData.ProductService;

namespace Inventory.InventoryManagement
{
    partial class FrmWarranty
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
            DevExpress.XtraLayout.ColumnDefinition columnDefinition1 = new DevExpress.XtraLayout.ColumnDefinition();
            DevExpress.XtraLayout.ColumnDefinition columnDefinition2 = new DevExpress.XtraLayout.ColumnDefinition();
            DevExpress.XtraLayout.ColumnDefinition columnDefinition3 = new DevExpress.XtraLayout.ColumnDefinition();
            DevExpress.XtraLayout.RowDefinition rowDefinition1 = new DevExpress.XtraLayout.RowDefinition();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.SaveBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.CloseBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.FormHotKeyBarStaticItem = new DevExpress.XtraBars.BarStaticItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.dxErrorProvider1 = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(this.components);
            this.dataLayoutControl1 = new DevExpress.XtraDataLayout.DataLayoutControl();
            this.dataLayoutControl2 = new DevExpress.XtraDataLayout.DataLayoutControl();
            this.ThemVaoHyperlinkLabelControl = new DevExpress.XtraEditors.HyperlinkLabelControl();
            this.BoRaHyperlinkLabelControl = new DevExpress.XtraEditors.HyperlinkLabelControl();
            this.WarrantyDtoGridControl = new DevExpress.XtraGrid.GridControl();
            this.warrantyDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.WarrantyDtoGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colWarrantyFrom = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMonthOfWarranty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colWarrantyUntil = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUniqueProductInfo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colProductVariantName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.WarrantyFromDateEdit = new DevExpress.XtraEditors.DateEdit();
            this.MonthOfWarrantyTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.WarrantyUntilDateEdit = new DevExpress.XtraEditors.DateEdit();
            this.UniqueProductInfoTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.StockInOutDetailIdSearchLookUpEdit = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.stockInDetailDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.StockInDetailSearchLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colFullNameHtml = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemHypertextLabel1 = new DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.ItemForUniqueProductInfo = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForStockInOutDetailId = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlGroup3 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.ItemForWarrantyFrom = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForMonthOfWarranty = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForWarrantyUntil = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.colId = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colCategoryCode = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colCategoryName = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colDescription = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colParentId = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colParentCategoryName = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colLevel = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colHasChildren = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colFullPath = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colProductCount = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).BeginInit();
            this.dataLayoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl2)).BeginInit();
            this.dataLayoutControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.WarrantyDtoGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.warrantyDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WarrantyDtoGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WarrantyFromDateEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WarrantyFromDateEdit.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MonthOfWarrantyTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WarrantyUntilDateEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WarrantyUntilDateEdit.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UniqueProductInfoTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StockInOutDetailIdSearchLookUpEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stockInDetailDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StockInDetailSearchLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemHypertextLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForUniqueProductInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForStockInOutDetailId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForWarrantyFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForMonthOfWarranty)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForWarrantyUntil)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
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
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.SaveBarButtonItem,
            this.CloseBarButtonItem,
            this.FormHotKeyBarStaticItem});
            this.barManager1.MainMenu = this.bar2;
            this.barManager1.MaxItemId = 4;
            // 
            // bar2
            // 
            this.bar2.BarName = "Main menu";
            this.bar2.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Top;
            this.bar2.DockCol = 0;
            this.bar2.DockRow = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.SaveBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.CloseBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(this.FormHotKeyBarStaticItem)});
            this.bar2.OptionsBar.MultiLine = true;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Main menu";
            // 
            // SaveBarButtonItem
            // 
            this.SaveBarButtonItem.Caption = "Lưu";
            this.SaveBarButtonItem.Id = 0;
            this.SaveBarButtonItem.ImageOptions.Image = global::Inventory.Properties.Resources.save_16x16;
            this.SaveBarButtonItem.Name = "SaveBarButtonItem";
            // 
            // CloseBarButtonItem
            // 
            this.CloseBarButtonItem.Caption = "Đóng";
            this.CloseBarButtonItem.Id = 1;
            this.CloseBarButtonItem.ImageOptions.Image = global::Inventory.Properties.Resources.cancel_16x16;
            this.CloseBarButtonItem.Name = "CloseBarButtonItem";
            // 
            // FormHotKeyBarStaticItem
            // 
            this.FormHotKeyBarStaticItem.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.FormHotKeyBarStaticItem.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True;
            this.FormHotKeyBarStaticItem.Caption = "barStaticItem1";
            this.FormHotKeyBarStaticItem.Id = 3;
            this.FormHotKeyBarStaticItem.Name = "FormHotKeyBarStaticItem";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager1;
            this.barDockControlTop.Size = new System.Drawing.Size(666, 24);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 660);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(666, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 24);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 636);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(666, 24);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 636);
            // 
            // dxErrorProvider1
            // 
            this.dxErrorProvider1.ContainerControl = this;
            // 
            // dataLayoutControl1
            // 
            this.dataLayoutControl1.Controls.Add(this.dataLayoutControl2);
            this.dataLayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataLayoutControl1.Location = new System.Drawing.Point(0, 24);
            this.dataLayoutControl1.Name = "dataLayoutControl1";
            this.dataLayoutControl1.Root = this.Root;
            this.dataLayoutControl1.Size = new System.Drawing.Size(666, 636);
            this.dataLayoutControl1.TabIndex = 10;
            this.dataLayoutControl1.Text = "dataLayoutControl1";
            // 
            // dataLayoutControl2
            // 
            this.dataLayoutControl2.Controls.Add(this.ThemVaoHyperlinkLabelControl);
            this.dataLayoutControl2.Controls.Add(this.BoRaHyperlinkLabelControl);
            this.dataLayoutControl2.Controls.Add(this.WarrantyDtoGridControl);
            this.dataLayoutControl2.Controls.Add(this.WarrantyFromDateEdit);
            this.dataLayoutControl2.Controls.Add(this.MonthOfWarrantyTextEdit);
            this.dataLayoutControl2.Controls.Add(this.WarrantyUntilDateEdit);
            this.dataLayoutControl2.Controls.Add(this.UniqueProductInfoTextEdit);
            this.dataLayoutControl2.Controls.Add(this.StockInOutDetailIdSearchLookUpEdit);
            this.dataLayoutControl2.Location = new System.Drawing.Point(12, 12);
            this.dataLayoutControl2.Name = "dataLayoutControl2";
            this.dataLayoutControl2.Root = this.layoutControlGroup1;
            this.dataLayoutControl2.Size = new System.Drawing.Size(642, 612);
            this.dataLayoutControl2.TabIndex = 0;
            this.dataLayoutControl2.Text = "dataLayoutControl2";
            // 
            // ThemVaoHyperlinkLabelControl
            // 
            this.ThemVaoHyperlinkLabelControl.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.ThemVaoHyperlinkLabelControl.ImageOptions.Image = global::Inventory.Properties.Resources.add_16x16;
            this.ThemVaoHyperlinkLabelControl.Location = new System.Drawing.Point(488, 105);
            this.ThemVaoHyperlinkLabelControl.Name = "ThemVaoHyperlinkLabelControl";
            this.ThemVaoHyperlinkLabelControl.Size = new System.Drawing.Size(68, 20);
            this.ThemVaoHyperlinkLabelControl.StyleController = this.dataLayoutControl2;
            this.ThemVaoHyperlinkLabelControl.TabIndex = 12;
            this.ThemVaoHyperlinkLabelControl.Text = "Thêm vào";
            // 
            // BoRaHyperlinkLabelControl
            // 
            this.BoRaHyperlinkLabelControl.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.BoRaHyperlinkLabelControl.ImageOptions.Image = global::Inventory.Properties.Resources.clear_16x16;
            this.BoRaHyperlinkLabelControl.Location = new System.Drawing.Point(576, 105);
            this.BoRaHyperlinkLabelControl.Name = "BoRaHyperlinkLabelControl";
            this.BoRaHyperlinkLabelControl.Size = new System.Drawing.Size(46, 20);
            this.BoRaHyperlinkLabelControl.StyleController = this.dataLayoutControl2;
            this.BoRaHyperlinkLabelControl.TabIndex = 11;
            this.BoRaHyperlinkLabelControl.Text = "Bỏ ra";
            // 
            // WarrantyDtoGridControl
            // 
            this.WarrantyDtoGridControl.DataSource = this.warrantyDtoBindingSource;
            this.WarrantyDtoGridControl.Location = new System.Drawing.Point(12, 129);
            this.WarrantyDtoGridControl.MainView = this.WarrantyDtoGridView;
            this.WarrantyDtoGridControl.MenuManager = this.barManager1;
            this.WarrantyDtoGridControl.Name = "WarrantyDtoGridControl";
            this.WarrantyDtoGridControl.Size = new System.Drawing.Size(618, 471);
            this.WarrantyDtoGridControl.TabIndex = 10;
            this.WarrantyDtoGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.WarrantyDtoGridView});
            // 
            // warrantyDtoBindingSource
            // 
            this.warrantyDtoBindingSource.DataSource = typeof(DTO.Inventory.InventoryManagement.WarrantyDto);
            // 
            // WarrantyDtoGridView
            // 
            this.WarrantyDtoGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colWarrantyFrom,
            this.colMonthOfWarranty,
            this.colWarrantyUntil,
            this.colUniqueProductInfo,
            this.colProductVariantName});
            this.WarrantyDtoGridView.GridControl = this.WarrantyDtoGridControl;
            this.WarrantyDtoGridView.GroupCount = 1;
            this.WarrantyDtoGridView.Name = "WarrantyDtoGridView";
            this.WarrantyDtoGridView.OptionsSelection.MultiSelect = true;
            this.WarrantyDtoGridView.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            this.WarrantyDtoGridView.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.colProductVariantName, DevExpress.Data.ColumnSortOrder.Ascending)});
            // 
            // colWarrantyFrom
            // 
            this.colWarrantyFrom.AppearanceCell.Options.UseTextOptions = true;
            this.colWarrantyFrom.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colWarrantyFrom.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colWarrantyFrom.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colWarrantyFrom.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colWarrantyFrom.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colWarrantyFrom.AppearanceHeader.Options.UseBackColor = true;
            this.colWarrantyFrom.AppearanceHeader.Options.UseFont = true;
            this.colWarrantyFrom.AppearanceHeader.Options.UseForeColor = true;
            this.colWarrantyFrom.AppearanceHeader.Options.UseTextOptions = true;
            this.colWarrantyFrom.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colWarrantyFrom.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colWarrantyFrom.Caption = "Từ ngày";
            this.colWarrantyFrom.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.colWarrantyFrom.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.colWarrantyFrom.FieldName = "WarrantyFrom";
            this.colWarrantyFrom.Name = "colWarrantyFrom";
            this.colWarrantyFrom.Visible = true;
            this.colWarrantyFrom.VisibleIndex = 2;
            this.colWarrantyFrom.Width = 120;
            // 
            // colMonthOfWarranty
            // 
            this.colMonthOfWarranty.AppearanceCell.Options.UseTextOptions = true;
            this.colMonthOfWarranty.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colMonthOfWarranty.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colMonthOfWarranty.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colMonthOfWarranty.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colMonthOfWarranty.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colMonthOfWarranty.AppearanceHeader.Options.UseBackColor = true;
            this.colMonthOfWarranty.AppearanceHeader.Options.UseFont = true;
            this.colMonthOfWarranty.AppearanceHeader.Options.UseForeColor = true;
            this.colMonthOfWarranty.AppearanceHeader.Options.UseTextOptions = true;
            this.colMonthOfWarranty.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colMonthOfWarranty.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colMonthOfWarranty.Caption = "Số tháng BH";
            this.colMonthOfWarranty.DisplayFormat.FormatString = "N0";
            this.colMonthOfWarranty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colMonthOfWarranty.FieldName = "MonthOfWarranty";
            this.colMonthOfWarranty.Name = "colMonthOfWarranty";
            this.colMonthOfWarranty.Visible = true;
            this.colMonthOfWarranty.VisibleIndex = 3;
            this.colMonthOfWarranty.Width = 100;
            // 
            // colWarrantyUntil
            // 
            this.colWarrantyUntil.AppearanceCell.Options.UseTextOptions = true;
            this.colWarrantyUntil.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colWarrantyUntil.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colWarrantyUntil.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colWarrantyUntil.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colWarrantyUntil.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colWarrantyUntil.AppearanceHeader.Options.UseBackColor = true;
            this.colWarrantyUntil.AppearanceHeader.Options.UseFont = true;
            this.colWarrantyUntil.AppearanceHeader.Options.UseForeColor = true;
            this.colWarrantyUntil.AppearanceHeader.Options.UseTextOptions = true;
            this.colWarrantyUntil.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colWarrantyUntil.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colWarrantyUntil.Caption = "Đến ngày";
            this.colWarrantyUntil.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.colWarrantyUntil.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.colWarrantyUntil.FieldName = "WarrantyUntil";
            this.colWarrantyUntil.Name = "colWarrantyUntil";
            this.colWarrantyUntil.Visible = true;
            this.colWarrantyUntil.VisibleIndex = 4;
            this.colWarrantyUntil.Width = 120;
            // 
            // colUniqueProductInfo
            // 
            this.colUniqueProductInfo.AppearanceCell.Options.UseTextOptions = true;
            this.colUniqueProductInfo.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colUniqueProductInfo.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colUniqueProductInfo.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colUniqueProductInfo.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colUniqueProductInfo.AppearanceHeader.Options.UseBackColor = true;
            this.colUniqueProductInfo.AppearanceHeader.Options.UseFont = true;
            this.colUniqueProductInfo.AppearanceHeader.Options.UseForeColor = true;
            this.colUniqueProductInfo.AppearanceHeader.Options.UseTextOptions = true;
            this.colUniqueProductInfo.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colUniqueProductInfo.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colUniqueProductInfo.Caption = "S/N | MAC | Part. No";
            this.colUniqueProductInfo.FieldName = "UniqueProductInfo";
            this.colUniqueProductInfo.Name = "colUniqueProductInfo";
            this.colUniqueProductInfo.Visible = true;
            this.colUniqueProductInfo.VisibleIndex = 1;
            this.colUniqueProductInfo.Width = 200;
            // 
            // colProductVariantName
            // 
            this.colProductVariantName.AppearanceCell.Options.UseTextOptions = true;
            this.colProductVariantName.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colProductVariantName.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colProductVariantName.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colProductVariantName.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colProductVariantName.AppearanceHeader.Options.UseBackColor = true;
            this.colProductVariantName.AppearanceHeader.Options.UseFont = true;
            this.colProductVariantName.AppearanceHeader.Options.UseForeColor = true;
            this.colProductVariantName.AppearanceHeader.Options.UseTextOptions = true;
            this.colProductVariantName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colProductVariantName.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colProductVariantName.Caption = "Tên sản phẩm";
            this.colProductVariantName.FieldName = "ProductVariantName";
            this.colProductVariantName.Name = "colProductVariantName";
            this.colProductVariantName.Visible = true;
            this.colProductVariantName.VisibleIndex = 4;
            this.colProductVariantName.Width = 250;
            // 
            // WarrantyFromDateEdit
            // 
            this.WarrantyFromDateEdit.EditValue = null;
            this.WarrantyFromDateEdit.Location = new System.Drawing.Point(77, 69);
            this.WarrantyFromDateEdit.MenuManager = this.barManager1;
            this.WarrantyFromDateEdit.Name = "WarrantyFromDateEdit";
            this.WarrantyFromDateEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.WarrantyFromDateEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.WarrantyFromDateEdit.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.WarrantyFromDateEdit.Size = new System.Drawing.Size(143, 20);
            this.WarrantyFromDateEdit.StyleController = this.dataLayoutControl2;
            this.WarrantyFromDateEdit.TabIndex = 4;
            // 
            // MonthOfWarrantyTextEdit
            // 
            this.MonthOfWarrantyTextEdit.Location = new System.Drawing.Point(325, 69);
            this.MonthOfWarrantyTextEdit.MenuManager = this.barManager1;
            this.MonthOfWarrantyTextEdit.Name = "MonthOfWarrantyTextEdit";
            this.MonthOfWarrantyTextEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.MonthOfWarrantyTextEdit.Properties.Appearance.Options.UseTextOptions = true;
            this.MonthOfWarrantyTextEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.MonthOfWarrantyTextEdit.Properties.Mask.EditMask = "N0";
            this.MonthOfWarrantyTextEdit.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.MonthOfWarrantyTextEdit.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.MonthOfWarrantyTextEdit.Size = new System.Drawing.Size(45, 20);
            this.MonthOfWarrantyTextEdit.StyleController = this.dataLayoutControl2;
            this.MonthOfWarrantyTextEdit.TabIndex = 5;
            // 
            // WarrantyUntilDateEdit
            // 
            this.WarrantyUntilDateEdit.EditValue = null;
            this.WarrantyUntilDateEdit.Location = new System.Drawing.Point(434, 69);
            this.WarrantyUntilDateEdit.MenuManager = this.barManager1;
            this.WarrantyUntilDateEdit.Name = "WarrantyUntilDateEdit";
            this.WarrantyUntilDateEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.WarrantyUntilDateEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.WarrantyUntilDateEdit.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.WarrantyUntilDateEdit.Size = new System.Drawing.Size(184, 20);
            this.WarrantyUntilDateEdit.StyleController = this.dataLayoutControl2;
            this.WarrantyUntilDateEdit.TabIndex = 6;
            // 
            // UniqueProductInfoTextEdit
            // 
            this.UniqueProductInfoTextEdit.Location = new System.Drawing.Point(127, 105);
            this.UniqueProductInfoTextEdit.MenuManager = this.barManager1;
            this.UniqueProductInfoTextEdit.Name = "UniqueProductInfoTextEdit";
            this.UniqueProductInfoTextEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.UniqueProductInfoTextEdit.Size = new System.Drawing.Size(349, 20);
            this.UniqueProductInfoTextEdit.StyleController = this.dataLayoutControl2;
            this.UniqueProductInfoTextEdit.TabIndex = 7;
            // 
            // StockInOutDetailIdSearchLookUpEdit
            // 
            this.StockInOutDetailIdSearchLookUpEdit.Location = new System.Drawing.Point(127, 12);
            this.StockInOutDetailIdSearchLookUpEdit.MenuManager = this.barManager1;
            this.StockInOutDetailIdSearchLookUpEdit.Name = "StockInOutDetailIdSearchLookUpEdit";
            this.StockInOutDetailIdSearchLookUpEdit.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.StockInOutDetailIdSearchLookUpEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.StockInOutDetailIdSearchLookUpEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.StockInOutDetailIdSearchLookUpEdit.Properties.DataSource = this.stockInDetailDtoBindingSource;
            this.StockInOutDetailIdSearchLookUpEdit.Properties.DisplayMember = "FullNameHtml";
            this.StockInOutDetailIdSearchLookUpEdit.Properties.NullText = "";
            this.StockInOutDetailIdSearchLookUpEdit.Properties.PopupView = this.StockInDetailSearchLookUpEdit1View;
            this.StockInOutDetailIdSearchLookUpEdit.Properties.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemHypertextLabel1});
            this.StockInOutDetailIdSearchLookUpEdit.Properties.ValueMember = "Id";
            this.StockInOutDetailIdSearchLookUpEdit.Size = new System.Drawing.Size(503, 20);
            this.StockInOutDetailIdSearchLookUpEdit.StyleController = this.dataLayoutControl2;
            this.StockInOutDetailIdSearchLookUpEdit.TabIndex = 9;
            // 
            // stockInDetailDtoBindingSource
            // 
            this.stockInDetailDtoBindingSource.DataSource = typeof(StockInDetailDto);
            // 
            // StockInDetailSearchLookUpEdit1View
            // 
            this.StockInDetailSearchLookUpEdit1View.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colFullNameHtml});
            this.StockInDetailSearchLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.StockInDetailSearchLookUpEdit1View.Name = "StockInDetailSearchLookUpEdit1View";
            this.StockInDetailSearchLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.StockInDetailSearchLookUpEdit1View.OptionsView.RowAutoHeight = true;
            this.StockInDetailSearchLookUpEdit1View.OptionsView.ShowAutoFilterRow = true;
            this.StockInDetailSearchLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // colFullNameHtml
            // 
            this.colFullNameHtml.ColumnEdit = this.repositoryItemHypertextLabel1;
            this.colFullNameHtml.FieldName = "FullNameHtml";
            this.colFullNameHtml.Name = "colFullNameHtml";
            this.colFullNameHtml.Visible = true;
            this.colFullNameHtml.VisibleIndex = 0;
            // 
            // repositoryItemHypertextLabel1
            // 
            this.repositoryItemHypertextLabel1.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.repositoryItemHypertextLabel1.Name = "repositoryItemHypertextLabel1";
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup2});
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Size = new System.Drawing.Size(642, 612);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlGroup2
            // 
            this.layoutControlGroup2.AllowDrawBackground = false;
            this.layoutControlGroup2.GroupBordersVisible = false;
            this.layoutControlGroup2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.ItemForUniqueProductInfo,
            this.ItemForStockInOutDetailId,
            this.layoutControlItem2,
            this.layoutControlGroup3,
            this.layoutControlItem3,
            this.layoutControlItem4});
            this.layoutControlGroup2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup2.Name = "autoGeneratedGroup0";
            this.layoutControlGroup2.Size = new System.Drawing.Size(622, 592);
            // 
            // ItemForUniqueProductInfo
            // 
            this.ItemForUniqueProductInfo.Control = this.UniqueProductInfoTextEdit;
            this.ItemForUniqueProductInfo.Location = new System.Drawing.Point(0, 93);
            this.ItemForUniqueProductInfo.Name = "ItemForUniqueProductInfo";
            this.ItemForUniqueProductInfo.Padding = new DevExpress.XtraLayout.Utils.Padding(10, 2, 2, 2);
            this.ItemForUniqueProductInfo.Size = new System.Drawing.Size(468, 24);
            this.ItemForUniqueProductInfo.Text = "S/N | Mac | Part.No";
            this.ItemForUniqueProductInfo.TextSize = new System.Drawing.Size(95, 13);
            // 
            // ItemForStockInOutDetailId
            // 
            this.ItemForStockInOutDetailId.Control = this.StockInOutDetailIdSearchLookUpEdit;
            this.ItemForStockInOutDetailId.Location = new System.Drawing.Point(0, 0);
            this.ItemForStockInOutDetailId.Name = "ItemForStockInOutDetailId";
            this.ItemForStockInOutDetailId.Padding = new DevExpress.XtraLayout.Utils.Padding(10, 2, 2, 2);
            this.ItemForStockInOutDetailId.Size = new System.Drawing.Size(622, 24);
            this.ItemForStockInOutDetailId.Text = "Sản phẩm bảo hành";
            this.ItemForStockInOutDetailId.TextSize = new System.Drawing.Size(95, 13);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.WarrantyDtoGridControl;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 117);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(622, 475);
            this.layoutControlItem2.TextVisible = false;
            // 
            // layoutControlGroup3
            // 
            this.layoutControlGroup3.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.layoutControlGroup3.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.ItemForWarrantyFrom,
            this.ItemForMonthOfWarranty,
            this.ItemForWarrantyUntil});
            this.layoutControlGroup3.LayoutMode = DevExpress.XtraLayout.Utils.LayoutMode.Table;
            this.layoutControlGroup3.Location = new System.Drawing.Point(0, 24);
            this.layoutControlGroup3.Name = "layoutControlGroup3";
            columnDefinition1.SizeType = System.Windows.Forms.SizeType.Absolute;
            columnDefinition1.Width = 200D;
            columnDefinition2.SizeType = System.Windows.Forms.SizeType.Absolute;
            columnDefinition2.Width = 150D;
            columnDefinition3.SizeType = System.Windows.Forms.SizeType.AutoSize;
            columnDefinition3.Width = 248D;
            this.layoutControlGroup3.OptionsTableLayoutGroup.ColumnDefinitions.AddRange(new DevExpress.XtraLayout.ColumnDefinition[] {
            columnDefinition1,
            columnDefinition2,
            columnDefinition3});
            rowDefinition1.Height = 24D;
            rowDefinition1.SizeType = System.Windows.Forms.SizeType.AutoSize;
            this.layoutControlGroup3.OptionsTableLayoutGroup.RowDefinitions.AddRange(new DevExpress.XtraLayout.RowDefinition[] {
            rowDefinition1});
            this.layoutControlGroup3.Size = new System.Drawing.Size(622, 69);
            this.layoutControlGroup3.Text = "Thời gian bảo hành";
            // 
            // ItemForWarrantyFrom
            // 
            this.ItemForWarrantyFrom.Control = this.WarrantyFromDateEdit;
            this.ItemForWarrantyFrom.Location = new System.Drawing.Point(0, 0);
            this.ItemForWarrantyFrom.Name = "ItemForWarrantyFrom";
            this.ItemForWarrantyFrom.Padding = new DevExpress.XtraLayout.Utils.Padding(10, 2, 2, 2);
            this.ItemForWarrantyFrom.Size = new System.Drawing.Size(200, 24);
            this.ItemForWarrantyFrom.Text = "Từ ngày";
            this.ItemForWarrantyFrom.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.ItemForWarrantyFrom.TextSize = new System.Drawing.Size(40, 13);
            this.ItemForWarrantyFrom.TextToControlDistance = 5;
            // 
            // ItemForMonthOfWarranty
            // 
            this.ItemForMonthOfWarranty.Control = this.MonthOfWarrantyTextEdit;
            this.ItemForMonthOfWarranty.Location = new System.Drawing.Point(200, 0);
            this.ItemForMonthOfWarranty.Name = "ItemForMonthOfWarranty";
            this.ItemForMonthOfWarranty.OptionsTableLayoutItem.ColumnIndex = 1;
            this.ItemForMonthOfWarranty.Padding = new DevExpress.XtraLayout.Utils.Padding(10, 2, 2, 2);
            this.ItemForMonthOfWarranty.Size = new System.Drawing.Size(150, 24);
            this.ItemForMonthOfWarranty.Text = "T.Gian BH (Tháng)";
            this.ItemForMonthOfWarranty.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.ItemForMonthOfWarranty.TextSize = new System.Drawing.Size(88, 13);
            this.ItemForMonthOfWarranty.TextToControlDistance = 5;
            // 
            // ItemForWarrantyUntil
            // 
            this.ItemForWarrantyUntil.Control = this.WarrantyUntilDateEdit;
            this.ItemForWarrantyUntil.Location = new System.Drawing.Point(350, 0);
            this.ItemForWarrantyUntil.Name = "ItemForWarrantyUntil";
            this.ItemForWarrantyUntil.OptionsTableLayoutItem.ColumnIndex = 2;
            this.ItemForWarrantyUntil.Padding = new DevExpress.XtraLayout.Utils.Padding(10, 2, 2, 2);
            this.ItemForWarrantyUntil.Size = new System.Drawing.Size(248, 24);
            this.ItemForWarrantyUntil.Text = "Đến ngày";
            this.ItemForWarrantyUntil.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.ItemForWarrantyUntil.TextSize = new System.Drawing.Size(47, 13);
            this.ItemForWarrantyUntil.TextToControlDistance = 5;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlItem3.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlItem3.Control = this.BoRaHyperlinkLabelControl;
            this.layoutControlItem3.Location = new System.Drawing.Point(556, 93);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Padding = new DevExpress.XtraLayout.Utils.Padding(10, 10, 2, 2);
            this.layoutControlItem3.Size = new System.Drawing.Size(66, 24);
            this.layoutControlItem3.TextVisible = false;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlItem4.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlItem4.Control = this.ThemVaoHyperlinkLabelControl;
            this.layoutControlItem4.Location = new System.Drawing.Point(468, 93);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Padding = new DevExpress.XtraLayout.Utils.Padding(10, 10, 2, 2);
            this.layoutControlItem4.Size = new System.Drawing.Size(88, 24);
            this.layoutControlItem4.TextVisible = false;
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(666, 636);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.dataLayoutControl2;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(646, 616);
            this.layoutControlItem1.TextVisible = false;
            // 
            // colId
            // 
            this.colId.FieldName = "Id";
            this.colId.Name = "colId";
            this.colId.Visible = true;
            this.colId.VisibleIndex = 0;
            // 
            // colCategoryCode
            // 
            this.colCategoryCode.FieldName = "CategoryCode";
            this.colCategoryCode.Name = "colCategoryCode";
            this.colCategoryCode.Visible = true;
            this.colCategoryCode.VisibleIndex = 1;
            // 
            // colCategoryName
            // 
            this.colCategoryName.FieldName = "CategoryName";
            this.colCategoryName.Name = "colCategoryName";
            this.colCategoryName.Visible = true;
            this.colCategoryName.VisibleIndex = 2;
            // 
            // colDescription
            // 
            this.colDescription.FieldName = "Description";
            this.colDescription.Name = "colDescription";
            this.colDescription.Visible = true;
            this.colDescription.VisibleIndex = 3;
            // 
            // colParentId
            // 
            this.colParentId.FieldName = "ParentId";
            this.colParentId.Name = "colParentId";
            this.colParentId.Visible = true;
            this.colParentId.VisibleIndex = 4;
            // 
            // colParentCategoryName
            // 
            this.colParentCategoryName.FieldName = "ParentCategoryName";
            this.colParentCategoryName.Name = "colParentCategoryName";
            this.colParentCategoryName.Visible = true;
            this.colParentCategoryName.VisibleIndex = 5;
            // 
            // colLevel
            // 
            this.colLevel.FieldName = "Level";
            this.colLevel.Name = "colLevel";
            this.colLevel.Visible = true;
            this.colLevel.VisibleIndex = 6;
            // 
            // colHasChildren
            // 
            this.colHasChildren.FieldName = "HasChildren";
            this.colHasChildren.Name = "colHasChildren";
            this.colHasChildren.Visible = true;
            this.colHasChildren.VisibleIndex = 7;
            // 
            // colFullPath
            // 
            this.colFullPath.FieldName = "FullPath";
            this.colFullPath.Name = "colFullPath";
            this.colFullPath.Visible = true;
            this.colFullPath.VisibleIndex = 8;
            // 
            // colProductCount
            // 
            this.colProductCount.FieldName = "ProductCount";
            this.colProductCount.Name = "colProductCount";
            this.colProductCount.Visible = true;
            this.colProductCount.VisibleIndex = 9;
            // 
            // FrmWarranty
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(666, 660);
            this.Controls.Add(this.dataLayoutControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmWarranty";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BẢO HÀNH SẢN PHẨM";
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).EndInit();
            this.dataLayoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl2)).EndInit();
            this.dataLayoutControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.WarrantyDtoGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.warrantyDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WarrantyDtoGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WarrantyFromDateEdit.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WarrantyFromDateEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MonthOfWarrantyTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WarrantyUntilDateEdit.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WarrantyUntilDateEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UniqueProductInfoTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StockInOutDetailIdSearchLookUpEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stockInDetailDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StockInDetailSearchLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemHypertextLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForUniqueProductInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForStockInOutDetailId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForWarrantyFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForMonthOfWarranty)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForWarrantyUntil)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private BarManager barManager1;
        private Bar bar2;
        private BarButtonItem SaveBarButtonItem;
        private BarButtonItem CloseBarButtonItem;
        private BarDockControl barDockControlTop;
        private BarDockControl barDockControlBottom;
        private BarDockControl barDockControlLeft;
        private BarDockControl barDockControlRight;
        private DXErrorProvider dxErrorProvider1;
        private DataLayoutControl dataLayoutControl1;
        private LayoutControlGroup Root;
        private TreeListColumn colId;
        private TreeListColumn colCategoryCode;
        private TreeListColumn colCategoryName;
        private TreeListColumn colDescription;
        private TreeListColumn colParentId;
        private TreeListColumn colParentCategoryName;
        private TreeListColumn colLevel;
        private TreeListColumn colHasChildren;
        private TreeListColumn colFullPath;
        private TreeListColumn colProductCount;
        private DataLayoutControl dataLayoutControl2;
        private LayoutControlGroup layoutControlGroup1;
        private LayoutControlItem layoutControlItem1;
        private DateEdit WarrantyFromDateEdit;
        private TextEdit MonthOfWarrantyTextEdit;
        private DateEdit WarrantyUntilDateEdit;
        private TextEdit UniqueProductInfoTextEdit;
        private LayoutControlGroup layoutControlGroup2;
        private LayoutControlItem ItemForWarrantyFrom;
        private LayoutControlItem ItemForMonthOfWarranty;
        private LayoutControlItem ItemForWarrantyUntil;
        private LayoutControlItem ItemForUniqueProductInfo;
        private DevExpress.XtraGrid.GridControl WarrantyDtoGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView WarrantyDtoGridView;
        private SearchLookUpEdit StockInOutDetailIdSearchLookUpEdit;
        private DevExpress.XtraGrid.Views.Grid.GridView StockInDetailSearchLookUpEdit1View;
        private LayoutControlItem ItemForStockInOutDetailId;
        private LayoutControlItem layoutControlItem2;
        private LayoutControlGroup layoutControlGroup3;
        private HyperlinkLabelControl ThemVaoHyperlinkLabelControl;
        private HyperlinkLabelControl BoRaHyperlinkLabelControl;
        private LayoutControlItem layoutControlItem3;
        private LayoutControlItem layoutControlItem4;
        private System.Windows.Forms.BindingSource warrantyDtoBindingSource;
        private DevExpress.XtraGrid.Columns.GridColumn colWarrantyFrom;
        private DevExpress.XtraGrid.Columns.GridColumn colMonthOfWarranty;
        private DevExpress.XtraGrid.Columns.GridColumn colWarrantyUntil;
        private DevExpress.XtraGrid.Columns.GridColumn colUniqueProductInfo;
        private System.Windows.Forms.BindingSource stockInDetailDtoBindingSource;
        private BarStaticItem FormHotKeyBarStaticItem;
        private DevExpress.XtraGrid.Columns.GridColumn colProductVariantName;
        private DevExpress.XtraGrid.Columns.GridColumn colFullNameHtml;
        private DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel repositoryItemHypertextLabel1;
    }
}