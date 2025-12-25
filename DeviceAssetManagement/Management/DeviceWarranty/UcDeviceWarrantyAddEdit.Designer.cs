using DevExpress.XtraBars;
using DevExpress.XtraDataLayout;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraLayout;
using DevExpress.XtraTreeList.Columns;
using DTO.DeviceAssetManagement;

namespace DeviceAssetManagement.Management.DeviceWarranty
{
    partial class UcDeviceWarrantyAddEdit
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
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.dxErrorProvider1 = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(this.components);
            this.dataLayoutControl1 = new DevExpress.XtraDataLayout.DataLayoutControl();
            this.dataLayoutControl2 = new DevExpress.XtraDataLayout.DataLayoutControl();
            this.WarrantyDtoGridControl = new DevExpress.XtraGrid.GridControl();
            this.warrantyDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.WarrantyDtoGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colWarrantyType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.LoaiBaoHanhEnumComboBox = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.colWarrantyFrom = new DevExpress.XtraGrid.Columns.GridColumn();
            this.NgayThangBaoHanhDateEdit = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.colWarrantyUntil = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMonthOfWarranty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemHypertextLabel1 = new DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
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
            ((System.ComponentModel.ISupportInitialize)(this.LoaiBaoHanhEnumComboBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NgayThangBaoHanhDateEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NgayThangBaoHanhDateEdit.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemHypertextLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
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
            this.CloseBarButtonItem});
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
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.CloseBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar2.OptionsBar.MultiLine = true;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Main menu";
            // 
            // SaveBarButtonItem
            // 
            this.SaveBarButtonItem.Caption = "Lưu";
            this.SaveBarButtonItem.Id = 0;
            this.SaveBarButtonItem.ImageOptions.Image = global::DeviceAssetManagement.Properties.Resources.save_16x16;
            this.SaveBarButtonItem.ImageOptions.LargeImage = global::DeviceAssetManagement.Properties.Resources.save_32x32;
            this.SaveBarButtonItem.Name = "SaveBarButtonItem";
            // 
            // CloseBarButtonItem
            // 
            this.CloseBarButtonItem.Caption = "Đóng";
            this.CloseBarButtonItem.Id = 1;
            this.CloseBarButtonItem.ImageOptions.Image = global::DeviceAssetManagement.Properties.Resources.cancel_16x16;
            this.CloseBarButtonItem.ImageOptions.LargeImage = global::DeviceAssetManagement.Properties.Resources.cancel_32x32;
            this.CloseBarButtonItem.Name = "CloseBarButtonItem";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager1;
            this.barDockControlTop.Size = new System.Drawing.Size(666, 39);
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
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 39);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 621);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(666, 39);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 621);
            // 
            // dxErrorProvider1
            // 
            this.dxErrorProvider1.ContainerControl = this;
            // 
            // dataLayoutControl1
            // 
            this.dataLayoutControl1.Controls.Add(this.dataLayoutControl2);
            this.dataLayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataLayoutControl1.Location = new System.Drawing.Point(0, 39);
            this.dataLayoutControl1.Name = "dataLayoutControl1";
            this.dataLayoutControl1.Root = this.Root;
            this.dataLayoutControl1.Size = new System.Drawing.Size(666, 621);
            this.dataLayoutControl1.TabIndex = 10;
            this.dataLayoutControl1.Text = "dataLayoutControl1";
            // 
            // dataLayoutControl2
            // 
            this.dataLayoutControl2.Controls.Add(this.WarrantyDtoGridControl);
            this.dataLayoutControl2.Location = new System.Drawing.Point(16, 16);
            this.dataLayoutControl2.Name = "dataLayoutControl2";
            this.dataLayoutControl2.Root = this.layoutControlGroup1;
            this.dataLayoutControl2.Size = new System.Drawing.Size(634, 589);
            this.dataLayoutControl2.TabIndex = 0;
            this.dataLayoutControl2.Text = "dataLayoutControl2";
            // 
            // WarrantyDtoGridControl
            // 
            this.WarrantyDtoGridControl.DataSource = this.warrantyDtoBindingSource;
            this.WarrantyDtoGridControl.Location = new System.Drawing.Point(16, 16);
            this.WarrantyDtoGridControl.MainView = this.WarrantyDtoGridView;
            this.WarrantyDtoGridControl.MenuManager = this.barManager1;
            this.WarrantyDtoGridControl.Name = "WarrantyDtoGridControl";
            this.WarrantyDtoGridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemHypertextLabel1,
            this.LoaiBaoHanhEnumComboBox,
            this.NgayThangBaoHanhDateEdit});
            this.WarrantyDtoGridControl.Size = new System.Drawing.Size(602, 557);
            this.WarrantyDtoGridControl.TabIndex = 10;
            this.WarrantyDtoGridControl.UseEmbeddedNavigator = true;
            this.WarrantyDtoGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.WarrantyDtoGridView});
            // 
            // warrantyDtoBindingSource
            // 
            this.warrantyDtoBindingSource.DataSource = typeof(DTO.DeviceAssetManagement.WarrantyDto);
            // 
            // WarrantyDtoGridView
            // 
            this.WarrantyDtoGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colWarrantyType,
            this.colWarrantyFrom,
            this.colWarrantyUntil,
            this.colMonthOfWarranty});
            this.WarrantyDtoGridView.GridControl = this.WarrantyDtoGridControl;
            this.WarrantyDtoGridView.Name = "WarrantyDtoGridView";
            this.WarrantyDtoGridView.OptionsNavigation.AutoFocusNewRow = true;
            this.WarrantyDtoGridView.OptionsSelection.MultiSelect = true;
            this.WarrantyDtoGridView.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            this.WarrantyDtoGridView.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            this.WarrantyDtoGridView.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom;
            this.WarrantyDtoGridView.OptionsView.RowAutoHeight = true;
            this.WarrantyDtoGridView.OptionsView.ShowGroupPanel = false;
            this.WarrantyDtoGridView.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.True;
            this.WarrantyDtoGridView.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.False;
            this.WarrantyDtoGridView.OptionsView.ShowViewCaption = true;
            this.WarrantyDtoGridView.ViewCaption = "THÔNG TIN BẢO HÀNH";
            // 
            // colWarrantyType
            // 
            this.colWarrantyType.AppearanceCell.Options.UseTextOptions = true;
            this.colWarrantyType.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colWarrantyType.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colWarrantyType.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colWarrantyType.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colWarrantyType.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colWarrantyType.AppearanceHeader.Options.UseBackColor = true;
            this.colWarrantyType.AppearanceHeader.Options.UseFont = true;
            this.colWarrantyType.AppearanceHeader.Options.UseForeColor = true;
            this.colWarrantyType.AppearanceHeader.Options.UseTextOptions = true;
            this.colWarrantyType.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colWarrantyType.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colWarrantyType.Caption = "Kiểu bảo hành";
            this.colWarrantyType.ColumnEdit = this.LoaiBaoHanhEnumComboBox;
            this.colWarrantyType.FieldName = "WarrantyType";
            this.colWarrantyType.Name = "colWarrantyType";
            this.colWarrantyType.Visible = true;
            this.colWarrantyType.VisibleIndex = 1;
            this.colWarrantyType.Width = 150;
            // 
            // LoaiBaoHanhEnumComboBox
            // 
            this.LoaiBaoHanhEnumComboBox.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.LoaiBaoHanhEnumComboBox.AutoHeight = false;
            this.LoaiBaoHanhEnumComboBox.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.LoaiBaoHanhEnumComboBox.Name = "LoaiBaoHanhEnumComboBox";
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
            this.colWarrantyFrom.Caption = "Ngày bắt đầu";
            this.colWarrantyFrom.ColumnEdit = this.NgayThangBaoHanhDateEdit;
            this.colWarrantyFrom.FieldName = "WarrantyFrom";
            this.colWarrantyFrom.Name = "colWarrantyFrom";
            this.colWarrantyFrom.Visible = true;
            this.colWarrantyFrom.VisibleIndex = 2;
            this.colWarrantyFrom.Width = 120;
            // 
            // NgayThangBaoHanhDateEdit
            // 
            this.NgayThangBaoHanhDateEdit.AutoHeight = false;
            this.NgayThangBaoHanhDateEdit.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.NgayThangBaoHanhDateEdit.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.NgayThangBaoHanhDateEdit.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.NgayThangBaoHanhDateEdit.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.NgayThangBaoHanhDateEdit.EditFormat.FormatString = "dd/MM/yyyy";
            this.NgayThangBaoHanhDateEdit.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.NgayThangBaoHanhDateEdit.MaskSettings.Set("mask", "dd/MM/yyyy");
            this.NgayThangBaoHanhDateEdit.Name = "NgayThangBaoHanhDateEdit";
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
            this.colWarrantyUntil.Caption = "Ngày hết hạn";
            this.colWarrantyUntil.ColumnEdit = this.NgayThangBaoHanhDateEdit;
            this.colWarrantyUntil.FieldName = "WarrantyUntil";
            this.colWarrantyUntil.Name = "colWarrantyUntil";
            this.colWarrantyUntil.Visible = true;
            this.colWarrantyUntil.VisibleIndex = 4;
            this.colWarrantyUntil.Width = 120;
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
            this.colMonthOfWarranty.Caption = "Tháng BH";
            this.colMonthOfWarranty.FieldName = "MonthOfWarranty";
            this.colMonthOfWarranty.Name = "colMonthOfWarranty";
            this.colMonthOfWarranty.Visible = true;
            this.colMonthOfWarranty.VisibleIndex = 3;
            this.colMonthOfWarranty.Width = 100;
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
            this.layoutControlGroup1.Size = new System.Drawing.Size(634, 589);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlGroup2
            // 
            this.layoutControlGroup2.AllowDrawBackground = false;
            this.layoutControlGroup2.GroupBordersVisible = false;
            this.layoutControlGroup2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem2});
            this.layoutControlGroup2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup2.Name = "autoGeneratedGroup0";
            this.layoutControlGroup2.Size = new System.Drawing.Size(608, 563);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.WarrantyDtoGridControl;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(608, 563);
            this.layoutControlItem2.TextVisible = false;
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(666, 621);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.dataLayoutControl2;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(640, 595);
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
            // UcDeviceWarrantyAddEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataLayoutControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "UcDeviceWarrantyAddEdit";
            this.Size = new System.Drawing.Size(666, 660);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).EndInit();
            this.dataLayoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl2)).EndInit();
            this.dataLayoutControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.WarrantyDtoGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.warrantyDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WarrantyDtoGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LoaiBaoHanhEnumComboBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NgayThangBaoHanhDateEdit.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NgayThangBaoHanhDateEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemHypertextLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
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
        private LayoutControlGroup layoutControlGroup2;
        private DevExpress.XtraGrid.GridControl WarrantyDtoGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView WarrantyDtoGridView;
        private LayoutControlItem layoutControlItem2;
        private System.Windows.Forms.BindingSource warrantyDtoBindingSource;
        private DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel repositoryItemHypertextLabel1;
        private DevExpress.XtraGrid.Columns.GridColumn colWarrantyType;
        private DevExpress.XtraGrid.Columns.GridColumn colWarrantyFrom;
        private DevExpress.XtraGrid.Columns.GridColumn colWarrantyUntil;
        private DevExpress.XtraGrid.Columns.GridColumn colMonthOfWarranty;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox LoaiBaoHanhEnumComboBox;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit NgayThangBaoHanhDateEdit;
    }
}
