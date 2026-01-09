using DevExpress.XtraBars;
using DevExpress.XtraDataLayout;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraLayout;

namespace Inventory.StockTakking
{
    partial class FrmFrmStocktakingMasterAddEdit
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
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.StocktakingDateDateEdit = new DevExpress.XtraEditors.DateEdit();
            this.ItemForStocktakingDate = new DevExpress.XtraLayout.LayoutControlItem();
            this.VoucherNumberTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.ItemForVoucherNumber = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForStocktakingType = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForStocktakingStatus = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForWarehouseName = new DevExpress.XtraLayout.LayoutControlItem();
            this.StartDateDateEdit = new DevExpress.XtraEditors.DateEdit();
            this.ItemForStartDate = new DevExpress.XtraLayout.LayoutControlItem();
            this.EndDateDateEdit = new DevExpress.XtraEditors.DateEdit();
            this.ItemForEndDate = new DevExpress.XtraLayout.LayoutControlItem();
            this.CountedByTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.ItemForCountedBy = new DevExpress.XtraLayout.LayoutControlItem();
            this.CountedDateDateEdit = new DevExpress.XtraEditors.DateEdit();
            this.ItemForCountedDate = new DevExpress.XtraLayout.LayoutControlItem();
            this.ReviewedByTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.ItemForReviewedBy = new DevExpress.XtraLayout.LayoutControlItem();
            this.ReviewedDateDateEdit = new DevExpress.XtraEditors.DateEdit();
            this.ItemForReviewedDate = new DevExpress.XtraLayout.LayoutControlItem();
            this.ApprovedByTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.ItemForApprovedBy = new DevExpress.XtraLayout.LayoutControlItem();
            this.ApprovedDateDateEdit = new DevExpress.XtraEditors.DateEdit();
            this.ItemForApprovedDate = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForNotes = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForReason = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForIsLocked = new DevExpress.XtraLayout.LayoutControlItem();
            this.LockedDateDateEdit = new DevExpress.XtraEditors.DateEdit();
            this.ItemForLockedDate = new DevExpress.XtraLayout.LayoutControlItem();
            this.LockedByTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.ItemForLockedBy = new DevExpress.XtraLayout.LayoutControlItem();
            this.StocktakingTypeTextEdit = new DevExpress.XtraEditors.ComboBoxEdit();
            this.StocktakingStatusTextEdit = new DevExpress.XtraEditors.CheckedComboBoxEdit();
            this.WarehouseNameTextEdit = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.searchLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup3 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup4 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.IsLockedCheckEdit = new DevExpress.XtraEditors.ToggleSwitch();
            this.NotesTextEdit = new DevExpress.XtraEditors.MemoEdit();
            this.ReasonTextEdit = new DevExpress.XtraEditors.MemoEdit();
            this.layoutControlGroup5 = new DevExpress.XtraLayout.LayoutControlGroup();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).BeginInit();
            this.dataLayoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StocktakingDateDateEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StocktakingDateDateEdit.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForStocktakingDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VoucherNumberTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForVoucherNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForStocktakingType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForStocktakingStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForWarehouseName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartDateDateEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartDateDateEdit.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForStartDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EndDateDateEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EndDateDateEdit.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForEndDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CountedByTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForCountedBy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CountedDateDateEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CountedDateDateEdit.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForCountedDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReviewedByTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForReviewedBy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReviewedDateDateEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReviewedDateDateEdit.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForReviewedDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ApprovedByTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForApprovedBy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ApprovedDateDateEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ApprovedDateDateEdit.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForApprovedDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForNotes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForReason)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForIsLocked)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LockedDateDateEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LockedDateDateEdit.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForLockedDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LockedByTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForLockedBy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StocktakingTypeTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StocktakingStatusTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WarehouseNameTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.IsLockedCheckEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NotesTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReasonTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup5)).BeginInit();
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
            this.barManager1.MaxItemId = 2;
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
            this.barDockControlTop.Size = new System.Drawing.Size(666, 24);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 742);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(666, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 24);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 718);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(666, 24);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 718);
            // 
            // dxErrorProvider1
            // 
            this.dxErrorProvider1.ContainerControl = this;
            // 
            // dataLayoutControl1
            // 
            this.dataLayoutControl1.Controls.Add(this.StocktakingDateDateEdit);
            this.dataLayoutControl1.Controls.Add(this.VoucherNumberTextEdit);
            this.dataLayoutControl1.Controls.Add(this.StartDateDateEdit);
            this.dataLayoutControl1.Controls.Add(this.EndDateDateEdit);
            this.dataLayoutControl1.Controls.Add(this.CountedByTextEdit);
            this.dataLayoutControl1.Controls.Add(this.CountedDateDateEdit);
            this.dataLayoutControl1.Controls.Add(this.ReviewedByTextEdit);
            this.dataLayoutControl1.Controls.Add(this.ReviewedDateDateEdit);
            this.dataLayoutControl1.Controls.Add(this.ApprovedByTextEdit);
            this.dataLayoutControl1.Controls.Add(this.ApprovedDateDateEdit);
            this.dataLayoutControl1.Controls.Add(this.LockedDateDateEdit);
            this.dataLayoutControl1.Controls.Add(this.LockedByTextEdit);
            this.dataLayoutControl1.Controls.Add(this.StocktakingTypeTextEdit);
            this.dataLayoutControl1.Controls.Add(this.StocktakingStatusTextEdit);
            this.dataLayoutControl1.Controls.Add(this.WarehouseNameTextEdit);
            this.dataLayoutControl1.Controls.Add(this.IsLockedCheckEdit);
            this.dataLayoutControl1.Controls.Add(this.NotesTextEdit);
            this.dataLayoutControl1.Controls.Add(this.ReasonTextEdit);
            this.dataLayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataLayoutControl1.Location = new System.Drawing.Point(0, 24);
            this.dataLayoutControl1.Name = "dataLayoutControl1";
            this.dataLayoutControl1.Root = this.Root;
            this.dataLayoutControl1.Size = new System.Drawing.Size(666, 718);
            this.dataLayoutControl1.TabIndex = 4;
            this.dataLayoutControl1.Text = "dataLayoutControl1";
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup1});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(666, 718);
            this.Root.TextVisible = false;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.AllowDrawBackground = false;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup2,
            this.layoutControlGroup3,
            this.layoutControlGroup4,
            this.layoutControlGroup5});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "autoGeneratedGroup0";
            this.layoutControlGroup1.Size = new System.Drawing.Size(646, 698);
            // 
            // StocktakingDateDateEdit
            // 
            this.StocktakingDateDateEdit.EditValue = null;
            this.StocktakingDateDateEdit.Location = new System.Drawing.Point(116, 45);
            this.StocktakingDateDateEdit.MenuManager = this.barManager1;
            this.StocktakingDateDateEdit.Name = "StocktakingDateDateEdit";
            this.StocktakingDateDateEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.StocktakingDateDateEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.StocktakingDateDateEdit.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.StocktakingDateDateEdit.Properties.CalendarTimeProperties.Mask.EditMask = "T";
            this.StocktakingDateDateEdit.Size = new System.Drawing.Size(526, 20);
            this.StocktakingDateDateEdit.StyleController = this.dataLayoutControl1;
            this.StocktakingDateDateEdit.TabIndex = 5;
            // 
            // ItemForStocktakingDate
            // 
            this.ItemForStocktakingDate.Control = this.StocktakingDateDateEdit;
            this.ItemForStocktakingDate.Location = new System.Drawing.Point(0, 0);
            this.ItemForStocktakingDate.Name = "ItemForStocktakingDate";
            this.ItemForStocktakingDate.Size = new System.Drawing.Size(622, 24);
            this.ItemForStocktakingDate.TextSize = new System.Drawing.Size(80, 13);
            // 
            // VoucherNumberTextEdit
            // 
            this.VoucherNumberTextEdit.Location = new System.Drawing.Point(116, 69);
            this.VoucherNumberTextEdit.MenuManager = this.barManager1;
            this.VoucherNumberTextEdit.Name = "VoucherNumberTextEdit";
            this.VoucherNumberTextEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.VoucherNumberTextEdit.Size = new System.Drawing.Size(215, 20);
            this.VoucherNumberTextEdit.StyleController = this.dataLayoutControl1;
            this.VoucherNumberTextEdit.TabIndex = 6;
            // 
            // ItemForVoucherNumber
            // 
            this.ItemForVoucherNumber.Control = this.VoucherNumberTextEdit;
            this.ItemForVoucherNumber.Location = new System.Drawing.Point(0, 24);
            this.ItemForVoucherNumber.Name = "ItemForVoucherNumber";
            this.ItemForVoucherNumber.Size = new System.Drawing.Size(311, 24);
            this.ItemForVoucherNumber.TextSize = new System.Drawing.Size(80, 13);
            // 
            // ItemForStocktakingType
            // 
            this.ItemForStocktakingType.Control = this.StocktakingTypeTextEdit;
            this.ItemForStocktakingType.Location = new System.Drawing.Point(0, 48);
            this.ItemForStocktakingType.Name = "ItemForStocktakingType";
            this.ItemForStocktakingType.Size = new System.Drawing.Size(622, 24);
            this.ItemForStocktakingType.TextSize = new System.Drawing.Size(80, 13);
            // 
            // ItemForStocktakingStatus
            // 
            this.ItemForStocktakingStatus.Control = this.StocktakingStatusTextEdit;
            this.ItemForStocktakingStatus.Location = new System.Drawing.Point(0, 72);
            this.ItemForStocktakingStatus.Name = "ItemForStocktakingStatus";
            this.ItemForStocktakingStatus.Size = new System.Drawing.Size(622, 24);
            this.ItemForStocktakingStatus.TextSize = new System.Drawing.Size(80, 13);
            // 
            // ItemForWarehouseName
            // 
            this.ItemForWarehouseName.Control = this.WarehouseNameTextEdit;
            this.ItemForWarehouseName.Location = new System.Drawing.Point(0, 96);
            this.ItemForWarehouseName.Name = "ItemForWarehouseName";
            this.ItemForWarehouseName.Size = new System.Drawing.Size(622, 24);
            this.ItemForWarehouseName.TextSize = new System.Drawing.Size(80, 13);
            // 
            // StartDateDateEdit
            // 
            this.StartDateDateEdit.EditValue = null;
            this.StartDateDateEdit.Location = new System.Drawing.Point(116, 210);
            this.StartDateDateEdit.MenuManager = this.barManager1;
            this.StartDateDateEdit.Name = "StartDateDateEdit";
            this.StartDateDateEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.StartDateDateEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.StartDateDateEdit.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.StartDateDateEdit.Properties.CalendarTimeProperties.Mask.EditMask = "T";
            this.StartDateDateEdit.Size = new System.Drawing.Size(526, 20);
            this.StartDateDateEdit.StyleController = this.dataLayoutControl1;
            this.StartDateDateEdit.TabIndex = 13;
            // 
            // ItemForStartDate
            // 
            this.ItemForStartDate.Control = this.StartDateDateEdit;
            this.ItemForStartDate.Location = new System.Drawing.Point(0, 0);
            this.ItemForStartDate.Name = "ItemForStartDate";
            this.ItemForStartDate.Size = new System.Drawing.Size(622, 24);
            this.ItemForStartDate.TextSize = new System.Drawing.Size(80, 13);
            // 
            // EndDateDateEdit
            // 
            this.EndDateDateEdit.EditValue = null;
            this.EndDateDateEdit.Location = new System.Drawing.Point(116, 234);
            this.EndDateDateEdit.MenuManager = this.barManager1;
            this.EndDateDateEdit.Name = "EndDateDateEdit";
            this.EndDateDateEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.EndDateDateEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.EndDateDateEdit.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.EndDateDateEdit.Properties.CalendarTimeProperties.Mask.EditMask = "T";
            this.EndDateDateEdit.Size = new System.Drawing.Size(526, 20);
            this.EndDateDateEdit.StyleController = this.dataLayoutControl1;
            this.EndDateDateEdit.TabIndex = 14;
            // 
            // ItemForEndDate
            // 
            this.ItemForEndDate.Control = this.EndDateDateEdit;
            this.ItemForEndDate.Location = new System.Drawing.Point(0, 24);
            this.ItemForEndDate.Name = "ItemForEndDate";
            this.ItemForEndDate.Size = new System.Drawing.Size(622, 24);
            this.ItemForEndDate.TextSize = new System.Drawing.Size(80, 13);
            // 
            // CountedByTextEdit
            // 
            this.CountedByTextEdit.Location = new System.Drawing.Point(116, 375);
            this.CountedByTextEdit.MenuManager = this.barManager1;
            this.CountedByTextEdit.Name = "CountedByTextEdit";
            this.CountedByTextEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.CountedByTextEdit.Size = new System.Drawing.Size(526, 20);
            this.CountedByTextEdit.StyleController = this.dataLayoutControl1;
            this.CountedByTextEdit.TabIndex = 15;
            // 
            // ItemForCountedBy
            // 
            this.ItemForCountedBy.Control = this.CountedByTextEdit;
            this.ItemForCountedBy.Location = new System.Drawing.Point(0, 0);
            this.ItemForCountedBy.Name = "ItemForCountedBy";
            this.ItemForCountedBy.Size = new System.Drawing.Size(622, 24);
            this.ItemForCountedBy.TextSize = new System.Drawing.Size(80, 13);
            // 
            // CountedDateDateEdit
            // 
            this.CountedDateDateEdit.EditValue = null;
            this.CountedDateDateEdit.Location = new System.Drawing.Point(116, 258);
            this.CountedDateDateEdit.MenuManager = this.barManager1;
            this.CountedDateDateEdit.Name = "CountedDateDateEdit";
            this.CountedDateDateEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.CountedDateDateEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CountedDateDateEdit.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CountedDateDateEdit.Properties.CalendarTimeProperties.Mask.EditMask = "T";
            this.CountedDateDateEdit.Size = new System.Drawing.Size(526, 20);
            this.CountedDateDateEdit.StyleController = this.dataLayoutControl1;
            this.CountedDateDateEdit.TabIndex = 16;
            // 
            // ItemForCountedDate
            // 
            this.ItemForCountedDate.Control = this.CountedDateDateEdit;
            this.ItemForCountedDate.Location = new System.Drawing.Point(0, 48);
            this.ItemForCountedDate.Name = "ItemForCountedDate";
            this.ItemForCountedDate.Size = new System.Drawing.Size(622, 24);
            this.ItemForCountedDate.TextSize = new System.Drawing.Size(80, 13);
            // 
            // ReviewedByTextEdit
            // 
            this.ReviewedByTextEdit.Location = new System.Drawing.Point(116, 399);
            this.ReviewedByTextEdit.MenuManager = this.barManager1;
            this.ReviewedByTextEdit.Name = "ReviewedByTextEdit";
            this.ReviewedByTextEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.ReviewedByTextEdit.Size = new System.Drawing.Size(526, 20);
            this.ReviewedByTextEdit.StyleController = this.dataLayoutControl1;
            this.ReviewedByTextEdit.TabIndex = 17;
            // 
            // ItemForReviewedBy
            // 
            this.ItemForReviewedBy.Control = this.ReviewedByTextEdit;
            this.ItemForReviewedBy.Location = new System.Drawing.Point(0, 24);
            this.ItemForReviewedBy.Name = "ItemForReviewedBy";
            this.ItemForReviewedBy.Size = new System.Drawing.Size(622, 24);
            this.ItemForReviewedBy.TextSize = new System.Drawing.Size(80, 13);
            // 
            // ReviewedDateDateEdit
            // 
            this.ReviewedDateDateEdit.EditValue = null;
            this.ReviewedDateDateEdit.Location = new System.Drawing.Point(116, 282);
            this.ReviewedDateDateEdit.MenuManager = this.barManager1;
            this.ReviewedDateDateEdit.Name = "ReviewedDateDateEdit";
            this.ReviewedDateDateEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.ReviewedDateDateEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ReviewedDateDateEdit.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ReviewedDateDateEdit.Properties.CalendarTimeProperties.Mask.EditMask = "T";
            this.ReviewedDateDateEdit.Size = new System.Drawing.Size(526, 20);
            this.ReviewedDateDateEdit.StyleController = this.dataLayoutControl1;
            this.ReviewedDateDateEdit.TabIndex = 18;
            // 
            // ItemForReviewedDate
            // 
            this.ItemForReviewedDate.Control = this.ReviewedDateDateEdit;
            this.ItemForReviewedDate.Location = new System.Drawing.Point(0, 72);
            this.ItemForReviewedDate.Name = "ItemForReviewedDate";
            this.ItemForReviewedDate.Size = new System.Drawing.Size(622, 24);
            this.ItemForReviewedDate.TextSize = new System.Drawing.Size(80, 13);
            // 
            // ApprovedByTextEdit
            // 
            this.ApprovedByTextEdit.Location = new System.Drawing.Point(116, 423);
            this.ApprovedByTextEdit.MenuManager = this.barManager1;
            this.ApprovedByTextEdit.Name = "ApprovedByTextEdit";
            this.ApprovedByTextEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.ApprovedByTextEdit.Size = new System.Drawing.Size(526, 20);
            this.ApprovedByTextEdit.StyleController = this.dataLayoutControl1;
            this.ApprovedByTextEdit.TabIndex = 19;
            // 
            // ItemForApprovedBy
            // 
            this.ItemForApprovedBy.Control = this.ApprovedByTextEdit;
            this.ItemForApprovedBy.Location = new System.Drawing.Point(0, 48);
            this.ItemForApprovedBy.Name = "ItemForApprovedBy";
            this.ItemForApprovedBy.Size = new System.Drawing.Size(622, 24);
            this.ItemForApprovedBy.TextSize = new System.Drawing.Size(80, 13);
            // 
            // ApprovedDateDateEdit
            // 
            this.ApprovedDateDateEdit.EditValue = null;
            this.ApprovedDateDateEdit.Location = new System.Drawing.Point(116, 306);
            this.ApprovedDateDateEdit.MenuManager = this.barManager1;
            this.ApprovedDateDateEdit.Name = "ApprovedDateDateEdit";
            this.ApprovedDateDateEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.ApprovedDateDateEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ApprovedDateDateEdit.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ApprovedDateDateEdit.Properties.CalendarTimeProperties.Mask.EditMask = "T";
            this.ApprovedDateDateEdit.Size = new System.Drawing.Size(526, 20);
            this.ApprovedDateDateEdit.StyleController = this.dataLayoutControl1;
            this.ApprovedDateDateEdit.TabIndex = 20;
            // 
            // ItemForApprovedDate
            // 
            this.ItemForApprovedDate.Control = this.ApprovedDateDateEdit;
            this.ItemForApprovedDate.Location = new System.Drawing.Point(0, 96);
            this.ItemForApprovedDate.Name = "ItemForApprovedDate";
            this.ItemForApprovedDate.Size = new System.Drawing.Size(622, 24);
            this.ItemForApprovedDate.TextSize = new System.Drawing.Size(80, 13);
            // 
            // ItemForNotes
            // 
            this.ItemForNotes.Control = this.NotesTextEdit;
            this.ItemForNotes.Location = new System.Drawing.Point(0, 48);
            this.ItemForNotes.Name = "ItemForNotes";
            this.ItemForNotes.Size = new System.Drawing.Size(311, 158);
            this.ItemForNotes.TextLocation = DevExpress.Utils.Locations.Top;
            this.ItemForNotes.TextSize = new System.Drawing.Size(80, 13);
            // 
            // ItemForReason
            // 
            this.ItemForReason.Control = this.ReasonTextEdit;
            this.ItemForReason.Location = new System.Drawing.Point(311, 48);
            this.ItemForReason.Name = "ItemForReason";
            this.ItemForReason.Size = new System.Drawing.Size(311, 158);
            this.ItemForReason.TextLocation = DevExpress.Utils.Locations.Top;
            this.ItemForReason.TextSize = new System.Drawing.Size(80, 13);
            // 
            // ItemForIsLocked
            // 
            this.ItemForIsLocked.Control = this.IsLockedCheckEdit;
            this.ItemForIsLocked.Location = new System.Drawing.Point(311, 24);
            this.ItemForIsLocked.Name = "ItemForIsLocked";
            this.ItemForIsLocked.Size = new System.Drawing.Size(311, 24);
            this.ItemForIsLocked.Text = "Tình trạng";
            this.ItemForIsLocked.TextSize = new System.Drawing.Size(80, 13);
            // 
            // LockedDateDateEdit
            // 
            this.LockedDateDateEdit.EditValue = null;
            this.LockedDateDateEdit.Location = new System.Drawing.Point(116, 516);
            this.LockedDateDateEdit.MenuManager = this.barManager1;
            this.LockedDateDateEdit.Name = "LockedDateDateEdit";
            this.LockedDateDateEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.LockedDateDateEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.LockedDateDateEdit.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.LockedDateDateEdit.Properties.CalendarTimeProperties.Mask.EditMask = "T";
            this.LockedDateDateEdit.Size = new System.Drawing.Size(526, 20);
            this.LockedDateDateEdit.StyleController = this.dataLayoutControl1;
            this.LockedDateDateEdit.TabIndex = 24;
            // 
            // ItemForLockedDate
            // 
            this.ItemForLockedDate.Control = this.LockedDateDateEdit;
            this.ItemForLockedDate.Location = new System.Drawing.Point(0, 24);
            this.ItemForLockedDate.Name = "ItemForLockedDate";
            this.ItemForLockedDate.Size = new System.Drawing.Size(622, 24);
            this.ItemForLockedDate.TextSize = new System.Drawing.Size(80, 13);
            // 
            // LockedByTextEdit
            // 
            this.LockedByTextEdit.Location = new System.Drawing.Point(116, 492);
            this.LockedByTextEdit.MenuManager = this.barManager1;
            this.LockedByTextEdit.Name = "LockedByTextEdit";
            this.LockedByTextEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.LockedByTextEdit.Size = new System.Drawing.Size(526, 20);
            this.LockedByTextEdit.StyleController = this.dataLayoutControl1;
            this.LockedByTextEdit.TabIndex = 25;
            // 
            // ItemForLockedBy
            // 
            this.ItemForLockedBy.Control = this.LockedByTextEdit;
            this.ItemForLockedBy.Location = new System.Drawing.Point(0, 0);
            this.ItemForLockedBy.Name = "ItemForLockedBy";
            this.ItemForLockedBy.Size = new System.Drawing.Size(622, 24);
            this.ItemForLockedBy.TextSize = new System.Drawing.Size(80, 13);
            // 
            // StocktakingTypeTextEdit
            // 
            this.StocktakingTypeTextEdit.Location = new System.Drawing.Point(116, 93);
            this.StocktakingTypeTextEdit.MenuManager = this.barManager1;
            this.StocktakingTypeTextEdit.Name = "StocktakingTypeTextEdit";
            this.StocktakingTypeTextEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.StocktakingTypeTextEdit.Properties.Appearance.Options.UseTextOptions = true;
            this.StocktakingTypeTextEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.StocktakingTypeTextEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.StocktakingTypeTextEdit.Size = new System.Drawing.Size(526, 20);
            this.StocktakingTypeTextEdit.StyleController = this.dataLayoutControl1;
            this.StocktakingTypeTextEdit.TabIndex = 7;
            // 
            // StocktakingStatusTextEdit
            // 
            this.StocktakingStatusTextEdit.Location = new System.Drawing.Point(116, 117);
            this.StocktakingStatusTextEdit.MenuManager = this.barManager1;
            this.StocktakingStatusTextEdit.Name = "StocktakingStatusTextEdit";
            this.StocktakingStatusTextEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.StocktakingStatusTextEdit.Properties.Appearance.Options.UseTextOptions = true;
            this.StocktakingStatusTextEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.StocktakingStatusTextEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.StocktakingStatusTextEdit.Properties.Mask.EditMask = "N0";
            this.StocktakingStatusTextEdit.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.StocktakingStatusTextEdit.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.StocktakingStatusTextEdit.Size = new System.Drawing.Size(526, 20);
            this.StocktakingStatusTextEdit.StyleController = this.dataLayoutControl1;
            this.StocktakingStatusTextEdit.TabIndex = 8;
            // 
            // WarehouseNameTextEdit
            // 
            this.WarehouseNameTextEdit.Location = new System.Drawing.Point(116, 141);
            this.WarehouseNameTextEdit.MenuManager = this.barManager1;
            this.WarehouseNameTextEdit.Name = "WarehouseNameTextEdit";
            this.WarehouseNameTextEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.WarehouseNameTextEdit.Properties.NullText = "";
            this.WarehouseNameTextEdit.Properties.PopupView = this.searchLookUpEdit1View;
            this.WarehouseNameTextEdit.Size = new System.Drawing.Size(526, 20);
            this.WarehouseNameTextEdit.StyleController = this.dataLayoutControl1;
            this.WarehouseNameTextEdit.TabIndex = 11;
            // 
            // searchLookUpEdit1View
            // 
            this.searchLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.searchLookUpEdit1View.Name = "searchLookUpEdit1View";
            this.searchLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.searchLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // layoutControlGroup2
            // 
            this.layoutControlGroup2.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.layoutControlGroup2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.ItemForStocktakingDate,
            this.ItemForVoucherNumber,
            this.ItemForStocktakingType,
            this.ItemForStocktakingStatus,
            this.ItemForWarehouseName,
            this.ItemForIsLocked});
            this.layoutControlGroup2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup2.Name = "layoutControlGroup2";
            this.layoutControlGroup2.Size = new System.Drawing.Size(646, 165);
            this.layoutControlGroup2.Text = "Thông tin phiếu";
            // 
            // layoutControlGroup3
            // 
            this.layoutControlGroup3.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.layoutControlGroup3.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.ItemForStartDate,
            this.ItemForEndDate,
            this.ItemForCountedDate,
            this.ItemForReviewedDate,
            this.ItemForApprovedDate});
            this.layoutControlGroup3.Location = new System.Drawing.Point(0, 165);
            this.layoutControlGroup3.Name = "layoutControlGroup3";
            this.layoutControlGroup3.Size = new System.Drawing.Size(646, 165);
            this.layoutControlGroup3.Text = "Thời gian kiểm kho";
            // 
            // layoutControlGroup4
            // 
            this.layoutControlGroup4.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.layoutControlGroup4.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.ItemForCountedBy,
            this.ItemForReviewedBy,
            this.ItemForApprovedBy});
            this.layoutControlGroup4.Location = new System.Drawing.Point(0, 330);
            this.layoutControlGroup4.Name = "layoutControlGroup4";
            this.layoutControlGroup4.Size = new System.Drawing.Size(646, 117);
            this.layoutControlGroup4.Text = "Nhân sự liên quan";
            // 
            // IsLockedCheckEdit
            // 
            this.IsLockedCheckEdit.Location = new System.Drawing.Point(427, 69);
            this.IsLockedCheckEdit.MenuManager = this.barManager1;
            this.IsLockedCheckEdit.Name = "IsLockedCheckEdit";
            this.IsLockedCheckEdit.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Default;
            this.IsLockedCheckEdit.Properties.OffText = "Off";
            this.IsLockedCheckEdit.Properties.OnText = "On";
            this.IsLockedCheckEdit.Size = new System.Drawing.Size(215, 18);
            this.IsLockedCheckEdit.StyleController = this.dataLayoutControl1;
            this.IsLockedCheckEdit.TabIndex = 23;
            // 
            // NotesTextEdit
            // 
            this.NotesTextEdit.Location = new System.Drawing.Point(24, 556);
            this.NotesTextEdit.MenuManager = this.barManager1;
            this.NotesTextEdit.Name = "NotesTextEdit";
            this.NotesTextEdit.Size = new System.Drawing.Size(307, 138);
            this.NotesTextEdit.StyleController = this.dataLayoutControl1;
            this.NotesTextEdit.TabIndex = 21;
            // 
            // ReasonTextEdit
            // 
            this.ReasonTextEdit.Location = new System.Drawing.Point(335, 556);
            this.ReasonTextEdit.MenuManager = this.barManager1;
            this.ReasonTextEdit.Name = "ReasonTextEdit";
            this.ReasonTextEdit.Size = new System.Drawing.Size(307, 138);
            this.ReasonTextEdit.StyleController = this.dataLayoutControl1;
            this.ReasonTextEdit.TabIndex = 22;
            // 
            // layoutControlGroup5
            // 
            this.layoutControlGroup5.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.layoutControlGroup5.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.ItemForNotes,
            this.ItemForReason,
            this.ItemForLockedBy,
            this.ItemForLockedDate});
            this.layoutControlGroup5.Location = new System.Drawing.Point(0, 447);
            this.layoutControlGroup5.Name = "layoutControlGroup5";
            this.layoutControlGroup5.Size = new System.Drawing.Size(646, 251);
            this.layoutControlGroup5.Text = "Khác";
            // 
            // FrmFrmStocktakingMasterAddEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(666, 742);
            this.Controls.Add(this.dataLayoutControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmFrmStocktakingMasterAddEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).EndInit();
            this.dataLayoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StocktakingDateDateEdit.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StocktakingDateDateEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForStocktakingDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VoucherNumberTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForVoucherNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForStocktakingType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForStocktakingStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForWarehouseName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartDateDateEdit.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartDateDateEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForStartDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EndDateDateEdit.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EndDateDateEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForEndDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CountedByTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForCountedBy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CountedDateDateEdit.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CountedDateDateEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForCountedDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReviewedByTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForReviewedBy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReviewedDateDateEdit.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReviewedDateDateEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForReviewedDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ApprovedByTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForApprovedBy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ApprovedDateDateEdit.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ApprovedDateDateEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForApprovedDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForNotes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForReason)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForIsLocked)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LockedDateDateEdit.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LockedDateDateEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForLockedDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LockedByTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForLockedBy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StocktakingTypeTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StocktakingStatusTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WarehouseNameTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.IsLockedCheckEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NotesTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReasonTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup5)).EndInit();
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
        private DateEdit StocktakingDateDateEdit;
        private TextEdit VoucherNumberTextEdit;
        private DateEdit StartDateDateEdit;
        private DateEdit EndDateDateEdit;
        private TextEdit CountedByTextEdit;
        private DateEdit CountedDateDateEdit;
        private TextEdit ReviewedByTextEdit;
        private DateEdit ReviewedDateDateEdit;
        private TextEdit ApprovedByTextEdit;
        private DateEdit ApprovedDateDateEdit;
        private DateEdit LockedDateDateEdit;
        private TextEdit LockedByTextEdit;
        private LayoutControlGroup Root;
        private LayoutControlGroup layoutControlGroup1;
        private LayoutControlItem ItemForStocktakingDate;
        private LayoutControlItem ItemForVoucherNumber;
        private LayoutControlItem ItemForStocktakingType;
        private LayoutControlItem ItemForStocktakingStatus;
        private LayoutControlItem ItemForWarehouseName;
        private LayoutControlItem ItemForStartDate;
        private LayoutControlItem ItemForEndDate;
        private LayoutControlItem ItemForCountedBy;
        private LayoutControlItem ItemForCountedDate;
        private LayoutControlItem ItemForReviewedBy;
        private LayoutControlItem ItemForReviewedDate;
        private LayoutControlItem ItemForApprovedBy;
        private LayoutControlItem ItemForApprovedDate;
        private LayoutControlItem ItemForNotes;
        private LayoutControlItem ItemForReason;
        private LayoutControlItem ItemForIsLocked;
        private LayoutControlItem ItemForLockedDate;
        private LayoutControlItem ItemForLockedBy;
        private ComboBoxEdit StocktakingTypeTextEdit;
        private CheckedComboBoxEdit StocktakingStatusTextEdit;
        private SearchLookUpEdit WarehouseNameTextEdit;
        private DevExpress.XtraGrid.Views.Grid.GridView searchLookUpEdit1View;
        private LayoutControlGroup layoutControlGroup2;
        private LayoutControlGroup layoutControlGroup3;
        private ToggleSwitch IsLockedCheckEdit;
        private MemoEdit NotesTextEdit;
        private MemoEdit ReasonTextEdit;
        private LayoutControlGroup layoutControlGroup4;
        private LayoutControlGroup layoutControlGroup5;
    }
}