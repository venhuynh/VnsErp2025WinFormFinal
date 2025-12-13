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
            DevExpress.XtraLayout.RowDefinition rowDefinition5 = new DevExpress.XtraLayout.RowDefinition();
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
            this.xtraOpenFileDialog1 = new DevExpress.XtraEditors.XtraOpenFileDialog(this.components);
            this.dataLayoutControl1 = new DevExpress.XtraDataLayout.DataLayoutControl();
            this.AttributeValueGridControl = new DevExpress.XtraGrid.GridControl();
            this.attributeValueDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.AttributeValueGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colValue = new DevExpress.XtraGrid.Columns.GridColumn();
            this.AttributeSearchLookUpEdit = new DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit();
            this.attributeDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.AttributeSearchLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colFullInfo1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAttributeName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PhieuXuatLapRapLookupEdit = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.xuatLapRapMasterListDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.searchLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.VariantCodeTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.IsActiveToggleSwitch = new DevExpress.XtraEditors.ToggleSwitch();
            this.UnitNameSearchLookupEdit = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.unitOfMeasureDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colFullInfo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.ItemForProductName = new DevExpress.XtraLayout.LayoutControlItem();
            this.simpleLabelItem1 = new DevExpress.XtraLayout.SimpleLabelItem();
            this.simpleLabelItem2 = new DevExpress.XtraLayout.SimpleLabelItem();
            this.ItemForVariantCode = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForIsActive = new DevExpress.XtraLayout.LayoutControlItem();
            this.simpleLabelItem3 = new DevExpress.XtraLayout.SimpleLabelItem();
            this.ItemForUnitName = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.productServiceDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.xuatLapRapMasterDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.colStockOutInfoHtml = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PhieuNhapLapRapHypertextLabel = new DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).BeginInit();
            this.dataLayoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AttributeValueGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.attributeValueDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AttributeValueGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AttributeSearchLookUpEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.attributeDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AttributeSearchLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PhieuXuatLapRapLookupEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xuatLapRapMasterListDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VariantCodeTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.IsActiveToggleSwitch.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UnitNameSearchLookupEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.unitOfMeasureDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForProductName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForVariantCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForIsActive)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForUnitName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.productServiceDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xuatLapRapMasterDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PhieuNhapLapRapHypertextLabel)).BeginInit();
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
            // xtraOpenFileDialog1
            // 
            this.xtraOpenFileDialog1.Filter = "Image Files (*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.webp)|*.jpg;*.jpeg;*.png;*.bmp;*.gi" +
    "f;*.webp|All Files (*.*)|*.*";
            this.xtraOpenFileDialog1.Title = "Chọn hình ảnh cho sản phẩm/dịch vụ";
            // 
            // dataLayoutControl1
            // 
            this.dataLayoutControl1.Controls.Add(this.AttributeValueGridControl);
            this.dataLayoutControl1.Controls.Add(this.PhieuXuatLapRapLookupEdit);
            this.dataLayoutControl1.Controls.Add(this.VariantCodeTextEdit);
            this.dataLayoutControl1.Controls.Add(this.IsActiveToggleSwitch);
            this.dataLayoutControl1.Controls.Add(this.UnitNameSearchLookupEdit);
            this.dataLayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataLayoutControl1.Location = new System.Drawing.Point(0, 24);
            this.dataLayoutControl1.Name = "dataLayoutControl1";
            this.dataLayoutControl1.Root = this.Root;
            this.dataLayoutControl1.Size = new System.Drawing.Size(666, 636);
            this.dataLayoutControl1.TabIndex = 15;
            this.dataLayoutControl1.Text = "dataLayoutControl1";
            // 
            // AttributeValueGridControl
            // 
            this.AttributeValueGridControl.DataSource = this.attributeValueDtoBindingSource;
            this.AttributeValueGridControl.Location = new System.Drawing.Point(12, 104);
            this.AttributeValueGridControl.MainView = this.AttributeValueGridView;
            this.AttributeValueGridControl.MenuManager = this.barManager1;
            this.AttributeValueGridControl.Name = "AttributeValueGridControl";
            this.AttributeValueGridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.AttributeSearchLookUpEdit});
            this.AttributeValueGridControl.Size = new System.Drawing.Size(642, 520);
            this.AttributeValueGridControl.TabIndex = 5;
            this.AttributeValueGridControl.UseEmbeddedNavigator = true;
            this.AttributeValueGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.AttributeValueGridView});
            // 
            // attributeValueDtoBindingSource
            // 
            this.attributeValueDtoBindingSource.DataSource = typeof(DTO.MasterData.ProductService.AttributeValueDto);
            // 
            // AttributeValueGridView
            // 
            this.AttributeValueGridView.Appearance.ViewCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.AttributeValueGridView.Appearance.ViewCaption.ForeColor = System.Drawing.Color.Blue;
            this.AttributeValueGridView.Appearance.ViewCaption.Options.UseFont = true;
            this.AttributeValueGridView.Appearance.ViewCaption.Options.UseForeColor = true;
            this.AttributeValueGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colValue,
            this.colAttributeName});
            this.AttributeValueGridView.GridControl = this.AttributeValueGridControl;
            this.AttributeValueGridView.Name = "AttributeValueGridView";
            this.AttributeValueGridView.OptionsNavigation.AutoFocusNewRow = true;
            this.AttributeValueGridView.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top;
            this.AttributeValueGridView.OptionsView.ShowGroupPanel = false;
            this.AttributeValueGridView.OptionsView.ShowViewCaption = true;
            this.AttributeValueGridView.ViewCaption = "DANH SÁCH CÁC THUỘC TÍNH BIẾN THỂ";
            // 
            // colValue
            // 
            this.colValue.ColumnEdit = this.AttributeSearchLookUpEdit;
            this.colValue.FieldName = "AttributeName";
            this.colValue.Name = "colValue";
            this.colValue.Visible = true;
            this.colValue.VisibleIndex = 0;
            // 
            // AttributeSearchLookUpEdit
            // 
            this.AttributeSearchLookUpEdit.AutoHeight = false;
            this.AttributeSearchLookUpEdit.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.AttributeSearchLookUpEdit.DataSource = this.attributeDtoBindingSource;
            this.AttributeSearchLookUpEdit.DisplayMember = "Name";
            this.AttributeSearchLookUpEdit.KeyMember = "Name";
            this.AttributeSearchLookUpEdit.Name = "AttributeSearchLookUpEdit";
            this.AttributeSearchLookUpEdit.PopupView = this.AttributeSearchLookUpEdit1View;
            this.AttributeSearchLookUpEdit.ValueMember = "Name";
            // 
            // attributeDtoBindingSource
            // 
            this.attributeDtoBindingSource.DataSource = typeof(DTO.MasterData.ProductService.AttributeDto);
            // 
            // AttributeSearchLookUpEdit1View
            // 
            this.AttributeSearchLookUpEdit1View.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colFullInfo1});
            this.AttributeSearchLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.AttributeSearchLookUpEdit1View.Name = "AttributeSearchLookUpEdit1View";
            this.AttributeSearchLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.AttributeSearchLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // colFullInfo1
            // 
            this.colFullInfo1.FieldName = "FullInfo";
            this.colFullInfo1.Name = "colFullInfo1";
            this.colFullInfo1.Visible = true;
            this.colFullInfo1.VisibleIndex = 0;
            // 
            // colAttributeName
            // 
            this.colAttributeName.FieldName = "Value";
            this.colAttributeName.Name = "colAttributeName";
            this.colAttributeName.Visible = true;
            this.colAttributeName.VisibleIndex = 1;
            // 
            // PhieuXuatLapRapLookupEdit
            // 
            this.PhieuXuatLapRapLookupEdit.Location = new System.Drawing.Point(173, 12);
            this.PhieuXuatLapRapLookupEdit.MenuManager = this.barManager1;
            this.PhieuXuatLapRapLookupEdit.Name = "PhieuXuatLapRapLookupEdit";
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
            this.searchLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            this.searchLookUpEdit1View.OptionsView.ColumnAutoWidth = false;
            // 
            // VariantCodeTextEdit
            // 
            this.VariantCodeTextEdit.Location = new System.Drawing.Point(173, 60);
            this.VariantCodeTextEdit.MenuManager = this.barManager1;
            this.VariantCodeTextEdit.Name = "VariantCodeTextEdit";
            this.VariantCodeTextEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.VariantCodeTextEdit.Size = new System.Drawing.Size(318, 20);
            this.VariantCodeTextEdit.StyleController = this.dataLayoutControl1;
            this.VariantCodeTextEdit.TabIndex = 3;
            // 
            // IsActiveToggleSwitch
            // 
            this.IsActiveToggleSwitch.EditValue = true;
            this.IsActiveToggleSwitch.Location = new System.Drawing.Point(495, 61);
            this.IsActiveToggleSwitch.MenuManager = this.barManager1;
            this.IsActiveToggleSwitch.Name = "IsActiveToggleSwitch";
            this.IsActiveToggleSwitch.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.IsActiveToggleSwitch.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Default;
            this.IsActiveToggleSwitch.Properties.OffText = "<color=\'red\'>Không sử dụng</color>";
            this.IsActiveToggleSwitch.Properties.OnText = "<color=\'blue\'>Đang sử dụng</color>";
            this.IsActiveToggleSwitch.Size = new System.Drawing.Size(159, 18);
            this.IsActiveToggleSwitch.StyleController = this.dataLayoutControl1;
            this.IsActiveToggleSwitch.TabIndex = 4;
            this.IsActiveToggleSwitch.ToolTip = "Trạng thái hoạt động của biến thể";
            // 
            // UnitNameSearchLookupEdit
            // 
            this.UnitNameSearchLookupEdit.Location = new System.Drawing.Point(173, 36);
            this.UnitNameSearchLookupEdit.MenuManager = this.barManager1;
            this.UnitNameSearchLookupEdit.Name = "UnitNameSearchLookupEdit";
            this.UnitNameSearchLookupEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.UnitNameSearchLookupEdit.Properties.DataSource = this.unitOfMeasureDtoBindingSource;
            this.UnitNameSearchLookupEdit.Properties.DisplayMember = "FullInfo";
            this.UnitNameSearchLookupEdit.Properties.NullText = "";
            this.UnitNameSearchLookupEdit.Properties.PopupView = this.gridView1;
            this.UnitNameSearchLookupEdit.Properties.ValueMember = "Id";
            this.UnitNameSearchLookupEdit.Size = new System.Drawing.Size(481, 20);
            this.UnitNameSearchLookupEdit.StyleController = this.dataLayoutControl1;
            this.UnitNameSearchLookupEdit.TabIndex = 2;
            // 
            // unitOfMeasureDtoBindingSource
            // 
            this.unitOfMeasureDtoBindingSource.DataSource = typeof(DTO.MasterData.ProductService.UnitOfMeasureDto);
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colFullInfo});
            this.gridView1.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // colFullInfo
            // 
            this.colFullInfo.FieldName = "FullInfo";
            this.colFullInfo.Name = "colFullInfo";
            this.colFullInfo.Visible = true;
            this.colFullInfo.VisibleIndex = 0;
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.ItemForProductName,
            this.simpleLabelItem1,
            this.simpleLabelItem2,
            this.ItemForVariantCode,
            this.ItemForIsActive,
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
            rowDefinition3.Height = 24D;
            rowDefinition3.SizeType = System.Windows.Forms.SizeType.AutoSize;
            rowDefinition4.Height = 20D;
            rowDefinition4.SizeType = System.Windows.Forms.SizeType.AutoSize;
            rowDefinition5.Height = 524D;
            rowDefinition5.SizeType = System.Windows.Forms.SizeType.AutoSize;
            this.Root.OptionsTableLayoutGroup.RowDefinitions.AddRange(new DevExpress.XtraLayout.RowDefinition[] {
            rowDefinition1,
            rowDefinition2,
            rowDefinition3,
            rowDefinition4,
            rowDefinition5});
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
            // simpleLabelItem2
            // 
            this.simpleLabelItem2.Location = new System.Drawing.Point(0, 48);
            this.simpleLabelItem2.Name = "simpleLabelItem2";
            this.simpleLabelItem2.OptionsTableLayoutItem.RowIndex = 2;
            this.simpleLabelItem2.Size = new System.Drawing.Size(161, 24);
            this.simpleLabelItem2.Text = "Mã biến thể";
            this.simpleLabelItem2.TextSize = new System.Drawing.Size(87, 13);
            // 
            // ItemForVariantCode
            // 
            this.ItemForVariantCode.Control = this.VariantCodeTextEdit;
            this.ItemForVariantCode.Location = new System.Drawing.Point(161, 48);
            this.ItemForVariantCode.Name = "ItemForVariantCode";
            this.ItemForVariantCode.OptionsTableLayoutItem.ColumnIndex = 1;
            this.ItemForVariantCode.OptionsTableLayoutItem.ColumnSpan = 2;
            this.ItemForVariantCode.OptionsTableLayoutItem.RowIndex = 2;
            this.ItemForVariantCode.Size = new System.Drawing.Size(322, 24);
            this.ItemForVariantCode.Text = "Mã biến thể";
            this.ItemForVariantCode.TextVisible = false;
            // 
            // ItemForIsActive
            // 
            this.ItemForIsActive.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.ItemForIsActive.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.ItemForIsActive.Control = this.IsActiveToggleSwitch;
            this.ItemForIsActive.Location = new System.Drawing.Point(483, 48);
            this.ItemForIsActive.Name = "ItemForIsActive";
            this.ItemForIsActive.OptionsTableLayoutItem.ColumnIndex = 3;
            this.ItemForIsActive.OptionsTableLayoutItem.RowIndex = 2;
            this.ItemForIsActive.OptionsToolTip.ToolTip = "Trạng thái hoạt động của biến thể";
            this.ItemForIsActive.Size = new System.Drawing.Size(163, 24);
            this.ItemForIsActive.TextVisible = false;
            // 
            // simpleLabelItem3
            // 
            this.simpleLabelItem3.Location = new System.Drawing.Point(0, 24);
            this.simpleLabelItem3.Name = "simpleLabelItem3";
            this.simpleLabelItem3.OptionsTableLayoutItem.RowIndex = 1;
            this.simpleLabelItem3.Size = new System.Drawing.Size(161, 24);
            this.simpleLabelItem3.Text = "Đơn vị tính";
            this.simpleLabelItem3.TextSize = new System.Drawing.Size(87, 13);
            // 
            // ItemForUnitName
            // 
            this.ItemForUnitName.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.ItemForUnitName.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.ItemForUnitName.Control = this.UnitNameSearchLookupEdit;
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
            this.layoutControlItem1.Control = this.AttributeValueGridControl;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 92);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.OptionsTableLayoutItem.ColumnSpan = 4;
            this.layoutControlItem1.OptionsTableLayoutItem.RowIndex = 4;
            this.layoutControlItem1.Size = new System.Drawing.Size(646, 524);
            this.layoutControlItem1.TextVisible = false;
            // 
            // productServiceDtoBindingSource
            // 
            this.productServiceDtoBindingSource.DataSource = typeof(DTO.MasterData.ProductService.ProductServiceDto);
            // 
            // xuatLapRapMasterDtoBindingSource
            // 
            this.xuatLapRapMasterDtoBindingSource.DataSource = typeof(DTO.Inventory.StockOut.XuatLapRap.XuatLapRapMasterDto);
            // 
            // colStockOutInfoHtml
            // 
            this.colStockOutInfoHtml.ColumnEdit = this.PhieuNhapLapRapHypertextLabel;
            this.colStockOutInfoHtml.FieldName = "StockInInfoHtml";
            this.colStockOutInfoHtml.Name = "colStockOutInfoHtml";
            this.colStockOutInfoHtml.Visible = true;
            this.colStockOutInfoHtml.VisibleIndex = 0;
            this.colStockOutInfoHtml.Width = 600;
            this.colStockOutInfoHtml.Caption = "Thông tin phiếu nhập";
            this.colStockOutInfoHtml.OptionsColumn.AllowEdit = false;
            this.colStockOutInfoHtml.OptionsColumn.ReadOnly = true;
            // 
            // PhieuNhapLapRapHypertextLabel
            // 
            this.PhieuNhapLapRapHypertextLabel.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.PhieuNhapLapRapHypertextLabel.Name = "PhieuNhapLapRapHypertextLabel";
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
            ((System.ComponentModel.ISupportInitialize)(this.AttributeValueGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.attributeValueDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AttributeValueGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AttributeSearchLookUpEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.attributeDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AttributeSearchLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PhieuXuatLapRapLookupEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xuatLapRapMasterListDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VariantCodeTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.IsActiveToggleSwitch.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UnitNameSearchLookupEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.unitOfMeasureDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForProductName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForVariantCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForIsActive)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForUnitName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.productServiceDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xuatLapRapMasterDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PhieuNhapLapRapHypertextLabel)).EndInit();
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
        private XtraOpenFileDialog xtraOpenFileDialog1;
        private DataLayoutControl dataLayoutControl1;
        private LayoutControlGroup Root;
        private SearchLookUpEdit PhieuXuatLapRapLookupEdit;
        private BindingSource productServiceDtoBindingSource;
        private GridView searchLookUpEdit1View;
        private TextEdit VariantCodeTextEdit;
        private ToggleSwitch IsActiveToggleSwitch;
        private SearchLookUpEdit UnitNameSearchLookupEdit;
        private BindingSource unitOfMeasureDtoBindingSource;
        private GridView gridView1;
        private GridColumn colFullInfo;
        private GridControl AttributeValueGridControl;
        private GridView AttributeValueGridView;
        private LayoutControlItem ItemForProductName;
        private BindingSource attributeValueDtoBindingSource;
        private GridColumn colValue;
        private GridColumn colAttributeName;
        private SimpleLabelItem simpleLabelItem1;
        private RepositoryItemSearchLookUpEdit AttributeSearchLookUpEdit;
        private BindingSource attributeDtoBindingSource;
        private GridView AttributeSearchLookUpEdit1View;
        private GridColumn colFullInfo1;
        private SimpleLabelItem simpleLabelItem2;
        private LayoutControlItem ItemForVariantCode;
        private LayoutControlItem ItemForIsActive;
        private SimpleLabelItem simpleLabelItem3;
        private LayoutControlItem ItemForUnitName;
        private LayoutControlItem layoutControlItem1;
        private BindingSource xuatLapRapMasterDtoBindingSource;
        private BindingSource xuatLapRapMasterListDtoBindingSource;
        private GridColumn colStockOutInfoHtml;
        private RepositoryItemHypertextLabel PhieuNhapLapRapHypertextLabel;
    }
}