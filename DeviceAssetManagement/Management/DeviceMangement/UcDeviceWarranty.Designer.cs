using DevExpress.XtraBars;
using DevExpress.XtraDataLayout;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraLayout;
using DevExpress.XtraTreeList.Columns;
using DTO.DeviceAssetManagement;

namespace DeviceAssetManagement.Management.DeviceMangement
{
    partial class UcDeviceWarranty
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
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.dxErrorProvider1 = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(this.components);
            this.dataLayoutControl1 = new DevExpress.XtraDataLayout.DataLayoutControl();
            this.dataLayoutControl2 = new DevExpress.XtraDataLayout.DataLayoutControl();
            this.LoaiBaoHanhComboBoxEdit = new DevExpress.XtraEditors.ComboBoxEdit();
            this.ThemVaoHyperlinkLabelControl = new DevExpress.XtraEditors.HyperlinkLabelControl();
            this.BoRaHyperlinkLabelControl = new DevExpress.XtraEditors.HyperlinkLabelControl();
            this.WarrantyDtoGridControl = new DevExpress.XtraGrid.GridControl();
            this.warrantyDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.WarrantyDtoGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colWarrantyInfoHtml = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemHypertextLabel1 = new DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel();
            this.WarrantyFromDateEdit = new DevExpress.XtraEditors.DateEdit();
            this.MonthOfWarrantyTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.WarrantyUntilDateEdit = new DevExpress.XtraEditors.DateEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlGroup3 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.ItemForWarrantyFrom = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForMonthOfWarranty = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForWarrantyUntil = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
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
            ((System.ComponentModel.ISupportInitialize)(this.LoaiBaoHanhComboBoxEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WarrantyDtoGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.warrantyDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WarrantyDtoGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemHypertextLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WarrantyFromDateEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WarrantyFromDateEdit.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MonthOfWarrantyTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WarrantyUntilDateEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WarrantyUntilDateEdit.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForWarrantyFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForMonthOfWarranty)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForWarrantyUntil)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
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
            this.SaveBarButtonItem.Name = "SaveBarButtonItem";
            // 
            // CloseBarButtonItem
            // 
            this.CloseBarButtonItem.Caption = "Đóng";
            this.CloseBarButtonItem.Id = 1;
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
            this.dataLayoutControl2.Controls.Add(this.LoaiBaoHanhComboBoxEdit);
            this.dataLayoutControl2.Controls.Add(this.ThemVaoHyperlinkLabelControl);
            this.dataLayoutControl2.Controls.Add(this.BoRaHyperlinkLabelControl);
            this.dataLayoutControl2.Controls.Add(this.WarrantyDtoGridControl);
            this.dataLayoutControl2.Controls.Add(this.WarrantyFromDateEdit);
            this.dataLayoutControl2.Controls.Add(this.MonthOfWarrantyTextEdit);
            this.dataLayoutControl2.Controls.Add(this.WarrantyUntilDateEdit);
            this.dataLayoutControl2.Location = new System.Drawing.Point(16, 16);
            this.dataLayoutControl2.Name = "dataLayoutControl2";
            this.dataLayoutControl2.Root = this.layoutControlGroup1;
            this.dataLayoutControl2.Size = new System.Drawing.Size(634, 589);
            this.dataLayoutControl2.TabIndex = 0;
            this.dataLayoutControl2.Text = "dataLayoutControl2";
            // 
            // LoaiBaoHanhComboBoxEdit
            // 
            this.LoaiBaoHanhComboBoxEdit.AllowHtmlTextInToolTip = DevExpress.Utils.DefaultBoolean.True;
            this.LoaiBaoHanhComboBoxEdit.Location = new System.Drawing.Point(99, 16);
            this.LoaiBaoHanhComboBoxEdit.MenuManager = this.barManager1;
            this.LoaiBaoHanhComboBoxEdit.Name = "LoaiBaoHanhComboBoxEdit";
            this.LoaiBaoHanhComboBoxEdit.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.LoaiBaoHanhComboBoxEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.LoaiBaoHanhComboBoxEdit.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.LoaiBaoHanhComboBoxEdit.Size = new System.Drawing.Size(519, 28);
            this.LoaiBaoHanhComboBoxEdit.StyleController = this.dataLayoutControl2;
            this.LoaiBaoHanhComboBoxEdit.TabIndex = 13;
            // 
            // ThemVaoHyperlinkLabelControl
            // 
            this.ThemVaoHyperlinkLabelControl.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.ThemVaoHyperlinkLabelControl.Location = new System.Drawing.Point(23, 142);
            this.ThemVaoHyperlinkLabelControl.Name = "ThemVaoHyperlinkLabelControl";
            this.ThemVaoHyperlinkLabelControl.Size = new System.Drawing.Size(68, 20);
            this.ThemVaoHyperlinkLabelControl.StyleController = this.dataLayoutControl2;
            this.ThemVaoHyperlinkLabelControl.TabIndex = 12;
            this.ThemVaoHyperlinkLabelControl.Text = "Thêm vào";
            // 
            // BoRaHyperlinkLabelControl
            // 
            this.BoRaHyperlinkLabelControl.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.BoRaHyperlinkLabelControl.Location = new System.Drawing.Point(111, 142);
            this.BoRaHyperlinkLabelControl.Name = "BoRaHyperlinkLabelControl";
            this.BoRaHyperlinkLabelControl.Size = new System.Drawing.Size(46, 20);
            this.BoRaHyperlinkLabelControl.StyleController = this.dataLayoutControl2;
            this.BoRaHyperlinkLabelControl.TabIndex = 11;
            this.BoRaHyperlinkLabelControl.Text = "Bỏ ra";
            // 
            // WarrantyDtoGridControl
            // 
            this.WarrantyDtoGridControl.DataSource = this.warrantyDtoBindingSource;
            this.WarrantyDtoGridControl.Location = new System.Drawing.Point(16, 175);
            this.WarrantyDtoGridControl.MainView = this.WarrantyDtoGridView;
            this.WarrantyDtoGridControl.MenuManager = this.barManager1;
            this.WarrantyDtoGridControl.Name = "WarrantyDtoGridControl";
            this.WarrantyDtoGridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemHypertextLabel1});
            this.WarrantyDtoGridControl.Size = new System.Drawing.Size(602, 398);
            this.WarrantyDtoGridControl.TabIndex = 10;
            this.WarrantyDtoGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.WarrantyDtoGridView});
            // 
            // warrantyDtoBindingSource
            // 
            this.warrantyDtoBindingSource.DataSource = typeof(WarrantyDto);
            // 
            // WarrantyDtoGridView
            // 
            this.WarrantyDtoGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colWarrantyInfoHtml});
            this.WarrantyDtoGridView.GridControl = this.WarrantyDtoGridControl;
            this.WarrantyDtoGridView.Name = "WarrantyDtoGridView";
            this.WarrantyDtoGridView.OptionsSelection.MultiSelect = true;
            this.WarrantyDtoGridView.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            this.WarrantyDtoGridView.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            this.WarrantyDtoGridView.OptionsView.RowAutoHeight = true;
            this.WarrantyDtoGridView.OptionsView.ShowGroupPanel = false;
            this.WarrantyDtoGridView.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.True;
            this.WarrantyDtoGridView.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.False;
            this.WarrantyDtoGridView.OptionsView.ShowViewCaption = true;
            this.WarrantyDtoGridView.ViewCaption = "THÔNG TIN BẢO HÀNH";
            // 
            // colWarrantyInfoHtml
            // 
            this.colWarrantyInfoHtml.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colWarrantyInfoHtml.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colWarrantyInfoHtml.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colWarrantyInfoHtml.AppearanceHeader.Options.UseBackColor = true;
            this.colWarrantyInfoHtml.AppearanceHeader.Options.UseFont = true;
            this.colWarrantyInfoHtml.AppearanceHeader.Options.UseForeColor = true;
            this.colWarrantyInfoHtml.AppearanceHeader.Options.UseTextOptions = true;
            this.colWarrantyInfoHtml.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colWarrantyInfoHtml.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colWarrantyInfoHtml.Caption = "Thông tin bảo hành";
            this.colWarrantyInfoHtml.ColumnEdit = this.repositoryItemHypertextLabel1;
            this.colWarrantyInfoHtml.FieldName = "WarrantyInfoHtml";
            this.colWarrantyInfoHtml.Name = "colWarrantyInfoHtml";
            this.colWarrantyInfoHtml.OptionsColumn.AllowEdit = false;
            this.colWarrantyInfoHtml.OptionsColumn.AllowFocus = false;
            this.colWarrantyInfoHtml.OptionsColumn.AllowMove = false;
            this.colWarrantyInfoHtml.OptionsColumn.ReadOnly = true;
            this.colWarrantyInfoHtml.Visible = true;
            this.colWarrantyInfoHtml.VisibleIndex = 1;
            this.colWarrantyInfoHtml.Width = 580;
            // 
            // repositoryItemHypertextLabel1
            // 
            this.repositoryItemHypertextLabel1.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.repositoryItemHypertextLabel1.Name = "repositoryItemHypertextLabel1";
            // 
            // WarrantyFromDateEdit
            // 
            this.WarrantyFromDateEdit.EditValue = null;
            this.WarrantyFromDateEdit.Location = new System.Drawing.Point(84, 86);
            this.WarrantyFromDateEdit.MenuManager = this.barManager1;
            this.WarrantyFromDateEdit.Name = "WarrantyFromDateEdit";
            this.WarrantyFromDateEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.WarrantyFromDateEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.WarrantyFromDateEdit.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.WarrantyFromDateEdit.Size = new System.Drawing.Size(143, 28);
            this.WarrantyFromDateEdit.StyleController = this.dataLayoutControl2;
            this.WarrantyFromDateEdit.TabIndex = 4;
            // 
            // MonthOfWarrantyTextEdit
            // 
            this.MonthOfWarrantyTextEdit.Location = new System.Drawing.Point(332, 86);
            this.MonthOfWarrantyTextEdit.MenuManager = this.barManager1;
            this.MonthOfWarrantyTextEdit.Name = "MonthOfWarrantyTextEdit";
            this.MonthOfWarrantyTextEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.MonthOfWarrantyTextEdit.Properties.Appearance.Options.UseTextOptions = true;
            this.MonthOfWarrantyTextEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.MonthOfWarrantyTextEdit.Properties.Mask.EditMask = "N0";
            this.MonthOfWarrantyTextEdit.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.MonthOfWarrantyTextEdit.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.MonthOfWarrantyTextEdit.Size = new System.Drawing.Size(45, 28);
            this.MonthOfWarrantyTextEdit.StyleController = this.dataLayoutControl2;
            this.MonthOfWarrantyTextEdit.TabIndex = 5;
            // 
            // WarrantyUntilDateEdit
            // 
            this.WarrantyUntilDateEdit.EditValue = null;
            this.WarrantyUntilDateEdit.Location = new System.Drawing.Point(441, 86);
            this.WarrantyUntilDateEdit.MenuManager = this.barManager1;
            this.WarrantyUntilDateEdit.Name = "WarrantyUntilDateEdit";
            this.WarrantyUntilDateEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.WarrantyUntilDateEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.WarrantyUntilDateEdit.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.WarrantyUntilDateEdit.Size = new System.Drawing.Size(162, 28);
            this.WarrantyUntilDateEdit.StyleController = this.dataLayoutControl2;
            this.WarrantyUntilDateEdit.TabIndex = 6;
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
            this.layoutControlItem2,
            this.layoutControlGroup3,
            this.layoutControlItem3,
            this.layoutControlItem4,
            this.layoutControlItem5,
            this.emptySpaceItem1});
            this.layoutControlGroup2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup2.Name = "autoGeneratedGroup0";
            this.layoutControlGroup2.Size = new System.Drawing.Size(608, 563);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.WarrantyDtoGridControl;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 159);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(608, 404);
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
            this.layoutControlGroup3.Location = new System.Drawing.Point(0, 34);
            this.layoutControlGroup3.Name = "layoutControlGroup3";
            columnDefinition1.SizeType = System.Windows.Forms.SizeType.Absolute;
            columnDefinition1.Width = 200D;
            columnDefinition2.SizeType = System.Windows.Forms.SizeType.Absolute;
            columnDefinition2.Width = 150D;
            columnDefinition3.SizeType = System.Windows.Forms.SizeType.AutoSize;
            columnDefinition3.Width = 226D;
            this.layoutControlGroup3.OptionsTableLayoutGroup.ColumnDefinitions.AddRange(new DevExpress.XtraLayout.ColumnDefinition[] {
            columnDefinition1,
            columnDefinition2,
            columnDefinition3});
            rowDefinition1.Height = 32D;
            rowDefinition1.SizeType = System.Windows.Forms.SizeType.AutoSize;
            this.layoutControlGroup3.OptionsTableLayoutGroup.RowDefinitions.AddRange(new DevExpress.XtraLayout.RowDefinition[] {
            rowDefinition1});
            this.layoutControlGroup3.Size = new System.Drawing.Size(608, 85);
            this.layoutControlGroup3.Text = "Thời gian bảo hành";
            // 
            // ItemForWarrantyFrom
            // 
            this.ItemForWarrantyFrom.Control = this.WarrantyFromDateEdit;
            this.ItemForWarrantyFrom.Location = new System.Drawing.Point(0, 0);
            this.ItemForWarrantyFrom.Name = "ItemForWarrantyFrom";
            this.ItemForWarrantyFrom.Padding = new DevExpress.XtraLayout.Utils.Padding(10, 2, 2, 2);
            this.ItemForWarrantyFrom.Size = new System.Drawing.Size(200, 32);
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
            this.ItemForMonthOfWarranty.Size = new System.Drawing.Size(150, 32);
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
            this.ItemForWarrantyUntil.Size = new System.Drawing.Size(226, 32);
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
            this.layoutControlItem3.Location = new System.Drawing.Point(88, 119);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Padding = new DevExpress.XtraLayout.Utils.Padding(10, 10, 10, 10);
            this.layoutControlItem3.Size = new System.Drawing.Size(66, 40);
            this.layoutControlItem3.TextVisible = false;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlItem4.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlItem4.Control = this.ThemVaoHyperlinkLabelControl;
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 119);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Padding = new DevExpress.XtraLayout.Utils.Padding(10, 10, 10, 10);
            this.layoutControlItem4.Size = new System.Drawing.Size(88, 40);
            this.layoutControlItem4.TextVisible = false;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.LoaiBaoHanhComboBoxEdit;
            this.layoutControlItem5.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(608, 34);
            this.layoutControlItem5.Text = "Loại bảo hành";
            this.layoutControlItem5.TextSize = new System.Drawing.Size(67, 13);
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.Location = new System.Drawing.Point(154, 119);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(454, 40);
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
            // UcDeviceWarranty
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataLayoutControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "UcDeviceWarranty";
            this.Size = new System.Drawing.Size(666, 660);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).EndInit();
            this.dataLayoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl2)).EndInit();
            this.dataLayoutControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LoaiBaoHanhComboBoxEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WarrantyDtoGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.warrantyDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WarrantyDtoGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemHypertextLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WarrantyFromDateEdit.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WarrantyFromDateEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MonthOfWarrantyTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WarrantyUntilDateEdit.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WarrantyUntilDateEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForWarrantyFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForMonthOfWarranty)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForWarrantyUntil)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
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
        private LayoutControlGroup layoutControlGroup2;
        private LayoutControlItem ItemForWarrantyFrom;
        private LayoutControlItem ItemForMonthOfWarranty;
        private LayoutControlItem ItemForWarrantyUntil;
        private DevExpress.XtraGrid.GridControl WarrantyDtoGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView WarrantyDtoGridView;
        private LayoutControlItem layoutControlItem2;
        private LayoutControlGroup layoutControlGroup3;
        private HyperlinkLabelControl ThemVaoHyperlinkLabelControl;
        private HyperlinkLabelControl BoRaHyperlinkLabelControl;
        private LayoutControlItem layoutControlItem3;
        private LayoutControlItem layoutControlItem4;
        private System.Windows.Forms.BindingSource warrantyDtoBindingSource;
        private ComboBoxEdit LoaiBaoHanhComboBoxEdit;
        private LayoutControlItem layoutControlItem5;
        private EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraGrid.Columns.GridColumn colWarrantyInfoHtml;
        private DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel repositoryItemHypertextLabel1;
    }
}
