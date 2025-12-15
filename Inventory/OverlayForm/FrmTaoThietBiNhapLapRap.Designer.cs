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
using DTO.MasterData.ProductService;

namespace Inventory.OverlayForm
{
    partial class FrmTaoThietBiNhapLapRap
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
            DevExpress.XtraLayout.RowDefinition rowDefinition4 = new DevExpress.XtraLayout.RowDefinition();
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
            this.VariantAttributeDtoGridControl = new DevExpress.XtraGrid.GridControl();
            this.variantAttributeDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.VariantAttributeDtoGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colAttributeName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemMemoEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
            this.colAttributeValue = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PhieuXuatLapRapLookupEdit = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.xuatLapRapMasterListDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.searchLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colStockOutInfoHtml = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PhieuNhapLapRapHypertextLabel = new DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel();
            this.ProductServiceLookupEdit = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.productServiceDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ProductServiceGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colThongTinHtml = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ProductServiceHypertextLabel = new DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel();
            this.UnitOfMeasureSearchLookupEdit = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.unitOfMeasureDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.UnitOfMeasureGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colDisplayHtml = new DevExpress.XtraGrid.Columns.GridColumn();
            this.UnitOfMeasureHypertextLabel = new DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.ItemForProductName = new DevExpress.XtraLayout.LayoutControlItem();
            this.simpleLabelItem3 = new DevExpress.XtraLayout.SimpleLabelItem();
            this.ItemForUnitName = new DevExpress.XtraLayout.LayoutControlItem();
            this.simpleLabelItem2 = new DevExpress.XtraLayout.SimpleLabelItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.simpleLabelItem1 = new DevExpress.XtraLayout.SimpleLabelItem();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).BeginInit();
            this.dataLayoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VariantAttributeDtoGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.variantAttributeDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VariantAttributeDtoGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PhieuXuatLapRapLookupEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xuatLapRapMasterListDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PhieuNhapLapRapHypertextLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductServiceLookupEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.productServiceDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductServiceGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductServiceHypertextLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UnitOfMeasureSearchLookupEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.unitOfMeasureDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UnitOfMeasureGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UnitOfMeasureHypertextLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForProductName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForUnitName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem1)).BeginInit();
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
            this.barManager1.MaxItemId = 3;
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
            this.barDockControlTop.Size = new System.Drawing.Size(611, 24);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 320);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(611, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 24);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 296);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(611, 24);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 296);
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
            this.dataLayoutControl1.Controls.Add(this.VariantAttributeDtoGridControl);
            this.dataLayoutControl1.Controls.Add(this.PhieuXuatLapRapLookupEdit);
            this.dataLayoutControl1.Controls.Add(this.ProductServiceLookupEdit);
            this.dataLayoutControl1.Controls.Add(this.UnitOfMeasureSearchLookupEdit);
            this.dataLayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataLayoutControl1.Location = new System.Drawing.Point(0, 24);
            this.dataLayoutControl1.Name = "dataLayoutControl1";
            this.dataLayoutControl1.Root = this.Root;
            this.dataLayoutControl1.Size = new System.Drawing.Size(611, 296);
            this.dataLayoutControl1.TabIndex = 15;
            this.dataLayoutControl1.Text = "dataLayoutControl1";
            // 
            // VariantAttributeDtoGridControl
            // 
            this.VariantAttributeDtoGridControl.DataSource = this.variantAttributeDtoBindingSource;
            this.VariantAttributeDtoGridControl.Location = new System.Drawing.Point(12, 84);
            this.VariantAttributeDtoGridControl.MainView = this.VariantAttributeDtoGridView;
            this.VariantAttributeDtoGridControl.MenuManager = this.barManager1;
            this.VariantAttributeDtoGridControl.Name = "VariantAttributeDtoGridControl";
            this.VariantAttributeDtoGridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemMemoEdit1});
            this.VariantAttributeDtoGridControl.Size = new System.Drawing.Size(587, 200);
            this.VariantAttributeDtoGridControl.TabIndex = 5;
            this.VariantAttributeDtoGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.VariantAttributeDtoGridView});
            // 
            // variantAttributeDtoBindingSource
            // 
            this.variantAttributeDtoBindingSource.DataSource = typeof(DTO.MasterData.ProductService.VariantAttributeDto);
            // 
            // VariantAttributeDtoGridView
            // 
            this.VariantAttributeDtoGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colAttributeName,
            this.colAttributeValue});
            this.VariantAttributeDtoGridView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.VariantAttributeDtoGridView.GridControl = this.VariantAttributeDtoGridControl;
            this.VariantAttributeDtoGridView.Name = "VariantAttributeDtoGridView";
            this.VariantAttributeDtoGridView.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.VariantAttributeDtoGridView.OptionsView.ColumnAutoWidth = false;
            this.VariantAttributeDtoGridView.OptionsView.RowAutoHeight = true;
            this.VariantAttributeDtoGridView.OptionsView.ShowGroupPanel = false;
            // 
            // colAttributeName
            // 
            this.colAttributeName.AppearanceCell.Options.UseTextOptions = true;
            this.colAttributeName.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colAttributeName.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colAttributeName.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colAttributeName.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colAttributeName.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colAttributeName.AppearanceHeader.Options.UseBackColor = true;
            this.colAttributeName.AppearanceHeader.Options.UseFont = true;
            this.colAttributeName.AppearanceHeader.Options.UseForeColor = true;
            this.colAttributeName.AppearanceHeader.Options.UseTextOptions = true;
            this.colAttributeName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colAttributeName.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colAttributeName.Caption = "Tên thuộc tính";
            this.colAttributeName.ColumnEdit = this.repositoryItemMemoEdit1;
            this.colAttributeName.FieldName = "AttributeName";
            this.colAttributeName.Name = "colAttributeName";
            this.colAttributeName.OptionsColumn.AllowEdit = false;
            this.colAttributeName.OptionsColumn.ReadOnly = true;
            this.colAttributeName.Visible = true;
            this.colAttributeName.VisibleIndex = 0;
            this.colAttributeName.Width = 200;
            // 
            // repositoryItemMemoEdit1
            // 
            this.repositoryItemMemoEdit1.Name = "repositoryItemMemoEdit1";
            // 
            // colAttributeValue
            // 
            this.colAttributeValue.AppearanceCell.Options.UseTextOptions = true;
            this.colAttributeValue.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colAttributeValue.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colAttributeValue.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colAttributeValue.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colAttributeValue.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colAttributeValue.AppearanceHeader.Options.UseBackColor = true;
            this.colAttributeValue.AppearanceHeader.Options.UseFont = true;
            this.colAttributeValue.AppearanceHeader.Options.UseForeColor = true;
            this.colAttributeValue.AppearanceHeader.Options.UseTextOptions = true;
            this.colAttributeValue.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colAttributeValue.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colAttributeValue.Caption = "Giá trị";
            this.colAttributeValue.FieldName = "AttributeValue";
            this.colAttributeValue.Name = "colAttributeValue";
            this.colAttributeValue.OptionsColumn.AllowEdit = false;
            this.colAttributeValue.OptionsColumn.ReadOnly = true;
            this.colAttributeValue.Visible = true;
            this.colAttributeValue.VisibleIndex = 1;
            this.colAttributeValue.Width = 350;
            // 
            // PhieuXuatLapRapLookupEdit
            // 
            this.PhieuXuatLapRapLookupEdit.Location = new System.Drawing.Point(159, 12);
            this.PhieuXuatLapRapLookupEdit.MenuManager = this.barManager1;
            this.PhieuXuatLapRapLookupEdit.Name = "PhieuXuatLapRapLookupEdit";
            this.PhieuXuatLapRapLookupEdit.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.PhieuXuatLapRapLookupEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.PhieuXuatLapRapLookupEdit.Properties.DataSource = this.xuatLapRapMasterListDtoBindingSource;
            this.PhieuXuatLapRapLookupEdit.Properties.DisplayMember = "StockInInfoHtml";
            this.PhieuXuatLapRapLookupEdit.Properties.NullText = "";
            this.PhieuXuatLapRapLookupEdit.Properties.PopupView = this.searchLookUpEdit1View;
            this.PhieuXuatLapRapLookupEdit.Properties.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.PhieuNhapLapRapHypertextLabel});
            this.PhieuXuatLapRapLookupEdit.Properties.ValueMember = "Id";
            this.PhieuXuatLapRapLookupEdit.Size = new System.Drawing.Size(440, 20);
            this.PhieuXuatLapRapLookupEdit.StyleController = this.dataLayoutControl1;
            this.PhieuXuatLapRapLookupEdit.TabIndex = 0;
            // 
            // xuatLapRapMasterListDtoBindingSource
            // 
            this.xuatLapRapMasterListDtoBindingSource.DataSource = typeof(DTO.Inventory.StockIn.NhapLapRap.NhapLapRapMasterListDto);
            // 
            // searchLookUpEdit1View
            // 
            this.searchLookUpEdit1View.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colStockOutInfoHtml});
            this.searchLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.searchLookUpEdit1View.Name = "searchLookUpEdit1View";
            this.searchLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.searchLookUpEdit1View.OptionsView.ColumnAutoWidth = false;
            this.searchLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // colStockOutInfoHtml
            // 
            this.colStockOutInfoHtml.Caption = "Thông tin phiếu xuất";
            this.colStockOutInfoHtml.ColumnEdit = this.PhieuNhapLapRapHypertextLabel;
            this.colStockOutInfoHtml.FieldName = "StockInInfoHtml";
            this.colStockOutInfoHtml.Name = "colStockOutInfoHtml";
            this.colStockOutInfoHtml.OptionsColumn.AllowEdit = false;
            this.colStockOutInfoHtml.OptionsColumn.ReadOnly = true;
            this.colStockOutInfoHtml.Visible = true;
            this.colStockOutInfoHtml.VisibleIndex = 0;
            this.colStockOutInfoHtml.Width = 600;
            // 
            // PhieuNhapLapRapHypertextLabel
            // 
            this.PhieuNhapLapRapHypertextLabel.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.PhieuNhapLapRapHypertextLabel.Name = "PhieuNhapLapRapHypertextLabel";
            // 
            // ProductServiceLookupEdit
            // 
            this.ProductServiceLookupEdit.Location = new System.Drawing.Point(159, 36);
            this.ProductServiceLookupEdit.MenuManager = this.barManager1;
            this.ProductServiceLookupEdit.Name = "ProductServiceLookupEdit";
            this.ProductServiceLookupEdit.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.ProductServiceLookupEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ProductServiceLookupEdit.Properties.DataSource = this.productServiceDtoBindingSource;
            this.ProductServiceLookupEdit.Properties.DisplayMember = "ThongTinHtml";
            this.ProductServiceLookupEdit.Properties.NullText = "";
            this.ProductServiceLookupEdit.Properties.PopupView = this.ProductServiceGridView;
            this.ProductServiceLookupEdit.Properties.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.ProductServiceHypertextLabel});
            this.ProductServiceLookupEdit.Properties.ValueMember = "Id";
            this.ProductServiceLookupEdit.Size = new System.Drawing.Size(440, 20);
            this.ProductServiceLookupEdit.StyleController = this.dataLayoutControl1;
            this.ProductServiceLookupEdit.TabIndex = 2;
            // 
            // productServiceDtoBindingSource
            // 
            this.productServiceDtoBindingSource.DataSource = typeof(DTO.MasterData.ProductService.ProductServiceDto);
            // 
            // ProductServiceGridView
            // 
            this.ProductServiceGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colThongTinHtml});
            this.ProductServiceGridView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.ProductServiceGridView.Name = "ProductServiceGridView";
            this.ProductServiceGridView.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.ProductServiceGridView.OptionsView.ColumnAutoWidth = false;
            this.ProductServiceGridView.OptionsView.ShowGroupPanel = false;
            // 
            // colThongTinHtml
            // 
            this.colThongTinHtml.Caption = "Thông tin sản phẩm";
            this.colThongTinHtml.ColumnEdit = this.ProductServiceHypertextLabel;
            this.colThongTinHtml.FieldName = "ThongTinHtml";
            this.colThongTinHtml.Name = "colThongTinHtml";
            this.colThongTinHtml.OptionsColumn.AllowEdit = false;
            this.colThongTinHtml.OptionsColumn.ReadOnly = true;
            this.colThongTinHtml.Visible = true;
            this.colThongTinHtml.VisibleIndex = 0;
            this.colThongTinHtml.Width = 600;
            // 
            // ProductServiceHypertextLabel
            // 
            this.ProductServiceHypertextLabel.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.ProductServiceHypertextLabel.Name = "ProductServiceHypertextLabel";
            // 
            // UnitOfMeasureSearchLookupEdit
            // 
            this.UnitOfMeasureSearchLookupEdit.Location = new System.Drawing.Point(159, 60);
            this.UnitOfMeasureSearchLookupEdit.MenuManager = this.barManager1;
            this.UnitOfMeasureSearchLookupEdit.Name = "UnitOfMeasureSearchLookupEdit";
            this.UnitOfMeasureSearchLookupEdit.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.UnitOfMeasureSearchLookupEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.UnitOfMeasureSearchLookupEdit.Properties.DataSource = this.unitOfMeasureDtoBindingSource;
            this.UnitOfMeasureSearchLookupEdit.Properties.DisplayMember = "DisplayHtml";
            this.UnitOfMeasureSearchLookupEdit.Properties.NullText = "";
            this.UnitOfMeasureSearchLookupEdit.Properties.PopupSizeable = false;
            this.UnitOfMeasureSearchLookupEdit.Properties.PopupView = this.UnitOfMeasureGridView;
            this.UnitOfMeasureSearchLookupEdit.Properties.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.UnitOfMeasureHypertextLabel});
            this.UnitOfMeasureSearchLookupEdit.Properties.ValueMember = "Id";
            this.UnitOfMeasureSearchLookupEdit.Size = new System.Drawing.Size(440, 20);
            this.UnitOfMeasureSearchLookupEdit.StyleController = this.dataLayoutControl1;
            this.UnitOfMeasureSearchLookupEdit.TabIndex = 4;
            // 
            // unitOfMeasureDtoBindingSource
            // 
            this.unitOfMeasureDtoBindingSource.DataSource = typeof(DTO.MasterData.ProductService.UnitOfMeasureDto);
            // 
            // UnitOfMeasureGridView
            // 
            this.UnitOfMeasureGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colDisplayHtml});
            this.UnitOfMeasureGridView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.UnitOfMeasureGridView.Name = "UnitOfMeasureGridView";
            this.UnitOfMeasureGridView.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.UnitOfMeasureGridView.OptionsView.ShowGroupPanel = false;
            // 
            // colDisplayHtml
            // 
            this.colDisplayHtml.ColumnEdit = this.UnitOfMeasureHypertextLabel;
            this.colDisplayHtml.FieldName = "DisplayHtml";
            this.colDisplayHtml.Name = "colDisplayHtml";
            this.colDisplayHtml.Visible = true;
            this.colDisplayHtml.VisibleIndex = 0;
            // 
            // UnitOfMeasureHypertextLabel
            // 
            this.UnitOfMeasureHypertextLabel.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.UnitOfMeasureHypertextLabel.Name = "UnitOfMeasureHypertextLabel";
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.ItemForProductName,
            this.simpleLabelItem3,
            this.ItemForUnitName,
            this.simpleLabelItem2,
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.simpleLabelItem1});
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
            rowDefinition3.Height = 24D;
            rowDefinition3.SizeType = System.Windows.Forms.SizeType.AutoSize;
            rowDefinition4.Height = 204D;
            rowDefinition4.SizeType = System.Windows.Forms.SizeType.AutoSize;
            this.Root.OptionsTableLayoutGroup.RowDefinitions.AddRange(new DevExpress.XtraLayout.RowDefinition[] {
            rowDefinition1,
            rowDefinition2,
            rowDefinition3,
            rowDefinition4});
            this.Root.Size = new System.Drawing.Size(611, 296);
            this.Root.TextVisible = false;
            // 
            // ItemForProductName
            // 
            this.ItemForProductName.Control = this.PhieuXuatLapRapLookupEdit;
            this.ItemForProductName.Location = new System.Drawing.Point(147, 0);
            this.ItemForProductName.Name = "ItemForProductName";
            this.ItemForProductName.OptionsTableLayoutItem.ColumnIndex = 1;
            this.ItemForProductName.OptionsTableLayoutItem.ColumnSpan = 3;
            this.ItemForProductName.Size = new System.Drawing.Size(444, 24);
            this.ItemForProductName.Text = "Sản phẩm dịch vụ";
            this.ItemForProductName.TextVisible = false;
            // 
            // simpleLabelItem3
            // 
            this.simpleLabelItem3.Location = new System.Drawing.Point(0, 24);
            this.simpleLabelItem3.Name = "simpleLabelItem3";
            this.simpleLabelItem3.OptionsTableLayoutItem.RowIndex = 1;
            this.simpleLabelItem3.Size = new System.Drawing.Size(147, 24);
            this.simpleLabelItem3.Text = "Tên sản phẩm";
            this.simpleLabelItem3.TextSize = new System.Drawing.Size(87, 13);
            // 
            // ItemForUnitName
            // 
            this.ItemForUnitName.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.ItemForUnitName.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.ItemForUnitName.Control = this.ProductServiceLookupEdit;
            this.ItemForUnitName.Location = new System.Drawing.Point(147, 24);
            this.ItemForUnitName.Name = "ItemForUnitName";
            this.ItemForUnitName.OptionsTableLayoutItem.ColumnIndex = 1;
            this.ItemForUnitName.OptionsTableLayoutItem.ColumnSpan = 3;
            this.ItemForUnitName.OptionsTableLayoutItem.RowIndex = 1;
            this.ItemForUnitName.Size = new System.Drawing.Size(444, 24);
            this.ItemForUnitName.Text = "Đơn vị tính";
            this.ItemForUnitName.TextVisible = false;
            // 
            // simpleLabelItem2
            // 
            this.simpleLabelItem2.Location = new System.Drawing.Point(0, 48);
            this.simpleLabelItem2.Name = "simpleLabelItem2";
            this.simpleLabelItem2.OptionsTableLayoutItem.RowIndex = 2;
            this.simpleLabelItem2.Size = new System.Drawing.Size(147, 24);
            this.simpleLabelItem2.Text = "Đơn vị tính";
            this.simpleLabelItem2.TextSize = new System.Drawing.Size(87, 13);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.UnitOfMeasureSearchLookupEdit;
            this.layoutControlItem1.Location = new System.Drawing.Point(147, 48);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.OptionsTableLayoutItem.ColumnIndex = 1;
            this.layoutControlItem1.OptionsTableLayoutItem.ColumnSpan = 3;
            this.layoutControlItem1.OptionsTableLayoutItem.RowIndex = 2;
            this.layoutControlItem1.Size = new System.Drawing.Size(444, 24);
            this.layoutControlItem1.TextVisible = false;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.VariantAttributeDtoGridControl;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 72);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.OptionsTableLayoutItem.ColumnSpan = 4;
            this.layoutControlItem2.OptionsTableLayoutItem.RowIndex = 3;
            this.layoutControlItem2.Size = new System.Drawing.Size(591, 204);
            this.layoutControlItem2.TextVisible = false;
            // 
            // simpleLabelItem1
            // 
            this.simpleLabelItem1.Location = new System.Drawing.Point(0, 0);
            this.simpleLabelItem1.Name = "simpleLabelItem1";
            this.simpleLabelItem1.Size = new System.Drawing.Size(147, 24);
            this.simpleLabelItem1.Text = "Phiếu xuất lắp ráp";
            this.simpleLabelItem1.TextSize = new System.Drawing.Size(87, 13);
            // 
            // FrmTaoThietBiNhapLapRap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(611, 320);
            this.Controls.Add(this.dataLayoutControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmTaoThietBiNhapLapRap";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tạo thiết bị lắp ráp từ phiếu xuất lắp ráp";
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).EndInit();
            this.dataLayoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.VariantAttributeDtoGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.variantAttributeDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VariantAttributeDtoGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PhieuXuatLapRapLookupEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xuatLapRapMasterListDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PhieuNhapLapRapHypertextLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductServiceLookupEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.productServiceDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductServiceGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductServiceHypertextLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UnitOfMeasureSearchLookupEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.unitOfMeasureDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UnitOfMeasureGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UnitOfMeasureHypertextLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForProductName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForUnitName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem1)).EndInit();
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
        private SearchLookUpEdit PhieuXuatLapRapLookupEdit;
        private BindingSource productServiceDtoBindingSource;
        private GridView searchLookUpEdit1View;
        private SearchLookUpEdit ProductServiceLookupEdit;
        private GridView ProductServiceGridView;
        private LayoutControlItem ItemForProductName;
        private SimpleLabelItem simpleLabelItem3;
        private LayoutControlItem ItemForUnitName;
        private BindingSource xuatLapRapMasterListDtoBindingSource;
        private GridColumn colStockOutInfoHtml;
        private RepositoryItemHypertextLabel PhieuNhapLapRapHypertextLabel;
        private GridColumn colThongTinHtml;
        private RepositoryItemHypertextLabel ProductServiceHypertextLabel;
        private SimpleLabelItem simpleLabelItem2;
        private LayoutControlItem layoutControlItem1;
        private SearchLookUpEdit UnitOfMeasureSearchLookupEdit;
        private GridView UnitOfMeasureGridView;
        private BindingSource unitOfMeasureDtoBindingSource;
        private GridColumn colDisplayHtml;
        private RepositoryItemHypertextLabel UnitOfMeasureHypertextLabel;
        private GridControl VariantAttributeDtoGridControl;
        private GridView VariantAttributeDtoGridView;
        private LayoutControlItem layoutControlItem2;
        private SimpleLabelItem simpleLabelItem1;
        private BindingSource variantAttributeDtoBindingSource;
        private GridColumn colAttributeName;
        private GridColumn colAttributeValue;
        private RepositoryItemMemoEdit repositoryItemMemoEdit1;
    }
}