using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraDataLayout;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout;
using DevExpress.XtraTreeList.Columns;

namespace Inventory.OverlayForm
{
    partial class FrmDeviceAddEdit
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
            DevExpress.XtraLayout.ColumnDefinition columnDefinition4 = new DevExpress.XtraLayout.ColumnDefinition();
            DevExpress.XtraLayout.RowDefinition rowDefinition1 = new DevExpress.XtraLayout.RowDefinition();
            DevExpress.XtraLayout.RowDefinition rowDefinition2 = new DevExpress.XtraLayout.RowDefinition();
            DevExpress.XtraLayout.RowDefinition rowDefinition3 = new DevExpress.XtraLayout.RowDefinition();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.SaveBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.CloseBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.dxErrorProvider1 = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(this.components);
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
            this.dataLayoutControl1 = new DevExpress.XtraDataLayout.DataLayoutControl();
            this.StatusComboBoxEdit = new DevExpress.XtraEditors.ComboBoxEdit();
            this.ProductVariantSearchLookUpEdit = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.productVariantListDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.searchLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colVariantFullName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ProductVariantHypertextLabel = new DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel();
            this.DeviceIdentifierComboBoxEdit = new DevExpress.XtraEditors.ComboBoxEdit();
            this.DeviceIdentifierTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.simpleLabelItem1 = new DevExpress.XtraLayout.SimpleLabelItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.simpleLabelItem2 = new DevExpress.XtraLayout.SimpleLabelItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.simpleLabelItem3 = new DevExpress.XtraLayout.SimpleLabelItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.NewDeviceBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).BeginInit();
            this.dataLayoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.StatusComboBoxEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductVariantSearchLookUpEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.productVariantListDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductVariantHypertextLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DeviceIdentifierComboBoxEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DeviceIdentifierTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
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
            this.NewDeviceBarButtonItem});
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
            this.bar2.FloatLocation = new System.Drawing.Point(4345, 346);
            this.bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.NewDeviceBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.SaveBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.CloseBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
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
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager1;
            this.barDockControlTop.Size = new System.Drawing.Size(993, 24);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 633);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(993, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 24);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 609);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(993, 24);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 609);
            // 
            // dxErrorProvider1
            // 
            this.dxErrorProvider1.ContainerControl = this;
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
            // dataLayoutControl1
            // 
            this.dataLayoutControl1.Controls.Add(this.gridControl1);
            this.dataLayoutControl1.Controls.Add(this.StatusComboBoxEdit);
            this.dataLayoutControl1.Controls.Add(this.ProductVariantSearchLookUpEdit);
            this.dataLayoutControl1.Controls.Add(this.DeviceIdentifierComboBoxEdit);
            this.dataLayoutControl1.Controls.Add(this.DeviceIdentifierTextEdit);
            this.dataLayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataLayoutControl1.Location = new System.Drawing.Point(0, 24);
            this.dataLayoutControl1.Name = "dataLayoutControl1";
            this.dataLayoutControl1.Root = this.Root;
            this.dataLayoutControl1.Size = new System.Drawing.Size(993, 609);
            this.dataLayoutControl1.TabIndex = 15;
            this.dataLayoutControl1.Text = "dataLayoutControl1";
            // 
            // StatusComboBoxEdit
            // 
            this.StatusComboBoxEdit.AllowHtmlTextInToolTip = DevExpress.Utils.DefaultBoolean.True;
            this.StatusComboBoxEdit.Location = new System.Drawing.Point(795, 12);
            this.StatusComboBoxEdit.MenuManager = this.barManager1;
            this.StatusComboBoxEdit.Name = "StatusComboBoxEdit";
            this.StatusComboBoxEdit.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.StatusComboBoxEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.StatusComboBoxEdit.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.StatusComboBoxEdit.Size = new System.Drawing.Size(186, 20);
            this.StatusComboBoxEdit.StyleController = this.dataLayoutControl1;
            this.StatusComboBoxEdit.TabIndex = 2;
            // 
            // ProductVariantSearchLookUpEdit
            // 
            this.ProductVariantSearchLookUpEdit.Location = new System.Drawing.Point(255, 12);
            this.ProductVariantSearchLookUpEdit.MenuManager = this.barManager1;
            this.ProductVariantSearchLookUpEdit.Name = "ProductVariantSearchLookUpEdit";
            this.ProductVariantSearchLookUpEdit.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.ProductVariantSearchLookUpEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ProductVariantSearchLookUpEdit.Properties.DataSource = this.productVariantListDtoBindingSource;
            this.ProductVariantSearchLookUpEdit.Properties.DisplayMember = "VariantFullName";
            this.ProductVariantSearchLookUpEdit.Properties.PopupView = this.searchLookUpEdit1View;
            this.ProductVariantSearchLookUpEdit.Properties.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.ProductVariantHypertextLabel});
            this.ProductVariantSearchLookUpEdit.Properties.ValueMember = "Id";
            this.ProductVariantSearchLookUpEdit.Size = new System.Drawing.Size(482, 20);
            this.ProductVariantSearchLookUpEdit.StyleController = this.dataLayoutControl1;
            this.ProductVariantSearchLookUpEdit.TabIndex = 0;
            // 
            // productVariantListDtoBindingSource
            // 
            this.productVariantListDtoBindingSource.DataSource = typeof(DTO.MasterData.ProductService.ProductVariantListDto);
            // 
            // searchLookUpEdit1View
            // 
            this.searchLookUpEdit1View.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colVariantFullName});
            this.searchLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.searchLookUpEdit1View.Name = "searchLookUpEdit1View";
            this.searchLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.searchLookUpEdit1View.OptionsView.RowAutoHeight = true;
            this.searchLookUpEdit1View.OptionsView.ShowAutoFilterRow = true;
            this.searchLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            this.searchLookUpEdit1View.OptionsView.ShowIndicator = false;
            // 
            // colVariantFullName
            // 
            this.colVariantFullName.AppearanceCell.Options.UseTextOptions = true;
            this.colVariantFullName.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colVariantFullName.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colVariantFullName.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colVariantFullName.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colVariantFullName.AppearanceHeader.Options.UseBackColor = true;
            this.colVariantFullName.AppearanceHeader.Options.UseFont = true;
            this.colVariantFullName.AppearanceHeader.Options.UseForeColor = true;
            this.colVariantFullName.AppearanceHeader.Options.UseTextOptions = true;
            this.colVariantFullName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colVariantFullName.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colVariantFullName.Caption = "Sản phẩm - Dịch vụ - Hàng hóa";
            this.colVariantFullName.ColumnEdit = this.ProductVariantHypertextLabel;
            this.colVariantFullName.FieldName = "VariantFullName";
            this.colVariantFullName.Name = "colVariantFullName";
            this.colVariantFullName.Visible = true;
            this.colVariantFullName.VisibleIndex = 0;
            // 
            // ProductVariantHypertextLabel
            // 
            this.ProductVariantHypertextLabel.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.ProductVariantHypertextLabel.Name = "ProductVariantHypertextLabel";
            // 
            // DeviceIdentifierComboBoxEdit
            // 
            this.DeviceIdentifierComboBoxEdit.Location = new System.Drawing.Point(255, 36);
            this.DeviceIdentifierComboBoxEdit.MenuManager = this.barManager1;
            this.DeviceIdentifierComboBoxEdit.Name = "DeviceIdentifierComboBoxEdit";
            this.DeviceIdentifierComboBoxEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.DeviceIdentifierComboBoxEdit.Size = new System.Drawing.Size(239, 20);
            this.DeviceIdentifierComboBoxEdit.StyleController = this.dataLayoutControl1;
            this.DeviceIdentifierComboBoxEdit.TabIndex = 3;
            // 
            // DeviceIdentifierTextEdit
            // 
            this.DeviceIdentifierTextEdit.Location = new System.Drawing.Point(741, 36);
            this.DeviceIdentifierTextEdit.MenuManager = this.barManager1;
            this.DeviceIdentifierTextEdit.Name = "DeviceIdentifierTextEdit";
            this.DeviceIdentifierTextEdit.Size = new System.Drawing.Size(240, 20);
            this.DeviceIdentifierTextEdit.StyleController = this.dataLayoutControl1;
            this.DeviceIdentifierTextEdit.TabIndex = 4;
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.simpleLabelItem1,
            this.layoutControlItem1,
            this.simpleLabelItem2,
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.simpleLabelItem3,
            this.layoutControlItem6,
            this.layoutControlItem4});
            this.Root.LayoutMode = DevExpress.XtraLayout.Utils.LayoutMode.Table;
            this.Root.Name = "Root";
            columnDefinition1.SizeType = System.Windows.Forms.SizeType.Percent;
            columnDefinition1.Width = 25D;
            columnDefinition2.SizeType = System.Windows.Forms.SizeType.Percent;
            columnDefinition2.Width = 25D;
            columnDefinition3.SizeType = System.Windows.Forms.SizeType.Percent;
            columnDefinition3.Width = 25D;
            columnDefinition4.SizeType = System.Windows.Forms.SizeType.Percent;
            columnDefinition4.Width = 25D;
            this.Root.OptionsTableLayoutGroup.ColumnDefinitions.AddRange(new DevExpress.XtraLayout.ColumnDefinition[] {
            columnDefinition1,
            columnDefinition2,
            columnDefinition3,
            columnDefinition4});
            rowDefinition1.Height = 24D;
            rowDefinition1.SizeType = System.Windows.Forms.SizeType.AutoSize;
            rowDefinition2.Height = 24D;
            rowDefinition2.SizeType = System.Windows.Forms.SizeType.AutoSize;
            rowDefinition3.Height = 541D;
            rowDefinition3.SizeType = System.Windows.Forms.SizeType.AutoSize;
            this.Root.OptionsTableLayoutGroup.RowDefinitions.AddRange(new DevExpress.XtraLayout.RowDefinition[] {
            rowDefinition1,
            rowDefinition2,
            rowDefinition3});
            this.Root.Size = new System.Drawing.Size(993, 609);
            this.Root.TextVisible = false;
            // 
            // simpleLabelItem1
            // 
            this.simpleLabelItem1.Location = new System.Drawing.Point(0, 0);
            this.simpleLabelItem1.Name = "simpleLabelItem1";
            this.simpleLabelItem1.Size = new System.Drawing.Size(243, 24);
            this.simpleLabelItem1.Text = "Sản phẩm - Dịch vụ - Hàng hóa";
            this.simpleLabelItem1.TextSize = new System.Drawing.Size(148, 13);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.ProductVariantSearchLookUpEdit;
            this.layoutControlItem1.Location = new System.Drawing.Point(243, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.OptionsTableLayoutItem.ColumnIndex = 1;
            this.layoutControlItem1.OptionsTableLayoutItem.ColumnSpan = 2;
            this.layoutControlItem1.Size = new System.Drawing.Size(486, 24);
            this.layoutControlItem1.TextVisible = false;
            // 
            // simpleLabelItem2
            // 
            this.simpleLabelItem2.Location = new System.Drawing.Point(0, 24);
            this.simpleLabelItem2.Name = "simpleLabelItem2";
            this.simpleLabelItem2.OptionsTableLayoutItem.RowIndex = 1;
            this.simpleLabelItem2.Size = new System.Drawing.Size(243, 24);
            this.simpleLabelItem2.Text = "Kiểu định danh";
            this.simpleLabelItem2.TextSize = new System.Drawing.Size(148, 13);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.DeviceIdentifierComboBoxEdit;
            this.layoutControlItem2.Location = new System.Drawing.Point(243, 24);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.OptionsTableLayoutItem.ColumnIndex = 1;
            this.layoutControlItem2.OptionsTableLayoutItem.RowIndex = 1;
            this.layoutControlItem2.Size = new System.Drawing.Size(243, 24);
            this.layoutControlItem2.TextVisible = false;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.DeviceIdentifierTextEdit;
            this.layoutControlItem3.Location = new System.Drawing.Point(729, 24);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.OptionsTableLayoutItem.ColumnIndex = 3;
            this.layoutControlItem3.OptionsTableLayoutItem.RowIndex = 1;
            this.layoutControlItem3.Size = new System.Drawing.Size(244, 24);
            this.layoutControlItem3.TextVisible = false;
            // 
            // simpleLabelItem3
            // 
            this.simpleLabelItem3.Location = new System.Drawing.Point(486, 24);
            this.simpleLabelItem3.Name = "simpleLabelItem3";
            this.simpleLabelItem3.OptionsTableLayoutItem.ColumnIndex = 2;
            this.simpleLabelItem3.OptionsTableLayoutItem.RowIndex = 1;
            this.simpleLabelItem3.Size = new System.Drawing.Size(243, 24);
            this.simpleLabelItem3.Text = "Giá trị";
            this.simpleLabelItem3.TextSize = new System.Drawing.Size(148, 13);
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.StatusComboBoxEdit;
            this.layoutControlItem6.Location = new System.Drawing.Point(729, 0);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.OptionsTableLayoutItem.ColumnIndex = 3;
            this.layoutControlItem6.Size = new System.Drawing.Size(244, 24);
            this.layoutControlItem6.Text = "Tình trạng";
            this.layoutControlItem6.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem6.TextSize = new System.Drawing.Size(49, 13);
            this.layoutControlItem6.TextToControlDistance = 5;
            // 
            // NewDeviceBarButtonItem
            // 
            this.NewDeviceBarButtonItem.Caption = "Thiết bị mới";
            this.NewDeviceBarButtonItem.Id = 3;
            this.NewDeviceBarButtonItem.ImageOptions.Image = global::Inventory.Properties.Resources.add_16x16;
            this.NewDeviceBarButtonItem.Name = "NewDeviceBarButtonItem";
            // 
            // gridControl1
            // 
            this.gridControl1.Location = new System.Drawing.Point(12, 60);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.MenuManager = this.barManager1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(969, 537);
            this.gridControl1.TabIndex = 5;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.gridControl1;
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 48);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.OptionsTableLayoutItem.ColumnSpan = 4;
            this.layoutControlItem4.OptionsTableLayoutItem.RowIndex = 2;
            this.layoutControlItem4.Size = new System.Drawing.Size(973, 541);
            this.layoutControlItem4.TextVisible = false;
            // 
            // FrmDeviceAddEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(993, 633);
            this.Controls.Add(this.dataLayoutControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmDeviceAddEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Thêm định danh thiết bị";
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).EndInit();
            this.dataLayoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.StatusComboBoxEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductVariantSearchLookUpEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.productVariantListDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductVariantHypertextLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DeviceIdentifierComboBoxEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DeviceIdentifierTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
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
        private DataLayoutControl dataLayoutControl1;
        private LayoutControlGroup Root;
        private SearchLookUpEdit ProductVariantSearchLookUpEdit;
        private GridView searchLookUpEdit1View;
        private SimpleLabelItem simpleLabelItem1;
        private LayoutControlItem layoutControlItem1;
        private RepositoryItemHypertextLabel ProductVariantHypertextLabel;
        private BindingSource productVariantListDtoBindingSource;
        private GridColumn colVariantFullName;
        private ComboBoxEdit DeviceIdentifierComboBoxEdit;
        private SimpleLabelItem simpleLabelItem2;
        private LayoutControlItem layoutControlItem2;
        private TextEdit DeviceIdentifierTextEdit;
        private LayoutControlItem layoutControlItem3;
        private SimpleLabelItem simpleLabelItem3;
        private ComboBoxEdit StatusComboBoxEdit;
        private LayoutControlItem layoutControlItem6;
        private BarButtonItem NewDeviceBarButtonItem;
        private GridControl gridControl1;
        private GridView gridView1;
        private LayoutControlItem layoutControlItem4;
    }
}