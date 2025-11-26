using DevExpress.XtraBars;
using DevExpress.XtraDataLayout;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraLayout;
using DevExpress.XtraTreeList.Columns;
using DTO.Inventory.StockIn.NhapHangThuongMai;

namespace Inventory.StockIn.NhapThietBiMuon
{
    partial class FrmNhapSerialMacEmei
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
            this.deviceDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.WarrantyDtoGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.UniqueProductInfoTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.StockInOutDetailIdSearchLookUpEdit = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.stockInOutProductHistoryDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.StockInDetailSearchLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colFullNameHtml = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemHypertextLabel1 = new DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel();
            this.DeviceIdentifierComboBoxEdit = new DevExpress.XtraEditors.ComboBoxEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.ItemForStockInOutDetailId = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlGroup3 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForUniqueProductInfo = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
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
            this.nhapThietBiMuonDetailDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.colFullInfo = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).BeginInit();
            this.dataLayoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl2)).BeginInit();
            this.dataLayoutControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.WarrantyDtoGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deviceDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WarrantyDtoGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UniqueProductInfoTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StockInOutDetailIdSearchLookUpEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stockInOutProductHistoryDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StockInDetailSearchLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemHypertextLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DeviceIdentifierComboBoxEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForStockInOutDetailId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForUniqueProductInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nhapThietBiMuonDetailDtoBindingSource)).BeginInit();
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
            this.dataLayoutControl2.Controls.Add(this.UniqueProductInfoTextEdit);
            this.dataLayoutControl2.Controls.Add(this.StockInOutDetailIdSearchLookUpEdit);
            this.dataLayoutControl2.Controls.Add(this.DeviceIdentifierComboBoxEdit);
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
            this.ThemVaoHyperlinkLabelControl.Location = new System.Drawing.Point(476, 69);
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
            this.BoRaHyperlinkLabelControl.Location = new System.Drawing.Point(564, 69);
            this.BoRaHyperlinkLabelControl.Name = "BoRaHyperlinkLabelControl";
            this.BoRaHyperlinkLabelControl.Size = new System.Drawing.Size(46, 20);
            this.BoRaHyperlinkLabelControl.StyleController = this.dataLayoutControl2;
            this.BoRaHyperlinkLabelControl.TabIndex = 11;
            this.BoRaHyperlinkLabelControl.Text = "Bỏ ra";
            // 
            // WarrantyDtoGridControl
            // 
            this.WarrantyDtoGridControl.DataSource = this.deviceDtoBindingSource;
            this.WarrantyDtoGridControl.Location = new System.Drawing.Point(12, 105);
            this.WarrantyDtoGridControl.MainView = this.WarrantyDtoGridView;
            this.WarrantyDtoGridControl.MenuManager = this.barManager1;
            this.WarrantyDtoGridControl.Name = "WarrantyDtoGridControl";
            this.WarrantyDtoGridControl.Size = new System.Drawing.Size(618, 495);
            this.WarrantyDtoGridControl.TabIndex = 10;
            this.WarrantyDtoGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.WarrantyDtoGridView});
            // 
            // deviceDtoBindingSource
            // 
            this.deviceDtoBindingSource.DataSource = typeof(DTO.Inventory.InventoryManagement.DeviceDto);
            // 
            // WarrantyDtoGridView
            // 
            this.WarrantyDtoGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colFullInfo});
            this.WarrantyDtoGridView.GridControl = this.WarrantyDtoGridControl;
            this.WarrantyDtoGridView.Name = "WarrantyDtoGridView";
            this.WarrantyDtoGridView.OptionsSelection.MultiSelect = true;
            this.WarrantyDtoGridView.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            // 
            // UniqueProductInfoTextEdit
            // 
            this.UniqueProductInfoTextEdit.Location = new System.Drawing.Point(223, 69);
            this.UniqueProductInfoTextEdit.MenuManager = this.barManager1;
            this.UniqueProductInfoTextEdit.Name = "UniqueProductInfoTextEdit";
            this.UniqueProductInfoTextEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.UniqueProductInfoTextEdit.Size = new System.Drawing.Size(241, 20);
            this.UniqueProductInfoTextEdit.StyleController = this.dataLayoutControl2;
            this.UniqueProductInfoTextEdit.TabIndex = 7;
            // 
            // StockInOutDetailIdSearchLookUpEdit
            // 
            this.StockInOutDetailIdSearchLookUpEdit.Location = new System.Drawing.Point(112, 12);
            this.StockInOutDetailIdSearchLookUpEdit.MenuManager = this.barManager1;
            this.StockInOutDetailIdSearchLookUpEdit.Name = "StockInOutDetailIdSearchLookUpEdit";
            this.StockInOutDetailIdSearchLookUpEdit.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.StockInOutDetailIdSearchLookUpEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.StockInOutDetailIdSearchLookUpEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.StockInOutDetailIdSearchLookUpEdit.Properties.DataSource = this.stockInOutProductHistoryDtoBindingSource;
            this.StockInOutDetailIdSearchLookUpEdit.Properties.DisplayMember = "ProductVariantFullNameHtml";
            this.StockInOutDetailIdSearchLookUpEdit.Properties.NullText = "";
            this.StockInOutDetailIdSearchLookUpEdit.Properties.PopupView = this.StockInDetailSearchLookUpEdit1View;
            this.StockInOutDetailIdSearchLookUpEdit.Properties.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemHypertextLabel1});
            this.StockInOutDetailIdSearchLookUpEdit.Properties.ValueMember = "Id";
            this.StockInOutDetailIdSearchLookUpEdit.Size = new System.Drawing.Size(518, 20);
            this.StockInOutDetailIdSearchLookUpEdit.StyleController = this.dataLayoutControl2;
            this.StockInOutDetailIdSearchLookUpEdit.TabIndex = 9;
            // 
            // stockInOutProductHistoryDtoBindingSource
            // 
            this.stockInOutProductHistoryDtoBindingSource.DataSource = typeof(DTO.Inventory.InventoryManagement.StockInOutProductHistoryDto);
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
            // DeviceIdentifierComboBoxEdit
            // 
            this.DeviceIdentifierComboBoxEdit.Location = new System.Drawing.Point(59, 69);
            this.DeviceIdentifierComboBoxEdit.MenuManager = this.barManager1;
            this.DeviceIdentifierComboBoxEdit.Name = "DeviceIdentifierComboBoxEdit";
            this.DeviceIdentifierComboBoxEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.DeviceIdentifierComboBoxEdit.Size = new System.Drawing.Size(114, 20);
            this.DeviceIdentifierComboBoxEdit.StyleController = this.dataLayoutControl2;
            this.DeviceIdentifierComboBoxEdit.TabIndex = 13;
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
            this.ItemForStockInOutDetailId,
            this.layoutControlItem2,
            this.layoutControlGroup3});
            this.layoutControlGroup2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup2.Name = "autoGeneratedGroup0";
            this.layoutControlGroup2.Size = new System.Drawing.Size(622, 592);
            // 
            // ItemForStockInOutDetailId
            // 
            this.ItemForStockInOutDetailId.Control = this.StockInOutDetailIdSearchLookUpEdit;
            this.ItemForStockInOutDetailId.Location = new System.Drawing.Point(0, 0);
            this.ItemForStockInOutDetailId.Name = "ItemForStockInOutDetailId";
            this.ItemForStockInOutDetailId.Padding = new DevExpress.XtraLayout.Utils.Padding(10, 2, 2, 2);
            this.ItemForStockInOutDetailId.Size = new System.Drawing.Size(622, 24);
            this.ItemForStockInOutDetailId.Text = "Thông tin tài sản";
            this.ItemForStockInOutDetailId.TextSize = new System.Drawing.Size(80, 13);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.WarrantyDtoGridControl;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 93);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(622, 499);
            this.layoutControlItem2.TextVisible = false;
            // 
            // layoutControlGroup3
            // 
            this.layoutControlGroup3.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.layoutControlGroup3.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem5,
            this.ItemForUniqueProductInfo,
            this.layoutControlItem4,
            this.layoutControlItem3});
            this.layoutControlGroup3.Location = new System.Drawing.Point(0, 24);
            this.layoutControlGroup3.Name = "layoutControlGroup3";
            this.layoutControlGroup3.Size = new System.Drawing.Size(622, 69);
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.DeviceIdentifierComboBoxEdit;
            this.layoutControlItem5.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(158, 24);
            this.layoutControlItem5.Spacing = new DevExpress.XtraLayout.Utils.Padding(5, 5, 0, 0);
            this.layoutControlItem5.Text = "Chọn";
            this.layoutControlItem5.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem5.TextSize = new System.Drawing.Size(25, 13);
            this.layoutControlItem5.TextToControlDistance = 5;
            // 
            // ItemForUniqueProductInfo
            // 
            this.ItemForUniqueProductInfo.Control = this.UniqueProductInfoTextEdit;
            this.ItemForUniqueProductInfo.Location = new System.Drawing.Point(158, 0);
            this.ItemForUniqueProductInfo.Name = "ItemForUniqueProductInfo";
            this.ItemForUniqueProductInfo.Padding = new DevExpress.XtraLayout.Utils.Padding(10, 2, 2, 2);
            this.ItemForUniqueProductInfo.Size = new System.Drawing.Size(286, 24);
            this.ItemForUniqueProductInfo.Text = "Giá trị";
            this.ItemForUniqueProductInfo.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.ItemForUniqueProductInfo.TextSize = new System.Drawing.Size(28, 13);
            this.ItemForUniqueProductInfo.TextToControlDistance = 5;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlItem4.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlItem4.Control = this.ThemVaoHyperlinkLabelControl;
            this.layoutControlItem4.Location = new System.Drawing.Point(444, 0);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Padding = new DevExpress.XtraLayout.Utils.Padding(10, 10, 2, 2);
            this.layoutControlItem4.Size = new System.Drawing.Size(88, 24);
            this.layoutControlItem4.TextVisible = false;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlItem3.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlItem3.Control = this.BoRaHyperlinkLabelControl;
            this.layoutControlItem3.Location = new System.Drawing.Point(532, 0);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Padding = new DevExpress.XtraLayout.Utils.Padding(10, 10, 2, 2);
            this.layoutControlItem3.Size = new System.Drawing.Size(66, 24);
            this.layoutControlItem3.TextVisible = false;
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
            // nhapThietBiMuonDetailDtoBindingSource
            // 
            this.nhapThietBiMuonDetailDtoBindingSource.DataSource = typeof(DTO.Inventory.StockIn.NhapThietBiMuon.NhapNoiBoDetailDto);
            // 
            // colFullInfo
            // 
            this.colFullInfo.FieldName = "FullInfo";
            this.colFullInfo.Name = "colFullInfo";
            this.colFullInfo.Visible = true;
            this.colFullInfo.VisibleIndex = 1;
            // 
            // FrmNhapSerialMacEmei
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
            this.Name = "FrmNhapSerialMacEmei";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NHẬP ĐỊNH DANH TÀI SẢN";
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).EndInit();
            this.dataLayoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl2)).EndInit();
            this.dataLayoutControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.WarrantyDtoGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deviceDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WarrantyDtoGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UniqueProductInfoTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StockInOutDetailIdSearchLookUpEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stockInOutProductHistoryDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StockInDetailSearchLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemHypertextLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DeviceIdentifierComboBoxEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForStockInOutDetailId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForUniqueProductInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nhapThietBiMuonDetailDtoBindingSource)).EndInit();
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
        private TextEdit UniqueProductInfoTextEdit;
        private LayoutControlGroup layoutControlGroup2;
        private LayoutControlItem ItemForUniqueProductInfo;
        private DevExpress.XtraGrid.GridControl WarrantyDtoGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView WarrantyDtoGridView;
        private SearchLookUpEdit StockInOutDetailIdSearchLookUpEdit;
        private DevExpress.XtraGrid.Views.Grid.GridView StockInDetailSearchLookUpEdit1View;
        private LayoutControlItem ItemForStockInOutDetailId;
        private LayoutControlItem layoutControlItem2;
        private HyperlinkLabelControl ThemVaoHyperlinkLabelControl;
        private HyperlinkLabelControl BoRaHyperlinkLabelControl;
        private LayoutControlItem layoutControlItem3;
        private LayoutControlItem layoutControlItem4;
        private BarStaticItem FormHotKeyBarStaticItem;
        private DevExpress.XtraGrid.Columns.GridColumn colFullNameHtml;
        private DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel repositoryItemHypertextLabel1;
        private ComboBoxEdit DeviceIdentifierComboBoxEdit;
        private LayoutControlGroup layoutControlGroup3;
        private LayoutControlItem layoutControlItem5;
        private System.Windows.Forms.BindingSource nhapThietBiMuonDetailDtoBindingSource;
        private System.Windows.Forms.BindingSource stockInOutProductHistoryDtoBindingSource;
        
        private System.Windows.Forms.BindingSource deviceDtoBindingSource;
        private DevExpress.XtraGrid.Columns.GridColumn colFullInfo;
    }
}