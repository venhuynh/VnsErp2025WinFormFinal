using DevExpress.XtraBars;
using DevExpress.XtraDataLayout;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraLayout;

namespace Inventory.StockTakking
{
    partial class FrmStocktakingDetailAddEdit
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
            this.barHeaderItem1 = new DevExpress.XtraBars.BarHeaderItem();
            this.SoLuongNhapXuatBarStaticItem = new DevExpress.XtraBars.BarStaticItem();
            this.dxErrorProvider1 = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(this.components);
            this.colParentDepartmentName = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colDepartmentName = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.productVariantSimpleDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataLayoutControl1 = new DevExpress.XtraDataLayout.DataLayoutControl();
            this.SystemQuantityMemoEdit = new DevExpress.XtraEditors.MemoEdit();
            this.CountedQuantityMemoEdit = new DevExpress.XtraEditors.MemoEdit();
            this.DifferenceQuantityMemoEdit = new DevExpress.XtraEditors.MemoEdit();
            this.SystemValueMemoEdit = new DevExpress.XtraEditors.MemoEdit();
            this.CountedValueMemoEdit = new DevExpress.XtraEditors.MemoEdit();
            this.DifferenceValueMemoEdit = new DevExpress.XtraEditors.MemoEdit();
            this.UnitPriceMemoEdit = new DevExpress.XtraEditors.MemoEdit();
            this.AdjustmentTypeImageComboBoxEdit = new DevExpress.XtraEditors.ImageComboBoxEdit();
            this.CountedByMemoEdit = new DevExpress.XtraEditors.MemoEdit();
            this.CountedDateDateEdit = new DevExpress.XtraEditors.DateEdit();
            this.IsReviewedCheckEdit = new DevExpress.XtraEditors.CheckEdit();
            this.ReviewedByMemoEdit = new DevExpress.XtraEditors.MemoEdit();
            this.ReviewedDateDateEdit = new DevExpress.XtraEditors.DateEdit();
            this.IsApprovedCheckEdit = new DevExpress.XtraEditors.CheckEdit();
            this.ApprovedByMemoEdit = new DevExpress.XtraEditors.MemoEdit();
            this.ApprovedDateDateEdit = new DevExpress.XtraEditors.DateEdit();
            this.ProductVariantNameSearchLookupEdit = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.searchLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.IsCountedToggleSwitch = new DevExpress.XtraEditors.ToggleSwitch();
            this.NotesMemoEdit = new DevExpress.XtraEditors.MemoEdit();
            this.ReviewNotesMemoEdit = new DevExpress.XtraEditors.MemoEdit();
            this.AdjustmentReasonMemoEdit = new DevExpress.XtraEditors.MemoEdit();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.ItemForProductVariantName = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForIsCounted = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlGroup3 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.ItemForSystemQuantity = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForCountedQuantity = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForDifferenceQuantity = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlGroup4 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.ItemForSystemValue = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForCountedValue = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForDifferenceValue = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForUnitPrice = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlGroup5 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.ItemForAdjustmentType = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForAdjustmentReason = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlGroup6 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.ItemForCountedBy = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForCountedDate = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlGroup7 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.ItemForReviewedBy = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForIsReviewed = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForReviewedDate = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForReviewNotes = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlGroup8 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.ItemForIsApproved = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForApprovedBy = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForApprovedDate = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForNotes = new DevExpress.XtraLayout.LayoutControlItem();
            this.colVariantFullName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.VariantFullNameHypertextLabel = new DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.productVariantSimpleDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).BeginInit();
            this.dataLayoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SystemQuantityMemoEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CountedQuantityMemoEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DifferenceQuantityMemoEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SystemValueMemoEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CountedValueMemoEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DifferenceValueMemoEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UnitPriceMemoEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AdjustmentTypeImageComboBoxEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CountedByMemoEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CountedDateDateEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CountedDateDateEdit.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.IsReviewedCheckEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReviewedByMemoEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReviewedDateDateEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReviewedDateDateEdit.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.IsApprovedCheckEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ApprovedByMemoEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ApprovedDateDateEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ApprovedDateDateEdit.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductVariantNameSearchLookupEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.IsCountedToggleSwitch.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NotesMemoEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReviewNotesMemoEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AdjustmentReasonMemoEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForProductVariantName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForIsCounted)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForSystemQuantity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForCountedQuantity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForDifferenceQuantity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForSystemValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForCountedValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForDifferenceValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForUnitPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForAdjustmentType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForAdjustmentReason)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForCountedBy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForCountedDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForReviewedBy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForIsReviewed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForReviewedDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForReviewNotes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForIsApproved)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForApprovedBy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForApprovedDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForNotes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VariantFullNameHypertextLabel)).BeginInit();
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
            this.barHeaderItem1,
            this.SoLuongNhapXuatBarStaticItem});
            this.barManager1.MainMenu = this.bar2;
            this.barManager1.MaxItemId = 8;
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
            this.barDockControlTop.Size = new System.Drawing.Size(762, 24);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 901);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(762, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 24);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 877);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(762, 24);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 877);
            // 
            // barHeaderItem1
            // 
            this.barHeaderItem1.Caption = "Tổng kết";
            this.barHeaderItem1.Id = 5;
            this.barHeaderItem1.Name = "barHeaderItem1";
            // 
            // SoLuongNhapXuatBarStaticItem
            // 
            this.SoLuongNhapXuatBarStaticItem.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True;
            this.SoLuongNhapXuatBarStaticItem.Caption = "Số lượng nhập xuất";
            this.SoLuongNhapXuatBarStaticItem.Id = 6;
            this.SoLuongNhapXuatBarStaticItem.Name = "SoLuongNhapXuatBarStaticItem";
            // 
            // dxErrorProvider1
            // 
            this.dxErrorProvider1.ContainerControl = this;
            // 
            // colParentDepartmentName
            // 
            this.colParentDepartmentName.Caption = "Phòng ban cấp trên";
            this.colParentDepartmentName.FieldName = "ParentDepartmentName";
            this.colParentDepartmentName.Name = "colParentDepartmentName";
            this.colParentDepartmentName.Visible = true;
            this.colParentDepartmentName.VisibleIndex = 0;
            // 
            // colDepartmentName
            // 
            this.colDepartmentName.Caption = "Tên phòng ban";
            this.colDepartmentName.FieldName = "DepartmentName";
            this.colDepartmentName.Name = "colDepartmentName";
            this.colDepartmentName.Visible = true;
            this.colDepartmentName.VisibleIndex = 1;
            // 
            // productVariantSimpleDtoBindingSource
            // 
            this.productVariantSimpleDtoBindingSource.DataSource = typeof(DTO.MasterData.ProductService.ProductVariantSimpleDto);
            // 
            // dataLayoutControl1
            // 
            this.dataLayoutControl1.Controls.Add(this.SystemQuantityMemoEdit);
            this.dataLayoutControl1.Controls.Add(this.CountedQuantityMemoEdit);
            this.dataLayoutControl1.Controls.Add(this.DifferenceQuantityMemoEdit);
            this.dataLayoutControl1.Controls.Add(this.SystemValueMemoEdit);
            this.dataLayoutControl1.Controls.Add(this.CountedValueMemoEdit);
            this.dataLayoutControl1.Controls.Add(this.DifferenceValueMemoEdit);
            this.dataLayoutControl1.Controls.Add(this.UnitPriceMemoEdit);
            this.dataLayoutControl1.Controls.Add(this.AdjustmentTypeImageComboBoxEdit);
            this.dataLayoutControl1.Controls.Add(this.CountedByMemoEdit);
            this.dataLayoutControl1.Controls.Add(this.CountedDateDateEdit);
            this.dataLayoutControl1.Controls.Add(this.IsReviewedCheckEdit);
            this.dataLayoutControl1.Controls.Add(this.ReviewedByMemoEdit);
            this.dataLayoutControl1.Controls.Add(this.ReviewedDateDateEdit);
            this.dataLayoutControl1.Controls.Add(this.IsApprovedCheckEdit);
            this.dataLayoutControl1.Controls.Add(this.ApprovedByMemoEdit);
            this.dataLayoutControl1.Controls.Add(this.ApprovedDateDateEdit);
            this.dataLayoutControl1.Controls.Add(this.ProductVariantNameSearchLookupEdit);
            this.dataLayoutControl1.Controls.Add(this.IsCountedToggleSwitch);
            this.dataLayoutControl1.Controls.Add(this.NotesMemoEdit);
            this.dataLayoutControl1.Controls.Add(this.ReviewNotesMemoEdit);
            this.dataLayoutControl1.Controls.Add(this.AdjustmentReasonMemoEdit);
            this.dataLayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataLayoutControl1.Location = new System.Drawing.Point(0, 24);
            this.dataLayoutControl1.Name = "dataLayoutControl1";
            this.dataLayoutControl1.Root = this.Root;
            this.dataLayoutControl1.Size = new System.Drawing.Size(762, 877);
            this.dataLayoutControl1.TabIndex = 19;
            this.dataLayoutControl1.Text = "dataLayoutControl1";
            // 
            // SystemQuantityMemoEdit
            // 
            this.SystemQuantityMemoEdit.Location = new System.Drawing.Point(116, 136);
            this.SystemQuantityMemoEdit.MenuManager = this.barManager1;
            this.SystemQuantityMemoEdit.Name = "SystemQuantityMemoEdit";
            this.SystemQuantityMemoEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.SystemQuantityMemoEdit.Properties.Appearance.Options.UseTextOptions = true;
            this.SystemQuantityMemoEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.SystemQuantityMemoEdit.Size = new System.Drawing.Size(121, 23);
            this.SystemQuantityMemoEdit.StyleController = this.dataLayoutControl1;
            this.SystemQuantityMemoEdit.TabIndex = 10;
            // 
            // CountedQuantityMemoEdit
            // 
            this.CountedQuantityMemoEdit.Location = new System.Drawing.Point(333, 136);
            this.CountedQuantityMemoEdit.MenuManager = this.barManager1;
            this.CountedQuantityMemoEdit.Name = "CountedQuantityMemoEdit";
            this.CountedQuantityMemoEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.CountedQuantityMemoEdit.Properties.Appearance.Options.UseTextOptions = true;
            this.CountedQuantityMemoEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.CountedQuantityMemoEdit.Size = new System.Drawing.Size(84, 23);
            this.CountedQuantityMemoEdit.StyleController = this.dataLayoutControl1;
            this.CountedQuantityMemoEdit.TabIndex = 11;
            // 
            // DifferenceQuantityMemoEdit
            // 
            this.DifferenceQuantityMemoEdit.Location = new System.Drawing.Point(513, 136);
            this.DifferenceQuantityMemoEdit.MenuManager = this.barManager1;
            this.DifferenceQuantityMemoEdit.Name = "DifferenceQuantityMemoEdit";
            this.DifferenceQuantityMemoEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.DifferenceQuantityMemoEdit.Properties.Appearance.Options.UseTextOptions = true;
            this.DifferenceQuantityMemoEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.DifferenceQuantityMemoEdit.Size = new System.Drawing.Size(225, 23);
            this.DifferenceQuantityMemoEdit.StyleController = this.dataLayoutControl1;
            this.DifferenceQuantityMemoEdit.TabIndex = 12;
            // 
            // SystemValueMemoEdit
            // 
            this.SystemValueMemoEdit.Location = new System.Drawing.Point(295, 208);
            this.SystemValueMemoEdit.MenuManager = this.barManager1;
            this.SystemValueMemoEdit.Name = "SystemValueMemoEdit";
            this.SystemValueMemoEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.SystemValueMemoEdit.Properties.Appearance.Options.UseTextOptions = true;
            this.SystemValueMemoEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.SystemValueMemoEdit.Size = new System.Drawing.Size(84, 23);
            this.SystemValueMemoEdit.StyleController = this.dataLayoutControl1;
            this.SystemValueMemoEdit.TabIndex = 13;
            // 
            // CountedValueMemoEdit
            // 
            this.CountedValueMemoEdit.Location = new System.Drawing.Point(475, 208);
            this.CountedValueMemoEdit.MenuManager = this.barManager1;
            this.CountedValueMemoEdit.Name = "CountedValueMemoEdit";
            this.CountedValueMemoEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.CountedValueMemoEdit.Properties.Appearance.Options.UseTextOptions = true;
            this.CountedValueMemoEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.CountedValueMemoEdit.Size = new System.Drawing.Size(83, 23);
            this.CountedValueMemoEdit.StyleController = this.dataLayoutControl1;
            this.CountedValueMemoEdit.TabIndex = 14;
            // 
            // DifferenceValueMemoEdit
            // 
            this.DifferenceValueMemoEdit.Location = new System.Drawing.Point(654, 208);
            this.DifferenceValueMemoEdit.MenuManager = this.barManager1;
            this.DifferenceValueMemoEdit.Name = "DifferenceValueMemoEdit";
            this.DifferenceValueMemoEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.DifferenceValueMemoEdit.Properties.Appearance.Options.UseTextOptions = true;
            this.DifferenceValueMemoEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.DifferenceValueMemoEdit.Size = new System.Drawing.Size(84, 23);
            this.DifferenceValueMemoEdit.StyleController = this.dataLayoutControl1;
            this.DifferenceValueMemoEdit.TabIndex = 15;
            // 
            // UnitPriceMemoEdit
            // 
            this.UnitPriceMemoEdit.Location = new System.Drawing.Point(116, 208);
            this.UnitPriceMemoEdit.MenuManager = this.barManager1;
            this.UnitPriceMemoEdit.Name = "UnitPriceMemoEdit";
            this.UnitPriceMemoEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.UnitPriceMemoEdit.Properties.Appearance.Options.UseTextOptions = true;
            this.UnitPriceMemoEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.UnitPriceMemoEdit.Size = new System.Drawing.Size(83, 23);
            this.UnitPriceMemoEdit.StyleController = this.dataLayoutControl1;
            this.UnitPriceMemoEdit.TabIndex = 16;
            // 
            // AdjustmentTypeImageComboBoxEdit
            // 
            this.AdjustmentTypeImageComboBoxEdit.Location = new System.Drawing.Point(116, 280);
            this.AdjustmentTypeImageComboBoxEdit.MenuManager = this.barManager1;
            this.AdjustmentTypeImageComboBoxEdit.Name = "AdjustmentTypeImageComboBoxEdit";
            this.AdjustmentTypeImageComboBoxEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.AdjustmentTypeImageComboBoxEdit.Properties.Appearance.Options.UseTextOptions = true;
            this.AdjustmentTypeImageComboBoxEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.AdjustmentTypeImageComboBoxEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.AdjustmentTypeImageComboBoxEdit.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.ImageComboBoxItem[] {
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("Tăng", DTO.Inventory.StockTakking.AdjustmentTypeEnum.Increase, 0),
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("Giảm", DTO.Inventory.StockTakking.AdjustmentTypeEnum.Decrease, 1),
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("Không điều chỉnh", DTO.Inventory.StockTakking.AdjustmentTypeEnum.NoAdjustment, 2)});
            this.AdjustmentTypeImageComboBoxEdit.Properties.UseCtrlScroll = true;
            this.AdjustmentTypeImageComboBoxEdit.Size = new System.Drawing.Size(622, 20);
            this.AdjustmentTypeImageComboBoxEdit.StyleController = this.dataLayoutControl1;
            this.AdjustmentTypeImageComboBoxEdit.TabIndex = 17;
            // 
            // CountedByMemoEdit
            // 
            this.CountedByMemoEdit.Location = new System.Drawing.Point(475, 430);
            this.CountedByMemoEdit.MenuManager = this.barManager1;
            this.CountedByMemoEdit.Name = "CountedByMemoEdit";
            this.CountedByMemoEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.CountedByMemoEdit.Size = new System.Drawing.Size(263, 20);
            this.CountedByMemoEdit.StyleController = this.dataLayoutControl1;
            this.CountedByMemoEdit.TabIndex = 20;
            // 
            // CountedDateDateEdit
            // 
            this.CountedDateDateEdit.EditValue = null;
            this.CountedDateDateEdit.Location = new System.Drawing.Point(116, 430);
            this.CountedDateDateEdit.MenuManager = this.barManager1;
            this.CountedDateDateEdit.Name = "CountedDateDateEdit";
            this.CountedDateDateEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.CountedDateDateEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CountedDateDateEdit.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CountedDateDateEdit.Size = new System.Drawing.Size(263, 20);
            this.CountedDateDateEdit.StyleController = this.dataLayoutControl1;
            this.CountedDateDateEdit.TabIndex = 21;
            // 
            // IsReviewedCheckEdit
            // 
            this.IsReviewedCheckEdit.Location = new System.Drawing.Point(24, 499);
            this.IsReviewedCheckEdit.MenuManager = this.barManager1;
            this.IsReviewedCheckEdit.Name = "IsReviewedCheckEdit";
            this.IsReviewedCheckEdit.Properties.Caption = "Đã rà soát";
            this.IsReviewedCheckEdit.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Default;
            this.IsReviewedCheckEdit.Size = new System.Drawing.Size(714, 20);
            this.IsReviewedCheckEdit.StyleController = this.dataLayoutControl1;
            this.IsReviewedCheckEdit.TabIndex = 22;
            // 
            // ReviewedByMemoEdit
            // 
            this.ReviewedByMemoEdit.Location = new System.Drawing.Point(116, 523);
            this.ReviewedByMemoEdit.MenuManager = this.barManager1;
            this.ReviewedByMemoEdit.Name = "ReviewedByMemoEdit";
            this.ReviewedByMemoEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.ReviewedByMemoEdit.Size = new System.Drawing.Size(622, 22);
            this.ReviewedByMemoEdit.StyleController = this.dataLayoutControl1;
            this.ReviewedByMemoEdit.TabIndex = 23;
            // 
            // ReviewedDateDateEdit
            // 
            this.ReviewedDateDateEdit.EditValue = null;
            this.ReviewedDateDateEdit.Location = new System.Drawing.Point(116, 549);
            this.ReviewedDateDateEdit.MenuManager = this.barManager1;
            this.ReviewedDateDateEdit.Name = "ReviewedDateDateEdit";
            this.ReviewedDateDateEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.ReviewedDateDateEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ReviewedDateDateEdit.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ReviewedDateDateEdit.Size = new System.Drawing.Size(622, 20);
            this.ReviewedDateDateEdit.StyleController = this.dataLayoutControl1;
            this.ReviewedDateDateEdit.TabIndex = 24;
            // 
            // IsApprovedCheckEdit
            // 
            this.IsApprovedCheckEdit.Location = new System.Drawing.Point(24, 700);
            this.IsApprovedCheckEdit.MenuManager = this.barManager1;
            this.IsApprovedCheckEdit.Name = "IsApprovedCheckEdit";
            this.IsApprovedCheckEdit.Properties.Caption = "Đã phê duyệt";
            this.IsApprovedCheckEdit.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Default;
            this.IsApprovedCheckEdit.Size = new System.Drawing.Size(714, 20);
            this.IsApprovedCheckEdit.StyleController = this.dataLayoutControl1;
            this.IsApprovedCheckEdit.TabIndex = 26;
            // 
            // ApprovedByMemoEdit
            // 
            this.ApprovedByMemoEdit.Location = new System.Drawing.Point(116, 724);
            this.ApprovedByMemoEdit.MenuManager = this.barManager1;
            this.ApprovedByMemoEdit.Name = "ApprovedByMemoEdit";
            this.ApprovedByMemoEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.ApprovedByMemoEdit.Size = new System.Drawing.Size(622, 22);
            this.ApprovedByMemoEdit.StyleController = this.dataLayoutControl1;
            this.ApprovedByMemoEdit.TabIndex = 27;
            // 
            // ApprovedDateDateEdit
            // 
            this.ApprovedDateDateEdit.EditValue = null;
            this.ApprovedDateDateEdit.Location = new System.Drawing.Point(116, 750);
            this.ApprovedDateDateEdit.MenuManager = this.barManager1;
            this.ApprovedDateDateEdit.Name = "ApprovedDateDateEdit";
            this.ApprovedDateDateEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.ApprovedDateDateEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ApprovedDateDateEdit.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ApprovedDateDateEdit.Size = new System.Drawing.Size(622, 20);
            this.ApprovedDateDateEdit.StyleController = this.dataLayoutControl1;
            this.ApprovedDateDateEdit.TabIndex = 28;
            // 
            // ProductVariantNameSearchLookupEdit
            // 
            this.ProductVariantNameSearchLookupEdit.Location = new System.Drawing.Point(116, 45);
            this.ProductVariantNameSearchLookupEdit.MenuManager = this.barManager1;
            this.ProductVariantNameSearchLookupEdit.Name = "ProductVariantNameSearchLookupEdit";
            this.ProductVariantNameSearchLookupEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ProductVariantNameSearchLookupEdit.Properties.DataSource = this.productVariantSimpleDtoBindingSource;
            this.ProductVariantNameSearchLookupEdit.Properties.DisplayMember = "ProductName";
            this.ProductVariantNameSearchLookupEdit.Properties.NullText = "";
            this.ProductVariantNameSearchLookupEdit.Properties.PopupView = this.searchLookUpEdit1View;
            this.ProductVariantNameSearchLookupEdit.Properties.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.VariantFullNameHypertextLabel});
            this.ProductVariantNameSearchLookupEdit.Properties.ValueMember = "Id";
            this.ProductVariantNameSearchLookupEdit.Size = new System.Drawing.Size(622, 20);
            this.ProductVariantNameSearchLookupEdit.StyleController = this.dataLayoutControl1;
            this.ProductVariantNameSearchLookupEdit.TabIndex = 7;
            // 
            // searchLookUpEdit1View
            // 
            this.searchLookUpEdit1View.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colVariantFullName});
            this.searchLookUpEdit1View.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.searchLookUpEdit1View.Appearance.HeaderPanel.Options.UseFont = true;
            this.searchLookUpEdit1View.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.searchLookUpEdit1View.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.searchLookUpEdit1View.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.searchLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.searchLookUpEdit1View.Name = "searchLookUpEdit1View";
            this.searchLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.searchLookUpEdit1View.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            this.searchLookUpEdit1View.OptionsView.ShowIndicator = false;
            this.searchLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // IsCountedToggleSwitch
            // 
            this.IsCountedToggleSwitch.AllowHtmlTextInToolTip = DevExpress.Utils.DefaultBoolean.True;
            this.IsCountedToggleSwitch.EditValue = true;
            this.IsCountedToggleSwitch.Location = new System.Drawing.Point(116, 69);
            this.IsCountedToggleSwitch.MenuManager = this.barManager1;
            this.IsCountedToggleSwitch.Name = "IsCountedToggleSwitch";
            this.IsCountedToggleSwitch.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.IsCountedToggleSwitch.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Default;
            this.IsCountedToggleSwitch.Properties.OffText = "<color=\'red\'>Chưa kiểm đếm</color>";
            this.IsCountedToggleSwitch.Properties.OnText = "<color=\'blue\'>Đã kiểm đếm</color>";
            this.IsCountedToggleSwitch.Size = new System.Drawing.Size(622, 18);
            this.IsCountedToggleSwitch.StyleController = this.dataLayoutControl1;
            this.IsCountedToggleSwitch.TabIndex = 19;
            // 
            // NotesMemoEdit
            // 
            this.NotesMemoEdit.Location = new System.Drawing.Point(116, 774);
            this.NotesMemoEdit.MenuManager = this.barManager1;
            this.NotesMemoEdit.Name = "NotesMemoEdit";
            this.NotesMemoEdit.Size = new System.Drawing.Size(622, 79);
            this.NotesMemoEdit.StyleController = this.dataLayoutControl1;
            this.NotesMemoEdit.TabIndex = 29;
            // 
            // ReviewNotesMemoEdit
            // 
            this.ReviewNotesMemoEdit.Location = new System.Drawing.Point(116, 573);
            this.ReviewNotesMemoEdit.MenuManager = this.barManager1;
            this.ReviewNotesMemoEdit.Name = "ReviewNotesMemoEdit";
            this.ReviewNotesMemoEdit.Size = new System.Drawing.Size(622, 78);
            this.ReviewNotesMemoEdit.StyleController = this.dataLayoutControl1;
            this.ReviewNotesMemoEdit.TabIndex = 25;
            // 
            // AdjustmentReasonMemoEdit
            // 
            this.AdjustmentReasonMemoEdit.Location = new System.Drawing.Point(116, 304);
            this.AdjustmentReasonMemoEdit.MenuManager = this.barManager1;
            this.AdjustmentReasonMemoEdit.Name = "AdjustmentReasonMemoEdit";
            this.AdjustmentReasonMemoEdit.Size = new System.Drawing.Size(622, 77);
            this.AdjustmentReasonMemoEdit.StyleController = this.dataLayoutControl1;
            this.AdjustmentReasonMemoEdit.TabIndex = 18;
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup1});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(762, 877);
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
            this.layoutControlGroup5,
            this.layoutControlGroup6,
            this.layoutControlGroup7,
            this.layoutControlGroup8});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "autoGeneratedGroup0";
            this.layoutControlGroup1.Size = new System.Drawing.Size(742, 857);
            // 
            // layoutControlGroup2
            // 
            this.layoutControlGroup2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.ItemForProductVariantName,
            this.ItemForIsCounted});
            this.layoutControlGroup2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup2.Name = "layoutControlGroup2";
            this.layoutControlGroup2.Size = new System.Drawing.Size(742, 91);
            this.layoutControlGroup2.Text = "Thông tin sản phẩm";
            // 
            // ItemForProductVariantName
            // 
            this.ItemForProductVariantName.AllowHtmlStringInCaption = true;
            this.ItemForProductVariantName.AppearanceItemCaption.Options.UseTextOptions = true;
            this.ItemForProductVariantName.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.ItemForProductVariantName.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.ItemForProductVariantName.Control = this.ProductVariantNameSearchLookupEdit;
            this.ItemForProductVariantName.Location = new System.Drawing.Point(0, 0);
            this.ItemForProductVariantName.Name = "ItemForProductVariantName";
            this.ItemForProductVariantName.Size = new System.Drawing.Size(718, 24);
            this.ItemForProductVariantName.Text = "Tên sản phẩm <color=\'red\'>*</color>";
            this.ItemForProductVariantName.TextSize = new System.Drawing.Size(80, 13);
            // 
            // ItemForIsCounted
            // 
            this.ItemForIsCounted.AllowHtmlStringInCaption = true;
            this.ItemForIsCounted.AppearanceItemCaption.Options.UseTextOptions = true;
            this.ItemForIsCounted.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.ItemForIsCounted.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.ItemForIsCounted.Control = this.IsCountedToggleSwitch;
            this.ItemForIsCounted.Location = new System.Drawing.Point(0, 24);
            this.ItemForIsCounted.Name = "ItemForIsCounted";
            this.ItemForIsCounted.Size = new System.Drawing.Size(718, 22);
            this.ItemForIsCounted.Text = "Đã kiểm đếm";
            this.ItemForIsCounted.TextSize = new System.Drawing.Size(80, 13);
            // 
            // layoutControlGroup3
            // 
            this.layoutControlGroup3.ExpandButtonVisible = true;
            this.layoutControlGroup3.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.layoutControlGroup3.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.ItemForSystemQuantity,
            this.ItemForCountedQuantity,
            this.ItemForDifferenceQuantity});
            this.layoutControlGroup3.Location = new System.Drawing.Point(0, 91);
            this.layoutControlGroup3.Name = "layoutControlGroup3";
            this.layoutControlGroup3.Size = new System.Drawing.Size(742, 72);
            this.layoutControlGroup3.Text = "Số lượng";
            // 
            // ItemForSystemQuantity
            // 
            this.ItemForSystemQuantity.AllowHtmlStringInCaption = true;
            this.ItemForSystemQuantity.AppearanceItemCaption.Options.UseTextOptions = true;
            this.ItemForSystemQuantity.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.ItemForSystemQuantity.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.ItemForSystemQuantity.Control = this.SystemQuantityMemoEdit;
            this.ItemForSystemQuantity.Location = new System.Drawing.Point(0, 0);
            this.ItemForSystemQuantity.Name = "ItemForSystemQuantity";
            this.ItemForSystemQuantity.Size = new System.Drawing.Size(217, 27);
            this.ItemForSystemQuantity.Text = "SL Hệ thống <color=\'red\'>*</color>";
            this.ItemForSystemQuantity.TextSize = new System.Drawing.Size(80, 13);
            // 
            // ItemForCountedQuantity
            // 
            this.ItemForCountedQuantity.AllowHtmlStringInCaption = true;
            this.ItemForCountedQuantity.AppearanceItemCaption.Options.UseTextOptions = true;
            this.ItemForCountedQuantity.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.ItemForCountedQuantity.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.ItemForCountedQuantity.Control = this.CountedQuantityMemoEdit;
            this.ItemForCountedQuantity.Location = new System.Drawing.Point(217, 0);
            this.ItemForCountedQuantity.Name = "ItemForCountedQuantity";
            this.ItemForCountedQuantity.Size = new System.Drawing.Size(180, 27);
            this.ItemForCountedQuantity.Text = "SL Đã kiểm";
            this.ItemForCountedQuantity.TextSize = new System.Drawing.Size(80, 13);
            // 
            // ItemForDifferenceQuantity
            // 
            this.ItemForDifferenceQuantity.AllowHtmlStringInCaption = true;
            this.ItemForDifferenceQuantity.AppearanceItemCaption.Options.UseTextOptions = true;
            this.ItemForDifferenceQuantity.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.ItemForDifferenceQuantity.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.ItemForDifferenceQuantity.Control = this.DifferenceQuantityMemoEdit;
            this.ItemForDifferenceQuantity.Location = new System.Drawing.Point(397, 0);
            this.ItemForDifferenceQuantity.Name = "ItemForDifferenceQuantity";
            this.ItemForDifferenceQuantity.Size = new System.Drawing.Size(321, 27);
            this.ItemForDifferenceQuantity.Text = "SL Chênh lệch <color=\'red\'>*</color>";
            this.ItemForDifferenceQuantity.TextSize = new System.Drawing.Size(80, 13);
            // 
            // layoutControlGroup4
            // 
            this.layoutControlGroup4.ExpandButtonVisible = true;
            this.layoutControlGroup4.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.layoutControlGroup4.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.ItemForSystemValue,
            this.ItemForCountedValue,
            this.ItemForDifferenceValue,
            this.ItemForUnitPrice});
            this.layoutControlGroup4.Location = new System.Drawing.Point(0, 163);
            this.layoutControlGroup4.Name = "layoutControlGroup4";
            this.layoutControlGroup4.Size = new System.Drawing.Size(742, 72);
            this.layoutControlGroup4.Text = "Giá trị";
            // 
            // ItemForSystemValue
            // 
            this.ItemForSystemValue.AllowHtmlStringInCaption = true;
            this.ItemForSystemValue.AppearanceItemCaption.Options.UseTextOptions = true;
            this.ItemForSystemValue.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.ItemForSystemValue.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.ItemForSystemValue.Control = this.SystemValueMemoEdit;
            this.ItemForSystemValue.Location = new System.Drawing.Point(179, 0);
            this.ItemForSystemValue.Name = "ItemForSystemValue";
            this.ItemForSystemValue.Size = new System.Drawing.Size(180, 27);
            this.ItemForSystemValue.Text = "Giá trị Hệ thống";
            this.ItemForSystemValue.TextSize = new System.Drawing.Size(80, 13);
            // 
            // ItemForCountedValue
            // 
            this.ItemForCountedValue.AllowHtmlStringInCaption = true;
            this.ItemForCountedValue.AppearanceItemCaption.Options.UseTextOptions = true;
            this.ItemForCountedValue.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.ItemForCountedValue.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.ItemForCountedValue.Control = this.CountedValueMemoEdit;
            this.ItemForCountedValue.Location = new System.Drawing.Point(359, 0);
            this.ItemForCountedValue.Name = "ItemForCountedValue";
            this.ItemForCountedValue.Size = new System.Drawing.Size(179, 27);
            this.ItemForCountedValue.Text = "Giá trị Đã kiểm";
            this.ItemForCountedValue.TextSize = new System.Drawing.Size(80, 13);
            // 
            // ItemForDifferenceValue
            // 
            this.ItemForDifferenceValue.AllowHtmlStringInCaption = true;
            this.ItemForDifferenceValue.AppearanceItemCaption.Options.UseTextOptions = true;
            this.ItemForDifferenceValue.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.ItemForDifferenceValue.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.ItemForDifferenceValue.Control = this.DifferenceValueMemoEdit;
            this.ItemForDifferenceValue.Location = new System.Drawing.Point(538, 0);
            this.ItemForDifferenceValue.Name = "ItemForDifferenceValue";
            this.ItemForDifferenceValue.Size = new System.Drawing.Size(180, 27);
            this.ItemForDifferenceValue.Text = "Giá trị Chênh lệch";
            this.ItemForDifferenceValue.TextSize = new System.Drawing.Size(80, 13);
            // 
            // ItemForUnitPrice
            // 
            this.ItemForUnitPrice.AllowHtmlStringInCaption = true;
            this.ItemForUnitPrice.AppearanceItemCaption.Options.UseTextOptions = true;
            this.ItemForUnitPrice.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.ItemForUnitPrice.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.ItemForUnitPrice.Control = this.UnitPriceMemoEdit;
            this.ItemForUnitPrice.Location = new System.Drawing.Point(0, 0);
            this.ItemForUnitPrice.Name = "ItemForUnitPrice";
            this.ItemForUnitPrice.Size = new System.Drawing.Size(179, 27);
            this.ItemForUnitPrice.Text = "Đơn giá";
            this.ItemForUnitPrice.TextSize = new System.Drawing.Size(80, 13);
            // 
            // layoutControlGroup5
            // 
            this.layoutControlGroup5.ExpandButtonVisible = true;
            this.layoutControlGroup5.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.layoutControlGroup5.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.ItemForAdjustmentType,
            this.ItemForAdjustmentReason});
            this.layoutControlGroup5.Location = new System.Drawing.Point(0, 235);
            this.layoutControlGroup5.Name = "layoutControlGroup5";
            this.layoutControlGroup5.Size = new System.Drawing.Size(742, 150);
            this.layoutControlGroup5.Text = "Điều chỉnh";
            // 
            // ItemForAdjustmentType
            // 
            this.ItemForAdjustmentType.AllowHtmlStringInCaption = true;
            this.ItemForAdjustmentType.AppearanceItemCaption.Options.UseTextOptions = true;
            this.ItemForAdjustmentType.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.ItemForAdjustmentType.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.ItemForAdjustmentType.Control = this.AdjustmentTypeImageComboBoxEdit;
            this.ItemForAdjustmentType.Location = new System.Drawing.Point(0, 0);
            this.ItemForAdjustmentType.Name = "ItemForAdjustmentType";
            this.ItemForAdjustmentType.Size = new System.Drawing.Size(718, 24);
            this.ItemForAdjustmentType.Text = "Loại điều chỉnh";
            this.ItemForAdjustmentType.TextSize = new System.Drawing.Size(80, 13);
            // 
            // ItemForAdjustmentReason
            // 
            this.ItemForAdjustmentReason.AllowHtmlStringInCaption = true;
            this.ItemForAdjustmentReason.AppearanceItemCaption.Options.UseTextOptions = true;
            this.ItemForAdjustmentReason.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.ItemForAdjustmentReason.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.ItemForAdjustmentReason.Control = this.AdjustmentReasonMemoEdit;
            this.ItemForAdjustmentReason.Location = new System.Drawing.Point(0, 24);
            this.ItemForAdjustmentReason.Name = "ItemForAdjustmentReason";
            this.ItemForAdjustmentReason.Size = new System.Drawing.Size(718, 81);
            this.ItemForAdjustmentReason.Text = "Lý do điều chỉnh";
            this.ItemForAdjustmentReason.TextSize = new System.Drawing.Size(80, 13);
            // 
            // layoutControlGroup6
            // 
            this.layoutControlGroup6.ExpandButtonVisible = true;
            this.layoutControlGroup6.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.layoutControlGroup6.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.ItemForCountedBy,
            this.ItemForCountedDate});
            this.layoutControlGroup6.Location = new System.Drawing.Point(0, 385);
            this.layoutControlGroup6.Name = "layoutControlGroup6";
            this.layoutControlGroup6.Size = new System.Drawing.Size(742, 69);
            this.layoutControlGroup6.Text = "Đảm nhiệm";
            // 
            // ItemForCountedBy
            // 
            this.ItemForCountedBy.AllowHtmlStringInCaption = true;
            this.ItemForCountedBy.AppearanceItemCaption.Options.UseTextOptions = true;
            this.ItemForCountedBy.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.ItemForCountedBy.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.ItemForCountedBy.Control = this.CountedByMemoEdit;
            this.ItemForCountedBy.Location = new System.Drawing.Point(359, 0);
            this.ItemForCountedBy.Name = "ItemForCountedBy";
            this.ItemForCountedBy.Size = new System.Drawing.Size(359, 24);
            this.ItemForCountedBy.Text = "Người kiểm đếm";
            this.ItemForCountedBy.TextSize = new System.Drawing.Size(80, 13);
            // 
            // ItemForCountedDate
            // 
            this.ItemForCountedDate.AllowHtmlStringInCaption = true;
            this.ItemForCountedDate.AppearanceItemCaption.Options.UseTextOptions = true;
            this.ItemForCountedDate.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.ItemForCountedDate.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.ItemForCountedDate.Control = this.CountedDateDateEdit;
            this.ItemForCountedDate.Location = new System.Drawing.Point(0, 0);
            this.ItemForCountedDate.Name = "ItemForCountedDate";
            this.ItemForCountedDate.Size = new System.Drawing.Size(359, 24);
            this.ItemForCountedDate.Text = "Ngày kiểm đếm";
            this.ItemForCountedDate.TextSize = new System.Drawing.Size(80, 13);
            // 
            // layoutControlGroup7
            // 
            this.layoutControlGroup7.ExpandButtonVisible = true;
            this.layoutControlGroup7.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.layoutControlGroup7.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.ItemForReviewedBy,
            this.ItemForIsReviewed,
            this.ItemForReviewedDate,
            this.ItemForReviewNotes});
            this.layoutControlGroup7.Location = new System.Drawing.Point(0, 454);
            this.layoutControlGroup7.Name = "layoutControlGroup7";
            this.layoutControlGroup7.Size = new System.Drawing.Size(742, 201);
            this.layoutControlGroup7.Text = "Rà soát";
            // 
            // ItemForReviewedBy
            // 
            this.ItemForReviewedBy.AllowHtmlStringInCaption = true;
            this.ItemForReviewedBy.AppearanceItemCaption.Options.UseTextOptions = true;
            this.ItemForReviewedBy.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.ItemForReviewedBy.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.ItemForReviewedBy.Control = this.ReviewedByMemoEdit;
            this.ItemForReviewedBy.Location = new System.Drawing.Point(0, 24);
            this.ItemForReviewedBy.Name = "ItemForReviewedBy";
            this.ItemForReviewedBy.Size = new System.Drawing.Size(718, 26);
            this.ItemForReviewedBy.Text = "Người rà soát";
            this.ItemForReviewedBy.TextSize = new System.Drawing.Size(80, 13);
            // 
            // ItemForIsReviewed
            // 
            this.ItemForIsReviewed.AllowHtmlStringInCaption = true;
            this.ItemForIsReviewed.AppearanceItemCaption.Options.UseTextOptions = true;
            this.ItemForIsReviewed.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.ItemForIsReviewed.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.ItemForIsReviewed.Control = this.IsReviewedCheckEdit;
            this.ItemForIsReviewed.Location = new System.Drawing.Point(0, 0);
            this.ItemForIsReviewed.Name = "ItemForIsReviewed";
            this.ItemForIsReviewed.Size = new System.Drawing.Size(718, 24);
            this.ItemForIsReviewed.Text = "Đã rà soát";
            this.ItemForIsReviewed.TextVisible = false;
            // 
            // ItemForReviewedDate
            // 
            this.ItemForReviewedDate.AllowHtmlStringInCaption = true;
            this.ItemForReviewedDate.AppearanceItemCaption.Options.UseTextOptions = true;
            this.ItemForReviewedDate.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.ItemForReviewedDate.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.ItemForReviewedDate.Control = this.ReviewedDateDateEdit;
            this.ItemForReviewedDate.Location = new System.Drawing.Point(0, 50);
            this.ItemForReviewedDate.Name = "ItemForReviewedDate";
            this.ItemForReviewedDate.Size = new System.Drawing.Size(718, 24);
            this.ItemForReviewedDate.Text = "Ngày rà soát";
            this.ItemForReviewedDate.TextSize = new System.Drawing.Size(80, 13);
            // 
            // ItemForReviewNotes
            // 
            this.ItemForReviewNotes.AllowHtmlStringInCaption = true;
            this.ItemForReviewNotes.AppearanceItemCaption.Options.UseTextOptions = true;
            this.ItemForReviewNotes.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.ItemForReviewNotes.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.ItemForReviewNotes.Control = this.ReviewNotesMemoEdit;
            this.ItemForReviewNotes.Location = new System.Drawing.Point(0, 74);
            this.ItemForReviewNotes.Name = "ItemForReviewNotes";
            this.ItemForReviewNotes.Size = new System.Drawing.Size(718, 82);
            this.ItemForReviewNotes.Text = "Ghi chú rà soát";
            this.ItemForReviewNotes.TextSize = new System.Drawing.Size(80, 13);
            // 
            // layoutControlGroup8
            // 
            this.layoutControlGroup8.ExpandButtonVisible = true;
            this.layoutControlGroup8.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.layoutControlGroup8.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.ItemForIsApproved,
            this.ItemForApprovedBy,
            this.ItemForApprovedDate,
            this.ItemForNotes});
            this.layoutControlGroup8.Location = new System.Drawing.Point(0, 655);
            this.layoutControlGroup8.Name = "layoutControlGroup8";
            this.layoutControlGroup8.Size = new System.Drawing.Size(742, 202);
            this.layoutControlGroup8.Text = "Phê duyệt";
            // 
            // ItemForIsApproved
            // 
            this.ItemForIsApproved.AllowHtmlStringInCaption = true;
            this.ItemForIsApproved.AppearanceItemCaption.Options.UseTextOptions = true;
            this.ItemForIsApproved.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.ItemForIsApproved.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.ItemForIsApproved.Control = this.IsApprovedCheckEdit;
            this.ItemForIsApproved.Location = new System.Drawing.Point(0, 0);
            this.ItemForIsApproved.Name = "ItemForIsApproved";
            this.ItemForIsApproved.Size = new System.Drawing.Size(718, 24);
            this.ItemForIsApproved.Text = "Đã phê duyệt";
            this.ItemForIsApproved.TextVisible = false;
            // 
            // ItemForApprovedBy
            // 
            this.ItemForApprovedBy.AllowHtmlStringInCaption = true;
            this.ItemForApprovedBy.AppearanceItemCaption.Options.UseTextOptions = true;
            this.ItemForApprovedBy.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.ItemForApprovedBy.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.ItemForApprovedBy.Control = this.ApprovedByMemoEdit;
            this.ItemForApprovedBy.Location = new System.Drawing.Point(0, 24);
            this.ItemForApprovedBy.Name = "ItemForApprovedBy";
            this.ItemForApprovedBy.Size = new System.Drawing.Size(718, 26);
            this.ItemForApprovedBy.Text = "Người phê duyệt";
            this.ItemForApprovedBy.TextSize = new System.Drawing.Size(80, 13);
            // 
            // ItemForApprovedDate
            // 
            this.ItemForApprovedDate.AllowHtmlStringInCaption = true;
            this.ItemForApprovedDate.AppearanceItemCaption.Options.UseTextOptions = true;
            this.ItemForApprovedDate.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.ItemForApprovedDate.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.ItemForApprovedDate.Control = this.ApprovedDateDateEdit;
            this.ItemForApprovedDate.Location = new System.Drawing.Point(0, 50);
            this.ItemForApprovedDate.Name = "ItemForApprovedDate";
            this.ItemForApprovedDate.Size = new System.Drawing.Size(718, 24);
            this.ItemForApprovedDate.Text = "Ngày phê duyệt";
            this.ItemForApprovedDate.TextSize = new System.Drawing.Size(80, 13);
            // 
            // ItemForNotes
            // 
            this.ItemForNotes.AllowHtmlStringInCaption = true;
            this.ItemForNotes.AppearanceItemCaption.Options.UseTextOptions = true;
            this.ItemForNotes.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.ItemForNotes.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.ItemForNotes.Control = this.NotesMemoEdit;
            this.ItemForNotes.Location = new System.Drawing.Point(0, 74);
            this.ItemForNotes.Name = "ItemForNotes";
            this.ItemForNotes.Size = new System.Drawing.Size(718, 83);
            this.ItemForNotes.Text = "Ghi chú";
            this.ItemForNotes.TextSize = new System.Drawing.Size(80, 13);
            // 
            // colVariantFullName
            // 
            this.colVariantFullName.ColumnEdit = this.VariantFullNameHypertextLabel;
            this.colVariantFullName.Caption = "Sản phẩm";
            this.colVariantFullName.FieldName = "VariantFullName";
            this.colVariantFullName.Name = "colVariantFullName";
            this.colVariantFullName.Visible = true;
            this.colVariantFullName.VisibleIndex = 0;
            // 
            // VariantFullNameHypertextLabel
            // 
            this.VariantFullNameHypertextLabel.Name = "VariantFullNameHypertextLabel";
            // 
            // FrmStocktakingDetailAddEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(762, 901);
            this.Controls.Add(this.dataLayoutControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmStocktakingDetailAddEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Chi tiết kiểm kho";
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.productVariantSimpleDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).EndInit();
            this.dataLayoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SystemQuantityMemoEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CountedQuantityMemoEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DifferenceQuantityMemoEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SystemValueMemoEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CountedValueMemoEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DifferenceValueMemoEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UnitPriceMemoEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AdjustmentTypeImageComboBoxEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CountedByMemoEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CountedDateDateEdit.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CountedDateDateEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.IsReviewedCheckEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReviewedByMemoEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReviewedDateDateEdit.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReviewedDateDateEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.IsApprovedCheckEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ApprovedByMemoEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ApprovedDateDateEdit.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ApprovedDateDateEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductVariantNameSearchLookupEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.IsCountedToggleSwitch.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NotesMemoEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReviewNotesMemoEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AdjustmentReasonMemoEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForProductVariantName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForIsCounted)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForSystemQuantity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForCountedQuantity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForDifferenceQuantity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForSystemValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForCountedValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForDifferenceValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForUnitPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForAdjustmentType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForAdjustmentReason)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForCountedBy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForCountedDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForReviewedBy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForIsReviewed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForReviewedDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForReviewNotes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForIsApproved)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForApprovedBy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForApprovedDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForNotes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VariantFullNameHypertextLabel)).EndInit();
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
        private DevExpress.XtraTreeList.Columns.TreeListColumn colParentDepartmentName;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colDepartmentName;
        private BarHeaderItem barHeaderItem1;
        private BarStaticItem SoLuongNhapXuatBarStaticItem;

        private System.Windows.Forms.BindingSource productVariantSimpleDtoBindingSource;
        private DataLayoutControl dataLayoutControl1;
        private LayoutControlGroup Root;
        private DevExpress.XtraEditors.MemoEdit SystemQuantityMemoEdit;
        private DevExpress.XtraEditors.MemoEdit CountedQuantityMemoEdit;
        private DevExpress.XtraEditors.MemoEdit DifferenceQuantityMemoEdit;
        private DevExpress.XtraEditors.MemoEdit SystemValueMemoEdit;
        private DevExpress.XtraEditors.MemoEdit CountedValueMemoEdit;
        private DevExpress.XtraEditors.MemoEdit DifferenceValueMemoEdit;
        private DevExpress.XtraEditors.MemoEdit UnitPriceMemoEdit;
        private DevExpress.XtraEditors.ImageComboBoxEdit AdjustmentTypeImageComboBoxEdit;
        private DevExpress.XtraEditors.MemoEdit CountedByMemoEdit;
        private DevExpress.XtraEditors.DateEdit CountedDateDateEdit;
        private DevExpress.XtraEditors.CheckEdit IsReviewedCheckEdit;
        private DevExpress.XtraEditors.MemoEdit ReviewedByMemoEdit;
        private DevExpress.XtraEditors.DateEdit ReviewedDateDateEdit;
        private DevExpress.XtraEditors.CheckEdit IsApprovedCheckEdit;
        private DevExpress.XtraEditors.MemoEdit ApprovedByMemoEdit;
        private DevExpress.XtraEditors.DateEdit ApprovedDateDateEdit;
        private LayoutControlGroup layoutControlGroup1;
        private LayoutControlItem ItemForProductVariantName;
        private LayoutControlItem ItemForSystemQuantity;
        private LayoutControlItem ItemForCountedQuantity;
        private LayoutControlItem ItemForDifferenceQuantity;
        private LayoutControlItem ItemForSystemValue;
        private LayoutControlItem ItemForCountedValue;
        private LayoutControlItem ItemForDifferenceValue;
        private LayoutControlItem ItemForUnitPrice;
        private LayoutControlItem ItemForAdjustmentType;
        private LayoutControlItem ItemForAdjustmentReason;
        private LayoutControlItem ItemForIsCounted;
        private LayoutControlItem ItemForCountedBy;
        private LayoutControlItem ItemForCountedDate;
        private LayoutControlItem ItemForIsReviewed;
        private LayoutControlItem ItemForReviewedBy;
        private LayoutControlItem ItemForReviewedDate;
        private LayoutControlItem ItemForReviewNotes;
        private LayoutControlItem ItemForIsApproved;
        private LayoutControlItem ItemForApprovedBy;
        private LayoutControlItem ItemForApprovedDate;
        private LayoutControlItem ItemForNotes;
        private DevExpress.XtraEditors.SearchLookUpEdit ProductVariantNameSearchLookupEdit;
        private DevExpress.XtraGrid.Views.Grid.GridView searchLookUpEdit1View;
        private LayoutControlGroup layoutControlGroup2;
        private LayoutControlGroup layoutControlGroup3;
        private LayoutControlGroup layoutControlGroup4;
        private LayoutControlGroup layoutControlGroup5;
        private DevExpress.XtraEditors.ToggleSwitch IsCountedToggleSwitch;
        private LayoutControlGroup layoutControlGroup6;
        private LayoutControlGroup layoutControlGroup7;
        private LayoutControlGroup layoutControlGroup8;
        private DevExpress.XtraEditors.MemoEdit NotesMemoEdit;
        private DevExpress.XtraEditors.MemoEdit ReviewNotesMemoEdit;
        private DevExpress.XtraEditors.MemoEdit AdjustmentReasonMemoEdit;
        private DevExpress.XtraGrid.Columns.GridColumn colVariantFullName;
        private DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel VariantFullNameHypertextLabel;
    }
}