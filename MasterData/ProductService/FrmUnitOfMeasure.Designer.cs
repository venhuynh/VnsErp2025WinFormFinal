using System.ComponentModel;
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

namespace MasterData.ProductService
{
    partial class FrmUnitOfMeasure
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            this.AddNewBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.SaveBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.EditBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.DeleteBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.RefreshBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
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
            this.AttributeGridControl = new DevExpress.XtraGrid.GridControl();
            this.unitOfMeasureDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.AttributeGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colDisplayHtml = new DevExpress.XtraGrid.Columns.GridColumn();
            this.HtmlContentHypertextLabel = new DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel();
            this.repositoryItemMemoEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
            this.CodeTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.NameTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.DescriptionTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.IsActiveCheckEdit = new DevExpress.XtraEditors.CheckEdit();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.ItemForCode = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForName = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForDescription = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForIsActive = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).BeginInit();
            this.dataLayoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AttributeGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.unitOfMeasureDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AttributeGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HtmlContentHypertextLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CodeTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NameTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DescriptionTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.IsActiveCheckEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForDescription)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForIsActive)).BeginInit();
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
            this.DeleteBarButtonItem,
            this.EditBarButtonItem,
            this.RefreshBarButtonItem,
            this.AddNewBarButtonItem});
            this.barManager1.MainMenu = this.bar2;
            this.barManager1.MaxItemId = 7;
            // 
            // bar2
            // 
            this.bar2.BarName = "Main menu";
            this.bar2.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Top;
            this.bar2.DockCol = 0;
            this.bar2.DockRow = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.AddNewBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.SaveBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.EditBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.DeleteBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.RefreshBarButtonItem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar2.OptionsBar.MultiLine = true;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Main menu";
            // 
            // AddNewBarButtonItem
            // 
            this.AddNewBarButtonItem.Caption = "Thêm mới";
            this.AddNewBarButtonItem.Id = 6;
            this.AddNewBarButtonItem.ImageOptions.Image = global::MasterData.Properties.Resources.addnewdatasource_16x16;
            this.AddNewBarButtonItem.Name = "AddNewBarButtonItem";
            // 
            // SaveBarButtonItem
            // 
            this.SaveBarButtonItem.Caption = "Lưu";
            this.SaveBarButtonItem.Id = 0;
            this.SaveBarButtonItem.ImageOptions.Image = global::MasterData.Properties.Resources.save_16x16;
            this.SaveBarButtonItem.ImageOptions.LargeImage = global::MasterData.Properties.Resources.save_32x32;
            this.SaveBarButtonItem.Name = "SaveBarButtonItem";
            // 
            // EditBarButtonItem
            // 
            this.EditBarButtonItem.Caption = "Điều chỉnh";
            this.EditBarButtonItem.Id = 4;
            this.EditBarButtonItem.ImageOptions.Image = global::MasterData.Properties.Resources.edittask_16x16;
            this.EditBarButtonItem.Name = "EditBarButtonItem";
            // 
            // DeleteBarButtonItem
            // 
            this.DeleteBarButtonItem.Caption = "Xóa";
            this.DeleteBarButtonItem.Id = 1;
            this.DeleteBarButtonItem.ImageOptions.Image = global::MasterData.Properties.Resources.cancel_16x16;
            this.DeleteBarButtonItem.ImageOptions.LargeImage = global::MasterData.Properties.Resources.cancel_32x32;
            this.DeleteBarButtonItem.Name = "DeleteBarButtonItem";
            // 
            // RefreshBarButtonItem
            // 
            this.RefreshBarButtonItem.Caption = "Làm mới";
            this.RefreshBarButtonItem.Id = 5;
            this.RefreshBarButtonItem.ImageOptions.Image = global::MasterData.Properties.Resources.recurrence_16x16;
            this.RefreshBarButtonItem.ImageOptions.LargeImage = global::MasterData.Properties.Resources.recurrence_32x32;
            this.RefreshBarButtonItem.Name = "RefreshBarButtonItem";
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
            this.dataLayoutControl1.Controls.Add(this.AttributeGridControl);
            this.dataLayoutControl1.Controls.Add(this.CodeTextEdit);
            this.dataLayoutControl1.Controls.Add(this.NameTextEdit);
            this.dataLayoutControl1.Controls.Add(this.DescriptionTextEdit);
            this.dataLayoutControl1.Controls.Add(this.IsActiveCheckEdit);
            this.dataLayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataLayoutControl1.Location = new System.Drawing.Point(0, 24);
            this.dataLayoutControl1.Name = "dataLayoutControl1";
            this.dataLayoutControl1.Root = this.Root;
            this.dataLayoutControl1.Size = new System.Drawing.Size(666, 636);
            this.dataLayoutControl1.TabIndex = 15;
            this.dataLayoutControl1.Text = "dataLayoutControl1";
            // 
            // AttributeGridControl
            // 
            this.AttributeGridControl.DataSource = this.unitOfMeasureDtoBindingSource;
            this.AttributeGridControl.Location = new System.Drawing.Point(12, 108);
            this.AttributeGridControl.MainView = this.AttributeGridView;
            this.AttributeGridControl.MenuManager = this.barManager1;
            this.AttributeGridControl.Name = "AttributeGridControl";
            this.AttributeGridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemMemoEdit1,
            this.HtmlContentHypertextLabel});
            this.AttributeGridControl.Size = new System.Drawing.Size(642, 516);
            this.AttributeGridControl.TabIndex = 4;
            this.AttributeGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.AttributeGridView});
            // 
            // unitOfMeasureDtoBindingSource
            // 
            this.unitOfMeasureDtoBindingSource.DataSource = typeof(DTO.MasterData.ProductService.UnitOfMeasureDto);
            // 
            // AttributeGridView
            // 
            this.AttributeGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colDisplayHtml});
            this.AttributeGridView.GridControl = this.AttributeGridControl;
            this.AttributeGridView.IndicatorWidth = 50;
            this.AttributeGridView.Name = "AttributeGridView";
            this.AttributeGridView.OptionsDetail.EnableMasterViewMode = false;
            this.AttributeGridView.OptionsFind.AlwaysVisible = true;
            this.AttributeGridView.OptionsSelection.MultiSelect = true;
            this.AttributeGridView.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            this.AttributeGridView.OptionsView.RowAutoHeight = true;
            this.AttributeGridView.OptionsView.ShowAutoFilterRow = true;
            this.AttributeGridView.OptionsView.ShowGroupPanel = false;
            // 
            // colDisplayHtml
            // 
            this.colDisplayHtml.AppearanceCell.Options.UseTextOptions = true;
            this.colDisplayHtml.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colDisplayHtml.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colDisplayHtml.AppearanceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.colDisplayHtml.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colDisplayHtml.AppearanceHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.colDisplayHtml.AppearanceHeader.Options.UseBackColor = true;
            this.colDisplayHtml.AppearanceHeader.Options.UseFont = true;
            this.colDisplayHtml.AppearanceHeader.Options.UseForeColor = true;
            this.colDisplayHtml.AppearanceHeader.Options.UseTextOptions = true;
            this.colDisplayHtml.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colDisplayHtml.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colDisplayHtml.Caption = "Thông tin đơn vị tính";
            this.colDisplayHtml.ColumnEdit = this.HtmlContentHypertextLabel;
            this.colDisplayHtml.FieldName = "DisplayHtml";
            this.colDisplayHtml.MinWidth = 300;
            this.colDisplayHtml.Name = "colDisplayHtml";
            this.colDisplayHtml.Visible = true;
            this.colDisplayHtml.VisibleIndex = 1;
            this.colDisplayHtml.Width = 500;
            // 
            // HtmlContentHypertextLabel
            // 
            this.HtmlContentHypertextLabel.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.HtmlContentHypertextLabel.Name = "HtmlContentHypertextLabel";
            // 
            // repositoryItemMemoEdit1
            // 
            this.repositoryItemMemoEdit1.Appearance.Options.UseTextOptions = true;
            this.repositoryItemMemoEdit1.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.repositoryItemMemoEdit1.Name = "repositoryItemMemoEdit1";
            // 
            // CodeTextEdit
            // 
            this.CodeTextEdit.Location = new System.Drawing.Point(73, 12);
            this.CodeTextEdit.MenuManager = this.barManager1;
            this.CodeTextEdit.Name = "CodeTextEdit";
            this.CodeTextEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.CodeTextEdit.Size = new System.Drawing.Size(581, 20);
            this.CodeTextEdit.StyleController = this.dataLayoutControl1;
            this.CodeTextEdit.TabIndex = 0;
            // 
            // NameTextEdit
            // 
            this.NameTextEdit.Location = new System.Drawing.Point(73, 36);
            this.NameTextEdit.MenuManager = this.barManager1;
            this.NameTextEdit.Name = "NameTextEdit";
            this.NameTextEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.NameTextEdit.Size = new System.Drawing.Size(581, 20);
            this.NameTextEdit.StyleController = this.dataLayoutControl1;
            this.NameTextEdit.TabIndex = 2;
            // 
            // DescriptionTextEdit
            // 
            this.DescriptionTextEdit.Location = new System.Drawing.Point(73, 60);
            this.DescriptionTextEdit.MenuManager = this.barManager1;
            this.DescriptionTextEdit.Name = "DescriptionTextEdit";
            this.DescriptionTextEdit.Size = new System.Drawing.Size(581, 20);
            this.DescriptionTextEdit.StyleController = this.dataLayoutControl1;
            this.DescriptionTextEdit.TabIndex = 3;
            // 
            // IsActiveCheckEdit
            // 
            this.IsActiveCheckEdit.Location = new System.Drawing.Point(73, 84);
            this.IsActiveCheckEdit.MenuManager = this.barManager1;
            this.IsActiveCheckEdit.Name = "IsActiveCheckEdit";
            this.IsActiveCheckEdit.Properties.Caption = "Hoạt động";
            this.IsActiveCheckEdit.Size = new System.Drawing.Size(581, 20);
            this.IsActiveCheckEdit.StyleController = this.dataLayoutControl1;
            this.IsActiveCheckEdit.TabIndex = 5;
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup1,
            this.layoutControlItem1});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(666, 636);
            this.Root.TextVisible = false;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.AllowDrawBackground = false;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.ItemForCode,
            this.ItemForName,
            this.ItemForDescription,
            this.ItemForIsActive});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "autoGeneratedGroup0";
            this.layoutControlGroup1.Size = new System.Drawing.Size(646, 96);
            // 
            // ItemForCode
            // 
            this.ItemForCode.Control = this.CodeTextEdit;
            this.ItemForCode.Location = new System.Drawing.Point(0, 0);
            this.ItemForCode.Name = "ItemForCode";
            this.ItemForCode.Size = new System.Drawing.Size(646, 24);
            this.ItemForCode.Text = "Mã ĐVT";
            this.ItemForCode.TextSize = new System.Drawing.Size(49, 13);
            // 
            // ItemForName
            // 
            this.ItemForName.Control = this.NameTextEdit;
            this.ItemForName.Location = new System.Drawing.Point(0, 24);
            this.ItemForName.Name = "ItemForName";
            this.ItemForName.Size = new System.Drawing.Size(646, 24);
            this.ItemForName.Text = "Tên ĐVT";
            this.ItemForName.TextSize = new System.Drawing.Size(49, 13);
            // 
            // ItemForDescription
            // 
            this.ItemForDescription.Control = this.DescriptionTextEdit;
            this.ItemForDescription.Location = new System.Drawing.Point(0, 48);
            this.ItemForDescription.Name = "ItemForDescription";
            this.ItemForDescription.Size = new System.Drawing.Size(646, 24);
            this.ItemForDescription.Text = "Mô tả";
            this.ItemForDescription.TextSize = new System.Drawing.Size(49, 13);
            // 
            // ItemForIsActive
            // 
            this.ItemForIsActive.Control = this.IsActiveCheckEdit;
            this.ItemForIsActive.Location = new System.Drawing.Point(0, 72);
            this.ItemForIsActive.Name = "ItemForIsActive";
            this.ItemForIsActive.Size = new System.Drawing.Size(646, 24);
            this.ItemForIsActive.Text = "Trạng thái";
            this.ItemForIsActive.TextSize = new System.Drawing.Size(49, 13);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.AttributeGridControl;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 96);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(646, 520);
            this.layoutControlItem1.TextVisible = false;
            // 
            // FrmUnitOfMeasure
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
            this.Name = "FrmUnitOfMeasure";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DANH SÁCH CÁC ĐƠN VỊ TÍNH";
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).EndInit();
            this.dataLayoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.AttributeGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.unitOfMeasureDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AttributeGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HtmlContentHypertextLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CodeTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NameTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DescriptionTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.IsActiveCheckEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForDescription)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForIsActive)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private BarManager barManager1;
        private Bar bar2;
        private BarButtonItem SaveBarButtonItem;
        private BarButtonItem DeleteBarButtonItem;
        private BarButtonItem AddNewBarButtonItem;
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
        private BarButtonItem EditBarButtonItem;
        private GridControl AttributeGridControl;
        private GridView AttributeGridView;
        private TextEdit CodeTextEdit;
        private TextEdit NameTextEdit;
        private TextEdit DescriptionTextEdit;
        private CheckEdit IsActiveCheckEdit;
        private LayoutControlGroup layoutControlGroup1;
        private LayoutControlItem ItemForCode;
        private LayoutControlItem ItemForName;
        private LayoutControlItem ItemForDescription;
        private LayoutControlItem ItemForIsActive;
        private LayoutControlItem layoutControlItem1;
        private BarButtonItem RefreshBarButtonItem;
        private BindingSource unitOfMeasureDtoBindingSource;
        private RepositoryItemMemoEdit repositoryItemMemoEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel HtmlContentHypertextLabel;
        private GridColumn colDisplayHtml;
    }
}
