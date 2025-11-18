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
    partial class FrmAttribute
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
            this.attributeDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.AttributeGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colId1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDataType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDescription1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemMemoEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
            this.NameTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.DataTypeTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.DescriptionTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.ItemForName = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForDataType = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForDescription = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).BeginInit();
            this.dataLayoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AttributeGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.attributeDtoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AttributeGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NameTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataTypeTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DescriptionTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForDataType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForDescription)).BeginInit();
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
            this.dataLayoutControl1.Controls.Add(this.NameTextEdit);
            this.dataLayoutControl1.Controls.Add(this.DataTypeTextEdit);
            this.dataLayoutControl1.Controls.Add(this.DescriptionTextEdit);
            this.dataLayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataLayoutControl1.Location = new System.Drawing.Point(0, 39);
            this.dataLayoutControl1.Name = "dataLayoutControl1";
            this.dataLayoutControl1.Root = this.Root;
            this.dataLayoutControl1.Size = new System.Drawing.Size(666, 621);
            this.dataLayoutControl1.TabIndex = 15;
            this.dataLayoutControl1.Text = "dataLayoutControl1";
            // 
            // AttributeGridControl
            // 
            this.AttributeGridControl.DataSource = this.attributeDtoBindingSource;
            this.AttributeGridControl.Location = new System.Drawing.Point(16, 118);
            this.AttributeGridControl.MainView = this.AttributeGridView;
            this.AttributeGridControl.MenuManager = this.barManager1;
            this.AttributeGridControl.Name = "AttributeGridControl";
            this.AttributeGridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemMemoEdit1});
            this.AttributeGridControl.Size = new System.Drawing.Size(634, 487);
            this.AttributeGridControl.TabIndex = 4;
            this.AttributeGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.AttributeGridView});
            // 
            // attributeDtoBindingSource
            // 
            this.attributeDtoBindingSource.DataSource = typeof(AttributeDto);
            // 
            // AttributeGridView
            // 
            this.AttributeGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colId1,
            this.colName,
            this.colDataType,
            this.colDescription1});
            this.AttributeGridView.GridControl = this.AttributeGridControl;
            this.AttributeGridView.IndicatorWidth = 50;
            this.AttributeGridView.Name = "AttributeGridView";
            this.AttributeGridView.OptionsDetail.EnableMasterViewMode = false;
            this.AttributeGridView.OptionsSelection.MultiSelect = true;
            this.AttributeGridView.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            this.AttributeGridView.OptionsView.RowAutoHeight = true;
            this.AttributeGridView.OptionsView.ShowAutoFilterRow = true;
            this.AttributeGridView.OptionsView.ShowGroupPanel = false;
            // 
            // colId1
            // 
            this.colId1.FieldName = "Id";
            this.colId1.Name = "colId1";
            // 
            // colName
            // 
            this.colName.FieldName = "Name";
            this.colName.Name = "colName";
            this.colName.Visible = true;
            this.colName.VisibleIndex = 1;
            // 
            // colDataType
            // 
            this.colDataType.FieldName = "DataType";
            this.colDataType.Name = "colDataType";
            this.colDataType.Visible = true;
            this.colDataType.VisibleIndex = 2;
            // 
            // colDescription1
            // 
            this.colDescription1.ColumnEdit = this.repositoryItemMemoEdit1;
            this.colDescription1.FieldName = "Description";
            this.colDescription1.Name = "colDescription1";
            this.colDescription1.Visible = true;
            this.colDescription1.VisibleIndex = 3;
            // 
            // repositoryItemMemoEdit1
            // 
            this.repositoryItemMemoEdit1.Appearance.Options.UseTextOptions = true;
            this.repositoryItemMemoEdit1.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.repositoryItemMemoEdit1.Name = "repositoryItemMemoEdit1";
            // 
            // NameTextEdit
            // 
            this.NameTextEdit.Location = new System.Drawing.Point(101, 16);
            this.NameTextEdit.MenuManager = this.barManager1;
            this.NameTextEdit.Name = "NameTextEdit";
            this.NameTextEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.NameTextEdit.Size = new System.Drawing.Size(549, 28);
            this.NameTextEdit.StyleController = this.dataLayoutControl1;
            this.NameTextEdit.TabIndex = 0;
            // 
            // DataTypeTextEdit
            // 
            this.DataTypeTextEdit.Location = new System.Drawing.Point(101, 50);
            this.DataTypeTextEdit.MenuManager = this.barManager1;
            this.DataTypeTextEdit.Name = "DataTypeTextEdit";
            this.DataTypeTextEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.DataTypeTextEdit.Size = new System.Drawing.Size(549, 28);
            this.DataTypeTextEdit.StyleController = this.dataLayoutControl1;
            this.DataTypeTextEdit.TabIndex = 2;
            // 
            // DescriptionTextEdit
            // 
            this.DescriptionTextEdit.Location = new System.Drawing.Point(101, 84);
            this.DescriptionTextEdit.MenuManager = this.barManager1;
            this.DescriptionTextEdit.Name = "DescriptionTextEdit";
            this.DescriptionTextEdit.Size = new System.Drawing.Size(549, 28);
            this.DescriptionTextEdit.StyleController = this.dataLayoutControl1;
            this.DescriptionTextEdit.TabIndex = 3;
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup1,
            this.layoutControlItem1});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(666, 621);
            this.Root.TextVisible = false;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.AllowDrawBackground = false;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.ItemForName,
            this.ItemForDataType,
            this.ItemForDescription});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "autoGeneratedGroup0";
            this.layoutControlGroup1.Size = new System.Drawing.Size(640, 102);
            // 
            // ItemForName
            // 
            this.ItemForName.Control = this.NameTextEdit;
            this.ItemForName.Location = new System.Drawing.Point(0, 0);
            this.ItemForName.Name = "ItemForName";
            this.ItemForName.Size = new System.Drawing.Size(640, 34);
            this.ItemForName.Text = "Tên thuộc tính";
            this.ItemForName.TextSize = new System.Drawing.Size(69, 13);
            // 
            // ItemForDataType
            // 
            this.ItemForDataType.Control = this.DataTypeTextEdit;
            this.ItemForDataType.Location = new System.Drawing.Point(0, 34);
            this.ItemForDataType.Name = "ItemForDataType";
            this.ItemForDataType.Size = new System.Drawing.Size(640, 34);
            this.ItemForDataType.Text = "Kiểu dữ liệu";
            this.ItemForDataType.TextSize = new System.Drawing.Size(69, 13);
            // 
            // ItemForDescription
            // 
            this.ItemForDescription.Control = this.DescriptionTextEdit;
            this.ItemForDescription.Location = new System.Drawing.Point(0, 68);
            this.ItemForDescription.Name = "ItemForDescription";
            this.ItemForDescription.Size = new System.Drawing.Size(640, 34);
            this.ItemForDescription.Text = "Mô tả";
            this.ItemForDescription.TextSize = new System.Drawing.Size(69, 13);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.AttributeGridControl;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 102);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(640, 493);
            this.layoutControlItem1.TextVisible = false;
            // 
            // FrmAttribute
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
            this.Name = "FrmAttribute";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DANH SÁCH CÁC THUỘC TÍNH";
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).EndInit();
            this.dataLayoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.AttributeGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.attributeDtoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AttributeGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NameTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataTypeTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DescriptionTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForDataType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForDescription)).EndInit();
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
        private TextEdit NameTextEdit;
        private TextEdit DataTypeTextEdit;
        private TextEdit DescriptionTextEdit;
        private LayoutControlGroup layoutControlGroup1;
        private LayoutControlItem ItemForName;
        private LayoutControlItem ItemForDataType;
        private LayoutControlItem ItemForDescription;
        private LayoutControlItem layoutControlItem1;
        private BindingSource attributeDtoBindingSource;
        private GridColumn colId1;
        private GridColumn colName;
        private GridColumn colDataType;
        private GridColumn colDescription1;
        private RepositoryItemMemoEdit repositoryItemMemoEdit1;
        private BarButtonItem RefreshBarButtonItem;
    }
}
