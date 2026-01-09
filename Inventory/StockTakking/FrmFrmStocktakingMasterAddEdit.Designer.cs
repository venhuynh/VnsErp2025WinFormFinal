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
            this.StocktakingDateDateEdit = new DevExpress.XtraEditors.DateEdit();
            this.VoucherNumberTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.StartDateDateEdit = new DevExpress.XtraEditors.DateEdit();
            this.EndDateDateEdit = new DevExpress.XtraEditors.DateEdit();
            this.CountedByTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.CountedDateDateEdit = new DevExpress.XtraEditors.DateEdit();
            this.ReviewedByTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.ReviewedDateDateEdit = new DevExpress.XtraEditors.DateEdit();
            this.ApprovedByTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.ApprovedDateDateEdit = new DevExpress.XtraEditors.DateEdit();
            this.LockedDateDateEdit = new DevExpress.XtraEditors.DateEdit();
            this.LockedByTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.StocktakingTypeComboBoxEdit = new DevExpress.XtraEditors.ComboBoxEdit();
            this.WarehouseNameSearchLookupEdit = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.companyBranchDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.CompanyBranchSearchLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colThongTinHtml = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CompanyBrachHypertextLabel = new DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel();
            this.IsLockedToggleSwitch = new DevExpress.XtraEditors.ToggleSwitch();
            this.NotesTextEdit = new DevExpress.XtraEditors.MemoEdit();
            this.ReasonTextEdit = new DevExpress.XtraEditors.MemoEdit();
            this.StocktakingStatusComboBoxEdit = new DevExpress.XtraEditors.ComboBoxEdit();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.ItemForStocktakingDate = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForVoucherNumber = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForStocktakingType = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForStocktakingStatus = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForWarehouseName = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForIsLocked = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlGroup3 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.ItemForStartDate = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForEndDate = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForCountedDate = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForReviewedDate = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForApprovedDate = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlGroup4 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.ItemForCountedBy = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForReviewedBy = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForApprovedBy = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlGroup5 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.ItemForNotes = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForReason = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForLockedBy = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForLockedDate = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).BeginInit();
            this.dataLayoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.StocktakingDateDateEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StocktakingDateDateEdit.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VoucherNumberTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartDateDateEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartDateDateEdit.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EndDateDateEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EndDateDateEdit.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CountedByTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CountedDateDateEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CountedDateDateEdit.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReviewedByTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReviewedDateDateEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReviewedDateDateEdit.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ApprovedByTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ApprovedDateDateEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ApprovedDateDateEdit.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LockedDateDateEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LockedDateDateEdit.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LockedByTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StocktakingTypeComboBoxEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WarehouseNameSearchLookupEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.companyBranchDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CompanyBranchSearchLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CompanyBrachHypertextLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.IsLockedToggleSwitch.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NotesTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReasonTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StocktakingStatusComboBoxEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForStocktakingDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForVoucherNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForStocktakingType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForStocktakingStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForWarehouseName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForIsLocked)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForStartDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForEndDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForCountedDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForReviewedDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForApprovedDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForCountedBy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForReviewedBy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForApprovedBy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForNotes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForReason)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForLockedBy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForLockedDate)).BeginInit();
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
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 1029);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(666, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 24);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 1005);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(666, 24);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 1005);
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
            this.dataLayoutControl1.Controls.Add(this.StocktakingTypeComboBoxEdit);
            this.dataLayoutControl1.Controls.Add(this.WarehouseNameSearchLookupEdit);
            this.dataLayoutControl1.Controls.Add(this.IsLockedToggleSwitch);
            this.dataLayoutControl1.Controls.Add(this.NotesTextEdit);
            this.dataLayoutControl1.Controls.Add(this.ReasonTextEdit);
            this.dataLayoutControl1.Controls.Add(this.StocktakingStatusComboBoxEdit);
            this.dataLayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataLayoutControl1.Location = new System.Drawing.Point(0, 24);
            this.dataLayoutControl1.Name = "dataLayoutControl1";
            this.dataLayoutControl1.Root = this.Root;
            this.dataLayoutControl1.Size = new System.Drawing.Size(666, 1005);
            this.dataLayoutControl1.TabIndex = 4;
            this.dataLayoutControl1.Text = "dataLayoutControl1";
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
            this.StocktakingDateDateEdit.Size = new System.Drawing.Size(526, 20);
            this.StocktakingDateDateEdit.StyleController = this.dataLayoutControl1;
            this.StocktakingDateDateEdit.TabIndex = 5;
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
            this.StartDateDateEdit.Size = new System.Drawing.Size(526, 20);
            this.StartDateDateEdit.StyleController = this.dataLayoutControl1;
            this.StartDateDateEdit.TabIndex = 13;
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
            this.EndDateDateEdit.Size = new System.Drawing.Size(526, 20);
            this.EndDateDateEdit.StyleController = this.dataLayoutControl1;
            this.EndDateDateEdit.TabIndex = 14;
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
            this.CountedDateDateEdit.Size = new System.Drawing.Size(526, 20);
            this.CountedDateDateEdit.StyleController = this.dataLayoutControl1;
            this.CountedDateDateEdit.TabIndex = 16;
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
            this.ReviewedDateDateEdit.Size = new System.Drawing.Size(526, 20);
            this.ReviewedDateDateEdit.StyleController = this.dataLayoutControl1;
            this.ReviewedDateDateEdit.TabIndex = 18;
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
            this.ApprovedDateDateEdit.Size = new System.Drawing.Size(526, 20);
            this.ApprovedDateDateEdit.StyleController = this.dataLayoutControl1;
            this.ApprovedDateDateEdit.TabIndex = 20;
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
            this.LockedDateDateEdit.Size = new System.Drawing.Size(526, 20);
            this.LockedDateDateEdit.StyleController = this.dataLayoutControl1;
            this.LockedDateDateEdit.TabIndex = 24;
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
            // StocktakingTypeComboBoxEdit
            // 
            this.StocktakingTypeComboBoxEdit.AllowHtmlTextInToolTip = DevExpress.Utils.DefaultBoolean.True;
            this.StocktakingTypeComboBoxEdit.Location = new System.Drawing.Point(116, 93);
            this.StocktakingTypeComboBoxEdit.MenuManager = this.barManager1;
            this.StocktakingTypeComboBoxEdit.Name = "StocktakingTypeComboBoxEdit";
            this.StocktakingTypeComboBoxEdit.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.StocktakingTypeComboBoxEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.StocktakingTypeComboBoxEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.StocktakingTypeComboBoxEdit.Size = new System.Drawing.Size(526, 20);
            this.StocktakingTypeComboBoxEdit.StyleController = this.dataLayoutControl1;
            this.StocktakingTypeComboBoxEdit.TabIndex = 7;
            // 
            // WarehouseNameSearchLookupEdit
            // 
            this.WarehouseNameSearchLookupEdit.AllowHtmlTextInToolTip = DevExpress.Utils.DefaultBoolean.True;
            this.WarehouseNameSearchLookupEdit.Location = new System.Drawing.Point(116, 141);
            this.WarehouseNameSearchLookupEdit.MenuManager = this.barManager1;
            this.WarehouseNameSearchLookupEdit.Name = "WarehouseNameSearchLookupEdit";
            this.WarehouseNameSearchLookupEdit.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.WarehouseNameSearchLookupEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.WarehouseNameSearchLookupEdit.Properties.DataSource = this.companyBranchDtoBindingSource;
            this.WarehouseNameSearchLookupEdit.Properties.DisplayMember = "ThongTinHtml";
            this.WarehouseNameSearchLookupEdit.Properties.NullText = "";
            this.WarehouseNameSearchLookupEdit.Properties.PopupView = this.CompanyBranchSearchLookUpEdit1View;
            this.WarehouseNameSearchLookupEdit.Properties.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.CompanyBrachHypertextLabel});
            this.WarehouseNameSearchLookupEdit.Properties.ValueMember = "Id";
            this.WarehouseNameSearchLookupEdit.Size = new System.Drawing.Size(526, 20);
            this.WarehouseNameSearchLookupEdit.StyleController = this.dataLayoutControl1;
            this.WarehouseNameSearchLookupEdit.TabIndex = 11;
            // 
            // companyBranchDtoBindingSource
            // 
            this.companyBranchDtoBindingSource.DataSource = typeof(DTO.MasterData.Company.CompanyBranchDto);
            // 
            // CompanyBranchSearchLookUpEdit1View
            // 
            this.CompanyBranchSearchLookUpEdit1View.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colThongTinHtml});
            this.CompanyBranchSearchLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.CompanyBranchSearchLookUpEdit1View.Name = "CompanyBranchSearchLookUpEdit1View";
            this.CompanyBranchSearchLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.CompanyBranchSearchLookUpEdit1View.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            this.CompanyBranchSearchLookUpEdit1View.OptionsView.RowAutoHeight = true;
            this.CompanyBranchSearchLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // colThongTinHtml
            // 
            this.colThongTinHtml.ColumnEdit = this.CompanyBrachHypertextLabel;
            this.colThongTinHtml.FieldName = "ThongTinHtml";
            this.colThongTinHtml.Name = "colThongTinHtml";
            this.colThongTinHtml.Visible = true;
            this.colThongTinHtml.VisibleIndex = 0;
            // 
            // CompanyBrachHypertextLabel
            // 
            this.CompanyBrachHypertextLabel.Name = "CompanyBrachHypertextLabel";
            // 
            // IsLockedToggleSwitch
            // 
            this.IsLockedToggleSwitch.AllowHtmlTextInToolTip = DevExpress.Utils.DefaultBoolean.True;
            this.IsLockedToggleSwitch.EditValue = true;
            this.IsLockedToggleSwitch.Location = new System.Drawing.Point(427, 69);
            this.IsLockedToggleSwitch.MenuManager = this.barManager1;
            this.IsLockedToggleSwitch.Name = "IsLockedToggleSwitch";
            this.IsLockedToggleSwitch.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.IsLockedToggleSwitch.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Default;
            this.IsLockedToggleSwitch.Properties.OffText = "<color=\'red\'>Đã khóa</color>";
            this.IsLockedToggleSwitch.Properties.OnText = "<color=\'blue\'>Chưa khóa</color>";
            this.IsLockedToggleSwitch.Size = new System.Drawing.Size(215, 18);
            this.IsLockedToggleSwitch.StyleController = this.dataLayoutControl1;
            this.IsLockedToggleSwitch.TabIndex = 23;
            // 
            // NotesTextEdit
            // 
            this.NotesTextEdit.Location = new System.Drawing.Point(24, 556);
            this.NotesTextEdit.MenuManager = this.barManager1;
            this.NotesTextEdit.Name = "NotesTextEdit";
            this.NotesTextEdit.Size = new System.Drawing.Size(307, 425);
            this.NotesTextEdit.StyleController = this.dataLayoutControl1;
            this.NotesTextEdit.TabIndex = 21;
            // 
            // ReasonTextEdit
            // 
            this.ReasonTextEdit.Location = new System.Drawing.Point(335, 556);
            this.ReasonTextEdit.MenuManager = this.barManager1;
            this.ReasonTextEdit.Name = "ReasonTextEdit";
            this.ReasonTextEdit.Size = new System.Drawing.Size(307, 425);
            this.ReasonTextEdit.StyleController = this.dataLayoutControl1;
            this.ReasonTextEdit.TabIndex = 22;
            // 
            // StocktakingStatusComboBoxEdit
            // 
            this.StocktakingStatusComboBoxEdit.AllowHtmlTextInToolTip = DevExpress.Utils.DefaultBoolean.True;
            this.StocktakingStatusComboBoxEdit.Location = new System.Drawing.Point(116, 117);
            this.StocktakingStatusComboBoxEdit.MenuManager = this.barManager1;
            this.StocktakingStatusComboBoxEdit.Name = "StocktakingStatusComboBoxEdit";
            this.StocktakingStatusComboBoxEdit.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.StocktakingStatusComboBoxEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.StocktakingStatusComboBoxEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.StocktakingStatusComboBoxEdit.Size = new System.Drawing.Size(526, 20);
            this.StocktakingStatusComboBoxEdit.StyleController = this.dataLayoutControl1;
            this.StocktakingStatusComboBoxEdit.TabIndex = 8;
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup1});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(666, 1005);
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
            this.layoutControlGroup1.Size = new System.Drawing.Size(646, 985);
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
            // ItemForStocktakingDate
            // 
            this.ItemForStocktakingDate.AllowHtmlStringInCaption = true;
            this.ItemForStocktakingDate.Control = this.StocktakingDateDateEdit;
            this.ItemForStocktakingDate.Location = new System.Drawing.Point(0, 0);
            this.ItemForStocktakingDate.Name = "ItemForStocktakingDate";
            this.ItemForStocktakingDate.OptionsToolTip.AllowHtmlString = DevExpress.Utils.DefaultBoolean.True;
            this.ItemForStocktakingDate.OptionsToolTip.IconAllowHtmlString = DevExpress.Utils.DefaultBoolean.True;
            this.ItemForStocktakingDate.Size = new System.Drawing.Size(622, 24);
            this.ItemForStocktakingDate.Text = "Ngày kiểm kho <color=\'red\'>*</color>";
            this.ItemForStocktakingDate.TextSize = new System.Drawing.Size(80, 13);
            // 
            // ItemForVoucherNumber
            // 
            this.ItemForVoucherNumber.AllowHtmlStringInCaption = true;
            this.ItemForVoucherNumber.Control = this.VoucherNumberTextEdit;
            this.ItemForVoucherNumber.Location = new System.Drawing.Point(0, 24);
            this.ItemForVoucherNumber.Name = "ItemForVoucherNumber";
            this.ItemForVoucherNumber.OptionsToolTip.AllowHtmlString = DevExpress.Utils.DefaultBoolean.True;
            this.ItemForVoucherNumber.OptionsToolTip.IconAllowHtmlString = DevExpress.Utils.DefaultBoolean.True;
            this.ItemForVoucherNumber.Size = new System.Drawing.Size(311, 24);
            this.ItemForVoucherNumber.Text = "Số phiếu <color=\'red\'>*</color>";
            this.ItemForVoucherNumber.TextSize = new System.Drawing.Size(80, 13);
            // 
            // ItemForStocktakingType
            // 
            this.ItemForStocktakingType.AllowHtmlStringInCaption = true;
            this.ItemForStocktakingType.Control = this.StocktakingTypeComboBoxEdit;
            this.ItemForStocktakingType.Location = new System.Drawing.Point(0, 48);
            this.ItemForStocktakingType.Name = "ItemForStocktakingType";
            this.ItemForStocktakingType.OptionsToolTip.AllowHtmlString = DevExpress.Utils.DefaultBoolean.True;
            this.ItemForStocktakingType.OptionsToolTip.IconAllowHtmlString = DevExpress.Utils.DefaultBoolean.True;
            this.ItemForStocktakingType.Size = new System.Drawing.Size(622, 24);
            this.ItemForStocktakingType.Text = "Loại kiểm kho <color=\'red\'>*</color>";
            this.ItemForStocktakingType.TextSize = new System.Drawing.Size(80, 13);
            // 
            // ItemForStocktakingStatus
            // 
            this.ItemForStocktakingStatus.AllowHtmlStringInCaption = true;
            this.ItemForStocktakingStatus.Control = this.StocktakingStatusComboBoxEdit;
            this.ItemForStocktakingStatus.Location = new System.Drawing.Point(0, 72);
            this.ItemForStocktakingStatus.Name = "ItemForStocktakingStatus";
            this.ItemForStocktakingStatus.OptionsToolTip.AllowHtmlString = DevExpress.Utils.DefaultBoolean.True;
            this.ItemForStocktakingStatus.OptionsToolTip.IconAllowHtmlString = DevExpress.Utils.DefaultBoolean.True;
            this.ItemForStocktakingStatus.Size = new System.Drawing.Size(622, 24);
            this.ItemForStocktakingStatus.Text = "Trạng thái <color=\'red\'>*</color>";
            this.ItemForStocktakingStatus.TextSize = new System.Drawing.Size(80, 13);
            // 
            // ItemForWarehouseName
            // 
            this.ItemForWarehouseName.AllowHtmlStringInCaption = true;
            this.ItemForWarehouseName.Control = this.WarehouseNameSearchLookupEdit;
            this.ItemForWarehouseName.Location = new System.Drawing.Point(0, 96);
            this.ItemForWarehouseName.Name = "ItemForWarehouseName";
            this.ItemForWarehouseName.OptionsToolTip.AllowHtmlString = DevExpress.Utils.DefaultBoolean.True;
            this.ItemForWarehouseName.OptionsToolTip.IconAllowHtmlString = DevExpress.Utils.DefaultBoolean.True;
            this.ItemForWarehouseName.Size = new System.Drawing.Size(622, 24);
            this.ItemForWarehouseName.Text = "Tên kho <color=\'red\'>*</color>";
            this.ItemForWarehouseName.TextSize = new System.Drawing.Size(80, 13);
            // 
            // ItemForIsLocked
            // 
            this.ItemForIsLocked.AllowHtmlStringInCaption = true;
            this.ItemForIsLocked.Control = this.IsLockedToggleSwitch;
            this.ItemForIsLocked.Location = new System.Drawing.Point(311, 24);
            this.ItemForIsLocked.Name = "ItemForIsLocked";
            this.ItemForIsLocked.OptionsToolTip.AllowHtmlString = DevExpress.Utils.DefaultBoolean.True;
            this.ItemForIsLocked.OptionsToolTip.IconAllowHtmlString = DevExpress.Utils.DefaultBoolean.True;
            this.ItemForIsLocked.Size = new System.Drawing.Size(311, 24);
            this.ItemForIsLocked.Text = "Đã khóa";
            this.ItemForIsLocked.TextSize = new System.Drawing.Size(80, 13);
            // 
            // layoutControlGroup3
            // 
            this.layoutControlGroup3.ExpandButtonVisible = true;
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
            // ItemForStartDate
            // 
            this.ItemForStartDate.AllowHtmlStringInCaption = true;
            this.ItemForStartDate.Control = this.StartDateDateEdit;
            this.ItemForStartDate.Location = new System.Drawing.Point(0, 0);
            this.ItemForStartDate.Name = "ItemForStartDate";
            this.ItemForStartDate.OptionsToolTip.AllowHtmlString = DevExpress.Utils.DefaultBoolean.True;
            this.ItemForStartDate.OptionsToolTip.IconAllowHtmlString = DevExpress.Utils.DefaultBoolean.True;
            this.ItemForStartDate.Size = new System.Drawing.Size(622, 24);
            this.ItemForStartDate.Text = "Ngày bắt đầu";
            this.ItemForStartDate.TextSize = new System.Drawing.Size(80, 13);
            // 
            // ItemForEndDate
            // 
            this.ItemForEndDate.AllowHtmlStringInCaption = true;
            this.ItemForEndDate.Control = this.EndDateDateEdit;
            this.ItemForEndDate.Location = new System.Drawing.Point(0, 24);
            this.ItemForEndDate.Name = "ItemForEndDate";
            this.ItemForEndDate.OptionsToolTip.AllowHtmlString = DevExpress.Utils.DefaultBoolean.True;
            this.ItemForEndDate.OptionsToolTip.IconAllowHtmlString = DevExpress.Utils.DefaultBoolean.True;
            this.ItemForEndDate.Size = new System.Drawing.Size(622, 24);
            this.ItemForEndDate.Text = "Ngày kết thúc";
            this.ItemForEndDate.TextSize = new System.Drawing.Size(80, 13);
            // 
            // ItemForCountedDate
            // 
            this.ItemForCountedDate.AllowHtmlStringInCaption = true;
            this.ItemForCountedDate.Control = this.CountedDateDateEdit;
            this.ItemForCountedDate.Location = new System.Drawing.Point(0, 48);
            this.ItemForCountedDate.Name = "ItemForCountedDate";
            this.ItemForCountedDate.OptionsToolTip.AllowHtmlString = DevExpress.Utils.DefaultBoolean.True;
            this.ItemForCountedDate.OptionsToolTip.IconAllowHtmlString = DevExpress.Utils.DefaultBoolean.True;
            this.ItemForCountedDate.Size = new System.Drawing.Size(622, 24);
            this.ItemForCountedDate.Text = "Ngày kiểm đếm";
            this.ItemForCountedDate.TextSize = new System.Drawing.Size(80, 13);
            // 
            // ItemForReviewedDate
            // 
            this.ItemForReviewedDate.AllowHtmlStringInCaption = true;
            this.ItemForReviewedDate.Control = this.ReviewedDateDateEdit;
            this.ItemForReviewedDate.Location = new System.Drawing.Point(0, 72);
            this.ItemForReviewedDate.Name = "ItemForReviewedDate";
            this.ItemForReviewedDate.OptionsToolTip.AllowHtmlString = DevExpress.Utils.DefaultBoolean.True;
            this.ItemForReviewedDate.OptionsToolTip.IconAllowHtmlString = DevExpress.Utils.DefaultBoolean.True;
            this.ItemForReviewedDate.Size = new System.Drawing.Size(622, 24);
            this.ItemForReviewedDate.Text = "Ngày rà soát";
            this.ItemForReviewedDate.TextSize = new System.Drawing.Size(80, 13);
            // 
            // ItemForApprovedDate
            // 
            this.ItemForApprovedDate.AllowHtmlStringInCaption = true;
            this.ItemForApprovedDate.Control = this.ApprovedDateDateEdit;
            this.ItemForApprovedDate.Location = new System.Drawing.Point(0, 96);
            this.ItemForApprovedDate.Name = "ItemForApprovedDate";
            this.ItemForApprovedDate.OptionsToolTip.AllowHtmlString = DevExpress.Utils.DefaultBoolean.True;
            this.ItemForApprovedDate.OptionsToolTip.IconAllowHtmlString = DevExpress.Utils.DefaultBoolean.True;
            this.ItemForApprovedDate.Size = new System.Drawing.Size(622, 24);
            this.ItemForApprovedDate.Text = "Ngày phê duyệt";
            this.ItemForApprovedDate.TextSize = new System.Drawing.Size(80, 13);
            // 
            // layoutControlGroup4
            // 
            this.layoutControlGroup4.ExpandButtonVisible = true;
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
            // ItemForCountedBy
            // 
            this.ItemForCountedBy.AllowHtmlStringInCaption = true;
            this.ItemForCountedBy.Control = this.CountedByTextEdit;
            this.ItemForCountedBy.Location = new System.Drawing.Point(0, 0);
            this.ItemForCountedBy.Name = "ItemForCountedBy";
            this.ItemForCountedBy.OptionsToolTip.AllowHtmlString = DevExpress.Utils.DefaultBoolean.True;
            this.ItemForCountedBy.OptionsToolTip.IconAllowHtmlString = DevExpress.Utils.DefaultBoolean.True;
            this.ItemForCountedBy.Size = new System.Drawing.Size(622, 24);
            this.ItemForCountedBy.Text = "Người kiểm đếm";
            this.ItemForCountedBy.TextSize = new System.Drawing.Size(80, 13);
            // 
            // ItemForReviewedBy
            // 
            this.ItemForReviewedBy.AllowHtmlStringInCaption = true;
            this.ItemForReviewedBy.Control = this.ReviewedByTextEdit;
            this.ItemForReviewedBy.Location = new System.Drawing.Point(0, 24);
            this.ItemForReviewedBy.Name = "ItemForReviewedBy";
            this.ItemForReviewedBy.OptionsToolTip.AllowHtmlString = DevExpress.Utils.DefaultBoolean.True;
            this.ItemForReviewedBy.OptionsToolTip.IconAllowHtmlString = DevExpress.Utils.DefaultBoolean.True;
            this.ItemForReviewedBy.Size = new System.Drawing.Size(622, 24);
            this.ItemForReviewedBy.Text = "Người rà soát";
            this.ItemForReviewedBy.TextSize = new System.Drawing.Size(80, 13);
            // 
            // ItemForApprovedBy
            // 
            this.ItemForApprovedBy.AllowHtmlStringInCaption = true;
            this.ItemForApprovedBy.Control = this.ApprovedByTextEdit;
            this.ItemForApprovedBy.Location = new System.Drawing.Point(0, 48);
            this.ItemForApprovedBy.Name = "ItemForApprovedBy";
            this.ItemForApprovedBy.OptionsToolTip.AllowHtmlString = DevExpress.Utils.DefaultBoolean.True;
            this.ItemForApprovedBy.OptionsToolTip.IconAllowHtmlString = DevExpress.Utils.DefaultBoolean.True;
            this.ItemForApprovedBy.Size = new System.Drawing.Size(622, 24);
            this.ItemForApprovedBy.Text = "Người phê duyệt";
            this.ItemForApprovedBy.TextSize = new System.Drawing.Size(80, 13);
            // 
            // layoutControlGroup5
            // 
            this.layoutControlGroup5.ExpandButtonVisible = true;
            this.layoutControlGroup5.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.layoutControlGroup5.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.ItemForNotes,
            this.ItemForReason,
            this.ItemForLockedBy,
            this.ItemForLockedDate});
            this.layoutControlGroup5.Location = new System.Drawing.Point(0, 447);
            this.layoutControlGroup5.Name = "layoutControlGroup5";
            this.layoutControlGroup5.Size = new System.Drawing.Size(646, 538);
            this.layoutControlGroup5.Text = "Khác";
            // 
            // ItemForNotes
            // 
            this.ItemForNotes.AllowHtmlStringInCaption = true;
            this.ItemForNotes.Control = this.NotesTextEdit;
            this.ItemForNotes.Location = new System.Drawing.Point(0, 48);
            this.ItemForNotes.Name = "ItemForNotes";
            this.ItemForNotes.OptionsToolTip.AllowHtmlString = DevExpress.Utils.DefaultBoolean.True;
            this.ItemForNotes.OptionsToolTip.IconAllowHtmlString = DevExpress.Utils.DefaultBoolean.True;
            this.ItemForNotes.Size = new System.Drawing.Size(311, 445);
            this.ItemForNotes.Text = "Ghi chú";
            this.ItemForNotes.TextLocation = DevExpress.Utils.Locations.Top;
            this.ItemForNotes.TextSize = new System.Drawing.Size(80, 13);
            // 
            // ItemForReason
            // 
            this.ItemForReason.AllowHtmlStringInCaption = true;
            this.ItemForReason.Control = this.ReasonTextEdit;
            this.ItemForReason.Location = new System.Drawing.Point(311, 48);
            this.ItemForReason.Name = "ItemForReason";
            this.ItemForReason.OptionsToolTip.AllowHtmlString = DevExpress.Utils.DefaultBoolean.True;
            this.ItemForReason.OptionsToolTip.IconAllowHtmlString = DevExpress.Utils.DefaultBoolean.True;
            this.ItemForReason.Size = new System.Drawing.Size(311, 445);
            this.ItemForReason.Text = "Lý do";
            this.ItemForReason.TextLocation = DevExpress.Utils.Locations.Top;
            this.ItemForReason.TextSize = new System.Drawing.Size(80, 13);
            // 
            // ItemForLockedBy
            // 
            this.ItemForLockedBy.AllowHtmlStringInCaption = true;
            this.ItemForLockedBy.Control = this.LockedByTextEdit;
            this.ItemForLockedBy.Location = new System.Drawing.Point(0, 0);
            this.ItemForLockedBy.Name = "ItemForLockedBy";
            this.ItemForLockedBy.OptionsToolTip.AllowHtmlString = DevExpress.Utils.DefaultBoolean.True;
            this.ItemForLockedBy.OptionsToolTip.IconAllowHtmlString = DevExpress.Utils.DefaultBoolean.True;
            this.ItemForLockedBy.Size = new System.Drawing.Size(622, 24);
            this.ItemForLockedBy.Text = "Người khóa";
            this.ItemForLockedBy.TextSize = new System.Drawing.Size(80, 13);
            // 
            // ItemForLockedDate
            // 
            this.ItemForLockedDate.AllowHtmlStringInCaption = true;
            this.ItemForLockedDate.Control = this.LockedDateDateEdit;
            this.ItemForLockedDate.Location = new System.Drawing.Point(0, 24);
            this.ItemForLockedDate.Name = "ItemForLockedDate";
            this.ItemForLockedDate.OptionsToolTip.AllowHtmlString = DevExpress.Utils.DefaultBoolean.True;
            this.ItemForLockedDate.OptionsToolTip.IconAllowHtmlString = DevExpress.Utils.DefaultBoolean.True;
            this.ItemForLockedDate.Size = new System.Drawing.Size(622, 24);
            this.ItemForLockedDate.Text = "Ngày khóa";
            this.ItemForLockedDate.TextSize = new System.Drawing.Size(80, 13);
            // 
            // FrmFrmStocktakingMasterAddEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(666, 1029);
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
            ((System.ComponentModel.ISupportInitialize)(this.StocktakingDateDateEdit.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StocktakingDateDateEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VoucherNumberTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartDateDateEdit.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartDateDateEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EndDateDateEdit.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EndDateDateEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CountedByTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CountedDateDateEdit.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CountedDateDateEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReviewedByTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReviewedDateDateEdit.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReviewedDateDateEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ApprovedByTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ApprovedDateDateEdit.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ApprovedDateDateEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LockedDateDateEdit.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LockedDateDateEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LockedByTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StocktakingTypeComboBoxEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WarehouseNameSearchLookupEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.companyBranchDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CompanyBranchSearchLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CompanyBrachHypertextLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.IsLockedToggleSwitch.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NotesTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReasonTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StocktakingStatusComboBoxEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForStocktakingDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForVoucherNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForStocktakingType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForStocktakingStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForWarehouseName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForIsLocked)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForStartDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForEndDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForCountedDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForReviewedDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForApprovedDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForCountedBy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForReviewedBy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForApprovedBy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForNotes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForReason)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForLockedBy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForLockedDate)).EndInit();
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
        private ComboBoxEdit StocktakingTypeComboBoxEdit;
        private SearchLookUpEdit WarehouseNameSearchLookupEdit;
        private DevExpress.XtraGrid.Views.Grid.GridView CompanyBranchSearchLookUpEdit1View;
        private LayoutControlGroup layoutControlGroup2;
        private LayoutControlGroup layoutControlGroup3;
        private ToggleSwitch IsLockedToggleSwitch;
        private MemoEdit NotesTextEdit;
        private MemoEdit ReasonTextEdit;
        private LayoutControlGroup layoutControlGroup4;
        private LayoutControlGroup layoutControlGroup5;
        private ComboBoxEdit StocktakingStatusComboBoxEdit;
        private System.Windows.Forms.BindingSource companyBranchDtoBindingSource;
        private DevExpress.XtraGrid.Columns.GridColumn colThongTinHtml;
        private DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel CompanyBrachHypertextLabel;
    }
}