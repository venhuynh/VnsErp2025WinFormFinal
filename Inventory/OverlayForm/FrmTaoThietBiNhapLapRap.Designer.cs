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
            this.XuatLapRapDetailDtoGridControl = new DevExpress.XtraGrid.GridControl();
            this.XuatLapRapDetailDtoGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.ProductVariantSearchLookUpEdit = new DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit();
            this.ProductVariantSearchLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
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
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.ItemForProductName = new DevExpress.XtraLayout.LayoutControlItem();
            this.simpleLabelItem1 = new DevExpress.XtraLayout.SimpleLabelItem();
            this.simpleLabelItem3 = new DevExpress.XtraLayout.SimpleLabelItem();
            this.ItemForUnitName = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.xuatLapRapDetailDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.colFullNameHtml = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colStockOutQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.StockOutQtyTextEdit = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.productVariantListDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.colVariantFullName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.VariantFullNameHtmlHypertextLabel = new DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).BeginInit();
            this.dataLayoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.XuatLapRapDetailDtoGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.XuatLapRapDetailDtoGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductVariantSearchLookUpEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductVariantSearchLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StockOutQtyTextEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PhieuXuatLapRapLookupEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xuatLapRapMasterListDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PhieuNhapLapRapHypertextLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductServiceLookupEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.productServiceDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductServiceGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductServiceHypertextLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForProductName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForUnitName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xuatLapRapDetailDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.productVariantListDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VariantFullNameHtmlHypertextLabel)).BeginInit();
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
            this.dataLayoutControl1.Controls.Add(this.XuatLapRapDetailDtoGridControl);
            this.dataLayoutControl1.Controls.Add(this.PhieuXuatLapRapLookupEdit);
            this.dataLayoutControl1.Controls.Add(this.ProductServiceLookupEdit);
            this.dataLayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataLayoutControl1.Location = new System.Drawing.Point(0, 24);
            this.dataLayoutControl1.Name = "dataLayoutControl1";
            this.dataLayoutControl1.Root = this.Root;
            this.dataLayoutControl1.Size = new System.Drawing.Size(666, 636);
            this.dataLayoutControl1.TabIndex = 15;
            this.dataLayoutControl1.Text = "dataLayoutControl1";
            // 
            // XuatLapRapDetailDtoGridControl
            // 
            this.XuatLapRapDetailDtoGridControl.DataSource = this.xuatLapRapDetailDtoBindingSource;
            this.XuatLapRapDetailDtoGridControl.Location = new System.Drawing.Point(12, 60);
            this.XuatLapRapDetailDtoGridControl.MainView = this.XuatLapRapDetailDtoGridView;
            this.XuatLapRapDetailDtoGridControl.MenuManager = this.barManager1;
            this.XuatLapRapDetailDtoGridControl.Name = "XuatLapRapDetailDtoGridControl";
            this.XuatLapRapDetailDtoGridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.ProductVariantSearchLookUpEdit});
            this.XuatLapRapDetailDtoGridControl.Size = new System.Drawing.Size(642, 564);
            this.XuatLapRapDetailDtoGridControl.TabIndex = 5;
            this.XuatLapRapDetailDtoGridControl.UseEmbeddedNavigator = true;
            this.XuatLapRapDetailDtoGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.XuatLapRapDetailDtoGridView});
            // 
            // XuatLapRapDetailDtoGridView
            // 
            this.XuatLapRapDetailDtoGridView.Appearance.ViewCaption.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.XuatLapRapDetailDtoGridView.Appearance.ViewCaption.ForeColor = DevExpress.LookAndFeel.DXSkinColors.ForeColors.Information;
            this.XuatLapRapDetailDtoGridView.Appearance.ViewCaption.Options.UseFont = true;
            this.XuatLapRapDetailDtoGridView.Appearance.ViewCaption.Options.UseForeColor = true;
            this.XuatLapRapDetailDtoGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colFullNameHtml,
            this.colStockOutQty});
            this.XuatLapRapDetailDtoGridView.GridControl = this.XuatLapRapDetailDtoGridControl;
            this.XuatLapRapDetailDtoGridView.IndicatorWidth = 50;
            this.XuatLapRapDetailDtoGridView.Name = "XuatLapRapDetailDtoGridView";
            this.XuatLapRapDetailDtoGridView.OptionsNavigation.AutoFocusNewRow = true;
            this.XuatLapRapDetailDtoGridView.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top;
            this.XuatLapRapDetailDtoGridView.OptionsView.RowAutoHeight = true;
            this.XuatLapRapDetailDtoGridView.OptionsView.ShowFooter = true;
            this.XuatLapRapDetailDtoGridView.OptionsView.ShowGroupPanel = false;
            this.XuatLapRapDetailDtoGridView.OptionsView.ShowViewCaption = true;
            this.XuatLapRapDetailDtoGridView.ViewCaption = "DANH SÁCH HÀNG HÓA NHẬP LẮP RÁP";
            // 
            // ProductVariantSearchLookUpEdit
            // 
            this.ProductVariantSearchLookUpEdit.AutoHeight = false;
            this.ProductVariantSearchLookUpEdit.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ProductVariantSearchLookUpEdit.DataSource = this.productVariantListDtoBindingSource;
            this.ProductVariantSearchLookUpEdit.DisplayMember = "Name";
            this.ProductVariantSearchLookUpEdit.KeyMember = "Name";
            this.ProductVariantSearchLookUpEdit.Name = "ProductVariantSearchLookUpEdit";
            this.ProductVariantSearchLookUpEdit.PopupView = this.ProductVariantSearchLookUpEdit1View;
            this.ProductVariantSearchLookUpEdit.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.VariantFullNameHtmlHypertextLabel});
            this.ProductVariantSearchLookUpEdit.ValueMember = "Name";
            // 
            // ProductVariantSearchLookUpEdit1View
            // 
            this.ProductVariantSearchLookUpEdit1View.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colVariantFullName});
            this.ProductVariantSearchLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.ProductVariantSearchLookUpEdit1View.Name = "ProductVariantSearchLookUpEdit1View";
            this.ProductVariantSearchLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.ProductVariantSearchLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // PhieuXuatLapRapLookupEdit
            // 
            this.PhieuXuatLapRapLookupEdit.Location = new System.Drawing.Point(173, 12);
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
            this.PhieuXuatLapRapLookupEdit.Size = new System.Drawing.Size(481, 20);
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
            this.colStockOutInfoHtml.Caption = "Thông tin phiếu nhập";
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
            this.ProductServiceLookupEdit.Location = new System.Drawing.Point(173, 36);
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
            this.ProductServiceLookupEdit.Size = new System.Drawing.Size(481, 20);
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
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.ItemForProductName,
            this.simpleLabelItem1,
            this.simpleLabelItem3,
            this.ItemForUnitName,
            this.layoutControlItem1});
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
            rowDefinition3.Height = 568D;
            rowDefinition3.SizeType = System.Windows.Forms.SizeType.AutoSize;
            this.Root.OptionsTableLayoutGroup.RowDefinitions.AddRange(new DevExpress.XtraLayout.RowDefinition[] {
            rowDefinition1,
            rowDefinition2,
            rowDefinition3});
            this.Root.Size = new System.Drawing.Size(666, 636);
            this.Root.TextVisible = false;
            // 
            // ItemForProductName
            // 
            this.ItemForProductName.Control = this.PhieuXuatLapRapLookupEdit;
            this.ItemForProductName.Location = new System.Drawing.Point(161, 0);
            this.ItemForProductName.Name = "ItemForProductName";
            this.ItemForProductName.OptionsTableLayoutItem.ColumnIndex = 1;
            this.ItemForProductName.OptionsTableLayoutItem.ColumnSpan = 3;
            this.ItemForProductName.Size = new System.Drawing.Size(485, 24);
            this.ItemForProductName.Text = "Sản phẩm dịch vụ";
            this.ItemForProductName.TextVisible = false;
            // 
            // simpleLabelItem1
            // 
            this.simpleLabelItem1.Location = new System.Drawing.Point(0, 0);
            this.simpleLabelItem1.Name = "simpleLabelItem1";
            this.simpleLabelItem1.Size = new System.Drawing.Size(161, 24);
            this.simpleLabelItem1.Text = "Phiếu xuất lắp ráp";
            this.simpleLabelItem1.TextSize = new System.Drawing.Size(87, 13);
            // 
            // simpleLabelItem3
            // 
            this.simpleLabelItem3.Location = new System.Drawing.Point(0, 24);
            this.simpleLabelItem3.Name = "simpleLabelItem3";
            this.simpleLabelItem3.OptionsTableLayoutItem.RowIndex = 1;
            this.simpleLabelItem3.Size = new System.Drawing.Size(161, 24);
            this.simpleLabelItem3.Text = "Tên sản phẩm";
            this.simpleLabelItem3.TextSize = new System.Drawing.Size(87, 13);
            // 
            // ItemForUnitName
            // 
            this.ItemForUnitName.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.ItemForUnitName.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.ItemForUnitName.Control = this.ProductServiceLookupEdit;
            this.ItemForUnitName.Location = new System.Drawing.Point(161, 24);
            this.ItemForUnitName.Name = "ItemForUnitName";
            this.ItemForUnitName.OptionsTableLayoutItem.ColumnIndex = 1;
            this.ItemForUnitName.OptionsTableLayoutItem.ColumnSpan = 3;
            this.ItemForUnitName.OptionsTableLayoutItem.RowIndex = 1;
            this.ItemForUnitName.Size = new System.Drawing.Size(485, 24);
            this.ItemForUnitName.Text = "Đơn vị tính";
            this.ItemForUnitName.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.XuatLapRapDetailDtoGridControl;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 48);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.OptionsTableLayoutItem.ColumnSpan = 4;
            this.layoutControlItem1.OptionsTableLayoutItem.RowIndex = 2;
            this.layoutControlItem1.Size = new System.Drawing.Size(646, 568);
            this.layoutControlItem1.TextVisible = false;
            // 
            // xuatLapRapDetailDtoBindingSource
            // 
            this.xuatLapRapDetailDtoBindingSource.DataSource = typeof(DTO.Inventory.StockOut.XuatLapRap.XuatLapRapDetailDto);
            // 
            // colFullNameHtml
            // 
            this.colFullNameHtml.AppearanceCell.Options.UseTextOptions = true;
            this.colFullNameHtml.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colFullNameHtml.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colFullNameHtml.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colFullNameHtml.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colFullNameHtml.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colFullNameHtml.AppearanceHeader.Options.UseBackColor = true;
            this.colFullNameHtml.AppearanceHeader.Options.UseFont = true;
            this.colFullNameHtml.AppearanceHeader.Options.UseForeColor = true;
            this.colFullNameHtml.AppearanceHeader.Options.UseTextOptions = true;
            this.colFullNameHtml.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colFullNameHtml.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colFullNameHtml.Caption = "Tên hàng hóa";
            this.colFullNameHtml.ColumnEdit = this.ProductVariantSearchLookUpEdit;
            this.colFullNameHtml.FieldName = "FullNameHtml";
            this.colFullNameHtml.Name = "colFullNameHtml";
            this.colFullNameHtml.OptionsColumn.AllowEdit = false;
            this.colFullNameHtml.OptionsColumn.ReadOnly = true;
            this.colFullNameHtml.Visible = true;
            this.colFullNameHtml.VisibleIndex = 0;
            this.colFullNameHtml.Width = 245;
            // 
            // colStockOutQty
            // 
            this.colStockOutQty.AppearanceCell.Options.UseTextOptions = true;
            this.colStockOutQty.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.colStockOutQty.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colStockOutQty.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colStockOutQty.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colStockOutQty.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colStockOutQty.AppearanceHeader.Options.UseBackColor = true;
            this.colStockOutQty.AppearanceHeader.Options.UseFont = true;
            this.colStockOutQty.AppearanceHeader.Options.UseForeColor = true;
            this.colStockOutQty.AppearanceHeader.Options.UseTextOptions = true;
            this.colStockOutQty.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colStockOutQty.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colStockOutQty.Caption = "Số lượng nhập";
            this.colStockOutQty.DisplayFormat.FormatString = "N2";
            this.colStockOutQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colStockOutQty.FieldName = "StockOutQty";
            this.colStockOutQty.Name = "colStockOutQty";
            this.colStockOutQty.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "StockOutQty", "Tổng: {0:N2}")});
            this.colStockOutQty.ColumnEdit = this.StockOutQtyTextEdit;
            this.colStockOutQty.Visible = true;
            this.colStockOutQty.VisibleIndex = 1;
            this.colStockOutQty.Width = 118;
            // 
            // StockOutQtyTextEdit
            // 
            this.StockOutQtyTextEdit.AutoHeight = false;
            this.StockOutQtyTextEdit.DisplayFormat.FormatString = "N2";
            this.StockOutQtyTextEdit.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.StockOutQtyTextEdit.EditFormat.FormatString = "N2";
            this.StockOutQtyTextEdit.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.StockOutQtyTextEdit.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.NumericMaskManager));
            this.StockOutQtyTextEdit.Name = "StockOutQtyTextEdit";
            // 
            // productVariantListDtoBindingSource
            // 
            this.productVariantListDtoBindingSource.DataSource = typeof(DTO.MasterData.ProductService.ProductVariantListDto);
            // 
            // colVariantFullName
            // 
            this.colVariantFullName.ColumnEdit = this.VariantFullNameHtmlHypertextLabel;
            this.colVariantFullName.FieldName = "VariantFullName";
            this.colVariantFullName.Name = "colVariantFullName";
            this.colVariantFullName.Visible = true;
            this.colVariantFullName.VisibleIndex = 0;
            // 
            // VariantFullNameHtmlHypertextLabel
            // 
            this.VariantFullNameHtmlHypertextLabel.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.VariantFullNameHtmlHypertextLabel.Name = "VariantFullNameHtmlHypertextLabel";
            // 
            // FrmTaoThietBiNhapLapRap
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
            this.Name = "FrmTaoThietBiNhapLapRap";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tạo thiết bị lắp ráp từ phiếu xuất lắp ráp";
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).EndInit();
            this.dataLayoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.XuatLapRapDetailDtoGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.XuatLapRapDetailDtoGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductVariantSearchLookUpEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductVariantSearchLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StockOutQtyTextEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PhieuXuatLapRapLookupEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xuatLapRapMasterListDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PhieuNhapLapRapHypertextLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductServiceLookupEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.productServiceDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductServiceGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductServiceHypertextLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForProductName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForUnitName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xuatLapRapDetailDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.productVariantListDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VariantFullNameHtmlHypertextLabel)).EndInit();
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
        private GridControl XuatLapRapDetailDtoGridControl;
        private GridView XuatLapRapDetailDtoGridView;
        private LayoutControlItem ItemForProductName;
        private SimpleLabelItem simpleLabelItem1;
        private RepositoryItemSearchLookUpEdit ProductVariantSearchLookUpEdit;
        private GridView ProductVariantSearchLookUpEdit1View;
        private SimpleLabelItem simpleLabelItem3;
        private LayoutControlItem ItemForUnitName;
        private BindingSource xuatLapRapMasterListDtoBindingSource;
        private GridColumn colStockOutInfoHtml;
        private RepositoryItemHypertextLabel PhieuNhapLapRapHypertextLabel;
        private GridColumn colThongTinHtml;
        private RepositoryItemHypertextLabel ProductServiceHypertextLabel;
        private LayoutControlItem layoutControlItem1;
        private BindingSource xuatLapRapDetailDtoBindingSource;
        private GridColumn colFullNameHtml;
        private GridColumn colStockOutQty;
        private RepositoryItemTextEdit StockOutQtyTextEdit;
        private BindingSource productVariantListDtoBindingSource;
        private GridColumn colVariantFullName;
        private RepositoryItemHypertextLabel VariantFullNameHtmlHypertextLabel;
    }
}